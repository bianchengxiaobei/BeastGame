using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class MapBehaviour : MonoBehaviour
{
    private static MapBehaviour s_instance = null;
    public static MapBehaviour Instance { get { return MapBehaviour.s_instance; } }
    private void Awake()
    {
        MapBehaviour.s_instance = this;
    }
    private void Start()
    {
        if (UnityGameEntry.Instance != null)
        {
            CSceneMgr.singleton.OnMapBehaviourPrepared();
        }
    }
    private void Update()
    {
 
    }
}
