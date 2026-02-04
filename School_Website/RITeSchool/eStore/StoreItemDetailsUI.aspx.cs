/* Class Name - StoreItemDetailsUI
 * Created By - Vishakha
 * Created On - 20 dec 2023
 * Description - This class is used to store item details.
 */

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Text;
using BusinessLogic.eStoreBL;
using SchoolEntities.eStore;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Linq;
using System.Data;
using SchoolEntities;
using System.Web.Script.Serialization;

public partial class StoreItemDetailsUI : SchoolBase
{
    #region Data Member(s)

    private StoreItemDetailsBL moStoreItemDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill dropdown, checkbox and hide controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStoreItemDetailsBL = new StoreItemDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                ReadQueryString();
                SetDefaultValues();
                FillStandardChkBoxList();
                FillUOM();
                FillGST();

                if (QueryString["Id"] != null && QueryString["Id"].ToString() != Constants.S_ZERO)
                {
                    int iId = QueryString["Id"].ToInt();
                    hidId.Value = iId.ToString();
                    SetControlsForEditMode(iId);
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else
                    hidId.Value = Constants.S_ZERO;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save item details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                StoreItemDetails oStoreItemDetails = Populate();
                int iStoreItemMasterId = moStoreItemDetailsBL.Save(oStoreItemDetails);

                if (ChkVariation.Checked)
                {
                    string sQueryString = "StoreItemMasterId=" + iStoreItemMasterId + "&StoreCategoryId=" + QueryString["StoreCategoryId"].ToString() + "&Filter=" + QueryString["Filter"].ToString() + "&OriginalStandardIds=" + QueryString["OriginalStandardIds"].ToString() + "&StoreCategoryName=" + QueryString["StoreCategoryName"].ToString() + "&ItemName=" + txtTitle.Text.Trim();
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                    string sRedirectUrl = "StoreItemVariationDetailsUI.aspx" + "?" + sEncrypt;
                    MasterPage oMasterPage = (MasterPage)this.Master;
                    oMasterPage.RedirectToNextPage(sRedirectUrl);
                }
                else
                {
                    string sQueryString = CommonUtility.EncryptQuerystring("StoreCategoryId=" + QueryString["StoreCategoryId"].ToString() + "&Filter=" + QueryString["Filter"].ToString() + "&OriginalStandardIds=" + QueryString["OriginalStandardIds"].ToString());
                    MasterPage oMasterPage = this.Master as MasterPage;
                    oMasterPage.RedirectToNextPage("StoreItemListUI.aspx?" + sQueryString);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void Validate_Duplication(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator obj = sender as CustomValidator;
            int iTypeId;
            if (obj == custValItemCode)
                iTypeId = 3;
            else
                iTypeId = 1;

            string sMessage = moStoreItemDetailsBL.Validate(txtTitle.Text.Trim(), hidId.Value.ToInt(), miSchoolId, miAcademicYearId, iTypeId, txtItemCode.Text.Trim());
            if (sMessage == string.Empty)
                e.IsValid = true;
            else
            {
                obj.ErrorMessage = sMessage;
                e.IsValid = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to populate item details.
    /// </summary>
    /// <returns></returns>
    private StoreItemDetails Populate()
    {
        string sFileIdsToDelete = string.Empty;
        if (hidDeleteedIds.Value.StartsWith(","))
            sFileIdsToDelete = hidDeleteedIds.Value.Substring(1);
        else
            sFileIdsToDelete = hidDeleteedIds.Value;

        StoreItemDetails oStoreItemDetails = new StoreItemDetails();

        oStoreItemDetails.StoreCategoryId = hidStoreCategoryId.Value.ToInt();
        oStoreItemDetails.Title = txtTitle.Text.Trim();
        oStoreItemDetails.Description = txtDescription.Text.Trim();

        if (rdGender.SelectedValue != string.Empty)
            oStoreItemDetails.Gender = rdGender.SelectedValue;
        else
            oStoreItemDetails.Gender = string.Empty;

        oStoreItemDetails.AssociatedStandards = GetSelectedStandards();
        oStoreItemDetails.AvailabilitySetting = ChkAvailability.Checked;

        if (ChkAvailability.Checked)
        {
            oStoreItemDetails.StartDate = txtStartDate.Text.ToDateTime();
            oStoreItemDetails.EndDate = txtEndDate.Text.ToDateTime();
        }
        else
        {
            oStoreItemDetails.StartDate = DateTime.MinValue;
            oStoreItemDetails.EndDate = DateTime.MinValue;
        }

        oStoreItemDetails.Price = txtPrice.Text.ToDecimal();
        oStoreItemDetails.Quantity = txtQuantity.Text.ToInt();
        oStoreItemDetails.ReOrderQuantity = txtReorderQty.Text.ToInt();
        oStoreItemDetails.IsVariation = ChkVariation.Checked;
        oStoreItemDetails.Id = hidId.Value.ToInt();
        oStoreItemDetails.ImageFileNames = GetFileNames();
        oStoreItemDetails.FileIdsToDelete = sFileIdsToDelete;

        oStoreItemDetails.ItemCode = txtItemCode.Text.Trim();
        oStoreItemDetails.UOMId = cmbUOM.SelectedValue.ToInt();
        oStoreItemDetails.GSTCategoryId = cmbGST.SelectedValue.ToInt();
        oStoreItemDetails.HSNCode = txtHSNCode.Text.Trim();
        oStoreItemDetails.MRP = txtMRP.Text.ToDecimal();

        if (txtDiscount.Text.Trim() != string.Empty)
            oStoreItemDetails.Discount = txtDiscount.Text.ToDecimal();
        else
            oStoreItemDetails.Discount = Constants.I_ZERO;

        return oStoreItemDetails;
    }

    /// <summary>
    /// This mehod is used to get selected std from checkbox list.
    /// </summary>
    /// <returns></returns>
    private string GetSelectedStandards()
    {
        StringBuilder sb = new StringBuilder();
        foreach (ListItem oItem in ChkStandards.Items)
        {
            if (oItem.Selected == true)
                sb.Append("," + oItem.Value);
        }

        if (sb.Length > 0)
            return sb.ToString().Substring(1);
        else
            return string.Empty;
    }

    /// <summary>
    /// This method is used to get details in edit mode.
    /// </summary>
    /// <param name="aiId"></param>
    private void SetControlsForEditMode(int aiId)
    {        
        StoreItemDetails oStoreItemDetails = moStoreItemDetailsBL.GetStoreItemDetails(aiId);
        txtTitle.Text = oStoreItemDetails.Title;
        txtDescription.Text = oStoreItemDetails.Description;
        rdGender.SelectedValue = oStoreItemDetails.Gender;
        ChkStandards.SelectedValue = oStoreItemDetails.AssociatedStandards;
        ChkAvailability.Checked = oStoreItemDetails.AvailabilitySetting;

        if (oStoreItemDetails.AvailabilitySetting)
        {
            txtStartDate.Text = oStoreItemDetails.StartDate.ToString(Constants.S_DATE_FORMAT);
            txtEndDate.Text = oStoreItemDetails.EndDate.ToString(Constants.S_DATE_FORMAT);
        }

        ChkVariation.Checked = oStoreItemDetails.IsVariation;
        txtPrice.Text = oStoreItemDetails.Price.ToString();
        txtQuantity.Text = oStoreItemDetails.Quantity.ToString();
        txtReorderQty.Text = oStoreItemDetails.ReOrderQuantity.ToString();

        txtItemCode.Text = oStoreItemDetails.ItemCode;
        img.ImageUrl = "../LibrarianManagement/Handler.ashx?id=" + oStoreItemDetails.ItemCode;
        img.Visible = true;
        cmbUOM.SelectedValue = oStoreItemDetails.UOMId.ToString();
        cmbGST.SelectedValue = oStoreItemDetails.GSTCategoryId.ToString();
        txtHSNCode.Text = oStoreItemDetails.HSNCode;
        txtMRP.Text = oStoreItemDetails.MRP.ToString();
        txtDiscount.Text = oStoreItemDetails.Discount.ToString();
        
        foreach (ListItem oItem in ChkStandards.Items)
        {
            if (oStoreItemDetails.StandardList.Contains(oItem.Value.ToInt()))
                oItem.Selected = true;
        }

        trAttachments.Visible = true;
        hidAttachmentCount.Value = oStoreItemDetails.AttachmentsDetails.Count.ToString();
        FillAttachments(oStoreItemDetails.AttachmentsDetails);
        btnClear.Visible = false;

        hidAreVariotionExists.Value = (oStoreItemDetails.AreVariationExists ? Constants.S_YES : Constants.S_NO);
    }

    /// <summary>
    /// This method is used to fill attachments.
    /// </summary>
    /// <param name="alstAttachment"></param>
    private void FillAttachments(List<Attachment> alstAttachment)
    {
        alstAttachment = alstAttachment.OrderBy(at => at.ImageFileName).ToList();
        int iIndex = 1;
        foreach (Attachment file in alstAttachment)
        {
            HtmlTableRow tr = new HtmlTableRow();

            HtmlTableCell td = new HtmlTableCell();
            td.Align = "left";
            td.ID = "td_" + file.Id;
            HyperLink hyper = new HyperLink();
            hyper.ID = "hyper_" + file.Id;
            hyper.Target = "_new";
            SetAttachment(file.ImageFileName, hyper);
            td.Controls.Add(hyper);
            tr.Controls.Add(td);

            HtmlTableCell tdAction = new HtmlTableCell();
            ImageButton img = new ImageButton();
            img.CausesValidation = false;
            img.ID = "img_" + file.Id;
            img.ImageUrl = "../images/IconGrid_Delete.gif";
            img.Attributes.Add("onclick", "HideAttachment('" + file.Id + "'); return false;");
            tdAction.Controls.Add(img);

            HiddenField hf = new HiddenField();
            hf.ID = "hidden_" + file.Id;
            hf.Value = Constants.S_ZERO;
            tdAction.Controls.Add(hf);

            HiddenField hfLinkValue = new HiddenField();
            hfLinkValue.ID = "hiddenLinkValue_" + file.Id;
            hfLinkValue.Value = file.ImageFileName;
            tdAction.Controls.Add(hfLinkValue);

            tr.Controls.Add(tdAction);

            tblAttachments.Rows.Add(tr);

            iIndex++;
        }
    }

    /// <summary>
    /// This method is used to set attachment.
    /// </summary>
    /// <param name="sAttachment"></param>
    /// <param name="lnkAttach"></param>
    private void SetAttachment(string sAttachment, HyperLink lnkAttach)
    {
        string sAttachmentURL = sAttachment;
        sAttachment = sAttachment.Replace("'", "\\\'");
        sAttachment = sAttachment.Replace("%", "%25");
        sAttachment = sAttachment.Replace("#", "%23");
        string sServerFilePath = "../Uploads/eStore/" + sAttachment;

        int iTimestampIndex = sAttachment.IndexOf("$");
        if (iTimestampIndex > -1)
            sAttachment = sAttachment.Remove(iTimestampIndex, 15);

        int iIndex = sAttachmentURL.IndexOf("$");
        if (iTimestampIndex > -1)
            sAttachmentURL = sAttachmentURL.Remove(iIndex, 15);
        lnkAttach.Text = sAttachmentURL;
        string sExtention = sAttachment.Substring(sAttachment.LastIndexOf(".") + 1).ToUpper();
        lnkAttach.Attributes.Add("onclick",
                                     String.Format("window.open('{0}','{1}').focus(); return false;",
                                                    sServerFilePath,
                                                    "_blank"));
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtTitle.Text = string.Empty;
        txtDescription.Text = string.Empty;
        ChkStandards.ClearSelection();
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtPrice.Text = string.Empty;
        txtQuantity.Text = string.Empty;
        txtReorderQty.Text = string.Empty;
        ChkAvailability.Checked = false;
        ChkVariation.Checked = false;
        hidId.Value = Constants.S_ZERO;
        hidAttachmentCount.Value = Constants.S_ZERO;
        ChkSelectAllStd.Checked = false;
        trAttachments.Visible = false;
        hidAreVariotionExists.Value = Constants.S_NO;
    }
   
    /// <summary>
    /// This method is used to fill standards checkbox list.
    /// </summary>
    private void FillStandardChkBoxList()
    {
        List<StandardList> lstStandard = moStoreItemDetailsBL.GetStandardList();
        ListSource.FillCheckBoxList(lstStandard, ChkStandards, "Standard_Name", "Original_Standard_Id");
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StoreCategoryId"] != null)
            hidStoreCategoryId.Value = QueryString["StoreCategoryId"].ToString();
        else
            hidStoreCategoryId.Value = Constants.S_ZERO;

        if (QueryString["StoreCategoryName"] != null)
            lblStoreCategoryName.Text = QueryString["StoreCategoryName"].ToString();

        if (hidStoreCategoryId.Value == Constants.S_ONE)
        {
            trGender.Visible = true;
            trVariation.Visible = true;
        }
        else
        {
            trGender.Visible = false;
            trVariation.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ValErrorMsgGenerate.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        string sQueryString = CommonUtility.EncryptQuerystring("StoreCategoryId=" + QueryString["StoreCategoryId"].ToString() + "&Filter=" + QueryString["Filter"].ToString() + "&OriginalStandardIds=" + QueryString["OriginalStandardIds"].ToString());
        btnCancel.PostBackUrl = "StoreItemListUI.aspx?" + sQueryString;
        hidAttachmentCount.Value = Constants.S_ZERO;

        btnSave.Attributes.Add("onclick","if(!ConfirmVaritionDelete()) return false;");
        trAttachments.Visible = false;

        txtDiscount.Text = Constants.S_ZERO;

        txtMRP.Attributes.Add("onchange", "SetAmount()");
        txtDiscount.Attributes.Add("onchange", "SetAmount()");
        cmbGST.Attributes.Add("onchange", "SetAmount()");
    }

    /// <summary>
    /// This method is used to get uploaded file.
    /// </summary>
    /// <returns></returns>
    private string GetFileNames()
    {
        List<string> lstFiles = new List<string>();
        HttpFileCollection oCollection = Request.Files;
        for (int iCount = 0; iCount < oCollection.Count; iCount++)
        {
            HttpPostedFile aoAttachment = oCollection[iCount];

            string sFileName = aoAttachment.FileName;
            string sServerFilePath;

            if (sFileName.Trim() != string.Empty)
            {
                sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");
                sServerFilePath = base.BasePath + "/RITeSchool/UPLOADS/eStore/" + sFileName;
                aoAttachment.SaveAs(sServerFilePath);
                lstFiles.Add(sFileName);
            }
        }

        string sFileNames = string.Join(",", lstFiles);
        return sFileNames;
    }

    /// <summary>
    /// This method is used to fill GST categories.
    /// </summary>
    private void FillGST()
    {
        PODetailsBL oPODetailsBL = new PODetailsBL();
        List<GSTCategory> lstGSTCategory = oPODetailsBL.GetGSTCategory();
        lstGSTCategory = lstGSTCategory.OrderBy(gst => gst.Id).ToList();
        ListSource.FillDropDownList(lstGSTCategory, cmbGST, "Name", "Id", Constants.S_SELECT);

        var jsSerializer = new JavaScriptSerializer();
        hidGSTData.Value = jsSerializer.Serialize(lstGSTCategory);
    }

    /// <summary>
    /// This method is used to fill UOM.
    /// </summary>
    private void FillUOM()
    {
        DataTable dtUOM = UOMMasterBL.GetAll(miSchoolId);
        ListSource.FillDropDownList(dtUOM, cmbUOM, "Name", "UOMId", Constants.S_SELECT);
    }

    #endregion
}