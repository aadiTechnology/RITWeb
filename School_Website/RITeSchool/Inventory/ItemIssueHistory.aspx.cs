using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;

public partial class ItemIssueHistory : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            ReadQueryString();
            if (!IsPostBack)
            {
                btnClose.Attributes.Add("onclick", "refreshParent()");
                lstvwItems.DataSourceID = lstDSobj.ID;
                ApplyMouseHoverEffect(new List<Button> {btnClose});
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["RequisitionID"] != null)
            hidRequistionId.Value = QueryString["RequisitionID"];
        
		if (QueryString["CreaterId"] != null)
            hidUserId.Value = QueryString["CreaterId"];
    }

    protected void lstvwItems_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItems.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwItems, DtPgCount);
                AddSortImage();
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according to the selected pageindex in the combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItems);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwItems.SortDirection.ToString() == "Ascending" || lstvwItems.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwItems.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwItems.SortExpression.ToString();
        else
            hidSortExpression.Value = "Issued_Date";
        HtmlTableRow oHtmlTableHeaderRow = lstvwItems.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This event is used to add sort image on the header of sorting column according to the sort direction. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
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
 
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}
