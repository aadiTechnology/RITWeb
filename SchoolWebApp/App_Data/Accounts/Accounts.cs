using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using System.Reflection;
using AccountsEntities;
using Utility;
using SchoolBusinessService;
using System.ServiceModel;
using BusinessLogic.Exceptions;

/// <summary>
/// Summary description for Accounts
/// </summary>
public class Accounts : SchoolBase
{

	#region -- Constructor --

	public Accounts()
	{
		base.InitializeMemberVariables();
	}
	
	#endregion 	
	

    /// <summary>
    ///		Exports a list of vouchers in open tally xml format.
    /// </summary>
    /// <param name="alstVouchers"></param>
    public static void ExportVoucherXML(List<Voucher> alstVouchers)
    {
        string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("ENVELOPE");
        XmlNode oXmlBody;
        XmlNode oXmlImportData;
        XmlNode oXmlRequestData;
        XmlNode oXmlTallyMessage;
        XmlNode oXmlVoucher;
        XmlNode oXmlLedger;
        XmlNode oXmlNode;
        XmlAttribute oXmlAttribute;
        oXmlBody = oDoc.CreateNode(S_ELEMENT, "BODY", string.Empty);
        oXmlImportData = oDoc.CreateNode(S_ELEMENT, "IMPORTDATA", string.Empty);
        oXmlRequestData = oDoc.CreateNode(S_ELEMENT, "REQUESTDATA", string.Empty);
        alstVouchers.ForEach(
            Voucher =>
            {
                oXmlTallyMessage = oDoc.CreateNode(S_ELEMENT, "TALLYMESSAGE", string.Empty);
                oXmlAttribute = oDoc.CreateAttribute("xmlns:UDF");
                oXmlAttribute.Value = "TallyUDF";
                oXmlTallyMessage.Attributes.Append(oXmlAttribute);

                oXmlVoucher = oDoc.CreateNode(S_ELEMENT, "VOUCHER", string.Empty);
                oXmlAttribute = oDoc.CreateAttribute("VCHTYPE");
                oXmlAttribute.Value = Voucher.VoucherType.Name;
                oXmlVoucher.Attributes.Append(oXmlAttribute);

                oXmlAttribute = oDoc.CreateAttribute("ACTION");
                oXmlAttribute.Value = "Create";
                oXmlVoucher.Attributes.Append(oXmlAttribute);

                oXmlNode = oDoc.CreateNode(S_ELEMENT, "DATE", string.Empty);
                oXmlNode.InnerText = Voucher.Date.ToString("yyyyMMdd");
                oXmlVoucher.AppendChild(oXmlNode);

                oXmlNode = oDoc.CreateNode(S_ELEMENT, "NARRATION", string.Empty);
                oXmlNode.InnerText = Voucher.Narration;
                oXmlVoucher.AppendChild(oXmlNode);

                oXmlNode = oDoc.CreateNode(S_ELEMENT, "VOUCHERTYPENAME", string.Empty);
                oXmlNode.InnerText = Voucher.VoucherType.Name;
                oXmlVoucher.AppendChild(oXmlNode);

                oXmlNode = oDoc.CreateNode(S_ELEMENT, "VOUCHERNUMBER", string.Empty);
                oXmlNode.InnerText = Voucher.SerialNumber;
                oXmlVoucher.AppendChild(oXmlNode);

                oXmlNode = oDoc.CreateNode(S_ELEMENT, "EFFECTIVEDATE", string.Empty);
                oXmlNode.InnerText = Voucher.Date.ToString("yyyyMMdd");
                oXmlVoucher.AppendChild(oXmlNode);

                Voucher.VoucherParticulars.ForEach(
                    VoucherPartucular =>
                    {
                        oXmlLedger = oDoc.CreateNode(S_ELEMENT, "ALLLEDGERENTRIES.LIST", string.Empty);

                        oXmlNode = oDoc.CreateNode(S_ELEMENT, "LEDGERNAME", string.Empty);
                        oXmlNode.InnerText = VoucherPartucular.Ledger.Name;
                        oXmlLedger.AppendChild(oXmlNode);

                        if (VoucherPartucular.IsDebit)
                        {
                            oXmlNode = oDoc.CreateNode(S_ELEMENT, "ISDEEMEDPOSITIVE", string.Empty);
                            oXmlNode.InnerText = "Yes";
                            oXmlLedger.AppendChild(oXmlNode);
                        }

                        oXmlNode = oDoc.CreateNode(S_ELEMENT, "AMOUNT", string.Empty);
                        oXmlNode.InnerText = VoucherPartucular.Amount.ToString();
                        oXmlLedger.AppendChild(oXmlNode);

                        oXmlVoucher.AppendChild(oXmlLedger);
                    }
                );

                oXmlTallyMessage.AppendChild(oXmlVoucher);
                oXmlRequestData.AppendChild(oXmlTallyMessage);
            }
        );

        oXmlImportData.AppendChild(oXmlRequestData);
        oXmlBody.AppendChild(oXmlImportData);
        root.AppendChild(oXmlBody);
        oDoc.AppendChild(root);

        string sVoucherXMLFilePath = HttpContext.Current.Server.MapPath("..") + "\\DOWNLOADS\\Voucher.xml";

        // File a file by the same name exists, delete it & Save the new XML file
        if (File.Exists(sVoucherXMLFilePath))
            File.Delete(sVoucherXMLFilePath);
        oDoc.Save(sVoucherXMLFilePath);
        HttpContext.Current.Response.ContentType = "text/xml";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=Voucher.xml");
        HttpContext.Current.Response.TransmitFile(sVoucherXMLFilePath);
        HttpContext.Current.Response.End();
    }

	/// <summary>
	///		Exports a single voucher in open tally xml format. 
	/// </summary>
	/// <param name="aoVoucher"></param>
	public static void ExportVoucherXML(Voucher aoVoucher)
	{
		ExportVoucherXML(new List<Voucher> { aoVoucher });
	}

	/// <summary>
	/// Records the fee payment in the Accounts module.
	/// </summary>
	/// <param name="aiStudentId"></param>
	/// <param name="asReceiptNo"></param>
	public void RecordCashPaymentForFeeConcession(int aiStudentId, string asReceiptNo)
	{
		// Create a fee voucher for the fees paid by the student
		AccountVoucherClient oVoucherClient = new AccountVoucherClient();
		try
		{
			oVoucherClient.Open();
			oVoucherClient.CreateFeeVoucherForCashPayment(miSchoolId, miAcademicYearId, miFinancialYearId, aiStudentId, asReceiptNo, miUserId);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
														string.Format("Accounts Module : An exception occured while recording a fee payment. StudentId : {0}. ReceiptNo : {1}",
																	aiStudentId,
																	asReceiptNo));
		}
		finally
		{
			if (oVoucherClient.State != CommunicationState.Faulted)
				oVoucherClient.Close();
		}
	}
}
