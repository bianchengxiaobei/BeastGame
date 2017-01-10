using UnityEngine;
using System.Collections.Generic;
using Game;
using Utility;
using Client.Common;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SelectBornPosState
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：选择出生点阶段状态
//----------------------------------------------------------------*/
#endregion
namespace Client.GameMain.OpState.Stage
{
    /// <summary>
    /// 选择出生点阶段状态
    /// </summary>
    public class SelectBornPosState : OpStateBase
    {
        private List<CVector3> m_listTargetHexs = new List<CVector3>();
        private List<int> m_listEffectIds = new List<int>();
        public override void OnEnter()
        {
            base.OnEnter();
            EMapNodeType eMapNodeType;
            if (Singleton<BeastRole>.singleton.CampType == Common.ECampType.CAMP_EMPIRE)
            {
                eMapNodeType = EMapNodeType.MAP_NODE_REBORN_EMPIRE;
            }
            else 
            {
                eMapNodeType = EMapNodeType.MAP_NODE_REBORN_LEAGUE;
            }
            this.m_listTargetHexs.Clear();
            this.m_listTargetHexs = Singleton<ClientMain>.singleton.scene.GetNodesByType(eMapNodeType);
            Singleton<ClientMain>.singleton.scene.GetNearNodesIgnoreHero(ref this.m_listTargetHexs);
            Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_Highlight, this.m_listTargetHexs);
            foreach (var cvet in this.m_listTargetHexs)
            {
                Vector3 hex3DPos = Hexagon.GetHex3DPos(cvet, Space.World);
                //在此坐标上播放设置神兽出生点前的特效
                //this.m_listEffectIds.Add();
            }
            string oPDlgTip = StringConfigMgr.GetString("StageNotice_BornState");
            //界面显示提示
            //DlgBase<DlgStateProgress, DlgStateProgressBehaviour>.singleton.ShowNotice(@string);
        }
        public override void OnLeave()
        {
            base.OnLeave();
            Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_Highlight);
            this.m_listTargetHexs.Clear();
            foreach (var effect in this.m_listEffectIds)
            {
                //停止特效
                //EffectManager.singleton.StopEffect(current);
            }
            this.m_listEffectIds.Clear();
        }
        public override void Refresh()
        {
            base.Refresh();
            this.OnLeave();
            this.OnEnter();
        }
        public override bool OnSelectPos(CVector3 vecHex)
        {
            bool result;
            if (!this.m_listTargetHexs.Exists((CVector3 vec) => vec.Equals(vecHex)))
            {
                result = true;
            }
            else 
            {
                Singleton<NetworkManager>.singleton.SendInitPos(vecHex);
                this.OnLeave();
                result = true;
            }
            return result;
        }
        public override bool OnButtonOkClick()
        {
            return true;
        }
    }
}