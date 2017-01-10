using UnityEngine;
using System;
using System.Collections;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IResourceManager
// 创建者：chen
// 修改者列表：
// 创建日期：2015/12/23
// 模块描述：资源管理器接口
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public interface IResourceManager
    {
        /// <summary>
        /// 加载进度
        /// </summary>
        float ProgressValue
        {
            get;
        }
        void Init(string strBaseResDir, string strBaseResWWWDir);
        void Clear();
        void OnUpdate();
        IAssetRequest LoadUI(string strDlgName, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest LoadTexture(string url, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest LoadAudio(string url, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest LoadAtlas(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest LoadModel(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest LoadScene(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest LoadEffect(string relativeUrl, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest CreateAssetRequest(string url, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType);
        IAssetRequest CreateAssetRequest(string url, AssetRequestFinishedEventHandler callBackFun, AssetPRI assetPRIType, EnumAssetType eAssetType);
        void SetAllLoadFinishedEventHandler(Action<bool> eventHandler);
        void SetAllUnLoadFinishedEventHandler(Action<bool> eventHandler);
    }
}
