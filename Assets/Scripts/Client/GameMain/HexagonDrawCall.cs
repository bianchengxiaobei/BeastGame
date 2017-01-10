using UnityEngine;
using System.Collections.Generic;
using Game;
using UnityAssetEx.Export;
using Utility.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：HexagonDrawCall
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.7
// 模块描述：绘制格子mesh类
//----------------------------------------------------------------*/
#endregion
public class HexagonDrawCall : MonoBehaviour
{
	#region 字段
    private MeshFilter m_MeshFilter;
    private MeshCollider m_MeshCollider;
    private List<Vector3> m_listVertex = new List<Vector3>();
    private List<Vector2> m_listUV = new List<Vector2>();
    private List<int> m_listVertexIndex = new List<int>();
    private float m_fY = 0f;
    private bool m_bPrepared = false;
    private List<CVector3> m_listHexagonCached = new List<CVector3>();
    private string m_strTextureFileCached = "";
    private Dictionary<string, IAssetRequest> m_dicTextureAssetRequest = new Dictionary<string, IAssetRequest>();
    private IXLog m_log = XLog.GetLog<HexagonDrawCall>();
	#endregion
	#region 属性
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 加载完资源之后回调，主要是添加格子的贴图
    /// </summary>
    /// <param name="assetRequest"></param>
    public void LoadFinishedEventHandler(IAssetRequest assetRequest)
    {
        IAssetResource assetResource = assetRequest.AssetResource;
        if (assetResource != null)
        {
            UnityEngine.Object mainAsset = assetResource.MainAsset;
            Texture texture = mainAsset as Texture;
            if (texture != null)
            {
                base.renderer.material.mainTexture = texture;
            }
        }
    }
    public void SetHexagons(List<CVector3> listHexagon, string strTextureFile=null)
    {
        if (!this.m_bPrepared)
        {
            this.m_listHexagonCached = listHexagon;
            this.m_strTextureFileCached = strTextureFile;
        }
        else 
        {
            this.m_listVertex.Clear();
            this.m_listVertexIndex.Clear();
            this.m_listUV.Clear();
            if (listHexagon != null && this.m_MeshFilter != null)
            {
                this.m_MeshFilter.mesh.Clear();
                if (!string.IsNullOrEmpty(strTextureFile))
                {
                    //如果已经完成下载的列表不包括贴图，就下载然后存到缓存
                    if (!this.m_dicTextureAssetRequest.ContainsKey(strTextureFile) || !this.m_dicTextureAssetRequest[strTextureFile].IsFinished)
                    {
                        IAssetRequest value = ResourceManager.singleton.LoadTexture(strTextureFile, new AssetRequestFinishedEventHandler(this.LoadFinishedEventHandler), AssetPRI.DownloadPRI_Plain);
                        this.m_dicTextureAssetRequest[strTextureFile] = value;
                    }
                    else 
                    {
                        this.LoadFinishedEventHandler(this.m_dicTextureAssetRequest[strTextureFile]);
                    }
                }
                foreach (CVector3 current in listHexagon)
                {
                    Hexagon.FillHexagon(current.nRow, current.nCol, this.m_fY, ref this.m_listVertex, ref this.m_listVertexIndex, ref this.m_listUV);
                }
                //重新绘画计算mesh
                this.m_MeshFilter.mesh.vertices = this.m_listVertex.ToArray();
                this.m_MeshFilter.mesh.uv = this.m_listUV.ToArray();
                this.m_MeshFilter.mesh.triangles = this.m_listVertexIndex.ToArray();
                this.m_MeshFilter.mesh.RecalculateNormals();
                if (this.m_MeshCollider != null)
                {
                    this.m_MeshCollider.sharedMesh = null;
                    this.m_MeshCollider.sharedMesh = this.m_MeshFilter.mesh;//重新修改meshcollider
                }
            }
        }
    }
    /// <summary>
    /// 清空格子的mesh
    /// </summary>
    public void ClearHexagons()
    {
        this.m_MeshFilter.mesh.Clear();
    }
	#endregion
	#region 私有方法
    private void Awake()
    {
        this.m_MeshFilter = base.GetComponent<MeshFilter>();
        this.m_MeshCollider = base.GetComponent<MeshCollider>();
        if (this.m_MeshFilter != null)
        {
            if (this.m_MeshCollider != null)
            {
                this.m_MeshCollider.sharedMesh = null;
                this.m_MeshCollider.sharedMesh = this.m_MeshFilter.mesh;
            }
        }
        else 
        {
            this.m_log.Error("this.m_MeshFilter == null");
        }
    }
    private void Start()
    {
        this.m_bPrepared = true;
        this.SetHexagons(this.m_listHexagonCached, this.m_strTextureFileCached);
    }
	#endregion
}
