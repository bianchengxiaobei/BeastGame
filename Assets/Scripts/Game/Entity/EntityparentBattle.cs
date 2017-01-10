using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityparentBattle
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
        #region 子类重写方法
        public virtual void OnDeath(int hitActionId)
        {
            if (this.m_battleManager != null)
            {
                this.m_battleManager.OnDead(hitActionId);
            }
        }
        public virtual void OnAttacking(int actionID, Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position)
        {
            if (this.m_battleManager != null)
            {
                this.m_battleManager.OnAttacking(actionID, ltwm, rotation, forward, position);
            }
        }
        /// <summary>
        /// 带方向释放技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="rotation"></param>
        public virtual void CastSkill(int skillId, Vector3 rotation)
        {

        }
        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skillId"></param>
        public virtual void CastSkill(int skillId)
        {
            walkingCastSkill = (currentMotionState == MotionState.WALKING);
            currSpellID = skillId;
            SkillData data = SkillData.dataMap[currSpellID];
            if (data == null || aiRate == 0)
            {
                return;
            }
            if (ID == GameWorld.thePlayer.ID)
            {
                float x = Transform.position.x;
                float z = Transform.position.z;
                byte angle = (byte)(Transform.eulerAngles.y * 0.5);
                //发送释放技能到服务器(坐标，方向，技能id)

            }
            m_battleManager.CastSkill(currSpellID);
        }
        /// <summary>
        /// 清除技能,到Idle状态
        /// </summary>
        /// <param name="remove"></param>
        /// <param name="naturalEnd"></param>
        public virtual void ClearSkill(bool remove = false, bool naturalEnd = false)
        {
            TimerHeap.DelTimer(hitTimerID);
            TimerHeap.DelTimer(delayAttackTimerID);
            if (currSpellID != -1)
            {
                if (SkillAction.dataMap.ContainsKey(currHitAction) && remove)
                {
                    RemoveSfx(currHitAction);
                }
                SkillData data;
                if (SkillData.dataMap.TryGetValue(currSpellID, out data) && remove)
                {
                    foreach (var action in data.skillAction)
                    {
                        RemoveSfx(action);
                    }
                }
                currHitAction = -1;
            }
            hitTimer.Clear();
            GameMotor theMotor = motor;
            if (Transform)
            {
                theMotor.enableStick = true;
                theMotor.enableRotation = true;
                theMotor.SetExtraSpeed(0);
                theMotor.SetMoveDirection(Vector3.zero);
            }
            ChangeMotionState(MotionState.IDLE);
            currSpellID = -1;
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 当前动画动作的名称
        /// </summary>
        /// <returns></returns>
        public string CurrActStateName()
        {
            if (null == this.animator)
            {
                return "";
            }
            var state = this.animator.GetCurrentAnimationClipState(0);
            if (state.Length == 0)
            {
                return "";
            }
            return state[0].clip.name;
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
