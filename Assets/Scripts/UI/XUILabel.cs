using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XUILabel
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
[AddComponentMenu("XUI/XUILabel")]
public class XUILabel : XUIObject, IXUIObject, IXUILabel
{
    public float m_fAlphaVar;
    private UILabel m_uiLabel;
    public float AlphaVar
    {
        get
        {
            return this.m_fAlphaVar;
        }
        set { this.m_fAlphaVar = value; }
    }
    public override float Alpha
    {
        get
        {
            if (null != this.m_uiLabel)
            {
                return this.m_uiLabel.alpha;
            }
            return 1f;
        }
        set
        {
            if (null != this.m_uiLabel)
            {
                this.m_uiLabel.alpha = value;
            }
        }
    }
    public Color Color
    {
        get
        {
            if (null != this.m_uiLabel)
            {
                return this.m_uiLabel.color;
            }
            return Color.white;
        }
        set
        {
            if (null != this.m_uiLabel)
            {
                this.m_uiLabel.color = value;
            }
        }
    }
    public int MaxWidth
    {
        get
        {
            if (null != this.m_uiLabel)
            {
                return this.m_uiLabel.width;
            }
            return 0;
        }
        set
        {
            if (null != this.m_uiLabel)
            {
                this.m_uiLabel.width = value;
            }
        }
    }
    public override Vector2 RelativeSize
    {
        get
        {
            if (null != this.m_uiLabel)
            {
                Vector2 relativeSize = this.m_uiLabel.relativeSize;
                relativeSize.x *= base.CachedTransform.localScale.x;
                relativeSize.y *= base.CachedTransform.localScale.y;
                return relativeSize;
            }
            return Vector2.zero;
        }
    }
    public string GetText()
    {
        return this.m_uiLabel.text;
    }
    public void SetText(string strText)
    {
        this.m_uiLabel.text = strText;
    }
    public override void Init()
    {
        base.Init();
        this.m_uiLabel = base.GetComponent<UILabel>();
        if (null == this.m_uiLabel)
        {
            Debug.LogError("null == m_uiLabel");
        }
    }
}
