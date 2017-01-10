using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIProgress
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIProgress : IXUIObject
    {
        float value
        {
            get;
            set;
        }
        Color Color
        {
            get;
            set;
        }
    }
}
