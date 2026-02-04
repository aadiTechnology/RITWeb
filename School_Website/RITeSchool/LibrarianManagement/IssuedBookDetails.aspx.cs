//Modified By :Rohini
//Date        :21/12/2011
//Description :User can reserve a book .

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class IssuedBookDetails : SchoolBase
{
    #region "Constant"

    const string S_DEFAULT_SORT_EXP = "Book_Title";

    #endregion
    #region "Data member"

    #endregion
    #region "Events"
    /// <summary>
    /// This Event is handled to Add a Sort Image to the Tables
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(Object sender, EventArgs e)
    {
        try
        {
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                FillLangaugeCombo();
                hidSortDirection.Value = Constants.S_ASCENDING;
                hidSortExpression.Value = S_DEFAULT_SORT_EXP;
                FillStandardCombo();
               
                if (Convert.ToInt16(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Constants.I_THREE)
                    FillBooksGridForStudent();
                FillIssuedBooksGrid();
                SetJavaScriptAttribute();
            }
            SetMediaType();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    ///// <summary>
    ///// This event is used for filling catagory combo for all media type.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void optAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwBookMaster.DataBind();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    ///// <summary>
    /////  This event is used for filling catagory combo for printable media type.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void optPrintable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwBookMaster.DataBind();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    ///// <summary>
    /////  This event is used for filling catagory combo for non printable media type.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void optNonPrintable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwBookMaster.DataBind();
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
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwBookMaster);
            DataPager oDataPager = lstvwBookMaster.FindControl("DtPgDropDown") as DataPager;
            DropDownList oDdlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
            hidPageNo.Value = (oDdlCnt.SelectedIndex + 1).ToString();
            ShowErrorMessage(false,string.Empty);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to serach a book.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
            lstvwBookMaster.DataBind();
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
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillBooksGridForStudent();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to clear the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtBookName.Text = string.Empty;
            txtPublisher.Text = string.Empty;
            txtAuthorName.Text = string.Empty;
            txtAccessionNumber.Text = string.Empty;
            optPrintable.Checked = false;
            optNonPrintable.Checked = false;
            optAll.Checked = true;
            cmbStandard.Items.Clear();
            ShowErrorMessage(false, string.Empty);
            cmbLanguage.ClearSelection();
            FillStandardCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to reserve a book.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            const string SORT = "S_Sort";
            string S_RESRVE_BOOK = "Resrve_Book";
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.CommandName == S_RESRVE_BOOK)
                {
                    ReserveBook(e);
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == SORT)
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
    /// This event is used to show datapager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwBookMaster.Items.Count > Constants.I_ZERO && !string.IsNullOrEmpty(lstvwBookMaster.DataSourceID))
            {
                lstvwBookMaster.Items.Clear();
                ControlUtility.FillListViewPagerFooter(lstvwBookMaster, DtPgCount);
                if (DtPgCount.TotalRowCount > DtPgCount.PageSize)
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
    #endregion
    #region "Private Mrethods"
    /// <summary>
    /// This method is used to set data pager.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetDataPager(bool abFlag)
    {
        trPagerBookDetails.Visible = abFlag;
        DtPgCount.Visible = abFlag;
    }
    /// <summary>
    /// This event is used to set attribute to link button.
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
                int iIsForIssue = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowId]["IsForIssue"]);
                if (iIsForIssue == Constants.I_ZERO)
                {
                    HtmlTableRow oHtmlTableHeaderRow = e.Item.FindControl("TrBookMaster") as HtmlTableRow;
                    if (oHtmlTableHeaderRow != null)
                        oHtmlTableHeaderRow.Style.Add(HtmlTextWriterStyle.Color, "red");
                }
                LinkButton oLnkBtn = oCurrentItem.FindControl("lnkbtnReserve") as LinkButton;
                if ((Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]).ToString() == Constants.UserRoles.Student.ToString())
                    oLnkBtn.Attributes.Add("onclick", "ConfirmReservation()");

                    oLnkBtn.Visible =!Convert.ToBoolean(lstvwBookMaster.DataKeys[iRowId]["Available_Books"]);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to hide or show parent button.
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
                int iRowIndex = oCurrentItem.DisplayIndex;
                Image oImgBtnForParent = oCurrentItem.FindControl("imgBtnForParent") as Image;
                oImgBtnForParent.Visible = Convert.ToBoolean(lstvwUsersBookDetails.DataKeys[iRowIndex]["IsForParent"]);
                Control oTdForParent = ((Control)oCurrentItem.FindControl("tdForParent"));
                if (Convert.ToInt32(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Convert.ToInt32(Constants.UserRoles.Student))
                    oTdForParent.Visible = true;
                else
                    oTdForParent.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to show or hide column header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsersBookDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUsersBookDetails.Items.Count > Constants.I_ZERO)
            {
                HtmlTableCell oHtmlTableCellParent = lstvwUsersBookDetails.FindControl("thForParent") as HtmlTableCell;
                if (Convert.ToInt16(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Convert.ToInt16(Constants.UserRoles.Student))
                    oHtmlTableCellParent.Visible = true;
                else
                    oHtmlTableCellParent.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is used to fill books list.
    /// </summary>
    private void FillBooksGridForStudent()
    {
        int iStudentStandard = Convert.ToInt16(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        lstvwBookMaster.DataSourceID = ObjDSBookDetails.ID;
        cmbStandard.SelectedValue = iStudentStandard.ToString();
    }
    /// <summary>
    /// This method is used to set javascript attribute.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        
		ApplyMouseHoverEffect(new List<Button>(){btnClear, btnSearch });
		SetDefaultButton(btnSearch);

		string sQueryString = "UserID=" + miUserId;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        hlnkReserveBook.NavigateUrl = hlnkReserveBook.NavigateUrl + sEncrypt;
        hlnkReserveBook.Attributes.Add("onclick", "window.open('" + hlnkReserveBook.NavigateUrl
                                        + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
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
    /// This method is used get language.
    /// </summary>
    /// <returns></returns>
    private void FillLangaugeCombo()
    {
        List<string> lstLangauge = BookBL.GetLanguages(miSchoolId); ;
        ListSource.FillDropDownList(lstLangauge, cmbLanguage, string.Empty, string.Empty, Constants.S_SELECT_ALL);
    }
    /// <summary>
    /// This method is used to set media type.
    /// </summary>
    private BookBL SetMediaType()
    {
        BookBL oBookBL = new BookBL
        {
            SchoolId = miSchoolId,
            UpdatedById = miUserId,
            InsertedById = miUserId,
            UpdatedDate = DateTime.Today
        };
        if (optNonPrintable.Checked)
            oBookBL.MediaType = Constants.I_ZERO;
        else if (optPrintable.Checked)
            oBookBL.MediaType = Constants.I_ONE;
        else if (optAll.Checked)
            oBookBL.MediaType = Constants.I_TWO;
        hidMediaType.Value = Convert.ToString(oBookBL.MediaType);
        return oBookBL;
    }
    /// <summary>
    /// This event is used to fill books list.
    /// </summary>
    private void FillIssuedBooksGrid()
    {
        IssueReturnBookBL oIssueReturnBookBL = new IssueReturnBookBL();
        DataTable oDtBookDetails = oIssueReturnBookBL.GetIssuedBookDetailsofUser(miSchoolId, miAcademicYearId, miUserId);
        lstvwUsersBookDetails.DataSource = oDtBookDetails;
        lstvwUsersBookDetails.DataBind();
    }
    /// <summary>
    /// This method is used to add sort image.
    /// </summary>
    private void AddSortImage()
    {
        string sSortExpression = S_DEFAULT_SORT_EXP;
        string sSortDirection = Constants.S_ASCENDING;
        if (!String.IsNullOrEmpty(hidSortExpression.Value))
            sSortExpression = hidSortExpression.Value;
        if (!String.IsNullOrEmpty(hidSortDirection.Value))
            sSortDirection = hidSortDirection.Value;
        HtmlTableRow oHtmlTableHeaderRow = lstvwBookMaster.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, sSortExpression, sSortDirection);
    }
    /// <summary>
    /// This event is used to set sort direction.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }
    /// <summary>
    /// This method is used to hid or show the controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void Disablecontrols(bool abAction)
    {
        txtBookName.Enabled = !abAction;
        txtPublisher.Enabled = !abAction;
        txtAuthorName.Enabled = !abAction;
        txtAccessionNumber.Enabled = !abAction;
        optPrintable.Enabled = !abAction;
        optNonPrintable.Enabled = !abAction;
        optAll.Enabled = !abAction;
        cmbStandard.Enabled = !abAction;
    }
    /// <summary>
    /// This method is used to resrve a book.
    /// </summary>
    /// <param name="e"></param>
    private void ReserveBook(ListViewCommandEventArgs e)
    {
        const string S_ERROR_MSG = "Book claim is not allowed.";
        const string S_SAME_BOOK = "Could not claim same book.";
        ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
        int iRowIndex = Convert.ToInt32(oCurrentItem.DisplayIndex);
        int iBookId = Convert.ToInt32(lstvwBookMaster.DataKeys[iRowIndex]["Book_Id"]);
        int iUserRoleId = (hidForParent.Value != Constants.S_ZERO && hidForParent.Value != string.Empty) ? Convert.ToInt32(Constants.UserRoles.Parent) : Convert.ToInt32(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
        int iReserveCountForPerson = BookBL.GetReserveBooksPerPerson(miSchoolId, miAcademicYearId, iUserRoleId); ;
        if (hidForParent.Value == string.Empty)
            hidForParent.Value = Constants.S_ZERO;
        int iReserveCnt = BookBL.GetReserveCount(miSchoolId, miAcademicYearId, miUserId, iBookId, Convert.ToInt32(hidForParent.Value));

        if (iReserveCnt != 999)
        {
            if (iReserveCountForPerson > iReserveCnt)
                SaveReservedBook(miUserId, iBookId);
            else
            {
                if (iReserveCountForPerson != Constants.I_ZERO)
                    ShowErrorMessage(true, "Can not claim more than " + iReserveCountForPerson + " book(s).");
                else
                    ShowErrorMessage(true, S_ERROR_MSG);
            }
        }
        else
            ShowErrorMessage(true,S_SAME_BOOK);
    }
    /// <summary>
    /// This method is used to show error message.
    /// </summary>
    /// <param name="abFlag"></param>
    /// <param name="asMsg"></param>
    private void ShowErrorMessage(bool abFlag,string asMsg)
    {
        lblUpdateSucess.Visible = !abFlag;
        lblError.Visible = abFlag;
        lblError.Text =asMsg;
        lblUpdateSucess.Text = string.Empty;
    }
    /// <summary>
    /// This method is used to save reserved book deatils.
    /// </summary>
    /// <param name="iUserId"></param>
    /// <param name="iBookId"></param>
    private void SaveReservedBook(int iUserId, int iBookId)
    {
        string S_SAVE_MSG = "Book claimed successfully !!!";
        BookBL oBookBL = new BookBL
        {
            BookId = iBookId,
            UserId = miUserId,
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            InsertedById = iUserId,
            ReservedByParent = Convert.ToInt32(hidForParent.Value)
        };
        oBookBL.SaveReserveBook();
        ShowErrorMessage(false, string.Empty);
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = S_SAVE_MSG;
    }
    #endregion
}
