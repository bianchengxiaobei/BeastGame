using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CollectDepResourceData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class CollectDepResourceData 
{
    /// <summary>
    /// 引用资源的名称
    /// </summary>
    public string mResourceName;
    /// <summary>
    /// 引用其他资源的名字集合
    /// </summary>
    public List<string> mDependResourceName = new List<string>();

    public CollectDepResourceData(string name, List<string> deps)
    {
        this.mResourceName = name;
        this.mDependResourceName = deps;
    }
}
