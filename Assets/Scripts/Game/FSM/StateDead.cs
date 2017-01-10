using UnityEngine;
using System.Collections;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：StateDead
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class StateDead : IState
    {
        public void Enter(EntityParent theOwner, params object[] args)
        {
            theOwner.CurrentMotionState = MotionState.DEAD;
        }
        public void Exit(EntityParent theOwner, params object[] args)
        {
            
        }
        public void Process(EntityParent theOwner, params object[] args)
        {
            int act = (int)ActionConstants.die;
            theOwner.ApplyRootMotion(true);
            string actName = theOwner.CurrActStateName();//当前动画的名称
            if (actName.EndsWith(PlayerActionName.actionOfNames[(int)ActionConstants.hit_air]) || actName.EndsWith("getup"))
            {
                act = (int)ActionConstants.die_knock_down;
                theOwner.SetAction(act);
            }
            else if (actName.EndsWith(PlayerActionName.actionOfNames[(int)ActionConstants.hited_ground]) || actName.EndsWith("knockout"))
            {
                act = (int)ActionConstants.die;
                theOwner.SetAction(act);
            }
            else 
            {

            }
            theOwner.SetSpeed(0);
            EventDispatch.TriggerEvent(Event.LogicSoundEvent.OnHitYelling, theOwner as EntityParent, act);
            if (theOwner is EntityMyself && theOwner.motor)
            {
                theOwner.motor.enableStick = false;
            }
        }
    }
}