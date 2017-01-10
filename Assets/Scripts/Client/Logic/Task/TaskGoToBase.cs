using UnityEngine;
using System.Collections;
using UILib.Export;
using Client.UI;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：TaskGoToBase
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：任务基类
//----------------------------------------------------------------*/
#endregion
public class TaskGoToBase 
{
	#region 字段
    protected bool m_bStart = false;//是否已经开始这个任务了
    protected int m_nEffectId = 0;
    protected int m_nEffectTypeId = 0;
    protected string m_strUIName = string.Empty;
    protected SafeXUIObject m_uiTarget = null;
	#endregion
	#region 属性
    /// <summary>
    /// 是否已经开始该任务
    /// </summary>
    public bool IsStart
    {
        get;
        private set;
    }
    /// <summary>
    /// 是否已经完成该任务
    /// </summary>
    public bool IsFinish
    {
        get;
        private set;
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    /// <summary>
    /// 初始化该任务信息
    /// </summary>
    /// <param name="strGoToMsg"></param>
    public virtual void Init(string strGoToMsg)
    {
        this.IsFinish = false;
    }
    /// <summary>
    /// 开始该任务
    /// </summary>
    public virtual void Start()
    {
 
    }
    /// <summary>
    /// 结束该任务
    /// </summary>
    public virtual void Finish()
    {
        this.IsFinish = true;
    }
    /// <summary>
    /// 检测是否任务已经完成，完成的话执行Finish()方法
    /// </summary>
    public void Update()
    {
        if (this.m_bStart)
        {
            if (this.m_nEffectId == 0)
            {
                this.Finish();
            }
            else 
            {
                if (this.m_uiTarget != null && this.m_uiTarget.UIObject != null)
                {
                    if (!this.m_uiTarget.UIObject.CachedGameObject.activeInHierarchy || 
                        (!string.IsNullOrEmpty(this.m_strUIName) && !this.m_strUIName.Equals(UIManager.singleton.GetUIObjectId(this.m_uiTarget.UIObject))))
                    {
                        this.Finish();
                    }
                }
            }
        }
    }
    /// <summary>
    /// 显示任务UI特效
    /// </summary>
    /// <param name="dlg"></param>
    public void OnDlgShow(IXUIDlg dlg)
    {
        //任务已经开始
        if (this.m_bStart && this.m_nEffectId <= 0 && !string.IsNullOrEmpty(this.m_strUIName))
        {
            if (this.m_strUIName.IndexOf(dlg.fileName) == 0)
            {
                this.PlayEffect();
            }
        }
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 播放UI特效
    /// </summary>
    protected void PlayEffect()
    {
        IXUIObject iXUIObject = string.IsNullOrEmpty(this.m_strUIName) ? null : UIManager.singleton.GetUIObject(this.m_strUIName);
        if (iXUIObject != null && this.m_nEffectId == 0)
        {
            this.m_uiTarget = SafeXUIObject.GetSafeXUIObject(iXUIObject);
            
        }
    }
	#endregion
}
