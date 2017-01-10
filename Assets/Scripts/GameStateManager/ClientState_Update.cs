using UnityEngine;
using System.Collections;
using Utility.Export;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_Update
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：更新状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 更新状态
/// </summary>
public class ClientState_Update : ClientState_Base
{
	#region 字段
    private IXLog m_log = XLog.GetLog<ClientState_Update>();
	#endregion
	#region 属性
	#endregion
	#region 公有方法
    /// <summary>
    /// 检查更新，然后进入ClientMain.Init();
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        this.m_log.Debug("ClientState_Update.OnEnter()");
        Singleton<ClientMain>.singleton.RegisterCallBackOnChangedFinished(delegate 
        {
            Singleton<ClientMain>.singleton.Init();
        });
    }
    public override void OnLeave()
    {
        base.OnLeave();
        //停止更新，设置更新界面不显示
    }
	#endregion
	#region 私有方法
	#endregion
}
