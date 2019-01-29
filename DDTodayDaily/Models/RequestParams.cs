using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDTodayDaily.Models
{
    public class RequestParams
    {
        public int Id { get; set; }
        public string ApiMethod { get; set; }
        public string Session { get; set; }
        public string Timestamp { get; set; }
        public string Format { get; set; }
        public string V { get; set; }
        public string PartnerId { get; set; }
        public string Simplify { get; set; }
        public string OpenComverSation { get; set; }
        public long OffSet { get; set; }
        public int Count { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string TemplateName { get; set; }
        public string UserId { get; set; }
        public List<string> UserIdList { get; set; }
        public long Cursor { get; set; }
        public long Size { get; set; }
        public string DepIdStr { get; set; }
        public long DepId { get; set; }

    }
}