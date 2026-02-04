using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;
using System.Data;
using SchoolEntities.Admin;
using SchoolEntities.OnlineExam;

namespace BusinessLogic
{
  public  class PublishOnlineExamBL
    {
       #region Data members

        private PublishOnlineExamDC moOnlineExamWiseQueConfigDC;
       
        #endregion
              #region Constructors

        public PublishOnlineExamBL()
        {
            this.moOnlineExamWiseQueConfigDC = new PublishOnlineExamDC();
        }


        public PublishOnlineExamBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moOnlineExamWiseQueConfigDC = new PublishOnlineExamDC(aiSchoolId, aiAcademicYearId,aiUpdatedById);
        }

        #endregion

        public List<OnlineExamResult> ExamResults
        {
            get { return moOnlineExamWiseQueConfigDC.ExamResults; }
        }

        public bool IsPublished
        {
            get { return moOnlineExamWiseQueConfigDC.IsPublished; }
        }

        public bool AllowPublish
        {
            get { return moOnlineExamWiseQueConfigDC.AllowPublish; }
        }

        public DataTable GetAssociatedStandards(int aiSchoolId, int aiAcademicYearId)
        {
            return moOnlineExamWiseQueConfigDC.GetAssociatedStandards(aiSchoolId, aiAcademicYearId);
        }

        public List<StudentInfo> GetAllStudentsForClass(int aiStdDivId, int aiExamId, int aiSubjectId)
        {
            return moOnlineExamWiseQueConfigDC.GetAllStudentsForClass(aiStdDivId, aiExamId, aiSubjectId);
        }

        public List<OnlineExamStatus> GetExamResult(int aiStdDivId, int aiExamId)
        {
            return moOnlineExamWiseQueConfigDC.GetExamResult(aiStdDivId, aiExamId);
        }

        public void Publish(int aiStdDivId, int aiExamId, bool abIsPublish)
        {
            moOnlineExamWiseQueConfigDC.Publish(aiStdDivId, aiExamId, abIsPublish);
        }
    }
}
