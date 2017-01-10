using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalXUIObject
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUIObject : IXUIObject
    {
        private object m_tipParam;
        private static GameObject m_cachedGameObject;
        private IXUIObject m_parent;
        private IXUIDlg m_parentDlg;
        private string m_tip;
        private float m_aphla;
        private bool m_isEnableOpen;
        public object TipParam
        {
            get{return this.m_tipParam;}
            set{this.m_tipParam = value;}
        }
        #region 属性
        public GameObject CachedGameObject
        {
            get{return LocalXUIObject.m_cachedGameObject;}
        }
        public Transform CachedTransform
        {
            get{return LocalXUIObject.m_cachedGameObject.transform;}
        }
        public IXUIObject parent
        {
            get{return this.m_parent;}
            set{this.parent = value;}
        }
        public IXUIDlg ParentDlg
        {
            get{return this.m_parentDlg;}
            set{this.m_parentDlg = value;}
        }
        public Vector2 RealSize
        {
            get{return Vector2.zero;}
        }
        public Vector2 RelativeSize
        {
            get{return Vector2.zero;}
        }
        public string Tip
        {
            get{return this.m_tip;}
            set{this.Tip = value;}
        }
        public Bounds AbsoluteBounds
        {
            get{return default(Bounds);}
        }
        public Bounds RelativeBounds
        {
            get{return default(Bounds);}
        }
        public bool IsError
        {
            get{return true;}
        }
        public float Alpha
        {
            get{return this.m_aphla;}
            set{this.m_aphla = value;}
        }
        public bool IsEnableOpen
        {
            get{return this.m_isEnableOpen;}
            set{this.m_isEnableOpen =value;}
        }
        #endregion
        public IXUIObject GetUIObject(string strPath) 
        {
            return null;
        }
        public bool IsVisible() 
        {
            return true;
        }
        public void SetVisible(bool bVisible) { }
        public void OnFocus() { }
        public void OnResolutionChange() { }
        public void Highlight(bool bTrue) { }
        public void RegisterMouseOnEventHandler(MouseOnEventHandler eventHandler) { }
        public void RegisterMouseLeaveEventHandler(MouseLeaveEventHandler eventHandler) { }
        public void RegisterPressUpEventHandler(PressUpEventHandler eventHandler) { }
        public void RegisterPressDownEventHandler(PressDownEventHandler eventHandler) { }
        public void RegisterClickEventHandler(ClickEventHandler eventHandler) { }
        public void RegisterLostFocusEventHandler(LostFocusEventHandler eventHandler) { }
        public void RegisterGetFocusEventHandler(GetFocusEventHandler eventHandler) { }
    }
}