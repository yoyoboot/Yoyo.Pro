using System;
using System.Collections.Generic;

namespace Yoyo.Pro.Extensions
{
    /// <summary>
    /// 集合list<see cref="IList<T>"/>的扩展方法
    /// </summary>
    public static class IListExtensions
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        /// 对集合进行混乱排序的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
