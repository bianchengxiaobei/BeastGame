using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using Game;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapNodeBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：地图格子Mono表现类
//----------------------------------------------------------------*/
#endregion
public class MapNodeBehaviour : MonoBehaviour
{
	#region 字段
    private const string BCBonePath = "bcpoint";
    private Animation m_anim;
    private bool m_bAffect = false;
    private bool m_bHighlight = false;
    private bool m_bMouseIn = false;
    private MapNodeBuilding m_mapNodeBuilding = null;
    private Renderer[] m_renderers = null;
    private Transform m_cacheTransform = null;
    private Collider m_cacheCollider = null;
    private Color m_InitOutlineColor = Color.black;
    /// <summary>
    /// 提高多少倍亮度
    /// </summary>
    private readonly float m_HighLightAddValue = 0.3f;
    /// <summary>
    /// 高亮renderer列表
    /// </summary>
    private List<Renderer> m_listMeshRender = new List<Renderer>();
    private IXLog m_log = XLog.GetLog<MapNodeBuilding>();
	#endregion
	#region 属性
    public MapNodeBuilding MapNodeBuilding
    {
        get
        {
            return this.m_mapNodeBuilding;
        }
        set
        {
            this.m_mapNodeBuilding = value;
        }
    }
    public bool IsAffect
    {
        get
        {
            return this.m_bAffect;
        }
        set
        {
            this.m_bAffect = value;
        }
    }
    public bool IsHighlight
    {
        get
        {
            return this.m_bHighlight;
        }
        set
        {
            this.m_bHighlight = value;
            //如果关闭或开启高亮功能，随带这也关或开碰撞器
            if (null != this.m_cacheCollider)
            {
                this.m_cacheCollider.enabled = this.m_bHighlight;
            }
        }
    }
    public Transform CachedTransform
    {
        get
        {
            if (null == this.m_cacheTransform)
            {
                this.m_cacheTransform = base.transform;
            }
            return this.m_cacheTransform;
        }
    }
    /// <summary>
    /// 是否鼠标悬停在上方
    /// </summary>
    public bool IsMouseIn
    {
        get
        {
            return this.m_bMouseIn;
        }
        set
        {
            if (this.m_bMouseIn != value)
            {
                this.m_bMouseIn = value;
                //如果鼠标hover就高亮显示
                this.Highlight(this.m_bMouseIn);
                this.ShowBaseTips(this.m_bMouseIn);
            }
        }
    }
    public bool IsCollider
    {
        set
        {
            if (null != this.m_cacheCollider)
            {
                this.m_cacheCollider.enabled = value;
            }
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="strAnimName"></param>
    public void PlayAnim(string strAnimName)
    {
        if (null != this.m_anim && !string.IsNullOrEmpty(strAnimName))
        {
            if (null != this.m_anim[strAnimName])
            {
                this.m_anim.Play(strAnimName);
            }
        }
    }
    /// <summary>
    /// 停止播放动画
    /// </summary>
    /// <param name="strAnimName"></param>
    public void StopAnim(string strAnimName)
    {
        if (null != this.m_anim && !string.IsNullOrEmpty(strAnimName))
        {
            if (null != this.m_anim[strAnimName])
            {
                this.m_anim.Stop(strAnimName);
            }
        }
    }
    /// <summary>
    /// 取得格子正方向（forward：Vector3(0,0,1)）
    /// </summary>
    /// <param name="vDir"></param>
    public void GetBcPointDir(ref Vector3 vDir)
    {
        if (this.m_cacheTransform != null)
        {
            Transform transform = this.m_cacheTransform.Find("bcpoint");
            if (transform != null)
            {
                vDir = transform.forward;
            }
        }
    }
    /// <summary>
    /// 获取格子的位置
    /// </summary>
    /// <param name="vPos"></param>
    public void GetBcPoint(ref Vector3 vPos)
    {
        if (this.m_cacheTransform != null)
        {
            Transform transform = this.m_cacheTransform.Find("bcpoint");
            if (transform != null)
            {
                vPos = transform.position;
            }
        }
    }
    /// <summary>
    /// 设置格子边界
    /// </summary>
    /// <param name="enable"></param>
    public void SetDoorHighLight(bool enable)
    {
        Renderer[] renderers = this.m_renderers;
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            if (null != renderer.material)
            {
                if (enable)
                {
                    renderer.material.shader = Shader.Find("TSF/BaseOutline1");
                }
                else
                {
                    renderer.material.shader = Shader.Find("TSF/Base1");
                }
            }
        }
    }
    /// <summary>
    /// 获取高亮格子的renderer的缓存
    /// </summary>
    public void SetHighLightData()
    {
        this.m_listMeshRender.Clear();
        HighLightNode[] componentsInChildren = base.gameObject.GetComponentsInChildren<HighLightNode>();
        HighLightNode[] array = componentsInChildren;
        for (int i = 0; i < array.Length; i++)
        {
            HighLightNode highLightNode = array[i];
            Renderer component = highLightNode.gameObject.GetComponent<Renderer>();
            if (null != component)
            {
                this.m_listMeshRender.Add(component);
            }
        }
        if (this.m_listMeshRender.Count > 0 && null != this.m_listMeshRender[0].material)
        {
            this.m_InitOutlineColor = this.m_listMeshRender[0].material.GetColor("_OutlineColor");
        }
    }
    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="nLayer"></param>
    public void SetLayer(int nLayer)
    {
        if (null != this.m_cacheTransform)
        {
            UnityTools.SetLayerRecursively(this.m_cacheTransform.gameObject, nLayer);
        }
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 高亮显示
    /// </summary>
    /// <param name="bTrue"></param>
    private void Highlight(bool bTrue)
    {
        foreach (Renderer current in this.m_listMeshRender)
        {
            if (null != current.material)
            {
                //亮度
                float @float = current.material.GetFloat("_Brightness");
                if (bTrue)
                {
                    current.material.SetFloat("_Brightness", @float * (1f + this.m_HighLightAddValue));
                    current.material.SetColor("_OutlineColor", new Color(0.5882353f, 0f, 0f));
                }
                else
                {
                    current.material.SetFloat("_Brightness", @float / (1f + this.m_HighLightAddValue));
                    current.material.SetColor("_OutlineColor", this.m_InitOutlineColor);
                }
            }
        }
    }
    /// <summary>
    /// 是否显示格子提示信息
    /// </summary>
    /// <param name="bTrue"></param>
    private void ShowBaseTips(bool bTrue)
    {
        if (bTrue)
        {
            List<CVector3> nodesByType = Singleton<ClientMain>.singleton.scene.GetNodesByType(this.m_mapNodeBuilding.MapNodeType);
            if (nodesByType.Count > 0)
            {
                //DlgBase<DlgTerrainTips, DlgTerrainTipsBehaviour>.singleton.Show(nodesByType[0]);
            }
        }
        else
        {
            //DlgBase<DlgTerrainTips, DlgTerrainTipsBehaviour>.singleton.SetVisible(false);
        }
    }

    private void Awake()
    {
        this.m_cacheTransform = base.transform;
        this.m_renderers = base.GetComponentsInChildren<Renderer>();
        this.m_cacheCollider = base.GetComponent<Collider>();
        this.m_anim = base.GetComponentInChildren<Animation>();
    }
    private void Start()
    {
        if (null != this.m_mapNodeBuilding)
        {
            this.m_mapNodeBuilding.Init(this);
        }
        if (null != this.m_anim)
        {
            if (null != this.m_anim["idle"])
            {
                this.m_anim["idle"].layer = 0;
                this.m_anim.Play("idle");
            }
            if (null != this.m_anim["open"])
            {
                this.m_anim["open"].layer = 1;
            }
            if (null != this.m_anim["close"])
            {
                this.m_anim["close"].layer = 1;
            }
            if (null != this.m_anim["attack"])
            {
                this.m_anim["attack"].layer = 1;
            }
        }
    }
    private void OnClick()
    {
        Debug.Log("MapNodeClick");
        if (this.m_mapNodeBuilding != null)
        {
            Debug.Log("BuildVect:" + this.m_mapNodeBuilding.HexPos);
            Singleton<InputManager>.singleton.OnMapClick(this.m_mapNodeBuilding.HexPos);
        }
    }
	#endregion
}
