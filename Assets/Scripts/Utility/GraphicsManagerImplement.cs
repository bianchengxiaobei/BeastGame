using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GraphicsManagerImplement
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility
{
    internal class GraphicsManagerImplement
    {
        private static GraphicsQuality s_GraphicsQuality;
        private static bool s_RealtimeShadows;
        private static int REDUCE_MAX_WINDOW_SIZE;
        private static List<IGraphicsResolution> s_fullScreenResolutions;
        private static readonly List<IGraphicsResolution> s_resolutions;
        public static GraphicsQuality RenderQualityLevel
        {
            get
            {
                return GraphicsManagerImplement.s_GraphicsQuality;
            }
            set
            {
                GraphicsManagerImplement.s_GraphicsQuality = value;
            }
        }
        public static IGraphicsResolution Default
        {
            get
            {
                IGraphicsResolution result;
                if (GraphicsManagerImplement.GoodGraphicsResolution.Count > 0)
                {
                    result = GraphicsManagerImplement.GoodGraphicsResolution[GraphicsManagerImplement.GoodGraphicsResolution.Count / 2];
                }
                else
                {
                    result = new GraphicsResolution(1024, 768);
                }
                return result;
            }
        }
        public static List<IGraphicsResolution> GoodGraphicsResolution
        {
            get
            {
                if (GraphicsManagerImplement.s_fullScreenResolutions.Count == 0)
                {
                    foreach (IGraphicsResolution current in GraphicsManagerImplement.ListAllResolutions)
                    {
                        Debug.Log(current.Width +"X"+ current.Height);
                        //if ((double)current.aspectRatio - 0.01 <= 1.7777777777777777 && (double)current.aspectRatio + 0.01 >= 1.3333333333333333 && current.Height >= 600)
                        //{
                            GraphicsManagerImplement.s_fullScreenResolutions.Add(current);
                        //}
                    }
                }
                return GraphicsManagerImplement.s_fullScreenResolutions;
            }
        }
        public static List<IGraphicsResolution> ListAllResolutions
        {
            get
            {
                if (GraphicsManagerImplement.s_resolutions.Count == 0)
                {
                    List<IGraphicsResolution> list = GraphicsManagerImplement.s_resolutions;
                    List<IGraphicsResolution> obj;
                    Monitor.Enter(obj = list);
                    try
                    {
                        Resolution[] resolutions = Screen.resolutions;
                        for (int i = 0; i < resolutions.Length; i++)
                        {
                            Resolution resolution = resolutions[i];
                            GraphicsManagerImplement.Add(resolution.width, resolution.height);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(obj);
                    }
                }
                return GraphicsManagerImplement.s_resolutions;
            }
        }
        static GraphicsManagerImplement()
        {
            GraphicsManagerImplement.REDUCE_MAX_WINDOW_SIZE = 80;
            GraphicsManagerImplement.s_fullScreenResolutions = new List<IGraphicsResolution>();
            GraphicsManagerImplement.s_resolutions = new List<IGraphicsResolution>();
        }
        public static IGraphicsResolution CreateResolution(int nWidth, int nHeight)
        {
            return new GraphicsResolution(nWidth, nHeight);
        }
        private static bool Add(int width, int height)
        {
            IGraphicsResolution item = GraphicsManagerImplement.CreateResolution(width, height);
            bool result;
            if (GraphicsManagerImplement.s_resolutions.BinarySearch(item) >= 0)
            {
                result = false;
            }
            else
            {
                GraphicsManagerImplement.s_resolutions.Add(item);
                GraphicsManagerImplement.s_resolutions.Sort();
                result = true;
            }
            return result;
        }
        private static void SetQualityByName(string qualityName)
        {
        }
        public static bool SetScreenResolution(ref int width, ref int height, bool fullscreen)
        {
            if (!fullscreen)
            {
                bool flag = false;
                if (width > Screen.currentResolution.width)
                {
                    width = Screen.currentResolution.width;
                    flag = true;
                }
                if (height > Screen.currentResolution.height)
                {
                    height = Screen.currentResolution.height;
                    flag = true;
                }
                if (flag)
                {
                    float num = (float)width / (float)height;
                    if ((double)num > 1.77777777777778)
                    {
                        width = (int)((double)height * 1.77777777777778);
                    }
                    else
                    {
                        if ((double)num < 1.3333333333333333)
                        {
                            height = (int)((double)width * 1.3333333333333333);
                        }
                    }
                }
            }
            Screen.SetResolution(width, height, fullscreen);
            return true;
        }
    }
}
