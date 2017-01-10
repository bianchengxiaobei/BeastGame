using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using UILib.Export;
using Utility;
using GameClient.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgBehaviourBase
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public class DlgBehaviourBase : MonoBehaviour, IXUIBehaviour,IXUIObject
    {
        private IXUIDlg m_uiDlgInterface = null;
        private IXUIObject[] m_uiChilds = null;
        private GameObject m_Go = null;
        private Transform m_Trans = null;
        private Dictionary<string, XUIObjectBase> m_dicId2UIObject = new Dictionary<string, XUIObjectBase>();
        private IXLog m_log = XLog.GetLog<DlgBehaviourBase>();
        public bool IsError
        {
            get { return false; }
        }
        public IXUIObject parent
        {
            get { return null; }
            set { }
        }
        public IXUIDlg ParentDlg
        {
            get { return this.m_uiDlgInterface; }
            set { this.m_uiDlgInterface = value; }
        }
        public IXUIObject[] UIChilds
        {
            get { return this.m_uiChilds; }
        }
        public Transform CachedTransform 
        {
            get 
            {
                if (this.m_Trans == null)
                {
                    this.m_Trans = base.transform;
                }
                return this.m_Trans;
            }
        }
        public GameObject CachedGameObject
        {
            get 
            {
                if (null == this.m_Go)
                {
                    this.m_Go = base.gameObject;
                }
                return this.m_Go;
            }      
        }
        /// <summary>
        /// 真实世界scale，不受父级影响
        /// </summary>
        public Vector2 RealSize 
        {
            get 
            {
                Vector2 zero = Vector2.zero;
                zero.x = base.transform.lossyScale.x;
                zero.y = base.transform.lossyScale.y;
                return zero;
            }
        }
        /// <summary>
        /// 相对于父级的scale
        /// </summary>
        public Vector2 RelativeSize 
        {
            get 
            {
                Vector2 zero = Vector2.zero;
                zero.x = base.transform.localScale.x;
                zero.y = base.transform.localScale.y;
                return zero;
            }
        }
        /// <summary>
        /// 包围盒
        /// </summary>
        public virtual Bounds AbsoluteBounds
        {
            get 
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }
        }
        /// <summary>
        /// 相对包围盒
        /// </summary>
        public Bounds RelativeBounds 
        {
            get 
            {
                Vector3 center = this.CachedTransform.InverseTransformPoint(this.AbsoluteBounds.min);
                Vector3 point = this.CachedTransform.InverseTransformPoint(this.AbsoluteBounds.max);
                Bounds result = new Bounds(center, Vector3.zero);
                result.Encapsulate(point);
                return result;
            }
        }
        public object TipParam
        {
            get;
            set;
        }
        public string Tip
        {
            get;
            set;
        }
        public float Alpha
        {
            get;
            set;
        }
        public bool IsEnableOpen
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        /// <returns></returns>
        public bool IsVisible()
        {
            return base.gameObject.activeInHierarchy; 
        }
        /// <summary>
        /// 鼠标是否悬停在ui上
        /// </summary>
        /// <returns></returns>
        public bool IsMouseIn()
        {
            bool result = false;
            foreach (var current in this.m_dicId2UIObject.Values)
            {
                if (current != null && current.IsMouseIn())
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }
        /// <summary>
        /// 设置ui是否可见
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            UIManager.singleton.SetVisible(base.gameObject, bVisible);
        }
        public void RegisterMouseOnEventHandler(MouseOnEventHandler eventHandler)
        {
        }
        public void RegisterMouseLeaveEventHandler(MouseLeaveEventHandler eventHandler)
        {
        }
        public void RegisterPressDownEventHandler(PressDownEventHandler eventHandler)
        {
        }
        public void RegisterPressUpEventHandler(PressUpEventHandler eventHandler)
        {
        }
        public void RegisterClickEventHandler(ClickEventHandler eventHandler)
        {
        }
        public void RegisterLostFocusEventHandler(LostFocusEventHandler eventHandler)
        {
        }
        public void RegisterGetFocusEventHandler(GetFocusEventHandler eventHandler)
        {
        }
        /// <summary>
        /// 按下处理，先得到焦点
        /// </summary>
        public void OnPress()
        {
            this.OnFocus();
        }
        /// <summary>
        /// 得到焦点
        /// </summary>
        public void OnFocus()
        {
            UIManager.singleton.Compositor(this.m_uiDlgInterface);
        }
        /// <summary>
        /// 初始化，UITip
        /// </summary>
        public virtual void Init()
        {
            this.m_Trans = base.transform;
            WidgetFactory.FindAllUIObjects(this.m_Trans, this, ref this.m_dicId2UIObject);
            foreach (var current in this.m_dicId2UIObject.Values)
            {
                current.parent = this;
                current.ParentDlg = this.ParentDlg;
                string strKey = string.Format("{0}#{1}", this.CachedGameObject.name, current.CachedGameObject.name);
                string tip = Singleton<UITipConfigMgr>.singleton.GetTip(strKey);//UI提示字符串
                if (!string.IsNullOrEmpty(tip))
                {
                    TipParam tipParam = null;
                    string[] array = tip.Split(new char[] {'#'});
                    if (array.Length == 1)
                    {
                        tipParam = new TipParam();
                        tipParam.TipType = EnumTipType.eTipType_Common;
                        tipParam.Tip = array[0];
                    }
                    else if (array.Length == 2)
                    {
                        tipParam = new TitleTipParam
                        {
                            TipType = EnumTipType.eTipType_Title,
                            Title = array[0],
                            Tip = array[1]
                        };
                    }
                    current.TipParam = tipParam;
                }
                if (!current.IsInited)
                {
                    current.Init();
                }
            }
        }
        public void _Update()
        {
 
        }
        /// <summary>
        /// 是否高亮显示
        /// </summary>
        /// <param name="bTrue"></param>
        public virtual void Highlight(bool bTrue)
        {
 
        }
        /// <summary>
        /// 适应分辨率
        /// </summary>
        public void OnResolutionChange()
        {
            foreach (var current in this.m_dicId2UIObject.Values)
            {
                current.OnResolutionChange();
            }
        }
        /// <summary>
        /// 根据名字获取UIObject
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public IXUIObject GetUIObject(string strName)
        {
            IXUIObject result;
            if (null == strName)
            {
                result = null;
            }
            else
            {
                string key = strName;
                int num = strName.LastIndexOf('/');
                if (num >= 0)
                {
                    key = strName.Substring(num + 1);
                }
                XUIObjectBase xUIObjectBase = null;
                if (this.m_dicId2UIObject.TryGetValue(key, out xUIObjectBase))
                {
                    result = xUIObjectBase;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        private void OnDestory()
        {
 
        }
        private void Awake()
        {
 
        }
        private void Start()
        {
 
        }
    }
}
