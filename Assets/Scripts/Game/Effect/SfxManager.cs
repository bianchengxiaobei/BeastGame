using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SfxManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.19
// 模块描述：角色特效管理器
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class SfxManager
    {
        #region 字段
        private EntityParent theOwner;
        #endregion
        #region 属性
        public Dictionary<int, List<uint>> sfxTimerIDDic { get; protected set; }
        #endregion
        #region 构造方法
        public SfxManager(EntityParent owner)
        {
            this.theOwner = owner;
            sfxTimerIDDic = new Dictionary<int, List<uint>>();
        }
        #endregion
        #region 公有方法
        public void RemoveSfx(int actionID)
        {
            var sfxs = sfxTimerIDDic.GetValueOrDefault(actionID,new List<uint>());
            foreach (var item in sfxs)
            {
                FrameTimerHeap.DelTimer(item);
            }
            Dictionary<int, float> sfx = SkillActionData.dataMap[actionID].sfx;
            if (null == sfx)
            {
                return;
            }
            SfxHandler handler = theOwner.sfxHandler;
            foreach (var item in sfx)
            {
                if (item.Key < 1000)
                {
                    StopUIFx(item.Key);
                }
                else
                {
                    handler.RemoveFx(item.Key);
                }
            }
        }
        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="actionId"></param>
        public void PlaySfx(int actionId)
        {
            if (!SkillAction.dataMap.ContainsKey(actionId))
            {
                return;
            }
            Dictionary<int, float> sfx = SkillAction.dataMap[actionId].sfx;
            SfxHandler sfxHandler = theOwner.sfxHandler;
            if (sfx != null && sfx.Count > 0)
            {
                if (!sfxTimerIDDic.ContainsKey(actionId))
                {
                    sfxTimerIDDic.Add(actionId, new List<uint>());
                }
                foreach (var pair in sfx)
                {
                    ///如果actionId小于1000的话是UI特效
                    if (pair.Key < 1000)
                    {
                        sfxTimerIDDic[actionId].Add(FrameTimerHeap.AddTimer((uint)(1000 * pair.Value), 0, PlayUIFx, pair.Key));
                    }
                    else
                    {
                        sfxTimerIDDic[actionId].Add(FrameTimerHeap.AddTimer((uint)(1000 * pair.Value), 0, TriggerCue, sfxHandler, pair.Key));
                    }
                }
            }
        }
        /// <summary>
        /// 清除所有特效
        /// </summary>
        public void ClearAllSfx()
        {
            foreach (var sfx in sfxTimerIDDic)
            {
                RemoveSfx(sfx.Key);
            }
            sfxTimerIDDic.Clear();
        }
        /// <summary>
        /// 播放UI特效
        /// </summary>
        /// <param name="actionId"></param>
        public void PlayUIFx(int actionId)
        {

        }
        /// <summary>
        /// 停止播放UI特效
        /// </summary>
        /// <param name="actionId"></param>
        public void StopUIFx(int actionId)
        {

        }
        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="actionId"></param>
        public void TriggerCue(SfxHandler handler, int actionId)
        {
            if (handler)
            {
                handler.HandleFx(actionId);
            }
        }
        #endregion
        #region 私有方法
        #endregion
    }
}
