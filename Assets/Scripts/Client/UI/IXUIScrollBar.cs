using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIScrollBar
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIScrollBar : IXUIObject
    {
        float value
        {
            get;
            set;
        }
        float size
        {
            get;
            set;
        }
        void RegisterScrollBarChangeEventHandler(ScrollBarChangeEventHandler eventHandler);
        void RegisterScrollBarDragFinishedEventHandler(ScrollBarDragFinishedEventHandler eventHandler);
    }
}

