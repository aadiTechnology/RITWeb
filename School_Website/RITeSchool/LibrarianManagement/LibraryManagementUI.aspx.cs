//// File Name  : LibrarianManagementUI.aspx.cs
//// Created By : Ashish
//// Date       : 15/09/2008
////Description : This is a main page for librarian where he/she decide which operation is taken as per condition.
////            : This class is used for Searching Books, Issuing and Handaling others Librarian functionality.
////Modified By :Rohini
////Date        :21/12/2011
////Description :1.Books can be issued to Parent 2.Book reservation facility.

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
using System.Drawing;

/// <summary>
/// This method is used to issue a books.
/// </summary>
public partial class LibraryManagementUI : SchoolBase
{
    #region " Constant "

    private const string S_USER_ID = "UserId";
    private const string S_RESERVATION_NOT_ALLOWED = "Book claim is not allowed.";
    private const string S_ALLOWED_ISSUE_BOOK = "Library settings details are not configured for the selected user.";
    private const string S_ISSUE_MSG = "Book issued successfully!!!";
    private const string S_SELECT_MSG = "User and Book should be selected to issue the book.";
    private const string S_SAVE_MSG = "Book claimed successfully !!!";
    private const string S_RESRVED_SAME_BOOK = "Could not claim same book.";

    #endregion
    
    #region " Events "
    /// <summary>
    /// This method is used to fill the category combo and set default value to the grid view    
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    FillLangaugeCombo();
                    FillUserRolesCombo();
                    FillBooksGrid();
                    FillStandardCombo();
                    FillClassCombo();
                    SetDefaultValues();
                    GetQuerystring();
                    trEmployeeNo.Visible = false;
                    SetClientScriptAttributes();
                    hidUserSortExpression.Value = string.Empty;
                }
            }
            
            SetMediaType();
            ShowErrorMessage(false, string.Empty);
            lblUpdateSucess.Visible = false;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is handled to Add a Sort Image to the Tables
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(Object sender, EventArgs e)
    {
		try
		{
			AddSortImage();
		}
		catch (Exception ex)
		{
			AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod().Name);
		}
    }

    /// <summary>
    /// This method is used to change the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
     protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwBookMaster);
            DataPager oDataPager = lstvwBookMaster.FindControl("DtPgDropDown") as DataPager;
            DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            hidBooksPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
	}

    /// <summary>
    /// This event is used to change the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	 protected void cmbddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
	 {
		 try
		 {
			 ControlUtility.SetDataPagerAccordingToPageNo(lstvwUsers);
             DataPager oDataPager = lstvwUsers.FindControl("DtPg") as DataPager;
		     if (oDataPager != null)
		     {
		         DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
		         if (ddlCnt != null) hidUsersPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
		     }
		 }
		 catch (Exception ex)
		 {
             ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		 }
	 }

    /// <summary>
    ///  This event is used check the first option button of the book details list and uncheck all remaining option buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                RadioButton oOptSelect = oCurrentItem.FindControl("optSelectToIssue") as RadioButton;
                ListView oLstvwBookDetails = sender as ListView;

                HiddenField oHidBookNo = oCurrentItem.FindControl("hidBookNo") as HiddenField;
                HiddenField oHidBookDetailsId = oCurrentItem.FindControl("hidBookDetailsId") as HiddenField;
                int iIsForIssue = Convert.ToInt32(lstvwBookMaster.DataKeys[0]["IsForIssue"]);
                if (oLstvwBookDetails != null)
                {
                    oHidBookNo.Value = oLstvwBookDetails.DataKeys[iRowId]["Book_No"].ToString();
                    oHidBookDetailsId.Value = oLstvwBookDetails.DataKeys[iRowId]["Book_Detail_Id"].ToString();
                }

                bool bIsBookLost = Convert.ToBoolean(oLstvwBookDetails.DataKeys[iRowId]["IsBookLost"]);
                bool bIsWriteOffBook = Convert.ToBoolean(oLstvwBookDetails.DataKeys[iRowId]["IsWriteOffBook"]);
                if (bIsBookLost == true || bIsWriteOffBook == true)
                {
                    oOptSelect.Visible = false;

                    Label lblItemCode = oCurrentItem.FindControl("lblItemCode") as Label;
                    lblItemCode.ForeColor = Color.Purple;

                    Label lblItemName = oCurrentItem.FindControl("lblItemName") as Label;
                    lblItemName.ForeColor = Color.Purple;
                }
                else
                {
                    oOptSelect.Visible = true;
                }

                //if (iRowId == Constants.I_ZERO)
                //{
                //    if (iIsForIssue == Constants.I_ONE)
                //        oOptSelect.Checked = true;
                //    else
                //        oOptSelect.Enabled = false;
                //}

                oOptSelect.Attributes.Add("onclick", "SelectBook(this);");
                
                if (lstvwBookMaster.Items.Count > Constants.I_ZERO)
                    trButtons.Visible = true;
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used set book details in books grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                ImageButton oImgBtnSelectBook = oCurrentItem.FindControl("imgBtnSelectBook") as ImageButton;
                int iIsForIssue = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowId]["IsForIssue"]);
                LinkButton oLnkBtnReserve = oCurrentItem.FindControl("lnkReservBooks") as LinkButton;

                ImageButton oImgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                int iAvailableBookCount = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowId]["Available_Books"]);

                int iBookId = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowId]["Book_Id"]);
                if (iAvailableBookCount > Constants.I_ZERO)
                {
                    if (iIsForIssue != Constants.I_ZERO)
                        oImgBtnSelectBook.Visible = true;
                    oImgBtnDelete.Visible = true;
                    oLnkBtnReserve.Visible = false;
                }
                else
                {
                    oImgBtnSelectBook.Visible = false;
                    oImgBtnDelete.Visible = false;
                    oLnkBtnReserve.Visible = true;
                }

                if (iIsForIssue == Constants.I_ZERO)
                {
                    HtmlTableRow oHtmlTableHeaderRow = e.Item.FindControl("TrBookMaster") as HtmlTableRow;
                    if (oHtmlTableHeaderRow != null)
                    {
                        oHtmlTableHeaderRow.Style.Add(HtmlTextWriterStyle.Color, "red");
                        if (oImgBtnSelectBook != null) oImgBtnSelectBook.Visible = false;
                        if (oLnkBtnReserve != null) oLnkBtnReserve.Visible = false;
                    }
                }

                string sEncryptedString = string.Empty;
                string sRemoveQuerystring = "BookId=" + iBookId +
                                            "&BookName=" + txtBookName.Text +
                                            "&MediaType=" + hidMediaType.Value +
                                            "&AuthorName=" + txtAuthorName.Text +
                                            "&Publisher=" + txtPublisher.Text +
                                            "&AccessionNumber=" + txtAccessionNumber.Text +
                                            "&StandardId=" + cmbStandard.SelectedValue +
                                            "&Language=" + cmbLanguage.SelectedItem.Text;
                sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sRemoveQuerystring);
                oImgBtnDelete.Attributes.Add("onclick", "window.open('RemoveBookPopUpUI.aspx?" + sEncryptedString + " ' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=680,height=500'); return false;");

                string sEditQuerystring = "BookId=" + iBookId + "&IsEditMode=" + Constants.S_EDIT_MODE;
                sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sEditQuerystring);
                ImageButton oEditBooksPopUp = (ImageButton)oCurrentItem.FindControl("imgBtnEdit");
                oEditBooksPopUp.Attributes.Add("onclick", "window.open('BookUI.aspx?" + sEncryptedString+ " ' , '_self');return false;");

                string sAddQuerystring = "BookId=" + iBookId + "&IsEditMode=" + Constants.S_ADD_MODE;
                sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sAddQuerystring);
                ImageButton oImgAddQuantity = (ImageButton)oCurrentItem.FindControl("imgBtnAdd");
                oImgAddQuantity.Attributes.Add("onclick", "window.open('BookUI.aspx?" + sEncryptedString+ " ' , '_self');return false;");
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used show copies of book.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
             string S_SORT = "S_Sort";
             string S_RESERVE_BOOK = "Reserve_Book";
            if (e.CommandName == "DETAIL")
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                if (oCurrentItem != null)
                {
                    int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                    int iBookId = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowId]["Book_Id"]);
                    ListViewDataItem oItem = lstvwBookMaster.Items[iRowId] as ListViewDataItem;
                    HtmlTableRow rw = oItem.FindControl("trBookDetails") as HtmlTableRow;
                    HtmlTableRow oHtmlTableRow = e.Item.FindControl("trBookDetails") as HtmlTableRow;
                    if (oHtmlTableRow != null)
                    {
                       HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdBookDetails") as HtmlTableCell;
                        ListView olstvwBookDetails = oHtmlTableCell.FindControl("lstvwBookDetails") as ListView;
                        IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
                        DataTable oDtBookDetails = oIssueReturnBookBL.GetBookDetails(miSchoolId, iBookId);
                        oHtmlTableRow.Visible = true;
                        olstvwBookDetails.DataSource = oDtBookDetails;
                        olstvwBookDetails.DataBind();
                        hidBookDetailsCount.Value = oDtBookDetails.Rows.Count.ToString();
                    }
                }
            }

            if (e.CommandName == S_RESERVE_BOOK)
                ReserveBook(e);
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT)
            {
                if (hidSortExpression.Value != e.CommandArgument.ToString())
                    hidSortDirection.Value = Constants.S_DESCENDING;
                SetSortVariables();
                hidSortExpression.Value = e.CommandArgument.ToString();
                lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used show user details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

                Button BtnCancelRenewReturn = oCurrentItem.FindControl("BtnCancelRenewReturn") as Button;
                Button[] btnArr = { BtnCancelRenewReturn };
                btnArr.ApplyEffect();

                // Set Different CssClass for Alternating rows
				HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("Tr2") as HtmlTableRow;
                int oCurrentIndex = oCurrentItem.DisplayIndex;
				if (oCurrentIndex % 2 == Constants.I_ONE)
				    if (oHtmlTableRow != null) oHtmlTableRow.Attributes.Add("class", "ClsGridAltRow");

                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                RadioButton oOptSelect = oCurrentItem.FindControl("optSelectUser") as RadioButton;
                LinkButton oLnkSelect = oCurrentItem.FindControl("lnkbtnDetail") as LinkButton;
                int iBookId = Convert.ToInt32(lstvwUsers.DataKeys[iRowId]["Book_Id"]);

                oLnkSelect.Visible = oLnkSelect.Visible = iBookId > Constants.I_ZERO;
              
                Control oTdRegNo = oCurrentItem.FindControl("tdRegNo") as Control;
                Control oTdRollNo = oCurrentItem.FindControl("tdRollNo") as Control;
                Control oTdEmpNo = oCurrentItem.FindControl("tdEmpNo") as Control;

                if (Convert.ToInt16(hidUserRoleID.Value) != Convert.ToInt16(Constants.UserRoles.Student) 
                    && Convert.ToInt16(hidUserRoleID.Value) != Convert.ToInt16(Constants.UserRoles.Parent))
                {
                    oTdRegNo.Visible = false;
                    oTdRollNo.Visible = false;
                    oTdEmpNo.Visible = true;
                }
                else
                {
                    oTdRegNo.Visible = true;
                    oTdRollNo.Visible = true;
                    oTdEmpNo.Visible = false;
                }
                //// If the Users IsActive flag is N, disable his Radiobutton and set a distinct ForeColor and Background
                if (lstvwUsers != null)
                    if (lstvwUsers.DataKeys[iRowId]["IsActive"].ToString() == Constants.S_NO)
                    {
                        oOptSelect.Enabled = false;
                        if (oHtmlTableRow != null)
                        {
                            oHtmlTableRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gainsboro");
                            oHtmlTableRow.Style.Add(HtmlTextWriterStyle.Color, "red");                            
                        }
                    }
                    else
                        oOptSelect.Attributes.Add("onclick", "SelectUser(this," + oCurrentIndex + ");");
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide/show the Reg No. column from user grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_DataBound(object sender, EventArgs e)
    {
        try
        {
            HtmlTableCell oHtmlTableCell = lstvwUsers.FindControl("thRegNo") as HtmlTableCell;
            HtmlTableCell oHtmlTableCellRNo = lstvwUsers.FindControl("thRollNo") as HtmlTableCell;
            HtmlTableCell oHtmlTableCellEmpNo = lstvwUsers.FindControl("thEmpNo") as HtmlTableCell;
            HtmlTableCell oHtmlTableCellClassDesg = lstvwUsers.FindControl("thClassDesg") as HtmlTableCell;
             LinkButton olnkClassDesg = null;
            if (lstvwUsers.Items.Count > Constants.I_ZERO)
            olnkClassDesg = oHtmlTableCellClassDesg.FindControl("lnkClassDesg") as LinkButton;
            
            if (Convert.ToInt16(hidUserRoleID.Value) != Convert.ToInt16(Constants.UserRoles.Student) && 
                Convert.ToInt16(hidUserRoleID.Value) != Convert.ToInt16(Constants.UserRoles.Parent))
            {
                if (oHtmlTableCell != null)
                    oHtmlTableCell.Visible = false;
                if (oHtmlTableCellRNo != null)
                oHtmlTableCellRNo.Visible = false;

                if (oHtmlTableCellEmpNo != null)
                    oHtmlTableCellEmpNo.Visible = true;
                if (olnkClassDesg != null)
                olnkClassDesg.Text = "Designation ";
            }
            else
            {
                if (oHtmlTableCell != null)
                {
                    oHtmlTableCell.Visible = true;
                    oHtmlTableCellRNo.Visible = true;
                    oHtmlTableCellEmpNo.Visible = false;
                    if (olnkClassDesg != null)
                    olnkClassDesg.Text = "Class";
                }
            }
            hidUserRowCount.Value = lstvwUsers.Items.Count.ToString();
            SetUserPager();
            SetDefaultUser();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

     /// <summary>
    ///  This event is used show book issued details of user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DETAIL")
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                if (oCurrentItem != null)
                {
                    int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
					int iUserId = Convert.ToInt32(lstvwUsers.DataKeys[iRowId][S_USER_ID]);
                    hidIsActivated.Value = lstvwUsers.DataKeys[iRowId]["IsActive"].ToString();                    
                    HtmlTableRow oHtmlTableRow = e.Item.FindControl("trBookDetails") as HtmlTableRow;
                    if (oHtmlTableRow != null)
                    {
                        HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdBookDetails") as HtmlTableCell;
                        if (oHtmlTableCell != null)
                        {
                            ListView oLstvwUsersBookDetails = oHtmlTableCell.FindControl("lstvwUsersBookDetails") as ListView;
                            oHtmlTableRow.Visible = true;
                            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
                            DataTable oDtBookDetails = oIssueReturnBookBL.GetIssuedBookDetailsofUser(miSchoolId, miAcademicYearId, iUserId);
                            oLstvwUsersBookDetails.DataSource = oDtBookDetails;
                            oLstvwUsersBookDetails.DataBind();
                            hidUsersBookDetails.Value = oLstvwUsersBookDetails.Items.Count.ToString();
                        }
                    }
                }
            }
			else if (e.CommandName == Constants.S_COMMAND_SORT)
			{
				if (hidUserSortExpression.Value != e.CommandArgument.ToString())
					hidUserSortDirection.Value = Constants.S_DESCENDING;
			}
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used return or renew the books issued by the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsersBookDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                HyperLink oHlnkReturnRenew = oCurrentItem.FindControl("lnkbtnDetail") as HyperLink;
                ListView oLstvwBookDetails = sender as ListView;
                string sBookNo = Convert.ToString(oLstvwBookDetails.DataKeys[iRowId]["Book_No"]);
                int iIsActivated = hidIsActivated.Value == Constants.S_YES ? Constants.I_ONE : Constants.I_ZERO;
                string sQueryString = "BookNo=" + sBookNo + "&IsActivated=" + iIsActivated;
                string sEncryptedString = CommonUtility.EncryptQuerystring(sQueryString);
                oHlnkReturnRenew.Attributes.Add("onclick", "window.open('" + oHlnkReturnRenew.NavigateUrl + "?" + sEncryptedString + " ' , '_self','scrollbars=yes,resizable=no,top=0,left=0'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search users and fill users ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUserSearch_Click(object sender, EventArgs e)
    {
        try
        {
            trUserHeader.Visible = true;
            trUserInfo.Visible = true;
            lstvwUsers.DataSourceID = string.Empty;
            if (!string.IsNullOrEmpty(txtUsrBarcode.Text))
                //// if Barcode is available in text box then search a book by barcode.
                txtUsrBarcode_TextChanged(new object(), EventArgs.Empty);
            else
                //// if Barcode is not available in text box then search  books according to applied filters.
                FillUserGrid();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search books and fill books ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            cmbStandard.Enabled = !chkForParentStaff.Checked;
            if (!string.IsNullOrEmpty(txtBookBarcode.Text))
                //// if Barcode is available in text box then search a user by barcode.
                txtBookBarcode_TextChanged(new object(), EventArgs.Empty);
            else
            {            //// if Barcode is not available in text box then search users according to applied filters.
                string sBook = txtBookBarcode.Text.Trim();
                string sIsBook = string.Empty;
                if (!string.IsNullOrEmpty(sBook))
                    sIsBook = sBook.Substring(0, 1);
                else
                    hidBookId.Value = Constants.S_ZERO;
                string sSchoolId = miSchoolId.ToString();
                if (sIsBook == Constants.BarcodeChar.Book.ToString() && sBook.Contains("P") && sBook.Contains(sSchoolId) && sBook.Length == (sBook.LastIndexOf(sSchoolId) + 2))
                {
                    int iBookId = Convert.ToInt32(sBook.Substring(1, sBook.LastIndexOf('P') - 1));
                    hidBookId.Value = iBookId.ToString();
                }
                
                FillBooksGrid();
                SetDataPager(true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());          
        }
    }

  

    /// <summary>
    /// This event is used to clear book search filter.
    /// </summary>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            trButtons.Visible = true;
            hidBookId.Value = string.Empty;
            txtBookName.Text = string.Empty;
            lblErrorMsg.Text = string.Empty; 
            txtPublisher.Text = string.Empty;
            txtAuthorName.Text = string.Empty;
            txtAccessionNumber.Text = string.Empty;
            txtBookBarcode.Text = string.Empty;
            optPrintable.Checked = false;
            optNonPrintable.Checked = false;
            chkForParentStaff.Checked = false;
            cmbStandard.Enabled = true;
            optAll.Checked = true;
            cmbStandard.Items.Clear();
            cmbLanguage.SelectedIndex = Constants.I_ZERO;
            cmbLanguage.ClearSelection();
            string abc = cmbLanguage.SelectedValue;
            FillStandardCombo();
            hidSortExpression.Value = string.Empty;
            hidSortDirection.Value = string.Empty;
            DataPager dtPgBook = lstvwBookMaster.FindControl("DtPgDropDown") as DataPager;
            if (dtPgBook != null)
            {
                dtPgBook.SetPageProperties(0, dtPgBook.PageSize, false);
                dtPgBook.Visible = true;
            }

            txtBookBarcode.Focus();
            lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear user search filter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUserClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtEmployeeNo.Text = string.Empty;
            trUserHeader.Visible = false;
            trUserInfo.Visible = false;
            cmbUserRole.SelectedValue = Constants.I_THREE.ToString();
            cmbUserClass.Items.Clear();
            cmbUserClass.Items.Add(new ListItem("---All---", Constants.S_ZERO));
            FillClassCombo();
            cmbUserClass.SelectedValue = Constants.S_ZERO;
            cmbUserClass.Enabled = true;
            txtUserName.Text = string.Empty;
            txtRollNo.Enabled = true;
            txtUsrBarcode.Text = string.Empty;
            txtRollNo.Text = string.Empty;
            txtUsrBarcode.Focus();
            lstvwUsers.DataSource = null;
            hidUserSortExpression.Value = string.Empty;
            hidUserSortDirection.Value = string.Empty;
            trEmployeeNo.Visible = false;
            trRollNo.Visible = true;
            trClass.Visible = true;
            lstvwUsers.DataBind();
      }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Cancel issue books.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelBooks_Click(object sender, EventArgs e)
    {
        try
        {
            btnUserClear_Click(new object(), EventArgs.Empty);
            btnClear_Click(new object(), EventArgs.Empty);
            FillUserGrid();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the sorting for book details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            const string S_SORT_BOOK = "asc";
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            FillBooksGrid(e.SortExpression + " " + ViewState[S_SORT_BOOK]);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the sorting for User details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidUserSortDirection.Value == Constants.S_DESCENDING)
				hidUserSortDirection.Value = Constants.S_ASCENDING;
			else
				hidUserSortDirection.Value = Constants.S_DESCENDING;
            
            hidUserSortExpression.Value = e.SortExpression;
            FillUserGrid();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search user by barcode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtUsrBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string sUser = txtUsrBarcode.Text.Trim();
            string sSchoolId = miSchoolId.ToString();
            if (!string.IsNullOrEmpty(sUser))
            {
                trUserHeader.Visible = true;
                trUserInfo.Visible = true;
                string sUserRole=string.Empty;
                if (cmbUserRole.SelectedItem.Text == Constants.UserRoles.Admin.ToString())
                    sUserRole = "M";
                else
                    sUserRole = cmbUserRole.SelectedItem.Text.Substring(0, 1);
                if (!string.IsNullOrEmpty(sUser))
                {
                    try
                    {
                        lstvwUsers.DataSource = IssueReturnBookBL.GetUser(sUserRole, sUser, miSchoolId, miAcademicYearId);
                        lstvwUsers.DataBind();
                    }
                    catch
                    {
                        SetDatasource();
                    }
                }
                else
                    SetDatasource();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to search book by barcode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtBookBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string sBook = txtBookBarcode.Text.Trim();
            string sIsBook = string.Empty;
            string sSchoolId = miSchoolId.ToString();
            if (!string.IsNullOrEmpty(sBook))
                sIsBook = sBook.Substring(0, 1);
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
              try
                {
                    string sBookId = sBook;                    
                    DataTable oDtBookDetails = oIssueReturnBookBL.GetBookDetailsByBarcode(miSchoolId, sBookId);
                    if (oDtBookDetails.Rows.Count > Constants.I_ZERO)
                    {
                        hidBookId.Value = oDtBookDetails.Rows[0]["Book_Id"].ToString();
                        lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
                    }
                    else
                        SetControlState();
                
                    if (lstvwBookMaster.Items.Count > Constants.I_ZERO)
                    {
                        if (oDtBookDetails != null && oDtBookDetails.Rows.Count > Constants.I_ZERO)
                        {
                            HtmlTableRow oHtmlTableRow = lstvwBookMaster.Items[0].FindControl("trBookDetails") as HtmlTableRow;
                            if (oHtmlTableRow != null)
                            {
                                HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdBookDetails") as HtmlTableCell;
                                ListView olstvwBookDetails = oHtmlTableCell.FindControl("lstvwBookDetails") as ListView;
                
                                oHtmlTableRow.Visible = true;
                                olstvwBookDetails.DataSource = oDtBookDetails;
                                olstvwBookDetails.DataBind();
                            }
                
                            hidBookDetailsCount.Value = oDtBookDetails.Rows.Count.ToString();
                            txtUsrBarcode.Focus();
                        }
                    }
                }
                catch
                {
                    SetControlState();
                } 
            }
            else
            {

                if (sIsBook == Convert.ToChar(Constants.BarcodeChar.Book).ToString() && sBook.Contains("P") && sBook.Contains(sSchoolId) && sBook.Length == (sBook.LastIndexOf(sSchoolId) + (sSchoolId.Length)))
                {
                    try
                    {
                        string  sBookId = Convert.ToString(sBook.Substring(1, sBook.LastIndexOf('P') - 1));                        
                        DataTable oDtBookDetails = oIssueReturnBookBL.GetBookDetailsByBarcode(miSchoolId, sBookId);
                        if (oDtBookDetails.Rows.Count > Constants.I_ZERO)
                        {
                            hidBookId.Value = oDtBookDetails.Rows[0]["Book_Id"].ToString();
                            lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
                        }
                        else
                            SetControlState();

                        if (lstvwBookMaster.Items.Count > Constants.I_ZERO)
                        {
                            if (oDtBookDetails != null && oDtBookDetails.Rows.Count > Constants.I_ZERO)
                            {
                                HtmlTableRow oHtmlTableRow = lstvwBookMaster.Items[0].FindControl("trBookDetails") as HtmlTableRow;
                                if (oHtmlTableRow != null)
                                {
                                    HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdBookDetails") as HtmlTableCell;
                                    ListView olstvwBookDetails = oHtmlTableCell.FindControl("lstvwBookDetails") as ListView;

                                    oHtmlTableRow.Visible = true;
                                    olstvwBookDetails.DataSource = oDtBookDetails;
                                    olstvwBookDetails.DataBind();
                                }

                                hidBookDetailsCount.Value = oDtBookDetails.Rows.Count.ToString();
                                txtUsrBarcode.Focus();
                            }
                        }
                    }
                    catch
                    {
                        SetControlState();
                    }
                }

                else
                    SetControlState();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Calss combo box if selected user role is student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
                hidUserRoleID.Value = cmbUserRole.SelectedValue;
                hidUserRole.Value = cmbUserRole.SelectedItem.Text;
                DataPager dt = lstvwUsers.FindControl("DtPgDropDown") as DataPager;
                if (dt != null)
                dt.SetPageProperties(0, 10, false);    
                if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student) ||
                    Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Parent))
                {
                    cmbUserClass.Enabled = true;
                    txtRollNo.Enabled = true;
                    trEmployeeNo.Visible = false;
                    trClass.Visible = true;
                    trRollNo.Visible = true;
                    FillClassCombo();
                }
                else
                {
                    cmbUserClass.Items.Clear();
                    cmbUserClass.Items.Add(new ListItem("---All---", Constants.S_ZERO));
                    cmbUserClass.Enabled = false;
                    txtRollNo.Text = string.Empty;
                    trRollNo.Visible = false;
                    trClass.Visible = false;
                    txtEmployeeNo.Text = string.Empty;
                    trEmployeeNo.Visible = true;
                    txtUsrBarcode.Text = string.Empty;
                }

                // Reset the SortExpression and SortDirection when UserRole is changed.
                hidUserSortDirection.Value = string.Empty;
                hidUserSortExpression.Value = string.Empty;
                hidSortDirection.Value = string.Empty;
                hidSortExpression.Value = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close the users renew/return sub grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancelRenewReturn_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            if (oButton != null) oButton.Parent.Parent.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close the users renew/return sub grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancelBook_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            ListView oLstvwBookDetails = oButton.Parent.FindControl("lstvwBookDetails") as ListView;
            for (int iCnt = 0; iCnt < oLstvwBookDetails.Items.Count; iCnt++)
                ((RadioButton)oLstvwBookDetails.Items[iCnt].FindControl("optSelectToIssue")).Checked = false;
            oButton.Parent.Parent.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show data pager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwBookMaster.Items.Count > Constants.I_ZERO && !string.IsNullOrEmpty(lstvwBookMaster.DataSourceID))
            {
                hidBookCount.Value = lstvwBookMaster.Items.Count.ToString();
                int iCnt = lstvwBookMaster.Items.Count;
                ControlUtility.FillListViewPagerFooter(lstvwBookMaster, DtPgDropDown);
                DropDownList oDropDownList = DtPgDropDown.FindControl("ddlCnt") as DropDownList;

                if (iCnt == Constants.I_ONE && oDropDownList == null && Convert.ToInt32(lstvwBookMaster.DataKeys[0]["Available_Books"]) != Constants.I_ZERO)
                    SelectBook();
                if (DtPgDropDown.TotalRowCount > DtPgDropDown.PageSize)
                    SetDataPager(true);
                else
                    SetDataPager(false);
            }
            else
                SetDataPager(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save issued details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            ShowErrorMessage(false, string.Empty);
            hidReturnDays.Value = GetIssuePeriod().ToString();

            int iNumberOfBookAllowed = IssueReturnBookBL.NumberOfBookAllowed(Convert.ToInt16(cmbUserRole.SelectedValue), miSchoolId, miAcademicYearId);

            if (iNumberOfBookAllowed > Constants.I_ZERO)
            {
                List<IssueReturnBookBL> oLstIssueReturnBookBL = PopulateIsssueBooksBL();

                if (oLstIssueReturnBookBL.Count > 0 && (hidUserID.Value != string.Empty && hidUserID.Value != Constants.S_ZERO))
                {
                    bool bForParent = false;
                    if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Parent))
                    {
                        bForParent = true;
                        hidUserRoleID.Value = Constants.I_THREE.ToString();
                    }

                    int iNumberOfBookIssued = IssueReturnBookBL.NumberOfBookIssued(Convert.ToInt16(hidUserRoleID.Value),
                                                                                    Convert.ToInt16(hidUserID.Value == string.Empty ? Constants.S_ZERO : hidUserID.Value),
                                                                                    miSchoolId,
                                                                                    miAcademicYearId,
                                                                                    bForParent);

                    if (oLstIssueReturnBookBL.Count <= (iNumberOfBookAllowed - iNumberOfBookIssued))
                    {
                        foreach (IssueReturnBookBL oIssueReturnBookBL in oLstIssueReturnBookBL)
                        {
                            oIssueReturnBookBL.IssueBook();
                            txtBookBarcode.Text = string.Empty;
                            FillBooksGrid();
                            FillUserGrid();
                            lblUpdateSucess.Visible = true;
                            lblUpdateSucess.Text = S_ISSUE_MSG;
                            hidUserID.Value = string.Empty;
                        }
                    }
                    else
                    {
                        if (bForParent)
                        {
                            string sErrorMsg = iNumberOfBookAllowed != Constants.I_ZERO ?
                                "More than " + iNumberOfBookAllowed + " book(s) can not be issued to Parent." : S_ALLOWED_ISSUE_BOOK;
                            ShowErrorMessage(true, sErrorMsg);
                        }
                        else
                            ShowErrorMessage(true, "More than " + iNumberOfBookAllowed + " book(s) can not be issued to user " + hidUserName.Value + " (" + hidUserRole.Value + ").");
                    }
                }
                else
                    ShowErrorMessage(true, S_SELECT_MSG);
            }
            else
                ShowErrorMessage(true, S_ALLOWED_ISSUE_BOOK);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region " Private Methods "
    /// <summary>
    /// This method is used to get book issue period.
    /// </summary>
    /// <returns></returns>
    private int GetIssuePeriod()
    {
        IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL
                                                   {
                                                       SchoolId = miSchoolId,
                                                       AcademicYearId = miAcademicYearId
                                                   };
        int iUserRoleId = Convert.ToInt32(cmbUserRole.SelectedValue);
        return oIssueReturnBookBL.GetIssuePeried(iUserRoleId);
    }

    /// <summary>
    /// This method is used to get the sekected books to issue.
    /// </summary>
    /// <returns></returns>
    private List<IssueReturnBookBL> PopulateIsssueBooksBL()
    {
        List<IssueReturnBookBL> oLstIssueReturnBookBL = new List<IssueReturnBookBL>();

        int iUserId = 0;
        DateTime dtActIssueDate = DateTime.Now;
        string sUserName = string.Empty;
        for (int iCount = 0; iCount < lstvwUsers.Items.Count; iCount++)
        {
            RadioButton optSelect = lstvwUsers.Items[iCount].FindControl("optSelectUser") as RadioButton;
            if (optSelect.Checked)
            {
                iUserId = Convert.ToInt32(lstvwUsers.DataKeys[iCount]["UserId"]);
                sUserName = lstvwUsers.DataKeys[iCount]["UserName"].ToString();
                hidUserID.Value = iUserId.ToString();
                hidUserName.Value = sUserName;
                break;
            }
        }

        if (iUserId == Constants.I_ZERO)
            hidUserID.Value = Constants.S_ZERO;


        
        for (int iRowCount = 0; iRowCount < lstvwBookMaster.Items.Count; iRowCount++)
        {
          HtmlTableRow oHtmlTableRow = lstvwBookMaster.Items[iRowCount].FindControl("trBookDetails") as HtmlTableRow;
            if (oHtmlTableRow != null)
            {
                HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdBookDetails") as HtmlTableCell;
                ListView olstvwBookDetails = oHtmlTableCell.FindControl("lstvwBookDetails") as ListView;

                for (int iCount = 0; iCount < olstvwBookDetails.Items.Count; iCount++)
                {
                    RadioButton optSelect = (RadioButton)olstvwBookDetails.Items[iCount].FindControl("optSelectToIssue");
                    if (optSelect.Checked)
                    {
                        IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
                        HiddenField hidBookNo = olstvwBookDetails.Items[iCount].FindControl("hidBookNo") as HiddenField;
                        HiddenField hidBookDetailsId = olstvwBookDetails.Items[iCount].FindControl("hidBookDetailsId") as HiddenField;

                        oIssueReturnBookBL.BookNo = Convert.ToString(hidBookNo.Value);
                        oIssueReturnBookBL.BookId = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowCount]["Book_Id"]);

                        oIssueReturnBookBL.BookDetailsId = Convert.ToInt32(hidBookDetailsId.Value);
                        oIssueReturnBookBL.BookName = lstvwBookMaster.DataKeys[iRowCount]["Book_Title"].ToString();
                        oIssueReturnBookBL.IssueId = iUserId;
                    
                        // Append current time in date. Thid require because while inserting Return date its timing is default 12:00 AM
                        string sDateWithCurrTime = System.DateTime.Today.AddDays(Convert.ToInt32(hidReturnDays.Value)).Date.ToString("M/dd/yyyy");
                        sDateWithCurrTime = sDateWithCurrTime + " " + DateTime.Now.ToString("HH:mm:ss");

                        if (hidActIssueDate.Value != null)
                            dtActIssueDate = Convert.ToDateTime(hidActIssueDate.Value);
                        oIssueReturnBookBL.IssueDate = dtActIssueDate;
                        oIssueReturnBookBL.ReturnDate = sDateWithCurrTime;
                        oIssueReturnBookBL.InsertedById = miUserId;
                        oIssueReturnBookBL.UserId = miUserId;
                        oIssueReturnBookBL.SchoolId = miSchoolId;
                        oIssueReturnBookBL.AcademicYearId = miAcademicYearId;
                        if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Parent))
                            oIssueReturnBookBL.IsForParent = Constants.I_ONE;
                        else
                            oIssueReturnBookBL.IsForParent = Constants.I_ZERO;
                        oLstIssueReturnBookBL.Add(oIssueReturnBookBL);
                    }
                }
            }
        }

        return oLstIssueReturnBookBL;
    }

    /// <summary>
    /// This method is used to fill the user role combo box.
    /// </summary>
    private void FillUserRolesCombo()
    {
        BookBL oBookBL = new BookBL();
        oBookBL.GetUserRoles();
        oBookBL.LstUserRoles.ForEach(User => cmbUserRole.Items.Add(new ListItem(User.User_Role_Name, User.User_Role_Id.ToString())));
    }

    /// <summary>
    /// This method is used to fill the class combo box if the selected user role is student.
    /// </summary>
    private void FillClassCombo()
    {       
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtClass = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ListSource.FillDropDownList(oDtClass, cmbUserClass, "StandardDivision", "SchoolWise_Standard_Division_id", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill language combo.
    /// </summary>
    private void FillLangaugeCombo()
    {
        List<string> lstLangauge = BookBL.GetLanguages(miSchoolId);
        ListSource.FillDropDownList(lstLangauge, cmbLanguage, string.Empty, string.Empty, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill the standard combo box.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill the UserListView.
    /// </summary>
    private void FillUserGrid()
    {
        lstvwUsers.DataSource = null;
        hidUserRoleID.Value = cmbUserRole.SelectedValue;
        hidUserRole.Value = cmbUserRole.SelectedItem.ToString();
		lstvwUsers.DataSourceID = ObjDSUserDetails.ID;
        hidUserRowCount.Value = lstvwUsers.Items.Count.ToString();
	}

	/// <summary>
	/// This method is used to fill the UsersListView with sorting.
	/// </summary>
	private void FillUserGrid(string sSortExpression)
	{
		hidUserRoleID.Value = cmbUserRole.SelectedValue;
		hidUserRowCount.Value = lstvwUsers.Items.Count.ToString();
		lstvwUsers.DataSourceID = ObjDSUserDetails.ID;
	}

    /// <summary>
    /// This method is used to fill the BooksListView.
    /// </summary>
    private void FillBooksGrid()
    {
        lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
    }

    /// <summary>
    /// This method is used to fill the BooksListView with sorting.
    /// </summary>
    /// 
    private void FillBooksGrid(string sSortExpression)
    {
        lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
        hidBookCount.Value = lstvwBookMaster.Items.Count.ToString();
    }

    ///// <summary>
    ///// 
    ///// This method is used to initialized Session variable.
    ///// </summary>
    ///// <returns></returns>
    private BookBL SetMediaType()
    {
        BookBL oBookBL = new BookBL();
        oBookBL.SchoolId = miSchoolId;
        oBookBL.UpdatedById = miUserId;
        oBookBL.InsertedById = miUserId;
        oBookBL.UpdatedDate = DateTime.Today;
        if (optNonPrintable.Checked)
            hidMediaType.Value = Constants.S_ZERO;
        else if (optPrintable.Checked)
            oBookBL.MediaType = Constants.I_ONE;
        else if (optAll.Checked)
            oBookBL.MediaType = Constants.I_TWO;
        hidMediaType.Value = Convert.ToString(oBookBL.MediaType);
        return oBookBL;
    }

    /// <summary>
    /// This method is used for getting Category Details
    /// </summary>
    /// <returns></returns>

    private DataTable GetMainCategoryDetails()
    {
        BookBL oBookBL = SetMediaType();
        DataTable oDtBook = oBookBL.GetMainCategoryDetails();
        return oDtBook;
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        chkForParentStaff.Attributes.Add("onclick", "EnableParentStaffSearch()");
        new Button[] { btnSearch, btnClear, btnIssueBooks, btnCancelBooks, btnUserSearch, btnUserClear }.ApplyEffect();
        string sQueryString = "UserID=" + miUserId;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        hlnkReserveBook.NavigateUrl = hlnkReserveBook.NavigateUrl + sEncrypt;
        hlnkReserveBook.Attributes.Add("onclick", "window.open('" + hlnkReserveBook.NavigateUrl+ "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=950,height=600'); return false;");
        btnIssueBooks.Attributes.Add("onclick", "ShowPopup(this,'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT) + "');return false;");
        btnOk.Attributes.Add("onclick", "if(!ConfirmReturn()){return false;}");
      HtmlForm oForm = (HtmlForm)this.Master.FindControl("form1");
        oForm.DefaultButton = btnSearch.UniqueID;
        ApplyMouseHoverEffect(new List<Button>() { btnOk, btnCancel });
    }

    /// <summary>
    /// This method is used to add exception to error log table in database.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="asMethodName"></param>
    private void AddExceptionToErrorLog(Exception ex, string asMethodName)
    {
        BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
    }

    /// <summary>
    /// This method is used to set row count of user grid and books grid.
    /// </summary>
    private void SetDefaultValues()
    {
        hidBookCount.Value = Convert.ToString(lstvwBookMaster.Items.Count);
        hidUserRowCount.Value = Convert.ToString(lstvwUsers.Items.Count);
        txtBookBarcode.Focus();

        cmbUserRole.SelectedValue = Constants.I_THREE.ToString();
        cmbUserClass.Enabled = true;

        trUserHeader.Visible = false;
        trUserInfo.Visible = false;
    }

    /// <summary>
    /// This procedure is used to add sort image.
    /// </summary>
    private void AddSortImage()
    {
        const string S_DEFAULT_SORT_EXP = "Book_Title";
        const string S_USER_DEFAULT_SORT_EXP = "FirstName";
        //// For User Details
		string sUserSortExpression = S_USER_DEFAULT_SORT_EXP;
		string sUserSortDirection = Constants.S_ASCENDING;
        if (!string.IsNullOrEmpty(hidUserSortExpression.Value))
			sUserSortExpression = hidUserSortExpression.Value;
        if (!string.IsNullOrEmpty(hidUserSortDirection.Value))
			sUserSortDirection = hidUserSortDirection.Value;
		HtmlTableRow oHtmlTableHeaderRow = lstvwUsers.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, sUserSortExpression, sUserSortDirection);
        
        // For Book Details
        string sSortExpression = S_DEFAULT_SORT_EXP;
        string sSortDirection = Constants.S_ASCENDING;
        if (!String.IsNullOrEmpty(hidSortExpression.Value))
            sSortExpression = hidSortExpression.Value;
        if (!string.IsNullOrEmpty(hidSortDirection.Value))
            sSortDirection = hidSortDirection.Value;
        oHtmlTableHeaderRow = lstvwBookMaster.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, sSortExpression, sSortDirection);
    }

    /// <summary>
    /// This method is used to select a book when search result returns single record then book is automatically selected.
    /// </summary>
    private void SelectBook()
    {
        ListViewDataItem oCurrentItem = lstvwBookMaster.Items[0] as ListViewDataItem;
                
        int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
        int iBookId = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowId]["Book_Id"]);
        ListViewDataItem oItem = lstvwBookMaster.Items[iRowId] as ListViewDataItem;
        HtmlTableRow rw = oItem.FindControl("trBookDetails") as HtmlTableRow;
        HtmlTableRow oHtmlTableRow = oItem.FindControl("trBookDetails") as HtmlTableRow;
        if (oHtmlTableRow != null)
        {
            HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdBookDetails") as HtmlTableCell;
            ListView olstvwBookDetails = oHtmlTableCell.FindControl("lstvwBookDetails") as ListView;
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
            DataTable oDtBookDetails = oIssueReturnBookBL.GetBookDetails(miSchoolId, iBookId);
            oHtmlTableRow.Visible = true;
            olstvwBookDetails.DataSource = oDtBookDetails;
            olstvwBookDetails.DataBind();
            hidBookDetailsCount.Value = oDtBookDetails.Rows.Count.ToString();
        }
    }
    
    /// <summary>
    /// This method is used to get cnt of books user can reserve.
    /// </summary>
    /// <param name="bIsForParent"></param>
    /// 
    private void ReserveCountForUser(bool bIsForParent)
    {
        int iUserRole = Convert.ToInt32(cmbUserRole.SelectedValue);
            hidReserveBookCount.Value = BookBL.GetReserveBooksPerPerson(miSchoolId, miAcademicYearId, iUserRole).ToString();
    }

    /// <summary>
    /// This method is used to reserve a book.
    /// </summary>
    /// <param name="e"></param>
    private void ReserveBook(ListViewCommandEventArgs e)
    {
        const string S_ERROR_MSG = "User should be selected.";
        bool bIsForParent = false;
        if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Parent))
            bIsForParent = true;
        GetUserId();
        ReserveCountForUser(bIsForParent);
        if (hidUserID.Value != Constants.S_ZERO && hidUserID.Value != string.Empty)
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowIndex = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iBookId = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowIndex]["Book_Id"]);
            int iReservedCnt = 0;
            int iForParent = bIsForParent ? Constants.I_ONE : Constants.I_ZERO;
        
            iReservedCnt = BookBL.GetReserveCount(miSchoolId, miAcademicYearId, Convert.ToInt32(hidUserID.Value), iBookId, iForParent);
            if (hidReserveBookCount.Value != Constants.S_ZERO)
            SaveReservedBook(iBookId, iReservedCnt, iForParent);
            else
                ShowErrorMessage(true, S_RESERVATION_NOT_ALLOWED);
        }
        else             
            ShowErrorMessage(true, S_ERROR_MSG);
            hidUserID.Value = string.Empty;
            FillUserGrid();
    }

    /// <summary>
    /// This method is used to save reserve a book.
    /// </summary>
    /// <param name="iBookId"></param>
    /// <param name="iReservedCnt"></param>
    /// <param name="iForParent"></param>
    private void SaveReservedBook(int iBookId, int iReservedCnt, int iForParent)
    {
         int I_SAME_BOOK = 999;
        if (iReservedCnt != I_SAME_BOOK)
        {
            if (Convert.ToInt32(hidReserveBookCount.Value) > iReservedCnt)
            {
                BookBL oBookBL = new BookBL
                {
                    BookId = iBookId,
                    SchoolId = miSchoolId,
                    AcademicYearId = miAcademicYearId,
                    InsertedById = miUserId
                };

                oBookBL.UserId = Convert.ToInt32(hidUserID.Value);
                oBookBL.ReservedByParent = iForParent;
                oBookBL.SaveReserveBook();
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = S_SAVE_MSG;
            }
            else
            {
                string sErrMsg = hidReserveBookCount.Value != Constants.S_ZERO ?
                    "Could not claim more than " + hidReserveBookCount.Value + " book(s)." : S_RESERVATION_NOT_ALLOWED;
                ShowErrorMessage(true, sErrMsg);
            }
        }
        else
            ShowErrorMessage(true, S_RESRVED_SAME_BOOK);
    }

    /// <summary>
    /// This method is used to get user id to reserve a book.
    /// </summary>
    private void GetUserId()
    {
        for (int iCount = 0; iCount < lstvwUsers.Items.Count; iCount++)
        {
            RadioButton optSelect = (RadioButton)lstvwUsers.Items[iCount].FindControl("optSelectUser");
            if (optSelect.Checked)
            {
                hidUserID.Value = Convert.ToInt32(lstvwUsers.DataKeys[iCount][S_USER_ID]).ToString();
                break;
            }
        }
    }

    /// <summary>
    /// This method is used to set sort direction.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to Show or hide error message.
    /// </summary>
    /// <param name="bVisible"></param>
    /// <param name="sMessage"></param>
    private void ShowErrorMessage(bool bVisible, string sMessage)
    {
        lblErrorMsg.Visible = bVisible;
        lblErrorMsg.Text = sMessage;
    }

    /// <summary>
    /// This method is used to set data pager according to records.
    /// </summary>
    /// <param name="aiFlag"></param>
    private void SetDataPager(bool aiFlag)
    {
        trPagerBookDetails.Visible = aiFlag;
        DtPgDropDown.Visible = aiFlag;
    }

    /// <summary>
    /// This method is used to set datasource to user listview.
    /// </summary>
    private void SetDatasource()
    {
        lstvwUsers.DataSourceID = string.Empty;
        lstvwUsers.DataSource = new DataTable();
        lstvwUsers.DataBind();
        txtUsrBarcode.Focus();
        txtUsrBarcode.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set control state of book list.
    /// </summary>
    private void SetControlState()
    {
        hidBookId.Value = "-1";        
        lstvwBookMaster.DataBind();       

        DtPgDropDown.Visible = false;
        trButtons.Visible = false;
        txtBookBarcode.Focus();
        txtBookBarcode.Text = string.Empty;
    }

    /// <summary>
    /// This method checks the preconditons to generate Time Table.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.BookManagement);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to visible/hide page controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        trLegend.Visible = false;
        trSearch.Visible = false;
        trUserHeader.Visible = false;
        trUserInfo.Visible = false;
        trlbl.Visible = false;
        trPagerBookDetails.Visible = false;
        trBookListView.Visible = false;
        btnIssueBooks.Visible = false;
        btnCancelBooks.Visible = false;
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void GetQuerystring()
    {
	    if (QueryString.IsNull() || QueryString.Count <= 0)
		    return;
	    
		if (QueryString["BookName"] != null)
		    txtBookName.Text = QueryString["BookName"];
	    if (QueryString["MediaType"] != null )
		    hidMediaType.Value = QueryString["MediaType"];
	    if (QueryString["AuthorName"] != null )
		    txtAuthorName.Text = QueryString["AuthorName"];
	    if (QueryString["Publisher"] != null)
		    txtPublisher.Text = QueryString["Publisher"];
	    if (QueryString["AccessionNumber"] != null )
		    txtAccessionNumber.Text = QueryString["AccessionNumber"];
	    if (QueryString["StandardId"] != null )
		    cmbStandard.SelectedValue = QueryString["StandardId"];
	    if (QueryString["Language"] != null )
		    cmbLanguage.SelectedItem.Text = QueryString["Language"];

	    if (QueryString["BookId"] != null )
	    {
		    int iBookId = Convert.ToInt32(QueryString["UserRoleId"]);
		    FillBooksGrid();
	    }
    }

    /// <summary>
    /// This method is used to set default user selected.
    /// </summary>
    private void SetDefaultUser()
    {
        if (lstvwUsers.Items.Count == Constants.I_ONE)
        {
            if (lstvwUsers.DataKeys[0]["IsActive"].ToString() == Constants.S_YES)
            {
                RadioButton oPtSelect = lstvwUsers.Items[0].FindControl("optSelectUser") as RadioButton;
                if (oPtSelect != null) oPtSelect.Checked = true;
            }
        }
    }

    /// <summary>
    /// This method is used to set data pager for user list.
    /// </summary>
    private void SetUserPager()
    {
        if (lstvwUsers.Items.Count > Constants.I_ZERO)
        {
            DataPager oUsersDtPgCount = lstvwUsers.FindControl("UsersDtPgCount") as DataPager;
            ControlUtility.FillListViewPagerFooter(lstvwUsers, oUsersDtPgCount);
            HtmlTable oTblPagerTable = lstvwUsers.FindControl("tblPagerUserDetails") as HtmlTable;
            if (oTblPagerTable != null)
                oTblPagerTable.Visible = true;
        }
        else
        {
            HtmlTable oTblPagerTable = lstvwUsers.FindControl("tblPagerUserDetails") as HtmlTable;
            if (oTblPagerTable != null)
                oTblPagerTable.Visible = false;
        }
    }

    #endregion " Private Methods "
}
