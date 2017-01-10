using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CommonDefine
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Common
{
    public class CommonDefine
    {
        private static bool m_bIsMobilePlatform;
        public static bool IsMobilePlatform
        {
            get
            {
                return CommonDefine.m_bIsMobilePlatform;
            }
        }
        static CommonDefine()
        {
            //CommonDefine.m_bIsMobilePlatform = false;
            CommonDefine.m_bIsMobilePlatform = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
        }
        /// <summary>
        /// assetbundle存储的文件目录
        /// </summary>
        public static string assetbundleFilePath = Application.dataPath + "bin/data/";
        /// <summary>
        /// 聊天时不同阵营不同颜色值
        /// </summary>
        /// <param name="bFriendly"></param>
        /// <returns></returns>
        public static string CampTypeToChatColor(bool bFriendly)
        {
            string result;
            if (bFriendly)
            {
                result = "[548ce0]";
            }
            else
            {
                result = "[b33030]";
            }
            return result;
        }
    }
    /// <summary>
    /// 角色动画动作id对应的动画名字
    /// </summary>
    public class PlayerActionNames
    {
        public readonly static Dictionary<int, string> animatorNames = new Dictionary<int, string>()
        {
            {-1, "idle"},
            { 0,"ready"}
        };
    }
    /// <summary>
    /// 实体状态配置
    /// </summary>
    public class StateCfg
    {
        public readonly static int DEATH_STATE = 0;             //死亡状态       
        public readonly static int DIZZY_STATE = 1;             //眩晕状态       
        public readonly static int POSSESS_STATE = 2;             //魅惑状态       
        public readonly static int IMMOBILIZE_STATE = 3;             //定身状态       
        public readonly static int SILENT_STATE = 4;             //沉默状态       
        public readonly static int STIFF_STATE = 5;             //僵直状态       
        public readonly static int FLOAT_STATE = 6;             //浮空状态       
        public readonly static int DOWN_STATE = 7;             //击倒状态       
        public readonly static int BACK_STATE = 8;             //击退状态       
        public readonly static int UP_STATE = 9;             //击飞状态       
        public readonly static int IMMUNITY_STATE = 10;            //免疫状态       
        public readonly static int NO_HIT_STATE = 11;            //无法被击中状态 
        public readonly static int SLOW_DOWN_STATE = 12;            //无法被击中状态 
        public readonly static int BATI_STATE = 13;            //霸体状态 
    }
    /// <summary>
    /// 游戏显示类型，有全屏，窗口，还有全屏窗口
    /// </summary>
    public enum EnumDisplayMode
    {
        eDisplayMode_FullScreen,
        eDisplayMode_Window,
        eDisplayMode_FullAndWindow
    }
    /// <summary>
    /// 神兽选择类型，分为匹配和排位
    /// </summary>
    public enum EGameBeastSelectType 
    {
        GAME_BEAST_SELECT_TYPE_MATCH,
        GAME_BEAST_SELECT_TYPE_RANK
    }
    /// <summary>
    /// 游戏类型,匹配，自定义，副本
    /// </summary>
    public enum EGameType 
    {
        GAME_TYPE_INVALID,
        GAME_TYPE_MATCH,
        GAME_TYPE_DIY,
        GAME_TYPE_PVE
    }
    /// <summary>
    /// 战斗模式，故事，自由模式，还有人机，天梯等
    /// </summary>
    public enum EnumMathMode 
    {
        EnumMathMode_NONE,
        EnumMathMode_FightMode,
        EnumMathMode_AdventureMode,
        EnumMathMode_Story,
        EnumMathMode_FreeMode,
        EnumMathMode_TrainingMode,
        EnumMathMode_QUICK_AI1,
        EnumMathMode_QUICK_AI2,
        EnumMathMode_QUICK_AI3,
        EnumMathMode_Expedition,
        EnumMathMode_Ladder,
        EnumMathMode_OMG,
        EnumMathMode_GuildBossBattle
    }
    /// <summary>
    /// 游戏阶段
    /// </summary>
    public enum EGamePhase
    {
        GAME_PHASE_INVALID,
        /// <summary>
        /// ban神兽
        /// </summary>
        GAME_PHASE_BANNING,
        /// <summary>
        /// 选择神兽
        /// </summary>
        GAME_PHASE_CHOOSING,
        /// <summary>
        /// 准备阶段
        /// </summary>
        GAME_PHASE_PREPARING,
        /// <summary>
        /// 加载阶段
        /// </summary>
        GAME_PHASE_LOADING,
        /// <summary>
        /// 游戏阶段
        /// </summary>
        GAME_PHASE_PLAYING,
        /// <summary>
        /// 结束
        /// </summary>
        GAME_PHASE_SETTLING
    }
    /// <summary>
    /// 阵营类别，分为Empire和League和无
    /// </summary>
    public enum ECampType
    {
        CAMP_INVALID,
        CAMP_EMPIRE,
        CAMP_LEAGUE
    }
    /// <summary>
    /// 所有游戏状态
    /// </summary>
    public enum EnumGameState
    {
        eState_Update,//更新状态
        eState_UpdateOtherRes,
        eState_Login,//登陆游戏状态
        eState_CreatPlayer,//创建游戏角色状态

        eState_Lobby,//进入游戏大厅
        eState_Match,//进入匹配
        eState_InRoom,//进入匹配选择
        eState_GameMain,//战斗主状态
        eState_Pve,
        eState_CustomRoom,
        eState_Guild,
        eState_Story,
        eState_SelectPlayer,//选择游戏角色状态
        eState_Max
    }
    public enum EnumMoveCenterType
    {
        eMoveCenterType_None,
        eMoveCenterType_Increas
    }
    /// <summary>
    /// 资源质量，有高，低，清晰，最高级别
    /// </summary>
    public enum AssetPRI
    {
        DownloadPRI_High = 3,
        DownloadPRI_Low = 1,
        DownloadPRI_Plain = 2,
        DownloadPRI_Superlative = 4
    }
    /// <summary>
    /// 加载类型，常规，无，等待，普通
    /// </summary>
    public enum ELoadingStyle
    {
        DefaultRule,
        None,
        LoaidngWait,
        LoadingNormal
    }
    /// <summary>
    /// 界面显示类型，普通，顶层，顶层模糊
    /// </summary>
    public enum EnumDlgCamera
    {
        Normal,
        Top,
        Top_Blur
    }
    public enum EnumAnimType
    {
        eAnimType_MoveTo,
        eAnimType_LeaveFrom,
        eAnimType_Use,
        eAnimType_Attack
    }
    /// <summary>
    /// 游戏神兽状态
    /// </summary>
    public enum ERoleStatus
    {
        ROLE_STATUS_NORMAL,//0000
        ROLE_STATUS_RESTRICTION,//禁锢0001
        ROLE_STATUS_SILENCE,//沉默0010
        ROLE_STATUS_VERTIGO = 4,//眩晕0100
        ROLE_STATUS_PHYSICAL_IMMUNE = 8,//物理免疫1000
        ROLE_STATUS_STATUS_IMMUNE = 16,//状态免疫10000
        ROLE_STATUS_SHELTER = 32,//草丛中
        ROLE_STATUS_SLOW_DOWN = 64,//减速
        ROLE_STATUS_SEALED = 128,//
        ROLE_STATUS_POISONED = 256,//中毒
        ROLE_STATUS_SPEED_UP = 512//加速
    }
    /// <summary>
    /// 操作神兽的阶段
    /// </summary>
    public enum EClientRoleStage
    {
        ROLE_STAGE_INVALID,
        ROLE_STAGE_COMPUTE_STATE,
        ROLE_STAGE_TAKE_CARD,
        ROLE_STAGE_MOVE,
        ROLE_STAGE_ACTION,
        ROLE_STAGE_DISCARD_CRAD,
        ROLE_STAGE_SELECT_BORN_POS,
        ROLE_STAGE_REVIVE,
        ROLE_STAGE_RE_SELECT_HERO,
        ROLE_STAGE_RE_SELECT_SKILL,
        ROLE_STAGE_RE_SELECT_CARD,
        ROLE_STAGE_FIRST_AID_QUERY,
        ROLE_STAGE_DODGE_QUERY,
        ROLE_STAGE_BASE_HURT_DEFENCE_QUERY,
        ROLE_STAGE_EXTRACT_ENEMY_CARD,
        ROLE_STAGE_ALTER_ENEMY_SKILL_CD,
        ROLE_STAGE_REMOVE,
        ROLE_STAGE_STATUS_PURIFY,
        ROLE_STAGE_ALTER_SELF_SKILL_CD,
        ROLE_STAGE_WAIT
    }
    /// <summary>
    /// 冷却类型，有技能和装备
    /// </summary>
    public enum ECoolDownType
    {
        COOL_DOWN_INVALID,
        COOL_DOWN_SKILL,
        COOL_DOWN_EQUIP
    }
    /// <summary>
    /// 操作时间类型，有重新开始计时和普通计时
    /// </summary>
    public enum EQueryTimeType
    {
        NORMAL_QUERY_TIME,
        BACKUP_QUERY_TIME
    }
    /// <summary>
    /// 操作阶段状态
    /// </summary>
    public enum enumOpState
    {
        eOpState_SelectBornPos,
        eOpState_Compute,
        eOpState_Move,
        eOpState_Action,
        eOpState_DiscardCard,
        eOpState_FirstAidQuery,
        eOpState_DodgeQuery,
        eOpState_ReMove,
        eOpState_ExtractEnemyCard,
        eOpState_AlterEnemySkillCD,
        eOpState_ReSelectHero,
        eOpState_ReSelectSkill,
        eOpState_ReSelectCard,
        eOpState_Revive,
        eOpState_StatusPurify,
        eOpState_AlterSelfSkillCD,
        eOpState_BaseHurtDefence,
        eOpState_Wait
    }
    /// <summary>
    /// 格子显示不同的类型
    /// </summary>
    public enum EnumShowHexagonType
    {
        eShowHexagonType_CastRange,
        eShowHexagonType_Highlight,
        eShowHexagonType_Affect,
        eShowHexagonType_Selected,
        eShowHexagonType_Hover,
        eShowHexagonType_Path,
        eShowHexagonType_Max
    }
    /// <summary>
    ///// 基地状态，分普通和庇护
    /// </summary>
    public enum EBaseStatus
    {
        BASE_STATUS_NORMAL,
        BASE_STATUS_SHELTER
    }
    /// <summary>
    /// 神兽播放声音的规则，有随机开始，出生时，重生，技能释放等等
    /// </summary>
    public enum EBeastVoiceRuleType
    {
        BeastRoundStart,
        BeastBorn,
        BeastRevive,
        SkillRelease,
        BeastWaitInRound,
        BeastWaitOutRound,
        SendSneer,
        BeastDie
    }
    /// <summary>
    /// 光圈类型
    /// </summary>
    public enum EAura
    {
        Aura_Skill_1802,
        Aura_Skill_1902,
        Aura_Skill_1203,
        Aura_Store,
        Aura_MagicSpring,
        Aura_AnimalMsger,
        Aura_Skill_3201,
        Aura_Skill_904,
        Aura_Skill_3803,
        Aura_Skill_3902,
        Aura_Skill_1403,
        Aura_Skill_4504,
        Aura_Skill_4603,
        Aura_Skill_4703,
        Aura_Skill_5003,
        Aura_Skill_5301,
        Aura_Skill_5309,
        Aura_Skill_4803 = 19,
        Aura_Skill_5401,
        Aura_Skill_704 = 17,
        Aura_Skill_5604 = 21,
        Aura_Skill_6003
    }
    /// <summary>
    /// 选择神兽的阶段，分为选择和准备
    /// </summary>
    public enum EnumSelectStep
    {
        eSelectStep_None,
        eSelectStep_Select,
        eSelectStep_Prepare
    }
    /// <summary>
    /// 鼠标icon类型
    /// </summary>
    public enum enumCursorType
    {
        eCursorType_Default,//默认
        eCursorType_Normal,//普通
        eCursorType_TicketCharge,
        eCursorType_MoneyCharge,
        eCursorType_LeftPage,
        eCursorType_RightPage,
        eCursorType_Disable,//禁用
        eCursorType_Highlight,//高亮
        eCursorType_Drag,//拖动
        eCursorType_Attack,//攻击
        eCursorType_Move,
        eCursorType_RedMark,
        eCursorType_YellowMark
    }
    /// <summary>
    /// 漂浮类型
    /// </summary>
    public enum enumFlyTextType
    {
        eFlyTextType_Common,
        eFlyTextType_Hp,
        eFlyTextType_SkillCD,
        eFlyTextType_EquipCD,
        eFlyTextType_Card,
        eFlyTextType_Gold,
        eFlyTextType_Dice,
        eFlyTextType_BeAttack,
        eFlyTextType_SystemInfo,
        eFlyTextType_Bulletin,
        eFlyTextType_Bulletin1,
        eFlyTextType_GetEquipOrCard,
        eFlyTextType_Status,
        eFlyTextType_Max,
        eFlyTextType_RemoveEquipOrCard
    }
    /// <summary>
    /// 扣血漂浮特效类型，比如暴击等
    /// </summary>
    public enum EnumHpEffectType
    {
        eHpEffectType_Damage,
        eHpEffectType_Crit,
        eHpEffectType_OverDamage,
        eHpEffectType_Heal
    }
    /// <summary>
    /// 扣血等级类型
    /// </summary>
    public enum EBloodLevel
    {
        None,
        Low,
        Middle,
        High
    }
    /// <summary>
    /// 角色职业类别索引
    /// </summary>
    public enum EnumRoleTypeIndex : int
    {
        e_RoleType_Explorer = 2,
        e_RoleType_Magician = 3,
        e_RoleType_Engineer = 0,
        e_RoleType_Cultivator = 1
    }
    /// <summary>
    /// 图集类型
    /// </summary>
    public enum EnumAtlasType
    {
        Beast,
        Skill,
        Equip
    }
    /// <summary>
    /// 技能激活类型
    /// </summary>
    public enum ESkillActivateType
    {
        SKILL_ACTIVATE_TYPE_INVALID,
        SKILL_ACTIVATE_TYPE_NORMAL,
        SKILL_ACTIVATE_TYPE_ROUND_ADD,
        SKILL_ACTIVATE_TYPE_SHOP,
        SKILL_ACTIVATE_TYPE_PICKUP,
        SKILL_ACTIVATE_TYPE_OMG_BUFF
    }
    /// <summary>
    /// 技能类型，有skill，equip，召唤师技能等
    /// </summary>
    public enum EnumSkillType
    {
        eSkillType_Invaild,
        eSkillType_Skill,
        eSKillType_Equip,
        eSkillType_Summelor
    }
    /// <summary>
    /// 技能使用的错误码
    /// </summary>
    public enum EnumErrorCodeCheckUse
    {
        eCheckErr_Success,
        eCheckErr_FatalErr,
        eCheckErr_NotActiveSkill,
        eCheckErr_NotActiveEquip,
        eCheckErr_NotUseCardInMove,
        eCheckErr_NoneTarget,
        eCheckErr_InCD,
        eCheckErr_NoneCanAttackCount,
        eCheckErr_InWater,
        eCheckErr_NoneSkillCanChoose,
        eCheckErr_AlreadyInBuff,
        eCheckErr_NoneEquip,
        eCheckErr_NoneCard,
        eCheckErr_NoneHeroNeedHeal,
        eCheckErr_GoldNotEnough,
        eCheckErr_HPNotEnough,
        eCheckErr_CardNotEnough,
        eCheckErr_HPNotCorrect,
        eCheckErr_UseCountLimit,
        eCheckErr_MoveDisLimit,
        eCheckErr_InSilence,
        eCheckErr_InSEALED,
        eCheckErr_NoneEffectChoose,
        eCheckErr_DefaultNotFitCondition,
        eCheckErr_MojingBaoDanNoCard,
        eCheckErr_MojingBaoDanNoEquip,
        eCheckErr_ShenShengBiHuBase,
        eCheckErr_ShenShengBiHuHero,
        eCheckErr_ZhengYiDamage,
        eCheckErr_ZhengYiHeal,
        eCheckErr_QianXingDefend,
        eCheckErr_QianXingAttack,
        eCheckErr_EquipInSEALED,
        eCheckErr_NoBase,
        eCheckErr_NoEquip,
        eCheckErr_NoCard75,
        eCheckErr_NoneTargetTerrain,
        eCheckErr_NoCard
    }
    /// <summary>
    /// 战棋技能战斗子阶段
    /// </summary>
    public enum enumSubActionState
    {
        eSubActionState_Disable,
        eSubActionState_Enable,
        eSubActionState_SkillUse
    }
}
