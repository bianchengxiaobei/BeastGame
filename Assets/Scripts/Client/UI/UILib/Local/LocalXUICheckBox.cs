using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalXUICheckBox
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUICheckBox : LocalXUIObject,IXUICheckBox,IXUIObject
    {
        #region 字段
        private static LocalXUICheckBox m_instance = new LocalXUICheckBox();
        private bool m_bIsChecked;
        #endregion
        #region 属性
        public bool bChecked
        {
            get { return this.m_bIsChecked; }
            set { this.m_bIsChecked = value; }
        }     
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public static LocalXUICheckBox GetInstance()
        {
            return LocalXUICheckBox.m_instance;
        }
        public void RegisterOnCheckEventHandler(CheckBoxOnCheckEventHandler eventHandler)
        { }
        public void SetEnable(bool bEnable) { }
        #endregion
        #region 私有方法
        #endregion
    }
}