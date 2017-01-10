using UnityEngine;
using System.Collections;
using Game;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCReq_SelectR 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.12
// 模块描述：客户端发送选择角色请求1016
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端发送选择角色请求消息1016
/// </summary>
public class CptcCReq_SelectRole : CProtocol
{
    #region 字段
    private const uint m_dwPtcReq_SelectRole = 1016;
    private static CptcCReq_SelectRole sendInstance = new CptcCReq_SelectRole();
    public long m_lRoleId;
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public CptcCReq_SelectRole() : base(1016)
    {
        m_lRoleId = 0L;
    }
    #endregion
    #region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_lRoleId);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.m_lRoleId);
        return bs;
    }
    public override void Process()
    {
        
    }
    #endregion
    #region 私有方法
    #endregion
    #region 析构方法
    #endregion
}
