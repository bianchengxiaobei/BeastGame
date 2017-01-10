using UnityEngine;
using System.Collections.Generic;
using Client.Common;
using Utility.Export;
using Utility;
using GameData;
using Client.Data;
using System;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BeastVoiceRule
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.11
// 模块描述：神兽声音规则类
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Audio
{
    /// <summary>
    /// 神兽声音规则类
    /// </summary>
    public class BeastVoiceRule
    {
        #region 字段
        private IXLog m_log = XLog.GetLog<BeastBehaviour>();
        #endregion
        #region 公有方法
        public bool PlayVoice(long beastId, EBeastVoiceRuleType eBeastVoiceRuleType)
        {
            return this.PlayVoice(beastId, eBeastVoiceRuleType, 0u);
        }
        public bool PlayVoice(long beastId, EBeastVoiceRuleType eBeastVoiceRuleType, uint dwExtraData)
        {
           /* bool result;
            if (Singleton<ClientMain>.singleton.IsChangingState)
            {
                result = false;
            }
            if(Singleton<ClientMain>.singleton.EGameState != EnumGameState.eState_GameMain)
            {
                result = false;
            }
            else 
            {
                Beast beast = Singleton<BeastManager>.singleton.GetBeastById(beastId);
                if (beast == null || beast.IsError)
                {
                    result = false;
                }
                else 
                {
                    DataVoiceRule voiceRule = null;
                    foreach (var data in GameData<DataVoiceRule>.dataMap)
                    {
                        if (data.Value.RuleType == eBeastVoiceRuleType.ToString())
                        {
                            voiceRule = data.Value;
                            break;
                        }
                    }
                    if (voiceRule == null)
                    {
                        result = false;
                    }
                    else
                    {
                        DataBeastVoice dataVoice = null;
                        foreach (var current in GameData<DataBeastVoice>.dataMap)
                        {
                            if (current.Value.BeastId == beast.BeastTypeId)
                            {
                                dataVoice = current.Value;
                                break;
                            }
                        }
                        if (dataVoice == null)
                        {
                            result = false;
                        }
                        else 
                        {
                            if (voiceRule.PlayWhenDead == 0 && beast.IsDead)
                            {
                                result = false;
                            }
                            else 
                            {
                                if (voiceRule.PlayWhenDizz == 0 && beast.IsInStatus(ERoleStatus.ROLE_STATUS_VERTIGO))
                                {
                                    result = false;
                                }
                                else 
                                {
                                    if (UnityEngine.Random.Range(0, 100) >= voiceRule.PlayRate)
                                    {
                                        result = false;
                                    }
                                    else 
                                    {
                                        if (beast.IsAudioPlaying)
                                        {
                                            if (!beast.IsAudioCanBeInterrupted)
                                            {
                                                result = false;
                                                return false;
                                            }
                                            if (voiceRule.InterruptOther == 0)
                                            {
                                                result = false;
                                                return false;
                                            }
                                        }
                                        string strAudio;
                                        string strBubble;
                                        this.GetAudioFileAndBubble(beast.BeastTypeId, eBeastVoiceRuleType, dwExtraData, out strAudio, out strBubble);
                                        if (string.IsNullOrEmpty(strAudio))
                                        {
                                            result = false;
                                        }
                                        else 
                                        {
                                            if (voiceRule.HasBubble == 0)
                                            {
                                                strBubble = string.Empty;
                                            }
                                            switch (voiceRule.PlayType)
                                            {
                                                case 0:
                                                    if (beast.Player.Id != Singleton<PlayerRole>.singleton.ID)
                                                    {
                                                        result = false;
                                                        return result;
                                                    }
                                                    beast.PlayVoice(strAudio, voiceRule.CanBeInterrupted > 0);
                                                    //气泡界面更新
                                                    //DlgBase<DlgChatBubble, DlgChatBubbleBehaviour>.singleton.ShowTextMsg(heroById.Id, empty, dataByRuleType.CanBeInterrupted > 0, true);
                                                    break;
                                                case 1:
                                                    beast.PlayVoice(strAudio, voiceRule.CanBeInterrupted > 0);
                                                    break;
                                                case 2:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }                  
                }
            }
            return result;
            * */
            return true;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 取得音频文件的路径
        /// </summary>
        /// <param name="dwBeastTypeId"></param>
        /// <param name="eRuleType"></param>
        /// <param name="dwExtraData"></param>
        /// <param name="strAudio"></param>
        /// <param name="strBubble"></param>
        private void GetAudioFileAndBubble(uint dwBeastTypeId, EBeastVoiceRuleType eRuleType, uint dwExtraData, out string strAudio, out string strBubble)
        {
            strAudio = string.Empty;
            strBubble = string.Empty;
            DataBeastVoice data = null;
            foreach (var current in GameData<DataBeastVoice>.dataMap)
            {
                if (current.Value.BeastId == dwBeastTypeId)
                {
                    data = current.Value;
                    break;
                }
            }
            if (null != data)
            {
                string voicePath = string.Empty;
                string bubblePath = string.Empty;
                switch (eRuleType)
                {
                    case EBeastVoiceRuleType.BeastBorn:
                        voicePath = data.BornVoice;
                        bubblePath = data.BornBubble;
                        break;
                    case EBeastVoiceRuleType.BeastRoundStart:
                        voicePath = data.RoundStartVoice;
                        bubblePath = data.RoundStartBubble;
                        break;
                    case EBeastVoiceRuleType.BeastRevive:
                        voicePath = data.ReviveVoice;
                        bubblePath = data.ReviveBubble;
                        break;
                    case EBeastVoiceRuleType.SkillRelease:
                        if (dwExtraData == 1u)
                        {
                            voicePath = data.NormalAttack_Voice;
                        }
                        else 
                        {
                            switch (dwExtraData % 10u)
                            {
                                case 1u:
                                    voicePath = data.Skill1_Voice;
                                    break;
                                case 2u:
                                    voicePath = data.Skill2_Voice;
                                    break;
                                case 3u:
                                    voicePath = data.Skill3_Voice;
                                    break;
                                case 4u:
                                    voicePath = data.Skill4_Voice;
                                    break;
                            }
                        }
                        break;
                    case EBeastVoiceRuleType.BeastWaitInRound:
                        voicePath = data.WaitInRoundVoice;
                        bubblePath = data.WaitInRoundBubble;
                        break;
                    case EBeastVoiceRuleType.BeastWaitOutRound:
                        voicePath = data.WaitOutRoundVoice;
                        bubblePath = data.WaitOutRoundBubble;
                        break;
                    case EBeastVoiceRuleType.SendSneer:
                        string[] voices = new string[]
					    {
						    data.SneerVoice1,
						    data.SneerVoice2,
						    data.SneerVoice3,
						    data.SneerVoice4,
						    data.SneerVoice5,
						    data.SneerVoice6
					    };
                        string[] texts = new string[]
					    {
						    data.SneerText1,
						    data.SneerText2,
						    data.SneerText3,
						    data.SneerText4,
						    data.SneerText5,
						    data.SneerText6
					    };
                        voicePath = voices[(int)(dwExtraData - 1u)];
                        bubblePath = texts[(int)(dwExtraData-1u)];
                        break;
                    case EBeastVoiceRuleType.BeastDie:
                        voicePath = data.DeadVoice;
                        bubblePath = data.DeadBubble;
                        break;
                }
                //attack|1;attack|2;
                if (!string.IsNullOrEmpty(voicePath))
                {
                    string[] allVoicePath = voicePath.Split(';');
                    string[] allbubblePath = bubblePath.Split(';');
                    List<string> m_listVoicePath = new List<string>();
                    List<int> m_listProbability = new List<int>();
                    int num = 0;
                    for (int i = 0; i < allVoicePath.Length; i++)
                    {
                        string paths = allVoicePath[i];
                        string[] voice = paths.Split('|');
                        try
                        {
                            string partPath = voice[0];
                            int probability = Convert.ToInt32(voice[1]);
                            m_listVoicePath.Add(partPath);
                            num += probability;
                            m_listProbability.Add(num);
                        }
                        catch (Exception e)
                        {
                            this.m_log.Error(e.ToString());
                        }
                    }
                    int random = UnityEngine.Random.Range(0, 100);
                    for (int j = 0; j < m_listVoicePath.Count; j++)
                    {
                        int probability = m_listProbability[j];
                        if (random < probability)
                        {
                            strAudio = "Audio/BeastSound/" + data.VoiceDir + "/" + m_listVoicePath[j];
                            strBubble = allbubblePath[j >= allbubblePath.Length ? 0 : j];
                            break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}