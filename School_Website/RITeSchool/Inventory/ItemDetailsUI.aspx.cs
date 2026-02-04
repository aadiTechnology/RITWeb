// File Name  : ItemDetailsUI.aspx.cs
// Created By : Amit
// Date       : 26/06/2009
// Description: This class is used to add/edit item details. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities.Inventory;
using SchoolEntities;
using System.Linq;
using PayrollReportingUserEntities;
using System.Configuration;
using System.Text;
using System.IO;
public partial class ItemDetailsUI : SchoolBase
{
    #region " Constants "

    //Table Indices
    const int I_TBL_ITEM_UOM = 0;
    const int I_TBL_ITEM_CATEGORY = 1;
    const int I_TBL_ITEM_CODE = 2;
    const int I_TBL_ITEM_GST = 3;
    private const int I_FILE_SIZE_LIMIT = 1048576;
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Inventory Items\\";
    private const string S_FILE_SIZE_ERROR = "Image size should not be greater than 1 MB";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Inventory Items/";
    private const string S_DELETE_MESSAGE = "Image file deleted successfully!!!";
    const string S_SCREENS_URL = "AddRequisitionUI.aspx";
    static string msFromUrl = string.Empty;
    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to set master page according to login user.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            string sFromPage = string.Empty;

            if (Request.QueryString.ToString() != string.Empty)
            {
                if (QueryString["FromPage"] != null)
                    sFromPage = QueryString["FromPage"];
            }

            //if (msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL || msFromUrl.TrimAll() == string.Empty)
            //    this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
            //else
            //    this.Page.MasterPageFile = "../MasterPages/MasterPage.master";

            if (sFromPage == S_SCREENS_URL)
                msFromUrl = sFromPage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill item category and unit of measurement combo.
    /// Set default control, decrypt query string and fill item detail controls as per new/edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQueryString();
                SetDefaultProperties();
                FillAllControl();
                FillVendor();  
                if (QueryString.Count > 0)
                {
                    AssignControlsAsPerMode();
                }
                else if (SchoolBase.Settings.IsAaryanSchool)  //for aaryan school
                 {
                     //FillVendor();  //fill the vendor name dropdown
                     if (hidIsNewItem.Value == "True") //
                     {
                         txtItemCode.Text = SetDefaultPriority();
                     }
                txtItemCode.Enabled = false;
                }
                if (msFromUrl.Equals(S_SCREENS_URL) || msFromUrl.TrimAll() == string.Empty)
                {                    
                    btnAddAndContinue.Visible = false;
                    btnBack.Visible = false;
                    hidIsFromRequisitionScreen.Value = Constants.S_YES;
                }

                else
                { 
                    btnBack.PostBackUrl = "~/RITeSchool/Inventory/ItemManagementUI.aspx";
                }

            }
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                txtItemQuantity.Text = Constants.S_ZERO;
                txtItemQuantity.Enabled = false;
                cmbSelectedUnitsUOMQty.Enabled = false;
            }

            hidFilePath.Value = Constants.S_ZERO;
           
        }
            
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillVendor()  //fill vendor dropdown 
    {
        
        var oStandardCollectionBL = new ItemsMasterBL(miSchoolId, miAcademicYearId);
        DataTable  oDtStandardCollection = oStandardCollectionBL.GetAllVendor(miSchoolId, miAcademicYearId);
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbVendor, "VendorId", "CompanyName", "--Select--");
       
    }
    /// <summary>
    /// This method is used to set default Item code 
    /// </summary>
    /// <returns></returns>
    /// 
    private string SetDefaultPriority()   //for autoItemCode AaryanSchool
    {
        ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
        return (oItemsMasterBL.GetHighestPriority(miSchoolId).ToString());
    }

    /// <summary>
    /// This event is used to manage item details and control goes to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnItemSave_Click(object sender, EventArgs e)
    {
        try
        {            
            if (SchoolBase.Settings.IsAaryanSchool)  //for aaryan school
            {              
                txtItemCode.Enabled = false;
            }
            
            AddItem();            
            BackToPreviousPage();            
        }
        catch (DuplicateEntityException Ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.ErrorMessage;
            txtItemName.Focus();
        }
        catch (UploadFileExceptions ex)
        {
            lblErr.Text = ex.Message;
        }
        catch (ApplicationException ex1)
        {
            lblErr.Text = ex1.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    protected void btnAddAndContinue_Click(object sender, EventArgs e)
    {
        try
        {
            if (SchoolBase.Settings.IsAaryanSchool)  //for aaryan school
            {


                txtItemCode.Enabled = false;
            }
            AddItem();
            ResetItemDetailControls();
            FillAllControl();
            txtItemCode.Text = SetDefaultPriority();
        }
        catch (DuplicateEntityException Ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.ErrorMessage;
            txtItemName.Focus();
        }
        catch (UploadFileExceptions ex)
        {
            lblErr.Text = ex.Message;
        }
        catch (ApplicationException ex1)
        {
            lblErr.Text = ex1.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go previous page. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidIsFromRequisitionScreen.Value == Constants.S_YES)
            {
                Response.Write("<Script language='Javascript'> window.close(); </Script>");
            }
            else            
                BackToPreviousPage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    #endregion " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }


    private void ReadQueryString()
    {
        hidItemName.Value = string.Empty;
        hidITemCode.Value = string.Empty;
        hidItemCAtegory.Value = Constants.S_ZERO;

        if (QueryString["ItemName"] != null)
            hidItemName.Value = QueryString["ItemName"];

        if (QueryString["ItemCode"] != null)
            hidITemCode.Value = QueryString["ItemCode"];

        if (QueryString["ItemCategory"] != null)
            hidItemCAtegory.Value = QueryString["ItemCategory"];
    }
    /// <summary>
    /// This method used to clear all controls.
    /// </summary>
    private void ResetItemDetailControls()
    {
        txtItemName.Text = string.Empty;
        txtItemCode.Text = string.Empty;
        txtItemQuantity.Text = string.Empty;
        txtItemPrice.Text = string.Empty;
        txtReorderLevel.Text = string.Empty;
        chkIsConsiderForDetailLevel.Checked = false;
        txtMake.Text = string.Empty;
        ddlUOM.SelectedIndex = 0;
        ddlCategory.SelectedIndex = 0;
        btnView.Visible = false;
        btnView1.Visible = false;
        btnView2.Visible = false;
        btnDelete.Visible = false;
        btnDelete1.Visible = false;
        btnDelete2.Visible = false;
        cmbGST.ClearSelection();
        txtHall.Text = string.Empty;  //hall
        txtRack.Text = string.Empty; //rack
        txtShelf.Text = string.Empty; //shelf
        txtInvoiceNo.Text = string.Empty;
        cmbVendor.ClearSelection();
    }

    /// <summary>
    /// This method used to fill 'Unit Of Measurement' and 'Item Category' combo box.
    /// </summary>
    private void FillAllControl()
    {
        ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
        DataSet oDSItemInfo = oItemsMasterBL.GetAddItemDetails(miSchoolId);
        ControlUtility.FillDropDownList(oDSItemInfo.Tables[I_TBL_ITEM_UOM], ref ddlUOM, "UOMID", "UOMUnit", "--Select--");
        ControlUtility.FillDropDownList(oDSItemInfo.Tables[I_TBL_ITEM_CATEGORY], ref ddlCategory, "ItemCategoryID", "ItemCategoryName", "--Select--");
        ControlUtility.FillDropDownList(oDSItemInfo.Tables[I_TBL_ITEM_GST], ref cmbGST, "Id", "Name", "--Select--");
        SetValuesForSelectedUOM();

        //txtItemCode.Text = oDSItemInfo.Tables[I_TBL_ITEM_CODE].Rows[0][0].ToString();
    }

    private void SetValuesForSelectedUOM()
    {
        string sUOMUnitText = ddlUOM.SelectedItem.Text;
        int iUOMIndexValue = ddlUOM.SelectedIndex;

        if (iUOMIndexValue == Constants.I_ZERO)
        {
            cmbSelectedUnitsUOMQty.Items.Clear();
            cmbSelectedUnitsReorderQty.Items.Clear();
            cmbSelectedUnitsUOMQty.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ZERO });
            cmbSelectedUnitsReorderQty.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });
        }
        else
        {
            cmbSelectedUnitsUOMQty.Items.Clear();
            cmbSelectedUnitsReorderQty.Items.Clear();
            cmbSelectedUnitsUOMQty.Items.Add(new ListItem { Text = sUOMUnitText, Value = Constants.S_ZERO });
            cmbSelectedUnitsUOMQty.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });
            cmbSelectedUnitsReorderQty.Items.Add(new ListItem { Text = sUOMUnitText, Value = Constants.S_ZERO });
            cmbSelectedUnitsReorderQty.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });
        }
    }

    /// <summary>
    /// This method is used to set default control .
    /// </summary>
    private void SetDefaultProperties()
    {
        valsumItems.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lblErrorMsg.Visible = false;
        btnAddAndContinue.Visible = true;

        ResetItemDetailControls();
        txtItemName.Focus();
        hidIsNewItem.Value = "True";
        btnItemSave.Text = "Add";

        btnItemSave.Attributes.Add("onclick", "ClearMessages();");
        btnAddAndContinue.Attributes.Add("onclick", "ClearMessages();");

        ApplyMouseHoverEffect(new List<Button> { btnCancel,btnBack, btnAddAndContinue, btnItemSave });

        btnCancel.Attributes.Add("onclick", "if(!ClosePopup()) return false;");
    }

    /// <summary>
    /// This method used populate ItemsMasterBL.
    /// </summary>
    /// <returns></returns>
    private ItemsMasterBL PopulateItemsMasterBL()
    {
        ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();

        oItemsMasterBL.ItemID = 0;
        oItemsMasterBL.ItemName = txtItemName.Text.Trim();
        oItemsMasterBL.ItemCode = txtItemCode.Text;
        oItemsMasterBL.Hall = txtHall.Text.Trim();  //hall
        oItemsMasterBL.RackNo = txtRack.Text.Trim();  //RackNo
        oItemsMasterBL.ShelfNo = txtShelf.Text.Trim();  //ShelfNo
        oItemsMasterBL.InvoiceNo = txtInvoiceNo.Text.Trim(); //Invoice no
 
        //if (SchoolBase.Settings.IsAaryanSchool)  //for aaryan school
        //{
            oItemsMasterBL.VendorId = Convert.ToInt32(cmbVendor.SelectedValue);  //vendor
        //}
        //else
        //{
        //    oItemsMasterBL.VendorId = 0;
        
        //}

        if (txtItemPrice.Text != string.Empty)
            oItemsMasterBL.ItemPrice = Convert.ToDecimal(txtItemPrice.Text);
        else
            oItemsMasterBL.ItemPrice = Convert.ToDecimal(Constants.S_ZERO);

        
        oItemsMasterBL.UOMID = Convert.ToInt32(ddlUOM.SelectedValue);
        
        oItemsMasterBL.ItemCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
        
        oItemsMasterBL.ItemQty = Convert.ToDecimal(txtItemQuantity.Text);
        
        oItemsMasterBL.ItemReorderLevelQty = Convert.ToDecimal(txtReorderLevel.Text);
        
        if (chkIsConsiderForDetailLevel.Checked)
            oItemsMasterBL.IsConsiderForDetailLevel = true;
        else
            oItemsMasterBL.IsConsiderForDetailLevel = false;
        oItemsMasterBL.Make = txtMake.Text;
        oItemsMasterBL.SchoolId = miSchoolId;
        oItemsMasterBL.InsertedById = miUserId;
        oItemsMasterBL.UpdatedById = miUserId;
        oItemsMasterBL.IsDeleted = false;
        oItemsMasterBL.GSTCategoryId = cmbGST.SelectedValue.ToInt();

        oItemsMasterBL.ConsiderUnitQuantity = cmbSelectedUnitsUOMQty.SelectedValue.ToInt();
        oItemsMasterBL.ConsiderUnitReorderLevel = cmbSelectedUnitsReorderQty.SelectedValue.ToInt();

        if (hidIsNewItem.Value == "False")
            oItemsMasterBL.ItemID = Convert.ToInt32(hidItemID.Value);
        
        oItemsMasterBL.ImageXml = base.GenerateXml(PopulateImage());

        return oItemsMasterBL;
    }

    /// <summary>
    /// This method is used for assigning control with the respected values in Edit/Add Copy mode.
    /// </summary>
    private void AssignControlsAsPerMode()
    {
        if (QueryString["ItemID"] != null && QueryString["IsEditMode"] != null)
        {
            string sItemID = QueryString["ItemID"];
            string sModeType = QueryString["IsEditMode"];
            if (sItemID != null && sModeType != null)
            {
                hidItemID.Value = sItemID;
                hidModeType.Value = sModeType;

                if (hidModeType.Value == "Edit")
                {
                    SetItemDetailsForEdit();
                    chkIsConsiderForDetailLevel.Enabled = false;
                }
            }
        }
       
    }

    /// <summary>
    /// This method is used to set controls at edit mode.
    /// </summary>
    private void SetItemDetailsForEdit()
    {
        int iItemId = Convert.ToInt32(hidItemID.Value);
        ItemsMasterBL oItemsMasterBL = new ItemsMasterBL(iItemId, miSchoolId);
        txtItemCode.Text = oItemsMasterBL.ItemCode;
        txtItemName.Text = oItemsMasterBL.ItemName;
        txtItemPrice.Text = Convert.ToDecimal(oItemsMasterBL.ItemPrice).ToString();
        txtItemQuantity.Text = Convert.ToDecimal(oItemsMasterBL.ItemQty).ToString();
        txtHall.Text = Convert.ToString(oItemsMasterBL.Hall);  //hall
        txtRack.Text = Convert.ToString(oItemsMasterBL.RackNo); //rackNo
        txtShelf.Text = Convert.ToString(oItemsMasterBL.ShelfNo); //shelf
        txtInvoiceNo.Text = Convert.ToString(oItemsMasterBL.InvoiceNo); //InvoicNO
        FillVendor();
        cmbVendor.SelectedValue = Convert.ToString(oItemsMasterBL.VendorId); //vendor 
        if (SchoolBase.Settings.IsAaryanSchool)  //for aaryan school
        {
            txtItemCode.Enabled = false;
        }
      
        //if (oItemsMasterBL.IsIssued)
        //{
        //    //txtItemQuantity.Enabled = false;
        //    //cmbSelectedUnitsUOMQty.Enabled = false;
        //    txtItemQuantity.Enabled = true;
        //    cmbSelectedUnitsUOMQty.Enabled = true;
        //}
        //else
        //{
        //    txtItemQuantity.Enabled = true;
        //    cmbSelectedUnitsUOMQty.Enabled = true;
        //}

        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();

        if (!lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowItemBalanceUpdation.ToInt() && ru.UserId == miUserId).Any())
        {
            txtItemQuantity.Enabled = false;
            cmbSelectedUnitsUOMQty.Enabled = false;
        }

        chkIsConsiderForDetailLevel.Checked = oItemsMasterBL.IsConsiderForDetailLevel;
        txtReorderLevel.Text = Convert.ToDecimal(oItemsMasterBL.ItemReorderLevelQty).ToString();
        txtMake.Text = oItemsMasterBL.Make;
        ddlCategory.SelectedValue = Convert.ToString(oItemsMasterBL.ItemCategoryID);
        ddlUOM.SelectedValue = Convert.ToString(oItemsMasterBL.UOMID);
        cmbGST.SelectedValue = oItemsMasterBL.GSTCategoryId.ToString();
       
        hidItemID.Value = Convert.ToString(oItemsMasterBL.ItemID);
        hidIsNewItem.Value = "False";
        btnItemSave.Text = "Update";
        btnAddAndContinue.Visible = false;
        SetValuesForSelectedUOM();
        SetValuesAsPerQty(oItemsMasterBL);
        // List<ItemImageDetails> lstvwItemImage = new List<ItemImageDetails>();

        List<ItemImageDetails> lstItemImage = oItemsMasterBL.GetImagesUrl(iItemId);


        if (lstItemImage != null && lstItemImage.Count > 0)
        {
            var result = (from Item in lstItemImage where Item.ControlId == 1 select Item).FirstOrDefault();
            if (result != null)
            {
                btnView.Visible = true;
                btnDelete.Visible = true;
                string sNewFileName = S_FOLDER_PATH + result.ImageUrl;
                btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            }
            else
            {
                btnView.Visible = false;
                btnDelete.Visible = false;
            }

            var result1 = (from Item in lstItemImage where Item.ControlId == 2 select Item).FirstOrDefault();
            if (result1 != null)
            {
                btnView1.Visible = true;
                btnDelete1.Visible = true;
                string sNewFileName = S_FOLDER_PATH + result1.ImageUrl;
                btnView1.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            }
            else
            {
                btnView1.Visible = false;
                btnDelete1.Visible = false;
            }

            var result2 = (from Item in lstItemImage where Item.ControlId == 3 select Item).FirstOrDefault();
            if (result2 != null)
            {
                btnView2.Visible = true;
                btnDelete2.Visible = true;
                string sNewFileName = S_FOLDER_PATH + result2.ImageUrl;
                btnView2.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            }
            else
            {
                btnView2.Visible = false;
                btnDelete2.Visible = false;
            }
        }

    }

    private void SetValuesAsPerQty(ItemsMasterBL oItemsMasterBL)
    {
        if (oItemsMasterBL.ConsiderUnitQuantity.ToString() == Constants.S_ZERO)
        {
            int mod = oItemsMasterBL.ItemQty.ToInt() % oItemsMasterBL.PieceCount.ToInt();
            if (mod == 0)
            {
                decimal iQuanityInUOM = oItemsMasterBL.ItemQty.ToDecimal() / oItemsMasterBL.PieceCount.ToInt();///
                txtItemQuantity.Text = iQuanityInUOM.ToString();
                cmbSelectedUnitsUOMQty.SelectedValue = Constants.S_ZERO;
            }
            else
                cmbSelectedUnitsUOMQty.SelectedValue = Constants.S_ONE;
        }

        if (oItemsMasterBL.ConsiderUnitReorderLevel.ToString() == Constants.S_ZERO)
        {
            int mod = oItemsMasterBL.ItemReorderLevelQty.ToInt() % oItemsMasterBL.PieceCount.ToInt();
            if (mod == 0)
            {
                decimal iQuanityInUOM = oItemsMasterBL.ItemReorderLevelQty.ToDecimal() / oItemsMasterBL.PieceCount.ToInt();
                txtReorderLevel.Text = iQuanityInUOM.ToString();
                cmbSelectedUnitsReorderQty.SelectedValue = Constants.S_ZERO;
            }
            else
                cmbSelectedUnitsReorderQty.SelectedValue = Constants.S_ONE;
        }
    }

    /// <summary>
    /// This method is used to move previous page. 
    /// </summary>
    private void BackToPreviousPage()
    {

        //if (hidIsFromRequisitionScreen.Value == Constants.S_YES)
        //{   
        //    string sEditQuerystring = "ItemCode=" + txtItemCode.Text.Trim();
        //    string sEditEncrypt = Utility.CommonUtility.EncryptQuerystring(sEditQuerystring.ToString());

        //    sEditEncrypt = string.Format("'?{0}'", sEditEncrypt);
        //    Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", sEditEncrypt));
        //}
        //else
        //{
            string sName = hidItemName.Value.ToString();
            string sItemCode = hidITemCode.Value.ToString();
            double dCategory = hidItemCAtegory.Value.ToDouble();
            string sBtnStatus = "Search";
            string sEditQuerystring = "ItemName=" + sName + "&ItemCode=" + sItemCode + "&ItemCategory=" + dCategory + "&btnStatus=" + sBtnStatus;
            string sEditEncrypt = Utility.CommonUtility.EncryptQuerystring(sEditQuerystring.ToString());

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Inventory/ItemManagementUI.aspx?" + sEditEncrypt);
        //}

    }

    private void AddItem()
    {
        ItemsMasterBL oItemsMasterBL = PopulateItemsMasterBL();
        
        if (oItemsMasterBL.IsDuplicateItemName() && oItemsMasterBL.IsDuplicateItemCode())
        {
            if (hidIsNewItem.Value == "True")
            {
                oItemsMasterBL.InsertItemsMaster();
                lblErrorMsg.Visible = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Item details added successfully!!!";
            }
            else
            {
                oItemsMasterBL.UpdateItemsMaster();
                lblErrorMsg.Visible = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Item details updated successfully!!!";
            }

        }
    }


    /// <summary>
    /// This method is used to check file size and then check correct file to specified location
    /// </summary>
    private string UploadImageFile(out string asFileName, FileUpload fileUploadItems)
    {
        asFileName = string.Empty;
        if (fileUploadItems.FileName.TrimAll() != string.Empty)
        {
            hidFilePath.Value = fileUploadItems.FileName.ToString();
        }
        if (hidFilePath.Value != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            if (hidIsNewItem.Value == "True")
            {
                if (fileUploadItems.HasFile)
                {
                    if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkName = CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName.ToString());
                        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                        fileUploadItems.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                    {
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                    }
                }
            }
            else
            {
                if (fileUploadItems.HasFile)
                {
                    if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkName = CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName.ToString());
                        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                        fileUploadItems.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                    {
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                    }
                }
            }
            //code for edit mode
            //else
            //{
            //    string sLinkName;
            //    if (fileUploadItems.HasFile)
            //    {
            //        if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
            //        {
            //            sLinkName = CommonUtility.GetFileNameForRenaming(hidFilePath.Value);
            //            string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
            //            fileUploadItems.SaveAs(sLinkPath);
            //            asFileName = sLinkName;
            //        }
            //        else
            //        {
            //            sReturnErrorMsg = S_FILE_SIZE_ERROR;
            //        }
            //    }
            //    else if (hidFilePath.Value != Constants.S_ZERO)
            //    {
            //        sLinkName = hidFilePath.Value;
            //        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
            //        asFileName = sLinkName;
            //    }
            //}
            return sReturnErrorMsg;
        }
        return string.Empty;
    }



    public List<ItemImageDetails> PopulateImage()
    {
        try
        {
            List<ItemImageDetails> lstItemImageDetails = new List<ItemImageDetails>();
            string sFileName = fileUploadItems.PostedFile.FileName;
            string sFileName1 = fileUploadItems1.PostedFile.FileName;
            string sFileName2 = fileUploadItems2.PostedFile.FileName;
            string sFileFormatError1 = CheckIsFileFileUploaded(fileUploadItems, sFileName);
            string sFileFormatError2 = CheckIsFileFileUploaded(fileUploadItems1, sFileName1);
            string sFileFormatError3 = CheckIsFileFileUploaded(fileUploadItems2, sFileName2);
            string sLinkName;

            if (sFileFormatError1 != string.Empty || sFileFormatError2 != string.Empty || sFileFormatError3 != string.Empty)
            {  
                if (sFileFormatError1 != string.Empty)
                    lblError1.Text = sFileFormatError1 + " for Item Image 1.";
                if (sFileFormatError2 != string.Empty)
                    lblError2.Text = sFileFormatError2 + " for Item Image 2.";
                if (sFileFormatError3 != string.Empty)
                    lblError3.Text = sFileFormatError3 + " for Item Image 3.";

                throw new ApplicationException(Constants.S_VALIDATION_SUMMARY_HEADER);
            }
            else
            {
                //for 1st file control
                if (sFileName != string.Empty)
                {
                    if (sFileName != sFileName1 && sFileName != sFileName2)
                    {
                        if (fileUploadItems.HasFile)
                        {
                            string sFileUploadErr = UploadImageFile(out sLinkName, fileUploadItems);
                            if (string.IsNullOrEmpty(sFileUploadErr))
                            {
                                ItemImageDetails lstItemImageDetails1 = new ItemImageDetails();
                                lstItemImageDetails1.ControlId = 1;
                                lstItemImageDetails1.ImageUrl = sLinkName;
                                lstItemImageDetails.Add(lstItemImageDetails1);
                            }
                            else
                            {
                                throw new ApplicationException(sFileUploadErr);
                            }
                        }
                    }
                    else
                    {
                        throw new UploadFileExceptions("File should not be Duplicate.");
                    }
                }
                //for 2nd file control
                if (sFileName1 != string.Empty)
                {
                    if (sFileName1 != sFileName && sFileName1 != sFileName2)
                    {
                        if (fileUploadItems1.HasFile)
                        {
                            string sFileUploadErr = UploadImageFile(out sLinkName, fileUploadItems1);
                            if (string.IsNullOrEmpty(sFileUploadErr))
                            {
                                ItemImageDetails lstItemImageDetails1 = new ItemImageDetails();
                                lstItemImageDetails1.ControlId = 2;
                                lstItemImageDetails1.ImageUrl = sLinkName;
                                lstItemImageDetails.Add(lstItemImageDetails1);
                            }
                            else
                            {
                                throw new ApplicationException(sFileUploadErr);
                            }
                        }
                    }
                    else
                    {
                        throw new UploadFileExceptions("File should not be Duplicate.");
                    }
                }
                //for 3rd file control.
                if (sFileName2 != string.Empty)
                {
                    if (fileUploadItems2.HasFile)
                    {
                        if (fileUploadItems2 != fileUploadItems && fileUploadItems2 != fileUploadItems1)
                        {
                            string sFileUploadErr = UploadImageFile(out sLinkName, fileUploadItems2);
                            if (string.IsNullOrEmpty(sFileUploadErr))
                            {
                                ItemImageDetails lstItemImageDetails1 = new ItemImageDetails();
                                lstItemImageDetails1.ControlId = 3;
                                lstItemImageDetails1.ImageUrl = sLinkName;
                                lstItemImageDetails.Add(lstItemImageDetails1);
                            }
                            else
                            {
                                throw new ApplicationException(sFileUploadErr);
                            }
                        }
                        else
                        {
                            throw new UploadFileExceptions("File should not be Duplicate.");
                        }
                    }
                }
            }

            return lstItemImageDetails;
        }
        catch (UploadFileExceptions ex)
        {
            throw new UploadFileExceptions(ex.Message);
        }
    }
    #endregion " Private Methods "

    /// <summary>
    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFileFileUploaded(FileUpload oFileUpload, string asFileName)
    {
        asFileName = string.Empty;
        if (oFileUpload.FileName != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkName = CommonUtility.GetFileNameForRenaming(oFileUpload.FileName.ToString());
            if (oFileUpload.HasFile)
            {
                string sFileName = oFileUpload.PostedFile.FileName;
                string sFileExtention = System.IO.Path.GetExtension(sFileName);
                string sFileMimeType = oFileUpload.PostedFile.ContentType;
                int iFileLengthinKb = oFileUpload.PostedFile.ContentLength / I_FILE_SIZE_LIMIT;

                string[] matchExtension = { ".jpg", ".png", ".jpeg", ".bmp"};
                string[] matchMimeType = { "image/jpg", "image/png", "image/jpeg", "image/bmp" };

                if (matchExtension.Contains(sFileExtention.ToLower()) && matchMimeType.Contains(sFileMimeType.ToLower()))
                {
                    if (oFileUpload.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                        oFileUpload.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                }
                else
                    sReturnErrorMsg = "File type should be between .bmp .jpg, .jpeg and .png.";
            }
            return sReturnErrorMsg;
        }
        //if (asFileName == string.Empty)
        //    asFileName = hidEventImage.Value;
        return string.Empty;
    }

    protected void btnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        ItemsMasterBL oItemMasterBL = new ItemsMasterBL();
        int iItemId = Convert.ToInt32(hidItemID.Value);
        oItemMasterBL.DeleteFileDetails(iItemId, 1);
        btnView.Visible = false;
        btnDelete.Visible = false;
        base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
    }
    protected void btnDelete1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {

        ItemsMasterBL oItemMasterBL = new ItemsMasterBL();
        int iItemId = Convert.ToInt32(hidItemID.Value);
        oItemMasterBL.DeleteFileDetails(iItemId, 2);
        btnView1.Visible = false;
        btnDelete1.Visible = false;
        base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
    }
    protected void btnDelete2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {

        ItemsMasterBL oItemMasterBL = new ItemsMasterBL();
        int iItemId = Convert.ToInt32(hidItemID.Value);
        oItemMasterBL.DeleteFileDetails(iItemId, 3);
        btnView2.Visible = false;
        btnDelete2.Visible = false;
        base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
    }


    protected void ddlUOM_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetValuesForSelectedUOM();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void AddLog(string asMessage, bool abIsStartingMessage = false)
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        if (ConfigurationManager.AppSettings["LogFilePath"] != null && ConfigurationManager.AppSettings["LogFilePath"].ToString() != string.Empty)
        {
            string sPath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            var sbContent = new StringBuilder();

            if (abIsStartingMessage)
                sbContent.AppendFormat("{0}{0}", Environment.NewLine, Environment.NewLine);

            sbContent.AppendFormat("School Id    : {0}{1}", iSchoolId, Environment.NewLine);
            sbContent.AppendFormat("DateTime    : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
            sbContent.AppendFormat("School Id   : {0}{1}", iSchoolId, Environment.NewLine);
            sbContent.AppendFormat("Message : {0}{1}", asMessage, Environment.NewLine);

            var swFile = new StreamWriter(sPath + "WebsiteLog.log", true);
            swFile.WriteLine("\n" + sbContent);
            swFile.Flush();
            swFile.Close();
        }
    }
    
}
