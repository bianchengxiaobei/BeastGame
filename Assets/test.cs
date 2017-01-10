using UnityEngine;
using System.Collections.Generic;
using Game;
using Utility;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：test
// 创建者：chen
// 修改者列表：
// 创建日期：
// 模块描述：
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public class test : MonoBehaviour
{
    void Start()
    {
        ResourceManager.singleton.Init(this.GetComponent("GameResourceManager") as IResourceManager);
        GameWorld.Init();
        RoleAttachedInfo info = new RoleAttachedInfo();
        EventDispatch.TriggerEvent<RoleAttachedInfo>(Game.Event.FrameWorkEvent.EntityAttached,info);
    }
    void Update()
    {
        ResourceManager.singleton.Update();
    }
}
