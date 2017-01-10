using UnityEngine;
using System.Collections;
using Utility.Export;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataMaplist
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：地图信息数据类
//----------------------------------------------------------------*/
#endregion
namespace GameData
{
    public class DataMaplist : GameData<DataMaplist>
    {
        public static readonly string fileName = "maplist";
        public int ID
        {
            get;
             set;
        }
        public string Name
        {
            get;
             set;
        }
        public int MapType
        {
            get;
             set;
        }
        public string MapFile
        {
            get;
             set;
        }
        public string MapFilePath
        {
            get;
             set;
        }
        public string PicFile
        {
            get;
             set;
        }
        public string BgSoundFile
        {
            get;
             set;
        }
       /// <summary>
       /// 血量小时的背景音乐
       /// </summary>
        public string WarnSoundFile
        {
            get;
             set;
        }
        public string ToughSoundFile
        {
            get;
             set;
        }
        public string NormalSoundFile
        {
            get;
             set;
        }
        public string LCameraPos
        {
            get;
             set;
        }
        public string LCameraAngle
        {
            get;
             set;
        }
        public string ECameraPos
        {
            get;
             set;
        }
        public string ECameraAngle
        {
            get;
             set;
        }
        public float MapCenterX
        {
            get;
             set;
        }
        public float MapCenterY
        {
            get;
             set;
        }
        public float MaxX
        {
            get;
             set;
        }
        public float MaxY
        {
            get;
             set;
        }
        public float MinCameraScale
        {
            get;
             set;
        }
        public float MaxCameraScale
        {
            get;
            set;
        }
        public float MouseWheelSensitivity
        {
            get;
             set;
        }
        public int LeagueBaseDeadEft
        {
            get;
             set;
        }
        public int EmpireBaseDeadEft
        {
            get;
             set;
        }
        public int MineBaseDeadEft
        {
            get;
             set;
        }
        public int TheirBaseDeadEft
        {
            get;
             set;
        }
        public int LeagueAttackedEffect
        {
            get;
             set;
        }
        public int EmpireAttackedEffect
        {
            get;
             set;
        }
        public int LeagueEffectWhenHeroDead
        {
            get;
             set;
        }
        public int EmpireEffectWhenHeroDead
        {
            get;
             set;
        }
        public int LeagueHighHpEffect
        {
            get;
             set;
        }
        public int EmpireHighHpEffect
        {
            get;
             set;
        }
        public int LeagueLowHpEffect
        {
            get;
             set;
        }
        public int EmpireLowHpEffect
        {
            get;
             set;
        }
    }
}
