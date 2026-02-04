// File Name   : RemoveBookPopUpUI.aspx.cs
// Created By  : Ashish
// Date        : 18/09/2008
// Description : This class is used to Remove selected (Particular) Book from database. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class RemeveBookPopUpUI : SchoolBase
{
    #region Constants

    const int I_COLUMN_INDEX_BOOK_NUMBER = 0;
    const int I_COLUMN_INDEX_BOOK_REMOVE = 1;
    const int I_COLUMN_INDEX_WRITE_OFF = 2;
    const int I_ZERO = 0;

    #endregion

    #region  " Event "

    /// <summary>
    /// This event is used to Initialized control, decript query string and set javascript property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
			if (!IsPostBack)
            {
                GetQuerystring();
                SerDefaultControl();
                SetClientScriptAttributes();
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove book details from the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBookRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(hidRowIndex.Value);
            string sBookNo = Convert.ToString(grdvwBook.DataKeys[iRowIndex].Value);
            string sBookTitle = grdvwBook.Rows[iRowIndex].Cells[1].Text;
            BookBL oBookBL = new BookBL();
            oBookBL.BookNumber = sBookNo;
            oBookBL.SchoolId = miSchoolId;
            oBookBL.BookRemoveReason = hidReason.Value;
            oBookBL.IsBookLost = false;
            int iBookId = Convert.ToInt32(hidBookId.Value);
            int iCount = oBookBL.GetCount(iBookId);
            if (iCount > 1)
                oBookBL.DeleteBook();
            else
                oBookBL.Delete(iBookId);
            grdvwBook.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to write off book details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnWriteOffBook_Click(object sender, EventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(hidRowIndex.Value);
            BookBL oBookBL = new BookBL();
            oBookBL.BookDetailsId = Convert.ToInt32(grdvwBook.DataKeys[iRowIndex]["Book_Detail_Id"]);
            oBookBL.SchoolId = miSchoolId;
            oBookBL.BookRemoveReason = HidWriteOff.Value;
            oBookBL.IsWriteOffBook = true;
            oBookBL.WriteOffDate = System.DateTime.Now;
            oBookBL.IsDeleted = Constants.C_YES;
            oBookBL.UpdatedById = miUserId;
            oBookBL.WriteOffBookCopy();
            grdvwBook.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This control is used to Set the PageIndex property to display that page selected by the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdvwBook.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdvwBook.PageIndex = pageList.SelectedIndex;
            grdvwBook.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound datatable from server side to grid veiw.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdvwBook.PageSize * grdvwBook.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwBook.PageSize) - 1);
                if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();

                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (e.ReturnValue.ToString() == "0")
                        {
                            trTotalRecId.Visible = false;
                            tbBookInfo.Visible = false;
                        }
                        else
                        {
                            trTotalRecId.Visible = true;
                            tbBookInfo.Visible = true;
                        }
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRecId.Visible = false;
                        else
                            trTotalRecId.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #region "Grid Event"

    /// <summary>
    /// This event is used for data bound to the grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwBook_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= I_ZERO)
            {
                ImageButton imgDelete = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_BOOK_REMOVE].Controls[I_ZERO];
                imgDelete.Attributes.Add("onclick", "ShowPopup(this," + e.Row.RowIndex + ");return false;");

                ImageButton imgWriteOff = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_WRITE_OFF].Controls[I_ZERO];
                imgWriteOff.Attributes.Add("onclick", "ShowWriteOffPopup(this," + e.Row.RowIndex + ");return false;");
            }
            SetGridPaging(e.Row);
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to do paging to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwBook_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwBook.PageIndex = e.NewPageIndex;
            grdvwBook.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method is used to set sortImage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwBook_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwBook_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            grdvwBook.DataSourceID = GrdDSobj.ID;

        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion "End Grid Event"

    #endregion

    #region " Private Method "

    /// <summary>
    /// This method is used to set javascript attribute when page is load.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button>() { btnBookRemove, btnCancel, btnClose , btnCancelWriteOff, btnWriteOffBook }); 
        string sEncryptedString = string.Empty;

       // string sRemoveQuerystring = "BookId=" + hidBookId.Value;
        string sRemoveQuerystring = "BookId=" + hidBookId.Value +
                                            "&BookName=" + hidBookName.Value +
                                            "&MediaType=" + hidMediaType.Value +
                                            "&MainCategoryId=" + hidMainCategory.Value +
                                            "&AuthorName=" + hidAuthorName.Value +
                                            "&Publisher=" + hidPublisher.Value +
                                            "&AccessionNumber=" + hidAccessionNumber.Value +
                                            "&StandardId=" + hidStandardId.Value +
                                            "&Description=" + hidDescription.Value;
        sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sRemoveQuerystring);
        sEncryptedString = "?" + sEncryptedString + "";

        btnClose.Attributes.Add("onclick", "return CloseWindow('" + sEncryptedString + "');");
    }

    /// <summary>
    /// This methos is used to set default control when page is load first time.
    /// </summary>
    private void SerDefaultControl()
    {
        hidSortExpression.Value = grdvwBook.Columns[I_COLUMN_INDEX_BOOK_NUMBER].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;

        BookBL oBookBL = new BookBL(Convert.ToInt32(hidBookId.Value), miSchoolId);
        lblBookTitle1.Text = oBookBL.BookName;
        lblCategory1.Text = oBookBL.MainCategoryName;
        lblAuthor.Text = oBookBL.AuthorName;
        lblPublisher1.Text = oBookBL.PublishedBy;
    }

    /// <summary>
    /// This method is used to decript query string.
    /// </summary>
    private void GetQuerystring()
    {
	    if (QueryString.Count <= 0 || QueryString.IsNull())
		    return;
	    
		if (QueryString["BookId"] != null)
		    hidBookId.Value = QueryString["BookId"];
	    if (QueryString["BookName"] != null)
		    hidBookName.Value = QueryString["BookName"];
	    if (QueryString["MediaType"] != null)
		    hidMediaType.Value = QueryString["MediaType"];
	    if (QueryString["MainCategoryId"] != null)
		    hidMainCategory.Value = QueryString["MainCategoryId"];
	    if (QueryString["AuthorName"] != null )
		    hidAuthorName.Value = QueryString["AuthorName"];
	    if (QueryString["Publisher"] != null )
		    hidPublisher.Value =QueryString["Publisher"];
	    if (QueryString["AccessionNumber"] != null )
		    hidAccessionNumber.Value = QueryString["AccessionNumber"];
	    if (QueryString["StandardId"] != null )
		    hidStandardId.Value = QueryString["StandardId"];
	    if (QueryString["Description"] != null)
		    hidDescription.Value = QueryString["Description"];
    }

    /// <summary>
    /// This method is used to set grid view paging.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetGridPaging(GridViewRow gridViewRow)
    {

        if (gridViewRow.RowType == DataControlRowType.Pager)
        {
            GridViewRow pagerRow = gridViewRow;
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

            if (pageList != null)
            {
                for (int i = 0; i < grdvwBook.PageCount; i++)
                {
                    // Create a ListItem object to represent a page.
                    int pageNumber = i + 1;
                    ListItem item = new ListItem(pageNumber.ToString());

                    if (i == grdvwBook.PageIndex)
                    {
                        item.Selected = true;
                    }
                    pageList.Items.Add(item);
                }
            }

            if (pageLabel != null)
            {
                int currentPage = grdvwBook.PageIndex + 1;
                pageLabel.Text = "Page " + currentPage.ToString() +
                  " of " + grdvwBook.PageCount.ToString();
            }
        }
    }

    #endregion
}
