using UnityEngine;
using System.Collections;
using System.IO;
using Game;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCNtf_CreateRole
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.28
// 模块描述：收到创建角色的消息
//----------------------------------------------------------------*/
#endregion
public class CptcCNtf_CreateRoleResult : CProtocol
{
	#region 字段
    private const int m_ptcM2CNtf_CreateRoleResultID = 2021;
    public int m_ErrCode;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    public CptcCNtf_CreateRoleResult()
        : base(2021)
    {
        this.m_ErrCode = 0;
    }
	#endregion
	#region 公有方法
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
        if (this.m_ErrCode != 0)
        {
            //说明创建角色出现错误，显示错误消息

            DlgBase<DlgCreateRole, DlgCreateRoleBehaviour>.singleton.IsClickCreateRole = false;
        }
    }
	#endregion
	#region 私有方法
	#endregion
}
