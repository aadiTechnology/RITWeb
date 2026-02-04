/* File Name = UpLoadNoticesUI.aspx.cs
 * Created Date - 14 December 2011
 * Created by - Poonam
 * Class Description - This class is defined to manage Notice Details.
 * Modified By: Rohini
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using DataCommunicator;
using MasterEntities;
using PushNotificationService;
using SchoolEntities;
using Utility;

/// <summary>
/// This class is used to add, edit or delete school notice.
/// </summary>
public partial class UploadNoticesUI :SchoolBase
{
    #region "Constants"

    private const string S_DEFAULT_SORT_EXP = "StartDate";
    private const string S_SAVE_STATEMENT = "Notice saved successfully !!!";
    private const string S_UPDATE_STATEMENT = "Notice updated successfully !!!";
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\School Notices\\";
    private const string S_COMMAND_UPDATE_NOTICE = "UpdateNotice";
    private const string S_COMMAND_DELETE_NOTICE = "DeleteNotice";
    private const string S_VIEW = "View";
    private const string S_ADD_UPDATE = "AddUpdate";
    private const string S_DUPLICATE_LINK_NAME = "Notice name already exists.";
    private const string S_FILE_NOT_FOUND = "File does not Exists";
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";
    private const int I_FILE_SIZE_LIMIT = 10485760;  // File limit is 10 MB
    private const string S_DELETE_MSG = "Notice deleted successfully !!!";
    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_FILE_EXIST = "File already exists.";
    private const string S_CHAR_A = "A";
    private const string S_END_TIME = "11:59 PM";
    private const string S_START_TIME = "12:00 AM";
    private const string S_SAVE_SELECTED_NOTICE = "Selected notices(s) saved successfully !!!";
    private const string S_BLANK_MSG = "Notice content should not be blank.";
    private const string S_SORT = "SortRow";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/School Notices/";    
    #endregion "Constants"

    #region Property

    private int iUsersCount { get; set; }

    #endregion

    #region Data Members

    List<StandardDivisionMaster> mlstStandardDivisionMaster = new List<StandardDivisionMaster>();

    #endregion


    #region "Events"

    /// <summary>
    /// This event is used to set default control fields and java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
			    base.SetDocType();
                FillApplicableRoles();
                trDtPgCount.Visible = true;
                SetJavaScriptAttributes();
                FillDisplayLocationComboboxes();
                DisableRoles(true);
                InitailizeHiddenField();
                FillStandardChkLstBox();
                FillNoticeDetailsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add the sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add Sort Image
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update notice details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateNoticeName())
            {
                lblErrorMsg.Text = S_DUPLICATE_LINK_NAME;
                FillNoticeDetailsListView();
            }
            else
            {
                string sErrorMsg = SaveNoticeDetails();
                if (string.IsNullOrEmpty(sErrorMsg))
                {
                    SetSaveBtnText();
                    ResetFields();
                    FillNoticeDetailsListView();                   
                }
                else
                    lblErrorMsg.Text = sErrorMsg;
            }

            cstFileNameValidation.Enabled = true;
        }
        catch (FileNotFoundException oEx)
        {
            lblErrorMsg.Text = oEx.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used while loading rows in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                if (oCurrentItem != null)
                    SetVisibilityOfColumns(oCurrentItem);
            }

            DataPager dtPager = lstvwNoticeDetails.FindControl("DtPgDropDown") as DataPager;
            if (lstvwNoticeDetails.Items.Count == 0)
                if(dtPager!=null)
                dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called while row in list view is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iNoticeId = Convert.ToInt32(lstvwNoticeDetails.DataKeys[iRowId]["NoticeId"]);
                hidNoticeImageId.Value = iNoticeId.ToString();
                string sDescription = Convert.ToString(lstvwNoticeDetails.DataKeys[iRowId]["NoticeDescription"]);
                string sImageName = Convert.ToString(lstvwNoticeDetails.DataKeys[iRowId]["NoticeImage"]);
                hidRowNo.Value = (oCurrentItem.DisplayIndex + 1).ToString();
                if (e.CommandName == S_COMMAND_UPDATE_NOTICE)
                {
                    SetEditMode(oCurrentItem, iNoticeId, sDescription, sImageName);
                }
                else if (e.CommandName == S_COMMAND_DELETE_NOTICE)
                {
                    SetDeleteMode(oCurrentItem, iNoticeId);
                    DataPager dtPager = lstvwNoticeDetails.FindControl("DtPgDropDown") as DataPager;
                   
                    FillNoticeDetailsListView();

                    if (dtPager.StartRowIndex != 0 && lstvwNoticeDetails.Items.Count== Constants.I_ONE)
                    {
                        if(dtPager!=null)
                        dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                        DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                    }
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT)
            {
                SetSortVariables();
                hidSortExpression.Value = e.CommandArgument.ToString();
                DataPager dtPager = lstvwNoticeDetails.FindControl("DtPgDropDown") as DataPager;
                if(dtPager!=null)
                dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                FillNoticeDetailsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called while cancelling update operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
            hidSortDirection.Value = Constants.S_DESCENDING;
            ResetFields();
            FillNoticeDetailsListView();
            hidCurrentOperation.Value = S_VIEW;
            cstFileNameValidation.Enabled = true;
            btnUpdate.Text = S_TEXT_SAVE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called when filter criteria of display location is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDisplayLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (hidCurrentOperation.Value != S_ADD_UPDATE)
                FillNoticeDetailsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called when filter criteria  is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optNotices_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (hidCurrentOperation.Value != S_ADD_UPDATE)
                FillNoticeDetailsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise Notices.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwNoticeDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called once while loading listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwNoticeDetails.Items.Count.ToString();
            if (lstvwNoticeDetails.Items.Count > Constants.I_ZERO)
            {
                lstvwNoticeDetails.Items.Clear();
                ControlUtility.FillListViewPagerFooter(lstvwNoticeDetails, DtPgCount);
                trDtPgCount.Visible = true;
                if (DtPgCount.TotalRowCount > DtPgCount.PageSize)
                    DtPgCount.Visible = true;
            }
            else
            {
                DtPgCount.SetPageProperties(0, 20, true);
                DtPgCount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called once while sorting listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save selected school notices which is displayed on school notices.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveSelected_Click(object sender, EventArgs e)
    {
        try
        {
            List<NoticeDetails> lstNoticeId = new List<NoticeDetails>();
            for (int iCnt = 0; iCnt < lstvwNoticeDetails.Items.Count; iCnt++)
            {
                NoticeDetails oNoticeDetails = new NoticeDetails();
                CheckBox chkSelect = lstvwNoticeDetails.Items[iCnt].FindControl("chkSelect") as CheckBox;
                oNoticeDetails.IsSelected = chkSelect.Checked;
                oNoticeDetails.NoticeId = Convert.ToInt32(lstvwNoticeDetails.DataKeys[iCnt]["NoticeId"]);
                oNoticeDetails.InertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
                lstNoticeId.Add(oNoticeDetails);
            }

            string sXml = CommonUtility.GenerateXml(lstNoticeId);
            NoticeDetailsBL.SaveSelectedNotices(sXml);
            lblUpdateSucess.Text = S_SAVE_SELECTED_NOTICE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind data to lstvwStandardDivisions list view
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
                var oDivision = mlstStandardDivisionMaster.Where(sd => sd.StandardId == iStandardId).Select(sd => new { DivisionName = sd.DivisionName, Id = sd.StandardDivisionId });
                chkStandardDivLst.DataSource = oDivision;
                chkStandardDivLst.DataTextField = "DivisionName";
                chkStandardDivLst.DataValueField = "Id";
                chkStandardDivLst.DataBind();
                chkStandard.Attributes.Add("onclick", "CheckAll(this,'" + iRowId + "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckAllCheck('" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind data to lstvwStandardDivisionsText list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStandardDivisionsText_ItemDataBound(object sender, ListViewItemEventArgs e)
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
                var oDivision = mlstStandardDivisionMaster.Where(sd => sd.StandardId == iStandardId).Select(sd => new { DivisionName = sd.DivisionName, Id = sd.StandardDivisionId });
                chkStandardDivLst.DataSource = oDivision;
                chkStandardDivLst.DataTextField = "DivisionName";
                chkStandardDivLst.DataValueField = "Id";
                chkStandardDivLst.DataBind();
                chkStandard.Attributes.Add("onclick", "CheckAllForText(this,'" + iRowId + "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckAllCheckForText('" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region " Text notice events"
    /// <summary>
    /// This method is used to save text notices.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveText_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateNoticeName())
            {
                lblErrorMsg.Text = S_DUPLICATE_LINK_NAME;
                FillNoticeDetailsListView();
            }
            else
            {
                FCKNoticeContent.ReadOnly = false;

                string sFileName = string.Empty;
                string sErrorMsg = string.Empty;
                if (FilTextNoticeUpload.FileName != string.Empty)
                {                    
                    if (FilTextNoticeUpload.HasFile)
                    {
                        CheckImageIsValid(ref sErrorMsg, out sFileName, FilTextNoticeUpload);
                    }
                }
                else
                    sFileName = hidNoticeImage.Value;

                if (sErrorMsg == string.Empty)
                {
                    string sPlainText = StripHTML(FCKNoticeContent.Text);
                    sPlainText = sPlainText.Replace("\r\r", string.Empty).Trim();
                    if (!string.IsNullOrEmpty(sPlainText.Trim()))
                    {
                        NoticeDetails oNoticeDetails = new NoticeDetails();
                        oNoticeDetails.NoticeName = txtNoticeName.Text.Trim();
                        oNoticeDetails.FileName = string.Empty;
                        oNoticeDetails.NoticeContent = HttpUtility.HtmlEncode(FCKNoticeContent.Text);
                        oNoticeDetails.NoticeId = string.IsNullOrEmpty(hidNoticeIdText.Value) ? 0 : hidNoticeIdText.Value.ToInt();
                        oNoticeDetails.SchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
                        oNoticeDetails.AcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();
                        oNoticeDetails.DisplayLocation = cmbDisplayLocationTextNotice.SelectedValue;
                        oNoticeDetails.SortOrder = txtSortOrderTextNotice.Text.ToInt();
                        txtStartTimeTextNotice.Text = string.IsNullOrEmpty(txtStartTimeTextNotice.Text) ? S_START_TIME : txtStartTimeTextNotice.Text;
                        txtEndDateTextNotice.Text = string.IsNullOrEmpty(txtEndTimeTextNotice.Text) ? S_END_TIME : txtEndDateTextNotice.Text;
                        oNoticeDetails.StartDate = Convert.ToDateTime(txtStartDateTextNotice.Text + ' ' + txtStartTimeTextNotice.Text).ToString();
                        oNoticeDetails.EndDate = Convert.ToDateTime(txtEndDateTextNotice.Text + ' ' + txtEndTimeTextNotice.Text).ToString();
                        oNoticeDetails.InertedById = Session[Constants.S_SESSION_USER_ID].ToInt();
                        oNoticeDetails.IsSelected = true;
                        oNoticeDetails.IsText = true;
                        oNoticeDetails.NoticeDescription = txtTextNoticeDescription.Text;
                        oNoticeDetails.NoticeImage = sFileName;
                        string sUserRoleIds = string.Join(",", GetSelectedRolesText());
                        string sXml = CommonUtility.GenerateXml(oNoticeDetails);

                        string sSelectedClassIds = GetStandardStrLstForText();

                        NoticeDetailsBL.Update(sXml, sUserRoleIds, sSelectedClassIds);
                        if (btnSaveText.Text == S_TEXT_SAVE)
                            lblUpdateSucess.Text = S_SAVE_STATEMENT;
                        else
                        {
                            lblUpdateSucess.Text = S_UPDATE_STATEMENT;
                            btnSaveText.Text = S_TEXT_SAVE;
                        }

                        hidStandardDivIds.Value = sSelectedClassIds;
                        SendPushNotification(sUserRoleIds, oNoticeDetails.NoticeName);
                        ClearTextNoticeControls();
                    }
                    else
                        lblErrorMsg.Text = S_BLANK_MSG;
                }
                else
                    lblErrorMsg.Text = sErrorMsg;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelText_Click(object sender, EventArgs e)
    {
        try
        {
            ClearTextNoticeControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to link notices.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optLink_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            trDtPgCount.Visible = true;
            DtPgCount.Attributes.Add("visibility", "visible");
            //vdocument.getElementById(_ClientDtPgCount).style.visibility = "visible";

            DtPgCount.Visible = true;
            btnSaveSelected.Attributes.Add("onclick", "if(!SelectedCount(0)){return false;}");
            FillNoticeDetailsListView();
            SetVisibility(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to display text notices
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optText_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetVisibility(true);
            ClearTextNoticeControls();
            btnSaveSelected.Attributes.Add("onclick", "if(!SelectedCount(0)){return false;}");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is fired while Display location combo box selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDisplayLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetFieldState(cmbDisplayLocation);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is fired while Display location combo box selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDisplayLocationTextNotice_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetFieldState(cmbDisplayLocationTextNotice);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set field state.
    /// </summary>
    /// <param name="aoDropDownList"></param>
    private void SetFieldState(DropDownList aoDropDownList)
    {
        if (aoDropDownList.SelectedValue == Convert.ToChar(Constants.NoticeDisplayLocation.Home_Page).ToString())
        {
            ClearRoles();
            SelectAllRoles(true);
            DisableRoles(false);
        }
        else
        {
            SelectAllRoles(false);
            DisableRoles(true);
        }
    }

    /// <summary>
    /// This event is fired when Notice image will be deleted.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {
            NoticeDetailsBL oNoticeDetailsBL = new NoticeDetailsBL();
            oNoticeDetailsBL.DeleteNoticeImage(hidNoticeImageId.Value.ToInt(), 0);            
            hidNoticeImage.Value = null;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is fired when text Notice image will be deleted.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnTxtDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {
            NoticeDetailsBL oNoticeDetailsBL = new NoticeDetailsBL();
            oNoticeDetailsBL.DeleteNoticeImage(hidNoticeImageId.Value.ToInt(), 1);            
            hidNoticeImage.Value = null;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method is used to fill standard check box list.
    /// </summary>
    private void FillStandardChkLstBox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        mlstStandardDivisionMaster = oStandardCollectionBL.GetAllClasses();
        var oStandards = mlstStandardDivisionMaster.Select(sd => new { StandardName = sd.StandardName, StandardId = sd.StandardId }).Distinct();
       
        lstvwStandardDivisions.DataSource = oStandards;
        lstvwStandardDivisions.DataBind();

        lstvwStandardDivisionsText.DataSource = oStandards;
        lstvwStandardDivisionsText.DataBind();
    }

    /// <summary>
    /// This method is used select all checkboxes.
    /// </summary>
    private void SelectAllRoles(bool abFlag)
    {
        if (optLink.Checked)
        {
            chkAll.Checked = abFlag;
            int iTotalRoles = chkListRoles.Items.Count;
            for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
                chkListRoles.Items[iListIndex].Selected = abFlag;
        }
        else
        {
            chkAllText.Checked = abFlag;
            int iTotalRoles = chkListRolesText.Items.Count;
            for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
                chkListRolesText.Items[iListIndex].Selected = abFlag;
        }
    }

    /// <summary>
    /// This method is used to hide or show filename column in listview.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void SetVisibilityOfColumns(ListViewDataItem oCurrentItem)
    {
        HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trItemtemplate") as HtmlTableRow;
        HtmlTableCell oHtmlTableCellHeader = lstvwNoticeDetails.FindControl("thFileName") as HtmlTableCell;
        HtmlTableCell oHtmlTableCellFileName = oHtmlTableRow.FindControl("tdFileName") as HtmlTableCell;
        CheckBox oChkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
        ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
        Label lblStartDate = oCurrentItem.FindControl("lblStartDt") as Label;
        DateTime dtStartDate = Convert.ToDateTime(lblStartDate.Text);
        HyperLink lnkFileName = oCurrentItem.FindControl("lnkBtnFileName") as HyperLink;
        DateTime dtCurrentDate = System.DateTime.Now;
        DateTime dtEndDate;
        Label lblEndDate = oCurrentItem.FindControl("lblEndDt") as Label;

        oHtmlTableCellHeader.Visible = !optText.Checked;
        oHtmlTableCellFileName.Visible = !optText.Checked;
        dtEndDate = !string.IsNullOrEmpty(lblEndDate.Text) ? dtEndDate = Convert.ToDateTime(lblEndDate.Text) : dtEndDate = Convert.ToDateTime(Constants.S_DEFAULT_DATE1);  // Seting to default value just to avoid Exception
        lblStartDate.Text = dtStartDate.ToString(Constants.S_DATE_FORMAT) + ' ' + dtStartDate.ToShortTimeString();
        lblEndDate.Text = dtEndDate.ToString(Constants.S_DATE_FORMAT) + ' ' + dtEndDate.ToShortTimeString();
        oChkSelect.Checked = Convert.ToBoolean(lstvwNoticeDetails.DataKeys[oCurrentItem.DisplayIndex]["IsSelected"]);
        if (lnkFileName != null)
        {
            lnkFileName.NavigateUrl = "../downloads/School Notices/" + ((NoticeDetails)oCurrentItem.DataItem).FileName.ToString();
            lnkFileName.Attributes.Add("onclick", "window.open('" + lnkFileName.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
        }

        if (((lblEndDate.Text == string.Empty) || (dtEndDate >= dtCurrentDate)) && (dtStartDate <= dtCurrentDate)) // Active Notice
            imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmActiveDelete()) {return false;}");
        else
            imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");
    }

    /// <summary>
    /// This method is used to delete a notice.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="iNoticeId"></param>
    private void SetDeleteMode(ListViewDataItem oCurrentItem, int iNoticeId)
    {
        if (optLink.Checked)
        {
            HyperLink oLblFileName = oCurrentItem.FindControl("lnkBtnFileName") as HyperLink;
            hidFileName.Value = oLblFileName.Text;
            DeleteNoticeDetails(iNoticeId);
            ResetFields();
        }
        else
        {
            NoticeDetailsBL.Delete(miSchoolId, iNoticeId, miUserId);
            hidCurrentOperation.Value = S_VIEW;
            lblUpdateSucess.Text = S_DELETE_MSG;
           
        }
    }

    /// <summary>
    /// This method is used to edit a notice.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="iNoticeId"></param>
    private void SetEditMode(ListViewDataItem oCurrentItem, int iNoticeId, string asDescription, string asImageNAme)
    {
        if (optLink.Checked)
        {
            SetControlsForEditMode(oCurrentItem, asDescription, asImageNAme);
            FillNoticeDetailsListView();
        }
        else
        {
            SetEditModeForTextNotice(iNoticeId);
            FillNoticeDetailsListView();
        }
    }

    /// <summary>
    /// This method is used to reset the controls for text notice.
    /// </summary>
    private void ClearTextNoticeControls()
    {
        hidMode.Value = Constants.S_NEW_MODE;
        hidNoticeIdText.Value = string.Empty;
        txtEndDateTextNotice.Text = string.Empty;
        txtEndTimeTextNotice.Text = string.Empty;
        txtNoticeName.Text = string.Empty;
        txtSortOrderTextNotice.Text = string.Empty;
        txtStartDateTextNotice.Text = string.Empty;
        txtStartTimeTextNotice.Text = string.Empty;
        FCKNoticeContent.Text = string.Empty;
        hidSortDirection.Value = string.Empty;
        hidSortExpression.Value = string.Empty;
        hidNoticeImage.Value = string.Empty;
        cmbDisplayLocationTextNotice.ClearSelection();
        ddlDisplayLocation.ClearSelection();
        optActiveNotices.Checked = false;
        optAllNotices.Checked = true;
        txtStartTimeTextNotice.Text = S_START_TIME;
        txtEndTimeTextNotice.Text = S_END_TIME;
        btnSaveText.Text = S_TEXT_SAVE;
        ClearRoles();
        chkAllText.Checked = false;
        txtDescription.Text = string.Empty;
        txtTextNoticeDescription.Text = string.Empty;
        DataPager dtPager = lstvwNoticeDetails.FindControl("DtPgDropDown") as DataPager;
        if(dtPager!=null)
        dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, true);

        foreach (ListViewDataItem Item in lstvwStandardDivisionsText.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;

            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)
                chkStandardDivLst.Items[iStandardIndex].Selected = false;

            chkStandard.Checked = false;
        }
    }

    /// <summary>
    /// This method is used to set values to controls in edit mode.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void SetControlsForEditMode(ListViewDataItem oCurrentItem, string asDescription, string asNoticeImage)
    {
        btnUpdate.Text = S_TEXT_UPDATE;
        hidCurrentOperation.Value = S_ADD_UPDATE;
        cstFileNameValidation.Enabled = false;
        Label oLblLinkName = oCurrentItem.FindControl("lblLinkName") as Label;
        Label oLblDisplayLocation = oCurrentItem.FindControl("lblDisplayLocation") as Label;
        Label oLblStartDt = oCurrentItem.FindControl("lblStartDt") as Label;
        Label oLblEndDt = oCurrentItem.FindControl("lblEndDt") as Label;
        Label oLblSortOrder = oCurrentItem.FindControl("lblSortOrder") as Label;
        HyperLink oLblFileName = oCurrentItem.FindControl("lnkBtnFileName") as HyperLink;
        txtLinkName.Text = oLblLinkName.Text;
        txtCalEndDtPopup.Text = Convert.ToDateTime(oLblEndDt.Text).ToString("dd-MMM-yyyy");
        txtCalStartDtPopup.Text = Convert.ToDateTime(oLblStartDt.Text).ToString("dd-MMM-yyyy");
        txtSortOrder.Text = oLblSortOrder.Text;
        txtStartTime.Text = Convert.ToDateTime(oLblStartDt.Text).ToShortTimeString();
        txtEndTime.Text = Convert.ToDateTime(oLblEndDt.Text).ToShortTimeString();
        txtDescription.Text = asDescription;
        if (asNoticeImage != string.Empty)
        {
            imgbtnDelete.Visible = true;
            btnView.Visible = true;

            hidNoticeImage.Value = asNoticeImage;
            string sNewFileName = S_FOLDER_PATH + asNoticeImage;
            btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
        }
        else
        {
            imgbtnDelete.Visible = false;
            btnView.Visible = false;
        }
      
        Constants.NoticeDisplayLocation location = Constants.NoticeDisplayLocation.Both;
        oLblDisplayLocation.Text = oLblDisplayLocation.Text.Trim().Replace(" ", "_");
        if (oLblDisplayLocation.Text == Constants.NoticeDisplayLocation.Control_Panel.ToString())
            location = Constants.NoticeDisplayLocation.Control_Panel;
        else if (oLblDisplayLocation.Text == Constants.NoticeDisplayLocation.Home_Page.ToString())
            location = Constants.NoticeDisplayLocation.Home_Page;
        cmbDisplayLocation.SelectedValue = Convert.ToChar(location).ToString();

        hidFileName.Value = oLblFileName.Text;
        hidNoticeId.Value = Convert.ToString(lstvwNoticeDetails.DataKeys[oCurrentItem.DisplayIndex]["NoticeId"]);
        hidSortOrder.Value = lstvwNoticeDetails.DataKeys[oCurrentItem.DisplayIndex]["SortOrder"].ToString();
        hidFileDisplayLocation.Value = lstvwNoticeDetails.DataKeys[oCurrentItem.DisplayIndex]["DisplayLocation"].ToString();

        ClearRoles();
            FetchRoles(Convert.ToInt32(lstvwNoticeDetails.DataKeys[oCurrentItem.DisplayIndex]["NoticeId"]));
            FetchStandardDivisions(Convert.ToInt32(lstvwNoticeDetails.DataKeys[oCurrentItem.DisplayIndex]["NoticeId"]));
    }


    /// <summary>
    /// This method is used to fetch roles from table MenusRoles according to MenuId.
    /// </summary>
    private void FetchRoles(int aiNoticeId)
    {
        int iItemCount, iRowCount, iRowIndex;
        NoticeDetailsBL oNoticeBoardBL = new NoticeDetailsBL();

        DataTable ODTRoles = oNoticeBoardBL.GetUserRolesForSelectedNoticeId(aiNoticeId);
        if (ODTRoles.Rows.Count > 0)
        {
            iItemCount = chkListRoles.Items.Count;
            iRowCount = ODTRoles.Rows.Count - 1;
            DataRow oDRRoles;
            iRowIndex = 0;
            for (int iIndex = 0; iIndex < iItemCount; iIndex++)
            {
                oDRRoles = ODTRoles.Rows[iRowIndex];
                if (chkListRoles.Items[iIndex].Value.ToString() == oDRRoles[0].ToString())
                {
                    chkListRoles.Items[iIndex].Selected = true;
                    if (iRowIndex < iRowCount)
                        iRowIndex++;
                }
                else
                    chkListRoles.Items[iIndex].Selected = false;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowClasses", "ShowClasses();", true);            
        }

    }
	/// <summary>
    /// This method is used to fetch Seleceted Standard_Division_Id for Normal.
    /// </summary>
    /// <param name="aiNoticeId"></param>
    private void FetchStandardDivisions(int aiNoticeId)
    {
        NoticeDetailsBL oNoticeBoardBL = new NoticeDetailsBL();

        List<string> sArrStandards = new List<string>();
        DataTable ODTClasses = oNoticeBoardBL.GetStandardDivisionsForSelectedNotice(aiNoticeId);

        for (int i = 0; i < ODTClasses.Rows.Count; i++)
            sArrStandards.Add(ODTClasses.Rows[i]["StandardDivisionId"].ToString());

        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;
            int iTotal = 0;
            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)
            {
                string sStandardId = chkStandardDivLst.Items[iStandardIndex].Value.ToString();
                if (sArrStandards.Contains(sStandardId))
                {
                    chkStandardDivLst.Items.FindByValue(sStandardId).Selected = true;
                    iTotal++;
                }
                else
                    chkStandardDivLst.Items.FindByValue(sStandardId).Selected = false;
            }

            if (iTotal == chkStandardDivLst.Items.Count)
                chkStandard.Checked = true;
            else
                chkStandard.Checked = false;
        }
    }

	/// <summary>
    /// This method is used to fetch Seleceted Standard_Division_Id for Text.
    /// </summary>
    /// <param name="aiNoticeId"></param>
    private void FetchStandardDivisionsForText(int aiNoticeId)
    {
        NoticeDetailsBL oNoticeBoardBL = new NoticeDetailsBL();

        List<string> sArrStandards = new List<string>();
        DataTable ODTClasses = oNoticeBoardBL.GetStandardDivisionsForSelectedNotice(aiNoticeId);
        
        for (int i = 0; i < ODTClasses.Rows.Count; i++)
            sArrStandards.Add(ODTClasses.Rows[i]["StandardDivisionId"].ToString());

        foreach (ListViewDataItem Item in lstvwStandardDivisionsText.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;
            int iTotal = 0;
            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)
            {
                string sStandardId = chkStandardDivLst.Items[iStandardIndex].Value.ToString();
                if (sArrStandards.Contains(sStandardId))
                {
                    chkStandardDivLst.Items.FindByValue(sStandardId).Selected = true;
                    iTotal++;
                }
                else
                    chkStandardDivLst.Items.FindByValue(sStandardId).Selected = false;
            }

            if (iTotal == chkStandardDivLst.Items.Count)
                chkStandard.Checked = true;
            else
                chkStandard.Checked = false;
        }
    }

    /// <summary>
    /// This method is used to fetch roles from table MenusRoles according to MenuId.
    /// </summary>
    private void FetchRolesText(int aiNoticeId)
    {
        int iItemCount, iRowCount, iRowIndex;
        NoticeDetailsBL oNoticeBoardBL = new NoticeDetailsBL();

        DataTable ODTRoles = oNoticeBoardBL.GetUserRolesForSelectedNoticeId(aiNoticeId);
        if (ODTRoles.Rows.Count > 0)
        {
            iItemCount = chkListRolesText.Items.Count;
            iRowCount = ODTRoles.Rows.Count - 1;
            DataRow oDRRoles;
            iRowIndex = 0;
            for (int iIndex = 0; iIndex < iItemCount; iIndex++)
            {
                oDRRoles = ODTRoles.Rows[iRowIndex];
                if (chkListRolesText.Items[iIndex].Value.ToString() == oDRRoles[0].ToString())
                {
                    chkListRolesText.Items[iIndex].Selected = true;
                    if (iRowIndex < iRowCount)
                        iRowIndex++;
                }
                else
                    chkListRolesText.Items[iIndex].Selected = false;
            }
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowClassesText", "ShowClassesText();", true);
    }
  

    /// <summary>
    /// This method is used to change save button text.
    /// </summary>
    private void SetSaveBtnText()
    {
        if (btnUpdate.Text == S_TEXT_UPDATE)
            lblUpdateSucess.Text = S_UPDATE_STATEMENT;
        else
            lblUpdateSucess.Text = S_SAVE_STATEMENT;
        hidCurrentOperation.Value = S_VIEW;
        btnUpdate.Text = S_TEXT_SAVE;
    }

    /// <summary>
    /// This method is used to assign values to hidden field.
    /// </summary>
    private void InitailizeHiddenField()
    {
        optAllNotices.Checked = true;
        tblLinkNoticeControls.Visible = true;
        tblTextNoticeControls.Visible = false;
        hidCurrentOperation.Value = S_VIEW;
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This Method checks for duplicate notice names 
    /// </summary>
    private bool ValidateNoticeName()
    {
        bool bIsValid = false;
        int iExistingNoticeId;
        string sNoticeName;
        string sStartDateTimeNotice;
        string sEndDateTimeNotice;
        if (optLink.Checked)
        {
            sStartDateTimeNotice = txtCalStartDtPopup.Text + " " + txtStartTime.Text;
            sEndDateTimeNotice = txtCalEndDtPopup.Text + " " + txtEndTime.Text;
            sNoticeName = txtLinkName.Text.Trim();
        }
        else
        {
            sStartDateTimeNotice = txtStartDateTextNotice.Text + " " + txtStartTimeTextNotice.Text;
            sEndDateTimeNotice = txtEndDateTextNotice.Text + " " + txtEndTimeTextNotice.Text;
            sNoticeName = txtNoticeName.Text.Trim();
        }


        iExistingNoticeId = NoticeDetailsBL.GetIDByName(miSchoolId, sNoticeName, sStartDateTimeNotice, sEndDateTimeNotice);
      
        if (optLink.Checked)
        {
            if (!string.IsNullOrEmpty(hidNoticeId.Value))
            {   // Update Operation for link notice
                int iNoticeId = Convert.ToInt32(hidNoticeId.Value);
                if ((iNoticeId == iExistingNoticeId) || (iExistingNoticeId == Constants.I_ZERO))
                    bIsValid = true;
            }
            else if (iExistingNoticeId == Constants.I_ZERO)    // ADD operation
                bIsValid = true;
        }
        else if (optText.Checked)
        { // // Update Operation for text notice 
            if (!string.IsNullOrEmpty(hidNoticeIdText.Value))
            {
                int iNoticeId = Convert.ToInt32(hidNoticeIdText.Value);
                if ((iNoticeId == iExistingNoticeId) || (iExistingNoticeId == Constants.I_ZERO))
                    bIsValid = true;
            }
            else if (iExistingNoticeId == Constants.I_ZERO)    // ADD operation
                bIsValid = true;
        }

        return bIsValid;
    }

    /// <summary>
    /// This Method Deletes perticular Notice.
    /// </summary>
    /// <param name="aiNoticeId"></param>
    private void DeleteNoticeDetails(int aiNoticeId)
    {
        string sServerPath = Server.MapPath("~");
        if (sServerPath.Substring(sServerPath.Length - 1) != "\\")

            sServerPath = sServerPath + "\\";

        // Check for File size
        if (hidFileName.Value != string.Empty)
        {
            if (File.Exists(sServerPath + S_FOLDER_LOCATION + hidFileName.Value))
                File.Delete(sServerPath + S_FOLDER_LOCATION + hidFileName.Value);
        }

        NoticeDetailsBL.Delete(miSchoolId, aiNoticeId, miUserId);
        cstFileNameValidation.Enabled = true;
        hidCurrentOperation.Value = S_VIEW;
        lblUpdateSucess.Text = S_DELETE_MSG;
       
    }

    /// <summary>
    /// This Method is used to fill Listview
    /// </summary>
    private void FillNoticeDetailsListView()
    {
        lstvwNoticeDetails.DataSourceID = ObjDSNoticeDetails.ID;
    }

    /// <summary>
    /// This Method is used to load comboboxe and set it to default values.
    /// </summary>
    private void FillDisplayLocationComboboxes()
    {
        ddlDisplayLocation.Items.Add(new ListItem(Constants.S_ALL, S_CHAR_A));
        foreach (var value in Enum.GetValues(typeof(Constants.NoticeDisplayLocation)))
        {
            string sName = value.ToString();
            cmbDisplayLocation.Items.Add(new ListItem(sName.Replace('_', ' '), Convert.ToChar(value).ToString()));
            ddlDisplayLocation.Items.Add(new ListItem(sName.Replace('_', ' '), Convert.ToChar(value).ToString()));
            ddlDisplayLocation.Text = Constants.NoticeDisplayLocation.Both.ToString();
            cmbDisplayLocationTextNotice.Items.Add(new ListItem(sName.Replace('_', ' '), Convert.ToChar(value).ToString()));
            cmbDisplayLocationTextNotice.Text = Constants.NoticeDisplayLocation.Both.ToString();
        }
        ddlDisplayLocation.SelectedValue = S_CHAR_A;
    }

    /// <summary>
    /// This method is used to set default control fields.
    /// </summary>
    private void ResetFields()
    {
        btnUpdate.Text = S_TEXT_SAVE;
        txtLinkName.Text = string.Empty;
        cmbDisplayLocation.SelectedValue = Convert.ToChar(Constants.NoticeDisplayLocation.Both).ToString();
        ddlDisplayLocation.ClearSelection();
        optAllNotices.Checked = true;
        optActiveNotices.Checked = false;
        txtCalStartDtPopup.Text = string.Empty;
        txtCalEndDtPopup.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        hidNoticeId.Value = string.Empty;
        hidFileDisplayLocation.Value = string.Empty;
        hidFileName.Value = string.Empty;
        hidSortDirection.Value = string.Empty;
        hidSortExpression.Value = string.Empty;
        hidSortOrder.Value = string.Empty;
        hidRowNo.Value = Constants.S_ZERO;
        hidNoticeImage.Value = string.Empty;
        txtEndTime.Text = S_END_TIME;
        txtStartTime.Text = S_START_TIME;
        txtLinkName.Focus();
        txtDescription.Text = string.Empty;
        txtTextNoticeDescription.Text = string.Empty;
        ClearRoles();
        chkAll.Checked = false;
        DataPager dtPager = lstvwNoticeDetails.FindControl("DtPgDropDown") as DataPager;
        if(dtPager!=null)
        dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, true);

        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;

            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)
                chkStandardDivLst.Items[iStandardIndex].Selected = false;

            chkStandard.Checked = false;
        }

        foreach (ListViewDataItem Item in lstvwStandardDivisionsText.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;

            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)
                chkStandardDivLst.Items[iStandardIndex].Selected = false;

            chkStandard.Checked = false;
        }
    }

    /// <summary>
    /// This method is used to fetch roles from table MenusRoles according to MenuId.
    /// </summary>
    private void ClearRoles()
    {
        for (int iIndex = 0; iIndex < chkListRoles.Items.Count; iIndex++)
            chkListRoles.Items[iIndex].Selected = false;

        for (int iIndex = 0; iIndex < chkListRolesText.Items.Count; iIndex++)
            chkListRolesText.Items[iIndex].Selected = false;
    }

  
    /// <summary>
    /// This method is used to disable roles while user selected homepage as display location.
    /// </summary>
    private void DisableRoles(bool abflag)
    {
        spnMandatoryUserRoleFile.Visible = abflag;
        chkAll.Enabled = abflag;
        for (int iIndex = 0; iIndex < chkListRoles.Items.Count; iIndex++)
            chkListRoles.Items[iIndex].Enabled = abflag;

        spnMandatoryUserRoleText.Visible = abflag;
        chkAllText.Enabled = abflag;
        for (int iIndex = 0; iIndex < chkListRolesText.Items.Count; iIndex++)
            chkListRolesText.Items[iIndex].Enabled = abflag;
    }

    /// <summary>
    /// This methos is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumErrorMsgText.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        new Button[] { btnUpdate, btnCancel, btnSaveText, btnSaveSelected, btnCancelText }.ApplyEffect();
        btnSaveSelected.Attributes.Add("onclick", "if(!SelectedCount(0)){return false;}");
        chkLstClasses.Attributes.Add("onclick", "CheckOrUncheckAllCheckBox()");
        chkLstClassesText.Attributes.Add("onclick", "CheckOrUncheckAllCheckBoxForText()");
        txtLinkName.Focus();
    }

    /// <summary>
    /// This methos is used to check file size and then check correct file to specified location
    /// </summary>
    private string UploadNoticeFile(ref string asFileName)
    {
        string sReturnErrorMsg = string.Empty;
        string sOldFileName = hidFileName.Value;
        string sServerPath = Server.MapPath("~");
        if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
            sServerPath = sServerPath + "\\";
        bool bHasFile = fileUploadItems.HasFile;
        string sNewFileName = sServerPath + S_FOLDER_LOCATION + fileUploadItems.FileName;
        if (bHasFile)
        {
            // Check for File size
            if (fileUploadItems.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                sReturnErrorMsg = S_FILE_SIZE_ERROR;
            else
            {
                if (sOldFileName != string.Empty)
                {
                    if (File.Exists(sServerPath + S_FOLDER_LOCATION + sOldFileName))
                        File.Delete(sServerPath + S_FOLDER_LOCATION + sOldFileName);
                }

                if (File.Exists(sNewFileName))
                {
                    asFileName = CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName);
                    fileUploadItems.SaveAs(sServerPath + S_FOLDER_LOCATION + asFileName);
                }
                else
                {
                    asFileName = fileUploadItems.FileName;
                    fileUploadItems.SaveAs(sNewFileName);
                }
            }
        }
        else
        {
            sReturnErrorMsg = S_FILE_NOT_FOUND;
            throw new FileNotFoundException();
        }

        return sReturnErrorMsg;
    }

    /// <summary>
    /// This methos is used Update notice details.
    /// </summary>
    private string SaveNoticeDetails()
    {
        string sErrorMsg = string.Empty;
        int iNoticeId = 0;
        string sFileName = fileUploadItems.FileName;

        if (hidNoticeId.Value != string.Empty)
            iNoticeId = Convert.ToInt32(hidNoticeId.Value);
        if (sFileName == string.Empty)
            sFileName = hidFileName.Value;
        else
            sErrorMsg = UploadNoticeFile(ref sFileName);

        string asFileName = string.Empty;

        if (FilUplodNotice.FileName != string.Empty)
        {            
            if (FilUplodNotice.HasFile)
            {
                CheckImageIsValid(ref sErrorMsg, out asFileName, FilUplodNotice);
            }
        }
        else
            asFileName = hidNoticeImage.Value;

        if (sErrorMsg == string.Empty)
        {
            if (string.IsNullOrEmpty(txtStartTime.Text))
                txtStartTime.Text = S_START_TIME;
            if (string.IsNullOrEmpty(txtEndTime.Text))
                txtEndTime.Text = S_END_TIME;
            DateTime dtStartdt = Convert.ToDateTime(txtCalStartDtPopup.Text + ' ' + txtStartTime.Text);
            DateTime dtEnddt = Convert.ToDateTime(txtCalEndDtPopup.Text + ' ' + txtEndTime.Text);
            int iSortOrder = Convert.ToInt32(txtSortOrder.Text);
            PopulateNoticeDetails(iNoticeId, sFileName, dtStartdt, dtEnddt, iSortOrder, asFileName);
        }

        return sErrorMsg;
    }

    private void CheckImageIsValid(ref string sErrorMsg, out string asFileName, FileUpload sFileUpload)
    {
        string sServerPath = Server.MapPath("~");
        if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
            sServerPath = sServerPath + "\\";
        string sLinkName = CommonUtility.GetFileNameForRenaming(sFileUpload.FileName.ToString());

        asFileName = string.Empty;
        string sFileName1 = sFileUpload.PostedFile.FileName;
        int iFileLengthinKb = sFileUpload.PostedFile.ContentLength / I_FILE_SIZE_LIMIT;        
                
        if (sFileUpload.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
        {
            string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
            sFileUpload.SaveAs(sLinkPath);
            asFileName = sLinkName;
        }
        else
            sErrorMsg = S_FILE_SIZE_ERROR;
    }

    /// <summary>
    /// This method is used to populate notice details.
    /// </summary>
    /// <param name="iNoticeId"></param>
    /// <param name="sFileName"></param>
    /// <param name="dtStartdt"></param>
    /// <param name="dtEnddt"></param>
    /// <param name="iSortOrder"></param>
    /// <param name="iSortOrderLocationChanged"></param>
    private void PopulateNoticeDetails(int aiNoticeId, string asFileName, DateTime dtStartdt, DateTime dtEnddt, int aiSortOrder, string asImageFileName)
    {
        NoticeDetails oNoticeDetails = new NoticeDetails();
        oNoticeDetails.SchoolId = miSchoolId;
        oNoticeDetails.NoticeId = aiNoticeId;
        oNoticeDetails.NoticeName = txtLinkName.Text.Trim();
        oNoticeDetails.DisplayLocation = cmbDisplayLocation.SelectedValue;
        oNoticeDetails.StartDate = dtStartdt.ToString();
        oNoticeDetails.EndDate = dtEnddt.ToString();
        oNoticeDetails.SortOrder = aiSortOrder;
        oNoticeDetails.FileName = asFileName;
        oNoticeDetails.InertedById = miUserId;
        oNoticeDetails.AcademicYearId = miAcademicYearId;
        oNoticeDetails.IsSelected = true;
        oNoticeDetails.NoticeDescription = txtDescription.Text.Trim();
        oNoticeDetails.NoticeImage = asImageFileName;
        string sUserRoleIds = string.Join(",", GetSelectedRoles());

        string sSelectedClassIds = GetStandardStrLst();

        string sXml = CommonUtility.GenerateXml(oNoticeDetails);
        NoticeDetailsBL.Update(sXml, sUserRoleIds, sSelectedClassIds);

        hidStandardDivIds.Value = sSelectedClassIds;
        NoticeDetailsBL oNoticeDetailsBL = new NoticeDetailsBL();
        SendPushNotification(sUserRoleIds, oNoticeDetails.NoticeName);
        
    }

    /// <summary>
    /// This method is used to get arraylist of standards.
    /// </summary>
    /// <returns></returns>
    private String GetStandardStrLst()
    {
        String strAssociatedStdLst="";
        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
            {
                if (chkStandardDivLst.Items[iCount].Selected)
                    strAssociatedStdLst = strAssociatedStdLst + chkStandardDivLst.Items[iCount].Value + ",";
            }
        }
        return strAssociatedStdLst;
    }

    /// <summary>
    /// This method is used to get arraylist of standards for text.
    /// </summary>
    /// <returns></returns>
    private String GetStandardStrLstForText()
    {
        String strAssociatedStdLst = "";

        foreach (ListViewDataItem Item in lstvwStandardDivisionsText.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
            {
                if (chkStandardDivLst.Items[iCount].Selected)
                    strAssociatedStdLst = strAssociatedStdLst +chkStandardDivLst.Items[iCount].Value + ",";
            }
        }
        return strAssociatedStdLst;
    }

    /// <summary>
    /// This method is used to collect selected roles into List collection.
    /// </summary>
    /// <returns></returns>
    private List<int> GetSelectedRoles()
    {
        int iTotalRoles = chkListRoles.Items.Count;
        List<int> RoleValues = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
        {
            if (chkListRoles.Items[iListIndex].Selected == true)
                RoleValues.Add(Convert.ToInt32(chkListRoles.Items[iListIndex].Value));
        }
        return RoleValues;
    }

    /// <summary>
    /// This method is used to collect selected roles into List collection.
    /// </summary>
    /// <returns></returns>
    private List<int> GetSelectedRolesText()
    {
        int iTotalRoles = chkListRolesText.Items.Count;
        List<int> RoleValues = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
        {
            if (chkListRolesText.Items[iListIndex].Selected == true)
                RoleValues.Add(Convert.ToInt32(chkListRolesText.Items[iListIndex].Value));
        }
        return RoleValues;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    /// <summary>
    /// This method is used to set sort image.
    /// </summary>
    private void AddSortImage()
    {
        if (string.IsNullOrEmpty(hidSortExpression.Value))
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        if (string.IsNullOrEmpty(hidSortDirection.Value))
            hidSortDirection.Value = Constants.S_DESCENDING;
        HtmlTableRow oHtmlTableHeaderRow = lstvwNoticeDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
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
    /// This method is used to set controls for text notice.
    /// </summary>
    /// <param name="iNoticeId"></param>
    private void SetEditModeForTextNotice(int iNoticeId)
    {
        btnSaveText.Text = S_TEXT_UPDATE;
        hidMode.Value = Constants.S_EDIT_MODE;
        NoticeDetailsBL oNoticeDetailsBL = new NoticeDetailsBL(iNoticeId);
        hidNoticeIdText.Value = iNoticeId.ToString();
        txtNoticeName.Text = oNoticeDetailsBL.NoticeDetails.NoticeName;
        txtEndDateTextNotice.Text = Convert.ToDateTime(oNoticeDetailsBL.NoticeDetails.EndDate).ToString("dd-MMM-yyyy");
        txtEndTimeTextNotice.Text = Convert.ToDateTime(oNoticeDetailsBL.NoticeDetails.EndDate).ToShortTimeString();
        txtStartDateTextNotice.Text = Convert.ToDateTime(oNoticeDetailsBL.NoticeDetails.StartDate).ToString("dd-MMM-yyyy");
        txtStartTimeTextNotice.Text = Convert.ToDateTime(oNoticeDetailsBL.NoticeDetails.StartDate).ToShortTimeString();
        txtSortOrderTextNotice.Text = oNoticeDetailsBL.NoticeDetails.SortOrder.ToString();
        txtTextNoticeDescription.Text = oNoticeDetailsBL.NoticeDetails.NoticeDescription.ToString();
        if (oNoticeDetailsBL.NoticeDetails.NoticeImage != null)
        {
            hidNoticeImage.Value = oNoticeDetailsBL.NoticeDetails.NoticeImage.ToString();
            btnTxtView.Visible = true;
            imgbtnTxtDelete.Visible = true;

            string sNewFileName = S_FOLDER_PATH + oNoticeDetailsBL.NoticeDetails.NoticeImage.ToString();
            btnTxtView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
        }
        else
        {
            imgbtnDelete.Visible = false;
            btnView.Visible = false;
        }
        if (!string.IsNullOrEmpty(oNoticeDetailsBL.NoticeDetails.NoticeContent))
        {
            FCKNoticeContent.Text = HttpUtility.HtmlDecode(oNoticeDetailsBL.NoticeDetails.NoticeContent);
        }
        else
            FCKNoticeContent.Text = HttpUtility.HtmlDecode("<p><BR><p>");
        if (oNoticeDetailsBL.NoticeDetails.DisplayLocation == "B")
            cmbDisplayLocationTextNotice.SelectedIndex = Constants.I_ZERO;
        else if (oNoticeDetailsBL.NoticeDetails.DisplayLocation == "C")
            cmbDisplayLocationTextNotice.SelectedIndex = Constants.I_ONE;
        else if (oNoticeDetailsBL.NoticeDetails.DisplayLocation == "H")
            cmbDisplayLocationTextNotice.SelectedIndex = Constants.I_TWO;

         ClearRoles();
        FetchRolesText(iNoticeId);
        FetchStandardDivisionsForText(iNoticeId);
    }

    /// <summary>
    /// this method is used to show controls and listview according to radio button selected.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetVisibility(bool abFlag)
    {
        tblTextNoticeControls.Visible = abFlag;
        tblLinkNoticeControls.Visible = !abFlag;
        trTextNoticeControls.Visible = abFlag;
    }

    /// <summary>
    /// This method is used to convert HTML to plai text.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private string StripHTML(string source)
    {
        string result;

        // Remove HTML Development formatting
        // Replace line breaks with space
        // because browsers inserts space
        result = source.Replace("\r", " ");

        // Replace line breaks with space
        // because browsers inserts space
        result = result.Replace("\n", " ");

        // Remove step-formatting
        result = result.Replace("\t", string.Empty);

        // Remove repeating spaces because browsers ignore them
        result = System.Text.RegularExpressions.Regex.Replace(result,
                                                              @"( )+", " ");

        // Remove the header (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*head([^>])*>", "<head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*head( )*>)", "</head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(<head>).*(</head>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all scripts (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*script([^>])*>", "<script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*script( )*>)", "</script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //result = System.Text.RegularExpressions.Regex.Replace(result,
        //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
        //         string.Empty,
        //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<script>).*(</script>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all styles (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*style([^>])*>", "<style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*style( )*>)", "</style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(<style>).*(</style>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert tabs in spaces of <td> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*td([^>])*>", "\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line breaks in places of <BR> and <LI> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*br( )*>", "\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*li( )*>", "\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line paragraphs (double line breaks) in place
        // if <P>, <DIV> and <TR> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*div([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*tr([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*p([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove remaining tags like <a>, links, images,
        // comments etc - anything that's enclosed inside < >
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[^>]*>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // replace special characters:
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @" ", " ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&bull;", " * ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lsaquo;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&rsaquo;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&trade;", "(tm)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&frasl;", "/",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lt;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&gt;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&copy;", "(c)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&reg;", "(r)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove all others. More can be added, see
        // http://hotwired.lycos.com/webmonkey/reference/special_characters/
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&(.{2,6});", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);


        // make line breaking consistent
        result = result.Replace("\n", "\r");

        // Remove extra line breaks and tabs:
        // replace over 2 breaks with 2 and over 4 tabs with 4.
        // Prepare first to remove any whitespaces in between
        // the escaped characters and remove redundant tabs in between line breaks
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)( )+(\r)", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\t)( )+(\t)", "\t\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\t)( )+(\r)", "\t\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)( )+(\t)", "\r\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove redundant tabs
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)(\t)+(\r)", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove multiple tabs following a line break with just one tab
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)(\t)+", "\r\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Initial replacement target string for line breaks
        string breaks = "\r\r\r";

        // Initial replacement target string for tabs
        string tabs = "\t\t\t\t\t";
        for (int index = 0; index < result.Length; index++)
        {
            result = result.Replace(breaks, "\r\r");
            result = result.Replace(tabs, "\t\t\t\t");
            breaks = breaks + "\r";
            tabs = tabs + "\t";
        }

        // That's it.
        return result;
    }

    public void FillApplicableRoles()
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBL();
        DataTable oDTRole = oNoticeBoardBL.RetriveRolesFromUserRoleMaster();
        iUsersCount = oDTRole.Rows.Count;
        if (!Settings.EnableOtherStaffLogin)
        {
            DataRow[] dr = oDTRole.Select("User_Role_Id=" + Constants.UserRoles.OtherStaff.ToInt());
            if (dr.Length > 0)
            {
                dr[0].Delete();
                oDTRole.AcceptChanges();
            }
        }

        chkListRoles.DataSource = oDTRole;
        chkListRoles.DataTextField =  "User_Role_Name";
        chkListRoles.DataValueField = "User_Role_Id";
        chkListRoles.DataBind();

        chkListRolesText.DataSource = oDTRole;
        chkListRolesText.DataTextField = "User_Role_Name";
        chkListRolesText.DataValueField = "User_Role_Id";
        chkListRolesText.DataBind();
        //chkAddListRoles.DataBind();
     }

    private NoticeBoardBL InitializeNoticeBoardBL()
    {
        NoticeBoardBL oNoticeBoardBL = new NoticeBoardBL();
        oNoticeBoardBL.SchoolId = miSchoolId;
        oNoticeBoardBL.AcademicYearId = miAcademicYearId;
        oNoticeBoardBL.InsertedById = miUserId;
        oNoticeBoardBL.UpdatedById = miUserId;
        oNoticeBoardBL.UpdatedDate = Convert.ToDateTime(System.DateTime.Today);

        return oNoticeBoardBL;
    }

    /// <summary>
    /// This method is used to get userids of user based on the selected role and entry of that 
    /// notification add into the notification table so service send notifiction to users selected for message.
    /// </summary>
    /// <param name="sUserRoleIds"></param>
    /// <param name="sNoticeName"></param>
    public override void SendPushNotification(string sUserRoleIds, object sNoticeName)
    {
        /* This code is used to return userid based on the selected role*/ 
        if (!string.IsNullOrEmpty(sUserRoleIds))
        {
                /*Split role Id's */
             int [] iRoleId=   Array.ConvertAll(sUserRoleIds.Split(','), int.Parse);
             List<int> listofusers= new List<int>();
            string sStdDivIds = string.Empty;
            foreach(var role in iRoleId)
            {
                if (role == Constants.I_THREE)
                    sStdDivIds = hidStandardDivIds.Value;
                else
                    sStdDivIds = string.Empty;
                /* Get All userid based on roleId*/
                List<int> intArrayUserId = MasterDataCollectionDC.GetAllUsersIdForUserRole(role, this.miSchoolId, this.miAcademicYearId, sStdDivIds);
                if (intArrayUserId.Count> 0)
                listofusers.AddRange(intArrayUserId);
            }
    
            PushNotificationClient pushNotificationClient = null;
            try
            {
                pushNotificationClient = new PushNotificationClient();
                int[] intArrayUserId = listofusers.ToArray();
                string noticeHeader = sNoticeName.ToString().Trim();
                Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
                dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_HEADING, noticeHeader);
                pushNotificationClient.SendNotification(NotificationMessageHeadings.NewSchoolNotice, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
                pushNotificationClient.Close();
            }
            catch (Exception)
            {   
            }
            finally
            {
                if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                    pushNotificationClient.Close();
            }
        }
    }

    #endregion "Private Methods"
}
