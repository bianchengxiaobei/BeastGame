using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CVector3
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CVector3 : IData, IEquatable<CVector3>
    {
        public static readonly CVector3 MaxValue = new CVector3(2147483647, 2147483647, 2147483647);
        public int m_nX;
        public int m_nY;
        public int m_nU;
        public int nCol
        {
            get
            {
                return this.m_nX;
            }
            set
            {
                this.m_nX = value;
                this.m_nY = this.m_nU + this.m_nX;
            }
        }
        public int nRow
        {
            get
            {
                return this.m_nU;
            }
            set
            {
                this.m_nU = value;
                this.m_nY = this.m_nU + this.m_nX;
            }
        }
        public CVector3()
        {
            this.m_nX = 2147483647;
            this.m_nY = 2147483647;
            this.m_nU = 2147483647;
        }
        public CVector3(CVector3 pos)
        {
            this.m_nX = pos.m_nX;
            this.m_nY = pos.m_nY;
            this.m_nU = pos.m_nU;
        }
        public CVector3(int nX, int nY, int nU)
        {
            this.m_nX = nX;
            this.m_nY = nY;
            this.m_nU = nU;
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_nX);
            bs.Write(this.m_nY);
            bs.Write(this.m_nU);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_nX);
            bs.Read(ref this.m_nY);
            bs.Read(ref this.m_nU);
            return bs;
        }
        public void Reset()
        {
            this.m_nX = 2147483647;
            this.m_nY = 2147483647;
            this.m_nU = 2147483647;
        }
        public bool IsValid()
        {
            return this.m_nX != 2147483647;
        }
        public bool Equals(CVector3 other)
        {
            return other != null && (this.nRow == other.nRow && this.nCol == other.nCol);
        }
        public void CopyFrom(CVector3 vec3Pos)
        {
            if (null != vec3Pos)
            {
                this.m_nX = vec3Pos.m_nX;
                this.m_nY = vec3Pos.m_nY;
                this.m_nU = vec3Pos.m_nU;
            }
        }
        public object Clone()
        {
            return base.MemberwiseClone();
        }
        public override string ToString()
        {
            return string.Format("X={0}, Y={1}, U={2}", this.m_nX, this.m_nY, this.m_nU);
        }
    }
}