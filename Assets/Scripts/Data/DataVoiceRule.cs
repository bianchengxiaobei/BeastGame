using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataVoiceRule
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.11
// 模块描述：神兽声音规则数据
//----------------------------------------------------------------*/
#endregion
namespace GameData
{
    /// <summary>
    /// 神兽声音规则数据
    /// </summary>
    public class DataVoiceRule : GameData<DataVoiceRule>
    {
        public static string filename = "dataVoiceRule";
        public int ID
        {
            get;
            set;
        }
        public string RuleType
        {
            get;
            set;
        }
        public int PlayRate
        {
            get;
            set;
        }
        public int PlayWhenDead
        {
            get;
            set;
        }
        public int PlayWhenDizz
        {
            get;
            set;
        }
        public int PlayType
        {
            get;
            set;
        }
        public int InterruptOther
        {
            get;
            set;
        }
        public int CanBeInterrupted
        {
            get;
            set;
        }
        public int HasBubble
        {
            get;
            set;
        }
    }
}