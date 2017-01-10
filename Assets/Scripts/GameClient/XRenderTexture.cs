using UnityEngine;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XRenderTexture
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient
{
    public class XRenderTexture
    {
        private RenderTexture m_renderTexture = null;
        private Camera m_camera = null;
        private int m_nWidth = 0;
        private int m_nHeight = 0;
        private IXLog m_log = XLog.GetLog<XRenderTexture>();
        public RenderTexture RenderTexture
        {
            get
            {
                return this.m_renderTexture;
            }
        }
        public int Width
        {
            get
            {
                return this.m_nWidth;
            }
        }
        public int Height
        {
            get
            {
                return this.m_nHeight;
            }
        }
        public XRenderTexture(Camera camera, int nWidth, int nHeight)
        {
            if (null == camera)
            {
                this.m_log.Error("null == camera");
            }
            this.m_camera = camera;
            this.m_nWidth = nWidth;
            this.m_nHeight = nHeight;
            this.m_renderTexture = new RenderTexture(nWidth, nHeight, 16);
            this.m_renderTexture.hideFlags = HideFlags.DontSave;
        }
        public void Rebuild(int nWidth, int nHeight)
        {
            UnityEngine.Object.Destroy(this.m_renderTexture);
            this.m_renderTexture = new RenderTexture(nWidth, nHeight, 16);
            this.m_renderTexture.hideFlags = HideFlags.DontSave;
        }
    }
}
