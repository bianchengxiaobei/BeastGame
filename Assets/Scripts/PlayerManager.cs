using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PlayerManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.8
// 模块描述：玩家管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 玩家管理器
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
	#region 字段
    private static Player s_playerError = new Player(null);
    private IXLog m_log = XLog.GetLog<PlayerManager>();
    /// <summary>
    /// 所有玩家的缓存字典
    /// </summary>
    private Dictionary<long, Player> m_dicAllPlayerId = new Dictionary<long, Player>();
    private Dictionary<string, Player> m_dicAllPlayerName = new Dictionary<string, Player>();
	#endregion
	#region 属性
    /// <summary>
    /// 错误的玩家实例
    /// </summary>
    public static Player ErrorPlayer
    {
        get
        {
            return PlayerManager.s_playerError;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 创建所有玩家
    /// </summary>
    /// <param name="listPlayerData"></param>
    public void CreateAllPlayer(List<PlayerData> listPlayerData)
    {
        this.AddPlayer(listPlayerData);
        //如果玩家所拥有的神兽数量大于0，初始化BeastRole的id
        if (Singleton<PlayerRole>.singleton.Player.ListBeastId.Count > 0)
        {
            Singleton<BeastRole>.singleton.Id = Singleton<PlayerRole>.singleton.Player.ListBeastId[0];
        }
        //DlgBase<DlgHeadInfo, DlgHeadInfoBehaviour>.singleton.Refresh();
        //DlgBase<DlgTab, DlgTabBehaviour>.singleton.Refresh();
        //Singleton<OpStateManager>.singleton.ChangeState(OpStateManager.enumOpState.eOpState_Wait);
    }
    /// <summary>
    /// 删除所有玩家的数据
    /// </summary>
    public void DelAllPlayers()
    {
        foreach (Player current in this.m_dicAllPlayerId.Values)
        {
            current.Dispose();
        }
        this.m_dicAllPlayerId.Clear();
        this.m_dicAllPlayerName.Clear();
       // DlgBase<DlgHeadInfo, DlgHeadInfoBehaviour>.singleton.Refresh();
    }
    /// <summary>
    /// 根据玩家id获取玩家类，如果娶不到就返回静态错误玩家实例
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Player GetPlayerByID(long id)
    {
        Player result;
        if (this.m_dicAllPlayerId.ContainsKey(id))
        {
            result = this.m_dicAllPlayerId[id];
        }
        else
        {
            result = PlayerManager.s_playerError;
        }
        return result;
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 添加Player
    /// </summary>
    /// <param name="listPlayer"></param>
    private void AddPlayer(List<PlayerData> listPlayer)
    {
        foreach (PlayerData current in listPlayer)
        {
            Player player = new Player(current);
            this.m_dicAllPlayerId[player.Id] = player;
            this.m_dicAllPlayerName[player.Name] = player;
        }
    }
	#endregion
}
