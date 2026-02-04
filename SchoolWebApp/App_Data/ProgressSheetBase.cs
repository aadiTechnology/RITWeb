/*
 * This Class is act as base class for progress sheet.
 * Any Type of progress sheet rendered in school can derive from this class 
 * to implement individual logic and display it to screen
 * Author: Shankar Gurav.
 * Date of creation: 27 Feb 2008
 * Date of modification: 27 Feb 2008
 */
using System;
using BusinessLogic;
using Utility;
using System.Data;

/// <summary>
/// Summary description for ProgressSheetBase
/// </summary>
public abstract class ProgressSheetBase : SchoolBase
{
    protected int miTestId = 0;
    protected Constants.PageMode menmPagemode = Constants.PageMode.Normal;
    public bool mbViewStudnetwiseProgressReport = false;
    public string msTestId = string.Empty;
    /// <summary>
    /// This methoid is used to create and fill progress sheet
    /// </summary>
    public abstract void ShowProgressSheet(int iStudentId);


    /// <summary>
    /// Used to get set Test Id
    /// </summary>
    public int TestId
    {
        get
        {
            return miTestId;
        }
        set
        {
            miTestId = value;
        }
    }

    /// <summary>
    /// Used to get set page mode.
    /// </summary>
    public Constants.PageMode PageMode
    {
        get
        {
            return menmPagemode;
        }
        set
        {
            menmPagemode = value;
        }
    }

    /// <summary>
    /// This methoid is used to create and fill progress sheet
    /// </summary>
    public abstract Int32 ShowProgressSheet(int iTeacherId, int iStudentId);

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    public Boolean isTestPublishedForStdDivId(int aiStandardDivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        return oSWStdDivTestMasterBL.isAnyTestPublished(miSchoolId, miAcademicYearId, aiStandardDivisionId);
    }

    public bool isTestPublishedForStudent(int aiStudentId, int aiStanderddivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        return oSWStdDivTestMasterBL.IsAnyTestPublishedForStudent(miSchoolId, miAcademicYearId, aiStudentId, aiStanderddivisionId);
    }


    /// <summary>
    /// This method is used to check whether term exam is published.
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <returns></returns>
    public bool IsTermExamPublished(int aiStandardDivisionId, out string asStandardName)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        return oSWStdDivTestMasterBL.IsTermExamPublished(miSchoolId, miAcademicYearId, aiStandardDivisionId, out asStandardName);
    }

    /// <summary>
    /// This method is used to check whether term exam is published.
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <returns></returns>
    public bool IsLastYEarTermExamPublished(int aiStandardDivisionId, int aiSchoolId, int aiAcademicYearId, out string asStandardName)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(aiSchoolId, aiAcademicYearId,aiStandardDivisionId);
        return oSWStdDivTestMasterBL.IsTermExamPublished(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, out asStandardName);
    }

    /// <summary>
    /// This method is used to check whether term exam is published.
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <returns></returns>
    public bool IsFinalResultPublished(int aiStandardDivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        return oSWStdDivTestMasterBL.IsFinalResultPublished(miSchoolId, miAcademicYearId, aiStandardDivisionId);
    }

    /// <summary>
    /// This method is used to check whether term exam is published.
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <returns></returns>
    public bool IsLatYearFinalResultPublished(int aiStandardDivisionId, int aiSchoolId, int aiAcademicYearId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(aiSchoolId, aiAcademicYearId,aiAcademicYearId);
        return oSWStdDivTestMasterBL.IsFinalResultPublished(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
    }

    public DataTable GetStudentsLastAYDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        return oSWStdDivTestMasterBL.GetStudentsLastAYDetails(aiSchoolId, aiAcademicYearId, aiStudentId);
    }
}
