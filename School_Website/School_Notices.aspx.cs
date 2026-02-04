// Modified by:- Rohini
// Date:- 13 Aprill 2012
// Description:- Retrieving notices from database.

using System;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

/// <summary>
/// This class is used to show school notices displayed on Home page.
/// </summary>
public partial class School_Notices : System.Web.UI.Page
{
    #region "Const"

    private const string FOLDER_PATH = "RITeSchool/DOWNLOADS/School Notices/";
    private const string S_DEFAULT_SORT_EXP = "StartDate";
    private const string S_SORT= "SortNotice";

    #endregion

    #region"Event"
    /// <summary>
    /// This event is used to add the sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add Sort Image
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod(), Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]).ToString());
        }
    }

    /// <summary>
    /// This method is used to load school notices.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
				hidSchoolId.Value = ConfigurationManager.AppSettings["SchoolID"];
                hidSortDirection.Value = Constants.S_DESCENDING;
                FillListview();
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            HyperLink oHyperLink = oCurrentItem.FindControl("lnkName") as HyperLink;
            if (!string.IsNullOrEmpty(((NoticeDetails)oCurrentItem.DataItem).FileName))
            {
                oHyperLink.NavigateUrl = FOLDER_PATH + (((NoticeDetails)oCurrentItem.DataItem).FileName);
                oHyperLink.Attributes.Add("onclick", "window.open('" + oHyperLink.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
            }
            else
            {
                string sNoticeContent = HttpUtility.HtmlDecode(((NoticeDetails)oCurrentItem.DataItem).NoticeContent);
                string sNoticeName = ((NoticeDetails)oCurrentItem.DataItem).NoticeName;
                sNoticeName = sNoticeName.Replace("'", "\\'");
                sNoticeContent = sNoticeContent.Replace("\n", " ");
                sNoticeContent = sNoticeContent.Replace("'", "\"");
                oHyperLink.Attributes.Add("onclick", "ShowNoticePopup('" + sNoticeContent + "','"+sNoticeName+"')");
            }

            Label lblStartDate = oCurrentItem.FindControl("lblStartDate") as Label;
            lblStartDate.Text = Convert.ToDateTime(lblStartDate.Text).ToString(Constants.S_STANDARD_DATE_FORMAT);
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise Notices.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwNoticeList);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod(), Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]).ToString());
        }
    }
    
    /// <summary>
    /// This method is used to show data pager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeList_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwNoticeList.Items.Count > Constants.I_ZERO)
            {
                lstvwNoticeList.Items.Clear();
                ControlUtility.FillListViewPagerFooter(lstvwNoticeList, DtPgCount);
                if (DtPgCount.TotalRowCount > DtPgCount.PageSize)
                    DtPgCount.Visible = true;
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod(), Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]).ToString());
        }
    }

    /// <summary>
    /// This event is used to edit or delete the school notice.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNoticeList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_SORT)
            {
                DataPager dtPager = lstvwNoticeList.FindControl("DtPgDropDown") as DataPager;
                dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                hidSortExpression.Value = e.CommandArgument.ToString();
                SetSortVariables();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod(), Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]).ToString());
        }
    }

    #endregion

    #region "Private Methods"
    /// <summary>
    /// This method is used to add new html row to html table.
    /// </summary>
    /// <param name="lstNotices"></param>
    private void FillListview()
    {
        lstvwNoticeList.DataSourceID = ObjDSNoticeDetails.ID;
        lstvwNoticeList.DataBind();
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    /// <summary>
    /// This method is used to set sort image.
    /// </summary>
    private void AddSortImage()
    {
        if (string.IsNullOrEmpty(hidSortExpression.Value))
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        if (string.IsNullOrEmpty(hidSortDirection.Value))
            hidSortDirection.Value = Constants.S_DESCENDING;
        HtmlTableRow oHtmlTableHeaderRow = lstvwNoticeList.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }
    #endregion

}
    

