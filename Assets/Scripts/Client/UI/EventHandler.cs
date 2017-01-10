using System;
using UnityEngine;
using UnityAssetEx.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EventHandler
// 创建者：chen
// 修改者列表：
// 创建日期：2015.9.6
// 模块描述：所有委托类型
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public delegate bool MouseOnEventHandler(IXUIObject iXUIObject);
    public delegate bool MouseLeaveEventHandler(IXUIObject iXUIObject);
    public delegate bool PressUpEventHandler(IXUIObject iXUIObject);
    public delegate bool PressDownEventHandler(IXUIObject iXUIObject);
    public delegate bool ClickEventHandler(IXUIObject iXUIObject);
    public delegate bool LostFocusEventHandler(IXUIObject iXUIObject);
    public delegate bool GetFocusEventHandler(IXUIObject iXUIObject);
    public delegate void AnimFinishedEventHandler();
    public delegate IAssetRequest LoadTextureAsynEventHandler(string url, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
    public delegate void TipShowEventHandler(bool bShow, IXUIObject uiObject);
    public delegate string TipGetterEventHandler(string strUIObjecId);
    public delegate void AddListItemEventHandler(IXUIDlg uiDlg, IXUIList uiList, IXUIListItem uiListItem);

    public delegate bool FilterUIEventHandler(IXUIObject iXUIObject, string funcName);

    public delegate bool ListSelectEventHandler(IXUIListItem uiListItem);
    public delegate bool ListDoubleClickEventHandler(IXUIListItem uiListItem);
    public delegate bool ListClickEventHandler(IXUIListItem uiListItem);
    public delegate bool ListPressDownEventHandler(IXUIListItem uiListItem);
    public delegate bool ListPressUpEventHandler(IXUIListItem uiListItem);
    public delegate bool ListMouseOnEventHandler(IXUIListItem uiListItem);
    public delegate bool ListMouseLeaveEventHandler(IXUIListItem uiListItem);
    public delegate bool ListDragEventHandler(IXUIListItem uiListItem, Vector2 delta);
    public delegate bool ListDragOffEventHandler(IXUIListItem srcListItem, IXUIListItem desListItem);
    public delegate bool ListDragReleaseEventHandler(IXUIListItem srcListItem, IXUIListItem desListItem);
    public delegate bool ListDropEventHandler(IXUIListItem srcListItem, IXUIListItem desListItem);
    public delegate void ListMoveReleaseEventHandler();
    /// <summary>
    /// 按钮事件委托类型
    /// </summary>
    /// <param name="iXUIButton"></param>
    /// <returns></returns>
    public delegate bool ButtonClickEventHandler(IXUIButton iXUIButton);
    /// <summary>
    /// 选择框事件委托类型
    /// </summary>
    /// <param name="iXUICheckBox"></param>
    /// <returns></returns>
    public delegate bool CheckBoxOnCheckEventHandler(IXUICheckBox iXUICheckBox);

    public delegate bool InputSubmitEventHandler(IXUIInput uiInput);
    public delegate bool InputChangeEventHandler(IXUIInput uiInput);


    public delegate bool PopupListSelectEventHanler(IXUIPopupList iXUIPopupList);

    public delegate bool ScrollBarChangeEventHandler(IXUIScrollBar iXUIScrollBar);
    public delegate bool ScrollBarDragFinishedEventHandler();

    public delegate bool SliderValueChangeEventHandler(IXUISlider uiSlider);
}