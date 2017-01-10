using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameMainResPreLoad 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.29
// 模块描述：游戏主战斗资源预先加载类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏主战斗资源预先加载类
/// </summary>
public class GameMainResPreLoad : Singleton<GameMainResPreLoad>
{
	#region 字段
    private List<int> m_AllRreLoadEffectIds = new List<int>();
    private IXLog m_log = XLog.GetLog<GameMainResPreLoad>();
    private Dictionary<int, int> m_PreEffectsInRound = new Dictionary<int, int>();
	#endregion
    #region 公有方法
    public void PreLoad()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            int num = list[i];
            if (num > 0 && !this.m_AllRreLoadEffectIds.Contains(num))
            {
                //特效管理器加载特效
                //effectManager.loadEffect();
                this.m_AllRreLoadEffectIds.Add(num);
            }
        }
    }
    public void ProcSingleEffectInRound(int nEffectId, bool preLoad)
    {
        if (nEffectId > 0)
        {
            if (preLoad)
            {
                if (this.m_PreEffectsInRound.ContainsKey(nEffectId))
                {
                    this.m_PreEffectsInRound[nEffectId] = this.m_PreEffectsInRound[nEffectId] + 1;
                }
                else
                {
                    this.m_PreEffectsInRound.Add(nEffectId, 0);
                    //特效管理器加载特效
                }
            }
            else 
            {
                if (this.m_PreEffectsInRound.ContainsKey(nEffectId))
                {
                    this.m_PreEffectsInRound[nEffectId] = this.m_PreEffectsInRound[nEffectId] - 1;
                    //特效管理器卸载特效
                    if (this.m_PreEffectsInRound[nEffectId] == 0)
                    {
                        //特效管理器卸载特效
                        //移除缓存
                        this.m_PreEffectsInRound.Remove(nEffectId);
                    }
                }
            }
        }
    }
    public void Unload()
    {
        for (int i = 0; i < this.m_AllRreLoadEffectIds.Count; i++)
        {
            int id = this.m_AllRreLoadEffectIds[i];
            //特效管理器卸载特效
        }
        this.m_AllRreLoadEffectIds.Clear();
    }
    #endregion
}
