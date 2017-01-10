using UnityEngine;
using System.Collections.Generic;
using Client.UI.UICommon;
using Client.Common;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SubActionState_Disable 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.29
// 模块描述：战棋技能阶段结束子系统
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋技能阶段结束子系统
/// </summary>
public class SubActionState_Disable : SubActionStateBase
{
    public override void OnEnter()
    {
        DlgBase<DlgMain, DlgMainBehaviour>.singleton.HighlightSkills(EnumSkillType.eSkillType_Skill, false);
        UIManager.singleton.ResetCurTouchState();
    }
    public override void OnLeave()
    {
        
    }
}
