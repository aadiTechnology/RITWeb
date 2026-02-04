/* File Name :- TeacherScreenAccessUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 24-Sept-2009
 * Purpose ;- Code Review.
 * Class Description :- This class is used to provide access of extra screens to teacher.
 */
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
using SchoolEntities;
using System.Resources;
using Utility;

public partial class TeacherScreenAccessUI : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_REPORTS_TABLE = "Reports";	

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// 	This event is used to fill salutation combo as well to set default properties and add/edit mode.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{            
			if (!IsPostBack)
			{
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
				if (CheckPreCondition())
				{
					FillTeachersComboBox();
					FillScreenAccessDetails();
				}
				SetJavaScriptAttributes();
				SetDefaultProperties();
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
                if (lblUpdateSucess != null && lblUpdateSucess.Text != string.Empty)
                    lblUpdateSucess.Text = Resources.LocalizedResources.ExtraScreensAssignSuccessfully;
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This method is used to save or update teacher details.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateTeacher();
			cmbTeachers.Focus();
            SaveConfigDetails(Constants.SchoolConfigurations.TeacherScreenAccess.ToInt());
			lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = Resources.LocalizedResources.ExtraScreensAssignSuccessfully; ;
		}
		catch (DuplicateUserException ex)
		{
			lblErrorMsg.Text = ex.Message;
			lblErrorMsg.Visible = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    ///  This event is used to search teacher by name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSearch.Text == Resources.LocalizedResources.Search)
            {
                SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();                
                int aiUserId = oSchoolWiseTeacherMasterBL.GetUserIdFromUserName(txtName.Text.Trim(), miSchoolId, miAcademicYearId);
                cmbTeachers.SelectedValue = aiUserId.ToString();
                cmbTeachers_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

	/// <summary>
	/// 	Redirects the user to the Teacher_Related menu in School Configuration.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			var oMasterPage = Master as MasterPage;
			oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related)));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		Populates details according to the teacher selected.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			FillScreenAccessDetails();
			lblUpdateSucess.Text = string.Empty;
			lblUpdateSucess.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to fill child listview.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwReportFolders_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			var oCurrentItem = e.Item as ListViewDataItem;

			var oChkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
			oChkSelect.Attributes.Add("onclick", "SelectUnSelectChilds('" + oCurrentItem.DisplayIndex + "',this);");

			var oHtmlTableRow = oCurrentItem.FindControl("trReports") as HtmlTableRow;
			var oHtmlTableCell = oHtmlTableRow.FindControl("tdReports") as HtmlTableCell;
			var lstvwReports = oHtmlTableCell.FindControl("lstvwReports") as ListView;
			DataTable dtReports = null;
			if (ViewState[S_REPORTS_TABLE] != null)
				dtReports = ViewState[S_REPORTS_TABLE] as DataTable;
			int iReportFolderId = lstvwReportFolders.DataKeys[oCurrentItem.DisplayIndex]["Report_Folder_Id"].ToInt();

			DataRow[] oDatarows = dtReports.Select("Report_Folder_Id = " + iReportFolderId);
            DataTable dtReportDetails = new DataTable();
			if (oDatarows.Length > 0)
                dtReportDetails = oDatarows.CopyToDataTable();

            lstvwReports.DataSource = dtReportDetails;
			lstvwReports.DataBind();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to disable column.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwReports_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			var oCurrentItem = e.Item as ListViewDataItem;
			var chkReportName = oCurrentItem.FindControl("chkReportName") as CheckBox;
			bool bHasFullAccess = (sender as ListView).DataKeys[oCurrentItem.DisplayIndex]["HasFullAccess"].ToBool();
			bool bIsViewAvailable = (sender as ListView).DataKeys[oCurrentItem.DisplayIndex]["IsViewAvailable"].ToBool();
			char cHasAccess = Convert.ToChar((sender as ListView).DataKeys[oCurrentItem.DisplayIndex]["HasAccess"]);
			var chkHasFullAccess = oCurrentItem.FindControl("chkHasFullAccess") as CheckBox;

			chkReportName.Checked = cHasAccess == 'Y';
			chkHasFullAccess.Checked = bHasFullAccess;
			chkHasFullAccess.Enabled = bIsViewAvailable;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// 	This method is used to fill screen access details.
	/// </summary>
	private void FillScreenAccessDetails()
	{
		DataSet oDSScreenAccessDetails = GetScreenAccessDetails();
		lstvwScreenAccess.DataSource = FilterRestrictedScreens(oDSScreenAccessDetails.Tables[0]);
		lstvwScreenAccess.DataBind();
		chkAcademicApplicable.Checked = false;
		chkFinancialYearApplicable.Checked = false;
		if (oDSScreenAccessDetails.Tables.Count > 2 && oDSScreenAccessDetails.Tables[3].Rows.Count > 0)
		{
			chkAcademicApplicable.Checked = Convert.ToChar(oDSScreenAccessDetails.Tables[3].Rows[0][0]) == Constants.C_YES;
		}

		FillReportFolderNode(oDSScreenAccessDetails);
	}

	/// <summary>
	///		Filters the passed DataSet and removes all restricted screens if the current user does not have access to it.
	/// </summary>
	/// <param name="adtScreenAccessDetails"></param>
	/// <returns></returns>
	private DataTable FilterRestrictedScreens(DataTable adtScreenAccessDetails)
	{
		if (adtScreenAccessDetails.IsNull() || adtScreenAccessDetails.Rows.Count == 0)
			return adtScreenAccessDetails;
	
		var oSchoolwiseSupervisorMaster = new SchoolWiseSupervisorMasterBL();
		List<SchoolModule> lstModules = oSchoolwiseSupervisorMaster.GetRestrictedModulesForUser(miUserId);

		if (lstModules.IsNull() || lstModules.Count == 0)
			return adtScreenAccessDetails;

		lstModules.ForEach(m => adtScreenAccessDetails.Select(String.Format("SchoolModulesId = {0}", m.Id))
		                        					  .ToList()
		                        					  .ForEach(r => r.Delete()));
	
		return adtScreenAccessDetails;
	}

	/// <summary>
	/// 	This method is used to update teacher detail.s
	/// </summary>
	private void UpdateTeacher()
	{
		int iTeacherUserId = cmbTeachers.SelectedValue.ToInt();
		
		var oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL
		                                 	{
		                                 		UserId = iTeacherUserId,
		                                 		IsAcademicYrApplicable = chkAcademicApplicable.Checked ? Constants.C_YES : Constants.C_NO,
		                                 		IsFinancialYearApplicable = chkFinancialYearApplicable.Checked,
		                                 		UpdatedById = miUserId
		                                 	};
		oSchoolWiseTeacherMasterBL.UpdateTeachersAcademicYrApplicable();

		var oSchoolWiseTeacherBL = new SchoolWiseSupervisorMasterBL
		                           	{
		                           		User_Id = iTeacherUserId
		                           	};
		string sReportsAccessId = GetSelectedReports();
		oSchoolWiseTeacherBL.AddSupervisorScreens(iTeacherUserId, miUserId, hidScreenAccess.Value, sReportsAccessId);

		FillScreenAccessDetails();
	}

	/// <summary>
	/// 	This method is used to get comma sapareted selected report Ids
	/// </summary>
	/// <returns> </returns>
	private string GetSelectedReports()
	{
		const string S_ELEMENT = "element";

		var oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement("ReportAccess");
		XmlNode oRootNode = oDoc.CreateNode(S_ELEMENT, "ReportAccess", "");

		foreach (ListViewDataItem oCurrentFolder in lstvwReportFolders.Items)
		{
			var oHtmlTableRow = oCurrentFolder.FindControl("trReports") as HtmlTableRow;
			var oHtmlTableCell = oHtmlTableRow.FindControl("tdReports") as HtmlTableCell;
			var lstvwReports = oHtmlTableCell.FindControl("lstvwReports") as ListView;

			foreach (ListViewDataItem oCurrentReport in lstvwReports.Items)
			{
				var chkReportName = oCurrentReport.FindControl("chkReportName") as CheckBox;
				var chkHasFullAccess = oCurrentReport.FindControl("chkHasFullAccess") as CheckBox;
				if (chkReportName.Checked)
				{
					XmlNode oNode = oDoc.CreateNode(S_ELEMENT, "ReportAccess", "");
					int iReportId = lstvwReports.DataKeys[oCurrentReport.DisplayIndex]["Report_Id"].ToInt();
					XmlAttribute attr = oDoc.CreateAttribute("Report_Id");
					attr.Value = iReportId.ToString();
					oNode.Attributes.Append(attr);

					attr = oDoc.CreateAttribute("HasFullAccess");
					attr.Value = chkHasFullAccess.Checked ? "1" : "0";
					oNode.Attributes.Append(attr);

					attr = oDoc.CreateAttribute("IsViewAvailable");
					attr.Value = lstvwReports.DataKeys[oCurrentReport.DisplayIndex]["IsViewAvailable"].ToBool() ? "1" : "0";
					oNode.Attributes.Append(attr);
					oRootNode.AppendChild(oNode);
				}
			}
		}

		root.AppendChild(oRootNode);
		return root.InnerXml;
	}

	/// <summary>
	/// 	This method is used to get teacher allow path ids.
	/// </summary>
	/// <returns> </returns>
	private DataSet GetScreenAccessDetails()
	{
		var oSchoolWiseTeacherBL = new SchoolWiseSupervisorMasterBL();
		DataSet oDSScreenId = oSchoolWiseTeacherBL.GetScreenAccessDetails(cmbTeachers.SelectedValue.ToInt(), miUserId, true);
		return oDSScreenId;
	}

	/// <summary>
	/// 	This method is used to set javascript attributes.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		btnSave.Attributes.Add("onclick", "CalculateAccess();");
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave });
	}

	/// <summary>
	/// 	This method is used to set default property.
	/// </summary>
	private void SetDefaultProperties()
	{
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
		cmbTeachers.Focus();
	}

	/// <summary>
	/// 	This method is used to fill tree view with navigation path name.
	/// </summary>
	/// <param name="aoDataSet"> </param>
	private void FillReportFolderNode(DataSet aoDataSet)
	{
		const int I_TBL_REPORT_FOLDER_NAME = 1;
		const int I_TBL_REPORT_NAME = 2;
		ViewState[S_REPORTS_TABLE] = aoDataSet.Tables[I_TBL_REPORT_NAME];
		DataTable aoDTReportFolders = aoDataSet.Tables[I_TBL_REPORT_FOLDER_NAME];
		lstvwReportFolders.DataSource = aoDTReportFolders;
		lstvwReportFolders.DataBind();
		foreach (ListViewDataItem oCurrentFolder in lstvwReportFolders.Items)
		{
			char sHasAccess = Convert.ToChar(lstvwReportFolders.DataKeys[oCurrentFolder.DisplayIndex]["HasAccess"]);
			if (sHasAccess != 'Y')
				continue;
			var chkRepFolderName = oCurrentFolder.FindControl("ChkSelect") as CheckBox;
			chkRepFolderName.Checked = true;
		}
	}

	/// <summary>
	/// 	This method is used to fill teachers combobox.
	/// </summary>
	private void FillTeachersComboBox()
	{
		var oSchoolUserCollectionBL = new SchoolUserCollectionBL();
        DataTable oDt = oSchoolUserCollectionBL.GetUserAsTeacherDetails(miSchoolId, miAcademicYearId, string.Empty, Constants.S_ZERO, "Teacher_Designation_Master.DesignationSortOrder,Teacher_First_Name,Teacher_Middle_Name,Teacher_Last_Name", 2000, 0);
       // oDt.DefaultView.Sort = "Teacher_First_Name,Teacher_Middle_Name,Teacher_Last_Name"; 
		cmbTeachers.Bind(oDt, "USER_ID", "Teacher_Name");
	}

	private bool CheckPreCondition()
	{
		bool bReturn = false;

		string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.TeacherScreenAccess);
		if (sLinks.Equals(string.Empty))
		{
			divErr.Visible = false;
			bReturn = true;
		}
		else
		{
			divErr.InnerHtml = sLinks;
			VisibleOrHideControls();
		}
		return bReturn;
	}

	private void VisibleOrHideControls()
	{
		btnSave.Visible = false;
		tblTeacherCombo.Visible = false;
		lblOtherMenus.Visible = false;
		lblSchoolConfig.Visible = false;
		TrLable.Visible = false;
	}

	#endregion -- PRIVATE METHOD(s) --

  
  
}