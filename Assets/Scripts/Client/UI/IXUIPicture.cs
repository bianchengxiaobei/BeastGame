using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIPicture
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIPicture : IXUIObject
    {
        Color Color
        {
            get;
            set;
        }
        void SetTexture(string strTextureFile);
        void SetTexture(Texture texture);
    }
}