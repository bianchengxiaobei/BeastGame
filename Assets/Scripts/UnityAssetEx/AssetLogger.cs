using UnityEngine;
using System;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：AssetLogger
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public class AssetLogger
    {
        private static IXLog s_log;
        private static EnumLogLevel s_eLogLevel;
        public static EnumLogLevel LogLevel
        {
            get
            {
                return AssetLogger.s_eLogLevel;
            }
            set
            {
                AssetLogger.s_eLogLevel = value;
            }
        }
        public static void Init(IXLog log)
        {
            AssetLogger.s_log = log;
        }
        public static void Debug(object message)
        {
            if (EnumLogLevel.eLogLevel_Debug < AssetLogger.s_eLogLevel)
            {
                return;
            }
            if (AssetLogger.s_log != null)
            {
                AssetLogger.s_log.Debug(message);
                return;
            }
            UnityEngine.Debug.Log(message);
        }
        public static void Error(object message)
        {
            if (EnumLogLevel.eLogLevel_Error < AssetLogger.s_eLogLevel)
            {
                return;
            }
            if (AssetLogger.s_log != null)
            {
                AssetLogger.s_log.Error(message);
                return;
            }
            UnityEngine.Debug.LogError(message);
        }
        public static void Fatal(object message)
        {
            if (EnumLogLevel.eLogLevel_Fatal < AssetLogger.s_eLogLevel)
            {
                return;
            }
            if (AssetLogger.s_log != null)
            {
                AssetLogger.s_log.Fatal(message);
                return;
            }
            UnityEngine.Debug.LogError(message);
        }
    }
    public enum EnumLogLevel
    {
        eLogLevel_Debug,
        eLogLevel_Error,
        eLogLevel_Fatal,
        eLogLevel_Max
    }
}
