using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgSelectRoleBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.9.18
// 模块描述：角色选择界面组件初始化
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色选择界面组件初始化
/// </summary>
public class DlgSelectRoleBehaviour : DlgBehaviourBase
{
	public IXUIButton m_Button_Sure = null;
    public IXUIButton m_Button_Back = null;
    public IXUIButton m_Button_Create = null;
    public IXUIButton m_Button_Delete = null;
    public IXUIList m_List_RoleList = null;
    public IXUIGroup m_Model_RoleModel = null;
    public override void Init()
    {
        base.Init();
        this.m_Button_Sure = base.GetUIObject("sureButton") as IXUIButton;
        this.m_Button_Back = base.GetUIObject("backButton") as IXUIButton;
        this.m_Button_Create = base.GetUIObject("createButton") as IXUIButton;
        this.m_Button_Delete = base.GetUIObject("deleteButton") as IXUIButton;
        this.m_List_RoleList = base.GetUIObject("roleList") as IXUIList;
        this.m_Model_RoleModel = base.GetUIObject("roleModel") as IXUIGroup;
    }
}
