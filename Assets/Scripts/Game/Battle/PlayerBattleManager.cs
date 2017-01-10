using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PlayerBattleManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.22
// 模块描述：角色战斗管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色战斗管理器
/// </summary>
namespace Game
{
    public class PlayerBattleManager : BattleManager
    {
        #region 字段
        private List<int> preCmds = new List<int>();
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public PlayerBattleManager(EntityParent _theOwner, SkillManager _skillManager):base(_theOwner,_skillManager)
        {
            m_skillManager = _skillManager;
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 普通攻击
        /// </summary>
        public void NormalAttack()
        {
            if (GameWorld.isInTown || (theOnwer as EntityMyself).DeathFlag == 1)
            {
                return;
            }
            //cd在冷却中
            if ((m_skillManager as PlayerSkillManager).IsCommonCooldown())
            {
                preCmds.Add(0);
                return;
            }
            //取得下一个普攻的id
            int nextSkill = (m_skillManager as PlayerSkillManager).GetNormalAttackId();
            //如果是和当前的技能id一样的话，就直接跳过
            if (nextSkill == theOnwer.currSpellID && theOnwer.currSpellID != -1)
            {
                preCmds.Add(0);
                return;
            }
            if ((m_skillManager as PlayerSkillManager).IsSkillCooldown(nextSkill))
            {
                (m_skillManager as PlayerSkillManager).ClearComboSkill();
                preCmds.Add(0);
                return;
            }
            if (!(m_skillManager as PlayerSkillManager).HasDependence(nextSkill))
            {
                ClearPreSkill();
            }
            (m_skillManager as PlayerSkillManager).ResetCoolTime(nextSkill);
            EntityMyself.preSkillTime = Time.realtimeSinceStartup;
            theOnwer.CastSkill(nextSkill);
            TimerHeap.AddTimer((uint)((m_skillManager as PlayerSkillManager).GetCommonCd(nextSkill)), 0, NextCmd);
        }
        /// <summary>
        /// 释放第一个技能
        /// </summary>
        public void SpellOneAttack()
        {
            ClearPreSkill();
            if (GameWorld.isInTown || (theOnwer as EntityMyself).DeathFlag == 1)
            {
                return;
            }
            //cd在冷却中
            if ((m_skillManager as PlayerSkillManager).IsCommonCooldown())
            {
                preCmds.Add(0);
                return;
            }
            int skillId = (m_skillManager as PlayerSkillManager).GetSpellOneId();
            if ((m_skillManager as PlayerSkillManager).IsSkillCooldown(skillId))
            {
                return;
            }
            (theOnwer as EntityMyself).ClearSkill();
            (m_skillManager as PlayerSkillManager).ClearComboSkill();
            (m_skillManager as PlayerSkillManager).ResetCoolTime(skillId);
            EntityMyself.preSkillTime = Time.realtimeSinceStartup;
            theOnwer.CastSkill(skillId);
            //在技能界面上显示cd
            SkillData data = SkillData.dataMap[skillId];
            //data.cd[0]就是他的cd
        }
        /// <summary>
        /// 释放第二个技能
        /// </summary>
        public void SpellTwoAttack()
        {
            ClearPreSkill();
            if (GameWorld.isInTown || (theOnwer as EntityMyself).DeathFlag == 1)
            {
                return;
            }
            //cd在冷却中
            if ((m_skillManager as PlayerSkillManager).IsCommonCooldown())
            {
                preCmds.Add(0);
                return;
            }
            int skillId = (m_skillManager as PlayerSkillManager).GetSpellTwoId();
            if ((m_skillManager as PlayerSkillManager).IsSkillCooldown(skillId))
            {
                return;
            }
            (theOnwer as EntityMyself).ClearSkill();
            (m_skillManager as PlayerSkillManager).ClearComboSkill();
            (m_skillManager as PlayerSkillManager).ResetCoolTime(skillId);
            EntityMyself.preSkillTime = Time.realtimeSinceStartup;
            theOnwer.CastSkill(skillId);
            //在技能界面上显示cd
            SkillData data = SkillData.dataMap[skillId];
            //data.cd[0]就是他的cd
        }
        public void SpellThreeAttack()
        {
            ClearPreSkill();
            if (GameWorld.isInTown || (theOnwer as EntityMyself).DeathFlag == 1)
            {
                return;
            }
            //cd在冷却中
            if ((m_skillManager as PlayerSkillManager).IsCommonCooldown())
            {
                preCmds.Add(0);
                return;
            }
            int skillId = (m_skillManager as PlayerSkillManager).GetSpellThreeId();
            if ((m_skillManager as PlayerSkillManager).IsSkillCooldown(skillId))
            {
                return;
            }
            (theOnwer as EntityMyself).ClearSkill();
            (m_skillManager as PlayerSkillManager).ClearComboSkill();
            (m_skillManager as PlayerSkillManager).ResetCoolTime(skillId);
            EntityMyself.preSkillTime = Time.realtimeSinceStartup;
            theOnwer.CastSkill(skillId);
            //在技能界面上显示cd
            SkillData data = SkillData.dataMap[skillId];
            //data.cd[0]就是他的cd
        }
        public void ClearPreSkill()
        {
            preCmds.Clear();
        }
        //下一个攻击指令
        public void NextCmd()
        {
            if (preCmds.Count == 0)
            {
                return;
            }
            preCmds.RemoveAt(0);
            NormalAttack();
        }
        #endregion
        #region 私有方法
        #endregion
        #region 析构方法
        #endregion
    }
}
