using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Client.Common;
using LitJson;
using UnityAssetEx.Export;
using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalResourceManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：WWW资源加载管理器
//----------------------------------------------------------------*/
#endregion 
namespace UnityAssetEx.Local
{
    public class LocalResourceManager : MonoBehaviour,IResourceManager
    {
        #region 私有变量
        private Queue<LocalAssetResource> assetResourceQueue = new Queue<LocalAssetResource>();
        /// <summary>
        /// 存储需要加载的本地资源LocalAssetResource。key=>资源名，value=>LocalAssetResource
        /// </summary>
        private Dictionary<string, LocalAssetResource> m_dicAssetResoure = new Dictionary<string, LocalAssetResource>();
        private Dictionary<string, LocalAssetCollectDepResource> m_dicAssetCollectDepResource = new Dictionary<string, LocalAssetCollectDepResource>();
        private LinkedList<LocalAssetResource> m_linkedListAssetResourceInLoading = new LinkedList<LocalAssetResource>();
        private LinkedList<LocalAssetResource> m_linkedListNeedToLoad = new LinkedList<LocalAssetResource>();
        private static LocalAssetRequest m_assetRequestError = new LocalAssetRequest(null, null);
        private CollectDepResourceDataMap m_DicUIResourceData;
        private CollectDepResourceDataMap m_DicEffectReourceData;
        private CollectDepResourceDataMap m_DicAtlasResourceData;
        private CollectDepResourceDataMap m_DicModelResourceData;
        private Action<bool> m_allLoadFinishedEventHandler;
        private Action<bool> m_allUnLoadFinishedEventHandler;
        private bool m_bLoadError = false;
        private float disposeTime = Time.time;
        private float unloadUseTime;//b
        private int m_total;//A
        private int m_current;//a
        #endregion
        #region 单例
        private static LocalResourceManager instance = null;
        public static LocalResourceManager GetInstance()
        {
            if (LocalResourceManager.instance == null)
            {
                GameObject gameObject = new GameObject("WWWResourceManager");
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                LocalResourceManager.instance = gameObject.AddComponent<LocalResourceManager>();
            }
            return LocalResourceManager.instance;
        }
        #endregion
        #region 构造函数
        private LocalResourceManager()
        {
 
        }
        #endregion
        #region 公有属性
        /// <summary>
        /// 加载进度
        /// </summary>
        public float ProgressValue
        {
            get 
            {
                if (this.m_total > 0)
                {
                    return (float)this.m_current / (float)this.m_total;
                }
                return 0f;
            }
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 初始化资源配置信息管理器，默认为UI和Effect特效资源
        /// </summary>
        public void Init(string strBaseResDir, string strBaseResWWWDir)
        {
            Resources.UnloadUnusedAssets();
            string path1 = ResourceManager.GetFullPath("data/ui.config", false);
            if (!this.InitUIResourceData(path1))
            {
                AssetLogger.Error(string.Format("false == InitUI(strUIConfig):{0}", path1));
            }
            /*string path2 = ResourceManager.GetFullPath("data/effect.config", false);
            if (!this.InitEffectResourceData(path2))
            {
                AssetLogger.Error(string.Format("false == InitEffect(strEffectConfig):{0}", path2));
            }*/
            string path3 = ResourceManager.GetFullPath("data/atlas.config", false);
            if (!this.InitAtlasResourceData(path3))
            {
                AssetLogger.Error(string.Format("false == InitAtlas(strAtlasConfig):{0}", path3));
            }
            /*
            string path4 = ResourceManager.GetFullPath("data/model.config", false);
            if (!this.InitModelResourceData(path4))
            {
                AssetLogger.Error(string.Format("false == InitModel(strModelConfig):{0}", path4));
            }
             * */
        }
        public void Clear()
        {
            while (this.assetResourceQueue.Count > 0)
            {
                LocalAssetResource resource = this.assetResourceQueue.Dequeue();
                resource.Dispose();
            }
            this.assetResourceQueue.Clear();
            foreach (LocalAssetResource current in this.m_dicAssetResoure.Values)
            {
                current.Dispose();
            }
            this.m_dicAssetResoure.Clear();
            this.m_dicAssetCollectDepResource.Clear();
        }
        /// <summary>
        /// 格式化路径，变成小写
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPath(string path)
        {
            string text = path.Replace("\\", "/");
            return text.ToLower();
        }
        /// <summary>
        /// 将路径转化成文件名不带后缀,将/转成_
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ChangePathToFilenameWithoutExtension(string path)
        {
            path = LocalResourceManager.FormatPath(path);
            string text = path.Replace('/', '_');
            text = text.Replace(" ", "");
            return Path.GetFileNameWithoutExtension(text);
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 初始化UI资源配置信息，从Json读取数据转成CollectDepResourceDataMap类型实例，并初始化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool InitUIResourceData(string path)
        {
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                JsonReader reader = new JsonReader(streamReader);
                this.m_DicUIResourceData = JsonMapper.ToObject<CollectDepResourceDataMap>(reader);
            }
            if (this.m_DicUIResourceData == null)
            {
                this.m_DicUIResourceData = new CollectDepResourceDataMap();
                return false;
            }
            CollectDepResourceDataMap.AddResourceDatas(this.m_DicUIResourceData.mDicResourceData);//初始化<string,ResoureData>ALL（包括独立和带引用）
            this.m_DicUIResourceData.mDicResourceData.Clear();//清除从配置信息加载的资源集合
            return true;
        }
        /// <summary>
        /// 初始化特效资源配置信息，从Json读取数据转成CollectDepResourceDataMap类型实例，并初始化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool InitEffectResourceData(string path)
        {
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                JsonReader read = new JsonReader(reader);
                this.m_DicEffectReourceData = JsonMapper.ToObject<CollectDepResourceDataMap>(read);
            }
            if (this.m_DicEffectReourceData == null)
            {
                this.m_DicEffectReourceData = new CollectDepResourceDataMap();
                return false;
            }
            CollectDepResourceDataMap.AddResourceDatas(this.m_DicEffectReourceData.mDicResourceData);
            this.m_DicEffectReourceData.mDicResourceData.Clear();
            return true;
        }
        /// <summary>
        /// 初始化图集资源，从Json读取数据转成CollectDepResourceDataMap类型实例，并初始化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool InitAtlasResourceData(string path)
        {
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                JsonReader reader = new JsonReader(streamReader);
                this.m_DicAtlasResourceData = JsonMapper.ToObject<CollectDepResourceDataMap>(reader);
            }
            if (this.m_DicAtlasResourceData == null)
            {
                this.m_DicAtlasResourceData = new CollectDepResourceDataMap();
                return false;
            }
            CollectDepResourceDataMap.AddResourceDatas(this.m_DicAtlasResourceData.mDicResourceData);
            this.m_DicAtlasResourceData.mDicResourceData.Clear();
            return true;
        }
        /// <summary>
        /// 初始化模型资源，从Json读取数据转成CollectDepResourceDataMap类型实例，并初始化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool InitModelResourceData(string path)
        {
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                JsonReader reader = new JsonReader(streamReader);
                this.m_DicModelResourceData = JsonMapper.ToObject<CollectDepResourceDataMap>(reader);
            }
            if (this.m_DicModelResourceData == null)
            {
                AssetLogger.Error("null == m_dicModelResourceData");
                this.m_DicModelResourceData = new CollectDepResourceDataMap();
                return false;
            }
            CollectDepResourceDataMap.AddResourceDatas(this.m_DicModelResourceData.mDicResourceData);
            this.m_DicModelResourceData.mDicResourceData.Clear();
            return true;
        }
        #endregion
        public IAssetRequest CreateAssetRequest(ResourceData resourceData, List<ResourceData> depResourceList, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI)
        {
            if (resourceData == null)
            {
                return null;
            }
            AssetLogger.Debug("CreateAssetRequest:"+resourceData.mResourceName);
            if (string.IsNullOrEmpty(resourceData.mPath))
            {
                return null;
            }
            LocalAssetCollectDepResource depResource = null;
            if (this.m_dicAssetCollectDepResource.TryGetValue(resourceData.mPath, out depResource))
            {
                return new LocalAssetRequest(depResource, callback);
            }
            depResource = LocalAssetCollectDepResource.Create();
            depResource.URL = resourceData.mPath;
            this.m_dicAssetCollectDepResource.Add(depResource.URL, depResource);
            if (depResourceList != null)
            {
                depResource.SetDepSize(depResourceList.Count);//初始化depAssetResource[]，引用资源的数组
                for (int i = 0; i < depResourceList.Count; i++)
                {
                    ResourceData resourceData2 = depResourceList[i];
                    Debug.Log(resourceData2.mResourceName);
                    LocalAssetResource assetResource = this.CreateAssetResource(resourceData2, new AssetLoadFinishedEventHandler(depResource.AssetComplete), assetPRI);
                    depResource.AddDep(assetResource, i);//赋值depAssetResource[]
                    Debug.Log(assetResource == null);
                }
            }
            LocalAssetResource asset = this.CreateAssetResource(resourceData, new AssetLoadFinishedEventHandler(depResource.AssetComplete), assetPRI);
            depResource.SetAsset(asset);
            depResource.EndCreate();
            return new LocalAssetRequest(depResource,callback);
        }
        /// <summary>
        /// 加载UI资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <param name="assetPRI"></param>
        /// <returns></returns>
        public IAssetRequest LoadUI(string path, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI)
        {
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                return null;
            }
            string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);//路径转成文件名不带后缀
            ResourceData resourceData = null;
            List<ResourceData> dependes = null;
            CollectDepResourceData collectDepResourceData = this.m_DicUIResourceData.GetCollectDepResourceData(text, out resourceData, out dependes);
            if (collectDepResourceData != null)
            {
                return this.CreateAssetRequest(resourceData, dependes, callback, assetPRI);//创建请求资源实例
            }
            AssetLogger.Error(string.Format("null == collectDepResourceData:{0}", text));
            return null;
        }
        public IAssetRequest LoadAtlas(string path, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                result = null;
            }
            else
            {
                string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);
                ResourceData resourceData = null;
                List<ResourceData> dependes = null;
                CollectDepResourceData collectDepResourceData = this.m_DicAtlasResourceData.GetCollectDepResourceData(text, out resourceData, out dependes);
                if (null != collectDepResourceData)
                {
                    result = this.CreateAssetRequest(resourceData, dependes, callBackFun, assetPRIType);
                }
                else
                {
                    AssetLogger.Error(string.Format("null == collectDepResourceData:{0}", text));
                    result = null;
                }
            }
            return result;
        }
        public IAssetRequest LoadEffect(string path, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI)
        {
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                return null;
            }
            string name = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);
            ResourceData resourceData = null;
            List<ResourceData> list = null;
            CollectDepResourceData collectDepResourceData = this.m_DicEffectReourceData.GetCollectDepResourceData(name, out resourceData, out list);
            if (collectDepResourceData != null)
            {
                return this.CreateAssetRequest(resourceData, list, callback, assetPRI);
            }
            AssetLogger.Error(string.Format("null == collectDepResourceData:{0}", path));
            return null;
        }
        public IAssetRequest LoadTexture(string path, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI)
        {
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                return null;
            }
            string arg = Path.GetDirectoryName(path).ToLower();//texture/hexagon
            string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);//texture_hexagon_select
            path = string.Format("data/{0}/{1}.ab", arg, text);
            ResourceData resourceData = CollectDepResourceDataMap.RefResource(text, path, 0, EnumAssetType.eAssetType_AssetBundleTexture);
            return this.CreateAssetRequest(resourceData, null, callback, assetPRI);
        }
        /// <summary>
        /// 加载模型资源
        /// </summary>
        /// <param name="path">Data/Model/HeroModel/{0}</param>
        /// <param name="callBackFun"></param>
        /// <param name="assetPRIType"></param>
        /// <returns></returns>
        public IAssetRequest LoadModel(string path, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                result = null;
            }
            else
            {
                string name = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);
                ResourceData resourceData = null;
                List<ResourceData> dependes = null;
                CollectDepResourceData collectDepResourceData = this.m_DicModelResourceData.GetCollectDepResourceData(name, out resourceData, out dependes);
                if (null != collectDepResourceData)
                {
                    result = this.CreateAssetRequest(resourceData, dependes, callBackFun, assetPRIType);
                }
                else
                {
                    AssetLogger.Error(string.Format("null == collectDepResourceData:{0}", path));
                    result = null;
                }
            }
            return result;
        }
        public IAssetRequest LoadAudio(string path, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI)
        {
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                return null;
            }
            string arg = Path.GetDirectoryName(path).ToLower();//audio/bgm/ui/login
            string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);//audio_bgm_ui_login
            path = string.Format("data/{0}/{1}.ab", arg, text);
            ResourceData resourceData = CollectDepResourceDataMap.RefResource(text, path, 0, EnumAssetType.eAssetType_AssetBundleAudio);
            return this.CreateAssetRequest(resourceData, null, callback, assetPRI);
        }
        public IAssetRequest LoadShader(string path, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                result = null;
            }
            else
            {
                string arg = Path.GetDirectoryName(path).ToLower();
                string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);
                path = string.Format("data/{0}/{1}.ab", arg, text);
                ResourceData resourceData = CollectDepResourceDataMap.RefResource(text, path, 0, EnumAssetType.eAssetType_AssetBundleShader);
                result = this.CreateAssetRequest(resourceData, null, callBackFun, assetPRIType);
            }
            return result;
        }
        public IAssetRequest LoadScene(string path, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
        {
            IAssetRequest result;
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                result = null;
            }
            else
            {
                string directoryName = Path.GetDirectoryName(path);
                string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);
                path = string.Format("data/{0}/{1}.unity3d", directoryName, text);
                ResourceData resourceData = CollectDepResourceDataMap.RefResource(text, path, 0, EnumAssetType.eAssetType_Scene);
                result = this.CreateAssetRequest(resourceData, null, callBackFun, assetPRIType);
            }
            return result;
        }
        public IAssetRequest CreateAssetRequest(string path, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI)
        {
            return this.CreateAssetRequest(path, callback, assetPRI, EnumAssetType.eAssetType_AssetBundlePrefab);
        }
        public IAssetRequest CreateAssetRequest(string path, AssetRequestFinishedEventHandler callback, AssetPRI assetPRI, EnumAssetType assetType)
        {
            if (string.IsNullOrEmpty(path))
            {
                AssetLogger.Error("string.IsNullOrEmpty(path) == true");
                return null;
            }
            string arg = Path.GetDirectoryName(path).ToLower();
            string text = LocalResourceManager.ChangePathToFilenameWithoutExtension(path);
            path = string.Format("data/{0}/{1}.ab", arg, text);
            ResourceData resourceData = CollectDepResourceDataMap.RefResource(text, path, 0, assetType);
            return this.CreateAssetRequest(resourceData, null, callback, assetPRI);
        }             
        /// <summary>
        /// 创建需要加载的LocalAssetResource，如果存在就获取，添加到下载链表中
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="handler"></param>
        /// <param name="assetPRI"></param>
        /// <returns></returns>
        public LocalAssetResource CreateAssetResource(ResourceData resource, AssetLoadFinishedEventHandler handler, AssetPRI assetPRI)
        {
            if (resource == null)
            {
                return null;
            }
            string mPath = resource.mPath;
            string key = Path.GetFileNameWithoutExtension(mPath).ToLower();//获取文件名，不包括扩展名
            LocalAssetResource assetResource = new LocalAssetResource(resource);
            if (!this.m_dicAssetResoure.ContainsKey(key))//如果不包含这个资源，就创建资源然后添加到资源集合中
            {
                assetResource = new LocalAssetResource(resource);
                assetResource.PRIType = assetPRI;
                this.m_dicAssetResoure.Add(key, assetResource);
                if (this.m_allLoadFinishedEventHandler != null)
                {
                    this.m_total += assetResource.Size;
                }
                Debug.Log(assetResource.URL + "添加到下载链表！");
                this.m_linkedListNeedToLoad.AddLast(assetResource);//把需要加载的资源添加到链表的最后
            }
            else 
            {
                assetResource = this.m_dicAssetResoure[key];
                Debug.Log("之前已经加载过了:"+assetResource.URL);
            }
            if (handler != null && !assetResource.Canceled)//设置加载完成之后的委托
            {
                assetResource.SetLoadFinishedHandler((AssetLoadFinishedEventHandler)Delegate.Combine(assetResource.GetLoadFinishedHandler(), handler));
            }
            return assetResource;
        }
        /// <summary>
        /// 设置所有资源加载完成的委托
        /// </summary>
        /// <param name="handler"></param>
        public void SetAllLoadFinishedEventHandler(Action<bool> handler)
        {
            this.m_allLoadFinishedEventHandler = (Action<bool>)Delegate.Combine(this.m_allLoadFinishedEventHandler, handler);
            this.m_bLoadError = false;
        }
        public void SetAllUnLoadFinishedEventHandler(Action<bool> eventHandler)
        {
            this.m_allUnLoadFinishedEventHandler = (Action<bool>)Delegate.Combine(this.m_allUnLoadFinishedEventHandler, eventHandler);
        }
        public void OnUpdate()
        {
            if (!this.UnLoadNotUsedResource() && this.m_allUnLoadFinishedEventHandler != null)
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
                Action<bool> allUnLoadFinishedEventHandler = this.m_allUnLoadFinishedEventHandler;
                this.m_allUnLoadFinishedEventHandler = null;
                allUnLoadFinishedEventHandler(true);
            }
            LinkedListNode<LocalAssetResource> next;
            for (LinkedListNode<LocalAssetResource> linkedListNode = this.m_linkedListAssetResourceInLoading.First; linkedListNode != null; linkedListNode = next)
            {
                next = linkedListNode.Next;
                LocalAssetResource value = linkedListNode.Value;
                if (value.IsDone)//已经加载完成
                {
                    Debug.Log(value.URL + "已经下载过了");
                    if (!string.IsNullOrEmpty(value.Error))//如果加载没错的话
                    {
                        this.m_bLoadError = true;
                        AssetLogger.Error(value.Error);
                    }
                    if (this.m_allLoadFinishedEventHandler != null)
                    {
                        this.m_current += value.Size;
                    }
                    this.m_linkedListAssetResourceInLoading.Remove(linkedListNode);
                    value.OnLoaded();
                }
                else //还没有加载
                {
                    if (value.Canceled)//如果已经取消就从链表中移除
                    {
                        Debug.Log("取消下载:" + value.URL);
                        this.m_linkedListAssetResourceInLoading.Remove(linkedListNode);
                    }
                    else 
                    {
                        if (!value.Started)
                        {
                            Debug.Log(value.URL+"开始下载");
                            value.BeginDownload();
                        }
                    }
                }
            }
            //如果平台是手机的话，就处理1个，不是的话处理5个
            int dealCount = Application.isMobilePlatform ? 1 : 10;
            if (this.m_linkedListNeedToLoad.Count > 0 && this.m_linkedListAssetResourceInLoading.Count < dealCount)//保持一次处理5个asset
            {
                Debug.Log(this.m_linkedListNeedToLoad.Count);
                LocalAssetResource value2 = this.m_linkedListNeedToLoad.First.Value;
                this.m_linkedListNeedToLoad.RemoveFirst();
                Debug.Log("AddLast:"+value2.URL);
                this.m_linkedListAssetResourceInLoading.AddLast(value2);
            }
            //如果没有加载任务了，就表示完成执行完成回调函数
            if (this.m_linkedListNeedToLoad.Count == 0 && this.m_linkedListAssetResourceInLoading.Count == 0 && this.m_allLoadFinishedEventHandler != null)
            {
                Action<bool> allLoadFinishedEventHandler = this.m_allLoadFinishedEventHandler;
                this.m_allLoadFinishedEventHandler = null;
                try
                {
                    allLoadFinishedEventHandler(!this.m_bLoadError);
                }
                catch (Exception e)
                {
                    AssetLogger.Fatal(e.ToString());
                }
                this.m_total = 0;
                this.m_current = 0;
            }
        }
        /// <summary>
        /// 释放没用的资源
        /// </summary>
        /// <returns></returns>
        private bool UnLoadNotUsedResource()
        {
            bool result = false;
            List<string> list = new List<string>();
            foreach (var current in this.m_dicAssetResoure)
            {
                //如果资源被引用的次数为0，就加入到需要被卸载的列表内
                if (0 >= current.Value.RefCount)
                {
                    Debug.Log("UnloadADd:"+current.Key);
                    list.Add(current.Key);
                }
            }
            //遍历所有需要卸载的列表，释放资源
            for (int i = 0; i < list.Count; i++)
            {
                string key = list[i];
                LocalAssetResource assetResource = null;
                if (this.m_dicAssetResoure.TryGetValue(key, out assetResource))
                {
                    if (assetResource != null)
                    {
                        Debug.Log("dispose" + assetResource.URL);
                        assetResource.Dispose();
                        result = true;
                    }
                    this.m_dicAssetResoure.Remove(key);
                }
            }
            bool flag = false;
            List<string> list2 = new List<string>();
            foreach (var current2 in this.m_dicAssetCollectDepResource)
            {
                if (0 >= current2.Value.RefCount)
                {
                    if (!flag)
                    {
                        list2.Add(current2.Key);
                        flag = true;
                    }
                    else 
                    {
                        if (current2.Value.GetRemoveQuickly())
                        {
                            list2.Add(current2.Key);
                        }
                    }
                }
            }
            for (int j = 0; j < list2.Count; j++)
            {
                string key2 = list2[j];
                LocalAssetCollectDepResource assetCollectDepResource = null;
                if (this.m_dicAssetCollectDepResource.TryGetValue(key2, out assetCollectDepResource))
                {
                    if (assetCollectDepResource != null)
                    {
                        Debug.Log("Unload:" + assetCollectDepResource.URL);
                        assetCollectDepResource.Dispose();
                        result = true;
                    }
                    this.m_dicAssetCollectDepResource.Remove(key2);
                }
            }
            return result;
        }
    }
}
