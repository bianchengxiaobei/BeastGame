using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgRoomBehaviour 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.27
// 模块描述：房间界面组件类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 房间界面组件类
/// </summary>
public class DlgRoomBehaviour : DlgBehaviourBase
{
	#region 字段
    public IXUIList m_ourBeastList = null;
    public IXUIList m_enmeyBeastList = null;
    public IXUIList m_BeastList = null;

    public IXUIButton m_Button_Confirm = null;
    public IXUILabel m_Label_Tips = null;
    public IXUILabel m_Label_Time = null;
    public IXUITextList m_TextList_Chat = null;
    #endregion
    #region 公共方法
    public override void Init()
    {      
        base.Init();
        #region 我方的神兽列表
        this.m_ourBeastList = base.GetUIObject("Beasts/OurBeasts") as IXUIList;
        #endregion
        #region 敌方的神兽列表
        this.m_enmeyBeastList = base.GetUIObject("Beasts/EnemyBeasts") as IXUIList;
        #endregion 
        #region 选择神兽列表
        this.m_BeastList = base.GetUIObject("Beasts/SelectBeastBack/BeastBg/Beastlist") as IXUIList;
        #endregion
        #region 确定选择按钮
        this.m_Button_Confirm = base.GetUIObject("Beasts/SureButton") as IXUIButton;
        #endregion 
        #region 提示lebel
        this.m_Label_Tips = base.GetUIObject("Beasts/RoomTip") as IXUILabel;
        #endregion
        #region 时间倒计时
        this.m_Label_Time = base.GetUIObject("Beasts/Time") as IXUILabel;
        #endregion
        #region 聊天文本
        this.m_TextList_Chat = base.GetUIObject("Beasts/ChatWindow/ChatTextList") as IXUITextList;
        #endregion
    }
	#endregion

}
