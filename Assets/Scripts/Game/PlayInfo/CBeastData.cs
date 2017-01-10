using UnityEngine;
using System.Collections.Generic;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CBeastData 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.25
// 模块描述：神兽数据类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽数据类
/// </summary>
public class CBeastData : IData
{
    public int m_dwID;
	public int m_dwExp;
	public int m_wLevel;
    public int m_dwCurrSuit;
	public List<int> m_oSkillList;
    public List<int> m_oSuitList;
	public CBeastData()
	{
		this.m_dwID = 0;
		this.m_dwExp = 0;
		this.m_wLevel = 0;
		this.m_oSkillList = new List<int>();
        this.m_oSuitList = new List<int>();
	}
    public CBeastData(int dwId, int dwExp, int dwLevel, int currSuit)
    {
        this.m_dwID = dwId;
        this.m_dwExp = dwExp;
        this.m_wLevel = dwLevel;
        this.m_dwCurrSuit = currSuit;
        this.m_oSkillList = new List<int>();
        this.m_oSuitList = new List<int>();
    }
	public override CByteStream Serialize(CByteStream bs)
	{
		bs.Write(this.m_dwID);
		bs.Write(this.m_dwExp);
		bs.Write(this.m_wLevel);
		bs.Write(this.m_oSkillList);
		return bs;
	}
	public override CByteStream DeSerialize(CByteStream bs)
	{
		bs.Read(ref this.m_dwID);
		bs.Read(ref this.m_dwExp);
		bs.Read(ref this.m_wLevel);
		bs.Read(this.m_oSkillList);
		return bs;
	}
	public void Reset()
	{
		this.m_dwID = 0;
		this.m_dwExp = 0;
        this.m_wLevel = 0;
		this.m_oSkillList.Clear();
	}
}
