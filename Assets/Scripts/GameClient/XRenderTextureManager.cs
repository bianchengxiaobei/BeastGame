using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XRenderTextureManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient
{
    internal class XRenderTextureManager : Singleton<XRenderTextureManager>
    {
        private IXLog m_log = XLog.GetLog<XRenderTextureManager>();
        private Dictionary<EnumRenderTextureType, XRenderTexture> m_dicRenderTexture = new Dictionary<EnumRenderTextureType, XRenderTexture>();
        public XRenderTexture GetRenderTexture(EnumRenderTextureType eRenderTextureType)
        {
            XRenderTexture xRenderTexture = null;
            XRenderTexture result;
            if (this.m_dicRenderTexture.TryGetValue(eRenderTextureType, out xRenderTexture))
            {
                result = xRenderTexture;
            }
            else
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 创建渲染贴图，加入到dicRendertexture里面
        /// </summary>
        /// <param name="eRenderTextureType"></param>
        /// <param name="camera"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        public void CreateXRenderTexture(EnumRenderTextureType eRenderTextureType, Camera camera, int nWidth, int nHeight)
        {
            if (this.m_dicRenderTexture.ContainsKey(eRenderTextureType))
            {
                this.m_log.Error(string.Format("m_dicRenderTexture.ContainsKey(eRenderTextureType) == true:{0}", eRenderTextureType.ToString()));
            }
            else
            {
                XRenderTexture value = new XRenderTexture(camera, nWidth, nHeight);
                this.m_dicRenderTexture.Add(eRenderTextureType, value);
            }
        }
        public void RebuildAllXRenderTextures(int nWidth, int nHeight)
        {
            foreach (XRenderTexture current in this.m_dicRenderTexture.Values)
            {
                current.Rebuild(nWidth, nHeight);
            }
        }
        public void Clear()
        {
        }
    }
    internal enum EnumRenderTextureType
    {
        eRenderTextureType_OutLine,
        eRenderTextureType_UIBackGround,
        eRenderTextureType_Max
    }
}
