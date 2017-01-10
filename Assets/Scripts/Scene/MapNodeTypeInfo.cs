using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapNodeTypeInfo
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：不同类型格子信息类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 不同类型格子信息类
/// </summary>
public class MapNodeTypeInfo 
{
    private EMapNodeType m_eMapNodeType;
    private string m_strModeFile = string.Empty;
    private Vector3 m_localPos = Vector3.zero;
    private Vector3 m_localRotation = Vector3.zero;
    public EMapNodeType MapNodeType
    {
        get
        {
            return this.m_eMapNodeType;
        }
        set
        {
            this.m_eMapNodeType = value;
        }
    }
    public string ModelFile
    {
        get
        {
            return this.m_strModeFile;
        }
        set
        {
            this.m_strModeFile = value;
        }
    }
    public Vector3 LocalPos
    {
        get
        {
            return this.m_localPos;
        }
        set
        {
            this.m_localPos = value;
        }
    }
    public Vector3 LocalRotation
    {
        get
        {
            return this.m_localRotation;
        }
        set
        {
            this.m_localRotation = value;
        }
    }
    public int EffectId
    {
        get;
        set;
    }
    public int GetEffectId
    {
        get;
        set;
    }
    public MapNodeTypeInfo()
    {
    }
    public MapNodeTypeInfo(EMapNodeType eMapNodeType, string strModelFile)
    {
        this.m_eMapNodeType = eMapNodeType;
        this.m_strModeFile = strModelFile;
    }
}
public enum EMapNodeType
{
    MAP_NODE_INVALID,
    MAP_NODE_EMPIRE_BASE,
    MAP_NODE_LEAGUE_BASE,
    MAP_NODE_ROCK,
    MAP_NODE_SHOP,
    MAP_NODE_MAGIC_SPRING,
    MAP_NODE_GOLDEN,
    MAP_NODE_CAMP,
    MAP_NODE_SWAMP,
    MAP_NODE_REBORN_EMPIRE,
    MAP_NODE_REBORN_LEAGUE,
    MAP_NODE_DESERT,
    MAP_NODE_WATER,
    MAP_NODE_FOREST,
    MAP_NODE_MONEY,
    MAP_NODE_TELEPORT,
    MAP_NODE_MAGIC_TURRET,
    MAP_NODE_RUNE_ARRAY,
    MAP_NODE_GOLDEN_NEW,
    MAP_NODE_MONEY_NEW,
    MAP_NODE_CAMP_NEW,
    MAP_NODE_ACCELERATE_ARRAY,
    MAP_NODE_DEVIL_ALTAR,
    MAP_NODE_HEALING_WARD,
    MAP_NODE_ICE_WALL,
    MAP_NODE_OPEN_DOOR,
    MAP_NODE_CLOSE_DOOR,
    MAP_NODE_DOOR_SWITCH,
    MAP_NODE_GOLDEN_NEW_2,
    MAP_NODE_STABLE,
    MAP_NODE_MONKEY,
    MAP_NODE_SKILL,
    MAP_NODE_TOTEM,
    MAP_NODE_ROCK_PRISON
}
public enum EMapJudgeType
{
    MAP_JUDGE_INVALID,
    MAP_JUDGE_DELAY,
    MAP_JUDGE_INSTANT,
    MAP_JUDGE_NO
}