using UnityEngine;
using System.Collections;
using UnityAssetEx.Export;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityMyself
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public partial class EntityMyself : EntityPlayer
    {
        #region 字段
        private bool m_bIsCreatingModel = false;
        //上次使用技能的时间
        public static float preSkillTime = 0;
        #endregion
        #region 属性
        #endregion
        #region 重写方法
        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
            CreateModel();
           
        }
        public override void CreateModel()
        {
            CreateActualModel();
        }
        public override void CreateActualModel()
        {
            m_bIsCreatingModel = true;
            DataAvatarModel data = GameData<DataAvatarModel>.dataMap[(int)Vocation];
            if (data != null)
            {
                ResourceManager.singleton.LoadModel(data.PrefabPath, new AssetRequestFinishedEventHandler((assetRequest) => 
                {
                    GameObject gameobject = assetRequest.AssetResource.MainAsset as GameObject;
                    gameobject = GameObject.Instantiate(gameobject) as GameObject;                   
                    gameobject.tag = "Player";
                    ActorMyself actor = gameobject.GetComponent<ActorMyself>();
                    if (actor == null)
                    {
                        actor = gameobject.AddComponent<ActorMyself>();
                    }
                    motor = gameobject.GetComponent<GameMotorMyself>();
                    if (motor == null)
                    {
                        motor = gameobject.AddComponent<GameMotorMyself>();
                    }
                    animator = gameobject.GetComponent<Animator>();
                    audioSource = gameobject.GetComponent<AudioSource>();
                    if (null == audioSource)
                    {
                        audioSource = gameobject.AddComponent<AudioSource>();
                    }
                    audioSource.rolloffMode = AudioRolloffMode.Custom;
                    sfxHandler = gameobject.AddComponent<SfxHandler>();
                    actor.m_motor = motor;
                    actor.Entity = this;
                    GameObject = gameobject;
                    Transform = gameobject.transform;
                    Transform.gameObject.layer = 8;
                    UpdatePosition();
                    if (data.Scale > 0)
                    {
                        Transform.localScale = new Vector3(data.Scale, data.Scale, data.Scale);
                    }
                    GameInputManager.singleton.Init(gameobject.GetComponent<PCGameInputManager>(),this);
                    gameobject.SetActive(false);
                    gameobject.SetActive(true);                 
                    m_bIsCreatingModel = false;
                }), Client.Common.AssetPRI.DownloadPRI_Plain);               
            }
            else
            {
                Debug.Log("找不到DataAvartarModel文件");
            }
        }
        
        #endregion
        #region 公有方法
        #endregion
        #region 私有方法
        #endregion
    }
}