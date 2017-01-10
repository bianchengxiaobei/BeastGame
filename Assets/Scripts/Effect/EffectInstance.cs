using UnityEngine;
using System.Collections;
using UnityAssetEx.Export;
using Effect.Export;
using System;
using Game;
using Utility.Export;
using Client.UI.UICommon;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EffectInstance
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect
{
    internal class EffectInstance
    {
        private const float FloatAccuracy = 0.0001f;
        private const float HeightCorrect = 1.3f;
        private const string BodyBone = "Bip01/Bip01 Pelvis/Bip01 Spine";
        private const string LeftHandBone = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand";
        private const string RightHandBone = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand";
        private const string MainColorName = "_Color";
        private const string TintColorName = "_TintColor";
        private const string BCBonePath = "bcpoint";
        private BezierMoveInfo m_BezierMoveInfo = null;
        private IAssetRequest m_effectAssetRequest = null;
        private IAssetRequest m_audioAssetRequest = null;
        private EffectInstanceData m_Data;
        private GameObject m_Object = null;
        private Material m_Mat = null;
        private Renderer[] m_renderers = null;
        private float m_fAllPowerDefault = 1f;
        private float m_fUVValueX = 0f;
        private float m_fUVValueY = 0f;
        public EffectInstanceData EffectInsCfgData;
        //父亲特效
        public Effect FatherEffect
        {
            get;
            set;
        }
        public Vector3 BornPos
        {
            get;
            set;
        }
        public float BornTime
        {
            get;
            set;
        }
        public bool Dead
        {
            get;
            set;
        }
        public bool Visible
        {
            get;
            set;
        }
        public bool IsInitPosition
        {
            get;
            set;
        }
        /// <summary>
        /// 特效所在的位置
        /// </summary>
        public Vector3 Pos
        {
            get
            {
                Vector3 result;
                if (null != this.m_Object)
                {
                    result = this.m_Object.transform.position;
                }
                else
                {
                    result = Vector3.zero;
                }
                return result;
            }
        }
        /// <summary>
        /// 加载特效实例
        /// </summary>
        /// <param name="data">特效实例数据</param>
        /// <returns></returns>
        public bool Load(EffectInstanceData data)
        {
            bool result;
            if (null == data)
            {
                result = false;
            }
            else
            {
                this.m_Data = data;             
                this.BornTime = Time.time + data.StartDelay;//加载特效出生的时间
                if (null != this.FatherEffect.Caster)
                {
                    Vector3 bornPos;
                    if (this.m_Data.CasterBindType == EffectInstanceBindType.Pos)
                    {
                        bornPos = this.FatherEffect.SourcePos;
                    }
                    else
                    {
                        if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out bornPos))
                        {
                            bornPos = this.FatherEffect.Caster.RealPos3D;
                        }
                    }
                    this.BornPos = bornPos;//加载特效出生位置
                }
                this.Visible = true;
                if (this.m_Data.Type != EffectInstanceType.AddMaterial)
                {
                    this.m_effectAssetRequest = ResourceManager.singleton.LoadEffect(this.m_Data.Path, new AssetRequestFinishedEventHandler(this.LoadEffectHandler), AssetPRI.DownloadPRI_Low);
                }
                else
                {
                    UnityEngine.Object obj = ResourceManager.singleton.Load(this.m_Data.Path);
                    this.LoadMatFinish(obj);
                }
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 获取特效绑定在人物的位置，hand, head，foot....
        /// </summary>
        /// <param name="c"></param>
        /// <param name="type"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool GetBindPos(IBeast c, EffectInstanceBindType type, out Vector3 pos)
        {
            pos = Vector3.zero;
            bool result;
            if (null == c)
            {
                result = false;
            }
            else
            {
                bool flag = false;
                switch (type)
                {
                    case EffectInstanceBindType.Body:
                        if (c.Object != null)
                        {
                            Transform body = c.Body;
                            if (body != null)
                            {
                                pos = body.position;
                                flag = true;
                            }
                        }
                        break;
                    case EffectInstanceBindType.Head:
                        if (c.Object != null)
                        {
                            pos.x = c.MovingPos.x;
                            pos.y = c.MovingPos.y + c.Height * 1.3f;
                            pos.z = c.MovingPos.z;
                            flag = true;
                        }
                        break;
                    case EffectInstanceBindType.Foot:
                        if (c.Object != null)
                        {
                            pos = c.MovingPos;
                            flag = true;
                        }
                        break;
                    case EffectInstanceBindType.LeftHand:
                        if (c.Object != null)
                        {
                            Transform leftHand = c.LeftHand;
                            if (null != leftHand)
                            {
                                pos = leftHand.position;
                                flag = true;
                            }
                        }
                        break;
                    case EffectInstanceBindType.RightHand:
                        if (c.Object != null)
                        {
                            Transform transform = c.RightHand;
                            if (transform != null)
                            {
                                pos = transform.position;
                                flag = true;
                            }
                        }
                        break;
                    case EffectInstanceBindType.LeftWeapon:
                        if (c.Object != null)
                        {
                            Transform transform = c.LeftSpecialTrans;
                            if (transform != null)
                            {
                                pos = transform.position;
                                flag = true;
                            }
                        }
                        break;
                    case EffectInstanceBindType.RightWeapon:
                        if (c.Object != null)
                        {
                            Transform transform = c.RightSpecialTrans;
                            if (transform != null)
                            {
                                pos = transform.position;
                                flag = true;
                            }
                        }
                        break;
                    case EffectInstanceBindType.OtherWeapon:
                        if (c.Object != null)
                        {
                            Transform transform = c.OtherSpecialTrans;
                            if (transform != null)
                            {
                                pos = transform.position;
                                flag = true;
                            }
                        }
                        break;
                }
                result = flag;
            }
            return result;
        }
        private void LoadMatFinish(UnityEngine.Object obj)
        {
            this.m_Mat = (UnityEngine.Object.Instantiate(obj) as Material);
            if (null == this.m_Mat)
            {
                EffectLogger.Error(string.Format("null == m_Mat:{0}", this.m_Data.Path));
            }
            this.Init();
        }
        public void SetLayer(int nLayer)
        {
            if (this.m_Object != null)
            {
                UnityTools.SetLayerRecursively(this.m_Object, nLayer);
            }
        }
        /// <summary>
        /// 加载特效资源回调函数
        /// </summary>
        /// <param name="assetRequest"></param>
        private void LoadEffectHandler(IAssetRequest assetRequest)
        {
            if (assetRequest != null && assetRequest.AssetResource != null)
            {
                UnityEngine.Object mainAsset = assetRequest.AssetResource.MainAsset;
                if (mainAsset != null)
                {
                    Vector3 zero = Vector3.zero;
                    this.GetInitPos(ref zero);
                    //实例化特效
                    this.m_Object = (UnityEngine.Object.Instantiate(mainAsset, zero, Quaternion.Euler(0f, 0f, 0f)) as GameObject);
                    UnityEngine.Object.DontDestroyOnLoad(this.m_Object);
                }
                //如果还有特效的声音，就加载
                if (null != this.m_Object && this.m_Data.Sound != null && this.m_Data.Sound.Length > 0)
                {
                    float volumeEffect = EffectManagerBase.VolumeEffect;
                    if (volumeEffect > 0.01f && EffectManagerBase.EnableEffectAudio)
                    {
                        this.m_audioAssetRequest = ResourceManager.singleton.LoadAudio(this.m_Data.Sound, new AssetRequestFinishedEventHandler(this.LoadSoundHandler), AssetPRI.DownloadPRI_Low);
                    }
                }
                this.Visible = false;
                this.SetVisible(this.Visible);
                if (null != this.m_Object && 0f != this.m_Data.Scale)
                {
                    if (EffectInstanceType.UIStand == this.m_Data.Type)
                    {
                        if (null != this.FatherEffect)
                        {
                            if (this.FatherEffect.CasterUIObject != null)
                            {
                                IXUIObject uIObject = this.FatherEffect.CasterUIObject.UIObject;
                                if (null != uIObject)
                                {
                                    Transform transform = this.m_Object.transform;
                                    transform.parent = uIObject.CachedTransform;
                                    transform.localPosition = Vector3.zero;
                                    transform.localScale = Vector3.one;
                                    transform.parent = EffectManagerBase.UIRoot;
                                    if (transform.localScale.Equals(Vector3.zero))
                                    {
                                        transform.localScale = Vector3.one;
                                    }
                                }
                                else
                                {
                                    EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                }
                            }
                            else
                            {
                                if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                                {
                                    this.m_Object.transform.parent = EffectManagerBase.UIRoot;
                                    this.m_Object.transform.localPosition = Vector3.zero;
                                    this.m_Object.transform.localScale = Vector3.one;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.FatherEffect.HighLight && EffectInstanceType.UITrace != this.m_Data.Type)
                        {
                            int layer = UnityEngine.LayerMask.NameToLayer("ScreenHighLight");
                            this.SetLayer(layer);
                        }
                    }
                    this.SetScale(this.m_Data.Scale);
                }
                this.Init();
                if (this.m_Data.CameraControl)
                {
                    EffectManagerBase.SetCameraCtrlByEffect(true);
                    EffectManagerBase.SetCameraFov(this.m_Data.Fov);
                }
            }
        }
        /// <summary>
        /// 根据特效的类型来初始化出生的位置
        /// </summary>
        /// <param name="vPos"></param>
        public void GetInitPos(ref Vector3 vPos)
        {
            switch (this.m_Data.Type)
            {
                case EffectInstanceType.UIStand:
                    if (null != this.FatherEffect)
                    {
                        if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                        {
                            vPos = this.FatherEffect.TargetPos;
                        }
                    }
                    break;
                case EffectInstanceType.Stand:
                    vPos = this.FatherEffect.TargetPos;
                    break;
                case EffectInstanceType.Follow:
                case EffectInstanceType.Trace:
                    if (this.FatherEffect != null && this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                    {
                        Vector3 vector;
                        if (this.m_Data.CasterBindType == EffectInstanceBindType.Pos)
                        {
                            vector = this.FatherEffect.SourcePos;
                        }
                        else
                        {
                            if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector))
                            {
                                vector = this.FatherEffect.Caster.RealPos3D;
                            }
                        }
                        vPos = vector;
                    }
                    break;
                case EffectInstanceType.BindToCamera:
                    vPos = Camera.main.camera.transform.position;
                    break;
                case EffectInstanceType.FollowTarget:
                    if (this.FatherEffect != null && this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError)
                    {
                        Vector3 vector;
                        if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector))
                        {
                            vector = this.FatherEffect.Caster.RealPos3D;
                        }
                        vPos = vector;
                    }
                    break;
            }
        }
        /// <summary>
        /// 加载特效音效之后的回调（主要是播放）
        /// </summary>
        /// <param name="assetRequest"></param>
        private void LoadSoundHandler(IAssetRequest assetRequest)
        {
            IAssetResource assetResource = assetRequest.AssetResource;
            if (null != assetResource)
            {
                UnityEngine.Object mainAsset = assetResource.MainAsset;
                if (mainAsset != null)
                {
                    AudioClip audioClip = mainAsset as AudioClip;
                    if (null != audioClip && null != this.m_Object)
                    {
                        AudioSource audioSource = this.m_Object.AddComponent<AudioSource>();
                        if (null != audioSource)
                        {
                            float volumeEffect = EffectManagerBase.VolumeEffect;
                            audioSource.clip = audioClip;
                            audioSource.volume = volumeEffect;
                            audioSource.Play();
                        }
                    }
                }
            }
        }
        private void Init()
        {
            if (!(null == this.m_Object) || this.m_Data.Type == EffectInstanceType.AddMaterial)
            {
                switch (this.m_Data.Type)
                {
                    case EffectInstanceType.UIStand://如果是停留在UI上
                        if (null != this.FatherEffect)
                        {
                            if (this.FatherEffect.CasterUIObject != null && this.FatherEffect.CasterUIObject.UIObject != null)
                            {
                                if (this.m_Object.transform.position != this.FatherEffect.CasterUIObject.UIObject.CachedTransform.position)
                                {
                                    //粒子特效就初始化在UI的位置
                                    this.m_Object.transform.position = this.FatherEffect.CasterUIObject.UIObject.CachedTransform.position;
                                }
                            }
                        }
                        break;
                    case EffectInstanceType.UITrace://如果是跟踪UI，初始化源坐标
                        if (EffectInstanceType.UITrace == this.m_Data.Type)
                        {
                            this.m_Object.transform.parent = EffectManagerBase.UIRoot;
                            this.m_Object.transform.localScale = Vector3.one;
                            Vector3 vector = Vector3.one;
                            if (null != this.FatherEffect)
                            {
                                if (null != this.FatherEffect.CasterUIObject)//如果源UI物体存在
                                {
                                    IXUIObject uIObject = this.FatherEffect.CasterUIObject.UIObject;
                                    if (null != uIObject)
                                    {
                                        vector = uIObject.CachedTransform.position;
                                    }
                                    else
                                    {
                                        vector = Vector3.one;
                                        EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                    }
                                }
                                else//如果不存在源ui物体，就去找人物，获取坐标
                                {
                                    if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector))
                                    {
                                        vector = Vector3.one;
                                    }
                                    else
                                    {
                                        vector = EffectManagerBase.WorldToScreenPoint(vector);
                                        vector = EffectManagerBase.UIScreenToWorldPoint(vector);
                                    }
                                }
                            }
                            this.FatherEffect.SourcePos = vector;
                            this.m_Object.transform.localPosition = vector;
                        }
                        break;
                    case EffectInstanceType.UIRandomTrace:
                        if (this.FatherEffect != null && !this.IsInitPosition)
                        {
                            Vector3 vector2 = Vector3.one;
                            if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                            {
                                if (EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector2))
                                {
                                    vector2 = EffectManagerBase.WorldToScreenPoint(vector2);
                                    vector2 = EffectManagerBase.UIScreenToWorldPoint(vector2);
                                    vector2.z = 0f;
                                }
                            }
                            else
                            {
                                if (this.FatherEffect.CasterUIObject != null && null != this.FatherEffect.CasterUIObject.UIObject)
                                {
                                    vector2 = this.FatherEffect.CasterUIObject.UIObject.CachedTransform.position;
                                }
                                else
                                {
                                    vector2 = this.FatherEffect.SourcePos;
                                }
                            }
                            this.m_Object.transform.position = vector2;
                            if (this.FatherEffect.TargetUIObject != null && null != this.FatherEffect.TargetUIObject.UIObject)
                            {
                                IEffectSetting effectSetting = this.m_Object.transform.GetComponent("EffectSettings") as IEffectSetting;
                                if (null != effectSetting)
                                {
                                    effectSetting.Init(this.FatherEffect.TargetUIObject.UIObject.CachedGameObject);
                                }
                                this.IsInitPosition = true;
                            }
                            else
                            {
                                EffectLogger.Error("oEffectSet.TargetUIObject is null");
                                this.Destroy();
                            }
                        }
                        break;
                    case EffectInstanceType.Stand:
                        if (this.m_Object.transform.position != this.FatherEffect.TargetPos)
                        {
                            this.m_Object.transform.position = this.FatherEffect.TargetPos;
                        }
                        if (this.m_Data.FollowLeagueDir)
                        {
                            Vector3 forward = EffectManagerBase.LeagueDir;
                            if (forward.magnitude != 0f)
                            {
                                this.m_Object.transform.forward = forward;
                            }
                        }
                        if (this.m_Data.FollowEmpireDir)
                        {
                            Vector3 forward = EffectManagerBase.EmpireDir;
                            if (forward.magnitude != 0f)
                            {
                                this.m_Object.transform.forward = forward;
                            }
                        }
                        break;
                    case EffectInstanceType.Follow:
                    case EffectInstanceType.Trace:
                        if (this.FatherEffect != null && this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                        {
                            Vector3 vector;
                            if (this.m_Data.CasterBindType == EffectInstanceBindType.Pos)
                            {
                                vector = this.FatherEffect.SourcePos;
                            }
                            else
                            {
                                if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector))
                                {
                                    vector = this.FatherEffect.Caster.RealPos3D;
                                }
                                if (this.m_Data.FollowDirection)
                                {
                                    if (this.m_Data.FollowDirection && null != this.FatherEffect.Caster.Object)
                                    {
                                        if (this.FatherEffect.Caster.Object.transform.forward.magnitude != 0f)
                                        {
                                            this.m_Object.transform.forward = this.FatherEffect.Caster.Object.transform.forward;
                                        }
                                    }
                                }
                                if (this.m_Data.FollowLeagueDir)
                                {
                                    Vector3 forward = EffectManagerBase.LeagueDir;
                                    if (forward.magnitude != 0f)
                                    {
                                        this.m_Object.transform.forward = forward;
                                    }
                                }
                                if (this.m_Data.FollowEmpireDir)
                                {
                                    Vector3 forward = EffectManagerBase.EmpireDir;
                                    if (forward.magnitude != 0f)
                                    {
                                        this.m_Object.transform.forward = forward;
                                    }
                                }
                            }
                            this.m_Object.transform.position = vector;
                        }
                        break;
                    case EffectInstanceType.BindToCamera:
                        this.m_Object.transform.position = Camera.mainCamera.transform.position;
                        break;
                    case EffectInstanceType.FollowTarget:
                        if (this.FatherEffect != null && this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError)
                        {
                            Vector3 vector;
                            if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector))
                            {
                                vector = this.FatherEffect.Caster.RealPos3D;
                            }
                            this.m_Object.transform.position = vector;
                        }
                        break;
                    case EffectInstanceType.RandomTrace:
                        if (this.FatherEffect != null && !this.IsInitPosition)
                        {
                            if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                            {
                                Vector3 one = Vector3.one;
                                EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out one);
                                this.m_Object.transform.position = one;
                                if (this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError && null != this.FatherEffect.Target.Body)
                                {
                                    IEffectSetting effectSetting = this.m_Object.transform.GetComponent("EffectSettings") as IEffectSetting;
                                    if (null != effectSetting)
                                    {
                                        effectSetting.Init(this.FatherEffect.Target.Body.gameObject);
                                    }
                                    this.IsInitPosition = true;
                                }
                                else
                                {
                                    EffectLogger.Error("oEffectSet.Target is null");
                                    this.Destroy();
                                }
                            }
                            else
                            {
                                EffectLogger.Error("oEffectSet.Caster is null");
                                this.Destroy();
                            }
                        }
                        break;
                    case EffectInstanceType.AddMaterial:
                        if (this.FatherEffect != null && !this.IsInitPosition)
                        {
                            if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                            {
                                if (null != this.m_Mat)
                                {
                                    this.FatherEffect.Caster.AddMaterial(this.m_Mat);
                                }
                            }
                        }
                        this.IsInitPosition = true;
                        break;
                    case EffectInstanceType.RopeEffect:
                        if (this.FatherEffect != null && !this.IsInitPosition)
                        {
                            if (null != this.FatherEffect.Caster)
                            {
                                Vector3 vector;
                                if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector))
                                {
                                    vector = this.FatherEffect.Caster.RealPos3D;
                                }
                                this.m_Object.transform.position = vector;
                            }
                            if (null != this.FatherEffect.Target)
                            {
                                Transform bindTransform = EffectInstance.GetBindTransform(this.FatherEffect.Target, this.m_Data.TargetBindType);
                                IEffectLayerRopeSet effectLayerRopeSet = this.m_Object.GetComponent("EffectLayerRopeSet") as IEffectLayerRopeSet;
                                if (null != effectLayerRopeSet)
                                {
                                    effectLayerRopeSet.SetEffectLayerTarget(bindTransform);
                                }
                                this.IsInitPosition = true;
                            }
                        }
                        break;
                }
            }
        }
        private void UpdatePosition()
        {
            if (this.FatherEffect.m_nEffecTypeId == 1000099)
            {
                int num = 1;
                num++;
            }
            if (!(null == this.m_Object) && null != this.m_Data)
            {
                switch (this.m_Data.Type)
                {
                    case EffectInstanceType.UIStand:
                        if (null != this.FatherEffect)
                        {
                            if (this.FatherEffect.CasterUIObject != null && this.FatherEffect.CasterUIObject.UIObject != null)
                            {
                                if (this.m_Object.transform.position != this.FatherEffect.CasterUIObject.UIObject.CachedTransform.position)
                                {
                                    this.m_Object.transform.position = this.FatherEffect.CasterUIObject.UIObject.CachedTransform.position;
                                }
                            }
                        }
                        break;
                    case EffectInstanceType.UITrace:
                        switch (this.m_Data.InstanceTraceType)
                        {
                            case TraceType.Line:
                                {
                                    Vector3 sourcePos = this.FatherEffect.SourcePos;
                                    Vector3 vector = this.FatherEffect.TargetPos;
                                    float num2 = 0f;
                                    if (null != this.FatherEffect)
                                    {
                                        if (null != this.FatherEffect.CasterUIObject)
                                        {
                                            IXUIObject uIObject = this.FatherEffect.CasterUIObject.UIObject;
                                            if (null != uIObject)
                                            {
                                                vector = uIObject.CachedTransform.position;
                                            }
                                            else
                                            {
                                                EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                            }
                                        }
                                        if (null != this.FatherEffect.TargetUIObject)
                                        {
                                            IXUIObject uIObject = this.FatherEffect.TargetUIObject.UIObject;
                                            if (null != uIObject)
                                            {
                                                vector = uIObject.CachedTransform.position;
                                            }
                                            else
                                            {
                                                EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                            }
                                        }
                                    }
                                    if (TraceMoveType.FixMoveSpeed == this.m_Data.InstanceTraceMoveType)
                                    {
                                        if (this.m_Data.MoveSpeed != 0f)
                                        {
                                            num2 = Vector3.Magnitude(vector - sourcePos) / this.m_Data.MoveSpeed;
                                        }
                                    }
                                    else
                                    {
                                        num2 = this.m_Data.TraceTime;
                                    }
                                    sourcePos.z = 0f;
                                    vector.z = 0f;
                                    Vector3 vector2 = vector - sourcePos;
                                    float num3 = Time.time - this.BornTime;
                                    Vector3 position = sourcePos + vector2 * ((num3 >= num2) ? 1f : (num3 / num2));
                                    this.m_Object.transform.position = position;
                                    if (num3 >= num2)
                                    {
                                        this.Dead = true;
                                        this.SetVisible(false);
                                    }
                                    break;
                                }
                            case TraceType.Bezier:
                                {
                                    Vector3 sourcePos = this.FatherEffect.SourcePos;
                                    Vector3 vector = this.FatherEffect.TargetPos;
                                    float num2 = 0f;
                                    if (null != this.FatherEffect)
                                    {
                                        if (null != this.FatherEffect.CasterUIObject)
                                        {
                                            IXUIObject uIObject = this.FatherEffect.CasterUIObject.UIObject;
                                            if (null != uIObject)
                                            {
                                                vector = uIObject.CachedTransform.position;
                                            }
                                            else
                                            {
                                                EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                            }
                                        }
                                        if (null != this.FatherEffect.TargetUIObject)
                                        {
                                            IXUIObject uIObject = this.FatherEffect.TargetUIObject.UIObject;
                                            if (null != uIObject)
                                            {
                                                vector = uIObject.CachedTransform.position;
                                            }
                                            else
                                            {
                                                EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                            }
                                        }
                                    }
                                    sourcePos.z = vector.z;
                                    if (TraceMoveType.FixMoveSpeed == this.m_Data.InstanceTraceMoveType)
                                    {
                                        if (this.m_Data.MoveSpeed != 0f)
                                        {
                                            num2 = Vector3.Magnitude(vector - this.BornPos) / this.m_Data.MoveSpeed;
                                        }
                                    }
                                    else
                                    {
                                        num2 = this.m_Data.TraceTime;
                                    }
                                    float num3 = Time.time - this.BornTime;
                                    Vector3 position2 = this.m_Object.transform.position;
                                    if (this.m_BezierMoveInfo == null)
                                    {
                                        this.m_BezierMoveInfo = this.m_Data.mBezierControl.GetMoveControl(sourcePos, vector);
                                    }
                                    if (num3 > num2)
                                    {
                                        this.Dead = true;
                                        this.SetVisible(false);
                                    }
                                    else
                                    {
                                        this.m_Object.transform.position = this.m_BezierMoveInfo.GetNextPos(num3 / num2);
                                    }
                                    break;
                                }
                            case TraceType.Gravity:
                                {
                                    Vector3 sourcePos = this.FatherEffect.SourcePos;
                                    Vector3 vector = this.FatherEffect.TargetPos;
                                    float num2 = 0f;
                                    if (null != this.FatherEffect)
                                    {
                                        if (null != this.FatherEffect.CasterUIObject)
                                        {
                                            IXUIObject uIObject = this.FatherEffect.CasterUIObject.UIObject;
                                            if (null != uIObject)
                                            {
                                                vector = uIObject.CachedTransform.position;
                                            }
                                            else
                                            {
                                                EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                            }
                                        }
                                        if (null != this.FatherEffect.TargetUIObject)
                                        {
                                            IXUIObject uIObject = this.FatherEffect.TargetUIObject.UIObject;
                                            if (null != uIObject)
                                            {
                                                vector = uIObject.CachedTransform.position;
                                            }
                                            else
                                            {
                                                EffectLogger.Error(string.Format("null == uiObject:{0}", this.FatherEffect.CasterUIObject));
                                            }
                                        }
                                    }
                                    if (TraceMoveType.FixMoveSpeed == this.m_Data.InstanceTraceMoveType)
                                    {
                                        if (this.m_Data.MoveSpeed != 0f)
                                        {
                                            num2 = Vector3.Magnitude(vector - sourcePos) / this.m_Data.MoveSpeed;
                                        }
                                    }
                                    else
                                    {
                                        num2 = this.m_Data.TraceTime;
                                    }
                                    sourcePos.z = 0f;
                                    vector.z = 0f;
                                    float y = sourcePos.y;
                                    float y2 = vector.y;
                                    float num4 = Mathf.Max(y, y2) + this.m_Data.GravityHeight;
                                    float num5 = 4f * (y2 + y - 2f * num4) / (num2 * num2);
                                    float num6 = (4f * num4 - y2 - 3f * y) / num2;
                                    float num3 = Time.time - this.BornTime;
                                    float y3 = (num3 <= num2) ? (y + num6 * num3 + 0.5f * num5 * num3 * num3) : y2;
                                    Vector3 vector2 = vector - sourcePos;
                                    Vector3 vector3 = sourcePos + vector2 * ((num3 >= num2) ? 1f : (num3 / num2));
                                    vector3.y = y3;
                                    this.m_Object.transform.position = vector3;
                                    if (num3 >= num2)
                                    {
                                        this.Dead = true;
                                        this.SetVisible(false);
                                    }
                                    break;
                                }
                        }
                        break;
                    case EffectInstanceType.Stand:
                        if (this.m_Object.transform.position != this.FatherEffect.TargetPos)
                        {
                            this.m_Object.transform.position = this.FatherEffect.TargetPos;
                        }
                        if (this.m_Data.FollowLeagueDir)
                        {
                            Vector3 forward = EffectManagerBase.LeagueDir;
                            if (forward.magnitude != 0f)
                            {
                                this.m_Object.transform.forward = forward;
                            }
                        }
                        if (this.m_Data.FollowEmpireDir)
                        {
                            Vector3 forward = EffectManagerBase.EmpireDir;
                            if (forward.magnitude != 0f)
                            {
                                this.m_Object.transform.forward = forward;
                            }
                        }
                        if (this.m_Data.FixDir && this.FatherEffect.FixDir.magnitude != 0f)
                        {
                            this.m_Object.transform.forward = this.FatherEffect.FixDir;
                        }
                        break;
                    case EffectInstanceType.Follow:
                        if (this.FatherEffect != null && this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                        {
                            Vector3 vector4;
                            if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector4))
                            {
                                this.m_Object.transform.position = this.FatherEffect.Caster.RealPos3D;
                            }
                            else
                            {
                                this.m_Object.transform.position = vector4;
                            }
                            if (this.m_Data.FollowDirection)
                            {
                                if (this.m_Data.FollowDirection && null != this.FatherEffect.Caster.Object)
                                {
                                    if (this.FatherEffect.Caster.Object.transform.forward.magnitude != 0f)
                                    {
                                        this.m_Object.transform.forward = this.FatherEffect.Caster.Object.transform.forward;
                                    }
                                }
                            }
                            if (this.m_Data.FollowLeagueDir)
                            {
                                Vector3 forward = EffectManagerBase.LeagueDir;
                                if (forward.magnitude != 0f)
                                {
                                    this.m_Object.transform.forward = forward;
                                }
                            }
                            if (this.m_Data.FollowEmpireDir)
                            {
                                Vector3 forward = EffectManagerBase.EmpireDir;
                                if (forward.magnitude != 0f)
                                {
                                    this.m_Object.transform.forward = forward;
                                }
                            }
                            if (this.m_Data.CameraControl)
                            {
                                Transform child = this.m_Object.transform.GetChild(0);
                                if (child != null)
                                {
                                    child = child.GetChild(0);
                                    if (child != null)
                                    {
                                        EffectManagerBase.SetCameraPosAndDir(child.position, child.forward);
                                    }
                                }
                            }
                        }
                        break;
                    case EffectInstanceType.Trace:
                        if (null != this.FatherEffect)
                        {
                            switch (this.m_Data.InstanceTraceType)
                            {
                                case TraceType.Line:
                                    {
                                        Vector3 vector;
                                        if (this.m_Data.TargetBindType == EffectInstanceBindType.Pos)
                                        {
                                            vector = this.FatherEffect.TargetPos;
                                        }
                                        else
                                        {
                                            if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector))
                                            {
                                                if (this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError)
                                                {
                                                    vector = this.FatherEffect.Target.RealPos3D;
                                                }
                                                else
                                                {
                                                    vector = this.FatherEffect.TargetPos;
                                                }
                                            }
                                        }
                                        float num2 = 0f;
                                        if (TraceMoveType.FixMoveSpeed == this.m_Data.InstanceTraceMoveType)
                                        {
                                            if (this.m_Data.MoveSpeed != 0f)
                                            {
                                                num2 = Vector3.Magnitude(vector - this.BornPos) / this.m_Data.MoveSpeed;
                                            }
                                        }
                                        else
                                        {
                                            num2 = this.m_Data.TraceTime;
                                        }
                                        Vector3 vector2 = vector - this.BornPos;
                                        float num3 = Time.time - this.BornTime;
                                        Vector3 b = (num3 < this.m_Data.PathDelay) ? Vector3.one : (vector2 * ((num3 >= num2) ? 1f : (num3 / num2)));
                                        Vector3 position = this.BornPos + b;
                                        this.m_Object.transform.position = position;
                                        if (vector2.magnitude != 0f)
                                        {
                                            this.m_Object.transform.forward = vector2;
                                        }
                                        if (num3 >= num2)
                                        {
                                            if (num3 >= num2 + this.m_Data.TraceDelay)
                                            {
                                                this.Dead = true;
                                                this.SetVisible(false, true);
                                            }
                                            else
                                            {
                                                this.SetVisible(false);
                                            }
                                        }
                                        break;
                                    }
                                case TraceType.Bezier:
                                    {
                                        Vector3 vector;
                                        if (this.m_Data.TargetBindType == EffectInstanceBindType.Pos)
                                        {
                                            vector = this.FatherEffect.TargetPos;
                                        }
                                        else
                                        {
                                            if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector))
                                            {
                                                if (this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError)
                                                {
                                                    vector = this.FatherEffect.Target.RealPos3D;
                                                }
                                                else
                                                {
                                                    vector = this.FatherEffect.TargetPos;
                                                }
                                            }
                                        }
                                        float num2 = 0f;
                                        if (TraceMoveType.FixMoveSpeed == this.m_Data.InstanceTraceMoveType)
                                        {
                                            if (this.m_Data.MoveSpeed != 0f)
                                            {
                                                num2 = Vector3.Magnitude(vector - this.BornPos) / this.m_Data.MoveSpeed;
                                            }
                                        }
                                        else
                                        {
                                            num2 = this.m_Data.TraceTime;
                                        }
                                        float num3 = Time.time - this.BornTime;
                                        Vector3 position2 = this.m_Object.transform.position;
                                        if (this.m_BezierMoveInfo == null)
                                        {
                                            this.m_BezierMoveInfo = this.m_Data.mBezierControl.GetMoveControl(this.BornPos, vector);
                                        }
                                        if (num3 > num2)
                                        {
                                            if (num3 >= num2 + this.m_Data.TraceDelay)
                                            {
                                                this.Dead = true;
                                                this.SetVisible(false, true);
                                            }
                                            else
                                            {
                                                this.SetVisible(false);
                                            }
                                        }
                                        else
                                        {
                                            this.m_Object.transform.position = ((num3 < this.m_Data.PathDelay) ? this.BornPos : this.m_BezierMoveInfo.GetNextPos(num3 / num2));
                                            this.m_Object.transform.forward = this.m_Object.transform.position - position2;
                                        }
                                        break;
                                    }
                                case TraceType.Gravity:
                                    {
                                        Vector3 vector;
                                        if (this.m_Data.TargetBindType == EffectInstanceBindType.Pos)
                                        {
                                            if (this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError)
                                            {
                                                vector = this.FatherEffect.Target.RealPos3D;
                                            }
                                            else
                                            {
                                                vector = this.FatherEffect.TargetPos;
                                            }
                                        }
                                        else
                                        {
                                            if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector))
                                            {
                                                vector = this.FatherEffect.Target.RealPos3D;
                                            }
                                        }
                                        float num2;
                                        if (TraceMoveType.FixMoveSpeed == this.m_Data.InstanceTraceMoveType)
                                        {
                                            if (this.m_Data.MoveSpeed != 0f)
                                            {
                                                float num7 = Vector3.Distance(vector, this.BornPos);
                                                num2 = num7 / this.m_Data.MoveSpeed;
                                            }
                                            else
                                            {
                                                num2 = 1f;
                                            }
                                        }
                                        else
                                        {
                                            if (this.m_Data.TraceTime != 0f)
                                            {
                                                num2 = this.m_Data.TraceTime;
                                            }
                                            else
                                            {
                                                num2 = 1f;
                                            }
                                        }
                                        float y = this.BornPos.y;
                                        float y2 = vector.y;
                                        float num4 = this.m_Data.GravityHeight;
                                        float num5 = 4f * (y2 + y - 2f * num4) / (num2 * num2);
                                        float num6 = (4f * num4 - y2 - 3f * y) / num2;
                                        float num3 = Time.time - this.BornTime;
                                        float y3 = (num3 <= num2) ? (y + num6 * num3 + 0.5f * num5 * num3 * num3) : y2;
                                        Vector3 vector2 = vector - this.BornPos;
                                        Vector3 b = (num3 < this.m_Data.PathDelay) ? Vector3.one : (vector2 * ((num3 >= num2) ? 1f : (num3 / num2)));
                                        Vector3 vector3 = this.BornPos + b;
                                        vector3.y = y3;
                                        Vector3 forward = vector3 - this.m_Object.transform.position;
                                        this.m_Object.transform.position = vector3;
                                        if (forward.magnitude != 0f)
                                        {
                                            this.m_Object.transform.forward = forward;
                                        }
                                        if (num3 >= num2)
                                        {
                                            if (num3 >= num2 + this.m_Data.TraceDelay)
                                            {
                                                this.Dead = true;
                                                this.SetVisible(false, true);
                                            }
                                            else
                                            {
                                                this.SetVisible(false);
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    case EffectInstanceType.SpaceLink:
                        {
                            Vector3 vector4;
                            if (!EffectInstance.GetBindPos(this.FatherEffect.Caster, this.m_Data.CasterBindType, out vector4))
                            {
                                if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                                {
                                    this.m_Object.transform.position = this.FatherEffect.Caster.RealPos3D;
                                }
                                else
                                {
                                    this.m_Object.transform.position = this.FatherEffect.SourcePos;
                                }
                            }
                            Vector3 vector;
                            if (EffectInstanceBindType.Pos == this.m_Data.TargetBindType)
                            {
                                vector = this.FatherEffect.TargetPos;
                            }
                            else
                            {
                                if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector))
                                {
                                    if (this.FatherEffect.Target != null && !this.FatherEffect.Target.IsError)
                                    {
                                        vector = this.FatherEffect.Target.RealPos3D;
                                    }
                                }
                            }
                            this.m_Object.transform.position = vector4;
                            float num7 = Vector3.Magnitude(vector - vector4);
                            this.m_Object.transform.localScale = new Vector3(1f, 1f, num7);
                            this.m_Object.transform.forward = Vector3.Normalize(vector - vector4);
                            break;
                        }
                    case EffectInstanceType.BindToCamera:
                        this.m_Object.transform.position = Camera.mainCamera.transform.position;
                        break;
                    case EffectInstanceType.FollowTarget:
                        if (this.FatherEffect != null && this.FatherEffect.Target != null && !this.FatherEffect.Caster.IsError)
                        {
                            Vector3 vector4;
                            if (!EffectInstance.GetBindPos(this.FatherEffect.Target, this.m_Data.TargetBindType, out vector4))
                            {
                                this.m_Object.transform.position = this.FatherEffect.Caster.RealPos3D;
                            }
                            else
                            {
                                this.m_Object.transform.position = vector4;
                            }
                            if (this.m_Data.FollowDirection)
                            {
                                if (this.m_Data.FollowDirection && null != this.FatherEffect.Target.Object)
                                {
                                    if (this.FatherEffect.Caster.Object.transform.forward.magnitude != 0f)
                                    {
                                        this.m_Object.transform.forward = this.FatherEffect.Target.Object.transform.forward;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
        public void Update()
        {
            try
            {
                if (this.FatherEffect != null && (this.FatherEffect.Caster == null || this.FatherEffect.Caster.IsModelVisible))
                {
                    if (this.m_Data.Life > 0f && Time.time - this.BornTime >= this.m_Data.Life)
                    {
                        this.Dead = true;
                        this.Destroy();
                    }
                    else
                    {
                        if (!this.Visible && Time.time - this.BornTime >= 0f)
                        {
                            this.Visible = true;
                            this.Init();
                            this.SetVisible(this.Visible);
                        }
                        if (this.Visible && !this.Dead)
                        {
                            float num = Time.time - this.BornTime;
                            if (this.m_Data.Life > 0f && this.m_Data.FadeInTime > 0f && num <= this.m_Data.FadeInTime)
                            {
                                float transparent = num / this.m_Data.FadeInTime;
                                this.SetTransparent(transparent);
                            }
                            if (this.m_Data.Life > 0f && this.m_Data.FadeOutTime > 0f && Time.time - this.BornTime >= this.m_Data.Life - this.m_Data.FadeOutTime)
                            {
                                float num2 = this.m_Data.Life - (Time.time - this.BornTime);
                                float transparent = num2 / this.m_Data.FadeOutTime;
                                this.SetTransparent(transparent);
                            }
                            if (this.m_Data.Type != EffectInstanceType.AddMaterial)
                            {
                                this.UpdatePosition();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EffectLogger.Fatal(ex.ToString());
            }
        }
        public static Transform GetBindTransform(IBeast c, EffectInstanceBindType type)
        {
            Transform transform = null;
            Transform result;
            if (c == null && null == c.Object)
            {
                result = transform;
            }
            else
            {
                switch (type)
                {
                    case EffectInstanceBindType.Body:
                        transform = c.Body;
                        break;
                    case EffectInstanceBindType.LeftHand:
                        transform = c.LeftHand;
                        break;
                    case EffectInstanceBindType.RightHand:
                        transform = c.RightHand;
                        break;
                    case EffectInstanceBindType.LeftWeapon:
                        transform = c.LeftSpecialTrans;
                        break;
                    case EffectInstanceBindType.RightWeapon:
                        transform = c.RightSpecialTrans;
                        break;
                    case EffectInstanceBindType.OtherWeapon:
                        transform = c.OtherSpecialTrans;
                        break;
                }
                result = transform;
            }
            return result;
        }
        public void Destroy()
        {
            if (this.m_Data.CameraControl)
            {
                EffectManagerBase.SetCameraCtrlByEffect(false);
            }
            if (this.m_Object != null)
            {
                UnityEngine.Object.Destroy(this.m_Object);
                this.m_Object = null;
            }
            if (this.m_Mat != null)
            {
                if (this.IsInitPosition)
                {
                    if (this.FatherEffect.Caster != null && !this.FatherEffect.Caster.IsError)
                    {
                        this.FatherEffect.Caster.DelMaterial(this.m_Mat);
                    }
                }
                UnityEngine.Object.Destroy(this.m_Mat);
                this.m_Mat = null;
            }
            if (null != this.m_effectAssetRequest)
            {
                this.m_effectAssetRequest.Dispose();
                this.m_effectAssetRequest = null;
            }
            if (null != this.m_audioAssetRequest)
            {
                this.m_audioAssetRequest.Dispose();
                this.m_audioAssetRequest = null;
            }
        }
        public void SetScale(float scale)
        {
            if (!(null == this.m_Object))
            {
                if (scale > 0f)
                {
                    this.m_Object.transform.localScale *= scale;
                    TrailRenderer[] componentsInChildren = this.m_Object.GetComponentsInChildren<TrailRenderer>(true);
                    TrailRenderer[] array = componentsInChildren;
                    for (int i = 0; i < array.Length; i++)
                    {
                        TrailRenderer trailRenderer = array[i];
                        if (null != trailRenderer)
                        {
                            trailRenderer.startWidth *= scale;
                            trailRenderer.endWidth *= scale;
                        }
                    }
                    for (int j = 0; j < this.m_Object.transform.childCount; j++)
                    {
                        Transform child = this.m_Object.transform.GetChild(j);
                        if (null != child && child.gameObject != null)
                        {
                            this.SetParticleSystemScale(child.gameObject, scale);
                        }
                    }
                }
            }
        }
        private void SetParticleSystemScale(GameObject gameObject, float scale)
        {
            if (!(null == gameObject))
            {
                if (scale > 0f)
                {
                    if (gameObject.particleSystem != null)
                    {
                        gameObject.particleSystem.startSize *= scale;
                        gameObject.particleSystem.startSpeed *= scale;
                        gameObject.transform.localScale *= scale;
                    }
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        Transform child = gameObject.transform.GetChild(i);
                        if (null != child && child.gameObject != null)
                        {
                            this.SetParticleSystemScale(child.gameObject, scale);
                        }
                    }
                }
            }
        }
        public void SetVisible(bool visible)
        {
            this.SetVisible(visible, false);
        }
        public void SetVisible(bool visible, bool bContainIgnoreNode)
        {
            if (!(null == this.m_Object))
            {
                if (this.FatherEffect.EffectCfgData.Id == 3022)
                {
                }
                IIgnoreNode ignoreNode = this.m_Object.GetComponent("IgnoreNode") as IIgnoreNode;
                bool flag = ignoreNode == null || bContainIgnoreNode;
                if (this.m_Object.audio != null && flag)
                {
                    if (visible)
                    {
                        this.m_Object.audio.Play();
                    }
                    else
                    {
                        this.m_Object.audio.Stop();
                    }
                }
                if (this.m_Object.renderer != null && flag)
                {
                    this.m_Object.renderer.enabled = visible;
                }
                if (this.m_Object.particleSystem != null && flag)
                {
                    if (visible)
                    {
                        this.m_Object.particleSystem.Play();
                    }
                    else
                    {
                        this.m_Object.particleSystem.Stop();
                    }
                }
                if (this.m_Object.animation != null && flag)
                {
                    if (visible)
                    {
                        this.m_Object.animation.Play();
                    }
                    else
                    {
                        this.m_Object.animation.Stop();
                    }
                }
                for (int i = 0; i < this.m_Object.transform.childCount; i++)
                {
                    Transform child = this.m_Object.transform.GetChild(i);
                    if (child != null && child.gameObject != null)
                    {
                        this.SetEffectSystemVisible(child.gameObject, visible, bContainIgnoreNode);
                    }
                }
            }
        }
        private void SetEffectSystemVisible(GameObject gameObject, bool visible, bool bContainIgnoreNode)
        {
            if (!(null == gameObject))
            {
                IIgnoreNode ignoreNode = gameObject.GetComponent("IgnoreNode") as IIgnoreNode;
                bool flag = ignoreNode == null || bContainIgnoreNode;
                if (gameObject.renderer != null && flag)
                {
                    gameObject.renderer.enabled = visible;
                }
                if (gameObject.particleSystem != null && flag)
                {
                    if (visible)
                    {
                        gameObject.particleSystem.Play();
                    }
                    else
                    {
                        gameObject.particleSystem.Stop();
                    }
                }
                if (gameObject.animation != null && flag)
                {
                    if (visible)
                    {
                        gameObject.animation.Play();
                    }
                    else
                    {
                        gameObject.animation.Stop();
                    }
                }
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    Transform child = gameObject.transform.GetChild(i);
                    if (child != null && child.gameObject != null)
                    {
                        this.SetEffectSystemVisible(child.gameObject, visible, bContainIgnoreNode);
                    }
                }
            }
        }
        public void SetTransparent(float transparent)
        {
            if (transparent >= 0f && transparent <= 1f)
            {
                if (this.m_renderers == null)
                {
                    if (null != this.m_Object)
                    {
                        this.m_renderers = this.m_Object.GetComponentsInChildren<Renderer>(true);
                    }
                }
                if (null != this.m_renderers)
                {
                    Renderer[] renderers = this.m_renderers;
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        Renderer renderer = renderers[i];
                        if (!(renderer == null))
                        {
                            Material[] materials = renderer.materials;
                            for (int j = 0; j < materials.Length; j++)
                            {
                                Material material = materials[j];
                                if (material.HasProperty("_Color"))
                                {
                                    Vector4 vector = material.GetVector("_Color");
                                    vector.w *= transparent;
                                    material.SetVector("_Color", vector);
                                }
                                if (material.HasProperty("_TintColor"))
                                {
                                    Vector4 vector = material.GetVector("_TintColor");
                                    vector.w *= transparent;
                                    material.SetVector("_TintColor", vector);
                                }
                            }
                        }
                    }
                }
            }
        }
        public void SetColor(Color color)
        {
            if (!(this.m_Object == null))
            {
                UnityTools.SetColor(this.m_Object, color);
            }
        }
    }
}

