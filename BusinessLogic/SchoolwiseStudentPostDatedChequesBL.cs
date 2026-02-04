// Class Name       :- SchoolwiseStudentPostDatedChequesBL
// Purpose          :- This class is used to manage SchoolwiseStudentPostDatedCheques details.
// Date Of creation :- 9/18/2008
// Author Name      :- 


using System;
using System.Data;
using DataCommunicator;
using System.Collections.Generic;
using System.Collections;

namespace BusinessLogic
{


    public class StudentPostDatedChequesBL
    {

        private StudentPostDatedChequesDC.StudentPostDatedChequesStruct moStudentPostDatedChequesStruct;

        private StudentPostDatedChequesDC moStudentPostDatedChequesDC;

        public StudentPostDatedChequesBL()
        {
            moStudentPostDatedChequesDC = new StudentPostDatedChequesDC();
        }

        public StudentPostDatedChequesBL(int miPostDatedChequeId)
        {
            moStudentPostDatedChequesDC = new StudentPostDatedChequesDC(miPostDatedChequeId);
            moStudentPostDatedChequesStruct = moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails;
        }

        public int PostDated_Cheque_Id
        {
            get
            {
                return moStudentPostDatedChequesStruct.miPostDatedChequeId;
            }
            set
            {
                moStudentPostDatedChequesStruct.miPostDatedChequeId = value;
            }
        }

        public int Student_Id
        {
            get
            {
                return moStudentPostDatedChequesStruct.miStudentId;
            }
            set
            {
                moStudentPostDatedChequesStruct.miStudentId = value;
            }
        }

        public string Cheque_Number
        {
            get
            {
                return moStudentPostDatedChequesStruct.msChequeNumber;
            }
            set
            {
                moStudentPostDatedChequesStruct.msChequeNumber = value;
            }
        }

        public System.DateTime Cheque_Date
        {
            get
            {
                return moStudentPostDatedChequesStruct.mdtChequeDate;
            }
            set
            {
                moStudentPostDatedChequesStruct.mdtChequeDate = value;
            }
        }

        public System.DateTime Cheque_Passed_Date
        {
            get
            {
                return moStudentPostDatedChequesStruct.mdtChequePassedDate;
            }
            set
            {
                moStudentPostDatedChequesStruct.mdtChequePassedDate = value;
            }
        }


        public int Bank_Id
        {
            get
            {
                return moStudentPostDatedChequesStruct.miBankId;
            }
            set
            {
                moStudentPostDatedChequesStruct.miBankId = value;
            }
        }

        public string Remarks
        {
            get
            {
                return moStudentPostDatedChequesStruct.msRemarks;
            }
            set
            {
                moStudentPostDatedChequesStruct.msRemarks = value;
            }
        }

        public int Cheque_Amount
        {
            get
            {
                return moStudentPostDatedChequesStruct.miChequeAmount;
            }
            set
            {
                moStudentPostDatedChequesStruct.miChequeAmount = value;
            }
        }

        public string Is_Deleted
        {
            get
            {
                return moStudentPostDatedChequesStruct.msIsDeleted;
            }
            set
            {
                moStudentPostDatedChequesStruct.msIsDeleted = value;
            }
        }

        public System.DateTime Insert_Date
        {
            get
            {
                return moStudentPostDatedChequesStruct.mdtInsertDate;
            }
            set
            {
                moStudentPostDatedChequesStruct.mdtInsertDate = value;
            }
        }

        public int Inserted_By_id
        {
            get
            {
                return moStudentPostDatedChequesStruct.miInsertedByid;
            }
            set
            {
                moStudentPostDatedChequesStruct.miInsertedByid = value;
            }
        }

        public System.DateTime Update_Date
        {
            get
            {
                return moStudentPostDatedChequesStruct.mdtUpdateDate;
            }
            set
            {
                moStudentPostDatedChequesStruct.mdtUpdateDate = value;
            }
        }

        public int Updated_By_Id
        {
            get
            {
                return moStudentPostDatedChequesStruct.miUpdatedById;
            }
            set
            {
                moStudentPostDatedChequesStruct.miUpdatedById = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moStudentPostDatedChequesStruct.miSchool_Id;
            }
            set
            {
                moStudentPostDatedChequesStruct.miSchool_Id = value;
            }
        }

        public int AcademicYrId
        {
            get
            {
                return moStudentPostDatedChequesStruct.miAcademicYr_Id;
            }
            set
            {
                moStudentPostDatedChequesStruct.miAcademicYr_Id = value;
            }
        }

        public string Enrolment_Number
        {
            get
            {
                return moStudentPostDatedChequesStruct.miEnrolment_Number;
            }
            set
            {
                moStudentPostDatedChequesStruct.miEnrolment_Number = value;
            }
        }




        public void InsertStudentPostDatedCheques()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            moStudentPostDatedChequesDC.InsertStudentPostDatedCheques();
        }

        public void UpdateStudentPostDatedCheques()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            moStudentPostDatedChequesDC.UpdateStudentPostDatedCheques();
        }

        public void DeleteStudentPostDatedCheques()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            moStudentPostDatedChequesDC.DeleteStudentPostDatedCheques();
        }

        /// <summary>
        /// This method is used to get postdated cheque details of a particular student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetStudentPostDatedChequeDetails(int aiStudentId, int aiSchoolId, int aiAcademicYrId)
        {
            return StudentPostDatedChequesDC.GetStudentPostDatedChequeDetails(aiStudentId, aiSchoolId, aiAcademicYrId);
        }

        /// <summary>
        /// This method is used to delete particular cheque entry logically.
        /// </summary>
        /// <param name="aiPostDatedChequeId"></param>
        public static void DeleteChequeDetails(int aiPostDatedChequeId)
        {
            StudentPostDatedChequesDC.DeleteChequeDetails(aiPostDatedChequeId);
        }

        /// <summary>
        /// This method is used to get student fee details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataSet GetStudentChequeDetails(int aiStudentId)
        {
            return moStudentPostDatedChequesDC.GetStudentChequeDetails(aiStudentId);
        }

        /// <summary>
        /// This method is used to check for duplicate cheque number.
        /// </summary>
        /// <param name="sChequeNo"></param>
        /// <returns></returns>
        public bool IsChequeNoDuplicate(string sChequeNo, int iStudentId)
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            return moStudentPostDatedChequesDC.IsChequeNoDuplicate(sChequeNo, iStudentId);
        }

        /// <summary>
        /// This method is used to check for duplicate cheque number.
        /// </summary>
        /// <param name="sChequeNo"></param>
        /// <returns></returns>
        public bool IsChequeNoDuplicate(string sChequeNo, int iStudentId, int aiReceiptNo, int aiAcademicYrId)
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            return moStudentPostDatedChequesDC.IsChequeNoDuplicate(sChequeNo, iStudentId, aiReceiptNo, aiAcademicYrId);
        }

        /// <summary>
        /// This method is used to check for duplicate cheque number.
        /// </summary>
        /// <param name="sChequeNo"></param>
        /// <returns></returns>
        public bool IsSwapNoDuplicate(string sSwapNo, int iStudentId)
        {
            return moStudentPostDatedChequesDC.IsSwapNoDuplicate(sSwapNo, iStudentId);
        }

        /// <summary>
        /// This method is used to check for duplicate cheque number.
        /// </summary>
        /// <param name="sChequeNo"></param>
        /// <returns></returns>
        public bool IsSwapNoDuplicate(string sSwapNo, int iStudentId,int aiReceiptNo, int aiAcademicYrId)
        {
            return moStudentPostDatedChequesDC.IsSwapNoDuplicate(sSwapNo, iStudentId, aiReceiptNo, aiAcademicYrId);
        }
        
        /// <summary>
        /// Method sets the cheque clearance/passed date to cheque.
        /// </summary>
        public void SetChequeClearance()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            moStudentPostDatedChequesDC.SetChequeClearance();
        }

        /// <summary>
        /// Method sets the cheque clearance/passed date to cheque.
        /// </summary>
        public void DeleteChequeClearance()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            moStudentPostDatedChequesDC.DeleteChequeClearance();
        }

        public void SetChequeClearanceDate(String asXML, bool abIsInternalFee)
        {
            StudentPostDatedChequesDC oStudentPostDatedChequesDC = new StudentPostDatedChequesDC();
            //oStudentPostDatedChequesDC.SetChequeClearanceDate(oArrayList);
            oStudentPostDatedChequesDC.UpdateStudentPostDatedChequeDetails(asXML, abIsInternalFee);
        }
        public void SetCautionClearanceDate(string asXML)
        {
            StudentPostDatedChequesDC oStudentPostDatedChequesDC = new StudentPostDatedChequesDC();
            oStudentPostDatedChequesDC.UpdateStudentCautionMoneyChequeDetails(asXML);
        }

        private string GetUpdateStaementForClearanceList()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            return moStudentPostDatedChequesDC.GetUpdateStaementForClearanceList();
        }

        private string GetDeleteStaementForClearanceList()
        {
            moStudentPostDatedChequesDC.StudentPostDatedChequesStructDetails = moStudentPostDatedChequesStruct;
            return moStudentPostDatedChequesDC.GetDeleteStaementForClearanceList();
        }

        public static DataTable IsDuplicateChequeNo(string asXML, bool abIsInternalFee)
        {
            return StudentPostDatedChequesDC.IsDuplicateChequeNo(asXML, abIsInternalFee);
        }

    }

    public class StudenChequesCollectionBL
    {

        // This function is used to Fetch the SchoolwiseStudentPostDatedCheques Details
        public static DataTable FetchChequesDetails(string asFilter, int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney, bool abSearchByChequeNo,  out int TotalAmount, bool abIsInternalFee)
        {
            return StudentChequesCollectionDC.FetchChequesDetails(asFilter, aiSchoolId, aiAcademicYrId, abIncludeAllCheques, abCautionMoney, abSearchByChequeNo, out TotalAmount, abIsInternalFee);
        }

        // This function is used to Fetch the SchoolwiseStudentPostDatedCheques Details
        public static DataTable FetchChequesDetails(DateTime adtStartDate, DateTime adtEndDate, int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney, bool abIsPaymentDate,  out int TotalAmount, bool abIsInternalFee,  int aiBankId = 0)
        {
            return StudentChequesCollectionDC.FetchChequesDetails(adtStartDate, adtEndDate, aiSchoolId, aiAcademicYrId, abIncludeAllCheques, abCautionMoney, abIsPaymentDate, out TotalAmount, abIsInternalFee, aiBankId);
        }

        public static DataTable FetchChequesDetails(int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney, out int TotalAmount, bool abIsInternalFee)
        {
            return StudentChequesCollectionDC.FetchChequesDetails(aiSchoolId, aiAcademicYrId, abIncludeAllCheques, abCautionMoney, out TotalAmount, abIsInternalFee);
        }

               
    }
}
