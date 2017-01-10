using UnityEngine;
using System.Collections.Generic;
using System;
using Client.UI;
using Client.UI.UICommon;
using Client.Common;
using Utility.Export;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgFlyText
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.23
// 模块描述：浮动文字界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 浮动文字界面
/// </summary>
public class DlgFlyText : DlgBase<DlgFlyText,DlgFlyTextBehaviour>
{
	#region 字段 
    private float m_posDepthZ = 0f;
    private Dictionary<enumFlyTextType, IFlyTextManager> m_dicFlyTextManagers = new Dictionary<enumFlyTextType, IFlyTextManager>();
    private IXLog m_log = XLog.GetLog<DlgFlyText>();
    #endregion
	#region 属性
    public override string fileName
    {
        get
        {
            return "DlgFlyText";
        }
    }
    public override int layer
    {
        get
        {
            return 3;
        }
    }
    public override uint Type
    {
        get
        {
            return 65536u;
        }
    }
    public override bool IsPersist
    {
        get
        {
            return true;
        }
    }
    public float PosZ
    {
        get 
        {
            if (Singleton<ClientMain>.singleton.EGameState == EnumGameState.eState_GameMain)
            {
                this.m_posDepthZ -= 1f;
            }
            else 
            {
                this.m_posDepthZ = 0f;
            }
            return this.m_posDepthZ;
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公共方法
    public override void Init()
    {
        this.m_dicFlyTextManagers.Add(enumFlyTextType.eFlyTextType_Hp, new HpFlyTextManager(base.uiBehaviour.List_Demage));
    }
    public override void Update()
    {
        foreach (var current in this.m_dicFlyTextManagers)
        {
            try
            {
                current.Value.Update();
            }
            catch (Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
    }
    /// <summary>
    /// 添加扣血漂浮界面
    /// </summary>
    /// <param name="nHpChange"></param>
    /// <param name="unTargetHeroId"></param>
    /// <param name="eHpEffectType"></param>
    public void AddHpEffect(int nHpChange,long unTargetBeastId,EnumHpEffectType eHpEffectType)
    {
        if (base.Prepared)
        {
            if (nHpChange != 0)
            {
                if (this.m_dicFlyTextManagers.ContainsKey(enumFlyTextType.eFlyTextType_Hp))
                {
                    string hpChange = nHpChange.ToString();
                    if (nHpChange > 0)
                    {
                        hpChange = string.Format("+{0}", nHpChange);
                    }
                    FlyTextEntity flyTextEntity = this.m_dicFlyTextManagers[enumFlyTextType.eFlyTextType_Hp].Add(hpChange,unTargetBeastId,this.PosZ);
                    if (flyTextEntity != null)
                    {
                        string animation = "Demage2";
                        switch (eHpEffectType)
                        {
                            case EnumHpEffectType.eHpEffectType_Damage:
                                animation = "Demage2";
                                flyTextEntity.FlyTextItem.SetVisible("Label_FlyTextGreen", false);//治疗效果的血量
                                flyTextEntity.FlyTextItem.SetVisible("Label_FlyTextRed_Offset", true);
                                flyTextEntity.FlyTextItem.SetVisible("Label_Hit_Offset", false);
                                flyTextEntity.FlyTextItem.SetVisible("Label_Crit_Offset", false);
                                flyTextEntity.FlyTextItem.SetText("Label_FlyTextRed", hpChange);
                                break;
                            case EnumHpEffectType.eHpEffectType_Crit:
                                animation = "Demage_Crit2";
                                flyTextEntity.FlyTextItem.SetVisible("Label_FlyTextGreen", false);
                                flyTextEntity.FlyTextItem.SetVisible("Label_FlyTextRed_Offset", false);
                                flyTextEntity.FlyTextItem.SetVisible("Label_Hit_Offset", false);
                                flyTextEntity.FlyTextItem.SetVisible("Label_Crit_Offset", true);
                                flyTextEntity.FlyTextItem.SetText("Label_Crit", hpChange);
                                break;
                                case EnumHpEffectType.eHpEffectType_Heal:
								animation = "Demage";
								flyTextEntity.FlyTextItem.SetVisible("Label_FlyTextGreen", true);
								flyTextEntity.FlyTextItem.SetVisible("Label_FlyTextRed_Offset", false);
								flyTextEntity.FlyTextItem.SetVisible("Label_Hit_Offset", false);
								flyTextEntity.FlyTextItem.SetVisible("Label_Crit_Offset", false);
								flyTextEntity.FlyTextItem.SetText("Label_FlyTextGreen", hpChange);
								break;
							}
                            Animation animation2 = flyTextEntity.FlyTextItem.CachedGameObject.animation;
                            if (animation2 != null)
                            {
                                animation2.Stop();
                                animation2.Play(animation,PlayMode.StopAll);
                            }
                        }
                    }
                }
            }
        }
    public void AddFlyText(string strText, long unTargetBeastId, enumFlyTextType eFlyTextType)
    {
        if (base.Prepared)
        {
            if (this.m_dicFlyTextManagers.ContainsKey(eFlyTextType))
            {
                this.m_dicFlyTextManagers[eFlyTextType].Add(strText, unTargetBeastId, this.PosZ);
            }
        }
    }
    /// <summary>
    /// 弹出系统消息
    /// </summary>
    /// <param name="strText"></param>
    public void AddSystemInfo(string strText)
    {
        
    }
    public void ResetPosZ()
    {
        this.m_posDepthZ = 0f;
    }
    public void ShowBlood(EBloodLevel lv)
    {
        if (base.Prepared)
        {
            IXUISprite iXUISprite;
            switch (lv)
            {
                case EBloodLevel.Low:
                    iXUISprite = base.uiBehaviour.m_Sprite_Red_Low;
                    break;
                case EBloodLevel.Middle:
                    iXUISprite = base.uiBehaviour.m_Sprite_Red_Middle;
                    break;
                case EBloodLevel.High:
                    iXUISprite = base.uiBehaviour.m_Sprite_Red_High;                    
                    break;
                default:
                    return;
            }
            if (iXUISprite != null)
            {
                Animation anim = iXUISprite.CachedTransform.animation;
                if (anim != null)
                {
                    anim.Stop();
                    anim.Play();
                }
            }
        }
    }
    public void ShowBlood()
    {
        this.ShowBlood(EBloodLevel.Low);
    }
    public void EndBlood()
    {
 
    }
    public void StartScreenBlur()
    {
        if (base.Prepared)
        {
            base.uiBehaviour.m_Sprite_Blur.SetVisible(true);
            int layer = LayerMask.NameToLayer("UIHigh");
            UIManager.singleton.SetLayer(base.uiBehaviour.List_Demage.CachedGameObject, layer);
        }
    }
    public void UpdateScreenBlur(float alpha)
    {
        if (base.Prepared)
        {
            Color color = base.uiBehaviour.m_Sprite_Blur.Color;
            color.a = alpha;
            base.uiBehaviour.m_Sprite_Blur.Color = color;
        }
    }
    public void EndScreenBlur()
    {
        if (base.Prepared)
        {
            base.uiBehaviour.m_Sprite_Blur.SetVisible(true);
            int layer = LayerMask.NameToLayer("UI");
            UIManager.singleton.SetLayer(base.uiBehaviour.List_Demage.CachedGameObject, layer);
        }
    }
	#endregion
	#region 私有方法
    protected override void OnUnLoad()
    {
        base.OnUnLoad();
        this.m_dicFlyTextManagers.Clear();
        this.ResetPosZ();
    }
	#endregion
	#region 析构方法
	#endregion
}
