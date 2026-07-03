/* Created By :- Sachin
* Created Date :- 10-Nov-2009
* Class Description :- This class is used to define user and staff group association.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.IO;

public partial class UserAndStaffGroupAsso : ExportDataTable
{
    #region Constants

    private const string S_STAFF_GROUPS = "StaffGroups";
    private const int I_USER_ROLE_TABLE = 0;
    private const int I_STAFF_GROUP_TABLE = 1;
    private const string S_STAFF_DETAILS = "Staff Details";
    private const string S_SAVE_MESSAGE = "Association has been saved successfully !!!";
    private const string S_DEFAULT_DATETIME = "1/1/1900 12:00:00 AM";
    private const string S_DEFAULT_DATE_2 = "01-Jan-0001";
    private const string S_DEFAULT_DATE_3 = "01-Jan-1900";
    private const int I_PAGE_SIZE = 20;
    private const string S_CONSOLIDATED_STAFF = "4";
    private const string S_PROFESSIONAL_STAFF = "5";
    private const string S_EDIT = "Edit";
    private const string S_ADD = "Add";
	
	// We set a default invalid value, since these variables are used in javascript.
	// Else, it would give a syntax error if the fields are not initialized.
	protected string CONSOLIDATED_STAFFGROUP_ID = Constants.S_DEFAUL_SCHOOL_ID;
	protected string PROFESSIONAL_STAFFGROUP_ID = Constants.S_DEFAUL_SCHOOL_ID;

    #endregion

    #region Data Member

    private DataTable oDTUserTable;
    private Dictionary<string, string> mdicUserStaffGroupMap;
    private UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL;
	
	#endregion

    #region Events

    /// <summary>
    /// This event is used to fill user role combobox and set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
            hidUserRoleId.Value = cmbUserRoles.SelectedValue;
			InitializeStaffGroupIds();
            if (!IsPostBack)
            {
               
                SetJavascriptAttributes();
                SetDefaultValues();
                if (CheckPreCondition())
                    FillUserRoleCombobox();
                FillUserTypesCombo();
                ReadQuerystring();
            }

            SetAttribute();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user grid according to selected role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbUserRoles.SelectedValue != Constants.S_ZERO || txtUserName.Text.Trim() != string.Empty)
            {
                trUserType.Visible = true;
                DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
                FillUserDetails();            
            }
            else
            {
                SetDefaultValues();
                trNoRecordMsg.Visible = false;
                lstvwAssociation.DataSource = null;
                lstvwAssociation.DataBind();
                trUserType.Visible = false;
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to set attributes on listview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssociation_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            DataRowView oDataRowView = (DataRowView)oCurrentItem.DataItem;
            int iUserId = Convert.ToInt32(oDataRowView["User_Id"]);

            string sStaffName = ((LinkButton)oCurrentItem.FindControl("lblStaffName")).Text;
            DropDownList cmbStaffGroups = (DropDownList)oCurrentItem.FindControl("cmbStaffGroups");
            TextBox txtEmployeeNo = (TextBox)oCurrentItem.FindControl("txtEmployeeNo");
            TextBox txtAccountNo = (TextBox)oCurrentItem.FindControl("txtAccountNo");
            TextBox txtPanNo = (TextBox)oCurrentItem.FindControl("txtPanNo");
            TextBox txtProvidentFundNo = (TextBox)oCurrentItem.FindControl("txtProvidentFundNo");
            TextBox txtUAN = (TextBox)oCurrentItem.FindControl("txtUAN");
            ImageButton imgActiveDeacActive = (ImageButton)oCurrentItem.FindControl("imgActiveDeactive");
            LinkButton lblStaffName = (LinkButton)oCurrentItem.FindControl("lblStaffName");
            LinkButton lnkBtnConfig = (LinkButton)oCurrentItem.FindControl("lnkBtnConfig");
            LinkButton lnkBtnLeave = (LinkButton)oCurrentItem.FindControl("lnkBtnLeave");
            LinkButton lnkBtnInsurance = (LinkButton)oCurrentItem.FindControl("lnkBtnInsurance");
            TextBox txtDOJ = (TextBox)oCurrentItem.FindControl("txtDateOfJoining");
            TextBox txtDOR = (TextBox)oCurrentItem.FindControl("txtDateOfResign");
            TextBox txtDOP = (TextBox)oCurrentItem.FindControl("txtDateOfPermanent");
            TextBox txtTransferDate = (TextBox)oCurrentItem.FindControl("txtTransferDate");

            LinkButton lnkBtnEnCash = (LinkButton)oCurrentItem.FindControl("lnkBtnEnCash"); 

            HiddenField hidIsLockedUser =oCurrentItem.FindControl("hidIsLockedUser") as HiddenField;
            
           
            if(txtAccountNo.Text == string.Empty)
            txtAccountNo.Text = "-";

            if (txtProvidentFundNo.Text == string.Empty)
            txtProvidentFundNo.Text = "-";

            if (txtDOJ.Text == S_DEFAULT_DATE_2 || txtDOJ.Text == S_DEFAULT_DATE_3)
                txtDOJ.Text = string.Empty;

            if (txtDOP.Text == S_DEFAULT_DATE_2 || txtDOP.Text == S_DEFAULT_DATE_3)
                txtDOP.Text = string.Empty;

            if (txtDOR.Text == S_DEFAULT_DATE_2 || txtDOR.Text == S_DEFAULT_DATE_3)
                txtDOR.Text = string.Empty;

            if (txtTransferDate.Text == S_DEFAULT_DATE_2 || txtTransferDate.Text == S_DEFAULT_DATE_3)
                txtTransferDate.Text = string.Empty;

            int iIsLocked = 0;
            if (lstvwAssociation.DataKeys[iRowId]["Is_Locked"] != null && lstvwAssociation.DataKeys[iRowId]["Is_Locked"] != DBNull.Value)
                iIsLocked = Convert.ToInt32(lstvwAssociation.DataKeys[iRowId]["Is_Locked"]);

            if (iIsLocked == 0)
            {
                imgActiveDeacActive.ToolTip = sStaffName + " [Activate]";
                imgActiveDeacActive.ImageUrl = "~/RITeSchool/images/Icon_UserUnlock.gif";
                imgActiveDeacActive.Attributes.Add("onclick", "if(!window.confirm('Are you sure you want to deactivate this user?')){return false;}");
            }
            else
            {
                imgActiveDeacActive.ToolTip = sStaffName + " [Deactivate]";
                imgActiveDeacActive.ImageUrl = "~/RITeSchool/images/Icon_UserLock.gif";
                imgActiveDeacActive.Attributes.Add("onclick", "if(!window.confirm('Are you sure you want to activate this user?')){return false;}");               
            }


            cmbStaffGroups.ToolTip = sStaffName + " [Staff Groups]";
            txtEmployeeNo.ToolTip = sStaffName + " [Employee No]";
            txtAccountNo.ToolTip = sStaffName + " [Account No]";
            txtProvidentFundNo.ToolTip = sStaffName + " [P.F. No]";
            txtUAN.ToolTip = sStaffName + "[Univarsal Account No]";
            txtPanNo.ToolTip = sStaffName + " [Pan No]";

            txtEmployeeNo.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "','N',this)){return false;}");
            txtAccountNo.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "','Y',this)){return false;}");
            txtProvidentFundNo.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "','Y',this)){return false;}");
            txtUAN.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "','Y',this)){return false;}");
            txtPanNo.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "','N',this)){return false;}");                

            lnkBtnConfig.ToolTip = sStaffName + " [Earnings-Deductions Configuration]";
            lnkBtnLeave.ToolTip = sStaffName + " [Leave Configuration]";
            lnkBtnInsurance.ToolTip = sStaffName + " [Insurance Details]";
            txtDOJ.ToolTip = sStaffName + " [Joining Date]";
            txtDOP.ToolTip = sStaffName + " [Permanent Date]";
            txtDOR.ToolTip = sStaffName + " [Resignation Date]";
            txtTransferDate.ToolTip = sStaffName + " [Transfer Date]";

            lnkBtnEnCash.ToolTip = sStaffName + "[Leave Encashment]";

            int iStaffGroupId = Convert.ToInt32(oDataRowView["StaffGroupId"]);
            
            SetControlsVisibility(oCurrentItem, oDataRowView, ref lnkBtnConfig, ref lnkBtnLeave, ref lnkBtnInsurance, iRowId, iStaffGroupId, iIsLocked);
                
                string sEdit = lnkBtnConfig.Text == S_EDIT ? Constants.S_ONE : Constants.S_ZERO;    
                string sStructureQueryString = CommonUtility.EncryptQuerystring("UserId=" + iUserId + "&UserName=" + sStaffName);
                lblStaffName.Attributes.Add("onclick", "OpenSalaryStructurePopup('" + sStructureQueryString + "','" + sEdit + "'); return false;");          
           
            int iUsersStaffGroupsAssociationId = Convert.ToInt32(oDataRowView["UsersStaffGroupsAssociationId"]);
            if (cmbStaffGroups.SelectedValue == CONSOLIDATED_STAFFGROUP_ID || cmbStaffGroups.SelectedValue == PROFESSIONAL_STAFFGROUP_ID)
            {
                txtDOP.Enabled = false;
                txtDOR.Enabled = false;
                txtTransferDate.Enabled = false;
            }
            
            hidIsLockedUser.Value = lstvwAssociation.DataKeys[iRowId]["Is_Locked"].ToString();
            if (iUsersStaffGroupsAssociationId == Constants.I_ZERO)
                hidIsLockedUser.Value = "False";

            char cIsLocked=Constants.C_NO;
            if (hidIsLockedUser.Value== "True")
                cIsLocked = 'Y';
            if (lstvwAssociation.DataKeys[iRowId]["IsDeleted"] != DBNull.Value)
            {                
                if (lstvwAssociation.DataKeys[iRowId]["IsDeleted"].ToString() == "Y")
                {
                    lnkBtnConfig.ForeColor = Color.Red;
                    lnkBtnLeave.ForeColor = Color.Red;
                    lnkBtnInsurance.ForeColor = Color.Red;
                    lblStaffName.ForeColor = Color.Red;
                    cmbStaffGroups.ForeColor = Color.Red;
                    txtEmployeeNo.ForeColor = Color.Red;
                    txtAccountNo.ForeColor = Color.Red;
                    txtProvidentFundNo.ForeColor = Color.Red;
                    txtUAN.ForeColor = Color.Red;
                    txtPanNo.ForeColor = Color.Red;
                    txtDOJ.ForeColor = Color.Red;
                    txtDOR.ForeColor = Color.Red;
                    txtDOP.ForeColor = Color.Red;
                    txtTransferDate.ForeColor = Color.Red;
                    lnkBtnEnCash.ForeColor = Color.Red;
                }
            }

            string sEncryptedQueryString = GetQueryString(iStaffGroupId, oDataRowView, iUserId, cIsLocked);
            string sLeavesQueryString = "UserId=" + iUserId +
                                        "&UserRoleId=" + cmbUserRoles.SelectedValue +
                                        "&Filter=" + txtUserName.Text.Trim() +
                                        "&IsLocked=" + cIsLocked;

            lnkBtnConfig.Attributes.Add("onclick", "if(!OpenPopup('" + sEncryptedQueryString + "'," + iRowId + ")) return false;");
            lnkBtnLeave.Attributes.Add("onclick", "if(!OpenAllowedLeavesPopup('" + CommonUtility.EncryptQuerystring(sLeavesQueryString) + "'," + iRowId + ")) return false;");
            lnkBtnInsurance.Attributes.Add("onclick", "if(!OpenInsurancePopup('" + sEncryptedQueryString + "'," + iRowId + ")) return false;");
			cmbStaffGroups.Attributes.Add("onchange", "if(!ConfirmUpdate('" + iStaffGroupId + "'," + iRowId + ")) return false;");
            //if (cmbStaffGroups.SelectedItem.Text == "Administrative Staff")
            //{
                lnkBtnEnCash.Enabled = true;
                lnkBtnEnCash.Attributes.Add("onclick", "if(!OpenEnCashLeavePopup('" + CommonUtility.EncryptQuerystring(sLeavesQueryString) + "'," + iRowId + ")) return false;");
            //}
            //else
            //{
            //    lnkBtnEnCash.Enabled = false;
            //    lnkBtnEnCash.Text = "-";
            //}
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Thi event is used to set pager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssociation_DataBound(object sender, EventArgs e)
    {
        try
        {
			if(oDTUserTable!= null && oDTUserTable.Rows.Count > Constants.I_ZERO)
            {
                ControlUtility.FillListViewPagerFooter(lstvwAssociation, DtPgCount);
                SetConfirmationMessage();
                FillHeaderCombobox();

                DataPager oDataPager = lstvwAssociation.FindControl("DtPgDropDown") as DataPager;
                if (oDataPager != null)
                {
                    DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                    if (ddlCnt != null)
                        hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
                }

				hidRowCnt.Value = lstvwAssociation.Items.Count.ToString();
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        ControlUtility.SetDataPagerAccordingToPageNo(lstvwAssociation);
        FillUserDetails();
    }

    /// <summary>
    /// This method is used to save association and add entry into configuration table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            if (hidIsConfigured.Value != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.UsersStaffGroupsAssociation));

            using (var swFile = new StreamWriter(Server.MapPath(@"~\AutoSearchCache.txt"), true))
            {
                swFile.WriteLine("\n" + DateTime.Now);
                swFile.Flush();
                swFile.Close();
            }
        }
        catch (SqlException ex)
        {
            trErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
            FillUserDetails();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to deactivate users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssociation_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            // Lock the selected user.
            if (e.CommandName.ToUpper().Equals("LOCK"))
            {   
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = oCurrentItem.DisplayIndex;

                bool bIsLocked = Convert.ToBoolean(lstvwAssociation.DataKeys[iRowId]["Is_Locked"]);
                int iUserId = Convert.ToInt32(lstvwAssociation.DataKeys[iRowId]["User_Id"]);                
                moUsersStaffGroupsAssociationBL.LockUnlocksalaryUser(iUserId, miSchoolId, !bIsLocked, miUserId);
                FillUserDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            trUserType.Visible = true;
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            hidUserStaffgroupsAssociationId.Value = string.Empty;
            FillUserDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable oDataTable = BuildTableForExport();
            if (oDataTable.IsNonEmpty())
                ExportToExcel("StaffGroupUsers.xls", oDataTable);
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
    /// This method is used to check date change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DateChanged(object sender, EventArgs e)
    {
        RJS.Web.WebControl.PopCalendar cal = sender as RJS.Web.WebControl.PopCalendar;
        ListViewDataItem item = cal.Parent.Parent.Parent as ListViewDataItem;
        hidUserStaffgroupsAssociationId.Value = hidUserStaffgroupsAssociationId.Value + "," + item.DisplayIndex.ToString();
    }

    /// <summary>
    /// This event is used to Change USer Type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to Change the RdbWithSalutation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RdbWithSalutation_CheckedChanged(object sender, EventArgs e)
    {
         try
        {
            btnSearch_Click(btnSearch, null);
        }
        catch (Exception ex)
         {
             ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
         }
    }
    /// <summary>
    /// This event is used to Change the RdbWithoutSalutation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RdbWithoutSalutation_CheckedChanged(object sender, EventArgs e)
    {
         try
        {
          btnSearch_Click(btnSearch, null);
        }
         catch (Exception ex)
         {
             ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
         }
    }


    #endregion

    #region Methods

	/// <summary>
	///		Deserializes the StaffGroupId mapped object from json, if it exists.
	/// </summary>
	private void InitializeStaffGroupIds()
	{
		if (!hidUserStaffGroupIdMap.Value.IsNullOrEmpty())
		{
			var jsSerializer = new JavaScriptSerializer();
			mdicUserStaffGroupMap = jsSerializer.Deserialize<Dictionary<string, string>>(hidUserStaffGroupIdMap.Value);

			SetStaffGroupIds();
		}
	}

	/// <summary>
	///		Sets the values of StaffGroup variables, from the mapped dictionary object.
	/// </summary>
	private void SetStaffGroupIds()
	{
		if (mdicUserStaffGroupMap.Count <= 0)
			return;
		
		if (mdicUserStaffGroupMap.ContainsValue(S_CONSOLIDATED_STAFF))
			CONSOLIDATED_STAFFGROUP_ID = mdicUserStaffGroupMap.Where(d => d.Value == S_CONSOLIDATED_STAFF).Select(d => d.Key).FirstOrDefault();
		
		if (mdicUserStaffGroupMap.ContainsValue(S_PROFESSIONAL_STAFF))
			PROFESSIONAL_STAFFGROUP_ID = mdicUserStaffGroupMap.Where(d => d.Value == S_PROFESSIONAL_STAFF).Select(d => d.Key).FirstOrDefault();
	}

	/// <summary>
	/// This function buils the table that will be used to Export User Staff Group Details to an Excel Sheet.
	/// </summary>
	/// <returns></returns>
	private DataTable BuildTableForExport()
	{	
		int iUserRoleId = Convert.ToInt32(cmbUserRoles.SelectedValue);
		string sUserName = txtUserName.Text.Trim();
		UsersStaffGroupsAssociationBL oUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL(miSchoolId, miAcademicYearId);
        DataTable oDataTable = oUsersStaffGroupsAssociationBL.GetUserDetails(iUserRoleId, sUserName, ddlUserType.SelectedValue.ToInt(), RdbWithSalutation.Checked);
		DataTable oExportTable = new DataTable();
        oExportTable.AddColumns(new string[] { "Staff Name", "Staff Group", "Employee No.", "Account No.", "P.F. No.", "UAN", "Pan No.", "Joining Date", "Permanent Date", "Resignation Date" });

		DataRow newRow = null;
		foreach (DataRow row in oDataTable.Rows)
		{
			newRow = oExportTable.NewRow();
			newRow["Staff Name"] = row["UserName"];
			newRow["Staff Group"] = row["StaffGroupName"];
			newRow["Employee No."] = row["EmployeeNo"];
			newRow["Account No."] = row["AccountNo"];
			newRow["P.F. No."] = row["ProvidentFundNo"];
            newRow["UAN"] = row["UAN"];
			newRow["Pan No."] = row["PanNo"];
			if (!row["DateOfJoining"].ToString().Equals(Constants.S_DEFAULT_DATE_4) && !row["DateOfJoining"].ToString().Equals(Constants.S_DEFAULT_DATE_3) )
				newRow["Joining Date"] = DateTime.Parse(row["DateOfJoining"].ToString()).ToString("dd-MMM-yyyy");
            if (!row["DateOfPermanent"].ToString().Equals(Constants.S_DEFAULT_DATE_4) && !row["DateOfJoining"].ToString().Equals(Constants.S_DEFAULT_DATE_3))
                newRow["Permanent Date"] = DateTime.Parse(row["DateOfPermanent"].ToString()).ToString("dd-MMM-yyyy");
            if (!row["DateOfResign"].ToString().Equals(Constants.S_DEFAULT_DATE_4) && !row["DateOfJoining"].ToString().Equals(Constants.S_DEFAULT_DATE_3) )
                newRow["Resignation Date"] = DateTime.Parse(row["DateOfResign"].ToString()).ToString("dd-MMM-yyyy");
            

			oExportTable.Rows.Add(newRow);
		}

        return oExportTable;
	}

    /// <summary>
    /// Validates that every checked row selected for save/update has a staff group assigned.
    /// </summary>
    private bool ValidateStaffGroupSelection(List<string> lstUserStaffgroupIds, out string asErrorMessage)
    {
        asErrorMessage = string.Empty;
        List<string> lstUsersWithoutStaffGroup = new List<string>();

        for (int iRowCount = 0; iRowCount <= lstvwAssociation.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwAssociation.Items[iRowCount];
            int iRowId = oCurrentItem.DisplayIndex;

            if (!lstUserStaffgroupIds.Contains(iRowId.ToString()))
                continue;

            CheckBox oChkSelect = (CheckBox)oCurrentItem.FindControl("ChkSelect");
            if (!oChkSelect.Checked)
                continue;

            DropDownList cmbStaffGroups = (DropDownList)oCurrentItem.FindControl("cmbStaffGroups");
            int iStaffGroupId = GetStaffGroupIdForSave(iRowCount, cmbStaffGroups, true);

            if (iStaffGroupId == Constants.I_ZERO)
            {
                LinkButton lblStaffName = (LinkButton)oCurrentItem.FindControl("lblStaffName");
                lstUsersWithoutStaffGroup.Add(lblStaffName.Text.Trim());
            }
        }

        if (lstUsersWithoutStaffGroup.Count > 0)
        {
            asErrorMessage = "Staff group should be assigned to user(s) : " + string.Join(", ", lstUsersWithoutStaffGroup.ToArray()) + ".";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Resolves staff group id from dropdown; uses data key when dropdown is disabled and did not post back a value.
    /// </summary>
    private int GetStaffGroupIdForSave(int aiRowCount, DropDownList acmbStaffGroups, bool abIsCheckedForSave)
    {
        int iStaffGroupId = Constants.I_ZERO;

        if (!string.IsNullOrEmpty(acmbStaffGroups.SelectedValue))
            iStaffGroupId = Convert.ToInt32(acmbStaffGroups.SelectedValue);

        if (iStaffGroupId == Constants.I_ZERO && abIsCheckedForSave
            && lstvwAssociation.DataKeys[aiRowCount]["StaffGroupId"] != null
            && lstvwAssociation.DataKeys[aiRowCount]["StaffGroupId"] != DBNull.Value)
        {
            int iDataKeyStaffGroupId = Convert.ToInt32(lstvwAssociation.DataKeys[aiRowCount]["StaffGroupId"]);
            if (iDataKeyStaffGroupId != Constants.I_ZERO && !acmbStaffGroups.Enabled)
                iStaffGroupId = iDataKeyStaffGroupId;
        }

        return iStaffGroupId;
    }
    
    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwAssociation.FindControl("DtPgDropDown") as DataPager;
		if (oDataPager != null)
		 {
			DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
			if (ddlCnt != null)
			{
					ddlCnt.Attributes.Add("onchange", "if(!MessageAboutDate('" + ddlCnt.ClientID + "')){return false;}");
			}
		}
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        cmbUserRoles.Focus();
        BtnSave.Visible = false;
        btnExport.Visible = false;
        btnCancel.Text = "Back";
        trNoRecordMsg.Visible = false;
        trPagerUserStaffGrAss.Visible = false;
        tblNote.Visible = false;
        var oForm = (HtmlForm)this.Master.FindControl("Form1");
        oForm.DefaultButton = btnSearch.UniqueID;

        if(Settings.FilterWithSalutation)
            RdbWithSalutation.Checked = true;        
        else
            RdbWithoutSalutation.Checked = true;
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.UsersStaffGroupsAssociation);
        if (!sLinks.Equals(string.Empty))
        {
            divErr.InnerHtml = sLinks;
            HideControls();
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to hide controls when either staff groups or other staff are not configured.
    /// </summary>
    private void HideControls()
    {
        trRole.Visible = false;
        trlistview.Visible = false;
        BtnSave.Visible = false;
        btnExport.Visible = false;
        btnCancel.Text = "Back";
    }

    /// <summary>
    /// This method is used to fill user role combobox.
    /// </summary>
    private void FillUserRoleCombobox()
    {   
        DataSet oDataSet = moUsersStaffGroupsAssociationBL.GetStaffGroupsAndRoles(miSchoolId);
        ControlUtility.FillDropDownList(oDataSet.Tables[I_USER_ROLE_TABLE], ref cmbUserRoles, "User_Role_Id", "User_Role_Name", Constants.S_SELECT);
        if (oDataSet != null && oDataSet.Tables.Count > 0)
		{
            ViewState.Add(S_STAFF_GROUPS, oDataSet.Tables[I_STAFF_GROUP_TABLE]);
			
			// Serialize mapping between StaffGroupId & it's OriginalStaffGroupId in json format.
			mdicUserStaffGroupMap = new Dictionary<string,string>();
			foreach (DataRow row in oDataSet.Tables[I_STAFF_GROUP_TABLE].Rows)
				mdicUserStaffGroupMap.Add(row["StaffGroupsId"].ToString(), row["OriginalStaffGroupsId"].ToString());
			var jsSerializer = new JavaScriptSerializer();
			hidUserStaffGroupIdMap.Value = jsSerializer.Serialize(mdicUserStaffGroupMap);

			SetStaffGroupIds();
		}
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        //BtnSave.Attributes.Add("onclick", "if(!CheckSelectedUsers(this)) return false;");
        BtnSave.Attributes.Add("onclick", "if(!ConfirmationMsg()) return false;");
    	ApplyMouseHoverEffect( new List<Button>{ btnExport, BtnSave, btnCancel, btnSearch });
        SetDefaultButton(btnSearch);
    	btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
        lnkCarryForword.Attributes.Add("onclick", "if(!OpenCarryForwordUserPopup()) return false;");
    }

    /// <summary>
    /// This method is used to fill user grid.
    /// </summary>
    private void FillUserDetails()
    {   
        int iUserRoleId = Convert.ToInt32(cmbUserRoles.SelectedValue);
        string sUserName = txtUserName.Text.Trim();

        UsersStaffGroupsAssociationBL oUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL(miSchoolId, miAcademicYearId);

        bool bWithSalutation = false;
        if (RdbWithSalutation.Checked)
            bWithSalutation = true;

        oDTUserTable = oUsersStaffGroupsAssociationBL.GetUserDetails(iUserRoleId, sUserName, ddlUserType.SelectedValue.ToInt(), bWithSalutation);
        if (oDTUserTable.Rows.Count > 0)
        {
            lstvwAssociation.DataSource = oDTUserTable;
            lstvwAssociation.DataBind();
            ControlUtility.FillListViewPagerFooter(lstvwAssociation, DtPgCount);
        }
        else
        {
            lstvwAssociation.DataSource = null;
            lstvwAssociation.DataBind();
            trPagerUserStaffGrAss.Visible = false;
        }

    	SetVisibility(oDTUserTable.Rows.Count > 0);
    }

    /// <summary>
    /// This method is used to set visibility according to action.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetVisibility(bool abAction)
    {
        tblNote.Visible = abAction;
		BtnSave.Visible = abAction;
        btnExport.Visible = abAction;
        trNoRecordMsg.Visible = !abAction;
        trPagerUserStaffGrAss.Visible = abAction;
        if (!abAction)
            btnCancel.Text = "Back";
        else
            btnCancel.Text = "Cancel";
    }

    /// <summary>
    /// This method is used to fill header staff group combobox.
    /// </summary>
    private void FillHeaderCombobox()
    {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwAssociation.FindControl("trHeaderContol");
        DropDownList cmbAllStaffGroups = (DropDownList)oHtmlTableRow.FindControl("cmbAllStaffGroups");
        DataTable oDTStaffGroupt = (DataTable)ViewState[S_STAFF_GROUPS];
        ControlUtility.FillDropDownList(oDTStaffGroupt, ref cmbAllStaffGroups, "StaffGroupsId", "StaffGroupsName", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set controls visibility.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    /// <param name="aoDataRowView"></param>
    /// <param name="alnkBtnConfig"></param>
    /// <param name="alnkBtnLeave"></param>
    /// <param name="aiRowId"></param>
    /// <param name="aiStaffGroupId"></param>
    private void SetControlsVisibility(ListViewDataItem aoCurrentItem, DataRowView aoDataRowView, ref LinkButton alnkBtnConfig, ref LinkButton alnkBtnLeave, ref LinkButton alnkBtnInsurance, int aiRowId, int aiStaffGroupId, int iIsLocked)
    {
        CheckBox chkSelect = (CheckBox)aoCurrentItem.FindControl("chkSelect");
        chkSelect.Attributes.Add("onclick", "VisibleControls(" + aiRowId + ")");

        DropDownList cmbStaffGroups = (DropDownList)aoCurrentItem.FindControl("cmbStaffGroups");
        DataTable oDTStaffGroupt = (DataTable)ViewState[S_STAFF_GROUPS];
        ControlUtility.FillDropDownList(oDTStaffGroupt, ref cmbStaffGroups, "StaffGroupsId", "StaffGroupsName", Constants.S_SELECT);
        int iUsersStaffGroupsAssociationId = Convert.ToInt32(aoDataRowView["UsersStaffGroupsAssociationId"]);

        TextBox txtAccountNo = (TextBox)aoCurrentItem.FindControl("txtAccountNo");
        TextBox txtProvidentFundNo = (TextBox)aoCurrentItem.FindControl("txtProvidentFundNo");
        TextBox txtUAN = (TextBox)aoCurrentItem.FindControl("txtUAN");
        TextBox txtEmployeeNo = (TextBox)aoCurrentItem.FindControl("txtEmployeeNo");
        TextBox txtPanNo = (TextBox)aoCurrentItem.FindControl("txtPanNo");
        char cIsConfigured = Convert.ToChar(aoDataRowView["IsConfigured"]);
        char cIsLeaveConfigured = Convert.ToChar(aoDataRowView["IsLeaveConfigured"]);
        int iMaritalStatus = Convert.ToInt16(aoDataRowView["MaritalStatus"]);

        ImageButton imgActiveDeacActive = (ImageButton)aoCurrentItem.FindControl("imgActiveDeactive");
        
        if (cIsConfigured == 'Y')
            alnkBtnConfig.Text = S_EDIT;
        else
            alnkBtnConfig.Text = S_ADD;

        if (cIsLeaveConfigured == 'Y')
            alnkBtnLeave.Text = S_EDIT;
        else
            alnkBtnLeave.Text = S_ADD;

        if (iMaritalStatus > 0)
            alnkBtnInsurance.Text = S_EDIT;
        else
            alnkBtnInsurance.Text = S_ADD;

        if (iUsersStaffGroupsAssociationId != 0)
        {
            if (iIsLocked == Constants.I_ZERO)
            {
                cmbStaffGroups.Enabled = true;
                txtAccountNo.Enabled = true;
                txtProvidentFundNo.Enabled = true;
                txtUAN.Enabled = true;
                txtEmployeeNo.Enabled = true;
                txtPanNo.Enabled = true;
            }
            chkSelect.Checked = true;
            imgActiveDeacActive.Visible = true;
            cmbStaffGroups.SelectedValue = aiStaffGroupId.ToString();
        }
        else
        {
            cmbStaffGroups.Enabled = false;
            txtProvidentFundNo.Enabled = false;
            txtUAN.Enabled = false;
            txtEmployeeNo.Enabled = false;
            txtAccountNo.Enabled = false;
            alnkBtnConfig.Visible = false;
            alnkBtnLeave.Visible = false;
            alnkBtnInsurance.Visible = false;
            imgActiveDeacActive.Visible = false;
            txtPanNo.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to get encrypted query string.
    /// </summary>
    /// <param name="aiStaffGroupId"></param>
    /// <param name="aoDataRowView"></param>
    /// <returns></returns>
    private string GetQueryString(int aiStaffGroupId, DataRowView aoDataRowView, int aiUserId, char cIsLocked)
    {
        string sUserName = Convert.ToString(aoDataRowView["UserName"]);
        string sStaffGroupsName = Convert.ToString(aoDataRowView["StaffGroupName"]);

        string sQueryString = "Is_Configured=" + hidIsConfigured.Value +
                              "&UserRoleId=" + cmbUserRoles.SelectedValue +
                              "&UserId=" + aiUserId +
                              "&UserName=" + sUserName +
                              "&StaffGroupId=" + aiStaffGroupId +
                              "&StaffGroupsName=" + sStaffGroupsName +
                              "&Filter=" + txtUserName.Text.Trim()+
                              "&IsLocked=" + cIsLocked;

        return CommonUtility.EncryptQuerystring(sQueryString);
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {   
        hidIsConfigured.Value = QueryString["Is_Configured"];
        if (QueryString["Filter"] != null)
            txtUserName.Text = QueryString["Filter"];
        if (QueryString["UserRoleId"] != null)
        {
            int iUserRoleId = QueryString["UserRoleId"].ToInt();
            cmbUserRoles.SelectedValue = iUserRoleId.ToString();
            FillUserDetails();
        }        
    }

    /// <summary>
    /// This method is used to save association.
    /// </summary>
    private void Save()
    {

		bool bDisplaySaveMsg = true;        
        UsersStaffGroupsAssociationBL oUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL(miSchoolId, miAcademicYearId);        
        UsersSGAssociation oUsersSGAssociation = new UsersSGAssociation();
        oUsersStaffGroupsAssociationBL.UsersSGAssociation = oUsersSGAssociation;
        oUsersSGAssociation.SchoolId = miSchoolId;
        oUsersSGAssociation.AcademicYearId = miAcademicYearId;
        oUsersSGAssociation.InsertedById = miUserId;
		string[] sUserStaffgroupIds = hidUserStaffgroupsAssociationId.Value.Split(',');
		List<string> lstUserStaffgroupIds = new List<string>();
		foreach (string str in sUserStaffgroupIds)
		{
			if (!string.IsNullOrEmpty(str))
			{
				lstUserStaffgroupIds.Add(str);
			}
		}

        string sStaffGroupValidationError;
        if (!ValidateStaffGroupSelection(lstUserStaffgroupIds, out sStaffGroupValidationError))
        {
            trErrorMessage.Visible = true;
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = sStaffGroupValidationError;
            FillUserDetails();
            return;
        }

        oUsersSGAssociation.UserXml = GenerateUserXml(lstUserStaffgroupIds);
        int iLeaveSeperaterDay = Settings.LeaveSeperaterDay;
        DataSet oDsMessage = oUsersStaffGroupsAssociationBL.Save(iLeaveSeperaterDay);
		if (oDsMessage.Tables.Count > 0)
		{
			if (oDsMessage.Tables[0].Rows.Count > 0)			
			{
				SetDuplicationMessage(oDsMessage.Tables[0]);
				bDisplaySaveMsg = false;
			}
		}

		if (oDsMessage.Tables.Count > 1)
		{
			trErrorMessage.Visible = true;
			lblErrorMessage.Visible = true;
            lblErrorMessage.Text = lblErrorMessage.Text + Convert.ToString(oDsMessage.Tables[1].Rows[0]["ErrorMsg"]);
			FillUserDetails();
			bDisplaySaveMsg = false;		
		}		

		if (bDisplaySaveMsg)
		{
		    trErrorMessage.Visible = true;
			lblMessage.Text = S_SAVE_MESSAGE;
			FillUserDetails();
		}

		hidUserStaffgroupsAssociationId.Value = string.Empty;
    }

    /// <summary>
    /// This method is used to display duplication message.
    /// </summary>
    /// <param name="aoDTMessage"></param>
    private void SetDuplicationMessage(DataTable aoDTMessage)
    {
        string sAccMessage = string.Empty;
        string sPFMessage = string.Empty;
        string sUANMessage = string.Empty;
        string sEmpNoMessage = string.Empty;
        string sPanNoMessage = string.Empty;
        trErrorMessage.Visible = true;
        lblErrorMessage.Visible = true;

        for (int iRowcount = 0; iRowcount <= aoDTMessage.Rows.Count - 1; iRowcount++)
        {
            if (Convert.ToString(aoDTMessage.Rows[iRowcount]["TypeOfMsg"]).Trim() == StaffDetails.A.ToString())
            {
                sAccMessage = "Account No. should not be same for user(s) : " + Convert.ToString(aoDTMessage.Rows[iRowcount]["Msg"]) + "." + "<BR />";
                lblErrorMessage.Text = lblErrorMessage.Text + sAccMessage;
            }

            if (Convert.ToString(aoDTMessage.Rows[iRowcount]["TypeOfMsg"]).Trim() == StaffDetails.P.ToString())
            {
                sPFMessage = "P.F. No. should not be same for user(s) : " + Convert.ToString(aoDTMessage.Rows[iRowcount]["Msg"]) + "." + "<BR />";
                lblErrorMessage.Text = lblErrorMessage.Text + sPFMessage;
            }

            if (Convert.ToString(aoDTMessage.Rows[iRowcount]["TypeOfMsg"]).Trim() == StaffDetails.U.ToString())
            {
                sUANMessage = "UAN should not be same for user(s) : " + Convert.ToString(aoDTMessage.Rows[iRowcount]["Msg"]) + "." + "<BR />";
                lblErrorMessage.Text = lblErrorMessage.Text + sUANMessage;
            }

            if (Convert.ToString(aoDTMessage.Rows[iRowcount]["TypeOfMsg"]).Trim() == StaffDetails.E.ToString())
            {
                sEmpNoMessage = "Employee No. should not be same for user(s) : " + Convert.ToString(aoDTMessage.Rows[iRowcount]["Msg"]) + "." + "<BR />";
                lblErrorMessage.Text = lblErrorMessage.Text + sEmpNoMessage;
            }

            if (Convert.ToString(aoDTMessage.Rows[iRowcount]["TypeOfMsg"]).Trim() == StaffDetails.C.ToString())
            {
                sPanNoMessage = "Pan No. should not be same for user(s) : " + Convert.ToString(aoDTMessage.Rows[iRowcount]["Msg"]) + "." + "<BR />";
                lblErrorMessage.Text = lblErrorMessage.Text + sPanNoMessage;
            }           
        }
    }

    /// <summary>
    /// This method is used to generate xml of association details.
    /// </summary>
    /// <returns></returns>
    private string GenerateUserXml(List<string> lstUserStaffgroupIds)
    {
        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("UserGroup");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "UserGroup", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwAssociation.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwAssociation.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            int iUsersStaffGroupsAssociationId = Convert.ToInt32(lstvwAssociation.DataKeys[iRowCount]["UsersStaffGroupsAssociationId"]);

            CheckBox oChkSelect = (CheckBox)oCurrentItem.FindControl("ChkSelect");
			if ((oChkSelect.Checked || iUsersStaffGroupsAssociationId != Constants.I_ZERO) 
				&& lstUserStaffgroupIds.Contains(iRowId.ToString())
                ) 
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "UserGroup", string.Empty);

                sAttribute = "UsersStaffGroupsAssociationId";
                XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = iUsersStaffGroupsAssociationId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "UserId";
                attr = oDoc.CreateAttribute(sAttribute);
                int iUserId = Convert.ToInt32(lstvwAssociation.DataKeys[iRowCount]["User_Id"]);
                attr.Value = iUserId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "StaffGroupsId";
                attr = oDoc.CreateAttribute(sAttribute);
                DropDownList cmbStaffGroups = (DropDownList)oCurrentItem.FindControl("cmbStaffGroups");
                int iStaffGroupId = GetStaffGroupIdForSave(iRowCount, cmbStaffGroups, oChkSelect.Checked);
                attr.Value = iStaffGroupId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "EmployeeNo";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtEmployeeNo = (TextBox)oCurrentItem.FindControl("txtEmployeeNo");
                attr.Value = txtEmployeeNo.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "AccountNo";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtAccountNo = (TextBox)oCurrentItem.FindControl("txtAccountNo");
                attr.Value = txtAccountNo.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "ProvidentFundNo";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtProvidentFundNo = (TextBox)oCurrentItem.FindControl("txtProvidentFundNo");
                attr.Value = txtProvidentFundNo.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "UAN";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtUAN = (TextBox)oCurrentItem.FindControl("txtUAN");
                attr.Value = txtUAN.Text.Trim();
                oXmlNode.Attributes.Append(attr);


                sAttribute = "PanNo";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtPanNo = (TextBox)oCurrentItem.FindControl("txtPanNo");
                attr.Value = txtPanNo.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "Is_Deleted";
                attr = oDoc.CreateAttribute(sAttribute);
                if (oChkSelect.Checked)
                    attr.Value = "N";
                else
                    attr.Value = "Y";
                oXmlNode.Attributes.Append(attr);

                sAttribute = "DateOfJoining";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtDateOfJoining = (TextBox)oCurrentItem.FindControl("txtDateOfJoining");               
                attr.Value = txtDateOfJoining.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "DateOfPermanent";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtDateOfPermanent = (TextBox)oCurrentItem.FindControl("txtDateOfPermanent");
                attr.Value = !txtDateOfPermanent.Enabled ? string.Empty : txtDateOfPermanent.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "DateOfResign";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtDateOfResign = (TextBox)oCurrentItem.FindControl("txtDateOfResign");
                attr.Value = txtDateOfResign.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "TransferDate";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtTransferDate = (TextBox)oCurrentItem.FindControl("txtTransferDate");
                attr.Value = txtTransferDate.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "UserName";
                attr = oDoc.CreateAttribute(sAttribute);
                LinkButton lblStaffName = (LinkButton)oCurrentItem.FindControl("lblStaffName");
                attr.Value = lblStaffName.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to set javascript attribute to link.
    /// </summary>
    private void SetAttribute()
    {
        string sQueryString = "IsFromStaffGroupAssociation=Y";
       string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        hlnkStaffStatus.Attributes.Add("onclick", "window.open('../Payroll/StaffStatusPopUp.aspx?" + sEncrypt +
                           "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=750,height=630'); return false;");
    }

    /// <summary>
    /// This method is used to Fill User Type Combobox.
    /// </summary>
    private void FillUserTypesCombo()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable dtUSerTypes = oMasterDataCollectionBL.GetAllUserTypes();        

        ControlUtility.FillDropDownList(dtUSerTypes, ref ddlUserType,
                                       "UserTypeId",
                                      "UserType", string.Empty);
        ddlUserType.SelectedValue = Constants.S_ONE;
    }

    #endregion

    #region Enum

    enum StaffDetails
    {
        A, // Account No
        E, // Employee No
        P, // P.F. No
        U,// UAN
        C  // Pan No
    }

    #endregion



   
}
