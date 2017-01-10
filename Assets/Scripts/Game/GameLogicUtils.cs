using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameLogicUtils
// 创建者：chen
// 修改者列表：
// 创建日期：2016.1.22
// 模块描述：游戏逻辑通用工具类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class GameLogicUtils
    {
        #region 字段
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        /*public static List<List<uint>> GetEntitiesInRange(Matrix4x4 ltwM,Quaternion rotation, Vector3 forward, Vector3 position,
            float radius, float offsetX = 0,float offsetY = 0, float angleOffset = 0, LayerMask layerMask = LayerMask.Character | LayerMask.Monster | LayerMask.Trap)
        {
            List<List<uint>> list = new List<List<uint>>();
            List<uint> listDummy = new List<uint>();
            List<uint> listMonster = new List<uint>();
            List<uint> listPlayer = new List<uint>();
            List<uint> listMercenary = new List<uint>();
            //如果游戏世界里面没有包含主角,就加入其中
            if (!GameWorld.Entities.ContainsKey(GameWorld.thePlayer.ID))
            {
                GameWorld.Entities.Add(GameWorld.thePlayer.ID, GameWorld.thePlayer);
            }
        }
         * */
        #endregion
        #region 私有方法
        #endregion
    }
    public enum LayerMask
    {
        Default = 1,
        Character = 256,
        Terrain = 512,
        Monster = 2048,
        NPC = 4096,
        Trap = 131072,
    }
}