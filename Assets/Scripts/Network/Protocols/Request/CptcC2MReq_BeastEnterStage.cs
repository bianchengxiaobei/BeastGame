using UnityEngine;
using System.Collections;
using Game;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcC2MReq_BeastEnterStage 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.26
// 模块描述：客户端发送神兽进入该战斗阶段
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端发送神兽进入该战斗阶段
/// </summary>
public class CptcC2MReq_BeastEnterStage : CProtocol
{
    #region 字段
    public long beastId;
    public int stage;
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public CptcC2MReq_BeastEnterStage() : base(1026)
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
    #region 私有方法
    #endregion
    #region 析构方法
    #endregion
}
