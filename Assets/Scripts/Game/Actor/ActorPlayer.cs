using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActorPlayer 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：玩家角色Actor对象
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 玩家角色Actor对象
/// </summary>
/// 
namespace Game
{
    public class ActorPlayer<T> : ActorParent<T> where T : EntityPlayer
    {
        void Start()
        {

        }
        void Update()
        {
            ActChange();
        }
    }
}
