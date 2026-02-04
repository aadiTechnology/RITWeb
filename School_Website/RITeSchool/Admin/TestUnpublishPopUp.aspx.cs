// File Name  : PrePrimaryProgressReportConfig.aspx.cs
// Created By : Shankar
// Date       : 22/10/2007

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// This Class is used to add and edit holiday management configuration.
/// </summary>
public partial class TestUnpublishPopUp : SchoolBase
{
    #region Event
    /// <summary>
    /// This method is used to decrypt query string and fill grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
                InitialiseForm();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to unpublish test
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            int iStdDivId = Convert.ToInt32(hidStandardDivisionId.Value);
            int iTestId = Convert.ToInt32(hidTestId.Value);
            ///testid -9999 means we are unpublishing final result and not any perticular test
            if (hidFrom.Value == null || hidFrom.Value == "0")
            {
                if (iTestId != -9999)
                {
                    SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, iStdDivId, iTestId);
                    oSWStdDivTestMasterBL.UnPublishTest(txtUnPublishReason.Text.Trim());
                }
                else
                {
                    SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, iStdDivId);
                    if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id != 0)
                        oSchoolWisdeAnnualResultPublishBL.UnPublishFinalResult(txtUnPublishReason.Text.Trim());
                }
            }
            else
            {
                AssignXseedGradesBL.Unpublish(iStdDivId, iTestId, miAcademicYearId, miSchoolId, txtUnPublishReason.Text.Trim(),miUserId);
            }
            Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + "'?" + hidQuery.Value + "'" + ";window.close();window.opener.focus(); </Script>");

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method event is used to close the window.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<Script language='Javascript'>window.close();</Script>");
    }

    #endregion

    #region Private Method

    /// <summary>
    /// This method initializes variables.
    /// </summary>
    private void InitialiseForm()
    {
        ReadQuerystring();
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnUnPublish.Attributes.Add("Onclick", "if(!(ClearErrorLabel('" + hidDependentExamNames.ClientID + "'))){return false;}");        
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnUnPublish });
    }
    
    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        hidQuery.Value = Request.QueryString.ToString();
		if (QueryString["TestId"] != null)
            hidTestId.Value = QueryString["TestId"];
		if (QueryString["StandardDivisionId"] != null)
			hidStandardDivisionId.Value = QueryString["StandardDivisionId"];
        if (QueryString["TeacherId"] != null)
            lblTeacherHeading.Text = QueryString["TeacherId"];
        if (QueryString["sTeacherName"] != null)
            lblTeacherHeading.Text = QueryString["sTeacherName"];
        if (QueryString["sTestName"] != null)
            lblTestName.Text = QueryString["sTestName"];
        hidFrom.Value = QueryString["From"] ?? Constants.S_ZERO;
        lblTestlbl.Text = hidFrom.Value != Constants.S_ZERO ? "Assessment :" : "Exam :";
        CheckPublishExamDependency();
    }

    public void CheckPublishExamDependency()
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        oSWStdDivTestMasterBL.School_id = miSchoolId;
        oSWStdDivTestMasterBL.Acadmic_year_id = miAcademicYearId;
        oSWStdDivTestMasterBL.Standerd_division_Id = Convert.ToInt32(hidStandardDivisionId.Value); 
        oSWStdDivTestMasterBL.SchoolWise_Test_Id = Convert.ToInt32(hidTestId.Value);
        oSWStdDivTestMasterBL.Is_Published = Constants.C_YES;
        oSWStdDivTestMasterBL.Inserted_By_id = miUserId;

        oSWStdDivTestMasterBL.CheckPublishExamDependency();
        hidDependentExamNames.Value = oSWStdDivTestMasterBL.lstPublishExamDependencyMaster[0].DependentExamName.ToString();
    }
    #endregion
}

