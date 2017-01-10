using UnityEngine;
using AnimationOrTween;
using System.Collections.Generic;
using Client.UI.UICommon;
using System;
using UnityAssetEx.Export;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUITool
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.20
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class XUITool : MonoBehaviour, IXUITool
{
    private LoadTextureAsynEventHandler m_loadResAsynEventHandler;
    private TipShowEventHandler m_tipShowEventHandler;
    private TipGetterEventHandler m_tipGetterEventHandler;
    private AddListItemEventHandler m_addListItemEventHandler;
    private bool m_bInOpState;
    private Vector2 m_vec2HotSpot = Vector2.zero;
    private string m_strCursorTextureFile = string.Empty;
    private Dictionary<string, Texture2D> m_dicCursorTexture = new Dictionary<string, Texture2D>();
    private static XUITool s_instance;
    public static XUITool Instance
    {
        get
        {
            return XUITool.s_instance;
        }
    }
    public int CurrentTouchID
    {
        get
        {
            return UICamera.currentTouchID;
        }
    }
    public bool IsEventProcessed
    {
        get
        {
            return UICamera.s_bEventProcessed;
        }
        set
        {
            UICamera.s_bEventProcessed = value;
        }
    }
    public UICamera.MouseOrTouch CurrentTouch
    {
        get
        {
            return UICamera.currentTouch;
        }
    }
    public bool IsInOpState
    {
        get
        {
            return this.m_bInOpState;
        }
        set
        {
            this.m_bInOpState = value;
        }
    }
    /// <summary>
    /// 是否鼠标或者手指触摸UI
    /// </summary>
    public bool IsAnyTouchInUI
    {

        /*get
        {
            foreach (var current in UICamera.activeTouches)
            {
                if (UICamera.genericEventHandler != current.current || (UICamera.genericEventHandler != current.dragged && null != current.dragged))
                {
                    return true;
                }
            }
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                UICamera.MouseOrTouch[] mMouse = UICamera.mMouse;
                for (int i = 0; i < mMouse.Length; i++)
                {
                    UICamera.MouseOrTouch mouseOrTouch = mMouse[i];
                    if (UICamera.genericEventHandler != mouseOrTouch.current || (UICamera.genericEventHandler != mouseOrTouch.dragged && null != mouseOrTouch.dragged))
                    {
                        return true;
                    }
                }
            }
            return UICamera.hoveredObject != UICamera.genericEventHandler;
        }*/
        get { return UICamera.isOverUI; }
    }
    public bool IsInputHasFocus
    {
        get
        {
            return UICamera.inputHasFocus;
        }
    }
    public GameObject CurFocus
    {
        get
        {
            if (UICamera.currentTouch != null)
            {
                return UICamera.currentTouch.pressed;
            }
            return null;
        }
    }
    public float SoundVolume
    {
        get
        {
            return NGUITools.soundVolume;
        }
        set
        {
            NGUITools.soundVolume = value;
        }
    }
    /// <summary>
    /// 获取NGUI的UICamera
    /// </summary>
    /// <returns></returns>
    public Camera GetUICamera()
    {
        return UICamera.mainCamera;
    }
    public void SetActive(GameObject obj, bool state)
    {
        NGUITools.SetActiveSelf(obj, state);
    }
    public void SetFocus(IXUIObject uiObject)
    {
        if (uiObject == null)
        {
            return;
        }
        UICamera.selectedObject = uiObject.CachedGameObject;
    }
    public void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        Transform transform = go.transform;
        int i = 0;
        int childCount = transform.GetChildCount();
        while (i < childCount)
        {
            Transform child = transform.GetChild(i);
            this.SetLayer(child.gameObject, layer);
            i++;
        }
    }
    public void SetUIEventHandler(GameObject obj)
    {
        UICamera.genericEventHandler = obj;
    }
    public void RegisterLoadResAsynEventHandler(LoadTextureAsynEventHandler eventHandler)
    {
        this.m_loadResAsynEventHandler = eventHandler;
    }
    public IAssetRequest LoadResAsyn(string url, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType)
    {
        if (this.m_loadResAsynEventHandler != null)
        {
            return this.m_loadResAsynEventHandler(url, callBackFun, assetPRIType);
        }
        Debug.LogError("null == m_loadUIAsynEventHandler");
        return null;
    }
    public void RegisterTipShowEventHandler(TipShowEventHandler eventHandler)
    {
        this.m_tipShowEventHandler = eventHandler;
    }
    public void RegisterTipGetterEventHandler(TipGetterEventHandler eventHandler)
    {
        this.m_tipGetterEventHandler = eventHandler;
    }
    public void RegisterAddListItemEventHandler(AddListItemEventHandler eventHandler)
    {
        this.m_addListItemEventHandler = eventHandler;
    }
    public void PlayAnim(Animation anim, string strClipName, AnimFinishedEventHandler eventHandler)
    {
        if (null == anim || strClipName == null || strClipName.Length == 0)
        {
            if (eventHandler != null)
            {
                eventHandler();
            }
            return;
        }
        /*ActiveAnimation activeAnimation = ActiveAnimation.Play(anim, strClipName, Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable,eventHandler);
        if (null != activeAnimation)
        {
            activeAnimation.Reset();
        }*/
        anim.Play(strClipName);
        if (eventHandler != null)
        {
            eventHandler();
        }
    }
    public void SetCursor(string strTextureFile, Vector2 hotspot)
    {
        if (string.IsNullOrEmpty(strTextureFile) || this.m_strCursorTextureFile.Equals(strTextureFile))
        {
            return;
        }
        this.m_vec2HotSpot = hotspot;
        this.m_strCursorTextureFile = strTextureFile;
        Texture2D texture = null;
        if (this.m_dicCursorTexture.TryGetValue(this.m_strCursorTextureFile, out texture))
        {
            Cursor.SetCursor(texture, this.m_vec2HotSpot, CursorMode.ForceSoftware);
        }
        else
        {
            XUITool.Instance.LoadResAsyn(strTextureFile, new AssetRequestFinishedEventHandler(this.OnLoadCursorTextureFinished), AssetPRI.DownloadPRI_High);
        }
    }
    private void OnLoadCursorTextureFinished(IAssetRequest assetRequest)
    {
        IAssetResource assetResource = assetRequest.AssetResource;
        if (assetResource == null)
        {
            return;
        }
        UnityEngine.Object mainAsset = assetResource.MainAsset;
        Texture2D texture2D = mainAsset as Texture2D;
        if (null == texture2D)
        {
            return;
        }
        Cursor.SetCursor(texture2D, this.m_vec2HotSpot, CursorMode.ForceSoftware);
        this.m_dicCursorTexture[this.m_strCursorTextureFile] = texture2D;
    }
    public void ShowCursor()
    {
        Screen.showCursor = true;
    }
    public void HideCursor()
    {
        Screen.showCursor = false;
    }
    public void ShowTooltip(GameObject obj, bool bShow)
    {
        if (null == obj && !bShow)
        {
            UITooltip.ShowText(string.Empty);
        }
    }
    public void ResetCurTouchState()
    {
        Debug.Log("ResetCurTouchState");
        for (int i = 0; i < UICamera.mMouse.Length; i++)
        {
            UICamera.MouseOrTouch mouseOrTouch = UICamera.mMouse[i];
            if (mouseOrTouch != null)
            {
                mouseOrTouch.dragStarted = false;
                mouseOrTouch.pressed = null;
                mouseOrTouch.dragged = null;
            }
        }
        foreach (UICamera.MouseOrTouch current in UICamera.activeTouches)
        {
            current.dragStarted = false;
            current.pressed = null;
            current.dragged = null;
        }
    }
    /// <summary>
    /// 设置物体及其所有子物体的渲染颜色
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="color"></param>
    public void SetColor(GameObject obj, Color color)
    {
        if (obj == null)
        {
            return;
        }
        //取得所有子物体的Renderer组件数组
        Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
        if (componentsInChildren == null)
        {
            return;
        }
        Renderer[] array = componentsInChildren;
        for (int i = 0; i < array.Length; i++)
        {
            Renderer renderer = array[i];
            if (!(renderer == null))
            {
                Material[] materials = renderer.materials;//修改Material的颜色
                for (int j = 0; j < materials.Length; j++)
                {
                    Material material = materials[j];
                    if (material.HasProperty("_Color"))
                    {
                        material.SetVector("_Color", color);
                    }
                    if (material.HasProperty("_TintColor"))
                    {
                        material.SetVector("_TintColor", color);
                    }
                }
            }
        }
    }
    public void LoadSceneAsync(string strScene, Action action)
    {
    }
    public void OnTip(bool bshow, IXUIObject uiObject)
    {
        if (this.m_tipShowEventHandler != null)
        {
            this.m_tipShowEventHandler(bshow, uiObject);
        }
    }
    public string GetTip(string strUIObjectId)
    {
        if (this.m_tipGetterEventHandler != null)
        {
            return this.m_tipGetterEventHandler(strUIObjectId);
        }
        return string.Empty;
    }
    public void OnAddListItem(IXUIDlg uiDlg, IXUIList uiList, IXUIListItem uiListItem)
    {
        if (this.m_addListItemEventHandler != null)
        {
            this.m_addListItemEventHandler(uiDlg, uiList, uiListItem);
        }
    }
    public void BeginTweenAlpha(GameObject go, float duration, float alpha)
    {
        TweenAlpha.Begin(go, duration, alpha);
    }
    public void BeginTweenPosition(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition.Begin(go, duration, pos);
    }
    public void BeginTweenScale(GameObject go, float duration, Vector3 scale)
    {
        TweenScale.Begin(go, duration, scale);
    }
    public void BeginTweenRotation(GameObject go, float duration, Quaternion rot)
    {
        TweenRotation.Begin(go, duration, rot);
    }
    public void BeginTweenVolume(GameObject go, float duration, float targetVolume)
    {
        TweenVolume.Begin(go, duration, targetVolume);
    }
    public static void S_OnTip(bool bshow, IXUIObject uiObject)
    {
        if (null != XUITool.Instance)
        {
            XUITool.Instance.OnTip(bshow, uiObject);
        }
    }
    public static void S_OnAddListItem(IXUIDlg uiDlg, IXUIList uiList, IXUIListItem uiListItem)
    {
        if (null != XUITool.Instance)
        {
            XUITool.Instance.OnAddListItem(uiDlg, uiList, uiListItem);
        }
    }
    private void Awake()
    {
        XUITool.s_instance = this;
    }
}