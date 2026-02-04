// Class Name       :- SchoolwiseStudentFeeDetailsBL
// Purpose          :- This class is used to manage SchoolwiseStudentFeeDetails details.
// Date Of creation :- 9/19/2008
// Author Name      :- Anu

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Xml;
using BusinessLogic.Exceptions;
using DataCommunicator;
using FeeEntities;
using SchoolEntities.Accounts;
using SchoolEntities.StudentFee;
using Utility;
using SchoolEntities;
using StudentEntities;

namespace BusinessLogic
{
    public class StudentFeeDetailsBL
	{
		#region -- Constants --

		private const string S_ELEMENT = "element";
		private const string S_STUDENT = "Student";

		#endregion Constants

	    #region -- MEMBER(s) --

	    private StudentFeeDetailsDC.StudentFeeDetailsStruct moStudentFeeDetailsStruct;
	    private StudentFeeDetailsDC moStudentFeeDetailsDC;
	    public bool bCanSendSMS;
	    public string sMobileNumber;
	    public int iUserId;
	    public static string sDesignation;
		private string msChallanNumber;
        private int miSchoolId;
        private int miAcademicYearId;
        private int miStudentId;
        private int miUserId;
        public int miMode;
        public int miRowCount;

	    #endregion -- MEMBER(s) --

	    #region -- CONSTRUCTOR(s) --

       /// <summary>
        /// Initializes a new instance of the class.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiStudentId"></param>
       /// <param name="aiUserId"></param>
        public StudentFeeDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miStudentId = aiStudentId;
            this.miUserId = aiUserId;
            this.moStudentFeeDetailsDC = new StudentFeeDetailsDC(aiSchoolId, aiAcademicYearId, aiStudentId, aiUserId);
        }

	    public StudentFeeDetailsBL()
	    {
		    moStudentFeeDetailsDC = new StudentFeeDetailsDC();
		    moStudentFeeDetailsStruct = moStudentFeeDetailsDC.StudentFeeDetailsStructDetails;
	    }

	    public StudentFeeDetailsBL(int miStudentFeeId)
	    {
		    moStudentFeeDetailsDC = new StudentFeeDetailsDC(miStudentFeeId);
		    moStudentFeeDetailsStruct = moStudentFeeDetailsDC.StudentFeeDetailsStructDetails;
	    }

        public StudentFeeDetailsBL(int miStudentFeeId, bool abIsInternalFee)
        {
            moStudentFeeDetailsDC = new StudentFeeDetailsDC(miStudentFeeId, abIsInternalFee);
            moStudentFeeDetailsStruct = moStudentFeeDetailsDC.StudentFeeDetailsStructDetails;
        }

	    #endregion -- CONSTRUCTOR(s) --

	    #region -- PROPERTIES --

        public List<StudentPayFeeDetails> StudentPayFeeDetails
        {
            get { return moStudentFeeDetailsDC.StudentPayFeeDetails; }
            set { moStudentFeeDetailsDC.StudentPayFeeDetails = value; }
        }

        public List<ChequeDetails> ChequeDetails
        {
            get { return moStudentFeeDetailsDC.ChequeDetails; }
            set { moStudentFeeDetailsDC.ChequeDetails = value; }
        }

        public EditFeeDetails EditFeeDetails
        {
            get { return moStudentFeeDetailsDC.EditFeeDetails; }
            set { moStudentFeeDetailsDC.EditFeeDetails = value; }
        }

        public SwapCardDetails SwapCardDetails
        {
            get { return moStudentFeeDetailsDC.SwapCardDetails; }
            set { moStudentFeeDetailsDC.SwapCardDetails = value; }
        }

        public ElectronicPaymentDetails ElectronicPaymentDetails
        {
            get { return moStudentFeeDetailsDC.ElectronicPaymentDetails; }
            set { moStudentFeeDetailsDC.ElectronicPaymentDetails = value; }
        }

        public StudentPayFeeDetails StudentPayFeeDetail
        {
            get { return moStudentFeeDetailsDC.StudentPayFeeDetail; }
            set { moStudentFeeDetailsDC.StudentPayFeeDetail = value; }
        }

        public string sChallanNumber
        {
            get { return msChallanNumber; }
            set { msChallanNumber = value; }
        }

        public int Mode
        {
            get { return miMode; }
            set { miMode = value; }
        }

        public int DepositedBankId
        {
            get { return moStudentFeeDetailsDC.DepositedBankId; }
            set { moStudentFeeDetailsDC.DepositedBankId = value; }
        }

        public string sRemarks
        {
            get { return moStudentFeeDetailsDC.sRemarks; }
            set { moStudentFeeDetailsDC.sRemarks = value; }
        }
        
	    public bool CanSendSMS
	    {
		    get { return moStudentFeeDetailsDC.CanSendSMS; }
		    set { moStudentFeeDetailsDC.CanSendSMS = value; }
	    }

	    public string MobileNumber
	    {
		    get { return moStudentFeeDetailsDC.MobileNumber; }
		    set { moStudentFeeDetailsDC.MobileNumber = value; }
	    }

        public virtual int AccountHeaderId
        {
            get { return moStudentFeeDetailsStruct.miAccountHeaderId; }
            set { moStudentFeeDetailsStruct.miAccountHeaderId = value; }
        }

        public string AccountHeaderName
        {
            get { return moStudentFeeDetailsStruct.msAccountHeaderName; }
            set { moStudentFeeDetailsStruct.msAccountHeaderName = value; }
        }

        public int FeeDefaulterUserId
	    {
		    get { return moStudentFeeDetailsDC.FeeDefaulterUserId; }
		    set { moStudentFeeDetailsDC.FeeDefaulterUserId = value; }
	    }

        public DateTime PaymentDate
        {
            get { return moStudentFeeDetailsDC.PaymentDate; }
            set { moStudentFeeDetailsDC.PaymentDate = value; }
        }

	    public string Designation
	    {
		    get { return moStudentFeeDetailsDC.Designation; }
		    set { moStudentFeeDetailsDC.Designation = value; }
	    }

	    public virtual int Schoolwise_Student_Fee_Id
	    {
		    get { return moStudentFeeDetailsStruct.miSchoolwiseStudentFeeId; }
		    set { moStudentFeeDetailsStruct.miSchoolwiseStudentFeeId = value; }
	    }

	    public virtual int Student_Id
	    {
		    get { return moStudentFeeDetailsStruct.miStudentId; }
		    set { moStudentFeeDetailsStruct.miStudentId = value; }
	    }

        public virtual bool ConsiderRTEStudent
        {
            get { return moStudentFeeDetailsStruct.mbIncludeRTEStudent; }
            set { moStudentFeeDetailsStruct.mbIncludeRTEStudent = value; }
        }
	    public virtual string Payable_For
	    {
		    get { return moStudentFeeDetailsStruct.msPayableFor; }
		    set { moStudentFeeDetailsStruct.msPayableFor = value; }
	    }

	    public virtual int Standard_Div_Id
	    {
		    get { return moStudentFeeDetailsStruct.miStandardDivId; }
		    set { moStudentFeeDetailsStruct.miStandardDivId = value; }
	    }

	    public virtual int Std_FeeType_Id
	    {
		    get { return moStudentFeeDetailsStruct.miStdFeeTypeId; }
		    set { moStudentFeeDetailsStruct.miStdFeeTypeId = value; }
	    }

	    public virtual int Amount
	    {
		    get { return moStudentFeeDetailsStruct.miAmount; }
		    set { moStudentFeeDetailsStruct.miAmount = value; }
	    }

	    public virtual string DebitOrCredit
	    {
		    get { return moStudentFeeDetailsStruct.msDebitOrCredit; }
		    set { moStudentFeeDetailsStruct.msDebitOrCredit = value; }
	    }

	    public virtual DateTime Paid_Date
	    {
		    get { return moStudentFeeDetailsStruct.mdtPaidDate; }
		    set { moStudentFeeDetailsStruct.mdtPaidDate = value; }
	    }

	    public virtual string Receipt_Number
	    {
		    get { return moStudentFeeDetailsStruct.msReceiptNumber; }
		    set { moStudentFeeDetailsStruct.msReceiptNumber = value; }
	    }

	    public virtual string Remarks
	    {
		    get { return moStudentFeeDetailsStruct.msRemarks; }
		    set { moStudentFeeDetailsStruct.msRemarks = value; }
	    }

	    public virtual int Student_Fee_Id
	    {
		    get { return moStudentFeeDetailsStruct.miStudentFeeId; }
		    set { moStudentFeeDetailsStruct.miStudentFeeId = value; }
	    }

	    public virtual int School_Id
	    {
		    get { return moStudentFeeDetailsStruct.miSchoolId; }
		    set { moStudentFeeDetailsStruct.miSchoolId = value; }
	    }

	    public virtual int Academic_Year_Id
	    {
		    get { return moStudentFeeDetailsStruct.miAcademicYearId; }
		    set { moStudentFeeDetailsStruct.miAcademicYearId = value; }
	    }

	    public virtual string Is_Deleted
	    {
		    get { return moStudentFeeDetailsStruct.msIsDeleted; }
		    set { moStudentFeeDetailsStruct.msIsDeleted = value; }
	    }

	    public virtual string FeeType
	    {
		    get { return moStudentFeeDetailsStruct.msFeeType; }
		    set { moStudentFeeDetailsStruct.msFeeType = value; }
	    }

	    public virtual DateTime Insert_Date
	    {
		    get { return moStudentFeeDetailsStruct.mdtInsertDate; }
		    set { moStudentFeeDetailsStruct.mdtInsertDate = value; }
	    }

	    public virtual int Inserted_By_id
	    {
		    get { return moStudentFeeDetailsStruct.miInsertedByid; }
		    set { moStudentFeeDetailsStruct.miInsertedByid = value; }
	    }

	    public virtual DateTime Update_Date
	    {
		    get { return moStudentFeeDetailsStruct.mdtUpdateDate; }
		    set { moStudentFeeDetailsStruct.mdtUpdateDate = value; }
	    }

	    public virtual int Updated_By_Id
	    {
		    get { return moStudentFeeDetailsStruct.miUpdatedById; }
		    set { moStudentFeeDetailsStruct.miUpdatedById = value; }
	    }

	    public virtual int SerialNumber
	    {
		    get { return moStudentFeeDetailsStruct.miSerialNumber; }
		    set { moStudentFeeDetailsStruct.miSerialNumber = value; }
	    }

	    public virtual Collection<StudentFeeDetailsDC> oStudentFeeDetails
	    {
		    get { return moStudentFeeDetailsStruct.moStudentFeeDetailsDC; }
		    set { moStudentFeeDetailsStruct.moStudentFeeDetailsDC = value; }
	    }

        public int TotalAmount
        {
            get { return moStudentFeeDetailsDC.TotalAmount; }
            set { moStudentFeeDetailsDC.TotalAmount = value; }
        }

        public int LastChequeBank
        {
            get { return moStudentFeeDetailsDC.LastChequeBank; }
            set { moStudentFeeDetailsDC.LastChequeBank = value; }
        }

        public bool IsDueDateApplicable
        {
            get { return moStudentFeeDetailsStruct.mbIsDueDateApplicable; }
            set { moStudentFeeDetailsStruct.mbIsDueDateApplicable = value; }
        }

        public bool IsConsiderForOnlinePayment
        {
            get { return moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment; }
            set { moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment = value; }
        }

        public List<PayableForDetails> Intervals
        {
            get { return moStudentFeeDetailsDC.Intervals; }
        }

        public List<PayableForDetails> FeeDetails
        {
            get { return moStudentFeeDetailsDC.FeeDetails; }
        }

	    #endregion -- PROPERTIES --

		#region -- RTE Student Fee Concession --

		/// <summary>
        /// This methid is used to pay 100% concession for RTE student.
		/// </summary>
		/// <param name="aiStudentId"></param>
		/// <returns></returns>
		public string AddConcessionForRTEStudent(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
		{
            List<int> lstiStudentId = GetAllStudentId(aiStudentId, aiSchoolId, aiAcademicYearId);
			DataSet oDSFeeDetails = GetStudentFeeDetails(aiStudentId, DateTime.Now, Constants.I_ZERO);
            InternalFeeConcessionForRTEStudent(aiStudentId, aiSchoolId, aiAcademicYearId);
            //List<int> lstiStudentId = GetAllStudentId(aiStudentId, aiSchoolId, aiAcademicYearId);
            for(int iOuterIndex = 0 ; iOuterIndex < oDSFeeDetails.Tables[Constants.I_ZERO].Rows.Count; iOuterIndex++)
            {
                for (int iInnerIndex = 0; iInnerIndex < lstiStudentId.Count; iInnerIndex++)
                {
                    if(oDSFeeDetails.Tables[Constants.I_ZERO].Rows[iOuterIndex]["Schoolwise_Student_Fee_Id"].ToInt() == lstiStudentId[iInnerIndex])
                    {
                        oDSFeeDetails.Tables[Constants.I_TWO].Rows[Constants.I_ZERO]["TotalAmtToBePaid"] = oDSFeeDetails.Tables[Constants.I_TWO].Rows[Constants.I_ZERO]["TotalAmtToBePaid"].ToInt() - oDSFeeDetails.Tables[Constants.I_ZERO].Rows[iOuterIndex]["Amount"].ToInt();                        
                        oDSFeeDetails.Tables[Constants.I_ZERO].Rows[iOuterIndex].Delete();
                        break;
                    }
                }
            }
            oDSFeeDetails.Tables[Constants.I_ZERO].AcceptChanges();
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
				int iAmountPaid = oDTFeeDetails.Rows[iRowIndex]["Amount_Paid"].ToInt();
				if (iAmountPaid != Constants.I_ZERO)
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
			PayStudentFee(0, 0, aiStudentId, sStudentFeeIDs, sRemarks, DateTime.Now, iConcessionAmount, sLateFeeDetails, 0, 'N', 0, 0, 0, string.Empty);

			return Receipt_Number;
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
				if (sArrStudentFeeId[iCnt] == string.Empty)
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

		#region -- PUBLIC METHOD(s) --

        /// <summary>
        /// This method is used to get the fee details for next year fee payment.
        /// </summary>
        /// <param name="abIsNewSudent"></param>
        /// <param name="aiStandardID"></param>
        /// <returns></returns>
		public DataSet getStudentFeeDetailsForNextYear(bool abIsNewSudent, int aiStandardID)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			return moStudentFeeDetailsDC.getStudentFeeDetailsForNextYear(abIsNewSudent, aiStandardID);
		}
      
        /// <summary>
        /// This method is used to pay fee for next year using cheque,card & electronic payments.
        /// </summary>
        /// <param name="asFeeDetailsXML"></param>
        /// <param name="asPaymentDetailsXML"></param>
        /// <param name="aiLateAmount"></param>
        /// <param name="aiPaymentMode"></param>
        /// <param name="iSerialNo"></param>
        public void InsertStudentFeeDetailsForNextYear(string asFeeDetailsXML, string asPaymentDetailsXML, int aiLateAmount, int aiPaymentMode, out int iSerialNo, int aiConcessionAmount = 0)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
            moStudentFeeDetailsDC.InsertStudentFeeDetailsForNextYear(asFeeDetailsXML, asPaymentDetailsXML, aiLateAmount, aiPaymentMode, out iSerialNo, aiConcessionAmount);
		}

        /// <summary>
        /// This method is used to get payable amount of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public FeeSMS GetPayableAmount(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
        {
            return moStudentFeeDetailsDC.GetPayableAmount(aiStudentId, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get yearwise student Id by user id.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public int GetYearwiseStudentId(int aiUserId, int aiSchoolId, int aiAcademicYearId)
        {
            return moStudentFeeDetailsDC.GetYearwiseStudentId(aiUserId, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to pay fee for next year using cash payment
        /// </summary>
        /// <param name="asFeeDetailsXML"></param>
        /// <param name="aiLateAmount"></param>
        /// <param name="iSerialNo"></param>
        public void InsertStudentFeeDetailsForNextYear(string asFeeDetailsXML, int aiLateAmount, out int iSerialNo, int aiConcessionAmount = 0)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
            moStudentFeeDetailsDC.InsertStudentFeeDetailsForNextYear(asFeeDetailsXML, aiLateAmount, out iSerialNo, aiConcessionAmount);
		}

        /// <summary>
        /// This method is used to get account header details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<AccountHeaderDetails> GetAccountHeaderDetails(int aiSchoolId, bool abIsForInternal)
        {
            return moStudentFeeDetailsDC.GetAccountHeaderDetails(aiSchoolId, abIsForInternal);
        }

        
        /// <summary>
        /// This method is used to delete the next year fee payment details.
        /// </summary>
        /// <param name="aiSerialNo"></param>
        /// <param name="aiUserID"></param>
		public void DeleteFeeDetailsForNextYear(int aiSerialNo, int aiUserID)
		{
			moStudentFeeDetailsDC.DeleteFeeDetailsForNextYear(aiSerialNo, aiUserID);
		}

        /// <summary>
        /// This method is used to to get Account header id by using fee type.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <returns></returns>
        public int GetAccountHeaderIdByFeeType(int aiSchoolId, int aiStandardId, int aiFeeTypeId)
        {
            return moStudentFeeDetailsDC.GetAccountHeaderIdByFeeType(aiSchoolId, aiStandardId, aiFeeTypeId);
        }

        public DataTable GetAllAccountHeaderCombo(int aiSchoolId, int aiAcademicYearId, int aiOriginalFeeTypeId, int aiIsSchoolFee)
        {
            return moStudentFeeDetailsDC.GetAllAccountHeaderCombo(aiSchoolId, aiAcademicYearId, aiOriginalFeeTypeId, aiIsSchoolFee);
        }

        public void ResetReceiptNumber(int aiSchoolId, int aiAcademicYearId, int aiAccountHeaderId, DateTime Date, int aiOrderById, int aiIsInternalFee)
        {
            this.moStudentFeeDetailsDC.ResetReceiptNumber(aiSchoolId, aiAcademicYearId, aiAccountHeaderId, Date, aiOrderById, aiIsInternalFee);
        }


        /// <summary>
        /// This function is used to insert the SchoolwiseStudentFeeDetails Details from student payables screen.
        /// </summary>
		public void InsertStudentFeeDetails()
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentFeeDetails();
		}

        /// <summary>
        /// This function is used to insert the SchoolwiseStudentFeeDetails Details from student payables screen for selected standard.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
		public void InsertStudentFeeDetails(ArrayList oarrStdDivIdLst)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentFeeDetails(oarrStdDivIdLst);
		}

        /// <summary>
        /// This function is used to insert the SchoolwiseStudentFeeDetails Details from student payables screen for selected standard-division.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
		public void InsertStudentFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentFeeDetails(oarrStdDivIdLst, aiStandardId);
		}

		/// <summary>
        /// This method is used to insert studentfeedetails records for standard-division.
		/// </summary>
		/// <param name="oarrStdIdLst"></param>
		public void CopyStudentFeeDetails(ArrayList oarrStdIdLst)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.CopyStudentFeeDetails(oarrStdIdLst);
		}

		/// <summary>
        /// This method is used to insert studentfeedetails records for standard-division.
		/// </summary>
		/// <param name="oarrStdDivIdLst"></param>
		/// <param name="aiStandardId"></param>
		/// <param name="aiDivisionId"></param>
		public void InsertStudentFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId, int aiDivisionId)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentFeeDetails(oarrStdDivIdLst, aiStandardId, aiDivisionId);
		}

        /// <summary>
        ///  This method is used to insert internal studentfeedetails records for standard-division from student payables screen
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
		public void InsertStudentInternalFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId, int aiDivisionId)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentInternalFeeDetails(oarrStdDivIdLst, aiStandardId, aiDivisionId);
		}

        /// <summary>
        /// This method is used to insert internal studentfeedetails records for standard from CopyFeeConfigurationPopup.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
		public void InsertStudentInternalFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentInternalFeeDetails(oarrStdDivIdLst, aiStandardId);
		}

        /// <summary>
        /// This method is used to insert internal fee details for specific student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiPaymentTypeId"></param>
        public void InsertStudentInternalFeeDetails(int aiStudentId, int aiPaymentTypeId, int aiIsNewEntry, int aiPdcId)
        {
            moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
            moStudentFeeDetailsDC.InsertStudentInternalFeeDetails(aiStudentId, aiPaymentTypeId, aiIsNewEntry, aiPdcId);
        }

        /// <summary>
        /// This method is used to insert internal studentfeedetails records for standard-division from CopyFeeConfigurationPopup.
        /// </summary>
        /// <param name="oarrStdIdLst"></param>
		public void CopyStudentInternalFeeDetails(ArrayList oarrStdIdLst)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.CopyStudentInternalFeeDetails(oarrStdIdLst);
		}

        /// <summary>
        /// This method is used to insert internal studentfeedetails records for standard-division.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
		public void InsertStudentInternalFeeDetails(ArrayList oarrStdDivIdLst)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertStudentInternalFeeDetails(oarrStdDivIdLst);
		}

        /// <summary>
        /// This function is used to update the SchoolwiseStudentFeeDetails Details at the school level from student payable screen.
        /// </summary>
		public virtual void UpdateStudentFeeDetails()
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.UpdateStudentFeeDetails();
		}

        public void UpdateStudentInternalFeeDetails(int aiDebitId)
        {
            moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
            moStudentFeeDetailsDC.UpdateStudentInternalFeeDetails(aiDebitId);
        }

        /// <summary>
        /// This function is used to update the SchoolwiseStudentFeeDetails Details for the selected debit entry from student payables screen.
        /// </summary>
        /// <param name="aiDebitId"></param>
		public void UpdateStudentFeeDetails(int aiDebitId)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.UpdateStudentFeeDetails(aiDebitId);
		}

        /// <summary>
        /// This function is used update extra added fee details from selected standard division from student payables screen.
        /// </summary>
        /// <param name="oarrStdDivLst"></param>
        /// <param name="asIsUpdate"></param>
		public void UpdateStudentFeeDetails(ArrayList oarrStdDivLst, string asIsUpdate)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.UpdateStudentFeeDetails(oarrStdDivLst, asIsUpdate);
		}

        /// <summary>
        /// This function is used to update internal fee details from student payables screen.
        /// </summary>
        /// <param name="oarrStdDivLst"></param>
        /// <param name="asIsUpdate"></param>
		public void UpdateStudentInternalFeeDetails(ArrayList oarrStdDivLst, string asIsUpdate)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.UpdateStudentInternalFeeDetails(oarrStdDivLst, asIsUpdate);
		}

        /// <summary>
        /// This function is used to delete the standard level debit details added as extra fees from student payables screen.
        /// </summary>
        /// <param name="aiSerialNo"></param>
        /// <param name="oarrStdDivIdsList"></param>
        public void DeleteDebitFeeDetails(int aiSerialNo, ArrayList oarrStdDivIdsList, int aiUpdatedByid, bool abIsInternalFee, int aiSchoolId, int aiAcademicYearId)
		{
            moStudentFeeDetailsDC.DeleteDebitFeeDetails(aiSerialNo, oarrStdDivIdsList, aiUpdatedByid, abIsInternalFee, aiSchoolId, aiAcademicYearId);
		}

        /// <summary>
        /// This function is used to delete the SchoolwiseStudentFeeDetails Details for bounced cheques.
        /// </summary>
        /// <param name="aiDebitId"></param>
        public void DeleteStudentBounceChequeFeeDetails(int aiDebitId, bool abIsInternalFee = false)
		{
            moStudentFeeDetailsDC.DeleteStudentBounceChequeFeeDetails(aiDebitId, abIsInternalFee);
		}

        /// <summary>
        /// This function is used to delete the SchoolwiseStudentFeeDetails Details from Student Payable screen.
        /// </summary>
        /// <param name="aiDebitId"></param>
		public void DeleteStudentFeeDetails(int aiDebitId, int aiUpdatedByid)
		{
			moStudentFeeDetailsDC.DeleteStudentFeeDetails(aiDebitId, aiUpdatedByid);
		}

        /// <summary>
        /// This function is used to delete the SchoolwiseStudentFeeDetails Details from Student Payable screen.
        /// </summary>
        /// <param name="aiDebitId"></param>
        public void DeleteStudentInternalFeeDetails(int aiDebitId, int aiUpdatedByid)
        {
            moStudentFeeDetailsDC.DeleteStudentInternalFeeDetails(aiDebitId, aiUpdatedByid);
        }

        /// <summary>
        /// This function is used to permanent delete student's debit details from student payable screen.
        /// </summary>
		public virtual void DeleteStudentFeeDetails()
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.DeleteStudentDebitDetails();
		}

        /// <summary>
        /// This method is used to get debit details of a particular student by selecting a student from search grid of student payable screen.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
		public DataSet GetStudentDebitDetails(int aiStudentId)
		{
			return moStudentFeeDetailsDC.GetStudentDebitDetails(aiStudentId);
		}

        public DataSet GetInternalFeesChequeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            return moStudentFeeDetailsDC.GetInternalFeesChequeDetails(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

		/// <summary>
        /// This method is used to get debit details of a particular student from Student Payable screen.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYrId"> </param>
		/// <param name="abIncludeInternalFee"> </param>
		/// <returns></returns>
		public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, bool abIncludeInternalFee)
		{
			return moStudentFeeDetailsDC.GetDebitDetails(aiSchoolId, aiAcademicYrId, abIncludeInternalFee);
		}

        /// <summary>
        /// This method is used to get debit details of a particular student from Student Payable screen.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYrId"> </param>
        /// <param name="abIncludeInternalFee"> </param>
        /// <returns></returns>
        public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            return moStudentFeeDetailsDC.GetDebitDetails(aiSchoolId, aiAcademicYrId, aiStudentId);
        }

        public DataTable GetFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiMode, bool abIsInternalFee)
        {
            return moStudentFeeDetailsDC.GetFeeDetails(aiSchoolId, aiAcademicYearId, aiStudentId, aiMode, abIsInternalFee);
        }

        public DataTable GetPaymentClearanceNotification(int aischoolid,int aiacademicyearid)
        {
            return moStudentFeeDetailsDC.GetPaymentClearanceNotification(aischoolid,aiacademicyearid);
        }
        /// <summary>
        /// This method is used to get debit details of a particular standard and is used on Student Payable screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
		public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, int aiStandardId, bool abIncludeInternalFee)
		{
			return moStudentFeeDetailsDC.GetDebitDetails(aiSchoolId, aiAcademicYrId, aiStandardId, abIncludeInternalFee);
		}
		
		/// <summary>
        /// This procedure is used to get the correct payment mode for selected receipt number.
        /// </summary>
        /// <param name="asReceiptNumber"></param>
        /// <returns></returns>
        public Constants.FeePaymentType GetPaymentModeForReceipt(string asReceiptNumber, int aiAccountHeaderId)
        {
            return moStudentFeeDetailsDC.GetPaymentModeForReceipt(asReceiptNumber, aiAccountHeaderId);            
        }

        /// <summary>
        /// This method is used to get student fee details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aodtCurrentDate"></param>
        /// <param name="abShowOnlyDebits"></param>
        /// <returns></returns>
        public DataSet GetStudentFeeDetails(int aiStudentId, DateTime adtCurrentDate, int aiLoginUserRoleId, bool abShowOnlyDebits = false)
        {
            return moStudentFeeDetailsDC.GetStudentFeeDetails(aiStudentId, adtCurrentDate, aiLoginUserRoleId, abShowOnlyDebits);
        }

        /// <summary>
        /// This method is used to get student fee details.
        /// </summary>
        /// <param name="aiPaymentType"></param>
        /// <param name="abShowOnlyDebits"></param>
        /// <param name="aiReceiptNumber"></param>
        /// <returns></returns>
        public List<StudentPaidFeeDetails> GetStudentFeeDetails(DateTime adtCurrentDate,int aiPaymentType, bool abShowOnlyDebits = false, int aiReceiptNumber = 0)
        {
            return moStudentFeeDetailsDC.GetStudentFeeDetails(adtCurrentDate, aiPaymentType, abShowOnlyDebits, aiReceiptNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtDueDate"></param>
        /// <param name="aiPaymentType"></param>
        /// <returns></returns>
        public List<StudentPaidFeeDetails> GetStudentFeeDetailsForOnlinePartialPayment(string asDueDatesFilterXML, DateTime dtCurrentDate, int aiStudentId, int aiAcademicYearId)
        {
            return moStudentFeeDetailsDC.GetStudentFeeDetailsForOnlinePartialPayment(asDueDatesFilterXML, dtCurrentDate, aiStudentId, aiAcademicYearId);
        }

		public DataTable GetReceiptDetailsForStudent(int aiSchoolId, int aiAcademicYearId, string sReceiptNo, int aiStudentId)
        {
            return moStudentFeeDetailsDC.GetReceiptDetailsForStudent(aiSchoolId, aiAcademicYearId, sReceiptNo, aiStudentId);
        }

       /// <summary>
        /// This method is used to get debit details of a particular standard and is used on Student Payable screen.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYrId"></param>
       /// <param name="aiStandardId"></param>
       /// <param name="aiDivisionId"></param>
       /// <param name="abIncludeInternalFee"></param>
       /// <returns></returns>
		public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, int aiStandardId, int aiDivisionId, bool abIncludeInternalFee)
		{
			return moStudentFeeDetailsDC.GetDebitDetails(aiSchoolId, aiAcademicYrId, aiStandardId, aiDivisionId, abIncludeInternalFee);
		}

		/// <summary>
		/// This method is used to get student fee details.
		/// </summary>
		/// <param name="aiStudentId"></param>
		/// <param name="odtCurrentDate"> </param>
		/// <returns></returns>
		public DataSet GetStudentFeeDetails(int aiStudentId, DateTime odtCurrentDate, int aiLoginUserRoleId)
		{
			return moStudentFeeDetailsDC.GetStudentFeeDetails(aiStudentId, odtCurrentDate, aiLoginUserRoleId);
		}

        /// <summary>
        /// This method is used to get all student id's.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<int> GetAllStudentId(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
        {
            return moStudentFeeDetailsDC.GetAllStudentId(aiStudentId,aiSchoolId,aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to add concession for RTE Student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void InternalFeeConcessionForRTEStudent(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
        {
            moStudentFeeDetailsDC.InternalFeeConcessionForRTEStudent(aiStudentId, aiSchoolId, aiAcademicYearId);
        }
		/// <summary>
        /// This function is used to pay fee with cash.
		/// </summary>
		/// <param name="asStudentPayFeeXML"></param>
		/// <param name="asCreditDetailsXML"></param>
		/// <returns></returns>
        public string PayStudentFeeWithCash(string asStudentPayFeeXML, string asCreditDetailsXML)
        {
            string sReceiptNumber = string.Empty;

            if (!msChallanNumber.IsNullOrEmpty() && Mode == 0)
                CheckDuplicateChallanNo(miSchoolId, miAcademicYearId, msChallanNumber);

            sReceiptNumber = moStudentFeeDetailsDC.PayStudentFeeWithCash(asStudentPayFeeXML, asCreditDetailsXML);
            ActivateFeeDefaulter(miStudentId);
            GetStudentMobileNumber();
            return sReceiptNumber;
        }

        public string PayStudentFeeWithJournalVoucher(string asStudentPayFeeXML, string asCreditDetailsXML, int aiLedgerId)
        {
            string sReceiptNumber = string.Empty;
            sReceiptNumber = moStudentFeeDetailsDC.PayStudentFeeWithJournalVoucher(asStudentPayFeeXML, asCreditDetailsXML, aiLedgerId);
            ActivateFeeDefaulter(miStudentId);
            GetStudentMobileNumber();
            return sReceiptNumber;
        }

        /// <summary>
        /// This method is used to pay fee using cheque.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asChequeDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayStudentFeeWithCheque(string asStudentPayFeeXML, string asChequeDetailsXML, string asCreditDetailsXML)
        {
            moStudentFeeDetailsDC.PayStudentFeeWithCheque(asStudentPayFeeXML, asChequeDetailsXML, asCreditDetailsXML);
            ActivateFeeDefaulter(miStudentId);
            GetStudentMobileNumber();
        }

       /// <summary>
        /// This method is used to pay fee using PDC cheque.
       /// </summary>
       /// <param name="asStudentPayFeeXML"></param>
       /// <param name="asChequeDetailsXML"></param>
       /// <param name="asCreditDetailsXML"></param>
        public void PayStudentFeeWithPDC(string asStudentPayFeeXML, string asChequeDetailsXML, string asCreditDetailsXML)
        {
            moStudentFeeDetailsDC.PayStudentFeeWithPDC(asStudentPayFeeXML, asChequeDetailsXML, asCreditDetailsXML);
            ActivateFeeDefaulter(miStudentId);
            GetStudentMobileNumber();
        }       
 
        /// <summary>
        /// This procedure is used to pay fee using swap card.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asCardDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayStudentFeeWithCard(string asStudentPayFeeXML, string asCardDetailsXML, string asCreditDetailsXML)
        {
            moStudentFeeDetailsDC.PayStudentFeeWithCard(asStudentPayFeeXML, asCardDetailsXML, asCreditDetailsXML);
            ActivateFeeDefaulter(miStudentId);
            GetStudentMobileNumber();
        }

        /// <summary>
        /// This procedure is used to pay fee using electronic payments.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asCardDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayFeeWithElectronicMode(string asStudentPayFeeXML, string asElectronicPaymentXML, string asCreditDetailsXML)
        {
            moStudentFeeDetailsDC.PayFeeWithElectronicMode(asStudentPayFeeXML, asElectronicPaymentXML, asCreditDetailsXML);
            ActivateFeeDefaulter(miStudentId);
            GetStudentMobileNumber();
        }

        /// <summary>
        /// This method is used to add concession for the student from student UI .
        /// </summary>
        /// <param name="aiAmtToBePaid"></param>
        /// <param name="aiActualAmt"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asStudentFeeIdList"></param>
        /// <param name="asRemarks"></param>
        /// <param name="odtPaymentDate"></param>
        /// <param name="aiConcessionAmt"></param>
        /// <param name="asLateFeeDetails"></param>
        /// <param name="aiLateFeeAmt"></param>
        /// <param name="acIsDirectlyDeposited"></param>
        /// <param name="aiBankId"></param>
        /// <param name="aiLedgerId"></param>
        /// <param name="aiReceiptNo"></param>
        /// <param name="asChallanNo"></param>
		public void PayStudentFee(int aiAmtToBePaid, int aiActualAmt, int aiStudentId, string asStudentFeeIdList, string asRemarks, DateTime odtPaymentDate, int aiConcessionAmt, string asLateFeeDetails, int aiLateFeeAmt, char acIsDirectlyDeposited, int aiBankId, int aiLedgerId, int aiReceiptNo, string asChallanNo)
		{
			
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			
			if (!asChallanNo.IsNullOrEmpty())
				CheckDuplicateChallanNo(moStudentFeeDetailsStruct.miSchoolId, moStudentFeeDetailsStruct.miAcademicYearId, asChallanNo);

			moStudentFeeDetailsDC.PayStudentFee(aiAmtToBePaid, aiActualAmt, aiStudentId, asStudentFeeIdList, asRemarks, odtPaymentDate, aiConcessionAmt, asLateFeeDetails, aiLateFeeAmt, acIsDirectlyDeposited, aiBankId, aiLedgerId, aiReceiptNo, asChallanNo);
			
			moStudentFeeDetailsStruct = moStudentFeeDetailsDC.StudentFeeDetailsStructDetails;
			ActivateFeeDefaulter(aiStudentId);
		}

        /// <summary>
        /// This method is used to pay fee using cheques from import cheques screen of super admin.
        /// </summary>
        /// <param name="aiAmtToBePaid"></param>
        /// <param name="aiActualAmt"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asStudentFeeIdList"></param>
        /// <param name="asRemarks"></param>
        /// <param name="asChequeDetails"></param>
        /// <param name="odtPaymentDate"></param>
        /// <param name="aiConcessionAmt"></param>
        /// <param name="asLateFeeDetails"></param>
        /// <param name="aiLateFeeAmt"></param>
        /// <param name="acIsDirectlyDeposited"></param>
        /// <param name="aiBankId"></param>
        /// <param name="asCreditDetailsList"></param>
        /// <param name="aodtClearanceDate"></param>
		public void PayStudentFeeWithCheque(int aiAmtToBePaid, int aiActualAmt, int aiStudentId, string asStudentFeeIdList, string asRemarks, string asChequeDetails, DateTime odtPaymentDate, int aiConcessionAmt, string asLateFeeDetails, int aiLateFeeAmt, char acIsDirectlyDeposited, int aiBankId, string asCreditDetailsList, DateTime aodtClearanceDate)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.PayStudentFeeWithCheque(aiAmtToBePaid, aiActualAmt, aiStudentId, asStudentFeeIdList, asRemarks, asChequeDetails, odtPaymentDate, aiConcessionAmt, asLateFeeDetails, aiLateFeeAmt, acIsDirectlyDeposited, aiBankId, asCreditDetailsList, aodtClearanceDate);
			ActivateFeeDefaulter(aiStudentId);
		}

		/// <summary>
		/// This method is used to get particular receipt details.
		/// </summary>
		/// <param name="aiReceiptNo"></param>
		/// <param name="aiAcademicYearId"> </param>
		/// <returns></returns>
		public static DataSet GetReceiptDetails(int aiReceiptNo, int aiAcademicYearId)
		{
			return StudentFeeDetailsDC.GetReceiptDetails(aiReceiptNo, aiAcademicYearId);
		}

        /// <summary>
        /// This method is used to get particular receipt details.
        /// </summary>
        /// <param name="aiReceiptNo"></param>
        /// <param name="aiAcademicYearId"> </param>
        /// <returns></returns>
        public static DataSet GetReceiptDetailsForSNS(string asReceiptNo, int aiAcademicYearId, int aiAccountHeaderId, int aiStudentId, bool abIsRefundFee)
        {
            return StudentFeeDetailsDC.GetReceiptDetailsForSNS(asReceiptNo, aiAcademicYearId, aiAccountHeaderId, aiStudentId, abIsRefundFee);
        }

        public DataTable CheckStudentsStandardDetails(int aiReceiptNo, int aiSchoolId, int aiAcademicYearId)
        {
            StudentFeeDetailsDC oStudentFeeDetailsDC = new StudentFeeDetailsDC();
            return oStudentFeeDetailsDC.CheckStudentsStandardDetails(aiReceiptNo, aiSchoolId, aiAcademicYearId);
        }


        /// <summary>
        /// This method is used to get particular receipt details for nex year using serial number.
        /// </summary>
        /// <param name="aiReceiptNo"></param>
        /// <returns></returns>
		public static DataSet GetReceiptDetails(int aiSerialNo)
		{
			return StudentFeeDetailsDC.GetReceiptDetails(aiSerialNo);
		}

        /// <summary>
        /// This method is used to delete last credit entry of a particular student.
        /// </summary>
        /// <param name="iStudentId"></param>
        /// <param name="sReceiptNo"></param>
		public void DeleteLastCreditEntry(int iStudentId, string sReceiptNo, int aiAccountHeaderId, int aiUpdatedById)
		{
            moStudentFeeDetailsDC.DeleteLastCreditEntry(iStudentId, sReceiptNo, aiAccountHeaderId, aiUpdatedById);
		}

		/// <summary>
        /// This method is used to get late fee details.
		/// </summary>
		/// <param name="asStudentFeeIdsList"></param>
		/// <param name="oPaymentDate"></param>
		/// <returns></returns>
		public DataTable GetLateFeeDetails(string asStudentFeeIdsList, DateTime oPaymentDate)
		{
			return moStudentFeeDetailsDC.GetLateFeeDetails(asStudentFeeIdsList, oPaymentDate);
		}

		/// <summary>
		/// This method is used to get standardwise fee types.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <param name="aiStandardId"></param>
		/// <returns></returns>
        public DataTable GetStandardFeeType(int aiSchoolId, int aiAcademicYrId, int aiStandardId)
		{
            return moStudentFeeDetailsDC.GetStandardFeeType(aiSchoolId, aiAcademicYrId, aiStandardId);
		}

        /// <summary>
        /// This method is used to get payable for according to fee type.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiOriginalFeeTypeId"></param>
        /// <returns></returns>
        public DataTable GetFeeTypewisePayableFor(int aiSchoolId, int aiAcademicYearId, int aiOriginalFeeTypeId, int aiStandardId)
        {
            return moStudentFeeDetailsDC.GetFeeTypewisePayableFor(aiSchoolId, aiAcademicYearId, aiOriginalFeeTypeId, aiStandardId);
        }

		/// <summary>
		/// This method is used to get intervals according to standardwise fee type.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <param name="aiStdFeeTypeId"></param>
		/// <param name="aiStudentId"> </param>
		/// <returns></returns>
		public DataTable GetIntervalsWithAmount(int aiSchoolId, int aiAcademicYrId, int aiStdFeeTypeId, int aiStudentId)
		{
			return moStudentFeeDetailsDC.GetIntervalsWithAmount(aiSchoolId, aiAcademicYrId, aiStdFeeTypeId, aiStudentId);
		}

		/// <summary>
		/// This method is used to get intervals according to standardwise fee type.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <param name="aiStdFeeTypeId"></param>
		/// <returns></returns>
		public DataTable GetIntervals(int aiSchoolId, int aiAcademicYrId, int aiStdFeeTypeId)
		{
			return moStudentFeeDetailsDC.GetIntervals(aiSchoolId, aiAcademicYrId, aiStdFeeTypeId);
		}
		
		/// <summary>
        /// This method is used to get intervals according to standardwise fee type.
        /// </summary>       
        /// <param name="aiStdFeeTypeId"></param>        
        /// <param name="asStudentDeeTypeIds"> </param>
        /// <param name="abIsExcess"> </param>
        /// <returns></returns>
        public List<StudentPaidFeeDetails> GetIntervals(int aiStdFeeTypeId, string asStudentDeeTypeIds, bool abIsExcess)
        {
            return moStudentFeeDetailsDC.GetIntervals( aiStdFeeTypeId, asStudentDeeTypeIds, abIsExcess);
        }

		/// <summary>
		/// This method is used to get intervals according to standardwise fee type.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <param name="aiStdFeeTypeId"></param>
		/// <param name="aiStudentID"> </param>
		/// <param name="asStudentDeeTypeIds"> </param>
		/// <param name="abIsExcess"> </param>
		/// <returns></returns>
		public DataTable GetIntervals(int aiSchoolId, int aiAcademicYrId, int aiStdFeeTypeId, int aiStudentID, string asStudentDeeTypeIds, bool abIsExcess)
		{
			return moStudentFeeDetailsDC.GetIntervals(aiSchoolId, aiAcademicYrId, aiStdFeeTypeId, aiStudentID, asStudentDeeTypeIds, abIsExcess);
		}

        /// <summary>
        ///		Returns the receipt number for the given PDC payment from student payable screen.
        /// </summary>
        /// <param name="aiYearwiseStudentId">Yearwise Student Id.</param>
        /// <param name="aiPDCId">Id of the PDC payment.</param>
        /// <returns>Receipt Number</returns>
		public int GetReceiptNoForPDCPayment(int aiYearwiseStudentId, int aiPDCId, int aiMode)
		{
            return moStudentFeeDetailsDC.GetReceiptNoForPDCPayment(aiYearwiseStudentId, aiPDCId, aiMode);
		}

        /// <summary>
        /// This method is used to rollback all transations if cheque deposited is bounced from student payable screen.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiPDCId"></param>
        /// <param name="asBouncedChequeDetails"></param>
		public void RollBackIfChequeIsBounce(int aiStudentId, int aiPDCId, string asBouncedChequeDetails, int aiMode)
		{
			moStudentFeeDetailsDC.RollBackIfChequeIsBounce(aiStudentId, aiPDCId, asBouncedChequeDetails, aiMode);
		}

		/// <summary>
        /// This method is used to get total amount paid by given student.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <param name="aistudentId"></param>
		/// <param name="aiYear"></param>
		/// <returns></returns>
        public string GetTotalAmtForITConciliationRpt(int aiSchoolId, int aiAcademicYrId, int aistudentId, int aiYear, int aiSelectedAcademicYearId)
		{
            return moStudentFeeDetailsDC.GetTotalAmtForITConciliationRpt(aiSchoolId, aiAcademicYrId, aistudentId, aiYear, aiSelectedAcademicYearId);
		}

		/// <summary>
		/// This method is used to get student caution money details for show note message.
		/// </summary>
		/// <param name="aiStudentId"> </param>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public DataTable GetStudentCautionMoneyDetails(int aiStudentId, int aiSchoolId)
		{
			return moStudentFeeDetailsDC.GetStudentCautionMoneyDetails(aiStudentId, aiSchoolId);
		}

        /// <summary>
        /// This method is used to get fee refund details of student on fee refund UI.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <returns></returns>
		public DataSet GetStudentRefundDetails(int aiStudentId, int aiSchoolId, int aiAcademicYearID)
		{
			return moStudentFeeDetailsDC.GetStudentRefundDetails(aiStudentId, aiSchoolId, aiAcademicYearID);
		}

        /// <summary>
        /// This method is used to insert refund fee details from fee refund screen.
        /// </summary>
        /// <param name="aiTotalAmount"></param>
        /// <param name="asRefundFeeDetails"></param>
		public DataTable InsertRefundFeeDetails(int aiTotalAmount, string asRefundFeeDetails)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			return moStudentFeeDetailsDC.InsertRefundFeeDetails(aiTotalAmount, asRefundFeeDetails);
		}

        /// <summary>
        /// This method is used to insert refund fee details from fee refund screen.
        /// </summary>
        /// <param name="adtChequeDate"></param>
        /// <param name="aiChequeNumber"></param>
        /// <param name="aiBankId"></param>
        /// <param name="aiTotalAmount"></param>
        /// <param name="asRefundFeeDetails"></param>
		public void InsertRefundFeeDetails(DateTime adtChequeDate, int aiChequeNumber, int aiBankId, int aiTotalAmount, string asRefundFeeDetails)
		{
			moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
			moStudentFeeDetailsDC.InsertRefundFeeDetails(adtChequeDate, aiChequeNumber, aiBankId, aiTotalAmount, asRefundFeeDetails);
		}

        /// <summary>
        /// This method is used to delete the refunded entries from fee base screen.
        /// </summary>
        /// <param name="aiRefundFeeDetailsID"></param>
		public void DeleteRefundFeeDetails(int aiRefundFeeDetailsID)
		{
			moStudentFeeDetailsDC.DeleteRefundFeeDetails(aiRefundFeeDetailsID);
		}

        /// <summary>
        /// This method is used to return the online fee payment details on student login to pay fee online.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcdYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asDueDatesFilter"></param>
        /// <returns></returns>
        public DataSet GetFeeDetailsForOnlineFee(int aiSchoolId, int aiAcdYrId, int aiStudentId, string asDueDatesFilter, int aiSchoolwiseStudentFeeId, bool abIsCautionMoneyOnlinePayment, bool abIsInternalFeeOnlinePayment)
		{
            return moStudentFeeDetailsDC.GetFeeDetailsForOnlineFee(aiSchoolId, aiAcdYrId, aiStudentId, asDueDatesFilter, aiSchoolwiseStudentFeeId, abIsCautionMoneyOnlinePayment, abIsInternalFeeOnlinePayment);
		}

        /// <summary>
        /// This method is call from Incomplete TransactionUI to get the incomplete fee transaction for selected criteria.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRegNo"></param>
        /// <param name="asTransactionDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataTable GetInCompleteTransaction(int aiSchoolId, int aiAcademicYearId, string asRegNo, DateTime asTransactionDate, string asPaymentCategoryFeeId, string sortExpression, bool IsIncomplete, int maximumRows, int startRowIndex)
		{
			int iStartIndex = startRowIndex;
			int iEndIndex = iStartIndex + maximumRows;

            if (asPaymentCategoryFeeId == null) 
                asPaymentCategoryFeeId = Constants.S_ZERO;

            DataTable oDt = StudentFeeDetailsDC.GetInCompleteTransaction(aiSchoolId, aiAcademicYearId, asRegNo, asTransactionDate, asPaymentCategoryFeeId, sortExpression,IsIncomplete, iEndIndex, iStartIndex);
            if (oDt != null && oDt.Rows.Count > 0)
                miRowCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            
               
            return oDt;
		}

        /// <summary>
        /// This method is used to get all the incompleted admission transaction which is accessed from Incomplete transactionUI.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRegNo"></param>
        /// <param name="asTransactionDate"></param>
        /// <returns></returns>
        public int CountRowsOfInCompleteTransaction(int aiSchoolId, int aiAcademicYearId, string asRegNo, DateTime asTransactionDate, string asPaymentCategoryFeeId, bool IsIncomplete)
		{
            return miRowCount;
		}

        /// <summary>
        /// This method is used to get all the incompleted admission transaction which is accessed from Incomplete transactionUI.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asMobileNumber"></param>
        /// <param name="asTransactionDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static DataTable GetInCompleteAdmissionTransaction(int aiSchoolId, int aiAcademicYearId, string asMobileNumber, DateTime asTransactionDate, string sortExpression, bool IsIncomplete, int maximumRows, int startRowIndex)
		{
			int iStartIndex = startRowIndex;
			int iEndIndex = iStartIndex + maximumRows;
            if (asMobileNumber == null) asMobileNumber = "";
            return StudentFeeDetailsDC.GetInCompleteAdmissionTransaction(aiSchoolId, aiAcademicYearId, asMobileNumber, asTransactionDate, sortExpression, IsIncomplete, iEndIndex, iStartIndex);
		}

        /// <summary>
        /// This method is used to get all the incompleted admission transaction which is accessed from Incomplete transactionUI.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asMobileNumber"></param>
        /// <param name="asTransactionDate"></param>
        /// <returns></returns>
        public static int CountRowsOfInCompleteAdmissionTransaction(int aiSchoolId, int aiAcademicYearId, string asMobileNumber, DateTime asTransactionDate, bool IsIncomplete)
		{
            return StudentFeeDetailsDC.CountRowsOfInCompleteAdmissionTransaction(aiSchoolId, aiAcademicYearId, asMobileNumber, asTransactionDate, IsIncomplete);
		}

        /// <summary>
        /// This method is used to get all extra fee details for a selected standard to copy fee configuration from one standard to another.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSerialNumber"></param>
        /// <returns></returns>
		public static DataTable GetStandardFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiSerialNumber)
		{
			return StudentFeeDetailsDC.GetStandardFeeDetails(aiSchoolId, aiAcademicYearId, aiSerialNumber);
		}


        /// <summary>
        /// This method is used to get account header id by using serial number.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSerialNumber"></param>
        /// <returns></returns>
        public static int GetAccountHeaderIdBySerialNo(int aiSchoolId, int aiAcademicYearId, int aiSerialNumber, int aiIsForInternalFee)
        {
            return StudentFeeDetailsDC.GetAccountHeaderIdBySerialNo(aiSchoolId, aiAcademicYearId, aiSerialNumber, aiIsForInternalFee);
        }

        /// <summary>
        /// This method is used to get the fee details for a selected standard to copy fee configuration from one standard to another.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardID"></param>
        /// <param name="asFeeType"></param>
        /// <param name="asPayableFor"></param>
        /// <returns></returns>
		public static DataTable GetStandardListForFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStandardID, string asFeeType, string asPayableFor)
		{
			return StudentFeeDetailsDC.GetStandardListForFeeDetails(aiSchoolId, aiAcademicYearId, aiStandardID, asFeeType, asPayableFor);
		}

        /// <summary>
        /// This returns all the data required to show the fees mini receipt.
        /// </summary>
        /// <param name="aiSubmissionID"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
		public static DataSet GetAdmissionReceiptDetails(int aiSubmissionID, int aiAcademicYearId)
		{
			return StudentFeeDetailsDC.GetAdmissionReceiptDetails(aiSubmissionID, aiAcademicYearId);
		}

        /// <summary>
        /// This method is used to fill all the Bank related combos from multiple screens.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet GetBankDetailsForNetBanking(int aiSchoolId)
		{
			return moStudentFeeDetailsDC.GetBankDetailsForNetBanking(aiSchoolId);
		}

        /// <summary>
        /// This method is used to check whether the given cheque number is alreday exists or not.
        /// </summary>
        /// <param name="aiBankId"></param>
        /// <param name="aiChequeNumber"></param>
        /// <returns></returns>
		public bool IsDuplicateChequeNumber(int aiBankId, int aiChequeNumber)
		{
			return moStudentFeeDetailsDC.IsDuplicateChequeNumber(aiBankId, aiChequeNumber);
		}

        /// <summary>
        /// This method is used to check whether the given card number is alreday exists or not.
        /// </summary>
        /// <param name="asCardNumber"></param>
        /// <returns></returns>
		public bool IsDuplicateCardNumber(string asCardNumber)
		{
			return moStudentFeeDetailsDC.IsDuplicateCardNumber(asCardNumber);
		}

        /// <summary>
        /// This method is used to check if the Txn number is duplicated for the current student or not.
        /// </summary>
        /// <param name="asTxnNumber"></param>
        /// <returns></returns>
        public bool IsDuplicateTxnNumberForNextYear(string asTxnNumber)
        {
            return moStudentFeeDetailsDC.IsDuplicateTxnNumberForNextYear(asTxnNumber);
        }

        /// <summary>
        /// This method is used to display fee details on the Pay fee for next year popup on pagelaod.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aodtCurrentDate"></param>
        /// <param name="AcademicYearId"></param>
        /// <param name="StudentId"></param>
        /// <returns></returns>
		public static DataTable GetFeeDetailsForDisplay(int aiStudentId, DateTime aodtCurrentDate, out int AcademicYearId, out int StudentId)
		{
			return StudentFeeDetailsDC.GetFeeDetailsForDisplay(aiStudentId, aodtCurrentDate, out AcademicYearId, out StudentId);
		}

        /// <summary>
        /// This method is used to get the recipt numbers to show the previous receipts generated for the student.
        /// </summary>
        /// <param name="aiPaymentMode"></param>
        /// <returns></returns>
        public DataTable GetReceiptNoToUpdate(int aiPaymentMode)
		{
			return moStudentFeeDetailsDC.GetReceiptNoToUpdate(aiPaymentMode);
		}

        /// <summary>
        /// This method is used to get the receipt number to print the receipt from Pay fee popup screen.
        /// </summary>
        /// <param name="iStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <returns></returns>
		public int GetReceiptNo(int iStudentId, int aiSchoolId, int aiAcademicYrId, int aiFeeTypeId)
		{
			return StudentFeeDetailsDC.GetReceiptNo(iStudentId, aiSchoolId, aiAcademicYrId, aiFeeTypeId);
		}

        /// <summary>
        /// This method is used to show previous year pending fee messege on the base screen of fee.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
		public static string PreviousFeesPending(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
		{
            return StudentFeeDetailsDC.PreviousFeesPending(aiSchoolId, aiAcademicYrId, aiStudentId);
		}

        /// <summary>
        /// This method is used to check student student is presnet or absent as per configureds days.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static string IsStudentAbsent(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            return StudentFeeDetailsDC.IsStudentAbsent(aiSchoolId, aiAcademicYrId, aiStudentId);  
        }

        /// <summary>
        /// This method is used to show previous year pending fee messege on the base screen of fee.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static bool PreviousInternalFeesPending(int aiSchoolId, int aiAcademicYrId, int aiStudentId, out string aiAcademicYear)
        {
            return StudentFeeDetailsDC.PreviousInternalFeesPending(aiSchoolId, aiAcademicYrId, aiStudentId, out aiAcademicYear);
        }

        /// <summary>
        /// This method is used to get remark for selected receipt.
        /// </summary>
        /// <param name="aiReceiptNo"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <returns></returns>
		public static string GetRemark(int aiReceiptNo, int aiSchoolID, int aiAcademicYearID)
		{
			return StudentFeeDetailsDC.GetRemark(aiReceiptNo, aiSchoolID, aiAcademicYearID);
		}       

        /// <summary>
        /// This method is used to check that selected student from search grid is on leave or not.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
		public static string IsOnLeave(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
		{
			return StudentFeeDetailsDC.IsOnLeave(aiStudentId, aiSchoolId, aiAcademicYearId);
		}

		/// <summary>
        /// Checks if the given Challan No already exists in the system. Will throw a DuplicateName exception if it already exists in the system.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="asChallanNo"></param>
		public static void CheckDuplicateChallanNo(int aiSchoolId, int aiAcademicYearId, string asChallanNo)
		{
			CheckDuplicateChallanNo(aiSchoolId, aiAcademicYearId, asChallanNo, -1);
		}

		/// <summary>
        /// Checks if the given Challan No already exists in the system. Will throw a DuplicateName exception if it already exists in the system.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="asChallanNo"></param>
		/// <param name="aiReceiptNo"></param>
		public static void CheckDuplicateChallanNo(int aiSchoolId, int aiAcademicYearId, string asChallanNo, int aiReceiptNo)
		{
			if (StudentFeeDetailsDC.IsDuplicateChallanNo(aiSchoolId, aiAcademicYearId, asChallanNo, aiReceiptNo == -1 ? string.Empty : aiReceiptNo.ToString()))
				throw new DuplicateName("Challan No. should not be duplicated.");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abIncludeInternalFees"></param>
        /// <param name="abIncludeCautionMoney"></param>
        /// <returns></returns>
		public FeeCollection GetFeeCollectionDetails(int aiSchoolId, int aiAcademicYearId, bool abIncludeInternalFees, bool abIncludeCautionMoney)
		{
			return moStudentFeeDetailsDC.GetFeeCollectionDetails(aiSchoolId, aiAcademicYearId, abIncludeInternalFees, abIncludeCautionMoney);
		}

        /// <summary>
        /// This method is used to check if there exists a pending fee for a student. And is used to block progress report from progress report screen.
        /// </summary>
        /// <returns></returns>
		public bool PendingFeesAvailableForStudent()
        {
            moStudentFeeDetailsDC.StudentFeeDetailsStructDetails = moStudentFeeDetailsStruct;
            return moStudentFeeDetailsDC.PendingFeesAvailableForStudent();
        }

        /// <summary>
        /// This method is used to load other fee types from student payables screen.
        /// </summary>
        /// <param name="aiSchoold"></param>
        /// <returns></returns>
		public List<string> GetOtherFeeTypes(int aiSchoold, bool abIsINternalFee)
		{
            return moStudentFeeDetailsDC.GetOtherFeeTypes(aiSchoold, abIsINternalFee);
		}

        /// <summary>
        /// This method will be used to get electronic transaction types.
        /// </summary>
        /// <returns></returns>
        public List<ElectronicPaymentType> GetElectronicPaymentTypes()
        {
            return moStudentFeeDetailsDC.GetElectronicPaymentTypes();
        }

        /// <summary>
        /// This method will be used to check whether the transaction number is duplicated for NEFT RTGS transaction.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateElectronicTxn(string asTxnNumber, Constants.PaymentMode aoMode)
        {
            return moStudentFeeDetailsDC.IsDuplicateElectronicTxn(asTxnNumber, aoMode);
        }

        /// <summary>
        /// This method is used to get the electronic payment details on clearance details screen.
        /// </summary>
        /// <param name="aoFeeClearanceFilters"></param>
        /// <returns></returns>
        public List<StudentFeeClearanceDetails> GetElectronicPayments(FeeClearanceFilters aoFeeClearanceFilters, bool abIsInternalFee)
        {
            return moStudentFeeDetailsDC.GetElectronicPayments(aoFeeClearanceFilters, abIsInternalFee);
        }

        /// <summary>
        /// This method is used to get the electronic payment details on clearance details screen.
        /// </summary>
        /// <param name="aoFeeClearanceFilters"></param>
        /// <returns></returns>
        public int GetElectronicPaymentsCount(FeeClearanceFilters aoFeeClearanceFilters)
        {
            return moStudentFeeDetailsDC.GetElectronicPaymentsCount(aoFeeClearanceFilters);
        }

        /// <summary>
        /// This method is used to update electronic payment clearance details.
        /// </summary>
        /// <param name="asElectronicPaymentXML"></param>
        public void UpdateElectronicPaymentCautionMoneyClearance(string asElectronicPaymentXML)
        {
            moStudentFeeDetailsDC.UpdateElectronicPaymentCautionMoneyClearance(asElectronicPaymentXML);
        }

        /// <summary>
        /// This method is used to update electronic payment clearance details.
        /// </summary>
        /// <param name="asElectronicPaymentXML"></param>
        public void UpdateElectronicPaymentClearance(string asElectronicPaymentXML, bool abIsInternalFee)
        {
            moStudentFeeDetailsDC.UpdateElectronicPaymentClearance(asElectronicPaymentXML, abIsInternalFee);
        }

        /// <summary>
        /// This method is used to update electronic payment clearance details.
        /// </summary>
        /// <param name="aiFeeTypeId"></param>
        /// <param name="asStandardIds"></param>
        public List<FeeStandards> GetAllFeeDetailsForExport(string asStandardIds, int aiFeeTypeId)
        {
            return this.moStudentFeeDetailsDC.GetAllFeeDetailsForExport(asStandardIds, aiFeeTypeId);
        }

        public bool ShowInauguralCertificateOption(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId, int aiStandardId, int aiStandardDivisionId)
        {
            return this.moStudentFeeDetailsDC.ShowInauguralCertificateOption(aiSchoolId, aiAcademicYearId, aiSchoolwiseStudentId, aiStandardId, aiStandardDivisionId);
        }

		#endregion -- PUBLIC METHOD(s) --

	    #region -- PRIVATE METHOD(s) --

	    /// <summary>
	    ///	Activates the student if there are no more fees pending for which deactivation settings do not apply to the user.
	    /// </summary>
	    /// <param name="aiStudentId">YearwiseStudentId of the student to be activated.</param>
	    private void ActivateFeeDefaulter(int aiStudentId)
	    {
		    try
		    {
			    moStudentFeeDetailsDC.ActivateFeeDefaulter(aiStudentId);
		    }
		    catch (Exception ex)
		    {
			    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), string.Format("There was an error activating the student after fee payment. StudentId : {0}", aiStudentId));
		    }
	    }

        /// <summary>
        /// This method is used to get the mobile numbers of student after fee payment.
        /// </summary>
        private void GetStudentMobileNumber()
        {
            moStudentFeeDetailsDC.GetStudentMobileNumber();
        }


	    #endregion -- PRIVATE METHOD(s) --
       
        public DataTable GetFinalAcademicYearDetails(int aiSchoolId, int aiStudentId)
        {
            return moStudentFeeDetailsDC.GetFinalAcademicYearDetails(aiSchoolId, aiStudentId);
        }

        public decimal GetFullPaymentConcessionAmount(int aiTotalAmount, bool abIsNewStudent, int aiStandardId)
        {
            return moStudentFeeDetailsDC.GetFullPaymentConcessionAmount(aiTotalAmount, abIsNewStudent, aiStandardId);
        }

        public string GetConcessionMessage(int aiStandardId, bool abIsForStudentLogin = false)
        {
            return moStudentFeeDetailsDC.GetConcessionMessage(aiStandardId, abIsForStudentLogin);
        }

        public List<FeeDetailsToExport> GetFeeDetailsToExport(int aiStandardId, int aiDivisionId, int aiStudentId)
        {
            return moStudentFeeDetailsDC.GetFeeDetailsToExport(aiStandardId, aiDivisionId, aiStudentId);
        }

        public List<FeeLedger> GetAllFeeLedgers(int aiStudentId, int aiStandardId, int aiDivisionId)
        {
            return moStudentFeeDetailsDC.GetAllFeeLedgers(aiStudentId,aiStandardId,aiDivisionId);
        }

        public int GetStudentDetails(int aiSchoolId, int aiAcademicYearId, string asReceiptNo)
        {
            return moStudentFeeDetailsDC.GetStudentDetails(aiSchoolId, aiAcademicYearId, asReceiptNo);
        }

        /// <summary>
        /// This method is used to return fee details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<SchoolEntities.Student> GetStudentAllFeesDetails(int aiStandardId, int aiDivisionId, string aiStartDate, string aiEndDate)
        {
            return moStudentFeeDetailsDC.GetStudentAllFeesDetails(aiStandardId, aiDivisionId, aiStartDate, aiEndDate);  //
        }


        public DataSet GetLastYearFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            return moStudentFeeDetailsDC.GetLastYearFeeDetails(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

        public bool IsLastYearPendingFeeExist(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            return moStudentFeeDetailsDC.IsLastYearPendingFeeExist(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

        public void DisableOrDeleteUnpaidFee(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, string asSerialNumber, Boolean abIsDisable, int aiUpdatedById)
        {
            moStudentFeeDetailsDC.DisableOrDeleteUnpaidFee(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asSerialNumber, abIsDisable, aiUpdatedById);
        }

        public PaidFeeDetails GetAllFeeDetailsForVP(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asStandardDivisionId, int aiStudentId, DateTime adFromDate, DateTime adToDate)
        {
            return moStudentFeeDetailsDC.GetAllFeeDetailsForVP(aiSchoolId, aiAcademicYearId, asStandardId, asStandardDivisionId, aiStudentId, adFromDate, adToDate);
        }

        public InternalPaidFeeExamDetails GetCompetitiveExamwiseDetails(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asDivisionId)
        {
            return moStudentFeeDetailsDC.GetCompetitiveExamwiseDetails(aiSchoolId, aiAcademicYearId, asStandardId,asDivisionId);
        }

        /// <summary>
        /// these method is used for get pending student count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asStandardId"></param>
        /// <param name="asDivisionId"></param>
        /// <returns></returns>
        public StudentsAcademicYearwisePendingFeeCountDetails GetYearwisePendingFeeStudent(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asDivisionId)
        {
            return moStudentFeeDetailsDC.GetYearwisePendingFeeStudent(aiSchoolId, aiAcademicYearId, asStandardId, asDivisionId);
        }
    }

	public class StudentFeeDetailsCollectionBL
    {
        #region Public Methods

        /// <summary>
        /// This method is used to update all debit entries if any fee type get changed
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <param name="aiAmount"></param>
        /// <param name="abIsStudentPayFee"></param>
        public static void UpdateDebitEntries(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiFeeTypeId, int aiAmount, bool abIsStudentPayFee)
        {
            StudentFeeDetailsCollectionDC.UpdateDebitEntries(aiSchoolId, aiAcademicYearId, aiStandardId, aiFeeTypeId, aiAmount, abIsStudentPayFee);
        }

        /// <summary>
        /// This method is used to update fees if any fees against selected fee type gets increased or decreased.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <param name="aiAmount"></param>
        /// <param name="abIsStudentPayFee"></param>
        /// <param name="adDueDate"></param>
        /// <param name="aiAmountForNewStudent"></param>
        /// <param name="aiAmountForOldStudent"></param>
        public static void UpdateDebitEntries(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiFeeTypeId, int aiAmount, bool abIsStudentPayFee, DateTime adDueDate, int aiAmountForNewStudent, int aiAmountForOldStudent)
        {
            StudentFeeDetailsCollectionDC.UpdateDebitEntries(aiSchoolId, aiAcademicYearId, aiStandardId, aiFeeTypeId, aiAmount, abIsStudentPayFee, adDueDate, aiAmountForNewStudent, aiAmountForOldStudent);
        }

        #endregion
    }
}