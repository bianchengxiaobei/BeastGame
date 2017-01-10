using UnityEngine;
using Client.UI.UICommon;
using System.Collections.Generic;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIButton
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
[AddComponentMenu("XUI/XUIButton")]
public class XUIButton : XUIObject, IXUIObject, IXUIButton
{
    private UIButtonOffset m_uiButtonOffset;
    private UIButtonScale m_uiButtonScale;
    private UIButtonColor m_uiButtonColor;
    private UIPlaySound[] m_uiButtoSounds;
    private Collider m_colliderCached;
    private UISprite[] m_uiSprites;
    private UITexture[] m_uiTextures;
    private UIHover[] m_uiHovers;
    private UIButtonKeyBinding[] m_ButtonKeyBinds;
    private ButtonClickEventHandler m_buttonClickEventHandler;
    private bool m_bEnable = true;
    private Dictionary<UILabel, Color> m_dicInitColorLabel = new Dictionary<UILabel, Color>();
    public override bool IsEnableOpen
    {
        set
        {
            if (value != this.IsEnableOpen)
            {
                base.IsEnableOpen = value;
                if (null != this.m_uiButtonColor)
                {
                    this.m_uiButtonColor.enabled = value;
                }
                if (null != this.m_uiButtonScale)
                {
                    this.m_uiButtonScale.enabled = value;
                }
                if (null != this.m_uiButtonOffset)
                {
                    this.m_uiButtonOffset.enabled = value;
                }
                UIPlaySound[] uiButtoSounds = this.m_uiButtoSounds;
                for (int i = 0; i < uiButtoSounds.Length; i++)
                {
                    UIPlaySound uIButtonSound = uiButtoSounds[i];
                    if (null != uIButtonSound)
                    {
                        uIButtonSound.enabled = value;
                    }
                }
                UIHover[] uiHovers = this.m_uiHovers;
                for (int j = 0; j < uiHovers.Length; j++)
                {
                    UIHover uIHover = uiHovers[j];
                    if (null != uIHover)
                    {
                        uIHover.enabled = value;
                    }
                }
                UIButtonKeyBinding[] buttonKeyBinds = this.m_ButtonKeyBinds;
                for (int k = 0; k < buttonKeyBinds.Length; k++)
                {
                    UIButtonKeyBinding uIButtonKeyBinding = buttonKeyBinds[k];
                    if (null != uIButtonKeyBinding)
                    {
                        uIButtonKeyBinding.enabled = value;
                    }
                }
                UISprite[] uiSprites = this.m_uiSprites;
                for (int m = 0; m < uiSprites.Length; m++)
                {
                    UISprite uISprite = uiSprites[m];
                    if (null != uISprite)
                    {
                        uISprite.color = ((!value) ? UnityTools.GrayColor : Color.white);
                    }
                }
                UITexture[] uiTextures = this.m_uiTextures;
                for (int n = 0; n < uiTextures.Length; n++)
                {
                    UITexture uITexture = uiTextures[n];
                    if (null != uITexture)
                    {
                        uITexture.color = ((!value) ? UnityTools.GrayColor : Color.white);
                    }
                }
                foreach (KeyValuePair<UILabel, Color> current in this.m_dicInitColorLabel)
                {
                    current.Key.color = ((!value) ? new Color(0.75f, 0.75f, 0.75f) : current.Value);
                }
            }
        }
    }
    public void SetCaption(string strText)
    {
        UILabel componentInChildren = base.GetComponentInChildren<UILabel>();
        if (null != componentInChildren)
        {
            componentInChildren.text = strText;
        }
    }
    public bool IsEnable()
    {
        return this.m_bEnable;
    }
    public void SetEnable(bool bEnable)
    {
        this.m_bEnable = bEnable;
        if (null != this.m_colliderCached)
        {
            this.m_colliderCached.enabled = bEnable;
        }
        if (null != this.m_uiButtonColor)
        {
            this.m_uiButtonColor.enabled = bEnable;
        }
        if (null != this.m_uiButtonScale)
        {
            this.m_uiButtonScale.enabled = bEnable;
        }
        if (null != this.m_uiButtonOffset)
        {
            this.m_uiButtonOffset.enabled = bEnable;
        }
        UIButtonKeyBinding[] buttonKeyBinds = this.m_ButtonKeyBinds;
        for (int j = 0; j < buttonKeyBinds.Length; j++)
        {
            UIButtonKeyBinding uIButtonKeyBinding = buttonKeyBinds[j];
            if (null != uIButtonKeyBinding)
            {
                uIButtonKeyBinding.enabled = bEnable;
            }
        }
        UISprite[] uiSprites = this.m_uiSprites;
        for (int k = 0; k < uiSprites.Length; k++)
        {
            UISprite uISprite = uiSprites[k];
            if (null != uISprite)
            {
                uISprite.color = ((!bEnable) ? Color.gray : Color.white);
            }
        }
        foreach (KeyValuePair<UILabel, Color> current in this.m_dicInitColorLabel)
        {
            current.Key.color = ((!bEnable) ? Color.gray : current.Value);
        }
    }
    public void RegisterClickEventHandler(ButtonClickEventHandler eventHandler)
    {
        this.m_buttonClickEventHandler = eventHandler;
    }
    protected override void _OnClick()
    {
        base._OnClick();
        if (this.m_buttonClickEventHandler != null && this.m_buttonClickEventHandler(this))
        {
            XUITool.Instance.IsEventProcessed = true;
        }
    }
    public override void Init()
    {
        base.Init();
        this.m_colliderCached = base.collider;
        if (null == this.m_colliderCached)
        {
            Debug.Log("null == m_colliderCached");
        }
        this.m_uiButtonColor = base.GetComponent<UIButtonColor>();
        this.m_uiButtonScale = base.GetComponent<UIButtonScale>();
        this.m_uiButtonOffset = base.GetComponent<UIButtonOffset>();
        this.m_uiButtoSounds = base.GetComponents<UIPlaySound>();
        this.m_uiSprites = base.GetComponentsInChildren<UISprite>(true);
        this.m_uiTextures = base.GetComponentsInChildren<UITexture>(true);
        this.m_uiHovers = base.GetComponents<UIHover>();
        this.m_ButtonKeyBinds = base.GetComponents<UIButtonKeyBinding>();
        this.m_dicInitColorLabel.Clear();
        UILabel[] componentsInChildren = base.GetComponentsInChildren<UILabel>(true);
        UILabel[] array = componentsInChildren;
        for (int i = 0; i < array.Length; i++)
        {
            UILabel uILabel = array[i];
            if (null != uILabel && !this.m_dicInitColorLabel.ContainsKey(uILabel))
            {
                this.m_dicInitColorLabel.Add(uILabel, uILabel.color);
            }
        }
    }
}

