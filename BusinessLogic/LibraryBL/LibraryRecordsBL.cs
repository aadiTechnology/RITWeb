using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;

namespace BusinessLogic
{   public class LibraryRecordsBL
    {
        LibraryRecordsDC moLibraryRecordsDC;
        public LibraryRecordsBL()
        {
            moLibraryRecordsDC = new LibraryRecordsDC();
        }

        public LibraryRecordsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moLibraryRecordsDC = new LibraryRecordsDC(aiSchoolId,aiAcademicYearId, aiUpdatedById);
        }

        public List<SchoolEntities.LibraryRecord> GetAll(int aiStandardId, int aiDivisionId, DateTime dtShowTime)
        {
            return moLibraryRecordsDC.GetAll(aiStandardId, aiDivisionId, dtShowTime);
        }

        public void SaveBookDetails(string sStudentBookDetailsXML, DateTime dtBookIssueReturnDate, int iStatusId)
        {
            moLibraryRecordsDC.SaveBookDetails(sStudentBookDetailsXML, dtBookIssueReturnDate, iStatusId);
        }
    }
}
