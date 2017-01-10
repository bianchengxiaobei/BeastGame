using UnityEngine;
using System.Collections.Generic;
using System;
using Utility;
using Utility.Export;
using Client.Common;
using Game;
using Client.GameMain.OpState.Stage;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：OpStateManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：阶段操作管理器
//----------------------------------------------------------------*/
#endregion
namespace Client.GameMain.OpState
{
    /// <summary>
    /// 阶段操作管理器
    /// </summary>
    public class OpStateManager : Singleton<OpStateManager>
    {
        #region 字段
        private IXLog m_log = XLog.GetLog<OpStateManager>();
        private long m_unPlayerId = 0;
        private CVector3 m_oPos = new CVector3();
        private enumOpState m_eOpStateLast = enumOpState.eOpState_Wait;
        private enumOpState m_eOpStateCurrent = enumOpState.eOpState_Wait;
        private Dictionary<enumOpState, OpStateBase> m_dicOpState = new Dictionary<enumOpState, OpStateBase>();
        private int m_unSkillId = 0;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public OpStateManager()
        {
            this.Init();
        }
        #endregion
        #region 公有方法
        public void Init()
        {
            this.m_dicOpState.Add(enumOpState.eOpState_SelectBornPos, new SelectBornPosState());
            this.m_dicOpState.Add(enumOpState.eOpState_Wait, new WaitState());
            this.m_dicOpState.Add(enumOpState.eOpState_Move, new MoveState());
            this.m_dicOpState.Add(enumOpState.eOpState_Action, ActionState.Singleton);
        }
        public void Update()
        {
            try
            {
                OpStateBase opStateBase = null;
                if (this.m_dicOpState.TryGetValue(this.m_eOpStateCurrent, out opStateBase))
                {
                    opStateBase.OnUpdate();
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        public void Refresh()
        {
            this.m_dicOpState[this.m_eOpStateCurrent].Refresh();
        }
        /// <summary>
        /// 改变操作状态
        /// </summary>
        /// <param name="eOpState"></param>
        /// <returns></returns>
        public bool ChangeState(enumOpState eOpState)
        {
            bool result;
            if (this.m_eOpStateCurrent == eOpState)
            {
                result = false;
            }
            else
            {
                OpStateBase opStateBase = null;
                this.m_dicOpState.TryGetValue(eOpState, out opStateBase);
                if (opStateBase == null)
                {
                    result = false;
                }
                else 
                {
                    this.m_eOpStateLast = this.m_eOpStateCurrent;
                    try
                    {
                        this.m_dicOpState[this.m_eOpStateLast].OnLeave();
                    }
                    catch (Exception e)
                    {
                        this.m_log.Fatal(e.ToString());
                    }
                    this.m_eOpStateCurrent = eOpState;
                    try
                    {
                        this.m_dicOpState[this.m_eOpStateCurrent].OnEnter();
                    }
                    catch (Exception e)
                    {
                        this.m_log.Fatal(e.ToString());
                    }
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 血量改变
        /// </summary>
        public void OnHpChange()
        {
            this.m_dicOpState[this.m_eOpStateCurrent].OnHpChange();
        }
        /// <summary>
        /// 选择地图格子点
        /// </summary>
        /// <param name="vecHexPos"></param>
        /// <returns></returns>
        public bool OnSelectPos(CVector3 vecHexPos)
        {
            bool result;
            if (this.m_dicOpState[this.m_eOpStateCurrent].OnSelectPos(vecHexPos))
            {
                this.m_oPos.nCol = vecHexPos.nCol;
                this.m_oPos.nRow = vecHexPos.nRow;
                result = true;
            }
            else
            {
                Beast beast = Singleton<BeastManager>.singleton.GetBeastByPos(vecHexPos);
                if (beast != null && !beast.IsError)
                {
                    if (this.m_dicOpState[this.m_eOpStateCurrent].OnClickBeast(beast.Id))
                    {
                        result = true;
                    }
                }
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 选择技能
        /// </summary>
        /// <param name="eSkillType"></param>
        /// <param name="unSkillId"></param>
        /// <returns></returns>
        public bool OnSelectSkill(EnumSkillType eSkillType, int unSkillId)
        {
            Debug.Log(this.m_eOpStateCurrent.ToString());
            if (this.m_dicOpState[this.m_eOpStateCurrent].OnSelectSkill(eSkillType, unSkillId))
            {
                this.m_unSkillId = unSkillId;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 点击该技能
        /// </summary>
        /// <param name="eSkillType"></param>
        /// <param name="unSkillId"></param>
        /// <returns></returns>
        public bool OnClickSkill(EnumSkillType eSkillType, int unSkillId)
        {
            if (this.m_dicOpState[this.m_eOpStateCurrent].OnClickSkill(eSkillType, unSkillId))
            {
                this.m_unSkillId = unSkillId;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 处理点击事件
        /// </summary>
        /// <returns></returns>
        public bool OnClick()
        {
            return this.m_dicOpState[this.m_eOpStateCurrent].OnClick();
        }
        /// <summary>
        /// 处理点击结束按钮
        /// </summary>
        /// <returns></returns>
        public bool OnButtonFinishClick()
        {
            return this.m_dicOpState[this.m_eOpStateCurrent].OnButtonFinishClick();
        }
        #endregion
        #region 私有方法
        #endregion
    }
}