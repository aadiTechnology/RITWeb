// -----------------------------------------------------------------------
// <copyright file="SupportDetails.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities
{
    using System;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SupportDetails : SchoolEntity
    {
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string FileName { get; set; }
        public int Id { get; set; }
        public string MobileNo { get; set; }
        public string Subject { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
