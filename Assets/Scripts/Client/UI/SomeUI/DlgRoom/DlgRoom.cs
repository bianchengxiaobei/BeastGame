using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Client.UI.UICommon;
using Client.UI;
using Utility.Export;
using Utility;
using Game;
using Client.Common;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgRoom 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.27
// 模块描述：匹配选择神兽房间的界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 匹配选择神兽房间的界面
/// </summary>
public class DlgRoom : DlgBase<DlgRoom,DlgRoomBehaviour>
{
	#region 字段
    private IXLog m_log = XLog.GetLog<DlgRoom>();
    private float m_fTimeLimit = 0;
    private float m_fTimeSelectStart = 0;
    private int m_fTimeRemain = 0;
    private long m_lBeastIdCurrent = 0;
    private long m_unRandomBeastID = 0;
    private int m_unCount = 0;
    private int m_unPlayerCount = 0;
    private int m_nCurEquipSelectIndex = 0;//当前装备选择的索引
    //是否选择了
    private bool m_bIsSelected = false;
    //是否默认选择
    private bool m_DefaultSelected = false;
    //是否只刷新一次
    private bool m_bShowOne = false;
    private EnumSelectStep m_eStep = EnumSelectStep.eSelectStep_None;
    //已经被玩家选择过的神兽列表key-->beastId,value--->beastTypeId
    private Dictionary<long, int> m_dicRoleSelectBeastID = new Dictionary<long, int>();
    //刷新确定选择按钮的队列委托，用来改变按钮的激活状态
    private Queue<Action> RefrenshOperation_ConfirmButton = new Queue<Action>();
    //玩家激活过的神兽列表
    private List<int> m_listActiveBeastId = new List<int>();
	#endregion
	#region 属性
    public override string fileName
    {
        get
        {
            return "DlgRoom";
        }
    }
    public override int layer
    {
        get
        {
            return 3;
        }
    }
    public override uint Type
    {
        get
        {
            return 256u;
        }
    }
    /// <summary>
    /// 是否已经选择了神兽,如果选了，就刷新确定按钮
    /// </summary>
    public bool IsSelected
    {
        get { return this.m_bIsSelected; }
        set 
        {
            this.m_bIsSelected = value;
            this.RefreshOperation(!this.m_bIsSelected);
        }
    }
    /// <summary>
    /// 已经选择的神兽Id
    /// </summary>
    public int SelectBeastTypeId
    {
        get;
        set;
    }
    public long CurSelectBeastId
    {
        get 
        {
            return this.m_lBeastIdCurrent; 
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公共方法
    public override void Init()
    {
        base.Init();
        this.m_unRandomBeastID = 2147483647u;
        if (Singleton<RoomManager>.singleton.IsObserver)
        {
            IXUIListItem[] allItems = base.uiBehaviour.m_ourBeastList.GetAllItems();
            for (int i = 0; i < allItems.Length; i++)
            {
                IXUIListItem item = allItems[i];
                item.RegisterClickEventHandler(new ClickEventHandler(this.OnListItemOnClickOurBeast));
            }
            allItems = base.uiBehaviour.m_enmeyBeastList.GetAllItems();
            for (int i = 0; i < allItems.Length; i++)
            {
                IXUIListItem item = allItems[i];
                item.RegisterClickEventHandler(new ClickEventHandler(this.OnListItemOnClickEnemyBeast));
            }
        }
    }
    public override void RegisterEvent()
    {
        base.RegisterEvent();
        base.uiBehaviour.m_BeastList.RegisterListClickEventHandler(new ListClickEventHandler(this.OnBeastListSelect));
        base.uiBehaviour.m_Button_Confirm.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnButtonConfirmClick));
    }
    protected override void OnShow()
    {
        base.OnShow();
        this.RefreshEquip(0u, 0);
        this.ShowBeastSelect();
        this.ShowBeasts();
        Singleton<CWindowHandle>.singleton.SetWindowFlash();
        this.m_unCount = 0;
        this.m_unPlayerCount = 0;
    }
    protected override void OnRefresh()
    {
        base.OnRefresh();
        this.RefreshTip(0u);
        this.RefreshAllPlayerInfo();
        this.RefreshBeastSelectStatus();
        //只刷新一次
        if (!this.m_bShowOne)
        {
            //刷新地图信息和游戏信息
            //this.ShowGameInfo();
            this.ShowCurSelectRole();
        }
        else 
        {
            this.SelectRole(this.m_lBeastIdCurrent);
        }
        Singleton<RoomManager>.singleton.OnRefreshFinish = true;
    }
    public override void Update()
    {
        base.Update();
        if (base.Prepared && base.IsVisible())
        {
            if (!this.m_DefaultSelected)
            {
                this.m_DefaultSelected = true;
            }
            float time = Time.time;
            float num = this.m_fTimeLimit - (time - this.m_fTimeSelectStart);
            num = num > 0 ? num : 0;
            //刷新进度条
            if (num < this.m_fTimeRemain) 
            {
                this.uiBehaviour.m_Label_Time.SetText(this.m_fTimeRemain.ToString());
                this.m_fTimeRemain--;
            }
            if (this.RefrenshOperation_ConfirmButton.Count > 0)
            {
                this.RefrenshOperation_ConfirmButton.Dequeue()();
            }
        }
    }
    public override void Reset()
    {
        base.Reset();
        this.IsSelected = false;
        this.m_dicRoleSelectBeastID.Clear();
        this.m_eStep = EnumSelectStep.eSelectStep_None;
    }
    /// <summary>
    /// 设置选择神兽的时间限制
    /// </summary>
    /// <param name="fTimeLimit"></param>
    /// <param name="step"></param>
    public void SetTimeLimit(float fTimeLimit, EnumSelectStep step)
    {
        this.m_fTimeLimit = fTimeLimit;
        this.m_fTimeSelectStart = Time.time;
        this.m_fTimeRemain = (int)this.m_fTimeLimit;
        this.m_eStep = step;
    }
    public void OnRecvChat(string strRoleName,string strBeastName,string nickName,bool isRandom,ECampType eCampType)
    {
        if (base.Prepared)
        {
            string forwardStr = isRandom ? "随机" : "";
            string colorStr = CommonDefine.CampTypeToChatColor(eCampType == Singleton<PlayerRole>.singleton.CampType);
            //if (eCampType == Singleton<PlayerRole>.singleton.CampType)
            //{
                base.uiBehaviour.m_TextList_Chat.Add(string.Format("{0}{1}[-]{2}[FDFDFD]选择了[00ffff]{3}[-]", new object[]
			    {
				    colorStr,
				    strRoleName,
				    forwardStr,
				    strBeastName
			    }));
            //}
        }
    }
    /// <summary>
    /// 选择该神兽
    /// </summary>
    /// <param name="unBeastMarkId"></param>
    public void SelectRole(long unBeastMarkId)
    {
        if (unBeastMarkId != 0 && base.Prepared)
        {
            //根据神兽id取得我方神兽列表中的item，如果不为为空就选中该item
            IXUIListItem itemId = base.uiBehaviour.m_ourBeastList.GetItemById(unBeastMarkId);
            if (itemId != null)
            {
                base.uiBehaviour.m_ourBeastList.SetSelectedItemById(unBeastMarkId);
            }
            else 
            {
                //如果为空，就去敌方列表中找，找到选中
                itemId = base.uiBehaviour.m_enmeyBeastList.GetItemById(unBeastMarkId);
                if (itemId != null)
                {
                    base.uiBehaviour.m_enmeyBeastList.SetSelectedItemById(unBeastMarkId);
                }
            }
            BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(unBeastMarkId);
            if (beastData != null)
            {
                //显示技能
                //this.ShowAllSkill(beastData.BeastTypeId);
                //刷新神兽选择列表的状态
                this.RefreshBeastSelectStatus();
                //显示选择后的神兽信息
                this.ShowBeastInfo(beastData.BeastTypeId);
                //刷新装备
                this.RefreshEquip(beastData.Id, beastData.BeastTypeId);
                //刷新提示
                this.RefreshTip(unBeastMarkId);
                if (Singleton<RoomManager>.singleton.IsObserver)
                {
                    this.ShowBeastSelect();
                }
                else 
                {
                    //如果已经被选择的话，就显示皮肤选择界面
                    if (beastData.IsSelected)
                    {
                        this.ShowSkinSelect();
                        base.uiBehaviour.m_BeastList.SetVisible(false);
                    }
                    //否则显示神兽界面选择
                    else
                    {
                        this.ShowBeastSelect();
                    }
                }
                this.IsSelected = beastData.IsSelected;
                this.m_nCurEquipSelectIndex = beastData.EquipIndex;
            }
        }
    }
    /// <summary>
    /// 刷新神兽选择之后的状态
    /// </summary>
    public void RefreshBeastSelectStatus()
    {
        if (base.uiBehaviour != null)
        {
            for (int i = 0; i < base.uiBehaviour.m_BeastList.Count; i++)
            {
                IXUIListItem item = base.uiBehaviour.m_BeastList.GetItemByIndex(i);
                if (item != null)
                {
                    IXUISprite icon = item.GetUIObject("Icon") as IXUISprite;
                    if (icon != null)
                    {
                        icon.Color = Color.white;
                    }
                }
            }
            //循环遍历所有已经选择的英雄，设置icon为灰色
            foreach (var current in this.m_dicRoleSelectBeastID)
            {
                if (this.m_unRandomBeastID != current.Value && this.m_lBeastIdCurrent != current.Value)
                {
                    IXUIListItem item = base.uiBehaviour.m_BeastList.GetItemById((uint)current.Value);
                    if (item != null && item.Index > 0)
                    {
                        IXUISprite icon = item.GetUIObject("Icon") as IXUISprite;
                        if (icon != null)
                        {
                            icon.Color = UnityTools.GrayColor;
                        }
                    }
                }
            }
            //如果可以重复选择，就不设置为灰色
            if (Singleton<RoomManager>.singleton.RepeatSelectBeastInAllCamp == 1)
            {
                foreach (var current in this.m_dicRoleSelectBeastID)
                {
                    if (this.m_unRandomBeastID != current.Value && this.m_lBeastIdCurrent != current.Value)
                    {
                        IXUIListItem item = base.uiBehaviour.m_BeastList.GetItemById((uint)current.Value);
                        if (item != null && item.Index > 0)
                        {
                            IXUISprite icon = item.GetUIObject("Icon") as IXUISprite;
                            if (icon != null)
                            {
                                icon.Color = Color.white ;
                            }
                        }
                    }
                }
            }
            if (Singleton<RoomManager>.singleton.IsLadderMode() && Singleton<RoomManager>.singleton.EGamePhase == EGamePhase.GAME_PHASE_CHOOSING)
            {
                //开始协程刷新被ban的神兽头像icon颜色
            }
        }
    }
    public void ShowCurSelectRole()
    {
        if (base.Prepared)
        {
            if (Singleton<RoomManager>.singleton.BeastSelectType == EGameBeastSelectType.GAME_BEAST_SELECT_TYPE_MATCH)
            {
                long emptyBeastId = Singleton<RoomManager>.singleton.GetEmptyBeastId(Singleton<PlayerRole>.singleton.ID);
                if (emptyBeastId > 0)
                {
                    this.m_lBeastIdCurrent = emptyBeastId;
                }
            }
            if (this.m_lBeastIdCurrent > 0)
            {
                this.SelectRole(this.m_lBeastIdCurrent);
            }
            else 
            {
                
                this.RefreshEquip(0, 0);
            }
            this.m_bShowOne = true;
        }
    }
    public void RefreshPlayerInfo(BeastData oBeast)
    {
        if (base.Prepared && oBeast != null)
        {
            if (Singleton<RoomManager>.singleton.RepeatSelectBeastInAllCamp == 0)
            {
                this.SetSelectBeastId(oBeast.Id, oBeast.BeastTypeId);
            }
            else
            {
                for (int i = 0; i < base.uiBehaviour.m_ourBeastList.Count; i++)
                {
                    IXUIListItem itemByIndex = base.uiBehaviour.m_ourBeastList.GetItemByIndex(i);
                    BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(itemByIndex.Id);
                    if (oBeast.Id == beastData.Id)
                    {
                        this.SetSelectBeastId(oBeast.Id, oBeast.BeastTypeId);
                    }
                }
            }
            IXUIListItem item = this.GetCampTypeListItem(oBeast.Id);
            if (item != null)
            {
                this.RefreshPlayerInfo(item, oBeast);
            }
            else 
            {
                Debug.Log("item == null");
            }
            this.RefreshBeastSelectStatus();
        }
    }
    /// <summary>
    /// 显示神兽的信息
    /// </summary>
    /// <param name="unBeastTypeId"></param>
    public void ShowBeastInfo(int unBeastTypeId)
    {
        if (base.Prepared)
        {
            DataBeastlist data = null;
            GameData<DataBeastlist>.dataMap.TryGetValue(unBeastTypeId, out data);
            this.ShowBeastBaseInfo(data, unBeastTypeId);
        }
    }
    /// <summary>
    /// 刷新装备
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="unEquipIndex"></param>
    public void RefreshEquip(long unBeastId,int unEquipIndex)
    {
        
    }
    public void RefreshSkill(BeastData oBeastData)
    {
        if (base.Prepared)
        {
            if (oBeastData != null)
            {
                if (Singleton<RoomManager>.singleton.RepeatSelectBeastInAllCamp == 0)
                {
                    this.SetSelectBeastId(oBeastData.Id, oBeastData.BeastTypeId);
                }
                else 
                {
                    for (int i = 0; i < base.uiBehaviour.m_ourBeastList.Count; i++)
                    {
                        IXUIListItem itemByIndex = base.uiBehaviour.m_ourBeastList.GetItemByIndex(i);
                        BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(itemByIndex.Id);
                        if (oBeastData.Id == beastData.Id)
                        {
                            this.SetSelectBeastId(oBeastData.Id, oBeastData.BeastTypeId);
                        }
                    }
                }
                IXUIListItem item = this.GetCampTypeListItem(oBeastData.Id);
                if (item != null)
                {
                    this.RefreshPlayerInfo(item,oBeastData);
                }
                this.RefreshSkill(oBeastData.Id,oBeastData.Skills);
            }
        }
    }
    /// <summary>
    /// 刷新操作提示，比如请选择神兽，确认装备等
    /// </summary>
    /// <param name="unBeastId"></param>
    public void RefreshTip(long unBeastId)
    {
        string strId = string.Empty;
        if (Singleton<RoomManager>.singleton.IsObserver)
        {
            strId = "DlgRoom.IsObservering";
        }
        else 
        {
            strId = "DlgRoom.PleaseSelectHero";
            if (0 != unBeastId)
            {
                BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(unBeastId);
                if (beastData != null && beastData.IsSelected)
                {
                    strId = "DlgRoom.FinishHeroSelect";
                }
            }
        }
        base.uiBehaviour.m_Label_Tips.SetText(StringConfigMgr.GetString(strId));
    }

	#endregion
	#region 私有方法
    /// <summary>
    /// 点击神兽列表里面的某个神兽
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private bool OnListItemOnClickOurBeast(IXUIObject obj)
    {
        Debug.Log("Onlistour");
        this.OnListOurBeastSelect(obj as IXUIListItem);
        return true;
    }
    private bool OnListItemOnClickEnemyBeast(IXUIObject obj)
    {
        Debug.Log("Onlistenemy");
        this.OnListEnemyBeastSelect(obj as IXUIListItem);
        return true;
    }
    /// <summary>
    /// 我方的玩家选择神兽
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool OnListOurBeastSelect(IXUIListItem item)
    {
        bool result;
        if (item == null || item.Id == 0u)
        {
            result = false;
        }
        else 
        {
            this.m_lBeastIdCurrent = item.Id;
            BeastData data = Singleton<RoomManager>.singleton.GetBeastData(this.m_lBeastIdCurrent);
            this.SelectRole(this.m_lBeastIdCurrent);
            result = true;
        }
        return result;
    }
    /// <summary>
    /// 敌方神兽被选择
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool OnListEnemyBeastSelect(IXUIListItem item)
    {
        bool result;
        if (item == null || 0u == item.Id)
        {
            result = false;
        }
        else
        {
            this.m_lBeastIdCurrent = item.Id;
            this.SelectRole(this.m_lBeastIdCurrent);
            result = true;
        }
        return result;
    }
    /// <summary>
    /// 点击确认选择按钮
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private bool OnButtonConfirmClick(IXUIButton button)
    {
        if (Singleton<RoomManager>.singleton.EGamePhase == EGamePhase.GAME_PHASE_CHOOSING)
        {
            this.SureSelectBeast();
        }
        return false;
    }
    private bool SureSelectBeast()
    {
        if (this.m_lBeastIdCurrent == 0)
        {
            return false;
        }
        else 
        {
            if (this.SelectBeastTypeId == -1)
            {
                return false;
            }
            else 
            {
                if (this.SelectBeastTypeId == this.m_unRandomBeastID)
                {
                    //处理随机选择
                }
                else 
                {
                    Singleton<NetworkManager>.singleton.SendConfirmBeast(this.m_lBeastIdCurrent);
                }
                return true;
            }
        }
    }
    private void ShowAllSkill(int beastId)
    {
        DataBeastlist data = null;
        GameData<DataBeastlist>.dataMap.TryGetValue(beastId, out data);
        if (data == null)
        {
            //清除技能
        }
        else 
        {
            List<int> skillList = new List<int>();
            skillList.Add(data.Skill_1);
            skillList.Add(data.Skill_2);
            skillList.Add(data.Skill_3);
            skillList.Add(data.Skill_4);
        }
    }
    /// <summary>
    /// 显示神兽基础信息
    /// </summary>
    /// <param name="data"></param>
    private void ShowBeastBaseInfo(DataBeastlist data,int unBeastTypeId)
    {

    }
    public void ShowCampTypeEffect(long beastId)
    {
        if (base.Prepared)
        {
            IXUIListItem campTypeItem = this.GetCampTypeListItem(beastId);
            if (campTypeItem != null)
            {
                this.ShowCampTypeEffect(campTypeItem);
            }
        }
    }
    private void ShowCampTypeEffect(IXUIListItem item)
    {
        if (item != null)
        {
            bool flag = true;
            BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(item.Id);
            if (beastData != null && beastData.BeastTypeId > -1 && beastData.IsSelected)
            {
                flag = false;
            }
            IXUISprite mask = item.GetUIObject("UnSelectMask") as IXUISprite;
            if (mask != null)
            {
                bool flag1 = item.Id > 0 && flag;
                if (flag1 && Singleton<RoomManager>.singleton.BeastSelectType == EGameBeastSelectType.GAME_BEAST_SELECT_TYPE_RANK && this.m_lBeastIdCurrent != item.Id)
                {
                    flag1 = false;
                }
                mask.SetVisible(flag1);
            }
            else 
            {
                Debug.Log("mask == null");
            }
            if (item.Id == this.m_lBeastIdCurrent)
            {
                this.IsSelected = !flag;
                this.RefreshTip(item.Id);
                if (!flag && !Singleton<RoomManager>.singleton.IsObserver)
                {
                    this.ShowSkinSelect();
                }
            }
        }
    }
    /// <summary>
    /// 显示英雄选择列表
    /// </summary>
    private void ShowBeastSelect()
    {
        base.uiBehaviour.m_BeastList.SetVisible(true);
    }
    /// <summary>
    /// 英雄选择完成之后选择皮肤
    /// </summary>
    private void ShowSkinSelect()
    {
 
    }
    /// <summary>
    /// 刷新确认按钮的激活状态
    /// </summary>
    /// <param name="bEnable"></param>
    private void RefreshOperation(bool bEnable)
    {
        this.RefrenshOperation_ConfirmButton.Enqueue(delegate
        {
            if (this.Prepared && !Singleton<RoomManager>.singleton.IsObserver)
            {
                this.uiBehaviour.m_Button_Confirm.SetEnable(bEnable);
            }
        });
    }
    /// <summary>
    /// 刷新所有玩家的信息
    /// </summary>
    private void RefreshAllPlayerInfo()
    {
        if (base.Prepared)
        {
            this.SetPlayerInfoToList(base.uiBehaviour.m_ourBeastList, Singleton<RoomManager>.singleton.OurPlayerDatas, Singleton<PlayerRole>.singleton.CampType);
            ECampType eCampType = (Singleton<PlayerRole>.singleton.CampType == ECampType.CAMP_EMPIRE) ? ECampType.CAMP_LEAGUE : ECampType.CAMP_EMPIRE;
            this.SetPlayerInfoToList(base.uiBehaviour.m_enmeyBeastList, Singleton<RoomManager>.singleton.EnemyPlayerDatas, eCampType);
        }
    }
    /// <summary>
    /// 我方和敌方的选择神兽列表刷新
    /// </summary>
    /// <param name="uiList"></param>
    /// <param name="listPlayerData"></param>
    /// <param name="eCampType"></param>
    private void SetPlayerInfoToList(IXUIList uiList, List<PlayerData> listPlayerData, ECampType eCampType)
    {
        if (uiList != null)
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                IXUIListItem item = uiList.GetItemByIndex(i);
                if (item != null)
                {
                    item.Clear();
                    IXUIList equip = item.GetUIObject("List_Equip") as IXUIList;
                    if (equip != null)
                    {
                        for (int j = 0; j < equip.Count; j++)
                        {
                            IXUIListItem equipItem = equip.GetItemByIndex(j);
                            if (equipItem != null)
                            {
                                equipItem.Clear();//清空装备icon
                            }
                        }
                    }
                    IXUIObject uiObject = item.GetUIObject("Texture_Shadow") as IXUIObject;
                    if (uiObject != null)
                    {
                        uiObject.SetVisible(true);
                    }
                    item.SetText("Label_Sequence", "");
                }
            }
            int num = 0;
            foreach (var playerData in listPlayerData)
            {
                foreach (var beastData in playerData.Beasts)
                {
                    
                    IXUIListItem item = (num >= uiList.Count) ? null : uiList.GetItemByIndex(num);
                    if (item != null)
                    {
                        this.RefreshPlayerInfo(item, playerData, beastData, eCampType);
                    }
                    num++;
                }
            }
            Debug.Log("Beast num=" + num);
        }
    }
    private void RefreshPlayerInfo(IXUIListItem uiListItem,PlayerData playerData,BeastData beast,ECampType eCampType)
    {
        if (beast != null)
        {
            this.SetSelectBeastId(beast.Id, beast.BeastTypeId);
            Debug.Log("Our BeastId:"+beast.Id);
            if (playerData != null  && uiListItem != null)
            {
                uiListItem.Clear();
                IXUILabel label = uiListItem.GetUIObject("Label_PlayerName") as IXUILabel;
                if (label != null)
                {
                    label.SetText(playerData.Name);
                }
                if (Singleton<RoomManager>.singleton.MathMode != EnumMathMode.EnumMathMode_Story)
                {
                    uiListItem.SetText("Label_Sequence", GetSequence(eCampType, uiListItem.Index).ToString());
                }
                this.RefreshPlayerInfo(uiListItem, beast);
            }
        }
    }
    private void RefreshPlayerInfo(IXUIListItem item, BeastData beast)
    {
        //DataBeastlist dataBeast = GameData<DataBeastlist>.dataMap[beast.BeastTypeId];
        DataBeastlist dataBeast = null;
        GameData<DataBeastlist>.dataMap.TryGetValue(beast.BeastTypeId, out dataBeast);
        IXUISprite icon = item.GetUIObject("Sprite_Icon") as IXUISprite;
        if (dataBeast != null)
        {
            //如果是自己的阵营，就显示头像。地方就显示随机头像
            if (beast.CampType == Singleton<PlayerRole>.singleton.CampType)
            {
                icon.SetSprite(dataBeast.IconFile,UIManager.singleton.GetAtlasName(EnumAtlasType.Beast,dataBeast.IconFile));
            }
            else
            {
                //icon.SetSprite("Window");
                icon.SetSprite(dataBeast.IconFile, UIManager.singleton.GetAtlasName(EnumAtlasType.Beast, dataBeast.IconFile));
            }
        }
        else 
        {
            if (beast.BeastTypeId == this.m_unRandomBeastID)
            {
                icon.SetSprite("9");
            }
        }
        bool visible = Singleton<RoomManager>.singleton.IsObserver || Singleton<PlayerRole>.singleton.CampType == beast.CampType;
        IXUIList equipList = item.GetUIObject("List_Equip") as IXUIList;
        if (equipList != null)
        {

        }
        else 
        {
            XLog.Log.Error("null == uiListEquip");
        }
        this.RefreshItemEnable(item, beast.Id);
        IXUIObject uiObject = item.GetUIObject("Texture_Shadow");
        if (uiObject != null)
        {
            uiObject.SetVisible(false);
        }
        item.Id = beast.Id;
    }
    /// <summary>
    /// 取得顺序
    /// </summary>
    /// <param name="eCampType"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private int GetSequence(ECampType eCampType, int index)
    {
        int result = 0;
        switch (eCampType)
        {
            case ECampType.CAMP_EMPIRE:
                if (index == 0)
                {
                    result = 1;
                }
                else
                {
                    if (index == 1)
                    {
                        result = 4;
                    }
                    else
                    {
                        if (index == 2)
                        {
                            result = 5;
                        }
                    }
                }
                break;
            case ECampType.CAMP_LEAGUE:
                if (index == 0)
                {
                    result = 2;
                }
                else
                {
                    if (index == 1)
                    {
                        result = 3;
                    }
                    else
                    {
                        if (index == 2)
                        {
                            result = 6;
                        }
                    }
                }
                break;
        }
        return result;
    }
    private IXUIListItem GetCampTypeListItem(long beastId)
    {
        if (!base.Prepared)
        {
            return null;
        }
        else 
        {
            IXUIListItem itemById = base.uiBehaviour.m_ourBeastList.GetItemById(beastId);
            if (null == itemById)
            {
                itemById = base.uiBehaviour.m_enmeyBeastList.GetItemById(beastId);
            }
            return itemById;
        }
    }
    /// <summary>
    /// 刷新该item是否被选择，如果选择就播放选择的动画
    /// </summary>
    /// <param name="uiItem"></param>
    /// <param name="unRoleId"></param>
    private void RefreshItemEnable(IXUIListItem uiItem,long unRoleId)
    {
        if (uiItem != null)
        {
            bool enableSelect = false;
            BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(unRoleId);
            if (beastData != null && beastData.PlayerId == Singleton<PlayerRole>.singleton.ID)
            {
                switch (Singleton<RoomManager>.singleton.BeastSelectType)
                {
                    case EGameBeastSelectType.GAME_BEAST_SELECT_TYPE_MATCH:
                        enableSelect = true;
                        break;
                    case EGameBeastSelectType.GAME_BEAST_SELECT_TYPE_RANK:
                        //if (this.m_unTurnRoleId == unRoleId || beastData.HTypeId > 0u)
                        //{
                       //     enableSelect = true;
                       // }
                        break;
                }
            }
            uiItem.SetEnableSelect(enableSelect);
        }
    }
    /// <summary>
    /// 确认选择神兽添加到dicRoleSelectBeastID中
    /// </summary>
    /// <param name="unBeastId"></param>
    /// <param name="unBeastTypeId"></param>
    private void SetSelectBeastId(long unBeastId,int unBeastTypeId)
    {
        if (this.m_dicRoleSelectBeastID.ContainsKey(unBeastId))
        {
            this.m_dicRoleSelectBeastID[unBeastId] = unBeastTypeId;
        }
        else 
        {
            this.m_dicRoleSelectBeastID.Add(unBeastId, unBeastTypeId);
        }
    }
    /// <summary>
    /// 显示不同类游戏的神兽选择列表
    /// </summary>
    private void ShowBeasts()
    {
        if (base.Prepared)
        {
            this.ShowRandomBeast();
            if (Singleton<RoomManager>.singleton.IsObserver)
            {
                this.ShowNormalBeasts();
            }
            else 
            {
                this.ShowNormalBeasts();
            }
        }
    }
    /// <summary>
    /// 显示随机神兽的icon
    /// </summary>
    private void ShowRandomBeast()
    {
        this.m_log.Debug("ShowRandomBeast");
        IXUIListItem item = base.uiBehaviour.m_BeastList.GetItemByIndex(0);
        if (item != null)
        {
            IXUISprite weekIcon = item.GetUIObject("Week") as IXUISprite;
            if (weekIcon != null)
            {
                weekIcon.SetVisible(false);
            }
            IXUISprite Icon = item.GetUIObject("Icon") as IXUISprite;
            if (Icon != null)
            {
                Icon.SetSprite("9");
                item.Id = this.m_unRandomBeastID;
                item.TipParam = new TipParam
                {
                    Tip = "随机选择一名您拥有的神兽，包括周免神兽"
                };
            }
        }
    }
    /// <summary>
    /// 显示普通模式的选择神兽列表
    /// </summary>
    private void ShowNormalBeasts()
    {
        if (base.Prepared)
        {
            bool isShowFreeBeast = Singleton<RoomManager>.singleton.IsShowFreeBeast;
            this.m_listActiveBeastId.Clear();
            List<DataBeastBook> data = GameData<DataBeastBook>.dataMap.Values.ToList<DataBeastBook>();
            int index = 1;
            int j=0;
            while (j < data.Count)
            {
                int beastId = data[j].BeastId;
                if (Singleton<RoomManager>.singleton.IsObserver || Singleton<PlayerRole>.singleton.IsBeastActive(beastId, false, isShowFreeBeast))
                {
                    IXUIListItem item;
                    if (index < base.uiBehaviour.m_BeastList.Count)
                    {
                        item = base.uiBehaviour.m_BeastList.GetItemByIndex(index);
                    }
                    else
                    {
                        item = base.uiBehaviour.m_BeastList.AddListItem();
                    }
                    if (item != null)
                    {
                        DataBeastlist dataBeastList = GameData<DataBeastlist>.dataMap[beastId];
                        if (dataBeastList != null)
                        {
                            IXUISprite sprite = item.GetUIObject("Week") as IXUISprite;
                            if (sprite != null)
                            {
                                bool flag = Singleton<PlayerRole>.singleton.IsFreeBeast(beastId);
                                sprite.SetVisible(flag && isShowFreeBeast);
                                if (flag)
                                {
                                    sprite.SetSprite("10");
                                }
                            }
                            item.SetVisible(true);
                            IXUISprite icon = item.GetUIObject("Icon") as IXUISprite;
                            icon.SetSprite(dataBeastList.IconFile, UIManager.singleton.GetAtlasName(EnumAtlasType.Beast, dataBeastList.IconFile));
                            item.TipParam = new TipParam
                            {
                                Tip = dataBeastList.Name
                            };
                            
                            item.Id = (long)dataBeastList.ID;
                            item.RegisterMouseOnEventHandler(new MouseOnEventHandler(this.BeastListMouseOn));
                            this.m_listActiveBeastId.Add(beastId);
                        }
                        else
                        {
                            item.SetVisible(false);
                        }
                        index++;
                    }
                }
                j++;
            }
            //是否神兽列表多余，就隐藏
            while (index < base.uiBehaviour.m_BeastList.Count)
            {
                IXUIListItem item = base.uiBehaviour.m_BeastList.GetItemByIndex(index);
                item.SetVisible(false);
                index++;
            }
        }
    }
    private bool BeastListMouseOn(IXUIObject obj)
    {
        IXUIListItem item = obj as IXUIListItem;
        return item != null;
    }
    /// <summary>
    /// 选择神兽
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool OnBeastListSelect(IXUIListItem item)
    {
        if (null == item)
        {
            return false;
        }
        //如果选择过了，就不能在选择其他神兽
        if (this.CanSelectOtherBeast())
        {
            return false;
        }
        else 
        {
            int id = (int)item.Id;
            //显示选择之后神兽的信息
            //this.ShowHeroInfo(id);
            if (!this.IsSucSelectBeast(ref id))
            {
                return false;
            }
            else 
            {
                this.SelectBeastTypeId = id;
                //显示技能
                //this.ShowAllSkill(id);
                //发送试图选择该神兽的请求
                this.SendSelectBeast(id);
                if (Singleton<RoomManager>.singleton.EGamePhase == EGamePhase.GAME_PHASE_BANNING)
                {
 
                }
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 是否能选择其他神兽，如果选择过了就不能再次选择
    /// </summary>
    /// <returns>true表示选择过了，false表示没有</returns>
    private bool CanSelectOtherBeast()
    {
        IXUIListItem selectItem = base.uiBehaviour.m_ourBeastList.GetSelectedItem();
        bool result;
        if (selectItem == null)
        {
            result = false;
        }
        else 
        {
            BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(selectItem.Id);
            result = (beastData != null && beastData.IsSelected);
        }
        return result;
    }
    /// <summary>
    /// 是否成功选择神兽，如果成功就返回神兽的TypeID
    /// </summary>
    /// <param name="unBeastTypeId"></param>
    /// <returns></returns>
    private bool IsSucSelectBeast(ref int unBeastTypeId)
    {
        if (Singleton<RoomManager>.singleton.IsObserver)
        {
            return true;
        }
        if (Singleton<RoomManager>.singleton.EGamePhase == EGamePhase.GAME_PHASE_BANNING)
        {
            return true;
        }
        IXUIListItem selectItem = base.uiBehaviour.m_ourBeastList.GetSelectedItem();
        if (selectItem == null)
        {
            //这里要处理错误提示信息
            return false;
        }
        BeastData beastData = Singleton<RoomManager>.singleton.GetBeastData(selectItem.Id);
        if (beastData == null)
        {
            return false;
        }
        if (beastData.IsSelected)
        {
            unBeastTypeId = beastData.BeastTypeId;
            return false;
        }
        if (this.m_unRandomBeastID == unBeastTypeId)
        {
            return true;
        }
        bool flag = false;
        //可以和对方重复选择一个神兽
        if (Singleton<RoomManager>.singleton.RepeatSelectBeastInAllCamp == 1)
        {
            flag = true;
            for (int i = 0; i < base.uiBehaviour.m_ourBeastList.Count; i++)
            {
                IXUIListItem itemByIndex = base.uiBehaviour.m_ourBeastList.GetItemByIndex(i);
                BeastData data = Singleton<RoomManager>.singleton.GetBeastData(itemByIndex.Id);
                //如果队友选了这个神兽，自己就不能再选择
                if (data.BeastTypeId == unBeastTypeId)
                {
                    flag = false;
                }
            }
        }
        foreach (var current in this.m_dicRoleSelectBeastID)
        {
            if (flag)
            {
                break;
            }
            if (current.Value > 0u && current.Value == unBeastTypeId)
            {
                if (selectItem.Id == current.Key)
                {
                    return false;
                }
                unBeastTypeId = beastData.BeastTypeId;
                BeastData otherBeastData = Singleton<RoomManager>.singleton.GetBeastData(current.Key);
                if (otherBeastData.PlayerId == beastData.PlayerId)
                {
                    //说明自己选择过了
                }
                else 
                {
                    //重复选择神兽
                }
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 刷新技能
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="listSkill"></param>
    private void RefreshSkill(long roleId, List<SkillGameData> listSkill)
    {
 
    }
    /// <summary>
    /// 发送选择该神兽消息,随便播放选择神兽声音
    /// </summary>
    /// <param name="beastId"></param>
    /// <returns></returns>
    private bool SendSelectBeast(int beastId)
    {
        if (beastId == -1 || this.m_lBeastIdCurrent == 0)
        {
            return false;
        }
        //这里我应该还有加入技能和装备，以后再来扩展
        Debug.Log(Singleton<RoomManager>.singleton.EGamePhase.ToString());
        if (!Singleton<RoomManager>.singleton.IsObserver && Singleton<RoomManager>.singleton.EGamePhase == EGamePhase.GAME_PHASE_CHOOSING)
        {
            if (Singleton<RoomManager>.singleton.IsLadderMode())
            {
                //这里是做天梯模式的处理
            } 
            this.PlayBeastSelectedVoice(beastId);
            Debug.Log("Send SelectId:" + this.m_lBeastIdCurrent);            
            Singleton<NetworkManager>.singleton.SendSelectBeast(this.m_lBeastIdCurrent,beastId);
        }
        return true;
    }
    /// <summary>
    /// 播放选择神兽后的声音
    /// </summary>
    /// <param name="unBeastId"></param>
    private void PlayBeastSelectedVoice(int unBeastId)
    {
 
    }
	#endregion
	#region 析构方法
	#endregion
}
