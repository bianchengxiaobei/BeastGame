using UnityEngine;
using System.Collections.Generic;
using System;
using ScriptableData;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SendMessageCommand 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.13
// 模块描述：Gameobject之间传递消息命令
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// Gameobject之间传递消息命令
/// </summary>
internal class SendMessageCommand : AbstractStoryCommand
{
    private IStoryValue<string> m_sObjNames = new StoryValue<string>();
    private IStoryValue<string> m_sMsg = new StoryValue<string>();
    private List<IStoryValue<object>> m_oArgs = new List<IStoryValue<object>>();
    public override IStoryCommand Clone()
    {
        SendMessageCommand cmd = new SendMessageCommand();
        cmd.m_sObjNames = this.m_sObjNames;
        cmd.m_sMsg = this.m_sMsg;
        foreach (var arg in m_oArgs)
        {
            cmd.m_oArgs.Add(arg);
        }
        return cmd;
    }
    protected override void UpdateVariables(StoryInstance instance)
    {
        m_sObjNames.Evaluate(instance);
        m_sMsg.Evaluate(instance);
        foreach (var arg in this.m_oArgs)
        {
            arg.Evaluate(instance);
        }
    }
    protected override void UpdateArguments(object iterator, object[] args)
    {
        m_sObjNames.Evaluate(iterator, args);
        m_sMsg.Evaluate(iterator, args);
        foreach (var arg in this.m_oArgs)
        {
            arg.Evaluate(iterator, args);
        }
    }
    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
        string objName = m_sObjNames.Value;
        string msg = m_sMsg.Value;
        List<object> argslist = new List<object>();
        foreach (var arg in this.m_oArgs)
        {
            argslist.Add(arg.Value);
        }
        object[] args = argslist.ToArray();
        if (args.Length == 1)
        {
            StoryManager.singleton.SendMessage(objName, msg, args[0]);
        }
        else
        {
            StoryManager.singleton.SendMessage(objName, msg, args);
        }
        return false;
    }
    protected override void Load(CallData callData)
    {
        int num = callData.GetParamNum();
        //第一个参数是GameObject的名字,第二个是GameObject对应的脚本的方法名字
        if (num > 1)
        {
            m_sObjNames.InitFromDsl(callData.GetParam(0));
            m_sMsg.InitFromDsl(callData.GetParam(1));
        }
        for (int i = 2; i < callData.GetParamNum(); i++)
        {
            StoryValue value = new StoryValue();
            value.InitFromDsl(callData.GetParam(i));
            m_oArgs.Add(value);
        }
    }
}
