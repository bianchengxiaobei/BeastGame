using UnityEngine;
using System;
using System.Text;
using System.Diagnostics;
using UnityPlugin.Export;
using System.Threading;
using Utility.Export;
using System.IO;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：WindowsPluginToolImpl
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：Window平台SDK实现工具
//----------------------------------------------------------------*/
#endregion
public class WindowsPluginToolImpl : IPluginTool
{
    private bool m_bCreatedMutexNew;
    private Mutex m_mutex;
    private IXLog m_log = XLog.GetLog<WindowsPluginToolImpl>();
    public virtual EnumPlatformType EPlatformType
    {
        get
        {
            return EnumPlatformType.ePlatformType_KZ;
        }
    }
    private static FileInfo LauncherFileInfo
    {
        get
        {
            return new FileInfo("../launcher.exe");
        }
    }
    private static bool Launched
    {
        get
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            for (int i = 0; i < commandLineArgs.Length; i++)
            {
                string text = commandLineArgs[i];
                if (text.Equals("-launch", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static bool NeedToRunLauncher
    {
        get
        {
            return WindowsPluginToolImpl.UsedOnThisPlatform && !WindowsPluginToolImpl.Launched;
        }
    }
    private static bool UsedOnThisPlatform
    {
        get
        {
            return true;
        }
    }
    public virtual bool Init()
    {
        if (WindowsPluginToolImpl.NeedToRunLauncher && Application.platform == RuntimePlatform.WindowsPlayer && WindowsPluginToolImpl.QuitClientAndLauncher())
        {
            return false;
        }
        this.m_mutex = new Mutex(true, "{E5520BF8-2230-4a81-A0F3-724391E3653D}", out this.m_bCreatedMutexNew);
        this.m_log.Debug("{E5520BF8-2230-4a81-A0F3-724391E3653D}:" + this.m_bCreatedMutexNew);
        return true;
    }
    public virtual void UnInit()
    {
        if (this.m_mutex != null)
        {
            if (this.m_bCreatedMutexNew)
            {
                this.m_mutex.ReleaseMutex();
            }
            this.m_mutex.Close();
            this.m_mutex = null;
        }
    }
    public int BeginCopyDataFiles(string assetDir)
    {
        return 0;
    }
    public void CopyAFile(string srcFile, string destFile)
    {
    }
    public string GetResPath()
    {
        return string.Format("{0}/..", Application.dataPath);
    }
    public string GetBundleId()
    {
        return string.Empty;
    }
    public string GetBundleVersion()
    {
        return string.Empty;
    }
    public string GetCopyFile(int index)
    {
        return string.Empty;
    }
    public bool HasDataFile(string path)
    {
        return false;
    }
    public void Install(string file)
    {
        PluginTool.Singleton.PushEvent(EnumPluginEventType.ePluginEventType_Install_Success);
    }
    public void InstallApp(string appPath)
    {
    }
    public virtual void Login()
    {
    }
    public virtual void LoginOut()
    {
    }
    public virtual string GetToken()
    {
        return string.Empty;
    }
    public virtual string GetUserId()
    {
        return string.Empty;
    }
    public virtual void Pay(string strUserAccount, string strGameAreaId)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(strUserAccount);
            string arg = Convert.ToBase64String(bytes);
            string url = string.Format("{0}?useraccount={1}&gameareaid={2}", PluginTool.Singleton.PayUrl, arg, strGameAreaId);
            Application.OpenURL(url);
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
    }
    public void RegisterAccount()
    {
        Application.OpenURL(PluginTool.Singleton.RegisterUrl);
    }
    public void ForgetAccount()
    {
        Application.OpenURL(PluginTool.Singleton.ForgetUrl);
    }
    public static bool QuitClientAndLauncher()
    {
        XLog.GetLog<WindowsPluginToolImpl>().Error("Lybns was not run from Launcher");
        if (!WindowsPluginToolImpl.LauncherFileInfo.Exists)
        {
            XLog.GetLog<WindowsPluginToolImpl>().Error("could not find Launcher");
            return false;
        }
        try
        {
            new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = WindowsPluginToolImpl.LauncherFileInfo.FullName,
                    Arguments = string.Empty
                },
                EnableRaisingEvents = true
            }.Start();
            XLog.GetLog<WindowsPluginToolImpl>().Debug("Launcher is running!");
            PluginTool.Singleton.HasAssuredToQuit = true;
            Application.Quit();
        }
        catch (Exception ex)
        {
            XLog.GetLog<WindowsPluginToolImpl>().Fatal(ex.ToString());
        }
        return true;
    }
}