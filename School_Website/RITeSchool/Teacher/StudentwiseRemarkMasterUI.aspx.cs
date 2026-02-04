// File Name  : StudentwiseRemarkMasterUI.aspx.cs
// Created By : Vinod
// Date       : 12 Dec 11
// Modified By : Pravin
// Modified Date:30 Mar 12
// Description: This class is used save student remark details.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using System.Drawing;
using ProgressReportEntities;
using SchoolEntities;
using Utility;
using StudentEntities;
using System.Web.Script.Serialization;
public partial class StudentwiseRemarkMasterUI : ExportDataTable
{
    #region Constant

    private const int I_STUDENT_PROGRESS_REPORT = 82;
    private const int I_STUDENT_PROGRESS_REPORT_FOR_PPSN = 132;
    private const int I_STUDENT_PROGRESS_REPORT_FOR_FBS = 120;
    private const int I_ZERO_INDEX = 0;
    private const string S_SUCCESSFUL_MSG = "Progress remarks saved successfully !!!";
    private const string S_UNDERSCORE = "_";
    private const string S_DOLLER = "$";
    private const string S_OPENPOPUP = "OpenPopup";
    private const string S_REMARK_TEMPLATE_KEYWORDS = "sNotes";
    private const string S_FULLNAME = "%FULLNAME%";
    private const string S_FIRSTNAME = "%FNAME%";
    private const string S_MIDDLENAME = "%MNAME%";
    private const string S_LASTNAME = "%LNAME%";
    private const string S_SALUTATION = "%MASTER/MISS%";
    private const string S_SHOW_REPORT_BTNTXT = "Show Report";
    private const string S_SAVE_SHOW_REPORT_BTNTXT = "Save and Show Report";

    #endregion

    #region Members

    // Static Members : Because only static membrs are accessible in those classes whose implement "ITemplate" interface.
    private static List<StudentwiseRemarkConfigDetails> mLstStudentwiseRemarkConfigDetails;
    private static List<RemarkMaster> mLstRemarkMaster;
    private static int miRemarkId = 0;
    public static string mstaticID;
    public RemarksConfigurationBL moRemarksConfigurationBL;
    
    #endregion

    #region Events

    /// <summary>
    /// This event is used to set session variable values.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            if (cmbTeachers.ClientID != null)
                BindListViewTemplate(Convert.ToInt32(Request.Params[cmbTeachers.ClientID.Replace(S_UNDERSCORE, S_DOLLER)]), Convert.ToInt32(Request.Params[cmbStudents.ClientID.Replace(S_UNDERSCORE, S_DOLLER)]), Convert.ToInt32(Request.Params[cmbTermName.ClientID.Replace(S_UNDERSCORE, S_DOLLER)]));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill all combobox and set javascripts attributes to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moRemarksConfigurationBL = new RemarksConfigurationBL(miSchoolId, miAcademicYearId, miUserId);            
            if (!IsPostBack)
            {
                SetScreenWidth();
                SetJavascriptAttributes();

                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                FillTermComboBox();
                FillTeachersComboBox();
                ReadQueryString();                
                FillRemarksCombo();
                SetDefaultValues();
                ListViewRemarkConfigTemplate.ShowPopupHandler += new EventHandler(btnShowpopup_Click);
                FillTemplateKeywords();
                SetPageBackValue();
            }
            btnExport.Attributes["onclick"] = "if(!ConfirmSave()) return false;";
           
            if (cmbTeachers.SelectedIndex == Constants.I_ZERO || GetFinalExamPublishedStatus() == true)
                EnableDisableTimer(false);
            else
                EnableDisableTimer(true);

            CheckPublishedStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student remark listbox of peraticular class teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeachers.SelectedIndex == Constants.I_ZERO || GetFinalExamPublishedStatus() == true)
                EnableDisableTimer(false);
            else
                EnableDisableTimer(true);
            
            RemarksConfigurationBL oRemarkTemplate = new RemarksConfigurationBL();
            DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
            DataTable oDTStudents = GetStudentDataTable(Convert.ToInt32(cmbTeachers.SelectedValue));
            btnSave.Enabled = btnExport.Enabled = Convert.ToInt32(cmbTeachers.SelectedValue) == Constants.I_ZERO ? false : true;
            FillStudentsComboBox(oDTStudents);
            if (oDTStudents.Rows.Count > Constants.I_ZERO)
            {
                hidStdDivId.Value = Convert.ToString(oDTStudents.Rows[I_ZERO_INDEX]["SchoolWise_Standard_Division_Id"]);
                hidStandardId.Value = Convert.ToString(oDTStudents.Rows[I_ZERO_INDEX]["Standard_Id"]);
                hidTeacherId.Value = Convert.ToString(oDTStudents.Rows[I_ZERO_INDEX]["Teacher_Id"]);
                hidcmbTeacherValue.Value = cmbTeachers.SelectedValue.ToString();
            }
            GetMaxRemarkLength();
            BindListViewTemplate();
            CheckPublishedStatus();
            BindListViewData();
            lblNorecord.Visible = false;
            IsXseedApplicable();
            SetPageBackValue();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method is used to check isXseed applicable.
    /// </summary>
    private void IsXseedApplicable()
    {
        int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        int iTeachersStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
        XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
        if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, miAcademicYearId, iStandardId, iTeachersStandardDivisionId))
            hidIsPreprimaryStandard.Value = Constants.S_ONE;
        else
            hidIsPreprimaryStandard.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This event is used to fill student remark listbox of peraticular term.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTermName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
            GetMaxRemarkLength();
            DisplayStudentList(Convert.ToInt32(cmbTeachers.SelectedValue));
            BindListViewTemplate();

            BindListViewData();
            hidcmbTermValue.Value = cmbTermName.SelectedValue.ToString();
            lblNorecord.Visible = false;
            CheckPublishedStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student remark listbox of peraticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          
            DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
            BindListViewTemplate();
            BindListViewData();
            lblUpdateSucess.Visible = false;
            lblNorecord.Visible = false;
            hidcmbStudentValue.Value = cmbStudents.SelectedValue.ToString();
            CheckPublishedStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUpdateSucess.Visible = false;
            SetDataPagerAccordingToPageNo();
            DropDownList ocmbPageCount = DtPgDropDown.Controls[I_ZERO_INDEX].FindControl("ddlCnt") as DropDownList;
            hidPageNo.Value = (ocmbPageCount.SelectedIndex + Constants.I_ONE).ToString();
            BindListViewTemplate();
            BindListViewData();
            CheckPublishedStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowpopup_Click(object sender, EventArgs e)
    {
        try
        {
            Button btnShow = sender as Button;
            string[] sArr = btnShow.ID.Split('_');
            if (sArr[1] != string.Empty)
            {
                hidTextBoxId.Value = "txt" + sArr[2];
                mstaticID = "txt" + sArr[2];                
            }
            int ibtnId = Convert.ToInt32(sArr[1]);
            miRemarkId = ibtnId;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data row wise.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void lstvwStudentRemarkDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label oRemarkLength;
            Label oLblRollNo = e.Item.FindControl("lblRollNo") as Label;
            Label oLblName = e.Item.FindControl("lblName") as Label;
            Label oLblRemark = e.Item.FindControl("LblOldRemark") as Label;            
            ListViewDataItem lstDataItem = e.Item as ListViewDataItem;
            if (oLblRollNo != null)
                oLblRollNo.Text = ((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).RollNo.ToString();
            if (oLblName != null)
                oLblName.Text = ((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).StudentName;
            if (oLblRemark != null)
            {
                string sRemark = ((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).OldRemark;
                if (sRemark.Trim() != string.Empty)
                    oLblRemark.Text = sRemark;
            }

            int iYearwiseStudentId = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["YearwiseStudentId"].ToInt();

            for (int iRemarkCount = Constants.I_ZERO; iRemarkCount < mLstRemarkMaster.Count; iRemarkCount++)
            {
                TextBox oRemarkTextBox = e.Item.FindControl("txt" + mLstRemarkMaster[iRemarkCount].RemarkName.RemoveSingleQuote()) as TextBox;
                Button obtnshowpopup = e.Item.FindControl("btn_" + mLstRemarkMaster[iRemarkCount].RemarkConfigId + "_" + mLstRemarkMaster[iRemarkCount].RemarkName.RemoveSingleQuote()) as Button;
                oRemarkLength = e.Item.FindControl("lbl" + mLstRemarkMaster[iRemarkCount].RemarkName.RemoveSingleQuote()) as Label;
                RadioButton oRdBtnPassedAndPromoted = e.Item.FindControl("rdbtnPassedAndPromoted") as RadioButton;
                RadioButton oRdBtnPromoted = e.Item.FindControl("rdbtnPromoted") as RadioButton;

                if (oRemarkTextBox != null)
                {
                    var lstRemarks = mLstStudentwiseRemarkConfigDetails.Where(rmk => rmk.YearwiseStudentId == iYearwiseStudentId).ToList();

                    foreach (var rmk in lstRemarks)
                    {
                        if (("txt" + rmk.RemarkMaster.RemarkName.RemoveSingleQuote()) == oRemarkTextBox.ID)
                            oRemarkTextBox.Text = rmk.Remark;
                    }

                    oRemarkTextBox.Attributes.Add("onChange", "if(!IsTextChange('" + oRemarkTextBox.ClientID + "')){return false;}");
                    oRemarkTextBox.Attributes.Add("onkeyup", "alertMsgLength(event, this);");
                    obtnshowpopup.Click += new EventHandler(btnShowpopup_Click);
                    if (cmbTermName.SelectedValue == Constants.I_TWO.ToString())
                        if (((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).IsPassedAndPromoted && oRdBtnPassedAndPromoted != null)
                            oRdBtnPassedAndPromoted.Checked = true;
                        else if (oRdBtnPromoted != null)
                            oRdBtnPromoted.Checked = true;
                }

                oRemarkLength.Text = (hidRemarkLength.Value.ToInt() - oRemarkTextBox.Text.Length).ToString();
                if (Convert.ToInt32(oRemarkLength.Text) < 0)
                    oRemarkLength.Text = Constants.S_ZERO;
                oRemarkLength.Text = " (" + oRemarkLength.Text + ")";
                if (((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).IsLeftStudent == Constants.I_ONE)
                    oLblRollNo.ForeColor = oLblName.ForeColor = Color.Red;
            }

            // Actually in Get USP, if more than 1 remarks are added for any student, then that many roe get retrieve from DB (i.e. for same roll no)
            // Hence, if same roll no > 1, then add textbox (Remark) value.
            var lstCurrentStudentRemarks = mLstStudentwiseRemarkConfigDetails.Where(s => s.StudentwiseRemarkId == ((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).StudentwiseRemarkId).ToList();
            int iRowCount = 0;
            if (lstCurrentStudentRemarks.Count > Constants.I_ONE)
            {
                for (int iPageCount = Constants.I_ZERO; iPageCount < lstCurrentStudentRemarks.Count; iPageCount++)
                {
                    while (iRowCount < mLstRemarkMaster.Count)
                    {
                        TextBox txtRemarkName = e.Item.FindControl("txt" + mLstRemarkMaster[iRowCount].RemarkName.RemoveSingleQuote()) as TextBox;
                        oRemarkLength = e.Item.FindControl("lbl" + mLstRemarkMaster[iRowCount].RemarkName.RemoveSingleQuote()) as Label;
                        if (txtRemarkName != null)
                            if ("txt" + lstCurrentStudentRemarks[iPageCount].RemarkMaster.RemarkName.RemoveSingleQuote() == txtRemarkName.ID)
                            {
                                txtRemarkName.Text = lstCurrentStudentRemarks[iPageCount].Remark;
                                oRemarkLength.Text = " (" + (hidRemarkLength.Value.ToInt() - txtRemarkName.Text.Length).ToString() + ")";
                                break;
                            }

                        iRowCount++;
                    }
                }
            }
            hidRollNo.Value = ((StudentwiseRemarkConfigDetails)lstDataItem.DataItem).RollNo.ToString();


        }
    }

    /// <summary>
    /// This event is used to save the remark of student in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            EnableDisableTimer(false);
            SaveRemarkDetails();
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = S_SUCCESSFUL_MSG;
            EnableDisableTimer(true);
            BindListViewTemplate();
            BindListViewData();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show student Report according to the selected filter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        try
        {
            EnableDisableTimer(false);
           
            if(btnShowReport.Text != S_SHOW_REPORT_BTNTXT)
                SaveRemarkDetails();

            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = S_SUCCESSFUL_MSG;
            EnableDisableTimer(true);
            BindListViewTemplate();
            BindListViewData();
            StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();

            // To check whether it is StudentwiseProgressReport.rpt report or not.
            int iReportId = StandardMasterBL.GetProgressReportForStandard(miSchoolId,
                                            miAcademicYearId,
                                            Convert.ToInt32(cmbTeachers.SelectedValue));
            bool bIsStudentWiseProgressReport = (iReportId == I_STUDENT_PROGRESS_REPORT || iReportId == I_STUDENT_PROGRESS_REPORT_FOR_FBS || iReportId == I_STUDENT_PROGRESS_REPORT_FOR_PPSN);
            DataTable odtReportRecord = oStudentwiseRemarkMasterBL.IsReportEmpty(Convert.ToInt32(cmbStudents.SelectedValue), Convert.ToInt32(hidStdDivId.Value), Convert.ToInt32(cmbTermName.SelectedValue), Convert.ToInt32(hidStandardId.Value),
                                                        miSchoolId, miAcademicYearId);
            if (odtReportRecord.Rows.Count == Constants.I_ZERO)
            {
                lblNorecord.Visible = true;
                lblNorecord.Text = "No exams are conducted for " + (cmbTermName.SelectedItem.Text == "Term-I" ? "First Term" : "Final Term") + ".";
            }
            else
            {
                // Assigning report as per standard.
                Constants.ExportReports oExportReport = bIsStudentWiseProgressReport ? Constants.ExportReports.StudentwiseProgressReport
                                                                : cmbTermName.SelectedValue == Constants.I_ONE.ToString() ? Constants.ExportReports.StudentTerm1ProgressReport
                                                                        : Constants.ExportReports.StudentTerm2ProgressReport; ;

                if (miSchoolId == Constants.SchoolId.SS.ToInt())
                {
                    oExportReport = bIsStudentWiseProgressReport ? Constants.ExportReports.StudentwiseProgressReportSS
                                                             : cmbTermName.SelectedValue == Constants.I_ONE.ToString() ? Constants.ExportReports.StudentTerm1ProgressReport
                                                                     : Constants.ExportReports.StudentTerm2ProgressReport;
                }

                if (miSchoolId == Constants.SchoolId.FBS.ToInt())
                {
                    oExportReport = bIsStudentWiseProgressReport ? Constants.ExportReports.StudentwiseProgressReportFBS
                                                             : cmbTermName.SelectedValue == Constants.I_ONE.ToString() ? Constants.ExportReports.StudentTerm1ProgressReport
                                                                     : Constants.ExportReports.StudentTerm2ProgressReport;
                }

                if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                {
                    oExportReport = bIsStudentWiseProgressReport ? Constants.ExportReports.StudentwiseProgressReportPPSN
                                                             : cmbTermName.SelectedValue == Constants.I_ONE.ToString() ? Constants.ExportReports.StudentTerm1ProgressReport
                                                                     : Constants.ExportReports.StudentTerm2ProgressReport;
                }

                ReportDisplay oReportDisplay = new ReportDisplay(oExportReport, GetFilterString(oExportReport), ExportFormatType.PortableDocFormat);
                oReportDisplay.DisplayReport();
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to sort the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortDirection();
            DisplayTemplateRemarks();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method gives you the templates according to selected remaark
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRemarksOnDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DisplayTemplateRemarks();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method gives you the templates according to selected remaark
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbGradesOnDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DisplayTemplateRemarks();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is handled to get selected studentsId
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentRemarkDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {   
                hidTextBoxId.Value = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["YearwiseStudentId"].ToString();
                var vSelectedRemarkLength = ((TextBox)lstvwStudentRemarkDetails.Items[e.Item.DisplayIndex].FindControl(mstaticID)).Text.Length.ToString();
                int number;
                if (Int32.TryParse(vSelectedRemarkLength.ToString(),out  number))
                    hidSelectedRemarkLength.Value = number.ToString();
                else
                    hidSelectedRemarkLength.Value = Constants.S_ZERO;
                
                
                if (e.CommandName == S_OPENPOPUP)
                {
                    cmbRemarksOnDiv.SelectedIndex = 0;
                    lblStudName.Text = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["StudentName"].ToString();
                    hidFname.Value = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["FName"].ToString();
                    hidMname.Value = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["MName"].ToString();
                    hidLname.Value = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["LName"].ToString();
                    hidSalutationId.Value = lstvwStudentRemarkDetails.DataKeys[e.Item.DisplayIndex]["SalutationId"].ToString();
                    FillGradesCombo();
                    DisplayTemplateRemarks();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "OpenPopup();", true);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save templates from popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPopupSave_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder oTemplates = new StringBuilder();
            foreach (ListViewDataItem Item in lstvwTemplates.Items)
            {
                CheckBox chkTemplate = Item.FindControl("chkTemplate") as CheckBox;
                if (chkTemplate.Checked)
                    oTemplates.Append(" " + (Item.FindControl("lblTemplate") as Label).Text);
            }

            foreach (ListViewDataItem Item in lstvwStudentRemarkDetails.Items)
            {
                string sYearwiseStudentId = lstvwStudentRemarkDetails.DataKeys[Item.DisplayIndex]["YearwiseStudentId"].ToString();
                TextBox txtId = Item.FindControl(mstaticID) as TextBox;                
                Label olblRemarksLength = Item.FindControl(mstaticID.Replace("txt", "lbl")) as Label;
                if (txtId != null && sYearwiseStudentId == hidTextBoxId.Value)
                {
                    txtId.Text += oTemplates.ToString().Trim();
                    hidTextChanged.Value = Constants.S_ONE;
                    olblRemarksLength.Text = " (" + (hidRemarkLength.Value.ToInt() - txtId.Text.Length) + ")";
                    break;
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CloseP", "HidePopup()", true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to call bind data to label
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label oLabel = e.Item.FindControl("lblTemplate") as Label;
                oLabel.Text = UpdateTemplateText(oLabel.Text);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Export students remarks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable oDTStudents = GetStudentDataTableToExport(cmbTeachers.SelectedValue.ToInt(), cmbStudents.SelectedValue.ToInt(), cmbTermName.SelectedValue.ToInt());
            if (oDTStudents.Rows.Count > Constants.I_ZERO)
                ExportToExcel("StudentwiseRemarkMaster.XLS", oDTStudents);
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save students remark at specific interval of time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void timer_Tick(object sender, EventArgs e)
    {
        try
        {
            if (hidTimerStart.Value == Constants.S_YES)
            {
                EnableDisableTimer(false);
                SaveRemarkDetails();
                EnableDisableTimer(true);
                BindListViewTemplate();
                BindListViewData();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///This method is used to fill the notes with appropriate rows and columns.
    /// </summary>
    private void FillTemplateKeywords()
    {
        List<RemarkTemplateKeyword> olstRemarkTemplateKeywords = RemarksConfigurationBL.GetTemplateNotes();
        var jsSerializer = new JavaScriptSerializer();
        hidRemarkTemplateKeywordsJSON.Value = jsSerializer.Serialize(olstRemarkTemplateKeywords);
    }

    /// <summary>
    /// This method is used to set screen width.
    /// </summary>
    private void SetScreenWidth()
    {
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            string str = Session[Constants.S_SESSION_SCREEN_WIDTH].ToString().Replace("px !important", string.Empty);
            int iWidth = Convert.ToInt32(str);
            iWidth = iWidth / 100 * 80;
            divStudentRemarkDetails.Style.Add("width", iWidth.ToString() + "px !important");
        }
        else
            divStudentRemarkDetails.Style.Add("width", Convert.ToString(1024) + "px !important");

    }

    /// <summary>
    /// This function is used to set appropriate values for the keywords.
    /// </summary>
    /// <param name="asNote"></param>
    /// <returns></returns>
    private string UpdateTemplateText(string asNote)
    {
        string sValue = string.Empty;
        string sTemplate = string.Empty;
        int iIndex = Constants.I_ZERO;

        List<RemarkTemplateKeyword> olstRemarkTemplateKeywords = new List<RemarkTemplateKeyword>();
        var jsonData = hidRemarkTemplateKeywordsJSON.Value;
        var jsSerializer = new JavaScriptSerializer();
        if (jsonData != string.Empty)
            olstRemarkTemplateKeywords = jsSerializer.Deserialize<List<RemarkTemplateKeyword>>(jsonData);

        olstRemarkTemplateKeywords.ForEach(templateText =>
        {
            sValue = Constants.Salutation.Master.ToInt() == Convert.ToInt32(hidSalutationId.Value) ? templateText.Male : templateText.Female;
            iIndex = asNote.IndexOf(templateText.Keyword);
            if (iIndex != -1 && templateText.Keyword != S_SALUTATION)
            {
                sTemplate = asNote.Substring(Constants.I_ZERO, iIndex).Trim();
                if (!sTemplate.EndsWith(".") && iIndex != Constants.I_ZERO)
                    sValue = sValue.ToLower();
            }
            asNote = asNote.Replace(templateText.Keyword, sValue);
            asNote = asNote.Replace(S_FULLNAME, lblStudName.Text);
            asNote = asNote.Replace(S_FIRSTNAME, hidFname.Value);
            asNote = asNote.Replace(S_MIDDLENAME, hidMname.Value);
            asNote = asNote.Replace(S_LASTNAME, hidLname.Value);
        });
        return asNote.TrimAll();
    }

    /// <summary>
    /// This method is use to get filter parameters to print report.
    /// </summary>
    /// <param name="aoExportReports"></param>
    /// <returns></returns>
    private string GetFilterString(Constants.ExportReports aoExportReports)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        string sSchoolYearFilter = string.Empty;
        string sNote = string.Empty;
        int iStudentId;

        iStudentId = Convert.ToInt32(cmbStudents.SelectedValue) != Constants.I_ZERO ? Convert.ToInt32(cmbStudents.SelectedValue) : Constants.I_ZERO;

        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        int iTerm_Id = Convert.ToInt32(cmbTermName.SelectedValue);
        sNote = (aoExportReports == Constants.ExportReports.StudentwiseProgressReport || aoExportReports == Constants.ExportReports.StudentwiseProgressReportFBS || aoExportReports == Constants.ExportReports.StudentwiseProgressReportPPSN) ? "Term1" : string.Empty;

        string sReportUSP = GetReportUspName(aoExportReports);
        int iStandardDivId = Convert.ToInt32(hidStdDivId.Value);
        string sViewNameSchoolID = sReportUSP + ".School_Id}";
        string sViewNameAcdYearId = sReportUSP + ".Academic_Year_Id}";
        string sViewNameStudentId = sReportUSP + ".StudentId}";
        string sViewNameStandardId = sReportUSP + ".Standard_Id}";
        string sViewNameDivisionId = sReportUSP + ".Division_Id}";
        string sViewNameTermId = string.Empty;
        string sViewNameNote = string.Empty;
        if (sReportUSP == Constants.S_EXPORT_STUDENTPROGRESSREPORT_USP)
        {
            sViewNameTermId = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USP + ".Term_Id}";
            sViewNameNote = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USP + ".Note}";
        }
        else if (sReportUSP == Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPSS)
        {
            sViewNameTermId = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPSS + ".Term_Id}";
            sViewNameNote = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPSS + ".Note}";
        }
        else if (sReportUSP == Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPFBS)
        {
            sViewNameTermId = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPFBS + ".Term_Id}";
            sViewNameNote = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPFBS + ".Note}";
        }
        else if (sReportUSP == Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPPPSN)
        {
            sViewNameTermId = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPPPSN + ".Term_Id}";
            sViewNameNote = Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPPPSN + ".Note}";
        }

        sSchoolYearFilter = "(" + sViewNameSchoolID + "=" + miSchoolId + " AND "
                           + sViewNameAcdYearId + "=" + miAcademicYearId + " AND "
                           + sViewNameStudentId + "=" + iStudentId + " AND "
                           + sViewNameStandardId + "=" + iStandardId + " AND "
                           + sViewNameDivisionId + "=" + iStandardDivId + " AND "
                           + ((sViewNameTermId != string.Empty) ? sViewNameTermId + "=" + iTerm_Id + " AND " : string.Empty)
                           + sViewNameNote + "=" + sNote + ")";
        return sSchoolYearFilter + "@ ";
    }

    /// <summary>
    /// This method is used to get usp name for report generation.
    /// </summary>
    /// <param name="aoExportReports"></param>
    /// <returns></returns>
    private string GetReportUspName(Constants.ExportReports aoExportReports)
    {
        switch (aoExportReports)
        {
            case Constants.ExportReports.StudentwiseProgressReport:
                return Constants.S_EXPORT_STUDENTPROGRESSREPORT_USP;
            case Constants.ExportReports.StudentTerm1ProgressReport:
                return Constants.S_EXPORT_STUDENT_TERM_1_PROGRESS_REPORT_USP;
            case Constants.ExportReports.StudentTerm2ProgressReport:
                return Constants.S_EXPORT_STUDENT_TERM_2_PROGRESS_REPORT_USP;
            case Constants.ExportReports.StudentwiseProgressReportSS:
                return Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPSS;
            case Constants.ExportReports.StudentwiseProgressReportFBS:
                return Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPFBS;
            case Constants.ExportReports.StudentwiseProgressReportPPSN:
                return Constants.S_EXPORT_STUDENTPROGRESSREPORT_USPPPSN;
        }

        return string.Empty;
    }

    /// <summary>
    /// This method is used to save remark details.
    /// </summary>
    private void SaveRemarkDetails()
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        int iStandardDivId = Convert.ToInt32(lstvwStudentRemarkDetails.DataKeys[I_ZERO_INDEX]["StandardDivisionId"]);
        oStudentwiseRemarkMasterBL.UpdateStudentwiseRemarkDetails(GenerateXml(PopulateStudentwiseRemarkList()), miSchoolId, miAcademicYearId, miUserId, iStandardDivId, Convert.ToInt32(cmbTermName.SelectedValue));
    }

    
    /// <summary>
    /// This method is used to Get flag for Final Exam published status for current year.
    /// </summary>
    /// <returns></returns>
    private bool GetFinalExamPublishedStatus(out bool abIsPublishedStatus)
    {    
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
       
       return  oStudentwiseRemarkMasterBL.GetFinalPublishedExamStatus(Convert.ToInt32(cmbTeachers.SelectedValue), miSchoolId, Convert.ToInt32(cmbTermName.SelectedValue), miAcademicYearId ,  out abIsPublishedStatus);
    }

    private bool GetFinalExamPublishedStatus()
    {
        bool bIsPublishedStatus;
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();

        return oStudentwiseRemarkMasterBL.GetFinalPublishedExamStatus(Convert.ToInt32(cmbTeachers.SelectedValue), miSchoolId, Convert.ToInt32(cmbTermName.SelectedValue), miAcademicYearId, out bIsPublishedStatus);
    }

    /// <summary>
    /// This method is used to set default values. 
    /// </summary>
    private void SetDefaultValues()
    {
        cmbTeachers.Focus();
        hidRollNo.Value = string.Empty;
        cmbTeachers.Focus();
        btnSave.Enabled = btnExport.Enabled = Convert.ToInt32(cmbTeachers.SelectedValue) == Constants.I_ZERO ? false : true;
        lblNorecord.Visible = false;
        hidRemarkLength.Value = Settings.RemarkLength.ToString();
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = "Template";
        cmbRemarksOnDiv.Focus();
        cmbRemarksOnDiv.Attributes.Add("onchange", "if(!MessageAlertPopUp()){return false;}");       
    }

    /// <summary>
    /// This event is used to fill teacher combo box.
    /// </summary>
    private void FillTeachersComboBox()
    {
        EnableDisableTimer(false);
        // get all class teachers
        DataTable oDtTeachers = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetAllClassTeachers1(miSchoolId, miAcademicYearId, miUserId);
        ControlUtility.FillDropDownList(oDtTeachers, ref cmbTeachers,
                                            Constants.S_STANDARD_DIVISION_ID_FIELD,
                                             Constants.S_TEACHER_NAME_FIELD,
                                             Constants.S_SELECT);
        if (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.ProgressRemarks).ToString() == Constants.S_NO && moUserRole != Constants.UserRoles.Admin)
        {
            
            string sMaxReamrkLength = string.Empty;

            if (oDtTeachers.Rows.Count > 0 && oDtTeachers.Rows[0]["IsReportingUser"].ToString() != Constants.S_ONE)
            {
                int iStdDivId = 0;
                int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
                DataRow[] dr = oDtTeachers.Select("Teacher_Id=" + iTeacherId);
                if (dr.Length > 0)
                    iStdDivId = Convert.ToInt32(dr[0].ItemArray[8]);

                ListSource.FillDropDownList(dr.CopyToDataTable(), cmbTeachers, "TeacherName", "Schoolwise_Standard_Division_Id", Constants.S_SELECT);
                cmbTeachers.SelectedValue = iStdDivId.ToString();

                sMaxReamrkLength = Convert.ToString(moRemarksConfigurationBL.GetConfiguredMaxRemarkLength(Convert.ToInt32(cmbTeachers.SelectedValue), cmbTermName.SelectedValue.ToInt()));
                hidRemarkLength.Value = sMaxReamrkLength.Equals(Constants.S_ZERO) ? Settings.RemarkLength.ToString() : sMaxReamrkLength.ToString();
                DataTable oDTStudents = GetStudentDataTable(iStdDivId);
                FillStudentsComboBox(oDTStudents);
                cmbStudents_SelectedIndexChanged(cmbStudents, null);

                EnableDisableTimer(false);
            }          
         
        }
    }

    /// <summary>
    ///  This method isused to get all student list of selected teacher.
    /// </summary>
    /// <param name="aiTeacherId"></param>
    /// <returns></returns>
    private DataTable GetStudentDataTable(int aiTeacherId)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        DataTable oDSStudentsList = oStudentwiseRemarkMasterBL.GetStudentListOfGivenClassTeacher(aiTeacherId, miAcademicYearId, miSchoolId, Convert.ToInt32(cmbTermName.SelectedValue));

        if (oDSStudentsList.Rows.Count > 0)
        {
            hidStdDivId.Value = Convert.ToString(oDSStudentsList.Rows[I_ZERO_INDEX]["SchoolWise_Standard_Division_Id"]);
            hidStandardId.Value = Convert.ToString(oDSStudentsList.Rows[I_ZERO_INDEX]["Standard_Id"]);
        }
        return oDSStudentsList;
    }

    /// <summary>
    ///  This method isused to get selected student list .
    /// </summary>
    /// <param name="aiTeacherId"></param>
    /// <returns></returns>
    private DataTable GetStudentDataTableToExport(int aiTeacherId, int aiStudentId, int aiTermId)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        StudentInfo oStudentInfo = new StudentInfo();
        List<StudentInfo> lstStudents = new List<StudentInfo>();
        List<StudentwiseRemarkConfigDetails> lstStudentRemarks = new List<StudentwiseRemarkConfigDetails>();
        List<StudentwiseRemarkConfigDetails> lstCategories = new List<StudentwiseRemarkConfigDetails>();
        lstStudents = oStudentwiseRemarkMasterBL.GetStudentDataToExport(miSchoolId, miAcademicYearId, aiTeacherId, aiStudentId, aiTermId);
        lstStudentRemarks = oStudentwiseRemarkMasterBL.StudenRemarks;
        lstCategories = oStudentwiseRemarkMasterBL.RemarkCategories;

        DataTable oDTExportTable = new DataTable();
        oDTExportTable.AddColumns(new string[] { "Roll No.", "Class Name", "Student Name", "Term Name" });
        lstCategories.ForEach(
            Config => { oDTExportTable.Columns.Add(Config.Remark); });
        //Add students
        lstStudents.ForEach(
             Student =>
             {
                 DataRow oDataRow = oDTExportTable.NewRow();
                 oDataRow["Roll No."] = Student.RollNo;
                 oDataRow["Class Name"] = Student.ClassName;
                 oDataRow["Student Name"] = Student.StudentName;

                 //Add Remark Details
                 for (int index = 3; index < oDTExportTable.Columns.Count; index++)
                 {
                     string sRemark = lstStudentRemarks.Where(st => st.YearwiseStudentId == Student.YearwiseStudentId && st.Remark == oDTExportTable.Columns[index].ColumnName)
                         .Select(remark => remark.RemarkDetails).FirstOrDefault();
                     oDataRow[oDTExportTable.Columns[index].ColumnName] = sRemark;
                 }
                 oDataRow["Term Name"] = cmbTermName.SelectedItem;
                 oDTExportTable.Rows.Add(oDataRow);
             }
            );
        return oDTExportTable;
    }

    /// <summary>
    /// This methd is used to fill term combobox.
    /// </summary>
    private void FillTermComboBox()
    {
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbTermName, "Value_Member", "Display_Member", string.Empty);
        hidcmbTermValue.Value = cmbTermName.SelectedValue.ToString();
    }

    /// <summary>
    /// This event is used to fill student combo box.
    /// </summary>
    /// <param name="aoDtStudent"></param>
    private void FillStudentsComboBox(DataTable aoDtStudent)
    {
        ControlUtility.FillDropDownList(aoDtStudent, ref cmbStudents, "Student_Id", "Student_Name", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to create list view template dynamically.
    /// </summary>
    /// <param name="aTeacherId"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiTermId"></param>
    private void BindListViewTemplate(int aTeacherId, int aiStudentId, int aiTermId)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        oStudentwiseRemarkMasterBL.GetStudentwiseRemarkConfigDetails(miSchoolId, miAcademicYearId, aTeacherId, aiStudentId, aiTermId);
        mLstStudentwiseRemarkConfigDetails = oStudentwiseRemarkMasterBL.olstStudentwiseRemarkConfigDetails;
        mLstRemarkMaster = oStudentwiseRemarkMasterBL.olstRemarkMaster;
        int iListCount = mLstStudentwiseRemarkConfigDetails.Count;
        int iTermId = (Request.Params[cmbTermName.ClientID.Replace(S_UNDERSCORE, S_DOLLER)]).ToInt();
        bool bAllowStudentResultSelection = Settings.AllowStudentResultSelection;
        if (mLstRemarkMaster.Count > Constants.I_ZERO)
        {
            trNorecordFound.Visible = false;
            trListView.Visible = btnSave.Enabled = true;

            lstvwStudentRemarkDetails.LayoutTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.EmptyItem, mLstStudentwiseRemarkConfigDetails, false, iTermId, bAllowStudentResultSelection, miSchoolId);
            lstvwStudentRemarkDetails.ItemTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.DataItem, mLstStudentwiseRemarkConfigDetails, false, iTermId, bAllowStudentResultSelection, miSchoolId);
            lstvwStudentRemarkDetails.AlternatingItemTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.DataItem, mLstStudentwiseRemarkConfigDetails, true, iTermId, bAllowStudentResultSelection, miSchoolId);
        }
        else
        {
            if (!cmbTermName.SelectedValue.IsNullOrEmpty())
                lstvwStudentRemarkDetails.LayoutTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.EmptyItem, mLstStudentwiseRemarkConfigDetails, false, iTermId, bAllowStudentResultSelection, miSchoolId);
            btnSave.Enabled = trListView.Visible = false;
            trNorecordFound.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to create list view template dynamically.
    /// </summary>
    private void BindListViewTemplate()
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        oStudentwiseRemarkMasterBL.GetStudentwiseRemarkConfigDetails(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue), Convert.ToInt32(cmbStudents.SelectedValue), Convert.ToInt32(cmbTermName.SelectedValue));
        mLstStudentwiseRemarkConfigDetails = oStudentwiseRemarkMasterBL.olstStudentwiseRemarkConfigDetails;
        mLstRemarkMaster = oStudentwiseRemarkMasterBL.olstRemarkMaster;
        hidRemarkListCount.Value = mLstRemarkMaster.Count.ToString();
        if (mLstRemarkMaster.Count == Constants.I_ZERO)
            btnSave.Enabled = trListView.Visible = trPagerUser.Visible = false;
        int iTermId = (Request.Params[cmbTermName.ClientID.Replace(S_UNDERSCORE, S_DOLLER)]).ToInt();
        bool bAllowStudentResultSelection = Settings.AllowStudentResultSelection;
        if (!cmbTermName.SelectedValue.IsNullOrEmpty())
        {
            lstvwStudentRemarkDetails.LayoutTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.EmptyItem, mLstStudentwiseRemarkConfigDetails, false, iTermId, bAllowStudentResultSelection, miSchoolId);
            lstvwStudentRemarkDetails.ItemTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.DataItem, mLstStudentwiseRemarkConfigDetails, false, iTermId, bAllowStudentResultSelection, miSchoolId);
            lstvwStudentRemarkDetails.AlternatingItemTemplate = new ListViewRemarkConfigTemplate(ListViewItemType.DataItem, mLstStudentwiseRemarkConfigDetails, true, iTermId, bAllowStudentResultSelection, miSchoolId);
        }

        for (int iNo = Constants.I_ZERO; iNo < mLstRemarkMaster.Count; iNo++)
            hidRemarkNameList.Value = hidRemarkNameList.Value == string.Empty ? mLstRemarkMaster[iNo].RemarkName.RemoveSingleQuote() : hidRemarkNameList.Value + "," + mLstRemarkMaster[iNo].RemarkName.RemoveSingleQuote();
    }

    /// <summary>
    /// This method is used to display Student combo box vales on page load.
    /// </summary>
    /// <param name="aiTeacherId"></param>
    private void DisplayStudentList(int aiTeacherId)
    {
        DataTable oDTStudents = GetStudentDataTable(aiTeacherId);
        FillStudentsComboBox(oDTStudents);
    }

    /// <summary>
    /// This method is used to bind data to list view.
    /// </summary>
    private void BindListViewData()
    {
        lstvwStudentRemarkDetails.DataSource = GetDistinctStudentRemarkList();
        lstvwStudentRemarkDetails.DataBind();
        FillListViewPagerFooter();
        DropDownList ocmbPageCount = DtPgDropDown.Controls[I_ZERO_INDEX].FindControl("ddlCnt") as DropDownList;
        ocmbPageCount.Attributes.Add("onchange", "if(!MessageAlert('" + ocmbPageCount.ClientID + "')){return false;}");
        hidListviewPageRowCnt.Value = lstvwStudentRemarkDetails.Items.Count.ToString();
        tdPgr.Width = (375 + (200 * mLstRemarkMaster.Count)).ToString() + "px";
        hidTextChanged.Value = string.Empty;

    }

    /// <summary>
    /// This method is used to get Maximum Remark Length.
    /// </summary>
    private void GetMaxRemarkLength()
    {
        string sMaxReamrkLength = string.Empty;
        hidStandardId.Value = hidStandardId.Value != string.Empty ? hidStandardId.Value : Constants.S_ZERO;
        sMaxReamrkLength = Convert.ToString(moRemarksConfigurationBL.GetConfiguredMaxRemarkLength(Convert.ToInt32(hidStandardId.Value), cmbTermName.SelectedValue.ToInt()));
        hidRemarkLength.Value = sMaxReamrkLength.Equals(Constants.S_ZERO) ? Settings.RemarkLength.ToString() : sMaxReamrkLength.ToString();
    }

    /// <summary>
    /// This method is used to fill data pager.
    /// </summary>
    private void FillListViewPagerFooter()
    {
        trDataPager.Visible = trPagerUser.Visible = false;
        int iCurrPage = (DtPgDropDown.StartRowIndex / DtPgDropDown.PageSize) + Constants.I_ONE;
        int iTotalPage = DtPgDropDown.TotalRowCount / DtPgDropDown.PageSize;
        if (iTotalPage * DtPgDropDown.PageSize < DtPgDropDown.TotalRowCount)
            iTotalPage += Constants.I_ONE;

        if (iTotalPage > Constants.I_ONE && mLstRemarkMaster.Count > Constants.I_ZERO)
        {
            trDataPager.Visible = trPagerUser.Visible = true;
            DropDownList ocmbPageCount = DtPgDropDown.Controls[I_ZERO_INDEX].FindControl("ddlCnt") as DropDownList;
            if (ocmbPageCount.Items.Count == Constants.I_ZERO)
            {
                for (int iPageCount = Constants.I_ONE; iPageCount <= iTotalPage; iPageCount++)
                    ocmbPageCount.Items.Add(iPageCount.ToString());

                // Set the DDL to the appropriate page value
                ocmbPageCount.Items.FindByValue(iCurrPage.ToString()).Selected = true;

                Label lblCurrentPageLabel = DtPgDropDown.Controls[I_ZERO_INDEX].FindControl("CurrentPageLabel") as Label;
                lblCurrentPageLabel.Font.Bold = true;
                lblCurrentPageLabel.Text = "Page " + iCurrPage + " of " + iTotalPage;
            }
        }
    }

    /// <summary>
    /// This method is used to fill dictinct student list view.
    /// </summary>
    /// <returns></returns>
    private List<StudentwiseRemarkConfigDetails> GetDistinctStudentRemarkList()
    {
        List<StudentwiseRemarkConfigDetails> lstDistinctStudentwiseRemarkConfigDetails = new List<StudentwiseRemarkConfigDetails>();
        foreach (StudentwiseRemarkConfigDetails student in mLstStudentwiseRemarkConfigDetails)
        {
            if (lstDistinctStudentwiseRemarkConfigDetails.Where(sRollNo => sRollNo.RollNo == student.RollNo && sRollNo.StudentName == student.StudentName).ToList().Count == Constants.I_ZERO)
                lstDistinctStudentwiseRemarkConfigDetails.Add(student);
        }

        hidStudentwiaseRemarkListCount.Value = lstDistinctStudentwiseRemarkConfigDetails.Count.ToString();
        return lstDistinctStudentwiseRemarkConfigDetails;
    }

    /// <summary>
    /// This method is used to Populate Studentwise Remark Object.
    /// </summary>
    /// <returns></returns>
    private List<StudentwiseRemarkConfigDetails> PopulateStudentwiseRemarkList()
    {
        List<StudentwiseRemarkConfigDetails> lstStudentwiseRemarkConfigDetail = new List<StudentwiseRemarkConfigDetails>();
        StudentwiseRemarkConfigDetails oStudentwiseRemarkConfigDetails;

        foreach (ListViewDataItem oCurrentItem in lstvwStudentRemarkDetails.Items)
        {
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iRemarkCount = 0;
            while (iRemarkCount < mLstRemarkMaster.Count)
            {
                TextBox oTxtRemark = (TextBox)oCurrentItem.FindControl("txt" + mLstRemarkMaster[iRemarkCount].RemarkName.RemoveSingleQuote());
                if (oTxtRemark != null)
                {
                    oStudentwiseRemarkConfigDetails = new StudentwiseRemarkConfigDetails();
                    oStudentwiseRemarkConfigDetails.YearwiseStudentId = Convert.ToInt32(lstvwStudentRemarkDetails.DataKeys[iRowId]["YearwiseStudentId"]);
                    oStudentwiseRemarkConfigDetails.StudentwiseRemarkId = Convert.ToInt32(lstvwStudentRemarkDetails.DataKeys[iRowId]["StudentwiseRemarkId"]);
                    oStudentwiseRemarkConfigDetails.Remark = oTxtRemark.Text.Trim();
                    RemarkMaster oRemarkMaster = new RemarkMaster
                    {
                        RemarkConfigId = mLstRemarkMaster[iRemarkCount].RemarkConfigId
                    };                   
                    
                    oStudentwiseRemarkConfigDetails.RemarkMaster = oRemarkMaster;
                    if (cmbTermName.SelectedValue == Constants.I_TWO.ToString())
                        if (((RadioButton)oCurrentItem.FindControl("rdbtnPassedAndPromoted")) != null)
                            oStudentwiseRemarkConfigDetails.IsPassedAndPromoted = ((RadioButton)oCurrentItem.FindControl("rdbtnPassedAndPromoted")).Checked;
                    lstStudentwiseRemarkConfigDetail.Add(oStudentwiseRemarkConfigDetails);
                }
                iRemarkCount++;
            }
        }

        return lstStudentwiseRemarkConfigDetail;
    }

    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        cmbStudents.Attributes.Add("onchange", "if(!MessageAlert('" + cmbStudents.ClientID + "')){return false;}");
        cmbTeachers.Attributes.Add("onchange", "if(!MessageAlert('" + cmbTeachers.ClientID + "')){return false;}");
        cmbTermName.Attributes.Add("onchange", "if(!MessageAlert('" + cmbTermName.ClientID + "')){return false;}");
        btnBack.Attributes.Add("onclick", "if(!MessageAlert('" + btnBack.ClientID + "')){return false;}");
        ApplyMouseHoverEffect(new List<Button> { btnPopupSave, btnClosePopUp, btnBack, btnSave, btnShowReport, btnExport });        
    }

    /// <summary>
    /// This method is used to set list view according selected page from the pager dropdownlist.
    /// Pager control name should be same as defined here.
    /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
    /// Same for drop down list in the pager control as well as label
    /// </summary>
    private void SetDataPagerAccordingToPageNo()
    {
        DropDownList ocmbCount = DtPgDropDown.Controls[I_ZERO_INDEX].FindControl("ddlCnt") as DropDownList;
        int iRowIndex = (Convert.ToInt32(ocmbCount.SelectedValue) - Constants.I_ONE) * DtPgDropDown.PageSize;

        DtPgDropDown.SetPageProperties(iRowIndex, DtPgDropDown.PageSize, true);

        int iCurrentPage = (DtPgDropDown.StartRowIndex / DtPgDropDown.PageSize) + Constants.I_ONE;
        int iTotalPages = DtPgDropDown.TotalRowCount / DtPgDropDown.PageSize;

        Label lblCurrentPageLabel = DtPgDropDown.Controls[I_ZERO_INDEX].FindControl("CurrentPageLabel") as Label;
        lblCurrentPageLabel.Text = "Page " + iCurrentPage + " of " + iTotalPages;
    }

    /// <summary>
    /// This method is used to fill remark combo
    /// </summary>    
    private void FillRemarksCombo()
    {
        try
        {
            List<RemarksCategory> olstRemarkTemplateConfig = RemarksCategoryBL.GetConfig(miSchoolId, miAcademicYearId);
            ListSource.FillDropDownList(olstRemarkTemplateConfig, cmbRemarksOnDiv, "Name", "Id", Constants.S_ALL);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill Grades combo
    /// </summary> 
    private void FillGradesCombo()
    {        
        DataTable odtGradeDetails = MarksGradesConfigurationBL.GetAllGradesForStandard(miSchoolId, miAcademicYearId, 0, 0);
        cmbGradesOnDiv.Bind(odtGradeDetails, "Marks_Grades_Configuration_Detail_ID", "Grade_Name", Constants.S_ALL);              
    }

    /// <summary>
    /// This method is used to display templates
    /// </summary>
    private void DisplayTemplateRemarks()
    {
        RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
        lstvwTemplates.DataSource = oTemplateConfigurationBL.GetAll(miSchoolId, Convert.ToInt32(cmbRemarksOnDiv.SelectedValue), hidSortExpression.Value, hidSortDirection.Value, string.Empty, miAcademicYearId, Convert.ToInt32(cmbGradesOnDiv.SelectedValue), hidStandardId.Value.ToInt());
        lstvwTemplates.DataBind();
        
        if (lstvwTemplates.Items.Count > 0)
        {
            btnPopupSave.Enabled = true;
            AddSortImage();
        }
        else
            btnPopupSave.Enabled = false;
    }

    /// <summary>
    /// This method is used to add image for sorted column.
    /// </summary>
    private void AddSortImage()
    {

        HtmlTableRow oHtmlTableHeaderRow = lstvwTemplates.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            AddImageToHeader(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to enable or disable timer.
    /// </summary>
    /// <param name="abEnableTimer"></param>
    /// <param name="asTimerStart"></param>
    private void EnableDisableTimer(bool abEnableTimer)
    {
        timer.Enabled = abEnableTimer;
        hidTimerStart.Value = abEnableTimer ? Constants.S_YES : Constants.S_NO;
    }

    /// <summary>
    /// This method is sets the sortdirection according to previous derection
    /// </summary>
    private void SetSortDirection()
    {
        if (string.IsNullOrEmpty(hidSortDirection.Value) || hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to check published status of exam.
    /// </summary>
    private void CheckPublishedStatus()
    {
        bool bIsPublishedStatus;

        GetFinalExamPublishedStatus(out bIsPublishedStatus);
        if (bIsPublishedStatus == true)
        {
            lstvwStudentRemarkDetails.Enabled = false;
            btnSave.Enabled = false;
            btnShowReport.Text = S_SHOW_REPORT_BTNTXT;
        }
        else  
        {
            lstvwStudentRemarkDetails.Enabled = true;
            btnSave.Enabled = true;
            btnShowReport.Text = S_SAVE_SHOW_REPORT_BTNTXT;
        }
    
    }

    /// <summary>
    /// This method is used to read the Query String and set value to Teacher Combo.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StdDivId"] != Constants.S_ZERO && QueryString["StdDivId"] != null)
        {
            cmbTeachers.SelectedValue = QueryString["StdDivId"];            
            cmbTeachers_SelectedIndexChanged(cmbTeachers, null);
        }
        if (QueryString["ExamId"] != Constants.S_ZERO && QueryString["ExamId"] != null)
            hidTestId.Value = QueryString["ExamId"];
            hidIsPrimary.Value = QueryString["IsPrimary"];
    }

    /// <summary>
    /// This method is used to Add image to the header of column according to sort direction
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asSortExpression"></param>
    /// <param name="asSortDirection"></param>
    private void AddImageToHeader(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
    {
        if (asSortExpression.Trim().Equals(""))
            return;

        // Create the sorting image based on the sort direction.
        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
        sortImage.ID = "sortImage";
        if (asSortDirection == "asc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
            sortImage.AlternateText = "Ascending Order";
        }
        else if (asSortDirection == "desc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
            sortImage.AlternateText = "Descending Order";
        }
        // Iterate through the Columns collection to determine the index
        // of the column being sorted.
        foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
        {
            asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");

            // Iterate through the cells collection to determine the index
            // of the cell being sorted.
            foreach (Control oControl in oHtmlTableCell.Controls)
            {
                LinkButton oLinkButton = oControl as LinkButton;
                if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
                {
                    System.Web.UI.WebControls.Image oImage = (System.Web.UI.WebControls.Image)oHtmlTableCell.FindControl("sortImage");
                    if (oImage == null)
                    {
                        // Add the image to the appropriate header cell.
                        if (sortImage.ImageUrl != "")
                        {
                            oHtmlTableCell.Controls.Add(sortImage);
                            break;
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    ///  This class is used to create list view template dynamically.
    /// </summary>    
    public class ListViewRemarkConfigTemplate : ITemplate
    {
        private ListViewItemType lstvwItemType;
        private List<StudentwiseRemarkConfigDetails> lstRemarkDetail;
        private bool isAlterNateRow = false;
        public static event EventHandler ShowPopupHandler;
        private int miTermId;
        private int miSchoolId;
        private bool mbAllowStudentResultSelection;

        public ListViewRemarkConfigTemplate(ListViewItemType alstItemType, List<StudentwiseRemarkConfigDetails> alstRemarkDetails, bool isAlterNate, int aiTermId, bool abAllowStudentResultSelection, int aiSchoolId)
        {
            lstvwItemType = alstItemType;
            lstRemarkDetail = alstRemarkDetails;
            isAlterNateRow = isAlterNate;
            miTermId = aiTermId;
            mbAllowStudentResultSelection = abAllowStudentResultSelection;
            miSchoolId = aiSchoolId;
        }

        /// <summary>
        /// This method is used to create template structure and bind data to listview.
        /// </summary>
        /// <param name="aoContainer"></param>
        public void InstantiateIn(Control aoContainer)
        {
            if (lstvwItemType == ListViewItemType.DataItem)
            {
                Literal ltrlDataItemTr = new Literal();
                Literal ltrlDataItemTd = new Literal();
                Label lblRollNo = new Label();
                Literal ltrlDataItemName = new Literal();
                Literal ltrlDataItemTdClose = new Literal();
                Literal ltrlDataItemTrClose = new Literal();

                ltrlDataItemTr.Text = isAlterNateRow == false ? "<tr class='ClsGridRow'>" : "<tr class='ClsGridAltRow'>";
                ltrlDataItemTd.Text = "<td align ='center' width='60px'>";
                lblRollNo.ID = "lblRollNo";
                lblRollNo.Width = Unit.Pixel(60);
                ltrlDataItemTrClose.Text = "</td>";

                aoContainer.Controls.Add(ltrlDataItemTr);
                aoContainer.Controls.Add(ltrlDataItemTd);
                aoContainer.Controls.Add(lblRollNo);
                aoContainer.Controls.Add(ltrlDataItemTrClose);

                Literal ltrlDataItemTdName = new Literal();
                Label lblName = new Label();
                Literal ltrlDataItemTdNameClose = new Literal();

                ltrlDataItemTdName.Text = "<td style='padding-left:8px' width='200px'>";
                lblName.ID = "lblName";
                lblName.Width = Unit.Pixel(200);
                ltrlDataItemTdNameClose.Text = "</td>";
                ltrlDataItemTrClose.Text = "</tr>";

                Literal ltrlDataItemOldRemark = new Literal();
                Label lblOldRemark = new Label();
                Literal ltrlDataItemOldRemarkClose = new Literal();

                if (miSchoolId == Constants.SchoolId.PPS.ToInt() && miTermId == Constants.I_TWO)
                    ltrlDataItemOldRemark.Text = "<td style='padding-left:8px'>";
                else
                    ltrlDataItemOldRemark.Text = "<td>";

                lblOldRemark.ID = "LblOldRemark";                
                ltrlDataItemOldRemarkClose.Text = "</td>";
                
                aoContainer.Controls.Add(ltrlDataItemTdName);
                aoContainer.Controls.Add(lblName);
                aoContainer.Controls.Add(ltrlDataItemTdNameClose);

                aoContainer.Controls.Add(ltrlDataItemOldRemark);
                aoContainer.Controls.Add(lblOldRemark);
                aoContainer.Controls.Add(ltrlDataItemOldRemarkClose);
                

                aoContainer.Controls.Add(ltrlDataItemTrClose);                

                for (int iNo = 0; iNo < mLstRemarkMaster.Count; iNo++)
                {
                    Literal ltrltd = new Literal();
                    Literal ltrtdClose = new Literal();
                    ltrltd.Text = "<td align = 'center' width='470px'>";
                    ltrtdClose.Text = "</td>";                   


                    TextBox txtRemark = new TextBox();
                    txtRemark.ID = "txt" + mLstRemarkMaster[iNo].RemarkName.RemoveSingleQuote();
                    txtRemark.Width = Unit.Pixel(400);

                    if (miSchoolId == Constants.SchoolId.PPS.ToInt() && miTermId == Constants.I_TWO)
                        txtRemark.Height = Unit.Percentage(100);

                    txtRemark.TextMode = TextBoxMode.MultiLine;

                    //Here we add a button after each textbox control
                    Button btnShowpopup = new Button();
                    btnShowpopup.ID = "btn_" + mLstRemarkMaster[iNo].RemarkConfigId + "_" + mLstRemarkMaster[iNo].RemarkName.RemoveSingleQuote();
                    btnShowpopup.Width = Unit.Pixel(20);
                    btnShowpopup.Height = Unit.Pixel(30);
                    btnShowpopup.Style["vertical-align"] = VerticalAlign.Top.ToString();
                    btnShowpopup.Text = "...";
                    btnShowpopup.CommandName = "OpenPopup";
                    btnShowpopup.Click += new EventHandler(btnShowpopup_Click);

                    Label olblRemarksLength = new Label();
                    olblRemarksLength.ID = "lbl" + mLstRemarkMaster[iNo].RemarkName.RemoveSingleQuote();
                    olblRemarksLength.Style["vertical-align"] = VerticalAlign.Top.ToString();

                    aoContainer.Controls.Add(ltrltd);                    
                    aoContainer.Controls.Add(txtRemark);
                    aoContainer.Controls.Add(btnShowpopup);
                    aoContainer.Controls.Add(olblRemarksLength);
                    aoContainer.Controls.Add(ltrtdClose);
                }

                aoContainer.Controls.Add(ltrlDataItemTrClose);
                if (mbAllowStudentResultSelection && miTermId == Constants.I_TWO)
                {
                    RadioButton oRdBtnPassedAndPromoted = new RadioButton();
                    oRdBtnPassedAndPromoted.ID = "rdbtnPassedAndPromoted";
                    oRdBtnPassedAndPromoted.GroupName = "PassedAndPromoted";
                    oRdBtnPassedAndPromoted.Text = "Passed And Promoted";

                    RadioButton oRdBtnPromoted = new RadioButton();
                    oRdBtnPromoted.ID = "rdbtnPromoted";
                    oRdBtnPromoted.Text = "Promoted";
                    oRdBtnPromoted.GroupName = "PassedAndPromoted";

                    ltrlDataItemTd = new Literal();
                    ltrlDataItemTd.Text = "<td align ='center' style='width:250px'>";
                    ltrlDataItemTrClose.Text = "</td>";

                    aoContainer.Controls.Add(ltrlDataItemTd);
                    aoContainer.Controls.Add(oRdBtnPassedAndPromoted);
                    aoContainer.Controls.Add(oRdBtnPromoted);
                    aoContainer.Controls.Add(ltrlDataItemTrClose);
                }

            }
            else
            {
                string sValue = string.Empty;
                if (miSchoolId == Constants.SchoolId.PPS.ToInt() && miTermId == Constants.I_TWO)
                    sValue = "style='padding-left:8px; text-align:center;width:350px'>Old Remarks</th>";
                else
                    sValue = "style='width:0px'></th>";
                Literal ltrlHeadertbl = new Literal();
                ltrlHeadertbl.Text = "<table cellpadding='0' cellspacing='1' style='color: #333333' class='GridBorder' align='center'>";
                ltrlHeadertbl.Text += "<tr class='ClsGridHeader'><th align='center'>Roll No.</th><th align='left' style='padding-left:8px'>Name</th><th align='left'" + sValue;
                Literal ltrthClose = new Literal();
                ltrthClose.Text = "</th>";

                Literal ltrlHeadertrClose = new Literal();
                ltrlHeadertrClose.Text = "</tr>";

                aoContainer.Controls.Add(ltrlHeadertbl);               

                for (int iNo = Constants.I_ZERO; iNo < mLstRemarkMaster.Count; iNo++)
                {
                    Literal ltrlthHeader = new Literal();
                    ltrlthHeader.Text = "<th align='center'>" + mLstRemarkMaster[iNo].RemarkName.ToString() + "</th>";
                    aoContainer.Controls.Add(ltrlthHeader);
                }               

                if (mbAllowStudentResultSelection && miTermId == Constants.I_TWO)
                {
                    ltrlHeadertbl = new Literal();
                    //ltrlHeadertbl.Text = "<th align='left' style='padding-left:8px'><input id='ChkSelectAll' type='checkbox' runat='server' style='margin-left:2px' onclick='CheckUncheckAllCheckBoxes(this);' />Is Passed And Promoted?</th>";
                    ltrlHeadertbl.Text = "<th align='center' style='padding-left:8px'>Result</th>";
                    aoContainer.Controls.Add(ltrlHeadertbl);
                    aoContainer.Controls.Add(ltrlHeadertrClose);
                }

                Literal ltrlItemPlaceHolder = new Literal();
                ltrlItemPlaceHolder.ID = "itemPlaceholder";
                Literal ltrlHeadertblClose = new Literal();
                ltrlHeadertblClose.Text = "</table>";

                aoContainer.Controls.Add(ltrlItemPlaceHolder);
                aoContainer.Controls.Add(ltrlHeadertblClose);
            }
        }

        void btnShowpopup_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPopupHandler(sender, e);
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            }
        }
    }

    /// <summary>
    /// This method is used to set the Back button Post Back URL.
    /// </summary>
    private void SetPageBackValue()
    {
        if (hidIsPrimary.Value == Convert.ToString(Constants.I_ZERO))
            btnBack.Visible = true;
        else
            btnBack.Visible = false;
        string sQueryString = "TestId=" + hidTestId.Value + "&TeacherId=" + (hidIsPreprimaryStandard.Value == Constants.S_ONE ? Constants.S_ZERO : cmbTeachers.SelectedValue);
        string sEncrypt =  CommonUtility.EncryptQuerystring(sQueryString);
        btnBack.PostBackUrl = "~/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx" + "?" + sEncrypt;
     
    }

    #endregion

}