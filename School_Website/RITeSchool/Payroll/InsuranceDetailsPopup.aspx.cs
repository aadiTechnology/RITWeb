// File Name   : InsuranceDetailsPopup.aspx.cs
// Created By  : Deepak
// Date        : 1 may 2011
// Description : This class is used to save insurance related details for the users.       

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

/// <summary>
/// 	This class is used to save users insurance details.
/// </summary>
public partial class InsuranceDetailsPopup : SchoolBase
{
	#region Constants

	private const string S_SAVE_MESSAGE = "Insurance details are saved successfully !!!";
	private const string S_SAVE_DEPENDENT_MESSAGE = "Dependant details are saved successfully !!!";
	private const string S_DELETE_DEPENDENT_MESSAGE = "Dependant details are deleted successfully !!!";
	private const string S_UPDATE_DEPENDENT_MESSAGE = "Dependant details are updated successfully !!!";
	private const string S_REMOVE = "REMOVE";
	private const string S_UPDATE_DEPENDENT = "UPDATE_DEPENDENT";
	private const string S_SORT_COMMAND = "Sort";

	private enum MaritalStatus
	{
		MARRIED = 1,
		UNMARRIED,
		WIDOW
	}

    private UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL;

	#endregion

	#region Event

	/// <summary>
	/// 	This event is used to read query string and set the control's attributes.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{            
            moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
			if (!IsPostBack)
			{
				ReadQuerystring();
				IntializeControls();
				if (!LoadInsuranceDetails())
				{
					optUnmarried.Checked = true;
					hidMode.Value = Constants.S_NEW_MODE;
					hidAddDependentMode.Value = Constants.S_NEW_MODE;
				}

				FillSalutationComboBox();
				hidMode.Value = Constants.S_NEW_MODE;
			}
			
			AddRemoveDeleteConfirmation();
			txtInsuranceCardNumSelf.Focus();
			lblUserName.Text = hidUserName.Value;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to set a screen control for marrial status married.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void optMarried_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			lblUpdate.Text = string.Empty;
			lblUpdate.Visible = false;
			ClearDependentControls();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to set a screen control for marrial status unmarried.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void optUnmarried_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			lblUpdate.Text = string.Empty;
			lblUpdate.Visible = false;
			ClearDependentControls();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to set a screen control for marrial status widow.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void optWidow_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			lblUpdate.Text = string.Empty;
			lblUpdate.Visible = false;
			ClearDependentControls();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to save insuarance amount and/or spouse details of the users.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			lblUpdate.Text = string.Empty;
			lblUpdate.Visible = false;
			Save();
			lblUpdate.Text = S_SAVE_MESSAGE;
			lblUpdate.Visible = true;
			tblDependent.Visible = true;
			LoadInsuranceDetails();
			AddRemoveDeleteConfirmation();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to add attribute listviews item control.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwDependentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
				oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                if (hidIsLocked.Value == Constants.S_YES)
                {
                    var oimgBtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                    oimgbtnDelete.Enabled = false;
                    oimgBtnEdit.Enabled = false;
                }

			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to delete and/or clear insuarance amount and/or spouse and/or child details of the users.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			if (btnCancel.Text == Constants.Action.Delete.ToString())
			{
				DeleteDependentDetails();
				RemoveOldInsuranceDetails();
				ClearDependentControls();
				txtInsuranceAmount.Text = string.Empty;
                txtInsuranceCardNumSelf.Text = string.Empty;
				btnCancel.Text = "Cancel";
				FillDependentGrid();
				tblDependent.Visible = false;
			}
			else
			{
				ClearDependentControls();
				txtInsuranceAmount.Text = string.Empty;
			}

			AddRemoveDeleteConfirmation();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used save child details.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnSaveDependent_Click(object sender, EventArgs e)
	{
		try
		{
			lblUpdate.Text = string.Empty;
			lblUpdate.Visible = false;
		
			switch (hidAddDependentMode.Value)
			{
				case Constants.S_NEW_MODE:
					InsertDependentDetails();
					break;
				case Constants.S_EDIT_MODE:
					UpdateDependentDetails(hidUsersInsuranceDependentId.Value.ToInt());
					break;
			}

			FillDependentGrid();
			ClearDependentControls();
			hidAddDependentMode.Value = Constants.S_NEW_MODE;
            hidMode.Value = Constants.S_NEW_MODE;
			hidUsersInsuranceDependentId.Value = String.Empty;
		}
		catch (DuplicateUserException duEx)
		{
			lblUpdate.Visible = false;
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = duEx.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to edit and delete the child details.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwDependentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName != S_SORT_COMMAND)
			{
				var ocurrentItem = (ListViewDataItem)e.Item;
				int iListIndex = ocurrentItem.DisplayIndex;
				int iUsersInsuranceDependentId = lstvwDependentDetails.DataKeys[iListIndex]["UsersInsuranceDependentId"].ToInt();
				hidUsersInsuranceDependentId.Value = iUsersInsuranceDependentId.ToString();				
				switch (e.CommandName)
				{
					case S_REMOVE:
						DeleteDependent(iUsersInsuranceDependentId, miSchoolId);
						break;
					case S_UPDATE_DEPENDENT:
						FillControlsForDependentUpdate(iUsersInsuranceDependentId, miSchoolId);
						break;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to clear controls of child details section.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnCancelDependent_Click(object sender, EventArgs e)
	{
		try
		{
			ClearDependentControls();
			hidAddDependentMode.Value = Constants.S_NEW_MODE;
			hidUsersInsuranceDependentId.Value = String.Empty;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to show or hide the dependant details section.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwDependentDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			tblDependent.Visible = lstvwDependentDetails.Items.Count > 0;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// 	This method is used to save insurance details.
	/// </summary>
	private void Save()
	{
		if (hidMode.Value == Constants.S_EDIT_MODE)
			RemoveOldInsuranceDetails();

		if (optMarried.Checked)
			SaveMarriedPersonDetails();
		else if (optUnmarried.Checked)
			SaveUnmarriedPersonDetails();
		else if (optWidow.Checked)
			SaveWidowPersonDetails();
	}

	/// <summary>
	/// 	This method is used to read query string.
	/// </summary>
	/// <returns> </returns>
	private void ReadQuerystring()
	{ 
        hidUserId.Value = QueryString["UserId"];
        hidUserName.Value = QueryString["UserName"];
        hidStaffGroupId.Value = QueryString["StaffGroupId"];        
        hidStaffGroupsName.Value = QueryString["StaffGroupsName"];        
        hidUserRoleId.Value = QueryString["UserRoleId"];
        hidFilter.Value = QueryString["Filter"];
        if (QueryString["IsLocked"] != null && QueryString["IsLocked"] == Constants.S_YES)        
        {
            btnSave.Enabled = false;
            btnSaveDependent.Enabled = false;
            hidIsLocked.Value = QueryString["IsLocked"];
            btnCancel.Enabled = false;
        }
	}

	/// <summary>
	/// 	This method used to save details of user whose status is widow.
	/// </summary>
	private void SaveWidowPersonDetails()
	{
        SaveInsuranceDetails(txtInsuranceAmount.Text.Trim().ToDecimal(), MaritalStatus.WIDOW, txtInsuranceCardNumSelf.Text.Trim());
		if (hidMode.Value == Constants.S_NEW_MODE)
			btnCancel.Text = Constants.Action.Delete.ToString();
	}

	/// <summary>
	/// 	This method used to save details of user whose status is unmarried.
	/// </summary>
	private void SaveUnmarriedPersonDetails()
	{
		RemoveOldInsuranceDetails();
        SaveInsuranceDetails(txtInsuranceAmount.Text.Trim().ToDecimal(), MaritalStatus.UNMARRIED, txtInsuranceCardNumSelf.Text.Trim());
		if (hidMode.Value == Constants.S_NEW_MODE)
			btnCancel.Text = Constants.Action.Delete.ToString();
		hidRowCount.Value = Constants.I_ZERO.ToString();
	}

	/// <summary>
	/// 	This method used to save details of user whose status is married.
	/// </summary>
	private void SaveMarriedPersonDetails()
	{
        SaveInsuranceDetails(txtInsuranceAmount.Text.Trim().ToDecimal(), MaritalStatus.MARRIED, txtInsuranceCardNumSelf.Text.Trim());
		if (hidMode.Value == Constants.S_NEW_MODE)
			btnCancel.Text = Constants.Action.Delete.ToString();
	}

	/// <summary>
	/// 	this method is used to delete all dependant details of the user.
	/// </summary>
	private void DeleteDependentDetails()
	{
        moUsersStaffGroupsAssociationBL.DeleteDependentDetails(hidUserId.Value.ToInt(), miSchoolId, miUserId);
	}

	/// <summary>
	/// 	This methos is used to clear the controls of child details section.
	/// </summary>
	private void ClearDependentControls()
	{
		txtDependentName.Text = string.Empty;
		txtDependentMiddleName.Text = string.Empty;
		txtDependentLastName.Text = string.Empty;
		txtDependentdDOB.Text = string.Empty;
		txtRelation.Text = string.Empty;
		txtInsuranceCardNum.Text = string.Empty;
		FillSalutationComboBox();
	}

	/// <summary>
	/// 	This methos is used to delete insurance amount and marrial status of the user.
	/// </summary>
	private void RemoveOldInsuranceDetails()
	{
        moUsersStaffGroupsAssociationBL.RemoveOldInsuranceDetails(hidUserId.Value.ToInt(), miSchoolId, miUserId);
	}

	/// <summary>
	/// 	This methos is used to save insurance amount and marrial status of the user.
	/// </summary>
	private void SaveInsuranceDetails(decimal aiAmount, MaritalStatus amaritalStatus,string asInsuranceCardNumber)
	{
		moUsersStaffGroupsAssociationBL.SaveInsuranceDetails(aiAmount, (int)amaritalStatus, hidUserId.Value.ToInt(),asInsuranceCardNumber,miUserId);
	}

	/// <summary>
	/// 	This methos is used to set controls from the dependant section in edit mode.
	/// </summary>
	private void FillControlsForDependentUpdate(int aiUsersInsuranceDependentId, int aiSchoolId)
	{
		UsersInsuranceDependent oUsersInsuranceDependent = moUsersStaffGroupsAssociationBL.GetDependent(aiUsersInsuranceDependentId, aiSchoolId);
		if (oUsersInsuranceDependent == null || oUsersInsuranceDependent.UsersInsuranceDependentId == 0)
			return;
		
		FillSalutationComboBox();

		cmbSalutation.SelectedValue = Convert.ToString(oUsersInsuranceDependent.SalutationId);
		txtDependentName.Text = oUsersInsuranceDependent.FirstName.Trim();
		txtDependentMiddleName.Text = oUsersInsuranceDependent.MiddleName.Trim();
		txtDependentLastName.Text = oUsersInsuranceDependent.LastName.Trim();
		txtInsuranceCardNum.Text = oUsersInsuranceDependent.InsuranceCardNumber.Trim();
		txtRelation.Text = oUsersInsuranceDependent.Relation.Trim();
		txtDependentdDOB.Text = oUsersInsuranceDependent.DateOfBirth.ToString("dd-MMM-yyyy");
		hidAddDependentMode.Value = Constants.S_EDIT_MODE;
	}

	/// <summary>
	/// 	This methos is used to delete a dependant details.
	/// </summary>
	private void DeleteDependent(int aiUsersInsuranceDependentId, int aiSchoolID)
	{
		moUsersStaffGroupsAssociationBL.DeleteDependent(aiUsersInsuranceDependentId, aiSchoolID, miUserId);
		lblUpdate.Text = S_DELETE_DEPENDENT_MESSAGE;
		lblUpdate.Visible = true;
		FillDependentGrid();
		hidAddDependentMode.Value = Constants.S_NEW_MODE;
		ClearDependentControls();
	}

	/// <summary>
	/// 	This method is used to fill the children's grid of for the user.
	/// </summary>
	private void FillDependentGrid()
	{
		List<UsersInsuranceDependent> oLstUsersInsuranceDependent = moUsersStaffGroupsAssociationBL.GetUserDependentDetails(hidUserId.Value.ToInt(), miSchoolId);
		lstvwDependentDetails.DataSource = oLstUsersInsuranceDependent;
		hidRowCount.Value = oLstUsersInsuranceDependent.Count.ToString();
		lstvwDependentDetails.DataBind();
		hidAddDependentMode.Value = Constants.S_NEW_MODE;
		tblDependent.Visible = true;
	}

	/// <summary>
	/// 	This method is used to update the child details.
	/// </summary>
	private void UpdateDependentDetails(int aiUsersInsuranceDependentId)
	{
		UsersInsuranceDependent oUsersInsuranceDependent = PopulateDependentDetails();
		moUsersStaffGroupsAssociationBL.UpdateDependentDetails(oUsersInsuranceDependent, miUserId, aiUsersInsuranceDependentId);
		lblUpdate.Text = S_UPDATE_DEPENDENT_MESSAGE;
		lblUpdate.Visible = true;
	}

	/// <summary>
	/// 	This method is used to add the child details.
	/// </summary>
	private void InsertDependentDetails()
	{
		UsersInsuranceDependent oUsersInsuranceDependent = PopulateDependentDetails();
		moUsersStaffGroupsAssociationBL.InsertDependentDetails(oUsersInsuranceDependent, hidUserId.Value.ToInt(), miSchoolId, miUserId);
		lblUpdate.Text = S_SAVE_DEPENDENT_MESSAGE;
		lblUpdate.Visible = true;
	}

	/// <summary>
	/// 	This method populates the child details.
	/// </summary>
	private UsersInsuranceDependent PopulateDependentDetails()
	{
		return new UsersInsuranceDependent
				{
					SalutationId = cmbSalutation.SelectedValue.ToInt(),
					FirstName	 = txtDependentName.Text,
					MiddleName	 = txtDependentMiddleName.Text,
					LastName	 = txtDependentLastName.Text,
					InsuranceCardNumber=txtInsuranceCardNum.Text,
					DateOfBirth  = txtDependentdDOB.Text.ToDateTime(),
					Relation	 = txtRelation.Text,
					UsersInsuranceDependentId = hidUsersInsuranceDependentId.Value.IsNullOrEmpty() ? 0 : hidUsersInsuranceDependentId.Value.ToInt(),
                    UserId = hidUserId.Value.ToInt()
				};
	}

	/// <summary>
	/// 	This method is used to add or remove confirmation message for deleting the record.
	/// </summary>
	private void AddRemoveDeleteConfirmation()
	{
		if (btnCancel.Text == Constants.Action.Delete.ToString())
			btnCancel.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
		else
			btnCancel.Attributes.Remove("onclick");
	}

	/// <summary>
	/// 	This method is used to intialize the controlss.
	/// </summary>
	private void IntializeControls()
	{
		SetJavascriptAttributes();        
		valSumDependentDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		valSumInsuranceDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;       
	}

	/// <summary>
	/// 	This method is used to set client side attributes for the controls.
	/// </summary>
	private void SetJavascriptAttributes()
	{
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnCancel, btnCancelDependent, btnSave, btnSaveDependent });

		string sQueryString = string.Format("UserRoleId={0}&Is_Configured={1}&Filter={2}", hidUserRoleId.Value, hidIsConfigured.Value, hidFilter.Value);

		sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
		sQueryString = string.Format("?{0}", sQueryString);

		btnBack.Attributes.Add("onclick", string.Format("return CloseWindow('{0}');", sQueryString));
	}

	/// <summary>
	/// 	This method is used to fill salutation combobox
	/// </summary>
	private void FillSalutationComboBox()
	{
		ListSource.FillDropDownList(ViewState["Salutations"] as List<Salutations>, cmbSalutation, "SalutationName", "SalutationId", string.Empty);
	}

	/// <summary>
	/// 	If the details of the users are saved then this method opens the screen in edit mode.
	/// </summary>
	/// <returns> </returns>
	private bool LoadInsuranceDetails()
	{
		//var oUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
		moUsersStaffGroupsAssociationBL.GetUserInsuranceDetails(hidUserId.Value.ToInt(), miSchoolId);
        List<Salutations> oLstSalutations = moUsersStaffGroupsAssociationBL.SalutationsForName;
		ViewState.Add("Salutations", oLstSalutations);
        Insurance oInsurance = moUsersStaffGroupsAssociationBL.InsuranceAmountAndStatus;
		if (oInsurance.InsuranceAmount != 0)
			tblDependent.Visible = true;
        List<UsersInsuranceDependent> oUsersInsuranceDependent = moUsersStaffGroupsAssociationBL.DependentDetails;
		if (oInsurance.UserStatus != 0)
		{
			switch (oInsurance.UserStatus)
			{
				case (int)MaritalStatus.MARRIED:
					LoadMarriedPersonDetails(oUsersInsuranceDependent);
					break;

				case (int)MaritalStatus.UNMARRIED:
					LoadUnmarriedPersonDetails(oUsersInsuranceDependent);
					break;

				case (int)MaritalStatus.WIDOW:
					LoadWidowPersonDetails(oUsersInsuranceDependent);
					break;
			}

			hidAddDependentMode.Value = Constants.S_NEW_MODE;
			btnCancel.Text = Constants.Action.Delete.ToString();
			txtInsuranceAmount.Text = Convert.ToString(oInsurance.InsuranceAmount);
            txtInsuranceCardNumSelf.Text = oInsurance.InsuranceCardNumber;
			hidMode.Value = oInsurance.UserStatus != 2 ? Constants.S_EDIT_MODE : Constants.S_NEW_MODE;
			return true;
		}

		return false;
	}

	/// <summary>
	/// 	This method is used load the controls with widow persons insurance details.
	/// </summary>
	/// <param name="aolstUsersInsuranceDependent"> </param>
	private void LoadWidowPersonDetails(List<UsersInsuranceDependent> aolstUsersInsuranceDependent)
	{
		optWidow.Checked = true;
		LoadChildDetails(aolstUsersInsuranceDependent);
	}

	/// <summary>
	/// 	This method is used load the controls with unmarried persons insurance details.
	/// </summary>
	/// <param name="aolstUsersInsuranceDependent"> </param>
	private void LoadUnmarriedPersonDetails(List<UsersInsuranceDependent> aolstUsersInsuranceDependent)
	{
		optUnmarried.Checked = true;
		LoadChildDetails(aolstUsersInsuranceDependent);
	}

	/// <summary>
	/// 	This method is used load the controls with married persons insurance details.
	/// </summary>
	/// <param name="aolstUsersInsuranceDependent"> </param>
	private void LoadMarriedPersonDetails(List<UsersInsuranceDependent> aolstUsersInsuranceDependent)
	{
		optMarried.Checked = true;
		LoadChildDetails(aolstUsersInsuranceDependent);
	}

	/// <summary>
	/// 	This method is used load the controls with children details.
	/// </summary>
	/// <param name="aolstUsersInsuranceDependent"> </param>
	private void LoadChildDetails(List<UsersInsuranceDependent> aolstUsersInsuranceDependent)
	{
		if (aolstUsersInsuranceDependent != null && aolstUsersInsuranceDependent.Count > 0)
			SetChildDetailsForUpdate(aolstUsersInsuranceDependent);
	}

	private void SetChildDetailsForUpdate(List<UsersInsuranceDependent> aolstUsersInsuranceDependent)
	{
		FillSalutationComboBox();
		lstvwDependentDetails.DataSource = aolstUsersInsuranceDependent;
		hidRowCount.Value = aolstUsersInsuranceDependent.Count().ToString();
		lstvwDependentDetails.DataBind();
		hidAddDependentMode.Value = Constants.S_NEW_MODE;
	}

	#endregion
}