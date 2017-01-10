using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcM2CNtf_StartBeastRound 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.5.14
// 模块描述：游戏服务器通知客户端开始神兽进入操作阶段
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏服务器通知客户端开始神兽进入操作阶段
/// </summary>
public class CPtcM2CNtf_StartBeastRound : CProtocol
{
	#region 字段
    public long m_dwRoleId;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CPtcM2CNtf_StartBeastRound()
        : base(1025)
    {
        this.m_dwRoleId = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwRoleId);
        return bs;
    }
    public override void Process()
    {
        XLog.Log.Debug("CPtcM2CNtf_StartBeastRound");
        Singleton<RoomManager>.singleton.StartBeastRound(this.m_dwRoleId);
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
