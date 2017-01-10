using UnityEngine;
using System.Collections;
using System;
using Utility;
using Client.Data;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcM2CNtf_SceneLoaded 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.18
// 模块描述：服务器发送给客户端该玩家加载完成
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器发送给客户端该玩家加载完成
/// </summary>
namespace Game
{
    public class CptcM2CNtf_SceneLoaded : CProtocol
    {
        #region 字段
        public long m_dwPlayerId;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public CptcM2CNtf_SceneLoaded():base(1018)
        {
            this.m_dwPlayerId = 0L;
        }
        #endregion
        #region 公共方法
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_dwPlayerId);
            return bs;
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_dwPlayerId);
            return bs;
        }
        public override void Process()
        {
            PlayerData player = Singleton<RoomManager>.singleton.GetPlayerDataById(this.m_dwPlayerId);
            if (player != null)
            {
                player.IsLoadFinish = true;
            }
            DlgBase<DlgLoading, DlgLoadingBehaviour>.singleton.RefreshMask();
        }
        #endregion
    }
}