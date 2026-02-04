/* File Name :- SubjectExpertAssignmentPopup.aspx.cs
 * Created Date :- 20-July-2016
 * Class Description :- This class is used to Assing subject teachers as a subject Expert.
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;
using SchoolEntities.Admin;
using System.Web.UI;

public partial class SubjectExpertAssignmentUI : SchoolBase
{
    #region Data Member(s)

    private ExamTypesConfigurationBL moExamTypesConfigurationBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill up subject combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillSubjectCombo();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Get Subject Teacher List for expert assignment.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiSubjectId></param>
    [WebMethod]
    public static DataSourceResult GetSubjectTeachers(int aiSchoolId, int aiAcademicYearId, int aiSubjectId)
    {
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL(aiSchoolId, aiAcademicYearId, 1);
        List<SubjectExperts> lstSubjectExpert = oAskMeQuestionMasterBL.GetSubjectExperts(aiSubjectId);

        var result = new DataSourceResult()
        {
            Data = lstSubjectExpert,
            Total = lstSubjectExpert.Count
        };

        return result;
    }

    /// <summary>
    /// This Event is used to save Teacher as a Subject Expert.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="asTeacherId"></param>
    /// <param name="aiSubjectId></param>
    [WebMethod]
    public static string SaveExpert(int aiSchoolId, int aiAcademicYearId, string asTeacherId, int aiSubjectId)
    {
        string sMessage = string.Empty;
        try
        {
            AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL(aiSchoolId, 0, 1);
            oAskMeQuestionMasterBL.SaveSubjectExperts(aiSchoolId, aiAcademicYearId, aiSubjectId, asTeacherId);
        }
        catch (SqlException se)
        {
            sMessage = se.Message;
        }
        return sMessage;
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();

        string sQuerystring = "MenuId=" + Convert.ToInt32(Constants.SchoolConfigMenuId.Ask_Me_Related);
        hidPostbackURL.Value = "../Admin/SchoolConfigurationControlPanel.aspx" + "?" + CommonUtility.EncryptQuerystring(sQuerystring);
    }

    /// <summary>
    /// This Method is used to fill the Subjects Combobox.
    /// </summary>
    private void FillSubjectCombo()
    {
        moExamTypesConfigurationBL = new ExamTypesConfigurationBL();
        List<YearWiseSubjectsDetails> lstSubjects = moExamTypesConfigurationBL.GetAllYearwiseSubjects(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstSubjects, cmbSubjects, "SubjectName", "SubjectId", Constants.S_SELECT);
    }

    #endregion
}