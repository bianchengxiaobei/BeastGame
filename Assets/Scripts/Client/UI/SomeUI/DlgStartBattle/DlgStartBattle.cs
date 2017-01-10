using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgStartBattle 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.26
// 模块描述：战斗开始消息ui
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 战斗开始消息ui
/// </summary>
public class DlgStartBattle : DlgBase<DlgStartBattle,DlgStartBattleBehaviour>
{
    #region 字段
    #endregion
    #region 属性
    public override string fileName
    {
        get
        {
            return "DlgStartBattle";
        }
    }
    public override uint Type
    {
        get
        {
            return 512u;
        }
    }
    #endregion
    #region 构造方法
    #endregion
    #region 公共方法
    public void Show()
    {
        this.SetVisible(true);
    }
    protected override void OnShow()
    {
        base.OnShow();
        if (base.uiBehaviour.m_Group_UIEffect_StartBattle != null)
        {
            /*if (base.uiBehaviour.m_Group_UIEffect_StartBattle.CachedGameObject.animation)
            {
                //UIManager.singleton.PlayAnim(base.uiBehaviour.m_Group_UIEffect_StartBattle.CachedGameObject.animation, "startbattle", new AnimFinishedEventHandler(this.OnAnimationFinish));

            }*/
            TweenPosition tween = base.uiBehaviour.m_Group_UIEffect_StartBattle.CachedGameObject.GetComponent<TweenPosition>();
            if (tween != null)
            {
                tween.PlayForward();
                tween.onFinished.Add(new EventDelegate(this.OnAnimationFinish));
            }
        }
    }
    #endregion
    #region 私有方法
    private void OnAnimationFinish()
    {
        this.SetVisible(false);
        base.UnLoad();
    }
    #endregion
    #region 析构方法
    #endregion
}
