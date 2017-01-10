using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgMainBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.16
// 模块描述：匹配战斗的操作主界面组件初始化类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 匹配战斗的操作主界面组件初始化类
/// </summary>
public class DlgMainBehaviour : DlgBehaviourBase
{
    #region 技能
    public IXUIButton m_Button_SkillQ = null;
    public IXUISprite m_Sprite_SkillQ = null;
    public IXUIButton m_Button_SkillW = null;
    public IXUISprite m_Sprite_SkillW = null;
    public IXUIButton m_Button_SkillE = null;
    public IXUISprite m_Sprite_SkillE = null;
    public IXUIButton m_Button_SkillR = null;
    public IXUISprite m_Sprite_SkillR = null;
    #endregion
    #region 装备
    public IXUIList m_List_Equipment = null;
    #endregion
    #region 移动、攻击按钮
    public IXUIButton m_Button_Move = null;
    public IXUIButton m_Button_Attack = null;
    #endregion
    #region 角色
    public IXUIList m_List_RoleList = null;
    public IXUILabel m_Label_RoleName = null;
    public IXUISprite m_Sprite_AvatarIcon = null;
    public IXUILabel m_Label_Speed = null;
    public IXUILabel m_Label_Attack = null;
    public IXUILabel m_Label_Life = null;
    //金币
    public IXUILabel m_Label_Gold = null;
    #endregion
    #region 结束按钮
    public IXUIButton m_Button_Finish = null;
    public IXUILabel m_Label_Finish = null;
    #endregion

    public override void Init()
    {
        base.Init();
        #region 技能
        this.m_Button_SkillQ = base.GetUIObject("pn_skill/bt_skillQ") as IXUIButton;
        this.m_Button_SkillW = base.GetUIObject("pn_skill/bt_skillW") as IXUIButton;
        this.m_Button_SkillE = base.GetUIObject("pn_skill/bt_skillE") as IXUIButton;
        this.m_Button_SkillR = base.GetUIObject("pn_skill/bt_skillR") as IXUIButton;
        this.m_Button_Attack = base.GetUIObject("bt_attack") as IXUIButton;

        this.m_Sprite_SkillQ = base.GetUIObject("pn_skill/bt_skillQ/sp_skillQ") as IXUISprite;
        this.m_Sprite_SkillW = base.GetUIObject("pn_skill/bt_skillW/sp_skillW") as IXUISprite;
        this.m_Sprite_SkillE = base.GetUIObject("pn_skill/bt_skillE/sp_skillE") as IXUISprite;
        this.m_Sprite_SkillR = base.GetUIObject("pn_skill/bt_skillR/sp_skillR") as IXUISprite;
        #endregion
        #region 角色
        this.m_List_RoleList = base.GetUIObject("pn_rolelist/gd_rolelist") as IXUIList;
        this.m_Label_RoleName = base.GetUIObject("bt_avatar/sp_avatar_bg/lb_name") as IXUILabel;
        this.m_Sprite_AvatarIcon = base.GetUIObject("bt_avatar/sp_avatar_bg/sp_avatar") as IXUISprite;
        this.m_Label_Speed = base.GetUIObject("pn_rolestatus/sp_speed/lb_speed") as IXUILabel;
        this.m_Label_Life = base.GetUIObject("pn_rolestatus/sp_life/lb_life") as IXUILabel;
        #endregion
        #region 结束按钮
        this.m_Button_Finish = base.GetUIObject("bt_end") as IXUIButton;
        this.m_Label_Finish = base.GetUIObject("bt_end/sp_background/lb_end") as IXUILabel;
        #endregion
        #region 移动按钮
        this.m_Button_Move = base.GetUIObject("bt_move") as IXUIButton;
        #endregion
    }
}
