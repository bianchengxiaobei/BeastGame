using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EffectData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect
{
    internal class EffectData
    {
        #region 字段
        private List<EffectInstanceData> m_InstanceDatas = new List<EffectInstanceData>();
        private CameraShakeData m_CameraShakeData;
        #endregion
        #region 属性
        public float HitPointTime
        {
            get;
            set;
        }
        public List<EffectInstanceData> InstanceDatas
        {
            get 
            {
                return this.m_InstanceDatas;
            }
        }
        public CameraShakeData CameraShake
        {
            get 
            {
                return this.m_CameraShakeData;
            }
        }
        public int Id
        {
            get;
            set;
        }
        public float Life
        {
            get;
            set;
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public bool Load(XmlNode effectNode)
        {
            this.m_InstanceDatas.Clear();
            this.m_CameraShakeData = null;
            bool result;
            if (null == effectNode)
            {
                result = false;
            }
            else
            {
                foreach (XmlNode xmlNode in effectNode.ChildNodes)
                {
                    string text = xmlNode.Name.ToLower();
                    if (text != null)
                    {
                        if (!(text == "id"))
                        {
                            if (!(text == "life"))
                            {
                                if (!(text == "hitpoint"))
                                {
                                    if (!(text == "instance"))
                                    {
                                        if (text == "camerashake")
                                        {
                                            this.m_CameraShakeData = new CameraShakeData();
                                            if (!this.m_CameraShakeData.Load(xmlNode))
                                            {
                                                this.m_CameraShakeData = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        EffectInstanceData effectInstanceData = new EffectInstanceData();
                                        if (effectInstanceData.Load(xmlNode))
                                        {
                                            this.m_InstanceDatas.Add(effectInstanceData);
                                        }
                                    }
                                }
                                else
                                {
                                    this.HitPointTime = (float)Convert.ToDouble(xmlNode.InnerText);
                                }
                            }
                            else
                            {
                                this.Life = (float)Convert.ToDouble(xmlNode.InnerText);
                            }
                        }
                        else
                        {
                            this.Id = Convert.ToInt32(xmlNode.InnerText);
                        }
                    }
                }
                if (null != this.m_CameraShakeData)
                {
                    if (this.m_CameraShakeData.Type == CameraShakeData.CameraShakeType.Animation)
                    {
                        if (null != this.m_CameraShakeData.ShakeObjectPath)
                        {
                            UnityEngine.Object obj = ResourceManager.singleton.Load(this.m_CameraShakeData.ShakeObjectPath);
                            this.OnShakeAnimObjLoaded(obj);
                        }
                    }
                }
                result = true;
            }
            return result;
        }
        public void OnShakeAnimObjLoaded(UnityEngine.Object obj)
        {
            this.m_CameraShakeData.AnimObj = (obj as GameObject);
        }
        #endregion
        #region 私有方法
        #endregion
    }
}

