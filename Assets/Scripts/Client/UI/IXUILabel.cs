using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUILabel
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUILabel : IXUIObject
    {
        /// <summary>
        /// 透明度
        /// </summary>
        float AlphaVar
        {
            get;
            set;
        }
        /// <summary>
        /// 颜色
        /// </summary>
        Color Color
        {
            get;
            set;
        }
        /// <summary>
        /// 最大长度
        /// </summary>
        int MaxWidth
        {
            get;
            set;
        }
        /// <summary>
        /// 取得字符内容
        /// </summary>
        /// <returns></returns>
        string GetText();
        /// <summary>
        /// 设置字符内容
        /// </summary>
        /// <param name="strText"></param>
        void SetText(string strText);
    }
}

