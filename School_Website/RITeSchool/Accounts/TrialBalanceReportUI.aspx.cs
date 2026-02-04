/* ---------------------------------------------------------------------------------
 *	FileName	: TrialBalanceReportUI.aspx.cs
 *	Author		: Pravin Shinde
 *	Date		: 18-Jul-2013
 *	Description	: This class is used to display trial balance report on dashboard.
 * ---------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using AccountsEntities;
using BusinessLogic.Exceptions;
using System.Linq;
using SchoolBusinessService;
using Utility;

public partial class TrialBalanceReportUI : SchoolBase
{
    #region -- CONSTANT(s) --

    private const string S_TRIAL_BALANCE = "Trial Balance";
    private const string S_GROUP = "G";
    private const string S_LEDGER = "L";
    private const string S_DEBIT = " Dr";
    private const string S_CREDIT = " Cr";
    private const int I_ROW_OPENING_DIFF = -98;
    private const int I_ROW_GRAND_TOTAL = -99;
    private const int I_DEBIT_DIFFRENCE = -1;
    private const int I_GROUP_ID = 1;

    #endregion -- CONSTANT(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Returns true if the Accounts module is enabled, false otherwise.
    /// </summary>
    protected bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    #endregion -- PROPERTIES --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This method is used to load the parent group details and to setting the default values to the controls on the page load.
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
                if (IsAccountsModuleEnabled)
                    StoreFinancialYearDetails();
                GetGroupDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        
    }

    /// <summary>
    /// This event is used to calculate details after the date change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            MenuItem oItem = null;
            if (MenuControl.Items.Count > 0)
            {
                oItem = MenuControl.Items[MenuControl.Items.Count - 1];
                SetDetailsAsPerMenu(oItem);
            }
            else
            {
                for (int iCount = MenuControl.Items.Count - 1; iCount > 0; iCount--)
                {
                    oItem = MenuControl.Items[iCount];
                    MenuControl.Items.Remove(oItem);
                }
            }
            SetNoRecordMessage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to load the details as per the item clicked on menu.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MenuControl_MenuItemClick(object sender, MenuEventArgs e)
    {
        try
        {
            var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
            if (txtStartDate.Text.IsNullOrEmpty())
                txtStartDate.Text = oFinancialYear.StartDate.ToString(Constants.S_DATE_FORMAT);
            if (txtEndDate.Text.IsNullOrEmpty())
                txtEndDate.Text = oFinancialYear.EndDate.ToString(Constants.S_DATE_FORMAT);

            MenuItem oMenuItem = e.Item;
            for (int iCount = MenuControl.Items.Count - 1; iCount >= 0; iCount--)
            {
                MenuItem oItem = MenuControl.Items[iCount];
                if (oItem.Equals(oMenuItem))
                {
                    SetDetailsAsPerMenu(oItem);
                    break;
                }
                else
                    MenuControl.Items.Remove(oItem);
            }
            SetNoRecordMessage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Sets a different classname for alternating items in the ListView and also sets values for non-databound items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroups_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iGroupId = lstvwGroups.DataKeys[iRowId]["Id"].ToInt();
                GroupNature oGroupNature = lstvwGroups.DataKeys[iRowId]["GroupNature"] as GroupNature;
                bool bIsSystemDefined = lstvwGroups.DataKeys[iRowId]["IsSystemDefined"].ToBool();
                var lblDebit = oCurrentItem.FindControl("lblDebit") as Label;
                var lblCredit = oCurrentItem.FindControl("lblCredit") as Label;
                var lblGrandTotal = oCurrentItem.FindControl("lblGrandTotal") as Label;
                var lnkGroupName = oCurrentItem.FindControl("lnkGroupName") as LinkButton;

                var trGridRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;

                //if (bIsSystemDefined)
                 //   lnkGroupName.Enabled = false;

                if (iGroupId == I_ROW_OPENING_DIFF)
                {
                    lnkGroupName.Visible = false;
                    lblGrandTotal.Visible = true;
                    lblGrandTotal.Font.Bold = true;
                    lblDebit.Font.Bold = true;
                    lblCredit.Font.Bold = true;
                    if (oGroupNature.Id == I_DEBIT_DIFFRENCE)
                        lblDebit.Visible = false;
                    else
                        lblCredit.Visible = false;
                    trGridRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C6D0AB");

                }
                else if (iGroupId == I_ROW_GRAND_TOTAL)
                {
                    lblDebit.Font.Bold = true;
                    lblCredit.Font.Bold = true;
                    lnkGroupName.Visible = false;
                    lblGrandTotal.Visible = true;
                    trGridRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C6D0AB");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Handles item command in the ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroups_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.CommandName == "GETLEDGERS")
                {
                    var oCurrentItem = e.Item as ListViewDataItem;
                    int iRowId = oCurrentItem.DisplayIndex;
                    int iGroupId = lstvwGroups.DataKeys[iRowId]["Id"].ToInt();
                    var lnkGroupName = oCurrentItem.FindControl("lnkGroupName") as LinkButton;
                    GetGroupDetails(iGroupId);
                    MenuControl.Items.Add(new MenuItem { Text = " >> " + lnkGroupName.Text, Value = S_GROUP + iGroupId.ToString() });

                    GetLedgerDetails(iGroupId);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the different properties according to the values binded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLedger_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iLedgerId = lstvwLedger.DataKeys[iRowId]["Id"].ToInt();
                var lblGrandTotal = oCurrentItem.FindControl("lblGrandTotal") as Label;
                var lnkLedgerName = oCurrentItem.FindControl("lnkLedgerName") as LinkButton;
                var lblDebit = oCurrentItem.FindControl("lblDebit") as Label;
                var lblCredit = oCurrentItem.FindControl("lblCredit") as Label;
                var trGridRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
                if (iLedgerId == I_ROW_GRAND_TOTAL)
                {
                    lblDebit.Font.Bold = true;
                    lblCredit.Font.Bold = true;
                    lnkLedgerName.Visible = false;
                    lblGrandTotal.Visible = true;
                    lblGrandTotal.Font.Bold = true;
                    trGridRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C6D0AB");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the command event on click of ledger in the listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLedger_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.CommandName == "MONTHWISEDETAILS")
                {
                    var oCurrentItem = e.Item as ListViewDataItem;
                    int iRowId = oCurrentItem.DisplayIndex;
                    int iLedgerId = lstvwLedger.DataKeys[iRowId]["Id"].ToInt();
                    int iGroupId = lstvwLedger.DataKeys[iRowId]["GroupId"].ToInt();
                    var lnkLedgerName = oCurrentItem.FindControl("lnkLedgerName") as LinkButton;
                    MenuControl.Items.Add(new MenuItem { Text = " >> " + lnkLedgerName.Text, Value = S_LEDGER + iLedgerId.ToString() + "," + iGroupId.ToString() });
                    GroupBody.Visible = false;
                    LedgerBody.Visible = false;
                    GetMonthlyDetails(iGroupId, iLedgerId);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Sets a different classname for alternating items in the ListView and also sets values for non-databound items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMonthwiseDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iMonthId = lstvwMonthwiseDetails.DataKeys[iRowId]["MonthId"].ToInt();
                var lblGrandTotal = oCurrentItem.FindControl("lblGrandTotal") as Label;
                var lnkMonthName = oCurrentItem.FindControl("lnkMonthName") as HyperLink;
                var lblDebit = oCurrentItem.FindControl("lblDebit") as Label;
                var lblCredit = oCurrentItem.FindControl("lblCredit") as Label;
                var lblClosingBalance = oCurrentItem.FindControl("lblClosingBalance") as Label;
                string sClosingBalance = lstvwMonthwiseDetails.DataKeys[iRowId]["ClosingBalance"].ToString();
                var trGridRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
                if (iMonthId == I_ROW_GRAND_TOTAL || iMonthId == I_ROW_OPENING_DIFF)
                {
                    lnkMonthName.Visible = false;
                    lblGrandTotal.Visible = true;
                    if (iMonthId == I_ROW_GRAND_TOTAL)
                    {
                        lblGrandTotal.Font.Bold = true;
                        lblDebit.Font.Bold = true;
                        lblCredit.Font.Bold = true;
                    }
                    trGridRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C6D0AB");

                    if (!sClosingBalance.IsNullOrEmpty())
                    {
                        if (sClosingBalance.ToDouble() < 0.0)
                            lblClosingBalance.Text = CommonUtility.FormatCurrency(sClosingBalance.ToDouble() * (-1).ToDouble()) + S_DEBIT;
                        else
                            lblClosingBalance.Text = CommonUtility.FormatCurrency(sClosingBalance) + S_CREDIT;
                    }
                }
                else
                {
                    
                    DateTime dtStartDate = lstvwMonthwiseDetails.DataKeys[iRowId]["StartDate"].ToDateTime();
                    DateTime dtEndDate = lstvwMonthwiseDetails.DataKeys[iRowId]["EndDate"].ToDateTime();
                    int iLedgerId = lstvwMonthwiseDetails.DataKeys[iRowId]["LedgerId"].ToInt();

                    string sQueryString = string.Format("LedgerId={0}&From={1}&To={2}", iLedgerId, dtStartDate,dtEndDate);
                    string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                    lnkMonthName.NavigateUrl = String.Format("../Accounts/LedgerSummaryUI.aspx?{0}", sEncrypt);
                    lnkMonthName.Attributes.Add("onclick",String.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=650'); return false;",lnkMonthName.NavigateUrl));
                    if (!sClosingBalance.IsNullOrEmpty())
                    {
                        if (sClosingBalance.ToDouble() < 0.0)
                            lblClosingBalance.Text = CommonUtility.FormatCurrency((sClosingBalance.ToDouble() * (-1).ToDouble())) + S_DEBIT;
                        else
                            lblClosingBalance.Text = CommonUtility.FormatCurrency(sClosingBalance) + S_CREDIT;
                    }
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
    /// This procedure is used to set No Record found message if no data found in any of the list view.
    /// </summary>
    private void SetNoRecordMessage()
    {
        if (GroupBody.Visible == false && LedgerBody.Visible == false && MonthwiseDetailsBody.Visible == false)
            tblNoRec.Visible = true;
        else
            tblNoRec.Visible = false;
    }

    /// <summary>
    /// This method is used to load all the group-subgroup details as per the selection.
    /// </summary>
    /// <param name="aiGroupId"></param>
    private void GetGroupDetails(int aiGroupId = 0)
    {
        var oAccountGroupClient = new AccountGroupClient();
        oAccountGroupClient.Open();
        List<Group> lstGroups = oAccountGroupClient.GetAllGroupDetails(miSchoolId, miFinancialYearId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), aiGroupId);
        if (lstGroups.Count > 0)
        {
            GroupBody.Visible = true;
            lstvwGroups.DataSource = lstGroups;
        }
        else
            GroupBody.Visible = false;
        lstvwGroups.DataBind();

        if (oAccountGroupClient != null && oAccountGroupClient.State != CommunicationState.Faulted)
            oAccountGroupClient.Close();
    }

    /// <summary>
    /// This method is used to fill all the ledger details as per the selection of a group.
    /// </summary>
    /// <param name="aiGroupId"></param>
    private void GetLedgerDetails(int aiGroupId)
    {
        var oAccountLedgerClient = new AccountLedgerClient(); 
        oAccountLedgerClient.Open(); ;
        List<Ledger> lstLedgers = oAccountLedgerClient.GetAllLedgerDetails(miSchoolId, miFinancialYearId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), aiGroupId);
        var oLedgers = from ledger in lstLedgers
                       select new
                                  {
                                      ledger.Id,
                                      ledger.Name,
                                      GroupId = ledger.Group.Id,
                                      Debit = ledger.Group.Debit,
                                      Credit = ledger.Group.Credit,
                                      ledger.IsSystemDefined,
                                      ledger.OpeningBalance,
                                      ledger.ClosingBlanace,
                                      ledger.Budget
                                  };
        if (oLedgers.Count() > 0)
        {
            LedgerBody.Visible = true;
            lstvwLedger.DataSource = oLedgers;
        }
        else
            LedgerBody.Visible = false;
        lstvwLedger.DataBind();

        if (oAccountLedgerClient != null && oAccountLedgerClient.State != CommunicationState.Faulted)
            oAccountLedgerClient.Close();
    }

    /// <summary>
    /// This event is used to fill all the monthwise ledger details as per the selection of a ledger. 
    /// </summary>
    /// <param name="aiGroupId"></param>
    /// <param name="aiLedgerId"></param>
    private void GetMonthlyDetails(int aiGroupId, int aiLedgerId)
    {
        var oAccountsBaseClient = new AccountsBaseClient();
        oAccountsBaseClient.Open();

        List<MonthlyTrialBalance> lstMonthlyTrialBalance = oAccountsBaseClient.GetMonthlyLedgerDetails(miSchoolId, miFinancialYearId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), aiLedgerId);
        var oMonthlyTrialBalance = from month in lstMonthlyTrialBalance
                                   select new
                                   {
                                       month.MonthId,
                                       month.MonthName,
                                       month.StartDate,
                                       month.EndDate,
                                       GroupId = aiGroupId,
                                       LedgerId = aiLedgerId,
                                       ClosingBalance = month.oGroup.Ledgers.Select(a => a.ClosingBlanace).FirstOrDefault(),
                                       Debit = month.oGroup.Debit,
                                       Credit = month.oGroup.Credit
                                   };
        MonthwiseDetailsBody.Visible = true;

        if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
            oAccountsBaseClient.Close();

        lstvwMonthwiseDetails.DataSource = oMonthlyTrialBalance;
        lstvwMonthwiseDetails.DataBind();
        SetNoRecordMessage();
    }

    /// <summary>
    /// This function is used to initialize controls to their default values.
    /// </summary>
    private void Initialize()
    {
        ApplyMouseHoverEffect(new List<Button> { btnShow });
        SetDefaultButton(btnShow);
        valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        txtStartDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        MenuControl.Items.Add(new MenuItem { Text = S_TRIAL_BALANCE, Value = S_GROUP + Constants.S_ZERO });
        LedgerBody.Visible = false;
        MonthwiseDetailsBody.Visible = false;
    }

    /// <summary>
    /// This is a private method used to set the details as per the menu selection.
    /// </summary>
    /// <param name="aoItem"></param>
    private void SetDetailsAsPerMenu(MenuItem aoItem)
    {
        string sCase = S_LEDGER;
        if (aoItem.Value.Contains(S_GROUP))
            sCase = S_GROUP;

        switch (sCase)
        {
            case S_LEDGER:
                GroupBody.Visible = false;
                MonthwiseDetailsBody.Visible = false;
                string[] sArrId = aoItem.Value.Split(',');
                int iLedgerId = sArrId[0].Substring(1).ToInt();
                GetMonthlyDetails(sArrId[I_GROUP_ID].ToInt(), iLedgerId);
                break;

            case S_GROUP:
                GroupBody.Visible = false;
                MonthwiseDetailsBody.Visible = false;
                GetLedgerDetails(aoItem.Value.Substring(1).ToInt());
                GetGroupDetails(aoItem.Value.Substring(1).ToInt());
                if (aoItem.Value.Substring(1).ToInt() == 0)
                {
                    LedgerBody.Visible = false;
                    GroupBody.Visible = true;
                    
                }
                break;
        }
    }

    /// <summary>
    /// Serializes the FinancialYearMaster entity object to a hidden field.
    /// </summary>
    private void StoreFinancialYearDetails()
    {
        var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
        if (oFinancialYear != null)
        {
            var jsSerializer = new JavaScriptSerializer();
            hidFinancialYearJSON.Value = jsSerializer.Serialize(oFinancialYear);
            txtStartDate.Text = oFinancialYear.StartDate.ToString(Constants.S_DATE_FORMAT);
            txtEndDate.Text = oFinancialYear.EndDate.ToString(Constants.S_DATE_FORMAT);
        }

        if (Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] != null)
            hidCanEditOldFinancialYear.Value = Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR].ToString().ToLower();
    }

    #endregion -- PRIVATE METHOD(s) --
}