using System;
//using CustomAuthenticationMVC.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Pharma.Models;
//using LMS.Admin.model;

namespace Pharma.membership
{
    public class CmembershipUser : MembershipUser
    {
        #region User Properties  

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Models.Role> Roles { get; set; }

        #endregion

        public CmembershipUser(Models.User user) : base("Cmembership", user.FirstName, user.UserId, user.FirstName, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.UserId.Value;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = user.Roles;
        }
    }
}