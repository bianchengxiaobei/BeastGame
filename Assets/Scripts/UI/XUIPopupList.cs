using UnityEngine;
using System.Collections.Generic;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIPopupList 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.28
// 模块描述：Poplist
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// Poplist
/// </summary>
[AddComponentMenu("XUI/XUIPopupList")]
public class XUIPopupList : XUIObject,IXUIObject,IXUIPopupList
{
    private struct SPopupListItem
    {
        private string m_strName;
        private object m_data;
        public string Name
        {
            get
            {
                return this.m_strName;
            }
        }
        public object Data
        {
            get
            {
                return this.m_data;
            }
        }
        public SPopupListItem(string strTitle, object data)
        {
            this.m_strName = strTitle;
            this.m_data = data;
        }
    }
    private short m_stSelectedIndex;
    private string m_strSelection = string.Empty;
    private PopupListSelectEventHanler m_popupListSelectEventHandler;
    private UIPopupList m_uiPopupList;
    private List<XUIPopupList.SPopupListItem> m_listItems = new List<XUIPopupList.SPopupListItem>();
    public short SelectedIndex
    {
        get
        {
            return this.m_stSelectedIndex;
        }
        set
        {
            if (value >= 0 && (int)value < this.m_listItems.Count)
            {
                this.m_stSelectedIndex = value;
                this.m_strSelection = this.m_listItems[(int)value].Name;
                if (null != this.m_uiPopupList)
                {
                    this.m_uiPopupList.Set(this.m_strSelection, false);
                }
            }
        }
    }
    public string Selection
    {
        get
        {
            this.m_strSelection = this.m_uiPopupList.value;
            return this.m_strSelection;
        }
        set
        {
            short num = 0;
            while ((int)num < this.m_listItems.Count)
            {
                if (this.m_listItems[(int)num].Equals(value))
                {
                    this.m_strSelection = value;
                    this.m_stSelectedIndex = num;
                    if (null != this.m_uiPopupList)
                    {
                        this.m_uiPopupList.Set(this.m_strSelection, false);
                    }
                }
                num += 1;
            }
        }
    }
    public bool AddItem(string strItem)
    {
        return this.AddItem(strItem, null);
    }
    public bool AddItem(string strItem, object data)
    {
        XUIPopupList.SPopupListItem item = new XUIPopupList.SPopupListItem(strItem, data);
        this.m_listItems.Add(item);
        if (null != this.m_uiPopupList)
        {
            this.m_uiPopupList.items.Add(strItem);
        }
        return true;
    }
    public void Clear()
    {
        this.m_listItems.Clear();
        if (null != this.m_uiPopupList)
        {
            this.m_uiPopupList.items.Clear();
            this.m_uiPopupList.Set(string.Empty, false);
        }
    }
    public object GetDataByIndex(int nIndex)
    {
        if (nIndex < this.m_listItems.Count)
        {
            return this.m_listItems[nIndex].Data;
        }
        return null;
    }
    public void RegisterPopupListSelectEventHandler(PopupListSelectEventHanler eventHandler)
    {
        this.m_popupListSelectEventHandler = eventHandler;
    }
    public override void Init()
    {
        base.Init();
        this.m_uiPopupList = base.GetComponent<UIPopupList>();
        if (null != this.m_uiPopupList)
        {
            EventDelegate ed = new EventDelegate();
            ed.target = this;
            ed.methodName = "OnSelectionChange";
            ed.parameters[0] = new EventDelegate.Parameter(this, "Selection");
            this.m_uiPopupList.onChange.Add(ed);
            for (int i = 0; i < this.m_uiPopupList.items.Count; i++)
            {
                XUIPopupList.SPopupListItem item = new XUIPopupList.SPopupListItem(this.m_uiPopupList.items[i], null);
                this.m_listItems.Add(item);
            }
        }
        else
        {
            Debug.LogError("null == m_uiPopupList");
        }
    }
    private void OnSelectionChange(string strItem)
    {
        int num = 0;
        while (num < this.m_listItems.Count)
        {
            if (this.m_listItems[num].Name.Equals(strItem))
            {
                this.m_stSelectedIndex = (short)num;
                this.m_strSelection = strItem;
                if (this.m_popupListSelectEventHandler != null)
                {
                    this.m_popupListSelectEventHandler(this);
                }
            }
            num += 1;
        }
    }
}
