using UnityEngine;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using Utility;
using Utility.Export;
using GameClient.Platform;
using GameClient.Data;
using Client.UI.UICommon;
using Client.UI;
using Client.Common;
using Game;
using GameClient;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Login
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：登陆逻辑处理
//----------------------------------------------------------------*/
#endregion
namespace Client.Logic
{
    public class Login : Singleton<Login>
    {
        #region 字段加属性
        private string m_dlgAnnouncementXml = string.Empty;
        private string m_strUserId = "";
        private string m_strPassword = "";
        private string m_strParnterId = string.Empty;
        private bool m_bLogined = false;
        private WWW m_wwwRequest = null;
        private WWW m_wwwRequestForTrack = null;
        private EAccountAuthType m_eAuthType = EAccountAuthType.ACCOUNT_AUTH_NORMAL;
        private IXLog m_log = XLog.GetLog<Login>();
        public float m_fLogin_Time = 0f;
        /// <summary>
        /// 是否已经登录了
        /// </summary>
        public bool IsLogined
        {
            get { return this.m_bLogined; }
        }
        public string UserId
        {
            get { return this.m_strUserId; }
        }
        public string Password
        {
            get { return this.m_strPassword; }
        }
        /// <summary>
        /// 用户类型，默认为normal
        /// </summary>
        public EAccountAuthType EAuthType
        {
            get { return this.m_eAuthType; }
        }
        public float LoginTime
        {
            get;
            private set;
        }
        #endregion
        #region 构造函数
        public Login()
        {
            this.m_eAuthType = EAccountAuthType.ACCOUNT_AUTH_NORMAL;
            this.m_strParnterId = "";
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 开始登录，先缓冲下载游戏的公告，然后设置登陆按钮不激活
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strPassword"></param>
        public void StartLogin(string strUserId, string strPassword)
        { 
            //UnityGameEntry.Instance.StartCoroutine(this.RequestAnnouncement());//协程下载公告
            DlgBase<DlgLogin, DlgLoginBehaviour>.singleton.EnableButtonEnter(false);//登陆按钮失效，在登陆的时候
            this.m_strUserId = strUserId;
            this.m_strPassword = strPassword;
            //如果Network没有连接上服务器，就开始连接
            if (Singleton<NetworkManager>.singleton.NetState == Game.SocketState.State_Connected)//连接上服务器的话
            {
                string macAddress = this.GetMacAddress();//mac地址
                string token = Singleton<PluginToolWrapper>.singleton.GetToken();//Token令牌
                Singleton<NetworkManager>.singleton.SendLogin(strUserId, strPassword,1);//发送登录消息给服务器
            }
            else 
            {
                Singleton<NetworkManager>.singleton.ConnectToServer();//如果还没有连接服务器，就连接
            }
        }
        /// <summary>
        /// 登陆成功做的处理,如果是创建角色就跳转，设置userOptions里面的数据，比如userid
        /// </summary>
        /// <param name="oPtc"></param>
        public void OnLoginSuccess(CptcG2CNtf_CharacterInfo oPtc)
        {
            if (null == oPtc)
            {
                this.m_log.Error("null == oPtc");

            }
            else 
            {
                if (oPtc.characters.Count == 0)//如果还没有角色，就开始创建创建角色
                {
                    this.m_bLogined = true;//设置已经登陆为ture
                    Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_CreatPlayer);//改变游戏状态为创建角色

                }
                UserOptions.Singleton.UserId = this.m_strUserId;
            }
        }
        /// <summary>
        /// 如果创建角色成功
        /// </summary>
        /// <param name="oPtc"></param>
        public void OnLoginSuccess(CPtcCNtf_EnterLobby oPtc)
        {
            if (null == oPtc)
            {
                this.m_log.Error("null == oPtc(CptcNtf_EnterLobby)");
            }
            else 
            {
                this.m_bLogined = true;
                this.LoginTime = Time.time;
                Singleton<PlayerRole>.singleton.ID = oPtc.m_accountID;
                Debug.Log("PlayerRole.Id=" + Singleton<PlayerRole>.singleton.ID);
                Singleton<PlayerRole>.singleton.RoleAllInfo = oPtc.m_roleAllInfo;
                this.EnterLobby();
                if (this.EAuthType != EAccountAuthType.ACCOUNT_AUTH_INTERNAL)
                {
                    UserOptions.Singleton.UserId = this.m_strUserId;
                    //UserOptions.Singleton.SaveUserConfig();
                }
            }
        }
        /// <summary>
        /// 登陆失败做的处理
        /// </summary>
        public void OnLoginFailed()
        {
            DlgBase<DlgLogin, DlgLoginBehaviour>.singleton.EnableButtonEnter(true);
        }
        /// <summary>
        /// 下载游戏公告
        /// </summary>
        /// <returns></returns>
        public IEnumerator RequestAnnouncement()
        {
            Singleton<PlatformConfig>.singleton.Init();
            string announceUrl = Singleton<PlatformConfig>.singleton.AnnounceUrl;
            WWW www = new WWW(announceUrl);
            yield return www;
            if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
            {
                this.m_dlgAnnouncementXml = www.text;
            }
            else
            {
                string url = "http://www.baidu.com";
                WWW www2 = new WWW(url);
                yield return www;
                if (string.IsNullOrEmpty(www2.error))
                {
                    this.m_dlgAnnouncementXml = www2.text;
                }
                else
                {
                    this.m_log.Error(www2.error);
                }
            }
            yield break;
        }
        /// <summary>
        /// 获取Mac地址,NetworkInterface类
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress()
        {
            string result;
            try
            {
                NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                if (allNetworkInterfaces.Length > 0)
                {
                    result = allNetworkInterfaces[0].GetPhysicalAddress().ToString();
                    return result;
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
            result = "";
            return result;
        }
        /// <summary>
        /// 如果连接成功，就开始登录
        /// </summary>
        public void OnConnectSuccess()
        {
            if (!this.IsLogined)
            {
                this.StartLogin(this.m_strUserId, this.m_strPassword);
            }
        }
        /// <summary>
        /// 连接服务器失败
        /// </summary>
        public void OnConnectFailed()
        {
            string @string = StringConfigMgr.GetString("Login.CanNotConnectServer");
            //警告界面窗口显示该警告

            DlgBase<DlgLogin, DlgLoginBehaviour>.singleton.EnableButtonEnter(true);
            this.LogToServer_NetworkFailed();
        }
        /// <summary>
        /// 进入游戏大厅
        /// </summary>
        public void EnterLobby()
        { 
            //如果下载的公告不为空的话，就初始化
            Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_Lobby);
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 打印连接服务器失败，并且连接上提供的ip
        /// </summary>
        private void LogToServer_NetworkFailed()
        {
            this.m_log.Debug("LogToServer_NetworkFailed");
            this.m_wwwRequest = new WWW("http://www.lybns.com/recordip.php");
        }
        #endregion 
    }
    public enum EAccountAuthType
    {
        ACCOUNT_AUTH_NORMAL,
        ACCOUNT_AUTH_INTERNAL
    }
}