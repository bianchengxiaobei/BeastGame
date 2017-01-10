using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CGuideData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CGuideData : IData
    {
        #region 字段 
        public byte m_currPassNum;
        public List<byte> m_firstHappenedFlags;
        #endregion
        #region 构造方法
        public CGuideData()
        {
            this.m_currPassNum = 0;
            this.m_firstHappenedFlags = new List<byte>();
        }
        public CGuideData(byte currPassNum)
        {
            this.m_currPassNum = currPassNum;
            this.m_firstHappenedFlags = new List<byte>();
        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            return bs;
        }
        public void CopyFrom(CGuideData cgd)
        {
            this.m_currPassNum = cgd.m_currPassNum;
            this.m_firstHappenedFlags.Clear();
            for (int i = 0; i < cgd.m_firstHappenedFlags.Count; i++)
            {
                this.m_firstHappenedFlags.Add(cgd.m_firstHappenedFlags[i]);
            }
        }
        public void Reset()
        {
            this.m_currPassNum = 0;
            this.m_firstHappenedFlags.Clear();
        }
        #endregion
    }
}
