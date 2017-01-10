using UnityEngine;
using Client.UI.UICommon;
using System.Collections;
using Utility;
using Utility.Export;
using GameClient.Audio;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DLgBeginAnimation
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class DlgBeginAnimation : DlgBase<DlgBeginAnimation,DlgBeginAnimationBehaviour>
    {
        #region 字段
        private IXLog m_log = XLog.GetLog<DlgBeginAnimation>();
        #endregion
        #region 属性
        public override string fileName
        {
            get
            {
                return "DlgBeginAnimation";
            }
        }
        public override int layer
        {
            get
            {
                return 0;
            }
        }
        public override uint Type
        {
            get
            {
                return 4u;
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override void Init()
        {
            this.m_log.Debug("DlgBeginAnimation.Init()");
            //base.uiBehaviour.m_Texture_video.SetVisible(true);
        }
        public override void Reset()
        {
        }
        public override void RegisterEvent()
        {
        }
        protected override void OnShow()
        {
            base.OnShow();
            UnityGameEntry.Instance.StartCoroutine(this.PlayBeginAnimation());
        }
        #endregion
        #region 私有方法
        private IEnumerator PlayBeginAnimation()
        {
            MovieTexture movieTexture = Resources.Load("Data/Video/beginAnimation") as MovieTexture;
            if (null == movieTexture)
            {
                this.Finish();
            }
            else 
            {
                while (!movieTexture.isReadyToPlay)
                {
                    yield return null;
                }
                movieTexture.anisoLevel = 8;
                movieTexture.filterMode = FilterMode.Trilinear;
                movieTexture.loop = false;
                if (base.Prepared)
                {
                    base.uiBehaviour.m_Texture_video.SetTexture(movieTexture);
                    AudioSource audioSource = base.uiBehaviour.m_Texture_video.CachedGameObject.GetComponent<AudioSource>();
                    if (null == audioSource)
                    {
                        audioSource = base.uiBehaviour.m_Texture_video.CachedGameObject.AddComponent<AudioSource>();
                    }
                    audioSource.clip = movieTexture.audioClip;
                    audioSource.volume = Singleton<AudioManager>.singleton.VolumeMain;
                    audioSource.Play();
                }
                movieTexture.Play();
                float num = movieTexture.duration;
                if (num < 0f)
                {
                    num = 12f;
                }
                yield return new WaitForSeconds(1f);
                this.Finish();
            }
        }
        /// <summary>
        /// 播放视频完成之后，游戏状态改为Login
        /// </summary>
        private void Finish()
        {
            if (base.Prepared)
            {
                this.SetVisible(false);
                Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_Update);
                base.UnLoad();
            }
        }
        #endregion
    }
}
