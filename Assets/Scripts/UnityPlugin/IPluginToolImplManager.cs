using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IPluginToolImplManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityPlugin.Export
{
    public interface IPluginToolImplManager
    {
        IPluginTool GetPluginTool(EnumPlatformType ePlatformType);
    }
}

