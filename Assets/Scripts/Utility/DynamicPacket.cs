using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DynamicPacket
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.7
// 模块描述：数据包解析器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 数据包解析器
/// </summary>
namespace Utility.Export
{
    public class DynamicPacket
    {
        public static IDynamicPacket Create()
        {
            return new DynamicPacketImplement();
        }
        public static IDynamicPacket Create(byte[] bytes)
        {
            return new DynamicPacketImplement(bytes);
        }
        public static IDynamicPacket Creates(byte[] bytes, int offset, int count)
        {
            return new DynamicPacketImplement(bytes, offset, count);
        }
    }
}
