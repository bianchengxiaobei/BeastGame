using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IDynamicData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Export
{
    public interface IDynamicData
    {
        void Deserialize(IDynamicPacket packet);
        void Serialize(IDynamicPacket packet);
    }
}
