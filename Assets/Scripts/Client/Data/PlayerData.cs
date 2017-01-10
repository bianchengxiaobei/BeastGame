using UnityEngine;
using System.Collections.Generic;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PlayerData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：房间内玩家角色信息数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 房间内玩家角色信息数据
/// </summary>
public class PlayerData 
{
	#region 字段
    private ECampType m_eCampType = ECampType.CAMP_INVALID;
    private string m_strName = "";
    private long m_unPlayerId;
    private string m_strIcon = "";
    private int m_unLevel;
    private bool m_bIsLoadFinish;
    private List<BeastData> m_listBeasts = new List<BeastData>();
    private bool m_bIsReconnecting = false;
	#endregion
	#region 属性
    /// <summary>
    /// 是否是重新连接
    /// </summary>
    public bool IsReconnect
    {
        get { return this.m_bIsReconnecting; }
        set { this.m_bIsReconnecting = value; }
    }
    /// <summary>
    /// 玩家所在阵营
    /// </summary>
    public ECampType CampType 
    {
        get { return this.m_eCampType; }
    }
    /// <summary>
    /// 玩家id
    /// </summary>
    public long PlayerId
    {
        get { return this.m_unPlayerId; }
    }
    /// <summary>
    /// 是否玩家数据加载完成
    /// </summary>
    public bool IsLoadFinish 
    {
        get { return this.m_bIsLoadFinish; }
        set { this.m_bIsLoadFinish = value; }
    }
    /// <summary>
    /// 玩家名字
    /// </summary>
    public string Name 
    {
        get { return this.m_strName; }
        set { this.m_strName = value; }
    }
    /// <summary>
    /// 玩家头像
    /// </summary>
    public string Icon 
    {
        get { return this.m_strIcon; }
        set { this.m_strIcon = value; }
    }
    /// <summary>
    /// 玩家等级
    /// </summary>
    public int Level 
    {
        get { return this.m_unLevel; }
        set { this.m_unLevel = value; }
    }
    /// <summary>
    /// 玩家所拥有的神兽数据信息列表
    /// </summary>
    public List<BeastData> Beasts 
    {
        get { return this.m_listBeasts; }
    }
	#endregion
	#region 构造方法
    public PlayerData(long playerId,ECampType eCampType)
    {
        this.m_unPlayerId = playerId;
        this.m_eCampType = eCampType;
    }
	#endregion
}
