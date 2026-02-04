// File Name  : TeacherMailingList.cs
// Created By : Pravin 
// Date       : 24/07/2013
//Description : This class is used to handle the mailing group popup.
using System.Collections.Generic;
namespace SchoolEntities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TeacherMailingList
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string Designation { get; set; }
        public int UserId { get; set; }
    }

    public class MailingGroup
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int IsDeleted { get; set; }
    }
}
