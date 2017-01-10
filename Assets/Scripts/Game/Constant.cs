using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Constant
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：常量池
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 角色动作ID对照动画名字表，用于技能结束的判断
    /// </summary>
    public class PlayerActionName
    {
        public readonly static Dictionary<int, string> actionOfNames = new Dictionary<int, string>
        { 
            {-1, "idle"},
            {0, "ready"},
            {1, "attack_1"},
            {2, "attack_2"},
            {3, "attack_3"},
            {4, "powercharge"},
            {5, "powerattack_1"},
            {6, "powerattack_2"},
            {7, "powerattack_3"},
            {8, "skill_1"},
            {9, "skill_2"},
            {10, "rush"},
            {11, "hit"},
            {12, "hitair"},
            {13, "hitground"},
            {14, "knockdown"},
            {15, "push"},
            {16, "stun"},
            {17, "die"},
        };
    }
    public enum Vocation : byte 
    {
        Engineer = 0
    }
    public enum TargetType 
    {
        Enemy = 0,//敌人
        Myself = 1,//自己
        TeamMember = 2,//团队成员
        Ally = 3//伙伴（宠物，雇佣军）
    }
    public enum TargetRangeType 
    {
        LineRange = 3,
        SelectRange = 0,
        CircleRange = 1,
        SingleTarget  =2,
        WorldRange = 6
    }
}
