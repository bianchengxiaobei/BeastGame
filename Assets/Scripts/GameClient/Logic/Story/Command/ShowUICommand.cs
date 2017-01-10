using UnityEngine;
using System.Collections;
using System;
using Client.UI;
using ScriptableData;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ShowUICommand 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.13
// 模块描述：是否显示UI命令
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 是否显示UI命令
/// </summary>
internal class ShowUICommand : AbstractStoryCommand
{
    private IStoryValue<string> m_oEnable = new StoryValue<string>();
    public override IStoryCommand Clone()
    {
        ShowUICommand cmd = new ShowUICommand();
        cmd.m_oEnable = this.m_oEnable.Clone();
        return cmd;
    }
    protected override void UpdateVariables(StoryInstance instance)
    {
        this.m_oEnable.Evaluate(instance);
    }
    protected override void UpdateArguments(object iterator, object[] args)
    {
        this.m_oEnable.Evaluate(iterator, args);
    }
    protected override void Load(CallData callData)
    {
        int num = callData.GetParamNum();
        if (num > 0)
        {
            m_oEnable.InitFromDsl(callData.GetParam(0));
        }
    }
    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
        if (m_oEnable.Value != "false")
        {
            //显示UI
            UIManager.singleton.ShowAllDlg();
        }
        else
        {
            //关闭所有UI
            UIManager.singleton.CloseAllDlg();
        }
        return false;
    }
}
