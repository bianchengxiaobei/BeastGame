using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CTaskListInfo
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：所有任务列表数据
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CTaskListInfo : IData
    {
        public List<CTaskData> m_MainList;
        public CTaskListInfo()
        {
            this.m_MainList = new List<CTaskData>();
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            return bs;
        }
    }
    
}