using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using GameClient;
using Utility;
using Utility.Export;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ScreenManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：屏幕分辨率管理器
//----------------------------------------------------------------*/
#endregion
namespace Client
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        private IXLog m_log = XLog.GetLog<ScreenManager>();
        /// <summary>
        /// 现在的宽度
        /// </summary>
        public int CurrentWidth
        {
            get;
            private set;
        }
        /// <summary>
        /// 现在的高度
        /// </summary>
        public int CurrentHeight
        {
            get;
            private set;
        }
        /// <summary>
        /// 初始化屏幕分辨率，从UserOption里面读取数据
        /// </summary>
        public void Awake()
        {
            this.m_log.Debug("ScreenManager.Awake()");
            if (!CommonDefine.IsMobilePlatform)//如果不是手机平台
            {
                int width = GraphicsManager.Default.Width;//默认为屏幕支持的分辨率的中间
                int height = GraphicsManager.Default.Height;
                EnumDisplayMode enumDisplayMode = EnumDisplayMode.eDisplayMode_Window;//默认为窗口模式
                if (UserOptions.Singleton.HasSet)//如果用户设置过的话，就采取用户设置的
                {
                    width = UserOptions.Singleton.Resolution.Width;
                    height = UserOptions.Singleton.Resolution.Height;
                    enumDisplayMode = (EnumDisplayMode)UserOptions.Singleton.DisplayMode;
                    if (enumDisplayMode == EnumDisplayMode.eDisplayMode_FullAndWindow)
                    {
                        enumDisplayMode = EnumDisplayMode.eDisplayMode_FullScreen;
                        UserOptions.Singleton.DisplayMode = (int)enumDisplayMode;
                    }
                }
                else 
                {
                    UserOptions.Singleton.DisplayMode = (int)enumDisplayMode;
                    UserOptions.Singleton.Resolution = GraphicsManager.CreateResolution(width, height);
                }
                this.SetResolution(width, height, enumDisplayMode);
            }
        }
        /// <summary>
        /// 初始化，设置fps为60
        /// </summary>
        public void Init()
        {
            
        }
        /// <summary>
        /// 设置游戏分辨率，如果显示模式为全屏，无论改width，height都是全屏
        /// 通知UIManager和XRenderTextureManager分辨率改了
        /// </summary>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="eDisplayMode"></param>
        public void SetResolution(int nWidth, int nHeight, EnumDisplayMode eDisplayMode)
        {
            if (eDisplayMode == EnumDisplayMode.eDisplayMode_FullScreen)
            {
                if (GraphicsManager.GoodGraphicsResolution.Count > 0)
                {
                    IGraphicsResolution graphicsResolution = GraphicsManager.GoodGraphicsResolution[GraphicsManager.GoodGraphicsResolution.Count - 1];
                    nWidth = graphicsResolution.Width;
                    nHeight = graphicsResolution.Height;
                }
            }
            this.SetResolution(nWidth, nHeight, eDisplayMode == EnumDisplayMode.eDisplayMode_FullScreen);
        }
        private void SetResolution(int nWidth, int nHeight, bool bFullScreen)
        {
            this.m_log.Debug(string.Format("SetResolution:nWidth={0}, nHeight={1}, bFullScreen={2}", nWidth, nHeight, bFullScreen));
            bool flag = GraphicsManager.SetScreenResolution(ref nWidth, ref nHeight, bFullScreen);
            if (flag)
            {
                this.CurrentWidth = nWidth;
                this.CurrentHeight = nHeight;
                this.m_log.Info("RebuildAllXRenderTextures");
                Singleton<XRenderTextureManager>.singleton.RebuildAllXRenderTextures(nWidth, nHeight);
                UIManager.singleton.OnResolutionChange();
            }
        }
    }
}
