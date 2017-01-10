using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EnumFlag
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game.Common
{
    public enum EBuffFlag
    {
        ROLE_BUFF_NONE,
        ROLE_BUFF_ANGRY,
        ROLE_BUFF_RAGE,
        ROLE_BUFF_COLD_BLOOD = 4,
        ROLE_BUFF_SHRED = 8,
        ROLE_BUFF_SPUNK = 16,
        ROLE_BUFF_STEALTH = 32,
        ROLE_BUFF_FOREST_RAID = 64,
        ROLE_BUFF_GOLDEN_STICK_1 = 128,
        ROLE_BUFF_GOLDEN_STICK_2 = 256,
        ROLE_BUFF_MAGIC_WAND = 512,
        ROLE_BUFF_SKILL_204 = 1024,
        ROLE_BUFF_SKILL_1804 = 2048,
        ROLE_BUFF_SKILL_2004 = 4096,
        ROLE_BUFF_SKILL_2404 = 8192,
        ROLE_BUFF_SKILL_1404 = 16384,
        ROLE_BUFF_SKILL_2704 = 32768,
        ROLE_BUFF_SKILL_2904 = 65536,
        ROLE_BUFF_BLUNDERBUSS = 131072,
        ROLE_BUFF_SKILL_3603 = 262144,
        ROLE_BUFF_SKILL_3801 = 524288,
        ROLE_BUFF_SKILL_4001 = 1048576,
        ROLE_BUFF_EQUIP_62 = 2097152,
        ROLE_BUFF_SKILL_3903 = 4194304,
        ROLE_BUFF_RAGE2 = 8388608,
        ROLE_BUFF_PIERCE1 = 16777216,
        ROLE_BUFF_PIERCE2 = 33554432
    }
    public enum ERoleStatus
    {
        ROLE_STATUS_NORMAL,
        ROLE_STATUS_RESTRICTION,
        ROLE_STATUS_SILENCE,
        ROLE_STATUS_VERTIGO = 4,
        ROLE_STATUS_PHYSICAL_IMMUNE = 8,
        ROLE_STATUS_STATUS_IMMUNE = 16,
        ROLE_STATUS_SHELTER = 32,
        ROLE_STATUS_SLOW_DOWN = 64,
        ROLE_STATUS_SEALED = 128,
        ROLE_STATUS_POISONED = 256
    }
}
