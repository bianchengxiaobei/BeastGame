using UnityEngine;
using System.Collections.Generic;
using Utility;
using System;
using System.Linq;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UILogicManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public abstract class UILogicManager
    {
        #region
        private HashSet<INotifyPropChanged> m_itemSources = new HashSet<INotifyPropChanged>();
        private EventController m_eventController;
        #endregion
        #region 属性
        public INotifyPropChanged ItemSource
        {
            set 
            {
                if (value != null && !this.m_itemSources.Contains(value))
                {
                    this.m_itemSources.Add(value);
                    value.SetEventController(this.m_eventController);
                    value.AddUnloadCallback(() => 
                    {
                        if (this.m_itemSources != null && this.m_itemSources.Contains(value))
                        {
                            this.m_itemSources.Remove(value);
                        }
                    });
                }
            }
        }
        #endregion
        #region 构造方法
        public UILogicManager()
        {
            this.m_eventController = new EventController();
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void SetBinding<T>(string key, Action<T> action)
        {
            if (this.m_eventController.ContainEvent(key))
            {
                return;
            }
            this.m_eventController.AddEventListener(key, action);
        }
        /// <summary>
        /// 更新UI属性值（效率不高，不要频繁使用）
        /// </summary>
        public void UpdateUI()
        {
            foreach (var itemSource in this.m_itemSources)
            {
                var type = itemSource.GetType();
                //获取带一个泛型参数回调方法的TriggerEvent
                var mTriggerEvent = this.m_eventController.GetType().GetMethods().FirstOrDefault(t => t.Name == "TriggerEvent" && t.IsGenericMethod && t.GetGenericArguments().Length == 1);
                foreach (var item in this.m_eventController.Events)
                {
                    var prop = type.GetProperty(item.Key);
                    if (null == prop)
                    {
                        continue;
                    }
                    //构造TriggerEvent方法
                    var method = mTriggerEvent.MakeGenericMethod(prop.PropertyType);
                    //获取属性值
                    var value = prop.GetGetMethod().Invoke(itemSource, null);
                    //调用TriggerEvent方法
                    method.Invoke(this.m_eventController, new object[] { item.Key, value });
                }
            }
        }
        /// <summary>
        /// 清空属性监听，但是需要再UI重新load的时候重新添加
        /// </summary>
        public virtual void Release()
        {
            foreach (var item in this.m_itemSources)
            {
                if (item != null)
                {
                    item.RemoveEventController(this.m_eventController);
                }
            }
            this.m_itemSources.Clear();
            this.m_eventController.Clear();
        }
        #endregion
        #region 私有方法
        #endregion
    }
}