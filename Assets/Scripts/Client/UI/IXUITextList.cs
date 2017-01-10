using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUITextList
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUITextList : IXUIObject
    {
        int OffsetLine
        {
            get;
            set;
        }
        int TotalLine
        {
            get;
        }
        int MaxShowLine
        {
            get;
        }
        void Clear();
        void Add(string text);
    }
}
