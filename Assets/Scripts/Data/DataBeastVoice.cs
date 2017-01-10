using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataHeroVoice
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.11
// 模块描述：神兽声音数据
//----------------------------------------------------------------*/
#endregion
namespace GameData
{
    /// <summary>
    /// 神兽声音数据
    /// </summary>
    public class DataBeastVoice : GameData<DataBeastVoice>
    {
        public static string filename = "dataBeastVoice";
        public int ID
        {
            get;
            set;
        }
        public int BeastId
        {
            get;
            set;
        }
        public string VoiceDir
        {
            get;
            set;
        }
        public string BornVoice
        {
            get;
            set;
        }
        public string BornBubble
        {
            get;
            set;
        }
        public string ReviveVoice
        {
            get;
            set;
        }
        public string ReviveBubble
        {
            get;
            set;
        }
        public string RoundStartVoice
        {
            get;
            set;
        }
        public string RoundStartBubble
        {
            get;
            set;
        }
        public string WaitInRoundVoice
        {
            get;
            set;
        }
        public string WaitInRoundBubble
        {
            get;
            set;
        }
        public string WaitOutRoundVoice
        {
            get;
            set;
        }
        public string WaitOutRoundBubble
        {
            get;
            set;
        }
        public string DeadVoice
        {
            get;
            set;
        }
        public string DeadBubble
        {
            get;
            set;
        }
        public string SelectVoice
        {
            get;
            set;
        }
        public string NormalAttack_Voice
        {
            get;
            set;
        }
        public string Skill1_Voice
        {
            get;
            set;
        }
        public string Skill2_Voice
        {
            get;
            set;
        }
        public string Skill3_Voice
        {
            get;
            set;
        }
        public string Skill4_Voice
        {
            get;
            set;
        }
        public string Skill9_Voice
        {
            get;
            set;
        }
        public string SneerVoice1
        {
            get;
            set;
        }
        public string SneerText1
        {
            get;
            set;
        }
        public string SneerVoice2
        {
            get;
            set;
        }
        public string SneerText2
        {
            get;
            set;
        }
        public string SneerVoice3
        {
            get;
            set;
        }
        public string SneerText3
        {
            get;
            set;
        }
        public string SneerVoice4
        {
            get;
            set;
        }
        public string SneerText4
        {
            get;
            set;
        }
        public string SneerVoice5
        {
            get;
            set;
        }
        public string SneerText5
        {
            get;
            set;
        }
        public string SneerVoice6
        {
            get;
            set;
        }
        public string SneerText6
        {
            get;
            set;
        }
    }
}
