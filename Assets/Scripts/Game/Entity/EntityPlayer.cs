using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityPlayer
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class EntityPlayer : EntityParent
    {
        #region 字段
        #endregion
        #region 属性
        #endregion
        #region 重写方法
        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
            itemInfo = new Hashtable();
        }
        #endregion
        #region 公有方法
        #endregion
        #region 私有方法
        #endregion
    }
}
