using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgLobbyBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：游戏大厅界面组件初始类
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class DlgLobbyBehaviour : DlgBehaviourBase
    {
        #region 字段
        #region 组件
        public IXUIButton m_Button_Play = null;
        public IXUILabel m_Label_Name = null;
        public IXUISprite m_Sprite_Avatar = null;
        public IXUILabel m_Label_Level = null;
        public IXUILabel m_Label_Money = null;
        public IXUILabel m_Label_Ticket = null;
        #endregion
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public override void Init()
        {
            base.Init();
            #region 开始战斗按钮
            this.m_Button_Play = base.GetUIObject("Play/PlayButton") as IXUIButton;
            if (this.m_Button_Play == null) 
            {
                Debug.Log("Play/PlayButton == null");
            }
            #endregion
            #region 角色名字
            this.m_Label_Name = base.GetUIObject("RoleStatus/Name/NameLabel") as IXUILabel;
            if (this.m_Label_Name == null)
            {
                Debug.Log("RoleStatus/Name/Label == null");
            }
            #endregion 
            #region 角色头像
            this.m_Sprite_Avatar = base.GetUIObject("RoleStatus/Avatar/Icon") as IXUISprite;
            if (this.m_Sprite_Avatar == null)
            {
                Debug.Log("RoleStatus/Avatar/Icon == null");
            }
            #endregion
            #region 角色等级
            this.m_Label_Level = base.GetUIObject("RoleStatus/Avatar/Level") as IXUILabel;
            if (this.m_Label_Level == null)
            {
                Debug.Log("RoleStatus/Avatar/Level == null");
            }
            #endregion
            #region 角色金币
            this.m_Label_Money = base.GetUIObject("RoleStatus/Gold/GoldLabel") as IXUILabel;
            if (this.m_Label_Money == null)
            {
                Debug.Log("RoleStatus/Gold/Label == null");
            }
            #endregion 
            #region 角色点券
            this.m_Label_Ticket = base.GetUIObject("RoleStatus/Ticket/TicketLabel") as IXUILabel;
            if (this.m_Label_Ticket == null)
            {
                Debug.Log("RoleStatus/Ticket/TicketLabel");
            }
            #endregion
        }
        #endregion
        #region 公有方法
        #endregion
        #region 私有方法
        #endregion
    }
}