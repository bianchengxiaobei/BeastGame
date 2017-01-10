using UnityEngine;
using System.Collections.Generic;
using System;
using UnityAssetEx.Export;
using Client.Common;
using System.IO;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DynamicAtlas
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.3
// 模块描述：动态图集
//----------------------------------------------------------------*/
#endregion
public class DynamicAtlas : IDisposable
{
	#region 字段
    public UIAtlas m_uiAtlas;
    public Shader m_shader = Shader.Find("Unlit/Transparent Colored2");
    private Material m_mat;
    private Texture2D m_tex;
    private GameObject m_Obj;
    private int i_maximumAtlasSize = 2048;
    private bool m_bDisposed;
    private string m_strAtlasName = string.Empty;
    private TextureFormat m_TextureFormat = TextureFormat.RGBA32;
    private Dictionary<string, DynamicAtlas.SpriteEntry> m_dicSpriteEntrys = new Dictionary<string, SpriteEntry>();
    private Dictionary<string, IAssetRequest> m_dicAssetRequestTexture = new Dictionary<string, IAssetRequest>();
    private Dictionary<string, DynamicAtlasManager.PrepareAtlasCallBack> m_dicPrepareCallBack = new Dictionary<string, DynamicAtlasManager.PrepareAtlasCallBack>();
	#endregion
	#region 属性
    /// <summary>
    /// 图集名称
    /// </summary>
    public string AtlasName
    {
        get
        {
            return this.m_strAtlasName;
        }
    }
	#endregion
	#region 构造方法
    public DynamicAtlas(string strAtlasName)
    {
        this.m_strAtlasName = strAtlasName;
        this.Init(DynamicAtlasManager.Instance.CachedTransform, (!Application.isMobilePlatform) ? TextureFormat.RGBA32 : TextureFormat.PVRTC_RGBA4);
    }
	#endregion
	#region 公有方法
    /// <summary>
    /// 初始化图集
    /// </summary>
    /// <param name="AtParent">父亲节点</param>
    /// <param name="format">图片格式</param>
    private void Init(Transform AtParent, TextureFormat format)
    {
        this.m_TextureFormat = format;
        if (this.m_tex == null)
        {
            this.m_tex = new Texture2D(1024, 1024, this.m_TextureFormat, false);
        }
        this.m_mat = new Material(this.m_shader);
        if (this.m_uiAtlas == null)
        {
            this.m_Obj = new GameObject(this.m_strAtlasName);
            this.m_uiAtlas = this.m_Obj.AddComponent<UIAtlas>();
            this.m_Obj.transform.parent = AtParent;
        }
        if (this.m_uiAtlas.spriteMaterial == null)
        {
            this.m_uiAtlas.spriteMaterial = this.m_mat;
        }
    }
   /// <summary>
   /// 释放资源
   /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        //如果已经释放就返回
        if (this.m_bDisposed)
        {
            return;
        }
        if (disposing)
        {
            this.Clear();
        }
        foreach (var current in this.m_dicAssetRequestTexture)
        {
            current.Value.Dispose();
        }
        this.m_dicAssetRequestTexture.Clear();
        NGUITools.Destroy(this.m_tex);
        this.m_tex = null;
        NGUITools.Destroy(this.m_Obj);
        this.m_Obj = null;
        this.m_bDisposed = true;
    }
    /// <summary>
    /// 清除该精灵所有资源
    /// </summary>
    public void Clear()
    {
        foreach (var current in this.m_dicSpriteEntrys.Values)
        {
            this.ReleaseSprite(current);
        }
        this.m_dicSpriteEntrys.Clear();
    }
    public void CreateAtlasFromTex(List<Texture2D> textures, bool isSaveFile=false)
    {
        if (textures == null || textures.Count <= 0)
        {
            Debug.LogError("textures is null or count <= 0 !!");
            return;
        }
        this.AddTexList(textures);
        if (isSaveFile)
        {
            string path = Application.dataPath + "/outSprite/";
            Directory.CreateDirectory(path);
            File.WriteAllBytes(path + "dynamic.png", this.m_tex.EncodeToPNG());
        }
    }
    public void AddTexList(List<Texture2D> addTextures)
    {
        for (int i = 0; i < addTextures.Count; i++)
        {
            //如果添加的图片不在
            if (!this.m_dicSpriteEntrys.ContainsKey(addTextures[i].name))
            {
                Debug.LogError("m_dicSpriteEntrys.ContainsKey(addTextures[i].name) == false:" + addTextures[i].name);
            }
            else
            {
                //初始化精灵
                this.m_dicSpriteEntrys[addTextures[i].name].Init(addTextures[i], this.m_TextureFormat);
            }
        }
        this.GenerateAtlas();
    }
    /// <summary>
    /// 增加精灵引用次数，如果不存在就动态创建加载
    /// </summary>
    /// <param name="texName"></param>
    /// <param name="callBack"></param>
    public void AddRefCount(string texName, DynamicAtlasManager.PrepareAtlasCallBack callBack)
    {
        if (string.IsNullOrEmpty(texName))
        {
            return;
        }
        //如果该动态图集不存在该精灵，就创建一个该精灵
        if (!this.m_dicSpriteEntrys.ContainsKey(texName))
        {
            this.m_dicSpriteEntrys[texName] = new DynamicAtlas.SpriteEntry(texName);
        }
        //精灵引用次数加一
        this.m_dicSpriteEntrys[texName].AddRef();
        if (this.m_dicSpriteEntrys[texName].IsPrepared)
        {
            if (callBack != null)
            {
                try
                {
                    callBack(this.AtlasName, texName, this.m_uiAtlas);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }
        else
        {
            if (callBack != null)
            {
                //如果精灵准备好的回调函数不为空的话，就加入到缓存中
                if (!this.m_dicPrepareCallBack.ContainsKey(texName))
                {
                    this.m_dicPrepareCallBack[texName] = callBack;
                }
                else
                {
                    DynamicAtlasManager.PrepareAtlasCallBack a = this.m_dicPrepareCallBack[texName];
                    this.m_dicPrepareCallBack[texName] = (DynamicAtlasManager.PrepareAtlasCallBack)Delegate.Combine(a, callBack);
                }
            }
            //加载该精灵资源
            if (!this.m_dicAssetRequestTexture.ContainsKey(texName))
            {
                string url = string.Format("Texture/{0}/{1}", this.m_strAtlasName, texName);
                IAssetRequest value = XUITool.Instance.LoadResAsyn(url, new AssetRequestFinishedEventHandler(this.OnLoadTextureFinished), AssetPRI.DownloadPRI_Low);
                this.m_dicAssetRequestTexture[texName] = value;
            }
        }
    }
    /// <summary>
    /// 移除该精灵引用
    /// </summary>
    /// <param name="texName"></param>
    /// <param name="callBack"></param>
    public void RemoveRefCount(string texName, DynamicAtlasManager.PrepareAtlasCallBack callBack)
    {
        if (this.m_dicPrepareCallBack.ContainsKey(texName))
        {
            Delegate.Remove(this.m_dicPrepareCallBack[texName], callBack);
        }
        else
        {
            Debug.LogError("m_dicPrepareCallBack.ContainsKey(texName) == false:" + texName);
        }
        this.m_dicSpriteEntrys[texName].DelRef();
        //当图集中精灵数量超过30张，并且该精灵没有被引用，就释放该精灵
        if (this.m_dicSpriteEntrys[texName].refCount <= 0 && this.m_dicSpriteEntrys.Count > 30)
        {
            this.ReleaseExtraSprites();
        }
    }
    /// <summary>
    /// 是否有该精灵
    /// </summary>
    /// <param name="texName"></param>
    /// <returns></returns>
    public bool Contains(string texName)
    {
        return this.m_dicSpriteEntrys.ContainsKey(texName);
    }
    #endregion
	#region 私有方法
    /// <summary>
    /// 释放加载进来的精灵（IAssetRequest.Dispose()）
    /// </summary>
    /// <param name="sprites"></param>
    private void ReleaseSprite(DynamicAtlas.SpriteEntry sprites)
    {
        sprites.Clear();
        IAssetRequest assetRequest = null;
        if (this.m_dicAssetRequestTexture.TryGetValue(sprites.spriteName, out assetRequest))
        {
            assetRequest.Dispose();
            this.m_dicAssetRequestTexture.Remove(sprites.spriteName);
        }
    }
    /// <summary>
    /// 释放没有用的精灵
    /// </summary>
    private void ReleaseExtraSprites()
    {
        List<string> list = new List<string>();
        foreach (string current in this.m_dicSpriteEntrys.Keys)
        {
            //循环遍历该图集中的所有精灵，如果发现没有被引用到的，直接释放
            if (this.m_dicSpriteEntrys[current].refCount <= 0)
            {
                this.ReleaseSprite(this.m_dicSpriteEntrys[current]);
                list.Add(current);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            this.m_dicSpriteEntrys.Remove(list[i]);
        }
    }
    private void OnLoadTextureFinished(IAssetRequest assetRequest)
    {
        if (null == assetRequest || null == assetRequest.AssetResource)
        {
            return;
        }
        //取得加载进来的图片
        Texture2D texture2D = assetRequest.AssetResource.MainAsset as Texture2D;
        if (texture2D != null)
        {
            this.CreateAtlasFromTex(new List<Texture2D>
            {
                texture2D
            });
            if (this.m_dicPrepareCallBack.ContainsKey(texture2D.name))
            {
                if (this.m_dicPrepareCallBack[texture2D.name] != null)
                {
                    try
                    {
                        this.m_dicPrepareCallBack[texture2D.name](this.AtlasName, texture2D.name, this.m_uiAtlas);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                this.m_dicPrepareCallBack[texture2D.name] = null;
            }
        }
    }
    /// <summary>
    /// 生成图集
    /// </summary>
    private void GenerateAtlas()
    {
        List<DynamicAtlas.SpriteEntry> list = new List<DynamicAtlas.SpriteEntry>();
        foreach (DynamicAtlas.SpriteEntry current in this.m_dicSpriteEntrys.Values)
        {
            if (current != null && current.IsPrepared)
            {
                list.Add(current);
            }
        }
        this.m_uiAtlas.spriteMaterial.mainTexture = this.UpdateTexture(this.m_uiAtlas, list);
        this.ReplaceSprites(this.m_uiAtlas, list);
    }
    /// <summary>
    /// 更新图集
    /// </summary>
    /// <param name="atlas"></param>
    /// <param name="sprites"></param>
    /// <returns></returns>
    private Texture2D UpdateTexture(UIAtlas atlas, List<DynamicAtlas.SpriteEntry> sprites)
    {
        this.PackTextures(this.m_tex, sprites);
        atlas.spriteMaterial.mainTexture = this.m_tex;
        return this.m_tex;
    }
    /// <summary>
    /// 打包图集
    /// </summary>
    /// <param name="tex">创建的图集</param>
    /// <param name="sprites">要打包的textures</param>
    private void PackTextures(Texture2D tex, List<DynamicAtlas.SpriteEntry> sprites)
    {
        Texture2D[] texArray = new Texture2D[sprites.Count];
        for (int i = 0; i < sprites.Count; i++)
        {
            texArray[i] = sprites[i].tex;
        }
        //创建图集，返回每张的uv坐标
        Rect[] rectArray = tex.PackTextures(texArray, 1, this.i_maximumAtlasSize);
        for (int j = 0; j < sprites.Count; j++)
        {
            sprites[j].rect = NGUIMath.ConvertToPixels(rectArray[j], tex.width, tex.height,true);
        }
    }
    private void ReplaceSprites(UIAtlas atlas, List<DynamicAtlas.SpriteEntry> sprites)
    {
        List<UISpriteData> spriteList = atlas.spriteList;
        List<UISpriteData> list = new List<UISpriteData>();
        for (int i = 0; i < sprites.Count; i++)
        {
            DynamicAtlas.SpriteEntry se = sprites[i];
            UISpriteData item = this.AddSprite(spriteList, se);
            list.Add(item);
        }
        int count = spriteList.Count;
        while (count > 0)
        {
            UISpriteData spriteData = spriteList[--count];
            if (list.Contains(spriteData))
            {
                spriteList.RemoveAt(count);
            }
        }
        atlas.MarkAsChanged();
    }
    /// <summary>
    /// 添加精灵到图集里面
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="se"></param>
    /// <returns></returns>
    private UISpriteData AddSprite(List<UISpriteData> sprites, DynamicAtlas.SpriteEntry se)
    {
        UISpriteData sprite = null;
        foreach (var current in sprites)
        {
            if (current.name == se.tex.name)
            {
                sprite = current;
                break;
            }
        }
        if (sprite != null)
        {
            sprite.CopyFrom(se);
        }
        else 
        {
            sprite = new UISpriteData();
            sprite.CopyFrom(se);
            sprites.Add(sprite);
        }
        return sprite;
    }
	#endregion
    #region 内部类
    private class SpriteEntry : UISpriteData
    {
        #region 字段
        private Texture2D m_tex;
        private Rect m_rect;
        private int m_minX;
        private int m_maxX;
        private int m_minY;
        private int m_maxY;
        private int m_refCount;
        private string m_spriteName = string.Empty;
        private bool m_bPrepared;
        #endregion
        #region 属性
        public Texture2D tex
        {
            get
            {
                return this.m_tex;
            }
        }
        public Rect rect
        {
            get
            {
                return this.m_rect;
            }
            set
            {
                this.m_rect = value;
            }
        }
        public int minX
        {
            get
            {
                return this.m_minX;
            }
        }
        public int maxX
        {
            get
            {
                return this.m_maxX;
            }
        }
        public int minY
        {
            get
            {
                return this.m_minY;
            }
        }
        public int maxY
        {
            get
            {
                return this.m_maxY;
            }
        }
        public int refCount
        {
            get
            {
                return this.m_refCount;
            }
        }
        public string spriteName
        {
            get
            {
                return this.m_spriteName;
            }
        }
        public bool IsPrepared
        {
            get
            {
                return this.m_bPrepared;
            }
        }
        #endregion
        #region 构造函数
        public SpriteEntry(string strName)
        {
            this.m_spriteName = strName;
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="textureFormat"></param>
        /// <returns></returns>
        public bool Init(Texture2D texture, TextureFormat textureFormat)
        {
            this.m_bPrepared = true;
            if (null == texture)
            {
                return false;
            }
            this.m_tex = texture;
            this.m_rect = new Rect(0f, 0f, (float)this.m_tex.width, (float)this.m_tex.height);
            return true;
        }
        /// <summary>
        /// 引用次数加一
        /// </summary>
        public void AddRef()
        {
            this.m_refCount++;
        }
        /// <summary>
        /// 引用次数减一
        /// </summary>
        public void DelRef()
        {
            this.m_refCount--;
            if (this.m_refCount < 0)
            {
                Debug.LogError(string.Format("Fatal Error m_refCount < 0: m_spriteName={0}", this.m_spriteName));
            }
        }
        public void Clear()
        {
            this.m_tex = null;
            this.m_bPrepared = false;
        }
        #endregion
    }
    #endregion
}
