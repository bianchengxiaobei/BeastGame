using UnityEngine;
using System.Collections;
using UnityPlugin.Export;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PluginToolImplManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：平台插件管理器
//----------------------------------------------------------------*/
#endregion
public class PluginToolImplManager : MonoBehaviour,IPluginToolImplManager
{
    /// <summary>
    /// 根据不同的平台得到不同的平台插件工具
    /// </summary>
    /// <param name="ePlatformType"></param>
    /// <returns></returns>
    public IPluginTool GetPluginTool(EnumPlatformType ePlatformType)
    {
        IPluginTool result = null;
        if (Application.platform == RuntimePlatform.Android)
        {
            result = null;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = null;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Type type = Type.GetType("WindowsPluginToolImpl");
            if (ePlatformType == EnumPlatformType.ePlatformType_Baidu)
            {
                type = Type.GetType("WindowsBaiduPluginToolImpl");
            }
            result = (IPluginTool)Activator.CreateInstance(type);
        }
        return result;
    }
}
