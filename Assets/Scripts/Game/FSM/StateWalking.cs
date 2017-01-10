using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：StateWalking
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：Walking状态
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class StateWalking : IState
    {
        public void Enter(EntityParent theOwner, params object[] args)
        {
            theOwner.CurrentMotionState = MotionState.WALKING;
            if (theOwner is EntityMyself)
            {
                theOwner.animator.speed = theOwner.moveSpeedRate * theOwner.gearMoveSpeedRate;
            }
        }
        public void Exit(EntityParent theOwner, params object[] args)
        {
            theOwner.ApplyRootMotion(true);
            theOwner.SetSpeed(1);
            if (theOwner is EntityBeast)
            {
                theOwner.motor.SetExtraSpeed(0);
                theOwner.motor.isMovingToTarget = false;
                return;
            }
            if (theOwner is EntityMyself)
            {
                theOwner.animator.speed = 1;
            }
        }
        public void Process(EntityParent theOwner, params object[] args)
        {
            GameMotor theMotor = theOwner.motor;
            if (theOwner is EntityBeast || (theOwner is EntityPlayer && !(theOwner is EntityMyself)))
            {
                theOwner.ApplyRootMotion(false);
                theOwner.SetSpeed(1);
                theMotor.SetSpeed(0.4f);
                if (theOwner.Speed == 0)
                {
                    theMotor.SetExtraSpeed(6);
                }
                else
                {
                    theMotor.SetExtraSpeed(theOwner.Speed);
                }
                return;
            }
            else 
            {                                                                        
                theOwner.ApplyRootMotion(true);
                theMotor.SetSpeed(0.4f);
                theMotor.SetExtraSpeed(0.4f);
            }
            theMotor.isMovable = true;
        }
    }
}