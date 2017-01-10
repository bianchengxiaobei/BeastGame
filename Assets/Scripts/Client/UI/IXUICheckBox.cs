using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUICheckBox
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUICheckBox : IXUIObject
    {
        bool bChecked
        {
            get;
            set;
        }
        void RegisterOnCheckEventHandler(CheckBoxOnCheckEventHandler eventHandler);
        void SetEnable(bool bEnable);
    }
}