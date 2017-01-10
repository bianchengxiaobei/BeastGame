using UnityEngine;
using System.Collections;
using Game;
using System;
using Utility;
using Client.UI.UICommon;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_AutoMatchResult 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.26
// 模块描述：游戏服务器向客户端发送匹配失败的结果
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏服务器向客户端发送匹配失败的结果1007
/// </summary>
public class CptcM2CNtf_AutoMatchResult : CProtocol
{
	#region 字段
    private const int m_dwPtcM2CNtf_AutoMatchResultID = 1007;
    private int m_nErrorCode;
    private int m_nPunishLeftTime;
    public int m_nPunishedPlayerId;
	#endregion
	#region 构造方法
    public CptcM2CNtf_AutoMatchResult()
        : base(1007)
    { 

    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        if (this.m_nErrorCode != 0)
        {
            string strText = string.Empty;
            if (this.m_nErrorCode == 829 || this.m_nErrorCode == 833)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, this.m_nPunishLeftTime);
                string playerName = string.Empty;
                if (this.m_nPunishedPlayerId != Singleton<PlayerRole>.singleton.ID)
                {
                    //取得该惩罚队友的名字，从TeamManager里面
                }
                //然后惩罚界面开始惩罚计时
            }
            else 
            {
                //其他错误处理
            }
            DlgBase<DlgMatch, DlgMatchBehaviour>.singleton.ClickMatch = false;
            if (this.m_nErrorCode == 63) //&&时间计时界面还没有显示
            {
                //发送取消匹配的请求
                Singleton<NetworkManager>.singleton.SendMatchCancel();
            }
        }
        
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
