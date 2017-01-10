using UnityEngine;
using System.Collections;
using Utility;
using Client.Common;
using Client.UI.UICommon;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientSta 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.9.18
// 模块描述：选择游戏角色状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 选择游戏角色状态
/// </summary>
public class ClientState_SelectPlayer : ClientState_Base
{
    public override void OnEnter()
    {
        base.OnEnter();
        //选择游戏角色的界面显示
        DlgBase<DlgSelectRole, DlgSelectRoleBehaviour>.singleton.SetVisible(true);
    }
    public override void OnLeave()
    {
        base.OnLeave();
        UIManager.singleton.CloseAllDlg(16u);
        UIManager.singleton.ResetAllDlg(16u);
        UIManager.singleton.UnLoadAllDlg(16u);
    }
}
