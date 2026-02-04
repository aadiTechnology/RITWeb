/* File Name :- TeacherInfoUI.aspx
 * Modified By :- Sachin
 * Modified Date :- 22-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display/edit/delete teachers.
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolAutoSearchService.Client;
using Utility;
public partial class TeacherInfoUI : ExportDataTable
{   
    #region Constants

    const int I_COLUMN_INDEX_TEACHER_NAME = 0;
    const int I_DATAKEY_TEACHER_ID = 1;
    const int I_DATAKEY_DESIGNATION_ID = 2;

    const int I_DATAKEY_USER_ID = 0;
    const int I_HEADMASTER_DESIGNATION_ID = 10;
    const int I_COLUMN_INDEX_CLASS_ASSSIGN = 4;
    const int I_COLUMN_INDEX_EDIT = 5;
    const int I_COLUMN_INDEX_ASSING_SUBJECT = 6;
    const int I_COLUMN_INDEX_DELETE = 9;
    const int I_COLUMN_INDEX_ADDITIONAL = 8;
    const int I_COLUMN_INDEX_TEACHER_DOB = 2;
    const int I_COLUMN_INDEX_QUALIFICATION = 3;

    #endregion

    #region Events

    /// <summary>
    /// This method is used to set default values to controls,set grid view properties and fill grid with teacher details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            lblError.Text = string.Empty;
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                CheckRoleAndSetGridview();
                RefreshValues();
                SetControlsDefaultValues();
                ReadQuerystring();
                FillUserTypesCombo();
                if(CheckPreCondition())
                    FillGridviewWithTeacherDetails();
                SetJavascriptAttributes();
                SetIdentitylinkURL();
                SetPhotolinkURL();
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }


            SetDefaultButton(btnSearch);
            txtName.Focus();                      
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search teacher by name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text.Trim().Equals(string.Empty))
                hidFilter.Value = String.Empty;
            else
                hidFilter.Value = " AND Teacher_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(txtName.Text.Trim(), true) + "%'";

            grdvwTeacherDetails.PageIndex = 0;
            FillGridviewWithTeacherDetails();
            SetIdentitylinkURL();
            SetPhotolinkURL();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to create a row to add default sort image. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);
                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, grdvwTeacherDetails.SortDirection);
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
    /// This event is used to fill gridview with changed page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwTeacherDetails.PageIndex = e.NewPageIndex;
            FillGridviewWithTeacherDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to sort gridview with ascending/descending order of seleted column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            FillGridviewWithTeacherDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to bind data rowwise to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                HyperLink oTeacherName = (HyperLink)e.Row.Cells[I_COLUMN_INDEX_TEACHER_NAME].Controls[Constants.I_ZERO];

                if (grdvwTeacherDetails.DataKeys[e.Row.RowIndex]["Is_Locked"].ToString() == "Y" && grdvwTeacherDetails.DataKeys[e.Row.RowIndex]["WorkingStatusId"].ToInt() == Constants.I_ONE)
                    e.Row.BackColor = System.Drawing.Color.Gainsboro;
                else if (grdvwTeacherDetails.DataKeys[e.Row.RowIndex]["Is_Locked"].ToString() == "Y" && grdvwTeacherDetails.DataKeys[e.Row.RowIndex]["WorkingStatusId"].ToInt() == Constants.I_TWO)
                    e.Row.BackColor = System.Drawing.Color.Pink;
                else if (grdvwTeacherDetails.DataKeys[e.Row.RowIndex]["Is_Locked"].ToString() == "Y" && grdvwTeacherDetails.DataKeys[e.Row.RowIndex]["WorkingStatusId"].ToInt() == Constants.I_THREE)
                    e.Row.BackColor = System.Drawing.Color.Turquoise;
                int iUserId = Convert.ToInt32(grdvwTeacherDetails.DataKeys[e.Row.RowIndex][I_DATAKEY_USER_ID].ToString());
                int iTeacherId = Convert.ToInt32(grdvwTeacherDetails.DataKeys[e.Row.RowIndex][I_DATAKEY_TEACHER_ID].ToString());

                string sClassAssign = e.Row.Cells[I_COLUMN_INDEX_CLASS_ASSSIGN].Text;
                Image oDelete = (Image)e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO];
                Image oChangeSubject = (Image)e.Row.Cells[7].Controls[0];
                oDelete.Attributes.Add("onclick", "if(! ConfirmDelete()){return false;}");


                if (Convert.ToInt32(grdvwTeacherDetails.DataKeys[e.Row.RowIndex][I_DATAKEY_DESIGNATION_ID]) == I_HEADMASTER_DESIGNATION_ID)
                    hidHeadMFlag.Value = "Y";

                Image oEditDetails = (Image)e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls[Constants.I_ZERO];

                Image oSubjectAssignment = (Image)e.Row.Cells[I_COLUMN_INDEX_ASSING_SUBJECT].Controls[Constants.I_ZERO];

                string sQualificationID = Convert.ToString(grdvwTeacherDetails.DataKeys[e.Row.RowIndex][I_DATAKEY_DESIGNATION_ID]);
                oSubjectAssignment.ToolTip = Resources.LocalizedResources.AssignTeacherToSubjectsdot;

                string sQuerystring = "UserId=" + iUserId
                                        + "&TeacherId=" + iTeacherId
                                        + "&HeadMasterFlag=" + hidHeadMFlag.Value
                                        + "&pIndex=" + grdvwTeacherDetails.PageIndex.ToString()
                                        + "&pSortExp=" + hidSortExpression.Value
                                        + "&pSortDirc=" + hidSortDirection.Value
                                        + "&QualificationID=" + sQualificationID
                                        + "&UserRoleId=" + Constants.UserRoles.Teacher.ToInt()
                                        + "&Is_Configured=" + hidIsConfigure.Value;
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
                oEditDetails.Attributes.Add("onclick", "window.open('TeacherUI.aspx?" + sEncrypt
                                                                  + " ' , '_self');return false;");

                Image oAdditionalDetails = (Image)e.Row.Cells[I_COLUMN_INDEX_ADDITIONAL].Controls[Constants.I_ZERO];
                string sQuery = "TeacherId=" + iTeacherId;
                oAdditionalDetails.Attributes.Add("onclick", "window.open('TeacherAdditionalDetailsUI.aspx?" + Utility.CommonUtility.EncryptQuerystring(sQuery) + "' , '_new'); return false;");
                if (SchoolBase.Settings.IsAaryanSchool)
                {
                    string sQuerystr = "UserId=" + iUserId
                                             + "&TeacherId=" + iTeacherId
                                             + "&HeadMasterFlag=" + hidHeadMFlag.Value
                                             + "&pIndex=" + grdvwTeacherDetails.PageIndex.ToString()
                                             + "&pSortExp=" + hidSortExpression.Value
                                             + "&pSortDirc=" + hidSortDirection.Value
                                             + "&QualificationID=" + sQualificationID
                                             + "&Is_Configured=" + hidIsConfigure.Value
                                             + "&Step=3";
                    oChangeSubject.Attributes.Add("onclick", "window.open('" + "TeacherUI.aspx" + "?" + Utility.CommonUtility.EncryptQuerystring(sQuerystr)
                                                                          + "' , '_self','scrollbars=yes,resizable=yes,top=0,left=0'); return false;");

                }    
                if (!SchoolBase.Settings.IsAaryanSchool)  
                {

                    if (oTeacherName.Enabled)
                        oTeacherName.Attributes.Add("onclick", "window.open('TeacherDetailsPopUp.aspx?" + sEncrypt
                                                                         + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700'); return false;");
                    string sQuerystr = "UserId=" + iUserId
                                            + "&TeacherId=" + iTeacherId
                                            + "&HeadMasterFlag=" + hidHeadMFlag.Value
                                            + "&pIndex=" + grdvwTeacherDetails.PageIndex.ToString()
                                            + "&pSortExp=" + hidSortExpression.Value
                                            + "&pSortDirc=" + hidSortDirection.Value
                                            + "&QualificationID=" + sQualificationID
                                            + "&Is_Configured=" + hidIsConfigure.Value
                                            + "&Step=3";

                    oChangeSubject.Attributes.Add("onclick", "window.open('" + "TeacherUI.aspx" + "?" + Utility.CommonUtility.EncryptQuerystring(sQuerystr)
                                                                    + "' , '_self','scrollbars=yes,resizable=yes,top=0,left=0'); return false;");
                }
                else 
                {
                    oTeacherName.Attributes.Add("onclick","window.open('EmployeeDetailsReportPopup.aspx?" + sEncrypt
                                                                + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700'); return false;");
                }

                    // assign teacher to subjects.
                    AssignTeacherTosubject(iTeacherId, oSubjectAssignment);
                   

                }

         


            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel"); 


                if (pageList != null)
                {
                    int pageNumber;
                    for (int i = 0; i < grdvwTeacherDetails.PageCount; i++)
                    {
                        pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (i == grdvwTeacherDetails.PageIndex)
                            item.Selected = true;
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    int currentPage = grdvwTeacherDetails.PageIndex + 1;
                    pageLabel.Text = Resources.LocalizedResources.PageNo + currentPage.ToString() + " " + Resources.LocalizedResources.Of + " " + grdvwTeacherDetails.PageCount.ToString() + " " + Resources.LocalizedResources.OutOflst;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete teacher details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "DELETE_TEACHER":

                    SchoolWiseTeacherMasterCollectionBL oSchoolWiseTeacherMasterCollectionBL = new SchoolWiseTeacherMasterCollectionBL();
                     int iRowIndex = Convert.ToInt32(e.CommandArgument);
                     int iTeacherId = Convert.ToInt32(grdvwTeacherDetails.DataKeys[iRowIndex][I_DATAKEY_TEACHER_ID]);
                     int iUserId = Convert.ToInt32(grdvwTeacherDetails.DataKeys[iRowIndex][I_DATAKEY_USER_ID]);
                     HyperLink oTeacherName = (HyperLink)grdvwTeacherDetails.Rows[iRowIndex].Cells[I_COLUMN_INDEX_TEACHER_NAME].Controls[Constants.I_ZERO];
                     string sTeacherName = oTeacherName.Text;
                    oSchoolWiseTeacherMasterCollectionBL.DeleteTeacher(miSchoolId, iTeacherId, sTeacherName, miAcademicYearId, iUserId, miFinancialYearId);
                    FillGridviewWithTeacherDetails();
                    btnAddTeacher.Enabled = true;
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = Resources.LocalizedResources.TeacherDeletedSuccessfullyMsg;
                    RefreshStaffCache(iUserId);
                    break;

            }
           
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblError.Text = CommonUtility.ModifyExceptionMessage(ex.Message, string.Empty, string.Empty, "can not be removed since assigned to", Resources.LocalizedResources.CanNotBeRemovedSinceAssignedTo);
            FillGridviewWithTeacherDetails();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set postback url and hide record count labels.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (grdvwTeacherDetails.Rows.Count < 0)
                trTotalRec.Visible = false;
			
            if (!IsPostBack)
                SetPostbackURL();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change gridview page index on change of page dropdown index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow pagerRow = grdvwTeacherDetails.BottomPagerRow;
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            grdvwTeacherDetails.PageIndex = pageList.SelectedIndex;
            FillGridviewWithTeacherDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set record count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdvwTeacherDetails.PageSize * grdvwTeacherDetails.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwTeacherDetails.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.ToString() == "0" || grdvwTeacherDetails.PageCount == 0 || grdvwTeacherDetails.PageCount < Constants.I_GRID_PAGE_COUNT)
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
    /// This event is used to set postback URL.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            SetPostbackURL();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to show all teacher date into excel sheet
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolUserCollectionBL oSchoolUserCollectionBL = new SchoolUserCollectionBL();
            DataTable oDataTable = oSchoolUserCollectionBL.GetAllTeacherDetails(miSchoolId, miAcademicYearId, "", "", 1000, 0,ddlUserType.SelectedValue.ToInt());

            string sCondition = string.Empty;
            if (ddlUserType.SelectedValue == "1")
                sCondition = "Is_Locked = 'N'";
            else if (ddlUserType.SelectedValue == "2")
                sCondition = "Is_Locked = 'Y'";
            else if (ddlUserType.SelectedValue == "3")
                sCondition = "IsInternalUser = 1";
            
            var dtFilter = oDataTable.Select(sCondition).CopyToDataTable();

            int iSrNo = 1;
            foreach (DataRow dr in dtFilter.Rows)
            {
                dr["RowNo"] = iSrNo;
                iSrNo++;
            }

            dtFilter.Columns.Remove("Is_Locked");
            dtFilter.Columns.Remove("IsInternalUser");

            ExportToExcel("TeacherInformation.xls", dtFilter);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillGridviewWithTeacherDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// This function sets the date format for date column property 
    /// </summary>
    /// 
    private void SetGridViewDate()
    {
        BoundField oReceivedDate = (BoundField)grdvwTeacherDetails.Columns[I_COLUMN_INDEX_TEACHER_DOB];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method i sused to hide all the controls.
    /// </summary>
    private void HideAllFields()
    {
		trLegend.Visible = false;
		trSearch.Visible = false;
		trTeacherTable.Visible = false;
		trButtons.Visible = false;
		btnCancel.Visible = true;
		btnCancel.Text = "Back";
    }

    /// <summary>
    /// This method is used to fill gridview with teacher details.
    /// </summary>
    private void FillGridviewWithTeacherDetails()
    {
        SetGridViewDate();
        grdvwTeacherDetails.DataSourceID = GrdDSobj.ID;
        grdvwTeacherDetails.DataBind();
        grdvwTeacherDetails.Columns[I_COLUMN_INDEX_QUALIFICATION].Visible = false;
    }

    /// <summary>
    /// This method is used to set gridview properties.
    /// </summary>
    private void GridViewProperties()
    {
        grdvwTeacherDetails.PageSize = Constants.I_GRID_PAGE_COUNT;
        grdvwTeacherDetails.EmptyDataText = Resources.LocalizedResources.NoTeacherAvailable;
    }

    /// <summary>
    /// This method is used to set default values of controls on page load.
    /// </summary>
    private void SetControlsDefaultValues()
    {
        const int I_COLUMN_INDEX_DESIGNATION = 1;
        hidSortExpression.Value = grdvwTeacherDetails.Columns[I_COLUMN_INDEX_DESIGNATION].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related));
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related));
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnAddTeacher, btnCancel, btnBack, btnSearch, btnUpload, btnExport });
        btnIdentityCard.Attributes["onmouseover"] = "javascript:fnover('" + btnIdentityCard.ClientID + "',this);";
        btnIdentityCard.Attributes["onmouseout"] = "javascript:fnout('" + btnIdentityCard.ClientID + "',this);";
    }

    /// <summary>
    /// This method is used to set postback URL.
    /// </summary>
    private void SetPostbackURL()
    {
        string sQuerystring = "HeadMasterFlag=" + hidHeadMFlag.Value + "&QualificationID=" + String.Empty + "&Is_Configured=" + hidIsConfigure.Value;
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        string sRedirectUrl = Constants.S_PAGE_TEACHER_UI + "?" + sEncryptedString;
        btnAddTeacher.PostBackUrl = sRedirectUrl;
    }

    /// <summary>
    /// This function checks the preconditons of Teachers .
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.Teacher);
        if (sLinks.Equals(String.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            HideAllFields();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to pass querystring to open popup for assigning teacher to subjects.
    /// </summary>
    /// <param name="aiTeacherId"></param>
    private void AssignTeacherTosubject(int aiTeacherId, Image aoSubjectAssignment)
    {
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        string sQuerystring = string.Empty;        
        bool bIsTeacherAssignforSubjects = oTeacherSubjectAssignmentBL.IsTeacherAssignedForSubject(miSchoolId, aiTeacherId);

        if (!bIsTeacherAssignforSubjects)
            sQuerystring = "TeacherId=" + aiTeacherId + "&ViewMode=" + Constants.ViewMode.New.ToString();
        else
            sQuerystring = "TeacherId=" + aiTeacherId + "&ViewMode=" + Constants.ViewMode.Edit.ToString();

        string sEncryptedstring = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        aoSubjectAssignment.Attributes.Add("onclick", "window.open('TeacherToSubjectsAssignmentUI.aspx?" + sEncryptedstring
                                                      + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=600'); return false;");
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            if (QueryString["pIndex"] != null)
            {
	            int iPageIndex;
				if (Int32.TryParse(QueryString["pIndex"], out iPageIndex))
					grdvwTeacherDetails.PageIndex = iPageIndex;
            }
            if (QueryString["pSortExp"] != null)
                hidSortExpression.Value = QueryString["pSortExp"];
            if (QueryString["pSortDirc"] != null)
                hidSortDirection.Value = QueryString["pSortDirc"];
            if (QueryString["Is_Configured"] != null)
                hidIsConfigure.Value = QueryString["Is_Configured"];
            if (QueryString["UserName"] != null)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = QueryString["UserName"];
            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    private void CheckRoleAndSetGridview()
    {
        if (moUserRole== Constants.UserRoles.Supervisor || moUserRole== Constants.UserRoles.Teacher)
        {
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Teacher).ToString();

            if (hidCanEdit.Value == "N")
            {
                btnAddTeacher.Visible = false;
                btnUpload.Visible = false;
                grdvwTeacherDetails.Columns[I_COLUMN_INDEX_EDIT].Visible = false;
                grdvwTeacherDetails.Columns[I_COLUMN_INDEX_ASSING_SUBJECT].Visible = false;
                grdvwTeacherDetails.Columns[I_COLUMN_INDEX_DELETE].Visible = false;                
            }
            else
            {
                btnAddTeacher.Visible = true;
                btnUpload.Visible = true;
                grdvwTeacherDetails.Columns[I_COLUMN_INDEX_EDIT].Visible = true;
                grdvwTeacherDetails.Columns[I_COLUMN_INDEX_ASSING_SUBJECT].Visible = true;
                grdvwTeacherDetails.Columns[I_COLUMN_INDEX_DELETE].Visible = true;
            }
        }
        else
        {
            btnAddTeacher.Visible = true;
            btnUpload.Visible = true;
            grdvwTeacherDetails.Columns[I_COLUMN_INDEX_EDIT].Visible = true;
            grdvwTeacherDetails.Columns[I_COLUMN_INDEX_ASSING_SUBJECT].Visible = true;
            grdvwTeacherDetails.Columns[I_COLUMN_INDEX_DELETE].Visible = true;
        }
    }

    #endregion

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetIdentitylinkURL()
    {
        btnIdentityCard.Enabled = true;
        string sQuerystring = "TeacherId=0" + "&TeacherName=" + txtName.Text.Trim(); ;
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = "../Admin/TeacherIdentityCardUI.aspx?" + sEncryptedString;

        btnIdentityCard.Attributes.Add("onclick", "ShowIdentities('" + sQuerystring + "');return false;");
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetPhotolinkURL()
    {
        btnUpload.Enabled = true;
        string sQuerystring = "TeacherName=" + txtName.Text.Trim();
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = "../Admin/TeacherPhotoUI.aspx?" + sEncryptedString;

        btnUpload.Attributes.Add("onclick", "ShowPhotos('" + sQuerystring + "');return false;");
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        GridViewProperties();
        hidAreYouSureToDeleteThisRecords.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords;
        FillGridviewWithTeacherDetails();
    }

    /// <summary>
    /// This method is used to refresh staff cache.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void RefreshStaffCache(int aiUserId)
    {
        List<int> lstUserIds = new List<int>();
        lstUserIds.Add(aiUserId);
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStaffCache(miSchoolId, miAcademicYearId, lstUserIds, Constants.Action.Update);
    }

    /// <summary>
    /// This method is used Fill the User Type Combobox.
    /// </summary>
    private void FillUserTypesCombo()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable dt = oMasterDataCollectionBL.GetAllUserTypes();
        ControlUtility.FillDropDownList(dt, ref ddlUserType,
                                       "UserTypeId",
                                      "UserType",string.Empty);
        ddlUserType.SelectedValue = Constants.S_ONE;
    }    
}

