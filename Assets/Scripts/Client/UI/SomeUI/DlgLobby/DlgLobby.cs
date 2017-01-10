using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Utility;
using Client.Data;
using Client.Common;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgLobby
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：游戏大厅界面事件类
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class DlgLobby : DlgBase<DlgLobby, DlgLobbyBehaviour>
    {
        #region 字段
        #endregion
        #region 属性
        public override string fileName
        {
            get
            {
                return "DlgLobby";
            }
        }
        public override int layer
        {
            get
            {
                return 0;
            }
        }
        public override uint Type
        {
            get
            {
                return 32u;
            }
        }
        public override bool IsPersist
        {
            get
            {
                return true;
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override void Init()
        {
            base.Init();
        }
        protected override void OnShow()
        {
            base.OnShow();
            this.OnRefresh();
        }
        protected override void OnRefresh()
        {
            base.OnRefresh();
            //刷新角色属性
            this.RefreshDynamicInfo();
        }
        public override void RegisterEvent()
        {
            this.uiBehaviour.m_Button_Play.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickPlayGame));
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        /// <param name="playButton"></param>
        /// <returns></returns>
        private bool OnClickPlayGame(IXUIButton playButton)
        {
            //将房间的战斗模式设置为匹配
            Singleton<RoomManager>.singleton.MathMode = EnumMathMode.EnumMathMode_FightMode;
            //进入匹配状态
            Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_Match);
            return true;
        }
        /// <summary>
        /// 刷新角色动态信息
        /// </summary>
        private void RefreshDynamicInfo()
        {
            //设置角色的昵称
            base.uiBehaviour.m_Label_Name.SetText(Singleton<PlayerRole>.singleton.Name);
            //设置角色的头像
            base.uiBehaviour.m_Sprite_Avatar.SetSprite(Singleton<PlayerRole>.singleton.Icon);
            //设置角色等级
            base.uiBehaviour.m_Label_Level.SetText(Singleton<PlayerRole>.singleton.Level.ToString());
            //设置角色金钱
            base.uiBehaviour.m_Label_Money.SetText(Singleton<PlayerRole>.singleton.Money.ToString());
            //设置点券
            base.uiBehaviour.m_Label_Ticket.SetText(Singleton<PlayerRole>.singleton.Ticket.ToString());
        }
        #endregion
    }
}