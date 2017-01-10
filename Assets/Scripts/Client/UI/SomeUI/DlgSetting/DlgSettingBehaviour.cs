using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgSettingBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.28
// 模块描述：游戏设置界面组件
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏设置界面组件
/// </summary>
public class DlgSettingBehaviour : DlgBehaviourBase
{
    #region 字段
    public IXUIPopupList m_Poplist_RS = null;
    #endregion
    public override void Init()
    {
        base.Init();
        this.m_Poplist_RS = base.GetUIObject("pl_rspoplist") as IXUIPopupList;
        if (this.m_Poplist_RS == null)
        {
            Debug.Log("Poplist_RS == null");
        }
    }
}
