using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgMatchBehaviour
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：匹配界面组件初始类
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class DlgMatchBehaviour : DlgBehaviourBase
    {
        #region 字段
        #region 开始匹配按钮
        public IXUIButton m_Button_StartMatch = null;
        #endregion
        #region 游戏模式选择，人机，匹配，新手
        public IXUICheckBox m_CheckBox_Match = null;
        public IXUICheckBox m_CheckBox_AI = null;
        public IXUICheckBox m_CheckBox_Training = null;
        public IXUICheckBox m_CheckBox_Custom = null;
        #endregion
        #region 游戏类型选择，经典和大乱斗；游戏地图选择；游戏匹配类型选择，排位和匹配
        public IXUICheckBox m_CheckBox_Classis = null;
        public IXUICheckBox m_CheckBox_Mass = null;

        public IXUICheckBox m_CheckBox_Map1 = null;
        public IXUICheckBox m_CheckBox_Map2 = null;

        public IXUICheckBox m_CheckBox_MatchMode = null;
        public IXUICheckBox m_CheckBox_RankMode = null;
        #endregion
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override void Init()
        {
            base.Init();
            #region 开始匹配按钮
            this.m_Button_StartMatch = base.GetUIObject("LastStep/StartMatch") as IXUIButton;
            if (this.m_Button_StartMatch == null)
            {
                Debug.Log("LastStep/StartMatch == null");
            }
            #endregion
            #region 游戏模式选择，人机，匹配，新手
            this.m_CheckBox_Match = base.GetUIObject("ModeSelect/Match") as IXUICheckBox;
            this.m_CheckBox_AI = base.GetUIObject("ModeSelect/ComputerAI") as IXUICheckBox;
            this.m_CheckBox_Training = base.GetUIObject("ModeSelect/Training") as IXUICheckBox;
            this.m_CheckBox_Custom = base.GetUIObject("ModeSelect/Custom") as IXUICheckBox;
            #endregion
            #region 游戏类型选择，经典和大乱斗；游戏地图选择；游戏匹配类型选择，排位和匹配
            this.m_CheckBox_Classis = base.GetUIObject("MatchInterface/GameMode/Classis") as IXUICheckBox;
            this.m_CheckBox_Mass = base.GetUIObject("MatchInterface/GameMode/Mass") as IXUICheckBox;

            this.m_CheckBox_Map1 = base.GetUIObject("MatchInterface/GameMap/Map1") as IXUICheckBox;
            this.m_CheckBox_Map2 = base.GetUIObject("MatchInterface/GameMap/Map2") as IXUICheckBox;

            this.m_CheckBox_MatchMode = base.GetUIObject("MatchInterface/GameMode/MatchMode") as IXUICheckBox;
            this.m_CheckBox_RankMode = base.GetUIObject("MatchInterface/GameMode/RankMode") as IXUICheckBox;
            #endregion
        }
        #endregion
        #region 私有方法
        #endregion
    }
}