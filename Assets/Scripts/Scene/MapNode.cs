using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapNode
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：地图格子信息类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 地图格子信息类
/// </summary>
public class MapNode 
{
    public EMapNodeType m_eMapNodeType;
    public EMapJudgeType m_eMapJudgeType;
    public bool m_bCanWalk;
    /// <summary>
    /// 格子类型
    /// </summary>
    public string m_strType = string.Empty;
    public string m_strName = string.Empty;
    private int m_nIndexX;
    private int m_nIndexY;
    private int m_nIndexU;
    /// <summary>
    /// 地图格子的x坐标（竖直方向）
    /// </summary>
    public int nIndexX
    {
        get
        {
            return this.m_nIndexX;
        }
        set
        {
            this.m_nIndexX = value;
        }
    }
    /// <summary>
    /// 地图格子的y坐标（偏右方向）
    /// </summary>
    public int nIndexY
    {
        get
        {
            return this.m_nIndexY;
        }
        set
        {
            this.m_nIndexY = value;
        }
    }
    /// <summary>
    /// 地图格子的u坐标（偏左方向）
    /// </summary>
    public int nIndexU
    {
        get
        {
            return this.m_nIndexU;
        }
        set
        {
            this.m_nIndexU = value;
        }
    }
    /// <summary>
    /// 当前格子所在列（竖直方向）
    /// </summary>
    public int nCol
    {
        get
        {
            return this.m_nIndexX;
        }
    }
    /// <summary>
    /// 当前格子所在行（以偏左方向根据）
    /// </summary>
    public int nRow
    {
        get
        {
            return this.m_nIndexU;
        }
    }
    public MapNode()
    {
        this.nIndexX = 0;
        this.nIndexY = 0;
        this.nIndexU = 0;
        this.m_eMapNodeType = EMapNodeType.MAP_NODE_INVALID;
        this.m_eMapJudgeType = EMapJudgeType.MAP_JUDGE_INVALID;
        this.m_bCanWalk = false;
    }
}
