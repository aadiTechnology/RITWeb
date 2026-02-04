/* File Name :- ShowImageGallery.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 17-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display and download Photo / Video galleries.
*/
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Web;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class ShowImageGallery : SchoolBase
{
    #region Constant    

    const string S_VIDEO_ID = "Video_Id";
    const int I_COLUMN_INDEX_SLIDE_SHOW = 2;
    const int I_COLUMN_INDEX_DOWNLOAD = 3;
    const int I_COLUMN_INDEX_GALLERY_NAME = 0;

    #endregion//Constant

    #region Events

    /// <summary>
    /// This event is used to fill grid with all available photo and video gallery details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            FillGalleryNames();
            FillVideoGalleryList();
            ExpandCollapsiblepanels();
            if (!IsPostBack)
                SetSortExpression();
			hidSchoolId.Value = (ConfigurationManager.AppSettings["SchoolID"]).ToString();
        }
         catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to display the download option availabe according to setting file value. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdImageGallery_DataBound(object sender, EventArgs e)
    {

        try
        {
            if (Settings.AllowPhotoGallaryDownloadForExternalSite != true)
                grdImageGallery.Columns[I_COLUMN_INDEX_DOWNLOAD].Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Event to apply sorting on grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdImageGallery_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            FillGalleryNames();

        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to display sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdImageGallery_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(grdImageGallery, hidSortExpression.Value);
                if (iSortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
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
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to attributes to grid photo grid columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdImageGallery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                ImageButton imgShow = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_SLIDE_SHOW].Controls[Constants.I_ZERO];
                string sGalleryName = e.Row.Cells[I_COLUMN_INDEX_GALLERY_NAME].Text;
                sGalleryName = HttpUtility.HtmlDecode(sGalleryName);
                string sQueryString = "xmlpath=" + StringUtility.DoHTMLEncoding(sGalleryName, false) + ".xml";
                string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQueryString);

                imgShow.Attributes.Add("onclick", "window.open('RITeSchool/Gallery/ImageGallery.aspx?" + sEncryptedString + "','_blank','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=10,left=20,width=950,height=690'); return false;");

                ImageButton imgDownload = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_DOWNLOAD].Controls[Constants.I_ZERO];
                string str = Server.MapPath(".");
                string sDestination = Server.MapPath("") + "\\RITeSchool" + "\\DOWNLOADS\\" + sGalleryName + ".zip";
                if (File.Exists(sDestination))
                    imgDownload.Attributes.Add("onclick", "window.open('RITeSchool//downloads/" + sGalleryName.Replace("'", "\\'") + ".zip','_self'); return false;");
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    #endregion Events

    #region Methods

    /// <summary>
    /// This method is used to set sort expression.
    /// </summary>
    private void SetSortExpression()
    {
        hidSortExpression.Value = "Update_Date";
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidVSortExpression.Value = "Update_Date";
        hidVSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// Method to fetch all photo galleries added for school and to bind them to grid.
    /// </summary>
    private void FillGalleryNames()
    {
		DataTable oDTPhotoGallery = ImageGalleryCollectionBL.GetAllGallriesWithCount(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));
        grdImageGallery.DataSource = oDTPhotoGallery;
        grdImageGallery.DataBind();
    }

    #endregion Methods

    #region Video Gallery

    #region Video Gallery Event(s)

    /// <summary>
    /// This event is used to change page index of video gallery gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdVideoGallery.PageIndex = e.NewPageIndex;
            FillVideoGalleryList();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to fetch video id from datakey.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex=0;
            int.TryParse(e.CommandArgument.ToString(),out iRowIndex);
            hidVedioId.Value = grdVideoGallery.DataKeys[iRowIndex][S_VIDEO_ID].ToString();
        }
         catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This method is used to set sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidVSortExpression.Value);
                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidVSortDirection.Value);
            }
        }
         catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event sets properties to video grid's column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            SetRowData(e.Row);
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    for (int i = 0; i < grdVideoGallery.PageCount; i++)
                    {
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        if (i == grdVideoGallery.PageIndex)
                            item.Selected = true;

                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {

                    // Calculate the current page number.
                    int currentPage = grdVideoGallery.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdVideoGallery.PageCount.ToString();

                }

            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This method is used to change sort expression/direction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdVideoGallery_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidVSortExpression.Value = e.SortExpression;
            if (hidVSortDirection.Value == Constants.S_DESCENDING)
                hidVSortDirection.Value = Constants.S_ASCENDING;
            else
                hidVSortDirection.Value = Constants.S_DESCENDING;

            hidVSortExpression.Value = hidVSortExpression.Value + " " + hidVSortDirection.Value;
            FillVideoGalleryList();
        }
         catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ObjectDSVideoGallery_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdVideoGallery.PageSize * grdVideoGallery.PageIndex) + 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdVideoGallery.PageSize) - 1);
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (e.ReturnValue.ToString() == "0")
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;

                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                    }

                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= 20)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to change page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow pagerRow = grdVideoGallery.BottomPagerRow;
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            grdVideoGallery.PageIndex = pageList.SelectedIndex;
            FillVideoGalleryList();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    #endregion//Video Gallery Event(s)

    #region Video Gallery Method(s)

    /// <summary>
    /// Tis method is used to fill gridview with video list.
    /// </summary>
    private void FillVideoGalleryList()
    {
        grdVideoGallery.DataSourceID = ObjectDSVideoGallery.ID;
    }

    /// <summary>
    /// This method is used to expand collapsible panel.
    /// </summary>
    private void ExpandCollapsiblepanels()
    {
        colpnlImageGallery.Collapsed = false;
        colpnlVideoGallery.Collapsed = false;
    }

    /// <summary>
    /// This method is used to set attributes at View button of video gallery gridview.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetRowData(GridViewRow gridViewRow)
    { 
         int iRowIndex = gridViewRow.RowIndex;
         if (iRowIndex >= 0)
         {
             int aiVideoId = Convert.ToInt32(grdVideoGallery.DataKeys[iRowIndex][S_VIDEO_ID]);
             string sQueryString = "src=" + aiVideoId + "&Schoolid=" + hidSchoolId.Value;
             string sEncryptedVideoId = Utility.CommonUtility.EncryptQuerystring(sQueryString);

             ImageButton oViewVideoGallery = (ImageButton)gridViewRow.FindControl("btnViewVideo");
             oViewVideoGallery.Attributes.Add("Onclick", "if (!ShowVideoGallery('" + sEncryptedVideoId + "')) return false;");          
         }
    }

    #endregion//Video Gallery Method(s)

    #endregion//Video Gallery         
}