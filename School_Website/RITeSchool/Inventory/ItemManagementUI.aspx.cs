// File Name  : ItemManagementUI.aspx.cs
// Created By : Amit 
// Date       : 20/06/2009
//Description : This class is used to search and add/edit/remove the inventory items.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using CrystalDecisions.Shared;
using System.Threading;
using PayrollReportingUserEntities;
using System.Linq;

public partial class ItemManagementUI : SchoolBase
{
    #region " Constants "

    const string S_COMMAND_REMOVE = "Remove";
    const string S_COMMAND_ITEM_DETAILS = "ItemDetails";
    const string S_DEFAULT_SORT_EXP = "ItemName";

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to fill item category combo and set default properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillItemCategoryCombo();
                SetDefaultProperties();
                CheckDeletePermisssion();
                hlnkManageCategoryOrUOM.Attributes.Add("onclick", "window.open('" + hlnkManageCategoryOrUOM.NavigateUrl
                       + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=630,height=500'); return false;");

                if (!QueryString["btnStatus"].IsNullOrEmpty())
                {
                    btnSearch.Text = QueryString["btnStatus"].ToString();                    
                }
                if (QueryString["ItemName"].IsNullOrEmpty())
                {
                    Check_Button_Search_Status();
                }
                else
                {
                    txtItemName.Text = QueryString["ItemName"].ToString();
                    txtItemCode.Text = QueryString["ItemCode"].ToString();
                    ddlCategory.SelectedValue = QueryString["ItemCategory"].ToString();
                    txtRackNumber.Text = QueryString["RackNo"].ToString();
                    txtHallNumber.Text = QueryString["Hall"].ToString();
                    txtShelfNumber.Text = QueryString["ShelfNo"].ToString();
                    Check_Button_Search_Status();
                }

            }
            lblErrorMsg.Text = string.Empty;
            //This method is used to set default properties to controls.
            SetDefaultButton(btnSearch);
            ShowHideControls(chkNonMoveItem.Checked);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    

    /// <summary>
    /// This event is used to search items according to search criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Check_Button_Search_Status();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select paging list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItemDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " List View Events "


    /// <summary>
    /// This event is used to fill page dropdown if item count is more than 20. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItemDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwItemDetails, DtPgCount);
                SetSortImage(S_DEFAULT_SORT_EXP);
                SetListviewHeader();
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    

    /// <summary>
    /// This event is used to create query string for each edit image and set properties. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ImageButton imgBtnDelete = e.Item.FindControl("imgbtnRemoveItem") as ImageButton;

            if (Settings.IsAaryanSchool && hidAllowDeletion.Value != Constants.S_ONE)
                imgBtnDelete.Visible = false;
           
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                ImageButton imgbtnEdit = e.Item.FindControl("imgbtnEditItem") as ImageButton;
                ImageButton imgbtnRemove = e.Item.FindControl("imgbtnRemoveItem") as ImageButton;
                LinkButton lnkbtnItemDetails = e.Item.FindControl("lnkbtnItemDetails") as LinkButton;

                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                int iItemID = Convert.ToInt32(oDataRowView["ItemID"]);

                string sItemName = Convert.ToString(oDataRowView["ItemName"]);

                string sIsEditMode = "Edit";                
                string sName = txtItemName.Text.ToString();
                string sItemCode = txtItemCode.Text.ToString();
                double dCategory = ddlCategory.SelectedIndex.ToDouble();
                string sRackNumber = txtRackNumber.Text.ToString();
                string sShelfNumber = txtShelfNumber.Text.ToString();
                string sHallNumber = txtHallNumber.Text.ToString();


                string sEditQuerystring = "ItemID=" + iItemID + "&IsEditMode=" + sIsEditMode + "&ItemName=" + sName + "&ItemCode=" + sItemCode + "&ItemCategory=" + dCategory + "&RackNo=" + sRackNumber +
                    "&Hall=" + sHallNumber +"&ShelfNo=" + sShelfNumber;
                string sEditEncrypt = Utility.CommonUtility.EncryptQuerystring(sEditQuerystring.ToString());

                lnkbtnItemDetails.Attributes.Add("onclick", "window.open('../Inventory/ItemSpecificationUI.aspx?" + sEditEncrypt
                                                             + " ' , '_self');return false;");

                imgbtnEdit.Attributes.Add("onclick", "window.open('../Inventory/ItemDetailsUI.aspx?" + sEditEncrypt
                                                             + " ' , '_self');return false;");
                imgbtnRemove.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");

                bool IsConsiderForDetailLevel = Convert.ToBoolean(oDataRowView["IsConsiderForDetailLevel"]);
                if (!IsConsiderForDetailLevel)
                {
                    lnkbtnItemDetails.Text = "--";
                    lnkbtnItemDetails.Style["pointer-events"] = "none";
                    lnkbtnItemDetails.Style["cursor"] = "default";
                }

                int iCategoryId = Convert.ToInt32(lstvwItemDetails.DataKeys[e.Item.DisplayIndex]["ItemCategoryID"].ToString());
                string sQueryString = CommonUtility.EncryptQuerystring("ItemId=" + iItemID + "&ItemCategoryId=" + iCategoryId);

                HiddenField hidData = e.Item.FindControl("hidData") as HiddenField;
                hidData.Value = sQueryString;

                LinkButton lnkExport = e.Item.FindControl("lnkExport") as LinkButton;

                lnkExport.Attributes.Add("onclick", "OpenReport("+e.Item.DisplayIndex+"); return false;");                
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ItemDetailsReport, GetFilterString(), ExportFormatType.Excel);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        { }
       
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to remove item from item list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(e.Item.DisplayIndex);
             if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iItemID = Convert.ToInt32(lstvwItemDetails.DataKeys[iRowIndex]["ItemID"].ToString());
                ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
                oItemsMasterBL.ItemID = iItemID;
                oItemsMasterBL.SchoolId = miSchoolId;
                oItemsMasterBL.UpdatedById = miSchoolId;
                oItemsMasterBL.GetDependancyForItemRemove(iItemID, miSchoolId, miAcademicYearId);
                oItemsMasterBL.DeleteItemDetails();
                lstvwItemDetails.DataSourceID = lstvwDSobj.ID;
            }            
        }
        catch (ReferenceExceptions ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            SetSortImage(S_DEFAULT_SORT_EXP);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort order in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }


    #endregion " List View Events "

    #region " private Methods "

    /// <summary>
    /// This method is used set AddSortImage
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asSortExpression"></param>
    /// <param name="asSortDirection"></param>
    private void AddSortImage(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
    {
        if (asSortExpression.Trim().Equals(""))
            return;

        // Create the sorting image based on the sort direction.
        Image sortImage = new Image();
        sortImage.ID = "sortImage";
        if (asSortDirection == "asc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
            sortImage.AlternateText = "Ascending Order";
        }
        else if (asSortDirection == "desc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
            sortImage.AlternateText = "Descending Order";
        }
        // Iterate through the Columns collection to determine the index
        // of the column being sorted.
        foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
        {
            asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");

            // Iterate through the cells collection to determine the index
            // of the cell being sorted.
            foreach (Control oControl in oHtmlTableCell.Controls)
            {
                LinkButton oLinkButton = oControl as LinkButton;
                if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
                {
                    Image oImage = (Image)oHtmlTableCell.FindControl("sortImage");
                    if (oImage == null)
                    {
                        // Add the image to the appropriate header cell.
                        if (sortImage.ImageUrl != "")
                        {
                            oHtmlTableCell.Controls.Add(sortImage);
                            break;
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// This method is used to fill item catogory combo.
    /// </summary>
    private void FillItemCategoryCombo()
    {
        DataTable oDTItemCategory = ItemCategoryMasterBL.GetAll(miSchoolId);
        ControlUtility.FillDropDownList(oDTItemCategory, ref ddlCategory, "ItemCategoryID", "Name", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set default properties.
    /// </summary>
    private void SetDefaultProperties()
    {
        txtItemName.Focus();
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valsumItems.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { btnSearch});
    }


    /// <summary>
    /// This method is used to show/hide From Date controls.
    /// </summary>
    /// <param name="abFlag"></param>
    private void ShowHideControls(bool abFlag)
    {
        tdLblFromDate.Visible = abFlag;
        tdTxtFromDate.Visible = abFlag;
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void SetSortImage(string asSortExpression)
    {
        if (lstvwItemDetails.SortDirection.ToString() == "Ascending" || lstvwItemDetails.SortDirection.ToString() == "")
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwItemDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwItemDetails.SortExpression.ToString();
        else
            hidSortExpression.Value = asSortExpression;
        HtmlTableRow oHtmlTableHeaderRow = lstvwItemDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }


    /// <summary>
    /// this method is used check the status of search button
    /// </summary>
    private void Check_Button_Search_Status()
    {
        try
        {            
            if (btnSearch.Text == "Search")
            {
                lstvwItemDetails.Visible = true;
                lstvwItemDetails.DataSourceID = lstvwDSobj.ID;
                lstvwItemDetails.DataBind();
                if (lstvwItemDetails.Items.Count > 0)
                {
                    DataPager pager = lstvwItemDetails.FindControl("DtPgDropDown") as DataPager;
                    pager.SetPageProperties(0, pager.PageSize, true);
                    btnExport.Visible = true;
                }
                txtItemName.Enabled = false;
                ddlCategory.Enabled = false;
                txtItemCode.Enabled = false;
                txtHallNumber.Enabled = false;
                txtRackNumber.Enabled = false;
                txtShelfNumber.Enabled = false;
                chkNonMoveItem.Enabled = false;
                chkShowItemBelowReorder.Enabled = false;
                txtFromDays.Enabled = false;
                btnSearch.Text = "Change Input";
            }
            else
            {
                btnSearch.Text = "Search";
                txtItemName.Enabled = true;
                ddlCategory.Enabled = true;
                txtItemCode.Enabled = true;
                txtItemName.Enabled = true;
                ddlCategory.Enabled = true;
                txtItemCode.Enabled = true;
                txtHallNumber.Enabled = true;
                txtRackNumber.Enabled = true;
                txtShelfNumber.Enabled = true;
                chkNonMoveItem.Enabled = true;
                chkShowItemBelowReorder.Enabled = true;
                txtFromDays.Enabled = true;
                lstvwItemDetails.Visible = false;
                lstvwItemDetails.DataSource = null;
                lstvwItemDetails.DataBind();                
                DtPgCount.Visible = false;
                btnExport.Visible = false;
            }
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void GetPostBackQueryString()
    {

        txtItemName.Text = QueryString["ItemName"].ToString();
        //txtRackNumber.Text = QueryString["RackNo"].ToString();
        lstvwItemDetails.Visible = true;
        lstvwItemDetails.DataSourceID = lstvwDSobj.ID;
        lstvwItemDetails.DataBind();
        if (lstvwItemDetails.Items.Count > 0)
        {
            DataPager pager = lstvwItemDetails.FindControl("DtPgDropDown") as DataPager;
            pager.SetPageProperties(0, pager.PageSize, true);
            btnExport.Visible = true;
        }
    }

    private string GetFilterString()
    {
        string sFilterStr = string.Empty;
        string sItemCode = "null";
        string sItemName = "null";
        string sRackNo = "null";
        string sShelfNo = "null";
        string sHall = "null";

        if (txtItemCode.Text.Trim() != string.Empty)
            sItemCode = txtItemCode.Text.Trim();
        if (txtItemName.Text.Trim() != string.Empty)
            sItemName = txtItemName.Text.Trim();
        if (txtRackNumber.Text.Trim() != string.Empty)
            sRackNo = txtRackNumber.Text.Trim();
        if (txtShelfNumber.Text.Trim() != string.Empty)
            sShelfNo = txtShelfNumber.Text.Trim();
        if (txtHallNumber.Text.Trim() != string.Empty)
            sHall = txtHallNumber.Text.Trim();

            sFilterStr = "(usp_GetItemDetailsforReport.SchoolID}=" + miSchoolId + "AND usp_GetItemDetailsforReport.ItemCode}=" + sItemCode + "AND usp_GetItemDetailsforReport.ItemName}=" + sItemName +
               "AND usp_GetItemDetailsforReport.RackNo }=" + sRackNo + "AND usp_GetItemDetailsforReport.ShelfNo}=" + sShelfNo + "AND usp_GetItemDetailsforReport.Hall}=" + sHall +
               "AND usp_GetItemDetailsforReport.IsQtyBelowReorderLevel}=" + (chkShowItemBelowReorder.Checked ? "1" : "0") +
               "AND usp_GetItemDetailsforReport.CategoryId}=" + ddlCategory.SelectedValue + ") @";

        return sFilterStr;
    }

    private void CheckDeletePermisssion()
    {
        hidAllowDeletion.Value = Constants.S_ONE;

        if (Settings.IsAaryanSchool)
        {
            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
            if (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowItemDeleteAccess.ToInt() && ru.UserId == miUserId).Any())
                hidAllowDeletion.Value = Constants.S_ONE;
            else
                hidAllowDeletion.Value = Constants.S_ZERO;
        }
    }

    private void SetListviewHeader()
    {
        if (Settings.IsAaryanSchool && hidAllowDeletion.Value != Constants.S_ONE)
        {
            HtmlTableRow tr = lstvwItemDetails.FindControl("trHeader") as HtmlTableRow;
            if (tr != null)
            {
                HtmlTableCell thRemove = tr.FindControl("thRemove") as HtmlTableCell;

                if (thRemove != null)
                    thRemove.Visible = false;
            }
        }
    }

    #endregion " private Methods "
}
