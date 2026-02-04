
// -----------------------------------------------------------------------
// <copyright file="UserLoginDetails.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserLoginDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string ClassName { get; set; }
    }

    public class UserData
    {
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string Type { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsSWCoordinator { get; set; }
    }
}
