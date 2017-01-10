using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CollectDepResourceDataMap
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class CollectDepResourceDataMap 
{
    /// <summary>
    /// 引用其他资源的资源集合key=>资源名字（可以解析出路径）,value=>CollectDepResourceData（资源名+List&lt;string&gt;引用的资源名集合）
    /// </summary>
    public Dictionary<string, CollectDepResourceData> mDicCollectDepResourceData = new Dictionary<string, CollectDepResourceData>();
    /// <summary>
    /// 游戏中某一分类的资源集合key=>资源名字（可以解析出路径）,value=>ResourceData（资源名+资源大小+资源路径+资源被引用次数）
    /// </summary>
    public Dictionary<string, ResourceData> mDicResourceData = new Dictionary<string, ResourceData>();
    public void Init()
    {

    }
    public void InitResourceData(AssetUnit unit,Dictionary<string,AssetUnit> allunit)
    {
        if (!this.mDicResourceData.ContainsKey(unit.mName))
        {
            ResourceData data = ResourceData.Create(unit.mName, unit.mPath, unit.mAssetSize, unit.mType);
            data.mRefCount = unit.mRefCount;
            data.mHasCheckRef = false;
            this.mDicResourceData.Add(unit.mName, data);
             List<string> deps = unit.mAllDependencies;
            foreach (var dep in deps)
            {
                //string name = BuildCommon.GetLevelABPathName(dep);
                if (allunit.ContainsKey(dep))
                {
                    AssetUnit unit1 = allunit[dep];
                    ResourceData data1 = ResourceData.Create(unit1.mName, unit1.mPath, unit1.mAssetSize, unit1.mType);
                    data1.mRefCount = unit1.mRefCount;
                    data1.mHasCheckRef = false;
                    if (!this.mDicResourceData.ContainsKey(unit1.mName))
                    {
                        this.mDicResourceData.Add(unit1.mName, data1);
                    }
                }
                else 
                {
                    continue;
                }
            }
        }
    }
    public void InitCollectDepData(AssetUnit unit)
    {
        if (!this.mDicCollectDepResourceData.ContainsKey(unit.mName))
        {
            List<string> deps = unit.mAllDependencies;
            List<string> temp = new List<string>();
            foreach (var dep in deps)
            {
                string name = BuildCommon.GetLevelABPathName(dep);
                temp.Add(name);
            }
            this.mDicCollectDepResourceData.Add(unit.mName, new CollectDepResourceData(unit.mName, temp));
        }
    }
}
