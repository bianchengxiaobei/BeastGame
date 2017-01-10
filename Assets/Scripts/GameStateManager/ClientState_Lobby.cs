using UnityEngine;
using System.Collections;
using Client.UI;
using Client.UI.UICommon;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_Lobby 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.25
// 模块描述：游戏大厅状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏大厅状态
/// </summary>
public class ClientState_Lobby : ClientState_Base
{
    public override void OnEnter()
    {
        base.OnEnter();
        DlgBase<DlgLobby, DlgLobbyBehaviour>.singleton.SetVisible(true);
    }
    public override void OnLeave()
    {
        base.OnLeave();
        /*UIManager.singleton.CloseAllDlg(32u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.ResetAllDlg(32u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.UnLoadAllDlg(32u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        */
    }
}
