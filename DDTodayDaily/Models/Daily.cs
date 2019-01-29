

using DDTodayDaily.Common;
using DDTodayDaily.EF;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DDTodayDaily.Models
{
    [Table("dd_daily")]
    public class Daily
    {
        [Key]
        public int DaiId { get; set; }
        public string DaiContents { get; set; }
        public long DaiCreateTime { get; set; }
        public string DaiCreatorId { get; set; }
        public string DaiCreatorName { get; set; }
        public string DaiDate { get; set; }
        public string DaiDeptName { get; set; }
        public string DaiFinishWork { get; set; }
        public bool DaiHaoMore { get; set; }
        public string DaiKey { get; set; }
        public string DaiLocation { get; set; }
        public string DaiNeedHelpWork { get; set; }
        public long DaiNextCursor { get; set; }
        public string Remark { get; set; }
        public string DaiReportId { get; set; }
        public long DaiSize { get; set; }
        public string DaiSort { get; set; }
        public string DaiTemplateName { get; set; }
        public string DaiTomorrowWork { get; set; }
        public string DaiType { get; set; }
        public string DaiUnfinishWork { get; set; }
        public string DaiValue { get; set; }
        public string DaiWorkPlace { get; set; }

        private List<string> userReportId = new List<string>();
        private List<Daily> dailyList = new List<Daily>();
        private Dictionary<string, string> userlistIdandname = new Dictionary<string, string>();


        /// <summary>
        /// 提交数据     统计
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public List<DailyCount> GetDailyCounts(RequestParams requestParams)
        {
            userlistIdandname.Clear();
            dailyList.Clear();
            List<string> creatorIds = new List<string>();
            List<DailyCount> dailyCounts = new List<DailyCount>();
            //上个月年月
            string yearmonth = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");//上个月
            int boforeMonthWorkDays = TooUtil.GetWorlDaysForBeforeMonth();
            GetOneEmplayeeDaily(requestParams);
            if (userlistIdandname.Count > 0)
            {
                foreach (var item in userlistIdandname)
                {
                    try
                    {

                        DailyCount dailyCountm = new DailyCount();
                        dailyCountm.SubmitedNum = dailyList.FindAll(delegate (Daily daily)
                        {
                            return daily.DaiCreatorId == item.Key && daily.DaiDate != null && daily.DaiDate.Length > 0 && daily.DaiDate != "" && daily.DaiDate.StartsWith(yearmonth) == true;
                        }).Count;
                        if (dailyCountm.SubmitedNum >= boforeMonthWorkDays)
                        {
                            dailyCountm.SubmitedNum = boforeMonthWorkDays;
                        }
                        dailyCountm.EmpName = item.Value;
                        dailyCountm.ShouldSubNum = boforeMonthWorkDays;
                        dailyCountm.NotSubmitted = boforeMonthWorkDays - dailyCountm.SubmitedNum;
                        dailyCountm.BeforMonth = yearmonth;
                        DailyCount dailyCount = dailyCounts.Find(delegate (DailyCount dailyCountn) { return dailyCountn.EmpName == item.Value; });
                        if (dailyCount == null)
                        {
                            dailyCounts.Add(dailyCountm);
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return dailyCounts;
        }


        public List<Daily> GetOneEmplayeeDaily(RequestParams requestParams)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/report/list");
            OapiReportListRequest request = new OapiReportListRequest();
            request.StartTime = requestParams.StartTime;
            request.EndTime = requestParams.EndTime;
            request.Cursor = requestParams.Cursor;
            request.Size = requestParams.Size;
            OapiReportListResponse response = client.Execute(request, TooUtil.GetAccessToken());
            JObject jobjext = (JObject)JsonConvert.DeserializeObject(response.Body);
            if (jobjext["errcode"].ToString() == "0" && jobjext["result"]["data_list"].ToString().Length > 0 && jobjext["result"]["data_list"].ToString() != "")
            {
                JArray jr = (JArray)jobjext["result"]["data_list"];
                if (jr.Count > 0)
                {
                    foreach (var item in jr)
                    {
                        if (!userReportId.Contains(item["report_id"].ToString()))
                        {
                            userReportId.Add(item["report_id"].ToString());
                            Daily daily = new Daily();
                            daily.DaiContents = item["contents"].ToString();
                            if (item["contents"] != null)
                            {
                                JArray jrcont = (JArray)item["contents"];
                                if (jrcont.Count > 0)
                                {
                                    foreach (var ite in jrcont)
                                    {
                                        if (ite["key"] != null && ite["key"].ToString() == "日志日期")
                                        {
                                            daily.DaiDate = ite["value"].ToString();
                                        }
                                    }
                                }
                            }
                            daily.Remark = item["remark"].ToString();
                            daily.DaiTemplateName = item["template_name"].ToString();
                            daily.DaiDeptName = item["template_name"].ToString();
                            daily.DaiCreatorName = item["creator_name"].ToString();
                            daily.DaiCreatorId = item["creator_id"].ToString();
                            daily.DaiCreateTime = Convert.ToInt64(item["create_time"].ToString());
                            daily.DaiReportId = item["report_id"].ToString();
                            if (!userlistIdandname.ContainsKey(item["creator_id"].ToString()))
                            {
                                userlistIdandname.Add(item["creator_id"].ToString(), item["creator_name"].ToString());
                            }
                            dailyList.Add(daily);
                        }
                    }
                }
            }
            if (response.Result.HasMore)
            {
                RequestParams requestParametersDd = new RequestParams();
                requestParametersDd = requestParams;
                requestParametersDd.Cursor = response.Result.NextCursor;
                GetOneEmplayeeDaily(requestParametersDd);
            }
            return dailyList;
        }


        public int SavaDailyToSqlServer()
        {
            int m = 0;
            if (dailyList.Count>0)
            {
                DailyDBContext dailyDBContext = new DailyDBContext();
                dailyList.ForEach(u =>
                {
                    try
                    {
                        Daily dailym = dailyDBContext.DailyModel.FirstOrDefault(mk => mk.DaiReportId == u.DaiReportId);
                        if (dailym != null&&dailym.DaiId>0)
                        {
                            // dailyDBContext.Entry(daily).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            dailyDBContext.DailyModel.Add(u);
                        }
                        m += dailyDBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   
                });
            }
            return m;
        }


    }
    public class DailyContents
    {
        public string DailySort { get; set; }
        public string DailyType { get; set; }
        public string DailyValue { get; set; }
        public string DailyKey { get; set; }
    }
}