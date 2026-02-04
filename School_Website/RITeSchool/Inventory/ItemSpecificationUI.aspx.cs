/* -------------------------------------------------------------------------------
 *	DEVELOPMENT LOG
 * -------------------------------------------------------------------------------
 *	Author	: Yogesh Karne
 *	Date	: 1-Jan-2016
 *	Purpose	: We can mark damage specific item.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using NewStockDetails;
using SchoolEntities.Inventory;
using Utility;

public partial class ItemSpecificationUI : SchoolBase
{
    #region Constant(s)

    private const string S_UPDATE_ITEM_SPECIFICATION = "UpdateItem";
    private const string S_DELETE_ITEM_SPECIFICATION = "DeleteItem";
    private const string S_DEFAULT_SORT_EXP = "SpecificationCode";
    private const string S_SORT = "SortRow";
    private const string S_SAVE_MESSAGE = "Item Details saved successfully !!!";
    private const string S_UPDATE_MESSAGE = "Item Details updated successfully !!!";
    private const int I_DESCRIPTION_LENGTH = 75;
    private const string S_DELETE_MESSAGE = "Item Details deleted successfully !!!";

    #endregion

    #region Data Member(s)

    private ItemSpecificationBL moItemSpecificationBL;
    
    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add the sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "SpecificationCode";
                hidSortDirection.Value = "asc";
            }
            base.AddSortImage(lstvwItemSpecificationDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Event will fire at page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moItemSpecificationBL = new ItemSpecificationBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                SetPostBackUrl();
                SetFields();
                FillItemSpecificationDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Event will fire listview data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemSpecificationDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItemSpecificationDetails.Items.Count > Constants.I_ZERO)
            {   
                ControlUtility.FillListViewPagerFooter(lstvwItemSpecificationDetails, DtPgCount);
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
    /// Event will fire on save event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnItemSave_Click(object sender, EventArgs e)
    {
        try
        {
            ItemSpecificationDetails oItemSpecificationDetails = Populate();
            moItemSpecificationBL.Save(oItemSpecificationDetails);

            if (hidId.Value == Constants.S_ZERO)
                lblMessage.Text = S_SAVE_MESSAGE;
            else
                lblMessage.Text = S_UPDATE_MESSAGE;

            ClearFields();
            FillItemSpecificationDetails();
            SetFields();
        }
        catch (SqlException ex)
        {
            lblError.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise Item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItemSpecificationDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fire on click of Cancel button.
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
    /// Event will fired on list view data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemSpecificationDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                bool bIsIssued = Convert.ToBoolean(lstvwItemSpecificationDetails.DataKeys[iRowId]["IsIssued"]);
                bool bIsDamaged = Convert.ToBoolean(lstvwItemSpecificationDetails.DataKeys[iRowId]["IsDamaged"]);

                string sDamageDescription = Convert.ToString(lstvwItemSpecificationDetails.DataKeys[iRowId]["DamageDescription"]);
                if (sDamageDescription.ToString() != string.Empty)
                {
                    Label lblDamageDescription = oCurrentItem.FindControl("lblDamageDescription") as Label;
                    if (sDamageDescription.Length >= I_DESCRIPTION_LENGTH)
                        lblDamageDescription.Text = sDamageDescription.Substring(0, I_DESCRIPTION_LENGTH) + "..";
                    else
                        lblDamageDescription.Text = sDamageDescription.ToString() ;
                }

                Image oImgIsIssued = e.Item.FindControl("imgBtnIsIssued") as Image;

                oImgIsIssued.Visible = bIsIssued;

                ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");

                imgBtnDelete.Visible = ((bIsIssued == true || bIsDamaged == true) ? false : true);

                if (bIsDamaged == true)
                {
                    HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trItemtemplate") as HtmlTableRow;
                    oHtmlTableRow.Style.Add(HtmlTextWriterStyle.Color, "Gray");
                }
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Event will fired on edit and delete button click present in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemSpecificationDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                hidId.Value = Convert.ToString(lstvwItemSpecificationDetails.DataKeys[iRowId]["Id"]);
                if (e.CommandName == S_UPDATE_ITEM_SPECIFICATION)
                {
                    ItemSpecificationDetails oItemSpecificationDetails = moItemSpecificationBL.Get(hidId.Value.ToInt());
                    txtSpecificationCode.Text = oItemSpecificationDetails.SpecificationCode;
                    txtDescription.Text = oItemSpecificationDetails.Description;
                    if (oItemSpecificationDetails.IsDamaged)
                    {
                        chkIsDamaged.Checked = true;
                        txtDamagedDt.Text = oItemSpecificationDetails.DamagedDate;
                        txtDamagedDiscription.Text = oItemSpecificationDetails.DamageDescription;
                        txtDamagedDiscription.Enabled = true;
                        txtItemPrice.Text = oItemSpecificationDetails.Price.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "SetFieldState(true);", true);
                    }
                    else
                    {
                        chkIsDamaged.Checked = false;
                        txtDamagedDiscription.Enabled = false;
                        txtItemPrice.Text = oItemSpecificationDetails.Price.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "SetFieldState(false);", true);
                    }
                    
                    btnItemSave.Enabled = (oItemSpecificationDetails.IsIssued ? false : true);

                    btnItemSave.Text = (hidId.Value == Constants.S_ZERO ? Constants.ButtonText.Save.ToString() : Constants.ButtonText.Update.ToString());
                }
                else if (e.CommandName == S_DELETE_ITEM_SPECIFICATION)
                {   
                    moItemSpecificationBL.Delete(hidId.Value.ToInt());
                    lblMessage.Text = S_DELETE_MESSAGE;
                    ClearFields();
                    FillItemSpecificationDetails();
                    SetFields();
                    
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillItemSpecificationDetails();
            }

            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        txtSpecificationCode.Focus();
        hidId.Value = Constants.S_ZERO;
        hidItemID.Value = QueryString["ItemID"].ToString();
        hidItemName.Value = QueryString["ItemName"].ToString();
        valsumItems.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button>() { btnItemSave, btnCancel, btnBack });
        chkIsDamaged.Attributes.Add("onclick", "SetFieldStatus(this)");
        btnCancel.Attributes.Add("onclick", "SetFieldState(false)");
        btnItemSave.Attributes.Add("onclick","ClearMessages()");
    }
    
    /// <summary>
    /// This method is used to set post back url on back button.
    /// </summary>
    private void SetPostBackUrl()
    {
        string sQueryString = "ItemName=" + hidItemName.Value.ToString();
        string sEncriptedQueryString = Utility.CommonUtility.EncryptQuerystring(sQueryString.ToString());
        btnBack.Attributes.Add("onclick", "window.open('../Inventory/ItemManagementUI.aspx?" + sEncriptedQueryString + " ' , '_self');return false;");
    }

    /// <summary>
    /// This method is used to fill listview details.
    /// </summary>
    private void FillItemSpecificationDetails()
    {
        lstvwItemSpecificationDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This method is used to set fields values.
    /// </summary>
    private void SetFields()
    {   
        int iItemId = Convert.ToInt32(hidItemID.Value);
        StockDetailsBL moStockDetailsBL = new StockDetailsBL(miSchoolId, miUserId);
        StockItemDetails oStockItemDetails = moStockDetailsBL.GetStockItemDetails(iItemId);
        spnItemName.InnerText = oStockItemDetails.ItemName;
        spnItemCode.InnerText = oStockItemDetails.ItemCode;
        spnCurrentStock.InnerText = oStockItemDetails.ItemQuantityWithUnits.ToString();
    }

    /// <summary>
    /// This method is used to clear controls. 
    /// </summary>
    private void ClearFields()
    {

        txtSpecificationCode.Text = string.Empty;
        txtItemPrice.Text = string.Empty;
        txtDescription.Text = string.Empty;
        chkIsDamaged.Checked = false;
        txtDamagedDt.Text = string.Empty;
        txtDamagedDiscription.Text = string.Empty;
        hidId.Value = Constants.S_ZERO;
        btnItemSave.Text = Constants.ButtonText.Save.ToString(); ;
        btnItemSave.Enabled = true;

        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "ClearControlesValues();", true);
    }

    /// <summary>
    /// This method is used to Populate object.
    /// </summary>
    /// <returns></returns>
    private ItemSpecificationDetails Populate()
    {
        ItemSpecificationDetails oItemSpecificationDetails = new ItemSpecificationDetails
        {
            Id = hidId.Value.ToInt(),
            ItemID = hidItemID.Value.ToInt(),
            SpecificationCode = txtSpecificationCode.Text.Trim(),
            Description = txtDescription.Text,
            IsDamaged = (chkIsDamaged.Checked ? true : false),
            DamagedDate = txtDamagedDt.Text,
            DamageDescription = txtDamagedDiscription.Text,
            Price = txtItemPrice.Text.Length == Constants.I_ZERO ? Constants.S_ZERO : txtItemPrice.Text
        };
        return oItemSpecificationDetails;
    }

    #endregion
}