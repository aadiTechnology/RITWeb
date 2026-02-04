using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class UsersPunchingDetails : SchoolBase
{
    #region Constant
    private const string S_DEFAULT_SORT_EXP = "IndexNo";
    private const string S_Non_Punched_DEFAULT_SORT_EXP = "Employee_No";
    #endregion

    #region Data MEmber(s)

    #endregion

    #region Event

    /// <summary>
    /// This Event is handled to Add a Sort Image to the Tables
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(Object sender, EventArgs e)
    {
        try
        {
            SetPunchedSortImage(S_DEFAULT_SORT_EXP);
            SetNonPunchedSortImage(S_Non_Punched_DEFAULT_SORT_EXP);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill checkedinUsers and NonCheckedinUsers ListView and set attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetDefaultValues();
            }

            FillPunchedUserDetails();
            FillNotPunchedUserDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected void lstvwUsersPunched_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUsersPunched.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwUsersPunched, DtPgCount);
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
    /// 
    /// </summary>
    protected void lstvwNotPunchedUsers_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwNotPunchedUsers.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwNotPunchedUsers, DtPgCount1);
            }
            else
                DtPgCount1.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUsersPunched);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwNotPunchedUsers);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill punched users and non punched user grids on date selction change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cal_Date_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
           
            FillPunchedUserDetails();
            FillNotPunchedUserDetails();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used add sorting to list view columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNotPunchedUsers_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetNonPunchedSortVariables();
            HidNonPunchedSortExprsn.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used add sorting to list view columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsersPunched_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetPunchedSortVariables();
            HidPunchedSortExprsn.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to fill Punched user details.
    /// </summary>
    public void FillPunchedUserDetails()
    {
        DataPager pager = lstvwUsersPunched.FindControl("DtPgDropDown") as DataPager;
        if (pager != null)
            pager.SetPageProperties(0, pager.PageSize, true);
        DtPgCount.Visible = true;

        lstvwUsersPunched.DataSourceID = ObjDSConfigureUsersPunched.ID;
        lstvwUsersPunched.DataBind();
       
    }

    /// <summary>
    /// This method is used to get Non punched user details.
    /// </summary>
    public void FillNotPunchedUserDetails()
    {
        DataPager pager1 = lstvwNotPunchedUsers.FindControl("DtPgDropDown") as DataPager;
        if (pager1 != null)
            pager1.SetPageProperties(0, pager1.PageSize, true);
        DtPgCount1.Visible = true;

        lstvwNotPunchedUsers.DataSourceID = ObjDSConfigureUsersNotPunched.ID;
        lstvwNotPunchedUsers.DataBind();
    }

    /// <summary>
    /// This method is used set default values.
    /// </summary>
    private void SetDefaultValues()
    {
         txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
         HidPunchedSortDirection.Value = Constants.S_ASCENDING;
         HidPunchedSortExprsn.Value = S_DEFAULT_SORT_EXP;
         HidNonPunchedSortDirection.Value = Constants.S_DESCENDING;
         HidNonPunchedSortExprsn.Value = S_Non_Punched_DEFAULT_SORT_EXP;
    }

    /// <summary>
    /// This method is used set sorting direction.
    /// </summary>
    private void SetNonPunchedSortVariables()
    {
        if (HidNonPunchedSortDirection.Value == Constants.S_DESCENDING)
            HidNonPunchedSortDirection.Value = Constants.S_ASCENDING;
        else
            HidNonPunchedSortDirection.Value = Constants.S_DESCENDING;
    }


    /// <summary>
    /// This method is used set sorting direction.
    /// </summary>
    private void SetPunchedSortVariables()
    {
        if (HidPunchedSortDirection.Value == Constants.S_DESCENDING)
            HidPunchedSortDirection.Value = Constants.S_ASCENDING;
        else
            HidPunchedSortDirection.Value = Constants.S_DESCENDING;
    }


    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void SetPunchedSortImage(string asSortExpression)
    {
        if (lstvwUsersPunched.SortDirection.ToString() == "Ascending" || lstvwUsersPunched.SortDirection.ToString() == "")
            HidPunchedSortDirection.Value = Constants.S_ASCENDING;
        else
            HidPunchedSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwUsersPunched.SortExpression != string.Empty)
            HidPunchedSortExprsn.Value = lstvwUsersPunched.SortExpression.ToString();
        else
            HidPunchedSortExprsn.Value = asSortExpression;
        HtmlTableRow oHtmlTableHeaderRow = lstvwUsersPunched.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, HidPunchedSortExprsn.Value, HidPunchedSortDirection.Value);
    }


    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void SetNonPunchedSortImage(string asSortExpression)
    {
        if (lstvwNotPunchedUsers.SortDirection.ToString() == "Ascending" || lstvwNotPunchedUsers.SortDirection.ToString() == "")
            HidNonPunchedSortDirection.Value = Constants.S_ASCENDING;
        else
            HidNonPunchedSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwNotPunchedUsers.SortExpression != string.Empty)
            HidNonPunchedSortExprsn.Value = lstvwNotPunchedUsers.SortExpression.ToString();
        else
            HidNonPunchedSortExprsn.Value = asSortExpression;
        HtmlTableRow oHtmlTableHeaderRow = lstvwNotPunchedUsers.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, HidNonPunchedSortExprsn.Value, HidNonPunchedSortDirection.Value);
    }

    #endregion
}