using UnityEngine;
using System.Collections;
using Client;
using Client.UI.UICommon;
using GameClient.Audio;
using Utility;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EffectManagerBase
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：特效管理基类
//----------------------------------------------------------------*/
#endregion
namespace Effect.Export
{
    public class EffectManagerBase 
    {
	    #region 字段
        private static IUIManager s_uiManager;
        private static IAudioManager s_audioManager;
        private static ICameraManager s_cameraManager;
	    #endregion
	    #region 属性
        /// <summary>
        /// 是否启用音频
        /// </summary>
        public static bool EnableEffectAudio
        {
            get 
            {
                bool result = true;
                if (null != EffectManagerBase.s_audioManager)
                {
                    result = (EffectManagerBase.s_audioManager.Enable && EffectManagerBase.s_audioManager.EnableEffect);
                }
                return result;
            }
        }
        /// <summary>
        /// 特效的音量
        /// </summary>
        public static float VolumeEffect
        {
            get 
            {
                float result = 1f;
                if (EffectManagerBase.s_audioManager != null)
                {
                    result = EffectManagerBase.s_audioManager.VolumeEffect * EffectManagerBase.s_audioManager.VolumeMain;
                }
                return result;
            }
        }
        /// <summary>
        /// UIRoot
        /// </summary>
        public static Transform UIRoot
        {
            get 
            {
                Transform result;
                if (null != EffectManagerBase.s_uiManager)
                {
                    result = EffectManagerBase.s_uiManager.UIRoot;
                }
                else 
                {
                    result = null;
                }
                return result;
            }
        }
        public static Vector3 LeagueDir
        {
            get;
            set;
        }
        public static Vector3 EmpireDir
        {
            get;
            set;
        }
        public bool HightLight 
        {
            get 
            {
                return Singleton<EffectManagerImplement>.singleton.HighLight;
            }
            set 
            {
                Singleton<EffectManagerImplement>.singleton.HighLight = value;
            }
        }
        #endregion
	    #region 构造方法
        #endregion
	    #region 公有方法
        /// <summary>
        /// 设置摄像机管理器
        /// </summary>
        /// <param name="cameraManager"></param>
        public static void SetCameraManager(ICameraManager cameraManager)
        {
            EffectManagerBase.s_cameraManager = cameraManager;
        }
        /// <summary>
        /// 设置声音管理器
        /// </summary>
        /// <param name="audioManager"></param>
        public static void SetAudioManager(IAudioManager audioManager)
        {
            EffectManagerBase.s_audioManager = audioManager;
        }
        /// <summary>
        /// 设置UIManager
        /// </summary>
        /// <param name="uiManager"></param>
        public static void SetUIManager(IUIManager uiManager)
        {
            EffectManagerBase.s_uiManager = uiManager;
        }
        public static Vector3 WorldToScreenPoint(Vector3 pos)
        {
            Vector3 result;
            if (null == Camera.main.camera)
            {
                result = Vector3.zero;
            }
            else
            {
                result = Camera.main.camera.WorldToScreenPoint(pos);
            }
            return result;
        }
        public static Vector3 UIScreenToWorldPoint(Vector3 pos)
        {
            Vector3 result;
            if (EffectManagerBase.s_uiManager == null || null == EffectManagerBase.s_uiManager.UICamera)
            {
                result = Vector3.zero;
            }
            else
            {
                result = EffectManagerBase.s_uiManager.UICamera.ScreenToWorldPoint(pos);
            }
            return result;
        }
        /// <summary>
        /// 设置摄像机的坐标和方向
        /// </summary>
        /// <param name="vPos"></param>
        /// <param name="vDir"></param>
        public static void SetCameraPosAndDir(Vector3 vPos, Vector3 vDir)
        {
            if (null != EffectManagerBase.s_cameraManager)
            {
                EffectManagerBase.s_cameraManager.SetCamerPosAndDir(vPos, vDir);
            }
        }
        /// <summary>
        /// 设置摄像机是否被摄像机控制
        /// </summary>
        /// <param name="bCtrled"></param>
        public static void SetCameraCtrlByEffect(bool bCtrled)
        {
            if (null != EffectManagerBase.s_cameraManager)
            {
                if (bCtrled)
                {
                    EffectManagerBase.s_cameraManager.BeginCtrlByEffect();
                }
                else 
                {
                    EffectManagerBase.s_cameraManager.EndCtrlByEffect();
                }
            }
        }
        /// <summary>
        /// 设置摄像机的视口
        /// </summary>
        /// <param name="fFov"></param>
        public static void SetCameraFov(float fFov)
        {
            EffectManagerBase.s_cameraManager.SetCameraFov(fFov);
        }
        /// <summary>
        /// 清除所有特效数据
        /// </summary>
        public void ClearEffectData()
        {
            try
            {
                Singleton<EffectManagerImplement>.singleton.ClearEffectData();
            }
            catch (Exception ex)
            {
                EffectLogger.Fatal(ex.ToString());
            }
        }
        #endregion
	    #region 私有方法
#endregion
    }
}