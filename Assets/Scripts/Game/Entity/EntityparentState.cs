using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityparentState
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public partial class EntityParent
    {
        #region 字段
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        /// <summary>
        /// Entity能否移动
        /// </summary>
        /// <returns></returns>
        public bool CanMove()
        {
            return this.canMove;
        }
        /// <summary>
        /// 通过StateFlag设置Action为idle动作
        /// </summary>
        public void SetActionByStateFlagInIdleState()
        {
            if ((this.stateFlag & dizzy_state) != 0)//也就是stateFlag的第二位必须为1
            {
                SetAction(16);
                AddCallbackInFrames(() => { SetAction(999); });
            }
        }
        #endregion
        #region 子类重写方法
        protected virtual void StateChange(ulong value)
        {

        }
        #endregion
    }
}