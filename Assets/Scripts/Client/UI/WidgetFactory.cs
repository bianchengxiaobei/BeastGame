using UnityEngine;
using System;
using System.Collections.Generic;
using Client.UI.UICommon;
using Client.UI.UICommon.Local;
using UILib.Local;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：WidgetFactory
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：ui组件工厂
//----------------------------------------------------------------*/
#endregion
namespace UILib.Export
{
    public class WidgetFactory
    {
        private static Dictionary<Type, IXUIObject> s_dicAllErrorWidget;
        static WidgetFactory()
        {
            WidgetFactory.s_dicAllErrorWidget = new Dictionary<Type, IXUIObject>();
            WidgetFactory.s_dicAllErrorWidget.Add(typeof(IXUIButton), LocalXUIButton.GetInstance());
            WidgetFactory.s_dicAllErrorWidget.Add(typeof(IXUILabel), LocalXUILabel.GetInstance());
            WidgetFactory.s_dicAllErrorWidget.Add(typeof(IXUIInput), LocalXUIInput.GetInstance());
            WidgetFactory.s_dicAllErrorWidget.Add(typeof(IXUISprite), LocalXUISprite.GetInstance());
        }
        public static T CreateWidget<T>() where T : class,IXUIObject
        {
            IXUIObject iXUIObject = null;
            WidgetFactory.s_dicAllErrorWidget.TryGetValue(typeof(T), out iXUIObject);
            return iXUIObject as T;
        }
        /// <summary>
        /// 找到所以在这个UI界面下的UI组件
        /// </summary>
        /// <param name="trans">界面的transform</param>
        /// <param name="parent">界面父类节点</param>
        /// <param name="dicAllUIObject">所有界面里的ui组件</param>
        public static void FindAllUIObjects(Transform trans, IXUIObject parent, ref Dictionary<string, XUIObjectBase> dicAllUIObject)
        {
            LocalWidgetTool.FindAllUIObjects(trans, parent, ref dicAllUIObject);
        }
        public static string GetUIObjectId(IXUIObject uiObject)
        {
            return LocalWidgetTool.GetUIObjectId(uiObject);
        }
    }
}
