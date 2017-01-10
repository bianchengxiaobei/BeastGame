using UnityEngine;
using System.Collections.Generic;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameWorld
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class GameWorld
    {
        #region 字段
        /// <summary>
        /// int=>场景中模型GUID
        /// </summary>
        private static Dictionary<int, EntityParent> gameObjects = new Dictionary<int, EntityParent>();
        /// <summary>
        /// 是否在城镇内
        /// </summary>
        public static bool isInTown = true;
        /// <summary>
        /// 是否显示技能特效
        /// </summary>
        public static bool isShowSkillFx = true;
        /// <summary>
        /// 是否正在加载场景
        /// </summary>
        public static bool isLoadingScene = false;
       /// <summary>
       /// 主角
       /// </summary>
        public static EntityMyself thePlayer;

        public static EntityParent m_currentEntity;
        public static SceneManager m_sceneManager;
        public static Dictionary<uint, EntityParent> entities = new Dictionary<uint, EntityParent>();
        #endregion
        #region 属性
        /// <summary>
        /// 游戏场景所有的实例物体
        /// </summary>
        public static Dictionary<int, EntityParent> GameObjects = new Dictionary<int, EntityParent>();
        /// <summary>
        /// 游戏中可见的实体，key=>uid,value=>entity
        /// </summary>
        public static Dictionary<uint, EntityParent> Entities
        {
            get { return entities;}
        }
        #endregion
        #region 构造方法
        static GameWorld()
        {
            AddListeners();
        }
        #endregion
        #region 公有方法
        public static void Init()
        {
            m_sceneManager = new SceneManager();
        }
        public static void SwitchScene(int oldSceneId, int newSceneId)
        {
            Application.LoadLevel(newSceneId);
            isLoadingScene = true;
        }
        /// <summary>
        /// 实体进入到游戏世界中
        /// </summary>
        /// <param name="entity"></param>
        public static void OnEnterWorld(EntityParent entity)
        {
            //主要是处理缓存实体到字典中
        }
        #endregion
        #region 私有方法
        private static void AddListeners()
        {
            EventDispatch.AddEventListener<RoleAttachedInfo>(Event.FrameWorkEvent.EntityAttached,OnEntityAttached);
        }
        private static void OnEntityAttached(RoleAttachedInfo roleInfo)
        {
            bool ab = false;
            if (GameWorld.thePlayer == null)
            {
                ab = true;
                thePlayer = new EntityMyself();
            }
            m_currentEntity = thePlayer;
            thePlayer.SetEntityInfo(roleInfo);
            if (ab)
            {
                thePlayer.OnEnterWorld();
            }
        }
        /// <summary>
        /// 实体进入到玩家的视线内，就实例化他
        /// </summary>
        /// <param name="info"></param>
        private static void AOINewEntity(EntityAttachedInfo info)
        {
            EntityParent entity;
            if (Entities.ContainsKey(info.Id) || (thePlayer != null && thePlayer.ID == info.Id))
            {
                return;
            }
            //根据传入的实体类型来初始化Entity
            switch (info.Name)
            {
                case "Avatar":
                    entity = new EntityPlayer();
                    break;
                case "Monster":
                    entity = new EntityBeast();
                    break;
                default:
                    entity = new EntityParent();
                    break;
            }
            entity.ID = info.Id;
            entity.SetEntityInfo(null);
            entity.OnEnterWorld();
            if (!isLoadingScene)
            {
                entity.CreateModel();
            }
            OnEnterWorld(entity);
        }
        #endregion
    }
}
