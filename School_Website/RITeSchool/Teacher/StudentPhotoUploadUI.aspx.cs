// File Name  : StudentPhotoUploadUI.aspx.cs
// Created By : Milind
// Created Date : 13/8/2009
//Class Description : This class is used to upload multiple students photos.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Linq;
using BusinessLogic.Exceptions;
using StudentEntities;
using Utility;
using PhotoUploadEntities;
using System.Web;

public partial class StudentPhotoUploadUI : SchoolBase
{

    #region Constants

    const int I_FILE_SIZE_LIMIT = 81920;//nearly 80 kb
    const int I_HEIGHT_LIMIT = 151;
    const int I_WIDTH_LIMIT = 112;

    #endregion

    #region Data Members

    ArrayList oArrlstDelete;
    ArrayList oArrlstSave;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill student list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                FillOperators();
                GetPrefixes();
                GetAllRegNoPostfixes();
                ReadQueryString();
                FillStandardCombobox();
                FillDivisionCombobox();
                SetDefaultValues();
                FillStudentsListview();
                SetJavascriptAttributes();
               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division combo according to selected standard and fill student listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            hidDivisionId.Value = "0";
            hidStandardId.Value = cmbStandard.SelectedValue;
            FillDivisionCombobox();            
            FillStudentsListview();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void optMain_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            if (optMain.Checked)
                SetControlsForLikeCriteria();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void optExact_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            if (optExact.Checked)
                SetControlsForExactMatchCriteria();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close the pop up according to user and access rigths of that user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            SetQueryString();
            string sURL = Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + HidBackUrl.Value;
            if (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor)
            {
                PopupMaster oMasterPage = (PopupMaster)this.Master;
                oMasterPage.RedirectToNextPage(sURL);
            }
            else if (moUserRole == Constants.UserRoles.Teacher)
            {
                if (Boolean.Parse(hidUserHasFullAccess.Value))
                {
                    PopupMaster oMasterPage = (PopupMaster)this.Master;
                    oMasterPage.RedirectToNextPage(sURL);
                }
                else
                    Response.Write("<Script language='Javascript'> window.close();window.opener.focus(); </Script>");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
    }

    /// <summary>
    /// This event is used to upload the student photo and save the path of that photo to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        int iRowId = 0;
        int iStudentId;
        string sFileName;
        try
        {
            oArrlstDelete = new ArrayList();
            oArrlstSave = new ArrayList();

            Collection<StudentBL> oStudents = new Collection<StudentBL>();
            StudentBL moStudentBL = new StudentBL();

            foreach (ListViewDataItem oListViewDataItem in lstvwStudentPhoto.Items)
            {
                StudentBL oStudentBL = new StudentBL();
                iRowId = Convert.ToInt32(oListViewDataItem.DisplayIndex);
                FileUpload oFileUpload = oListViewDataItem.FindControl("FileUploadLogo") as FileUpload;
				  iStudentId = Convert.ToInt32(lstvwStudentPhoto.DataKeys[iRowId]["SchoolWise_Student_Id"]);
                  HiddenField oHidPhotoCaptureStatus = oListViewDataItem.FindControl("hidPhotoCapturedStatus") as HiddenField;
                  if (oFileUpload.HasFile && oHidPhotoCaptureStatus.Value == "N")
                {
                    hidFilePath.Value = lstvwStudentPhoto.DataKeys[iRowId]["Photo_file_Path"].ToString().Trim();
                    sFileName = SaveFileOnServer(oFileUpload);
                  
                    oStudentBL.StudentId = iStudentId;
                    oStudentBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
                    oStudentBL.PhotoFilePathInBinary = GetByteArrayFromFileField(oFileUpload);
                    oStudentBL.SchoolId = miSchoolId;
                    oStudents.Add(oStudentBL);
                }
				else if (HttpContext.Current.Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
			    {
					List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
                    var oImage = lstImageData.Where(lst => lst.UserID == iStudentId).LastOrDefault();
                    if (!oImage.IsNull())
                    {
                        oStudentBL.StudentId = oImage.UserID;
                        oStudentBL.PhotoFilePath = string.Empty;
                        oStudentBL.PhotoFilePathInBinary = oImage.ImagesData; ;
                        oStudentBL.SchoolId = miSchoolId;
                        oStudents.Add(oStudentBL);
                    }
               }
			   else
			     oStudentBL.PhotoFilePath = string.Empty;
		 }

            for (int iCount = 0; iCount < oArrlstDelete.Count; iCount++)
            {
                if (File.Exists(oArrlstDelete[iCount].ToString()))
                    File.Delete(oArrlstDelete[iCount].ToString());
            }
            moStudentBL.UploadStudentPhoto(oStudents);

            DataPager oDataPager = lstvwStudentPhoto.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager.Visible)
            {
                DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
                int iPageCount;
                iPageCount = (Convert.ToInt32(ddlCount.SelectedIndex) * Constants.I_GRID_PAGE_COUNT);
                DtPgCount.SetPageProperties(iPageCount, Constants.I_GRID_PAGE_COUNT, false);
            }
            FillStudentsListview();
			lblUpdateSucess.Text ="Photo uploaded successfully!!!";
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value="N";
        }
        catch (ApplicationException ex)
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Text = ex.Message + " at row number " + (iRowId + 1)+".";
        }
        catch (Exception ex)
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student list view according to searching creteria.
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
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillStudentsListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
    }

    /// <summary>
    /// This event is used to fill student list view according for selected class.
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
             DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
             hidDivisionId.Value = cmbDivision.SelectedValue;
             FillStudentsListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set image for grid column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentPhoto_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;
				ImageButton imageWebcam = (ImageButton)oCurrentItem.FindControl("ibtnPhoto");
				int iStudentId = lstvwStudentPhoto.DataKeys[oCurrentItem.DisplayIndex]["SchoolWise_Student_Id"].ToInt();
                int iRowNo = e.Item.DisplayIndex;
                string sQueryString = "UserId=" + iStudentId + "&RowNo=" + iRowNo;
				imageWebcam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
                Image oImg = (Image)e.Item.FindControl("imgPhoto");
                if (oImg != null)
                {
                    //If photo is uploaded
					if (oDataRowView["Photo_file_Path_Image"]==DBNull.Value)
                        oImg.ImageUrl = Constants.S_UPLOAD_IMAGE_STATUS_BLANK_PHOTO;
                    else
                        oImg.ImageUrl = Constants.S_UPLOAD_IMAGE_STATUS_TRUE;
                }
            }
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentPhoto);
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
    protected void lstvwStudentPhoto_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentPhoto.Items.Count > 0)
            {
                FillPageNoCombo(lstvwStudentPhoto, DtPgCount);
                btnUpload.Visible = true;
                DataPager oDataPager = lstvwStudentPhoto.FindControl("DtPgDropDown") as DataPager;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                hidPageNo.Value = iCurrentPage.ToString();
            }
            else
            {
                DtPgCount.Visible = false;
                btnUpload.Visible = false;
            }
            hidCount.Value = lstvwStudentPhoto.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        hidDivisionId.Value = QueryString["DivisionId"];
        hidStandardId.Value = QueryString["StandardId"];        
        hidIsExactMatch.Value = QueryString["abIsExactMatch"];

        if (Convert.ToBoolean(hidIsExactMatch.Value))
        {
            optExact.Checked = true;
            SetControlsForExactMatchCriteria();
            txtReg.Text = QueryString["RegNo"];

            hidOperator.Value = QueryString["asOperator"];
            hidPrefix.Value = QueryString["asPrefix"];

            cmbPrefix.SelectedValue =hidPrefix.Value;
            cmbOperation.SelectedValue =hidOperator.Value;
        }
        else
        {
            optMain.Checked = true;
            SetControlsForLikeCriteria();
            txtName.Text = QueryString["NameOrRegNo"];
        }
    }

    /// <summary>
    /// This function is used to fill Division combobox.
    /// </summary>
    private void FillDivisionCombobox()
    {
        int aiStandardId = Convert.ToInt32(hidStandardId.Value);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
        cmbDivision.SelectedValue = hidDivisionId.Value;
    }

    /// <summary>
    /// This function is used to fill combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
        cmbStandard.SelectedValue = hidStandardId.Value;

    }

    /// <summary>
    /// This method is used to fill student listview.
    /// </summary>
    private void FillStudentsListview()
    {
        lstvwStudentPhoto.DataSourceID = lstvwStudentDSobj.ID;
        lstvwStudentPhoto.DataBind();
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetDefaultValues()
    {
        DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        if (moUserRole != Constants.UserRoles.Admin)
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student).ToString();
        txtName.Focus();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnClose, btnUpload, btnSearch });
        cmbDivision.Attributes.Add("onchange", "SetWaitCursor()");
        btnSearch.Attributes.Add("onclick", "SetWaitCursor()");
        btnUpload.Attributes.Add("onclick", "SetWaitCursor()");
    }

    /// <summary>
    /// This method is used fill the datapager dropdown list in the list view.
    /// Pager control name should be same as defined here.
    /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
    /// Same for drop down list in the pager control as well as label
    /// </summary>
    public static void FillPageNoCombo(ListView oListView, DataPager oPgCntDataPager)
    {
        DataPager oDataPager = oListView.FindControl("DtPgDropDown") as DataPager;
        System.Web.UI.HtmlControls.HtmlTableRow otblDataPager = oListView.FindControl("trDataPager") as System.Web.UI.HtmlControls.HtmlTableRow;
        otblDataPager.Visible = false;
        oPgCntDataPager.Visible = false;
        int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
        int iTotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
        if (iTotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
            iTotalPages += 1;

        if (iTotalPages > 1)
        {
            otblDataPager.Visible = true;
            oPgCntDataPager.Visible = true;
            //Populate the DropDownList if needed
            DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
            ddlCount.Attributes.Add("onchange", "if(!MessageAboutUpload('" + ddlCount.ClientID + "')){return false;}");

            if (ddlCount.Items.Count == 0)
            {
                //Add a list item for each page
                for (int iddlCount = 1; iddlCount <= iTotalPages; iddlCount++)
                    ddlCount.Items.Add(iddlCount.ToString());

                //Set the DDL to the appropriate page value
                ddlCount.Items.FindByValue(iCurrentPage.ToString()).Selected = true;
                Label oLabel = (oDataPager.Controls[0].FindControl("CurrentPageLabel")) as Label;
                oLabel.Font.Bold = true;
                oLabel.Text = "Page " + iCurrentPage + " of " + iTotalPages;
            }
        }
    }

    /// <summary>
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload FileUploadLogo)
    {
        string asFileName = FileUploadLogo.FileName;
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
            oArrlstSave.Add(sServerFilePath);
        }
        FileUploadLogo.SaveAs(sServerFilePath);
        string sErrMessage = ValidateFile(sServerFilePath);
        if (sErrMessage.Equals(string.Empty))
        {
            //delete exesting logo            
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
    private string ValidateFile(string asServerFilePath)
    {
        string sReturnErrorMsg = String.Empty;
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
            {
                sReturnErrorMsg = "Height and width of photo file should not exceed " + I_HEIGHT_LIMIT + "px and " + I_WIDTH_LIMIT + "px respectively";
                bIsValid = false;
            }
            else
            {
                if (oImg.Height > I_HEIGHT_LIMIT)
                {
                    sReturnErrorMsg = "Height of photo file should not exceed " + I_HEIGHT_LIMIT + "px";
                    bIsValid = false;
                }
                if (oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = "Width of photo file should not exceed " + I_WIDTH_LIMIT + "px";
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
    /// This method is used to set querystring according to selected standard and division.
    /// </summary>
    private void SetQueryString()
    {
        StringBuilder sQueryString = new StringBuilder();

        sQueryString.AppendFormat("StandardId={0}", cmbStandard.SelectedValue);
        sQueryString.AppendFormat("&DivisionId={0}", cmbDivision.SelectedValue);
        sQueryString.AppendFormat("&NameOrRegNo={0}", txtName.Text.Trim());
        sQueryString.AppendFormat("&RegNo={0}", txtReg.Text.Trim());
        if (optExact.Checked )
        {
            hidPrefix.Value = cmbPrefix.SelectedValue;
            hidOperator.Value = cmbOperation.SelectedValue;
        }
        else
        {
            hidPrefix.Value = string.Empty;
            hidOperator.Value = string.Empty;
            hidIsExactMatch.Value = false.ToString();
        }
        sQueryString.AppendFormat("&abIsExactMatch={0}", hidIsExactMatch.Value);
        
        sQueryString.AppendFormat("&asOperator={0}", hidOperator.Value);
        sQueryString.AppendFormat("&asPrefix={0}", hidPrefix.Value);

        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
        HidBackUrl.Value = sEncrypt;
    }

    /// <summary>
    /// This method is used to fill the operators in the dropdownlist.
    /// </summary>
    private void FillOperators()
    {
        List<Operator> olstOperators = StudentBL.GetOperators();
        ListSource.FillDropDownList(olstOperators, cmbOperation, "Text", "Value", string.Empty);
    }

    /// <summary>
    /// This method is used to get the list of prefixes.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    private void GetPrefixes()
    {
        int iSchoolId = miSchoolId.ToInt();
        List<string> olstPrefixes = StudentBL.GetPrefixes(iSchoolId, miAcademicYearId);
        cmbPrefix.Items.Add(new ListItem(Constants.S_ALL, Constants.S_ALL));
        if (olstPrefixes.Count > Constants.I_ZERO)
            olstPrefixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    private void GetAllRegNoPostfixes()
    {
        int iSchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();        
        List<string> lstRegNoPostfixes = StudentBL.GetAllRegNoPostfixes(iSchoolId, miAcademicYearId);
        if (lstRegNoPostfixes.Count > Constants.I_ZERO)
            lstRegNoPostfixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    private void SetControlsForLikeCriteria()
    {
        txtName.Text = "";
        txtReg.Text = "";
        txtReg.Enabled = false;
        cmbPrefix.ClearSelection();
        cmbOperation.ClearSelection();
        cmbOperation.Enabled = false;
        cmbPrefix.Enabled = false;
        txtName.Enabled = true;
        txtName.Focus();
        hidIsExactMatch.Value = false.ToString();
    }

    private void SetControlsForExactMatchCriteria()
    {
        txtName.Text = string.Empty;
        txtReg.Text = string.Empty;
        txtName.Enabled = false;
        txtReg.Enabled = true;
        cmbOperation.Enabled = true;
        cmbPrefix.Enabled = true;
        txtReg.Focus();
        hidIsExactMatch.Value = true.ToString();
    }

    #endregion
}