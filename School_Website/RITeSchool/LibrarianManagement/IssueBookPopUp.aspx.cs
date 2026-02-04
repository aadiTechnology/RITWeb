/*
 *  File Name : - IssueBookPopUp.aspx.cs
 *  Purpose   : - This class is used to display all user name and their ids to select to issue
 *                Book.
 *  Date      : - 22-Sept-2008
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class IssueBookPopUp : SchoolBase
{

    #region Constants

    const int I_USERNAME_COLUMN_INDEX = 1;
    const string S_CHECK_BOX_DELETE = "ChkBoxDelete";
    const string S_SELECT_AT_LEAST_ONE_USER = "No user is selected. Are you sure you want to continue?";
    const string S_TEACHER_SORT = "Designation_Id";
    const string S_STDDIV_SORT = "ID";
    const string S_ROLL_SORT = "Roll_No";
    const string S_NAME_SORT = "Name";

    #endregion

    #region Data Member

    string sUserName, sUserId;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill grid view as per selected user radio button from the Issue book UI.
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
                SetDefaultSortGridArrow();
                grdvwSelectUser.DataSource = null;
                grdvwSelectUser.DataBind();

				if (QueryString["UsersList"] != null)
				{
					if (QueryString["UsersList"] == Constants.UserRoles.Teacher.ToString())
						FillGridWithTeacherDetails();
					else if (QueryString["UsersList"] == Constants.UserRoles.Student.ToString())
						FectchStandardDivDetails();
					else if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
						FillGridWithSupervisorDetails();
				}
                
                SetVisibilityOfControls();
              
                btnClose.Attributes.Add("onclick", "if(!(closewindow())){return false;}");
                btnCloseUp.Attributes.Add("onclick", "if(!(closewindow())){return false;}");
                DDListStdDiv.Attributes.Add("onchange", "getUserIds()");
            }

			ApplyMouseHoverEffect(new List<Button>() { imgBtnOk, imgBtnOKUp, btnCloseUp, btnClose });
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// On this event submit string of user name and their ids for issue books.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnOk_Click(object sender, EventArgs e)
    {
        try
        {
            GetUsersList();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            if (DDListStdDiv.SelectedIndex != 0)
            {
                SetDefaultSortGridArrow();
                int iStdDivID = Convert.ToInt32(DDListStdDiv.SelectedValue);
                FillControlsWithSelectedStdDivID(iStdDivID);
            }
            else
                grdvwSelectUser.DataBind();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
				{
					asStandardDivisionIds += oListItem.Value + ",";
				}
				if (asStandardDivisionIds != string.Empty)
					asStandardDivisionIds = asStandardDivisionIds.Substring(0, asStandardDivisionIds.LastIndexOf(","));
				oDataSet = StudentBL.GetAllStudentsByStdDivForBookIssue(miSchoolId, asStandardDivisionIds, miAcademicYearId, txtName.Text.Trim());
			}
			else
			{
				oDataSet = StudentBL.GetAllStudentsByStdDivForBookIssue(miSchoolId, 0, miAcademicYearId, txtName.Text.Trim());
			}
			FillGridWithUserDetails(oDataSet);
			hidIsIndivisualStudentId.Value = "Y";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
    }

    #endregion

    #region GridView Events

    /// <summary>
    /// This event occurs to sort columns the of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwSelectUser_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {           
            hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;

	        if (QueryString["UsersList"] == Constants.UserRoles.Student.ToString() && moUserRole == Constants.UserRoles.Admin)
		        hidSortExpression.Value = S_NAME_SORT;
	        DisplayAllUserName();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
                RadioButton r1 = ((RadioButton)e.Row.FindControl("ChkBoxDelete"));
                r1.Attributes.Add("onclick", "AlertMe(this);");
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

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
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    # region Helping Methods for Select user

    /// <summary>
    /// This method is used to set default sort grid arrow.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
	    if (QueryString["UsersList"] != Constants.UserRoles.Teacher.ToString() && QueryString["UsersList"] != Constants.UserRoles.Student.ToString())
		    hidSortExpression.Value = grdvwSelectUser.Columns[I_USERNAME_COLUMN_INDEX].SortExpression;
	    else if (QueryString["UsersList"] == Constants.UserRoles.Student.ToString())
		    hidSortExpression.Value = S_ROLL_SORT;
	    else
		    hidSortExpression.Value = S_TEACHER_SORT;
	    
		hidSortDirection.Value = Utility.Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to get user list.
    /// </summary>
    private void GetUsersList()
    {
        int iSelectedUserCount = 0;
        string sStdDivList = string.Empty;


        for (int iCount = 0; iCount < grdvwSelectUser.Rows.Count; iCount++)
        {
            if (((System.Web.UI.WebControls.RadioButton)grdvwSelectUser.Rows[iCount].FindControl(S_CHECK_BOX_DELETE)).Checked == true)
            {          
                if (DDListStdDiv.Visible != true)
                {
                    sUserName = grdvwSelectUser.Rows[iCount].Cells[I_USERNAME_COLUMN_INDEX].Text.ToString();
                }
                else
                {
                    sUserName = grdvwSelectUser.DataKeys[iCount]["Name"].ToString();
                }
                sUserId = grdvwSelectUser.DataKeys[iCount]["ID"].ToString();
                iSelectedUserCount = iSelectedUserCount + 1;
            }
        }


        if (iSelectedUserCount > 0)
        {
            if (sStdDivList != string.Empty)
            {
                sStdDivList = sStdDivList.Substring(0, sStdDivList.Length - 2);
                lblErrorMsg.Text = "There are no students in the folllowing standard-division(s) [" + sStdDivList + "]";
            }
            else
            {
                /*if (!String.IsNullOrEmpty(sUserName))
                {                   
                    if (iSelectedUserCount == grdvwSelectUser.Rows.Count && tblForStudentDiv.Visible == true)
                        sUserName = DDListStdDiv.SelectedItem.Text;
                }*/

                Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('" + sUserName + "','" + sUserId + "');</Script>");
                Response.Write("<Script type='text/javascript'>window.close();</Script>");
            }
        }
        else
        {
            Response.Write("<Script  type='text/javascript'>window.opener.SetToUserId('','');</Script>");
            Response.Write("<Script type='text/javascript'>window.close();</Script>");
        }

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
    /// 
    private void DisplayAllUserName()
    {
        DataTable moDataTable = new DataTable();
        if (ViewState["GridViewDS"] != null)
            moDataTable = (DataTable)ViewState["GridViewDS"];

        if (moDataTable != null)
        {
            string sSortExpression = hidSortExpression.Value + " " + hidSortDirection.Value;
			if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString() && hidSortExpression.Value.Contains("Name"))
                sSortExpression = S_TEACHER_SORT +" " + hidSortDirection.Value + "," + hidSortExpression.Value + " " + hidSortDirection.Value;
            DataView oDtView = new DataView(moDataTable, "", sSortExpression, DataViewRowState.OriginalRows);
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
    /// This method is used to fill control with selected standardwise division id on the grid view.
    /// </summary>
    /// <param name="aiStdDivID"></param>
    private void FillControlsWithSelectedStdDivID(int aiStdDivID)
    {
		DataTable oDataSet = StudentBL.GetAllStudentsByStdDivForBookIssue(miSchoolId, aiStdDivID, miAcademicYearId, string.Empty);
        FillGridWithUserDetails(oDataSet);
    }

    /// <summary>
    /// This method is used to set visibility of class combo box as per user role.
    /// </summary>
    private void SetVisibilityOfControls()
    {
        Constants.UserRoles eUserRoles = moUserRole;
		if (QueryString["UsersList"] != Constants.UserRoles.Student.ToString())                   
            tblForStudentDiv.Visible = false;
        ShowRegNoFilter(true);
        ChangeCntrlAccesiblity(false);
    }

    /// <summary>
    /// This function is used to fill the Drop Down list with the StdDivDetails.
    /// </summary>
    private void FectchStandardDivDetails()
    {
        //if Librarian and Admin login
        //Std-div are displayed in the combo
        //and as the user selects one the students of selected class are listed in the grid
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Teacher)
        {
            StandardDivisionCollectionBL obj = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
            DataTable oDtStdDiv = obj.GetAssociatedStandardsDivisions();
            ControlUtility.FillDropDownList(oDtStdDiv, ref DDListStdDiv, "SchoolWise_Standard_Division_id", "StandardDivision", Constants.S_SELECT);
        }       
    }
      
    /// <summary>
    /// This fucntions is used to Fill the grid view with the user details.
    /// </summary>
    private void FillGridWithTeacherDetails()
    {        
        int iTeacherId = 0;
        int iUserRoleId = Convert.ToInt32(moUserRole);

        if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
        DataTable oDataSetUserDetails;
        oDataSetUserDetails = SchoolWiseTeacherMasterCollectionBL.FetchTeacherDetailsForMessageFacillity(miSchoolId, miAcademicYearId, iTeacherId, miUserId, iUserRoleId, Constants.I_ZERO);
        FillGridWithUserDetails(oDataSetUserDetails);
    }

    /// <summary>
    /// This fucntions is used to Fill the grid view with the user details.
    /// </summary>
    private void FillGridWithSupervisorDetails()
    {   
        DataTable oDTUserDetails = SchoolWiseSupervisorMasterCollectionBL.FetchSchoolWiseSupervisorMasterDetails(miSchoolId, miAcademicYearId, Constants.S_ASCENDING,"Designation_Id", Constants.I_ZERO,99999,1,string.Empty);
        FillGridWithUserDetails(oDTUserDetails);
    }

    /// <summary>
    /// This function is used to fetch the Grid view with the User details.
    /// </summary>
    /// <param name="oDataSetUserDetails"></param>
    private void FillGridWithUserDetails(DataTable oDataSetUserDetails)
    {
		if (QueryString["UsersList"] == Constants.UserRoles.Teacher.ToString())
            grdvwSelectUser.Columns[1].HeaderText = "Teacher Name (Designation)";
		else if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
            grdvwSelectUser.Columns[1].HeaderText = "Admin Staff Name (Designation)";
		else if ((QueryString["UsersList"] == Constants.UserRoles.Student.ToString() && moUserRole == Constants.UserRoles.Admin))
        {
         if(rdoStdDiv.Checked)
            grdvwSelectUser.Columns[1].HeaderText = "Standard-Division Name";
         else if (rdoStudentReg.Checked)
             grdvwSelectUser.Columns[1].HeaderText = "Student Name";
        }
		else if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString())
            grdvwSelectUser.Columns[1].HeaderText = Constants.S_SUPERVISOR_ROLE_NAME;
		else if (QueryString["UsersList"] == Constants.UserRoles.Student.ToString())
            grdvwSelectUser.Columns[1].HeaderText = "Student Name";
        else
            grdvwSelectUser.Columns[1].HeaderText = "User Name";

        string sSortExpression =  hidSortExpression.Value + " " + hidSortDirection.Value;
        if (QueryString["UsersList"] == Constants.UserRoles.Supervisor.ToString() && hidSortExpression.Value.Contains("Name"))
            sSortExpression = S_TEACHER_SORT + " " + hidSortDirection.Value + "," + hidSortExpression.Value + " " + hidSortDirection.Value;
        DataView oDtVw = new DataView(oDataSetUserDetails, "", sSortExpression, DataViewRowState.OriginalRows);
        grdvwSelectUser.DataSource = oDtVw;
        grdvwSelectUser.DataBind();
        ViewState["GridViewDS"] = oDataSetUserDetails;
        
    }

	/// <summary>
    /// This method is used to change student std div radio button selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoStdDiv_CheckedChanged(object sender, EventArgs e)
    {
        txtName.Text = string.Empty;
        ChangeCntrlAccesiblity(false);
        grdvwSelectUser.DataBind();
    }

    /// <summary>
    /// This method is used to cange accessiblity of std div combo or searc textbox.
    /// </summary>
    /// <param name="p"></param>
    private void ChangeCntrlAccesiblity(bool bEnableSearch)
    {
        txtName.Enabled = bEnableSearch;
        btnSearch.Enabled = bEnableSearch;
        DDListStdDiv.Enabled = !bEnableSearch;
    }

    /// <summary>
    /// This method is used to visible/hide student registration controls.
    /// </summary>
    /// <param name="bIsFilterApplicable"></param>
    private void ShowRegNoFilter(Boolean bIsFilterApplicable)
    {
        tdrdoStdDiv.Visible = bIsFilterApplicable;
        trRegFilter.Visible = bIsFilterApplicable;
    }

    /// <summary>
    /// This method is used to change student radio button selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoStudentReg_CheckedChanged(object sender, EventArgs e)
    {
        DDListStdDiv.SelectedIndex = 0;
        ChangeCntrlAccesiblity(true);
        grdvwSelectUser.DataBind();
    }

    #endregion

}
