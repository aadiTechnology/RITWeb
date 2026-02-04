/*File Name - LessonPlanApprovalUI.aspx.cs
 * Created By - Sachin
 * Created Date - 1 Jun 2015
 * Description - This class is used to manage lesson plan details.
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using LessonPlanEntities;
using Utility;
using System.Web.Script.Serialization;

public partial class LessonPlanApprovalUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Lesson Plan saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Lesson Plan submitted successfully !!!";
    private const string S_APPROVE_MESSAGE = "Lesson Plan approved successfully !!!";
    private const string S_SAVE_COMMENT_MESSAGE = "Comment saved successfully !!!";
    private const string S_UPDATE_DATE_MESSAGE = "Date updated successfully !!!";
    private const string S_VALID_DATE = "End Date should not be less than Start Date.";

    #endregion

    #region Data Member(s)

    private LessonPlanDetailsBL moLessonPlanDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moLessonPlanDetailsBL = new LessonPlanDetailsBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnApprove.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnApproveUpper.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnSaveUpper.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    timer.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnSaveComment.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnSaveCommentUpper.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                {
                    ReadQueryString();
                    SetDates();
                    FillLessonDetails();
                }
            }
        }
        catch (Exception ex)
        {
            string sMessage = "Start Date = '" + hidStartDate.Value + "'";
            if (btnApprove.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Approve";
            else if (btnApproveUpper.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Approve";
            else if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Save";
            else if (btnSaveUpper.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Save";
            else if (timer.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Timer";
            else if (btnSaveComment.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Save Comment";
            else if (btnSaveCommentUpper.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                sMessage = "Start Date = '" + hidStartDate.Value + "', Button = Save Comment";

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), sMessage);
        }
    }

    /// <summary>
    /// This event is used to show / save lesson plan details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReadQueryString();
                SetDates();
                FillLessonDetails();
                SetJavascriptAttribues();
                SetTranslatorLinks();
                timer.Enabled = true;
                DisableValidators();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to divert back to lesson list screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMaster = this.Master as MasterPage;
            oMaster.RedirectToNextPage("LessonPlanUI.aspx?" + CommonUtility.EncryptQuerystring("UserId=" + hidUserId.Value));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save lesson plan details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            Save();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            UpdateDateFields();
            FillLessonDetails();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
        }
    }

    /// <summary>
    /// This event is used to submit / approve lesson plan.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            int iUserId = hidUserId.Value.ToInt();
            moLessonPlanDetailsBL.Submit(iUserId, miUserId, hidOldStartDate.Value.ToDateTime(), hidOldEndDate.Value.ToDateTime());

            if (hidUserId.Value.ToInt() == miUserId)
            {
                base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
                base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            }
            else
            {
                base.DisplayMessage(S_APPROVE_MESSAGE, false, tdMessage);
                base.DisplayMessage(S_APPROVE_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            }

            FillLessonDetails();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
        }
    }

    /// <summary>
    /// This event is used to save comments of approver.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveComment_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            SaveComment();
            base.DisplayMessage(S_SAVE_COMMENT_MESSAGE, false, tdMessage);
            base.DisplayMessage(S_SAVE_COMMENT_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            UpdateDateFields();
            FillLessonDetails();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
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
            if (btnSave.Enabled || btnSaveUpper.Enabled)
            {
                timer.Enabled = false;
                Save();
                UpdateDateFields();
                timer.Enabled = true;
            }
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
        }
    }

    /// <summary>
    /// This event is used to save lesson plan details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveUpper_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            Save();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            UpdateDateFields();
            FillLessonDetails();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
        }
    }

    /// <summary>
    /// This event is used to approve lessonplan or submit lesson plan.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApproveUpper_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            int iUserId = hidUserId.Value.ToInt();
            moLessonPlanDetailsBL.Submit(iUserId, miUserId, hidOldStartDate.Value.ToDateTime(), hidOldEndDate.Value.ToDateTime());

            if (hidUserId.Value.ToInt() == miUserId)
            {
                base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
                base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            }
            else
            {
                base.DisplayMessage(S_APPROVE_MESSAGE, false, tdMessage);
                base.DisplayMessage(S_APPROVE_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            }

            FillLessonDetails();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
        }
    }

    /// <summary>
    /// This event is used to save lesson plan comments.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveCommentUpper_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            SaveComment();
            base.DisplayMessage(S_SAVE_COMMENT_MESSAGE, false, tdMessage);
            base.DisplayMessage(S_SAVE_COMMENT_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            UpdateDateFields();
            FillLessonDetails();            
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            timer.Enabled = true;
        }
    }    

    /// <summary>
    /// This event is used to Update lesson plan Date for full Access User.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveDate_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEndDate.Text.ToDateTime() <= txtStartDate.Text.ToDateTime())
            {
                base.DisplayMessage(S_VALID_DATE, true, tdMessageTop, "lblErrMessageTop");
            }
            else
            {
                int iUserId = hidUserId.Value.ToInt();
                DateTime dt = hidNewStartDate.Value.ToDateTime();
                moLessonPlanDetailsBL.UpdateDate(iUserId, miUserId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), hidNewStartDate.Value.ToDateTime(), hidNewEndDate.Value.ToDateTime());

                hidStartDate.Value = txtStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                hidNewStartDate.Value = txtStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                hidOldStartDate.Value = txtStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

                hidEndDate.Value = txtEndDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                hidNewEndDate.Value = txtEndDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                hidOldEndDate.Value = txtEndDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

                base.DisplayMessage(S_UPDATE_DATE_MESSAGE, false, tdMessageTop, "lblErrMessageTop");
            }
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessageTop, "lblErrMessageTop");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to read query string and set value to hidden fields.
    /// </summary>
    private void ReadQueryString()
    {
        if (IsPostBack)
        {
            hidUserId.Value = Convert.ToString(Request.Params[hidUserId.ClientID.Replace("_", "$")]);
            hidStartDate.Value = Convert.ToString(Request.Params[hidStartDate.ClientID.Replace("_", "$")]);
            hidEndDate.Value = Convert.ToString(Request.Params[hidEndDate.ClientID.Replace("_", "$")]);
            hidOldStartDate.Value = Convert.ToString(Request.Params[hidOldStartDate.ClientID.Replace("_", "$")]);
            hidOldEndDate.Value = Convert.ToString(Request.Params[hidOldEndDate.ClientID.Replace("_", "$")]);
        }
        else
        {
            hidUserId.Value = QueryString["UserId"];
            hidStartDate.Value = QueryString["StartDate"];
            hidEndDate.Value = QueryString["EndDate"];
            hidOldStartDate.Value = QueryString["StartDate"];
            hidOldEndDate.Value = QueryString["EndDate"];
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttribues()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave, btnSaveUpper, btnApprove, btnApproveUpper, btnSaveComment, btnSaveCommentUpper });
        btnApprove.Attributes.Add("onclick", "if(!ConfirmApprove()) return false;");
        btnApproveUpper.Attributes.Add("onclick", "if(!ConfirmApprove()) return false;");
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        if (hidUserId.Value.ToInt() == miUserId)
        {
            btnApprove.Text = "Submit";
            btnApproveUpper.Text = "Submit";
            btnSaveComment.Visible = false;
            btnSaveCommentUpper.Visible = false;
        }
        else
        {
            btnSaveComment.Visible = true;
            btnSaveCommentUpper.Visible = true;
        }
        btnApprove.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
        btnApproveUpper.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
        btnSave.Attributes.Add("onclick", "ClearMessage()");
        btnSaveUpper.Attributes.Add("onclick", "ClearMessage()");
        hidAcademicYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToDateTime().ToString(Constants.S_DATE_FORMAT);
        hidAcademicYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToDateTime().ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to fill lesson plan details.
    /// </summary>
    private void FillLessonDetails()
    {
        int iUserId = hidUserId.Value.ToInt();
        DateTime dtStartDate = hidStartDate.Value.ToDateTime();
        DateTime dtEndDate = hidEndDate.Value.ToDateTime();
        bool bIsNewMode = (hidOldStartDate.Value.ToDateTime() == DateTime.MinValue ? true : false);

        moLessonPlanDetailsBL.GetAll(iUserId, miUserId, dtStartDate, dtEndDate, bIsNewMode);

        tblLessons.Rows.Clear();

        hidStandardDivIds.Value = moLessonPlanDetailsBL.LessonPlanStandard.StandardDivisionIds;

        FillBasicDetails();
        FillParameters();
        SetButtonState();
        FillComments();
        CheckHasFullAccess();

        if (moLessonPlanDetailsBL.LessonPlanPhrases.Count > 0)
        {
            FillPhrases();
            trPhrases.Visible = true;
            trWords.Visible = true;
            trWordSearch.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to fill lesson plan phrses.
    /// </summary>
    private void FillPhrases()
    {
        List<string> lstWordTitles =  moLessonPlanDetailsBL.LessonPlanPhrases.Where(lpp => lpp.IsPhrase == false).Select(lpp => lpp.Title).OrderBy(lpp => lpp).ToList();
        txtWords.Text = string.Join(", ", lstWordTitles);
                 
        var jsSerializer = new JavaScriptSerializer();
		hidWords.Value = jsSerializer.Serialize(lstWordTitles);
        
        List<string> lstPhraseTitles = moLessonPlanDetailsBL.LessonPlanPhrases.Where(lpp => lpp.IsPhrase == true).Select(lpp => lpp.Title).OrderBy(lpp => lpp).ToList();
        txtPhrases.Text = string.Join(", ", lstPhraseTitles);

        hidPhrases.Value = jsSerializer.Serialize(lstPhraseTitles);
    }

    /// <summary>
    /// This method is used to fill comments.
    /// </summary>
    private void FillComments()
    {
        tblComments.Rows.Clear();
        moLessonPlanDetailsBL.LessonPlanReportingUsers.OrderBy(rs => rs.ApprovalSortOrder).ToList().ForEach
            (
                usr =>
                {
                    HtmlTableRow trHeader = new HtmlTableRow();
                    AddCell(trHeader, "Name", "ClsProgressGridTestHeader", "Left", 1, "Width:10%");
                    AddCell(trHeader, usr.ReportingUserName, "ClsProgressGridTestHeader", "Left", 1, "Width:40%;font-weight:Normal;");

                    if (usr.ReportingUserId == hidUserId.Value.ToInt())
                        AddCell(trHeader, "Submitted On", "ClsProgressGridTestHeader", "Left", 1, "Width:10%");
                    else
                        AddCell(trHeader, "Approved On", "ClsProgressGridTestHeader", "Left", 1, "Width:10%");

                    var oComment = moLessonPlanDetailsBL.ApproverComments.Where(cmt => cmt.ReportingUserId == usr.ReportingUserId).FirstOrDefault();

                    string sUpdatedDate = "-";
                    if (oComment != null && oComment.IsPublished)
                        sUpdatedDate = oComment.UpdateDate.ToString(Constants.S_DATE_FORMAT + " hh:MM tt");

                    AddCell(trHeader, sUpdatedDate, "ClsProgressGridTestHeader", "Left", 1, "Width:40%;font-weight:Normal;");
                    tblComments.Rows.Add(trHeader);

                    if (usr.ReportingUserId != hidUserId.Value.ToInt())
                    {
                        HtmlTableRow trComment = new HtmlTableRow();

                        if (usr.ReportingUserId == miUserId)
                        {
                            TextBox txtComment = new TextBox();
                            txtComment.ID = "txtComment";
                            txtComment.Width = Unit.Percentage(100);
                            txtComment.TextMode = TextBoxMode.MultiLine;
                            txtComment.Height = Unit.Pixel(50);
                            txtComment.Text = (oComment != null ? oComment.Comment : string.Empty);
                            txtComment.CssClass = "MidTxt";

                            if (!moLessonPlanDetailsBL.ButtonState.EnableSubmitButton && !moLessonPlanDetailsBL.ButtonState.EnableSaveButton)
                            {
                                txtComment.ReadOnly = true;
                                txtComment.BackColor = System.Drawing.Color.LightGray;

                            }

                            AddCell(trComment, string.Empty, "ClsMarksCell", "Left", 4, string.Empty, txtComment);
                        }
                        else
                        {
                            Label oLabel = new Label();
                            oLabel.Text = (oComment == null || oComment.Comment == string.Empty ? "-" : oComment.Comment);
                            AddCell(trComment, string.Empty, "ClsMarksCell", "Left", 4, string.Empty, oLabel);
                        }

                        tblComments.Rows.Add(trComment);
                    }
                }

            );
    }

    /// <summary>
    /// This method is used to set date fields.
    /// </summary>
    private void SetDates()
    {
        if (hidStartDate.Value.ToDateTime() == DateTime.MinValue)
        {
            int iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Date.Year, DateTime.Now.Date.Month);
            DateTime dtStartDate = DateTime.Now.Date;
            DateTime dtEndDate = DateTime.Now.Date;
            Constants.LessonPlanConfigTypes oLessonPlanConfigTypes = (Constants.LessonPlanConfigTypes)Settings.LessonPlanConfigTypeId;
            switch (oLessonPlanConfigTypes)
            {
                case Constants.LessonPlanConfigTypes.Day:
                    dtStartDate = DateTime.Now.Date;
                    dtEndDate = DateTime.Now.Date;
                    break;
                case Constants.LessonPlanConfigTypes.Month:
                    dtStartDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
                    dtEndDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, iDaysInMonth);
                    break;
                case Constants.LessonPlanConfigTypes.FortNight:
                    if (DateTime.Now.Date.Day <= 15)
                    {
                        dtStartDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
                        dtEndDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 15);
                    }
                    else
                    {
                        dtStartDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 16);
                        dtEndDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, iDaysInMonth);
                    }

                    break;
                case Constants.LessonPlanConfigTypes.Week:
                    dtStartDate = DateTime.Now.AddDays((DateTime.Now.Date.DayOfWeek.ToInt() * -1) + 1);
                    int iLastDayOfWeek = moLessonPlanDetailsBL.GetLastDayOfWeek();
                    dtEndDate = DateTime.Now.Date.AddDays(iLastDayOfWeek - DateTime.Now.Date.DayOfWeek.ToInt());
                    break;
            }

            SetDateVisibility(true);
            txtStartDate.Text = dtStartDate.ToString(Constants.S_DATE_FORMAT);
            txtEndDate.Text = dtEndDate.ToString(Constants.S_DATE_FORMAT);
            hidStartDate.Value = dtStartDate.ToString(Constants.S_DATE_FORMAT);
            hidEndDate.Value = dtEndDate.ToString(Constants.S_DATE_FORMAT);
        }
        else
        {
            txtStartDate.Text = hidStartDate.Value;
            txtEndDate.Text = hidEndDate.Value;
        }
    }

    /// <summary>
    /// This method is used to set controls visibility.
    /// </summary>
    /// <param name="abStatus"></param>
    private void SetDateVisibility(bool abStatus)
    {
        tdStartDate.Visible = abStatus;
        tdEndDate.Visible = abStatus;
        tdLegendStartDate.Visible = !abStatus;
        tdLegendEndDate.Visible = !abStatus;
    }

    /// <summary>
    /// This method is used to set bsic details.
    /// </summary>
    private void FillBasicDetails()
    {
        lblTeacherName.Text = moLessonPlanDetailsBL.BasicDetails.TeacherName;
        lblStartDate.Text = hidStartDate.Value;
        lblEndDate.Text = hidEndDate.Value;
    }

    /// <summary>
    /// This method is used to fill parameters.
    /// </summary>
    private void FillParameters()
    {
        moLessonPlanDetailsBL.PlanConfigs.ForEach(
        cnf =>
        {
            string sIdPrefix = cnf.StdDivId + "_" + cnf.SubjectId + "_";

            HtmlTableRow trSubject = new HtmlTableRow();
            AddCell(trSubject, string.Empty, "ClsClassNameHeader", "Left", 1);
            AddCell(trSubject, cnf.ClassName + " ( " + cnf.SubjectName + " )", "ClsClassNameHeader", "Left", 2, "Width:150px;font-size:15px;");

            HtmlTable tbl = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
                        
            TextBox txtSubjectStartDate = new TextBox();
            txtSubjectStartDate.ID = "txtSubjectStartDate_" + cnf.StdDivId + "_" + cnf.SubjectId;
            txtSubjectStartDate.Attributes.Add("placeholder", "Start Date (dd-MMM-yyyy)");
            txtSubjectStartDate.Width = Unit.Pixel(200);
            AddCell(tr, "", "ClsClassNameHeader", "Center", 1, "", txtSubjectStartDate);
            
            TextBox txtSubjectEndDate = new TextBox();
            txtSubjectEndDate.ID =  "txtSubjectEndDate_" + cnf.StdDivId + "_" + cnf.SubjectId;
            txtSubjectEndDate.Attributes.Add("placeholder", "End Date (dd-MMM-yyyy)");
            txtSubjectEndDate.Width = Unit.Pixel(200);
            AddCell(tr, "", "ClsClassNameHeader", "Center", 1, "", txtSubjectEndDate);

            if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
            {
                txtSubjectStartDate.Visible = false;
                txtSubjectEndDate.Visible = false;
            }
            else
            {
                txtSubjectStartDate.Visible = true;
                txtSubjectEndDate.Visible = true;
            }

            HiddenField hf = new HiddenField();
            hf.ID = "hid_" + cnf.StdDivId + "_" + cnf.SubjectId;
            hf.Value = cnf.ClassName + " ( " + cnf.SubjectName + " )";
            AddCell(tr, "", "ClsClassNameHeader", "Center", 1, "", hf);

            LessonPlanDetails oLessonPlanDetails = moLessonPlanDetailsBL.LessonPlanDetails.Where(lpd => lpd.StdDivId == cnf.StdDivId && lpd.SubjectId == cnf.SubjectId).FirstOrDefault();
            if (oLessonPlanDetails != null)
            {
                if (oLessonPlanDetails.SubjectStartDate != string.Empty)
                    txtSubjectStartDate.Text = oLessonPlanDetails.SubjectStartDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    txtSubjectStartDate.Text = string.Empty;

                if (oLessonPlanDetails.SubjectEndDate != string.Empty)
                    txtSubjectEndDate.Text = oLessonPlanDetails.SubjectEndDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    txtSubjectEndDate.Text = string.Empty;
            }

            if (!moLessonPlanDetailsBL.ButtonState.EnableSaveButton)
            {
                txtSubjectStartDate.Enabled = false;
                txtSubjectEndDate.Enabled = false;
            }
            else
            {
                txtSubjectStartDate.Enabled = true;
                txtSubjectEndDate.Enabled = true;
            }

            tbl.Rows.Add(tr);

            AddCell(trSubject, string.Empty, "ClsClassNameHeader", "right", 1,"", tbl);

            tblLessons.Rows.Add(trSubject);

            HtmlTableRow trHeader = new HtmlTableRow();
            AddCell(trHeader, Resources.LocalizedResources.SrNo, "ClsProgressGridTestHeader", "Center", 1, "Width:50px");
            AddCell(trHeader, "Parameter", "ClsProgressGridTestHeader", "Center", 3);
            tblLessons.Rows.Add(trHeader);

            int iSrNo = 0;
            int iSubSrNo = 1;

            moLessonPlanDetailsBL.Parameters.Where(prm => prm.LessonPlanCategoryId == cnf.LessonPlanCategoryId && (prm.SubjectCategoryId == 1 || prm.SubjectCategoryId == cnf.SubjectCategoryId))
            .OrderBy(prm => prm.SortOrder)
            .ToList()
            .ForEach
             (
                    parameter =>
                    {

                       // LessonPlanParameters oLessonPlanParameters = moLessonPlanDetailsBL.Parameters.Where(prm => prm.ParentParameterId == parameter.Id).FirstOrDefault();

                       //if (oLessonPlanParameters == null)
                       //{
                           HtmlTableRow oHtmlTableRow = new HtmlTableRow { ID = "tr_" + sIdPrefix + parameter.Id };
                           //Label oLabel = new Label { ID = "lblParameter_" + sIdPrefix + parameter.Id, Text = (iSrNo).ToString() };
                           Label oLabel = new Label { ID = "lblParameter_" + sIdPrefix + parameter.Id };

                           AddCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Center", 1, string.Empty, oLabel);

                           if (moLessonPlanDetailsBL.Parameters.Any(prm => prm.ParentParameterId != 0 && prm.Id == parameter.Id))
                           {
                               AddCell(oHtmlTableRow, iSrNo.ToString() + "." + (iSubSrNo++) + ") " + parameter.Title, "ClsMarksCell", "left", 3, "font-weight:bold;");
                               oLabel.Text = string.Empty;
                           }
                           else
                           {
                               iSrNo++;

                               oLabel.Text = iSrNo.ToString();
                               AddCell(oHtmlTableRow, parameter.Title, "ClsMarksCell", "left", 3, "font-weight:bold");                               
                           }

                           tblLessons.Rows.Add(oHtmlTableRow);

                           HtmlTableRow trObservation = new HtmlTableRow();
                           AddCell(trObservation, string.Empty, "ClsMarksCell");

                           FillObservations(parameter.Id, ref trObservation, sIdPrefix, cnf);

                           tblLessons.Rows.Add(trObservation);
                      // }
                       //else
                       //{
                       //    HtmlTableRow oHtmlTableRow = new HtmlTableRow();

                       //    iSubSrNo = 1;
                       //    Label oLabel = new Label { ID = "lblParameter_" + sIdPrefix + parameter.Id, Text = (++iSrNo).ToString() };
                       //    AddCell(oHtmlTableRow, string.Empty, "ClsMarksCell", "Center", 1, string.Empty, oLabel);
                                                      
                       //    AddCell(oHtmlTableRow, parameter.Title, "ClsMarksCell", "left", 3, "font-weight:bold");
                       //    tblLessons.Rows.Add(oHtmlTableRow);
                       //}
                    });            

            if (moLessonPlanDetailsBL.ButtonState.EnableSaveButton && miUserId == hidUserId.Value.ToInt())
            {
                if (moLessonPlanDetailsBL.PlanConfigs.Any(sb => sb.StandardId == cnf.StandardId && sb.StdDivId != cnf.StdDivId && sb.SubjectId == cnf.SubjectId))
                {
                    HtmlTableRow trCopyData = new HtmlTableRow();
                    LinkButton lnkCopy = new LinkButton();
                    lnkCopy.ID = "lnk_" + cnf.StdDivId + "_" + cnf.SubjectId;
                    lnkCopy.Font.Bold = true;
                    lnkCopy.Style.Add("padding-right", "10px");                    
                    lnkCopy.Font.Size = 11;
                    lnkCopy.Font.Underline = true;
                    lnkCopy.Text = "Copy to other classes";                    
                    lnkCopy.Attributes.Add("onclick", "CopyToOtherClass(this); return false;");
                    trCopyData.Height = "30px";
                    AddCell(trCopyData, string.Empty, "", "right", 2, string.Empty, lnkCopy);
                    tblLessons.Rows.Add(trCopyData);
                }
                else
                {
                    HtmlTableRow trEmptyRow = new HtmlTableRow();
                    AddCell(trEmptyRow, string.Empty, string.Empty, "Center", 4, "height:10px;");
                    tblLessons.Rows.Add(trEmptyRow);
                }
            }
            else
            {
                HtmlTableRow trEmptyRow = new HtmlTableRow();
                AddCell(trEmptyRow, string.Empty, string.Empty, "Center", 4, "height:10px;");
                tblLessons.Rows.Add(trEmptyRow);
            }
        });
    }

    /// <summary>
    /// This method is used to fill observations.
    /// </summary>
    /// <param name="aiParameterId"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asPrefix"></param>
    /// <param name="oLessonPlanConfig"></param>
    private void FillObservations(int aiParameterId, ref HtmlTableRow aoHtmlTableRow, string asPrefix, LessonPlanConfig oLessonPlanConfig)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.ColSpan = 3;
        HtmlTable oHtmlTable = SetObservationHeaders(aiParameterId, asPrefix);
        moLessonPlanDetailsBL.LessonPlanReportingUsers.ForEach
            (
                obs =>
                {
                    if (QueryString["UserId"].ToInt() == obs.ReportingUserId)
                    {
                        HtmlTableRow trComment = new HtmlTableRow { ID = "trCom_" + asPrefix + aiParameterId + "_" + obs.ReportingUserId };

                        var oObservation = moLessonPlanDetailsBL.LessonPlanDetails.Where(lp => lp.ParameterId == aiParameterId && lp.ReportingUserId == obs.ReportingUserId &&
                            lp.StdDivId == oLessonPlanConfig.StdDivId && lp.SubjectId == oLessonPlanConfig.SubjectId).FirstOrDefault();

                        SetObservationAssignmentView(aiParameterId, obs, trComment, oObservation, asPrefix, oLessonPlanConfig);

                        oHtmlTable.Rows.Add(trComment);
                    }
                });

        oHtmlTableCell.Controls.Add(oHtmlTable);
        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }

    /// <summary>
    /// This method is used to set observation header.
    /// </summary>
    /// <param name="aiParameterId"></param>
    /// <returns>HtmlTable</returns>
    private HtmlTable SetObservationHeaders(int aiParameterId, string asIdPrefix)
    {
        HtmlTable oHtmlTable = new HtmlTable { ID = "tblComments_" + asIdPrefix + aiParameterId, Width = "100%" };
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        return oHtmlTable;
    }

    /// <summary>
    /// Tihs method is used to set observation assignment view.
    /// </summary>
    /// <param name="aiParameterId"></param>
    /// <param name="aoReportingStaff"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aoObservation"></param>
    private void SetObservationAssignmentView(int aiParameterId, LessonPlanReportingConfig aoReportingStaff, HtmlTableRow aoHtmlTableRow, LessonPlanDetails aoObservation, string asPrefix, LessonPlanConfig aoLessonPlanConfig)
    {
        if (hidUserId.Value.ToInt() == miUserId)
        {
            TextBox oTextBox = new TextBox { ID = "txtComment_" + asPrefix + aiParameterId + "_" + aoReportingStaff.ReportingUserId, Width = Unit.Percentage(100), Height = Unit.Pixel(70), TextMode = TextBoxMode.MultiLine };

            if (aoObservation != null)
                oTextBox.Text = aoObservation.Comment;
            else
                oTextBox.Text = string.Empty;

            oTextBox.Font.Bold = false;

            if (!moLessonPlanDetailsBL.ButtonState.EnableSaveButton)
            {
                oTextBox.ReadOnly = true;
                oTextBox.BackColor = System.Drawing.Color.LightGray;
                oTextBox.Font.Bold = true;
            }

            if (hidUserId.Value != miUserId.ToString())
            {
                oTextBox.Font.Bold = true;
                oTextBox.ReadOnly = true;
                oTextBox.BackColor = System.Drawing.Color.LightGray;
            }

            HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = string.Empty, Align = "left", ColSpan = 2 };

            if (moLessonPlanDetailsBL.Parameters.Any(prm => prm.ParentParameterId != 0 && prm.Id == aiParameterId))
            {                
                oTextBox.Width = Unit.Percentage(98);                
                Label lbl = new Label { Text = "", Width = Unit.Percentage(2) };
                oHtmlTableCell.Controls.Add(lbl);

            }

            
            oHtmlTableCell.Attributes.Add("class", "ClsMarksCell");
            oHtmlTableCell.Controls.Add(oTextBox);

            oHtmlTableCell.Style.Add("Padding-Left", "5pt");
            
            aoHtmlTableRow.Cells.Add(oHtmlTableCell);
        }
        else
        {
            Label oLabel = new Label();

            if (aoObservation != null && aoObservation.Comment.Trim() != string.Empty)
                oLabel.Text = aoObservation.Comment;
            else
                oLabel.Text = "--";

            AddCell(aoHtmlTableRow, string.Empty, "ClsMarksCell", "Left", 2, string.Empty, oLabel);
        }
    }

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        btnApprove.Enabled = false;
        btnApproveUpper.Enabled = false;
        btnSave.Enabled = false;
        btnSaveUpper.Enabled = false;
        btnSaveComment.Enabled = false;
        btnSaveCommentUpper.Enabled = false;
        if (moLessonPlanDetailsBL.ButtonState.EnableSaveButton)
        {
            btnSave.Enabled = true;
            btnSaveUpper.Enabled = true;
            if (moLessonPlanDetailsBL.ButtonState.EnableSubmitButton)
            {
                btnApprove.Enabled = true;
                btnApproveUpper.Enabled = true;
            }
        }

        if (hidUserId.Value.ToInt() != miUserId)
        {
            btnSave.Visible = false;
            btnSaveUpper.Visible = false;
            if (moLessonPlanDetailsBL.ButtonState.EnableSubmitButton)
            {
                btnApprove.Enabled = true;
                btnApproveUpper.Enabled = true;
                btnSaveComment.Visible = true;
                btnSaveComment.Enabled = true;
                btnSaveCommentUpper.Visible = true;
                btnSaveCommentUpper.Enabled = true;
            }

            if (!moLessonPlanDetailsBL.ButtonState.EnableSaveButton && !moLessonPlanDetailsBL.ButtonState.EnableSubmitButton)
                SetDateVisibility(false);
        }
        else
        {
            if (!moLessonPlanDetailsBL.ButtonState.EnableSaveButton)
                SetDateVisibility(false);
        }

        hidIsReportingUser.Value = Constants.S_ONE;
        if (hidUserId.Value.ToInt() != miUserId)
            hidIsReportingUser.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to set visibility of fields.
    /// </summary>
    /// <param name="abStatus"></param>
    private void SetVisibility(bool abStatus)
    {
        btnSave.Enabled = abStatus;
        btnSaveUpper.Enabled = abStatus;
        txtStartDate.Enabled = abStatus;
        txtEndDate.Enabled = abStatus;
        calStartDate.Enabled = abStatus;
        calEndDate.Enabled = abStatus;
    }

    /// <summary>
    /// This method is used to save lesson plan details.
    /// </summary>
    private void Save()
    {
        List<LessonPlanDetails> lstComments = new List<LessonPlanDetails>();
        bool bIsNonEmptyCommnetFound = false;
        foreach (HtmlTableRow oHtmlTableRow in tblLessons.Rows)
        {
            if (oHtmlTableRow.ID != null)
            {
                string sParameterId = oHtmlTableRow.ID.Substring(oHtmlTableRow.ID.IndexOf("_") + 1);
                HtmlTable oHtmlTable = oHtmlTableRow.FindControl("tblComments_" + sParameterId) as HtmlTable;
                if (oHtmlTable != null)
                {
                    foreach (HtmlTableRow tr in oHtmlTable.Rows)
                    {
                        if (tr.ID != null)
                        {
                            string iReportingUserId = tr.ID.Substring(tr.ID.IndexOf("_"));
                            TextBox txtComment = tr.FindControl("txtComment" + iReportingUserId) as TextBox;
                            string[] sArr = iReportingUserId.Split('_');

                            LessonPlanDetails oLessonPlanDetails = new LessonPlanDetails();
                            oLessonPlanDetails.ParameterId = sArr[3].ToInt();
                            oLessonPlanDetails.Comment = txtComment.Text.Trim();
                            oLessonPlanDetails.StdDivId = sArr[1].ToInt();
                            oLessonPlanDetails.SubjectId = sArr[2].ToInt();

                            TextBox txtSubjectStartDate = tr.FindControl("txtSubjectStartDate_" + sArr[1].ToInt() + "_" + sArr[2].ToInt()) as TextBox;
                            if (txtSubjectStartDate.Text != string.Empty)
                                oLessonPlanDetails.SubjectStartDate = txtSubjectStartDate.Text.Trim();
                            else
                                oLessonPlanDetails.SubjectStartDate = string.Empty;
                           
                            TextBox txtSubjectEndDate = tr.FindControl("txtSubjectEndDate_" + sArr[1].ToInt() + "_" + sArr[2].ToInt()) as TextBox;

                            if (txtSubjectEndDate.Text != string.Empty)
                                oLessonPlanDetails.SubjectEndDate = txtSubjectEndDate.Text.Trim();
                            else
                                oLessonPlanDetails.SubjectEndDate = string.Empty;

                            if (txtComment.Text.Trim() != string.Empty)
                                bIsNonEmptyCommnetFound = true;

                            lstComments.Add(oLessonPlanDetails);
                        }
                    }
                }
            }
        }

        if (lstComments.Count > 0 && bIsNonEmptyCommnetFound)
        {
            string sXml = base.GenerateXml(lstComments);
            int iUserId = hidUserId.Value.ToInt();

            moLessonPlanDetailsBL.Save(iUserId, miUserId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), sXml, hidOldStartDate.Value.ToDateTime(), hidOldEndDate.Value.ToDateTime());
        }
    }

    /// <summary>
    /// This method is used to save comments.
    /// </summary>
    private void SaveComment()
    {
        string sApproverComment = string.Empty;

        if (hidUserId.Value.ToInt() != miUserId)
        {
            foreach (HtmlTableRow oHtmlTableRow in tblComments.Rows)
            {
                TextBox txtComment = oHtmlTableRow.FindControl("txtComment") as TextBox;
                if (txtComment != null)
                {
                    sApproverComment = txtComment.Text.Trim();
                    break;
                }
            }
        }

        int iUserId = hidUserId.Value.ToInt();
        moLessonPlanDetailsBL.SaveComment(iUserId, miUserId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), sApproverComment, hidStartDate.Value.ToDateTime(), hidEndDate.Value.ToDateTime());
    }

    /// <summary>
    /// This method is used to update date fields.
    /// </summary>
    private void UpdateDateFields()
    {
        hidStartDate.Value = txtStartDate.Text;
        hidEndDate.Value = txtEndDate.Text;
        hidOldStartDate.Value = txtStartDate.Text;
        hidOldEndDate.Value = txtEndDate.Text;
    }

    /// <summary>
    /// This method is sued to set translator links.
    /// </summary>
    private void SetTranslatorLinks()
    {
        string sTranslationToolPath = @"../DOWNLOADS/Lesson Plan/InputToolsSetup.exe";
        string sTranslationGuidePath = @"../DOWNLOADS/Lesson Plan/GOOGLE TOOL GUIDE.pdf";
        lnkbtnTranslationTool.Attributes.Add("onclick", "OpenWindow('" + sTranslationToolPath + "'); return false;");
        lnkbtnTranslationGuide.Attributes.Add("onclick", "OpenWindow('" + sTranslationGuidePath + "'); return false;");
    }

    /// <summary>
    /// This method is used to check user has full access or not.
    /// </summary>
    private void CheckHasFullAccess()
    {        
        int iIsPublished = moLessonPlanDetailsBL.ApproverComments.Select(dr => dr.LessonPlanXMLId).FirstOrDefault();
        bool bIsReportingUser = moLessonPlanDetailsBL.ApproverComments.Select(dl => dl.IsReportingUser).FirstOrDefault();
        if (miUserId != hidUserId.Value.ToInt() && iIsPublished == Constants.I_ZERO)
        {
            if (bIsReportingUser)
            {
                if (!moLessonPlanDetailsBL.ButtonState.EnableSubmitButton)
                {
                    btnSaveDate.Visible = true;
                    btnApprove.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveComment.Enabled = false;
                    btnSaveCommentUpper.Enabled = false;
                    btnApproveUpper.Enabled = false;
                    hidNewStartDate.Value = txtStartDate.Text;
                    hidNewEndDate.Value = txtEndDate.Text;
                }                
                    tdStartDate.Visible = true;
                    tdEndDate.Visible = true;
                    tdLegendStartDate.Visible = false;
                    tdLegendEndDate.Visible = false;          
            }
            else
            {
                if ((!moLessonPlanDetailsBL.ButtonState.EnableSubmitButton && !moLessonPlanDetailsBL.ButtonState.EnableSaveButton) || (moLessonPlanDetailsBL.ButtonState.EnableSubmitButton && !moLessonPlanDetailsBL.ButtonState.EnableSaveButton))
                {
                    tdStartDate.Visible = true;
                    tdEndDate.Visible = true;
                    tdLegendStartDate.Visible = false;
                    tdLegendEndDate.Visible = false;
                    btnSaveDate.Visible = true;
                    btnApprove.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveComment.Enabled = false;
                    btnSaveCommentUpper.Enabled = false;
                    btnApproveUpper.Enabled = false;
                    hidNewStartDate.Value = txtStartDate.Text;
                    hidNewEndDate.Value = txtEndDate.Text;
                }
            }            
        }
    }

    private void DisableValidators()
    {
        if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
            CustValidateBlankSubjectDate.Enabled = false;
        else
            CustValidateBlankSubjectDate.Enabled = true;
    }

    #endregion
}