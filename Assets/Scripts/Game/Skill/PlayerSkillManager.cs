using UnityEngine;
using System.Collections.Generic;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PlayerSkillManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.22
// 模块描述：角色技能管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色技能管理器
/// </summary>
namespace Game
{
    public class PlayerSkillManager : SkillManager
    {
        #region 字段
        //公共cd为200毫秒,也就是0.2秒
        public int CommonCd = 200;
        private float m_fLastAttackTime = 0.0f;
        private int m_iLastSkillId = 0;
        //记录技能释放的最后时刻
        private Dictionary<int, float> skilllastCastTime = new Dictionary<int, float>();
        //技能冷却时间
        private Dictionary<int, int> skillCoolTime = new Dictionary<int, int>();
        //依赖技能（组合技能）
        private Dictionary<int, List<int>> dependenceSkill = new Dictionary<int, List<int>>();
        //组合技能的cd
        private Dictionary<int, int> comboSkillPeriod = new Dictionary<int, int>();
        //技能的公共cd
        private Dictionary<int, int> commonCD = new Dictionary<int, int>();
        private Dictionary<int, int> comboSkillPeriodStart = new Dictionary<int, int>();

        private SkillMapping skillMapping = new SkillMapping();
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public PlayerSkillManager(EntityParent _theOwner) : base(_theOwner)
        {
            theOwner = _theOwner;
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 获取普通攻击id
        /// </summary>
        /// <returns></returns>
        public int GetNormalAttackId()
        {
            int interval = (int)(Time.realtimeSinceStartup - m_fLastAttackTime);
            if (dependenceSkill.ContainsKey(m_iLastSkillId) && this.comboSkillPeriod.ContainsKey(m_iLastSkillId))
            {
                int nextSkill = dependenceSkill[m_iLastSkillId][2];
                int cd = comboSkillPeriodStart[m_iLastSkillId];
                if (commonCD[m_iLastSkillId] > cd)
                {
                    cd = commonCD[m_iLastSkillId];
                }
                if (nextSkill > 0 && interval > cd && interval < this.comboSkillPeriod[m_iLastSkillId])
                {
                    m_iLastSkillId = nextSkill;
                    return nextSkill;
                }
            }
            m_iLastSkillId = skillMapping.normalAttack;
            return skillMapping.normalAttack;
        }
        /// <summary>
        /// 取得第一个位置的技能id
        /// </summary>
        /// <returns></returns>
        public int GetSpellOneId()
        {
            return skillMapping.spellOne;
        }
        /// <summary>
        /// 取得第二个位置的技能id
        /// </summary>
        /// <returns></returns>
        public int GetSpellTwoId()
        {
            return skillMapping.spellTwo;
        }
        /// <summary>
        /// 取得第三个位置的技能id
        /// </summary>
        /// <returns></returns>
        public int GetSpellThreeId()
        {
            return skillMapping.spellThree;
        }
        /// <summary>
        /// 是否技能在冷却当中
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool IsSkillCooldown(int skillId)
        {
            if (!SkillData.dataMap.ContainsKey(skillId))
            {
                return true;
            }
            if (!this.skilllastCastTime.ContainsKey(skillId))
            {
                skilllastCastTime[skillId] = 0;
            }
            int skillInterval = (int)((Time.realtimeSinceStartup - skilllastCastTime[skillId]) * 1000);
            if (!this.skillCoolTime.ContainsKey(skillId))
            {
                skillCoolTime[skillId] = 0;
            }
            if (skillInterval < this.skillCoolTime[skillId])
            {
                Debug.Log("技能正在冷却当中");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否技能有依赖技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool HasDependence(int skillId)
        {
            if (this.dependenceSkill.ContainsKey(skillId) && this.dependenceSkill[skillId][2] > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否在公共cd冷却当中，也就是攻击频率
        /// </summary>
        /// <returns></returns>
        public bool IsCommonCooldown()
        {
            int attackInterval = (int)((Time.realtimeSinceStartup - m_fLastAttackTime) * 1000);
            if (attackInterval < CommonCd)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取得该技能的公共cd
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public int GetCommonCd(int skillId)
        {
            if (!commonCD.ContainsKey(skillId))
            {
                return 0;
            }
            return commonCD[skillId];
        }

        public void ClearComboSkill()
        {
            m_iLastSkillId = 0;
        }
        /// <summary>
        /// 重设该技能的cd
        /// </summary>
        /// <param name="skillId"></param>
        public void ResetCoolTime(int skillId)
        {
            CommonCd = commonCD[skillId];
            m_fLastAttackTime = Time.realtimeSinceStartup;
            skilllastCastTime[skillId] = m_fLastAttackTime;
        }
        #endregion
        #region 私有方法
        #endregion
        #region 析构方法
        #endregion
    }
}
