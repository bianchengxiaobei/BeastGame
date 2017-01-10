using System.Threading;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：Singleton
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility
{
    public class Singleton<T> where T : new()
    {
        private static T s_singleton = default(T);
        private static object s_objectLock = new object();
        public static T singleton
        {
            get
            {
                if (Singleton<T>.s_singleton == null)
                {
                    object obj;
                    Monitor.Enter(obj = Singleton<T>.s_objectLock);
                    try
                    {
                        if (Singleton<T>.s_singleton == null)
                        {
                            Singleton<T>.s_singleton = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
                        }
                    }
                    finally
                    {
                        Monitor.Exit(obj);
                    }
                }
                return Singleton<T>.s_singleton;
            }
        }
        protected Singleton()
        {
        }
    }
}