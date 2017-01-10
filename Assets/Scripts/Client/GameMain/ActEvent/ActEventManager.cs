using UnityEngine;
using System.Collections.Generic;
using System;
using Utility;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActEventManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.21
// 模块描述：事件管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 事件管理器
/// </summary>
namespace Client.GameMain
{
    public class ActEventManager : Singleton<ActEventManager>
    {
        private Queue<ActEvent> m_queueActEvent = new Queue<ActEvent>();//事件动作队列
        private List<Queue<ActEvent>> m_listQueueOld = new List<Queue<ActEvent>>();
        private bool m_bStarted = false;
        private Action m_eventHandlerOnActFinish = null;
        private IXLog m_log = XLog.GetLog<ActEventManager>();
        public void Start()
        {
            this.m_bStarted = true;
        }
        /// <summary>
        /// 添加动作事件
        /// </summary>
        /// <param name="actEvent"></param>
        public void AddEvent(ActEvent actEvent)
        {
            if (null != actEvent)
            {
                if (this.m_queueActEvent.Count == 0)
                {
                    actEvent.Start();
                }
                this.m_queueActEvent.Enqueue(actEvent);
            }
        }
        /// <summary>
        /// 注册完成该动作之后的委托
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RegisterActFinishEventHandle(Action eventHandler)
        {
            if (null != eventHandler)
            {
                this.m_eventHandlerOnActFinish = (Action)Delegate.Combine(this.m_eventHandlerOnActFinish, eventHandler);
            }
        }
        public void NewQueue()
        {
            if (this.m_queueActEvent.Count > 0)
            {
                this.m_listQueueOld.Add(this.m_queueActEvent);
                this.m_queueActEvent = new Queue<ActEvent>();
            }
        }
        public void Update()
        {
            try
            {
                if (this.m_bStarted)
                {
                    for (int i = this.m_listQueueOld.Count - 1; i >= 0; i--)
                    {
                        Queue<ActEvent> queue = this.m_listQueueOld[i];
                        if (queue == null || queue.Count == 0)
                        {
                            this.m_listQueueOld.RemoveAt(i);
                        }
                        else
                        {
                            this.UpdateQueue(queue);
                        }
                    }
                    this.UpdateQueue(this.m_queueActEvent);
                    if (null != this.m_eventHandlerOnActFinish)
                    {
                        if (this.m_queueActEvent.Count == 0)
                        {
                            Action eventHandlerOnActFinish = this.m_eventHandlerOnActFinish;
                            this.m_eventHandlerOnActFinish = null;
                            eventHandlerOnActFinish();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        public void Reset()
        {
            foreach (ActEvent current in this.m_queueActEvent)
            {
                current.Dispose();
            }
            this.m_queueActEvent.Clear();
            for (int i = 0; i < this.m_listQueueOld.Count; i++)
            {
                Queue<ActEvent> queue = this.m_listQueueOld[i];
                foreach (ActEvent current in queue)
                {
                    current.Dispose();
                }
            }
            this.m_listQueueOld.Clear();
            this.m_eventHandlerOnActFinish = null;
        }
        private void UpdateQueue(Queue<ActEvent> queueActEvent)
        {
            if (queueActEvent != null && queueActEvent.Count != 0)
            {
                ActEvent actEvent = queueActEvent.Peek();
                if (null != actEvent)
                {
                    if (actEvent.IsOver())
                    {
                        actEvent = queueActEvent.Dequeue();
                        actEvent.Dispose();
                        if (queueActEvent.Count > 0)
                        {
                            actEvent = queueActEvent.Peek();
                            if (null != actEvent)
                            {
                                actEvent.Start();
                            }
                        }
                    }
                }
            }
        }
    }
}