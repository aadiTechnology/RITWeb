using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Data;
using BusinessLogic;
using Utility;
using System.IO;
using SchoolEntities;
using System.Data.SqlClient;

public partial class UploadVideoViewUI : SchoolBase
{
    VideoGalleryBL moVideoGalleryBL;

    #region Constants

    private const int I_DELETE_COLUMN_INDEX = 4;
    private const int I_VIEW_VIDEOS = 1;

    private const string S_EDIT_COMMAND = "EDIT_ROW";
    private const string S_DELETE_COMMAND = "DELETE_ROW";
    private const string S_SAVE = "Save";
    private const string S_UPDATE = "Update";

    private const string S_UPDATE_MESSAGE = "Video updated successfully!!!";
    private const string S_SAVE_MESSAGE = "Video saved successfully!!!";
    private const string S_UPDATE_ERROR_MESSAGE = "Failed to update photo comment.";
    private const string S_DELETE_MESSAGE = "Video deleted successfully!!!";
    private const string S_DELETE_ERROR_MESSAGE = "Failed to delete photo.";
    private const string S_EDIT_ERROR_MESSAGE = "There was an error editing photo.";

    #endregion

    #region Event's

    /// <summary>
    /// This event is used for load the data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVideoGalleryBL = new VideoGalleryBL();
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillVideoGallery();
                DisableControls(true);
                SetJavascriptAttributes();             
            }
            var oForm = this.Master.FindControl("form1") as HtmlForm;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete selected photo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sCommand = e.CommandName.ToUpper();

        try
        {
            int iRowIndex = e.CommandArgument.ToInt();
            switch (sCommand)
            {
                case S_EDIT_COMMAND:
                    EditComment(iRowIndex);
                    break;
                case S_DELETE_COMMAND:
                    {
                        DeleteVideos(iRowIndex);

                        string sGalleryName = lblGalleryName.Text;

                        // Check if the Gallery contains any images
                        // If it DOES NOT, then delete it's Zip archive & XML file stored on the server
                        var oImageGalleryBL = new ImageGalleryBL
                        {
                            SchoolId = miSchoolId
                        };
                        int iGalleryImageCount = oImageGalleryBL.GetPhotoCount(sGalleryName);
                        if (iGalleryImageCount == 0)
                        {
                            string sGalleryZipFilePath = Server.MapPath("..") + "\\DOWNLOADS\\" + sGalleryName + ".zip";
                            if (File.Exists(sGalleryZipFilePath))
                                File.Delete(sGalleryZipFilePath);
                            string sGalleryXMLFilePath = Server.MapPath("..") + "\\Gallery\\" + sGalleryName + ".xml";
                            if (File.Exists(sGalleryXMLFilePath))
                                File.Delete(sGalleryXMLFilePath);
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            string sMessage = sCommand == S_EDIT_COMMAND ? S_EDIT_ERROR_MESSAGE : S_DELETE_ERROR_MESSAGE;
            SetMessage(sMessage, true);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add delete button attribute.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            SetRowData(e.Row);
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                var oPhotoDelete = e.Row.Cells[I_DELETE_COLUMN_INDEX].Controls[Constants.I_ZERO] as ImageButton;
                oPhotoDelete.Attributes.Add("onclick", "if(!ConfirmPhotoDelete()) {return false;}");
                var oImg = e.Row.Cells[1].Controls[Constants.I_ZERO] as Image;
                oImg.ImageUrl = "..\\" + oImg.ImageUrl;
                oImg.Height = 120;
                oImg.Width = 160;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save video details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVideoUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            VideoDetails oVideoDetails = new VideoDetails
            {
                SchoolId = miSchoolId,
                VideoId = hidVideoGalleryId.Value.ToInt(),
                VideoDetailsId = hidVideoGallaryDetailsId.Value.ToInt(),
                sVideoComment = txtComment.Text,
                sVideoName = txtName.Text,
                sVideoUrl = txtURL.Text,
                InsertedById = miUserId,
                SubjectId = hidSubjectId.Value.ToInt()
            };

            VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
            oVideoGalleryBL.UpdateVideoDescription(oVideoDetails);
            FillVideoGallery();  
            if(btnVideoUpdate.Text == S_SAVE)
                SetMessage(S_SAVE_MESSAGE, false);
            else
                SetMessage(S_UPDATE_MESSAGE, false);
            CleareFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
            oVideoGalleryBL.UpdateGalleyName(miSchoolId, txtName.Text.TrimAll(), hidVideoGalleryId.Value.ToInt(), miUserId);
            SetMessage("Name is updated successfully !!!", false);
            lblGalleryName.Text = txtName.Text.TrimAll();
        }
        catch(SqlException ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for cleare the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CleareFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used for redirct to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("UploadPhotoUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to decrypt the given query string.
    /// </summary>
    private void ReadQuerystring()
    {
        if (QueryString.Count > 0 && QueryString["VideoGallaryId"] != null && QueryString["VideoGallaryName"] != null)
        {
            hidVideoGalleryId.Value = QueryString["VideoGallaryId"].ToString();
            lblGalleryName.Text = QueryString["VideoGallaryName"];
            txtName.Text = QueryString["VideoGallaryName"];
            hidSubjectId.Value = QueryString["SubjectId"];
            lblSubjectName.Text = QueryString["SubjectName"];

            hidUrlSourceId.Value = QueryString["UrlSourceId"];
            lblUrlSource.Text = QueryString["URLSource"];
        }        
    }

    /// <summary>
    /// This method is used to set row data
    /// </summary>
    private void SetRowData(GridViewRow gridViewRow)
    {
        int iRowIndex = gridViewRow.RowIndex;
        if (iRowIndex >= Constants.I_ZERO)
        {
            string sVideoURL = grdVideos.DataKeys[iRowIndex]["URL"].ToString();
            lblGalleryName.Text = grdVideos.DataKeys[iRowIndex]["VideoName"].ToString();
            int iVideoDetailsId = grdVideos.DataKeys[iRowIndex]["VideoDetailsId"].ToInt();
            string sQueryString = "&VideoDetailsId=" + iVideoDetailsId.ToString();
            string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
            HtmlAnchor aView = (HtmlAnchor)gridViewRow.FindControl("aView");
            LinkButton a1 = (LinkButton)gridViewRow.FindControl("a1");
            if (hidUrlSourceId.Value == "1")
            {
                aView.HRef = sVideoURL;
                aView.Attributes.Add("onclick", "return false");
                a1.Visible = false;
            }
            else 
            {
                   aView.Visible = false;
                   a1.Attributes.Add("onclick", "window.open('PlayVideoPopup.aspx?" + sEncryptedString
                                                           + " ' , '_new' ,'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=550');return false;");
            }
        }
        
    }

    /// <summary>
    /// This method is used to fill photo gallery.
    /// </summary>
    private void FillVideoGallery()
    {
        DataTable oDataTable = ImageGalleryCollectionBL.FetchVideosForGallery(hidVideoGalleryId.Value.ToInt(), miSchoolId, hidSubjectId.Value.ToInt());
        grdVideos.Columns[0].Visible = true;
        grdVideos.DataSource = oDataTable;
        grdVideos.DataBind();
        grdVideos.Columns[0].Visible = false;

        if (Settings.IsAaryanSchool)
            trSubject.Visible = true;
        UrlLink.Visible = true;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {        
        ApplyMouseHoverEffect(new List<Button> { btnVideoUpdate,btnBack});
        btnVideoUpdate.Text = S_SAVE;
        //btnVideoUpdate.Enabled = true;
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumHeader.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnVideoUpdate.Attributes.Add("onclick", "ClearMessage()");
        btnUpdate.Attributes.Add("onclick", "ClearMessage()");
    }

    /// <summary>
    /// This method is used to edit comment.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void EditComment(int iRowIndex)
    {
        const int I_EDIT_COLUMN_INDEX = 1;
        btnVideoUpdate.Text = S_UPDATE;
        txtComment.Text = HttpUtility.HtmlDecode(grdVideos.Rows[iRowIndex].Cells[I_EDIT_COLUMN_INDEX].Text.Trim());
        hidVideoGallaryDetailsId.Value = grdVideos.DataKeys[iRowIndex]["VideoDetailsId"].ToString();
        txtURL.Text = grdVideos.DataKeys[iRowIndex]["URL"].ToString();
        txtName.Text = grdVideos.DataKeys[iRowIndex]["VideoName"].ToString();
        DisableControls(false);
        txtComment.Focus();
    }

    /// <summary>
    /// This method is used to disable controls.
    /// </summary>
    /// <param name="abFlag"></param>
    private void DisableControls(bool abFlag)
    {
        //btnVideoUpdate.Enabled = !abFlag;
    }

    /// <summary>
    /// This method is used to delete photo.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void DeleteVideos(int iRowIndex)
    {
        // Get image ID from grid and delete the image from database.
        int iVideoId = grdVideos.DataKeys[iRowIndex]["VideoId"].ToString().ToInt();
        int iVideoDetailsId = grdVideos.DataKeys[iRowIndex]["VideoDetailsId"].ToString().ToInt();
        VideoDetails oVideoDetails = new VideoDetails()
        {
            SchoolId = miSchoolId,
            VideoId = iVideoId,
            VideoDetailsId = iVideoDetailsId,
            InsertedById = miUserId
        };

        VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
        moVideoGalleryBL.DeleteVideoGalleryDetails(oVideoDetails);

        if (hidVideoGallaryDetailsId.Value.ToInt() == iVideoDetailsId)
            CleareFields();

        DisableControls(true);
        txtComment.Text = string.Empty;        
        FillVideoGallery();

        SetMessage(S_DELETE_MESSAGE, false);
    }

    /// <summary>
    ///	Sets the message to be shown on the page.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="abIsError"></param>
    private void SetMessage(string asMessage, bool abIsError)
    {
        lblErrorMessage.Visible = lblUpateMessage.Visible = false;
        (abIsError ? lblErrorMessage : lblUpateMessage).Text = asMessage;
        (abIsError ? lblErrorMessage : lblUpateMessage).Visible = true;
    }

    /// <summary>
    ///	this method is used to cleare all the fields.
    /// </summary>
    private void CleareFields()
    {
        txtComment.Text = string.Empty;
        txtName.Text = string.Empty;
        txtURL.Text = string.Empty;
        //btnVideoUpdate.Enabled = false;
        btnVideoUpdate.Text = S_SAVE;
        hidVideoGallaryDetailsId.Value = Constants.S_ZERO;
    }

    #endregion
}