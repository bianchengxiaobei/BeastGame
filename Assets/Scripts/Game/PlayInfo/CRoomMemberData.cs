using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CRoomMemberData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.1
// 模块描述：房间内成员数据信息
/----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CRoomMemberData : IData
    {
        #region 字段
        /// <summary>
        /// 玩家id
        /// </summary>
        public long m_unPlayerID;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int m_unLevel;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string m_strName;
        /// <summary>
        /// 玩家头像
        /// </summary>
        public string m_strIcon;
        /// <summary>
        /// 玩家神兽列表
        /// </summary>
        public List<long> m_lBeastList;
        /// <summary>
        /// 是否玩家是重新连接的,如果是1的话，表示是重连
        /// </summary>
        public byte m_btIsReconnecting;
        public Dictionary<int, CBeastData> m_oBeastMap;
        #endregion
        #region 构造方法
        public CRoomMemberData()
        {
            this.m_unPlayerID = 0;
            this.m_unLevel = 0;
            this.m_strName = "";
            this.m_strIcon = "";
            this.m_lBeastList = new List<long>();
            this.m_oBeastMap = new Dictionary<int, CBeastData>();
            this.m_btIsReconnecting = 0;
        }
        public CRoomMemberData(long playerID, int level, string name, string icon, byte isReconnect)
        {
            this.m_unPlayerID = playerID;
            this.m_unLevel = level;
            this.m_strName = name;
            this.m_strIcon = icon;
            this.m_lBeastList = new List<long>();
            this.m_oBeastMap = new Dictionary<int, CBeastData>();
            this.m_btIsReconnecting = isReconnect;
        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {         
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_unPlayerID);
            bs.Read(ref this.m_unLevel);
            bs.Read(ref this.m_strName);
            bs.Read(ref this.m_strIcon);
            bs.Read(ref this.m_btIsReconnecting);
            bs.Read(this.m_lBeastList);
            this.m_oBeastMap.Clear();
            int num = 0;
            bs.Read(ref num);
            for (int i = 0; i < num; i++)
            {
                int key = 0;
                CBeastData data = new CBeastData();
                bs.Read(ref key);
                bs.Read(data);
                this.m_oBeastMap.Add(key, data);
            }
            return bs;
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
