using UnityEngine;
using System.Collections.Generic;
using Utility;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCNtf_StartGame
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：服务器想客户端发送开始游戏战斗消息
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 服务器想客户端发送开始游戏战斗消息
    /// </summary>
    public class CPtcCNtf_StartGame : CProtocol 
    {
        #region 字段
        private const int m_dwPtcG2CNtf_StartGameID = 1102;
        public long m_dwRoleID;
        public List<long> m_oPlayOrder;//玩家战斗中的顺序
        public int m_wTimeLimit;//选择出生点的时间限制
        public byte m_btEmpireHp;//红方阵营的血量
        public byte m_btLeagueHp;//蓝方阵营的血量
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public CPtcCNtf_StartGame() : base(1021)
		{
            this.m_oPlayOrder = new List<long>();
		}
        #endregion
        #region 公有方法
        public override void Process()
        {
            //所有的神兽进入战斗等待阶段
            ICollection<Beast> allBeasts = Singleton<BeastManager>.singleton.GetAllBeasts();
            foreach (var beast in allBeasts)
            {
                Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(this.m_dwRoleID,EClientRoleStage.ROLE_STAGE_WAIT,0u);
            }
            XLog.Log.Debug("CPtcG2CNtf_StartGame");
            Singleton<RoomManager>.singleton.BattleStartTime = Time.time;
            Singleton<RoomManager>.singleton.TimeLimitForSelectBornPos = this.m_wTimeLimit;
            Singleton<RoomManager>.singleton.SetCampMaxHp((uint)this.m_btEmpireHp, (uint)this.m_btLeagueHp);
            Singleton<RoomManager>.singleton.OnCampHpChange(ECampType.CAMP_EMPIRE, (uint)this.m_btEmpireHp);
            Singleton<RoomManager>.singleton.OnCampHpChange(ECampType.CAMP_LEAGUE, (uint)this.m_btLeagueHp);
            //初始化神兽战斗顺序
            for (int i = 0; i < this.m_oPlayOrder.Count; i++)
            {
                Beast beast = Singleton<BeastManager>.singleton.GetBeastById(this.m_oPlayOrder[i]);
                if (null != beast)
                {
                    beast.Order = i + 1;
                }
            }
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_dwRoleID);
            bs.Read(ref this.m_btEmpireHp);
            bs.Read(ref this.m_btLeagueHp);
            bs.Read(ref this.m_wTimeLimit);
            int num = 0;
            bs.Read(ref num);
            this.m_oPlayOrder.Clear();
            for (int i = 0; i < num; i++)
            {
                long id = 0;
                bs.Read(ref id);
                this.m_oPlayOrder.Add(id);
            }
            return bs;
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            return bs;
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
