using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIButton
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIButton : IXUIObject
    {
        void SetCaption(string strText);
        bool IsEnable();
        void SetEnable(bool bEnable);
        /// <summary>
        /// 注册按钮事件监听
        /// </summary>
        /// <param name="eventHandler"></param>
        void RegisterClickEventHandler(ButtonClickEventHandler eventHandler);
    }
}