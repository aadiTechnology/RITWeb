/*
 * File Name - SurveyFeedbackUI.aspx.cs
 * Created By - Sachin
 * Created Date - 13 Mar 2015
 * Description - This class is used to submit feedback / survey details on given parameter(s).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Survey;
using Utility;

public partial class SurveyFeedbackUI : SchoolBase
{
    #region Data Member(s)
    
    private SurveyFeedbackBL moSurveyFeedbackBL;
    private List<SurveyFeedbackDetails> mlstFeedbackDetails; 

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill feedback details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moSurveyFeedbackBL = new SurveyFeedbackBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                     btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillFeedbackDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill feedback details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSurveyFeedbackBL = new SurveyFeedbackBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();
                FillFeedbackDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save feedback / survey details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();

            if (hidConfirmSubmit.Value == Constants.S_YES)
            {
                int iUserId = hidUserId.Value.ToInt();
                int iSurveyId = hidSurveyId.Value.ToInt();
                moSurveyFeedbackBL.Submit(iUserId, iSurveyId, true);
                base.DisplayMessage("Feedback details saved and submitted successfully !!!", false, tdMessage);
            }
            else
                base.DisplayMessage("Feedback details saved successfully !!!", false, tdMessage);

            FillFeedbackDetails();            
            hidConfirmSubmit.Value = Constants.S_NO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit feedback / survey details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iUserId = hidUserId.Value.ToInt();
            int iSurveyId = hidSurveyId.Value.ToInt();
            Save();
            moSurveyFeedbackBL.Submit(iUserId, iSurveyId, true);
            FillFeedbackDetails();

            base.DisplayMessage("Feedback details submitted successfully !!!", false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        if (moSurveyFeedbackBL.IsFeedbackSubmitted)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
        }
        else
        {
            if (mlstFeedbackDetails.Count == 0)
                btnSubmit.Enabled = false;
            else
                btnSubmit.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to fill feedback / survey details.
    /// </summary>
    private void FillFeedbackDetails()
    {
        int iSurveyId = QueryString["SurveyId"].ToInt();
        int iUserId = QueryString["UserId"].ToInt();

        tblGrades.Rows.Clear();
        tblParameter.Rows.Clear();

        mlstFeedbackDetails = moSurveyFeedbackBL.GetFeedbackDetails(iSurveyId, iUserId);

        SetSchoolDetails();
        FillGrades();
        FillParameters();
        SetButtonState();

        hidNonMandatoryFieldIds.Value = string.Empty;
        List<int> lstParameterIds = moSurveyFeedbackBL.FeedbackParameters.Where(prm => !prm.IsMandatory).Select(prm => prm.Id).ToList();
        if (lstParameterIds.Count > 0)
        {
            string sIds = string.Join(",", lstParameterIds);
            if(sIds.StartsWith(","))
                sIds = sIds.Substring(1);
            hidNonMandatoryFieldIds.Value = sIds;
        }
    }

    /// <summary>
    /// This method is used to fill parameters.
    /// </summary>
    private void FillParameters()
    {
        moSurveyFeedbackBL.FeedbackCategories.ForEach
            (
                categoryObj =>
                {
                    int iSrNo = 1;
                    if (categoryObj.ShowNameOnReport)
                    {
                        HtmlTableRow trSkill = new HtmlTableRow();
                        AddCell(trSkill, categoryObj.Name, "ClsProgressGridTestHeader", "left", 4);
                        tblParameter.Rows.Add(trSkill);
                    }
                    else
                    {
                        HtmlTableRow trSkill = new HtmlTableRow();
                        AddCell(trSkill, string.Empty, string.Empty, "left", 4, "height:10px");
                        tblParameter.Rows.Add(trSkill);
                    }

                    if (((Constants.FeedbackInputTypes)categoryObj.InputTypeId) == Constants.FeedbackInputTypes.Grade)
                    {
                        HtmlTableRow trHeader = new HtmlTableRow();
                        AddCell(trHeader, Resources.LocalizedResources.SrNo, "ClsProgressGridTestHeader", "Center", 1, "Width:50px");
                        AddCell(trHeader, "Parameter", "ClsProgressGridTestHeader", "left", 1);
                        AddCell(trHeader, "Grade", "ClsProgressGridTestHeader", "left", 1);
                        tblParameter.Rows.Add(trHeader);
                    }
                    else if (((Constants.FeedbackInputTypes)categoryObj.InputTypeId) == Constants.FeedbackInputTypes.Text)
                    {
                        if (!categoryObj.ShowNameOnReport)
                        {
                            HtmlTableRow trHeader = new HtmlTableRow();
                            AddCell(trHeader, Resources.LocalizedResources.SrNo, "ClsProgressGridTestHeader", "Center", 1, "Width:50px");
                            AddCell(trHeader, "Parameter", "ClsProgressGridTestHeader", "left", 1);
                            AddCell(trHeader, "Observation", "ClsProgressGridTestHeader", "left", 1);
                            tblParameter.Rows.Add(trHeader);
                        }
                    }

                    moSurveyFeedbackBL.FeedbackParameters
                    .Join(moSurveyFeedbackBL.FeedbackCategories, parameter => parameter.FeedbackCategoryId, category => category.Id, (parameter, category) => new
                    {
                        Title = parameter.Title,
                        ParameterId = parameter.Id,
                        CategorySortOrder = category.SortOrder,
                        ParameterSortOrder = parameter.SortOrder,
                        CategoryId = category.Id,
                        InputTypeId = category.InputTypeId,
                        AllowParameterUpdation = parameter.AllowParameterUpdation
                    })
                    .Where(ct => ct.CategoryId == categoryObj.Id)
                    .OrderBy(ct => ct.CategorySortOrder)
                    .ToList()
                    .ForEach
                     (
                            parameter =>
                            {
                                HtmlTableRow oHtmlTableRow = new HtmlTableRow { ID = "tr_" + parameter.ParameterId };

                                if (categoryObj.IsEditableToAll)
                                {
                                    Label oLabel = new Label { ID = "lblParameter_" + parameter.ParameterId, Text = (iSrNo++).ToString() };
                                    AddCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Center", 1, string.Empty, oLabel);

                                    var oSurveyDetails = mlstFeedbackDetails.Where(obsrv => obsrv.FeedbackParameterId == parameter.ParameterId).FirstOrDefault();

                                    if (parameter.AllowParameterUpdation)
                                    {

                                        TextBox oTextBox = new TextBox { ID = "txtParameter_" + parameter.ParameterId, Width = Unit.Pixel(150), MaxLength = 100 };

                                        if (oTextBox != null)
                                        {
                                            if (oSurveyDetails != null)
                                                oTextBox.Text = oSurveyDetails.ParameterSubject;

                                            if (moSurveyFeedbackBL.IsFeedbackSubmitted)
                                                oTextBox.Enabled = false;
                                        }

                                        AddCell(oHtmlTableRow, parameter.Title + "&nbsp;&nbsp;&nbsp;", "ClsMarksCell", "left", 1, string.Empty, oTextBox);
                                    }
                                    else
                                        AddCell(oHtmlTableRow, parameter.Title, "ClsMarksCell", "left", 1);

                                    HtmlTableCell oObservation = new HtmlTableCell();

                                    SetObservationAssignmentView(parameter.ParameterId, oHtmlTableRow, oSurveyDetails, parameter.InputTypeId);
                                    oHtmlTableRow.Cells.Add(oObservation);
                                }
                                else
                                    AddCell(oHtmlTableRow, parameter.Title, "ClsMarksCell", "left", 4, "line-height:20px;font-weight:bold;background-color:white");

                                tblParameter.Rows.Add(oHtmlTableRow);
                            });
                }
            );
    }

    /// <summary>
    /// This method is used to set observation assignment view.
    /// </summary>
    /// <param name="aiParameterId"></param>
    /// <param name="aoReportingStaff"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aoObservation"></param>
    private void SetObservationAssignmentView(int aiParameterId, HtmlTableRow aoHtmlTableRow, SurveyFeedbackDetails aoObservation, int aiInputTypeId)
    {
        var bIsMandatory = moSurveyFeedbackBL.FeedbackParameters.Where(prm => prm.Id == aiParameterId).FirstOrDefault().IsMandatory;
        if (((Constants.FeedbackInputTypes)aiInputTypeId) == Constants.FeedbackInputTypes.Grade)
        {
            DropDownList oDropDownList = new DropDownList { ID = "cmbGrade_" + aiParameterId, Width = Unit.Pixel(180) };
            if (oDropDownList != null)
            {
                oDropDownList.Items.Clear();
                List<FeedbackGrade> lstFeedbackGrades = moSurveyFeedbackBL.FeedbackGrades.Select(gd => new FeedbackGrade { ShortName = gd.ShortName + " (" + gd.Name + ")", Id = gd.Id }).ToList();

                ListSource.FillDropDownList(lstFeedbackGrades, oDropDownList, "ShortName", "Id", Constants.S_SELECT);

                if (aoObservation != null)
                    oDropDownList.SelectedValue = aoObservation.FeedbackGradeId.ToString();

                if (moSurveyFeedbackBL.IsFeedbackSubmitted)
                    oDropDownList.Enabled = false;

                AddCellWithMandatoryMark(aoHtmlTableRow, string.Empty, "ClsMarksCell", "left", 1, "Text-Align:left", oDropDownList, bIsMandatory);
            }
        }
        else if (((Constants.FeedbackInputTypes)aiInputTypeId) == Constants.FeedbackInputTypes.Text)
        {
            TextBox oTextBox = new TextBox { ID = "txtObservation_" + aiParameterId, Width = Unit.Percentage(100), TextMode = TextBoxMode.MultiLine };

            if (oTextBox != null)
            {
                if (aoObservation != null)
                    oTextBox.Text = aoObservation.Observation;

                if (moSurveyFeedbackBL.IsFeedbackSubmitted)
                    oTextBox.Enabled = false;

                AddCellWithMandatoryMark(aoHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, string.Empty, oTextBox, bIsMandatory);
            }
        }
    }

    /// <summary>
    /// This method is used to set grades.
    /// </summary>
    private void FillGrades()
    {
        HtmlTableRow oHeader = new HtmlTableRow();
        base.AddCell(oHeader, Resources.LocalizedResources.KeyToRate, "HeadTxtBWOPadding", "Center", 2, "Color:Navy");
        tblGrades.Rows.Add(oHeader);

        HtmlTableRow oHeaderNames = new HtmlTableRow();
        base.AddCell(oHeaderNames, Resources.LocalizedResources.Grade, "ClsProgressGridTestHeader", "Left", 1, "Width:20%");
        base.AddCell(oHeaderNames, Resources.LocalizedResources.Description, "ClsProgressGridTestHeader", "Left");
        tblGrades.Rows.Add(oHeaderNames);

        moSurveyFeedbackBL.FeedbackGrades.ForEach
            (
                grade =>
                {
                    HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                    base.AddCell(oHtmlTableRow, grade.ShortName, "ClsMarksCell", "Left");
                    base.AddCell(oHtmlTableRow, grade.Description, "ClsMarksCell", "Left");
                    tblGrades.Rows.Add(oHtmlTableRow);
                });
    }

    /// <summary>
    /// This method is used to set school details.
    /// </summary>
    private void SetSchoolDetails()
    {
        lblSchoolName.Text = moSurveyFeedbackBL.SchoolInfo.SchoolName;
        lblOrgName.Text = moSurveyFeedbackBL.SchoolInfo.OrganizationName;
        lblSchoolAddress.Text = moSurveyFeedbackBL.SchoolInfo.Address;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnSubmit });
        string sAssignmentPage = CommonUtility.EncryptQuerystring("Year=" + QueryString["Year"]);
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
        btnSave.Attributes.Add("onclick", "ConfirmSave()");

        hidSurveyId.Value = QueryString["SurveyId"].ToString();
        hidUserId.Value = QueryString["UserId"].ToString();
    }

    /// <summary>
    /// This method is used to save feedback / survey details.
    /// </summary>
    private void Save()
    {
        List<SurveyFeedbackDetails> lstObservations = new List<SurveyFeedbackDetails>();
        foreach (HtmlTableRow tr in tblParameter.Rows)
        {
            if (tr.ID != null)
            {
                string sParameterId = tr.ID.Substring(tr.ID.IndexOf("_") + 1);

                DropDownList cmbGrade = tr.FindControl("cmbGrade_" + sParameterId) as DropDownList;
                TextBox txtObservation = tr.FindControl("txtObservation_" + sParameterId) as TextBox;
                TextBox txtParameter = tr.FindControl("txtParameter_" + sParameterId) as TextBox;

                SurveyFeedbackDetails oSurveyFeedbackDetails = new SurveyFeedbackDetails();
                oSurveyFeedbackDetails.FeedbackParameterId = sParameterId.ToInt();
                oSurveyFeedbackDetails.FeedbackGradeId = 0;
                oSurveyFeedbackDetails.ParameterSubject = string.Empty;
                oSurveyFeedbackDetails.Observation = string.Empty;
                if (cmbGrade != null && cmbGrade.SelectedValue != Constants.S_ZERO)
                    oSurveyFeedbackDetails.FeedbackGradeId = cmbGrade.SelectedValue.ToInt();

                if (txtObservation != null)
                    oSurveyFeedbackDetails.Observation = txtObservation.Text.Trim();

                if (cmbGrade != null || txtObservation != null)
                    lstObservations.Add(oSurveyFeedbackDetails);

                if (txtParameter != null)
                {
                    txtParameter.Text = txtParameter.Text.Trim();
                    if (txtParameter.Text != string.Empty)
                        oSurveyFeedbackDetails.ParameterSubject = txtParameter.Text;
                }
            }
        }

        if (lstObservations.Count > 0)
        {
            string sXml = base.GenerateXml(lstObservations);

            int iUserId = hidUserId.Value.ToInt();
            int iSurveyId = hidSurveyId.Value.ToInt();

            moSurveyFeedbackBL.Save(iUserId, iSurveyId, sXml);
        }
    } 

    #endregion
}