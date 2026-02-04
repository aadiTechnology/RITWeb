using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class AlumniStudentsUI : ExportDataTable
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FilllstvwAlumniStudentDetails();
            SetDefaultValues();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAlumniDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        hidSortExpression.Value = e.SortExpression;
        SetSortVariables(); 
    }

    /// <summary>
    /// Initializes the DataPager control of the ListView and adds a Sort Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAlumniDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwAlumniDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwAlumniDetails, dtPagerCount);
                AddSortImage();
            }
            else
                dtPagerCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise Alumni Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwAlumniDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        int iPassoutYear = txtPassoutYr.Text != "" ? (txtPassoutYr.Text).ToInt() : Constants.I_ZERO;

        AlumniStudentBL oAlumniStudentBL = new AlumniStudentBL();
        DataTable odtAlumniDetails = oAlumniStudentBL.GetAlumniStudentDetailsToExport(iPassoutYear, miSchoolId);
        
        txtPassoutYr.Text = string.Empty;

        AddSortImage();

        if (odtAlumniDetails.Rows.Count != 0)
        {
            ExportToExcel("AlumniStudentDetails.XLS", odtAlumniDetails);
        }
        else
            lblNoRecordFound.Visible = true;
    }

    #region Private Method

    private void FilllstvwAlumniStudentDetails()
    {
        lstvwAlumniDetails.DataSourceID = odsAlumniDetails.ID;
        lstvwAlumniDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;

    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwAlumniDetails.SortDirection.ToString() == "Ascending" || lstvwAlumniDetails.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        HtmlTableRow oHtmlTableHeaderRow = lstvwAlumniDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        HtmlTableRow oHtmlTableHeaderRow = lstvwAlumniDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }
    #endregion Private Method
}