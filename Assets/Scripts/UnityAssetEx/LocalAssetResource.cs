using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using UnityAssetEx.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalAssetResource
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.3
// 模块描述：本地资源管理类
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Local
{
    public class LocalAssetResource : IDisposable, IAssetResource
    {
        //public delegate void AssetLoadFinishedEventHandler(LocalAssetResource resource);5
        #region 变量
        public delegate void LoadComplete(LocalAssetResource assetSO);
        public static Dictionary<string, LocalAssetResource> m_dicAllAssetResource = new Dictionary<string, LocalAssetResource>();
        private DateTime m_dataBeginLoadTime;
        private WWW m_www;
        public UnityEngine.Object m_MainAsset;
        private string m_url = string.Empty;
        private string m_strErrorInfo = string.Empty;
        private ResourceData m_resourceData;
        private AssetLoadFinishedEventHandler handler;
        private bool m_isDone;
        private bool m_bCancel;
        private bool m_bHasCallBacked;
        private bool m_isDispose;
        private bool m_isBeginDownload;
        private int m_RefCount;
        protected EnumAssetType m_assetType;
        private AssetBundle m_AssetBunle;
        protected AssetPRI m_assetPRI = AssetPRI.DownloadPRI_Plain;
        /// <summary>
        /// 已经加载过的资源url
        /// </summary>
        private static HashSet<string> s_hashSetAssetURL = new HashSet<string>();
        /// <summary>
        /// 已经加载过的资源名
        /// </summary>
        private static HashSet<string> s_hashSetAssetName = new HashSet<string>();
        #endregion
        #region 属性

        /// <summary>
        /// url加载地址，既www路径
        /// </summary>
        public string URL
        {
            get { return this.m_url; }
        }
        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsDone 
        {
            get 
            {
                if (this.m_www != null)
                {
                    this.m_isDone = this.m_www.isDone;
                }
                return this.m_isDone;
            }
        }
        /// <summary>
        /// 主资源（UnityEngine.Object类型）
        /// </summary>
        public UnityEngine.Object MainAsset 
        {
            get { return this.m_MainAsset; }   
        }
        /// <summary>
        /// 获得www请求页面的内容
        /// </summary>
        public string Text 
        {
            get 
            {
                if (this.m_www != null)
                {
                    return this.m_www.text;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 获得www请求资源的二进制数组
        /// </summary>
        public virtual byte[] Bytes 
        {
            get 
            {
                if (this.m_www != null)
                {
                    return this.m_www.bytes;
                }
                return null;
            }
        }
        /// <summary>
        /// 获得www的texture资源
        /// </summary>
        public Texture Texture 
        {
            get 
            {
                if (this.m_www != null)
                {
                    return this.m_www.texture;
                }
                return null;
            }
        }
        /// <summary>
        /// 获得www的音频资源
        /// </summary>
        public AudioClip Audio 
        {
            get 
            {
                if (this.m_www != null)
                {
                    return this.m_www.audioClip;
                }
                return null;
            }
        }
        /// <summary>
        /// 加载资源进度
        /// </summary>
        public float progress
        {
            get 
            {
                if (this.m_isDone)
                {
                    return 1f;
                }
                if (this.m_www != null)
                {
                    return this.m_www.progress;
                }
                else 
                {
                    if (this.m_AssetBunle != null)
                    {
                        return 1f;
                    }
                    else 
                    {
                        return 0f;
                    }
                }
            }
        }
        /// <summary>
        /// 加载资源大小
        /// </summary>
        public int Size 
        {
            get 
            {
                if (this.m_resourceData != null)
                {
                    return this.m_resourceData.mSize;
                }
                else 
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error 
        {
            get 
            {
                if (this.m_www != null)
                {
                    return this.m_www.error;
                }
                else 
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 是否下载开始
        /// </summary>
        public bool Started 
        {
            get { return this.m_isBeginDownload; }
        }
        /// <summary>
        /// 该资源被引用的次数
        /// </summary>
        public int RefCount 
        {
            get 
            {
                return this.m_RefCount;
            }
        }
        /// <summary>
        /// 是否取消下载
        /// </summary>
        public bool Canceled 
        {
            get 
            {
                return this.m_bCancel;
            }
        }
        public bool HasCallBack 
        {
            get { return this.m_bHasCallBacked; }
        }
        public EnumAssetType AssetType 
        {
            get { return this.m_assetType; }
            set { this.m_assetType = value; }
        }
        public AssetPRI PRIType 
        {
            get { return this.m_assetPRI; }
            set { this.m_assetPRI = value; }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 主要初始化url，resourceData,isfinished=false
        /// </summary>
        /// <param name="data"></param>
        public LocalAssetResource(ResourceData data)
        {
            if (data == null)
            {
                AssetLogger.Error("null == resourceData");
            }
            this.m_resourceData = data;
            if (this.m_resourceData != null)
            {
                if (this.m_resourceData.mPath.EndsWith(".unity3d"))
                {
                    this.m_url = ResourceManager.GetFullPath(this.m_resourceData.mPath, true);
                }
                else 
                {
                    this.m_url = ResourceManager.GetFullPath(this.m_resourceData.mPath, false);
                }            
            }
            this.m_bCancel = false;
        }
        #endregion
        public AssetLoadFinishedEventHandler GetLoadFinishedHandler()
        {
            return this.handler;
        }
        public void SetLoadFinishedHandler(AssetLoadFinishedEventHandler handler)
        {
            this.handler = handler;
        }
        /// <summary>
        /// 开始加载assetbundle，通过本地url从内存中加载
        /// </summary>
        public void BeginDownload()
        {
            this.m_dataBeginLoadTime = DateTime.Now;
            try
            {
                AssetLogger.Debug("BeginDownLoad:" + this.URL);
                this.m_isBeginDownload = true;
                if (LocalAssetResource.s_hashSetAssetURL.Contains(this.URL))//已经加载过了
                {
                    AssetLogger.Error("s_hashSet.Contains(m_strAssetUrl) == true: " + this.URL);
                    this.m_isDone = true;
                }
                else 
                {
                    string text = Path.GetFileName(this.URL).ToLower();//取得要加载的资源名（小写）
                    if (!LocalAssetResource.s_hashSetAssetName.Add(text))//添加资源名到资源名集合中
                    {
                        AssetLogger.Error(string.Format("s_hashSetAssetName.Add(strAssetName) == false:{0}", text));//添加失败
                    }
                    if (!this.m_url.EndsWith(".unity3d"))
                    {
                        string text2 = this.URL;
                        if (this.URL.IndexOf("file:///") == 0)
                        {
                            text2 = this.URL.Substring(8);
                        }
                        FileInfo fileInfo = new FileInfo(text2);
                        if (fileInfo.Exists)
                        {
                            /*FileStream fileStream = fileInfo.OpenRead();//打开文件流
                            Debug.Log("doload"+fileStream.Length);
                            byte[] array = new byte[fileStream.Length];
                            fileStream.Read(array, 0, (int)fileStream.Length);//读取资源
                            fileStream.Close();
                            this.m_AssetBunle = AssetBundle.CreateFromMemoryImmediate(array);//从内存中创建assetbundleCreateRequest，这个比较占内存，虽然是异步的
                            */
                            this.m_AssetBunle = AssetBundle.CreateFromFile(text2);
                            if (!LocalAssetResource.m_dicAllAssetResource.ContainsKey(this.m_url))
                            {
                                LocalAssetResource.m_dicAllAssetResource.Add(this.m_url, this);
                                if (LocalAssetResource.m_dicAllAssetResource.Count == 200)
                                {
                                    AssetLogger.Debug(string.Format("AssetBundle count Exceed 200, it is dangerous!", new object[0]));
                                }
                            }
                            this.m_isDone = true;
                        }
                        else
                        {
                            //不存在文件资源，报错
                            AssetLogger.Error(string.Format("fileInfo.Exists == false:{0}", text2));
                            this.m_isDone = true;
                        }
                    }
                    else
                    {
                        //如果平台不是windows，那么就用www来加载资源
                        AssetLogger.Debug("Scene: BeginDownLoad:" + this.URL);
                        this.m_www = new WWW(this.URL);
                    }
                    LocalAssetResource.s_hashSetAssetURL.Add(this.URL);//加载完成之后，把资源的url添加到url集合中
                }
            }
            catch (Exception e)
            {
                AssetLogger.Fatal(e.ToString());
                this.m_isDone = true;
            }
        }
        /// <summary>
        /// 加载主资源MainAsset
        /// </summary>
        public void LoadMainAsset()
        {
            try
            {
                if (this.IsDone && this.m_AssetBunle != null)//如果assetbundle已经加载完成
                {
                    this.m_MainAsset = this.m_AssetBunle.mainAsset;//取得assetbundle的主资源
                    Debug.Log(this.m_MainAsset.name);
                    /*if (!this.m_MainAsset.name.Contains("Backdrop") && !this.m_MainAsset.name.Contains("Fantasy Atlast") && !this.m_MainAsset.name.Contains("msyha") && !this.m_MainAsset.name.Contains("Fantasy Atlasm") && !this.m_MainAsset.name.Contains("Unlit/Transparent Colored"))
                    {
                        GameObject a = (GameObject)UnityEngine.Object.Instantiate(this.m_MainAsset);
                    }
                    */
                }
            }
            catch (OutOfMemoryException ex2)
            {
                AssetLogger.Fatal(ex2.ToString());
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
            catch (Exception ex1)
            {
                AssetLogger.Fatal("AssetEx LoadAll Exception: " + ex1.ToString());
            }
        }
        /// <summary>
        /// 卸载资源（只是卸载assetbundle本身的内存镜像，不包含已经load进去的asset资源）
        /// </summary>
        public void UnloadAsset()
        {
            if (this.m_AssetBunle != null && (this.m_resourceData == null || this.m_resourceData.mRefCount <= 1))
            {
                if (LocalAssetResource.m_dicAllAssetResource.ContainsKey(this.m_url))
                {
                    LocalAssetResource.m_dicAllAssetResource.Remove(this.m_url);
                }
                this.m_AssetBunle.Unload(false);
                this.m_AssetBunle = null;
            }
        }
        /// <summary>
        /// 增加被引用次数
        /// </summary>
        public void AddRef()
        {
            this.m_RefCount++;
            AssetLogger.Debug(string.Format("AddRef[{0}]:{1}", this.m_RefCount, this.URL));
        }
        public void DelRef()
        {
            this.m_RefCount--;
            if (0 >= this.m_RefCount)
            {
                this.m_RefCount = 0;
            }
            AssetLogger.Debug(string.Format("DelRef[{0}]:{1}", this.m_RefCount, this.URL));
        }
        /// <summary>
        /// 释放已经加载的资源
        /// </summary>
        public void Dispose()
        {
            Debug.Log("2");
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放已经加载的资源
        /// </summary>
        /// <param name="value"></param>
        protected void Dispose(bool value)
        {
            //如果已经释放了，就说明也不做
            if (this.m_isDispose)
            {
                return;
            }
            if (value)
            {
                LocalAssetResource.s_hashSetAssetURL.Remove(this.URL);
                string text = Path.GetFileName(this.URL.ToLower());//取得指定url路径的文件名+后缀名
                if (!LocalAssetResource.s_hashSetAssetName.Remove(text))//移除失败
                {
                    AssetLogger.Error("s_hashSetAssetName.Remove(strAssetName) == false: " + text);
                }
                if (this.m_AssetBunle != null)
                {
                    if (LocalAssetResource.m_dicAllAssetResource.ContainsKey(this.m_url))
                    {
                        LocalAssetResource.m_dicAllAssetResource.Remove(this.m_url);
                    }
                    Debug.Log("Assetbundle卸载完全");
                    this.m_AssetBunle.Unload(true);//卸载assetbundle上所有的资源
                    this.m_AssetBunle = null;
                    this.m_MainAsset = null;//设置MainAsset为null，过段时间会被回收
                    AssetLogger.Debug(string.Format("Unload:[{0}]", this.URL));//卸载成功
                }
                else 
                {
                    if (this.m_MainAsset != null)
                    {
                        if (this.m_MainAsset is GameObject || this.m_MainAsset is Component)
                        {
                            UnityEngine.Object.DestroyImmediate(this.m_MainAsset, true);
                        }
                    }
                    else 
                    {
                        Resources.UnloadAsset(this.m_MainAsset);
                    }
                    this.m_MainAsset = null;
                }
                if (this.m_www != null)
                {
                    this.m_www.Dispose();//释放www的资源
                    this.m_www = null;
                }
                this.m_bCancel = true;
            }
            this.m_isDispose = true;//设置释放成功
        }
        /// <summary>
        /// 本地资源加载完成之后处理
        /// </summary>
        public void OnLoaded()
        {
            if (this.m_assetType <= EnumAssetType.eAssetType_AssetBundleFont || this.m_assetType == EnumAssetType.eAssetType_Scene)
            {
                if (this.m_www != null)
                {
                    this.m_AssetBunle = this.m_www.assetBundle;
                    this.m_www.Dispose();
                    this.m_www = null;
                }
            }
            TimeSpan timeSpan = DateTime.Now - this.m_dataBeginLoadTime;
            try
            {
                if (!this.m_bHasCallBacked)
                {
                    this.m_bHasCallBacked = true;
                    if (this.GetLoadFinishedHandler() != null)
                    {
                        this.GetLoadFinishedHandler()(this);
                    }
                }
            }
            catch (Exception e)
            {
                AssetLogger.Fatal(e.ToString());
            }

        }
        ~LocalAssetResource()
        {
            this.Dispose(false);
        }
    }
}
