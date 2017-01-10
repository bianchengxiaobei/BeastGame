using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CTaskData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：单任务数据
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CTaskData : IData
    {
        public uint m_id;
        public byte m_btStatus;
        public Dictionary<byte, uint> m_aimMap;
        public uint m_acceptTime;
        public CTaskData()
        {
            this.m_id = 0u;
            this.m_btStatus = 0;
            this.m_aimMap = new Dictionary<byte, uint>();
            this.m_acceptTime = 0u;
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            return bs;
        }
    }
}