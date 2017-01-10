using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIListItem
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIListItem : IXUIObject
    {
        #region 属性
        /// <summary>
        /// 列表id
        /// </summary>
        long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 游戏中唯一的id
        /// </summary>
        ulong GUID
        {
            get;
            set;
        }
        /// <summary>
        /// 列表索引
        /// </summary>
        int Index
        {
            get;
        }
        /// <summary>
        /// 是否被选择中
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }
        /// <summary>
        /// 高亮的颜色
        /// </summary>
        Color HighlightColor
        {
            get;
            set;
        }
        #endregion
        /// <summary>
        /// 设置是否可见
        /// </summary>
        /// <param name="strId"></param>
        /// <param name="bVisible"></param>
        void SetVisible(string strId, bool bVisible);
        /// <summary>
        /// 设置UI精灵
        /// </summary>
        /// <param name="strSprite"></param>
        void SetIconSprite(string strSprite);
        void SetSprite(string strId, string strSprite);
        /// <summary>
        /// 设置UI精灵，从指定的UIAtlas中
        /// </summary>
        /// <param name="strSprite"></param>
        /// <param name="strAtlas"></param>
        void SetIconSprite(string strSprite, string strAtlas);
        /// <summary>
        /// 设置UI图片
        /// </summary>
        /// <param name="strTexture"></param>
        void SetIconTexture(string strTexture);
        /// <summary>
        /// 设置UI颜色
        /// </summary>
        /// <param name="color"></param>
        void SetColor(Color color);
        /// <summary>
        /// 设置UI标签
        /// </summary>
        /// <param name="strId"></param>
        /// <param name="strText"></param>
        /// <returns></returns>
        bool SetText(string strId, string strText);
        void SetEnable(bool bEnable);
        void SetEnableSelect(bool bEnable);
        void SetSize(float cellWidth, float cellHeight);
        void Clear();
        Dictionary<string, XUIObjectBase> GetAllXUIObj();
    }
}
