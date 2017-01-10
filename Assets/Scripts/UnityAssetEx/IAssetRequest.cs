using UnityEngine;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IAssetRequest
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public delegate void AssetRequestFinishedEventHandler(IAssetRequest assetRequest);
    public interface IAssetRequest : IDisposable
    {
        IAssetResource AssetResource
        {
            get;
        }
        bool IsError
        {
            get;
        }
        /// <summary>
        /// 资源已经加载完成
        /// </summary>
        bool IsFinished
        {
            get;
        }
        object Data
        {
            get;
            set;
        }
        bool RemoveQuickly
        {
            get;
            set;
        }
    }
}