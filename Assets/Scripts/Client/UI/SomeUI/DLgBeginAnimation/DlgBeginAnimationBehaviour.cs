using UnityEngine;
using Client.UI.UICommon;
using UILib.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgBeginAnimationBeha
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：开场动画UI组件初始化
//----------------------------------------------------------------*/
#endregion
public class DlgBeginAnimationBehaviour : DlgBehaviourBase
{
    public IXUIPicture m_Texture_video = null;
    public override void Init()
    {
        base.Init();
        this.m_Texture_video = base.GetUIObject("Texture_video") as IXUIPicture;
        if (null == this.m_Texture_video)
        {
            Debug.Log("Texture_video is null");
            this.m_Texture_video = WidgetFactory.CreateWidget<IXUIPicture>();
        }
    }
}
