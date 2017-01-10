using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Data;
using Client.Common;
using Utility.Export;
using Client.UI.UICommon;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCNtf_EnterScene
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：服务器发送给客户端进入游戏场景
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器发送给客户端进入游戏场景1015
/// </summary>
public class CPtcCNtf_EnterScene : CProtocol
{
	#region 字段
    private IXLog m_log = XLog.GetLog<CPtcCNtf_EnterScene>();
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CPtcCNtf_EnterScene() : base(1015)
    {
        
    }
	#endregion
	#region 公有方法
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        this.m_log.Debug("CptcCNtf_EnterScene");
        Singleton<RoomManager>.singleton.PlayerRoleData.IsLoadFinish = false;
        Singleton<RoomManager>.singleton.EnterGameMain();
        DlgBase<DlgLoading, DlgLoadingBehaviour>.singleton.SetVisible(true);
        Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_GameMain,ELoadingStyle.None, new Action(DlgBase<DlgLoading, DlgLoadingBehaviour>.singleton.OnAllPrepared));
    }
	#endregion
	#region 私有方法
	#endregion
}
