using UnityEngine;
using System.Collections;
using Client.Common;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCNtf_CDBegin
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：服务器发送客户端更新技能cd和装备cd
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    /// <summary>
    /// 服务器发送客户端更新技能cd和装备cd
    /// </summary>
    public class CPtcCNtf_CDBegin : CProtocol
    {
        #region 字段
        private const int m_dwPtcG2CNtf_CDBeginID = 1019;
        public long m_dwRoleID;//神兽id
        public int m_dwID;//技能id
        public byte m_btValue;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        public CPtcCNtf_CDBegin()
            : base(1019)
        {
            this.m_dwRoleID = 0;
            this.m_dwID = 0;
            this.m_btValue = 0;
        }
        #endregion
        #region 公有方法
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_dwRoleID);
            bs.Write(this.m_dwID);
            bs.Write(this.m_btValue);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_dwRoleID);
            bs.Read(ref this.m_dwID);
            bs.Read(ref this.m_btValue);
            return bs;
        }
        public override void Process()
        {
            int skillId = this.m_dwID;
            int type = this.m_dwID >> 16;//10000末尾4位随便填有（skill）,100000末尾4位随便填有（equip）
            ECoolDownType eCoolDownType = (ECoolDownType)type;
            XLog.Log.Debug(string.Concat(new object[]
			{
				"Rec CPtcG2CNtf_CDBegin",
				eCoolDownType.ToString(),
				"id:",
				skillId, 
				" CD:",
				this.m_btValue.ToString()
			}));
            Beast beast = Singleton<BeastManager>.singleton.GetBeastById(this.m_dwRoleID);
            byte preCDTime = 0;
            switch (eCoolDownType) 
            {
                case ECoolDownType.COOL_DOWN_SKILL:
                    if (beast != null)
                    {
                        preCDTime = beast.GetSkillById(skillId).CDTime;
                    }
                    Singleton<BeastManager>.singleton.OnBeastSkillCDChange(this.m_dwRoleID, skillId, this.m_btValue);
                    break;
                case ECoolDownType.COOL_DOWN_EQUIP:
                    if (beast != null)
                    {
                        //暂时不做
                    }
                    break;
            }
        }
        #endregion
        #region 私有方法
        #endregion
    }
}