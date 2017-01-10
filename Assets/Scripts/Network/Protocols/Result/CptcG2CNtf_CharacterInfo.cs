using UnityEngine;
using System.Collections.Generic;
using Game;
using CharacterInfo = Game.CharacterInfo;
using Utility;
using Client.UI.UICommon;
using Client.Logic;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcG2CNtf_CharacterInfo 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.20
// 模块描述：服务器发送给客户端角色列表消息
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 服务器发送给客户端角色列表消息，id=1003
/// </summary>
public class CptcG2CNtf_CharacterInfo : CProtocol
{
    private const int m_dwCptcG2CNtf_CharacterInfoId = 1003;
    public List<CharacterInfo> characters;
    public CptcG2CNtf_CharacterInfo()
        : base(1003)
    {
        characters = new List<CharacterInfo>();
    }
    public override CByteStream DeSerialize(CByteStream bs)
    {
        this.characters.Clear();
        int num = 0;
        bs.Read(ref num);
        for (int i = 0; i < num; i++)
        {
            CharacterInfo info = new CharacterInfo();
            bs.Read(info);
            characters.Add(info);
        }
        return bs;
    }
    public override CByteStream Serialize(CByteStream bs)
    {
        bs.Write(this.characters.Count);
        for (int i = 0; i < this.characters.Count; i++)
        {
            bs.Write(this.characters[i]);
        }
        return bs;
    }
    public override void Process()
    {
        Singleton<Login>.singleton.OnLoginSuccess(this);
        if (this.characters.Count != 0)
        {
            //进入选择角色的状态,显示选择游戏角色界面
            Singleton<ClientMain>.singleton.ChangeGameState(EnumGameState.eState_SelectPlayer);
        }
    }
}
