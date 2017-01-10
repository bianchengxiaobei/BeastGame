using UnityEngine;
using System.Collections;
using Game;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCNtf_SelectRoleResult 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.12
// 模块描述：服务器发送给客户端选择角色结果1017
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器发送给客户端选择角色结果1017
/// </summary>
public class CptcCNtf_SelectRoleResult : CProtocol
{
    #region 字段
    private const uint m_dwCptcNtf_SelectRoleResultId = 1017;
    public int m_iErrorCode = -1;
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public CptcCNtf_SelectRoleResult() : base(1017)
    {
        this.m_iErrorCode = -1;
    }
    #endregion
    #region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.m_iErrorCode);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    { 
        bs.Write(this.m_iErrorCode);
        return bs;
    }
    public override void Process()
    {
        if (this.m_iErrorCode == 0)
        {
            //选择该角色没有错误,服务器会继续发送CPtcCNtf_EnterLobby消息，初始化角色所有的信息，如果角色处在剧情中，就进入到故事状态中

        }
        else
        {

        }
    }
    #endregion
    #region 私有方法
    #endregion
    #region 析构方法
    #endregion
}
