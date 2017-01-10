using UnityEngine;
using System.Collections;
using Utility;
using Client.UI;
using Client.Data;
using Client.Common;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：WaitState 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.15
// 模块描述：等待阶段状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 等待阶段状态
/// </summary>
namespace Client.GameMain.OpState.Stage
{
    public class WaitState : OpStateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //清除所有的格子
            Singleton<HexagonManager>.singleton.ClearAllHexagon();
            UIManager.singleton.SetCursor(Singleton<RoomManager>.singleton.IsObserver ? enumCursorType.eCursorType_Normal : enumCursorType.eCursorType_Disable);
            CSceneMgr.singleton.ControlColliderEnable(true);
        }
        public override void OnLeave()
        {
            base.OnLeave();
            CSceneMgr.singleton.ControlColliderEnable(false);
        }
    }
}
