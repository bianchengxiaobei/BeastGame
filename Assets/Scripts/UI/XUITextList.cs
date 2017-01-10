using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUITextList 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.13
// 模块描述：文本实现类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 文本实现类
/// </summary>
[AddComponentMenu("XUI/XUITextList")]
public class XUITextList : XUIObject, IXUIObject, IXUITextList
{
    private UITextList m_uiTextList;
    public int OffsetLine
    {
        get
        {
            if (null != this.m_uiTextList)
            {
                return (int)this.m_uiTextList.scrollValue;
            }
            return 0;
        }
        set
        {
            if (null != this.m_uiTextList)
            {
                this.m_uiTextList.scrollValue = (float)value;
            }
        }
    }
    public int TotalLine
    {
        get
        {
            if (null != this.m_uiTextList)
            {
                return this.m_uiTextList.mTotalLines;
            }
            return 1;
        }
    }
    public int MaxShowLine
    {
        get
        {
            if (null != this.m_uiTextList)
            {
                return this.m_uiTextList.scrollHeight;
            }
            return 1;
        }
    }
    public void Clear()
    {
        if (null != this.m_uiTextList)
        {
            this.m_uiTextList.Clear();
        }
    }
    public void Add(string text)
    {
        if (null != this.m_uiTextList)
        {
            this.m_uiTextList.Add(text);
        }
    }
    public override void Init()
    {
        base.Init();
        if (null == this.m_uiTextList)
        {
            this.m_uiTextList = base.GetComponent<UITextList>();
            if (null == this.m_uiTextList)
            {
                this.m_uiTextList = base.GetComponentInChildren<UITextList>();
            }
        }
        if (null == this.m_uiTextList)
        {
            Debug.LogError("null == m_uiTextList");
        }
    }
}
