using UnityEngine;
using System.Collections.Generic;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CBeastInfo 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.25
// 模块描述：神兽信息描述
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽信息描述
/// </summary>
public class CBeastInfo : IData
{
	public Dictionary<int, CBeastData> m_oBeastMap;
    public Dictionary<int, CBeastData> m_oWeekBeastMap;
	public CBeastInfo()
	{
		this.m_oBeastMap = new Dictionary<int, CBeastData>();
        this.m_oWeekBeastMap = new Dictionary<int, CBeastData>();
	}
	public override CByteStream Serialize(CByteStream bs)
	{
		bs.Write(this.m_oBeastMap.Count);
		foreach (KeyValuePair<int, CBeastData> current in this.m_oBeastMap)
		{
			bs.Write(current.Key);
			bs.Write(current.Value);
		}
		return bs;
	}
	public override CByteStream DeSerialize(CByteStream bs)
	{
		this.m_oBeastMap.Clear();
		int num = 0;
		bs.Read(ref num);
		for (int i = 0; i < num; i++)
		{
			int key = 0;
			CBeastData CBeastData = new CBeastData();
			bs.Read(ref key);
			bs.Read(CBeastData);
			this.m_oBeastMap.Add(key, CBeastData);
		}
		return bs;
	}
	public void Reset()
	{
		this.m_oBeastMap.Clear();
        this.m_oWeekBeastMap.Clear();
	}
}
