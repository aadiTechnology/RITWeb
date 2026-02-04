// File Name  : UserMailingList.cs
// Created By : Pravin 
// Date       : 24/07/2013
//Description : This class is used to handle the mailing group popup.
using System;
using System.Collections.Generic;
using BookEntities;
namespace SchoolEntities
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Designation { get; set; }
        public bool IsInGroup { get; set; }
        public bool IsDeactivated { get; set; }
    }

    public class MailingGroup
    {
        public int GroupId { get; set; }
        public string Name { get; set; }        
        public List<UserRoles> lstUserRoles { get; set; }
        public string Users { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAllDeactivated { get; set; }
    }
}
