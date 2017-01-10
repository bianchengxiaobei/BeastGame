using UnityEngine;
using System.Collections.Generic;
using Client.Data;
using Utility.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SkillManager
// 创建者：chen
// 修改者列表：
// 创建日期：2015.12.24
// 模块描述：技能管理系统
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class SkillManager
    {
        public static bool showSkillRange = false;//是否显示技能的范围
        protected EntityParent theOwner;//技能管理器拥有者
        #region 字段
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public SkillManager(EntityParent owner)
        {
            this.theOwner = owner;
        }
        #endregion
        #region 公有方法
        public void AttackEffect(int hitActionID, Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position)
        {
            SkillAction s = SkillAction.dataMap[hitActionID];
            if (s.damageFlag == 0)//等于1才有伤害
            {
                return;
            }
            List<List<uint>> list = GetHitEntities(hitActionID, ltwm, rotation, forward, position);
            if (list.Count != 4)
            {
                return;
            }
            if (theOwner is EntityMyself && list[0].Count > 0)
            {

            }

        }
        /// <summary>
        /// 发出攻击
        /// </summary>
        /// <param name="hitActionId"></param>
        /// <param name="itwm"></param>
        /// <param name="rotation"></param>
        /// <param name="forward"></param>
        /// <param name="position"></param>
        public void OnAttacking(int hitActionId, Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position)
        {
            SkillAction action = SkillAction.dataMap[hitActionId];
            if (action.removeCollider == 1)
            {

            }
            //动画动作
            int act = action.action;
            if (act > 0)
            {
                if (PlayerActionNames.animatorNames.ContainsKey(act))
                {
                    theOwner.skillActName = PlayerActionNames.animatorNames[act];
                }
                else
                {
                    Debug.LogWarning("找不到动作id");
                    theOwner.skillActName = "";
                }
                theOwner.SetAction(act);
            }
            AttackingFx(action);
            AttackingMove(action);
            AttackBuff(action);

            List<object> args = new List<object>();
            args.Add(ltwm);
            args.Add(rotation);
            args.Add(forward);
            args.Add(position);
            theOwner.delayAttackTimerID = TimerHeap.AddTimer((uint)(action.actionBeginDuration / theOwner.aiRate), 0, DelayAttack, hitActionId, args);
        }
        #endregion 
        #region 私有方法
        /// <summary>
        /// 根据技能id，获取到受击者的列表
        /// 返回值 是一个三元组。分别是 dummy list, monster list, player list
        /// </summary>
        /// <param name="hitActionID"></param>
        /// <param name="ltwm"></param>
        /// <param name="rotation"></param>
        /// <param name="forward"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private List<List<uint>> GetHitEntities(int hitActionID, Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position)
        {
            var spellData = SkillAction.dataMap[hitActionID];
            //技能实施的目标类型，0敌人，1自己，2队友，3友方
            int targetType = spellData.targetType;
            //技能范围类型。0 扇形 1 圆形 2单体 3直线 4前方
            int targetRangeType = spellData.targetRangeType;
            //攻击范围参数。 针对不同类型，有不同的意义。 浮点数列表
            List<float> targetRangeParam = spellData.targetRangeParam;
            float offsetX = spellData.hitXoffset;
            float offsetY = spellData.hitYoffset;
            float angleOffset = 180;
            List<List<uint>> entities = new List<List<uint>>();
            if (targetType == (int)TargetType.Myself)
            {
                List<uint> listDummy = new List<uint>();
                List<uint> listMonster = new List<uint>();
                List<uint> listPlayer = new List<uint>();
                List<uint> listMercenary = new List<uint>();
                listPlayer.Add(theOwner.ID);
                entities.Add(listDummy);
                entities.Add(listMonster);
                entities.Add(listPlayer);
                entities.Add(listMercenary);
                return entities;
            }
            if (theOwner.Transform == null)
            {
                return entities;
            }
            Matrix4x4 entityltwm = theOwner.Transform.localToWorldMatrix;
            Quaternion entityrotation = theOwner.Transform.rotation;
            Vector3 entityforward = theOwner.Transform.forward;
            Vector3 entityposition = theOwner.Transform.position;
            //如果技能释放的位置不是自身，当castPosType为0的时候，不是自身
            if (spellData.castPosType == 0)
            {
                entityltwm = ltwm;
                entityrotation = rotation;
                entityforward = forward;
                entityposition = position;
            }
            TargetRangeType rangeType = (TargetRangeType)targetRangeType;
            switch (rangeType)
            {
                //圆形
                case TargetRangeType.CircleRange:
                    if (targetRangeParam.Count >= 1)
                    {
                        float radius = targetRangeParam[0] * 0.01f;//参数1为范围半径
                        if (spellData.castPosType == 2 && theOwner is EntityDummy)
                        {
                            EntityParent e = theOwner.GetTargetEntity();
                            if (e != null)
                            {
                                entities = UnityTools.GetEntityInRange(e.Transform.position, radius, offsetX, offsetY, angleOffset);
                            }
                        }
                        else
                        {
                            entities = UnityTools.GetEntityInRange(entityltwm, entityrotation, entityforward, entityposition, radius, offsetX, offsetY, angleOffset);
                        }
                    }
                    break;
                //直线
                case TargetRangeType.LineRange:
                    if (targetRangeParam.Count >= 2)
                    {
                        float length = targetRangeParam[0] * 0.01f;
                        float width = targetRangeParam[1] * 0.01f;
                        entities = UnityTools.GetEnitiesForntLineNew(entityltwm, entityrotation, entityforward, entityposition, length, entityforward, width, offsetX, offsetY, angleOffset);
                    }
                    break;
            }
            return entities;
        }
        /// <summary>
        /// 显示攻击特效
        /// </summary>
        /// <param name="action"></param>
        private void AttackingFx(SkillAction action)
        {
            if (!GameWorld.isShowSkillFx)
            {
                return;
            }
            theOwner.PlaySfx(action.id);
            if (action.cameraTweenId > 0)
            {
                //如果有震屏，调用摄像机脚本的震屏API接口
            }
            if (string.IsNullOrEmpty(action.sound))
            {
                //技能释放音效
            }
        }
        /// <summary>
        /// 释放技能的时候移动
        /// </summary>
        /// <param name="action"></param>
        private void AttackingMove(SkillAction action)
        {
            GameMotor motor = theOwner.motor;
            if (motor == null)
            {
                return;
            }
            float extraSpeed = action.extraSpeed;
            if (extraSpeed != 0)
            {
                motor.SetExtraSpeed(extraSpeed);
                motor.SetMoveDirection(theOwner.Transform.forward);
                //延迟extraSt时间后，设置速度为0
                TimerHeap.AddTimer<GameMotor>((uint)action.extraSt, 0, (m) => { m.SetExtraSpeed(0); }, motor);
            }
            else
            {
                motor.SetExtraSpeed(0);
            }
            //如果该技能是带传送的，直接传送
            if (action.teleportDistance > 0 && extraSpeed <= 0)
            {
                Vector3 dst = Vector3.zero;
                dst = theOwner.Transform.position + theOwner.Transform.forward * action.teleportDistance;
                motor.TeleportTo(dst);
            }
        }
        /// <summary>
        /// 技能增加buff
        /// </summary>
        /// <param name="action"></param>
        private void AttackBuff(SkillAction action)
        {
            if (action.casterAddBuff != null)
            {
                foreach (var id in action.casterAddBuff)
                {
                    theOwner.ClientAddBuff(id);
                }
            }
            if (action.casterDelBuff != null)
            {
                foreach (var id in action.casterDelBuff)
                {
                    theOwner.ClientDelBuff(id);
                }
            }
        }
        /// <summary>
        /// 延迟攻击
        /// </summary>
        /// <param name="hitActionId"></param>
        /// <param name="args"></param>
        private void DelayAttack(int hitActionId, List<object> args)
        {

        }
        /// <summary>
        /// 攻击客户端怪物
        /// </summary>
        /// <param name="hitActionId"></param>
        /// <param name="beasts"></param>
        private void AttackBeast(int hitActionId, List<uint> beasts)
        {
            //受伤者列表
            Dictionary<uint, List<int>> wounded = new Dictionary<uint, List<int>>();
            for (int i = 0; i < beasts.Count; i++)
            {
                List<int> harm = new List<int>();
                if (!GameWorld.Entities.ContainsKey(beasts[i]))
                {
                    continue;
                }
                EntityParent e = GameWorld.Entities[beasts[i]];
                //如果怪物不能被击中或者已经死亡
                if (UnityTools.BitFlag(e.StateFlag, StateCfg.NO_HIT_STATE) == 1 || UnityTools.BitFlag(e.StateFlag, StateCfg.DEATH_STATE) == 1)
                {
                    continue;
                }
                harm = CalculateDamage.CacuDamage(hitActionId, theOwner.ID, beasts[i]);
                wounded.Add(beasts[i], harm);
                uint demage = 0;
                demage = e.CurHp < harm[1] ? e.CurHp : (uint)harm[1];
                e.CurHp -= demage;
            }
        }
        #endregion
    }
}
