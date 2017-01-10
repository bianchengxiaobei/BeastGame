using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using Client;
using Utility.Export;
using Utility;
using GameData;
using Client.Data;
using Client.Common;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CSceneMgr
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：场景地图管理器
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    internal class CSceneMgr
    {
        #region 字段
        private static CSceneMgr gs_Singleton = new CSceneMgr();
        private CScene m_sceneCurrent = null;//现在的场景
        private IXLog m_log = XLog.GetLog<CSceneMgr>();
        private Coroutine m_coroutineOnScenePrepare;
        private AsyncOperation m_AsyncOperation = null;
        private IAssetRequest m_sceneRequest = null;
        private List<MapNodeBuilding> m_listMapNodeBuilding = new List<MapNodeBuilding>();
        private Action m_actionOnLoadSceneFinish = null;//加载场景完成的委托
        public Action OnScenePreparedAction = null;
        private bool m_bReviveMask = false;//复活标志
        private bool m_bLoadSceneFinished = false;//是否加载完成
        private bool m_bMapBehaviourPrepared = false;//地图格子是否准备好创建
        private List<CTerrainNodeData> m_ListNodeData = null;
        #endregion
        #region 属性
        public static CSceneMgr singleton
        {
            get 
            {
                return CSceneMgr.gs_Singleton;
            }
        }
        public CScene CurScene
        {
            get 
            {
                return this.m_sceneCurrent;
            }
        }
        /// <summary>
        /// 复活标记
        /// </summary>
        public bool IsReviveMask 
        {
            get { return this.m_bReviveMask;}
            set
            {
                this.m_bReviveMask = value;
                CameraManager.Instance.FollowHeroInRound = this.m_bReviveMask;
            }
        }
        /// <summary>
        /// 加载进度
        /// </summary>
        public float ProcessValue
        {
            get
            {
                float result;
                if (null != this.m_AsyncOperation)
                {
                    result = this.m_AsyncOperation.progress;
                }
                else
                {
                    result = 1f;
                }
                return result;
            }
        }
        #endregion
        #region 构造函数
        public CSceneMgr()
        {
            MapCfg.Register("empire_base", EMapNodeType.MAP_NODE_EMPIRE_BASE, EMapJudgeType.MAP_JUDGE_INVALID, false);
            MapCfg.Register("league_base", EMapNodeType.MAP_NODE_LEAGUE_BASE, EMapJudgeType.MAP_JUDGE_INVALID, false);
            MapCfg.Register("rock", EMapNodeType.MAP_NODE_ROCK, EMapJudgeType.MAP_JUDGE_INVALID, false);
            MapCfg.Register("shop", EMapNodeType.MAP_NODE_SHOP, EMapJudgeType.MAP_JUDGE_INVALID, false);
            MapCfg.Register("magic_spring", EMapNodeType.MAP_NODE_MAGIC_SPRING, EMapJudgeType.MAP_JUDGE_DELAY, false);
            MapCfg.Register("golden", EMapNodeType.MAP_NODE_GOLDEN, EMapJudgeType.MAP_JUDGE_DELAY, true);
            MapCfg.Register("camp", EMapNodeType.MAP_NODE_CAMP, EMapJudgeType.MAP_JUDGE_DELAY, true);
            MapCfg.Register("swamp", EMapNodeType.MAP_NODE_SWAMP, EMapJudgeType.MAP_JUDGE_DELAY, true);
            MapCfg.Register("reborn_empire", EMapNodeType.MAP_NODE_REBORN_EMPIRE, EMapJudgeType.MAP_JUDGE_INVALID, true);
            MapCfg.Register("reborn_league", EMapNodeType.MAP_NODE_REBORN_LEAGUE, EMapJudgeType.MAP_JUDGE_INVALID, true);
            MapCfg.Register("desert", EMapNodeType.MAP_NODE_DESERT, EMapJudgeType.MAP_JUDGE_INVALID, true);
            MapCfg.Register("water", EMapNodeType.MAP_NODE_WATER, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("forest", EMapNodeType.MAP_NODE_FOREST, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("money", EMapNodeType.MAP_NODE_MONEY, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("teleport", EMapNodeType.MAP_NODE_TELEPORT, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("magic_turret", EMapNodeType.MAP_NODE_MAGIC_TURRET, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("rune_array", EMapNodeType.MAP_NODE_RUNE_ARRAY, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("golden_new", EMapNodeType.MAP_NODE_GOLDEN_NEW, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("money_new", EMapNodeType.MAP_NODE_MONEY_NEW, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("camp_new", EMapNodeType.MAP_NODE_CAMP_NEW, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("accelerate_array", EMapNodeType.MAP_NODE_ACCELERATE_ARRAY, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("devil_altar", EMapNodeType.MAP_NODE_DEVIL_ALTAR, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("healing_ward", EMapNodeType.MAP_NODE_HEALING_WARD, EMapJudgeType.MAP_JUDGE_INSTANT, false);
            MapCfg.Register("frost_column", EMapNodeType.MAP_NODE_ICE_WALL, EMapJudgeType.MAP_JUDGE_INSTANT, false);
            MapCfg.Register("open_door", EMapNodeType.MAP_NODE_OPEN_DOOR, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("close_door", EMapNodeType.MAP_NODE_CLOSE_DOOR, EMapJudgeType.MAP_JUDGE_INSTANT, false);
            MapCfg.Register("door_switch", EMapNodeType.MAP_NODE_DOOR_SWITCH, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("golden_new_2", EMapNodeType.MAP_NODE_GOLDEN_NEW_2, EMapJudgeType.MAP_JUDGE_INSTANT, true);
            MapCfg.Register("stable", EMapNodeType.MAP_NODE_STABLE, EMapJudgeType.MAP_JUDGE_DELAY, true);
            MapCfg.Register("MonkeyKing_image", EMapNodeType.MAP_NODE_MONKEY, EMapJudgeType.MAP_JUDGE_INSTANT, false);
            MapCfg.Register("totem", EMapNodeType.MAP_NODE_TOTEM, EMapJudgeType.MAP_JUDGE_INSTANT, false);
            MapCfg.Register("skill", EMapNodeType.MAP_NODE_SKILL, EMapJudgeType.MAP_JUDGE_INSTANT, false);
            MapCfg.Register("rock_prison", EMapNodeType.MAP_NODE_ROCK_PRISON, EMapJudgeType.MAP_JUDGE_INSTANT, false);
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 神兽复活重新设置摄像机
        /// </summary>
        public void ReviveResetCamera()
        {
            if (!CameraManager.Instance.FollowHeroInRound)
            {
                this.IsReviveMask = true;
            }
        }
        /// <summary>
        /// 初始化摄像机的位置
        /// </summary>
        public void ResetCamera()
        {
            if (Camera.main != null)
            {
                //初始化摄像机的根节点
                CameraManager.Instance.InitGameNode();
                DataMaplist mapData = GameData<DataMaplist>.dataMap[(int)Singleton<RoomManager>.singleton.MapId];
                if (Singleton<PlayerRole>.singleton.CampType == ECampType.CAMP_LEAGUE)
                {
                    if (mapData != null)
                    {
                        CameraManager.Instance.GameNode.localPosition = UnityTools.String2Vector3(mapData.LCameraPos);
                        CameraManager.Instance.GameNode.localEulerAngles = UnityTools.String2Vector3(mapData.LCameraAngle);
                    }
                    else
                    {
                        //默认的摄像机初始位置
                    }
                }
                else
                {
                    if (mapData != null)
                    {
                        CameraManager.Instance.GameNode.localPosition = UnityTools.String2Vector3(mapData.ECameraPos);
                        CameraManager.Instance.GameNode.localEulerAngles = UnityTools.String2Vector3(mapData.ECameraAngle);
                    }
                    else
                    {
                        //默认的摄像机初始位置
                    }
                }
                CameraManager.Instance.MaxMapX = mapData.MapCenterX + mapData.MaxX;
                CameraManager.Instance.MinMapX = mapData.MapCenterX - mapData.MaxX;
                CameraManager.Instance.MaxMapZ = mapData.MapCenterY + mapData.MaxY;
                CameraManager.Instance.MinMapZ = mapData.MapCenterY - mapData.MaxY;
                CameraManager.Instance.MaxScale = mapData.MaxCameraScale;
                CameraManager.Instance.MinScale = mapData.MinCameraScale;
                CameraManager.Instance.MouseWheelSensitivity = mapData.MouseWheelSensitivity;
                CameraManager.Instance.Init();
            }
        }
        /// <summary>
        /// 创建游戏地图场景
        /// </summary>
        /// <param name="dwMapID">地图id</param>
        /// <returns></returns>
        public CScene CreateScene(uint dwMapID)
        {
            this.m_bLoadSceneFinished = false;
            this.m_bMapBehaviourPrepared = false;
            this.RegisterLoadSceneFinishCallback(new Action(this.OnLoadSceneFinish));
            this.m_log.Debug("CreateScene:" + dwMapID);
            this.m_sceneCurrent = new CScene();
            CScene result;
            if (!this.m_sceneCurrent.Init(dwMapID))
            {
                result = null;
            }
            else 
            {
                this.ResetCamera();
                result = this.m_sceneCurrent;
            }
            return result;
        }
        /// <summary>
        /// 加载战斗游戏场景
        /// </summary>
        public void LoadScene()
        {
            if (null == this.m_sceneCurrent)
            {
                this.m_log.Error("null == m_sceneCurrent");
            }
            else
            {
                DataMaplist dataByID = GameData<DataMaplist>.dataMap[((int)Singleton<RoomManager>.singleton.MapId)];
                if (null == dataByID)
                {
                    this.m_log.Error("null == dataMap:" + Singleton<RoomManager>.singleton.MapId);
                }
                else 
                {
                    Singleton<HexagonManager>.singleton.Init(this.m_sceneCurrent.InitPos, this.m_sceneCurrent.InitRotation);
                    this.CreateTerrainBuildings(this.m_sceneCurrent);
                    this.SetMapNodeBuildingVisible(EMapNodeType.MAP_NODE_LEAGUE_BASE, true);
                    this.SetMapNodeBuildingVisible(EMapNodeType.MAP_NODE_EMPIRE_BASE, true);
                    this.EnterScene(dataByID.MapFile);
                }
            }
        }
        public void OnStartBeastRound(long unBeastId)
        {
            for (int i = 0; i < this.m_listMapNodeBuilding.Count; i++)
            {
                MapNodeBuilding mapNodeBuilding = this.m_listMapNodeBuilding[i];
                mapNodeBuilding.OnStartBeastRound(unBeastId);
            }
        }
        /// <summary>
        /// 创建格子模型建筑
        /// </summary>
        /// <param name="scene"></param>
        public void CreateTerrainBuildings(CScene scene)
        {
            foreach (var XYAndNode in scene.DicMapData)
            {
                foreach (var YAndNode in XYAndNode.Value)
                {
                    MapNode node = YAndNode.Value;
                    MapNodeBuilding mapNodeBuilding = new MapNodeBuilding();
                    mapNodeBuilding.MapNodeType = node.m_eMapNodeType;
                    mapNodeBuilding.HexPos = new CVector3(node.nIndexX, node.nIndexY, node.nIndexU);
                    //从服务器收到动态地图的消息，里面存储不同格子坐标上的不同建筑
                    if (this.m_ListNodeData != null)
                    {
                        for (int i = 0; i < this.m_ListNodeData.Count; i++)
                        {
                            if (mapNodeBuilding.HexPos.Equals(this.m_ListNodeData[i].m_oPos))
                            {
                                EMapNodeType eMapNodeType = (EMapNodeType)this.m_ListNodeData[i].m_nType;
                                mapNodeBuilding.MapNodeType = eMapNodeType;
                                mapNodeBuilding.ChangeNodeType(eMapNodeType);
                                this.m_sceneCurrent.ChangeMapNodeType(this.m_ListNodeData[i].m_oPos, MapCfg.MapNodeType2String(eMapNodeType));
                                break;
                            }
                        }
                    }

                    //DataTerrain_node dataByType = DataTerrain_nodeManager.Instance.GetDataByType(MapCfg.MapNodeType2String(node.m_eMapNodeType));
                    DataTerrainNode dataByType = GameData<DataTerrainNode>.dataMap[MapCfg.MapNodeTypeToInt(node.m_eMapNodeType)];
                    if (null != dataByType)
                    {
                        mapNodeBuilding.EffectId = dataByType.EffectId;
                        mapNodeBuilding.TriggerEffectId = dataByType.TriggerEffectId;
                    }
                    //初始化基地模型
                    MapNodeTypeInfo mapNodeTypeInfo = null;
                    if (scene.DicMapNodeTypeInfo.TryGetValue(node.m_eMapNodeType, out mapNodeTypeInfo))
                    {
                        mapNodeBuilding.ModelFile = mapNodeTypeInfo.ModelFile;
                        mapNodeBuilding.OriginModelFile = mapNodeTypeInfo.ModelFile;
                        mapNodeBuilding.ChangeModel();
                    }
                    this.m_listMapNodeBuilding.Add(mapNodeBuilding);
                }
            }
        }
        /// <summary>
        /// 显示格子建筑是否高亮
        /// </summary>
        /// <param name="eShowHexagonType"></param>
        /// <param name="listHex"></param>
        public void ShowHexagon(EnumShowHexagonType eShowHexagonType, List<CVector3> listHex)
        {
            if (eShowHexagonType == EnumShowHexagonType.eShowHexagonType_Highlight)
            {
                for (int i = 0; i < this.m_listMapNodeBuilding.Count; i++)
                {
                    MapNodeBuilding mapNodeBuild = this.m_listMapNodeBuilding[i];
                    if (listHex != null && listHex.Exists((CVector3 p) => p.Equals(mapNodeBuild.HexPos)))
                    {
                        mapNodeBuild.IsHighlight = true;
                    }
                    else
                    {
                        mapNodeBuild.IsHighlight = false;
                    }
                }
            }
        }
        /// <summary>
        /// 不显示格子建筑高亮
        /// </summary>
        /// <param name="eShowHexagonType"></param>
        public void ClearHexagon(EnumShowHexagonType eShowHexagonType)
        {
            if (eShowHexagonType == EnumShowHexagonType.eShowHexagonType_Highlight)
            {
                for (int i = 0; i < this.m_listMapNodeBuilding.Count; i++)
                {
                    MapNodeBuilding mapNodeBuilding = this.m_listMapNodeBuilding[i];
                    mapNodeBuilding.IsHighlight = false;
                }
            }
        }
        /// <summary>
        /// 显示技能释放格子
        /// </summary>
        /// <param name="list"></param>
        public void ShowCaseRange(List<CVector3> list)
        {
            Singleton<HexagonManager>.singleton.ShowHexagon(EnumShowHexagonType.eShowHexagonType_CastRange, list);
        }
        /// <summary>
        /// 不显示技能范围格子
        /// </summary>
        public void ClearCastRange()
        {
            Singleton<HexagonManager>.singleton.ClearHexagon(EnumShowHexagonType.eShowHexagonType_CastRange);
        }
        /// <summary>
        /// 显示所有格子模型
        /// </summary>
        /// <param name="eMapNodeType"></param>
        /// <param name="bVisible"></param>
        public void SetMapNodeBuildingVisible(EMapNodeType eMapNodeType, bool bVisible)
        {
            for (int i = 0; i < this.m_listMapNodeBuilding.Count; i++)
            {
                MapNodeBuilding mapNodeBuilding = this.m_listMapNodeBuilding[i];
                if (mapNodeBuilding.MapNodeType == eMapNodeType)
                {
                    mapNodeBuilding.SetVisible(bVisible);
                }
            }
        }
        /// <summary>
        /// 是否开启基地的碰撞器的启动
        /// </summary>
        /// <param name="enable"></param>
        public void ControlColliderEnable(bool enable)
        {
            MapNodeBuilding base1 = this.GetMapNodeBuildingByType(EMapNodeType.MAP_NODE_EMPIRE_BASE);
            if (base1 != null)
            {
                base1.IsCollider = enable;
            }
            MapNodeBuilding base2 = this.GetMapNodeBuildingByType(EMapNodeType.MAP_NODE_LEAGUE_BASE);
            if (base2 != null)
            {
                base2.IsCollider = enable;
            }
        }
        /// <summary>
        /// 根据格子类型取得MapNodeBuilding类
        /// </summary>
        /// <param name="eMapNodeType"></param>
        /// <returns></returns>
        public MapNodeBuilding GetMapNodeBuildingByType(EMapNodeType eMapNodeType)
        {
            MapNodeBuilding result = null;
            foreach (var build in this.m_listMapNodeBuilding)
            {
                if (build.MapNodeType == eMapNodeType)
                {
                    result = build;
                }
            }
            return result;
        }
        /// <summary>
        /// 通过资源加载场景回调
        /// </summary>
        /// <param name="assetRequest"></param>
        public void OnLoadSceneAssetFinished(IAssetRequest assetRequest)
        {
            if (assetRequest != null && null != assetRequest.AssetResource)
            {
                this.m_sceneRequest = assetRequest;
                string strSceneName = Path.GetFileNameWithoutExtension(assetRequest.AssetResource.URL).Replace("scenes_", "");
                UnityGameEntry.Instance.StartCoroutine(this.loadScene(strSceneName));
            }
        }
        /// <summary>
        /// 地图MapBehavior准备好
        /// </summary>
        public void OnMapBehaviourPrepared()
        {
            this.m_bMapBehaviourPrepared = true;
            Singleton<HexagonManager>.singleton.ConstructMap();
            if (this.m_bLoadSceneFinished && this.m_bMapBehaviourPrepared)
            {
                this.m_coroutineOnScenePrepare = UnityGameEntry.Instance.StartCoroutine(this.OnScenePrepared());
            }
        }
        /// <summary>
        /// 停止协程
        /// </summary>
        public void Clear()
        {
            if (this.m_coroutineOnScenePrepare != null)
            {
                UnityGameEntry.Instance.StopCoroutine(this.m_coroutineOnScenePrepare);
            }
        }
        #endregion 
        #region 私有方法
        /// <summary>
        /// 注册加载场景完成之后的委托
        /// </summary>
        /// <param name="actionCallback"></param>
        private void RegisterLoadSceneFinishCallback(Action actionCallback)
        {
            this.m_actionOnLoadSceneFinish = (Action)Delegate.Combine(this.m_actionOnLoadSceneFinish, actionCallback);
        }
        /// <summary>
        /// 场景加载完成的委托方法
        /// </summary>
        private void OnLoadSceneFinish()
        {
            this.m_bLoadSceneFinished = true;
            if (this.m_bLoadSceneFinished && this.m_bMapBehaviourPrepared)
            {
                this.m_coroutineOnScenePrepare = UnityGameEntry.Instance.StartCoroutine(this.OnScenePrepared());
            }
        }
        /// <summary>
        /// 协程场景准备，当场景加载完成之后
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnScenePrepared()
        {
            yield return new WaitForSeconds(3f);
            if (Singleton<RoomManager>.singleton.GetOurCampData().CampType == Singleton<PlayerRole>.singleton.CampType)
            {
                GameObject bornPos = GameObject.Find("Born_Pos");
                if (bornPos != null)
                {
                    bornPos.renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
                }
            }
            GameObject colliders = GameObject.Find("/Tree_Box_Collider");
            if (colliders != null)
            {
                BoxCollider[] componentsInChildren = colliders.GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    BoxCollider boxCollider = componentsInChildren[i];
                    boxCollider.size = new Vector3(0.2f, boxCollider.size.y, 0.2f);
                }
            }
            if (this.OnScenePreparedAction != null)
            {
                this.OnScenePreparedAction();
                this.OnScenePreparedAction = null;
            }
            yield break;
        }
        private void EnterScene(string strSceneName)
        {
            this.m_log.Debug("EnterScene:" + strSceneName);
            if (Application.loadedLevelName.Equals(strSceneName))
            {
                this.m_log.Error(string.Format("Application.loadedLevelName.Equals(strSceneName) == true:{0}", strSceneName));
            }
            else 
            {
                if (!CommonDefine.IsMobilePlatform)
                {
                   UnityGameEntry.Instance.StartCoroutine(this.loadScene(strSceneName));
                }
                else
                {
                    IAssetRequest assetRequest = ResourceManager.singleton.LoadScene("scenes/" + strSceneName, new AssetRequestFinishedEventHandler(this.OnLoadSceneAssetFinished), AssetPRI.DownloadPRI_Superlative);
                }
            }
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="strSceneName"></param>
        /// <returns></returns>
        private IEnumerator loadScene(string strSceneName)
        {
            Debug.Log("load scene: " + strSceneName);
            //异步加载场景
            this.m_AsyncOperation = Application.LoadLevelAsync(strSceneName);
            yield return this.m_AsyncOperation.isDone;
            try
            {
                //执行场景加载完成回调
                if (null != this.m_actionOnLoadSceneFinish)
                {
                    this.m_actionOnLoadSceneFinish();
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
            this.m_actionOnLoadSceneFinish = null;
            if (this.m_sceneRequest != null)
            {
                this.m_sceneRequest.Dispose();
            }
            yield return null;
            yield break;
        }
        #endregion 
    }
}