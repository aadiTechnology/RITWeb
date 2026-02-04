using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Data;
using PayrollEntities;
using MasterEntities;
using Utility;
using System.Data.SqlClient;
/// <summary>
/// This class is used to save and update retirement notice configuration of staff. 
/// </summary>
public partial class RetirementNoticeConfigUI : SchoolBase
{

	#region Data Member(s)

	private RetirementNoticeConfigBL moRetirementNoticeConfigBL;

	#endregion

	#region Event(s)

	/// <summary>
	/// This event is used to fill retirement notice details in list view.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			hidIsConfigured.Value = QueryString["Is_Configured"];
			moRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
			cmbUserRole.Focus();
			if (!IsPostBack)
			{
				FillUserRoleCombo();
				FillRetirementNoticeConfigList();
				SetJavaScriptAttributes();
			}

		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is  used to save/update retirement notice details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RetirementNoticeConfiguration oRetirementNoticeConfig = Populate();
			moRetirementNoticeConfigBL.Save(oRetirementNoticeConfig);
			if (btnSave.Text == Constants.ButtonText.Update.ToString())
				DisplayMessage(Constants.ItemState.updated, false);
			else
			{
				DisplayMessage(Constants.ItemState.saved, false);
				if (hidIsConfigured.Value == Constants.S_NO)
				{
					SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RetirementNoticeConfiguration));
					hidIsConfigured.Value = Constants.S_YES;
				}
			}
			FillRetirementNoticeConfigList();
			ResetFields();
		}
		catch (SqlException se)
		{
			DisplayMessage(se.Message, true, tdMessage);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to go back to payroll dashboard.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related)));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to edit retirement notice configuration for particular staff.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwRetirementNotice_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			int iRetNoticeConfigId = Convert.ToInt32(lstvwRetirementNotice.DataKeys[e.Item.DisplayIndex]["Id"]);
			if (e.CommandName == Constants.S_COMMAND_UPDATE)
			{
				hidRetNoticeConfigId.Value = iRetNoticeConfigId.ToString();
				RetirementNoticeConfiguration oRetirementNoticeConfig = moRetirementNoticeConfigBL.Get(iRetNoticeConfigId);
				if (oRetirementNoticeConfig != null)
				{
					cmbUserRole.SelectedValue = oRetirementNoticeConfig.UserRole.Id.ToString();
					txtAge.Text = oRetirementNoticeConfig.RetirementAge.ToString();
					txtDays.Text = oRetirementNoticeConfig.ReminderDays.ToString();
					cmbUserRole.Enabled = false;
				}
				btnSave.Text = Constants.ButtonText.Update.ToString();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set the ListView controls set the serial no for each row of ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwRetirementNotice_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewItem oCurrentItem = (ListViewItem)e.Item;
				Label lblSrNo = (Label)oCurrentItem.FindControl("lblSrNo");
				lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();

			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to reset fields.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ResetFields();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region Method(s)

	/// <summary>
	/// This method is used to fill user role combo.
	/// </summary>
	private void FillUserRoleCombo()
	{
		// Fill the user role's combobox with all the user roles available in the system.
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		DataTable oDSStateCollection = oMasterDataCollectionBL.GetAllUserRoles();
        ControlUtility.FillDropDownList(oDSStateCollection.Select("User_Role_Id = 1 OR User_Role_Id = 2 OR User_Role_Id = 6 OR User_Role_Id = 7"), ref cmbUserRole,
										Constants.S_USER_ROLE_ID_FIELD,
										Constants.S_USER_ROLE_NAME_FIELD,
										Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to populate retirement notice config object.
	/// </summary>
	/// <returns></returns>
	private RetirementNoticeConfiguration Populate()
	{
		RetirementNoticeConfiguration oRetirementNoticeConfig = new RetirementNoticeConfiguration
		{
			Id =Convert.ToInt32(hidRetNoticeConfigId.Value),
			UserRole = new UserRoleMaster { Id = cmbUserRole.SelectedValue.ToInt() },
			RetirementAge = Convert.ToInt32(txtAge.Text),
			ReminderDays = Convert.ToInt32(txtDays.Text),
		};
		return oRetirementNoticeConfig;
	}

	/// <summary>
	/// This method is used to fill listview of retirement notices.
	/// </summary>
	private void FillRetirementNoticeConfigList()
	{
		List<RetirementNoticeConfiguration> lstRetirementNoticeConfig = moRetirementNoticeConfigBL.GetAll();
		lstvwRetirementNotice.DataSource = lstRetirementNoticeConfig;
		lstvwRetirementNotice.DataBind();
	}

	/// <summary>
	/// This method is used to set javascript attributes.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnCancel, btnBack, btnSave });
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		btnSave.Attributes.Add("Onclick", "ClearSuccessfulMessage()");
	}

	/// <summary>
	/// This method is used to reset fields.
	/// </summary>
	private void ResetFields()
	{
		hidRetNoticeConfigId.Value = Constants.S_ZERO;
		cmbUserRole.Enabled = true;
		cmbUserRole.ClearSelection();
		cmbUserRole.Focus();
		cmbUserRole.Focus();
		txtDays.Text = string.Empty;
		txtAge.Text = string.Empty;
		btnSave.Text = Constants.ButtonText.Save.ToString();

	}

	/// <summary>
	/// This method is used to display message.
	/// </summary>
	/// <param name="aoItemState"></param>
	/// <param name="abIsErrorMessage"></param>
	private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
	{
		string sMessage = "Retirement notice details has been " + aoItemState.ToString() + " successfully !!!";
		DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
	}

	#endregion

}