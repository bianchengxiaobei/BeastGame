using UnityEngine;
using Client.UI.UICommon;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：LocalXUIGroup
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace UILib.Local
{
    public class LocalXUIGroup : LocalXUIObject,IXUIGroup,IXUIObject
    {
        private static LocalXUIGroup m_instance = new LocalXUIGroup();
        public static LocalXUIGroup GetInstance()
        {
            return LocalXUIGroup.m_instance;
        }
    }
}
