using UnityEngine;
using System;
using System.Collections.Generic;
using Utility;
using Client.Common;
using Game;
using Utility.Export;
using GameData;
using GameClient.Audio;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：RoomManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：房间数据管理器
/----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    internal class RoomManager: Singleton<RoomManager>
    {
        #region 字段
        private long m_unInRoundBeastId = 0;//神兽顺序id
        private uint m_unBeastRoundCount = 1u;//神兽顺序次数
        private bool m_bHasResetRoundOver = false;//是否重置神兽顺序
        private bool m_bIsObserverMode = false;
        private uint m_unMapId = 0u;//选择的地图id
        private long m_ulRoomId = 0u;//房间id
        private bool m_bIsInGameMain;//是否在主游戏状态
        private bool m_bGameOver = true;//是否游戏结束了
        private bool m_bIsAsyncLoadingThenSetGameInfo = false;//是否处理玩家血量变化是异步的
        private List<CRoomAllData> m_AysncDataForSetGameInfo = null;//所有玩家的房间数据信息
        private float m_fBattleStartTime = 0f;//游戏战斗开始时间
        private int m_AINumInTeam = 0;//房间内机器人的数量
        private byte m_BeasetRepeatSelectInAllCamp = (byte)1;//是否可以选择重复的神兽
        private float m_fTimeLimit = 0;//ban人时间限制
        private float m_fTimeLimitForSelectBornPos = 0f;//选择出生点的时间限制
        private float m_fStartTimeForSelectBornPos = 0f;//选择出生点的开始时间
        private EMatchtype m_eMatchType = EMatchtype.MATCH_INVALID;//匹配类型
        private EnumMathMode m_EnumMathMode = EnumMathMode.EnumMathMode_FightMode;//战斗模式
        private EGameBeastSelectType m_eGameBeastSelectType = EGameBeastSelectType.GAME_BEAST_SELECT_TYPE_MATCH;//神兽选择类型
        private EGameType m_eGameType = EGameType.GAME_TYPE_INVALID;//游戏类型
        private EGamePhase m_eGamePhase = EGamePhase.GAME_PHASE_CHOOSING;//匹配选人阶段，默认为普通选人
        private IXLog m_log = XLog.GetLog<RoomManager>();
        private List<PlayerData> m_listOurPlayers = new List<PlayerData>();//我方玩家
        private List<PlayerData> m_listEnemyPlayers = new List<PlayerData>();//敌方玩家
        private Dictionary<uint, CBeastData> m_listLeagueBeasts = new Dictionary<uint, CBeastData>();//league阵营的神兽数据
        private Dictionary<uint, CBeastData> m_listEmpireBeasts = new Dictionary<uint, CBeastData>();//empire阵营的神兽数据
        private static PlayerData s_playerRoleDataError = new PlayerData(0u, ECampType.CAMP_INVALID);//静态错误的玩家数据
        private CampData[] m_CampDatas = null;//阵营数据信息
        #endregion
        #region 属性
        public long BeastIdInRound 
        {
            get 
            {
                return this.m_unInRoundBeastId;
            }
            set 
            {
                this.m_unInRoundBeastId = value;
            }
        }
        public uint BeastRound
        {
            get
            {
                return this.m_unBeastRoundCount;
            }
            set
            {
                this.m_unBeastRoundCount = value;
            }
        }
        /// <summary>
        /// 地图id
        /// </summary>
        public uint MapId 
        {
            get { return this.m_unMapId; }
            set { this.m_unMapId = value; }
        }
        /// <summary>
        /// 房间id
        /// </summary>
        public long RoomId 
        {
            get { return this.m_ulRoomId; }
            set { this.m_ulRoomId = value; }
        }
        /// <summary>
        /// 开始战斗时间
        /// </summary>
        public float BattleStartTime
        {
            get
            {
                return this.m_fBattleStartTime;
            }
            set
            {
                this.m_fBattleStartTime = value;
            }
        }
        /// <summary>
        /// ban人时间限制
        /// </summary>
        public float TimeLimit 
        {
            get { return this.m_fTimeLimit; }
            set { this.m_fTimeLimit = value; }
        }
        /// <summary>
        /// 是否游戏结束了
        /// </summary>
        public bool GameOver 
        {
            get { return this.m_bGameOver; }
            set { this.m_bGameOver = value; }
        }
        /// <summary>
        /// 选择出生点的时间限制
        /// </summary>
        public float TimeLimitForSelectBornPos
        {
            get
            {
                return this.m_fTimeLimitForSelectBornPos;
            }
            set
            {
                this.m_fTimeLimitForSelectBornPos = value;
                this.m_fStartTimeForSelectBornPos = Time.time;
            }
        }
        /// <summary>
        /// 房间中所有玩家的数据
        /// </summary>
        public List<PlayerData> AllPlayerDatas
        {
            get
            {
                List<PlayerData> list = new List<PlayerData>(this.m_listOurPlayers);
                list.AddRange(this.m_listEnemyPlayers);
                return list;
            }
        }
        /// <summary>
        /// 房间内机器人的数量
        /// </summary>
        public int AINumInTeam
        {
            set { this.m_AINumInTeam = value; }
        }
        /// <summary>
        /// 是否刷新完成了
        /// </summary>
        public bool OnRefreshFinish
        {
            get;
            set;
        }
        /// <summary>
        /// 我方玩家角色信息
        /// </summary>
        public List<PlayerData> OurPlayerDatas
        {
            get { return this.m_listOurPlayers; }
        }
        /// <summary>
        /// 敌方玩家角色的信息
        /// </summary>
        public List<PlayerData> EnemyPlayerDatas
        {
            get { return this.m_listEnemyPlayers; }
        }
        /// <summary>
        /// 玩家自己的数据信息
        /// </summary>
        public PlayerData PlayerRoleData
        {
            get
            {
                PlayerData result;
                foreach (PlayerData current in this.m_listOurPlayers)
                {
                    if (Singleton<PlayerRole>.singleton.ID == current.PlayerId)
                    {
                        result = current;
                        return result;
                    }
                }
                result = RoomManager.s_playerRoleDataError;
                return result;
            }
        }
        /// <summary>
        /// 是否是观察者的模式
        /// </summary>
        public bool IsObserver 
        {
            get { return this.m_bIsObserverMode; }
            set 
            {
                if (value != this.m_bIsObserverMode)
                {
                    this.m_bIsObserverMode = value;
                    if (this.m_bIsObserverMode)
                    {
                        
                    }
                }
            }
        }
        /// <summary>
        /// 是否显示周免神兽
        /// </summary>
        public bool IsShowFreeBeast
        {
            get
            {
                return Singleton<RoomManager>.singleton.MathMode != EnumMathMode.EnumMathMode_AdventureMode && Singleton<RoomManager>.singleton.MathMode != EnumMathMode.EnumMathMode_Story && Singleton<RoomManager>.singleton.MathMode != EnumMathMode.EnumMathMode_Expedition;
            }
        }
        /// <summary>
        /// 是否双方阵营可以选择一样的神兽
        /// </summary>
        public byte RepeatSelectBeastInAllCamp 
        {
            get 
            {
                return this.m_BeasetRepeatSelectInAllCamp;
            }
        }
        /// <summary>
        /// 匹配类型
        /// </summary>
        public EMatchtype MatchType 
        {
            get { return this.m_eMatchType; }
            set { this.m_eMatchType = value; }
        }
        /// <summary>
        /// 游戏类型
        /// </summary>
        public EGameType GameType 
        {
            get { return this.m_eGameType; }
            set { this.m_eGameType = value; }
        }
        /// <summary>
        /// 战斗模式，天梯，人机，匹配，故事等
        /// </summary>
        public EnumMathMode MathMode 
        {
            get { return this.m_EnumMathMode; }
            set { this.m_EnumMathMode = value; }
        }
        /// <summary>
        /// 匹配选人阶段
        /// </summary>
        public EGamePhase EGamePhase 
        {
            get { return this.m_eGamePhase; }
            set { this.m_eGamePhase = value; }
        }
        /// <summary>
        /// 神兽选择类型
        /// </summary>
        public EGameBeastSelectType BeastSelectType 
        {
            get { return this.m_eGameBeastSelectType; }
            set { this.m_eGameBeastSelectType = value; }
        }
        #endregion
        #region 构造方法
        public RoomManager()
        {
            this.m_CampDatas = new CampData[2];
            this.m_CampDatas[0] = new CampData(10u, ECampType.CAMP_EMPIRE);
            this.m_CampDatas[1] = new CampData(10u, ECampType.CAMP_LEAGUE);
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 玩家进入房间,初始化房间内所有玩家的数据
        /// </summary>
        /// <param name="ulRoomId"></param>
        /// <param name="oEmpireMemberList"></param>
        /// <param name="oLeagueMemberList"></param>
        /// <param name="uMapid"></param>
        public void OnPlayerEnter(long ulRoomId, List<CRoomMemberData> oEmpireMemberList, List<CRoomMemberData> oLeagueMemberList, uint uMapid)
        {
            this.m_ulRoomId = ulRoomId;
            this.m_unMapId = uMapid;
            List<PlayerData> empireList = new List<PlayerData>();
            List<PlayerData> leagueList = new List<PlayerData>();
            //初始化empire方的玩家信息
            for (int i = 0; i < oEmpireMemberList.Count; i++)
            {
                CRoomMemberData data = oEmpireMemberList[i];
                PlayerData playerData = new PlayerData(data.m_unPlayerID, ECampType.CAMP_EMPIRE);
                //初始化角色姓名，头像，等级
                playerData.Name = data.m_strName;
                playerData.Icon = data.m_strIcon;
                playerData.Level = data.m_unLevel;
                //初始化角色拥有的神兽信息
                for (int j = 0; j < data.m_lBeastList.Count; j++)
                {
                    BeastData beastData = new BeastData(playerData.PlayerId,data.m_lBeastList[j],ECampType.CAMP_EMPIRE);
                    playerData.Beasts.Add(beastData);
                }
                //如果房间内的玩家刚好是自己本身,这说明没有重连
                if (data.m_unPlayerID == Singleton<PlayerRole>.singleton.ID)
                {
                    Singleton<PlayerRole>.singleton.CampType = ECampType.CAMP_EMPIRE;
                    playerData.IsReconnect = false;
                }
                //如果不是自己，则判断重连标记是否等1，是的话就是重连
                else 
                {
                    playerData.IsReconnect = (data.m_btIsReconnecting == 1);
                }
                empireList.Add(playerData);
            }
            //再初始化league方玩家的信息
            for (int i = 0; i < oLeagueMemberList.Count; i++)
            {
                CRoomMemberData data = oLeagueMemberList[i];
                PlayerData playerData = new PlayerData(data.m_unPlayerID, ECampType.CAMP_LEAGUE);
                //初始化角色姓名，头像，等级
                playerData.Name = data.m_strName;
                playerData.Icon = data.m_strIcon;
                playerData.Level = data.m_unLevel;
                Debug.Log("BeastList:" + data.m_lBeastList.Count);
                //初始化角色拥有的神兽信息
                for (int j = 0; j < data.m_lBeastList.Count; j++)
                {
                    Debug.Log("init beastdata");
                    BeastData beastData = new BeastData(playerData.PlayerId, data.m_lBeastList[j], ECampType.CAMP_LEAGUE);
                    playerData.Beasts.Add(beastData);
                }
                //如果房间内的玩家刚好是自己本身，初始化自身的阵营
                if (data.m_unPlayerID == Singleton<PlayerRole>.singleton.ID)
                {
                    Singleton<PlayerRole>.singleton.CampType = ECampType.CAMP_LEAGUE;
                    playerData.IsReconnect = false;
                }
                else
                {
                    playerData.IsReconnect = (data.m_btIsReconnecting == 1);
                }
                leagueList.Add(playerData);
            }
            if (ECampType.CAMP_LEAGUE == Singleton<PlayerRole>.singleton.CampType)
            {
                this.m_listOurPlayers = leagueList;
                this.m_listEnemyPlayers = empireList;
            }
            else 
            {
                this.m_listEnemyPlayers = leagueList;
                this.m_listOurPlayers = empireList;
            }
            DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.Refresh();
        }
        /// <summary>
        /// 玩家确认选择该神兽
        /// </summary>
        /// <param name="unRoleId"></param>
        /// <param name="unBeastTypeId"></param>
        /// <param name="unBeastLevel"></param>
        /// <param name="oSkillList"></param>
        /// <param name="btIsRandom"></param>
        /// <param name="nSuitId"></param>
        public void OnPlayerSelectBeast(long unRoleId,int unBeastTypeId,int unBeastLevel,ref List<int> oSkillList,byte btIsRandom,int nSuitId)
        {
            BeastData beastData = null;
            foreach (var current in this.m_listEnemyPlayers)
            {
                for (int i = 0; i < current.Beasts.Count; i++)
                {
                    BeastData beast = current.Beasts[i];
                    if (beast.Id == unRoleId)
                    {
                        beastData = beast;
                        beast.BeastTypeId = unBeastTypeId;
                        beast.BeastLevel = unBeastLevel;
                        beast.IsRandom = (btIsRandom > 0);
                        beast.SuitId = nSuitId;
                        beast.Skills.Clear();
                        foreach (var skill in oSkillList)
                        {
                            SkillGameData data = new SkillGameData(skill);
                            beast.Skills.Add(data);
                        }
                    }
                }
            }
            foreach (var current in this.m_listOurPlayers)
            {
                for (int i = 0; i < current.Beasts.Count; i++)
                {
                    BeastData beast = current.Beasts[i];
                    if (beast.Id == unRoleId)
                    {
                        beastData = beast;
                        beast.BeastTypeId = unBeastTypeId;
                        beast.BeastLevel = unBeastLevel;
                        beast.IsRandom = (btIsRandom > 0);
                        beast.SuitId = nSuitId;
                        beast.Skills.Clear();
                        foreach (var skill in oSkillList)
                        {
                            SkillGameData data = new SkillGameData(skill);
                            beast.Skills.Add(data);
                        }
                    }
                }
            }
            if (beastData != null)
            {
                if (!beastData.IsRandom)
                {
                    DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.RefreshPlayerInfo(beastData);
                    if (beastData.Id == DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.CurSelectBeastId)
                    {

                    }
                }
                if (beastData.PlayerId == Singleton<PlayerRole>.singleton.ID)
                {
                    CBeastData data = new CBeastData(unBeastTypeId, 0, unBeastLevel, nSuitId);
                    data.m_oSkillList.AddRange(oSkillList);
                    data.m_oSuitList.Add(nSuitId);
                    Singleton<PlayerRole>.singleton.AddTempBeastData(data);
                }
            }
            else 
            {
                this.m_log.Error("null == beastDataCur:unBeastId:" + unRoleId);
            }
            BeastData beastData1 = this.GetBeastData(unRoleId);
            if (beastData.PlayerId == Singleton<PlayerRole>.singleton.ID)
            {
                //刷新技能
                DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.RefreshSkill(beastData1);
            }
        }
        public void RecvConfirmBeast(long beastId)
        {
            BeastData data = Singleton<RoomManager>.singleton.GetBeastData(beastId);
            if (data != null)
            {
                data.IsSelected = true;
                if (data.IsRandom)
                {
                    if (data.PlayerId == Singleton<PlayerRole>.singleton.ID)
                    {
                        DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.IsSelected = data.IsSelected;
                    }
                }
                else 
                {
                    this.ConfirmBeast(beastId);
                }
                if (data.PlayerId == Singleton<PlayerRole>.singleton.ID)
                {
                    DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.ShowCurSelectRole();
                }
            }
        }
        public void ConfirmBeast(long beastId)
        {
            PlayerData playerData = this.GetPlayerData(beastId);
            BeastData beastData = this.GetBeastData(beastId);
            if (playerData != null && beastData != null)
            {
                DataBeastlist dataList = null;
                GameData<DataBeastlist>.dataMap.TryGetValue(beastData.BeastTypeId, out dataList);
                if (dataList != null)
                {
                    DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.OnRecvChat(playerData.Name, dataList.Name, dataList.NickName, beastData.IsRandom, playerData.CampType);
                    DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.ShowCampTypeEffect(beastId);
                }
            }
        }
       /// <summary>
        /// 玩家添加神兽到场景中
        /// </summary>
        /// <param name="unBeastId"></param>
        /// <param name="oInitialPos"></param>
        public void OnAddHeroToScene(long unBeastId, CVector3 oInitialPos)
        {
            Singleton<BeastManager>.singleton.InitBeastPos(unBeastId, oInitialPos);
            Singleton<BeastManager>.singleton.OnBeastRevive(unBeastId);
           // Singleton<BeastManager>.singleton.StopPassiveSkillEffect(unHeroId);
            Singleton<BeastManager>.singleton.ActiveSkills(unBeastId);
            Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(unBeastId, EClientRoleStage.ROLE_STAGE_WAIT, 0u);
            Singleton<BeastManager>.singleton.OnAddBeastToSceneFinished(unBeastId);
            if (1u == this.m_unBeastRoundCount && Singleton<BeastManager>.singleton.IsAllBeastNoDie)
            {
                Singleton<BeastManager>.singleton.OnBeastBorn(unBeastId);
                this.ShowNextSelfBeastInfo(unBeastId);
            }
        }
        /// <summary>
        /// 改变阵营的血量,然后根据血量判断是否播放不同的背景乐
        /// </summary>
        /// <param name="eCampType"></param>
        /// <param name="unHp"></param>
        public void OnCampHpChange(ECampType eCampType, uint unHp)
        {
            uint index = (uint)(eCampType - ECampType.CAMP_EMPIRE);
            if (index >= 0 && index < this.m_CampDatas.Length)
            {
                this.m_CampDatas[index].Hp = unHp;
            }
            //根据不同基地的血量播放不同的背景音乐
            this.PlayMusicByBaseHp();
        }
        /// <summary>
        /// 进入主游戏战斗
        /// </summary>
        public void EnterGameMain()
        {
            //在进入主战斗先离开
            this.LeaveGameMain();
            this.m_bIsInGameMain = true;
            Singleton<PlayerManager>.singleton.CreateAllPlayer(Singleton<RoomManager>.singleton.AllPlayerDatas);
            CSceneMgr.singleton.CreateScene(Singleton<RoomManager>.singleton.MapId);
        }
        /// <summary>
        /// 离开主游戏战斗
        /// </summary>
        public void LeaveGameMain()
        {
            if (this.m_bIsInGameMain)
            {
                Singleton<RoomManager>.singleton.Reset();
                Singleton<PlayerManager>.singleton.DelAllPlayers();
                CSceneMgr.singleton.Clear();
            }
            this.m_bIsInGameMain = false;
        }
       /// <summary>
       /// 计算双方阵营内机器人的数量
       /// </summary>
       /// <param name="oEmpireMemberList"></param>
       /// <param name="oLeagueMemberList"></param>
       /// <returns></returns>
        public int GetAINumInTeam(List<CRoomMemberData> oEmpireMemberList, List<CRoomMemberData> oLeagueMemberList)
        {
            List<CRoomMemberData> list = (Singleton<PlayerRole>.singleton.CampType == ECampType.CAMP_EMPIRE) ? oEmpireMemberList : oLeagueMemberList;
            int AICount = 0;
            list.ForEach(delegate(CRoomMemberData e)
            {
                if (e.m_unPlayerID >= 268435455u)
                {
                    AICount++;
                }
            });
            return AICount;
        }
        /// <summary>
        /// 重新设置房间数据
        /// </summary>
        public void Reset()
        {
            try
            {
                
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        /// <summary>
        /// 取得我方阵营数据
        /// </summary>
        /// <returns></returns>
        public CampData GetOurCampData()
        {
            return this.GetCampData(Singleton<PlayerRole>.singleton.CampType);
        }
        public CampData GetCampData(ECampType eCampType)
        {
            int num = (int)(eCampType - ECampType.CAMP_EMPIRE);
            CampData result;
            if (0 <= num && num < 2)
            {
                result = this.m_CampDatas[num];
            }
            else
            {
                result = default(CampData);
            }
            return result;
        }
        /// <summary>
        /// 设置阵营的最大血量
        /// </summary>
        /// <param name="unEmpireHp"></param>
        /// <param name="unLeagueHp"></param>
        public void SetCampMaxHp(uint unEmpireHp, uint unLeagueHp)
        {
            try
            {
                this.m_CampDatas[0].MaxHp = unEmpireHp;
                this.m_CampDatas[1].MaxHp = unLeagueHp;
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        /// <summary>
        /// 取得该玩家的神兽数据
        /// </summary>
        /// <param name="beastId"></param>
        /// <returns></returns>
        public BeastData GetBeastData(long beastId)
        {
            BeastData result;
            foreach (var current in this.m_listOurPlayers)
            {
                BeastData beastData = current.Beasts.Find((BeastData data)=>data.Id == beastId);
                if (beastData != null)
                {
                    result = beastData;
                    return result;
                }
            }
            foreach (var current in this.m_listEnemyPlayers)
            {
                BeastData data = current.Beasts.Find((BeastData data1)=>data1.Id == beastId);
                if (data != null)
                {
                    result = data;
                    return result;
                }
            }
            result = null;
            return result;
        }
        /// <summary>
        /// 根据神兽ID取得神兽对应的头像Icon
        /// </summary>
        /// <param name="beastId"></param>
        /// <returns></returns>
        public string GetBeastIcon(long beastId)
        {
            BeastData beastData = this.GetBeastData(beastId);
            string result = string.Empty;
            if (beastData != null)
            {
                DataBeastlist beastList = GameData<DataBeastlist>.dataMap[beastData.BeastTypeId];
                if (beastList != null)
                {
                    result = beastList.IconFile;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据玩家的神兽Id取得玩家信息
        /// </summary>
        /// <param name="beastId"></param>
        /// <returns></returns>
        private PlayerData GetPlayerData(long beastId)
        {
            foreach (var playerOur in this.m_listOurPlayers)
            {
                BeastData beastData = playerOur.Beasts.Find((BeastData beast) => beast.Id == beastId);
                if (beastData != null)
                {
                    return playerOur;
                }
            }
            foreach (var playerEnemy in this.m_listEnemyPlayers)
            {
                BeastData beastData = playerEnemy.Beasts.Find((BeastData data) => data.Id == beastId);
                if (beastData != null)
                {
                    return playerEnemy;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据玩家Id取得玩家数据
        /// </summary>
        /// <param name="lPlayerId"></param>
        /// <returns></returns>
        public PlayerData GetPlayerDataById(long lPlayerId)
        {
            PlayerData playerData = this.m_listOurPlayers.Find((PlayerData player) => player.PlayerId == lPlayerId);
            PlayerData result;
            if (null != playerData)
            {
                result = playerData;
            }
            else
            {
                playerData = this.m_listEnemyPlayers.Find((PlayerData player) => player.PlayerId == lPlayerId);
                if (null != playerData)
                {
                    result = playerData;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        /// <summary>
        /// 玩家取得还没有选择的神兽id
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public long GetEmptyBeastId(long playerId)
        {
            long result;
            foreach (var current in this.m_listOurPlayers)
            {
                if (current.PlayerId == playerId)
                {
                    foreach (var beastData in current.Beasts)
                    {
                        if (!beastData.IsSelected)
                        {
                            result = beastData.Id;
                            return result;
                        }
                    }
                }
            }
            foreach (var current in this.m_listEnemyPlayers)
            {
                if (current.PlayerId == playerId)
                {
                    foreach (var beastData in current.Beasts)
                    {
                        if (!beastData.IsSelected)
                        {
                            result = beastData.Id;
                            return result;
                        }
                    }
                }
            }
            result = 0;
            return result;
        }
        /// <summary>
        /// 根据基地不同的血量，播放不同的背景音乐
        /// </summary>
        public void PlayMusicByBaseHp()
        {
            DataMaplist dataMap = GameData<DataMaplist>.dataMap[(int)this.m_unMapId];
            if (dataMap != null)
            {
                uint emHp = this.m_CampDatas[0].Hp;
                uint leHp = this.m_CampDatas[1].Hp;
                if (emHp <= 3u || leHp <= 3u)
                {
                    string audioPath = dataMap.WarnSoundFile;
                    if (!string.IsNullOrEmpty(audioPath))
                    {
                        Singleton<AudioManager>.singleton.PlayMusic(string.Format("Audio/BGM/Map/{0}", audioPath));
                    }
                }
                else if(emHp + leHp <= 10u)
                {
                    string audioPath = dataMap.ToughSoundFile;
                    if (!string.IsNullOrEmpty(audioPath))
                    {
                        Singleton<AudioManager>.singleton.PlayMusic(string.Format("Audio/BGM/Map/{0}", audioPath));
                    }
                }
                else if (emHp + leHp <= 16u)
                {
                    string audioPath = dataMap.NormalSoundFile;
                    if (!string.IsNullOrEmpty(audioPath))
                    {
                        Singleton<AudioManager>.singleton.PlayMusic(string.Format("Audio/BGM/Map/{0}", audioPath));
                    }
                }
            }
        }
        /// <summary>
        /// 玩家开始选择神兽出生点
        /// </summary>
        /// <param name="beastId"></param>
        public void StartPlayerSelectBornPos(long beastId)
        {
            float timeLimit = this.m_fTimeLimitForSelectBornPos - (Time.time - this.m_fTimeLimitForSelectBornPos);
            if (timeLimit < 0f)
            {
                timeLimit = 0f;
            }
            if (Singleton<PlayerRole>.singleton.Player.ListBeastId.Contains(beastId))
            {
                Singleton<BeastRole>.singleton.Id = beastId;
            }
            //角色进入选择出生点阶段
            Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(beastId, EClientRoleStage.ROLE_STAGE_SELECT_BORN_POS, (uint)timeLimit);
        }
        /// <summary>
        /// 是否是异步处理血量改变
        /// </summary>
        /// <param name="unRoleId"></param>
        /// <param name="btHp"></param>
        /// <returns></returns>
        public bool ProcessHpChangedAsync(long unRoleId, byte btHp)
        {
            bool result;
            if (!this.m_bIsAsyncLoadingThenSetGameInfo)
            {
                result = false;
            }
            else
            {
                CRoomAllData cRoomAllData = this.m_AysncDataForSetGameInfo[0];
                foreach (CRoleData current in cRoomAllData.m_oRoleList)
                {
                    if (current.m_dwRoleId == unRoleId)
                    {
                        current.m_btHp = btHp;
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 开始轮到玩家操作
        /// </summary>
        /// <param name="beastId"></param>
        public void StartBeastRound(long beastId)
        {
            CSceneMgr.singleton.OnStartBeastRound(beastId);
            this.m_unInRoundBeastId = beastId;
            //tab界面进入该神兽的操作
            if (this.IsObserver)
            {

            }
            else 
            {
                if (Singleton<PlayerRole>.singleton.Player.ListBeastId.Contains(beastId))
                {
                    Singleton<BeastRole>.singleton.Id = beastId;
                }
            }
            Beast beast = Singleton<BeastManager>.singleton.GetBeastById(beastId);
            if (beast != null)
            {
                Singleton<BeastManager>.singleton.OnBeastRoundStart(beastId);
                Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(beastId, EClientRoleStage.ROLE_STAGE_WAIT, 0u);
            }
            beast = Singleton<BeastManager>.singleton.GetBeastByOrder(1);
            if (beast != null)
            {
                if (beastId == beast.Id)
                {
                    //记录到记录板上
                    //this.RecordRoundCount();
                    if (this.m_bHasResetRoundOver)
                    {
                        this.m_unBeastRoundCount += 1u;
                    }
                    if (this.m_unBeastRoundCount == 1u)
                    {
                        this.DoStartFight();
                        //播放战斗开始音频
                    }
                    if (this.m_unBeastRoundCount >= 1u)
                    {

                    } 
                }
            }
        }
        public void StopBeastRound(long beastId)
        {
            if (this.m_unInRoundBeastId == beastId || this.m_unInRoundBeastId == 0)
            {
                this.m_bHasResetRoundOver = true;
                Singleton<BeastManager>.singleton.OnBeastRoundEnd(beastId);
                Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(beastId, EClientRoleStage.ROLE_STAGE_WAIT, 0u);
                this.ShowNextSelfBeastInfo(beastId);
            }
        }
        /// <summary>
        /// 是否是天梯模式
        /// </summary>
        /// <returns></returns>
        public bool IsLadderMode()
        {
            return this.MathMode == EnumMathMode.EnumMathMode_Ladder;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 显示自己下个神兽的信息
        /// </summary>
        /// <param name="unBeastId"></param>
        private void ShowNextSelfBeastInfo(long unBeastId)
        {
            if (this.MatchType == EMatchtype.MATCH_1C3)
            {
                Beast beast = Singleton<BeastManager>.singleton.GetBeastById(unBeastId);
                if (beast != null)
                {
                    int order = beast.Order;//取得该神兽的顺序index
                    int i = 0;
                    while (i <= 5)
                    {
                        order = (order + 1 > 6) ? 1 : order + 1;//下个神兽
                        Beast beastByOrder = Singleton<BeastManager>.singleton.GetBeastByOrder(order);
                        if (beastByOrder != null)
                        {
                            //判断下个神兽是否是自己的神兽
                            if (beastByOrder.PlayerId != Singleton<PlayerRole>.singleton.ID)
                            {
                                i++;
                                continue;
                            }
                            //判断是否是正在操作的神兽，如果不是说明是下个自己的神兽，就刷新操作界面
                            if (Singleton<BeastRole>.singleton.Id != beastByOrder.Id)
                            {
                                Singleton<BeastRole>.singleton.Id = beastByOrder.Id;
                                //刷新主界面
                                DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshPlayerRoleInfo();
                            }
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 开始战斗，当玩家选择好神兽的初始化位置
        /// </summary>
        private void DoStartFight()
        {
            DlgBase<DlgStartBattle, DlgStartBattleBehaviour>.singleton.Show();
        }
        #endregion
    }
}
