using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IXUIDlg
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public interface IXUIDlg
    {
        #region 属性
        IXUIBehaviour uiBehaviourInterface
        {
            get;
        }
        string fileName
        {
            get;
        }
        int layer
        {
            get;
        }
        uint Type
        {
            get;
        }
        bool IsLimitClick
        {
            get;
            set;
        }
        #endregion
        void _Init(IXUIBehaviour iXUIPanel);
        void _Update();
        void _FixedUpdate();
        void Load();
        bool UnLoad();
        void SetVisible(bool bVisible);
        bool IsVisible();
        void Reset();
        void SetDepthZ(int nDepthZ);
        void OnFinishChangeState();
    }
}