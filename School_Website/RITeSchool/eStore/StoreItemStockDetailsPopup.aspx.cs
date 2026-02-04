/*File Name - StoreItemStockDetailsPopup.aspx.cs
 * Created Date - 20-Jan-2024
 * Created By - Vishakha
 * Description - This class is used to add store item stock details.
 */
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.eStoreBL;
using BusinessLogic.Exceptions;
using SchoolEntities.eStore;
using Utility;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;
using SchoolEntities;
using System.Web.Script.Serialization;

public partial class StoreItemStockDetailsPopup : SchoolBase
{
    #region Data Member(s)

    private StoreItemStockDetailsBL moStoreItemStockDetailsBL;

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
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill stock details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStoreItemStockDetailsBL = new StoreItemStockDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                GetQueryString();
                SetDefaultValues();
                FillStockDetails();
                FillSearchResult(false);
                LoadGSTDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save store item stock details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            
            if (btnSave.Text == Constants.ButtonText.Save.ToString())
                lblMessage.Text = "Stock details saved successfully !!!";
            else
                lblMessage.Text = "Stock details updated successfully !!!";

            ClearFields();
            FillStockDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Cancel store item stock details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit/delete store item stock details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemStockDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwStoreItemStockDetails.DataKeys[e.Item.DisplayIndex]["StockMasterId"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    SetValueForUpdate(iId);
                }
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moStoreItemStockDetailsBL.Delete(iId);
                    lblMessage.Text = "Stock details deleted successfully !!!";
                    ClearFields();
                    FillStockDetails();
                }
            }
        }
        catch (SqlException ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Font.Bold = false;
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
    protected void lstvwStoreItemStockDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStoreItemStockDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStoreItemStockDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemStockDetails_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to set confirmation message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStoreItemStockDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                StoreItemStockMaster oStoreItemStockDetails = e.Item.DataItem as StoreItemStockMaster;

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                Label lblDate = e.Item.FindControl("lblDate") as Label;
                lblDate.Text = oStoreItemStockDetails.Date.ToString(Constants.S_DATE_FORMAT);
            }
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStoreItemStockDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillSearchResult(true);
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVariationDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVariationDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVariationDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwVariationDetails, DtPgCount);
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
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVariationDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidFirstSortExpression.Value = e.SortExpression;
            SetFirstSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle update and delete action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVariationDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwVariationDetails.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == "SELECT")
                    AddInBasket(iId);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove item from basket.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStockItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwVariationDetails.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == "RemoveCommand")
                    RemoveFromBasket(e.Item.DisplayIndex);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle change event of controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStockItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                TextBox txtMRP = e.Item.FindControl("txtMRP") as TextBox;
                TextBox txtDiscount = e.Item.FindControl("txtDiscount") as TextBox;
                TextBox txtQuantity = e.Item.FindControl("txtQuantity") as TextBox;

                txtMRP.Attributes.Add("onchange", "CalculatePrice(" + e.Item.DisplayIndex + ")");
                txtDiscount.Attributes.Add("onchange", "CalculatePrice(" + e.Item.DisplayIndex + ")");
                txtQuantity.Attributes.Add("onchange", "CalculatePrice(" + e.Item.DisplayIndex + ")");

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
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

    /// <summary>
    /// This method is used to save store item stock details.
    /// </summary>
    private void Save()
    {
        StoreItemStockMaster oStoreItemStockDetails = new StoreItemStockMaster();        
        oStoreItemStockDetails.NetPrice = txtPrice.Text.ToDecimal();
        oStoreItemStockDetails.Date = txtDate.Text.ToDateTime();
        oStoreItemStockDetails.Description = txtDescription.Text.ToString();

        if (txtAdjustedAmount.Text != string.Empty)
            oStoreItemStockDetails.AdjustedAmount = txtAdjustedAmount.Text.ToDecimal();
        else
            oStoreItemStockDetails.AdjustedAmount = 0;

        if (txtTransportAmount.Text != string.Empty)
            oStoreItemStockDetails.TransportAmount = txtTransportAmount.Text.ToDecimal();
        else
            oStoreItemStockDetails.TransportAmount = 0;

        oStoreItemStockDetails.TotalAmount = txtAmount.Text.ToDecimal();
        oStoreItemStockDetails.StockMasterId = hidId.Value.ToInt();
        oStoreItemStockDetails.StockDetails = GetStockDetails();

        moStoreItemStockDetailsBL.Save(oStoreItemStockDetails);
    }

    /// <summary>
    /// This method is used to get stock details.
    /// </summary>
    /// <returns></returns>
    private string GetStockDetails()
    {
        List<StoreItemStockDetails> lstVariation = new List<StoreItemStockDetails>();

        foreach (ListViewDataItem Oitem in lstvwStockItems.Items)
        {
            StoreItemStockDetails oStockDetails = new StoreItemStockDetails();

            oStockDetails.ItemVariationDetailId = lstvwStockItems.DataKeys[Oitem.DisplayIndex]["ItemVariationDetailId"].ToInt();

            HiddenField hidIdNew = Oitem.FindControl("hidIdNew") as HiddenField;
            oStockDetails.Id = hidIdNew.Value.ToInt();

            TextBox txtMRP = Oitem.FindControl("txtMRP") as TextBox;
            oStockDetails.MRP = txtMRP.Text.ToDecimal();

            TextBox txtDiscount = Oitem.FindControl("txtDiscount") as TextBox;

            if (txtDiscount.Text != string.Empty)
                oStockDetails.Discount = txtDiscount.Text.ToDecimal();
            else
                oStockDetails.Discount = 0;

            TextBox txtQuantity = Oitem.FindControl("txtQuantity") as TextBox;
            oStockDetails.NewQuantity = txtQuantity.Text.ToInt();

            TextBox txtTPrice = Oitem.FindControl("txtTPrice") as TextBox;
            oStockDetails.Price = txtTPrice.Text.ToDecimal();

            lstVariation.Add(oStockDetails);
        }

        return base.GenerateXml(lstVariation);
    }

    /// <summary>
    /// This method is used fill store item stock details listview.
    /// </summary>
    private void FillStockDetails()
    {
        lstvwStoreItemStockDetails.DataSourceID = lstvwDSobj.ID;
        lstvwStoreItemStockDetails.DataBind();
    }

    /// <summary>
    /// This method is used to add item in basket.
    /// </summary>
    /// <param name="aiVariationId"></param>
    private void AddInBasket(int aiVariationId)
    {
        List<StoreItemStockDetails> lstVariation = new List<StoreItemStockDetails>();

        foreach (ListViewDataItem Oitem in lstvwStockItems.Items)
        {
            StoreItemStockDetails oStockDetails = new StoreItemStockDetails();

            oStockDetails.ItemVariationDetailId = lstvwStockItems.DataKeys[Oitem.DisplayIndex]["ItemVariationDetailId"].ToInt();
            
            HiddenField hidIdNew = Oitem.FindControl("hidIdNew") as HiddenField;
            oStockDetails.Id = hidIdNew.Value.ToInt();

            Label lblColor = Oitem.FindControl("lblColor") as Label;
            oStockDetails.Color = lblColor.Text;

            Label lblSize = Oitem.FindControl("lblSize") as Label;
            oStockDetails.Size = lblSize.Text;

            Label lblItemCode = Oitem.FindControl("lblItemCode") as Label;
            oStockDetails.ItemCode = lblItemCode.Text;

            Label lblTitle = Oitem.FindControl("lblTitle") as Label;
            oStockDetails.Title = lblTitle.Text;

            Label lblUOM = Oitem.FindControl("lblUOM") as Label;
            oStockDetails.UOM = lblUOM.Text;
            
            TextBox txtMRP = Oitem.FindControl("txtMRP") as TextBox;
            oStockDetails.MRP = txtMRP.Text.ToDecimal();

            TextBox txtDiscount = Oitem.FindControl("txtDiscount") as TextBox;
            oStockDetails.Discount = txtDiscount.Text.ToDecimal();

            TextBox txtQuantity = Oitem.FindControl("txtQuantity") as TextBox;
            oStockDetails.NewQuantity = txtQuantity.Text.ToInt();

            Label lblGST = Oitem.FindControl("lblGST") as Label;
            oStockDetails.GST = lblGST.Text;

            HiddenField hidGSTCategoryId = Oitem.FindControl("hidGSTCategoryId") as HiddenField;
            oStockDetails.GSTCategoryId = hidGSTCategoryId.Value.ToInt();

            TextBox txtTPrice = Oitem.FindControl("txtTPrice") as TextBox;
            oStockDetails.Price = txtTPrice.Text.ToDecimal();
                        
            lstVariation.Add(oStockDetails);  
        }


        if (lstVariation.Any(lv => lv.Id == aiVariationId))
            lstVariation.RemoveAll(lv=> lv.Id == aiVariationId);

        StoreItemVariationBL oStoreItemVariationBL = new StoreItemVariationBL(miSchoolId, miAcademicYearId, miUserId);
        StoreItemVariationDetails oItemVariationDetails = oStoreItemVariationBL.Get(aiVariationId);
        oItemVariationDetails.Price = 0;
        oItemVariationDetails.Quantity = 0;

        StoreItemStockDetails oStoreItemStockDetails = new StoreItemStockDetails();
        oStoreItemStockDetails.Color = oItemVariationDetails.Color;
        oStoreItemStockDetails.GST = oItemVariationDetails.GST;
        oStoreItemStockDetails.GSTCategoryId = oItemVariationDetails.GSTCategoryId;
        oStoreItemStockDetails.ItemCode = oItemVariationDetails.ItemCode;

        oStoreItemStockDetails.ItemMasterId = oItemVariationDetails.StoreItemMasterId;
        oStoreItemStockDetails.ItemVariationDetailId = oItemVariationDetails.Id;

        oStoreItemStockDetails.MRP = oItemVariationDetails.MRP;
        oStoreItemStockDetails.NewQuantity = 0;
        oStoreItemStockDetails.Price = 0;
        oStoreItemStockDetails.Id = 0;
        oStoreItemStockDetails.Size = oItemVariationDetails.Size;
        oStoreItemStockDetails.Title = oItemVariationDetails.Title;
        oStoreItemStockDetails.UOM = oItemVariationDetails.UOM;

        lstVariation.Add(oStoreItemStockDetails);

        lstvwStockItems.DataSource = lstVariation;
        lstvwStockItems.DataBind();
    }

    /// <summary>
    /// This method is used to remove item from basket.
    /// </summary>
    /// <param name="aiIndex"></param>
    private void RemoveFromBasket(int aiIndex)
    {
        List<StoreItemStockDetails> lstVariation = new List<StoreItemStockDetails>();

        foreach (ListViewDataItem Oitem in lstvwStockItems.Items)
        {
            if (Oitem.DisplayIndex != aiIndex)
            {
                StoreItemStockDetails oStockDetails = new StoreItemStockDetails();

                oStockDetails.ItemVariationDetailId = lstvwStockItems.DataKeys[Oitem.DisplayIndex]["ItemVariationDetailId"].ToInt();

                HiddenField hidIdNew = Oitem.FindControl("hidIdNew") as HiddenField;
                oStockDetails.Id = hidIdNew.Value.ToInt();

                Label lblColor = Oitem.FindControl("lblColor") as Label;
                oStockDetails.Color = lblColor.Text;

                Label lblSize = Oitem.FindControl("lblSize") as Label;
                oStockDetails.Size = lblSize.Text;

                Label lblItemCode = Oitem.FindControl("lblItemCode") as Label;
                oStockDetails.ItemCode = lblItemCode.Text;

                Label lblTitle = Oitem.FindControl("lblTitle") as Label;
                oStockDetails.Title = lblTitle.Text;

                Label lblUOM = Oitem.FindControl("lblUOM") as Label;
                oStockDetails.UOM = lblUOM.Text;

                TextBox txtMRP = Oitem.FindControl("txtMRP") as TextBox;
                oStockDetails.MRP = txtMRP.Text.ToDecimal();

                TextBox txtDiscount = Oitem.FindControl("txtDiscount") as TextBox;
                oStockDetails.Discount = txtDiscount.Text.ToDecimal();

                TextBox txtQuantity = Oitem.FindControl("txtQuantity") as TextBox;
                oStockDetails.NewQuantity = txtQuantity.Text.ToInt();

                Label lblGST = Oitem.FindControl("lblGST") as Label;
                oStockDetails.GST = lblGST.Text;

                HiddenField hidGSTCategoryId = Oitem.FindControl("hidGSTCategoryId") as HiddenField;
                oStockDetails.GSTCategoryId = hidGSTCategoryId.Value.ToInt();

                TextBox txtTPrice = Oitem.FindControl("txtTPrice") as TextBox;
                oStockDetails.Price = txtTPrice.Text.ToDecimal();

                lstVariation.Add(oStockDetails);
            }
        }

        decimal dcAmount = 0;
        if (lstVariation.Count > 0)
            dcAmount = lstVariation.Sum(lv => lv.Price);

        txtAmount.Text = dcAmount.ToString();
        txtPrice.Text = Math.Round((dcAmount + (txtTransportAmount.Text == string.Empty ? 0 : txtTransportAmount.Text.ToDecimal()) +
            (txtAdjustedAmount.Text == string.Empty ? 0 : txtAdjustedAmount.Text.ToDecimal())),0).ToString();

        lstvwStockItems.DataSource = lstVariation;
        lstvwStockItems.DataBind();
    }

    /// <summary>
    /// This method is used to set sort order.
    /// </summary>
    private void SetFirstSortVariables()
    {
        if (hidFirstSortDirection.Value == Constants.S_DESCENDING)
            hidFirstSortDirection.Value = Constants.S_ASCENDING;
        else
            hidFirstSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to get querysting.
    /// </summary>
    private void GetQueryString()
    {
        hidStoreItemMasterId.Value = Constants.S_ZERO;
        if (QueryString["ItemMasterId"] != null)
        {
            hidItemMasterId.Value = QueryString["ItemMasterId"];
            StoreItemDetailsBL oStoreItemDetailsBL = new StoreItemDetailsBL(miSchoolId,miUserId, miAcademicYearId);
            StoreItemDetails oStoreItemDetails = oStoreItemDetailsBL.GetStoreItemDetails(hidItemMasterId.Value.ToInt());
        }

        if (QueryString["ItemVariationDetailId"] == null || QueryString["ItemVariationDetailId"].ToString() == "0")
        {
            hidItemVariationDetailId.Value = Constants.S_ZERO;
        }
        else
        {
            hidItemVariationDetailId.Value = QueryString["ItemVariationDetailId"];
            StoreItemVariationBL oStoreItemVariationBL = new StoreItemVariationBL(miSchoolId, miAcademicYearId, miUserId);
            StoreItemVariationDetails oStoreItemVariationDetails = oStoreItemVariationBL.Get(hidItemVariationDetailId.Value.ToInt());
        }
        
        if (QueryString["StoreCategoryName"] != null)
            lblStoreCategory.Text = QueryString["StoreCategoryName"].ToString();
    }

    /// <summary>
    /// This method is used to get details in edit mode.
    /// </summary>
    /// <param name="aiId"></param>
    private void SetValueForUpdate(int aiId)
    {
        btnSave.Text = Constants.ButtonText.Update.ToString();
        StoreItemStock oStoreItemStock = moStoreItemStockDetailsBL.Get(aiId);
        hidId.Value = aiId.ToString();    
        txtPrice.Text = oStoreItemStock.StockMaster.NetPrice.ToString();
        txtDate.Text = oStoreItemStock.StockMaster.Date.ToString(Constants.S_DATE_FORMAT);
        txtDescription.Text = oStoreItemStock.StockMaster.Description.ToString();

        txtAmount.Text = oStoreItemStock.StockMaster.TotalAmount.ToString();
        txtTransportAmount.Text = oStoreItemStock.StockMaster.TransportAmount.ToString();
        txtAdjustedAmount.Text = oStoreItemStock.StockMaster.AdjustedAmount.ToString();

        lstvwStockItems.DataSource = oStoreItemStock.StockDetails;
        lstvwStockItems.DataBind();
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        hidId.Value = Constants.S_ZERO;
        txtPrice.Text = string.Empty;
        txtDate.Text = string.Empty;
        txtDescription.Text = string.Empty;

        txtTransportAmount.Text = string.Empty;
        txtAdjustedAmount.Text = string.Empty;
        txtAmount.Text = string.Empty;

        lstvwStockItems.DataSource = null;
        lstvwStockItems.DataBind();

        txtSearch.Text = string.Empty;
        FillSearchResult(false);
        
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = "StoreItemListUI.aspx";

        txtTransportAmount.Attributes.Add("onchange","SetFinalAmount()");
        txtAdjustedAmount.Attributes.Add("onchange", "SetFinalAmount()");
    }

    /// <summary>
    /// This method is used to fill serach result.
    /// </summary>
    /// <param name="abLoadData"></param>
    private void FillSearchResult(bool abLoadData)
    {
        if (abLoadData)
        {
            lstvwVariationDetails.DataSourceID = objdsVariations.ID;
            lstvwVariationDetails.DataBind();
        }
        else
        {
            lstvwVariationDetails.DataSourceID = null;
            lstvwVariationDetails.DataBind();
        }
    }

    /// <summary>
    /// This method is used to load GST details.
    /// </summary>
    private void LoadGSTDetails()
    {
        PODetailsBL oPODetailsBL = new PODetailsBL();
        List<GSTCategory> lstGSTCategory = oPODetailsBL.GetGSTCategory();
        lstGSTCategory = lstGSTCategory.OrderBy(gst => gst.Id).ToList();

        var jsSerializer = new JavaScriptSerializer();
        hidGSTData.Value = jsSerializer.Serialize(lstGSTCategory);
    }

    #endregion
}