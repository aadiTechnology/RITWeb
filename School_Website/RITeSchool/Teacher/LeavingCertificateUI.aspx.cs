/* File Name    :   LeavingCertificateUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 26-Sept-2009
 * Purpose :- Code Review.
 * Class Description :   This class is used to fill leaving cerficate related details of student  
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data.SqlClient;
using SchoolEntities;
using System.Linq;

public partial class LeavingCertificateUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region "Constants"

    const string S_COMMAND_SELECT = "Select";
    const string S_STANDARD_ID = "StandardId";
    const string S_DIVISION_ID = "DivisionId";
    const string S_LC_UPDATE_MESSAGE = "LC Details updated successfully!!!";
    #endregion

    #region Data Members

    string msStandardDivision;
    string msRegNo = String.Empty;

    #endregion

    #region Events

    /// <summary>
    /// This method is used to change masterpage.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            //if (IsEnrolmentNumberPassed())
            //{
            //    if (CheckUrlReferer())
                    this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
            //}

        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring,display student personal and LC details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
  {
        try
        {
            if (!IsPostBack)
            {
                SetValueToControls();
                hidSearch.Value = "Search";
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                ReadQueryString();
                SetDefaultValues();
                SetStandardDivision();
                if (QueryString.Count > Constants.I_ZERO)
                {
                     DisplayStudentLCDetails(hidStudentId.Value.ToInt());
                    MainDataTable.Visible = true;
                    if (miSchoolId == Constants.SchoolId.SVP.ToInt())
                        trStudentNOSVP.Visible = true;
                    else
                        trStudentNOSVP.Visible = false;
                }
                else
                    SetSearchView();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                txtDateofAdmission.Text = DateCultureConversion(txtDateofAdmission.Text, hidCultureInfo.Value, Session[Constants.S_SESSION_LANGUAGE].ToString());
                txtDOB.Text = DateCultureConversion(txtDOB.Text, hidCultureInfo.Value, Session[Constants.S_SESSION_LANGUAGE].ToString());
                calDateOfLeaving.Text = DateCultureConversion(calDateOfLeaving.Text, hidCultureInfo.Value, Session[Constants.S_SESSION_LANGUAGE].ToString());
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }

            SetJavascriptAttribute();

            if (tblSearchInput.Visible)
                btnCancel.Visible = false;

            if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                txtCategory.Enabled = true;

            if (SchoolBase.Settings.AllowDOBinTextEdit==true)
                trDOBText.Visible = true;
            else
                trDOBText.Visible = false;
           
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to display details of selected student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudent_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_SELECT)
            {
                msRegNo = Convert.ToString(((ImageButton)(e.CommandSource)).CommandArgument);
                int iStudentId = Convert.ToInt32(lstVwStudent.DataKeys[e.Item.DisplayIndex]["SchoolWise_Student_Id"]);
                MainDataTable.Visible = true;
                if (DisplayStudentLCDetails(iStudentId))
                {
                    tblHead.Visible = false;
                    lstVwStudent.Visible = false;
                    DataPgCnt.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudent_DataBound(object sender, EventArgs e)
    {
        try
        {
            DataPager oDataPager = (DataPager)lstVwStudent.FindControl("DataPgCnt1");
            System.Web.UI.HtmlControls.HtmlTable otblDataPager = (System.Web.UI.HtmlControls.HtmlTable)lstVwStudent.FindControl("tblDataPager");
            if (otblDataPager != null && oDataPager != null)
            {
                otblDataPager.Visible = false;
                DataPgCnt.Visible = false;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                int iTotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
                if (iTotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
                    iTotalPages += 1;

                if (iTotalPages > Constants.I_ONE)
                {
                    otblDataPager.Visible = true;
                    DataPgCnt.Visible = true;
                    //Populate the DropDownList if needed
                    DropDownList oDropDownList = (DropDownList)(oDataPager.Controls[0].FindControl("ddlCnt"));

                    if (oDropDownList.Items.Count == Constants.I_ZERO)
                    {
                        //Add a list item for each page
                        for (int iPageCount = 1; iPageCount <= iTotalPages; iPageCount++)
                            oDropDownList.Items.Add(iPageCount.ToString());

                        //Set the DDL to the appropriate page value
                        oDropDownList.Items.FindByValue(iCurrentPage.ToString()).Selected = true;
                        Label oLabel = (Label)(oDataPager.Controls[0].FindControl("CurrentPageLabel"));
                        oLabel.Font.Bold = true;
                        oLabel.Text = Resources.LocalizedResources.PageNo + " " + iCurrentPage + Resources.LocalizedResources.Of + " " + iTotalPages + " " + Resources.LocalizedResources.OutOflst;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to read registration number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;
                Label oLabel = (Label)oCurrentItem.FindControl("lblReg_No");
                msRegNo = oLabel.Text;
            }
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            DataPager oPager = (DataPager)lstVwStudent.FindControl("DataPgCnt1");
            DropDownList oDropDownList = (DropDownList)(oPager.Controls[0].FindControl("ddlCnt"));
            int iRowIndex = (Convert.ToInt32(oDropDownList.SelectedValue) - 1) * oPager.PageSize;

            oPager.SetPageProperties(iRowIndex, oPager.PageSize, true);

            int icurrentPage = (oPager.StartRowIndex / oPager.PageSize) + 1;
            int itotalPages = oPager.TotalRowCount / oPager.PageSize;

            Label oLabel = (Label)(oPager.Controls[0].FindControl("CurrentPageLabel"));
            oLabel.Text = "Page " + icurrentPage + " of " + itotalPages;

        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save all LC detailss.
    /// of student. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iReturnValue = InsertLeavingCertificateDetails(true);
            if (iReturnValue == Constants.I_ONE)
            {
                int iStudentId = Convert.ToInt32(hidStandardId.Value);
                StudentBL oStudentBL = new StudentBL();
                oStudentBL.UpdateIsLeaveFlag(iStudentId);
            }
            if(iReturnValue == Constants.I_ZERO)
                lblUpdateMessage.Text = S_LC_UPDATE_MESSAGE;
            else
                lblUpdateMessage.Text = Resources.LocalizedResources.LCDetailsSavedSuccessfully;
            lblUpdateMessage.Visible = true;
            DDLFormatType.Enabled = true;
            btnReport.Enabled = true;
            DDLFormatType2.Enabled = true;
            btnReport2.Enabled = true;
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage, "lblUpdateMessage");            
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to come back to the previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            string sURL = getReturnUrl();
            Response.Redirect(sURL, false);
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student according to name/Reg. No.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRegNo.Enabled)
            {
                lstVwStudent.Visible = true;
                lstVwStudent.DataSourceID = lstDSobj.ID;
                lstVwStudent.DataBind();
                btnSearch.Text = Resources.LocalizedResources.ChangeInput;
                hidSearch.Value = "Change Input";
                txtRegNo.Enabled = false;
                lblErr.Text = string.Empty;                
                SetListView();
            }
            else
            {
                lstVwStudent.DataSourceID = null;
                lstVwStudent.Visible = false;
                MainDataTable.Visible = false;
                btnSearch.Text = Resources.LocalizedResources.Search;
                hidSearch.Value = "Seacrh";
                btnReport.Enabled = false;
                DDLFormatType.Enabled = false;
                btnReport2.Enabled = false;
                DDLFormatType2.Enabled = false;
                tblReport.Visible = true;               
                trReportBottom.Visible = false;
                txtRegNo.Enabled = true;
                DataPgCnt.Visible = false;
                lblErr.Text = string.Empty;                
            }
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to save LC information as well to print LC report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReport_Click(object sender, EventArgs e)
    {
        try
        {
            hidRegNo.Value = txtLCRegNumber.Text;
            int iStudentId = Convert.ToInt32(hidStudentId.Value);
            int iReturnValue = InsertLeavingCertificateDetails(false);
            if (iReturnValue == Constants.I_ONE)
            {
                StudentBL oStudentBL = new StudentBL();
                oStudentBL.UpdateIsLeaveFlag(iStudentId);
            }
            DisplayReport();
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    /// <param name="asCompanyId"></param>
    /// <param name="asReportPath"></param>
    private void DisplayReport()
    {
        ReportDisplay oReportDisplay = null;
        Constants.SchoolId oSchoolId = (Constants.SchoolId)miSchoolId;
        string sIsDemosite = ConfigurationManager.AppSettings["IsDemoSite"];
        LCDetailsBL oLCDetailsBL = new LCDetailsBL();
        string sFinalPrintdate = QueryString.Count > 0 ? (calPrintdate.Text.Trim() == string.Empty ? "null" : calPrintdate.Text.Trim()) : (calPrintdateTop.Text.Trim() == string.Empty ? "null" : calPrintdateTop.Text);
        oLCDetailsBL.AddLCPrintCount(miSchoolId, txtLCRegNumber.Text.Trim(), miUserId, sFinalPrintdate); 
        switch (oSchoolId)
        {
            case Constants.SchoolId.SS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateSS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.PPS:
                {                               
                    if (sIsDemosite != null && sIsDemosite == Constants.S_YES)
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificate, GetFilterString(), GeneratReportAsPerFormat());
                    else
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificatePP, GetFilterString(), GeneratReportAsPerFormat());

                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.LFS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateLFS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.JPS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateJPS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.DSK:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateDSK, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.SNS:
                {                 
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateSNS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.JOS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateJOS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.PPSN:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCerificatePPSN, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.MVPS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateMVPS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.SSN:
                {
                    if (chkDisplayMarathi.Checked || chkSSNMarathi.Checked)
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateSSNMarathi, GetFilterString(), GeneratReportAsPerFormat());                    
                    else
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateSSN, GetFilterString(), GeneratReportAsPerFormat());

                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.SPS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateSPS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.SVP:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateSVP, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.OWS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateOWS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.CSNS:
            case Constants.SchoolId.CSNP:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateCSNP, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.PPSH:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.TransferCertificatePPSH, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.HSP:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateHSP, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.MCPS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateMCPS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.ZLSP:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateForZeal, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.BFS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateForBFS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.DYPV: //
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateForDYPV, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.NPS : 
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateNurseryTo9th_NPS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.DPIS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateDPIS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.VPMCPS:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCerificateVPMCPS, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            case Constants.SchoolId.PIONEER:
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCerificatePioneer, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
            default:
                 if (SchoolBase.Settings.IsAaryanSchool)
                  {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificateForAryan, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                   }
            
                 else
                {
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.LeavingCertificate, GetFilterString(), GeneratReportAsPerFormat());
                    oReportDisplay.DisplayReport();
                    break;
                }
             
        }

    }

    /// <summary>
    /// this method is for geting filter
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        SchoolWiseAcademicYearMasterBL oSchoolAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDTSchoolInfo = oSchoolAcademicYearBL.GetSchoolInfo(miSchoolId, miAcademicYearId);
        string sAcademicYearName = "Year " + oDTSchoolInfo.Rows[Constants.I_ZERO]["Year"].ToString();
        string sOrgName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Orgn_Name"].ToString();
        string sSchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();
        string sRecordSelectionFormula = string.Empty;

        string sFinalPrintdate = QueryString.Count > 0 ? (calPrintdate.Text.Trim() == string.Empty ? "null" : calPrintdate.Text.Trim()) : (calPrintdateTop.Text.Trim() == string.Empty ? "null" : calPrintdateTop.Text);

        if (miSchoolId == Constants.SchoolId.LFS.ToInt())
            sRecordSelectionFormula = "(usp_LeavingCertificateForLFS.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificateForLFS.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificateForLFS.Academic_Year_Id} =" + miAcademicYearId + " AND  usp_LeavingCertificate.PrintDate} = " + sFinalPrintdate + ") @";
        else if (miSchoolId == Constants.SchoolId.SSN.ToInt())
        {
            string sCheck = Constants.S_ZERO;
            if (chkDisplayMarathi.Checked || chkSSNMarathi.Checked)
                sCheck = Constants.S_ONE;

            sRecordSelectionFormula = "(usp_LeavingCertificate_SSN.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_SSN.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_SSN.PrintDate} = " + sFinalPrintdate + " AND usp_LeavingCertificate_SSN.DisplayInMarathi } =" + sCheck + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.SPS.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificate_SPS.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_SPS.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_SPS.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.OWS.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificate_OWS.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_OWS.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_OWS.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if ((miSchoolId == Constants.SchoolId.CSNP.ToInt() || miSchoolId == Constants.SchoolId.CSNS.ToInt()))
        {
            sRecordSelectionFormula = "(usp_LeavingCertificate_SSN.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_SSN.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_SSN.PrintDate} = " + sFinalPrintdate + " AND usp_LeavingCertificate_SSN.DisplayInMarathi } =" + Constants.S_ZERO + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.SVP.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificate_SVP.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_SVP.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_SVP.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            sRecordSelectionFormula = "(usp_TransferCertificate_PPSH.School_Id}=)" + miSchoolId + "AND usp_TransferCertificate_PPSH.Enrolment_Number} =" + hidRegNo.Value + "AND usp_TransferCertificate_PPSH.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.HSP.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificateHSP.School_Id}=)" + miSchoolId + "AND usp_LeavingCertificateHSP.Enrolment_Number} =" + hidRegNo.Value + "AND usp_LeavingCertificateHSP.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.MVPS.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificateMVPS.School_Id}=)" + miSchoolId + "AND usp_LeavingCertificateMVPS.Enrolment_Number} =" + hidRegNo.Value + "AND usp_LeavingCertificateMVPS.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.ZLSP.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificateDYP.School_Id}=)" + miSchoolId + "AND usp_LeavingCertificateDYP.Enrolment_Number =" + hidRegNo.Value + "AND usp_LeavingCertificateDYP;.PrintDate = " + sFinalPrintdate + ") @";
        }
        else if (SchoolBase.Settings.IsAaryanSchool)  
        {
            sRecordSelectionFormula = "(usp_LeavingCertificateForAryan.School_Id}=)" + miSchoolId + "AND usp_LeavingCertificateForAryan.Enrolment_Number =" + hidRegNo.Value + "AND usp_LeavingCertificateForAryan;.PrintDate = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.DYPV.ToInt()) //
        {
            sRecordSelectionFormula = "(usp_LeavingCertificateForDYPV.School_Id}=)" + miSchoolId + "AND usp_LeavingCertificateForDYPV.Enrolment_Number =" + hidRegNo.Value + "AND usp_LeavingCertificateForDYPV;.PrintDate = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.NPS.ToInt()) //NPS
        {
            sRecordSelectionFormula = "(usp_LeavingCertificateNurseryTo9th_NPS.School_Id}=)" + miSchoolId + "AND usp_LeavingCertificateNurseryTo9th_NPS.Enrolment_Number =" + hidRegNo.Value + "AND usp_LeavingCertificateNurseryTo9th_NPS;.PrintDate = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.VPMCPS.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificate_VPMCPS.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_VPMCPS.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_VPMCPS.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else if (miSchoolId == Constants.SchoolId.PIONEER.ToInt())
        {
            sRecordSelectionFormula = "(usp_LeavingCertificate_Pioneer.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate_Pioneer.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate_Pioneer.PrintDate} = " + sFinalPrintdate + ") @";
        }
        else
            sRecordSelectionFormula = "(usp_LeavingCertificate.School_Id}=" + miSchoolId + " AND  usp_LeavingCertificate.Enrolment_Number} =" + hidRegNo.Value + " AND  usp_LeavingCertificate.PrintDate} = " + sFinalPrintdate + ") @";

        return sRecordSelectionFormula;
    }

    /// <summary>
    /// This method is for exporting the report in selected format.
    /// </summary>
    private ExportFormatType GeneratReportAsPerFormat()
    {
        switch (hidFormat.Value.ToString())
        {
            case "Excel":
                return ExportFormatType.Excel;
            case "PDF":
                return ExportFormatType.PortableDocFormat;
            case "MS Word":
                return ExportFormatType.RichText;
            default:
                return ExportFormatType.CrystalReport;
        }
    }

    /// <summary>
    /// This method is used to set listview.
    /// </summary>
    private void SetListView()
    {
        if (lstVwStudent.Items.Count == Constants.I_ONE)
        {
            int iStudentId = Convert.ToInt32(lstVwStudent.DataKeys[0]["SchoolWise_Student_Id"]);
            MainDataTable.Visible = true;
            if (DisplayStudentLCDetails(iStudentId))
            {
                lstVwStudent.Visible = false;
                DataPgCnt.Visible = false;
            }
        }
        else if (lstVwStudent.Items.Count == Constants.I_ZERO)
        {
            lstVwStudent.Visible = false;
            DataPgCnt.Visible = false;
            lblErr.Text = Resources.LocalizedResources.StudentNotFound;
            DDLFormatType.Enabled = false;
            btnReport.Enabled = false;
            DDLFormatType2.Enabled = false;
            btnReport2.Enabled = false;           
            tblReport.Visible = true;
            trReportBottom.Visible = false;
        }
        else
        {
            lstVwStudent.Visible = true;
            MainDataTable.Visible = false;
            DataPgCnt.Visible = true;
            DDLFormatType.Enabled = false;
            btnReport.Enabled = false;
            DDLFormatType2.Enabled = false;
            btnReport2.Enabled = false;
            tblReport.Visible = true;
            trReportBottom.Visible = false;            
        }
    }

    /// <summary>
    /// This method is used to return Return URL.
    /// </summary>
    /// <returns></returns>
    private string getReturnUrl()
    {
        return Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + HidBackUrl.Value;
    }

    /// <summary>
    /// This method is used to display leaving certificate details.
    /// </summary>
    private bool DisplayStudentLCDetails(int iStudentId)
    {
        int iSchoolId = miSchoolId;
        DataTable oDT = LCDetailsBL.GetStudentLCDetails(iSchoolId, msRegNo, iStudentId);

        if (miSchoolId == Constants.SchoolId.SVP.ToInt())
            trStudentNOSVP.Visible = true;
        else
            trStudentNOSVP.Visible = false;

        if (Convert.ToInt32(oDT.Rows[0]["Student_Id"]) != Constants.I_ZERO)
        {
            txtStudentName.Text = Convert.ToString(oDT.Rows[0]["StudentName"]);
            txtMotherName.Text = Convert.ToString(oDT.Rows[0]["Mother_Name"]);
            txtFatherName.Text = Convert.ToString(oDT.Rows[0]["FatherName"]);
            //txtSerialNumber.Text = Convert.ToString(oDT.Rows[0]["SerialNumber"]);
            txtRegNumber.Text = Convert.ToString(oDT.Rows[0]["Enrolment_Number"]);
            if(oDT.Rows[0]["LCEnrolmentNumber"].ToString() == Constants.S_ZERO)
                txtLCRegNumber.Text = Convert.ToString(oDT.Rows[0]["Enrolment_Number"]);           
            else
                txtLCRegNumber.Text = Convert.ToString(oDT.Rows[0]["LCEnrolmentNumber"]);           
            txtCaste.Text = Convert.ToString(oDT.Rows[0]["CasteAndSubCaste"]);
            txtParentName.Text = Convert.ToString(oDT.Rows[0]["Parent_Name"]);
            cDateofAdmission.DateValue = Convert.ToDateTime(oDT.Rows[0]["Admission_Date"]);
            txtYearOfLeaving.Text = Convert.ToDateTime(oDT.Rows[0]["Admission_Date"]).Year.ToString();
            cDateOfBirth.DateValue = Convert.ToDateTime(oDT.Rows[0]["DOB"]);
            txtDOBWords.Text = Convert.ToString(oDT.Rows[0]["DateOfBirthInText"]);
            txtNationality.Text = Convert.ToString(oDT.Rows[0]["Nationality"]);
            msStandardDivision = Convert.ToString(oDT.Rows[0]["Class_Name_While_Leaving"]);
            hidServerDate.Value = Convert.ToString(DateTime.Today);
            lblErr.Text = string.Empty;
            hidStandardId.Value = Convert.ToString(oDT.Rows[0]["Standard_Id"]);
            hidDivisionId.Value = Convert.ToString(oDT.Rows[0]["Division_id"]);
            hidStandardId.Value = Convert.ToString(oDT.Rows[0]["Student_Id"]);
            hidStudentId.Value = Convert.ToString(oDT.Rows[0]["Student_Id"]);
            cDateOfLeaving.DateValue = Convert.ToDateTime(oDT.Rows[0]["Date_of_Leaving"]);

            hidDateOfBirthInText.Value = Convert.ToString(oDT.Rows[0]["DOB"]);
            hidLcDetailId.Value = Convert.ToString(oDT.Rows[0]["LC_Details_Id"]);
            txtBirthPlace.Text = Convert.ToString(oDT.Rows[0]["Birthplace"]);
            txtBirthDistrict.Text = Convert.ToString(oDT.Rows[0]["Birthdistrict"]);
            txtBirthTaluka.Text = Convert.ToString(oDT.Rows[0]["Birthtaluka"]);
            txtState.Text = Convert.ToString(oDT.Rows[0]["State"]);
            txtCountry.Text = Convert.ToString(oDT.Rows[0]["Country"]);
            txtAadharCardNo.Text = Convert.ToString(oDT.Rows[0]["AadharCardNo"]);
            txtUDISENO.Text = Convert.ToString(oDT.Rows[0]["UDISENumber"]);
            txtProgress.Text = Convert.ToString(oDT.Rows[0]["Progress_Details"]);
            txtLstStandardDivName.Text = Convert.ToString(oDT.Rows[0]["Last_School_Standard"]);
            txtReason.Text = Convert.ToString(oDT.Rows[0]["Reason_Of_Leaving"]);
            txtRemarks.Text = Convert.ToString(oDT.Rows[0]["Remarks"]);
            txtLstSchoolName.Text = Convert.ToString(oDT.Rows[0]["Last_school_name"]);
            txtLastSchoolDetails.Text = Convert.ToString(oDT.Rows[0]["Last_school_attended"]);
            if(Convert.ToString(oDT.Rows[0]["Religion"]) != string.Empty)
                txtReligion.Text = Convert.ToString(oDT.Rows[0]["Religion"]);
            string sYearOfLeaving = Convert.ToString(oDT.Rows[0]["Academic_Year_While_Leaving"]);
            if (sYearOfLeaving != null && sYearOfLeaving != "")
                txtYearOfLeaving.Text = sYearOfLeaving;
            txtCurrentStandard.Text = Convert.ToString(oDT.Rows[0]["Class_Name_While_Leaving"]);
            cDateOfLeaving.DateValue = Convert.ToDateTime(oDT.Rows[0]["Date_of_Leaving"]);
            txtMotherTongue.Text = Convert.ToString(oDT.Rows[0]["MotherTongue"]);
            txtConduct.Text = Convert.ToString(oDT.Rows[0]["Conduct"]);
            txtPromotion.Text = oDT.Rows[0]["Promotion"].ToString();
            txtAdmissionStandard.Text = oDT.Rows[0]["AdmissionStandard"].ToString();

            txtBookNo.Text = oDT.Rows[0]["BookNo"].ToString();
            txtSLNo.Text = oDT.Rows[0]["SLNo"].ToString();
            txtLastExamTaken.Text = oDT.Rows[0]["AnnualExaminationResult"].ToString();
            txtWhetherFailed.Text = oDT.Rows[0]["WhetherIsFailed"].ToString();
            txtExtraCurricular.Text = oDT.Rows[0]["ExtraCurricularActivity"].ToString();
            txtAcademicPerformance.Text = oDT.Rows[0]["AcademicPerformance"].ToString();
            txtStudentUIDNo.Text = oDT.Rows[0]["StudentUIDNo"].ToString();
            txtDocument.Text = oDT.Rows[0]["DocumentInSupportOfDOB"].ToString();
            txtSubjectStudied.Text = oDT.Rows[0]["SubjectStudied"].ToString();
            txtSchoolDues.Text = oDT.Rows[0]["SchoolDues"].ToString();
            txtPenNo.Text = oDT.Rows[0]["PENNo"].ToString();
            txtApaarId.Text = oDT.Rows[0]["ApaarId"].ToString();   

            if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
            {
                bool bIsFailed = Convert.ToBoolean(oDT.Rows[0]["IsFailed"]);
                rdoExamStatusNA.Visible = false;
                if (bIsFailed)
                    rdoFail.Checked = true;
                else
                    rdoPass.Checked = true;
            }
            else
            {
                rdoExamStatusNA.Visible = true;
                rdoExamStatusBlank.Visible = true;
                int iExamStatus = Convert.ToInt32(oDT.Rows[0]["ExamStatusId"]);
                if (iExamStatus == Constants.I_ONE)
                    rdoPass.Checked = true;
                else if (iExamStatus == Constants.I_TWO)
                    rdoFail.Checked = true;
                else if (iExamStatus == Constants.I_THREE)
                    rdoExamStatusNA.Checked = true;
                else if (iExamStatus == Constants.I_FOUR)
                    rdoExamStatusBlank.Checked = true;
            }
        
            int bIsAllDueSettled = Convert.ToInt32(oDT.Rows[0]["IsAllDueSettled"]);        
            if (bIsAllDueSettled == Constants.I_ONE)
                rdoYes.Checked = true;
            else if (bIsAllDueSettled == Constants.I_TWO)
                rdoNo.Checked = true;
            else if (bIsAllDueSettled == Constants.I_THREE)
                rdoNotApplicable.Checked = true;

            btnReport.Enabled = !(hidLcDetailId.Value == Constants.S_ZERO);
            txtCategory.Text = oDT.Rows[0]["Category"].ToString();
            DDLFormatType.Enabled = btnReport.Enabled;
            btnReport2.Enabled = btnReport.Enabled;
            if (oDT.Rows[0]["ApplicationDate"].ToString() != null && oDT.Rows[0]["ApplicationDate"].ToString() != string.Empty)
                txtApplicationDate.Text = oDT.Rows[0]["ApplicationDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
            else
                txtApplicationDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);

            if (oDT.Rows[0]["StudentNo"].ToString() != null && oDT.Rows[0]["StudentNo"].ToString() != string.Empty)
                txtStudentNo.Text = oDT.Rows[0]["StudentNo"].ToString();
            else
                txtStudentNo.Text = string.Empty;

            if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
            {
                if (Convert.ToBoolean(oDT.Rows[0]["IsPromotedToHigherClass"]) != null)
                {
                    bool IsPromotedToHigher = Convert.ToBoolean(oDT.Rows[0]["IsPromotedToHigherClass"]);
                    if (IsPromotedToHigher)
                        chkIsPromotedToNext.Checked = true;
                }
                else
                    chkIsPromotedToNext.Checked = false;
            }
            else
            {
                int iPromotrdToNext = Constants.I_ZERO;
                iPromotrdToNext = Convert.ToInt32(oDT.Rows[0]["PramotedToNextStatusId"]);
                if (iPromotrdToNext == Constants.I_ONE)
                    rdoPramotedYes.Checked = true;
                else if (iPromotrdToNext == Constants.I_TWO)
                    rdoPramotedNo.Checked = true;
                else if (iPromotrdToNext == Constants.I_THREE)
                    rdoPromotedNA.Checked = true;
                else if (iPromotrdToNext == Constants.I_FOUR)
                    rdoPromotedBlank.Checked = true;
            }

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                DDLFormatType2.Enabled = false;
            else
                DDLFormatType2.Enabled = btnReport.Enabled;

            tblReport.Visible = false;
            trReportBottom.Visible = true;            

            if (oDT.Rows[0]["LC_Details_id"].ToInt() == 0)
            {
                List<LeavingCertificateConfig> olstLeavingCertificateConfig = LeavingCertificateConfigBL.GetLeavingCertificateConfigList(iSchoolId);
                txtReason.Text = GetDefaultValue(olstLeavingCertificateConfig, Constants.DefaultLCValues.ReasonOfLeavingSchool);
                txtConduct.Text = GetDefaultValue(olstLeavingCertificateConfig, Constants.DefaultLCValues.Conduct);
                txtProgress.Text = GetDefaultValue(olstLeavingCertificateConfig, Constants.DefaultLCValues.Progress);
            }

            SetValueToControls();

            return true;
        }
        else
        {
            MainDataTable.Visible = false;
            lblErr.Text = Resources.LocalizedResources.StudentNotFound;
            return false;
        }
    }

    /// <summary>
    /// This method is used to set values of standard and division to hidden fields.
    /// </summary>
    private void SetStandardDivision()
    {
        // When the page is opened from the StudentListUI screen.
        if (QueryString.Count > Constants.I_ZERO)
        {
            hidStandardId.Value = QueryString[S_STANDARD_ID] ?? Constants.S_ZERO;
            hidDivisionId.Value = QueryString[S_DIVISION_ID] ?? Constants.S_ZERO;
            trlblError.Visible = false;
            tblSearchInput.Visible = false;
        }
        else
        {
            MainDataTable.Visible = false;
            trlblError.Visible = true;
            tblSearchInput.Visible = true;
        }
    }

    /// <summary>
    /// this method sets url for student list page
    /// </summary>
    /// <returns></returns>
    private string GetURL()
    {
        string sQueryString = "StandardId=" + hidStandardId.Value
                       + "&DivisionId=" + hidDivisionId.Value;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        string sURL = Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + sEncrypt;
        return sURL;
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (!IsEnrolmentNumberPassed())
        {
            string sTestDecrypt = Server.UrlDecode(Convert.ToString(Request.QueryString));
            HidBackUrl.Value = sTestDecrypt;
            if (QueryString["StudentId"] != null)
                hidStudentId.Value = QueryString["StudentId"];
            if (QueryString["RegNo"] != null)
                msRegNo = QueryString["RegNo"];
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private bool IsEnrolmentNumberPassed()
    {
        return QueryString.Count <= 0 || QueryString["RegNo"] == null;
    }

    /// <summary>
    /// It checks from which page it is going to be opened
    /// </summary>
    /// <returns></returns>
    private bool CheckUrlReferer()
    {
        if (Request.UrlReferrer != null)
        {
            string sUrl = Request.UrlReferrer.AbsolutePath;
            sUrl = sUrl.Substring(sUrl.LastIndexOf("/") + 1);
            if (sUrl.Equals("ControlPanel.aspx") || sUrl.Equals("LeavingCertificateUI.aspx"))
                return true;
        }
        return false;
    }

    /// <summary>
    /// This method is used to populate LCDetailsBL object.
    /// </summary>
    private Int32 InsertLeavingCertificateDetails(bool abIsSaveAction)
    {
        LCDetailsBL oLCDetailsBL = new LCDetailsBL();

        //oLCDetailsBL.SerialNumber = Convert.ToInt32(txtSerialNumber.Text.Trim());
        oLCDetailsBL.EnrolmentNumber = txtRegNumber.Text.Trim();
        oLCDetailsBL.LCEnrolmentNumber = txtLCRegNumber.Text.Trim();
        oLCDetailsBL.StudentName = txtStudentName.Text.Trim();
        oLCDetailsBL.MotherName = txtMotherName.Text.Trim();
        oLCDetailsBL.FatherName = txtFatherName.Text.Trim();
        oLCDetailsBL.ParentName = txtParentName.Text.Trim();
        oLCDetailsBL.CasteAndSubCaste = txtCaste.Text.Trim();
        oLCDetailsBL.Nationality = txtNationality.Text.Trim();
        oLCDetailsBL.AdmissionDate = txtDateofAdmission.Text.ToDateTime();
        oLCDetailsBL.Birthplace = txtBirthPlace.Text.Trim();
        oLCDetailsBL.Behavior_Details = string.Empty;
        oLCDetailsBL.Progress_Details = txtProgress.Text.Trim();
        oLCDetailsBL.Reason_Of_Leaving = txtReason.Text.Trim();
        oLCDetailsBL.Remarks = txtRemarks.Text.Trim();
        oLCDetailsBL.Last_school_attended = txtLastSchoolDetails.Text;
        oLCDetailsBL.Last_School_Standard = txtLstStandardDivName.Text;
        oLCDetailsBL.Academic_Year_While_Leaving = txtYearOfLeaving.Text;
        oLCDetailsBL.Class_Name_While_Leaving = txtCurrentStandard.Text;
        oLCDetailsBL.School_Id = miSchoolId;
        oLCDetailsBL.Student_Id = Convert.ToInt32(hidStandardId.Value);
        oLCDetailsBL.LC_Note = string.Empty;
        oLCDetailsBL.DOB = cDateOfBirth.DateValue;
        if (SchoolBase.Settings.AllowDOBinTextEdit==true)
        {
            oLCDetailsBL.DOBText = txtDOBWords.Text.ToString();
        }
        else
        {
            oLCDetailsBL.DOBText = CommonUtility.GetDOBInWords(txtDOB.Text.Trim().ToDateTime());
        }
       // oLCDetailsBL.DOBText = CommonUtility.GetDOBInWords(cDateOfBirth.DateValue.ToDateTime());
        oLCDetailsBL.Date_of_Leaving = cDateOfLeaving.DateValue;
        oLCDetailsBL.DateOfLeavingInWord = CommonUtility.GetDOBInWords(cDateOfLeaving.DateValue.ToDateTime());
        oLCDetailsBL.Inserted_By_Id = miUserId;
        oLCDetailsBL.Last_school_name = txtLstSchoolName.Text;
        oLCDetailsBL.MotherTongue = txtMotherTongue.Text;
        oLCDetailsBL.Conduct = txtConduct.Text;
        oLCDetailsBL.Promotion = txtPromotion.Text.Trim();
        oLCDetailsBL.ClassNameWhileAdmission = txtAdmissionStandard.Text;
        oLCDetailsBL.Birthdistrict = txtBirthDistrict.Text;
        oLCDetailsBL.Birthtaluka = txtBirthTaluka.Text;
        oLCDetailsBL.State = txtState.Text;
        oLCDetailsBL.Country = txtCountry.Text;
        oLCDetailsBL.AadharCardNo = txtAadharCardNo.Text;
        oLCDetailsBL.UDISENumber = txtUDISENO.Text;
        oLCDetailsBL.Religion = txtReligion.Text;
        oLCDetailsBL.IsPromotedToHigherClass = chkIsPromotedToNext.Checked;
        oLCDetailsBL.ApplicationDate = txtApplicationDate.Text.ToString().ToDateTime();
        oLCDetailsBL.StudentNo = txtStudentNo.Text;
        oLCDetailsBL.BookNo = txtBookNo.Text.Trim();
        oLCDetailsBL.BookSLNo = txtSLNo.Text.Trim();
        oLCDetailsBL.AnnualExaminationResult = txtLastExamTaken.Text.Trim();
        oLCDetailsBL.WhetherFailed = txtWhetherFailed.Text.Trim();
        oLCDetailsBL.ExtraCurricularActivities = txtExtraCurricular.Text.Trim();
        oLCDetailsBL.CategoryName = txtCategory.Text;
        oLCDetailsBL.AcademicPerformance = txtAcademicPerformance.Text.Trim();
        oLCDetailsBL.StudentUIDNo = txtStudentUIDNo.Text.Trim();
        oLCDetailsBL.DocumentInSupportOfDOB = txtDocument.Text.Trim();
        oLCDetailsBL.SubjectStudied = txtSubjectStudied.Text.Trim();
        oLCDetailsBL.SchoolDues = txtSchoolDues.Text.Trim();
        oLCDetailsBL.PENNo = txtPenNo.Text.Trim();
        oLCDetailsBL.ApaarId = txtApaarId.Text.Trim();


        bool bIsFailed = false;
        if (rdoFail.Checked == true)
            bIsFailed = true;
        oLCDetailsBL.IsFailed = bIsFailed;
        int bIsAllDueSettled = Constants.I_ZERO;
       
		if (rdoYes.Checked)
			bIsAllDueSettled = Constants.I_ONE;
		else if (rdoNo.Checked)
			bIsAllDueSettled = Constants.I_TWO;
		else if (rdoNotApplicable.Checked)
			bIsAllDueSettled = Constants.I_THREE;
		oLCDetailsBL.IsAllDueSettled = bIsAllDueSettled;

        int iExamStatusId = Constants.I_ZERO;
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            if (rdoPass.Checked)
                iExamStatusId = Constants.I_ONE;
            else if (rdoFail.Checked)
                iExamStatusId = Constants.I_TWO;
            else if (rdoExamStatusNA.Checked)
                iExamStatusId = Constants.I_THREE;
            else if (rdoExamStatusBlank.Checked)
                iExamStatusId = Constants.I_FOUR;
        }
        oLCDetailsBL.ExamStatusId = iExamStatusId;

        int iPramotedToNextId = Constants.I_ZERO;
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            if (rdoPramotedYes.Checked)
                iPramotedToNextId = Constants.I_ONE;
            else if (rdoPramotedNo.Checked)
                iPramotedToNextId = Constants.I_TWO;
            else if (rdoPromotedNA.Checked)
                iPramotedToNextId = Constants.I_THREE;
            else if (rdoPromotedBlank.Checked)
                iPramotedToNextId = Constants.I_FOUR;
        }
        oLCDetailsBL.PramotedToNextClassId = iPramotedToNextId;
        int iReturnValue = 0;
        if (hidLcDetailId.Value == Constants.S_ZERO)
        {
            iReturnValue = oLCDetailsBL.InsertLCDetails(abIsSaveAction);
            hidLcDetailId.Value = iReturnValue.ToString();
        }
        else
        {
            oLCDetailsBL.LC_Details_Id = Convert.ToInt32(hidLcDetailId.Value);
            oLCDetailsBL.UpdateLCDetails(abIsSaveAction);
        }

        return iReturnValue;
    }

    /// <summary>
    /// This method is used to set javascript attribute.
    /// </summary>
    private void SetJavascriptAttribute()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnReport, btnSave, btnSearch });

        if (moUserRole == Constants.UserRoles.Teacher)
        {
            btnCancel.Attributes["onclick"] = "refreshParent()";
            hidIsTeacher.Value = true.ToString();
        }
        else
            btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this, 'Search')";
        btnReport.Attributes.Add("onclick", "ClearUpdateMessage(); if(!ValidateControls()) return false;");
		btnSave.Attributes.Add("onclick","CrearMessage();");

        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            trPramotedToNext.Visible = false;
            trPramotedToNextPPSH.Visible = true;
            rdoExamStatusNA.Visible = true;
        }
        else
        {
            trPramotedToNext.Visible = true;
            trPramotedToNextPPSH.Visible = false;
            rdoExamStatusNA.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set search view.
    /// </summary>
    private void SetSearchView()
    {
        txtRegNo.Focus();
        cstReg.Visible = true;
        tblHead.Visible = false;
        btnReport.Enabled = false;
        DDLFormatType.Enabled = false;
        btnReport.Enabled = false;
        DDLFormatType2.Enabled = false;
        btnReport2.Enabled = false;

        tblReport.Visible = true;
        trReportBottom.Visible = false;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        calPrintdateTop.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        calPrintdate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        rdoNotApplicable.Checked = true;
        txtBirthPlace.Focus();
        cstReg.Visible = false;
        SetDefaultButton(btnSearch);
    }

    /// <summary>
    /// This method is used to log an exception to the error log table in the database.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="currentMethod"></param>
    private void AddExceptionToErrorLog(Exception ex, MethodBase currentMethod)
    {
        int iUserid = miUserId;
        ExceptionHandler.WriteExceptionToErrorLog(String.Format("{0}. Trace: {1}", ex.Message, ex.StackTrace),
                                                  String.Format("{0}.{1}", currentMethod.DeclaringType.FullName, currentMethod.Name),
                                                  iUserid);
    }
    /// <summary>
    /// This method is used to set design according to selected language
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        btnSearch.Text = oResourceManager.GetString(hidSearch.Value.Replace(" ", string.Empty));
        hidLastAttendedSchoolStandardShouldBeBetween.Value = Resources.LocalizedResources.LastAttendedSchoolStandardShouldBeBetween;
        hidDateOfLeavingShouldBeSelected.Value = Resources.LocalizedResources.DateOfLeavingShouldBeSelected;
        hidDateOfLeavingShouldNotBeFutureDate.Value = Resources.LocalizedResources.DateOfLeavingShouldNotBeFutureDate;
        hidDateOfLeavingShouldBeGreaterThanDateOfAdmission.Value = Resources.LocalizedResources.DateOfLeavingShouldBeGreaterThanDateOfAdmission;
        hidDateOfAdmissionShouldBeSelected.Value = Resources.LocalizedResources.DateOfAdmissionShouldBeSelected;
        hidDateOfAdmissionShouldNotBeFutureDate.Value = Resources.LocalizedResources.DateOfAdmissionShouldNotBeFutureDate;
        hidBirthDateShouldBeSelected.Value = Resources.LocalizedResources.BirthDateShouldBeSelected;
        hidBirthDateShouldNotBeFutureDate.Value = Resources.LocalizedResources.BirthDateShouldNotBeFutureDate;
        hidProgressRemarkOfStudentShouldBeOfLengthLessThan.Value = Resources.LocalizedResources.ProgressRemarkOfStudentShouldBeOfLengthLessThan;
        hidLastSchoolAttendedAddressDetailsOfStudentShould.Value = Resources.LocalizedResources.LastSchoolAttendedAddressDetailsOfStudentShould;
        hidReasonForLeavingSchoolShouldBeOfLength.Value = Resources.LocalizedResources.ReasonForLeavingSchoolShouldBeOfLength;
        hidRemarksShouldBeOfLengthLessThan.Value = Resources.LocalizedResources.RemarksShouldBeOfLengthLessThan;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        ValidationSummary1.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to return default value.
    /// </summary>
    /// <param name="olstLeavingCertificateConfig"></param>
    /// <param name="aoValue"></param>
    /// <returns></returns>
    private string GetDefaultValue(List<LeavingCertificateConfig> aolstLeavingCertificateConfig, Constants.DefaultLCValues aoValue)
    {
        LeavingCertificateConfig oLeavingCertificateConfig = aolstLeavingCertificateConfig.Where(lc => lc.OriginalId == aoValue.ToInt()).FirstOrDefault();
        return oLeavingCertificateConfig.DefaultValue;
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetValueToControls()
    {
        if (miSchoolId == Constants.SchoolId.SSN.ToInt())
        {
            tdSSNDisplayInmarathi.Visible = true;
            tdDisplayInmarathi.Visible = true;
            tdSSNMarathi.Visible = true;
            chkSSNMarathi.Visible = true;

            lblConduct.Text = "Academic Performance";
            lblPromotion.Text = "Behaviour";

            if (hidLcDetailId.Value == Constants.S_ZERO)
            {
                txtConduct.Text = "Good";
                txtPromotion.Text = "Good";
            }
        }
        else if (miSchoolId == Constants.SchoolId.SPS.ToInt() && hidLcDetailId.Value == Constants.S_ZERO)
        {
            txtConduct.Text = "GOOD";
            txtProgress.Text = "GOOD";
            txtReason.Text = "PARENT’S WISH";

            lblConduct.Text = "Conduct";
            lblPromotion.Text = "Promotion";
        }
        else
        {
            tdSSNDisplayInmarathi.Visible = false;
            tdDisplayInmarathi.Visible = false;
            tdSSNMarathi.Visible = false;
            chkSSNMarathi.Visible = false;

            lblConduct.Text = "Conduct";
            lblPromotion.Text = "Promotion";
        }
    }

    #endregion
}
