using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActionSoundData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class ActionSoundData : GameData<ActionSoundData>
    {
        /// <summary>
        /// 职业
        /// </summary>
        public int vocation { get; set; }
        /// <summary>
        /// 动画id
        /// </summary>
        public int action { get; set; }
        /// <summary>
        /// 音效 key=>对应SoundData的id，value应该主要来随机播放的概率比例
        /// </summary>
        public Dictionary<int, int> sound { get; set; }
        public static readonly string fileName = "ActionSound";
    }
}