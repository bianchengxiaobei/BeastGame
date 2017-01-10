using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：FSMParent
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：行为状态机管理器
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public abstract class FSMParent
    {
        #region 字段
        protected Dictionary<string, IState> m_theFSM = new Dictionary<string, IState>();
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public FSMParent()
        {
            
        }
        #endregion
        #region 公有方法
        public virtual void ChangeStatus(EntityParent owner, string newState, params object[] args)
        {
 
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
