using UnityEngine;
using System.Collections;
using Game;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcC2MReq_EndRoleStage 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.28
// 模块描述：客户端发送结束神兽的本战斗阶段
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端发送结束神兽的本战斗阶段
/// </summary>
public class CptcC2MReq_EndRoleStage : CProtocol
{
    #region 字段
    public long beastId;
    public int stage;
    #endregion
    #region 构造方法
    public CptcC2MReq_EndRoleStage() : base(1030)
    {
        this.beastId = 0;
        this.stage = 0;
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
        bs.Write(this.stage);
        return bs;
    }
    public override void Process()
    {
        
    }
    #endregion

}
