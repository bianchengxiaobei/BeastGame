using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：TaskMainManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：主线任务管理器
//----------------------------------------------------------------*/
#endregion
public class TaskMainManager : IDynamicData
{
	#region 字段
    private static readonly TaskMainManager instance;
    private List<TaskMain> mainList = new List<TaskMain>();//主线任务列表

	#endregion
	#region 属性
    public static TaskMainManager Instance 
    {
        get 
        {
            return TaskMainManager.instance;
        }
    }
    /// <summary>
    /// 主线任务列表
    /// </summary>
    public List<TaskMain> MainTaskList 
    {
        get 
        {
            return this.mainList;
        }
    }
    /// <summary>
    /// 主线任务数量
    /// </summary>
    public int Count 
    {
        get 
        {
            return this.mainList.Count;
        }
    }
	#endregion
	#region 构造方法
    static TaskMainManager()
    {
        TaskMainManager.instance = new TaskMainManager();
    }
	#endregion
	#region 公有方法
    /// <summary>
    /// 清除所有主线任务
    /// </summary>
    public void Clear()
    {
        this.mainList.Clear();
    }
    public void Serialize(IDynamicPacket packet)
    {
        packet.Write<TaskMain>(this.mainList);
    }
    public void Deserialize(IDynamicPacket packet)
    {
 
    }
    /// <summary>
    /// 根据任务id获取主线任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TaskMain GetDataByID(int id)
    {
        TaskMain result;
        foreach (var task in this.mainList)
        {
            if (task.ID == id)
            {
                result = task;
                return result;
            }
        }
        result = null;
        return result;
    }
	#endregion
	#region 私有方法
	#endregion
}
