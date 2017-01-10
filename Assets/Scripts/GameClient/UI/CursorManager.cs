using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
using Client.Common;
using Client.UI;
using UnityAssetEx.Export;
using GameClient;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CursorManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：鼠标管理器
//----------------------------------------------------------------*/
#endregion
namespace GameClient.UI
{
    internal class CursorManager : Singleton<CursorManager>
    {
        /// <summary>
        /// 鼠标类型，初始化为Normal
        /// </summary>
        private enumCursorType m_eCursorType = enumCursorType.eCursorType_Normal;
        private Dictionary<string, Texture2D> m_dicCursorTexture = new Dictionary<string, Texture2D>();
        private HashSet<string> m_setLoadingTexture = new HashSet<string>();
        private List<enumCursorType> m_listCachedCursorType = new List<enumCursorType>();
        private IXLog m_log = XLog.GetLog<CursorManager>();
        public enumCursorType CurrentCursorType
        {
            get
            {
                return this.m_eCursorType;
            }
        }
        /// <summary>
        /// 构造函数，默认为normal类型
        /// </summary>
        public CursorManager()
        {
            this.m_eCursorType = enumCursorType.eCursorType_Normal;
            this.m_listCachedCursorType.Add(this.m_eCursorType);
        }
        /// <summary>
        /// 如果是手机平台就不显示鼠标，否则显示
        /// </summary>
        public void Awake()
        {
            if (CommonDefine.IsMobilePlatform)
            {
                this.HideCursor();
            }
            else
            {
                this.ShowCursor();
            }
        }
        /// <summary>
        /// 进入这个鼠标类型
        /// </summary>
        /// <param name="eCursorType"></param>
        public void EnterCursor(enumCursorType eCursorType)
        {
            if (!CommonDefine.IsMobilePlatform)
            {
                if (!this.m_listCachedCursorType.Contains(eCursorType))//缓存鼠标不包括这个鼠标类型就加进去
                {
                    this.m_listCachedCursorType.Add(eCursorType);
                    this.m_listCachedCursorType.Sort();
                    this.m_eCursorType = this.m_listCachedCursorType[this.m_listCachedCursorType.Count - 1];
                    if (this.m_listCachedCursorType.Count > 10)//如果超过10个就报错
                    {
                        this.m_log.Error("m_listCachedCursorType.Count > 10:" + this.m_listCachedCursorType.Count);
                    }
                    this.RefreshCursor();
                }
                else
                {
                    //鼠标进入已经进入的鼠标报错
                    this.m_log.Error(string.Format("m_listCachedCursorType.Contains(eCursorType) == true:{0}", eCursorType));
                }
            }
        }
        /// <summary>
        /// 离开这个鼠标类型
        /// </summary>
        /// <param name="eCursorType"></param>
        public void LeaveCursor(enumCursorType eCursorType)
        {
            if (!CommonDefine.IsMobilePlatform)
            {
                if (!this.m_listCachedCursorType.Contains(eCursorType))
                {
                    this.m_log.Error(string.Format("m_listCachedCursorType.Contains(eCursorType) == false:{0}", eCursorType));
                }
                else
                {
                    this.m_listCachedCursorType.Remove(eCursorType);
                    if (this.m_listCachedCursorType.Count > 0)
                    {
                        this.m_eCursorType = this.m_listCachedCursorType[this.m_listCachedCursorType.Count - 1];
                        this.RefreshCursor();
                    }
                    else
                    {
                        this.m_eCursorType = enumCursorType.eCursorType_Default;
                    }
                }
            }
        }
        /// <summary>
        /// 根据现在的鼠标类型刷新鼠标
        /// </summary>
        public void RefreshCursor()
        {
            if (!CommonDefine.IsMobilePlatform)
            {
                string text = "";
                Vector2 zero = Vector2.zero;
                switch (this.m_eCursorType)
                {
                    case enumCursorType.eCursorType_Default:
                    case enumCursorType.eCursorType_Normal:
                        if (!UIManager.singleton.LButtonPressed)
                        {
                            text = "Texture/Cursor/hand";
                        }
                        else
                        {
                            text = "Texture/Cursor/down";
                        }
                        break;
                    case enumCursorType.eCursorType_TicketCharge:
                        if (!UIManager.singleton.LButtonPressed)
                        {
                            text = "Texture/Cursor/RealMoney";
                        }
                        else
                        {
                            text = "Texture/Cursor/RealMoney_Down";
                        }
                        break;
                    case enumCursorType.eCursorType_MoneyCharge:
                        if (!UIManager.singleton.LButtonPressed)
                        {
                            text = "Texture/Cursor/GameMoney";
                        }
                        else
                        {
                            text = "Texture/Cursor/GameMoney_Down";
                        }
                        break;
                    case enumCursorType.eCursorType_LeftPage:
                        text = "Texture/Cursor/pageleft";
                        break;
                    case enumCursorType.eCursorType_RightPage:
                        text = "Texture/Cursor/pageright";
                        break;
                    case enumCursorType.eCursorType_Disable:
                        text = "Texture/Cursor/roundoutside";
                        break;
                    case enumCursorType.eCursorType_Highlight:
                        text = "Texture/Cursor/hand_highlight";
                        break;
                    case enumCursorType.eCursorType_Drag:
                        text = "Texture/Cursor/drag";
                        break;
                    case enumCursorType.eCursorType_Attack:
                        text = "Texture/Cursor/selected";
                        break;
                    case enumCursorType.eCursorType_Move:
                        text = "Texture/Cursor/move";
                        break;
                }
                if (!string.IsNullOrEmpty(text))
                {
                    SCursorItem cursorItem = Singleton<CursorConfigMgr>.singleton.GetCursorItem(text);
                    if (cursorItem.Software)
                    {
                        this.SetCursor(text, cursorItem.Hotspot, CursorMode.ForceSoftware);
                    }
                    else
                    {
                        this.SetCursor(text, cursorItem.Hotspot, CursorMode.Auto);
                    }
                }
            }
        }
        private void SetCursor(string strTextureFile, Vector2 hotspot, CursorMode cursorMode)
        {
            Texture2D texture = null;
            if (this.m_dicCursorTexture.TryGetValue(strTextureFile, out texture))
            {
                Cursor.SetCursor(texture, hotspot, cursorMode);
            }
            else
            {
                if (!this.m_setLoadingTexture.Contains(strTextureFile))
                {
                    this.m_setLoadingTexture.Add(strTextureFile);
                    IAssetRequest assetRequest = ResourceManager.singleton.LoadTexture(strTextureFile, new AssetRequestFinishedEventHandler(this.OnLoadCursorTextureFinished), AssetPRI.DownloadPRI_High);
                    assetRequest.Data = strTextureFile;
                }
            }
        }
        private void OnLoadCursorTextureFinished(IAssetRequest assetRequest)
        {
            IAssetResource assetResource = assetRequest.AssetResource;
            if (null != assetResource)
            {
                UnityEngine.Object mainAsset = assetResource.MainAsset;
                Texture2D texture2D = mainAsset as Texture2D;
                if (!(null == texture2D))
                {
                    string key = assetRequest.Data as string;
                    this.m_dicCursorTexture[key] = texture2D;
                    this.RefreshCursor();
                }
            }
        }
        private void ShowCursor()
        {
            Screen.showCursor = true;
        }
        private void HideCursor()
        {
            Screen.showCursor = false;
        }
    }
}