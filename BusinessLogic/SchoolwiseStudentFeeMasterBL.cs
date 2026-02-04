using System;
using System.Data;
using System.Collections;
using DataCommunicator;
using System.Collections.Generic;

namespace BusinessLogic
{

    public class SchoolwiseStudentFeeMasterBL
    {


        #region DataMembers and properties

        #region Data members

        private SchoolwiseStudentFeeMasterDC.SchoolwiseStudentFeeMasterStruct moSchoolwiseStudentFeeMasterStruct;
        private SchoolwiseStudentFeeMasterDC moSchoolwiseStudentFeeMasterDC = new SchoolwiseStudentFeeMasterDC();

        #endregion
        #region Properties

        public int SchoolwiseStudentFeeId
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miSchoolwiseStudentFeeId; }
            set { moSchoolwiseStudentFeeMasterStruct.miSchoolwiseStudentFeeId = value; }
        }

        public DateTime Paymentdate
        {

            get { return moSchoolwiseStudentFeeMasterStruct.mdtPaymentdate; }
            set { moSchoolwiseStudentFeeMasterStruct.mdtPaymentdate = value; }
        }

        public int YearwisestudentId
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId; }
            set { moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId = value; }
        }

        public int StandardFeeTypeId
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId; }
            set { moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId = value; }
        }

        public int DueAmount
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miDueAmount; }
            set { moSchoolwiseStudentFeeMasterStruct.miDueAmount = value; }
        }
         public int ConcessionAmount
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miConcessionAmount; }
            set { moSchoolwiseStudentFeeMasterStruct.miConcessionAmount = value; }
        }
        
        public int LateFeeAmount
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miLateFeeAmount; }
            set { moSchoolwiseStudentFeeMasterStruct.miLateFeeAmount = value; }
        }

        public int TotalFeeAmount
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miTotalFeeAmount; }
            set { moSchoolwiseStudentFeeMasterStruct.miTotalFeeAmount = value; }
        }

        public string ReceiptNumber
        {

            get { return moSchoolwiseStudentFeeMasterStruct.msReceiptNumber; }
            set { moSchoolwiseStudentFeeMasterStruct.msReceiptNumber = value; }
        }

        public int Interval
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miInterval; }
            set { moSchoolwiseStudentFeeMasterStruct.miInterval = value; }
        }

        public int SchoolId
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miSchoolId; }
            set { moSchoolwiseStudentFeeMasterStruct.miSchoolId = value; }
        }

        public int AcademicYearId
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miAcademicYearId; }
            set { moSchoolwiseStudentFeeMasterStruct.miAcademicYearId = value; }
        }

        public int InsertedById
        {

            get { return moSchoolwiseStudentFeeMasterStruct.miInsertedById; }
            set { moSchoolwiseStudentFeeMasterStruct.miInsertedById = value; }
        }

        public DateTime InsertDate
        {

            get { return moSchoolwiseStudentFeeMasterStruct.mdtInsertDate; }
            set { moSchoolwiseStudentFeeMasterStruct.mdtInsertDate = value; }
        }

        public int UpdatedById
        {
            get { return moSchoolwiseStudentFeeMasterStruct.miUpdatedById; }
            set { moSchoolwiseStudentFeeMasterStruct.miUpdatedById = value; }
        }

        public DateTime UpdateDate
        {
            get { return moSchoolwiseStudentFeeMasterStruct.mdtUpdateDate; }
            set { moSchoolwiseStudentFeeMasterStruct.mdtUpdateDate = value; }
        }

        public string Isdeleted
        {
            get { return moSchoolwiseStudentFeeMasterStruct.msIsdeleted; }
            set { moSchoolwiseStudentFeeMasterStruct.msIsdeleted = value; }
        }
        public string Description
        {
            get { return moSchoolwiseStudentFeeMasterStruct.msDescription; }
            set { moSchoolwiseStudentFeeMasterStruct.msDescription = value; }
        }

        public string ChequeNumber
        {
            get { return moSchoolwiseStudentFeeMasterStruct.msChequeNumber; }
            set { moSchoolwiseStudentFeeMasterStruct.msChequeNumber = value; }
        }

        public DateTime ChequeDate
        {
            get { return moSchoolwiseStudentFeeMasterStruct.mdtChequeDate; }
            set { moSchoolwiseStudentFeeMasterStruct.mdtChequeDate = value; }
        }

        public string BankName
        {
            get { return moSchoolwiseStudentFeeMasterStruct.msBankName; }
            set { moSchoolwiseStudentFeeMasterStruct.msBankName = value; }
        }

        public string Remarks
        {
            get { return moSchoolwiseStudentFeeMasterStruct.msRemarks; }
            set { moSchoolwiseStudentFeeMasterStruct.msRemarks = value; }
        }

        public string GUID
        {
            get { return moSchoolwiseStudentFeeMasterStruct.msGUID; }
            set { moSchoolwiseStudentFeeMasterStruct.msGUID = value; }
        }
        

        #endregion
        #endregion

        #region Constructors

        public SchoolwiseStudentFeeMasterBL()
        {
        }
        public SchoolwiseStudentFeeMasterBL(int aiId)
        {

            SchoolwiseStudentFeeMasterDC moSchoolwiseStudentFeeMasterDC = new SchoolwiseStudentFeeMasterDC(aiId);

        }
        #endregion

        #region Public Methods
        public DataSet GetIntervals()
        {
            moSchoolwiseStudentFeeMasterDC.SchoolwiseStudentFeeMasterStructDetails = moSchoolwiseStudentFeeMasterStruct;
            return moSchoolwiseStudentFeeMasterDC.GetIntervals();
        }
   
        public void InsertSchoolwiseStudentFeeMaster( Hashtable aoLateFee)
        {

            moSchoolwiseStudentFeeMasterDC.SchoolwiseStudentFeeMasterStructDetails = moSchoolwiseStudentFeeMasterStruct;
            moSchoolwiseStudentFeeMasterDC.InsertSchoolwiseStudentFeeMaster(aoLateFee);
        }
        public void UpdateSchoolwiseStudentFeeMaster()
        {

            moSchoolwiseStudentFeeMasterDC.SchoolwiseStudentFeeMasterStructDetails = moSchoolwiseStudentFeeMasterStruct;
            moSchoolwiseStudentFeeMasterDC.UpdateSchoolwiseStudentFeeMaster();
        }
        public void DeleteSchoolwiseStudentFeeMaster()
        {

            moSchoolwiseStudentFeeMasterDC.SchoolwiseStudentFeeMasterStructDetails = moSchoolwiseStudentFeeMasterStruct;
            moSchoolwiseStudentFeeMasterDC.DeleteSchoolwiseStudentFeeMaster();
        }
        public static DataSet GetFeeDetailsForStudent(int aiYearwiseStudentId)
        {
            return SchoolwiseStudentFeeMasterDC.GetFeeDetailsForStudent(aiYearwiseStudentId);
        }
        public static DataTable GetPaymentDetailsForReciept(int aiStudentFeesPaymentId)
        {
            return SchoolwiseStudentFeeMasterDC.GetPaymentDetailsForReciept(aiStudentFeesPaymentId);
        }
        public static DataSet GetPaymentDetailsForTermReciept(int aiSchoolId, int aiAcademicYearId, int aiStudentFeesPaymentId)
        {
            return SchoolwiseStudentFeeMasterDC.GetPaymentDetailsForTermReciept(aiSchoolId,aiAcademicYearId, aiStudentFeesPaymentId);
        }

        /// <summary>
        /// This method is used to get count of GUID. 
        /// </summary>
        /// <param name="asGUID"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static int GetGUIDCnt(string asGUID, int aiStudentId)
        {
            return SchoolwiseStudentFeeMasterDC.GetGUIDCnt(asGUID, aiStudentId);
        }

        /// <summary>
        /// This method is used to get last fee entry for particular student and for particular fee type.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiStdFeeTypeId"></param>
        /// <returns></returns>
        public static int GetLastFeeEntry(int aiStudentId, int aiStdFeeTypeId)
        {
            return SchoolwiseStudentFeeMasterDC.GetLastFeeEntry(aiStudentId, aiStdFeeTypeId);
        }

        /// <summary>
        /// This method is used to update remarks of a particular transaction.
        /// </summary>
        /// <param name="aiStudentFeeId"></param>
        /// <param name="asRemarks"></param>
        public static void UpdateStudentReamrks(int aiStudentFeeId, string asRemarks)
        {
            SchoolwiseStudentFeeMasterDC.UpdateStudentReamrks(aiStudentFeeId, asRemarks);
        }

        /// <summary>
        /// This method is used to delete student's fee details.
        /// </summary>
        /// <param name="aiStudentFeesPaymentId"></param>
        /// <param name="aiYrwise_Student_Id"></param>
        /// <param name="aiReceipt_No"></param>
        public static void DeleteStudentDetails(int aiStudentFeeId, int aiYrwiseStudentId,
                                                                                         string asReceipt_No)
        {
            SchoolwiseStudentFeeMasterDC.DeleteStudentDetails(aiStudentFeeId, aiYrwiseStudentId, asReceipt_No);
        }

        /// <summary>
        /// This method is used to delete student fee details if cheque is bounced.
        /// </summary>
        /// <param name="aiYrwiseStudentId"></param>
        /// <param name="aiIntervalCnt"></param>
        /// <param name="aiStdFeeId"></param>
        public static void DeleteStudentFeeDetails(int aiYrwiseStudentId, int aiIntervalCnt, int aiStdFeeId)
        {
            SchoolwiseStudentFeeMasterDC.DeleteStudentFeeDetails(aiYrwiseStudentId, aiIntervalCnt, aiStdFeeId);
        }

        public static DataTable GetFeeTypes(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId)
        {
            return SchoolwiseStudentFeeMasterDC.GetFeeTypes(aiSchoolId,aiAcademicYearId,aiStandardId,aiDivisionId);
        }

        /// <summary>
        /// This method is used to return all active ledger ids of selected financial year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <returns></returns>
        public static List<int> GetActiveLedgersIds(int aiSchoolId, int aiFinancialYearId)
        {
            return SchoolwiseStudentFeeMasterDC.GetActiveLedgersIds(aiSchoolId, aiFinancialYearId);
        }

        #endregion
    }

}
