using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ClientState_Story 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.12
// 模块描述：游戏剧情状态
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏剧情状态
/// </summary>
public class ClientState_Story : ClientState_Base
{
    public override void OnEnter()
    {
        base.OnEnter();
        //显示所有剧情的界面，比如第一章....
    }
    public override void OnLeave()
    {
        base.OnLeave();
    }
}
