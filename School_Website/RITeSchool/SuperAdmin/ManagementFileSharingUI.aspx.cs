using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using SuperAdminEntities;
using Utility;

public partial class ManagementFileSharingUI : SchoolBase
{
    #region -- MEMBER(s) --

	private const int I_SUPERADMIN_ROLE_ID = 1;
	private const string S_DEFAULT_SORT_EXP = "UpdateDate";

	private bool mbIsSuperAdmin;
	private int miSuperAdminUserId;
	private bool mbIsCurrentAcademicYr = true;

	#endregion -- MEMBER(s) --
    
	#region -- EVENT(s) --

	/// <summary>
	/// This Event is used to set the MasterPage based on the logged in user.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected override void OnPreInit(EventArgs e)
	{
		try
		{
            base.OnPreInit(e);
			
			// When logged in user is NOT a SuperAdmin
            if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] == null)
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";			
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This method is used to set view according to login user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
        { 
			SetMemberVariables();
			if(!IsPostBack)
			{
                SetPostbackUrl();
				DisplayControlsAsPerLogin();
				FillAcademicYearCombo();
			}

			SetJavaScriptAttributes();
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add the sort image for the FileList table.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_PreRenderComplete(object sender, EventArgs e)
	{
		try
		{
			// Add Sort Image
			AddSortImage();
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to upload a file to the database & save it on the server.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnUpload_Click(object sender, EventArgs e)
	{
		try
		{
			if(FileUploadControl.HasFile)
			{
				// Upload the file to the server
				string sFileName = UploadFileToServer();

				// Record the upload in the database
				ManagementFileUploadDetails oFileUpload = PopulateFileUploadDetails(-1, sFileName);

				ManagementFileSharingBL oMgmtFileSharingBL = new ManagementFileSharingBL();
				if(oMgmtFileSharingBL.InsertFile(oFileUpload))
				{
					lblUpdateMessage.Text = "File Uploaded Successfully.";
					// Update the FileDetails ListView
					// We need to clear the ListView Items since the Item.Count property remains the same(old value) if it's DataSource is null.
					lstvwFileList.Items.Clear();
					lstvwFileList.DataSourceID = FileListObjDataSource.ID;
					// Send SMS to Recipients
					if(chkSendSMS.Checked) SendSMS();
					// Reset Upload form controls.
					ResetControls();
				}
				else
					lblErrorMsg.Text = "There was an error uploading the file.";
			}
			else
			{
				lblErrorMsg.Text = "File not found!";
			}
		}		
		catch(Exception ex)
		{
			lblErrorMsg.Text = "There was an error uploading the file.";
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update a file in the database with the new changes.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			string sFileName = String.Empty;
			if(FileUploadControl.HasFile)
			{
				// Delete the file if it exists.
				//string sFilePath = Server.MapPath("..") + hidOldFilePath.Value;
                string sFilePath = base.BasePath+"/RITeSchool" + hidOldFilePath.Value;

				if(File.Exists(sFilePath))
					File.Delete(sFilePath);
				sFileName = UploadFileToServer();
			}
			else
				sFileName = hidOldFilePath.Value.Substring(hidOldFilePath.Value.LastIndexOf('/') + 1);

			ManagementFileUploadDetails oFileUpload = PopulateFileUploadDetails(Convert.ToInt32(hidOldUploadId.Value), sFileName);

			ManagementFileSharingBL oMgmtFileSharingBL = new ManagementFileSharingBL();
			if(oMgmtFileSharingBL.InsertFile(oFileUpload))
			{
				lblUpdateMessage.Text = "File Updated Successfully.";

				// Update the FileDetails ListView
				// We need to clear the ListView Items since the Item.Count property remains the same(old value) if it's DataSource is null.
				lstvwFileList.Items.Clear();
				lstvwFileList.DataSourceID = FileListObjDataSource.ID;

				// Reset Upload form controls.
				ResetControls();
			}
			else
			{
				lblErrorMsg.Text = "There was an error updating the file.";
			}
		}
		catch(Exception ex)
		{
			lblErrorMsg.Text = "There was an error updating the file.";
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to reset the controls on the Upload/Update form
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ResetControls();
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to hide certain controls if the logged in user is a SuperAdmin & also populate the DataPager control of the ListView
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFileList_DataBound(object sender, EventArgs e)
	{
		try
		{
			if(lstvwFileList.Items.Count > 0)
			{
				// Hide the Edit & Delete columns if logged in user is a SuperAdmin
				if(mbIsSuperAdmin)
				{
					lstvwFileList.FindControl("EditColHeader").Visible = false;
					lstvwFileList.FindControl("DeleteColHeader").Visible = false;
				}
				else
				{
					lstvwFileList.FindControl("EditColHeader").Visible = true;
					lstvwFileList.FindControl("DeleteColHeader").Visible = true;
				}

				// Initialize the DataPager control
				DataPager DtPgCount = lstvwFileList.FindControl("DtPgCount") as DataPager;
				if(DtPgCount != null)
					ControlUtility.FillListViewPagerFooter(lstvwFileList, DtPgCount);
			}
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to show/hide and set properties of certain controls displayed in the ListView based on the logged in user.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFileList_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if(e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				ImageButton oImgBtn = e.Item.FindControl("imgBtnDownload") as ImageButton;

				if(mbIsSuperAdmin)
				{
					e.Item.FindControl("EditButtonCell").Visible = false;
					e.Item.FindControl("DeleteButtonCell").Visible = false;
				}
				if(oImgBtn != null)
				{
					string sFilePath = lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
					string sFileExt = sFilePath.Substring(sFilePath.LastIndexOf(".") + 1);
					
					string sExtMap = "PDF,JPG,JPEG,PNG,TXT";
					oImgBtn.Attributes.Add("onclick",
											String.Format("window.open('..{0}','{1}');{2}",
														   sFilePath,
														   sExtMap.IndexOf(sFileExt.ToUpper()) > -1 ? "_blank" : "_self",
														   mbIsSuperAdmin ? "" : "return false;"));
					if(mbIsSuperAdmin) oImgBtn.CommandName = "DOWNLOAD";
				}

				HtmlTableRow oGridRow = e.Item.FindControl("trGridRow") as HtmlTableRow;
				if(mbIsSuperAdmin && oGridRow != null)
				{
					bool IsRead = Convert.ToBoolean(lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["IsRead"]);
					if(!IsRead)
					{
						if(String.IsNullOrEmpty(oGridRow.Attributes["class"]))
							oGridRow.Attributes["class"] = "ClsUnread";
						else
							oGridRow.Attributes["class"] += " ClsUnread";
					}
				}
			}
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to handle various commands fired by the FileList ListView control.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFileList_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if(e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
				int iFileUploadId;
				ManagementFileSharingBL oMgmtFileSharingBL;
				switch(e.CommandName)
				{
					case "EDIT":
						iFileUploadId = Convert.ToInt32(lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["UploadId"]);
						hidOldUploadId.Value = iFileUploadId.ToString();
						hidOldFilePath.Value = lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
						LoadFileUploadDetails(iFileUploadId);

						UpdateWarning.Visible = true;
						btnUpload.Visible = false;
						btnUpdate.Visible = true;
						fileRequired.Visible = false;
						break;

					case "DELETEFILE":
						iFileUploadId = Convert.ToInt32(lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["UploadId"]);
						oMgmtFileSharingBL = new ManagementFileSharingBL();
						if(oMgmtFileSharingBL.DeleteFile(iFileUploadId))
						{
							string sFileName = lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
							//string sFilePath = Server.MapPath("..") + sFileName;
                            string sFilePath = base.BasePath+"/RITeSchool" + sFileName;
							if(File.Exists(sFilePath))
								File.Delete(sFilePath);

							lblUpdateMessage.Text = "File deleted successfully.";

							// We need to clear the ListView Items since the Item.Count property remains the same(old value) if it's DataSource is null.
							lstvwFileList.Items.Clear();
							lstvwFileList.DataSourceID = FileListObjDataSource.ID;
						}
						else
						{
							lblErrorMsg.Text = "There was an error deleting the file.";
						}
						ResetControls();
						break;

					case "DOWNLOAD":
						if(!Convert.ToBoolean(lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["IsRead"]))
						{
							iFileUploadId = Convert.ToInt32(lstvwFileList.DataKeys[oCurrentItem.DisplayIndex]["UploadId"]);
							ManagementFileSharingBL.MarkAsRead(iFileUploadId, miSuperAdminUserId);

							HtmlTableRow oGridRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
							if(oGridRow != null && oGridRow.Attributes["class"].IndexOf("ClsUnread") > -1)
								oGridRow.Attributes["class"] = oGridRow.Attributes["class"].Replace(" ClsUnread", "");
						}
						break;
				}
			}
			else if(e.CommandName == "SORT")
			{
				if(hidSortExpression.Value != e.CommandArgument.ToString())
					hidSortDirection.Value = Constants.S_DESCENDING;				
			}
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to handle the sorting of the ListView
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFileList_Sorting(object sender, ListViewSortEventArgs e)
	{
		try
		{
			SetSortVariables();
			hidSortExpression.Value = e.SortExpression;
			// We need to clear the ListView Items since the Item.Count property remains the same(old value) if it's DataSource is null.
			lstvwFileList.Items.Clear();
			lstvwFileList.DataSourceID = FileListObjDataSource.ID;
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to store the value of the selected Academic Year in a hidden field.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			hidAcademicYearId.Value = (sender as DropDownList).SelectedValue;
			// We need to clear the ListView Items since the Item.Count property remains the same(old value) if it's DataSource is null.
			lstvwFileList.Items.Clear();
			lstvwFileList.DataSourceID = FileListObjDataSource.ID;
			lstvwFileList.DataBind();
			mbIsCurrentAcademicYr = hidAcademicYearId.Value == miAcademicYearId.ToString();
			btnUpload.Enabled = mbIsCurrentAcademicYr;
			trAcademicYrNotice.Visible = !mbIsSuperAdmin && !mbIsCurrentAcademicYr;
			ResetControls();
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update the ListView pager controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwFileList);
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set an invalid userid if the logged in user is not a SuperAdmin
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void FileListObjDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		try
		{
			if(!mbIsSuperAdmin)
				e.InputParameters["aiUserId"] = -1;
		}
		catch(Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This function sets up the member variables from session, for later use.
	/// </summary>
	private void SetMemberVariables()
	{
        
		
        if(Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
			miSuperAdminUserId = Convert.ToInt32(Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID]);
		mbIsSuperAdmin = Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null;
	}

	/// <summary>
	/// This function is used to display controls according to the logged in user.
	/// </summary>
    private void DisplayControlsAsPerLogin()
	{
        if (mbIsSuperAdmin)
        {
            tdEditProfile.Visible = false;
            trFileHeader.Visible = true;
        }
        else
        {
            FillSuperAdminList(String.Empty);
            FileUploadRow.Visible = true;
            tdEditProfile.Visible = true;
            trFileHeader.Visible = false;
        }
	}

	/// <summary>
	/// This function populates the Academic Year DropDownList
	/// </summary>
	private void FillAcademicYearCombo()
	{
		DataTable oDtYearTable = GetDataForAcademicYear();
		if(ddlAcademicYear != null)
		{
			ControlUtility.FillDropDownList(oDtYearTable,
											ref ddlAcademicYear,
											"Academic_Year_Id",
											"YearValue",
											Constants.S_SELECT_ALL);

			ddlAcademicYear.SelectedValue = miAcademicYearId.ToString();

			if(mbIsSuperAdmin && ddlAcademicYear.SelectedValue == "0")
				hidAcademicYearId.Value = "-1";
			else
				hidAcademicYearId.Value = miAcademicYearId.ToString();

			trAcademicYrNotice.Visible = !mbIsSuperAdmin && !mbIsCurrentAcademicYr;
		}
	}

	/// <summary>
	/// This function populates the CheckboxList of SuperAdmins.
	/// </summary>
	/// <param name="asSelectedIdList"></param>
	private void FillSuperAdminList(string asSelectedIdList)
	{
		SuperAdminDetailsBL oSuperAdminDetailsBL = new SuperAdminDetailsBL();
		List<SuperAdminDetails> oSuperAdminList = oSuperAdminDetailsBL.GetAll();
		int iCount = 0;
		if(oSuperAdminList != null && oSuperAdminList.Count > 0)
		{
			oSuperAdminList.ForEach(oSuperAdmin =>
			{
				string sAdminId = oSuperAdmin.UserId.ToString();
				chklstSuperAdmin.Items.Add(new ListItem(oSuperAdmin.FullName, sAdminId));
				if(asSelectedIdList.IndexOf(sAdminId) > -1) chklstSuperAdmin.Items[iCount].Selected = true;
				iCount++;
			});
		}
	}

	/// <summary>
	/// This function is used to provide data about all academic years of school.
	/// </summary>
	/// <returns></returns>
	private DataTable GetDataForAcademicYear()
	{
		SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
		DataTable oDtYearInfo = null;

		if(!mbIsSuperAdmin)
			oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId, miUserId, moUserRole.ToInt());
		else
			oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSuperAdmin(miSchoolId, miUserId);

		return oDtYearInfo;
	}

	/// <summary>
	/// This method is used to set java script attributes.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		valSumErrorMessage.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		new Button[] { btnUpload, btnCancel, btnBack,btnUpdate }.ApplyEffect();
	}

	/// <summary>
	/// This function uploads the selected file in the FileUploadControl to the server
	/// </summary>
	/// <returns>The name of the file.</returns>
	private string UploadFileToServer()
	{
		// Append Timestamp to the FileName to prevent files getting over-written.
		string sFileName = FileUploadControl.FileName.Insert(FileUploadControl.FileName.LastIndexOf('.'), DateTime.Now.ToString("_yyyyMMddHHmmss"));

		// Save the file to the server
		sFileName = sFileName.Replace(" ", "_");
		//string path = Server.MapPath("..") + @"\UPLOADS\Management\" + sFileName;
        string path = base.BasePath + @"\RITeSchool\UPLOADS\Management\" + sFileName;
		FileUploadControl.SaveAs(path);

		return sFileName;
	}

	/// <summary>
	/// This function creates a ManagementFileUploadDetails object from values on the page.
	/// </summary>
	/// <param name="aiUploadId">Used in Update Operations. Pass -1 for Insert Operations.</param>
	/// <param name="asFileName">The filename of the File uploaded.</param>
	/// <returns>The populated ManagementFileUploadDetails object.</returns>
	private ManagementFileUploadDetails PopulateFileUploadDetails(int aiUploadId, string asFileName)
	{
		// Record the upload in the database
		ManagementFileUploadDetails oFileUpload = new ManagementFileUploadDetails
		{
			UploadId = aiUploadId,
			Title = txtTitle.Text,
			Description = txtDescription.Text,
			FilePath = @"/UPLOADS/Management/" + asFileName,
			UploadedById = miUserId,
			SchoolId = miSchoolId,
			AcademicYearId = miAcademicYearId
		};

		List<string> oSelectedIds = new List<string>();
		foreach(ListItem item in chklstSuperAdmin.Items)
		{
			if(item.Selected)
				oSelectedIds.Add(item.Value);
		}

		oFileUpload.SelectedUserIds = String.Join(",", oSelectedIds.ToArray());
		return oFileUpload;
	}

	/// <summary>
	/// This function is used to load fileupload details of the file being edited.
	/// </summary>
	/// <param name="aiFileUploadId"></param>
	private void LoadFileUploadDetails(int aiFileUploadId)
	{
		ManagementFileSharingBL oMgmtFileSharingBL = new ManagementFileSharingBL(aiFileUploadId);
		txtTitle.Text = oMgmtFileSharingBL.FileUploadDetails.Title;
		txtDescription.Text = oMgmtFileSharingBL.FileUploadDetails.Description;

		// Save Uploaded For Ids to a HiddenField for later actions(update)
		hidOldUploadedForIds.Value = oMgmtFileSharingBL.FileUploadDetails.SelectedUserIds;

		foreach(ListItem item in chklstSuperAdmin.Items)
		{
			item.Selected = false;
			foreach(string id in oMgmtFileSharingBL.FileUploadDetails.SelectedUserIds.Split(','))
			{
				if(item.Value.Equals(id)) item.Selected = true;
			}
		}
	}

	/// <summary>
	/// This function sets the hidden fields value that is maintained to remember sort direction
	/// </summary>
	private void SetSortVariables()
	{
		if(hidSortDirection.Value == Constants.S_DESCENDING)
			hidSortDirection.Value = Constants.S_ASCENDING;
		else
			hidSortDirection.Value = Constants.S_DESCENDING;
	}

	/// <summary>
	/// This function adds an image to the header column to indicate the currently sorted column.
	/// </summary>
	private void AddSortImage()
	{
		string sSortExpression = S_DEFAULT_SORT_EXP;
		string sSortDirection = Constants.S_DESCENDING;
		if(!String.IsNullOrEmpty(hidSortExpression.Value))
			sSortExpression = hidSortExpression.Value;
		if(!String.IsNullOrEmpty(hidSortDirection.Value))
			sSortDirection = hidSortDirection.Value;
		HtmlTableRow oHtmlTableHeaderRow = lstvwFileList.FindControl("trHeader") as HtmlTableRow;
		if(oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, sSortExpression, sSortDirection);
	}

	/// <summary>
	/// This function sends an SMS to all the users that are selected from the Management Users list.
	/// </summary>
	private void SendSMS()
	{

		string sSalarySMSText = string.Empty;
		string sSmsSubject = string.Empty;

		SuperAdminDetailsBL oSuperAdminDetailsBL = new SuperAdminDetailsBL();
		List<SuperAdminDetails> oSuperAdminList = oSuperAdminDetailsBL.GetAll();

		SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
		string sSMSSenderName = oSchoolBL.SMSSenderName;

		string sFileUploadSMS = string.Empty;
		Hashtable moManualMobileNo = new Hashtable();

		string sDisplayText = String.Empty;

		foreach(ListItem item in chklstSuperAdmin.Items)
		{
			if(item.Selected)
				sDisplayText += item.Text + ", ";
		}
		sDisplayText = sDisplayText.Substring(0, sDisplayText.LastIndexOf(","));

		foreach(ListItem item in chklstSuperAdmin.Items)
		{
			if(item.Selected)
			{
				SuperAdminDetails oSuperAdmin = oSuperAdminList.Find(obj => obj.UserId == Convert.ToInt32(item.Value));
				moManualMobileNo[oSuperAdmin.UserId] = oSuperAdmin.MobileNumber;
				sFileUploadSMS = String.Format("Greetings from PPS PUNE! A new File - \"{0}\" is available for your review on our school website. Thanks...", FileUploadControl.FileName);

				SMS oSMS = new SMS();
				oSMS.SenderRoleID = moUserRole.ToInt();
				oSMS.SenderID = miUserId;
				oSMS.InsertedByID = -9999;
				oSMS.Sender = oSchoolBL.SMSSenderName;
				oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
				oSMS.SMSText = sFileUploadSMS;
				oSMS.AcademicYearID = miAcademicYearId;
				oSMS.SchoolID = miSchoolId;
				oSMS.DisplayText = sDisplayText;
				oSMS.To = moManualMobileNo;
				oSMS.Send();
				moManualMobileNo.Clear();
			}
		}
	}

	/// <summary>
	/// This function resets all the controls on the Upload/Update file form.
	/// </summary>
	private void ResetControls()
	{
		txtTitle.Text = String.Empty;
		txtDescription.Text = String.Empty;
		chkSendSMS.Checked = false;

		foreach(ListItem item in chklstSuperAdmin.Items)
			item.Selected = false;

		UpdateWarning.Visible = false;
		btnUpload.Visible = true;
		btnUpdate.Visible = false;
		fileRequired.Visible = true;

		hidOldUploadId.Value = String.Empty;
		hidOldFilePath.Value = String.Empty;
		hidOldUploadedForIds.Value = String.Empty;
	}

	/// <summary>
	/// This method is used to log an exception to the error log table in the database.
	/// </summary>
	/// <param name="ex"></param>
	/// <param name="asMethodName"></param>
	private void AddExceptionToErrorLog(Exception ex, MethodBase currentMethod)
	{
		ExceptionHandler.WriteExceptionToErrorLog(String.Format("{0}. Trace: {1}", ex.Message, ex.StackTrace)
												 , String.Format("{0}.{1}", currentMethod.DeclaringType.FullName, currentMethod.Name)
												 , miUserId);
	}

    /// <summary>
    /// This method is used to set postback URL.
    /// </summary>
    private void SetPostbackUrl()
    {
        if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
        {
            btnBack.PostBackUrl = Constants.S_PAGE_SUPERADMIN_DASHBOARD;
        }
        else 
        {
            btnBack.Visible = false;
        }
    }

	#endregion -- PRIVATE METHOD(s) --
}