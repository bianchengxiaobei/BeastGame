using UnityEngine;
using System.Collections;
using Utility;
using Utility.Export;
using Game;
using Client.GameMain.OpState;
using Client.UI.UICommon;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：InputManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：战棋操作管理器
//----------------------------------------------------------------*/
#endregion
public class InputManager : Singleton<InputManager>
{
    #region 字段
    private IXLog m_log = XLog.GetLog<InputManager>();
    private int m_layerMask = 0;
    private int m_layerMaskForPlayer = 1 << UnityEngine.LayerMask.NameToLayer("Highlight") | 1 << UnityEngine.LayerMask.NameToLayer("Player");
    private int m_layerMaskForMap = 1 << UnityEngine.LayerMask.NameToLayer("HexagonMap");
    private int m_layerMaskForMapNodeBuilding = 1 << UnityEngine.LayerMask.NameToLayer("MapNodeBuilding");
    #endregion
    #region 属性
    #endregion
    #region 构造方法
    public InputManager()
    {
        this.m_layerMask = this.m_layerMaskForMapNodeBuilding;
        this.EnableBeastSelect(true);
        this.EnableMapSelect(true);
    }
    #endregion
    #region 公有方法
    public bool OnMapClick(CVector3 vecHexPos)
    {
        Beast beast = Singleton<BeastManager>.singleton.GetBeastByPos(vecHexPos);
        //DlgBase<DlgMain, DlgMainBehaviour>.singleton.OnClickStore(vec3HexPos);
        bool flag = Singleton<OpStateManager>.singleton.OnSelectPos(vecHexPos);
        if (!flag)
        {
            if (beast != null)
            {
                //DlgBase<DlgHeroTips, DlgHeroTipsBehaviour>.singleton.OnClickHero(heroByMovingPos.Id);
            }
            //DlgBase<DlgMain, DlgMainBehaviour>.singleton.SendMapClick(vec3HexPos);
        }
        return flag;
    }
    //UIManager.singleton.CurrentTouchID = -1 ==> 鼠标左击
    //UIManager.singleton.CurrentTouchID = -2 ==> 鼠标右击
    public void OnClick()
    {
        this.m_log.Debug("Onclick");
        UICamera.MouseOrTouch currentTouch = UIManager.singleton.CurrentTouch;
        if (null == currentTouch)
        {
            this.m_log.Error("null == mouseOrTouch");
        }
        else
        {
            bool isEventProcessed = UIManager.singleton.IsEventProcessed;
            if (Camera.main != null)
            {
                if (UIManager.singleton.CurrentTouchID == -2 && !isEventProcessed)
                {

                    
                }
                if (UIManager.singleton.CurrentTouchID != -2 && !isEventProcessed)
                {
                    isEventProcessed = Singleton<OpStateManager>.singleton.OnClick();
                    if (isEventProcessed)
                    {
                        UIManager.singleton.IsEventProcessed = true;
                    }
                }
                if (!UIManager.singleton.IsAnyTouchInUI && UIManager.singleton.CurrentTouchID != -2 && !isEventProcessed)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(currentTouch.pos.x, currentTouch.pos.y, 0f));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, float.PositiveInfinity, this.m_layerMask))
                    {
                        this.m_log.Debug("Raycast:hit=:" + hit.transform.name);
                        int num = 1 << hit.transform.gameObject.layer;
                        if ((num & this.m_layerMaskForMap) > 0)
                        {
                            hit.transform.gameObject.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                        }
                        else
                        {
                            hit.transform.gameObject.SendMessage("OnClick");
                        }
                    }
                }
            }
        }
    }
    #endregion
    #region 私有方法
    private void EnableMapSelect(bool bEnable)
    {
        if (bEnable)
        {
            this.m_layerMask |= this.m_layerMaskForMap;
        }
        else
        {
            this.m_layerMask &= ~this.m_layerMaskForMap;
        }
    }
    private void EnableBeastSelect(bool bEnable)
    {
        if (bEnable)
        {
            this.m_layerMask |= this.m_layerMaskForPlayer;
        }
        else
        {
            this.m_layerMask &= ~this.m_layerMaskForPlayer;
        }
    }
    #endregion
}
