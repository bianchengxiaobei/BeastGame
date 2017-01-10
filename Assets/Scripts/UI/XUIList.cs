using UnityEngine;
using System;
using System.Collections.Generic;
using Client.UI.UICommon;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIList
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.2
// 模块描述：UI列表
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// UI列表界面实现类
/// </summary>
[AddComponentMenu("XUI/XUIList")]
public class XUIList : XUIObject, IXUIObject, IXUIList
{
	#region 字段
    #region Inspector显示
    public GameObject m_prefabListItem;
    public bool m_bMultiSelect;
    public bool m_bWidthCenter;
    public EnumMoveCenterType m_eMoveCenterType;
    #endregion
    private List<XUIListItem> m_listXUIListItem = new List<XUIListItem>();
    private List<XUIListItem> m_listXUIListItemSelected = new List<XUIListItem>();
    private bool m_bHasAddItem;
    private bool m_bMoveFinish = true;
    private float m_fStartTime;
    private float m_fMinMagnitude;
    private Vector3 m_vStartPos = Vector3.zero;
    private Vector3 m_vFinishPos = Vector3.zero;
    private UIGrid m_uiGrid;
    private UITable m_uiTable;
    #region 列表事件委托
    private ListSelectEventHandler m_eventHandlerOnSelect;
    private ListSelectEventHandler m_eventHandlerOnUnSelect;
    private ListDoubleClickEventHandler m_eventHandlerOnDoubleClick;
    private ListClickEventHandler m_eventHandlerOnClick;
    private ListDragEventHandler m_eventHandlerOnDrag;
    private ListDragReleaseEventHandler m_eventHandlerOnDragRelease;
    private ListDragOffEventHandler m_eventHandlerOnDragOff;
    private ListDropEventHandler m_eventHandlerOnDrop;
    private ListPressDownEventHandler m_eventHandlerOnPressDown;
    private ListPressUpEventHandler m_eventHandlerOnPressUp;
    private ListMouseOnEventHandler m_eventHandlerOnMouseOn;
    private ListMouseLeaveEventHandler m_eventHandlerOnMouseLeave;
    private ListMoveReleaseEventHandler m_eventHandlerMoveRelease;
    #endregion
    #endregion
    #region 属性
    /// <summary>
    /// 列表中的项目数量
    /// </summary>
    public int Count 
    {
        get
        {
            if (null == this.m_listXUIListItem)
            {
                return 0;
            }
            return this.m_listXUIListItem.Count;
        }
    }
    /// <summary>
    /// 是否启用多选择
    /// </summary>
    public bool EnableMultiSelect 
    {
        get {return this.m_bMultiSelect; }
        set 
        {
            this.m_bMultiSelect = value;
            foreach (var current in this.m_listXUIListItem)
            {
                current.SetEnableMultiSelect(this.m_bMultiSelect);
            }
        }
    }
    #endregion
    #region 构造方法
    #endregion
    #region 公有方法
    /// <summary>
    /// 刷新列表
    /// </summary>
    public void Refresh()
    {
        if (this.m_uiGrid != null)
        {
            this.m_uiGrid.repositionNow = true;
        }
        if (this.m_uiTable != null)
        {
            this.RefreshItemSizeByTable();
            this.m_uiTable.repositionNow = true;
        }
        this.RefreshAllItemStatus();
    }
    /// <summary>
    /// 设置网格内元素的间隔大小
    /// </summary>
    /// <param name="cellWidth"></param>
    /// <param name="cellHight"></param>
    public void SetSize(float cellWidth, float cellHight)
    {
        if (null == this.m_uiGrid)
        {
            return;
        }
        this.m_uiGrid.cellWidth = cellWidth;
        this.m_uiGrid.cellHeight = cellHight;
        this.m_uiGrid.Reposition();
    }
    /// <summary>
    /// 取得选择列表中第一个单项的索引，否则返回-1
    /// </summary>
    /// <returns>索引</returns>
    public int GetSelectedIndex()
    {
        if (this.m_listXUIListItemSelected.Count > 0)
        {
            return this.m_listXUIListItemSelected[0].Index;
        }
        return -1;
    }
    /// <summary>
    /// 设置选择该索引单项，并移到中心
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectedIndex(int index)
    {
        XUIListItem selectedItem = this.GetItemByIndex(index) as XUIListItem;
        this.SetSelectedItem(selectedItem);
        this.FocusIndex(index);
    }
    /// <summary>
    /// 根据索引设置该单项为选中单项
    /// </summary>
    /// <param name="uiListItem"></param>
    public void SetSelectedItem(XUIListItem uiListItem)
    {
        //遍历所有已经选择上的单项，初始化为不选择上
        for (int i = 0; i < this.m_listXUIListItemSelected.Count; i++)
        {
            this.m_listXUIListItemSelected[i].SetSelected(false);
        }
        //清空已经选择上的单项引用
        this.m_listXUIListItemSelected.Clear();
        if (uiListItem != null)
        {
            uiListItem.SetSelected(true);
            this.m_listXUIListItemSelected.Add(uiListItem);
        }
        if (this.m_bWidthCenter && uiListItem != null)
        {
            this.MoveWidthCenter(uiListItem.Index);
        }
    }
    /// <summary>
    /// 设置多选项为选中状态，根据索引
    /// </summary>
    /// <param name="listItemIndex"></param>
    public void SetSelectedItemByIndex(List<int> listItemIndex)
    {
        if (this.m_bMultiSelect)
        {
            this.m_listXUIListItemSelected.Clear();
            foreach (XUIListItem current in this.m_listXUIListItem)
            {
                if (listItemIndex != null && listItemIndex.Contains(current.Index))
                {
                    current.SetSelected(true);
                    this.m_listXUIListItemSelected.Add(current);
                }
                else
                {
                    current.SetSelected(false);
                }
            }
        }
        else
        {
            Debug.LogError("false == m_bMultiSelect");
        }
    }
    /// <summary>
    /// 根据id设置该单项为选中单项
    /// </summary>
    /// <param name="unId"></param>
    public void SetSelectedItemById(long unId)
    {
        XUIListItem selectedItem = this.GetItemById(unId) as XUIListItem;
        this.SetSelectedItem(selectedItem);
    }
    /// <summary>
    /// 根据id设置该多选项为选中状态
    /// </summary>
    /// <param name="listItemId"></param>
    public void SetSelectedItemById(List<long> listItemId)
    {
        if (this.m_bMultiSelect)
        {
            this.m_listXUIListItemSelected.Clear();
            foreach (XUIListItem current in this.m_listXUIListItem)
            {
                if (listItemId != null && listItemId.Contains(current.Id))
                {
                    current.SetSelected(true);
                    this.m_listXUIListItemSelected.Add(current);
                }
                else
                {
                    current.SetSelected(false);
                }
            }
        }
        else
        {
            Debug.LogError("false == m_bMultiSelect");
        }
    }
    /// <summary>
    /// 根据索引取得列表中的单项元素
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IXUIListItem GetItemByIndex(int index)
    {
        if (null == this.m_listXUIListItem)
        {
            return null;
        }
        if (0 > index || index >= this.m_listXUIListItem.Count)
        {
            return null;
        }
        return this.m_listXUIListItem[index];
    }
    /// <summary>
    /// 根据id取得列表中的单项元素
    /// </summary>
    /// <param name="unId"></param>
    /// <returns></returns>
    public IXUIListItem GetItemById(long unId)
    {
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            if (unId == current.Id)
            {
                return current;
            }
        }
        return null;
    }
    /// <summary>
    /// 根据id取得列表中是否可见的元素
    /// </summary>
    /// <param name="unId"></param>
    /// <param name="bVisible">是否可见</param>
    /// <returns></returns>
    public IXUIListItem GetItemById(long unId, bool bVisible)
    {
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            if (unId == current.Id && current.IsVisible() == bVisible)
            {
                return current;
            }
        }
        return null;
    }
    /// <summary>
    /// 获取选中列表中的所哟元素
    /// </summary>
    /// <returns></returns>
    public IXUIListItem[] GetSelectedItems()
    {
        return this.m_listXUIListItemSelected.ToArray();
    }
    /// <summary>
    /// 获取选中列表中的第一个元素
    /// </summary>
    /// <returns></returns>
    public IXUIListItem GetSelectedItem()
    {
        if (this.m_listXUIListItemSelected.Count > 0)
        {
            return this.m_listXUIListItemSelected[0];
        }
        return null;
    }

    /// <summary>
    /// 聚焦到此单项
    /// </summary>
    /// <param name="index"></param>
    public void FocusIndex(int index)
    {
        UIScrollView uiScrollView = base.GetComponent<UIScrollView>();
        UIGrid uiGrid = base.GetComponent<UIGrid>();
        if (null == uiScrollView || null == uiScrollView.panel || null == uiGrid)
        {
            return;
        }
        if (index < this.Count)
        {
            if (this.Count <= 5)
            {
                return;
            }
            if (index < 2)
            {
                index = 2;
            }
            if (index >= this.Count - 2)
            {
                index = this.Count - 3;
            }
            float rangeX = uiGrid.cellWidth * this.Count / 2 - uiGrid.cellWidth * index - uiGrid.cellWidth / 2;
            UIPanel clipPanel = uiScrollView.panel;
            Vector4 clipRange = clipPanel.baseClipRegion;
            clipRange.x = -rangeX;
            clipPanel.baseClipRegion = clipRange;
            Vector3 localPosition = clipPanel.transform.position;
            localPosition.x = rangeX;
            clipPanel.transform.localPosition = localPosition;
        }
    }
    /// <summary>
    /// 根据游戏id获取列表中的元素
    /// </summary>
    /// <param name="ulId"></param>
    /// <returns></returns>
    public IXUIListItem GetItemByGUID(ulong ulId)
    {
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            if (ulId == current.GUID)
            {
                return current;
            }
        }
        return null;
    }
    /// <summary>
    /// 获取所有列表中的元素
    /// </summary>
    /// <returns></returns>
    public IXUIListItem[] GetAllItems()
    {
        return this.m_listXUIListItem.ToArray();
    }
    public IXUIListItem AddListItem(GameObject obj) 
    {
        if (null != obj)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
            gameObject.name = string.Format("{0:0000}", this.Count);//格式是0001，0002......
            gameObject.transform.parent = base.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.identity;
            NGUITools.SetLayer(gameObject, this.CachedGameObject.layer);
            XUIListItem item = gameObject.GetComponent<XUIListItem>();
            if (null == item)
            {
                XLog.Log.Error("null == uiListItem");
                item = gameObject.AddComponent<XUIListItem>();
            }
            item.Index = this.Count;
            item.Id = (long)item.Index;
            item.parent = this;
            item.ParentDlg = this.ParentDlg;
            if (!item.IsInited)
            {
                item.Init();
            }
            this.m_listXUIListItem.Add(item);
            this.m_bHasAddItem = true;
            this.Refresh();
            return item;
        }
        else 
        {
            XLog.Log.Debug("prefabItem == null");
        }
        return null;
    }
    /// <summary>
    /// 添加在Inspector窗口显示的物体到UI列表中
    /// </summary>
    /// <returns></returns>
    public IXUIListItem AddListItem() 
    {
        if (this.m_prefabListItem != null)
        {
            return this.AddListItem(this.m_prefabListItem);
        }
        return null;
    }
    public bool DelItemById(long unId)
    {
        IXUIListItem itemById = this.GetItemById(unId);
        return this.DelItem(itemById);
    }
    public bool DelItemByIndex(int nIndex)
    {
        IXUIListItem itemByIndex = this.GetItemByIndex(nIndex);
        return this.DelItem(itemByIndex);
    }
    public bool DelItem(IXUIListItem iUIListItem)
    {
        XUIListItem xUIListItem = iUIListItem as XUIListItem;
        if (null == xUIListItem)
        {
            return false;
        }
        this.m_listXUIListItemSelected.Remove(xUIListItem);
        int index = xUIListItem.Index;
        for (int i = index + 1; i < this.Count; i++)
        {
            this.m_listXUIListItem[i].name = string.Format("{0:0000}", i - 1);
            this.m_listXUIListItem[i].Index = i - 1;
        }
        this.m_listXUIListItem.Remove(xUIListItem);
        xUIListItem.gameObject.transform.parent = null;
        UnityEngine.Object.Destroy(xUIListItem.gameObject);
        this.Refresh();
        return true;
    }
    public void Clear()
    {
        if (this.m_listXUIListItem == null)
        {
            return;
        }
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            current.gameObject.transform.parent = null;
            UnityEngine.Object.Destroy(current.gameObject);
        }
        this.m_listXUIListItemSelected.Clear();
        this.m_listXUIListItem.Clear();
        this.Refresh();
    }
    /// <summary>
    /// 设置所有的列表元素是否可用
    /// </summary>
    /// <param name="bEnable"></param>
    public void SetEnable(bool bEnable)
    {
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            current.SetEnable(bEnable);
        }
    }
    public void SetEnableSelect(bool bEnable)
    {
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            current.SetEnableSelect(bEnable);
        }
        if (!bEnable)
        {
            this.m_listXUIListItemSelected.Clear();
        }
    }
    public void SetEnableSelect(List<long> listIds)
    {
        if (listIds == null)
        {
            return;
        }
        foreach (XUIListItem current in this.m_listXUIListItem)
        {
            if (listIds.Contains(current.Id))
            {
                current.SetEnableSelect(true);
            }
            else
            {
                current.SetEnableSelect(false);
            }
        }
        this.m_listXUIListItemSelected.RemoveAll((XUIListItem x) => !listIds.Contains(x.Id));
    }
    public override void Highlight(bool bTrue)
    {
        base.Highlight(bTrue);
        foreach (var current in this.m_listXUIListItem)
        {
            current.Highlight(bTrue);
        }
    }
    public void Highlight(List<long> listIds)
    {
        this.Highlight(false);
        if (listIds == null)
        {
            return;
        }
        foreach (var current in this.m_listXUIListItem)
        {
            if (listIds.Contains(current.Id))
            {
                current.Highlight(true);
            }
        }
    }
    /// <summary>
    /// 选中该元素
    /// </summary>
    /// <param name="listItem"></param>
    public void OnSelectItem(XUIListItem listItem)
	{
		this.SelectItem(listItem, true);
	}
    /// <summary>
    /// 选中该单项元素
    /// </summary>
    /// <param name="listItem"></param>
    /// <param name="bTrigerEvent"></param>
    public void SelectItem(XUIListItem listItem, bool bTrigerEvent)
	{
		if (null == listItem)
		{
			return;
		}
		if (this.m_listXUIListItemSelected.Contains(listItem))
		{
			return;
		}
		if (!this.m_bMultiSelect)
		{
			this.m_listXUIListItemSelected.Clear();
		}
		this.m_listXUIListItemSelected.Add(listItem);
		if (this.m_eventHandlerOnSelect != null && bTrigerEvent && this.m_eventHandlerOnSelect(listItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
		if (this.m_bWidthCenter && null != listItem)
		{
			this.MoveWidthCenter(listItem.Index);
		}
	}
    #region 注册事件
    public void RegisterListSelectEventHandler(ListSelectEventHandler eventHandler)
	{
		this.m_eventHandlerOnSelect = eventHandler;
	}
	public void RegisterListUnSelectEventHandler(ListSelectEventHandler eventHandler)
	{
		this.m_eventHandlerOnUnSelect = eventHandler;
	}
	public void RegisterListDoubleClickEventHandler(ListDoubleClickEventHandler eventHandler)
	{
		this.m_eventHandlerOnDoubleClick = eventHandler;
	}
	public void RegisterListClickEventHandler(ListClickEventHandler eventHandler)
	{
		this.m_eventHandlerOnClick = eventHandler;
	}
	public void RegisterListPressDownEventHandler(ListPressDownEventHandler eventHandler)
	{
		this.m_eventHandlerOnPressDown = eventHandler;
	}
	public void RegisterListPressUpEventHandler(ListPressUpEventHandler eventHandler)
	{
		this.m_eventHandlerOnPressUp = eventHandler;
	}
	public void RegisterListMouseOnEventHandler(ListMouseOnEventHandler eventHandler)
	{
		this.m_eventHandlerOnMouseOn = eventHandler;
	}
	public void RegisterListMouseLeaveEventHandler(ListMouseLeaveEventHandler eventHandler)
	{
		this.m_eventHandlerOnMouseLeave = eventHandler;
	}
	public void RegisterListDragEventHandler(ListDragEventHandler eventHandler)
	{
		this.m_eventHandlerOnDrag = eventHandler;
	}
	public void RegisterListDragOffEventHandler(ListDragOffEventHandler eventHandler)
	{
		this.m_eventHandlerOnDragOff = eventHandler;
	}
	public void RegisterListDragReleaseEventHandler(ListDragReleaseEventHandler eventHandler)
	{
		this.m_eventHandlerOnDragRelease = eventHandler;
	}
	public void RegisterListDropEventHandler(ListDropEventHandler eventHandler)
	{
		this.m_eventHandlerOnDrop = eventHandler;
	}
    public void RegisterListMoveReleaseEventHandler(ListMoveReleaseEventHandler eventHandler)
    {
        this.m_eventHandlerMoveRelease = eventHandler;
    }
    #endregion
    #region 事件处理
    public void _OnClick(XUIListItem uiListItem)
	{
		if (this.m_bWidthCenter && this.m_bMoveFinish && this.GetSelectedIndex() != uiListItem.Index)
		{
			this.SelectItem(uiListItem, false);
		}
		if (this.m_eventHandlerOnClick != null && this.m_eventHandlerOnClick(uiListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
    public void OnPressDown(XUIListItem uiListItem)
    {
        if (this.m_eventHandlerOnPressDown != null && this.m_eventHandlerOnPressDown(uiListItem))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    public void OnPressUp(XUIListItem uiListItem)
	{
		if (this.m_eventHandlerOnPressUp != null && this.m_eventHandlerOnPressUp(uiListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
	public void OnMouseOn(XUIListItem uiListItem)
	{
		if (this.m_eventHandlerOnMouseOn != null && this.m_eventHandlerOnMouseOn(uiListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
	public void OnMouseLeave(XUIListItem uiListItem)
	{
		if (this.m_eventHandlerOnMouseLeave != null && this.m_eventHandlerOnMouseLeave(uiListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
	public void _OnDrag(XUIListItem uiListItem, Vector2 delta)
	{
		if (this.m_eventHandlerOnDrag != null && this.m_eventHandlerOnDrag(uiListItem, delta))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
	public void _OnDrop(XUIListItem srcListItem, XUIListItem desListItem)
	{
		if (null == srcListItem || null == desListItem)
		{
			return;
		}
		if (this.m_eventHandlerOnDrop != null && this.m_eventHandlerOnDrop(srcListItem, desListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
			srcListItem.ParentXUIList._OnDragOff(srcListItem, desListItem);
		}
	}
	public void _OnDragRelease(XUIListItem srcListItem, XUIListItem desListItem)
	{
		if (this.m_eventHandlerOnDragRelease != null && this.m_eventHandlerOnDragRelease(srcListItem, desListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
	public void _OnDragOff(XUIListItem srcListItem, XUIListItem desListItem)
	{
		if (this.m_eventHandlerOnDragOff != null && this.m_eventHandlerOnDragOff(srcListItem, desListItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
	public void OnUnSelectItem(XUIListItem listItem)
	{
		this.UnSelectItem(listItem, true);
	}
	public void UnSelectItem(XUIListItem listItem, bool bTrigerEvent)
	{
		if (null == listItem)
		{
			return;
		}
		if (!this.m_listXUIListItemSelected.Contains(listItem))
		{
			return;
		}
		this.m_listXUIListItemSelected.Remove(listItem);
		if (this.m_eventHandlerOnUnSelect != null && bTrigerEvent && this.m_eventHandlerOnUnSelect(listItem))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
		if (bTrigerEvent && !this.m_bMultiSelect && this.m_listXUIListItemSelected.Count == 0 && this.m_eventHandlerOnSelect != null && this.m_eventHandlerOnSelect(null))
		{
			XUITool.Instance.IsEventProcessed = true;
		}
	}
    #endregion
    #endregion
    #region 私有方法
    /// <summary>
    /// 刷新Table中元素的大小（实际是碰撞器大小）
    /// </summary>
    private void RefreshItemSizeByTable()
    {
        foreach (var current in this.m_listXUIListItem)
        {
            if (current != null)
            {
                current.RefreshSize();
            }
        }
    }
    /// <summary>
    /// 刷新所偶列表中的单项状态，比如大小位置等
    /// </summary>
    private void RefreshAllItemStatus()
    {
        EnumMoveCenterType eMoveCenterType = this.m_eMoveCenterType;
        if (eMoveCenterType == EnumMoveCenterType.eMoveCenterType_Increas)
        {
            int index = this.GetSelectedIndex();
            //表示当前没有选择一个单项
            if (index < 0)
            {
                this.SetSelectedIndex(0);
            }
            index = 0;
            foreach (var current in this.m_listXUIListItem)
            {
                current.InitStatus(this.m_eMoveCenterType, index);
            }
        }
    }
    /// <summary>
    /// 单项移动到中心位置
    /// </summary>
    /// <param name="index"></param>
    private void MoveWidthCenter(int index)
    {
        //如果是多选择的并且网格是多行或者多列的，就返回不执行
        if (this.m_bMultiSelect && (null == this.m_uiGrid || this.m_uiGrid.maxPerLine > 0))
        {
            return;
        }
        this.m_vStartPos = base.transform.localPosition;
        this.m_vFinishPos = this.m_vStartPos;
        this.m_vFinishPos.x = -1f * this.m_uiGrid.cellWidth * (float)index;
        for (int i = 0; i < this.m_listXUIListItem.Count; i++)
        {
            this.m_listXUIListItem[i].InitMoveCenterInfo(this.m_eMoveCenterType, index);
        }
        this.m_fStartTime = Time.time;
        this.m_bMoveFinish = false;
    }

    public override void  Init()
    {
 	    base.Init();
        //先清除
        this.m_listXUIListItem.Clear();
        this.m_listXUIListItemSelected.Clear();
        //然后初始化列表
        for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			XUIListItem component = child.GetComponent<XUIListItem>();
			if (null == component)
			{
				Debug.LogError(string.Format("null == uiListItem. path ={0}", NGUITools.GetHierarchy(child.gameObject)));
			}
			else
			{
				component.parent = this;
				component.ParentDlg = this.ParentDlg;
				this.m_listXUIListItem.Add(component);
			}
		}
        //列表根据名字来排序
        this.m_listXUIListItem.Sort(new Comparison<XUIListItem>(XUIList.SortByName));
        int num = 0;
        foreach (var current in this.m_listXUIListItem)
        {
            current.name = string.Format("{0:0000}", num);//名字设置成0001，0002这样的格式
			current.Index = num;//初始索引
			current.Id = (long)num;//初始id
			if (!current.IsInited)
			{
				current.Init();//初始化单项元素
			}
            if (current.IsSelected)
            {
                this.m_listXUIListItemSelected.Add(current);//如果选中就添加到选中列表中
            }
            num++;
        }
        this.m_uiGrid = base.GetComponent<UIGrid>();
		if (null != this.m_uiGrid)
		{
			this.m_uiGrid.Reposition();
		}
		this.m_uiTable = base.GetComponent<UITable>();
		if (null != this.m_uiTable)
		{
			this.m_uiTable.Reposition();
		}
        this.RefreshAllItemStatus();
    }
    #endregion
    #region 静态方法
    /// <summary>
    /// 根据元素的名字先后顺序排序
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int SortByName(XUIListItem a,XUIListItem b)
    {
        return string.Compare(a.name,b.name);
    }
    #endregion
}
