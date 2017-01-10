using UnityEngine;
using System;
using System.Collections.Generic;
using Utility;
using GameClient.Platform;
using Client;
using Client.Common;
using Client.UI.UICommon;
using Client.UI;
using GameClient.UI;
using GameClient.Data;
using Utility.Export;
using UnityAssetEx.Export;
using Client.Logic;
using Game;
using GameClient.Audio;
using Client.GameMain.OpState;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientMain
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：游戏主管理器
//----------------------------------------------------------------*/
#endregion
public class ClientMain : Singleton<ClientMain>
{
    #region 字段
    private Queue<StateChangeArgs> m_GameStateQueue = new Queue<StateChangeArgs>();
    private ClientStateMachine m_ClientStateMachine = new ClientStateMachine();
    private bool m_bHasInited = false;
    private IXLog m_log = XLog.GetLog<ClientMain>();
    #endregion
    #region 属性
    /// <summary>
    /// 游戏现在的状态
    /// </summary>
    public EnumGameState EGameState
    {
        get
        {
            return this.m_ClientStateMachine.CurrentGameState;
        }
    }
    /// <summary>
    /// 游戏下一个状态
    /// </summary>
    public EnumGameState ENextGameState
    {
        get
        {
            return this.m_ClientStateMachine.NextGameState;
        }
    }
    /// <summary>
    /// 是否正在改变状态
    /// </summary>
    public bool IsChangingState
    {
        get
        {
            return this.m_ClientStateMachine.IsChangingState;
        }
    }
    public CScene scene
    {
        get
        {
            return CSceneMgr.singleton.CurScene;
        }
    }
    #endregion
    #region 公有方法
    public void Init()
    {
        this.Init(false);
    }
    public void Init(bool bForceReInit)
    {
        ResourceManager.singleton.SetAllLoadFinishedEventHandler(new Action<bool>(this.OnBeforeStartGameResourceLoadFinished));//当资源全部加载完成之后回调，进入登陆状态
        if (bForceReInit)
        {
            this.PreInit();
        }
        else
        {
            if (this.m_bHasInited)
            {
                return;
            }
        }
        this.m_bHasInited = true;
        Singleton<ScreenManager>.singleton.Init();//设置fps=60帧
        Singleton<PublishConfig>.singleton.Init();
        Singleton<WordFilter>.singleton.Init();
        GameConfig.singleton.Init();
        SkillGameManager.Init();
        GameConfig.singleton.DoGlobalConfig();//设置游戏的运行速度
        Singleton<ConfigCS>.singleton.Init();//电脑配置信息
        Singleton<UITipConfigMgr>.singleton.Init();
        Singleton<NetworkManager>.singleton.Init();
        Singleton<AudioManager>.singleton.Init();
    }
    public void PreInit()
    {
        this.m_log.Debug("ClientMain.PreInit()");
        ResourceManager.singleton.Init(UnityGameEntry.Instance.GetComponent("GameResourceManager") as IResourceManager);
    }
    public void Awake()
    {
        XLog.Init();
        this.m_log.Debug("ClientMain.Awake()");
        //LuaWrapper.singleton.Awake();
        //Singleton<ScreenManager>.singleton.Awake();//屏幕分辨率类
        Singleton<CursorManager>.singleton.Awake();//鼠标设置类
        Singleton<CWindowHandle>.singleton.Init();//窗口化设置类
        this.m_log.Debug("ClientMain.Awake()结束");
    }
    public void Start()
    {
        this.m_log.Debug("ClientMain.Start()");
        this.m_ClientStateMachine.Init();
        if (!Singleton<PluginToolWrapper>.singleton.Init())
        {
            this.m_log.Error("PluginTool.Singleton.Init() == false");
        }
        Singleton<PluginToolWrapper>.singleton.Install();
    }
    public void FixedUpdate()
    {
        Singleton<NetworkManager>.singleton.FixedUpdate();
        UIManager.singleton.FixedUpdate();
    }
    public void Update()
    {
        Singleton<PluginToolWrapper>.singleton.Update();
        Singleton<AudioManager>.singleton.Update();
        ResourceManager.singleton.Update();
        Singleton<OpStateManager>.singleton.Update();
        UIManager.singleton.Update();
        Singleton<CWindowHandle>.singleton.Update();
    }
    public void ChangeGameState(EnumGameState eGameState)
    {
        this.ChangeGameState(eGameState, ELoadingStyle.DefaultRule);
    }
    public void ChangeGameState(EnumGameState eGameState, ELoadingStyle loadingStyle)
    {
        this.ChangeGameState(eGameState, loadingStyle, null);
    }
    /// <summary>
    /// 改变游戏状态，进行改变的处理
    /// </summary>
    /// <param name="eGameState"></param>
    /// <param name="loadingStyle"></param>
    /// <param name="callBackWhenChangeFinished"></param>
    public void ChangeGameState(EnumGameState eGameState, ELoadingStyle loadingStyle, Action callBackWhenChangeFinished)
    {
        if (this.IsChangingState)
        {
            StateChangeArgs item = new StateChangeArgs
            {
                GameState = eGameState,
                LoadingStyle = loadingStyle,
                CallBack = callBackWhenChangeFinished
            };
            this.m_GameStateQueue.Enqueue(item);
        }
        else
        {
            this.m_ClientStateMachine.ConvertToState(eGameState, loadingStyle, callBackWhenChangeFinished);
            this.RegisterCallBackOnChangedFinished(new Action(this.ChangeGameStateQueue));
        }
    }
    /// <summary>
    /// 状态改变完成之后回调，执行ChangeGameState
    /// </summary>
    public void ChangeGameStateQueue()
    {
        if (0 != this.m_GameStateQueue.Count)
        {
            StateChangeArgs stateChangeArgs = this.m_GameStateQueue.Dequeue();
            this.ChangeGameState(stateChangeArgs.GameState, stateChangeArgs.LoadingStyle, stateChangeArgs.CallBack);
        }
    }
    /// <summary>
    /// 重新进入游戏，也就是重新进入主界面
    /// </summary>
    public void ReEnterGame()
    {
        this.ChangeGameState(EnumGameState.eState_GameMain);
    }
    public void Tick()
    {
        StoryManager.singleton.Tick();
    }
    #endregion
    #region 私有方法
    /// <summary>
    /// 进入登陆状态
    /// </summary>
    /// <param name="bSuccess"></param>
    private void OnBeforeStartGameResourceLoadFinished(bool bSuccess)
    {
        Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_Login);
        //Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_CreatPlayer);
    }
    public void RegisterCallBackOnChangedFinished(Action callBackWhenChangeFinished)
    {
        this.m_ClientStateMachine.RegisterCallBackOnChangedFinished(callBackWhenChangeFinished);
    }
    #endregion
}

