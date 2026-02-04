/*File Name - XseedProgressReportUI.aspx.cs
 * Created Date - 2-July-2011
 * Created by - Sachin
 * Class Description - This class is used to display xseed progress report.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;
using System.Web.UI;
using System.Data;
using System.Resources;
using CrystalDecisions.Shared;

public partial class XseedProgressReportUI : SchoolBase
{
    #region Data Members

    XseedProgressReportBL moXseedProgressReportBL;
    HtmlTable moTblStudentdetails;
    HtmlTable moTblGrades;
    HtmlTable moTblXseedLearningOutcomes;
    HtmlTable moTblNonXseedProgressReport;
    HtmlTable moTblCoCurricularSubjects;
    HtmlTable moNoteTable;
    HtmlTable moRemarkTable;
    bool mbIsOldProgressReport;
	private int miStudentId = 0;
	private int miStandardDivisionId = 0;
	private int miStandardId = 0;
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    private const string S_ERR_MSG_ASSESSMENT_NOT_PUBLISHED = "Assessment result is not available for this student.";

    #endregion

    private bool ShowCurrentYearData
    {
        get
        {
            return hidShowCurrentYearData.Value == Constants.S_ONE;
        }
    }

    #region Events

    /// <summary>
    /// This event is used to set masterpage.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnPreInit(EventArgs e)
    {
        try
        {
			base.OnPreInit(e);
            
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
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
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
    /// This event is used to fill class teacher and assessment combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            bool bShowCurrentYearData = false;
            if (!IsPostBack)
            {
                SetOldProgressReportUrl();
                if (CheckPreCondition())
                {
                    hidShowCurrentYearData.Value = Constants.S_ZERO;
                    if (QueryString["ShowCurrentYearData"] != null && QueryString["ShowCurrentYearData"].ToString() == Constants.S_ONE)
                    {
                        bShowCurrentYearData = true;
                        hidShowCurrentYearData.Value = Constants.S_ONE;
                    }

                    FillAcademicYearCombo(bShowCurrentYearData);
                    if (mbIsOldProgressReport)
                    {
                        if (cmbAcademicYear.Items.Count == 1)
                            cmbAcademicYear.Enabled = false;
                        else
                            lblOldAcademicYear.Text = CommonUtility.DisplayAcademicYear(cmbAcademicYear.SelectedItem.Text);

                        if (bShowCurrentYearData)
                        {
                            cmbAcademicYear.Enabled = false;
                            lblOldAcademicYear.Text = string.Empty;
                            btnPrintPreview.Visible = false;
                        }

                        SetOldAcademicYearMode(true);
                        IsXseedApplicable();
                        hlnkOldAcademicRecord.Visible = false;
                    }
                    else
                    {
                        SetOldAcademicYearMode(false);

                        if (!Convert.ToBoolean(Session[Constants.S_SESSION_IS_NEW_ADMISSION]))
                        {
                            hlnkOldAcademicRecord.Visible = true;
                            SetOldProgressReportUrl();
                        }
                        else
                            hlnkOldAcademicRecord.Visible = false;

                        if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                        {   
                            MasterPage oMasterPage = (MasterPage)this.Master;
                            SiteMapPath siteMap = (SiteMapPath)oMasterPage.FindControl("SiteMapPath1");
                            oMasterPage.NodeTitle = "Pre-Primary Progress Report";
                        }
                    }


                    FillAssessmentAndClassTeachers();
                    SetProgressReportAsPerUserRole();
                }
                else
                    btnBack.Visible = false;
                
                SetJavascriptAttributes();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    //hidBtnBack.Value = "Back";
                }
                RefreshValues();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }           
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student and assessment combobxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbClassTeacher_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
            List<YearwiseStudentMaster> lstYearwiseStudentMaster = new List<YearwiseStudentMaster>();
            if (cmbClassTeacher.SelectedValue != "0")
            {
                int iStandardDivisionId = Convert.ToInt32(cmbClassTeacher.SelectedValue);
                lstYearwiseStudentMaster = oXseedProgressReportBL.GetStudents(miSchoolId, miAcademicYearId, iStandardDivisionId, hidStudentId.Value.ToInt());
            }
            ListSource.FillDropDownList(lstYearwiseStudentMaster, cmbStudents, "StudentName", "YearwiseStudentId", Constants.S_SELECT_ALL);
            ListSource.FillDropDownList(oXseedProgressReportBL.AssessmentMaster, cmbAssessment, "Name", "AssessmentId", moUserRole == Constants.UserRoles.Student ? string.Empty : Constants.S_SELECT);
			if (moUserRole == Constants.UserRoles.Student || (moUserRole == Constants.UserRoles.Teacher && mbIsOldProgressReport))
            {
                ListItem olstdtStudents = cmbStudents.Items.FindByValue(mbIsOldProgressReport ? miStudentId.ToString() : Session[Constants.S_SESSION_STUDENT_ID].ToString());
                if (olstdtStudents != null)
                    olstdtStudents.Selected = true;

                if (oXseedProgressReportBL.AssessmentMaster.Count > Constants.I_ZERO)
                {
                    StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                    oStudentFeeDetailsBL.School_Id = miSchoolId;
                    oStudentFeeDetailsBL.Academic_Year_Id = miAcademicYearId;
                    oStudentFeeDetailsBL.Student_Id = hidStudentId.Value.ToInt();
                                       
                    //bool bIsFeePending = Settings.BlockProgressReportIfFeesArePending && oStudentFeeDetailsBL.PendingFeesAvailableForStudent();

                    bool bIsFeePending = false;
                    if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
                        bIsFeePending = Settings.BlockProgressReportIfFeesArePending && oStudentFeeDetailsBL.PendingFeesAvailableForStudent();
                    else
                        bIsFeePending = oStudentFeeDetailsBL.PendingFeesAvailableForStudent();

					ProgressReportBL oProgressReportBL = new ProgressReportBL(miSchoolId, miAcademicYearId,miUserId);
					string sBlockProgressReportReason = oProgressReportBL.GetBlockProgressReportReason(hidStudentId.Value.ToInt());
                    tdDownloadPDF.Visible = false;
                    if (!bIsFeePending && sBlockProgressReportReason.IsNullOrEmpty())
					{
                        tdDownloadPDF.Visible = true;
                        cmbAssessment.SelectedValue = oXseedProgressReportBL.AssessmentMaster.Last().AssessmentId.ToString();
                        cmbAssessment_SelectedIndexChanged(cmbAssessment, null);
                    }
                    else
                        throw new BlockProgessReport(bIsFeePending ? Resources.LocalizedResources.MsgBlockedStudentReason : string.Empty, sBlockProgressReportReason);
                }
                else
                    SetErrorMessageAttributes(Resources.LocalizedResources.MsgAssessmentResultUnAvailable, string.Empty);
            }
        }
		catch (BlockProgessReport ex)
		{
			SetErrorMessageAttributes(ex.Message, ex.BlockProgressReportReason);
		}
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    private void SetErrorMessageAttributes(string asErrorMessage, string asBlockPrgressReportReason)
    {
		if (!asErrorMessage.IsNullOrEmpty())
		{
			trErrorMessage.Visible = true;
			lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
			lblErrorMsg.Text = asErrorMessage;
		}

		if (!asBlockPrgressReportReason.IsNullOrEmpty())
		{
			lblBlockProgressReortReason.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
			lblBlockProgressReortReason.Style.Add(HtmlTextWriterStyle.Display, "inline-block");
			trBloclkProgress.Visible = true;
            lblBlockProgressReortReason.Text = Resources.LocalizedResources.MsgProhibitedProgressReport + " :<BR />" + asBlockPrgressReportReason + "<BR />" + Resources.LocalizedResources.MsgProhibitedProgressReport1;
		}

        trStudentDetails.Visible = false;
        tdcmbAssemetns.Visible = false;
        tdAssemetns.Visible = false;
        tdShow.Visible = false;
        tdPrintPreview.Visible = false;        
    }

    /// <summary>
    /// This event is used to display print preview of xseed progress report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrintPreview_Click(object sender, EventArgs e)
    {
        try
        {
            int iAcademicYearId = 0;
            if (hidIsOldReport.Value == Constants.S_YES)
                iAcademicYearId = Convert.ToInt32(cmbAcademicYear.SelectedValue);
            else
                iAcademicYearId = miAcademicYearId;
            hidQueryString.Value = CommonUtility.EncryptQuerystring(
                                "AssessmentId=" + cmbAssessment.SelectedValue +
                                "&StandardDivisionId=" + cmbClassTeacher.SelectedValue +
                                "&StudentId=" + cmbStudents.SelectedValue +
                                "&AcademicYearId=" + iAcademicYearId);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display progress report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            tblMainProgressReport.Visible = true;
            if (cmbAssessment.SelectedValue != "0" && cmbClassTeacher.SelectedValue != "0")
                DisplayProgressReport();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check whether xseed is applicable for last year standard/class.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            IsXseedApplicable();
            hlnkOldAcademicRecord.Visible = false;

            DataTable oDTOldAcademicYearDetails = StudentBL.GetYearwiseStudentDetails(miSchoolId, Convert.ToInt32(cmbAcademicYear.SelectedValue), miStudentId);
            if (oDTOldAcademicYearDetails.IsNonEmpty())
            {
                int iStudentId = Convert.ToInt32(oDTOldAcademicYearDetails.Rows[0]["Yearwise_Student_Id"]);
                int aiStandardDivisionId = Convert.ToInt32(oDTOldAcademicYearDetails.Rows[0]["Schoolwise_Standard_Division_Id"]);
                int aiStandardId = Convert.ToInt32(oDTOldAcademicYearDetails.Rows[0]["Standard_Id"]);
                string sQueryString = "IsOldProgressReport=Y&AcademcYearId=" + cmbAcademicYear.SelectedValue + "&StudentId=" + iStudentId + "&StdDivId=" + aiStandardDivisionId + "&StandardId=" + aiStandardId;
                Response.Redirect("../Xseed/XseedProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString),false);
            }
        }
		catch (System.Threading.ThreadAbortException)
		{
		}
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblMainProgressReport.Visible = false;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			if (moUserRole == Constants.UserRoles.Student || (moUserRole == Constants.UserRoles.Teacher && mbIsOldProgressReport))
            {
                tblMainProgressReport.Visible = true;
                if (cmbAssessment.SelectedValue != "0" && cmbClassTeacher.SelectedValue != "0")
                    DisplayProgressReport();
            }
            else
                tblMainProgressReport.Visible = false;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Download the PDF.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = null;
            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())                            
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_Xseed, FilterString(), ExportFormatType.PortableDocFormat);
            else if(miSchoolId == Constants.SchoolId.PPS.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.XseedProgressReport_PPS, FilterString(), ExportFormatType.PortableDocFormat);

            oReportDisplay.DisplayReport();            
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    private string FilterString()
    {
        string sFilterString = string.Empty;

        int iAcYearId = miAcademicYearId;
        if (cmbAcademicYear.SelectedValue != string.Empty)
            iAcYearId = cmbAcademicYear.SelectedValue.ToInt();

        DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, iAcYearId, Session[Constants.S_SESSION_STUDENT_ID].ToInt());

        int iAcademicYearId;
        int iStdDivId;
        int iStudentId;
        int iStdid;
        if (mbIsOldProgressReport)
        {
            iAcademicYearId = cmbAcademicYear.SelectedValue.ToInt();
            iStdDivId = oDatatable.Rows[0]["Schoolwise_Standard_Division_Id"].ToInt();
            iStudentId = oDatatable.Rows[0]["YearWise_Student_Id"].ToInt();
            iStdid = oDatatable.Rows[0]["Standard_Id"].ToInt();
        }
        else
        {
            iAcademicYearId = miAcademicYearId;
            iStdDivId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
            iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
            iStdid = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
        }


        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            sFilterString = "(usp_GetXseedProgressReport_PPSH.School_Id}=" + miSchoolId + "AND usp_GetXseedProgressReport_PPSH.Academic_Year_Id}=" + iAcademicYearId + "AND usp_GetXseedProgressReport_PPSH.YearwiseStudentId}=" + iStudentId + "AND usp_GetXseedProgressReport_PPSH.Standard_Id}=" + iStdid + "AND usp_GetXseedProgressReport_PPSH.AssessmentId}=" + cmbAssessment.SelectedValue + "AND usp_GetXseedProgressReport_PPSH.SchoolWise_Standard_Division_Id}=" + iStdDivId + ") @";
        else if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            sFilterString = "(Xseed.usp_GetXseedProgressReport.School_Id}=" + miSchoolId + "AND Xseed.usp_GetXseedProgressReport.Academic_Year_Id}=" + iAcademicYearId + "AND Xseed.usp_GetXseedProgressReport.YearwiseStudentId}=" + iStudentId + "AND Xseed.usp_GetXseedProgressReport.Standard_Id}=" + iStdid + "AND Xseed.usp_GetXseedProgressReport.SchoolWise_Standard_Division_Id}=" + iStdDivId + "AND Xseed.usp_GetXseedProgressReport.AssessmentId}=" + cmbAssessment.SelectedValue + "AND Xseed.usp_GetXseedProgressReport.IsFromReportScreen}=0" + ") @";        

        return sFilterString;
    }

    /// <summary>
    /// This method is used to create tables for progress report.
    /// </summary>
    private void CreateTables()
    {
        moTblStudentdetails = new HtmlTable();

        moTblGrades = new HtmlTable();
        moTblXseedLearningOutcomes = new HtmlTable();
        moTblNonXseedProgressReport = new HtmlTable();
        moTblCoCurricularSubjects = new HtmlTable();
        moNoteTable = new HtmlTable();
        
        AddTable(moTblStudentdetails);
        AddTable(moTblGrades);
        AddTable(moTblXseedLearningOutcomes);
        AddTable(moTblNonXseedProgressReport);
        AddTable(moTblCoCurricularSubjects);
        
        moRemarkTable = new HtmlTable();        
        AddTable(moRemarkTable);
        
        AddTable(moNoteTable);

        CreateGradeHeaders();
        CreateLearningOutcomeHeader();

        if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
        {
            CreateNonXseedOutcomesHeader();
            CreateCoCurricularSubjectsHeaders();
            CreateNoteTable();
        }
    }

    /// <summary>
    /// This method is used to create progress report note table.
    /// </summary>
    private void CreateNoteTable()
    {
       // AddEmptyRow(moNoteTable);
        HtmlTableRow trProgressReportNote = new HtmlTableRow();
        HtmlTableCell tdProgressReportNote = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Note + ":", Align = "left" };
        tdProgressReportNote.Style.Add("Padding-Left", "5px");
        tdProgressReportNote.Attributes.Add("class", "clsLabel");
        trProgressReportNote.Cells.Add(tdProgressReportNote);
        moNoteTable.Rows.Add(trProgressReportNote);
    }

    /// <summary>
    /// This method is used to createco-curricular subjects.
    /// </summary>
    private void CreateCoCurricularSubjectsHeaders()
    {
        HtmlTableRow trCoCurricularSubjectsHeaders = new HtmlTableRow();

        trCoCurricularSubjectsHeaders.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = Resources.LocalizedResources.CoCurricularSubjects, Align = "Center" });
        trCoCurricularSubjectsHeaders.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblCoCurricularSubjects.Rows.Add(trCoCurricularSubjectsHeaders);

        trCoCurricularSubjectsHeaders = new HtmlTableRow();
        HtmlTableCell tdCoCurricularSubjectsHeaders = new HtmlTableCell { ColSpan = 2, InnerHtml = Resources.LocalizedResources.Subject, Align = "Left", Width = "240px" };
        tdCoCurricularSubjectsHeaders.Style.Add("Padding-Left", "5px");
        trCoCurricularSubjectsHeaders.Cells.Add(tdCoCurricularSubjectsHeaders);
        trCoCurricularSubjectsHeaders.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade, Width = "100px", Align = "Center" });

        tdCoCurricularSubjectsHeaders = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.FacilitatorsObservation, Width = "550px", Align = "Left" };
        tdCoCurricularSubjectsHeaders.Style.Add("Padding-Left", "5px");
        trCoCurricularSubjectsHeaders.Cells.Add(tdCoCurricularSubjectsHeaders);

        trCoCurricularSubjectsHeaders.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblCoCurricularSubjects.Rows.Add(trCoCurricularSubjectsHeaders);
    }

    /// <summary>
    /// This method is used to create non xseed learning outcome header.
    /// </summary>
    private void CreateNonXseedOutcomesHeader()
    {
        HtmlTableRow thNonXseedProgressReport = new HtmlTableRow();
        thNonXseedProgressReport.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = Resources.LocalizedResources.NonXseedCurricularSubjects , Align = "Center" });
        thNonXseedProgressReport.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblNonXseedProgressReport.Rows.Add(thNonXseedProgressReport);

        thNonXseedProgressReport = new HtmlTableRow();
        HtmlTableCell tdNonXseedProgressReport = new HtmlTableCell { ColSpan = 2, InnerHtml = Resources.LocalizedResources.Subject , Align = "Left", Width = "400px" };
        tdNonXseedProgressReport.Style.Add("Padding-Left", "5px");
        thNonXseedProgressReport.Cells.Add(tdNonXseedProgressReport);
        thNonXseedProgressReport.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade , Width = "100px", Align = "Center" });

        tdNonXseedProgressReport = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.FacilitatorsObservation , Width = "400px", Align = "Left" };
        tdNonXseedProgressReport.Style.Add("Padding-Left", "5px");
        thNonXseedProgressReport.Cells.Add(tdNonXseedProgressReport);
        thNonXseedProgressReport.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblNonXseedProgressReport.Rows.Add(thNonXseedProgressReport);
    }

    /// <summary>
    /// This method is use to create learning outcome table header,
    /// </summary>
    private void CreateLearningOutcomeHeader()
    {
        string sHeader = Resources.LocalizedResources.XseedCurricularSubjects;

        if (miSchoolId == Constants.SchoolId.PPS.ToInt())        
            sHeader = "Pre-Primary Curricular Subjects";

        HtmlTableRow thLearningOutcome = new HtmlTableRow();
        thLearningOutcome.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = sHeader, Align = "Center" });
        thLearningOutcome.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblXseedLearningOutcomes.Rows.Add(thLearningOutcome);

        thLearningOutcome = new HtmlTableRow();
        thLearningOutcome.Cells.Add(new HtmlTableCell { InnerHtml =Resources.LocalizedResources.SrNo , Align = "Center", Width = "60px" });

        HtmlTableCell tdLearningOutcome = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.LearningOutcome , Align = "Left", Width = "380px" };
        tdLearningOutcome.Style.Add("Padding-Left", "5px");
        thLearningOutcome.Cells.Add(tdLearningOutcome);

        thLearningOutcome.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade , Width = "100px", Align = "Center" });

        tdLearningOutcome = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.FacilitatorsObservation , Align = "Left" };
        tdLearningOutcome.Style.Add("Padding-Left", "5px");
        thLearningOutcome.Cells.Add(tdLearningOutcome);
        thLearningOutcome.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblXseedLearningOutcomes.Rows.Add(thLearningOutcome);
    }

    /// <summary>
    /// This method is used to create grade header.
    /// </summary>
    private void CreateGradeHeaders()
    {
        HtmlTableRow thGradeDetails = new HtmlTableRow();
        thGradeDetails.Cells.Add(new HtmlTableCell { ColSpan = 2, InnerHtml = Resources.LocalizedResources.KeyToCurricularAndCoCurricular , Align = "Center" });
        thGradeDetails.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblGrades.Rows.Add(thGradeDetails);

        thGradeDetails = new HtmlTableRow();
        HtmlTableCell tdGradeDetails = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade , Align = "left", Width="100px" };
        tdGradeDetails.Style.Add("Padding-Left", "5px");
        thGradeDetails.Cells.Add(tdGradeDetails);

        tdGradeDetails = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Description, Align = "left", Width = "80%" };
        tdGradeDetails.Style.Add("Padding-Left", "5px");
        thGradeDetails.Cells.Add(tdGradeDetails);

        thGradeDetails.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblGrades.Rows.Add(thGradeDetails);
    }

    /// <summary>
    /// This method is used to add sub tables into main table.
    /// </summary>
    /// <param name="ahtmlTable"></param>
    private void AddTable(HtmlTable ahtmlTable)
    {
        HtmlTableRow trSubTable = new HtmlTableRow();
        HtmlTableCell tdSubTable = new HtmlTableCell { Width = "90%" };
        tdSubTable.Controls.Add(ahtmlTable);
        trSubTable.Cells.Add(tdSubTable);
        tdSubTable.Align = "Center";
        ahtmlTable.Width = "90%";
        tblMainProgressReport.Rows.Add(trSubTable);
    }

    /// <summary>
    /// This method is used to add empty ow in given table.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    private void AddEmptyRow(HtmlTable aoHtmlTable)
    {
        HtmlTableRow trEmpty = new HtmlTableRow();
        HtmlTableCell tdEmpty = new HtmlTableCell { Width = "90%" };
        trEmpty.Cells.Add(tdEmpty);
        trEmpty.Height = "10px";
        aoHtmlTable.Rows.Add(trEmpty);
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnBack,btnShow,btnPrintPreview});
        valSummary.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        if (moUserRole != Constants.UserRoles.Student)
            trStudentDetails.Visible = true;                    
    }

    /// <summary>
    /// This method is used to set progress report view as per login user role.
    /// </summary>
    private void SetProgressReportAsPerUserRole()
    {
        if (moUserRole == Constants.UserRoles.Student)
        {
            hidStudentId.Value = (mbIsOldProgressReport ? miStudentId.ToString() : Session[Constants.S_SESSION_STUDENT_ID].ToString());

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt() || miSchoolId == Constants.SchoolId.PPS.ToInt())
                tdDownloadPDF.Visible = true;

            cmbClassTeacher.SelectedValue = mbIsOldProgressReport ? miStandardDivisionId.ToString() : Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToString();
            cmbClassTeacher_SelectedIndexChanged(cmbClassTeacher, null);

            trStudentDetails.Visible = false;
            btnShow.Visible = false;
            lblMandetory.Visible = false;           

            if (cmbAssessment.Items.Count == Constants.I_ZERO)
            {
                tmpRow.Visible = false;
                trErrorMessage.Visible = true;
                lblErrorMsg.Text = S_ERR_MSG_ASSESSMENT_NOT_PUBLISHED;
                tdDownloadPDF.Visible = false;
            }
        }
        else if (moUserRole == Constants.UserRoles.Teacher && (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedProgressReport) != Constants.C_YES))
        {
            cmbClassTeacher.SelectedValue = hidstdDivId.Value;
            cmbClassTeacher_SelectedIndexChanged(cmbClassTeacher, null);
            tdClassTeacher.Visible = false;
            //tdStudents.Visible = tdCmbStudents.Visible = false;	
            tdcmbClassTeacher.Visible = false;
            lblMandetory.Visible = true;
            hlnkOldAcademicRecord.Visible = false;
        }
		else if (moUserRole == Constants.UserRoles.Teacher && mbIsOldProgressReport)
		{
			hidStudentId.Value = miStudentId.ToString();
			cmbStudents.SelectedValue = miStudentId.ToString();
			cmbClassTeacher.SelectedValue = mbIsOldProgressReport ? miStandardDivisionId.ToString() : Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToString();
			cmbClassTeacher_SelectedIndexChanged(cmbClassTeacher, null);
			tdStudents.Visible = tdClassTeacher.Visible = tdcmbClassTeacher.Visible = tdCmbStudents.Visible = false;			
			trStudentDetails.Visible = false;			
			trStudentDetails.Visible = false;
			btnShow.Visible = false;
			lblMandetory.Visible = false;
            hlnkOldAcademicRecord.Visible = false;
		}
    }

    /// <summary>
    /// This method is used to set old academic year mode.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetOldAcademicYearMode(bool abFlag)
    {
        if (cmbAcademicYear.Items.Count > 0)
            hlnkOldAcademicRecord.Visible = !abFlag;
        else
            hlnkOldAcademicRecord.Visible = false;

        cmbAcademicYear.Visible = abFlag;
        tdAcademicYrs.Visible = abFlag;
        if (abFlag)
        {
            btnBack.Text = Resources.LocalizedResources.Close;
            hidBtnBack.Value = "Close";
            btnBack.Attributes.Add("onclick", "window.close()");
        }
        else
        {
            btnBack.Text = Resources.LocalizedResources.Back;
            hidBtnBack.Value = "Back";
            btnBack.Visible = false;
        }

        if (moUserRole != Constants.UserRoles.Student)
            hlnkOldAcademicRecord.Visible = false;
    }

    /// <summary>
    /// This method is used to check whether xseed is applicable for selected student/teacher.
    /// </summary>
    private void IsXseedApplicable()
    {
        if (hidIsOldReport.Value == Constants.S_YES)
        {
			int iStandardId = miStandardId;// Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
			DataTable oDatatable = StudentBL.GetYearwiseStudentDetails(miSchoolId, Convert.ToInt32(cmbAcademicYear.SelectedValue), miStudentId);
			iStandardId = oDatatable.Rows[0][1].ToInt();
            int iTeachersStandardDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_STDDIV_ID]);
            int iAcademicYearId = Convert.ToInt32(cmbAcademicYear.SelectedValue);
            XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
			string bIsTeacherLogin=moUserRole == Constants.UserRoles.Teacher ? "true" : "false";
            if (!oXseedProgressReportBL.IsXseedApplicable(miSchoolId, iAcademicYearId, iStandardId, iTeachersStandardDivisionId))
            {
				string sQueryString = "IsOldProgressReport=True&AcademcYearId=" + cmbAcademicYear.SelectedValue + "&StudentId=" + miStudentId + "&StdDivId=" + miStandardDivisionId + "&IsTeacherLogin=" +bIsTeacherLogin ;
                Response.Redirect("../Student/StudentProgressSheet.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
            }
        }
    }

    /// <summary>
    /// This method is used to fill class teacher and assessment combobox.
    /// </summary>
    private void FillAssessmentAndClassTeachers()
    {
        int iAcademicYearId = 0;
        if (hidIsOldReport.Value == Constants.S_YES)
            iAcademicYearId = Convert.ToInt32(cmbAcademicYear.SelectedValue);
        else
            iAcademicYearId = miAcademicYearId;

        XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
        oXseedProgressReportBL.GetClassTeachers(miSchoolId, iAcademicYearId);        
        ListSource.FillDropDownList(oXseedProgressReportBL.ClassTeacherDetails, cmbClassTeacher, "TeacherName", "StandardDivisionId", Constants.S_SELECT);
        if (moUserRole == Constants.UserRoles.Teacher && (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedProgressReport) != Constants.C_YES))
        {
            hidstdDivId.Value = oXseedProgressReportBL.ClassTeacherDetails
                                                      .Where(teacher => teacher.TeacherId == Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]))
                                                      .Select(teacher => teacher.StandardDivisionId)
                                                      .FirstOrDefault()
                                                      .ToString();

            List<AssessmentMaster> lstAssessments = oXseedProgressReportBL.GetPublishedAssesments(miSchoolId, iAcademicYearId, hidstdDivId.Value.ToInt());
            if (lstAssessments.Count == 0)
            {
                tmpRow.Visible = false;
                trErrorMessage.Visible = true;
                lblErrorMsg.Text = "Published assessments are not available.";
            }
            else
                ListSource.FillDropDownList(lstAssessments, cmbAssessment, "Name", "AssessmentId", moUserRole == Constants.UserRoles.Student ? string.Empty : Constants.S_SELECT);
        }
        else if(ShowCurrentYearData)
        {
            List<AssessmentMaster> lstAssessments = oXseedProgressReportBL.GetPublishedAssesments(miSchoolId, iAcademicYearId, QueryString["StdDivId"].ToInt());

            cmbClassTeacher.SelectedValue = QueryString["StdDivId"].ToString();
            cmbClassTeacher.Enabled = false;
            cmbClassTeacher_SelectedIndexChanged(cmbClassTeacher, null);

            cmbStudents.SelectedValue = QueryString["StudentId"].ToString();
            cmbStudents.Enabled = false;

            if (lstAssessments.Count == 0)
            {
                tmpRow.Visible = false;
                trErrorMessage.Visible = true;
                lblErrorMsg.Text = "Published assessments are not available.";
            }
            else
            {
                ListSource.FillDropDownList(lstAssessments, cmbAssessment, "Name", "AssessmentId", Constants.S_SELECT);

                if (lstAssessments.Count == 1)
                    cmbAssessment.SelectedIndex = 1;
                else
                    cmbAssessment.SelectedIndex = 2;
                
                btnShow_Click(btnShow, null);
            }
        }
        else
            ListSource.FillDropDownList(oXseedProgressReportBL.AssessmentMaster, cmbAssessment, "Name", "AssessmentId", moUserRole == Constants.UserRoles.Student ? string.Empty : Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to display progress report.
    /// </summary>
    private void DisplayProgressReport()
    {
        int iStudentId = Convert.ToInt32(cmbStudents.SelectedValue);

        if (moUserRole == Constants.UserRoles.Student && iStudentId == 0)
            iStudentId = hidStudentId.Value.ToInt();

        int iAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        int iStandDivisionId = Convert.ToInt32(cmbClassTeacher.SelectedValue);
        int iAcademicYearId = 0;

        if (moUserRole == Constants.UserRoles.Student && hidIsOldReport.Value == Constants.S_YES)
            iAcademicYearId = Convert.ToInt32(cmbAcademicYear.SelectedValue);
        else
            iAcademicYearId = miAcademicYearId;

        moXseedProgressReportBL = new XseedProgressReportBL
        {
            ExamResult = new ExamResult
            {
                SchoolId = miSchoolId,
                AcademicYearId = iAcademicYearId,
                AssessmentId = iAssessmentId,
                YearwiseStudentId = iStudentId,
                StandardDivisionId = iStandDivisionId
            }
        };
        moXseedProgressReportBL.GetXseedProgressReport();
        if (!moXseedProgressReportBL.AssessmentPublishStatus && !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus)
        {
            trErrorMessage.Visible = true;
            lblErrorMsg.Text = Resources.LocalizedResources.MsgAssessmentResultUnAvailable;
        }

        else
            SetStudentwiseProgressReport();
    }

    /// <summary>
    /// This method is used to set standardwise progress report.
    /// </summary>
    private void SetStudentwiseProgressReport()
    {
        bool bIsReportAvailable = false;
        if (moXseedProgressReportBL.YearwiseStudentMaster.Count > 0)
        {
            trErrorMessage.Visible = false;
            moXseedProgressReportBL.YearwiseStudentMaster.ForEach
             (
                student =>
                {
                    List<StudentsLearningOutcome> learningOutcome = moXseedProgressReportBL.StudentsLearningOutcomes.Where(stud => stud.YearwiseStudentId == student.YearwiseStudentId).ToList();
                    if (learningOutcome.Count > 0)
                    {
                        CreateTables();
                        moTblStudentdetails.Rows.Add(GetSchoolDetails("SocietyName", moXseedProgressReportBL.SchoolEntity.OrganizationName));
                        moTblStudentdetails.Rows.Add(GetSchoolDetails("ActualSchoolName", moXseedProgressReportBL.SchoolEntity.SchoolName));
                        moTblStudentdetails.Rows.Add(GetSchoolDetails("ClsReportHead", Resources.LocalizedResources.ProgressReport));
                        FillStudentDetails(student.YearwiseStudentId);
                        FillGrades();
                        FillAssessmentAndAttendanceDetails(student.YearwiseStudentId);
                        FillLearingOutcomeDetails(student.YearwiseStudentId);
                        FillRemark(student.YearwiseStudentId);
                        FillNotes();
                        AddSeperater();
                        bIsReportAvailable = true;
                    }
                }
            );
        }

        if (!bIsReportAvailable)
            trErrorMessage.Visible = true;
        else
            tblMainProgressReport.Rows.RemoveAt(tblMainProgressReport.Rows.Count - 1);
    }

    /// <summary>
    /// This method is used to display remark.
    /// </summary>
    /// <param name="aiYearwiseStudentId"></param>
    private void FillRemark(int aiYearwiseStudentId)
    {
        string sRemark = string.Empty;

        if (moXseedProgressReportBL.XseedRemarks.Where(rmk => rmk.YearwiseStudentId == aiYearwiseStudentId).Any())
            sRemark = moXseedProgressReportBL.XseedRemarks.Where(rmk => rmk.YearwiseStudentId == aiYearwiseStudentId).Select(rmk => rmk.Remark).FirstOrDefault();

        if (sRemark.Trim() != string.Empty)
        {
            HtmlTableRow trRemark = new HtmlTableRow();
            HtmlTableCell tdRemark = new HtmlTableCell();
            Label lblRemark = new Label { Text = "Remark ", CssClass = "ClsLabel" };
            tdRemark.Width = "100px";
            tdRemark.Attributes.Add("class", "ClsBorderLight");
            tdRemark.Controls.Add(lblRemark);
            trRemark.Cells.Add(tdRemark);

            tdRemark = new HtmlTableCell();

            Label lblComment = new Label();
            lblComment.CssClass = "ClsLabel";
            lblComment.Width = Unit.Percentage(100);

            lblComment.Text = sRemark;

            tdRemark.Controls.Add(lblComment);
            tdRemark.Align = "Justified";
            tdRemark.Attributes.Add("Padding-Left", "5px");
            tdRemark.Attributes.Add("Padding-Top", "5px");
            tdRemark.Attributes.Add("Class", "ClsBorderLight");

            trRemark.Cells.Add(tdRemark);
            moRemarkTable.Rows.Add(trRemark);
        }       
    }

    /// <summary>
    /// This method is used to fill note details.
    /// </summary>
    private void FillNotes()
    {
        HtmlTableRow trNote;
        HtmlTableCell tdNote;
        moXseedProgressReportBL.GradeMaster.Where(grade => grade.ConsideredAsAbsent || grade.ConsideredAsExempted).OrderBy(grade => grade.SortOrder).ToList()
            .ForEach
            (
                grade =>
                {
                    trNote = new HtmlTableRow();
                    tdNote = new HtmlTableCell { InnerHtml = grade.GradeName + " - " + grade.Description, Align = "Left" };
                    tdNote.Style.Add("Padding-left", "5px");
                    tdNote.Attributes.Add("class", "clsLabel");
                    trNote.Cells.Add(tdNote);
                    moNoteTable.Rows.Add(trNote);
                }
            );
    }

    /// <summary>
    /// This method is used to add seperator.
    /// </summary>
    private void AddSeperater()
    {
        HtmlTableRow trSeperator = new HtmlTableRow();
        HtmlTableCell tdSeperator = new HtmlTableCell
                                        {
                                            Width = "100%",
                                            InnerHtml =
                                                "------------------------------------------------------------------------------------------------------------------------------------------------------------------------"
                                        };
        trSeperator.Cells.Add(tdSeperator);
        trSeperator.Height = "10px";
        tblMainProgressReport.Rows.Add(trSeperator);
    }

    /// <summary>
    /// This method is used to fill assessment and attendance details.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillAssessmentAndAttendanceDetails(int aiStudentId)
    {
        HtmlTableRow trAssessmentsAndAttendance = new HtmlTableRow();
        HtmlTableCell tdAssessmentsAndAttendance = new HtmlTableCell
                                                       {
                                                           InnerHtml = Resources.LocalizedResources.Assessment,
                                                           Align = "Center",
                                                           Width = "110px"
                                                       };
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        tdAssessmentsAndAttendance = new HtmlTableCell { ColSpan = 3 };
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
        tdAssessmentsAndAttendance.InnerHtml = moXseedProgressReportBL.YearwiseStudentMaster.Where(student => student.YearwiseStudentId == aiStudentId).FirstOrDefault().Assessment;
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);
        tdAssessmentsAndAttendance = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Attendance, ColSpan = 2, Align = "Right" };
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
        tdAssessmentsAndAttendance.Style.Add("Padding-Right", "5px");
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        string sPresentDays = string.Empty;
        List<StudentAttendance> lstStudentAttendance = moXseedProgressReportBL.StudentAttendance.Where(attendance => attendance.YearwiseStudentId == aiStudentId).ToList();
        if (lstStudentAttendance.Count > 0)
            sPresentDays = lstStudentAttendance.Where(student => student.IsPresent).Count() + " / " + lstStudentAttendance.Count;

        tdAssessmentsAndAttendance = new HtmlTableCell { ColSpan = 2, InnerHtml = sPresentDays };
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        moTblStudentdetails.Rows.Add(trAssessmentsAndAttendance);
    }

    /// <summary>
    /// This method is used to fill grades.
    /// </summary>
    private void FillGrades()
    {
        moXseedProgressReportBL.GradeMaster.Where(grade => !grade.ConsideredAsAbsent && !grade.ConsideredAsExempted).ToList().ForEach
            (
                grade =>
                {
                    HtmlTableRow trGradeDetails = new HtmlTableRow();
                    HtmlTableCell tdGradeDetails = new HtmlTableCell();
                    tdGradeDetails.Style.Add("Padding-Left", "5px");
                    tdGradeDetails.InnerHtml = grade.GradeName;
                    trGradeDetails.Cells.Add(tdGradeDetails);

                    tdGradeDetails = new HtmlTableCell();
                    tdGradeDetails.Style.Add("Padding-Left", "5px");
                    tdGradeDetails.InnerHtml = grade.Description == string.Empty ? "-" : grade.Description;
                    trGradeDetails.Cells.Add(tdGradeDetails);

                    trGradeDetails.Attributes.Add("class", "ClsMarksCell");
                    moTblGrades.Rows.Add(trGradeDetails);
                }
            );
    }

    /// <summary>
    /// This method is used to fill learning outcomes into table.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillLearingOutcomeDetails(int aiStudentId)
    {
        List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster = moXseedProgressReportBL.SubjectSections
                               .Join(moXseedProgressReportBL.LearningOutcomesObservations, subjectSection => subjectSection.SubjectSectionConfigurationId, outcome => outcome.SubjectSectionConfigurationId, (subjectSection, outcome) => new { subjectSection = subjectSection, LearningOutcome = outcome })
                               .Where(student => student.LearningOutcome.YearwiseStudentId == aiStudentId)
                               .Select(subjetSection => subjetSection.subjectSection)
                               .Distinct()
                               .ToList();

        if (moXseedProgressReportBL.StudentsLearningOutcomes.Count > 0)
        {
            trErrorMessage.Visible = false;
            lstSubjectSectionConfigurationMaster.ForEach
                                        (
                                            subjectSection =>
                                            {
                                                HtmlTableRow trSubjectSection = GetSubjectSectionCell(subjectSection.SubjectSectionName);
                                                moTblXseedLearningOutcomes.Rows.Add(trSubjectSection);
                                                FillLearningOutcomes(subjectSection.SubjectSectionConfigurationId, aiStudentId);
                                            }
                                        );

            moTblXseedLearningOutcomes.Visible = lstSubjectSectionConfigurationMaster.Count > 0;
            
            if(miSchoolId != Constants.SchoolId.PPSH.ToInt())
                FillNonXseedSubjectDetails(aiStudentId);
        }
        else
            trErrorMessage.Visible = true;
    }

    /// <summary>
    /// This method is used to fill student details.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillStudentDetails(int aiStudentId)
    {
        YearwiseStudentMaster oYearwiseStudentMaster = moXseedProgressReportBL.YearwiseStudentMaster
                               .Where(student => student.YearwiseStudentId == aiStudentId)
                               .FirstOrDefault();

        if (oYearwiseStudentMaster != null)
        {
            HtmlTableRow trStudentDetail = new HtmlTableRow();
            HtmlTableCell tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = Resources.LocalizedResources.RollNo };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");

            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center" };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            tdStudentDetails.InnerHtml = oYearwiseStudentMaster.RollNo.ToString();
            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = Resources.LocalizedResources.Name };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Left", InnerHtml = oYearwiseStudentMaster.StudentName };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = Resources.LocalizedResources.Class };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Left", InnerHtml = oYearwiseStudentMaster.Class };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = Resources.LocalizedResources.Year };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
            trStudentDetail.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Left", InnerHtml = oYearwiseStudentMaster.AcademicYear };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            trStudentDetail.Cells.Add(tdStudentDetails);

            moTblStudentdetails.Rows.Add(trStudentDetail);
        }
    }

    /// <summary>
    /// This method is used to return school details.
    /// </summary>
    /// <param name="asClass"></param>
    /// <param name="asName"></param>
    /// <returns></returns>
    private HtmlTableRow GetSchoolDetails(string asClass, string asName)
    {
        HtmlTableRow OHtmlTableRow = new HtmlTableRow();
        HtmlTableCell OHtmlTableCell = new HtmlTableCell { ColSpan = 8, Align = "Center", InnerHtml = asName };
        OHtmlTableRow.Cells.Add(OHtmlTableCell);
        OHtmlTableRow.Attributes.Add("class", asClass);
        moTblStudentdetails.Rows.Add(OHtmlTableRow);
        return OHtmlTableRow;
    }

    /// <summary>
    /// This method is used to fill non xseed subject details.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillNonXseedSubjectDetails(int aiStudentId)
    {
        List<NonXseedSubjectGrades> lstNonXseedSubjectGardes = moXseedProgressReportBL.NonXseedSubjectGrades.Where(grade => grade.YearwiseStudentId == aiStudentId && !grade.IsCoCurricularActivity).ToList();
        lstNonXseedSubjectGardes.ForEach(grade => moTblNonXseedProgressReport.Rows.Add(GetSubjectGradeCell(grade)));
        moTblNonXseedProgressReport.Visible = lstNonXseedSubjectGardes.Count > 0;

        lstNonXseedSubjectGardes = moXseedProgressReportBL.NonXseedSubjectGrades.Where(grade => grade.YearwiseStudentId == aiStudentId && grade.IsCoCurricularActivity).ToList();
        lstNonXseedSubjectGardes.ForEach(grade => moTblCoCurricularSubjects.Rows.Add(GetSubjectGradeCell(grade)));
        moTblCoCurricularSubjects.Visible = lstNonXseedSubjectGardes.Count > 0;
    }

    /// <summary>
    /// This method is used to returnsubject grade cell.
    /// </summary>
    /// <param name="aoGrade"></param>
    /// <returns></returns>
    private HtmlTableRow GetSubjectGradeCell(NonXseedSubjectGrades aoGrade)
    {
        HtmlTableRow trGradeDetails = new HtmlTableRow();
        HtmlTableCell tdGradeDetails = new HtmlTableCell
                                           {
                                               InnerHtml = aoGrade.SubjectName,
                                               ColSpan = 2,
                                               Align = "left"
                                           };

        tdGradeDetails.Style.Add("padding-left", "5px");
        trGradeDetails.Cells.Add(tdGradeDetails);

        tdGradeDetails = new HtmlTableCell { InnerHtml = aoGrade.ShortName, Align = "Center" };
        trGradeDetails.Cells.Add(tdGradeDetails);

        tdGradeDetails = new HtmlTableCell { InnerHtml = aoGrade.Observation, Align = "left" };
        tdGradeDetails.Style.Add("padding-left", "5px");
        trGradeDetails.Cells.Add(tdGradeDetails);

        trGradeDetails.Attributes.Add("class", " ClsMarksCell");

        return trGradeDetails;
    }

    /// <summary>
    /// This method is used to fill learning outcomes.
    /// </summary>
    /// <param name="aiSubjectSectionConfigurationId"></param>
    /// <param name="aiStudentId"></param>
    private void FillLearningOutcomes(int aiSubjectSectionConfigurationId, int aiStudentId)
    {
        string sObservation = moXseedProgressReportBL.LearningOutcomesObservations
            .Where(observation => observation.SubjectSectionConfigurationId == aiSubjectSectionConfigurationId && observation.YearwiseStudentId == aiStudentId)
            .Select(observation => observation.Observation)
            .FirstOrDefault();

        List<StudentsLearningOutcome> lstStudentsLearningOutcomes = moXseedProgressReportBL.StudentsLearningOutcomes
                                .Where(outcome => outcome.SubjectSectionConfigId == aiSubjectSectionConfigurationId && outcome.YearwiseStudentId == aiStudentId).OrderBy(sortorder=> sortorder.LearningOutcomeSortOrder)
                                .ToList();

        HtmlTableRow trFirstRow;
        bool bIsFirstRow = false;
        HtmlTableRow oHtmlTableFirstRow = new HtmlTableRow();
        int iRowIndex = 1;
        lstStudentsLearningOutcomes.ForEach
            (
                learningOutcome =>
                {
                    trFirstRow = GetLearningOutcomeCell(learningOutcome.LearningOutcome, learningOutcome.ShortName, iRowIndex++);
                    if (!bIsFirstRow)
                    {
                        oHtmlTableFirstRow = trFirstRow;
                        bIsFirstRow = true;
                    }
                    moTblXseedLearningOutcomes.Rows.Add(trFirstRow);
                }
            );

        HtmlTableCell tdObservation = CreateObservationCell(sObservation, lstStudentsLearningOutcomes.Count);
        tdObservation.RowSpan = lstStudentsLearningOutcomes.Count;
        oHtmlTableFirstRow.Cells.Add(tdObservation);
    }

    /// <summary>
    /// This method is used to create observation cell.
    /// </summary>
    /// <param name="asObservation"></param>
    /// <param name="aiRowSpan"></param>
    /// <returns></returns>
    private HtmlTableCell CreateObservationCell(string asObservation, int aiRowSpan)
    {
        HtmlTableCell tdObservation = new HtmlTableCell { InnerHtml = asObservation, Align = "left" };
        tdObservation.Style.Add("padding-left", "5px");
        tdObservation.RowSpan = aiRowSpan;
        return tdObservation;
    }

    /// <summary>
    /// This method is used to return subject section cell.
    /// </summary>
    /// <param name="asSubjectSection"></param>
    /// <returns></returns>
    private HtmlTableRow GetSubjectSectionCell(string asSubjectSection)
    {
        HtmlTableRow trSubjectSection = new HtmlTableRow();
        HtmlTableCell tdSubjectSection = new HtmlTableCell
                                             {
                                                 ColSpan = 4,
                                                 InnerHtml = asSubjectSection,
                                                 Align = "center"
                                             };
        tdSubjectSection.Attributes.Add("class", "ProgressReportHeader");
        trSubjectSection.Cells.Add(tdSubjectSection);
        return trSubjectSection;
    }

    /// <summary>
    /// This method is used to return learning outcome cell.
    /// </summary>
    /// <param name="asLearningOutcome"></param>
    /// <param name="asGradeName"></param>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private HtmlTableRow GetLearningOutcomeCell(string asLearningOutcome, string asGradeName, int aiRowIndex)
    {
        HtmlTableRow trLearningOutcome = new HtmlTableRow();
        HtmlTableCell tdLearningOutcome;

        tdLearningOutcome = new HtmlTableCell { Align = "Center", InnerHtml = aiRowIndex.ToString() };
        trLearningOutcome.Cells.Add(tdLearningOutcome);

        tdLearningOutcome = new HtmlTableCell { InnerHtml = asLearningOutcome, Align = "left" };
        tdLearningOutcome.Style.Add("padding-left", "5px");
        trLearningOutcome.Cells.Add(tdLearningOutcome);

        tdLearningOutcome = new HtmlTableCell { InnerHtml = asGradeName, Align = "Center" };
        trLearningOutcome.Cells.Add(tdLearningOutcome);
        trLearningOutcome.Attributes.Add("class", "Lbl10pt ClsMarksCell");

        return trLearningOutcome;
    }

    /// <summary>
    /// This method is used to set old progress report url.
    /// </summary>
    private void SetOldProgressReportUrl()
    {
        hidIsOldReport.Value = mbIsOldProgressReport ? Constants.S_YES : Constants.S_NO;
        string sQueryString = "../Student/StudentProgressSheet.aspx?" + CommonUtility.EncryptQuerystring("IsOldProgressReport=True");
        hlnkOldAcademicRecord.Attributes.Add("onclick", "ShowOldProgressReports('" + sQueryString + "');return false;");        
		trHeader.Visible = mbIsOldProgressReport;

        if (moUserRole != Constants.UserRoles.Student || Convert.ToBoolean(Session[Constants.S_SESSION_IS_NEW_ADMISSION]))
            hlnkOldAcademicRecord.Visible = false;
    }

    /// <summary>
    /// This method is used to fill academic year combobox.
    /// </summary>
    private void FillAcademicYearCombo(bool abShowCurrentYearData)
    {
        int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
        System.Data.DataTable oDTPassedAcademicYears = SchoolWiseAcademicYearMasterBL.GetPassedAcademicYears(miSchoolId, miStudentId, abShowCurrentYearData);
        if (oDTPassedAcademicYears.IsNonEmpty())
        {
            ControlUtility.FillDropDownList(oDTPassedAcademicYears, ref cmbAcademicYear,
                                         "Value_Member",
                                         "Display_Member",
                                         "");
            cmbAcademicYear.SelectedValue = miAcademicYearId.ToString();
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring passed to this page.
    /// </summary>
    private bool IsOldProgressReport()
    {
	    bool bIsOldProgressReport = QueryString["IsOldProgressReport"] == Constants.S_YES;
	    
	    if (!QueryString["AcademcYearId"].IsNullOrEmpty())
            miAcademicYearId = QueryString["AcademcYearId"].ToInt();
		if (!QueryString["StudentId"].IsNullOrEmpty())
			 miStudentId = QueryString["StudentId"].ToInt();
		if (!QueryString["StdDivId"].IsNullOrEmpty())
			miStandardDivisionId = QueryString["StdDivId"].ToInt();
		if (!QueryString["StandardId"].IsNullOrEmpty())
			miStandardId = QueryString["StandardId"].ToInt();
        
        return bIsOldProgressReport;
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.XseedResults,miAcademicYearId);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            tblProgressReportDetails.Visible = false;
            trErrorMessage.Visible = false;
            trFilterSelection.Visible = false;            
            trOldAcademicDetails.Visible = false;
            trHeader.Visible = false;
        }
        return bReturn;
    }
    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        valSummary.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnBack.Text = oResourceManager.GetString(hidBtnBack.Value.Replace(" ", string.Empty));
    }

    #endregion
    
}