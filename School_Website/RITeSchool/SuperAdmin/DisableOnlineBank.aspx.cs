using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using FeeEntities;
using Utility;

public partial class DisableOnlineBank : SchoolBase
{

	#region --- Constants ---

	public const string S_DEFAULT_DATE_2 = "01/01/1900 12:00:00 AM";
	public const string S_DEFAULT_DATE_3 = "1/1/1900 12:00:00 AM"; //Constants.S_DEFAULT_DATE_4
	public const string S_DEFAULT_DATE_4 = "01-Jan-1900";
	public const string S_DEFAULT_DATE_5 = "01-Jan-1900 12:00 AM";
	public const string S_DASH = "-";
	public const string S_SAVE_MSG = "Rule to disable bank for online payment is saved successfully.";
	public const string S_UPDATE_MSG = "Rule to disable bank for online payment is updated successfully.";
	public const string S_DELETE_MSG = "Rule to disable bank for online payment is deleted successfully.";
	public const int I_ELEVEN = 11;
	public const string S_SAVE = "Save";
	public const string S_UPDATE = "Update";

	#endregion

	#region --- Events ---

	/// <summary>
	/// This event is used to Initialise session variable, set javascript attribute, fill combobox and listview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				Initialize();
				FillBankCombo();
				FillBankDetails();
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
	protected void lstvwDisabledBankDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				Label lblEndDateTime = e.Item.FindControl("lblEndDateTime") as Label;
				ImageButton imgBtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
				ImageButton imgBtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
				HtmlTableRow oHtmlTableRow = e.Item.FindControl("trListViewRow") as HtmlTableRow;

				int iRowId = e.Item.DisplayIndex;
				if ((lblEndDateTime.Text == S_DEFAULT_DATE_2 || lblEndDateTime.Text == S_DEFAULT_DATE_3 || lblEndDateTime.Text == S_DEFAULT_DATE_4 || lblEndDateTime.Text == S_DEFAULT_DATE_5))
					lblEndDateTime.Text = S_DASH;

				string sRuleState = lstvwDisabledBankDetails.DataKeys[iRowId]["RuleStatus"].ToString();

				// CURRENT active rule
				if (sRuleState == "C")
					oHtmlTableRow.Style.Add("background-color", "Pink");
				//  PAST Inactive rule
				else if (sRuleState == "P")
				{
					oHtmlTableRow.Style.Add("background-color", "Silver");
					imgBtnDelete.Visible = imgBtnEdit.Visible = false;
				}
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
	protected void lstvwDisabledBankDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwDisabledBankDetails.Items.Count > 0)
			{
				SetVisibility(true);
				ControlUtility.FillListViewPagerFooter(lstvwDisabledBankDetails, DtPgCount);
				DataPager oDataPager = lstvwDisabledBankDetails.FindControl("DtPgDropDown") as DataPager;
				int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
				hidPageNo.Value = iCurrentPage.ToString();

				/********** Do not delete - need to implement JSON*************/
				//populateBlockedBank();
				//SerializeBlocksBanks();
			}
			else
				SetVisibility(false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to edit or delete bank scedule details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwDisabledBankDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				lblErrorMsg.Text = lblSuccessMsg.Text = string.Empty;
				hidIsNewOrIsExisting.Value = Constants.S_ONE;
				SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
				HiddenField hidSchoolwiseBankId = e.Item.FindControl("hidSchoolwiseBankId") as HiddenField;
				Label lblStartDateTime = e.Item.FindControl("lblStartDateTime") as Label;
				Label lblEndDateTime = e.Item.FindControl("lblEndDateTime") as Label;
				int iRowId = Convert.ToInt32(e.Item.DisplayIndex);
				hidRowId.Value = iRowId.ToString();
				int iDisabledBankId = Convert.ToInt32(lstvwDisabledBankDetails.DataKeys[iRowId]["DisabledBankId"]);

				if (e.CommandName == Constants.S_COMMAND_REMOVE)
				{
					if (Convert.ToInt32(hidPageNo.Value) > Constants.I_ONE && lstvwDisabledBankDetails.Items.Count == Constants.I_ONE)
						SetDataPagerValue();

					oSchoolwiseBankMasterBL.DeleteDisableBankDetails(iDisabledBankId, miSchoolId, miUserId);
					SetDefaultValues();
					FillBankDetails();
					hidIsNewOrIsExisting.Value = Constants.S_ZERO;
					lblSuccessMsg.Text = S_DELETE_MSG;
				}
				else
				{
					btnSave.Text = S_UPDATE;
					ddlBankName.SelectedValue = hidSchoolwiseBankId.Value;
					txtStartDate.Text = (lblStartDateTime.Text).Substring(Constants.I_ZERO, I_ELEVEN);
					txtStartTime.Text = Convert.ToDateTime(lblStartDateTime.Text).ToString("hh:mm tt");
					if (lblEndDateTime.Text == S_DASH)
						txtEndDate.Text = txtEndTime.Text = string.Empty;
					else
					{
						txtEndDate.Text = (lblEndDateTime.Text).Substring(Constants.I_ZERO, I_ELEVEN);
						txtEndTime.Text = Convert.ToDateTime(lblEndDateTime.Text).ToString("hh:mm tt");
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
	/// This event is used to set page count.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwDisabledBankDetails);
			DataPager dtPager = lstvwDisabledBankDetails.FindControl("DtPgDropDown") as DataPager;
			DropDownList ddlCnt = (dtPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
			hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
			FillBankDetails();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to clear error message and set all the controls to its default value.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			SetDefaultValues();
			lblErrorMsg.Text = lblSuccessMsg.Text = string.Empty;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save bank desable details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			lblErrorMsg.Text = lblSuccessMsg.Text = string.Empty;
			SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
			SaveBankDetails(oSchoolwiseBankMasterBL);
			SetDefaultValues();
		}
		catch (SqlException oEx)
		{
			lblErrorMsg.Text = oEx.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region --- Methods ---

	/// <summary>
	/// This method is used to set initial values which are require while page loading.
	/// </summary>
	private void Initialize()
	{
		ddlBankName.Focus();
		ApplyMouseHoverEffect(new List<Button> { btnBack, btnCancel, btnSave });
		valSumBank.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		SetDefaultValues();
	}

	/// <summary>
	/// This method is used to fill combobox with bank list.
	/// </summary>
	private void FillBankCombo()
	{
		SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
		ListSource.FillDropDownList(oSchoolwiseBankMasterBL.GetNetBankingDetails(miSchoolId), ddlBankName, "RegisterdBankName", "NetBankingBankId", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to fill list view with the disabled bank details.
	/// </summary>
	private void FillBankDetails()
	{
		SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
		oSchoolwiseBankMasterBL.GetDisabledBankDetails(miSchoolId);
		FillBankListview(oSchoolwiseBankMasterBL);
	}

	/// <summary>
	/// This method is used to fill list view with the disabled bank details.
	/// </summary>
	/// <param name="oSchoolwiseBankMasterBL"></param>
	private void FillBankListview(SchoolwiseBankMasterBL oSchoolwiseBankMasterBL)
	{
		lstvwDisabledBankDetails.DataSource = oSchoolwiseBankMasterBL.lstDisbaleBankDetailList;
		lstvwDisabledBankDetails.DataBind();
		if (lstvwDisabledBankDetails.Items.Count > Constants.I_ZERO)
			ControlUtility.FillListViewPagerFooter(lstvwDisabledBankDetails, DtPgCount);
	}

	/// <summary>
	/// This method is used to save bank details and bind details to listview.
	/// </summary>
	/// <param name="oSchoolwiseBankMasterBL"></param>
	private void SaveBankDetails(SchoolwiseBankMasterBL oSchoolwiseBankMasterBL)
	{
		oSchoolwiseBankMasterBL.SaveBankDisablePeriodDetails(GenerateXml(PopulateBankDisableDetails()), miSchoolId, miUserId);
		lblSuccessMsg.Text = hidIsNewOrIsExisting.Value == Constants.S_ZERO ? S_SAVE_MSG : S_UPDATE_MSG;
		FillBankListview(oSchoolwiseBankMasterBL);
	}

	/// <summary>
	/// This method is used to populate bank disable list.
	/// </summary>
	/// <returns></returns>
	public List<DisbaleBankDetails> PopulateBankDisableDetails()
	{
		List<DisbaleBankDetails> lstDisbaleBankDetails = new List<DisbaleBankDetails>();
		DisbaleBankDetails moDisbaleBankDetails = new DisbaleBankDetails();
		RegBankDetails moRegBankDetails = new RegBankDetails();
		moDisbaleBankDetails.DisabledBankId = hidIsNewOrIsExisting.Value == Constants.S_ZERO ? Constants.I_ZERO : Convert.ToInt32(lstvwDisabledBankDetails.DataKeys[Convert.ToInt32(hidRowId.Value)]["DisabledBankId"]);
		moRegBankDetails.NetBankingBankId = Convert.ToInt32(ddlBankName.SelectedValue);
		moDisbaleBankDetails.RegBankDetails = moRegBankDetails;
		moDisbaleBankDetails.StartDateTime = Convert.ToDateTime(txtStartDate.Text + " " + txtStartTime.Text);
		moDisbaleBankDetails.EndDateTime = txtEndDate.Text != string.Empty ? Convert.ToDateTime(txtEndDate.Text + " " + txtEndTime.Text) : S_DEFAULT_DATE_2.ToDateTime();
		lstDisbaleBankDetails.Add(moDisbaleBankDetails);
		return lstDisbaleBankDetails;
	}

	/// <summary>
	/// This method is used to set visibility according to action.
	/// </summary>
	/// <param name="abAction"></param>
	private void SetVisibility(bool abAction)
	{
		trPhotoPager.Visible = abAction;
		DtPgCount.Visible = abAction;
	}

	/********** Do not delete - need to implement JSON*************/
	/* 
	/// <summary>
	/// This method is used to fill currently blocked bank detail list.
	/// </summary>
	private void populateBlockedBank()
	{
		var obj = new Dictionary<string, object>();
		int iCount = 0;
		Label lblEndDateTime;
		BlockedBank oBlockedBank = null;
		mlstBlockedBanks = new List<BlockedBank>();
		while (iCount < lstvwDisabledBankDetails.Items.Count)
		{
			lblEndDateTime = lstvwDisabledBankDetails.Items[iCount].FindControl("lblEndDateTime") as Label;

			oBlockedBank = new BlockedBank
			{
				DisableBankId = Convert.ToInt32(lstvwDisabledBankDetails.DataKeys[iCount]["DisabledBankId"]),
				BankId = Convert.ToInt32((lstvwDisabledBankDetails.Items[iCount].FindControl("hidSchoolwiseBankId") as HiddenField).Value),
				StartDate = Convert.ToDateTime((lstvwDisabledBankDetails.Items[iCount].FindControl("lblStartDateTime") as Label).Text),
				EndDate = lblEndDateTime.Text != S_DASH ? Convert.ToDateTime(lblEndDateTime.Text) : DateTime.Now
			};
			mlstBlockedBanks.Add(oBlockedBank);
			iCount++;
		}
	}

	/// <summary>
	/// This method is used to serialize block bank object.
	/// </summary>
	private void SerializeBlocksBanks()
	{
		if (mlstBlockedBanks.IsNull() || mlstBlockedBanks.Count <= Constants.I_ZERO)
			return;
		var obj = new Dictionary<string, object>();
		mlstBlockedBanks.ForEach(bank =>
		{
			if (!obj.ContainsKey(bank.DisableBankId.ToString()))
			{
				obj.Add(bank.DisableBankId.ToString(),
				new
				{
					BankId = bank.BankId.ToString(),
					StartDate = bank.StartDate.ToString(),
					EndDate = bank.EndDate.ToString(),
				});
			}
		});

		var sJSONSerializer = new JavaScriptSerializer();
		hidBlockedBanksJSON.Value = sJSONSerializer.Serialize(obj);
	}
	*/

	/// <summary>
	/// This method is used to set data pager value after popup closed.
	/// </summary>
	private void SetDataPagerValue()
	{
		if (hidPageNo.Value.ToInt() <= Constants.I_ZERO)
			return;

		var oDtPager = lstvwDisabledBankDetails.FindControl("DtPgDropDown") as DataPager;
		// If the records displayed on the page are less than the page size, we need not show the pager controls.        
		if (oDtPager == null || (oDtPager.TotalRowCount <= oDtPager.PageSize))
			return;

		var ddlCnt = (oDtPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
		ddlCnt.SelectedValue = (Convert.ToInt32(hidPageNo.Value) - Constants.I_ONE).ToString();
		ddlCnt_SelectedIndexChanged(ddlCnt, null);

	}

	/// <summary>
	/// This method is used to set default valued to controls.
	/// </summary>
	private void SetDefaultValues()
	{
		hidIsNewOrIsExisting.Value = Constants.S_ZERO;
		ddlBankName.SelectedValue = Constants.S_ZERO;
		txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
		txtStartTime.Text = DateTime.Now.ToString("hh:mm tt");
		txtEndDate.Text = txtEndTime.Text = string.Empty;
		btnSave.Text = S_SAVE;
	}

	#endregion

}
