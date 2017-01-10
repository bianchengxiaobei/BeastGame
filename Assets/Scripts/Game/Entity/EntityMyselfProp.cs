using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EntityMyselfProp 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.14
// 模块描述：玩家自身角色属性
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 玩家自身角色属性
    /// </summary>
    public partial class EntityMyself
    {
        #region 字段
        private int m_sceneId = 0;
        private byte m_deathFlag = 0;
        #endregion
        #region 属性
        public int SceneId
        {
            get
            {
                return this.m_sceneId;
            }
            set
            {
                if (m_sceneId == value)
                {
                    return;
                }
                int lastId = m_sceneId;
                m_sceneId = value;
                if (lastId != m_sceneId)
                {
                    //关闭UI这些界面
                }
                RealSwitchScene(lastId, m_sceneId);
            }
        }
        /// <summary>
        /// 死亡标记,如果大于0的话，就让实体改变到death状态
        /// </summary>
        public byte DeathFlag
        {
            get { return this.m_deathFlag; }
            set
            {
                this.m_deathFlag = value;
                if (m_deathFlag > 0)
                {
                    //如果SceneId不等于主城，也就是在战场中的SceneId，就发送给服务器取得复活的时间
                    this.OnDeath(-1);
                }
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公共方法
        #endregion
        #region 私有方法
        private void RealSwitchScene(int oldId, int newId)
        {
            GameWorld.SwitchScene(oldId, newId);
        }
        #endregion
        #region 析构方法
        #endregion
    }
}