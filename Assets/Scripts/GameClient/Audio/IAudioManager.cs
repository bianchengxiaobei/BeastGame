using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IAudioManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Audio
{
    public interface IAudioManager
    {
        float VolumeMain { get;}
        float VolumeEffect { get; }
        bool Enable { get; }
        bool EnableEffect { get; }
    }
}