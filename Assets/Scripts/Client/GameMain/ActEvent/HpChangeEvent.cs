using UnityEngine;
using System.Collections;
using Utility;
using Client.UI.UICommon;
using Client.Common;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：HpChangeEvent 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.21
// 模块描述：血量改变事件
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 血量改变事件
/// </summary>
namespace Client.GameMain
{
    public class HpChangeEvent : ActEvent
    {
        protected long m_unBeastId = 0;
        public long BeastId 
        {
            get { return this.m_unBeastId; }
            set { this.m_unBeastId = value; }
        }
        public int HpChange
        {
            get;
            set;
        }
        public override void Trigger()
        {
            base.Trigger();
            XLog.Log.Debug("HpChangeEvent:Trigger");
            if (this.HpChange > 0)
            {
                //浮动加血文字显示
                DlgBase<DlgFlyText, DlgFlyTextBehaviour>.singleton.AddHpEffect(this.HpChange, this.m_unBeastId, EnumHpEffectType.eHpEffectType_Heal);
            }
            else 
            {
                DlgBase<DlgFlyText, DlgFlyTextBehaviour>.singleton.AddHpEffect(this.HpChange, this.m_unBeastId, EnumHpEffectType.eHpEffectType_Damage);
                //浮动扣血文字显示
                int hpChange = this.HpChange;
                Beast beast = Singleton<BeastManager>.singleton.GetBeastById(this.BeastId);
                if (beast != null && !beast.IsError)
                {
                    if (hpChange < 0)
                    {
                        //可能播放扣血的特效
                    }
                }
            }
        }
    }
}