using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Client.Skill;
using Game;
using Utility;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UseSkillBase 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.29
// 模块描述：使用技能实例基类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 使用技能实例基类
/// </summary>
public abstract class UseSkillBase 
{
    #region 字段
    protected int m_unSkillId = 0;
    protected EnumSkillType m_eSkillType = EnumSkillType.eSkillType_Skill;
    protected byte m_btSelectedIndex = 0;
    protected bool m_bOutMainUI = false;
    protected bool m_bClickLocked = false;
    #endregion
    #region 属性
    public int SkillId
    {
        get
        {
            return this.m_unSkillId;
        }
        set
        {
            this.m_unSkillId = value;
        }
    }
    public EnumSkillType SkillType
    {
        get
        {
            return this.m_eSkillType;
        }
    }
    #endregion
    #region 构造方法
    #endregion
    #region 子类重写方法
    public virtual void OnEnter()
    {
        this.ShowCastRange();
        UIManager.singleton.IsInOpState = true;
        UIManager.singleton.SetCursor(enumCursorType.eCursorType_Drag);

    }
    public virtual void OnLeave()
    {
        UIManager.singleton.IsInOpState = false;
        this.ClearCastRange();
        UIManager.singleton.ResetCurTouchState();
    }
    protected virtual void ShowCastRange()
    {
        SkillBase skill = SkillGameManager.GetSkillBase(this.m_unSkillId);
        if (skill != null)
        {
            List<CVector3> castRange = skill.GetCastRange(Singleton<BeastRole>.singleton.Id);
            CSceneMgr.singleton.ShowCaseRange(castRange);
        }
    }
    public virtual void OnUpdate()
    {
        if (!this.m_bClickLocked)
        {
            this.UpdateState();
        }
    }
    public virtual void UpdateState()
    {
        float y = Input.mousePosition.y;
        float num = y / Screen.height;
        if (num > 0.27f && !this.m_bOutMainUI)
        {
            SkillBase skill = SkillGameManager.GetSkillBase(this.m_unSkillId);
            if (skill != null)
            {
                EnumErrorCodeCheckUse errorCode = skill.CheckUse(Singleton<BeastRole>.singleton.Id);
                if (errorCode != EnumErrorCodeCheckUse.eCheckErr_Success)
                {
                    this.ShowErrCheckUse(errorCode);
                }
                else
                {
                    this.m_bOutMainUI = true;
                    //当鼠标离开UI做的处理
                }
            }
        }
        if (num < 0.25f && this.m_bOutMainUI)
        {
            this.m_bOutMainUI = false;
            //当鼠标进入UI做的处理
        }
    }
    public virtual bool OnClick()
    {
        if (!this.m_bClickLocked)
        {
            if (this.m_bOutMainUI)
            {
                this.OnLockOperation();
                return true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void OnLockOperation()
    {
        this.m_bClickLocked = true;
    }
    public virtual bool OnButtonOkClick()
    {
        return false;
    }
    public virtual bool OnSelectSkill(EnumSkillType eType,int skillId)
    {
        return true;
    }
    public virtual bool OnClickSkill(int skillId)
    {
        return this.m_unSkillId == skillId && this.OnClick();
    }
    public virtual void OnOutMain()
    {

    }
    public virtual void OnInMain()
    {

    }
    public virtual bool OnHoverBeast(long beastId)
    {
        return false;
    }
    public virtual bool OnSelectBeast(long unTargetBeastId)
    {
        return false;
    }
    public virtual bool OnHoverPos(CVector3 pos)
    {
        return true;
    }
    public virtual bool OnSelectPos(CVector3 pos)
    {
        return false;
    }
    #endregion
    #region 保护方法
    /// <summary>
    /// 不显示技能范围格子
    /// </summary>
    protected void ClearCastRange()
    {
        CSceneMgr.singleton.ClearCastRange();
    }
    #endregion
    #region 公共方法
    /// <summary>
    /// 当使用技能不正确的时候做的处理
    /// </summary>
    public void ShowErrCheckUse(EnumErrorCodeCheckUse errorCode)
    {

    }
	#endregion
}
