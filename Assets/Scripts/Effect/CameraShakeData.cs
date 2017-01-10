using UnityEngine;
using System.Collections;
using System.Xml;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CameraShakeData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect{
public class CameraShakeData
	{
		public enum PositionType
		{
			Caster,
			Target,
			SrcPos,
			DestPos
		}
		public enum CameraShakeType
		{
			NoShake,
			Normal,
			Horizontal,
			Vertical,
			Animation
		}
		private float m_ForcedDelay = 0f;
		private string m_ShakeAnimPath = "";
		private GameObject m_Obj = null;
		public float ForcedDelay
		{
			get
			{
				return this.m_ForcedDelay;
			}
			set
			{
				this.m_ForcedDelay = value;
			}
		}
		public CameraShakeData.PositionType PosType
		{
			get;
			set;
		}
		public CameraShakeData.CameraShakeType Type
		{
			get;
			set;
		}
		public float Life
		{
			get;
			set;
		}
		public float StartDelay
		{
			get;
			set;
		}
		public float MaxRange
		{
			get;
			set;
		}
		public float MinRange
		{
			get;
			set;
		}
		public float MaxAmplitude
		{
			get;
			set;
		}
		public float MinAmplitude
		{
			get;
			set;
		}
		public float AmplitudeAttenuation
		{
			get;
			set;
		}
		public float Frequency
		{
			get;
			set;
		}
		public float FrequencyKeepDuration
		{
			get;
			set;
		}
		public float FrequencyAttenuation
		{
			get;
			set;
		}
		public string ShakeObjectPath
		{
			get
			{
				return this.m_ShakeAnimPath;
			}
			set
			{
				this.m_ShakeAnimPath = value;
			}
		}
		public GameObject AnimObj
		{
			get
			{
				return this.m_Obj;
			}
			set
			{
				this.m_Obj = value;
			}
		}
		public void OnAnimLoad(GameObject obj)
		{
			this.AnimObj = obj;
		}
		public void Release()
		{
		}
		public bool Load(XmlNode CameraShakeNode)
		{
			bool result;
			if (null == CameraShakeNode)
			{
				result = false;
			}
			else
			{
				foreach (XmlNode xmlNode in CameraShakeNode.ChildNodes)
				{
					string text = xmlNode.Name.ToLower();
					switch (text)
					{
					case "positiontype":
						text = xmlNode.InnerText.ToLower();
						if (text != null)
						{
							if (!(text == "srcpos"))
							{
								if (!(text == "destpos"))
								{
									if (!(text == "caster"))
									{
										if (text == "target")
										{
											this.PosType = CameraShakeData.PositionType.Target;
										}
									}
									else
									{
										this.PosType = CameraShakeData.PositionType.Caster;
									}
								}
								else
								{
									this.PosType = CameraShakeData.PositionType.DestPos;
								}
							}
							else
							{
								this.PosType = CameraShakeData.PositionType.SrcPos;
							}
						}
						break;
					case "type":
						text = xmlNode.InnerText.ToLower();
						if (text != null)
						{
							if (!(text == "anim"))
							{
								if (!(text == "noshake"))
								{
									if (!(text == "normal"))
									{
										if (!(text == "horizontal"))
										{
											if (text == "vertical")
											{
												this.Type = CameraShakeData.CameraShakeType.Vertical;
											}
										}
										else
										{
											this.Type = CameraShakeData.CameraShakeType.Horizontal;
										}
									}
									else
									{
										this.Type = CameraShakeData.CameraShakeType.Normal;
									}
								}
								else
								{
									this.Type = CameraShakeData.CameraShakeType.NoShake;
								}
							}
							else
							{
								this.Type = CameraShakeData.CameraShakeType.Animation;
							}
						}
						break;
					case "animpath":
						this.ShakeObjectPath = xmlNode.InnerText;
						break;
					case "startdelay":
						this.StartDelay = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "life":
						this.Life = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "maxrange":
						this.MaxRange = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "minrange":
						this.MinRange = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "maxamplitude":
						this.MaxAmplitude = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "minamplitude":
						this.MinAmplitude = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "amplitudeattenuation":
						this.AmplitudeAttenuation = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "frequency":
						this.Frequency = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "frequencykeepduration":
						this.FrequencyKeepDuration = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					case "frequencyattenuation":
						this.FrequencyAttenuation = (float)Convert.ToDouble(xmlNode.InnerText);
						break;
					}
				}
				result = true;
			}
			return result;
		}
	}
}

