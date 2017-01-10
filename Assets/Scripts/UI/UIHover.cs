using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UIHover
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class UIHover : MonoBehaviour
{
    public GameObject m_objTarget;
    private void Start()
    {
        this.UpdateImage();
    }
    private void Update()
    {
    }
    private void UpdateImage()
    {
        this.OnHover(UICamera.IsHighlighted(base.gameObject));
    }
    private void OnHover(bool isOver)
    {
        if (null != this.m_objTarget && base.enabled)
        {
            NGUITools.SetActiveSelf(this.m_objTarget, isOver);
        }
    }
    private void OnDisable()
    {
        if (null != this.m_objTarget && base.enabled)
        {
            NGUITools.SetActiveSelf(this.m_objTarget, false);
        }
    }
}