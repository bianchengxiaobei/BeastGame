using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using UnityPlugin.Local;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PluginTool
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityPlugin.Export
{
    public class PluginTool
    {
        #region 字段
        private IPluginTool m_pluginToolImpl;
        private bool m_bHasAssuredToQuit;
        private Queue<PluginEvent> m_queuePluginEvent = new Queue<PluginEvent>();
        private static PluginTool mInst;
        #endregion
        #region 属性
        public static PluginTool Singleton
        {
            get
            {
                if (PluginTool.mInst == null)
                {
                    PluginTool.mInst = new PluginTool();
                }
                return PluginTool.mInst;
            }
        }
        public EnumPlatformType EPlatformType
        {
            get
            {
                if (this.m_pluginToolImpl == null)
                {
                    return EnumPlatformType.ePlatformType_Undef;
                }
                return this.m_pluginToolImpl.EPlatformType;
            }
        }
        public bool HasAssuredToQuit
        {
            get
            {
                return this.m_bHasAssuredToQuit;
            }
            set
            {
                this.m_bHasAssuredToQuit = value;
            }
        }
        public string OutputInfo
        {
            get;
            set;
        }
        public string PayUrl
        {
            get;
            set;
        }
        public string RegisterUrl
        {
            get;
            set;
        }
        public string ForgetUrl
        {
            get;
            set;
        }
        #endregion
        private PluginTool()
        {
        }
        public void SetLog(IXLog log)
        {
            PluginLogger.Init(log);
        }
        public bool Init(IPluginTool pluginTool)
        {
            if (pluginTool == null)
            {
                return false;
            }
            this.m_pluginToolImpl = pluginTool;
            return this.m_pluginToolImpl.Init();
        }
        public void UnInit()
        {
            if (this.m_pluginToolImpl != null)
            {
                this.m_pluginToolImpl.UnInit();
            }
        }
        public void Update()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                PluginLogger.PluginFatal(ex.ToString());
            }
        }
        public void Install(string file)
        {
            if (this.m_pluginToolImpl != null)
            {
                this.m_pluginToolImpl.Install(file);
            }
        }
        internal int BeginCopyDataFiles(string assetDir)
        {
            if (this.m_pluginToolImpl == null)
            {
                return 0;
            }
            return this.m_pluginToolImpl.BeginCopyDataFiles(assetDir);
        }
        internal void CopyAFile(string srcFile, string destFile)
        {
            if (this.m_pluginToolImpl == null)
            {
                return;
            }
            this.m_pluginToolImpl.CopyAFile(srcFile, destFile);
        }
        public string GetResPath()
        {
            if (this.m_pluginToolImpl == null)
            {
                return string.Empty;
            }
            return this.m_pluginToolImpl.GetResPath();
        }
        public string GetBundleId()
        {
            if (this.m_pluginToolImpl == null)
            {
                return string.Empty;
            }
            return this.m_pluginToolImpl.GetBundleId();
        }
        public string GetBundleVersion()
        {
            if (this.m_pluginToolImpl == null)
            {
                return string.Empty;
            }
            return this.m_pluginToolImpl.GetBundleVersion();
        }
        internal string GetCopyFile(int index)
        {
            if (this.m_pluginToolImpl == null)
            {
                return string.Empty;
            }
            return this.m_pluginToolImpl.GetCopyFile(index);
        }
        internal bool HasDataFile(string path)
        {
            return this.m_pluginToolImpl != null && this.m_pluginToolImpl.HasDataFile(path);
        }
        public bool PopEvent(out PluginEvent pluginEvent)
        {
            if (this.m_queuePluginEvent.Count > 0)
            {
                pluginEvent = this.m_queuePluginEvent.Dequeue();
                return true;
            }
            pluginEvent = null;
            return false;
        }
        public PluginEvent PushEvent(EnumPluginEventType ePluginEventType)
        {
            PluginEvent pluginEvent = new PluginEvent();
            pluginEvent.Type = ePluginEventType;
            this.m_queuePluginEvent.Enqueue(pluginEvent);
            return pluginEvent;
        }
        public void Login()
        {
            if (this.m_pluginToolImpl == null)
            {
                return;
            }
            this.m_pluginToolImpl.Login();
        }
        public string GetToken()
        {
            if (this.m_pluginToolImpl == null)
            {
                return string.Empty;
            }
            return this.m_pluginToolImpl.GetToken();
        }
        public string GetUserId()
        {
            if (this.m_pluginToolImpl == null)
            {
                return string.Empty;
            }
            return this.m_pluginToolImpl.GetUserId();
        }
        public void LoginOut()
        {
            if (this.m_pluginToolImpl == null)
            {
                return;
            }
            this.m_pluginToolImpl.LoginOut();
        }
        public void Pay(string strUserAccount, string strGameAreaId)
        {
            if (this.m_pluginToolImpl == null)
            {
                return;
            }
            this.m_pluginToolImpl.Pay(strUserAccount, strGameAreaId);
        }
        public void RegisterAccount()
        {
            if (this.m_pluginToolImpl == null)
            {
                return;
            }
            this.m_pluginToolImpl.RegisterAccount();
        }
        public void ForgetAccount()
        {
            if (this.m_pluginToolImpl == null)
            {
                return;
            }
            this.m_pluginToolImpl.ForgetAccount();
        }
    }
}