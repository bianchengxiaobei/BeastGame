using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：FSMMotion
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：行为状态机管理器
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class FSMMotion : FSMParent
    {
        public FSMMotion()
        {
            m_theFSM.Add(MotionState.IDLE, MotionStateSet.stateIdle);
            m_theFSM.Add(MotionState.WALKING, MotionStateSet.stateWalking);
        }
        public override void ChangeStatus(EntityParent owner, string newState, params object[] args)
        {
            if (owner.CurrentMotionState == newState && newState != MotionState.ATTACKING)
            {
                return;
            }
            if (!m_theFSM.ContainsKey(newState))
            {
                return;
            }
            m_theFSM[owner.CurrentMotionState].Exit(owner, args);
            m_theFSM[newState].Enter(owner, args);
            m_theFSM[newState].Process(owner,args);
        }
    }
    public static class MotionStateSet 
    {
        public static StateIdle stateIdle = new StateIdle();
        public static StateWalking stateWalking = new StateWalking();
    }
    public static class MotionState 
    {
        static readonly public string IDLE = "idle";
        static readonly public string WALKING = "walking";
        static readonly public string DEAD = "dead";
        static readonly public string CHARGING = "charging";
        static readonly public string ATTACKING = "attacking";
        static readonly public string HIT = "hit";
        static readonly public string PREPARING = "preparing";
        static readonly public string ROLL = "roll";

        static readonly public string LOCKING = "locking";
        static readonly public string PICKING = "picking";
    }
    
}