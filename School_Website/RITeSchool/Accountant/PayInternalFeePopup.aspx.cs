// File Name  : PayInternalFeePopup.aspx.cs
// Created By : Deepak
// Date       : 07/11/2009
//Description :This class is used pay internal fees.  

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
using SchoolEntities.StudentFee;

public partial class PayInternalFeePopup : SchoolBase
{
    #region -- CONSTANT(s) --
   
    private bool mbIsFeePaid = false;

    #endregion -- CONSTANT(s) --

    private Constants.PaymentMode PaymentType
    {
        get
        {
            if (optCash.Checked)
                return Constants.PaymentMode.Cash;
            else if (optCheque.Checked)
                return Constants.PaymentMode.Cheque;
            else
                return Constants.PaymentMode.Electronic;
        }
    }

    private int TypeId
    {
        get
        {
            if (optCash.Checked)
                return 1;
            else if (optCheque.Checked)
                return 2;
            else
                return 3;
        }
    }

    private bool IsNextYearPayment
    {
        get { return (hidIsNextYearFeePayment.Value == Constants.S_ZERO ? false : true); }
    }

    /// <summary>
    /// Returns true if the Accounts module is enabled, false otherwise.
    /// </summary>
    private bool UpdateInternalFeeinDayBook
    {
        get
        {
            if (moSchool == Constants.SchoolId.PPS)
                return Settings.EnableAccountsModule;
            else
                return false;
        }
    }

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
                DisplayFeeDetails();
				SetJavaScriptAttributes();
                cal_PaymentDate.DateValue = Convert.ToDateTime(DateTime.Today.ToString("dd-MMM-yyyy", new CultureInfo("en")));
				hidServerDate.Value = Convert.ToString(DateTime.Today,new CultureInfo("en"));
                FillChequeDetails();
                SetFocus();
                RefreshValues();
                FillElectronicPaymentTypes();               
			}
			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    private void SetFocus()
    {
        CheckBox chkSelect = lstvwInternalFee.FindControl("chkSelectAll") as CheckBox;
        if (chkSelect != null)
            chkSelect.Focus();
    }
	/// <summary>
	/// This event is used to save fee details and print the receipt.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSaveAndPrint_Click(object sender, EventArgs e)
	{
		try
		{
            int iSerialNo;
            int iInternalFeeDetailsId = Save(out iSerialNo);
            DisplayFeeDetails();
            DisplayMessage(Resources.LocalizedResources.FeesPaidSuccessfully);
            txtRemark.Text = "";
            if (iInternalFeeDetailsId != 0 || IsNextYearPayment)
            {
                string sQueryString = string.Format("StudentId={0}&InternalFeeDetailsId={1}&AcademicYear={2}&RegNo={3}&SerialNumber={4}&IsNextYearFeePayment={5}",
                                                     hidStudentId.Value,
                                                     iInternalFeeDetailsId,
                                                     miAcademicYearId,
                                                     hidRegNo.Value,
                                                     iSerialNo,
                                                     (IsNextYearPayment?1:0));
                sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                Response.Write("<Script language='javascript'>window.open('../Accountant/InternalFeePaymentReceipt.aspx?" + sQueryString + "','_blank','left=0, top=0, height=450, width=670, resizable= no, scrollbars= yes')</Script>");
            }
            else
            {
                string sQueryString = string.Format("&RegNo={0}&StandardID={1}&FeeTypeID={2}&pIndex={3}",
                                                     hidRegNo.Value,
                                                     hidStandardID.Value,
                                                     hidFeeTypeID.Value,
                                                     hidPageIndex.Value);
                Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + "'?" + CommonUtility.EncryptQuerystring(sQueryString) + "'" + ";window.close();window.opener.focus(); </Script>");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(" + TypeId + ");", true);
		}
        catch(ApplicationException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
        finally
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(" + TypeId + ");", true);
        }
	}

	/// <summary>
	/// This event is used to save fee details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
        try
        {
            int iSerialNo;
            Save(out iSerialNo);
            DisplayFeeDetails();           
            DisplayMessage(Resources.LocalizedResources.FeesPaidSuccessfully);
            txtRemark.Text = "";
            Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
            Response.Write("<Script language='Javascript'>window.opener.location.reload(true); window.close();window.opener.focus(); </Script>");
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(" + TypeId + ");", true);
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
                TextBox txtPartialFee = oCurrentItem.FindControl("txtPartialFee") as TextBox;                
                HyperLink hlnkReceipt = oCurrentItem.FindControl("hlnkReceipt") as HyperLink;
                ImageButton imgDelete = oCurrentItem.FindControl("imgDelete") as ImageButton;                
                HyperLink ohlnkReceipt = oCurrentItem.FindControl("hlnkReceipt") as HyperLink;
                Label lblPaidDate=oCurrentItem.FindControl("lblPaidDate") as Label;
                lblPaidDate.Text = lblPaidDate.Text.ToDateTime().ToString("dd-MMM-yyyy", new CultureInfo("en"));
                Label lblAmount = oCurrentItem.FindControl("lblAmount") as Label;
                Label lblFeeType = oCurrentItem.FindControl("lblFeeType") as Label;
                Label lblPaybleFor = oCurrentItem.FindControl("lblPaybleFor") as Label;
                string sDebitCredit = lstvwInternalFee.DataKeys[iRowId]["DebitCredit"].ToString();
                int iInternalFeeDetailsId=lstvwInternalFee.DataKeys[iRowId]["InternalFeeDetailsId"].ToInt();
                int iReceiptNo = lstvwInternalFee.DataKeys[iRowId]["ReceiptNo"].ToInt();
                int iIsLastCredit = lstvwInternalFee.DataKeys[iRowId]["IsLastCredit"].ToInt();
                int iSerialNumber = lstvwInternalFee.DataKeys[iRowId]["SerialNumber"].ToInt();
                int iNetBankingPaymentTransactionId = lstvwInternalFee.DataKeys[iRowId]["NetBankingPaymentTransactionId"].ToInt();

                int iAccountHeaderId = lstvwInternalFee.DataKeys[iRowId]["AccountHeaderId"].ToInt();
                string sPaymentDoneDate = lstvwInternalFee.DataKeys[iRowId]["PaymentDoneDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);

                InternalFeeDebitDetails oInternalFeeDebitDetails = e.Item.DataItem as InternalFeeDebitDetails;

                if (!oInternalFeeDebitDetails.IsDueDateApplicable)
                    lblPaidDate.Text = "-";

                if (sDebitCredit.Trim() != "Debit")                
                {
                    chkSelect.Visible = false;
                    txtPartialFee.Visible = false;
                    string sRecieptQueryString = String.Format("StudentId={0}&AcademicYear={1}&RegNo={2}&InternalFeeDetailsId={3}&ReceiptNo={4}&SerialNumber={5}&IsNextYearFeePayment={6}&Date={7}&AccountHeaderId={8}", hidStudentId.Value, miAcademicYearId, hidRegNo.Value, iInternalFeeDetailsId, iReceiptNo, iSerialNumber, (IsNextYearPayment ? 1 : 0), sPaymentDoneDate, iAccountHeaderId);
                    ohlnkReceipt.Attributes.Add("onclick", "if(!OpenRecieptPopup( '" + CommonUtility.EncryptQuerystring(sRecieptQueryString) + "' )) return false;");
                }
                else
                {
                    txtPartialFee.Enabled = false;
                    hlnkReceipt.Visible = false;                    
                    imgDelete.Visible = false;
                }

                if (iReceiptNo.ToString() == "999999")
                    hlnkReceipt.Visible = false;
                else
                    mbIsFeePaid = true;
                if (iIsLastCredit == Constants.I_ONE)
                {
                    if (iNetBankingPaymentTransactionId == 0)
                    {
                        imgDelete.Visible = true;
                        imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                    }
                    else
                        imgDelete.Visible = false;
                }
                else
                    imgDelete.Visible = false;

                if (iReceiptNo.ToString() == "888888")
                    chkSelect.Visible = false;


                chkSelect.Attributes.Add("onclick", "CheckSelected(this,'" + iRowId + "')");
                txtPartialFee.Attributes.Add("onchange", "ChangeFees(this, " + iRowId + ")");

                if (!chkSelect.Checked && chkSelect.Visible && oInternalFeeDebitDetails.IsDueDateApplicable && lblPaidDate.Text.ToDateTime() < DateTime.Today)
                {
                    var tableRow = oCurrentItem.FindControl("trlstvwRow") as System.Web.UI.HtmlControls.HtmlTableRow;
                    tableRow.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FEEABA");
                    lblPaidDate.ForeColor = Color.Red;
                    txtPartialFee.ForeColor = Color.Red;
                    lblAmount.ForeColor = Color.Red;
                    lblPaybleFor.ForeColor = Color.Red;
                    lblFeeType.ForeColor = Color.Red;                    
                }

                if (oInternalFeeDebitDetails.DebitCredit.Trim() == "Credit")
                {
                    if (!oInternalFeeDebitDetails.IsCleared)
                    {
                        HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trlstvwRow") as HtmlTableRow;
                        if (oHtmlTableRow != null)
                            oHtmlTableRow.Attributes.Add("class", "UnclearedChq");
                    }

                    if (oInternalFeeDebitDetails.IsChequeBounced == Constants.S_YES)
                    {
                        HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trlstvwRow") as HtmlTableRow;
                        if (oHtmlTableRow != null)
                        {   
                            oHtmlTableRow.Attributes.Add("class", "BounceCheque");
                            hlnkReceipt.Visible = false;
                            imgDelete.Visible = false;
                        }
                    }
                }

                if (hidIsNextYearFeePayment.Value == Constants.S_ONE)
                    tdCustomReceipt.Visible = false;
                else
                    tdCustomReceipt.Visible = mbIsFeePaid;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwInternalFee_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;

            if (e.CommandName == "Remove")
            {
                var oInternalFeeDetailsBL = new InternalFeeDetailsBL();                
                int iInternalFeeDetailsId = lstvwInternalFee.DataKeys[iRowId]["InternalFeeDetailsId"].ToInt();
                int iInternalFeeMasterId = lstvwInternalFee.DataKeys[iRowId]["InternalFeeMasterId"].ToInt();
                int iReceiptNo = lstvwInternalFee.DataKeys[iRowId]["ReceiptNo"].ToInt();

                string sStudentFeeIdsXML = String.Empty;
                if (UpdateInternalFeeinDayBook)
                {
                    var oVoucherClient = new AccountVoucherClient();
                    try
                    {
                        oVoucherClient.Open();
                        List<FeeVoucherParticulars> lstFeeParticulars = oVoucherClient.GetInternalFeePaymentParticulars(miSchoolId, miAcademicYearId, miFinancialYearId, hidStudentId.Value.ToInt(), iReceiptNo.ToString());
                        sStudentFeeIdsXML = CommonUtility.GetXMLForList(lstFeeParticulars);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while getting FeeVoucher particulars. StudentId : {0}. ReceiptNo : {1}", hidStudentId.Value.ToInt(), iReceiptNo.ToString()));
                    }
                    finally
                    {
                        if (oVoucherClient.State != CommunicationState.Faulted)
                            oVoucherClient.Close();
                    }
                }

                oInternalFeeDetailsBL.DeleteInternalFeeDetails(iInternalFeeDetailsId, miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt(), miUserId, IsNextYearPayment, iInternalFeeMasterId);

                if (UpdateInternalFeeinDayBook)
                {
                    var oVoucherClient = new AccountVoucherClient();
                    try
                    {
                        oVoucherClient.Open();
                        oVoucherClient.DeleteInternalFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, hidStudentId.Value.ToInt(), iReceiptNo.ToString(), sStudentFeeIdsXML, miUserId, true);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while deleting a fee payment. StudentId : {0}. ReceiptNo : {1}", hidStudentId.Value.ToInt(), iReceiptNo.ToString()));
                    }
                    finally
                    {
                        if (oVoucherClient.State != CommunicationState.Faulted)
                            oVoucherClient.Close();
                    }
                }

				txtRemark.Text = string.Empty;
				cal_PaymentDate.DateValue = DateTime.Today;
                DisplayMessage(Resources.LocalizedResources.FeesDeletedSuccessfully);
                DisplayFeeDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(" + TypeId + ");", true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to close popup.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
            if (hidPageIndex.Value.ToString()=="-9999")
                Response.Write("<Script language='Javascript'>window.opener.UpdateStaus();window.close();</Script>");
            else
            {
                if (hidRegNo.Value.Contains("Form Number"))
                    Response.Write("<Script language='Javascript'>window.opener.UpdateStaus();window.close();</Script>");
                else
                {
                    string sQueryString = string.Format("&RegNo={0}&StandardID={1}&FeeTypeID={2}&pIndex={3}", hidRegNo.Value, hidStandardID.Value, hidFeeTypeID.Value, hidPageIndex.Value);
                    hidQueryString.Value = sQueryString;
                    Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + "'?" + CommonUtility.EncryptQuerystring(sQueryString) + "'" + ";window.close();window.opener.focus(); </Script>");
                }
            }            
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sStr"></param>
    private void DisplayMessage(string sStr)
    {
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = sStr;        
    }

	/// <summary>
	/// This method populate InternalFeeDetailsBL obect and returns it.
	/// </summary>
	/// <returns></returns>
    private List<InternalFeeDebitDetails> GetInternalFeeDebitDetails()
	{
        
        List<InternalFeeDebitDetails> oInternalFeeDebitDetails = new List<InternalFeeDebitDetails>();
        for (int iRowCount = 0; iRowCount < lstvwInternalFee.Items.Count ; iRowCount++)
        {
            CheckBox chkSelect= lstvwInternalFee.Items[iRowCount].FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked)
            {
                int iIternalFeeDetailsId = lstvwInternalFee.DataKeys[iRowCount]["InternalFeeDetailsId"].ToInt();
                int iFeeDetailsId = lstvwInternalFee.DataKeys[iRowCount]["FeeDetailsId"].ToInt();                
                TextBox txtPartialFee = lstvwInternalFee.Items[iRowCount].FindControl("txtPartialFee") as TextBox;
                int iInternalFeeMasterId = lstvwInternalFee.DataKeys[iRowCount]["InternalFeeMasterId"].ToInt();

                if (txtPartialFee != null && txtPartialFee.Text.Trim()!="")
                {
                
                    oInternalFeeDebitDetails.Add(new InternalFeeDebitDetails
                                                {
                                                    InternalFeeDetailsId=iIternalFeeDetailsId,
                                                    Amount=txtPartialFee.Text.ToInt(),
                                                    Remarks= txtRemark.Text,
                                                    FeeDetailsId = iFeeDetailsId,
                                                    InternalFeeMasterId = iInternalFeeMasterId
                                                });                   
                }
            }
        }
        return oInternalFeeDebitDetails;
	}
    ////////new method add////////////////////////////
    private string GetElectronicXml()
    {
        if (optElectronic.Checked)
        {
            InternalFeeElectronicDetails oInternalFeeElectronicDetails = new InternalFeeElectronicDetails
            {
                BankId = ddlBankName.SelectedValue.ToInt(),
                TransactionNumber = txtTransactionNo.Text,
                TypeId = cmbElectronicTypes.SelectedValue.ToInt(),
                DepositedBankId = ddlDepositeInBank.SelectedValue.ToInt()
            };
            return base.GenerateXml(oInternalFeeElectronicDetails);
        }
        else
            return string.Empty;

    }
	/// <summary>
	/// This method is used to save fee details.
	/// </summary>
	/// <returns></returns>
	private int Save(out int aiSerialNumber)
	{
        InternalFeeDetailsBL oInternalFeeDetailsBL=new InternalFeeDetailsBL();
        if (optCheque.Checked)
        {
            if (oInternalFeeDetailsBL.IsDuplicateChequeNumber(miSchoolId, miAcademicYearId, txtChequeNumber.Text, hidStudentId.Value.ToInt(), cmbBank.SelectedValue.ToInt(),IsNextYearPayment))
            {
                throw new ApplicationException("Cheque Number already exists for this student. Please enter another cheque number.");
            }
        }
        else if (optElectronic.Checked)
        {
            if (oInternalFeeDetailsBL.IsDuplicateTransactionNumber(miSchoolId, miAcademicYearId, txtTransactionNo.Text, hidStudentId.Value.ToInt(), ddlBankName.SelectedValue.ToInt(), IsNextYearPayment, cmbElectronicTypes.SelectedValue.ToInt()))
            {
                throw new ApplicationException("Transaction Number already exists for this type and bank. Please enter another Transaction Number.");
            }
        }
       
        List<InternalFeeDebitDetails> oInternalFeeDebitDetails = GetInternalFeeDebitDetails();
        string sXML=GenerateXml(oInternalFeeDebitDetails);
        string sChequeDetailsXml = GetChequeXml();
        string sElectronicDetailsXml = GetElectronicXml();          ///////////////////new add
        DataTable oDTInternalFeedetailsId = oInternalFeeDetailsBL.InsertInternalFeeDetails(sXML, miUserId, miSchoolId, miAcademicYearId, txtDate.Text.ToDateTime(), hidStudentId.Value.ToInt(), sChequeDetailsXml, PaymentType.ToInt(), IsNextYearPayment, sElectronicDetailsXml);


        if (UpdateInternalFeeinDayBook && optCash.Checked)
            RecordCashPayment(hidStudentId.Value.ToInt(), oDTInternalFeedetailsId.Rows[0]["ReceiptNo"].ToString());

		cal_PaymentDate.DateValue = DateTime.Today;

        if(optCheque.Checked)
            ResetChequeDetails();

        if (optElectronic.Checked)
            ResetElectronicDetails();
        
        aiSerialNumber = oDTInternalFeedetailsId.Rows[0]["SerialNumber"].ToInt();
        if (oDTInternalFeedetailsId != null && oDTInternalFeedetailsId.Rows.Count > 0 && oDTInternalFeedetailsId.Rows[0][0] != DBNull.Value)
            return oDTInternalFeedetailsId.Rows[0][0].ToInt();
		return 0;
	}

    /// <summary>
    /// This method is used to reset cheque details.
    /// </summary>
    private void ResetChequeDetails()
    {
        txtChequeNumber.Text = string.Empty;
        txtChequeDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        cmbBank.ClearSelection();
        cmbBank.SelectedValue = hidDefaultBankId.Value;
        cmbDepositInBank.SelectedValue = hidDefaultDepositeBank.Value;
        ddlDepositeInBank.SelectedValue = hidDefaultDepositeBank.Value;
    }

    /// <summary>
    /// This method is used to return cheque xml.
    /// </summary>
    /// <returns></returns>
    private string GetChequeXml()
    {
        if(optCheque.Checked)
        {
            InternalFeeChequeDetails oInternalFeeChequeDetails = new InternalFeeChequeDetails
            {
                BankId = cmbBank.SelectedValue.ToInt(),
                ChequeDate = txtChequeDate.Text,
                ChequeNo = txtChequeNumber.Text,
                DepositedBankId = cmbDepositInBank.SelectedValue.ToInt()
            };
            return base.GenerateXml(oInternalFeeChequeDetails);
        }
        else
            return string.Empty;
    }

    /// <summary>
    /// This Method is used to set default remark text.
    /// </summary>
    private void DisplayFeeDetails()
    {
        InternalFeeDetailsBL oInternalFeeDetailsBL = new InternalFeeDetailsBL();        
        List<InternalFeeDebitDetails> lstInternalFeeDebitDetails = oInternalFeeDetailsBL.GetInternalFeeDebitDetails(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt(), IsNextYearPayment);
        lstvwInternalFee.DataSource = lstInternalFeeDebitDetails;
        lstvwInternalFee.DataBind();       
        CheckBox chkSelectAll = lstvwInternalFee.FindControl("chkSelectAll") as CheckBox;
        if (chkSelectAll != null)
            chkSelectAll.Checked = false;

        hidDefaultBankId.Value = Constants.S_ZERO;
        if (lstInternalFeeDebitDetails.Count > 0)
            hidDefaultBankId.Value = lstInternalFeeDebitDetails[0].FrequentlyUsedBankId.ToString();        
    }

	/// <summary>
	/// This method is used to decrypt query string.
	/// </summary>
	private void ReadQuerystring()
	{
		if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
			return;       

		if (!QueryString["StudentId"].IsNull())
			hidStudentId.Value = QueryString["StudentId"];
			
		if (!QueryString["NextAcademicYearId"].IsNull())
			hidNextAcademicYearId.Value = QueryString["NextAcademicYearId"];
			
		if (!QueryString["StudentName"].IsNull())
		{
			hidStudentName.Value = QueryString["StudentName"];
			lblStudentHeading.Text = hidStudentName.Value;
		}

		if (!QueryString["RegNo"].IsNull())
			hidRegNo.Value = QueryString["RegNo"];
			
		if (!QueryString["FromDate"].IsNull())
			hidFromDate.Value = QueryString["FromDate"];
			
		if (!QueryString["ToDate"].IsNull())
			hidToDate.Value = QueryString["ToDate"];
			
		if (!QueryString["IncludePaid"].IsNull())
			hidIncludePaid.Value = QueryString["IncludePaid"];
			
		if (!QueryString["PayForNextYear"].IsNull())
			hidPayForNextYear.Value = QueryString["PayForNextYear"];
			
		if (!QueryString["IsRegNoFilter"].IsNull())
			hidIsRegNoFilter.Value = QueryString["IsRegNoFilter"];
			
		if (!QueryString["StandardID"].IsNull())
			hidStandardID.Value = QueryString["StandardID"];
			
		if (!QueryString["DivisionID"].IsNull())
			hidDivisionID.Value = QueryString["DivisionID"];
			
		if (!QueryString["FeeTypeID"].IsNull())
			hidFeeTypeID.Value = QueryString["FeeTypeID"];
			
		if (!QueryString["InternalFeeDetailsId"].IsNull())
			hidInternalFeeDetailsId.Value = QueryString["InternalFeeDetailsId"];
			
		if (!QueryString["pIndex"].IsNull())
			hidPageIndex.Value = QueryString["pIndex"];

        hidIsNextYearFeePayment.Value = Constants.S_ZERO;
        if (!QueryString["IsNextYearFeePayment"].IsNullOrEmpty())
        {
            hidIsNextYearFeePayment.Value = QueryString["IsNextYearFeePayment"].ToString();
            tdCustomReceipt.Visible = false;
        }
	}

	/// <summary>
	/// This method used to set java script attributes for buttons.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnSaveAndPrint });
        btnSave.Attributes.Add("onclick","DisableButtons()");
        btnSaveAndPrint.Attributes.Add("onclick", "DisableButtons()");
        
        string sQueryString = String.Format("StudentId={0}&StudentName={1}&Amount={2}&RegNo={3}&pIndex={4}", hidStudentId.Value, lblStudentHeading.Text, null, null, null);
        hlnkCustomReceipt.Attributes.Add("onclick", "if(!OpenReceiptPopup( 'CustomizeInternalRecieptPopUp.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");
        if (Settings.AutoPopulateInternalFeeRemark)
            hidRemark.Value = Constants.S_ONE;
        else
            hidRemark.Value = Constants.S_ZERO;
	}

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidAreYouSureYouWantToDeleteThisFeeDetails.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisFeeDetails;
        hidValOneFeeTypeSelected.Value = Resources.LocalizedResources.ValOneFeeTypeSelected;
        hidValAmountNotBlankOrZero.Value = Resources.LocalizedResources.ValAmountNotBlankOrZero;
        hidAmountShouldNotGreaterThan.Value = Resources.LocalizedResources.AmountShouldNotGreaterThan;
        hidPaymentDateShouldNotFutureDate.Value = Resources.LocalizedResources.PaymentDateShouldNotFutureDate;
    }

    /// <summary>
    /// This method is used to fill combobox with bank list.
    /// </summary>
    private void FillBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        ControlUtility.FillDropDownList(dtBankList, ref cmbBank, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);

        if (cmbBank.Items.Count > 0)
        {
            ListItem oListItem = cmbBank.Items.FindByValue(hidDefaultBankId.Value);
            if (oListItem != null)
                oListItem.Selected = true;
        }

        if (Settings.EnableAccountsModule)
        {
            BankAccountClient oBankClient = new BankAccountClient();
            try
            {
                oBankClient.Open();
                List<BankAccount> lstLedgers = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);
                ListSource.FillDropDownList(lstLedgers, cmbDepositInBank, "Name", "Id", Constants.S_SELECT);
                ListSource.FillDropDownList(lstLedgers, ddlDepositeInBank, "Name", "Id", Constants.S_SELECT);
                ControlUtility.FillDropDownList(dtBankList, ref ddlBankName, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);////new add


                var BankId = lstLedgers.Where(a => a.IsInternalDefault == true).FirstOrDefault();

                if (!BankId.IsNull())
                {
                    cmbDepositInBank.SelectedValue = BankId.Id.ToString();
                    hidDefaultDepositeBank.Value = BankId.Id.ToString();
                    ddlDepositeInBank.SelectedValue = BankId.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an exception fetching Bank Account details.");
            }
            finally
            {
                if (oBankClient != null && oBankClient.State != CommunicationState.Faulted)
                    oBankClient.Close();
            }
        }
    }

    /// <summary>
    /// this method is used to fill cheque details.
    /// </summary>
    private void FillChequeDetails()
    {
        optCash.Attributes.Add("onclick", "SetFeeType(1)");
        optCheque.Attributes.Add("onclick", "SetFeeType(2)");
        txtChequeDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        FillBankCombo();        
        optElectronic.Attributes.Add("onclick", "SetFeeType(3)");
        
        if (Settings.DefaultInternalFeeType.ToLower() == Constants.PaymentMode.Cash.ToString().ToLower())
        {
            optCash.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(1);", true);
        }
        else if (Settings.DefaultInternalFeeType.ToLower() == Constants.PaymentMode.Cheque.ToString().ToLower())
        {
            optCheque.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(2);", true);
        }
        else if (Settings.DefaultInternalFeeType.ToLower() == Constants.PaymentMode.Electronic.ToString().ToLower())
        {
            optElectronic.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFeeType", "SetFeeType(3);", true);
        }
    }
    private void ResetElectronicDetails()                            ///////////////////////////new add method
    {
        txtTransactionNo.Text = string.Empty;
        cmbElectronicTypes.ClearSelection();
        ddlBankName.ClearSelection();
        ddlDepositeInBank.ClearSelection();

    }

    private void FillElectronicPaymentTypes()
    {
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, 0, 0, 0);
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbElectronicTypes, "Type", "TypeId", Constants.S_SELECT);
    }

    /// <summary>
    /// Records the fee payment in the Accounts module.
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <param name="asReceiptNo"></param>
    private void RecordCashPayment(int aiStudentId, string asReceiptNo)
    {
        // Create a fee voucher for the fees paid by the student
        AccountVoucherClient oVoucherClient = new AccountVoucherClient();
        try
        {
            oVoucherClient.Open();
            oVoucherClient.CreateInternalFeeVoucherForCashPayment(miSchoolId, miAcademicYearId, miFinancialYearId, aiStudentId, asReceiptNo, miUserId);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), string.Format("Accounts Module : An exception occured while recording a fee payment. StudentId : {0}. ReceiptNo : {1}", aiStudentId, asReceiptNo));
        }
        finally
        {
            if (oVoucherClient.State != CommunicationState.Faulted)
                oVoucherClient.Close();
        }
    }

	#endregion -- PRIVATE METHOD(s) --
}
