using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using UILib.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgCreateRoleBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.21
// 模块描述：角色创建界面组件初始化
//----------------------------------------------------------------*/
#endregion
public class DlgCreateRoleBehaviour : DlgBehaviourBase
{
    public IXUIButton m_Button_EnterGame = null;//进入游戏按钮
    public IXUIButton m_Button_BackLogin = null;//退后到登录界面按钮
    public IXUIButton m_Button_BackSelectRoleType = null;//返回到选择角色职业按钮
    public IXUIButton m_Button_Next = null;//进入修饰角色头发等按钮
    #region 人物选择按钮
    public IXUICheckBox m_Button_Explorer = null;//探险家
    public IXUICheckBox m_Button_Engineer = null;//工程师
    public IXUICheckBox m_Button_SoulHunter = null;//狩魔人
    public IXUICheckBox m_Button_Cultivator = null;//修炼者
    public IXUICheckBox m_Button_Magician = null;//魔法师
    public IXUICheckBox m_Button_WitchDoctor = null;//巫医
    public IXUIList m_List_RoleType = null;//角色职业列表
    #endregion
    #region 人物性别选择
    public IXUICheckBox m_Button_RoleMan;//男性
    public IXUICheckBox m_Buttin_RoleWoman;//女性
    #endregion
    public IXUIGroup m_Label_RoleIntroduce = null;//角色介绍
    public IXUIPicture m_Sprite_RoleMovie = null;//角色视频
    public IXUIInput m_Input_RoleName = null;//角色名字
    public override void Init()
    {
        base.Init();
        #region 进入游戏按钮
        this.m_Button_EnterGame = base.GetUIObject("pn_create2/bt_entergame") as IXUIButton;
        if (null == this.m_Button_EnterGame)
        {
            Debug.Log("this.ButtonEnterGame == null");
            this.m_Button_EnterGame = WidgetFactory.CreateWidget<IXUIButton>();
        }
        #endregion
        #region 退回登陆界面按钮
        this.m_Button_BackLogin = base.GetUIObject("pn_create2/bt_backlogin") as IXUIButton;
        if (null == this.m_Button_BackLogin)
        {
            Debug.Log("this.m_Button_Back == null");
            this.m_Button_BackLogin = WidgetFactory.CreateWidget<IXUIButton>();
        }
        #endregion
        #region 进入修饰角色头发等按钮
        this.m_Button_Next = base.GetUIObject("pn_create1/bt_next") as IXUIButton;
        if (this.m_Button_Next == null)
        {
            Debug.LogWarning("ButtonNext == null");
            this.m_Button_Next = WidgetFactory.CreateWidget<IXUIButton>();
        }
        #endregion
        #region 返回到选择角色职业按钮
        this.m_Button_BackSelectRoleType = base.GetUIObject("pn_create2/bt_back") as IXUIButton;
        #endregion
        #region 人物选择按钮
        /*this.m_Button_Explorer = base.GetUIObject("Explorer") as IXUICheckBox;
        this.m_Button_Engineer = base.GetUIObject("Engineer") as IXUICheckBox;
        this.m_Button_SoulHunter = base.GetUIObject("SoulHunter") as IXUICheckBox;
        this.m_Button_Cultivator = base.GetUIObject("Cultivator") as IXUICheckBox;
        this.m_Button_Magician = base.GetUIObject("Magician") as IXUICheckBox;
        this.m_Button_WitchDoctor = base.GetUIObject("WitchDoctor") as IXUICheckBox;
        */
        this.m_List_RoleType = base.GetUIObject("pn_create1/sp_link/sp_roletype_bg/tb_roletype") as IXUIList;
        #endregion
        #region 人物性别
        /* this.m_Button_RoleMan = base.GetUIObject("Sex/Man") as IXUICheckBox;
         this.m_Buttin_RoleWoman = base.GetUIObject("Sex/Woman") as IXUICheckBox;
         */
        #endregion
        #region 人物介绍
        this.m_Label_RoleIntroduce = base.GetUIObject("pn_create1/sp_intro") as IXUIGroup;
        #endregion
        #region 人物视频
        this.m_Sprite_RoleMovie = base.GetUIObject("pn_create1/sp_intro/sp_video/tx_video") as IXUIPicture;
        #endregion
        #region 人物名字
        this.m_Input_RoleName = base.GetUIObject("pn_create2/sp_link/ip_username") as IXUIInput;
        #endregion 
    }
}
