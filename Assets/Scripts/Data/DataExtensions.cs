using UnityEngine;
using System.Collections.Generic;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataExtensions
// 创建者：chen
// 修改者列表：
// 创建日期：
// 模块描述：
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public static class DataExtensions 
{
    public static List<int> GetExtraSkills(this DataBeastlist dataHeroList)
    {
        List<int> list = new List<int>();
        List<int> result;
        if (string.IsNullOrEmpty(dataHeroList.ExtraSkill))
        {
            result = list;
        }
        else
        {
            string[] array = dataHeroList.ExtraSkill.Split(new char[]
				{
					';'
				});
            for (int i = 0; i < array.Length; i++)
            {
                string value = array[i];
                int item = Convert.ToInt32(value);
                list.Add(item);
            }
            result = list;
        }
        return result;
    }
    /// <summary>
    /// 分割字符串，根据";"来分割，转换成List
    /// </summary>
    /// <param name="strArray"></param>
    /// <returns></returns>
    public static List<string> GetArrayList(string strArray)
    {
        List<string> list = new List<string>();
        if (!string.IsNullOrEmpty(strArray))
        {
            string[] array = strArray.Split(new char[]
				{
					';'
				});
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
        }
        return list;
    }
}
