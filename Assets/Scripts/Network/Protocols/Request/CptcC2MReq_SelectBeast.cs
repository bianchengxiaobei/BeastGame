using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcC2MReq_SelectBeast 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.30
// 模块描述：客户端向服务器发送确认选择神兽的请求消息1011
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端向服务器发送确认选择神兽的请求消息1011
/// </summary>
public class CptcC2MReq_SelectBeast : CProtocol
{
	#region 字段
    private const int m_dwPtcC2MReq_SelectBeastId = 1011;
    private static CptcC2MReq_SelectBeast instance = new CptcC2MReq_SelectBeast();
    public long m_dwBeastId;
	#endregion
	#region 构造方法
    public CptcC2MReq_SelectBeast()
        : base(1011)
    {
        this.m_dwBeastId = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwBeastId);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_dwBeastId);
        return bs;
    }
    public override void Process()
    {
        
    }
    public static CptcC2MReq_SelectBeast GetInstance()
    {
        return CptcC2MReq_SelectBeast.instance;
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
