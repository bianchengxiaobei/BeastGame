using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SkillActionData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class SkillActionData : GameData<SkillActionData>
    {
        public int cameraTweenId { get; set; }
        public string sound { get; set; }
        public string soundHit { get; set; }
        public int action { get; set; }
        public int duration { get; set; }
        public int actionTime { get; set; }
        public int nextTime { get; set; }
        public int enableStick { get; set; }
        public Dictionary<int, float> sfx { get; set; }
    }
}
