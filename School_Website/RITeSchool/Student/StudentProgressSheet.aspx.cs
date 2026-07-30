/*
 * Created By   :
 * Created date : * 
 *This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 28 Jan 2008
 * Date of modification: 2 Feb 2008 
 Modification Log:
 * Updated By   : Vinod
 * Updated Date : 1 Mar 2012
 * Log          : "Old Academic Year" link is now available for class teacher also.
 
 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;
using XseedReportEntities;
using System.Threading;
using CrystalDecisions.Shared;
using System.Web.Services;

/// <summary>
/// This class is used to display progress report of pre-primary/primary students.
/// </summary>
public partial class StudentProgressSheet : SchoolBase
{
    #region Constant

    private const string S_ERROR_MSG = "Class teacher not yet associated.";
    private const string S_NA = " N/A ";
    private const string S_FALSE = "False";
    private const string S_NO_EXAM_PUBLISH_MSG = "No exam of this class has been published for the current academic year.";
    private const int I_PPS_2022_23 = 53;
    private const int I_PPS_2023_24 = 54;
    private const int I_PPS_2025_26 = 56;
    #endregion

    #region Members

    private int miStdDivId = 0;
    private int miStandardId = 0;
    private int miStudentId = 0;    
    private int miTeacherId = 0;
    private bool mbIsTeacherLogin = false;
    private bool mbIsOldProgressReport;
    private PrePrimaryProgressSheetConfigBL moPrePrimaryProgressSheetConfigBL;
    
    List<string> mlstPioneerGradeReportStandards = new List<string> { "Nursery", "Junior KG", "Senior KG", "1", "2" };
    
    #endregion

    private bool ShowCurrentYearData
    {
        get
        {
            return hidShowCurrentYearData.Value == Constants.S_ONE;
        }
    }

    private string PendingFeeMessage
    {
        get
        {
            if (moSchool == Constants.SchoolId.PPSN)
                return "Please contact the school office.";
            else
                return Constants.S_FEES_PENDING_FOR_STUDENT_MSG;
        }
    }

    #region Events

    /// <summary>
    /// This event is used to set masterpage.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);			
			InitializeMembers();
            mbIsOldProgressReport = IsOldProgressReport();
            if (mbIsOldProgressReport)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method event is used to render student's progress report while first time page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{
            Page.Culture = "en";
			cmbTeachers.Focus();
            SetJavascriptAttributes();
			if (!IsPostBack)
			{
                hidShowCurrentYearData.Value = Constants.S_ZERO;
                if (QueryString["ShowCurrentYearData"] != null && QueryString["ShowCurrentYearData"].ToString() == Constants.S_ONE)
                    hidShowCurrentYearData.Value = Constants.S_ONE;

				IsXseedApplicable();
				trAcademicYear.Visible = false;
				ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
				if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] == null)
					Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = Constants.S_ZERO;
				tdhlnkToppers.Visible = false;
                btnDownload.Visible = false;
                btnDownloadTestReport.Visible = false;

				hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.ProgressReportConfig).ToString();

                if (moUserRole == Constants.UserRoles.Student && miSchoolId == Constants.SchoolId.SVP.ToInt())
                {
                    trSVPbtnDownload.Visible = true;
                    trbtnDonloadPDF.Visible = false;
                    trStudentProgressReport.Visible = false;
                    trGradeConfiguration.Visible = false;
                    btnPrint.Visible = false;
                }

				if (moUserRole == Constants.UserRoles.Admin
					|| bool.Parse(hidUserHasFullAccess.Value))
				{
					FillTeachersComboBox();
					VisibleHideTeacherCombo(true);
					VisibleHideGenerateButton(true);

					// This case occurs when Class Teacher is Login and has Full access, and looking Old Year Academic Year data (Student Progress report).
					if (mbIsTeacherLogin == true)
						DisplayStudentPregressReport();
				}

				// When Class Teacher is login but not having Full Access. miStudentId = 0 filter apply - because When first time page load that time Student combobox value is Zero.
				else if (moUserRole == Constants.UserRoles.Teacher && hidUserHasFullAccess.Value == S_FALSE && mbIsTeacherLogin == true && miStudentId.ToString() != Constants.S_ZERO)
					DisplayStudentPregressReport();
				else if (moUserRole == Constants.UserRoles.Teacher)
					DisplayTeacherProgressReport();
				else if (moUserRole == Constants.UserRoles.Student)
					DisplayStudentPregressReport();
				SetDefaultButton(btnShow);
                SetDownloadButtonState(true);
   			}
			btnCancelUp.Visible = false;
			btnPrint.Enabled = true;
		}
		catch (MarksNotAvailableForResult ex)
		{
			SetNoRecordMessage(ex);
			btnPrint.Visible = false;
			hlnkToppers.Visible = false;
            SetDownloadButtonState(false, true);
		}
		catch (NoResultFound ex)
		{
			SetNoRecordMessage(ex);
			lblmandatory.Visible = false;
			btnPrint.Visible = false;
            SetDownloadButtonState(false, true);
		}
		catch (SqlException ex)
		{
			SetNoRecordMessage(ex);
			btnPrint.Enabled = false;
            SetDownloadButtonState(false, true);
		}
		catch (BlockProgessReport ex)
		{
			SetNoRecordMessage(ex);
			btnPrint.Enabled = false;
            SetDownloadButtonState(false, true);
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex)
		{
			SetNoRecordMessage(ex);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
        finally 
        {
            if (!mbIsOldProgressReport)
                btnCancel.Visible = false;
        }
    }

    /// <summary>
    /// This event is used to display progress report according to academic year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAcademicYrId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            miAcademicYearId = Convert.ToInt32(cmbAcademicYrId.SelectedValue);
            hidLastAcademicYrId.Value = cmbAcademicYrId.SelectedValue;
            Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = miAcademicYearId.ToString();
            lblOldAcademicYear.Text = CommonUtility.DisplayAcademicYear(cmbAcademicYrId.SelectedItem.Text);
            IsXseedApplicable();
            DisplayProgressReportForStudent();
        }
        catch (MarksNotAvailableForResult ex)
        {
            SetNoRecordMessage(ex);
            btnPrint.Visible = false;
            hlnkToppers.Visible = false;
        }
        catch (SqlException ex)
        {
            SetNoRecordMessage(ex);
            btnPrint.Enabled = false;
        }
        catch (BlockProgessReport ex)
        {
            SetNoRecordMessage(ex);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            btnPrint.Enabled = false;
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
    /// This method event is used to navigate to control panel when user press cancel button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            HttpContext.Current.Session.Remove(Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to grid WithOut Subject html table cell.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdWithOutSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label olblMonth;
            Label olblRemark;
            GridView grdWithOutSubjects = (GridView)sender;
            int cnt = 0;
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.VerticalAlign = VerticalAlign.Middle;
                cell.HorizontalAlign = HorizontalAlign.Center;
                if (cnt > 0)
                {
                    cell.Controls.Clear();
                    olblMonth = new Label();
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        olblMonth.Text = " " + cell.Text + "<br />";
                        cell.Controls.Add(olblMonth);
                    }

                    olblRemark = new Label();

                    if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= Constants.I_ZERO)
                    {
                        string sRemark;
                        if (moPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].IsPublished)
                        {
                            PrePrimaryStudentsExamResult remarks = GetRemarkForMonth(e.Row.RowIndex, cnt);
                            sRemark = remarks.PrePrimaryRemarkId != Constants.I_ZERO ? GetRemarkName(remarks.PrePrimaryRemarkId) : S_NA;
                        }
                        else
                            sRemark = S_NA;
                        olblRemark.Text = sRemark;
                    }

                    cell.Controls.Add(olblRemark);
                    cell.Height = 30;
                }
                else
                {
                    if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
                    {
                        cell.Font.Bold = true;
                        cell.Font.Size = 10;
                    }
                }

                cnt++;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to grid With Subject html table cell.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdWithSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label olblMonth;
            Label olblRemark;
            GridView grdWithSubjects = (GridView)sender;
            int iCnt = 0;
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.VerticalAlign = VerticalAlign.Middle;
                cell.HorizontalAlign = HorizontalAlign.Center;
                if (iCnt > Constants.I_ONE)
                {
                    cell.Controls.Clear();
                    olblMonth = new Label();

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        olblMonth.Text = " " + cell.Text + "<br />";
                        cell.Controls.Add(olblMonth);
                    }

                    olblRemark = new Label();
                    if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= Constants.I_ZERO)
                    {
                        string sRemark;
                        if (moPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[iCnt - 2].IsPublished)
                        {
                            PrePrimaryStudentsExamResult remarks = GetRemarkForMonthWithSubject(e.Row.RowIndex, iCnt);
                            sRemark = remarks.PrePrimaryRemarkId != Constants.I_ZERO ? GetRemarkName(remarks.PrePrimaryRemarkId) : S_NA;
                        }
                        else
                            sRemark = S_NA;
                        olblRemark.Text = sRemark;
                    }

                    cell.Controls.Add(olblRemark);
                    cell.Height = 30;
                }
                else if (iCnt == Constants.I_ZERO)
                {
                    if (hidSubName.Value != cell.Text)
                    {
                        if (hidRowNo.Value != "-1")
                        {
                            grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].RowSpan = hidRowSpan.Value.ToInt();
                            grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].Text = hidSubName.Value;
                        }

                        hidSubName.Value = cell.Text;
                        cell.Font.Bold = true;
                        hidRowNo.Value = e.Row.RowIndex.ToString();
                        hidRowSpan.Value = Constants.S_ONE;
                    }
                    else
                    {
                        cell.Text = string.Empty;
                        cell.Visible = false;
                        hidRowSpan.Value = (hidRowSpan.Value.ToInt() + 1).ToString();

                        if (moPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.Count() == (e.Row.RowIndex + 1))
                        {
                            grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].RowSpan = hidRowSpan.Value.ToInt();
                            grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].Text = hidSubName.Value;
                        }
                    }
                }
                else
                {
                    if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= Constants.I_ZERO)
                    {
                        cell.Font.Bold = true;
                        cell.Font.Bold = true;
                        cell.Font.Size = 10;
                    }
                }

                iCnt++;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event method is used to show progress sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = miAcademicYearId;
            DataTable odtTeachers = TeacherStandardDetailsCollectionBL.GetTeachersForPrePrimaryProgressReport((cmbTeachers.SelectedValue.ToInt() > Constants.I_ZERO)
                                                                                                                    ? cmbTeachers.SelectedValue.ToInt()
                                                                                                                    : Session[Constants.S_SESSION_TEACHER_ID].ToInt(),
                                                                                                             miSchoolId,
                                                                                                             miAcademicYearId);            

            if (odtTeachers.Rows.Count > Constants.I_ZERO)
            {
                if (cmbStudents.SelectedValue == Constants.S_ZERO)
                {
                    StudentProgress oStudentProgress = new StudentProgress();
					DataTable odtStudents = GetStudentData((cmbTeachers.SelectedValue.ToInt() > Constants.I_ZERO)
																													? cmbTeachers.SelectedValue.ToInt()
																													: Session[Constants.S_SESSION_TEACHER_ID].ToInt());
                    for (int i = 0; i < odtStudents.Rows.Count; i++)
                        DisplayProgresReport(odtStudents.Rows[i]["Student_Id"].ToInt());
                }
                else
                    DisplayProgresReport(cmbStudents.SelectedValue.ToInt());
            }
            else
                ShowStudProgressSheet();            
            SetUrlToLinkButton();
        }
        catch (MarksNotAvailableForResult ex)
        {
            SetNoRecordMessage(ex);
            btnPrint.Enabled = false;
        }
        catch (NoResultFound ex)
        {
            SetNoRecordMessage(ex);
            btnPrint.Enabled = false;
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            SetNoRecordMessage(ex);
            btnPrint.Enabled = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show student of class for selected class teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lnkbtnGradeConfigurationDetails.Visible = false;
            hlnkOldAcademicRecord.Visible = false;
            int iTecherId = cmbTeachers.SelectedValue.ToInt();
			DataTable oDtStudents = GetStudentData(iTecherId);
			FillStudentsComboBox(GetStudentData(iTecherId));
            AddPrintAttributes();
            if (iTecherId != Constants.I_ZERO)
                SetToppersLinkURL();
            else
                hlnkToppers.Enabled = false;
            if (cmbTeachers.SelectedValue != Constants.S_ZERO && oDtStudents != null && oDtStudents.Rows.Count>Constants.I_ZERO && hidUserHasFullAccess.Value == "True")				
				hidStandardId.Value = oDtStudents.Rows[0][0].ToString();
			if (moUserRole == Constants.UserRoles.Teacher)
				VisibleHideGenerateButton(true);
        }
        catch (Exception ex)
        {
            SetNoRecordMessage(ex);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to download the progress report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = null;
            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            {
                StudentProgress oStudentProgress = new StudentProgress();
                bool IsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);

                int iAccYearId = miAcademicYearId;

                if (mbIsOldProgressReport)
                {
                    if (cmbAcademicYrId.SelectedValue != string.Empty)
                        iAccYearId = cmbAcademicYrId.SelectedValue.ToInt();
                }

                if (iAccYearId >= 14 && (hidStandardName.Value == "3" || hidStandardName.Value == "4" || hidStandardName.Value == "5"))
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.HolosticProgressReportPPSNFor3to5, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
                else if (hidStandardName.Value == "1" || hidStandardName.Value == "2" || hidStandardName.Value == "3" || hidStandardName.Value == "4" || hidStandardName.Value == "5")
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_1stTO5th, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
                else if (hidStandardName.Value == "6" || hidStandardName.Value == "7" || hidStandardName.Value == "8") 
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
                else if (hidStandardName.Value == "9" && IsFinalExamPublished && iAccYearId >= 11)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_9th, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
                else if (hidStandardName.Value == "9" && IsFinalExamPublished)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentFinalProgressReport9thStd_PPSH_AY10, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
                else if (hidStandardName.Value == "9" && !IsFinalExamPublished)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_9th, GetFilterString(false, false), ExportFormatType.PortableDocFormat);

                oReportDisplay.AcademicYearId = iAccYearId;
            }
            else if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseTermProgressReportSNS_1rdTO5th2024, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
            else if (miSchoolId == Constants.SchoolId.PKIS.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPKIS, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
            else if (miSchoolId == Constants.SchoolId.SVP.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportSVP, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
            else if (miSchoolId == Constants.SchoolId.MNS.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportMNS, GetFilterString(false, false), ExportFormatType.PortableDocFormat);
            else if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                oReportDisplay = DisplayPPSReport(false);
            }
            else if (miSchoolId == Constants.SchoolId.NPS.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.FinalProgressReportNPS, GetFilterString(false, false), ExportFormatType.PortableDocFormat);

            else if (miSchoolId == Constants.SchoolId.PIONEER.ToInt())
            {
                int iAccYearId = miAcademicYearId;
                if (cmbAcademicYrId.SelectedValue != string.Empty)
                    iAccYearId = cmbAcademicYrId.SelectedValue.ToInt();

                DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, iAccYearId, Session[Constants.S_SESSION_STUDENT_ID].ToInt());
                hidOldStudentId.Value = oDatatable.Rows[0]["YearWise_Student_Id"].ToString();

                int iAcademicYearId, iStdDivId, iStudentId, iStdid;
                if (mbIsOldProgressReport)
                {
                    iAcademicYearId = cmbAcademicYrId.SelectedValue.ToInt();
                    iStdDivId = oDatatable.Rows[0]["Schoolwise_Standard_Division_Id"].ToInt();
                    iStudentId = oDatatable.Rows[0]["YearWise_Student_Id"].ToInt();
                    iStdid = oDatatable.Rows[0]["Standard_Id"].ToInt();
                }
                else
                {
                    iAcademicYearId = miAcademicYearId;
                    iStdDivId = miStdDivId;
                    iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
                    iStdid = hidStandardId.Value.ToInt();
                }

                StudentProgress oStudentProgress = new StudentProgress();
                bool IsFinalExamPublished = oStudentProgress.IsFinalResultPublished(iStdDivId);
                int TermId = IsFinalExamPublished ? 2 : 1;

               string sFilterString = GetFilterStringForPioneer(TermId, iAcademicYearId, iStudentId, iStdid, iStdDivId);

               if (mlstPioneerGradeReportStandards.Contains(hidStandardName.Value))
               {
                   oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPioneer_NurseryTO2nd, sFilterString, ExportFormatType.PortableDocFormat);
                   oReportDisplay.TermId = TermId;
               }
               else
               {
                   if (TermId == 1)
                       oReportDisplay = new ReportDisplay(Constants.ExportReports.HalfYearlyReportFor3To9Pioneer, sFilterString, ExportFormatType.PortableDocFormat);
                   else
                       oReportDisplay = new ReportDisplay(Constants.ExportReports.FinalProgressCardForPioneer3To9, sFilterString, ExportFormatType.PortableDocFormat);
               }
            }
            
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to download progress report for SVP School.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSVPDownload_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = null;
            StudentProgress oStudentProgress = new StudentProgress();
            bool IsFinalExamPublished;
            IsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);

            if (miSchoolId == Constants.SchoolId.SVP.ToInt())
            {
                if (IsFinalExamPublished)
                {
                    if(hidStandardName.Value == "9")
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportSVP_9, GetFilterStringForSVPSchool(IsFinalExamPublished), ExportFormatType.PortableDocFormat);
                    else
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportSVP, GetFilterStringForSVPSchool(IsFinalExamPublished), ExportFormatType.PortableDocFormat);
                }
                else
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseTermProgressReportSVP, GetFilterStringForSVPSchool(IsFinalExamPublished), ExportFormatType.PortableDocFormat);
            }
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to get the filter string for the dowmload PDF.
    /// </summary>
    private string GetFilterString(bool bIsGradeingReport, bool bIsFinalExamPublished)
    {
        string sFilterStr = string.Empty;
        int iAcademicYearId;
        int iStdDivId;
        int iStudentId;
        int iStdid;
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            DataTable oDatatable = new DataTable();
            //oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, hidLastAcademicYrId.Value.ToInt(), Session[Constants.S_SESSION_STUDENT_ID].ToInt());

            int iAccYearId = miAcademicYearId;
            if (cmbAcademicYrId.SelectedValue != string.Empty)
                iAccYearId = cmbAcademicYrId.SelectedValue.ToInt();

            oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, iAccYearId, Session[Constants.S_SESSION_STUDENT_ID].ToInt());
            hidOldStudentId.Value = oDatatable.Rows[0]["YearWise_Student_Id"].ToString();

            if (mbIsOldProgressReport)
            {
                //iAcademicYearId = hidLastAcademicYrId.Value.ToInt();
                //iStdDivId = hidOldStdDivId.Value.ToInt();
                //iStudentId = hidOldStudentId.Value.ToInt();

                iAcademicYearId = cmbAcademicYrId.SelectedValue.ToInt();
                iStdDivId = oDatatable.Rows[0]["Schoolwise_Standard_Division_Id"].ToInt();
                iStudentId = oDatatable.Rows[0]["YearWise_Student_Id"].ToInt();
                iStdid = oDatatable.Rows[0]["Standard_Id"].ToInt();
            }
            else
            {
                iAcademicYearId = miAcademicYearId;
                iStdDivId = miStdDivId;
                iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
                iStdid = hidStandardId.Value.ToInt();
            }

            StudentProgress oStudentProgress = new StudentProgress();
            bool bIsFinalPublished = oStudentProgress.IsFinalResultPublished(iStdDivId);

            if (iAcademicYearId>=14 && (hidStandardName.Value == "3" || hidStandardName.Value == "4" || hidStandardName.Value == "5"))
                sFilterStr = "(usp_GetDetailsForHolisticReportFor3To5Std.School_Id}=" + miSchoolId + "AND usp_GetDetailsForHolisticReportFor3To5Std.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetDetailsForHolisticReportFor3To5Std.Student_Id}=" + iStudentId + "AND usp_GetDetailsForHolisticReportFor3To5Std.Standard_Id}=" + iStdid + "AND usp_GetDetailsForHolisticReportFor3To5Std.Term_Id}=" + (bIsFinalPublished?2:1) + " AND usp_GetDetailsForHolisticReportFor3To5Std.IsFromReportScreen}=0AND usp_GetDetailsForHolisticReportFor3To5Std.Division_Id}=" + iStdDivId + ") @";
            else if (hidStandardName.Value == "6" || hidStandardName.Value == "7" || hidStandardName.Value == "8")
                sFilterStr = "(usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.StudentId}=" + iStudentId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Standard_Id}=" + iStdid + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Term_Id}=1AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.IsFromStudnetLogin}=1AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Division_Id}=" + iStdDivId + ") @";
            else if(hidStandardName.Value == "9")
                sFilterStr = "(usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.StudentId}=" + iStudentId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.IsFromStudentLogin}=1AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Standard_Id}=" + iStdid + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Division_Id}=" + iStdDivId + ") @";
            else if (hidStandardName.Value == "1" || hidStandardName.Value == "2" || hidStandardName.Value == "3" || hidStandardName.Value == "4" || hidStandardName.Value == "5")
                sFilterStr = "(usp_GetStudentObservationDetailsForReport_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetStudentObservationDetailsForReport_PPSH.StudentId}=" + iStudentId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Standard_Id}=" + iStdid + "AND usp_GetStudentObservationDetailsForReport_PPSH.IsFromStudentLogin}=1 AND usp_GetStudentObservationDetailsForReport_PPSH.Term_Id}="+(bIsFinalPublished?2:1)+" AND usp_GetStudentObservationDetailsForReport_PPSH.Division_Id}=" + iStdDivId + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            sFilterStr = "(usp_StudentTermProgressReportForSNS1TO5.School_Id}=" + miSchoolId + " AND usp_StudentTermProgressReportForSNS1TO5.Academic_Year_Id}=" + miAcademicYearId + " AND usp_StudentTermProgressReportForSNS1TO5.Standard_Id}=" + hidStandardId.Value + " AND usp_StudentTermProgressReportForSNS1TO5.Schoolwise_Standard_Division_Id}=" + miStdDivId + " AND usp_StudentTermProgressReportForSNS1TO5.Student_Id}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + " AND usp_StudentTermProgressReportForSNS1TO5.Term_Id}=" + Constants.I_ONE + " AND usp_StudentTermProgressReportForSNS1TO5.IsOpenFromReportScreen}=" + 0 +") @";
        //if (miSchoolId == Constants.SchoolId.SVP.ToInt())
        //{
        //    StudentProgress oStudentProgress = new StudentProgress();
        //    bool IsFinalExamPublished;
        //    IsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);

        //    if (IsFinalExamPublished)
        //        sFilterStr = "(USP_StudentProgressReportSVP.Note}=" + string.Empty + "AND USP_StudentProgressReportSVP.Term_Id}=" + Constants.I_TWO + "AND USP_StudentProgressReportSVP.School_Id}=" + miSchoolId + " AND USP_StudentProgressReportSVP.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentProgressReportSVP.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentProgressReportSVP.Division_Id}=" + miStdDivId + " AND USP_StudentProgressReportSVP.IsFromReportScreen}=0 AND USP_StudentProgressReportSVP.Academic_Year_Id}=" + miAcademicYearId + ") @";
        //    else
        //        sFilterStr = "(USP_StudentTermProgressReportSVP.Note}=" + string.Empty + "AND USP_StudentTermProgressReportSVP.Term_Id}=" + Constants.I_ONE + "AND USP_StudentTermProgressReportSVP.School_Id}=" + miSchoolId + " AND USP_StudentTermProgressReportSVP.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentTermProgressReportSVP.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentTermProgressReportSVP.Division_Id}=" + miStdDivId + " AND USP_StudentTermProgressReportSVP.IsFromReportScreen}=0 AND USP_StudentTermProgressReportSVP.Academic_Year_Id}=" + miAcademicYearId + ") @";
        //}
        else if (miSchoolId == Constants.SchoolId.PKIS.ToInt())
        {
            StudentProgress oStudentProgress = new StudentProgress();
            bool IsFinalExamPublished;
            IsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);

            if (IsFinalExamPublished)
                sFilterStr = "(USP_StudentProgressReportPKSC.School_Id}=" + miSchoolId + "AND USP_StudentProgressReportPKSC.Academic_Year_Id}=" + miAcademicYearId + "AND USP_StudentProgressReportPKSC.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentProgressReportPKSC.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentProgressReportPKSC.Division_Id}=" + miStdDivId + "AND USP_StudentProgressReportPKSC.Term_Id}=" + Constants.I_TWO + "AND USP_StudentProgressReportPKSC.Note}=" + string.Empty + " AND USP_StudentProgressReportPKSC.IsFromReportScreen}=1" + ") @";
            else
                sFilterStr = "(USP_StudentProgressReportPKSC.School_Id}=" + miSchoolId + "AND USP_StudentProgressReportPKSC.Academic_Year_Id}=" + miAcademicYearId + "AND USP_StudentProgressReportPKSC.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentProgressReportPKSC.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentProgressReportPKSC.Division_Id}=" + miStdDivId + "AND USP_StudentProgressReportPKSC.Term_Id}=" + Constants.I_ONE + "AND USP_StudentProgressReportPKSC.Note}=" + string.Empty + " AND USP_StudentProgressReportPKSC.IsFromReportScreen}=1" + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.PPS.ToInt())
        {           

            if (mbIsOldProgressReport)
            {
                iAcademicYearId = hidLastAcademicYrId.Value.ToInt();
                iStdDivId = hidOldStdDivId.Value.ToInt();
                iStudentId = hidOldStudentId.Value.ToInt();
            }
            else
            {
                iAcademicYearId = miAcademicYearId;
                iStdDivId = miStdDivId;
                iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
            }

            if (!bIsGradeingReport)
            {
                if (bIsFinalExamPublished)
                {
                    if (iAcademicYearId >= I_PPS_2022_23)
                        sFilterStr = "(usp_GetFinalProgressReportDetailsForPP.School_Id}=" + miSchoolId + "AND usp_GetFinalProgressReportDetailsForPP.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetFinalProgressReportDetailsForPP.Standard_Id}=" + hidStandardId.Value + " AND usp_GetFinalProgressReportDetailsForPP.Division_Id}=" + iStdDivId + "AND usp_GetFinalProgressReportDetailsForPP.StudentId}=" + iStudentId + "AND usp_GetFinalProgressReportDetailsForPP.Note}=" + string.Empty + ") @";
                    else
                        sFilterStr = "(usp_GetFinalProgressReport.School_Id}=" + miSchoolId + "AND usp_GetFinalProgressReport.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetFinalProgressReport.Standard_Id}=" + hidStandardId.Value + " AND usp_GetFinalProgressReport.Division_Id}=" + iStdDivId + "AND usp_GetFinalProgressReport.StudentId}=" + iStudentId + "AND usp_GetFinalProgressReport.Note}=" + string.Empty + ") @";
                }
                else
                {
                    if (iAcademicYearId <= 52)
                        sFilterStr = "(usp_GetMarkingSystemProgressReport.School_Id}=" + miSchoolId + "AND usp_GetMarkingSystemProgressReport.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetMarkingSystemProgressReport.Standard_Id}=" + hidStandardId.Value + " AND usp_GetMarkingSystemProgressReport.Division_Id}=" + iStdDivId + "AND usp_GetMarkingSystemProgressReport.StudentId}=" + iStudentId + "AND usp_GetMarkingSystemProgressReport.Term_Id}=" + Constants.I_ONE + "AND usp_GetMarkingSystemProgressReport.Note}=" + string.Empty + "AND usp_GetMarkingSystemProgressReport.IsFromReportScreen}=0" + ") @";
                    else
                        sFilterStr = "(usp_GetTerm1ProgressReportDetailsForPP.School_Id}=" + miSchoolId + "AND usp_GetTerm1ProgressReportDetailsForPP.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetTerm1ProgressReportDetailsForPP.Standard_Id}=" + hidStandardId.Value + " AND usp_GetTerm1ProgressReportDetailsForPP.Division_Id}=" + iStdDivId + "AND usp_GetTerm1ProgressReportDetailsForPP.StudentId}=" + iStudentId + "AND usp_GetTerm1ProgressReportDetailsForPP.Term_Id}=" + Constants.I_ONE + "AND usp_GetTerm1ProgressReportDetailsForPP.Note}=" + string.Empty + "AND usp_GetTerm1ProgressReportDetailsForPP.IsFromReportScreen}=0" + ") @";
                }
            }
            else
            {
                if (bIsFinalExamPublished)
                    sFilterStr = "(usp_GetFinalProgressReport.School_Id}=" + miSchoolId + "AND usp_GetFinalProgressReport.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetFinalProgressReport.Standard_Id}=" + hidStandardId.Value + " AND usp_GetFinalProgressReport.Division_Id}=" + iStdDivId + "AND usp_GetFinalProgressReport.StudentId}=" + iStudentId + "AND usp_GetFinalProgressReport.Note}=" + string.Empty + ") @";
                else
                    sFilterStr = "(usp_GetGradingSystemProgressReport.School_Id}=" + miSchoolId + "AND usp_GetGradingSystemProgressReport.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetGradingSystemProgressReport.Standard_Id}=" + hidStandardId.Value + " AND usp_GetGradingSystemProgressReport.Division_Id}=" + iStdDivId + "AND usp_GetGradingSystemProgressReport.StudentId}=" + iStudentId + "AND usp_GetGradingSystemProgressReport.Term_Id}=" + Constants.I_ONE + "AND usp_GetGradingSystemProgressReport.Note}=" + string.Empty + "AND usp_GetGradingSystemProgressReport.IsFromReportScreen}=0" + ") @";
            }

        }
        else if (miSchoolId == Constants.SchoolId.NPS.ToInt())
            sFilterStr = "(usp_StudentwiseProgressReportForNPS.School_Id}=" + miSchoolId + "AND usp_StudentwiseProgressReportForNPS.Academic_Year_Id}=" + miAcademicYearId + "AND usp_StudentwiseProgressReportForNPS.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND usp_StudentwiseProgressReportForNPS.Standard_Id}=" + hidStandardId.Value + " AND usp_StudentwiseProgressReportForNPS.Division_Id}=" + miStdDivId + "AND usp_StudentwiseProgressReportForNPS.Term_Id}=" + Constants.I_TWO + "AND usp_StudentwiseProgressReportForNPS.Note}=" + string.Empty + ") @";
        else if (miSchoolId == Constants.SchoolId.MNS.ToInt())
            sFilterStr = "(usp_StudentProgressReportForMNS.School_Id}=" + miSchoolId + "AND usp_StudentProgressReportForMNS.Academic_Year_Id}=" + miAcademicYearId + "AND usp_StudentProgressReportForMNS.Student_Id}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND usp_StudentProgressReportForMNS.Standard_Id}=" + hidStandardId.Value + " AND usp_StudentProgressReportForMNS.Schoolwise_Standard_Division_Id}=" + miStdDivId + "AND usp_StudentProgressReportForMNS.Term_Id}=" + Constants.I_ONE+") @";

        return sFilterStr;
    }

    private string GetFilterStringForPioneer(int aiTermId, int aiAcademicYearId, int aiStudentId, int aiStdid, int aiStdDivId)
    {        
        string sFilterStr = string.Empty;
        if (!mlstPioneerGradeReportStandards.Contains(hidStandardName.Value))
        {
            if (aiTermId == 1)
                sFilterStr = "(usp_GetDetailsForHalfYearlyReport_Pioneer.School_Id}=" + miSchoolId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.StudentId}=" + aiStudentId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Standard_Id}=" + aiStdid + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Division_Id}=" + aiStdDivId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Term_Id}=" + Constants.I_ONE + " AND usp_GetDetailsForHalfYearlyReport_Pioneer.IsFromReportScreen}=0" + ") @";
            else
                sFilterStr = "(USP_StudentFinalProgressReportCBSEForPioneer.School_Id}=" + miSchoolId + "AND USP_StudentFinalProgressReportCBSEForPioneer.Academic_Year_Id}=" + aiAcademicYearId + "AND USP_StudentFinalProgressReportCBSEForPioneer.StudentId}=" + aiStudentId + "AND USP_StudentFinalProgressReportCBSEForPioneer.Standard_Id}=" + aiStdid + "AND USP_StudentFinalProgressReportCBSEForPioneer.Division_Id}=" + aiStdDivId + "AND USP_StudentFinalProgressReportCBSEForPioneer.Term_Id}=" + Constants.I_TWO + " AND usp_GetDetailsForHalfYearlyReport_Pioneer.Note}=AND usp_GetDetailsForHalfYearlyReport_Pioneer.IsFromReportScreen}=0" + ") @";
        }
        else
            sFilterStr = "(usp_GetProgressReportDetailsForPrePrimaryPioneer.School_Id}=" + miSchoolId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.Standard_Id}=" + aiStdid + " AND usp_GetProgressReportDetailsForPrePrimaryPioneer.Division_Id}=" + aiStdDivId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.StudentId}=" + aiStudentId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.TestId}=" + 0 + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.IsFromReportScreen}=0" + ") @";

        return sFilterStr;
    }

    /// <summary>
    /// This method is used to get filter string for report display for SVP school.
    /// </summary>
    /// <param name="bIsExamPublished"></param>
    /// <returns></returns>
    private string GetFilterStringForSVPSchool(bool bIsFinalExamPublished)
    {
        string sFilterStr = string.Empty;

        if (bIsFinalExamPublished)
        {
            if(hidStandardName.Value == "9")
                sFilterStr = "(USP_StudentFinalProgressReportSVP_9.Note}=" + string.Empty + "AND USP_StudentFinalProgressReportSVP_9.Term_Id}=" + Constants.I_TWO + "AND USP_StudentFinalProgressReportSVP_9.School_Id}=" + miSchoolId + " AND USP_StudentFinalProgressReportSVP_9.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentFinalProgressReportSVP_9.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentFinalProgressReportSVP_9.Division_Id}=" + miStdDivId + " AND USP_StudentFinalProgressReportSVP_9.Academic_Year_Id}=" + miAcademicYearId + ") @";
            else
                sFilterStr = "(USP_StudentProgressReportSVP.Note}=" + string.Empty + "AND USP_StudentProgressReportSVP.Term_Id}=" + Constants.I_TWO + "AND USP_StudentProgressReportSVP.School_Id}=" + miSchoolId + " AND USP_StudentProgressReportSVP.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentProgressReportSVP.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentProgressReportSVP.Division_Id}=" + miStdDivId + " AND USP_StudentProgressReportSVP.IsFromReportScreen}=0 AND USP_StudentProgressReportSVP.Academic_Year_Id}=" + miAcademicYearId + ") @";
        }
        else
            sFilterStr = "(USP_StudentTermProgressReportSVP.Note}=" + string.Empty + "AND USP_StudentTermProgressReportSVP.Term_Id}=" + Constants.I_ONE + "AND USP_StudentTermProgressReportSVP.School_Id}=" + miSchoolId + " AND USP_StudentTermProgressReportSVP.StudentId}=" + Session[Constants.S_SESSION_STUDENT_ID].ToInt() + "AND USP_StudentTermProgressReportSVP.Standard_Id}=" + hidStandardId.Value + " AND USP_StudentTermProgressReportSVP.Division_Id}=" + miStdDivId + " AND USP_StudentTermProgressReportSVP.IsFromReportScreen}=0 AND USP_StudentTermProgressReportSVP.Academic_Year_Id}=" + miAcademicYearId + ") @";

        return sFilterStr;
    }

	/// <summary>
	/// This method is used to get student data for provided standard division id.
	/// </summary>
	/// <param name="aiStdDivId"></param>
	/// <returns></returns>
	private DataTable GetStudentData(int aiStdDivId)
	{
		StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
		DataTable oDtStudents = oStudentwiseRemarkMasterBL.GetStudentListOfGivenClassTeacher(aiStdDivId, miAcademicYearId, miSchoolId, Constants.I_ZERO);
		return oDtStudents;
	}

    /// <summary>
    /// This method is used to show data on student cmb change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lnkbtnGradeConfigurationDetails.Visible = false;
            hidStudId.Value = cmbStudents.SelectedValue;
            if (moUserRole == Constants.UserRoles.Teacher && hidUserHasFullAccess.Value == S_FALSE && string.Equals(Session[Constants.S_SESSION_IS_CLASS_TEACHER], Constants.S_YES))
                hidStandardId.Value = Session[Constants.S_SESSION_TEACHER_STANDARD_ID].ToString();
            if (cmbStudents.SelectedValue != Constants.S_ZERO && moUserRole == Constants.UserRoles.Teacher && string.Equals(Session[Constants.S_SESSION_IS_CLASS_TEACHER], Constants.S_YES))
                SetOldYearRecordLinkVisibility();
            else
                trAcademicYear.Visible = false;
            AddPrintAttributes();
        }
        catch (Exception ex)
        {
            SetNoRecordMessage(ex);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to download PPS final progress report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDowloadTerm2Report_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = DisplayPPSReport(true);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnDownloadPrelimReport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, hidLastAcademicYrId.Value.ToInt(), Session[Constants.S_SESSION_STUDENT_ID].ToInt());
            hidOldStudentId.Value = oDatatable.Rows[0]["YearWise_Student_Id"].ToString();

            int iAcademicYearId;
            int iStdDivId;
            int iStudentId;
            int iStandardId;

            if (mbIsOldProgressReport)
            {
                iAcademicYearId = hidLastAcademicYrId.Value.ToInt();
                iStdDivId = hidOldStdDivId.Value.ToInt();
                iStudentId = hidOldStudentId.Value.ToInt();
                iStandardId = hidStandardId.Value.ToInt();
            }
            else
            {
                iAcademicYearId = miAcademicYearId;
                iStdDivId = miStdDivId;
                iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
                iStandardId = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
            }

            if (moSchool == Constants.SchoolId.PPS && iAcademicYearId <= 52)
            {
                string sFilterStr = "(usp_GetPreliminaryExaminationProgressReport.School_Id}=" + miSchoolId + "AND usp_GetPreliminaryExaminationProgressReport.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetPreliminaryExaminationProgressReport.Standard_Id}=" + iStandardId + " AND usp_GetPreliminaryExaminationProgressReport.Division_Id}=" + iStdDivId + "AND usp_GetPreliminaryExaminationProgressReport.StudentId}=" + iStudentId + "AND usp_GetPreliminaryExaminationProgressReport.IsFromReportScreen}=0 AND usp_GetPreliminaryExaminationProgressReport.Note}=" + string.Empty + ") @";
                ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.PrelimReport, sFilterStr, ExportFormatType.PortableDocFormat);
                oReportDisplay.AcademicYearId = iAcademicYearId;
                oReportDisplay.DisplayReport();
            }
            else
            {
                string sFilterStr = "(usp_GetPrelimProgressReportForPP.School_Id}=" + miSchoolId + "AND usp_GetPrelimProgressReportForPP.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetPrelimProgressReportForPP.Standard_Id}=" + iStandardId + " AND usp_GetPrelimProgressReportForPP.Division_Id}=" + iStdDivId + "AND usp_GetPrelimProgressReportForPP.StudentId}=" + iStudentId + "AND usp_GetPrelimProgressReportForPP.IsFromReportScreen}=0 AND usp_GetPrelimProgressReportForPP.Note}=" + string.Empty + ") @";
                ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.PrelimReportPP, sFilterStr, ExportFormatType.PortableDocFormat);
                oReportDisplay.AcademicYearId = iAcademicYearId;
                oReportDisplay.DisplayReport();
            }

        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Private Method

    /// <summary>
    /// This method is used to Get Remark For Month With Subject.
    /// </summary>
    /// <param name="aiRowNo"></param>
    /// <param name="aiColNo"></param>
    /// <returns></returns>
    private PrePrimaryStudentsExamResult GetRemarkForMonthWithSubject(int aiRowNo, int aiColNo)
    {
        PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = moPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[aiColNo - 2];
        PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = moPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects[aiRowNo];
        PrePrimaryStudentsExamResult oRemarks = (from remark in moPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamResultWithSubjects.AsParallel().AsEnumerable()
                                                 where remark.PreprimaryExamConfigurationId == oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId
                                                       && remark.PrePrimaryProgressReportSubSubjectId == oPrePrimaryProgressReportSubSubjects.SubSubjectID
                                                       && remark.PrePrimarySubjectId == oPrePrimaryProgressReportSubSubjects.SubjectID
                                                 select new PrePrimaryStudentsExamResult
                                                 {
                                                     PrePrimaryRemarkId = remark.PrePrimaryRemarkId,
                                                     PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                                                     PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
                                                 }).FirstOrDefault();

        if (oRemarks != null)
            return oRemarks;
        else
        {
            oRemarks = new PrePrimaryStudentsExamResult
            {
                PrePrimaryRemarkId = 0,
                PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
            };

            return oRemarks;
        }
    }

    /// <summary>
    /// THis method is used to Get Remark Name.
    /// </summary>
    /// <param name="aiRemarkId"></param>
    /// <returns></returns>
    private string GetRemarkName(int aiRemarkId)
    {
        string sRmrkName = (from remark in moPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig
                            where remark.PrePrimaryProgressReportRemarkId == aiRemarkId
                            select remark.PrePrimaryProgressReportRemarkName).FirstOrDefault();
        return sRmrkName;
    }

    /// <summary>
    /// This method is used to Get Remark For Month.
    /// </summary>
    /// <param name="aiRowNo"></param>
    /// <param name="aiColNo"></param>
    /// <returns></returns>
    private PrePrimaryStudentsExamResult GetRemarkForMonth(int aiRowNo, int aiColNo)
    {
        PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = moPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[aiColNo - 1];
        PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = moPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects[aiRowNo];
        PrePrimaryStudentsExamResult oRemarks = (from remark in moPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamResultWithoutSubjects.AsParallel().AsEnumerable()
                                                 where remark.PreprimaryExamConfigurationId == oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId
                                                       && remark.PrePrimaryProgressReportSubSubjectId == oPrePrimaryProgressReportSubSubjects.SubSubjectID
                                                 select new PrePrimaryStudentsExamResult
                                                 {
                                                     PrePrimaryRemarkId = remark.PrePrimaryRemarkId,
                                                     PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                                                     PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
                                                 }).FirstOrDefault();

        if (oRemarks != null)
            return oRemarks;
        else
        {
            oRemarks = new PrePrimaryStudentsExamResult
            {
                PrePrimaryRemarkId = 0,
                PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
            };

            return oRemarks;
        }
    }

    /// <summary>
    /// This method is used to fill progress report table.
    /// </summary>
    /// <param name="aoPrePrimaryProgressSheetConfigBL"></param>
    private void FillProgressReportTables(PrePrimaryProgressSheetConfigBL aoPrePrimaryProgressSheetConfigBL)
    {
        DataTable oDtProgressreportWoSubject = new DataTable();
        DataTable oDtProgressreportWSubject = new DataTable();
        oDtProgressreportWoSubject.Columns.Add("Skills / Behaviour");
        oDtProgressreportWSubject.Columns.Add("Subjects");
        oDtProgressreportWSubject.Columns.Add("Skills / Behaviour");
        aoPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails.ForEach
        (
            s =>
            {
                oDtProgressreportWoSubject.Columns.Add(s.MonthAbbreviation);
                oDtProgressreportWSubject.Columns.Add(s.MonthAbbreviation);
            }
        );
        aoPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects.ForEach
        (
            examresult =>
            {
                DataRow oDataRow = oDtProgressreportWoSubject.NewRow();
                oDataRow[0] = examresult.SubSubjectName;
                oDtProgressreportWoSubject.Rows.Add(oDataRow);
            }
        );
        aoPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.ForEach
        (
            examresult =>
            {
                DataRow oDataRow = oDtProgressreportWSubject.NewRow();
                oDataRow[0] = examresult.SubjectName;
                oDataRow[1] = examresult.SubSubjectName;
                oDtProgressreportWSubject.Rows.Add(oDataRow);
            }
        );

        CreateStudentInfo(aoPrePrimaryProgressSheetConfigBL);
        FillAllGrids(oDtProgressreportWoSubject, oDtProgressreportWSubject);
    }

    /// <summary>
    /// This method is used to Create Student Info.
    /// </summary>
    /// <param name="aoPrePrimaryProgressSheetConfigBL"></param>
    private void CreateStudentInfo(PrePrimaryProgressSheetConfigBL aoPrePrimaryProgressSheetConfigBL)
    {
        HtmlTable oHeaderHtmlTable = CreateHdTable();
        CreateHdSchoolName(oHeaderHtmlTable, aoPrePrimaryProgressSheetConfigBL);
        CreateHdProgressCard(oHeaderHtmlTable);
        CreateHdStudentName(oHeaderHtmlTable, aoPrePrimaryProgressSheetConfigBL);
        CreateHdStudentAttendance(oHeaderHtmlTable, aoPrePrimaryProgressSheetConfigBL);
        GridViewScrollContainer.Controls.Add(oHeaderHtmlTable);
        oHeaderHtmlTable.Dispose();
    }

    /// <summary>
    /// This methos is used to create not applicable ledgend.
    /// </summary>
    private HtmlTable CreateHdTable()
    {
        HtmlTable oHeaderHtmlTable = new HtmlTable();
        oHeaderHtmlTable.EnableViewState = false;
        oHeaderHtmlTable.CellPadding = Constants.I_ZERO;
        oHeaderHtmlTable.CellSpacing = Constants.I_ONE;
        oHeaderHtmlTable.Attributes.Add("class", "ClsBorderNoBg BGReport");
        oHeaderHtmlTable.Width = "100%";
        oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        return oHeaderHtmlTable;
    }

    /// <summary>
    /// This methos is used to create not Schooll Name header.
    /// </summary>
    /// <param name="aoHeaderHtmlTable"></param>
    /// <param name="aoPrePrimaryProgressSheetConfigBL"></param>
    private void CreateHdSchoolName(HtmlTable aoHeaderHtmlTable, PrePrimaryProgressSheetConfigBL aoPrePrimaryProgressSheetConfigBL)
    {
        string sSchoolName = Convert.ToString(aoPrePrimaryProgressSheetConfigBL.StudentDetails.School_Name);
        string sSchoolOrgnName = Convert.ToString(aoPrePrimaryProgressSheetConfigBL.StudentDetails.School_Orgn_Name);
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, sSchoolOrgnName, "SocietyName", Constants.I_ONE, Constants.I_EIGHT, HorizontalAlign.Center);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, sSchoolName, "ActualSchoolName", Constants.I_ONE, Constants.I_EIGHT, HorizontalAlign.Center);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to create progress report header
    /// </summary>
    /// <param name="aoHeaderHtmlTable"></param>
    private void CreateHdProgressCard(HtmlTable aoHeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Progress Report", "ClsReportHead", Constants.I_ONE, Constants.I_EIGHT, HorizontalAlign.Center);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This methos is used to create not Student name.
    /// </summary>
    /// <param name="aoHeaderHtmlTable"></param>
    /// <param name="aoPrePrimaryProgressSheetConfigBL"></param>
    private void CreateHdStudentName(HtmlTable aoHeaderHtmlTable, PrePrimaryProgressSheetConfigBL aoPrePrimaryProgressSheetConfigBL)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        AddStudentInfo(oHtmlTableRow, "Roll No. ", aoPrePrimaryProgressSheetConfigBL.StudentDetails.RollNo.ToString());
        AddStudentInfo(oHtmlTableRow, "Name ", aoPrePrimaryProgressSheetConfigBL.StudentDetails.StudentName);
        AddStudentInfo(oHtmlTableRow, "Class ", aoPrePrimaryProgressSheetConfigBL.StudentDetails.ClassName);
        AddStudentInfo(oHtmlTableRow, "Year ", aoPrePrimaryProgressSheetConfigBL.StudentDetails.Academic_Year);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This methos is used to create Student attendance.
    /// </summary>
    /// <param name="aoHeaderHtmlTable"></param>
    /// <param name="aoPrePrimaryProgressSheetConfigBL"></param>
    private void CreateHdStudentAttendance(HtmlTable aoHeaderHtmlTable, PrePrimaryProgressSheetConfigBL aoPrePrimaryProgressSheetConfigBL)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Term-I Attendance", "ClsBGWhite ClsBorderlight", Constants.I_ZERO, Constants.I_TWO, HorizontalAlign.Right);
        CreateHtmlCell(oHtmlTableRow, aoPrePrimaryProgressSheetConfigBL.StudentDetails.First_Term_PresentDay + " out of " + aoPrePrimaryProgressSheetConfigBL.StudentDetails.First_Term_Total, "ClsHilightTextB ClspaddingR ClsBorderlight", 0, 2, HorizontalAlign.Left);
        CreateHtmlCell(oHtmlTableRow, "Term-II Attendance", "ClsBGWhite ClsBorderlight ", Constants.I_ZERO, Constants.I_TWO, HorizontalAlign.Right);
        CreateHtmlCell(oHtmlTableRow, aoPrePrimaryProgressSheetConfigBL.StudentDetails.Final_Term_PresentDay + " out of " + aoPrePrimaryProgressSheetConfigBL.StudentDetails.Final_Term_Total, "ClsHilightTextB ClspaddingR ClsBorderlight", 0, 2, HorizontalAlign.Left);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to create cell.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asInnerText"></param>
    /// <param name="asClassName"></param>
    /// <param name="aiRowSpan"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asAlignment"></param>
    private void CreateHtmlCell(HtmlTableRow aoHtmlTableRow, string asInnerText, string asClassName, int aiRowSpan, int aiColSpan, HorizontalAlign asAlignment)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = asInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", aiRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", aiColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", asClassName);
        oHtmlTableCell.Attributes.Add("style", "padding-" + asAlignment + ": 10px");
        oHtmlTableCell.Align = asAlignment.ToString();
        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to student info pair to html row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asLblText"></param>
    /// <param name="asLblVal"></param>
    private void AddStudentInfo(HtmlTableRow aoHtmlTableRow, string asLblText, string asLblVal)
    {
        Label oLabel = new Label { Text = asLblText, CssClass = "LblRht ClspaddingR" };
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Controls.Add(oLabel);
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ClsBorderlight");
        oHtmlTableCell.NoWrap = true;
        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
        if (asLblVal != string.Empty)
        {
            oLabel = new Label { Text = asLblVal, CssClass = "ClsHilightTextB ClspaddingR" };
            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.Controls.Add(oLabel);
            oHtmlTableCell.Align = "left";
            oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ClsBorderlight");
            oHtmlTableCell.NoWrap = true;
            aoHtmlTableRow.Cells.Add(oHtmlTableCell);
        }
    }

    /// <summary>
    /// This methodis used tp fill all grids.
    /// </summary>
    /// <param name="aoDtProgressreportWOSubject"></param>
    /// <param name="aoDtProgressreportWSubject"></param>
    private void FillAllGrids(DataTable aoDtProgressreportWOSubject, DataTable aoDtProgressreportWSubject)
    {
        GridView grdWithSubjects = new GridView();
        GridView grdWithOutSubjects = new GridView();

        grdWithSubjects.Width = Unit.Percentage(100);
        grdWithOutSubjects.Width = Unit.Percentage(100);

        grdWithSubjects.RowStyle.CssClass = "Lbl10pt ConfigHeadBG";
        grdWithSubjects.RowStyle.Font.Size = FontUnit.Point(9);
        grdWithSubjects.RowStyle.Font.Bold = false;

        grdWithOutSubjects.RowStyle.CssClass = "Lbl10pt ConfigHeadBG";
        grdWithOutSubjects.RowStyle.Font.Size = FontUnit.Point(9);

        grdWithSubjects.HeaderStyle.CssClass = "ClsProgressGridTestHeader";
        grdWithSubjects.HeaderStyle.Font.Size = FontUnit.Point(10);
        grdWithSubjects.HeaderStyle.Height = Unit.Pixel(40);

        grdWithOutSubjects.HeaderStyle.CssClass = "ClsProgressGridTestHeader";
        grdWithOutSubjects.HeaderStyle.Font.Size = FontUnit.Point(10);
        grdWithOutSubjects.HeaderStyle.Height = Unit.Pixel(40);

        grdWithSubjects.AlternatingRowStyle.CssClass = "ClsProgressGridTestHeader";
        grdWithSubjects.AlternatingRowStyle.Font.Size = FontUnit.Point(9);
        grdWithSubjects.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#eef1ea");
        grdWithSubjects.AlternatingRowStyle.ForeColor = System.Drawing.Color.Black;
        grdWithSubjects.AlternatingRowStyle.Font.Bold = false;

        grdWithOutSubjects.AlternatingRowStyle.CssClass = "ClsProgressGridTestHeader";
        grdWithOutSubjects.AlternatingRowStyle.Font.Size = FontUnit.Point(9);
        grdWithOutSubjects.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#eef1ea");
        grdWithOutSubjects.AlternatingRowStyle.ForeColor = System.Drawing.Color.Black;
        grdWithOutSubjects.AlternatingRowStyle.Font.Bold = false;

        grdWithSubjects.GridLines = GridLines.None;
        grdWithSubjects.CellSpacing = Constants.I_ONE;
        grdWithSubjects.ForeColor = System.Drawing.Color.Black;

        grdWithOutSubjects.GridLines = GridLines.None;
        grdWithOutSubjects.CellSpacing = Constants.I_ONE;
        grdWithOutSubjects.ForeColor = System.Drawing.Color.Black;

        if (aoDtProgressreportWOSubject.Rows.Count > Constants.I_ZERO)
        {
            grdWithOutSubjects.RowDataBound += grdWithOutSubjects_RowDataBound;
            grdWithOutSubjects.DataSource = aoDtProgressreportWOSubject;
            grdWithOutSubjects.DataBind();

            GridViewScrollContainer.Visible = true;

            if (grdWithOutSubjects.Rows.Count > Constants.I_ZERO)
            {
                HtmlTable oHeaderHtmlTable = new HtmlTable();
                oHeaderHtmlTable.EnableViewState = false;
                oHeaderHtmlTable.Width = "100%";
                oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
                oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
                oHeaderHtmlTable.Align = "center";

                HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                oHtmlTableRow.Align = "center";

                Label lblModuleName = new Label();
                lblModuleName.Text = moPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule[0].ModuleName;
                lblModuleName.CssClass = "HeadTxtBWOPadding";

                HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Controls.Add(lblModuleName);
                oHtmlTableCell.Align = "center";

                oHtmlTableRow.Cells.Add(oHtmlTableCell);
                oHeaderHtmlTable.Rows.Add(oHtmlTableRow);
                GridViewScrollContainer.Controls.Add(oHeaderHtmlTable);
            }

            GridViewScrollContainer.Controls.Add(grdWithOutSubjects);
        }

        if (aoDtProgressreportWSubject.Rows.Count > Constants.I_ZERO)
        {
            grdWithSubjects.RowDataBound += grdWithSubjects_RowDataBound;
            grdWithSubjects.DataSource = aoDtProgressreportWSubject;
            grdWithSubjects.DataBind();

            GridViewScrollContainer.Visible = true;

            if (grdWithSubjects.Rows.Count > Constants.I_ZERO)
            {
                HtmlTable oHeaderHtmlTable = new HtmlTable();
                oHeaderHtmlTable.EnableViewState = false;
                oHeaderHtmlTable.Width = "100%";
                oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
                oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
                oHeaderHtmlTable.Align = "center";

                HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                oHtmlTableRow.Align = "center";

                Label lblModuleNameWithSubject = new Label();
                lblModuleNameWithSubject.Text = moPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule[1].ModuleName;
                lblModuleNameWithSubject.CssClass = "HeadTxtBWOPadding";

                HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Controls.Add(lblModuleNameWithSubject);
                oHtmlTableCell.Align = "center";

                oHtmlTableRow.Cells.Add(oHtmlTableCell);
                oHeaderHtmlTable.Rows.Add(oHtmlTableRow);
                GridViewScrollContainer.Controls.Add(oHeaderHtmlTable);
            }

            GridViewScrollContainer.Controls.Add(grdWithSubjects);
        }

        if (moPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment.Count != Constants.I_ZERO)
        {
            GridView grdViewRemarks = new GridView();
            grdViewRemarks.Width = Unit.Percentage(100);

            grdViewRemarks.RowStyle.CssClass = "Lbl10pt ConfigHeadBG";
            grdViewRemarks.RowStyle.Font.Size = FontUnit.Point(9);
            grdViewRemarks.RowStyle.Font.Bold = false;

            grdViewRemarks.HeaderStyle.CssClass = "ClsProgressGridTestHeader";
            grdViewRemarks.HeaderStyle.Font.Size = FontUnit.Point(10);
            grdViewRemarks.HeaderStyle.Height = Unit.Pixel(30);

            grdViewRemarks.AlternatingRowStyle.CssClass = "ClsProgressGridTestHeader";
            grdViewRemarks.AlternatingRowStyle.Font.Size = FontUnit.Point(9);
            grdViewRemarks.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#eef1ea");
            grdViewRemarks.AlternatingRowStyle.ForeColor = System.Drawing.Color.Black;
            grdViewRemarks.AlternatingRowStyle.Font.Bold = false;

            grdViewRemarks.RowStyle.Wrap = true;
            grdViewRemarks.HorizontalAlign = HorizontalAlign.Center;

            grdViewRemarks.GridLines = GridLines.None;
            grdViewRemarks.CellSpacing = Constants.I_ONE;
            grdViewRemarks.ForeColor = System.Drawing.Color.Black;

            grdViewRemarks.DataSource = moPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment.Where(ec => ec.IsPublished == true).Select(sExamComments => new { Header = sExamComments.Header, Comments = sExamComments.Comment });
            grdViewRemarks.DataBind();

            if (grdViewRemarks.Rows.Count > Constants.I_ZERO)
            {
                HtmlTable oHeaderHtmlTable = new HtmlTable();
                oHeaderHtmlTable.EnableViewState = false;
                oHeaderHtmlTable.Width = "100%";
                oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
                oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
                oHeaderHtmlTable.Align = "center";

                HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                oHtmlTableRow.Align = "center";

                Label lblRemark = new Label();
                lblRemark.Text = "Remarks";
                lblRemark.CssClass = "HeadTxtBWOPadding";

                HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Controls.Add(lblRemark);
                oHtmlTableCell.Align = "center";

                oHtmlTableRow.Cells.Add(oHtmlTableCell);

                oHeaderHtmlTable.Rows.Add(oHtmlTableRow);

                for (int i = 0; i < grdViewRemarks.Rows.Count; i++)
                    grdViewRemarks.Rows[i].Cells[1].Wrap = true;

                GridViewScrollContainer.Controls.Add(oHeaderHtmlTable);
            }

            GridViewScrollContainer.Controls.Add(grdViewRemarks);
        }

        if (cmbStudents.SelectedValue == Constants.S_ZERO)
        {
            HtmlTable oHtmlTable = new HtmlTable();
            oHtmlTable.EnableViewState = false;
            oHtmlTable.Width = "100%";
            oHtmlTable.Height = "50px";
            oHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
            oHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
            oHtmlTable.Align = "center";

            HtmlTableRow oTableRow = new HtmlTableRow();
            oTableRow.Align = "center";

            Label lblLine = new Label();
            lblLine.Text = "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            HtmlTableCell oTableCell = new HtmlTableCell();
            oTableCell.Controls.Add(lblLine);
            oTableRow.Cells.Add(oTableCell);
            oHtmlTable.Rows.Add(oTableRow);
            GridViewScrollContainer.Controls.Add(oHtmlTable);
        }
    }

    /// <summary>
    /// This method is used o display progress report.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void DisplayProgresReport(int aiStudentId)
    {
        int iAcademicYearId = miAcademicYearId;
        moPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
		
        moPrePrimaryProgressSheetConfigBL.GetPrePrimaryProgressSheetDetailsOfStudent(miSchoolId, iAcademicYearId, aiStudentId);

        if (moPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails.Count == Constants.I_ZERO ||
            moPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig.Count == Constants.I_ZERO ||
            (moPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.Count == Constants.I_ZERO && moPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects.Count == Constants.I_ZERO))
        {
            if (GridViewScrollContainer.FindControl("lblNotPublished") == null)
            {
                HtmlTable oHtmlTable = new HtmlTable();
                oHtmlTable.EnableViewState = false;
                oHtmlTable.Width = "100%";
                oHtmlTable.Height = "50px";
                oHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
                oHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
                oHtmlTable.Align = "center";

                HtmlTableRow oTableRow = new HtmlTableRow();
                oTableRow.Align = "center";

                Label oLabel = new Label();
                oLabel.ID = "lblNotPublished";
                oLabel.CssClass = "ClsConfigText";
                oLabel.Text = "Progress report is not published yet.";

                HtmlTableCell oTableCell = new HtmlTableCell();

                oTableCell.Controls.Add(oLabel);
                oTableCell.BgColor = "#E6EEFC";
                oTableRow.Cells.Add(oTableCell);

                oHtmlTable.Rows.Add(oTableRow);
                oTableCell.Style.Add("border", "1px solid #8FBC8F");
                GridViewScrollContainer.Controls.Add(oHtmlTable);
            }
        }
        else
        {
            IEnumerable<PrePrimaryConfiguredMonthDetails> lstMonthPublished = from IsPublishCount in moPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails
                                                                              where IsPublishCount.IsPublished == true
                                                                              select new PrePrimaryConfiguredMonthDetails { IsPublished = IsPublishCount.IsPublished };

            if (lstMonthPublished.Count() == Constants.I_ZERO)
            {
                if (GridViewScrollContainer.FindControl("lblNotPublished") == null)
                {
                    HtmlTable oHtmlTable = new HtmlTable();
                    oHtmlTable.EnableViewState = false;
                    oHtmlTable.Width = "100%";
                    oHtmlTable.Height = "20px";
                    oHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
                    oHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
                    oHtmlTable.Align = "center";

                    HtmlTableRow oTableRow = new HtmlTableRow();
                    oTableRow.Align = "center";

                    Label oLabel = new Label();
                    oLabel.ID = "lblNotPublished";
                    oLabel.CssClass = "ClsConfigText";
                    oLabel.Text = "Progress report is not published yet.";

                    HtmlTableCell oTableCell = new HtmlTableCell();

                    oTableCell.Controls.Add(oLabel);
                    oTableCell.BgColor = "#E6EEFC";
                    oTableRow.Cells.Add(oTableCell);
                    oHtmlTable.Rows.Add(oTableRow);
                    oTableCell.Style.Add("border", "1px solid #8FBC8F");

                    GridViewScrollContainer.Controls.Add(oHtmlTable);
                }
            }
            else
                FillProgressReportTables(moPrePrimaryProgressSheetConfigBL);
        }
    }

    /// <summary>
    /// This method is used to set "old Academic Year" link visiblity to class teacher.
    /// </summary>
    private void SetOldYearRecordLinkVisibility()
    {
        trAcademicYear.Visible = true;
        hidStudId.Value = cmbStudents.SelectedValue;
		FillAcademicYearCombo();

        if (mbIsOldProgressReport)
        {
            hlnkOldAcademicRecord.Visible = false;
            tdAcademicYrs.Visible = true;
            cmbAcademicYrId.Visible = true;
        }
        else
        {
            Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = null;
            tdAcademicYrs.Visible = false;
            cmbAcademicYrId.Visible = false;            
            if (cmbAcademicYrId.Items.Count > Constants.I_ZERO)
            {
                hidLastAcademicYrId.Value = miAcademicYearId.ToString();
                hlnkOldAcademicRecord.Visible = true;
                SetOldProgressReportUrl();
            }
            else
                trAcademicYear.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnPrint, btnShow, btnPrint, btnCancel, btnCancelUp });
	    btnPrint.Attributes.Add("onclick", "GeneratePrint();return false;");
        lnkbtnGradeConfigurationDetails.Attributes.Add("onclick", "OpenPopup(); return false;");
    }

    /// <summary>
    /// This method is used to display progress report.
    /// </summary>
    private void DisplayProgressReportForStudent()
    {
		VisibleHideTeacherCombo(false);
        VisibleHideStudentCombo(false);
        AddPrintAttributes();
        btnShow.Visible = false;
        tdbtnShow.Visible = false;
        tdbtnPrint.Align = HorizontalAlign.Left.ToString();
        tdbtnPrint.Attributes.Remove("class");
        int iStandardId = (moUserRole == Constants.UserRoles.Teacher && string.Equals(Session[Constants.S_SESSION_IS_CLASS_TEACHER], Constants.S_YES)) ? miStandardId.ToInt() : Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        ProgressSheetBase oStudentProgress = (ProgressSheetBase)new StudentProgress();
        bool bIsReportPublishedForAnyStudent = oStudentProgress.isTestPublishedForStudent(miStudentId, miStdDivId);
    	bool bIsExamPublishedForStdDiv = oStudentProgress.isTestPublishedForStdDivId(miStdDivId);

        StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        oStudentFeeDetailsBL.School_Id = miSchoolId;
        oStudentFeeDetailsBL.Academic_Year_Id = miAcademicYearId;
        oStudentFeeDetailsBL.Student_Id = miStudentId;

        bool bIsFeePending  = false;
        if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
            bIsFeePending = Settings.BlockProgressReportIfFeesArePending && oStudentFeeDetailsBL.PendingFeesAvailableForStudent();
        else
            bIsFeePending = oStudentFeeDetailsBL.PendingFeesAvailableForStudent();

        if (bIsFeePending)
            hidIsPendingFee.Value = Constants.S_ONE;
        else
            hidIsPendingFee.Value = Constants.S_ZERO;

		if (mbIsOldProgressReport || bIsExamPublishedForStdDiv || bIsReportPublishedForAnyStudent)
        { 
            ProgressReportBL oProgressReportBL = new ProgressReportBL(miSchoolId, miAcademicYearId, miUserId);
			string sBlockProgressReportReason = oProgressReportBL.GetBlockProgressReportReason(miStudentId);

            bool bShowScreenData = true;
            if (miSchoolId == 18 && miAcademicYearId >= 51)
                bShowScreenData = false;
            
            if (moSchool == Constants.SchoolId.PIONEER)
            {
                DataTable oDTStudentData = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, Session[Constants.S_SESSION_STUDENT_ID].ToInt());
                string sStdName = string.Empty;
                if (oDTStudentData.Rows.Count > 0 && oDTStudentData.Rows[0]["Standard_Name"] != DBNull.Value)
                    sStdName = oDTStudentData.Rows[0]["Standard_Name"].ToString();

                if (mlstPioneerGradeReportStandards.Contains(sStdName))
                    bShowScreenData = false;
            }

            if ((!bIsFeePending && bShowScreenData && sBlockProgressReportReason.IsNullOrEmpty()) || (!bIsExamPublishedForStdDiv && bIsReportPublishedForAnyStudent && !oStudentProgress.isTestPublishedForStudent(miStudentId, Constants.I_ZERO)))
			{
				lblmandatory.Visible = false;
				VisibleHideGenerateButton(true);
				miStudentId = (moUserRole == Constants.UserRoles.Student) ? Session[Constants.S_SESSION_STUDENT_ID].ToInt() : hidStudId.Value.ToInt();

				if (mbIsOldProgressReport)
				{
					DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, miStudentId);
					miStudentId = oDatatable.Rows[0][0].ToInt();
					iStandardId = oDatatable.Rows[0][1].ToInt();
					miStdDivId = oDatatable.Rows[0][2].ToInt();
					hidStudId.Value = miStudentId.ToString();
                    hidStandardId.Value = iStandardId.ToString();
				}
				
                TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
                int iStdDivId = 0;
                if (!cmbTeachers.SelectedValue.IsNullOrEmpty())
                    iStdDivId = (cmbTeachers.SelectedValue.ToInt() > Constants.I_ZERO) ? cmbTeachers.SelectedValue.ToInt() : Session[Constants.S_SESSION_TEACHER_ID].ToInt();
                else
                    iStdDivId = miStdDivId;
                DataTable odtTeachers = TeacherStandardDetailsCollectionBL.GetTeachersForPrePrimaryProgressReport(iStdDivId,
                                                                                                             miSchoolId,
                                                                                                             miAcademicYearId);

                if (!oTeacherStandardDetailsBL.IsPreprimaryExamConfiguration(miSchoolId, miAcademicYearId, moUserRole != Constants.UserRoles.Student ? miStdDivId : miStudentId, ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]).ToString())
                    || odtTeachers.Rows.Count == 0)
				{
					oStudentProgress = ProgressSheet.GetProgressSheet(GridViewScrollContainer, miSchoolId, miAcademicYearId, miStudentId, Constants.UserRoles.Student);
					oStudentProgress.ShowProgressSheet(miStudentId);

					if (oStudentProgress is StudentProgress)
					{
						GenerateFinalResult(miStudentId);
                        if (moUserRole == Constants.UserRoles.Student)
                            tdhlnkToppers.Visible = false;
                        else
                        {
                            if (ShowCurrentYearData)
                                tdhlnkToppers.Visible = false;
                            else
                                tdhlnkToppers.Visible = Settings.ShowTopppers;
                        }

						tdhlnkToppers.Width = "100%";
						tdhlnkToppers.Align = HorizontalAlign.Right.ToString();
						hlnkToppers.Enabled = true;
						SetToppersLinkURL();
                        SetDownloadButtonState(true);
						TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL();
						if (oTeacherStandardDetailsCollectionBL.CheckIfStandardHasOnlyGradeSystem(0, iStandardId) == Constants.C_YES)
							tdhlnkToppers.Visible = false;
					}
					else
						tdhlnkToppers.Visible = false;
				}
				else
					DisplayProgresReport(hidStudId.Value.ToInt());
			}
			else
			{
				lblBlockProgressReportReason.Visible = true;
                if (lblErrorMsg.Text != string.Empty)
				    lblErrorMsg.Visible = true;

                if (!bShowScreenData)
                {
                    //lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    //lblErrorMsg.CssClass = "LblNoRecord";
                    //lblErrorMsg.Visible = true;

                    if (bIsFeePending || sBlockProgressReportReason != string.Empty)
                    {
                        SetDownloadButtonState(false, true);
                        throw new BlockProgessReport(bIsFeePending ? PendingFeeMessage : string.Empty, sBlockProgressReportReason);
                    }
                    else
                    {
                        lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                        lblErrorMsg.CssClass = "LblNoRecord";
                        lblErrorMsg.Visible = true;

                        lblErrorMsg.Text = "On publish, you will see download buttons to download Term 1/2 progress report.";

                        if (mbIsOldProgressReport)
                        {
                            DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, miStudentId);
                            miStudentId = oDatatable.Rows[0][0].ToInt();
                            iStandardId = oDatatable.Rows[0][1].ToInt();
                            miStdDivId = oDatatable.Rows[0][2].ToInt();
                            hidStudId.Value = miStudentId.ToString();
                            hidStandardId.Value = iStandardId.ToString();

                            SetDownloadButtonState(true);
                        }
                    }
                }
                else
                    throw new BlockProgessReport(bIsFeePending ? PendingFeeMessage : string.Empty, sBlockProgressReportReason);
			}
            SetUrlToLinkButton();
        }
        else
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = S_NO_EXAM_PUBLISH_MSG;
        }
        //else
        //    throw new NoResultFound(S_NO_EXAM_PUBLISH_MSG);
    }

    /// <summary>
    /// This method is used to set values of session variable to member variables.
    /// </summary>
    private void InitializeMembers()
    {
        InitializeMemberVariables();
        if (Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID] != null)
            miStdDivId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
        if (Session[Constants.S_SESSION_TEACHER_ID] != null)
            miTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();
    }

    /// <summary>
    ///  This method is used to set old progress report hyperlink attribute.
    /// </summary>
    private void SetOldProgressReportUrl()
    {
        string sQueryString;
        if (moUserRole == Constants.UserRoles.Teacher && string.Equals(Session[Constants.S_SESSION_IS_CLASS_TEACHER], Constants.S_YES))
            sQueryString = "../Student/StudentProgressSheet.aspx?" + CommonUtility.EncryptQuerystring("IsOldProgressReport=True&IsTeacherLogin=True&StudentId=" + hidStudId.Value + "&StandardId=" + hidStandardId.Value+"&SdtDivId="+cmbTeachers.SelectedValue);
        else
            sQueryString = "../Student/StudentProgressSheet.aspx?" + CommonUtility.EncryptQuerystring("IsOldProgressReport=True");
		hlnkOldAcademicRecord.Attributes.Add("onclick", "myWindow =window.open('" + sQueryString + "','_new','width=900,height=700,scrollbars=yes'); myWindow.focus();");
    }

    /// <summary>
    /// This method is used to decrypt querystring passed to this page.
    /// </summary>
    private bool IsOldProgressReport()
    {
        bool bIsOldProgressReport = false;
        
		if (QueryString.Count > Constants.I_ZERO)
        {
            if (QueryString["IsOldProgressReport"] != null)
                bIsOldProgressReport = QueryString["IsOldProgressReport"].ToBool();
            if (QueryString["AcademcYearId"] != null)
            {
                miAcademicYearId = QueryString["AcademcYearId"].ToInt();
                Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = miAcademicYearId;
            }
            if (QueryString["IsTeacherLogin"] != null)
                mbIsTeacherLogin = QueryString["IsTeacherLogin"].ToBool();
            if (QueryString["StudentId"] != null)
                miStudentId = QueryString["StudentId"].ToInt();
            if (QueryString["StandardId"] != null)
                miStandardId = QueryString["StandardId"].ToInt();
			if (QueryString["StdDivId"] != null)
				miStdDivId = QueryString["StdDivId"].ToInt();
        }

        return bIsOldProgressReport;
    }

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideTeacherCombo(bool abAction)
    {
        tdcmbTeachers.Visible = abAction;
        tdlblTeacher.Visible = abAction;
     }

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideStudentCombo(bool abAction)
    {
        tdUPanelStudent.Visible = false;
        tdUPanelStudent.Attributes.Remove("class");
        cmbStudents.Visible = abAction;
        tdlblStudent.Visible = abAction;
    }

    /// <summary>
    /// This method is used to display exception.
    /// </summary>
    /// <param name="ex"></param>
    private void SetNoRecordMessage(Exception ex)
    {
        lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
        lblErrorMsg.CssClass = "LblNoRecord";
        lblErrorMsg.Visible = true;
        lblErrorMsg.Text = ex.Message;
    }

	/// <summary>
	/// This method is used to display exception.
	/// </summary>
	/// <param name="ex"></param>
	private void SetNoRecordMessage(BlockProgessReport ex)
	{
		if (!ex.Message.IsNullOrEmpty())
		{
			lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign,"left");
			lblErrorMsg.CssClass = "LblNoRecord";
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = ex.Message;
		}

		if (!ex.BlockProgressReportReason.IsNullOrEmpty())
		{		
			lblBlockProgressReportReason.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
			lblBlockProgressReportReason.CssClass = "LblNoRecord";
			lblBlockProgressReportReason.Visible = true;
			lblBlockProgressReportReason.Style.Add(HtmlTextWriterStyle.Display, "inline-block");
			lblBlockProgressReportReason.Text = "You are prohibited to view the progress report due to following reason :<BR />" + ex.BlockProgressReportReason + "<BR />Please do the needful to view the progress report.";
		}
	}

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideGenerateButton(bool abAction)
    {
        if (ShowCurrentYearData)
        {
            tdbtnPrint.Visible = false;
            btnPrint.Visible = false;
            tdhlnkToppers.Visible = false;
        }
        else
        {
            tdbtnPrint.Visible = abAction;
            if (moUserRole == Constants.UserRoles.Student && miSchoolId != Constants.SchoolId.SVP.ToInt())
                btnPrint.Visible = abAction;
            else
                btnPrint.Visible = false;
        }
    }

    /// <summary>
    /// This function is used to fill teacher combo
    /// </summary>
    private void FillTeachersComboBox()
    {
        // get all class teachers
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		List<ClassTeacherDetails> lstTeacher = MasterDataCollectionBL.GetClassTeacher(miSchoolId, miAcademicYearId);

		if (moUserRole == Constants.UserRoles.Teacher && !hidUserHasFullAccess.Value.ToBool() && !mbIsOldProgressReport)
		{
			List<ClassTeacherDetails> lstClassTeachers = lstTeacher.Where(Teacher => Teacher.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).ToList();
			ListSource.FillDropDownList(lstClassTeachers, cmbTeachers, "TeacherName", "StandardDivisionId", lstClassTeachers.Count == Constants.I_ZERO ? Constants.S_SELECT : string.Empty);
			if (lstClassTeachers.Count == Constants.I_ONE)
			{
				cmbTeachers.SelectedIndex = Constants.I_ONE;
				cmbTeachers.Enabled = false;
			}		
		}
		else
			ListSource.FillDropDownList(lstTeacher, cmbTeachers, "TeacherName", "StandardDivisionId", Constants.S_SELECT);

        if (cmbTeachers.Items.Count == Constants.I_ZERO)
        {
            if (moUserRole == Constants.UserRoles.Admin)
            {
                pnlErrorMsg.Visible = true;
                lblErrorMsgPre.Text = Constants.S_ERROR_MSG_FOR_ALL_CONFIGURATION;
                pnlFilter.Visible = false;
                GridViewScrollContainer.Visible = false;
            }
            else
            {
                if (moUserRole == Constants.UserRoles.Supervisor)
                {
                    pnlErrorMsg.Visible = true;
                    lblErrorMsgPre.Text = Constants.S_NONADMIN_PRECONDITION_MSG;
                    pnlFilter.Visible = false;
                    GridViewScrollContainer.Visible = false;
                    Hyper.Visible = false;
                }
                else
                {
                    lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");                    
                    lblErrorMsg.Text = S_ERROR_MSG;
                    lblErrorMsg.CssClass = "LblNoRecord";
                    lblErrorMsg.Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// This function is used to fill student's combo
    /// </summary>
    private void FillStudentsComboBox(DataTable aoDtStudent)
    {
        // get all class teachers
        cmbStudents.Bind(aoDtStudent, "Student_Id", "Student_Name", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetToppersLinkURL()
    {
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(miStdDivId);
        string sQueryString = "ExamType=0&ToppersType=0&StdDivId=" + miStdDivId.ToString() + "&StdId=" + oStandardDivisionMasterBL.StandardId.ToString()+"&IsOldYear="+mbIsOldProgressReport;
        sQueryString = "../Student/ExamToppersUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
        hlnkToppers.Attributes.Add("onclick", "ShowToppers('" + sQueryString + "');return false;");
        hlnkToppers.Visible = Settings.ShowTopppers;
    }

    /// <summary>
    /// This method is used to add attribute for print button.
    /// </summary>
    private void AddPrintAttributes()
    {
        int iAcademicYearId = 0;
        iAcademicYearId = !mbIsOldProgressReport ? miAcademicYearId : Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();
        string sQryStr = "mode=print";
        if (moUserRole == Constants.UserRoles.Admin
            || bool.Parse(hidUserHasFullAccess.Value))
        {
			sQryStr = sQryStr + "&iStdDivId=" + cmbTeachers.SelectedValue;

            // If old Academic year recor, then take hiden variable value as Student id.
            sQryStr = iAcademicYearId != miAcademicYearId ? sQryStr + "&iStudId=" + hidStudId.Value : sQryStr + "&iStudId=" + cmbStudents.SelectedValue;
            sQryStr = sQryStr + "&iAcademicYearId=" + iAcademicYearId;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
			sQryStr = sQryStr + "&iStdDivId=" + (cmbTeachers.SelectedValue == "0" ? miStdDivId.ToString() : cmbTeachers.SelectedValue);
			sQryStr = iAcademicYearId != miAcademicYearId ? sQryStr + "&iStudId=" + hidStudId.Value : sQryStr + "&iStudId=" + (cmbStudents.SelectedValue == "0" ? miStudentId.ToString() : cmbStudents.SelectedValue);
            sQryStr = sQryStr + "&iAcademicYearId=" + iAcademicYearId;
        }
        else
        {
			sQryStr = sQryStr + "&iStdDivId=0";
            sQryStr = sQryStr + "&iStudId=" + Convert.ToString(Session[Constants.S_SESSION_STUDENT_ID]);
            sQryStr = sQryStr + "&iAcademicYearId=" + iAcademicYearId;
        }

        sQryStr = Utility.CommonUtility.EncryptQuerystring(sQryStr);
        hidQery.Value = sQryStr;
    }

    /// <summary>
    /// This function is used to show progress sheet forselected criteria.
    /// </summary>
    private void ShowStudProgressSheet()
    {
        int iStudentId = cmbStudents.SelectedValue.ToInt();
        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
		int iStandardDivisionId = cmbTeachers.SelectedValue.ToInt();
        ProgressSheetBase oStudentProgress = (ProgressSheetBase)new StudentProgress();
        if (oStudentProgress.isTestPublishedForStdDivId(iStandardDivisionId) || oStudentProgress.isTestPublishedForStudent(cmbStudents.SelectedValue.ToInt(), iStandardDivisionId))
        {
            if (iStudentId != Constants.I_ZERO)
            {
                oStudentProgress = ProgressSheet.GetProgressSheet(GridViewScrollContainer, miSchoolId, miAcademicYearId, iStudentId, Constants.UserRoles.Student);
                oStudentProgress.ShowProgressSheet(iStudentId);
            }
            else
            {
				oStudentProgress = ProgressSheet.GetProgressSheet(GridViewScrollContainer, miSchoolId, miAcademicYearId, iStandardDivisionId, Constants.UserRoles.Teacher);
				int iResult = oStudentProgress.ShowProgressSheet(iStandardDivisionId, iStudentId);
                if (iResult > Constants.I_ONE)
                    btnCancelUp.Visible = true;
            }
        }
        else
            throw new NoResultFound(S_NO_EXAM_PUBLISH_MSG);
    }

    /// <summary>
    /// This method is used to fill academic year combo on page load.
    /// </summary>
    private void FillAcademicYearCombo()
    {
        int iStudentId = 0;
        iStudentId = moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor ? Convert.ToInt32(hidStudId.Value) : Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
        
        miStudentId = StudentBL.GetYearwiseStudentId(miSchoolId, miAcademicYearId, iStudentId);

        DataTable oDtYearInfo = SchoolWiseAcademicYearMasterBL.GetPassedAcademicYears(miSchoolId, iStudentId, ShowCurrentYearData);
        if (oDtYearInfo != null && oDtYearInfo.Rows.Count > Constants.I_ZERO && oDtYearInfo.Rows[0][0] != DBNull.Value)
        {
            cmbAcademicYrId.Bind(oDtYearInfo, "Value_Member", "Display_Member", string.Empty);
            if (mbIsOldProgressReport)
            {
                if (!ShowCurrentYearData)
                {
                    if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] != null)
                        cmbAcademicYrId.SelectedValue = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToString();
                    lblOldAcademicYear.Text = CommonUtility.DisplayAcademicYear(cmbAcademicYrId.SelectedItem.Text);
                    if (miAcademicYearId == Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt())
                        miAcademicYearId = cmbAcademicYrId.SelectedValue.ToInt();
                }
                else
                {   
                    cmbAcademicYrId.SelectedValue = miAcademicYearId.ToString();
                    cmbAcademicYrId.Enabled = false;
                }
            }
        }
		else
		{
			cmbAcademicYrId.Items.Clear();
		}

        if (cmbAcademicYrId.Items.Count == Constants.I_ONE)
            cmbAcademicYrId.Enabled = false;
    }

    /// <summary>
    /// This method is used to create an result of a student
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void GenerateFinalResult(int aiStudentId)
    {
        try
        {
            StudentResult oStudentResult = new StudentResult(ResultContainer);
            if (mbIsOldProgressReport == false)
                oStudentResult.ShowProgressSheetNote();
            else
            {
                // To saprate two grids.
                HtmlTable oHeaderHtmlTable = new HtmlTable();
                HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                oHtmlTableRow.Cells.Add(oHtmlTableCell);
                oHeaderHtmlTable.Rows.Add(oHtmlTableRow);
                LiteralControl oLiteralControl = new LiteralControl("<br />");
                GridViewScrollContainer.Controls.Add(oLiteralControl);
                GridViewScrollContainer.Controls.Add(oHeaderHtmlTable);
            }

            if (Settings.ShowAnnualInProgressSheet)
                if (mbIsOldProgressReport || CheckIsResultPublished())
                    oStudentResult.FillProgressReport(aiStudentId);
        }
        catch (MarksNotAvailableForResult ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (NoResultFound ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
    }

    /// <summary>
    /// This method is used to check that is Result is published or not
    /// </summary>
    private bool CheckIsResultPublished()
    {
        SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, miStdDivId);
        if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id == Constants.I_ZERO)
            return false;
        return true;
    }

    /// <summary>
    /// This method is used to display progress report when login user is student.
    /// </summary>
    private void DisplayStudentPregressReport()
    {
        const string S_CLOSE = "Close";
        const string S_BACK = "Back";

        if (moUserRole == Constants.UserRoles.Student)
            hidStudId.Value = miUserId.ToString();
        else
            hidStudId.Value = miStudentId.ToString();
        trAcademicYear.Visible = true;
        if(cmbAcademicYrId.Items.Count<=0)
		FillAcademicYearCombo();
        if (mbIsOldProgressReport)
        {
            btnCancel.Text = S_CLOSE;
            btnCancelUp.Text = S_CLOSE;
            trHeader.Visible = true;
            hlnkOldAcademicRecord.Visible = false;
            tdAcademicYrs.Visible = true;
            cmbAcademicYrId.Visible = true;
            Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = miAcademicYearId.ToString();
            DisplayProgressReportForStudent();
            hidLastAcademicYrId.Value = miAcademicYearId.ToString();
        }
        else
        {
            Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = null;
            btnCancel.Text = S_BACK;
            btnCancelUp.Text = S_BACK;
            tdAcademicYrs.Visible = false;
            cmbAcademicYrId.Visible = false;
            trHeader.Visible = false;
            string sNewStudent = S_FALSE;
            if (Session[Constants.S_SESSION_IS_NEW_ADMISSION] != null)
                sNewStudent = Session[Constants.S_SESSION_IS_NEW_ADMISSION].ToString();
            if (cmbAcademicYrId.Items.Count > 0 && sNewStudent == S_FALSE)
            {
                hidLastAcademicYrId.Value = miAcademicYearId.ToString();
                hlnkOldAcademicRecord.Visible = true;
                SetOldProgressReportUrl();
            }
            else
                hlnkOldAcademicRecord.Visible = false;

            DisplayProgressReportForStudent();
        }
    }

    private void SetDownloadButtonState(bool abShowButtonForStudent, bool abIsProgressReportBlocked=false)
    {
        btnDownload.Visible = false;
        btnDowloadTerm2Report.Visible = false;
        btnDownloadTestReport.Visible = false;
        btnDownloadPrelimReport.Visible = false;
        StudentProgress oStudentProgress = new StudentProgress();
        string sStandardName = string.Empty;
        if ((miSchoolId == Constants.SchoolId.PPSH.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.PPS.ToInt() || miSchoolId == Constants.SchoolId.NPS.ToInt() || miSchoolId == Constants.SchoolId.MNS.ToInt()) && !mbIsOldProgressReport && abShowButtonForStudent && moUserRole == Constants.UserRoles.Student)
        {
            bool bIsTermExamPublished = oStudentProgress.IsTermExamPublished(miStdDivId, out sStandardName);
            hidStandardName.Value = sStandardName;

            if (bIsTermExamPublished && miSchoolId != Constants.SchoolId.PPSH.ToInt())
            {
                if (miSchoolId != Constants.SchoolId.SVP.ToInt())
                    btnDownload.Visible = true;
                else
                    btnSVPDownload.Visible = true;
            }

            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                if (!bIsTermExamPublished || hidIsPendingFee.Value == Constants.S_ONE)
                {
                    btnDownload.Visible = false;
                    trblinkmessgae.Visible = false;
                }
                else
                {
                    btnDownload.Visible = true;
                    btnDownload.Text = "DOWNLOAD TERM 1 REPORT";
                    trblinkmessgae.Visible = true;
                }


                SetTerm2ReportButtonState();

                //if (!bIsTermExamPublished && (sStandardName == "9" || sStandardName == "10"))
                //    btnDownload.Visible = false;

                btnPrint.Visible = false;
            }

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            {
                bIsTermExamPublished = oStudentProgress.IsTermExamPublished(miStdDivId, out sStandardName);
                bool bIsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);

                if (bIsFinalExamPublished && (hidStandardName.Value == "6" || hidStandardName.Value == "7" || hidStandardName.Value == "8" || hidStandardName.Value == "9"))
                    btnDownload.Visible = true;
                else if (bIsTermExamPublished)
                    btnDownload.Visible = true;
                //else if (bIsTermExamPublished && (hidStandardName.Value == "1" || hidStandardName.Value == "2" || hidStandardName.Value == "3" || hidStandardName.Value == "4" || hidStandardName.Value == "5"))
                //    btnDownload.Visible = true;
                else
                    btnDownload.Visible = false;
            }

            if (miSchoolId == Constants.SchoolId.NPS.ToInt())
            {
                bool bIsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);
                if (bIsFinalExamPublished)
                    btnDownload.Visible = true;
                else
                    btnDownload.Visible = false;
            }            
        }

        if ((miSchoolId == Constants.SchoolId.PPS.ToInt() || miSchoolId == Constants.SchoolId.PPSH.ToInt()) && mbIsOldProgressReport)
        {
           hidOldStdDivId.Value = miStdDivId.ToString();
            bool bIsTermExamPublished = oStudentProgress.IsLastYEarTermExamPublished(miStdDivId, miSchoolId, miAcademicYearId, out sStandardName);
            hidStandardName.Value = sStandardName;
            if (bIsTermExamPublished)
            {
                btnDownload.Visible = true;
                btnDownload.Text = "DOWNLOAD TERM 1 REPORT";
                trblinkmessgae.Visible = true;

                if(miSchoolId == Constants.SchoolId.PPSH.ToInt())
                    btnDownload.Text = "DOWNLOAD PDF";
            }

            if (miAcademicYearId >= 50 || miSchoolId == Constants.SchoolId.PPSH.ToInt())
                SetTerm2ReportButtonState();
            else
                {
                    btnDownload.Visible = false;
                    trblinkmessgae.Visible = false;
                    btnDowloadTerm2Report.Visible = false;
                }
        }

        if (miSchoolId == Constants.SchoolId.PPS.ToInt())
        {
            if (sStandardName == "10" && hidIsPendingFee.Value != Constants.S_ONE)
            {
                SchoolWiseStanderedDivisionTestMasterBL oSchoolWiseStanderedDivisionTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
                bool bIsPrelimExamPublished = oSchoolWiseStanderedDivisionTestMasterBL.IsPrelimExamPublished(miStdDivId, miSchoolId, miAcademicYearId);
                btnDownloadPrelimReport.Visible = bIsPrelimExamPublished;
            }
        }

        if (miSchoolId == Constants.SchoolId.PKIS.ToInt() && !mbIsOldProgressReport && abShowButtonForStudent && moUserRole == Constants.UserRoles.Student)
        {
            bool bIsTermExamPublished = oStudentProgress.IsTermExamPublished(miStdDivId, out sStandardName);
            if (bIsTermExamPublished)
                btnDownload.Visible = true;
        }
        else if (miSchoolId == Constants.SchoolId.SPS.ToInt() && !mbIsOldProgressReport && abShowButtonForStudent && moUserRole == Constants.UserRoles.Student)
        {
            bool bIsTermExamPublished = oStudentProgress.IsTermExamPublished(miStdDivId, out sStandardName);
            btnDownloadTestReport.Visible = true;
            btnDownloadTestReport.Attributes.Add("onclick", "OpenTestPopup(); return false;");

            hidStudentIdForReport.Value = miStudentId.ToString();
            hidStandardDivisionId.Value = miStdDivId.ToString();
            TestCollectionBL TestCollectionBL = new TestCollectionBL(miSchoolId,miAcademicYearId);
            DataTable dt = TestCollectionBL.GetAllpublishedTestsForStandard(hidStandardId.Value.ToInt(), miStudentId);
            ListSource.FillDropDownList(dt, cmbTests, "SchoolWise_Test_Name", "SchoolWise_Test_Id", Constants.S_SELECT);
        }
        else if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            bool bIsTermExamPublished = oStudentProgress.IsTermExamPublished(miStdDivId, out sStandardName);
            if (bIsTermExamPublished)
            {
                if (sStandardName == "1" || sStandardName == "2" || sStandardName == "3" || sStandardName == "4" || sStandardName == "5")
                    btnDownload.Visible = true;
                else
                    btnDownload.Visible = false;
            }
            else
                btnDownload.Visible = false;
        }
        else if (miSchoolId == Constants.SchoolId.PIONEER.ToInt())
        {
            if (!abIsProgressReportBlocked)
            {
                bool bIsTermExamPublished = oStudentProgress.IsTermExamPublished(miStdDivId, out sStandardName);
                hidStandardName.Value = sStandardName;
                if (bIsTermExamPublished)
                    btnDownload.Visible = true;
                else
                    btnDownload.Visible = false;
            }
            else
                btnDownload.Visible = false;
        }
    }

    private void SetTerm2ReportButtonState()
    {
        StudentProgress oStudentProgress = new StudentProgress();
        bool bIsFinalExamPublished = false;
        if (mbIsOldProgressReport)
            bIsFinalExamPublished = oStudentProgress.IsLatYearFinalResultPublished(hidOldStdDivId.Value.ToInt(), miSchoolId, hidLastAcademicYrId.Value.ToInt());
        else
            bIsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);

        if (bIsFinalExamPublished && hidIsPendingFee.Value != Constants.S_ONE && miSchoolId != Constants.SchoolId.PPSH.ToInt())
            btnDowloadTerm2Report.Visible = true;
        else
            btnDowloadTerm2Report.Visible = false;
    }

    /// <summary>
    /// This method is used to display progress report to teacher.
    /// </summary>
    private void DisplayTeacherProgressReport()
    {
        trMandatory.Visible = false;
        FillTeachersComboBox();
		miStdDivId = miStdDivId != 0 ? miStdDivId : cmbTeachers.SelectedValue.ToInt();        

        StudentProgress oStudentProgress = new StudentProgress();

        DataTable odtTeachers = TeacherStandardDetailsCollectionBL.GetTeachersForPrePrimaryProgressReport(miStdDivId,
                                                                                                             miSchoolId,
                                                                                                             miAcademicYearId);
        if (odtTeachers.Rows.Count > Constants.I_ZERO)
        {
            if (cmbStudents.SelectedValue == Constants.S_ZERO)
            {
                StudentProgress objStudentProgress = new StudentProgress();
                DataTable odtStudents = GetStudentData(miStdDivId);
                for (int i = 0; i < odtStudents.Rows.Count; i++)
                    DisplayProgresReport(odtStudents.Rows[i]["Student_Id"].ToInt());
                DataTable oDTStudents = GetStudentData(miStdDivId);
                FillStudentsComboBox(oDTStudents);
                SetToppersLinkURL();
                VisibleHideGenerateButton(true);
            }
        }
        else if(miStdDivId!=Constants.I_ZERO)
        {
			DataTable oDtStudents = GetStudentData(miStdDivId);
			FillStudentsComboBox(oDtStudents);
            if (oStudentProgress.isTestPublishedForStdDivId(miStdDivId) || oStudentProgress.isTestPublishedForStudent(0, miStdDivId))
            {
                VisibleHideGenerateButton(true);				
                SetToppersLinkURL();
				AddPrintAttributes();
            }
            else
            {
                //VisibleHideStudentCombo(true);
                VisibleHideGenerateButton(true);
                btnShow.Visible = true;
                tdbtnShow.Visible = true;
				btnPrint.Visible = true;
                tdbtnPrint.Align = HorizontalAlign.Left.ToString();
                throw new NoResultFound(S_NO_EXAM_PUBLISH_MSG);
            }
        }
    }

    /// <summary>
    /// This mehod is used to cke whether Xseed is applicable or not.
    /// </summary>
	private void IsXseedApplicable()
    {
		int iTeachersStandardDivisionId = Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToInt();

        if (moUserRole == Constants.UserRoles.Student)
	    {
		    int iStandardId = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
		    
		    if (!mbIsOldProgressReport)
		    {
			    int iAcademicYearId = miAcademicYearId;
			    if (mbIsOldProgressReport)
				    iAcademicYearId = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();
			    XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
			    if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, iAcademicYearId, iStandardId,
			                                                 iTeachersStandardDivisionId))
			    {
                    string sQueryString = "IsOldProgressReport=N&AcademcYearId=" + cmbAcademicYrId.SelectedValue + "&ShowCurrentYearData=0";
					    MasterPage oMasterPage = this.Master as MasterPage;
					    if (oMasterPage != null)
						    oMasterPage.RedirectToNextPage("../Xseed/XseedProgressReportUI.aspx?" +
						                                   CommonUtility.EncryptQuerystring(sQueryString));
				   }
		    }
		    else
		    {
				
				FillAcademicYearCombo();
			    DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, miStudentId);
			    XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
				if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, miAcademicYearId, oDatatable.Rows[0][1].ToInt(),
			                                                 iTeachersStandardDivisionId))
			    {
                    string sQueryString = "IsOldProgressReport=Y&AcademcYearId=" + cmbAcademicYrId.SelectedValue + "&StudentId=" + oDatatable.Rows[0][0].ToInt() + "&StdDivId=" + oDatatable.Rows[0][2].ToInt() + "&StandardId=" + oDatatable.Rows[0][1].ToInt() + "&ShowCurrentYearData=0";
				    Response.Redirect("../Xseed/XseedProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
			    }
		    }
	    }
        else if (moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Admin)
		{
            if (mbIsOldProgressReport)
            {
                hidStudId.Value = miStudentId.ToString();
                FillAcademicYearCombo();
                DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, miStudentId);
                XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
                if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, miAcademicYearId, oDatatable.Rows[0][1].ToInt(),
                                                             iTeachersStandardDivisionId))
                {
                string sQueryString = string.Empty;
                if (!ShowCurrentYearData)
                    sQueryString = "IsOldProgressReport=Y&AcademcYearId=" + cmbAcademicYrId.SelectedValue + "&StudentId=" + oDatatable.Rows[0][0].ToInt() + "&StdDivId=" + oDatatable.Rows[0][2].ToInt() + "&StandardId=" + oDatatable.Rows[0][1].ToInt() + "&ShowCurrentYearData=0";
                else
                    sQueryString = "IsOldProgressReport=Y&AcademcYearId=" + cmbAcademicYrId.SelectedValue + "&StudentId=" + oDatatable.Rows[0][0].ToInt() + "&StdDivId=" + oDatatable.Rows[0][2].ToInt() + "&StandardId=" + oDatatable.Rows[0][1].ToInt() + "&ShowCurrentYearData=1";

                    //string sQueryString = "IsOldProgressReport=Y&AcademcYearId=" + cmbAcademicYrId.SelectedValue + "&StudentId=" + oDatatable.Rows[0][0].ToInt() + "&StdDivId=" + oDatatable.Rows[0][2].ToInt() + "&StandardId=" + oDatatable.Rows[0][1].ToInt() + "&ShowCurrentYearData=0";
				    //Response.Redirect("../Xseed/XseedProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));

                    Response.Redirect("../Xseed/XseedProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
                }
            }
            else
            {
                XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
                if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, miAcademicYearId, miStandardId,
                                                                iTeachersStandardDivisionId))
                {
                    string sQueryString = "AcademcYearId=" + cmbAcademicYrId.SelectedValue + "&StdDivId=" + Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToInt() + "&StandardId=" + Session[Constants.S_SESSION_TEACHER_STANDARD_ID].ToInt() + "&ShowCurrentYearData=0";
                    Response.Redirect("../Xseed/XseedProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
                }
            }
		}
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetUrlToLinkButton()
    {
        lnkbtnGradeConfigurationDetails.Attributes.Add("onclick", "OpenPopup(); return false;");
        lnkbtnGradeConfigurationDetails.Visible = true;
        FillMarkGradeListViewSubject();
        FillMarkGradeListViewCurricularSubject();        
    }

    /// <summary>
    /// This method for FillMarkGradeListViews
    /// </summary>
    /// <param name="abIsCoCurricularSubjects"></param>
    private void FillMarkGradeListViewSubject()
    {
        MarksGradesConfigurationBL oMarksGradesConfigurationBL = new MarksGradesConfigurationBL();
        oMarksGradesConfigurationBL.Academic_Year_Id = miAcademicYearId;       
        oMarksGradesConfigurationBL.School_Id = miSchoolId;
        if (!mbIsOldProgressReport)
        {
            if (moUserRole == Constants.UserRoles.Admin
                        || bool.Parse(hidUserHasFullAccess.Value) || moUserRole == Constants.UserRoles.Teacher)
            {
                int iTecherId = cmbTeachers.SelectedValue.ToInt();
                DataTable oDtStudents = GetStudentData(iTecherId);
                if (cmbTeachers.SelectedValue != Constants.S_ZERO && oDtStudents != null && oDtStudents.Rows.Count > Constants.I_ZERO)
                    hidStandardId.Value = oDtStudents.Rows[0][0].ToString();
            }
            else
                hidStandardId.Value = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToString();
        }
        oMarksGradesConfigurationBL.Standard_Id =Convert.ToInt32(hidStandardId.Value);
        oMarksGradesConfigurationBL.IsCoCurricularSubjects = false;
        DataSet oDsMarkGradesSubject = oMarksGradesConfigurationBL.FetchMarksGradesConfigurationDetails();
        DataRow[] drArr = oDsMarkGradesSubject.Tables[1].Select("Standard_Id IS NOT NULL");
        if (drArr.Length > 0)
        {
            lstvwGradeConfigurationDetailsSubject.DataSource = drArr.CopyToDataTable();
            lstvwGradeConfigurationDetailsSubject.DataBind();
        }
        else
        {
            lstvwGradeConfigurationDetailsSubject.DataSource = null;
            lstvwGradeConfigurationDetailsSubject.DataBind();
        }

        RemoveRemarkColumn(lstvwGradeConfigurationDetailsSubject);
    }

    /// <summary>
    /// This method is used to remove remark column of config. grades.
    /// </summary>
    /// <param name="aoListView"></param>
    private void RemoveRemarkColumn(ListView aoListView)
    {
        if (miSchoolId == Constants.SchoolId.SPS.ToInt())
        {
            HtmlTableRow tr = aoListView.FindControl("trHeader") as HtmlTableRow;
            if (tr != null)
            {
                HtmlTableCell th = tr.FindControl("thRemarkSub") as HtmlTableCell;
                if (th != null)
                    th.Visible = false;
            }

            foreach (ListViewItem item in aoListView.Items)
            {
                HtmlTableCell td = item.FindControl("tdRemark") as HtmlTableCell;
                if (td != null)
                    td.Visible = false;
            }
        }
    }

    private void FillMarkGradeListViewCurricularSubject()
    {
        MarksGradesConfigurationBL oMarksGradesConfigurationBL = new MarksGradesConfigurationBL();
        oMarksGradesConfigurationBL.Academic_Year_Id = miAcademicYearId;
        oMarksGradesConfigurationBL.School_Id = miSchoolId;
        if (!mbIsOldProgressReport)
        {
            if (moUserRole == Constants.UserRoles.Admin
                        || bool.Parse(hidUserHasFullAccess.Value) || moUserRole == Constants.UserRoles.Teacher)
            {
                int iTecherId = cmbTeachers.SelectedValue.ToInt();
                DataTable oDtStudents = GetStudentData(iTecherId);
                if (cmbTeachers.SelectedValue != Constants.S_ZERO && oDtStudents != null && oDtStudents.Rows.Count > Constants.I_ZERO)
                    hidStandardId.Value = oDtStudents.Rows[0][0].ToString();
            }
            else
                hidStandardId.Value = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToString();
        }
        oMarksGradesConfigurationBL.Standard_Id = Convert.ToInt32(hidStandardId.Value);       
        oMarksGradesConfigurationBL.IsCoCurricularSubjects = true;
        DataSet oDsMarkGrades = oMarksGradesConfigurationBL.FetchMarksGradesConfigurationDetails();
        DataRow[] drArr = oDsMarkGrades.Tables[1].Select("Standard_Id IS NOT NULL");
        if (drArr.Length > 0)
        {
            lstvwGradingConfigurationDetailsCurricularSubject.DataSource = drArr.CopyToDataTable();
            lstvwGradingConfigurationDetailsCurricularSubject.DataBind();
        }
        else
        {
            lstvwGradingConfigurationDetailsCurricularSubject.DataSource = null;
            lstvwGradingConfigurationDetailsCurricularSubject.DataBind();
        }

        RemoveRemarkColumn(lstvwGradingConfigurationDetailsCurricularSubject);
    }

	#endregion    
    
    #region Public Method(s)

    /// <summary>
    /// This method is used to return encrypted query string.
    /// </summary>
    /// <param name="asQueryString"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(string asStandardId, string asStdDivId, string asTestId, string asStudentId)
    {
        return "../Common/TestReportPopup.aspx?" + CommonUtility.EncryptQuerystring("StandardId=" + asStandardId + "&StdDivId=" + asStdDivId + "&TestId=" + asTestId + "&StudentId=" + asStudentId);
    }

    private ReportDisplay DisplayPPSReport(bool IsFinalExamPublished)
    {
        ReportDisplay oReportDisplay;
        StudentProgress oStudentProgress = new StudentProgress();

        //Please comment this code after school started properly. This code is temporary.
        int iStandardId = Constants.I_ZERO;
        string sStandardName = string.Empty;
        
        int iStudentId = Constants.I_ZERO;
        if (moUserRole == Constants.UserRoles.Teacher)
            iStudentId = hidStudId.Value.ToInt();
        else if (moUserRole == Constants.UserRoles.Student)
            iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();

        DataTable oDatatable = new DataTable();
        if (mbIsOldProgressReport)
        {
            oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, hidLastAcademicYrId.Value.ToInt(), iStudentId);
            //IsFinalExamPublished = oStudentProgress.IsLatYearFinalResultPublished(hidOldStdDivId.Value.ToInt(), miSchoolId, hidLastAcademicYrId.Value.ToInt());

            hidOldStudentId.Value = oDatatable.Rows[0]["YearWise_Student_Id"].ToString();
        }
        else
        {
            oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, iStudentId);
            //IsFinalExamPublished = oStudentProgress.IsFinalResultPublished(miStdDivId);
        }

        iStandardId = oDatatable.Rows[0][1].ToInt();
        hidStandardId.Value = iStandardId.ToString();
        sStandardName = oDatatable.Rows[0]["Standard_Name"].ToString();

        hidOldStdDivId.Value = oDatatable.Rows[0]["SchoolWise_Standard_Division_Id"].ToString();

        bool bIsLateJoinee = oDatatable.Rows[0]["IsLateJoinee"].ToBool();
        if (bIsLateJoinee)
            IsFinalExamPublished = false;

        bool bIsGradingstandard;

        if (mbIsOldProgressReport)
            bIsGradingstandard = StandardMasterBL.IsGradingStandard(miSchoolId, hidLastAcademicYrId.Value.ToInt(), hidStandardId.Value.ToInt());
        else
            bIsGradingstandard = StandardMasterBL.IsGradingStandard(miSchoolId, miAcademicYearId, hidStandardId.Value.ToInt());

        if (!bIsGradingstandard)
        {
            if ((mbIsOldProgressReport ? hidLastAcademicYrId.Value.ToInt() : miAcademicYearId) <= 52)
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPS, GetFilterString(false, IsFinalExamPublished), ExportFormatType.PortableDocFormat);
            else
            {
                if (IsFinalExamPublished)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.FinalReportPP, GetFilterString(false, IsFinalExamPublished), ExportFormatType.PortableDocFormat);
                else
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.PPSTermwiseReport, GetFilterString(false, IsFinalExamPublished), ExportFormatType.PortableDocFormat);
            }
        }
        else
            oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPS_Grading, GetFilterString(true, IsFinalExamPublished), ExportFormatType.PortableDocFormat);

        int iAccYrId = (mbIsOldProgressReport ? hidLastAcademicYrId.Value.ToInt() : miAcademicYearId);
        oReportDisplay.AcademicYearId = iAccYrId;

        if (IsFinalExamPublished)
        {
            oReportDisplay.TermId = Constants.I_TWO;
            
            if (iAccYrId >= 51)
            {
                if (bIsGradingstandard)
                {
                    if (iAccYrId >= I_PPS_2025_26)
                    {
                        if (sStandardName == "5")
                        {
                            oReportDisplay.FileName = "StudentFinalProgressReportGradingFor5th_2026.rpt";
                        }
                        else if (sStandardName == "1" || sStandardName == "2" || sStandardName == "3" || sStandardName == "4")
                        {
                            oReportDisplay.FileName = "StudentFinalProgressReportGrading2026.rpt";
                        }
                    }
                    else   if (iAccYrId >= I_PPS_2022_23)
                    {
                        if (iAccYrId >= I_PPS_2023_24 && sStandardName == "5")
                            oReportDisplay.FileName = "StudentFinalProgressReportGradingFor5th_2024.rpt";
                        else
                            oReportDisplay.FileName = "StudentFinalProgressReportGrading2023.rpt";
                    }
                    else
                        oReportDisplay.FileName = "StudentFinalProgressReportGrading51.rpt";
                }
                else
                {
                    if (iAccYrId >= I_PPS_2025_26)
                    {
                       if (sStandardName == "6" || sStandardName == "7" || sStandardName == "8" || sStandardName == "9" || sStandardName == "10")
                        {
                            oReportDisplay.FileName = "FinalProgressReportPP2026.rpt";
                        }
                    }
                    else   if (iAccYrId >= I_PPS_2022_23)
                    {
                        oReportDisplay.FileName = "FinalProgressReportPP.rpt";
                    }
                    else
                    {
                        if (hidStandardName.Value == "6" || hidStandardName.Value == "7" || hidStandardName.Value == "8")
                            oReportDisplay.FileName = "StudentFinalProgressReportMarking51_6to8.rpt";
                        else
                            oReportDisplay.FileName = "StudentFinalProgressReportMarking51.rpt";
                    }
                }
            }
        }
        else
        {
            oReportDisplay.TermId = Constants.I_ONE;

            int aiAccYrId = (mbIsOldProgressReport ? hidLastAcademicYrId.Value.ToInt() : miAcademicYearId);
            if (aiAccYrId == 51 || aiAccYrId == 52)
            {
                if (!bIsGradingstandard)
                    oReportDisplay.FileName = "StudentTerm1ProgressReport51.rpt";
                else
                    oReportDisplay.FileName = "StudentwiseProgressReport51.rpt";
            }
            else if (aiAccYrId >= I_PPS_2022_23)
            {
                if (!bIsGradingstandard)
                    oReportDisplay.FileName = "TermwiseProgressReportPP.rpt";
                else
                {
                    oReportDisplay.FileName = "StudentwiseProgressReportPP53.rpt";

                    if (bIsLateJoinee)
                    {
                        oReportDisplay.TermId = 2;
                        oReportDisplay.AllowSecondTermFromTermReport = true;
                    }
                }
            }
        }


        return oReportDisplay;
    }

    #endregion

    
}