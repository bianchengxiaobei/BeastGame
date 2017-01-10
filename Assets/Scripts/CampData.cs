using UnityEngine;
using System.Collections;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CampData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.11
// 模块描述：阵营数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 阵营数据（血量，最大血量，初始金钱）
/// </summary>
public struct CampData
{
    #region 字段
    private uint m_unMoney;
    private uint m_unHp;
    private uint m_unMaxHp;
    private ECampType m_eCampType;
    private EBaseStatus m_eBaseStatus;
    #endregion
    public uint Money
    {
        get
        {
            return this.m_unMoney;
        }
        set
        {
            this.m_unMoney = value;
        }
    }
    public uint MaxHp
    {
        get
        {
            return this.m_unMaxHp;
        }
        set
        {
            this.m_unMaxHp = value;
        }
    }
    public uint Hp
    {
        get
        {
            return this.m_unHp;
        }
        set
        {
            this.m_unHp = value;
        }
    }
    public EBaseStatus Status
    {
        get
        {
            return this.m_eBaseStatus;
        }
        set
        {
            this.m_eBaseStatus = value;
        }
    }
    public ECampType CampType
    {
        get
        {
            return this.m_eCampType;
        }
    }
    public CampData(uint unHp, ECampType eCampType)
    {
        this.m_unHp = unHp;
        this.m_eCampType = eCampType;
        this.m_unMoney = 0u;
        this.m_unMaxHp = 0u;
        this.m_eBaseStatus = EBaseStatus.BASE_STATUS_NORMAL;
    }
}