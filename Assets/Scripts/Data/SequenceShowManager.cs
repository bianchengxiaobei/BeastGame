using UnityEngine;
using System.Collections.Generic;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SequenceShowManager
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class SequenceShowManager : Singleton<SequenceShowManager>
    {
        public bool UseSeqShow
        {
            get;
            set;
        }
        public float PreStageSeg
        {
            get;
            set;
        }
        public float ShowStageSeg
        {
            get;
            set;
        }
        public float StatusSeg
        {
            get;
            set;
        }
    }
}
