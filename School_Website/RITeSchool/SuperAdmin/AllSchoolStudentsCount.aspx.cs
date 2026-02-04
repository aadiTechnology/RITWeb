using System;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;
using System.Data;
using Utility;
using System.Web.Services;
using Kendo.DynamicLinq;
using SchoolEntities;
using System.Linq;
public partial class AllSchoolStudentsCount : SchoolBase
{

    #region -- EVENT(s) --
    /// <summary>
    /// This event is used to Initialize controls on Page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) 
            {
                FillAcademicYears();
            }
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
    
    #region --Private Methods--
    /// <summary>
    /// This method is used to fill academic year dropdown.
    /// </summary>
    private void FillAcademicYears()
    {
        var oAllSchoolStudentCountBL = new AllSchoolStudentCountBL();
        List<AcademicYear> lstYearInfo = oAllSchoolStudentCountBL.GetAllAcademicYears();
        cmbAcademicYear.Bind(lstYearInfo, "Id", "Year");
        cmbAcademicYear.SelectedValue = lstYearInfo.Last().Id.ToString();
    }
    #endregion

    #region --Public Methods--
    /// <summary>
    /// This method is used to get total count of students.
    /// </summary>
    /// <param name="asAcademicYear"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetStudentsCount(string asAcademicYear)
    {
        try
        {
            var oAllSchoolStudentCountBL = new AllSchoolStudentCountBL();
            List<AllStudentCount> lstStudentsCount = oAllSchoolStudentCountBL.GetStudentsCountList(asAcademicYear);

            var result = new DataSourceResult()
            {
                Data = lstStudentsCount,
                Total = lstStudentsCount.Count
            };

            return result;
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
            return null;
        }
    }
    #endregion
}