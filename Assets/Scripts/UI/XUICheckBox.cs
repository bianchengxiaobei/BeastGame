using Client.UI.UICommon;
using System;
using UnityEngine;
[AddComponentMenu("XUI/XUICheckBox")]
public class XUICheckBox : XUIObject, IXUIObject, IXUICheckBox
{
    private CheckBoxOnCheckEventHandler m_eventHandlerOnCheck;
    private UIButton m_uiButton;
    private UIToggle m_uiCheckBox;
    private BoxCollider m_uiBoxCollider;
    public bool bChecked
    {
        get
        {
            return null != this.m_uiCheckBox && this.m_uiCheckBox.value;
        }
        set
        {
            if (null != this.m_uiCheckBox)
            {
                this.m_uiCheckBox.Set(value);
            }
        }
    }
    public override void Init()
    {
        base.Init();
        this.m_uiCheckBox = base.GetComponent<UIToggle>();
        if (null == this.m_uiCheckBox)
        {
            Debug.LogError("null == m_uiCheckBox");
        }
        this.m_uiButton = base.GetComponent<UIButton>();
        this.m_uiBoxCollider = base.GetComponent<BoxCollider>();
    }
    /// <summary>
    /// 状态改变
    /// </summary>
    /// <param name="bChecked"></param>
    private void OnStateChange(bool bChecked)
    {
        if (this.m_eventHandlerOnCheck != null && this.m_eventHandlerOnCheck(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    /// <summary>
    /// 注册选项被选中的事件
    /// </summary>
    /// <param name="eventHandler"></param>
    public void RegisterOnCheckEventHandler(CheckBoxOnCheckEventHandler eventHandler)
    {
        this.m_eventHandlerOnCheck = eventHandler;
        if (null != this.m_uiCheckBox)
        {
            this.m_uiCheckBox.onChange.Add(new EventDelegate(this, "OnStateChange"));
        }
    }
    public void SetEnable(bool bEnable)
    {
        if (this.m_uiBoxCollider != null)
        {
            this.m_uiBoxCollider.enabled = bEnable;
        }
        if (null != this.m_uiButton)
        {
            this.m_uiButton.isEnabled = bEnable;
        }
    }
}
