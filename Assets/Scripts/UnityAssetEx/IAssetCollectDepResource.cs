using UnityEngine;
using System;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IAssetCollectDepResource
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：带引用资源管理接口
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public interface IAssetCollectDepResource : IDisposable
    {
        string URL
        {
            get;
            set;
        }
        void SetAsset(IAssetResource assetSO);
        void SetDepSize(int length);
        void AddDep(IAssetResource depSO, int index);
        void AssetComplete(IAssetResource assetSO);
    }
}
