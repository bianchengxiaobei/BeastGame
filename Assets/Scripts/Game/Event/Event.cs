using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Event
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public static class Event
    {
        /// <summary>
        ///实体音效事件类
        /// </summary>
        public static class LogicSoundEvent
        {
            public readonly static string OnHitYelling = "LogicSoundEvent.OnHitYelling";
            public readonly static string LogicPlaySoundByClip = "LogicSoundEvent.LogicPlaySoundByClip";
        }
        public static class FrameWorkEvent
        {
            public readonly static string EntityAttached = "FrameWorkEvent.EntityAttached";
        }
    }
}
