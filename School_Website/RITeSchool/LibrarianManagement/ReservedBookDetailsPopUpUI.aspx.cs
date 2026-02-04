// File Name  : ReservedBookDetailsPopUpUI.aspx.cs
// Created By : Rohini
// Date       : 07/11/2011
//Description : This class is used to show all reserve books to user.

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class ReservedBookDetailsPopUpUI : SchoolBase
{
    #region "Constants"

    const string S_DEFAULT_SORT_EXP = "Book_Title";
    const string S_SORT = "S_SORT";
    const string S_CANCEL_RESERVATION = "CANCEL_RESERVATION";
    const string S_SAVE_MESSAGE = "Book claim cancelled successfully !!!";

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to add the sort image for the Ledger list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
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
    /// This event is used to set default values to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
			if (!IsPostBack)
            {
                GetQueryString();
                SetDefaultValues();
                FillReservedBookList();
            }
            lblUpdateSucess.Text = string.Empty;
            lblUpdateSucess.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update the ListView pager controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwReservedBooks);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evwnt is used to initialize data pager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReservedBooks_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwReservedBooks.Items.Count > Constants.I_ZERO)
            {
                SetListviewColumnVisibility();
                // Initialize the DataPager control
                DataPager oDtPgCount = lstvwReservedBooks.FindControl("DtPgCount") as DataPager;
                ControlUtility.FillListViewPagerFooter(lstvwReservedBooks, oDtPgCount);
            };
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel book reservation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReservedBooks_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if(e.Item.ItemType==ListViewItemType.DataItem)
            {
                if (e.CommandName ==S_CANCEL_RESERVATION)
                {
                    CancelBookReservation(e);
                    if (lstvwReservedBooks.Items.Count == Constants.I_ONE)
                    {
                        DataPager oDtPgCount = lstvwReservedBooks.FindControl("DtPgCount") as DataPager;
                        oDtPgCount.SetPageProperties(Constants.I_ZERO, oDtPgCount.PageSize, true);
                    }
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT)
            {
                if (hidSortExpression.Value != e.CommandArgument.ToString())
                    hidSortDirection.Value = Constants.S_DESCENDING;
                hidSortExpression.Value = e.CommandArgument.ToString();
                SetSortVariables();
                FillReservedBookList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to listview controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReservedBooks_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowindex = oCurrentItem.DisplayIndex;
                if (oCurrentItem != null)
                { 
                    bool bIsUserHasfullAccess = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.BookManagement);
                    Control oTdForParent = oCurrentItem.FindControl("tdParent") as Control;
                    Control oTdDesignation = oCurrentItem.FindControl("tdDesignation") as Control;
                    Control oTdClass = oCurrentItem.FindControl("tdClass") as Control;
                    Control oTdUserName =oCurrentItem.FindControl("tdUserName") as Control;

                    ImageButton oImgBtnCancel = oCurrentItem.FindControl("imgBtnCancel") as ImageButton;
                    Image oImgBtnForParent = oCurrentItem.FindControl("imgBtnForParent") as Image;
                    oImgBtnForParent.Visible = Convert.ToBoolean(lstvwReservedBooks.DataKeys[iRowindex]["IsForParent"]);
                    ImageButton imgBtn = oCurrentItem.FindControl("imgBtnCancel") as ImageButton;
                    if (imgBtn != null) imgBtn.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");

                    Label lblDate = oCurrentItem.FindControl("lblDate") as Label;
                    if (lblDate != null)
                    {
                        DateTime dtReservationDate = Convert.ToDateTime(((BookEntities.LibaryUsers)oCurrentItem.DataItem).ReservationDate);
                        lblDate.Text = dtReservationDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
                    }
                    if (tdChkAll != null)
                    {
                        oTdDesignation.Visible = chkShowAll.Checked;
                        oTdClass.Visible = chkShowAll.Checked;
                        oTdUserName.Visible = chkShowAll.Checked;
                    }
                    if (miUserId == Convert.ToInt32(lstvwReservedBooks.DataKeys[iRowindex]["UserId"]) || moUserRole == Constants.UserRoles.Admin || bIsUserHasfullAccess)
                        oImgBtnCancel.Visible = true;
                    else
                        oImgBtnCancel.Visible = false;

	                if (moUserRole == Constants.UserRoles.Student)
		                oTdForParent.Visible = true;
	                else if (moUserRole == Constants.UserRoles.Admin || bIsUserHasfullAccess)
	                {
		                oTdForParent.Visible = true;
		                oTdDesignation.Visible = true;
		                oTdUserName.Visible = true;
		                oTdClass.Visible = true;
	                }
	                else
		                oTdForParent.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search a book or user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillReservedBookList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show all books reserved by all users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataPager oDtPgCount = lstvwReservedBooks.FindControl("DtPgCount") as DataPager;
            if(oDtPgCount!=null)
            oDtPgCount.SetPageProperties(0, oDtPgCount.PageSize, true);
            FillReservedBookList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        string sUserId=GettQuerystring();
        if (sUserId != string.Empty)
            hidUserId.Value = sUserId;
    }

    /// <summary>
    /// This method is used to get all reserved books.
    /// </summary>
    private void FillReservedBookList()
    {
        lstvwReservedBooks.DataSourceID = objdsReservedBooksList.ID;
    }

    /// <summary>
    /// This method is used to set javascript attribute.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;        
		ApplyMouseHoverEffect(new List<Button> { btnClose, btnSearch, btnClear });
        btnClose.Attributes.Add("onclick", "window.close()");
        btnClear.Attributes.Add("onclick", "ClearControl()");
        bool bIsUserHasfullAccess = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.BookManagement);
        if (moUserRole == Constants.UserRoles.Admin || bIsUserHasfullAccess)
            tdChkAll.Visible = false;
		SetDefaultButton(btnSearch);
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string GettQuerystring()
    {
        return QueryString["UserID"] ?? String.Empty;
    }

    /// <summary>
    /// This method is used to cancel book reservation.
    /// </summary>
    /// <param name="e"></param>
    private void CancelBookReservation(ListViewCommandEventArgs e)
    {
        lblUpdateSucess.Text = string.Empty;
        ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
        int iRowindex = oCurrentItem.DisplayIndex;

        if (lstvwReservedBooks != null)
        {
            BookBL oBookBL = new BookBL();
            oBookBL.CancelBookReservation(Convert.ToInt32(lstvwReservedBooks.DataKeys[iRowindex]["UserId"])
                                          , Convert.ToInt32(lstvwReservedBooks.DataKeys[iRowindex]["Book_Id"])
                                          , miSchoolId
                                          , miAcademicYearId);
        }
        FillReservedBookList();
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = S_SAVE_MESSAGE;
    }

    /// <summary>
    /// This method is used to add sort image.
    /// </summary>
    private void AddSortImage()
    {
        if (string.IsNullOrEmpty(hidSortExpression.Value))
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwReservedBooks.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set sort direction.
    /// </summary>
    private void SetSortVariables()
    {
	    hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

	/// <summary>
    /// This method is used show or hide listview columns.
    /// </summary>
    private void SetListviewColumnVisibility()
    {
        bool bIsUserHasfullAccess = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.BookManagement);
        HtmlTableCell oHtmlTableCellParent = lstvwReservedBooks.FindControl("thForParent") as HtmlTableCell;
        HtmlTableCell oHtmlTableCellDesignation = lstvwReservedBooks.FindControl("thDesignation") as HtmlTableCell;
        HtmlTableCell oHtmlTableCellUserName = lstvwReservedBooks.FindControl("thUserName") as HtmlTableCell;
        HtmlTableCell oHtmlTableCellClass =lstvwReservedBooks.FindControl("thClass") as HtmlTableCell;
        HtmlTableCell oHtmlTableUserName = lstvwReservedBooks.FindControl("thUserName") as HtmlTableCell;
        if (tdChkAll != null)
        {
            oHtmlTableCellDesignation.Visible = chkShowAll.Checked;
            oHtmlTableCellClass.Visible = chkShowAll.Checked;
            oHtmlTableUserName.Visible = chkShowAll.Checked;
        }
        if (moUserRole == Constants.UserRoles.Student)
        {
            oHtmlTableCellParent.Visible = true;
            tdChkAll.Visible = true;
        }
        else if (moUserRole == Constants.UserRoles.Admin || bIsUserHasfullAccess)
        {
            oHtmlTableCellParent.Visible = true;
            oHtmlTableCellDesignation.Visible = true;
            oHtmlTableCellClass.Visible = true;
            oHtmlTableCellUserName.Visible = true;
            tdChkAll.Visible = false;
        }
        else
        {
            oHtmlTableCellParent.Visible = false;
            tdChkAll.Visible = true;
        }
    }

 #endregion
}
