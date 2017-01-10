using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BezierMoveInfo
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect
{
    internal class BezierMoveInfo
    {
        #region 字段
        public Vector3 _p0 = Vector3.zero;
        public Vector3 _p1 = Vector3.zero;
        public Vector3 _p2 = Vector3.zero;
        public Vector3 _p3 = Vector3.zero;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        /// <summary>
        /// 获得曲线的下一个点
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetNextPos(float t)
        {
            return this._p0 * (1f - t) * (1f - t) * (1f - t) + 3f * t * (1f - t) * (1f - t) * this._p1 + 3f * t * t * (1f - t) * this._p2 + t * t * t * this._p3;
        }
        public Vector3 GetNextDir(float t)
        {
            return -3f * this._p0 * (1 - t) * (1 - t) + 3f * t * (1 - t) * (1 - t) * this._p1 + 3f * t * t * (1f - t) * this._p2 + t * t * t * this._p3;
        }
        #endregion
        #region 私有方法
        #endregion
    }
}