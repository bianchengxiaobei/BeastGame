using UnityEngine;
using System.Collections;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CTerrainNodeData
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：山地格子信息
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    [Serializable]
    public class CTerrainNodeData : IData
    {
        public int m_nType;
        public CVector3 m_oPos;
        public CTerrainNodeData()
        {
            this.m_nType = 0;
            this.m_oPos = new CVector3();
        }
        public CTerrainNodeData(int nType)
        {
            this.m_nType = nType;
            this.m_oPos = new CVector3();
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_nType);
            bs.Write(this.m_oPos);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_nType);
            bs.Read(this.m_oPos);
            return bs;
        }
        public void CopyFrom(CTerrainNodeData rhs)
        {
            this.m_nType = rhs.m_nType;
            this.m_oPos.CopyFrom(rhs.m_oPos);
        }
        public void Reset()
        {
            this.m_nType = 0;
            this.m_oPos.Reset();
        }
    }
}