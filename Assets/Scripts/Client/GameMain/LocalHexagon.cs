using UnityEngine;
using System.Collections.Generic;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalHexagon
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Local
{
    public class HexagonImplement
    {
        #region 字段
        /// <summary>
        /// 六边形格子的上下顶点的距离（作为先对长度），好计算坐标
        /// </summary>
        public static float m_fSideLength = 0.85f * Mathf.Sqrt(3f);
        private static Transform m_transformBase = null;
        #endregion
        #region 属性
        public static Transform TransformBase
        {
            get
            {
                return HexagonImplement.m_transformBase;
            }
            set
            {
                HexagonImplement.m_transformBase = value;
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public static Transform GetTransform()
        {
            return HexagonImplement.m_transformBase;
        }
        public static void SetTransform(Transform transform)
        {
            HexagonImplement.m_transformBase = transform;
        }
        /// <summary>
        /// 填充格子mesh，总共有7个顶点，中心为一点，然后边长其他六个顶点
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="fY"></param>
        /// <param name="listVertex"></param>
        /// <param name="listVertexIndex"></param>
        /// <param name="listUV"></param>
        /// <returns></returns>
        public static bool FillHexagon(int nRow, int nCol, float fY, ref List<Vector3> listVertex, ref List<int> listVertexIndex, ref List<Vector2> listUV)
        {
            int count = listVertex.Count;//定点数量
            float centerX = (float)nCol * HexagonImplement.m_fSideLength + (float)nRow * HexagonImplement.m_fSideLength / 2;
            float centerZ = (float)nRow * HexagonImplement.m_fSideLength * Mathf.Sqrt(3f) / 2f;
            listVertex.Add(new Vector3(centerX, fY, centerZ));
            listUV.Add(new Vector2(0.5f, 0.5f));
            float x1 = centerX - 0.5f * HexagonImplement.m_fSideLength;
            float z1 = centerZ - 0.5f / Mathf.Sqrt(3f) * HexagonImplement.m_fSideLength;
            listVertex.Add(new Vector3(x1, fY, z1));
            listUV.Add(new Vector2(0f, 0.75f));
            float x2 = centerX;
            float z2 = centerZ - 1f / Mathf.Sqrt(3f) * HexagonImplement.m_fSideLength;
            listVertex.Add(new Vector3(x2, fY, z2));
            listUV.Add(new Vector2(0.5f, 1f));
            float x3 = centerX + 0.5f * HexagonImplement.m_fSideLength;
            float z3 = centerZ - 0.5f / Mathf.Sqrt(3f) * HexagonImplement.m_fSideLength;
            listVertex.Add(new Vector3(x3, fY, z3));
            listUV.Add(new Vector2(1f, 0.75f));
            float x4 = centerX + 0.5f * HexagonImplement.m_fSideLength;
            float z4 = centerZ + 0.5f / Mathf.Sqrt(3f) * HexagonImplement.m_fSideLength;
            listVertex.Add(new Vector3(x4, fY, z4));
            listUV.Add(new Vector2(1f, 0.25f));
            float x5 = centerX;
            float z5 = centerZ + 1f / Mathf.Sqrt(3f) * HexagonImplement.m_fSideLength;
            listVertex.Add(new Vector3(x5, fY, z5));
            listUV.Add(new Vector2(0.5f, 0f));
            float x6 = centerX - 0.5f * HexagonImplement.m_fSideLength;
            float z6 = centerZ + 0.5f / Mathf.Sqrt(3f) * HexagonImplement.m_fSideLength;
            listVertex.Add(new Vector3(x6, fY, z6));
            listUV.Add(new Vector2(0f, 0.25f));
            int[] collection = new int[]
            {
                count + 1,
				    count,
				    count + 2,
				    count + 2,
				    count,
				    count + 3,
				    count + 3,
				    count,
				    count + 4,
				    count + 4,
				    count,
				    count + 5,
				    count + 5,
				    count,
				    count + 6,
				    count + 6,
				    count,
				    count + 1
            };
            listVertexIndex.AddRange(collection);
            return (count + 7) == listVertex.Count;
        }
        /// <summary>
        /// 根据格子坐标计算得到相对格子的世界坐标
        /// </summary>
        /// <param name="hexPos"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public static Vector3 GetHex3DPos(CVector3 hexPos, Space space)
        {
            Vector3 result;
            if (null != hexPos)
            {
                result = HexagonImplement.GetHex3DPosByIndex(hexPos.nRow, hexPos.nCol, space);
            }
            else
            {
                result = Vector3.zero;
            }
            return result;
        }
        /// <summary>
        /// 根据格子坐标计算得到相对格子的世界坐标
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public static Vector3 GetHex3DPosByIndex(int nRow, int nCol, Space space)
        {
            Vector3 vector = Vector3.zero;
            vector.x = (float)nCol * HexagonImplement.m_fSideLength + (float)nRow * HexagonImplement.m_fSideLength / 2f;
            vector.z = (float)nRow * HexagonImplement.m_fSideLength * Mathf.Sqrt(3f) / 2f;
            if (HexagonImplement.TransformBase != null && Space.World == space)
            {
                vector = HexagonImplement.TransformBase.TransformPoint(vector);
            }
            return vector;
        }
        /// <summary>
        /// 根据格子坐标计算得到相对格子的世界平面坐标（x,y）
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public static Vector2 GetHexPosByIndex(int nRow, int nCol, Space space)
        {
            Vector2 zero = Vector2.zero;
            Vector3 hex3DPosByIndex = HexagonImplement.GetHex3DPosByIndex(nRow, nCol, space);
            zero.x = hex3DPosByIndex.x;
            zero.y = hex3DPosByIndex.z;
            return zero;
        }
        /// <summary>
        /// 根据格子坐标计算得到自身平面坐标（x,y）
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static Vector2 GetHexPosByIndex(int nRow, int nCol)
        {
            return HexagonImplement.GetHexPosByIndex(nRow, nCol, Space.Self);
        }
        /// <summary>
        /// 根据世界坐标获取格子坐标
        /// </summary>
        /// <param name="realPos"></param>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static bool GetHexIndexByPos(Vector3 realPos, out int nRow, out int nCol)
        {
            if (null != HexagonImplement.TransformBase)
            {
                realPos = HexagonImplement.TransformBase.InverseTransformPoint(realPos);
            }
            Vector2 pos = new Vector2(realPos.x, realPos.z);
            float num = (pos.x - pos.y / Mathf.Sqrt(3f)) / HexagonImplement.m_fSideLength;
            int num2 = (int)num;
            float num3 = pos.y / (HexagonImplement.m_fSideLength * Mathf.Sqrt(3f) / 2f);
            int num4 = (int)num3;
            int num5 = (int)Mathf.Sign(num) * (Mathf.Abs(num2) + 1);
            int num6 = (int)Mathf.Sign(num3) * (Mathf.Abs(num4) + 1);
            Vector2 hexPosByIndex = HexagonImplement.GetHexPosByIndex(num4, num2);
            Vector2 hexPosByIndex2 = HexagonImplement.GetHexPosByIndex(num6, num2);
            Vector2 hexPosByIndex3 = HexagonImplement.GetHexPosByIndex(num4, num5);
            Vector2 hexPosByIndex4 = HexagonImplement.GetHexPosByIndex(num6, num5);
            float num7 = HexagonImplement.SquareDistance(pos, hexPosByIndex);
            float num8 = HexagonImplement.SquareDistance(pos, hexPosByIndex2);
            float num9 = HexagonImplement.SquareDistance(pos, hexPosByIndex3);
            float num10 = HexagonImplement.SquareDistance(pos, hexPosByIndex4);
            nCol = num2;
            nRow = num4;
            float num11 = num7;
            if (num8 < num11)
            {
                nCol = num2;
                nRow = num6;
                num11 = num8;
            }
            if (num9 < num11)
            {
                nCol = num5;
                nRow = num4;
                num11 = num9;
            }
            if (num10 < num11)
            {
                nCol = num5;
                nRow = num6;
            }
            return true;
        }
        /// <summary>
        /// 计算两平面坐标的距离
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static float SquareDistance(Vector2 pos1, Vector2 pos2)
        {
            return Mathf.Pow(pos1.x - pos2.x, 2f) + Mathf.Pow(pos1.y - pos2.y, 2f);
        }
        #endregion
        #region 私有方法

        #endregion
    }
}