using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCNtf_CreateRole
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：创建角色请求
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 创建角色请求1004
/// </summary>
public class CptcCReq_CreateRole : CProtocol
{
	#region 字段
    private const uint m_dwPtcC2MReq_CreateRoleID = 1004;
    private static CptcCReq_CreateRole sendInstance = new CptcCReq_CreateRole();
    public string m_roleName;
    public string m_roleIcon;
    public int m_roleIndex;
    public byte m_sex;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcCReq_CreateRole()
        : base(1004)
    {
        this.m_roleName = "";
        this.m_roleIcon = "";
        this.m_roleIndex = 0;
        this.m_sex = 0;
    }
	#endregion
	#region 公有方法
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_roleName);
        bs.Write(this.m_roleIcon);
        bs.Write(this.m_sex);
        bs.Write(this.m_roleIndex);
        return bs;
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_roleName);
        bs.Read(ref this.m_roleIcon);
        bs.Read(ref this.m_sex);
        bs.Read(ref this.m_roleIndex);
        return bs;
    }
    public override void Process()
    {
        
    }
    public static CptcCReq_CreateRole GetSendInstance()
    {
        CptcCReq_CreateRole.sendInstance.Reset();
        return CptcCReq_CreateRole.sendInstance;
    }
	#endregion
	#region 私有方法
    private void Reset()
    {
        this.m_roleName = "";
        this.m_roleIcon = "";
        this.m_sex = 0;
    }
	#endregion
}
