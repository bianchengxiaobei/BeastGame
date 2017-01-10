using UnityEngine;
using System.Xml;
using System;
using Effect.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EffectInstanceData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Effect
{
    internal class EffectInstanceData
    {
        #region 字段
        public BezierControl mBezierControl = new BezierControl();
        public float Fov = 60f;
        #endregion
        #region 属性
        public EffectInstanceType Type
        {
            get;
            set;
        }
        public EffectInstanceBindType CasterBindType
        {
            get;
            set;
        }
        public EffectInstanceBindType TargetBindType
        {
            get;
            set;
        }
        public float Life 
        { get; set; }
        /// <summary>
        /// 特效路径
        /// </summary>
        public string Path
        {
            get;
            set;
        }
        /// <summary>
        /// 声音资源路径
        /// </summary>
        public string Sound
        {
            get;
            set;
        }
        public float Scale
        {
            get;
            set;
        }
        public float StartDelay
        {
            get;
            set;
        }
        public float TraceDelay
        {
            get;
            set;
        }
        public float PathDelay
        {
            get;
            set;
        }
        public float FadeInTime
        {
            get;
            set;
        }
        public float FadeOutTime
        {
            get;
            set;
        }
        public float UVSpeedX
        {
            get;
            set;
        }
        public float UVSpeedY
        {
            get;
            set;
        }
        public bool FollowDirection
        {
            get;
            set;
        }
        public bool FollowLeagueDir
        {
            get;
            set;
        }
        public bool FollowEmpireDir
        {
            get;
            set;
        }
        public bool CameraControl
        {
            get;
            set;
        }
        public float CameraControlDelay
        {
            get;
            set;
        }
        public bool FixDir
        {
            get;
            set;
        }
        public float TraceTime
        {
            get;
            set;
        }
        public float MoveSpeed
        {
            get;
            set;
        }
        public float GravityHeight
        {
            get;
            set;
        }
        public int RandTraceCount
        {
            get;
            set;
        }
        public TraceType InstanceTraceType
        {
            get;
            set;
        }
        public TraceMoveType InstanceTraceMoveType
        {
            get;
            set;
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public bool Load(XmlNode node)
		{
			bool result;
			if (null == node)
			{
				result = false;
			}
			else
			{
				try
				{
					foreach (XmlNode xmlNode in node.ChildNodes)
					{
						string text = xmlNode.Name.ToLower();
						switch (text)
						{
						case "beziercontrol":
							this.mBezierControl.Load(xmlNode);
							break;
						case "type":
							text = xmlNode.InnerText.ToLower();
							switch (text)
							{
							case "uistand":
								this.Type = EffectInstanceType.UIStand;
								break;
							case "uirandomtrace":
								this.Type = EffectInstanceType.UIRandomTrace;
								break;
							case "randomtrace":
								this.Type = EffectInstanceType.RandomTrace;
								break;
							case "addmaterial":
								this.Type = EffectInstanceType.AddMaterial;
								break;
							case "uitrace":
								this.Type = EffectInstanceType.UITrace;
								break;
							case "stand":
								this.Type = EffectInstanceType.Stand;
								break;
							case "follow":
								this.Type = EffectInstanceType.Follow;
								break;
							case "followtarget":
								this.Type = EffectInstanceType.FollowTarget;
								break;
							case "trace":
								this.Type = EffectInstanceType.Trace;
								break;
							case "spacelink":
								this.Type = EffectInstanceType.SpaceLink;
								break;
							case "bindtocamera":
								this.Type = EffectInstanceType.BindToCamera;
								break;
							case "ropeeffect":
								this.Type = EffectInstanceType.RopeEffect;
								break;
							}
							break;
						case "life":
							this.Life = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "fov":
							this.Fov = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "cameracontrol":
							this.CameraControl = (Convert.ToInt32(xmlNode.InnerText) == 1);
							break;
						case "cameracontroldelay":
							this.CameraControlDelay = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "path":
							this.Path = xmlNode.InnerText;
							break;
						case "sound":
							this.Sound = xmlNode.InnerText;
							break;
						case "scale":
							this.Scale = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "startdelay":
							this.StartDelay = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "tracedelay":
							this.TraceDelay = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "pathdelay":
							this.PathDelay = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "fadeintime":
							this.FadeInTime = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "fadeouttime":
							this.FadeOutTime = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "uvspeedx":
							this.UVSpeedX = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "uvspeedy":
							this.UVSpeedY = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "followdirection":
						{
							int num2 = Convert.ToInt32(xmlNode.InnerText);
							if (num2 == 1)
							{
								this.FollowDirection = true;
							}
							if (num2 == 2)
							{
								this.FollowLeagueDir = true;
							}
							if (num2 == 3)
							{
								this.FollowEmpireDir = true;
							}
							if (num2 == 4)
							{
								this.FixDir = true;
							}
							break;
						}
						case "tracetime":
							this.TraceTime = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "tracetype":
							text = xmlNode.InnerText.ToLower();
							if (text == null)
							{
								goto IL_630;
							}
							if (!(text == "bezier"))
							{
								if (!(text == "line"))
								{
									if (!(text == "gravity"))
									{
										goto IL_630;
									}
									this.InstanceTraceType = TraceType.Gravity;
								}
								else
								{
									this.InstanceTraceType = TraceType.Line;
								}
							}
							else
							{
								this.InstanceTraceType = TraceType.Bezier;
							}
							break;
							IL_630:
							this.InstanceTraceType = TraceType.Line;
							break;
						case "gravityheight":
							this.GravityHeight = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "tracemovetype":
							if ("fixspeed" == xmlNode.InnerText.ToLower())
							{
								this.InstanceTraceMoveType = TraceMoveType.FixMoveSpeed;
							}
							else
							{
								this.InstanceTraceMoveType = TraceMoveType.FixMoveTime;
							}
							break;
						case "movespeed":
							this.MoveSpeed = (float)Convert.ToDouble(xmlNode.InnerText);
							break;
						case "casterbindtype":
							text = xmlNode.InnerText.ToLower();
							switch (text)
							{
							case "head":
								this.CasterBindType = EffectInstanceBindType.Head;
								break;
							case "body":
								this.CasterBindType = EffectInstanceBindType.Body;
								break;
							case "foot":
								this.CasterBindType = EffectInstanceBindType.Foot;
								break;
							case "lefthand":
								this.CasterBindType = EffectInstanceBindType.LeftHand;
								break;
							case "righthand":
								this.CasterBindType = EffectInstanceBindType.RightHand;
								break;
							case "pos":
								this.CasterBindType = EffectInstanceBindType.Pos;
								break;
							case "leftspecial":
								this.CasterBindType = EffectInstanceBindType.LeftWeapon;
								break;
							case "rightspecial":
								this.CasterBindType = EffectInstanceBindType.RightWeapon;
								break;
							case "bcpoint":
								this.CasterBindType = EffectInstanceBindType.OtherWeapon;
								break;
							}
							break;
						case "targetbindtype":
							text = xmlNode.InnerText.ToLower();
							switch (text)
							{
							case "head":
								this.TargetBindType = EffectInstanceBindType.Head;
								break;
							case "body":
								this.TargetBindType = EffectInstanceBindType.Body;
								break;
							case "foot":
								this.TargetBindType = EffectInstanceBindType.Foot;
								break;
							case "lefthand":
								this.TargetBindType = EffectInstanceBindType.LeftHand;
								break;
							case "righthand":
								this.TargetBindType = EffectInstanceBindType.RightHand;
								break;
							case "pos":
								this.TargetBindType = EffectInstanceBindType.Pos;
								break;
							case "leftspecial":
								this.TargetBindType = EffectInstanceBindType.LeftWeapon;
								break;
							case "rightspecial":
								this.TargetBindType = EffectInstanceBindType.RightWeapon;
								break;
							case "bcpoint":
								this.TargetBindType = EffectInstanceBindType.OtherWeapon;
								break;
							}
							break;
						case "randtracecount":
							this.RandTraceCount = Convert.ToInt32(xmlNode.InnerText);
							break;
						}
					}
				}
				catch (Exception ex)
				{
					EffectLogger.Error(ex.ToString());
				}
				result = true;
			}
			return result;
        }
        #endregion
        #region 私有方法
        #endregion
    }
        
    /// <summary>
    /// 特效实例的类型
    /// </summary>
    public enum EffectInstanceType
    {
        UIStand,
        UITrace,
        UIRandomTrace,
        Stand,
        Follow,
        Trace,
        SpaceLink,
        BindToCamera,
        FollowTarget,
        RandomTrace,
        AddMaterial,
        RopeEffect
    }
    /// <summary>
    /// 特效绑定位置类型
    /// </summary>
    public enum EffectInstanceBindType
    {
        Body,
        Head,
        Foot,
        LeftHand,
        RightHand,
        Pos,
        LeftWeapon,
        RightWeapon,
        OtherWeapon
    }
    public enum TraceType
    {
        Line,
        Bezier,
        Gravity
    }
    public enum TraceMoveType
    {
        FixMoveTime,
        FixMoveSpeed
    }
}
