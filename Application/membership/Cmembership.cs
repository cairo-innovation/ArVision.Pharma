//using CustomAuthenticationMVC.DataAccess;
//using LMS.Admin.model;
//using LMS.Admin.dal;
using Pharma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
//using LMS.Admin.bll;

namespace Pharma.membership
{
    public class Cmembership : MembershipProvider
    {


        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="username"></param>  
        /// <param name="password"></param>  
        /// <returns></returns>  
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            //get data
            //bllUserRole bll = new bllUserRole();
            //var user = bll.UserRoleRepository.SearchFor(s =>
            //          s.Username.ToLower() == username.ToLower()
            //          && s.Password == password && s.Active!=0 && (s.RoleID==1 || s.RoleID==2 || s.RoleID==4)).FirstOrDefault();
            //if (user == null)
            //{
            //    bllRegisteredReferralChannel bllr = new bllRegisteredReferralChannel();
            //    saleuser = bllr.SearchFor(s =>
            //          (s.Name.ToLower().Trim() == username.ToLower().Trim()
            //            || s.SalesEmail.ToLower().Trim() == username.ToLower().Trim())
            //            && s.SalesPassword == password).FirstOrDefault();
            //}
            //var blls = new bllSystemUser();
            //SystemUser systemuser = null;
            //if (adminuser != null)
            //{
            //    systemuser = blls.Get(g => g.UserId == adminuser.Id, "Roles").FirstOrDefault();
            //}
            //else if (saleuser != null)
            //{
            //    systemuser = blls.Get(g => g.UserId == saleuser.Id, "Roles").FirstOrDefault();
            //}
            //if (systemuser.Active != true)
            //{
            //    adminuser = null;
            //    saleuser = null;
            //}
            //User user = UserRepository.LogIn(username, password);
            //using (AuthenticationDB dbContext = new AuthenticationDB())
            //{
            //    var user = (from us in dbContext.Users
            //                where string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0
            //                && string.Compare(password, us.Password, StringComparison.OrdinalIgnoreCase) == 0
            //                && us.IsActive == true
            //                select us).FirstOrDefault();

            //    return (user != null) ? true : false;
            //}
            return (username.ToLower().Trim()=="admin" && password=="admin") ? true  : false;
        }

        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="username"></param>  
        /// <param name="password"></param>  
        /// <param name="email"></param>  
        /// <param name="passwordQuestion"></param>  
        /// <param name="passwordAnswer"></param>  
        /// <param name="isApproved"></param>  
        /// <param name="providerUserKey"></param>  
        /// <param name="status"></param>  
        /// <returns></returns>  
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="username"></param>  
        /// <param name="userIsOnline"></param>  
        /// <returns></returns>  
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            //get data
            //bllUserRole bll = new bllUserRole();
            //var user = bll.UserRoleRepository.Get(s =>
            //          s.Username.ToLower() == username.ToLower(), "user", "role").FirstOrDefault();
            Models.User uuser = new Models.User();
            uuser.FirstName = "Admin";
            uuser.LastName = "Admin";
            uuser.Password = "admin";
            uuser.UserName = "admin";
            uuser.UserId =1;
            //user.UserId = systemuser.UserId;
            var roles = new List<Models.Role>();
            roles.Add(new Models.Role { RoleName = "Admin" });
            uuser.Roles = roles;//systemuser.Roles;
            //}
            var selectedUser = new CmembershipUser(uuser);

            return selectedUser;
        }

        public override string GetUserNameByEmail(string email)
        {
            //using (AuthenticationDB dbContext = new DataAccess.AuthenticationDB())
            //{
            //    string username = (from u in dbContext.Users
            //                       where string.Compare(email, u.Email) == 0
            //                       select u.Username).FirstOrDefault();

            //    return !string.IsNullOrEmpty(username) ? username : string.Empty;
            //}
            return !string.IsNullOrEmpty("") ? "" : string.Empty;
        }

        #region Overrides of Membership Provider  

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}