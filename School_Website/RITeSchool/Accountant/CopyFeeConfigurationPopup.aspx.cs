using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class CopyFeeConfigurationPopup : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_SELECT_AT_LEAST_ONE_GROUP = "At least one standard should be selected for copying the fee.";

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to fill standard grid.
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
				GetStandardFeeDetails();
				GetStandardList();
				FillStandardGridView();
				chkSendSMS.Focus();
			}
			btnCopy.Attributes.Add("onclick", String.Format("if(!ConfirmCopy('{0}','{1}')) {{return false;}}", grdStandards.AllowPaging, S_SELECT_AT_LEAST_ONE_GROUP));
			ApplyMouseHoverEffect(new List<Button> { btnCopy, btnClose });
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to copy the fee configuration to the seleted standard.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCopy_Click(object sender, EventArgs e)
	{
		try
		{
			ArrayList arrStdLst = GetStdDivIdLst();
			StudentFeeDetailsBL oStudentFeeDetailsBL = PopulateFeeDetailsBL();
			if (hidIsInternalFee.Value.ToBool())
				oStudentFeeDetailsBL.CopyStudentInternalFeeDetails(arrStdLst);
			else
				oStudentFeeDetailsBL.CopyStudentFeeDetails(arrStdLst);

			string sPath = SendMessage("New");

			if (!sPath.IsNullOrEmpty())
				Response.Write("<Script language='javascript'> window.opener.location.href='" + sPath + "'; window.close();</Script>");
			else
				Response.Write("<Script language='javascript'> window.close(); window.opener.focus();</Script>");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to close the pop up
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			Response.Write("<Script language='Javascript'> window.close();window.opener.focus(); </Script>");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			int iRowIndex = e.Row.RowIndex;
			if (iRowIndex >= 0)
			{
				int iStandardId = grdStandards.DataKeys[iRowIndex]["Standard_Id"].ToInt();
				var chk = e.Row.Cells[Constants.I_ZERO].FindControl("ChkBoxCopy") as CheckBox;
				if (hidStandardIDList.Value.Contains(iStandardId.ToString()))
				{
					chk.Visible = false;
					e.Row.Enabled = false;
				}
				else
				{
					chk.Visible = true;
					e.Row.Enabled = true;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to check that SMS has to send or not.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkSendSMS_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			hidSendSms.Value = chkSendSMS.Checked ? Constants.S_YES : Constants.S_NO;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to check that Message has to send or not.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkSendMessage_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			hidSendMsg.Value = chkSendMessage.Checked ? Constants.S_YES : Constants.S_NO;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdStandards_DataBound(object sender, EventArgs e)
	{
		try
		{
			int iCountEnable = 0;
			for (int i = 0; i < grdStandards.Rows.Count; i++)
			{
				if (!grdStandards.Rows[i].Enabled)
					iCountEnable++;
			}
			if (iCountEnable == grdStandards.Rows.Count)
			{
				//HtmlInputCheckBox chk = grdStandards.HeaderRow.Cells[0].Controls[1] as HtmlInputCheckBox;
				var chk = grdStandards.HeaderRow.Cells[0].FindControl("ChkAllCopy") as HtmlInputCheckBox;
				chk.Visible = false;
				btnCopy.Enabled = false;
			}
			else
			{
				var chk = grdStandards.HeaderRow.Cells[0].FindControl("ChkAllCopy") as HtmlInputCheckBox;
				chk.Visible = true;
				// grdStandards.HeaderRow.Enabled = true;
				btnCopy.Enabled = true;
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
	/// This method is used to send a sms for new fee type added/updated.
	/// </summary>
	/// <param name="asIsNew"></param>
	private string SendMessage(string asIsNew)
	{
		//MasterPage oMasterPage = (MasterPage)this.Master;
		int iSmsId = 0;
		if (asIsNew.Equals(Constants.S_NEW_MODE))
			iSmsId = Constants.SMSTemplate.NewFeesSMS.ToInt();
		else if (asIsNew.Equals(Constants.S_EDIT_MODE))
			iSmsId = Constants.SMSTemplate.NewFeesUpdateSMS.ToInt();
		else
			iSmsId = Constants.SMSTemplate.FeesDeletedSMS.ToInt();

		string sQueryString = string.Empty;
		string sPath = string.Empty;

		if (hidSendSms.Value.Equals(Constants.S_YES) && hidSendMsg.Value.Equals(Constants.S_YES))
		{
			// Redirect to Sms center with a flag that will further redirect to message center.
			sQueryString = PrepareQueryString(true, iSmsId);
			hidSendSms.Value = "N";
			hidSendMsg.Value = "N";
			sPath = "../Common/SMSUI.aspx?" + sQueryString;
		}
		else if (hidSendSms.Value.Equals(Constants.S_YES))
		{
			// Redirect to Sms center with a flag that will not redirect to message center.
			sQueryString = PrepareQueryString(false, iSmsId);
			hidSendSms.Value = "N";
			sPath = "../Common/SMSUI.aspx?" + sQueryString;
		}
		else if (hidSendMsg.Value.Equals(Constants.S_YES))
		{
			// Redirect to message center.
			sQueryString = PrepareQueryString(false, iSmsId);
			hidSendMsg.Value = "N";
			sPath = "../Common/SendMessageFromInbox.aspx?" + sQueryString;
		}
		return sPath;
	}

	/// <summary>
	/// This method is used to prepare Query Strings.
	/// </summary>
	private string PrepareQueryString(bool abIsRedirectToMsgCenter, int aiSmsId)
	{
		const string S_PAGE = "CopyFeeConfiguration";
        string sQuerystring = string.Format("From={0}{1}&SmsId={2}&StandardId={3}&FeeType={4}&DueDate={5}&Amount={6}&PayableFor={7}&ConsiderForRTEConcession={8}",
											 S_PAGE,
											 (abIsRedirectToMsgCenter ? "&SendMsg=Y" : string.Empty),
											 aiSmsId,
											 hidSelectedStdList.Value,
											 lblFeeType.Text,
											 lblPaidDate.Text,
											 lblAmount.Text,
											 lblPayableFor.Text,
                                             chkRTEStudent.Checked);

		string sQueryString = CommonUtility.EncryptQuerystring(sQuerystring);

		return sQueryString;
	}

	/// <summary>
	/// This method is used to fill standard grid view.
	/// </summary>
	private void FillStandardGridView()
	{
		var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
		int iStandardID = hidStandardID.Value.ToInt();
		DataTable oDTUserDetails = oStandardCollectionBL.GetAllStandardsForFee(iStandardID);
		grdStandards.DataSource = oDTUserDetails;
		grdStandards.DataBind();
		hidRowCnt.Value = Convert.ToString(grdStandards.Rows.Count);
	}

	/// <summary>
	/// This method is used to decrypt query string.
	/// </summary>
	private void ReadQuerystring()
	{
		if (Request.QueryString.ToString() != String.Empty)
		{
			if (!QueryString["SerialNumber"].IsNull())
				hidSerialNumber.Value = QueryString["SerialNumber"];
            if (!QueryString["IsInternalFee"].IsNull())
                hidIsForInternalFee.Value = QueryString["IsInternalFee"].ToString();
		}
	}

	/// <summary>
	/// This method is used to get standard fee details.
	/// </summary>
	private void GetStandardFeeDetails()
	{
		DataTable oDataTable = StudentFeeDetailsBL.GetStandardFeeDetails(miSchoolId, miAcademicYearId, hidSerialNumber.Value.ToInt());

        hidAccountHeaderId.Value = StudentFeeDetailsBL.GetAccountHeaderIdBySerialNo(miSchoolId, miAcademicYearId, hidSerialNumber.Value.ToInt(), hidIsForInternalFee.Value.ToInt()).ToString();
        
		lblAmount.Text = oDataTable.Rows[0]["Amount"].ToString();
		lblPaidDate.Text = oDataTable.Rows[0]["Paid_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
		lblPayableFor.Text = oDataTable.Rows[0]["Payable_For"].ToString();
		lblFeeType.Text = oDataTable.Rows[0]["Fee_Type"].ToString();
		lblRemarks.Text = oDataTable.Rows[0]["Remarks"].ToString();
		hidDebitID.Value = oDataTable.Rows[0]["DebitID"].ToString();
		hidIsInternalFee.Value = oDataTable.Rows[0]["IsInternalFee"].ToString();
		hidStandardID.Value = oDataTable.Rows[0]["Standard_Id"].ToString();
        hidIsDueDateApplicable.Value = oDataTable.Rows[0]["IsDueDateApplicable"].ToBool() ? Constants.S_YES : Constants.S_NO;
        hidIsOnlinePaymentApplicable.Value = oDataTable.Rows[0]["IsOnlinePaymentApplicable"].ToBool() ? Constants.S_YES : Constants.S_NO;
	}

	/// <summary>
	/// This method is used to get stddivid for selected standard-division 
	/// and add it to arralist and return arraylist.
	/// </summary>
	/// <returns></returns>
	private ArrayList GetStdDivIdLst()
	{
		var oarrStdIdLst = new ArrayList();
		for (int i = 0; i < grdStandards.Rows.Count; i++)
		{
			var chk = grdStandards.Rows[i].Cells[Constants.I_ONE].FindControl("ChkBoxCopy") as CheckBox;
			if (chk.Visible && chk.Checked)
			{
				oarrStdIdLst.Add(grdStandards.DataKeys[i]["Standard_Id"]);
				hidSelectedStdList.Value = hidSelectedStdList.Value + "," + grdStandards.DataKeys[i]["Standard_Id"];
			}
		}
		hidSelectedStdList.Value = hidSelectedStdList.Value.Substring(1);
		return oarrStdIdLst;
	}

	/// <summary>
	/// This method is used to populate StudentFeeDetailsBL and returns its object.
	/// </summary>
	/// <returns></returns>
	private StudentFeeDetailsBL PopulateFeeDetailsBL()
	{
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL
            {
                Academic_Year_Id = miAcademicYearId,
                School_Id = miSchoolId,
                Inserted_By_id = miUserId,
                Insert_Date = DateTime.Now,
                Amount = lblAmount.Text.ToInt(),
                DebitOrCredit = "Debit",
                Paid_Date = lblPaidDate.Text.ToDateTime(),
                Remarks = lblRemarks.Text.Trim(),
                Payable_For = lblPayableFor.Text,
                FeeType = lblFeeType.Text,
                ConsiderRTEStudent = (chkRTEStudent.Checked == true) ? false : true,
                AccountHeaderId = hidAccountHeaderId.Value.ToInt(),
                IsDueDateApplicable = (hidIsDueDateApplicable.Value == Constants.S_YES ? true : false),
                IsConsiderForOnlinePayment = (hidIsOnlinePaymentApplicable.Value == Constants.S_YES ? true : false)
            };
        return oStudentFeeDetailsBL;
	}

	private void GetStandardList()
	{
		int iStandardId = hidStandardID.Value.ToInt();
		hidStandardList.Value = string.Empty;
		DataTable oDataTable = StudentFeeDetailsBL.GetStandardListForFeeDetails(miSchoolId, miAcademicYearId, iStandardId, lblFeeType.Text.Trim(), lblPayableFor.Text.Trim());

		foreach (DataRow datarow in oDataTable.Rows)
		{
			hidStandardList.Value += datarow["Standard_Name"].ToString() + " , ";
			hidStandardIDList.Value += datarow["Standard_Id"].ToString() + " , ";
		}

		if (hidStandardList.Value != string.Empty)
			hidStandardList.Value = hidStandardList.Value.Substring(0, (hidStandardList.Value.LastIndexOf(" , ")));

		if (hidStandardIDList.Value != string.Empty)
			hidStandardIDList.Value = hidStandardIDList.Value.Substring(0, (hidStandardIDList.Value.LastIndexOf(" , ")));
	}

	#endregion -- PRIVATE METHOD(s) --

}
