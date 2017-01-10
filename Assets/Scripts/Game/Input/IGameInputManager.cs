using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IGameInputManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：游戏世界输入设备管理器接口
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏世界输入设备管理器接口
/// </summary>
namespace Game {
    public interface IGameInputManager
    {
        bool IsMoving
        {
            get;
        }
        Vector2 Direction
        {
            get; set;
        }
        Vector3 OrginPos
        {
            get;
            set;
        }
        void Init(EntityParent theOwner);
        void Reset();
    }
}