using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

/// <summary>
/// This class is used to display all financial year.
/// </summary>
public partial class SchoolwiseFinancialYearUI : SchoolBase
{
	#region "Member"

	private AccountsBaseClient moAccountsBaseClient;
	private FinancialYearBL moFinancialYearBL = new FinancialYearBL();

	#endregion

	#region "Constant"

	private const string S_SORT = "SORT_ROW";
	private const string S_DEFAULT_SORT_EXP = "StartDate";
	private const string S_DEFAULT_SORT_EXP_END_DATE = "EndDate";
	private const string S_DEFAULT_SORT_DIR = Constants.S_DESCENDING;

	#endregion

	#region "Events"
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
			base.AddSortImage(lstvwFinancialYears, hidSortExpression.Value, hidSortDirection.Value);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to initialize member variable and display financial year details
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	 {
		try
		{			
			if (Settings.EnableAccountsModule)
				InitializeGroupServiceObj();

			if (!IsPostBack)
			{
				hidSortExpression.Value = S_DEFAULT_SORT_EXP;
				hidSortDirection.Value = S_DEFAULT_SORT_DIR;
				BindData(S_DEFAULT_SORT_DIR, S_DEFAULT_SORT_EXP);
				ApplyMouseHoverEffect(new List<Button>() { btnCancel, btnSave });
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set controls according to data.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFinancialYears_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				var oFinancialYear = ((AccountsEntities.FinancialYear)oCurrentItem.DataItem);
		
				RadioButton optCurrentYear = oCurrentItem.FindControl("optCurrentYear") as RadioButton;
				CheckBox chkClosed = oCurrentItem.FindControl("chkClosed") as CheckBox;				
				Label lblStartDate = oCurrentItem.FindControl("lblStartDate") as Label;
				Label lblEndDate = oCurrentItem.FindControl("lblEndDate") as Label;

				lblStartDate.Text = oFinancialYear.StartDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
				lblEndDate.Text = oFinancialYear.EndDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
				optCurrentYear.Checked = oFinancialYear.IsCurrent;				
				chkClosed.Checked = (((AccountsEntities.FinancialYear)oCurrentItem.DataItem).IsClosed);				
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update financial year details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			List<FinancialYear> lstFinancialYear = new List<FinancialYear>();
			FinancialYear oFinancialYear = null;
			CheckBox chkCloseYear;
			RadioButton optCurrentYear;

			for (int iListCount = 0; iListCount < lstvwFinancialYears.Items.Count; iListCount++)
			{
				chkCloseYear = lstvwFinancialYears.Items[iListCount].FindControl("chkClosed") as CheckBox;
				optCurrentYear = lstvwFinancialYears.Items[iListCount].FindControl("optCurrentYear") as RadioButton;

				oFinancialYear = new FinancialYear()
				{
					Id = lstvwFinancialYears.DataKeys[iListCount]["FinancialYearId"].ToInt(),
					IsClosed = chkCloseYear.Checked,
					IsCurrent = optCurrentYear.Checked
				};

				lstFinancialYear.Add(oFinancialYear);
			}

			string sXml = GenerateXml(lstFinancialYear);
			moFinancialYearBL.UpdateFinancialYearDetails(sXml, Session[Constants.S_SESSION_USER_ID].ToInt());
            lblSuccess.Text = "Financial year updated successfully !!!";

			BindData(hidSortDirection.Value, hidSortExpression.Value);

            if (Settings.EnableAccountsModule)
                moAccountsBaseClient.RebuildCache();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used for sorting.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFinancialYears_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName == S_SORT)
			{
				base.RevertSortOrder(hidSortDirection);
				if (hidSortExpression.Value != e.CommandArgument.ToString())
					hidSortDirection.Value = Constants.S_DESCENDING;
				
				hidSortExpression.Value = e.CommandArgument.ToString();
				BindData(hidSortDirection.Value, hidSortExpression.Value);				
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region "Private method"

	/// <summary>
	/// Initializes the Group service object.
	/// </summary>
	private void InitializeGroupServiceObj()
	{
		moAccountsBaseClient = new AccountsBaseClient();
		moAccountsBaseClient.Open();
	}

	/// <summary>
	/// This method is used to bind data to list view.
	/// </summary>
	private void BindData(string asSortDirection, string asSortExpression)
	{
		List<FinancialYear> lstFinancialYear = moFinancialYearBL.GetAllFinancialYears(miSchoolId);
		if (lstFinancialYear.Count < 1)
			btnSave.Visible = false;
		if (asSortDirection == Constants.S_ASCENDING)
		{
			if (asSortExpression == S_DEFAULT_SORT_EXP)
				lstvwFinancialYears.DataSource = lstFinancialYear.OrderBy(FinancialYr => FinancialYr.StartDate);
			else if (asSortExpression == S_DEFAULT_SORT_EXP_END_DATE)
				lstvwFinancialYears.DataSource = lstFinancialYear.OrderBy(FinancialYr => FinancialYr.StartDate);
		}
		else if (asSortDirection == Constants.S_DESCENDING)
		{
			if (asSortExpression == S_DEFAULT_SORT_EXP)
				lstvwFinancialYears.DataSource = lstFinancialYear.OrderByDescending(FinancialYr => FinancialYr.StartDate);
			else if (asSortExpression == S_DEFAULT_SORT_EXP_END_DATE)
				lstvwFinancialYears.DataSource = lstFinancialYear.OrderByDescending(FinancialYr => FinancialYr.StartDate);
		}

		lstvwFinancialYears.DataBind();
	}

	#endregion
}