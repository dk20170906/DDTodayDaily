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

    public class Department
    {

        public int Id { get; set; }
        public bool AutoAddUser { get; set; }
        public bool CreateDeptGroup { get; set; }
        public long  DepId { get; set; }
        public string DepName { get; set; }
        public long ParentId { get; set; }

        /// <summary>
        ///获取所有的部门列表信息
        /// </summary>
        /// <param name="repuestParams"></param>
        /// <returns></returns>
        public List<Department> GetAllDepIds(RequestParams repuestParams)
        {
            List<Department> depList = new List<Department>();
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            request.Id = "1";//如果不传，默认为1表示根部门id
            request.SetHttpMethod("GET");
            OapiDepartmentListResponse response = client.Execute(request, TooUtil.GetAccessToken());
                     if(response.Errcode==0&&response.Department.Count>0)
            {
                response.Department.ForEach(u =>
                {
                    Department department = new Department();
                    department.DepId = u.Id;
                    department.ParentId = u.Parentid;
                    department.DepName = u.Name;
                    department.CreateDeptGroup = u.CreateDeptGroup;
                    department.AutoAddUser = u.AutoAddUser;
                    depList.Add(department);
                });
            }
            return depList;
        }
    }
}