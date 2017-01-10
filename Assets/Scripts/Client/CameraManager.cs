using UnityEngine;
using System.Collections;
using Utility.Export;
using Game;
using Client.UI;
using Utility;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CameraManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.17
// 模块描述：摄像机管理器
//----------------------------------------------------------------*/
#endregion
namespace Client
{
    public class CameraManager
    {
        #region 字段
        /// <summary>
        /// 私有内部类，重置摄像机信息
        /// </summary>
        private class CameraResetInfo
        {
            public Vector3 vLookAtPos = Vector3.zero;
            public float fDistance = 0f;
            public float fScale = 1f;
            public float fXDelta = 0f;
            public float fYDelta = 0f;
            public void Reset()
            {
                this.vLookAtPos = Vector3.zero;
                this.fDistance = 0f;
                this.fScale = 1f;
                this.fXDelta = 0f;
                this.fYDelta = 0f;
            }
        }
        private bool m_bCtrlByEffect = false;
        private Camera m_EffectHighLightCamera = null;
        private Vector3 m_lookAtPos;
        private GUITexture m_SceneTexture = null;
        //所有摄像机的根节点
        private Transform m_GameNode = null;
        private bool m_bFollowHeroInRound = false;
        public bool CameraMoveEffect = false;
        private int m_blurRefCount = 0;
        public bool IsOverLook = false;
        private long m_nForceFollowHeroId = -1;
        private CameraManager.CameraResetInfo resetInfo = new CameraManager.CameraResetInfo();
        private float m_fDistance = 1f;
        private float m_fScale = 1f;
        private Vector3 m_InitCameraEulerAngle;
        private float m_fDeltaXAngle = 0f;
        private float m_fDeltaYAngle = 0f;
        private Vector3 m_vRight = Vector3.right;
        private Vector3 m_vForward = Vector3.forward;
        private Vector3 m_vTargetTranslatingPos = Vector3.zero;
        private float m_fTranslatingSpeed = 0f;
        private bool m_Lock = false;
        private bool m_bSHow = false;
        private bool m_bFreeLockInFollow = false;
        private IXLog m_log = XLog.GetLog<CameraManager>();
        #endregion
        #region 属性
        /// <summary>
        /// 摄像机是否追随主角
        /// </summary>
        public bool FollowHeroInRound
        {
            get
            {
                return this.m_bFollowHeroInRound;
            }
            set
            {
                this.m_bFollowHeroInRound = value;
                this.m_nForceFollowHeroId = -1;
                if (this.m_bFollowHeroInRound)
                {
                    this.resetInfo.vLookAtPos = this.LookAtPos;
                    this.resetInfo.fDistance = this.Distance;
                    this.resetInfo.fScale = this.Scale;
                    this.resetInfo.fXDelta = this.m_fDeltaXAngle;
                    this.resetInfo.fYDelta = this.m_fDeltaYAngle;
                    this.m_fDistance = this.Distance;
                    this.m_fScale = this.Scale;
                    /*uint heroIdInRound = Singleton<RoomManager>.singleton.HeroIdInRound;
                    Hero heroById = Singleton<HeroManager>.singleton.GetHeroById(heroIdInRound);
                    if (heroById != HeroManager.ErrorHero)
                    {
                        this.m_lookAtPos = heroById.MovingPos;
                        this.m_lookAtPos.y = this.m_lookAtPos.y + GameConfig.singleton.FollowHeightOffset;
                    }
                     * */
                }
            }
        }
        /// <summary>
        /// 所有摄像机的根节点
        /// </summary>
        public Transform GameNode
        {
            get
            {
                return this.m_GameNode;
            }
        }
        /// <summary>
        /// 摄像机看的目标位置
        /// </summary>
        public Vector3 LookAtPos
        {
            get
            {
                return this.m_lookAtPos;
            }
            private set
            {
                this.m_lookAtPos = value;
            }
        }
        public float Distance
        {
            get
            {
                return this.m_fDistance;
            }
        }
        public float Scale
        {
            get
            {
                return this.m_fScale;
            }
            set
            {
                this.m_fScale = value;
            }
        }
        public float MouseWheelSensitivity
        {
            get;
            set;
        }
        public float MinScale
        {
            get;
            set;
        }
        public float MaxScale
        {
            get;set;
        }
        public float MaxMapX
        {
            get;
            set;
        }
        public float MinMapX
        {
            get;
            set;
        }
        public float MaxMapZ
        {
            get;
            set;
        }
        public float MinMapZ
        {
            get;
            set;
        }
        public bool FPP
        {
            get;
            set;
        }
        public float FPPDistance
        {
            get;
            set;
        }
        public bool IsTranslating
        {
            get;
            set;
        }
        public bool Lock
        {
            get
            {
                return this.m_Lock;
            }
            set
            {
                this.m_Lock = value;
            }
        }
        #endregion
        #region 构造方法
        public static CameraManager Instance = new CameraManager();
        #endregion
        #region 公有方法
        /// <summary>
        /// 设置摄像机和高亮特效摄像机的fieldOfView
        /// </summary>
        /// <param name="fFov"></param>
        public void SetCameraFov(float fFov)
        {
            if (Camera.main != null)
            {
                Camera.main.fieldOfView = fFov;
            }
            if (this.m_EffectHighLightCamera != null)
            {
                this.m_EffectHighLightCamera.fieldOfView = fFov;
            }
        }
        /// <summary>
        /// 特效开始控制
        /// </summary>
        public void BeginCtrlByEffect()
        {
            this.m_bCtrlByEffect = true;
        }
        /// <summary>
        /// 结束控制特效
        /// </summary>
        public void EndCtrlByEffect()
        {
            this.m_bCtrlByEffect = false;
            this.SetCameraFov(30f);
        }

        public void ForceFollowHero(int nHeroId)
        {
            if (this.m_bFollowHeroInRound)
            {
                this.m_nForceFollowHeroId = nHeroId;
                if (this.m_nForceFollowHeroId == 0)
                {
                    this.InitCameraLookAt();
                    this.ResetRotation();
                }
                if (this.m_nForceFollowHeroId == -1)
                {
                    this.OnReSelectSkill();
                    this.Reset();
                }
            }
        }
        public void OnReSelectSkill()
        {
            CSceneMgr.singleton.ReviveResetCamera();
        }
        public void Reset()
        {
            this.CameraMoveEffect = false;
            this.Lock = false;
            this.m_fScale = 1f;
            this.m_fDeltaXAngle = 0f;
            this.m_fDeltaYAngle = 0f;
            this.m_nForceFollowHeroId = -1;
            this.m_bCtrlByEffect = false;
        }
        public void SetCamerPosAndLookAt(Vector3 cameraPos, Vector3 lookAtPos)
        {
            this.m_lookAtPos = lookAtPos;
            this.m_fDistance = Vector3.Magnitude(cameraPos - lookAtPos);
            this.Scale = 1f;
        }
        public void SetCamerPosAndLookAt(Vector3 lookAtPos, float fdist, float fScale)
        {
            this.m_lookAtPos = lookAtPos;
            this.m_fDistance = fdist;
            this.Scale = fScale;
        }
        public void TranslateTo(float x, float y, float z, float fSpeed)
        {
            this.m_vTargetTranslatingPos = new Vector3(x, y, z);
            this.m_fTranslatingSpeed = fSpeed;
            this.IsTranslating = true;

        }
        public void TranslateTo(float x, float y, float z)
        {
            this.TranslateTo(x, y, z, GameConfig.singleton.CameAutoMoveSpeed);
        }
        public void Translate(float MapX, float MapZ)
        {
            if (!this.FPP && !this.m_Lock && this.m_nForceFollowHeroId < 0 && !this.IsTranslating)
            {
                if (!this.FollowHeroInRound || this.m_bFreeLockInFollow) 
                {
                    if (!this.m_bCtrlByEffect)
                    {
                        Quaternion rotation = Quaternion.Euler(this.m_InitCameraEulerAngle.x + this.m_fDeltaXAngle, this.m_InitCameraEulerAngle.y + this.m_fDeltaYAngle, this.m_InitCameraEulerAngle.z);
                        Vector3 vector = rotation * Vector3.right;
                        Vector3 up = Vector3.up;
                        Vector3 a = Vector3.Cross(vector, up);
                        Vector3 vDelta = MapX * vector + MapZ * a;
                        this.MoveLookAt(vDelta);
                    }
                }
 
            }
        }
        public void Roll(float deltaScale)
        {
            if (!this.Lock)
            {
                if (!UIManager.singleton.IsAnyTouchInUI)
                {
                    if (!this.m_bCtrlByEffect)
                    {
                        this.m_fScale -= deltaScale;
                        if (this.m_fScale > 1f)
                        {
                            this.m_fScale = 1f;

                        }
                        if (this.m_fScale < this.MinScale)
                        {
                            this.m_fScale = this.MinScale;
                        }
                    }
                }
            }
        }
        public void Roate(float xAngle, float yAngle)
        {
            if (!this.Lock)
            {
                this.m_fDeltaXAngle += xAngle;
                this.m_fDeltaYAngle += yAngle;
            }
        }
        /// <summary>
        /// 初始化摄像机根节点
        /// </summary>
        public void InitGameNode()
        {
            if (null == this.m_GameNode)
            {
                this.m_GameNode = Camera.main.transform.parent;
            }
        }
        public void DefaultInit()
        {
            if (null == this.m_GameNode)
            {
                this.m_GameNode = Camera.main.transform.parent;
            }
            
        }
        public void Init()
        {
            if (null == this.m_GameNode)
            {
                this.m_GameNode = Camera.main.transform.parent;
            }
            if (this.m_GameNode != null)
            {
                Transform transform = this.m_GameNode.FindChild("EffectHighlightCamera");
                if (transform != null)
                {
                    this.m_EffectHighLightCamera = transform.camera;
                }
            }
            if (this.m_GameNode != null && this.m_GameNode.parent != null)
            {
                Transform transform = this.m_GameNode.parent.FindChild("ScreenBlur");
                if (transform != null)
                {
                    this.m_SceneTexture = transform.gameObject.GetComponent<GUITexture>();
                }
            }
            this.InitCameraPosDir();
            this.Reset();
        }
        public void BeginScreenBlur()
        { 
        }
        public void EndScreenBlur()
        {

        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 初始化摄像机的LookAtPos坐标
        /// </summary>
        private void InitCameraLookAt()
        {
            Vector3 forward = new Vector3(0f, 0f, 1f);
            this.m_InitCameraEulerAngle = this.m_GameNode.localEulerAngles;
            forward = this.m_GameNode.forward;
            if (forward.y != 0f)
            {
                float d = -this.m_GameNode.position.y / forward.y;
                this.m_lookAtPos = this.m_GameNode.position + d * forward;
            }
            else
            {
                this.m_lookAtPos = this.m_GameNode.position;
                this.m_lookAtPos.z = 0f;
            }
        }
        /// <summary>
        /// 重新设置摄像机的旋转
        /// </summary>
        private void ResetRotation()
        {
            this.m_fDeltaXAngle = 0f;
            this.m_fDeltaYAngle = 0f;
        }
        /// <summary>
        /// 重新设置缩放为1f
        /// </summary>
        private void ResetScale()
        {
            this.m_fScale = 1f;
        }
        /// <summary>
        /// 移动摄像机
        /// </summary>
        /// <param name="vDelta">移动的距离</param>
        private void MoveLookAt(Vector3 vDelta)
        {
            if (GameConfig.singleton.RestrictMove)//如果限制移动范围的话
            {
                this.m_lookAtPos += vDelta;//vDelta移动的小段距离
                if (this.m_lookAtPos.x + vDelta.x > this.MaxMapX)
                {
                    this.m_lookAtPos.x = this.MaxMapX;
                }
                if (this.m_lookAtPos.x + vDelta.x < MinMapX)
                {
                    this.m_lookAtPos.x = this.MinMapX;
                }
                if (this.m_lookAtPos.z + vDelta.z > this.MaxMapX)
                {
                    this.m_lookAtPos.z = this.MaxMapZ;
                }
                if (this.m_lookAtPos.z + vDelta.z < this.MinMapZ)
                {
                    this.m_lookAtPos.z = this.MinMapZ;
                }
                //主要是lookAtPos的y轴的值不会改变，所以移动的时候x和z会变
            }
            else 
            {

            }
        }
        private void InitCameraPosDir()
        {
            Vector3 forward = new Vector3(0f, 0f, 1f);
            this.m_InitCameraEulerAngle = this.m_GameNode.localEulerAngles;
            forward = this.m_GameNode.forward;
            if (forward.y != 0f)
            {
                float a = -this.m_GameNode.position.y / forward.y;
                this.m_lookAtPos = this.m_GameNode.position;
            }
            else 
            {
                this.m_lookAtPos = this.m_GameNode.position;
                this.m_lookAtPos.z = 0f;
            }
            this.m_fDistance = Vector3.Magnitude(this.m_GameNode.position - this.m_lookAtPos);
        }
        private void OnUpdate()
        {
            if (!this.m_bCtrlByEffect)
            {
                if (null != this.m_GameNode)
                {
                    if (!this.IsTranslating && this.IsFollowing() && !this.CameraMoveEffect)//如果没有移动，且正在跟随，没有移动特效
                    {
                        long num = 0;
                        if (this.m_nForceFollowHeroId < 0)
                        {
                            num = Singleton<RoomManager>.singleton.BeastIdInRound;
                        }
                        else 
                        {
                            if (this.m_nForceFollowHeroId > 0)
                            {
                                num = this.m_nForceFollowHeroId;
                            }
                        }
                        
                    }
                }
            }
        }
        private bool IsFollowing()
        {
            return this.m_nForceFollowHeroId > 0 || this.FollowHeroInRound;
        }
        #endregion
    }
    public interface ICameraManager
    {
        void SetCamerPosAndDir(Vector3 vPos, Vector3 vDir);
        void BeginCtrlByEffect();
        void EndCtrlByEffect();
        void SetCameraFov(float fFov);
    }
}
