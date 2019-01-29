using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDTodayDaily.Models
{
    public class DailyCount
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string CreatorId { get; set; }
        public int AccountMonth { get; set; }
        public string BeforMonth { get; set; }
        public int AccountYear { get; set; }
        public int AccountDay { get; set; }

        public string ReportId { get; set; }

        public string CreateDate { get; set; }
        public int ShouldSubNum { get; set; }
        public int SubmitedNum { get; set; }
        public int NotSubmitted { get; set; }
    }
}