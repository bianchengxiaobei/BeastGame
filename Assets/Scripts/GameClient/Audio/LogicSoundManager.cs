using UnityEngine;
using System.Collections.Generic;
using Utility;
using Game;
using Event = Game.Event;
using UnityAssetEx.Export;
using Client.Data;
using System.Linq;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LogicSoundManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class LogicSoundManager : Singleton<LogicSoundManager>
{
	#region 主角音效
    /// <summary>
    /// 主角音效
    /// </summary>
    public static Dictionary<int, AudioClip> avatarAudioClipBuffer = new Dictionary<int, AudioClip>();
	#endregion
	#region 属性
	#endregion
	#region 构造方法
    static LogicSoundManager()
    {
 
    }
	#endregion
	#region 公有方法
    public void Init()
    {
        AddListeners();
    }
    /// <summary>
    /// 播放实体音效（根据实体的职业）
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="action"></param>
    public void OnHitYelling(EntityParent entity, int action)
    {
        AudioSource ownerSource = entity.audioSource;
        if (null == ownerSource)
        {
            return;
        }
        int ownerVocation = (int)entity.Vocation;//主角职业
        ActionSoundData data = ActionSoundData.dataMap.FirstOrDefault(t => t.Value.vocation == ownerVocation && t.Value.action == action).Value;
        if (null == data)
        {
            return;
        }
        int sum = 0;
        foreach (var soundMessage in data.sound)
        {
            sum += soundMessage.Value;
        }
        int soundID = -1;
        int temp = RandomHelper.GetRandomInt(0, sum);
        foreach (var soundMessage in data.sound)
        {
            if (temp < soundMessage.Value)
            {
                soundID = soundMessage.Key;
                break;
            }
            temp -= soundMessage.Value;
        }
        if (soundID == -1)
        {
            return;
        }
        if (entity is EntityMyself)
        {
            MyselfLogicPlaySound(ownerSource, soundID);
        }
        else 
        {

        }
    }
    
	#endregion
	#region 私有方法
    private void AddListeners()
    {
        EventDispatch.AddEventListener<EntityParent,int>(Event.LogicSoundEvent.OnHitYelling, OnHitYelling);
    }
    public void RemoveListeners()
    {
        EventDispatch.RemoveEventListener<EntityParent,int>(Event.LogicSoundEvent.OnHitYelling, OnHitYelling);
    }
    /// <summary>
    /// 播放主角音效
    /// </summary>
    /// <param name="source"></param>
    /// <param name="soundID"></param>
    public void MyselfLogicPlaySound(AudioSource source, int soundID)
    {
        if (avatarAudioClipBuffer.ContainsKey(soundID))//如果已经加载过，包含这个主角音效，直接播放
        {
            EventDispatch.TriggerEvent<AudioSource, AudioClip>(Event.LogicSoundEvent.LogicPlaySoundByClip, source, avatarAudioClipBuffer[soundID]);//通过事件驱动触发播放委托
            return;
        }
        if (!SoundData.dataMap.ContainsKey(soundID))
        {
            Debug.Log("音效ID" + soundID + "不存在！");
            return;
        }
        ResourceManager.singleton.LoadAudio(SoundData.dataMap[soundID].path, new AssetRequestFinishedEventHandler((assetRequest) => 
        {
            Object clip = assetRequest.AssetResource.MainAsset;
            UnityEngine.Object.DontDestroyOnLoad(clip);
            if (clip is AudioClip)
            {
                EventDispatch.TriggerEvent<AudioSource, AudioClip>(Event.LogicSoundEvent.LogicPlaySoundByClip, source, clip as AudioClip);
                if (!avatarAudioClipBuffer.ContainsKey(soundID))
                {
                    avatarAudioClipBuffer.Add(soundID, clip as AudioClip);
                }
            }
        }),AssetPRI.DownloadPRI_Low);
    }
	#endregion
}
