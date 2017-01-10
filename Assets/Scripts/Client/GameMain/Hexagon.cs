using UnityEngine;
using System.Collections.Generic;
using Utility.Local;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Hexagon
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Export
{
    public class Hexagon
    {
        #region 字段
        #endregion
        #region 属性
        public static Transform TransformBase 
        {
            get { return HexagonImplement.GetTransform(); }
            set { HexagonImplement.SetTransform(value); }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public static bool FillHexagon(int nRow, int nCol, float fY, ref List<Vector3> listVertex, ref List<int> listVertexIndex, ref List<Vector2> listUV)
        {
            return HexagonImplement.FillHexagon(nRow, nCol, fY,ref listVertex,ref listVertexIndex,ref listUV);
        }
        public static Vector3 GetHex3DPos(CVector3 hexPos, Space space)
        {
            return HexagonImplement.GetHex3DPos(hexPos, space);
        }
        public static Vector3 GetHex3DPosByIndex(int nRow, int nCol, Space space)
        {
            return HexagonImplement.GetHex3DPosByIndex(nRow, nCol, space);
        }
        public static Vector2 GetHexPosByIndex(int nRow, int nCol, Space space)
        {
            return HexagonImplement.GetHexPosByIndex(nRow, nCol, space);
        }
        /// <summary>
        /// 根据真实世界坐标取得格子坐标
        /// </summary>
        /// <param name="realPos"></param>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static bool GetHexIndexByPos(Vector3 realPos, out int nRow, out int nCol)
        {
            return HexagonImplement.GetHexIndexByPos(realPos, out nRow, out nCol);
        }
        #endregion
        #region 私有方法
        #endregion
    }
}