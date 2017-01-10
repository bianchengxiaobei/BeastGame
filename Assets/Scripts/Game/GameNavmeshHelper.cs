using UnityEngine;
using System.Collections;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameNavmeshHelper 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：Navmesh帮助类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// Navmesh帮助类
/// </summary>
public class GameNavmeshHelper 
{
    #region 字段
    Transform m_parent;
    GameObject m_navGo;
    NavMeshAgent m_navAgent;
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public GameNavmeshHelper(Transform parent)
    {
        m_parent = parent;
        //添加一个子GameObject到parent中，用于寻路
        m_navAgent = parent.GetComponentInChildren<NavMeshAgent>();
        if (m_navAgent != null)
        {
            m_navGo = m_navAgent.gameObject;
            m_navAgent.enabled = false;
        }
        else
        {
            m_navGo = new GameObject();
            UnityTools.ChangeObjectParentDontChangeLocalTransform(m_navGo.transform, parent);
            ResetPositionToCLoseToNavMesh();
            m_navAgent = m_navGo.AddComponent<NavMeshAgent>();
            m_navAgent.enabled = false;
            m_navAgent.height = 2f;
            m_navAgent.radius = 1f;
        }
    }
    #endregion
    #region 公共方法
    public NavMeshPath GetPathByTarget(Vector3 target)
    {
        if (m_navGo == null)
        {
            m_navGo = new GameObject();
        }
        NavMeshPath path = new NavMeshPath();
        target = GetPointCloseToTheMesh(target);
        if (!ResetPositionToCLoseToNavMesh())
        {
            Debug.LogWarning("不能发现Navmesh");
            return path;
        }
        try
        {
            NavMesh.CalculatePath(m_navGo.transform.position, target, -1, path);
            if (path.corners.Length <= 0)
            {
                m_navAgent.enabled = true;

                m_navAgent.SetDestination(target);
                path = m_navAgent.path;
                if (path.corners.Length <= 0)
                {
                    Debug.LogError("Path's corner Count <= 0");
                }
                m_navAgent.Stop();
            }
        }
        catch
        {
           
        }
        for (int i = 0; i < path.corners.Length; i++)
        {
            UnityTools.GetPointInTerrain(path.corners[i].x, path.corners[i].z, out  path.corners[i]);
        }

        return path;
    }
    #endregion
    #region 私有方法

    private bool ResetPositionToCLoseToNavMesh()
    {
        Vector3 sourcePostion = m_parent.position;//The position to place agent
        m_navGo.transform.position = sourcePostion;
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(sourcePostion, out closestHit, 10, -1))
        {
            m_navGo.transform.position = closestHit.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetPointCloseToTheMesh(Vector3 sourcePostion)
    {
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(sourcePostion, out closestHit, 10, -1))
        {
            return closestHit.position;
        }
        else
        {
            return sourcePostion;
        }
    }
    #endregion
    #region 析构方法
    #endregion
}
