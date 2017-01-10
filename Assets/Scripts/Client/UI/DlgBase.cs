using UnityEngine;
using Object = UnityEngine.Object;
using System;
using System.Collections;
using Utility;
using UnityAssetEx.Export;
using UnityAssetEx.Local;
using Utility.Export;
using System.Threading;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgBase
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：UI基础管理器
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public abstract class DlgBase<TDlgClass,TUIBehaviour> : IXUIDlg where TDlgClass : IXUIDlg ,new () where TUIBehaviour : DlgBehaviourBase
    {
        private TUIBehaviour m_uiBehaviour = default(TUIBehaviour);//界面组件behavior
        private static TDlgClass s_instance = default(TDlgClass);//界面管理类
        private static object s_objLock = new object();
        private bool m_bVisible = false;
        private bool m_bLoaded = false;
        private bool m_bLimitClick = true;
        private float m_fDepthZ = 0f;
        private bool m_bNeedToRefresh = false;
        private IAssetRequest m_assetRequest = null;
        private float m_fCloseTime = 0f;
        protected bool m_bInChangeState = false;
        private IXLog m_log = XLog.GetLog(typeof(DlgBase<TDlgClass, TUIBehaviour>));
        /// <summary>
        /// 多线程初始化界面，uimanager.add（这个界面）
        /// </summary>
        public static TDlgClass singleton 
        {
            get
            {
                if (null == DlgBase<TDlgClass, TUIBehaviour>.s_instance)
                {
                    object obj;
                    Monitor.Enter(obj = DlgBase<TDlgClass, TUIBehaviour>.s_objLock);
                    try
                    {
                        if (null == DlgBase<TDlgClass, TUIBehaviour>.s_instance)
                        {
                            DlgBase<TDlgClass, TUIBehaviour>.s_instance = ((default(TDlgClass) == null) ? Activator.CreateInstance<TDlgClass>() : default(TDlgClass));
                            UIManager.singleton.AddDlg(DlgBase<TDlgClass, TUIBehaviour>.s_instance);
                            DlgBase<TDlgClass, TUIBehaviour>.s_instance.SetVisible(false);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(obj);
                    }
                }
                return DlgBase<TDlgClass, TUIBehaviour>.s_instance;
            }
        }
        public IXUIBehaviour uiBehaviourInterface
        {
            get
            {
                return this.m_uiBehaviour;
            }
        }
        public TUIBehaviour uiBehaviour
        {
            get
            {
                return this.m_uiBehaviour;
            }
        }
        public bool Prepared
        {
            get
            {
                return null != this.m_uiBehaviour;
            }
        }
        #region 子类实现
        public virtual string fileName
        {
            get
            {
                return "";
            }
        }
        public virtual int layer
        {
            get
            {
                return 2;
            }
        }
        public virtual uint Type
        {
            get
            {
                return 128u;
            }
        }
        public virtual EnumDlgCamera ShowType
        {
            get
            {
                return EnumDlgCamera.Normal;
            }
        }
        public virtual bool IsBundle
        {
            get
            {
                return true;
            }
        }
        public virtual bool IsPersist
        {
            get
            {
                return false;
            }
        }
        public virtual bool IsLimitClick
        {
            get
            {
                return this.m_bLimitClick;
            }
            set
            {
                this.m_bLimitClick = value;
            }
        }
        public virtual bool NotSupportRoll
        {
            get
            {
                return false;
            }
        }
        #endregion
        public void _Init(IXUIBehaviour iXUIPanel)
        {
            this.m_uiBehaviour = (iXUIPanel as TUIBehaviour);
            this.InnerInit();
        }
        public void _Update()
        {
            try
            {
                if (this.Prepared)
                {
                    this.m_uiBehaviour._Update();
                }
                if (this.Prepared && this.IsVisible())
                {
                    this.Update();
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        public void _FixedUpdate()
        {
            try
            {
                if (!this.IsPersist && !this.IsVisible() && this.Prepared)
                {
                    if (Time.time - this.m_fCloseTime > 10f)
                    {
                        this.UnLoad(false);
                    }
                }
                this.FixedUpdate();
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        /// <summary>
        /// 设置ui的z轴深度
        /// </summary>
        /// <param name="nDepthZ">深度</param>
        public void SetDepthZ(int nDepthZ)
        {
            this.m_fDepthZ = (float)(nDepthZ * 10);
            this.RefreshDepth();
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        /// <returns></returns>
        public bool IsVisible()
        {
            return this.m_bVisible;
        }
        /// <summary>
        /// 加载UI资源
        /// </summary>
        public void Load()
        {
            try
            {
                if (!this.m_bLoaded)
                {
                    this.m_bLoaded = true;
                    string text = string.Format("ui/{0}/{0}", this.fileName);
                    if (this.IsBundle)
                    {
                        this.m_assetRequest = ResourceManager.singleton.LoadUI(text, new AssetRequestFinishedEventHandler(this.OnLoadUIFinishedEventHandler), AssetPRI.DownloadPRI_Plain);
                    }
                    else
                    {
                        GameObject gameObject = Resources.Load(text) as GameObject;
                        GameObject gameObject2;
                        if (null != gameObject)
                        {
                            gameObject2 = (Object.Instantiate(gameObject) as GameObject);
                        }
                        else
                        {
                            gameObject2 = new GameObject(this.fileName);
                            this.m_log.Fatal(string.Format("null == assetResource: {0}", this.fileName));
                        }
                        if (null != gameObject2)
                        {
                            gameObject2.name = this.fileName;
                            gameObject2.transform.parent = UIManager.singleton.UIRoot;
                            gameObject2.transform.localPosition = new Vector3(-0.5f, 0.5f, 0f);
                            gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
                            TUIBehaviour tUIBehaviour = gameObject2.AddComponent<TUIBehaviour>();
                            tUIBehaviour.ParentDlg = this;
                            this._Init(tUIBehaviour);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        /// <summary>
        /// 卸载Ui，释放UI界面
        /// </summary>
        /// <returns></returns>
        public bool UnLoad(bool bQuickly)
        {
            bool result;
            try
            {
                if (this.Prepared && !this.IsVisible())
                {
                    try
                    {
                        this.OnUnLoad();
                    }
                    catch (Exception ex)
                    {
                        this.m_log.Fatal(ex.ToString());
                    }
                    TUIBehaviour uiBehaviour = this.uiBehaviour;
                    UnityEngine.Object.Destroy(uiBehaviour.gameObject);//destoryUI界面
                    this.m_uiBehaviour = default(TUIBehaviour);
                    this.m_bLoaded = false;
                    if (null != this.m_assetRequest)
                    {
                        this.m_assetRequest.RemoveQuickly = bQuickly;
                        this.m_assetRequest.Dispose();//释放UI资源
                        this.m_assetRequest = null;
                    }
                    result = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
            result = false;
            return result;
        }
        public bool UnLoad()
        {
            return this.UnLoad(true);
        }
        public void OnFinishChangeState()
        {
            if (this.m_bInChangeState)
            {
                this.m_bInChangeState = false;
                this.RefreshDepth();
            }
        }
        /// <summary>
        /// 刷新UI
        /// </summary>
        public void Refresh()
        {
            try
            {
                if (this.Prepared)
                {
                    this.OnRefresh();
                }
                else
                {
                    this.m_bNeedToRefresh = true;
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        /// <summary>
        /// 初始化UI的tip，深度，注册事件，然后设置可见
        /// </summary>
        private void InnerInit()
        {
            try
            {
                this.m_uiBehaviour.Init();
                this.SetDepthZ(this.layer * 10);
                this.Init();
                this.RegisterEvent();
                TUIBehaviour uiBehaviour = this.uiBehaviour;
                uiBehaviour.SetVisible(this.m_bVisible);
                if (this.m_bVisible)
                {
                    
                    this.OnShow();
                    if (this.m_bNeedToRefresh)
                    {
                        this.m_bNeedToRefresh = false;
                        this.OnRefresh();
                    }
                    UIManager.singleton.Compositor(this);
                    UIManager.singleton.OnDlgShow(this);
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        /// <summary>
        /// 刷新ui的z轴深度
        /// </summary>
        private void RefreshDepth()
        {
            if (this.Prepared)
            {
                TUIBehaviour uiBehaviour = this.uiBehaviour;
                Vector3 localPosition = uiBehaviour.transform.localPosition;
                localPosition.z = this.m_fDepthZ;
                if (this.m_bInChangeState)
                {
                    localPosition.z = this.m_fDepthZ + 10000f;
                }
                uiBehaviour = this.uiBehaviour;
                uiBehaviour.transform.localPosition = localPosition;
            }
        }
       /// <summary>
       /// 设置层级
       /// </summary>
       /// <param name="layer"></param>
        private void SetLayer(EnumDlgCamera layer)
        {
            if (this.Prepared)
            {
                bool result;
                if (layer == EnumDlgCamera.Normal)
                {
                    TUIBehaviour uiBehaviour = this.uiBehaviour;
                    result = (uiBehaviour.CachedGameObject.layer == Singleton<LayerManager>.singleton.NormalLayer);
                }
                else
                {
                    result = true;
                }
                if (!result)
                {
                    TUIBehaviour uiBehaviour = this.uiBehaviour;
                    UnityTools.SetLayerRecursively(uiBehaviour.gameObject, Singleton<LayerManager>.singleton.NormalLayer);
                }
                else
                {
                    if (layer > EnumDlgCamera.Normal)
                    {
                        TUIBehaviour uiBehaviour = this.uiBehaviour;
                        if (uiBehaviour.CachedGameObject.layer != Singleton<LayerManager>.singleton.TopLayer)
                        {
                            uiBehaviour = this.uiBehaviour;
                            UnityTools.SetLayerRecursively(uiBehaviour.gameObject, Singleton<LayerManager>.singleton.TopLayer);
                        }
                        if (layer == EnumDlgCamera.Top_Blur)
                        {
                            //CameraManager.Instance.SetScreenBlurStatus(true);
                        }
                    }
                }
                if (this.NotSupportRoll)
                {
                    //CameraManager.Instance.SetUIRollSupport(true);
                }
            }
        }
        private void OnLoadUIFinishedEventHandler(IAssetRequest assetRequest)
        {
            try
            {
                IAssetResource assetResource = assetRequest.AssetResource;
                GameObject gameObject;
                if (assetResource != null && null != assetResource.MainAsset)
                {
                    
                    UnityEngine.Object mainAsset = assetResource.MainAsset;
                   
                    gameObject = (UnityEngine.Object.Instantiate(mainAsset) as GameObject);
                }
                else
                {
                    gameObject = new GameObject(this.fileName + "_NotFound");
                    this.m_log.Fatal(string.Format("null == assetResource: {0}", this.fileName));
                }
                if (null != gameObject)
                {
                    gameObject.name = this.fileName;
                    gameObject.transform.parent = UIManager.singleton.UIRoot;
                    gameObject.transform.localPosition = new Vector3(-0.5f, 0.5f, 0f);
                    gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    TUIBehaviour tUIBehaviour = gameObject.AddComponent<TUIBehaviour>();//添加脚本
                    tUIBehaviour.ParentDlg = this;
                    this._Init(tUIBehaviour);
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }

        #region 子类实现
        public virtual void SetVisible(bool bIsVisible)
        {
            try
            {
                if (this.Prepared)
                {
                    TUIBehaviour uiBehaviour = this.uiBehaviour;
                    uiBehaviour.SetVisible(bIsVisible);//设置界面可见
                }
                bool bVisible = this.m_bVisible;
                this.m_bVisible = bIsVisible;
                if (this.m_bVisible)
                {
                    if (!this.m_bLoaded)
                    {
                        this.Load();
                    }
                    else
                    {
                        if (this.Prepared)
                        {
                            if (bVisible != this.m_bVisible)
                            {
                                this.SetLayer(this.ShowType);
                            }
                            this.OnShow();
                            UIManager.singleton.Compositor(this);
                            UIManager.singleton.OnDlgShow(this);
                        }
                    }      
                }
                else 
                {
                    this.m_fCloseTime = Time.time;
                    if (this.Prepared && bVisible != this.m_bVisible)
                    {
                        if (this.NotSupportRoll)
                        {
                            //CameraManager.Instance.SetUIRollSupport(false);
                        }
                        if (this.ShowType == EnumDlgCamera.Top_Blur)
                        {
                            //CameraManager.Instance.SetScreenBlurStatus(false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }

        public virtual void Init()
        {
 
        }
        public virtual void RegisterEvent()
        {
        }
        public virtual void Update()
        {
 
        }
        public virtual void FixedUpdate()
        {
        }
        public virtual void Reset()
        {
        }
        protected virtual void OnUnLoad()
        {
        }
        protected virtual void OnShow()
        {
        }
        /// <summary>
        /// 刷新UI界面，由子类实现
        /// </summary>
        protected virtual void OnRefresh()
        {
        }
        #endregion
    }
}