using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCReq_AutoMatch
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：自动匹配请求
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端请求开始匹配1006
/// </summary>
public class CptcCReq_AutoMatch : CProtocol
{
    private const uint m_dwPtcC2MReq_AutoMatchID = 1006;
    private static CptcCReq_AutoMatch instance = new CptcCReq_AutoMatch();
    public int m_uMapID;
    public byte m_btMatchType;
    public byte m_btAIDifficulty;

    public CptcCReq_AutoMatch()
        : base(1006)
    {
        this.m_uMapID = 0;
        this.m_btMatchType = 0;
        this.m_btAIDifficulty = 0;
    }
    public static CptcCReq_AutoMatch GetInstance()
    {
        CptcCReq_AutoMatch.instance.Reset();
        return CptcCReq_AutoMatch.instance;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_uMapID);
        bs.Write(this.m_btMatchType);
        bs.Write(this.m_btAIDifficulty);
        return bs;
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_uMapID);
        bs.Read(ref this.m_btMatchType);
        bs.Read(ref this.m_btAIDifficulty);
        return bs;
    }
    public override void Process()
    {
        
    }
    public void Reset()
    {
        this.m_uMapID = 0;
        this.m_btMatchType = 0;
        this.m_btAIDifficulty = 0;
    }
}
