// File Name  : StockBalanceUI.aspx.cs
// Created By : Amit
// Date       : 01/07/2009
// Description: This class is used balance item quantity in stock with reason. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic;

public partial class StockBalanceUI : SchoolBase
{
    #region " Constants "

    private const string S_LVIEW_CURRENT_STOCK_LABEL = "lblItemStock";
    private const string S_LVIEW_BALANCE_STOCK_TEXTBOX = "txtNewStock";
    private const string S_LVIEW_REASON_TEXTBOX = "txtReason";
    private const string S_LVIEW_MENDETORY_LABEL = "lblMend";    
    private const string S_COMMAND_BALANCE_ITEM = "BalanceItem";
    private const string S_BUTTON_SEARCH = "Search";
    private const string S_BUTTON_CHANGE_INPUT = "Change Input";
    private const string S_BUTTON_STOCK_UPDATE = "btnUpdateStock";
    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to fill item category combo box and set default control.
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
                ShowStockBalance();
                ReadQuerystring();
                ApplyMouseHoverEffect(new List<Button> {btnSearch, btnBack});
            }
            SetDefaultButton(btnSearch);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search items as per filter criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ShowStockBalance();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    #endregion " Events "

    #region " Listview Events "

    /// <summary>
    /// This event is used to fill listview pager at footer .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItems.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwItems, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to balance item quantity with reason of balance.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_BALANCE_ITEM)
            {
                ListViewDataItem oLstVwItem = (ListViewDataItem)e.Item;
                int iItemID = Convert.ToInt32(((Button)(e.CommandSource)).CommandArgument);

                TextBox oBalanceStock = oLstVwItem.FindControl(S_LVIEW_BALANCE_STOCK_TEXTBOX) as TextBox;
                TextBox oReason = oLstVwItem.FindControl(S_LVIEW_REASON_TEXTBOX) as TextBox;
                Label oCurrentStock = oLstVwItem.FindControl(S_LVIEW_CURRENT_STOCK_LABEL) as Label;

                double dCurrentStock = Convert.ToDouble(oCurrentStock.Text);
                double dBalanceStock = Convert.ToDouble(oBalanceStock.Text);
                string sReason = oReason.Text;

                StockBalancesDetailsBL oStockBalancesDetailsBL = new StockBalancesDetailsBL();
                oStockBalancesDetailsBL.ItemID = iItemID;
                oStockBalancesDetailsBL.OrginalItemQty = dCurrentStock;
                oStockBalancesDetailsBL.BalencedItemQty = dBalanceStock;
                oStockBalancesDetailsBL.Reason = sReason;
                oStockBalancesDetailsBL.School_Id = miSchoolId;
                oStockBalancesDetailsBL.Inserted_By_Id = miUserId;
                oStockBalancesDetailsBL.Updated_By_Id = miUserId;
                oStockBalancesDetailsBL.Is_Deleted = false;

                oStockBalancesDetailsBL.InsertStockBalancesDetails();

                SetDefaultProperties();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set properties to controls in listview for validation to balance item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oLstVwItem = (ListViewDataItem)e.Item;

                TextBox oReason = oLstVwItem.FindControl(S_LVIEW_REASON_TEXTBOX) as TextBox;
                Label oMend = oLstVwItem.FindControl(S_LVIEW_MENDETORY_LABEL) as Label;
                oReason.Enabled = false;
                oMend.Enabled = false;                    
                TextBox txtNewStock = e.Item.FindControl("txtNewStock") as TextBox;
                txtNewStock.Enabled = false;                     
                int iRowId = Convert.ToInt32(oLstVwItem.DisplayIndex);
                LinkButton oLinkButton = e.Item.FindControl(S_BUTTON_STOCK_UPDATE) as LinkButton;
                int iItemID = Convert.ToInt32(lstvwItems.DataKeys[iRowId]["ItemID"]);                     
                string sQueryString = "ItemId=" + iItemID + "&ItemName=" + txtItemName.Text + "&ItemCode=" + txtItemCode.Text + "&ItemCategory=" + ddlCategory.SelectedValue +
                         "&ShowItemBelowReorder=" + chkShowItemBelowReorder.Checked;                
                oLinkButton.Attributes.Add("onclick", "window.open('../Inventory/StockDetailsPopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500');return false;");                 
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging property of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItems);
            lstvwItems.DataSourceID = lstvwDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Listview Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to fill item category combo. 
    /// </summary>
    private void FillItemCategoryCombo()
    {
        ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
        DataTable oDTItemCategories = oItemsMasterBL.GetInventoryCategories(miSchoolId);
        ControlUtility.FillDropDownList(oDTItemCategories, ref ddlCategory, "ItemCategoryID", "ItemCategoryName", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used set default properties of controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        txtItemName.Focus();
        lstvwItems.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// this method is used check the Stock balance of search button
    /// </summary>
    private void ShowStockBalance()
    {
        try
        {
            if (btnSearch.Text == S_BUTTON_SEARCH)
            {
                lstvwItems.Visible = true;
                lstvwItems.DataSourceID = lstvwDSobj.ID;
                lstvwItems.DataBind();
                if (lstvwItems.Items.Count > 0)
                {
                    DataPager oDataPager = lstvwItems.FindControl("DtPgDropDown") as DataPager;
                    oDataPager.SetPageProperties(0, oDataPager.PageSize, true);
                }                
                txtItemName.Enabled = false;
                ddlCategory.Enabled = false;
                txtItemCode.Enabled = false;
                txtHall.Enabled = false;  //hall
                txtRackNo.Enabled = false; //rack
                txtShelfNo.Enabled = false;  //shelf
                chkShowItemBelowReorder.Enabled = false;
                btnSearch.Text = S_BUTTON_CHANGE_INPUT;

            }
            else
            {
                btnSearch.Text = S_BUTTON_SEARCH;
                txtItemName.Enabled = true;
                ddlCategory.Enabled = true;
                txtItemCode.Enabled = true;
                txtHall.Enabled = true;  //hall
                txtRackNo.Enabled = true; //rack
                txtShelfNo.Enabled = true;  //shelf
                chkShowItemBelowReorder.Enabled = true;
                lstvwItems.DataSource = null;
                lstvwItems.DataBind();
                lstvwItems.Visible = false;
                DtPgCount.Visible = false;
            }
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
        {
            if (QueryString["OriginalItemId"] != null)
            {
                if (QueryString["OriginalItemName"] != null)
                {
                    txtItemName.Text = QueryString["OriginalItemName"];                    
                }
                if (QueryString["OriginalItemCode"] != null)
                {
                    txtItemCode.Text = QueryString["OriginalItemCode"];
                }
                if (QueryString["OriginalItemCategory"] != null)
                {
                    ddlCategory.SelectedValue = QueryString["OriginalItemCategory"];
                }
                if (QueryString["IsShowItemBelowReorder"] != null)
                {
                    chkShowItemBelowReorder.Checked = QueryString["IsShowItemBelowReorder"].ToBool();
                }
                btnSearch.Text = S_BUTTON_SEARCH;
                ShowStockBalance();
            }
        }
    }

    #endregion " Private Methods "

}
    