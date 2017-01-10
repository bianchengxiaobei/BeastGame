using UnityEngine;
using System.Xml;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BezierControl
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect
{
    internal class BezierControl
    {
        #region 字段
        private float p1x;
        private float p1y;
        private float p2x;
        private float p2y;
        private float angle;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public BezierMoveInfo GetMoveControl(Vector3 p0, Vector3 p3)
        {
            BezierMoveInfo bezierMoveInfo = new BezierMoveInfo();
            bezierMoveInfo._p0 = p0;
            bezierMoveInfo._p3 = p3;
            float d = Vector3.Magnitude(p0 - p3);
            Vector3 vector = p3 - p0;
            Vector3 rhs = Vector3.Cross(Vector3.up,vector);
            Vector3 vector2 = Vector3.Cross(vector, rhs);
            Quaternion rotation = Quaternion.AngleAxis(this.angle, vector);
            vector2 = rotation * vector2;
            vector = Vector3.Normalize(vector);
            vector2 = Vector3.Normalize(vector2);
            bezierMoveInfo._p1 = p0 + vector * this.p1x * d + vector2 * this.p1y * d;
            bezierMoveInfo._p2 = p0 + vector * this.p2x * d + vector2 * this.p2y * d;
            return bezierMoveInfo;
        }

        public bool Load(XmlNode NodeInfo)
		{
			bool result;
			if (null == NodeInfo)
			{
				result = false;
			}
			else
			{
				foreach (XmlNode xmlNode in NodeInfo.ChildNodes)
				{
					string text = xmlNode.Name.ToLower();
					if (text != null)
					{
						if (!(text == "p1x"))
						{
							if (!(text == "p2x"))
							{
								if (!(text == "p1y"))
								{
									if (!(text == "p2y"))
									{
										if (text == "angle")
										{
											this.angle = (float)Convert.ToDouble(xmlNode.InnerText);
										}
									}
									else
									{
										this.p2y = (float)Convert.ToDouble(xmlNode.InnerText);
									}
								}
								else
								{
									this.p1y = (float)Convert.ToDouble(xmlNode.InnerText);
								}
							}
							else
							{
								this.p2x = (float)Convert.ToDouble(xmlNode.InnerText);
							}
						}
						else
						{
							this.p1x = (float)Convert.ToDouble(xmlNode.InnerText);
						}
					}
				}
				result = true;
			}
			return result;
		}	
        #endregion
        #region 私有方法
        #endregion
    }
}