using UnityEngine;
using System.Collections.Generic;
using Game;
using UILib.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Effect
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect
{
    internal class Effect
    {
        /// <summary>
        /// 位置类型，Player，Pos
        /// </summary>
        public enum PosType 
        {
            Pos,Player
        }
        #region 字段
        public Camera targetCamera = null;
        public Vector3 FixDir = Vector3.zero;
        public Effect.PosType eForceTargetType = PosType.Player;
        public Effect.PosType eForceCasterType = PosType.Player;
        public bool HighLight = false;
        public int m_nEffecTypeId = 0;
        private List<EffectInstance> m_EffectInstances;
        private EffectData m_Data;
        private bool m_Loaded = false;
        private float m_StartTime;
        private bool m_Shaked = false;
        private IBeast m_heroTarget = null;
        private IBeast m_heroCast = null;
        #endregion
        #region 属性
        public List<EffectInstance> EffectInstances
        {
            get
            {
                return this.m_EffectInstances;
            }
        }
        public EffectData EffectCfgData
        {
            get
            {
                return this.m_Data;
            }
        }
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 释放特效的物体
        /// </summary>
        public IBeast Caster
        {
            get
            {
                return this.m_heroCast;
            }
            set
            {
                this.m_heroCast = value;
            }
        }
        /// <summary>
        /// 特效施法的目标物体
        /// </summary>
        public IBeast Target
        {
            get
            {
                return this.m_heroTarget;
            }
            set
            {
                this.m_heroTarget = value;
            }
        }
        public SafeXUIObject CasterUIObject
        {
            get;
            set;
        }
        public SafeXUIObject TargetUIObject
        {
            get;
            set;
        }
        public Vector3 TargetPos
        {
            get;
            set;
        }
        public Vector3 SourcePos
        {
            get;
            set;
        }
        public bool Dead
        {
            get;
            set;
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public void Update()
        {
 
        }
        /// <summary>
        /// 设置初始位置
        /// </summary>
        /// <param name="pos"></param>
        public void SetSrcPos(Vector3 pos)
        {
            this.SourcePos = pos;
            foreach (EffectInstance current in this.EffectInstances)
            {
                current.BornPos = pos;
            }
        }
        public void SetHightLayer()
        {
            int layer = UnityEngine.LayerMask.NameToLayer("Hightlight");
            this.SetLayer(layer);
        }
        public void SetEffectLayer()
        {
            int layer = UnityEngine.LayerMask.NameToLayer("Effect");
            this.SetLayer(layer);
        }
        public void SetLayer(int layer)
        {
            foreach (EffectInstance current in this.m_EffectInstances)
            {
                current.SetLayer(layer);
            }
        }
        public bool Load(EffectData data)
        {
            if (this.m_EffectInstances == null)
            {
                this.m_EffectInstances = new List<EffectInstance>();
            }
            else
            {
                this.m_EffectInstances.Clear();
            }
            bool result;
            if (null == data)
            {
                result = false;
            }
            else
            {
                this.m_Data = data;
                this.SourcePos = new Vector3(0f, 0f, 0f);
                this.TargetPos = new Vector3(0f, 0f, 0f);
                foreach (EffectInstanceData current in this.m_Data.InstanceDatas)
                {
                    EffectInstance effectInstance = new EffectInstance();
                    effectInstance.FatherEffect = this;
                    if (effectInstance.Load(current))
                    {
                        this.m_EffectInstances.Add(effectInstance);
                    }
                }
                this.m_Loaded = true;
                this.m_StartTime = Time.time;
                result = true;
            }
            return result;
        }
        public void SetScale(float scale)
        {
            if (scale > 0f)
            {
                foreach (EffectInstance current in this.m_EffectInstances)
                {
                    current.SetScale(scale);
                }
            }
        }
        public void SetColor(Color color)
        {
            for (int i = 0; i < this.m_EffectInstances.Count; i++)
            {
                EffectInstance effectInstance = this.m_EffectInstances[i];
                effectInstance.SetColor(color);
            }
        }
        public void SetVisible(bool bVisible)
        {
            for (int i = 0; i < this.m_EffectInstances.Count; i++)
            {
                EffectInstance effectInstance = this.m_EffectInstances[i];
                if (null != effectInstance)
                {
                    effectInstance.SetVisible(bVisible);
                }
            }
        }
        public void Destroy()
        {
            foreach (EffectInstance current in this.m_EffectInstances)
            {
                current.Destroy();
            }
            this.m_EffectInstances.Clear();
        }
        #endregion
        #region 私有方法
        #endregion
    }
    
}