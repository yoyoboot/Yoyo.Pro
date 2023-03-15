using System;

namespace Yoyo.Pro.Extensions
{

    /// <summary>
    ///   小数<see cref="decimal" />类型的扩展辅助操作类
    /// </summary>
    public static class DecimalExtensions
    {

        /// <summary>
        /// 将秒转换为时分秒结构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHourMinSecond(this decimal value)
        {
            var length = Convert.ToInt32(value);

            //500056
          
            int hour = 0;
            int minute = 0;
            int second = length;
             

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }

            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }

            if (hour<1)
            {
                return ( minute.ToString() + ":"
                  + second.ToString());
            }
            else
            {
                return (hour.ToString() + ":" + minute.ToString() + ":"
                  + second.ToString());
            }

           // var str=

           
            
        }



         
    }
}
