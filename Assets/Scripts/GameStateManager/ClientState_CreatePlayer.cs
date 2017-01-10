using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Utility;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_CreatePlayer 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.20
// 模块描述：创建角色状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 创建角色状态
/// </summary>
public class ClientState_CreatePlayer : ClientState_Base
{
    public override void OnEnter()
    {
        base.OnEnter();
        DlgBase<DlgCreateRole, DlgCreateRoleBehaviour>.singleton.SetVisible(true);
        //播放创建角色的音乐
    }
    public override void OnLeave()
    {
        base.OnLeave();
        UIManager.singleton.CloseAllDlg(8u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.ResetAllDlg(8u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.UnLoadAllDlg(8u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
    }
}
