using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST.Models
{
    public class Action
    {
        public string Name { get; set; }
    }

    public class Module
    {
        public List<Action> Actions { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Result
    {
        public object ADID { get; set; }
        public object BrandCode { get; set; }
        public string CRMID { get; set; }
        public object CRM_ID { get; set; }
        public object DefaultOpco { get; set; }
        public object Email { get; set; }
        public object Facilities { get; set; }
        public object LIMSID { get; set; }
        public string LIMSID_Lab { get; set; }
        public string M3ID { get; set; }
        public object M3ID_ChildIDs { get; set; }
        public List<object> OPCOs { get; set; }
        public object PIN { get; set; }
        public int cmpID { get; set; }
        public string cmpKey { get; set; }
        public string cmpName { get; set; }
        public List<Module> Modules { get; set; }
    }

    public class HALogin
    {
        public List<Result> result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }
}