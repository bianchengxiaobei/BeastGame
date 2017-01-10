using UnityEngine;
using System.Collections;
using Game;
using Utility.Export;
using Utility;
using Client.Logic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCNtf_EnterLobby
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：收到进入大厅消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 收到进入大厅消息
/// </summary>
public class CPtcCNtf_EnterLobby : CProtocol
{
	#region 字段
    private const int m_ptcM2CNtf_EnterLobbyID = 1005;
    private IXLog m_log = XLog.GetLog<CPtcCNtf_EnterLobby>();
    //角色id
    public long m_accountID;
    //角色所有的信息
    public CRoleAllInfo m_roleAllInfo;
   // public byte m_UnFinishedFlag;
	#endregion
	#region 构造方法
    public CPtcCNtf_EnterLobby()
        : base(1005)
    {
        this.m_accountID = 0L;
        this.m_roleAllInfo = new CRoleAllInfo();
    }
	#endregion
	#region 公有方法

    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_accountID);
        bs.Write(this.m_roleAllInfo);
        return bs;
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_accountID);
        bs.Read(this.m_roleAllInfo);
        return bs;
    }
    public override void Process()
    {
        this.m_log.Debug("收到进入游戏大厅消息！");
        Singleton<Login>.singleton.OnLoginSuccess(this);
    }
	#endregion
}
