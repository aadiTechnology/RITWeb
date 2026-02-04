/*
 *  File Name : - ImportBookUI.aspx.cs
 *  Purpose   : - This class is used to import book UI details from excel sheet to database
 *                and show book details on grid veiw.
 *  Date      : - 12 May 2009
 *  Author    : - Ashish  
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class ImportBookUI : SchoolBase
{
    #region " Data Member "

    string msServerFilePath;

    #endregion

    const string S_DEFAULT_SORT_EXP = "Book_Title";

    #region " Event "

    /// <summary>
    /// This event is used to set client side attributes, fill grid view and set default controls. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                SetDefaultProperty();
                SetDefaultSortGridArrow();
                InitializeFields();
            }
            SetClientScriptAttributes();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to import book details in the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportBook_Click(object sender, EventArgs e)
    {
        try
        {
            string sFileName =CommonUtility.GetFileNameForRenaming(fileUploadBooks.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            msServerFilePath = sFolderName + sFileName;
            
            fileUploadBooks.SaveAs(msServerFilePath);

            string sErrorMessage = "";
            string sSourceFileName = fileUploadBooks.PostedFile.FileName;

            ImportBookBL oImportBookBL = new ImportBookBL(sSourceFileName, msServerFilePath);
			oImportBookBL.UserId = miUserId;
            oImportBookBL.SchoolId = miSchoolId;
            oImportBookBL.AcademicYearId = miAcademicYearId;
            BookBL oBookBL = new BookBL();
			oBookBL.SchoolId = miSchoolId;
            sErrorMessage = oImportBookBL.UploadFile(Convert.ToInt32(Constants.SchoolConfigurations.LibraryVendors));

            if (sErrorMessage.Equals(""))
            {
                lblHead.CssClass = "ClsHilightTextB";
                lblHead.Text = "File imported successfully !!!";
                lblHead.Visible = true;
                grdvwImportBooks.DataSourceID = GrdDSobj.ID;
            }
            else
            {
                lblHead.Text = sErrorMessage;
                lblHead.Visible = true;
            }
        }
        catch (BusinessLogic.Exceptions.NullStudentDateofBirthExceptions ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        catch (BusinessLogic.Exceptions.InvalidBookDataException ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        catch (BusinessLogic.Exceptions.ValidMobileNumberExceptions ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        finally
        {
            try
            {
                if (System.IO.File.Exists(msServerFilePath))
                    System.IO.File.Delete(msServerFilePath);
            }
            catch (Exception ex)
            {
				ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
    }

    /// <summary>
    /// This event is used to set total record count on the top of the grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdvwImportBooks.PageSize * grdvwImportBooks.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwImportBooks.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0")
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
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
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill grid view as per selected drop down values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdvwImportBooks.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdvwImportBooks.PageIndex = pageList.SelectedIndex;
            grdvwImportBooks.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is for going to back page. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBack_Click(object sender, EventArgs e)
    {
        try
        {            
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/LibrarianManagement/LibraryManagementUI.aspx");
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region " Grid Event"

    /// <summary>
    /// This event is used to set record count to the top of the grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwImportBooks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdvwImportBooks.PageCount; i++)
                    {
						// Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.                        
                        if (i == grdvwImportBooks.PageIndex)
                        {
                            item.Selected = true;
                        }

                        // Add the ListItem object to the Items collection of the DropDownList.
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdvwImportBooks.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdvwImportBooks.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set grid view paging and fill grid view as per page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwImportBooks_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwImportBooks.PageIndex = e.NewPageIndex;
            grdvwImportBooks.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sorting image to the grid view header coloum.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwImportBooks_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, grdvwImportBooks.SortExpression);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, grdvwImportBooks.SortDirection);
                }
                else
                    CommonUtility.AddSortImage(0, e.Row, grdvwImportBooks.SortDirection);
            }           
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort grid view as per selected header column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwImportBooks_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;
            grdvwImportBooks.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion " End - Grid Event"

    #region " private Method "
    
    /// <summary>
    /// This method is used to set default sort arrow in grid view.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        const Int32 I_COL_INDEX_BOOK_TITLE = 0;
        hidSortExpression.Value = grdvwImportBooks.Columns[I_COL_INDEX_BOOK_TITLE].SortExpression; 
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeFields()
    {
        trTotalRec.Visible = false;
    }

    /// <summary>
    /// This method is used to set client side javascript attributes.
    /// </summary>
    private void SetClientScriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button>(){ btnImportBook, imgbtnBack });
    }

    /// <summary>
    /// This method is used to set validation header text and set hyperlink attributes on javascript.
    /// </summary>
    private void SetDefaultProperty()
    {
        fileUploadBooks.Focus();
        lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../DOWNLOADS/BookDetails.xls','_self'); return false;");
        lnkDownloadTemplate.CssClass = "CursorHand";
        btnImportBook.Attributes["onclick"] = "javascript:DisableButtons(this)";
        imgbtnBack.Attributes["onclick"] = "javascript:DisableButtons(this)";
        valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }
    #endregion

}
