using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgStartBattleBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.26
// 模块描述：战斗开始提示消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战斗开始提示消息
/// </summary>
public class DlgStartBattleBehaviour : DlgBehaviourBase
{
    public IXUIGroup m_Group_UIEffect_StartBattle = null;
    public override void Init()
    {
        base.Init();
        this.m_Group_UIEffect_StartBattle = base.GetUIObject("gp_uieffect") as IXUIGroup;
    }
}
