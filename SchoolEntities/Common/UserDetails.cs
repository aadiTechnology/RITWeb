using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterEntities;

namespace SchoolEntities
{
    public class UserDetails : SchoolEntity
    {
        public int User_Id { get; set; }
        public bool CanApproveVoucher { get; set; }
        public bool CanCreateVoucher { get; set; }
        public bool CanSelfApprove { get; set; }
        public bool Is_Locked { get; set; }
    }

	public class UserDocument
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int UserId { get; set; }
		public int DocumentCount { get; set; }
		public int DocumentTypeId { get; set; }
	}

	public class UserEducationDetails:SchoolEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public Qualification Qualification { get; set; } 		
		public string YearOfPassing { get; set; }
		public string University { get; set; }
		public int PassClassId { get; set; }
		public string Class { get; set; }
		public int AttachmentCount { get; set; }
	}

	public class UserExperienceDetails:SchoolEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Organization { get; set; }
		public DateTime JoiningDate { get; set; }
		public DateTime LeftDate { get; set; }
		public int AttachmentCount { get; set; }
	}

}
