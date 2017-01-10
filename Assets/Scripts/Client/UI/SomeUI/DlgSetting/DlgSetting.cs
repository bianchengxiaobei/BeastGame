using UnityEngine;
using System.Collections;
using Utility;
using Utility.Export;
using Client.UI.UICommon;
using Client.Common;
using Client;
using GameClient;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgSetting 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.28
// 模块描述：游戏设置界面
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏设置界面
/// </summary>
public class DlgSetting : DlgBase<DlgSetting,DlgSettingBehaviour>
{
    #region 字段
    private short m_cacheResolutionIndexWhenFullSceen = 0;
    #endregion
    #region 属性
    public override string fileName
    {
        get
        {
            return "DlgSetting";
        }
    }
    public override int layer
    {
        get
        {
            return -5;
        }
    }
    public override uint Type
    {
        get
        {
            return 65536u;
        }
    }
    public override EnumDlgCamera ShowType
    {
        get
        {
            return EnumDlgCamera.Top;
        }
    }
    #endregion
    #region 重写方法
    public override void Init()
    {
        if (base.uiBehaviour.m_Poplist_RS != null)
        {
            this.uiBehaviour.m_Poplist_RS.Clear();
            Debug.Log(GraphicsManager.GoodGraphicsResolution.Count);
            for (int i = 0; i < GraphicsManager.GoodGraphicsResolution.Count; i++)
            {
                IGraphicsResolution resolution = GraphicsManager.GoodGraphicsResolution[i];
                string item = string.Format("{0}X{1}", resolution.Width, resolution.Height);
                this.uiBehaviour.m_Poplist_RS.AddItem(item, resolution);
                if (resolution.Width == Singleton<ScreenManager>.singleton.CurrentWidth && resolution.Height == Singleton<ScreenManager>.singleton.CurrentHeight)
                {
                    this.uiBehaviour.m_Poplist_RS.SelectedIndex = (short)i;
                }
            }
        }
    }
    public override void RegisterEvent()
    {
        base.uiBehaviour.m_Poplist_RS.RegisterPopupListSelectEventHandler(new PopupListSelectEventHanler(this.OnPopupListResolutionSelect));
    }
    #endregion
    #region 公共方法
    #endregion
    #region 私有方法
    private bool OnPopupListResolutionSelect(IXUIPopupList uiPopupList)
    {
        XLog.Log.Debug("OnPoP");
        bool result;
        if (1 == UserOptions.Singleton.DisplayMode)
        {
            IGraphicsResolution graphicsResolution = (IGraphicsResolution)uiPopupList.GetDataByIndex((int)uiPopupList.SelectedIndex);
            if (null == graphicsResolution)
            {
                result = false;
                return result;
            }
            UserOptions.Singleton.Resolution = graphicsResolution;
            XLog.Log.Debug("Set");
            Singleton<ScreenManager>.singleton.SetResolution(graphicsResolution.Width, graphicsResolution.Height, (EnumDisplayMode)UserOptions.Singleton.DisplayMode);
        }
        else
        {
            uiPopupList.SelectedIndex = this.m_cacheResolutionIndexWhenFullSceen;
        }
        result = true;
        return result;
    }
    #endregion
    #region 析构方法
    #endregion
}
