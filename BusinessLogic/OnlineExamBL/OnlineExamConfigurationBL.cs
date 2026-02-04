using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;
using System.Data;
using SchoolEntities.Admin;
namespace BusinessLogic
{
   public class OnlineExamConfigurationBL
    {
        #region Data members

        private OnlineExamConfigurationDC moOnlineExamWiseQueConfigDC;
        private int miRecordCount;
        #endregion
        #region Constructors

        public OnlineExamConfigurationBL()
        {
            this.moOnlineExamWiseQueConfigDC = new OnlineExamConfigurationDC();
        }
       
        public OnlineExamConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moOnlineExamWiseQueConfigDC = new OnlineExamConfigurationDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        public DataTable GetAllQuestions(int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            return moOnlineExamWiseQueConfigDC.GetAllQuestions(aiStandardId, aiStandardDivisionId, aiSubjectId);
        }
        public DataSet GetDetailsForUpdateQuestions(int aiVehicleId)
        {
            return moOnlineExamWiseQueConfigDC.GetDetailsForUpdateQuestions(aiVehicleId);
        }
        public DataTable GetAllTestsForClass()
        {
            return moOnlineExamWiseQueConfigDC.GetAllTestsForClass();
        }

        public DataTable GetAllTestsForStudent(int aiStudentId)
        {
            return moOnlineExamWiseQueConfigDC.GetAllTestsForStudent(aiStudentId);
        }


        public DataTable GetAssociatedStandards(int aiStandardId) // pass Standardid 
        {
            return moOnlineExamWiseQueConfigDC.GetAssociatedStandards(aiStandardId);
        }
        public DataTable GetAllStandards()
        {
            return moOnlineExamWiseQueConfigDC.GetAllStandards();
        }
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects(int aiStdId, int aiStdDivId)
        {
            return moOnlineExamWiseQueConfigDC.GetAllYearwiseSubjects(aiStdId,aiStdDivId);
        }
        public void Save(string asStaffXML, OnlineExamConfiguration oExamConfig, int aiStandardId)
        {
            moOnlineExamWiseQueConfigDC.Save(asStaffXML, oExamConfig, aiStandardId);
        }
        public void Delete(int aiVehicleId)
        {
            moOnlineExamWiseQueConfigDC.Delete(aiVehicleId);
        }

        //public static DataSet GetDetailsForUpdateQuestions(int aiVehicleId, int aiSchoolID, int aiAcademicYearId)
        //{
        //    return OnlineExamQuestionConfigurationDC.GetDetailsForUpdateQuestions(aiVehicleId, aiSchoolID, aiAcademicYearId);
        //}
        public List<OnlineExamConfiguration> GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId, String sortExpression,string sortDirection, int startRowIndex, int maximumRows,int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            List<OnlineExamConfiguration> lstOnlineExamConfiguration = moOnlineExamWiseQueConfigDC.GetAllExamQuestionConfiguration(aiSchoolId, aiAcademicYearId, sortExpression, sortDirection, iEndIndex, startRowIndex, aiStandardId, aiStandardDivisionId, aiSubjectId);

            miRecordCount = 0;
            if (lstOnlineExamConfiguration.Count > 0)
                miRecordCount = lstOnlineExamConfiguration[0].TotalRows;

            return lstOnlineExamConfiguration;

        }
        public int CountTotalExamQuestionConfiguration(Int32 aiSchoolId, Int32 aiAcademicYearId, String sortExpression, string sortDirection, int maximumRows, int startRowIndex, int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            return miRecordCount;            
        }

       
        //public DataTable CopyExamConfigurationForClasses(int aitargetstddivid, int aisubject, string sids, string ExamXML)
        //{

        //    return moOnlineExamWiseQueConfigDC.CopyExamConfigurationForClasses(aitargetstddivid, aisubject, sids, ExamXML);
        //}


        public void Submit(int aiStdId, int aiStdDivId, int aiSubjectId, bool abIsSubmit)
        {
            moOnlineExamWiseQueConfigDC.Submit(aiStdId, aiStdDivId, aiSubjectId, abIsSubmit);
        }

        public ButtonStateDetails GetButtonState(int aiStdId, int aiStdDivId, int aiSubjectId)
        {
            return moOnlineExamWiseQueConfigDC.GetButtonState(aiStdId, aiStdDivId, aiSubjectId);
        }
    }
}
