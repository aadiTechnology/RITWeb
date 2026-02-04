/*
* This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 27 Feb 2008
 * Date of modification: 27 Feb 2008
 */
using System;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
/// <summary>
/// Summary description for ProgressSheet
/// </summary>
public class ProgressSheet
{
	public ProgressSheet()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	public static ProgressSheetBase GetProgressSheet(Panel oPanel, int iSchoolId, int iAcademicYearId, int iUserId, Constants.UserRoles oUserRole)
	{
		if (IsPrePrimaryExamConfiguration(iSchoolId, iAcademicYearId, iUserId,oUserRole.ToString()))
		{
			ProgressSheetBase oProgressSheetBase = new PrePrimaryStudentProgressDisplay(oPanel);
			return oProgressSheetBase;
		}
		else
		{
			ProgressSheetBase oProgressSheetBase = new StudentProgress(oPanel);
			return oProgressSheetBase;
		}
	}

	public static bool IsPrePrimaryExamConfiguration(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asUserRole)
	{
		TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
		return oTeacherStandardDetailsBL.IsPreprimaryExamConfiguration(aiSchoolId, aiAcademicYearId, aiUserId,asUserRole);
	}

	private static bool IsUserPrePrimary(int iSchoolId, int iAcademicYearId, int iUserId, Constants.UserRoles oUserRole)
	{
		Boolean bResult = false;
		if (oUserRole == Constants.UserRoles.Teacher)
		{
			TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
			bResult = oTeacherStandardDetailsBL.IsTeacherPrePrimary(iSchoolId, iAcademicYearId, iUserId);
		}
		else if (oUserRole == Constants.UserRoles.Student)
		{
			StudentBL oStudentBL = new StudentBL();
			bResult = oStudentBL.IsStudentPrePrimary(iSchoolId, iAcademicYearId, iUserId);
		}
		return bResult;
	}

}
