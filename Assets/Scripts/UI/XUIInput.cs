using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIInput
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
[AddComponentMenu("XUI/XUIInput")]
public class XUIInput : XUIObject, IXUIObject, IXUIInput
{
    private UIInput m_uiInput;
    protected Collider m_collider;
    private InputSubmitEventHandler m_inputSubmitEventHandler;
    private InputChangeEventHandler m_inputChangeEventHandler;
    public bool IsSelected
    {
        get
        {
            return null != this.m_uiInput && this.m_uiInput.isSelected;
        }
        set
        {
            if (null != this.m_uiInput)
            {
                this.m_uiInput.isSelected = value;
            }
        }
    }
    public override void Init()
    {
        base.Init();
        this.m_uiInput = base.GetComponent<UIInput>();
        this.m_collider = base.GetComponent<Collider>();
        if (null != this.m_uiInput)
        {
            this.m_uiInput.onSubmit.Add(new EventDelegate(this,"_OnSubmit"));
        }
        else
        {
            Debug.LogError("null == m_uiInput");
        }
    }
    public string GetText()
    {
        UIInput component = base.GetComponent<UIInput>();
        if (null != component)
        {
            return component.value;
        }
        return string.Empty;
    }
    public void SetEnable(bool bEnable)
    {
        if (null != this.m_collider)
        {
            this.m_collider.enabled = bEnable;
        }
    }
    public bool IsEnable()
    {
        return this.m_collider.enabled;
    }
    public void SetText(string strText)
    {
        UIInput component = base.GetComponent<UIInput>();
        if (null != component)
        {
            component.value = strText;
        }
    }
    public void RegisterSubmitEventHandler(InputSubmitEventHandler eventHandler)
    {
        this.m_inputSubmitEventHandler = eventHandler;
    }
    public void RegisterChangeEventHandler(InputChangeEventHandler eventHandler)
    {
        this.m_inputChangeEventHandler = eventHandler;
    }
    private void _OnSubmit(string strText)
    {
        if (this.m_inputSubmitEventHandler != null && this.m_inputSubmitEventHandler(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    private void OnInputChanged(UIInput uiInput)
    {
        if (this.m_inputChangeEventHandler != null && this.m_inputChangeEventHandler(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
}

