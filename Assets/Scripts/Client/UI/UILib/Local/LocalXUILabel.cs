using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalXUILabel
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUILabel:LocalXUIObject,IXUILabel,IXUIObject
    {
        private static LocalXUILabel m_instance = new LocalXUILabel();
        private float m_fAlphaVar;
        private Color m_color;
        private int m_fMaxWidth;

        public float AlphaVar 
        {
            get { return this.m_fAlphaVar; }
            set { this.m_fAlphaVar = value; }
        }
        public Color Color
        {
            get { return this.m_color; }
            set { this.m_color = value; }
        }
        public int MaxWidth 
        {
            get { return this.m_fMaxWidth; }
            set { this.m_fMaxWidth = value; }
        }
        public static LocalXUILabel GetInstance()
        {
            return LocalXUILabel.m_instance;
        }
        public string GetText()
        {
            return string.Empty;
        }
        public void SetText(string A)
        {
        }
    }
}