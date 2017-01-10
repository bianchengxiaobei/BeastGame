using UnityEngine;
using System;
using System.Xml;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityAssetEx.Export;
using Utility.Export;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Scene
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：整张地图格子信息类
//----------------------------------------------------------------*/
#endregion
public class CMapData 
{
    private uint m_MapID;
    public CVector3 empireMapNode;
    public CVector3 leagueMapNode;
    private Dictionary<int, Dictionary<int, MapNode>> m_dicMapData;
    private Dictionary<EMapNodeType, MapNodeTypeInfo> m_dicMapNodeTypeInfo;
    private MapCfg m_mapCfg;
    private IXLog logger = new Logger("Scene");
    public Vector3 InitPos
    {
        get;
        set;
    }
    public Vector3 InitRotation
    {
        get;
        set;
    }
    public PVESetting PVESetting
    {
        get;
        set;
    }
    /// <summary>
    /// 不同类型格子的信息缓存字典
    /// </summary>
    public Dictionary<EMapNodeType, MapNodeTypeInfo> DicMapNodeTypeInfo
    {
        get
        {
            return this.m_dicMapNodeTypeInfo;
        }
    }
    /// <summary>
    /// 整张地图格子坐标缓存
    /// </summary>
    public Dictionary<int, Dictionary<int, MapNode>> DicMapData
    {
        get
        {
            return this.m_dicMapData;
        }
    }
    public CMapData()
    {
        this.m_MapID = 0u;
        this.m_dicMapData = new Dictionary<int, Dictionary<int, MapNode>>();
        this.m_dicMapNodeTypeInfo = new Dictionary<EMapNodeType, MapNodeTypeInfo>();
        this.m_mapCfg = new MapCfg();
        this.empireMapNode = new CVector3();
        this.leagueMapNode = new CVector3();
    }
    ~CMapData()
    {
        this.m_dicMapData.Clear();
        this.m_dicMapNodeTypeInfo.Clear();
    }
    /// <summary>
    /// 初始化地图信息，根据地图配置文件
    /// </summary>
    /// <param name="mapID">地图id</param>
    /// <param name="strConfigFile">配置文件xml</param>
    /// <returns></returns>
    public bool Init(uint mapID, string strConfigFile)
    {
        this.m_MapID = mapID;
        if (!string.IsNullOrEmpty(strConfigFile))
        {
            string strRelativePath = string.Format("config/{0}", strConfigFile);
            XmlDocument xmlDocument = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath, false));
            this.OnLoadFinishEventHandler(xmlDocument);
        }
        else
        {
            Debug.LogError("null == strMapFile:" + mapID);
        }
        return true;
    }
    /// <summary>
    /// 是否存在这个MapNode
    /// </summary>
    /// <param name="x">格子坐标x</param>
    /// <param name="y">格子坐标y</param>
    /// <returns></returns>
    public bool HasMapNode(int x,int y)
    {
        return this.m_dicMapData.ContainsKey(x) && this.m_dicMapData[x].ContainsKey(y);
    }
    public bool ChangeMapNodeType(CVector3 oPos, string strMapNodeType)
    {
        MapNode mapNode = this.GetMapNode(oPos.m_nX, oPos.m_nY);
        if (null == mapNode)
        {
            return false;
        }
        mapNode.m_strType = strMapNodeType;
        return MapCfg.MapNodeTransfer(mapNode);
    }
    public bool IsNodesInLine(CVector3 a, CVector3 b)
    {
        return a.m_nX == b.m_nX || a.m_nY == b.m_nY || a.m_nU == b.m_nY;
    }
    public MapNode GetMapNode(int x, int y)
    {
        if (!this.HasMapNode(x, y))
        {
            return null;
        }
        return this.m_dicMapData[x][y];
    }
    public bool IsRay(CVector3 oSrc, CVector3 oDest1, CVector3 oDest2)
    {
        if (oSrc.m_nX == oDest1.m_nX && oSrc.m_nX == oDest2.m_nX)
        {
            if (oDest1.m_nY > oSrc.m_nY && oDest2.m_nY > oSrc.m_nY)
            {
                return true;
            }
            if (oDest1.m_nY < oSrc.m_nY && oDest2.m_nY < oSrc.m_nY)
            {
                return true;
            }
        }
        if (oSrc.m_nY == oDest1.m_nY && oSrc.m_nY == oDest2.m_nY)
        {
            if (oDest1.m_nX > oSrc.m_nX && oDest2.m_nX > oSrc.m_nX)
            {
                return true;
            }
            if (oDest1.m_nX < oSrc.m_nX && oDest2.m_nX < oSrc.m_nX)
            {
                return true;
            }
        }
        if (oSrc.m_nU == oDest1.m_nU && oSrc.m_nU == oDest2.m_nU)
        {
            if (oDest1.m_nX > oSrc.m_nX && oDest2.m_nX > oSrc.m_nX)
            {
                return true;
            }
            if (oDest1.m_nX < oSrc.m_nX && oDest2.m_nX < oSrc.m_nX)
            {
                return true;
            }
        }
        return false;
    }
    public List<MapNode> GetNodesByType(EMapNodeType eMapNodeType)
    {
        List<MapNode> list = new List<MapNode>();
        foreach (var current in this.m_dicMapData)
        {
            foreach (var current2 in current.Value)
            {
                MapNode value = current2.Value;
                if (value.m_eMapNodeType == eMapNodeType)
                {
                    list.Add(value);
                }
            }
        }
        return list;
    }
    private void OnLoadFinishEventHandler(XmlDocument xmlDoc)
    {

        try
        {
            XmlNode xmlNode = xmlDoc.SelectSingleNode("table");
            XmlNode xmlNode2 = xmlNode.SelectSingleNode("map");
            XmlNodeList childNodes = xmlNode2.ChildNodes;
            foreach (XmlNode xmlNode3 in childNodes)
            {
                XmlElement xmlElement = (XmlElement)xmlNode3;
                MapNode mapNode = new MapNode();
                //初始化格子坐标
                mapNode.nIndexX = Convert.ToInt32(xmlElement.GetAttribute("X"));
                mapNode.nIndexY = Convert.ToInt32(xmlElement.GetAttribute("Y"));
                mapNode.nIndexU = Convert.ToInt32(xmlElement.GetAttribute("U"));
                mapNode.m_strType = xmlElement.GetAttribute("type");
                //初始化格子类型
                if (!MapCfg.MapNodeTransfer(mapNode))
                {
                    Debug.LogError("m_oCfg.MapNodeTransfer(oNode) == false");
                    return;
                }
                if (this.m_dicMapData.ContainsKey(mapNode.nIndexX))
                {
                    //如果xy都已经存在缓存格子中，说明肯定出错了，重复初始化话，就报错
                    if (this.m_dicMapData[mapNode.nIndexX].ContainsKey(mapNode.nIndexY))
                    {
                        string message = string.Format("XY conflict x={0},y={1}", mapNode.nIndexX, mapNode.nIndexY);
                        this.logger.Error("m_oDicMapData[oNode.nIndexX].ContainsKey(oNode.nIndexY) == true,");
                        this.logger.Error(message);
                        return;
                    }
                    //如果没有重复，就加到缓存格子中
                    this.m_dicMapData[mapNode.nIndexX].Add(mapNode.nIndexY, mapNode);
                }
                else
                {
                    Dictionary<int, MapNode> dictionary = new Dictionary<int, MapNode>();
                    dictionary.Add(mapNode.nIndexY, mapNode);
                    this.m_dicMapData.Add(mapNode.nIndexX, dictionary);
                }
                //如果节点的类型是基地类型，初始化基地节点变量
                if (mapNode.m_eMapNodeType == EMapNodeType.MAP_NODE_EMPIRE_BASE)
                {
                    this.empireMapNode.m_nX = mapNode.nIndexX;
                    this.empireMapNode.m_nY = mapNode.nIndexY;
                    this.empireMapNode.m_nU = mapNode.nIndexU;
                }
                else
                {
                    if (mapNode.m_eMapNodeType == EMapNodeType.MAP_NODE_LEAGUE_BASE)
                    {
                        this.leagueMapNode.m_nX = mapNode.nIndexX;
                        this.leagueMapNode.m_nY = mapNode.nIndexY;
                        this.leagueMapNode.m_nU = mapNode.nIndexU;
                    }
                }
            }
            XmlNode xmlNode4 = xmlNode.SelectSingleNode("mapInfo");
            if (xmlNode4 != null)
            {
                XmlElement xmlElement2 = xmlNode4 as XmlElement;
                string attribute = xmlElement2.GetAttribute("pos");
                Vector3 vector = UnityTools.String2Vector3(attribute);
                string attribute2 = xmlElement2.GetAttribute("rotation");
                Vector3 vector2 = UnityTools.String2Vector3(attribute2);
                //初始化地图坐标和旋转方向
                this.InitPos = vector;
                this.InitRotation = vector2;
                XmlNodeList xmlNodeList = xmlNode4.SelectNodes("nodeType");
                foreach (XmlNode xmlNode5 in xmlNodeList)
                {
                    try
                    {
                        XmlElement xmlElement3 = (XmlElement)xmlNode5;
                        string attribute3 = xmlElement3.GetAttribute("type");
                        EMapNodeType eMapNodeType = MapCfg.String2MapNodeType(attribute3);
                        string attribute4 = xmlElement3.GetAttribute("file");
                        MapNodeTypeInfo mapNodeTypeInfo = new MapNodeTypeInfo(eMapNodeType, attribute4);
                        string attribute5 = xmlElement3.GetAttribute("localPos");
                        if (!string.IsNullOrEmpty(attribute5))
                        {
                            mapNodeTypeInfo.LocalPos = UnityTools.String2Vector3(attribute5);
                        }
                        string attribute6 = xmlElement3.GetAttribute("localRotation");
                        if (!string.IsNullOrEmpty(attribute6))
                        {
                            mapNodeTypeInfo.LocalRotation = UnityTools.String2Vector3(attribute6);
                        }
                        string attribute7 = xmlElement3.GetAttribute("effectId");
                        if (!string.IsNullOrEmpty(attribute7))
                        {
                            mapNodeTypeInfo.EffectId = Convert.ToInt32(attribute7);
                        }
                        string attribute8 = xmlElement3.GetAttribute("triggerEffectId");
                        if (!string.IsNullOrEmpty(attribute8))
                        {
                            mapNodeTypeInfo.GetEffectId = Convert.ToInt32(attribute8);
                        }
                        this.m_dicMapNodeTypeInfo.Add(eMapNodeType, mapNodeTypeInfo);
                    }
                    catch (Exception ex)
                    {
                        this.logger.Fatal(ex.ToString());
                    }
                }
            }
            XmlNode xmlNode6 = xmlNode.SelectSingleNode("pveSetting");
            if (xmlNode6 != null)
            {
                this.PVESetting = new PVESetting();
                XmlNode xmlNode7 = xmlNode6.SelectSingleNode("ourBase");
                if (xmlNode7 != null)
                {
                    XmlElement xmlElement4 = (XmlElement)xmlNode7;
                    string attribute9 = xmlElement4.GetAttribute("file");
                    this.PVESetting.OurBaseModelFile = attribute9;
                    string attribute10 = xmlElement4.GetAttribute("LocalPos");
                    if (!string.IsNullOrEmpty(attribute10))
                    {
                        this.PVESetting.OurBaseLocalPos = UnityTools.String2Vector3(attribute10);
                    }
                    string attribute11 = xmlElement4.GetAttribute("LocalRotation");
                    if (!string.IsNullOrEmpty(attribute11))
                    {
                        this.PVESetting.OurBaseLocalRotation = UnityTools.String2Vector3(attribute11);
                    }
                }
                XmlNode xmlNode8 = xmlNode6.SelectSingleNode("enemyBase");
                if (xmlNode8 != null)
                {
                    XmlElement xmlElement5 = (XmlElement)xmlNode8;
                    string attribute12 = xmlElement5.GetAttribute("file");
                    this.PVESetting.EnemyBaseModelFile = attribute12;
                    string attribute13 = xmlElement5.GetAttribute("LocalPos");
                    if (!string.IsNullOrEmpty(attribute13))
                    {
                        this.PVESetting.EnemyBaseLocalPos = UnityTools.String2Vector3(attribute13);
                    }
                    string attribute14 = xmlElement5.GetAttribute("LocalRotation");
                    if (!string.IsNullOrEmpty(attribute14))
                    {
                        this.PVESetting.EnemyBaseLocalRotation = UnityTools.String2Vector3(attribute14);
                    }
                }
            }
        }
        catch (Exception ex2)
        {
            this.logger.Fatal(ex2.Message);
        }
    }
}
