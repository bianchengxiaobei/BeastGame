using UnityEngine;
using System.Collections;
using Utility;
using Client.Data;
using Client.GameMain.OpState;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCNtf_AddRoleToScene
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.11
// 模块描述：服务器向客户端发送添加神兽到场景中消息
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 服务器向客户端发送添加神兽到场景中消息
    /// </summary>
    public class CPtcCNtf_AddRoleToScene : CProtocol
    {
        #region 字段
        private const uint m_dwPtcG2CNtf_AddRoleToSceneID = 1115;
        public long m_dwRoleID;
        public CVector3 m_oInitialPos;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
                public CPtcCNtf_AddRoleToScene() : base(1024)
		{
			this.m_dwRoleID = 0;
			this.m_oInitialPos = new CVector3();
		}
        #endregion
        #region 公有方法
		public override CByteStream Serialize(CByteStream bs)
		{
			bs.Write(this.m_dwRoleID);
			bs.Write(this.m_oInitialPos);
			return bs;
		}
		public override CByteStream DeSerialize(CByteStream bs)
		{
			bs.Read(ref this.m_dwRoleID);
			bs.Read(this.m_oInitialPos);
			return bs;
		}
        public override void Process()
        {
            Singleton<RoomManager>.singleton.OnAddHeroToScene(this.m_dwRoleID, this.m_oInitialPos);
            XLog.Log.Debug(string.Concat(new object[]
			{
				"CPtcG2CNtf_AddRoleToScene: ",
				this.m_oInitialPos.nRow,
				" ",
				this.m_oInitialPos.nCol
			}));
            Singleton<OpStateManager>.singleton.Refresh();
            if (this.m_dwRoleID == Singleton<BeastRole>.singleton.Id && !Singleton<BeastManager>.singleton.IsAllBeastPrepared())
            {
                string @string = StringConfigMgr.GetString("StageNotice_WaitOtherPlayerBorn");
                //DlgBase<DlgStateProgress, DlgStateProgressBehaviour>.singleton.ShowNotice(@string);
            }
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
