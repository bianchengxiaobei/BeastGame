using UnityEngine;
using System;
using System.Collections;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalAssetRequest
// 创建者：chen
// 修改者列表：
// 创建日期：2015.12.24
// 模块描述：本地下载资源管理类
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Local
{
    public class LocalAssetRequest : IDisposable , IAssetRequest
    {
        private bool m_isFinished;
        private bool m_isErroe;
        private object m_data;
        private bool m_isRemoveQuickly;
        private bool m_isDispose;
        private LocalAssetCollectDepResource m_assetCollectDepResource;
        private AssetRequestFinishedEventHandler handler;
        public IAssetResource AssetResource
        {
            get 
            {
                if (this.m_assetCollectDepResource != null)
                {
                    return this.m_assetCollectDepResource.GetAssetResource();
                }
                return null;
            }
        }
        public bool IsError
        {
            get { return this.m_isErroe; }
        }
        public bool IsFinished
        {
            get { return this.m_isFinished; }
        }
        public object Data
        {
            get { return this.m_data; }
            set { this.m_data = value; }
        }
        public bool RemoveQuickly
        {
            get { return this.m_isRemoveQuickly; }
            set 
            {
                this.m_isRemoveQuickly = value;
                this.m_assetCollectDepResource.SetIsRemoveQuickly(value);
            }
        }
        #region 构造函数
        /// <summary>
        /// 构造函数，初始化需要下载的资源和下载完成之后的委托回调
        /// </summary>
        /// <param name="assetCollectDepResource"></param>
        /// <param name="handler"></param>
        public LocalAssetRequest(IAssetCollectDepResource assetCollectDepResource, AssetRequestFinishedEventHandler handler)
        {
            LocalAssetCollectDepResource colldepResource = assetCollectDepResource as LocalAssetCollectDepResource;
            if (colldepResource == null || colldepResource.GetAssetResource() == null)
            {
                this.m_isErroe = true;
                return;
            }
            this.handler = handler;
            this.m_assetCollectDepResource = colldepResource;
            this.m_assetCollectDepResource.AddAssetRequest(this);
            if (handler != null && this.m_assetCollectDepResource.HasCallBack())
            {
                LocalResourceManager.GetInstance().StartCoroutine(this.DelayCallBack(handler,this));
            }
        }
        #endregion
        public void Dispose()
        {
            this.RemoveQuickly = true;
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool value)
        {
            if (this.m_isDispose)
            {
                return;
            }
            if (value || this.m_assetCollectDepResource != null)
            {
                Debug.Log("AssetRequest Dispose");
                this.m_assetCollectDepResource.RemoveAssetRequest(this);
                this.m_assetCollectDepResource = null;
                this.handler = null;
            }
            this.m_isDispose = true;
        }
        public void OnAssetRequestFinishedHandler(IAssetResource request)
        {
            this.m_isFinished = true;
            if (this.handler != null)
            {
                this.handler(this);
            }
        }
        ~LocalAssetRequest()
        {
            this.Dispose(false);
        }
        /// <summary>
        /// 暂停0.01秒之后执行委托
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private IEnumerator DelayCallBack(AssetRequestFinishedEventHandler eventHandler, IAssetRequest request)
        {
            yield return new WaitForSeconds(0.01f);
            this.m_isFinished = true;
            eventHandler(request);
            yield break;
        }
    }
}
