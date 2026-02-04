/* File Name :- 
 * Modified By :- Sachin
 * Modified Date :- 19-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display available students of selected class and import students.
*/
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using System.Configuration;
using System.Text;
using SchoolAutoSearchService.Service;
public partial class ImportStudentUI : SchoolBase
{
    #region Constants

    const int I_DOB_COLUMN_INDEX = 3;
    const int I_REGNOPOSTFIX_COLUMN_INDEX = 7;
	private const int I_SMS_TEMPLATE_TXT = 2;
	private const int I_SMS_SUBJECT_TXT = 1;
	private const int I_SMS_TYPE = 3;
	const string S_REPLACE_URL = "http://";

    #endregion

    #region  Event

    /// <summary>
    /// This event is used to fill standard combobox, set sort arrow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                RefreshValue();
                cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, "0"));
                SetDefaultSortArrow();
                FillStandardCombobox();
                SetJavascriptAttributes();
                InitializeFields();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                   
                }
            
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                FillStudentGrid();
                InitializeFields();
            }
            if ((Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] != null) && (Convert.ToChar(Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED]) == Constants.C_YES) && (Convert.ToChar(Session[Constants.S_SESSION_IS_FINALYEAR_GENERATED]) == Constants.C_NO))
                chkSendSMS.Enabled = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set record count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdvwAllStudents.PageSize * grdvwAllStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwAllStudents.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0" || grdvwAllStudents.PageCount == 0)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change gridview page index on change of page dropdown combobox index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdvwAllStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdvwAllStudents.PageIndex = pageList.SelectedIndex;
            FillStudentGrid();
            lblHead.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill page index combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwAllStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    for (int iPageIndex = 0; iPageIndex < grdvwAllStudents.PageCount; iPageIndex++)
                    {
                        int pageNumber = iPageIndex + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (iPageIndex == grdvwAllStudents.PageIndex)
                            item.Selected = true;
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdvwAllStudents.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text =  Resources.LocalizedResources.PageNo + " " + currentPage.ToString() + " " +
                      Resources.LocalizedResources.Of+ " " + grdvwAllStudents.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to import student,
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportStudent_Click(object sender, EventArgs e)
    {
        string sServerFilePath = string.Empty;
        string sFileName;
        try
        {
            if (CheckPreCondition())
            {
                sFileName =CommonUtility.GetFileNameForRenaming(fileUploadStudents.FileName);
                //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
                string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
                sServerFilePath = sFolderName + sFileName;
                fileUploadStudents.SaveAs(sServerFilePath);

                string sErrorMessage = string.Empty;
                sErrorMessage = UploadFile(sServerFilePath);

                if (sErrorMessage.Equals(""))
                {
                    lblHead.CssClass = "ClsHilightTextB";
                    lblHead.Text = Resources.LocalizedResources.MsgFileUpload;
                    lblHead.Visible = true;
                    FillStudentGrid();
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Student));
                   RefreshStudentCache();
                }
                else
                    DisplayError(sErrorMessage);
            }
          
        }
        catch (BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        
        catch (BusinessLogic.Exceptions.DuplicateStudentExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.DuplicateStudentUniqueNoExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.DuplicateGeneralRegisterNumberExceptions ex)
        {
            catchException(ex);
        }

        catch (BusinessLogic.Exceptions.InvalidRegisterNoPrefixExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentRollNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentFirstNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.DuplicateRollNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMiddleNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentLastNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMotherNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentDateofBirthExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentAdmissionDateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentJoiningDateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentSexExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentBloodGroupExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentParentNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentParentOccupationExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentAddressExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentCityExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentStateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentPincodeExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMobileExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidMobileNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentCategoryExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidateStudentSubAreaName ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentCasteSubcasteExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidPincodeExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullPhotoFileExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NoRecordFoundExceptions ex)
        {
            catchException(ex);
        }
        catch(BusinessLogic.Exceptions.ValidEmailAddressExceptions ex)
        {
            catchException(ex);
        }
       

        catch (Exception ex)
        {
            //lblHead.Text = Resources.LocalizedResources.FileUploadData;
            lblHead.Text = ex.Message + " - " + ex.StackTrace;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        try
        {
            if (System.IO.File.Exists(sServerFilePath))
                System.IO.File.Delete(sServerFilePath);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        
    }

    /// <summary>
    /// This event is used to fill division combobox and student grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            FillDivisionCombobox(iStandardId);
            SetDefaultSortArrow();
            if (cmbStandard.SelectedValue != "0")
            {
                divlbl.Visible = true;
                grdvwAllStudents.Visible = true;
                grdvwAllStudents.PageIndex = 0;
                FillStudentGrid();
            }
            else
            {
                divlbl.Visible = false;
                grdvwAllStudents.Visible = false;
            }
            lblHead.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill grid according to class.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetDefaultSortArrow();
            if (cmbDivision.SelectedValue != "0")
            {
                divlbl.Visible = true;
                grdvwAllStudents.Visible = true;
                grdvwAllStudents.PageIndex = 0;
                FillStudentGrid();
            }
            else
            {
                divlbl.Visible = false;
                grdvwAllStudents.Visible = false;
            }
            lblHead.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display student according to selected page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwAllStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwAllStudents.PageIndex = e.NewPageIndex;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwAllStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add a sort direction image to the appropriate column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, grdvwAllStudents.SortDirection);
                }
                else
                    CommonUtility.AddSortImage(1, e.Row, grdvwAllStudents.SortDirection);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method

    /// <summary>
    /// This method is used to check precondition for a standard 
    /// ie fee configuration for particular standard is set or not. 
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        string sLinks = ReferenceBL.GetStudentUIPreConditionMsg(iStandardId);

        if (sLinks.Equals(""))
            bReturn = true;
        else
            DisplayError(sLinks);

        return bReturn;
    }

     /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../DOWNLOADS/StudentDetails.xls?Version=1.1','_self'); return false;");
        btnImportStudent.Attributes["onclick"] = "javascript:DisableButtons(this)";
        imgbtnBack.Attributes["onclick"] = "javascript:DisableButtons(this)";        
        ApplyMouseHoverEffect(new List<Button> { btnImportStudent, imgbtnBack });
    }

    /// <summary>
    /// This method is used to upload file.
    /// </summary>
    /// <param name="sServerFilePath"></param>
    /// <returns></returns>
    private string UploadFile(string sServerFilePath)
    {
        string sErrorMesage = string.Empty;
        //postfix change
        string sMiddleStringInMessage = ", ";       
        StringBuilder oStringBuilder = new StringBuilder();
        string sSourceFileName = fileUploadStudents.PostedFile.FileName;
        Constants.UploadFileType oUploadFileType = Constants.UploadFileType.Student;        
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        DataSet oDsMaster = MasterDataCollectionBL.GetAllMasterDataForStudent(miSchoolId, miAcademicYearId, iStandardId, Convert.ToInt32(cmbDivision.SelectedValue));

        FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, sServerFilePath, oUploadFileType);
        oFileUploadUtility.UserId = miUserId;
        oFileUploadUtility.SchoolId = miSchoolId;
        oFileUploadUtility.StandardId = iStandardId;
        oFileUploadUtility.DivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
        oFileUploadUtility.AcademicYearId = miAcademicYearId;
        oFileUploadUtility.RegistrationPrefix = Convert.ToString(oDsMaster.Tables[2].Rows[0][0]);
        //for Postfix         
        if (oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows.Count > 0)
        {
            for (int i = 0; i < oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows.Count; i++)
                oStringBuilder.Append(sMiddleStringInMessage + Convert.ToString(oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows[i][0]));
            if(oStringBuilder.ToString().StartsWith(sMiddleStringInMessage))
                 oFileUploadUtility.RegistrationPostfix = oStringBuilder.ToString().Substring(2);
            else
                 oFileUploadUtility.RegistrationPostfix = oStringBuilder.ToString();
        }

       
        //find length of postfix
        //oFileUploadUtility.PostFixLength = Convert.ToInt16(oStringBuilder.Length / (oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows.Count + sMiddleStringInMessage.Length)); 
        oFileUploadUtility.PostFixLength = oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows.Count > 0 ? oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows[0][0].ToString().Length : 0;

        //Standardwise academic year change.
        DataTable oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(miSchoolId, miAcademicYearId, iStandardId);
        oFileUploadUtility.AcademicYearStartDate = Convert.ToDateTime(oDT.Rows[0]["StartDate"].ToString());
        oFileUploadUtility.AcademicYearEndDate = Convert.ToDateTime(oDT.Rows[0]["EndDate"].ToString());    
        // End.		
		oFileUploadUtility.bIsConcessionApplicable = Settings.IsConcessionApplicable;			
		oFileUploadUtility.bIsRTEApplicable = Settings.IsRTEApplicable;  
		
		sErrorMesage = oFileUploadUtility.UploadFile();

		if (oFileUploadUtility.bIsRTEApplicable && oFileUploadUtility.RTEStudentIDs != null)
		{			
			List<int> lstIRTEStudIds = oFileUploadUtility.RTEStudentIDs;
			StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
			Accounts oAccounts = new Accounts();
			string sReceiptNumber = string.Empty;
			foreach (var iStudentId in lstIRTEStudIds)
			{
				 sReceiptNumber = oStudentFeeDetailsBL.AddConcessionForRTEStudent(iStudentId,miSchoolId,miAcademicYearId);

				// Create a fee voucher for the fee concession for RTE student.
				if (Settings.EnableAccountsModule)
				{
					oAccounts.RecordCashPaymentForFeeConcession(iStudentId, sReceiptNumber);
				}
			}			
		}

		if (sErrorMesage == string.Empty && chkSendSMS.Checked)    
               SendLoginDetailSMS(oFileUploadUtility.ImportedStudentsRegNumbers);
        
        return sErrorMesage;
    }

    /// <summary>
    /// This method is used to fill student gridview.
    /// </summary>
    private void FillStudentGrid()
    {
        SetDateFormat();
        grdvwAllStudents.DataSourceID = GrdDSobj.ID;

    }

    /// <summary>
    /// This method is used to send login details sms to parent.
    /// </summary>
    private void SendLoginDetailSMS(string osImportedStudents)
    {
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
        if (chkSendSMS.Checked)
        {
            int iSMSType = 0;
            DataTable odtImportedStudents = StudentBL.GetAllStudents(miSchoolId, miAcademicYearId, osImportedStudents);
            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
            int iSmsID = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsID, miSchoolId);
            if (oDTSmsTemplate.Rows.Count != 0)
            {
                if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                {
                    sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);

                    if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
                }
                if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                    iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
            }
            foreach (DataRow drStudent in odtImportedStudents.Rows)
            {
                string sUserLogin = drStudent["User_Login"].ToString();
                string sUserPass = CommonUtility.GetDecryptedPassword(sUserLogin, drStudent["User_Password"].ToString());
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", sUserLogin).Replace("%PASSWORD%", sUserPass);
                SMS oSMS = new SMS();
                oSMS.Sender = oSchoolBL.SMSSenderName;
                oSMS.SMSText = sLoginDetailsSmsText;
                oSMS.SMSType = iSMSType;
                oSMS.TemplateRegistrationId = sTemplateRegistrationId;
                oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                oSMS.DisplayText = drStudent["Name"].ToString();
                oSMS.To.Add(drStudent["ID"].ToString(), drStudent["Mobile_Number"].ToString());
				if (!string.IsNullOrEmpty(drStudent["Mobile_Number2"].ToString()))
					oSMS.To.Add(drStudent["ID"].ToString() + "sm;", drStudent["Mobile_Number2"].ToString());
                oSMS.Send();
				if(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MobileUrl"]))
				SendMobileDetailsSMS(oSchoolBL, drStudent);
            }
        }
    }

	/// <summary>
	/// This method is used to send sms about mobile site details.
	/// </summary>
	/// <param name="oSchoolBL"></param>
	/// <param name="drStudent"></param>
	private void SendMobileDetailsSMS(SchoolBL aoSchoolBL, DataRow adrStudent)
	{
		string sMobileSmsTemplate = string.Empty;
        string sTemplateRegistrationId = string.Empty;
		string sSmsSubject = string.Empty;
		int iTemplateId = Constants.SMSTemplate.MobileWebsiteDetailsSMS.ToInt();
		int iSMSType = 0;
		DataTable oDTMobileSMSTemplate = SmsTemplateBL.GetTemplate(iTemplateId, miSchoolId);

		if (oDTMobileSMSTemplate.IsNonEmpty())
		{
			if (oDTMobileSMSTemplate.Rows[Constants.I_ZERO][I_SMS_TEMPLATE_TXT] != DBNull.Value)
			{
				sMobileSmsTemplate = Convert.ToString(oDTMobileSMSTemplate.Rows[0][I_SMS_TEMPLATE_TXT]);

                if (oDTMobileSMSTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    sTemplateRegistrationId = oDTMobileSMSTemplate.Rows[0]["TemplateRegistrationId"].ToString();

				sSmsSubject = Convert.ToString(oDTMobileSMSTemplate.Rows[0][I_SMS_SUBJECT_TXT]);
			}
			if (oDTMobileSMSTemplate.Rows[Constants.I_ZERO][I_SMS_TYPE] != DBNull.Value)
				iSMSType = oDTMobileSMSTemplate.Rows[Constants.I_ZERO][I_SMS_TYPE].ToInt();

			SMS oSMS = new SMS();
			oSMS.Sender = aoSchoolBL.SMSSenderName;
			oSMS.SMSText = sMobileSmsTemplate.Replace("%WEBSITE%", ConfigurationManager.AppSettings["MobileUrl"].Replace(S_REPLACE_URL, string.Empty));
			oSMS.SMSType = iSMSType;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
			oSMS.School_Name = aoSchoolBL.SchoolName + "::" + sSmsSubject;
			oSMS.DisplayText = adrStudent["Name"].ToString();
			oSMS.To.Add(adrStudent["ID"].ToString(), adrStudent["Mobile_Number"].ToString());
			if (!string.IsNullOrEmpty(adrStudent["Mobile_Number2"].ToString()))
				oSMS.To.Add(adrStudent["ID"].ToString() + "sm;", adrStudent["Mobile_Number2"].ToString());
			oSMS.Send();
		}
	}
    /// <summary>
    /// This method is used to set error message.
    /// </summary>
    /// <param name="ex"></param>
    private void catchException(Exception ex)
    {
        lblHead.Text = ex.Message;
        lblHead.CssClass = "ClsLabel";
        lblHead.Visible = true;
        lblHead.ForeColor = System.Drawing.Color.Red;
    }

    /// <summary>
    /// This function sets the label for total records
    /// </summary>
    /// <param name="aiCount"></param>
    private void SetTotalRecordLabel(int aiCount)
    {
        if (aiCount > 0)
            divlbl.Visible = true;
        else
            divlbl.Visible = false;
    }

    /// <summary>
    /// This method is used to set date format for grid column.
    /// </summary>
    private void SetDateFormat()
    {
        BoundField oReceivedDate = (BoundField)grdvwAllStudents.Columns[I_DOB_COLUMN_INDEX];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method fills combobox with standards.
    /// /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId,miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
        cmbDivision.Items.Add(Constants.S_SELECT);
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox(int aiStandardId)
    {
        if (aiStandardId == 0)
        {
            cmbDivision.Items.Clear();
            cmbDivision.Items.Add(Constants.S_SELECT);
        }
        else
        {
            DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId,miAcademicYearId);
            DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
            ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                           Constants.S_DIVISION_ID_FIELD,
                                           Constants.S_DIVISION_NAME_FIELD,
                                           string.Empty);
        }

    }

    /// <summary>
    /// This method is used to display error message.
    /// </summary>
    /// <param name="asError"></param>
    private void DisplayError(string asError)
    {
        lblHead.Text = asError;
        lblHead.Visible = true;
    }

    /// <summary>
    /// This function is used to set sort variables
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set default sort arrow in grid.
    /// </summary>
    private void SetDefaultSortArrow()
    {
        const int I_ROLL_NO_COLUMN_INDEX = 1;
        hidSortExpression.Value = grdvwAllStudents.Columns[I_ROLL_NO_COLUMN_INDEX].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeFields()
    {
        valErrorMsg.HeaderText  = Resources.LocalizedResources.PleaseFixFollowingError;
        trTotalRec.Visible = false;
        cmbStandard.Focus();
    }

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache()
    {
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, new List<int>(), Constants.Action.Insert);
    }

    private void RefreshValue()
    {
        hidValFileUpload.Value = Resources.LocalizedResources.ValFileUpload;
        hidValFileUploadType.Value = Resources.LocalizedResources.ValFileUploadType;
    }
    
    #endregion
}
