using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DynamicAtlasManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.3
// 模块描述：动态图集管理器
//----------------------------------------------------------------*/
#endregion
public class DynamicAtlasManager : MonoBehaviour
{
	#region 字段
    public delegate void PrepareAtlasCallBack(string strAtlas, string strSprite, UIAtlas atlas);
    public static DynamicAtlasManager Instance;
    private Dictionary<string, DynamicAtlas> m_dicUIAtlas = new Dictionary<string, DynamicAtlas>();
    private Transform m_transformCached;
    #endregion
	#region 属性
    /// <summary>
    /// 动态图集管理器的Transform
    /// </summary>
    public Transform CachedTransform
    {
        get
        {
            return this.m_transformCached;
        }
    }
	#endregion
	#region 构造方法
    static DynamicAtlasManager()
    {
        GameObject gameObject = new GameObject("DynamicAtlasParent");
        DontDestroyOnLoad(gameObject);
        DynamicAtlasManager.Instance = gameObject.AddComponent<DynamicAtlasManager>();
    }
	#endregion
	#region 公有方法
    /// <summary>
    /// 增加该精灵引用次数
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="textureName"></param>
    /// <param name="callBack"></param>
    public void AddRefCount(string atlasName, string textureName, DynamicAtlasManager.PrepareAtlasCallBack callBack)
    {
        if (string.IsNullOrEmpty(atlasName) || string.IsNullOrEmpty(textureName))
        {
            return;
        }
        //如果不存在该图集，就动态创建一个图集
        if (!this.m_dicUIAtlas.ContainsKey(atlasName))
        {
            this.m_dicUIAtlas[atlasName] = new DynamicAtlas(atlasName);
        }
        this.m_dicUIAtlas[atlasName].AddRefCount(textureName, callBack);
    }
    /// <summary>
    /// 移除该精灵引用的次数，如果该图集中精灵数量超过30，则释放该精灵资源
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="textureName"></param>
    /// <param name="callBack"></param>
    public void RemoveRefCount(string atlasName, string textureName, DynamicAtlasManager.PrepareAtlasCallBack callBack)
    {
        if (this.ContainsTexture(atlasName, textureName))
        {
            this.m_dicUIAtlas[atlasName].RemoveRefCount(textureName, callBack);
        }
    }
    /// <summary>
    /// 是否在该图集中包含该精灵
    /// </summary>
    /// <param name="atlasName">图集</param>
    /// <param name="textureName">精灵</param>
    /// <returns></returns>
    public bool ContainsTexture(string atlasName, string textureName)
    {
        return this.Contains(atlasName) && this.m_dicUIAtlas[atlasName].Contains(textureName);
    }
    public bool Contains(string atlasName)
    {
        return this.m_dicUIAtlas.ContainsKey(atlasName);
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 初始化transform
    /// </summary>
    private void Awake()
    {
        this.m_transformCached = base.transform;
    }
	#endregion
}
