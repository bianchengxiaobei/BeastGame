using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Utility.Export;
using Game;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Map
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：地图类
//----------------------------------------------------------------*/
#endregion
internal class Map 
{
    private uint m_MapID;
    private CMapData m_oMapData;
    /// <summary>
    /// 存储神兽所在的格子字典
    /// </summary>
    private Dictionary<long, CVector3> m_oDicRolesPos;
    private Dictionary<int, Dictionary<int, PathNode>> m_oTablePathNodes;
    private IXLog logger = new Logger("Map");
    public CMapData GetScene() 
    {
        return this.m_oMapData;
    }
    public Map()
    {
        this.m_MapID = 0u;
        this.m_oMapData = null;
        this.m_oDicRolesPos = new Dictionary<long, CVector3>();
        this.m_oTablePathNodes = new Dictionary<int, Dictionary<int, PathNode>>();
    }
    /// <summary>
    /// 初始化地图，根据配置文件
    /// </summary>
    /// <param name="mapID">地图id</param>
    /// <param name="strConfigFile">配置文件</param>
    /// <returns></returns>
    public bool Init(uint mapID, string strConfigFile)
    {
        this.m_MapID = mapID;
        this.m_oMapData = new CMapData();
        this.m_oMapData.Init(this.m_MapID, strConfigFile);
        this.OnInitFinished();
        return true;
    }
    /// <summary>
    /// 地图上是否有这个节点
    /// </summary>
    /// <param name="pos">格子坐标</param>
    /// <returns></returns>
    public bool HasMapNode(CVector3 pos)
    {
        return this.m_oMapData.HasMapNode(pos.m_nX, pos.m_nY);
    }
    /// <summary>
    /// 改变地图格子类型，改变PathNode的类型
    /// </summary>
    /// <param name="oPos"></param>
    /// <param name="strMapNodeType"></param>
    /// <returns></returns>
    public bool ChangeMapNodeType(CVector3 oPos, string strMapNodeType)
    {
        if (null == this.m_oMapData)
        {
            return false;
        }
        Dictionary<int, PathNode> dictionary = null;
        if (this.m_oTablePathNodes.TryGetValue(oPos.m_nX, out dictionary))
        {
            PathNode localMapNode = null;
            if (dictionary.TryGetValue(oPos.m_nY, out localMapNode))
            {
                localMapNode.m_bCanWalk = MapCfg.GetNodeWalkType(strMapNodeType);
            }
        }
        return this.m_oMapData.ChangeMapNodeType(oPos, strMapNodeType);
    }
    /// <summary>
    /// 是否这个格子上有神兽站立
    /// </summary>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public bool IsPosStandByAnyRole(CVector3 oPos)
    {
        foreach (CVector3 current in this.m_oDicRolesPos.Values)
        {
            if (current.Equals(oPos))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsPosBlocked(CVector3 oPos)
    {
        return !this.m_oMapData.HasMapNode(oPos.m_nX, oPos.m_nY) || !this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY].m_bCanWalk;
    }
    /// <summary>
    /// 神兽设置到该格子坐标上
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="oPos"></param>
    /// <returns></returns>
    public bool RoleSetToInitialPos(long unBeastId, CVector3 oPos)
    {
        if (!this.m_oMapData.HasMapNode(oPos.m_nX, oPos.m_nY))
        {
            this.logger.Error(string.Format("HasMapNode(oPos) == false:{0}", oPos.ToString()));
        }
        if (this.m_oMapData.GetMapNode(oPos.m_nX, oPos.m_nY) == null)
        {
            return false;
        }
        //判断这个格子上是否已经有神兽了
        if (this.IsPosStandByAnyRole(oPos))
        {
            this.logger.Error(string.Format("IsPosStandByAnyRole(oPos):{0}", oPos.ToString()));
            return false;
        }
        //如果神兽格子上不存在改神兽，就吧该神兽加到该字典
        if (!this.m_oDicRolesPos.ContainsKey(unBeastId))
        {
            this.m_oDicRolesPos.Add(unBeastId, new CVector3(oPos));
            this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY].m_bCanWalk = false;
            this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY].m_BeastID = unBeastId;
            return true;
        }
        this.logger.Error(string.Format("m_oDicRolesPos.ContainsnIndexX(unHeroId) == false:{0}", unBeastId));
        return false;
    }
    public void DelRoleFromScene(long unBeastId) 
    {
        if (!this.m_oDicRolesPos.ContainsKey(unBeastId))
        {
            this.logger.Error(string.Format("m_oDicRolesPos.ContainsnIndexX(unHeroId) == false:{0}", unBeastId));
            return;
        }
        int nX = this.m_oDicRolesPos[unBeastId].m_nX;
        int nY = this.m_oDicRolesPos[unBeastId].m_nY;
        this.m_oTablePathNodes[nX][nY].m_bCanWalk = true;
        this.m_oTablePathNodes[nX][nY].m_BeastID = 0u;
        this.m_oDicRolesPos.Remove(unBeastId);
    }
    public CVector3 GetRolePos(uint unHeroId)
    {
        if (!this.m_oDicRolesPos.ContainsKey(unHeroId))
        {
            return null;
        }
        return this.m_oDicRolesPos[unHeroId];
    }
    public EMapNodeType GetMapNodeType(CVector3 oPos)
    {
        MapNode mapNode = this.m_oMapData.GetMapNode(oPos.m_nX, oPos.m_nY);
        if (mapNode == null)
        {
            return EMapNodeType.MAP_NODE_INVALID;
        }
        return mapNode.m_eMapNodeType;
    }
    public CVector3 GetBasePos(ECampType nCamp)
    {
        CVector3 result;
        if (nCamp == ECampType.CAMP_EMPIRE)
        {
            result = this.m_oMapData.empireMapNode;
        }
        else
        {
            if (nCamp == ECampType.CAMP_LEAGUE)
            {
                result = this.m_oMapData.leagueMapNode;
            }
            else
            {
                result = null;
            }
        }
        return result;
    }
    public bool PlaceAEnergyTrap(CVector3 oPos)
    {
        if (this.isCanPlaceATrap(oPos))
        {
            return false;
        }
        PathNode mapNode = this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY];
        mapNode.sum += 1;
        return true;
    }
    public bool CanOverlapTrap(CVector3 pos)
    {
        return this.m_oTablePathNodes[pos.m_nX][pos.m_nY].sum != 0;
    }
    private bool isCanPlaceATrap(CVector3 pos)
    {
        return !this.IsPosBlocked(pos) && !this.CanOverlapTrap(pos);
    }
    public bool CancelAEnergyTrap(CVector3 oPos) 
    {
        PathNode expr = this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY];
        expr.sum -= 1;
        return true;
    }
    public bool RoleJumpToPos(uint unHeroId, CVector3 oPos)
    {
        if (this.IsPosBlocked(oPos))
        {
            return false;
        }
        if (!this.m_oDicRolesPos.ContainsKey(unHeroId))
        {
            this.m_oDicRolesPos.Add(unHeroId, oPos);
        }
        else 
        {
            int nX = this.m_oDicRolesPos[unHeroId].m_nX;
            int nY = this.m_oDicRolesPos[unHeroId].m_nY;
            this.m_oTablePathNodes[nX][nY].m_bCanWalk = true;
            this.m_oTablePathNodes[nX][nY].m_BeastID = 0u;
            this.m_oDicRolesPos[unHeroId] = new CVector3(oPos);
        }
        this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY].m_bCanWalk = false;
        this.m_oTablePathNodes[oPos.m_nX][oPos.m_nY].m_BeastID = unHeroId;
        return true;
    }
    public bool RoleMoveTo(uint unHeroId, List<CVector3> oPath)
    {
        return this.RoleJumpToPos(unHeroId, oPath.First<CVector3>());
    }
    public bool SwapPlayer(uint unSrcPlayerId, uint unDstPlayerId, CVector3 srcPos, CVector3 dstPos)
    {
        if (!this.m_oDicRolesPos.ContainsKey(unSrcPlayerId))
        {
            this.m_oDicRolesPos.Add(unSrcPlayerId, srcPos);
        }
        else 
        {
            this.m_oDicRolesPos[unSrcPlayerId] = new CVector3(srcPos);
        }
        if (!this.m_oDicRolesPos.ContainsKey(unDstPlayerId))
        {
            this.m_oDicRolesPos.Add(unDstPlayerId, new CVector3(dstPos));
        }
        else 
        {
            this.m_oDicRolesPos[unDstPlayerId] = new CVector3(dstPos);
        }
        this.m_oTablePathNodes[srcPos.m_nX][srcPos.m_nY].m_BeastID = unSrcPlayerId;
        this.m_oTablePathNodes[dstPos.m_nX][dstPos.m_nY].m_BeastID = unDstPlayerId;
        return true;
    }
    public bool FindPath(int nMaxDistance, CVector3 oSrcPos, CVector3 oDestPos, ref List<CVector3> oListPath, bool bIgnoreHeroObstacle)
    {
        if (nMaxDistance <= 0)
        {
            return false;
        }
        if (oSrcPos == oDestPos)
        {
            return false;
        }
        if (!this.m_oTablePathNodes.ContainsKey(oSrcPos.m_nX) || !this.m_oTablePathNodes[oSrcPos.m_nX].ContainsKey(oSrcPos.m_nY))
        {
            this.logger.Error(string.Format("oSrcPos(oSrcPos.m_nX={0},oSrcPos.m_nY={1}) Is Invalid!", oSrcPos.m_nX, oSrcPos.m_nY));
            return false;
        }
        if (!this.m_oTablePathNodes.ContainsKey(oDestPos.m_nX) || !this.m_oTablePathNodes[oDestPos.m_nX].ContainsKey(oDestPos.m_nY))
        {
            this.logger.Error(string.Format("oDestPos(oDestPos.m_nX={0},oDestPos.m_nY={1}) Is Invalid!", oDestPos.m_nX, oDestPos.m_nY));
            return false;
        }
        PathNode srcMapNode = this.m_oTablePathNodes[oSrcPos.m_nX][oSrcPos.m_nY];
        if (null == srcMapNode)
        {
            return false;
        }
        PathNode desMapNode = this.m_oTablePathNodes[oDestPos.m_nX][oDestPos.m_nY];
        if (null == desMapNode || !desMapNode.m_bCanWalk)
        {
            return false;
        }
        foreach (var current in this.m_oTablePathNodes.Values)
        {
            foreach (var current2 in current.Values)
            {
                current2.distance = 1000;
                current2.maxDistance = 10000;
                current2.parentMapNode = null;
            }
        }
        srcMapNode.distance = 0;
        srcMapNode.maxDistance = 0;
        Queue<PathNode> queue = new Queue<PathNode>();
        queue.Enqueue(srcMapNode);
        oListPath.Clear();
        uint num = 0u;
        while (queue.Count > 0)
        {
            PathNode firstMapNode = queue.Peek();
            for (int i = 0; i < 6; i++)
            {
                PathNode nearMapNode = firstMapNode.m_listNextNode[i];
                if (nearMapNode != null && firstMapNode.parentMapNode != nearMapNode && (nearMapNode.sum == 0 || (nearMapNode.nIndexX == oDestPos.m_nX && nearMapNode.nIndexY == oDestPos.m_nY)))
                {
                    if (nearMapNode.m_bCanWalk || (bIgnoreHeroObstacle && nearMapNode.m_BeastID > 0u))
                    {
                        int distance = firstMapNode.distance + 1;
                        int maxDistance = firstMapNode.maxDistance + 1;
                        if (distance <= nMaxDistance && maxDistance < nearMapNode.maxDistance)
                        {
                            nearMapNode.distance = distance;
                            nearMapNode.maxDistance = maxDistance;
                            nearMapNode.parentMapNode = firstMapNode;
                            queue.Enqueue(nearMapNode);
                        }
                    }
                    num += 1u;
                }
            }
            queue.Dequeue();
        }
        PathNode endNode = desMapNode;
        if (endNode.distance <= nMaxDistance)
        {
            do
            {
                CVector3 cVector = new CVector3();
                cVector.m_nX = endNode.nIndexX;
                cVector.m_nY = endNode.nIndexY;
                cVector.m_nU = endNode.nIndexU;
                oListPath.Add(cVector);
                endNode = endNode.parentMapNode;
            } while (endNode != null);
            return true;
        }
        return false;
    }
    public bool GetNearNodes(int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bIgnore)
    {
        if (nMaxDistance < 0)
        {
            nMaxDistance = 2147483647;
        }
        if (nMaxDistance == 0)
        {
            return false;
        }
        if (!this.m_oMapData.HasMapNode(oSrc.m_nX, oSrc.m_nY))
        {
            return false;
        }
        foreach (var current in this.m_oTablePathNodes)
        {
            foreach (var current2 in current.Value)
            {
                current2.Value.distance = 1000;
            }
        }
        this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY].distance = 0;
        oFindedNodes.Clear();
        PathNode srcMapNode = this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY];
        Queue<PathNode> queue = new Queue<PathNode>();
        queue.Enqueue(srcMapNode);
        while (queue.Count != 0)
        {
            PathNode node = queue.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                PathNode nearNode = node.m_listNextNode[i];
                if (nearNode != null)
                {
                    CVector3 cVector = new CVector3();
                    cVector.m_nX = nearNode.nIndexX;
                    cVector.m_nY = nearNode.nIndexY;
                    cVector.m_nU = nearNode.nIndexU;
                    if ((nearNode.m_bCanWalk || (bIgnore && this.IsPosStandByAnyRole(cVector)) || (nearNode.m_bCanWalk && CanOverlapTrap(cVector))) && oSrc != cVector)
                    {
                        int distance = node.distance + 1;
                        if (distance <= nMaxDistance && distance < nearNode.distance)
                        {
                            if (!this.CanOverlapTrap(cVector))
                            {
                                nearNode.distance = distance;
                                queue.Enqueue(nearNode);
                            }
                            if (!oFindedNodes.Contains(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                        }
                    }
                }
            }
        }
        return true;
    }
    public bool GetNearNodesIgnoreObstruct(int nMinDistance, int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bReturnObstruct, bool bReturnEnergyTrap)
    {
        bool nearNodesIgnoreObstruct;
        if (bReturnObstruct)
        {
            nearNodesIgnoreObstruct = this.GetNearNodesIgnoreObstruct(nMinDistance, nMaxDistance, oSrc, ref oFindedNodes, 4294967295u, bReturnEnergyTrap);
        }
        else
        {
            nearNodesIgnoreObstruct = this.GetNearNodesIgnoreObstruct(nMinDistance, nMaxDistance, oSrc, ref oFindedNodes, 2u, bReturnEnergyTrap);
        }
        return nearNodesIgnoreObstruct;
    }
    public bool GetNearNodesIgnoreObstruct(int nMinDistance, int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, uint unReturnObstructType, bool bReturnEnergyTrap)
    {
        if (nMaxDistance < 0)
        {
            nMaxDistance = 2147483647;
        }
        bool result;
        if (nMaxDistance == 0)
        {
            result = false;
        }
        else
        {
            if (nMinDistance > nMaxDistance)
            {
                result = false;
            }
            else
            {
                if (!this.m_oMapData.HasMapNode(oSrc.m_nX, oSrc.m_nY))
                {
                    result = false;
                }
                else
                {
                    foreach (KeyValuePair<int, Dictionary<int, PathNode>> current in this.m_oTablePathNodes)
                    {
                        foreach (KeyValuePair<int, PathNode> current2 in current.Value)
                        {
                            current2.Value.distance = 1000;
                        }
                    }
                    this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY].distance = 0;
                    oFindedNodes.Clear();
                    PathNode item = this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY];
                    Queue<PathNode> queue = new Queue<PathNode>();
                    queue.Enqueue(item);
                    while (queue.Count != 0)
                    {
                        PathNode pathNode = queue.Dequeue();
                        for (int i = 0; i < 6; i++)
                        {
                            PathNode pathNode2 = pathNode.m_listNextNode[i];
                            if (pathNode2 != null)
                            {
                                CVector3 cVector = new CVector3();
                                cVector.m_nX = pathNode2.nIndexX;
                                cVector.m_nY = pathNode2.nIndexY;
                                cVector.m_nU = pathNode2.nIndexU;
                                int num = pathNode.distance + 1;
                                if (num <= nMaxDistance && num < pathNode2.distance)
                                {
                                    pathNode2.distance = num;
                                    queue.Enqueue(pathNode2);
                                    if (!oFindedNodes.Contains(cVector))
                                    {
                                        bool flag = false;
                                        if (pathNode2.m_bCanWalk)
                                        {
                                            flag = true;
                                        }
                                        if (bReturnEnergyTrap && pathNode2.m_bCanWalk)
                                        {
                                            flag = true;
                                        }
                                        if (!pathNode2.m_bCanWalk)
                                        {
                                            if (this.IsPosStandByAnyRole(cVector) && (unReturnObstructType & 2u) > 0u)
                                            {
                                                flag = true;
                                            }
                                            if (!this.IsPosStandByAnyRole(cVector) && (unReturnObstructType & 1u) > 0u)
                                            {
                                                flag = true;
                                            }
                                        }
                                        if (flag)
                                        {
                                            oFindedNodes.Add(cVector);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    oFindedNodes.Add(oSrc);
                    for (int j = oFindedNodes.Count - 1; j >= 0; j--)
                    {
                        if (!this.CheckDistance(oFindedNodes[j], nMinDistance))
                        {
                            oFindedNodes.RemoveAt(j);
                        }
                    }
                    result = true;
                }
            }
        }
        return result;
    }
    public bool GetNearNodesInLineIgnoreObstruct(int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bReturnObstruct)
    {
        List<CVector3> list = new List<CVector3>();
        bool result = this.GetNearNodesInLine(1, nMaxDistance, oSrc,ref oFindedNodes, bReturnObstruct, true);
        foreach (CVector3 current in list)
        {
            if (this.IsNodesInLine(current, oSrc))
            {
                oFindedNodes.Add(current);
            }
        }
        return result;
    }
    public bool GetNearNodesInLine(int a,int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, bool bReturnObstruct, bool bIgnore)
    {
        if (bReturnObstruct)
        {
            return this.GetNearNodesReturnObstruct(a, nMaxDistance, oSrc,ref oFindedNodes, 4294967295u, bIgnore);
        }
        return this.GetNearNodesReturnObstruct(a, nMaxDistance, oSrc,ref oFindedNodes, 2u, bIgnore);
    }
    public bool GetNearNodesReturnObstruct(int a,int nMaxDistance, CVector3 oSrc, ref List<CVector3> oFindedNodes, uint obstructID, bool bIgnore)
    {
        if (nMaxDistance < 0)
        {
            nMaxDistance = 2147483647;
        }
        if (a == 0)
        {
            return false;
        }
        if (a > nMaxDistance)
        {
            return false;
        }
        if (!this.m_oMapData.HasMapNode(oSrc.m_nX, oSrc.m_nY))
        {
            return false;
        }
        foreach (var current in this.m_oTablePathNodes)
        {
            foreach (var current2 in current.Value)
            {
                current2.Value.distance = 1000;
            }
        }
        this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY].distance = 0;
        oFindedNodes.Clear();
        PathNode item = this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY];
        Queue<PathNode> queue = new Queue<PathNode>();
        queue.Enqueue(item);
        while (queue.Count != 0)
        {
            PathNode curMapNode = queue.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                PathNode nearNode = curMapNode.m_listNextNode[i];
                if (nearNode != null)
                {
                    CVector3 cVector = new CVector3();
                    cVector.m_nX = nearNode.nIndexX;
                    cVector.m_nY = nearNode.nIndexY;
                    cVector.m_nU = nearNode.nIndexU;
                    int distance = curMapNode.distance + 1;
                    if (distance <= nMaxDistance && distance < nearNode.distance)
                    {
                        nearNode.distance = distance;
                        queue.Enqueue(nearNode);
                        if (!oFindedNodes.Contains(cVector))
                        {
                            bool flag = false;
                            if (nearNode.m_bCanWalk && !this.CanOverlapTrap(cVector))
                            {
                                flag = true;
                            }
                            if (bIgnore && this.CanOverlapTrap(cVector) && nearNode.m_bCanWalk)
                            {
                                flag = true;
                            }
                            if (!nearNode.m_bCanWalk)
                            {
                                if (this.IsPosStandByAnyRole(cVector) && (obstructID & 2u) > 0)
                                {
                                    flag = true;
                                }
                                if (!this.IsPosStandByAnyRole(cVector) && (obstructID & 1u) > 0)
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                oFindedNodes.Add(cVector);
                            }
                        }
                    }

                }
            }
        }
        oFindedNodes.Add(oSrc);
        for (int j = oFindedNodes.Count - 1; j >= 0; j--)
        {
            if (!this.CheckDistance(oFindedNodes[j], a))
            {
                oFindedNodes.RemoveAt(j);
            }
        }
        return true;
    }
    public bool IsNodesInLine(CVector3 a, CVector3 b)
    {
        return this.m_oMapData.IsNodesInLine(a, b);
    }
    public bool GetDirectionNodes(CVector3 oSrc, CVector3 oDest, uint unDis, ref List<CVector3> oFindedNodes)
    {
        return this.GetDirectionNodesReturnObstruct(oSrc, oDest, unDis, ref oFindedNodes, false);
    }
    public bool GetDirectionNodesReturnObstruct(CVector3 oSrc, CVector3 oDest, uint unDis, ref List<CVector3> oFindedNodes, bool returnObstruct)
    {
        if (!this.IsNodesInLine(oSrc, oDest))
        {
            return false;
        }
        if (oSrc.m_nX == oDest.m_nX)
        {
            if (oSrc.m_nY <= oDest.m_nY)
            {
                int num = 1;
                while (num <= unDis)
                {
                    CVector3 cVector = new CVector3();
                    cVector.m_nX = oSrc.m_nX;
                    cVector.m_nY = oSrc.m_nY + num;
                    cVector.m_nU = cVector.m_nY - cVector.m_nX;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (returnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    num++;
                }
            }
            else
            {
                int num2 = 1;
                while (num2 <= unDis)
                {
                    CVector3 cVector = new CVector3();
                    cVector.m_nX = oSrc.m_nX;
                    cVector.m_nY = oSrc.m_nY - num2;
                    cVector.m_nU = cVector.m_nY - cVector.m_nX;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (returnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    num2++;
                }
            }
        }
        else 
        {
            if (oSrc.m_nY == oDest.m_nY)
            {
                if (oSrc.m_nX <= oDest.m_nY)
                {
                    int num3 = 1;
                    while (num3 <= unDis)
                    {
                        CVector3 cVector = new CVector3();
                        cVector.m_nX = oSrc.m_nX + num3;
                        cVector.m_nY = oSrc.m_nY;
                        cVector.m_nU = cVector.m_nY - cVector.m_nX;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (returnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        num3++;
                    }
                }
                else
                {
                    int num4 = 1;
                    while (num4 <= unDis)
                    {
                        CVector3 cVector = new CVector3();
                        cVector.m_nX = oSrc.m_nX - num4;
                        cVector.m_nY = oSrc.m_nY;
                        cVector.m_nU = cVector.m_nY - cVector.m_nX;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (returnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        num4++;
                    }
                }
            }
            else 
            {
                if (oSrc.m_nU == oDest.m_nU)
                {
                    if (oSrc.m_nX <= oDest.m_nX)
                    {
                        int num = 1;
                        while (num <= unDis)
                        {
                            CVector3 cVector = new CVector3();
                            cVector.m_nX = oSrc.m_nX + num;
                            cVector.m_nY = cVector.m_nX + cVector.m_nU;
                            cVector.m_nU = oSrc.m_nU;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (returnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            num++;
                        }
                    }
                    else 
                    {
                        int num = 1;
                        while (num <= unDis)
                        {
                            CVector3 cVector = new CVector3();
                            cVector.m_nX = oSrc.m_nX - num;
                            cVector.m_nY = cVector.m_nX + cVector.m_nU;
                            cVector.m_nU = oSrc.m_nU;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (returnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            num++;
                        }
                    }
                }
            }
        }
        return true;
    }
    private bool CheckDistance(CVector3 srcNode, int distance)
    {
        PathNode node = this.m_oTablePathNodes[srcNode.m_nX][srcNode.m_nY];
        return node.distance >= distance;
    }
    public bool GetThreeDirectionNodes(CVector3 oSrc, CVector3 oDest, uint unDis, ref List<CVector3> oFindedNodes, bool bReturnObstruct) 
    {
        if (!this.IsNodesInLine(oSrc, oDest))//如果不在同一条直线上
        {
            return false;
        }
        if (oSrc.m_nX == oDest.m_nX)
        {
            if (oSrc.m_nY <= oDest.m_nY)//当在同一列（x），且目标在正上方
            {
                int num = 1;
                while (num <= unDis)
                {
                    CVector3 cVector = new CVector3();//目标在正上方undic个单位的所有node
                    cVector.m_nX = oSrc.m_nX;
                    cVector.m_nY = oSrc.m_nY + num;
                    cVector.m_nU = cVector.m_nY - cVector.m_nX;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (bReturnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    cVector = new CVector3();//目标在左上方undic个单位的所有node
                    cVector.m_nY = oSrc.m_nY;
                    cVector.m_nU = oSrc.m_nU + num;
                    cVector.m_nX = cVector.m_nY - cVector.m_nU;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (bReturnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    cVector = new CVector3();//目标在右上方undic个单位的所有node
                    cVector.m_nU = oSrc.m_nU;
                    cVector.m_nX = oSrc.m_nX + num;
                    cVector.m_nY = cVector.m_nX + cVector.m_nU;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (bReturnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    num++;
                }
            }
            else//在同一列，但是目标在正下方
            {
                int num = 1;
                while (num <= unDis)
                {
                    CVector3 cVector = new CVector3();//在同一列，目标正下方
                    cVector.m_nX = oSrc.m_nX;
                    cVector.m_nY = oSrc.m_nY - num;
                    cVector.m_nU = cVector.m_nY - cVector.m_nX;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (bReturnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    cVector = new CVector3();//在同一列，目标正左下方
                    cVector.m_nU = oSrc.m_nU;
                    cVector.m_nX = oSrc.m_nX - num;
                    cVector.m_nY = cVector.m_nX + cVector.m_nU;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (bReturnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    cVector = new CVector3();//在同一列，目标正右下方
                    cVector.m_nY = oSrc.m_nY;
                    cVector.m_nU = oSrc.m_nU - num;
                    cVector.m_nX = cVector.m_nY - cVector.m_nU;
                    if (!this.HasMapNode(cVector))
                    {
                        break;
                    }
                    if (bReturnObstruct || !this.IsPosBlocked(cVector))
                    {
                        oFindedNodes.Add(cVector);
                    }
                    num++;
                }
            }
        }
        else 
        {
            if (oSrc.m_nY == oDest.m_nY)//在同一y列
            {
                if (oSrc.m_nX <= oDest.m_nX)//目标在下方
                {
                    int num = 1;
                    while (num <= unDis)
                    {
                        CVector3 cVector = new CVector3();//同一y下方
                        cVector.m_nY = oSrc.m_nY;
                        cVector.m_nX = oSrc.m_nX + num;
                        cVector.m_nU = cVector.m_nY - cVector.m_nX;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (bReturnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        cVector = new CVector3();
                        cVector.m_nU = oSrc.m_nU;
                        cVector.m_nX = oSrc.m_nX + num;
                        cVector.m_nY = cVector.m_nX + cVector.m_nU;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (bReturnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        cVector = new CVector3();
                        cVector.m_nX = oSrc.m_nX;
                        cVector.m_nY = oSrc.m_nY - num;
                        cVector.m_nU = cVector.m_nY - cVector.m_nX;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (bReturnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        num++;
                    }
                }
                else
                {
                    int num = 1;
                    while (num <= unDis)
                    {
                        CVector3 cVector = new CVector3();
                        cVector.m_nY = oSrc.m_nY;
                        cVector.m_nX = oSrc.m_nX - num;
                        cVector.m_nU = cVector.m_nY - cVector.m_nX;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (bReturnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        cVector = new CVector3();
                        cVector.m_nU = oSrc.m_nU;
                        cVector.m_nX = oSrc.m_nX - num;
                        cVector.m_nY = cVector.m_nX + cVector.m_nU;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (bReturnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        cVector = new CVector3();
                        cVector.m_nX = oSrc.m_nX;
                        cVector.m_nY = oSrc.m_nY + num;
                        cVector.m_nU = cVector.m_nY - cVector.m_nX;
                        if (!this.HasMapNode(cVector))
                        {
                            break;
                        }
                        if (bReturnObstruct || !this.IsPosBlocked(cVector))
                        {
                            oFindedNodes.Add(cVector);
                        }
                        num++;
                    }
                }
            }
            else 
            {
                if (oSrc.m_nU == oDest.m_nU)
                {
                    if (oSrc.m_nX <= oDest.m_nX)
                    {
                        int num = 1;
                        while (num <= unDis)
                        {
                            CVector3 cVector = new CVector3();
                            cVector.m_nX = oSrc.m_nX + num;
                            cVector.m_nU = oSrc.m_nU;
                            cVector.m_nY = cVector.m_nX + cVector.m_nU;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (bReturnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            cVector = new CVector3();
                            cVector.m_nX = oSrc.m_nX;
                            cVector.m_nY = oSrc.m_nY + num;
                            cVector.m_nU = cVector.m_nY - cVector.m_nX;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (bReturnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            cVector = new CVector3();
                            cVector.m_nY = oSrc.m_nY;
                            cVector.m_nU = oSrc.m_nU - num;
                            cVector.m_nX = cVector.m_nY - cVector.m_nU;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (bReturnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            num++;
                        }
                    }
                    else 
                    {
                        int num = 1;
                        while (num <= unDis)
                        {
                            CVector3 cVector = new CVector3();
                            cVector.m_nX = oSrc.m_nX - num;
                            cVector.m_nU = oSrc.m_nU;
                            cVector.m_nY = cVector.m_nX + cVector.m_nU;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (bReturnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            cVector = new CVector3();
                            cVector.m_nX = oSrc.m_nX;
                            cVector.m_nY = oSrc.m_nY - num;
                            cVector.m_nU = cVector.m_nY - cVector.m_nX;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (bReturnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            cVector = new CVector3();
                            cVector.m_nY = oSrc.m_nY;
                            cVector.m_nU = oSrc.m_nU + num;
                            cVector.m_nX = cVector.m_nY - cVector.m_nU;
                            if (!this.HasMapNode(cVector))
                            {
                                break;
                            }
                            if (bReturnObstruct || !this.IsPosBlocked(cVector))
                            {
                                oFindedNodes.Add(cVector);
                            }
                            num++;
                        }
                    }
                }
            }
        }
        return true;
    }
    public bool IsRay(CVector3 oSrc, CVector3 oDest1, CVector3 oDest2)
    {
        return this.m_oMapData.IsRay(oSrc, oDest1, oDest2);
    }
    public void GetAdjacentNodes(CVector3 oSrc, ref List<CVector3> oFindedNodes)
    {
        if (!this.m_oTablePathNodes.ContainsKey(oSrc.m_nX) || !this.m_oTablePathNodes[oSrc.m_nX].ContainsKey(oSrc.m_nY))
        {
            this.logger.Fatal(string.Format("GetAdjacentNodes:oSrc({0}) is not exist:",oSrc.ToString()));
            return;
        }
        PathNode mapNode = this.m_oTablePathNodes[oSrc.m_nX][oSrc.m_nY];
        for (int i = 0; i < 6; i++)
        {
            PathNode nearNode = mapNode.m_listNextNode[i];
            if (nearNode != null && nearNode.m_bCanWalk)
            {
                CVector3 cVector = new CVector3();
                cVector.m_nX = nearNode.nIndexX;
                cVector.m_nY = nearNode.nIndexY;
                cVector.m_nU = nearNode.nIndexU;
                if (!oFindedNodes.Contains(cVector))
                {
                    oFindedNodes.Add(cVector);
                }
            }
        }
    }
    public List<CVector3> GetNodesByType(EMapNodeType eMapNodeType)
    {
        List<MapNode> list = this.m_oMapData.GetNodesByType(eMapNodeType);
        List<CVector3> list2 = new List<CVector3>();
        foreach (var item in list)
        {
            list2.Add(new CVector3
            {
                nRow = item.nRow,
                nCol = item.nCol
            });
        }
        return list2;
    }
    public void OnRoleDie(uint unHeroId)
    {
        if (!this.m_oDicRolesPos.ContainsKey(unHeroId))
        {
            return;
        }
        int nX = this.m_oDicRolesPos[unHeroId].m_nX;
        int nY = this.m_oDicRolesPos[unHeroId].m_nY;
        this.m_oTablePathNodes[nX][nY].m_bCanWalk = true;
        this.m_oTablePathNodes[nX][nY].m_BeastID = 0u;
    }
    public uint CalDistance(CVector3 oSrcPos, CVector3 oDstPos)
    {
        int xDis = oSrcPos.m_nX > oDstPos.m_nX ? oSrcPos.m_nX - oDstPos.m_nX : oDstPos.m_nX - oSrcPos.m_nX;
        int yDis = oSrcPos.m_nY > oDstPos.m_nY ? oSrcPos.m_nY - oDstPos.m_nY : oDstPos.m_nY - oSrcPos.m_nY;
        int uDis = oSrcPos.m_nU > oDstPos.m_nU ? oSrcPos.m_nU - oDstPos.m_nU : oDstPos.m_nU - oSrcPos.m_nU;
        int dis1 = xDis + yDis;
        int dis2 = xDis + uDis;
        int dis3 = uDis + yDis;
        int dis = dis1;
        if (dis2 < dis1)
        {
            dis = dis2;
        }
        if (dis3 < dis1)
        {
            dis = dis3;
        }
        if (dis < 0)
        {
            return 4294967295u;
        }
        return (uint)dis;
    }
    public bool IsAdjacent(CVector3 oPos1, CVector3 oPos2)
    {
        return (oPos1.m_nX == oPos2.m_nX || oPos1.m_nY == oPos2.m_nY || oPos1.m_nU == oPos2.m_nU) && (Mathf.Abs(oPos1.m_nX - oPos2.m_nX) == 1 || Mathf.Abs(oPos1.m_nY - oPos2.m_nY) == 1 || Mathf.Abs(oPos1.m_nU - oPos2.m_nU) == 1);
    }
    #region 私有函数
    /// <summary>
    /// map初始化完成之后,将初始化路径节点和路径节点的下个节点
    /// </summary>
    private void OnInitFinished()
    {
        //初始化PathNode字典集合
        foreach (var current in this.m_oMapData.DicMapData)
        {
            Dictionary<int, PathNode> dicPathNodes = new Dictionary<int, PathNode>();
            foreach (var current2 in current.Value)
            {
                PathNode pathNode = new PathNode();
                pathNode.nIndexX = current2.Value.nIndexX;
                pathNode.nIndexY = current2.Value.nIndexY;
                pathNode.nIndexU = current2.Value.nIndexU;
                pathNode.m_bCanWalk = current2.Value.m_bCanWalk;
                dicPathNodes.Add(current2.Key, pathNode);
            }
            this.m_oTablePathNodes.Add(current.Key, dicPathNodes);
        }
        //判断该路径节点6个方向上的节点是否存在，如果存在就加到下个路径中
        foreach (var current in this.m_oTablePathNodes)
        {
            foreach (var current2 in current.Value)
            {
                int nIndexX = current.Key;
                int nIndexY = current2.Key;
                if (this.m_oMapData.HasMapNode(nIndexX + 1, nIndexY + 1))
                {
                    current2.Value.m_listNextNode.Add(this.m_oTablePathNodes[nIndexX + 1][nIndexY + 1]);
                }
                else 
                {
                    current2.Value.m_listNextNode.Add(null);
                }
                if (this.m_oMapData.HasMapNode(nIndexX, nIndexY + 1))
                {
                    current2.Value.m_listNextNode.Add(this.m_oTablePathNodes[nIndexX][nIndexY + 1]);
                }
                else 
                {
                    current2.Value.m_listNextNode.Add(null);
                }
                if (this.m_oMapData.HasMapNode(nIndexX - 1, nIndexY))
                {
                    current2.Value.m_listNextNode.Add(this.m_oTablePathNodes[nIndexX - 1][nIndexY]);
                }
                else
                {
                    current2.Value.m_listNextNode.Add(null);
                }
                if (this.m_oMapData.HasMapNode(nIndexX - 1, nIndexY - 1))
                {
                    current2.Value.m_listNextNode.Add(this.m_oTablePathNodes[nIndexX - 1][nIndexY - 1]);
                }
                else
                {
                    current2.Value.m_listNextNode.Add(null);
                }
                if (this.m_oMapData.HasMapNode(nIndexX, nIndexY - 1))
                {
                    current2.Value.m_listNextNode.Add(this.m_oTablePathNodes[nIndexX][nIndexY - 1]);
                }
                else
                {
                    current2.Value.m_listNextNode.Add(null);
                }
                if (this.m_oMapData.HasMapNode(nIndexX + 1, nIndexY))
                {
                    current2.Value.m_listNextNode.Add(this.m_oTablePathNodes[nIndexX + 1][nIndexY]);
                }
                else
                {
                    current2.Value.m_listNextNode.Add(null);
                }            
            }
        }
    }
    #endregion
}