// File Name   : SupervisorUserListUI.aspx.cs
// Created By  : Ashish
// Date        : 08/12/2008
// Description : This class provided supervisor user list. where we add/edit/delete this supervisor

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolAutoSearchService.Client;
using Utility;

public partial class SupervisorListUI : SchoolBase
{
    #region " Constants "

    string msServerFilePath;
    string msFileName;
    const Int32 I_PK_USER_ID = 0;
    const Int32 I_DATA_KEY_SUPERVISOR_ID = 1;
    const string S_CMD_NAME_DELETE_SUPERVISOR = "DELETE_SUPERVISOR";
    const string S_PAGE_SUPERVISOR_DETAILS = "~/Admin/SupervisorDetailsUI.aspx";
    const string S_IS_CONFIGURED = "Is_Configured";
	const int I_BINARY_PHOTO_IMAGE=4;
    const int I_COLUMN_INDEX_SUPERVISOR_NAME = 0;

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to fill Supervisor's list grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                RefreshValue();
                lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/AdminStaffDetails.xls','_self');return false;");
                ReadQuerystring();
                SetDefaultProperties();
                FillUserTypesCombo();
                FillSupervisorListGrid();
                SetClientScriptAttributes();

                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();

                }

            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                SetDefaultProperties();
            }
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)  /////added for paging
    {
        try
        {
            GridViewRow pagerRow = grdSupervisors.BottomPagerRow;
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            grdSupervisors.PageIndex = pageList.SelectedIndex;
             FillSupervisorListGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to navigate control to control panel page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Other_User_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evetn is used to add new Supervisor user for same school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            RedirectToAddSupervisor();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evetn is used display user as per user type like activated, Deactivated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSupervisorListGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " Grid Events "

    /// <summary>
    /// This method is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSupervisors_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();   
            FillSupervisorListGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to set sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSupervisors_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);
                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                else
                    CommonUtility.AddSortImage(1, e.Row, hidSortDirection.Value);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove the supervisor entry from grid when supervisor deleted.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSupervisors_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case S_CMD_NAME_DELETE_SUPERVISOR:
                    Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);
                    DeleteSupervisorDetails(iRowIndex);
                    FillSupervisorListGrid();
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = Resources.LocalizedResources.AdminStaffDelete;
                    if (grdSupervisors.Rows.Count == 0)
                        DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AdminStaffConfig));
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set grid column attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSupervisors_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
          
            {
                Image oImg = (Image)e.Row.FindControl("imgPhotoUpload");
                if (oImg != null)
                {
                    //If photo is uploaded
					if (grdSupervisors.DataKeys[e.Row.RowIndex][I_BINARY_PHOTO_IMAGE] == DBNull.Value)
                        oImg.ImageUrl = "~/RITeSchool/images/IconGridStudentBlankPh.gif";
                    else
                        oImg.ImageUrl = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
                }
            }
            SetRowData(e.Row);
            if (e.Row.RowType == DataControlRowType.Pager)  //new code added
            {
                GridViewRow pagerRow = e.Row;
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    int pageNumber;
                    for (int i = 0; i < grdSupervisors.PageCount; i++)
                    {
                        pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (i == grdSupervisors.PageIndex)
                            item.Selected = true;
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    int currentPage = grdSupervisors.PageIndex + 1;
                    pageLabel.Text = Resources.LocalizedResources.PageNo + currentPage.ToString() + " " + Resources.LocalizedResources.Of + " " + grdSupervisors.PageCount.ToString() + " " + Resources.LocalizedResources.OutOflst;
                }
            }
          
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Grid Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            hidIsConfig.Value = QueryString[S_IS_CONFIGURED];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>
    private void SetDefaultProperties()
    {
        ValSummaryErrMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        if (string.IsNullOrEmpty(hidSortDirection.Value))
            hidSortDirection.Value = Constants.S_ASCENDING;
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.NodeTitle = Constants.S_SUPERVISOR_ROLE_NAME;
    }

    ///<Summary>
    ///This method is used to fill Supervisor's grid
    ///</Summary>
    private void FillSupervisorListGrid()   ///////
    {
        //DataTable oDTUserDetails = SchoolWiseSupervisorMasterCollectionBL.FetchSchoolWiseSupervisorMasterDetails(miSchoolId, miAcademicYearId, hidSortDirection.Value, hidSortExpression.Value, ddlUserType.SelectedValue.ToInt());       
        //grdSupervisors.DataSource = oDTUserDetails.DefaultView;
        //grdSupervisors.DataBind();
        grdSupervisors.DataSourceID = GrdDSobj.ID;
        grdSupervisors.DataBind();
    }
    /// <summary>
    /// This event is used to set record count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)  /////
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdSupervisors.PageSize * grdSupervisors.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdSupervisors.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.ToString() == "0" || grdSupervisors.PageCount == 0 || grdSupervisors.PageCount < Constants.I_GRID_PAGE_COUNT)
                        trTotalRec.Visible = false;
                    else
                        trTotalRec.Visible = true;

                    if (lblTotal.Text != string.Empty && Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                        lblEndIndex.Text = e.ReturnValue.ToString();
                }
                if (lblTotal.Text != String.Empty)
                {
                    if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                        trTotalRec.Visible = false;
                    else
                        trTotalRec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill gridview with changed page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSupervisors_PageIndexChanging(object sender, GridViewPageEventArgs e)  /////
    {
        try
        {
            grdSupervisors.PageIndex = e.NewPageIndex;
            FillSupervisorListGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   

    /// <summary>
    /// This method is used to set sort direction for grid column.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnAdd, btnBack });
    }

    /// <summary>
    /// This method is used to delete supervisor details.
    /// </summary>
    private void DeleteSupervisorDetails(Int32 iRowIndex)
    {
        SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        oSchoolWiseSupervisorMasterBL.User_Id = Convert.ToInt32(grdSupervisors.DataKeys[iRowIndex][I_PK_USER_ID].ToString());
        oSchoolWiseSupervisorMasterBL.Supervisor_Id = Convert.ToInt32(grdSupervisors.DataKeys[iRowIndex][I_DATA_KEY_SUPERVISOR_ID]);
        oSchoolWiseSupervisorMasterBL.Updated_By_Id = miUserId;
        oSchoolWiseSupervisorMasterBL.Update_Date = DateTime.Now;
        oSchoolWiseSupervisorMasterBL.DeleteSupervisor();
        RefreshStaffCache(oSchoolWiseSupervisorMasterBL.User_Id, Constants.Action.Update);
    }

    /// <summary>
    /// This method is used to bound java script function.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetRowData(GridViewRow gridViewRow)
    {
        const Int32 I_COL_INDEX_EDIT = 4;
        const Int32 I_COL_INDEX_DELETE = 5;

        int iRowIndex = gridViewRow.RowIndex;
        if (iRowIndex >= 0)
        {
            HyperLink oUserName = (HyperLink)gridViewRow.Cells[I_COLUMN_INDEX_SUPERVISOR_NAME].Controls[Constants.I_ZERO];
          
            int iUserId = Convert.ToInt32(grdSupervisors.DataKeys[iRowIndex][I_PK_USER_ID]);
            int iSupervisorId = Convert.ToInt32(grdSupervisors.DataKeys[iRowIndex][I_DATA_KEY_SUPERVISOR_ID]);
            string sIsLocked = Convert.ToString(grdSupervisors.DataKeys[iRowIndex]["Is_Locked"]);            
            Image oEditDetails = (Image)gridViewRow.Cells[I_COL_INDEX_EDIT].Controls[Constants.I_ZERO];
            string sQuerystring = "UserId=" + iUserId + "&SupervisorId=" + iSupervisorId + "&Is_Configured=" + hidIsConfig.Value; ;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
            if (sIsLocked == "Y")
                gridViewRow.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#DCDCDC !important;");

            oEditDetails.Attributes.Add("onclick", "window.open('../Admin/SupervisorDetailsUI.aspx?" + sEncrypt
                                                              + " ' , '_self');return false;");
            Image oDelete = (Image)gridViewRow.Cells[I_COL_INDEX_DELETE].Controls[Constants.I_ZERO];
            oDelete.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdSupervisors.AllowPaging + "'))){return false;}");

            if (SchoolBase.Settings.IsAaryanSchool)
            {
                string sQuerystr = "UserId=" + iUserId + "&UserRoleId=" + Constants.UserRoles.Supervisor.ToInt()+ "&IncludeDeactivatUser=1";

                oUserName.Attributes.Add("onclick", "window.open('EmployeeDetailsReportPopup.aspx?" + Utility.CommonUtility.EncryptQuerystring(sQuerystr)
                                                                    + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700'); return false;");
            }
           
        }
        
    }

    /// <summary>
    /// This method is used to to redirect to the Supervisor Details web page.
    /// </summary>
    private void RedirectToAddSupervisor()
    {
        string sQuerystring = S_IS_CONFIGURED + "=" + hidIsConfig.Value;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        string sRedirectUrl = S_PAGE_SUPERVISOR_DETAILS + "?" + sEncrypt;
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage(sRedirectUrl);
    }

    /// <summary>
    /// This method is used to refill data and  appropriate messages.
    /// </summary>
    private void RefillData()
    {
        lblHead.CssClass = "ClsHilightTextB";
        lblHead.Text = Resources.LocalizedResources.FileUplpadSuccessfully;
        lblHead.Visible = true;
        SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AdminStaffConfig));
        FillSupervisorListGrid();
    }

    #endregion " Private Methods "

    protected void btnImportAdminStaff_Click(object sender, EventArgs e)
    {
        try
        {
            lblUpdateSucess.Visible = false;
            msFileName = CommonUtility.GetFileNameForRenaming(fileUploadAdminStaff.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            msServerFilePath = sFolderName + msFileName;
            fileUploadAdminStaff.SaveAs(msServerFilePath);
            string sErrorMessage = "";
            string sSourceFileName = fileUploadAdminStaff.PostedFile.FileName;
            Constants.UploadFileType eUploadFileType = Constants.UploadFileType.Supervisor;

            FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, msServerFilePath, eUploadFileType);
            oFileUploadUtility.UserId = miUserId;
            oFileUploadUtility.SchoolId = miSchoolId;
            oFileUploadUtility.AcademicYearId = miAcademicYearId;
            oFileUploadUtility.CanPublishUnpublishExam = Settings.AllowPublishUnpublishExam ;
            sErrorMessage = oFileUploadUtility.UploadFile();

            if (sErrorMessage.Equals(""))
            {
                RefillData();
                RefreshStaffCache(0, Constants.Action.Insert);
            }
            else
                DisplayError(sErrorMessage);
        }
        catch (SqlException ex)
        {           
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (BusinessLogic.Exceptions.ValidEmailAddressExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentFirstNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentLastNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentAdmissionDateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentJoiningDateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMobileExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidMobileNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentDateofBirthExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidPincodeExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMotherNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentAddressExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullEmergencyContactException ex)
        {
            catchException(ex);
        }
        catch (NoRecordFoundExceptions ex)
        {
            catchException(ex);
        }
        catch (ValidExceptions ex)
        {
            catchException(ex);
        }
        catch (Exception ex)
        {
            //lblHead.Text = Resources.LocalizedResources.FileUploadData;
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        try
        {
            if (System.IO.File.Exists(msServerFilePath))
                System.IO.File.Delete(msServerFilePath);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        FillSupervisorListGrid();
    }
    /// <summary>
    /// This method is used to search Staff.
    /// </summary>
    /// <param name="asError"></param>
    protected void btnSearch_Click(object sender, EventArgs e)  //
    {
        try
        {
            if (txtName.Text.Trim().Equals(string.Empty))
                hidFilter.Value = String.Empty;
            else
                hidFilter.Value = " AND GSLST.FullName LIKE '%" + StringUtility.ReplaceSingleQuoteInString(txtName.Text.Trim(), true) + "%'";

            grdSupervisors.PageIndex = 0;
            FillSupervisorListGrid();
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to display error messahe.
    /// </summary>
    /// <param name="asError"></param>
    private void DisplayError(string asError)
    {
        lblHead.Text = asError;
    }
    private void catchException(Exception ex)
    {
        lblError.Text = ex.Message;
        lblError.CssClass = "ClsLabel";
        lblError.Visible = true;
        lblError.ForeColor = System.Drawing.Color.Red;
        grdSupervisors.Visible = true;
    }

    /// <summary>
    /// This method is used to refresh staff cache.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void RefreshStaffCache(int aiUserId, Constants.Action aoAction)
    {
        List<int> lstUserIds = new List<int>();
        if (aiUserId != 0)
            lstUserIds.Add(aiUserId);
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStaffCache(miSchoolId, miAcademicYearId, lstUserIds, aoAction);
    }

    private void RefreshValue()
    {
        hidValFileUpload.Value = Resources.LocalizedResources.ValFileUpload;
        hidValFileUploadType.Value = Resources.LocalizedResources.ValFileUploadType;
        hidDeleteAdminStaff.Value = Resources.LocalizedResources.DeleteAdminStaff;
    }

    /// <summary>
    /// This method is used Fill the User Type Combobox.
    /// </summary>
    private void FillUserTypesCombo()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable dtUSerTypes = oMasterDataCollectionBL.GetAllUserTypes();
        ControlUtility.FillDropDownList(dtUSerTypes, ref ddlUserType,
                                       "UserTypeId",
                                      "UserType", string.Empty);
        ddlUserType.SelectedValue = Constants.S_ONE;
    }

}
