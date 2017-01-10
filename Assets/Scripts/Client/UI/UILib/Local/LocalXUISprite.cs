using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalXUISprite
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUISprite : LocalXUIObject,IXUISprite,IXUIObject
    {
        private static LocalXUISprite m_instance = new LocalXUISprite();
        private Color m_color;
        private string m_spriteName;
        private IXUIAtlas m_atlas;

        public Color Color 
        {
            get { return this.m_color; }
            set { this.m_color = value; }
        }
        public string SpriteName 
        {
            get { return this.m_spriteName; }
            set { this.m_spriteName = value; }
        }
        public IXUIAtlas UIAtlas
        {
            get { return this.m_atlas; }
            set { this.m_atlas = value; }
        }
        public static LocalXUISprite GetInstance()
        {
            return LocalXUISprite.m_instance;
        }
        public bool PlayFlash(bool A)
        {
            return false;
        }
        public void SetEnable(bool A)
        {
        }
        public bool SetSprite(string A)
        {
            return false;
        }
        public bool SetSprite(string A, string a)
        {
            return false;
        }
        public bool StopFlash()
        {
            return false;
        }
    }
}
