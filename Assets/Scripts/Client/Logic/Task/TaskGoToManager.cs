using UnityEngine;
using System.Collections.Generic;
using Utility;
using System;
using Client.UI.UICommon;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：TaskGoToManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：任务管理器
//----------------------------------------------------------------*/
#endregion
public class TaskGoToManager : Singleton<TaskGoToManager>
{
	#region 字段
    private Dictionary<int, TaskGoToBase> m_dicTaskGoToInfo = new Dictionary<int, TaskGoToBase>();//所有任务
    private List<TaskGoToBase> m_listTaskGoToBase = new List<TaskGoToBase>();
	#endregion
	#region 公有方法
    /// <summary>
    /// 初始化任务，从角色信息那边取的
    /// </summary>
    public void Init()
    {
        //先取得角色所有的任务列表
        CTaskListInfo oTaskListInfo = Singleton<PlayerRole>.singleton.RoleAllInfo.m_oTaskListInfo;
        if (oTaskListInfo != null)
        {
            //如果任务列表的主线任务不为空的话，就添加到管理器中
            if (oTaskListInfo.m_MainList != null)
            {
                foreach (var current in oTaskListInfo.m_MainList)
                {
                    this.AddTaskGoTo((int)current.m_id);
                }
            }
        }
    }
    /// <summary>
    /// 添加任务到dic中
    /// </summary>
    /// <param name="nTaskId"></param>
    public void AddTaskGoTo(int nTaskId)
    {
        if (!this.m_dicTaskGoToInfo.ContainsKey(nTaskId))
        {
            TaskGoToBase taskGoToBase = null;
            TaskMain dataById = TaskMainManager.Instance.GetDataByID(nTaskId);
            if (dataById != null)
            {
                taskGoToBase = this.AddTaskGoTo(dataById.GoTo);
            }
            if (taskGoToBase != null)
            {
                this.m_dicTaskGoToInfo.Add(nTaskId, taskGoToBase);
            }
        }
    }
    /// <summary>
    /// 移除该任务
    /// </summary>
    /// <param name="nTaskId"></param>
    /// <returns></returns>
    public bool RemoveTaskGoTo(int nTaskId)
    {
        bool result;
        if (this.m_dicTaskGoToInfo.ContainsKey(nTaskId))
        {
            this.m_dicTaskGoToInfo[nTaskId].Finish();
            this.m_dicTaskGoToInfo.Remove(nTaskId);
            result = true;
        }
        else 
        {
            result =  false;
        }
        return result;
    }
    /// <summary>
    /// 开始该任务
    /// </summary>
    /// <param name="nTaskId"></param>
    public void GoTo(int nTaskId)
    {
        TaskGoToBase taskGoToBase = null;
        this.m_dicTaskGoToInfo.TryGetValue(nTaskId, out taskGoToBase);
        if (taskGoToBase != null)
        {
            taskGoToBase.Start();
        }
    }
    /// <summary>
    /// 创建任务，添加到List列表中
    /// </summary>
    /// <param name="strGoTo"></param>
    /// <returns></returns>
    public TaskGoToBase CreateGoToEvent(string strGoTo)
    {
        TaskGoToBase taskGoToBase = this.AddTaskGoTo(strGoTo);
        if (taskGoToBase != null)
        {
            this.m_listTaskGoToBase.Add(taskGoToBase);
        }
        return taskGoToBase;
    }
    /// <summary>
    /// 显示所有任务的UI特效
    /// </summary>
    /// <param name="dlg"></param>
    public void OnDlgShow(IXUIDlg dlg)
    {
        foreach (var current in this.m_dicTaskGoToInfo)
        {
            current.Value.OnDlgShow(dlg);
        }
        foreach (var current2 in this.m_listTaskGoToBase)
        {
            if (current2 != null)
            {
                current2.OnDlgShow(dlg);
            }
        }
    }
    /// <summary>
    /// 每个任务都执行Update
    /// </summary>
    public void Update()
    {
        foreach (var current in this.m_dicTaskGoToInfo)
        {
            current.Value.Update();
        }
        for (int i = this.m_listTaskGoToBase.Count - 1; i >= 0; i++)
        {
            TaskGoToBase taskGoToBase = this.m_listTaskGoToBase[i];
            if (taskGoToBase != null)
            {
                taskGoToBase.Update();
                //如果任务已经完成了，就移除该任务
                if (taskGoToBase.IsFinish)
                {
                    this.m_listTaskGoToBase.Remove(taskGoToBase);
                }
            }
        }
    }
	#endregion
	#region 私有方法
    private TaskGoToBase AddTaskGoTo(string strGoTo)
    {
        TaskGoToBase taskGoToBase = null;
        TaskGoToBase result;
        if (string.IsNullOrEmpty(strGoTo))
        {
            result = taskGoToBase;//也就是为空
        }
        else 
        {
            try
            {
                int num = strGoTo.IndexOf(';');
                bool flag = false;
                if (num < 0)
                {
                    num = 1;
                    flag = true;
                }
                switch (Convert.ToInt32(strGoTo.Substring(0, num)))
                {
                    case 1:
                        break;
                }
                if (taskGoToBase != null && !flag)
                {
                    string strGoToMsg = strGoTo.Substring(num + 1);
                    taskGoToBase.Init(strGoToMsg);
                }
            }
            catch (Exception e)
            {
                XLog.Log.Error(e.ToString());
            }
            result = taskGoToBase;
        }
        return result;
    }
	#endregion
}
