using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalXUIButton
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUIButton :LocalXUIObject,IXUIButton,IXUIObject
    {
        #region 字段
        private static LocalXUIButton m_instance = new LocalXUIButton();
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public static LocalXUIButton GetInstance()
        {
            return LocalXUIButton.m_instance;
        }
        public void SetCaption(string A)
        {
        }
        public bool IsEnable()
        {
            return true;
        }
        public void SetEnable(bool A)
        {
        }
        public void RegisterClickEventHandler(ButtonClickEventHandler A)
        {
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
