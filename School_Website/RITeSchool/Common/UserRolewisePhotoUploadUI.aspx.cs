// Class Name       :- UserRolewisePhotoUploadUI
// Purpose          :- This class is used to manage UserRolewisePhotoUpload details.
// Date Of creation :- 5/11/2011
// Author Name      :- Vinod

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Linq;
using PhotoUploadEntities;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Web;
using SchoolEntities;
using PayrollEntities;
using System.Threading;
public partial class UserRolewisePhotoUploadUI : ExportDataTable
{
    #region Constants

    const int I_FILE_SIZE_LIMIT = 1048576;//nearly 1 mb
    const int I_HEIGHT_LIMIT = 151;
    const int I_WIDTH_LIMIT = 112;
    const int I_PAGE_SIZE = 20;

    #endregion

    #region Data Members

    ArrayList oArrlstDelete=null;
    ArrayList oArrlstSave = null;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill user list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetVisibility(false);
                FillUserRoleCombo();
                SetJavaScriptAttributes();
                trNoRecordMsg.Visible = false;
                FillUserListview();
            }
            valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            cmbUserRole.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to listview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserPhotoDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				ImageButton imageWebcam = (ImageButton)oCurrentItem.FindControl("ibtnPhoto");
                CheckBox RemovePhoto = (CheckBox)oCurrentItem.FindControl("chkremovephoto");
				int iUserId = lstvwUserPhotoDetails.DataKeys[oCurrentItem.DisplayIndex]["UserId"].ToInt();
				int iRowNo=e.Item.DisplayIndex;
				string sQueryString = "UserId=" + iUserId + "&RowNo=" + iRowNo;
				imageWebcam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
                Image oImg = (Image)e.Item.FindControl("imgPhoto");
                if (oImg != null)
                {
                    //If photo is uploaded
                    if (((PhotoUploadEntities.UserRolewisePhotoDetails)(oCurrentItem.DataItem)).BinaryPhotoImage.IsNull())
                    {
                        oImg.ImageUrl = Constants.S_UPLOAD_IMAGE_STATUS_BLANK_PHOTO;
                        RemovePhoto.Visible = false;
                    }
                    else
                    {
                        oImg.ImageUrl = Constants.S_UPLOAD_IMAGE_STATUS_TRUE;
                        RemovePhoto.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to upload the user photo as per the user role selected `and save the path of that photo to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            oArrlstDelete = new ArrayList();
            oArrlstSave = new ArrayList();
            
            UserRolewisePhotoUploadBL moUserRolewisePhotoUploadBL = new UserRolewisePhotoUploadBL();           
            for (int iCount = 0; iCount < oArrlstDelete.Count; iCount++)
            {
                if (File.Exists(oArrlstDelete[iCount].ToString()))
                    File.Delete(oArrlstDelete[iCount].ToString());
            }
            moUserRolewisePhotoUploadBL.UploadUsersPhoto(GetCollectionOfUserPhotoDetailsToUpload());

            DataPager oDataPager = lstvwUserPhotoDetails.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager.Visible)
            {
                DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
                DtPgCount.SetPageProperties((Convert.ToInt32(ddlCount.SelectedIndex) * I_PAGE_SIZE),I_PAGE_SIZE, false);
            }
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "User details updated successfully!!!";
            FillUserListview();
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
        }
        catch (ApplicationException ex)
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Text = ex.Message ;
        }
        catch (Exception ex)
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill user list view according to searching creteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            hidStandardId.Value = cmbStandard.SelectedValue;
            hidDivisionId.Value = cmbDivision.SelectedValue;
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            FillUserListview();            
        }
        catch (Exception ex)
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
     
    /// <summary>
    /// This event is used to set page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserPhotoDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUserPhotoDetails.Items.Count > 0)
            {
                SetConfirmationMessage();
                SetVisibility(true);
                ControlUtility.FillListViewPagerFooter(lstvwUserPhotoDetails, DtPgCount);               
                btnUpload.Visible = true;
                btnExport.Visible=true;
                DataPager oDataPager = lstvwUserPhotoDetails.FindControl("DtPgDropDown") as DataPager;
				
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                hidPageNo.Value = iCurrentPage.ToString();
            }
            else
            {
                SetVisibility(false);
                DtPgCount.Visible = false;
                btnUpload.Visible = false;
                btnExport.Visible = false;
            }
            hidCount.Value = lstvwUserPhotoDetails.Items.Count.ToString(); 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUserPhotoDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill standard combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            cmbDivision.Items.Clear();
            cmbStandard.Items.Clear();
            if (cmbUserRole.SelectedValue != Constants.I_ZERO.ToString() || txtUserName.Text.Trim() != string.Empty)
            {
                DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
                if (Convert.ToInt32(cmbUserRole.SelectedValue) == Constants.I_THREE)
                {
                    trStd.Visible = true;
                    trDiv.Visible = true;
                    FillStandardCombo();
                }
                else
                {
                    trStd.Visible = false;
                    trDiv.Visible = false;
                }
                FillUserListview();
            }
            else
            {
                trStd.Visible = false;
                trDiv.Visible = false;
                FillUserListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division's combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            cmbDivision.Visible = true;
            FillDivisionCombobox(iStandardId);
            if (cmbStandard.SelectedIndex != 0)
            {               
                hidStandardId.Value = cmbStandard.SelectedValue;
                hidDivisionId.Value = cmbDivision.SelectedValue;
            }
            else
            {
                hidStandardId.Value = Constants.I_ZERO.ToString();
                hidDivisionId.Value = Constants.I_ZERO.ToString();

                cmbDivision.Items.Add(new ListItem(Constants.S_SELECT_ALL, Constants.I_ZERO.ToString()));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division's combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            hidDivisionId.Value = cmbDivision.SelectedValue; 
            FillUserListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to create report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string sPhotostatus;
            string sUserName = txtUserName.Text;
            int iStandardId = 0, iDivisionId = 0;
            if (cmbStandard.Visible == true)
                iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            if (cmbDivision.Visible == true)
                iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
            bool bIsPhotoPresent;
            if (chkUserWithPhoto.Checked == true)
                bIsPhotoPresent = true;
            else
                bIsPhotoPresent = false;
            int iUserRoleId = Convert.ToInt32(cmbUserRole.SelectedValue);
            UserRolewisePhotoUploadBL oUserRolewisePhotoUploadBL = new UserRolewisePhotoUploadBL();
            List<UserRolewisePhotoDetails> lstUserRolewisePhotoDetails = oUserRolewisePhotoUploadBL.GetUserDetailsForPhotoUplaod(miSchoolId, miAcademicYearId, iUserRoleId, sUserName, bIsPhotoPresent, 200000, 0, iStandardId, iDivisionId);

            DataTable dtPhotoDetails = new DataTable();
            dtPhotoDetails.Columns.Add("Sr No", typeof(int));
            dtPhotoDetails.Columns.Add("Name", typeof(string));
            dtPhotoDetails.Columns.Add("Class", typeof(string));
            dtPhotoDetails.Columns.Add("User Role", typeof(string));
            dtPhotoDetails.Columns.Add("Photo Status", typeof(string));

            foreach (UserRolewisePhotoDetails aoUserRolewisePhotoDetails in lstUserRolewisePhotoDetails)
            {
                sPhotostatus = (aoUserRolewisePhotoDetails.BinaryPhotoImage != null ? "Yes" : "No");
                dtPhotoDetails.Rows.Add(aoUserRolewisePhotoDetails.RowNo, aoUserRolewisePhotoDetails.UserName, aoUserRolewisePhotoDetails.ClassName, aoUserRolewisePhotoDetails.UserRoleName, sPhotostatus);
            }

            ExportToExcel("UserPhotoStatus.xls", dtPhotoDetails);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    #endregion

    #region private Methods

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwUserPhotoDetails.FindControl("DtPgDropDown") as DataPager;
        DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCount.Attributes.Add("onchange", "if(!MessageAboutUpload('" + ddlCount.ClientID + "')){return false;}");
    }

    /// <summary>
    /// This method fills combobox with standards.
    /// </summary>   
    private void FillUserRoleCombo()
    {
        UserRolewisePhotoUploadBL oUserRolewisePhotoUploadBL = new UserRolewisePhotoUploadBL(miSchoolId, miAcademicYearId);
        DataTable oDtUserRoleCollection = oUserRolewisePhotoUploadBL.GetUserRoleDetail();

        if (!Settings.EnableTransportModule)
        {
            DataRow[] oDataRows = oDtUserRoleCollection.Select("UserRoleId=" + Constants.UserRoles.TransportStaff.ToInt());
            if (oDataRows.Length > 0)
            {
                oDataRows[0].Delete();
                oDtUserRoleCollection.AcceptChanges();
            }
        }

        ControlUtility.FillDropDownList(oDtUserRoleCollection, ref cmbUserRole,
                                        "UserRoleId", "UserRoleName", Constants.S_SELECT_ALL);
        cmbUserRole.SelectedValue = Constants.I_ONE.ToString();
    }

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
        YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT_ALL);

        //Add item into division combobox.        
        cmbDivision.Items.Add(new ListItem(Constants.S_SELECT_ALL, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox(int aiStandardId)
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
    }

    /// <summary>
    /// This method is used to fill user listview.
    /// </summary>
    private void FillUserListview()
    {
        if (cmbStandard.SelectedIndex == 0)
        {
            hidStandardId.Value = Constants.I_ZERO.ToString();
            hidDivisionId.Value = Constants.I_ZERO.ToString();
        }
        lstvwUserPhotoDetails.DataSourceID = lstvwDsObj.ID;
        lstvwUserPhotoDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set visibility according to action.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetVisibility(bool abAction)
    {
        btnUpload.Visible = abAction;
        btnExport.Visible = abAction;
        trNoRecordMsg.Visible = !abAction;
        trPhotoPager.Visible = abAction;
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetVisibility()
    {
        btnUpload.Visible = false;
        btnExport.Visible = false;
        trPhotoPager.Visible = false;
        trNoRecordMsg.Visible = false;
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
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload FileUploadPhoto , int iRowId)
    {
        string asFileName = FileUploadPhoto.FileName;
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
            oArrlstSave.Add(sServerFilePath);
        }
        FileUploadPhoto.SaveAs(sServerFilePath);
        string sErrMessage = ValidateFile(sServerFilePath, iRowId);
        if (sErrMessage.Equals(string.Empty))
        {
            //delete exesting file            
            string sFileToDelete = Server.MapPath(".") + hidFilePath.Value;
            oArrlstDelete.Add(sFileToDelete);
        }
        else
        {
            for (int iCount = 0; iCount < oArrlstSave.Count; iCount++)
                File.Delete(oArrlstSave[iCount].ToString());

            throw new ApplicationException(sErrMessage);
        }
        return sFileName;
    }

    /// <summary>
    ///This method is used to validate size, height and width of uploaded files.
    /// </summary>
    private string ValidateFile(string asServerFilePath,  int iRowId)
    {
        string sReturnErrorMsg = String.Empty;
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
            {
                sReturnErrorMsg = "Height and width of photo file should not exceed " + I_HEIGHT_LIMIT + "px and " + I_WIDTH_LIMIT + "px respectively at row number " + (iRowId + 1) + ".";
                bIsValid = false;
				// this is to clear session image data captured web cam.
				this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
				hidIsPhotoCaptured.Value = "N";
            }
            else
            {
                if (oImg.Height > I_HEIGHT_LIMIT)
                {
                    sReturnErrorMsg = "Height of photo file should not exceed " + I_HEIGHT_LIMIT + "px at row number " + (iRowId + 1) + ".";
                    bIsValid = false;
					// this is to clear session image data captured web cam.
					this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
					hidIsPhotoCaptured.Value = "N";
                }
                if (oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = "Width of photo file should not exceed " + I_WIDTH_LIMIT + "px at row number " + (iRowId + 1) + ".";
                    bIsValid = false;
					// this is to clear session image data captured web cam.
					this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
					hidIsPhotoCaptured.Value = "N";
                }
            }
            oFileStream.Close();
            oImg = null;
        }
        FileInfo oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = "Size of photo file is too large at row number " + (iRowId + 1) + ".";
            bIsValid = false;
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        oFile = null;
        return sReturnErrorMsg;
    }

    /// <summary>
    /// This method is used to get collection of user photo details to upload.
    /// </summary>
    /// <returns></returns>
    private Collection<UserRolewisePhotoUploadBL> GetCollectionOfUserPhotoDetailsToUpload()
    {
        int iRowId = 0;
        string sFileName;

        Collection<UserRolewisePhotoUploadBL> oUserDetails = new Collection<UserRolewisePhotoUploadBL>();
		
        foreach (ListViewDataItem oListViewDataItem in lstvwUserPhotoDetails.Items)
        {
            UserRolewisePhotoUploadBL oUserRolewisePhotoUploadBL = new UserRolewisePhotoUploadBL(miSchoolId, miAcademicYearId);
            UserRolewisePhotoDetails oUserRolewisePhotoDetails = new UserRolewisePhotoDetails();
            iRowId = Convert.ToInt32(oListViewDataItem.DisplayIndex);
            FileUpload oFileUpload = oListViewDataItem.FindControl("FileUploadPhoto") as FileUpload;
           
            CheckBox oRemovePhoto = oListViewDataItem.FindControl("chkremovephoto") as CheckBox;
		   HiddenField oHidPhotoCaptureStatus = oListViewDataItem.FindControl("hidPhotoCapturedStatus") as HiddenField;
			int iUserId= Convert.ToInt32(lstvwUserPhotoDetails.DataKeys[iRowId]["UserId"]);

            if (oRemovePhoto.Checked == true)
            {
                oUserRolewisePhotoDetails.UserId = iUserId;
                oUserRolewisePhotoDetails.UserRoleId = Convert.ToInt32(lstvwUserPhotoDetails.DataKeys[iRowId]["UserRoleId"]);
                if (oRemovePhoto.Checked == true)
                    oUserRolewisePhotoDetails.RemovePhoto = true;
                else
                    oUserRolewisePhotoDetails.RemovePhoto = false;
                oUserRolewisePhotoUploadBL.UserRolewisePhotoDetails = oUserRolewisePhotoDetails;
                oUserDetails.Add(oUserRolewisePhotoUploadBL);
            }
            else if (oFileUpload.HasFile && oHidPhotoCaptureStatus.Value == "N")
            {
                hidFilePath.Value = lstvwUserPhotoDetails.DataKeys[iRowId]["PhotoFilePath"].ToString().Trim();
                sFileName = SaveFileOnServer(oFileUpload, iRowId);
                Byte[] ImageBinaryData = this.GetByteArrayFromFileFieldDetails(oFileUpload);
                oUserRolewisePhotoDetails.UserId = iUserId;
                oUserRolewisePhotoDetails.UserRoleId = Convert.ToInt32(lstvwUserPhotoDetails.DataKeys[iRowId]["UserRoleId"]);
                oUserRolewisePhotoDetails.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
                oUserRolewisePhotoDetails.BinaryPhotoImage = ImageBinaryData;
                oUserRolewisePhotoUploadBL.UserRolewisePhotoDetails = oUserRolewisePhotoDetails;
                oUserDetails.Add(oUserRolewisePhotoUploadBL);
            }
            else if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null)
            {
                List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
                var oImage = lstImageData.Where(lst => lst.UserID == iUserId).LastOrDefault();
                if (!oImage.IsNull())
                {
                    oUserRolewisePhotoDetails.UserId = oImage.UserID;
                    oUserRolewisePhotoDetails.UserRoleId = Convert.ToInt32(lstvwUserPhotoDetails.DataKeys[iRowId]["UserRoleId"]);
                    oUserRolewisePhotoDetails.PhotoFilePath = string.Empty;
                    oUserRolewisePhotoDetails.BinaryPhotoImage = oImage.ImagesData;
                    oUserRolewisePhotoUploadBL.UserRolewisePhotoDetails = oUserRolewisePhotoDetails;
                    oUserDetails.Add(oUserRolewisePhotoUploadBL);
                }
            }
            else
                oUserRolewisePhotoDetails.PhotoFilePath = string.Empty;
        }

        return oUserDetails;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch ,btnUpload, btnExport });       
    }
    #endregion 
}
