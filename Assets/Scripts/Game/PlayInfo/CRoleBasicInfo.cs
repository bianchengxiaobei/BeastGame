using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CRoleBasicInfo
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：角色基础信息
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CRoleBasicInfo : IData
    {
        #region 字段
        public long m_ID;
        public string m_strAccount;
        public string m_strName;
        public string m_strIcon;
        public int m_level;
        public int m_exp;
        public int m_money;
        public int m_ticket;
        public long m_onlineGTimes;
        public long m_loginTimes;
        #endregion
        #region 构造方法
        public CRoleBasicInfo()
        {

        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_ID);
            bs.Write(this.m_strAccount);
            bs.Write(this.m_strName);
            bs.Write(this.m_strIcon);
            bs.Write(this.m_level);
            bs.Write(this.m_exp);
            bs.Write(this.m_money);
            bs.Write(this.m_ticket);
            bs.Write(this.m_onlineGTimes);
            bs.Write(this.m_loginTimes);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_ID);
            bs.Read(ref this.m_strAccount);
            bs.Read(ref this.m_strName);
            bs.Read(ref this.m_strIcon);
            bs.Read(ref this.m_level);
            bs.Read(ref this.m_exp);
            bs.Read(ref this.m_money);
            bs.Read(ref this.m_ticket);
            bs.Read(ref this.m_onlineGTimes);
            bs.Read(ref this.m_loginTimes);
            return bs;
        }
        public void CopyFrom(CRoleBasicInfo rbi)
        {
 
        }
        public void Reset()
        {
 
        }
        #endregion
        #region 私有方法
        #endregion
    }
}