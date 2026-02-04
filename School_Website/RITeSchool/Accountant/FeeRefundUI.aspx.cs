// File Name  : FeeRefundUI.aspx.cs
// Created By : Milind
// Date       : 29/7/2009
//Description :This class is used to refund the student fee.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class FeeRefundUI : SchoolBase
{

	#region Constants

	private const int I_BANK_TABLE = 1;
	private const int I_REFUND_TABLE = 0;
	private const int I_MAX_DATE = 2;

	#endregion

	#region Events

	#region Page Events

	/// <summary>
	/// This event is used to fill list view with student paid fees.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();

				ReadQueryString();
				if (hidStudentId.Value != null && hidStudentId.Value != Constants.S_EMPTY_STRING)
				{
					FillRefundDetailControls();
					optCheque.Checked = true;
					EnabledDisableControls(true);
					SetJavaScriptsAttributes();
				}
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
			hidServerDate.Value = Convert.ToString(DateTime.Today);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to enabled only the controls which are used to refund fee by Cheque.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optCheque_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (optCheque.Checked)
				EnabledDisableControls(true);
			else
				optCash.Checked = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to enabled only the controls which are used to refund fee by Cash
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optCash_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (optCash.Checked)
				EnabledDisableControls(false);
			else
				optCheque.Checked = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used save the refund details in the database
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnRefund_Click(object sender, EventArgs e)
	{
		try
		{
			int iTotalAmount = 0;
			StudentFeeDetailsBL oStudentFeeDetailsBL = PopulateStudFeeDetailsBL();
			string sRefundFeeDetails = GenerateRefundFeeDetailsXML(out iTotalAmount);

			//If fee is refund by cheque that time need the cheque details for saving.
			if (optCheque.Checked)
			{
				DateTime dtChequeDate = cal_ChequeDate.DateValue;
				int iChequeNumber = Convert.ToInt32(txtChequeNumber.Text.Trim());
				int iBankId = Convert.ToInt32(ddlBankName.SelectedValue);

				oStudentFeeDetailsBL.InsertRefundFeeDetails(dtChequeDate, iChequeNumber, iBankId, iTotalAmount, sRefundFeeDetails);
			}
			else
				oStudentFeeDetailsBL.InsertRefundFeeDetails(iTotalAmount, sRefundFeeDetails);

			SetQueryString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to close the pop up.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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

	#endregion

	#region ListView Events

	/// <summary>
	/// This event is used add the javascripts attributs on header check box of the list view.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwRefundFee_DataBound(object sender, EventArgs e)
	{
		try
		{
			System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableHeaderRow = lstvwRefundFee.FindControl("trHeader") as System.Web.UI.HtmlControls.HtmlTableRow;
			CheckBox ChkHeader = oHtmlTableHeaderRow.FindControl("chkAll") as CheckBox;
			ChkHeader.Attributes.Add("Onclick", "EnabledCheckBox('" + ChkHeader.ClientID + "'," + lstvwRefundFee.Items.Count + ")");
			btnRefund.Attributes.Add("Onclick", "if(!CheckAtleastOneCheckBox('" + ChkHeader.ClientID + "'," + lstvwRefundFee.Items.Count + ")){return false;}");
			hidRowCount.Value = lstvwRefundFee.Items.Count.ToString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used add the javascripts attributs on each row check box of the list view.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwRefundFee_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{

			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				CheckBox chkRefundRow = e.Item.FindControl("chkRefund") as CheckBox;
				TextBox txtRefundAmountRow = e.Item.FindControl("txtRefundAmount") as TextBox;
				Label lblStarRow = e.Item.FindControl("lblStar") as Label;
				HiddenField hidActualAmount = e.Item.FindControl("hidActualAmount") as HiddenField;
				txtRefundAmountRow.Enabled = false;
				lblStarRow.Enabled = false;

				chkRefundRow.Attributes.Add("Onclick", "EnabledTextBox('" + chkRefundRow.ClientID + "','" + txtRefundAmountRow.ClientID + "','" + lblStarRow.ClientID + "','" + hidActualAmount.ClientID + "')");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#endregion

	#region Private Methods

	/// <summary>
	/// This method is used to read querystring.
	/// </summary>
	private void ReadQueryString()
	{
		hidStudentId.Value = QueryString["StudentId"];
	}

	/// <summary>
	/// This method is used fill the controls on the pop up
	/// </summary>
	private void FillRefundDetailControls()
	{
		int iStudentId = Convert.ToInt32(hidStudentId.Value);
		DataSet oDSRefundDetails;
		StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
		oDSRefundDetails = oStudentFeeDetailsBL.GetStudentRefundDetails(iStudentId, miSchoolId, miAcademicYearId);
		ddlBankName.Bind(oDSRefundDetails.Tables[I_BANK_TABLE], "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
		if (oDSRefundDetails.Tables[I_REFUND_TABLE].Rows.Count > 0)
		{
			lstvwRefundFee.DataSource = oDSRefundDetails.Tables[I_REFUND_TABLE];
			lstvwRefundFee.DataBind();
			cal_ChequeDate.DateValue = System.DateTime.Now;
			cal_Date.DateValue = System.DateTime.Now;
			hidStdDivId.Value = (oDSRefundDetails.Tables[I_REFUND_TABLE].Rows[0]["Standard_Div_Id"]).ToString();
			hidMaxdate.Value = (oDSRefundDetails.Tables[I_MAX_DATE].Rows[0]["MaxDate"]).ToString();
		}
		else
			btnRefund.Enabled = false;
	}

	/// <summary>
	/// This method is used enabled or disabled controls according to radio button.
	/// </summary>
	private void EnabledDisableControls(bool abFlag)
	{
		txtChequeNumber.Text = "";
		txtChequeDate.Enabled = abFlag;
		txtChequeNumber.Enabled = abFlag;
		cal_ChequeDate.Enabled = abFlag;
		ddlBankName.Enabled = abFlag;
		ddlBankName.SelectedValue = "0";
	}

	/// <summary>
	/// This method is used assing value to StudentFeeDetailsBL class properties
	/// </summary>
	private StudentFeeDetailsBL PopulateStudFeeDetailsBL()
	{
		StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();

		oStudentFeeDetailsBL.School_Id = miSchoolId;
		oStudentFeeDetailsBL.Academic_Year_Id = miAcademicYearId;
		oStudentFeeDetailsBL.Standard_Div_Id = Convert.ToInt32(hidStdDivId.Value);
		oStudentFeeDetailsBL.Student_Id = Convert.ToInt32(hidStudentId.Value);
		oStudentFeeDetailsBL.Inserted_By_id = miUserId;
		oStudentFeeDetailsBL.Paid_Date = cal_Date.DateValue;
		oStudentFeeDetailsBL.Remarks = txtRemarks.Text.Trim();

		return oStudentFeeDetailsBL;
	}

	/// <summary>
	/// Generate XML for the student refund details.
	/// </summary>
	/// <returns></returns>
	private string GenerateRefundFeeDetailsXML(out int aiTotalAmount)
	{
		aiTotalAmount = 0;
		const string S_ELEMENT = "element";
		XmlDocument oDoc = new XmlDocument();

		// Create a root level element.
		XmlElement root = oDoc.CreateElement("RefundFeeDetails");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "RefundFeeDetails", "");

		// Loop through all the list view rows.
		foreach (ListViewDataItem oListViewDataItem in lstvwRefundFee.Items)
		{
			CheckBox ochkRefund = oListViewDataItem.FindControl("chkRefund") as CheckBox;

			if (ochkRefund.Checked)
			{
				// Create root xml element.
				XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "RefundFeeDetails", "");

				int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
				Label olblPayableFor = oListViewDataItem.FindControl("lblPaybleFor") as Label;
				Label olblFeeType = oListViewDataItem.FindControl("lblFeeType") as Label;
				TextBox otxtAmount = oListViewDataItem.FindControl("txtRefundAmount") as TextBox;

				string sAtrrName = "Payable_For";
				XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = olblPayableFor.Text.Trim();
				oXmlNode.Attributes.Append(attr);

				sAtrrName = "Fee_Type";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = olblFeeType.Text.Trim();
				oXmlNode.Attributes.Append(attr);

				sAtrrName = "Amount";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = otxtAmount.Text.Trim();
				oXmlNode.Attributes.Append(attr);
				aiTotalAmount += Convert.ToInt32(otxtAmount.Text.Trim());

				sAtrrName = "Std_FeeType_Id";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = (lstvwRefundFee.DataKeys[iRowId]["Std_FeeType_Id"]).ToString();
				oXmlNode.Attributes.Append(attr);

				sAtrrName = "Student_Fee_Id";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = (lstvwRefundFee.DataKeys[iRowId]["Schoolwise_Student_Fee_Id"]).ToString();
				oXmlNode.Attributes.Append(attr);

                sAtrrName = "AccountHeaderId";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = (lstvwRefundFee.DataKeys[iRowId]["AccountHeaderId"]).ToString();
                oXmlNode.Attributes.Append(attr);

				// Add the node to root node.
				oXmlRootNode.AppendChild(oXmlNode);
			}
		}
		// Add the root node to document element. 
		root.AppendChild(oXmlRootNode);

		// return the string generated.
		return root.InnerXml;
	}

	/// <summary>
	/// This method is used to create query string and redirect to base screen.
	/// </summary>
	private void SetQueryString()
	{
		string sQueryString = "StudentId=" + hidStudentId.Value;
		string sEncryptQueryString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
		sQueryString = "'?" + sEncryptQueryString + "'";
		Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); ");
		Response.Write("window.close();");
		Response.Write("</script>");
	}

	/// <summary>
	/// This method is used to set the java scripts attributes.
	/// </summary>
	private void SetJavaScriptsAttributes()
	{
		optCash.Attributes.Add("Onclick", "SetTotalAmount()");
		optCheque.Attributes.Add("Onclick", "SetTotalAmount()");
		ApplyMouseHoverEffect(new List<Button> { btnRefund, btnClose });
	}

    /// <summary>
    /// This method is used to set design according to thelanguage selected.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valChequeData.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidChequeNumberShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeNumberShouldNotBeBlank;
        hidBankNameShouldBeSelected.Value = Resources.LocalizedResources.BankNameShouldBeSelected;
        hidRefundDateShouldBeGreaterThanOrEqualToPaymentDate.Value = Resources.LocalizedResources.RefundDateShouldBeGreaterThanOrEqualToPaymentDate;
        hidRefundDateShouldNotBeFutureDate.Value = Resources.LocalizedResources.RefundDateShouldNotBeFutureDate;
        hidRefundDateShouldNotBeBlank.Value = Resources.LocalizedResources.RefundDateShouldNotBeBlank;
        hidChequeDateShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeDateShouldNotBeBlank;
        hidRefundAmountShouldNotBeGreaterThanActualAmount.Value = Resources.LocalizedResources.RefundAmountShouldNotBeGreaterThanActualAmount;
        hidRefundAmountShouldBeGreaterThanZero.Value = Resources.LocalizedResources.RefundAmountShouldBeGreaterThanZero;
        hidPleaseFixFollowingError.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAtLeastOneFeeShouldBeSelectedForRefund.Value = Resources.LocalizedResources.AtLeastOneFeeShouldBeSelectedForRefund;
        hidRefundAmountShouldNotBeBlank.Value = Resources.LocalizedResources.RefundAmountShouldNotBeBlank;
    }
	#endregion

}
