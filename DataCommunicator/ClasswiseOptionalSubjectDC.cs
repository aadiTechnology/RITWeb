// Class Name       :- ClasswiseOptionalSubjectDC
// Purpose          :- This class is used to manage OptionalSubjectBL details.
// Date Of Modification :- 3/1/2012
// Modified By      :- Vipul Jadhav

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using MasterEntities;
using Utility;

namespace DataCommunicator
{
    public class ClasswiseOptionalSubjectDC
    {
        #region "Data members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private int miStandardDivisionId = 0;
        private List<SubjectMaster> mlstSubjectsWithMarksAssigned;
        private List<SubjectMaster> mlstSubjectsAssignedToStudents;
        private List<SubjectMaster> mlstSubjectsAssociatedWithTimeTable;

        public List<SubjectMaster> SubjectsWithMarksAssigned
        {
            get { return mlstSubjectsWithMarksAssigned; }
        }

        public List<SubjectMaster> SubjectsAssignedToStudents
        {
            get { return mlstSubjectsAssignedToStudents; }
        }

        public List<SubjectMaster> SubjectsAssociatedWithTimeTable
        {
            get { return mlstSubjectsAssociatedWithTimeTable; }
        }

        #endregion

        #region " Constructors"

        public ClasswiseOptionalSubjectDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public ClasswiseOptionalSubjectDC(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miStandardDivisionId = aiStandardDivisionId;
        }

        #endregion

        #region "Public methods"
        /// <summary>
        /// This method is get subjects list.
        /// </summary>
        /// <returns></returns>
        public List<OptionalSubject> GetAllChildSubjects(int aiParentOptionalSubjectId)
        {
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("SchoolWise_Standard_Division_Id", miStandardDivisionId, SqlDbType.Int);
                if (aiParentOptionalSubjectId != 0)
                    oSqlServerDbUtility.AddParameter("ParentGroupId", aiParentOptionalSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOptionalSubjects"))
                {
                    GenericClass<OptionalSubject> oOptionalSubject = new GenericClass<OptionalSubject>();
                    return oOptionalSubject.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to get all standardwise optional subjects.
        /// </summary>
        /// <returns></returns>
        public List<OptionalSubject> GetAll()
        {
            List<OptionalSubject> oOptionalSubjectDetails = new List<OptionalSubject>();
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("SchoolWise_Standard_Division_Id", miStandardDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOptionalSubjectsForStandardDivision"))
                {
                    GenericClass<OptionalSubject> oOptionalSubject = new GenericClass<OptionalSubject>();
                    oOptionalSubjectDetails = oOptionalSubject.GetFilledObjectList(oSqlDataReader);
                }
            }

            return oOptionalSubjectDetails;
        }

        /// <summary>
        /// This method is used to save optional subject detaills.
        /// </summary>
        /// <param name="asXml"></param>
        public void Save(string asXml)
        {
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("schoolId", miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("SchoolWise_Standard_Division_Id", miStandardDivisionId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("OptionalSubXML", asXml, SqlDbType.Xml);
                oSqlServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertOptionalSubjectDetails");
            }
        }

        /// <summary>
        /// This method is used to delete optional subject group.
        /// </summary>
        /// <param name="aiParentOptionalSubjectId"></param>
        public int Delete(int aiParentOptionalSubjectId)
        {
            
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("Standard_Division_Id", miStandardDivisionId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("ParentGroupId", aiParentOptionalSubjectId, SqlDbType.Int);
                SqlParameter oSqlParam = oSqlServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSqlServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteOptionalSubjectDetails");
                return Convert.ToInt32(oSqlParam.Value);
            }
            
        }

        /// <summary>
        /// This method is used to get optional subjects for marks transfer.
        /// </summary>
        /// <returns></returns>
        public List<OptionalSubject> GetForClass()
        {
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("StandardDivisionId", miStandardDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOptionalSubjectsForMarksTransfer"))
                {
                    GenericClass<OptionalSubject> oOptionalSubject = new GenericClass<OptionalSubject>();
                    return oOptionalSubject.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to validate optional subject group.
        /// </summary>
        /// <param name="aiParentOptionalSubjectId"></param>
        public void ValidateOptionalSubjects(int aiParentOptionalSubjectId)
        {
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("StandardDivisionId", miStandardDivisionId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("ParentGroupId", aiParentOptionalSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ValidateOptionalSubjects"))
                {
                    GenericClass<SubjectMaster> oSubjectMaster = new GenericClass<SubjectMaster>();
                    mlstSubjectsWithMarksAssigned = oSubjectMaster.GetFilledObjectList(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        mlstSubjectsAssignedToStudents = oSubjectMaster.GetFilledObjectList(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        mlstSubjectsAssociatedWithTimeTable = oSubjectMaster.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        #endregion
    }
}
