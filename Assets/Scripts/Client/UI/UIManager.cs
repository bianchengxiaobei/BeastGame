using UnityEngine;
using System.Collections;
using Client;
using Client.Common;
using Client.UI.UICommon;
using UILib.Export;
using Utility.Export;
using UnityAssetEx.Export;
using GameClient.Data;
using Utility;
using GameClient.UI;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UIManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.20
// 模块描述：UI管理器
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class UIManager : UIManagerBase, IUIManager
    {
        #region 字段
        private Camera m_UIHightLightCamera = null;
        private float m_fLastExpressionTime = 0f;
        private enumCursorType m_eCursorTypeLast = enumCursorType.eCursorType_Normal;
        private static UIManager s_instance = null;
        private bool m_bLButtonPressed = false;
        private bool m_bShow3DUI = false;
        private IXLog m_log = XLog.GetLog<UIManager>();
        #endregion
        #region 属性
        public static UIManager singleton
        {
            get
            {
                if (null == UIManager.s_instance)
                {
                    UIManager.s_instance = new UIManager();
                }
                return UIManager.s_instance;
            }
        }
        public bool LButtonPressed
        {
            get
            {
                return this.m_bLButtonPressed;
            }
        }
        public bool Show3DUI
        {
            set
            {
                this.m_bShow3DUI = value;
            }
        }
        public float lastEnumbbleTime
        {
            get
            {
                return this.m_fLastExpressionTime;
            }
            set
            {
                this.m_fLastExpressionTime = value;
            }
        }
        #endregion
        #region 公有方法
        /// <summary>
        /// 初始化UIManager，注册UI事件监听，和UIHighlightCamera
        /// </summary>
        /// <param name="uiTool"></param>
        /// <param name="uiRoot"></param>
        /// <returns></returns>
        public override bool Init(IXUITool uiTool, Transform uiRoot)
        {
            bool result;
            if (null == uiTool || null == uiRoot)
            {
                result = false;
            }
            else 
            {
                if (!base.Init(uiTool, uiRoot))
                {
                    this.m_log.Error("false == base.Init(uiTool,uiRoot)");
                    result = false;
                }
                else 
                {
                    base.RegisterUIEventHandler(UnityGameEntry.Instance.gameObject);
                    base.RegisterLoadResAsynEventHandler(new LoadTextureAsynEventHandler(ResourceManager.singleton.LoadTexture));
                    base.RegisterTipShowEventHandler(new TipShowEventHandler(this.TipShow));
                    base.RegisterTipGetterEventHandler(new TipGetterEventHandler(this.GetTip));
                    base.RegisterAddListItemEventHandler(new AddListItemEventHandler(this.OnAddListItem));
                    XUIObjectBase.RegisterClickFilter(new FilterUIEventHandler(this.OnUIObjectEventFilter));
                    XLog.Log.Debug("UIManager Init Success!");
                    Transform transform = uiRoot.transform.parent.Find("CameraHighLight");
                    if (transform != null)
                    {
                        this.m_UIHightLightCamera = transform.gameObject.GetComponent<Camera>();
                    }
                    result = true;
                }
            }
            return result;
        }
        public void OnDlgShow(IXUIDlg uiDlg)
        {
          
        }
        public void TipShow(bool bShow, IXUIObject uiObject)
        {
 
        }
        public string GetTip(string strUIObjectId)
        {
            return Singleton<UITipConfigMgr>.singleton.GetTip(strUIObjectId);
        }
        /// <summary>
        /// 设置鼠标icon图标类型
        /// </summary>
        /// <param name="eCursorType"></param>
        public void SetCursor(enumCursorType eCursorType)
        {
            Singleton<CursorManager>.singleton.LeaveCursor(this.m_eCursorTypeLast);
            Singleton<CursorManager>.singleton.EnterCursor(eCursorType);
            this.m_eCursorTypeLast = eCursorType;
        }
        /// <summary>
        /// 取得图集名称
        /// </summary>
        /// <param name="eAtlasType"></param>
        /// <param name="stringId"></param>
        /// <returns></returns>
        public string GetAtlasName(EnumAtlasType eAtlasType, string stringId)
        {
            int id;
            string result = string.Empty;
            try
            {
                bool flag = int.TryParse(stringId, out id);
                if (flag)
                {
                    result = this.GetAtlasNameById(eAtlasType, id);
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
                Debug.LogException(e);
            }
            return result;
        }
        #endregion
        #region 私有方法
        private string GetAtlasNameById(EnumAtlasType eAtlasType,int id)
        {
            string result = string.Empty;
            switch (eAtlasType)
            {
                case EnumAtlasType.Beast:
                    if (id >= 0 && id <= 40)
                    {
                        result = string.Format("Atlas/BeastIcon/{0}", "BeastAvatarIcon");                       
                    }
                    break;
                case EnumAtlasType.Skill:
                    if (id >= 0 && id <= 30)
                    {
                        result = string.Format("Atlas/SkillIcon/Exproler/{0}", "ExprolerSkillIcon");
                    }
                    if (id >= 10000 && id <= 10100)
                    {
                        result = string.Format("Atlas/SkillIcon/Exproler/{0}", "ExprolerSkillIcon");
                    }
                    break;
            }
            return result;
        }
        #endregion
    }
}
