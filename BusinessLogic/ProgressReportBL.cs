// -----------------------------------------------------------------------
/* File Name - ProgressReportBL.cs
 * Created Date - 22-March-2013
 * Created by - Lakshman Shinde
 * Class Description - This class is used for Block student progress report.
 */
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using Utility;
using ProgressReportEntities;
namespace BusinessLogic
{

	public class ProgressReportBL
	{

		#region Data members

			ProgressReportDC moProgressReportDC;

		#endregion

		# region Property(s)

		#endregion

		#region Constructor

		public ProgressReportBL()
		{
			moProgressReportDC = new ProgressReportDC();
		}

		public ProgressReportBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
		{
			moProgressReportDC = new ProgressReportDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
		}

		#endregion

		#region Public methods

		/// <summary>
		/// This method is uesd to get list of blocked unblocked student 
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiTeacherId"></param>
		/// <param name="abShowblocked"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asSearch"></param>
		/// <param name="sortExpression"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		public List<BlockStudentsProgressReportDetails> GetAllBlockedUnBlockedStudents(int aiStdDivId, bool abShowblocked, int aiStudentId, string asSearch, String sortExpression, int maximumRows, int startRowIndex)
		{
			if (asSearch.IsNullOrEmpty())
				asSearch = string.Empty;
			int iStartIndex = startRowIndex;
			int iEndIndex = iStartIndex + maximumRows;
			List<BlockStudentsProgressReportDetails> lstBlockStudentsProgressReportDetails = moProgressReportDC.GetAllBlockedUnBlockedStudents(aiStdDivId, abShowblocked, aiStudentId, asSearch, sortExpression, iStartIndex, iEndIndex);
			return lstBlockStudentsProgressReportDetails;
		}

		/// <summary>
		/// This menthod is used to get the count of blocked unblocked student
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiTeacherId"></param>
		/// <param name="abShowblocked"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asSearch"></param>
		/// <param name="sortExpression"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		public int GetCount(int aiStdDivId, bool abShowblocked, int aiStudentId, string asSearch, String sortExpression, int maximumRows, int startRowIndex)
		{
			return moProgressReportDC.StudentCount;
		}

		/// <summary>
		/// This Method is uesd to Save the reason to block progres report
		/// </summary>
		/// <param name="asXml"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiInsertedById"></param>
		/// <param name="abIsUpdateOrUnblock"></param>
		public void SaveBlockStudentDetails(string asXml, bool abIsUpdateOrUnblock)
		{
			moProgressReportDC.SaveBlockStudentDetails(asXml, abIsUpdateOrUnblock);
		}

		/// <summary>
		/// This methosd is used to get Progress report blocked reason to display on progress report
		/// </summary>
		/// <param name="aiStudentId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public string GetBlockProgressReportReason(int aiStudentId)
		{
			return moProgressReportDC.GetBlockProgressReportReason(aiStudentId);
		}

		#endregion
	}

}