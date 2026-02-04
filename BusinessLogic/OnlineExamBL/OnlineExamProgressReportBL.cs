using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator.OnlineExamDC;
using SchoolEntities.OnlineExam;

namespace BusinessLogic.OnlineExamBL
{
    public class OnlineExamProgressReportBL
    {
        OnlineExamProgressReportDC moOnlineExamProgressReportDC;
        public OnlineExamProgressReportBL()
        {
            moOnlineExamProgressReportDC = new OnlineExamProgressReportDC();
        }

        public OnlineExamProgressReportBL(int aiSchoolId, int aiAcademicYearId)
        {
            moOnlineExamProgressReportDC = new OnlineExamProgressReportDC(aiSchoolId, aiAcademicYearId);
        }

        public OnlineExamProgressReportDetails GetDetails(int aiStdDivId, int aiStudentId)
        {
            return moOnlineExamProgressReportDC.GetDetails(aiStdDivId, aiStudentId);
        }
    }
}
