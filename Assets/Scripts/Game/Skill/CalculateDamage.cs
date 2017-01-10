using UnityEngine;
using System.Collections.Generic;
using System;
using Utility;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CalculateDamage
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.20
// 模块描述：计算技能伤害类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CalculateDamage
    {
        public static List<int> CacuDamage(int hitActionID, uint attackerID, uint victimID)
        {
            List<int> result = new List<int>();
            EntityParent attacker = null;//攻击者
            EntityParent victimer = null;//受击者
            double levelCorrect = 1.00f;
            if (attackerID == GameWorld.thePlayer.ID && GameWorld.Entities.ContainsKey(victimID))
            {
                //如果攻击者刚好是主角，受击者刚好在场景中
                attacker = GameWorld.thePlayer;
                victimer = GameWorld.Entities[victimID] as EntityDummy;
                double levelGap = victimer.Level - attacker.Level;
                if (levelGap >= 20)
                {
                    levelCorrect = 0.1f;
                }
                else if (levelGap > 10 && levelGap < 20)
                {
                    levelCorrect = 1 - levelGap * 0.05f;
                }
            }
            else if (victimID == GameWorld.thePlayer.ID && GameWorld.Entities.ContainsKey(attackerID))
            {
                //如果攻击者是客户端怪物，受击者是主角
                attacker = GameWorld.Entities[attackerID];
                victimer = GameWorld.thePlayer;
            }

            bool critFlag = false;//是否发生暴击
            var atk = GetProperty(attacker, "Attack");//角色的攻击力
            double extraCritDamage = 0.00f;//暴击伤害
            if (RandomHelper.GetRandomFloat() <= GetCritRate(attacker))
            {
                critFlag = true;
                extraCritDamage = GetProperty(attacker,"critExtraAttack");
            }
            //1miss 2暴击 3普通攻击
            int retFlag;
            if (critFlag)
            {
                retFlag = 2;
            }
            else 
            {
                retFlag = 3;
            }
            SkillAction skillData = SkillAction.dataMap[hitActionID];
            int skillDamage = skillData.damage;
            int addDamage = (int)CacuAddDamage(skillData,attacker,victimer);
            result.Add(retFlag);
            result.Add(skillDamage + addDamage);
            return result;
        }
        /// <summary>
        /// 取得实体的暴击率
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <returns></returns>
        public static double GetCritRate(EntityParent attacker)
        {
            var crit = GetProperty(attacker, "critRate");
            return crit;
        }
        /// <summary>
        /// 返回技能加成伤害
        /// </summary>
        /// <param name="skillData"></param>
        /// <param name="attacker"></param>
        /// <param name="victimer"></param>
        /// <returns></returns>
        public static double CacuAddDamage(SkillAction skillData,EntityParent attacker,EntityParent victimer)
        {
            if (skillData.damageAddType == (byte)damageAddType.AD)
            {
                var atk = GetProperty(attacker, "Attack");
                return skillData.damageAdd * atk;
            }
            else if (skillData.damageAddType == (byte)damageAddType.AP)
            {
                var ap = GetProperty(attacker, "AbilityPower");
                return skillData.damageAdd * ap;
            }
            else if (skillData.damageAddType == (byte)damageAddType.AR)
            {
                var ar = GetProperty(attacker, "Armor");
                return skillData.damageAdd * ar;
            }
            else if (skillData.damageAddType == (byte)damageAddType.MyselfHP)
            {
                var hp = GetProperty(attacker, "HP");
                return skillData.damageAdd * hp;
            }
            else if (skillData.damageAddType == (byte)damageAddType.OhterHP)
            {
                if (null == victimer)
                {
                    var hp = GetProperty(attacker, "HP");
                    return skillData.damageAdd * hp;
                }
                else 
                {
                    var hp = GetProperty(victimer, "HP");
                    return skillData.damageAdd * hp;
                }
            }
            return 0;
        }


        /// <summary>
        /// 根据属性名从实体属性中找到对应的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attrName"></param>
        /// <returns>属性值</returns>
        private static double GetProperty(EntityParent entity, string attrName)
        {
            double value = 0;
            var prop = entity.GetType().GetProperty(attrName);
            if (prop != null)
            {
                value = Convert.ToDouble(prop.GetGetMethod().Invoke(entity, null));
            }
            else 
            {
                //如果没有这个属性名，就从DoubleAttrs<string,double>中找，如果在没有去IntAttr中找，在没有就为0
                value = entity.DoubleAttrs.GetValueOrDefault(attrName, entity.IntAttrs.GetValueOrDefault(attrName, 0));
            }
            return value;
        }
    }
    public enum damageAddType :byte
    {
        AD = 1,
        AP = 2,
        MyselfHP = 3,
        OhterHP = 4,
        AR = 5,//护甲
        MR = 6//魔抗
    }
}
