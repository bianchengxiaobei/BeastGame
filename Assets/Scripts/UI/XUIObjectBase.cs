using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI.UICommon.Local;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIObjectBase
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public abstract class XUIObjectBase : MonoBehaviour, IXUIObject
{
    #region 属性
    public string m_strTipText = "";
    private IXUIObject m_parent;
    private IXUIDlg m_parentDlg;
    private bool m_bInited;
    private GameObject m_Go;
    private Transform m_Trans;
    private bool m_bIsMouseIn;
    protected Bounds m_AbsoluteBounds = default(Bounds);
    protected bool m_bSizeChanged = true;
    protected float m_fAlpha = 1f;
    protected MouseOnEventHandler m_mouseOnEventHandler;
    protected MouseLeaveEventHandler m_mouseLeaveEventHandler;
    protected PressUpEventHandler m_eventHandlerPressUp;
    protected PressDownEventHandler m_eventHandlerPressDown;
    protected ClickEventHandler m_eventHandlerClick;
    protected LostFocusEventHandler m_eventHandlerLostFocus;
    protected GetFocusEventHandler m_eventHandlerGetFocus;
    private static FilterUIEventHandler s_actionUIEventFilter;
    public object TipParam
    {
        get;
        set;
    }
    public virtual IXUIObject parent
    {
        get
        {
            return this.m_parent;
        }
        set
        {
            this.m_parent = value;
        }
    }
    public IXUIDlg ParentDlg
    {
        get
        {
            return this.m_parentDlg;
        }
        set
        {
            this.m_parentDlg = value;
        }
    }
    public string Tip
    {
        get
        {
            return this.m_strTipText;
        }
        set
        {
            this.m_strTipText = value;
        }
    }
    public virtual Vector2 RealSize
    {
        get
        {
            Vector2 zero = Vector2.zero;
            zero.x = base.transform.lossyScale.x;
            zero.y = base.transform.lossyScale.y;
            return zero;
        }
    }
    public virtual Vector2 RelativeSize
    {
        get
        {
            Vector2 zero = Vector2.zero;
            zero.x = base.transform.localScale.x;
            zero.y = base.transform.localScale.y;
            return zero;
        }
    }
    public virtual Bounds AbsoluteBounds
    {
        get
        {
            return this.m_AbsoluteBounds;
        }
    }
    public Bounds RelativeBounds
    {
        get
        {
            Vector3 center = this.CachedTransform.InverseTransformPoint(this.AbsoluteBounds.min);
            Vector3 point = this.CachedTransform.InverseTransformPoint(this.AbsoluteBounds.max);
            Bounds result = new Bounds(center, Vector3.zero);
            result.Encapsulate(point);
            return result;
        }
    }
    public bool IsError
    {
        get
        {
            return false;
        }
    }
    public bool IsInited
    {
        get
        {
            return this.m_bInited;
        }
    }
    public GameObject CachedGameObject
    {
        get
        {
            if (this.m_Go == null)
            {
                this.m_Go = base.gameObject;
            }
            return this.m_Go;
        }
    }
    public Transform CachedTransform
    {
        get
        {
            if (this.m_Trans == null)
            {
                this.m_Trans = base.transform;
            }
            return this.m_Trans;
        }
    }
    public virtual float Alpha
    {
        get
        {
            return this.m_fAlpha;
        }
        set
        {
            this.m_fAlpha = value;
        }
    }
    public virtual bool IsEnableOpen
    {
        get;
        set;
    }
    public static FilterUIEventHandler ActionUIEventFilter
    {
        get
        {
            return XUIObjectBase.s_actionUIEventFilter;
        }
    }
    #endregion
    #region 方法
    public virtual IXUIObject GetUIObject(string strPath)
    {
        return null;
    }
    public bool IsVisible()
    {
        return base.gameObject.activeInHierarchy;
    }
    public static void RegisterClickFilter(FilterUIEventHandler actionFilter)
    {
        XUIObjectBase.s_actionUIEventFilter = actionFilter;
    }
    public void RegisterMouseOnEventHandler(MouseOnEventHandler eventHandler)
    {
        this.m_mouseOnEventHandler = eventHandler;
    }
    public void RegisterMouseLeaveEventHandler(MouseLeaveEventHandler eventHandler)
    {
        this.m_mouseLeaveEventHandler = eventHandler;
    }
    public void RegisterPressUpEventHandler(PressUpEventHandler eventHandler)
    {
        this.m_eventHandlerPressUp = eventHandler;
    }
    public void RegisterPressDownEventHandler(PressDownEventHandler eventHandler)
    {
        this.m_eventHandlerPressDown = eventHandler;
    }
    public void RegisterClickEventHandler(ClickEventHandler eventHandler)
    {
        this.m_eventHandlerClick = eventHandler;
    }
    public void RegisterLostFocusEventHandler(LostFocusEventHandler eventHandler)
    {
        this.m_eventHandlerLostFocus = eventHandler;
    }
    public void RegisterGetFocusEventHandler(GetFocusEventHandler eventHandler)
    {
        this.m_eventHandlerGetFocus = eventHandler;
    }
    public virtual void SetVisible(bool bVisible)
    {
    }
    public virtual bool IsMouseIn()
    {
        return this.m_bIsMouseIn;
    }
    public virtual void OnFocus()
    {
        if (this.parent != null)
        {
            this.parent.OnFocus();
        }
    }
    public void OnResolutionChange()
    {
        this.m_bSizeChanged = true;
    }
    public virtual void Highlight(bool bTrue)
    {
    }
    public virtual void Init()
    {
        this.m_bInited = true;
    }
    protected virtual void OnAwake()
    {
    }
    protected virtual void OnStart()
    {
    }
    protected virtual void OnUpdate()
    {
    }
    protected virtual void OnMouseOn()
    {
    }
    protected virtual void OnMouseLeave()
    {
    }
    protected virtual void OnPressDown()
    {
    }
    protected virtual void OnPressUp()
    {
    }
    protected virtual void _OnClick()
    {
    }
    private void Awake()
    {
        this.OnAwake();
    }
    private void Start()
    {
        if (!this.m_bInited)
        {
            this.Init();
        }
        this.OnStart();
    }
    private void Update()
    {
        this.OnUpdate();
    }
    private void OnHover(bool bHover)
    {
        if (!this.m_bIsMouseIn && bHover)
        {
            this.m_bIsMouseIn = true;
            this.OnMouseOn();
        }
        if (this.m_bIsMouseIn && !bHover)
        {
            this.m_bIsMouseIn = false;
            this.OnMouseLeave();
        }
    }
    private void OnPress(bool bPressed)
    {
        if (bPressed)
        {
            this.OnFocus();
            this.OnPressDown();
            return;
        }
        this.OnPressUp();
    }
    private void OnClick()
    {
        if (Singleton<LocalUIManagerBase>.singleton.GetCurrentTouchID() != -2)
        {
            this._OnClick();
        }
    }
    private void OnSelect(bool bSelect)
    {
        if (bSelect)
        {
            if (this.m_eventHandlerGetFocus != null && this.m_eventHandlerGetFocus(this))
            {
                Singleton<LocalUIManagerBase>.singleton.SetEventProcessed(true);
                return;
            }
        }
        else
        {
            if (this.m_eventHandlerLostFocus != null && this.m_eventHandlerLostFocus(this))
            {
                Singleton<LocalUIManagerBase>.singleton.SetEventProcessed(true);
            }
        }
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        this.OnHover(false);
    }
    #endregion
}
