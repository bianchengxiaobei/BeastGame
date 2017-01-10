using UnityEngine;
using System;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUITool
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUITool
    {
        #region 属性
        bool IsAnyTouchInUI
        {
            get;
        }
        bool IsInputHasFocus
        {
            get;
        }
        int CurrentTouchID
        {
            get;
        }
        UICamera.MouseOrTouch CurrentTouch
        {
            get;
        }
        bool IsInOpState
        {
            get;
            set;
        }
        bool IsEventProcessed
        {
            get;
            set;
        }
        GameObject CurFocus
        {
            get;
        }
        float SoundVolume
        {
            get;
            set;
        }
        #endregion
        void SetActive(GameObject obj, bool state);
        void SetFocus(IXUIObject uiObject);
        void SetLayer(GameObject go, int layer);
        void SetUIEventHandler(GameObject obj);
        void RegisterLoadResAsynEventHandler(LoadTextureAsynEventHandler eventHandler);
        void RegisterTipShowEventHandler(TipShowEventHandler eventHandler);
        void RegisterTipGetterEventHandler(TipGetterEventHandler eventHandler);
        void RegisterAddListItemEventHandler(AddListItemEventHandler eventHandler);
        Camera GetUICamera();
        void PlayAnim(Animation anim, string strClipName, AnimFinishedEventHandler eventHandler);
        void SetCursor(string strTextureFile, Vector2 hotspot);
        void ShowTooltip(GameObject obj, bool bShow);
        void ResetCurTouchState();
        void SetColor(GameObject obj, Color color);
        void LoadSceneAsync(string strScene, Action action);
        void BeginTweenPosition(GameObject go, float duration, Vector3 pos);
        void BeginTweenAlpha(GameObject go, float duration, float alpha);
        void BeginTweenScale(GameObject go, float duration, Vector3 scale);
        void BeginTweenRotation(GameObject go, float duration, Quaternion rot);
        void BeginTweenVolume(GameObject go, float duration, float targetVolume);
    }
}
