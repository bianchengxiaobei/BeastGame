using UnityEngine;
using System.Collections.Generic;
using Utility;
using Client.Common;
using Client.UI.UICommon;
using Client.UI;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SubActionState_Enable 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.29
// 模块描述：战棋技能阶段开始子系统
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋技能阶段开始子系统
/// </summary>
namespace Client.GameMain.OpState.Stage
{
    public class SubActionState_Enable : SubActionStateBase
    {
        public override void OnEnter()
        {
            //取得能使用的技能，然后高亮显示
            List<int> canUseSkill = Singleton<BeastRole>.singleton.GetCanUseSkillOrEquip(EnumSkillType.eSkillType_Skill);
            DlgBase<DlgMain, DlgMainBehaviour>.singleton.HighlightSkills(EnumSkillType.eSkillType_Skill, canUseSkill);
            UIManager.singleton.SetCursor(enumCursorType.eCursorType_Normal);
            //显示选择技能提示消息
        }
        public override void OnLeave()
        {
            //提示消息掩藏
        }
        public override void OnHpChange()
        {

        }
        public override bool OnSelectSkill(EnumSkillType eType, int unSkillId)
        {
            if (unSkillId == 0)
            {
                return false;
            }
            else
            {
                ActionState.Singleton.ChangeState(enumSubActionState.eSubActionState_SkillUse);
                ActionState.Singleton.OnSelectSkill(eType, unSkillId);
                return true;
            }
        }
    }
}