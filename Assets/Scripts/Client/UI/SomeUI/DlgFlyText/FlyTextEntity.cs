using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Utility;
using UILib.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：FlyTextEntity 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.23
// 模块描述：浮动文字实体类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 浮动文字实体类
/// </summary>
internal class FlyTextEntity
{
    private Vector3 m_posStart;
    private float m_fTimeStart;
    private IXUIListItem m_uiListItem;
    private IXUILabel m_uiLabel;
    private long m_unTargetBeastId;
    private bool m_bActive;
    private IXUISprite m_uiSprite;
    private float m_posZ;
    public IXUIListItem FlyTextItem
    {
        get
        {
            return this.m_uiListItem;
        }
    }
    public Beast Target
    {
        get
        {
            return Singleton<BeastManager>.singleton.GetBeastById(this.m_unTargetBeastId);
        }
        set
        {
            if (null != value)
            {
                this.m_unTargetBeastId = value.Id;
            }
        }
    }
    public bool Active
    {
        get
        {
            return this.m_bActive;
        }
        set
        {
            this.m_bActive = value;
            this.m_uiListItem.SetVisible(value);
        }
    }
    public Vector3 PosStart
    {
        get
        {
            return this.m_posStart;
        }
        set
        {
            this.m_posStart = value;
        }
    }
    public float TimeStart
    {
        get
        {
            return this.m_fTimeStart;
        }
        set
        {
            this.m_fTimeStart = value;
        }
    }
    public Transform Transform
    {
        get
        {
            return this.m_uiListItem.CachedTransform;
        }
    }
    public IXUILabel Label
    {
        get
        {
            IXUILabel result;
            if (null != this.m_uiLabel)
            {
                result = this.m_uiLabel;
            }
            else
            {
                result = WidgetFactory.CreateWidget<IXUILabel>();
            }
            return result;
        }
    }
    public IXUISprite Shadow
    {
        get
        {
            IXUISprite result;
            if (null != this.m_uiSprite)
            {
                result = this.m_uiSprite;
            }
            else
            {
                result = WidgetFactory.CreateWidget<IXUISprite>();
            }
            return result;
        }
    }
    public float PosZ
    {
        get
        {
            return this.m_posZ;
        }
        set
        {
            this.m_posZ = value;
        }
    }
    public FlyTextEntity(IXUIListItem flyTextItem, long unTargetHeroId)
    {
        this.m_bActive = true;
        this.m_unTargetBeastId = unTargetHeroId;
        this.m_uiListItem = flyTextItem;
        this.m_uiLabel = (this.m_uiListItem.GetUIObject("Label_FlyText") as IXUILabel);
        this.m_uiSprite = (this.m_uiListItem.GetUIObject("Sprite_Shadow") as IXUISprite);
        if (null == this.m_uiLabel)
        {
            this.m_uiLabel = WidgetFactory.CreateWidget<IXUILabel>();
            XLog.Log.Debug("null == m_uiLabel");
        }
        if (null == this.m_uiSprite)
        {
            this.m_uiSprite = WidgetFactory.CreateWidget<IXUISprite>();
        }
    }
}