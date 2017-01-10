using UnityEngine;
using System.Collections;
using Game;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：OpStateBase
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：操作阶段抽象基类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 操作阶段抽象基类
/// </summary>
public abstract class OpStateBase 
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
    public virtual bool OnHoverPos(CVector3 vecHex)
    {
        return false;
    }
    public virtual bool OnSelectPos(CVector3 vecHex)
    {
        return false;
    }
    public virtual bool OnButtonOkClick()
    {
        return false;
    }
    public virtual void OnHpChange()
    {
 
    }
    public virtual bool OnHoverBeast(long beastId)
    {
        return false;
    }
    public virtual bool OnClickBeast(long beastId)
    {
        return false;
    }
    public virtual bool OnUnSelectBeast(long beastId)
    {
        return false;
    }
    public virtual bool OnClick()
    {
        return false;
    }
    public virtual bool OnButtonFinishClick()
    {
        return false;
    }
    public virtual bool OnClickSkill(EnumSkillType eSkillType,int skillId)
    {
        return false;
    }
    public virtual bool OnSelectSkill(EnumSkillType eSkillType, int skillId)
    {
        return false;
    }
}
