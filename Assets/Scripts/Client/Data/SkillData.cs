using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SkillData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：技能数据配置
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class SkillData : GameData<SkillData>
    {
        //技能数据
        public int name { get; protected set; }
        public int level { get; protected set; }
        public int desc { get; protected set; }
        public int icon { get; protected set; }
        public int posi { get; protected set; }
        public int vocation { get; protected set; }
        public int weapon { get; protected set; }
        public List<int> dependSkill { get; protected set; }

        //学习限制
        //施放条件
        public List<int> cd { get; protected set; }
        public int castTime { get; protected set; }
        public int castRange { get; protected set; }

        public List<int> skillAction { get; protected set; }

        public int totalActionDuration = 0;
        public static readonly string fileName = "SkillData";
    }
}