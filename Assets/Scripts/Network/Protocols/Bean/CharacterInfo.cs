using UnityEngine;
using System.Collections;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CharacterInfo 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.20
// 模块描述：角色信息实体类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 角色信息实体类
    /// </summary>
    public class CharacterInfo : IData
    {
        #region 字段
        private long m_nplayerId;//角色id
        private string name;//角色昵称
        private int level;//角色等级
        private byte sex;//角色性别
        private string icon;//角色头像
        private int logintime;//登陆时间
        //这个是新增加的字段，服务器那边还没有修改过来
        private int playerIndex;//角色索引id，可以根据配置表来查是哪个角色
        private int mapId;//所在的地图索引
        #endregion
        #region 属性
        public long PlayerId
        {
            get { return m_nplayerId; }
            set { m_nplayerId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public byte Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        public int Logintime
        {
            get { return logintime; }
            set { logintime = value; }
        }
        public int PlayerIndex 
        {
            get { return this.playerIndex; }
            set { this.playerIndex = value; }
        }
        public int MapId 
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公共方法
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_nplayerId);
            bs.Write(this.name);
            bs.Write(this.level);
            bs.Write(this.sex);
            bs.Write(this.icon);
            bs.Write(this.logintime);
            bs.Write(this.playerIndex);
            bs.Write(this.mapId);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_nplayerId);
            bs.Read(ref this.name);
            bs.Read(ref this.level);
            bs.Read(ref this.sex);
            bs.Read(ref this.icon);
            bs.Read(ref this.logintime);
            bs.Read(ref this.playerIndex);
            bs.Read(ref this.mapId);
            return bs;
        }
        #endregion
        #region 私有方法
        #endregion
        #region 析构方法
        #endregion
    }
}