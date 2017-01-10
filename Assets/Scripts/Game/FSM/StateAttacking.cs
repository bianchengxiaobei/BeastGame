using UnityEngine;
using System.Collections.Generic;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：StateAttacking
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class StateAttacking : IState
    {
        public void Enter(EntityParent theOwner,params object[] args)
        {
            theOwner.CurrentMotionState = MotionState.ATTACKING;
        }
        public void Exit(EntityParent theOwner, params object[] args)
        {
 
        }
        public void Process(EntityParent theOwner, params object[] args)
        {
            if (args.Length != 1)
            {
                Debug.LogError("没有攻击技能");
                return;
            }
            int spellId = (int)args[0];
            SkillData s = SkillData.dataMap[spellId];
            theOwner.motor.speed = 0;
            theOwner.motor.targetSpeed = 0;
            int baseTime = 0;
            for (int i = 0; i < s.skillAction.Count; i++)
            {
                SkillAction action = SkillAction.dataMap[s.skillAction[0]];
                List<object> args1 = new List<object>();
                args1.Add(s.skillAction[0]);
                args1.Add(theOwner.Transform.localToWorldMatrix);
                args1.Add(theOwner.Transform.rotation);
                args1.Add(theOwner.Transform.forward);
                args1.Add(theOwner.Transform.position);
                //播放技能的第一个动作
                if (i == 0)
                {
                    ProcessHit(theOwner, spellId, args1);
                    if (theOwner is EntityMyself)
                    {
                        theOwner.motor.enableStick = action.enableStick > 0;
                    }
                }
                //如果没有后续动作了就跳出循环
                if (i + 1 == s.skillAction.Count)
                {
                    break;
                }
                //记录加到定时器里面的hit动作
                uint tid = 0;
                List<object> args2 = new List<object>();
                args2.Add(s.skillAction[i + 1]);
                args2.Add(theOwner.Transform.localToWorldMatrix);
                args2.Add(theOwner.Transform.rotation);
                args2.Add(theOwner.Transform.forward);
                args2.Add(theOwner.Transform.position);
                if (action.actionTime > 0)
                {
                    tid = TimerHeap.AddTimer((uint)((baseTime + action.actionTime) / theOwner.aiRate), 0, ProcessHit, theOwner, spellId, args2);
                    baseTime += action.actionTime;
                }
                if (action.nextHitTime > 0)
                {
                    tid = TimerHeap.AddTimer((uint)((baseTime + action.nextHitTime) / theOwner.aiRate), 0, ProcessHit, theOwner, spellId, args2);
                    baseTime += action.nextHitTime;
                }
                theOwner.hitTimer.Add(tid);
            }
           
            /*int actionID = (int)args[0];
            SkillActionData action = SkillActionData.dataMap[actionID];
            SkillData skill = SkillData.dataMap[theOwner.currSpellID];
            int duration = action.duration;
            if (duration <= 0 && skill.skillAction.Count > 1 && theOwner.hitActionIdx >= (skill.skillAction.Count - 1))
            {
                if (SkillActionData.dataMap[skill.skillAction[0]].duration <= 0)
                {
                     //攻击结束，进入idle状态
                    theOwner.AddCallbackInFrames<EntityParent>((_theOwner) => 
                    {
                        
                    },theOwner);
                }
            }
            else if (duration > 0 && action.action > 0)
            {
                TimerHeap.AddTimer<int, EntityParent>((uint)duration, 0, (_actionID,_theOwner) => 
                {
                    GameMotor theMotor = _theOwner.motor;
                    if (_theOwner.Transform)
                    {
                        theMotor.enableStick = true;//驱动设置为静止
                        theMotor.SetExtraSpeed(0);//额外速度为0
                        theMotor.SetMoveDirection(Vector3.zero);//移动方向为（0，0，0）
                    }
                    _theOwner.ChangeMotionState(MotionState.IDLE);//改变状态为idle
                },actionID,theOwner);
            }
            if (action.duration > 0)
            {
                TimerHeap.AddTimer<int, EntityParent>((uint)action.duration, 0, (_actionID, _theOwner) =>
                {

                },actionID,theOwner);
            }*/
        }
        private void ProcessHit(EntityParent theOwner,int spellId,List<object> args)
        {
            int actionId = (int)args[0];
            UnityEngine.Matrix4x4 ltwm = (UnityEngine.Matrix4x4)args[1];
            UnityEngine.Quaternion rotation = (UnityEngine.Quaternion)args[2];
            UnityEngine.Vector3 forward = (UnityEngine.Vector3)args[3];
            UnityEngine.Vector3 position = (UnityEngine.Vector3)args[4];
            if (theOwner is EntityDummy && theOwner.animator != null)
            {
                theOwner.animator.applyRootMotion = true;
            }
            SkillAction action = SkillAction.dataMap[actionId];
            SkillData skill = SkillData.dataMap[spellId];
            int duration = action.duration;
            if (duration <= 0 && skill.skillAction.Count > 1)
            {
                if (SkillAction.dataMap[skill.skillAction[0]].duration <= 0)
                {
                    //攻击结束，进入idle状态
                    theOwner.AddCallbackInFrames<int,EntityParent>((_actionId,_theOwner) =>
                    {
                        
                    },actionId,theOwner);
                }
            }
            else if (duration > 0 && action.action > 0)
            {
                TimerHeap.AddTimer<int, EntityParent>((uint)duration, 0, (_actionID, _theOwner) =>
                {
                    GameMotor theMotor = _theOwner.motor;
                    if (_theOwner.Transform)
                    {
                        theMotor.enableStick = true;
                        theMotor.SetExtraSpeed(0);//额外速度为0
                        theMotor.SetMoveDirection(Vector3.zero);//移动方向为（0，0，0）
                    }
                    _theOwner.ChangeMotionState(MotionState.IDLE);//改变状态为idle
                }, actionId, theOwner);
            }
            //移除技能特效
            if (action.duration > 0)
            {
                TimerHeap.AddTimer<int, EntityParent>((uint)action.duration, 0, (_actionID, _theOwner) =>
                {
                    _theOwner.RemoveSfx(_actionID);
                }, actionId, theOwner);
            }
            theOwner.OnAttacking(actionId, ltwm, rotation, forward, position);
        }
    }
}
