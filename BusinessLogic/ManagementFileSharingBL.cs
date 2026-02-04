/* Filename		:- ManagementFileSharingBL.cs
 * Author		:- Vishal Shah
 * Created On	:- 17-August-2011
 * Description	:- This is the Business Logic Layer Class for the File Sharing feature for SuperAdmins (Management Role Group)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{

	public class ManagementFileSharingBL
	{

		#region -- MEMBER(s) --

		ManagementFileSharingDC moManagementFileSharingDC;

		#endregion -- MEMBER(s) --


		#region -- PROPERTIES --

		public ManagementFileUploadDetails FileUploadDetails
		{
			get { return moManagementFileSharingDC.moFileUploadDetails; }
			set { moManagementFileSharingDC.moFileUploadDetails = value; }
		}

		#endregion -- PROPERTIES --


		#region -- CONSTRUCTOR(s) --

		public ManagementFileSharingBL()
		{
			moManagementFileSharingDC = new ManagementFileSharingDC();
		}

		public ManagementFileSharingBL(int aiFileUploadId)
		{
			moManagementFileSharingDC = new ManagementFileSharingDC(aiFileUploadId);
		}

		#endregion -- CONSTRUCTOR(s) --


		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// Retruns a paged list of files uploaded on the server
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="sortExpression"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		public List<ManagementFileUploadDetails> GetAllFiles(int aiSchoolId, int aiAcademicYearId, int aiUserId, string sortExpression, int maximumRows, int startRowIndex)
		{
			int iEndIndex = startRowIndex + maximumRows;
			// Increment the startRowIndex to prevent returning the last record of the previous page.
			if(startRowIndex != 0) startRowIndex++;
			return moManagementFileSharingDC.GetAllFiles(aiSchoolId, aiAcademicYearId, aiUserId, sortExpression, startRowIndex, iEndIndex);
		}

		/// <summary>
		/// Gets a count of the files uploaded on the server
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="sortExpression"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		public int GetCount(int aiSchoolId, int aiAcademicYearId, int aiUserId, string sortExpression, int maximumRows, int startRowIndex)
		{
			return moManagementFileSharingDC.GetCount(aiSchoolId, aiAcademicYearId, aiUserId);
		}

		/// <summary>
		/// This function is used to Insert AND Update FileUpload details in the database
		/// </summary>
		/// <param name="aoFileDetails">The ManagementFileUploadDetails object which contains details about the file to upload/update</param>
		/// <returns>A boolean value indicating if the operation was succesfull</returns>
		public bool InsertFile(ManagementFileUploadDetails aoFileDetails)
		{
			return moManagementFileSharingDC.InsertFile(aoFileDetails);
		}

		/// <summary>
		/// Marks an uploaded file as deleted in the database.
		/// </summary>
		/// <param name="aiFileId"></param>
		/// <returns></returns>
		public bool DeleteFile(int aiFileId)
		{
			return moManagementFileSharingDC.DeleteFile(aiFileId);
		}

		/// <summary>
		/// Marks the file as read when a user has downloaded the file.
		/// </summary>
		/// <param name="aiUserId"></param>
		public static void MarkAsRead(int aiFileUploadId, int aiUserId)
		{
			ManagementFileSharingDC.MarkAsRead(aiFileUploadId, aiUserId);
		}

		#endregion -- PUBLIC METHOD(s) --


		#region -- PRIVATE METHOD(s)--



		#endregion -- PRIVATE METHOD(s) --

	}
}