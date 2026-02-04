/* Filename		:- FileUploadDetails.cs
 * Author		:- Vishal Shah
 * Created On	:- 17-August-2011
 * Description	:- This is the Entity Class for the File Sharing feature for SuperAdmins (Management Role Group)
 *				   which holds details about a single uploaded file.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{

	public class ManagementFileUploadDetails : SchoolEntity
	{

		#region -- PROPERTIES --

		public int UploadId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string FilePath { get; set; }
		public int UploadedById { get; set; }
		public string UploadedBy { get; set; }
		public DateTime UploadDate { get; set; }
		public string SelectedUserIds { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool IsRead { get; set; }

		#endregion -- PROPERTIES --

	}
}
