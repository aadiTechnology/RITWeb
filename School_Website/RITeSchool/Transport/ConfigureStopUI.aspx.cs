using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Transport;
using Utility;

public partial class ConfigureStopUI : SchoolBase
{
	#region "CONSTANTS"
	const string S_COMMAND_REMOVE = "REMOVESTOP";
	const string S_COMMAND_UPDATE = "UPDATESTOP";
	const string S_DEFAULT_SORT_EXP = "Name";
	const string S_EDIT_MODE = "EDIT";
	const string S_MODE_NEW = "NEW";
	const string S_TRANSPORT_DETAILS_ADDED_MSG = "Stop Name is saved successfully!!!";
	const string S_TRANSPORT_DETAILS_UPDATED_MSG = "Stop Name is updated successfully!!!";
	const string S_TRANSPORTNAME_DETAILS_DELETED_MSG = "Stop Name is Deleted successfully!!!";
	const string S_TRANSPORT_DETAILS_DELETED_MSG = "Stop Name can not be deleted since associated with Route.";
	const string S_TRANSPORT_DATES_ADDED_MSG = "Transport dates Added/Updated Successfully.";
	const string S_TRANSPORT_DATES_ERROR_MSG = "Transport service duration dates should be within current academic year.";
    const string S_TRANSPORT_DATES_EMPTY_MSG = "Start Date and End Date should not be empty.";
      const string S_TRANSPORT_END_DATE_ERROR_MSG = "End Date should be greater than or equal to Start Date.";
	#endregion

	#region "Data Members"

	private StopMasterBL moStopMasterBL;

	#endregion

	#region "EVENTS"
	/// <summary>
	/// This event is used to fill existing StopNames listView
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			moStopMasterBL = new StopMasterBL(miSchoolId, miAcademicYearId, miUserId);
			if (!IsPostBack)
			{
				SetDefaultValues();
				SetTransportDatesDefaultValues();
				FillUserRoleListView();
				FillExistingStopListview();
				SetJavascriptAttributes();
			}
			lblCheckDependency.Visible = false;
			lblUpdateSucess.Visible = false;
			txtStopName.Focus();
			btnAdd.Text = "Add";
			lblErrorMsg.Visible = false;
			valSumErrorMsg.Visible = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add attribute to existing StopName listviews item control.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwConfigureStop_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
				ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
				oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                TextBox txtCharges = oCurrentItem.FindControl("txtCharges") as TextBox;
                txtCharges.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
                TextBox txtOneWayCharges = oCurrentItem.FindControl("txtOneWayCharges") as TextBox;
                txtOneWayCharges.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill footer property of existing Stop name listview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwConfigureStop_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwConfigureStop.Items.Count > 0)
			{
				ControlUtility.FillListViewPagerFooter(lstvwConfigureStop, DtPgCount);
				if (IsPostBack)
					AddSortImage();

               
			}
			else
			{
				DtPgCount.Visible = false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to Edit or Delete stop Names 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwConfigureStop_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName != "Sort")
			{
				ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
				int iListIndex = oCurrentItem.DisplayIndex;
				int iStopId = Convert.ToInt32(lstvwConfigureStop.DataKeys[iListIndex]["StopId"]);
				string sStopName = lstvwConfigureStop.DataKeys[iListIndex]["StopName"].ToString();
				hidStopId.Value = iStopId.ToString();
                hidStopName.Value = lstvwConfigureStop.DataKeys[iListIndex]["StopName"].ToString();
				if (e.CommandName == S_COMMAND_REMOVE)
					DeleteStopMasterDetails(iStopId);
				else if (e.CommandName == S_COMMAND_UPDATE)
					SetStopCharges(iStopId);

				divSetting.Style.Add("visibility", "hidden");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to Add Stop Names
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnAdd_Click(object sender, EventArgs e)
	{
        try
        {
            if (Page.IsValid)
            {
                lblCheckDependency.Visible = false;
                SaveStopDetails();
                if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StopConfiguration));
                FillExistingStopListview();
            }
        }
        catch (DuplicateEntityException Ex)
        {
            lblErrorMsg.Visible = true;
            AddSortImage();
            lblErrorMsg.Text = Ex.ErrorMessage;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            btnAdd.Text = "Update";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
	}

	/// <summary>
	/// This event is used  to configure Transport service dates.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSavePopup_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtEndDate.Text) || string.IsNullOrEmpty(txtStartDate.Text))
			{
				divSetting.Style.Add("visibility", "visible");
				divSetting.Style.Add("display", "block");
				lblMsg.Visible = true;
				lblMsg.Text = S_TRANSPORT_DATES_EMPTY_MSG;
			}
			else
			{
				DateTime dtAccYearStardDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
				DateTime dtAccYearEndDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
				DateTime dtStartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
				DateTime dtEndDate = Convert.ToDateTime(txtEndDate.Text.ToString());

				if (dtStartDate > dtEndDate)
				{
					divSetting.Style.Add("visibility", "visible");
					divSetting.Style.Add("display", "block");
					lblMsg.Visible = true;
					lblMsg.Text = S_TRANSPORT_END_DATE_ERROR_MSG;
				}
				else if (dtStartDate >= dtAccYearStardDate && dtEndDate <= dtAccYearEndDate)
				{
					SaveTransportDates();
                    divSetting.Style.Add("visibility", "hidden");
                    divSetting.Style.Add("display", "none");
					lblUpdateSucess.Visible = true;
					lblUpdateSucess.Text = S_TRANSPORT_DATES_ADDED_MSG;
				}
				else
				{
                    divSetting.Style.Add("visibility", "visible");
                    divSetting.Style.Add("display", "block");
                    lblMsg.Visible = true;
                    lblMsg.Text = S_TRANSPORT_DATES_ERROR_MSG;
				}
			}
			SetTransportDatesDefaultValues();
		}
		catch (SqlException Exc)
		{
			divSetting.Style.Add("visibility", "visible");
			divSetting.Style.Add("display", "block");
			lblMsg.Visible = true;
			AddSortImage();
			lblMsg.Text = Exc.Message;
		}
		catch (DuplicateEntityException Ex)
		{
			lblErrorMsg.Visible = true;
			AddSortImage();
			lblErrorMsg.Text = Ex.ErrorMessage;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to sort the listview of StopName by Name.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwConfigureStop_Sorting(object sender, ListViewSortEventArgs e)
	{
		try
		{
			hidSortExpression.Value = e.SortExpression;
			SetSortVariables();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to view page wise Stop Name list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwConfigureStop);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to Cancel Saving and clear text values. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>   
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ClearFields();
			txtStopName.Focus();
			AddSortImage();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to search the Stops by names.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillExistingStopListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion

	#region "PRIVATE METHODS"
	/// <summary>
	/// This method is used to set JavaScript attributes
	/// </summary>
	private void SetJavascriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnCancel, btnAdd, btnBack });
		btnAdd.Attributes["onclick"] = "ResetUpdateLbl()";
		btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted));
	}

	/// <summary>
	/// This method is used set sort variables.
	/// </summary>
	private void SetSortVariables()
	{
		if (hidSortDirection.Value == Constants.S_DESCENDING)
			hidSortDirection.Value = Constants.S_ASCENDING;
		else
			hidSortDirection.Value = Constants.S_DESCENDING;
	}

	/// <summary>
	/// This method is used to set sorting image to list view headers.
	/// </summary>
	private void AddSortImage()
	{
		if (lstvwConfigureStop.SortDirection.ToString() == "Ascending" || lstvwConfigureStop.SortDirection.ToString() == string.Empty)
			hidSortDirection.Value = Constants.S_ASCENDING;
		else
			hidSortDirection.Value = Constants.S_DESCENDING;
		if (lstvwConfigureStop.SortExpression != string.Empty)
			hidSortExpression.Value = lstvwConfigureStop.SortExpression.ToString();
		else
			hidSortExpression.Value = S_DEFAULT_SORT_EXP;

		HtmlTableRow oHtmlTableHeaderRow = lstvwConfigureStop.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
	}

	/// <summary>
	/// This method is used set datasource  to ListView
	/// </summary>
	/// 
	private void FillExistingStopListview()
	{
		lstvwConfigureStop.DataSourceID = ObjDSConfigureStop.ID;
		lstvwConfigureStop.DataBind();
	}

	/// <summary>
	/// This Method is used to read values for StopMasterBL properties.
	/// </summary>
	/// <returns></returns>
	private void Populate()
	{
		moStopMasterBL.StopName = txtStopName.Text.Trim();
		moStopMasterBL.StopId = 0;
		moStopMasterBL.SchoolId = miSchoolId;
		moStopMasterBL.Academic_Year_Id = miAcademicYearId;
		moStopMasterBL.InsertedById = miUserId;
		moStopMasterBL.InsertDate = DateTime.Now;
		moStopMasterBL.UpdateDate = DateTime.Now;
		moStopMasterBL.UpdatedById = miUserId;
		if (hidMode.Value == S_EDIT_MODE)
			moStopMasterBL.StopId = Convert.ToInt32(hidStopId.Value);
	}

	/// <summary>
	/// This Method is used to save Transport service dates.
	/// </summary>
	private void SaveTransportDates()
	{
		TransportServiceDates oTransportServiceDates = new TransportServiceDates
		{
			StartDate = Convert.ToDateTime(txtStartDate.Text.Trim()),
			EndDate = Convert.ToDateTime(txtEndDate.Text.Trim()),
		};

		moStopMasterBL.InsertServiceDateDetails(oTransportServiceDates);
	}

	/// <summary>
	/// This method is used to Insert/Update Stop Details
	/// </summary>
	private void SaveStopDetails()
	{
		Populate();
		if (moStopMasterBL.IsDuplicateStopName())
		{
			moStopMasterBL.Insert(GenerateXml(PopulateStopCharges()));
			if (hidMode.Value != S_EDIT_MODE)
			{
				lblUpdateSucess.Visible = true;
				lblUpdateSucess.Text = S_TRANSPORT_DETAILS_ADDED_MSG;
			}
			else
			{
				lblUpdateSucess.Visible = true;
				lblUpdateSucess.Text = S_TRANSPORT_DETAILS_UPDATED_MSG;
			}
		}
		ClearFields();
	}

	/// <summary>
	/// This method is used to populate document details
	/// </summary>
	/// <returns></returns>
	private StopDetails PopulateStopCharges()
	{
		List<StopCharge> lstStopChargess = new List<StopCharge>();
		StopCharge oStopCharge = null;
		StopDetails oStopDetails = new StopDetails();

		for (int iRowCount = 0; iRowCount < lstvwStopCharges.Items.Count; iRowCount++)
		{
			oStopCharge = new StopCharge();
			ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStopCharges.Items[iRowCount];
			int iRowId = oCurrentItem.DisplayIndex;
			TextBox txtCharges = (TextBox)lstvwStopCharges.Items[iRowId].FindControl("txtCharges");
			TextBox txtOneWayCharges = (TextBox)lstvwStopCharges.Items[iRowId].FindControl("txtOneWayCharges");
			oStopCharge.RoleId = Convert.ToInt32(lstvwStopCharges.DataKeys[iRowId]["RoleId"]);
			oStopCharge.Charges = txtCharges.Text;
			oStopCharge.OneWayCharges = txtOneWayCharges.Text;
			lstStopChargess.Add(oStopCharge);
		}
		oStopDetails.StopName = txtStopName.Text.Trim();
		oStopDetails.StopId = Convert.ToInt32(hidStopId.Value);
		oStopDetails.lstStopCharges = lstStopChargess;
		return oStopDetails;
	}

	/// <summary>
	/// This method is used to set stop charges.
	/// </summary>
	/// <param name="aiStopId"></param>
	/// <param name="aiSchoolId"></param>
	private void SetStopCharges(int aiStopId)
	{
		lblUpdateSucess.Text = string.Empty;
		AddSortImage();
		HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStopCharges.FindControl("trHeaderControls");
		
		StopDetails oStopDetails = new StopDetails();
		oStopDetails = moStopMasterBL.GetStopDetails(aiStopId);
		txtStopName.Text = oStopDetails.StopName;
		for (int iRowCount = 0; iRowCount < lstvwStopCharges.Items.Count; iRowCount++)
		{
			ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStopCharges.Items[iRowCount];
			int iRowId = oCurrentItem.DisplayIndex;
			int iRoleId = Convert.ToInt32(lstvwStopCharges.DataKeys[iRowId]["RoleId"]);
			TextBox txtCharges = (TextBox)lstvwStopCharges.Items[iRowId].FindControl("txtCharges");
			TextBox txtOneWayCharges = (TextBox)lstvwStopCharges.Items[iRowId].FindControl("txtOneWayCharges");
			var oStopCharges = oStopDetails.lstStopCharges.Where(st => st.RoleId == iRoleId).FirstOrDefault();
			if (oStopCharges != null)
			{
				txtCharges.Text = oStopCharges.Charges.ToString();
				txtOneWayCharges.Text = oStopCharges.OneWayCharges.ToString();
			}
		}
		hidMode.Value = S_EDIT_MODE;
		btnAdd.Text = "Update";
	}

	/// <summary>
	/// This method is used to Delete StopMaster Details
	/// </summary>
	/// <param name="aiStopId"></param>
	/// <param name="aiSchoolId"></param>
	private void DeleteStopMasterDetails(int aiStopId)
	{
		int iCheckDependency = CheckDependencyForStopName();
		if (iCheckDependency == 0)
		{
			moStopMasterBL.DeleteStopMaster(aiStopId);
			lblUpdateSucess.Visible = true;
			lblUpdateSucess.Text = S_TRANSPORTNAME_DETAILS_DELETED_MSG;
		}
		else
		{
			lblCheckDependency.Visible = true;
			lblCheckDependency.Text = S_TRANSPORT_DETAILS_DELETED_MSG.Replace("hidStopName.Value", hidStopName.Value);
		}
		int iTotalStopCount = moStopMasterBL.GetTotalStopCount();
		if (iTotalStopCount == 0)
			DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StopConfiguration));
		FillExistingStopListview();
		ClearFields();
	}

	/// <summary>
	/// This Method is used to clear form fields.
	/// </summary>
	private void ClearFields()
	{
		HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStopCharges.FindControl("trHeaderControls");
		TextBox txtHeaderCharges = (TextBox)oHtmlTableRow.FindControl("txtAllCharges");
		TextBox txtHeaderOneWayCharges = (TextBox)oHtmlTableRow.FindControl("txtAllOneWayCharges");
		txtHeaderCharges.Text = string.Empty;
		txtHeaderOneWayCharges.Text = string.Empty;
		txtStopName.Text = string.Empty;
		for (int iRowCount = 0; iRowCount < lstvwStopCharges.Items.Count; iRowCount++)
		{
			ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStopCharges.Items[iRowCount];
			int iRowId = oCurrentItem.DisplayIndex;
			TextBox txtCharges = (TextBox)lstvwStopCharges.Items[iRowId].FindControl("txtCharges");
			TextBox txtOneWayCharges = (TextBox)lstvwStopCharges.Items[iRowId].FindControl("txtOneWayCharges");
			txtCharges.Text = Constants.S_ZERO;
			txtOneWayCharges.Text = Constants.S_ZERO;
		}
		hidStopId.Value = Constants.S_ZERO;
		txtStopName.Focus();
		valSumErrorMsg.Style.Add("innerhtml", "");
		hidMode.Value = S_MODE_NEW;
	}

	/// <summary>
	/// This method is used to set default values.
	/// </summary>
	private void SetDefaultValues()
	{
		AddSortImage();
		hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		hidSortDirection.Value = SortDirection.Ascending.ToString();
		btnClosePopUp.Attributes.Add("onclick", "HidePopup();return false;");
	}

	/// <summary>
	/// This method is used to Set Transport Service dates.
	/// </summary>
	private void SetTransportDatesDefaultValues()
	{
		TransportServiceDates oTransportServiceDates = moStopMasterBL.GetTransportServiceDates();
		if (oTransportServiceDates.StartDate.ToShortDateString() != Constants.S_DEFAULT_DATE && oTransportServiceDates.EndDate.ToShortDateString() != Constants.S_DEFAULT_DATE)
		{
			txtStartDate.Text = oTransportServiceDates.StartDate.ToString(Constants.S_DATE_FORMAT);
			txtEndDate.Text = oTransportServiceDates.EndDate.ToString(Constants.S_DATE_FORMAT);
		}
		else
		{
			txtStartDate.Text = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]).ToString(Constants.S_DATE_FORMAT);
			txtEndDate.Text = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]).ToString(Constants.S_DATE_FORMAT);
		}
		hidTransportStardDate.Value = txtStartDate.Text;
		hidTransportEndDate.Value = txtEndDate.Text;
	}

	/// <summary>
	/// This method is used to find the association of Stop Name with route.
	/// </summary>
	/// <returns></returns>
	private int CheckDependencyForStopName()
	{
		int iStopId = Convert.ToInt32(hidStopId.Value);
		return moStopMasterBL.CheckDependencyForStopName(iStopId);
	}

	/// <summary>
	/// This method is used to fill user role names in list view.
	/// </summary>
	private void FillUserRoleListView()
	{
		List<StopCharge> lstUserRole = moStopMasterBL.GetUserRoleWiseCharges();
		lstvwStopCharges.DataSource = lstUserRole;
		lstvwStopCharges.DataBind();
	}

	#endregion
 
}
