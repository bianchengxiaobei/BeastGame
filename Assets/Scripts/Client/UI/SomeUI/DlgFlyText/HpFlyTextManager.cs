using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：HpFlyTextManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.24
// 模块描述：血量浮动文字管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 血量浮动文字管理器
/// </summary>
internal class HpFlyTextManager : FlyTextManagerBase,IFlyTextManager
{
    public HpFlyTextManager(IXUIList uiList)
        : base(uiList)
    {
        this.m_fTotalTime = 3f;
    }
    protected override void InitFlyText(FlyTextEntity flyText, string strText, long targetBeast)
    {
        if (Camera.main != null)
        {
            base.InitFlyText(flyText, strText, targetBeast);
            Beast heroById = Singleton<BeastManager>.singleton.GetBeastById(targetBeast);
            if (null != heroById)
            {
                Vector3 movingPos = heroById.MovingPos;
                movingPos.y += heroById.Height + 1f;
                Vector3 position = Camera.main.WorldToScreenPoint(movingPos);
                Vector3 position2 = UIManager.singleton.UICamera.ScreenToWorldPoint(position);
                flyText.Transform.position = position2;
                Vector3 localPosition = flyText.Transform.localPosition;
                flyText.Transform.localPosition = new Vector3(localPosition.x, localPosition.y, flyText.PosZ);
            }
        }
    }
    protected override void Translate(ref FlyTextEntity flyText, float fElapseTime)
    {
        base.Translate(ref flyText, fElapseTime);
        Vector3 movingPos = flyText.Target.MovingPos;
        movingPos.y += flyText.Target.Height + 1f;
        Vector3 position = Camera.main.WorldToScreenPoint(movingPos);
        Vector3 vector = UIManager.singleton.UICamera.ScreenToWorldPoint(position);
        Vector3 zero = Vector3.zero;
        zero.x = Mathf.Lerp(flyText.Transform.position.x, vector.x, 1f);
        zero.y = Mathf.Lerp(flyText.Transform.position.y, vector.y, 1f);
        flyText.Transform.position = zero;
        Vector3 localPosition = flyText.Transform.localPosition;
        flyText.Transform.localPosition = new Vector3(localPosition.x, localPosition.y, flyText.PosZ);
    }
}
