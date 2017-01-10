using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_Match 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.25
// 模块描述：匹配状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 匹配状态
/// </summary>
public class ClientState_Match : ClientState_Base
{
    public override void OnEnter()
    {
        base.OnEnter();
        DlgBase<DlgMatch, DlgMatchBehaviour>.singleton.SetVisible(true);
    }
    public override void OnLeave()
    {
        base.OnLeave();
        UIManager.singleton.CloseAllDlg(32u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.ResetAllDlg(32u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.UnLoadAllDlg(32u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.CloseAllDlg(128u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.ResetAllDlg(128u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.UnLoadAllDlg(128u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
    }
}
