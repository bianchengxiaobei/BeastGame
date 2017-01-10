using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCNtf_StartGame
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.9
// 模块描述：客户端向服务器发送开始游戏消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 客户端向服务器发送加载完成消息
/// </summary>
namespace Game
{
    public class CptcCReq_EnterScene : CProtocol
    {
        #region 字段
        private const uint m_dwPtcC2GNtf_EnterSceneID = 1017;
        private static CptcCReq_EnterScene instance = new CptcCReq_EnterScene();
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public CptcCReq_EnterScene()
            : base(1017)
        {

        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            return bs;
        }
        public override void Process()
        {
            
        }
        public static CptcCReq_EnterScene GetInstance()
        {
            CptcCReq_EnterScene.instance.Reset();
            return CptcCReq_EnterScene.instance;
        }
        public void Reset()
        {
 
        }
        #endregion
        #region 私有方法
        #endregion
    }
}