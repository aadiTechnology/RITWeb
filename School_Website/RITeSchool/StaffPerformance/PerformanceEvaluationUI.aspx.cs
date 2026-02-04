using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using StaffPerformanceEntity;
using Utility;
using System.Web.UI.HtmlControls;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class PerformanceEvaluationUI : SchoolBase
{
    #region Data Member(s)

    private StaffPerformanceEvaluationBL moStaffPerformanceEvaluationBL;
    private List<StaffPerformanceObservation> mlstObservations;

    #endregion

    #region Property(s)

    private bool IsFinalApprover
    {
        get
        {
            return moStaffPerformanceEvaluationBL.ReportingStaffs.Where(staff => staff.ReportingUserId == miUserId && staff.IsFinalApprover).Any();
        }
    }

    private bool IsSupervisor
    {
        get
        {
            return moStaffPerformanceEvaluationBL.ReportingStaffs.Where(staff => staff.ReportingUserId == miUserId && staff.IsSupervisor).Any();
        }
    }

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to set master page.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (QueryString["IsViewMode"] == Constants.S_YES)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moStaffPerformanceEvaluationBL = new StaffPerformanceEvaluationBL(miSchoolId, miUserId, QueryString["UserId"].ToInt(), miAcademicYearId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnPublish.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    timer.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                     btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillPerformanceEvaluationDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill performance evaluation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (QueryString["UserId"].ToInt() == miUserId)
                    hidIsLoginUser.Value = Constants.S_YES;
                else
                    hidIsLoginUser.Value = Constants.S_NO;

                FillPerformanceEvaluationDetails();
                SetJavaScriptAttributes();
                timer.Enabled = true;
            }
            SetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save performance evaluation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            Save();
            FillPerformanceEvaluationDetails();
            base.DisplayMessage(Resources.LocalizedResources.msgStaffPerformanceSaved, false, tdMessage);
            timer.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to publish performance evaluation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            int iYear = QueryString["Year"].ToInt();
            bool bIsPublish = btnPublish.Text == Resources.LocalizedResources.Publish ? true : false;
            if (bIsPublish)
                Save();

            string sEffectiveDate = txtEffectiveFromDate.Text;
            string sLastIncrementDate = txtLastIncrementDate.Text;

            moStaffPerformanceEvaluationBL.Publish(iYear, bIsPublish, sEffectiveDate, sLastIncrementDate);
            FillPerformanceEvaluationDetails();
            if (bIsPublish)
                base.DisplayMessage(Resources.LocalizedResources.msgStaffPerformancePublished, false, tdMessage);
            else
                base.DisplayMessage(Resources.LocalizedResources.msgStaffPerformanceUnPublished, false, tdMessage);
            timer.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        timer.Enabled = false;
        int iUserId = QueryString["UserId"].ToInt();
        moStaffPerformanceEvaluationBL.RejectSubmittion(iUserId, txtReason.Text.Trim(), miUserId, miAcademicYearId, QueryString["Year"].ToInt());
        FillPerformanceEvaluationDetails();
        txtReason.Text = string.Empty;
        timer.Enabled = true;
    }

    /// <summary>
    /// This event is used to submit performance evaluation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            int iYear = QueryString["Year"].ToInt();
            Save();
            bool bIsSubmitAction = (btnSubmit.Text == "Submit" ? true : false);
            moStaffPerformanceEvaluationBL.Submit(iYear, bIsSubmitAction);
            FillPerformanceEvaluationDetails();
            base.DisplayMessage(Resources.LocalizedResources.msgStaffPerformanceSubmitted, false, tdMessage);
            if (bIsSubmitAction)
                btnSubmit.Text = "Un Submit";
            else
                btnSubmit.Text = "Submit";
            timer.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save performance evaluation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void timer_Tick(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Enabled)
            {
                timer.Enabled = false;
                Save();
                timer.Enabled = true;
                hidBtnState.Value = "true";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill up performance evaluation details.
    /// </summary>
    private void FillPerformanceEvaluationDetails()
    {
        int iYear = QueryString["Year"].ToInt();

        tblGrades.Rows.Clear();
        tblParameter.Rows.Clear();
        tblLinks.Rows.Clear();
        mlstObservations = moStaffPerformanceEvaluationBL.GetAll(miUserId, iYear);
        if (SetView())
        {
            SetSchoolDetails();
            SetUserDetails();
            
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                if (QueryString["Year"].ToInt() >= 51)
                {
                    var oTypeId = moStaffPerformanceEvaluationBL.PerformanceParameters.Select(PP => PP.AppraisalFormTypeId).FirstOrDefault();
                    if (oTypeId == 2)
                        FillGrades();
                }
                else
                    FillGrades();
            }
            else
                FillGrades();

            FillParameters();
            SetButtonState();
            AddAttachment();
        }
    }

    /// <summary>
    /// This method is used to show message if performance details are not published.
    /// </summary>
    private bool SetView()
    {
        if (moStaffPerformanceEvaluationBL.UserDetails == null)
        {
            tdFinalApprover.Visible = true;
            tblEvaluation.Visible = false;
            return false;
        }
        else
        {
            tdFinalApprover.Visible = false;
            tblEvaluation.Visible = true;
            return true;
        }
    }

    /// <summary>
    /// This method is used to set buttons state.
    /// </summary>
    private void SetButtonState()
    
    {
       btnSave.Enabled = moStaffPerformanceEvaluationBL.ButtonState.EnableSaveButton;
        hidBtnState.Value = moStaffPerformanceEvaluationBL.ButtonState.EnableSaveButton.ToString();
        btnSubmit.Enabled = moStaffPerformanceEvaluationBL.ButtonState.EnableSubmitButton;
        btnRejectSubmittion.Enabled = moStaffPerformanceEvaluationBL.ButtonState.EnableRejectButton;

        if (!moStaffPerformanceEvaluationBL.ButtonState.EnableSaveButton)
        {
            txtClassestaught.ReadOnly = true;
            txtTeachersubjects.ReadOnly = true;
        }

        if (!moStaffPerformanceEvaluationBL.ButtonState.CanUserAddComments)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            hidBtnState.Value = "false";
        }

        if (IsFinalApprover)
        {
            btnPublish.Enabled = moStaffPerformanceEvaluationBL.ButtonState.EnablePublishButton;
            btnViewReport.Enabled = moStaffPerformanceEvaluationBL.ButtonState.EnablePublishButton;

            if (moStaffPerformanceEvaluationBL.ButtonState.IsPublished)
            {
                btnPublish.Text = "Un Publish";
                hidIsPublishAction.Value = Constants.S_NO;
            }
            else
            {
                btnPublish.Text = "Publish";
                hidIsPublishAction.Value = Constants.S_YES;

                if (moStaffPerformanceEvaluationBL.ButtonState.EnablePublishButton)
                {
                    btnSubmit.Text = "Un Submit";
                    btnSubmit.Enabled = true;
                }                
            }
        }
        else
        {
            btnPublish.Visible = false;
            btnViewReport.Visible = false;
        }

        if (QueryString["UserId"].ToInt() == miUserId)
            btnRejectSubmittion.Visible = false;

        btnPublish.Attributes.Add("onclick", "if(!ConfirmPublish('" + (btnPublish.Text == Resources.LocalizedResources.Publish ? Constants.S_ONE : Constants.S_ZERO) + "')) return false;");
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
    }

    /// <summary>
    /// This method is used to fill parameters.
    /// </summary>
    private void FillParameters()
    {
        HtmlTableRow trHeader = new HtmlTableRow();
        this.AddTableCell(trHeader, Resources.LocalizedResources.SrNo, "ClsProgressGridTestHeader", "Center", 1, "Width:50px");
        this.AddTableCell(trHeader, Resources.LocalizedResources.PerformanceParameter, "ClsProgressGridTestHeader", "Center", 3);
        tblParameter.Rows.Add(trHeader);

        moStaffPerformanceEvaluationBL.PerformanceSkills.ForEach
            (
                skillObj =>
                {
                    HtmlTableRow trSkill = new HtmlTableRow();
                    this.AddTableCell(trSkill, skillObj.SkillName, "ClsProgressGridTestHeader", "left", 4);
                    tblParameter.Rows.Add(trSkill);

                    int iSrNo = 1;

                    moStaffPerformanceEvaluationBL.PerformanceParameters
                    .Join(moStaffPerformanceEvaluationBL.PerformanceSkills, parameter => parameter.SkillId, skill => skill.SkillId, (parameter, skill) => new { Title = parameter.Title, ParameterId = parameter.Id, SkillSortOrder = skill.SortOrder, ParameterSortOrder = parameter.SortOrder,
                            SkillId = skill.SkillId, InputTypeId = skill.InputTypeId})
                    .Where(skl => skl.SkillId == skillObj.SkillId)
                    .OrderBy(skl => skl.SkillSortOrder)
                    .ToList()
                    .ForEach
                     (
                            parameter =>
                            {
                                HtmlTableRow oHtmlTableRow = new HtmlTableRow { ID = "tr_" + parameter.ParameterId };
                                Label oLabel = new Label { ID = "lblParameter_" + parameter.ParameterId, Text = (iSrNo++).ToString() };
                                this.AddTableCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Center", 1, string.Empty, oLabel);
                                this.AddTableCell(oHtmlTableRow, parameter.Title, "ClsMarksCell", "left", 3, "font-weight:bold");
                                tblParameter.Rows.Add(oHtmlTableRow);

                                HtmlTableRow trObservation = new HtmlTableRow();
                                this.AddTableCell(trObservation, string.Empty, "ClsMarksCell");

                                HtmlTableCell oObservation = new HtmlTableCell();
                                FillObservations(oObservation, parameter.ParameterId, parameter.InputTypeId, skillObj.IsEditableToAll);
                                trObservation.Cells.Add(oObservation);

                                tblParameter.Rows.Add(trObservation);
                            });
                }
            );
    }

    /// <summary>
    /// This method is used to add cell into given row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asCaption"></param>
    /// <param name="asClass"></param>
    /// <param name="asAlign"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyles"></param>
    /// <param name="aoControl"></param>
    private void AddTableCell(HtmlTableRow aoHtmlTableRow, string asCaption, string asClass, string asAlign = "Center", int aiColSpan = 1, string asStyles = "", Control aoControl = null)
    {
        string[] stl;
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = asCaption, Align = asAlign, ColSpan = aiColSpan };
        oHtmlTableCell.Attributes.Add("class", asClass);
        if (aoControl != null)
            oHtmlTableCell.Controls.Add(aoControl);

        oHtmlTableCell.Style.Add("Padding-Left", "5pt");

        if (asStyles != string.Empty)
        {
            string[] sArrStyles = asStyles.Split(';');
            sArrStyles.ToList().ForEach
                (
                    style =>
                    {
                        if (style.Trim() != string.Empty)
                        {
                            stl = style.Split(':');
                            if (stl[0] != string.Empty && stl[1] != string.Empty)
                                oHtmlTableCell.Style.Add(stl[0], stl[1]);
                            stl = null;
                        }
                    });
        }

        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }

    /// <summary>
    /// This method is used to fill observations.
    /// </summary>
    /// <param name="aoHtmlTableCell"></param>
    /// <param name="aiParameterId"></param>
    private void FillObservations(HtmlTableCell aoHtmlTableCell, int aiParameterId, int aiInputTypeId, bool abIsEditableToAll)
    {
        HtmlTable oHtmlTable = SetObservationHeaders(aiParameterId, aiInputTypeId);
        moStaffPerformanceEvaluationBL.ReportingStaffs.ForEach        
            (
                obs =>
                {
                    if (abIsEditableToAll || QueryString["UserId"].ToInt() == obs.ReportingUserId)
                    {
                        HtmlTableRow oHtmlTableRow = new HtmlTableRow { ID = "trObs_" + aiParameterId + "_" + obs.ReportingUserId };

                        string sName = obs.Name;

                        this.AddTableCell(oHtmlTableRow, sName, "ClsMarksCell", "Left", 1, "width:250px");

                        var oObservation = mlstObservations.Where(obsrv => obsrv.ParameterId == aiParameterId && obsrv.ReportingUserId == obs.ReportingUserId).FirstOrDefault();

                        // If login user is not final approver then set observation assignment view else set final approver view.                        
                        if (obs.ReportingUserId == miUserId && (QueryString["IsViewMode"] == null))
                            SetObservationAssignmentView(aiParameterId, obs, oHtmlTableRow, oObservation, aiInputTypeId);
                        else
                            SetFinalApproverView(obs, oHtmlTableRow, oObservation, aiInputTypeId);

                        oHtmlTable.Rows.Add(oHtmlTableRow);
                    }
                });
        aoHtmlTableCell.Controls.Add(oHtmlTable);
    }

    /// <summary>
    /// Tihs method is used to set observation assignment view.
    /// </summary>
    /// <param name="aiParameterId"></param>
    /// <param name="aoReportingStaff"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aoObservation"></param>
    private void SetObservationAssignmentView(int aiParameterId, ReportingStaff aoReportingStaff, HtmlTableRow aoHtmlTableRow, StaffPerformanceObservation aoObservation, int aiInputTypeId)
    {
        if (aiInputTypeId == Constants.FeedbackInputTypes.Grade.ToInt())
        {
            DropDownList oDropDownList = new DropDownList { ID = "cmbGrade_" + aiParameterId + "_" + aoReportingStaff.ReportingUserId, Width = Unit.Pixel(150) };
            oDropDownList.Items.Clear();
            List<PerformanceGrade> lstPerformanceGrades = moStaffPerformanceEvaluationBL.PerformanceGrades.Select(gd => new PerformanceGrade { ShortName = gd.ShortName + " (" + gd.GradeName + ")", GradeId = gd.GradeId }).ToList();
           
            ListSource.FillDropDownList(lstPerformanceGrades, oDropDownList, "ShortName", "GradeId", Constants.S_SELECT);

            if (aoObservation != null)
                oDropDownList.SelectedValue = aoObservation.GradeId.ToString();

            oDropDownList.Enabled = moStaffPerformanceEvaluationBL.ButtonState.CanUserAddComments;

            if (moStaffPerformanceEvaluationBL.StaffPerformanceStatus.Where(user => user.ReportingUserId == miUserId && user.IsPublished).Any())
                oDropDownList.Enabled = false;

            this.AddTableCell(aoHtmlTableRow, string.Empty, "ClsMarksCell", "left", 1, "Text-Align:left", oDropDownList);
        }
        else
        {
            TextBox oTextBox = new TextBox { ID = "txtObservation_" + aiParameterId + "_" + aoReportingStaff.ReportingUserId, Width = Unit.Percentage(100), Height = Unit.Pixel(70), TextMode = TextBoxMode.MultiLine };

            if (aoObservation != null)
                oTextBox.Text = aoObservation.Observation;

            oTextBox.Enabled = moStaffPerformanceEvaluationBL.ButtonState.CanUserAddComments;

            // if login user has published details then disable all fields.
            if (moStaffPerformanceEvaluationBL.StaffPerformanceStatus.Where(user => user.ReportingUserId == miUserId && user.IsPublished).Any())
                oTextBox.Enabled = false;

            if (hidIsLoginUser.Value == Constants.S_YES)
            {
                if (miSchoolId == Constants.SchoolId.PPS.ToInt() && QueryString["Year"].ToInt() >= 51)
                {
                    var iSkillid = moStaffPerformanceEvaluationBL.PerformanceParameters.Where(pp => pp.Id == aiParameterId).Select(pp => pp.SkillId).FirstOrDefault();
                    var skill = moStaffPerformanceEvaluationBL.PerformanceSkills.Where(ps => ps.SkillId == iSkillid).Select(ps => ps.SkillName).FirstOrDefault();
                    if (skill.Trim() == "Remark")
                    {
                        //oTextBox.Text = "-";
                        oTextBox.Enabled = false;
                    }
                }
            }

            this.AddTableCell(aoHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, string.Empty, oTextBox);

            //var sSkillName = (from skill in moStaffPerformanceEvaluationBL.PerformanceSkills
            //                 join parameter in moStaffPerformanceEvaluationBL.PerformanceParameters
            //                 on skill.SkillId equals parameter.SkillId
            //                 where parameter.Id == aiParameterId
            //                 select skill.SkillName).FirstOrDefault();

            //HtmlTableCell oCell = aoHtmlTableRow.Cells[aoHtmlTableRow.Cells.Count - 1];
            //Label oLabel = new Label();
            //oLabel.ID = "lbl_" + aiParameterId;
            //oLabel.Text = "(" + oTextBox.Text.Length.ToString() + ")";
            //oTextBox.Attributes.Add("onchange", "UpdateObservationLength(this,'" + oLabel + "')");
            //oCell.Controls.Add(oLabel);
        }
    }

    /// <summary>
    /// This method is used to set final approver or reporting view.
    /// </summary>
    /// <param name="aoReportingStaff"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aoObservation"></param>
    private void SetFinalApproverView(ReportingStaff aoReportingStaff, HtmlTableRow aoHtmlTableRow, StaffPerformanceObservation aoObservation, int aiInputTypeId)
    {
        Label lblGrade = new Label();
        Label lblObservation = new Label();

        if (aoObservation != null)
        {
            int iReportingUserId = aoReportingStaff.ReportingUserId;
            bool bIsPublished = moStaffPerformanceEvaluationBL.StaffPerformanceStatus.Where(staff => staff.IsPublished && staff.ReportingUserId == aoReportingStaff.ReportingUserId).Any();

            // if login user has published details then disable all fields.
            if (moStaffPerformanceEvaluationBL.StaffPerformanceStatus.Where(user => user.ReportingUserId == iReportingUserId && user.IsPublished).Any())
            {
                if (aiInputTypeId == Constants.FeedbackInputTypes.Grade.ToInt())
                {
                    string sGradeName = moStaffPerformanceEvaluationBL.PerformanceGrades.Where(grd => grd.GradeId == aoObservation.GradeId).Select(grd => grd.ShortName +" (" + grd.GradeName + ")").FirstOrDefault();
                    lblGrade.Text = sGradeName;
                }
                else
                    lblObservation.Text = aoObservation.Observation;
            }
        }

        if (aiInputTypeId == Constants.FeedbackInputTypes.Grade.ToInt())
            this.AddTableCell(aoHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, string.Empty, lblGrade);
        else
            this.AddTableCell(aoHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, string.Empty, lblObservation);
    }

    /// <summary>
    /// This method is used to add attachment.
    /// </summary>
    private void AddAttachment()
    {
        HtmlTable oHtmlTable = new HtmlTable();
        HtmlTableRow oHtmlTableRow2 = new HtmlTableRow();
        this.AddTableCell(oHtmlTableRow2, "Attachment", "ClsProgressGridTestHeader", "Left", 3, "font-weight:bold");
        tblLinks.Rows.Add(oHtmlTableRow2);
        List<ReportingStaff> lstReportingStaff = moStaffPerformanceEvaluationBL.ReportingStaffs;   
        moStaffPerformanceEvaluationBL.ReportingStaffs.ForEach
            (
                obs =>
                {
                    HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                    string sControlState;
                    LinkButton oLinkButton = new LinkButton { ID = "lnkAttachment_" + obs.ReportingUserId};
                    Label oLabel = new Label { ID = "lblName_"  + obs.ReportingUserId, Text=obs.Name };
                    string sClientId = oLinkButton.ClientID;
                    if (obs.AttachmentCount == string.Empty)
                    {
                        obs.AttachmentCount = "0";
                    }
                    if (obs != null)
                        oLinkButton.Text = obs.AttachmentCount + " Files Uploaded";
                    if (miUserId==obs.ReportingUserId)
                        sControlState = hidBtnState.Value;
                    else
                        sControlState = "false";
                    string sQueryString = "UserId=" + QueryString["UserId"].ToInt() + "&DocumentId=0&IsSubmitted=0&DocumentTypeId=" + Constants.DocumentTypes.PerformanceEvaluation.ToInt() + "&SetContolState=" + sControlState + "&AcademicYear=" + QueryString["Year"].ToInt() + "&ReportingUserId=" + obs.ReportingUserId + "&ClientId=" + sClientId;
                    oLinkButton.Attributes.Add("onclick", "window.open('../Payroll/InvestmentDocumentPopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString)+"', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500');return false;");
                    this.AddTableCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, "width:50px");
                    this.AddTableCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, "width:250px", oLabel);
                    this.AddTableCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 1, "width:600px", oLinkButton);
                    tblLinks.Rows.Add(oHtmlTableRow);
                });      
    }
    /// <summary>
    /// This method is used to set observation header.
    /// </summary>
    /// <param name="aiParameterId"></param>
    /// <returns>HtmlTable</returns>
    private HtmlTable SetObservationHeaders(int aiParameterId, int aiInputTypeId)
    {
        HtmlTable oHtmlTable = new HtmlTable { ID = "tblObservations_" + aiParameterId, Width = "100%" };
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();

        this.AddTableCell(oHtmlTableRow, Resources.LocalizedResources.Observer, "ClsProgressGridTestHeader", "Left", 1, "font-weight:bold;width:200px");
        if (aiInputTypeId == Constants.FeedbackInputTypes.Grade.ToInt())
            this.AddTableCell(oHtmlTableRow, Resources.LocalizedResources.Grade, "ClsProgressGridTestHeader", "Left", 1, "font-weight:bold;");
        else
            this.AddTableCell(oHtmlTableRow, Resources.LocalizedResources.Observation, "ClsProgressGridTestHeader", "Left", 1, "font-weight:bold");

        oHtmlTable.Rows.Add(oHtmlTableRow);
        return oHtmlTable;
    }

    /// <summary>
    /// This method is used to set grades.
    /// </summary>
    private void FillGrades()
    {
        HtmlTableRow oHeader = new HtmlTableRow();
        this.AddTableCell(oHeader, Resources.LocalizedResources.KeyToRate, "HeadTxtBWOPadding", "Center", 2);
        tblGrades.Rows.Add(oHeader);

        HtmlTableRow oHeaderNames = new HtmlTableRow();
        this.AddTableCell(oHeaderNames, Resources.LocalizedResources.Grade, "ClsProgressGridTestHeader", "Left", 1, "Width:20%");
        this.AddTableCell(oHeaderNames, Resources.LocalizedResources.Description, "ClsProgressGridTestHeader", "Left");
        tblGrades.Rows.Add(oHeaderNames);

        moStaffPerformanceEvaluationBL.PerformanceGrades.ForEach
            (
                grade =>
                {
                    HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                    this.AddTableCell(oHtmlTableRow, grade.ShortName, "ClsMarksCell", "Left");
                    this.AddTableCell(oHtmlTableRow, grade.Description, "ClsMarksCell", "Left");
                    tblGrades.Rows.Add(oHtmlTableRow);
                });
    }

    /// <summary>
    /// This method is used to set user details.
    /// </summary>
    private void SetUserDetails()
    {
        lblName.Text = moStaffPerformanceEvaluationBL.UserDetails.Name;
        lblDesignation.Text = moStaffPerformanceEvaluationBL.UserDetails.Designation;

        lblEmployeeNo.Text = moStaffPerformanceEvaluationBL.UserDetails.EmployeeNo;
        lblJoiningDate.Text = moStaffPerformanceEvaluationBL.UserDetails.JoiningDate;
        lblJobStatus.Text = moStaffPerformanceEvaluationBL.UserDetails.JobStatus;
        lblServiceLength.Text = moStaffPerformanceEvaluationBL.UserDetails.ServiceLength;

        lblFormFor.Text = moStaffPerformanceEvaluationBL.UserDetails.FormFor;
        //lblSubjectTaught.Text = moStaffPerformanceEvaluationBL.UserDetails.Subjects;
      //  lblStandardTaught.Text = moStaffPerformanceEvaluationBL.UserDetails.Standards;////comment line
        lblAcademicYear.Text = moStaffPerformanceEvaluationBL.UserDetails.AcademicYear;

        txtLastIncrementDate.Text = moStaffPerformanceEvaluationBL.UserDetails.LastIncrementDate;
        txtEffectiveFromDate.Text = moStaffPerformanceEvaluationBL.UserDetails.EffectiveFromDate;

        lblAddress.Text = moStaffPerformanceEvaluationBL.UserDetails.Address;                         ////new line add
        lblHighestEducation.Text = moStaffPerformanceEvaluationBL.UserDetails.HighestEducation;       ////new line add

        if (moStaffPerformanceEvaluationBL.UserDetails.Standards != string.Empty)
            txtClassestaught.Text = moStaffPerformanceEvaluationBL.UserDetails.Standards;

        if (moStaffPerformanceEvaluationBL.UserDetails.Subjects != string.Empty)
            txtTeachersubjects.Text = moStaffPerformanceEvaluationBL.UserDetails.Subjects;
        
        if (IsFinalApprover)
        {
            if (moStaffPerformanceEvaluationBL.ButtonState.IsPublished)
            {
                txtEffectiveFromDate.Enabled = false;
                txtLastIncrementDate.Enabled = false;
                cal_EffectiveFromDate.Enabled = false;
                cal_LastIncrementDate.Enabled = false;
            }
            else
            {
                txtEffectiveFromDate.Enabled = true;
                txtLastIncrementDate.Enabled = true;
                cal_EffectiveFromDate.Enabled = true;
                cal_LastIncrementDate.Enabled = true;
            }

            trEffectiveDate.Visible = true;
          
            txtLastIncrementDate.Visible = true;
            cal_LastIncrementDate.Visible = true;
            txtLastIncrementDate.Text = moStaffPerformanceEvaluationBL.UserDetails.LastIncrementDate;
        }
        else
        {
            trEffectiveDate.Visible = false;
            if (moStaffPerformanceEvaluationBL.UserDetails.LastIncrementDate != string.Empty)
                lblLastIncrementDate.Text = moStaffPerformanceEvaluationBL.UserDetails.LastIncrementDate;
            else
                lblLastIncrementDate.Text = "-";

            txtLastIncrementDate.Visible = false;
            cal_LastIncrementDate.Visible = false;
            lblLastIncrementDate.Visible = true;
        }

        if (moStaffPerformanceEvaluationBL.UserDetails.UserRoleId == Constants.UserRoles.Teacher.ToInt())
        {
            trStandards.Visible = true;
            //trSubjects.Visible = true;
        }
        else
        {
             trStandards.Visible = false;
            trSubjects.Visible = false;
        }

        hidIsFinalApprover.Value = (IsFinalApprover ? Constants.S_YES : Constants.S_NO);
    }

    /// <summary>
    /// This method is used to set school details.
    /// </summary>
    private void SetSchoolDetails()
    {
        lblSchoolName.Text = moStaffPerformanceEvaluationBL.SchoolEntity.SchoolName;
        lblOrgName.Text = moStaffPerformanceEvaluationBL.SchoolEntity.OrganizationName;
        lblSchoolAddress.Text = moStaffPerformanceEvaluationBL.SchoolEntity.Address;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnBack, btnPublish, btnSave, btnViewReport, btnClose, btnBackUp, btnSubmit, btnContinue, btnClosePopup });

        string sAssignmentPage = CommonUtility.EncryptQuerystring("Year=" + QueryString["Year"].ToString() + "&Status=" + QueryString["Status"].ToString());
        btnBack.PostBackUrl = "PerformanceGradeAssignmentUI.aspx?" + sAssignmentPage;
        btnBackUp.PostBackUrl = "PerformanceGradeAssignmentUI.aspx?" + sAssignmentPage;

        string sQueryString = CommonUtility.EncryptQuerystring("UserId=" + QueryString["UserId"] + "&Year=" + QueryString["Year"] + "&IsViewMode=Y&Status=" + QueryString["Status"].ToString());
        btnViewReport.Attributes.Add("onclick", "OpenReport('" + sQueryString + "'); return false;");
        btnClose.Attributes.Add("onclick", "window.close(); return false;");
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;

        btnContinue.Attributes.Add("onclick", "if(!ValidateReason()) return false;");

        if (QueryString["IsViewMode"] != null && QueryString["IsViewMode"].ToString() == Constants.S_YES)
        {
            tblGrades.Width = "90%";
            tblParameter.Width = "90%";
            tblSchoolDetails.Width = "90%";
            tblUserDetails.Width = "90%";
            trButtonClose.Visible = true;
            trButtons.Visible = false;
            hidIsViewMode.Value = Constants.S_YES;
        }
        else
        {
            trButtonClose.Visible = false;
            trButtons.Visible = true;
            hidIsViewMode.Value = Constants.S_NO;
        }
    }

    /// <summary>
    /// This method is used to save performance evaluation Details.
    /// </summary>
    private void Save()
    {
        List<StaffPerformanceObservation> lstObservations = new List<StaffPerformanceObservation>();
        foreach (HtmlTableRow oHtmlTableRow in tblParameter.Rows)
        {
            if (oHtmlTableRow.ID != null)
            {
                string sParameterId = oHtmlTableRow.ID.Substring(oHtmlTableRow.ID.IndexOf("_") + 1);
                HtmlTable oHtmlTable = oHtmlTableRow.FindControl("tblObservations_" + sParameterId) as HtmlTable;
                if (oHtmlTable != null)
                {
                    foreach (HtmlTableRow tr in oHtmlTable.Rows)
                    {
                        if (tr.ID != null)
                        {
                            string iReportingUserId = tr.ID.Substring(tr.ID.IndexOf("_"));
                            DropDownList cmbGrade = tr.FindControl("cmbGrade" + iReportingUserId) as DropDownList;
                            TextBox txtObservation = tr.FindControl("txtObservation" + iReportingUserId) as TextBox;


                            StaffPerformanceObservation oStaffPerformanceObservation = new StaffPerformanceObservation();
                            oStaffPerformanceObservation.ParameterId = sParameterId.ToInt();
                            oStaffPerformanceObservation.GradeId = 0;
                            oStaffPerformanceObservation.Observation = string.Empty;
                            if (cmbGrade != null)
                            {
                                if (cmbGrade.SelectedValue != Constants.S_ZERO)
                                    oStaffPerformanceObservation.GradeId = cmbGrade.SelectedValue.ToInt();                                    
                            }

                            if (txtObservation != null)
                                oStaffPerformanceObservation.Observation = txtObservation.Text.Trim();                                

                            if (cmbGrade != null || txtObservation != null)
                                lstObservations.Add(oStaffPerformanceObservation);
                        }
                    }
                }
            }
        }

        if (lstObservations.Count > 0)
        {
            string sXml = base.GenerateXml(lstObservations);
            int iYear = QueryString["Year"].ToInt();          
            moStaffPerformanceEvaluationBL.Save(miUserId, iYear, sXml, txtClassestaught.Text.Trim(), txtTeachersubjects.Text.Trim());
        }
    }

    /// <summary>
    /// This method is used to refresh field values according to selected culture.
    /// </summary>
    private void SetFields()
    {
        hidValGradeSelected.Value = Resources.LocalizedResources.valGradeSelecttion;
        hidvalBlankObservation.Value = Resources.LocalizedResources.valBlankObservation;
        hidvalObservationLength.Value = Resources.LocalizedResources.valObservationMaxLength;
        hidvalActionSaveandPublish.Value = Resources.LocalizedResources.msgActionSaveandPublish;
        hidvalActionSaveandSubmit.Value = Resources.LocalizedResources.msgActionSaveandSubmit;
        hidvalActionUnPublish.Value = Resources.LocalizedResources.msgActionUnPublish;
    }   
    #endregion

}