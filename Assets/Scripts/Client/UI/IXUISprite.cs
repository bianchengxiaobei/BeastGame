using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUISprite
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUISprite : IXUIObject
    {
        /// <summary>
        /// 颜色
        /// </summary>
        Color Color
        {
            get;
            set;
        }
        /// <summary>
        /// 精灵名字
        /// </summary>
        string SpriteName
        {
            get;
        }
        /// <summary>
        /// 精灵所在的图集
        /// </summary>
        IXUIAtlas UIAtlas
        {
            get;
        }
        /// <summary>
        /// 播放flash动画
        /// </summary>
        /// <param name="bLoop">是否循环</param>
        /// <returns></returns>
        bool PlayFlash(bool bLoop);
        /// <summary>
        /// 设置是否掩藏
        /// </summary>
        /// <param name="bEnable"></param>
        void SetEnable(bool bEnable);
        /// <summary>
        /// 设置精灵的名字
        /// </summary>
        /// <param name="strSpriteName"></param>
        /// <returns></returns>
        bool SetSprite(string strSpriteName);
        /// <summary>
        /// 设置精灵的名字和所在的图集
        /// </summary>
        /// <param name="strSpriteName"></param>
        /// <param name="strAtlas"></param>
        /// <returns></returns>
        bool SetSprite(string strSpriteName, string strAtlas);
        /// <summary>
        /// 停止播放flash动画
        /// </summary>
        /// <returns></returns>
        bool StopFlash();
    }
}
