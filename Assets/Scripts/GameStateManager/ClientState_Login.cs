using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI;
using Utility;
using GameClient.Audio;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_Login
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：登陆状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 登陆状态
/// </summary>
public class ClientState_Login : ClientState_Base
{
    public override void OnEnter()
    {
        Debug.Log("ClientState_Login.OnEnter()");
        base.OnEnter();
        DlgBase<DlgLogin, DlgBehaviourBase>.singleton.SetVisible(true);
        //DlgBase<DlgSetting, DlgSettingBehaviour>.singleton.SetVisible(true);
        //CWindowHandle.singleton.Correct();
        AudioManager.singleton.PlayMusic("AudioClips/login");
    }
    public override void OnLeave()
    {
        base.OnLeave();
        DlgBase<DlgLogin, DlgLoginBehaviour>.singleton.Reset();
        UIManager.singleton.CloseAllDlg(4u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.ResetAllDlg(4u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
        UIManager.singleton.UnLoadAllDlg(4u, 1u << (int)((byte)Singleton<ClientMain>.singleton.ENextGameState) | 65536u);
    }
}
