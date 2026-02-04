/* File Name :- PayInternalFeeOnlinePopup.aspx.cs
 * Created Date :- 08-Mar-2019
 * Class Description :- This class is used pay internal Fee online.
 * Created By :- Dnyaneshwar SHinde.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Drawing;
using BusinessLogic;
using BusinessLogic.Exceptions;
using FeeEntities;
using Utility;
using System.Globalization;
using SchoolBusinessService;
using AccountsEntities;
using System.ServiceModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class PayInternalFeeOnlinePopup : SchoolBase
{

    #region -- CONSTANT(s) --

    private const string S_CHECK_BOX_STUDENTPAY = "chkSelect";
    
    #endregion -- CONSTANT(s) --    

    #region -- DataMember --

    private bool IsNextYearPayment
    {
        get { return (hidIsNextYearFeePayment.Value == Constants.S_ZERO ? false : true); }
    }

    #endregion

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This event is used to set defualt remark text,default date and decrypt query string.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillAcademicYears();
                DisplayFeeDetails();
                SetJavaScriptAttributes();
                SetFocus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to enable disable listview controls and set javascript to listview buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwInternalFee_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;                
                HyperLink hlnkReceipt = oCurrentItem.FindControl("hlnkReceipt") as HyperLink;                
                HyperLink ohlnkReceipt = oCurrentItem.FindControl("hlnkReceipt") as HyperLink;
                Label lblPaidDate = oCurrentItem.FindControl("lblPaidDate") as Label;
                lblPaidDate.Text = lblPaidDate.Text.ToDateTime().ToString("dd-MMM-yyyy", new CultureInfo("en"));
                Label lblAmount = oCurrentItem.FindControl("lblAmount") as Label;
                Label lblFeeType = oCurrentItem.FindControl("lblFeeType") as Label;
                Label lblPaybleFor = oCurrentItem.FindControl("lblPaybleFor") as Label;
                string sDebitCredit = lstvwInternalFee.DataKeys[iRowId]["DebitCredit"].ToString();
                int iInternalFeeDetailsId = lstvwInternalFee.DataKeys[iRowId]["InternalFeeDetailsId"].ToInt();
                int iReceiptNo = lstvwInternalFee.DataKeys[iRowId]["ReceiptNo"].ToInt();                
                int iSerialNumber = lstvwInternalFee.DataKeys[iRowId]["SerialNumber"].ToInt();
                int iSchoolwiseStudentId = lstvwInternalFee.DataKeys[iRowId]["SchoolwiseStudentId"].ToInt();

                InternalFeeDebitDetails oInternalFeeDebitDetails = e.Item.DataItem as InternalFeeDebitDetails;
                if (chkSelect != null && hidIsOnlinePayment.Value != Constants.S_ONE)
                {
                    chkSelect.Visible = false;
                }
                if (!oInternalFeeDebitDetails.IsDueDateApplicable)
                    lblPaidDate.Text = "-";

                if (sDebitCredit.Trim() != "Debit")
                {
                    chkSelect.Visible = false;
                    int iAcademicYearId = miAcademicYearId;
                    if (IsNextYearPayment)
                        iAcademicYearId = hidNextAcademicYearId.Value.ToInt();

                    string sRecieptQueryString = String.Format("StudentId={0}&AcademicYear={1}&RegNo={2}&InternalFeeDetailsId={3}&ReceiptNo={4}&SerialNumber={5}&IsNextYearFeePayment={6}", iSchoolwiseStudentId, iAcademicYearId, hidRegNo.Value, iInternalFeeDetailsId, iReceiptNo, iSerialNumber, (IsNextYearPayment ? 1 : 0));
                    ohlnkReceipt.Attributes.Add("onclick", "if(!OpenRecieptPopup( '" + CommonUtility.EncryptQuerystring(sRecieptQueryString) + "' )) return false;");
                }
                else
                {   
                    hlnkReceipt.Visible = false;                    
                }

                if (iReceiptNo.ToString() == "999999")
                    hlnkReceipt.Visible = false;                     

                if (iReceiptNo.ToString() == "888888")
                    chkSelect.Visible = false;

                chkSelect.Attributes.Add("onclick", "CheckSelected(this,'" + iRowId + "')");                

                if (!chkSelect.Checked && chkSelect.Visible && oInternalFeeDebitDetails.IsDueDateApplicable && lblPaidDate.Text.ToDateTime() < DateTime.Today)
                {
                    var tableRow = oCurrentItem.FindControl("trlstvwRow") as System.Web.UI.HtmlControls.HtmlTableRow;
                    tableRow.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FEEABA");
                    lblPaidDate.ForeColor = Color.Red;                    
                    lblAmount.ForeColor = Color.Red;
                    lblPaybleFor.ForeColor = Color.Red;
                    lblFeeType.ForeColor = Color.Red;
                }                      
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to pay internal Fee online.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder strInternalFeeIds = new StringBuilder();            
            int iTotalAmount = Constants.I_ZERO;            

            for (int iRowCnt = 0; iRowCnt < lstvwInternalFee.Items.Count; iRowCnt++)
            {
                var oChkPay = lstvwInternalFee.Items[iRowCnt].FindControl(S_CHECK_BOX_STUDENTPAY) as CheckBox;

                if (oChkPay.Checked)
                {
                    strInternalFeeIds = strInternalFeeIds.Append("," + Convert.ToString(lstvwInternalFee.DataKeys[iRowCnt]["InternalFeeDetailsId"]));                    
                    iTotalAmount = iTotalAmount + lstvwInternalFee.DataKeys[iRowCnt]["Amount"].ToInt();
                }
            }

            string sInternalFeeId = string.Empty;
            if (strInternalFeeIds.ToString().StartsWith(","))
                sInternalFeeId = strInternalFeeIds.ToString().Substring(1);

            strInternalFeeIds.Clear();

            //Set query string.
            //string sQueryString = string.Format("StudentId={0}&InternalFeeDetailsId={1}&IsOnlineInternalFeePayment={2}&IsForNextYear={3}&AcadmicYearId={4}&TotalAmount={5}&IsForInternalFee=1", hidStudentId.Value, sInternalFeeId, 1, hidIsNextYearFeePayment.Value == Constants.S_ZERO ? Constants.S_NO : Constants.S_YES, hidNextAcademicYearId.Value, iTotalAmount);
            string sQueryString = string.Format("StudentId={0}&InternalFeeDetailsId={1}&IsOnlineInternalFeePayment={2}&IsForNextYear={3}&AcadmicYearId={4}&TotalAmount={5}&IsForInternalFee=1", hidStudentId.Value, sInternalFeeId, 1, hidIsNextYearFeePayment.Value == Constants.S_ZERO ? Constants.S_NO : Constants.S_YES, cmbAcademicYrId.SelectedValue, iTotalAmount);
            hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);                       
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display fee details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAcademicYrId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           DisplayFeeDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region -- Private Method(s) --

    /// <summary>
    /// This method is used to set focus on controls.
    /// </summary>
    private void SetFocus()
    {
        CheckBox chkSelect = lstvwInternalFee.FindControl("chkSelectAll") as CheckBox;
        if (chkSelect != null)
            chkSelect.Focus();
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        hidStudentId.Value = Session[Constants.S_SESSION_STUDENT_ID].ToString();

        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
        {
            if (Session["InternalFeeData"] != null)
            {
                Dictionary<string, string> oDict = Session["InternalFeeData"] as Dictionary<string, string>;
                hidStudentId.Value = oDict["StudentId"];
                hidNextAcademicYearId.Value = oDict["NextAcademicYearId"];
                hidRegNo.Value = oDict["RegNo"];
                hidIsNextYearFeePayment.Value = oDict["IsNextYearFeePayment"];
                hidIsOnlinePayment.Value = oDict["IsOnlinePayment"];
            }

            return;
        }

        if (!QueryString["StudentId"].IsNull())
            hidStudentId.Value = QueryString["StudentId"];

        if (!QueryString["NextAcademicYearId"].IsNull())
            hidNextAcademicYearId.Value = QueryString["NextAcademicYearId"];       

        if (!QueryString["RegNo"].IsNull())
            hidRegNo.Value = QueryString["RegNo"];

        if (!QueryString["IsNextYearFeePayment"].IsNull())
            hidIsNextYearFeePayment.Value = QueryString["IsNextYearFeePayment"];
        else
            hidIsNextYearFeePayment.Value = Constants.S_ZERO;

        if (!QueryString["IsOnlinePayment"].IsNull() && QueryString["IsOnlinePayment"] == Constants.S_ONE)
        {
            hidIsOnlinePayment.Value = Constants.S_ONE;
            btnSave.Visible = true;            
        }
        else
        {
            hidIsOnlinePayment.Value = Constants.S_ZERO;
            btnSave.Visible = false;            
        }

        Dictionary<string,string> oDictData = new Dictionary<string,string>
        { 
            {"StudentId",hidStudentId.Value},
            {"NextAcademicYearId",hidNextAcademicYearId.Value },
            {"RegNo", hidRegNo.Value},
            {"IsNextYearFeePayment", hidIsNextYearFeePayment.Value},
            {"IsOnlinePayment", hidIsOnlinePayment.Value}
        };
        Session["InternalFeeData"] = oDictData;
    }

    /// <summary>
    /// This Method is used to set default remark text.
    /// </summary>
    private void DisplayFeeDetails()
    {
        InternalFeeDetailsBL oInternalFeeDetailsBL = new InternalFeeDetailsBL();
        List<InternalFeeDebitDetails> lstInternalFeeDebitDetails = oInternalFeeDetailsBL.GetInternalFeeDebitDetailsForOnlinePayment(miSchoolId, cmbAcademicYrId.SelectedValue.ToInt(), hidStudentId.Value.ToInt(), IsNextYearPayment);
        lstvwInternalFee.DataSource = lstInternalFeeDebitDetails;
        lstvwInternalFee.DataBind();

        if (lstInternalFeeDebitDetails.Count > 0)
            hidStudentId.Value = lstInternalFeeDebitDetails[0].YearwiseStudentId.ToString();

        CheckBox chkSelectAll = lstvwInternalFee.FindControl("chkSelectAll") as CheckBox;
        if (lstInternalFeeDebitDetails.Any(SS => SS.DebitCredit == "Debit"))
        {
            btnSave.Enabled = true;
           // chkSelectAll.Visible = true;
        }
        else
        {
            btnSave.Enabled = false;
         //   chkSelectAll.Visible = false;
        }

        if (chkSelectAll != null)
            chkSelectAll.Checked = false;
    }

    /// <summary>
    /// This method used to set java script attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributes()
    {   
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        string sQueryString = String.Format("StudentId={0}&Amount={1}&RegNo={2}&pIndex={3}", hidStudentId.Value, null, null, null);        
        if (Settings.AutoPopulateInternalFeeRemark)
            hidRemark.Value = Constants.S_ONE;
        else
            hidRemark.Value = Constants.S_ZERO;

        btnSave.Attributes.Add("Onclick", string.Format("if(!(ConfirmActionForStudent('{0}','{1}'))){{return false;}}",1,Resources.LocalizedResources.AtLeastOneEntryShouldBeSelectForPayingFee));
        btnCancel.Attributes.Add("onclick", "CloseWindow()");
    }

    private void FillAcademicYears()
    {
        DataSet oDtYearInfo = SchoolWiseAcademicYearMasterBL.GetPendingFeeAcademicYears(miSchoolId, hidStudentId.Value.ToInt(), miAcademicYearId, true, hidIsNextYearFeePayment.Value.ToInt().ToBool());
        if (oDtYearInfo != null && oDtYearInfo.Tables[0].Rows.Count > 0 && oDtYearInfo.Tables[0].Rows[0][0] != DBNull.Value)
        {
            cmbAcademicYrId.Bind(oDtYearInfo.Tables[0], "Academic_Year_Id", "AcademicYear", Constants.S_SELECT);

            if (oDtYearInfo.Tables[1].Rows.Count > 0)
                lblPendingFeeAcademicYear.Text = oDtYearInfo.Tables[1].Rows[0][0].ToString();
            else
                lblPendingFeeAcademicYear.Text = "-";

            if (cmbAcademicYrId.Items.Count == 1)
                cmbAcademicYrId.Enabled = false;
            else
            {
                if (Session[Constants.S_SESSION_SELECTED_YEAR] != null && Session[Constants.S_SESSION_SELECTED_YEAR].ToString() != string.Empty && Session[Constants.S_SESSION_SELECTED_YEAR].ToString() != Constants.S_ZERO &&
                    Session[Constants.S_SESSION_DO_REFRESH_PAGE] != null && Session[Constants.S_SESSION_DO_REFRESH_PAGE].ToString() == Constants.S_ONE)
                {
                    int iAcademicYEar = Session[Constants.S_SESSION_SELECTED_YEAR].ToInt();
                    ListItem oListItem = cmbAcademicYrId.Items.FindByValue(iAcademicYEar.ToString());

                    if (oListItem != null)
                        oListItem.Selected = true;
                    else
                        cmbAcademicYrId.SelectedValue = oDtYearInfo.Tables[0].Rows[0]["Academic_Year_Id"].ToString();
                }
                else
                    cmbAcademicYrId.SelectedValue = oDtYearInfo.Tables[0].Rows[0]["Academic_Year_Id"].ToString();
            }
        }
        else
            cmbAcademicYrId.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });

        Session[Constants.S_SESSION_SELECTED_YEAR] = null;
        Session[Constants.S_SESSION_DO_REFRESH_PAGE] = null;
    } 
       
    #endregion     
   
}