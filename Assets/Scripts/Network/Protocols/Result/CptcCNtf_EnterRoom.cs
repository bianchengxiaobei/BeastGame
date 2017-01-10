using UnityEngine;
using System.Collections.Generic;
using Utility;
using Client.Common;
using Client.Data;
using Utility.Export;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCNtf_EnterRoom
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.1
// 模块描述：服务器发送进入房间消息1009
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 服务器发送进入房间消息1009
    /// </summary>
    public class CptcCNtf_EnterRoom : CProtocol
    {
        private const int m_dwPtcG2CNtf_EnterRoomID = 1009;
        private IXLog m_log = XLog.GetLog<CptcCNtf_EnterRoom>();
        /// <summary>
        /// 房间id
        /// </summary>
        public long m_qwRoomID;
        /// <summary>
        /// 选择的地图id
        /// </summary>
        public int m_qwMapID;
        /// <summary>
        /// 时间限制
        /// </summary>
        public int m_qwTimeLimit;
        /// <summary>
        /// 游戏类型
        /// </summary>
        public byte m_btGameType;
        /// <summary>
        /// 神兽选择类型，分为匹配和排位
        /// </summary>
        public byte m_btBeastSelectType;
        /// <summary>
        /// 红队所有成员
        /// </summary>
        public List<CRoomMemberData> m_oEmpireMemberList;
        /// <summary>
        /// 蓝队所有成员
        /// </summary>
        public List<CRoomMemberData> m_oLeagueMemberList;

        public CptcCNtf_EnterRoom() : base(1009)
        {
            this.m_qwRoomID = 0;
            this.m_qwMapID = 0;
            this.m_qwTimeLimit = 0;
            this.m_btBeastSelectType = 0;
            this.m_btGameType = 0;
            this.m_oEmpireMemberList = new List<CRoomMemberData>();
            this.m_oLeagueMemberList = new List<CRoomMemberData>();
        }
        public override CByteStream Serialize(CByteStream bs)
        {         
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_qwRoomID);
            this.m_oEmpireMemberList.Clear();
            int num = 0;
            bs.Read(ref num);
            for (int i = 0; i < num; i++)
            {
                CRoomMemberData data = new CRoomMemberData();
                bs.Read(data);
                this.m_oEmpireMemberList.Add(data);
            }
            this.m_oLeagueMemberList.Clear();
            num = 0;
            bs.Read(ref num);
            for (int i = 0; i < num; i++)
            {
                CRoomMemberData data = new CRoomMemberData();
                bs.Read(data);
                this.m_oLeagueMemberList.Add(data);
            }
            bs.Read(ref this.m_qwTimeLimit);
            bs.Read(ref this.m_qwMapID);
            bs.Read(ref this.m_btGameType);
            return bs;
        }
        public override void Process()
        {
            this.m_log.Debug("CptcCNtf_EnterRoom" + (EGameType)this.m_btGameType);
            //房间内所有的成员
            List<CRoomMemberData> allMemberList = new List<CRoomMemberData>();
            allMemberList.AddRange(this.m_oEmpireMemberList);
            allMemberList.AddRange(this.m_oLeagueMemberList);
            //是否是观察者，根据判断自己是否在房间内，不在就是观察者
            Singleton<RoomManager>.singleton.IsObserver = !allMemberList.Exists((CRoomMemberData o) => o.m_unPlayerID == Singleton<PlayerRole>.singleton.ID);
            if (Singleton<RoomManager>.singleton.IsObserver)
            {
                //进行处理
                Debug.Log("IsObserver");
            }
            //如果对战是3人对3人，那么匹配模式是3v3，否则就是1人3个神兽
            if (this.m_oEmpireMemberList.Count == 3 && this.m_oLeagueMemberList.Count == 3)
            {
                Singleton<RoomManager>.singleton.MatchType = EMatchtype.MATCH_3V3;
            }
            else 
            {
                Singleton<RoomManager>.singleton.MatchType = EMatchtype.MATCH_1C3;
            }
            Singleton<PlayerRole>.singleton.GameType = (EGameType)this.m_btGameType;
            Singleton<RoomManager>.singleton.GameType = (EGameType)this.m_btGameType;
            //如果是天梯模式的话，进入BanPick阶段
            if (Singleton<RoomManager>.singleton.IsLadderMode())
            {
                Singleton<RoomManager>.singleton.EGamePhase = EGamePhase.GAME_PHASE_BANNING;
                //暂时还没做这个功能
            }
            //否则是普通的选人
            else 
            {
                Singleton<RoomManager>.singleton.EGamePhase = EGamePhase.GAME_PHASE_CHOOSING;
            }
            Singleton<RoomManager>.singleton.BeastSelectType = (EGameBeastSelectType)this.m_btBeastSelectType;
            //初始化房间内所有玩家的PlayerData
            Singleton<RoomManager>.singleton.OnPlayerEnter(this.m_qwRoomID, this.m_oEmpireMemberList, this.m_oLeagueMemberList, (uint)this.m_qwMapID);
            //计算双方队伍中机器人的数量
            Singleton<RoomManager>.singleton.AINumInTeam = Singleton<RoomManager>.singleton.GetAINumInTeam(this.m_oEmpireMemberList, this.m_oLeagueMemberList);
            if (this.m_qwTimeLimit > 0)
            {
                if (Singleton<RoomManager>.singleton.EGamePhase == EGamePhase.GAME_PHASE_BANNING)
                {
                    Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_InRoom);
                    Singleton<RoomManager>.singleton.TimeLimit = (float)this.m_qwTimeLimit;
                }
                else 
                {
                    Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_InRoom);
                    DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.SetTimeLimit(this.m_qwTimeLimit, EnumSelectStep.eSelectStep_Select);
                }        
            }
        }
    }
}
