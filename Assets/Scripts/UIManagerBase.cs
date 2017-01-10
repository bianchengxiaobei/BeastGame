using UnityEngine;
using Utility;
using Client.UI.UICommon;
using Client.UI.UICommon.Local;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UIManagerBase
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.20
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Export
{
    public class UIManagerBase
    {
        /// <summary>
        /// NGUI的UICamera
        /// </summary>
        public Camera UICamera
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetUICamera();
            }
        }
        public bool IsAnyTouchInUI
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetIsAnyTouchInUI();
            }
        }
        public int CurrentTouchID
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetCurrentTouchID();
            }
        }
        public UICamera.MouseOrTouch CurrentTouch
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetMouseOrTouch();
            }
        }
        public bool IsInputHasFocus
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetIsInputHasFocus();
            }
        }
        public bool IsInOpState
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetIsInOpState();
            }
            set
            {
                Singleton<LocalUIManagerBase>.singleton.SetIsInOpState(value);
            }
        }
        public bool IsEventProcessed
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetIsEventProcessed();
            }
            set
            {
                Singleton<LocalUIManagerBase>.singleton.SetEventProcessed(value);
            }
        }
        public float SoundVolume
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetSoundVolume();
            }
            set
            {
                Singleton<LocalUIManagerBase>.singleton.SetSoundVolume(value);
            }
        }
        public Transform UIRoot
        {
            get
            {
                return Singleton<LocalUIManagerBase>.singleton.GetUIRoot();
            }
        }
        /// <summary>
        /// 初始化UIManager,因为UImanager都是底层LocalUIManagerBase来执行的
        /// 主要初始化UITool，UIRoot，HighlightCamera
        /// </summary>
        /// <param name="uiTool"></param>
        /// <param name="uiRoot"></param>
        /// <returns></returns>
        public virtual bool Init(IXUITool uiTool, Transform uiRoot)
        {
            return Singleton<LocalUIManagerBase>.singleton.Init(uiTool, uiRoot);
        }
        public void SetLayer(GameObject go, int layer)
        {
            Singleton<LocalUIManagerBase>.singleton.SetLayer(go, layer);
        }
        public void SetFocus(IXUIObject uiObject)
        {
            Singleton<LocalUIManagerBase>.singleton.SetFocus(uiObject);
        }
        /// <summary>
        /// 添加ui到缓存中，dicui,listui,listlayerui
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        public bool AddDlg(IXUIDlg dlg)
        {
            return Singleton<LocalUIManagerBase>.singleton.AddDlg(dlg);
        }
        public void SetVisible(GameObject uiGameObject, bool bVisible)
        {
            Singleton<LocalUIManagerBase>.singleton.SetVisible(uiGameObject, bVisible);
        }
        protected virtual void OnAddListItem(IXUIDlg uiDlg, IXUIList uiList, IXUIListItem uiListItem)
        {
        }
        public virtual bool OnUIObjectEventFilter(IXUIObject uiObject, string funcName)
        {
            return true;
        }
        public void Clear()
        {
            Singleton<LocalUIManagerBase>.singleton.Clear();
        }
        public void CloseAllDlg()
        {
            Singleton<LocalUIManagerBase>.singleton.CloseAllDlg();
        }
        public void CloseAllDlg(uint unDlgType)
        {
            this.CloseAllDlg(unDlgType, 0u);
        }
        public void CloseAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            Singleton<LocalUIManagerBase>.singleton.CloseAllDlg(unDlgType, unDlgTypeExclude);
        }
        public void ShowAllDlg()
        {
            Singleton<LocalUIManagerBase>.singleton.ShowAllDlg();
        }
        public void ResetAllDlg(uint unDlgType)
        {
            this.ResetAllDlg(unDlgType, 0u);
        }
        public void ResetAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            Singleton<LocalUIManagerBase>.singleton.ResetAllDlg(unDlgType, unDlgTypeExclude);
        }
        public void UnLoadAllDlg(uint unDlgType)
        {
            this.UnLoadAllDlg(unDlgType, 0u);
        }
        public void UnLoadAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            Singleton<LocalUIManagerBase>.singleton.UnLoadAllDlg(unDlgType, unDlgTypeExclude);
        }
        public void LoadAllDlg(uint unDlgType)
        {
            this.LoadAllDlg(unDlgType, 0u);
        }
        public void LoadAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            Singleton<LocalUIManagerBase>.singleton.LoadAllDlg(unDlgType, unDlgTypeExclude);
        }
        public void OnFinishChangeState()
        {
            Singleton<LocalUIManagerBase>.singleton.OnFinishChangeState();
        }
        public void Compositor(IXUIDlg dlg)
        {
            Singleton<LocalUIManagerBase>.singleton.Compositor(dlg);
        }
        /// <summary>
        /// 执行当前场景中存在的XUIDlg界面的update
        /// </summary>
        public void Update()
        {
            Singleton<LocalUIManagerBase>.singleton.Update();
        }
        /// <summary>
        /// 执行当前场景中存在的XUIDlg界面的fixedupdate
        /// </summary>
        public void FixedUpdate()
        {
            Singleton<LocalUIManagerBase>.singleton.FixedUpdate();
        }
        public void ShowTooltip(GameObject obj, bool bShow)
        {
            Singleton<LocalUIManagerBase>.singleton.ShowTooltip(obj, bShow);
        }
        public void ResetCurTouchState()
        {
            Singleton<LocalUIManagerBase>.singleton.ResetCurTouchState();
        }
        public void PlayAnim(Animation anim, string strClipName, AnimFinishedEventHandler eventHandler)
        {
            Singleton<LocalUIManagerBase>.singleton.PlayAnim(anim, strClipName, eventHandler);
        }
        public void BeginTweenAlpha(GameObject go, float duration, float alpha)
        {
            Singleton<LocalUIManagerBase>.singleton.BeginTweenAlpha(go, duration, alpha);
        }
        public void BeginTweenPosition(GameObject go, float duration, Vector3 pos)
        {
            Singleton<LocalUIManagerBase>.singleton.BeginTweenPosition(go, duration, pos);
        }
        public void BeginTweenScale(GameObject go, float duration, Vector3 scale)
        {
            Singleton<LocalUIManagerBase>.singleton.BeginTweenScale(go, duration, scale);
        }
        public void BeginTweenRotation(GameObject go, float duration, Quaternion rot)
        {
            Singleton<LocalUIManagerBase>.singleton.BeginTweenRotation(go, duration, rot);
        }
        public void BeginTweenVolume(GameObject go, float duration, float targetVolume)
        {
            Singleton<LocalUIManagerBase>.singleton.BeginTweenVolume(go, duration, targetVolume);
        }
        public void SetColor(GameObject obj, Color color)
        {
            Singleton<LocalUIManagerBase>.singleton.SetColor(obj, color);
        }
        public void OnResolutionChange()
        {
            Singleton<LocalUIManagerBase>.singleton.OnResolutionChange();
        }
        public IXUIDlg GetDlg(string strDlgName)
        {
            return Singleton<LocalUIManagerBase>.singleton.GetDlg(strDlgName);
        }
        public IXUIObject GetUIObject(string strUIObjectId)
        {
            return Singleton<LocalUIManagerBase>.singleton.GetUIObject(strUIObjectId);
        }
        public string GetUIObjectId(IXUIObject uiObject)
        {
            return WidgetFactory.GetUIObjectId(uiObject);
        }
        protected void RegisterUIEventHandler(GameObject gameObject)
        {
            Singleton<LocalUIManagerBase>.singleton.RegisterUIEventHandler(gameObject);
        }
        protected void RegisterLoadResAsynEventHandler(LoadTextureAsynEventHandler action)
        {
            Singleton<LocalUIManagerBase>.singleton.RegisterLoadResAsynEventHandler(action);
        }
        public void RegisterTipShowEventHandler(TipShowEventHandler eventHandler)
        {
            Singleton<LocalUIManagerBase>.singleton.RegisterTipShowEventHandler(eventHandler);
        }
        public void RegisterTipGetterEventHandler(TipGetterEventHandler eventHandler)
        {
            Singleton<LocalUIManagerBase>.singleton.RegisterTipGetterEventHandler(eventHandler);
        }
        public void RegisterAddListItemEventHandler(AddListItemEventHandler eventHandler)
        {
            Singleton<LocalUIManagerBase>.singleton.RegisterAddListItemEventHandler(eventHandler);
        }
    }
}
