using UnityEngine;
using System.Collections.Generic;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EventDispatch
// 创建者：chen
// 修改者列表：
// 创建日期：2015.11.10
// 模块描述：事件的注册与分发
//----------------------------------------------------------------*/
#endregion
namespace Utility
{
    public class EventDispatch
    {
        #region 字段
        private static EventController m_eventController = new EventController();
        #endregion
        #region 属性
        /// <summary>
        /// 所有已经注册的事件
        /// </summary>
        public static Dictionary<string, Delegate> Events
        {
            get { return EventDispatch.m_eventController.Events; }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        #region 添加事件
        public static void AddEventListener(string eventType, Action handler)
        {
            m_eventController.AddEventListener(eventType, handler);
        }
        public static void AddEventListener<T>(string eventType, Action<T> handler)
        {
            m_eventController.AddEventListener(eventType, handler);
        }
        public static void AddEventListener<T,U>(string eventType, Action<T,U> handler)
        {
            m_eventController.AddEventListener(eventType, handler);
        }
        public static void AddEventListener<T,U,V>(string eventType, Action<T,U,V> handler)
        {
            m_eventController.AddEventListener(eventType, handler);
        }
        public static void AddEventListener<T,U,V,W>(string eventType, Action<T,U,V,W> handler)
        {
            m_eventController.AddEventListener(eventType, handler);
        }
        #endregion
        #region 移除事件
        public static void RemoveEventListener(string eventType, Action handler)
        {
            m_eventController.RemoveEventListener(eventType, handler);
        }
        public static void RemoveEventListener<T>(string eventType, Action<T> handler)
        {
            m_eventController.RemoveEventListener(eventType, handler);
        }
        public static void RemoveEventListener<T,U>(string eventType, Action<T,U> handler)
        {
            m_eventController.RemoveEventListener(eventType, handler);
        }
        public static void RemoveEventListener<T,U,V>(string eventType, Action<T,U,V> handler)
        {
            m_eventController.RemoveEventListener(eventType, handler);
        }
        public static void RemoveEventListener<T,U,V,W>(string eventType, Action<T,U,V,W> handler)
        {
            m_eventController.RemoveEventListener(eventType, handler);
        }
        #endregion
        #region 触发事件
        public static void TriggerEvent(string eventType)
        {
            m_eventController.TriggerEvent(eventType);
        }
        public static void TriggerEvent<T>(string eventType,T arg1)
        {
            m_eventController.TriggerEvent(eventType,arg1);
        }
        public static void TriggerEvent<T,U>(string eventType, T arg1,U arg2)
        {
            m_eventController.TriggerEvent(eventType, arg1,arg2);
        }
        public static void TriggerEvent<T, U,V>(string eventType, T arg1, U arg2,V arg3)
        {
            m_eventController.TriggerEvent(eventType, arg1, arg2,arg3);
        }
        public static void TriggerEvent<T, U,V,W>(string eventType, T arg1, U arg2,V arg3, W arg4)
        {
            m_eventController.TriggerEvent(eventType, arg1, arg2,arg3,arg4);
        }
        #endregion 
        public static void MarkEventToPermanent(string eventType)
        {
            m_eventController.MarkEventToPermanent(eventType);
        }
        public static void Clear()
        {
            m_eventController.Clear();
        }
        #endregion
        #region 私有方法
        #endregion
    }
    public class EventController
    {
        private Dictionary<string, Delegate> m_events = new Dictionary<string, Delegate>();
        private List<string> m_permanentEvents = new List<string>();
        public Dictionary<string, Delegate> Events
        {
            get { return this.m_events; }
        }
        #region 添加事件
        /// <summary>
        /// 添加单个事件委托
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler">委托</param>
        public void AddEventListener(string eventType, Action handler)
        {
            OnListenerAdding(eventType, handler);
            this.m_events[eventType] = (Action)this.m_events[eventType]+handler;
        }
        public void AddEventListener<T>(string eventType, Action<T> handler)
        {
            OnListenerAdding(eventType, handler);
            this.m_events[eventType] = (Action<T>)this.m_events[eventType] + handler;
        }
        public void AddEventListener<T, U>(string eventType, Action<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            this.m_events[eventType] = (Action<T,U>)this.m_events[eventType]+handler;
        }
        public void AddEventListener<T, U, V>(string eventType, Action<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            this.m_events[eventType] = (Action<T, U, V>)this.m_events[eventType] + handler;
        }
        public void AddEventListener<T, U, V,W>(string eventType, Action<T, U, V,W> handler)
        {
            OnListenerAdding(eventType, handler);
            this.m_events[eventType] = (Action<T, U, V,W>)this.m_events[eventType] + handler;
        }
        #endregion
        #region 移除事件
        /// <summary>
        /// 移除单个事件委托
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler">委托</param>
        public void RemoveEventListener(string eventType, Action handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                this.m_events[eventType] = (Action)this.m_events[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        public void RemoveEventListener<T>(string eventType, Action<T> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                this.m_events[eventType] = (Action<T>)this.m_events[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        public void RemoveEventListener<T,U>(string eventType, Action<T,U> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                this.m_events[eventType] = (Action<T,U>)this.m_events[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        public void RemoveEventListener<T,U,V>(string eventType, Action<T,U,V> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                this.m_events[eventType] = (Action<T,U,V>)this.m_events[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        public void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                this.m_events[eventType] = (Action<T, U, V, W>)this.m_events[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        #endregion 
        #region 触发事件
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventType"></param>
        public void TriggerEvent(string eventType)
        {
            Delegate myEvent;
            if (!this.m_events.TryGetValue(eventType, out myEvent))
            {
                return;
            }
            Delegate[] handlers = myEvent.GetInvocationList();
            for (int i = 0; i < handlers.Length; i++)
            {
                Action handler = handlers[i] as Action;
                if (null == handler)
                {
                    throw new Exception("触发事件出现错误，这种事件类型委托为空！");
                }
                try
                {
                    handler();
                }
                catch(Exception e) 
                {
                    Debug.LogException(e);
                }
            }
        }
        /// <summary>
        /// 触发事件（带一个参数）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventType"></param>
        /// <param name="arg1">参数T</param>
        public void TriggerEvent<T>(string eventType, T arg1)
        {
            Delegate myEvent;
            if (!this.m_events.TryGetValue(eventType, out myEvent))
            {
                return;
            }
            Delegate[] handlers = myEvent.GetInvocationList();
            for (int i = 0; i < handlers.Length; i++)
            {
                Action<T> handler = handlers[i] as Action<T>;
                if (null == handler)
                {
                    throw new Exception("触发事件出现错误，这种事件类型委托为空！");
                }
                try
                {
                    handler(arg1);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        public void TriggerEvent<T,U>(string eventType, T arg1, U arg2)
        {
            Delegate myEvent;
            if (!this.m_events.TryGetValue(eventType, out myEvent))
            {
                return;
            }
            Delegate[] handlers = myEvent.GetInvocationList();
            for (int i = 0; i < handlers.Length; i++)
            {
                Action<T,U> handler = handlers[i] as Action<T,U>;
                if (null == handler)
                {
                    throw new Exception("触发事件出现错误，这种事件类型委托为空！");
                }
                try
                {
                    handler(arg1,arg2);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        public void TriggerEvent<T,U,V>(string eventType, T arg1, U arg2, V arg3)
        {
            Delegate myEvent;
            if (!this.m_events.TryGetValue(eventType, out myEvent))
            {
                return;
            }
            Delegate[] handlers = myEvent.GetInvocationList();
            for (int i = 0; i < handlers.Length; i++)
            {
                Action<T,U,V> handler = handlers[i] as Action<T,U,V>;
                if (null == handler)
                {
                    throw new Exception("触发事件出现错误，这种事件类型委托为空！");
                }
                try
                {
                    handler(arg1,arg2,arg3);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        public void TriggerEvent<T,U,V,W>(string eventType, T arg1,U arg2, V arg3, W arg4)
        {
            Delegate myEvent;
            if (!this.m_events.TryGetValue(eventType, out myEvent))
            {
                return;
            }
            Delegate[] handlers = myEvent.GetInvocationList();
            for (int i = 0; i < handlers.Length; i++)
            {
                Action<T,U,V,W> handler = handlers[i] as Action<T,U,V,W>;
                if (null == handler)
                {
                    throw new Exception("触发事件出现错误，这种事件类型委托为空！");
                }
                try
                {
                    handler(arg1,arg2,arg3,arg4);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        #endregion
        /// <summary>
        /// 标记该事件为永久事件
        /// </summary>
        /// <param name="eventType"></param>
        public void MarkEventToPermanent(string eventType)
        {
            this.m_permanentEvents.Add(eventType);
        }
        /// <summary>
        /// 清除非永久性的事件
        /// </summary>
        public void Clear()
        {
            List<string> eventToRemove = new List<string>();
            foreach (var pair in this.m_events)
            {
                bool wasFound = false;
                foreach (var pEvent in this.m_permanentEvents)
                {
                    if (pair.Key == pEvent)
                    {
                        wasFound = true;
                        break;
                    }
                }
                if (!wasFound)
                {
                    eventToRemove.Add(pair.Key);
                }
            }
            foreach (string Event in eventToRemove)
            {
                this.m_events.Remove(Event);
            }
        }
        /// <summary>
        /// 是否已经注册此事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool ContainEvent(string eventType)
        {
            return this.m_events.ContainsKey(eventType);
        }
        /// <summary>
        /// 添加事件，如果这个事件类型已经存在了，就添加委托
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        private void OnListenerAdding(string eventType, Delegate callback)
        {
            if (!this.m_events.ContainsKey(eventType))
            {
                this.m_events.Add(eventType, null);
            }
            Delegate myEvent = this.m_events[eventType];
            if (myEvent != null && myEvent.GetType() != callback.GetType())
            {
                throw new Exception("注册事件的类型和原先的不一样:"+myEvent.GetType().Name+" And "+callback.GetType());
            }
        }
        /// <summary>
        /// 移除这个事件，如果事件的委托还存在，就不移除
        /// </summary>
        /// <param name="eventType"></param>
        private void OnListenerRemoved(string eventType)
        {
            if (this.m_events.ContainsKey(eventType) && this.m_events[eventType] == null)
            {
                this.m_events.Remove(eventType);
            }
        }
        /// <summary>
        /// 移除事件前的检测，如果不存在或类型不符合，就false
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        private bool OnListenerRemoving(string eventType, Delegate handler)
        {
            if (!this.m_events.ContainsKey(eventType))
            {
                return false;
            }
            Delegate myEvent = this.m_events[eventType];
            if (myEvent != null && myEvent.GetType() != handler.GetType())
            {
                throw new Exception("移除事件失败，事件类型不匹配");
            }
            else 
            {
                return true;
            }
        }
    }
}

