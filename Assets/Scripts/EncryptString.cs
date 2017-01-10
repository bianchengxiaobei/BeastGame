using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：EncryptString
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：xml加密管理
//----------------------------------------------------------------*/
#endregion
internal class EncryptString 
{
	private static DESCryptoServiceProvider s_provider = new DESCryptoServiceProvider();
	private static byte[] s_rgbIV = new byte[]
	{
		18,
		52,
		86,
		120,
		144,
		171,
		205,
		239
	};
	public static byte[] Encrypt(string PlainText, string strKey)
	{
		byte[] result;
		if (8 > strKey.Length)
		{
			result = null;
		}
		else
		{
			byte[] bytes = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, EncryptString.s_provider.CreateEncryptor(bytes, EncryptString.s_rgbIV), CryptoStreamMode.Write);
			StreamWriter streamWriter = new StreamWriter(cryptoStream);
			streamWriter.Write(PlainText);
			streamWriter.Close();
			cryptoStream.Close();
			byte[] array = memoryStream.ToArray();
			memoryStream.Close();
			result = array;
		}
		return result;
	}
	public static string Decrypt(byte[] CypherText, string strKey)
	{
		string result;
		if (8 > strKey.Length)
		{
			result = null;
		}
		else
		{
			byte[] bytes = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
			MemoryStream memoryStream = new MemoryStream(CypherText);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, EncryptString.s_provider.CreateDecryptor(bytes, EncryptString.s_rgbIV), CryptoStreamMode.Read);
			StreamReader streamReader = new StreamReader(cryptoStream);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			cryptoStream.Close();
			memoryStream.Close();
			result = text;
		}
		return result;
	}
}
