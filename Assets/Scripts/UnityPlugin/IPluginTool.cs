using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IPluginTool
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityPlugin.Export
{
    public interface IPluginTool
    {
        EnumPlatformType EPlatformType
        {
            get;
        }
        int BeginCopyDataFiles(string assetDir);
        void CopyAFile(string srcFile, string destFile);
        string GetResPath();
        string GetBundleId();
        string GetBundleVersion();
        string GetCopyFile(int index);
        bool HasDataFile(string path);
        bool Init();
        void UnInit();
        void Install(string file);
        void InstallApp(string appPath);
        void Login();
        void LoginOut();
        string GetToken();
        string GetUserId();
        void Pay(string strUserAccount, string strGameAreaId);
        void RegisterAccount();
        void ForgetAccount();
    }
    public enum EnumPlatformType
    {
        ePlatformType_KZ = 1,
        ePlatformType_Baidu,
        ePlatformType_Undef
    }
}