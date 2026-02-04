using System;
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;
using SchoolEntities.Admin;

namespace BusinessLogic
{
    public class OnlineExamQuestionConfigurationBL
    {
        #region Data members

        private OnlineExamQuestionConfigurationDC moOnlineExamQuestionConfigurationDC;
        private int miTotalRows;

        #endregion

        #region Constructors

        public OnlineExamQuestionConfigurationBL()
        {
            this.moOnlineExamQuestionConfigurationDC = new OnlineExamQuestionConfigurationDC();
        }

        public OnlineExamQuestionConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moOnlineExamQuestionConfigurationDC = new OnlineExamQuestionConfigurationDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Method(s)

        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects()
        {
            return moOnlineExamQuestionConfigurationDC.GetAllYearwiseSubjects();
        }
        public DataTable GetAllStandards()
        {
            return moOnlineExamQuestionConfigurationDC.GetAllStandards();
        }

        public DataTable GetAssociatedStandards(int aistandard)
        {
            return moOnlineExamQuestionConfigurationDC.GetAssociatedStandards(aistandard);
        }

        public void Save(string asQuestionXML, OnlineExamQuestionConfig oExamConfig, int aiStandardId)
        {
            moOnlineExamQuestionConfigurationDC.Save(asQuestionXML, oExamConfig, aiStandardId);
        }

        //public DataTable CopySubjectConfiguration(int aitargetstddivid, int aisubject, string sids)
        //{
        //    return moOnlineExamQuestionConfigurationDC.CopySubjectConfiguration(aitargetstddivid, aisubject, sids);
        //}

        public void DeleteQuestionAnswerImage(int aiQuestionId, int aiAnswerId)
        {
            moOnlineExamQuestionConfigurationDC.DeleteQuestionAnswerImage(aiQuestionId, aiAnswerId);
        }

        public List<OnlineExamQuestionConfig> GetAll(int aiSchoolId, int aiAcademicYearId, String sortExpression, string sortDirection, int startRowIndex, int maximumRows,int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            maximumRows = maximumRows + startRowIndex;
            List<OnlineExamQuestionConfig> lstOnlineExamQuestionConfig = moOnlineExamQuestionConfigurationDC.GetAll(aiSchoolId, aiAcademicYearId, sortExpression, sortDirection, iEndIndex, startRowIndex, aiStandardId, aiStandardDivisionId, aiSubjectId);

            miTotalRows = 0;
            if (lstOnlineExamQuestionConfig.Count > 0)
                miTotalRows = lstOnlineExamQuestionConfig[0].TotalRows;

            return lstOnlineExamQuestionConfig;
        }

        public int Count(Int32 aiSchoolId, Int32 aiAcademicYearId, String sortExpression, string sortDirection, int maximumRows, int startRowIndex, int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            return miTotalRows;
           // return OnlineExamQuestionConfigurationDC.Count(aiSchoolId, aiAcademicYearId, sortExpression, maximumRows, startRowIndex, aiStandardDivisionId, aiSubjectId);
        }

        public List<OnlineExamQuestionConfig> Get(int aiVehicleId)
        {
            return moOnlineExamQuestionConfigurationDC.Get(aiVehicleId);
        }
        public static DataSet GetDetailsForUpdateQuestions(int aiVehicleId, int aiSchoolID, int aiAcademicYearId)
        {
            return OnlineExamWiseQueConfigDC.GetDetailsForUpdateQuestions(aiVehicleId, aiSchoolID, aiAcademicYearId);
        }


        /// <summary>
        /// This method is used to delete the parameter from the given list view.
        /// </summary>
        /// <param name="aiParameterId"></param>
        public void Delete(int aiId)
        {
            moOnlineExamQuestionConfigurationDC.Delete(aiId);
        }

        #endregion

        public ButtonStateDetails GetButtonState(int aiStdId, int aiStdDivId, int aiSubjectId)
        {
          return  moOnlineExamQuestionConfigurationDC.GetButtonState(aiStdId, aiStdDivId, aiSubjectId);
        }

        public void Submit(int aiStdId, int aiStdDivId, int aiSubjectId, bool abIsSubmit)
        {
            moOnlineExamQuestionConfigurationDC.Submit(aiStdId, aiStdDivId, aiSubjectId, abIsSubmit);
        }
    }
}
