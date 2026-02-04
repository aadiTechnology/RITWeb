/* File Name - SavedSalaryDifferencePopup.aspx.cs
 * Created By - Sachin
 * Created Date - 21 Nov 2012
 * Description - This class is used to show salary difference in details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class SavedSalaryDifferencePopup : SchoolBase
{

    #region Constant(s)

    private const string S_LEAVE_DEDUCTED = "Leave Deducted "; 

    #endregion

    #region Data Member(s)

    List<SavedSalaryDifference> mlstSavedSalaryDifference;
    int miUsrId = 0; 

    #endregion

    #region Event(s)

    /// <summary>
    /// TYhis event is used to fill salary difference listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                bool bShowPaid = SetDefaultValues();
                FillSavedSalaryDifference(bShowPaid);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show paid salary difference.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPaid_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillSavedSalaryDifference(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show saved salary difference.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optSaved_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillSavedSalaryDifference(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete last transaction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEarningDeduction_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DELETE")
            {
                int iSalaryDifferenceId = Convert.ToInt32(lstvwEarningDeduction.DataKeys[e.Item.DisplayIndex]["SalaryDifferenceId"]);
                SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, 0, miUserId);
                oSalaryDifferenceBL.DeleteLastTransaction(iSalaryDifferenceId, miUserId);

                lblmessage.Text = "Last transaction has been deleted successfully!!!";
                lblmessage.Visible = true;
                FillSavedSalaryDifference(false);
            }
            else if (e.CommandName == "DETAILS")
            {   
                DisplayDetailListview(e.Item as ListViewDataItem);             
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle delete condition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEarningDeduction_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        try
        {
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// This event is used to close popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            hidQueryString.Value = string.Format("'?{0}'", hidQueryString.Value);
            Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", hidQueryString.Value));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    private void GetSavedSalaryDifference(bool abShowPaid)
    {
        int iMonthId = Convert.ToInt32(QueryString["MonthId"]);
        int iYear = Convert.ToInt32(QueryString["Year"]);
		SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, 0, miUserId);
        mlstSavedSalaryDifference = oSalaryDifferenceBL.GetSavedSalaryDifferenceDetails(iMonthId, iYear, abShowPaid);
    }

    /// <summary>
    /// This method is used to fill salary difference details listview.
    /// </summary>
    /// <param name="abShowPaid"></param>
    private void FillSavedSalaryDifference(bool abShowPaid)
    {
        GetSavedSalaryDifference(abShowPaid);

        var oSalDiffAmountDetails = mlstSavedSalaryDifference.Where(SD => SD.EarningDeductionName == PayrollConstants.S_NET_SALARY).GroupBy(SD => SD.SalaryDifferenceId).Select(SD => new { SalaryDifferenceId = SD.Key, Amount = SD.Sum(SD1 => SD1.Amount) });

        var oSavedSalaryDifference = mlstSavedSalaryDifference.Select(salDiff =>
                        new
                        {
                            SalaryDifferenceId = salDiff.SalaryDifferenceId,
                            UserId = salDiff.UserId,
                            UserName = salDiff.UserName,
                            Designation = salDiff.Designation,
                            Amount = oSalDiffAmountDetails.Where(SD2 => SD2.SalaryDifferenceId == salDiff.SalaryDifferenceId).Select(SD1 => SD1.Amount).FirstOrDefault()
                        }).Distinct();

        int iUserId = Convert.ToInt32(QueryString["UserId"]);
        if (iUserId != 0)
            oSavedSalaryDifference = oSavedSalaryDifference.Where(salDiff => salDiff.UserId == iUserId);

        int iEarningDeductionId = Convert.ToInt32(QueryString["EarningDeductionId"]);
        if (iEarningDeductionId != 0)
            oSavedSalaryDifference = from salDiff in oSavedSalaryDifference
                                       join diff in mlstSavedSalaryDifference
                                       on salDiff.SalaryDifferenceId equals diff.SalaryDifferenceId
                                       where diff.EarningDeductionId == iEarningDeductionId
                                       && !diff.EarningDeductionName.Contains(S_LEAVE_DEDUCTED)
                                       select salDiff;

        lstvwEarningDeduction.DataSource = oSavedSalaryDifference;
        lstvwEarningDeduction.DataBind();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    /// <returns></returns>
    private bool SetDefaultValues()
    {
        bool bShowPaid = Convert.ToBoolean(Convert.ToInt32(QueryString["ShowPaid"]));
        optPaid.Checked = bShowPaid;
        optSaved.Checked = !bShowPaid;
        hidQueryString.Value = Request.QueryString.ToString();
        ApplyMouseHoverEffect(new List<Button> { BtnClose });
        
        lblYear.Text = QueryString["Year"].ToString();
        lblMonth.Text = new DateTime(Convert.ToInt32(QueryString["Year"]), Convert.ToInt32(QueryString["MonthId"]), 1).ToString("MMMM");

        return bShowPaid;
    }

    /// <summary>
    /// This method is used to set delete button state.
    /// </summary>
    /// <param name="aoHtmlTableCell"></param>
    /// <param name="aiSalaryDifferenceId"></param>
    private void SetDeleteButtonState(HtmlTableCell aoHtmlTableCell, int aiSalaryDifferenceId)
    {
        Button btnDelete = aoHtmlTableCell.FindControl("btnDelete") as Button;
        ApplyMouseHoverEffect(new List<Button> { btnDelete });
        btnDelete.Visible = false;
        if (optSaved.Checked && mlstSavedSalaryDifference.FindAll(ED => ED.IsLastTransaction && ED.SalaryDifferenceId == aiSalaryDifferenceId).Count > 0 && Convert.ToInt32(QueryString["EarningDeductionId"]) == 0)
        {
            btnDelete.Visible = true;
            btnDelete.Attributes.Add("onclick", "if(!confirm('Are you sure you want to delete this transaction?')) return false;");
        }
    }

    /// <summary>
    /// This method is used to display details listview.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="alstvwEarnDeduct"></param>
    /// <param name="alstSavedSalaryDifference"></param>
    /// <param name="aiUserId"></param>
    /// <param name="aiEarningDeductionId"></param>
    private void DisplayDetailListview(ListViewDataItem aoCurrentItem)
    {
        GetSavedSalaryDifference(optPaid.Checked);
        ResetUserDetails();

        HtmlTableRow oHtmlTableRow = aoCurrentItem.FindControl("trlstvwEarnDeducts") as HtmlTableRow;
        HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdlstvwEarnDeducts") as HtmlTableCell;

        if (oHtmlTableRow != null)
            oHtmlTableRow.Visible = true;

        int iSalaryDifferenceId = Convert.ToInt32(lstvwEarningDeduction.DataKeys[aoCurrentItem.DisplayIndex]["SalaryDifferenceId"]);
        var lstSavedSalaryDifference = mlstSavedSalaryDifference.Where(salDiff => salDiff.SalaryDifferenceId == iSalaryDifferenceId);

        var oUserSalDifference = lstSavedSalaryDifference;
        int iUserId = Convert.ToInt32(lstvwEarningDeduction.DataKeys[aoCurrentItem.DisplayIndex]["UserId"]);
        if (iUserId != 0)
            oUserSalDifference = lstSavedSalaryDifference.Where(salDiff => salDiff.UserId == iUserId);

        int iEarningDeductionId = Convert.ToInt32(QueryString["EarningDeductionId"]);
        if (iEarningDeductionId != 0)
            lstSavedSalaryDifference = oUserSalDifference.Where(salDiff => salDiff.EarningDeductionId == iEarningDeductionId);

        ListView lstvwEarnDeduct = oHtmlTableCell.FindControl("lstvwEarnDeduct") as ListView;

        if (lstSavedSalaryDifference.Count() > 0)
        {
            lstvwEarnDeduct.DataSource = lstSavedSalaryDifference;
            lstvwEarnDeduct.DataBind();

            if (miUsrId == iUserId)
            {
                HtmlTableRow Tr2 = aoCurrentItem.FindControl("Tr2") as HtmlTableRow;
                if (Tr2 != null)
                    Tr2.Visible = false;

                HtmlTableRow Tr3 = aoCurrentItem.FindControl("Tr3") as HtmlTableRow;
                if (Tr3 != null)
                    Tr3.Visible = false;
            }
            else
                miUsrId = iUserId;
        }
        else
        {
            if (iEarningDeductionId != 0)
                aoCurrentItem.Visible = false;
        }

        SetDeleteButtonState(oHtmlTableCell, iSalaryDifferenceId);
    }

    /// <summary>
    /// This method is used to reset user details.
    /// </summary>
    private void ResetUserDetails()
    {
        foreach (ListViewItem oCurrewntItem in lstvwEarningDeduction.Items)
        {
            HtmlTableRow oHtmlTableRow = oCurrewntItem.FindControl("trlstvwEarnDeducts") as HtmlTableRow;
            if (oHtmlTableRow != null)
                oHtmlTableRow.Visible = false;
        }
    }

    #endregion
}