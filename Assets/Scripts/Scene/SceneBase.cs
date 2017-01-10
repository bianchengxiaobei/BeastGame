using UnityEngine;
using System.Collections.Generic;
using Game;
using Client.Common;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SceneBase
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：场景地图基类
//----------------------------------------------------------------*/
#endregion
public class SceneBase 
{
    private Map m_map;
    /// <summary>
    /// 地图格子数据
    /// </summary>
    public Dictionary<int, Dictionary<int, MapNode>> DicMapData
    {
        get
        {
            if (this.m_map == null)
            {
                return new Dictionary<int, Dictionary<int, MapNode>>();
            }
            return this.m_map.GetScene().DicMapData;
        }
    }
    /// <summary>
    /// 地图格子类型
    /// </summary>
    public Dictionary<EMapNodeType, MapNodeTypeInfo> DicMapNodeTypeInfo
    {
        get
        {
            if (this.m_map == null)
            {
                return new Dictionary<EMapNodeType, MapNodeTypeInfo>();
            }
            return this.m_map.GetScene().DicMapNodeTypeInfo;
        }
    }
    public PVESetting PVESetting
    {
        get 
        {
            if (null == this.m_map)
            {
                return null;
            }
            return this.m_map.GetScene().PVESetting;
        }
    }
    public Vector3 InitPos
    {
        get
        {
            if (this.m_map == null)
            {
                return Vector3.zero;
            }
            return this.m_map.GetScene().InitPos;
        }
    }
    public Vector3 InitRotation
    {
        get 
        {
            if (null == this.m_map)
            {
                return Vector3.zero;
            }
            return this.m_map.GetScene().InitRotation;
        }
    }
    /// <summary>
    /// 初始化地图
    /// </summary>
    /// <param name="dwMapID">地图id</param>
    /// <param name="strConfigFile">配置文件</param>
    /// <returns></returns>
    public bool Init(uint dwMapID, string strConfigFile)
    {
        this.m_map = new Map();
        return this.m_map.Init(dwMapID, strConfigFile);//从配置文件中读取地图格子个数（x,y,u）和类型(type)
    }
    /// <summary>
    /// 地图上是否存在这个节点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool HasMapNode(CVector3 pos)
    {
        return this.m_map != null && this.m_map.HasMapNode(pos);
    }
    /// <summary>
    /// 改变mapNode和PathNode的类型
    /// </summary>
    /// <param name="oPos"></param>
    /// <param name="strMapNodeType"></param>
    /// <returns></returns>
    public bool ChangeMapNodeType(CVector3 oPos, string strMapNodeType)
    {
        return this.m_map != null && this.m_map.ChangeMapNodeType(oPos, strMapNodeType);
    }
    /// <summary>
    /// 这个Node上是否有角色站立
    /// </summary>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public bool IsPosStandByAnyRole(CVector3 oPos)
    {
        return this.m_map != null && this.m_map.IsPosStandByAnyRole(oPos);
    }
    /// <summary>
    /// 是否node是阻塞的
    /// </summary>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public bool IsPosBlocked(CVector3 oPos)
    {
        return this.m_map != null && this.m_map.IsPosBlocked(oPos);
    }
    /// <summary>
    /// 设置角色到这个node上,改变不能行走状态
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public bool RoleSetToInitialPos(long unBeastId, CVector3 oPos)
    {
        return this.m_map != null && this.m_map.RoleSetToInitialPos(unBeastId, oPos);
    }
    /// <summary>
    /// 从场景中删除这个角色
    /// </summary>
    /// <param name="unBeastId"></param>
    public void DelRoleFromScene(long unBeastId)
    {
        if (this.m_map == null)
        {
            return;
        }
        this.m_map.DelRoleFromScene(unBeastId);
    }
    /// <summary>
    /// 取得角色在地图上的坐标
    /// </summary>
    /// <param name="unHeroId"></param>
    /// <returns></returns>
    public CVector3 GetRolePos(uint unHeroId)
    {
        if (this.m_map == null)
        {
            return null;
        }
        return this.m_map.GetRolePos(unHeroId);
    }
    /// <summary>
    /// 根据坐标取得node类型
    /// </summary>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public EMapNodeType GetMapNodeType(CVector3 oPos)
    {
        if (this.m_map == null)
        {
            return EMapNodeType.MAP_NODE_INVALID;
        }
        return this.m_map.GetMapNodeType(oPos);
    }
    public virtual bool PlaceAEnergyTrap(CVector3 oPos)
    {
        return this.m_map != null && this.m_map.PlaceAEnergyTrap(oPos);
    }
    public bool IsHasAEnergyTrap(CVector3 oPos)
    {
        return this.m_map != null && this.m_map.CanOverlapTrap(oPos);
    }
    public virtual bool CancelAEnergyTrap(CVector3 oPos)
    {
        return this.m_map != null && this.m_map.CancelAEnergyTrap(oPos);
    }
    public bool RoleJumpToPos(uint unHeroId, CVector3 oPos)
    {
        return this.m_map != null && this.m_map.RoleJumpToPos(unHeroId, oPos);
    }
    public bool RoleMoveTo(uint unHeroId, List<CVector3> oPath)
    {
        return this.m_map != null && this.m_map.RoleMoveTo(unHeroId, oPath);
    }
    public bool SwapPlayer(uint unSrcPlayerId, uint unDstPlayerId, CVector3 srcPos, CVector3 dstPos)
    {
        return this.m_map != null && this.m_map.SwapPlayer(unSrcPlayerId, unDstPlayerId, srcPos, dstPos);
    }
    public bool FindPath(int nMaxDistance, CVector3 oSrcPos, CVector3 oDestPos, ref List<CVector3> oListPath, bool bIgnoreHeroObstacle)
    {
        return this.m_map != null && this.m_map.FindPath(nMaxDistance, oSrcPos, oDestPos, ref oListPath, bIgnoreHeroObstacle);
    }
    public bool GetNearNodes(int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bIgnore)
    {
        return this.m_map != null && this.m_map.GetNearNodes(nMaxDistance, oSrc, ref oFindedNodes, bIgnore);
    }
    public bool GetNearNodesInLineIgnoreObstruct(int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bReturnObstruct)
    {
        return this.m_map != null && this.m_map.GetNearNodesInLineIgnoreObstruct(nMaxDistance, oSrc, ref oFindedNodes, bReturnObstruct);
    }
    public bool GetNearNodesIgnoreObstruct(int nMinDistance, int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bReturnObstruct, bool bReturnEnergyTrap)
    {
        return null != this.m_map && this.m_map.GetNearNodesIgnoreObstruct(nMinDistance, nMaxDistance, oSrc, ref oFindedNodes, bReturnObstruct, bReturnEnergyTrap);
    }
    public bool GetNearNodesIgnoreObstruct(int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes)
    {
        return null != this.m_map && this.m_map.GetNearNodesIgnoreObstruct(1, nMaxDistance, oSrc, ref oFindedNodes, false, true);
    }
    public bool GetDirectionNodes(CVector3 oSrc, CVector3 oDest, uint unDis, ref List<CVector3> oFindedNodes)
    {
        return this.m_map != null && this.m_map.GetDirectionNodes(oSrc, oDest, unDis, ref oFindedNodes);
    }
    public bool GetDirectionNodes(CVector3 oSrc, CVector3 oDest, uint unDis, ref List<CVector3> oFindedNodes, bool bReturnObstruct)
    {
        return this.m_map != null && this.m_map.GetDirectionNodesReturnObstruct(oSrc, oDest, unDis, ref oFindedNodes, bReturnObstruct);
    }
    public bool GetThreeDirectionNodes(CVector3 oSrc, CVector3 oDest, uint unDis, ref List<CVector3> oFindedNodes, bool bReturnObstruct)
    {
        return this.m_map != null && this.m_map.GetThreeDirectionNodes(oSrc, oDest, unDis, ref oFindedNodes, bReturnObstruct);
    }
    public bool IsLine(CVector3 oSrc, CVector3 oDest)
    {
        return this.m_map != null && this.m_map.IsNodesInLine(oSrc, oDest);
    }
    public bool IsRay(CVector3 oSrc, CVector3 oDest1, CVector3 oDest2)
    {
        return this.m_map != null && this.m_map.IsRay(oSrc, oDest1, oDest2);
    }
    /// <summary>
    /// 获得目标bode相邻的所有node
    /// </summary>
    /// <param name="oSrc"></param>
    /// <param name="oFindedNodes"></param>
    public void GetAdjacentNodes(CVector3 oSrc, ref List<CVector3> oFindedNodes)
    {
        if (this.m_map == null)
        {
            return;
        }
        this.m_map.GetAdjacentNodes(oSrc, ref oFindedNodes);
    }
    /// <summary>
    /// 取得基地的格子坐标
    /// </summary>
    /// <param name="nCamp"></param>
    /// <returns></returns>
    public CVector3 GetBasePos(ECampType nCamp)
    {
        CVector3 result;
        if (null == this.m_map)
        {
            result = null;
        }
        else
        {
            result = this.m_map.GetBasePos(nCamp);
        }
        return result;
    }
    /// <summary>
    /// 根据node类型找出地图上所有的node
    /// </summary>
    /// <param name="eMapNodeType"></param>
    /// <returns></returns>
    public List<CVector3> GetNodesByType(EMapNodeType eMapNodeType)
    {
        if (this.m_map == null)
        {
            return null;
        }
        return this.m_map.GetNodesByType(eMapNodeType);
    }
    /// <summary>
    /// 游戏角色死亡清空node设置可行走
    /// </summary>
    /// <param name="unHeroId"></param>
    public void OnRoleDie(uint unHeroId)
    {
        if (this.m_map == null)
        {
            return;
        }
        this.m_map.OnRoleDie(unHeroId);
    }
    /// <summary>
    /// 计算两个node的最小距离
    /// </summary>
    /// <param name="oSrcPos"></param>
    /// <param name="oDstPos"></param>
    /// <returns></returns>
    public uint CalDistance(CVector3 oSrcPos, CVector3 oDstPos)
    {
        if (this.m_map == null)
        {
            return 0u;
        }
        return this.m_map.CalDistance(oSrcPos, oDstPos);
    }
    public bool IsAdjacent(CVector3 oPos1, CVector3 oPos2)
    {
        return this.m_map != null && this.m_map.IsAdjacent(oPos1, oPos2);
    }
}
