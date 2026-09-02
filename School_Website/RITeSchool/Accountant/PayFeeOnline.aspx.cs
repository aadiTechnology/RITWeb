// File Name   : PayFeeOnline.aspx.cs
// Created By  : Milind
// Date        : 27 Nov 2009
// Description : This class is used to display the details (Actual Amount,Late Fee amount etc.)
//               of the fee which are selected on the parent screen.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Web;
using System.Linq;
using SchoolEntities.StudentFee;
using System.Configuration;

public partial class PayFeeOnline : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string TOTAL_AMOUNT = "Total Amount";
    private const string CONCESSION_AMOUNT = "Concession Amount";
    private const string LATE_FEE_AMOUNT = "LATE FEE AMOUNT";
    private const string S_SCHOOWISE_STUDENT_FEE_ID = "Schoolwise_Student_Fee_Id";    

    private StudentFeeDetailsBL moStudentFeeDetailsBL;

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// 	This event is used to fill all controls on the pop up according to query string.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, miUserId, miUserId);
				if (ReadQueryString())
				{
                    //int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
                    //if (iSchoolId == Constants.SchoolId.DYPV.ToInt())
                    //    trBankList.Visible = false;

					hlnkBankDetails.Attributes.Add("onclick", string.Format("window.open('{0}', '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=680'); return false;", hlnkBankDetails.NavigateUrl));
					if (hidIsForNextYear.Value == Constants.S_YES)
						InitializeFormForNextYear();
					else
						InitializeForm();
                    SetConcessionMessage();
                    DisplayLateFeeNote();
					ApplyMouseHoverEffect(new List<Button> { btnPay, btnClose });
                    FillStudentFeeListView();

                    if (hidRestrictStudentsFeePayment.Value == Constants.S_YES)
                        btnPay.Attributes.Add("onclick", "ShowPendingFeeAlert(); return false;");
                    else
                    {
                        btnPay_Click(btnPay, null);
                    }
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used to close the pop up and refresh parent screen.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
            if (hidIsOldAcademicYearPayment.Value == Constants.S_ONE)
            {
                ClearSessionVariables();
                Response.Write(string.Format("<script type=\"text/javascript\">window.close(); window.opener.focus();</script>"));                
                //Response.Write(string.Format("<script type=\"text/javascript\">window.opener.location.reload(); window.close(); window.opener.focus();</script>"));                
            }
            else if (hidIsFinalYear.Value == Constants.S_NO)
            {
                string sQueryString = SetQueryString();
                ClearSessionVariables();
                Response.Write(string.Format("<script type=\"text/javascript\">window.opener.location=window.opener.location.pathname+'?{0}';window.opener.focus(); window.close();</script>", sQueryString));
            }
            else
            {
                ClearSessionVariables();
                Response.Write("<script type=\"text/javascript\">window.close();window.opener.focus();</script>");
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This event is used redirect to the next (NetBankingUI.aspx) page.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnPay_Click(object sender, EventArgs e)
	{
		try
		{
            string sStudentFeeIds = string.Empty;
            if (Settings.EnablePartialFeePaymentForStudentLogin)
            {
                int iCOncessionAmount = 0;
                if (ViewState[CONCESSION_AMOUNT] != null && ViewState[CONCESSION_AMOUNT].ToString().Trim() != string.Empty)
                    iCOncessionAmount = ViewState[CONCESSION_AMOUNT].ToInt();

                int iLateFeeAmount = 0;
                if (ViewState[LATE_FEE_AMOUNT] != null && ViewState[LATE_FEE_AMOUNT].ToString().Trim() != string.Empty)
                    iLateFeeAmount = ViewState[LATE_FEE_AMOUNT].ToInt();

                if (hidPartialAmount.Value.ToInt() > Constants.I_ZERO)
                    ViewState[TOTAL_AMOUNT] = hidPartialAmount.Value.ToInt() + iLateFeeAmount - iCOncessionAmount;

                foreach (ListViewDataItem oListViewDataItem in lstvwStudentFee.Items)
                {
                    int iSchoolwiseStudentFeeId = lstvwStudentFee.DataKeys[oListViewDataItem.DisplayIndex]["SchoolwiseStudentFeeId"].ToInt();
                    TextBox txtActualAmount = oListViewDataItem.FindControl("txtActualAmount") as TextBox;

                    string sAmount = txtActualAmount.Text.Trim();

                    if (sAmount != Constants.S_ZERO)
                        sStudentFeeIds = sStudentFeeIds + ',' + iSchoolwiseStudentFeeId + "$" + sAmount;
                }

                if (sStudentFeeIds.StartsWith(","))
                    sStudentFeeIds = sStudentFeeIds.Substring(1);

                if (hidFinalRemark.Value != string.Empty)
                {
                    string sRemarkToStore = hidFinalRemark.Value;
                    if (sRemarkToStore.Length > 2000)
                        sRemarkToStore = sRemarkToStore.Substring(0, 1998) + "..";
                    Session["LateFeeRemarks"] = sRemarkToStore;
                }
                else
                {
                    string sRemarkToStore = txtRemarks.Text;
                    if (sRemarkToStore.Length > 2000)
                        sRemarkToStore = sRemarkToStore.Substring(0, 1998) + "..";
                    Session["LateFeeRemarks"] = sRemarkToStore;
                }
            }

            int iTotalAmt = ViewState[TOTAL_AMOUNT].ToInt();
            int iConcessionAmount = ViewState[CONCESSION_AMOUNT].ToInt();
            string sQueryString = string.Empty;
            if (hidIsCautionMoneyPayentOnline.Value == Constants.S_ONE)
                sQueryString = string.Format("From=CautionMoney&TotalAmount={0}&ConcessionAmount={1}&ScStudentId={2}", iTotalAmt, iConcessionAmount, hidStudentId.Value);
            else if (hidIsInternalFeePaymentOnline.Value == Constants.S_ONE)
                sQueryString = string.Format("From=InternalFee&TotalAmount={0}&ConcessionAmount={1}&AcademicYearId={2}", iTotalAmt, iConcessionAmount,hidAcademicYrId.Value);
            else
                sQueryString = string.Format("From=StudentFee&TotalAmount={0}&ConcessionAmount={1}&IsOldAcademicYearPayment={2}&SchoolwiseStudentFeeIds={3}&SelectedFeeType={4}&ScStudentId={5}&NxtAcYearId={6}", iTotalAmt, iConcessionAmount, hidIsOldAcademicYearPayment.Value, sStudentFeeIds, hidSelectedFeeType.Value, hidStudentId.Value, hidAcademicYrId.Value);
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
            string sUrl = "~/RITeSchool/PaymentConfirmationUI.Aspx?" + sEncrypt;
            Response.Redirect(sUrl, false);
		}
        catch (System.Threading.ThreadAbortException)
        { }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    protected void lstvwStudentFee_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {              
                Label lblAmount = oCurrentItem.FindControl("lblAmount") as Label;
                TextBox txtActualAmount = oCurrentItem.FindControl("txtActualAmount") as TextBox;
                txtActualAmount.Attributes.Add("onblur", "CalculateActualAmt(this,'" + iRowId + "')");
                txtActualAmount.Text = lblAmount.Text;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	private string SetQueryString()
	{
		string sEncryptQueryString = string.Empty;
		if (Session["IsForNextYear"] != null && Session["IsForNextYear"].ToString() == Constants.S_YES)
		{
			string sQueryString = String.Format("StudentId={0}&Academic_Year_ID={1}&StandardID={2}", Session["NewStudentID"], Session["NewAcademicYearID"], Session["NewStandardID"]);
			sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
		}
		return sEncryptQueryString;
	}

	private void ClearSessionVariables()
	{
		Session["NewStudentID"] = null;
		Session["NewStandardID"] = null;
		Session["NewAcademicYearID"] = null;
		Session["IsForNextYear"] = null;
		Session["FinalAcademicYearId"] = null;
		Session["FinalYearStudentId"] = null;
        Session["IsOldAcademicYearPayment"] = null;
	}

	/// <summary>
	/// 	This method is used to read querystring.
	/// </summary>
	private bool ReadQueryString()
	{
        if (!QueryString["InternalFeeDetailsId"].IsNull())
        {            
             hidInternalFeeDetailsID.Value = QueryString["InternalFeeDetailsId"];

             if (!QueryString["StudentId"].IsNull())
                 hidStudentId.Value = QueryString["StudentId"];

             if (!QueryString["InternalFeeDetailsId"].IsNull())
                 hidInternalFeeDetailsID.Value = QueryString["InternalFeeDetailsId"];

             if (!QueryString["IsOnlineInternalFeePayment"].IsNull())
             {
                 hidIsInternalFeePaymentOnline.Value = QueryString["IsOnlineInternalFeePayment"].ToString();
                 Session["IsInternalFeePayment"] = Constants.S_YES;
             }
             else
                 hidIsInternalFeePaymentOnline.Value = Constants.S_ZERO;

            if (!QueryString["IsForNextYear"].IsNull())
            {
                hidIsForNextYear.Value = QueryString["IsForNextYear"];
                Session["IsForNextYear"] = QueryString["IsForNextYear"];
            }
            else
                Session["IsForNextYear"] = null;

            if (!QueryString["TotalAmount"].IsNull())
            {
                hidTotalAmount.Value = QueryString["TotalAmount"];
                ViewState[TOTAL_AMOUNT] = (QueryString["TotalAmount"].ToInt() + hidLateFeeAmount.Value.ToInt()).ToString();
            }
            else
                ViewState[TOTAL_AMOUNT] = Constants.S_ZERO;

            if (!QueryString["AcadmicYearId"].IsNull())
                hidAcademicYrId.Value = QueryString["AcadmicYearId"];
            else
                hidAcademicYrId.Value = Constants.S_ZERO;
        }
        else
        {
          Session["IsInternalFeePayment"] = Constants.S_NO;          
          if (!QueryString["DueDates"].IsNull())
            {
                hidDuedates.Value = QueryString["DueDates"];

                if (!QueryString["StudentId"].IsNull())
                    hidStudentId.Value = QueryString["StudentId"];

                if (!QueryString["DueDates"].IsNull())
                    hidDuedates.Value = QueryString["DueDates"];

                if (!QueryString["Remarks"].IsNull())
                    hidRemarks.Value = QueryString["Remarks"];

                if (!QueryString["AcadmicYearId"].IsNull())
                    hidAcademicYrId.Value = QueryString["AcadmicYearId"];
                else
                    hidAcademicYrId.Value = Constants.S_ZERO;

                if (!QueryString["StanardID"].IsNull())
                    hidStandard.Value = QueryString["StanardID"];

                if (!QueryString["IsForNextYear"].IsNull())
                    hidIsForNextYear.Value = QueryString["IsForNextYear"];

                if (!QueryString["LateFeeAmount"].IsNull())
                    hidLateFeeAmount.Value = QueryString["LateFeeAmount"];

                if (!QueryString["SchoolwiseStudentFeeId"].IsNull() && QueryString["SchoolwiseStudentFeeId"].ToString().Trim() != string.Empty)
                    hidSchoolwiseStudentFeeId.Value = QueryString["SchoolwiseStudentFeeId"];
                else
                    hidSchoolwiseStudentFeeId.Value = Constants.S_ZERO;

                if (!QueryString["IsFinalYear"].IsNull())
                {
                    hidIsFinalYear.Value = QueryString["IsFinalYear"];
                    Session["FinalAcademicYearId"] = hidAcademicYrId.Value.ToInt();
                    Session["FinalYearStudentId"] = hidStudentId.Value.ToInt();
                }

                if (!QueryString["TotalAmount"].IsNull())
                {
                    hidTotalAmount.Value = QueryString["TotalAmount"];
                    ViewState[TOTAL_AMOUNT] = (QueryString["TotalAmount"].ToInt() + hidLateFeeAmount.Value.ToInt()).ToString();
                }

                if (!QueryString["IsOnlineCautionMoneyPayment"].IsNull())
                    hidIsCautionMoneyPayentOnline.Value = QueryString["IsOnlineCautionMoneyPayment"].ToString();
                else
                    hidIsCautionMoneyPayentOnline.Value = Constants.S_ZERO;

                if (QueryString["IsOldAcademicYearPayment"] != null)
                    hidIsOldAcademicYearPayment.Value = QueryString["IsOldAcademicYearPayment"].ToString();
                else
                    hidIsOldAcademicYearPayment.Value = Constants.S_ZERO;

                if (QueryString["ConcessionAmount"] != null)
                    hidConcessionAmount.Value = QueryString["ConcessionAmount"];
                else
                    hidConcessionAmount.Value = Constants.S_ZERO;

                if (QueryString["FeeType"] != null)
                    hidSelectedFeeType.Value = QueryString["FeeType"];
                else
                    hidSelectedFeeType.Value = string.Empty;
            }
            else
            {
                Response.Write("<script>window.close();</script>");
                return false;
            }          
        }
		return true;
	}

	private void InitializeFormForNextYear()
	{
        Session["PendingFeeTransactionId"] = null;
		txtAmountTobePaid.Text = ViewState[TOTAL_AMOUNT].ToString();
		txtPayableAmt.Text = (ViewState[TOTAL_AMOUNT].ToString().ToInt() - hidLateFeeAmount.Value.ToInt()).ToString();
		txtLateFeeAmt.Text = hidLateFeeAmount.Value;
		cal_PaymentDate.DateValue = DateTime.Today;
		txtRemarks.Text = hidRemarks.Value;

        hidActualAmount.Value = txtPayableAmt.Text;

        var oStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, hidAcademicYrId.Value.ToInt(), hidStudentId.Value.ToInt(), miUserId);
        decimal dcConcessionAmount = oStudentFeeDetailsBL.GetFullPaymentConcessionAmount(txtPayableAmt.Text.ToInt(), false, hidStandard.Value.ToInt());
        if (dcConcessionAmount != 0 || hidConcessionAmount.Value != "0")
        {
            if (dcConcessionAmount == 0)
                dcConcessionAmount = hidConcessionAmount.Value.ToInt();

            txtConcessionAmount.Text = dcConcessionAmount.ToString();
            trConcession.Visible = true;
            txtAmountTobePaid.Text = (Convert.ToInt32(txtAmountTobePaid.Text) - dcConcessionAmount.ToInt()).ToString();
            ViewState[TOTAL_AMOUNT] = txtAmountTobePaid.Text;
            ViewState[LATE_FEE_AMOUNT] = (txtLateFeeAmt.Text == string.Empty ? Constants.S_ZERO : txtLateFeeAmt.Text);

            txtRemarks.Text = txtRemarks.Text + " with  Concession Fee (Concession Fee - Rs. " + dcConcessionAmount + "/-)";
            hidRemarks.Value = txtRemarks.Text;

            ViewState[CONCESSION_AMOUNT] = dcConcessionAmount;
        }

        string sRemarkForSession = hidRemarks.Value + ".";
        if (sRemarkForSession.Length > 2000)
            sRemarkForSession = sRemarkForSession.Substring(0, 1998) + "..";
		Session["LateFeeRemarks"] = sRemarkForSession;
		Session["DueDates"] = hidDuedates.Value;
		Session["IsForNextYear"] = hidIsForNextYear.Value;
		Session["NewStudentID"] = hidStudentId.Value;
		Session["NewStandardID"] = hidStandard.Value;
		Session["NewAcademicYearID"] = hidAcademicYrId.Value;
		Session["LateFeeAmount"] = hidLateFeeAmount.Value;
        Session["InternalFeeDetailsId"] = hidInternalFeeDetailsID.Value;        
	}

	/// <summary>
	/// 	This method is used to initialize controls on the page.
	/// </summary>
	private void InitializeForm()
	{
		cal_PaymentDate.DateValue = DateTime.Today;
		FillControlsOnForm();
	}

	/// <summary>
	/// 	This method is used to fill the controls on the screen and assign the value to the Session variables which are needed for paying fee online.
	/// </summary>
	private void FillControlsOnForm()
	{
        Session["PendingFeeTransactionId"] = null;
		const int I_DISPLAYED_DETAILS_TABLE = 0;
		const int I_LATE_FEE_DETAILS_TABLE = 1;
		const int I_REMARKS_TABLE = 2;

        int iAcdYrId = ((string.IsNullOrEmpty(hidAcademicYrId.Value.Trim()) || hidAcademicYrId.Value == Constants.S_ZERO) ? miAcademicYearId : hidAcademicYrId.Value.ToInt());
		int iStudentID = (string.IsNullOrEmpty(hidStudentId.Value) ? Session[Constants.S_SESSION_STUDENT_ID].ToInt() : hidStudentId.Value.ToInt());
		string asDueDatesFilterXML = GetXMLForDueDates();

		var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataSet oDSFeeDeatils = oStudentFeeDetailsBL.GetFeeDetailsForOnlineFee(miSchoolId, iAcdYrId, iStudentID, asDueDatesFilterXML, hidSchoolwiseStudentFeeId.Value.ToInt(), hidIsCautionMoneyPayentOnline.Value.ToInt().ToBool(), hidIsInternalFeePaymentOnline.Value.ToInt().ToBool());

		//First table contains details related to actual amount.
		DataTable oDataTable = oDSFeeDeatils.Tables[I_DISPLAYED_DETAILS_TABLE];
        int iPayableAmount = 0;
        if (oDataTable.Rows[0]["Amount"] != DBNull.Value)
            iPayableAmount = oDataTable.Rows[0]["Amount"].ToInt();

        txtPayableAmt.Text = iPayableAmount.ToString();
        hidActualAmount.Value = iPayableAmount.ToString();

        int iConcessionAmount = 0;
        if (oDataTable.Rows[0]["ConcessionAmount"] != DBNull.Value)
            iConcessionAmount = Convert.ToInt32(oDataTable.Rows[0]["ConcessionAmount"]);
        txtConcessionAmount.Text = iConcessionAmount.ToString();
        ViewState[CONCESSION_AMOUNT] = txtConcessionAmount.Text;
        
        if (!string.IsNullOrEmpty(txtConcessionAmount.Text) && Convert.ToInt32(txtConcessionAmount.Text) != 0)
            trConcession.Visible = true;

		//Second table contains details of late fee.
		DataTable oDtLateFeeDeatils = oDSFeeDeatils.Tables[I_LATE_FEE_DETAILS_TABLE];
		string sLateFeeDetails = "";
		int iLateFee = 0;
		int iMaxFee = Settings.MaxFee;
		for (int iCount = 0; iCount < oDtLateFeeDeatils.Rows.Count; iCount++)
		{
			sLateFeeDetails = sLateFeeDetails + " + " + oDtLateFeeDeatils.Rows[iCount]["LateFeeAmount"];
			iLateFee += oDtLateFeeDeatils.Rows[iCount]["LateFeeAmount"].ToInt();
			if (iLateFee > iMaxFee)
			{
				if (Settings.IsMaxFeeApplicable)
				{
					trNote.Visible = true;
					lblVerifyNote.Text = Settings.MaxFeeNote;
					iLateFee = iMaxFee;
					break;
				}
			}
		}

		txtLateFeeAmt.Text = iLateFee.ToString();
        txtAmountTobePaid.Text = (iPayableAmount + iLateFee - iConcessionAmount).ToString();
		ViewState[TOTAL_AMOUNT] = txtAmountTobePaid.Text;
        ViewState[LATE_FEE_AMOUNT] = (txtLateFeeAmt.Text == string.Empty ? Constants.S_ZERO : txtLateFeeAmt.Text);

        if (iPayableAmount == 0 && iLateFee == 0)
        {
            btnPay.Enabled = false;
            return;
        }

		//Third table contains details of all payable fee such as Schoolwise_Student_Fee_Id,Fee Type,Payble For etc. which 
		//are used to display the remarks as well as for maintaining the details for saving the details for fee.    
		DataTable oMainTable = oDSFeeDeatils.Tables[I_REMARKS_TABLE];
		string sRemarks = "";
		string sLateFeeRemarks = "";

		//List<int> is maintained the Schoolwise_Student_Fee_Id of the student fees.
		var oArrSchoolStudId = new List<int>();
		for (int iCount = 0; iCount < oMainTable.Rows.Count; iCount++)
		{
			sRemarks = string.Format("{0}, {1} ({2} - Rs. {3} /-)", sRemarks, oMainTable.Rows[iCount]["Payable_For"], oMainTable.Rows[iCount]["Fee_Type"], oMainTable.Rows[iCount]["Amount"]);
			oArrSchoolStudId.Add(oMainTable.Rows[iCount]["Schoolwise_Student_Fee_Id"].ToInt());
			if (oMainTable.Rows[iCount]["LateFeeAmount"].ToInt() > 0)
				sLateFeeRemarks += ", " + oMainTable.Rows[iCount]["Payable_For"];
		}

        if (oMainTable.Rows[0]["RestrictStudentFeePayment"] != null && oMainTable.Rows[0]["RestrictStudentFeePayment"].ToBool() == true)
            hidRestrictStudentsFeePayment.Value = Constants.S_YES;

		Session["Schoolwise_Student_Fee_Id"] = oArrSchoolStudId;

		//sRemarks = sRemarks.Trim();
		if (sRemarks.StartsWith(","))
			sRemarks = sRemarks.Substring(1).Trim();

		//if late fee is not zero then display the distinct amount in the label.
		//And details of that late in the remarks.        
		if (iLateFee > 0)
		{
			sLateFeeDetails = sLateFeeDetails.Substring(3);
			lblLateFeeDetails.Visible = true;
			lblLateFeeDetails.Text = String.Format("({0}) ", sLateFeeDetails);

			sLateFeeRemarks = sLateFeeRemarks.Substring(1).Trim();
			sLateFeeRemarks = String.Format("Late fee for {0}", sLateFeeRemarks);
            string sConcessionFeeRemark = string.Empty;
            if (iConcessionAmount > 0)
                sConcessionFeeRemark = String.Format("with Concession Fee (Concession Fee - Rs. {0}/-)", iConcessionAmount);
			string sFeeRemarks = String.Format("Amount paid for {0} {1} & {2}", sRemarks, sConcessionFeeRemark, sLateFeeRemarks);
            if (sFeeRemarks.Length > 2000)
                sFeeRemarks = sFeeRemarks.Substring(0, 1998) + "..";
			Session["LateFeeRemarks"] = sFeeRemarks;
			txtRemarks.Text = String.Format("{0} (Rs. {1}/-)", sFeeRemarks, iLateFee);            
		}
		else
		{
			txtRemarks.Text = String.Format("Amount paid for {0}.", sRemarks);
            if (iConcessionAmount > 0)
                txtRemarks.Text = String.Format("Amount paid for {0} with  Concession Fee (Concession Fee - Rs. {1}/-).", sRemarks, iConcessionAmount);
            string sRemarkToStore = txtRemarks.Text;
            if (sRemarkToStore.Length > 2000)
                sRemarkToStore = sRemarkToStore.Substring(0, 1998) + "..";
			Session["LateFeeRemarks"] = sRemarkToStore;
		}
		Session["LateFeeAmount"] = iLateFee;
        if (Convert.ToBoolean(HttpContext.Current.Session[Constants.S_SESSION_IS_LOGIN_FROM_MOBILE]))
        {
            txtPayableAmt.Attributes.Add("style", "color:#000 !important;");
            txtLateFeeAmt.Attributes.Add("style", "color:#000 !important;");
            txtAmountTobePaid.Attributes.Add("style", "color:#000 !important;");
            txtPaymentDate.Attributes.Add("style", "color:#000 !important;");
            btnClose.Visible = false;
        }

        if (oDSFeeDeatils != null && oDSFeeDeatils.Tables.Count == 4 && oDSFeeDeatils.Tables[3].Rows[0][0] != DBNull.Value && oDSFeeDeatils.Tables[3].Rows[0][0].ToString() != string.Empty)
        {
            trErrMessage.Visible = true;
            lblErrMessage.Text = oDSFeeDeatils.Tables[3].Rows[0][0].ToString();
            btnPay.Visible = false;
        }
	}

	/// <summary>
	/// 	Generate XML for the Due Dates.
	/// </summary>
	/// <returns> </returns>
	private string GetXMLForDueDates()
	{
		const string S_ELEMENT = "element";
		var oDoc = new XmlDocument();

        if (hidIsInternalFeePaymentOnline.Value == Constants.S_ONE)
        {
            // Create a root level element.
            XmlElement root = oDoc.CreateElement("DueDates");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DueDates", "");

            string[] sArrInternalFeeDetailsId = hidInternalFeeDetailsID.Value.Split(',');
            // Loop through all the list view items.
            foreach (string sInternalFeeId in sArrInternalFeeDetailsId)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "DueDates", "");

                string sAtrrName = "InternalFeeDetailsId";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sInternalFeeId;

                oXmlNode.Attributes.Append(attr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }
        else
        {
            // Create a root level element.
            XmlElement root = oDoc.CreateElement("DueDates");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DueDates", "");

            string[] sArrInternalFeeDetailsId = hidInternalFeeDetailsID.Value.Split(',');
            string[] sArrDueDates = hidDuedates.Value.Split(',');
            // Loop through all the list view items.
            foreach (string sDueDate in sArrDueDates)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "DueDates", "");

                string sAtrrName = "DueDate";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sDueDate;

                oXmlNode.Attributes.Append(attr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }
	}

    /// <summary>
    /// This method is used to set concession.
    /// </summary>
    private void SetConcessionMessage()
    {
        if (moUserRole == Constants.UserRoles.Student)
        {
            int iStandardId = (string.IsNullOrEmpty(hidStandard.Value.Trim()) ? Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt() : hidStandard.Value.ToInt());
            int iAcademicYearId = ((string.IsNullOrEmpty(hidAcademicYrId.Value.Trim()) || hidAcademicYrId.Value == Constants.S_ZERO) ? miAcademicYearId : hidAcademicYrId.Value.ToInt());
            StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, iAcademicYearId, hidStudentId.Value.ToInt(), miUserId);
            string sConcessionMessage = moStudentFeeDetailsBL.GetConcessionMessage(iStandardId, true);
            if (!string.IsNullOrEmpty(sConcessionMessage))
            {
                trConcesionMessage.Visible = true;
                lblConcessionMessage.Text = sConcessionMessage;
            }
            else
            {
                lblConcessionMessage.Text = string.Empty;
                trConcesionMessage.Visible = false;
            }
        }
    }

    /// <summary>
    /// This method is used to display late fee note.
    /// </summary>
    private void DisplayLateFeeNote()
    {
        if (miSchoolId == 11 && moUserRole == Constants.UserRoles.Student && SchoolBase.Settings.DisplayLateFeeNote == true)
        {
            trNotePPSHStudent.Visible = true;
        }
    }

    private void FillStudentFeeListView()
    {
        hidIsPartialFeePaymentEnabled.Value = Settings.EnablePartialFeePaymentForStudentLogin.ToString();
        if (Settings.EnablePartialFeePaymentForStudentLogin)
        {
            hidMinimumPartialAmount.Value = Settings.MinimumPartialAmountForOnline.ToString();            

            trFeeDetailsPPSH.Visible = true;
            DateTime dtCurrentDate = DateTime.Now;
            if (DateTime.TryParse(txtPaymentDate.Text, out dtCurrentDate))
                dtCurrentDate = txtPaymentDate.Text.ToDateTime();

            string asDueDatesFilterXML = GetXMLForDueDates();

            int iAcademicYearId = ((string.IsNullOrEmpty(hidAcademicYrId.Value.Trim()) || hidAcademicYrId.Value == Constants.S_ZERO) ? miAcademicYearId : hidAcademicYrId.Value.ToInt());
            List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = moStudentFeeDetailsBL.GetStudentFeeDetailsForOnlinePartialPayment(asDueDatesFilterXML, dtCurrentDate, hidStudentId.Value.ToInt(), iAcademicYearId);
            List<StudentPayFeeDetails> lstStudentPayFeeDetails = moStudentFeeDetailsBL.StudentPayFeeDetails;
            var oStudentDebitDetails = from Paid in lstStudentPaidFeeDetails
                                       join Pay in lstStudentPayFeeDetails
                                       on Paid.SchoolwiseStudentFeeId equals Pay.SchoolwiseStudentFeeId
                                       select new
                                       {
                                           Paid.SchoolwiseStudentFeeId,
                                           Paid.PayableFor,
                                           Paid.Amount,
                                           Paid.FeeType,
                                           Paid.AmountPayable,
                                           Paid.DebitOrCredit,
                                           Paid.LateFeeAmount,
                                           Paid.SerialNumber,
                                           Paid.StandardwiseFeeTypeId,
                                           Pay.PaymentDate,
                                           Pay.ReceiptNumberOutput,
                                           Paid.ConcessionAmount,
                                           Paid.AccountHeaderId
                                       };

            lstvwStudentFee.DataSource = oStudentDebitDetails;
            lstvwStudentFee.DataBind();

            int iTotalAmount = (from Paid in lstStudentPaidFeeDetails
                       join Pay in lstStudentPayFeeDetails
                       on Paid.SchoolwiseStudentFeeId equals Pay.SchoolwiseStudentFeeId
                       select Paid.Amount).Sum();

            hidPartialAmount.Value = iTotalAmount.ToString();

        }
        else
            trFeeDetailsPPSH.Visible = false;
    }

	#endregion -- PRIVATE METHOD(s) --
}