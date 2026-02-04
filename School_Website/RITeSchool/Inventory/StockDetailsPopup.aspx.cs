/* File Name = StockDetailsPopup.aspx.cs
 * Created Date - 30 December 2015
 * Created by - Sanket
 * Class Description - This class is defined to Add New stock Details.
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using NewStockDetails;
using Utility;
using System.Data;
public partial class StockDetailsPopup : SchoolBase
{
    #region "Data Members"

    private StockDetailsBL moStockDetailsBL;

    #endregion

    #region "Constants"

    private const string S_SAVE_MESSAGE = "Item stock Details saved successfully !!!";
    private const string S_UPDATE_MESSAGE = "Item stock Details updated successfully !!!";
    private const string S_DELETE_MESSAGE = "Item stock Details deleted successfully !!!";
    private const string S_SORT_ROW = "SortRow";

    #endregion

    #region "events"

    /// <summary>
    /// Thos event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            base.AddSortImage(lstvwStockDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to load page control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStockDetailsBL = new StockDetailsBL(miSchoolId,miUserId);
            if (!IsPostBack)
            {
                ReadQuerystring();
                SetDefaultValues();
                FillControls();
                FillStockDetailsListView();
                SetJavascriptAttributes();
                FillVendor();   //fill vendors

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    private void FillVendor()  //fill vendor dropdown 
    {

        var oStandardCollectionBL = new StockDetailsBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAllVendor(miSchoolId, miAcademicYearId);
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbVendor, "VendorId", "VendorName", "--Select--");

    }
    /// <summary>
    /// This event is used to save details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();

            if (hidId.Value.ToString() == Constants.S_ZERO)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);

            FillStockDetailsListView();
            FillControls();
            ClearFields();
        }
        catch (SqlException se)
        {
            base.DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear the controls.
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
    /// This event is used to perform updated or delete operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStockDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {                
                int iNewItemId = Convert.ToInt32(lstvwStockDetails.DataKeys[e.Item.DisplayIndex]["Id"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    StockDetails oStockDetails = moStockDetailsBL.Get(iNewItemId);

                    FillControls();
                    cmbUnits.SelectedValue = (oStockDetails.ConsiderInUnitQuanity == 1 ? Constants.S_ONE : Constants.S_ZERO);

                    if (oStockDetails.ConsiderInUnitQuanity == 1)
                        txtQuantity.Text = oStockDetails.ItemQuantity.ToString();
                    else
                    {
                        if (oStockDetails.ItemQuantity % oStockDetails.UOMPieceCount == 0)
                            txtQuantity.Text = Math.Round(oStockDetails.ItemQuantity / oStockDetails.UOMPieceCount, 2).ToString();
                        else
                        {
                            txtQuantity.Text = oStockDetails.ItemQuantity.ToString();
                            cmbUnits.SelectedValue = Constants.S_ONE;
                        }
                    }

                    txtItemPrice.Text = oStockDetails.price.ToString();
                    txtDate.Text = oStockDetails.Date.ToString(Constants.S_DATE_FORMAT);
                    txtDescription.Text = oStockDetails.Description;
                    hidId.Value = oStockDetails.Id.ToString();
                    cmbVendor.SelectedValue = oStockDetails.VendorId.ToString();
                    txtInvoiceNo.Text = oStockDetails.InvoiceNo;
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                    
                }
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moStockDetailsBL.Delete(iNewItemId);
                    FillStockDetailsListView();
                    FillControls();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    hidId.Value = Constants.S_ZERO;
                    ClearFields();
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {                
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillStockDetailsListView();
            }
        }
        catch (SqlException se)
        {
            base.DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to boud data for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStockDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStockDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStockDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cloce popup and refresh base screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + "'?"
                                + CommonUtility.EncryptQuerystring("OriginalItemId=" + QueryString["ItemId"] + "&OriginalItemName=" + QueryString["ItemName"] + "&OriginalItemCode=" + QueryString["ItemCode"]
                                + "&OriginalItemCategory=" + QueryString["ItemCategory"] + "&IsShowItemBelowReorder=" + QueryString["ShowItemBelowReorder"]) + "'" + ";window.close();window.opener.focus(); </Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStockDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound the data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStockDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ImageButton imgbtnRemove = e.Item.FindControl("imgBtnDelete") as ImageButton;
            imgbtnRemove.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");
            Label lblDate = e.Item.FindControl("lblDate") as Label;
            lblDate.Text = (Convert.ToDateTime(lblDate.Text)).ToString(Constants.S_DATE_FORMAT);            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Methods"
    
    /// <summary>
    /// This method is used to Save or Update the stock details.
    /// </summary>
    private void Save()
    {
        
        StockDetails oStockDetails = new StockDetails();
        
        oStockDetails.ItemId = Convert.ToInt32(hidItemId.Value);
        oStockDetails.ItemQuantity = Convert.ToDecimal(txtQuantity.Text);
        if(txtItemPrice.Text != string.Empty)
        oStockDetails.price =  Convert.ToDecimal(txtItemPrice.Text);
        oStockDetails.Date = Convert.ToDateTime(txtDate.Text);
        oStockDetails.ConsiderInUnitQuanity = Convert.ToInt32(cmbUnits.SelectedValue.ToString());
        oStockDetails.Description = txtDescription.Text;
        oStockDetails.InvoiceNo = txtInvoiceNo.Text;  //invoice no
        oStockDetails.VendorId = Convert.ToInt32(cmbVendor.SelectedValue.ToString()); //vendor
        moStockDetailsBL.Save(oStockDetails,hidId.Value.ToInt());
    }

    /// <summary>
    /// This method is used to fill Stock Details Listview.
    /// </summary>
    private void FillStockDetailsListView()
    {        
        lstvwStockDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This method is used to fill the control.
    /// </summary>
    private void FillControls()
    {        
        int iItemId = Convert.ToInt32(hidItemId.Value);
        StockItemDetails oStockItemDetails = moStockDetailsBL.GetStockItemDetails(iItemId);
        lblItemOriginalName.Text = oStockItemDetails.ItemName;
        lblItemOriginalCode.Text = oStockItemDetails.ItemCode;
        lblCurrentOriginalStock.Text = oStockItemDetails.ItemQuantityWithUnits;

        string sUOMUnit = oStockItemDetails.CurrentStockUOM;

        cmbUnits.Items.Clear();
        cmbUnits.Items.Add(new ListItem { Text = sUOMUnit, Value = Constants.S_ZERO });
        cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });


    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        txtDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
        hidId.Value = Constants.S_ZERO;
        hidSortDirection.Value = Constants.S_DESCENDING;
        txtQuantity.Focus();
    }

    /// <summary>
    /// This method is used to read Query string.
    /// </summary>
    private void ReadQuerystring()
    {  
        hidItemId.Value = QueryString["ItemId"].ToString();
    }

    /// <summary>
    /// This method is used to clear the fields.
    /// </summary>
    public void ClearFields()
    {
        txtQuantity.Text = string.Empty;
        txtItemPrice.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        hidId.Value = Constants.S_ZERO;
        hidSortDirection.Value = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        cmbUnits.SelectedIndex = 0;
        txtInvoiceNo.Text = string.Empty;  //invoice no
        cmbVendor.ClearSelection();        //vendor
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnClose });
        btnSave.Attributes.Add("onclick", "ClearMessage()");
    }

    #endregion

}