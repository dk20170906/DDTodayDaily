using DDTodayDaily.Common;
using DDTodayDaily.EF;
using DDTodayDaily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DDTodayDaily.Controllers
{
    public class DailyController : Controller
    {
        // GET: Daily
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDailyCounts()
        {
            Daily daily = new Daily();            
            RequestParams requestParams = new RequestParams();
            requestParams.Cursor = 0L;
            requestParams.Size = 100L;
            requestParams.OffSet = 0L;

            DateTime dt = DateTime.Now;
            int days = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("dailyDaysBefore"));
            DateTime dtbefor = dt.AddDays(days);
            requestParams.StartTime = TooUtil.ToUnixTime(dtbefor);
            requestParams.EndTime = TooUtil.ToUnixTime(dt);
            //保存数据库
           Task<int> task = new Task<int>(new Func<int>(() => { return daily.SavaDailyToSqlServer(); }));
            Task<List<DailyCount>> task1 = new Task<List<DailyCount>>(new Func<List<DailyCount>>(() =>
            {
             return daily.GetDailyCounts(requestParams);
            }));
     
            task1.Start();
             Task.WaitAll(new Task[] { task1 });
            task.Start();
            if (task1.Result.Count>0)
            {
                return Json(new
                {
                    code = 0,
                    msg = "请求成功！",
                    data = task1.Result
                }, JsonRequestBehavior.AllowGet);
            }


            //if (dailyContents.Count>0)
            //{
            //    return Json(new
            //    {
            //        code = 0,
            //        msg = "请求成功！",
            //        data = dailyContents
            //    }, JsonRequestBehavior.AllowGet);
            //}
            return Json(new
            {
                code = -1,
                msg = "请求失败！",
            }, JsonRequestBehavior.AllowGet);
        }
           

        public int AddDailyTest()
        {
            try
            {
                Daily daily = new Daily();
                daily.Remark = "654d54sd";
                DailyDBContext dailyDBContext = new DailyDBContext();
                dailyDBContext.DailyModel.Add(daily);
                int m = dailyDBContext.SaveChanges();
                return m;
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
    }
}
