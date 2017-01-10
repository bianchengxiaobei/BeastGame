using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Game;
using Utility.Export;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapNodeBuilding
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class MapNodeBuilding 
{
	#region 字段
    private Vector3 m_localRotation = Vector3.zero;
    private Vector3 m_localPos = Vector3.zero;
    private CVector3 m_vec3HexPos = new CVector3();
    private MapNodeBehaviour m_mapNodeBehaviour = null;
    private GameObject m_gameObject = null;
    private string m_strModelFile = string.Empty;
    private string m_strOriginModelFile = string.Empty;
    private EMapNodeType m_MapNodeType = EMapNodeType.MAP_NODE_INVALID;
    private bool m_bDisposed = false;
    private bool m_bVisible = true;
    private bool m_bHighlight = false;
    private bool m_bAffect = false;
    private int m_nHightlightEffectId = 0;
    private int m_nEffectInstanceId = -1;
    private int m_nDodgeEffectId = 0;
    private int m_nOMGEffectInstanceId = 0;
    private IXLog m_log = XLog.GetLog<MapNodeBuilding>();
    private List<GameObject> m_ListObj = new List<GameObject>();
	#endregion
	#region 属性
    /// <summary>
    /// 格子类型
    /// </summary>
    public EMapNodeType MapNodeType
    {
        get
        {
            return this.m_MapNodeType;
        }
        set
        {
            this.m_MapNodeType = value;
        }
    }
    /// <summary>
    /// 是否开启碰撞器
    /// </summary>
    public bool IsCollider
    {
        set
        {
            if (null != this.m_mapNodeBehaviour)
            {
                this.m_mapNodeBehaviour.IsCollider = value;
            }
        }
    }
    /// <summary>
    /// 模型路径
    /// </summary>
    public string ModelFile
    {
        get
        {
            return this.m_strModelFile;
        }
        set
        {
            this.m_strModelFile = value;
        }
    }
    /// <summary>
    /// 原始模型路径
    /// </summary>
    public string OriginModelFile
    {
        set
        {
            this.m_strOriginModelFile = value;
        }
    }
    /// <summary>
    /// 所在的地图格子坐标
    /// </summary>
    public CVector3 HexPos
    {
        get
        {
            return this.m_vec3HexPos;
        }
        set
        {
            if (!this.m_vec3HexPos.Equals(value))
            {
                this.m_vec3HexPos.CopyFrom(value);
                this.UpdatePosition();
            }
        }
    }
    public Vector3 LocalPos
    {
        get
        {
            return this.m_localPos;
        }
        set
        {
            this.m_localPos = value;
            this.UpdatePosition();
        }
    }
    public Vector3 LocalRotation
    {
        get
        {
            return this.m_localRotation;
        }
        set
        {
            this.m_localRotation = value;
            this.UpdateRotation();
        }
    }
    public Vector3 RealPos
    {
        get
        {
            return Hexagon.GetHex3DPos(this.m_vec3HexPos, Space.World);
        }
    }
    public int EffectId
    {
        get;
        set;
    }
    public int TriggerEffectId
    {
        get;
        set;
    }
    public int OMGEffectId
    {
        get;
        set;
    }
    public bool IsHighlight
    {
        get
        {
            return this.m_bHighlight;
        }
        set
        {
            if (value != this.m_bHighlight)
            {
                this.m_bHighlight = value;
                if (null != this.m_mapNodeBehaviour)
                {
                    this.m_mapNodeBehaviour.IsHighlight = this.m_bHighlight;
                }
                if (this.m_bHighlight)
                {
                    if (0 == this.m_nHightlightEffectId)
                    {
                        
                    }
                }
                else
                {
                    if (0 != this.m_nHightlightEffectId)
                    {
                        //EffectManager.singleton.StopEffect(this.m_nHightlightEffectId);
                        this.m_nHightlightEffectId = 0;
                    }
                }
            }
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    public void Init(MapNodeBehaviour mapNodeBehaviour)
    {
        this.m_mapNodeBehaviour = mapNodeBehaviour;
        Vector3 hex3DPos = Hexagon.GetHex3DPos(this.m_vec3HexPos, Space.World);
        this.m_mapNodeBehaviour.transform.position = hex3DPos;
        this.m_mapNodeBehaviour.IsHighlight = this.m_bHighlight;
        this.m_mapNodeBehaviour.IsAffect = this.m_bAffect;
        this.UpdatePosition();
        this.UpdateRotation();
    }
    /// <summary>
    /// 更改地图格子类型，也要重新替换模型
    /// </summary>
    /// <param name="type"></param>
    public void ChangeNodeType(EMapNodeType type)
    {
        switch(type)
        {
            case EMapNodeType.MAP_NODE_ROCK:
                this.m_strModelFile = "Data/3D/Scene/EnergyTrap";
                this.ChangeModel();
                break;
        }
    }
    /// <summary>
    /// 更换模型
    /// </summary>
    public void ChangeModel()
    {
        if (!string.IsNullOrEmpty(this.m_strModelFile))
        {
            Object obj = ResourceManager.singleton.Load(this.m_strModelFile);
            this.LoadModelFinished(obj);
        }
    }
    /// <summary>
    /// 显示格子模型
    /// </summary>
    /// <param name="bVisible"></param>
    public void SetVisible(bool bVisible)
    {
        if (this.m_bVisible != bVisible)
        {
            this.m_bVisible = bVisible;
            if (null != this.m_gameObject)
            {
                this.m_gameObject.SetActive(this.m_bVisible);
            }
        }
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 更新世界坐标
    /// </summary>
    private void UpdatePosition()
    {
        if (null != this.m_mapNodeBehaviour)
        {
            Vector3 vector = Hexagon.GetHex3DPos(this.m_vec3HexPos, Space.World);
            vector += this.m_localPos;
            this.m_mapNodeBehaviour.CachedTransform.position = vector;
        }
    }
    /// <summary>
    /// 更新旋转
    /// </summary>
    private void UpdateRotation()
    {
        if (null != this.m_mapNodeBehaviour)
        {
            this.m_mapNodeBehaviour.CachedTransform.eulerAngles = this.m_localRotation;
        }
    }
    private void LoadModelFinished(Object obj)
    {
        if (null == obj)
        {
            this.m_log.Error("null == obj");
        }
        else 
        {
            //如果先前的模型还存在游戏中的话，就先摧毁
            if (this.m_gameObject != null)
            {
                Object.Destroy(this.m_gameObject);
                this.m_gameObject = null;
            }
            this.m_gameObject = (GameObject)Object.Instantiate(obj);
            if (null == this.m_gameObject)
            {
                this.m_log.Error("m_gameObject == null");
            }
            else 
            {
                Object.DontDestroyOnLoad(this.m_gameObject);
                this.UpdatePosition();
                this.UpdateRotation();
                MapNodeBehaviour mapNodeBehaviour = this.m_gameObject.AddComponent<MapNodeBehaviour>();
                mapNodeBehaviour.MapNodeBuilding = this;
                this.m_gameObject.SetActive(this.m_bVisible);
                if (EMapNodeType.MAP_NODE_EMPIRE_BASE == this.MapNodeType || EMapNodeType.MAP_NODE_LEAGUE_BASE == this.MapNodeType)
                {
                    mapNodeBehaviour.SetHighLightData();
                }
            }
        }
    }
    /// <summary>
    /// 开始特效
    /// </summary>
    /// <param name="unBeastId"></param>
    public void OnStartBeastRound(long unBeastId)
    {
        if (this.EffectId > 0)
        {
            /*if (!CSceneMgr.singleton.CurScene.IsPosStandByAnyRole(this.m_vec3HexPos) && this.m_nEffectInstanceId == -1)
            {
                this.m_log.Debug("MapNodeBuilding: PlayEffect:" + this.EffectId);
                this.m_nEffectInstanceId = EffectManager.singleton.PlayEffect(this.EffectId, this.RealPos, this.RealPos);
                EffectManager.singleton.PlayEffect(106, this.RealPos, this.RealPos);
            }*/
        }
        if (this.OMGEffectId > 0 && this.m_nOMGEffectInstanceId <= 0)
        {
            /*this.m_log.Debug("MapNodeBuilding: PlayOMGEffect:" + this.OMGEffectId);
            this.m_nOMGEffectInstanceId = EffectManager.singleton.PlayEffect(this.OMGEffectId, null, this.RealPos, null, null, this.RealPos, null, Vector3.zero);
             * */
        }
    }
	#endregion
}
