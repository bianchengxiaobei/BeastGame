using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Utility.Export;
using UnityAssetEx.Export;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UserOptions
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient
{
    internal class UserOptions
    {
        private const string UserConfigFile = "config/user/useroptionscfg.xml";
        private bool m_bHasSet = false;
        private bool m_bIsRememberAccount = false;
        private string m_strUserId;
        private string m_strServerName = string.Empty;
        private int m_nDisplayMode = 1;
        private IGraphicsResolution m_IGraphicsResolution = null;
        private bool m_bMuteSound = false;
        private float m_fBGSoundValue = 0.5f;
        private float m_fUISoundValue = 0.75f;
        private float m_fSoundValue = 0.5f;
        private float m_fBeastVoiceValue = 1f;
        private GraphicsQuality m_eGraphicsQuality = GraphicsQuality.Medium;
        private string m_strRoomName = string.Empty;
        private string m_strRoomPW = string.Empty;
        private int m_nRoomControlMode = 3;
        private uint m_unRoomMapID = 0u;
        private int m_nRoomHeroSelectMode = 0;
        private int m_nRoomActionMode = 0;
        private bool m_bFollowHero = false;
        private bool m_bCombatlog = true;
        private bool m_bNotOpenResignPrompt = false;
        private static UserOptions s_instance = null;
        private IXLog m_log = XLog.GetLog<UserOptions>();
        public static UserOptions Singleton
        {
            get
            {
                if (UserOptions.s_instance == null)
                {
                    UserOptions.s_instance = new UserOptions();
                    UserOptions.s_instance.Init();
                }
                return UserOptions.s_instance;
            }
        }
        public bool HasSet
        {
            get
            {
                return this.m_bHasSet;
            }
        }
        public bool IsRememberAccount
        {
            get
            {
                return this.m_bIsRememberAccount;
            }
            set
            {
                this.m_bIsRememberAccount = value;
            }
        }
        public string UserId
        {
            get
            {
                return this.m_strUserId;
            }
            set
            {
                this.m_strUserId = value;
            }
        }
        public string ServerName
        {
            get
            {
                return this.m_strServerName;
            }
            set
            {
                this.m_strServerName = value;
            }
        }
        public int DisplayMode
        {
            get
            {
                return this.m_nDisplayMode;
            }
            set
            {
                this.m_nDisplayMode = value;
            }
        }
        public IGraphicsResolution Resolution
        {
            get
            {
                return this.m_IGraphicsResolution;
            }
            set
            {
                this.m_IGraphicsResolution = value;
            }
        }
        public bool MuteSound
        {
            get
            {
                return this.m_bMuteSound;
            }
            set
            {
                this.m_bMuteSound = value;
            }
        }
        public float BGSoundValue
        {
            get
            {
                return this.m_fBGSoundValue;
            }
            set
            {
                this.m_fBGSoundValue = value;
            }
        }
        public float UISoundValue
        {
            get
            {
                return this.m_fUISoundValue;
            }
            set
            {
                this.m_fUISoundValue = value;
            }
        }
        public float SoundValue
        {
            get
            {
                return this.m_fSoundValue;
            }
            set
            {
                this.m_fSoundValue = value;
            }
        }
        public float BeastVoiceValue
        {
            get
            {
                return this.m_fBeastVoiceValue;
            }
            set
            {
                this.m_fBeastVoiceValue = value;
            }
        }
        public GraphicsQuality Quality
        {
            get
            {
                return this.m_eGraphicsQuality;
            }
            set
            {
                this.m_eGraphicsQuality = value;
            }
        }
        public string RoomName
        {
            get
            {
                return this.m_strRoomName;
            }
            set
            {
                this.m_strRoomName = value;
            }
        }
        public string RoomPassword
        {
            get
            {
                return this.m_strRoomPW;
            }
            set
            {
                this.m_strRoomPW = value;
            }
        }
        public int RoomControlMode
        {
            get
            {
                return this.m_nRoomControlMode;
            }
            set
            {
                this.m_nRoomControlMode = value;
            }
        }
        public uint RoomMapID
        {
            get
            {
                return this.m_unRoomMapID;
            }
            set
            {
                this.m_unRoomMapID = value;
            }
        }
        public int RoomHeroSelectMode
        {
            get
            {
                return this.m_nRoomHeroSelectMode;
            }
            set
            {
                this.m_nRoomHeroSelectMode = value;
            }
        }
        public int RoomActionMode
        {
            get
            {
                return this.m_nRoomActionMode;
            }
            set
            {
                this.m_nRoomActionMode = value;
            }
        }
        public bool FollowRole
        {
            get
            {
                return this.m_bFollowHero;
            }
            set
            {
                this.m_bFollowHero = value;
            }
        }
        public bool Combatlog
        {
            get
            {
                return this.m_bCombatlog;
            }
            set
            {
                this.m_bCombatlog = value;
            }
        }
        public bool NotOpenResignPrompt
        {
            get
            {
                return this.m_bNotOpenResignPrompt;
            }
            set
            {
                this.m_bNotOpenResignPrompt = value;
            }
        }
        public UserOptions()
        {
            this.m_IGraphicsResolution = GraphicsManager.Default;
        }
        public void Init()
        {
            try
            {
                this.FollowRole = GameConfig.singleton.FollowCamera;
                this.LoadUserConfig();
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        public void SaveUserConfig()
        {
            string fullPath = ResourceManager.GetFullPath("config/user/useroptionscfg.xml", false);
            string directoryName = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(newChild);
            XmlElement xmlElement = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(xmlElement);
            XmlElement xmlElement2 = xmlDocument.CreateElement("element");
            xmlElement2.SetAttribute("id", "rememberaccount");
            xmlElement2.SetAttribute("value", this.m_bIsRememberAccount ? 1.ToString() : 0.ToString());
            xmlElement.AppendChild(xmlElement2);
            XmlElement xmlElement3 = xmlDocument.CreateElement("element");
            xmlElement3.SetAttribute("id", "userid");
            xmlElement3.SetAttribute("value", this.m_bIsRememberAccount ? this.m_strUserId : "");
            xmlElement.AppendChild(xmlElement3);
            XmlElement xmlElement4 = xmlDocument.CreateElement("element");
            xmlElement4.SetAttribute("id", "servername");
            xmlElement4.SetAttribute("value", this.m_strServerName);
            xmlElement.AppendChild(xmlElement4);
            XmlElement xmlElement5 = xmlDocument.CreateElement("element");
            xmlElement5.SetAttribute("id", "displaymode");
            xmlElement5.SetAttribute("value", this.m_nDisplayMode.ToString());
            xmlElement.AppendChild(xmlElement5);
            XmlElement xmlElement6 = xmlDocument.CreateElement("element");
            xmlElement6.SetAttribute("id", "resolution");
            xmlElement6.SetAttribute("value", string.Format("{0},{1}", this.m_IGraphicsResolution.Width, this.m_IGraphicsResolution.Height));
            xmlElement.AppendChild(xmlElement6);
            XmlElement xmlElement7 = xmlDocument.CreateElement("element");
            xmlElement7.SetAttribute("id", "mutesound");
            xmlElement7.SetAttribute("value", this.m_bMuteSound ? 1.ToString() : 0.ToString());
            xmlElement.AppendChild(xmlElement7);
            XmlElement xmlElement8 = xmlDocument.CreateElement("element");
            xmlElement8.SetAttribute("id", "bgsoundvalue");
            xmlElement8.SetAttribute("value", this.m_fBGSoundValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement8);
            XmlElement xmlElement9 = xmlDocument.CreateElement("element");
            xmlElement9.SetAttribute("id", "uisoundvalue");
            xmlElement9.SetAttribute("value", this.m_fUISoundValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement9);
            XmlElement xmlElement10 = xmlDocument.CreateElement("element");
            xmlElement10.SetAttribute("id", "soundvalue");
            xmlElement10.SetAttribute("value", this.m_fSoundValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement10);
            XmlElement xmlElement11 = xmlDocument.CreateElement("element");
            xmlElement11.SetAttribute("id", "herovoicevalue");
            xmlElement11.SetAttribute("value", this.m_fBeastVoiceValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement11);
            XmlElement xmlElement12 = xmlDocument.CreateElement("element");
            xmlElement12.SetAttribute("id", "qualitysetting");
            xmlElement12.SetAttribute("value", string.Format("{0}", (int)this.m_eGraphicsQuality));
            xmlElement.AppendChild(xmlElement12);
            XmlElement xmlElement13 = xmlDocument.CreateElement("element");
            xmlElement13.SetAttribute("id", "roomname");
            xmlElement13.SetAttribute("value", this.m_strRoomName);
            xmlElement.AppendChild(xmlElement13);
            XmlElement xmlElement14 = xmlDocument.CreateElement("element");
            xmlElement14.SetAttribute("id", "roompassword");
            xmlElement14.SetAttribute("value", this.m_strRoomPW);
            xmlElement.AppendChild(xmlElement14);
            XmlElement xmlElement15 = xmlDocument.CreateElement("element");
            xmlElement15.SetAttribute("id", "roomcontrolmode");
            xmlElement15.SetAttribute("value", string.Format("{0}", this.m_nRoomControlMode));
            xmlElement.AppendChild(xmlElement15);
            XmlElement xmlElement16 = xmlDocument.CreateElement("element");
            xmlElement16.SetAttribute("id", "roommapid");
            xmlElement16.SetAttribute("value", string.Format("{0}", this.m_unRoomMapID));
            xmlElement.AppendChild(xmlElement16);
            XmlElement xmlElement17 = xmlDocument.CreateElement("element");
            xmlElement17.SetAttribute("id", "roomheroselectmode");
            xmlElement17.SetAttribute("value", string.Format("{0}", this.m_nRoomHeroSelectMode));
            xmlElement.AppendChild(xmlElement17);
            XmlElement xmlElement18 = xmlDocument.CreateElement("element");
            xmlElement18.SetAttribute("id", "roomactionmode");
            xmlElement18.SetAttribute("value", string.Format("{0}", this.m_nRoomActionMode));
            xmlElement.AppendChild(xmlElement18);
            XmlElement xmlElement19 = xmlDocument.CreateElement("element");
            xmlElement19.SetAttribute("id", "followrole");
            xmlElement19.SetAttribute("value", this.m_bFollowHero ? 1.ToString() : 0.ToString());
            xmlElement.AppendChild(xmlElement19);
            XmlElement xmlElement20 = xmlDocument.CreateElement("element");
            xmlElement20.SetAttribute("id", "opencombatlog");
            xmlElement20.SetAttribute("value", this.m_bCombatlog ? 1.ToString() : 0.ToString());
            xmlElement.AppendChild(xmlElement20);
            XmlElement xmlElement21 = xmlDocument.CreateElement("element");
            xmlElement21.SetAttribute("id", "notopenresignprompt");
            xmlElement21.SetAttribute("value", this.m_bNotOpenResignPrompt ? 1.ToString() : 0.ToString());
            xmlElement.AppendChild(xmlElement21);
            xmlDocument.Save(fullPath);
        }
        private void LoadUserConfig()
        {
            string fullPath = ResourceManager.GetFullPath("config/user/useroptionscfg.xml", false);
            if (File.Exists(fullPath))
            {
                using (XmlReader xmlReader = XmlReader.Create(fullPath))
                {
                    if (null != xmlReader)
                    {
                        this.m_bHasSet = true;
                        while (xmlReader.Read())
                        {
                            if (xmlReader.Name == "element" && xmlReader.NodeType == XmlNodeType.Element)
                            {
                                string text = xmlReader.GetAttribute("id").ToLower();
                                string attribute = xmlReader.GetAttribute("value");
                                string text2 = text;
                                switch (text2)
                                {
                                    case "rememberaccount":
                                        this.m_bIsRememberAccount = (Convert.ToInt32(attribute) != 0);
                                        break;
                                    case "userid":
                                        this.m_strUserId = attribute;
                                        break;
                                    case "servername":
                                        this.m_strServerName = attribute;
                                        break;
                                    case "displaymode":
                                        this.m_nDisplayMode = Convert.ToInt32(attribute);
                                        break;
                                    case "resolution":
                                        {
                                            string[] array = attribute.Split(new char[]
									{
										','
									});
                                            if (array.Length == 2)
                                            {
                                                int nWidth = Convert.ToInt32(array[0]);
                                                int nHeight = Convert.ToInt32(array[1]);
                                                this.m_IGraphicsResolution = GraphicsManager.CreateResolution(nWidth, nHeight);
                                            }
                                            break;
                                        }
                                    case "mutesound":
                                        this.m_bMuteSound = (Convert.ToInt32(attribute) != 0);
                                        break;
                                    case "bgsoundvalue":
                                        this.m_fBGSoundValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "uisoundvalue":
                                        this.m_fUISoundValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "soundvalue":
                                        this.m_fSoundValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "herovoicevalue":
                                        this.m_fBeastVoiceValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "roomname":
                                        this.m_strRoomName = attribute;
                                        break;
                                    case "roompassword":
                                        this.m_strRoomPW = attribute;
                                        break;
                                    case "roomcontrolmode":
                                        this.m_nRoomControlMode = Convert.ToInt32(attribute);
                                        break;
                                    case "roommapid":
                                        this.m_unRoomMapID = Convert.ToUInt32(attribute);
                                        break;
                                    case "roomheroselectmode":
                                        this.m_nRoomHeroSelectMode = Convert.ToInt32(attribute);
                                        break;
                                    case "roomactionmode":
                                        this.m_nRoomActionMode = Convert.ToInt32(attribute);
                                        break;
                                    case "followrole":
                                        this.m_bFollowHero = (Convert.ToInt32(attribute) != 0);
                                        break;
                                    case "opencombatlog":
                                        this.m_bCombatlog = (Convert.ToInt32(attribute) != 0);
                                        break;
                                    case "notopenresignprompt":
                                        this.m_bNotOpenResignPrompt = (Convert.ToInt32(attribute) != 0);
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
