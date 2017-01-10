using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
using Client.Common;
using Client.UI.UICommon;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PlayerRole
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：角色信息类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class PlayerRole : Singleton<PlayerRole>
    {
        #region 字段
        private long m_unPlayerId = 0;//玩家id
        private CRoleAllInfo m_roleAllInfo = new CRoleAllInfo();//角色所有信息
        private ECampType m_eCampType = ECampType.CAMP_INVALID;//玩家所在阵营
        private EGameType m_eGameType = EGameType.GAME_TYPE_INVALID;//玩家正在的游戏类型
        /// <summary>
        /// 有期限的神兽map
        /// </summary>
        private Dictionary<int, CBeastData> m_dicTempBeastMap = new Dictionary<int, CBeastData>();
        private IXLog m_log = XLog.GetLog<PlayerRole>();
        #endregion
        #region 属性
        /// <summary>
        /// 玩家角色id，登陆成功的时候初始化
        /// </summary>
        public long ID
        {
            get { return this.m_unPlayerId; }
            set { this.m_unPlayerId = value; }
        }
        /// <summary>
        /// 玩家实例
        /// </summary>
        public Player Player
        {
            get
            {
                return Singleton<PlayerManager>.singleton.GetPlayerByID(this.m_unPlayerId);
            }
        }
        /// <summary>
        /// 人物名字
        /// </summary>
        public string Name
        {
            get { return this.m_roleAllInfo.m_oRoleBasicInfo.m_strName; }
            set { this.m_roleAllInfo.m_oRoleBasicInfo.m_strName = value; }
        }
        /// <summary>
        /// 人物图标头像
        /// </summary>
        public string Icon
        {
            get { return this.m_roleAllInfo.m_oRoleBasicInfo.m_strIcon; }

            set { this.m_roleAllInfo.m_oRoleBasicInfo.m_strIcon = value; }
        }
        /// <summary>
        /// 人物等级
        /// </summary>
        public uint Level
        {
            get { return (uint)this.m_roleAllInfo.m_oRoleBasicInfo.m_level; }
            set 
            { 
                this.m_roleAllInfo.m_oRoleBasicInfo.m_level = (int)value;
                DlgBase<DlgLobby, DlgLobbyBehaviour>.singleton.Refresh();
            }
        }
        /// <summary>
        /// 玩家金币
        /// </summary>
        public int Money 
        {
            get { return this.m_roleAllInfo.m_oRoleBasicInfo.m_money; }
            set
            {
                if (value != this.m_roleAllInfo.m_oRoleBasicInfo.m_money)
                {
                    this.m_roleAllInfo.m_oRoleBasicInfo.m_money = value;
                    DlgBase<DlgLobby, DlgLobbyBehaviour>.singleton.Refresh();
                }
            }
        }
        /// <summary>
        /// 玩家点券
        /// </summary>
        public int Ticket 
        {
            get 
            {
                return this.m_roleAllInfo.m_oRoleBasicInfo.m_ticket;
            }
            set 
            {
                if (value != this.m_roleAllInfo.m_oRoleBasicInfo.m_ticket)
                {
                    this.m_roleAllInfo.m_oRoleBasicInfo.m_ticket = value;
                    DlgBase<DlgLobby, DlgLobbyBehaviour>.singleton.Refresh();
                }
            }
        }
        /// <summary>
        /// 角色全部信息
        /// </summary>
        public CRoleAllInfo RoleAllInfo 
        {
            get { return this.m_roleAllInfo; }
            set { this.m_roleAllInfo = value; }
        }
        /// <summary>
        /// 玩家正在进行的游戏类型
        /// </summary>
        public EGameType GameType 
        {
            get { return this.m_eGameType; }
            set { this.m_eGameType = value; }
        }
        /// <summary>
        /// 玩家所在阵营
        /// </summary>
        public ECampType CampType 
        {
            get { return this.m_eCampType; }
            set { this.m_eCampType = value; }
        }
        #endregion
        /// <summary>
        /// 获得召唤师所拥有的神兽数量
        /// </summary>
        public int GetActiveBeastCount()
        {
            return 0;
        }
        /// <summary>
        /// 取得玩家拥有的神兽信息，是否有包括周免
        /// </summary>
        /// <param name="beastTypeId"></param>
        /// <param name="bContainFree"></param>
        /// <returns></returns>
        public CBeastData GetBeastData(int beastTypeId, bool bContainFree)
        {
            CBeastData cBeastData = this.GetBeastData(beastTypeId);
            if (bContainFree)
            {
                CBeastInfo info = this.m_roleAllInfo.m_oBeastInfo;
                if (info != null && info.m_oWeekBeastMap.ContainsKey(beastTypeId))
                {
                    CBeastData cWeekBeastData = info.m_oWeekBeastMap[beastTypeId];
                    if (cBeastData == null)
                    {
                        cBeastData = cWeekBeastData;
                    }
                }
            }
            return cBeastData;
        }
        public CBeastData GetBeastData(int beastTypeId)
        {
            CBeastData result = null;
            CBeastInfo oBeastInfo = this.m_roleAllInfo.m_oBeastInfo;
            if (oBeastInfo != null)
            {
                oBeastInfo.m_oBeastMap.TryGetValue(beastTypeId, out result);
            }
            return result;
        }
        public void AddTempBeastData(CBeastData beastData)
        {
            if (beastData != null)
            {
                this.m_dicTempBeastMap[beastData.m_dwID] = beastData;
            }
        }
        /// <summary>
        /// 是否玩家已经拥有该神兽
        /// </summary>
        /// <param name="unBeastId">神兽id</param>
        /// <param name="bIncludeTemp">是否包括玩家有期限的神兽列表</param>
        /// <param name="bContainFree">是否包括周免神兽</param>
        /// <returns></returns>
        public bool IsBeastActive(int unBeastId,bool bIncludeTemp, bool bContainFree)
        {
            bool flag = this.IsBeastActive(unBeastId, bIncludeTemp);
            if (!flag && bContainFree)
            {
                CBeastInfo info = this.m_roleAllInfo.m_oBeastInfo;
                if (info != null)
                {
                    flag = this.m_roleAllInfo.m_oBeastInfo.m_oWeekBeastMap.ContainsKey(unBeastId);
                }
            }
            return flag;
        }
        /// <summary>
        /// 是否是周免神兽
        /// </summary>
        /// <param name="beastId"></param>
        /// <returns></returns>
        public bool IsFreeBeast(int beastId)
        {
            CBeastInfo info = this.m_roleAllInfo.m_oBeastInfo;
            return info != null && info.m_oWeekBeastMap.ContainsKey(beastId) && info.m_oWeekBeastMap[beastId].m_wLevel > 0;
        }
        #region 私有方法
        private bool IsBeastActive(int unBeastId, bool bIncludeTemp)
        {
            CBeastData data = null;
            Dictionary<int, CBeastData> dictionary = this.m_roleAllInfo.m_oBeastInfo.m_oBeastMap;
            dictionary.TryGetValue((int)unBeastId, out data);
            bool result = false;
            if (data != null && data.m_wLevel > 0)
            {
                result = true;
            }
            else 
            {
                if (!bIncludeTemp)
                {
                    result = false;
                }
                else 
                {
                    this.m_dicTempBeastMap.TryGetValue(unBeastId, out data);
                    result = (data != null && data.m_wLevel > 0);
                }
            }
            return result;
        }
        #endregion
    }
}