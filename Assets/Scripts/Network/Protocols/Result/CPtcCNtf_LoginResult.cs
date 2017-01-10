using UnityEngine;
using System.Collections;
using Utility;
using Client.Logic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CPtcCNtf_LoginResult
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.22
// 模块描述：服务器返回客户端登陆结果
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class CPtcCNtf_LoginResult : CProtocol
    {
        private const uint m_dwPtcR2CNtf_LoginResultID = 1000;
        public int m_ErrCode;
        public CPtcCNtf_LoginResult()
            : base(1000)
        {
            this.m_ErrCode = 0;
        }
        public override void Process()
        {
            if (this.m_ErrCode == 0)
            {
                //Singleton<Login>.singleton.OnLoginSuccess(this);//如果没有错误，就登陆成功
            }
            else 
            {
                Singleton<Login>.singleton.OnLoginFailed();//否则登陆失败
                string errString = StringConfigMgr.GetErrString(this.m_ErrCode);
                Debug.Log(errString);
                if (this.m_ErrCode == 7)
                {
 
                }
            }
        }
        public override CByteStream DeSerialize(CByteStream bs)
        {
            //throw new System.NotImplementedException();
            bs.Read(ref this.m_ErrCode);
            return bs;
        }
        public override CByteStream Serialize(CByteStream bs)
        {
            //throw new System.NotImplementedException();
            bs.Write(this.m_ErrCode);
            return bs;
        }
    }
}