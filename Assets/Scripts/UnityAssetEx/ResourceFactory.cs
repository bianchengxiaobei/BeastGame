using UnityEngine;
using Utility.Export;
using UnityAssetEx.Local;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ResourceFactory
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：资源管理器工厂
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public class ResourceFactory
    {
        /// <summary>
        /// 取得本地资源管理器
        /// </summary>
        /// <returns></returns>
        public static IResourceManager GetResourceManager()
        {
            return LocalResourceManager.GetInstance();
        }
        public static void SetLog(IXLog log)
        {
            AssetLogger.Init(log);
        }
    }
}
