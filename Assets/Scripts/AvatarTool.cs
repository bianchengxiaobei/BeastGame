using UnityEngine;
using System.Collections.Generic;
using UnityAssetEx.Export;
using Utility.Export;
using System;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：AvatarTool
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：换装工具
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 换装工具
/// </summary>
public class AvatarTool 
{
	#region 字段
    private bool m_bFinished = false;
    private int m_nMergeType = 0;
    private int m_unBeastTypeId = -1;
    private string m_strBaseModelPath = string.Empty;
    private int m_nSuitId = -1;
    private IAssetRequest m_assetRequestSuitModel = null;
    private IAssetRequest m_assetRequestBaseModel = null;
    private List<IAssetRequest> m_listAssetRequestSuitTexture = new List<IAssetRequest>();
    private GameObject m_gameObject = null;
    private IXLog m_log = XLog.GetLog<AvatarTool>();
    private Action<GameObject> m_callBackFinish = null;
	#endregion
	#region 属性
    /// <summary>
    /// 神兽模型GameObject缓存
    /// </summary>
    public GameObject GameObjectCached
    {
        get
        {
            return this.m_gameObject;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 卸载该模型，包括皮肤
    /// </summary>
    public void Unload()
    {
        if (null != this.m_gameObject)
        {
            SkinnedMeshRenderer component = this.m_gameObject.GetComponent<SkinnedMeshRenderer>();
            if (null != component)
            {
                //先卸载贴图
                if (null != component.material)
                {
                    if (null != component.material.mainTexture)
                    {
                        UnityEngine.Object.Destroy(component.material.mainTexture);
                    }
                    //再卸载材质
                    UnityEngine.Object.Destroy(component.material);
                }
                //再卸载多边形
                if (null != component.sharedMesh)
                {
                    UnityEngine.Object.Destroy(component.sharedMesh);
                }
            }
            //最后卸载GameObject
            UnityEngine.Object.Destroy(this.m_gameObject);
            this.m_gameObject = null;
        }
        //卸载之前下载的资源缓存
        if (null != this.m_assetRequestBaseModel)
        {
            this.m_assetRequestBaseModel.RemoveQuickly = true;
            this.m_assetRequestBaseModel.Dispose();
            this.m_assetRequestBaseModel = null;
        }
        if (null != this.m_assetRequestSuitModel)
        {
            this.m_assetRequestSuitModel.RemoveQuickly = true;
            this.m_assetRequestSuitModel.Dispose();
            this.m_assetRequestSuitModel = null;
        }
        //卸载所有该神兽拥有的皮肤贴图
        for (int i = 0; i < this.m_listAssetRequestSuitTexture.Count; i++)
        {
            IAssetRequest assetRequest = this.m_listAssetRequestSuitTexture[i];
            if (null != assetRequest)
            {
                assetRequest.RemoveQuickly = true;
                assetRequest.Dispose();
            }
        }
        this.m_listAssetRequestSuitTexture.Clear();
        this.m_unBeastTypeId = 0;
        this.m_strBaseModelPath = string.Empty;
        this.m_nSuitId = 0;
        this.m_callBackFinish = null;
        this.m_bFinished = false;
    }
    /// <summary>
    /// 创建模型，根据模型的皮肤
    /// </summary>
    /// <param name="unBeastTypeId">神兽模型id</param>
    /// <param name="nSuitId">皮肤id</param>
    /// <param name="callBack">回调</param>
    public void Dowork(int unBeastTypeId, int nSuitId, Action<GameObject> callBack)
    {
        if (this.m_unBeastTypeId == unBeastTypeId && this.m_nSuitId == nSuitId)
        {
            if (null != callBack)
            {
                if (this.m_bFinished)
                {
                    callBack(this.m_gameObject);
                }
                else
                {
                    this.m_callBackFinish = (Action<GameObject>)Delegate.Combine(this.m_callBackFinish, callBack);
                }
            }
        }
        else 
        {
            //在创建模型前先卸载之前的缓存模型
            this.Unload();
            this.m_unBeastTypeId = unBeastTypeId;
            this.m_nSuitId = nSuitId;
            this.m_callBackFinish = callBack;
            //DataSuit dataByID = DataSuitManager.Instance.GetDataByID(nSuitId);
            DataSuit dataByID = GameData<DataSuit>.dataMap[nSuitId];
            if (null != dataByID)
            {
                //合并类型
                this.m_nMergeType = dataByID.Merge;
                List<string> arrayList = DataExtensions.GetArrayList(dataByID.Texture);
                if (arrayList.Count != 0)
                {
                    //加载模型皮肤需要的贴图
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        string relativeUrl = string.Format("Texture/SuitTexture/{0}", arrayList[i]);
                        this.m_listAssetRequestSuitTexture.Add(ResourceManager.singleton.LoadTexture(relativeUrl, new AssetRequestFinishedEventHandler(this.OnLoadSuitTextureFinished), AssetPRI.DownloadPRI_Plain));
                    }
                }
                //加载皮肤模型
                if (!string.IsNullOrEmpty(dataByID.SuitPath))
                {
                    string relativeUrl2 = string.Format("Data/Model/BeastModel/{0}", dataByID.SuitPath);
                    this.m_assetRequestSuitModel = ResourceManager.singleton.LoadModel(relativeUrl2, new AssetRequestFinishedEventHandler(this.OnLoadSuitModelFinished), AssetPRI.DownloadPRI_Plain);
                }
            }
            //获取基础模型路径
            this.m_strBaseModelPath = AvatarTool.GetBasePath(nSuitId, unBeastTypeId);
            //加载基础模型
            this.m_assetRequestBaseModel = ResourceManager.singleton.LoadModel(this.m_strBaseModelPath, new AssetRequestFinishedEventHandler(this.OnLoadBaseModelFinished), AssetPRI.DownloadPRI_Plain);
        }
    }
    /// <summary>
    /// 根据神兽id和皮肤id，获取源模型路径,如果该模型没有皮肤，就返回原始模型
    /// </summary>
    /// <param name="nSuitId"></param>
    /// <param name="nBeastTypeId"></param>
    /// <returns></returns>
    public static string GetBasePath(int nSuitId, int nBeastTypeId)
    {
        string result = string.Empty;
        //DataSuit dataByID = DataSuitManager.Instance.GetDataByID(nSuitId);
        DataSuit dataByID = GameData<DataSuit>.dataMap[nSuitId];
        if (dataByID != null && !string.IsNullOrEmpty(dataByID.BasePath))
        {
            result = string.Format("Data/Model/BeastModel/{0}", dataByID.BasePath);
        }
        else
        {
           // DataBeastlist dataById = DataBeastlistManager.Instance.GetDataById(nBeastTypeId);
            DataBeastlist dataById = GameData<DataBeastlist>.dataMap[nBeastTypeId];
            if (null != dataById)
            {
                result = string.Format("Data/Model/BeastModel/{0}/{1}", dataById.ModelFile, dataById.ModelFile);
            }
        }
        return result;
    }
    /// <summary>
    /// 合并模型
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static Transform CombineModel(Transform root)
    {
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        List<CombineInstance> list = new List<CombineInstance>();
        List<Material> list2 = new List<Material>();
        Material material = null;
        List<Transform> list3 = new List<Transform>();
        Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>();
        List<Texture2D> list4 = new List<Texture2D>();
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        List<Vector2[]> list5 = new List<Vector2[]>();
        SkinnedMeshRenderer[] componentsInChildren2 = root.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < componentsInChildren2.Length; i++)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren2[i];
            if (material == null)
            {
                material = (UnityEngine.Object.Instantiate(skinnedMeshRenderer.sharedMaterial) as Material);
            }
            for (int j = 0; j < skinnedMeshRenderer.sharedMesh.subMeshCount; j++)
            {
                list.Add(new CombineInstance
                {
                    mesh = skinnedMeshRenderer.sharedMesh,
                    subMeshIndex = j
                });
            }
            list5.Add(skinnedMeshRenderer.sharedMesh.uv);
            num3 += skinnedMeshRenderer.sharedMesh.uv.Length;
            if (skinnedMeshRenderer.material.mainTexture != null)
            {
                list4.Add(skinnedMeshRenderer.renderer.material.mainTexture as Texture2D);
                num += skinnedMeshRenderer.renderer.material.mainTexture.width;
                num2 += skinnedMeshRenderer.renderer.material.mainTexture.height;
            }
            Transform[] bones = skinnedMeshRenderer.bones;
            for (int k = 0; k < bones.Length; k++)
            {
                Transform transform = bones[k];
                Transform[] array = componentsInChildren;
                for (int l = 0; l < array.Length; l++)
                {
                    Transform transform2 = array[l];
                    if (!(transform2.name != transform.name))
                    {
                        list3.Add(transform2);
                        break;
                    }
                }
            }
            UnityEngine.Object.Destroy(skinnedMeshRenderer.gameObject);
        }
        SkinnedMeshRenderer skinnedMeshRenderer2 = root.gameObject.GetComponent<SkinnedMeshRenderer>();
        if (!skinnedMeshRenderer2)
        {
            skinnedMeshRenderer2 = root.gameObject.AddComponent<SkinnedMeshRenderer>();
        }
        skinnedMeshRenderer2.sharedMesh = new Mesh();
        skinnedMeshRenderer2.sharedMesh.CombineMeshes(list.ToArray(), true, false);
        skinnedMeshRenderer2.bones = list3.ToArray();
        skinnedMeshRenderer2.material = material;
        Texture2D texture2D;
        if (Application.isMobilePlatform)
        {
            texture2D = new Texture2D(1024, 1024, TextureFormat.PVRTC_RGBA4, false);
        }
        else
        {
            texture2D = new Texture2D(1024, 512, TextureFormat.DXT5, false);
        }
        Rect[] array2 = texture2D.PackTextures(list4.ToArray(), 0);
        Vector2[] array3 = new Vector2[num3];
        int num4 = 0;
        for (int m = 0; m < list5.Count; m++)
        {
            Vector2[] array4 = list5[m];
            for (int i = 0; i < array4.Length; i++)
            {
                Vector2 vector = array4[i];
                array3[num4].x = Mathf.Lerp(array2[m].xMin, array2[m].xMax, vector.x);
                array3[num4].y = Mathf.Lerp(array2[m].yMin, array2[m].yMax, vector.y);
                num4++;
            }
        }
        skinnedMeshRenderer2.material.mainTexture = texture2D;
        skinnedMeshRenderer2.sharedMesh.uv = array3;
        XLog.GetLog<AvatarTool>().Debug(string.Concat(new object[]
	{
		root.name,
		"CombineModel takes : ",
		(Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f,
		" ms"
	}));
        return root;
    }
	#endregion
	#region 私有方法
    private void OnLoadSuitTextureFinished(IAssetRequest assetRequest)
    {
        this.DetectMerge();
    }
    private void OnLoadSuitModelFinished(IAssetRequest assetRequest)
    {
        this.DetectMerge();
    }
    /// <summary>
    /// 加载基础模型完成之后回调
    /// </summary>
    /// <param name="assetRequest"></param>
    private void OnLoadBaseModelFinished(IAssetRequest assetRequest)
    {
        this.DetectMerge();
    }
    /// <summary>
    /// 创建模型（合并操作等）
    /// </summary>
    private void DetectMerge()
    {
        if (this.m_assetRequestBaseModel != null)
        {
            if (this.m_assetRequestBaseModel == null || this.m_assetRequestBaseModel.IsFinished)
            {
                if (this.m_assetRequestSuitModel == null || this.m_assetRequestSuitModel.IsFinished)
                {
                    for (int i = 0; i < this.m_listAssetRequestSuitTexture.Count; i++)
                    {
                        if (this.m_listAssetRequestSuitTexture[i] != null && !this.m_listAssetRequestSuitTexture[i].IsFinished)
                        {
                            return;
                        }
                    }
                    if (this.m_assetRequestBaseModel.AssetResource.MainAsset == null)
                    {
                        this.m_log.Error(string.Format("m_assetRequestBaseModel.AssetResource.MainAsset == null: m_strBaseModelPath={0}, m_unHeroTypeId={1}, m_nSuitId={2}", this.m_strBaseModelPath, this.m_unBeastTypeId, this.m_nSuitId));
                    }
                    else
                    {
                        try
                        {
                            GameObject gameObject = null;
                            if (this.m_assetRequestBaseModel != null && null != this.m_assetRequestBaseModel.AssetResource)
                            {
                                //实例化基础模型
                                gameObject = (UnityEngine.Object.Instantiate(this.m_assetRequestBaseModel.AssetResource.MainAsset) as GameObject);
                                List<Texture> list = new List<Texture>();
                                for (int i = 0; i < this.m_listAssetRequestSuitTexture.Count; i++)
                                {
                                    list.Add(this.m_listAssetRequestSuitTexture[i].AssetResource.MainAsset as Texture);
                                }
                                //将皮肤贴图上到模型上
                                AvatarTool.ChangeTex(gameObject, list);
                            }
                            GameObject goSuit = null;
                            if (this.m_assetRequestSuitModel != null && null != this.m_assetRequestSuitModel.AssetResource)
                            {
                                //实例化皮肤模型
                                goSuit = (UnityEngine.Object.Instantiate(this.m_assetRequestSuitModel.AssetResource.MainAsset) as GameObject);
                            }
                            this.m_gameObject = AvatarTool.MakeSureRoot(gameObject, goSuit, this.m_nMergeType);
                            if (null == this.m_gameObject)
                            {
                                this.m_log.Error(string.Format("null == m_gameObject: heroTypeId={0}, suitId={1}", this.m_unBeastTypeId, this.m_nSuitId));
                            }
                            //合并模型
                            AvatarTool.CombineModel(this.m_gameObject.transform);
                            //清除缓存
                            for (int i = 0; i < this.m_listAssetRequestSuitTexture.Count; i++)
                            {
                                IAssetRequest assetRequest = this.m_listAssetRequestSuitTexture[i];
                                if (null != assetRequest)
                                {
                                    assetRequest.RemoveQuickly = true;
                                    assetRequest.Dispose();
                                }
                            }
                            this.m_listAssetRequestSuitTexture.Clear();
                        }
                        catch (Exception ex)
                        {
                            this.m_log.Debug(string.Format("AvatarTool:m_strBaseModelPath={0}, m_unHeroTypeId={1}, m_nSuitId={2}", this.m_strBaseModelPath, this.m_unBeastTypeId, this.m_nSuitId));
                            this.m_log.Fatal(ex.ToString());
                        }
                        this.m_bFinished = true;
                        //执行创建模型完成后回调
                        if (null != this.m_callBackFinish)
                        {
                            this.m_callBackFinish(this.m_gameObject);
                            this.m_callBackFinish = null;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 所有贴图加到模型上
    /// </summary>
    /// <param name="goModel"></param>
    /// <param name="listTexture"></param>
    private static void ChangeTex(GameObject goModel, List<Texture> listTexture)
    {
        if (listTexture.Count > 0 && !(null == goModel))
        {
            SkinnedMeshRenderer[] componentsInChildren = goModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (i < listTexture.Count && null != listTexture[i])
                {
                    componentsInChildren[i].material.mainTexture = listTexture[i];
                }
            }
        }
    }
    /// <summary>
    /// 设置根节点
    /// 如果合并类型是0的话，皮肤模型是基础模型的子物体
    /// 如果合并类型是其他的话，基础模型是皮肤模型的子物体
    /// </summary>
    /// <param name="goModle"></param>
    /// <param name="goSuit"></param>
    /// <param name="merge"></param>
    /// <returns></returns>
    private static GameObject MakeSureRoot(GameObject goModle, GameObject goSuit, int merge)
    {
        GameObject result;
        if (null == goModle)
        {
            result = null;
        }
        else
        {
            if (goSuit == null)
            {
                result = goModle;
            }
            else
            {
                if (merge == 0)
                {
                    goSuit.transform.SetParent(goModle.transform, false);
                    if (goSuit.GetComponent<CharacterController>() != null)
                    {
                        //卸载皮肤模型上的角色控制器（重复了）
                        UnityEngine.Object.Destroy(goSuit.GetComponent<CharacterController>());
                    }
                    result = goModle;
                }
                else
                {
                    goModle.transform.SetParent(goSuit.transform, false);
                    if (goModle.GetComponent<CharacterController>() != null)
                    {
                        UnityEngine.Object.Destroy(goModle.GetComponent<CharacterController>());
                    }
                    result = goSuit;
                }
            }
        }
        return result;
    }
	#endregion
}
