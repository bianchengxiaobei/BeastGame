using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CRoleStatInfo
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：角色战斗方面总信息
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CRoleStateInfo : IData
    {
        #region 字段
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            return bs;
        }
        #endregion
    }
}
