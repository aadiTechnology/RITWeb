// File Name  : TransportStaffUI.aspx.cs
// Created By : Deepak
// Date       : 5 July 2010
//Description :This class is used to add, eidt, delete Transport Staff member's details. 

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class TransportStaffUI :SchoolBase
{
    #region "CONSTANTS"

    private const int I_FILE_SIZE_LIMIT = 81920;//nearly 80 kb
    const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";

    const string S_DEFAULT_SORT_EXP = "Name";
    private const string S_DELETE_MESSAGE = "Transport staff deleted successfully!!!";
    
    #endregion

    #region "EVENTS"

    /// <summary>
    /// This event is used to fill Salutation combo,Designation combo, existing Staff Members listView 
    /// and to set the default properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                
                SetDefaultValues();
                FillSalutationComboBox();
                FillDesignationCombobox();
                FillExistingStaffListview();
                SetJavascriptAttributes();
                tblUsername.Visible = false;
                SetQueryString();
            }
            cmbSalutation.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add confirmation message while deleting existing satff member's record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportStaff_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                 Label lblDOB = e.Item.FindControl("lblDOB") as Label;
                 if (lblDOB.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_5)
                     lblDOB.Text = lblDOB.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                 else lblDOB.Text = "-";
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                Image oImg = (Image)e.Item.FindControl("imgPhotoUpload");
                if (oImg != null)
                {
                    //If photo is uploaded
                    if ((((DataCommunicator.TransportStaffDC.TransportStaff)(oCurrentItem.DataItem)).msPhotoFilePath).ToString().Trim().Equals(string.Empty))
                        oImg.ImageUrl = "~/RITeSchool/images/IconGridStudentBlankPh.gif";
                    else
                        oImg.ImageUrl = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property and add sort image for existing staff members listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportStaff_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTransportStaff.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwTransportStaff, DtPgCount);
                if (IsPostBack)
                    AddSortImage();
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
    /// This event is used to edit and delete the existing staff members details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportStaff_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iTransportStaffId = Convert.ToInt32(lstvwTransportStaff.DataKeys[iListIndex]["miTransportStaffId"]);
                int iUserId = Convert.ToInt32(lstvwTransportStaff.DataKeys[iListIndex]["miUserId"]);
                hidUserRoleid.Value = Convert.ToString(Constants.UserRoles.TransportStaff);
                hidTransportStaffID.Value = iTransportStaffId.ToString();
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                    DeleteTransportStaffDetails(iTransportStaffId);
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    FillControlsForStaffUpdate(iTransportStaffId);
                    string S_PAGE = "~/Admin/SupervisorDetailsUI.aspx";
                    string sQuerystring = "&User_Role_Id=" + hidUserRoleid.Value + "&TransportStaffID=" + hidTransportStaffID.Value + "&UserId=" + iUserId;
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
                    string sRedirectUrl = S_PAGE + "?" + sEncrypt;
                    MasterPage oMasterPage = (MasterPage)this.Master;
                    oMasterPage.RedirectToNextPage(sRedirectUrl);
                }                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the listview of staff members by Name,Designation and Mobile No..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lstvwTransportStaff_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise staff list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTransportStaff);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save TransportStaff details as well as its configuration details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            SaveStaffDetails();
            if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.TransportStaff));
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
       
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancle saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "PRIVATE METHODS"


    /// <summary>
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload FileUploadLogo)
    {
        const int I_HEIGHT_LIMIT = 151;
        const int I_WIDTH_LIMIT = 112;
        string sFileName = FileUploadLogo.FileName;
        string asFileName = sFileName;
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;
        string sServerFilePath = sFolderName + sFileName;
        if (File.Exists(sServerFilePath))
            asFileName =CommonUtility.GetFileNameForRenaming(sFileName);
        sServerFilePath = sFolderName + asFileName;
        UploadPhoto.SaveAs(sServerFilePath);
        string sErrorMsg = ValidateFile(sServerFilePath, I_HEIGHT_LIMIT, I_WIDTH_LIMIT, sFileName);
        if (sErrorMsg.Equals(string.Empty))
        {
            //delete exesting logo
            string sFileToDelete = Server.MapPath(".") + hidFilePath.Value;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
            lblErrorMsg.Text = sErrorMsg;
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrorMsg);
        }
        return sFileName;
    }

    /// <summary>
    /// This method is used to convert image in binary format.
    /// </summary>
    /// <param name="FileField"></param>
    /// <returns></returns>
    private Byte[] GetByteArrayFromFileFieldDetails(FileUpload FileField)
    {
        //' Returns a byte array from the passed 
        //' file field controls file
        int intFileLength;
        Byte[] bytedata = new byte[0];
        System.IO.Stream oStream;
        if (FileField.PostedFile != null && FileField.PostedFile.ContentLength != 0)
        {
            intFileLength = FileField.PostedFile.ContentLength;
            bytedata = new byte[intFileLength];
            oStream = FileField.PostedFile.InputStream;
            oStream.Read(bytedata, 0, intFileLength);
        }
        return bytedata;
    }

    /// <summary>
    /// This method is used to validate uploaded file.
    /// </summary>
    /// <param name="asServerFilePath"></param>
    /// <param name="aiHeight"></param>
    /// <param name="aiWidth"></param>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string ValidateFile(string asServerFilePath, int aiHeight, int aiWidth, string asFileName)
    {
        string sReturnErrorMsg = string.Empty;
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > aiHeight && oImg.Width > aiWidth)
            {
                sReturnErrorMsg = "Height and Width of photo file should not exceed " + aiHeight + "px and " + aiWidth + "px respectively.";
                bIsValid = false;
            }
            else
            {
                if (oImg.Height > aiHeight)
                {
                    sReturnErrorMsg = "Height of photo file should not exceed " + aiHeight + "px.";
                    bIsValid = false;
                }
                if (oImg.Width > aiWidth)
                {
                    sReturnErrorMsg = "Width of photo file should not exceed " + aiWidth + "px.";
                    bIsValid = false;
                }
            }
            oFileStream.Close();
            oImg = null;
        }
        FileInfo oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = "Size of photo file is too large.";
            bIsValid = false;
        }
        oFile = null;
        return sReturnErrorMsg;
    }
        
    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnBack});
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted));
    }
    /// <summary>
    /// This method sets the QueryString.
    /// </summary>

    private void SetQueryString()
    {
        string sQueryString = "&User_Role_Id=" + Convert.ToString(Constants.UserRoles.TransportStaff) + "&TransportStaffID=" + hidTransportStaffID.Value + "&UserId=" + Constants.S_ZERO;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        hidQueryString.Value = sEncrypt;
        btnAdd.Attributes.Add("onclick", "window.open('../Admin/SupervisorDetailsUI.aspx?" + sEncrypt
                                   + "' , '_self'); return false;");
    }

    /// <summary>
    /// This method is used set datasource  to existing staff listView
    /// </summary>
    private void FillExistingStaffListview()
    {
        lstvwTransportStaff.DataSourceID = ObjDSTransportStaff.ID;
        lstvwTransportStaff.DataBind();
    }

    /// <summary>
    /// This method is used to fill salutation combo.
    /// </summary>
    private void FillSalutationComboBox()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    /// <summary>
    /// This method is used to fill designation combo.
    /// </summary>
    private void FillDesignationCombobox()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillDesignationCombobox(ref cmbDesignation, Constants.UserRoles.TransportStaff);
    }

    /// <summary>
    /// This method is used to create transport staff object.
    /// </summary>
    /// <returns>TransportStaffBL</returns>
    private TransportStaffBL CreateTransportStaffObject()
    {
        TransportStaffBL oTransportStaffBL = new TransportStaffBL();
        oTransportStaffBL.SchoolId = miSchoolId;
        oTransportStaffBL.AcademicYearId = miAcademicYearId;
        oTransportStaffBL.SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue);
        oTransportStaffBL.FirstName = txtFirstName.Text.ToTitleCase();
        oTransportStaffBL.MiddleName =txtMiddleName.Text.ToTitleCase();
        oTransportStaffBL.LastName = txtLastName.Text.ToTitleCase();
        oTransportStaffBL.MobileNo = txtMobileNo.Text.Trim();
        oTransportStaffBL.Address = txtAddress.Text.Trim();
        oTransportStaffBL.EmergencyContact = txtEmergencyNo.Text.Trim();
        oTransportStaffBL.DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue);
        oTransportStaffBL.InsertedById = miUserId;
        oTransportStaffBL.DOB = txtCalDobPopup.Text.ToDateTime();
        if (hidMode.Value == Constants.S_EDIT_MODE)
        oTransportStaffBL.UserId =Convert.ToInt32(hidUserId.Value);
        return oTransportStaffBL;
    }

    /// <summary>
    /// This method is used to clear form fields.
    /// </summary>
    private void ClearFields()
    {
        cmbSalutation.SelectedValue = "1";
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtEmergencyNo.Text = string.Empty;
        cmbDesignation.SelectedValue = "0";
        cmbSalutation.Focus();
        hidMode.Value = Constants.S_NEW_MODE;
        lblErrorMsg.Text = string.Empty;
        lblUpdateSucess.Text = string.Empty;
        imgPhoto.ImageUrl = S_DEFAULT_PHOTO;
        txtCalDobPopup.Text = string.Empty;
    }
  

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwTransportStaff.SortDirection.ToString() == "Ascending" || lstvwTransportStaff.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwTransportStaff.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwTransportStaff.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwTransportStaff.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to delete exisiting satff member's details as well as it checks dependancy of satff member with vehicle.
    /// And also checks if at least one staff member's details has been configured or not.
    /// </summary>
    /// <param name="aiTransportStaffId"></param>
    /// <param name="aiSchoolID"></param>
    /// <returns></returns>
    private void DeleteTransportStaffDetails(int aiTransportStaffId)
    {
        TransportStaffBL oTransportStaffBL = new TransportStaffBL();
        int iRowCount = 0;
        DataTable oDTDeleteMsg = oTransportStaffBL.DeleteStaff(aiTransportStaffId, miSchoolId,miAcademicYearId, out iRowCount);
        if (oDTDeleteMsg != null && oDTDeleteMsg.Rows.Count > 0 && Convert.ToString(oDTDeleteMsg.Rows[0]["msg"]) != string.Empty)
        {
            AddSortImage();
            lblErrorMsg.Text = "Transport staff " + Convert.ToString(oDTDeleteMsg.Rows[0]["msg"]) + " can not be deleted since associated with vehicle.";
        }
        else
        {            
            if (iRowCount == 0)
                DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.TransportStaff));
            FillExistingStaffListview();
            ClearFields();
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = S_DELETE_MESSAGE;
        }
    }

    /// <summary>
    /// This method is used to set controls to update staff details.
    /// </summary>
    private void FillControlsForStaffUpdate(int aiTransportStaffId)
    {
        lblUpdateSucess.Text = string.Empty;
        TransportStaffBL oTransportStaffBL = new TransportStaffBL(aiTransportStaffId, miSchoolId,miAcademicYearId);
        cmbSalutation.SelectedValue = oTransportStaffBL.SalutationId.ToString();
        txtFirstName.Text = oTransportStaffBL.FirstName.ToString();
        txtMiddleName.Text = oTransportStaffBL.MiddleName.ToString();
        txtLastName.Text = oTransportStaffBL.LastName.ToString();
        if (oTransportStaffBL.Address != null)
                txtAddress.Text = oTransportStaffBL.Address.ToString();
        else
            txtAddress.Text = string.Empty;
        if (oTransportStaffBL.DOB.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
            txtCalDobPopup.Text = oTransportStaffBL.DOB.ToString(Constants.S_DATE_FORMAT);
        else
            txtCalDobPopup.Text = string.Empty;
        txtMobileNo.Text = Convert.ToString(oTransportStaffBL.MobileNo);
        txtEmergencyNo.Text = oTransportStaffBL.EmergencyContact;

        var oDesignation = cmbDesignation.Items.FindByValue(oTransportStaffBL.DesignationId.ToString());
        if (oDesignation != null)
            oDesignation.Selected = true;
        
        hidTransportStaffID.Value = oTransportStaffBL.TransportStaffId.ToString();
        hidMode.Value = Constants.S_EDIT_MODE;
        hidFilePath.Value = oTransportStaffBL.PhotoFilePath;
        string sFile = ".." + hidFilePath.Value;
        string sServerFilePath = Server.MapPath("..") + hidFilePath.Value;
        hidUserId.Value = oTransportStaffBL.UserId.ToString();
        if (File.Exists(sServerFilePath))
            imgPhoto.ImageUrl = sFile;
        else
            imgPhoto.ImageUrl = S_DEFAULT_PHOTO;

        AddSortImage();
    }
    /// <summary>
    /// This method is used to save Staff details.
    /// </summary>
    private void SaveStaffDetails()
    {
        TransportStaffBL oTransportStaffBL = CreateTransportStaffObject();
        string sFileName;
        Byte[] ImageBinaryData = { };
        if (UploadPhoto.HasFile)
        {
            sFileName = SaveFileOnServer(UploadPhoto);
            ImageBinaryData = this.GetByteArrayFromFileFieldDetails(UploadPhoto);
            oTransportStaffBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            oTransportStaffBL.BinaryPhotoImage = ImageBinaryData;
        }
        else
            oTransportStaffBL.PhotoFilePath = string.Empty;

        //if (hidMode.Value != Constants.S_EDIT_MODE)
        //    oTransportStaffBL.Insert();
        //else
        //{
        //    oTransportStaffBL.TransportStaffId = Convert.ToInt32(hidTransportStaffID.Value);
        //    oTransportStaffBL.EmergencyContact = txtEmergencyNo.Text.Trim();

        //    oTransportStaffBL.UpdateStaff();
        //}
        FillExistingStaffListview();
        ClearFields();
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = "Staff Details Saved Successfully!!";
        hidMode.Value = Constants.S_NEW_MODE;
    }

    /// <summary>
    /// This method is used to set default values for sorting and error message heading.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        HtmlTableRow oHtmlTableHeaderRow = lstvwTransportStaff.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    #endregion
}
