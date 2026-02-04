using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using SchoolBusinessService;
using System.ServiceModel;
using System.IO;
using System.Web;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;

/// <summary>
/// This class is used to give full concession for RTE student.
/// </summary>
public class StudentConcession : SchoolBase
{

	#region Constants

	private const string S_ELEMENT = "element";
	private const string S_STUDENT = "Student";

	#endregion Constants

	#region constructors

	public StudentConcession()
	{	
		InitializeMemberVariables();
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// This methid is used to pay 100% concession for RTE student.
	/// </summary>
	/// <param name="aiStudentId"></param>

		public void AddConcessionForRTEStudent(int aiStudentId)
		{
		   StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
		   DataSet oDSFeeDetails = oStudentFeeDetailsBL.GetStudentFeeDetails(aiStudentId, DateTime.Now);
		   DataRow oDRTotalAmount = oDSFeeDetails.Tables[Constants.I_TWO].Rows[Constants.I_ZERO];
		   int iTotalAmount = 0;
		   if (oDRTotalAmount["TotalAmtToBePaid"] != DBNull.Value)
			   iTotalAmount = oDRTotalAmount["TotalAmtToBePaid"].ToInt();	  
		   int iConcessionAmount = iTotalAmount;	   

		   DataTable oDTFeeDetails = oDSFeeDetails.Tables[Constants.I_ZERO];
		   string sStudentFeeIDs = string.Empty;
		   string sRemarks = string.Empty;
		   string sLateFeeDetails = string.Empty;
		   for (int iRowIndex = 0; iRowIndex < oDTFeeDetails.Rows.Count; iRowIndex++)
		   {
			   int iAmountPaid = oDTFeeDetails.Rows[iRowIndex][Constants.I_FIVE].ToInt();
				if(iAmountPaid != Constants.I_ZERO)
					continue;

				int iRowAmt = oDTFeeDetails.Rows[iRowIndex]["Amount"].ToInt();
				sStudentFeeIDs = sStudentFeeIDs + "," + oDTFeeDetails.Rows[iRowIndex]["Schoolwise_Student_Fee_Id"].ToString();
				if (!sRemarks.Contains(oDTFeeDetails.Rows[iRowIndex]["Payable_For"].ToString()))
					sRemarks = string.Format("{0}, {1}({2} - Rs. {3} /-)", sRemarks, oDTFeeDetails.Rows[iRowIndex]["Payable_For"].ToString(), oDTFeeDetails.Rows[iRowIndex]["Fee_Type"].ToString(), iRowAmt);

		   }
		   if (sStudentFeeIDs.StartsWith(","))
			   sStudentFeeIDs = sStudentFeeIDs.Substring(1);

		   if (sRemarks.StartsWith(","))
			   sRemarks = sRemarks.Substring(1);

		   sLateFeeDetails = GetXMLForLateFeeDetails();
		   sStudentFeeIDs = GetXMLForStudentFeeIds(sStudentFeeIDs);
		   oStudentFeeDetailsBL.PayStudentFee(0, 0, aiStudentId, sStudentFeeIDs, sRemarks, DateTime.Now, iConcessionAmount, sLateFeeDetails, 0, 'N', 0, 0, 0, string.Empty);

		   if(Settings.EnableAccountsModule)
			   RecordCashPayment(aiStudentId, oStudentFeeDetailsBL.Receipt_Number);
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

	   /// <summary>
	   /// This method generates XML for student fee ids.
	   /// </summary>
	   /// <param name="asStudentFeeIDs"></param>
	   /// <returns></returns>

	   private string GetXMLForStudentFeeIds(string asStudentFeeIDs)
	   {
		   const string S_STUDENT_FEE_ID = "Student_Fee_Id";
		   const string S_STUDENT_LATE_FEE = "Late_Fee_Amt";
		   const string S_STUDENT_FEE_LIST = "StudentFeeList";
		   var oDoc = new XmlDocument();
		   XmlElement root = oDoc.CreateElement(S_STUDENT_FEE_LIST);
		   XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT_FEE_LIST, string.Empty);

		   string sStudentFeeIds = asStudentFeeIDs;
		   string[] sArrStudentFeeId = sStudentFeeIds.Split(',');
		   int iLateFeeAmt;
		   string sAtrrName;
		   string sAtrrName1;
		   XmlAttribute attr;
		   int iStudentFeeId = 0;
		   for (int iCnt = 0; iCnt < sArrStudentFeeId.Length; iCnt++)
		   {
			   if(sArrStudentFeeId[iCnt] == string.Empty)
					continue;

			   iStudentFeeId = Convert.ToInt32(sArrStudentFeeId[iCnt]);
			   iLateFeeAmt = 0;			   

			   XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, string.Empty);

			   sAtrrName = S_STUDENT_FEE_ID;
			   attr = oDoc.CreateAttribute(sAtrrName);
			   attr.Value = iStudentFeeId.ToString();
			   oXmlNode.Attributes.Append(attr);
			   oXmlRootNode.AppendChild(oXmlNode);

			   sAtrrName1 = S_STUDENT_LATE_FEE;
			   XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
			   attr1.Value = iLateFeeAmt.ToString();
			   oXmlNode.Attributes.Append(attr1);
			   oXmlRootNode.AppendChild(oXmlNode);

		   }
		   root.AppendChild(oXmlRootNode);
		   return root.InnerXml;
	   }

	   /// <summary>
	   /// This method creates an XML for student fee id list.
	   /// </summary>
	   /// <returns></returns>
	   private string GetXMLForLateFeeDetails()
	   {
		   const string S_LATE_FEE_AMT = "Late_Fee_Amt";
		   const string S_LATE_FEE_DESC = "Late_Fee_Desc";

		   XmlDocument oDoc = new XmlDocument();
		   XmlElement root = oDoc.CreateElement("LateFeeDetails");
		   XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "LateFeeDetails", string.Empty);

		   XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "LateFee", string.Empty);

		   string sAtrrName = S_LATE_FEE_AMT;
		   XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		   attr.Value = Constants.S_ZERO;
		   oXmlNode.Attributes.Append(attr);

		   string sAtrrName1 = S_LATE_FEE_DESC;
		   XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
		   attr1.Value = string.Empty;
		   oXmlNode.Attributes.Append(attr1);

		   oXmlRootNode.AppendChild(oXmlNode);
		   root.AppendChild(oXmlRootNode);
		   return root.InnerXml;
	   }

	   #endregion
}