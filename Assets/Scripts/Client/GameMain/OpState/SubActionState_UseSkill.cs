using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Utility.Export;
using Client.Skill;
using Client.GameMain.OpState.Stage;
using Client.UI;
using Utility;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SubActionState_UseSkill 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.29
// 模块描述：战棋战斗阶段子系统之使用技能
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋战斗阶段子系统之使用技能
/// </summary>
public class SubActionState_UseSkill : SubActionStateBase
{
    #region 字段
    private static SubActionState_UseSkill m_sIntance = new SubActionState_UseSkill();
    private EnumSkillType m_eCurSkillType = EnumSkillType.eSkillType_Skill;
    private int m_unCurSkillTypeId = 0;
    private UseSkillBase m_curState = null;
    private Dictionary<EnumSkillType, Dictionary<int, UseSkillBase>> m_dicSkillState = new Dictionary<EnumSkillType, Dictionary<int, UseSkillBase>>();
    private IXLog m_log = XLog.GetLog<SubActionState_UseSkill>();
    #endregion
    #region 构造函数
    public SubActionState_UseSkill()
    {
        //这里注册所有的UseSkillBase
        this.Register(new UseSkill_None());
        this.Register(new UseSkill_NormalAttack());
    }
    public static SubActionState_UseSkill Instance
    {
        get
        {
            return SubActionState_UseSkill.m_sIntance;
        }
    }
    #endregion
    #region 重写方法
    public override void OnEnter()
    {
        this.ChangeSkill(EnumSkillType.eSkillType_Skill, 0);
    }
    public override void OnLeave()
    {
        UIManager.singleton.ResetCurTouchState();
        this.ChangeSkill(EnumSkillType.eSkillType_Skill, 0);
    }
    public override void OnUpdate()
    {
        this.m_curState.OnUpdate();
    }
    public override bool OnClick()
    {
        return this.m_curState.OnClick();
    }
    public override bool OnSelectSkill(EnumSkillType eType, int unSkillId)
    {
        Debug.Log("Use Skill Select Skill");
        if (unSkillId == -1)
        {
            ActionState.Singleton.ChangeState(enumSubActionState.eSubActionState_Enable);
            return true;
        }
        if (this.m_curState.OnSelectSkill(eType, unSkillId))
        {
            return true;
        }
        if (this.ChangeSkill(eType, unSkillId))
        {
            SkillBase skill = null;
            EnumErrorCodeCheckUse errorCode = skill.CheckUse(Singleton<BeastRole>.singleton.Id);
            if (errorCode == EnumErrorCodeCheckUse.eCheckErr_Success)
            {
                this.m_curState.OnLockOperation();
            }
            else
            {
                this.m_curState.ShowErrCheckUse(errorCode);
                ActionState.Singleton.ChangeState(enumSubActionState.eSubActionState_Enable);
            }
        }
        return true;
    }
    public override bool OnClickSkill(EnumSkillType eSkillType, int skillId)
    {
        return this.m_curState.OnClickSkill(skillId);
    }
    public override bool OnHoverBeast(long unBeastId)
    {
        return this.m_curState.OnHoverBeast(unBeastId);
    }
    public override bool OnClickBeast(long unBeastId)
    {
        return this.m_curState.OnSelectBeast(unBeastId);
    }
    public override bool OnHoverPos(CVector3 vec3Hex)
    {
        return this.m_curState.OnHoverPos(vec3Hex);
    }
    public override bool OnSelectPos(CVector3 vec3Hex)
    {
        return this.m_curState.OnSelectPos(vec3Hex);
    }
    public override bool OnButtonOkClick()
    {
        return this.m_curState.OnButtonOkClick();
    }
    #endregion
    #region 公有方法
    public bool ChangeSkill(EnumSkillType eType,int skillId)
    {
        //int num = EnumSkillType.eSkillType_Skill == eType
        UseSkillBase useSkillBase = null;
        bool result = false;
        if (this.m_dicSkillState.ContainsKey(eType))
        {
            this.m_dicSkillState[eType].TryGetValue(skillId, out useSkillBase);
            if (useSkillBase != null)
            {
                if (this.m_curState != null)
                {
                    this.m_curState.OnLeave();
                }
                this.m_curState = useSkillBase;
                // this.m_unCurSkillTypeId = 
                this.m_eCurSkillType = eType;
                this.m_log.Debug(string.Format("ChangeSkill:skillType={0}", this.m_eCurSkillType.ToString()));
                this.m_curState.SkillId = skillId;
                this.m_curState.OnEnter();
                result = true;
                return result;
            }
        }
        ActionState.Singleton.ChangeState(enumSubActionState.eSubActionState_Enable);
        return false;
    }
    #endregion
    #region 私有函数
    private void Register(UseSkillBase skillState)
    {
        if (skillState != null)
        {
            SkillBase skill = SkillGameManager.GetSkillBase(skillState.SkillId);
            if (null == skill)
            {
                this.m_log.Error("null == skillBase,找不到该技能");
            }
            else
            {
                if (!this.m_dicSkillState.ContainsKey(skillState.SkillType))
                {
                    this.m_dicSkillState.Add(skillState.SkillType, new Dictionary<int, UseSkillBase>());
                }
                this.m_dicSkillState[skillState.SkillType][skillState.SkillId] = skillState;
            }
        }
    }
    #endregion
}
