using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIObject
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIObject
    {
        #region 属性
        /// <summary>
        /// 提示信息
        /// </summary>
        object TipParam
        {
            get;
            set;
        }
        /// <summary>
        /// UIObject
        /// </summary>
        GameObject CachedGameObject
        {
            get;
        }
        /// <summary>
        /// UITransform
        /// </summary>
        Transform CachedTransform
        {
            get;
        }
        /// <summary>
        /// UI父亲节点Object
        /// </summary>
        IXUIObject parent
        {
            get;
            set;
        }
        /// <summary>
        /// UI父亲节点界面
        /// </summary>
        IXUIDlg ParentDlg
        {
            get;
            set;
        }
        Vector2 RealSize
        {
            get;
        }
        Vector2 RelativeSize
        {
            get;
        }
        string Tip
        {
            get;
            set;
        }
        Bounds AbsoluteBounds
        {
            get;
        }
        Bounds RelativeBounds
        {
            get;
        }
        bool IsError
        {
            get;
        }
        float Alpha
        {
            get;
            set;
        }
        bool IsEnableOpen
        {
            get;
            set;
        }
        #endregion
        IXUIObject GetUIObject(string strPath);
        bool IsVisible();
        void SetVisible(bool bVisible);
        void OnFocus();
        void OnResolutionChange();
        void Highlight(bool bTrue);
        void RegisterMouseOnEventHandler(MouseOnEventHandler eventHandler);
        void RegisterMouseLeaveEventHandler(MouseLeaveEventHandler eventHandler);
        void RegisterPressUpEventHandler(PressUpEventHandler eventHandler);
        void RegisterPressDownEventHandler(PressDownEventHandler eventHandler);
        void RegisterClickEventHandler(ClickEventHandler eventHandler);
        void RegisterLostFocusEventHandler(LostFocusEventHandler eventHandler);
        void RegisterGetFocusEventHandler(GetFocusEventHandler eventHandler);
    }
}