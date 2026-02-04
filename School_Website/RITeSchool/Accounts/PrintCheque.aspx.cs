/* -----------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 14-Apr-2012
 *	Purpose		: Displays cheque details for printing a cheque.
 * -----------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.XPath;
using AccountsEntities;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

public partial class PrintCheque : SchoolBase
{
	
	#region -- MEMBER(s) --

	private int miConfigId;
	private bool mbIsCrossCheque;
	private DateTime mdtChequeDate;
	private string msPayeeName;
	private decimal mdAmount;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// Reads from the Query string and displays cheque details on the screen.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			ReadQueryString();
			DisplayDetails();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// Initializes member variables with values from the QueryString.
	/// </summary>
	private void ReadQueryString()
	{
		if (Request.QueryString.Count <= 0)
			return;

		if (!QueryString["ConfigId"].IsNullOrEmpty())
			miConfigId = QueryString["ConfigId"].ToInt();
		
		if (!QueryString["IsCrossCheque"].IsNullOrEmpty())
			mbIsCrossCheque = QueryString["IsCrossCheque"].ToInt().ToBool();
		
		if (!QueryString["ChequeDate"].IsNullOrEmpty())
			mdtChequeDate = QueryString["ChequeDate"].ToDateTime();
		
		if (!QueryString["PayeeName"].IsNullOrEmpty())
			msPayeeName = QueryString["PayeeName"];
		
		if (!QueryString["Amount"].IsNullOrEmpty())
			mdAmount = QueryString["Amount"].ToDecimal();
	}

	/// <summary>
	/// Displays & places the elements on the screen as per the given Cheqe configid.
	/// </summary>
	private void DisplayDetails()
	{
        var oBankAccountClient = new BankAccountClient();
		oBankAccountClient.Open();

		int iSchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
		ChequeConfiguration oChequeConfiguration = oBankAccountClient.GetChequeConfiguration(iSchoolId, miConfigId);

		XElement xele = XElement.Parse(oChequeConfiguration.ConfigXML);

		crosschq.Visible = mbIsCrossCheque;
		
		if (xele.XPathSelectElement("Date").Attribute("Show").Value == "true")
		{
			date.Attributes["style"] = String.Format("top: {0}px; left: {1}px;",
													  xele.XPathSelectElement("Date").Attribute("Top").Value,
													  xele.XPathSelectElement("Date").Attribute("Left").Value);
			date.InnerText = mdtChequeDate.ToString("dd-MMM-yyyy");
		}

		if (xele.XPathSelectElement("Payee").Attribute("Show").Value == "true")
		{
			payee.Attributes["style"] = String.Format("top: {0}px; left: {1}px; width: {2}px;",
													  xele.XPathSelectElement("Payee").Attribute("Top").Value,
													  xele.XPathSelectElement("Payee").Attribute("Left").Value,
													  xele.XPathSelectElement("Payee").Attribute("Width").Value);
			payee.InnerText = msPayeeName;
		}

		if (xele.XPathSelectElement("Amount").Attribute("Show").Value == "true")
		{
			amount.Attributes["style"] = String.Format("top: {0}px; left: {1}px;",
													   xele.XPathSelectElement("Amount").Attribute("Top").Value,
													   xele.XPathSelectElement("Amount").Attribute("Left").Value);
			amount.InnerText = String.Format("** {0} **", CommonUtility.FormatCurrency(mdAmount));
		}

		if (xele.XPathSelectElement("AmountInWords").Attribute("Show").Value == "true")
		{
			amountinwords.Attributes["style"] = String.Format("top: {0}px; left: {1}px; width: {2}px; height: {3}px; text-indent: {4}px; line-height: {5}px;",
															  xele.XPathSelectElement("AmountInWords").Attribute("Top").Value,
															  xele.XPathSelectElement("AmountInWords").Attribute("Left").Value,
															  xele.XPathSelectElement("AmountInWords").Attribute("Width").Value,
															  xele.XPathSelectElement("AmountInWords").Attribute("Height").Value,
															  xele.XPathSelectElement("AmountInWords").Attribute("Indent").Value,
															  xele.XPathSelectElement("AmountInWords").Attribute("LineSpacing").Value);
			amountinwords.InnerText = String.Format("** {0} **", CommonUtility.GetNumberInWords(mdAmount.ToString()).Replace("Rupees ", String.Empty));
		}

		if (xele.XPathSelectElement("Company").Attribute("Show").Value == "true")
		{
			company.Attributes["style"] = String.Format("top: {0}px; left: {1}px; width: {2}px;",
													  xele.XPathSelectElement("Company").Attribute("Top").Value,
													  xele.XPathSelectElement("Company").Attribute("Left").Value,
													  xele.XPathSelectElement("Company").Attribute("Width").Value);
			company.InnerText = xele.XPathSelectElement("Company").Attribute("Name").Value;
		}

		if (xele.XPathSelectElement("Signatory1").Attribute("Show").Value == "true")
		{
			signatory1.Attributes["style"] = String.Format("top: {0}px; left: {1}px; width: {2}px;",
														   xele.XPathSelectElement("Signatory1").Attribute("Top").Value,
														   xele.XPathSelectElement("Signatory1").Attribute("Left").Value,
														   xele.XPathSelectElement("Signatory1").Attribute("Width").Value);
			signatory1.InnerText = xele.XPathSelectElement("Signatory1").Attribute("Name").Value;
		}

		if (xele.XPathSelectElement("Signatory2").Attribute("Show").Value == "true")
		{
			signatory2.Attributes["style"] = String.Format("top: {0}px; left: {1}px; width: {2}px;",
														   xele.XPathSelectElement("Signatory2").Attribute("Top").Value,
														   xele.XPathSelectElement("Signatory2").Attribute("Left").Value,
														   xele.XPathSelectElement("Signatory2").Attribute("Width").Value);
			signatory2.InnerText = xele.XPathSelectElement("Signatory2").Attribute("Name").Value;
		}

		if (oBankAccountClient.State != CommunicationState.Faulted)
			oBankAccountClient.Close();
	}

	#endregion -- PRIVATE METHOD(s) --

}