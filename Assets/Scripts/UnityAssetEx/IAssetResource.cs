using UnityEngine;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IAssetResource
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：资源管理接口
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public delegate void AssetLoadFinishedEventHandler(IAssetResource assetResource);
    public interface IAssetResource : IDisposable
    {
        string URL
        {
            get;
        }
        bool IsDone
        {
            get;
        }
        UnityEngine.Object MainAsset
        {
            get;
        }
        string Text
        {
            get;
        }
        byte[] Bytes
        {
            get;
        }
        Texture Texture
        {
            get;
        }
        AudioClip Audio
        {
            get;
        }
        float progress
        {
            get;
        }
    }
    /// <summary>
    /// 资源类型，（ab和resources）无定义、abPrefab、abTexture、abAudio、abShader、abFont、text、texture、streamAudio、movie
    /// </summary>
    public enum EnumAssetType
    {
        eAssetType_Undefined,
        eAssetType_AssetBundlePrefab,
        eAssetType_AssetBundleTexture,
        eAssetType_AssetBundleAudio,
        eAssetType_AssetBundleShader,
        eAssetType_AssetBundleFont,
        eAssetType_Text,
        eAssetType_Texture,
        eAssetType_StreamAudio,
        eAssetType_Movie,
        eAssetType_Scene
    }
}