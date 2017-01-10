using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IState
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：状态接口
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public interface IState
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        void Enter(EntityParent entity, params object[] args);
        /// <summary>
        /// 离开状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        void Exit(EntityParent entity, params object[] args);
        /// <summary>
        /// 处理状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        void Process(EntityParent entity, params object[] args);
    }
}
