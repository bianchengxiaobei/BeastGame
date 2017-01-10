using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CptcCReq_Login
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CptcCReq_Login : CProtocol
    {
        #region 字段和属性
        private const int m_dwPtcC2RReq_LoginID = 1002;
        private static CptcCReq_Login m_instance = new CptcCReq_Login();
        public int m_nServerId;//服务器id
        public string m_strAccountName;//用户名
        public string m_strPassword;//密码
        /*public string m_strToken;//令牌
        public int m_Version;//版本号
        public byte m_AuthType;//用户类型
        public byte m_Platform;//平台类型
        public string m_strClientConfig;//用户配置信息
        public string m_strMacAddr;//用户mac地址
        public string m_strPartner;
         * */
        #endregion
        #region 构造方法
        public CptcCReq_Login()
            : base(1002)
        {
            this.m_strAccountName = "";
            this.m_strPassword = "";
            this.m_nServerId = 0;
        }
        #endregion
        #region 公有方法
        public static CptcCReq_Login GetSendInstance()
        {
            CptcCReq_Login.m_instance.Reset();
            return CptcCReq_Login.m_instance;
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            bs.Write(this.m_strAccountName);
            bs.Write(this.m_strPassword);
            bs.Write(this.m_nServerId);
            return bs;
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            bs.Read(ref this.m_strAccountName);
            bs.Read(ref this.m_strPassword);
            bs.Read(ref this.m_nServerId);
            return bs;
        }
        public override void Process()
        {
        }
        public void Reset()
        {
            this.m_strAccountName = "";
            this.m_strPassword = "";
            this.m_nServerId = 0;
        }
        #endregion
        #region 私有方法

        #endregion
    }
}
