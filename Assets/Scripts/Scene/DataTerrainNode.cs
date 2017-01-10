using UnityEngine;
using System.Collections;
using Utility.Export;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataTerrainNode
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.17
// 模块描述：地形节点配置信息类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class DataTerrainNode : GameData<DataTerrainNode>
    {
        public const string fileName = "DataTerrainNode";
        public int ID
        {
            get;
            private set;
        }
        public string Type
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public string Tip
        {
            get;
            private set;
        }
        public int EffectId
        {
            get;
            private set;
        }
        public int TriggerEffectId
        {
            get;
            private set;
        }
        public int StandEffectId
        {
            get;
            private set;
        }
        public int BywayEffectId
        {
            get;
            private set;
        }
        public string Colorname
        {
            get;
            private set;
        }      
    }
}