/* File Name :- UploadPhotoUI.aspx.cs
 * Modified By ;- Sachin
 * Purpose:- Code Review.
 * Class Description :- This class is used to show/add/delete/edit/
 *                      display slideshow of photo gallery and video gallery. 
*/

using System;
using System.Text;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.IO.Compression;
using System.Xml;
using BusinessLogic;
using SchoolEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolEntities.Admin;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Linq;
using System.Resources;
using System.Configuration;
public partial class ManageGalleryUI :SchoolBase
{
   
    #region Constants
 
    const string S_VIDEO_ID = "Video_Id";
    const string S_VIDEO_URL = "Video_Url";
    const int I_VIDEO_NAME_COLUMNID = 0;
    const int I_PHOTO_GALLERY_NAME_COLUMN_INDEX = 0;
    const int I_VIDEO_GALLERY_NAME_COLUMN_INDEX = 0;
    const string S_DELETE_ROW = "DELETEROW";
    const string S_EDIT_ROW = "EDITROW";
    const string S_UPDATE_MESSAGE = "Photo gallery updated successfully!!!";
    const string S_CREATE_MESSAGE = "Photo gallery saved successfully!!!";
    const string S_BTN_ADD_TEXT = "Add";
    const string S_BTN_UPDATE_TEXT = "Update";
    const string S_UPDATE_DATE = "Update_Date";
    const string S_ADD_MORE_SUBJECTS = "ADDSUBJECT";


  
    #endregion

    #region Member

    List<string> oPathList = new List<string>();
   
    DataTable moDtImageGallery;
    List<StandardDivisions> mlstStandardDivisions;

    #endregion

    #region Photo Gallery

    #region Photo Gallery Events
    
    /// <summary>
    /// This event is used for following purpose :
    /// 1)To check login role and if it is supervisor with no edit permission then redirect to ShowImageGallery web form.
    /// 2)To expand collapsable panels.
    /// 3)To fill photo galleries and video galleries into respective gridview at page load.
    /// 4)To set sort properties and javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                CheckLoginRoleAndRedirect();               
                FillPhotoGallery();
                FillVideoGallery();
                FillSectionForPhotoGallery();
                FillStandardDivisionLstBox();
                SetSortProperties();
                FillApplicableRoles();
                SetDefaultProperties();
                SetJavaScriptAttributes();
                chkAddMore.Checked = false;                
                FillSubjectCombo();
            }
                
            else
            {               
                DisplySortImage(grdPhotoGallery, hidPhotoGallerySortExpression.Value, hidPhotoGallerySortDirection.Value);               
                DisplySortImage(grdVideoGallery, hidSortExpression.Value, hidSortDirection.Value);

                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add/update photo gallery name and add photos into gallery.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPhotoAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sGalleryName = txtGalleryName.Text.Trim();
            string sOldGalleryName = hidOrgGalleryName.Value;
            bool bIsUpdate = (btnPhotoAdd.Text == S_BTN_UPDATE_TEXT);
            bool bAtleastOneFileSelected = CheckIfAtleastOneFileIsSelected();            

            if (sGalleryName != string.Empty)
            {
                ImageGalleryBL oImgGalleryBL = InitializePhotoGallery();

                StringBuilder sbSectionIds = new StringBuilder();
                string sSectionIds = string.Empty;

                for (int iItemCount = 0; iItemCount < chkSectionList.Items.Count; iItemCount++)
                {
                    if (chkSectionList.Items[iItemCount].Selected)
                    {
                        sbSectionIds = sbSectionIds.Append("," + chkSectionList.Items[iItemCount].Value);

                    }
                }

                if (sbSectionIds.ToString().StartsWith(","))
                    sSectionIds = sbSectionIds.ToString().Substring(1);

                oImgGalleryBL.AssociatedSection = sSectionIds;
                
                // Determing if the Gallery name already exists in the database.
                if (!oImgGalleryBL.IsDuplicatePhotoGalleryName(btnPhotoAdd.Text))
                {
                    // If it's an UPDATE operation
                    if (bIsUpdate)
                    {
                        // If the user has selected atleast one image for upload
                        if (bAtleastOneFileSelected)
                        {
                            AddPhotos(oImgGalleryBL);

                            if (chkAddMore.Checked)
                            {
                                txtGalleryName.Text = sGalleryName;
                                btnPhotoAdd.Text = S_BTN_UPDATE_TEXT;
                                FillClassDetails(oImgGalleryBL);
                            }
                            else
                                ResetPhotoGalleryControls();
                            lblUpdate.Text = S_UPDATE_MESSAGE;
                            trUpdate.Visible = true;
                        }
                        // If no new files are selected for upload
                        else
                        {

                            ImageGalleryBL.UpdateGalleryName(sOldGalleryName, sGalleryName, miUserId, sSectionIds, GetClasses());
                                UpdateGalleryFiles(sOldGalleryName, sGalleryName);
                                if (chkAddMore.Checked)
                                {
                                    txtGalleryName.Text = sGalleryName;
                                    btnPhotoAdd.Text = S_BTN_UPDATE_TEXT;
                                    FillClassDetails(oImgGalleryBL);
                                }
                                else
                                    ResetPhotoGalleryControls();
                                hidOrgGalleryName.Value = sGalleryName;
                                trDuplicatePhotoGallery.Visible = false;
                                lblUpdate.Text = S_UPDATE_MESSAGE;
                                trUpdate.Visible = true;
                                ClearErrorLabels();
                                lblFileMdtNotice.Visible = true;                           
                                
                                chkAddMore.Checked = false;                                                   
                                                      
                        }
                    }
                    // If it's an INSERT operation
                    else
                    {
                        AddPhotos(oImgGalleryBL);
                        if (chkAddMore.Checked)
                        {
                            txtGalleryName.Text = sGalleryName;
                            btnPhotoAdd.Text = S_BTN_UPDATE_TEXT;
                            FillClassDetails(oImgGalleryBL);
                        }
                        else
                            ResetPhotoGalleryControls();
                        lblFileMdtNotice.Visible = true;
                        lblUpdate.Text = S_CREATE_MESSAGE;
                        trUpdate.Visible = true;
                    }
                    SetSortProperties();
                    FillPhotoGallery();
                }
                // Gallery name already exists in the database, so display an error.
                else
                {
                    trDuplicatePhotoGallery.Visible = true;
                    trUpdate.Visible = false;
                }
            }
        }
        catch (UploadFileExceptions)
        {
            trUpdate.Visible = false;
        }
        catch (Exception ex)
        {
            txtGalleryName.Text = string.Empty;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
     
    /// <summary>
    /// This event is used to clear all the photo gallery controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPhotoCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetPhotoGalleryControls();
            ClearErrorLabels();
            chkAddMore.Checked = false;
            lblFileMdtNotice.Visible = true;

            if (hidShowPhotoUploadCount.Value.ToInt() > Constants.I_ONE)
                hidShowPhotoUploadCount.Value = Constants.S_ONE;
          
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to perform operations according to Command name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPhotoGallery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex;
            string sGalleryName;
            switch (e.CommandName)
                    

            {
                case S_EDIT_ROW:
                    iRowIndex = Convert.ToInt32(e.CommandArgument);
                    sGalleryName = HttpUtility.HtmlDecode(grdPhotoGallery.Rows[iRowIndex].Cells[I_PHOTO_GALLERY_NAME_COLUMN_INDEX].Text).Trim();
                    txtGalleryName.Text = sGalleryName;
                   
                    hidOrgGalleryName.Value = txtGalleryName.Text;
                    ImageGalleryBL oImageGalleryBL = new ImageGalleryBL();
                    List<int> lstSectionIds = oImageGalleryBL.GetSectionsForParticularPhotoGallery(txtGalleryName.Text,miSchoolId);

                    for (int iItemCount = 0; iItemCount < chkSectionList.Items.Count; iItemCount++)
                        chkSectionList.Items[iItemCount].Selected = false;

                    foreach (var iSectionId in lstSectionIds)
                    {
                        chkSectionList.Items.FindByValue(iSectionId.ToString()).Selected = true;
                    }

                    int ichkListCount = chkSectionList.Items.Count;

                    int iCount = 0;

                    for (int iItemCount = 0; iItemCount < chkSectionList.Items.Count; iItemCount++)
                    {
                        if (chkSectionList.Items[iItemCount].Selected)
                            iCount++;
                    }

                    chkSelectAll.Checked = ichkListCount == iCount;

                    FillClassDetails(oImageGalleryBL);

                    //List<int> lstStandardDivisions = oImageGalleryBL.GetStandardDivisionsPhotoGallery(txtGalleryName.Text, miSchoolId);
                    //int iStdCount = 0;
                    
                    //foreach (var item in lstvwStandardDivisions.Items)
                    //{
                    //    CheckBox chkStandard = item.FindControl("chkStandard") as CheckBox;
                    //    CheckBoxList chkStandardDivLst = item.FindControl("chkStandardDivLst") as CheckBoxList;
                    //    int iCnt = 0;
                    //    for (int iItemCount = 0; iItemCount < chkStandardDivLst.Items.Count; iItemCount++)
                    //    {
                    //        if (lstStandardDivisions.Contains(chkStandardDivLst.Items[iItemCount].Value.ToInt()))
                    //        {
                    //            chkStandardDivLst.Items[iItemCount].Selected = true;
                    //            iCnt++;
                    //        }
                    //        else
                    //            chkStandardDivLst.Items[iItemCount].Selected = false;
                    //    }

                    //    if (iCnt == chkStandardDivLst.Items.Count)
                    //    {
                    //        chkStandard.Checked = true;
                    //        iStdCount++;
                    //    }
                    //    else
                    //        chkStandard.Checked = false;
                    //}

                    lblFileMdtNotice.Visible = false;
                    btnPhotoAdd.Text = S_BTN_UPDATE_TEXT;                   
                    break;
                    case S_DELETE_ROW:
                    iRowIndex = Convert.ToInt32(e.CommandArgument);
                    sGalleryName = HttpUtility.HtmlDecode(grdPhotoGallery.Rows[iRowIndex].Cells[I_PHOTO_GALLERY_NAME_COLUMN_INDEX].Text).Trim();


                    DeleteGalleryFiles(sGalleryName);
                    ImageGalleryBL.DeletePhotoGallery(sGalleryName);
                    SetSortProperties();
                    FillPhotoGallery();
                    ResetPhotoGalleryControls();
                    lblFileMdtNotice.Visible = true;
                    break;
            }
            ResetComments();
            ClearErrorLabels();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to perform operations according to Command name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStandardDivisions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
           if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkStandard = oCurrentItem.FindControl("chkStandard") as CheckBox;
                CheckBoxList chkStandardDivLst = oCurrentItem.FindControl("chkStandardDivLst") as CheckBoxList;
                int iStandardId = lstvwStandardDivisions.DataKeys[iRowId]["StandardId"].ToInt();
                var oList = mlstStandardDivisions.Where(sd => sd.StandardId == iStandardId).OrderBy(sd => sd.OriginalStandardId).ThenBy(sd=>sd.StandardDivisionId).Select(sd => new { sd.StandardDivisionId, sd.DivisionName }).ToList();
                ListSource.FillCheckBoxList(oList, chkStandardDivLst, "DivisionName", "StandardDivisionId");



                chkStandard.Attributes.Add("onclick", "CheckAll(this,'" + iRowId + "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckStd('" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set properties to gridview's column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPhotoGallery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            SetPhotoGridviewRowData(e.Row);
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;
                FillPageIndexCombobox(pagerRow);
                SetCurrentPageLabel(pagerRow);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set record range of displayed records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ObjectDSPhotoGallery_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            SetRecordCountLabels(e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sortImage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPhotoGallery_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidPhotoGallerySortExpression.Value);
                if (sortColumnIndex != -1)
                {
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidPhotoGallerySortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page number of photo gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PhotoGalleryPageDDList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow pagerRow = grdPhotoGallery.BottomPagerRow;
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PhotoGalleryPageDDList");
            grdPhotoGallery.PageIndex = pageList.SelectedIndex;
            FillPhotoGallery();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page index of photo grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPhotoGallery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdPhotoGallery.PageIndex = e.NewPageIndex;
            FillPhotoGallery();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort photo gallery names according to updated date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPhotoGallery_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SetSortDetails(hidPhotoGallerySortExpression, hidPhotoGallerySortDirection, e);
            FillPhotoGallery();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }



    #endregion

    #region Photo Gallery Mathods

    /// <summary>
    /// This method is used to set values record range labels.
    /// </summary>
    /// <param name="e"></param>
    private void SetRecordCountLabels(ObjectDataSourceStatusEventArgs e)
    {
        if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
        {
            lblFirstIndex.Text = Convert.ToString((grdPhotoGallery.PageSize * grdPhotoGallery.PageIndex) + 1);
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblLastIndex.Text = Convert.ToString((Convert.ToInt32(lblFirstIndex.Text) + grdPhotoGallery.PageSize) - 1);
                lblTotalPhotos.Text = e.ReturnValue.ToString();
                if (e.ReturnValue.GetType() != typeof(DataTable))
                {
                    if (e.ReturnValue.ToString() == "0" || Convert.ToInt32(e.ReturnValue) <= Constants.I_GRID_PAGE_COUNT)
                        trPhotoGalleryRowCount.Visible = false;
                    else
                        trPhotoGalleryRowCount.Visible = true;
                    if (Convert.ToInt32(lblLastIndex.Text) > Convert.ToInt32(lblTotalPhotos.Text))
                        lblLastIndex.Text = e.ReturnValue.ToString();
                }

                if (lblTotal.Text != string.Empty)
                {
                    if (Convert.ToInt32(lblTotalPhotos.Text) <= Constants.I_GRID_PAGE_COUNT)
                        trPhotoGalleryRowCount.Visible = false;
                    else
                        trPhotoGalleryRowCount.Visible = true;
                }
            }

        }
       
    }
   
    /// <summary>
    /// This method is used to display current page number.
    /// </summary>
    /// <param name="pagerRow"></param>
    private void SetCurrentPageLabel(GridViewRow pagerRow)
    {
        Label pageLabel = (Label)pagerRow.Cells[0].FindControl("PhotoGalleryCurrentPageLabel");
        if (pageLabel != null)
        {
            // Calculate the current page number.
            int currentPage = grdPhotoGallery.PageIndex + 1;
            // Update the Label control with the current page information.
            pageLabel.Text = "Page " + currentPage.ToString() + " of " + grdPhotoGallery.PageCount.ToString();
        }
    }

    /// <summary>
    /// This method is used to fill page index combobox.
    /// </summary>
    /// <param name="pagerRow"></param>
    private void FillPageIndexCombobox(GridViewRow pagerRow)
    {
        DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PhotoGalleryPageDDList");
        if (pageList != null)
        {
            for (int iPageCount = 0; iPageCount < grdPhotoGallery.PageCount; iPageCount++)
            {
                ListItem oListItem = new ListItem((iPageCount + 1).ToString());
                if (iPageCount == grdPhotoGallery.PageIndex)
                    oListItem.Selected = true;
                pageList.Items.Add(oListItem);
            }
        }
    }

    /// <summary>
    /// This method is used to set default properties.
    /// </summary>
    private void SetDefaultProperties()
    {
        colpnlPhotoGallery.Collapsed = false;
        colpnlVideoGallery.Collapsed = false;
        valSummaryPhotoDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSummaryPhotoUpdate.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSummaryVideo.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        txtGalleryName.Focus();
        for (int iItemCount = 0; iItemCount < chkSectionList.Items.Count; iItemCount++)
                chkSectionList.Items[iItemCount].Selected = true;
        chkSelectAll.Checked = true;

        SetDefaultValuesToUserRole();
        chkAllForVideo.Checked = true;

        SetVideoURlSource();
    }

    private void SetVideoURlSource()
    {
        if (!Settings.IsAaryanSchool)
        {
            ddlUrlSource.SelectedValue = "1";
            ddlUrlSource.Enabled = false;
            lnkRITeSchoolVideo.Visible = false;
        }

        if (ConfigurationManager.AppSettings["AzureMediaWebsiteURL"] != null && ConfigurationManager.AppSettings["AzureMediaWebsiteURL"].ToString() != string.Empty)
        {
            hidVideoWebsiteURL.Value = ConfigurationManager.AppSettings["AzureMediaWebsiteURL"].ToString() + "?" + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&InsertedById=" + miUserId);
        }
    }

    /// <summary>
    /// This method is used to reset photo gallery controls.
    /// </summary>
    private void ResetPhotoGalleryControls()
    {
        ResetComments();
        txtGalleryName.Text = string.Empty;
        hidOrgGalleryName.Value = string.Empty;
        trUpdate.Visible = false;
        if (btnPhotoAdd.Text == S_BTN_UPDATE_TEXT)
            btnPhotoAdd.Text = S_BTN_ADD_TEXT;
        for (int iItemCount = 0; iItemCount < chkSectionList.Items.Count; iItemCount++)
                chkSectionList.Items[iItemCount].Selected = true;
        chkSelectAll.Checked = true;

        ResetClasses();
    }

    private void ResetClasses()
    {
        foreach (var item in lstvwStandardDivisions.Items)
        {
            CheckBox chkStandard = item.FindControl("chkStandard") as CheckBox;
            chkStandard.Checked = false;

            CheckBoxList chkStandardDivLst = item.FindControl("chkStandardDivLst") as CheckBoxList;
            for (int iItemCount = 0; iItemCount < chkStandardDivLst.Items.Count; iItemCount++)
                chkStandardDivLst.Items[iItemCount].Selected = false;
        }

        chkAllDivs.Checked = false;
    }

    /// <summary>
    /// This method is used to fill standard check box list.
    /// </summary>
    private void FillStandardDivisionLstBox()
    {
        ImageGalleryBL oImageGalleryBL = new ImageGalleryBL();
        mlstStandardDivisions = oImageGalleryBL.GetStandrdDiVision(miSchoolId, miAcademicYearId);
        
         var oData =    mlstStandardDivisions.OrderBy(sd => sd.OriginalStandardId).Select(sd => new { sd.StandardId, sd.StandardName}).Distinct().ToList();
         lstvwStandardDivisions.DataSource = oData;
         lstvwStandardDivisions.DataBind();

        ///This block is used to fill Standard Divisions for Video gallery.
         lstvwVideoStandardDivision.DataSource = oData;
         lstvwVideoStandardDivision.DataBind();
    }

    private void FillSectionForPhotoGallery()
    {
        ImageGalleryBL oImageGalleryBL = new ImageGalleryBL();
        List<ImageGallery> lstImageGallery = oImageGalleryBL.GetAllCategories();
        ListSource.FillCheckBoxList(lstImageGallery, chkSectionList, "SectionName", "SectionId");        
    }

    /// <summary>
    /// This method is used to clear comments.
    /// </summary>
    private void ResetComments()
    {
        TextBox txtComment;
        for (int iIndex = 1; iIndex <= 20; iIndex++)
        {
            txtComment = (TextBox)this.FindControl("ctl00$MainBody$txtComment" + iIndex);
            txtComment.Text = string.Empty;
            Label oLabel = (Label)this.FindControl("ctl00$MainBody$lblErrMsg" + iIndex);
            oLabel.Text = string.Empty;
        }
    }

    /// <summary>
    /// This method is used to fill photo galleries into gridview.
    /// </summary>
    private void FillPhotoGallery()
    {
        grdPhotoGallery.DataSourceID = ObjectDSPhotoGallery.ID;
        
    }

    /// <summary>
    /// This method is used to add photos into new/existing gallery.
    /// </summary>
    private void AddPhotos(ImageGalleryBL oImageGalleryBL)
    {
        try
        {
            // Validate all files uploaded for the size and extention.
            string sGalleryName = txtGalleryName.Text.Trim();            
            if (ValidateImageFiles())
            {
                // If ADD operation, delete the Gallery files(Zip & XML) as both of them will get created again
                if (btnPhotoAdd.Text == S_BTN_ADD_TEXT || CheckIfAtleastOneFileIsSelected())
                {
                    string sOldGalleryName = hidOrgGalleryName.Value;
                    DeleteGalleryFiles(sOldGalleryName);
                }                
           
                // Get XML for all the images to be uploaded.               
                string sXML = GetUploadFilesXML();
                oImageGalleryBL.ImageDetails = sXML;                

                oImageGalleryBL.ClassesIds = GetClasses();
                oImageGalleryBL.SavePhotoGallery();
                moDtImageGallery = ImageGalleryBL.GetImages(miSchoolId, sGalleryName);
                CreateGalleryFlashXML(sGalleryName);
                CreateGalleryImagesZip(sGalleryName);

                hidOrgGalleryName.Value = sGalleryName;
                trDuplicatePhotoGallery.Visible = false;
                lblFileMdtNotice.Visible = true;
                ClearErrorLabels();
                ResetPhotoGalleryControls();
                btnPhotoAdd.Text = S_BTN_ADD_TEXT;
                hidShowPhotoUploadCount.Value = Constants.S_ONE;
            }
            if (chkAddMore.Checked)
            {
                txtGalleryName.Text = sGalleryName;
                hidOrgGalleryName.Value = txtGalleryName.Text;
                btnPhotoAdd.Text = S_BTN_UPDATE_TEXT;

                FillClassDetails(oImageGalleryBL);
            }
        }
        catch (UploadFileExceptions ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to Fill the class details if user check add more combobox.
    /// </summary>
    /// <param name="oImageGalleryBL"></param>
    private void FillClassDetails(ImageGalleryBL oImageGalleryBL)
    {
        List<int> lstStandardDivisions = oImageGalleryBL.GetStandardDivisionsPhotoGallery(txtGalleryName.Text, miSchoolId);
        int iStdCount = 0;

        foreach (var item in lstvwStandardDivisions.Items)
        {
            CheckBox chkStandard = item.FindControl("chkStandard") as CheckBox;
            CheckBoxList chkStandardDivLst = item.FindControl("chkStandardDivLst") as CheckBoxList;
            int iCnt = 0;
            for (int iItemCount = 0; iItemCount < chkStandardDivLst.Items.Count; iItemCount++)
            {
                if (lstStandardDivisions.Contains(chkStandardDivLst.Items[iItemCount].Value.ToInt()))
                {
                    chkStandardDivLst.Items[iItemCount].Selected = true;
                    iCnt++;
                }
                else
                    chkStandardDivLst.Items[iItemCount].Selected = false;
            }

            if (iCnt == chkStandardDivLst.Items.Count)
            {
                chkStandard.Checked = true;
                iStdCount++;
            }
            else
                chkStandard.Checked = false;
        }

        chkAllDivs.Checked = iStdCount == lstvwStandardDivisions.Items.Count;
    }    

    private string GetClasses()
    {
        StringBuilder oStandards = new StringBuilder();
        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
            {
                if (chkStandardDivLst.Items[iCount].Selected)
                    oStandards.Append("," + chkStandardDivLst.Items[iCount].Value);
            }
        }

        string sIds = string.Empty;
        if (oStandards.ToString().Length > 0)
            sIds = oStandards.ToString().Substring(1);
        return sIds;
    }

    /// <summary>
    /// Creates a Zip archive of a Photo Gallery
    /// </summary>
    /// <param name="asGalleryName">Name of the Photo Gallery</param>
    private void CreateGalleryImagesZip(string asGalleryName)
    {
        if (moDtImageGallery != null && moDtImageGallery.Rows.Count > 0 && moDtImageGallery.Rows[0][0] != DBNull.Value)
        {
            string sFileName;
            int iIndex;
            int iCount = 0;
            string sDestination = Server.MapPath("..") + "\\DOWNLOADS\\" + asGalleryName + ".zip";
            if (File.Exists(sDestination))
                File.Delete(sDestination);
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(sDestination))
            {
                iCount = moDtImageGallery.Rows.Count;
                for (iIndex = 0; iIndex < iCount; iIndex++)
                {
                    sFileName = Server.MapPath("..") + "\\" + moDtImageGallery.Rows[iIndex][0].ToString();
                    zip.AddFile(sFileName, asGalleryName);
                }
                zip.Save();
            }
        }
    }

    /// <summary>
    /// Deletes the Zip and XML files related to a Photo Gallery
    /// </summary>
    /// <param name="asGalleryName">Name of the Photo Gallery</param>
    private void DeleteGalleryFiles(string asGalleryName)
    {
        // Delete the Zip archive
        string sGalleryZipFilePath = Server.MapPath("..") + "\\DOWNLOADS\\" + asGalleryName + ".zip";
        if (File.Exists(sGalleryZipFilePath))
            File.Delete(sGalleryZipFilePath);

        // Physically delete all the files that belonged to the Gallery
        if (btnPhotoAdd.Text == S_BTN_ADD_TEXT)
        {
            DataTable oGalleryImages = moDtImageGallery;
            if (oGalleryImages == null)
                oGalleryImages = ImageGalleryBL.GetImages(miSchoolId, asGalleryName);
            int iRowCount = oGalleryImages.Rows.Count;
            if (oGalleryImages != null && iRowCount > 0)
            {
                string sFileName;
                for (int i = 0; i < iRowCount; i++)
                {
                    if (oGalleryImages.Rows[i][0] != DBNull.Value)
                    {
                        sFileName = Server.MapPath("..") + "\\" + oGalleryImages.Rows[i][0].ToString();
                        if (File.Exists(sFileName))
                            File.Delete(sFileName);
                    }
                }
            }
        }
        // Delete the XML file
        string sGalleryXMLFilePath = Server.MapPath("..") + "\\Gallery\\" + asGalleryName + ".xml";
        if (File.Exists(sGalleryXMLFilePath))
            File.Delete(sGalleryXMLFilePath);
    }

    /// <summary>
    /// This method is used to add attributes on view and delete button of photo gridview.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetPhotoGridviewRowData(GridViewRow gridViewRow)
    {
        int iRowIndex = gridViewRow.RowIndex;
        if (iRowIndex >= 0)
        {
            string sImageGalleryName = gridViewRow.Cells[I_PHOTO_GALLERY_NAME_COLUMN_INDEX].Text;
            SetViewButtonAttribute(gridViewRow, HttpUtility.HtmlDecode(sImageGalleryName));
            SetSlideShowAttribute(gridViewRow, HttpUtility.HtmlDecode(sImageGalleryName));
            ImageButton oDeleteGallery = (ImageButton)gridViewRow.FindControl("btnDeleteImageGallery");
            oDeleteGallery.Attributes.Add("Onclick", "if(!ConfirmPhotoGalleryDelete()){return false;}");

            ImageButton imgDownload = (ImageButton)gridViewRow.Cells[3].FindControl("btnDownload");
            string sDestination = Server.MapPath("..") + "\\DOWNLOADS\\" + HttpUtility.HtmlDecode(sImageGalleryName) + ".zip";
            sImageGalleryName = HttpUtility.HtmlDecode(sImageGalleryName);
            if (File.Exists(sDestination))
            {
                imgDownload.Attributes.Add("onclick", "window.open('../downloads/" + sImageGalleryName.Replace("'", "\\'") + ".zip','_self'); {return false};");
            }
        }
    }

    /// <summary>
    /// This method is used to add attribute to view button.
    /// </summary>
    /// <param name="gridViewRow"></param>
    /// <param name="sImageGalleryName"></param>
    private void SetViewButtonAttribute(GridViewRow gridViewRow, string sImageGalleryName)
    {
        string sQueryString = "ImageGalleryName=" + StringUtility.DoHTMLEncoding(sImageGalleryName,false);
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        ImageButton oViewPhotoGallery = (ImageButton)gridViewRow.FindControl("btnViewImageGallery");
        oViewPhotoGallery.Attributes.Add("Onclick", "if (!ShowPhotos('" + sEncryptedString + "')) {return false;}");
    }

    /// <summary>
    /// This method is used to add attribute to view button.
    /// </summary>
    /// <param name="gridViewRow"></param>
    /// <param name="sImageGalleryName"></param>
    private void SetVideoViewButtonAttribute(GridViewRow gridViewRow, int iVideoId, int iSubjectId,int iUrlSourceId,string sURLSource, string sSubjectName)
    {
        string sVideoGallaryName = gridViewRow.Cells[I_VIDEO_GALLERY_NAME_COLUMN_INDEX].Text;
        string sQueryString = "VideoGallaryId=" + iVideoId.ToString() + "&VideoGallaryName=" + StringUtility.DoHTMLEncoding(sVideoGallaryName, false) + "&SubjectId=" + iSubjectId.ToString() + "&UrlSourceId=" + iUrlSourceId.ToString() + "&URLSource=" + sURLSource.ToString() + "&SubjectName=" + sSubjectName; ;
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        ImageButton oViewVideoGallery = (ImageButton)gridViewRow.FindControl("btnViewVideoGallery");        
        //Response.Redirect("UploadVideoViewUI.aspx?" + sEncryptedString);
        oViewVideoGallery.Attributes.Add("Onclick", "if (!ShowVideos('" + sEncryptedString + "')) {return false;}");
    }

    /// <summary>
    /// This method is used to add attribute to slide show button.
    /// </summary>
    /// <param name="gridViewRow"></param>
    /// <param name="sImageGalleryName"></param>
    private void SetSlideShowAttribute(GridViewRow gridViewRow, string sImageGalleryName)
    {
        string sQueryString = "xmlpath=" + StringUtility.DoHTMLEncoding(sImageGalleryName,false) + ".xml";
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        ImageButton oSlideShow = (ImageButton)gridViewRow.FindControl("btnSlideShow");
        oSlideShow.Attributes.Add("Onclick", "if (!ShowGallery('" + sEncryptedString + "')) {return false;}");
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnPhotoAdd, btnPhotoCancel, btnVideoAdd, btnVideoCancel});
        btnVideoCancel.Attributes.Add("onclick", "ClearVideoValSum();");
        btnPhotoCancel.Attributes.Add("onclick", "ClearPhoto();");
        btnPhotoAdd.Attributes["onclick"] = "DisableButtons()";
        btnVideoAdd.Attributes["onclick"] = "DisableButtons()";
        trAssociatedClass.Visible = Settings.ShowPhotoGalleryPerClasswise;
        chkAllDivs.Attributes.Add("onclick", "SelectAllDivisions(this)");
        chkAllDivForVdo.Attributes.Add("onclick", "SelectAllDivisionsForVideo(this)");

        if (Settings.ShowPhotoGalleryPerClasswise)
            hidPhotoGalleryPerClasswise.Value = Constants.S_ONE;
        else
            hidPhotoGalleryPerClasswise.Value = Constants.S_ZERO;

        hidShowPhotoUploadCount.Value = Constants.S_ONE;
        hidShowVideoUploadCount.Value = Constants.S_ONE;

        if (Settings.IsAaryanSchool)
        {
            trSubjectDetails.Visible = true;
            trStartDate.Visible = true;
            trEndDate.Visible = true;
        }
        else
        {
            trSubjectDetails.Visible = false;
            trStartDate.Visible = false;
            trEndDate.Visible = false;
            reqVideoStartDate.Enabled = false;
            reqVideoEndDate.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to initialize photo gallery.
    /// </summary>
    /// <returns></returns>
    private ImageGalleryBL InitializePhotoGallery()
    {
        ImageGalleryBL oGallery = new ImageGalleryBL();
        oGallery.AcademicYrId = miAcademicYearId;
        oGallery.SchoolId = miSchoolId;
        oGallery.OrgGalleryName = hidOrgGalleryName.Value;
        oGallery.GalleryName = txtGalleryName.Text.Trim();
        oGallery.Inserted_By_id = miUserId; 
        return oGallery;
       
        
       
    }

    /// <summary>
    /// This method is used to check login role and display gallery view.
    /// </summary>
    private void CheckLoginRoleAndRedirect()
    {
        //if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
        //{
        //        Char cCanEdit = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.PhotoGallery);
        //    if (cCanEdit == Constants.C_NO)
        //    {
        //        MasterPage oMasterPage = (MasterPage)this.Master;
        //        oMasterPage.RedirectToNextPage("~/RITeSchool/common/ShowImageGallery.aspx");
        //    }
        //}
    }

    /// <summary>
    /// This method is used to set default properties.
    /// </summary>
    private void SetSortProperties()
    {
        hidSortExpression.Value = "VG." + S_UPDATE_DATE;
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = hidSortExpression.Value + " " + hidSortDirection.Value;

        hidPhotoGallerySortExpression.Value = S_UPDATE_DATE;
        hidPhotoGallerySortDirection.Value = Constants.S_DESCENDING;
        hidPhotoGallerySortExpression.Value = hidPhotoGallerySortExpression.Value + " " + hidPhotoGallerySortDirection.Value;
    }

    /// <summary>
    /// This method is used to create XML of given photo gallery & save it on the server
    /// </summary>
    /// <param name="asGalleryName"></param>
    private void CreateGalleryFlashXML(string asGalleryName)
    {
        const int I_IMAGE_WIDTH = 800;
        const int I_IMAGE_HEIGHT = 500;
        const string S_ELEMENT = "element";

        if (moDtImageGallery.Rows.Count > 0)
        {
            // Create XML file for these images.
            XmlDocument oDoc = new XmlDocument();

            // Create a root level element.
            XmlElement root = oDoc.CreateElement("gallery");
            XmlNode oXmlBaseNode = GetBaseXMLDocument(ref oDoc, ref root, asGalleryName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "images", string.Empty);

            string sAtrrName = "id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = "images";
            oXmlRootNode.Attributes.Append(attr);

            foreach (DataRow oRow in moDtImageGallery.Rows)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "image", string.Empty);
                string sPath = oRow["Image_Path"].ToString();
                sPath = sPath.Substring(sPath.LastIndexOf("\\") + 1);

                sAtrrName = "path";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sPath;
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "width";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = I_IMAGE_WIDTH.ToString();
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "height";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = I_IMAGE_HEIGHT.ToString();
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "thumbpath";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sPath;
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "comment";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = oRow["Comment"].ToString(); 
                oXmlNode.Attributes.Append(attr);

                oXmlRootNode.AppendChild(oXmlNode);
            }

            oXmlBaseNode.AppendChild(oXmlRootNode);

            // Add the root node to document element. 
            oDoc.AppendChild(oXmlBaseNode);

            // Replace spaces with underscore in Gallery Name
            string sGalleryXMLFilePath = Server.MapPath("..") + "\\Gallery\\" + asGalleryName + ".xml";

            // File a file by the same name exists, delete it & Save the new XML file
            if (File.Exists(sGalleryXMLFilePath))
                File.Delete(sGalleryXMLFilePath);
            oDoc.Save(sGalleryXMLFilePath);
        }
    }

    /// <summary>
    /// This function is used to update the Gallery Files (Zip & XML)
    /// </summary>
    /// <param name="asOldGalleryName"></param>
    /// <param name="asNewGalleryName"></param>
    private void UpdateGalleryFiles(string asOldGalleryName, string asNewGalleryName)
    {
        // Update the XML filename
        string sOldPath = Server.MapPath("..") + @"\Gallery\" + asOldGalleryName + ".xml";
        string sNewPath = Server.MapPath("..") + @"\Gallery\" + asNewGalleryName + ".xml";
        if (!File.Exists(sNewPath))
            File.Move(sOldPath, sNewPath);

        // Update the Gallery name in the new XML file
        XmlDocument oXMLDoc = new XmlDocument();
        oXMLDoc.Load(sNewPath);

        XmlAttribute attr = oXMLDoc.CreateAttribute("date");
        attr.Value = "Gallery Name :  " + asNewGalleryName;
        oXMLDoc.FirstChild.Attributes.Append(attr);

        oXMLDoc.Save(sNewPath);

        // Update the Zip filename
        sOldPath = Server.MapPath("..") + @"\DOWNLOADS\" + asOldGalleryName + ".zip";
        sNewPath = Server.MapPath("..") + @"\DOWNLOADS\" + asNewGalleryName + ".zip";
        if (!File.Exists(sNewPath))
            File.Move(sOldPath, sNewPath);     
       
    }

    /// <summary>
    /// This method is used to validates image extentions and size of each image. 
    /// </summary>
    /// <returns></returns>
    private bool ValidateImageFiles()
    {
        bool bResult = true;
        //const string S_FILE_SIZE_ERROR_MESSAGE = " File size exceeds.";
        //const string S_FILE_TYPE_ERROR_MESSAGE = " Invalid file type.";
        //const string S_FILE_EXT_ERROR_MESSAGE = "  Invalid file path.";
        //const string S_EXTN_JPEG = ".JPEG";
        //const string S_EXTN_JPG = ".JPG";
        //const string S_EXTN_BMP = ".BMP";
        //const string S_EXTN_PNG = ".PNG";
        //const int I_FILE_SIZE_LIMIT = 10485760;

        //int iFileSize = 0;
        //for (int iCount = 1; iCount <= 20; iCount++)
        //{
        //    FileUpload oUpload = (FileUpload)this.FindControl("ctl00$MainBody$flImage" + iCount);
        //    Label oLabel = (Label)this.FindControl("ctl00$MainBody$lblErrMsg" + iCount);
        //    oLabel.Text = string.Empty;

        //    StringBuilder obj = new StringBuilder();
        //    if (oUpload.HasFile)
        //    {
        //        iFileSize = 0;
        //        foreach (HttpPostedFile uploadedFile in oUpload.PostedFiles)
        //        {
        //            string sFileName = uploadedFile.FileName;
        //            if (sFileName != string.Empty)
        //            {
        //                //if (uploadedFile.HasFile)
        //                //{
        //                if (!sFileName.ToUpper().EndsWith(S_EXTN_JPEG) && !sFileName.ToUpper().EndsWith(S_EXTN_JPG) &&
        //                    !sFileName.ToUpper().EndsWith(S_EXTN_BMP) && !sFileName.ToUpper().EndsWith(S_EXTN_PNG))
        //                {
        //                    obj.Append(S_FILE_TYPE_ERROR_MESSAGE);
        //                    bResult = false;
        //                    break;
        //                }

        //                if (uploadedFile.ContentLength > I_FILE_SIZE_LIMIT)
        //                {
        //                    obj.Append("," + S_FILE_SIZE_ERROR_MESSAGE);
        //                    break;
        //                    bResult = false;
        //                }
        //                else
        //                    iFileSize += uploadedFile.ContentLength;
        //            }
        //        }

        //        if (iFileSize > I_FILE_SIZE_LIMIT)
        //            obj.Append("," + S_FILE_SIZE_ERROR_MESSAGE);
        //    }

        //    if (obj.Length > 0)
        //        oLabel.Text = obj.ToString();
        //}

        //if (!bResult)
        //    throw new UploadFileExceptions(string.Empty);        
        return bResult;
    }

    /// <summary>
    /// This function is used to determine if atleast one file is selected for upload.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfAtleastOneFileIsSelected()
    {
        bool bResult = false;

        for (int iCount = 1; iCount <= 20; iCount++)
        {
            FileUpload oUpload = (FileUpload)this.FindControl("ctl00$MainBody$flImage" + iCount);
            if (oUpload.HasFile)
            {
                bResult = true;
                break;
            }
        }

        return bResult;
    }

    /// <summary>
    /// This method is used to clear error labels.
    /// </summary>
    private void ClearErrorLabels()
    {
        for (int iCount = 1; iCount <= 5; iCount++)
        {
            Label oLabel = (Label)this.FindControl("ctl00$MainBody$lblErrMsg" + iCount);
            oLabel.Text = string.Empty;
        }
    }

    /// <summary>
    /// Thos method is used to generate XML string.
    /// </summary>
    /// <returns></returns>

    
    private string GetUploadFilesXML()
    {

        ImageGalleryBL oImageGalleryBL = new ImageGalleryBL();

        int iStartCount = oImageGalleryBL.GetPhotoCount(txtGalleryName.Text.Trim()) + 1;
        const string S_ELEMENT = "element";
        XmlDocument oXMLDoc = new XmlDocument();

        // Create a root level element.
        XmlElement oXMLRootElement = oXMLDoc.CreateElement("PhotoGallery");
        XmlNode oXmlRootNode = oXMLDoc.CreateNode(S_ELEMENT, "PhotoGallery", string.Empty);
        XmlNode oXmlNode = oXMLDoc.CreateNode(S_ELEMENT, "PhotoGallery", string.Empty);

        // Create distinct names for the images. Logic - <Sr_No>_<file name>
        // Save the files images\<gallery name> folder.

        HttpFileCollection oCollection = Request.Files;
        for (int iCount = 0; iCount < oCollection.Count; iCount++)
        {
            HttpPostedFile aoAttachment = oCollection[iCount];

            string sFileName = aoAttachment.FileName;

            if (!aoAttachment.FileName.Trim().Equals(string.Empty))
                {
                    string sIndexNo = oCollection.Keys[iCount].Replace("ctl00$MainBody$flImage", string.Empty);

                    sFileName = GetFileNameForRenaming(@"Images\Gallery" + @"\" + sFileName);
                    aoAttachment.SaveAs(Server.MapPath("..") + @"\" + sFileName);
                    oXmlNode = oXMLDoc.CreateNode(S_ELEMENT, "PhotoGallery", string.Empty);

                    string sAtrrName = "Image_Path";
                    XmlAttribute attr = oXMLDoc.CreateAttribute(sAtrrName);
                    attr.Value = sFileName;
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Image_SrNo";
                    attr = oXMLDoc.CreateAttribute(sAtrrName);
                    attr.Value = iStartCount.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Comment";
                    attr = oXMLDoc.CreateAttribute(sAtrrName);
                    TextBox txtComment = (TextBox)this.FindControl("ctl00$MainBody$txtComment" + sIndexNo);
                    attr.Value = StringUtility.ReplaceSingleQuoteInString(txtComment.Text.Trim(), false);
                    oXmlNode.Attributes.Append(attr);

                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                    iStartCount++;
                }
        }
        // Add the root node to document element. 
        oXMLRootElement.AppendChild(oXmlRootNode);
        return oXMLRootElement.InnerXml;
    }

    /// <summary>
    /// This method is used to generate base XML Document.
    /// </summary>
    /// <param name="oDoc"></param>
    /// <param name="root"></param>
    /// <param name="asGalleryName"></param>
    /// <returns></returns>
    private XmlNode GetBaseXMLDocument(ref XmlDocument oDoc, ref XmlElement root, string asGalleryName)
    {
        const string S_ELEMENT = "element";
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "gallery", string.Empty);

        string sAtrrName = "base";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "background";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#ffffff";
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "banner";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#ffffff";
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "text";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#cc3366";
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "link";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#1313A2";
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "alink";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#8F6F6F";
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "vlink";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#550080";
        oXmlRootNode.Attributes.Append(attr);

        sAtrrName = "date";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "Gallery Name :  " + asGalleryName;
        oXmlRootNode.Attributes.Append(attr);

        // Next element "banner".
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "banner", string.Empty);

        sAtrrName = "font";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "Verdana";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "fontsize";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "5";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "color";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#F0F0F0";
        oXmlNode.Attributes.Append(attr);

        oXmlRootNode.AppendChild(oXmlNode);

        // Next element "thumbnail".
        oXmlNode = oDoc.CreateNode(S_ELEMENT, "thumbnail", string.Empty);

        sAtrrName = "base";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "/RITeSchool/images/gallery/";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "font";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "Verdana";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "fontsize";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "4";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "color";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#F0F0F0";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "border";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "0";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "rows";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "0";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "col";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "0";
        oXmlNode.Attributes.Append(attr);

        oXmlRootNode.AppendChild(oXmlNode);

        // Next element "large".
        oXmlNode = oDoc.CreateNode(S_ELEMENT, "large", string.Empty);

        sAtrrName = "base";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "../images/gallery/";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "font";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "Verdana";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "fontsize";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "4";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "color";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "#F0F0F0";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "border";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "0";
        oXmlNode.Attributes.Append(attr);

        oXmlRootNode.AppendChild(oXmlNode);

        return oXmlRootNode;
    }

    /// <summary>
    /// This method modifies the file name as it is duplicate on the server.
    /// Current time:monutes:seconds are appended to the file name and same is then returned.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string GetFileNameForRenaming(string asFileName)
    {
        string sFileName;
        // Remove the extension from the file name.
        sFileName = asFileName.Substring(0, asFileName.LastIndexOf("."));
        // Append the time format to the file name.
        if (!IsDuplicatePhoto(asFileName))
            sFileName = sFileName + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        else
        {
            Thread.Sleep(1);
            sFileName = sFileName + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
        }
        // Again append the original extension of the file.
        sFileName = sFileName + asFileName.Substring(asFileName.LastIndexOf("."));
        // Return the file name.
        return sFileName;
    }

    /// <summary>
    /// This method is used to check duplicate photo name.
    /// </summary>
    private bool IsDuplicatePhoto(string asFileName)
    {
        oPathList.Contains(asFileName);
        if (!oPathList.Contains(asFileName))
        {
            oPathList.Add(asFileName);
            return false;
        }
        return true;
    }

    #endregion

    #endregion

    #region Video Gallery

    #region Video Galary Events

    /// <summary>
    /// This event is used to add/update video gallery information.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVideoAdd_Click(object sender, EventArgs e)
    {
        try
        {   
            if (txtVideoName.Text != string.Empty)
            {
                VideoGalleryBL oVideoGalleryBL = InitialiazeVideoGallery();
                string sMassage = GetValidityMassage(oVideoGalleryBL);
                if (sMassage == "Valid")
                {
                    //oVideoGalleryBL.Video_Url = txtVideoUrl.Text.Trim();
                    int aiId = 0;
                    DateTime dtStartDate = Convert.ToDateTime("01-Jan-1900");
                    DateTime dtEndDate = Convert.ToDateTime("01-Jan-1900");

                    if (txtStartDate.Text != string.Empty)
                        dtStartDate = txtStartDate.Text.ToDateTime();

                    if (txtEndDate.Text != string.Empty)
                        dtEndDate = txtEndDate.Text.ToDateTime();

                    bool bAddMoreSubjects = false;

                    if (hidAddMoreSubjects.Value == Constants.S_ONE)
                        bAddMoreSubjects = true;

                    if (btnVideoAdd.Text == S_BTN_ADD_TEXT)
                    {
                        VideoDetails oVideoDetails = new VideoDetails
                        {
                            SchoolId = miSchoolId,
                            VideoId = hidVedioId.Value.ToInt(),
                            sVideoName = txtVideoName.Text,
                            VideoDetailsXML = GenerateXml(),
                            InsertedById = miUserId,
                            StartDate = dtStartDate,
                            EndDate = dtEndDate,
                            UserRoleIds = GetSelectedStaffGroups(),
                            StandardDivisionIds = GetClassesForVideo(),
                            SubjectId = cmbSubject.SelectedValue.ToInt(),
                            ShowOnExternalWebsite = chkShowOnExternal.Checked,
                            OldSubjectId = Constants.I_ZERO,
                             UrlSourceId=ddlUrlSource.SelectedIndex.ToInt()/////////////////////////new line add
                        };

                        oVideoGalleryBL.InsertVideoGallery(hidVedioId.Value.ToInt(), oVideoDetails, bAddMoreSubjects, out aiId);
                        if (chkAddMoreVideos.Checked)
                        {
                            hidVedioId.Value = aiId.ToString();
                        }
                        trUpdate.Visible = true;
                        lblVideoMessage.Text = "Video gallery saved successfully!!!";
                    }
                    else
                    {
                        VideoDetails oVideoDetails = new VideoDetails
                        {
                            SchoolId = miSchoolId,
                            VideoId = hidVedioId.Value.ToInt(),
                            sVideoName = txtVideoName.Text,                            
                            InsertedById = miUserId,
                            StartDate = dtStartDate,
                            EndDate = dtEndDate,
                            UserRoleIds = GetSelectedStaffGroups(),
                            StandardDivisionIds = GetClassesForVideo(),
                            SubjectId = cmbSubject.SelectedValue.ToInt(),
                            ShowOnExternalWebsite = chkShowOnExternal.Checked,
                            OldSubjectId = hidOldSubjectId.Value.ToInt(),
                            UrlSourceId = ddlUrlSource.SelectedIndex.ToInt()/////////////////////////new line add
                        };

                        oVideoGalleryBL.InsertVideoGallery(hidVedioId.Value.ToInt(), oVideoDetails, bAddMoreSubjects, out aiId);
                        btnVideoAdd.Text = S_BTN_ADD_TEXT;
                        trUpdate.Visible = true;
                        lblVideoMessage.Text = " Video gallery updated successfully!!!";
                    }
                    trDuplicateVideoGalleryname.Visible = false;
                    SetSortProperties();
                    FillVideoGallery();
                    ResetVideoGalleryControls();                   
                }
                else
                {
                    trDuplicateVideoGalleryname.Visible = true;
                    lblDuplicatevideo.Text = sMassage;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private string GetSelectedStaffGroups()
    {
        StringBuilder sbStaffGroupIds = new StringBuilder();
        string sStaffGroupIds = string.Empty;

        for (int iItemCount = 0; iItemCount < chkUserRoleLst.Items.Count; iItemCount++)
        {
            if (chkUserRoleLst.Items[iItemCount].Selected)
            {
                sbStaffGroupIds = sbStaffGroupIds.Append("," + chkUserRoleLst.Items[iItemCount].Value);

            }
        }

        if (sbStaffGroupIds.ToString().StartsWith(","))
            sStaffGroupIds = sbStaffGroupIds.ToString().Substring(1);

        return sStaffGroupIds;
    }

    private string GenerateXml()
    {
        List<SaveVideoDetails> lstVVideoDetails = new List<SaveVideoDetails>();

        for (int iValue = 1; iValue <= 25; iValue++)
        {
            TextBox otxtURL = (TextBox)this.FindControl("ctl00$MainBody$txtVideoUrl" + iValue);
            TextBox otxtDesc = (TextBox)this.FindControl("ctl00$MainBody$txtVidoComment" + iValue);

            if (otxtURL.Text != null && otxtURL.Text != null)
            {
                if (otxtURL.Text != string.Empty || otxtDesc.Text != string.Empty)
                {
                    SaveVideoDetails lstVideoDetails = new SaveVideoDetails
                    {
                          VideoId = hidVedioId.Value.ToInt(),
                          VideoURL = otxtURL.Text,
                          Comment = otxtDesc.Text
                    };                    
                    lstVVideoDetails.Add(lstVideoDetails);
                }
            }            
        }
        return base.GenerateXml(lstVVideoDetails);
    }

    /// <summary>
    /// This event is used to clear video gallery controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVideoCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearVideoGalleryDetails(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region Video Gridview Events

    /// <summary>
    /// This event is used to perform operations according to Command name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex;
            hidAddMoreSubjects.Value = Constants.S_ZERO;
            if (e.CommandName == S_EDIT_ROW)
            {
                iRowIndex = Convert.ToInt32(e.CommandArgument);
                hidVedioId.Value = grdVideoGallery.DataKeys[iRowIndex][S_VIDEO_ID].ToString();
                tblMoreVideoUpload.Visible = false;
                FillVideoGalleryDetails(iRowIndex, false);
            }
            if (e.CommandName == S_DELETE_ROW)
            {
                iRowIndex = Convert.ToInt32(e.CommandArgument);
                DeleteVideo(iRowIndex);
                SetSortProperties();
                FillVideoGallery();
                lblVideoMessage.Text = "Video gallery deleted successfully!!!";
            }
            if (e.CommandName == S_ADD_MORE_SUBJECTS)
            {
                iRowIndex = Convert.ToInt32(e.CommandArgument);
                hidVedioId.Value = grdVideoGallery.DataKeys[iRowIndex][S_VIDEO_ID].ToString();
                hidAddMoreSubjects.Value = Constants.S_ONE;
                FillVideoGalleryDetails(iRowIndex, true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set record range of video gallery gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ObjectDSVideoGallery_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdVideoGallery.PageSize * grdVideoGallery.PageIndex) + 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdVideoGallery.PageSize) - 1);
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (e.ReturnValue.ToString() == "0" || grdVideoGallery.PageCount == 0)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;

                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                    }

                    if (lblTotal.Text != string.Empty)
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
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
    /// This method is used to set sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);
                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow pagerRow = grdVideoGallery.BottomPagerRow;
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            grdVideoGallery.PageIndex = pageList.SelectedIndex;
            FillVideoGallery();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdVideoGallery.PageIndex = e.NewPageIndex;
            FillVideoGallery();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort gallery names according to updated date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SetSortDetails(hidSortExpression, hidSortDirection, e);
            FillVideoGallery();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill page index combobox and set current page label.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        try
        {
            SetRowData(e.Row);
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");
                if (pageList != null)
                {
                    for (int i = 0; i < grdVideoGallery.PageCount; i++)
                    {
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (i == grdVideoGallery.PageIndex)
                            item.Selected = true;
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdVideoGallery.PageIndex + 1;
                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() + " of " + grdVideoGallery.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion//Video Gridview Events

    #endregion//Video Galary Event(s)

    #region Video Gallery Methods
    /// <summary>
    /// This method is used to initialize vedio gallery.
    /// </summary>
    /// <returns></returns>
    private VideoGalleryBL InitialiazeVideoGallery()
    {
        VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
        oVideoGalleryBL.Academic_Year_Id = miAcademicYearId;
        oVideoGalleryBL.School_Id = miSchoolId;
        oVideoGalleryBL.Video_Name = txtVideoName.Text.Trim();

        if (btnVideoAdd.Text == S_BTN_ADD_TEXT)
            oVideoGalleryBL.Inserted_By_Id = miUserId;
        else
        {
            oVideoGalleryBL.Updated_By_Id = miUserId;
            oVideoGalleryBL.Video_Id = Convert.ToInt32(hidVedioId.Value);
        }
        return oVideoGalleryBL;
    }

    /// <summary>
    /// This method is used to delete video gallery.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void DeleteVideo(int iRowIndex)
    {
        hidVedioId.Value = grdVideoGallery.DataKeys[iRowIndex][S_VIDEO_ID].ToString();
        int iSubjectId = grdVideoGallery.DataKeys[iRowIndex]["SubjectId"].ToInt();
        //VideoGalleryBL oVideoGalleryBL = InitialiazeVideoGallery();
        VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
        oVideoGalleryBL.Video_Id = Convert.ToInt32(hidVedioId.Value);
        oVideoGalleryBL.SubjectId = iSubjectId;
        oVideoGalleryBL.School_Id = miSchoolId;
        oVideoGalleryBL.Updated_By_Id = miUserId;
        oVideoGalleryBL.DeleteVideoGallery();
        ResetVideoGalleryControls();
    }

    /// <summary>
    /// This method is used to clear controls and fill video gallery gridview.
    /// </summary>
    private void ResetVideoGalleryControls()
    {
        trDuplicateVideoGalleryname.Visible = false;        
        FillVideoGallery();

        ClearVideoGalleryDetails(!chkAddMoreVideos.Checked);
    }

    /// <summary>
    /// This method is used to get video url.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetVideoUrl(string asUrl)
    {
        if (asUrl != string.Empty)
        {
            int iLastIndex = asUrl.LastIndexOf('?');
            if (iLastIndex != -1)
            {
                string sUrl = asUrl.Substring(0, iLastIndex);
                string sQueryString = asUrl.Substring(asUrl.LastIndexOf('?') + 1);
                HttpRequest moHttpRequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                                sUrl,
                                                sQueryString);
                if (moHttpRequest.QueryString["v"] == null)
                    throw new ApplicationException(" Invalid video url");
                return moHttpRequest.QueryString["v"];
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// This method is used to check video Url.
    /// </summary>
    /// <param name="asUrl"></param>
    /// <returns></returns>
    private bool IsValidVideoUrl(string asUrl)
    {
        bool bIsValid = false;
        if (asUrl != string.Empty)
        {
            int iLastIndex = asUrl.LastIndexOf('?');
            if (iLastIndex > 0)
            {
                string sQueryString = asUrl.Substring(asUrl.LastIndexOf('?') + 1);
                if (sQueryString.Length > 2 && sQueryString.Contains("v="))
                    bIsValid = true;
            }
        }
        return bIsValid;
    }

    /// <summary>
    /// This method is used to fill controls on click of video gallery gridview row.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void FillVideoGalleryDetails(int iRowIndex, bool bAddMoreSubjects)
    {
        FillSubjectCombo();        
        FillApplicableRoles();

        int iSubjectId = grdVideoGallery.DataKeys[iRowIndex]["SubjectId"].ToInt();
        int iUrlSourceId = grdVideoGallery.DataKeys[iRowIndex]["UrlSourceId"].ToInt();
        VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
        VideoDetails oVideoDetails = new VideoDetails();
        oVideoDetails = oVideoGalleryBL.GetVideoDetailsForEdit(miSchoolId, hidVedioId.Value.ToInt(), iSubjectId,iUrlSourceId);
        string sUserRoleIds = oVideoDetails.UserRoleIds;
        string sStandardDivIds = oVideoDetails.StandardDivisionIds;
        
        string[] sUserRoles = sUserRoleIds.Split(',');
        string[] sStdDiv = sStandardDivIds.Split(',');

        for (int iItemCount = 0; iItemCount < chkUserRoleLst.Items.Count; iItemCount++)
            chkUserRoleLst.Items[iItemCount].Selected = false;

        foreach (var iUserRoleId in sUserRoles)
            chkUserRoleLst.Items.FindByValue(iUserRoleId.ToString().Trim()).Selected = true;

        FillClassDetailsForVideo(sStdDiv);

        if (oVideoDetails.StartDate.ToDateTime().ToString() != Constants.S_DEFAULT_DATE_4)
            txtStartDate.Text = oVideoDetails.StartDate.ToString(Constants.S_DATE_FORMAT);

        if (oVideoDetails.EndDate.ToDateTime().ToString() != Constants.S_DEFAULT_DATE_4)
            txtEndDate.Text = oVideoDetails.EndDate.ToString(Constants.S_DATE_FORMAT);

        cmbSubject.SelectedValue = oVideoDetails.SubjectId.ToString();
        ddlUrlSource.SelectedValue = oVideoDetails.UrlSourceId.ToString();//////////////////////////////add new line
        hidOldSubjectId.Value = oVideoDetails.SubjectId.ToString();
        chkShowOnExternal.Checked = oVideoDetails.ShowOnExternalWebsite;
        ddlUrlSource.SelectedValue = oVideoDetails.UrlSourceId.ToString();

        txtVideoName.Text = HttpUtility.HtmlDecode(grdVideoGallery.Rows[iRowIndex].Cells[I_VIDEO_NAME_COLUMNID].Text).Trim();        
        btnVideoAdd.Text = S_BTN_UPDATE_TEXT;

        if (bAddMoreSubjects)
            DisableVideoControls(false);
    }

    private void DisableVideoControls(bool bEnabled)
    {
        btnVideoAdd.Text = S_BTN_ADD_TEXT;
        txtVideoName.Enabled = bEnabled;
        txtStartDate.Enabled = bEnabled;
        cal_StartDate.Enabled = bEnabled;
        txtEndDate.Enabled = bEnabled;
        cal_EndDate.Enabled = bEnabled;
        chkAllForVideo.Enabled = bEnabled;
        chkUserRoleLst.Enabled = bEnabled;
        lstvwVideoStandardDivision.Enabled = bEnabled;
        chkAllDivForVdo.Enabled = bEnabled;

        if (Settings.IsAaryanSchool)
            ddlUrlSource.Enabled = bEnabled;
        else
            ddlUrlSource.Enabled = false;

        cmbSubject.ClearSelection();
        hidOldSubjectId.Value = "-1";
    }

    /// <summary>
    /// This method is used to clear video gallery details.
    /// </summary>
    private void ClearVideoGalleryDetails(bool abClearAll)
    {
        if (abClearAll)
        {
            txtVideoName.Text = string.Empty;
            hidVedioId.Value = Constants.S_ZERO;
        }

        txtVidoComment1.Text = string.Empty;
        txtVidoComment2.Text = string.Empty;
        txtVidoComment3.Text = string.Empty;
        txtVidoComment4.Text = string.Empty;
        txtVidoComment5.Text = string.Empty;
        txtVideoUrl1.Text = string.Empty;
        txtVideoUrl2.Text = string.Empty;
        txtVideoUrl3.Text = string.Empty;
        txtVideoUrl4.Text = string.Empty;
        txtVideoUrl5.Text = string.Empty;

        //txtVideoUrl.Text = string.Empty;
        if (btnVideoAdd.Text == S_BTN_UPDATE_TEXT)
            btnVideoAdd.Text = S_BTN_ADD_TEXT;

        tblMoreVideoUpload.Visible = true;
        txtStartDate.Text = string.Empty;
        txtEndDate.Text = string.Empty;
        cmbSubject.ClearSelection();
       
        SetVideoURlSource();

        hidOldSubjectId.Value = "-1";
        ResetVideoGalleryClasses();
        SetDefaultValuesToUserRole();

        DisableVideoControls(true);
    }

    private void SetDefaultValuesToUserRole()
    {
        for (int iItemCount = 0; iItemCount < chkUserRoleLst.Items.Count; iItemCount++)
            chkUserRoleLst.Items[iItemCount].Selected = true;
    }

    /// <summary>
    /// Tis method is used to fill video gridview.
    /// </summary>
    private void FillVideoGallery()
    {
        grdVideoGallery.DataSourceID = ObjectDSVideoGallery.ID;

        if (!Settings.IsAaryanSchool)
        {
            grdVideoGallery.Columns[1].Visible = false;
            grdVideoGallery.Columns[3].Visible = false;
            grdVideoGallery.Columns[4].Visible = false;
            grdVideoGallery.Columns[5].Visible = false;

        }        
    }

    /// <summary>
    /// This method is used to add attributes on view and delete button of gridview.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetRowData(GridViewRow gridViewRow)
    {
        int iRowIndex = gridViewRow.RowIndex;
        if (iRowIndex >= 0)
        {
            //string sVideoUrl = grdVideoGallery.DataKeys[iRowIndex][S_VIDEO_URL].ToString();
            int iVideoId = grdVideoGallery.DataKeys[iRowIndex]["Video_Id"].ToInt();
            int iSubjectId = grdVideoGallery.DataKeys[iRowIndex]["SubjectId"].ToInt();
            int iUrlSourceId = grdVideoGallery.DataKeys[iRowIndex]["UrlSourceId"].ToInt();
            string sSubjectName = grdVideoGallery.DataKeys[iRowIndex]["Subject_Name"].ToString();
          string sURLSource = grdVideoGallery.DataKeys[iRowIndex]["URLSource"].ToString();
            SetVideoViewButtonAttribute(gridViewRow, iVideoId, iSubjectId,iUrlSourceId,sURLSource, sSubjectName);

            //HtmlAnchor aView = (HtmlAnchor)gridViewRow.FindControl("aView");
            //aView.HRef = sVideoUrl;

            ImageButton oDeleteMessage = (ImageButton)gridViewRow.FindControl("btnDeleteVideo");
            oDeleteMessage.Attributes.Add("Onclick", "if(!ConfirmVideoDelete()){return false;}");
        }
    }

    /// <summary>
    /// This method is used to validate gallery name and url.
    /// </summary>
    /// <param name="oVideoGalleryBL"></param>
    /// <returns></returns>
    private string GetValidityMassage(VideoGalleryBL oVideoGalleryBL)
    {
        string sMassage = "Valid";
        if (oVideoGalleryBL.IsDuplicateVideoName(btnVideoAdd.Text, hidVedioId.Value.ToInt()))
            sMassage = "Video name already exists.";
        return sMassage;
    }

    public void FillApplicableRoles()
    {
        NoticeBoardBL oNoticeBoardBL = new NoticeBoardBL();
        DataTable oDTRole = oNoticeBoardBL.RetriveRolesFromUserRoleMaster();
        
        if (!Settings.EnableOtherStaffLogin)
        {
            DataRow[] dr = oDTRole.Select("User_Role_Id=" + Constants.UserRoles.OtherStaff.ToInt());
            if (dr.Length > 0)
            {
                dr[0].Delete();
                oDTRole.AcceptChanges();
            }
        }

        chkUserRoleLst.DataSource = oDTRole;
        chkUserRoleLst.DataTextField = "User_Role_Name";
        chkUserRoleLst.DataValueField = "User_Role_Id";
        chkUserRoleLst.DataBind();
    }

    #endregion Video Galary Method(s)

    #endregion

    #region Common Methods

    /// <summary>
    /// This method is used to set sort image.
    /// </summary>
    private void DisplySortImage(GridView aoGridView, string asSortExpression, string asSortDirection)
    {
        int iSortColIndex=CommonUtility.GetSortColumnIndex(aoGridView,asSortExpression);
        if (aoGridView.Rows.Count > 0 && iSortColIndex!=-1)
            CommonUtility.AddSortImage(iSortColIndex, aoGridView.HeaderRow, asSortDirection);
    }

    /// <summary>
    /// This method is used to change sort direction.
    /// </summary>
    /// <param name="ahSortExpression"></param>
    /// <param name="ahSortDirection"></param>
    /// <param name="e"></param>
    private void SetSortDetails(HiddenField ahSortExpression, HiddenField ahSortDirection, GridViewSortEventArgs e)
    {
        if (!ahSortExpression.Value.Contains(e.SortExpression))
            ahSortDirection.Value = Constants.S_ASCENDING;
        else
            SetSortDirection(ahSortDirection);
        ahSortExpression.Value = e.SortExpression;
        ahSortExpression.Value = ahSortExpression.Value + " " + ahSortDirection.Value;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ahSortDirection"></param>
    private void SetSortDirection(HiddenField ahSortDirection)
    {
        ahSortDirection.Value = ahSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }
    #endregion

    /// <summary>
    /// This event is used to set class data to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVideoStandardDivision_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkVdoStandard = oCurrentItem.FindControl("chkVdoStandard") as CheckBox;
                CheckBoxList chkVideoStandardDivLst = oCurrentItem.FindControl("chkvideoStandardDivLst") as CheckBoxList;
                int iStandardId = lstvwVideoStandardDivision.DataKeys[iRowId]["StandardId"].ToInt();                
                var oList = mlstStandardDivisions.Where(sd => sd.StandardId == iStandardId).OrderBy(sd => sd.OriginalStandardId).ThenBy(sd => sd.StandardDivisionId).Select(sd => new { sd.StandardDivisionId, sd.DivisionName }).ToList();
                ListSource.FillCheckBoxList(oList, chkVideoStandardDivLst, "DivisionName", "StandardDivisionId");

                chkVdoStandard.Attributes.Add("onclick", "CheckAllForVideo(this,'" + iRowId + "')");
                chkVideoStandardDivLst.Attributes.Add("onclick", "CheckStdForVideo('" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to reset classes.
    /// </summary>
    private void ResetVideoGalleryClasses()
    {
        foreach (var item in lstvwVideoStandardDivision.Items)
        {
            CheckBox chkVdoStandard = item.FindControl("chkVdoStandard") as CheckBox;
            chkVdoStandard.Checked = false;

            CheckBoxList chkStandardDivLst = item.FindControl("chkvideoStandardDivLst") as CheckBoxList;
            for (int iItemCount = 0; iItemCount < chkStandardDivLst.Items.Count; iItemCount++)
                chkStandardDivLst.Items[iItemCount].Selected = false;
        }

        chkAllDivForVdo.Checked = false;
    }

    /// <summary>
    /// This method is used to fill subject combobox.
    /// </summary>
    private void FillSubjectCombo()
    { 
        VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
        DataTable dtSubjects = oVideoGalleryBL.GetAllSubjectsForVideoGallery(miSchoolId, miAcademicYearId);

        cmbSubject.Bind(dtSubjects, "Value_Member", "Display_Member", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to get classes for Saving in database.
    /// </summary>
    /// <returns></returns>
    private string GetClassesForVideo()
    {
        StringBuilder oStandards = new StringBuilder();
        foreach (ListViewDataItem Item in lstvwVideoStandardDivision.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkvideoStandardDivLst") as CheckBoxList;
            for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
            {
                if (chkStandardDivLst.Items[iCount].Selected)
                    oStandards.Append("," + chkStandardDivLst.Items[iCount].Value);
            }
        }

        string sIds = string.Empty;
        if (oStandards.ToString().Length > 0)
            sIds = oStandards.ToString().Substring(1);
        return sIds;
    }

    /// <summary>
    /// This method is used to Fill the class details for video gallery.
    /// </summary>
    /// <param name="oImageGalleryBL"></param>
    private void FillClassDetailsForVideo(string[] sStandardDivs)
    {  
        int iStdCount = 0;

        foreach (var item in lstvwVideoStandardDivision.Items)
        {
            CheckBox chkVideoStandard = item.FindControl("chkVdoStandard") as CheckBox;
            CheckBoxList chkvideoStandardDivLst = item.FindControl("chkvideoStandardDivLst") as CheckBoxList;
            int iCnt = 0;
            for (int iItemCount = 0; iItemCount < chkvideoStandardDivLst.Items.Count; iItemCount++)
            {
                if (sStandardDivs.Contains(chkvideoStandardDivLst.Items[iItemCount].Value.ToString()))
                {
                    chkvideoStandardDivLst.Items[iItemCount].Selected = true;
                    iCnt++;
                }
                else
                    chkvideoStandardDivLst.Items[iItemCount].Selected = false;
            }

            if (iCnt == chkvideoStandardDivLst.Items.Count)
            {
                chkVideoStandard.Checked = true;
                iStdCount++;
            }
            else
                chkVideoStandard.Checked = false;
        }
        chkAllDivs.Checked = iStdCount == lstvwStandardDivisions.Items.Count;
    }   
}