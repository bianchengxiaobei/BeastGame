using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PluginEvent
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityPlugin.Export
{
    public class PluginEvent
    {
        public EnumPluginEventType Type
        {
            get;
            set;
        }
        public string Param
        {
            get;
            set;
        }
    }
    public enum EnumPluginEventType
    {
        ePluginEventType_Install_Fail,
        ePluginEventType_Install_Success,
        ePluginEventType_Update_FailGetUpdateInfo,
        ePluginEventType_Update_FailParseUpdateInfo,
        ePluginEventType_Update_LatestVersion,
        ePluginEventType_Update_NeedUpdateApp,
        ePluginEventType_Update_FailGetFileList,
        ePluginEventType_Update_FailGetFile,
        ePluginEventType_Update_Success,
        ePluginEventType_Login_Success,
        ePluginEventType_Login_Fail
    }
}
