using UnityEngine;
using System;
using Utility;
using Utility.Export;
using UnityPlugin.Export;
using GameClient.Data;
using Client.Common;
using Client.UI;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PluginToolWrapper
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：平台SDK管理器
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Platform
{
    internal class PluginToolWrapper : Singleton<PluginToolWrapper>
    {
        private IXLog m_log = XLog.GetLog<PluginToolWrapper>();
        /// <summary>
        /// 用户平台类型，有无，百度，空中网（根据所在平台）
        /// </summary>
        public EAuthPlatformType EAuthPlatformType
        {
            get
            {
                EAuthPlatformType result = EAuthPlatformType.AUTH_PLATFORM_NONE;
                switch (PluginTool.Singleton.EPlatformType)
                {
                    case EnumPlatformType.ePlatformType_KZ:
                        result = EAuthPlatformType.AUTH_PLATFORM_KONGZHONG;
                        break;
                    case EnumPlatformType.ePlatformType_Baidu:
                        result = EAuthPlatformType.AUTH_PLATFORM_BAIDU;
                        break;
                }
                return result;
            }
        }
        /// <summary>
        /// 初始化PluginTool
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            Singleton<PlatformConfig>.singleton.Init();
            IPluginToolImplManager pluginToolImplManager = UnityGameEntry.Instance.GetComponent("PluginToolImplManager") as IPluginToolImplManager;
            bool result;
            if (null == pluginToolImplManager)
            {
                this.m_log.Fatal("null == pluginToolManager");
                result = false;
            }
            else
            {
                IPluginTool pluginTool = pluginToolImplManager.GetPluginTool(Singleton<PlatformConfig>.singleton.EPlatformType);
                if (null == pluginTool)
                {
                    this.m_log.Fatal("null == pluginTool");
                    result = false;
                }
                else
                {
                    PluginTool.Singleton.PayUrl = Singleton<PlatformConfig>.singleton.PayUrl;
                    PluginTool.Singleton.RegisterUrl = Singleton<PlatformConfig>.singleton.RegisterUrl;
                    PluginTool.Singleton.ForgetUrl = Singleton<PlatformConfig>.singleton.ForgetUrl;
                    result = PluginTool.Singleton.Init(pluginTool);
                }
            }
            return result;
        }
        public void UnInit()
        {
            PluginTool.Singleton.UnInit();
        }
        public void Update()
        {
            try
            {
                PluginTool.Singleton.Update();
                PluginEvent pluginEvent = null;
                if (PluginTool.Singleton.PopEvent(out pluginEvent))
                {
                    this.m_log.Debug(string.Format("PluginTool.Singleton.PopEvent:{0}", pluginEvent.Type.ToString()));
                    switch (pluginEvent.Type)
                    {
                        case EnumPluginEventType.ePluginEventType_Install_Success:
                            Debug.Log("Install Success!");
                            //预初始化ClientMain,实际上就是为了进入登陆状态
                            Singleton<ClientMain>.singleton.PreInit();
                            //如果是手机平台的话，不用播放视频直接更新
                            if (CommonDefine.IsMobilePlatform)
                            {
                                Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_Update);
                            }
                            else 
                            {
                                //播放开场动画
                                DlgBase<DlgBeginAnimation, DlgBeginAnimationBehaviour>.singleton.SetVisible(true);
                               // DlgBase<DlgTest, DlgTestBehaviour>.singleton.SetVisible(true);
                                //DlgBase<DlgLogin, DlgLoginBehaviour>.singleton.SetVisible(true);
                            }
                            break;
                        case EnumPluginEventType.ePluginEventType_Login_Success:
                            //如果登陆成功，开始登陆
                            Singleton<Client.Logic.Login>.singleton.StartLogin(PluginTool.Singleton.GetUserId(), string.Empty);
                            break;
                        case EnumPluginEventType.ePluginEventType_Login_Fail:
                           // Singleton<Client.Logic.Login>.singleton.OnLoginFailed();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        public void Install()
        {
            PluginTool.Singleton.Install("");
        }
        public void Login()
        {
            PluginTool.Singleton.Login();
        }
        public void LoginOut()
        {
            PluginTool.Singleton.LoginOut();
        }
        public string GetToken()
        {
            return PluginTool.Singleton.GetToken();
        }
        public void Pay(string strUserAccount, string strGameAreaId)
        {
            PluginTool.Singleton.Pay(strUserAccount, strGameAreaId);
        }
        public void RegisterAccount()
        {
            PluginTool.Singleton.RegisterAccount();
        }
        public void ForgetAccount()
        {
            PluginTool.Singleton.ForgetAccount();
        }
    }
    public enum EAuthPlatformType
    {
        AUTH_PLATFORM_NONE,
        AUTH_PLATFORM_KONGZHONG,
        AUTH_PLATFORM_BAIDU
    }
}