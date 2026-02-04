// File Name  : TeacherPhotoUI.aspx.cs
// Created By : DEEPAK
// Created Date : 18/6/2010
//Class Description : This class is used to upload multiple Teacher photos.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Linq;
using BusinessLogic.Exceptions;
using Utility;
using System.Web;
using PhotoUploadEntities;

public partial class TeacherPhotoUI : SchoolBase
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
    /// This event is used to fill teacher list view.
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
                SetDefaultValues();
                FillTeacherListview();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to upload the teacher photo and save the path of that photo to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        int iRowId = 0;
        int iUser_Id;
        string sFileName;
        try
        {
            oArrlstDelete = new ArrayList();
            oArrlstSave = new ArrayList();

            Collection<SchoolWiseTeacherMasterBL> oTeachers = new Collection<SchoolWiseTeacherMasterBL>();
            SchoolWiseTeacherMasterBL moSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();

            foreach (ListViewDataItem oListViewDataItem in lstvwTeacherPhoto.Items)
            {
                SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
                iRowId = Convert.ToInt32(oListViewDataItem.DisplayIndex);
                FileUpload oFileUpload = oListViewDataItem.FindControl("FileUploadLogo") as FileUpload;
				HiddenField oHidPhotoCaptureStatus = oListViewDataItem.FindControl("hidPhotoCapturedStatus") as HiddenField;
				iUser_Id = Convert.ToInt32(lstvwTeacherPhoto.DataKeys[iRowId]["User_Id"]);
				if (oFileUpload.HasFile && oHidPhotoCaptureStatus.Value == "N")
                {
                    hidFilePath.Value = lstvwTeacherPhoto.DataKeys[iRowId]["Photo_file_Path"].ToString().Trim();
                    sFileName = SaveFileOnServer(oFileUpload);
                    Byte[] ImageBinaryData =base.GetByteArrayFromFileField(oFileUpload);
                    oSchoolWiseTeacherMasterBL.UserId = iUser_Id;
                    oSchoolWiseTeacherMasterBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
                    oSchoolWiseTeacherMasterBL.BinaryPhotoImage = ImageBinaryData;
                    oSchoolWiseTeacherMasterBL.SchoolId = miSchoolId;
                    oTeachers.Add(oSchoolWiseTeacherMasterBL);
                }
				else if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
				{
					List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
					var oImage = lstImageData.Where(lst => lst.UserID == iUser_Id).LastOrDefault();
					if (!oImage.IsNull())
					{
						oSchoolWiseTeacherMasterBL.UserId = oImage.UserID;
						oSchoolWiseTeacherMasterBL.PhotoFilePath = string.Empty;
						oSchoolWiseTeacherMasterBL.BinaryPhotoImage = oImage.ImagesData;
						oSchoolWiseTeacherMasterBL.SchoolId = miSchoolId;
						oTeachers.Add(oSchoolWiseTeacherMasterBL);
					}
				}
                else
                    oSchoolWiseTeacherMasterBL.BinaryPhotoImage = null;
            }

            for (int iCount = 0; iCount < oArrlstDelete.Count; iCount++)
            {
                if (File.Exists(oArrlstDelete[iCount].ToString()))
                    File.Delete(oArrlstDelete[iCount].ToString());
            }
            moSchoolWiseTeacherMasterBL.UploadTeacherstPhoto(oTeachers);

            DataPager oDataPager = lstvwTeacherPhoto.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager.Visible)
            {
                DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
                int iPageCount;
                iPageCount = (Convert.ToInt32(ddlCount.SelectedIndex) * Constants.I_GRID_PAGE_COUNT);
                DtPgCount.SetPageProperties(iPageCount, Constants.I_GRID_PAGE_COUNT, false);
            }
			lblUpdateSucess.Text="Photo uploaded successfully!!!";
            FillTeacherListview();
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
        }
        catch (ApplicationException ex)
        {
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Text = ex.Message + " at row number " + (iRowId + 1) + ".";
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
    /// This event is used to fill teacher list view according to searching creteria.
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
            FillTeacherListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to clear the session and close pop up.
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
			 Response.Write(string.Format("<Script language='Javascript'>window.close();window.opener.focus(); </Script>"));
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
    protected void lstvwTeacherPhoto_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;
				ImageButton imageWebcam = (ImageButton)oCurrentItem.FindControl("ibtnPhoto");
				int iUserId = lstvwTeacherPhoto.DataKeys[oCurrentItem.DisplayIndex]["User_Id"].ToInt();
				int iRowNo = e.Item.DisplayIndex;
				string sQueryString = "UserId=" + iUserId + "&RowNo=" + iRowNo;
				imageWebcam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
                Image oImg = (Image)e.Item.FindControl("imgPhoto");
                if (oImg != null)
                {
                    //If photo is uploaded
					if (oDataRowView["BinaryPhotoImage"]==DBNull.Value)
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTeacherPhoto);
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
    protected void lstvwTeacherPhoto_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTeacherPhoto.Items.Count > 0)
            {
                FillPageNoCombo(lstvwTeacherPhoto, DtPgCount);
                btnUpload.Visible = true;
                DataPager oDataPager = lstvwTeacherPhoto.FindControl("DtPgDropDown") as DataPager;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                hidPageNo.Value = iCurrentPage.ToString();
            }
            else
            {
                DtPgCount.Visible = false;
                btnUpload.Visible = false;
            }
            hidCount.Value = lstvwTeacherPhoto.Items.Count.ToString();
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
        if (QueryString["TeacherName"] != null)
            txtTeacherName.Text = QueryString["TeacherName"];
    }

    /// <summary>
    /// This method is used to fill teacher listview.
    /// </summary>
    private void FillTeacherListview()
    {
        lstvwTeacherPhoto.DataSourceID = lstvwTeacherPhotoDsObj.ID;
        lstvwTeacherPhoto.DataBind();
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetDefaultValues()
    {
        chkTeacherWithoutPhoto.Checked = true;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        txtTeacherName.Focus();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnClose, btnUpload, btnSearch });
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
            sReturnErrorMsg = "Size of photo file is too large";
            bIsValid = false;
        }
        oFile = null;
        return sReturnErrorMsg;
    }

    #endregion
	
}