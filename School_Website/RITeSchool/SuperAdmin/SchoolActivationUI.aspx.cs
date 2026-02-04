/* File Name :- SchoolActivationUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 26-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display school list with allowed and exceeded SMS count.
 *                      Here we can activate/deactivate school as well as change allowed SMS count.
*/

using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;

public partial class SchoolActivationUI :SchoolBase
{
    #region Constants

    const int I_SCHOOL_NAME_COLUMN_INDEX = 0;    
    const int I_ACTIVATE_FLAG_COLUMN_INDEX = 5;
    const int I_SCHOOL_ID_COLUMN_INDEX = 6;
    
    const string S_IMG_FOR_ACTIVATE = "~/RITeSchool/images/Icon_UserLock.gif";
    const string S_IMG_FOR_DEACTIVATE = "~/RITeSchool/images/Icon_UserUnlock.gif";

    const int I_ACTIVATE_FLAG_IMG_COLUMN_INDEX = 1;
    const int I_EXCEEDED_SMS_COLUMN_INDEX = 3;
    const int I_LOGIN_COLUMN_INDEX = 7;

    #endregion

    #region Events

    /// <summary>
    /// This method is used set fill school details into grid and set default values to controls.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {                
                SetControlsDefaultValues();                
                DisplayMessageInbox();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back on dashboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
			Session[Constants.S_SESSION_SCHOOL_ID] = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
            SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; oSuperAdminMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save sms count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            const int I_ALLOWED_SMS_COLUMN_INDEX = 2;
            SchoolBL oSchoolBL;
            TextBox oNumericBox;
            int iAllowedSmsCount;
            int iSchoolId;
            foreach (GridViewRow oGridViewRow in grdvwSchoolList.Rows)
            {
                oSchoolBL = new SchoolBL();
                oNumericBox = (TextBox)oGridViewRow.Cells[I_ALLOWED_SMS_COLUMN_INDEX].FindControl("nTxtAllowedSmS");
                iAllowedSmsCount = Convert.ToInt32(oNumericBox.Text);
                iSchoolId = Convert.ToInt32(oGridViewRow.Cells[I_SCHOOL_ID_COLUMN_INDEX].Text);
                oSchoolBL.AllowedSMSCount = iAllowedSmsCount;
                oSchoolBL.SchoolId = iSchoolId;
                oSchoolBL.UpdateSchoolSMSCount(miAcademicYearId);
                lblSuccessMsg.Visible = true;
                lblSuccessMsg.Text = "SMS details saved successfully !!!";
            }
            DisplayMessageInbox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set attribute on gridview column.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSchoolList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {            
            if (e.Row.RowIndex >= 0)
            {                
                Image oActiveFlag = (Image)e.Row.Cells[I_ACTIVATE_FLAG_IMG_COLUMN_INDEX].Controls[0];
                System.Web.UI.WebControls.Button obtnLogin = (System.Web.UI.WebControls.Button)e.Row.Cells[I_LOGIN_COLUMN_INDEX].Controls[0];
                if (e.Row.Cells[I_ACTIVATE_FLAG_COLUMN_INDEX].Text.ToString() == Constants.C_NO.ToString())
                {
                    oActiveFlag.ToolTip = "Activate";
                    oActiveFlag.ImageUrl = S_IMG_FOR_ACTIVATE;//Activate button 
                    oActiveFlag.Attributes.Add("onclick", "if(!ConfirmAction('Are you sure you want to activate this school?')){return false;}");                    
                    e.Row.Font.Bold = false;
                    obtnLogin.Visible = false;
                }
                else
                {
                    oActiveFlag.ToolTip = "Deactivate";
                    oActiveFlag.ImageUrl = S_IMG_FOR_DEACTIVATE;// Deactivate button
                    oActiveFlag.Attributes.Add("onclick", "if(!ConfirmAction('Are you sure you want to deactivate this school?')){return false;}");
                    e.Row.Font.Bold = true;
                    obtnLogin.Visible = true;
                    ApplyMouseHoverEffect(new List<Button> { obtnLogin });
                }
                int iSentSMSCount = Convert.ToInt32(e.Row.Cells[I_EXCEEDED_SMS_COLUMN_INDEX].Text);
                if (iSentSMSCount > 0)
                    e.Row.Cells[I_EXCEEDED_SMS_COLUMN_INDEX].ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to sort the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSchoolList_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            DisplayMessageInbox();
            DataView oDataView = ((System.Data.DataView)grdvwSchoolList.DataSource);
            oDataView.Sort = e.SortExpression + " " + hidSortDirection.Value;
            VisibleOrHideColumnsOfInboxGrid(true);
            grdvwSchoolList.DataSource = oDataView;
            grdvwSchoolList.DataBind();
            VisibleOrHideColumnsOfInboxGrid(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method used to set page index to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSchoolList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwSchoolList.PageIndex = e.NewPageIndex;
            ViewState[Constants.S_DEFAULT_GRID_SORT] = null;
            hidSortExpression.Value = Constants.S_NOT_SPECIFIED;
            hidSortDirection.Value = Utility.Constants.S_DESCENDING;
            DisplayMessageInbox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add default sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSchoolList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            { 
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);
                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                Table PagerTable = (Table)e.Row.Cells[0].Controls[0];
                PagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to activate/deactivate and login to nselected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSchoolList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ACTIVATE_ROW"))
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                SchoolBL oSchoolBL = new SchoolBL();
                char cActivateFlag = Convert.ToChar(grdvwSchoolList.Rows[iRowIndex].Cells[I_ACTIVATE_FLAG_COLUMN_INDEX].Text);
                int iSchoolId = Convert.ToInt32(grdvwSchoolList.Rows[iRowIndex].Cells[I_SCHOOL_ID_COLUMN_INDEX].Text);
                if (cActivateFlag == Constants.C_NO)
                {
                    oSchoolBL.UpdateSchoolActivationFlag(iSchoolId);
                    hidIsActive.Value = "Y";
                }
                else
                {
                    oSchoolBL.UpdateSchoolDeActivationFlag(iSchoolId);
                    hidIsActive.Value = "N";
                }
                DisplayMessageInbox();

            }
            else if (e.CommandName.ToUpper().Equals("LOGIN"))
            {
                string sUserRole = "Admin";
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iSchoolId = Convert.ToInt32(grdvwSchoolList.Rows[iRowIndex].Cells[I_SCHOOL_ID_COLUMN_INDEX].Text);
                DataTable oDTSchoolDetails = SchoolBL.GetSchoolAcademicDetails(iSchoolId);
                DataRow oDR = oDTSchoolDetails.Rows[0];
                Session[Constants.S_SESSION_SCHOOL_ID] = Convert.ToInt32(grdvwSchoolList.Rows[iRowIndex].Cells[I_SCHOOL_ID_COLUMN_INDEX].Text);                
                Session[Constants.S_SESSION_SCHOOL_NAME] = grdvwSchoolList.Rows[iRowIndex].Cells[I_SCHOOL_NAME_COLUMN_INDEX].Text;                
                Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = Convert.ToInt32(oDR["Academic_Year_ID"].ToString());
                Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDR["Start_date"].ToString();
                Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDR["End_date"].ToString();
                DataTable oDTSchools;

                if (Session["S_SCHOOL_NAMES"] == null)
                    oDTSchools = MasterDataCollectionBL.GetAllSchools();
                else
                    oDTSchools = (DataTable)Session["S_SCHOOL_NAMES"];

                DataRow[] oArrRows = oDTSchools.Select("School_Id =" + iSchoolId);
                Session["S_SESSION_DEFAULT_MENU_ID"] = oArrRows[0]["Default_Menu_Id"].ToString();
                Session[Constants.S_SESSION_SUPERVISOR_ROLE_NAME_FIELD] = sUserRole;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, // Ticket version
                "Admin1", // Username associated with ticket
                DateTime.Now, // Date/time issued
                DateTime.Now.AddMinutes(30), // Date/time to expire
                true, // "true" for a persistent user cookie
                sUserRole, // User-data, in this case the roles
                FormsAuthentication.FormsCookiePath);// Path cookie valid for

                // Encrypt the cookie using the machine key for secure transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                   FormsAuthentication.FormsCookieName, // Name of auth cookie
                   hash); // Hashed ticket

                // Set the cookie's expiration time to the tickets expiration time
                if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

                // Add the cookie to the list for outgoing response
                Response.Cookies.Add(cookie);

                Response.Write("<Script language ='javascript'>window.open('../Common/ControlPanel.aspx','_blank'); </Script>");                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set values to control.
    /// </summary>
    private void SetControlsDefaultValues()
    {
        if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
            Session["I_SCHOOL_ID"] = null;
        grdvwSchoolList.PageSize = Constants.I_GRID_PAGE_COUNT;
        grdvwSchoolList.EmptyDataText = Constants.S_BLANK_GRID_MESSAGE;
        grdvwSchoolList.RowDataBound += new GridViewRowEventHandler(grdvwSchoolList_RowDataBound);
        grdvwSchoolList.RowCommand += new GridViewCommandEventHandler(grdvwSchoolList_RowCommand);
        hidSortExpression.Value = Constants.S_NOT_SPECIFIED;
        hidSortDirection.Value = Utility.Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
    }
    /// <summary>
    /// This method is used to visible/hide columns of the grid.
    /// </summary>
    /// <param name="abIsVisible"></param>
    private void VisibleOrHideColumnsOfInboxGrid(bool abIsVisible)
    {
        grdvwSchoolList.Columns[I_SCHOOL_ID_COLUMN_INDEX].Visible = abIsVisible;
        grdvwSchoolList.Columns[I_ACTIVATE_FLAG_COLUMN_INDEX].Visible = abIsVisible;
    }
    /// <summary>
    /// This method is used to display SMS details.
    /// </summary>
    private void DisplayMessageInbox()
    {
        SchoolBL oSchoolBL = new SchoolBL();
        VisibleOrHideColumnsOfInboxGrid(true);
        DataTable oDTSchoolLists = oSchoolBL.GetAllSchoolForActivation();
        grdvwSchoolList.DataSource = oDTSchoolLists.DefaultView;
        grdvwSchoolList.DataBind();
        VisibleOrHideColumnsOfInboxGrid(false);
        lblTotalCount.Text = Convert.ToString(oDTSchoolLists.Rows.Count);
    }

    /// <summary>
    /// This method is used to set default sort arrow.
    /// </summary>
    private void SetDefaultSortArrow()
    {
        if (ViewState[Constants.S_DEFAULT_GRID_SORT] == null && grdvwSchoolList.Rows.Count != Constants.I_ZERO)
        {
            GridViewRow oGridViewRow = grdvwSchoolList.HeaderRow;
            hidSortExpression.Value = Constants.S_DESCENDING;
            CommonUtility.AddSortImage(I_SCHOOL_NAME_COLUMN_INDEX, oGridViewRow, hidSortExpression.Value);
            ViewState[Constants.S_DEFAULT_GRID_SORT] = "true";
        }
    }
    #endregion  
}
