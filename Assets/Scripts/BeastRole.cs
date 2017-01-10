using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
using Client.Common;
using Game;
using Client.Data;
using Client.GameMain.OpState;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BeastRole
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.8
// 模块描述：自身神兽角色类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 自身神兽角色类
/// </summary>
public class BeastRole : Singleton<BeastRole>
{
	#region 字段
    private long m_unBeastId = 0;
    private IXLog m_log = XLog.GetLog<BeastRole>();
	#endregion
	#region 属性
    /// <summary>
    /// 操作的神兽id
    /// </summary>
    public long Id
    {
        get
        {
            return this.m_unBeastId;
        }
        set
        {
            if (this.m_unBeastId != value)
            {
                //this.UnLockAllCard();
                this.m_unBeastId = value;
               // DlgBase<DlgMain, DlgMainBehaviour>.singleton.Refresh();
            }
        }
    }
    /// <summary>
    /// 当前操作阶段
    /// </summary>
    public EClientRoleStage eRoleStage
    {
        get
        {
            EClientRoleStage result;
            if (null == this)
            {
                result = EClientRoleStage.ROLE_STAGE_INVALID;
            }
            else
            {
                result = this.Beast.eRoleStage;
            }
            return result;
        }
    }
    /// <summary>
    /// 根据id从BeastManager里面取得，如果取不到，就取静态错误的数据
    /// </summary>
    public Beast Beast 
    {
        get 
        {
            Beast beastId = Singleton<BeastManager>.singleton.GetBeastById(this.m_unBeastId);
            if (beastId != null)
            {
                return beastId;
            }
            else 
            {
                this.m_log.Error(string.Format("null == hero:{0}", this.m_unBeastId));
                return BeastManager.BeastError;
            }
        }
    }
    /// <summary>
    /// 神兽角色所在的阵营
    /// </summary>
    public ECampType CampType
    {
        get
        {
            ECampType eCampType;
            if (this.Beast == null || this.Beast.IsError)
            {
                eCampType = Singleton<PlayerRole>.singleton.CampType;
            }
            else
            {
                eCampType = this.Beast.eCampType;
            }
            return eCampType;
        }
    }
    /// <summary>
    /// 神兽拥有的技能
    /// </summary>
    public List<SkillGameData> Skills
    {
        get
        {
            if (this.Beast != null)
            {
                return this.Beast.Skills;
            }
            else
            {
                return new List<SkillGameData>();
            }
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 改变神兽角色最大移动距离,刷新战斗界面神兽信息表
    /// </summary>
    /// <param name="unMaxMoveDis"></param>
    public void OnChangeMaxMoveDis(int unMaxMoveDis)
    {
        //DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshPlayerRoleInfo();
    }
   /// <summary>
   /// 神兽移动是，改变主操作界面
   /// </summary>
   /// <param name="pos"></param>
    public void OnMove(CVector3 pos)
    {
       // DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshPlayerRoleInfo();
    }
    public void OnHpChange(int hp)
    {
        //改变主界面的信息
        Singleton<OpStateManager>.singleton.OnHpChange();
    }
    /// <summary>
    /// 自己的神兽重生，操作界面发送改变
    /// </summary>
    public void OnRevive()
    {
        //主界面的颜色变成白色
        //DlgBase<DlgMain, DlgMainBehaviour>.singleton.OnHeroRoleRevive();
    }
    /// <summary>
    /// 添加技能做的回调
    /// </summary>
    /// <param name="skillId"></param>
    public void OnAddSkill(int skillId)
    {

    }
    /// <summary>
    /// 自己神兽角色进入操作状态
    /// </summary>
    /// <param name="eRoleStage"></param>
    /// <param name="unBackUpTime"></param>
    /// <param name="unTimeLimit"></param>
    /// <param name="unTargetHeroID"></param>
    /// <param name="eQueryTimeType"></param>
    public void OnEnterRoleStage(EClientRoleStage eRoleStage, uint unBackUpTime, uint unTimeLimit, uint unTargetHeroID, EQueryTimeType eQueryTimeType)
    {
        //DlgBase<DlgMain, DlgMainBehaviour>.singleton.OnSelfEnterRoleStage(eRoleStage, unBackUpTime, unTimeLimit, eQueryTimeType);
        switch (eRoleStage)
        {
            case EClientRoleStage.ROLE_STAGE_COMPUTE_STATE:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_Compute);
                break;
            case EClientRoleStage.ROLE_STAGE_TAKE_CARD:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_Wait);
                break;
            case EClientRoleStage.ROLE_STAGE_MOVE:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_Move);
                break;
            case EClientRoleStage.ROLE_STAGE_ACTION:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_Action);
                break;
            case EClientRoleStage.ROLE_STAGE_DISCARD_CRAD:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_DiscardCard);
                break;
            case EClientRoleStage.ROLE_STAGE_SELECT_BORN_POS:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_SelectBornPos);
                break;
            case EClientRoleStage.ROLE_STAGE_REVIVE:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_Revive);
                break;
            case EClientRoleStage.ROLE_STAGE_RE_SELECT_HERO:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_ReSelectHero);
                break;
            case EClientRoleStage.ROLE_STAGE_RE_SELECT_SKILL:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_ReSelectSkill);
                break;
            case EClientRoleStage.ROLE_STAGE_RE_SELECT_CARD:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_ReSelectCard);
                break;
            case EClientRoleStage.ROLE_STAGE_FIRST_AID_QUERY:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_FirstAidQuery);
                break;
            case EClientRoleStage.ROLE_STAGE_DODGE_QUERY:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_DodgeQuery);
                break;
            case EClientRoleStage.ROLE_STAGE_BASE_HURT_DEFENCE_QUERY:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_BaseHurtDefence);
                break;
            case EClientRoleStage.ROLE_STAGE_EXTRACT_ENEMY_CARD:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_ExtractEnemyCard);
                break;
            case EClientRoleStage.ROLE_STAGE_REMOVE:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_ReMove);
                break;
            case EClientRoleStage.ROLE_STAGE_STATUS_PURIFY:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_StatusPurify);
                break;
            case EClientRoleStage.ROLE_STAGE_ALTER_SELF_SKILL_CD:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_AlterSelfSkillCD);
                break;
            case EClientRoleStage.ROLE_STAGE_WAIT:
                Singleton<OpStateManager>.singleton.ChangeState(enumOpState.eOpState_Wait);
                break;
        }
    }
    public void OnRoundStart()
    {
        //主操作界面开始
        this.Beast.IsMoved = false;
    }
    /// <summary>
    /// 刷新神兽阶段状态
    /// </summary>
    /// <param name="unBackUpTime"></param>
    /// <param name="unTimeLimit"></param>
    /// <param name="unTargetHeroID"></param>
    /// <param name="eQueryTimeType"></param>
    public void RefreshRoleStage(uint unBackUpTime, uint unTimeLimit, uint unTargetHeroID, EQueryTimeType eQueryTimeType)
    {
        //刷新神兽阶段状态,离开在重新进入
       // DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshSelfRoleStage(unBackUpTime, unTimeLimit, unTargetHeroID, eQueryTimeType);
        Singleton<OpStateManager>.singleton.Refresh();
    }
    /// <summary>
    /// 取得神兽能使用的技能列表
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<int> GetCanUseSkillOrEquip(EnumSkillType type)
    {
        return this.Beast.GetCanUseSkillOrEquip(type);
    }
	#endregion
	#region 私有方法
	#endregion
}
