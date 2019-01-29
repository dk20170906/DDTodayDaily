using DDTodayDaily.Common;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;           
using System.Linq;
using System.Web;

namespace DDTodayDaily.Models
{

    public class Emplayee
    {

        public int EmpId { get; set; }
        public string EmpUserId { get; set; }
        public string EmpUnionid { get; set; }
        public string EmpName { get; set; }
        public long EmpOrder { get; set; }
        public string EmpMobile { get; set; }
       public string EmpTel { get; set; }
        public string EmpWorkPlace { get; set; }
        public string EmpDepUserRemark { get; set; }
        public bool EmpIsAdmin { get; set; }
        public bool EmpIsBoss { get; set; }
        public bool EmpIsHide { get; set; }
        public bool EmpIsLeader { get; set; }
        public bool EmpActive { get; set; }
        public string EmpPosition { get; set; }
        public string EmpEmail { get; set; }
        public string EmpJobNumber { get; set; }
        public string EmpExtattr { get; set; }
        public int EmpTimeStamp { get; set; }
        public string EmpPlace { get; set; }
             public string EmpAvatar { get; set; }
        public string EmpDatailPlace { get; set; }
        public string EmpRemark { get; set; }
        public string EmpLatitude { get; set; }
        public string EmpLongitude { get; set; }
        public string EmpImageDate { get; set; }
        public string EmpDepartmentIdsStr { get; set; }

      private  List<Emplayee> empList = new List<Emplayee>();
        /// <summary>
        /// 获取一个部门的用户列表
        /// </summary>
        /// <param name="repuestParams"></param>
        /// <returns></returns>
        public List<Emplayee> GetOneDepEmplayee(RequestParams repuestParams)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/list");
            OapiUserListRequest request = new OapiUserListRequest();
            request.DepartmentId = repuestParams.DepId;
            request.Offset = repuestParams.OffSet;
            request.Size = repuestParams.Size;
            request.SetHttpMethod("GET");
            OapiUserListResponse response = client.Execute(request, TooUtil.GetAccessToken());
            if (response.Errcode==0&&response.Userlist!=null&&response.Userlist.Count>0)
            {
                response.Userlist.ForEach(u =>
                {
                    Emplayee emplayee = new Emplayee();
                    emplayee.EmpUserId = u.Userid;
                    emplayee.EmpName = u.Name;
                    emplayee.EmpUnionid = u.Unionid;
                    emplayee.EmpMobile = u.Mobile;
                    emplayee.EmpTel = u.Tel;
                    emplayee.EmpWorkPlace = u.WorkPlace;
                    emplayee.EmpRemark = u.Remark;
                    emplayee.EmpOrder = u.Order;
                    emplayee.EmpIsAdmin = u.IsAdmin;
                    emplayee.EmpIsBoss = u.IsBoss;
                    emplayee.EmpIsHide = u.IsHide;
                    emplayee.EmpIsLeader = u.IsLeader;
                    emplayee.EmpActive = u.Active;
                    emplayee.EmpDepartmentIdsStr = u.Department;
                    emplayee.EmpPosition = u.Position;
                    emplayee.EmpEmail = u.Email;
                    emplayee.EmpAvatar = u.Avatar;
                    emplayee.EmpJobNumber = u.Jobnumber;
                    emplayee.EmpExtattr = u.Extattr;
                    if (empList.All<Emplayee>(k=>k.EmpUserId!=emplayee.EmpUserId))
                    {
                        empList.Add(emplayee);
                    }
                });
            }
            if (response.HasMore)
            {
                GetOneDepEmplayee(repuestParams);
            }
            return empList;
        }
            /// <summary>
            /// 获取所有部门里的用户集合
            /// </summary>
            /// <param name="requestParams"></param>
            /// <returns></returns>
        public List<Emplayee> GetMoreDepUserList(RequestParams requestParams)
        {
            Department department = new Department();
            List<Emplayee> empList = new List<Emplayee>();
            List<Department> depList = department.GetAllDepIds(requestParams);
            if (depList.Count>0)
            {
                depList.ForEach(u =>
                {
                    RequestParams requestParamsm = requestParams;           
                    requestParamsm.DepId = u.DepId;               
                    List<Emplayee> empListm= GetOneDepEmplayee(requestParamsm);
                    empList = empList.Union(empListm).ToList();
                });
            }
            return empList;
        }
    }
}