using UnityEngine;
using System.Collections.Generic;
using Game;
using Client.UI;
using Client.Common;
using Client.UI.UICommon;
using Utility;
using Utility.Export;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MoveState 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.26
// 模块描述：战斗操作阶段的移动状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战斗操作阶段的移动状态
/// </summary>
namespace Client.GameMain.OpState.Stage
{
    public class MoveState : OpStateBase
    {
        #region 字段
        private CVector3 m_vecTargetPos = new CVector3();
        private List<CVector3> m_listHexs = new List<CVector3>();
        #endregion
        #region 重写方法
        public override void OnEnter()
        {
            UIManager.singleton.SetCursor(enumCursorType.eCursorType_Move);
            CVector3 pos = Singleton<BeastRole>.singleton.Beast.Pos;
            int maxMoveDis = Singleton<BeastRole>.singleton.Beast.MaxMoveDis;
            this.m_listHexs.Clear();
            Singleton<ClientMain>.singleton.scene.GetNearNodes(maxMoveDis, pos, ref this.m_listHexs,false);
            Singleton<ClientMain>.singleton.scene.GetNearNodesIgnoreHero(ref this.m_listHexs);
            Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_Highlight, this.m_listHexs, "Texture/Hexagon/Select");
        }
        public override void OnLeave()
        {
            Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_Highlight);
            Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_Path);
            this.m_vecTargetPos.CopyFrom(CVector3.MaxValue);
            this.m_listHexs.Clear();
        }
        public override void Refresh()
        {
            base.Refresh();
            this.OnLeave();
            this.OnEnter();
        }
        public override bool OnSelectPos(CVector3 vecHex)
        {
            if (!this.m_listHexs.Exists((CVector3 p) => p.Equals(vecHex)))
            {
                return false;
            }
            if (vecHex.Equals(this.m_vecTargetPos))
            {
                //相当于点击两次然后发送移动请求消息给服务器
                Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_Path);
                this.OnButtonOkClick();
                return true;
            }
            bool isBlocked = Singleton<ClientMain>.singleton.scene.IsPosBlocked(vecHex);

            if (isBlocked)
            {
                Debug.Log("Block");
                //显示提示消息，不能选择这个点
                return false;
            }
            else
            {
                this.m_vecTargetPos.CopyFrom(vecHex);
                Vector3 realPos = Hexagon.GetHex3DPos(this.m_vecTargetPos, Space.World);
                Beast beast = Singleton<BeastManager>.singleton.GetBeastById(Singleton<RoomManager>.singleton.BeastIdInRound);
                if (beast != null)
                {
                    //路径
                    List<CVector3> list = new List<CVector3>();
                    Singleton<ClientMain>.singleton.scene.FindPath(2147483647, beast.Pos, this.m_vecTargetPos, ref list, false);
                    //去除头和尾，因为不显示
                    if (list.Count > 0)
                    {
                        list.RemoveAt(0);
                        list.RemoveAt(list.Count - 1);
                    }
                    Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_Path, list);
                }
            }
            return true;
        }
        public override bool OnButtonFinishClick()
        {
            Singleton<NetworkManager>.singleton.SendFinishRoleStage(EClientRoleStage.ROLE_STAGE_MOVE);
            DlgBase<DlgMain, DlgMainBehaviour>.singleton.EnableButtonFinish(false, EClientRoleStage.ROLE_STAGE_ACTION);
            return true;
        }
        /// <summary>
        /// 发送移动请求
        /// </summary>
        /// <returns></returns>
        public override bool OnButtonOkClick()
        {
            CVector3 pos = new CVector3(this.m_vecTargetPos);
            Singleton<NetworkManager>.singleton.SendBeastMoveReq(pos);
            this.OnLeave();
            return true;
        }
        #endregion


    }
}