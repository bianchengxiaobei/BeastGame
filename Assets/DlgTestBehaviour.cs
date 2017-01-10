using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgTestBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：
// 模块描述：
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public class DlgTestBehaviour : DlgBehaviourBase
{
    public IXUISprite m_button;
    public override void Init()
    {
        base.Init();
        this.m_button = base.GetUIObject("Sprite") as IXUISprite;
        if (this.m_button == null)
        {
            Debug.LogError("this.button == null");
        }
    }
}
