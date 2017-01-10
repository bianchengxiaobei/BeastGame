using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_InRoom 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.31
// 模块描述：在匹配房间状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 在匹配房间状态
/// </summary>
public class ClientState_InRoom : ClientState_Base
{
    public override void OnEnter()
    {
        XLog.Log.Debug("Enter ClientState_InRoom");
        base.OnEnter();
        DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.Reset();
        DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.SetVisible(true);
    }
    public override void OnLeave()
    {
        base.OnLeave();
        UIManager.singleton.CloseAllDlg(256u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.ResetAllDlg(256u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.UnLoadAllDlg(256u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
    }
}
