using UnityEngine;
using System.Collections.Generic;
using Game;
using Client.Common;
using Utility;
using Client.Skill;
using Client.UI;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UseSkill_NormalAttack 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.12.14
// 模块描述：普通攻击
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 普通攻击
/// </summary>
public class UseSkill_NormalAttack : UseSkillBase 
{
    #region 字段
    private CVector3 m_vec3TargetPos = new CVector3();
    private List<CVector3> m_listValidTargetPos = new List<CVector3>();
    private List<long> m_listValidTargetBeastId = new List<long>();
    private long m_unTargetBeastId = 0;
    private int m_unTargetSkillId = 0;
    private EnumSkillType m_eTargetSkillType = EnumSkillType.eSkillType_Skill;
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public UseSkill_NormalAttack()
    {
        this.m_unSkillId = 0;
        this.m_eSkillType = EnumSkillType.eSkillType_Skill;
    }
    #endregion
    #region 重写方法
    public override void OnEnter()
    {
        base.OnEnter();
        SkillBase skill = SkillGameManager.GetSkillBase(this.m_unSkillId);
        if (skill != null)
        {
            this.m_listValidTargetPos = skill.GetValidTargetHexs(Singleton<BeastRole>.singleton.Id);
            this.m_listValidTargetBeastId = skill.GetValidTargetPlayers(Singleton<BeastRole>.singleton.Id);
        }
        Beast beast = Singleton<BeastManager>.singleton.GetBeastById(Singleton<BeastRole>.singleton.Id);
        //还没有攻击过
        if (beast.UsedAttackToBaseBuildingCount < 1)
        {
            Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_Highlight, this.m_listValidTargetPos);
        }
        //高亮角色模型（边缘外发光）
    }
    public override void OnLockOperation()
    {
        base.OnLockOperation();
        //显示提示选择攻击目标

        //设置鼠标类型为攻击
        UIManager.singleton.SetCursor(enumCursorType.eCursorType_Attack);
    }
    public override bool OnHoverBeast(long beastId)
    {
        if (this.m_listValidTargetBeastId.Contains(beastId))
        {
            SkillBase skill = SkillGameManager.GetSkillBase(this.m_unSkillId);
            if (skill != null)
            {
                List<CVector3> affectAreaByTargetBeast = skill.GetAffectAreaByTargetBeast(Singleton<BeastRole>.singleton.Id, beastId);
                Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_Affect, affectAreaByTargetBeast);
                List<long> affectBeastsByTargetBeast = skill.GetAffectBeastsByTargetBeast(Singleton<BeastRole>.singleton.Id, beastId);
                //高亮角色模型

            }
        }
        else
        {
            Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_Affect);
            //不显示角色模型高亮
        }
        return true;
    }
    public override bool OnHoverPos(CVector3 pos)
    {
        if (m_listValidTargetPos.Exists((CVector3 p) => p.Equals(pos)))
        {
            SkillBase skill = SkillGameManager.GetSkillBase(this.m_unSkillId);
            if (skill != null)
            {
                List<CVector3> affectAreaByTargetBeast = skill.GetAffectAreaByTargetPos(Singleton<BeastRole>.singleton.Id, pos);
                Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_Affect, affectAreaByTargetBeast);
                List<long> affectBeastsByTargetBeast = skill.GetAffectBeastsByTargetPos(Singleton<BeastRole>.singleton.Id, pos);
                //高亮角色模型
            }
        }
        else
        {
            Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_Affect);
            //不显示角色模型高亮
        }
        return true;
    }
    public override bool OnSelectPos(CVector3 pos)
    {
        Beast beast = Singleton<BeastManager>.singleton.GetBeastById(Singleton<BeastRole>.singleton.Id);
        if (beast.UsedAttackToBaseBuildingCount >= 1)
        {
            //弹出攻击过的提示消息
            DlgBase<DlgFlyText, DlgFlyTextBehaviour>.singleton.AddSystemInfo(StringConfigMgr.GetString("DlgMain.AttactOncePreRound"));
            return false;
        }
        else
        {
            if (this.m_listValidTargetPos.Exists((CVector3 p) => p.Equals(pos)))
            {
                Beast beastByPos = Singleton<BeastManager>.singleton.GetBeastByPos(pos);
                if (beastByPos != null)
                {
                    this.m_unTargetBeastId = beastByPos.Id;
                }
                else
                {
                    this.m_unTargetBeastId = 0;
                }
                this.m_vec3TargetPos = pos;
                this.OnButtonOkClick();
            }
            return true;
        }
    }
    public override bool OnSelectBeast(long unTargetBeastId)
    {
        if (!this.m_listValidTargetBeastId.Contains(unTargetBeastId))
        {
            return false;
        }
        else
        {
            this.m_unTargetBeastId = unTargetBeastId;
            Beast beast = Singleton<BeastManager>.singleton.GetBeastById(unTargetBeastId);
            if (beast != null)
            {
                this.m_vec3TargetPos.CopyFrom(CVector3.MaxValue);
                this.OnButtonOkClick();
            }
            return true;
        }
    }
    public override bool OnButtonOkClick()
    {
        bool result = false;
        //发送释放技能的请求到服务器
        CPtcC2MReq_CastSkill msg = new CPtcC2MReq_CastSkill();
        msg.m_dwRoleId = Singleton<BeastRole>.singleton.Id;
        msg.m_dwSkillId = this.m_unSkillId;
        if (this.m_unTargetBeastId != 0)
        {
            msg.m_dwTargetRoleId = this.m_unTargetBeastId;
        }
        else
        {
            msg.m_oTargetPos = this.m_vec3TargetPos;
        }
        
        Singleton<NetworkManager>.singleton.SendUseSkill(msg);
        result = true;
        return result;
    }
    #endregion
    #region 私有方法
    #endregion
    #region 析构方法
    #endregion
}
