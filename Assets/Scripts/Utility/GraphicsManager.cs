using UnityEngine;
using System;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GraphicsManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Export
{
    public class GraphicsManager
    {
        /// <summary>
        /// 画质等级，分高、中、低
        /// </summary>
        public static GraphicsQuality RenderQualityLevel
        {
            get
            {
                return GraphicsManagerImplement.RenderQualityLevel;
            }
            set
            {
                GraphicsManagerImplement.RenderQualityLevel = value;
            }
        }
        /// <summary>
        /// 默认分辨率为你屏幕的分辨率
        /// </summary>
        public static IGraphicsResolution Default
        {
            get
            {
                return GraphicsManagerImplement.Default;
            }
        }
        /// <summary>
        /// 高屏幕分辨率
        /// </summary>
        public static List<IGraphicsResolution> GoodGraphicsResolution
        {
            get
            {
                return GraphicsManagerImplement.GoodGraphicsResolution;
            }
        }
        /// <summary>
        /// 所有你的电脑支持的分辨率
        /// </summary>
        public static List<IGraphicsResolution> ListAllResolutions
        {
            get
            {
                return GraphicsManagerImplement.ListAllResolutions;
            }
        }
        public static bool SetScreenResolution(ref int width, ref int height, bool fullscreen)
        {
            return GraphicsManagerImplement.SetScreenResolution(ref width, ref height, fullscreen);
        }
        public static IGraphicsResolution CreateResolution(int nWidth, int nHeight)
        {
            return GraphicsManagerImplement.CreateResolution(nWidth, nHeight);
        }
    }
    public enum GraphicsQuality
    {
        Low,
        Medium,
        High
    }
    public interface IGraphicsResolution : IComparable
    {
        float aspectRatio
        {
            get;
        }
        int Width
        {
            get;
        }
        int Height
        {
            get;
        }
    }
}
