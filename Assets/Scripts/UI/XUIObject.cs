using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIObject
// 创建者：chen
// 修改者列表：
// 创建日期：2016.9.20
// 模块描述：UI物体组件
//----------------------------------------------------------------*/
#endregion
public abstract class XUIObject : XUIObjectBase
{
    private bool m_bEnableOpen = true;
    public override Bounds AbsoluteBounds
    {
        get
        {
            if (this.m_bSizeChanged)
            {
                this.m_AbsoluteBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.CachedTransform);
                this.m_bSizeChanged = false;
            }
            return this.m_AbsoluteBounds;
        }
    }
    public override bool IsEnableOpen
    {
        get
        {
            return this.m_bEnableOpen;
        }
        set
        {
            this.m_bEnableOpen = value;
        }
    }
    public override void SetVisible(bool bVisible)
    {
        if (null != XUITool.Instance)
        {
            XUITool.Instance.SetActive(base.gameObject, bVisible);
        }
    }
    public override void Init()
    {
        base.Init();
    }
    protected override void OnMouseOn()
    {
        base.OnMouseOn();
        if (this.m_mouseOnEventHandler != null && this.m_mouseOnEventHandler(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    protected override void OnMouseLeave()
    {
        base.OnMouseLeave();
        if (this.m_mouseLeaveEventHandler != null && this.m_mouseLeaveEventHandler(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    protected override void OnPressDown()
    {
        base.OnPressDown();
        if (this.m_eventHandlerPressDown != null && this.m_eventHandlerPressDown(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    protected override void OnPressUp()
    {
        base.OnPressUp();
        if (this.m_eventHandlerPressUp != null && this.m_eventHandlerPressUp(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    protected override void _OnClick()
    {
        base._OnClick();
        if (this.m_eventHandlerClick != null && this.m_eventHandlerClick(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    private void OnTooltip(bool bshow)
    {
        XUITool.S_OnTip(bshow, this);
    }
}
