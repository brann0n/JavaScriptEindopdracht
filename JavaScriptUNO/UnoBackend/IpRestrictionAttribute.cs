using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavaScriptUNO.UnoBackend
{
    public class IpRestrictionAttribute : AuthorizeAttribute
    {
        //only localhost and localsubnet addresses
        private readonly List<WhitelistModel> WhiteListIps = new List<WhitelistModel>()
        {
            new WhitelistModel("195.95.174.0", true),   
            new WhitelistModel("127.0.0.1", false) 
        };

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string ip = httpContext.Request.UserHostAddress;

            if (WhiteListIps.IpAllowed(ip))
            {
                return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized, "Based on your IP you cannot access this page.");
        }
    }

    public class WhitelistModel
    {
        public string Ip { get; set; }
        public bool IsSubnet { get; set; }
        public WhitelistModel(string ip, bool subnet)
        {
            Ip = ip;
            IsSubnet = subnet;
        }
    }
}