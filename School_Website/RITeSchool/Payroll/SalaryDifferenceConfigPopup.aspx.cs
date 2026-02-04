/* File Name - SalaryDifferenceConfigPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 21 Nov 2012
 * Description - This class is used to configure salary difference details.
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

public partial class SalaryDifferenceConfigPopup : SchoolBase
{
    #region Data Member(s)

    private SalaryDifferenceBL moSalaryDifferenceBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill earning deduction grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                FillEarningDeductions(false);
                SetDefaulValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill formula combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEarningDeduction_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                SalaryDifferenceConfigDetails oSalaryDifferenceConfigDetails = oCurrentItem.DataItem as SalaryDifferenceConfigDetails;

                CheckBox ChkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
                ChkSelect.Attributes.Add("onclick", "DisableControls(this," + iRowId + ")");
                ChkSelect.Checked = oSalaryDifferenceConfigDetails.IsConfigured;

                DropDownList cmbEarnDeductFormula = oCurrentItem.FindControl("cmbEarnDeductFormula") as DropDownList;
                if (cmbEarnDeductFormula != null)
                {
                    ChkSelect.CheckedChanged += new EventHandler(ChkSelect_CheckedChanged);
                    List<EarningsDeductionsFormulae> lstSelectedFormulae = moSalaryDifferenceBL.EarningDeductionFormulaBL.EarningsDeductionsFormulae
                                                                           .Where(ed => ed.EarningsDeductionsId == oSalaryDifferenceConfigDetails.EarningsDeductionsId).ToList();
                    List<AmountRange> lstAmountRanges = moSalaryDifferenceBL.AmountRangeBL.AmountRanges
                                                        .Where(ar => ar.EarningsDeductionsId == oSalaryDifferenceConfigDetails.EarningsDeductionsId)
                                                        .Distinct()
                                                        .ToList();

                    if (lstSelectedFormulae.Count > 0)
                    {
                        ListSource.FillDropDownList(lstSelectedFormulae, cmbEarnDeductFormula, "FormulaName", "FormulaId", string.Empty);
                        cmbEarnDeductFormula.SelectedValue = optSaved.Checked ? oSalaryDifferenceConfigDetails.FormulaRangeId.ToString()
                                                                : lstSelectedFormulae.Where(ed => ed.IsDefault).FirstOrDefault().FormulaId.ToString();
                    }
                    else if (lstAmountRanges.Count > 0)
                    {
                        ListSource.FillDropDownList(lstAmountRanges, cmbEarnDeductFormula, "RangeName", "RangeId", string.Empty);
                        cmbEarnDeductFormula.SelectedValue = optSaved.Checked ? oSalaryDifferenceConfigDetails.FormulaRangeId.ToString()
                                                                : lstAmountRanges.Where(ed => ed.IsDefault).FirstOrDefault().RangeId.ToString();
                    }
                    else
                        cmbEarnDeductFormula.Visible = false;

                    if (oSalaryDifferenceConfigDetails.IsConfigured)
                        ChkSelect_CheckedChanged(ChkSelect, null);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show all the users that are associated with this formula explicitely.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ChkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkSelect = sender as CheckBox;
            ListViewDataItem oCurrentItem = ChkSelect.Parent.Parent.Parent as ListViewDataItem;
            if (ChkSelect.Checked)
            {
                DropDownList cmbEarnDeductFormula = oCurrentItem.FindControl("cmbEarnDeductFormula") as DropDownList;
                if (cmbEarnDeductFormula != null && cmbEarnDeductFormula.Visible)
                {
                    HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trlstvwUsers") as HtmlTableRow;
                    HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdlstvwUsers") as HtmlTableCell;
                    bool bIsUserPresent = DisplayUserDetails(oCurrentItem, oHtmlTableCell);
                    oHtmlTableRow.Visible = bIsUserPresent;
                }
            }
            else
                HideUserDetails(oCurrentItem);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select all the earning and deductions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ChkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ChkSelect_CheckedChanged(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select user associated formula.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                UsersFormulaAndRanges oUsersFormulaAndRanges = oCurrentItem.DataItem as UsersFormulaAndRanges;
                DropDownList cmbEarnDeduct = oCurrentItem.FindControl("cmbEarnDeduct") as DropDownList;
                if (cmbEarnDeduct != null)
                {
                    List<EarningsDeductionsFormulae> lstEarningsDeductionsFormulae = moSalaryDifferenceBL.EarningDeductionFormulaBL.EarningsDeductionsFormulae;
                    List<EarningsDeductionsFormulae> lstSelectedFormulae = lstEarningsDeductionsFormulae.Where(ed => ed.EarningsDeductionsId == Convert.ToInt32(hidEarningDeductionId.Value)).ToList();
                    if (lstSelectedFormulae.Count > 0)
                    {
                        ListSource.FillDropDownList(lstSelectedFormulae, cmbEarnDeduct, "FormulaName", "FormulaId", string.Empty);
                        cmbEarnDeduct.SelectedValue = oUsersFormulaAndRanges.FormulaRangeId.ToString();
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
    /// This event is used to save configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sXml = getXml();
            SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, miAcademicYearId, miUserId);
            oSalaryDifferenceBL.SaveConfig(sXml, 0, miUserId);
            hidQueryString.Value = string.Format("'?{0}'", hidQueryString.Value);
            Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", hidQueryString.Value));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show default configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optDefault_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillEarningDeductions(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show saved configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optSaved_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillEarningDeductions(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to save default values.
    /// </summary>
    private void SetDefaulValues()
    {
        ApplyMouseHoverEffect(new List<Button> { BtnSave, BtnClose });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidQueryString.Value = Request.QueryString.ToString();
        lblYear.Text = QueryString["Year"].ToString();        
        lblMonth.Text = new DateTime(Convert.ToInt32(QueryString["Year"]), Convert.ToInt32(QueryString["MonthId"]), 1).ToString("MMMM");
    }

    /// <summary>
    /// This method is used to fill earning deductions in listview.
    /// </summary>
    /// <param name="abShowDefault"></param>
    private void FillEarningDeductions(bool abShowDefault)
    {
        moSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, miAcademicYearId, miUserId);
        moSalaryDifferenceBL.GetSalaryDifferenceConfigDetails(abShowDefault, Convert.ToInt32(QueryString["MonthId"]), Convert.ToInt32(QueryString["Year"]));
        List<SalaryDifferenceConfigDetails> lstSalaryDifferenceConfigDetails = moSalaryDifferenceBL.UsersEarningsDeductionsBL.SalaryDifferenceConfigDetails;

        if (!abShowDefault)
        {
            if (lstSalaryDifferenceConfigDetails.FindAll(ED => ED.IsConfigured).Count == 0)
            {
                if (!optDefault.Checked)
                {
                    optSaved.Enabled = false;
                    optDefault.Checked = true;
                    moSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, miAcademicYearId, miUserId);
                    moSalaryDifferenceBL.GetSalaryDifferenceConfigDetails(true, Convert.ToInt32(QueryString["MonthId"]), Convert.ToInt32(QueryString["Year"]));
                    lstSalaryDifferenceConfigDetails = moSalaryDifferenceBL.UsersEarningsDeductionsBL.SalaryDifferenceConfigDetails;
                }
            }
            else
            {
                optSaved.Enabled = true;
                optSaved.Checked = true;
            }
        }

        lstvwEarningDeduction.DataSource = lstSalaryDifferenceConfigDetails.Where(ed => ed.UserId == 0);
        lstvwEarningDeduction.DataBind();
    }

    /// <summary>
    /// This method is used to hide user listview.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void HideUserDetails(ListViewDataItem oCurrentItem)
    {
        if (oCurrentItem != null)
        {
            HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trlstvwUsers") as HtmlTableRow;
            if (oHtmlTableRow != null)
                oHtmlTableRow.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to reset user details.
    /// </summary>
    private void ResetUserDetails()
    {
        foreach (ListViewItem oCurrewntItem in lstvwEarningDeduction.Items)
        {
            HtmlTableRow oHtmlTableRow = oCurrewntItem.FindControl("trlstvwUsers") as HtmlTableRow;
            if (oHtmlTableRow != null)
                oHtmlTableRow.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to display user details.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    /// <param name="aoHtmlTableCell"></param>
    /// <returns></returns>
    private bool DisplayUserDetails(ListViewDataItem aoCurrentItem, HtmlTableCell aoHtmlTableCell)
    {
        int iRowId = Convert.ToInt32(aoCurrentItem.DisplayIndex);
        int iEarningDeductionId = Convert.ToInt32(lstvwEarningDeduction.DataKeys[iRowId]["EarningsDeductionsId"]);
        hidEarningDeductionId.Value = iEarningDeductionId.ToString();

        moSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, miAcademicYearId, miUserId);
        moSalaryDifferenceBL.GetSalaryDifferenceConfigDetails(optDefault.Checked, Convert.ToInt32(QueryString["MonthId"]), Convert.ToInt32(QueryString["Year"]));

        List<UsersFormulaAndRanges> lstUsers = (from userFormula in moSalaryDifferenceBL.UsersEarningsDeductionsBL.UsersFormulaAndRanges
                                                join formula in moSalaryDifferenceBL.EarningDeductionFormulaBL.EarningsDeductionsFormulae
                                               on userFormula.FormulaRangeId equals formula.FormulaId
                                                where formula.EarningsDeductionsId == iEarningDeductionId
                                                select userFormula).ToList();

        ListView lstvwUsers = aoHtmlTableCell.FindControl("lstvwUsers") as ListView;
        lstvwUsers.DataSource = lstUsers;
        lstvwUsers.DataBind();
        return lstUsers.Count > 0;
    }

    /// <summary>
    /// This methd is used to save configuration xml.
    /// </summary>
    /// <returns></returns>
    private string getXml()
    {
        List<SalaryDifferenceConfigDetails> lstSalaryDifferenceConfigDetails = new List<SalaryDifferenceConfigDetails>();
        foreach (ListViewDataItem oCurrentItem in lstvwEarningDeduction.Items)
        {
            SalaryDifferenceConfigDetails oSalaryDifferenceConfigDetails;
            CheckBox ChkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            if (ChkSelect.Checked)
            {
                oSalaryDifferenceConfigDetails = new SalaryDifferenceConfigDetails();

                int iEarningDeductionId = Convert.ToInt32(lstvwEarningDeduction.DataKeys[oCurrentItem.DisplayIndex]["EarningsDeductionsId"]);

                oSalaryDifferenceConfigDetails.EarningsDeductionsId = iEarningDeductionId;
                oSalaryDifferenceConfigDetails.FormulaRangeId = 0;
                oSalaryDifferenceConfigDetails.UserId = 0;

                DropDownList cmbEarnDeductFormula = oCurrentItem.FindControl("cmbEarnDeductFormula") as DropDownList;
                if (cmbEarnDeductFormula != null && cmbEarnDeductFormula.Visible)
                {
                    oSalaryDifferenceConfigDetails.FormulaRangeId = Convert.ToInt32(cmbEarnDeductFormula.SelectedValue);
                    lstSalaryDifferenceConfigDetails.Add(oSalaryDifferenceConfigDetails);

                    HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trlstvwUsers") as HtmlTableRow;
                    if (oHtmlTableRow != null)
                    {
                        HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdlstvwUsers") as HtmlTableCell;
                        if (oHtmlTableCell != null)
                        {
                            ListView lstvwUsers = oHtmlTableCell.FindControl("lstvwUsers") as ListView;
                            SalaryDifferenceConfigDetails oSalaryDiffConfigDtl = new SalaryDifferenceConfigDetails();
                            foreach (ListViewDataItem oListViewDataItem in lstvwUsers.Items)
                            {
                                DropDownList cmbEarnDeduct = oListViewDataItem.FindControl("cmbEarnDeduct") as DropDownList;
                                int iFormulaRangeId = Convert.ToInt32(lstvwUsers.DataKeys[oListViewDataItem.DisplayIndex]["FormulaRangeId"]);
                                int iUserId = Convert.ToInt32(lstvwUsers.DataKeys[oListViewDataItem.DisplayIndex]["UserId"]);

                                int iSelectedFormulaRangeId = Convert.ToInt32(cmbEarnDeduct.SelectedValue);

                                oSalaryDiffConfigDtl = new SalaryDifferenceConfigDetails
                                {
                                    EarningsDeductionsId = iEarningDeductionId,
                                    FormulaRangeId = iSelectedFormulaRangeId,
                                    UserId = iUserId
                                };
                                lstSalaryDifferenceConfigDetails.Add(oSalaryDiffConfigDtl);
                            }
                        }
                    }
                }
                else
                    lstSalaryDifferenceConfigDetails.Add(oSalaryDifferenceConfigDetails);
            }
        }
        string sXml = GenerateXml(lstSalaryDifferenceConfigDetails);
        return sXml;
    }

    #endregion

}