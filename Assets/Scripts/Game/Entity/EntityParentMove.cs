using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityParentMove 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：实体基类移动处理部分
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 实体基类移动处理部分
/// </summary>
namespace Game
{
    public partial class EntityParent
    {
        public virtual void Move()
        {
            if (this is EntityMyself && (this as EntityMyself).DeathFlag == 1)
            {
                return;
            }
            if (m_battleManager == null)
            {
                ChangeMotionState(MotionState.WALKING);
            }
            else
            {
                this.m_battleManager.Move();
            }
        }
        public virtual void TurnTo(float x, float y, float z)
        {
            if (currentMotionState == MotionState.DEAD)
            {
                return;
            }
            if (motor)
            {
                motor.RotateTo(y);
            }
        }
        public virtual void MoveTo(float x,float y,float z)
        {
            if (currentMotionState == MotionState.DEAD)
            {
                return;
            }
            if (motor == null || Mathf.Abs(x - Transform.position.x) < 0.1f && Mathf.Abs(z - Transform.position.z) < 0.1f)
            {
                return;
            }
            Vector3 v = new Vector3(x, y, z);
            if (this is EntityMyself)
            {
                if (Mathf.Abs(x - Transform.position.x) < 0.3f && Mathf.Abs(z - Transform.position.z) < 0.3f)
                {
                    return;
                }
                if (motor.MoveToByNav(v))
                {
                    Move();
                }
            }
        }
        public virtual void MoveTo(float x, float z, float dx, float dy, float dz)
        {
            if (!Transform)
                return;
            if (currentMotionState == MotionState.DEAD) return;
            if (Mathf.Abs(x - Transform.position.x) < 0.1f && Mathf.Abs(z - Transform.position.z) < 0.1f)
            {
                TurnTo(dx, dy, dz);
            }
            else
            {
                MoveTo(x, z);
            }
        }
        public virtual void MoveTo(float x, float z)
        {
            if (currentMotionState == MotionState.DEAD) return;
            MoveTo(x, 0, z);
        }
    }
}
