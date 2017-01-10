using UnityEngine;
using System.Collections.Generic;
using Client.UI.UICommon;
using Client.Common;
using UILib.Export;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIListItem
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.2
// 模块描述：单个列表UI项
//----------------------------------------------------------------*/
#endregion
[AddComponentMenu("XUI/XUIListItem")]
public class XUIListItem : XUIObject, IXUIObject,IXUIListItem
{
	#region 字段
    public UISprite m_uiSpriteIcon;
    public UISprite m_uiSpriteShadow;
    public UITexture m_uiTexture;
    public UILabel[] m_uiLabels;
    
    private UIButton m_uiButton;
    private UIButtonScale m_uiButtonScale;
    private UIToggle m_uiCheckBox;
    private UIPanel m_uiPanel;
    private UIPlaySound[] m_uiButtonSounds;
    private UISprite[] m_uiSprites;
    private UITexture[] m_uiTextures;
    protected XUIGroup m_xuiGroupFlash;
    private XUISprite m_xuiSpriteFlash;
    private Dictionary<string, XUIObjectBase> m_dicId2UIObject = new Dictionary<string, XUIObjectBase>();
    private Dictionary<UILabel, Color> m_dicInitColorLabel = new Dictionary<UILabel, Color>();
    public int m_nIndex = -1;
    public long m_unId;
    public ulong m_ulGUID;
    private bool m_bUpdateTransform;
    private float m_fFinishPosZ;
    private float m_fFinishAlpha = 1f;
    private bool m_bSticky;
    private Vector3 m_vFinishScale = Vector3.zero;
    protected Collider m_collider;
    private Color m_colorHighlight = Color.clear;
    private string m_strTextureFile = string.Empty;
    private IAssetRequest m_assetRequest;
    private IAssetRequest m_assetRequestOld;
	#endregion
	#region 属性
    /// <summary>
    /// 单个项所在列表的id
    /// </summary>
    public long Id 
    {
        get { return this.m_unId; }
        set { this.m_unId = value; }
    }
    /// <summary>
    /// 单个项所在游戏中的id
    /// </summary>
    public ulong GUID 
    {
        get { return this.m_ulGUID; }
        set { this.m_ulGUID = value; }
    }
    /// <summary>
    /// 单个项所在列表的索引
    /// </summary>
    public int Index 
    {
        get { return this.m_nIndex; }
        set { this.m_nIndex = value; }
    }
    /// <summary>
    /// 是否该单项被选中
    /// </summary>
    public bool IsSelected 
    {
        get 
        {
            return this.m_uiCheckBox != null && this.m_uiCheckBox.value;
        }
        set 
        {
            if (this.m_uiCheckBox != null && this.m_uiCheckBox.value != value)
            {
                this.m_uiCheckBox.Set(value);//设置UIToggle为value
                if (value)
                {
                    //在列表中选中该单项
                    this.ParentXUIList.SelectItem(this, false);
                }
                else 
                {
                    this.ParentXUIList.UnSelectItem(this, false);
                }
            }
        }
    }
    /// <summary>
    /// 父亲列表
    /// </summary>
    public XUIList ParentXUIList
    {
        get 
        {
            XUIList xuiList = this.parent as XUIList;
            if (null == xuiList)
            {
                Debug.LogError("null == uiList");
            }
            return xuiList;
        }
    }
    /// <summary>
    /// 高亮颜色
    /// </summary>
    public Color HighlightColor 
    {
        get { return this.m_colorHighlight; }
        set 
        {
            if (this.m_colorHighlight != value)
            {
                this.m_colorHighlight = value;
                if (this.m_xuiGroupFlash != null && XUITool.Instance != null)
                {
                    XUITool.Instance.SetColor(this.m_xuiGroupFlash.CachedGameObject, this.m_colorHighlight);
                }
            }
        }

    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 设置可以多选
    /// </summary>
    /// <param name="bEnable"></param>
    public void SetEnableMultiSelect(bool bEnable)
    {
        if (this.m_uiCheckBox != null)
        {
            if (bEnable)
            {
                this.m_uiCheckBox.group = 0;
            }
        }
    }
    /// <summary>
    /// 刷新单项大小（主要更改碰撞器大小）
    /// </summary>
    public void RefreshSize()
    {
        Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(this.CachedTransform);
        if (base.collider != null)
        {
            BoxCollider boxCollider = base.collider as BoxCollider;
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
            }
        }
    }
    /// <summary>
    /// 设置单项为是否选择上
    /// </summary>
    /// <param name="value"></param>
    public void SetSelected(bool value)
    {
        if (this.m_uiCheckBox != null && this.m_uiCheckBox.value != value)
        {
            this.m_uiCheckBox.Set(value);
        }
    }
    /// <summary>
    /// 根据选中单项的索引初始化其他单项的位置大小
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="nCurIndex"></param>
    public void InitStatus(EnumMoveCenterType eType, int nCurIndex)
    {
        this.InitMoveCenterInfo(eType, nCurIndex);
        if (eType == EnumMoveCenterType.eMoveCenterType_Increas)
        {
            if (this.m_uiPanel != null)
            {
                this.m_uiPanel.alpha = this.m_fFinishAlpha;
            }
            Vector3 localPosition = base.transform.position;
            localPosition.z = this.m_fFinishPosZ;
            base.transform.localPosition = localPosition;
            base.transform.localScale = this.m_vFinishScale;
        }
    }
    /// <summary>
    /// 根据需要移中的索引初始化位置信息
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="nCurIndex"></param>
    public void InitMoveCenterInfo(EnumMoveCenterType eType,int nCurIndex)
    {
        //单项间隔多少距离
        int dis = Mathf.Abs(this.m_nIndex - nCurIndex);
        this.m_bUpdateTransform = false;
        this.m_vFinishScale = base.transform.localScale;
        this.m_fFinishPosZ = base.transform.localPosition.z;
        if (eType == EnumMoveCenterType.eMoveCenterType_Increas)
        {
            this.m_fFinishAlpha = dis <= 2 ? 1 : 0;
            this.m_vFinishScale.x = 1f - dis * 0.1f;
            this.m_vFinishScale.y = 1f - dis * 0.1f;
            this.m_fFinishPosZ = 1f * dis;
            this.m_bUpdateTransform = true;
            if (this.m_nIndex == nCurIndex)
            {
                Vector3 localPosition = base.transform.localPosition;
                localPosition.z = -1;
                base.transform.localPosition = localPosition;
            }
        }
    }
    /// <summary>
    /// 设置触摸碰撞是不是可用
    /// </summary>
    /// <param name="bEnable"></param>
    public void SetEnable(bool bEnable)
    {
        if (null != this.m_collider)
        {
            this.m_collider.enabled = bEnable;
        }
        if (null != this.m_uiButton)
        {
            this.m_uiButton.isEnabled = bEnable;
        }
    }
    public void SetEnableSelect(bool bEnable)
    {
        if (!bEnable && null != this.m_uiCheckBox)
        {
            this.m_uiCheckBox.value = false;
        }
        this.Highlight(bEnable);
        if (null != this.m_uiCheckBox)
        {
            this.m_uiCheckBox.enabled = bEnable;
        }
    }
    /// <summary>
    /// 设置大小
    /// </summary>
    /// <param name="cellWidth"></param>
    /// <param name="cellHeight"></param>
    public void SetSize(float cellWidth, float cellHeight)
    {
        if (null != this.m_uiSpriteIcon)
        {
            this.m_uiSpriteIcon.transform.localScale = new Vector3(cellWidth, cellHeight, 1f);
        }
        if (null != this.m_uiTexture)
        {
            this.m_uiTexture.transform.localScale = new Vector3(cellWidth, cellHeight, 1f);
        }
        this.m_bSizeChanged = true;
    }
    /// <summary>
    /// 设置是否可见
    /// </summary>
    /// <param name="strId"></param>
    /// <param name="vVisible"></param>
    public void SetVisible(string strId, bool bVisible)
    {
        IXUIObject uIObject = this.GetUIObject(strId);
        if (uIObject != null)
        {
            uIObject.SetVisible(bVisible);
        }
    }
    /// <summary>
    /// 设置sprite,Texture的颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        if (null != this.m_uiSpriteIcon)
        {
            this.m_uiSpriteIcon.color = color;
        }
        if (null != this.m_uiTexture)
        {
            this.m_uiTexture.color = color;
        }
    }
    /// <summary>
    /// 根据id取得Label，设置文字内容
    /// </summary>
    /// <param name="strId">UI的id</param>
    /// <param name="strText">文字</param>
    /// <returns></returns>
    public bool SetText(string strId, string strText)
    {
        IXUILabel iXUILabel = this.GetUIObject(strId) as IXUILabel;
        if (iXUILabel != null)
        {
            iXUILabel.SetText(strText);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置IconSprite精灵
    /// </summary>
    /// <param name="strSprite"></param>
    public void SetIconSprite(string strSprite)
    {
        if (null != this.m_uiSpriteIcon)
        {
            this.m_uiSpriteIcon.spriteName = strSprite.Substring(strSprite.LastIndexOf("\\") + 1);
            this.m_uiSpriteIcon.enabled = true;
        }
        XUISprite xUISprite = this.GetUIObject("Sprite_Icon") as XUISprite;
        if (null != xUISprite)
        {
            xUISprite.SetSprite(strSprite);
        }
    }
    /// <summary>
    /// 设置IconSprite精灵，使用其他图集
    /// </summary>
    /// <param name="strSprite"></param>
    /// <param name="strAtlas"></param>
    public void SetIconSprite(string strSprite, string strAtlas)
    {
        XUISprite xUISprite = this.GetUIObject("Sprite_Icon") as XUISprite;
        if (null != xUISprite)
        {
            xUISprite.SetSprite(strSprite, strAtlas);
        }
    }
    /// <summary>
    /// 根据UI的id设置精灵
    /// </summary>
    /// <param name="strId"></param>
    /// <param name="strSprite"></param>
    public void SetSprite(string strId, string strSprite)
    {
        XUISprite xUISprite = this.GetUIObject(strId) as XUISprite;
        if (null != xUISprite)
        {
            xUISprite.SetSprite(strSprite);
        }
    }
    /// <summary>
    /// 根据路径取得资源设置IconTexture
    /// </summary>
    /// <param name="strTexture"></param>
    public void SetIconTexture(string strTexture)
    {
        if (null != this.m_uiTexture && !this.m_strTextureFile.Equals(strTexture))
        {
            this.m_strTextureFile = strTexture;
            if (this.m_assetRequest != null)
            {
                if (this.m_assetRequest.IsFinished)
                {
                    if (this.m_assetRequestOld != null)
                    {
                        this.m_assetRequestOld.Dispose();
                        this.m_assetRequestOld = null;
                    }
                    this.m_assetRequestOld = this.m_assetRequest;
                }
                else
                {
                    this.m_assetRequest.Dispose();
                }
                this.m_assetRequest = null;
            }
            this.m_assetRequest = XUITool.Instance.LoadResAsyn(this.m_strTextureFile, new AssetRequestFinishedEventHandler(this.OnLoadTextureFinished), AssetPRI.DownloadPRI_Low);
        }
        XUIPicture xUIPicture = this.GetUIObject("Texture_Icon") as XUIPicture;
        if (null != xUIPicture)
        {
            xUIPicture.SetTexture(strTexture);
        }
    }
    /// <summary>
    /// 取得全部的ui
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, XUIObjectBase> GetAllXUIObj()
    {
        return this.m_dicId2UIObject;
    }
    /// <summary>
    /// 清空ListItem里面的组件，label设空，精灵和texture设置为false
    /// </summary>
    public void Clear()
    {
        this.m_unId = 0u;
        UILabel[] uiLabels = this.m_uiLabels;
        //清空所有Label的文字
        for (int i = 0; i < uiLabels.Length; i++)
        {
            UILabel uILabel = uiLabels[i];
            uILabel.text = string.Empty;
        }
        //关闭精灵的显示
        if (null != this.m_uiSpriteIcon)
        {
            this.m_uiSpriteIcon.enabled = false;
        }
        if (null != this.m_uiTexture)
        {
            this.m_uiTexture.enabled = false;
        }
        //设置提示为空
        this.Tip = string.Empty;
        this.TipParam = null;
    }
    /// <summary>
    /// 高亮显示(实际播放帧动画)
    /// </summary>
    /// <param name="bTrue"></param>
    public override void Highlight(bool bTrue)
    {
        if (this.m_xuiSpriteFlash != null)
        {
            if (bTrue)
            {
                this.m_xuiSpriteFlash.PlayFlash(true);
            }
            else 
            {
                this.m_xuiSpriteFlash.StopFlash();
            }
        }
        if (this.m_xuiGroupFlash != null)
        {
            NGUITools.SetActiveChildren(this.m_xuiGroupFlash.CachedGameObject, bTrue);
        }
    }
    /// <summary>
    /// 根据Hirearchy层级路径获取UI物体（实际是获取组件名字，然后去缓存中取）
    /// </summary>
    /// <param name="strPath"></param>
    /// <returns></returns>
    public override IXUIObject GetUIObject(string strPath)
    {
        if (null == strPath)
        {
            return null;
        }
        string key = strPath;
        int index = strPath.LastIndexOf('/');
        if (index > 0)
        {
            //截取路径后面的UI名称，也就是ui的id
            key = strPath.Substring(index + 1);
        }
        XUIObjectBase result;
        if (this.m_dicId2UIObject.TryGetValue(key, out result))
        {
            return result;
        }
        return null;
    }
    public override void Init()
    {
        base.Init();
        if (XUITool.Instance != null) 
        {
            string uiObjectId = WidgetFactory.GetUIObjectId(this);
            string tip = XUITool.Instance.GetTip(uiObjectId);
            if (!string.IsNullOrEmpty(tip))
            {
                this.Tip = tip;
            }
        }
        WidgetFactory.FindAllUIObjects(base.transform,this, ref this.m_dicId2UIObject);
        foreach (var current in this.m_dicId2UIObject.Values)
        {
            current.parent = this;
            current.ParentDlg = this.ParentDlg;
            if (XUITool.Instance != null)
            {
                string uIObjectId2 = WidgetFactory.GetUIObjectId(current);
			    string tip2 = XUITool.Instance.GetTip(uIObjectId2);
			    if (!string.IsNullOrEmpty(tip2))
			    {
				    current.Tip = tip2;
			    }
            }
            if (!current.IsInited)
            {
                current.Init();
            }
        }
        if (null == this.m_uiSpriteIcon)
	    {
		    Transform transform = base.transform.FindChild("Sprite_Icon");
		    if (null != transform)
		    {
			    this.m_uiSpriteIcon = transform.GetComponent<UISprite>();
		    }
	    }
	    if (null == this.m_uiTexture)
	    {
		    Transform transform2 = base.transform.FindChild("Texture_Icon");
		    if (null != transform2)
		    {
			    this.m_uiTexture = transform2.GetComponent<UITexture>();
		    }
	    }
        this.m_collider = base.GetComponent<Collider>();
	    this.m_uiLabels = base.GetComponentsInChildren<UILabel>();
	    this.m_uiCheckBox = base.GetComponent<UIToggle>();
	    this.m_uiButton = base.GetComponent<UIButton>();
	    this.m_uiButtonScale = base.GetComponent<UIButtonScale>();
	    this.m_uiPanel = base.GetComponent<UIPanel>();
	    this.m_uiButtonSounds = base.GetComponents<UIPlaySound>();
	    this.m_uiSprites = base.GetComponentsInChildren<UISprite>(true);
	    this.m_uiTextures = base.GetComponentsInChildren<UITexture>(true);
        this.m_dicInitColorLabel.Clear();
        //所有label的颜色都缓存起来
	    UILabel[] uiLabels = this.m_uiLabels;
        for (int i = 0; i < uiLabels.Length; i++)
	    {
		    UILabel uILabel = uiLabels[i];
		    if (null != uILabel && !this.m_dicInitColorLabel.ContainsKey(uILabel))
		    {
			    this.m_dicInitColorLabel.Add(uILabel, uILabel.color);
		    }
	    }
        //添加checkbox的事件监听，点击选中和不选中
        if (null != this.m_uiCheckBox)
	    {
		    EventDelegate.Add(this.m_uiCheckBox.onChange,OnSelectStateChange);
	    }
        //初始化动画帧精灵
        this.m_xuiSpriteFlash = (this.GetUIObject("Sprite_Flash") as XUISprite);
	    this.m_xuiGroupFlash = (this.GetUIObject("Group_Flash") as XUIGroup);
        //不高亮显示
        this.Highlight(false);
    }
    protected override void _OnClick()
    {
        base._OnClick();
        this.ParentXUIList._OnClick(this);
    }
    protected override void OnPressDown()
    {
        base.OnPressDown();
        this.ParentXUIList.OnPressDown(this);
        if (base.enabled && !UICamera.current.stickyPress)
        {
            this.m_bSticky = true;
            //UICamera.current.stickyPress = true;
        }
    }
    protected override void OnPressUp()
    {
        base.OnPressUp();
        this.ParentXUIList.OnPressUp(this);
        if (base.enabled && this.m_bSticky)
        {
            this.m_bSticky = false;
            //UICamera.current.stickyPress = false;
        }
    }
    protected override void OnMouseOn()
    {
        base.OnMouseOn();
        this.ParentXUIList.OnMouseOn(this);
    }
    protected override void OnMouseLeave()
    {
        base.OnMouseLeave();
        this.ParentXUIList.OnMouseLeave(this);
    }
	#endregion
	#region 私有方法
    private void OnSelectStateChange()
    {
        bool bSelected = UIToggle.current.value;
	    if (bSelected)
	    {
		    this.ParentXUIList.OnSelectItem(this);
	    }
	    else
	    {
		    this.ParentXUIList.OnUnSelectItem(this);
	    }
    }
    /// <summary>
    /// 加载Texture完成回调，取得资源显示
    /// </summary>
    /// <param name="assetRequest"></param>
    private void OnLoadTextureFinished(IAssetRequest assetRequest)
    {
        IAssetResource assetResource = assetRequest.AssetResource;
        if (assetResource == null)
        {
            return;
        }
        Object mainAsset = assetResource.MainAsset;
        Texture texture = mainAsset as Texture;
        if (null == texture)
        {
            return;
        }
        this.m_uiTexture.mainTexture = texture;
        this.m_uiTexture.enabled = true;
        if (this.m_assetRequestOld != null)
        {
            this.m_assetRequestOld.Dispose();
            this.m_assetRequestOld = null;
        }
    }
    private void OnDrag(Vector2 delta)
    {
        UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
        this.ParentXUIList._OnDrag(this,delta);
    }
    private void OnDrop(GameObject obj)
    {
        XUIListItem component = obj.GetComponent<XUIListItem>();
        if (null != component)
        {
            this.ParentXUIList._OnDrop(component, this);
        }
    }
    private void OnDragRelease(GameObject obj)
    {
        XUIListItem component = obj.GetComponent<XUIListItem>();
        this.ParentXUIList._OnDragRelease(this, component);
    }
	#endregion
}
