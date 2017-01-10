using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using Utility;
using Client.Common;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BeastManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：神兽管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽管理器
/// </summary>
public class BeastManager 
{
	#region 字段
    /// <summary>
    /// 缓存神兽字典
    /// </summary>
    private Dictionary<long, Beast> m_dicAllBeastId = new Dictionary<long, Beast>();
    private IXLog m_log = XLog.GetLog<BeastManager>();
    private static Beast s_beastErro = new Beast(null);
	#endregion
	#region 属性
    public static Beast BeastError 
    {
        get { return BeastManager.s_beastErro; }
    }
    /// <summary>
    /// 是否所有的神兽有死过
    /// </summary>
    public bool IsAllBeastNoDie
    {
        get 
        {
            foreach (var current in this.m_dicAllBeastId.Values)
            {
                if (current.IsEverDead)
                {
                    return false;
                }
            }
            return true;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 加载所有神兽模型
    /// </summary>
    public void LoadAllBeastModel()
    {
        foreach (var current in this.m_dicAllBeastId.Values)
        {
            current.ChangeModel();
        }
    }
    /// <summary>
    /// 根据神兽数据创建神兽，加入到缓存m_dicAllBeastId字典中
    /// </summary>
    /// <param name="beastData">神兽数据</param>
    /// <returns></returns>
    public Beast CreateBeast(BeastData beastData)
    {
        Beast beast = new Beast(beastData);
        if (beast != null)
        {
            this.m_dicAllBeastId[beast.Id] = beast;
            beast.SetVisible(false);
        }
        else 
        {
            this.m_log.Error("beast == null");
        }
        return beast;
    }
    /// <summary>
    /// 根据神兽id删除神兽
    /// </summary>
    /// <param name="unBeastId"></param>
    public void DelBeast(long unBeastId)
    {
        Beast beastById = this.GetBeastById(unBeastId);
        if (null != beastById)
        {
            Singleton<ClientMain>.singleton.scene.DelRoleFromScene(beastById.Id);
            this.m_dicAllBeastId.Remove(beastById.Id);
            beastById.Dispose();
        }
    }
    /// <summary>
    /// 初始化神兽出生坐标,设置到这个格子坐标上然后初始化正方向
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="vec3Hex"></param>
    public void InitBeastPos(long unBeastId, CVector3 vec3Hex)
    {
        Beast beast = this.GetBeastById(unBeastId);
        if (beast != null)
        {
            beast.Move(vec3Hex);
            beast.SetVisible(true);
            Singleton<ClientMain>.singleton.scene.RoleSetToInitialPos(beast.Id, vec3Hex);
            ECampType eCampType = ECampType.CAMP_EMPIRE;
            if (eCampType == beast.eCampType)
            {
                eCampType = ECampType.CAMP_LEAGUE;
            }
            //取得和该神兽对立阵营的基地坐标
            CVector3 baseHexPos = Singleton<ClientMain>.singleton.scene.GetBasePos(eCampType);
            Vector3 baseHex3DPos = Hexagon.GetHex3DPos(baseHexPos, Space.World);
            Vector3 hex3DPos = Hexagon.GetHex3DPos(vec3Hex, Space.World);
            Vector3 vector = baseHex3DPos - hex3DPos;
            //然后得到神兽的正方向
            beast.Forward = new Vector2(vector.x, vector.z);
            //SkillManager.NotifyHeroPosChange(unHeroId);
        }
    }
    /// <summary>
    /// 激活神兽技能
    /// </summary>
    /// <param name="beastId"></param>
    public void ActiveSkills(long beastId)
    {
        Beast beast = this.GetBeastById(beastId);
        if (beast != null)
        {
            beast.ActiveSkills();
        }
    }
    /// <summary>
    /// 刷新所有神兽的光圈
    /// </summary>
    public void RefreshAllBeastAura()
    {
        ICollection<Beast> allBeasts = this.GetAllBeasts();
        foreach (Beast beast in allBeasts)
        {
            beast.RefreshAura();
        }
    }
    public void MoveBeast(long beastId, List<CVector3> listPath)
    {
        if (listPath.Count != 0)
        {
            Beast beast = this.GetBeastById(beastId);
            if (beast != null)
            {
                beast.Move(listPath);
                
            }
        }
    }
    /// <summary>
    /// 通过id获取Beast,如果取不到就从静态有错的Beast实例
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Beast GetBeastById(long id)
    {
        Beast beast;
        if (this.m_dicAllBeastId.ContainsKey(id))
        {
            beast = this.m_dicAllBeastId[id];
        }
        else 
        {
            Debug.Log("Error");
            beast = BeastManager.s_beastErro;
        }
        return beast;
    }
    /// <summary>
    /// 通过Order战斗顺序获取Beast,如果取不到就从静态有错的Beast实例
    /// </summary>
    /// <param name="nOrder"></param>
    /// <returns></returns>
    public Beast GetBeastByOrder(int nOrder)
    {
        Beast beast;
        foreach (var current in this.m_dicAllBeastId.Values)
        {
            if (current.Order == nOrder)
            {
                beast = current;
                return beast;
            }
        }
        beast = BeastManager.BeastError;
        return beast;
    }
    /// <summary>
    /// 根据格子坐标取得神兽，如果该格子坐标上没有神兽就返回空
    /// </summary>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public Beast GetBeastByPos(CVector3 oPos)
    {
        Beast result;
        foreach (Beast current in this.m_dicAllBeastId.Values)
        {
            if (current.Pos.Equals(oPos) && !current.IsDead)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    /// <summary>
    /// 技能cd改变通知，更改界面和技能管理器内存中的技能数据
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="unSkillId"></param>
    /// <param name="unCD"></param>
    public void OnBeastSkillCDChange(long unBeastId, int unSkillId, int unCD)
    {
        Beast beast = this.GetBeastById(unBeastId);
        if (null != beast)
        {
            beast.OnSkillCDChange(unSkillId, (byte)unCD);
        }
    }
    /// <summary>
    /// 神兽出生，主要处理播放声音
    /// </summary>
    /// <param name="unBeastId"></param>
    public void OnBeastBorn(long unBeastId)
    {
        Beast beast = this.GetBeastById(unBeastId);
        if (null != beast)
        {
            beast.OnBorn();
        }
    }
    /// <summary>
    /// 神兽血量改变
    /// </summary>
    /// <param name="beastId"></param>
    /// <param name="unHp"></param>
    public void OnBeastHpChange(long beastId, int unHp)
    {
        Beast beast = this.GetBeastById(beastId);
        if (beast != null)
        {
            beast.OnHpChange(unHp);
        }
    }
    /// <summary>
    /// 神兽进入战斗阶段
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="eRoleStage"></param>
    /// <param name="unTimeLimit"></param>
    public void OnBeastEnterRoleStage(long unBeastId, EClientRoleStage eRoleStage, uint unTimeLimit)
    {
        this.OnBeastEnterRoleStage(unBeastId, eRoleStage, 0u, unTimeLimit, 0u, EQueryTimeType.NORMAL_QUERY_TIME);
    }
    public void OnBeastEnterRoleStage(long unBeastId, EClientRoleStage eRoleStage, uint unBackupLimit, uint unTimeLimit, EQueryTimeType eQueryTimeType)
    {
        this.OnBeastEnterRoleStage(unBeastId, eRoleStage, unBackupLimit, unTimeLimit, 0u, eQueryTimeType);
    }
    public void OnBeastEnterRoleStage(long unBeastId, EClientRoleStage eRoleStage, uint unBackupLimit, uint unTimeLimit, uint unTargetBeastId, EQueryTimeType eQueryTimeType)
    {
        Beast beastById = this.GetBeastById(unBeastId);
        if (beastById != null)
        {
            beastById.TargetBeastId = unTargetBeastId;
            beastById.OnEnterRoleStage(eRoleStage, unBackupLimit, unTimeLimit, unTargetBeastId, eQueryTimeType);
        }
    }
    /// <summary>
    /// 轮到该神兽开始操作
    /// </summary>
    public void OnBeastRoundStart(long beastId)
    {
        Beast beast = this.GetBeastById(beastId);
        if (beast != null)
        {
            beast.OnRoundStart();
        }
    }
    /// <summary>
    ///该神兽操作结束
    /// </summary>
    /// <param name="beastId"></param>
    public void OnBeastRoundEnd(long beastId)
    {
        Beast beast = this.GetBeastById(beastId);
        if (beast != null)
        {
            beast.OnRoundEnd();
        }
    }
    /// <summary>
    /// 神兽进入场景初始化
    /// </summary>
    /// <param name="unBeastId"></param>
    public void OnBeastRevive(long unBeastId)
    {
        Beast beastById = this.GetBeastById(unBeastId);
        if (null != beastById)
        {
            beastById.OnRevive();
        }
    }
    /// <summary>
    /// 在神兽添加到场景中完成的回调
    /// </summary>
    /// <param name="unBeastId"></param>
    public void OnAddBeastToSceneFinished(long unBeastId)
    {
        Beast beast = this.GetBeastById(unBeastId);
        if (beast != null)
        {
            beast.OnAddBeastToSceneFinished();
        }
    }
    /// <summary>
    /// 获得全部神兽
    /// </summary>
    /// <returns></returns>
    public ICollection<Beast> GetAllBeasts()
    {
        return this.m_dicAllBeastId.Values;
    }
    /// <summary>
    /// 取得对立的所有神兽列表ID
    /// </summary>
    /// <param name="beastId"></param>
    /// <returns></returns>
    public List<long> GetAllEnemy(long beastId)
    {
        List<long> list = new List<long>();
        Beast beast = Singleton<BeastManager>.singleton.GetBeastById(beastId);
        if (beast != null)
        {
            ICollection<Beast> allBeasts = this.GetAllBeasts();
            foreach (var cur in allBeasts)
            {
                if (cur.eCampType != beast.eCampType)
                {
                    list.Add(cur.Id);
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 取得某一方阵营的所有神兽
    /// </summary>
    /// <param name="eCampType"></param>
    /// <returns></returns>
    public List<long> GetAllBeastIDByCamp(ECampType eCampType)
    {
        List<long> list = new List<long>();
        ICollection<Beast> allBeasts = this.GetAllBeasts();
        foreach (var beast in allBeasts)
        {
            if (beast.eCampType == eCampType)
            {
                list.Add(beast.Id);
            }
        }
        return list;
    }
    public List<Beast> GetAllBeastByCamp(ECampType eCampType)
    {
        List<Beast> list = new List<Beast>();
        ICollection<Beast> allBeasts = this.GetAllBeasts();
        foreach (var beast in allBeasts)
        {
            if (beast.eCampType == eCampType)
            {
                list.Add(beast);
            }
        }
        return list;
    }
    /// <summary>
    /// 是否所有的神兽准备好了
    /// </summary>
    /// <returns></returns>
    public bool IsAllBeastPrepared()
    {
        foreach (var beast in this.m_dicAllBeastId.Values)
        {
            if (!beast.IsVisible)
            {
                return false;
            }
        }
        return true;
    }
	#endregion
	#region 私有方法
	#endregion
}
