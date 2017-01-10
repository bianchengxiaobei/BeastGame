using UnityEngine;
using System.Collections;
using Game;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcC2MReq_Move 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.26
// 模块描述：客户端请求服务器神兽移动消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端请求服务器神兽移动消息
/// </summary>
public class CPtcC2MReq_Move : CProtocol
{
    #region 字段
    public long beastId;
    public CVector3 desPos;
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public CPtcC2MReq_Move() : base(1028)
    {
        this.beastId = 0;
        this.desPos = new CVector3();
    }
    #endregion
    #region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.beastId);
        bs.Write(this.desPos);
        return bs;
    }
    public override void Process()
    {
        
    }
    public void SetPos(CVector3 pos)
    {
        this.desPos.CopyFrom(pos);
    }
    #endregion

}
