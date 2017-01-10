using UnityEngine;
using System.Collections.Generic;
using UnityAssetEx.Export;
using Utility.Export;
using Utility;
using Client.UI.UICommon;
using Client.UI;
using System;
using GameClient;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UnityGameEntry
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：游戏驱动入口
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏驱动入口
/// </summary>
public class UnityGameEntry : MonoBehaviour
{
    #region 字段
    private static UnityGameEntry s_instance = null;
    public Transform m_objUIRoot = null;//UI的根目录，UIRoot
    public GameObject m_prefabPlayer = null;
    public float ZoomSpeed = 10f;
    public float MovingSpeed = 0.5f;
    public float RotateSpeed = 1f;
    public float distance = 5f;
    public int Iterations = 3;
    public float Spread = 0.7f;
    public Color outterColor = new Color(0.133f, 1f, 0f, 1f);
    public Camera outterLineCamera = null;
    public Shader compositeShader;//复合shader
    private Material m_CompositeMaterial = null;//复合材质
    public Shader blurShader;//模糊shader
    private Material m_blurMaterial = null;//模糊材质
    public Shader cutoffShader;
    private Material m_cutoffMaterial = null;
    private Material m_outterLineMaterial = null;
    private BlurEffect m_camBlurEffect = null;
    private IXLog m_log = XLog.GetLog<UnityGameEntry>();
    private Transform m_transBeastRoot = null;//圣兽的根节点
    private bool m_bApplicationFocus = false;
    #endregion
    #region 属性
    public static UnityGameEntry Instance
    {
        get
        {
            return UnityGameEntry.s_instance;
        }
    }
    public int MainCameraCullingMask
    {
        get;
        private set;
    }
    protected Material CompositeMaterial
    {
        get 
        {
            if (this.m_CompositeMaterial == null)
            {
                this.m_CompositeMaterial = new Material(this.compositeShader);
                this.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.CompositeMaterial;
        }
    }
    protected Material blurMaterial
    {
        get
        {
            if (this.m_blurMaterial == null)
            {
                this.m_blurMaterial = new Material(this.blurShader);
                this.m_blurMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.m_blurMaterial;
        }
    }
    protected Material cutoffMaterial
    {
        get
        {
            if (this.m_cutoffMaterial == null)
            {
                this.m_cutoffMaterial = new Material(this.cutoffShader);
                this.m_cutoffMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.m_cutoffMaterial;
        }
    }
    protected Material outterLineMat
    {
        get
        {
            if (this.m_outterLineMaterial == null)
            {
                this.m_outterLineMaterial = new Material(string.Concat(new object[]
				{
					"Shader\"Hidden/SolidBody1\"{SubShader{Pass{Color(",
					this.outterColor.r,
					",",
					this.outterColor.g,
					",",
					this.outterColor.b,
					",",
					this.outterColor.a,
					")}}}"
				}));
                this.m_outterLineMaterial.hideFlags = HideFlags.HideAndDontSave;
                this.m_outterLineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.m_outterLineMaterial;
        }
    }
    public Transform BeastRoot
    {
        get
        {
            return this.m_transBeastRoot;
        }
    }
    public bool IsApplicationFocus
    {
        get
        {
            return this.m_bApplicationFocus;
        }
    }
    #endregion
    #region 方法
    private void Awake()
    {
        this.m_log.Debug("UnityGameEntry.Awake()");
        Application.targetFrameRate = 30;
        UnityGameEntry.s_instance = this;
        if (base.transform.parent != null)
        {
            UnityEngine.Object.DontDestroyOnLoad(base.transform.parent.gameObject);//防止再加载其他场景删除这个脚本
        }
        Singleton<ClientMain>.singleton.Awake();
    }
    private void Start()
    {
        this.m_log.Debug("UnityGameEntry.Start()");
        InvokeRepeating("Tick", 1, 0.1f);
        //初始化神兽根节点
        if (base.transform.parent != null)
        {
            this.m_transBeastRoot = base.transform.parent.FindChild("Beasts");
        }
        if (null == this.m_transBeastRoot)
        {
            XLog.Log.Fatal("null == m_transHeroRoot");
        }
        ///初始化UIManager
        if (null != this.m_objUIRoot)
        {
            IXUITool uiTool = this.m_objUIRoot.GetComponent("XUITool") as IXUITool;
            UIManager.singleton.Init(uiTool, this.m_objUIRoot);
        }
        //为UICamera自动添加模糊脚本
       /* GameObject gameObject = GameObject.Find("UI Root/Camera");
        if (gameObject != null && gameObject.GetComponent<BlurEffect>() == null)
        {
            this.m_camBlurEffect = gameObject.AddComponent<BlurEffect>();
            this.m_camBlurEffect.blurShader = Shader.Find("Hidden/BlurEffectConeTap");
            this.m_camBlurEffect.iterations = 2;
            this.m_camBlurEffect.enabled = false;
        }
        */
        this.MainCameraCullingMask = Camera.main.cullingMask;
        if (Application.isMobilePlatform)
        {
            Camera main = Camera.main;
            if (null != main)
            {
                Component component = main.gameObject.GetComponent("ContrastEnhance");
                UnityEngine.Object.Destroy(component);
                component = main.gameObject.GetComponent("Bloom");
                UnityEngine.Object.Destroy(component);
                component = main.gameObject.GetComponent("AntialiasingAsPostEffect");
                UnityEngine.Object.Destroy(component);
            }
        }
        Singleton<XRenderTextureManager>.singleton.CreateXRenderTexture(EnumRenderTextureType.eRenderTextureType_OutLine,this.outterLineCamera, (int)this.outterLineCamera.pixelWidth, (int)this.outterLineCamera.pixelHeight);
        Singleton<ClientMain>.singleton.Start();       
    }
    private void FixedUpdate()
    {
        Singleton<ClientMain>.singleton.FixedUpdate();
    }
    private void Update()
    {
        Singleton<ClientMain>.singleton.Update();
    }
    private void OnApplicationFocus(bool focusStatus)
    {
        this.m_bApplicationFocus = focusStatus;
    }
    public void Tick()
    {
        Singleton<ClientMain>.singleton.Tick();
    }
    #region 事件
    private void OnClick()
    {
        this.m_log.Debug("Onclick");
        Singleton<InputManager>.singleton.OnClick();
    }
    #endregion
    #endregion
}
