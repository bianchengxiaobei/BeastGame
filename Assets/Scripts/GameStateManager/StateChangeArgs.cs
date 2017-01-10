using UnityEngine;
using System;
using System.Collections;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：StateChangeArgs
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：状态改变参数
//----------------------------------------------------------------*/
#endregion
public class StateChangeArgs 
{
    public EnumGameState GameState;
    public ELoadingStyle LoadingStyle;
    public Action CallBack;
}
