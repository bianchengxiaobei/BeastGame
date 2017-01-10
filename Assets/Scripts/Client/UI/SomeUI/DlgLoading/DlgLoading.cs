using UnityEngine;
using System.Collections.Generic;
using Client.UI;
using Client.UI.UICommon;
using Utility;
using Client.Data;
using UnityAssetEx.Export;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgLoading
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.9
// 模块描述：加载界面处理类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 加载界面处理类
/// </summary>
public class DlgLoading :DlgBase<DlgLoading,DlgLoadingBehaviour>
{
	#region 字段
    private bool m_bStartTime;
    private int m_iDelayTime;
    private float m_fStartTime;
    private float m_fProgressValue = 0f;
    private int m_iCampNum = 3;//一方阵营神兽的数量
	#endregion
	#region 属性
    public override string fileName
    {
        get
        {
            return "DlgLoading";
        }
    }
    public override int layer
    {
        get
        {
            return -6;
        }
    }
    public override uint Type
    {
        get
        {
            return 65536u;
        }
    }
    public override bool IsPersist
    {
        get
        {
            return true;
        }
    }
    public override Client.Common.EnumDlgCamera ShowType
    {
        get
        {
            return Client.Common.EnumDlgCamera.Top;
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
        this.ShowBaseInfo();
        this.ShowOurBeastInfo();
        this.ShowEnemyBeastInfo();
    }
    /// <summary>
    /// 更新进度条
    /// </summary>
    public override void Update()
    {
        base.Update();
        if (this.m_bStartTime)
        {
            if (Time.time - this.m_fStartTime > (float)this.m_iDelayTime)
            {
                this.m_bStartTime = false;
                Camera.main.cullingMask = UnityGameEntry.Instance.MainCameraCullingMask;
                this.SetVisible(false);
            }
        }
        //这里是加载时的进度条，我不打算做，忽略掉
        /*if (base.Prepared)
        {
            if (this.m_fProgressValue < 1f)
            {
                this.m_fProgressValue = (ResourceManager.singleton.ProgressValue + CSceneMgr.singleton.ProcessValue) / 2f;
            }
            //base.uiBehaviour.m_progressBar.value = this.m_fProgressValue;
        }*/
    }
    /// <summary>
    /// 所有资源全部加载完成之后回调，发送给服务器进行确认，然后进入战斗
    /// </summary>
    public void OnAllPrepared()
    {
        Debug.Log("OnAllPrepared");
        //发送进入战斗的消息给服务器
        Singleton<NetworkManager>.singleton.SendEnterMainScene();
        Singleton<RoomManager>.singleton.PlayerRoleData.IsLoadFinish = true;
        //刷新loading的角色mask，主要是等待中去掉
        this.RefreshMask();
    }
    //刷新加载完成后去除的遮罩，实际上LOL并没有，所以我这里不采用，只是设置几个变量
    public void RefreshMask()
    {
        if (base.Prepared && base.IsVisible())
        {
            bool flag = true;
            for (int i = 0; i < base.uiBehaviour.m_list_OurPlayer.Count; i++)
            {
                IXUIListItem item = base.uiBehaviour.m_list_OurPlayer.GetItemByIndex(i);
                if (item != null)
                {
                    flag = this.IsFinishLoad(item);
                }
            }
            for (int i = 0; i < base.uiBehaviour.m_list_EnemyPlayer.Count; i++)
            {
                IXUIListItem item = base.uiBehaviour.m_list_EnemyPlayer.GetItemByIndex(i);
                if (item != null)
                {
                    flag = this.IsFinishLoad(item);
                }
            }
            if (flag)
            {
                //过几秒中Loading界面消失，在Update里面更新时间
                this.m_fStartTime = Time.time;
                this.m_bStartTime = true;
            }          
        } 
    }
	#endregion
	#region 私有方法
    /// <summary>
    /// 显示加载界面的基础信息
    /// </summary>
    private void ShowBaseInfo()
    {
        this.m_fProgressValue = 0f;
        Camera.main.cullingMask = 0;//默认摄像机的剔除遮罩为Nothing
        string tip = StringConfigMgr.GetTips();//随机取出提示
        base.uiBehaviour.m_label_Tip.SetText(tip);//设置提示Label的值
        this.m_iDelayTime = 2;//这里应该从配置文件中读取，然后赋值.gameconfig.xml
    }
    /// <summary>
    /// 显示加载界面的我方角色的信息
    /// </summary>
    private void ShowOurBeastInfo()
    {
        List<PlayerData> ourPlayers = Singleton<RoomManager>.singleton.OurPlayerDatas;
        int index = 0;
        foreach (var player in ourPlayers)
        {
            foreach (var beast in player.Beasts)
            {
                if (index < this.m_iCampNum)
                {
                    IXUIListItem item = base.uiBehaviour.m_list_OurPlayer.GetItemByIndex(index);
                    //如果该List下面没有item，就动态添加item
                    if (null == item)
                    {
                        item = base.uiBehaviour.m_list_OurPlayer.AddListItem();
                    }
                    this.ShowBeastInfo(item, player, beast);
                    index++;
                }
            }
        }
        while (index < base.uiBehaviour.m_list_OurPlayer.Count)
        {
            IXUIListItem item = base.uiBehaviour.m_list_OurPlayer.GetItemByIndex(index);
            if (item != null)
            {
                item.SetVisible(false);
            }
            index++;
        }
    }
    /// <summary>
    /// 显示神兽信息界面
    /// </summary>
    /// <param name="item"></param>
    /// <param name="playerData"></param>
    /// <param name="beastData"></param>
    private void ShowBeastInfo(IXUIListItem item,PlayerData playerData, BeastData beastData)
    {
        if (item != null && playerData != null && beastData != null)
        {
            item.SetText("NickNameAndLevel/Label_Player_Name", playerData.Name);
            item.SetText("NickNameAndLevel/Label_Player_Level", "LV" + playerData.Level);
            item.SetSprite("Icon/Sprite_Player_Icon", playerData.Icon);
            DataBeastlist beastList = GameData<DataBeastlist>.dataMap[beastData.BeastTypeId];
            if (beastList != null)
            {
                //取得皮肤数据
                DataSuit suit = null;
                GameData<DataSuit>.dataMap.TryGetValue(beastData.BeastTypeId,out suit);
                //神兽名字
                string beastName = string.IsNullOrEmpty(beastList.NickName) ? "" : ("-" + beastList.NickName);
                //神兽皮肤名字
                string beastNickname = (suit == null) ? beastList.Name : suit.Name;
                item.SetText("Label_Beast_Name", string.Format("{0}{1}", beastNickname, beastName));
                //设置神兽皮肤
                IXUIPicture beastSkin = item.GetUIObject("Picture_Beast_Skin") as IXUIPicture;
                if (beastSkin != null)
                {
                    beastSkin.SetVisible(true);
                    beastSkin.SetTexture(string.Format("Texture/Beast/{0}", (suit == null) ? beastList.ModelFile : suit.PicName));
                }
            }
            else 
            {
                item.SetVisible(false);
            }
            item.Id = playerData.PlayerId;
            item.SetVisible(true);
        }
    }
    private void ShowEnemyBeastInfo()
    {
        List<PlayerData> enemyPlayers = Singleton<RoomManager>.singleton.EnemyPlayerDatas;
        int index = 0;
        foreach (var player in enemyPlayers)
        {
            foreach (var beast in player.Beasts)
            {
                if (index < this.m_iCampNum)
                {
                    IXUIListItem item = base.uiBehaviour.m_list_EnemyPlayer.GetItemByIndex(index);
                    if (null == item)
                    {
                        item = base.uiBehaviour.m_list_EnemyPlayer.AddListItem();
                    }
                    this.ShowBeastInfo(item, player, beast);
                    index++;
                }
            }
        }
        while (index < base.uiBehaviour.m_list_EnemyPlayer.Count)
        {
            IXUIListItem item = base.uiBehaviour.m_list_EnemyPlayer.GetItemByIndex(index);
            if (item != null)
            {
                item.SetVisible(false);
            }
            index++;
        }
    }
    /// <summary>
    /// 是否该玩家加载完成
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool IsFinishLoad(IXUIListItem item)
    {
        bool result = true;
        if (item != null)
        {
            PlayerData player = Singleton<RoomManager>.singleton.GetPlayerDataById(item.Id);
            if (player != null)
            {
                if (player.IsLoadFinish)
                {
                    //这里做加载完成的界面显示处理
                }
                else
                {
                    result = false;
                }
            }
        }
        return result;
    }
    #endregion
}
