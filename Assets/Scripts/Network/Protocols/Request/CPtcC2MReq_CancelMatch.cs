using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcC2MReq_CancelMatch 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.26
// 模块描述：客户端向游戏服务器发送取消匹配的请求
/----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端向游戏服务器发送取消匹配的请求1007
/// </summary>
public class CPtcC2MReq_CancelMatch : CProtocol
{
    private const uint m_dwPtcC2MReq_CancelMatchID = 1007;
    private static CPtcC2MReq_CancelMatch sm_oSendInstance = new CPtcC2MReq_CancelMatch();
    public CPtcC2MReq_CancelMatch()
        : base(1007)
    {
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
    }
    public static CPtcC2MReq_CancelMatch GetSendInstance()
    {
        CPtcC2MReq_CancelMatch.sm_oSendInstance.Reset();
        return CPtcC2MReq_CancelMatch.sm_oSendInstance;
    }
    public void Reset()
    {
    }
}
