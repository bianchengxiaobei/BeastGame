using UnityEngine;
using System.Collections.Generic;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcC2MReq_TrySelectBeast 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.30
// 模块描述：客户端请求服务器试图选择该神兽
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端请求服务器试图选择该神兽1010
/// </summary>
public class CptcC2MReq_TrySelectBeast : CProtocol
{
	#region 字段
    private const int m_dwPtcC2MReq_TrySelectBeastId = 1010;
    private static CptcC2MReq_TrySelectBeast m_instance = new CptcC2MReq_TrySelectBeast();
    public long m_dwBeastId;
    public int m_dwBeastTypeId;
    public int m_dwSuitId;
	#endregion
	#region 构造方法
    public CptcC2MReq_TrySelectBeast()
        : base(1010)
    {
        this.m_dwBeastId = 0;
        this.m_dwBeastTypeId = 0;
        this.m_dwSuitId = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwBeastId);
        bs.Read(ref this.m_dwBeastTypeId);
        bs.Read(ref this.m_dwSuitId);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_dwBeastId);
        bs.Write(this.m_dwBeastTypeId);
        bs.Write(this.m_dwSuitId);
        return bs;
    }
    public void Reset()
    {
        this.m_dwBeastId = 0;
        this.m_dwBeastTypeId = 0;
        this.m_dwSuitId = 0;
    }
    public static CptcC2MReq_TrySelectBeast GetInstance()
    {
        CptcC2MReq_TrySelectBeast.m_instance.Reset();
        return CptcC2MReq_TrySelectBeast.m_instance;
    }
    public override void Process()
    {
        
    }
	#endregion
}
