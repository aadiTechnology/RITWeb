/*
 *  File Name : - SelectUserName.aspx.cs
 *  Purpose   : - This class is used to display all user name and their ids to select to send
 *                message to them.
 *  Date      : - 18-May-2007
 */

using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SchoolEntities.Teacher;

public partial class SelectUserName : SchoolBase
{

    #region Constants

    const int I_USERNAME_COLUMN_INDEX = 1;
    const int I_EMAILID_COLUMN_INDEX = 2;
    const int I_USERID_COLUMN_INDEX = 2;
    const int I_SUPPLIER_ID_COLUMN_INDEX = 3;
    const int I_SUPPLIER_EMAIL_ID_COLUMN_INDEX = 2;
    const int I_SUPPLIER_TYPE_COLUMN_INDEX = 5;
    const int I_SUPPLIER_NAME_COLUMN_INDEX = 1;
    const string S_ALL = "All";
    const string S_CHECK_BOX_SELECT = "ChkBoxSelect";
    const string S_SELECT_AT_LEAST_ONE_USER = "No user is selected. Are you sure you want to continue?";
    const string S_TEACHER_SORT = "SortOrder,Teacher_First_Name";
    const string S_PARENT_TEACHER_SORT = "DesignationId,Name";
    const string S_SUPERVISOR_SORT = "SortOrder,Supervisor_First_Name";
    const string S_OTHER_STAFF_SORT = "SortOrder,FirstName";
    const string S_STDDIV_SORT = "Original_Standard_Id , Original_Division_Id";
    const string S_STDDIV_SORT_DESC = "Original_Standard_Id desc, Original_Division_Id";
    const string S_ROLL_SORT = "Roll_No";
    const int I_USER_ID_INDEX = 2;
    const string S_LEFT_STUDENTS = "LeftStudents";

    #endregion

    #region DataMember

    string sUserName = string.Empty;
    string sUserId = string.Empty;

    #endregion

    #region properties

    public bool IsLeftStudent
    {
        get
        {
            if (hidIsLeftStudents.Value == Constants.S_YES)
                return true;
            else
                return false;
        }
    }        

    #endregion

    #region Events

    /// <summary>
    /// This method is used to initialize on page load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            GridViewProperties();
            lblErrorMsg.Text = string.Empty;
            if (!IsPostBack)
            {   
                if (!QueryString["IsCc"].IsNull())
                {
                    hidIsCc.Value = "1";
                }

                if (moUserRole == Constants.UserRoles.Teacher && !QueryString["Mode"].IsNull())
                    if (QueryString["Mode"] == "SMS")
                    {
                        char cAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter);
                        hidUserHasFullAccess.Value = (cAccess == Constants.C_YES ? true : false).ToString();
                    }
                    else if (QueryString["Mode"] == "Message")
                    {
                        char cAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.MessageCenter);
                        hidUserHasFullAccess.Value = (cAccess == Constants.C_YES ? true : false).ToString();
                    }

                SetDefaultSortGridArrow();
                FillGridsAccordingToRequest();
                InitalizePageControls();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// On this event submit string of user name and their ids to new message mpage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnOk_Click(object sender, EventArgs e)
    {
        try
        {
            ManageSelectedUsersList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to search by reg no or name
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {           
            DataTable oDataSet = null;
            if (moUserRole == Constants.UserRoles.Teacher)
            {
                string asStandardDivisionIds = string.Empty;
                foreach (ListItem oListItem in DDListStdDiv.Items)
                    asStandardDivisionIds += oListItem.Value + ",";
                if (asStandardDivisionIds != string.Empty)
                    asStandardDivisionIds = asStandardDivisionIds.Substring(0, asStandardDivisionIds.LastIndexOf(","));
                if (IsLeftStudent == false)
                    oDataSet = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, asStandardDivisionIds, miAcademicYearId, txtName.Text.Trim());
                else
                    oDataSet = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, 0, miAcademicYearId, txtName.Text.Trim(), cmbType.SelectedValue.ToInt(), IsLeftStudent);
            }
            else
            {
                oDataSet = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, 0, miAcademicYearId, txtName.Text.Trim(), cmbType.SelectedValue.ToInt(), IsLeftStudent);
            }
            FillGridWithUserDetails(oDataSet);
            hidIsIndivisualStudentId.Value = "Y";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to change student radio button selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoStudentReg_CheckedChanged(object sender, EventArgs e)
    {
        try
        {            
             DDListStdDiv.SelectedIndex = 0;
             ChangeCntrlAccesiblity(true);
             grdvwSelectUser.DataBind();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to change student std div radio button selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoStdDiv_CheckedChanged(object sender, EventArgs e)
    {
        try
        {            
            txtName.Text = string.Empty;
            ChangeCntrlAccesiblity(false);
            grdvwSelectUser.DataBind();            
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to fetch the students od the selected Std Div Id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDListStdDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (IsLeftStudent)
            {
                SetDefaultSortGridArrow();
                int iStdDivID = Convert.ToInt32(DDListStdDiv.SelectedValue);
                FillControlsWithSelectedStdDivID(iStdDivID);
            }
            else
            {
                if (DDListStdDiv.SelectedIndex != 0)
                {
                    SetDefaultSortGridArrow();
                    int iStdDivID = Convert.ToInt32(DDListStdDiv.SelectedValue);
                    FillControlsWithSelectedStdDivID(iStdDivID);
                }
                else
                    grdvwSelectUser.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillGridsAccordingToRequest();
        }        
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    #endregion

    #region GridView Events

    /// <summary>
    /// This event is used for selecting index changing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSelectUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwSelectUser.PageIndex = e.NewPageIndex;
            DisplayAllUserName();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event occurs to sort columns the of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSelectUser_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;
            
			if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
            {               
                if (moUserRole == Constants.UserRoles.Admin && QueryString["IsStudentLevel"].IsNull())
                {
	                if (hidSortDirection.Value.Equals(Constants.S_DESCENDING))
		                hidSortExpression.Value = S_STDDIV_SORT_DESC;
	                else
		                hidSortExpression.Value = S_STDDIV_SORT;
                }
            }
            DisplayAllUserName();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event occurs to bound data with each row of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSelectUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                CheckBox chkSelect = ((CheckBox)e.Row.FindControl("ChkBoxSelect"));
                Label oLabel = (Label)e.Row.Cells[I_USERNAME_COLUMN_INDEX].FindControl("lblUName");
                //Check select check box if current row user is already selected
                if (QueryString["IsCc"].IsNull())
                {
                    string[] sArrUserIdList = hidSelectedUserId.Value.Split(';');
                    foreach (string sUserid in sArrUserIdList)
                    {
                        if (grdvwSelectUser.DataKeys[e.Row.RowIndex]["ID"].ToString() == sUserid.Trim())
                        {
                            chkSelect.Checked = true;
                            hidUserIds.Value += sUserid.Trim() + "||";
                            hidUserNames.Value += oLabel.Text.Trim() + "||";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(hidSelectedUserIdCc.Value))
                    {
                        string[] sArrUserIdListCc = hidSelectedUserIdCc.Value.Split(';');
                        foreach (string sUserid in sArrUserIdListCc)
                        {
                            if (grdvwSelectUser.DataKeys[e.Row.RowIndex]["ID"].ToString() == sUserid.Trim())
                            {
                                chkSelect.Checked = true;
                                hidUserIdsCc.Value += sUserid.Trim() + "||";
                                hidUserNamesCc.Value += oLabel.Text.Trim() + "||";
                            }
                        }
                    }
                }
                chkSelect.Attributes.Add("onclick", "RemoveUser('" + oLabel.Text + "','" + Convert.ToInt32(grdvwSelectUser.DataKeys[e.Row.RowIndex]["ID"]) + "','" + e.Row.RowIndex + "');");
            }

            if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || Boolean.Parse(hidUserHasFullAccess.Value) || (hidIsPTAMember.Value == Constants.S_YES))
                    && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString())
                        && ((QueryString["IsStudentLevel"].IsNull() || QueryString["IsPTAStudentLevel"].IsNull())))
            
            {
                if (!IsLeftStudent)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        int iTypeId = Constants.I_ZERO;
                        if (miSchoolId == Constants.SchoolId.SPS.ToInt())
                            iTypeId = cmbType.SelectedValue.ToInt();

                        int iStandardDivisionId = Convert.ToInt32(grdvwSelectUser.DataKeys[e.Row.RowIndex]["ID"]);
                        string sUrl = "SelectUserName.aspx?" + CommonUtility.EncryptQuerystring(Request.QueryString.ToString() + "&IsStudentLevel=Y&iStandardDivisionId=" + iStandardDivisionId.ToString() +
                                                             "&UsersList=" + Constants.UserRoles.Student.ToString() + "&TypeId=" + iTypeId+"&IsPTAStudentLevel=Y");
                        Label oLabel = (Label)e.Row.Cells[I_USERNAME_COLUMN_INDEX].FindControl("lblUName");
                        oLabel.Text = "<A href='" + sUrl + "'>" + oLabel.Text + "</A>";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdvwSelectUser_RowCreated(object sender, GridViewRowEventArgs e)
    {
        // Use the RowType property to determine whether the 
        // row being created is the header row. 
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = I_USERNAME_COLUMN_INDEX;

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
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

    #endregion

    # region Helping Methods for Select buyer

    /// <summary>
    /// Set grid view sorting depending on user selection.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        if (!QueryString["UsersList"].IsNull())
        {
            if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
                hidSortExpression.Value = S_SUPERVISOR_SORT;
            
			if (QueryString["UsersList"] == Constants.UserRoles.OtherStaff.ToString())
                hidSortExpression.Value = S_OTHER_STAFF_SORT;

            else if (QueryString["UsersList"] != Constants.UserRoles.Teacher.ToString() && QueryString["UsersList"] != Constants.UserRoles.ParentTeacherAssociation.ToString() && !QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
                hidSortExpression.Value = grdvwSelectUser.Columns[I_USERNAME_COLUMN_INDEX].SortExpression;
            else if (QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
            {
	            if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || Boolean.Parse(hidUserHasFullAccess.Value)) && QueryString["IsStudentLevel"].IsNull())
		            hidSortExpression.Value = S_STDDIV_SORT;
	            else
		            hidSortExpression.Value = S_ROLL_SORT;

                if (miSchoolId == Constants.SchoolId.SPS.ToInt() && moUserRole != Constants.UserRoles.Student)
                {
                    trSPSType.Visible = true;
                    cmbType.SelectedValue = QueryString["TypeId"];
                }
                else
                    trSPSType.Visible = false;
	            
				ShowOrHideRegNoFilter(true);
                ChangeCntrlAccesiblity(false);
            }
            else
            {
                if (QueryString["UsersList"] == Constants.UserRoles.ParentTeacherAssociation.ToString())
                {

                    hidSortExpression.Value = S_PARENT_TEACHER_SORT;
                    grdvwSelectUser.Columns[I_USERNAME_COLUMN_INDEX].SortExpression = S_PARENT_TEACHER_SORT;
                }
                else
                {
                    hidSortExpression.Value = S_TEACHER_SORT;
                    grdvwSelectUser.Columns[I_USERNAME_COLUMN_INDEX].SortExpression = S_TEACHER_SORT;
                }
            }
        }
            hidSortDirection.Value = Constants.S_ASCENDING;       
    }

    /// <summary>
    /// This method is used to get selected user and set them to parent (calling) page.
    /// </summary>
    private void ManageSelectedUsersList()
    {
        int iSelectedUserCount = 0;
        string sStdDivList = string.Empty;
        for (int iCount = 0; iCount < grdvwSelectUser.Rows.Count; iCount++)
        {
            string sUsrName = string.Empty;
            if (((CheckBox)grdvwSelectUser.Rows[iCount].FindControl(S_CHECK_BOX_SELECT)).Checked)
            {
                if (!DDListStdDiv.Visible)
                {
                    //if (!QueryString["UsersList"].IsNull() && !QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
                    //    sUsrName = ((Label)grdvwSelectUser.Rows[iCount].Cells[I_USERNAME_COLUMN_INDEX].FindControl("lblUName")).Text;
                    //else
                        sUsrName = grdvwSelectUser.DataKeys[iCount]["OriginalName"].ToString();
                }
                else
                    sUsrName = grdvwSelectUser.DataKeys[iCount]["OriginalName"].ToString();

                sUserId += grdvwSelectUser.DataKeys[iCount]["ID"].ToString() + "; ";
                iSelectedUserCount = iSelectedUserCount + 1;

                // If login user is admin or teacher, supervison having full access them check whether all students are selcted by user or not.
                if ((moUserRole == Constants.UserRoles.Admin || Boolean.Parse(hidUserHasFullAccess.Value))
						&& (!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
							&& QueryString["IsStudentLevel"].IsNull())
                    sStdDivList = GenerateStudentCountDetails(iCount, sStdDivList);
                sUserName += sUsrName + ", ";

                if (QueryString["IsCc"].IsNull())
                {
                    if (!hidSelectedUserId.Value.EndsWith(";"))
                        hidSelectedUserId.Value = hidSelectedUserId.Value + ";";
                    if (!hidSelectedUserId.Value.StartsWith("; "))
                        hidSelectedUserId.Value = "; " + hidSelectedUserId.Value;
                    hidSelectedUserId.Value = hidSelectedUserId.Value.Replace(";" + grdvwSelectUser.DataKeys[iCount]["ID"].ToString() + ";", ";");
                    hidSelectedUserNames.Value = hidSelectedUserNames.Value.Replace(sUsrName + ", ", string.Empty).Replace(sUsrName, string.Empty);
                }
                else
                {
                    if (!hidSelectedUserIdCc.Value.EndsWith(";"))
                        hidSelectedUserIdCc.Value = hidSelectedUserIdCc.Value + ";";
                    if (!hidSelectedUserIdCc.Value.StartsWith("; "))
                        hidSelectedUserIdCc.Value = "; " + hidSelectedUserIdCc.Value;
                    hidSelectedUserIdCc.Value = hidSelectedUserIdCc.Value.Replace("; " + grdvwSelectUser.DataKeys[iCount]["ID"].ToString() + ";", ";");
                    hidSelectedUserNamesCc.Value = hidSelectedUserNamesCc.Value.Replace(sUsrName + ", ", string.Empty).Replace(sUsrName, string.Empty);
                }
            }
            if (QueryString["IsCc"].IsNull())
            {
                string[] sArrUserIdList = hidUserIds.Value.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sUsrid in sArrUserIdList)
                {
                    if (!sUserId.Contains(sUsrid))
                    {
                        if (grdvwSelectUser.DataKeys[iCount]["ID"].ToString() == sUsrid)
                        {
                            string sName = string.Empty;
                            if (!QueryString["UsersList"].IsNull() && !QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
                                sName = ((Label)grdvwSelectUser.Rows[iCount].Cells[I_USERNAME_COLUMN_INDEX].FindControl("lblUName")).Text;
                            else
                                sName = grdvwSelectUser.DataKeys[iCount]["OriginalName"].ToString();

                            hidSelectedUserId.Value = hidSelectedUserId.Value.Replace(sUsrid + "; ", string.Empty).Replace(sUsrid, string.Empty);
                            hidSelectedUserNames.Value = hidSelectedUserNames.Value.Replace(sName + ", ", string.Empty).Replace(sName, string.Empty);
                        }
                    }
                }
            }
            else
            {
                string[] sArrUserIdList = hidUserIdsCc.Value.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sUsrid in sArrUserIdList)
                {
                    if (!sUserId.Contains(sUsrid))
                    {
                        if (grdvwSelectUser.DataKeys[iCount]["ID"].ToString() == sUsrid)
                        {
                            string sName = string.Empty;
                            if (!QueryString["UsersList"].IsNull() && !QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
                                sName = ((Label)grdvwSelectUser.Rows[iCount].Cells[I_USERNAME_COLUMN_INDEX].FindControl("lblUName")).Text;
                            else
                                sName = grdvwSelectUser.DataKeys[iCount]["OriginalName"].ToString();

                            hidSelectedUserIdCc.Value = hidSelectedUserIdCc.Value.Replace(sUsrid + "; ", string.Empty).Replace(sUsrid, string.Empty);
                            hidSelectedUserNamesCc.Value = hidSelectedUserNamesCc.Value.Replace(sName + ", ", string.Empty).Replace(sName, string.Empty);
                        }
                    }
                }
            }
        }
        if (QueryString["IsCc"].IsNull())
        {
            hidSelectedUserId.Value = hidSelectedUserId.Value.TrimStart(';');
            hidSelectedUserId.Value = hidSelectedUserId.Value.Trim();
            hidSelectedUserId.Value = hidSelectedUserId.Value.TrimEnd(';');
        }
        else
        {
            hidSelectedUserIdCc.Value = hidSelectedUserIdCc.Value.TrimStart(';');
            hidSelectedUserIdCc.Value = hidSelectedUserIdCc.Value.Trim();
            hidSelectedUserIdCc.Value = hidSelectedUserIdCc.Value.TrimEnd(';');
        }

        if ((!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()) ||
              moUserRole == Constants.UserRoles.Student) && QueryString["IsCc"].IsNull())
        {
            if (hidSelectedUserId.Value.Trim() != string.Empty && !sUserId.Contains(hidSelectedUserId.Value))
            {
                sUserId += hidSelectedUserId.Value + "; ";
                if (hidSelectedUserNames.Value.Trim() != string.Empty)
                    sUserName += hidSelectedUserNames.Value + ", ";
            }
        }

        else if ((!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()) ||
              moUserRole == Constants.UserRoles.Student) && !QueryString["IsCc"].IsNull())
        {
            if (hidSelectedUserIdCc.Value.Trim() != string.Empty && !sUserId.Contains(hidSelectedUserIdCc.Value))
            {
                sUserId += hidSelectedUserIdCc.Value + "; ";
                if (hidSelectedUserNamesCc.Value.Trim() != string.Empty)
                    sUserName += hidSelectedUserNamesCc.Value + ", ";
            }
        }

        if (iSelectedUserCount > 0 || sUserId.Length > 0)
        {
            if (sStdDivList != string.Empty && !IsLeftStudent)
            {
                sStdDivList = sStdDivList.Substring(0, sStdDivList.Length - 2);
                lblErrorMsg.Text = "There are no students in the folllowing class(s) [" + sStdDivList + "]";
            }
            else
            {
                if (!String.IsNullOrEmpty(sUserName))
                {
                    sUserName = sUserName.Remove(sUserName.Length - 2);
                    sUserId = sUserId.Remove(sUserId.Length - 2);

                    if (iSelectedUserCount == grdvwSelectUser.Rows.Count && tblForStudentDiv.Visible && !rdoStudentReg.Checked && QueryString["IsCc"].IsNull())
                    {
                        //This checks for to allow select multiple division
                        if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()) && ((!QueryString["IsStudentLevel"].IsNull() && QueryString["IsStudentLevel"] == "Y")) || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                        {
                            sUserName = DDListStdDiv.SelectedItem.Text + ", ";
                            hidSelectedUserNames.Value = hidSelectedUserNames.Value.Replace(sUserName, "").Replace(DDListStdDiv.SelectedItem.Text, "");
                            if (hidSelectedUserNames.Value.Trim() != string.Empty)
                                sUserName += hidSelectedUserNames.Value + ", ";
                        }
                        else
                        {
                            if(!IsLeftStudent)
                                sUserName = DDListStdDiv.SelectedItem.Text;
                        }
                    }

                    if (iSelectedUserCount == grdvwSelectUser.Rows.Count && tblForStudentDiv.Visible && !rdoStudentReg.Checked && !QueryString["IsCc"].IsNull())
                    {
                        if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString())
                                   && ((!QueryString["IsStudentLevel"].IsNull() && QueryString["IsStudentLevel"] == "Y")) || moUserRole == Constants.UserRoles.Supervisor ||
                                       moUserRole == Constants.UserRoles.Teacher)
                        {
                            sUserName = DDListStdDiv.SelectedItem.Text + ", ";
                            hidSelectedUserNamesCc.Value = hidSelectedUserNamesCc.Value.Replace(sUserName, "").Replace(DDListStdDiv.SelectedItem.Text, "");
                            if (hidSelectedUserNamesCc.Value.Trim() != string.Empty)
                                sUserName += hidSelectedUserNamesCc.Value + ", ";
                        }
                        else
                            sUserName = DDListStdDiv.SelectedItem.Text;                    
                    }
                }
                if (sUserName.Trim().EndsWith(","))
                {
                    sUserName = sUserName.Substring(0, sUserName.LastIndexOf(','));
                }
                if (sUserId.Trim().EndsWith(";"))
                {
                    sUserId = sUserId.Substring(0, sUserId.LastIndexOf(';'));
                }
                sUserName = sUserName.Replace("'", "\\'");
                if (QueryString["IsCc"].IsNull())
                {   
                    if (QueryString["IsStudentLevel"].IsNull())
                        Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('" + sUserName + "','" + sUserId + "','N');</Script>");
                    else if(hidIsPTAMember.Value == Constants.S_YES && QueryString["IsPTAStudentLevel"].IsNull())
                        Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('" + sUserName + "','" + sUserId + "','N');</Script>");
                    else
                        Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('" + sUserName + "','" + sUserId + "','Y');</Script>");
                }
                else
                {
                    if (QueryString["IsStudentLevel"].IsNull())
                        Response.Write("<Script  type='text/javascript'>window.opener.SetCcUserId('" + sUserName + "','" + sUserId + "','N');</Script>");
                    else if(hidIsPTAMember.Value == Constants.S_YES && QueryString["IsPTAStudentLevel"].IsNull())
                        Response.Write("<Script  type='text/javascript'>window.opener.SetCcUserId('" + sUserName + "','" + sUserId + "','N');</Script>");
                    else
                        Response.Write("<Script  type='text/javascript'>window.opener.SetCcUserId('" + sUserName + "','" + sUserId + "','Y');</Script>");
                }

                Response.Write("<Script type='text/javascript'>window.close();</Script>");
            }
        }
        else
        {
            if (QueryString["IsCc"].IsNull())
            {
                if (QueryString["IsStudentLevel"].IsNull())
                    Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('','','N');</Script>");
                else
                    Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('','','Y');</Script>");
            }
            else
            {
                if (QueryString["IsStudentLevel"].IsNull())
                    Response.Write("<Script  type='text/javascript'>window.opener.SetCcUserId('','','N');</Script>");
                else
                    Response.Write("<Script  type='text/javascript'>window.opener.SetCcUserId('','','Y');</Script>");
            }
            Response.Write("<Script type='text/javascript'>window.close();</Script>");
        }
    }

    /// <summary>
    /// This function is used to generate the list of the standard division list whose student count is zero.
    /// </summary>
    /// <param name="aiCount"></param>
    /// <param name="sStdDivList"></param>
    /// <returns></returns>
    private string GenerateStudentCountDetails(int aiCount, string sStdDivList)
    {
        if ((grdvwSelectUser.Rows[aiCount].FindControl("HidStudentCount") as HiddenField).Value == "0")
        {
            Label oLabel = (Label)grdvwSelectUser.Rows[aiCount].Cells[I_USERNAME_COLUMN_INDEX].FindControl("lblUName");
            sStdDivList += oLabel.Text + ", ";
        }
        return sStdDivList;
    }

    /// <summary>
    /// This method is used to set properties of grid.
    /// </summary>
    private void GridViewProperties()
    {
        grdvwSelectUser.PageSize = 2000;//Constants.I_GRID_PAGE_COUNT;
        grdvwSelectUser.EmptyDataText = Constants.S_BLANK_GRID_MESSAGE;

        imgBtnOk.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdvwSelectUser.AllowPaging + "','" + S_SELECT_AT_LEAST_ONE_USER + "'))){return false;}");
        imgBtnOKUp.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdvwSelectUser.AllowPaging + "','" + S_SELECT_AT_LEAST_ONE_USER + "'))){return false;}");
    }

    /// <summary>
    /// This method is used to dispaly user name and their ids in grid.
    /// </summary>
    private void DisplayAllUserName()
    {
        DataTable moDataTable = new DataTable();
        if (ViewState["GridViewDS"] != null)
            moDataTable = (DataTable)ViewState["GridViewDS"];

        if (moDataTable != null)
        {
            hidSortExpression.Value = hidSortExpression.Value.Replace("asc", "").Replace("desc", "");
            hidSortExpression.Value = hidSortExpression.Value.Replace(" ", "");
            hidSortExpression.Value = hidSortExpression.Value.Replace(",", " " + hidSortDirection.Value + ",");
            
            DataView oDtView = new DataView(moDataTable);
            oDtView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
            grdvwSelectUser.DataSource = oDtView;
        }
        else
            grdvwSelectUser.DataSource = null;

        grdvwSelectUser.DataBind();
        SetGridViewPageCount();
    }

    /// <summary>
    /// This method sets Page Size of Gridview grdvwSelectUser.
    /// </summary>
    private void SetGridViewPageCount()
    {
        int iGridRowCount = grdvwSelectUser.Rows.Count;
        if (iGridRowCount <= Constants.I_GRID_PAGE_COUNT)
            grdvwSelectUser.PageSize = Constants.I_GRID_PAGE_COUNT;
        else
            grdvwSelectUser.PageSize = iGridRowCount;
    }

    /// <summary>
    /// This method is used to set controls with selected std-div
    /// </summary>
    /// <param name="aiStdDivID"></param>
    private void FillControlsWithSelectedStdDivID(int aiStdDivID)
    {
        int iTypeId = Constants.I_ZERO;

        if(miSchoolId == Constants.SchoolId.SPS.ToInt())
            iTypeId = cmbType.SelectedValue.ToInt();

        DataTable oDataSet = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, aiStdDivID, miAcademicYearId, string.Empty, iTypeId, IsLeftStudent);
        
        FillGridWithUserDetails(oDataSet);
    }

    /// <summary>
    /// This function is used to initialize page controls and sett attibutes required.
    /// </summary>
    private void InitalizePageControls()
    {
        //if ((((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor) || Boolean.Parse(hidUserHasFullAccess.Value) || hidIsPTAMember.Value == Constants.S_YES) && (QueryString["IsStudentLevel"].IsNull() && QueryString["IsPTAStudentLevel"].IsNull())) ||
        //       moUserRole == Constants.UserRoles.Student ||
        //        (!QueryString["UsersList"].IsNull() && !QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString())))

        if ((((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor) || Boolean.Parse(hidUserHasFullAccess.Value) || hidIsPTAMember.Value == Constants.S_YES) && (QueryString["IsStudentLevel"].IsNull() && QueryString["IsPTAStudentLevel"].IsNull())) ||
               moUserRole == Constants.UserRoles.Student ||
                (!QueryString["UsersList"].IsNull() && !QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString())))
        {
            if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == S_LEFT_STUDENTS)                
            {
                trClassDetails.Visible = true;
                rdoStdDiv.Checked = true;
                rdoStudentReg.Checked = false;
                txtName.Enabled = false;
                btnSearch.Enabled = false;
            }
            else
                tblForStudentDiv.Visible = false;
        }

        if (hidIsPTAMember.Value == Constants.S_YES)
        {
            if (QueryString["IsPTAStudentLevel"].IsNull())
                tblForStudentDiv.Visible = false;
            else
                tblForStudentDiv.Visible = true;
        }

        if (!QueryString["Mode"].IsNull() && QueryString["Mode"] == "SMS")
            lblSelectUser.Text = "Select User To Send SMS";

        btnClose.Attributes.Add("onclick", "if(!(closewindow())){return false;}");
        btnCloseUp.Attributes.Add("onclick", "if(!(closewindow())){return false;}");
        DDListStdDiv.Attributes.Add("onchange", "getUserIds()");

        ApplyMouseHoverEffect(new List<Button> { imgBtnOk, imgBtnOKUp, btnClose, btnCloseUp, btnSearch });               
    }

    /// <summary>
    /// This function is used to fill the Drop Down list with the StdDivDetails.
    /// </summary>
    private void FectchStandardDivDetails()
    {       
        int iTeacherID = 0;

        //if admin
        //Std-div are displayed in the grid
        if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || Boolean.Parse(hidUserHasFullAccess.Value)) && (QueryString["IsStudentLevel"].IsNull()))
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataTable oDSStdDivDetails = oMasterDataCollectionBL.GetStandardDivisionDetailsForMessageDetails(miSchoolId, miAcademicYearId, cmbType.SelectedValue.ToInt(), ref  DDListStdDiv, miUserId);
            FillGridWithUserDetails(oDSStdDivDetails);
        }
        else if (hidIsPTAMember.Value == Constants.S_YES && QueryString["IsPTAStudentLevel"].IsNull())
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataTable oDSStdDivDetails = oMasterDataCollectionBL.GetStandardDivisionDetailsForMessageDetails(miSchoolId, miAcademicYearId, cmbType.SelectedValue.ToInt(), ref  DDListStdDiv, miUserId);
            FillGridWithUserDetails(oDSStdDivDetails);
        }
        //if teacher
        //Std-div(associated with teacher) are displayed in the combo
        //and as the user selects one the students of selected class are listed in the grid
        else if (moUserRole == Constants.UserRoles.Teacher || (!QueryString["IsStudentLevel"].IsNull() && QueryString["IsStudentLevel"] == "Y"))
        {
            iTeacherID = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            if (Boolean.Parse(hidUserHasFullAccess.Value) || hidIsPTAMember.Value == Constants.S_YES)
                iTeacherID = 0;
            if (miSchoolId == Constants.SchoolId.SPS.ToInt())
            {
                StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL();
                DataTable dtDivision = oStandardDivisionMasterBL.GetStandardDivisionNamesForMessaging(miSchoolId, miAcademicYearId, iTeacherID, cmbType.SelectedValue.ToInt());
                ControlUtility.FillDropDownList(dtDivision, ref DDListStdDiv, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_SELECT);
            }
            else
                oMasterDataCollectionBL.FillStandardDivisionComboBoxOfStudents(miSchoolId, iTeacherID, miAcademicYearId, ref DDListStdDiv, miUserId);

            SetValuesOfAlreadySelectedUsers();
            
           if (moSchool == Constants.SchoolId.PPSN && Boolean.Parse(hidUserHasFullAccess.Value) == false)
            {
              DataTable oDataTable = oMasterDataCollectionBL.GetClassTeachers(miSchoolId, miAcademicYearId);

               oDataTable = oDataTable.AsEnumerable().GroupBy(row => new
               {
                   Standard_Id = row.Field<int>("Standard_Id"),
                   Division_Id = row.Field<int>("Division_Id"),
                   ClassName = row.Field<string>("ClassName"),
                })
                .Select(g => g.First())
                .CopyToDataTable();

               AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
               List<CoordinateDetails> lstCoordinatorDetails = oAttendanceDetailsBL.GetCoordinatorDetails(miSchoolId, miAcademicYearId);
               List<int> lstStandardIds = lstCoordinatorDetails.Where(ct => ct.UserId == miUserId).Select(ct => ct.StandardId).ToList();
                if (lstStandardIds.Count > 0)
                {
                  DataRow[] dtArray = oDataTable.Select("Standard_Id IN (" + string.Join(",", lstStandardIds) + ")");

                    if (dtArray.Length > 0)
                        oDataTable = dtArray.CopyToDataTable();

                    DataTable oDT = DDListStdDiv.DataSource as DataTable;
                    DataRow[] dtArr = oDT.Select("StandardId NOT IN (" + string.Join(",", lstStandardIds) + ")");

                    if (dtArr.Length > 0)
                    {
                        dtArr = dtArr.OrderBy(da => da.Field<int>("Original_Standard_Id")).ThenBy(da => da.Field<int>("Original_Division_Id")).AsEnumerable().ToArray();
                        for (int iIndex = 0; iIndex < dtArr.Length; iIndex++)
                        {
                            DataRow dr = oDataTable.NewRow();
                            dr["SchoolWise_Standard_Division_Id"] = dtArr[iIndex]["StdDivId"];
                            dr["ClassName"] = dtArr[iIndex]["StandardDivision"];

                            oDataTable.Rows.Add(dr);
                        }
                    }

                    ControlUtility.FillDropDownList(oDataTable, ref DDListStdDiv,
                                       Constants.S_STANDARD_DIVISION_ID_FIELD,
                                       "ClassName",
                                      Constants.S_SELECT);   
                }
              }

        }
        //if Supervisor
        else if (moUserRole == Constants.UserRoles.Supervisor)
        {
            StandardDivisionCollectionBL obj = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
            DataTable oDtStdDiv = obj.GetAssociatedStandardsDivisions();
            ControlUtility.FillDropDownList(oDtStdDiv, ref DDListStdDiv, "SchoolWise_Standard_Division_id", "StandardDivision", Constants.S_SELECT);
        }        
    }

    /// <summary>
    /// This method is used to show gridview according to standard division
    /// </summary>
    private void SetValuesOfAlreadySelectedUsers()
    {
        if (DDListStdDiv.Items.Count > 1)
        {
            if (QueryString["IsStudentLevel"].IsNull())
            {
                DDListStdDiv.Items[1].Selected = true;
                FillControlsWithSelectedStdDivID(Convert.ToInt32(DDListStdDiv.Items[1].Value));
            }
            else
            {
                int iStandardDivisionId=0;
                if (!QueryString["iStandardDivisionId"].IsNull())
                    iStandardDivisionId = Convert.ToInt32(QueryString["iStandardDivisionId"]);
                if (iStandardDivisionId == 0)
                    iStandardDivisionId = Convert.ToInt32(DDListStdDiv.Items[1].Value);
                DDListStdDiv.SelectedValue = iStandardDivisionId.ToString();
                FillControlsWithSelectedStdDivID(iStandardDivisionId);
                hidIsIndivisualStudentId.Value = "Y";
            }
        }
    }

    /// <summary>
    /// This functions is used to Fill the grid view with the user details.
    /// </summary>
    private void FillGridWithTeacherDetails()
    {
        int iTeacherId = 0;                
        if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
        DataTable oDataSetUserDetails = SchoolWiseTeacherMasterCollectionBL.FetchTeacherDetailsForMessageFacillity(miSchoolId, miAcademicYearId, iTeacherId, miUserId, moUserRole.ToInt(), cmbType.SelectedValue.ToInt());
      
        FillGridWithUserDetails(oDataSetUserDetails);
    }

    /// <summary>
    /// This method is used to fill user grid
    /// </summary>
    private void FillAllUserGrid()
    {
        DataTable oDataSetUserDetails = SchoolUserCollectionBL.GetAllUsers(miSchoolId, miAcademicYearId);
        FillGridWithUserDetails(oDataSetUserDetails);
    }

    /// <summary>
    /// This method is used to fill grid view with supervisors details
    /// </summary>
    private void FillGridWithSupervisorDetails()
    {
        //This fucntions is used to Fill the grid view with the user details.
        hidSortExpression.Value = S_SUPERVISOR_SORT;
        DataTable oDTUserDetails = SchoolWiseSupervisorMasterCollectionBL.GetSupervisorDetailsForMsging(miSchoolId, miAcademicYearId, moUserRole);
        FillGridWithUserDetails(oDTUserDetails);
    }

    private void FillGridWithParentTeacherAssociationDetails()
    {
        //This fucntions is used to Fill the grid view with the user details.       
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        DataTable oDTUserDetails = oParentTeacherAssociationDetailsBL.FetchParentTeacherAssociationDetails(miSchoolId, miAcademicYearId, Constants.SchoolCommittees.PTA.ToInt(), miUserId);
        FillGridWithUserDetails(oDTUserDetails);
    }
    private void FillGridWithOtherStaffDetails()
    {
        //This fucntions is used to Fill the grid view with the user details.
        DataTable oDTUserDetails = SchoolWiseSupervisorMasterCollectionBL.FetchSchoolWiseOtherStaffMasterDetails(miSchoolId, miAcademicYearId);
        FillGridWithUserDetails(oDTUserDetails);
    }

    /// <summary>
    /// This method is used to fill users gridview and set column headers according to type of user selected.
    /// </summary>
    /// <param name="oDataSetUserDetails"></param>
    private void FillGridWithUserDetails(DataTable oDataSetUserDetails)
    {
        //This function is used to fetch the Grid view with the User details.
        if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == Constants.UserRoles.Teacher.ToString())
            grdvwSelectUser.Columns[1].HeaderText = "Teacher Name (Designation)";
        else if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
            grdvwSelectUser.Columns[1].HeaderText = "Admin Staff Name (Designation)";
        else if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == Constants.UserRoles.OtherStaff.ToString())
            grdvwSelectUser.Columns[1].HeaderText = "Other Staff Name (Designation)";
        else if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == "ParentTeacherAssociation")
            grdvwSelectUser.Columns[1].HeaderText = "Name";
        else if ((!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()) && (QueryString["IsStudentLevel"].IsNull()) && QueryString["UsersList"] != S_LEFT_STUDENTS &&
                    (moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor) || Boolean.Parse(hidUserHasFullAccess.Value)))
            grdvwSelectUser.Columns[1].HeaderText = "Class Name";
        else if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
            grdvwSelectUser.Columns[1].HeaderText = "Student Name";
        else if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
            grdvwSelectUser.Columns[1].HeaderText = Constants.S_SUPERVISOR_ROLE_NAME;
        else if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == Constants.UserRoles.ParentTeacherAssociation.ToString())
            grdvwSelectUser.Columns[1].HeaderText = " Name (Designation)";
        else
            grdvwSelectUser.Columns[1].HeaderText = "User Name";

        if (miSchoolId != Constants.SchoolId.PPSN.ToInt() && oDataSetUserDetails.Columns.Contains(hidSortExpression.Value) && !QueryString["UsersList"].IsNull() && QueryString["UsersList"] != S_LEFT_STUDENTS)
            oDataSetUserDetails = new DataView(oDataSetUserDetails, "", hidSortExpression.Value + " " + hidSortDirection.Value, DataViewRowState.OriginalRows).ToTable();

        var Ids = oDataSetUserDetails.AsEnumerable().Select(sb => sb.Field<int>("Id")).ToList();
        if (Ids.Count > 0)
            hidIds.Value = string.Join(",", Ids);
        else
            hidIds.Value = string.Empty;

        grdvwSelectUser.DataSource = oDataSetUserDetails;
        grdvwSelectUser.DataBind();

       
        if (oDataSetUserDetails.Rows.Count == 0)
        {
            imgBtnOKUp.Visible = false;
            imgBtnOk.Visible = false;
        }
        else
        {
            grdvwSelectUser.Columns[2].Visible = false;

            //if (QueryString["UsersList"] == null || !QueryString["UsersList"].Contains("ParentTeacherAssociation"))
            //{
            //    grdvwSelectUser.Columns[2].Visible = false;
            //}
            //else
            //{
            //    lblNote.Visible = true;
            //    lblNote.Text = "Note - In case of parent members, child name will be visible in recipient list.";
            //}
        }
        
        ViewState["GridViewDS"] = oDataSetUserDetails;
    }

     /// <summary>
    /// This method is used to change accessibility of std div combo or search textbox.
    /// </summary>
    /// <param name="p"></param>
    private void ChangeCntrlAccesiblity(bool bEnableSearch)
    {
        txtName.Enabled = bEnableSearch;
        btnSearch.Enabled = bEnableSearch;
        DDListStdDiv.Enabled = !bEnableSearch;
    }

    /// <summary>
    /// This function is used to set controls visibility
    /// </summary>
    /// <param name="bIsFilterApplicable"></param>
    private void ShowOrHideRegNoFilter(Boolean bIsFilterApplicable)
    {
        tdrdoStdDiv.Visible = bIsFilterApplicable;
        trRegFilter.Visible = bIsFilterApplicable;
    }

    /// <summary>
    /// This method is used to fill user grid according to request.
    /// </summary>
    private void FillGridsAccordingToRequest()
    {
        if (!QueryString["UsersList"].IsNull())
        {
            if (QueryString["UsersList"] == Constants.UserRoles.Teacher.ToString())
            {
                if (miSchoolId == Constants.SchoolId.SPS.ToInt() && moUserRole != Constants.UserRoles.Student)
                    trSPSType.Visible = true;
                else
                    trSPSType.Visible = false;
                FillGridWithTeacherDetails();
            }
            else if (QueryString["UsersList"].Contains(Constants.UserRoles.Student.ToString()))
            {
                hidIsPTAMember.Value = Constants.S_NO;

                if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
                    bool bIsPTAMrmber = oMessageDetailsBL.IsPTAMember(miSchoolId, miAcademicYearId, miUserId);
                    hidIsPTAMember.Value = bIsPTAMrmber ? Constants.S_YES : Constants.S_NO;
                }

                if (miSchoolId == Constants.SchoolId.SPS.ToInt() && moUserRole != Constants.UserRoles.Student)
                    trSPSType.Visible = true;
                else
                    trSPSType.Visible = false;

                if (!QueryString["UsersList"].IsNull() && QueryString["UsersList"] == S_LEFT_STUDENTS)
                {
                    hidIsLeftStudents.Value = Constants.S_YES;
                    FillClassCombo();                    
                }
                else
                {
                    hidIsLeftStudents.Value = Constants.S_NO;
                    FectchStandardDivDetails();
                }
            }
            else if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
                FillGridWithSupervisorDetails();
            else if (QueryString["UsersList"] == Constants.UserRoles.OtherStaff.ToString())
                FillGridWithOtherStaffDetails();
            else if (QueryString["UsersList"] == Constants.UserRoles.ParentTeacherAssociation.ToString())
                FillGridWithParentTeacherAssociationDetails();

            if (QueryString["UsersList"].Equals(S_ALL))
                FillAllUserGrid();
        }
    }

    /// <summary>
    /// This method is used to fill class combobox for Left Students.
    /// </summary>
    private void FillClassCombo()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillStandardDivisionComboBoxOfStudents(miSchoolId, Constants.I_ZERO, miAcademicYearId, ref DDListStdDiv, miUserId);
        FillControlsWithSelectedStdDivID(Constants.I_ZERO);
    }
    
    #endregion    
}


