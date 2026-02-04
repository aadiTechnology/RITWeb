// Class Name       :- ClasswiseOptionalSubjectBL
// Purpose          :- This class is used to manage OptionalSubjectBL details.
// Date Of Modification :- 3/1/2012
// Modified By      :- Vipul Jadhav

using System.Collections.Generic;
using DataCommunicator;
using System.Linq;
using Utility;
using SchoolEntities;
using MasterEntities;
using BusinessLogic.Exceptions;


namespace BusinessLogic
{
    /// <summary>
    /// This class is used to manage OptionalSubjectBL details.
    /// </summary>
    public class ClasswiseOptionalSubjectBL
    {
        #region "Data members"

        private ClasswiseOptionalSubjectDC moClasswiseOptionalSubjectConfigurationDC = null;

        #endregion

        #region " Constructors"

        public ClasswiseOptionalSubjectBL(int aiSchoolId, int aiAcademicYearId)
        {
            this.moClasswiseOptionalSubjectConfigurationDC = new ClasswiseOptionalSubjectDC(aiSchoolId, aiAcademicYearId);
        }

        public ClasswiseOptionalSubjectBL(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            this.moClasswiseOptionalSubjectConfigurationDC = new ClasswiseOptionalSubjectDC(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
        }

        #endregion

        #region "Public methods"

        /// <summary>
        /// This method is used to get all subject list.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiParentOptionalSubjectId"></param>
        /// <returns></returns>
        public List<OptionalSubject> GetAllChildSubjects(int aiParentOptionalSubjectId)
        {
            return moClasswiseOptionalSubjectConfigurationDC.GetAllChildSubjects(aiParentOptionalSubjectId);
        }

        /// <summary>
        /// This method is used to get all subject list.
        /// </summary>
        /// <returns></returns>
        public List<OptionalSubject> GetAll()
        {
            return moClasswiseOptionalSubjectConfigurationDC.GetAll();
        }

        /// <summary>
        /// This method is used to save optional subject configuration details.
        /// </summary>
        /// <param name="asXml"></param>
        public void Save(string asXml)
        {
            moClasswiseOptionalSubjectConfigurationDC.Save(asXml);
        }

        /// <summary>
        /// This method is used to delete optional subject group.
        /// </summary>
        /// <param name="aiParentOptionalSubjectId"></param>
        public int Delete(int aiParentOptionalSubjectId)
        {
            string sErrorMsg = string.Empty;
            string sSubjects = string.Empty;
            string sSubjectGroup = string.Empty;
            moClasswiseOptionalSubjectConfigurationDC.ValidateOptionalSubjects(aiParentOptionalSubjectId);

            sSubjects = string.Join(", ", moClasswiseOptionalSubjectConfigurationDC.SubjectsWithMarksAssigned.Where(oSubject => oSubject.SubjectId != 0).Select(oSubject => oSubject.SubjectName));
            sSubjectGroup = string.Join(", ", moClasswiseOptionalSubjectConfigurationDC.SubjectsWithMarksAssigned.Where(oSubject => oSubject.SubjectId == 0).Select(oSubject => oSubject.SubjectName));

            if (!sSubjects.IsNullOrEmpty())
                sErrorMsg += "<li>Marks assignment is already done for subject(s): " + sSubjects + "</li>";
            if (!sSubjectGroup.IsNullOrEmpty())
                sErrorMsg += "<li>Marks assignment is already done for subject group(s): " + sSubjectGroup + "</li>";

            sSubjects = string.Join(", ", moClasswiseOptionalSubjectConfigurationDC.SubjectsAssignedToStudents.Where(oSubject => oSubject.SubjectId != 0).Select(oSubject => oSubject.SubjectName));
            sSubjectGroup = string.Join(", ", moClasswiseOptionalSubjectConfigurationDC.SubjectsAssignedToStudents.Where(oSubject => oSubject.SubjectId == 0).Select(oSubject => oSubject.SubjectName));

            if (!sSubjects.IsNullOrEmpty())
                sErrorMsg += "<li>Students are associated with subject(s): " + sSubjects + "</li>";
            if (!sSubjectGroup.IsNullOrEmpty())
                sErrorMsg += "<li>Students are associated with subject group(s): " + sSubjectGroup + "</li>";

            sSubjects = string.Join(", ", moClasswiseOptionalSubjectConfigurationDC.SubjectsAssociatedWithTimeTable.Where(oSubject => oSubject.SubjectId != 0).Select(oSubject => oSubject.SubjectName));
            sSubjectGroup = string.Join(", ", moClasswiseOptionalSubjectConfigurationDC.SubjectsAssociatedWithTimeTable.Where(oSubject => oSubject.SubjectId == 0).Select(oSubject => oSubject.SubjectName));

            if (!sSubjects.IsNullOrEmpty())
                sErrorMsg += "<li>Timetable is configured for subject(s): " + sSubjects + "</li>";
            if (!sSubjectGroup.IsNullOrEmpty())
                sErrorMsg += "<li>Timetable is configured for subject group(s): " + sSubjectGroup + "</li>";

            if (sErrorMsg.IsNullOrEmpty())
             return moClasswiseOptionalSubjectConfigurationDC.Delete(aiParentOptionalSubjectId);
            else
                throw new ReferenceExceptions("Optional subject group cannot be removed since:" + "<br /><ul>" + sErrorMsg + "</ul>");
        }

        /// <summary>
        /// This method is used to get optional subjects for marks transfer.
        /// </summary>
        /// <returns></returns>
        public List<OptionalSubject> GetForClass()
        {
            return moClasswiseOptionalSubjectConfigurationDC.GetForClass();
        }

        #endregion
    }
}
