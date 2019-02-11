using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharma.Models
{
    public class AccountViewModel
    {
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class CSerializeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> RoleName { get; set; }

    }

    public class User
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

    }
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}