/* File Name :- SchoolwiseAcademicYearUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 25-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display academic years with their status.
*/
 
using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Threading;

public partial class SchoolwiseAcademicYearUI : SchoolBase
{
    #region Constants

    const int I_COLUMN_INDEX_EDIT = 4;
    const int I_COLUMN_INEDX_CHECKBOX = 0;
    const int I_COLUMN_CLOSE_YEAR_INEDX_CHECKBOX = 3;
    const string S_CHECKBOX_IS_CURRENT_YEAR = "chkCurrentYear";
    const string S_CHECKBOX_CLOSE_YEAR = "chkCloseYear";
    const int I_SCHOOL_ACADEMIC_YEAR_ID_DATAKEY = 0;
    const int I_IS_CURRENT_YEAR_DATAKEY = 2;
    const int I_IS_CLOSE_YEAR_DATAKEY = 3;

    #endregion

    #region Events
	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			base.OnPreInit(e);
			if (Settings.IsMiniSite)
				Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";				
			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    /// <summary>
    /// This method is used to fill academic year grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetDefaultSorting();
                FillAcademicYearGrid();                
                SetJavascriptAttributes();                
            }

            if (Settings.IsMiniSite) {
                btnBack.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add attributes on edit button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAcademicYear_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {  

            if (e.Row.RowIndex >= 0)
            {
                CheckBox chkIsCurrentYear = (CheckBox)e.Row.Cells[I_COLUMN_INEDX_CHECKBOX].FindControl(S_CHECKBOX_IS_CURRENT_YEAR);
                CheckBox chkIsCloseYear = (CheckBox)e.Row.Cells[I_COLUMN_CLOSE_YEAR_INEDX_CHECKBOX].FindControl(S_CHECKBOX_CLOSE_YEAR);
                ImageButton imgEdit = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls[Constants.I_ZERO];

                //To transfer SchoolId and Academic year id to SchoolwiseAcademicYearPopup page using query string.
                Int32 iAcademicYearId = Convert.ToInt32(grdAcademicYear.DataKeys[e.Row.RowIndex][I_SCHOOL_ACADEMIC_YEAR_ID_DATAKEY].ToString());
                string sQueryString = "SchoolId=" + miSchoolId + "&AcademicYearId=" + iAcademicYearId;
                string sEncryptSchoolAndAcademicId = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                imgEdit.Attributes.Add("onclick", "window.open('../SuperAdmin/SchoolwiseAcademicYearPopup.aspx?" + sEncryptSchoolAndAcademicId + "','_new','scrollbars=yes,resizable=no,top=0,left=0,width=850,height=650');return false;");
                chkIsCurrentYear.Enabled = false;
                chkIsCloseYear.Enabled = false;
                if (grdAcademicYear.DataKeys[e.Row.RowIndex][I_IS_CURRENT_YEAR_DATAKEY].ToString().Equals(Convert.ToString(Constants.C_YES)))
                    chkIsCurrentYear.Checked = true;
                else
                    chkIsCurrentYear.Checked = false;

                if (grdAcademicYear.DataKeys[e.Row.RowIndex][I_IS_CLOSE_YEAR_DATAKEY].ToString().Equals(Convert.ToString(Constants.C_YES)))
                    chkIsCloseYear.Checked = true;
                else
                    chkIsCloseYear.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort grid data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAcademicYear_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;
            FillAcademicYearGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add default sort image. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAcademicYear_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add a sort direction image to the appropriate column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master;
            oSuperAdminMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to set default sorting.
    /// </summary>
    private void SetDefaultSorting()
    {
        hidSortExpression.Value = grdAcademicYear.Columns[1].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to fill academic year grid.
    /// </summary>
    private void FillAcademicYearGrid()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        SetDefaultDateFormat();
        DataTable oDTAcademicYear = oSchoolWiseAcademicYearMasterBL.GetAllSchoolwiseAcademicYearInfo(miSchoolId);
        DataView oDataView = oDTAcademicYear.DefaultView;
        oDataView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
        grdAcademicYear.DataSource = oDataView;
        grdAcademicYear.DataBind();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack });
    }

    /// <summary>
    /// This method is used to set default date format.
    /// </summary>
    private void SetDefaultDateFormat()
    {
        BoundField oReopenDate = (BoundField)grdAcademicYear.Columns[1];
        oReopenDate.HtmlEncode = false;
        oReopenDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        oReopenDate = (BoundField)grdAcademicYear.Columns[2];
        oReopenDate.HtmlEncode = false;
        oReopenDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    #endregion


}
