using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgFlyTextBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.23
// 模块描述:浮动文字组件初始化
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 浮动文字组件初始化
/// </summary>
public class DlgFlyTextBehaviour : DlgBehaviourBase
{
    public IXUIList List_Demage = null;
    public IXUISprite m_Sprite_Red_Low;
    public IXUISprite m_Sprite_Red_Middle;
    public IXUISprite m_Sprite_Red_High;
    public IXUISprite m_Sprite_Blur;
    public override void Init()
    {
        base.Init();
        this.List_Demage = base.GetUIObject("List_Damage") as IXUIList;
    }
}
