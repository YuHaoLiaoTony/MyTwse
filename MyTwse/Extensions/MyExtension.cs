﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MyTwse.Extensions
{
    public static class MyExtension
    {
        public static bool IsHoliday(this DateTime date)
        {
            Task<bool> isHoliday = date.IsHolidayAsync();

            isHoliday.Wait();

            return isHoliday.Result;
        }
        /// <summary>
        /// 判断是不是周末/节假日
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>周末和节假日返回true，工作日返回false</returns>
        public static async Task<bool> IsHolidayAsync(this DateTime date)
        {
            var isHoliday = false;

            var webClient = new System.Net.WebClient();

            var PostVars = new System.Collections.Specialized.NameValueCollection
            {
                { "d", date.ToString("yyyyMMdd") }//参数
            };

            try
            {
                var day = date.DayOfWeek;
                //判断是否为周末
                if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
                    return true;

                //0为工作日，1为周末，2为法定节假日

                var byteResult = await webClient.UploadValuesTaskAsync("http://tool.bitefu.net/jiari/", "POST", PostVars);//请求地址,传参方式,参数集合

                var result = Encoding.UTF8.GetString(byteResult);//获取返回值

                if (result == "1" || result == "2")
                    isHoliday = true;

            }
            catch
            {
                isHoliday = false;
            }

            return isHoliday;

        }
    }

}
