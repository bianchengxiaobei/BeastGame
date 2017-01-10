using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Utility;
using Client.Data;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgMatch
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：匹配界面事件类
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class DlgMatch : DlgBase<DlgMatch, DlgMatchBehaviour>
    {
        #region 字段
        /// <summary>
        /// 是否第一次点击匹配按钮
        /// </summary>
        private bool m_bClickMatch = false;
        //private EMatchtype m_eMatchType = EMatchtype.MATCH_3V3;
        private EGameType m_eMatchType = EGameType.GAME_TYPE_MATCH;
        #endregion
        #region 属性
        public override string fileName
        {
            get
            {
                return "DlgMatch";
            }
        }
        public override int layer
        {
            get
            {
                return -2;
            }
        }
        public override uint Type
        {
            get
            {
                return 128u;
            }
        }
        public bool ClickMatch 
        {
            set 
            {
                this.m_bClickMatch = value;
            }
        }
        public EGameType MatchType 
        {
            get { return this.m_eMatchType; }
            set { this.m_eMatchType = value; }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override void Init()
        {
            base.Init();
            base.uiBehaviour.m_CheckBox_Classis.bChecked = true;
        }
        protected override void OnShow()
        {
            base.OnShow();
            //this.uiBehaviour.m_CheckBox_Classis.SetVisible(false);
            //this.uiBehaviour.m_CheckBox_Mass.SetVisible(false);
            //this.uiBehaviour.m_CheckBox_Map1.SetVisible(false);
            //this.uiBehaviour.m_CheckBox_Map2.SetVisible(false);
            this.uiBehaviour.m_CheckBox_MatchMode.SetVisible(false);
            this.uiBehaviour.m_CheckBox_RankMode.SetVisible(false);
            this.uiBehaviour.m_Button_StartMatch.CachedTransform.parent.gameObject.SetActive(false);
        }
        public override void RegisterEvent()
        {
            this.uiBehaviour.m_Button_StartMatch.RegisterClickEventHandler(new ButtonClickEventHandler(this.StartMatchSearch));
            this.uiBehaviour.m_CheckBox_Classis.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnCheckBoxClassis));
            this.uiBehaviour.m_CheckBox_Map1.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnCheckBoxMap1));
            this.uiBehaviour.m_CheckBox_MatchMode.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnCheckBoxMatchMode));
            this.uiBehaviour.m_CheckBox_Map2.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnCheckBoxMap2));
            base.RegisterEvent();
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 开始匹配搜索（发送请求）
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool StartMatchSearch(IXUIButton button)
        {
            //此处是为了限制反复点击，不断地发送消息，应该是在收到结果之后再设置回来
            if (!this.m_bClickMatch) 
            {
                this.ClickMatch = this.SendMatchReq();
            }
            return this.m_bClickMatch;
        }
        /// <summary>
        /// 向服务器发送匹配请求
        /// </summary>
        /// <returns></returns>
        private bool SendMatchReq()
        {
            //取得选择的地图id
            uint uMapId = Singleton<RoomManager>.singleton.MapId;
            //取得选择的匹配类型
           // EMatchtype eMatchType = this.m_eMatchType;
            EGameType eMatchType = this.m_eMatchType;
            //发送请求
            Singleton<NetworkManager>.singleton.SendMatchReq(uMapId, eMatchType,EAIDifficulty.AI_DIFFICULTY_EASY);
            return true;
        }
        /// <summary>
        /// 选择经典模式
        /// </summary>
        private bool OnCheckBoxClassis(IXUICheckBox checkBox)
        {
            //this.uiBehaviour.m_CheckBox_Map1.SetVisible(true);
            //this.uiBehaviour.m_CheckBox_Map2.SetVisible(true);
            return true;
        }
        private bool OnCheckBoxMap1(IXUICheckBox box)
        {
            if (box.bChecked)
            {
                this.uiBehaviour.m_CheckBox_MatchMode.SetVisible(true);
                this.uiBehaviour.m_CheckBox_RankMode.SetVisible(true);
                Singleton<RoomManager>.singleton.MapId = 0;
            }
            return true;
        }
        private bool OnCheckBoxMap2(IXUICheckBox box)
        {
            if (box.bChecked)
            {
                this.uiBehaviour.m_CheckBox_MatchMode.SetVisible(true);
                this.uiBehaviour.m_CheckBox_RankMode.SetVisible(true);
                Singleton<RoomManager>.singleton.MapId = 1;
            }
            return true;
        }
        private bool OnCheckBoxMatchMode(IXUICheckBox box)
        {
            if (box.bChecked)
            {
                this.uiBehaviour.m_Button_StartMatch.CachedTransform.parent.gameObject.SetActive(true);
            }
            return true;
        }
        #endregion
    }
}
/// <summary>
/// 匹配类型，1v1，2v2,3v3等
/// </summary>
public enum EMatchtype 
{
    MATCH_INVALID,
    MATCH_1V1,
    MATCH_2V2,
    MATCH_3V3,
    MATCH_4V4,
    MATCH_5V5,
    MATCH_1C3,
    MATCH_FREE
}
/// <summary>
/// 人机难度级别，分简单，普通，困难
/// </summary>
public enum EAIDifficulty 
{
    AI_DIFFICULTY_EASY,
    AI_DIFFICULTY_NORMAL,
    AI_DIFFICULTY_HARD
}
