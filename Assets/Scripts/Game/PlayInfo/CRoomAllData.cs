using UnityEngine;
using System.Collections.Generic;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CRoomAllData 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.20
// 模块描述：房间内所有玩家的数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 房间内所有玩家的数据
/// </summary>
public class CRoomAllData : IData
{
	#region 属性
    public long m_dwRoomId;
    public int m_dwMapId;
    public byte m_btGameType;
    public byte m_btEmpireHp;
    public byte m_btLeagueHp;
    public List<CRoomMemberData> m_oEmpireMemberList;
    public List<CRoomMemberData> m_oLeagueMemberList;
    public List<CRoleData> m_oRoleList;
	#endregion
	#region 构造方法
    public CRoomAllData()
    {
        this.m_oEmpireMemberList = new List<CRoomMemberData>();
        this.m_oLeagueMemberList = new List<CRoomMemberData>();
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
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
