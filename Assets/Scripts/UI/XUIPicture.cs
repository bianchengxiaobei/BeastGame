using Client.UI.UICommon;
using System;
using UnityAssetEx.Export;
using UnityEngine;
using Client.Common;
/// <summary>
/// UITexture
/// </summary>
[AddComponentMenu("XUI/XUIPicture")]
public class XUIPicture : XUIObject, IXUIObject, IXUIPicture
{
    private UITexture m_uiTexture;
    private string m_strTextureFile = string.Empty;
    private IAssetRequest m_assetRequestOld;
    private IAssetRequest m_assetRequest;
    public Color Color
    {
        get
        {
            if (null != this.m_uiTexture)
            {
                return this.m_uiTexture.color;
            }
            return Color.white;
        }
        set
        {
            if (null != this.m_uiTexture)
            {
                this.m_uiTexture.color = value;
            }
        }
    }
    /// <summary>
    /// 设置Texture
    /// </summary>
    /// <param name="texture"></param>
    public void SetTexture(Texture texture)
    {
        if (null != this.m_uiTexture)
        {
            this.m_uiTexture.mainTexture = texture;
        }
    }
    /// <summary>
    /// 根据路径加载后设置Texture
    /// </summary>
    /// <param name="strTextureFile"></param>
    public void SetTexture(string strTextureFile)
    {
        if (string.IsNullOrEmpty(strTextureFile))
        {
            return;
        }
        if (null != this.m_uiTexture && !this.m_strTextureFile.Equals(strTextureFile))
        {
            this.m_strTextureFile = strTextureFile;
            if (this.m_assetRequest != null)
            {
                if (this.m_assetRequest.IsFinished)
                {
                    if (this.m_assetRequestOld != null)
                    {
                        this.m_assetRequestOld.Dispose();
                        this.m_assetRequestOld = null;
                    }
                    this.m_assetRequestOld = this.m_assetRequest;
                }
                else
                {
                    this.m_assetRequest.Dispose();
                }
                this.m_assetRequest = null;
            }
            this.m_assetRequest = XUITool.Instance.LoadResAsyn(this.m_strTextureFile, new AssetRequestFinishedEventHandler(this.OnLoadTextureFinished), AssetPRI.DownloadPRI_Low);
        }
    }
    public override void Init()
    {
        base.Init();
        this.m_uiTexture = base.GetComponent<UITexture>();
        if (null == this.m_uiTexture)
        {
            Debug.LogError("null == m_uiTexture");
        }
    }
    private void OnLoadTextureFinished(IAssetRequest assetRequest)
    {
        IAssetResource assetResource = assetRequest.AssetResource;
        if (assetResource == null)
        {
            return;
        }
        UnityEngine.Object mainAsset = assetResource.MainAsset;
        Texture texture = mainAsset as Texture;
        if (null == texture)
        {
            XLog.Log.Error("texture == null");
            return;
        }
        this.m_uiTexture.mainTexture = texture;
        this.m_uiTexture.enabled = true;
        if (this.m_assetRequestOld != null)
        {
            this.m_assetRequestOld.Dispose();
            this.m_assetRequestOld = null;
        }
    }
    private void OnDestroy()
    {
        if (this.m_assetRequestOld != null)
        {
            this.m_assetRequestOld.Dispose();
            this.m_assetRequestOld = null;
        }
        if (this.m_assetRequest != null)
        {
            this.m_assetRequest.Dispose();
            this.m_assetRequest = null;
        }
    }
}