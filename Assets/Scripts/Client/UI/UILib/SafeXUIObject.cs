using UnityEngine;
using Client.UI.UICommon;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SafeXUIObject
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Export
{
    public class SafeXUIObject
    {
        private IXUIObject m_uiObject;
        private static Dictionary<IXUIObject, SafeXUIObject> s_dicAllSafeXUIObject = new Dictionary<IXUIObject, SafeXUIObject>();
        public IXUIObject UIObject
        {
            get
            {
                return this.m_uiObject;
            }
            private set
            {
                this.m_uiObject = value;
            }
        }
        private SafeXUIObject(IXUIObject uiObject)
        {
            this.m_uiObject = uiObject;
        }
        public static SafeXUIObject GetSafeXUIObject(IXUIObject uiObject)
        {
            if (uiObject == null)
            {
                return null;
            }
            SafeXUIObject safeXUIObject = null;
            if (SafeXUIObject.s_dicAllSafeXUIObject.TryGetValue(uiObject, out safeXUIObject))
            {
                return safeXUIObject;
            }
            safeXUIObject = new SafeXUIObject(uiObject);
            SafeXUIObject.s_dicAllSafeXUIObject.Add(uiObject, safeXUIObject);
            return safeXUIObject;
        }
        public static void OnDestoryXUIObject(IXUIObject uiObject)
        {
            SafeXUIObject safeXUIObject = null;
            if (SafeXUIObject.s_dicAllSafeXUIObject.TryGetValue(uiObject, out safeXUIObject))
            {
                safeXUIObject.UIObject = null;
                SafeXUIObject.s_dicAllSafeXUIObject.Remove(uiObject);
            }
        }
    }
}