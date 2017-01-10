using UnityEngine;
using System.Collections;
using System;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GraphicsResolution
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility
{
    public class GraphicsResolution : IGraphicsResolution, IComparable
    {
        public float aspectRatio
        {
            get;
            private set;
        }
        public static GraphicsResolution current
        {
            get
            {
                return GraphicsResolution.Create(Screen.currentResolution);
            }
        }
        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }
        public GraphicsResolution()
        {
        }
        public GraphicsResolution(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.aspectRatio = (float)this.Width / (float)this.Height;
        }
        public int CompareTo(object obj)
        {
            GraphicsResolution graphicsResolution = obj as GraphicsResolution;
            int result;
            if (graphicsResolution == null)
            {
                result = 1;
            }
            else
            {
                if (this.Width < graphicsResolution.Width)
                {
                    result = -1;
                }
                else
                {
                    if (this.Width > graphicsResolution.Width)
                    {
                        result = 1;
                    }
                    else
                    {
                        if (this.Height < graphicsResolution.Height)
                        {
                            result = -1;
                        }
                        else
                        {
                            if (this.Height > graphicsResolution.Height)
                            {
                                result = 1;
                            }
                            else
                            {
                                result = 0;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public static GraphicsResolution Create(Resolution res)
        {
            return new GraphicsResolution(res.width, res.height);
        }
        public static GraphicsResolution Create(int width, int height)
        {
            return new GraphicsResolution(width, height);
        }
        public override bool Equals(object obj)
        {
            bool result;
            if (obj == null)
            {
                result = false;
            }
            else
            {
                GraphicsResolution graphicsResolution = obj as GraphicsResolution;
                result = (graphicsResolution != null && this.Width == graphicsResolution.Width && this.Height == graphicsResolution.Height);
            }
            return result;
        }
        public override int GetHashCode()
        {
            int num = 23;
            num = num * 17 + this.Width.GetHashCode();
            return num * 17 + this.Height.GetHashCode();
        }
    }
}
