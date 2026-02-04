// File Name  : ReturnRenewBookUI.aspx.cs
// Created By : Ashish
// Date       : 16/09/2008
//Description :This class is used to Return/Renew Issued Library Books. and also enter loss book details.
//Modified by :Rohini
//Date : 22/12/2011
//Description:Code review

using System;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Web;

public partial class ReturnRenewUI : SchoolBase
{

    #region  " Constants "

    const int I_USER_ROLE_ID = 1;
    const int I_ISSUE_DATE_COLUMN = 6;
    const int I_RETURN_DATE_COLUMN = 7;
    const string I_RENEW_ATTEMPT_DATAKEY_ID = "Renew_Attempt";
    const string S_CMD_NAME_RENEW_BOOK = "RENEW_BOOK";
    const string S_BOOK_ID = "Book_ID";
    const string S_COMMAND_REMOVE_BOOK = "Remove";
    const string S_COMMAND_RETURN_BOOK = "Return";
    const string S_BOOK_ISSUED_TO = "Book_Issued_To";
    const string S_IS_FOR_PARENT = "IsForParent";
    const string S_PARENT_RENEW_ATTEMPT = "ParentRenewAttempt";
    const string S_RETURN_SUCCESS = "Book returned successfully !!!";
    const string S_REMOVE_SUCCESS = "Book removed successfully !!!";
    const string S_RENEW_SUCCESS = "Book renewed successfully !!!";

    #endregion

    #region " Event "

    /// <summary>
    /// This event is used to check precondition and initialized control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    SetDefaultProperties();
					SetDefaultButton(btnUserBookSearch);
                    SetDefaultSortGridArrow();
                    GetQueryString();
                    FillClassCombo();
                    hidSortDirection.Value = Constants.S_ASCENDING;
                }
            }
            SetClientScriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search books..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUserBookSearch_Click(object sender, EventArgs e)
    {
        try
        {
            hidReturnBookID.Value = txtBookID.Text;
            FillGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        
    }

    /// <summary>
    /// This event is used to save late fee.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLateFee_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidCommandName.Value == S_COMMAND_REMOVE_BOOK)
            {
                RemoveBook();
                SetUpdateMessage(true,S_REMOVE_SUCCESS);
            }
            else if (hidCommandName.Value == S_COMMAND_RETURN_BOOK)
            {
                ReturnBook();
                if (txtLateFee.Text != string.Empty)
                    txtAmt.Text = txtLateFee.Text;
                if (txtAmt.Text != Constants.S_ZERO && txtAmt.Text != string.Empty)
                    SaveLateFeeDetails();
                grdReturnRenewBooks.PageIndex = Constants.I_ZERO;
                ResetControls();
                SetUpdateMessage(true, S_RETURN_SUCCESS);
            }
            else if (hidCommandName.Value == S_CMD_NAME_RENEW_BOOK)
            {
                int iRowIndex = Convert.ToInt32(hidRowNo.Value);
                RenewBookWithLateFee(iRowIndex);
                SetUpdateMessage(true, S_RENEW_SUCCESS);
            }
            FillGrid();
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    
    }
   
    /// <summary>
    /// This event is used to search books by barcode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string S_RESET = "-99";
            string sBook = txtBarcode.Text.Trim();
            string sIsBook = string.Empty;
            string sSchoolId = miSchoolId.ToString();
            if (!string.IsNullOrEmpty(sBook))
                sIsBook = sBook.Substring(0, 1);

            if (sBook != string.Empty && sIsBook == Convert.ToChar(Constants.BarcodeChar.Book).ToString()
                && sBook.Contains(Convert.ToChar(Constants.BarcodeChar.Separator).ToString()) && sBook.Contains(miSchoolId.ToString()) && sBook.Length == (sBook.LastIndexOf(sSchoolId) + (sSchoolId.Length)))
            {
                try
                {
                    if (Convert.ToInt32(sBook.Substring(1, sBook.LastIndexOf(Convert.ToChar(Constants.BarcodeChar.Separator).ToString()) - 1)) > Constants.I_ZERO)
                        hidBookDetailsID.Value = sBook.Substring(1, sBook.LastIndexOf(Convert.ToChar(Constants.BarcodeChar.Separator).ToString()) - 1);
                }
                catch
                {
                    txtBarcode.Text = string.Empty;
                    hidBookDetailsID.Value = S_RESET;
                }
            }
            else if (sBook != string.Empty)
                hidBookDetailsID.Value = S_RESET;
            else
                hidBookDetailsID.Value = Constants.S_ZERO;

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
            GridViewRow oPagerRow = grdReturnRenewBooks.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = oPagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;

            // Set the PageIndex property to display that page selected by the user.
            grdReturnRenewBooks.PageIndex = pageList.SelectedIndex;
            FillGrid();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display rows index on lable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdReturnRenewBooks.PageSize * grdReturnRenewBooks.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdReturnRenewBooks.PageSize) - 1);
                if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    SetDataPager(e);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This method is used to come back to the previous page. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = this.Master as MasterPage;
            oMasterPage.RedirectToNextPage("~/LibrarianManagement/LibraryManagementUI.aspx");

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion end event

    #region " Grid Event"

    /// <summary>
    /// This event is used to renew issued books and refresh grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdRetuenRenewBooks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_CMD_NAME_RENEW_BOOK)
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                RenewBookWithLateFee(iRowIndex);
               
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is handled to set navigate url and status image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdRetuenRenewBooks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string I_ISSUE_TO_DATAKEYNAME_ID = "Book_Issued_To";
            string I_BOOK_DATAKEYNAME_ID = "Book_No";
            string I_NO_OF_ATTEMPT_RENEW_DATAKEY_ID = "No_Of_Attempt_Renew";
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                int iRowIndex = e.Row.RowIndex;
                int iNoOfAttempRenew = Convert.ToInt32(grdReturnRenewBooks.DataKeys[e.Row.RowIndex][I_NO_OF_ATTEMPT_RENEW_DATAKEY_ID]) + 1;
                int iRenwAttempt = Convert.ToInt32(grdReturnRenewBooks.DataKeys[e.Row.RowIndex][I_RENEW_ATTEMPT_DATAKEY_ID]);
                int iLateFee = 0;
                int iBookId = Convert.ToInt32(grdReturnRenewBooks.DataKeys[e.Row.RowIndex][S_BOOK_ID]);
                string sBookNo = grdReturnRenewBooks.DataKeys[e.Row.RowIndex][I_BOOK_DATAKEYNAME_ID].ToString();
                int sBookIssuedTo=Convert.ToInt32(grdReturnRenewBooks.DataKeys[e.Row.RowIndex][I_ISSUE_TO_DATAKEYNAME_ID]);
                ImageButton oImgRenew = e.Row.FindControl("btnRenew") as ImageButton;

                if (Convert.ToBoolean(grdReturnRenewBooks.DataKeys[iRowIndex][S_IS_FOR_PARENT]))
                {
                    iRenwAttempt = Convert.ToInt32(grdReturnRenewBooks.DataKeys[e.Row.RowIndex][S_PARENT_RENEW_ATTEMPT]);
                    iNoOfAttempRenew = IssueReturnBookBL.GetIssuedCntForParent(sBookIssuedTo, iBookId)+1;
                }
                string sRenwAttempt = iNoOfAttempRenew.ToString();
                string sRenewMsg = string.Empty;
                DateTime dtIssueDate = Convert.ToDateTime(e.Row.Cells[I_ISSUE_DATE_COLUMN].Text);
                DateTime dtReturnDate = DateTime.MinValue;
                if (HttpUtility.HtmlDecode(e.Row.Cells[I_RETURN_DATE_COLUMN].Text).Trim() != string.Empty)
                    dtReturnDate = Convert.ToDateTime(e.Row.Cells[I_RETURN_DATE_COLUMN].Text);

                DateTime dtTodayDate = DateTime.Today;
                //display Left student and deacivated student 
                if (grdReturnRenewBooks.DataKeys[iRowIndex]["SchoolLeft_Date"] != DBNull.Value ||
                    grdReturnRenewBooks.DataKeys[iRowIndex]["Is_Locked"].ToString() == Constants.S_YES)
                {
                    oImgRenew.Enabled = false;
                    e.Row.BackColor = System.Drawing.Color.Gainsboro;
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
                
                Image oImgForParent = e.Row.FindControl("imgBtnForParent") as Image;
                oImgForParent.Visible = Convert.ToBoolean(grdReturnRenewBooks.DataKeys[iRowIndex][S_IS_FOR_PARENT]);
                
                //calculate late fee
                if (dtReturnDate != DateTime.MinValue && dtReturnDate < dtTodayDate)
                {
                    int LateFee = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex]["Late_Fee_Per_Day"]);
                    iLateFee = Convert.ToInt32(dtTodayDate.Subtract(dtReturnDate).TotalDays) * LateFee;
                    
                }
                //Message according to No of attempts
                if (iNoOfAttempRenew <= iRenwAttempt)
                {
                    sRenewMsg = "Book renew attempt - #" + sRenwAttempt + ". Are you sure you want to renew this book?";
                    if (iNoOfAttempRenew >= iRenwAttempt)
                        sRenewMsg = "Last book renew attempt. Are you sure you want to renew this book?";
                    oImgRenew.Attributes.Add("onclick", "if(!ConfirmRenew('" + sRenewMsg + "','Confirm',this,'" + sBookNo + "','" + iBookId + "'," + iLateFee + "," + sBookIssuedTo + ",'"+S_CMD_NAME_RENEW_BOOK+"'," + e.Row.RowIndex + ")){return false;}");
                }
                if (iNoOfAttempRenew > iRenwAttempt)
                    sRenewMsg = "You already have renewed this book for " + iRenwAttempt + " time. Please return this book.";
               
                //it is used when we aply late fee 
                // HyperLink oHyperLink = (HyperLink)e.Row.Cells[I_LATE_FEE_COLUMN].Controls[Constants.I_ZERO];
                if (dtTodayDate > dtReturnDate)
                    e.Row.BackColor = System.Drawing.Color.Pink;
                //Added javascript attribute
                ImageButton oImgReturn = (ImageButton)e.Row.FindControl("btnReturn");
                oImgReturn.Attributes.Add("onclick", "ShowPopup(this,'" + sBookNo + "','" + dtReturnDate.ToShortDateString() + "','" + dtIssueDate.ToShortDateString() + "','"+ DateTime.Now.ToString(Constants.S_DATE_FORMAT) +"'," + iLateFee + "," + iRowIndex +"," + sBookIssuedTo + ");return false;");

                ImageButton oImgRemoveBook = (ImageButton)e.Row.FindControl("btnRemove");
                oImgRemoveBook.Attributes.Add("onclick", "  ShowRemovePopup(this,'" + sBookNo + "','" + iBookId + "'," + iLateFee + "," + e.Row.RowIndex + "," + sBookIssuedTo + ");return false;");

                btnBookRemove.Attributes.Add("onclick", "if(!ConfirmRemove(this,'" + sBookNo + "','" + iBookId + "'," + sBookIssuedTo + ",'"+S_COMMAND_REMOVE_BOOK+"')){return false;}");
                btnReturnBook.Attributes.Add("onclick", "if(!ConfirmReturn(this,'" + sBookNo + "','" + iBookId + "'," + sBookIssuedTo + ",'"+S_COMMAND_RETURN_BOOK+"')){return false;}");

                /* oHyperLink.Enabled = true;
                 oHyperLink.Text = dtDiff.Days.ToString();
                 int iRowIndex = Convert.ToInt32(e.Row.RowIndex);
                 string sQueryString = "BookIssuedTo=" + grdRetuenRenewBooks.DataKeys[e.Row.RowIndex][I_ISSUE_TO_DATAKEYNAME_ID]
                                         + "&BookID=" + grdRetuenRenewBooks.DataKeys[e.Row.RowIndex][I_BOOK_DATAKEYNAME_ID];

                 string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                 oHyperLink.Attributes.Add("onclick", "window.open('StudentLibraryFeesUI.aspx?" + sEncrypt
                                                                            + " ' , '_self');return false;");
             }
             else
             {
                 oHyperLink.Text = "0";
                 oHyperLink.Enabled = false;
                 oHyperLink.NavigateUrl = "";
             }*/
            }
            else
                SetScrollbar();

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
    protected void grdRetuenRenewBooks_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdReturnRenewBooks.PageIndex = e.NewPageIndex;
            FillGrid();
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
    protected void grdRetuenRenewBooks_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = sender as GridView;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (iSortColumnIndex != -1)
                {
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
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
    protected void grdRetuenRenewBooks_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables((e.SortExpression.ToString()));
            FillGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion " End - Grid Event"

    #region " Private Method "
    /// <summary>
    /// This method is used to fill gridview.
    /// </summary>
    private void FillGrid()
    {
        grdReturnRenewBooks.DataSourceID = GrdDSobj.ID;
    }

    /// <summary>
    /// This method is used to fill class combo.
    /// </summary>
    private void FillClassCombo()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDT = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ListSource.FillDropDownList(oDT, cmbClass, "StandardDivision", "SchoolWise_Standard_Division_id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void GetQueryString()
    {
        const string S_CLOSE= "Close";

		if (QueryString["IsActivated"] != null)
			chkShowDeactivatedUser.Checked = QueryString["IsActivated"] != Constants.S_ONE;
		if (QueryString["BookNo"] != null)
        {
			txtBookID.Text = QueryString["BookNo"];
			hidReturnBookID.Value = QueryString["BookNo"];
            btnBack.Text = S_CLOSE;
            btnBack.Visible = true;
            FillGrid();
        }
    }

    //// <summary>
    ///// This method is used to sort grid
    ///// </summary>
    private void SetSortVariables(string asSortExpression)
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
        hidSortExpression.Value = asSortExpression;
        hidSortExpression.Value = hidSortExpression.Value + " " + hidSortDirection.Value;
    }

    /// <summary>
    /// This method is used to set default sort image.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        hidSortExpression.Value = grdReturnRenewBooks.Columns[0].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to check precondition for library setting for issue books.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AdminStaffConfig);

        if (!sLinks.Equals(string.Empty))
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls as per requirement.
    /// </summary>
    private void VisibleOrHideControls()
    {
        tblReturnRenew.Visible = false;
        grdReturnRenewBooks.Visible = false;
    }

    /// <summary>
    /// This method is used to set javascript attribute when page is load first time.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        lblError.Visible = false;
        txtBarcode.Focus();
        new Button[] { btnBack, btnBookRemove, btnCancel, btnLateFee, btnReturnBook, btnUserBookSearch, Button1 }.ApplyEffect();
    }

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>   
    private void SetDefaultProperties()
    {
        int I_COLUMN_INDEX_BOOK_TITLE = 2;
        txtBookID.Focus();
        BoundField oIssueDate = (BoundField)grdReturnRenewBooks.Columns[I_ISSUE_DATE_COLUMN];
        oIssueDate.HtmlEncode = false;
        oIssueDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;

        BoundField oReturnDate = (BoundField)grdReturnRenewBooks.Columns[I_RETURN_DATE_COLUMN];
        oReturnDate.HtmlEncode = false;
        oReturnDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;

        HtmlForm oForm = (HtmlForm)this.Master.FindControl("form1");
        oForm.DefaultButton = btnUserBookSearch.UniqueID;

        valsumReturnRenewBook.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        hidSortExpression.Value = grdReturnRenewBooks.Columns[I_COLUMN_INDEX_BOOK_TITLE].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to set grid view paging.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetGridPaging(GridViewRow gridViewRow)
    {
        if (gridViewRow.RowType == DataControlRowType.Pager)
        {
            GridViewRow oPagerRow = gridViewRow;

            // Retrieve the DropDownList and Label controls from the row.
            DropDownList oPageList = (DropDownList)oPagerRow.Cells[0].FindControl("PageDropDownList");
            Label oPageLabel = (Label)oPagerRow.Cells[0].FindControl("CurrentPageLabel");

            if (oPageList != null)
            {
                // Create the values for the DropDownList control based on 
                // the  total number of pages required to display the data
                // source.
                for (int i = 0; i < grdReturnRenewBooks.PageCount; i++)
                {
                    // Create a ListItem object to represent a page.
                    int iPageNumber = i + 1;
                    ListItem oItem = new ListItem(iPageNumber.ToString());

                    // If the ListItem object matches the currently selected
                    // page, flag the ListItem object as being selected. Because
                    // the DropDownList control is recreated each time the pager
                    // row gets created, this will persist the selected item in
                    // the DropDownList control.   
                    if (i == grdReturnRenewBooks.PageIndex)
                    {
                        oItem.Selected = true;
                    }

                    // Add the ListItem object to the Items collection of the 
                    // DropDownList.
                    oPageList.Items.Add(oItem);
                }
            }

            if (oPageLabel != null)
            {
                // Calculate the current page number.
                int iCurrentPage = grdReturnRenewBooks.PageIndex + 1;

                // Update the Label control with the current page information.
                oPageLabel.Text = "Page " + iCurrentPage.ToString() +
                  " of " + grdReturnRenewBooks.PageCount.ToString();

            }

        }
    }

    /// <summary>
    /// This method is used to reset all return renew book controls.
    /// </summary>
    private void ResetControls()
    {
        txtBookID.Text = string.Empty;
        txtUserName.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set scrollbar.
    /// </summary>
    private void SetScrollbar()
    {
        if (grdReturnRenewBooks.Rows.Count == Constants.I_ZERO)
            pnlGid.ScrollBars = ScrollBars.None;
        else
            pnlGid.ScrollBars = ScrollBars.Horizontal;
    }

    /// <summary>
    /// This method is used to return book.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    private void ReturnBook()
    {
        string sBookNo = hidBookNo.Value;
        DateTime dtActReturnDate = DateTime.Now;
         
		IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL
												   {
													   BookNo = sBookNo,
													   UpdatedById = miUserId
												   };
        if (hidActReturnDate.Value != string.Empty)
            dtActReturnDate = Convert.ToDateTime(hidActReturnDate.Value);
        //Append current time in date. Thid require because while inserting Return date its timing is default 12:00 AM
        string sDateWithCurrTime = dtActReturnDate.Date.ToString("M/dd/yyyy");
        sDateWithCurrTime = sDateWithCurrTime + " " + DateTime.Now.ToString("HH:mm:ss");
        oIssueReturnBookBL.ActualReturnDate = sDateWithCurrTime;
        oIssueReturnBookBL.ReturnBook();
        if (hidCommandName.Value == S_COMMAND_RETURN_BOOK)
        SendMailToReserveBookUsers();
    }

    /// <summary>
    /// This method is used to save late fee.
    /// </summary>
    private void SaveLateFeeDetails()
    {
        IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL
        {
            BookIssuedTo = Convert.ToInt32(hidUserId.Value),
            BookNo = hidBookNo.Value,
            InsertedById = miUserId,
            SchoolId = miSchoolId,
            LateFee = txtAmt.Text != string.Empty ? Convert.ToInt32(txtAmt.Text) : Constants.I_ZERO,
            AcademicYearId = miAcademicYearId
        };
        oIssueReturnBookBL.SaveLateFee();
    }

    /// <summary>
    /// This method is used to send mail to all users who reserve book.
    /// </summary>
    private void SendMailToReserveBookUsers()
    {
        string S_MESSAGE = "Claimed Book \'%BOOKNAME%\'  is now available in the library, please contact to librarian to issue it.      \n- Regards, %SCHOOL_NAME%.";
        const string S_MAIL_SUBJECT = "Book availability";
        int iRowIndex = Convert.ToInt32(hidRowNo.Value);
        string sBookTitle = grdReturnRenewBooks.Rows[iRowIndex].Cells[3].Text;
        string BookReserveUserList = grdReturnRenewBooks.DataKeys[iRowIndex]["BookReserveUserList"].ToString();
        if (BookReserveUserList!=string.Empty)
            SendMessage(BookReserveUserList, S_MAIL_SUBJECT, S_MESSAGE.Replace("%BOOKNAME%", sBookTitle).Replace("%SCHOOL_NAME%", ConfigurationManager.AppSettings["SchoolName"]));
    }

    /// <summary>
    /// This method is used to send mail.
    /// </summary>
    /// <param name="asUserId"></param>
    /// <param name="sMsgSubject"></param>
    /// <param name="sMsgBody"></param>
    private void SendMessage(string asUserId, string sMsgSubject, string sMsgBody)
    {
        Message oMessage = new Message();
        oMessage.sMessageBody = sMsgBody;
        oMessage.sMessageSubject = sMsgSubject;
        oMessage.SetMessageReceivers(asUserId, miUserId);
        oMessage.InsertMessageDetails(miUserId, Convert.ToInt32(moUserRole), miAcademicYearId);
    }

    /// <summary>
    /// This method is used to renew book.
    /// </summary>
    private void RenewBookWithLateFee(int iRowIndex)
    {
        int I_NUMBER_OF_ATTEMPT_RENEW = 2;
        DateTime oDtToday = DateTime.Today;
        int iCurrentRenewAttempts = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][I_NUMBER_OF_ATTEMPT_RENEW].ToString());
        int iTotalRenewAttempts = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][I_RENEW_ATTEMPT_DATAKEY_ID]);
        //DateTime oDtReturnDate = Convert.ToDateTime(grdReturnRenewBooks.Rows[iRowIndex].Cells[I_RETURN_DATE_COLUMN].Text);
        DateTime oDtReturnDate = DateTime.MinValue;
          if (HttpUtility.HtmlDecode(grdReturnRenewBooks.Rows[iRowIndex].Cells[I_RETURN_DATE_COLUMN].Text).Trim() != string.Empty)
              oDtReturnDate = Convert.ToDateTime(grdReturnRenewBooks.Rows[iRowIndex].Cells[I_RETURN_DATE_COLUMN].Text);

        if (Convert.ToBoolean(grdReturnRenewBooks.DataKeys[iRowIndex][S_IS_FOR_PARENT]))
        {
            int iUserId = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][S_BOOK_ISSUED_TO]);
            int iBookId = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][S_BOOK_ID]);
            iTotalRenewAttempts = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][S_PARENT_RENEW_ATTEMPT]);
            iCurrentRenewAttempts = IssueReturnBookBL.GetIssuedCntForParent(iUserId, iBookId);
        }

        if (iCurrentRenewAttempts < iTotalRenewAttempts)
        {
            lblError.Visible = false;
            int iUserRoleId = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][I_USER_ROLE_ID].ToString());
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL
            {
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId,
                BookIssueId = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex]["Issue_Id"]),
                BookNo = Convert.ToString(grdReturnRenewBooks.DataKeys[iRowIndex][Constants.I_ZERO]),
                UserId = miUserId,
                IsForParent = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][S_IS_FOR_PARENT])
            };

            DateTime DtRenewDate = oDtReturnDate.AddDays(Convert.ToInt32(oIssueReturnBookBL.GetIssuePeried(iUserRoleId)));
            //Append current time in date. Thid require because while inserting Return date its timing is default 12:00 AM
            string sDateWithCurrTime = DtRenewDate.Date.ToString("M/dd/yyyy");
            sDateWithCurrTime = sDateWithCurrTime + " " + DateTime.Now.ToString("HH:mm:ss");

            oIssueReturnBookBL.RenewDate = sDateWithCurrTime;
            oIssueReturnBookBL.RenewAttempts = iCurrentRenewAttempts + 1;
            oIssueReturnBookBL.RenewBook();
            SetUpdateMessage(true, S_RENEW_SUCCESS);
            if (txtAmt.Text != Constants.S_ZERO && txtAmt.Text != string.Empty)
            SaveLateFeeDetails();
        }
        else
        {
            lblError.Visible = true;
            if (iCurrentRenewAttempts != Constants.I_ZERO)
                lblError.Text = "You already have renewed this book for " + iCurrentRenewAttempts + " time(s). Please return this book.";
            else if (iTotalRenewAttempts == Constants.I_ZERO)
                lblError.Text = "Book renewal is not allowed.";
            
        }
    }

    /// <summary>
    /// This method is used to remove book.
    /// </summary>
    private void RemoveBook()
    {
        string sBookNo = hidBookNo.Value;
        BookBL oBookBL = new BookBL
        {
            BookNumber = sBookNo,
            SchoolId = miSchoolId,
            BookRemoveReason = hidReason.Value,
            IsBookLost = true
        };
        int iBookId = Convert.ToInt32(hidBookId.Value);
        int iCount = oBookBL.GetCount(iBookId);
        ReturnBook();
        DeleteBook(oBookBL, iBookId, iCount);
        if (txtLateFeeLost.Text != string.Empty)
            txtAmt.Text = txtLateFeeLost.Text;
        if (txtAmt.Text != Constants.S_ZERO && txtAmt.Text != string.Empty)
            SaveLateFeeDetails();
        grdReturnRenewBooks.PageIndex = Constants.I_ZERO;
        ResetControls();
    }

    /// <summary>
    /// This method is used to delete a book.
    /// </summary>
    /// <param name="oBookBL"></param>
    /// <param name="iBookId"></param>
    /// <param name="iCount"></param>
    private void DeleteBook(BookBL oBookBL, int iBookId, int iCount)
    {
        if (iCount > Constants.I_ONE)
        {
            oBookBL.DeleteBook();
            int iRowIndex = Convert.ToInt32(hidRowNo.Value);
			oBookBL.CancelBookReservation(Constants.I_ZERO, iBookId, miSchoolId, miAcademicYearId);
        }
        else
        {
            oBookBL.Delete(iBookId);
            int iRowIndex = Convert.ToInt32(hidRowNo.Value);
            int iUserId = Convert.ToInt32(grdReturnRenewBooks.DataKeys[iRowIndex][S_BOOK_ISSUED_TO]);
            oBookBL.CancelBookReservation(Constants.I_ZERO, iBookId, miSchoolId, miAcademicYearId);
        }
    }

    /// <summary>
    /// This method is used to show or hide data pager.
    /// </summary>
    /// <param name="e"></param>
    private void SetDataPager(ObjectDataSourceStatusEventArgs e)
    {
        if (e.ReturnValue.GetType() != typeof(DataTable))
        {
            if (e.ReturnValue.ToString() == Constants.S_ZERO || grdReturnRenewBooks.PageCount == Constants.I_ZERO)
                trTotalRecId.Visible = false;
            else
                trTotalRecId.Visible = true;
            if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                lblEndIndex.Text = e.ReturnValue.ToString();
        }
        if (lblTotal.Text != string.Empty)
        {
            if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                trTotalRecId.Visible = false;
            else
                trTotalRecId.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to set success message.
    /// </summary>
    /// <param name="abFlag"></param>
    /// <param name="asMsg"></param>
    private void SetUpdateMessage(bool abFlag, string asMsg)
    {
        lblError.Visible = !abFlag;
        lblError.Text = string.Empty;
        lblMessage.Visible = abFlag;
        lblMessage.Text = asMsg;
    }
    #endregion

}
