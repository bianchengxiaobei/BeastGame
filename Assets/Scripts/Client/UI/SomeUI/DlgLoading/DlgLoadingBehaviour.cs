using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgLoadingBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.9
// 模块描述：玩家对战加载界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 玩家对战加载界面
/// </summary>
public class DlgLoadingBehaviour : DlgBehaviourBase
{
	#region 字段
    public IXUIList m_list_OurPlayer = null;
    public IXUIList m_list_EnemyPlayer = null;
    public IXUISprite m_sprite_Bg = null;
    public IXUILabel m_label_Tip = null;
    //public IXUIProgress m_progressBar = null;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    public override void Init()
    {        
        base.Init();
        #region 我方角色
        this.m_list_OurPlayer = base.GetUIObject("Top/OurPlayerList") as IXUIList;
        #endregion
        #region 敌方角色
        this.m_list_EnemyPlayer = base.GetUIObject("Down/EnemyPlayerList") as IXUIList;
        #endregion 
        #region 加载背景
        this.m_sprite_Bg = base.GetUIObject("Background") as IXUISprite;
        #endregion
        #region 加载时的小提示
        this.m_label_Tip = base.GetUIObject("Center/Label_Tip") as IXUILabel;
        #endregion
        #region 加载进度条
        //this.m_progressBar = base.GetUIObject("") as IXUIProgress;
        #endregion 
    }
	#endregion
	#region 私有方法
	#endregion
}
