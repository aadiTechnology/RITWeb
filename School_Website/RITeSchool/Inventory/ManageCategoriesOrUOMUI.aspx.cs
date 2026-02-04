// File Name  : ManageCategoriesOrUOMUI.aspx.cs
// Created By : Deepak
// Date       : 26/2/2010
//Description : This class is used to add/edit/delete Item categories/Unit of measurement.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Data.SqlClient;

public partial class ManageCategoriesOrUOMUI : SchoolBase
{
    #region Constants

    const string S_REMOVE = "REMOVE";
    const string S_UPDATE_CATEGORIES_UOM = "UPDATE_CATEGORIES_UOM";
    const string S_EDIT = "EDIT";
    const string S_MODE_NEW = "NEW";

    #endregion

    #region  Events
    /// <summary>
    /// This event is used to set default properties and 
    /// javascript attributes for contols and fill listview
    /// for item category or unit of measurement.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {   
                SetJavaScriptAttributes();
                SetLabel();
                FillCategoriesORUOMGrid();
                SetDefaultValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill listview with item categories and Reset contols.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optItemCategory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetLabel();
            ResetControls();
            FillCategoriesORUOMGrid();
            SetListControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill listview with unit of measurement and Reset contols.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optUOM_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetLabel();
            ResetControls();
            FillCategoriesORUOMGrid();
            SetListControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to insert/update item categories/unit of measurement 
    /// and displys successfull or error message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SetLabel();
            if (optUOM.Checked)
            {
                UOMMasterBL oUOMMasterBL = PopulateUOMMasterBL();
                if (oUOMMasterBL.IsDuplicateUOM())
                {
                    if (hidMode.Value != S_EDIT)
                    {
                        oUOMMasterBL.InsertUOMMaster();
                        lblMessage.Visible = true;
                        lblMessage.Text = "Unit of Measurment saved successfully!!! ";
                    }
                    else
                    {
                        int UOMId = Convert.ToInt32(hidUOMId.Value);
                        oUOMMasterBL.UpdateUOMMaster(UOMId);                        
                        lblMessage.Visible = true;
                        lblMessage.Text = "Unit of Measurment updated successfully!!! ";
                    }                   
                }
            }
            else
            {
                ItemCategoryMasterBL oItemCategoryMasterBL = PopulateItemCategoryMasterBL();
                if (oItemCategoryMasterBL.IsDuplicateItemCategory())
                {
                    if (hidMode.Value != S_EDIT)
                        oItemCategoryMasterBL.InsertItemCategoryMaster();
                    else
                    {
                        int iCategoryId = Convert.ToInt32(hidCategoryId.Value);
                        oItemCategoryMasterBL.UpdateItemCategoryMaster(iCategoryId);
                    }
                    lblMessage.Visible = true;
                    lblMessage.Text = "Item Category saved successfully!!! ";
                }
            }
            ClearFields();
            FillCategoriesORUOMGrid();
        }
        catch (DuplicateEntityException Ex)
        {
            lblError.Visible = true;
            lblError.Text = Ex.ErrorMessage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to reset controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetLabel();
            ResetControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set contols in update mode or delete item category/unit of measurement.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwCategoriesOrUOM_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iListIndex = oCurrentItem.DisplayIndex;
            int iUOMId = Convert.ToInt32(lstvwCategoriesOrUOM.DataKeys[iListIndex]["UOMID"]);
            int iCategoryId = Convert.ToInt32(lstvwCategoriesOrUOM.DataKeys[iListIndex]["ItemCategoryID"]);
            hidUOMId.Value = Convert.ToString(iUOMId);
            hidCategoryId.Value = Convert.ToString(iCategoryId);
            if (e.CommandName == S_REMOVE)
                DeleteCategoriesOrUOM(optUOM.Checked, iUOMId, iCategoryId, miSchoolId);
            else if (e.CommandName == S_UPDATE_CATEGORIES_UOM)
            {
                FillControlsForUpdate(optUOM.Checked, iUOMId, iCategoryId, miSchoolId);

                hidMode.Value = S_EDIT;
            }
            SetLabel();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to display confirmation message, on click of delete button in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwCategoriesOrUOM_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                HtmlTableCell tdPieceCount = e.Item.FindControl("tdPieceCount") as HtmlTableCell;
                if (optUOM.Checked)               
                    tdPieceCount.Visible = true; 
                else
                    tdPieceCount.Visible = false;
                ImageButton oImageButtonEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                ImageButton oImageButtonDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                int iIsUOMUsed = lstvwCategoriesOrUOM.DataKeys[e.Item.DisplayIndex]["IsUsed"].ToInt();
                if (iIsUOMUsed == 1)
                {
                    oImageButtonEdit.Visible = false;
                    oImageButtonDelete.Visible = false;
                }
                else
                {
                    oImageButtonEdit.Visible = true;
                    oImageButtonDelete.Visible = true;
                }
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to set Displaycount tr & listview column visible as per option checked.
    /// </summary>
    private void SetListControls()
    {
        if (optUOM.Checked)
            DisplayCount.Visible = true;
        else
            DisplayCount.Visible = false;
              
            HtmlTableRow tr = lstvwCategoriesOrUOM.FindControl("trHeader") as HtmlTableRow;

            if (tr != null)
            {
                HtmlTableCell thPieceCount = tr.FindControl("thPieceCount") as HtmlTableCell;
                if (thPieceCount != null)
                {
                    if (optUOM.Checked == false)
                        thPieceCount.Visible = false;
                    else
                        thPieceCount.Visible = true;
                }
            }        
    }

    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnClose });
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        btnClose.Attributes["onclick"] = "window.opener.location.href = window.opener.location.href;window.close();window.opener.focus();";
    }
    /// <summary>
    /// This method set validation summary header and make item category option checked as default.
    /// </summary>
    private void SetDefaultValues()
    {
        optItemCategory.Checked = true;
        valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        SetListControls();
    }
    /// <summary>
    /// This method resets the controls.
    /// </summary>
    private void ResetControls()
    {
        txtName.Text = string.Empty;
        txtPieceCount.Text = string.Empty;
        hidCategoryId.Value = string.Empty;
        hidUOMId.Value = string.Empty;
        hidMode.Value = "New";
    }
    /// <summary>
    /// This event is used to fill listview as per option checked,eaither by item categories or unit of measurement.
    /// </summary>
    private void FillCategoriesORUOMGrid()
    {
        DataTable oDTCategoriesORUOM = null;
        if (optUOM.Checked)
            oDTCategoriesORUOM = UOMMasterBL.GetAll(miSchoolId);
        else
            oDTCategoriesORUOM = ItemCategoryMasterBL.GetAll(miSchoolId);
        lstvwCategoriesOrUOM.DataSource = oDTCategoriesORUOM;
        lstvwCategoriesOrUOM.DataBind();
    }
    /// <summary>
    /// This method is used to set lable as per option checked.
    /// </summary>
    private void SetLabel()
    {
        if (optUOM.Checked)
            lblName.Text = "Unit of Measurment :";
        else
            lblName.Text = "Item Category :";        
    }

    /// <summary>
    /// This method used to populate ItemCategoryMasterBL object.
    /// </summary>
    /// <returns></returns>
    private ItemCategoryMasterBL PopulateItemCategoryMasterBL()
    {
        ItemCategoryMasterBL oItemCategoryMasterBL = new ItemCategoryMasterBL();
        oItemCategoryMasterBL.ItemCategoryID = 0;
        oItemCategoryMasterBL.School_Id = miSchoolId;
        oItemCategoryMasterBL.ItemCategoryName = txtName.Text.Trim();
        oItemCategoryMasterBL.InsertedById = miUserId;
        if (hidMode.Value == S_EDIT)
            oItemCategoryMasterBL.ItemCategoryID = Convert.ToInt32(hidCategoryId.Value);
        return oItemCategoryMasterBL;
    }
    /// <summary>
    /// This method used to populate UOMMasterBL object.
    /// </summary>
    /// <returns></returns>
    private UOMMasterBL PopulateUOMMasterBL()
    {
        UOMMasterBL oUOMMasterBL = new UOMMasterBL();
        oUOMMasterBL.UOMID = 0;

        oUOMMasterBL.School_Id = miSchoolId;
        oUOMMasterBL.UOMUnit = txtName.Text.Trim();
        oUOMMasterBL.UOMPieceCount = Convert.ToDecimal(txtPieceCount.Text.Trim());
        oUOMMasterBL.InsertedById = miUserId;
        if (hidMode.Value == S_EDIT)
            oUOMMasterBL.UOMID = Convert.ToInt32(hidUOMId.Value);
        return oUOMMasterBL;

    }
    /// <summary>
    /// This method sets controls in update mode for updating item category/unit of measurment.
    /// </summary>
    /// <param name="bCategoryOrUOM"></param>
    /// <param name="iUOMId"></param>
    /// <param name="iCategoryId"></param>
    /// <param name="iSchoolID"></param>
    private void FillControlsForUpdate(bool bCategoryOrUOM, int iUOMId, int iCategoryId, int iSchoolID)
    {
        if (bCategoryOrUOM)
        {
            UOMMasterBL oUOMMasterBL = new UOMMasterBL(iUOMId, iSchoolID);
            txtName.Text = oUOMMasterBL.UOMUnit;
            txtPieceCount.Text = Convert.ToString(oUOMMasterBL.UOMPieceCount);
        }
        else
        {
            ItemCategoryMasterBL oItemCategoryMasterBL = new ItemCategoryMasterBL(iCategoryId, iSchoolID);
            txtName.Text = oItemCategoryMasterBL.ItemCategoryName;
        }
    }
    /// <summary>
    /// This method is used to delete item category/unit of measurment.
    /// </summary>
    /// <param name="bCategoryOrUOM"></param>
    /// <param name="iUOMId"></param>
    /// <param name="iCategoryId"></param>
    /// <param name="iSchoolID"></param>
    private void DeleteCategoriesOrUOM(bool bCategoryOrUOM, int iUOMId, int iCategoryId, int iSchoolID)
    {
        string IsDependent;
        if (bCategoryOrUOM)
        {
            UOMMasterBL oUOMMasterBL = new UOMMasterBL();
            IsDependent = oUOMMasterBL.DeleteUOMMaster(iSchoolID, iUOMId);
        }
        else
        {
            ItemCategoryMasterBL oItemCategoryMasterBL = new ItemCategoryMasterBL();
            IsDependent = oItemCategoryMasterBL.DeleteItemCategoryMaster(iSchoolID, iCategoryId);
        }
        SetLabel();
        string sName = lblName.Text.Trim();
        sName = sName.Replace(":", " ");
        if (IsDependent == "Y")
            lblError.Text = " This " + sName + "can not be deleted since it is associated with item(s).";
        else
        {
            lblMessage.Visible = true;
            if (bCategoryOrUOM)
                lblMessage.Text = "Unit of Measurment deleted successfully!!!";
            else
                lblMessage.Text = "Item Category deleted successfully!!!";
        }
        FillCategoriesORUOMGrid();
        ResetControls();
    }
    public void ClearFields()
    {
        txtName.Text = string.Empty;
        txtPieceCount.Text = string.Empty;
        hidMode.Value = S_MODE_NEW;
    }
    #endregion

}
