using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SoundData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class SoundData : GameData<SoundData>
    {
        public string path { get; protected set; }
        public static readonly string fileName = "SoundData";
    }
}
