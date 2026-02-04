using System.Collections;
using System.Data;
using DataCommunicator;
using System.Collections.Generic;
using XseedReportEntities;
using Utility;

namespace BusinessLogic
{
    public class TeacherSubjectDetailsBL
    {
          #region " Data Members & Properties"

            #region " Data Members "

            // Object of the TeacherEducationDetailsDC Class. 
            //Using this object call the methods of the TeacherEducationDetailsDC Class.

            TeacherSubjectDetailsDC moTeacherSubjectDetailsDC;
            TeacherSubjectDetailsDC.TeacherSubInfoStruct moTeacherSubInfoStruct;
            private Constants.Action eAction;

            #endregion

            #region " Properties "

            public int TeacherSubjectId
            {
                get
                {
                    return moTeacherSubInfoStruct.miTeacherSubjectId;
                }
                set
                {
                    moTeacherSubInfoStruct.miTeacherSubjectId = value;
                }
            }

            public int TeacherId
            {
                get
                {
                    return moTeacherSubInfoStruct.miTeacherId;
                }
                set
                {
                    moTeacherSubInfoStruct.miTeacherId = value;
                }
            }

           public int SubjectId
            {
                get
                {
                    return moTeacherSubInfoStruct.miSubjectId;
                }
                set
                {
                    moTeacherSubInfoStruct.miSubjectId = value;
                }
            }

        public string SubjectName
        {
            get
            {
                return moTeacherSubInfoStruct.msSubjectName;
            }
            set
            {
                moTeacherSubInfoStruct.msSubjectName = value;
            }
        }

            public int InsertedById
            {
                get
                {
                    return moTeacherSubInfoStruct.miInsertedById;
                }
                set
                {
                    moTeacherSubInfoStruct.miInsertedById = value;
                }
            }

            public int UpdatedById
            {
                get
                {
                    return moTeacherSubInfoStruct.miUpdatedById;
                }
                set
                {
                    moTeacherSubInfoStruct.miUpdatedById = value;
                }
            }

            public Constants.Action ConfigurationAction
            {
                 get { return eAction; }
                 set { eAction = value; }
            }

            #endregion

            #endregion

          #region " OverLoaded Constructors "

        public TeacherSubjectDetailsBL()
            {
                //Default constructor
                moTeacherSubjectDetailsDC = new TeacherSubjectDetailsDC();
            }

            #endregion

          #region " Public Methods "

            /// <summary>
            /// This method is used to get all details from UI to insert in database.
            /// </summary>
            /// <returns></returns>

            public string InsertTeacherSubjectDetails()
            {
                // This Function is used to insert the record in to database. 
                moTeacherSubjectDetailsDC.TeacherSubInfoStructure = moTeacherSubInfoStruct;
                return moTeacherSubjectDetailsDC.GetSubjectDetailsInsertStatement();  
            }

        public DataTable FetchSubjectDetailsForTeacherId(int aiTeacherId)
        {
            moTeacherSubjectDetailsDC.TeacherSubInfoStructure = moTeacherSubInfoStruct;
            return moTeacherSubjectDetailsDC.FetchSubjectDetailsForTeacherId(aiTeacherId);  
        }

        public DataTable FetchSubjectDetailsForEditDetails(int aiTeacherId, int aiSchoolId, int aiAcademicYearId)
        {
            moTeacherSubjectDetailsDC.TeacherSubInfoStructure = moTeacherSubInfoStruct;
            return moTeacherSubjectDetailsDC.FetchSubjectDetailsForEditDetails(aiTeacherId, aiSchoolId, aiAcademicYearId);  
        }

        public ArrayList GetAllSubjectsForTeacher(int aiTeacherId)
        {
            return moTeacherSubjectDetailsDC.GetAllSubjectsForTeacher(aiTeacherId);
        }

        /// <summary>
        ///  This method is used to get teacher associated subjects.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<SubjectMaster> GetTeacherAssociatedSubjects(int aiTeacherId, int aiStandardId, int aiAcademicYearId, int aiSchoolId, bool abConsiderSubjectSections)
        {
            return TeacherSubjectDetailsDC.GetTeacherAssociatedSubjects(aiTeacherId, aiStandardId, aiAcademicYearId, aiSchoolId, abConsiderSubjectSections);
        }
            #endregion
    }

    public class TeacherSubjectDetailsCollectionBL
    {
        private TeacherSubjectDetailsCollectionDC moTeacherSubjectDetailsCollectionDC = null;

        public TeacherSubjectDetailsCollectionBL()
        {
            moTeacherSubjectDetailsCollectionDC = new TeacherSubjectDetailsCollectionDC();
        }

        public bool DeleteTeacherSubjectDetails(ArrayList aoArrDeleteTeacherIds)
        {
            moTeacherSubjectDetailsCollectionDC.DeleteTeacherSubjectDetails(aoArrDeleteTeacherIds);
            return true;
        }

        public bool DeleteTeacherSubjectDetails(int aiTeacherId)
        {
            moTeacherSubjectDetailsCollectionDC.DeleteTeacherSubjectDetails(aiTeacherId);
            return true;
        }


        public static string RemoveAllSubjectsForTeacherId(int aiTeacherId)
        {
            return TeacherSubjectDetailsCollectionDC.RemoveAllSubjectsForTeacherId(aiTeacherId);
        }

        
    }
}                                           
