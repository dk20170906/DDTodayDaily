using DDTodayDaily.Models;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace DDTodayDaily.Common
{
    public class TooUtil
    {
        public static string GetAccessToken()
        {
            string corpId = System.Configuration.ConfigurationManager.AppSettings.Get("corpId");
            string corpSecret = System.Configuration.ConfigurationManager.AppSettings.Get("corpSecret");

            String url = "https://oapi.dingtalk.com/gettoken?corpid=" + corpId + "&corpsecret=" + corpSecret;

            String response = HttpHandler.Get(url);

            if (response != null && response.Length > 0 && response != "")
            {
                ResultObj jsonObj = JsonConvert.DeserializeObject<ResultObj>(response);
                if (jsonObj.Errcode == 0)
                {
                    return jsonObj.Access_token;
                }
                else
                {
                    return jsonObj.Errmsg;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        /// <summary>
        /// 通过时间时间戳得到时间
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
        /// <summary>
        /// 获取指定时间的时间  戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
        }
        /// <summary>
        /// 获取上个月有多少个工作日
        /// </summary>
        /// <returns></returns>
        public static int GetWorlDaysForBeforeMonth()
        {
            //上个月年月
            string yearmonthoneday = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01";//上个月
            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString(yearmonthoneday));    // 当前日期月份的第一天
            int year = dt.Year;            // 获得年
            int month = dt.Month;   // 获得月
            int days = DateTime.DaysInMonth(year, month);     // 获得该月总共多少天

            // 休息天数
            int weekDays = 0;

            for (int i = 0; i < days; i++)
            {
                // 判断是否为周六，周日，是则记录天数。
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        weekDays++;
                        break;
                    case DayOfWeek.Sunday:
                        weekDays++;
                        break;
                }
                dt = dt.AddDays(1);
            }
            // 工作日
            return days - weekDays;
        }


       /// <summary>
       /// 将字符串转为list集合
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="JsonStr"></param>
       /// <returns></returns>
        public static List<T> JSONStringToList<T>( string JsonStr)
        {

            JavaScriptSerializer Serializer = new JavaScriptSerializer();

            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);

            return objs;

        }
        public static T Deserialize<T>(string json)

        {

            T obj = Activator.CreateInstance<T>();

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))

            {

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());

                return (T)serializer.ReadObject(ms);

            }
        }
    }
}