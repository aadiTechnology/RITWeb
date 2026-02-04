/*File Name - StoreItemListUI.aspx.cs
 * Created By - Rutuja
 * Created Date - 08 Jan 2023
 * Description - This class is used to display Store Item List.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.eStoreBL;
using BusinessLogic.Exceptions;
using SchoolEntities.eStore;
using Utility;

public partial class StoreItemListUI : SchoolBase
{
    #region Constants
    
    private const string S_DELETE_MSG = "Item details deleted successfully !!!";
    private const string S_UPDATE_MSG = "Item Details updated successfully !!!";
    
    #endregion

    #region DataMembers
    
    private StoreItemBL moStoreItemBL;
    
    #endregion

    #region Events
    /// <summary>
    ///  This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "Title";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwStoreItemDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display Store Items List.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStoreItemBL = new StoreItemBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStoreCategoryCombo();
                FillStandards();
                SetFields();
                SetStockLink();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    
    /// <summary>
    /// This event is used to search Store Items from list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        foreach (ListItem oItem in chklstStandards.Items)
        {
            if (oItem.Selected)
                sb.Append("," + oItem.Value);
        }

        if (sb.Length > 0)
            hidStandards.Value = sb.ToString().Substring(1);
        else
            hidStandards.Value = string.Empty;

        DataPager oDataPager = lstvwStoreItemDetails.FindControl("DtPgDropDown") as DataPager;
        if (oDataPager != null)
            oDataPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, true);

        FillStoreItemListView();
    }

    /// <summary>
    /// This event used set paging for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStoreItemDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Store Item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {  
                ImageButton imgBtnDelete = e.Item.FindControl("imgbtnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                LinkButton lnkbtnVarioation = e.Item.FindControl("lnkbtnVarioation") as LinkButton;
                Label lblVariation = e.Item.FindControl("lblVariation") as Label;
                LinkButton lnkbtnFreeItem = e.Item.FindControl("lnkbtnFreeItem") as LinkButton;
                //LinkButton lnkAddStock = e.Item.FindControl("lnkAddStock") as LinkButton;

                int iId = Convert.ToInt32(lstvwStoreItemDetails.DataKeys[e.Item.DisplayIndex]["Id"]);

                if (lstvwStoreItemDetails.DataKeys[e.Item.DisplayIndex]["IsVariationAvailable"].ToBool())
                {
                    lnkbtnVarioation.Visible = true;
                    lblVariation.Visible = false;
                    //lnkAddStock.Visible = false;
                    lnkbtnFreeItem.Visible = false;
                }
                else
                {
                    lnkbtnVarioation.Visible = false;
                    lblVariation.Visible = true;
                    //lnkAddStock.Visible = true;

                    string sQueryString = "ItemMasterId=" + iId + "&ItemVariationDetailId=" + 0 + "&StoreCategoryName=" + ddlStoreCategory.SelectedItem.Text;
                    sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                    //lnkAddStock.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");

                    lnkbtnFreeItem.Visible = true;
                    HiddenField hidQueryString = e.Item.FindControl("hidQueryString") as HiddenField;                    
                    hidQueryString.Value = Utility.CommonUtility.EncryptQuerystring("BaseItemMasterId=" + iId + "&ItemType=M");
                    lnkbtnFreeItem.Attributes.Add("onclick", "OpenFeeItemPopup(" + e.Item.DisplayIndex + "); return false;");
                }

                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill page footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStoreItemDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStoreItemDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select Store Item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int aiId = Convert.ToInt32(lstvwStoreItemDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"]);

                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    string sQueryString = "Id=" + aiId + "&StoreCategoryId=" + ddlStoreCategory.SelectedValue + "&Filter=" + txtSearch.Text + "&OriginalStandardIds=" + hidStandards.Value + "&StoreCategoryName=" + ddlStoreCategory.SelectedItem.Text;
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                    string sRedirectUrl = "StoreItemDetailsUI.aspx" + "?" + sEncrypt;
                    MasterPage oMasterPage = (MasterPage)this.Master;
                    oMasterPage.RedirectToNextPage(sRedirectUrl);
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    DeleteItem(aiId);
                    FillStoreItemListView();
                }
                else if (e.CommandName == "ADDVARIATION")
                {
                    string sTitle = lstvwStoreItemDetails.DataKeys[e.Item.DisplayIndex]["ItemName"].ToString();
                    string sQueryString = "StoreItemMasterId=" + aiId + "&StoreCategoryId=" + ddlStoreCategory.SelectedValue + "&Filter=" + txtSearch.Text + "&OriginalStandardIds=" + hidStandards.Value + "&StoreCategoryName=" + ddlStoreCategory.SelectedItem.Text + "&ItemName=" + sTitle;
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                    string sRedirectUrl = "StoreItemVariationDetailsUI.aspx" + "?" + sEncrypt;
                    MasterPage oMasterPage = (MasterPage)this.Master;
                    oMasterPage.RedirectToNextPage(sRedirectUrl);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used to sort the list view of store items by Title and Associated Standards.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemDetails_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to fill store items in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStoreCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStoreItemListView();
            SetStockLink();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetStockLink()
    {
        hlnkStock.NavigateUrl = "StoreItemStockDetailsPopup.aspx?"+CommonUtility.EncryptQuerystring("StoreCategoryName=" + ddlStoreCategory.SelectedItem.Text);
    }

    /// <summary>
    /// This event is used to fill store items in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = "&Id=" + 0 + "&StoreCategoryId=" + ddlStoreCategory.SelectedValue + "&Filter=" + txtSearch.Text + "&OriginalStandardIds=" + hidStandards.Value + "&StoreCategoryName=" + ddlStoreCategory.SelectedItem.Text;
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);

            MasterPage oMaster = this.Master as MasterPage;
            oMaster.RedirectToNextPage("StoreItemDetailsUI.aspx?" + sEncrypt);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to Fill Store Item listview
    /// </summary>
    private void FillStoreItemListView()
    {
        lstvwStoreItemDetails.DataSourceID = lstvwObjDS.ID;
        lstvwStoreItemDetails.DataBind();
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
    /// This method is used to fill standards checkbox list.
    /// </summary>
    private void FillStandards()
    {
        StoreItemDetailsBL oStoreItemDetailsBL = new StoreItemDetailsBL(miSchoolId, miUserId, miAcademicYearId);
        List<StandardList> lstStandard = oStoreItemDetailsBL.GetStandardList();
        ListSource.FillCheckBoxList(lstStandard, chklstStandards, "Standard_Name", "Original_Standard_Id");
    }

    /// <summary>
    /// This method is used to fill Store Category Combobox.
    /// </summary>
    private void FillStoreCategoryCombo()
    {
        DataTable oDTStoreCategories = moStoreItemBL.GetStoreCategories();
        ControlUtility.FillDropDownList(oDTStoreCategories, ref ddlStoreCategory, "Id", "Name", string.Empty);

        ListItem oListItem = ddlStoreCategory.Items.FindByText("Uniform");
        if (oListItem != null)
        {
            oListItem.Selected = true;
            ddlStoreCategory_SelectedIndexChanged(ddlStoreCategory, null);
        }
    }

    /// <summary>
    /// This method is used to Delete Store Item.
    /// </summary>
    /// <param name="aiId"></param>
    private void DeleteItem(int aiId)
    {
        moStoreItemBL.DeleteItem(aiId);
        lblUpdate.Text = S_DELETE_MSG;
        FillStoreItemListView();
    }

    /// <summary>
    /// This method is used to set field state.
    /// </summary>
    private void SetFields()
    {
        if (QueryString["StoreCategoryId"] != null || QueryString["OriginalStandardIds"] != null || QueryString["Filter"] != null)
        {
            if (QueryString["StoreCategoryId"] != null)
                ddlStoreCategory.SelectedValue = QueryString["StoreCategoryId"].ToString();

            if (QueryString["Filter"] != null)
                txtSearch.Text = QueryString["Filter"].ToString();

            if (QueryString["OriginalStandardIds"] != null)
            {
                string sOriginalStandardIds = QueryString["OriginalStandardIds"].ToString();
                string[] sArrData = sOriginalStandardIds.Split(',');
                if (sArrData.Length > 0)
                {
                    foreach (ListItem oItem in chklstStandards.Items)
                    {
                        if (sArrData.Contains(oItem.Value))
                            oItem.Selected = true;
                    }
                }

                int iTotalSelected = chklstStandards.Items.Cast<ListItem>().Count(item => item.Selected);
                if(iTotalSelected == chklstStandards.Items.Count)
                    ChkSelectAllStd.Checked = true;
            }

            BtnSearch_Click(BtnSearch, null);
        }
    }

    #endregion
}