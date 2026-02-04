// File Name   : BankDetailsPopup.aspx.cs
// Created By  : -
// Date        : -
// Modified By : Milind
// Date        : 10 Sept 09
// Description : This class is used to display the bank name in the database and  also we can add new bank
//                name here.       

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Resources;

/// <summary>
/// This Class is used to add and edit the bank details .
/// </summary>
public partial class BankDetailsPopup : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
	#region -- CONSTANT(s) --

	private const string S_CMD_NAME_DELETE_BANK = "DELETE_BANK";
	private const string S_CMD_NAME_EDIT_BANK = "EDIT_BANK";
	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to decrypt query string and fill grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                hidSave.Value = "Save";
                if (Session[Constants.S_SESSION_LANGUAGE].ToString() != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
				txtBankName.Focus();
				Initialise();
				FillBankGrid();
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }

			SetClientScriptAttributes();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add confirmation message on delete button.
	/// And also show and hide the delete button according to bank dependency.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdBanks_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
            SchoolwiseBankMasterBL oSchoolwiseBankMasterBL;
			int iRowindex = e.Row.RowIndex;
			if (iRowindex >= 0)
			{
				var imgDelete = e.Row.Cells[2].Controls[Constants.I_ZERO] as ImageButton;
				imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
				int iCount = grdBanks.DataKeys[iRowindex]["Count"].ToInt();
				if (iCount != 0)
					imgDelete.Visible = false;

                oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
                oSchoolwiseBankMasterBL.Schoolwise_Bank_Id = grdBanks.DataKeys[iRowindex]["Schoolwise_Bank_Id"].ToInt();
                oSchoolwiseBankMasterBL.School_Id = miSchoolId;

                if (oSchoolwiseBankMasterBL.IsBankInCardPayment())
                    imgDelete.Visible = false;
                if (oSchoolwiseBankMasterBL.IsBankInCashPayment())
                    imgDelete.Visible = false;

			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to delete or edit the bank details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdBanks_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper() != "SORT")
			{
				int iRowIndex = e.CommandArgument.ToInt();
				SchoolwiseBankMasterBL oSchoolwiseBankMasterBL;
				int iBankID;
				switch (e.CommandName)
				{
					//For deleting the bank details
					case S_CMD_NAME_DELETE_BANK:
						oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
						iBankID = grdBanks.DataKeys[iRowIndex][0].ToInt();
						oSchoolwiseBankMasterBL.Schoolwise_Bank_Id = iBankID;
						oSchoolwiseBankMasterBL.School_Id = miSchoolId;
                        if (oSchoolwiseBankMasterBL.IsBankInPTchallan())
                            lblErrorMsg.Text = Resources.LocalizedResources.BankAlreadyUsedInPTChallanDetails;
                        if(lblErrorMsg.Text == string.Empty)
                        {
                            oSchoolwiseBankMasterBL.DeleteSchoolwiseBankMaster();
                            lblMessage.Visible = true;
                            lblMessage.Text = Resources.LocalizedResources.BankNameDeletedSuccessfully;
                        }

						txtBankName.Text = String.Empty;
						hidBankId.Value = String.Empty;
						FillBankGrid();
                        btnSave.Text = Resources.LocalizedResources.Save;
                        hidSave.Value = "Save";
						break;
					//For editing the bank details
					case S_CMD_NAME_EDIT_BANK:
						iBankID = grdBanks.DataKeys[iRowIndex][0].ToInt();
						oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL(iBankID);
						txtBankName.Text = oSchoolwiseBankMasterBL.Bank_Name;
						hidBankId.Value = oSchoolwiseBankMasterBL.Schoolwise_Bank_Id.ToString();
                        lblMessage.Visible = false;
                        btnSave.Text = Resources.LocalizedResources.Update;
                        hidSave.Value = "Update";
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
	/// This event is used to save,update bank details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		lblErrorMsg.Text = String.Empty;
		try
		{
			SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = SetAllFieldToBankMaster();
            if (hidBankId.Value == String.Empty)
            {
                oSchoolwiseBankMasterBL.InsertSchoolwiseBankMaster();
                lblMessage.Visible = true;
                lblMessage.Text = Resources.LocalizedResources.BankNameAddedSuccessfully;                
            }
            else  //To Update bank details
            {
                oSchoolwiseBankMasterBL.Schoolwise_Bank_Id = hidBankId.Value.ToInt();
                oSchoolwiseBankMasterBL.UpdateSchoolwiseBankMaster();
                lblMessage.Visible = true;
                lblMessage.Text = Resources.LocalizedResources.BankNameUpdatedSuccessfully;                
            }
			txtBankName.Text = String.Empty;
			hidBankId.Value = String.Empty;
			FillBankGrid();
            btnSave.Text = Resources.LocalizedResources.Save;
            hidSave.Value = "Save";
		}
		catch (DuplicateBankName)
		{
            lblErrorMsg.Text = Resources.LocalizedResources.BankNameAlreadyExists;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to clear the cotrols on the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			hidBankId.Value = String.Empty;
			txtBankName.Text = string.Empty;
            lblMessage.Visible = false;
            btnSave.Text = Resources.LocalizedResources.Save;
            hidSave.Value = "Save";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to close pop up.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			Response.Write("<Script language='Javascript'> window.close();window.opener.focus(); </Script>");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method initialises variables.
	/// </summary>
	private void Initialise()
	{
		btnSave.Attributes.Add("onclick", "if(!ClearErrorLabel()){return false;}");
		btnCancel.Attributes.Add("onclick", "if(!ClearErrorLabel()){return false;}");
		grdBanks.Attributes.Add("onclick", "if(!ClearErrorLabel()){return false;}");

	}

	/// <summary>
	/// This method is used to set client side attributes to controls.
	/// </summary>
	private void SetClientScriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnClose });
	}

	/// <summary>
	/// This method is used to set all fields of BankMaster.
	/// </summary>
	private SchoolwiseBankMasterBL SetAllFieldToBankMaster()
	{
		return new SchoolwiseBankMasterBL
				{
					School_Id = miSchoolId,
					Bank_Name = txtBankName.Text
				};
	}

	/// <summary>
	/// This method is used to fiil the bank details grid.
	/// </summary>
	private void FillBankGrid()
	{
		var oSchoolwiseBankMaster = new SchoolwiseBankMaster();
		DataTable oDt = oSchoolwiseBankMaster.GetSchoolwiseBankMasterDetails(miSchoolId);
		grdBanks.DataSource = oDt.DefaultView;
		grdBanks.DataBind();
	}
    /// <summary>
    /// This method  is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        hidAreYouSureYouWantToDeleteThisBank.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisBank;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnSave.Text = oResourceManager.GetString(hidSave.Value.Replace(" ", string.Empty));
    }
	#endregion -- PRIVATE METHOD(s) --
}
