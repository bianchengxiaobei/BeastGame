using UnityEngine;
using System.Collections;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LayerManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：UI层级管理器
//----------------------------------------------------------------*/
#endregion
public class LayerManager : Singleton<LayerManager>
{
    private int m_normalLayer = LayerMask.NameToLayer("NGUI");
    private int m_topLayer = LayerMask.NameToLayer("NGUITop");
    /// <summary>
    /// 普通层，“NGUI”
    /// </summary>
    public int NormalLayer
    {
        get
        {
            return this.m_normalLayer;
        }
    }
    /// <summary>
    /// 定级层，“NGUITop”
    /// </summary>
    public int TopLayer
    {
        get
        {
            return this.m_topLayer;
        }
    }
}
