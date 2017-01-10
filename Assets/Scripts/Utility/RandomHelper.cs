using UnityEngine;
using System.Collections;
using Random = System.Random;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：RandomHelper
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：各种随机数
//----------------------------------------------------------------*/
#endregion
namespace Utility
{
    public class RandomHelper
    {
        private static Random random = CreateRandom();
        /// <summary>
        /// 创建一个产生不重复随机数的随机生成器
        /// </summary>
        /// <returns></returns>
        public static System.Random CreateRandom()
        {
            long tick = DateTime.Now.Ticks;
            return new System.Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }
        /// <summary>
        /// 返回一个随机整数，如果positive为true，就是随机非负整数，否则就是正负1/2的概率
        /// </summary>
        /// <param name="positive">是否为正整数</param>
        /// <returns></returns>
        public static int GetRandomInt(bool positive = true)
        {
            if (positive)
            {
                return random.Next();
            }
            else 
            {
                int flag = random.Next() % 2;
                if (flag == 0)
                {
                    return random.Next();
                }
                else 
                {
                    return -random.Next();
                }
            }
        }
        /// <summary>
        /// 如果max为正，取得小于max的随机正整数,如果max为负数，那么取得大于-max的随机负整数
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int max)
        {
            if (max > 0)
            {
                return random.Next() % max;
            }
            else if (max < 0)
            {
                return -random.Next() % max;
            }
            else 
            {
                return 0;
            }
        }
        /// <summary>
        /// 取得给定范围内的随机数，如果min比max还要大，就返回最小值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int min, int max)
        {
            if (min < max)
            {
                return random.Next(min, max);
            }
            else 
            {
                return max;
            }
        }
        
        /// <summary>
        /// 返回介于0.0~1.0随机浮点数
        /// </summary>
        /// <returns></returns>
        public static float GetRandomFloat()
        {
            return (float)random.NextDouble();
        }
        /// <summary>
        /// 返回介于0.0~max随机浮点数
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetRandomFloat(float max)
        {
            return (float)random.NextDouble() * max;
        }
        /// <summary>
        /// 返回min~max随机浮点数，如果min>max则返回max最小值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetRandomFloat(float min, float max)
        {
            if (min < max)
            {
                return (float)random.NextDouble() * (max - min) + min;
            }
            else 
            {
                Debug.LogWarning("RandomHelper:min > max");
                return max;
            }
        }
    }
}