using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIInput
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIInput : IXUIObject
    {
        bool IsSelected
        {
            get;
            set;
        }
        bool IsEnable();
        void SetEnable(bool bEnable);
        string GetText();
        void SetText(string strText);
        void RegisterSubmitEventHandler(InputSubmitEventHandler eventHandler);
        void RegisterChangeEventHandler(InputChangeEventHandler eventHandler);
    }
}
