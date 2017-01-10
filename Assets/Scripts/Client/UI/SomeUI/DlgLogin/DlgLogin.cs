using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Utility.Export;
using GameClient;
using GameClient.Platform;
using Utility;
using GameClient.Data;
using UnityPlugin.Export;
using Client.Logic;
using Game;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgLogin
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.22
// 模块描述：登陆UI界面管理器
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class DlgLogin : DlgBase<DlgLogin, DlgLoginBehaviour>
    {
        #region 字段
        private bool m_bEnableButtonLogin = true;
        private float m_fLastTimeSendMsg = 0f;
        private float m_fWaitTimeToClose = 0f;
        private bool m_bNeedToClose = false;
        private IXLog m_log = XLog.GetLog<DlgLogin>();
        private bool m_bIsUseOptionPassword;
        private bool m_bNeedOpenPluginLogin = true;
        private float m_fNeedOpenPluginLoginStartTime;
        #endregion
        #region 属性
        public override string fileName
        {
            get
            {
                return "DlgLogin";
            }
        }
        public override int layer
        {
            get
            {
                return -5;
            }
        }
        public override uint Type
        {
            get
            {
                return 4u;
            }
        }
        public override EnumDlgCamera ShowType
        {
            get
            {
                return EnumDlgCamera.Top;
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override void Init()
        {
            base.Init();
            this.m_bIsUseOptionPassword = UserOptions.Singleton.IsRememberAccount;
            if (UserOptions.Singleton.IsRememberAccount)
            {
                base.uiBehaviour.m_Input_ID.SetText(UserOptions.Singleton.UserId);
                if (!string.IsNullOrEmpty(UserOptions.Singleton.UserId))
                {
                    UIManager.singleton.SetFocus(base.uiBehaviour.m_Input_PW);
                }
            }
            else
            {
                base.uiBehaviour.m_Input_ID.SetText("");
                base.uiBehaviour.m_Input_PW.SetText("");
                UIManager.singleton.SetFocus(base.uiBehaviour.m_Input_ID);
                base.uiBehaviour.m_Checkbox_Remember.bChecked = UserOptions.Singleton.IsRememberAccount;
                base.Refresh();
            }
        }
        /// <summary>
        /// 重置登陆界面（实则吧输入密码清空）
        /// </summary>
        public override void Reset()
        {
            if (base.Prepared)
            {
                base.uiBehaviour.m_Input_PW.SetText("");
            }
        }
        public override void RegisterEvent()
        {
            base.uiBehaviour.m_Button_Enter.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnButtonEnterClick));
            //base.uiBehaviour.m_PopupList_Server.RegisterPopupListSelectEventHandler(new PopupListSelectEventHanler(this.OnPopupListSelect));
            base.uiBehaviour.m_Checkbox_Remember.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnCheckboxRememberCheck));
            base.uiBehaviour.m_Label_Register.RegisterClickEventHandler(new ClickEventHandler(this.OnRegisterClick));
            base.uiBehaviour.m_Label_Forget.RegisterClickEventHandler(new ClickEventHandler(this.OnForgetClick));
        }
        public override void SetVisible(bool bIsVisible)
        {
            if (bIsVisible)
            {
                this.m_bNeedToClose = false;
            }
            base.SetVisible(bIsVisible);
        }
        protected override void OnShow()
        {
            base.OnShow();
            this.EnableButtonEnter(true);
            if (Singleton<PluginToolWrapper>.singleton.EAuthPlatformType == EAuthPlatformType.AUTH_PLATFORM_KONGZHONG)
            {
                //base.uiBehaviour.m_Group_Input.SetVisible(true);
            }
            else 
            {
                //base.uiBehaviour.m_Group_Input.SetVisible(false);
            }
            this.m_fNeedOpenPluginLoginStartTime = Time.time;
            this.m_bNeedOpenPluginLogin = true;
        }
        /// <summary>
        /// 是否启用登陆按钮，刷新
        /// </summary>
        /// <param name="bEnable"></param>
        public void EnableButtonEnter(bool bEnable)
        {
            this.m_bEnableButtonLogin = bEnable;
            base.Refresh();
        }
        public void Close()
        {
            if (base.IsVisible())
            {
                this.m_bNeedToClose = true;
            }
        }
        /// <summary>
        /// 这里主要是设置登陆按钮的可用性和显示版本信息
        /// </summary>
        protected override void OnRefresh()
        {
            base.OnRefresh();
            base.uiBehaviour.m_Button_Enter.SetEnable(this.m_bEnableButtonLogin);
            base.uiBehaviour.m_Label_Ver.SetText("版本0.0.1");
        }

        public override void Update()
        {
            base.Update();
            if (this.m_bNeedToClose)
            {
                if (Time.time > this.m_fWaitTimeToClose)
                {
                    this.SetVisible(false);
                    base.UnLoad();
                    this.m_bNeedToClose = false;
                }
            }
            //如果可见
            if (base.IsVisible())
            {
                if (this.m_bNeedOpenPluginLogin && UnityGameEntry.Instance.IsApplicationFocus)
                {
                    if (Time.time - this.m_fNeedOpenPluginLoginStartTime > 0.5f)
                    {
                        this.m_bNeedOpenPluginLogin = true;
                        Singleton<PluginToolWrapper>.singleton.Login();
                    }
                }
            }
        }
        #endregion
        #region 私有方法
        private bool OnButtonEnterClick(IXUIButton uiButton)
        {
            Singleton<CWindowHandle>.singleton.Correct();
            bool result;
            if (!this.m_bEnableButtonLogin)
            {
                result = false;
            }
            else
            {
                if (Singleton<PlatformConfig>.singleton.EPlatformType == EnumPlatformType.ePlatformType_KZ)
                {
                    string account = base.uiBehaviour.m_Input_ID.GetText();//用户名
                    string password = CEncrypt.Encrypt(account, base.uiBehaviour.m_Input_PW.GetText());//密码加密
                    if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))//如果密码或者用户名为空，就消息提示为空
                    {
                       //DlgBase<DlgMessageBox, DlgMessageBoxBehaviour>.singleton.Show(StringConfigMgr.GetString("DlgSet.OnButtonEnterClickContent"), StringConfigMgr.GetString("DlgLogin.OnButtonEnterClickTitle"), null);
                        result = false;
                        return result;
                    }
                    XLog.Log.Debug("strUserId: " + account);
                    //开始登陆
                    Singleton<Login>.singleton.StartLogin(account, password);
                    this.m_fLastTimeSendMsg = Time.time;
                }
                else
                {
                    this.m_bNeedOpenPluginLogin = false;
                    Singleton<PluginToolWrapper>.singleton.Login();
                }
                result = true;
            }
            return result;
        }
        private bool OnCheckboxRememberCheck(IXUICheckBox uiCheckBox)
        {
            bool result;
            if (null == uiCheckBox)
            {
                result = true;
            }
            else
            {
                UserOptions.Singleton.IsRememberAccount = uiCheckBox.bChecked;
                result = true;
            }
            return result;
        }
        private bool OnForgetClick(IXUIObject uiObject)
        {
            Singleton<PluginToolWrapper>.singleton.ForgetAccount();
            return true;
        }
        private bool OnRegisterClick(IXUIObject uiObject)
        {
            Singleton<PluginToolWrapper>.singleton.RegisterAccount();
            return true;
        }
        #endregion
    }
}
