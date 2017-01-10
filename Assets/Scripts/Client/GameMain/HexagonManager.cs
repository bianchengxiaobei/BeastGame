using UnityEngine;
using System.Collections.Generic;
using Utility;
using Game;
using UnityAssetEx.Export;
using Utility.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：HexagonManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.7
// 模块描述：格子生成管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 格子生成管理器
/// </summary>
public class HexagonManager : Singleton<HexagonManager>
{
	#region 字段
    private HexagonDrawCall m_mapHexagonDrawCall = null;//正常显示地图格子
    private HexagonDrawCall m_castRangeHexagonDrawCall = null;//技能范围
    private HexagonDrawCall m_highlightHexagonDrawCall = null;//高亮
    private HexagonDrawCall m_affectHexagonDrawCall = null;//特效
    private HexagonDrawCall m_selectHexagonDrawCall = null;//选择
    private HexagonDrawCall m_pathHexagonDrawCall = null;//路径
    private List<CVector3> m_listHexagonEnabled = new List<CVector3>();
    private GameObject m_objPlane = null;//地图格子的平面
    private GameObject m_objHover = null;//地图格子上方悬浮物
    private List<IAssetRequest> m_listAssetRequestCached = new List<IAssetRequest>();
	#endregion
	#region 属性
    /// <summary>
    /// 所有激活的地图格子List
    /// </summary>
    public List<CVector3> ListHexagonEnabled
    {
        get
        {
            return this.m_listHexagonEnabled;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 初始化，绘制所有格子mesh
    /// </summary>
    /// <param name="vec3InitPos"></param>
    /// <param name="vec3Rotation"></param>
    public void Init(Vector3 vec3InitPos, Vector3 vec3Rotation)
    {
        //生成地图格子父类，MapHexagon，添加MapBehavior脚本
        this.m_objPlane = new GameObject("MapHexagon");
        this.m_objPlane.layer = UnityEngine.LayerMask.NameToLayer("HexagonMap");
        if (this.m_objPlane != null)
        {
            UnityEngine.Object.DontDestroyOnLoad(this.m_objPlane);
            //添加Mono脚本
            this.m_objPlane.AddComponent<MapBehaviour>();
            this.m_objPlane.transform.position = vec3InitPos;
            this.m_objPlane.transform.eulerAngles = vec3Rotation;
        }
        Transform transform = this.m_objPlane.transform;
        Hexagon.TransformBase = transform;

        #region 加载普通格子地图
        UnityEngine.Object original = Resources.Load("Data/3D/Scene/Hexagon/MapHexagon");
        GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
        if (gameObject != null)
        {
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;           
            //添加脚本HexagonDrawCall，渲染出mesh
            this.m_mapHexagonDrawCall = gameObject.GetComponent<HexagonDrawCall>();
            if (null == this.m_mapHexagonDrawCall)
            {
                this.m_mapHexagonDrawCall = gameObject.AddComponent<HexagonDrawCall>();
            }
        }
        #endregion
        #region 加载可以释放技能范围格子地图
        original = Resources.Load("Data/3D/Scene/Hexagon/CastRangeHexagon");
        gameObject = (UnityEngine.Object.Instantiate(original) as GameObject);
        if (null != gameObject)
        {
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            this.m_castRangeHexagonDrawCall = gameObject.GetComponent<HexagonDrawCall>();
            if (null == this.m_castRangeHexagonDrawCall)
            {
                this.m_castRangeHexagonDrawCall = gameObject.AddComponent<HexagonDrawCall>();
            }
        }
        #endregion
        #region 加载高亮格子地图
        original = Resources.Load("Data/3D/Scene/Hexagon/HighlightHexagon");
        gameObject = (UnityEngine.Object.Instantiate(original) as GameObject);
        if (null != gameObject)
        {
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            this.m_highlightHexagonDrawCall = gameObject.GetComponent<HexagonDrawCall>();
            if (null == this.m_highlightHexagonDrawCall)
            {
                this.m_highlightHexagonDrawCall = gameObject.AddComponent<HexagonDrawCall>();
            }
        }
        #endregion
        #region 加载技能范围的格子
        original = Resources.Load("Data/3D/Scene/Hexagon/AffectHexagon");
        gameObject = (UnityEngine.Object.Instantiate(original) as GameObject);
        if (null != gameObject)
        {
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            this.m_affectHexagonDrawCall = gameObject.GetComponent<HexagonDrawCall>();
            if (null == this.m_affectHexagonDrawCall)
            {
                this.m_affectHexagonDrawCall = gameObject.AddComponent<HexagonDrawCall>();
            }
        }
        #endregion
        #region 选择中格子地图
        original = Resources.Load("Data/3D/Scene/Hexagon/SelectHexagon");
        gameObject = (UnityEngine.Object.Instantiate(original) as GameObject);
        if (null != gameObject)
        {
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            this.m_selectHexagonDrawCall = gameObject.GetComponent<HexagonDrawCall>();
            if (null == this.m_selectHexagonDrawCall)
            {
                this.m_selectHexagonDrawCall = gameObject.AddComponent<HexagonDrawCall>();
            }
        }
        #endregion
        #region 路径格子地图
        original = Resources.Load("Data/3D/Scene/Hexagon/PathHexagon");
        gameObject = (UnityEngine.Object.Instantiate(original) as GameObject);
        if (null != gameObject)
        {
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            this.m_pathHexagonDrawCall = gameObject.GetComponent<HexagonDrawCall>();
            if (null == this.m_pathHexagonDrawCall)
            {
                this.m_pathHexagonDrawCall = gameObject.AddComponent<HexagonDrawCall>();
            }
        }
        #endregion
        IAssetRequest item = ResourceManager.singleton.LoadTexture("Texture/Hexagon/Select",null,AssetPRI.DownloadPRI_Plain);
        this.m_listAssetRequestCached.Add(item);
    }
    /// <summary>
    /// 构建地图格子
    /// </summary>
    /// <returns></returns>
    public bool ConstructMap()
    {
        List<CVector3> list = new List<CVector3>();
        Dictionary<int, Dictionary<int, MapNode>> dicMapData = Singleton<ClientMain>.singleton.scene.DicMapData;
        foreach (var current in dicMapData)
        {
            foreach (var current2 in current.Value)
            {
                CVector3 item = new CVector3(current2.Value.nIndexX, current2.Value.nIndexY, current2.Value.nIndexU);
                list.Add(item);
            }
        }
        if (null != this.m_mapHexagonDrawCall)
        {
            this.m_mapHexagonDrawCall.SetHexagons(list,"");
        }
        return true;
    }
    /// <summary>
    /// 显示不同类型的格子（包括建筑）
    /// </summary>
    /// <param name="eShowHexagonType"></param>
    /// <param name="listHex"></param>
    public void ShowHexagon(EnumShowHexagonType eShowHexagonType, List<CVector3> listHex)
    {
        string strTexture = "";
        if (EnumShowHexagonType.eShowHexagonType_Highlight == eShowHexagonType)
        {
            strTexture = "Texture/Hexagon/Highlight";
        }
        this.ShowHexagon(eShowHexagonType, listHex, strTexture);
    }
    public void ShowHexagon(EnumShowHexagonType eShowHexagonType, List<CVector3> listHex, string strTexture)
    {
        switch (eShowHexagonType)
        {
            case EnumShowHexagonType.eShowHexagonType_CastRange:
                if (null != this.m_castRangeHexagonDrawCall)
                {
                    this.m_castRangeHexagonDrawCall.SetHexagons(listHex, strTexture);
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Highlight:
                if (null != this.m_highlightHexagonDrawCall)
                {
                    this.m_highlightHexagonDrawCall.SetHexagons(listHex, strTexture);
                }
                if (null != listHex)
                {
                    this.m_listHexagonEnabled = listHex;
                }
                else
                {
                    this.m_listHexagonEnabled.Clear();
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Affect:
                if (null != this.m_affectHexagonDrawCall)
                {
                    this.m_affectHexagonDrawCall.SetHexagons(listHex, strTexture);
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Selected:
                if (null != this.m_selectHexagonDrawCall)
                {
                    this.m_selectHexagonDrawCall.SetHexagons(listHex, strTexture);
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Hover:
                this.ShowHoverHexagons(listHex);
                break;
            case EnumShowHexagonType.eShowHexagonType_Path:
                if (null != this.m_pathHexagonDrawCall)
                {
                    this.m_pathHexagonDrawCall.SetHexagons(listHex, strTexture);
                }
                break;
        }
        CSceneMgr.singleton.ShowHexagon(eShowHexagonType, listHex);
    }
    /// <summary>
    /// 不显示该类型的格子（包括建筑）
    /// </summary>
    /// <param name="eShowHexagonType"></param>
    public void ClearHexagon(EnumShowHexagonType eShowHexagonType)
    {
        switch (eShowHexagonType)
        {
            case EnumShowHexagonType.eShowHexagonType_CastRange:
                if (null != this.m_castRangeHexagonDrawCall)
                {
                    this.m_castRangeHexagonDrawCall.ClearHexagons();
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Highlight:
                if (null != this.m_highlightHexagonDrawCall)
                {
                    this.m_highlightHexagonDrawCall.ClearHexagons();
                }
                this.m_listHexagonEnabled.Clear();
                break;
            case EnumShowHexagonType.eShowHexagonType_Affect:
                if (null != this.m_affectHexagonDrawCall)
                {
                    this.m_affectHexagonDrawCall.ClearHexagons();
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Selected:
                if (null != this.m_selectHexagonDrawCall)
                {
                    this.m_selectHexagonDrawCall.ClearHexagons();
                }
                break;
            case EnumShowHexagonType.eShowHexagonType_Path:
                if (null != this.m_pathHexagonDrawCall)
                {
                    this.m_pathHexagonDrawCall.ClearHexagons();
                }
                break;
        }
        CSceneMgr.singleton.ClearHexagon(eShowHexagonType);
    }
    /// <summary>
    /// 不显示所有的格子
    /// </summary>
    public void ClearAllHexagon()
    {
        EnumShowHexagonType showType = EnumShowHexagonType.eShowHexagonType_CastRange;
        for (; showType < EnumShowHexagonType.eShowHexagonType_Max; showType++)
        {
            this.ClearHexagon(showType);
        }
    }
    #endregion
	#region 私有方法
    /// <summary>
    /// 实例化选择出生点的箭头
    /// </summary>
    /// <param name="listHexPos"></param>
    private void ShowHoverHexagons(List<CVector3> listHexPos)
    {
        if (listHexPos.Count > 0)
        {
            Vector3 hex3DPos = Hexagon.GetHex3DPos(listHexPos[0], Space.World);
            if (null == this.m_objHover)
            {
                Object hoverObj = ResourceManager.singleton.Load("Data/3D/Scene/Hover");
                if (null == hoverObj)
                {
                    return;
                }
                this.m_objHover = (GameObject)Object.Instantiate(hoverObj);
                if (null == this.m_objHover)
                {
                    return;
                }
                Object.DontDestroyOnLoad(this.m_objHover);
            }
            this.m_objHover.transform.position = hex3DPos;
            this.m_objHover.SetActive(true);
        }
    }
	#endregion
}
