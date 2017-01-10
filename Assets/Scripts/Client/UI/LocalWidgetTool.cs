using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalWidgetTool
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon.Local
{
    internal class LocalWidgetTool
    {
        /// <summary>
        /// 找到所以在这个UI界面下的UI组件
        /// </summary>
        /// <param name="trans">界面的transform</param>
        /// <param name="parent">界面父类节点</param>
        /// <param name="dicAllUIObject">所有界面里的ui组件</param>
        public static void FindAllUIObjects(Transform trans, IXUIObject parent, ref Dictionary<string, XUIObjectBase> dicAllUIObject)
        {
            int i = 0;
            while (i < trans.childCount)
            {
                Transform child = trans.GetChild(i);
                XUIObjectBase component = child.GetComponent<XUIObjectBase>();
                if (component == null)
                {
                    goto IL_67;
                }
                //如果不是ListItem就加到dicAllUIObject里面
                if (component.GetType().GetInterface("IXUIListItem") == null)
                {
                    if (dicAllUIObject.ContainsKey(component.name))
                    {
                        Debug.LogError("m_dicId2UIObject.ContainsKey:" + LocalWidgetTool.GetUIObjectId(component));
                    }
                    dicAllUIObject[component.name] = component;
                    component.parent = parent;
                    goto IL_67;
                }
                IL_6F:
                    i++;
                    continue;
                IL_67:
                    LocalWidgetTool.FindAllUIObjects(child, parent,ref dicAllUIObject);
                    goto IL_6F;
            }
        }
        /// <summary>
        /// 获取UI组件的id
        /// </summary>
        /// <param name="iXUIObject"></param>
        /// <returns></returns>
        public static string GetUIObjectId(IXUIObject iXUIObject)
        {
            if (iXUIObject == null)
            {
                return string.Empty;
            }
            string text = iXUIObject.CachedGameObject.name;
            IXUIListItem iXUIListItem = iXUIObject as IXUIListItem;
            if (iXUIListItem != null)
            {
                text = iXUIListItem.Id.ToString();
            }
            while (iXUIObject.parent != null)
            {
                iXUIObject = iXUIObject.parent;
                string arg = iXUIObject.CachedGameObject.name;
                iXUIListItem = (iXUIObject as IXUIListItem);
                if (iXUIListItem != null)
                {
                    arg = iXUIListItem.Id.ToString();
                }
                text = string.Format("{0}#{1}", arg, text);
            }
            return text;
        }
    }
}
