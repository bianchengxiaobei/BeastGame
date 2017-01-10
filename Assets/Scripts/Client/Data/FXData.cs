using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：FXData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class FXData : GameData<FXData>
    {
        public static readonly string fileName = "xml/FXData";
        public string player { get; set; }
        public FXStatic isStatic { get; set; }
        public FXConflict isConflict { get; set; }
        public EffectType effectType{ get; set;}
        public FXLocationType locationType { get; set; }
        public int level { get; set; }
        public Vector3 location { get; set; }
        public string resourcePath { get; set; }
        public float duration { get; set; }
        /// <summary>
        /// 特效所在的组别
        /// </summary>
        public int group { get; set; }
        /// <summary>
        /// 特效过了多少秒之后开始出现
        /// </summary>
        public float fadeDelay { get; set; }
        /// <summary>
        /// 特效延迟持续的时间
        /// </summary>
        public float fadeDuration { get; set; }
        /// <summary>
        /// 开始淡入特效的alpha值
        /// </summary>
        public float fadeStart { get; set; }
        /// <summary>
        /// 结束特效的alpha值
        /// </summary>
        public float fadeEnd { get; set; }
        /// <summary>
        /// 特效动画
        /// </summary>
        public string anim { get; set; }
    }
    /// <summary>
    /// 是否要保留
    /// </summary>
    public enum FXStatic : byte 
    {
        NotStatic = 0,//不保留
        Static = 1//保留
    }
    /// <summary>
    /// 是否与其他特效冲突
    /// </summary>
    public enum FXConflict : byte
    {
        NotConflict = 0,
        Conflict = 1
    }
    /// <summary>
    /// 特效类型0=>普通，1=>飞行
    /// </summary>
    public enum EffectType : byte
    {
        Normal = 0,
        Flying = 1
    }
    /// <summary>
    /// 特效坐标相对位置类型
    /// </summary>
    public enum FXLocationType : byte
    {
        World=0,
        SelfLocal = 1,
        SelfSlot = 2,
        SelfWorld,
        SlotWorld
    }
}
