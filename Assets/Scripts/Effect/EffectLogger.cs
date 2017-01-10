using UnityEngine;
using System.Collections;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EffectLogger
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect.Export
{
    public class EffectLogger
    {
        private static IXLog s_log = null;
        private static EnumLogLevel s_eLogLevel = EnumLogLevel.eLogLevel_Debug;
        public static EnumLogLevel LogLevel
        {
            get
            {
                return EffectLogger.s_eLogLevel;
            }
            set
            {
                EffectLogger.s_eLogLevel = value;
            }
        }
        public static void Init(IXLog log)
        {
            EffectLogger.s_log = log;
        }
        public static void Debug(object message)
        {
            if (EnumLogLevel.eLogLevel_Debug >= EffectLogger.s_eLogLevel)
            {
                if (null != EffectLogger.s_log)
                {
                    EffectLogger.s_log.Debug(message);
                }
                else
                {
                    UnityEngine.Debug.Log(message);
                }
            }
        }
        public static void Error(object message)
        {
            if (EnumLogLevel.eLogLevel_Error >= EffectLogger.s_eLogLevel)
            {
                if (null != EffectLogger.s_log)
                {
                    EffectLogger.s_log.Error(message);
                }
                else
                {
                    UnityEngine.Debug.LogError(message);
                }
            }
        }
        public static void Fatal(object message)
        {
            if (EnumLogLevel.eLogLevel_Fatal >= EffectLogger.s_eLogLevel)
            {
                if (null != EffectLogger.s_log)
                {
                    EffectLogger.s_log.Fatal(message);
                }
                else
                {
                    UnityEngine.Debug.LogError(message);
                }
            }
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
