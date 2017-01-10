using UnityEngine;
using System.Collections.Generic;
using Game;
using Client.Data;
using UnityAssetEx.Export;
using GameClient.Audio;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityShow 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.15
// 模块描述：角色展示类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色展示类
/// </summary>
public class EntityShow : EntityPlayer
{
    #region 字段
    protected string kIdleAnim = null;
    protected string kShowAnim = "begin_show";
    protected string soundClip = null;
    #endregion
    #region 属性
    #endregion
    #region 子类重写方法
    public override void OnEnterWorld()
    {
        base.OnEnterWorld();

        switch ((int)this.Vocation)
        {
            case 0:
                LoadExprolerData();
                break;
            case 1:
                LoadExprolerData();
                break;
            case 2:
                LoadExprolerData();
                break;
            case 3:
                LoadExprolerData();
                break;
        }
        this.CreateActualModel();
    }
    public override void CreateActualModel()
    {
        base.CreateActualModel();
        DataAvatarModel data = GameData<DataAvatarModel>.dataMap[(int)Vocation];
        if (data != null)
        {
            ResourceManager.singleton.LoadModel(data.PrefabPath, new AssetRequestFinishedEventHandler((assetRequest) =>
            {
                GameObject gameobject = assetRequest.AssetResource.MainAsset as GameObject;
                gameobject = GameObject.Instantiate(gameobject) as GameObject;
                gameobject.name = "RenderNode";
                gameobject.transform.SetParent(Transform);
                gameobject.transform.localPosition = Vector3.zero;
                gameobject.transform.localScale = Vector3.one;
                gameobject.transform.localRotation = Quaternion.identity;
                GameObject = gameobject;
                if (!IsVisiable)
                {
                    this.SetVisiableWhenLoad(IsVisiable);
                }
                GatherRenderNodeInfo(gameobject);
                GatherMeshes(gameobject);
                Animation anim = gameobject.GetComponent<Animation>();
                anim.playAutomatically = false;
                anim.cullingType = AnimationCullingType.AlwaysAnimate;
                if (null == anim)
                {
                    Debug.LogError("没有Animation组件");
                }
                else
                {
                    animation = anim;
                }
                for (int k = 0; k < gameobject.transform.childCount; k++)
                {
                    Transform childNode = gameobject.transform.GetChild(k);
                    if (childNode.name == "Bip01")
                    {
                        boneRoot = childNode.gameObject;
                        break;
                    }
                }
                //关闭unity自带的投影
                for (int k = 0; k < gameobject.transform.childCount; ++k)
                {
                    Transform kChildTransform = gameobject.transform.GetChild(k);
                    GameObject kChildObject = kChildTransform.gameObject;

                    SkinnedMeshRenderer kSkinnedMeshRenderer = kChildObject.GetComponent<SkinnedMeshRenderer>();
                    if (kSkinnedMeshRenderer != null)
                        kSkinnedMeshRenderer.receiveShadows = false;

                    MeshRenderer kStaticMeshRenderer = kChildObject.GetComponent<MeshRenderer>();
                    if (kStaticMeshRenderer != null)
                        kStaticMeshRenderer.receiveShadows = false;
                }
                //加载投影
                this.PlayShow();
                uint[] equipment = new uint[] { 1100,1101,1102,1103,1104,1105,1106 };
                UpdateAvatar(equipment);
            }), Client.Common.AssetPRI.DownloadPRI_Plain);
        }
    }
    #endregion
    #region 公共方法
    public void PlayIdle()
    {
        if (this.animation != null && kIdleAnim != null && 
            this.animation[this.kIdleAnim] != null)
        {
            this.animation.cullingType = AnimationCullingType.AlwaysAnimate;
            this.animation[kIdleAnim].wrapMode = WrapMode.Loop;
            this.animation.CrossFadeQueued(kIdleAnim, 0.3f);
        }
    }
    public void PlayShow()
    {
        if (this.animation != null && kIdleAnim != null && 
            this.animation[this.kIdleAnim] != null && this.animation[this.kShowAnim] != null)
        {
            this.animation.cullingType = AnimationCullingType.AlwaysAnimate;
            this.animation[kShowAnim].wrapMode = WrapMode.Once;
            this.animation.Play(kShowAnim);
            if (this.soundClip != null)
            {
                AudioManager.singleton.PlayAudioOneShot(this.soundClip, () => 
                {
                    
                });
            }
            this.PlayIdle();
            this.SetVisiableWhenLoad(true);
        }
    }
    public bool IsPlayingShowAnim()
    {
        if (this.animation == null || kShowAnim == null || animation[kShowAnim] == null)
        {
            return false;
        }
        return this.animation.IsPlaying(kShowAnim);
    }
    #endregion
    #region 私有方法
    private void LoadExprolerData()
    {
        this.kIdleAnim = "warrior_idle_01";
        this.soundClip = "AudioClips/CreateCharacter/createcharacter_explorer.wav";
    }
    private void LoadEngineerData()
    {
        this.kIdleAnim = "warrior_idle_01";
        this.soundClip = "AudioClips/CreateCharacter/createcharacter_explorer.wav";
    }
    private void LoadCultivation()
    {
        this.kIdleAnim = "warrior_idle_01";
        this.soundClip = "AudioClips/CreateCharacter/createcharacter_explorer.wav";
    }
    private void LoadMagicain()
    {
        this.kIdleAnim = "warrior_idle_01";
        this.soundClip = "AudioClips/CreateCharacter/createcharacter_explorer.wav";
    }
    public void UpdateAvatar(uint[] equipId)
    {
        if (null == this.itemInfo || equipId.Length == 0)
        {
            return;
        }
        List<GameItem> akItemAdd = new List<GameItem>();
        List<GameItem> akItemRemain = new List<GameItem>();
        for (int i = 0; i < equipId.Length; i++)
        {
            int iEquitId = (int)equipId[i];
            if (itemInfo.ContainsKey(iEquitId))
            {
                akItemRemain.Add(itemInfo[iEquitId] as GameItem);
            }
            else
            {
                DataItemTamplates data = GameData<DataItemTamplates>.dataMap[iEquitId];
                if (null == data)
                {
                    continue;
                }
                GameItem item = new GameItem();
                akItemAdd.Add(item);
                item.templateId = iEquitId;
                item.mdlPath = data.FileName;
                item.mdlPartName = data.FilePart;
                item.anchorNodeName = data.AnchorNodeName;
            }
        }
        foreach (var item in akItemAdd)
        {
            itemInfo[item.templateId] = item;
            if (item.mdlPartName.Length > 0)
            {
                ChangeAvatar(item.mdlPath, item.mdlPartName, item.anchorNodeName);
            }
        }
    }
    #endregion
    #region 析构方法
    #endregion
}
