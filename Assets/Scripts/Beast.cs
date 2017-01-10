using UnityEngine;
using System.Collections.Generic;
using System;
using Game;
using Utility.Export;
using Utility;
using Client.Data;
using Client.Common;
using GameClient.Audio;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Hero
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：神兽类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽类
/// </summary>
public class Beast : IBeast, IDisposable
{
    #region 字段
    private IXLog m_log = XLog.GetLog<Beast>();
    private const string m_BCName = "";
    private bool m_bHideHeadInfo = false;
    private int m_nPassiveSkillTEft = 0;
    private long m_unPlayerId = 0;
    private long m_unBeastId = 0;
    private GameObject m_beastModelObj = null;
    private GameObject m_beastChildObj = null;
    private int m_dwBeastTypeId = 0;
    //现在模型所在的格子坐标
    private CVector3 m_vec3HexPos = new CVector3();
    private BeastBehaviour m_beastBehaviour = null;
    private AvatarTool m_avatar = new AvatarTool();
    private SkillGameManager m_skillManager = null;
    private List<int> m_listTargetHeroSkillID = new List<int>();
    private List<int> m_listTargetHeroEquipID = new List<int>();
    private List<int> m_listBeastOriginActivedSkillID = new List<int>();
    private EClientRoleStage m_eRoleStage = EClientRoleStage.ROLE_STAGE_INVALID;
    private ECampType m_eCampType = ECampType.CAMP_INVALID;
    private int m_unHp;             //生命值
    private int m_unHpMax = 10;     //最大生命值
    private int m_unMaxMoveDis = 1;//最大移动距离
    private int m_unMoney = 0;     //金币
    private int m_nMaxAttackDis = 1;//最大攻击距离
    private int m_unSlowDownValue = 0;//减速值
    private int m_unSpeedUpValue = 0;//加速值
    private int m_suitId = 0;
    private bool m_bDead = false;
    private bool m_bEverDead = false;//是否曾经死过
    private bool m_bModelLoaded = false;
    private bool m_bVisible = false;//是否模型可见
    private bool m_bInRound = false;
    private bool m_bToPlayVoiceInRound = false;//是否轮到播放声音
    private bool m_bToPlayVoiceOutRound = false;//是否没有轮到就播放声音
    private float m_fvoiceOutRoundMoment = 0f;///没有轮到随机播放声音的值
    private float m_fVoiceInRoundMoment = 0f;
    private bool m_bToPlayRoundStartVoice = true;//开始随机播放声音
    private bool m_bIsErrorBeast = true;
    private bool m_bDisposed = false;
    private long m_unTargetBeastId = 0u;
    private int m_unStatus = 0;
    private int m_unBuff = 0;
    #endregion
    #region 属性
    /// <summary>
    /// 神兽id
    /// </summary>
    public long Id
    {
        get
        {
            return this.m_unBeastId;
        }
        set
        {
            this.m_unBeastId = value;
        }
    }
    /// <summary>
    /// 神兽类型id
    /// </summary>
    public int BeastTypeId
    {
        get
        {
            return this.m_dwBeastTypeId;
        }
        set
        {
            this.m_dwBeastTypeId = value;
            //DataBeastlist dataById = DataBeastlistManager.Instance.GetDataById((int)this.m_dwBeastTypeId);
            DataBeastlist dataById = GameData<DataBeastlist>.dataMap[this.m_dwBeastTypeId];
            if (null != dataById)
            {
                this.m_unHpMax = dataById.Hp;
                this.m_unMaxMoveDis = dataById.Move;
            }
        }
    }
    /// <summary>
    /// 神兽模型GameObejct
    /// </summary>
    public GameObject Object
    {
        get
        {
            return this.m_beastModelObj;
        }
    }
    /// <summary>
    /// 构造神兽时数据出错
    /// </summary>
    public bool IsError
    {
        get { return this.m_bIsErrorBeast; }
    }
    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsVisible
    {
        get { return this.m_bVisible; }
    }
    /// <summary>
    /// 是否是自身神兽角色
    /// </summary>
    public bool Role
    {
        get
        {
            return Singleton<BeastRole>.singleton.Id == this.m_unBeastId;
        }
    }
    /// <summary>
    /// 拥有该神兽的玩家
    /// </summary>
    public Player Player
    {
        get
        {
            Player playerByID = Singleton<PlayerManager>.singleton.GetPlayerByID(this.m_unPlayerId);
            Player result;
            if (null != playerByID)
            {
                result = playerByID;
            }
            else
            {
                result = PlayerManager.ErrorPlayer;
            }
            return result;
        }
    }
    /// <summary>
    /// 玩家id
    /// </summary>
    public long PlayerId
    {
        get
        {
            return this.m_unPlayerId;
        }
    }
    /// <summary>
    /// 当前神兽所在的操作阶段
    /// </summary>
    public EClientRoleStage eRoleStage
    {
        get
        {
            return this.m_eRoleStage;
        }
        set
        {
            this.m_eRoleStage = value;
        }
    }
    /// <summary>
    /// 当前所在的阵营
    /// </summary>
    public ECampType eCampType
    {
        get
        {
            return this.m_eCampType;
        }
        set
        {
            this.m_eCampType = value;
        }
    }
    /// <summary>
    /// 是否模型可见
    /// </summary>
    public bool IsModelVisible
    {
        get
        {
            return !this.m_bIsErrorBeast && null != this.m_beastModelObj && this.m_bVisible;
        }
    }
    /// <summary>
    /// 真实格子所在3d坐标
    /// </summary>
    public Vector3 RealPos3D
    {
        get
        {
            return Hexagon.GetHex3DPosByIndex(this.m_vec3HexPos.nRow, this.m_vec3HexPos.nCol, Space.World);
        }
    }
    /// <summary>
    /// 真实格子所在2d坐标
    /// </summary>
    public Vector2 RealPos2D
    {
        get
        {
            return Hexagon.GetHexPosByIndex(this.m_vec3HexPos.nRow, this.m_vec3HexPos.nCol, Space.World);
        }
    }
    /// <summary>
    /// 真实世界人物所在坐标
    /// </summary>
    public Vector3 MovingPos
    {
        get
        {
            Vector3 result;
            if (this.m_beastBehaviour != null)
            {
                result = this.m_beastBehaviour.CachedTransform.position;
            }
            else
            {
                result = Vector3.zero;
            }
            return result;
        }
    }
    /// <summary>
    /// 神兽正方向
    /// </summary>
    public Vector2 Forward
    {
        get
        {
            Vector2 result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.Forward;
            }
            else
            {
                result = Vector2.zero;
            }
            return result;
        }
        set
        {
            if (null != this.m_beastBehaviour)
            {
                this.m_beastBehaviour.Forward = value;
            }
        }
    }
    /// <summary>
    /// 所在格子坐标
    /// </summary>
    public CVector3 Pos
    {
        get { return this.m_vec3HexPos; }
    }
    /// <summary>
    /// 玩家战斗顺序
    /// </summary>
    public int Order
    {
        get;
        set;
    }
    /// <summary>
    /// 神兽的高度
    /// </summary>
    public float Height
    {
        get
        {
            float result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.Height;
            }
            else
            {
                result = 1f;
            }
            return result;
        }
    }

    /// <summary>
    /// 神兽身体所在的Transform
    /// </summary>
    public Transform Body
    {
        get
        {
            Transform result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.BodyTrans;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    public Transform LeftHand
    {
        get
        {
            Transform result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.LeftHandTrans;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    public Transform RightHand
    {
        get
        {
            Transform result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.RightHandTrans;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    public Transform LeftSpecialTrans
    {
        get
        {
            Transform result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.LeftSpecialTrans;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    public Transform RightSpecialTrans
    {
        get
        {
            Transform result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.RightSpecialTrans;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    public Transform OtherSpecialTrans
    {
        get
        {
            Transform result;
            if (null != this.m_beastBehaviour)
            {
                result = this.m_beastBehaviour.OtherSpecialTrans;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
    /// <summary>
    /// 神兽血量
    /// </summary>
    public int Hp
    {
        get
        {
            return this.m_unHp;
        }
        set
        {
            this.m_unHp = value;
        }
    }
    /// <summary>
    /// 神兽血量最大值
    /// </summary>
    public int HpMax
    {
        get
        {
            return this.m_unHpMax;
        }
        set
        {
            this.m_unHpMax = value;
        }
    }
    /// <summary>
    /// 攻击目标神兽的id
    /// </summary>
    public long TargetBeastId
    {
        get
        {
            return this.m_unTargetBeastId;
        }
        set
        {
            this.m_unTargetBeastId = value;
        }
    }
    /// <summary>
    /// 最大攻击距离
    /// </summary>
    public int MaxAttackDis
    {
        get
        {
            return this.m_nMaxAttackDis;
        }
        set
        {
            this.m_nMaxAttackDis = value;
        }
    }
    /// <summary>
    /// 已经攻击的次数
    /// </summary>
    /// <returns></returns>
    public int UsedAttackToBaseBuildingCount
    {
        get
        {
            return this.m_skillManager.UseAttackToBaseBuildingCount;
        }
    }
    /// <summary>
    /// 最大移动距离
    /// </summary>
    public int MaxMoveDis
    {
        get
        {
            int result;
            //如果在禁锢和眩晕状态下，最大移动为0
            if (this.IsInStatus(ERoleStatus.ROLE_STATUS_RESTRICTION) || this.IsInStatus(ERoleStatus.ROLE_STATUS_VERTIGO))
            {
                result = 0;
            }
            else
            {
                //是否有加速减速
                int num = this.m_unMaxMoveDis;
                num -= this.m_unSlowDownValue;
                num += this.m_unSpeedUpValue;
                result = ((num > 0) ? num : 0);
            }
            return result;
        }
        set
        {
            if (this.m_unMaxMoveDis != value)
            {
                this.m_unMaxMoveDis = value;
                this.OnChangeMaxMoveDis(this.m_unMaxMoveDis);
            }
        }
    }
    /// <summary>
    /// 神兽拥有的金币
    /// </summary>
    public int Money
    {
        get { return this.m_unMoney; }
    }
    /// <summary>
    /// 神兽拥有的技能
    /// </summary>
    public List<SkillGameData> Skills
    {
        get
        {
            return this.m_skillManager.Skills;
        }
    }
    /// <summary>
    /// 是否死亡
    /// </summary>
    public bool IsDead
    {
        get
        {
            return this.m_bDead;
        }
    }
    /// <summary>
    /// 是否曾经死过
    /// </summary>
    public bool IsEverDead
    {
        get
        {
            return this.m_bEverDead;
        }
    }
    /// <summary>
    /// 是否已经移动了
    /// </summary>
    public bool IsMoved
    {
        get;
        set;
    }
    /// <summary>
    /// 是否神兽在播放声音
    /// </summary>
    public bool IsAudioPlaying
    {
        get
        {
            return null != this.m_beastBehaviour && this.m_beastBehaviour.IsAudioPlaying;
        }
    }
    /// <summary>
    /// 该神兽播放声音是否能被打断
    /// </summary>
    public bool IsAudioCanBeInterrupted
    {
        get;
        private set;
    }
    #endregion
    #region 构造方法
    public Beast(BeastData beastData)
    {
        if (null == beastData)
        {
            this.m_bIsErrorBeast = true;
            this.m_skillManager = new SkillGameManager(0);
        }
        else
        {
            this.m_bIsErrorBeast = false;
            DataBeastlist dataBeast = null;
            GameData<DataBeastlist>.dataMap.TryGetValue(beastData.BeastTypeId, out dataBeast);
            if (dataBeast != null)
            {
                this.MaxMoveDis = dataBeast.Move;
                this.Hp = dataBeast.Hp;
                if (dataBeast.Skill_1 > 0)
                {
                    this.m_listBeastOriginActivedSkillID.Add(dataBeast.Skill_1);
                }
                if (dataBeast.Skill_2 > 0)
                {
                    this.m_listBeastOriginActivedSkillID.Add(dataBeast.Skill_2);
                }
                if (dataBeast.Skill_3 > 0)
                {
                    this.m_listBeastOriginActivedSkillID.Add(dataBeast.Skill_3);
                }
                if (dataBeast.Skill_4 > 0)
                {
                    this.m_listBeastOriginActivedSkillID.Add(dataBeast.Skill_4);
                }
            }
            else
            {
                this.m_log.Error(string.Format("null == beastConfig: beastData.BeastTypeId=:{0}", beastData.BeastTypeId));
            }
            this.m_unBeastId = beastData.Id;
            this.m_unPlayerId = beastData.PlayerId;
            this.m_dwBeastTypeId = beastData.BeastTypeId;
            this.m_eCampType = beastData.CampType;
            this.m_unHp = beastData.Hp;
            this.m_suitId = beastData.SuitId;
            this.m_skillManager = new SkillGameManager(this.m_unBeastId);
            this.m_skillManager.AddSkill(1, false);
            foreach (var skillData in beastData.Skills)
            {
                this.m_skillManager.AddSkill(skillData, false, ESkillActivateType.SKILL_ACTIVATE_TYPE_INVALID);
            }
        }
    }
    #endregion
    #region 公有方法
    public void AddMaterial(Material material)
    {
        if (null != this.m_beastBehaviour)
        {
            this.m_beastBehaviour.AddMaterial(material);
        }
    }
    public void DelMaterial(Material material)
    {
        if (null != this.m_beastBehaviour)
        {
            this.m_beastBehaviour.DelMaterial(material);
        }
    }
    /// <summary>
    /// 创建模型
    /// </summary>
    public void ChangeModel()
    {
        this.m_avatar.Dowork(this.BeastTypeId, this.m_suitId, new Action<GameObject>(this.OnAvatarFinished));
    }
    /// <summary>
    /// 播放神兽声音
    /// </summary>
    /// <param name="strAudioFile"></param>
    /// <param name="bCanBeInterrupted"></param>
    public void PlayVoice(string strAudioFile, bool bCanBeInterrupted)
    {
        if (null != this.m_beastBehaviour)
        {
            if (!this.m_beastBehaviour.IsAudioPlaying || this.IsAudioCanBeInterrupted)
            {
                this.m_beastBehaviour.PlayVoice(strAudioFile);
                this.IsAudioCanBeInterrupted = bCanBeInterrupted;
            }
        }
    }
    /// <summary>
    /// 神兽激活技能
    /// </summary>
    public void ActiveSkills()
    {
        this.m_skillManager.ActiveSkills();
    }
    /// <summary>
    /// 神兽移动到该格子坐标
    /// </summary>
    /// <param name="vec3DestPos"></param>
    public void Move(CVector3 vec3DestPos)
    {
        this.OnLeaveFrom(this.m_vec3HexPos);
        this.m_vec3HexPos.nCol = vec3DestPos.nCol;
        this.m_vec3HexPos.nRow = vec3DestPos.nRow;
        this.OnMoveTo(this.m_vec3HexPos);
        //if (!Singleton<SequenceShowManager>.singleton.CanRecevieMsg())
        //{
        this.MoveAction(vec3DestPos);
        //}
    }
    public void Move(List<CVector3> listPath)
    {
        this.OnLeaveFrom(this.m_vec3HexPos);
        this.m_vec3HexPos.nCol = listPath[0].nCol;
        this.m_vec3HexPos.nRow = listPath[0].nRow;
        this.OnMoveTo(this.m_vec3HexPos);
        this.m_beastBehaviour.Move(listPath);

    }
    public void OnMoveTo(CVector3 hexPos)
    {
       //、this.m_terrainCardManager.OnHeroMoveTo(hexPos);
        //this.SkillManager.OnHeroPosChange(this.Id);
        //如果是自身的神兽
        if (this.Role)
        {
            this.IsMoved = true;
            Singleton<BeastRole>.singleton.OnMove(hexPos);
        }
    }
    /// <summary>
    /// 神兽离开上个格子坐标
    /// </summary>
    /// <param name="hexPos"></param>
    public void OnLeaveFrom(CVector3 hexPos)
    {
    }
    /// <summary>
    /// 神兽复活
    /// </summary>
    public void OnRevive()
    {
        this.m_bDead = false;
        this.SetVisible(true);
        if (this.m_beastBehaviour != null)
        {
            this.m_beastBehaviour.OnRevive();
        }
        if (this.Role)
        {
            Singleton<BeastRole>.singleton.OnRevive();
        }
        if (Singleton<RoomManager>.singleton.BeastRound > 1u)
        {
           
        }
    }
    /// <summary>
    /// 操作开始
    /// </summary>
    public void OnRoundStart()
    {
        this.m_bInRound = true;
        this.m_beastBehaviour.Shadow.SetActive(true);
        if (Role)
        {
            Singleton<BeastRole>.singleton.OnRoundStart();
        }
        if (this.m_bToPlayRoundStartVoice)
        {
            Singleton<BeastVoiceRule>.singleton.PlayVoice(this.m_unBeastId, EBeastVoiceRuleType.BeastRoundStart);
        }
        this.m_bToPlayRoundStartVoice = true;
        this.m_bToPlayVoiceOutRound = false;
        this.m_bToPlayVoiceInRound = true;
        this.m_fVoiceInRoundMoment = Time.time + (float)UnityEngine.Random.Range(10, 15);
    }
    /// <summary>
    /// 战斗阶段结束
    /// </summary>
    public void OnRoundEnd()
    {
        this.m_bInRound = false;
        this.m_beastBehaviour.Shadow.SetActive(false);
    }
    /// <summary>
    /// 出生时回调，主要处理播放出生声音
    /// </summary>
    public void OnBorn()
    {
        bool flag = false;
        if (Singleton<RoomManager>.singleton.MatchType == EMatchtype.MATCH_3V3)
        {
            if (this.Order <= 2)
            {
                flag = true;
            }
        }
        else 
        {
            flag = true;
        }
        if (flag)
        {
            if (Singleton<BeastVoiceRule>.singleton.PlayVoice(this.Id, EBeastVoiceRuleType.BeastBorn))
            {
                if (this.Order == 1)
                {
                    this.m_bToPlayRoundStartVoice = false;
                }
            }
        }
    }
    /// <summary>
    /// 血量改变
    /// </summary>
    /// <param name="hp"></param>
    public void OnHpChange(int hp)
    {
        this.OnHpChange(hp, this.m_unHpMax);
    }
    /// <summary>
    /// 血量改变
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="hpMax"></param>
    public void OnHpChange(int hp, int hpMax)
    {
        this.m_unHp = hp;
        this.m_unHpMax = hpMax;
        if (this.Role)
        {
            Singleton<BeastRole>.singleton.OnHpChange(hp);
        }
        this.m_skillManager.OnBeastHpChange();
        //刷新头像信息
        //刷新tab信息
    }
    /// <summary>
    /// 神兽添加到场景中结束的处理
    /// </summary>
    public void OnAddBeastToSceneFinished()
    {
        this.RefreshAura();
    }
    public void RefreshAura()
    {
        List<EAura> list = new List<EAura>();
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="strAnimName"></param>
    public void PlayAnim(string strAnimName)
    {
        if (null != this.m_beastBehaviour)
        {
            this.m_beastBehaviour.PlayAnim(strAnimName);
        }
    }
    /// <summary>
    /// 停止播放动画，最后停止在idle动画
    /// </summary>
    /// <param name="strAnimName"></param>
    public void StopAnim(string strAnimName)
    {
        if (null != this.m_beastBehaviour)
        {
            this.m_beastBehaviour.StopAnim(strAnimName);
        }
    }
    /// <summary>
    /// 神兽是否在该状态下
    /// </summary>
    /// <param name="eRoleStatus"></param>
    /// <returns></returns>
    public bool IsInStatus(ERoleStatus eRoleStatus)
    {
        return (this.m_unStatus & (uint)eRoleStatus) > 0u;
    }
    /// <summary>
    /// 改变最大移动距离是，设置角色属性界面的数值
    /// </summary>
    /// <param name="unMaxMoveDis"></param>
    public void OnChangeMaxMoveDis(int unMaxMoveDis)
    {
        this.m_unMaxMoveDis = unMaxMoveDis;
        if (this.Role)
        {
            Singleton<BeastRole>.singleton.OnChangeMaxMoveDis(this.m_unMaxMoveDis);
        }
    }
    /// <summary>
    /// 改变技能cd，设置刷新角色属性界面和tab界面
    /// </summary>
    /// <param name="unSkillId"></param>
    /// <param name="byteCD"></param>
    public void OnSkillCDChange(int unSkillId, byte byteCD)
    {
        this.m_skillManager.OnSkillCDChange(unSkillId, byteCD);
        if (this.Role)
        {
            //DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshCard(EnumCardType.eCardType_Skill, unSkillId);
        }
        //DlgBase<DlgTab, DlgTabBehaviour>.singleton.Refresh();
    }
    /// <summary>
    /// 神兽进入阶段
    /// </summary>
    /// <param name="eNewRoleStage"></param>
    /// <param name="unBackUpTime"></param>
    /// <param name="unTimeLimit"></param>
    /// <param name="unTargetPlayerID"></param>
    /// <param name="eQueryTimeType"></param>
    public void OnEnterRoleStage(EClientRoleStage eNewRoleStage, uint unBackUpTime, uint unTimeLimit, uint unTargetPlayerID, EQueryTimeType eQueryTimeType)
    {
        //如果又一次重新进入该操作状态，就刷新该状态
        if (this.eRoleStage == eNewRoleStage)
        {
            this.RefreshRoleStage(unBackUpTime, unTimeLimit, unTargetPlayerID, eQueryTimeType);
        }
        else 
        {
            if (eNewRoleStage == EClientRoleStage.ROLE_STAGE_WAIT)
            {
                //如果是操作等待状态的时候，就不随机播放声音
                this.m_bToPlayVoiceInRound = false;
                this.m_bToPlayVoiceOutRound = true;
                this.m_fvoiceOutRoundMoment = Time.time + (float)UnityEngine.Random.Range(60, 90);
            }
            else 
            {
                this.m_bToPlayVoiceOutRound = false;
            }
            if (eNewRoleStage == EClientRoleStage.ROLE_STAGE_MOVE)
            {
                this.PlayAnim("Run");
            }
            else 
            {
                if (this.eRoleStage == EClientRoleStage.ROLE_STAGE_MOVE)
                {
                    this.StopAnim("Run");
                }
            }
            this.eRoleStage = eNewRoleStage;
            if (eNewRoleStage == EClientRoleStage.ROLE_STAGE_ACTION)
            {
                Debug.Log("进入到Action阶段");
            }
            if (this.Role) 
            {
                Singleton<BeastRole>.singleton.OnEnterRoleStage(eNewRoleStage, unBackUpTime, unTimeLimit, unTargetPlayerID, eQueryTimeType);
            }
        }
    }
    /// <summary>
    /// 通过技能id取得技能
    /// </summary>
    /// <param name="unSkillId"></param>
    /// <returns></returns>
    public SkillGameData GetSkillById(int unSkillId)
    {
        return this.m_skillManager.GetSkillById(unSkillId);
    }

    public List<int> GetCanUseSkillOrEquip(EnumSkillType eType)
    {
        List<int> result = new List<int>();
        if (eType == EnumSkillType.eSkillType_Skill)
        {
            result = m_skillManager.GetCanUseSkills();
        }
        else if(eType == EnumSkillType.eSKillType_Equip)
        {

        }
        else if (eType == EnumSkillType.eSkillType_Summelor)
        {

        }
        return result;
    }
    /// <summary>
    /// 该神兽是否已经激活了该技能
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool HasActivatedSkill(int skillId)
    {
        return this.m_skillManager.HasSkill(skillId);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!this.m_bDisposed)
        {
            if (disposing)
            {
 
            }
            if (this.m_beastModelObj != null)
            {
                UnityEngine.Object.Destroy(this.m_beastModelObj);
                this.m_beastModelObj = null;
                this.m_beastChildObj = null;
            }
            this.m_bDisposed = true;
        }
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 创建模型完成之后的回调函数
    /// </summary>
    /// <param name="obj"></param>
    private void OnAvatarFinished(GameObject obj)
    {
        this.m_beastModelObj = obj;
        this.LoadModelFinished();
    }
    private void LoadModelFinished()
    {
        if (null == this.m_beastModelObj)
        {
            this.m_log.Error("heroModelObj is null");
        }
        else
        {
            UnityEngine.Object.DontDestroyOnLoad(this.m_beastModelObj);
            this.m_beastBehaviour = this.m_beastModelObj.AddComponent<BeastBehaviour>();
            this.m_beastBehaviour.Beast = this;
            this.m_beastBehaviour.transform.parent = UnityGameEntry.Instance.BeastRoot;
            DataBeastlist dataById = GameData<DataBeastlist>.dataMap[this.m_dwBeastTypeId];
            if (dataById != null && dataById.Movementspeed != 0f)
            {
                this.m_beastBehaviour.MoveSpeed = dataById.Movementspeed;
            }
            if (Singleton<RoomManager>.singleton.IsObserver)
            {
                this.PlayAnim("Idle");
            }
            this.SetVisible(this.m_bVisible);
            if (this.m_bVisible)
            {
                this.MoveAction(this.m_vec3HexPos);
            }
        }
    }
    /// <summary>
    /// 刷新神兽阶段
    /// </summary>
    /// <param name="unBackUpTime"></param>
    /// <param name="unTimeLimit"></param>
    /// <param name="unTargetPlayerID"></param>
    /// <param name="eQueryTimeType"></param>
    private void RefreshRoleStage(uint unBackUpTime, uint unTimeLimit, uint unTargetPlayerID, EQueryTimeType eQueryTimeType)
    {
        if (this.Role)
        {
            Singleton<BeastRole>.singleton.RefreshRoleStage(unBackUpTime, unTimeLimit, unTargetPlayerID, eQueryTimeType);
        }
       // DlgBase<DlgHeadInfo, DlgHeadInfoBehaviour>.singleton.OnPlayerEnterRoleStage(this, this.m_eRoleStage, unTimeLimit);
    }
    /// <summary>
    /// 设置模型是否可见
    /// </summary>
    /// <param name="bVisible"></param>
    public void SetVisible(bool bVisible)
    {
        if (null != this.m_beastModelObj)
        {       
            this.m_beastModelObj.SetActive(bVisible);
        }
        this.m_bVisible = bVisible;
        //this.SetEffectVisible(bVisible);
        //DlgBase<DlgHeadInfo, DlgHeadInfoBehaviour>.singleton.Refresh(this);
    }
    /// <summary>
    /// 移动到这个格子坐标的动作表现
    /// </summary>
    /// <param name="vec3DestPos"></param>
    public void MoveAction(CVector3 vec3DestPos)
    {
        if (null == this.m_beastBehaviour)
        {
            this.m_log.Error("null == m_heroBehaviour");
        }
        else
        {
            this.m_beastBehaviour.SetPos(vec3DestPos);
        }
    }
    /// <summary>
    /// 模型离开格子的表现
    /// </summary>
    /// <param name="hexPos"></param>
    public void OnActionLeaveFrom(CVector3 hexPos)
    {
       // CSceneMgr.singleton.OnHeroLeaveFromAction(this.Id, hexPos);
    }
    /// <summary>
    /// 模型移动到这个格子的表现
    /// </summary>
    /// <param name="hexPos"></param>
    public void OnActionMoveTo(CVector3 hexPos)
    {
        //CSceneMgr.singleton.OnHeroMoveToAction(this.Id, hexPos);
    }
    public void ShowShadow()
    {

    }
	#endregion
    #region 析构方法
    ~Beast()
    {
        this.Dispose(false);
    }
    #endregion
}
