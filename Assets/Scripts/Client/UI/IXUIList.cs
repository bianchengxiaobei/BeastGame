using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIList
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.28
// 模块描述：自定义UI列表接口
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIList : IXUIObject
    {
        #region 属性
        int Count
        {
            get;
        }
        bool EnableMultiSelect
        {
            get;
            set;
        }
        #endregion
        void Clear();
        void Refresh();
        int GetSelectedIndex();
        void SetSelectedIndex(int nIndex);
        void SetSelectedItemById(long unId);
        void SetSelectedItemById(List<long> listItemId);
        void SetSelectedItemByIndex(List<int> listItemIndex);
        IXUIListItem GetSelectedItem();
        IXUIListItem[] GetSelectedItems();
        IXUIListItem GetItemByGUID(ulong ulId);
        IXUIListItem GetItemById(long unId);
        IXUIListItem GetItemById(long unId, bool bVisible);
        IXUIListItem GetItemByIndex(int nIndex);
        IXUIListItem[] GetAllItems();
        IXUIListItem AddListItem(GameObject obj);
        IXUIListItem AddListItem();
        void SetEnable(bool bEnable);
        void SetEnableSelect(bool bEnable);
        void SetEnableSelect(List<long> listIds);
        bool DelItem(IXUIListItem iUIListItem);
        bool DelItemById(long unId);
        bool DelItemByIndex(int nIndex);
        void Highlight(List<long> listIds);
        void SetSize(float cellWidth, float cellHeight);
        void RegisterListSelectEventHandler(ListSelectEventHandler eventHandler);
        void RegisterListUnSelectEventHandler(ListSelectEventHandler eventHandler);
        void RegisterListDoubleClickEventHandler(ListDoubleClickEventHandler eventHandler);
        void RegisterListClickEventHandler(ListClickEventHandler eventHandler);
        void RegisterListPressDownEventHandler(ListPressDownEventHandler eventHandler);
        void RegisterListPressUpEventHandler(ListPressUpEventHandler eventHandler);
        void RegisterListMouseOnEventHandler(ListMouseOnEventHandler eventHandler);
        void RegisterListMouseLeaveEventHandler(ListMouseLeaveEventHandler eventHandler);
        void RegisterListDragEventHandler(ListDragEventHandler eventHandler);
        void RegisterListDragOffEventHandler(ListDragOffEventHandler eventHandler);
        void RegisterListDragReleaseEventHandler(ListDragReleaseEventHandler eventHandler);
        void RegisterListDropEventHandler(ListDropEventHandler eventHandler);
        void RegisterListMoveReleaseEventHandler(ListMoveReleaseEventHandler eventHandler);
    }
}
