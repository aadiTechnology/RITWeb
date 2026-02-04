// File Name  : LockingUser.aspx.cs
// Created By : Anugandha
// Date       : 27/02/2008
//Description :This class is used to the activate or deactivate user as well to change it's password.

// Modified By : Sachin
// Modified Date : 16/07/2009
// Description : Integration of LockingUser and UserLoginUI.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Web.UI;
using SchoolEntities;

public partial class LockingUser : SchoolBase
{
    #region Constants    
   const int I_USER_ID = 0;
   const int I_NAME_COLUMN_INDEX = 1;
   const int I_USERNAME_COLUMN = 3;
   const int I_ACTIVATE_COLUMN = 4;
   const int I_PASSWORD_COLUMN = 5;
   const int I_SEND_SMS_COLUMN = 6;
   const int I_SMSMESSAGE_COLUMN = 7;
   const int I_LOGIN_COLUMN_ID = 8;
   const int I_IS_LOCKED = 1;

   const string S_DEACTIVATION_REASON = "Deactivation_Reason";
   const string S_SMSMESSAGE_REMOVE = "Are you sure you want to remove this user from the SMS & Message Center address book?";
   const string S_SMSMESSAGE_ADD = "Are you sure you want to add this user to the SMS & Message Center address book?";
   const string S_OTHERSTAFF_SMSMESSAGE_REMOVE = "Are you sure you want to remove this user from the SMS Center address book?";
   const string S_OTHERSTAFF_SMSMESSAGE_ADD = "Are you sure you want to add this user to the SMS Center address book?";
   const string S_IS_CONSIDEREDFORMESSAGE = "IsConsideredForMessage";
   const string S_SCREENS_URL = "ScreensUI.aspx";
   static string msFromUrl = string.Empty;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to set masterpage according to login user.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

           if (!IsPostBack)
                msFromUrl = GetFromPageUrl();
            
            string sFromPage = string.Empty;

            if(Request.QueryString.ToString() != string.Empty)
            {
                if(QueryString["FromPage"] != null)
                    sFromPage = QueryString["FromPage"];
			}

			if(msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL)
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
                
            if(sFromPage == S_SCREENS_URL)
				msFromUrl = sFromPage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user role combobox and hide login column according to login user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetSuperAdminView();
                if (CheckPreCondition())
                {
                    InitializeControls();
                    FillUserRoleCombo();
                    ReadQuerystring();
                }
                ddlUserRole.Focus();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    
                }
            }
            SetJavascriptAttributes();
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                FillUserGrid();
                lblSearch.Text = Resources.LocalizedResources.Name_RegNo_UserName;
                ReadQuerystring();
            }

            //Back button is hidden when user navigate to this screen from other screen except ScreensUI screen 
            if (hidBackUrl.Value != "../SuperAdmin/ScreensUI.aspx")
            {
                btnCancel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to search user details according to selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillUserGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///<Summary>
    ///This event is used to cancel the transaction.
    ///</Summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidBackUrl.Value == "../SuperAdmin/ScreensUI.aspx")
            {
                // Set the school details in the session and redirect to next page.                
				Session[Constants.S_SESSION_SCHOOL_ID] = ConfigurationManager.AppSettings["SchoolID"].ToInt();
                Response.Redirect(hidBackUrl.Value, false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region Grid Events

    /// <summary>
    /// This event is used to lock/unlock,login to user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("LOGIN")) // Login with selected user's user id.
            {
                int I_LOGIN_NAME_COLUMN_INDEX = 3;
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                string sLoginName = grdUsers.Rows[iRowIndex].Cells[I_LOGIN_NAME_COLUMN_INDEX].Text;

                UpdateSessionVariableAndRedirectToNextPage(miSchoolId, sLoginName);
            }
            else if(e.CommandName.ToUpper().Equals("SMS"))
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iUserId = Convert.ToInt32(grdUsers.DataKeys[iRowIndex][I_USER_ID].ToString());
                bool bIsConsideredForMessage = Convert.ToBoolean(grdUsers.DataKeys[iRowIndex][S_IS_CONSIDEREDFORMESSAGE]);
                SchoolUserBL.AddRemoveUserFromSmsMessageList(iUserId, !bIsConsideredForMessage,miSchoolId,miUserId);

                if(Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student)) 
                     grdUsers.DataSourceID = GrdODStudent.ID;
                else
                    grdUsers.DataSourceID = GrdDSobj.ID;
            }
            else if (e.CommandName == "SEND_SMS")
            {                
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iUserId = Convert.ToInt32(grdUsers.DataKeys[iRowIndex][I_USER_ID].ToString());

                SendLoginSMSToUser(iUserId);                
                lblMessage.Text = Resources.LocalizedResources.SentSMSMsg;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort expression.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student) && e.SortExpression == "Name")
                e.SortExpression = e.SortExpression.Replace("Name", "First_Name " + hidSortDirection.Value);
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event sets properties to grid's column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                Button btn = e.Row.FindControl("btnLogin") as Button;
                ApplyMouseHoverEffect(new List<Button> { btn });
                int iRowindex = e.Row.RowIndex;
                ImageButton imgActivate = (ImageButton)(e.Row.Cells[I_ACTIVATE_COLUMN].Controls[0]);
                ImageButton imgPassword = (ImageButton)(e.Row.Cells[I_PASSWORD_COLUMN].Controls[0]);
                ImageButton imgSmsMessage = (ImageButton)(e.Row.Cells[I_SMSMESSAGE_COLUMN].Controls[0]);
                ImageButton imgSendSMS = (ImageButton)(e.Row.Cells[I_SEND_SMS_COLUMN].Controls[0]);

                int iUserId = Convert.ToInt32(grdUsers.DataKeys[iRowindex][0].ToString());
                string sUserName = grdUsers.DataKeys[iRowindex][2].ToString();
                string sMobileNumber = grdUsers.DataKeys[iRowindex][3].ToString();
                string sIsLocked = grdUsers.DataKeys[iRowindex][I_IS_LOCKED].ToString();
                string sDeactivationReson = grdUsers.DataKeys[iRowindex][S_DEACTIVATION_REASON].ToString();
                bool bIsConsideredForMessage = Convert.ToBoolean(grdUsers.DataKeys[iRowindex][S_IS_CONSIDEREDFORMESSAGE]);
				if (Settings.IsMiniSite)
				{
					if (e.Row.RowType == DataControlRowType.DataRow)
					{
						e.Row.Cells[5].Visible = false;
					}
				}

                string sQueryString = "User_Id=" + iUserId;
                string sEncryptUserId = Utility.CommonUtility.EncryptQuerystring(sQueryString);

                imgPassword.Attributes.Add("onclick", "window.open('../Admin/ChangePasswordPopUp.aspx?" +
                    sEncryptUserId + "', '_new','scrollbars=yes,resizable=no,top=0,left=0,width=730,height=370px');return false;");

                string sString = "User_Id=" + iUserId.ToString() + "&UserName=" + sUserName + "&FromPage=" + msFromUrl +
                         "&UserRoleId=" + ddlUserRole.SelectedValue + "&Mobile_Number=" + sMobileNumber + "&IsLocked=" + sIsLocked + "&NameFilter=" + txtSearch.Text +
                         "&StandarId=" + ddlStandard.SelectedValue + "&DivisionId=" + ddlDivision.SelectedValue + "&Deactivation_Reason=" + sDeactivationReson + "&UserTypeId=" + ddlUserType.SelectedValue;
                string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sString);

                if (sIsLocked == Convert.ToString(Constants.C_NO))
                {
                    imgActivate.ToolTip = Resources.LocalizedResources.Deactivate;
                    imgActivate.ImageUrl = "~/RITeSchool/images/Icon_UserUnlock.gif";

                    imgActivate.Attributes.Add("onclick", "window.open('../Admin/UserDeactivePopUp.aspx?" + sEncryptedString
                                                                      + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=675,height=370');return false;");
                }
                else
                {
                    imgActivate.ToolTip = Resources.LocalizedResources.Activate;
                    imgActivate.ImageUrl = "~/RITeSchool/images/Icon_UserLock.gif";
                    imgActivate.Attributes.Add("onclick", "window.open('../Admin/UserDeactivePopUp.aspx?" + sEncryptedString
                                                                      + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=675,height=370');return false;");

                }

                imgPassword.Attributes.Add("onclick", "window.open('../Admin/ChangePasswordPopUp.aspx?" +
                    sEncryptUserId + "', '_new','scrollbars=yes,resizable=no,top=0,left=0,width=730,height=370px');return false;");

                if(bIsConsideredForMessage)
                {
                    if (Convert.ToInt32(ddlUserRole.SelectedValue) != Convert.ToInt32(Constants.UserRoles.OtherStaff))
                    {
                        imgSmsMessage.ToolTip = Resources.LocalizedResources.RemoveSMSMessageList;
                        imgSmsMessage.ImageUrl = "~/RITeSchool/images/IconGrid_Mail.jpg";
                        imgSmsMessage.Attributes.Add("onclick", "if(!ConfirmSmsMessage('" + Resources.LocalizedResources.MsgRemoveUserFromSMSMessage + "')){return false;}");
                    }
                    else
                    {
                        imgSmsMessage.ToolTip = Resources.LocalizedResources.RemoveSMSList;
                        imgSmsMessage.ImageUrl = "~/RITeSchool/images/IconGrid_Mail.jpg";
                        imgSmsMessage.Attributes.Add("onclick", "if(!ConfirmSmsMessage('" + Resources.LocalizedResources.MsgRemoveUserFromSMSAddressBook + "')){return false;}");
                    }
                }
                else
                {
                    if (Convert.ToInt32(ddlUserRole.SelectedValue) != Convert.ToInt32(Constants.UserRoles.OtherStaff))
                    {
                        imgSmsMessage.ToolTip = Resources.LocalizedResources.AddSMSMessageList;
                        imgSmsMessage.ImageUrl = "~/RITeSchool/images/IconGrid_Mail.gif";
                        imgSmsMessage.Attributes.Add("onclick", "if(!ConfirmSmsMessage('" + Resources.LocalizedResources.MsgAddUserToSMSMessage + "')){return false;}");
                    }
                    else
                    {
                        imgSmsMessage.ToolTip = Resources.LocalizedResources.AddSMSList;
                        imgSmsMessage.ImageUrl = "~/RITeSchool/images/IconGrid_Mail.gif";
                        imgSmsMessage.Attributes.Add("onclick", "if(!ConfirmSmsMessage('" + Resources.LocalizedResources.MsgAddUserToSMSAddressBook + "')){return false;}");

                    }                    
                }

                if (sIsLocked == Convert.ToString(Constants.C_NO))
                {
                    if (bIsConsideredForMessage)
                        imgSendSMS.Attributes.Add("onclick", "if(!ConfirmSmsMessage('" + Resources.LocalizedResources.ConfirmSendSMS + "')){return false;}");
                    else
                        imgSendSMS.Attributes.Add("onclick", "ShowAlert('" + Resources.LocalizedResources.ActivateSMS + "'); return false;");
                }
                else
                {
                    if (bIsConsideredForMessage)
                        imgSendSMS.Attributes.Add("onclick", "ShowAlert('" + Resources.LocalizedResources.ActivateUser + "'); return false;");
                    else
                        imgSendSMS.Attributes.Add("onclick", "ShowAlert('" + Resources.LocalizedResources.ActivateUserAndSMS + "'); return false;");
                }
                imgSendSMS.ToolTip = Resources.LocalizedResources.SendLoginSMS;
            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;
                // Retrieve the DropDownList and Label controls from the row.
                DropDownList ddlPageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label lblCurrentPage = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (ddlPageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    int iPageNo = 0;
                    for (int iPageNumber = 0; iPageNumber < grdUsers.PageCount; iPageNumber++)
                    {
                        // Create a ListItem object to represent a page.
                        iPageNo = iPageNo + 1;
                        ListItem itemPageNo = new ListItem(iPageNo.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.                        
                        if (iPageNo == grdUsers.PageIndex + 1)
                        {
                            itemPageNo.Selected = true;
                        }

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        ddlPageList.Items.Add(itemPageNo);
                    }
                }
                if (lblCurrentPage != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdUsers.PageIndex + 1;

                    // Update the Label control with the current page information.
                    lblCurrentPage.Text = Resources.LocalizedResources.PageNo + currentPage.ToString() + " "+
                      Resources.LocalizedResources.Of + " "+ grdUsers.PageCount.ToString() + " " + Resources.LocalizedResources.OutOflst;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort imaege.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView grdUser = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int iSortColumnIndex;
                if (Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student))
                    iSortColumnIndex = CommonUtility.GetSortColumnIndex(grdUser, grdUser.SortExpression);
                else
                    iSortColumnIndex = CommonUtility.GetSortColumnIndex(grdUser, hidSortExpression.Value);

                if (iSortColumnIndex != -1)
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
                else
                    CommonUtility.AddSortImage(1, e.Row, hidSortDirection.Value);
				if (Settings.IsMiniSite)
					if (e.Row.RowType == DataControlRowType.Header)
						e.Row.Cells[5].Visible = false;
				
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill gridview after changing page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdUsers.PageIndex = e.NewPageIndex;
            FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change gridview page index according to index of paging combbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdUsers.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList ddlPageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdUsers.PageIndex = ddlPageList.SelectedIndex;
            FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
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
                lblStartIndex.Text = Convert.ToString((grdUsers.PageSize * grdUsers.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdUsers.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0" || grdUsers.PageCount == 0)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
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

    #endregion

    #region CoboBox Event

    /// <summary>
    /// This event is used to fill standard combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdUsers.PageIndex = 0;
            grdUsers.Columns[0].Visible = false;
            pnlForStudent.Visible = false;            
            ddlDivision.Items.Clear();
            SetDefaultSortArrow();

            if (ddlUserRole.SelectedIndex == 0)
            {
                pnlUserGrid.Visible = false;
                txtSearch.Text = string.Empty;
                tblSearch.Visible = false;
                pnlForUserType.Visible = false;
            }
            else if (Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student))
            {
                pnlForUserType.Visible = true;
                pnlForStudent.Visible = true;
                pnlUserGrid.Visible = true;
                ddlStandard.Visible = true;
                FillStandardCombo();
                FillUserGrid();
                tblSearch.Visible = true;
                lblSearch.Text = Resources.LocalizedResources.Name_RegNo_UserName;
                grdUsers.Columns[0].Visible = true;
                tblSearch.Width = Unit.Percentage(60).ToString();
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
                SetEmptyDataText();
            }
            else
            {
                pnlForUserType.Visible = true;
                tblSearch.Visible = true;
                lblSearch.Text = Resources.LocalizedResources.Name_UserName;
                tblSearch.Width = Unit.Percentage(52).ToString();
                pnlUserGrid.Visible = true;
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
                SetEmptyDataText();
            }
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
            grdUsers.PageIndex = 0;
            grdUsers.Columns[0].Visible = false;
            FillUserGrid();
            FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
            grdUsers.PageIndex = 0;
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
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            ddlDivision.Visible = true;
            trTotalRec.Visible = false;
            FillDivisionCombobox(iStandardId);
            if (ddlStandard.SelectedIndex != 0)
            {
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
                grdUsers.PageIndex = 0;
                hidStandardId.Value = ddlStandard.SelectedValue;
                hidDivisionId.Value = ddlDivision.SelectedValue;
            }
            else
            {
                hidStandardId.Value = "0";
                hidDivisionId.Value = "0";
                trTotalRec.Visible = false;                
                ListItem olstDivision = new ListItem();
                olstDivision.Text = "-- All --";
                ddlDivision.Items.Add(olstDivision);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to display grid with respected standard and division.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidDivisionId.Value = ddlDivision.SelectedValue;
            SetDefaultSortArrow();
            FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
            grdUsers.PageIndex = 0;
            trTotalRec.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to  toggle(show/hide) columns for other staff. 
    /// </summary>
    /// <param name="abflag"></param>
    private void ToggleColumnsforOtherstaff(bool abflag)
    {
        grdUsers.Columns[I_USERNAME_COLUMN].Visible = abflag;
        grdUsers.Columns[I_ACTIVATE_COLUMN].Visible = abflag;
        grdUsers.Columns[I_PASSWORD_COLUMN].Visible = abflag;
        grdUsers.Columns[I_SEND_SMS_COLUMN].Visible = abflag;
    }

    /// <summary>
    /// This method is used to fill grid with selected user.
    /// </summary>
    private void FillUserGrid()
    {
        grdUsers.PageIndex = 0;
        if (ddlStandard.SelectedIndex == 0)
        {
            hidStandardId.Value = "0";
            hidDivisionId.Value = "0";
        }
        if (!string.IsNullOrEmpty(ddlUserRole.SelectedValue))
        {
            grdUsers.DataSourceID = Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student) ? GrdODStudent.ID : GrdDSobj.ID;
        }
        SetEmptyDataText();
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeControls()
    {
        // Set default button property.
        System.Web.UI.HtmlControls.HtmlForm oForm = (System.Web.UI.HtmlControls.HtmlForm)this.Master.FindControl("Form1");
        oForm.DefaultButton = btnSearch.UniqueID;
        grdUsers.Columns[0].Visible = false;
    }

    /// <summary>
    /// This method is used to fill user role combo.
    /// </summary>
    private void FillUserRoleCombo()
    {
        // Fill the user role's combobox with all the user roles available in the system.
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSStateCollection = oMasterDataCollectionBL.GetAllUserRolesExceptAdmin();
		if (Settings.IsMiniSite)
		{
			DataTable oDtMiniVersionUsers = new DataTable();
			oDSStateCollection = oDSStateCollection.Select("User_Role_Id in(2,6) ").CopyToDataTable();
		}
        DataRow[] oDataRow = new DataRow[0];
        oDataRow = oDSStateCollection.Select("User_Role_Id=6");
        if (oDataRow.Length > 0)
            oDataRow[0][Constants.S_USER_ROLE_NAME_FIELD] = Convert.ToString(Constants.S_SUPERVISOR_ROLE_NAME);
        if (msFromUrl.Equals(S_SCREENS_URL) && !Settings.EnableOtherStaffLogin)
        {
            oDataRow = oDSStateCollection.Select("User_Role_Id=7");
            if (oDataRow.Length > 0)
                oDSStateCollection.Rows.Remove(oDataRow[0]);
        }
        else
        {
            oDataRow = oDSStateCollection.Select("User_Role_Id=7");
            if (oDataRow.Length > 0)
                oDataRow[0][Constants.S_USER_ROLE_NAME_FIELD] = "Other Staff";
        }

        if (msFromUrl.Equals(S_SCREENS_URL) && !Settings.EnableTransportStaffForActiveDeactive)
        {
            oDataRow = oDSStateCollection.Select("User_Role_Id=8");
            if (oDataRow.Length > 0)
                oDSStateCollection.Rows.Remove(oDataRow[0]);
        }
        else
        {
            oDataRow = oDSStateCollection.Select("User_Role_Id=8");
            if (oDataRow.Length > 0)
                oDataRow[0][Constants.S_USER_ROLE_NAME_FIELD] = "Transport Staff";
        }

        oDSStateCollection = oDSStateCollection.Select("User_Role_Id <> 10").CopyToDataTable();

        ControlUtility.FillDropDownList(oDSStateCollection, ref ddlUserRole,
                                        Constants.S_USER_ROLE_ID_FIELD,
                                       Constants.S_USER_ROLE_NAME_FIELD,
                                       Constants.S_SELECT);

    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSearch });
    }

    /// <summary>
    /// This method is used to show login column.
    /// </summary>
    private void SetSuperAdminView()
    {        
        if (msFromUrl.Equals(S_SCREENS_URL))
        {
            hidBackUrl.Value = "../SuperAdmin/ScreensUI.aspx";
            grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = true;
        }
        tblSearch.Visible = false;
        trTotalRec.Visible = false;
    }    

    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
        YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();        
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       "-- All --");

        // Add item into division combobox.
        ListItem olstDivision = new ListItem();
        olstDivision.Text = "-- All --";
        ddlDivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox(int aiStandardId)
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
    }

    /// <summary>
    /// This method is used to fill grid with particular user.
    /// </summary>
    /// <param name="aiUserRoleId"></param>
    /// <param name="sSortExpression"></param>
    private void FillUserDetailGrid(int aiUserRoleId, String sSortExpression)
    {        
        SchoolUserCollectionBL oSchoolUserCollectionBL = new SchoolUserCollectionBL();        
        if (aiUserRoleId == Convert.ToInt32(Constants.UserRoles.Student))
        {
            if (grdUsers.SortExpression.Contains("Name"))
            {
                grdUsers.Columns[I_NAME_COLUMN_INDEX].SortExpression = "First_Name";
                hidSortExpression.Value = "First_Name";
            }                      
            grdUsers.Columns[0].Visible = true;
            ToggleColumnsforOtherstaff(true);
            grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = true;
            grdUsers.DataSourceID = GrdODStudent.ID;
        }
        else
        {
            if (grdUsers.SortExpression.Contains("First_Name"))
            {
                grdUsers.Columns[I_NAME_COLUMN_INDEX].SortExpression = "Name";
                hidSortExpression.Value = "Name";
            }
            ToggleColumnsforOtherstaff(true);            
            if (aiUserRoleId == Convert.ToInt32(Constants.UserRoles.OtherStaff) && !Settings.EnableOtherStaffLogin)
            {
                grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = false;
                ToggleColumnsforOtherstaff(false);
            }
            else if (aiUserRoleId == Convert.ToInt32(Constants.UserRoles.TransportStaff) && Settings.EnableTransportStaffForActiveDeactive)
                SetControlsForTransportStaff();               
            
            grdUsers.DataSourceID = GrdDSobj.ID;
        }
        if (msFromUrl.Equals(S_SCREENS_URL))
        {
            hidBackUrl.Value = "../SuperAdmin/ScreensUI.aspx";
            if (ddlUserRole.SelectedValue.ToInt() != Constants.UserRoles.OtherStaff.ToInt() || (ddlUserRole.SelectedValue.ToInt() == Constants.UserRoles.OtherStaff.ToInt() && Settings.EnableOtherStaffLogin))
            {
                if(ddlUserRole.SelectedValue.ToInt() != Constants.UserRoles.TransportStaff.ToInt())
                    grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = true;
                else
                    grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = false;
            }
        }
        else
            grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = false;
    }

    /// <summary>
    /// This method is used to set empty data text to grid.
    /// </summary>
    private void SetEmptyDataText()
    {
        if (ddlUserRole.SelectedValue == Convert.ToString(Constants.UserRoles.Student.ToInt()))
            grdUsers.EmptyDataText = Resources.LocalizedResources.NoStudentAvaliable;
        else if (ddlUserRole.SelectedValue == Convert.ToString(Constants.UserRoles.Supervisor.ToInt()))
            grdUsers.EmptyDataText = Resources.LocalizedResources.NoStaffAvailable.Replace("%replace%", Constants.S_SUPERVISOR_ROLE_NAME.Replace("Admin Staff", Resources.LocalizedResources.AdminStaff));
        else if (ddlUserRole.SelectedValue == Convert.ToString(Constants.UserRoles.OtherStaff.ToInt()))
            grdUsers.EmptyDataText = Resources.LocalizedResources.NoOtherStaffAvaliable;
        else
            grdUsers.EmptyDataText = Resources.LocalizedResources.NoTeacherAvailable;
    }

    /// <summary>
    /// This function is used to set initial values to sort variables.
    /// </summary>
    private void SetDefaultSortArrow()
    {

        if (ddlUserRole.SelectedValue == Convert.ToString(Constants.UserRoles.OtherStaff.ToInt()))
            grdUsers.Columns[I_SMSMESSAGE_COLUMN].HeaderText = Resources.LocalizedResources.ActivateDeactivateSMS;
        else
            grdUsers.Columns[I_SMSMESSAGE_COLUMN].HeaderText = Resources.LocalizedResources.ActivateDeactivateSMSMessage;

        if (ddlUserRole.SelectedValue == Convert.ToString(Constants.UserRoles.Student.ToInt()))
        {
            grdUsers.Columns[I_NAME_COLUMN_INDEX].SortExpression = "Name";
            grdUsers.Columns[I_NAME_COLUMN_INDEX].HeaderText = Resources.LocalizedResources.Name;
        }
        else
        {
            if (Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Teacher) ||
                Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Supervisor))
            {
                grdUsers.Columns[I_NAME_COLUMN_INDEX].SortExpression = "Name";
                grdUsers.Columns[I_NAME_COLUMN_INDEX].HeaderText = Resources.LocalizedResources.Name_Designation;
            }
            else
            {
                grdUsers.Columns[I_NAME_COLUMN_INDEX].SortExpression = "Name";
                grdUsers.Columns[I_NAME_COLUMN_INDEX].HeaderText = Resources.LocalizedResources.Name;
            }

            hidSortExpression.Value = grdUsers.Columns[I_NAME_COLUMN_INDEX].SortExpression;
        }
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This function checks the preconditons of WeekdayTimeTable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.UserMangement);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            trPrecondition.Visible = true;
            VisibleOrHideControls();
        }
        return bReturn;

    }

    /// <summary>
    /// This method is used to show or hide controls depends configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        pnlForStudent.Visible = false;        
        pnlLegend.Visible = false;
        btnCancel.Text = "Back";
        trUserRole.Visible = false;
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        pnlForUserType.Visible = false;
        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
        {
            if (QueryString["UserRoleId"] != null)
            {
                int iUserRoleId = QueryString["UserRoleId"].ToInt();
                ddlUserRole.SelectedValue = iUserRoleId.ToString();
                if (QueryString["NameFilter"] != null)
                {
                    tblSearch.Visible = true;
                    txtSearch.Text = QueryString["NameFilter"];
                    lblSearch.Text = Resources.LocalizedResources.Name_UserName;
                    btnSearch.Visible = true;
                }

                if (iUserRoleId == Constants.UserRoles.TransportStaff.ToInt() && Settings.EnableTransportStaffForActiveDeactive)
                    SetControlsForTransportStaff();

               if (QueryString["UserTypeId"] != null)
                {
                    pnlForUserType.Visible = true;
                    ddlUserType.SelectedValue = QueryString["UserTypeId"].ToString();
                }
                
                if (ddlUserRole.SelectedValue.ToInt() == Constants.UserRoles.Student.ToInt() &&
                    QueryString["StandarId"] != null && QueryString["DivisionId"] != null)
                {
                    pnlForStudent.Visible = true;
                    FillStandardCombo();
                    ddlStandard.SelectedValue = QueryString["StandarId"];
                    if (QueryString["StandarId"].ToInt() != 0)
                    {
                        FillDivisionCombobox(QueryString["StandarId"].ToInt());
                        ddlDivision.SelectedValue = QueryString["DivisionId"];
                    }
                    hidStandardId.Value = ddlStandard.SelectedValue;
                    hidDivisionId.Value = ddlDivision.SelectedValue;
                    lblSearch.Text = Resources.LocalizedResources.Name_RegNo_UserName;
                    grdUsers.Columns[0].Visible = true;
                }
                else
                {
                    pnlForStudent.Visible = false;
                    hidStandardId.Value = "0";
                    hidDivisionId.Value = "0";
                }
                SetDefaultSortArrow();
                FillUserGrid();
            }
        }
    }

    /// <summary>
    /// This method is used to send Login SMS TO User.
    /// <param name="iUserId"></param>
    /// </summary>
    private void SendLoginSMSToUser(int iUserId)
    {
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        UserDetailsForLoginSMS oUserDetailsForLoginSMS = oSchoolUserBL.GetUserDetailsForLogin(miSchoolId, miAcademicYearId, iUserId);
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
        int iSMSType = 0;
        string sPassword = string.Empty;
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        
        if (oDTSmsTemplate.Rows.Count != 0)
        {
            if (oUserDetailsForLoginSMS.UserLogin.ToString() != string.Empty)
            {
                sPassword = CommonUtility.GetDecryptedPassword(oUserDetailsForLoginSMS.UserLogin.ToLower(), oUserDetailsForLoginSMS.Password);
            }
            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
            {
                sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", oUserDetailsForLoginSMS.UserLogin).Replace("%PASSWORD%", sPassword);

                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();
            }
            if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
        }

        DataTable oDataTable = SchoolUserCollectionBL.GetPasswordRecoveryDetails(oUserDetailsForLoginSMS.UserId, miSchoolId);

        if (oDataTable.Rows.Count > 0)
        {
            SMS oSMS = new SMS();
            oSMS.SchoolID = oSchoolBL.SchoolId;
            oSMS.AcademicYearID = Convert.ToInt32(oDataTable.Rows[0]["Academic_Year_ID"]);
            oSMS.SenderID = Convert.ToInt32(oDataTable.Rows[0]["AdminUserId"]);
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + " :: Forgot Password";
            oSMS.DisplayText = Convert.ToString(oDataTable.Rows[0]["UserName"]);
            oSMS.SMSType = iSMSType;
            oSMS.SMSTypeId = Constants.SMSTypes.ForgotPasswordDetailSMS.ToInt();
            oSMS.To.Add(oUserDetailsForLoginSMS.UserId, oUserDetailsForLoginSMS.MobileNumber);
            if (oUserDetailsForLoginSMS.MobileNumber1 != string.Empty)
                oSMS.To.Add(oUserDetailsForLoginSMS.UserId + "sm;", oUserDetailsForLoginSMS.MobileNumber1);

            oSMS.SendLoginSMS(miSchoolId,miAcademicYearId,iUserId);
            oSMS = null;
        }      
    }

    /// <summary>
    /// This method is used to set control visibility for Transport staff.
    /// <param name="iUserId"></param>
    /// </summary>
    private void SetControlsForTransportStaff()
    {
        grdUsers.Columns[I_USERNAME_COLUMN].Visible = false;
        grdUsers.Columns[I_PASSWORD_COLUMN].Visible = false;
        grdUsers.Columns[I_SEND_SMS_COLUMN].Visible = false;
        grdUsers.Columns[I_SMSMESSAGE_COLUMN].Visible = false;
        grdUsers.Columns[I_LOGIN_COLUMN_ID].Visible = false;
    }

    #endregion
}
