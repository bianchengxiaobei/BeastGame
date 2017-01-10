using UnityEngine;
using System.Collections.Generic;
using Utility;
using GameData;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CScene
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：场景地图类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CScene : SceneBase
    {
        #region 属性和字段
        private int m_nMyBaseDeadEft;
        private int m_nTheirBaseDeadEft;
        public int m_nLeagueAttackedEffect;
        public int m_nEmpireAttackedEffect;
        private int m_nEmpireBaseDeadEft;
        private int m_nLeagueBaseDeadEft;
        public int m_nLeagueHighHpEffect;
        public int m_nEmpireHighHpEffect;
        public int m_nLeagueLowHpEffect;
        public int m_nEmpireLowHpEffect;
        public int m_nLeagueEffectWhenBeastDead;
        public int m_nEmpireEffectWhenBeastDead;
        #endregion
        #region 公有方法
        /// <summary>
        /// 根据阵营类型获取基地被攻击的特效
        /// </summary>
        /// <param name="campType">阵营类型</param>
        /// <returns>特效id</returns>
        public int GetBaseBeAttackedEffect(ECampType campType)
        {
            int result;
            if (campType == ECampType.CAMP_EMPIRE)
            {
                result = this.m_nEmpireAttackedEffect;
            }
            else if (campType == ECampType.CAMP_LEAGUE)
            {
                result = this.m_nLeagueAttackedEffect;
            }
            else
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 根据阵营类型获取基地高血量的特效，如果阵营不存在就放回0
        /// </summary>
        /// <param name="campType">阵营类型</param>
        /// <returns></returns>
        public int GetBaseHighHpEffect(ECampType campType)
        {
            int result;
            if (campType == ECampType.CAMP_EMPIRE)
            {
                result = this.m_nEmpireHighHpEffect;
            }
            else if (campType == ECampType.CAMP_LEAGUE)
            {
                result = this.m_nLeagueHighHpEffect;
            }
            else 
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 根据阵营类型获取基地低血量的特效
        /// </summary>
        /// <param name="campType">阵营类型</param>
        /// <returns>特效id</returns>
        public int GetBaseLowHpEffect(ECampType campType)
        {
            int result;
            if (campType == ECampType.CAMP_EMPIRE)
            {
                result = this.m_nEmpireLowHpEffect;
            }
            else
            {
                if (campType == ECampType.CAMP_LEAGUE)
                {
                    result = this.m_nLeagueHighHpEffect;
                }
                else 
                {
                    result = 0;
                }
            }
            return result;
        }
        /// <summary>
        /// 清除地图格子
        /// </summary>
        public void Clear()
        {
            
        }
        /// <summary>
        /// 初始化场景地图信息，从配置文件中读取加载信息
        /// </summary>
        /// <param name="dwMapID"></param>
        /// <returns></returns>
        public bool Init(uint dwMapID)
        {
            //根据地图id取得地图信息数据
            DataMaplist dataById = GameData<DataMaplist>.dataMap[(int)dwMapID];
            bool result;
            if (null == dataById)
            {
                result = false;
            }
            else
            {
                base.Init(dwMapID, dataById.MapFilePath);
                if (dataById != null)
                {
                    this.m_nEmpireBaseDeadEft = dataById.EmpireBaseDeadEft;
                    this.m_nLeagueBaseDeadEft = dataById.LeagueBaseDeadEft;
                    this.m_nMyBaseDeadEft = dataById.MineBaseDeadEft;
                    this.m_nTheirBaseDeadEft = dataById.TheirBaseDeadEft;
                    this.m_nEmpireHighHpEffect = dataById.EmpireHighHpEffect;
                    this.m_nLeagueHighHpEffect = dataById.LeagueHighHpEffect;
                    this.m_nEmpireLowHpEffect = dataById.EmpireLowHpEffect;
                    this.m_nLeagueLowHpEffect = dataById.LeagueLowHpEffect;
                    this.m_nLeagueEffectWhenBeastDead = dataById.LeagueEffectWhenHeroDead;
                    this.m_nEmpireEffectWhenBeastDead = dataById.EmpireEffectWhenHeroDead;
                    this.m_nLeagueAttackedEffect = dataById.LeagueAttackedEffect;
                    this.m_nEmpireAttackedEffect = dataById.EmpireAttackedEffect;
                }
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 不包括神兽在内的临近格子坐标
        /// </summary>
        /// <param name="listHexs"></param>
        public void GetNearNodesIgnoreHero(ref List<CVector3> listHexs)
        {
            for (int i = listHexs.Count - 1; i >= 0; i--)
            {
                Beast beast = Singleton<BeastManager>.singleton.GetBeastByPos(listHexs[i]);
                if (beast != null && !beast.IsDead)
                {
                    listHexs.RemoveAt(i);
                }
            }
        }
        #region 取得周围所哟敌人
        public void GetNearEnemys(long unSrcBeastId, int nMaxDis, ref List<long> listBeastId)
        {
            this.GetNearEnemys(unSrcBeastId, nMaxDis, false, ref listBeastId);
        }
        public void GetNearEnemys(long unSrcBeastId, int nMaxDis, bool bNeedInLine, ref List<long> listBeastId)
        {
            Beast beastById = Singleton<BeastManager>.singleton.GetBeastById(unSrcBeastId);
            if (beastById != null && null != listBeastId)
            {
                CVector3 pos = beastById.Pos;
                this.GetNearEnemys(unSrcBeastId, pos, bNeedInLine, nMaxDis, ref listBeastId);
            }
        }
        public void GetNearEnemys(long unSrcBeastId, CVector3 vSrcPos, bool bNeedInLine, int nMaxDis, ref List<long> listHeroId)
        {
            List<long> allEnemys = Singleton<BeastManager>.singleton.GetAllEnemy(unSrcBeastId);
            foreach (uint current in allEnemys)
            {
                Beast beast = Singleton<BeastManager>.singleton.GetBeastById(current);
                if (!beast.IsDead)
                {
                    uint num = base.CalDistance(beast.Pos, vSrcPos);
                    if (num <= nMaxDis)
                    {
                        if (!bNeedInLine || base.IsLine(beast.Pos, vSrcPos))
                        {
                            listHeroId.Add(current);
                        }
                    }
                }
            }
        }
        #endregion
        #endregion
        #region 私有方法

        #endregion

    }
}