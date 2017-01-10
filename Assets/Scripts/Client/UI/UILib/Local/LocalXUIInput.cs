using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：localXUIInout
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUIInput : LocalXUIObject,IXUIInput,IXUIObject
    {
        private static LocalXUIInput m_instance = new LocalXUIInput();
        private bool m_bIsSelected;
        public bool IsSelected 
        {
            get { return this.m_bIsSelected; }
            set { this.m_bIsSelected = value; }
        }
        public static LocalXUIInput GetInstance()
        {
            return LocalXUIInput.m_instance;
        }
        public bool IsEnable()
        {
            return true;
        }
        public void SetEnable(bool A)
        {
        }
        public string GetText()
        {
            return string.Empty;
        }
        public void SetText(string A)
        {
        }
        public void RegisterSubmitEventHandler(InputSubmitEventHandler A)
        {
        }
        public void RegisterChangeEventHandler(InputChangeEventHandler A)
        {
        }
    }
}
