using UnityEngine;
using System.Collections;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UseSkill_None 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.12.19
// 模块描述：初始使用技能的状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 初始使用技能的状态
/// </summary>
public class UseSkill_None : UseSkillBase
{
    public override void OnEnter()
    {
       
    }
    public override void OnLeave()
    {
        
    }
    public override bool OnSelectSkill(EnumSkillType eType, int skillId)
    {
        return false;
    }
}
