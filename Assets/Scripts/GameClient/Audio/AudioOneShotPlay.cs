using UnityEngine;
using System.Collections;
using Utility.Export;
using UnityAssetEx.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：AudioOneShotPlay
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Audio
{
    internal class AudioOneShotPlay
    {
        #region 字段
        private GameObject m_hostObject;
        private VoidDelegate m_callBack;
        private AudioSource m_audioSource;
        private float m_audioVolume;
        private string m_audioFile;
        private IXLog m_log = XLog.GetLog<AudioOneShotPlay>();
        #endregion
        #region 属性
        public bool IsStopped
        {
            get;
            private set;
        }
        #endregion
        #region 构造方法
        public AudioOneShotPlay(GameObject hostObject, string strAudioFile, float audioVolume, VoidDelegate callback)
        {
            this.m_hostObject = hostObject;
            this.m_callBack = callback;
            this.m_audioVolume = audioVolume;
            this.m_audioFile = strAudioFile;
            this.IsStopped = false;
            ResourceManager.singleton.LoadAudio(strAudioFile, new AssetRequestFinishedEventHandler(this.OnLoadOneShotAudioFinished), AssetPRI.DownloadPRI_Low);
        }
        #endregion
        #region 公有方法
        public void Stop()
        {
            this.IsStopped = true;
            if (null != this.m_audioSource)
            {
                UnityEngine.Object.Destroy(this.m_audioSource);
            }
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 加载音效回调
        /// </summary>
        /// <param name="assetRequest"></param>
        private void OnLoadOneShotAudioFinished(IAssetRequest assetRequest)
        {
            if (assetRequest != null && assetRequest.AssetResource != null)
            {
                if (!this.IsStopped)
                {
                    this.m_audioSource = this.m_hostObject.AddComponent<AudioSource>();
                    this.m_audioSource.clip = (assetRequest.AssetResource.MainAsset as AudioClip);
                    this.m_audioSource.volume = this.m_audioVolume;
                    this.m_audioSource.Play();
                    UnityGameEntry.Instance.StartCoroutine(this.AutoDistroyOneShotAudio());
                }
            }
        }
        /// <summary>
        /// 播放音效完成之后自动摧毁
        /// </summary>
        /// <returns></returns>
        private IEnumerator AutoDistroyOneShotAudio()
        {
            if (null == this.m_audioSource.clip)
            {
                this.m_log.Error(this.m_audioFile + " does not exist");
                UnityEngine.Object.Destroy(this.m_audioSource);
                this.IsStopped = true;
            }
            else 
            {
                if (this.m_audioSource.clip.length <= 0f)
                {
                    this.m_log.Debug(this.m_audioSource.clip.name + " length is zero");
                }
                yield return new WaitForSeconds(this.m_audioSource.clip.length);//等待音效播放完成
                if (this.m_audioSource != null && !this.IsStopped)
                {
                    UnityEngine.Object.Destroy(this.m_audioSource);
                    this.IsStopped = true;
                    if (null != this.m_callBack)
                    {
                        this.m_callBack();//音效播放完成后回调
                    }
                }
            }
            yield break;
        }
        #endregion
    }
    public delegate void VoidDelegate();
}
