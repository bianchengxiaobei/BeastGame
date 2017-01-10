using UnityEngine;
using System;
using System.IO;
using UnityAssetEx.Local;
using UnityPlugin.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ResourceManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.3
// 模块描述：资源加载管理器
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public class ResourceManager
    {
        private IResourceManager m_resourceManager;
        private static ResourceManager s_instance;
        public static ResourceManager singleton
        {
            get
            {
                if (ResourceManager.s_instance == null)
                {
                    ResourceManager.s_instance = new ResourceManager();
                }
                return ResourceManager.s_instance;
            }
        }
        /// <summary>
        /// 加载进度
        /// </summary>
        public float ProgressValue
        {
            get
            {
                if (this.m_resourceManager == null)
                {
                    AssetLogger.Error("null == m_resourceManager");
                    return 0f;
                }
                return this.m_resourceManager.ProgressValue;
            }
        }
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private ResourceManager()
        {
            this.m_resourceManager = ResourceFactory.GetResourceManager();
        }
        /// <summary>
        /// 初始化资源管理器
        /// </summary>
        /// <param name="editorResourceManager"></param>
        public void Init(IResourceManager editorResourceManager)
        {
            if ((RuntimePlatform.WindowsEditor == Application.platform || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer) && editorResourceManager != null)
            {
                this.m_resourceManager = editorResourceManager;
                //this.m_resourceManager = ResourceFactory.GetResourceManager();
            }
            if (this.m_resourceManager == null)
            {
                this.m_resourceManager = ResourceFactory.GetResourceManager();
            }
            if (this.m_resourceManager != null)
            {
                this.m_resourceManager.Init(ResourceManager.GetFullPath("", false), ResourceManager.GetFullPath("", true));
                return;
            }
            AssetLogger.Error("null == m_resourceManager");
        }
        public void Clear()
        {
            if (this.m_resourceManager == null)
            {
                AssetLogger.Error("null == m_resourceManager");
                return;
            }
            this.m_resourceManager.Clear();
        }
        /// <summary>
        /// 通过Resources加载资源
        /// </summary>
        /// <param name="strAssetFile"></param>
        /// <returns></returns>
        public UnityEngine.Object Load(string strAssetFile)
        {
            UnityEngine.Object @object = null;
            if (!string.IsNullOrEmpty(strAssetFile))
            {
                @object = Resources.Load(strAssetFile);
            }
            if (null == @object)
            {
                AssetLogger.Error("null == obj: " + strAssetFile);
            }
            Resources.UnloadUnusedAssets();
            return @object;
        }
        public IAssetRequest CreateAssetRequest(string strCompleteUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType, EnumAssetType eAssetType)
        {
            if (this.m_resourceManager == null)
            {
                AssetLogger.Error("null == m_resourceManager");
                return null;
            }
            IAssetRequest result = null;
            try
            {
                result = this.m_resourceManager.CreateAssetRequest(strCompleteUrl, callBackFun, assetPRIType, eAssetType);
            }
            catch (Exception ex)
            {
                AssetLogger.Fatal(ex.ToString());
            }
            return result;
        }
        public IAssetRequest LoadTexture(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            if (this.m_resourceManager == null)
            {
                AssetLogger.Error("null == m_resourceManager");
                return null;
            }
            IAssetRequest result = null;
            try
            {
                result = this.m_resourceManager.LoadTexture(relativeUrl, callBackFun, assetPRIType);
            }
            catch (Exception ex)
            {
                AssetLogger.Fatal(ex.ToString());
            }
            return result;
        }
        public IAssetRequest LoadAtlas(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (null == this.m_resourceManager)
            {
                AssetLogger.Error("null == m_resourceManager");
                result = null;
            }
            else
            {
                IAssetRequest assetRequest = null;
                try
                {
                    assetRequest = this.m_resourceManager.LoadAtlas(relativeUrl, callBackFun, assetPRIType);
                }
                catch (Exception ex)
                {
                    AssetLogger.Fatal(ex.ToString());
                }
                result = assetRequest;
            }
            return result;
        }
        public IAssetRequest LoadAudio(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            if (this.m_resourceManager == null)
            {
                AssetLogger.Error("null == m_resourceManager");
                return null;
            }
            IAssetRequest result = null;
            try
            {
                result = this.m_resourceManager.LoadAudio(relativeUrl, callBackFun, assetPRIType);
            }
            catch (Exception ex)
            {
                AssetLogger.Fatal(ex.ToString());
            }
            return result;
        }
        public IAssetRequest LoadUI(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            if (this.m_resourceManager == null)
            {
                AssetLogger.Error("null == m_resourceManager");
                return null;
            }
            IAssetRequest result = null;
            try
            {
                result = this.m_resourceManager.LoadUI(relativeUrl, callBackFun, assetPRIType);
            }
            catch (Exception ex)
            {
                AssetLogger.Fatal(ex.ToString());
            }
            return result;
        }
        public IAssetRequest LoadEffect(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            if (this.m_resourceManager == null)
            {
                AssetLogger.Error("null == m_resourceManager");
                return null;
            }
            IAssetRequest result = null;
            try
            {
                result = this.m_resourceManager.LoadEffect(relativeUrl, callBackFun, assetPRIType);
            }
            catch (Exception ex)
            {
                AssetLogger.Fatal(ex.ToString());
            }
            return result;
        }
        public IAssetRequest LoadModel(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (null == this.m_resourceManager)
            {
                AssetLogger.Error("null == m_resourceManager");
                result = null;
            }
            else
            {
                IAssetRequest assetRequest = null;
                try
                {
                    assetRequest = this.m_resourceManager.LoadModel(relativeUrl, callBackFun, assetPRIType);
                }
                catch (Exception ex)
                {
                    AssetLogger.Fatal(ex.ToString());
                }
                result = assetRequest;
            }
            return result;
        }
        public IAssetRequest LoadScene(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (null == this.m_resourceManager)
            {
                AssetLogger.Error("null == m_resourceManager");
                result = null;
            }
            else
            {
                IAssetRequest assetRequest = null;
                try
                {
                    assetRequest = this.m_resourceManager.LoadScene(relativeUrl, callBackFun, assetPRIType);
                }
                catch (Exception ex)
                {
                    AssetLogger.Fatal(ex.ToString());
                }
                result = assetRequest;
            }
            return result;
        }
        public void SetAllLoadFinishedEventHandler(Action<bool> eventHandler)
        {
            if (this.m_resourceManager == null)
            {
                return;
            }
            this.m_resourceManager.SetAllLoadFinishedEventHandler(eventHandler);
        }
        public void SetAllUnLoadFinishedEventHandler(Action<bool> eventHandler)
        {
            if (null != this.m_resourceManager)
            {
                this.m_resourceManager.SetAllUnLoadFinishedEventHandler(eventHandler);
            }
        }
        public void Update()
        {
            try
            {
                if (this.m_resourceManager != null)
                {
                    this.m_resourceManager.OnUpdate();
                }
            }
            catch (Exception ex)
            {
                AssetLogger.Fatal(ex.ToString());
            }
        }
        public static string GetFullPath(string strRelativePath)
        {
            return ResourceManager.GetFullPath(strRelativePath, true);
        }
        /// <summary>
        /// 取得游戏资源全目录
        /// </summary>
        /// <param name="strRelativePath">相对路径</param>
        /// <param name="bIsWWW">是否用www来加载</param>
        /// <returns>dataPath+小写格式的相对地址</returns>
        public static string GetFullPath(string strRelativePath, bool bIsWWW)
        {
            string result = LocalResourceManager.FormatPath(strRelativePath);//小写格式的相对地址
            RuntimePlatform platform = Application.platform;
            switch (platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    if (bIsWWW)
                    {
                        result = string.Format("file:///{0}/bin/{1}", Application.dataPath, strRelativePath);
                    }
                    else
                    {
                        result = string.Format("{0}/bin/{1}", Application.dataPath, strRelativePath);
                    }
                    break;
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXWebPlayer:
                case RuntimePlatform.OSXDashboardPlayer:
                case (RuntimePlatform)6:
                    break;
                case RuntimePlatform.WindowsPlayer:
                    if (bIsWWW)
                    {
                        result = string.Format("file:///{0}/../{1}", Application.dataPath, strRelativePath);
                    }
                    else
                    {
                        result = string.Format("{0}/../{1}", Application.dataPath, strRelativePath);
                    }
                    break;
                case RuntimePlatform.WindowsWebPlayer:
                    result = string.Format("{0}/../{1}", Application.dataPath, strRelativePath);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    if (bIsWWW)
                    {
                        result = string.Format("file://{0}/{1}", PluginTool.Singleton.GetResPath(), strRelativePath);
                    }
                    else
                    {
                        result = string.Format("{0}/{1}", PluginTool.Singleton.GetResPath(), strRelativePath);
                    }
                    break;
                default:
                    if (platform == RuntimePlatform.Android)
                    {
                        if (bIsWWW)
                        {
                            result = string.Format("file://{0}/{1}", PluginTool.Singleton.GetResPath(), strRelativePath);
                        }
                        else
                        {
                            result = string.Format("{0}/{1}", PluginTool.Singleton.GetResPath(), strRelativePath);
                        }
                    }
                    break;
            }
            return result;
        }
        public static string GetTextWithoutBOM(byte[] bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }
            MemoryStream memoryStream = new MemoryStream(bytes);
            StreamReader streamReader = new StreamReader(memoryStream, true);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            memoryStream.Close();
            return result;
        }
    }
}

