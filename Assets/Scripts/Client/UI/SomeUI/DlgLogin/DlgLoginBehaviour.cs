using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using UILib.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgLoginBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class DlgLoginBehaviour : DlgBehaviourBase
{
    public IXUIButton m_Button_Enter = null;
    public IXUILabel m_Label_Ver = null;
    //public IXUIGroup m_Group_Input = null;
    public IXUICheckBox m_Checkbox_Remember = null;
    public IXUIButton m_Label_Register = null;
    public IXUIButton m_Label_Forget = null;
    public IXUIInput m_Input_ID = null;
    public IXUIInput m_Input_PW = null;


    public override void Init()
    {
        base.Init();
        this.m_Button_Enter = (base.GetUIObject("bt_entergame") as IXUIButton);
        if (null == this.m_Button_Enter)
        {
            Debug.Log("Button_Enter is null!");
            this.m_Button_Enter = WidgetFactory.CreateWidget<IXUIButton>();
        }
        this.m_Label_Ver = (base.GetUIObject("lb_version") as IXUILabel);
        if (null == this.m_Label_Ver)
        {
            Debug.Log("Anchor/Label_Ver is null!");
            this.m_Label_Ver = WidgetFactory.CreateWidget<IXUILabel>();
        }
        /*this.m_Group_Input = (base.GetUIObject("Scale/Group_Input") as IXUIGroup);
        if (null == this.m_Group_Input)
        {
            Debug.Log("Group_Input is null!");
            this.m_Group_Input = WidgetFactory.CreateWidget<IXUIGroup>();
        }
        */
        this.m_Checkbox_Remember = (base.GetUIObject("sp_input_bg/cb_remenber") as IXUICheckBox);
        if (null == this.m_Checkbox_Remember)
        {
            Debug.Log("Checkbox_Remember is null!");
            this.m_Checkbox_Remember = WidgetFactory.CreateWidget<IXUICheckBox>();
        }
        this.m_Label_Register = (base.GetUIObject("sp_input_bg/bt_regist") as IXUIButton);
        if (null == this.m_Label_Register)
        {
            Debug.Log("Group_Input/Account/Label_Register is null!");
            this.m_Label_Register = WidgetFactory.CreateWidget<IXUIButton>();
        }
        this.m_Label_Forget = (base.GetUIObject("sp_input_bg/bt_forget") as IXUIButton);
        if (null == this.m_Label_Forget)
        {
            Debug.Log("Group_Input/Account/Label_Forget is null!");
            this.m_Label_Forget = WidgetFactory.CreateWidget<IXUIButton>();
        }
        this.m_Input_ID = (base.GetUIObject("sp_input_bg/ip_username") as IXUIInput);
        if (null == this.m_Input_ID)
        {
            Debug.Log("Group_Input/Input_ID is null!");
            this.m_Input_ID = WidgetFactory.CreateWidget<IXUIInput>();
        }
        this.m_Input_PW = (base.GetUIObject("sp_input_bg/ip_password") as IXUIInput);
        if (null == this.m_Input_PW)
        {
            Debug.Log("Group_Input/Input_PW is null!");
            this.m_Input_PW = WidgetFactory.CreateWidget<IXUIInput>();
        }
    }
}
