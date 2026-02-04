using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities.StudentFee;

public partial class PayFeeForNextAcaYear : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_FEE_DETAILS = "FeeDetails";
    private const string S_TXN_EXISTS_MESSAGE = "Txn Number already exists for this student. Please enter another Txn number.";    
    private const string S_CHEQUE_NUMBER_EXISTS_MESSAGE = "Cheque Number already exists for this student. Please enter another Txn number.";
    private const string S_DELETE_SUCCESS = "Fee payment deleted successfully!!!";
    private const string S_SAVED_SUCCESS = "Fee payment saved  successfully!!!";
    private const string S_FAILED_TO_SAVE = "Failed to save fee payment.";

	#endregion -- CONSTANT(s) --

    #region -- DATA MEMBER(s) --

    private StudentFeeDetailsBL moStudentFeeDetailsBL;

    #endregion

    #region -- MEMBER(s) --

    private int miTotalAmount;
	private DateTime dtDueDate;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{           
            ReadQueryString();
            moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, hidAcadmicYear.Value.ToInt(), hidStudentId.Value.ToInt(), miUserId);
			if (!IsPostBack)
			{
				if (hidIsFInalYear.Value == "True")
				{
					lblHeader.Text = "Paid Fees Details";
					lstvwPaidDetails.Visible = true;
					lstvwPayFee.Visible = false;
					DisplayFeeDetails();
					tblPaymentDetail.Visible = false;
					trNote.Visible = false;
					btnPay.Visible = false;
					btnPayPrint.Visible = false;
					SetJavaScriptsAttributes();
				}
				else
				{
					lstvwPaidDetails.Visible = false;
					lstvwPayFee.Visible = true;
					FillFeeListview();
					Initialize();
					SetJavaScriptsAttributes();
				}

				if (moUserRole == Constants.UserRoles.Student && Settings.EnabledOnlineFee)
				{
                    //if (hidIsFInalYear.Value == "True")
                    //    btnPayOnline.Visible = false;
                    //else
                    
                    btnPayOnline.Visible = true;
					trNote.Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void optCheque_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (optCheque.Checked)
				lblErrMsg.Text = string.Empty;
			EnableDisabledChequeControls(true);
			EnableDisabledBankCombo(true);
			EnableDisabledCardControls(false);
            DisplayElectronicControls(false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void optCash_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (optCash.Checked)
			{
				lblErrMsg.Text = string.Empty;
				txtChequeNumber.Text = string.Empty;
			}
			EnableDisabledChequeControls(false);
			EnableDisabledBankCombo(false);
			EnableDisabledCardControls(false);
            DisplayElectronicControls(false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void optCard_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (optCard.Checked)
			{
				lblErrMsg.Text = string.Empty;
				txtChequeNumber.Text = string.Empty;
				txtSwapNumber.Focus();
                lblCardType.Visible = true;
                ddlCardType.Visible = true;
				EnableDisabledCardControls(true);
                DisplayElectronicControls(false);
			}
			EnableDisabledChequeControls(false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to handle the electronic payment checked event on click of it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optElectronic_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optElectronic.Checked)
            {
                lblErrMsg.Text = string.Empty;                
                txtChequeNumber.Text = string.Empty;
                txtSwapNumber.Focus();
                EnableDisabledCardControls(true);                
			    EnableDisabledChequeControls(false);
                DisplayElectronicControls(true);
                lblCardType.Visible = false;
                ddlCardType.Visible = false;
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	protected void btnPay_Click(object sender, EventArgs e)
	{
		try
		{
            moStudentFeeDetailsBL.StudentPayFeeDetail = new StudentPayFeeDetails
            {
                PaymentDate= txtDate.Text.Trim().ToDateTime(),
                Remarks = txtRemarks.Text.Trim()
            };

            if (txtConcessionAmount.Text == string.Empty)
                txtConcessionAmount.Text = Constants.S_ZERO;

			string sFeeDetailsXML = GeneratePayFeeXML();
			int iLateAmount = txtLateFeeAmount.Text.Trim().ToInt();
            int iConcessionAmount = txtConcessionAmount.Text.ToInt();
			int iSerialNo = 0;
            if (optCheque.Checked)
            {
                if (moStudentFeeDetailsBL.IsDuplicateChequeNumber(ddlBankName.SelectedValue.ToInt(), txtChequeNumber.Text.Trim().ToInt()))
                {
                    FillFeeListview();
                    SetFields(true, S_CHEQUE_NUMBER_EXISTS_MESSAGE);
                }
                else
                {
                    SetFields(false, string.Empty);
                    string sChequeDetailsXML = GetChequeDetailsXML();
                    moStudentFeeDetailsBL.InsertStudentFeeDetailsForNextYear(sFeeDetailsXML, sChequeDetailsXML, iLateAmount, Constants.PaymentMode.Cheque.ToInt(), out iSerialNo, iConcessionAmount);
                }
            }
            else if (optCard.Checked)
                PayFeeWithCard(iLateAmount, sFeeDetailsXML, out iSerialNo);
            else if (optElectronic.Checked)
                PayFeeWithElectronic(iLateAmount, sFeeDetailsXML, out iSerialNo);
            else
                moStudentFeeDetailsBL.InsertStudentFeeDetailsForNextYear(sFeeDetailsXML, iLateAmount, out iSerialNo, iConcessionAmount);

			if (lblErrMsg.Text == string.Empty)
			{
				FillFeeListview();
				Initialize();
				ResetControls();
				EnableDisabledChequeControls(true);
                SetDefaultPaymentType();
			}
            if (sender == btnPayPrint && lblErrMsg.Text == string.Empty)
			{
				string sQueryString = "SerialNo=" + iSerialNo.ToString();
				sQueryString += "&NewAcdYear=" + true.ToString();
				string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);

				ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Pay", "window.open('../Accountant/FeesMiniReceipt.aspx?" + sEncrypt + "','_blank','left=0, top=0, height=550, width=750, status=no, resizable= no, scrollbars= yes')", true);
			}

            if (lblErrMsg.Text == string.Empty)
            {
                trSuccessMsg.Visible = true;
                lblUpdateMessage.Text = S_SAVED_SUCCESS;
            }
		}
		catch (Exception ex)
		{
            trlblErrMsg.Visible = true;
            lblErrMsg.Visible = true;
            lblErrMsg.Text = S_FAILED_TO_SAVE;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}    

	protected void lstvwPayFee_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iRowId = oCurrentItem.DisplayIndex;
				int iSerialNo = lstvwPayFee.DataKeys[iRowId]["SerialNo"].ToInt();
				var imgDelete = e.Item.FindControl("imgbtnDeleteItem") as ImageButton;
				var chkPayRow = e.Item.FindControl("chkPay") as CheckBox;
				var oHyperLinkField = (e.Item.FindControl("lnkMini")) as HyperLink;
				var oHtmlTableCell = e.Item.FindControl("tdDelete") as HtmlTableCell;
			    var tdActualAmount = e.Item.FindControl("tdActualAmount") as HtmlTableCell;
				var oHtmlTableCellChkAll = e.Item.FindControl("tdchkPay") as HtmlTableCell;
				var lblActualAmount = e.Item.FindControl("lblActualAmount") as Label;
				var txtActualAmount = e.Item.FindControl("txtActualAmount") as TextBox;
                HiddenField hidConcessionAmount = e.Item.FindControl("hidConcessionAmount") as HiddenField;

				if (iSerialNo != Constants.I_ZERO)
				{
					chkPayRow.Visible = false;
					lblActualAmount.Visible = true;
					txtActualAmount.Visible = false;

					if (iSerialNo != -9999)
					{
						imgDelete.Visible = true;
						oHyperLinkField.Visible = true;
						string sQueryString = "SerialNo=" + iSerialNo.ToString();
						sQueryString += "&NewAcdYear=" + true.ToString();
						string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
						oHyperLinkField.Visible = true;
						oHyperLinkField.NavigateUrl = oHyperLinkField.NavigateUrl + sEncrypt;
						oHyperLinkField.Attributes.Add("onclick", "window.open('" + oHyperLinkField.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
						imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
					}
					else
					{
						imgDelete.Visible = false;
						oHyperLinkField.Visible = false;
					}
				}
				else
				{
					imgDelete.Visible = false;
					oHyperLinkField.Visible = false;
				}

				if (moUserRole == Constants.UserRoles.Student)
				{
                    if(miSchoolId != Constants.SchoolId.SNS.ToInt())
					    chkPayRow.Visible = false;

				    tdActualAmount.Visible = false;
					DateTime dPaidDate = lstvwPayFee.DataKeys[iRowId]["PaidDate"].ToDateTime();
					if (dtDueDate != dPaidDate && iSerialNo == 0)
					{
						dtDueDate = dPaidDate;
						chkPayRow.Visible = true;
					}
					if (!Settings.EnabledOnlineFee)
						oHtmlTableCellChkAll.Visible = false;

					oHtmlTableCell.Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void lstvwPayFee_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName == "Remove")
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iRowId = oCurrentItem.DisplayIndex;
				int iSerialNo = lstvwPayFee.DataKeys[iRowId]["SerialNo"].ToInt();

				var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
				oStudentFeeDetailsBL.DeleteFeeDetailsForNextYear(iSerialNo, miUserId);

				FillFeeListview();
                lblErrMsg.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                trSuccessMsg.Visible = true;
                lblUpdateMessage.Text = S_DELETE_SUCCESS;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void lstvwPayFee_DataBound(object sender, EventArgs e)
	{
		try
		{
			var oHtmlTableHeaderRow = lstvwPayFee.FindControl("trHeader") as HtmlTableRow;
            if (oHtmlTableHeaderRow.IsNull())
                return;
			var oHtmlTableCell = oHtmlTableHeaderRow.FindControl("thDelete") as HtmlTableCell;
            var thActualAmount = oHtmlTableHeaderRow.FindControl("thActualAmount") as HtmlTableCell;
			var oHtmlTableCellChkAll = oHtmlTableHeaderRow.FindControl("thchkPay") as HtmlTableCell;
			var chkHeader = oHtmlTableHeaderRow.FindControl("chkAll") as CheckBox;
			btnPayOnline.Attributes.Add("Onclick", string.Format("if(!CheckAtleastOneCheckBoxForNextYear('{0}', {1})){{return false;}}", lstvwPayFee.ClientID, lstvwPayFee.Items.Count));
			hidRowCount.Value = lstvwPayFee.Items.Count.ToString();

			if (moUserRole == Constants.UserRoles.Student)
			{
				chkHeader.Visible = false;
				oHtmlTableCell.Visible = false;
				btnPay.Visible = false;
				btnPayPrint.Visible = false;
				tblPaymentDetail.Visible = false;
				trNote.Visible = false;
			    thActualAmount.Visible = false;
				if (Settings.EnabledOnlineFee)
				{
					btnPayOnline.Visible = true;
					oHtmlTableCellChkAll.Visible = true;
				}
				else
				{
					oHtmlTableCellChkAll.Visible = false;
				}
			}
			else
			{
				btnPayOnline.Visible = false;
				oHtmlTableCell.Visible = true;
				chkHeader.Visible = true;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void lstvwPaidDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{               
				var oCurrentItem = e.Item as ListViewDataItem;
				int iRowId = oCurrentItem.DisplayIndex;
				int iReceiptNumber = lstvwPaidDetails.DataKeys[iRowId]["Receipt_Number"].ToInt();
				int iNextYearId = lstvwPaidDetails.DataKeys[iRowId]["NextAcademicYear"].ToInt();
				var oHyperLinkField = e.Item.FindControl("lnkMini") as HyperLink;
				var chkPayRow = e.Item.FindControl("chkPay") as CheckBox;                
                var oHtmlTableTdchkAll = e.Item.FindControl("tdchkPay") as HtmlTableCell;

				if (hidIsFInalYear.Value == "True")
				{
					if (iReceiptNumber != Constants.I_ZERO)
					{
						oHyperLinkField.Visible = true;
						string sQueryString = String.Format("ReceiptNo={0}&AcademicYear={1}", iReceiptNumber.ToString(), iNextYearId.ToString());
						string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
						oHyperLinkField.Visible = true;
						oHyperLinkField.NavigateUrl = oHyperLinkField.NavigateUrl + sEncrypt;
						oHyperLinkField.Attributes.Add("onclick", "window.open('" + oHyperLinkField.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
					}
					else
						oHyperLinkField.Visible = false;

					chkPayRow.Visible = false;
					DateTime dPaidDate = lstvwPaidDetails.DataKeys[iRowId]["PaidDate"].ToDateTime();
					if (dtDueDate != dPaidDate && iReceiptNumber == 0)
					{
						dtDueDate = dPaidDate;
						chkPayRow.Visible = true;
					}

                    //oHtmlTableTdchkAll.Visible = moUserRole != Constants.UserRoles.Student;
				}
			}			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			SetQueryString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnPayOnline_Click(object sender, EventArgs e)
	{
		try
		{
			btnPayOnline.Enabled = false;
			string sQueryString = String.Empty;
			string sRemarks = String.Empty;
			string sLateemarks = String.Empty;
			string sDueDates = String.Empty;
			var dDueDate = new DateTime();
			int iAmount = 0;
			int iLateFeeAmount = 0;
            int iConcessionAmt = 0;
            string sFeeType = string.Empty;

			if (!lstvwPaidDetails.Visible)
			{
				foreach (ListViewDataItem oListViewDataItem in lstvwPayFee.Items)
				{
					int iRowId = oListViewDataItem.DataItemIndex;
					var oCheckBox = oListViewDataItem.FindControl("chkPay") as CheckBox;
					var olblDueDate = oListViewDataItem.FindControl("lblPaidDate") as Label;
					var olblPaybleFor = oListViewDataItem.FindControl("lblPaybleFor") as Label;
					var olblAmount = oListViewDataItem.FindControl("lblAmount") as Label;
					var olblLateFee = oListViewDataItem.FindControl("lblLateFee") as Label;
                    var olblFeeType = oListViewDataItem.FindControl("lblFeeType") as Label;
					DateTime dtDueDate = olblDueDate.Text.Trim().ToDateTime();
					int iSerialNo = lstvwPayFee.DataKeys[iRowId]["SerialNo"].ToInt();
                    var iConcessionAmount = lstvwPayFee.DataKeys[iRowId]["ConcessionAmount"].ToInt();

					if (dDueDate == dtDueDate && iSerialNo == 0)
					{
                        sRemarks = sRemarks + ", " + olblPaybleFor.Text.Trim() + " (" + olblFeeType.Text.Trim() + " - Rs." + olblAmount.Text.Trim() + "/-" + ")";
						iAmount = iAmount + olblAmount.Text.ToInt();
					}
					if (oCheckBox.Checked)
					{
                        sRemarks = sRemarks + ", " + olblPaybleFor.Text.Trim() + " (" + olblFeeType.Text.Trim() + " - Rs." + olblAmount.Text.Trim() + "/-" + ")";
						iAmount = iAmount + olblAmount.Text.ToInt();
						dDueDate = dtDueDate;
						iLateFeeAmount += olblLateFee.Text.Trim().ToInt();
						sDueDates = sDueDates + "," + dtDueDate;

                        iConcessionAmt = iConcessionAmt + iConcessionAmount.ToInt();

						if (olblLateFee.Text.Trim() != "0")
							sLateemarks = sLateemarks + "," + olblPaybleFor.Text.Trim();

                        if (moSchool == Constants.SchoolId.SNS)
                        {
                            sFeeType = olblFeeType.Text.Trim() + "$"+ olblPaybleFor.Text.Trim();
                            break;
                        }
					}
				}
				if (sDueDates.StartsWith(","))
					sDueDates = sDueDates.Substring(1);
				if (sLateemarks.StartsWith(","))
				{
					sLateemarks = sLateemarks.Substring(1);
					sLateemarks = " and late fee for " + sLateemarks;
				}
				if (sRemarks.StartsWith(","))
					sRemarks = sRemarks.Substring(1);
				sRemarks = "Amount paid for " + sRemarks + sLateemarks;

				//Set query string.
				sQueryString = sQueryString + "StudentId=" + hidStudentId.Value;
				sQueryString = sQueryString + "&DueDates=" + sDueDates;
				sQueryString = sQueryString + "&Remarks=" + sRemarks;
				sQueryString = sQueryString + "&AcadmicYearId=" + hidAcadmicYear.Value;
				sQueryString = sQueryString + "&StanardID=" + hidStdDivId.Value;
				sQueryString = sQueryString + "&TotalAmount=" + iAmount.ToString();
				sQueryString = sQueryString + "&LateFeeAmount=" + iLateFeeAmount.ToString();
				sQueryString = sQueryString + "&IsForNextYear=Y";
                sQueryString = sQueryString + "&ConcessionAmount=" + iConcessionAmt.ToString();
                sQueryString = sQueryString + "&FeeType=" + sFeeType;        

				string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
				hidQueryString.Value = sEncrypt;

				ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PayOnline", "window.open('../Accountant/PayFeeOnline.aspx?" + sEncrypt + "','_blank','left=0, top=0, height=470, width=1000, status=no, resizable= no, scrollbars= yes')", true);
			}
			else
			{
				btnPayOnline.Enabled = false;

				foreach (ListViewDataItem oListViewDataItem in lstvwPaidDetails.Items)
				{
					var oCheckBox = oListViewDataItem.FindControl("chkPay") as CheckBox;

					//Calculate total amount (checked checkbox)
					if (!oCheckBox.Checked)
						continue;
					
					var olblDueDate = oListViewDataItem.FindControl("lblPaidDate") as Label;
					DateTime dtDueDate = olblDueDate.Text.ToDateTime();
					sDueDates = sDueDates + "," + dtDueDate;
				}

				if (sDueDates.StartsWith(","))
					sDueDates = sDueDates.Substring(1);

				if (sRemarks.StartsWith(","))
					sRemarks = sRemarks.Substring(1);

                StudentBL oStudentBL = new StudentBL(miSchoolId,hidAcadmicYear.Value.ToInt(),hidStudentId.Value.ToInt());
                
				//Set query string.
				//sQueryString = sQueryString + "StudentId=" + hidStudentId.Value;
                sQueryString = sQueryString + "StudentId=" + oStudentBL.YearWiseStudentId;
				sQueryString = sQueryString + "&DueDates=" + sDueDates;
				sQueryString = sQueryString + "&Remarks=" + sRemarks;
				sQueryString = sQueryString + "&AcadmicYearId=" + hidAcadmicYear.Value;
				sQueryString = sQueryString + "&IsFinalYear=Y";

				string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);

				ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PayOnline", "window.open('../Accountant/PayFeeOnline.aspx?" + sEncrypt + "','_new','left=0, top=0, height=470, width=1000, status=no, resizable= no, scrollbars= yes')", true);
			}
		}

		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnPayPrint_Click(object sender, EventArgs e)
	{
		btnPay_Click(sender, e);
	}

	protected void lstvwPaidDetails_DataBound(object sender, EventArgs e)	
    {
		try
		{
            var oHtmlTableHeaderRow = lstvwPaidDetails.FindControl("trHeader") as HtmlTableRow;
            //var oHtmlTableCellChkAll = oHtmlTableHeaderRow.FindControl("thchkPay") as HtmlTableCell;

            //oHtmlTableCellChkAll.Visible = moUserRole != Constants.UserRoles.Student;

            //if (hidIsFInalYear.Value == "True")
            //    btnPayOnline.Visible = false;

			if (lstvwPaidDetails.Items.Count > 0)
				btnPayOnline.Attributes.Add("Onclick", string.Format("if(!CheckAtleastOneCheckBoxForNextYear('{0}', {1})){{return false;}}", lstvwPaidDetails.ClientID, lstvwPaidDetails.Items.Count));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method will be used to fill all the electronic types into the types dropdownlist.
    /// </summary>
    private void FillElectronicPaymentTypes()
    {
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbElectronicTypes, "Type", "TypeId", Constants.S_SELECT);
    }
	
	/// <summary>
	/// Generate XML for the Items.
	/// </summary>
	/// <returns></returns>
	private string GeneratePayFeeXML()
	{
		const string S_ELEMENT = "element";
		var oDoc = new XmlDocument();

		// Create a root level element.
		XmlElement root = oDoc.CreateElement("PaidFee");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "PaidFee", String.Empty);
		miTotalAmount = 0;

		// Loop through all the list view items.
		foreach (ListViewDataItem oListViewDataItem in lstvwPayFee.Items)
		{
			var oCheckBox = oListViewDataItem.FindControl("chkPay") as CheckBox;

			if (oCheckBox == null || !oCheckBox.Checked)
				continue;

			// Create root xml element.
			XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "PaidFee", String.Empty);

			int iRowId = oListViewDataItem.DataItemIndex;
			var olblPaybleFor = oListViewDataItem.FindControl("lblPaybleFor") as Label;
			var txtActualAmount = oListViewDataItem.FindControl("txtActualAmount") as TextBox;
            var lblAmount = oListViewDataItem.FindControl("lblAmount") as Label;
			var olblFeeType = oListViewDataItem.FindControl("lblFeeType") as Label;

			string sAtrrName = "Std_FeeType_Id";
			XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = (lstvwPayFee.DataKeys[iRowId]["Std_FeeType_Id"]).ToString();
			oXmlNode.Attributes.Append(attr);

			int iamt = txtActualAmount.Text.Trim().ToInt();
			sAtrrName = "Amount";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = iamt.ToString();
			oXmlNode.Attributes.Append(attr);

			miTotalAmount += iamt;

			sAtrrName = "Payable_For";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = olblPaybleFor.Text.Trim();
			oXmlNode.Attributes.Append(attr);

			sAtrrName = "FeeType";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = olblFeeType.Text.Trim();
			oXmlNode.Attributes.Append(attr);

			// Add the node to root node.
			oXmlRootNode.AppendChild(oXmlNode);
		}

		// Add the root node to document element. 
		root.AppendChild(oXmlRootNode);

		// return the string generated.
		return root.InnerXml;
	}

	private string GetChequeDetailsXML()
	{
		const string S_ELEMENT = "element";
		var oDoc = new XmlDocument();

		// Create a root level element.
		XmlElement root = oDoc.CreateElement("ChequeDetails");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ChequeDetails", String.Empty);

		// Create root xml element.
		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "ChequeDetails", String.Empty);

		string sAtrrName = "Amount";
		XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = txtActualAmount.Text;

		oXmlNode.Attributes.Append(attr);

		sAtrrName = "ChequeNo";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = txtChequeNumber.Text.Trim();

		oXmlNode.Attributes.Append(attr);

		sAtrrName = "ChequeDate";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = txtChequeDate.Text.Trim();

		oXmlNode.Attributes.Append(attr);

		sAtrrName = "BankID";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = ddlBankName.SelectedValue;

		oXmlNode.Attributes.Append(attr);

		// Add the node to root node.
		oXmlRootNode.AppendChild(oXmlNode);
		// Add the root node to document element. 
		root.AppendChild(oXmlRootNode);

		// return the string generated.
		return root.InnerXml;
	}

	private string GetPaymentDetailsXML()
	{
		const string S_ELEMENT = "element";
		var oDoc = new XmlDocument();
        XmlElement root;
        XmlNode oXmlRootNode;
        XmlNode oXmlNode;
		// Create a root level element.
        if (optCard.Checked)
        {
            root = oDoc.CreateElement("CardDetails");
            oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "CardDetails", String.Empty);            
            oXmlNode = oDoc.CreateNode(S_ELEMENT, "CardDetails", String.Empty);
        }
        else
        {
            root = oDoc.CreateElement("ElectronicDetails");
            oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ElectronicDetails", String.Empty);
            oXmlNode = oDoc.CreateNode(S_ELEMENT, "ElectronicDetails", String.Empty);
        }

		string sAtrrName = "Amount";
		XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = txtActualAmount.Text;

		oXmlNode.Attributes.Append(attr);

        if(optCard.Checked)
		    sAtrrName = "CardNo";
        else
            sAtrrName = "TxnNo";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = txtSwapNumber.Text.Trim();

		oXmlNode.Attributes.Append(attr);

        if (optCard.Checked)
        {
            sAtrrName = "CardType";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = ddlCardType.SelectedValue;
        }
        else
        {
            sAtrrName = "ElectronicType";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = cmbElectronicTypes.SelectedValue;
        }

		oXmlNode.Attributes.Append(attr);

		sAtrrName = "BankID";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = ddlBankName.SelectedValue;

		oXmlNode.Attributes.Append(attr);

		// Add the node to root node.
		oXmlRootNode.AppendChild(oXmlNode);
		// Add the root node to document element. 
		root.AppendChild(oXmlRootNode);

		// return the string generated.
		return root.InnerXml;
	}

	/// <summary>
	/// This method is used to read querystring.
	/// </summary>
	private void ReadQueryString()
	{
		hidStudentId.Value = QueryString["StudentId"];
		hidAcadmicYear.Value = QueryString["Academic_Year_ID"];
		hidStdDivId.Value = QueryString["StandardID"];
		hidIsFInalYear.Value = QueryString["IsFinalYear"];

		hidStudentIdQurStr.Value = moUserRole == Constants.UserRoles.Student ? Session[Constants.S_SESSION_STUDENT_ID].ToString() : QueryString["StudentIdQurStr"];
	}

	private void FillFeeListview()
	{
		const int I_BANK_TABLE = 1;
		
		DataSet oDataSet = moStudentFeeDetailsBL.getStudentFeeDetailsForNextYear(false, hidStdDivId.Value.ToInt());
        lstvwPayFee.DataSource = null;
        lstvwPayFee.DataBind();

		lstvwPayFee.DataSource = oDataSet.Tables[0];
		lstvwPayFee.DataBind();

		ViewState[S_FEE_DETAILS] = oDataSet.Tables[0];

		ddlBankName.Bind(oDataSet.Tables[I_BANK_TABLE], "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
		FillCardTypeCombo();
        FillElectronicPaymentTypes();
	}

	private void Initialize()
	{
		cal_ChequeDate.DateValue = DateTime.Now;
		cal_Date.DateValue = DateTime.Now;        		
        SetDefaultPaymentType();
	}

	private void ResetControls()
	{
        optCard.Checked = false;
        optCash.Checked = false;
        optElectronic.Checked = false;
		optCheque.Checked = true;
		txtChequeNumber.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		txtLateFeeAmount.Text = "0";
        txtConcessionAmount.Text = "0";
		hidLateFeeAmt.Value = "0";
		hidActualAmt.Value = "0";
        hidTotalActualAmount.Value = "0";
		hidLateFeeRemark.Value = string.Empty;
		txtAmountPayable.Text = string.Empty;
		txtTotalAmount.Text = string.Empty;
		txtActualAmount.Text = string.Empty;
        lblCardType.Visible = true;
        ddlCardType.Visible = true;
	}

    /// <summary>
    /// This method is used to set the default payment type associated to the school.
    /// </summary>
    private void SetDefaultPaymentType()
    {
        if (Settings.DefaultFeeType == Constants.FeePaymentType.Cash.ToString())
        {
            optCash.Checked = true;
            optCash_CheckedChanged(this, new EventArgs());            
        }
        else if (Settings.DefaultFeeType == Constants.FeePaymentType.Electronic.ToString())
        {
            optElectronic.Checked = true;
            optElectronic_CheckedChanged(this, new EventArgs());
        }
        else if (Settings.DefaultFeeType == Constants.FeePaymentType.SwapCard.ToString())
        {
            optCard.Checked = true;
            optCard_CheckedChanged(this, new EventArgs());
        }
        else
        {
            optCheque.Checked = true;
            EnableDisabledCardControls(false);
            DisplayElectronicControls(false);
        }
    }

	private void EnableDisabledChequeControls(bool abFlag)
	{
		txtChequeDate.Enabled = abFlag;
		txtChequeNumber.Enabled = abFlag;

		if (abFlag)
			EnableDisabledBankCombo(true);
	}

	private void EnableDisabledCardControls(bool abFlag)
	{
		txtSwapNumber.Enabled = abFlag;
        if (!lblCardType.Visible)
            lblCardType.Visible = true;
        if (!ddlCardType.Visible)
            ddlCardType.Visible = true;
        ddlCardType.Enabled = abFlag;
		ddlCardType.SelectedValue = "0";        
		txtSwapNumber.Text = string.Empty;              
        
		if (abFlag)
			EnableDisabledBankCombo(true);
	}

    /// <summary>
    /// This method is used to display or hide electronic controls depending on flag passed.
    /// </summary>
    /// <param name="abFlag"></param>
    private void DisplayElectronicControls(bool abFlag)
    {
        lblType.Visible = abFlag;
        cmbElectronicTypes.Visible = abFlag;        
    }

	private void EnableDisabledBankCombo(bool abFlag)
	{
		ddlBankName.Enabled = abFlag;
		ddlBankName.SelectedValue = "0";
	}

	private void FillCardTypeCombo()
	{
		var oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
		DataTable dtCardTypeList = oSchoolwiseBankMasterBL.GetSchoolwiseCardTypeList(miSchoolId);

		ddlCardType.Bind(dtCardTypeList, "CardTypeId", "CardType", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to create query string and redirect to base screen.
	/// </summary>
	private void SetQueryString()
	{
		string sQueryString = "StudentId=" + hidStudentIdQurStr.Value;
		string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
		sQueryString = "'?" + sEncryptQueryString + "'";
		ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ClosePopup", "window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); window.close();", true);
	}

	private void DisplayFeeDetails()
	{
		int AcademicYearId = 0;
		int StudentId = 0;
		DataTable oDT = StudentFeeDetailsBL.GetFeeDetailsForDisplay(hidStudentId.Value.ToInt(), DateTime.Now, out AcademicYearId, out StudentId);
		lstvwPaidDetails.DataSource = oDT;
		lstvwPaidDetails.DataBind();
		hidAcadmicYear.Value = Convert.ToString(AcademicYearId);
		hidStudentId.Value = Convert.ToString(StudentId);
	}

	/// <summary>
	/// This method is used to set the java scripts attributes.
	/// </summary>
	private void SetJavaScriptsAttributes()
	{
		valChequeData.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;		
		optCash.Attributes.Add("Onclick", "SetTotalAmount();if(!ResetValidationSummary()){return false;}");
		optCard.Attributes.Add("Onclick", "SetTotalAmount();if(!ResetValidationSummary()){return false;}");
		optCheque.Attributes.Add("Onclick", "SetTotalAmount(); if(!ResetValidationSummary()){return false;}");
        optElectronic.Attributes.Add("Onclick", "SetTotalAmount(); if(!ResetValidationSummary()){return false;}");
		btnPay.Attributes.Add("onclick", "if(!SelectedCount()){return false;}");
		btnPayPrint.Attributes.Add("onclick", "if(!SelectedCount()){return false;}");
        txtConcessionAmount.Attributes.Add("onchange", "SetConcessionRemark()");        
		ApplyMouseHoverEffect(new List<Button> { btnPay, btnPayPrint, btnClose, btnPayOnline });

        if (moSchool == Constants.SchoolId.SNS)
            hidRestrictMultipleFees.Value = Constants.S_ONE;
        else
            hidRestrictMultipleFees.Value = Constants.S_ZERO;

        SetConcessionMessage();
	}

    /// <summary>
    /// This method is used to set concession.
    /// </summary>
    private void SetConcessionMessage()
    {
        if (moUserRole != Constants.UserRoles.Student)
        {
            string sConcessionMessage = moStudentFeeDetailsBL.GetConcessionMessage(hidStdDivId.Value.ToInt());
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
    /// This method will be used to set the common controls to show diff. messages.
    /// </summary>
    /// <param name="abflag"></param>
    /// <param name="asMessage"></param>
    private void SetFields(bool abflag, string asMessage)
    {
        trlblErrMsg.Visible = abflag;
        lblErrMsg.Visible = abflag;
        lblErrMsg.Text = asMessage;
    }

    /// <summary>
    /// This is a private method and is used to pay fee with card payments.
    /// </summary>
    /// <param name="aiLateAmount"></param>
    /// <param name="asFeeDetailsXML"></param>
    /// <param name="iSerialNo"></param>
	private void PayFeeWithCard(int aiLateAmount, string asFeeDetailsXML, out int aiSerialNo)
	{
		aiSerialNo = 0;
        if (moStudentFeeDetailsBL.IsDuplicateCardNumber(txtSwapNumber.Text.Trim()))
		{
			FillFeeListview();
            SetFields(true,S_TXN_EXISTS_MESSAGE);
		}
		else
		{
            SetFields(false, string.Empty);			
            string sCardDetailsXML = GetPaymentDetailsXML();
			moStudentFeeDetailsBL.InsertStudentFeeDetailsForNextYear(asFeeDetailsXML, sCardDetailsXML,aiLateAmount,Constants.PaymentMode.Card.ToInt(), out aiSerialNo,txtConcessionAmount.Text.ToInt());
		}
	}

    /// <summary>
    /// This method is used to pay fee by electronic mode.
    /// </summary>
    /// <param name="aiLateAmount"></param>
    /// <param name="asFeeDetailsXML"></param>
    /// <param name="iSerialNo"></param>
    private void PayFeeWithElectronic(int aiLateAmount, string asFeeDetailsXML, out int iSerialNo)
    {
        iSerialNo = 0;
        if (moStudentFeeDetailsBL.IsDuplicateTxnNumberForNextYear(txtSwapNumber.Text.Trim()))
        {
            //FillFeeListview();            
            SetFields(true, S_TXN_EXISTS_MESSAGE);
        }
        else
        {
            SetFields(false, string.Empty);			
            string sElectronicDetailsXML = GetPaymentDetailsXML();
            moStudentFeeDetailsBL.InsertStudentFeeDetailsForNextYear(asFeeDetailsXML, sElectronicDetailsXML, aiLateAmount,Constants.PaymentMode.Electronic.ToInt(), out iSerialNo,txtConcessionAmount.Text.ToInt());
        }
    }

	#endregion -- PRIVATE METHOD(s) --
}
