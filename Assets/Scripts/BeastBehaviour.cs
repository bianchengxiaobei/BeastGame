using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using Game;
using GameClient.Audio;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：HeroBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.9
// 模块描述：神兽mono行为类
//----------------------------------------------------------------*/
#endregion
public class BeastBehaviour : MonoBehaviour
{
	#region 字段
    private static Shader shader_inTree_streamer_character = Shader.Find("Character/InTree_Streamer");
    private static Shader shader_streamer_character = Shader.Find("Character/Streamer");
    private static Shader shader_inTree_normal_character = Shader.Find("Character/InTree_Normal");
    private static Shader shader_normal_character = Shader.Find("Character/Normal");
    private IXLog m_log = XLog.GetLog<BeastBehaviour>();
    private Transform m_cachedTransform = null;
    private GameObject m_cachedGameObject = null;
    private GameObject m_gameObjectShadow = null;//阴影物体
    private CharacterController m_characterController = null;
    private SkinnedMeshRenderer m_skinnedMeshRender = null;
    private Animation m_anim;
    private AudioSource m_audioSource = null;
    private AudioOneShotPlay m_CurrentVoice = null;
    private Transform m_BodyTrans;
    private Transform m_LeftHandTrans;
    private Transform m_RightHandTrans;
    private Transform m_LeftSpecialTrans;
    private Transform m_RightSpecialTrans;
    /// <summary>
    /// bcpoint节点
    /// </summary>
    private Transform m_OtherSpecialTrans;
    private Color m_colorInnerColorCached = Color.black;
    private Color m_colorRimColorCached = Color.black;
    /// <summary>
    /// 普通高亮颜色缓存
    /// </summary>
    private Color m_colorOutlineColorCached = Color.black;
    /// <summary>
    /// 普通高亮系数缓存
    /// </summary>
    private float m_fBrightColorFactorCached = 0f;
    private Vector4 m_vector4RimParamCached = Vector4.zero;
    private Vector3 m_posNext = new Vector3(50f, 0f, 50f);
    /// <summary>
    /// 模型移动的上一个坐标，作为缓存
    /// </summary>
    private CVector3 m_vec3HexActionPos = new CVector3();
    private Queue<Vector3> m_queueMovePositions = new Queue<Vector3>();
    private Beast m_beast = null;
    private float m_fMoveSpeed = 4f;
    private float m_fIdle01Moment = 0f;
    private Vector3 m_vec3DistanceDelta;
    private bool m_bHighlight = false;//是否高亮显示
    private bool m_bIsInTree = false;//是否在草丛中
    private bool m_bMoving = false;//是否正在移动
    private bool m_bAffect = false;//在技能范围内
    private bool m_bMouseIn = false;//鼠标在上方
	#endregion
	#region 属性
    public GameObject CachedGameObject
    {
        get
        {
            if (null == this.m_cachedGameObject)
            {
                this.m_cachedGameObject = base.gameObject;
            }
            return this.m_cachedGameObject;
        }
    }
    public Transform CachedTransform
    {
        get
        {
            if (null == this.m_cachedTransform)
            {
                this.m_cachedTransform = base.transform;
            }
            return this.m_cachedTransform;
        }
    }
    public GameObject Shadow
    {
        get { return this.m_gameObjectShadow; }
    }
    /// <summary>
    /// 神兽模型角色控制器所在的高度
    /// </summary>
    public float Height
    {
        get
        {
            float result;
            if (null != this.m_characterController)
            {
                result = this.m_characterController.height * this.CachedTransform.localScale.y;
            }
            else
            {
                result = 1f;
            }
            return result;
        }
    }
    /// <summary>
    /// 移动速度
    /// </summary>
    public float MoveSpeed
    {
        get
        {
            return this.m_fMoveSpeed;
        }
        set
        {
            this.m_fMoveSpeed = value;
        }
    }
    /// <summary>
    /// 神兽模型body的Transform
    /// </summary>
    public Transform BodyTrans
    {
        get
        {
            return this.m_BodyTrans;
        }
    }
    public Transform LeftHandTrans
    {
        get
        {
            return this.m_LeftHandTrans;
        }
    }
    public Transform RightHandTrans
    {
        get
        {
            return this.m_RightHandTrans;
        }
    }
    public Transform LeftSpecialTrans
    {
        get
        {
            return this.m_LeftSpecialTrans;
        }
    }
    public Transform RightSpecialTrans
    {
        get
        {
            return this.m_RightSpecialTrans;
        }
    }
    /// <summary>
    /// bcpoint节点
    /// </summary>
    public Transform OtherSpecialTrans
    {
        get
        {
            return this.m_OtherSpecialTrans;
        }
    }
    /// <summary>
    /// 神兽平面坐标
    /// </summary>
    public Vector2 Forward
    {
        get
        {
            Vector2 zero = Vector2.zero;
            zero.x = this.m_cachedTransform.forward.x;
            zero.y = this.m_cachedTransform.forward.z;
            return zero;
        }
        set
        {
            Vector3 zero = Vector3.zero;
            zero.x = value.x;
            zero.z = value.y;
            this.m_cachedTransform.forward = zero.normalized;
        }
    }
    public Beast Beast
    {
        get
        {
            return this.m_beast;
        }
        set
        {
            this.m_beast = value;
        }
    }
    /// <summary>
    /// 鼠标在神兽上分
    /// </summary>
    public bool IsMouseIn
    {
        get
        {
            return this.m_bMouseIn;
        }
        set
        {
            if (value != this.m_bMouseIn)
            {
                this.m_bMouseIn = value;
            }
        }
    }
    /// <summary>
    /// 是否神兽在播放声音
    /// </summary>
    public bool IsAudioPlaying
    {
        get
        {
            return this.m_CurrentVoice != null && !this.m_CurrentVoice.IsStopped;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 给模型添加材质
    /// </summary>
    /// <param name="material"></param>
    public void AddMaterial(Material material)
    {
        this.m_log.Debug("AddMaterial: " + material);
        if (!(null == this.m_skinnedMeshRender) && !(null == material))
        {
            Shader shader = this.m_bIsInTree ? BeastBehaviour.shader_inTree_streamer_character : BeastBehaviour.shader_streamer_character;
            if (null == shader)
            {
                this.m_log.Error("Shader:Character/Streamer is null");
            }
            else
            {
                this.m_skinnedMeshRender.sharedMaterial.shader = shader;
                if (material.HasProperty("_InnerColor"))
                {
                    this.m_colorInnerColorCached = this.m_skinnedMeshRender.sharedMaterial.GetColor("_InnerColor");
                    this.m_skinnedMeshRender.sharedMaterial.SetColor("_InnerColor", material.GetColor("_InnerColor"));
                }
                if (material.HasProperty("_RimColor"))
                {
                    this.m_colorRimColorCached = this.m_skinnedMeshRender.sharedMaterial.GetColor("_RimColor");
                    this.m_skinnedMeshRender.sharedMaterial.SetColor("_RimColor", material.GetColor("_RimColor"));
                }
                if (material.HasProperty("_RimParam"))
                {
                    this.m_vector4RimParamCached = this.m_skinnedMeshRender.sharedMaterial.GetVector("_RimParam");
                    this.m_skinnedMeshRender.sharedMaterial.SetVector("_RimParam", material.GetVector("_RimParam"));
                }
                if (material.HasProperty("_StreamerTex"))
                {
                    this.m_skinnedMeshRender.sharedMaterial.SetTexture("_StreamerTex", material.GetTexture("_StreamerTex"));
                }
                if (material.HasProperty("_StreamerTransform"))
                {
                    this.m_skinnedMeshRender.sharedMaterial.SetVector("_StreamerTransform", material.GetVector("_StreamerTransform"));
                }
                if (material.HasProperty("_StreamerColorBlend"))
                {
                    this.m_skinnedMeshRender.sharedMaterial.SetVector("_StreamerColorBlend", material.GetVector("_StreamerColorBlend"));
                }
            }
        }
    }
    /// <summary>
    /// 给模型删除材质
    /// </summary>
    /// <param name="material"></param>
    public void DelMaterial(Material material)
    {
        Shader shader = this.m_bIsInTree ? BeastBehaviour.shader_inTree_normal_character : BeastBehaviour.shader_normal_character;
        if (null == shader)
        {
            this.m_log.Error("Shader:Character/Normal == null");
        }
        else
        {
            if (null != this.m_skinnedMeshRender)
            {
                this.m_skinnedMeshRender.sharedMaterial.shader = shader;
                this.m_skinnedMeshRender.sharedMaterial.SetColor("_InnerColor", this.m_colorInnerColorCached);
                this.m_skinnedMeshRender.sharedMaterial.SetColor("_RimColor", this.m_colorRimColorCached);
                this.m_skinnedMeshRender.sharedMaterial.SetVector("_RimParam", this.m_vector4RimParamCached);
            }
        }
    }
    /// <summary>
    /// 设置神兽到这个格子坐标
    /// </summary>
    /// <param name="hexPos"></param>
    public void SetPos(CVector3 hexPos)
    {
        XLog.Log.Debug(string.Concat(new object[]
	    {
		    "Trans to: Row=",
		    hexPos.nRow,
		    " Col=",
		    hexPos.nCol
	    }));
        //根据格子坐标获取世界坐标
        Vector3 hex3DPos = Hexagon.GetHex3DPos(hexPos, Space.World);
        this.SetPos(hex3DPos);
        this.Beast.OnActionLeaveFrom(this.m_vec3HexActionPos);
        this.m_vec3HexActionPos.CopyFrom(hexPos);
        this.Beast.OnActionMoveTo(hexPos);
    }
    /// <summary>
    /// 根据格子路径移动
    /// </summary>
    /// <param name="listVec3Path">离角色越来越近的格子坐标列表</param>
    public void Move(List<CVector3> listVec3Path)
    {
        if (listVec3Path != null)
        {
            for (int i = listVec3Path.Count-1; i >= 0;i--)
            {
                CVector3 cVector = listVec3Path[i];
                Vector2 hexPosByIndex = Hexagon.GetHexPosByIndex(cVector.nRow, cVector.nCol, Space.World);
                Vector3 pos = new Vector3(hexPosByIndex.x, 0f, hexPosByIndex.y);
                this.PushTargetPosition(pos);
            }
            this.m_vec3HexActionPos.CopyFrom(listVec3Path[listVec3Path.Count - 1]);
        }
    }
    /// <summary>
    /// 播放动画(有带加速)
    /// </summary>
    /// <param name="strAnimName"></param>
    /// <param name="playtime"></param>
    public void PlayAnim(string strAnimName,float playtime)
    {
        if (!string.IsNullOrEmpty(strAnimName))
        {
            if (this.m_anim == null)
            {
                this.m_log.Error(string.Format("null == m_anim:{0}", base.name));
            }
            else 
            {
                AnimationState state = this.animation[strAnimName];
                if (state != null)
                {
                    if (playtime > 0f)
                    {
                        state.speed = state.clip.length / playtime;
                    }
                    else 
                    {
                        state.speed = 1;
                    }
                    switch (strAnimName)
                    {
                        case "Idle":
                            state.wrapMode = WrapMode.Loop;
                            this.m_anim.CrossFade(strAnimName);
                            this.CalculateIdle01Moment(0f);
                            break;
                        case "Run":
                            state.wrapMode = WrapMode.Loop;
                            this.m_anim.CrossFade(strAnimName);
                            this.CalculateIdle01Moment(-1f);
                            break;
                        case "Death":
                            state.wrapMode = WrapMode.Once;
							this.m_anim.CrossFade(strAnimName);
							this.CalculateIdle01Moment(-1f);
                            break;
                        default:
                            state.wrapMode = WrapMode.Once;
							this.m_anim.Play(strAnimName);
							this.m_anim.CrossFadeQueued("Idle");
							this.CalculateIdle01Moment((playtime > 0f) ? playtime : state.length);
                            break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 播放动画(不带加速)
    /// </summary>
    /// <param name="strAnimName"></param>
    public void PlayAnim(string strAnimName)
    {
        this.PlayAnim(strAnimName, 0f);
    }
    /// <summary>
    /// 停止播放动画，转到idle动画状态
    /// </summary>
    /// <param name="strAnimName"></param>
    public void StopAnim(string strAnimName)
    {
        if (this.m_anim != null)
        {
            if (!string.IsNullOrEmpty(strAnimName) && this.m_anim[strAnimName] != null)
            {
                this.PlayAnim("Idle");
            }
        }
    }
    /// <summary>
    /// 播放神兽声音
    /// </summary>
    /// <param name="strAudioFile"></param>
    public void PlayVoice(string strAudioFile)
    {
        if (this.m_CurrentVoice != null && !this.m_CurrentVoice.IsStopped)
        {
            this.m_CurrentVoice.Stop();
            this.m_CurrentVoice = null;
        }
        float volumeBeastVoice = Singleton<AudioManager>.singleton.VolumeBeastVoice;
        this.m_CurrentVoice = Singleton<AudioManager>.singleton.PlayAudioOneShot(strAudioFile, volumeBeastVoice, null);
    }
    /// <summary>
    /// 重生,播放idle动画
    /// </summary>
    public void OnRevive()
    {
        if (this.m_anim != null)
        {
            Debug.Log("Play Idle");
            this.PlayAnim("Idle");
        }
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 设置神兽模型坐标，根据格子坐标的xz，获取格子碰撞器的y，然后向上偏移一点点坐标
    /// </summary>
    /// <param name="pos"></param>
    private void SetPos(Vector3 pos)
    {
        int layerMask = 1 << UnityEngine.LayerMask.NameToLayer("HexagonMap");
        RaycastHit raycastHit;
        if (Physics.Raycast(new Vector3(pos.x, 100f, pos.z), Vector3.down, out raycastHit, 150f, layerMask))
        {
            pos.y = raycastHit.point.y;
        }
        pos.y += 0.05f;
        this.m_cachedTransform.position = pos;
    }
    /// <summary>
    /// 计算idle是不同的动画
    /// </summary>
    /// <param name="fDelay"></param>
    private void CalculateIdle01Moment(float fDelay)
    {
        if (fDelay < 0f)
        {
            this.m_fIdle01Moment = fDelay;
        }
        else
        {
            this.m_fIdle01Moment = Time.time + fDelay + (float)Random.Range(4, 8);
        }
    }
    /// <summary>
    /// 从队列内取目标路径
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool PopTargetPosition(ref Vector3 pos)
    {
        bool result;
        if (this.m_queueMovePositions.Count == 0)
        {
            result = false;
        }
        else 
        {
            pos = this.m_queueMovePositions.Dequeue();
            result = true;
        }
        return result;
    }
    /// <summary>
    /// 坐标压入队列内
    /// </summary>
    /// <param name="pos"></param>
    private void PushTargetPosition(Vector3 pos)
    {
        this.m_queueMovePositions.Enqueue(pos);
    }
    /// <summary>
    /// 寻找绑定节点
    /// </summary>
    /// <param name="transRoot">根节点</param>
    /// <param name="strBoneName">绑定节点名称</param>
    /// <param name="transBone"></param>
    /// <returns></returns>
    private bool FindBindBone(Transform transRoot, string strBoneName, ref Transform transBone)
    {
        bool result;
        if (null == transRoot || string.IsNullOrEmpty(strBoneName))
        {
            this.m_log.Error("null == transRoot || string.IsNullOrEmpty(strBoneName) == true");
            result = false;
        }
        else
        {
            //根节点的名称必须在开头包含bcpoint字样
            string a = transRoot.name.Split(new char[]
		    {
			    '/'
		    })[0];
            if (a == strBoneName)
            {
                transBone = transRoot;
                result = true;
            }
            else
            {
                int childCount = transRoot.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    if (this.FindBindBone(transRoot.GetChild(i), strBoneName, ref transBone))
                    {
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
        }
        return result;
    }
   /// <summary>
   /// 人物是否高亮显示（改变外边框颜色明暗）
   /// </summary>
   /// <param name="bTrue"></param>
    private void Highlight(bool bTrue)
    {
        if (this.m_skinnedMeshRender != null)
        {
            if (this.m_bHighlight != bTrue)
            {
                this.m_bHighlight = bTrue;
                if (this.m_bHighlight)
                {
                    this.m_fBrightColorFactorCached = this.m_skinnedMeshRender.material.GetFloat("_BrightColorFactor");
                    this.m_colorOutlineColorCached = this.m_skinnedMeshRender.material.GetColor("_OutlineColor");
                    this.m_skinnedMeshRender.material.SetFloat("_BrightColorFactor", 1.5f);
                    this.m_skinnedMeshRender.material.SetColor("_OutlineColor", new Color(0f, 0.78f, 1f, 1f));
                }
                else
                {
                    this.m_skinnedMeshRender.material.SetFloat("_BrightColorFactor", this.m_fBrightColorFactorCached);
                    this.m_skinnedMeshRender.material.SetColor("_OutlineColor", this.m_colorOutlineColorCached);
                }
            }
        }
    }
    private void Awake()
    {
        this.m_cachedTransform = base.transform;
        this.m_cachedGameObject = base.gameObject;
        this.m_characterController = base.GetComponent<CharacterController>();
        this.m_skinnedMeshRender = this.m_cachedTransform.GetComponentInChildren<SkinnedMeshRenderer>();
        if (null == this.m_characterController)
        {
            this.m_log.Error(string.Format("null == m_collider:{0}", this.m_cachedGameObject.name));
        }
        if (!this.FindBindBone(this.m_cachedTransform, "Bip01", ref this.m_OtherSpecialTrans))
        {
            this.m_log.Error("false == FindBindBone(m_cachedTransform, bcpoint, ref m_OtherSpecialTrans):" + base.name);
        }
        if (!this.FindBindBone(this.m_cachedTransform, "Middlepoint_L", ref this.m_LeftSpecialTrans))
        {
        }
        if (!this.FindBindBone(this.m_cachedTransform, "Middlepoint_R", ref this.m_RightSpecialTrans))
        {
        }
        if (!this.FindBindBone(this.m_cachedTransform, "Bip01 Spine", ref this.m_BodyTrans))
        {
            this.m_log.Error("false == FindBindBone(m_cachedTransform, Bip01 Spine, ref m_BodyTrans):" + base.name);
        }
        if (!this.FindBindBone(this.m_cachedTransform, "Bip01 L Hand", ref this.m_LeftHandTrans))
        {
            this.m_log.Error("false == FindBindBone(m_cachedTransform, Bip01 L Hand, ref m_LeftHandTrans):" + base.name);
        }
        if (!this.FindBindBone(this.m_cachedTransform, "Bip01 R Hand", ref this.m_RightHandTrans))
        {
            this.m_log.Error("false == FindBindBone(m_cachedTransform, Bip01 R Hand, ref m_RightHandTrans):" + base.name);
        }
        this.m_anim = base.GetComponentInChildren<Animation>();
        if (null == this.m_anim)
        {
            this.m_log.Fatal("null == m_anim:" + base.gameObject.name);
        }
        else 
        {
            this.m_anim.Play("Idle");
        }
        //神兽节点下有Shadow节点
        Transform transform = this.m_cachedTransform.Find("Shadow");
        if (null != transform)
        {
            this.m_gameObjectShadow = transform.gameObject;
            this.m_gameObjectShadow.SetActive(false);
        }
        this.m_audioSource = this.m_cachedGameObject.AddComponent<AudioSource>();
        CapsuleCollider capsuleCollider = base.gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = 0.1f;
        capsuleCollider.center = this.m_characterController.center;
        base.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        if (null != base.renderer)
        {
            base.renderer.sharedMaterial.renderQueue = 2000;
        }
    }
    private void Start()
    {
        if (this.m_beast != null)
        {
            this.SetPos(this.m_beast.RealPos3D);
        }
    }
    private void Update()
    {
        if (this.m_fIdle01Moment > 0f && Time.time > this.m_fIdle01Moment)
        {
            this.PlayAnim("Idle2");
        }
        //如果没有在移动的话
        if (!this.m_bMoving)
        {
            if (this.PopTargetPosition(ref this.m_posNext))
            {
                this.m_bMoving = true;
                this.PlayAnim("Run");
            }
        }
        if (this.m_bMoving)
        {
            this.m_vec3DistanceDelta = this.m_posNext - this.m_cachedTransform.position;
            this.m_vec3DistanceDelta.y = 0f;
            while (this.m_vec3DistanceDelta.magnitude < 0.01f)
            {
                this.SetPos(this.m_posNext);
                if (!this.PopTargetPosition(ref this.m_posNext))
                {
                    this.m_bMoving = false;
                    this.StopAnim("Run");//转成idle
                    int nRow = 0;
                    int nCol = 0;
                    Hexagon.GetHexIndexByPos(this.Beast.MovingPos,out nRow,out nCol);
                    CVector3 cVector = new CVector3();
                    cVector.nRow = nRow;
                    cVector.nCol = nCol;
                    this.m_vec3HexActionPos.CopyFrom(cVector);
                    break;
                }
                Vector3 delta = this.m_posNext - this.m_cachedTransform.position;
                this.Forward = new Vector2(delta.x, delta.z);
                this.m_vec3DistanceDelta = this.m_posNext - this.m_cachedTransform.position;

            }
            if (this.m_bMoving)
            {
                Vector3 b = this.m_vec3DistanceDelta.normalized * this.m_fMoveSpeed * Time.deltaTime;
                if (b.magnitude >= this.m_vec3DistanceDelta.magnitude)
                {
                    b = this.m_vec3DistanceDelta;
                }
                this.SetPos(this.m_cachedTransform.position + b);
            }
        }
        if (this.m_bAffect || this.IsMouseIn)
        {
            //就高亮显示
            this.Highlight(true);
        }
        else 
        {
            this.Highlight(false);
        }
    }
	#endregion
}
