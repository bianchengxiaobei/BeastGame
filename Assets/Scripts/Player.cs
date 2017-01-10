using UnityEngine;
using System.Collections.Generic;
using System;
using Client.Common;
using Utility;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Player
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.8
// 模块描述：玩家类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 玩家类
/// </summary>
public class Player : IDisposable
{
	#region 字段
    private bool m_bDisposed = false;
    private long m_unPlayerId = 0;
    private string m_strPlayerName = "";
    private List<long> m_listBeastId = new List<long>();
    private ECampType m_eCampType;
    private bool m_bIsRole = false;
    private bool m_bIsLostConnect = false;
    private bool m_bIsErrorPlayer = true;
	#endregion
	#region 属性
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string Name
    {
        get
        {
            return this.m_strPlayerName;
        }
        set
        {
            this.m_strPlayerName = value;
        }
    }
    /// <summary>
    /// 玩家聊天时的不同阵营不同颜色的昵称[548ce0]昵称[-]
    /// </summary>
    public string ColorPlayerName
    {
        get
        {
            string str = CommonDefine.CampTypeToChatColor(this.CampType == Singleton<PlayerRole>.singleton.CampType);
            return str + this.Name + "[-]";
        }
    }
    /// <summary>
    /// 玩家所在阵营
    /// </summary>
    public ECampType CampType
    {
        get
        {
            return this.m_eCampType;
        }
    }
    /// <summary>
    /// 玩家id
    /// </summary>
    public long Id
    {
        get
        {
            return this.m_unPlayerId;
        }
    }
    /// <summary>
    /// 玩家所拥有的神兽id
    /// </summary>
    public List<long> ListBeastId
    {
        get
        {
            return this.m_listBeastId;
        }
    }
    /// <summary>
    /// 是否是玩家
    /// </summary>
    public bool Role
    {
        get
        {
            return this.m_bIsRole;
        }
    }
    /// <summary>
    /// 是否是重连
    /// </summary>
    public bool IsLostConnect
    {
        get
        {
            return this.m_bIsLostConnect;
        }
        set
        {
            if (value != this.m_bIsLostConnect)
            {
                this.m_bIsLostConnect = value;
                //DlgBase<DlgHeros, DlgHerosBehaviour>.singleton.Refresh();
            }
        }
    }
    /// <summary>
    /// 是否加载玩家时候出错，无数据
    /// </summary>
    public bool IsError
    {
        get
        {
            return this.m_bIsErrorPlayer;
        }
    }
	#endregion
	#region 构造方法
    public Player(PlayerData playerData)
    {
        if (null != playerData)
        {
            this.m_bIsErrorPlayer = false;
            this.m_unPlayerId = playerData.PlayerId;
            this.m_eCampType = playerData.CampType;
            this.m_strPlayerName = playerData.Name;
            this.m_bIsLostConnect = playerData.IsReconnect;
            this.m_bIsRole = (Singleton<PlayerRole>.singleton.ID == this.m_unPlayerId);
            this.m_listBeastId.Clear();
            for (int i = 0; i < playerData.Beasts.Count; i++)
            {
                BeastData beastData = playerData.Beasts[i];
                this.m_listBeastId.Add(beastData.Id);
                Beast beast = Singleton<BeastManager>.singleton.CreateBeast(beastData);
                if (null != beast)
                {
                }
            }
        }
    }
	#endregion
	#region 公有方法
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
	#endregion
	#region 私有方法
    protected virtual void Dispose(bool disposing)
    {
        if (!this.m_bDisposed)
        {
            if (disposing)
            {
                for (int i = this.m_listBeastId.Count - 1; i >= 0; i--)
                {
                    long unHeroId = this.m_listBeastId[i];
                    Singleton<BeastManager>.singleton.DelBeast(unHeroId);
                }
                this.m_listBeastId.Clear();
            }
            this.m_bDisposed = true;
        }
    }
	#endregion
}
