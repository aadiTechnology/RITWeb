/* ---------------------------------------------------------------------
 *	FileName	: GroupMasterUI.cs
 *	Author		: Rohini V. Ghule
 *	Date		: 5-Oct-2011
 *	Description : This class is used to add, edit and remove the groups
 * ---------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

public partial class GroupMasterUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_UPDATE = "Update";
	private const string S_SAVE = "Save";
	private const string S_DEFAULT_SORT_EXP = "Name";
	private const string S_SAVE_MESSAGE = "Group is saved successfully!!!";
	private const string S_UPDATE_MESSAGE = "Group is updated successfully!!!";
	private const string S_DELETE_MESSAGE = "Group is deleted successfully!!!";
	private const string S_SORT = "SORT_ROW";
	private const string S_GROUP_NAME = "%GROUPNAME%";

	#endregion  -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private AccountGroupClient moAccountGroupClient;

	#endregion -- MEMBER(s) --

	#region -- EVENT(s) --

	/// <summary>
	/// This event is used to initialise the controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				InitializeServiceObj();
				InitializeFields();
				FillGroupNatureCombo();
				FillGroupsListVeiw();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObj();
		}
	}

	/// <summary>
	/// This event is used to add the sort image for the Ledger list.
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
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save group details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			InitializeServiceObj();
			Group oGroup = PopulateGroupDetails();
			string sErrorMessage = moAccountGroupClient.SaveGroup(oGroup);
			if (String.IsNullOrEmpty(sErrorMessage))
			{
				if (!IsConfigured())
					SaveConfigDetails(Constants.SchoolConfigurations.Groups.ToInt());
				FillGroupsListVeiw();
                ClearFields();
				lblUpdateSucess.Text = btnSave.Text == S_SAVE ? S_SAVE_MESSAGE : S_UPDATE_MESSAGE;
				lblUpdateSucess.Visible = true;
                btnSave.Text = S_SAVE;
			}
			else
			{
				lblError.Text = sErrorMessage;
				lblError.Visible = true;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObj();
		}
	}

	/// <summary>
	/// This event is used to update or remove the groups. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwGroupDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		string sGroupName = string.Empty;
		try
		{
			InitializeServiceObj();
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iRowIndex = oCurrentItem.DisplayIndex;
				hidRowNo.Value = iRowIndex.ToString();
				switch (e.CommandName)
				{
				    case Constants.S_COMMAND_UPDATE:
				        Update(iRowIndex, e);
				        break;
				    case Constants.S_COMMAND_REMOVE:
				        {
				            sGroupName = (oCurrentItem.FindControl("lblGroupName") as Label).Text;
				            int iGroupId = lstvwGroupDetails.DataKeys[iRowIndex]["Id"].ToInt();
				            Delete(iGroupId);
				            FillGroupsListVeiw();
				            ClearFields();
				            lblError.Text = string.Empty;
				            lblUpdateSucess.Text = S_DELETE_MESSAGE;
				            btnSave.Text = S_SAVE;
				        }
				        break;
				}
			}
			else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT)
			{
				if (hidSortExpression.Value != e.CommandArgument.ToString())
					hidSortDirection.Value = Constants.S_DESCENDING;
				SetSortVariables();

				hidSortExpression.Value = e.CommandArgument.ToString();
				lstvwGroupDetails.DataSourceID = objdsGroupList.ID;

				var oDtPgDropDown = lstvwGroupDetails.FindControl("DtPgDropDown") as DataPager;
				if (oDtPgDropDown != null)
					oDtPgDropDown.SetPageProperties(0, oDtPgDropDown.PageSize, true);
			}
		}
		catch (FaultException<SchoolBusinessService.DependencyException> ex)
		{
			lblError.Text = ex.Detail.ErrorMessage.Replace(S_GROUP_NAME, sGroupName);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObj();
		}
	}

	/// <summary>
	/// This event is used to set values to listview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwGroupDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;

				// Set a diff class for alternate rows
				if (oCurrentItem.DisplayIndex % 2 == 1)
				{
					var oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
					oHTMLCurrentRow.Attributes.Add("class", "ClsGridAltRow");
				}

				int iRowId = oCurrentItem.DisplayIndex;
				var imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
				imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");
				var imgBtnEdit = oCurrentItem.FindControl("imgBtnEdit") as ImageButton;
				var imgIsPrimary = oCurrentItem.FindControl("imgIsPrimary") as HtmlControl;

				imgBtnDelete.Visible = !lstvwGroupDetails.DataKeys[iRowId]["IsSystemDefined"].ToBool();
				imgBtnEdit.Visible   = imgBtnDelete.Visible;
				imgIsPrimary.Visible = lstvwGroupDetails.DataKeys[iRowId]["IsPrimary"].ToBool();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwGroupDetails);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to initialize the DataPager control of the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwGroupDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwGroupDetails.Items.Count > 0)
			{
				// Initialize the DataPager control
				var oDtPgCount = lstvwGroupDetails.FindControl("DtPgCount") as DataPager;
				ControlUtility.FillListViewPagerFooter(lstvwGroupDetails, oDtPgCount);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to cancel operation.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ClearFields();
            btnSave.Text = S_SAVE;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method is used to set javascript attribute.
	/// </summary>
	private void InitializeFields()
	{
		hidMode.Value = Constants.S_NEW_MODE;
		hidSortDirection.Value = Constants.S_ASCENDING;
		hidRowNo.Value = "-999";
		txtGroupName.Focus();

		btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Accounts_Related));
		ApplyMouseHoverEffect(new List<Button> { btnBack, btnCancel, btnSave });
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		cmbParentGroup.Attributes["onchange"] = "javascript:ParentGroupOnChange(this)";
		cmbParentGroup.Attributes["onClick"] = "this.oldIndex = this.selectedIndex";
	}

	/// <summary>
	/// This method is used fill Group nature combobox.
	/// </summary>
	private void FillGroupNatureCombo()
	{
		List<GroupNature> lstGroupNature = moAccountGroupClient.GetAllNatures(miSchoolId);
		ListSource.FillDropDownList(lstGroupNature, cmbGroupNature, "Name", "Id", Constants.S_SELECT);
	}

	/// <summary>
	/// This mrthod is used to fill listview.
	/// </summary>
	private void FillGroupsListVeiw()
	{
		lstvwGroupDetails.DataSourceID = objdsGroupList.ID;

		List<Group> lstGroups = moAccountGroupClient.GetAllGroups(miSchoolId, miFinancialYearId);
		ListSource.FillDropDownList(lstGroups, cmbParentGroup, "Name", "Id", "Primary Group");

		var obj = new Dictionary<string, object>();
		lstGroups.ForEach(group => obj.Add(group.Id.ToString(), new { group.GroupNature.Id }));

		var jsSerializer = new JavaScriptSerializer();
		hidGroupsJSON.Value = jsSerializer.Serialize(obj);
	}

	/// <summary>
	/// This method is used to add sort image.
	/// </summary>
	private void AddSortImage()
	{
		if (string.IsNullOrEmpty(hidSortExpression.Value))
			hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		var oHtmlTableHeaderRow = lstvwGroupDetails.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
	}

	/// <summary>
	/// This method is used to set sort direction.
	/// </summary>
	private void SetSortVariables()
	{
	    hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

    /// <summary>
	/// This method is used to populate group deatails.
	/// </summary>
	/// <returns></returns>
	private Group PopulateGroupDetails()
	{
        int iGroupId = 0;
		if (hidMode.Value == Constants.S_EDIT_MODE)
			iGroupId = hidGroupId.Value.ToInt();

		return new Group
		        {
		            Id							= iGroupId,
					ParentGroup					= new Group { Id = cmbParentGroup.SelectedValue.ToInt() },
		            Name						= txtGroupName.Text.Trim(),
		            IsPrimary					= cmbParentGroup.SelectedValue.ToInt() == Constants.I_ZERO,
		            GroupNature					= new GroupNature { Id = cmbGroupNature.SelectedValue.ToInt() },
		            IsConsideredForTrialBalance = chkTrialBalance.Checked,
                    IsPANDetailsRequired        = chkIsPanRequired.Checked,
		            SchoolId					= miSchoolId,
		            FinancialYearId				= miFinancialYearId,
		            InsertedById				= miUserId
		        };
	}

	/// <summary>
	/// This method is  used to delete group.
	/// </summary>
	/// <param name="aiGroupId"></param>
	private void Delete(int aiGroupId)
	{
		string sErrorMessage = moAccountGroupClient.DeleteGroup(miSchoolId, aiGroupId, miUserId);
		if (!string.IsNullOrEmpty(sErrorMessage))
			throw new FaultException<SchoolBusinessService.DependencyException>(new SchoolBusinessService.DependencyException { ErrorMessage = sErrorMessage });
		if (moAccountGroupClient.IsAtLeastOneGroupExist(miSchoolId, miFinancialYearId))
			DeleteConfigDetails(Constants.SchoolConfigurations.Groups.ToInt());
	}

	/// <summary>
	///		Determines from the QueryString if screen is configured.
	/// </summary>
	/// <returns></returns>
	private bool IsConfigured()
	{
		return !QueryString[Constants.S_IS_CONFIGURED].IsNull() && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
	}

	/// <summary>
	/// This method is used to set values to controls.
	/// </summary>
	/// <param name="aiRowIndex"></param>
	/// <param name="e"> </param>
	private void Update(int aiRowIndex, ListViewCommandEventArgs e)
	{
		btnSave.Text = S_UPDATE;
		lblUpdateSucess.Text = String.Empty;
		var lblGroupName = e.Item.FindControl("lblGroupName") as Label;
		txtGroupName.Text = lblGroupName.Text;
		hidGroupId.Value = lstvwGroupDetails.DataKeys[aiRowIndex]["Id"].ToString();
		var oGroup = lstvwGroupDetails.DataKeys[aiRowIndex]["ParentGroup"] as Group;
		cmbParentGroup.SelectedValue = oGroup.Id.ToString();
		chkTrialBalance.Checked = lstvwGroupDetails.DataKeys[aiRowIndex]["IsConsideredForTrialBalance"].ToBool();
        chkIsPanRequired.Checked = lstvwGroupDetails.DataKeys[aiRowIndex]["IsPANDetailsRequired"].ToBool();
		hidMode.Value = Constants.S_EDIT_MODE;
		lblError.Text = String.Empty;
		var oGroupNature = lstvwGroupDetails.DataKeys[aiRowIndex]["GroupNature"] as GroupNature;
		cmbGroupNature.SelectedValue = oGroupNature.Id.ToString();
		cmbGroupNature.Enabled = cmbParentGroup.SelectedValue == Constants.S_ZERO;
		spanError.Style["display"] = cmbGroupNature.Enabled ? String.Empty : "none";
	}

	/// <summary>
	/// This method is used to clear the fields.
	/// </summary>
	private void ClearFields()
	{
		lblError.Text = String.Empty;
		lblUpdateSucess.Text = String.Empty;
		txtGroupName.Text = String.Empty;
		cmbGroupNature.ClearSelection();
		cmbParentGroup.ClearSelection();
		chkTrialBalance.Checked = false;
	    chkIsPanRequired.Checked = false;
		cmbGroupNature.Enabled = true;
		spanError.Style["display"] = String.Empty;
		hidGroupId.Value = String.Empty;
		hidMode.Value = Constants.S_NEW_MODE;
		hidRowNo.Value = "-999";
	}

	/// <summary>
	/// This method is used to initialize service object.
	/// </summary>
	private void InitializeServiceObj()
	{
		moAccountGroupClient = new AccountGroupClient();
		moAccountGroupClient.Open();
	}

	/// <summary>
	/// Disposes off the Account Group client service object.
	/// </summary>
	private void CloseServiceObj()
	{
		if (moAccountGroupClient != null && moAccountGroupClient.State != CommunicationState.Faulted)
			moAccountGroupClient.Close();
	}

	#endregion -- PRIVATE METHOD(s) --

}