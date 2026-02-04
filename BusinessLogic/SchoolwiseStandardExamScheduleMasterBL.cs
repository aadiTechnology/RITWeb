// File Name       : SchoolwiseStandardExamScheduleMasterDC
// Purpose         : This class is used to manage SchoolwiseStandardExamScheduleMaster details.
// Date Of creation: 2/2/2008
// Author Name     : 

using System;
using System.Data;
using Utility;
using DataCommunicator;
using BookEntities;
using System.Collections.Generic;

namespace BusinessLogic
{


    public class SchoolwiseStandardExamScheduleMasterBL
    {


        private SchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStruct moSchoolwiseStandardExamScheduleMasterStruct;

        private SchoolwiseStandardExamScheduleMasterDC moSchoolwiseStandardExamScheduleMasterDC = new SchoolwiseStandardExamScheduleMasterDC();

        public SchoolwiseStandardExamScheduleMasterBL()
        {

        }

        public SchoolwiseStandardExamScheduleMasterBL(int miSchoolwiseStandardExamScheduleId)
        {
            moSchoolwiseStandardExamScheduleMasterDC = new SchoolwiseStandardExamScheduleMasterDC(miSchoolwiseStandardExamScheduleId);
            moSchoolwiseStandardExamScheduleMasterStruct = moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails;
        }

        public SchoolwiseStandardExamScheduleMasterBL(int aiStandardId, int aiTestId)
        {
            moSchoolwiseStandardExamScheduleMasterDC = new SchoolwiseStandardExamScheduleMasterDC(aiStandardId, aiTestId);
            moSchoolwiseStandardExamScheduleMasterStruct = moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails;
        }

        public SchoolwiseStandardExamScheduleMasterBL(int aiStandardId, int aiTestId, int aiSubjectId)
        {
            moSchoolwiseStandardExamScheduleMasterDC = new SchoolwiseStandardExamScheduleMasterDC(aiStandardId, aiTestId, aiSubjectId);
            moSchoolwiseStandardExamScheduleMasterStruct = moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails;
        }



        public int Schoolwise_Standard_Exam_Schedule_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId = value;
            }
        }

        public DateTime SubjectExamStartDate
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.mdSubjectExamStartDate;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.mdSubjectExamStartDate = value;
            }
        }
        public int Standard_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miStandardId;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miStandardId = value;
            }
        }

        public int Standard_Test_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miStandardTestId;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miStandardTestId = value;
            }
        }

        public int SchoolWise_Test_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miSchoolWiseTestId;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miSchoolWiseTestId = value;
            }
        }

        public DateTime Exam_Start_Date
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate = value;
            }
        }

        public DateTime Exam_End_Date
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate = value;
            }
        }

        public int Total_Exam_Days
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miTotalExamDays;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miTotalExamDays = value;
            }
        }

        public string Instructions
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.msInstructions;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.msInstructions = value;
            }
        }

        public int School_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId = value;
            }
        }

        public int academic_Year_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId = value;
            }
        }

        public string Is_Deleted
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.msIsDeleted;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime Insert_Date
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.mdtInsertDate;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.mdtInsertDate = value;
            }
        }

        public string Inserted_By_id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.msInsertedByid;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.msInsertedByid = value;
            }
        }

        public DateTime Update_Date
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.mdtUpdateDate = value;
            }
        }

        public string Updated_By_Id
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.msUpdatedById;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.msUpdatedById = value;
            }
        }

        public string Exam_Details
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct.msExamDetails;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct.msExamDetails = value;
            }
        }

      
        /// <summary>
        /// This method is used to insert record in SchoolwiseStandardExamScheduleMaster table.
        /// </summary>
        public int InsertSchoolwiseStandardExamScheduleMaster()
        {
            string sErrMsg = CheckDependencies();
            if (String.IsNullOrEmpty(sErrMsg))
            {
                moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
                return moSchoolwiseStandardExamScheduleMasterDC.InsertSchoolwiseStandardExamScheduleMaster();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sErrMsg);
            }
        }
        //This method use to getstandrds to which given exam applicable
        public static List<ClassDetails> GetStandards(int aiSchoolId, int aiAcademicYearId, int aiExmaid)
        {
            return SchoolwiseStandardExamScheduleMasterDC.GetStandards(aiSchoolId, aiAcademicYearId, aiExmaid);
        }


        public void UpdateSchoolwiseStandardExamScheduleMaster(bool bCheckDependancies)
        {
            string sErrMsg = CheckDependencies();
            if (String.IsNullOrEmpty(sErrMsg) || !bCheckDependancies)
            {
                moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
                moSchoolwiseStandardExamScheduleMasterDC.UpdateSchoolwiseStandardExamScheduleMaster();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sErrMsg);
            }
        }

        public void UpdateExamScheduleInstruction()
        {
            string sErrMsg = CheckDependencies();
            if (String.IsNullOrEmpty(sErrMsg))
            {
                moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
                moSchoolwiseStandardExamScheduleMasterDC.UpdateExamScheduleInstruction();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sErrMsg);
            }
        }

        /// <summary>
        /// This method returns the message informing about the dependencies.
        /// </summary>
        /// <returns></returns>
        public string CheckDependencies()
        {
            string sReturn = "";
            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.ExamSchedule), moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId, "Exam schedule ", moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId);
            return sReturn;
        }
        public void DeleteSchoolwiseStandardExamScheduleMaster()
        {

            moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
            moSchoolwiseStandardExamScheduleMasterDC.DeleteSchoolwiseStandardExamScheduleMaster();
        }


        public DataSet InsertExamScheduleDetails()
        {
            moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
            return moSchoolwiseStandardExamScheduleMasterDC.InsertExamScheduleDetails();
        }

        /// <summary>
        /// This method is used to submit or Unsubmit the exam Schedule.
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="abIsUnsubmit"></param>
        /// <param name="aiSchoolwiseTestId"></param>
        /// </summary>
        public void SubmitExamSchedule(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStandardId, bool abIsUnsubmit, int aiSchoolwiseTestId)
        {
            moSchoolwiseStandardExamScheduleMasterDC.SubmitExamSchedule(aiSchoolId, aiAcademicYearId, aiUserId, aiStandardId, abIsUnsubmit, aiSchoolwiseTestId);
        }

        /// <summary>
        /// This method is used to check perdefined start and end date.
        /// </summary>
        public void IsStartAndEndDatePredefined(int aiStandard_Id)
        {
            moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
            Int32 ipredefinedDateCount = moSchoolwiseStandardExamScheduleMasterDC.IsExamStartAndEndDatePredefined(aiStandard_Id);
            if (ipredefinedDateCount != Constants.I_ZERO)
            {
                throw new BusinessLogic.Exceptions.PreDefinedStartAndEndDate("Exam date(s) are overlapping with other exam of this standard.");
            }
        }

        /// <summary>
        /// This method is used to fill StandardwiseExamSchedule grid.
        /// </summary>
        public DataTable FillStandardwiseExamScheduleGrid(int aiStandardId)
        {
            moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
            return moSchoolwiseStandardExamScheduleMasterDC.FillStandardwiseExamScheduleGrid(aiStandardId);
        }

        public static DataSet GetStdExamSchedule(int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolwiseStandardExamScheduleMasterDC.GetStdExamSchedule(aiSchoolId, aiAcademicYearId);
        }


        public DataSet GetStandardwiseExamSchedule(int aiStandardId, int aiDivisionId)
        {
            moSchoolwiseStandardExamScheduleMasterDC.SchoolwiseStandardExamScheduleMasterStructDetails = moSchoolwiseStandardExamScheduleMasterStruct;
            return moSchoolwiseStandardExamScheduleMasterDC.GetStandardwiseExamSchedule(aiStandardId, aiDivisionId);
        }
        public static DataSet GetStandardwiseExamScheduleForTeacher(int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolwiseStandardExamScheduleMasterDC.GetStandardwiseExamScheduleForTeacher(aiSchoolId, aiAcademicYearId);
        }

        public void CopyExamScheduleToSelectedStandards(int aiSchoolId, int aiAcademicYearId, int aiSourceStandardId, int aiSourceStandardTestId, string asTargetStandardXml)
        {
           moSchoolwiseStandardExamScheduleMasterDC.CopyExamScheduleToSelectedStandards(aiSchoolId, aiAcademicYearId, aiSourceStandardId, aiSourceStandardTestId, asTargetStandardXml);
        }
    }

}
