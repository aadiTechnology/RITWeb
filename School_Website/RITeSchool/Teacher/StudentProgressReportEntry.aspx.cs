using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;

public partial class StudentProgressReportEntry : SchoolBase
{

    #region Class Members

    Int32 miStudentId = 0;
    int miStandardId = 0;
    PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = null;
    private const string S_CSSCLASS_COMBO = "LblNormal";

    #endregion Class Members

    #region Events

    override protected void OnInit(EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.FindControl("SiteMapPath1");            
            GetQueryString();
            base.OnInit(e);
            if (moUserRole == Constants.UserRoles.Student || hidIsStudentWiseProgressReport.Value == Constants.S_YES)
            {   
                IsXseedApplicable();
                if (hidIsStudentWiseProgressReport.Value != Constants.S_YES)
                    miStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID].ToString());
                btnSave.Visible = false;
                grdWithSubjects.CssClass = "";
                grdWithOutSubjects.CssClass = "";
                trOldAcadmicYr.Visible = true;
                string QueryString = "../Student/StudentProgressSheet.aspx?" + CommonUtility.EncryptQuerystring("IsOldProgressReport=True");
                hlnkOldAcademicRecord.Attributes.Add("onclick", "ShowOldProgressReports('" + QueryString + "');return false;");
            }
            DisplayProgresReport();
            FillMonthsGrid();
        }
        catch (System.Threading.ThreadAbortException)
        {
        }
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {   
            }
        }
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdWithOutSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DropDownList ocmbRemark;
            Label olblMonth;
            Label olblRemark;
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

                    if (moUserRole != Constants.UserRoles.Student && hidIsStudentWiseProgressReport.Value == Constants.S_NO)
                    {
                        ocmbRemark = new DropDownList();
                        FillRemarkComboBox(ocmbRemark);
                        ocmbRemark.CssClass = S_CSSCLASS_COMBO;
                        ocmbRemark.Width = 100;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            ocmbRemark.Attributes.Add("onchange", "SelectAll(this, " + cnt + ", '" + grdWithOutSubjects.AllowPaging + "')");
                            ocmbRemark.ID = "cmb_" + oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].PreprimaryExamConfigurationId + "_";
                        }
                        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
                        {
                            PrePrimaryStudentsExamResult remarks = GetRemarkForMonth(e.Row.RowIndex, cnt);
                            ocmbRemark.SelectedValue = remarks.PrePrimaryRemarkId.ToString();
                            ocmbRemark.ID = "cmb_" + remarks.PrePrimaryProgressReportSubSubjectId + "_" + remarks.PreprimaryExamConfigurationId + "_" + cnt.ToString();
                        }

                        if (hidFrom.Value.Equals("ExamResult") || hidFrom.Value.Equals("StudentWiseProgressReport"))
                            ocmbRemark.Enabled = !(oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].IsPublished || ((hidFrom.Value.Equals("StudentWiseProgressReport") || hidIsStudentWiseProgressReport.Value == Constants.S_YES) && oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].PreprimaryStudentWiseTestPublishStatus == Constants.S_YES));
                        else
                            ocmbRemark.Enabled = !oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].IsSubmitted;

                        cell.Controls.Add(ocmbRemark);
                    }
                    else
                    {
                        string sRemark = string.Empty;
                        olblRemark = new Label();

                        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
                        {
                            if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].IsPublished || oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].PreprimaryStudentWiseTestPublishStatus == Constants.S_YES || hidIsStudentWiseProgressReport.Value == Constants.S_YES)
                            {
                                PrePrimaryStudentsExamResult remarks = GetRemarkForMonth(e.Row.RowIndex, cnt);
                                if (remarks.PrePrimaryRemarkId != 0)
                                    sRemark = GetRemarkName(remarks.PrePrimaryRemarkId);
                                else
                                    sRemark = " N/A ";
                            }
                            else
                                sRemark = " N/A ";
                            olblRemark.Text = sRemark;
                        }
                        cell.Controls.Add(olblRemark);
                    }
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
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void grdWithSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DropDownList ocmbRemark;
            Label olblMonth;
            Label olblRemark;
            int cnt = 0;
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.VerticalAlign = VerticalAlign.Middle;
                cell.HorizontalAlign = HorizontalAlign.Center;
                if (cnt > 1)
                {
                    cell.Controls.Clear();
                    olblMonth = new Label();
                    ocmbRemark = new DropDownList();

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        olblMonth.Text = " " + cell.Text + "<br />";
                        cell.Controls.Add(olblMonth);
                    }

                    if (moUserRole != Constants.UserRoles.Student && hidIsStudentWiseProgressReport.Value == Constants.S_NO)
                    {
                        FillRemarkComboBox(ocmbRemark);
                        ocmbRemark.CssClass = S_CSSCLASS_COMBO;
                        ocmbRemark.Width = 100;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            ocmbRemark.Attributes.Add("onchange", "SelectAllWithSubjects(this, " + cnt + ", '" + grdWithSubjects.AllowPaging + "')");
                            ocmbRemark.ID = "cmb_" + oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].PreprimaryExamConfigurationId + "_";
                        }
                        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
                        {
                            PrePrimaryStudentsExamResult remarks = GetRemarkForMonthWithSubject(e.Row.RowIndex, cnt);
                            ocmbRemark.SelectedValue = remarks.PrePrimaryRemarkId.ToString();
                            ocmbRemark.ID = "cmb_" + remarks.PrePrimaryProgressReportSubSubjectId + "_" + remarks.PreprimaryExamConfigurationId + "_" + cnt.ToString();
                        }
                        cell.Controls.Add(ocmbRemark);

                        if (hidFrom.Value.Equals("ExamResult") || hidFrom.Value.Equals("StudentWiseProgressReport"))
                            ocmbRemark.Enabled = !(oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].IsPublished || ((hidFrom.Value.Equals("StudentWiseProgressReport") || hidIsStudentWiseProgressReport.Value == Constants.S_YES) && oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].PreprimaryStudentWiseTestPublishStatus == Constants.S_YES));
                        else
                            ocmbRemark.Enabled = !oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].IsSubmitted;
                    }
                    else
                    {
                        string sRemark = string.Empty;
                        olblRemark = new Label();
                        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
                        {
                            if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].IsPublished || oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].PreprimaryStudentWiseTestPublishStatus == Constants.S_YES || hidIsStudentWiseProgressReport.Value == Constants.S_YES)
                            {
                                PrePrimaryStudentsExamResult remarks = GetRemarkForMonthWithSubject(e.Row.RowIndex, cnt);
                                if (remarks.PrePrimaryRemarkId != 0)
                                    sRemark = GetRemarkName(remarks.PrePrimaryRemarkId);
                                else
                                    sRemark = " N/A ";
                            }
                            else
                                sRemark = " N/A ";
                            olblRemark.Text = sRemark;
                        }
                        cell.Controls.Add(olblRemark);
                    }

                    cell.Height = 30;
                }
                else if (cnt == 0)
                {
                    if (hidSubName.Value != cell.Text)
                    {
                        if (hidRowNo.Value != "-1")
                        {
                            grdWithSubjects.Rows[Convert.ToInt32(hidRowNo.Value)].Cells[0].RowSpan = Convert.ToInt32(hidRowSpan.Value);
                            grdWithSubjects.Rows[Convert.ToInt32(hidRowNo.Value)].Cells[0].Text = hidSubName.Value;
                        }
                        hidSubName.Value = cell.Text;
                        cell.Font.Bold = true;
                        hidRowNo.Value = e.Row.RowIndex.ToString();
                        hidRowSpan.Value = "1";
                    }
                    else
                    {
                        cell.Text = string.Empty;
                        cell.Visible = false;
                        hidRowSpan.Value = (Convert.ToInt32(hidRowSpan.Value) + 1).ToString();

                        if (oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.Count() == (e.Row.RowIndex + 1))
                        {
                            grdWithSubjects.Rows[Convert.ToInt32(hidRowNo.Value)].Cells[0].RowSpan = Convert.ToInt32(hidRowSpan.Value);
                            grdWithSubjects.Rows[Convert.ToInt32(hidRowNo.Value)].Cells[0].Text = hidSubName.Value;
                        }
                    }
                }
                else
                {
                    if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
                    {
                        cell.Font.Bold = true;
                        cell.Font.Bold = true;
                        cell.Font.Size = 10;
                    }
                }
                cnt++;
            }
        }
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sRemarksForStudent = GetXMLForStudentRemarksDetails();
            string sCommentsForStudent = GetXMLForStudentComments();
            PrePrimaryProgressSheetConfigBL.SaveProgressDetailsOfStudent(miStudentId, miAcademicYearId, miSchoolId, miUserId, sRemarksForStudent, string.Empty, sCommentsForStudent, false);
            if (!hidFrom.Value.Equals("StudentWiseProgressReport"))
                RedirecttoStudentList();
            else
            {
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage("~/RITeSchool/Teacher/StudentProgressReportEntry.aspx?" + Request.QueryString.ToString());
            }
            lblSuccessfulMsg.Text = "Marks Saved successfully!!!";
        }
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            PrePrimaryProgressSheetConfigBL.SaveProgressDetailsOfStudent(miStudentId, miAcademicYearId, miSchoolId, miUserId, string.Empty, GetStudentWiseMonthsToPublish(), string.Empty, true);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/Teacher/StudentProgressReportEntry.aspx?" + Request.QueryString.ToString());
            lblSuccessfulMsg.Text = "Marks Published successfully!!!";
        }
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex,MethodBase.GetCurrentMethod());
        }
    }


    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            string sUrl = string.Empty;
            //if (hidIsStudentWiseProgressReport.Value == Constants.S_YES)
            //    sUrl = Request.QueryString.ToString();
            //else 
            if (hidFrom.Value.Equals("StudentWiseProgressReport"))
                sUrl = CommonUtility.EncryptQuerystring(CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString())).Replace("&From=StudentWiseProgressReport", "&IsStudentWiseProfressReport=Y"));
            oMasterPage.RedirectToNextPage("~/RITeSchool/Teacher/StudentProgressReportEntry.aspx?" + sUrl);
        }
        catch (Exception Ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(Ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            RedirecttoStudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdViewRemarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                int iRowIndex = e.Row.RowIndex;
                TextBox oTextBox = (TextBox)e.Row.Cells[1].FindControl("txtremarks");
                oTextBox.Text = Convert.ToString(grdViewRemarks.DataKeys[iRowIndex]["Comment"]);
                if (moUserRole != Constants.UserRoles.Student && hidIsStudentWiseProgressReport.Value == Constants.S_NO)
                {
                    if (hidFrom.Value.Equals("ExamResult") || hidFrom.Value.Equals("StudentWiseProgressReport"))
                        oTextBox.ReadOnly = Convert.ToBoolean(grdViewRemarks.DataKeys[iRowIndex]["IsPublished"]);
                    else
                        oTextBox.ReadOnly = Convert.ToBoolean(grdViewRemarks.DataKeys[iRowIndex]["IsSubmitted"]);
                }
                else
                {
                    oTextBox.ReadOnly = true;
                    if (!Convert.ToBoolean(grdViewRemarks.DataKeys[iRowIndex]["IsPublished"]))
                        oTextBox.Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwMonths_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ((CheckBox)oCurrentItem.FindControl("chkMonth")).Checked = Convert.ToBoolean(lstvwMonths.DataKeys[oCurrentItem.DisplayIndex]["IsPublished"]);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwMonths_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwMonths.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Method


    private void FillMonthsGrid()
    {
        if (hidFrom.Value.Equals("StudentWiseProgressReport"))
        {
            lstvwMonths.Visible = true;
            lstvwMonths.DataSource = PrePrimaryProgressReportMonthsBL.GetStudentWiseMonthsList(miSchoolId, miAcademicYearId, miStandardId, miStudentId);
            lstvwMonths.DataBind();
        }
    }

    private string GetStudentWiseMonthsToPublish()
    {
        string sMonths = string.Empty;
        foreach (ListViewDataItem oCurrentItem in lstvwMonths.Items)
        {
            CheckBox ochkMonth = oCurrentItem.FindControl("chkMonth") as CheckBox;
            if (ochkMonth.Checked == true)
               sMonths += Convert.ToString(lstvwMonths.DataKeys[oCurrentItem.DisplayIndex]["PrePrimaryProgressReportMonthId"]) + ",";
        }
        return sMonths;
    }

    private void FillRemarkComboBox(DropDownList oDropDownList)
    {
        oDropDownList.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        oPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig.ForEach(remark => oDropDownList.Items.Add(new ListItem(remark.PrePrimaryProgressReportRemarkName, remark.PrePrimaryProgressReportRemarkId.ToString())));
    }

    private void GetQueryString()
    {
	    if (QueryString.Count <= 0)
		    return;
	    
		if (QueryString["StudentId"] != null)
		    miStudentId = QueryString["StudentId"].ToInt();
	    if (QueryString["StandardId"] != null)
		    miStandardId = QueryString["StandardId"].ToInt();
	    if (QueryString["From"] != null)
		    hidFrom.Value = QueryString["From"];
	    if (QueryString["IsStudentWiseProfressReport"] != null)
		    hidIsStudentWiseProgressReport.Value = QueryString["IsStudentWiseProfressReport"];
	    tblNote.Visible = btnBackDown.Visible = btnView.Visible = btnPublish.Visible = hidFrom.Value.Equals("StudentWiseProgressReport");
	    btnBack.Visible = !hidFrom.Value.Equals("StudentWiseProgressReport");

	    ApplyMouseHoverEffect(new List<Button> { btnBack, btnBackDown, btnPublish, btnView, btnSave });
	    btnPublish.Attributes.Add("Onclick", "if(!(ConfirmAction(this))){return false;}");
	    btnView.Attributes.Add("Onclick", "if(!(ConfirmAction(this))){return false;}");
	    hidBackUrl.Value = Request.QueryString.ToString();
    }

    
    private void DisplayProgresReport()
    {
        oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        oPrePrimaryProgressSheetConfigBL.GetPrePrimaryProgressSheetDetailsOfStudent(miSchoolId, miAcademicYearId, miStudentId);

        if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails.Count == 0 ||
            oPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig.Count == 0 ||
            (oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.Count == 0 && oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects.Count == 0))
        {
            SetVisibilityMode(true,false);
            if (moUserRole == Constants.UserRoles.Student)
                lblnotyetPublish.Text = "Progress report is not published yet.";
        }
        else
        {
            var IsMonthPublishCount = from IsPublishCount in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails
                                      where IsPublishCount.IsPublished == true || IsPublishCount.PreprimaryStudentWiseTestPublishStatus == Constants.S_YES
                                      select new PrePrimaryConfiguredMonthDetails { IsPublished = IsPublishCount.IsPublished };
            if ((moUserRole == Constants.UserRoles.Student) && (Convert.ToInt32(IsMonthPublishCount.Count()) == 0))
            {
                SetVisibilityMode(true, false);
                if (moUserRole == Constants.UserRoles.Student)
                    lblnotyetPublish.Text = "Progress report is not published yet.";
            }
            else
            {
                if (moUserRole == Constants.UserRoles.Student)
                {
                    StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                    oStudentFeeDetailsBL.School_Id = miSchoolId;
                    oStudentFeeDetailsBL.Academic_Year_Id = miAcademicYearId;
                    oStudentFeeDetailsBL.Student_Id = miStudentId;
                    if (!Settings.BlockProgressReportIfFeesArePending|| !oStudentFeeDetailsBL.PendingFeesAvailableForStudent())
                        FillProgressReportTables(oPrePrimaryProgressSheetConfigBL);
                    else
                    {
                        SetVisibilityMode(true, false);
                        lblnotyetPublish.Text = Constants.S_FEES_PENDING_FOR_STUDENT_MSG;
                    }
                }
                else FillProgressReportTables(oPrePrimaryProgressSheetConfigBL);
            }
        }
        hlnkOldAcademicRecord.Visible = oPrePrimaryProgressSheetConfigBL.StudentDetails.IsNewStudent != 1 && hidIsStudentWiseProgressReport.Value != Constants.S_YES; 
    }

    /// <summary>
    /// This method is used to set visibility of controls. 
    /// </summary>
    private void SetVisibilityMode(bool abTrue, bool abFalse)
    {
        trPrecondition.Visible = abTrue;
        if (trPrecondition.Visible)
        {
            tdlistview.Visible = false;
            tblNote.Visible = false;
            btnPublish.Visible = false;
            btnView.Visible = false;
        }
        lblModuleName.Visible = abFalse;
        GridViewScrollContainer.Visible = abFalse;
        lblModuleNameWithSubject.Visible = abFalse;
        GridViewSubjects.Visible = abFalse;
        btnSave.Visible = abFalse;
    }
    private void FillProgressReportTables(PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL)
    {
        DataTable oDtProgressreportWOSubject = new DataTable();
        DataTable oDtProgressreportWSubject = new DataTable();
        oDtProgressreportWOSubject.Columns.Add("Skills / Behaviour");
        oDtProgressreportWSubject.Columns.Add("Subjects");
        oDtProgressreportWSubject.Columns.Add("Skills / Behaviour");

        oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails.ForEach
            (
                s =>
                {
                    oDtProgressreportWOSubject.Columns.Add(s.MonthAbbreviation);
                    oDtProgressreportWSubject.Columns.Add(s.MonthAbbreviation);
                }
            );

        oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects.ForEach
            (
                examresult =>
                {
                    DataRow oDataRow = oDtProgressreportWOSubject.NewRow();
                    oDataRow[0] = examresult.SubSubjectName;
                    oDtProgressreportWOSubject.Rows.Add(oDataRow);
                }
            );
        oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.ForEach
            (
                examresult =>
                {
                    DataRow oDataRow = oDtProgressreportWSubject.NewRow();
                    oDataRow[0] = examresult.SubjectName;
                    oDataRow[1] = examresult.SubSubjectName;
                    oDtProgressreportWSubject.Rows.Add(oDataRow);
                }
            );

        lblModuleName.Text = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule[0].ModuleName;
        lblModuleNameWithSubject.Text = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule[1].ModuleName;

        FillAllGrids(oDtProgressreportWOSubject, oDtProgressreportWSubject);
        CreateStudentInfo();
    }

    private void FillAllGrids(DataTable oDtProgressreportWOSubject, DataTable oDtProgressreportWSubject)
    {
        if (oDtProgressreportWOSubject.Rows.Count > 0)
        {
            grdWithOutSubjects.DataSource = oDtProgressreportWOSubject;
            grdWithOutSubjects.DataBind();
        }
        else
        {
            lblModuleName.Visible = false;
            GridViewScrollContainer.Visible = false;
        }

        if (oDtProgressreportWSubject.Rows.Count > 0)
        {
            grdWithSubjects.DataSource = oDtProgressreportWSubject;
            grdWithSubjects.DataBind();
        }
        else
        {
            lblModuleNameWithSubject.Visible = false;
            GridViewSubjects.Visible = false;
        }

        if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment.Count != 0)
        {
            if (moUserRole != Constants.UserRoles.Student && hidIsStudentWiseProgressReport.Value == Constants.S_NO)
                grdViewRemarks.DataSource = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment;
            else
                grdViewRemarks.DataSource = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment.Where(i => i.IsPublished == true);


            grdViewRemarks.DataBind();
            if (grdViewRemarks.Rows.Count > 0)
            {
                tdRemarks.Visible = true;
                lblRemarks.Visible = true;
            }
        }
    }

    private PrePrimaryStudentsExamResult GetRemarkForMonth(int airowno, int aicolno)
    {
        PrePrimaryStudentsExamResult remarks;
        PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[aicolno - 1];
        PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects[airowno];
        remarks = (from remark in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamResultWithoutSubjects
                   where remark.PreprimaryExamConfigurationId == oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId
                   && remark.PrePrimaryProgressReportSubSubjectId == oPrePrimaryProgressReportSubSubjects.SubSubjectID
                   select new PrePrimaryStudentsExamResult
                   {
                       PrePrimaryRemarkId = remark.PrePrimaryRemarkId,
                       PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                       PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,

                   }).FirstOrDefault();
        if (remarks != null)
            return remarks;
        else
        {
            remarks = new PrePrimaryStudentsExamResult
            {
                PrePrimaryRemarkId = 0,
                PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
            };
            return remarks;
        }
    }

    private PrePrimaryStudentsExamResult GetRemarkForMonthWithSubject(int airowno, int aicolno)
    {
        PrePrimaryStudentsExamResult remarks;
        PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[aicolno - 2];
        PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects[airowno];
        remarks = (from remark in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamResultWithSubjects
                   where remark.PreprimaryExamConfigurationId == oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId
                   && remark.PrePrimaryProgressReportSubSubjectId == oPrePrimaryProgressReportSubSubjects.SubSubjectID
                   && remark.PrePrimarySubjectId == oPrePrimaryProgressReportSubSubjects.SubjectID
                   select new PrePrimaryStudentsExamResult
                   {
                       PrePrimaryRemarkId = remark.PrePrimaryRemarkId,
                       PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                       PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,

                   }).FirstOrDefault();
        if (remarks != null)
            return remarks;
        else
        {
            remarks = new PrePrimaryStudentsExamResult
            {
                PrePrimaryRemarkId = 0,
                PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
                PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
            };
            return remarks;
        }
    }

    private string GetXMLForStudentComments()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentProgressReportComment");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentProgressReportComment", "");

        for (int iRowIndex = 0; iRowIndex < grdViewRemarks.Rows.Count; iRowIndex++)
        {
            TextBox oTextBox = (TextBox)grdViewRemarks.Rows[iRowIndex].Cells[1].FindControl("txtremarks");

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentProgressReportComment", "");
            //TextBox oTextBox = (TextBox)oControl;

            XmlAttribute attr = oDoc.CreateAttribute("MonthId");
            attr.Value = grdViewRemarks.DataKeys[iRowIndex]["MonthId"].ToString();
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("Progress_Entry_Id");
            attr.Value = grdViewRemarks.DataKeys[iRowIndex]["Progress_Entry_Id"].ToString();
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("Comments");
            attr.Value = oTextBox.Text;
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    private string GetXMLForStudentRemarksDetails()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentProgressReportDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentProgressReportDetails", "");

        for (int iRowIndex = 0; iRowIndex < grdWithOutSubjects.Rows.Count; iRowIndex++)
        {
            int iColumnsCount = grdWithOutSubjects.Rows[iRowIndex].Cells.Count;
            for (int iColumnIndex = 1; iColumnIndex < iColumnsCount; iColumnIndex++)
                if (grdWithOutSubjects.Rows[iRowIndex].Cells[iColumnIndex].Controls.Count > 0)
                {
                    Control oControl = grdWithOutSubjects.Rows[iRowIndex].Cells[iColumnIndex].Controls[0];
                    if (oControl is DropDownList)
                    {
                        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentProgressReportDetails", "");

                        DropDownList oDropDownList = (DropDownList)oControl;

                        if (oDropDownList.SelectedValue != "0")
                        {
                            XmlAttribute attr = oDoc.CreateAttribute("RemarkId");
                            attr.Value = oDropDownList.SelectedValue;
                            oXmlNode.Attributes.Append(attr);

                            string sdropdownid = oDropDownList.ID;
                            sdropdownid = sdropdownid.Substring(0, sdropdownid.LastIndexOf('_'));
                            string sPreprimaryExamConfigurationId = sdropdownid.Substring(sdropdownid.LastIndexOf('_') + 1);
                            string sPrePrimaryProgressReportSubSubjectId = sdropdownid.Substring(sdropdownid.IndexOf('_') + 1, (sdropdownid.LastIndexOf('_') - sdropdownid.IndexOf('_')) - 1);

                            attr = oDoc.CreateAttribute("PrePrimaryProgressReportSubSubjectId");
                            attr.Value = sPrePrimaryProgressReportSubSubjectId;
                            oXmlNode.Attributes.Append(attr);

                            attr = oDoc.CreateAttribute("PreprimaryExamConfigurationId");
                            attr.Value = sPreprimaryExamConfigurationId;
                            oXmlNode.Attributes.Append(attr);

                            // Add the node to root node.
                            oXmlRootNode.AppendChild(oXmlNode);

                        }
                    }
                }
        }

        for (int iRowIndex = 0; iRowIndex < grdWithSubjects.Rows.Count; iRowIndex++)
        {
            int iColumnsCount = grdWithSubjects.Rows[iRowIndex].Cells.Count;
            for (int iColumnIndex = 1; iColumnIndex < iColumnsCount; iColumnIndex++)
                if (grdWithSubjects.Rows[iRowIndex].Cells[iColumnIndex].Controls.Count > 0)
                {
                    Control oControl = grdWithSubjects.Rows[iRowIndex].Cells[iColumnIndex].Controls[0];
                    if (oControl is DropDownList)
                    {
                        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentProgressReportDetails", "");

                        DropDownList oDropDownList = (DropDownList)oControl;

                        if (oDropDownList.SelectedValue != "0")
                        {
                            XmlAttribute attr = oDoc.CreateAttribute("RemarkId");
                            attr.Value = oDropDownList.SelectedValue;
                            oXmlNode.Attributes.Append(attr);

                            string sdropdownid = oDropDownList.ID;
                            sdropdownid = sdropdownid.Substring(0, sdropdownid.LastIndexOf('_'));
                            string sPreprimaryExamConfigurationId = sdropdownid.Substring(sdropdownid.LastIndexOf('_') + 1);
                            string sPrePrimaryProgressReportSubSubjectId = sdropdownid.Substring(sdropdownid.IndexOf('_') + 1, (sdropdownid.LastIndexOf('_') - sdropdownid.IndexOf('_')) - 1);

                            attr = oDoc.CreateAttribute("PrePrimaryProgressReportSubSubjectId");
                            attr.Value = sPrePrimaryProgressReportSubSubjectId;
                            oXmlNode.Attributes.Append(attr);

                            attr = oDoc.CreateAttribute("PreprimaryExamConfigurationId");
                            attr.Value = sPreprimaryExamConfigurationId;
                            oXmlNode.Attributes.Append(attr);

                            // Add the node to root node.
                            oXmlRootNode.AppendChild(oXmlNode);

                        }
                    }
                }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    private void RedirecttoStudentList()
    {
        string sUrl = "";
        if (moUserRole != Constants.UserRoles.Student)
        {
            if (hidFrom.Value.Equals("ExamResult"))
                sUrl = "~/Teacher/ClassTeacherTestMarksUI.aspx?";
            else if (hidFrom.Value.Equals("StudentWiseProgressReport"))
                sUrl = "~/ProgressReport/StudentwiseProgreesReportUI.aspx?";
            else
                sUrl = "~/Teacher/PrePrimaryStudentProgressList.aspx?";
        }
        else
            sUrl = "~/RITeSchool/Common/ControlPanel.aspx";
        if (hidIsStudentWiseProgressReport.Value == Constants.S_YES)
        {
            sUrl = "~/RITeSchool/Teacher/StudentProgressReportEntry.aspx?" + CommonUtility.EncryptQuerystring(CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString())).Replace("&IsStudentWiseProfressReport=Y", "&From=StudentWiseProgressReport"));
            hidBackUrl.Value = string.Empty;
        }
        MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(sUrl + hidBackUrl.Value);
    }

    private string GetRemarkName(int aiRemarkId)
    {
        string sRmrkName = (from remark in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig.AsParallel()
                            where remark.PrePrimaryProgressReportRemarkId == aiRemarkId
                            select remark.PrePrimaryProgressReportRemarkName).FirstOrDefault();

        return sRmrkName;
    }

    /// <summary>
    /// This method is used to create student's Header information.
    /// </summary>    
    private void CreateStudentInfo()
    {
        HtmlTable HeaderHtmlTable = CreateHdTable();
        CreateHdSchoolName(HeaderHtmlTable);
        CreateHdProgressCard(HeaderHtmlTable);
        CreateHdStudentName(HeaderHtmlTable);
        CreateHdStudentAttendance(HeaderHtmlTable);
        pnlHeader.Controls.Add(HeaderHtmlTable);
        HeaderHtmlTable.Dispose();
    }

    /// <summary>
    /// This methos is used to create not applicable ledgend.
    /// </summary>
    private HtmlTable CreateHdTable()
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 0;
        HeaderHtmlTable.CellSpacing = 1;
        HeaderHtmlTable.Attributes.Add("class", "ClsBorderNoBg BGReport");
        HeaderHtmlTable.Width = "100%";
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        return HeaderHtmlTable;
    }

    /// <summary>
    /// This methos is used to create not Schooll Name header.
    /// </summary>
    private void CreateHdSchoolName(HtmlTable HeaderHtmlTable)
    {
        String sSchoolName = Convert.ToString(oPrePrimaryProgressSheetConfigBL.StudentDetails.School_Name);
        String sSchoolOrgnName = Convert.ToString(oPrePrimaryProgressSheetConfigBL.StudentDetails.School_Orgn_Name);
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, sSchoolOrgnName, "SocietyName", 1, 8, HorizontalAlign.Center);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, sSchoolName, "ActualSchoolName", 1, 8, HorizontalAlign.Center);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to create cell
    /// </summary>
    /// <param name="sInnerText"></param>
    /// <param name="sClassName"></param>
    /// <param name="iRowSpan"></param>
    /// <param name="iColSpan"></param>
    private void CreateHtmlCell(HtmlTableRow oHtmlTableRow, String sInnerText, String sClassName, int iRowSpan, int iColSpan, HorizontalAlign sAlignment)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = sInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", iRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", iColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", sClassName);
        oHtmlTableCell.Attributes.Add("style", "padding-" + sAlignment + ": 10px");
        oHtmlTableCell.Align = sAlignment.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to create progress report header
    /// </summary>
    /// <param name="HeaderHtmlTable"></param>
    private void CreateHdProgressCard(HtmlTable HeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Progress Report", "ClsReportHead", 1, 8, HorizontalAlign.Center);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This methos is used to create not Student name.
    /// </summary>
    private void CreateHdStudentName(HtmlTable HeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        AddStudentInfo(oHtmlTableRow, "Roll No. ", oPrePrimaryProgressSheetConfigBL.StudentDetails.RollNo.ToString());
        AddStudentInfo(oHtmlTableRow, "Name ", oPrePrimaryProgressSheetConfigBL.StudentDetails.StudentName);
        AddStudentInfo(oHtmlTableRow, "Class ", oPrePrimaryProgressSheetConfigBL.StudentDetails.ClassName);
        AddStudentInfo(oHtmlTableRow, "Year ", oPrePrimaryProgressSheetConfigBL.StudentDetails.Academic_Year);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This methos is used to create Student attendance.
    /// </summary>
    private void CreateHdStudentAttendance(HtmlTable HeaderHtmlTable)
    {        
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Term-I Attendance", "ClsBGWhite ClsBorderlight", 0, 2, HorizontalAlign.Right);
        CreateHtmlCell(oHtmlTableRow, oPrePrimaryProgressSheetConfigBL.StudentDetails.First_Term_PresentDay + " out of " + oPrePrimaryProgressSheetConfigBL.StudentDetails.First_Term_Total , "ClsHilightTextB ClspaddingR ClsBorderlight", 0, 2, HorizontalAlign.Left);
        CreateHtmlCell(oHtmlTableRow, "Term-II Attendance", "ClsBGWhite ClsBorderlight ", 0, 2, HorizontalAlign.Right);
        CreateHtmlCell(oHtmlTableRow, oPrePrimaryProgressSheetConfigBL.StudentDetails.Final_Term_PresentDay + " out of " + oPrePrimaryProgressSheetConfigBL.StudentDetails.Final_Term_Total, "ClsHilightTextB ClspaddingR ClsBorderlight", 0, 2, HorizontalAlign.Left);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to student info pair to html row.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="asLblText"></param>
    /// <param name="asLblVal"></param>
    private void AddStudentInfo(HtmlTableRow oHtmlTableRow, String asLblText, String asLblVal)
    {
        Label oLabel = new Label();
        oLabel.Text = asLblText;
        oLabel.CssClass = "LblRht ClspaddingR";
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Controls.Add(oLabel);
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ClsBorderlight");
        oHtmlTableCell.NoWrap = true;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        if (asLblVal != "")
        {
            oLabel = new Label();
            oLabel.Text = asLblVal;
            oLabel.CssClass = "ClsHilightTextB ClspaddingR";

            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.Controls.Add(oLabel);
            oHtmlTableCell.Align = "left";
            oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ClsBorderlight");
            oHtmlTableCell.NoWrap = true;
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
        }
    }

    private void IsXseedApplicable()
    {
        int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        int iTeachersStandardDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_STDDIV_ID]);
        XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
        if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, miAcademicYearId, iStandardId, iTeachersStandardDivisionId))
        {
            string sQueryString = "IsOldProgressReport=N&AcademcYearId=" + miAcademicYearId;
            Response.Redirect("../Xseed/XseedProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
        }
    }

    #endregion
}
