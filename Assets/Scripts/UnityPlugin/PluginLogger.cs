using UnityEngine;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PluginLogger
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityPlugin.Local
{
    internal class PluginLogger
    {
        private static IXLog logger;
        public static void Init(IXLog logger)
        {
            PluginLogger.logger = logger;
        }
        public static void PluginDebug(object A)
        {
            if (PluginLogger.logger != null)
            {
                PluginLogger.logger.Debug(A);
                return;
            }
            Debug.Log(A);
        }
        public static void PluginError(object A)
        {
            if (PluginLogger.logger != null)
            {
                PluginLogger.logger.Error(A);
                return;
            }
            Debug.LogError(A);
        }
        public static void PluginFatal(object A)
        {
            if (PluginLogger.logger != null)
            {
                PluginLogger.logger.Fatal(A);
                return;
            }
            Debug.LogError(A);
        }
    }
}
