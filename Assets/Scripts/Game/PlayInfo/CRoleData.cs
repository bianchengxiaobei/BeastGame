using UnityEngine;
using System.Collections.Generic;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CRoleData 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.20
// 模块描述：神兽角色数据类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽角色数据类
/// </summary>
public class CRoleData : IData 
{
	#region 字段
	#endregion
	#region 属性
    public long m_dwRoleId;
    public int m_dwBeastId;
    public byte m_btHp;
    public byte m_btMaxHp;
	#endregion
	#region 构造方法
	#endregion
	#region 公共方法
    public override CByteStream DeSerialize(CByteStream bs)
    {
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        return bs;
    }
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
