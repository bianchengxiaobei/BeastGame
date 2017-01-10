using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIPopupList
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIPopupList : IXUIObject
    {
        short SelectedIndex
        {
            get;
            set;
        }
        string Selection
        {
            get;
            set;
        }
        bool AddItem(string strItem);
        bool AddItem(string strItem, object data);
        void Clear();
        object GetDataByIndex(int index);
        void RegisterPopupListSelectEventHandler(PopupListSelectEventHanler eventHandler);
    }
}
