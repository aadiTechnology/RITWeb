/*File Name - StoreItemVariationDetailsUI.aspx.cs
 * Created Date - 12-Jan-2024
 * Created By - Sachin
 * Description - This class is used to manage sote item variations.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.eStoreBL;
using BusinessLogic.Exceptions;
using SchoolEntities.eStore;
using Utility;
using System.Linq;
using SchoolEntities;
using System.Data;
using System.Web.Script.Serialization;

public partial class StoreItemVariationDetailsUI : SchoolBase
{
    #region Data Member(s)

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

            AddSortImage(lstvwVariationDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill colrs, sizes and variation details.
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
                FillColors();
                FillSizes();
                FillUOM();
                FillGST();
                SetDefaultValues();
                FillVariations();
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
    protected void btnCanel_Click(object sender, EventArgs e)
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

    /// <summary>
    /// This event is used to save variation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {                
                StoreItemVariationDetails oStoreItemVariationDetails = Populate();
                moStoreItemVariationBL.Save(oStoreItemVariationDetails);
                lblMessage.Text = "Item variation details saved successfully !!!";
                ClearFields();
                FillVariations();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
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
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    SetValueForUpdate(iId);
                }
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moStoreItemVariationBL.Delete(iId);
                    lblMessage.Text = "Item variation details deleted successfully !!!";
                    ClearFields();
                    FillVariations();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set confirmation message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVariationDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                HiddenField hidQueryString = e.Item.FindControl("hidQueryString") as HiddenField;
                LinkButton lnkbtnFreeItem = e.Item.FindControl("lnkbtnFreeItem") as LinkButton;

                StoreItemVariationDetails oStoreItemVariationDetails = e.Item.DataItem as StoreItemVariationDetails;

                if (oStoreItemVariationDetails.IsFreeItemExist)
                    lnkbtnFreeItem.Text = "Edit";
                else
                    lnkbtnFreeItem.Text = "Add";

                int iId = Convert.ToInt32(lstvwVariationDetails.DataKeys[e.Item.DisplayIndex]["Id"]);
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring("BaseItemVariationId=" + iId + "&ItemType=V");
                hidQueryString.Value = sEncrypt;                
                lnkbtnFreeItem.Attributes.Add("onclick", "OpenFeeItemPopup("+e.Item.DisplayIndex+"); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to validate duplications.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Validate_Duplication(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator obj = sender as CustomValidator;
            int iTypeId;
            if (obj == custValColorSize)
                iTypeId = 2;
            else if (obj == custValItemCode)
                iTypeId = 3;
            else
                iTypeId = 1;

            string sMessage = moStoreItemVariationBL.Validate(txtTitle.Text.Trim(), cmbColor.SelectedValue.ToInt(), cmbSize.SelectedValue.ToInt(), hidId.Value.ToInt(), miSchoolId, miAcademicYearId, iTypeId, hidStoreItemMasterId.Value.ToInt(), txtItemCode.Text.Trim());
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

    /// <summary>
    /// This event is used to search record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataPager oDataPager = lstvwVariationDetails.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager != null)
                oDataPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, true);

            FillVariations();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StoreItemMasterId"] != null)
        {
            hidStoreItemMasterId.Value = QueryString["StoreItemMasterId"].ToString();

            StoreItemDetailsBL oStoreItemDetailsBL = new StoreItemDetailsBL(miSchoolId,miAcademicYearId,miUserId);
            StoreItemDetails oStoreItemDetails = oStoreItemDetailsBL.GetStoreItemDetails(hidStoreItemMasterId.Value.ToInt());
            lblItemName.Text = oStoreItemDetails.Title;
            lblGender.Text = oStoreItemDetails.Gender == "M" ? "Boy" : "Girl";

            hidBaseTitle.Value = oStoreItemDetails.Title;
            hidBasePrice.Value = oStoreItemDetails.Price.ToString();
            hidBaseQuantity.Value = oStoreItemDetails.Quantity.ToString();
            hidBaseReordQty.Value = oStoreItemDetails.ReOrderQuantity.ToString();

            hidBaseItemCode.Value = oStoreItemDetails.ItemCode;
            hidBaseUOMId.Value = oStoreItemDetails.UOMId.ToString();
            hidBaseMRP.Value = oStoreItemDetails.MRP.ToString();
            hidBaseDiscount.Value = oStoreItemDetails.Discount.ToString();
            hidBaseGSTId.Value = oStoreItemDetails.GSTCategoryId.ToString();
            hidBaseHSNCode.Value = oStoreItemDetails.HSNCode;
        }
        else
            hidStoreItemMasterId.Value = Constants.S_ZERO;

        if (QueryString["StoreCategoryName"] != null)
        {
            lblStoreItemCategory.Text = QueryString["StoreCategoryName"].ToString();
            hidStoreCategoryName.Value = QueryString["StoreCategoryName"].ToString();
        }
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        base.SetDefaultButton(btnSearch);
        btnSave.Attributes.Add("onclick", "ResetMessage();");
        hidAttachmentCount.Value = Constants.S_ZERO;

        string sQueryString = CommonUtility.EncryptQuerystring("StoreCategoryId=" + QueryString["StoreCategoryId"].ToString() + "&Filter=" + QueryString["Filter"].ToString() + "&OriginalStandardIds=" + QueryString["OriginalStandardIds"].ToString());
        btnBack.PostBackUrl = "StoreItemListUI.aspx?" + sQueryString;

        txtMRP.Attributes.Add("onchange", "SetAmount()");
        txtDiscount.Attributes.Add("onchange", "SetAmount()");
        cmbGST.Attributes.Add("onchange", "SetAmount()");
    }

    /// <summary>
    /// This method is used to fill colors.
    /// </summary>
    private void FillColors()
    {
        List<Color> lstColor = moStoreItemVariationBL.GetColors();
        ListSource.FillDropDownList(lstColor, cmbColor, "Name", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to sizes.
    /// </summary>
    private void FillSizes()
    {
        List<StoreItemSize> lstStoreItemSizes = moStoreItemVariationBL.GetStoreItemSizes();
        ListSource.FillDropDownList(lstStoreItemSizes, cmbSize, "Size", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        cmbColor.ClearSelection();
        cmbSize.ClearSelection();
        txtPrice.Text = string.Empty;
        txtQuantity.Text = string.Empty;
        txtReorderQuantity.Text = string.Empty;
        txtTitle.Text = string.Empty;
        hidId.Value = Constants.S_ZERO;
        hidAttachmentCount.Value = Constants.S_ZERO;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        trAttachments.Visible = false;
        cmbUOM.ClearSelection();
        cmbGST.ClearSelection();
        txtItemCode.Text = string.Empty;
        txtHSNCode.Text = string.Empty;
        txtMRP.Text = string.Empty;
        txtDiscount.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to populate entity object.
    /// </summary>
    /// <returns></returns>
    private StoreItemVariationDetails Populate()
    {
        string sFileIdsToDelete = string.Empty;
        if (hidDeleteedIds.Value.StartsWith(","))
            sFileIdsToDelete = hidDeleteedIds.Value.Substring(1);
        else
            sFileIdsToDelete = hidDeleteedIds.Value;
        
        StoreItemVariationDetails oStoreItemVariationDetails = new StoreItemVariationDetails
        {
            ColorId = cmbColor.SelectedValue.ToInt(),
            FileNames = GetFileNames(),
            Id = hidId.Value.ToInt(),
            Price = txtPrice.Text.ToDecimal(),
            Quantity = txtQuantity.Text.ToInt(),
            ReorderQuantity = txtReorderQuantity.Text.ToInt(),
            SizeId = cmbSize.SelectedValue.ToInt(),
            StoreItemMasterId = hidStoreItemMasterId.Value.ToInt(),
            Title = txtTitle.Text.Trim(),
            FileIdsToDelete = sFileIdsToDelete,
            ItemCode = txtItemCode.Text.Trim(),
            UOMId = cmbUOM.SelectedValue.ToInt(),
            GSTCategoryId = cmbGST.SelectedValue.ToInt(),
            HSNCode = txtHSNCode.Text.Trim(),
            MRP = txtMRP.Text.ToDecimal(),
            Discount = (txtDiscount.Text.Trim() != string.Empty?txtDiscount.Text.ToDecimal():Constants.I_ZERO)
        };
        return oStoreItemVariationDetails;
    }

    /// <summary>
    /// This method is used to fill variations.
    /// </summary>
    private void FillVariations()
    {
        lstvwVariationDetails.DataSourceID = objdsVariations.ID;
        lstvwVariationDetails.DataBind();
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
    /// This method is used to set values for updation.
    /// </summary>
    /// <param name="aiId"></param>
    private void SetValueForUpdate(int aiId)
    {
        StoreItemVariationDetails oStoreItemVariationDetails = moStoreItemVariationBL.Get(aiId);
        hidId.Value = aiId.ToString();
        cmbColor.SelectedValue = oStoreItemVariationDetails.ColorId.ToString();
        cmbSize.SelectedValue = oStoreItemVariationDetails.SizeId.ToString();
        txtTitle.Text = oStoreItemVariationDetails.Title;
        txtPrice.Text = oStoreItemVariationDetails.Price.ToString();
        txtQuantity.Text = oStoreItemVariationDetails.Quantity.ToString();
        txtReorderQuantity.Text = oStoreItemVariationDetails.ReorderQuantity.ToString();

        txtItemCode.Text = oStoreItemVariationDetails.ItemCode;

        if (oStoreItemVariationDetails.ItemCode != string.Empty)
        {
            img.ImageUrl = "../LibrarianManagement/Handler.ashx?id=" + oStoreItemVariationDetails.ItemCode;
            img.Visible = true;
        }

        cmbUOM.SelectedValue = oStoreItemVariationDetails.UOMId.ToString();
        cmbGST.SelectedValue = oStoreItemVariationDetails.GSTCategoryId.ToString();
        txtHSNCode.Text = oStoreItemVariationDetails.HSNCode;
        txtMRP.Text = oStoreItemVariationDetails.MRP.ToString();
        txtDiscount.Text = oStoreItemVariationDetails.Discount.ToString();

        btnSave.Text = Constants.ButtonText.Update.ToString();
        hidAttachmentCount.Value = oStoreItemVariationDetails.ImageFileList.Count.ToString();
        FillAttachments(oStoreItemVariationDetails.ImageFileList);
        trAttachments.Visible = true;
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


    private void FillGST()
    {
        PODetailsBL oPODetailsBL = new PODetailsBL();
        List<GSTCategory> lstGSTCategory = oPODetailsBL.GetGSTCategory();
        lstGSTCategory = lstGSTCategory.OrderBy(gst => gst.Id).ToList();
        ListSource.FillDropDownList(lstGSTCategory, cmbGST, "Name", "Id", Constants.S_SELECT);

        var jsSerializer = new JavaScriptSerializer();
        hidGSTData.Value = jsSerializer.Serialize(lstGSTCategory);
    }

    private void FillUOM()
    {
        DataTable dtUOM = UOMMasterBL.GetAll(miSchoolId);
        ListSource.FillDropDownList(dtUOM, cmbUOM, "Name", "UOMId", Constants.S_SELECT);
    }

    #endregion
}