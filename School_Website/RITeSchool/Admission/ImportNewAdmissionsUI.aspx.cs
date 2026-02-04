// File Name :- ImportNewAdmissionsUI.aspx.cs
// Purpose   :- This class is used to import new admitted student details.
// Date      :- 5 Dec 2009
// Author    :- Amit
//

using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using BusinessLogic;
using Utility;
using System.Collections;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class ImportNewAdmissionsUI : SchoolBase
{
    #region " Data Member and Constants"

    const string S_DEFAULT_SORT_EXP = "Form_Number";
    const string S_SCREENS_URL = "ScreensUI.aspx";
    static string msURL = String.Empty;
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";

    #endregion " Data Member and Constants"

    #region " Events "

    /// <summary>
    /// This Event is handled to Add a Sort Image to the Tables
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(Object sender, EventArgs e)
    {
        try
        {
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set master page based whether this screen is invoked from 
    /// super admin or from Admin.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);
			
			if (!IsPostBack)
                msURL = GetSourceUrl();
            if (msURL.Contains(S_SCREENS_URL))
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());

        }
    }

    /// <summary>
    /// This event is used to set client side attributes, fill list view and set default controls. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {          
            if (!IsPostBack)
            {
                FillAllPageControls();
                SetDefaultProperties();
            }
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());

        }
    }

    /// <summary>
    /// This event is used to import new admitted student details in the database.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportStudent_Click(object sender, EventArgs e)
    {
        string sServerFilePath = string.Empty;
        try
        {
            string sErrorMessage = string.Empty;
            string sFileName =CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            sServerFilePath = sFolderName + sFileName;

            fileUploadItems.SaveAs(sServerFilePath);

            bool bIsOnlineAdmission = false;

            if (rdoOnlineAdmission.Checked)
                bIsOnlineAdmission = true;

            string sSourceFileName = fileUploadItems.PostedFile.FileName;

            ImportStudentAdmissionBL oImportStudentAdmissionBL = new ImportStudentAdmissionBL(sSourceFileName, sServerFilePath);
            oImportStudentAdmissionBL.SchoolId = miSchoolId;
            oImportStudentAdmissionBL.AcademicYearId =Convert.ToInt32(hidNextAcademiYearId.Value);
            oImportStudentAdmissionBL.StandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            oImportStudentAdmissionBL.UserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
            oImportStudentAdmissionBL.SchoolStartDate = Convert.ToDateTime(hidSchoolStartDate.Value);
            oImportStudentAdmissionBL.IsOnlineAdmission = bIsOnlineAdmission;
            oImportStudentAdmissionBL.AllowDuplicateStudent = Settings.AllowDuplicateStudentsForAdmission;
            sErrorMessage = oImportStudentAdmissionBL.UploadFile();

            if (sErrorMessage == string.Empty && chkSms.Checked)
            {
                Hashtable oHashtableMobileNumber = new Hashtable();
                oHashtableMobileNumber = oImportStudentAdmissionBL.oHashtable;
                SendSMS(oHashtableMobileNumber);
            }
            ShowUploadMsg(sErrorMessage);
           
        }
        catch (BusinessLogic.Exceptions.InvalidItemDataException ex)
        {
            lblUploadErrMsg.Text = ex.Message;
            lblUploadErrMsg.CssClass = "ClsLabel";
            lblUploadErrMsg.Visible = true;
            lblUploadErrMsg.ForeColor = System.Drawing.Color.Red;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());

        }
        finally
        {
            try
            {
                if (System.IO.File.Exists(sServerFilePath))
                    System.IO.File.Delete(sServerFilePath);
            }
            catch (Exception ex)
            {
              ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
            }
        }
    }

    #endregion " Events "

    #region " Listview Events "

    /// <summary>
    /// This event is used to set list view footer and sorting image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentDetails.Items.Count > Constants.I_ZERO)
            {
                ControlUtility.FillListViewPagerFooter(lstvwStudentDetails, DtPgCount);
                AddSortImage();
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used add sorting to list view columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentDetails);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill list view by selected standard student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwStudentDetails.DataSourceID = lstvwObjDS.ID;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Listview Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to set validation header text and set hyperlink attributes on javascript.
    /// </summary>
    private void SetDefaultProperties()
    {
        ddlStandard.Focus();
        valsumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hlnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/NewStudentAdmissionDetails.xls?version=1.0','_self'); return false;");
        new Button[] {btnBack,btnImportStudent}.ApplyEffect(); 
        btnImportStudent.Attributes["onclick"] = "javascript:DisableButtons(this)";
        btnImportStudent.Attributes["onclick"] = "javascript:DisableButtons(this)";
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        rdoManualAdmission.Checked = true;
    }

    /// <summary>
    /// This method is used to fill standard combo and set Acadamic Year hidden field.
    /// </summary>
    private void FillAllPageControls()
    {
        // Table Indices
        const int S_TBL_NEW_ACADAMIC_YEAR = 0;
        const int S_TBL_STANDARDS = 1;
        const int S_TBL_ADMIN= 2;
        const int S_TBL_START_DATE=3;

        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
		S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
        DataSet oDSSchoolDetails = oStudentAdmissionsBL.GetNextAcadamicYearDetails(miSchoolId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR);

        if (oDSSchoolDetails != null)
        {
            if (oDSSchoolDetails.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows.Count > Constants.I_ZERO)
                hidNextAcademiYearId.Value = oDSSchoolDetails.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows[0]["Academic_Year_ID"].ToString();

            if (oDSSchoolDetails.Tables[S_TBL_STANDARDS].Rows.Count > Constants.I_ZERO)
                ControlUtility.FillDropDownList(oDSSchoolDetails.Tables[S_TBL_STANDARDS], ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
            else
                ddlStandard.Items.Add(new ListItem(Constants.S_SELECT,"0"));
            
            if (oDSSchoolDetails.Tables[S_TBL_ADMIN].Rows.Count > Constants.I_ZERO)
                hidAdminID.Value = oDSSchoolDetails.Tables[S_TBL_ADMIN].Rows[0]["User_Id"].ToString();

            if (oDSSchoolDetails.Tables[S_TBL_START_DATE].Rows.Count > Constants.I_ZERO)
                hidSchoolStartDate.Value = oDSSchoolDetails.Tables[S_TBL_START_DATE].Rows[0]["Start_date"].ToString();
        }
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set student admission importing message and fill student detail list view.
    /// </summary>
    /// <param name="asErrorMessage"></param>
    private void ShowUploadMsg(string asErrorMessage)
    {
        if (string.IsNullOrEmpty(asErrorMessage))
        {
            lblUploadMsg.CssClass = "ClsHilightTextB";
            lblUploadMsg.Text = "File uploaded successfully !!!";
            lblUploadMsg.Visible = true;
            DataPager pager = lstvwStudentDetails.FindControl("DtPgDropDown") as DataPager;
            if (pager != null)
                pager.SetPageProperties(0, pager.PageSize, true);
            lstvwStudentDetails.DataSourceID = lstvwObjDS.ID;
        }
        else
        {
            lblUploadErrMsg.Text = asErrorMessage;
            lblUploadErrMsg.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetSourceUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
    /// This method is used to set sorting image in list view header column.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwStudentDetails.SortDirection.ToString() == "Ascending")
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwStudentDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwStudentDetails.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    private void SendSMS(Hashtable oHashtableMobileNumber)
    {
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        string sLoginDetailsTextSms = string.Empty;
        string sSmsSubject = string.Empty;
        string sTemplateRegistrationId = string.Empty; ////
        int iSMSType = 0;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.OnlineAdmissionLoginDetailsSMS);
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTSmsTemplate.Rows.Count != Constants.I_ZERO)
        {
            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
            {
                sLoginDetailsTextSms = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
                sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);

                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)  ////
                    sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString(); ////
            }
            if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
        }
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);

        foreach (DictionaryEntry oEntry in oHashtableMobileNumber)
        {         
            string sLoginDetailsSmsText = string.Empty;   
            string sUserID = Convert.ToString(oEntry.Key);
            string sPassword = Convert.ToString(oEntry.Value);
            sLoginDetailsSmsText = sLoginDetailsTextSms.Replace("%LOGIN%", sUserID).Replace("%PASSWORD%", sPassword);
            string sDisplayText = sPassword;
            Hashtable oHashtable = new Hashtable();
            oHashtable[sPassword] = sPassword;

            SMS oSMS = new SMS();
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.SenderID = Convert.ToInt32(hidAdminID.Value);
            oSMS.InsertedByID = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSType = iSMSType;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.AcademicYearID = iAcademicYearId;
            oSMS.SchoolID = miSchoolId;
            oSMS.DisplayText = sDisplayText;
            oSMS.ToManualNumbers = oHashtable;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId; ////
            oSMS.Send();
        }
    }

    #endregion " Private Methods "
}
