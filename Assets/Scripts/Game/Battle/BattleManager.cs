using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BattleManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.20
// 模块描述：战斗管理器
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public static class BattleAttr
    {
        public static string Attack = "Attack";
    }
    public class BattleManager
    {
        #region 字段
        protected EntityParent theOnwer;
        protected SkillManager m_skillManager;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public BattleManager(EntityParent _theOwner, SkillManager _skillManager)
        {
            this.theOnwer = _theOwner;
            this.m_skillManager = _skillManager;
        }
        #endregion
        #region 公有方法
        #endregion
        #region 子类重写方法
        /// <summary>
        /// 死亡状态
        /// </summary>
        /// <param name="hitActionId"></param>
        public virtual void OnDead(int hitActionId)
        {
            theOnwer.ChangeMotionState(MotionState.DEAD, hitActionId);
        }
        /// <summary>
        /// 攻击状态，播放攻击动作
        /// </summary>
        /// <param name="nSkillID"></param>
        /// <param name="ltwm"></param>
        /// <param name="rotation"></param>
        /// <param name="forward"></param>
        /// <param name="position"></param>
        public virtual void OnAttacking(int nSkillID, Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position)
        {
            m_skillManager.OnAttacking(nSkillID, ltwm, rotation, forward, position);
        }
        /// <summary>
        /// 释放技能状态
        /// </summary>
        /// <param name="skillId"></param>
        public virtual void CastSkill(int skillId)
        {
            if (theOnwer.CurrentMotionState == MotionState.DEAD
               || theOnwer.CurrentMotionState == MotionState.HIT
               || theOnwer.CurrentMotionState == MotionState.PICKING)
            {
                return;
            }
            theOnwer.ChangeMotionState(MotionState.ATTACKING, skillId);
        }
        /// <summary>
        /// 行走状态
        /// </summary>
        public virtual void Move()
        {
            if (theOnwer.CurrentMotionState == MotionState.DEAD
                || theOnwer.CurrentMotionState == MotionState.ATTACKING
                || theOnwer.CurrentMotionState == MotionState.HIT
                || theOnwer.CurrentMotionState == MotionState.PICKING)
            {
                return;
            }
            theOnwer.ChangeMotionState(MotionState.WALKING);
        }
        /// <summary>
        /// Idle状态
        /// </summary>
        public virtual void Idle()
        {
            if (theOnwer.CurrentMotionState == MotionState.DEAD
              || theOnwer.CurrentMotionState == MotionState.ATTACKING
              || theOnwer.CurrentMotionState == MotionState.HIT
              || theOnwer.CurrentMotionState == MotionState.PICKING)
            {
                return;
            }
            theOnwer.ChangeMotionState(MotionState.IDLE);
        }
        #endregion
        #region 私有方法
        #endregion
    }
}