using UnityEngine;
using System.Collections.Generic;
using Game;
using Client.Data;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_TrySelectBeast 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.30
// 模块描述：服务器向客户端方发送试图选择该神兽的回应消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器向客户端方发送试图选择该神兽的回应消息1012
/// </summary>
public class CptcM2CNtf_TrySelectBeast : CProtocol
{
	#region 字段
    private const int m_dwPtcM2CNtf_TrySelectBeastId = 1012;
    public long m_dwBeastId;
    public int m_dwBeastTypeId;
    public int m_dwLevel;
    public List<int> m_oSkillList;
    public byte m_btIsRandom;
    public int m_dwSuitId;
	#endregion
	#region 构造方法
    public CptcM2CNtf_TrySelectBeast()
        : base(1012)
    {
        this.m_dwBeastId = 0;
        this.m_dwBeastTypeId = 0;
        this.m_oSkillList = new List<int>();
        this.m_btIsRandom = 0;
        this.m_dwSuitId = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwBeastId);
        bs.Read(ref this.m_dwBeastTypeId);
        bs.Read(ref this.m_dwLevel);
        bs.Read(ref this.m_dwSuitId);
        bs.Read(ref this.m_btIsRandom);
        bs.Read(this.m_oSkillList);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_dwBeastId);
        bs.Write(this.m_dwBeastTypeId);
        bs.Write(this.m_dwSuitId);
        bs.Write(this.m_btIsRandom);
        bs.Write(this.m_oSkillList);
        return bs;
    }
    public override void Process()
    {
        XLog.Log.Debug("CptcM2CNtf_TrySelectBeast");
        XLog.Log.Debug("TrySelect_BeastId:" + this.m_dwBeastId);
        XLog.Log.Debug("TrySelect_BeastypeTId:" + this.m_dwBeastTypeId);
        //在RoomManager里面Try选择神兽
        Singleton<RoomManager>.singleton.OnPlayerSelectBeast(this.m_dwBeastId, this.m_dwBeastTypeId, this.m_dwLevel, ref this.m_oSkillList, this.m_btIsRandom, this.m_dwSuitId);
    }
	#endregion
}
