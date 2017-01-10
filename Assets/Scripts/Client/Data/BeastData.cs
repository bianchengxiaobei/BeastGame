using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BeastData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：神兽信息数据
//----------------------------------------------------------------*/
#endregion
public class BeastData 
{
	#region 字段
    private long m_unBeastId;//神兽id
    private int m_unBeastTypeId;
    private long m_unPlayerId;//角色id
    private int m_nBeastLevel;//神兽等级
    private int m_nSuitId = 0;//皮肤id
    private int m_unEquipIndex = 0;//当前装备选择索引
    private bool m_bSelected = false;//是否已经被选择了
    private bool m_bRandom = false;//是否是随机英雄
    private ECampType m_eCamptype;//阵营
    private List<SkillGameData> m_oSkillList = new List<SkillGameData>();//神兽技能
	#endregion
	#region 属性
    /// <summary>
    /// 神兽id
    /// </summary>
    public long Id 
    {
        get
        {
            return this.m_unBeastId;
        }
    }
    /// <summary>
    /// 角色id
    /// </summary>
    public long PlayerId 
    {
        get 
        {
            return this.m_unPlayerId;
        }
    }
    /// <summary>
    /// 所在阵营
    /// </summary>
    public ECampType CampType 
    {
        get { return this.m_eCamptype; }
    }
    /// <summary>
    /// 神兽等级
    /// </summary>
    public int BeastLevel 
    {
        get { return this.m_nBeastLevel; }
        set { this.m_nBeastLevel = value; }
    }
    /// <summary>
    /// 神兽血量
    /// </summary>
    public int Hp
    {
        get;
        set;
    }
    /// <summary>
    /// 皮肤Id
    /// </summary>
    public int SuitId
    {
        get
        {
            if (this.m_nSuitId <= 0)
            {
                //List<DataSuit> dataListByHeroId = DataSuitManager.Instance.GetDataListByHeroId((int)this.m_unHeroTypeId);
                List<DataSuit> dataListByBeastId = new List<DataSuit>();
                foreach (var current in GameData<DataSuit>.dataMap)
                {
                    if (current.Key == (int)this.m_unBeastTypeId)
                    {
                        dataListByBeastId.Add(current.Value);
                    }
                }
                if (dataListByBeastId.Count > 0)
                {
                    this.m_nSuitId = dataListByBeastId[0].ID;
                }
            }
            return this.m_nSuitId;
        }
        set
        {
            this.m_nSuitId = value;
        }
    }
    /// <summary>
    /// 神兽类型id
    /// </summary>
    public int BeastTypeId
    {
        get
        {
            return this.m_unBeastTypeId;
        }
        set
        {
            this.m_unBeastTypeId = value;
            DataBeastlist dataById = GameData<DataBeastlist>.dataMap[(int)this.m_unBeastTypeId];
            if (null != dataById)
            {
                this.Hp = dataById.Hp;
            }
        }
    }
    /// <summary>
    /// 是否被选择上
    /// </summary>
    public bool IsSelected
    {
        get
        {
            return this.m_bSelected;
        }
        set
        {
            this.m_bSelected = value;
        }
    }
    /// <summary>
    /// 当前装备选择索引
    /// </summary>
    public int EquipIndex
    {
        get
        {
            return this.m_unEquipIndex;
        }
        set
        {
            this.m_unEquipIndex = value;
        }
    }
    /// <summary>
    /// 是否随机
    /// </summary>
    public bool IsRandom 
    {
        get { return this.m_bRandom; }
        set { this.m_bRandom = value; }
    }
    /// <summary>
    /// 神兽技能列表
    /// </summary>
    public List<SkillGameData> Skills 
    {
        get { return this.m_oSkillList; }
    }
	#endregion
	#region 构造方法
    public BeastData(long playerId, long beastId, ECampType eCampType)
    {
        this.m_unPlayerId = playerId;
        this.m_unBeastId = beastId;
        this.m_eCamptype = eCampType;
        this.m_unBeastTypeId = -1;
        this.m_nBeastLevel = 0;
    }
	#endregion
	#region 公有方法
	#endregion
	#region 私有方法
	#endregion
}

