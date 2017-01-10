using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SkillGameData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：战棋技能数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战棋技能数据
/// </summary>
public class SkillGameData
{
	private int m_unSkillId = 0;
	private byte m_byteCDTime = 0;
	private DataSkill m_skillConfig = null;
	private bool m_bIsError = true;
	private bool m_bLockUsed = false;
	private static SkillGameData s_skillDataError = new SkillGameData();
	public byte CDTime
	{
		get
		{
			return this.m_byteCDTime;
		}
		set
		{
			this.m_byteCDTime = value;
		}
	}
    public int SkillType
    {
        get { return this.m_skillConfig.SkillType; }
    }
	public int Id
	{
		get
		{
			return this.m_unSkillId;
		}
	}
	public bool IsActive
	{
		get
		{
			return null != this.m_skillConfig && this.m_skillConfig.IsActive;
		}
	}
	public string TipInfo
	{
		get
		{
			string result;
			if (null != this.m_skillConfig)
			{
				result = this.m_skillConfig.Desc;
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
	public string SmallIconFile
	{
		get
		{
			string result;
			if (null != this.m_skillConfig)
			{
				result = this.m_skillConfig.SmallIcon;
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
	public string IconFile
	{
		get
		{
			string result;
			if (null != this.m_skillConfig)
			{
				result = this.m_skillConfig.Icon;
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
	public string Name
	{
		get
		{
			string result;
			if (null != this.m_skillConfig)
			{
				result = this.m_skillConfig.Name;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}
	public bool IsError
	{
		get
		{
			return this.m_bIsError;
		}
	}
	public bool LockUsed
	{
		get
		{
			return this.m_bLockUsed;
		}
		set
		{
			this.m_bLockUsed = value;
		}
	}
	public static SkillGameData SkillDataError
	{
		get
		{
			return SkillGameData.s_skillDataError;
		}
	}
	public SkillGameData(int unSkillId)
	{
		this.m_unSkillId = unSkillId;
		this.m_skillConfig = GameData<DataSkill>.dataMap[unSkillId];
		this.m_bIsError = false;
	}
	private SkillGameData()
	{
		this.m_bIsError = true;
	}
}
