using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Data;
using Client.GameMain;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_ChangeHp 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.20
// 模块描述：服务器发送给客户端神兽血量变化
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器发送给客户端神兽血量变化
/// </summary>
public class CptcM2CNtf_ChangeHp : CProtocol
{
	#region 字段
    public long m_dwRoleId;
    public int m_btHp;
    public byte reason;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcM2CNtf_ChangeHp()
        : base(1020)
    {
        this.m_dwRoleId = 0;
        this.m_btHp = 0;
    }
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_dwRoleId);
        bs.Read(ref this.m_btHp);
        bs.Read(ref this.reason);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        Debug.Log("Hp Init");
        Beast beast = Singleton<BeastManager>.singleton.GetBeastById(this.m_dwRoleId);
        if (beast != null)
        {
            int hp = beast.Hp;
            int hpChange = this.m_btHp - hp;
            if (!Singleton<RoomManager>.singleton.ProcessHpChangedAsync(this.m_dwRoleId, (byte)this.m_btHp))
            {
                Singleton<BeastManager>.singleton.OnBeastHpChange(this.m_dwRoleId, this.m_btHp);
            }
            /*HpChangeEvent hpChangeEvent = new HpChangeEvent();
            hpChangeEvent.DurationTime = 0.5f;
            hpChangeEvent.BeastId = this.m_dwRoleId;
            hpChangeEvent.HpChange = hpChange;
            Singleton<ActEventManager>.singleton.AddEvent(hpChangeEvent);
            */
        }
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
