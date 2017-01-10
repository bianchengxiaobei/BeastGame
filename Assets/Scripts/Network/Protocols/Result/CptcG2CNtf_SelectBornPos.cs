using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcG2CNtf_SelectBornPos 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.16
// 模块描述：服务器发送给客户端选择神兽出生点的消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器发送给客户端选择神兽出生点的消息
/// </summary>
public class CptcG2CNtf_SelectBornPos : CProtocol
{
	#region 字段
    public long m_dwRoleId;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcG2CNtf_SelectBornPos()
        : base(1022)
    {
        this.m_dwRoleId = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwRoleId);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        XLog.Log.Debug("CptcG2CNtf_SelectBornPos:BeastId:" + this.m_dwRoleId);
        //开始该角色选择出生点
        Singleton<RoomManager>.singleton.StartPlayerSelectBornPos(this.m_dwRoleId);
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
