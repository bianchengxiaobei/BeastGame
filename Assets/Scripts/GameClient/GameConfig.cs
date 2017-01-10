using UnityEngine;
using System.Collections.Generic;
using Game.Common;
using UnityAssetEx.Export;
using System.Xml;
using System;
using Utility;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameConfig
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class GameConfig
    {
        public HashSet<int> NoAttackSkills = new HashSet<int>();
        public HashSet<int> NoAttackEquips = new HashSet<int>();
        public HashSet<int> NoAttackCards = new HashSet<int>();
        private static GameConfig gs_Singleton = new GameConfig();
        public int EmpireBaseEffectWhenHeroDead = 0;
        public int LeagueBaseEffectWhenHeroDead = 0;
        public int EmpireBaseExploreEffect = 0;
        public int LeagueBaseExploreEffect = 0;
        public int EmpireBaseAttackedEffect = 0;
        public int LeagueBaseAttackedEffect = 0;
        public int EmpireBaseLowHpEffect = 0;
        public int LeagueBaseLowHpEffect = 0;
        public int EmpireBaseHighHpEffect = 0;
        public int LeagueBaseHighHpEffect = 0;
        public int EmpireBaseShieldEfffect = 0;
        public int LeagueBaseShieldEfffect = 0;
        public int BaseLowHp = 0;
        public float GameEndUIShowTime = 0f;
        public float GameEndCameraMoveTime = 2f;
        public float GameEndCameraDist = 15f;
        public float GameEndFailTipTime = 0f;
        public float BaseExploreTime = 3f;
        public float BaseShowDelay = 0f;
        public float KillShowDelay = 0f;
        public Dictionary<EBuffFlag, int> BuffEffects = new Dictionary<EBuffFlag, int>();
        public Dictionary<ERoleStatus, int> StatusEffects = new Dictionary<ERoleStatus, int>();
        public float CameraMoveSpeed = 12f;
        public float SeqShowTimeOut = 0f;
        public int ChatBubbleShowTime = 5;
        public int ChatShowTime = 1;
        public float DlgFlyText = 100f;
        public int DlgLoadingShowTime = 2;
        public int SoundTimeInterval = 10;
        public int ExpressionTimeInterval = 60;
        public int ExpressionShowCount = 2;
        public float BaseDelayHideTime = 1f;
        public float SummaryBlackDuration = 1f;
        public float SummaryBlackDepth = 1f;
        public float DlgPveSelectBirthTimeInterval = 1.7f;
        public float DlgPveSelectHerosBurnTimeInterval = 0.5f;
        public float DlgPveSelectShowCardTimeInterval = 0.3f;
        public float DlgPveSelectHideCardTimeInterval = 1f;
        public float DlgPveShowHeroIconTimeInterval = 0.5f;
        public float DlgSeting = 10f;
        public float DlgSurrenderWaitTime = 20f;
        public float DlgSurrenderPlayerInterval = 20f;
        public float FollowHeightOffset = 1f;
        public float FollowDist = 15f;
        public bool FollowCamera = false;
        private float m_fGameSpeed = 1f;
        public int ForestEffect = 0;
        public int ForestSkillId = 0;
        public int WaterEffect = 0;
        public int AddMoneyEffect = 0;
        public int LoseMoneyEffect = 0;
        public int AddMoneyFlyEffect = 0;
        public int TeleportEffect = 0;
        public int ShowCardChgValue = 0;
        public int ShowCDChgValue = 0;
        private float m_fCameAutoMoveSpeed = 1f;
        private bool bCameraExt = false;
        public float Hscale = 0f;
        public float Vscale = 0f;
        public bool RestrictMove = false;
        public int WarningChgHpLow = 1;
        public int WarningChgHpMiddle = 3;
        public int NoticeChgHp = 3;
        public int MoveStagePassEffect = 0;
        public int MoveStagePassUIEffect = 0;
        public int ActionStagePassEffect = 0;
        public int ActionStagePassUIEffect = 0;
        public int AddCardFlyEffect;
        public int AddEquipFlyEffect;
        public float m_fMinRotateAngle = 0f;
        public float m_fMaxRotateAngle = 0f;
        public float m_fDeceRotateAngle = 0f;
        public float m_fMouseBright = 2f;
        public int m_nMinChargeSpeed = 0;
        public int m_nMinChargeCount = 0;
        public float m_fExpBookChargeSpeed = 0f;
        public int m_nExpBookChargeCount = 0;
        public int m_nPVEEnterItemID = 0;
        public int m_nPVEEnterItemCount = 0;
        public int m_nPVEEnterMoney = 0;
        public int m_nPVEEnterTicket = 0;
        public int m_nNeedForceEnterPassId = 0;
        public int m_nNeedForceSaveEquipCount = 0;
        public float m_fquitqueuefactor = 1f;
        public int m_unexaggeratefactor = 1;
        public float m_fchangefactor = 1f;
        public float m_fchangeinterval = 1f;
        public int m_unMoney = 0;
        public float DuraTimeFlyBox = 0f;
        public float DuraTimeShowBox = 0f;
        public float DuraTimeFlyItem = 0f;
        public float DuraTimeShowItemZero = 0f;
        public float DuraTimeShowItemOne = 0f;
        public float DuraTimeShowItemTwo = 0f;
        public int MoneyCritCondition = 0;
        public int ItemCritCondition = 0;
        public static GameConfig singleton
        {
            get
            {
                return GameConfig.gs_Singleton;
            }
        }
        public bool EnableHeroSleepChat
        {
            get;
            set;
        }
        public float GameSpeed
        {
            get
            {
                return this.m_fGameSpeed;
            }
            set
            {
                this.m_fGameSpeed = value;
            }
        }
        public float CameAutoMoveSpeed
        {
            get
            {
                return this.m_fCameAutoMoveSpeed;
            }
            set
            {
                this.m_fCameAutoMoveSpeed = value;
            }
        }
        public bool CameraExt
        {
            get
            {
                return this.bCameraExt;
            }
            set
            {
                this.bCameraExt = value;
            }
        }
        public GameConfig()
        {
            this.EnableHeroSleepChat = true;
        }
        public void Init()
        {
            string strRelativePath = "config/gameconfig.xml";
            XmlDocument xmlDocument = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath, false));
            this.OnLoadFinishEventHandler(xmlDocument);
        }
        private void OnLoadFinishEventHandler(XmlDocument xmlDoc)
        {
            if (null != xmlDoc)
            {
                try
                {
                    XmlNode xmlNode = xmlDoc.SelectSingleNode("GameConfig");
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in childNodes)
                    {
                        if (xmlNode2.Name.ToLower() == "attanimcfg")
                        {
                            this.LoadAttackAnimCfg(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "global")
                        {
                            this.LoadGlobalConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "stagepasseffect")
                        {
                            this.LoadStagePassEffect(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "skill")
                        {
                            this.LoadSkillConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "effect")
                        {
                            this.LoadEffectConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "camerafollow")
                        {
                            this.LoadCameraFollowConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "buffeffect")
                        {
                            this.LoadBuffEffectConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "statuseffect")
                        {
                            this.LoadStatusEffectConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "gameend")
                        {
                            this.LoadGameEndConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "summarydark")
                        {
                            this.LoadSummayrStageEffect(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "specaileffect")
                        {
                            this.LoadSpecialEffectConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "heroshow")
                        {
                            this.LoadHeroShowConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "book")
                        {
                            this.LoadBookConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "pvecondition")
                        {
                            this.LoadPVEConditionConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "story")
                        {
                            this.LoadStoryConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "lingup")
                        {
                            this.LoadLineUpConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "refreshbuff")
                        {
                            this.LoadBuffRefreshConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "boxdura")
                        {
                            this.LoadBoxDuraConfig(xmlNode2);
                        }
                        if (xmlNode2.Name.ToLower() == "drop_crit_condition")
                        {
                            this.LoadDropCritConditionConfig(xmlNode2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    XLog.Log.Fatal(ex.Message);
                }
            }
        }
        private void readNoAttackCfg(XmlElement RowNode, string key, HashSet<int> hashSet)
        {
            hashSet.Clear();
            if (RowNode.HasAttribute(key))
            {
                string attribute = RowNode.GetAttribute(key);
                if (attribute != null)
                {
                    string[] array = attribute.Split(new char[]
					{
						';'
					});
                    for (int i = 0; i < array.Length; i++)
                    {
                        int item = Convert.ToInt32(array[i]);
                        if (!hashSet.Contains(item))
                        {
                            hashSet.Add(item);
                        }
                    }
                }
            }
        }
        public bool LoadAttackAnimCfg(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("skills"))
                {
                    this.readNoAttackCfg(xmlElement, "skills", this.NoAttackSkills);
                }
                if (xmlElement.HasAttribute("equips"))
                {
                    this.readNoAttackCfg(xmlElement, "equips", this.NoAttackEquips);
                }
                if (xmlElement.HasAttribute("cards"))
                {
                    this.readNoAttackCfg(xmlElement, "cards", this.NoAttackCards);
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadGlobalConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("SequenceShow"))
                {
                    Singleton<SequenceShowManager>.singleton.UseSeqShow = (xmlElement.GetAttribute("SequenceShow") == "true");
                }
                if (xmlElement.HasAttribute("SeqShowTimeOut"))
                {
                    this.SeqShowTimeOut = (float)Convert.ToDouble(xmlElement.GetAttribute("SeqShowTimeOut"));
                }
                if (xmlElement.HasAttribute("GameSpeed"))
                {
                    this.GameSpeed = (float)Convert.ToDouble(xmlElement.GetAttribute("GameSpeed"));
                }
                if (xmlElement.HasAttribute("CameraExt"))
                {
                    this.CameraExt = (Convert.ToInt32(xmlElement.GetAttribute("CameraExt")) == 1);
                }
                if (xmlElement.HasAttribute("CameAutoMoveSpeed"))
                {
                    this.CameAutoMoveSpeed = (float)Convert.ToDouble(xmlElement.GetAttribute("CameAutoMoveSpeed"));
                }
                if (xmlElement.HasAttribute("VScale"))
                {
                    this.Vscale = (float)Convert.ToDouble(xmlElement.GetAttribute("VScale"));
                }
                if (xmlElement.HasAttribute("HScale"))
                {
                    this.Hscale = (float)Convert.ToDouble(xmlElement.GetAttribute("HScale"));
                }
                if (xmlElement.HasAttribute("RestrictMove"))
                {
                    this.RestrictMove = (Convert.ToInt32(xmlElement.GetAttribute("RestrictMove")) == 1);
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadSkillConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                Singleton<SequenceShowManager>.singleton.PreStageSeg = (float)Convert.ToDouble(xmlElement.GetAttribute("PreStageSeg"));
                Singleton<SequenceShowManager>.singleton.ShowStageSeg = (float)Convert.ToDouble(xmlElement.GetAttribute("ShowStageSeg"));
                Singleton<SequenceShowManager>.singleton.StatusSeg = (float)Convert.ToDouble(xmlElement.GetAttribute("StatusSeg"));
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadGameEndConfig(XmlNode RowNode)
        {
            XmlElement xmlElement = (XmlElement)RowNode;
            string empty = string.Empty;
            if (xmlElement.HasAttribute("GameEndUIShowTime"))
            {
                this.GameEndUIShowTime = (float)Convert.ToDouble(xmlElement.GetAttribute("GameEndUIShowTime"));
            }
            if (xmlElement.HasAttribute("GameEndCameraMoveTime"))
            {
                this.GameEndCameraMoveTime = (float)Convert.ToDouble(xmlElement.GetAttribute("GameEndCameraMoveTime"));
            }
            if (xmlElement.HasAttribute("GameEndCameraDist"))
            {
                this.GameEndCameraDist = (float)Convert.ToDouble(xmlElement.GetAttribute("GameEndCameraDist"));
            }
            if (xmlElement.HasAttribute("GameEndFailTipTime"))
            {
                this.GameEndFailTipTime = (float)Convert.ToDouble(xmlElement.GetAttribute("GameEndFailTipTime"));
            }
            if (xmlElement.HasAttribute("BaseExploreTime"))
            {
                this.BaseExploreTime = (float)Convert.ToDouble(xmlElement.GetAttribute("BaseExploreTime"));
            }
            if (xmlElement.HasAttribute("BaseShowDelay"))
            {
                this.BaseShowDelay = (float)Convert.ToDouble(xmlElement.GetAttribute("BaseShowDelay"));
            }
            if (xmlElement.HasAttribute("KillShowDelay"))
            {
                this.KillShowDelay = (float)Convert.ToDouble(xmlElement.GetAttribute("KillShowDelay"));
            }
            if (xmlElement.HasAttribute("ChatBubbleShowTime"))
            {
                this.ChatBubbleShowTime = (int)Convert.ToDouble(xmlElement.GetAttribute("ChatBubbleShowTime"));
            }
            if (xmlElement.HasAttribute("BaseDelayHideTime"))
            {
                this.BaseDelayHideTime = (float)Convert.ToDouble(xmlElement.GetAttribute("BaseDelayHideTime"));
            }
            if (xmlElement.HasAttribute("ChatShowTime"))
            {
                this.ChatShowTime = (int)Convert.ToDouble(xmlElement.GetAttribute("ChatShowTime"));
            }
            if (xmlElement.HasAttribute("DlgLoadingShowTime"))
            {
                this.DlgLoadingShowTime = (int)Convert.ToDouble(xmlElement.GetAttribute("DlgLoadingShowTime"));
            }
            if (xmlElement.HasAttribute("SoundTimeInterval"))
            {
                this.SoundTimeInterval = (int)Convert.ToDouble(xmlElement.GetAttribute("SoundTimeInterval"));
            }
            if (xmlElement.HasAttribute("ExpressionTimeInterval"))
            {
                this.ExpressionTimeInterval = (int)Convert.ToDouble(xmlElement.GetAttribute("ExpressionTimeInterval"));
            }
            if (xmlElement.HasAttribute("ExpressionShowCount"))
            {
                this.ExpressionShowCount = (int)Convert.ToDouble(xmlElement.GetAttribute("ExpressionShowCount"));
            }
            if (xmlElement.HasAttribute("DlgPveSelectHerosBurnTimeInterval"))
            {
                this.DlgPveSelectHerosBurnTimeInterval = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgPveSelectHerosBurnTimeInterval"));
            }
            if (xmlElement.HasAttribute("DlgPveSelectBirthTimeInterval"))
            {
                this.DlgPveSelectBirthTimeInterval = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgPveSelectBirthTimeInterval"));
            }
            if (xmlElement.HasAttribute("DlgPveSelectShowCardTime"))
            {
                this.DlgPveSelectShowCardTimeInterval = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgPveSelectShowCardTimeInterval"));
            }
            if (xmlElement.HasAttribute("DlgPveSelectHideCardTimeInterval"))
            {
                this.DlgPveSelectHideCardTimeInterval = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgPveSelectHideCardTimeInterval"));
            }
            if (xmlElement.HasAttribute("DlgPveShowHeroIconTimeInterval"))
            {
                this.DlgPveShowHeroIconTimeInterval = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgPveShowHeroIconTimeInterval"));
            }
            if (xmlElement.HasAttribute("DlgSeting"))
            {
                this.DlgSeting = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgSeting"));
            }
            if (xmlElement.HasAttribute("DlgSurrenderWaitTime"))
            {
                this.DlgSurrenderWaitTime = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgSurrenderWaitTime"));
            }
            if (xmlElement.HasAttribute("DlgSurrenderPlayerInterval"))
            {
                this.DlgSurrenderPlayerInterval = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgSurrenderPlayerInterval"));
            }
            if (xmlElement.HasAttribute("DlgFlyText"))
            {
                this.DlgFlyText = (float)Convert.ToDouble(xmlElement.GetAttribute("DlgFlyText"));
            }
            return true;
        }
        public bool LoadSummayrStageEffect(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("SummaryBlackDepth"))
                {
                    this.SummaryBlackDepth = (float)Convert.ToDouble(xmlElement.GetAttribute("SummaryBlackDepth"));
                }
                if (xmlElement.HasAttribute("SummaryBlackDuration"))
                {
                    this.SummaryBlackDuration = (float)Convert.ToDouble(xmlElement.GetAttribute("SummaryBlackDuration"));
                }
                if (xmlElement.HasAttribute("WarningChgHpLow"))
                {
                    this.WarningChgHpLow = Convert.ToInt32(xmlElement.GetAttribute("WarningChgHpLow"));
                }
                if (xmlElement.HasAttribute("WarningChgHpMiddle"))
                {
                    this.WarningChgHpMiddle = Convert.ToInt32(xmlElement.GetAttribute("WarningChgHpMiddle"));
                }
                if (xmlElement.HasAttribute("NoticeChgHp"))
                {
                    this.NoticeChgHp = Convert.ToInt32(xmlElement.GetAttribute("NoticeChgHp"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadEffectConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("LeagueBaseEffectWhenHeroDead"))
                {
                    this.LeagueBaseEffectWhenHeroDead = Convert.ToInt32(xmlElement.GetAttribute("LeagueBaseEffectWhenHeroDead"));
                }
                if (xmlElement.HasAttribute("EmpireBaseEffectWhenHeroDead"))
                {
                    this.EmpireBaseEffectWhenHeroDead = Convert.ToInt32(xmlElement.GetAttribute("EmpireBaseEffectWhenHeroDead"));
                }
                if (xmlElement.HasAttribute("LeagueBaseExploreEffect"))
                {
                    this.LeagueBaseExploreEffect = Convert.ToInt32(xmlElement.GetAttribute("LeagueBaseExploreEffect"));
                }
                if (xmlElement.HasAttribute("EmpireBaseExploreEffect"))
                {
                    this.EmpireBaseExploreEffect = Convert.ToInt32(xmlElement.GetAttribute("EmpireBaseExploreEffect"));
                }
                if (xmlElement.HasAttribute("LeagueBaseAttackedEffect"))
                {
                    this.LeagueBaseAttackedEffect = Convert.ToInt32(xmlElement.GetAttribute("LeagueBaseAttackedEffect"));
                }
                if (xmlElement.HasAttribute("EmpireBaseAttackedEffect"))
                {
                    this.EmpireBaseAttackedEffect = Convert.ToInt32(xmlElement.GetAttribute("EmpireBaseAttackedEffect"));
                }
                if (xmlElement.HasAttribute("EmpireBaseLowHpEffect"))
                {
                    this.EmpireBaseLowHpEffect = Convert.ToInt32(xmlElement.GetAttribute("EmpireBaseLowHpEffect"));
                }
                if (xmlElement.HasAttribute("LeagueBaseLowHpEffect"))
                {
                    this.LeagueBaseLowHpEffect = Convert.ToInt32(xmlElement.GetAttribute("LeagueBaseLowHpEffect"));
                }
                if (xmlElement.HasAttribute("EmpireBaseHighHpEffect"))
                {
                    this.EmpireBaseHighHpEffect = Convert.ToInt32(xmlElement.GetAttribute("EmpireBaseHighHpEffect"));
                }
                if (xmlElement.HasAttribute("LeagueBaseHighHpEffect"))
                {
                    this.LeagueBaseHighHpEffect = Convert.ToInt32(xmlElement.GetAttribute("LeagueBaseHighHpEffect"));
                }
                if (xmlElement.HasAttribute("EmpireBaseShieldEfffect"))
                {
                    this.EmpireBaseShieldEfffect = Convert.ToInt32(xmlElement.GetAttribute("EmpireBaseShieldEfffect"));
                }
                if (xmlElement.HasAttribute("LeagueBaseShieldEfffect"))
                {
                    this.LeagueBaseShieldEfffect = Convert.ToInt32(xmlElement.GetAttribute("LeagueBaseShieldEfffect"));
                }
                this.BaseLowHp = Convert.ToInt32(xmlElement.GetAttribute("BaseLowHp"));
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadHeadIconConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("ShowCardChgValue"))
                {
                    this.ShowCardChgValue = Convert.ToInt32(xmlElement.GetAttribute("ShowCardChgValue"));
                }
                if (xmlElement.HasAttribute("ShowCDChgValue"))
                {
                    this.ShowCDChgValue = Convert.ToInt32(xmlElement.GetAttribute("ShowCDChgValue"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadCameraFollowConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("FollowHeightOffset"))
                {
                    this.FollowHeightOffset = (float)Convert.ToDouble(xmlElement.GetAttribute("FollowHeightOffset"));
                }
                if (xmlElement.HasAttribute("FollowDist"))
                {
                    this.FollowDist = (float)Convert.ToDouble(xmlElement.GetAttribute("FollowDist"));
                }
                if (xmlElement.HasAttribute("CameraMoveSpeed"))
                {
                    this.CameraMoveSpeed = (float)Convert.ToDouble(xmlElement.GetAttribute("CameraMoveSpeed"));
                }
                if (xmlElement.HasAttribute("FollowCamera"))
                {
                    this.FollowCamera = (Convert.ToInt32(xmlElement.GetAttribute("FollowCamera")) == 1);
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadStatusEffectConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                string attribute = xmlElement.GetAttribute("Restriction_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_RESTRICTION] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Silence_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_SILENCE] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Vertigo_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_VERTIGO] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("PhysicalImmune_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_PHYSICAL_IMMUNE] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("StatusImmune_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_STATUS_IMMUNE] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Shelter_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_SHELTER] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("SlowDown_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_SLOW_DOWN] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Sealed_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_SEALED] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Poison_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.StatusEffects[ERoleStatus.ROLE_STATUS_POISONED] = Convert.ToInt32(attribute);
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadBuffEffectConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                string attribute = xmlElement.GetAttribute("Angry_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_ANGRY] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Rage_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_RAGE] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("ColdBlood_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_COLD_BLOOD] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Shred_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_SHRED] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Spunk_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_SPUNK] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("Stealth_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_STEALTH] = Convert.ToInt32(attribute);
                }
                attribute = xmlElement.GetAttribute("ForestRaid_Effect");
                if (!string.IsNullOrEmpty(attribute))
                {
                    this.BuffEffects[EBuffFlag.ROLE_BUFF_FOREST_RAID] = Convert.ToInt32(attribute);
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadSpecialEffectConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("ForestEffect"))
                {
                    this.ForestEffect = Convert.ToInt32(xmlElement.GetAttribute("ForestEffect"));
                }
                if (xmlElement.HasAttribute("TeleportEffect"))
                {
                    this.TeleportEffect = Convert.ToInt32(xmlElement.GetAttribute("TeleportEffect"));
                }
                if (xmlElement.HasAttribute("WaterEffect"))
                {
                    this.WaterEffect = Convert.ToInt32(xmlElement.GetAttribute("WaterEffect"));
                }
                if (xmlElement.HasAttribute("ForestSkillId"))
                {
                    this.ForestSkillId = Convert.ToInt32(xmlElement.GetAttribute("ForestSkillId"));
                }
                if (xmlElement.HasAttribute("LoseMoneyEffect"))
                {
                    this.LoseMoneyEffect = Convert.ToInt32(xmlElement.GetAttribute("LoseMoneyEffect"));
                }
                if (xmlElement.HasAttribute("AddMoneyEffect"))
                {
                    this.AddMoneyEffect = Convert.ToInt32(xmlElement.GetAttribute("AddMoneyEffect"));
                }
                if (xmlElement.HasAttribute("AddMoneyFlyEffect"))
                {
                    this.AddMoneyFlyEffect = Convert.ToInt32(xmlElement.GetAttribute("AddMoneyFlyEffect"));
                }
                if (xmlElement.HasAttribute("AddCardFlyEffect"))
                {
                    this.AddCardFlyEffect = Convert.ToInt32(xmlElement.GetAttribute("AddCardFlyEffect"));
                }
                if (xmlElement.HasAttribute("AddEquipFlyEffect"))
                {
                    this.AddEquipFlyEffect = Convert.ToInt32(xmlElement.GetAttribute("AddEquipFlyEffect"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public void DoGlobalConfig()
        {
            Time.timeScale = this.GameSpeed;
        }
        public bool LoadStagePassEffect(XmlNode RowNode)
        {
            XmlElement xmlElement = (XmlElement)RowNode;
            if (xmlElement.HasAttribute("MoveStagePassEffect"))
            {
                this.MoveStagePassEffect = Convert.ToInt32(xmlElement.GetAttribute("MoveStagePassEffect"));
            }
            if (xmlElement.HasAttribute("MoveStagePassUIEffect"))
            {
                this.MoveStagePassEffect = Convert.ToInt32(xmlElement.GetAttribute("MoveStagePassUIEffect"));
            }
            if (xmlElement.HasAttribute("ActionStagePassEffect"))
            {
                this.MoveStagePassEffect = Convert.ToInt32(xmlElement.GetAttribute("ActionStagePassEffect"));
            }
            if (xmlElement.HasAttribute("ActionStagePassUIEffect"))
            {
                this.MoveStagePassEffect = Convert.ToInt32(xmlElement.GetAttribute("ActionStagePassUIEffect"));
            }
            return true;
        }
        public bool LoadHeroShowConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("MinRotateAngle"))
                {
                    this.m_fMinRotateAngle = (float)Convert.ToDouble(xmlElement.GetAttribute("MinRotateAngle"));
                }
                if (xmlElement.HasAttribute("MaxRotateAngle"))
                {
                    this.m_fMaxRotateAngle = (float)Convert.ToDouble(xmlElement.GetAttribute("MaxRotateAngle"));
                }
                if (xmlElement.HasAttribute("DeceRotateAngle"))
                {
                    this.m_fDeceRotateAngle = (float)Convert.ToDouble(xmlElement.GetAttribute("DeceRotateAngle"));
                }
                if (xmlElement.HasAttribute("MouseBright"))
                {
                    this.m_fMouseBright = (float)Convert.ToDouble(xmlElement.GetAttribute("MouseBright"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadBookConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("MinChargeSpeed"))
                {
                    this.m_nMinChargeSpeed = Convert.ToInt32(xmlElement.GetAttribute("MinChargeSpeed"));
                }
                if (xmlElement.HasAttribute("MinChargeCount"))
                {
                    this.m_nMinChargeCount = Convert.ToInt32(xmlElement.GetAttribute("MinChargeCount"));
                }
                if (xmlElement.HasAttribute("ExpBookChargeSpeed"))
                {
                    this.m_fExpBookChargeSpeed = (float)Convert.ToDouble(xmlElement.GetAttribute("ExpBookChargeSpeed"));
                }
                if (xmlElement.HasAttribute("ExpBookChargeCount"))
                {
                    this.m_nExpBookChargeCount = Convert.ToInt32(xmlElement.GetAttribute("ExpBookChargeCount"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadPVEConditionConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("PVEEnterItemID"))
                {
                    this.m_nPVEEnterItemID = Convert.ToInt32(xmlElement.GetAttribute("PVEEnterItemID"));
                }
                if (xmlElement.HasAttribute("PVEEnterItemCount"))
                {
                    this.m_nPVEEnterItemCount = Convert.ToInt32(xmlElement.GetAttribute("PVEEnterItemCount"));
                }
                if (xmlElement.HasAttribute("PVEEnterMoney"))
                {
                    this.m_nPVEEnterMoney = Convert.ToInt32(xmlElement.GetAttribute("PVEEnterMoney"));
                }
                if (xmlElement.HasAttribute("PVEEnterTicket"))
                {
                    this.m_nPVEEnterTicket = Convert.ToInt32(xmlElement.GetAttribute("PVEEnterTicket"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadStoryConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("ForceEnterPassId"))
                {
                    this.m_nNeedForceEnterPassId = Convert.ToInt32(xmlElement.GetAttribute("ForceEnterPassId"));
                }
                if (xmlElement.HasAttribute("ForceSaveEquipCount"))
                {
                    this.m_nNeedForceSaveEquipCount = Convert.ToInt32(xmlElement.GetAttribute("ForceSaveEquipCount"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadLineUpConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("quitqueuefactor"))
                {
                    this.m_fquitqueuefactor = (float)Convert.ToDouble(xmlElement.GetAttribute("quitqueuefactor"));
                }
                if (xmlElement.HasAttribute("exaggeratefactor"))
                {
                    this.m_unexaggeratefactor = (int)Convert.ToInt16(xmlElement.GetAttribute("exaggeratefactor"));
                }
                if (xmlElement.HasAttribute("changefactor"))
                {
                    this.m_fchangefactor = (float)Convert.ToDouble(xmlElement.GetAttribute("changefactor"));
                }
                if (xmlElement.HasAttribute("changeinterval"))
                {
                    this.m_fchangeinterval = (float)Convert.ToDouble(xmlElement.GetAttribute("changeinterval"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadBuffRefreshConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("money"))
                {
                    this.m_unMoney = (int)Convert.ToInt16(xmlElement.GetAttribute("money"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadBoxDuraConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("flybox"))
                {
                    this.DuraTimeFlyBox = (float)Convert.ToDouble(xmlElement.GetAttribute("flybox"));
                }
                if (xmlElement.HasAttribute("showbox"))
                {
                    this.DuraTimeShowBox = (float)Convert.ToDouble(xmlElement.GetAttribute("showbox"));
                }
                if (xmlElement.HasAttribute("flyitem"))
                {
                    this.DuraTimeFlyItem = (float)Convert.ToDouble(xmlElement.GetAttribute("flyitem"));
                }
                if (xmlElement.HasAttribute("showitemzero"))
                {
                    this.DuraTimeShowItemZero = (float)Convert.ToDouble(xmlElement.GetAttribute("showitemzero"));
                }
                if (xmlElement.HasAttribute("showitemone"))
                {
                    this.DuraTimeShowItemOne = (float)Convert.ToDouble(xmlElement.GetAttribute("showitemone"));
                }
                if (xmlElement.HasAttribute("showitemtwo"))
                {
                    this.DuraTimeShowItemTwo = (float)Convert.ToDouble(xmlElement.GetAttribute("showitemtwo"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
        public bool LoadDropCritConditionConfig(XmlNode RowNode)
        {
            bool result = true;
            try
            {
                XmlElement xmlElement = (XmlElement)RowNode;
                if (xmlElement.HasAttribute("money_crit_condition"))
                {
                    this.MoneyCritCondition = Convert.ToInt32(xmlElement.GetAttribute("money_crit_condition"));
                }
                if (xmlElement.HasAttribute("item_crit_condition"))
                {
                    this.ItemCritCondition = Convert.ToInt32(xmlElement.GetAttribute("item_crit_condition"));
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Error(ex.ToString());
                result = false;
            }
            return result;
        }
    }
}
