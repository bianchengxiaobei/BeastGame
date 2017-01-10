using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：TipParam
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI
{
    public class TipParam
    {
        public EnumTipType TipType
        {
            get;
            set;
        }
        public string Tip
        {
            get;
            set;
        }
        public TipParam()
        {
            this.TipType = EnumTipType.eTipType_Common;
        }
    }
    public class TitleTipParam : TipParam
    {
        public string Title
        {
            get;
            set;
        }
        public TitleTipParam()
        {
            base.TipType = EnumTipType.eTipType_Title;
        }
    }
    public enum EnumTipType
    {
        eTipType_Common,
        eTipType_Title
    }
}
