using UnityEngine;
using System.Collections.Generic;
using Game;
using Utility;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_SelectBeast 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.30
// 模块描述：服务器向客户端发送确认选择神兽的结果反馈消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器向客户端发送确认选择神兽的结果反馈消息1013
/// </summary>
public class CptcM2CNtf_SelectBeast : CProtocol
{
	#region 字段
    private const int m_dwPtcM2CNtf_SelectBeastId = 1013;
    public long m_dwBeastId;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcM2CNtf_SelectBeast() 
        : base(1013)
    {
        this.m_dwBeastId = 0;
    }
	#endregion
	#region 公共方法
    public override void Process()
    {
        XLog.Log.Debug("ConfirmSelectBeast:" + this.m_dwBeastId);
        Singleton<RoomManager>.singleton.RecvConfirmBeast(this.m_dwBeastId);
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwBeastId);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
