using System;
using BusinessLogic;
using Utility;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BusinessLogic.Exceptions;

/// <summary>
/// This class displays the fees details of selected student.
/// Only Admin and Librarian users have access to the page.
/// 1. User 1st selects a student.
/// 2. The details of fees paid by him are displayed.
/// 3. And a data entry form is provided to pay fees.
/// 4. To pay: He 1st selects a fee type. and the details of the feetype,
/// like. the fee to be paid for 1 interval(month, term, year) are displayed.
/// user then selects an interval. i.e no of (
/// </summary>

public partial class StudentLibraryFees : SchoolBase
{
    #region " Constants "


    private const string S_STANDARD_FEE_TYPE_ID = "SchoolWise_Standard_FeeType_Id";
    private const string S_ORIGINAL_FEE_TYPE_ID = "Original_Fee_Type_Id";
    private const string S_FEE_TYPE_NAME_FIEELD = "Fee_Type";
    private const string S_FEE_TYPE_ID_FIELD = "Fee_Type_Id";

    private const string S_VIESTATE_FEETYPES = "View_FeeTypes";
    private const string S_VW_FEECONFIG = "Fee_Config";
    private const string S_VW_LATEFEECONFIG = "LateFee_Config";


    private const string S_LBL_TEXT_FOR_MONTHLY = "Month(s) ";
    private const string S_LBL_TEXT_FOR_TERM = "Term(s) ";
    private const string S_LBL_TEXT_FOR_ANNUAL = "Annual payment Intervals :";
    private const string S_LBL_TEXT_DEFAULT = "Select Interval :";

    private const string S_LABEL_TEXT_RCPT_NO = "Receipt Number : ";
    private const int I_DATAKEYNAME_ID = 3;
    private const int I_LNK_COL_INDEX = 7;

    private const string S_BUTTON_SEARCH_CPTION = "Show";
    private const string S_BUTTON_CHANGE_CPTION = "Change student";

    private const string S_LBL_TEXT_FOR_MONTHLY_RATE = "Monthly Fees :";
    private const string S_LBL_TEXT_FOR_TERM_RATE = "Term Fees :";
    private const string S_LBL_TEXT_FOR_ANNUAL_RATE = "Annual Fees :";
    private const string S_LBL_TEXT_DEFAULT_RATE = "Fees :";

    private const string S_PRINT_URL_TERM = "~/Common/TermFeesReciept.aspx?";
    private const string S_PRINT_URL_MONTHLY = "ReceiptForMonthlyFeesPayment.aspx?";

    private const string S_FEE_TYPE_MONTHLY = "Monthly";
    private const string S_FEE_TYPE_TERM = "Term";
    private const string S_FEE_TYPE_ANNUAL = "Annual";

    private const int I_COLUMN_INDEX_PAYMENT_DATE = 2;
    private const int I_COLUMN_INDEX_PENDING_DUE_DATE = 2;
    private const string S_DEFAULT = "0";

    private const string S_VW_INTERVALS = "IntervalsDS";

    private const string S_ERROR_MSG_FEE_NOT_CONFIGURED = "Fees structure for this standard is not yet configured.";
    private const string S_ERROR_MSG_LATEFEE_NOT_CONFIGURED = "<BR>Late fees not declared. ";
    const string S_ERR_MSG_FOR_STUDENT = "Fee details not yet configured.";
    #endregion

    #region " Events "

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			ApplyMouseHoverEffect(new List<Button>() { btnBack, btnPay, btnPayPrint, btnSearch1 });
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("ReturnRenewUI.aspx");
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Method
    #endregion
}
