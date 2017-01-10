using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CRegister
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    internal class CRegister
    {
        /// <summary>
        /// 注册所有消息协议
        /// </summary>
        public static void RegistProtocol()
        {
            CProtocol.Register(new CptcCReq_Login());//1002
            CProtocol.Register(new CPtcCNtf_LoginResult());//1000
            CProtocol.Register(new CptcCReq_CreateRole());//1004
            CProtocol.Register(new CptcG2CNtf_CharacterInfo());//1003
            CProtocol.Register(new CPtcCNtf_EnterLobby());//1005
            CProtocol.Register(new CptcCReq_AutoMatch());//1006
            CProtocol.Register(new CptcM2CNtf_AutoMatchResult());//1007
            CProtocol.Register(new CptcM2CNtf_MatchStart());//1008
            CProtocol.Register(new CptcCNtf_EnterRoom());//1009
            CProtocol.Register(new CptcC2MReq_TrySelectBeast());//1010
            CProtocol.Register(new CptcC2MReq_SelectBeast());//1011
            CProtocol.Register(new CptcM2CNtf_TrySelectBeast());//1012
            CProtocol.Register(new CptcM2CNtf_SelectBeast());//1013
            CProtocol.Register(new CptcM2CNtf_GamePrepare());//1014
            CProtocol.Register(new CPtcCNtf_EnterScene());//1015
            CProtocol.Register(new CptcCReq_SelectRole());//1016
            CProtocol.Register(new CptcCReq_EnterScene());//1017
            CProtocol.Register(new CptcM2CNtf_SceneLoaded());//1018
            CProtocol.Register(new CPtcCNtf_CDBegin());//1019
            CProtocol.Register(new CptcM2CNtf_ChangeHp());//1020
            CProtocol.Register(new CPtcCNtf_StartGame());//1021
            CProtocol.Register(new CptcG2CNtf_SelectBornPos());//1022
            CProtocol.Register(new CPtcCReq_AddRoleToScene());//1023
            CProtocol.Register(new CPtcCNtf_AddRoleToScene());//1024
            CProtocol.Register(new CPtcM2CNtf_StartBeastRound());//1025 
            CProtocol.Register(new CptcC2MReq_BeastEnterStage());//1026
            CProtocol.Register(new CptcM2CNtf_EnterStage());//1027
            CProtocol.Register(new CPtcC2MReq_Move());//1028
            CProtocol.Register(new CPtcM2CNtf_Move());//1029
            CProtocol.Register(new CptcC2MReq_EndRoleStage());//1030
            CProtocol.Register(new CPtcC2MReq_CastSkill());//1031
        }
    }
}
