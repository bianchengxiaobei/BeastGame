using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IMouseOrTouch
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IMouseOrTouch
    {
        Vector2 Pos
        {
            get;
        }
        GameObject Current
        {
            get;
        }
        GameObject Pressed
        {
            get;
        }
    }
}

