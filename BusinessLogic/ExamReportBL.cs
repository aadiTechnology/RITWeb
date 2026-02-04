using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities.ProgressReport;
using DataCommunicator;

namespace BusinessLogic
{    
    public class ExamReportBL
    {
        ExamReporstDC moExamReporstDC;

        public ExamReportBL()
        {
            moExamReporstDC = new ExamReporstDC();
        }

        public TestwiseMark GetMarkDetailsForTestwiseReport(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStdDivId, int aiTestId)
        {
            return moExamReporstDC.GetMarkDetailsForTestwiseReport(aiSchoolId, aiAcademicYearId, aiStandardId, aiStdDivId, aiTestId);
        }
    }
}
