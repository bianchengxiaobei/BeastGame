using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：RankData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Client.Data
{
    public class RankData : GameData<RankData>
    {
        public int type { get; protected set; }
        public int priority { get; protected set; }
        public int name { get; protected set; }
        public List<int> tip { get; protected set; }
        public int ifReward { get; protected set; }
        public int rewardUI { get; protected set; }
        public Dictionary<int, int> title { get; protected set; }
        public int rankCount { get; protected set; }

        static public readonly string fileName = "RankList";
        //static public Dictionary<int, RankData> dataMap { get; set; }

        /// <summary>
        /// 获取通过priority排序的ID列表
        /// </summary>
        /// <returns></returns>
        static public List<int> GetSortRankIDList()
        {
            List<int> sortRankIDList = new List<int>();
            foreach (int rankID in dataMap.Keys)
            {
                sortRankIDList.Add(rankID);
            }

            sortRankIDList.Sort(delegate(int p1, int p2)
            {
                if (dataMap[p1].priority > dataMap[p2].priority)
                    return 1;
                else
                    return -1;
            });

            return sortRankIDList;
        }

        /// <summary>
        /// 通过ID获取RankData
        /// </summary>
        /// <returns></returns>
        static public RankData GetRankDataByID(int id)
        {
            if (dataMap.ContainsKey(id))
                return dataMap[id];

            return null;
        }

        /// <summary>
        /// 通过ID随机获取Tip
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public int GetRandomTipByID(int id)
        {
            if (dataMap.ContainsKey(id))
            {
                int index = Random.Range(0, dataMap[id].tip.Count);
                if (index < dataMap[id].tip.Count)
                    return dataMap[id].tip[index];
            }

            return 0;
        }
    }
}