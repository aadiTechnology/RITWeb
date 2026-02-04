// File Name  : IssueRenewReturnUI.aspx.cs
// Created By : Vinod
// Date       : 02 Dec aa
// Description: This class is used toIssue, Renew and Return Books.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;

public partial class IssueRenewReturnUI : SchoolBase
{
    #region  " Constants "

    private const string S_ISSUE_BOOK = "ISSUE";
    private const string S_RENEW_BOOK = "RENEW";
    private const string S_RETURN_BOOK = "RETURN";
    private const string S_MAIL_SUBJECT = "Book availability";
    private const string S_BOOK_CLAIM_MESSAGE = "Claimed Book \'%BOOKNAME%\' is now available in the library, please contact to librarian to issue it.      \n- Regards, %SCHOOL_NAME%.";
    private const string S_ACCESSION_OR_BARCODE_EMPTY_MESSAGE = "Accession No. / Barcode should not be blank.";
    private const string S_RENEW_SUCCESS_MESSAGE = "Book renewed successfully !!!";
    private const int I_PAGE_SIZE = 50;
    private const int I_DATE_LENGTH = 10;

    #endregion

    #region "Data Member"

    private List<IssueBookUserMaster> moLstIssueBookUserMaster;
    BookBL moBookBL = new BookBL();

    #endregion

    #region Events

    /// <summary>
    /// This event is used Initialise controls, set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            valSumErrorMessage.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            if (!IsPostBack)
            {
                SetDefaultButton(btnSearch);
                SetJavaScriptAttribute();
                FillUserRolesCombo();
                FillClassCombo();
            }
            cmbUserRole.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Class combo box if selected user role is student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearAllMessage();
            txtRollNoOrEmpNo.Text = string.Empty;
            if (lstvwUsers.Items.Count > 0)
            {
                DataPager oDataPager = lstvwUsers.FindControl("DtPgDropDown") as DataPager;
                DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                hidPageNo.Value = Constants.I_ZERO.ToString();
            }
            if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student) || Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Parent))
            {
                cmbClass.Enabled = true;
                spnRollNoOrEmpNo.InnerHtml = "Roll No.:";
                FillClassCombo();
                spnStar.Visible = true;
            }
            else
            {
                cmbClass.Items.Clear();
                cmbClass.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
                cmbClass.Enabled = false;
                spnRollNoOrEmpNo.InnerHtml = "Employee No.:";
                txtRollNoOrEmpNo.Text = string.Empty;
                spnStar.Visible = false;

                //HideEnrolmentColumn();
            }
            SetJSAttributeAsPerUserRole();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAllMessage();
        ControlUtility.SetDataPagerAccordingToPageNo(lstvwUsers);
        FillUserListView();
    }

    /// <summary>
    /// This event is used to get user details as per the filter selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAllMessage();
            DtPgCount.SetPageProperties(Convert.ToInt32(hidPageNo.Value), I_PAGE_SIZE, false);
            FillUserListView();
            rdoSingleBook.Checked = true;
            HideEnrolmentColumn();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = oCurrentItem.DisplayIndex;
                int iLateFee = 0;
                HiddenField hidUserId = null;
                HiddenField hidUserBookRenewDetails = oCurrentItem.FindControl("hidUserBookRenewDetails") as HiddenField;
                HiddenField hidUserIssueBookCount = oCurrentItem.FindControl("hidUserIssueBookCount") as HiddenField;
                hidUserBookRenewDetails.Value = ((IssueBookUserMaster)oCurrentItem.DataItem).BookwiseRenewCount;
                ImageButton imgIssue = oCurrentItem.FindControl("imgbtnIssue") as ImageButton;
                ImageButton imgbtnRenew = oCurrentItem.FindControl("imgbtnRenew") as ImageButton;
                ImageButton imgbtnReturn = oCurrentItem.FindControl("imgbtnReturn") as ImageButton;
                TextBox txtAccessionNoOrBarcode = oCurrentItem.FindControl("txtAccessionNoBarcode") as TextBox;
                hidUserId = oCurrentItem.FindControl("hidUserID") as HiddenField;
                hidUserIssueBookCount.Value = hidUserIssueBookCount.Value == string.Empty ? Constants.S_ZERO : hidUserIssueBookCount.Value;
                HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trUser") as HtmlTableRow;
                imgbtnRenew.Visible = imgbtnReturn.Visible = Convert.ToInt32(lstvwUsers.DataKeys[iRowId]["UserIssueBookCount"].ToString()) == Constants.I_ZERO ? false : true;

                HiddenField hidIssueCount = e.Item.FindControl("hidIssueCount") as HiddenField;
                hidIssueCount.Value = lstvwUsers.DataKeys[iRowId]["UserIssueBookCount"].ToString();

                HiddenField hidHasLateEntry = e.Item.FindControl("hidHasLateEntry") as HiddenField;
                hidHasLateEntry.Value = (lstvwUsers.DataKeys[oCurrentItem.DisplayIndex]["HasLateEntry"].ToBool() ? Constants.S_ONE : Constants.S_ZERO);

                if (hidUserIssueBookCount.Value.ToInt() >= 1)
                {
                    DateTime dtTodayDate = DateTime.Today;
                    string sRenewDetail = hidUserBookRenewDetails.Value;
                    string[] saRenewDetails = sRenewDetail.Split(new string[] { "$DOLLER$" }, StringSplitOptions.None);
                    for (int iarrIndex = 0; iarrIndex < saRenewDetails.Length; iarrIndex++)
                    {
                        if (saRenewDetails[iarrIndex] != "")
                        {
                            string sReturnDate = saRenewDetails[iarrIndex].Substring(saRenewDetails[iarrIndex].Length - I_DATE_LENGTH, I_DATE_LENGTH);
                            DateTime dtReturnDate = Convert.ToDateTime(sReturnDate);
                            if (dtReturnDate < DateTime.Today)
                            {
                                iLateFee = Convert.ToInt32(dtTodayDate.Subtract(dtReturnDate).TotalDays) * Convert.ToInt32(hidLateFeePerDay.Value);
                                oHtmlTableRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Pink");
                            }
                        }

                    }
                    imgbtnRenew.Attributes.Add("onclick", "if(!ConfirmRenewBook('" + this + "','" + iRowId + "','" + miSchoolId + "','" + iLateFee + "'," + hidUserId.Value + " )){return false;}");
                }
                imgIssue.Attributes.Add("onclick", "if(!ValidateUserIssueBookCount( '" + iRowId + "', '" + miSchoolId + "','" + hidUserIssueBookCount.ClientID + "')){return false;}");
                txtAccessionNoOrBarcode.Attributes.Add("onkeydown", "return CheckEnteredChar(event, '" + iRowId + "');");
                if (lstvwUsers != null)
                {
                    if (lstvwUsers.DataKeys[iRowId]["IsActive"].ToString() == "N")
                    {
                        if (oHtmlTableRow != null)
                        {
                            (oHtmlTableRow.FindControl("txtAccessionNoBarcode") as TextBox).Enabled = imgbtnRenew.Visible == true ? true : false;
                            oHtmlTableRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gainsboro");
                            oHtmlTableRow.Style.Add(HtmlTextWriterStyle.Color, "red");
                            imgbtnRenew.Visible = imgIssue.Visible = false;
                        }
                    }
                }

                HtmlTableCell otdImgbtnIssue = oCurrentItem.FindControl("tdImgbtnIssue") as HtmlTableCell;
                HtmlTableCell otdimgbtnRenew = oCurrentItem.FindControl("tdimgbtnRenew") as HtmlTableCell;
                HtmlTableCell otdimgbtnReturn = oCurrentItem.FindControl("tdimgbtnReturn") as HtmlTableCell;

                if (rdoBulkBook.Checked && otdImgbtnIssue != null && otdimgbtnRenew != null && otdimgbtnReturn != null)
                {
                    otdImgbtnIssue.Visible = false;
                    otdimgbtnRenew.Visible = false;
                    otdimgbtnReturn.Visible = false;
                }

                HtmlTableCell otdEnrollmentNo = oCurrentItem.FindControl("tdEnrollmentNo") as HtmlTableCell;
                otdEnrollmentNo.Visible = false;

                if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student) || Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Parent))
                    otdEnrollmentNo.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill datapager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (moLstIssueBookUserMaster.Count > 0)
            {
                if (lstvwUsers.Items.Count > Constants.I_ZERO)
                {
                    trLegend.Visible = true;
                    trBookAssignment.Visible = true;
                    SetHeadearText();
                    DataPager oDataPager = lstvwUsers.FindControl("DtPgDropDown") as DataPager;
                    DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                    hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
                    ddlCnt.Attributes.Add("onchange", "");
                }
            }
            else
            {
                trLegend.Visible = trPagerUser.Visible = false;
                trBookAssignment.Visible = false;
                rdoBulkBook.Checked = false;
                trBulkStudentsButton.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// Renew book for user who allready have late fee for same book
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLateFee_Click(object sender, EventArgs e)
    {
        try
        {
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
            int iRowIndex = Convert.ToInt32(hidRowNo.Value);
            int iUserId = Convert.ToInt32(hidUserId.Value);
            oIssueReturnBookBL.RenewUserBook(iUserId, Convert.ToInt32(cmbUserRole.SelectedValue), hidtxtAccessionOrBarcode.Value, miSchoolId, miAcademicYearId, miUserId);
            if (oIssueReturnBookBL.LstUserBookRenewDetails.Count > 0)
                hidUserBookRenewDetails.Value = oIssueReturnBookBL.LstUserBookRenewDetails.First().BookwiseRenewCount.ToString();
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = S_RENEW_SUCCESS_MESSAGE;
            lblErrorMsg.Text = string.Empty;
            FillUserListView();
            SaveLateFeeDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for issue, return and renew book.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ClearAllMessage();
            int iRowIndex = e.Item.DisplayIndex;
            int iUserId = Convert.ToInt32(lstvwUsers.DataKeys[iRowIndex]["UserId"].ToString());
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
            TextBox txtAccessionNoBarcode = e.Item.FindControl("txtAccessionNoBarcode") as TextBox;
            HiddenField hidUserIssueBookCount = e.Item.FindControl("hidUserIssueBookCount") as HiddenField;
            hidtxtAccessionOrBarcode.Value = txtAccessionNoBarcode.Text;
            ImageButton imgIssue = e.Item.FindControl("imgbtnIssue") as ImageButton;
            ImageButton imgbtnRenew = e.Item.FindControl("imgbtnRenew") as ImageButton;
            ImageButton imgbtnReturn = e.Item.FindControl("imgbtnReturn") as ImageButton;
            HiddenField hidUserBookRenewDetails = e.Item.FindControl("hidUserBookRenewDetails") as HiddenField;
            HiddenField hidUserBookNo = e.Item.FindControl("hidUserBookNo") as HiddenField;
            TextBox txtbxAcccessionNo = e.Item.FindControl("txtAccessionNoBarcode") as TextBox;
            hidtxtAccessionOrBarcode.Value = txtbxAcccessionNo.Text;
            if (e.CommandName == S_ISSUE_BOOK)
            {
                int iReturnDays = GetIssuePeriod();

                ////Add no. of return days in Current date to set Return Date.
                string sDateWithCurrTime = System.DateTime.Today.AddDays(iReturnDays).Date.ToString("M/dd/yyyy");
                oIssueReturnBookBL.IssueBookToUser(iUserId, sDateWithCurrTime, Convert.ToInt32(cmbUserRole.SelectedValue), txtAccessionNoBarcode.Text, miSchoolId, miAcademicYearId, miUserId);

                SetSuccessfullMessage(oIssueReturnBookBL, "Book issued successfully !!!", txtAccessionNoBarcode, hidUserBookRenewDetails);

                hidUserIssueBookCount.Value = (Convert.ToInt32(hidUserIssueBookCount.Value) + 1).ToString();

                ////Set Renew an dReturn button visiblity.
                imgbtnRenew.Visible = imgbtnReturn.Visible = Convert.ToInt32(hidUserIssueBookCount.Value) == Constants.I_ZERO ? false : true;
            }
            else if (e.CommandName == S_RENEW_BOOK)
            {
                oIssueReturnBookBL.RenewUserBook(iUserId, Convert.ToInt32(cmbUserRole.SelectedValue), txtAccessionNoBarcode.Text, miSchoolId, miAcademicYearId, miUserId);
                SetSuccessfullMessage(oIssueReturnBookBL, S_RENEW_SUCCESS_MESSAGE, txtAccessionNoBarcode, hidUserBookRenewDetails);
                FillUserListView();
            }
            else if (e.CommandName == S_RETURN_BOOK)
            {
                if (txtbxAcccessionNo.Text != string.Empty)
                {
                    int iLateFee = 0;
                    oIssueReturnBookBL.GetBookReturnDate(iUserId, Convert.ToInt32(cmbUserRole.SelectedValue), txtAccessionNoBarcode.Text, miSchoolId, miAcademicYearId, miUserId);
                    List<IssueReturnDateMaster> lstIssueReturnDateMaster = oIssueReturnBookBL.LstIssueReturnDateMaster;
                    DateTime dtIssueDate = Convert.ToDateTime(lstIssueReturnDateMaster.First().IssueDate.ToString());
                    DateTime dtReturnDate = Convert.ToDateTime(lstIssueReturnDateMaster.First().ReturnDate.ToString());
                    hidBookName.Value = lstIssueReturnDateMaster.First().MailReserveBookUserMaster.BookName.ToString();
                    hidBookReserveUserList.Value = lstIssueReturnDateMaster.First().MailReserveBookUserMaster.BookReserveUserList.ToString();
                    DateTime dtTodayDate = DateTime.Today;
                    if (dtReturnDate < dtTodayDate)
                        iLateFee = Convert.ToInt32(dtTodayDate.Subtract(dtReturnDate).TotalDays) * Convert.ToInt32(hidLateFeePerDay.Value);
                    ImageButton imgReturn = e.Item.FindControl("imgbtnReturn") as ImageButton;
                    string sAccessionNo = txtbxAcccessionNo.Text.Replace("'", "\\'").ToString();
                    btnReturnBook.Attributes.Add("onclick", "if(!ConfirmReturnBook(' " + iRowIndex + "','" + miSchoolId + "','" + sAccessionNo + "')){return false;}");
                    hidtxtAccessionOrBarcode.Value = hidtxtAccessionOrBarcode.Value.Replace("'", "\\'");
                    ScriptManager.RegisterStartupScript(imgReturn, this.GetType(), "ShowPopup", "ShowPopup(this,'" + hidtxtAccessionOrBarcode.Value + "','" + dtReturnDate.ToShortDateString() + "','" + dtIssueDate.ToShortDateString() + "'," + iLateFee + "," + iRowIndex + "," + iUserId + ")", true);
                }
                else
                {
                    lblErrorMsg.Text = S_ACCESSION_OR_BARCODE_EMPTY_MESSAGE;
                    lblUpdateSucess.Text = string.Empty;
                }
            }
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message + " for Serial number : " + (Convert.ToInt32(e.Item.DataItemIndex) + 1);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method is used to return book.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturnBook_Click(object sender, EventArgs e)
    {
        try
        {
            ReturnBook();
            if (txtLateFee.Text != string.Empty)
            {
                SaveLateFeeDetails();
                txtLateFee.Text = "";
            }
            HiddenField hidUserIssueBookCount = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)].FindControl("hidUserIssueBookCount") as HiddenField;
            ListViewDataItem oListViewDataItem = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)];
            FillUserListView();
            ImageButton imgIssue = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)].FindControl("imgbtnIssue") as ImageButton;
            ImageButton imgbtnRenew = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)].FindControl("imgbtnRenew") as ImageButton;
            ImageButton imgbtnReturn = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)].FindControl("imgbtnReturn") as ImageButton;
            hidUserIssueBookCount.Value = hidUserIssueBookCount.Value != Constants.I_ZERO.ToString() ? (Convert.ToInt32(hidUserIssueBookCount.Value) - 1).ToString() : hidUserIssueBookCount.Value;
            oListViewDataItem.Visible = imgbtnRenew.Visible = imgbtnReturn.Visible = Convert.ToInt32(hidUserIssueBookCount.Value) == Constants.I_ZERO ? false : true;
            ScriptManager.RegisterStartupScript(btnReturnBook, this.GetType(), "ShowPopup", "HidePopup();", true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change the Control state for Single book implementation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoSingleBook_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserListView();
            SetListHeaderVisibility(true);
            trBulkStudentsButton.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Change all contrils for Bulk implementataion.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoBulkBook_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserListView();
            SetListHeaderVisibility(false);
            trBulkStudentsButton.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Issue the books in bulk.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIssue_Click(object sender, EventArgs e)
    {
        try
        {
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
            int iReturnDays = GetIssuePeriod();
            string sDateWithCurrTime = System.DateTime.Today.AddDays(iReturnDays).Date.ToString("M/dd/yyyy");

            string xmlBookIssueDetails = PopulateBookIssueDetails();
            oIssueReturnBookBL.IssueBooksToUserInBulk(sDateWithCurrTime, cmbUserRole.SelectedValue.ToInt(), miSchoolId, miAcademicYearId, miUserId, xmlBookIssueDetails, Constants.I_ONE);
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "Book issued successfully !!!";
            lblErrorMsg.Text = string.Empty;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to renew the books in bulk.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRenew_Click(object sender, EventArgs e)
    {
        try
        {
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
            int iReturnDays = GetIssuePeriod();
            string sDateWithCurrTime = System.DateTime.Today.AddDays(iReturnDays).Date.ToString("M/dd/yyyy");

            string xmlBookIssueDetails = PopulateBookIssueDetails();
            oIssueReturnBookBL.IssueBooksToUserInBulk(sDateWithCurrTime, cmbUserRole.SelectedValue.ToInt(), miSchoolId, miAcademicYearId, miUserId, xmlBookIssueDetails, Constants.I_TWO);
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = S_RENEW_SUCCESS_MESSAGE;
            lblErrorMsg.Text = string.Empty;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to return the book in bulk.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
            int iReturnDays = GetIssuePeriod();
            string sDateWithCurrTime = System.DateTime.Today.AddDays(iReturnDays).Date.ToString("M/dd/yyyy");

            string xmlBookIssueDetails = PopulateBookIssueDetails();
            oIssueReturnBookBL.IssueBooksToUserInBulk(sDateWithCurrTime, cmbUserRole.SelectedValue.ToInt(), miSchoolId, miAcademicYearId, miUserId, xmlBookIssueDetails, Constants.I_THREE);
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "Book returned successfully!!!";
            lblErrorMsg.Text = string.Empty;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblUpdateSucess.Text = string.Empty;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

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
            LateFee = txtLateFee.Text != string.Empty ? Convert.ToInt32(txtLateFee.Text) : Constants.I_ZERO,
            AcademicYearId = miAcademicYearId
        };
        oIssueReturnBookBL.SaveLateFee();
    }
    /// <summary>
    /// This method is used to set header text as per the user role selection. 
    /// </summary>
    private void SetHeadearText()
    {
        HtmlTableRow oHtmlTableRow = lstvwUsers.FindControl("trHeader") as HtmlTableRow;
        Label lblRollNoEmpNo = oHtmlTableRow.FindControl("lblRollNoEmployeeNo") as Label;
        Label lblClassDesig = oHtmlTableRow.FindControl("lblDesignationClass") as Label;
        if (Convert.ToInt32(cmbUserRole.SelectedValue) != Constants.I_THREE && Convert.ToInt32(cmbUserRole.SelectedValue) != 9)
        {
            lblRollNoEmpNo.Text = "Employee No.";
            lblClassDesig.Text = "Designation";
        }
        else
        {
            lblRollNoEmpNo.Text = "Roll No.";
            lblClassDesig.Text = "Class";
        }
    }

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
        return oIssueReturnBookBL.GetIssuePeried(Convert.ToInt32(cmbUserRole.SelectedValue));
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
        DataTable dtClass = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ListSource.FillDropDownList(dtClass, cmbClass, "StandardDivision", "SchoolWise_Standard_Division_id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill user list view.
    /// </summary>
    private void FillUserListView()
    {
        //BookBL oBookBL = new BookBL();
        moBookBL.SchoolId = miSchoolId;
        moBookBL.AcademicYearId = miAcademicYearId;
        moBookBL.GetAllUsersForIssueBook(GenerateXml(PopulateIssueBookUserDetailsObject()));
        moLstIssueBookUserMaster = moBookBL.LstIssueBookUserMaster;
        if (moBookBL.LstIssueBookUserMaster.Count > 0)
        {
            ////All BookId's, BookDetailsId's and BookNames (with comma seperated) set to hidden variable. 
            ////Set Book Issue and Return Max Count.
            if (moBookBL.LstBookIssueRenewCountMaster.Count > 0)
            {
                trLegend.Visible = trPagerUser.Visible = true;
                hidMaxIssueBookCount.Value = moBookBL.LstBookIssueRenewCountMaster.First().MaxIssueBookCount.ToString();
                hidMaxRenewBookCount.Value = moBookBL.LstBookIssueRenewCountMaster.First().MaxRenewBookCount.ToString();
                ////Late Fee Dtails
                hidLateFeePerDay.Value = moBookBL.LstBookIssueRenewCountMaster.First().LateFeePerDay.ToString();
                hidLateFeeEffectiveFrom.Value = moBookBL.LstBookIssueRenewCountMaster.First().LateFeeEffectiveFrom.ToString();
            }
            else
                hidMaxRenewBookCount.Value = hidMaxIssueBookCount.Value = string.Empty;
            lblErrorMsg.Text = string.Empty;
            lstvwUsers.DataSource = moBookBL.LstIssueBookUserMaster;
            lstvwUsers.DataBind();
            ControlUtility.FillListViewPagerFooter(lstvwUsers, DtPgCount);
        }
        else
        {
            trLegend.Visible = trPagerUser.Visible = false;
            lstvwUsers.DataSource = null;
            lstvwUsers.DataBind();
        }
    }

    /// <summary>
    /// This method is used to populate "IssueBookUserDetails" object.
    /// </summary>
    /// <returns></returns>
    private List<IssueBookUserMaster> PopulateIssueBookUserDetailsObject()
    {
        string sUserBarcode = txtBarcode.Text.Trim();
        IssueBookUserDetails oIssueBookUserDetails = new IssueBookUserDetails();
        IssueBookUserMaster oIssueBookUserMaster = new IssueBookUserMaster();
        List<IssueBookUserMaster> lstIssueBookUserMaster = new List<IssueBookUserMaster>();
        int iUserId = Constants.I_ZERO;
        oIssueBookUserDetails.UserRoleId = cmbUserRole.SelectedValue.ToInt();
        oIssueBookUserMaster.UserId = iUserId;
        oIssueBookUserMaster.StandardDivisionId = Convert.ToInt32(cmbClass.SelectedValue);
        oIssueBookUserMaster.IsActive = chkShowDeactiveUser.Checked ? Constants.S_NO : Constants.S_YES;
        if (!string.IsNullOrEmpty(sUserBarcode))
        {
            ////Take first character from barcode string to find user role.
            oIssueBookUserMaster.EnrollOrEmpNo = sUserBarcode;
        }

        oIssueBookUserDetails.RollNoOrEmployeeNo = txtRollNoOrEmpNo.Text.Trim();
        oIssueBookUserMaster.UserName = txtUserName.Text.Trim().Replace("'", "''");
        oIssueBookUserMaster.IssueBookUserDetail = oIssueBookUserDetails;
        lstIssueBookUserMaster.Add(oIssueBookUserMaster);
        return lstIssueBookUserMaster;
    }

    /// <summary>
    /// This method is used to return book.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    private void ReturnBook()
    {
        System.DateTime dtActReturnDate = System.DateTime.Now;
        IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
        oIssueReturnBookBL.SchoolId = miSchoolId;
        oIssueReturnBookBL.AcademicYearId = miAcademicYearId;
        oIssueReturnBookBL.BookNo = hidtxtAccessionOrBarcode.Value.Replace("\\", "");
        oIssueReturnBookBL.UpdatedById = miUserId;
        oIssueReturnBookBL.UserId = Convert.ToInt32(lstvwUsers.DataKeys[Convert.ToInt32(hidRowNo.Value)]["UserId"].ToString());
        if (hidActReturnDate.Value != string.Empty)
            dtActReturnDate = Convert.ToDateTime(hidActReturnDate.Value);

        ////Append current time in date. Thid require because while inserting Return date its timing is default 12:00 AM
        string sDateWithCurrTime = dtActReturnDate.Date.ToString("M/dd/yyyy");
        sDateWithCurrTime = sDateWithCurrTime + " " + DateTime.Now.ToString("HH:mm:ss");

        oIssueReturnBookBL.ActualReturnDate = sDateWithCurrTime;
        oIssueReturnBookBL.ReturnUserBook();
        HiddenField hidUserBookRenewDetails = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)].FindControl("hidUserBookRenewDetails") as HiddenField;
        TextBox txtAccessionNoBarcode = lstvwUsers.Items[Convert.ToInt32(hidRowNo.Value)].FindControl("txtAccessionNoBarcode") as TextBox;
        SendMailToReserveBookUsers();
        SetSuccessfullMessage(oIssueReturnBookBL, "Book returned successfully!!!", txtAccessionNoBarcode, hidUserBookRenewDetails);
    }

    /// <summary>
    /// This method is used to send mail to all users who reserve book.
    /// </summary>
    private void SendMailToReserveBookUsers()
    {
        if (hidBookReserveUserList.Value != String.Empty)
            SendMessage(hidBookReserveUserList.Value, S_MAIL_SUBJECT, S_BOOK_CLAIM_MESSAGE.Replace("%BOOKNAME%", hidBookName.Value).Replace("%SCHOOL_NAME%", ConfigurationManager.AppSettings["SchoolName"]));
    }

    /// <summary>
    /// This method is used to send mail to all users who have claim for this book.
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
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        btnCancel.Attributes.Add("onclick", "return ClearAllControl();");
        ApplyMouseHoverEffect(new List<Button>() { btnCancel, btnClose, btnReturnBook, btnSearch });
        hidShowLateValidation.Value = (miSchoolId == Constants.SchoolId.SPS.ToInt() ? Constants.S_ONE : Constants.S_ZERO);
        btnIssue.Attributes.Add("onclick", "if(!ValidateLateCase()) return false;");
    }

    /// <summary>
    /// This method is used to clear messages.
    /// </summary>
    private void ClearAllMessage()
    {
        lblErrorMsg.Text = lblUpdateSucess.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set javascipt attribute as per the user role.
    /// </summary>
    private void SetJSAttributeAsPerUserRole()
    {
        if (cmbUserRole.SelectedValue == Constants.I_THREE.ToString())
        {
            txtRollNoOrEmpNo.Attributes.Add("onkeyup", "extractNumber(this,0,false)");
            txtRollNoOrEmpNo.Attributes.Add("onkeyup", "extractNumber(this,0,false)");
            txtRollNoOrEmpNo.MaxLength = Constants.I_FIVE;
        }
        else
        {
            txtRollNoOrEmpNo.Attributes.Add("onkeyup", "");
            txtRollNoOrEmpNo.Attributes.Add("onkeyup", "");
            txtRollNoOrEmpNo.MaxLength = 10;
        }
    }

    /// <summary>
    /// This method is used to set sucessfull message.
    /// </summary>
    /// <param name="oIssueReturnBookBL"></param>
    /// <param name="asMessage"></param>
    /// <param name="txtAccessionNoBarcode"></param>
    /// <param name="hidUserBookRenewDetails"></param>
    private void SetSuccessfullMessage(IssueReturnBookBL oIssueReturnBookBL, string asMessage, TextBox txtAccessionNoBarcode, HiddenField hidUserBookRenewDetails)
    {
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = asMessage;
        lblErrorMsg.Text = string.Empty;
        txtAccessionNoBarcode.Text = string.Empty;
        if (oIssueReturnBookBL.LstUserBookRenewDetails.Count > 0)
            hidUserBookRenewDetails.Value = oIssueReturnBookBL.LstUserBookRenewDetails.First().BookwiseRenewCount.ToString();
    }

    private void SetListHeaderVisibility(bool bFlag)
    {
        HtmlTableRow tr = lstvwUsers.FindControl("trHeader") as HtmlTableRow;
        if (tr != null)
        {
            HtmlTableCell thIssue = tr.FindControl("thBookIssue") as HtmlTableCell;
            HtmlTableCell thBookRenew = tr.FindControl("thBookRenew") as HtmlTableCell;
            HtmlTableCell thBookReturn = tr.FindControl("thBookReturn") as HtmlTableCell;

            if (thIssue != null)
                thIssue.Visible = bFlag;

            if (thBookRenew != null)
                thBookRenew.Visible = bFlag;

            if (thBookReturn != null)
                thBookReturn.Visible = bFlag;
        }
    }

    /// <summary>
    /// This method is used to Populate all the book information for Bulk book.s
    /// </summary>
    /// <returns></returns>
    private string PopulateBookIssueDetails()
    {
        int i = Constants.I_ZERO;
        List<BulkBookDetails> lstBulkBookDetails = new List<BulkBookDetails>();
        int iCount = lstvwUsers.Items.Count;
        int iCounter = Constants.I_ZERO;

        while (i < lstvwUsers.Items.Count)
        {
            TextBox txtBookIssue = (TextBox)lstvwUsers.Items[i].FindControl("txtAccessionNoBarcode");
            HiddenField hidUserIssueBookCount = lstvwUsers.Items[i].FindControl("hidUserIssueBookCount") as HiddenField;
            if (txtBookIssue.Text != string.Empty)
            {
                BulkBookDetails oBulkBookDetails = new BulkBookDetails();
                oBulkBookDetails.UserId = Convert.ToInt32(lstvwUsers.DataKeys[i]["UserId"]);
                oBulkBookDetails.AccessionNo = txtBookIssue.Text;

                lstBulkBookDetails.Add(oBulkBookDetails);
            }
            else
            {
                iCounter++;
            }

            if (iCounter == iCount)
            {
                throw new ApplicationException("Please enter at list one Accession No. / Barcode.");
            }

            i++;
        }
        return base.GenerateXml(lstBulkBookDetails);
    }

    /// <summary>
    /// This method is used to set enrolment No column as visible false;
    /// </summary>
    private void HideEnrolmentColumn()
    {
        HtmlTableRow tr = lstvwUsers.FindControl("trHeader") as HtmlTableRow;

        if (tr != null)
        {
            HtmlTableCell thEnrolmentNo = tr.FindControl("thGrNo") as HtmlTableCell;
            if (thEnrolmentNo != null)
            {
                if (Convert.ToInt32(cmbUserRole.SelectedValue) != Convert.ToInt32(Constants.UserRoles.Student) && Convert.ToInt32(cmbUserRole.SelectedValue) != Convert.ToInt32(Constants.UserRoles.Parent))
                    thEnrolmentNo.Visible = false;
                else
                    thEnrolmentNo.Visible = true;
            }
        }
    }

    #endregion
}