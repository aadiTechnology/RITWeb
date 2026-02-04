// File Name  : BookUI.aspx.cs
// Created By : Ashish
// Date       : 05/09/2008
// Description: This class is used to Add,Edit,delete Book information.
// Modified BY: Rohini
// Description: Category Parent staff is added.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Globalization;

/// <summary>
/// This class is used to add a new book or edit book details.
/// </summary>
public partial class BookUI : SchoolBase
{
    #region " Constants "

    private const int I_TEST_NAME_COLUMN_INDEX = 0;
    private const string S_DELETE_ROW = "DELETE_ROW";
    private const string S_EDIT_ROW = "EDIT_ROW";
    private const string S_BOOK_ID = "Book_Id";
    private const string S_BOOK_NUMBER = "Book_No";
    private const string S_BOOK_DETAIL_ID = "Book_Detail_Id";
    private const string S_BOOK_ISSUE_STATUS = "Book_Issue_Status";    
    private const string S_INT_DATA_TYPE = "System.Int32";
    private const string S_IDECIMAL_DATA_TYPE = "System.Decimal";
    private const string S_STRING_DATA_TYPE = "System.String";
    private const string S_ACCESSION_DETAILS = "grdBookNo_DataSource";
    private const string S_EDIT_IMAGE_URL = "~/RITeSchool/images/IconGrid_EditDis.gif";
    private const string S_ONE_ACCESSION = "At least one Accession Details should be added.";
    private const string S_DUPLICATE_BOOK_NO = "Accession number already exists.";
    private const string S_ADD_MODE = "Add";
    private const string S_EDIT_MODE = "Edit";
    private const string S_YES = "Yes";
    private const string S_FOR_PARENT_STAFF = "For Parent / Staff";
    private const string S_HUNDRED = "100";
    private const string S_BOOK_ISSUED = "This book already issued.";
    public const string S_DEFAULT_DATE_2 = "01/01/1900 12:00:00 AM"; /////
    #endregion

    #region " Event "

    /// <summary>
    /// This event is used to fill main and sub category combo Box, set default control, encrypt query string.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (CheckPreCondition())
            {
                if (!IsPostBack)
                {
                    FillAllCombos();
                    SetDefaultControl();
                    AssignControlForAddEditMode();
                    SetAccessionDetailsAttributes();
                    wizard_BookDetails.ActiveStepChanged +=
                                      new EventHandler(wizard_BookDetails_ActiveStepChanged);
                }
                valsumBookNo.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
               // valsumBooks.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;//////////
                ShowControl();
                chkIsGifted.Attributes.Add("onclick", "ClearErrorMsg();");
               
             }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set next button click and also check duplication of Book Title.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    
    protected void wizard_BookDetails_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            HideAccessionDetailsControl();
            BookBL oBookBL = new BookBL
                                 {
                                     BookId = Convert.ToInt32(hidBookId.Value),
                                     BookName = txtBookName.Text,
                                     AuthorName = txtAuthorName.Text
                                 };
            if (optPrintable.Checked)
                oBookBL.MediaType = 1;
            else if (optNonPrintable.Checked)
                oBookBL.MediaType = 0;
            oBookBL.MainCategoryId = Convert.ToInt32(cmbMainCategory.SelectedItem.Value);
           // oBookBL.ISBN = txtISBN.Text;
            oBookBL.IsDuplicateBook();
        }
        catch (BusinessLogic.Exceptions.DuplicateEntityException ex)
        {
            ShowBookNoError(true, ex.ErrorMessage);
            wizard_BookDetails.ActiveStepIndex = 0;            
            e.Cancel = true;            
            txtBookName.Focus();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to save book details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_BookDetails_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
           
            if (grdBookNo.Rows.Count > Constants.I_ZERO)
            {
                if (hidIsNewBook.Value == "true" && hidIsAddQuantity.Value == "false")
                    AddNewBooks();
                else if (hidIsNewBook.Value == "true" && hidIsAddQuantity.Value == "true")
                    AddAccessionDetails();
                else if (hidIsNewBook.Value == "false" && hidIsAddQuantity.Value == "false")
                    UpdateExistingBooks();
            }
            else
                ShowBookNoError(true, S_ONE_ACCESSION);
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void wizard_BookDetails_StepPreviousButton(object sender, WizardNavigationEventArgs e)
    {
        
    }
    /// <summary>
    /// This event is used to cancel wizard control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_BookDetails_CancelButtonClick(object sender, EventArgs e)
    {
        try
        {            
            if (wizard_BookDetails.ActiveStep == WizardStep1)
            {
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage(Constants.S_PAGE_LIBRARY_MANAGEMENT);
            }
            else if (wizard_BookDetails.ActiveStep == WizardStep2)
            {
                EmptyTextBoxes();
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage(Constants.S_PAGE_LIBRARY_MANAGEMENT);
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set Default control on Wizard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_BookDetails_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            if (wizard_BookDetails.ActiveStep == WizardStep1)
                SetAccessionDetailsAttributes();
            if (wizard_BookDetails.ActiveStep == WizardStep2)
                SetAccessionNumbersDetails();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for Adding Book Number to Grid View.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidIsNewBookNo.Value == "true" && hidModeType.Value != S_EDIT_MODE)
            {
                IsDuplicateBookNo();
                AddAccessionDetailsToGrid();
                EmptyTextBoxes();
            }
            else
            {
                IsDuplicateBookNo();
                int iRowIndex = Convert.ToInt32(hidIndexNo.Value);
                SaveEditBookNo(iRowIndex);
                SetAddQuantityControl();
            }
            txtBookNo.Focus();
        }
        catch (BusinessLogic.Exceptions.DuplicateEntityException ex)
        {
            ShowBookNoError(true, ex.ErrorMessage);
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for checking Media Type like Printable or non-printable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPrintable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillMainCategoryCombo();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to redirect wizard step to the previews step.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_BookDetails_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            if (wizard_BookDetails.ActiveStep.Name.Equals("Step 2"))
            {
                if (hidModeType.Value == S_ADD_MODE)
                    wizard_BookDetails.ActiveStepIndex = 0;
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Grid View Event "

    /// <summary>
    /// This event sets properties to grid's column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdBookNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            const int I_DELETE_COLUMN_INDEX = 8;
            const int I_EDIT_COLUMN_INDEX = 7;
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                ImageButton oImgDelete = (ImageButton)e.Row.Cells[I_DELETE_COLUMN_INDEX].Controls[Constants.I_ZERO];
                oImgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                ImageButton oImgEdit = (ImageButton)e.Row.Cells[I_EDIT_COLUMN_INDEX].Controls[Constants.I_ZERO];
                string sBookIssueStatus = grdBookNo.DataKeys[e.Row.RowIndex]["Book_Issue_Status"].ToString();
                string sPurchaseDate = Convert.ToString(grdBookNo.DataKeys[e.Row.RowIndex]["PurchaseDate"]);
                if (sPurchaseDate != string.Empty)
                {
                    DateTime dtPurchaseDate = Convert.ToDateTime(sPurchaseDate);
                    e.Row.Cells[4].Text = dtPurchaseDate.ToString("dd-MMM-yyyy"); 
                }
                if (sBookIssueStatus.Equals(Constants.S_YES))
                {
                    btnAdd.Visible = false;
                    oImgEdit.ImageUrl = S_EDIT_IMAGE_URL;
                    oImgEdit.Attributes.Add("onclick", "if(!NoAction()) {return false;}");
                    oImgEdit.ToolTip = S_BOOK_ISSUED;
                }
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set purchase date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwAddBookNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                if (grdvwAddBookNo.DataKeys != null)
                {
                    string sPurchaseDate = Convert.ToString(grdvwAddBookNo.DataKeys[e.Row.RowIndex]["PurchaseDate"]);
                    if (sPurchaseDate != string.Empty)
                    {
                        DateTime dtPurchaseDate = Convert.ToDateTime(sPurchaseDate);
                        e.Row.Cells[4].Text = dtPurchaseDate.ToString("dd-MMM-yyyy");
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
    /// This event is used for Edit or delete Book Number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdBookNo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex;
            bool bResult = int.TryParse(e.CommandArgument.ToString(), out iRowIndex);
            if (bResult)
            {
                hidIndexNo.Value = Convert.ToString(iRowIndex);
                switch (e.CommandName.ToUpper())
                {
                    case S_DELETE_ROW:
                        DeletedBookNoDetails(iRowIndex);
                        HideAccessionDetailsControl();
                        ResetBookNoControls();
                        break;
                    case S_EDIT_ROW:
                        btnAdd.Visible = true;
                        EditBookNoDetails(iRowIndex);
                        txtBookNo.Focus();
                        break;
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
    protected void grdBookNo_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            DataTable oDataTable = (DataTable)ViewState[S_ACCESSION_DETAILS];
            oDataTable.DefaultView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
            grdBookNo.DataSource = oDataTable;
            grdBookNo.DataBind();
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
    protected void grdBookNo_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((GridView)sender);

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
    protected void grdvwAddBookNo_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            int iBookId = Convert.ToInt32(hidBookId.Value);
            BookBL oBookBL = new BookBL();
            DataTable oDtBookNo = oBookBL.GetBookNoDetails(iBookId, miSchoolId);
            oDtBookNo.DefaultView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
            grdvwAddBookNo.DataSource = oDtBookNo;
            grdvwAddBookNo.DataBind();
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
    protected void grdvwAddBookNo_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((GridView)sender);

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

    #endregion

    #region " Private Method "
    /// <summary>
    /// This method is used to clear the controls.
    /// </summary>
    private void EmptyTextBoxes()
    {
        txtBookNo.Text = string.Empty;
        txtBookPrice.Text = string.Empty;
        txtTotPages.Text = string.Empty;
        txtBillNo.Text = string.Empty;
        txtPurchaseDate.Text = string.Empty;
        chkIsGifted.Checked = false;
        cmbVendorName.SelectedIndex = 0;
        
    }

    /// <summary>
    /// This method is used to initialized Session variable.
    /// </summary>
    /// <returns></returns>
    private BookBL InitialiseBookBL()
    {
        BookBL oBookBL = new BookBL
                             {
                                 SchoolId = miSchoolId,
                                 UpdatedById = miUserId,
                                 InsertedById = miUserId,
                                 UpdatedDate = DateTime.Today
                             };
        if (optPrintable.Checked)
            oBookBL.MediaType = 1;
        else if (optNonPrintable.Checked)
            oBookBL.MediaType = 0;
        return oBookBL;
    }   

    /// <summary>
    /// This method is used to set default control.
    /// </summary>
    private void SetDefaultControl()
    {
        hidSortExpression.Value = grdBookNo.Columns[I_TEST_NAME_COLUMN_INDEX].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
        
        hidIsNewBook.Value = "true";
        hidIsNewBookNo.Value = "true";
        hidIsAddQuantity.Value = "false";
        hidBookSrNo.Value = "0";
        hidBookId.Value = "0";
        grdBookNo.Columns[7].Visible = true;
        grdBookNo.Columns[8].Visible = true; 
        txtBookName.Focus();
    }

    /// <summary>
    /// This method is used to Show control at the level of page load.
    /// </summary>
    private void ShowControl()
    {
        if (hidIsAddQuantity.Value == "false")
        {
            HideControl(true);
            tdAddBookGrd.Visible = false;
        }
        lblMessage.Visible = false;
        grdBookNo.Visible = true;
    }
   
    /// <summary>
    /// This method is used to set attributes on book details screen.
    /// </summary>
    private void SetAccessionDetailsAttributes()
    {
        if (wizard_BookDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID") != null)
        {
            Button oButton = (Button)wizard_BookDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            if (oButton != null)
                new Button[] { oButton }.ApplyEffect();
            oButton = (Button)wizard_BookDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton");
            if (oButton != null)
                new Button[] { oButton }.ApplyEffect();
        }
    }

    /// <summary>
    /// This method is used to set attributes on accession number detail screen.
    /// </summary>
    private void SetAccessionNumbersDetails()
    {
        txtBookNo.Focus();
        btnAdd.Attributes.Add("onclick", "if(!ClearErrorMsg()) {return false;}");

        Button oBtnFinish = (Button)wizard_BookDetails.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");

        //wizard_BookDetails_FinishButtonClick(object,new WizardNavigationEventArgs());
        if (oBtnFinish != null)
            new Button[] { oBtnFinish }.ApplyEffect();

        Button oButton = (Button)wizard_BookDetails.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton");
        if (oButton != null)
                new Button[] { oButton }.ApplyEffect();

        oButton = (Button)wizard_BookDetails.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
        if (oButton != null)
        {
            new Button[] { oButton }.ApplyEffect();
            oButton.Attributes.Add("onclick", "if(!ClearErrorMsg()) {return false;}");
        }

        oButton = (Button)wizard_BookDetails.FindControl("btnAdd");
        if (oButton != null)
            new Button[] { oButton }.ApplyEffect();
    }

    /// <summary>
    /// This method is used for the Adding new Books in database.
    /// </summary>
    private void AddNewBooks()
    {
        BookBL oBookBL = new BookBL();
        List<SchoolBookDetails> lstAccessionDetails = GetBookCopyList();
        AddBook(lstAccessionDetails);
        RedirectToMainPage();
    }

    /// <summary>
    /// This method is used for Adding Book Quantity with Book No.
    /// </summary>
    private void AddAccessionDetails()
    {
        List<SchoolBookDetails> lstAccessionDetails = GetBookCopyList();
        BookBL oBookBL = InitialiseBookBL();
        int iBookId = Convert.ToInt32(hidBookId.Value);
        oBookBL.AddAccessionDetails(lstAccessionDetails, iBookId);
        RedirectToMainPage();
    }

    /// <summary>
    /// This method is used for the updating existing Books.
    /// </summary>
    private void UpdateExistingBooks()
    {
        BookBL oBookBL = InitialiseBookBL();
        oBookBL.BookId = Convert.ToInt32(hidBookId.Value);
        UpdateBook();
        RedirectToMainPage();
    }

    /// <summary>
    /// This method is used to check Book Number duplication.
    /// </summary>
    /// <returns></returns>
    private void IsDuplicateBookNo()
    {
        BookBL oBookBL = new BookBL();
        oBookBL.BookSrNo = Convert.ToInt32(hidBookSrNo.Value);
        oBookBL.IsDuplicateBookNo(txtBookNo.Text);
    }

    /// <summary>
    /// This method is used to Add Book Number on grid view.
    /// </summary>
    private void AddAccessionDetailsToGrid()
    {
        DataTable oDtAccessionDetails;
        if (ViewState[S_ACCESSION_DETAILS] == null)
            oDtAccessionDetails = CreateAccessionDetailsTable();
        else
            oDtAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        string sMsg = CheckBookNoIsDuplicate(oDtAccessionDetails);
        if (string.IsNullOrEmpty(sMsg))
        {
            // Once a table has been created,create DataRow. 
            oDtAccessionDetails.Rows.Add(GetAccessionDetails(oDtAccessionDetails.NewRow()));
            oDtAccessionDetails.DefaultView.Sort = "Book_No";
            grdBookNo.DataSource = oDtAccessionDetails.DefaultView;
            ViewState[S_ACCESSION_DETAILS] = oDtAccessionDetails;
            grdBookNo.DataBind();
            btnAdd.Text = S_ADD_MODE;
            HideAccessionDetailsControl();
            lblNewAccessions.Visible = true;
        }
        else
            ShowBookNoError(true, sMsg);
    }

    /// <summary>
    /// This method is used to display Error Message on the form.
    /// </summary>
    private void ShowBookNoError(bool abFlag, string asMsg)
    {
        lblMessage.Visible = abFlag;
        lblMessage.Text = asMsg;
        grdBookNo.Visible = false;
        lblNewAccessions.Visible = false;
    }

    /// <summary>
    /// This method is used for the saving Editing information in the table and store information in veiwstate.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    private void SaveEditBookNo(int aiRowIndex)
    {
        const int I_IS_GIFTED = 0;
        const int I_BILL_NO = 1;
        const int I_VENDORE_ID = 2;
        const int I_PURCHASE_DATE = 3;
        const int I_BOOK_PRICE = 4;
        const int I_TOTAL_PAGES = 5;
        const int I_VENDORE_NAME = 6;
        const int I_BOOK_NO = 7;
        DataTable oDtAccessionDetails;
        oDtAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        string sMsg = CheckBookNoIsDuplicate(oDtAccessionDetails);
        if (string.IsNullOrEmpty(sMsg))
        {
            DataRow oDTRow = oDtAccessionDetails.NewRow();
            oDTRow = oDtAccessionDetails.Rows[aiRowIndex];
            oDTRow.BeginEdit();
            oDTRow[I_BOOK_NO] = (string)txtBookNo.Text;
            oDTRow[I_BOOK_PRICE] = (string)txtBookPrice.Text;
            if (txtTotPages.Text != string.Empty)
                oDTRow[I_TOTAL_PAGES] = Convert.ToInt32(txtTotPages.Text);
            else
                oDTRow[I_TOTAL_PAGES] = DBNull.Value;
            oDTRow[I_PURCHASE_DATE] = string.IsNullOrEmpty(txtPurchaseDate.Text) ? DBNull.Value.ToString() : txtPurchaseDate.Text;
            oDTRow[I_BILL_NO] = (string)txtBillNo.Text;
            if (chkIsGifted.Checked == true)
            {
                oDTRow[I_IS_GIFTED] = S_YES;
            }
            else
                oDTRow[I_IS_GIFTED] = "No";
            if (cmbVendorName.SelectedIndex != Constants.I_ZERO)
                oDTRow[I_VENDORE_NAME] = Convert.ToString(cmbVendorName.SelectedItem);
            else
                oDTRow[I_VENDORE_NAME] = string.Empty;
            oDTRow[I_VENDORE_ID] = Convert.ToString(cmbVendorName.SelectedValue);
            oDtAccessionDetails.AcceptChanges();
            oDtAccessionDetails.Rows[aiRowIndex].EndEdit();
            grdBookNo.DataSource = oDtAccessionDetails;
            grdBookNo.DataBind();
            ViewState[S_ACCESSION_DETAILS] = oDtAccessionDetails;
            if (grdBookNo.Rows.Count != Constants.I_ZERO)
            {
                EmptyTextBoxes();
                txtBookNo.Focus();
            }
        }
        else
            ShowBookNoError(true, sMsg);
    }

    /// <summary>
    /// This method is used to delete Book No. details.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private bool DeletedBookNoDetails(int aiRowIndex)
    {
        DataTable oDTAccessionDetails;
        oDTAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        DataRow oDTRow = oDTAccessionDetails.NewRow();
        oDTRow = oDTAccessionDetails.Rows[aiRowIndex];
        oDTRow.Delete();
        oDTAccessionDetails.AcceptChanges();
        grdBookNo.DataSource = oDTAccessionDetails;
        grdBookNo.DataBind();
        ViewState[S_ACCESSION_DETAILS] = oDTAccessionDetails;  
        return true;
    }

    /// <summary>
    /// This method is used to hide book Book no.
    /// </summary>
    /// <param name="iBookQuantity"></param>
    private void HideAccessionDetailsControl()
    {
        if (grdBookNo.Rows.Count <= Constants.I_ZERO)
        {
            tdBookNo.Visible = false;
            hidIsNewBookNo.Value = "true";
        }
        else
        {
            txtBookNo.Focus();
            tdBookNo.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to reset book no controls.
    /// </summary>
    private void ResetBookNoControls()
    {
        txtBookNo.Focus();
        EmptyTextBoxes();
        btnAdd.Text = S_ADD_MODE;
    }

    /// <summary>
    /// This Method is used for Editing Book Number which is temperory store in the Grid View.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void EditBookNoDetails(int aiRowIndex)
    {
        string sBookNo = Convert.ToString(grdBookNo.DataKeys[aiRowIndex]["Book_No"]);
        hidBookSrNo.Value = Convert.ToString(grdBookNo.DataKeys[aiRowIndex]["Book_Detail_Id"]);
        txtBookNo.Text = sBookNo;
        DataTable oDTAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        string VendorId = grdBookNo.DataKeys[aiRowIndex]["VendorId"].ToString();
        txtBookPrice.Text = Convert.ToString(oDTAccessionDetails.Rows[aiRowIndex]["Book_Price"]);
        txtTotPages.Text = Convert.ToString(oDTAccessionDetails.Rows[aiRowIndex]["TotalPages"]);
        txtBillNo.Text = Convert.ToString(oDTAccessionDetails.Rows[aiRowIndex]["BillNo"]);
        string sDate = Convert.ToString(oDTAccessionDetails.Rows[aiRowIndex]["PurchaseDate"]);
        if (sDate != string.Empty)
        {
            DateTime dtPurchasedate = Convert.ToDateTime(sDate);
            hidPurchaseDate.Value = dtPurchasedate.ToString("dd-MMM-yyyy");
        }
        else hidPurchaseDate.Value = string.Empty;
        txtPurchaseDate.Text = hidPurchaseDate.Value;
        cmbVendorName.SelectedValue = VendorId;
        string sIsGifted = Convert.ToString(oDTAccessionDetails.Rows[aiRowIndex]["IsGifted"]);
        if (sIsGifted == S_YES)
        {
            chkIsGifted.Checked = true;
        }
        else
            chkIsGifted.Checked = false;
        txtISBN.Text = Convert.ToString(oDTAccessionDetails.Rows[aiRowIndex]["ISBN"]);
        btnAdd.Text = "Update";
        hidIsNewBookNo.Value = "false";
        grdBookNo.Columns[7].Visible = true;
        txtBookNo.ReadOnly = false;
        chkIsGifted.Enabled = true;
        txtBookPrice.ReadOnly = false;
        txtTotPages.ReadOnly = false;
        cPurchageDate.Enabled = true;
        txtPurchaseDate.ReadOnly = false;
        txtBillNo.ReadOnly = false;
        cmbVendorName.Enabled = true;
    }

    /// <summary>
    /// This method is used for getting Category Details
    /// </summary>
    /// <returns></returns>
    private DataTable GetMainCategoryDetails()
    {
        BookBL oBookBL = InitialiseBookBL();
        DataTable oDtBook = oBookBL.GetMainCategoryDetails();
        return oDtBook;
    }

    /// <summary>
    /// This method is used to get the details of book copy to add. 
    /// </summary>
    /// <returns></returns>
    private List<SchoolBookDetails> GetBookCopyList()
    {
        List<SchoolBookDetails> oLstAccessionDetails = new List<SchoolBookDetails>();
        DataTable oDTAccessionDetails;
        oDTAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        ArrayList oArrList = new ArrayList();
        foreach (DataRow oAccessionDetailsDataRow in oDTAccessionDetails.Rows)
        {
            SchoolBookDetails oSchoolBookDetails = new SchoolBookDetails
                                                       {
                                                           Book_No = Convert.ToString(oAccessionDetailsDataRow[S_BOOK_NUMBER]),
                                                           VendorId = Convert.ToInt32(oAccessionDetailsDataRow["VendorId"]),
                                                           Book_Price = Convert.ToDecimal(oAccessionDetailsDataRow["Book_Price"])
                                                       };
            if (oAccessionDetailsDataRow["TotalPages"].ToString() != string.Empty && oAccessionDetailsDataRow["TotalPages"] != DBNull.Value)
                oSchoolBookDetails.TotalPages = Convert.ToInt32(oAccessionDetailsDataRow["TotalPages"]);
            else
                oSchoolBookDetails.TotalPages = Constants.I_ZERO;
            if (Convert.ToString(oAccessionDetailsDataRow["IsGifted"]) == S_YES)
                oSchoolBookDetails.IsGifted = Constants.I_ONE;
            else
                oSchoolBookDetails.IsGifted = Constants.I_ZERO;
            if (oAccessionDetailsDataRow["PurchaseDate"].ToString() != string.Empty)
                oSchoolBookDetails.DateOfPurchage = Convert.ToDateTime(oAccessionDetailsDataRow["PurchaseDate"]);
            oSchoolBookDetails.ISBN = Convert.ToString(oAccessionDetailsDataRow["ISBN"]);
            oSchoolBookDetails.BillNo = Convert.ToString(oAccessionDetailsDataRow["BillNo"]);
            oLstAccessionDetails.Add(oSchoolBookDetails);
        }
        return oLstAccessionDetails;
    }

    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow GetAccessionDetails(DataRow aoDataRow)
    {
        int i = Convert.ToInt32(grdBookNo.Rows.Count);
        DataRow oDrItem = aoDataRow;
        oDrItem[S_BOOK_NUMBER] = Convert.ToString(txtBookNo.Text);
        if (txtBookPrice.Text == ".")
            txtBookPrice.Text = "0.0";
        oDrItem["Book_Price"] = string.Format("{0:0.00}", Convert.ToDecimal(txtBookPrice.Text));
        if (txtTotPages.Text != string.Empty)
            oDrItem["TotalPages"] = Convert.ToInt32(txtTotPages.Text);
        else
            oDrItem["TotalPages"] = string.Empty;
        if (chkIsGifted.Checked == true)
            oDrItem["IsGifted"] = S_YES;
        else
            oDrItem["IsGifted"] = "No";
        oDrItem["BillNo"] = Convert.ToString(txtBillNo.Text);
        if (cmbVendorName.SelectedIndex != Constants.I_ZERO)
            oDrItem["VendorName"] = Convert.ToString(cmbVendorName.SelectedItem);
        else
            oDrItem["VendorName"] = string.Empty;
        oDrItem["VendorId"] = Convert.ToInt32(cmbVendorName.SelectedValue);
        oDrItem["PurchaseDate"] = Convert.ToString(txtPurchaseDate.Text);
       // oDrItem["ISBN"] = Convert.ToString(txtISBN.Text);
        oDrItem[S_BOOK_DETAIL_ID] = Convert.ToString(i + 1);
        return oDrItem;
    }

    /// <summary>
    /// This method is used to creat Book object which is used for further inserting or updating operation.
    /// </summary>
    /// <returns></returns>
    private BookBL PopulateBookBLObjects()
    {
        BookBL oBookBL = InitialiseBookBL();
        oBookBL.BookName = txtBookName.Text;
        oBookBL.AuthorName = txtAuthorName.Text;        
        oBookBL.MainCategoryName = cmbMainCategory.SelectedItem.Text;
        oBookBL.MainCategoryId = Convert.ToInt32(cmbMainCategory.SelectedItem.Value);
        oBookBL.PublishedBy = txtPublisherName.Text;
        oBookBL.Description = txtDescription.Text;
        oBookBL.Remark = txtRemark.Text;
        oBookBL.RackNumber = txtRackNumber.Text;
        oBookBL.ShelfNumber = txtShelf.Text;
        oBookBL.UserId = miUserId;
        oBookBL.AcademicYearId = miAcademicYearId;
        oBookBL.UpdatedDate = DateTime.Now;
        oBookBL.Classification = txtClassification.Text;
        oBookBL.LostPercentage = Convert.ToDecimal(txtLostPercentage.Text);
        oBookBL.Language = txtLanguage.Text;
        oBookBL.ISBN = txtISBN.Text;
        oBookBL.IsForIssue = Convert.ToInt16(chkForReadong.Checked);
        oBookBL.CallNumber = (txtcallnumber.Text);/////////
        oBookBL.Series = (txtseries.Text);/////////
        oBookBL.Status = txtstatus.Text;////////////
        //oBookBL.PublicationDate = Convert.ToDateTime(txtpublicationdate.Text);///////////////
       // oBookBL.PublicationDate =txtpublicationdate.Text.ToDateTime();///////////////line updated
        if (txtpublicationdate.Text == string.Empty)//
        {
            txtpublicationdate.Text = S_DEFAULT_DATE_2;
        }
        oBookBL.PublicationDate = txtpublicationdate.Text.ToDateTime();//
        if (txtEdition.Text != string.Empty)
            oBookBL.BookEdition = txtEdition.Text;
        else
            oBookBL.BookEdition = string.Empty;
        if (txtBookYear.Text != string.Empty)
            oBookBL.BookYear = txtBookYear.Text;
        else
            oBookBL.BookYear = string.Empty;

       

        oBookBL.SelectedClasses = GetSelectedStandards();
        return oBookBL;
    }

    /// <summary>
    /// This method is used to get the saved standards for books.
    /// </summary>
    /// <returns></returns>
    private List<int> GetSelectedStandards()
    {
        int iTotalStandards = chkListClasses.Items.Count;
        List<int> oLstSelectedStandards = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalStandards; iListIndex++)
        {
            if (chkListClasses.Items[iListIndex].Selected == true)
                oLstSelectedStandards.Add(Convert.ToInt32(chkListClasses.Items[iListIndex].Value));
        }
        return oLstSelectedStandards;
    }

    /// <summary>
    /// This mothod is used for Updating Books
    /// </summary>
    private void UpdateBook()
    {
        ArrayList oArrayList = GetUpdateStatement();
        BookBL oBookBL = PopulateBookBLObjects();
        int iBookId = Convert.ToInt32(hidBookId.Value);
        oBookBL.UpdateBook(iBookId, oArrayList);
    }

    /// <summary>
    /// This method is used to get Update statement for Updating Book No.
    /// </summary>
    /// <returns></returns>
    private ArrayList GetUpdateStatement()
    {
        int iIsGifted;
        DataTable oDTAccessionDetails;
        oDTAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        ArrayList oArrUpdateStatement = new ArrayList();
        foreach (DataRow oAccessionDetailsDataRow in oDTAccessionDetails.Rows)
        {
            string sBookNo = Convert.ToString(oAccessionDetailsDataRow[S_BOOK_NUMBER]);
            int iBookSrNo = Convert.ToInt32(oAccessionDetailsDataRow[S_BOOK_DETAIL_ID]);
            string sIsGifted = Convert.ToString(oAccessionDetailsDataRow["IsGifted"]);
            iIsGifted = sIsGifted == S_YES ? Constants.I_ONE : Constants.I_ZERO;
            int iBookId = Convert.ToInt32(oAccessionDetailsDataRow[S_BOOK_ID]);
            string sDate = Convert.ToString(oAccessionDetailsDataRow["PurchaseDate"]);
            string sBillNo = Convert.ToString(oAccessionDetailsDataRow["BillNo"]);

            BookBL oBookBL = InitialiseBookBL();
            if (oAccessionDetailsDataRow["TotalPages"] == DBNull.Value)
                oAccessionDetailsDataRow["TotalPages"] = Constants.I_ZERO;
            string sUpdateStatement = oBookBL.GetUpdateStatementForBookNo(iBookSrNo, sBookNo, iBookId, iIsGifted, 
                                                                            Convert.ToDecimal(oAccessionDetailsDataRow["Book_Price"]), 
                                                                            Convert.ToInt32(oAccessionDetailsDataRow["TotalPages"]),
                                                                            sBillNo, Convert.ToInt32(oAccessionDetailsDataRow["VendorId"]), sDate);
            oArrUpdateStatement.Add(sUpdateStatement);
        }
        return oArrUpdateStatement;
    }

    /// <summary>
    /// This method is used to create new datatable for Book Details.
    /// </summary>
    private DataTable CreateAccessionDetailsTable()
    {
        DataTable oDtBookDetails = new DataTable();
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, "IsGifted", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, "BillNo", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, "VendorId", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, "PurchaseDate", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_IDECIMAL_DATA_TYPE, "Book_Price", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, "TotalPages", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, "VendorName", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, "ISBN", ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_BOOK_NUMBER, ref oDtBookDetails, true);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_BOOK_DETAIL_ID, ref oDtBookDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_BOOK_ISSUE_STATUS, ref oDtBookDetails, false);


        return oDtBookDetails;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemTable(string asDataType, string asColumnName, ref DataTable aoDataTable, bool abIsPrimaryKey)
    {
        DataColumn oDataColumn = new DataColumn();
        oDataColumn.DataType = Type.GetType(asDataType);
        oDataColumn.ColumnName = asColumnName;
        aoDataTable.Columns.Add(oDataColumn);

        if (abIsPrimaryKey)
        {
            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = oDataColumn;
            aoDataTable.PrimaryKey = keys;
        }
    }

    /// <summary>
    /// This method is used for Add Book in database.
    /// </summary>
    /// <param name="oArrayList"></param>
    private void AddBook(List<SchoolBookDetails> aoLstAccessionDetails)
    {
        BookBL oBookBL = PopulateBookBLObjects();
        oBookBL.AddBook(aoLstAccessionDetails);
    }

    /// <summary>
    /// This method is used for the Checking duplication for Book No. 
    /// </summary>
    /// <param name="oDTAccessionDetails"></param>
    /// <returns></returns>
    private string CheckBookNoIsDuplicate(DataTable aoDtAccessionDetails)
    {
        string sMsg = string.Empty;
        if (aoDtAccessionDetails.Rows.Count > Constants.I_ZERO)
        {            
            ArrayList oArrayList = GetAccessionNos();
            string sBookNo = Convert.ToString(txtBookNo.Text.Trim().ToLower());            
           
            for (int iCount = 0; iCount < oArrayList.Count; iCount++)
            {
                string sArrBookNo = Convert.ToString(oArrayList[iCount]).Trim().ToLower();
                if (hidIsNewBookNo.Value == "true")
                {
                    if (sBookNo == sArrBookNo)
                    {
                        sMsg = S_DUPLICATE_BOOK_NO;
                        break;
                    }
                }
                else if (iCount != Convert.ToInt32(hidIndexNo.Value))
                {                    
                    if (sBookNo == sArrBookNo)
                    {
                        sMsg = S_DUPLICATE_BOOK_NO;
                        break;
                    }
                }
            }
        }
        return sMsg;
    }

    /// <summary>
    /// This method is used  for keeping DataTable value in ArrayList.
    /// </summary>
    /// <returns></returns>
    private ArrayList GetAccessionNos()
    {
        DataTable oDTAccessionDetails;
        oDTAccessionDetails = (DataTable)ViewState[S_ACCESSION_DETAILS];
        ArrayList oArrList = new ArrayList();
        foreach (DataRow oBookDataRow in oDTAccessionDetails.Rows)
        {
            string sBookNo = Convert.ToString(oBookDataRow[S_BOOK_NUMBER]);
            oArrList.Add(sBookNo);
        }
        return oArrList;
    }
    
    /// <summary>
    /// This method is used for editing Book Information.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void FillBookInfoForEdit()
    {
        int iBookId = Convert.ToInt32(hidBookId.Value);
        hidBookId.Value = Convert.ToString(iBookId);
        
        BookBL oBookBL = new BookBL(iBookId, miSchoolId);
        AssignBookDetails(oBookBL);
        if (hidIsAddQuantity.Value == "false")
        {            
            hidIsNewBook.Value = "false";
            hidIsNewBookNo.Value = "false";
        }
        FillAccessionDetailsGrid(iBookId, miSchoolId);
    }

    /// <summary>
    /// This method is used for Assinging Book Details to the controls
    /// </summary>
    /// <param name="oBookBL"></param>
    /// <returns></returns>
    private void AssignBookDetails(BookBL oBookBL)
    {
        txtBookName.Text = oBookBL.BookName;
        if (oBookBL.MediaType == Constants.I_ZERO)
        {
            optNonPrintable.Checked = true;
            optPrintable.Checked = false;
        }
        else if (oBookBL.MediaType == Constants.I_ONE)
        {
            optPrintable.Checked = true;
            optNonPrintable.Checked = false;
        }
        cmbMainCategory.SelectedValue = Convert.ToString(oBookBL.MainCategoryId);
        int iMainCategory = Convert.ToInt32(cmbMainCategory.SelectedIndex.ToString());
        cmbMainCategory.SelectedIndex = iMainCategory;
        txtAuthorName.Text = oBookBL.AuthorName;   
        txtPublisherName.Text = oBookBL.PublishedBy;
        hidBookId.Value = Convert.ToString(oBookBL.BookId);
        txtDescription.Text = Convert.ToString(oBookBL.Description);
        txtRemark.Text = oBookBL.Remark;
        txtRackNumber.Text = oBookBL.RackNumber;
        txtShelf.Text = oBookBL.ShelfNumber;
        txtLanguage.Text = oBookBL.Language;
        txtISBN.Text = oBookBL.ISBN;
        txtClassification.Text = oBookBL.Classification;
        txtEdition.Text = oBookBL.BookEdition;
        txtBookYear.Text = oBookBL.BookYear;
        //txtcallnumber.Text =(oBookBL.CallNumber).ToString();/////////////
      //  txtseries.Text =(oBookBL.Series).ToString();//////////////
      //  txtstatus.Text = oBookBL.Status;////////////////
        txtcallnumber.Text = Convert.ToString(oBookBL.CallNumber);//
        txtseries.Text = Convert.ToString(oBookBL.Series);  //
        txtstatus.Text = oBookBL.Status; //
        txtBookNo.Text = oBookBL.BookNumber;   // new added
        txtpublicationdate.Text = Convert.ToDateTime(oBookBL.PublicationDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));/////////////////
       
        chkForReadong.Checked = oBookBL.IsForIssue == Constants.I_ONE;
        
        txtLostPercentage.Text = Convert.ToString(oBookBL.LostPercentage);
        List<ClassDetails> oLstStandards = oBookBL.GetSavedStandards(miSchoolId, Convert.ToInt32(hidBookId.Value), miAcademicYearId);

        for (int i = 0; i < chkListClasses.Items.Count; i++)
        {
            List<ClassDetails> oLstFilteredStandards = oLstStandards.Where(std => std.StandardDivisionId == Convert.ToInt32(chkListClasses.Items[i].Value)).ToList();

            if (oLstFilteredStandards.Count == 1)            
                chkListClasses.Items[i].Selected = true;
        }       
    }

    /// <summary>
    /// This method is used for Setting default control at the time of editing
    /// </summary>
    private void SetEditModeControls()
    {

        btnAdd.Visible = false;
        grdBookNo.Columns[8].Visible = false;
        grdBookNo.Columns[7].Visible = true;
        EmptyTextBoxes();
        txtBookNo.ReadOnly = true;
        txtBookPrice.ReadOnly = true;
        txtTotPages.ReadOnly = true;
        cPurchageDate.Enabled = false;
        chkIsGifted.Enabled = false;
        txtPurchaseDate.ReadOnly = true;
        txtBillNo.ReadOnly = true;
        cmbVendorName.Enabled = false;
        lblNewAccessions.Visible = false;
    }

    /// <summary>
    /// This method is used to hide control when we Add Book Quantity.
    /// </summary>
    private void HideControl(bool abFlag)
    {
        grdBookNo.Visible = abFlag;
        tdBookNo.Visible = abFlag;
        txtAuthorName.Enabled = abFlag;
        txtBookName.Enabled = abFlag;
        txtPublisherName.Enabled = abFlag;
        txtDescription.Enabled = abFlag;
        txtRemark.Enabled = abFlag;
        txtRackNumber.Enabled = abFlag;
        txtShelf.Enabled = abFlag;
        cmbMainCategory.Enabled = abFlag;
        optPrintable.Enabled = abFlag;
        optNonPrintable.Enabled = abFlag;
        txtLanguage.Enabled = abFlag;
        txtISBN.Enabled = abFlag;
        txtClassification.Enabled = abFlag;
        txtLostPercentage.Enabled = abFlag;
        chkForReadong.Enabled = abFlag;
        chkListClasses.Enabled = abFlag;
        ChkAllStandards.Enabled = abFlag;
    }

    /// <summary>
    /// This method is used for filling grid view with Book No.
    /// </summary>
    /// <param name="iBookId"></param>
    /// <param name="iSchoolId"></param>
    private void FillAccessionDetailsGrid(int aiBookId, int aiSchoolId)
    {
        BookBL oBookBL = new BookBL();
        DataTable oDtBookNo = oBookBL.GetBookNoDetails(aiBookId, aiSchoolId);
        ViewState[S_ACCESSION_DETAILS] = oDtBookNo;
        if (hidIsAddQuantity.Value == "false")
        {
            grdBookNo.DataSource = oDtBookNo.DefaultView;
            grdBookNo.DataBind();
        }
        else
        {
            tdAddBookGrd.Visible = true;
            ViewState[S_ACCESSION_DETAILS] = null;
            grdvwAddBookNo.DataSource = oDtBookNo.DefaultView;
            grdvwAddBookNo.DataBind();
        }
    }

    /// <summary>
    /// This method is used to set default value at the time of Add Book Quantity.
    /// </summary>
    private void SetDefaultValueForAddQuntity()
    {
        hidIsAddQuantity.Value = "true";
        hidIsNewBook.Value = "true";
        hidIsNewBookNo.Value = "true";
        hidBookSrNo.Value = "0";
        ViewState[S_ACCESSION_DETAILS] = null;
        grdBookNo.DataSource = null;
        grdBookNo.DataBind();
        grdBookNo.Columns[7].Visible = true;
        grdBookNo.Columns[8].Visible = true;
        EmptyTextBoxes();
        btnAdd.Text = S_ADD_MODE;
    }

    /// <summary>
    /// This method is used for assigning control with the respected values in Edit/Add Copy mode.
    /// </summary>
    private void AssignControlForAddEditMode()
    {
		if (QueryString != null && QueryString.Count > 0)
        {
			if (QueryString["BookId"] != null)
			hidBookId.Value = QueryString["BookId"];
			if (QueryString["IsEditMode"] != null)
			hidModeType.Value = QueryString["IsEditMode"];
            if (hidModeType.Value == S_EDIT_MODE)
            {
                hidIsAddQuantity.Value = "false";
                FillBookInfoForEdit();
                SetEditModeControls();
            }
            else if (hidModeType.Value == S_ADD_MODE)
            {
                SetDefaultValueForAddQuntity();
                txtBookPrice.Text = string.Empty;
                txtTotPages.Text = string.Empty;
                FillBookInfoForEdit();
                HideControl(false);
                wizard_BookDetails.ActiveStepIndex = Constants.I_ONE;
            }
        }
    }

    /// <summary>
    /// This method is used to redirect main source page.
    /// </summary>
    private void RedirectToMainPage()
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage(Constants.S_PAGE_LIBRARY_MANAGEMENT);
    }

    /// <summary>
    /// This method is used for the setting of hidden feild as per Add/Edit opertation.
    /// </summary>
    private void SetAddQuantityControl()
    {
        btnAdd.Text = S_ADD_MODE;
        if (hidModeType.Value != S_EDIT_MODE)
        {
            hidIsNewBookNo.Value = "true";
            txtBookNo.ReadOnly = false;
            txtBookPrice.ReadOnly = false;
            txtTotPages.ReadOnly = false;
            cPurchageDate.Enabled = true;
            chkIsGifted.Enabled = true;
            txtPurchaseDate.ReadOnly = false;
            txtBillNo.ReadOnly = false;
            cmbVendorName.Enabled = true;
            txtISBN.ReadOnly = false;
        }
        else
        {
            txtBookNo.ReadOnly = true;
            txtBookPrice.ReadOnly = true;
            txtTotPages.ReadOnly = true;
            btnAdd.Visible = false;
            cPurchageDate.Enabled = false;
            chkIsGifted.Enabled = false;
            txtPurchaseDate.ReadOnly = true;
            txtBillNo.ReadOnly = true;
            cmbVendorName.Enabled = false;
            txtISBN.ReadOnly = true;
        }
    }

    /// <summary>
    /// This method is used to fill Category, Standards and Vendors combo box.
    /// </summary>
    private void FillAllCombos()
    {
        FillMainCategoryCombo();
        FillStandardsCombo();
        FillVendorsCombo();
    }

    /// <summary>
    /// This method is used to fill Category combo box.
    /// </summary>
    /// <param name="oDTCategory"></param>
    private void FillMainCategoryCombo()
    {
        DataTable oDTBook = GetMainCategoryDetails();
        ControlUtility.FillDropDownList(oDTBook, ref cmbMainCategory, "Category_Id", "Category_Name", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill Standards combo box.
    /// </summary>
    private void FillStandardsCombo()
    {
        List<ClassDetails> lstClassDetails = BookBL.GetStandards(miSchoolId, miAcademicYearId);
        lstClassDetails.ForEach(ClassNames => chkListClasses.Items.Add(new ListItem(ClassNames.Classname, ClassNames.StandardDivisionId.ToString())));
        chkListClasses.Items.Add(new ListItem(S_FOR_PARENT_STAFF, S_HUNDRED));
    }

    /// <summary>
    /// This method is used to fill Vendors combo box.
    /// </summary>
    private void FillVendorsCombo()
    {
        cmbVendorName.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
        List<LibraryVendors> lstLibraryVendors = BookBL.GetLibraryVendors(miSchoolId);
        lstLibraryVendors.ForEach(Vendor => cmbVendorName.Items.Add(new ListItem(Vendor.VendorName, Vendor.VendorId.ToString())));
    }

    /// <summary>
    /// This function checks the preconditons of Exams.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AddEditBook);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            btnBack.Visible = false;
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
    /// This method is used to visible or hide controls depends on configuration done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        wizard_BookDetails.Visible = false;
        btnBack.Visible = true;
    }

    #endregion


   
}
