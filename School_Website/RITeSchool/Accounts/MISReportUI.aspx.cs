/* ------------------------------------------------------------------------------------------------
 *	FileName	: MISReport.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 24-March-2012
 *	Purpose		: Displays an overall summary of Income & Expenses in the financial year.
 * ------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

/// <summary>
///		Displays an overall summary of Income & Expenses in the financial year.
/// </summary>
public partial class MISReportUI : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_NEG_CLASS = " neg";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private string msStartYear;
	private string msStartYearShort;
	private string msEndYear;
	private string msEndYearShort;

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	/// <summary>
	///		Determines if the page is opened as a popup.
	/// </summary>
	private bool IsPopup
	{
		get { return QueryString["IsPopup"] == Constants.S_YES; }
	}

	/// <summary>
	///		Determines if the page is opened from the Management Dashboard.
	/// </summary>
	private bool IsFromMgmtDashboard
	{
		get { return QueryString["IsFromMgmtDashboard"] == Constants.S_YES; }
	}

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This Event is used to set the MasterPage based on the logged in user.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			base.OnPreInit(e);
			
			if (IsPopup)
				Page.MasterPageFile = "../MasterPages/PopupMaster.master";
			else if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null && !IsFromMgmtDashboard)
                Page.MasterPageFile = "../MasterPages/MasterPage.master";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
     
	}

	/// <summary>
	/// Handles the loading of a page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				SetPostbackUrl();
				SetStudentTotal();
				BindReport();

				ApplyMouseHoverEffect(new List<Button> { btnBack });

				if (IsPopup)
				{
					btnBack.Visible = false;
					btnClose.Visible = true;
				}

                base.OnPreInit(e);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		Hides certain controls on the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_PreRender(object sender, EventArgs e)
	{
		try
		{
			if (IsFromMgmtDashboard)
				HideMasterControls();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Sets the totals for each group in the section.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwSection_OnItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var item = e.Item as ListViewDataItem;
				var oGroup = item.DataItem as MISReportGroup;

				SetLabelProperties(item, "lblAprGroupTotal"		, oGroup.MonthlyTotals.April	, false);
				SetLabelProperties(item, "lblMayGroupTotal"		, oGroup.MonthlyTotals.May		, false);
				SetLabelProperties(item, "lblJunGroupTotal"		, oGroup.MonthlyTotals.June		, false);
				SetLabelProperties(item, "lblJulGroupTotal"		, oGroup.MonthlyTotals.July		, false);
				SetLabelProperties(item, "lblAugGroupTotal"		, oGroup.MonthlyTotals.August	, false);
				SetLabelProperties(item, "lblSepGroupTotal"		, oGroup.MonthlyTotals.September, false);
				SetLabelProperties(item, "lblOctGroupTotal"		, oGroup.MonthlyTotals.October	, false);
				SetLabelProperties(item, "lblNovGroupTotal"		, oGroup.MonthlyTotals.November , false);
				SetLabelProperties(item, "lblDecGroupTotal"		, oGroup.MonthlyTotals.December , false);
				SetLabelProperties(item, "lblJanGroupTotal"		, oGroup.MonthlyTotals.January	, false);
				SetLabelProperties(item, "lblFebGroupTotal"		, oGroup.MonthlyTotals.February , false);
				SetLabelProperties(item, "lblMarGroupTotal"		, oGroup.MonthlyTotals.March	, false);
				SetLabelProperties(item, "lblQuarter1GroupTotal", oGroup.MonthlyTotals.Quarter1	, false);
				SetLabelProperties(item, "lblQuarter2GroupTotal", oGroup.MonthlyTotals.Quarter2	, false);
				SetLabelProperties(item, "lblQuarter3GroupTotal", oGroup.MonthlyTotals.Quarter3	, false);
				SetLabelProperties(item, "lblQuarter4GroupTotal", oGroup.MonthlyTotals.Quarter4	, false);
				SetLabelProperties(item, "lblTerm1GroupTotal"	, oGroup.MonthlyTotals.Term1	, false);
				SetLabelProperties(item, "lblTerm2GroupTotal"	, oGroup.MonthlyTotals.Term2	, false);
				
				decimal dAnnualTotal = oGroup.MonthlyTotals.Annual;
				SetLabelProperties(item, "lblAnnualGroupTotal", dAnnualTotal, false);

				decimal dBudgetTotal = oGroup.Budget;
				SetLabelProperties(item, "lblBudgetGroupTotal", dBudgetTotal, false);

				decimal dVarianceGroupTotal = oGroup.GroupNature.Id == Constants.GroupNature.Expenses.ToInt() ? dBudgetTotal - dAnnualTotal : dAnnualTotal - dBudgetTotal;
				SetLabelProperties(item, "lblVarianceGroupTotal", dVarianceGroupTotal, true);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Sets the titles for columns & grand total of each section.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwSection_OnDataBound(object sender, EventArgs e)
	{
		try
		{
			var lstvwSection = sender as ListView;
			if (lstvwSection.Items.Count > 0)
			{
				SetLabelTitle(lstvwSection, "lblAprTitle"	  , String.Format("April (Rs.)<br>(Apr-{0})"					  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblMayTitle"	  , String.Format("May (Rs.)<br>(May-{0})"						  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblJunTitle"	  , String.Format("June (Rs.)<br>(Jun-{0})"						  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblJulTitle"	  , String.Format("July (Rs.)<br>(Jul-{0})"						  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblAugTitle"	  , String.Format("August (Rs.)<br>(Aug-{0})"					  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblSepTitle"	  , String.Format("September (Rs.)<br>(Sep-{0})"				  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblOctTitle"	  , String.Format("October (Rs.)<br>(Oct-{0})"					  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblNovTitle"	  , String.Format("November (Rs.)<br>(Nov-{0})"					  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblDecTitle"	  , String.Format("December (Rs.)<br>(Dec-{0})"					  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblJanTitle"	  , String.Format("January (Rs.)<br>(Jan-{0})"					  , msEndYearShort));
				SetLabelTitle(lstvwSection, "lblFebTitle"	  , String.Format("February (Rs.)<br>(Feb-{0})"					  , msEndYearShort));
				SetLabelTitle(lstvwSection, "lblMarTitle"	  , String.Format("March (Rs.)<br>(Mar-{0})"					  , msEndYearShort));
				SetLabelTitle(lstvwSection, "lblQuarter1Title", String.Format("Quarter I (Rs.)<br>(1-Apr-{0} to 30-Jun-{0})"  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblQuarter2Title", String.Format("Quarter II (Rs.)<br>(1-Jul-{0} to 30-Sep-{0})" , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblQuarter3Title", String.Format("Quarter III (Rs.)<br>(1-Oct-{0} to 31-Dec-{0})", msStartYearShort));
				SetLabelTitle(lstvwSection, "lblQuarter4Title", String.Format("Quarter IV (Rs.)<br>(1-Jan-{0} to 31-Mar-{0})" , msEndYearShort));
				SetLabelTitle(lstvwSection, "lblTerm1Title"	  , String.Format("Term I (Rs.)<br>(1-Apr-{0} to 30-Sep-{0})"	  , msStartYearShort));
				SetLabelTitle(lstvwSection, "lblTerm2Title"	  , String.Format("Term II (Rs.)<br>(1-Oct-{0} to 31-Mar-{1})"	  , msStartYearShort, msEndYearShort));
				SetLabelTitle(lstvwSection, "lblAnnualTitle"  , String.Format("Annual (Rs.)<br>(1-Apr-{0} to 31-Mar-{1})"	  , msStartYearShort, msEndYearShort));
			}
			else
				lstvwSection.Parent.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Sets the amount for each ledger.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwInner_OnItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var item = e.Item as ListViewDataItem;
				var oLedger = item.DataItem as MISReportLedger;

				SetLabelProperties(item, "hlnkAprAmount"	 , oLedger.MonthlyTotals.April	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=4&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkMayAmount"	 , oLedger.MonthlyTotals.May	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=5&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkJunAmount"	 , oLedger.MonthlyTotals.June	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=6&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkJulAmount"	 , oLedger.MonthlyTotals.July	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=7&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkAugAmount"	 , oLedger.MonthlyTotals.August   , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=8&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkSepAmount"	 , oLedger.MonthlyTotals.September, CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=9&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkOctAmount"	 , oLedger.MonthlyTotals.October  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=10&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkNovAmount"	 , oLedger.MonthlyTotals.November , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=11&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkDecAmount"	 , oLedger.MonthlyTotals.December , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=12&Year={1}"			  , oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkJanAmount"	 , oLedger.MonthlyTotals.January  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=1&Year={1}"			  , oLedger.Id, msEndYear)));
				SetLabelProperties(item, "hlnkFebAmount"	 , oLedger.MonthlyTotals.February , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=2&Year={1}"			  , oLedger.Id, msEndYear)));
				SetLabelProperties(item, "hlnkMarAmount"	 , oLedger.MonthlyTotals.March	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&Month=3&Year={1}"			  , oLedger.Id, msEndYear)));
				SetLabelProperties(item, "hlnkQuarter1Amount", oLedger.MonthlyTotals.Quarter1 , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Apr-{1}&To=30-Jun-{1}", oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkQuarter2Amount", oLedger.MonthlyTotals.Quarter2 , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Jul-{1}&To=30-Sep-{1}", oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkQuarter3Amount", oLedger.MonthlyTotals.Quarter3 , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Oct-{1}&To=31-Dec-{1}", oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkQuarter4Amount", oLedger.MonthlyTotals.Quarter4 , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Jan-{1}&To=31-Mar-{1}", oLedger.Id, msEndYear)));
				SetLabelProperties(item, "hlnkTerm1Amount"	 , oLedger.MonthlyTotals.Term1	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Apr-{1}&To=30-Sep-{1}", oLedger.Id, msStartYear)));
				SetLabelProperties(item, "hlnkTerm2Amount"	 , oLedger.MonthlyTotals.Term2	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Oct-{1}&To=31-Mar-{2}", oLedger.Id, msStartYear, msEndYear)));
				SetLabelProperties(item, "hlnkAnnualAmount"	 , oLedger.MonthlyTotals.Annual	  , CommonUtility.EncryptQuerystring(String.Format("LedgerId={0}&From=1-Apr-{1}&To=31-Mar-{2}", oLedger.Id, msStartYear, msEndYear)));
				SetLabelProperties(item, "lblBudgetAmount"	 , oLedger.Budget, false);

				decimal dVariance;

				if (oLedger.Group.GroupNature.Id == Constants.GroupNature.Expenses.ToInt())
					dVariance = oLedger.Budget - Math.Abs(oLedger.MonthlyTotals.Annual);
				else
					dVariance = oLedger.MonthlyTotals.Annual - oLedger.Budget;

				SetLabelProperties(item, "lblVarianceAmount" , dVariance, true);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	
	/// <summary>
	///		
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwMISReport_OnItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var item = e.Item as ListViewDataItem;
				if (item != null)
				{
					var oReportSection = item.DataItem as MISReportSection;
					if (oReportSection.Title != "Outflow")
					{
						Control oControl = item.FindControl("secfooter");
						oControl.Visible = false;
					}
					else
					{
						var lstReportSections = lstvwMISReport.DataSource as List<MISReportSection>;
						MISReportSection oInflowSection = lstReportSections.Find(section => section.Title == "Inflow");
						MISReportSection oOutflowSection = lstReportSections.Find(section => section.Title == "Outflow");

						if (oInflowSection != null && oOutflowSection != null)
						{
							SetLabelProperties(item, "lblJanGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.January) - Math.Abs(oOutflowSection.MonthlyTotals.January), false);
							SetLabelProperties(item, "lblFebGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.February) - Math.Abs(oOutflowSection.MonthlyTotals.February), false);
							SetLabelProperties(item, "lblMarGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.March) - Math.Abs(oOutflowSection.MonthlyTotals.March), false);
							SetLabelProperties(item, "lblAprGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.April) - Math.Abs(oOutflowSection.MonthlyTotals.April), false);
							SetLabelProperties(item, "lblMayGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.May) - Math.Abs(oOutflowSection.MonthlyTotals.May), false);
							SetLabelProperties(item, "lblJunGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.June) - Math.Abs(oOutflowSection.MonthlyTotals.June), false);
							SetLabelProperties(item, "lblJulGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.July) - Math.Abs(oOutflowSection.MonthlyTotals.July), false);
							SetLabelProperties(item, "lblAugGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.August) - Math.Abs(oOutflowSection.MonthlyTotals.August), false);
							SetLabelProperties(item, "lblSepGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.September) - Math.Abs(oOutflowSection.MonthlyTotals.September), false);
							SetLabelProperties(item, "lblOctGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.October) - Math.Abs(oOutflowSection.MonthlyTotals.October), false);
							SetLabelProperties(item, "lblNovGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.November) - Math.Abs(oOutflowSection.MonthlyTotals.November), false);
							SetLabelProperties(item, "lblDecGrossTotal"		, Math.Abs(oInflowSection.MonthlyTotals.December) - Math.Abs(oOutflowSection.MonthlyTotals.December), false);
							SetLabelProperties(item, "lblQuarter1GrossTotal", Math.Abs(oInflowSection.MonthlyTotals.Quarter1) - Math.Abs(oOutflowSection.MonthlyTotals.Quarter1), false);
							SetLabelProperties(item, "lblQuarter2GrossTotal", Math.Abs(oInflowSection.MonthlyTotals.Quarter2) - Math.Abs(oOutflowSection.MonthlyTotals.Quarter2), false);
							SetLabelProperties(item, "lblQuarter3GrossTotal", Math.Abs(oInflowSection.MonthlyTotals.Quarter3) - Math.Abs(oOutflowSection.MonthlyTotals.Quarter3), false);
							SetLabelProperties(item, "lblQuarter4GrossTotal", Math.Abs(oInflowSection.MonthlyTotals.Quarter4) - Math.Abs(oOutflowSection.MonthlyTotals.Quarter4), false);
							SetLabelProperties(item, "lblTerm1GrossTotal"	, Math.Abs(oInflowSection.MonthlyTotals.Term1)	  - Math.Abs(oOutflowSection.MonthlyTotals.Term1)	, false);
							SetLabelProperties(item, "lblTerm2GrossTotal"	, Math.Abs(oInflowSection.MonthlyTotals.Term2)	  - Math.Abs(oOutflowSection.MonthlyTotals.Term2)	, false);
							SetLabelProperties(item, "lblAnnualGrossTotal"	, Math.Abs(oInflowSection.MonthlyTotals.Annual)	  - Math.Abs(oOutflowSection.MonthlyTotals.Annual)	, false);
							SetLabelProperties(item, "lblBudgetGrossTotal"	, Math.Abs(oInflowSection.Budget)				  - Math.Abs(oOutflowSection.Budget)				, false);
							SetLabelProperties(item, "lblVarianceGrossTotal", Math.Abs(oInflowSection.Budget) - Math.Abs(oInflowSection.MonthlyTotals.Annual) - (Math.Abs(oOutflowSection.Budget) - Math.Abs(oOutflowSection.MonthlyTotals.Annual)), true);
						}
					}

					SetLabelProperties(item.Controls[1].FindControl("lblJanTotal")	   , oReportSection.MonthlyTotals.January, false);
					SetLabelProperties(item.Controls[1].FindControl("lblFebTotal")	   , oReportSection.MonthlyTotals.February, false);
					SetLabelProperties(item.Controls[1].FindControl("lblMarTotal")	   , oReportSection.MonthlyTotals.March, false);
					SetLabelProperties(item.Controls[1].FindControl("lblAprTotal")	   , oReportSection.MonthlyTotals.April, false);
					SetLabelProperties(item.Controls[1].FindControl("lblMayTotal")	   , oReportSection.MonthlyTotals.May, false);
					SetLabelProperties(item.Controls[1].FindControl("lblJunTotal")	   , oReportSection.MonthlyTotals.June, false);
					SetLabelProperties(item.Controls[1].FindControl("lblJulTotal")	   , oReportSection.MonthlyTotals.July, false);
					SetLabelProperties(item.Controls[1].FindControl("lblAugTotal")	   , oReportSection.MonthlyTotals.August, false);
					SetLabelProperties(item.Controls[1].FindControl("lblSepTotal")	   , oReportSection.MonthlyTotals.September, false);
					SetLabelProperties(item.Controls[1].FindControl("lblOctTotal")	   , oReportSection.MonthlyTotals.October, false);
					SetLabelProperties(item.Controls[1].FindControl("lblNovTotal")	   , oReportSection.MonthlyTotals.November, false);
					SetLabelProperties(item.Controls[1].FindControl("lblDecTotal")	   , oReportSection.MonthlyTotals.December, false);
					SetLabelProperties(item.Controls[1].FindControl("lblQuarter1Total"), oReportSection.MonthlyTotals.Quarter1, false);
					SetLabelProperties(item.Controls[1].FindControl("lblQuarter2Total"), oReportSection.MonthlyTotals.Quarter2, false);
					SetLabelProperties(item.Controls[1].FindControl("lblQuarter3Total"), oReportSection.MonthlyTotals.Quarter3, false);
					SetLabelProperties(item.Controls[1].FindControl("lblQuarter4Total"), oReportSection.MonthlyTotals.Quarter4, false);
					SetLabelProperties(item.Controls[1].FindControl("lblTerm1Total")   , oReportSection.MonthlyTotals.Term1	  , false);
					SetLabelProperties(item.Controls[1].FindControl("lblTerm2Total")   , oReportSection.MonthlyTotals.Term2	  , false);

					decimal dAnnualTotal = oReportSection.MonthlyTotals.Annual;
					SetLabelProperties(item.Controls[1].FindControl("lblAnnualTotal"), dAnnualTotal, false);

					decimal dBudgetTotal = oReportSection.Budget;
					SetLabelProperties(item.Controls[1].FindControl("lblBudgetTotal"), dBudgetTotal, false);

					decimal dVariance;

					if (oReportSection.Title == "Outflow")
						dVariance = dBudgetTotal - dAnnualTotal;
					else
						dVariance = Math.Abs(dAnnualTotal) - dBudgetTotal;

					SetLabelProperties(item.Controls[1].FindControl("lblVarianceTotal"), dVariance, true);
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	
	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///		Displays the total student count in the school.
	/// </summary>
	private void SetStudentTotal()
	{
		if (IsFromMgmtDashboard)
		{
			tblStudentCount.Visible = false;
		}
		else
		{
			var oStudentBL = new StudentCollectionBL(miSchoolId, miAcademicYearId);
			int iStudentCount = oStudentBL.GetStudentCount();
			hlnkStudentCount.Text = iStudentCount.ToString();
			hlnkStudentCount.Attributes.Add("onclick",
											 String.Format("window.open('{0}?{1}' , '_new','scrollbars=yes,resizable=yes,top=0,left=0,width=900,height=670'); return false;",
															hlnkStudentCount.NavigateUrl,
															CommonUtility.EncryptQuerystring("IsManagementUser=Y")));
		}
	}
	
	/// <summary>
	/// Rebinds the MIS Report to its DataSource.
	/// </summary>
	private void BindReport()
	{
		int iFinancialYearId = miFinancialYearId;
		var oMISFinancialYear = Session[Constants.S_SESSION_MIS_FINANCIAL_YEAR] as Management.Entities.FinancialYear;
		if (oMISFinancialYear == null)
		{
			var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
			msStartYear = oFinancialYear.StartDate.ToString("yyyy");
			msEndYear = oFinancialYear.EndDate.ToString("yyyy");
			msStartYearShort = oFinancialYear.StartDate.ToString("yy");
			msEndYearShort = oFinancialYear.EndDate.ToString("yy");
		}
		else
		{
			msStartYear = oMISFinancialYear.StartDate.ToString("yyyy");
			msEndYear = oMISFinancialYear.EndDate.ToString("yyyy");
			msStartYearShort = oMISFinancialYear.StartDate.ToString("yy");
			msEndYearShort = oMISFinancialYear.EndDate.ToString("yy");
			iFinancialYearId = oMISFinancialYear.Id;
		}
		
		AccountsBaseClient oAccountsBaseClient = null;
		try
		{
			List<MISReportSection> lstMISReportSections = null;
			
			if (IsPopup)
				lstMISReportSections = Session[Constants.S_SESSION_MANAGEMENT_MISREPORT] as List<MISReportSection>;

			if (lstMISReportSections == null)
			{
				oAccountsBaseClient = new AccountsBaseClient();
				oAccountsBaseClient.Open();
				lstMISReportSections = oAccountsBaseClient.GetMISReport(miSchoolId, iFinancialYearId);
			}

			lstvwMISReport.DataSource = lstMISReportSections;
			lstvwMISReport.DataBind();

			if (lstMISReportSections.Count == 0)
			{
				trViewTypeRow.Visible = false;
				trExpandCollapse.Visible = false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : Exception occured while binding MIS Report.");
		}
		finally
		{
			if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
				oAccountsBaseClient.Close();	
		}
	}

	/// <summary>
	/// Sets the text and class properties of a Label.
	/// </summary>
	/// <param name="aoItem"></param>
	/// <param name="asLabelId"></param>
	/// <param name="adAmount"></param>
	/// <param name="abSetClass"> </param>
	private void SetLabelProperties(ListViewDataItem aoItem, string asLabelId, decimal adAmount, bool abSetClass)
	{
		if (aoItem == null)
			return;
		
		var label = aoItem.FindControl(asLabelId) as Label;
		label.Text = CommonUtility.FormatCurrency((adAmount));
		if (abSetClass && adAmount < 0)
			label.CssClass +=  S_NEG_CLASS;
	}

	/// <summary>
	/// Sets the text and class properties of a Label.
	/// </summary>
	/// <param name="aoControl"></param>
	/// <param name="adAmount"></param>
	/// <param name="abSetClass"> </param>
	private void SetLabelProperties(Control aoControl, decimal adAmount, bool abSetClass)
	{
		if (aoControl == null)
			return;
		
		var label = aoControl as Label;
		label.Text = CommonUtility.FormatCurrency((adAmount));
		if (abSetClass && adAmount < 0)
			label.CssClass +=  S_NEG_CLASS;
	}

	/// <summary>
	/// Sets the properties for a hyperlink label.
	/// </summary>
	/// <param name="aoItem"></param>
	/// <param name="asLabelId"></param>
	/// <param name="adAmount"></param>
	/// <param name="asQueryString"></param>
	private void SetLabelProperties(ListViewDataItem aoItem, string asLabelId, decimal adAmount, string asQueryString)
	{
		if (aoItem == null)
			return;
		
		var hyperlink = aoItem.FindControl(asLabelId) as HyperLink;
		hyperlink.Text = CommonUtility.FormatCurrency((adAmount));

		if (!IsPopup)
		{
			hyperlink.NavigateUrl = String.Format("../Accounts/LedgerSummaryUI.aspx?{0}", asQueryString);
			hyperlink.Attributes.Add("onclick", String.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=880,height=650'); return false;", hyperlink.NavigateUrl));
		}
		else
			hyperlink.Enabled = false;
	}

	/// <summary>
	/// Sets the title of a Label.
	/// </summary>
	/// <param name="aoListView"></param>
	/// <param name="asLabelId"></param>
	/// <param name="asTitle"></param>
	private void SetLabelTitle(ListView aoListView, string asLabelId, string asTitle)
	{
		if (aoListView != null)
		{
			var label = aoListView.FindControl(asLabelId) as Label;
			label.Text = asTitle;
		}
	}

	/// <summary>
	/// This method is used to set postback URL.
	/// </summary>
	private void SetPostbackUrl()
	{
		if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
		{
			if (IsFromMgmtDashboard)
			{
				btnBack.PostBackUrl = Constants.S_PAGE_MANAGEMENT_DASHBOARD;
				tblHeader.Visible = false;
			}
			else
			{
				btnBack.PostBackUrl = Constants.S_PAGE_SUPERADMIN_DASHBOARD;
				tblHeader.Visible = true;
			}
		}
		else
		{
            btnBack.Visible = false;
			tblHeader.Visible = false;
		}
	}

	/// <summary>
	///		Hides certain controls on the master page.
	/// </summary>
	private void HideMasterControls()
	{
		var hlnkEmail = this.Master.FindControl("hlnkEmail") as HyperLink;
		if (hlnkEmail != null)
			hlnkEmail.Visible = false;

		var hlnkSupport = this.Master.FindControl("hlnkSupport") as HyperLink;
		if (hlnkSupport != null)
			hlnkSupport.Visible = false;

		var lnkFeedback = this.Master.FindControl("lnkFeedback") as LinkButton;
		if (lnkFeedback != null)
			lnkFeedback.Visible = false;
	}

	#endregion -- PRIVATE METHOD(s) --
}