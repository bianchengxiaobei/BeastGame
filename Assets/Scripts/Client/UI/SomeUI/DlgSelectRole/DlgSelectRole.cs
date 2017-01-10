using UnityEngine;
using System.Collections.Generic;
using Client.UI.UICommon;
using Client.Common;
using Game;
using CharacterInfo = Game.CharacterInfo;
using Client.Data;
using Utility;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgSelectRole 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.9.18
// 模块描述：选择角色界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 选择角色界面
/// </summary>
public class DlgSelectRole : DlgBase<DlgSelectRole,DlgSelectRoleBehaviour>
{
	#region 字段
    private List<CharacterInfo> m_lCharacterList = null;
    private long m_bSelectedRoleId = -65535;
    private IXLog m_log = XLog.GetLog<DlgSelectRole>();
	#endregion
	#region 属性
    /// <summary>
    /// 选择上的角色Id
    /// </summary>
    public long SelectRoleId
    {
        get { return this.m_bSelectedRoleId; }
        set { this.m_bSelectedRoleId = value; }
    }
    public override string fileName
    {
        get
        {
            return "DlgSelectRole";
        }
    }
    public override int layer
    {
        get
        {
            return -5;
        }
    }
    public override uint Type
    {
        get
        {
            return 16u;
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
	#region 公共方法
    public override void Init()
    {
        base.Init();
        m_lCharacterList = ((CptcG2CNtf_CharacterInfo)CProtocol.GetProtocol(1003)).characters;
        if (null == m_lCharacterList || m_lCharacterList.Count == 0)
        {
            Debug.LogError("角色信息为空");
            return;
        }      
    }
    protected override void OnShow()
    {
        base.OnShow();
        ShowRoles();
    }
    public override void RegisterEvent()
    {
        base.RegisterEvent();
        base.uiBehaviour.m_Button_Create.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickCreateRoleButton));
        base.uiBehaviour.m_Button_Sure.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickEnterGameButton));
        base.uiBehaviour.m_List_RoleList.RegisterListClickEventHandler(new ListClickEventHandler(this.OnRoleListSelect));
    }
	#endregion
	#region 私有方法
    private void ShowRoles()
    {
        if (base.Prepared)
        {
            int i = 0;
            int index = 0;
            while (i < m_lCharacterList.Count)
            {
                CharacterInfo characterInfo = m_lCharacterList[i];
                int roleId = characterInfo.PlayerIndex;
                Debug.Log(roleId);
                IXUIListItem item;
                if (index < base.uiBehaviour.m_List_RoleList.Count)
                {
                    item = base.uiBehaviour.m_List_RoleList.GetItemByIndex(index);
                }
                else 
                {
                    item = base.uiBehaviour.m_List_RoleList.AddListItem();
                }
                if (item != null)
                {
                    DataPlayerList dataRoleList = GameData<DataPlayerList>.dataMap[roleId];
                    if (dataRoleList != null)
                    {
                        IXUILabel nameLabel = item.GetUIObject("name/nameLabel") as IXUILabel;
                        if (nameLabel != null)
                        {
                            Debug.Log(characterInfo.Name);
                            nameLabel.SetText("Lv."+characterInfo.Level+"  "+characterInfo.Name+"\n"+dataRoleList.Name);
                        }
                        else
                        {
                            Debug.Log("Label == null");
                        }
                        IXUISprite icon = item.GetUIObject("Icon") as IXUISprite;
                        if (icon != null)
                        {
                            icon.SetSprite(dataRoleList.IconFile);
                        }
                        else
                        {
                            Debug.Log("Icon == null");
                        }
                        item.SetVisible(true);
                        item.Id = characterInfo.PlayerId;
                    }
                    else
                    {
                        item.SetVisible(false);
                    }
                    index++;
                }
                else 
                {
                    Debug.Log("Item == null");
                }
                i++;
            }
        }
    }
    /// <summary>
    /// 点击创建角色按钮
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private bool OnClickCreateRoleButton(IXUIButton button)
    {
        Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_CreatPlayer);
        return true;
    }
    /// <summary>
    /// 点击进入游戏按钮（确认选择按钮）
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private bool OnClickEnterGameButton(IXUIButton button)
    {
        //取得选择角色的id，然后发送给服务器
        if (this.SelectRoleId != -65535)
        {
            NetworkManager.singleton.SendSelectRole(this.SelectRoleId);
        }
        else
        {
            m_log.Error("选择角色为空，请重新选择角色");
        }
        return true;
    }
    /// <summary>
    /// 某个角色被选择事件
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool OnRoleListSelect(IXUIListItem item)
    {
        if (null == item)
        {
            return false;
        }
        if (this.SelectRoleId != item.Id)
        {
            Debug.Log("Select Id:" + item.Id);
            this.SelectRoleId = item.Id;          
        }
        return true;
    }
	#endregion
	#region 析构方法
	#endregion
}
