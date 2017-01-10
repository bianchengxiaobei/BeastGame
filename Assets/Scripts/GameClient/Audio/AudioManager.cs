using UnityEngine;
using System.Collections;
using Utility;
using UnityAssetEx.Export;
using Utility.Export;
using Event = Game.Event;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：AudioManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.11
// 模块描述：声音管理器
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Audio
{
    internal class AudioManager : Singleton<AudioManager>, IAudioManager
    {
        #region 字段
        private string m_strCurMusicFile = string.Empty;
        private AudioSource m_curMusicAudioSource = null;
        private IAssetRequest m_assetRequestMusic = null;
        private float m_fVolumeBgMusic = 1f;
        private float m_fVolumeEffect = 1f;
        private float m_fVolumeMain = 1f;
        private float m_fVolumeBeastVoice = 1f;
        private bool m_bIsMuted = false;
        private bool m_bEnableMusic = true;
        private bool m_bEnableEffect = true;
        private bool m_bEnable = true;
        private bool m_bNeedFadeOut = false;
        private float m_fFadeOutDuration = 1f;
        private float m_fFadeOutStartTime = 0f;
        private GameObject m_gameObjectCached = null;
        private IXLog m_log = XLog.GetLog<AudioManager>();
        #endregion
        #region 属性
        /// <summary>
        /// 背景音乐的音量
        /// </summary>
        public float VolumeBgMusic
        {
            get { return this.m_fVolumeBgMusic; }
            set { this.m_fVolumeBgMusic = value; }
        }
        /// <summary>
        /// 特效的音量
        /// </summary>
        public float VolumeEffect 
        {
            get { return this.m_fVolumeEffect; }
            set { this.m_fVolumeEffect = value; }
        }
        /// <summary>
        /// 神兽的音量
        /// </summary>
        public float VolumeBeastVoice 
        {
            get { return this.m_fVolumeBeastVoice; }
            set 
            {
                this.m_fVolumeBeastVoice = value;
                UserOptions.Singleton.BeastVoiceValue = this.m_fVolumeBeastVoice;
            }
        }
        /// <summary>
        /// 主界面的音量
        /// </summary>
        public float VolumeMain 
        {
            get { return this.m_fVolumeMain; }
            set { this.m_fVolumeMain = value; }
        }
        /// <summary>
        /// 是否是静音
        /// </summary>
        public bool IsMuted 
        {
            get { return this.m_bIsMuted; }
            set { this.m_bIsMuted = value; }
        }
        /// <summary>
        /// 是否启用背景音乐
        /// </summary>
        public bool EnableMusic 
        {
            get { return this.m_bEnableMusic; }
            set { this.m_bEnableMusic = value; }
        }
        /// <summary>
        /// 是否启用特效音效
        /// </summary>
        public bool EnableEffect 
        {
            get { return this.m_bEnableEffect; }
            set { this.m_bEnableEffect = value; }
        }
        public bool Enable 
        {
            get { return this.m_bEnable; }
            set { this.m_bEnable = value; }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.m_gameObjectCached = new GameObject("AudioManager");
            Object.DontDestroyOnLoad(this.m_gameObjectCached);
            this.m_curMusicAudioSource = this.m_gameObjectCached.AddComponent<AudioSource>();
            this.IsMuted = UserOptions.Singleton.MuteSound;
            this.VolumeMain = UserOptions.Singleton.SoundValue;
            this.VolumeBgMusic = UserOptions.Singleton.BGSoundValue;
            this.VolumeEffect = UserOptions.Singleton.UISoundValue;
            AddListeners();
        }
        public void AddListeners()
        {
            EventDispatch.AddEventListener<AudioSource, AudioClip>(Event.LogicSoundEvent.LogicPlaySoundByClip, LogicPlaySoundByClip);
        }
        public void RemoveListeners()
        {
            EventDispatch.RemoveEventListener<AudioSource, AudioClip>(Event.LogicSoundEvent.LogicPlaySoundByClip, LogicPlaySoundByClip);
        }
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="strAudio"></param>
        public void PlayMusic(string strAudio)
        {
            if (null == this.m_curMusicAudioSource)
            {
                this.m_log.Error("null == AudioSource");
            }
            else 
            {
                this.m_bNeedFadeOut = false;
                if (strAudio != this.m_strCurMusicFile)
                {
                    ResourceManager.singleton.LoadAudio(strAudio, new AssetRequestFinishedEventHandler(this.OnLoadMusicFinished), AssetPRI.DownloadPRI_Low);
                    this.m_strCurMusicFile = strAudio;
                }
            }
        }
        /// <summary>
        /// 停止播放音乐
        /// </summary>
        public void StopMusic()
        {
            if (null == this.m_curMusicAudioSource)
            {
                this.m_log.Error("null == AudioSource");
            }
            else 
            {
                this.m_curMusicAudioSource.Stop();
                this.m_curMusicAudioSource.clip = null;
                if (null != this.m_assetRequestMusic)
                {
                    this.m_assetRequestMusic.Dispose();
                    this.m_assetRequestMusic = null;
                }
            }
        }
        /// <summary>
        /// 淡出音乐（从有到无）
        /// </summary>
        /// <param name="duration">淡出间隔</param>
        public void FadeOutMusic(float duration)
        {
            this.m_bNeedFadeOut = true;
            this.m_fFadeOutDuration = duration > 0.1f ? duration : 0.1f;
            this.m_fFadeOutStartTime = Time.time;
        }
        public AudioOneShotPlay PlayAudioOneShot(string strAudioFile, VoidDelegate callBack)
        {
            return this.PlayAudioOneShot(strAudioFile, this.VolumeEffect, callBack);
        }
        public AudioOneShotPlay PlayAudioOneShot(string strAudioFile, float fVolume, VoidDelegate callBack)
        {
            return new AudioOneShotPlay(this.m_gameObjectCached, strAudioFile, fVolume, callBack);
        }
        /// <summary>
        /// 播放实体音效
        /// </summary>
        /// <param name="source"></param>
        /// <param name="clip"></param>
        public void LogicPlaySoundByClip(AudioSource source, AudioClip clip)
        {
            PlaySoundOnSourceByObject(source, clip);
        }

        /// <summary>
        /// 如果有音乐淡出的话，就慢慢减小音量，然后停止
        /// </summary>
        public void Update()
        {
            if (this.m_bNeedFadeOut)
            {
                float num = (this.m_fFadeOutDuration + this.m_fFadeOutStartTime - Time.time) / this.m_fFadeOutDuration;
                if (num > 0f)
                {
                    if (this.m_curMusicAudioSource != null)
                    {
                        this.m_curMusicAudioSource.volume = num * this.m_fVolumeBgMusic;
                    }
                }
                else 
                {
                    this.m_bNeedFadeOut = false;
                    this.StopMusic();
                }
            }
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 加载音频的回调函数，主要是处理播放
        /// </summary>
        /// <param name="assetRequest"></param>
        private void OnLoadMusicFinished(IAssetRequest assetRequest)
        {
            //如果不存在音频就停止播放
            if (null == assetRequest || null == assetRequest.AssetResource)
            {
                this.m_curMusicAudioSource.Stop();
                this.m_curMusicAudioSource.clip = null;
            }
            else
            {
                if (this.m_assetRequestMusic != null)
                {
                    this.m_assetRequestMusic.Dispose();
                }
                this.m_assetRequestMusic = assetRequest;
                this.m_curMusicAudioSource.clip = (assetRequest.AssetResource.MainAsset as AudioClip);
                this.m_curMusicAudioSource.loop = true;
                this.m_curMusicAudioSource.volume = this.m_fVolumeBgMusic;
                if (this.m_bEnableMusic || this.m_bEnable)
                {
                    this.m_curMusicAudioSource.Play();
                }
            }
        }
        private void PlaySoundOnSourceByObject(AudioSource gameObjectAudioSource, AudioClip clip, bool isLoop = false)
        {
            if (null == clip || null == gameObjectAudioSource)
            {
                Debug.LogWarning("实体没有AudioSource,AudioClip不存在");
                return;
            }
            gameObjectAudioSource.clip = clip;
            gameObjectAudioSource.volume = m_fVolumeMain;
            gameObjectAudioSource.loop = isLoop;
            gameObjectAudioSource.Play();
        }
        #endregion
    }
}
