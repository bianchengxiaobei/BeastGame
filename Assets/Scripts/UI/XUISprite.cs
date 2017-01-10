using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using UnityAssetEx.Export;
using Utility.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUISprite
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.2
// 模块描述：UI精灵
/----------------------------------------------------------------*/
#endregion
[AddComponentMenu("XUI/XUISprite")]
public class XUISprite : XUIObject,IXUIObject,IXUISprite
{
	#region 字段
    private string m_spriteNameCached = string.Empty;
    private string m_atlasNameCached = string.Empty;
    private UISprite m_uiSprite;
    private UISpriteAnimation m_uiSpriteAnimation;
    private IAssetRequest m_assetRequestAtlas;
    private IAssetRequest m_assetRequestAtlasOld;
    private IXLog m_log = XLog.GetLog<XUISprite>();
	#endregion
	#region 属性
    /// <summary>
    /// UISprite的透明度
    /// </summary>
    public override float Alpha
    {
        get
        {
            if (this.m_uiSprite != null)
            {
                return this.m_uiSprite.alpha;
            }
            return 0f;
        }
        set
        {
            if (this.m_uiSprite != null)
            {
                this.m_uiSprite.alpha = value;
            }
        }
    }
    /// <summary>
    /// 取得精灵的颜色
    /// 如果精灵不存在的话，就设置颜色为白色
    /// </summary>
    public Color Color 
    {
        get 
        {
            if (this.m_uiSprite != null)
            {
                return this.m_uiSprite.color;
            }
            return Color.white;
        }
        set
        {
            if (null != this.m_uiSprite)
            {
                this.m_uiSprite.color = value;
            }
        }
    }
    /// <summary>
    /// 取得精灵的名字，所在图集里面的名字
    /// </summary>
    public string SpriteName 
    {
        get 
        {
            if (this.m_uiSprite != null)
            {
                return this.m_uiSprite.spriteName;
            }
            return null;
        }
        set 
        {

        }
    }
    /// <summary>
    /// 取得精灵所在的图集
    /// </summary>
    public IXUIAtlas UIAtlas
    {
        get
        {
            if (null == this.m_uiSprite)
            {
                return null;
            }
            if (null == this.m_uiSprite.atlas)
            {
                return null;
            }
            return this.m_uiSprite.atlas.GetComponent<XUIAtlas>();
        }
    }
	#endregion
	#region 公有方法
    public override void Init()
    {
        base.Init();
        this.m_uiSprite = base.GetComponent<UISprite>();
        if (null == this.m_uiSprite)
        {
            string hierarchy = NGUITools.GetHierarchy(base.gameObject);
            Debug.LogError("null == m_uiSprite:" + hierarchy);
        }
        this.m_uiSpriteAnimation = base.GetComponent<UISpriteAnimation>();
    }
    /// <summary>
    /// 开始播放帧动画
    /// </summary>
    /// <param name="bLoop">是否循环</param>
    /// <returns></returns>
    public bool PlayFlash(bool bLoop)
    {
        if (this.m_uiSpriteAnimation == null)
        {
            return false;
        }
        this.m_uiSpriteAnimation.loop = bLoop;
        this.m_uiSpriteAnimation.ResetToBeginning();
        this.m_uiSpriteAnimation.enabled = true;
        return true;
    }
    /// <summary>
    /// 停止播放帧动画
    /// </summary>
    /// <returns></returns>
    public bool StopFlash()
    {
        if (null == this.m_uiSpriteAnimation)
        {
            return false;
        }
        this.m_uiSpriteAnimation.enabled = false;
        return true;
    }
    public void SetEnable(bool bEnable)
    {
        Collider collider = base.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = bEnable;
        }
    }
    /// <summary>
    /// 设置精灵
    /// </summary>
    /// <param name="strSprite">精灵名称</param>
    /// <param name="strAtlas">精灵所在图集名称</param>
    /// <returns></returns>
    public bool SetSprite(string strSprite, string strAtlas)
    {
        if (null == this.m_uiSprite)
        {
            return false;
        }
        //如果设置的精灵所在图集和缓存图集一样，并且精灵名称也一样，则直接展示精灵
        if (this.m_atlasNameCached.Equals(strAtlas) && this.m_spriteNameCached.Equals(strSprite))
        {
            if (!this.m_uiSprite.enabled)
            {
                this.m_uiSprite.enabled = true;
            }
            return true;
        }
        //当设置的图集和精灵都不一样的时候，就要清除缓存图集，并且如果是动态图集的话，就减少引用次数，如果图集较大，则清除精灵资源
        if (!string.IsNullOrEmpty(this.m_atlasNameCached) && !string.IsNullOrEmpty(this.m_spriteNameCached))
        {
            this.RemoveSpriteRef(this.m_atlasNameCached, this.m_spriteNameCached);
            //清除缓存图集精灵
            this.m_spriteNameCached = string.Empty;
            this.m_spriteNameCached = string.Empty;
        }
        //重新设置缓存图集精灵
        this.m_spriteNameCached = strSprite;
        this.m_atlasNameCached = strAtlas;
        this.PrepareAtlas(strAtlas, strSprite);
        return true;
    }
    /// <summary>
    /// 设置精灵（缓存图集中取）
    /// </summary>
    /// <param name="strSpriteName"></param>
    /// <returns></returns>
    public bool SetSprite(string strSpriteName)
    {
        if (null == this.m_uiSprite)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(this.m_atlasNameCached))
        {
            this.SetSprite(this.m_atlasNameCached, strSpriteName);
        }
        else
        {
            if (!this.m_uiSprite.enabled)
            {
                this.m_uiSprite.enabled = true;
            }
            this.m_uiSprite.spriteName = strSpriteName;
        }
        return true;
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 加载图集完成之后，初始化精灵，并显示
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoadAtlasFinished(IAssetRequest assetRequest)
    {
        if (assetRequest.AssetResource == null || assetRequest.AssetResource.MainAsset == null)
        {
            return;
        }
        GameObject gameObject = assetRequest.AssetResource.MainAsset as GameObject;
        UIAtlas component = gameObject.GetComponent<UIAtlas>();
        if (null == component)
        {
            this.m_log.Error(string.Format("null == atlas:{0}", assetRequest.AssetResource.URL));
            return;
        }
        this.OnPrepareAtlas(assetRequest.AssetResource.URL, string.Empty, component);
    }
    /// <summary>
    /// 移除该精灵引用的次数（如果是动态图集的话）
    /// </summary>
    /// <param name="strAtlas"></param>
    /// <param name="strSprite"></param>
    private void RemoveSpriteRef(string strAtlas, string strSprite)
    {
        if (strAtlas.StartsWith("Dynamic_"))//如果图集是动态图集的话
        {
            DynamicAtlasManager.Instance.RemoveRefCount(strAtlas, strSprite, new DynamicAtlasManager.PrepareAtlasCallBack(this.OnPrepareAtlas));
        }
    }
    private void PrepareAtlas(string strAtlas, string strSprite)
    {
        //如果是动态图集的话
        if (strAtlas.StartsWith("Dynamic_", System.StringComparison.CurrentCultureIgnoreCase))
        {
            DynamicAtlasManager.Instance.AddRefCount(strAtlas, strSprite, new DynamicAtlasManager.PrepareAtlasCallBack(this.OnPrepareAtlas));
        }
        else
        {
            //如果不是动态图集的话
            if (this.m_assetRequestAtlas != null)
            {
                if (this.m_assetRequestAtlas.IsFinished)
                {
                    if (this.m_assetRequestAtlasOld != null)
                    {
                        this.m_assetRequestAtlasOld.Dispose();
                        this.m_assetRequestAtlasOld = null;
                    }
                    this.m_assetRequestAtlasOld = this.m_assetRequestAtlas;
                }
                else
                {
                    this.m_assetRequestAtlas.Dispose();
                }
                this.m_assetRequestAtlas = null;
            }
            this.m_assetRequestAtlas = ResourceManager.singleton.LoadAtlas(strAtlas, new AssetRequestFinishedEventHandler(this.OnLoadAtlasFinished), AssetPRI.DownloadPRI_Plain);
        }
    }
    /// <summary>
    /// 显示UISprite
    /// </summary>
    /// <param name="strAtlas"></param>
    /// <param name="strSprite"></param>
    /// <param name="atlas"></param>
    private void OnPrepareAtlas(string strAtlas, string strSprite, UIAtlas atlas)
    {
        if (string.IsNullOrEmpty(strAtlas) || atlas == null)
        {
            return;
        }
        if (this.m_atlasNameCached.Equals(strAtlas, System.StringComparison.OrdinalIgnoreCase))
        {
            this.m_uiSprite.atlas = atlas;
            this.m_uiSprite.spriteName = this.m_spriteNameCached;
            this.m_uiSprite.enabled = true;
        }
        else
        {
            this.m_log.Fatal("m_atlasNameCached.Equals(strAtlas, StringComparison.OrdinalIgnoreCase) == false:" + this.m_atlasNameCached);
        }
    }
    private void OnDestroy()
    {
        this.RemoveSpriteRef(this.m_atlasNameCached, this.m_spriteNameCached);
        if (this.m_assetRequestAtlasOld != null)
        {
            this.m_assetRequestAtlasOld.Dispose();
            this.m_assetRequestAtlasOld = null;
        }
        if (this.m_assetRequestAtlas != null)
        {
            this.m_assetRequestAtlas.Dispose();
            this.m_assetRequestAtlas = null;
        }
    }
    #endregion
}
