using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapNodeManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：路径节点类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 路径节点类
/// </summary>
internal class PathNode
{
    public int nIndexX;
    public int nIndexY;
    public int nIndexU;
    public bool m_bCanWalk;
    public int distance;
    public int maxDistance;
    public PathNode parentMapNode;
    public List<PathNode> m_listNextNode;
    public long m_BeastID;
    public byte sum;
    public PathNode()
    {
        this.nIndexX = 0;
        this.nIndexY = 0;
        this.nIndexU = 0;
        this.distance = 0;
        this.m_bCanWalk = false;
        this.m_listNextNode = new List<PathNode>();
        this.m_BeastID = 0;
        this.sum = 0;
    }
}
