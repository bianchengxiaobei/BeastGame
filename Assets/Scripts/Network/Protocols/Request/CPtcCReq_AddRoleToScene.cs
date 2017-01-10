using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCReq_AddRoleToScene
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：客户端向服务器发送选择神兽到出生点请求
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端向服务器发送选择神兽到出生点请求
/// </summary>
namespace Game
{
    public class CPtcCReq_AddRoleToScene : CProtocol
    {
        #region 字段
        private const int m_dwPtcC2GReq_AddRoleToSceneID = 1114;
        private static CPtcCReq_AddRoleToScene m_sInstance = new CPtcCReq_AddRoleToScene();
        public long m_unRoleId;
        public CVector3 m_oInitPos;
        #endregion
        #region 构造方法
        public CPtcCReq_AddRoleToScene()
            : base(1023)
        {
            this.m_unRoleId = 0;
            this.m_oInitPos = new CVector3();
        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_unRoleId);
            bs.Write(this.m_oInitPos);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_unRoleId);
            bs.Read(this.m_oInitPos);
            return bs;
        }
        public override void Process()
        {
        }
        public static CPtcCReq_AddRoleToScene GetSendInstance()
        {
            CPtcCReq_AddRoleToScene.m_sInstance.Reset();
            return CPtcCReq_AddRoleToScene.m_sInstance;
        }
        public void Reset()
        {
            this.m_unRoleId = 0;
            this.m_oInitPos.Reset();
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
