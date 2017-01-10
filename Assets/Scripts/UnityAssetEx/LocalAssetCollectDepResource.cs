using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalAssetCollectDepResource
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：本地带引用资源管理类
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Local
{
    internal class LocalAssetCollectDepResource : IDisposable, IAssetCollectDepResource
    {
        /// <summary>
        /// 当前资源
        /// </summary>
        protected LocalAssetResource m_assetResource;
        /// <summary>
        /// 存储引用的资源
        /// </summary>
        private LocalAssetResource[] m_depAssetResources;
        /// <summary>
        /// 存储加载出错的资源
        /// </summary>
        protected List<LocalAssetResource> m_listErrorAsset = new List<LocalAssetResource>();
        protected float m_fBeginTime = Time.realtimeSinceStartup;
        protected bool m_isRemoveQuickly = false;
        protected bool m_HasCallBack = false;
        private bool m_bDisposed = false;
        private string m_url = string.Empty;
        /// <summary>
        /// 下载请求资源列表集合
        /// </summary>
        private List<LocalAssetRequest> m_listAssetRequest = new List<LocalAssetRequest>();
        #region 属性
        /// <summary>
        /// 下载的url路径
        /// </summary>
        public string URL
        {
            get{return this.m_url;}
            set { this.m_url = value; }
        }
        /// <summary>
        /// 加载的资源AssetResource
        /// </summary>
        public LocalAssetResource AssetResource 
        {
            get { return this.m_assetResource; }
        }
        /// <summary>
        /// 加载出错的资源列表
        /// </summary>
        public List<LocalAssetResource> ErrorAssets
        {
            get { return this.m_listErrorAsset; }
        }
        #endregion
        #region 构造函数
        public LocalAssetCollectDepResource()
        {
            this.m_bDisposed = false;
        }
        ~LocalAssetCollectDepResource()
        {
            this.Dispose(false);
        }
        /// <summary>
        /// 下载请求的数量
        /// </summary>
        public int RefCount 
        {
            get { return this.m_listAssetRequest.Count; }
        }
        /// <summary>
        /// 需要下载的资源数量
        /// </summary>
        public int AssetCount 
        {
            get 
            {
                int count = 0;
                if (this.m_assetResource != null)
                {
                    count++;
                }
                if (this.m_depAssetResources != null)
                {
                    return count + this.m_depAssetResources.Length;
                }
                return count;
            }
        }
        /// <summary>
        /// 已经加载完成的资源数量
        /// </summary>
        public int CompleteCount 
        {
            get 
            {
                int count = 0;
                if (this.m_assetResource != null && this.m_assetResource.IsDone)
                {
                    count++;
                }
                if (this.m_depAssetResources != null)
                {
                    for (int i=0; i<this.m_depAssetResources.Length; i++)
                    {
                        LocalAssetResource asset = this.m_depAssetResources[i];
                        if (asset != null && asset.IsDone)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }
        #endregion
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        public static LocalAssetCollectDepResource Create()
        {
            return new LocalAssetCollectDepResource();
        }
        /// <summary>
        /// 取得引用资源数组
        /// </summary>
        /// <returns></returns>
        public LocalAssetResource[] GetDepAssetResource()
        {
            return this.m_depAssetResources;
        }
        /// <summary>
        /// 取得开始加载时间
        /// </summary>
        /// <returns></returns>
        public float GetBeginTime()
        {
            return this.m_fBeginTime;
        }
        /// <summary>
        /// 设置当前资源，初始化assetResource
        /// </summary>
        /// <param name="assetSO"></param>
        public void SetAsset(IAssetResource assetSO) 
        {
            if (this.m_assetResource != null)
            {
                AssetLogger.Error(string.Format("null != m_assetResource:{0}", this.GetAssetResourceStates()));
                return;
            }
            if (assetSO != null)
            {
                this.m_assetResource = assetSO as LocalAssetResource;
                this.m_assetResource.AddRef();
            }
        }
        /// <summary>
        /// 设置引用资源的数目，初始化depAssetResource[]数组
        /// </summary>
        /// <param name="length"></param>
        public void SetDepSize(int length) 
        {
            if (length > 0)
            {
                if (this.m_depAssetResources != null)
                {
                    AssetLogger.Error("this.m_depAssetResource != null");
                }
                this.m_depAssetResources = new LocalAssetResource[length];
            }
        }
        /// <summary>
        /// 添加引用资源，赋值depAssetResource[]
        /// </summary>
        /// <param name="depSO">需要添加的资源</param>
        /// <param name="index">添加到哪个位置</param>
        public void AddDep(IAssetResource depSO, int index)
        {
            LocalAssetResource assetResource = depSO as LocalAssetResource;
            if (assetResource != null && this.m_depAssetResources != null && index < this.m_depAssetResources.Length && index >= 0 && !this.HasAssetResource(assetResource))
            {
                this.m_depAssetResources[index] = assetResource;
                assetResource.AddRef();
            }
        }
        /// <summary>
        /// 资源加载完成之后的委托，主要是检测出加载出错的资源，然后设置finished为true，然后加载主资源，添加委托，卸载assetbundle
        /// </summary>
        /// <param name="resource"></param>
        public void AssetComplete(IAssetResource resource)
        {
            this.DetectComplete();
        }
        public void EndCreate()
        {
            this.DetectComplete();
        }
        public void DetectComplete()
        {
            if (!this.m_HasCallBack && this.DetectAllHasFinished())
            {
                if (this.m_assetResource != null)
                {
                    AssetLogger.Debug("Asset: DetectComplete:" + this.m_assetResource.URL);
                }
                else 
                {
                    AssetLogger.Error("Asset: DetectComplete: null == m_assetResource");
                }
                this.m_HasCallBack = true;//设置加载完成
                this.DebugError();//打印出出错的资源
                this.LoadMainAsset();
                /*Debug.Log("Main:" + this.m_assetResource.MainAsset);
                for (int j = 0; j < this.m_depAssetResources.Length; j++)
                {
                    Debug.Log("dep:" + this.m_depAssetResources[j].MainAsset);
                }
                 */
                try
                {                 
                    for (int i = 0; i < this.m_listAssetRequest.Count; i++)
                    {
                        LocalAssetRequest request = this.m_listAssetRequest[i];
                        request.OnAssetRequestFinishedHandler(this.m_assetResource);//执行资源加载完成之后的委托
                    }
                }
                catch(Exception e) 
                {
                    AssetLogger.Fatal(e.ToString());
                }
                this.UnloadAssetBundle();
            }
        }
        /// <summary>
        /// 打印出错资源的信息
        /// </summary>
        public void DebugError()
        {
            foreach (var current in this.m_listErrorAsset)
            {
                AssetLogger.Error(current.URL + "has some error:" + current.Error);
            }
        }
        /// <summary>
        /// 加载带有引用资源的资源（包括自身和引用资源）的主资源
        /// </summary>
        private void LoadMainAsset()
        {
            if (this.m_depAssetResources != null)
            {
                for (int i = 0; i < this.m_depAssetResources.Length; i++)
                {
                    LocalAssetResource resource = this.m_depAssetResources[i];
                    if (resource != null)
                    {
                        resource.LoadMainAsset();//加载引用资源的主资源
                    }
                }
            }
            if (this.m_assetResource != null)
            {
                this.m_assetResource.LoadMainAsset();//加载当前资源的主资源
            }
        }
        /// <summary>
        /// 卸载Assetbundle内存镜像引用
        /// </summary>
        private void UnloadAssetBundle()
        {
            if (this.m_depAssetResources != null)
            {
                for (int i = 0; i < this.m_depAssetResources.Length; i++)
                {
                    LocalAssetResource resource = this.m_depAssetResources[i];
                    if (resource != null)
                    {
                        resource.UnloadAsset();
                    }
                }
            }
            if (this.m_assetResource != null)
            {
                this.m_assetResource.UnloadAsset();
            }
        }
        /// <summary>
        /// 检测资源是否下载出错
        /// </summary>
        /// <returns></returns>
        private bool DetectAllHasFinished()
        {
            if (this.m_assetResource != null)
            {
                if (!this.m_assetResource.HasCallBack)//如果完成的话，就检测资源是否下载出错
                {
                    return false;
                }
                this.DeceteResourceError(this.m_assetResource);
            }
            if (this.m_depAssetResources != null)
            {
                for (int i = 0; i < this.m_depAssetResources.Length; i++)
                {
                    LocalAssetResource resource = this.m_depAssetResources[i];
                    if (resource != null)
                    {
                        if (!resource.HasCallBack)
                        {
                            return false;
                        }
                        this.DeceteResourceError(resource);
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 检测哪些资源在加载是出错，就添加到listAssetResource集合里面去
        /// </summary>
        /// <param name="resource"></param>
        private void DeceteResourceError(LocalAssetResource resource)
        {
            if (resource != null)
            {
                if (string.IsNullOrEmpty(resource.Error))//如果资源下载没出错的话
                {
                    if (this.m_listErrorAsset.Contains(resource))//如果存在的话，表示第一次下载出错，第二次正确，就移除
                    {
                        this.m_listErrorAsset.Remove(resource);
                        return;
                    }
                }
                else 
                {
                    if (!this.m_listErrorAsset.Contains(resource))
                    {
                        this.m_listErrorAsset.Add(resource);
                    }
                }
            }
        }
        public void RemoveAssetRequest(LocalAssetRequest request)
        {
            if (!this.m_listAssetRequest.Remove(request))
            {
                AssetLogger.Error("false == m_listAssetRequest.Remove(assetRequest):");
            }
        }
        public void AddAssetRequest(LocalAssetRequest request)
        {
            this.m_listAssetRequest.Add(request);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            Debug.Log("Dispose");
            if (!this.m_bDisposed)
            {
                if (disposing)
                {
                    Clear();
                }
                this.m_bDisposed = true;
            }
        }
        private void Clear()
        {
            if (this.m_assetResource != null)
            {
                this.m_assetResource.DelRef();
                this.m_assetResource = null;
            }
            if (this.m_depAssetResources != null)
            {
                for (int i = 0; i < this.m_depAssetResources.Length; i++)
                {
                    LocalAssetResource assetResource = this.m_depAssetResources[i];
                    if (assetResource != null)
                    {
                        Debug.Log("dep :"+assetResource.URL+"delref:"+assetResource.RefCount);
                        assetResource.DelRef();
                    }
                }
                this.m_depAssetResources = null;
            }
        }
        public string GetAssetResourceStates()
        {
            StringBuilder stringBuilder = new StringBuilder(128);
            if (this.m_assetResource != null)
            {
                stringBuilder.AppendFormat("{0}:{1}.{2}", this.m_assetResource.URL, this.m_assetResource.IsDone, this.m_assetResource.Error);
            }
            if (this.m_depAssetResources != null)
            {
                for (int i = 0; i < this.m_depAssetResources.Length; i++)
                {
                    LocalAssetResource assetResource = this.m_depAssetResources[i];
                    if (assetResource != null)
                    {
                        stringBuilder.AppendFormat("{0}:{1}.{2}", assetResource.URL, assetResource.IsDone, assetResource.Error);
                    }
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 获得要下载的资源
        /// </summary>
        /// <returns></returns>
        public LocalAssetResource GetAssetResource()
        {
            return this.m_assetResource;
        }
        public bool HasCallBack()
        {
            return this.m_HasCallBack;
        }
        /// <summary>
        /// 查看引用资源数组里面是否已经包含该资源
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool HasAssetResource(LocalAssetResource resource)
        {
            if (this.m_assetResource == resource)
            {
                return true;
            }
            if (this.m_depAssetResources != null)
            {
                for (int i = 0; i < this.m_depAssetResources.Length; i++)
                {
                    LocalAssetResource depResource = this.m_depAssetResources[i];
                    if (depResource == resource)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void SetIsRemoveQuickly(bool value)
        {
            this.m_isRemoveQuickly = value;
        }
        public bool GetRemoveQuickly()
        {
            return this.m_isRemoveQuickly;
        }
    }
}
