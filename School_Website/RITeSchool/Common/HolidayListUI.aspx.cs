// File Name   : HolidaysmanagementConfiguration.aspx.cs
// Created By  : Ketan
// Date        : 28/11/2007   
// Description : This class is used to show school holiday list.
// Modified By : Amit
// Date        : 24/9/2009  

using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// This class is used to provides the UI to enlist the Holidays and allows to Add/Edit/Delete 
/// the holidays from this list.
/// </summary>
public partial class HolidayListUI : SchoolBase
{

    #region Member

    DateTime dtUpcomingHolidyDate;

    #endregion

    #region Constant

    const Int32 I_COLUMN_INDEX_START_DATE = 0;
    const Int32 I_COLUMN_INDEX_END_DATE = 1;
    const Int32 I_COLUMN_INDEX_EDIT = 5;
    const Int32 I_COLUMN_INDEX_DELETE = 6;
    const Int32 I_COLUMN_INDEX_STANDARDS = 3;
    const string S_CMD_NAME_DELETE_HOLIDAY = "DELETE_HOLIDAYS";
    const string S_CMD_NAME_SORT = "SORT";

    #endregion

    #region Events

    /// <summary>
    /// This method is used to fill grid view.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                CheckRoleAndAssignDisplayView();
                SetControlsAsPerViewMode();
                string sTestDecrypt = GetQuerystring();
                if (CheckPreCondition())
                {
                    SetSortingFieldDefaultValues();
                    FillHolidayListGridview();
                    btnAdd.Attributes.Add("onclick", "window.open('../Admin/HolidayConfigurationPopup.aspx?" + sTestDecrypt + "', '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=680');return false;");
                }
                SetPostbackUrl();
                SetClientSideScriptAttributes();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    // caltxtDeletedDate.DateValue = Convert.ToDateTime(DateCultureConversion(DateTime.Today.ToString(), hidCultureInfo.Value, Session[Constants.S_SESSION_LANGUAGE].ToString()).ToString("dd MMM yyyy")));
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            dtUpcomingHolidyDate = GetUpcomingHolidayDate();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// This event is used to add paging for grid veiw. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdHoliDaysManagement.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdHoliDaysManagement.PageIndex = pageList.SelectedIndex;
            FillHolidayListGridview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   
    /// <summary>
    /// This event is used to bind data rows to grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdHoliDaysManagement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("btnDeleteHoliday");
                imgDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()){return false;}");

                ImageButton imgEdit = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls[Constants.I_ZERO];
                // To transfer HolidayId to  HolidayConfigurationPopup page using query string.
                Int32 iHolidayId = Convert.ToInt32(grdHoliDaysManagement.DataKeys[e.Row.RowIndex]["Holiday_Id"].ToString());
                string sQueryString = "HolidayId=" + iHolidayId + "&Is_Configured=" + hidIsConfig.Value; ;
                string sEncryptHolidayId = Utility.CommonUtility.EncryptQuerystring(sQueryString);

                // This will transfer holidayID in encrypeted format to HolidayConfigurationPopup page.
                imgEdit.Attributes.Add("onclick", "window.open('../Admin/HolidayConfigurationPopup.aspx?" +
                sEncryptHolidayId + "', '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=680');return false;");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string sStartDate = (string)e.Row.Cells[I_COLUMN_INDEX_START_DATE].Text;
                string sEndDate = (string)e.Row.Cells[I_COLUMN_INDEX_END_DATE].Text;
                int iRowIndex = ((GridViewRow)e.Row).RowIndex;
                string sStartDay = (grdHoliDaysManagement.DataKeys[iRowIndex]["StartDay"]).ToString();
                string sEndtDay = (grdHoliDaysManagement.DataKeys[e.Row.RowIndex]["EndDay"]).ToString();

                e.Row.Cells[I_COLUMN_INDEX_START_DATE].Text = e.Row.Cells[I_COLUMN_INDEX_START_DATE].Text + "&nbsp;" + "(" + sStartDay + ")";
                e.Row.Cells[I_COLUMN_INDEX_END_DATE].Text = e.Row.Cells[I_COLUMN_INDEX_END_DATE].Text + "&nbsp;" + "(" + sEndtDay + ")";
                DateTime dtToday = DateTime.Today;
                DateTime dtStartDate = Convert.ToDateTime(sStartDate);
                DateTime dtEndDate = Convert.ToDateTime(sEndDate);

                if (dtUpcomingHolidyDate == dtStartDate)
                {
                    e.Row.Font.Bold = true;
                    e.Row.Style.Add("Background-color", "#EFDCC9 ! important");
                }
                else if (dtStartDate < dtToday && dtEndDate < dtToday)
                    e.Row.Style.Add("color", "#bebebe! important");
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data source.
                    for (int i = 0; i < grdHoliDaysManagement.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.                        
                        if (i == grdHoliDaysManagement.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the dropDownList.
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int icurrentPage = grdHoliDaysManagement.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + icurrentPage.ToString() +
                      " of " + grdHoliDaysManagement.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete holiday record. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdHoliDaysManagement_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() != S_CMD_NAME_SORT)
            {
                Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);
                switch (e.CommandName)
                {
                    case S_CMD_NAME_DELETE_HOLIDAY:
                        int iHolidayID = Convert.ToInt32(grdHoliDaysManagement.DataKeys[iRowIndex]["Holiday_Id"].ToString());
                        HolidaysMasterBL oHolidaysMasterBL = new HolidaysMasterBL();
                        oHolidaysMasterBL.HolidayId = iHolidayID;
                        oHolidaysMasterBL.AcademicYearId = miAcademicYearId;
                        oHolidaysMasterBL.SchoolId = miSchoolId;
                        oHolidaysMasterBL.DeleteHolidaysMaster();
                        dtUpcomingHolidyDate = GetUpcomingHolidayDate();

                        GridViewRow pagerRow = grdHoliDaysManagement.BottomPagerRow;
                        DropDownList oDropDownList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                        int iSelectedVal =Convert.ToInt32(oDropDownList.SelectedValue);
                        int icount = grdHoliDaysManagement.Rows.Count;
                        if (icount ==1 && iSelectedVal>1)
                            {
                                oDropDownList.SelectedValue = (Convert.ToInt32(oDropDownList.SelectedValue) - 1).ToString();
                                PageDropDownList_SelectedIndexChanged(null, new EventArgs());
                            }  
                        else
                            FillHolidayListGridview();
                        grdHoliDaysManagement.DataBind();
                        if (grdHoliDaysManagement.Rows.Count ==0)
                            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.HolidaysManagement));

                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
	

    /// <summary>
    /// This event is used to show holiday records count in grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdHoliDaysManagement.PageSize * grdHoliDaysManagement.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdHoliDaysManagement.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0" || grdHoliDaysManagement.PageCount == 0)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
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

    #endregion

    #region Private Method

    /// <summary>
    /// This method is used to set java script properties to page controls.
    /// </summary>
    private void SetClientSideScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnAdd, btnBack });        
    }

    /// <summary>
    /// This method is used to check if the login user is of superviser role or teacher and 
    /// check the access he have.
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.HolidaysManagement).ToString();
    }

    /// <summary>
    /// This method is used to set postback URL for back. 
    /// </summary>
    private void SetPostbackUrl()
    {
        if (moUserRole== Constants.UserRoles.Admin || moUserRole== Constants.UserRoles.Supervisor)
        {
            btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Attendance_Related));
        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                string sUrl = Request.UrlReferrer.AbsolutePath;
                sUrl = sUrl.Substring(sUrl.LastIndexOf("/") + 1);
				
				// If user redirect from school configuration then set back url of school configuration screen.
                if (sUrl == "SchoolConfigurationControlPanel.aspx")
                    btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Attendance_Related));
                else
                    btnBack.Visible = false;
            }
            else
                btnBack.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string GetQuerystring()
    {
        string sTestDecrypt = string.Empty;
        try
        {
            sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());
            
			if (!QueryString["Is_Configured"].IsNull())
                hidIsConfig.Value = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
        return sTestDecrypt;
    }

    /// <summary>
    /// This method is used to fill holiday information into grid.
    /// </summary>
    private void FillHolidayListGridview()
    {
        
        HolidaysMasterBL oHolidaysMasterBL = new HolidaysMasterBL();
        SetGridViewDateColumnProperties();
        VisibleOrHideColumnsOfGridAsPerUserRole(false);
        grdHoliDaysManagement.DataSourceID = GrdDSobj.ID;
    }

    /// <summary>
    /// This method is used to visible or hide columns as per the logged in user roles.
    /// </summary>
    /// <param name="abIsVisible"></param>
    private void VisibleOrHideColumnsOfGridAsPerUserRole(bool abIsVisible)
    {
        if (moUserRole == Constants.UserRoles.Admin || ((moUserRole == Constants.UserRoles.Supervisor) || (moUserRole== Constants.UserRoles.Teacher)
            && (Convert.ToChar(hidCanEdit.Value) == Constants.C_YES)))       
        {
            grdHoliDaysManagement.Columns[I_COLUMN_INDEX_EDIT].Visible = !abIsVisible;
            grdHoliDaysManagement.Columns[I_COLUMN_INDEX_DELETE].Visible = !abIsVisible;
        }
        else
        {
            grdHoliDaysManagement.Columns[I_COLUMN_INDEX_EDIT].Visible = abIsVisible;
            grdHoliDaysManagement.Columns[I_COLUMN_INDEX_DELETE].Visible = abIsVisible;
        }

        if(moUserRole == Constants.UserRoles.Student)
            grdHoliDaysManagement.Columns[I_COLUMN_INDEX_STANDARDS].Visible = abIsVisible;
        else
            grdHoliDaysManagement.Columns[I_COLUMN_INDEX_STANDARDS].Visible = !abIsVisible;
    }

    /// <summary>
    /// This method is used to show/hide holiday add/edit button as per access rights.
    /// </summary>
    private void SetControlsAsPerViewMode()
    {
        if (moUserRole == Constants.UserRoles.Admin || ((moUserRole== Constants.UserRoles.Supervisor) || (moUserRole == Constants.UserRoles.Teacher)
            && (Convert.ToChar(hidCanEdit.Value) == Constants.C_YES)))        
        {
            btnAdd.Visible = true;
        }
        else
        {
            btnAdd.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to display start and end date in proper formate.
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        BoundField oStartDate = (BoundField)grdHoliDaysManagement.Columns[I_COLUMN_INDEX_START_DATE];
        BoundField oEndDate = (BoundField)grdHoliDaysManagement.Columns[I_COLUMN_INDEX_END_DATE];
        oEndDate.HtmlEncode = false;
        oStartDate.HtmlEncode = false;
        oStartDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        oEndDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method is used to set hidden field to default value for grid sorting.
    /// </summary>
    private void SetSortingFieldDefaultValues()
    {
        hidSortExpression.Value = "Holiday_Start_Date";
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to get upcoming holiday date.
    /// </summary>
    /// <returns></returns>
    private DateTime GetUpcomingHolidayDate()
    {
        if(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt()==Constants.UserRoles.Student.ToInt())        
            return HolidaysMasterBL.GetUpcomingHolidayDate(miSchoolId, miAcademicYearId,Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt(),Session[Constants.S_SESSION_STUDENT_DIVISION_ID].ToInt());
        else
            return HolidaysMasterBL.GetUpcomingHolidayDate(miSchoolId, miAcademicYearId, Constants.I_ZERO, Constants.I_ZERO);
    }

    #region configuration functions

    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;

        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.HolidaysManagement);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            pnlFields.Visible = false;
            btnAdd.Visible = false;
        }
        return bReturn;
    }

    private void RefreshValue()
    {
        hidAlertMessageForHoliday.Value = Resources.LocalizedResources.AlertMessageForHoliday;
        FillHolidayListGridview();
    }
   
    #endregion

    #endregion

}
