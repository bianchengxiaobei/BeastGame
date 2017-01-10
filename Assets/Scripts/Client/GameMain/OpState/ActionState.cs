using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Client.UI.UICommon;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActionState 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.29
// 模块描述：战棋技能战斗阶段
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋技能战斗阶段
/// </summary>
namespace Client.GameMain.OpState.Stage
{
    public class ActionState : OpStateBase
    {
        #region 字段
        private enumSubActionState m_eSubActionStateLast = enumSubActionState.eSubActionState_Disable;
        private enumSubActionState m_eSubActionStateCurrent = enumSubActionState.eSubActionState_Disable;
        private Dictionary<enumSubActionState, SubActionStateBase> m_dicSubActionState = null;
        private static ActionState m_oInstance = new ActionState();
        #endregion
        #region 属性
        public static ActionState Singleton
        {
            get
            {
                return ActionState.m_oInstance;
            }
        }
        #endregion
        #region 构造方法
        public ActionState()
        {
            this.m_dicSubActionState = new Dictionary<enumSubActionState, SubActionStateBase>();
            this.m_dicSubActionState.Add(enumSubActionState.eSubActionState_Disable, new SubActionState_Disable());
            this.m_dicSubActionState.Add(enumSubActionState.eSubActionState_Enable, new SubActionState_Enable());
            this.m_dicSubActionState.Add(enumSubActionState.eSubActionState_SkillUse, SubActionState_UseSkill.Instance);
        }
        #endregion
        #region 公共方法
        public bool ChangeState(enumSubActionState eSubActionState)
        {
            SubActionStateBase subActionStateBase = null;
            this.m_dicSubActionState.TryGetValue(eSubActionState, out subActionStateBase);
            if (null == subActionStateBase)
            {
                return false;
            }
            else
            {
                this.m_eSubActionStateLast = this.m_eSubActionStateCurrent;
                this.m_dicSubActionState[this.m_eSubActionStateLast].OnLeave();
                this.m_eSubActionStateCurrent = eSubActionState;
                this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnEnter();
                return true;
            }
        }
        #endregion
        #region 重写方法
        public override void OnEnter()
        {
            Debug.Log("进入到ActionState");
            this.ChangeState(enumSubActionState.eSubActionState_Enable);
            DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshPlayerRoleInfo();
        }
        public override void OnLeave()
        {
            this.ChangeState(enumSubActionState.eSubActionState_Disable);
            DlgBase<DlgMain, DlgMainBehaviour>.singleton.RefreshPlayerRoleInfo();
        }
        public override void OnUpdate()
        {
            this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnUpdate();
        }
        public override bool OnClick()
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnClick();
        }
        public override bool OnSelectSkill(EnumSkillType eSkillType, int skillId)
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnSelectSkill(eSkillType, skillId);
        }
        /// <summary>
        /// 点击技能按钮
        /// </summary>
        /// <param name="eSkillType"></param>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public override bool OnClickSkill(EnumSkillType eSkillType, int skillId)
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnClickSkill(eSkillType, skillId);
        }
        public override bool OnHoverPos(CVector3 vecHex)
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnHoverPos(vecHex);
        }
        public override bool OnSelectPos(CVector3 vecHex)
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnSelectPos(vecHex);
        }
        public override bool OnHoverBeast(long beastId)
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnHoverBeast(beastId);
        }
        public override bool OnClickBeast(long beastId)
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnClickBeast(beastId);
        }
        public override bool OnButtonOkClick()
        {
            return this.m_dicSubActionState[this.m_eSubActionStateCurrent].OnButtonOkClick();
        }
        #endregion
        #region 析构方法
        #endregion
    }
}