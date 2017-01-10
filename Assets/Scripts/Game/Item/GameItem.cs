using UnityEngine;
using System.Collections;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameItem 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.16
// 模块描述：角色装备信息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色装备信息
/// </summary>
public class GameItem : object,IComparable
{
    #region 字段
    public int templateId = -1;
    public string mdlPath = "";
    public string mdlPartName = "";
    public string anchorNodeName = "";
    public int equipPos = -1;
    public int bagIndex = -1;
    public int count = 0;
    public int quility = 0;
    public int[] gemList = null;
    #endregion
    #region 构造方法
    #endregion
    #region 公共方法
    public int CompareTo(object item)
    {
        return 1;
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
