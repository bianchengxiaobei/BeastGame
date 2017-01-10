using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：RoleAllInfo
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：角色所有的信息类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CRoleAllInfo : IData
    {
        #region 字段
        public CTaskListInfo m_oTaskListInfo;
        public CRoleBasicInfo m_oRoleBasicInfo;
        public CBeastInfo m_oBeastInfo;
        #endregion
        #region 构造方法
        public CRoleAllInfo()
        {
            this.m_oRoleBasicInfo = new CRoleBasicInfo();
            this.m_oBeastInfo = new CBeastInfo();
            this.m_oTaskListInfo = new CTaskListInfo();
        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_oRoleBasicInfo);
            bs.Write(this.m_oBeastInfo);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(this.m_oRoleBasicInfo);
            bs.Read(this.m_oBeastInfo);
            return bs;
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
