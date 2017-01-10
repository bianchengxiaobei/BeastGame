using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUIAltas
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.2
// 模块描述：UI图集
//----------------------------------------------------------------*/
#endregion
[AddComponentMenu("XUI/XUIAtlas")]
public class XUIAtlas : MonoBehaviour, IXUIAtlas
{
    private UIAtlas m_uiAltas;
    public UIAtlas Altas 
    {
        get { return this.m_uiAltas; }
        set { this.m_uiAltas = value; }
    }
    private void Awake()
    {
        this.m_uiAltas = base.GetComponent<UIAtlas>();
        if (null == this.m_uiAltas)
        {
            Debug.LogError("null == m_uiAltas");
        }
    }
}
