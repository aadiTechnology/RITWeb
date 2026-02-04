using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;

public partial class SubmitProgreesReportResult : SchoolBase
{
    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
			if (!IsPostBack)
            {
                GetQuerystring();
                FillMonthsGridView();
                SetDefaultValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdMonths_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        const string S_RESULT_ALREADY_PUBLISHED = "Result already published";
        const string S_NOT_PUBLISHED = "Not submitted";

        try
        {
            if (e.Row.RowIndex >= 0)
            {
                int rowindex = e.Row.RowIndex;

                bool bSumbit = Convert.ToBoolean(grdMonths.DataKeys[rowindex]["IsSubmitted"]);
                bool bPublish = Convert.ToBoolean(grdMonths.DataKeys[rowindex]["IsPublished"]);
                string sRollNos = Convert.ToString(grdMonths.DataKeys[rowindex]["RollNos"]);

                ImageButton btnStatus = (ImageButton)e.Row.FindControl("btnStatus");
                ImageButton btnUnpublish = (ImageButton)e.Row.FindControl("btnUnpublish");
                TextBox txlUnpublishReason = (TextBox)e.Row.FindControl("txlUnpublishReason");
                Label olblStatus = (Label)e.Row.FindControl("lblStatus");
                if (Convert.ToBoolean(hidIsUnpublish.Value))
                {
                    btnStatus.Visible = false;
                    olblStatus.Visible = true;
                    if (!bSumbit)
                    {
                        olblStatus.Text = S_NOT_PUBLISHED;
                        olblStatus.ToolTip = S_NOT_PUBLISHED;
                    }
                    else if (bSumbit && !bPublish)
                    {
                        olblStatus.Text = "Submitted but not yet published";
                        olblStatus.ToolTip = "Submitted but not yet published";
                    }
                    else if (bPublish)
                    {
                        olblStatus.Text = S_RESULT_ALREADY_PUBLISHED;
                        olblStatus.ToolTip = S_RESULT_ALREADY_PUBLISHED;
                        btnUnpublish.Visible = true;
                        txlUnpublishReason.Visible = true;
                        btnUnpublish.ImageUrl = "~/RITeSchool/images/icoGrid_SubmitExamMarks.gif";
                        btnUnpublish.ToolTip = "Unpublish";
                        btnUnpublish.Attributes.Add("onclick", "if(!ConfirmUnpublishAction(" + (rowindex + 2) + ")) return false;");
                    }
                }
                else
                {
                    if (hidIsMonthConfig.Value != string.Empty)
                    {
                        if (!bSumbit)
                        {
                            btnStatus.Attributes.Add("onclick", "if(!ConfirmAction('" + sRollNos + "',false)) return false;");
                            btnStatus.ImageUrl = "~/RITeSchool/images/icoGrid_SubmitExamMarks.gif";
                            btnStatus.ToolTip = "Submit";
                        }
                        else
                        {
                            btnStatus.Visible = false;
                            olblStatus.Visible = true;
                            if (bPublish)
                            {
                                olblStatus.Text = S_RESULT_ALREADY_PUBLISHED;
                                olblStatus.ToolTip = S_RESULT_ALREADY_PUBLISHED;
                            }
                            else
                            {
                                olblStatus.Text = "Result already Submitted";
                                olblStatus.ToolTip = "Result already Submitted";
                            }
                        }
                    }
                    else
                    {
                        if (bSumbit && !bPublish)
                        {
                            btnStatus.Attributes.Add("onclick", "if(!ConfirmAction('" + sRollNos + "',true)) return false;");
                            btnStatus.ImageUrl = "~/RITeSchool/images/icoGrid_SubmitExamMarks.gif";
                            btnStatus.ToolTip = "Publish";
                        }
                        else if (!bSumbit)
                        {
                            btnStatus.Visible = false;
                            olblStatus.Visible = true;
                            olblStatus.Text = S_NOT_PUBLISHED;
                            olblStatus.ToolTip = S_NOT_PUBLISHED;
                        }

                        if (bPublish)
                        {
                            btnStatus.Visible = false;
                            olblStatus.Visible = true;
                            olblStatus.Text = S_RESULT_ALREADY_PUBLISHED;
                            olblStatus.ToolTip = S_RESULT_ALREADY_PUBLISHED;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdMonths_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblSuccess.Text = string.Empty;
            int iRowIndex = Convert.ToInt32(e.CommandArgument);
            PrePrimaryProgressReportMonthsBL oPrePrimaryProgressReportMonthsBL = new PrePrimaryProgressReportMonthsBL();
            PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = new PrePrimaryConfiguredMonthDetails
            {
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId,
                PreprimaryExamConfigurationId = Convert.ToInt32(grdMonths.DataKeys[iRowIndex]["PreprimaryExamConfigurationId"]),
            };

            if (e.CommandName == "SUBMIT")
            {
                bool isPublished = hidIsMonthConfig.Value != string.Empty ? false : true;
                oPrePrimaryConfiguredMonthDetails.IsSubmitted = true;
                oPrePrimaryConfiguredMonthDetails.IsPublished = hidIsMonthConfig.Value != string.Empty ? false : true;
                oPrePrimaryProgressReportMonthsBL.PrePrimaryConfiguredMonthDetailsEntity = oPrePrimaryConfiguredMonthDetails;
                oPrePrimaryProgressReportMonthsBL.UpdateStatusClass();
                if (isPublished)
                    lblSuccess.Text = "Result published successfully !!!";
                else
                    lblSuccess.Text = "Result submited successfully !!!";
            }
            else if (e.CommandName == "UBPUBLISH")
            {
                TextBox txlUnpublishReason = (TextBox)grdMonths.Rows[iRowIndex].FindControl("txlUnpublishReason");
                oPrePrimaryConfiguredMonthDetails.UnpublishReason = txlUnpublishReason.Text.Trim();
                oPrePrimaryProgressReportMonthsBL.PrePrimaryConfiguredMonthDetailsEntity = oPrePrimaryConfiguredMonthDetails;
                oPrePrimaryProgressReportMonthsBL.UnpublishExam();
                lblSuccess.Text = "Result unpublished successfully !!!";
            }

            FillMonthsGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
  
    #endregion

    #region Private Methods

    private void GetQuerystring()
    {
        if (QueryString.Count > 0)
        {
            if (QueryString["StandardDivisionId"] != null)
                hidStandDivId.Value = QueryString["StandardDivisionId"];
            hidIsMonthConfig.Value = QueryString["IsMonthConfig"] ?? String.Empty;
            if (QueryString["IsUnpublish"] != null)
                hidIsUnpublish.Value = QueryString["IsUnpublish"];
        }
    }

    private void FillMonthsGridView()
    {
        int iStandDivId = Convert.ToInt32(hidStandDivId.Value);
        PrePrimaryProgressReportMonthsBL oPrePrimaryProgressReportMonthsBL = new PrePrimaryProgressReportMonthsBL();
        oPrePrimaryProgressReportMonthsBL.GetClasswiseMonthsList(miSchoolId, miAcademicYearId, iStandDivId);
        List<PrePrimaryConfiguredMonthDetails> olstPrePrimaryProgressReportMonths = oPrePrimaryProgressReportMonthsBL.PrePrimaryConfiguredMonthList;
        grdMonths.DataSource = olstPrePrimaryProgressReportMonths;
        grdMonths.DataBind();
    }

    private void SetDefaultValues()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel });
        btnCancel.Attributes.Add("onclick", "window.close();");
        if (Convert.ToBoolean(hidIsUnpublish.Value))
        {
            grdMonths.Columns[2].Visible = true;
            grdMonths.Columns[3].Visible = true;
        }
        else
        {
            grdMonths.Columns[2].Visible = false;
            grdMonths.Columns[3].Visible = false;
        }
    }

    #endregion
}
