// File Name - SectionDetailsPopup.aspx.cs
// Creator - Sachin
// Created Date - 
// Descrption - This class is used to configure income tax section details.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class SectionDetailsPopup : SchoolBase
{
    #region Constant(s)

    private const string S_SORT_ORDER = "SortOrder";
    private const string S_MAX_AMOUNT = "MaxAmount";

    #endregion

    #region Data Member(s)

    private SectionDetailsBL moSectionDetailsBL;

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
            base.AddSortImage(lstvwSections, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill section details list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moSectionDetailsBL = new SectionDetailsBL(miSchoolId, miFinancialYearId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillSectionGroups();                
                FillSectionDetails();
            }
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
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete section details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSections_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                int iSectionId = Convert.ToInt32(lstvwSections.DataKeys[e.Item.DisplayIndex]["Id"]);
                int iSectionGroupId = Convert.ToInt32(lstvwSections.DataKeys[e.Item.DisplayIndex]["SectionGroupId"]);
                Label lblName = e.Item.FindControl("lblName") as Label;
                Label lblSortOrder = e.Item.FindControl("lblSortOrder") as Label;

                List<SectionDetails> lstSectionDetails = moSectionDetailsBL.GetAll();
                SectionDetails oSectionDetails = lstSectionDetails.Where(sd => sd.Id == iSectionId).FirstOrDefault();
                txtSectionName.Text = oSectionDetails.Name;
                txtSortOrder.Text = oSectionDetails.SortOrder.ToString();
                txtMaxAmount.Text = oSectionDetails.MaxAmount.ToDecimal().ToString();
                hidSectionId.Value = iSectionId.ToString();
                cmbSectionGroup.SelectedValue = iSectionGroupId.ToString();

                if (cmbSectionGroup.SelectedValue.ToInt() == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt())
                {
                    cmbCategory.Enabled = true;
                    txtMaxAmount.Enabled = true;
                    cmbCategory.SelectedValue = oSectionDetails.CategoryId.ToString();
                }
                else
                {
                    cmbCategory.ClearSelection();
                    cmbCategory.Enabled = false;
                    txtMaxAmount.Enabled = false;
                }

                BtnSave.Text = Constants.ButtonText.Update.ToString();
            }
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                int iSectionId = Convert.ToInt32(lstvwSections.DataKeys[e.Item.DisplayIndex]["Id"]);
                moSectionDetailsBL.Delete(iSectionId);
                FillSectionDetails();                
                cmbSectionGroup.ClearSelection();
                DisplayMessage(Constants.ItemState.deleted, false);
                if (hidSectionId.Value == iSectionId.ToString())
                    ResetFields();
            }
            else if (e.CommandName == Constants.S_COMMAND_SORT)
            {
                List<SectionDetails> lstSectionDetails = moSectionDetailsBL.GetAll();
                if (hidSortDirection.Value == Constants.S_ASCENDING)
                {
                    if (e.CommandArgument.ToString() == S_SORT_ORDER)
                        lstSectionDetails = lstSectionDetails.OrderByDescending(sd => sd.SortOrder).ToList();
                    else if(e.CommandArgument.ToString() == S_MAX_AMOUNT)
                        lstSectionDetails = lstSectionDetails.OrderByDescending(sd => sd.MaxAmount).ToList();
                    else
                        lstSectionDetails = lstSectionDetails.OrderByDescending(sd => sd.Name).ToList();

                    hidSortDirection.Value = Constants.S_DESCENDING;
                }    
                else
                {
                    if (e.CommandArgument.ToString() == S_SORT_ORDER)
                        lstSectionDetails = lstSectionDetails.OrderBy(sd => sd.SortOrder).ToList();
                    else if (e.CommandArgument.ToString() == S_MAX_AMOUNT)
                        lstSectionDetails = lstSectionDetails.OrderBy(sd => sd.MaxAmount).ToList();
                    else
                        lstSectionDetails = lstSectionDetails.OrderBy(sd => sd.Name).ToList();
                    hidSortDirection.Value = Constants.S_ASCENDING;
                }

                lstvwSections.DataSource = lstSectionDetails;
                lstvwSections.DataBind();
            }
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to attribute for image button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSections_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                int iRowIndex = oCurrentItem.DisplayIndex;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                Label lblMaxAmount = e.Item.FindControl("lblMaxAmount") as Label;
                if (Convert.ToDecimal(lblMaxAmount.Text) == 0)
                    lblMaxAmount.Text = "-";                

                int iCategoryId = lstvwSections.DataKeys[iRowIndex]["CategoryId"].ToInt();
                Label lblCategory = e.Item.FindControl("lblCategory") as Label;

                if (iCategoryId == Constants.I_ZERO)
                    lblCategory.Text = "-";
                else if (iCategoryId == Constants.SectionCategories.A.ToInt())
                    lblCategory.Text = Constants.SectionCategories.A.ToString();
                else
                    lblCategory.Text = Constants.SectionCategories.B.ToString();
                    
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save section details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SectionDetails oSectionDetails = new SectionDetails
            {
                Id = Convert.ToInt32(hidSectionId.Value),
                Name = txtSectionName.Text.Trim(),
                SortOrder = Convert.ToInt32(txtSortOrder.Text),
                SectionGroupId = Convert.ToInt32(cmbSectionGroup.SelectedValue),
                MaxAmount = Convert.ToDecimal(txtMaxAmount.Text == string.Empty ? 0 : Convert.ToDecimal(txtMaxAmount.Text)),
                CategoryId = cmbCategory.SelectedValue.ToInt()
            };

            moSectionDetailsBL.SectionDetails = oSectionDetails;
            moSectionDetailsBL.Save();

            DisplayMessage(hidSectionId.Value == Constants.S_ZERO ? Constants.ItemState.saved : Constants.ItemState.updated, false);
            ResetFields();
            FillSectionDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort expression.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSections_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { BtnSave, BtnCancel, btnClose });
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = "SortOrder";
        cmbSectionGroup.Focus();
        BtnSave.Attributes.Add("onclick", "SetState()");
        hidCategoryFor.Value =Convert.ToString(Constants.SectionGroups.DeductionUnderChapterVIA.ToInt());
        txtMaxAmount.Attributes.Add("onchange", "CheckValue(this)");
    }

    /// <summary>
    /// This method is used to fill section details in list view.
    /// </summary>
    private void FillSectionDetails()
    {
        List<SectionDetails> lstSectionDetails = moSectionDetailsBL.GetAll();
        lstvwSections.DataSource = lstSectionDetails;
        lstvwSections.DataBind();
    }

    /// <summary>
    /// This method is used to fill section group details in list view.
    /// </summary>
    private void FillSectionGroups()
    {
        List<SectionGroup> lstSectionGroup = moSectionDetailsBL.GetAllSectionGroups();
        ListSource.FillDropDownList(lstSectionGroup, cmbSectionGroup, "Name", "Id", Constants.S_SELECT);
    }
    
    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        txtSectionName.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        hidSectionId.Value = Constants.S_ZERO;
        cmbSectionGroup.ClearSelection();
        cmbCategory.ClearSelection();
        cmbCategory.Enabled = false;
        txtMaxAmount.Text = Constants.S_ZERO;
        BtnSave.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Section " + aoItemState.ToString() + " successfully !!!";
        DisplayMessage(sMessage, abIsErrorMessage,tdMessage);
    }

    #endregion
}