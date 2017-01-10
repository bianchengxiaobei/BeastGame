using UnityEngine;
using System.Collections.Generic;
using Game;
using Utility;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcM2CNtf_Move 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.27
// 模块描述：服务器通知神兽移动消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器通知神兽移动消息
/// </summary>
public class CPtcM2CNtf_Move : CProtocol
{
    #region 字段
    public long beastId;
    public List<CVector3> listPath;
    #endregion
    #region 构造方法
    public CPtcM2CNtf_Move() : base(1029)
    {
        this.beastId = 0;
        this.listPath = new List<CVector3>();
    }
    #endregion
    #region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        bs.Read(ref this.beastId);
        int num = 0;
        bs.Read(ref num);
        this.listPath.Clear();
        for (int i = 0; i < num; i++)
        {
            CVector3 pos = new CVector3();
            bs.Read(pos);
            this.listPath.Add(pos);
        }
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
    public override void Process()
    {
        XLog.Log.Debug("CPtcM2CNtf_Move");
        Singleton<BeastManager>.singleton.MoveBeast(this.beastId, this.listPath);
    }
    #endregion
}
