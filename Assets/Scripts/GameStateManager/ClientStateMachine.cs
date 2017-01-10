using UnityEngine;
using System;
using System.Collections.Generic;
using Client.Common;
using Client.UI;
using UnityAssetEx.Export;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientStateMachine
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：游戏状态管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏状态管理器
/// </summary>
public class ClientStateMachine 
{
	#region 字段
    private Dictionary<EnumGameState, ClientState_Base> m_dicClientState = new Dictionary<EnumGameState, ClientState_Base>();
    private ClientState_Base m_currentClientState = null;
    /// <summary>
    /// 是否场景准备完成
    /// </summary>
    private bool m_bScenePrepared = false;
    /// <summary>
    /// 是否资源加载完成
    /// </summary>
    private bool m_bResourceLoaded = false;
    private ELoadingStyle m_currentLoadingStyle = ELoadingStyle.None;
    private Action m_callBackWhenChangeFinished = null;
	#endregion
	#region 属性
    /// <summary>
    /// 现在的状态类型
    /// </summary>
    public EnumGameState CurrentGameState
    {
        get;
        private set;
    }
    /// <summary>
    /// 下一个状态类型
    /// </summary>
    public EnumGameState NextGameState
    {
        get;
        private set;
    }
    /// <summary>
    /// 是否正在改变状态
    /// </summary>
    public bool IsChangingState
    {
        get;
        private set;
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    public void Init()
    {
        this.m_dicClientState.Add(EnumGameState.eState_Login, new ClientState_Login());
        this.m_dicClientState.Add(EnumGameState.eState_Update, new ClientState_Update());
        this.m_dicClientState.Add(EnumGameState.eState_CreatPlayer, new ClientState_CreatePlayer());
        this.m_dicClientState.Add(EnumGameState.eState_SelectPlayer, new ClientState_SelectPlayer());
        this.m_dicClientState.Add(EnumGameState.eState_Lobby, new ClientState_Lobby());
        this.m_dicClientState.Add(EnumGameState.eState_Match, new ClientState_Match());
        this.m_dicClientState.Add(EnumGameState.eState_InRoom, new ClientState_InRoom());
        this.m_dicClientState.Add(EnumGameState.eState_GameMain, new ClientState_GameMain());
        this.CurrentGameState = EnumGameState.eState_Max;
        this.IsChangingState = false;
    }
    /// <summary>
    /// 注册状态改变后的回调函数
    /// </summary>
    /// <param name="callBackOnChangeFinished"></param>
    public void RegisterCallBackOnChangedFinished(Action callBackOnChangeFinished)
    {
        this.m_callBackWhenChangeFinished = (Action)Delegate.Combine(this.m_callBackWhenChangeFinished, callBackOnChangeFinished);
    }
    public void ConvertToState(EnumGameState nextGameState, ELoadingStyle loadingStyle, Action callback)
    {
        if (nextGameState != this.CurrentGameState)
        {
            this.NextGameState = nextGameState;
            this.m_currentLoadingStyle = loadingStyle;
            this.m_callBackWhenChangeFinished = (Action)Delegate.Combine(this.m_callBackWhenChangeFinished, callback);
            if (ELoadingStyle.DefaultRule == this.m_currentLoadingStyle)
            {
                if (EnumGameState.eState_GameMain == this.CurrentGameState)
                {
                    this.m_currentLoadingStyle = ELoadingStyle.LoadingNormal;
                }
                else 
                {
                    this.m_currentLoadingStyle = ELoadingStyle.LoaidngWait;
                }
            }
            //加载界面显示
            this.SetLoadingVisible(this.m_currentLoadingStyle, true);
        }
        this.IsChangingState = true;
        if (this.CurrentGameState != EnumGameState.eState_Max)
        {
            //如果不是中间状态的话，就离开该状态
            this.m_currentClientState.OnLeave();
            //设置资源全部卸载完成之后回调
            ResourceManager.singleton.SetAllUnLoadFinishedEventHandler(delegate(bool o)
            {
                this.DoChangeToNewState();
            });
        }
        else 
        {
            this.DoChangeToNewState();
        }
    }
    #endregion
	#region 私有方法
    /// <summary>
    /// 设置不同加载类型的加载界面是否显示
    /// </summary>
    /// <param name="dlgType"></param>
    /// <param name="bVisible"></param>
    private void SetLoadingVisible(ELoadingStyle dlgType,bool bVisible)
    {
        switch (dlgType)
        {
            case ELoadingStyle.LoaidngWait:
                break;
            case ELoadingStyle.LoadingNormal:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 资源全部加载完成之后回调
    /// </summary>
    private void DoChangeToNewState()
    {
        //现在的状态改变
        this.CurrentGameState = this.NextGameState;
        //吧下个状态变成中间状态
        this.NextGameState = EnumGameState.eState_Max;
        this.m_bResourceLoaded = false;
        this.m_bScenePrepared = true;
        if (this.CurrentGameState == EnumGameState.eState_GameMain)
        {
            this.m_bScenePrepared = false;
            CSceneMgr.singleton.OnScenePreparedAction = (Action)Delegate.Combine(CSceneMgr.singleton.OnScenePreparedAction,(Action)delegate
            {
                this.m_bScenePrepared = true;
                this.OnPartLoaded();
            });
        }
        //进入到该状态
        this.m_currentClientState = this.m_dicClientState[this.CurrentGameState];
        this.m_currentClientState.OnEnter();
        ResourceManager.singleton.SetAllLoadFinishedEventHandler(delegate(bool o)
        {
            this.m_bResourceLoaded = true;
            this.OnPartLoaded();
        });
    }
    /// <summary>
    /// 状态改变完成的最后处理
    /// </summary>
    private void OnPartLoaded()
    {
        //如果场景，资源都加载完成
        if (this.m_bResourceLoaded && this.m_bScenePrepared)
        {
            //设置改变状态为false，因为状态已经改变完成
            this.IsChangingState = false;
            //不显示加载界面
            this.SetLoadingVisible(this.m_currentLoadingStyle, false);
            //执行所有界面完成改变状态的函数
            UIManager.singleton.OnFinishChangeState();
            //执行改变状态完成之后的回调函数
            if (null != this.m_callBackWhenChangeFinished)
            {
                Action callBackWhenChangeFinished = this.m_callBackWhenChangeFinished;
                this.m_callBackWhenChangeFinished = null;
                callBackWhenChangeFinished();
            }
        }
    }
    #endregion
}
