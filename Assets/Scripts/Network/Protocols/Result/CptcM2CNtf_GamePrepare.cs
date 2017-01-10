using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Common;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_GamePrepare 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.13
// 模块描述：客户端通知服务进入游戏准备状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端通知服务进入游戏准备状态1014
/// </summary>
public class CptcM2CNtf_GamePrepare : CProtocol
{
	#region 字段
    private const int m_dwPtcM2CNtf_GamePrepareId = 1014;
    public int m_dwTimeLimit;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcM2CNtf_GamePrepare()
        : base(1014)
    {
        this.m_dwTimeLimit = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwTimeLimit);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        DlgBase<DlgRoom, DlgRoomBehaviour>.singleton.SetTimeLimit(this.m_dwTimeLimit,EnumSelectStep.eSelectStep_Prepare);
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
