using System;
using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CollectDepResourceDataMap
// 创建者：chen
// 修改者列表：
// 创建日期：2015.12.24
// 模块描述：引用其他资源的资源管理器
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
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
        private static Dictionary<string, ResourceData> s_dicAllResourceData = new Dictionary<string, ResourceData>();
        /// <summary>
        /// 根据资源名称从ALL中取得资源
        /// </summary>
        /// <param name="name">UI名称</param>
        /// <returns></returns>
        public static ResourceData GetResourceData(string name)
        {
            ResourceData result = null;
            if (!CollectDepResourceDataMap.s_dicAllResourceData.TryGetValue(name, out result))
            {
                Debug.Log(name);
                return null;
            }
            return result;
        }
        /// <summary>
        /// 增加资源到All所有资源集合&lt;string,ResourceData&gt;中
        /// </summary>
        /// <param name="dicResourceData"></param>
        public static void AddResourceDatas(Dictionary<string, ResourceData> dicResourceData)
        {
            foreach (ResourceData current in dicResourceData.Values)
            {
                Debug.Log(current.mResourceName);
                if (CollectDepResourceDataMap.s_dicAllResourceData.ContainsKey(current.mResourceName))
                {
                    CollectDepResourceDataMap.s_dicAllResourceData[current.mResourceName].mRefCount += current.mRefCount;//如果已经有这个资源就增加引用次数
                }
                else
                {
                    CollectDepResourceDataMap.s_dicAllResourceData[current.mResourceName] = current;
                }
            }
        }
        /// <summary>
        /// 创建资源数据，如果已经存在就直接从中取得
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <param name="eAssetType"></param>
        /// <returns>资源</returns>
        public static ResourceData RefResource(string name, string path, int size, EnumAssetType eAssetType)
        {
            ResourceData resourceData = null;
            if (!CollectDepResourceDataMap.s_dicAllResourceData.TryGetValue(name, out resourceData))
            {
                resourceData = ResourceData.Create(name, path, size, eAssetType);
                resourceData.mHasCheckRef = true;
                CollectDepResourceDataMap.s_dicAllResourceData[name] = resourceData;
            }
            else
            {
                if (!resourceData.mHasCheckRef)
                {
                    resourceData.mHasCheckRef = true;
                    resourceData.mRefCount++;
                }
            }
            return resourceData;
        }
        /// <summary>
        /// 清除已经从配置文件中加载的集合资源
        /// </summary>
        public void Clear()
        {
            this.mDicCollectDepResourceData.Clear();
            this.mDicResourceData.Clear();
        }
        /// <summary>
        /// 根据名字取得引用资源实例，并初始化ResourceData和List&lt;ResourceData&gt;引用资源
        /// </summary>
        /// <param name="name">资源名字</param>
        /// <param name="resourceData">资源</param>
        /// <param name="depends">引用的资源</param>
        /// <returns>引用的资源</returns>
        public CollectDepResourceData GetCollectDepResourceData(string name, out ResourceData resourceData, out List<ResourceData> depends)
        {
            CollectDepResourceData collectDepResourceData = this.GetCollectDepResourceData(name);
            if (collectDepResourceData == null)
            {
                resourceData = null;
                depends = null;
                return null;
            }
            resourceData = CollectDepResourceDataMap.GetResourceData(collectDepResourceData.mResourceName);
            depends = new List<ResourceData>();
            List<string> mDependResourceName = collectDepResourceData.mDependResourceName;//该资源引用其他资源的名称集合
            for (int i = 0; i < mDependResourceName.Count; i++)
            {
                ResourceData resourceData2 = CollectDepResourceDataMap.GetResourceData(mDependResourceName[i]);
                //Debug.Log("dependName:" + resourceData2.mResourceName);
                depends.Add(resourceData2);
            }
            return collectDepResourceData;
        }
        /// <summary>
        /// 根据名字取得有引用的资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private CollectDepResourceData GetCollectDepResourceData(string name)
        {
            CollectDepResourceData result = null;
            if (!this.mDicCollectDepResourceData.TryGetValue(name, out result))
            {
                AssetLogger.Error(string.Format("mDicCollectDepResourceData.TryGetValue(name, out collectDepResourceData) == false:{0}", name));
            }
            return result;
        }
    }
}
