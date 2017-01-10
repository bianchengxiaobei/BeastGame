using UnityEngine;
using System.Collections.Generic;
using Utility;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：NotifyPropChanged
// 创建者：chen
// 修改者列表：
// 创建日期：2015.10.2
// 模块描述：Entity属性改变通知UI
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public abstract class NotifyPropChanged
    {
        #region 字段
        /// <summary>
        /// 所有改变属性后UI改变的事件集合（无重复）
        /// </summary>
        private HashSet<EventController> m_uiBindingSet = new HashSet<EventController>();
        private Action m_onUnload;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        /// <summary>
        /// 实体属性改变后ui变化处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void OnPropertyChanged<T>(string propertyName, T value)
        {
            foreach (var uiEvent in this.m_uiBindingSet)
            {
                if (uiEvent != null)
                {
                    uiEvent.TriggerEvent(propertyName, value);
                }
            }
        }
        /// <summary>
        /// 添加UI绑定事件
        /// </summary>
        /// <param name="controller"></param>
        public void SetEventController(EventController controller)
        {
            this.m_uiBindingSet.Add(controller);
        }
        /// <summary>
        /// 移除UI事件绑定
        /// </summary>
        /// <param name="controller"></param>
        public void RemoveEventController(EventController controller)
        {
            this.m_uiBindingSet.Remove(controller);
        }
        /// <summary>
        /// 监听实体资源释放回调
        /// </summary>
        /// <param name="unload"></param>
        public void AddUnloadCallback(Action unload)
        {
            this.m_onUnload = unload;
        }
        /// <summary>
        /// 清除绑定数据，并执行资源释放回调委托
        /// </summary>
        protected void ClearBinding()
        {
            if (this.m_onUnload != null)
            {
                this.m_onUnload();
            }
            this.m_uiBindingSet.Clear();
        }
        #endregion
        #region 私有方法
        #endregion
    }
    public interface INotifyPropChanged
    {
        /// <summary>
        /// 设置事件控制器。
        /// </summary>
        /// <param name="controller"></param>
        void SetEventController(EventController controller);

        /// <summary>
        /// 移除事件控制器。
        /// </summary>
        /// <param name="controller"></param>
        void RemoveEventController(EventController controller);

        /// <summary>
        /// 在更改属性值时发生。
        /// </summary>
        /// <typeparam name="T">属性类型。</typeparam>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="value">属性值。</param>
        void OnPropertyChanged<T>(string propertyName, T value);

        /// <summary>
        /// 监听实体资源释放回调。
        /// </summary>
        /// <param name="onUnload">回调事件处理</param>
        void AddUnloadCallback(Action onUnload);
    }
}