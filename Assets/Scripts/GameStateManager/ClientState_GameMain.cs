using UnityEngine;
using System.Collections;
using Utility;
using Game;
using Client.Data;
using GameData;
using Client.UI.UICommon;
using GameClient.Audio;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_GameMain
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：游戏战斗主状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏战斗主状态
/// </summary>
public class ClientState_GameMain : ClientState_Base
{
    public override void OnEnter()
    {
        base.OnEnter();
        //加载所有神兽的模型
        Singleton<BeastManager>.singleton.LoadAllBeastModel();
        //加载场景
        CSceneMgr.singleton.LoadScene();
        if (!Singleton<RoomManager>.singleton.IsObserver)
        {
            DlgBase<DlgMain, DlgMainBehaviour>.singleton.SetVisible(true);
        }
        Singleton<RoomManager>.singleton.GameOver = false;
        DataMaplist mapData = GameData<DataMaplist>.dataMap[(int)Singleton<RoomManager>.singleton.MapId];
        if (mapData != null)
        {
            string audioPath = mapData.BgSoundFile;
            if (!string.IsNullOrEmpty(audioPath))
            {
                Singleton<AudioManager>.singleton.PlayMusic("Audio/BMG/Map/"+audioPath);
            }
        }
        Singleton<GameMainResPreLoad>.singleton.PreLoad();
    }
    public override void OnLeave()
    {
        base.OnLeave();
    }
}
