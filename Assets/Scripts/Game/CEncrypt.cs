using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CEncrypt
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：密码加密类
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    internal class CEncrypt
    {
        public static string Encrypt(string user, string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + "chenfuling");
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }
            return stringBuilder.ToString().ToLower();
        }
    }
}