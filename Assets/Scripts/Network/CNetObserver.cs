using UnityEngine;
using System.Collections;
using Game;
using Utility;
using Client.Logic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CNetObserver
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class CNetObserver : INetObserver
{
    public CNetProcessor oProc = null;
    public void OnConnect(bool bSuccess)
    {
        if (!bSuccess)
        {
            if (!Singleton<Login>.singleton.IsLogined)
            {
                Singleton<Login>.singleton.OnConnectFailed();
            }
            else
            {

            }
        }
        else 
        {
            XLog.Log.Debug("Connect Success!");
            if (!Singleton<Login>.singleton.IsLogined)
            {
                Singleton<Login>.singleton.OnConnectSuccess();
            }
            else 
            {
                
            }
        }
    }
    public void OnClosed(NetErrCode nErrCode)
    {
      
    }
    public void OnReceive(int unType, int nLen)
    {
      //  Singleton<PerformanceAnalyzer>.singleton.OnRecevie((uint)nLen);
    }
    public void OnSend(int dwType, int nLen)
    {
      //  Singleton<PerformanceAnalyzer>.singleton.OnSend((uint)nLen);
    }
}
