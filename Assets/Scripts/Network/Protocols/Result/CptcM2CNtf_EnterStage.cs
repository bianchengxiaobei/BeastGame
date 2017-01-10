using UnityEngine;
using System.Collections.Generic;
using Game;
using System;
using Utility;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_EnterStage 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.26
// 模块描述：服务器通知客户端神兽进入该战斗阶段
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器通知客户端神兽进入该战斗阶段
/// </summary>
public class CptcM2CNtf_EnterStage : CProtocol
{
    #region 字段
    public long beastId;
    public int stage;
  
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public CptcM2CNtf_EnterStage() : base(1027)
    {
        this.beastId = 0;
        this.stage = 0;
    }
    #endregion
    #region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.beastId);
        bs.Read(ref this.stage);
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        XLog.Log.Debug("CptcM2CNtf_EnterStage");
        ICollection<Beast> allBeasts = Singleton<BeastManager>.singleton.GetAllBeasts();
        foreach (var beast in allBeasts)
        {
            if (beast.Id == this.beastId && this.beastId == Singleton<RoomManager>.singleton.BeastIdInRound) 
            {
                Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(beast.Id, (EClientRoleStage)this.stage, 0u,0u,EQueryTimeType.NORMAL_QUERY_TIME);

            }
            else
            {
                Singleton<BeastManager>.singleton.OnBeastEnterRoleStage(beast.Id, EClientRoleStage.ROLE_STAGE_WAIT, 0u);
            }
        }
    }
    #endregion
    #region 私有方法
    #endregion
    #region 析构方法
    #endregion
}
