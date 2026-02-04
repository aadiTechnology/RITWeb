/* Class Name - GSTInvoiceDetailsUI
 * Created By - Vishakha
 * Created On - 24-Jun-2022
 * Description - This class is used to manage GST invoice details.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Web.Script.Serialization;

public partial class GSTInvoiceDetailsUI : SchoolBase
{
    #region Constant(s)

    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_SAVE_MSG = "GST Invoice details saved successfully !!!";
    private const string S_UPDATE_MSG = "GST Invoice details updated successfully !!!";
    private const string S_DELETE_MSG = "GST Invoice details deleted successfully !!!";
    private const string S_COMMAND_DELETE = "DeleteReceiverDetails";
    private const string S_COMMAND_UPDATE = "UpdateReceiverDetails"; 

    #endregion

    #region Data Member(s)

    private GSTInvoiceDetailsBL moGSTInvoiceDetailsBL; 

    #endregion

    #region Event(s)
    /// <summary>
    /// THis event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "InvoiceNo";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwReceiverDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill GSt category, receiver details and GST details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moGSTInvoiceDetailsBL = new GSTInvoiceDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillReceiverName();
                FillGSTCategory();
                FillDescriptions(0);
                FillGSTInvoiceDetails();
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit, delete and sorting activity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwReceiverDetails.DataKeys[iRowId]["Id"]);

                if (e.CommandName == S_COMMAND_UPDATE)
                    SetControlsForEditMode(iId);
                else if (e.CommandName == S_COMMAND_DELETE)
                {
                    Delete(iId);
                    ResetFields();
                    FillDescriptions(0);
                    FillGSTInvoiceDetails();
                }
            }
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// THis event is used to fill GST Invoice details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GSTInvoiceDetails oGSTInvoiceDetails = e.Item.DataItem as GSTInvoiceDetails;
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDelete") as ImageButton;
                
                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                Label lblInvoiceDate1 = e.Item.FindControl("lblInvoiceDate1") as Label;
                lblInvoiceDate1.Text = oGSTInvoiceDetails.InvoiceDate.ToString(Constants.S_DATE_FORMAT);

                ListView lstvwDescription = e.Item.FindControl("lstvwDescription") as ListView;
                if (lstvwDescription != null)
                {
                    lstvwDescription.DataSource = oGSTInvoiceDetails.Descriptions;
                    lstvwDescription.DataBind();
                }

                //It is to show inner listview.
                LinkButton LnkBtnDetails = e.Item.FindControl("LnkBtnDetails") as LinkButton;
                LnkBtnDetails.Attributes.Add("onclick", "ShowDescription(" + e.Item.DisplayIndex + "); return false;");

                int iId = Convert.ToInt32(lstvwReceiverDetails.DataKeys[e.Item.DisplayIndex]["Id"].ToString());
                string sQueryString = CommonUtility.EncryptQuerystring("Id=" + iId );

                HiddenField hidData1 = e.Item.FindControl("hidData1") as HiddenField;
                hidData1.Value = sQueryString;

                LinkButton lnkExport = e.Item.FindControl("lnkExport") as LinkButton;
                lnkExport.Attributes.Add("onclick", "OpenReport(" + e.Item.DisplayIndex + "); return false;");     
       
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// THis event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwReceiverDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwReceiverDetails, DtPgCount);
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
    /// This event is used to sort variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwReceiverDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

   /// <summary>
   /// This event is used to save GST details.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.IsValid)
            {
                Save();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel GST details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            FillGSTInvoiceDetails();
            btnSave.Text = S_TEXT_SAVE;
            LblUpdateSuccess.Text = "";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search GST details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillGSTInvoiceDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill description.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGSTDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GSTInvoiceDescription oGSTInvoiceDescription = e.Item.DataItem as GSTInvoiceDescription;
                TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                if (oGSTInvoiceDescription.Amount != 0)
                    txtAmount.Text = oGSTInvoiceDescription.Amount.ToString();

                txtAmount.Attributes.Add("onchange", "SetTotalAmount();");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill non duplicate Invoice no.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    protected void Validate_InvoiceNo(object obj, ServerValidateEventArgs e)
    {
        bool bIsValid = moGSTInvoiceDetailsBL.IsInvoiceNoDuplicate(hidId.Value.ToInt(), txtInvoiceNo.Text.Trim());
        e.IsValid = !bIsValid;
    }

    protected void hidData_ValueChanged(object sender, EventArgs e)
    {
        string sUserId = ddlReceiverName.SelectedValue;
        FillReceiverName();
        ddlReceiverName.SelectedValue = sUserId;
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to populate GST details.
    /// </summary>
    /// <param name="iId"></param>
    /// <returns></returns>
    private GSTInvoiceDetails PopulateGStDetails(int iId)
    {
        GSTInvoiceDetails oGSTInvoice = new GSTInvoiceDetails
        {
            Id = iId,
            ReceiverId = ddlReceiverName.SelectedValue.ToInt(),
            InvoiceNo = txtInvoiceNo.Text,
            InvoiceDate = txtInvoiceDate.Text.ToDateTime(),
            TotalAmount = txtTotal.Text.ToDecimal(),
            GSTCategoryId = ddlGSTCategory.SelectedValue.ToInt(),
            CGST = txtCGST.Text.ToDecimal(),
            SGST = txtSGST.Text.ToDecimal(),
            FinalAmount = txtTotalAmount.Text.ToDecimal(),
            AdditionalRemark = txtAdditionalRemark.Text
        };
        return oGSTInvoice;
    }

    /// <summary>
    /// This method is used to populate Description.
    /// </summary>
    /// <param name="aiInvoiceId"></param>
    /// <returns></returns>
    private List<GSTInvoiceDescription> PopulateDescriptions(int aiInvoiceId)
    {
        List<GSTInvoiceDescription> oGSTInvoiceDetails = new List<GSTInvoiceDescription>();
        {
            foreach (ListViewDataItem item in lstvwGSTDetails.Items)
            {
                TextBox oTextBoxDescription = item.FindControl("txtDescription") as TextBox;
                TextBox oTextBoxAmount = item.FindControl("txtAmount") as TextBox;
                int iId = lstvwGSTDetails.DataKeys[item.DisplayIndex]["Id"].ToInt();

                if (oTextBoxAmount.Text.Trim() != string.Empty && oTextBoxAmount.Text.Trim() != Constants.S_ZERO && oTextBoxDescription.Text.Trim() != string.Empty)
                {
                    oGSTInvoiceDetails.Add(new GSTInvoiceDescription
                    {
                        Id = iId,
                        GSTInvoiceId = aiInvoiceId,
                        Description = oTextBoxDescription.Text.Trim(),
                        Amount = oTextBoxAmount.Text.ToInt()
                    });
                }
            }

            return oGSTInvoiceDetails;
        }
    }

    /// <summary>
    /// This method is used to save GST details.
    /// </summary>
    private void Save()
    {
        int iId = 0;
        if (hidId.Value != string.Empty)
        {
            iId = Convert.ToInt32(hidId.Value);
        }

        GSTInvoiceDetails oGSTInvoiceDetails = PopulateGStDetails(iId);
        List<GSTInvoiceDescription> oGSTInvoiceDetails1 = PopulateDescriptions(iId);
        
        string sXml = base.GenerateXml(oGSTInvoiceDetails1);

        moGSTInvoiceDetailsBL.Save(sXml, oGSTInvoiceDetails);
        if (btnSave.Text == S_TEXT_SAVE)
            LblUpdateSuccess.Text = S_SAVE_MSG;
        else
        {
            LblUpdateSuccess.Text = S_UPDATE_MSG;
            btnSave.Text = S_TEXT_SAVE;
        }
        hidId.Value = "0";
        
        FillGSTInvoiceDetails();
        ResetFields();
    }

   /// <summary>
   /// This method is used to edit GST details.
   /// </summary>
   /// <param name="aiId"></param>
    private void SetControlsForEditMode(int aiId)
    {
        btnSave.Text = S_TEXT_UPDATE;
        hidId.Value = aiId.ToString();

        GSTInvoiceDetails oGSTInvoiceDetails = moGSTInvoiceDetailsBL.Get(aiId);
        ddlReceiverName.SelectedValue = oGSTInvoiceDetails.ServiceReceiverId.ToString();
        txtInvoiceNo.Text = oGSTInvoiceDetails.InvoiceNo;
        txtInvoiceDate.Text = oGSTInvoiceDetails.InvoiceDate.ToString(Constants.S_DATE_FORMAT);
        txtTotal.Text = oGSTInvoiceDetails.TotalAmount.ToString();
        ddlGSTCategory.SelectedValue = oGSTInvoiceDetails.GSTCategoryId.ToString();
        txtCGST.Text = oGSTInvoiceDetails.CGST.ToString();
        txtSGST.Text = oGSTInvoiceDetails.SGST.ToString();
        txtTotalAmount.Text = oGSTInvoiceDetails.FinalAmount.ToString();
        txtAdditionalRemark.Text = oGSTInvoiceDetails.AdditionalRemark;

        FillDescriptions(aiId);
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
    /// This method is used to delete perticular record of receiver details.
    /// </summary>
    /// <param name="InvoiceNo"></param>
    private void Delete(int aiId)
    {
        moGSTInvoiceDetailsBL.Delete(aiId);
        LblUpdateSuccess.Text = S_DELETE_MSG;
    }

   /// <summary>
   /// This method is used to reset fields.
   /// </summary>
    private void ResetFields()
    {
        ddlReceiverName.ClearSelection();
        ddlGSTCategory.ClearSelection();
        txtTotalAmount.Text = string.Empty;
        txtInvoiceDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtInvoiceNo.Text = string.Empty;
        txtSearch.Text = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        txtCGST.Text = string.Empty;
        txtSGST.Text = string.Empty;
        txtTotal.Text = string.Empty;
        txtAdditionalRemark.Text = string.Empty;
        FillDescriptions(0);
        ddlReceiverName.Focus();
    }

    /// <summary>
    /// This method is used to fill Receiver name dropdown.
    /// </summary>
    private void FillReceiverName()
    {
        List<ReceiverName> lstReceiverName = moGSTInvoiceDetailsBL.GetReceiverName();
        ListSource.FillDropDownList(lstReceiverName, ddlReceiverName, "Name", "ReceiverId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill GSTCategoryId dropdown
    /// </summary>
    private void FillGSTCategory()
    {
        List<GSTCategory> lstGSTCategory = moGSTInvoiceDetailsBL.GetGSTCategory();
        ListSource.FillDropDownList(lstGSTCategory, ddlGSTCategory, "Name", "Id", Constants.S_SELECT);
        
        var jsSerializer = new JavaScriptSerializer();
		hidGSTData.Value = jsSerializer.Serialize(lstGSTCategory);
    }

    /// <summary>
    /// This method is used to fill Description listview.
    /// </summary>
    /// <param name="aiId"></param>
    private void FillDescriptions(int aiId)
    {
        List<GSTInvoiceDescription> lstGSTDetails = moGSTInvoiceDetailsBL.GetGSTDescriptions(aiId);
        lstvwGSTDetails.DataSource = lstGSTDetails;
        lstvwGSTDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill GST Invoice details listview.
    /// </summary>
    private void FillGSTInvoiceDetails()
    {
        lstvwReceiverDetails.DataSourceID = objdsReceiverDetails.ID;
        lstvwReceiverDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ResetLabel()");
        txtInvoiceNoPrefix.Text = Settings.InvoiceNoPrefix;
        lnkReceiverDetails.Attributes.Add("onclick", "OpenReceiverPopup(); return false;");
        lnkBankDetails.Attributes.Add("onclick", "OpenBankPopup(); return false;");
        ddlGSTCategory.Attributes.Add("onchange", "SetTotalAmount();");
    }

    #endregion    
}