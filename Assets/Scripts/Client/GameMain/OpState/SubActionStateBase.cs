using UnityEngine;
using System.Collections;
using Game;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SubActionStateBase 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.29
// 模块描述：战棋战斗技能阶段子系统
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋战斗技能阶段子系统
/// </summary>
public abstract class SubActionStateBase
{
    public virtual void OnEnter()
    {
    }
    public virtual void OnLeave()
    {
    }
    public virtual void OnUpdate()
    {
    }
    public virtual void Refresh()
    {
    }
    public virtual void OnHpChange()
    {
    }
    public virtual void OnTerrainChange()
    {
    }
    public virtual void OnMoneyChange()
    {
    }
    public virtual void OnStatusChange()
    {
    }
    public virtual bool OnClick()
    {
        return false;
    }
    public virtual bool OnRightButtonClick()
    {
        return false;
    }
    public virtual bool OnHoverPos(CVector3 vec3Hex)
    {
        return false;
    }
    public virtual bool OnSelectPos(CVector3 vec3Hex)
    {
        return false;
    }
    public virtual bool OnHoverBeast(long unBeastId)
    {
        return false;
    }
    public virtual bool OnSelectPlayer(long unPlayerId)
    {
        return false;
    }
    public virtual bool OnUnSelectPlayer(long unPlayerId)
    {
        return false;
    }
    public virtual bool OnClickBeast(long unBeastId)
    {
        return false;
    }
    public virtual bool OnSelectSkill(EnumSkillType eType,int unSkillId)
    {
        return false;
    }
    public virtual bool OnSelectEquip(int unEquipId)
    {
        return false;
    }
    public virtual bool OnSelectPlayerSkill(int unSkillId)
    {
        return false;
    }
    public virtual bool OnSelectPlayerEquip(int unEquipId)
    {
        return false;
    }
    public virtual bool OnButtonOkClick()
    {
        return false;
    }
    public virtual bool OnClickSkill(EnumSkillType eSkillType, int skillId)
    {
        return false;
    }
}
