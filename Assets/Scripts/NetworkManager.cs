using UnityEngine;
using System;
using System.Collections;
using Utility;
using Game;
using GameClient.Platform;
using Utility.Export;
using UnityAssetEx.Export;
using Client.Logic;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：NetworkManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：网络通信管理器
//----------------------------------------------------------------*/
#endregion
public class NetworkManager : Singleton<NetworkManager>
{
    #region 字段和属性
    private WWW m_wwwRequest = null;
    private float m_fLastTimeForHeartBeat = 0f;
    public static string serverURL = "";
    public static string setverIP = "127.0.0.1";
    public static int serverPort = 8080;
    public CNetProcessor m_netProcessor = null;
    public CNetwork m_network = null;
    private IXLog m_log = XLog.GetLog<NetworkManager>();
    /// <summary>
    /// 网络状态
    /// </summary>
    public SocketState NetState 
    {
        get 
        {
            SocketState result;
            if (this.m_network != null)
            {
                result = this.m_network.GetState();
            }
            else 
            {
                result = SocketState.State_Closed;
            }
            return result;
        }
    }
    #endregion
    #region 构造方法
    public NetworkManager()
    {
        NetworkManager.serverURL = "http://127.0.0.1/BeastGame/ServerUrl.html";
        NetworkManager.setverIP = "";
        NetworkManager.serverPort = 8000;
    }
    #endregion 

    #region 公有方法
    /// <summary>
    /// 初始化network
    /// </summary>
    public void Init()
    {
        this.m_log.Debug("Init()");
        CRegister.RegistProtocol();//注册协议
        CNetObserver cNetObserver = new CNetObserver();
        this.m_netProcessor = new CNetProcessor();//消息处理器
        this.m_netProcessor.Observer = cNetObserver;
        cNetObserver.oProc = this.m_netProcessor;
        this.m_network = new CNetwork();
        CPacketBreaker oBreaker = new CPacketBreaker();//分包管理器
        string fullPath = ResourceManager.GetFullPath("", false);
        if (!this.m_network.Init(this.m_netProcessor, oBreaker, 65536u, 65536u, fullPath, true))
        {
            this.m_log.Fatal("Net Init Error");
        }
        else 
        {
            this.m_log.Debug("Net Init Success");
            this.m_netProcessor.Network = this.m_network;
        }
        
    }
    public void FixedUpdate()
    {
        try
        {
            if (this.m_netProcessor != null && null != this.m_netProcessor.Network)
            {
                this.m_netProcessor.Network.ProcessMsg();
            }
        }
        catch (Exception e)
        {
            this.m_log.Fatal(e.ToString());
        }
        float time = Time.time;
        if (time - this.m_fLastTimeForHeartBeat > 5f && this.NetState == SocketState.State_Connected)//每隔5秒发送心跳包
        {
            try
            {
                if (null != this.m_netProcessor)
                {
                    //this.m_netProcessor.SendKeepAlivePacket();//发送心跳包
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
            this.m_fLastTimeForHeartBeat = time;
        }
    }
    /// <summary>
    /// 连接服务器，先访问url获取ip和端口，然后连接
    /// </summary>
    public void ConnectToServer()
    {
        if (string.IsNullOrEmpty(NetworkManager.serverURL))
        {
            this.m_log.Error("serverURL isNullOrEmpty");
        }
        UnityGameEntry.Instance.StartCoroutine(this.ConnectToURL());
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="ptc">消息协议</param>
    public void SendMsg(CProtocol ptc)
    {
        try
        {
            if (this.m_netProcessor != null)
            {
                this.m_netProcessor.Send(ptc);
            }
        }
        catch (Exception e)
        {
            this.m_log.Fatal(e.ToString());
        }
    }
    /// <summary>
    /// 发送登录消息
    /// </summary>
    /// <param name="strUserName">用户名</param>
    /// <param name="strPassword">密码</param>
    public void SendLogin(string strUserName, string strPassword, int serverId)
    {
        this.SendMsg(new CptcCReq_Login 
        {
            m_strAccountName = strUserName,
            m_strPassword = strPassword,
            m_nServerId = serverId
        });
    }
    /// <summary>
    /// 发送创建角色消息
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="roleType">角色id</param>
    /// <param name="sex">性别</param>
    public void SendCreateRole(string roleName,string  icon,byte sex,int roleIndex)
    {
        this.SendMsg(new CptcCReq_CreateRole
        {
            m_roleName = roleName,
            m_roleIcon = icon,
            m_sex = sex,
            m_roleIndex = roleIndex
        });
    }
    #region 发送选择人物角色消息
    /// <summary>
    /// 发送选择人物角色消息
    /// </summary>
    /// <param name="roleId"></param>
    public void SendSelectRole(long roleId)
    {
        this.SendMsg(new CptcCReq_SelectRole
        {
            m_lRoleId = roleId
        });
    }
    #endregion
    /// <summary>
    /// 发送开始匹配的请求消息
    /// </summary>
    /// <param name="uMapId">地图id</param>
    /// <param name="eMatchType">匹配类型</param>
    /// <param name="eAIDifficulty">人机难度</param>
    public void SendMatchReq(uint uMapId,EGameType eMatchType,EAIDifficulty eAIDifficulty)
    {
        this.SendMsg(new CptcCReq_AutoMatch
        {
            m_uMapID = (int)uMapId,
            m_btMatchType = (byte)eMatchType,
            m_btAIDifficulty = (byte)eAIDifficulty
        });
        XLog.Log.Debug("------CPtcC2MReq_AutoMatch-------");
    }
    /// <summary>
    /// 发送取消匹配消息
    /// </summary>
    public void SendMatchCancel()
    {
        CPtcC2MReq_CancelMatch sendInstance = CPtcC2MReq_CancelMatch.GetSendInstance();
        this.SendMsg(sendInstance);
    }
    /// <summary>
    /// 发送试图选择该神兽
    /// </summary>
    /// <param name="beastId"></param>
    /// <param name="nBeastTypeId"></param>
    public void SendSelectBeast(long beastId, int nBeastTypeId)
    {
        CptcC2MReq_TrySelectBeast sendInstance = CptcC2MReq_TrySelectBeast.GetInstance();
        sendInstance.m_dwBeastId = beastId;
        sendInstance.m_dwBeastTypeId = nBeastTypeId;
        this.SendMsg(sendInstance);
        this.m_log.Debug(string.Format("SendTrySelectBeast:BeastId={0},BeastTypeId={1}", beastId, nBeastTypeId));
    }
    /// <summary>
    /// 发送确认选择该神兽
    /// </summary>
    /// <param name="beastId"></param>
    public void SendConfirmBeast(long beastId)
    {
        CptcC2MReq_SelectBeast sendIntance = CptcC2MReq_SelectBeast.GetInstance();
        sendIntance.m_dwBeastId = beastId;
        this.SendMsg(sendIntance);
    }
    /// <summary>
    /// 发送选择神兽出生点的请求消息
    /// </summary>
    /// <param name="vec3HexPos">选择的出生点格子坐标</param>
    public void SendInitPos(CVector3 vec3HexPos)
    {
        CPtcCReq_AddRoleToScene sendInstance = CPtcCReq_AddRoleToScene.GetSendInstance();
        sendInstance.m_unRoleId = Singleton<BeastRole>.singleton.Id;
        sendInstance.m_oInitPos = vec3HexPos;
        this.SendMsg(sendInstance);
        this.m_log.Debug(string.Format("SendInitPos: unBeastId={0}", sendInstance.m_unRoleId));
    }
    /// <summary>
    /// 发送神兽请求进入该战斗阶段
    /// </summary>
    /// <param name="stage"></param>
    public void SendBeastEnterStage(int stage)
    {
        CptcC2MReq_BeastEnterStage msg = new CptcC2MReq_BeastEnterStage();
        msg.beastId = Singleton<BeastRole>.singleton.Id;
        msg.stage = stage;
        this.SendMsg(msg);
        this.m_log.Debug("神兽请求进入战斗阶段："+((EClientRoleStage)stage).ToString());
    }
    /// <summary>
    /// 发送神兽请求移动到某个位置
    /// </summary>
    /// <param name="vecTargetPos"></param>
    public void SendBeastMoveReq(CVector3 vecTargetPos)
    {
        CPtcC2MReq_Move msg = new CPtcC2MReq_Move();
        msg.beastId = Singleton<BeastRole>.singleton.Id;
        msg.desPos = vecTargetPos;
        this.SendMsg(msg);
        this.m_log.Debug("SendMoveReq:" + vecTargetPos.ToString());
    }
    /// <summary>
    /// 发送给服务器结束本战斗阶段
    /// </summary>
    /// <param name="stage"></param>
    public void SendFinishRoleStage(EClientRoleStage stage)
    {
        CptcC2MReq_EndRoleStage msg = new CptcC2MReq_EndRoleStage();
        msg.beastId = Singleton<BeastRole>.singleton.Id;
        msg.stage = (int)Singleton<BeastRole>.singleton.eRoleStage;
        this.SendMsg(msg);
        this.m_log.Debug("SendFinishedRoleStage:" + stage.ToString());
    }
    /// <summary>
    /// 发送给服务器释放技能消息
    /// </summary>
    /// <param name="msg"></param>
    public void SendUseSkill(CPtcC2MReq_CastSkill msg)
    {
        this.SendMsg(msg);
    }
    #endregion
    #region 私有方法
    /// <summary>
    /// 协程连接url获取ip和端口号，成功之后开始连接服务器，格式为127.0.0.1：8080
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConnectToURL()
    {
        this.m_log.Debug("ConnectToURL:" + NetworkManager.serverURL);
        this.m_wwwRequest = new WWW(NetworkManager.serverURL);
        bool flag = false;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);//每隔0.5s钟，去检测是否获取到ip地址
            if (this.m_wwwRequest.isDone)
            {
                if (string.IsNullOrEmpty(this.m_wwwRequest.error))
                {
                    if (!string.IsNullOrEmpty(this.m_wwwRequest.text))
                    {
                        string[] array = this.m_wwwRequest.text.Split(':');
                        if (array.Length == 2)
                        {
                            NetworkManager.setverIP = array[0];//获取IP
                            NetworkManager.serverPort = Convert.ToInt32(array[1]);//获取端口号
                            flag = true;
                            RealConnectToServer();
                        }
                        else 
                        {
                            this.m_log.Error("m_wwwRequest.text=:" + this.m_wwwRequest.text);
                        }
                    }
                }
                else 
                {
                    this.m_log.Error(this.m_wwwRequest.error);
                }
                break;
            } 
        }
        if (this.m_wwwRequest != null)
        {
            this.m_wwwRequest.Dispose();//释放www资源
            this.m_wwwRequest = null;
        }
        yield break;
    }
    /// <summary>
    /// 真正连接服务器
    /// </summary>
    private void RealConnectToServer()
    {
        this.m_log.Info(string.Format("ConnectToServer:{0}:{1}", NetworkManager.setverIP, NetworkManager.serverPort));
        if (!this.m_netProcessor.Network.Connect(NetworkManager.setverIP, NetworkManager.serverPort))
        {
            //如果没有重新连接
            if (!Singleton<Login>.singleton.IsLogined)
            {
                Singleton<Login>.singleton.OnConnectFailed();
            }
            else
            {
                
            }
        }
        else 
        {
            this.m_log.Debug("Connecting......");
        }
    }
    public void SendEnterMainScene()
    {
        CptcCReq_EnterScene sendInstance = CptcCReq_EnterScene.GetInstance();
        this.SendMsg(sendInstance);
        this.m_log.Debug("SendEnterMainScene");
    }
    #endregion

}
