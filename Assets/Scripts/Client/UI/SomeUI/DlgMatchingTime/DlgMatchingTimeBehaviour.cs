using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgMatchingTimeBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.26
// 模块描述：匹配等待界面组件
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 匹配等待界面组件
/// </summary>
public class DlgMatchingTimeBehaviour : DlgBehaviourBase
{
	#region 字段
    public IXUIButton m_Button_Cancel = null;
    public IXUILabel m_Label_TimeYuJi = null;
    public IXUILabel m_Label_TimeInfact = null;
	#endregion
	#region 公共方法
    public override void Init()
    {
        base.Init();
        this.m_Button_Cancel = base.GetUIObject("Cancel") as IXUIButton;
        this.m_Label_TimeYuJi = base.GetUIObject("YujiTime/YujiTimeLabel") as IXUILabel;
        if (this.m_Label_TimeYuJi == null)
        {
            Debug.Log("this.m_Label_TimeYuJi == null");
        }
        this.m_Label_TimeInfact = base.GetUIObject("InfactTime/InfactLabel") as IXUILabel;
    }
	#endregion
	
}
