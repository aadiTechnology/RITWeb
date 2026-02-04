using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using BookEntities;
using BusinessLogic;
using Utility;

public partial class BookBarcodeUI : SchoolBase
{
    #region " Constant "

    private const string S_SORT_ORDER = Constants.S_ASCENDING;
    private const string S_DEFAULT_EXPRESSION = "Book_Title";

    #endregion

    private List<BookDetails> mlstBooks;	
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
                FillCategoryCombo();
                FillStandardCombo();
                SetClientScriptAttributes();
            }
            lblErrorMsg.Visible = false;
            lblUpdateSucess.Visible = false;
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for filling catagory combo for all media type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillCategoryCombo();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used for filling catagory combo for printable media type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPrintable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillCategoryCombo();            
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used for filling catagory combo for non printable media type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optNonPrintable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillCategoryCombo();            
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            ViewState[S_SORT_ORDER] = Constants.S_ASCENDING;
            hidBookSortExpression.Value = S_DEFAULT_EXPRESSION;
            FillBooksGrid();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to open the print barcode popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBookMaster_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (mlstBooks.Count > Constants.I_ZERO)
                Session.Add("BookDetailsList", (List<BookDetails>)lstvwBookMaster.DataSource);
			string sEncryptQueryString = null;
			if (QueryString["Is_Configured"] != null)				
                sEncryptQueryString = CommonUtility.EncryptQuerystring(QueryString.AllKeys[0] + "=" + QueryString[0]);
			btnGenerateBarcode.Attributes.Add("onclick", "window.open('BookBarcodePopup.aspx?" +sEncryptQueryString
                                                             + " ' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=680,height=500'); return false;");
            HtmlTableRow oHtmlTableBookHeaderRow = lstvwBookMaster.FindControl("trHeader") as HtmlTableRow;
            if (oHtmlTableBookHeaderRow != null)
                CommonUtility.AddSortImage(oHtmlTableBookHeaderRow, hidBookSortExpression.Value, ((string)ViewState[S_SORT_ORDER] == null ? "asc" : (string)ViewState[S_SORT_ORDER]));
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear book search filter.
    /// </summary>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCotrols();
            InitialiseBookMembers();
            FillBooksGrid();
            if (sender == btnClear)
            {
                HtmlTableRow oHtmlTableBookHeaderRow = lstvwBookMaster.FindControl("trHeader") as HtmlTableRow;
                if (oHtmlTableBookHeaderRow != null)
                {
                    ViewState[S_SORT_ORDER] = Constants.S_ASCENDING;
                    CommonUtility.AddSortImage(oHtmlTableBookHeaderRow, S_DEFAULT_EXPRESSION, S_SORT_ORDER);
                }
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            if ((string)ViewState[S_SORT_ORDER] == Constants.S_ASCENDING)
                ViewState[S_SORT_ORDER] = Constants.S_DESCENDING;
            else
                ViewState[S_SORT_ORDER] = Constants.S_ASCENDING;
            hidBookSortExpression.Value = e.SortExpression;
            FillBooksGrid(e.SortExpression + " " + ViewState[S_SORT_ORDER]);
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Private Methods "

    /// <summary>
    /// This method is used to fill the standard combo box.
    /// </summary>
    private void FillStandardCombo()
    {	
        cmbStandard.Items.Add(new ListItem(Constants.S_SELECT_ALL, Constants.S_ZERO));
        List<ClassDetails> oLstClassDetails = BookBL.GetStandards(miSchoolId, miAcademicYearId);
        oLstClassDetails.ForEach(ClassNames => cmbStandard.Items.Add(new ListItem(ClassNames.Classname, ClassNames.StandardDivisionId.ToString())));
    }

    /// <summary>
    /// This method is used to fill the BooksListView.
    /// </summary>
    private void FillBooksGrid()
    {
        int bkno;
        int iToCount = Constants.I_ZERO;
        int iFromCount = Constants.I_ZERO;
        pnlBooks.Visible = true;
        tblBookCount.Visible = true;
        mlstBooks = new List<BookDetails>();

        List<BookDetails> oLstAllBooks = BookBL.GetBookDetails(miSchoolId, txtBookName.Text, GetMediaType(), Convert.ToInt32(cmbMainCategory.SelectedValue), txtAuthorName.Text, txtPublisher.Text, txtDescription.Text, txtAccessionNumber.Text, Convert.ToInt32(cmbStandard.SelectedValue == string.Empty ? Constants.S_ZERO : cmbStandard.SelectedValue), string.Empty,txtAccessionFromNumber.Text,txtAccessionTo.Text,txtPrefix.Text);        
        var intLstBooks = oLstAllBooks.AsParallel<BookDetails>().AsOrdered<BookDetails>().Where<BookDetails>(book => int.TryParse(book.Book_No, out bkno)).OrderBy(book => Convert.ToInt32(book.Book_No)).Select(book => book);
        var strLstBooks = oLstAllBooks.AsParallel<BookDetails>().AsOrdered<BookDetails>().Where<BookDetails>(book => !int.TryParse(book.Book_No, out bkno)).OrderBy(book => book.Book_No).Select(book => book);
        var lstBooks = intLstBooks.Union(strLstBooks).ToList();
        
        if (!string.IsNullOrEmpty(txtDisplayFrom.Text.Trim()) && !string.IsNullOrEmpty(txtDisplayTo.Text.Trim()))
        {
            iToCount = Convert.ToInt32(txtDisplayTo.Text) - 1;
            iFromCount = Convert.ToInt32(txtDisplayFrom.Text) - 1;
            mlstBooks = lstBooks.Except(lstBooks.Take(iFromCount)).Take(iToCount - (iFromCount - 1)).ToList();
        }
        else if (!string.IsNullOrEmpty(txtDisplayFrom.Text.Trim()))
        {
            iFromCount = Convert.ToInt32(txtDisplayFrom.Text) - 1;
            mlstBooks = lstBooks.Except(lstBooks.Take(iFromCount)).ToList();
        }
        else if (!string.IsNullOrEmpty(txtDisplayTo.Text.Trim()))
        {
            iToCount = Convert.ToInt32(txtDisplayTo.Text);
            mlstBooks = lstBooks.Take(iToCount).ToList();
        }
        else
            mlstBooks = lstBooks;

        lstvwBookMaster.DataSource = mlstBooks.OrderBy(s => s.Book_No).ToList();
        BindDataToList(lstBooks);
        lstvwBookMaster.DataBind();
        SetControlVisibility();
    }

    /// <summary>
    /// This method is used to fill the BooksListView with sorting.
    /// </summary>
    private void FillBooksGrid(string sSortExpression)
    {
        pnlBooks.Visible = true;
        tblBookCount.Visible = true;

        List<BookDetails> oLstBooks = BookBL.GetBookDetails(miSchoolId, txtBookName.Text, GetMediaType(), Convert.ToInt32(cmbMainCategory.SelectedValue), txtAuthorName.Text, txtPublisher.Text, txtDescription.Text, txtAccessionNumber.Text, Convert.ToInt32(cmbStandard.SelectedValue == string.Empty ? Constants.S_ZERO : cmbStandard.SelectedValue), sSortExpression, txtAccessionFromNumber.Text, txtAccessionTo.Text, txtPrefix.Text);
        BindDataToList(oLstBooks);
        lstvwBookMaster.DataBind();
        SetControlVisibility();
    }

    /// <summary>
    /// This method is used to return media type.
    /// </summary>
    /// <returns></returns>
    private int GetMediaType()
    {
        int iReturnValue = Constants.I_TWO;
        if (optNonPrintable.Checked)
            iReturnValue = Constants.I_ZERO;
        else if (optPrintable.Checked)
            iReturnValue = Constants.I_ONE;
        return iReturnValue;
    }

    /// <summary>
    /// 
    /// This method is used to initialized Session variable.
    /// </summary>
    /// <returns></returns>
    private BookBL InitialiseBookMembers()
    {
        BookBL oBookBL = new BookBL();
        oBookBL.SchoolId = miSchoolId;
        oBookBL.UpdatedById = miUserId;
        oBookBL.InsertedById = miUserId;
        oBookBL.UpdatedDate = DateTime.Today;
        
        if (optNonPrintable.Checked)
            oBookBL.MediaType = Constants.I_ZERO;
        else if (optPrintable.Checked)
            oBookBL.MediaType = Constants.I_ONE;
        else
            oBookBL.MediaType = Constants.I_TWO;

        hidMediaType.Value = Convert.ToString(oBookBL.MediaType);
        return oBookBL;
    }

    /// <summary>
    /// This method is used to fill main category combo box.
    /// </summary>
    private void FillCategoryCombo()
    {
        DataTable oDTBook = GetMainCategoryDetails();
        ControlUtility.FillDropDownList(oDTBook, ref cmbMainCategory, "Category_Id", "Category_Name", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used for getting Category Details
    /// </summary>
    /// <returns></returns>
    private DataTable GetMainCategoryDetails()
    {
        BookBL oBookBL = InitialiseBookMembers();
        DataTable oDTBook = oBookBL.GetMainCategoryDetails();
        return oDTBook;
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        valsumBarcode.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Library_Related));
		ApplyMouseHoverEffect(new List<Button>(){ btnBack, btnSearch, btnClear, btnGenerateBarcode });
		SetDefaultButton(btnSearch);
    }

    /// <summary>
    /// This method is used to bind the particular no of records to listview.
    /// </summary>
    /// <param name="lstBooks"></param>
    private void BindDataToList(List<BookDetails> aLstBooks)
    {
        int iToCount = 0;
        int iFromCount = 0;
        if (!string.IsNullOrEmpty(txtDisplayFrom.Text.Trim()) && !string.IsNullOrEmpty(txtDisplayTo.Text.Trim()))
        {
            iToCount = Convert.ToInt32(txtDisplayTo.Text) - 1;
            iFromCount = Convert.ToInt32(txtDisplayFrom.Text) - 1;
            lstvwBookMaster.DataSource = aLstBooks.Except(aLstBooks.Take(iFromCount)).Take(iToCount - (iFromCount - 1)).ToList();
        }
        else if (!string.IsNullOrEmpty(txtDisplayFrom.Text.Trim()))
        {
            iFromCount = Convert.ToInt32(txtDisplayFrom.Text) - 1;
            lstvwBookMaster.DataSource = aLstBooks.Except(aLstBooks.Take(iFromCount));
        }
        else if (!string.IsNullOrEmpty(txtDisplayTo.Text.Trim()))
        {
            iToCount = Convert.ToInt32(txtDisplayTo.Text);
            lstvwBookMaster.DataSource = aLstBooks.Take(iToCount).ToList();
        }
        else
            lstvwBookMaster.DataSource = aLstBooks;
    }

    /// <summary>
    /// This method is used to set controls visibility.
    /// </summary>
    private void SetControlVisibility()
    {
        if (lstvwBookMaster.Items.Count > Constants.I_ZERO)
        {
            btnGenerateBarcode.Enabled = true;            
            lblBookCount.Text = lstvwBookMaster.Items.Count.ToString();
        }
        else
        {
            btnGenerateBarcode.Enabled = false;            
            lblBookCount.Text = Constants.S_ZERO;
        }
    }

    /// <summary>
    /// This method is used to clear the controls.
    /// </summary>
    private void ClearCotrols()
    {
        txtDisplayFrom.Text = string.Empty;
        txtDisplayTo.Text = string.Empty;
        txtBookName.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtPublisher.Text = string.Empty;
        txtAuthorName.Text = string.Empty;
        txtAccessionNumber.Text = string.Empty;
        txtPrefix.Text = string.Empty;
        txtAccessionFromNumber.Text = string.Empty;
        txtAccessionTo.Text = string.Empty;
        optPrintable.Checked = false;
        optNonPrintable.Checked = false;
        optAll.Checked = true;
        cmbMainCategory.SelectedValue = Constants.S_ZERO;
        FillCategoryCombo();
        cmbStandard.Items.Clear();
        FillStandardCombo();
    }

    #endregion " Private Methods "
}
