using UnityEngine;
using System.Collections;
using Utility;
using Game;
using System.Collections.Generic;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SkillNormalAttack 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.21
// 模块描述：战棋技能-普通攻击
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋技能-普通攻击
/// </summary>
namespace Client.Skill
{
    public class SkillNormalAttack : SkillBase
    {
        #region 构造方法
        public SkillNormalAttack()
        {
            this.m_unskillId = 1;
        }
        #endregion
        #region 公共方法
        #endregion
        #region 重写方法
        public override int GetUseDistance(long BeastId)
        {
            Beast beast = Singleton<BeastManager>.singleton.GetBeastById(BeastId);
            if (beast != null)
            {
                return beast.MaxAttackDis;
            }
            else
            {
                return 0;
            }
        }
        public override List<CVector3> GetValidTargetHexs(long unMasterBeastId)
        {
            List<CVector3> listTargetHex = new List<CVector3>();
            Beast beast = Singleton<BeastManager>.singleton.GetBeastById(unMasterBeastId);
            if (beast == null || beast.IsError)
            {
                return listTargetHex;
            }
            else
            {
                int maxAttackDis = beast.MaxAttackDis;
                SkillBase skill = SkillGameManager.GetSkillBase(this.m_unskillId);
                if (skill != null)
                {
                    List<long> list = new List<long>();
                    Singleton<ClientMain>.singleton.scene.GetNearEnemys(unMasterBeastId,maxAttackDis,ref list);
                    foreach (var id in list)
                    {
                        Beast beastById = Singleton<BeastManager>.singleton.GetBeastById(id);
                        if (beastById != null)
                        {
                            listTargetHex.Add(beastById.Pos);
                        }
                    }
                }
                /*
                ECampType nCamp = ECampType.CAMP_EMPIRE;
                if (Singleton<BeastRole>.singleton.CampType == ECampType.CAMP_EMPIRE)
                {
                    nCamp = ECampType.CAMP_LEAGUE;
                }
                CVector3 basePos = Singleton<ClientMain>.singleton.
                */
                List<CVector3> targetList = new List<CVector3>();
                Singleton<ClientMain>.singleton.scene.GetNearNodesIgnoreObstruct(1, 1, beast.Pos, ref targetList, true, true);
                targetList.ForEach(delegate (CVector3 hex)
                {
                    if (listTargetHex.Contains(hex))
                    {
                        listTargetHex.Remove(hex);
                    }
                });
                return listTargetHex;
            }
        }
        public override List<long> GetValidTargetPlayers(long unMasterBeastId)
        {
            return base.GetValidTargetPlayers(unMasterBeastId);
        }
        public override string GetSkillAnimName(long beastId)
        {
            string attack = "attack";
            return attack;
        }       
        #endregion
    }
}