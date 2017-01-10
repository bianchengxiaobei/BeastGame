using UnityEngine;
using System;
using System.Collections.Generic;
using Utility.Export;
using Utility;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UIManagerBase
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.20
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon.Local
{
    internal class LocalUIManagerBase : Singleton<LocalUIManagerBase>
    {
        private Camera m_highlightCamera;
        private IXUITool m_uiTool;
        private Transform m_uiRoot;
        private Dictionary<string, IXUIDlg> m_dicDlgs = new Dictionary<string, IXUIDlg>();
        private Dictionary<int, List<IXUIDlg>> m_dicUILayer = new Dictionary<int, List<IXUIDlg>>();
        private List<IXUIDlg> m_listDlg = new List<IXUIDlg>();
        private IXLog m_log = new Logger("UIManagerBase");

        #region 方法
        /// <summary> 
        /// 初始化UIManager,因为UImanager都是底层LocalUIManagerBase来执行的
        /// 主要初始化UITool，UIRoot，HighlightCamera
        /// </summary>
        /// <param name="uiTool"></param>
        /// <param name="uiRoot"></param>
        /// <returns></returns>
        public  bool Init(IXUITool uiTool, Transform uiRoot)
        {
            if (uiTool == null || null == uiRoot)
            {
                return false;
            }
            this.m_uiTool = uiTool;
            this.m_uiRoot = uiRoot;
            this.m_log.Debug("UIManager Init Success!");
            Transform transform = uiRoot.transform.parent.Find("CameraHighLight");
            if (transform != null)
            {
                this.m_highlightCamera = transform.gameObject.GetComponent<Camera>();
            }
            return true;
        }
        /// <summary>
        /// 设置ui物体层级
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layer"></param>
        public void SetLayer(GameObject go, int layer)
        {
            this.m_uiTool.SetLayer(go, layer);
        }
        public UICamera.MouseOrTouch GetMouseOrTouch()
        {
            return this.m_uiTool.CurrentTouch;
        }
        public void SetFocus(IXUIObject uiObject)
        {
            this.m_uiTool.SetFocus(uiObject);
        }
        /// <summary>
        /// 添加ui界面到缓存中
        /// </summary>
        /// <param name="uiDlg"></param>
        /// <returns></returns>
        public bool AddDlg(IXUIDlg uiDlg)
        {
            if (this.m_dicDlgs.ContainsKey(uiDlg.fileName))
            {
                this.m_log.Debug("true == m_dicDlgs.ContainsKey(dlg.fileName): " + uiDlg.fileName);
                return false;
            }
            StackTraceUtility.ExtractStackTrace();
            this.m_dicDlgs.Add(uiDlg.fileName, uiDlg);
            this.m_listDlg.Add(uiDlg);
            List<IXUIDlg> list = null;
            if (this.m_dicUILayer.TryGetValue(uiDlg.layer, out list))
            {
                list.Add(uiDlg);
            }
            else 
            {
                list = new List<IXUIDlg>();
                list.Add(uiDlg);
                this.m_dicUILayer.Add(uiDlg.layer, list);
            }
            return true;
        }
        /// <summary>
        /// 设置ui物体是否可见
        /// </summary>
        /// <param name="uiGameObject"></param>
        /// <param name="bVisible"></param>
        public void SetVisible(GameObject uiGameObject, bool bVisible)
        {
            if (this.m_uiTool != null && null != uiGameObject)
            {
                this.m_uiTool.SetActive(uiGameObject, bVisible);
            }
        }
        /// <summary>
        /// 清除屏幕上的所有可见ui界面
        /// </summary>
        public void Clear()
        {
            foreach (IXUIDlg current in this.m_listDlg)
            {
                current.SetVisible(false);
                current.UnLoad();
            }
            this.m_dicDlgs.Clear();
            this.m_listDlg.Clear();
            this.m_dicUILayer.Clear();
        }
        /// <summary>
        /// 关闭所有该类型的ui物体（实际上是掩藏ui物体）
        /// </summary>
        /// <param name="unDlgType"></param>
        /// <param name="unDlgTypeExclude"></param>
        public void CloseAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            foreach (IXUIDlg current in this.m_dicDlgs.Values)
            {
                if ((current.Type & unDlgType) > 0u && (current.Type & unDlgTypeExclude) == 0u)
                {
                    current.SetVisible(false);
                }
            }
        }
        /// <summary>
        /// 关闭所有的UI界面
        /// </summary>
        public void CloseAllDlg()
        {
            foreach (var current in this.m_dicDlgs.Values)
            {
                current.SetVisible(false);
            }
        }
        public void ShowAllDlg()
        {
            foreach (var current in this.m_dicDlgs.Values)
            {
                current.SetVisible(true);
            }
        }
        /// <summary>
        /// 重新设置ui界面
        /// </summary>
        /// <param name="unDlgType"></param>
        /// <param name="unDlgTypeExclude"></param>
        public void ResetAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            foreach (IXUIDlg current in this.m_dicDlgs.Values)
            {
                if ((current.Type & unDlgType) > 0u && (current.Type & unDlgTypeExclude) == 0u)
                {
                    current.Reset();
                }
            }
        }
        /// <summary>
        /// 卸载ui界面
        /// </summary>
        /// <param name="unDlgType"></param>
        /// <param name="unDlgTypeExclude"></param>
        public void UnLoadAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            foreach (IXUIDlg current in this.m_dicDlgs.Values)
            {
                if ((current.Type & unDlgType) > 0u && (current.Type & unDlgTypeExclude) == 0u)
                {
                    current.UnLoad();
                }
            }
        }
        /// <summary>
        /// 加载ui界面
        /// </summary>
        /// <param name="unDlgType"></param>
        /// <param name="unDlgTypeExclude"></param>
        public void LoadAllDlg(uint unDlgType, uint unDlgTypeExclude)
        {
            foreach (IXUIDlg current in this.m_dicDlgs.Values)
            {
                if ((current.Type & unDlgType) > 0u && (current.Type & unDlgTypeExclude) == 0u)
                {
                    current.Load();
                }
            }
        }
        public void OnFinishChangeState()
        {
            foreach (IXUIDlg current in this.m_dicDlgs.Values)
            {
                current.OnFinishChangeState();
            }
        }
        public void Compositor(IXUIDlg dlg)
        {
            List<IXUIDlg> list = null;
            if (this.m_dicUILayer.TryGetValue(dlg.layer, out list))
            {
                if (list.IndexOf(dlg) != 0)
                {
                    list.Remove(dlg);
                    list.Insert(0, dlg);
                }
                int num = 0;
                using (List<IXUIDlg>.Enumerator enumerator = list.GetEnumerator())//Enumerator枚举数
                {
                    while (enumerator.MoveNext())//移到下一项
                    {
                        IXUIDlg current = enumerator.Current;
                        if (current.IsVisible())
                        {
                            current.SetDepthZ(num + 10 * dlg.layer);
                            num++;
                        }
                    }
                    return;
                }
            }
            this.m_log.Debug("m_dicUILayer.TryGetValue(dlg.layer, out listDlg) error!: " + dlg.fileName);
        }
        public void Update()
        {
            try
            {
                for (int i = 0; i < this.m_listDlg.Count; i++)
                {
                    IXUIDlg ixUIDlg = this.m_listDlg[i];
                    ixUIDlg._Update();
                }
            }
            catch(Exception e) 
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        public void FixedUpdate()
        {
            try
            {
                for (int i = 0; i < this.m_listDlg.Count; i++)
                {
                    IXUIDlg iXUIDlg = this.m_listDlg[i];
                    iXUIDlg._FixedUpdate();
                }
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        public void ShowTooltip(GameObject obj, bool bShow)
        {
            if (!bShow && this.m_uiTool != null)
            {
                this.m_uiTool.ShowTooltip(obj, bShow);
            }
        }
        public void ResetCurTouchState()
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.ResetCurTouchState();
            }
        }
        public void PlayAnim(Animation anim, string strClipName, AnimFinishedEventHandler eventHandler)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.PlayAnim(anim, strClipName, eventHandler);
            }
        }
        public void BeginTweenAlpha(GameObject go, float duration, float alpha)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.BeginTweenAlpha(go, duration, alpha);
            }
        }
        public void BeginTweenPosition(GameObject go, float duration, Vector3 pos)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.BeginTweenPosition(go, duration, pos);
            }
        }
        public void BeginTweenScale(GameObject go, float duration, Vector3 scale)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.BeginTweenScale(go, duration, scale);
            }
        }
        public void BeginTweenRotation(GameObject go, float duration, Quaternion rot)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.BeginTweenRotation(go, duration, rot);
            }
        }
        public void BeginTweenVolume(GameObject go, float duration, float targetVolume)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.BeginTweenVolume(go, duration, targetVolume);
            }
        }
        public void SetColor(GameObject obj, Color color)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.SetColor(obj, color);
            }
        }
        public void OnResolutionChange()
        {
            foreach (var current in this.m_dicDlgs.Values)
            {
                if (current.uiBehaviourInterface != null)
                {
                    current.uiBehaviourInterface.OnResolutionChange();
                }
            }
        }
        /// <summary>
        /// 根据ui名字取得ui界面
        /// </summary>
        /// <param name="strDlgName"></param>
        /// <returns></returns>
        public IXUIDlg GetDlg(string strDlgName)
        {
            if (string.IsNullOrEmpty(strDlgName))
            {
                return null;
            }
            IXUIDlg result = null;
            this.m_dicDlgs.TryGetValue(strDlgName, out result);
            return result;
        }
        /// <summary>
        /// 取得ui组件，根据ui的名字id
        /// </summary>
        /// <param name="strUIObjectId"></param>
        /// <returns></returns>
        public IXUIObject GetUIObject(string strUIObjectId)
        {
            if (string.IsNullOrEmpty(strUIObjectId))
            {
                return null;
            }
            string[] array = strUIObjectId.Split(new char[]
            {
                '#'
            });
            IXUIDlg iXUIDlg = this.GetDlg(array[0]);
            if (iXUIDlg == null || iXUIDlg.uiBehaviourInterface == null)
            {
                return null;
            }
            IXUIObject iXUIObject = iXUIDlg.uiBehaviourInterface;
            try
            {
                for (int i = 1; i < array.Length; i++)
                {
                    string strPath = array[i];
                    iXUIObject = iXUIObject.GetUIObject(strPath);//取得这个界面的组件
                    IXUIList iXUIList = iXUIObject as IXUIList;
                    if (iXUIList != null)
                    {
                        i++;
                        if (i < array.Length)
                        {
                            uint num = Convert.ToUInt32(array[i]);
                            iXUIObject = iXUIList.GetItemById(num, true);
                            if (iXUIObject == null)
                            {
                                this.m_log.Error(string.Format("null == uiObject:{0}:{1}", strPath, num));
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
            return iXUIObject;
        }
        public void RegisterUIEventHandler(GameObject gameObject)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.SetUIEventHandler(gameObject);
                return;
            }
            this.m_log.Error("null == m_uiTool");
        }
        public void RegisterLoadResAsynEventHandler(LoadTextureAsynEventHandler action)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.RegisterLoadResAsynEventHandler(action);
                return;
            }
            this.m_log.Error("null == m_uiTool");
        }
        public void RegisterTipShowEventHandler(TipShowEventHandler eventHandler)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.RegisterTipShowEventHandler(eventHandler);
                return;
            }
            this.m_log.Error("null == m_uiTool");
        }
        public void RegisterTipGetterEventHandler(TipGetterEventHandler eventHandler)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.RegisterTipGetterEventHandler(eventHandler);
                return;
            }
            this.m_log.Error("null == m_uiTool");
        }
        public void RegisterAddListItemEventHandler(AddListItemEventHandler eventHandler)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.RegisterAddListItemEventHandler(eventHandler);
                return;
            }
            this.m_log.Error("null == m_uiTool");
        }
        /// <summary>
        /// 获得当前触摸的ID
        /// </summary>
        /// <returns></returns>
        public int GetCurrentTouchID()
        {
            if (this.m_uiTool != null)
            {
                return this.m_uiTool.CurrentTouchID;
            }
            return 0;
        }
        public void SetEventProcessed(bool value)
        {
            if (this.m_uiTool != null)
            {
                this.m_uiTool.IsEventProcessed = value;
            }
        }
        public bool GetIsEventProcessed()
        {
            return this.m_uiTool != null && this.m_uiTool.IsEventProcessed;
        }
        public float GetSoundVolume()
        {
            if (this.m_uiTool != null)
            {
                return this.m_uiTool.SoundVolume;
            }
            return 1f;
        }
        public void SetSoundVolume(float value)
        {
            this.m_uiTool.SoundVolume = value;
        }
        /// <summary>
        /// 获取NGUI的UICamera
        /// </summary>
        /// <returns></returns>
        public Camera GetUICamera()
        {
            return this.m_uiTool.GetUICamera();
        }
        /// <summary>
        /// 获取是否鼠标或者手指触摸UI
        /// </summary>
        /// <returns></returns>
        public bool GetIsAnyTouchInUI()
        {
            return this.m_uiTool != null && this.m_uiTool.IsAnyTouchInUI;
        }
        public bool GetIsInputHasFocus()
        {
            return this.m_uiTool != null && this.m_uiTool.IsInputHasFocus;
        }
        public bool GetIsInOpState()
        {
            return this.m_uiTool != null && this.m_uiTool.IsInOpState;
        }
        public void SetIsInOpState(bool value)
        {
            this.m_uiTool.IsInOpState = value;
        }
        public Transform GetUIRoot()
        {
            return this.m_uiRoot;
        }
        #endregion
    }
}
