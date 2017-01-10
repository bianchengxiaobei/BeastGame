using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Common;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_MatchStart 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.26
// 模块描述：服务器向客户端发送开始匹配等待消息1008
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器向客户端发送开始匹配等待消息1008
/// </summary>
public class CptcM2CNtf_MatchStart : CProtocol
{
	#region 字段
    private const int m_dwPtcM2CNtf_MatchStartId = 1008;
    public int m_nReason;
    public int m_nWaitTime;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcM2CNtf_MatchStart()
        : base(1008)
    {
        this.m_nReason = 0;
        this.m_nWaitTime = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_nReason);
        bs.Read(ref this.m_nWaitTime);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_nReason);
        bs.Write(this.m_nWaitTime);
        return bs;
    }
    public override void Process()
    {
        if (this.m_nReason != 3)
        {
            if (Singleton<ClientMain>.singleton.EGameState == EnumGameState.eState_Match)
            {
                DlgBase<DlgMatchingTime, DlgMatchingTimeBehaviour>.singleton.SetVisible(true);
                DlgBase<DlgMatchingTime, DlgMatchingTimeBehaviour>.singleton.StartMatch((uint)this.m_nWaitTime);
            }
            else
            {
                Singleton<NetworkManager>.singleton.SendMatchCancel();
            }
        }
        else 
        {
            DlgBase<DlgMatchingTime, DlgMatchingTimeBehaviour>.singleton.SetVisible(true);
            DlgBase<DlgMatchingTime, DlgMatchingTimeBehaviour>.singleton.StartMatch((uint)this.m_nWaitTime);
        }
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
