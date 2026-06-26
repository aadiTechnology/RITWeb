// File Name - IncomeTaxDetailsUI.aspx.cs
// Creator - Pravin
// Created Date - 
// Description - This class is used to publish income tax details.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class IncomeTaxDetailsUI : SchoolBase
{ 
    #region Constant(s)

    private const string S_PUBLISH = "Publish";
    private const string S_UNPUBLISH = "Unpublish";
    private const string S_PUBLISH_SUCCESS = "Income Tax Details for all users published successfully !!!";
    private const string S_UNPUBLISH_SUCCESS = "Income Tax Details for all users unpublished successfully !!!";

    #endregion

    #region Data Member(s)

    private IncomeTaxDetailsBL moIncomeTaxDetailsBL;
    
    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill tax deduction in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                CheckIsPublished();
                SetDefaultValues();
                FillStaffGroups();
                FillUsers();  
                ReadQueryString();                
            }                    
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill users in staff group combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidName.Value = txtUserName.Text;
            DataPager oDtPgDropDown = lstvwTaxDetails.FindControl("DtPgDropDown") as DataPager;
            if (oDtPgDropDown != null)
            {
                DropDownList oddlCnt = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
                if (oddlCnt != null)
                {
                    if (!oddlCnt.SelectedValue.IsNullOrEmpty())
                    {
                        if(oddlCnt.Items.Count>=1)
                            oddlCnt.SelectedIndex = 0;
                        cmbPageCnt_SelectedIndexChanged(oddlCnt, null);
                    }
                }
            }
            lstvwTaxDetails.DataBind();            
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reset fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            cmbStaffGroups.SelectedValue = Constants.S_ZERO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is  used to publish income tax configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        bool aiIsPublished = false;
        if (btnPublish.Text.ToString() == S_PUBLISH)
        {
            aiIsPublished = true;
            btnPublish.Text = S_UNPUBLISH;
            lblMessage.Text = S_PUBLISH_SUCCESS;
        }
        else
        {
            btnPublish.Text = S_PUBLISH;
            lblMessage.Text = S_UNPUBLISH_SUCCESS;
        }

        moIncomeTaxDetailsBL.Publish(aiIsPublished);
        FillUsers();
    }

    /// <summary>
    /// This method is used to add attribute on heperkinks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTaxDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            btnPublish.Enabled = true;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {               
                HyperLink lnkInvestmentDeclaration = e.Item.FindControl("lnkInvestmentDeclaration") as HyperLink;
                HyperLink lnkTDSDetails = e.Item.FindControl("lnkTDSDetails") as HyperLink;
                string sUserId = lstvwTaxDetails.DataKeys[iRowId]["UserId"].ToString();
                bool bIsPublished = lstvwTaxDetails.DataKeys[iRowId]["IsPublished"].ToBool();                
                string sStaffGroupId = cmbStaffGroups.SelectedValue;

                if (!bIsPublished)
                {
                    HtmlTableRow oHtmlTableHeaderRow = e.Item.FindControl("Tr2") as HtmlTableRow;
                    oHtmlTableHeaderRow.Style.Add("background-color", "#FFCCCC");

                    foreach (HtmlTableCell cell in oHtmlTableHeaderRow.Cells)
                    {
                        cell.Style.Add("background-color", "#FFCCCC");
                    }
                }

                string sQueryString = "UserId=" + sUserId + "&StaffGroupId=" + sStaffGroupId + "&PageIndex="+hidPageNo.Value.ToString()+"&UserName="+hidName.Value.ToString();
				string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                lnkInvestmentDeclaration.NavigateUrl = lnkInvestmentDeclaration.NavigateUrl + sEncrypt;
                lnkTDSDetails.NavigateUrl = lnkTDSDetails.NavigateUrl + sEncrypt;               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is to set the listview footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTaxDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTaxDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwTaxDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used set paging for listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTaxDetails);
            DataPager oDataPager = lstvwTaxDetails.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager != null)
            {
                DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                if (ddlCnt != null)
                    hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
            }

            FillUsers();
            hidName.Value = txtUserName.Text;           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to search the selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hidName.Value = txtUserName.Text;
        FillUsers();
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This function is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;
        if (!QueryString["PageIndex"].IsNull())
            hidPageNo.Value = QueryString["PageIndex"];

        if (!QueryString["UserName"].IsNull())
        {
            hidName.Value = QueryString["UserName"];
            txtUserName.Text = hidName.Value;
        }

        if (!QueryString["StaffGroupId"].IsNullOrEmpty())
            cmbStaffGroups.SelectedValue = QueryString["StaffGroupId"];

        DataPager oDtPgDropDown = lstvwTaxDetails.FindControl("DtPgDropDown") as DataPager;
        DropDownList oddlCnt = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
        if (oddlCnt != null)
        {
            oddlCnt.SelectedValue = hidPageNo.Value;
            cmbPageCnt_SelectedIndexChanged(oddlCnt, null);
        }
    }

    /// <summary>
    /// This method is used to fill up staff group combo box.
    /// </summary>
    ///  This is a existing method.
    private void FillStaffGroups()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();       
        DataTable dtAllStaffGroups = StaffGroupsBL.GetAll(miSchoolId);
        DataRow[] drStaffGroups = dtAllStaffGroups.Select("SchoolId=" + miSchoolId);
        DataTable dtStaffGroups = dtAllStaffGroups.Clone();
        if (drStaffGroups.Length > 0)
            dtStaffGroups = drStaffGroups.CopyToDataTable();
        ControlUtility.FillDropDownList(dtStaffGroups, ref cmbStaffGroups, "StaffGroupsId", "StaffGroupsName", Constants.S_ALL);        
    }

    /// <summary>
    /// This is to manage pubish unpublish status.
    /// </summary>
    private void CheckIsPublished()
    {
        bool bIsPublished = false;
        bIsPublished = moIncomeTaxDetailsBL.CheckIsPublished();
        if (bIsPublished)
            btnPublish.Text = S_UNPUBLISH;
        else
            btnPublish.Text = S_PUBLISH;
    }

    
    /// <summary>
    /// This method is used to fill section combo box.
    /// </summary>
    private void FillUsers()
    {
        btnPublish.Enabled = false;
        lstvwTaxDetails.DataSourceID = objdsIncomeTax.ID;
        lstvwTaxDetails.DataBind();        
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ApplyMouseHoverEffect(new List<Button> { btnPublish, btnSearch});
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        SetDefaultButton(btnSearch);
        cmbStaffGroups.Focus();
        hidName.Value = string.Empty;
        hidPageNo.Value = Constants.S_ONE;
        cmbStaffGroups.Focus();
        btnPublish.Attributes.Add("onclick", "if(!ConfirmDelete(this)) return false;");
    }
    
    #endregion
    
}