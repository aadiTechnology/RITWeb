/*File Name - FreeItemDetailsPopup.aspx.cs
 * Created Date - 30 may 2024
 * Created By - Vishakha
 * Description - This class is used to manage free item details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic.eStoreBL;
using BusinessLogic;
using SchoolEntities.eStore;
using System.Linq;
using System.Data;

public partial class FreeItemDetailsPopup : SchoolBase
{
    #region Data Member(s)

    const string S_ITEM_DETAILS_DATA = "LstFreeItems_DataSource";
    const string S_TEXT_SEARCH = "Search";
    const string S_TEXT_CHANGE_INPUT = "Change Input";
    const int I_MAX_LISTVIEW_ROWS = 5;
    private const string S_TEXT_UPDATE = "Update";
    private const string S_COMMAND_DELETE = "Remove";
    private const string S_COMMAND_UPDATE = "Update";

    private StoreItemVariationBL moStoreItemVariationBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add sort image.
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

            AddSortImage(lstvwFreeItems, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill free item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStoreItemVariationBL = new StoreItemVariationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ReadQueryString();
                FillFreeItemDetails();
                SetDefaultValues();
                btnClose.Attributes.Add("onclick", "CloseWindow()");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save free item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();

            if (btnSave.Text == Constants.ButtonText.Save.ToString())
                lblMessage.Text = "Free item details saved successfully !!!";
            else
                lblMessage.Text = "Free item details updated successfully !!!";

            FillFreeItemDetails();
            ClearFields();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwFreeItems);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to search free item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSearch.Text == S_TEXT_SEARCH)
            {
                btnSearch.Text = S_TEXT_CHANGE_INPUT;
                txtSearch.Enabled = false;
                DtPgCount.SetPageProperties(0, I_MAX_LISTVIEW_ROWS, false);
                lstvwFreeItems.DataSourceID = lstDSobj.ID;
                lstvwFreeItems.DataBind();
            }
            else
            {
                txtSearch.Enabled = true;
                lstvwFreeItems.DataSourceID = null;
                btnSearch.Text = S_TEXT_SEARCH;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to set paging details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFreeItems_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwFreeItems.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwFreeItems, DtPgCount);
            else
            {
                DtPgCount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle update and delete action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFreeItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iId = lstvwFreeItems.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();

            if (e.CommandName == "Add")
            {
                StoreItemVariationBL oStoreItemVariationBL = new StoreItemVariationBL(miSchoolId, miAcademicYearId, miUserId);
                StoreItemVariationDetails oStoreItemVariationDetails = oStoreItemVariationBL.Get(iId);
                
                lblItemName.Text = oStoreItemVariationDetails.Title;
                lblItemCode.Text = oStoreItemVariationDetails.ItemCode;
                hidItemVariationId.Value = oStoreItemVariationDetails.Id.ToString();
               
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwFreeItems);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwFreeItems_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        try
        {

        }
        catch (Exception)
        {
        }
    }

    protected void lstvwFreeItemDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwFreeItemDetails.DataKeys[iRowId]["Id"]);

                if (e.CommandName == S_COMMAND_UPDATE)
                    SetControlsForEditMode(iId);
                else if (e.CommandName == S_COMMAND_DELETE)
                {
                    Delete(iId);
                    ClearFields();
                    FillFreeItemDetails();
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwFreeItemDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("imgbtnDeleteItem") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    private void Save()
    {
        FreeItemDetails oFreeItemDetails = new FreeItemDetails();
        oFreeItemDetails.Quantity = txtQuantity.Text.ToInt();
        oFreeItemDetails.Id = HidId.Value.ToInt();
        oFreeItemDetails.ItemVariationId = hidItemVariationId.Value.ToInt();
        oFreeItemDetails.BaseItemVariationId = hidBaseItemVariationId.Value.ToInt();
        
        moStoreItemVariationBL.SaveFreeItemDetails(oFreeItemDetails);
    }

    private void SetControlsForEditMode(int aiId)
    {
        btnSave.Text = S_TEXT_UPDATE;
        
        FreeItemDetails oFreeItemDetails = moStoreItemVariationBL.GetFreeItems(aiId);
        txtQuantity.Text = oFreeItemDetails.Quantity.ToString();
        lblItemCode.Text = oFreeItemDetails.ItemCode;
        lblItemName.Text = oFreeItemDetails.Title;
        HidId.Value = aiId.ToString();
        hidItemVariationId.Value = oFreeItemDetails.ItemVariationId.ToString();
    }

    private void FillFreeItemDetails()
    {
        List<FreeItemDetails> lstFreeItemDetails = moStoreItemVariationBL.GetAllFreeItems(hidBaseItemVariationId.Value.ToInt());
        lstvwFreeItemDetails.DataSource = lstFreeItemDetails;
        lstvwFreeItemDetails.DataBind();
    }

    private void Delete(int aiId)
    {
        moStoreItemVariationBL.DeleteFreeItems(aiId);
        lblMessage.Text = "Free item details deleted successfully !!!";
    }

    private void ClearFields()
    {
        txtQuantity.Text = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        HidId.Value = "0";
        hidItemVariationId.Value = "0";
        lblItemName.Text = string.Empty;
        lblItemCode.Text = string.Empty;
        hidItemVariationId.Value = Constants.S_ZERO;
    }

    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    private void ReadQueryString()
    {
        if (QueryString["BaseItemVariationId"] != null)
        {
            hidBaseItemVariationId.Value = QueryString["BaseItemVariationId"].ToString();

            StoreItemVariationBL oStoreItemVariationBL = new StoreItemVariationBL(miSchoolId, miAcademicYearId, miUserId);
            StoreItemVariationDetails oStoreItemVariationDetails = oStoreItemVariationBL.Get(hidBaseItemVariationId.Value.ToInt());

            lblBaseItemCode.Text = oStoreItemVariationDetails.ItemCode;
            lblBaseItemName.Text = oStoreItemVariationDetails.Title;
        }
        else if (QueryString["BaseItemMasterId"] != null)
        {
            //hidBaseItemMasterId.Value = QueryString["BaseItemMasterId"].ToString();

            StoreItemDetailsBL oStoreItemDetailsBL = new StoreItemDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            StoreItemDetails oStoreItemDetails = oStoreItemDetailsBL.GetStoreItemDetails(QueryString["BaseItemMasterId"].ToInt());
            lblBaseItemCode.Text = oStoreItemDetails.ItemCode;
            lblBaseItemName.Text = oStoreItemDetails.Title;
            hidBaseItemVariationId.Value = oStoreItemDetails.StoreItemVariationId.ToString();
        }
        else
            hidBaseItemVariationId.Value = Constants.S_ZERO;

        //if (QueryString["BaseItemMasterId"] != null)
        //{
        //    //hidBaseItemMasterId.Value = QueryString["BaseItemMasterId"].ToString();

        //    StoreItemDetailsBL oStoreItemDetailsBL = new StoreItemDetailsBL(miSchoolId, miUserId, miAcademicYearId);
        //    StoreItemDetails oStoreItemDetails = oStoreItemDetailsBL.GetStoreItemDetails(QueryString["BaseItemMasterId"].ToInt());
        //    lblBaseItemCode.Text = oStoreItemDetails.ItemCode;
        //    lblBaseItemName.Text = oStoreItemDetails.Title;
        //    hidBaseItemVariationId.Value = oStoreItemDetails.StoreItemVariationId.ToString();
        //}
        //else
        //    hidBaseItemVariationId.Value = Constants.S_ZERO;
        //    //hidBaseItemMasterId.Value = Constants.S_ZERO;

        if (QueryString["ItemType"] != null)
            hidItemType.Value = QueryString["ItemType"].ToString();
    }
    protected void lstvwFreeItemDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {

    }

    #endregion
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }
    protected void lstvwFreeItems_Sorting(object sender, ListViewSortEventArgs e)
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

    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }
}

    
