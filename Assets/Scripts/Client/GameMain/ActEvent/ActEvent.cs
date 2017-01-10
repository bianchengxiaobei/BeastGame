using UnityEngine;
using System.Collections;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActEvent 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.21
// 模块描述：事件基类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 事件基类
/// </summary>
namespace Client.GameMain
{
    public class ActEvent : IDisposable
    {
        protected float m_fDurationTime = 0f;
        protected float m_fStartTime = 0f;
        protected bool m_bStarted = false;
        public float DurationTime
        {
            get
            {
                return this.m_fDurationTime;
            }
            set
            {
                this.m_fDurationTime = value;
            }
        }
        public Action StartCallBack
        {
            get;
            set;
        }
        public Action FinishCallBack
        {
            get;
            set;
        }
        /// <summary>
        /// 开始该动作
        /// </summary>
        public void Start()
        {
            if (!this.m_bStarted)
            {
                this.m_fStartTime = Time.time;
                this.m_bStarted = true;
                try
                {
                    this.Trigger();
                }
                catch (Exception ex)
                {
                    XLog.GetLog<ActEvent>().Fatal(ex.ToString());
                }
            }
        }
        public virtual void Trigger()
        {
            if (null != this.StartCallBack)
            {
                Action startCallBack = this.StartCallBack;
                this.StartCallBack = null;
                startCallBack();
            }
        }
        public bool IsOver()
        {
            bool flag = Time.time - this.m_fStartTime > this.m_fDurationTime;
            if (flag && null != this.FinishCallBack)
            {
                Action finishCallBack = this.FinishCallBack;
                this.FinishCallBack = null;
                finishCallBack();
            }
            return flag;
        }
        ~ActEvent()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}

