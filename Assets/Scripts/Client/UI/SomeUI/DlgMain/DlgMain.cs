using UnityEngine;
using System.Collections.Generic;
using Client.UI.UICommon;
using Client.UI;
using Utility.Export;
using Client.Common;
using Utility;
using Client.Data;
using Game;
using Client.GameMain.OpState;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgMain 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.18
// 模块描述：匹配战斗主界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 匹配战斗主界面
/// </summary>
public class DlgMain : DlgBase<DlgMain,DlgMainBehaviour>
{
    #region 字段
    private IXLog m_log = XLog.GetLog<DlgMain>();
    //角色所在的战斗阶段
    private EClientRoleStage m_eRoleStage = EClientRoleStage.ROLE_STAGE_INVALID;
    private EQueryTimeType m_eQueryTimeType = EQueryTimeType.NORMAL_QUERY_TIME;
    private float m_fTotalTime = 0f;//进度条总时间
    private float m_fBackupTime = 0f;
    private float m_fSecondStart = 0f;
    #endregion
    #region 属性
    public override string fileName
    {
        get
        {
            return "DlgMain";
        }
    }
    public override int layer
    {
        get
        {
            return 0;
        }
    }
    #endregion
    #region 构造方法
    #endregion
    #region 重写方法
    public override void Init()
    {
        base.Init();
        this.OnSelfEnterRoleStage(this.m_eRoleStage, (uint)this.m_fBackupTime, (uint)(this.m_fTotalTime - this.m_fBackupTime), this.m_eQueryTimeType);
    }
    protected override void OnRefresh()
    {
        base.OnRefresh();
        this.RefreshPlayerRoleInfo();
        this.RefreshPlayerRoleMoney();
        this.RefreshAllRole();
        this.RefreshSkill(true);
    }
    protected override void OnShow()
    {
        this.Refresh();
        bool flag = !Singleton<RoomManager>.singleton.IsObserver;
        base.uiBehaviour.m_Button_Finish.SetVisible(flag);
        base.uiBehaviour.m_Button_Finish.SetEnable(flag);
        Singleton<OpStateManager>.singleton.Refresh();
    }
    public override void RegisterEvent()
    {
        this.uiBehaviour.m_Button_Move.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickMoveButton));
        this.uiBehaviour.m_Button_Attack.RegisterPressDownEventHandler(new PressDownEventHandler(this.OnClickDownNormalAttack));
        this.uiBehaviour.m_Button_Attack.RegisterPressUpEventHandler(new PressUpEventHandler(this.OnClickUpNormalAttack));
    }
    #endregion
    #region 公有方法
    /// <summary>
    /// 根据玩家的战斗阶段初始化
    /// </summary>
    /// <param name="eRoleStage"></param>
    /// <param name="unBackUpTime"></param>
    /// <param name="unTimeLimit"></param>
    /// <param name="eQueryTimeType"></param>
    public void OnSelfEnterRoleStage(EClientRoleStage eRoleStage, uint unBackUpTime, uint unTimeLimit, EQueryTimeType eQueryTimeType)
    {
        this.m_eRoleStage = eRoleStage;
        this.m_eQueryTimeType = eQueryTimeType;
        switch (this.m_eRoleStage)
        {
            case EClientRoleStage.ROLE_STAGE_REMOVE:
                break;
            case EClientRoleStage.ROLE_STAGE_WAIT:
                this.EndShowStage();
                break;
               
        }
        if (base.Prepared)
        {
            if (this.m_eRoleStage == EClientRoleStage.ROLE_STAGE_MOVE)
            {
                base.uiBehaviour.m_Button_Finish.SetVisible(true);
                base.uiBehaviour.m_Label_Finish.SetText("Stop");
            }
            else if (this.m_eRoleStage == EClientRoleStage.ROLE_STAGE_ACTION)
            {
                base.uiBehaviour.m_Button_Finish.SetVisible(true);
                base.uiBehaviour.m_Label_Finish.SetText("End");
            }
            else if (this.m_eRoleStage == EClientRoleStage.ROLE_STAGE_WAIT)
            {
                base.uiBehaviour.m_Button_Finish.SetVisible(false);
            }
            else if (this.m_eRoleStage == EClientRoleStage.ROLE_STAGE_SELECT_BORN_POS)
            {
                base.uiBehaviour.m_Button_Finish.SetVisible(false);
            }
        }
    }
    /// <summary>
    /// 显示角色的基础信息,比如血量，速度，攻击距离等
    /// </summary>
    public void RefreshPlayerRoleInfo()
    {
        if (base.Prepared)
        {
            int beastTypeId = Singleton<BeastRole>.singleton.Beast.BeastTypeId;
            DataBeastlist data = GameData<DataBeastlist>.dataMap[beastTypeId];
            if (data != null)
            {
                base.uiBehaviour.m_Label_RoleName.SetText(data.Name);
                string beastIconIndex = Singleton<RoomManager>.singleton.GetBeastIcon(Singleton<BeastRole>.singleton.Beast.Id);
                base.uiBehaviour.m_Sprite_AvatarIcon.SetSprite(beastIconIndex,UIManager.singleton.GetAtlasName(EnumAtlasType.Beast,beastIconIndex));
                CBeastData beastData = Singleton<PlayerRole>.singleton.GetBeastData(beastTypeId, true);
                if (beastData != null)
                {
                    //这里是玩家数量度这些，无关紧要
                }
            }
            Color color = Singleton<BeastRole>.singleton.Beast.IsDead ? UnityTools.GrayColor : Color.white;
            base.uiBehaviour.m_Sprite_AvatarIcon.Color = color;
            int hp = Singleton<BeastRole>.singleton.Beast.Hp;
            int hpMax = Singleton<BeastRole>.singleton.Beast.HpMax;
            string text = string.Format("Life {0}/{1}", hp, hpMax);
            base.uiBehaviour.m_Label_Life.SetText(text);
            base.uiBehaviour.m_Label_Speed.SetText(string.Format("Speed {0}", Singleton<BeastRole>.singleton.Beast.MaxMoveDis.ToString()));
        }
    }
    /// <summary>
    /// 刷新角色拥有的金币
    /// </summary>
    public void RefreshPlayerRoleMoney()
    {
        if (base.Prepared)
        {
            //金币改变专门通过消息来处理
            //base.uiBehaviour.m_Label_Gold.SetText(Singleton<BeastRole>.singleton.Beast.Money.ToString());
        }
    }
    /// <summary>
    /// 刷新技能
    /// </summary>
    /// <param name="bNeedRebuild"></param>
    public void RefreshSkill(bool bNeedRebuild)
    {
        if (base.Prepared)
        {
            if (bNeedRebuild)
            {
                List<SkillGameData> skills = Singleton<BeastRole>.singleton.Beast.Skills;
                for (int i = 0; i < skills.Count; i++)
                {
                    SkillGameData skillData = skills[i];
                    switch (skillData.SkillType)
                    {
                        case 0:
                            base.uiBehaviour.m_Sprite_SkillQ.SetSprite(skillData.IconFile,UIManager.singleton.GetAtlasName(EnumAtlasType.Skill,skillData.IconFile));
                            break;
                        case 1:
                            base.uiBehaviour.m_Sprite_SkillW.SetSprite(skillData.IconFile, UIManager.singleton.GetAtlasName(EnumAtlasType.Skill, skillData.IconFile));
                            break;
                        case 2:
                            base.uiBehaviour.m_Sprite_SkillE.SetSprite(skillData.IconFile, UIManager.singleton.GetAtlasName(EnumAtlasType.Skill, skillData.IconFile));
                            break;
                        case 3:
                            base.uiBehaviour.m_Sprite_SkillR.SetSprite(skillData.IconFile, UIManager.singleton.GetAtlasName(EnumAtlasType.Skill, skillData.IconFile));
                            break;
                        case 4:
                            break;
                        case 5:
                            break;

                    }
                }
            }
            else
            {

            }
        }
    }
    /// <summary>
    /// 刷新所有己方角色信息
    /// </summary>
    public void RefreshAllRole()
    {
        if (Singleton<RoomManager>.singleton.MatchType == EMatchtype.MATCH_1C3)
        {
            if (base.Prepared)
            {
                List<Beast> list = Singleton<BeastManager>.singleton.GetAllBeastByCamp(Singleton<PlayerRole>.singleton.CampType);
                for (int i = 0; i < list.Count; i++)
                {
                    IXUIListItem item = base.uiBehaviour.m_List_RoleList.GetItemByIndex(i);
                    if (null == item)
                    {
                        item = base.uiBehaviour.m_List_RoleList.AddListItem();
                    }
                    Beast beast = list[i];
                    item.Id = beast.Id;
                    DataBeastlist data = GameData<DataBeastlist>.dataMap[beast.BeastTypeId];
                    int hp = beast.Hp;
                    int hpMax = beast.HpMax;
                    string text = string.Format("{0}/{1}", hp, hpMax);
                    if (data != null)
                    {
                        item.SetText("lb_roleName", data.Name);//设置神兽名称
                        item.SetText("lb_life", text);//设置神兽血量
                        item.SetIconSprite(data.IconFile,UIManager.singleton.GetAtlasName(EnumAtlasType.Beast,data.IconFile));
                    }
                }
            }
        }
    }
    public void EnableButtonFinish(bool enable,EClientRoleStage eRoleStage)
    {
        if (base.Prepared)
        {
            base.uiBehaviour.m_Button_Finish.SetVisible(false);
            if (eRoleStage == EClientRoleStage.ROLE_STAGE_MOVE || eRoleStage == EClientRoleStage.ROLE_STAGE_REMOVE)
            {
                base.uiBehaviour.m_Label_Finish.SetText("Stop");
            }
            if (eRoleStage == EClientRoleStage.ROLE_STAGE_ACTION)
            {
                base.uiBehaviour.m_Label_Finish.SetText("End");
            }
        }
    }
    /// <summary>
    /// 高亮显示技能
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="skillIds"></param>
    public void HighlightSkills(EnumSkillType eType,List<int> skillIds)
    {
        if (base.Prepared)
        {
            if (EnumSkillType.eSkillType_Skill == eType)
            {

            }
        }
    }
    /// <summary>
    /// 是否高亮显示技能
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="bVisible"></param>
    public void HighlightSkills(EnumSkillType eType, bool bVisible)
    {
        if (base.Prepared)
        {
            if (EnumSkillType.eSkillType_Skill == eType)
            {

            }
        }
    }
    #endregion
    #region 私有方法
    /// <summary>
    /// 不显示战斗阶段的界面
    /// </summary>
    private void EndShowStage()
    {
        this.m_fTotalTime = 0f;
        if (base.Prepared)
        {
            this.uiBehaviour.m_Button_Finish.SetVisible(false);
            //base.uiBehaviour.
        }
    }
    /// <summary>
    /// 点击MOve按钮
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private bool OnClickMoveButton(IXUIButton button)
    {
        if (Singleton<BeastRole>.singleton.Beast.Id == Singleton<RoomManager>.singleton.BeastIdInRound)
        {
            NetworkManager.singleton.SendBeastEnterStage((int)EClientRoleStage.ROLE_STAGE_MOVE);
        }
        else
        {
            return false;
        }     
        return true;
    }
    private bool OnClickDownNormalAttack(IXUIObject button)
    {
        Debug.Log("Press Down");
        if (Singleton<BeastRole>.singleton.Beast.Id == Singleton<RoomManager>.singleton.BeastIdInRound)
        {
            Debug.Log("发送进入到使用技能的阶段");
            NetworkManager.singleton.SendBeastEnterStage((int)EClientRoleStage.ROLE_STAGE_ACTION);
        }
        if (Singleton<OpStateManager>.singleton.OnSelectSkill(EnumSkillType.eSkillType_Skill, 1))
        {
            return true;
        }
        Debug.Log("False");
        return false;
    }
    private bool OnClickUpNormalAttack(IXUIObject button)
    {
        Debug.Log("Press Up");
        if (Singleton<OpStateManager>.singleton.OnClickSkill(EnumSkillType.eSkillType_Skill, 1))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 点击结束按钮
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private bool OnButtonFinishClick(IXUIButton button)
    {
        Singleton<OpStateManager>.singleton.OnButtonFinishClick();
        return true;
    }
    #endregion
    #region 析构方法
    #endregion
}
