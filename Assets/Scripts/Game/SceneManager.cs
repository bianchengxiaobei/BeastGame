using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SceneManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.15
// 模块描述：地图场景加载管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 地图场景加载管理器
/// </summary>
public class SceneManager 
{
    #region 字段
    private int m_lastSceneId = -1;
    private string m_lastSceneResourceName = string.Empty;
    private List<UnityEngine.Object> m_sceneObjects = new List<Object>();
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    #endregion
    #region 公共方法
    /// <summary>
    /// 检测是否加载跟现在一样的场景
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CheckSameScene(int id)
    {
        if (m_lastSceneId == id)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
