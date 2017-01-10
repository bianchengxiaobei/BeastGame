using UnityEngine;
using System.Collections;
using Client.UI;
using Client.UI.UICommon;
using System;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgMatchingTime 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.26
// 模块描述：匹配等待预计时间界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 匹配等待预计时间界面
/// </summary>
public class DlgMatchingTime : DlgBase<DlgMatchingTime,DlgMatchingTimeBehaviour>
{
	#region 字段
    private string strForeastTime;
    private uint foreastTime;
    private float m_fTimeMatchStart = 0;
	#endregion
	#region 属性
    public override string fileName
    {
        get
        {
            return "DlgMatchingTime";
        }
    }
    public override int layer
    {
        get
        {
            return -3;
        }
    }
    public override uint Type
    {
        get
        {
            return 160u;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公共方法
    public override void Init()
    {
        base.Init();
        base.uiBehaviour.m_Label_TimeYuJi.SetText(this.strForeastTime);
    }
    public override void RegisterEvent()
    {
        base.uiBehaviour.m_Button_Cancel.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickCancel));
    }
    protected override void OnShow()
    {
        base.OnShow();
        DlgBase<DlgMatch, DlgMatchBehaviour>.singleton.ClickMatch = false;
    }
    public override void Update()
    {
        base.Update();
        if (base.Prepared && base.IsVisible())
        {
            if (this.m_fTimeMatchStart != 0)
            {
                float time = Time.time;
                float duration = time - this.m_fTimeMatchStart;
                TimeSpan span = TimeSpan.FromSeconds(duration);
                string text = string.Format("{0:d2}:{1:d2}", span.Minutes, span.Seconds);
                base.uiBehaviour.m_Label_TimeInfact.SetText(text);
            }
        }
    }
    public void StartMatch(uint time)
    {
        this.foreastTime = time;
        this.m_fTimeMatchStart = Time.time;
        TimeSpan timeSpan = TimeSpan.FromSeconds(this.foreastTime);
        string text = string.Format("{0:d2}:{1:d2}", timeSpan.Minutes, timeSpan.Seconds);
        this.strForeastTime = text;
        if (base.Prepared)
        {
            base.uiBehaviour.m_Label_TimeYuJi.SetText(this.strForeastTime);
        }
    }
	#endregion
	#region 私有方法
    private bool OnClickCancel(IXUIButton button)
    {
        Singleton<NetworkManager>.singleton.SendMatchCancel();
        this.SetVisible(false);
        return true;
    }
	#endregion
	#region 析构方法
	#endregion
}
