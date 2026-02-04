using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class TeacherSubjectAssignmentBL
    {

        #region DataMembers and properties

        #region Data members

        private TeacherSubjectAssignmentDC.TeacherSubjectAssignmentStruct moTeacherSubjectAssignmentStruct;
        private TeacherSubjectAssignmentDC moTeacherSubjectAssignmentDC = new TeacherSubjectAssignmentDC();
        private Constants.Action eAction;

        #endregion
        #region Properties

        public int TeacherSubjectId
        {

            get { return moTeacherSubjectAssignmentStruct.miTeacherSubjectId; }
            set { moTeacherSubjectAssignmentStruct.miTeacherSubjectId = value; }
        }

        public int SchoolId
        {

            get { return moTeacherSubjectAssignmentStruct.miSchoolId; }
            set { moTeacherSubjectAssignmentStruct.miSchoolId = value; }
        }

        public int SubjectId
        {

            get { return moTeacherSubjectAssignmentStruct.miSubjectId; }
            set { moTeacherSubjectAssignmentStruct.miSubjectId = value; }
        }

        public int StandardDivisionId
        {

            get { return moTeacherSubjectAssignmentStruct.miStandardDivisionId; }
            set { moTeacherSubjectAssignmentStruct.miStandardDivisionId = value; }
        }

        public int TeacherId
        {

            get { return moTeacherSubjectAssignmentStruct.miTeacherId; }
            set { moTeacherSubjectAssignmentStruct.miTeacherId = value; }
        }

        public bool IsExclusive
        {

            get { return moTeacherSubjectAssignmentStruct.mbIsExclusive; }
            set { moTeacherSubjectAssignmentStruct.mbIsExclusive = value; }
        }

        public int InsertedById
        {

            get { return moTeacherSubjectAssignmentStruct.miInsertedById; }
            set { moTeacherSubjectAssignmentStruct.miInsertedById = value; }
        }

        public int UpdatedById
        {

            get { return moTeacherSubjectAssignmentStruct.miUpdatedById; }
            set { moTeacherSubjectAssignmentStruct.miUpdatedById = value; }
        }

        public Constants.Action AssignmentAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public TeacherSubjectAssignmentBL()
        {
        }
        public TeacherSubjectAssignmentBL(int aiId)
        {

            TeacherSubjectAssignmentDC moTeacherSubjectAssignmentDC = new TeacherSubjectAssignmentDC(aiId);

        }
        #endregion

        #region Public Methods

        public Int32 InsertTeacherSubjectAssignment()
        {

            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.InsertTeacherSubjectAssignment();
        }

        public void UpdateTeacherSubjectAssignment()
        {
            string sMessage = CheckDependencies();
            if (sMessage.Equals(""))
            {
                moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
                moTeacherSubjectAssignmentDC.UpdateTeacherSubjectAssignment();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
        }

        public bool DeleteAssignSubjectForTeacher(ArrayList aoArrayList)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            moTeacherSubjectAssignmentDC.DeleteAssignSubjectForTeacher(aoArrayList);
            return true;
        }
        /// <summary>
        /// This method returns the message informing about the dependencies.
        /// </summary>
        /// <returns></returns>
        public string CheckDependencies( )
        {
            string sReturn = "";
            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.TeacherClassSubject), moTeacherSubjectAssignmentStruct.miTeacherSubjectId, "", 0);
            return sReturn;
        }
      

        public bool IsSubjectAssignToTeacher(int aiStandardDivisionId, int aiSubjectId)
        {
            // This method calls the DC method to check if the current Buyer login is duplicate or not.
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.IsSubjectAssignToTeacher(aiStandardDivisionId, aiSubjectId);
        }


        public string GetInsertStatementForTeacherSubjectAssignment()
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetInsertStatementForTeacherSubjectAssignment();
        }

        public string GetUpdateStatementForTeacherSubjectAssignment()
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetUpdateStatementForTeacherSubjectAssignment();
        }

        public string GetDeleteStatementForTeacherSubjectAssignment()
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetDeleteStatementForTeacherSubjectAssignment();
        }


        public string GetDeleteStatementForTeacherId(int aiTeacherId, int aiSubjectId,int aiStandardDivisionId)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetDeleteStatementForTeacherId(aiTeacherId, aiSubjectId, aiStandardDivisionId);
        }
        public bool IsTeacherAssignedForSubject(int aiSchoolId, int aiTeacherId)
        {
            return moTeacherSubjectAssignmentDC.IsTeacherAssignedForSubject(aiSchoolId, aiTeacherId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public DataTable GetSubjectAssignedTeacherName(int aiStandardDivisionId, int aiSubjectId, int aiTeacherId)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetSubjectAssignedTeacherName(aiStandardDivisionId, aiSubjectId, aiTeacherId);
        }
        public void DeleteTeacherSubjectAssignmentForTeacher(int aiTeacherId, int aiSubjectId, int aiStandardDivisionId)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            moTeacherSubjectAssignmentDC.DeleteTeacherSubjectAssignmentForTeacher(aiTeacherId, aiSubjectId, aiStandardDivisionId);
        }

        public DataTable GetSubjectAssignedTeacherDetails(int aiStandardDivisionId, int aiSubjectId, int aiSchoolId, int aiAcademicYearId,string sFilter)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetSubjectAssignedTeacherDetails(aiStandardDivisionId, aiSubjectId, aiSchoolId, aiAcademicYearId,sFilter);
        }

        public DataTable GetTeacherSubjectIdOfAlreadyAssignedSubject(int aiStandardDivisionId, int aiSubjectId)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetTeacherSubjectIdOfAlreadyAssignedSubject(aiStandardDivisionId, aiSubjectId);
        
        }


        public DataTable GetListOfTeacherSubjectsforStudent(int aiUserId, int aiSchoolId, int aiAcademicYearId)
        {
            moTeacherSubjectAssignmentDC.TeacherSubjectAssignmentStructDetails = moTeacherSubjectAssignmentStruct;
            return moTeacherSubjectAssignmentDC.GetListOfTeacherSubjectsforStudent(aiUserId, aiSchoolId, aiAcademicYearId);
        }

        public DataSet GetTeacherAndStandardForTT(int aiSchoolId, int aiAcademicYearId)
        {
            return moTeacherSubjectAssignmentDC.GetTeacherAndStandardForTT(aiSchoolId, aiAcademicYearId);
        }

        public DataTable GetTeacherSubjectDetails(int aiSchoolId, int aiAcademicYearId)
        {
            return moTeacherSubjectAssignmentDC.GetTeacherSubjectDetails(aiSchoolId, aiAcademicYearId);
        }

        public DataSet GetTeacherSubjectMaxLecDetails(int aiSchoolId, int aiAcademicYearId)
        {
            return moTeacherSubjectAssignmentDC.GetTeacherSubjectMaxLecDetails(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is sued to return standard division id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public int GetStdDivId(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            return moTeacherSubjectAssignmentDC.GetStdDivId(aiSchoolId, aiAcademicYearId, aiTeacherId);
        }

        #endregion
    }

    public class TeacherSubjectAssignmentCollectionBL
    {
        #region DataMembers
        TeacherSubjectAssignmentCollectionDC moTeacherSubjectAssignmentCollectionDC = null;
        #endregion
        #region Properties
        public bool IsPublished
        {
            get {return moTeacherSubjectAssignmentCollectionDC.mbIsPublished; }
            set { moTeacherSubjectAssignmentCollectionDC.mbIsPublished = value; }
        }

        public bool ToppersGenerated
        {
            get { return moTeacherSubjectAssignmentCollectionDC.mbToppersGenerated; }
            set { moTeacherSubjectAssignmentCollectionDC.mbToppersGenerated = value; }
        }

        #endregion

        #region Constructor
        public TeacherSubjectAssignmentCollectionBL(int aiTeacherId)
        {
            moTeacherSubjectAssignmentCollectionDC = new TeacherSubjectAssignmentCollectionDC(aiTeacherId);
        }
        public TeacherSubjectAssignmentCollectionBL()
        {
            moTeacherSubjectAssignmentCollectionDC = new TeacherSubjectAssignmentCollectionDC();
        }
        #endregion

        #region Public Methods
        public void UpdatePreviousTeacherSubjects(Collection<TeacherSubjectAssignmentBL> aoTeacherSubjects)
        {
            ArrayList aoArrStatement = new ArrayList();
            IEnumerator oIEnum = aoTeacherSubjects.GetEnumerator();
            
            while (oIEnum.MoveNext())
            {
                TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = (TeacherSubjectAssignmentBL)oIEnum.Current;

                switch (oTeacherSubjectAssignmentBL.AssignmentAction)
                {
                    case Constants.Action.Insert:
                        aoArrStatement.Add(oTeacherSubjectAssignmentBL.GetInsertStatementForTeacherSubjectAssignment());
                        break;
                    case Constants.Action.Update:
                        aoArrStatement.Add(oTeacherSubjectAssignmentBL.GetUpdateStatementForTeacherSubjectAssignment());
                        break;
                    case Constants.Action.Delete:
                        aoArrStatement.Add(oTeacherSubjectAssignmentBL.GetDeleteStatementForTeacherSubjectAssignment());
                        break;
                }
            }

            moTeacherSubjectAssignmentCollectionDC.UpdateTeacherSubjects(aoArrStatement);
        }




        public void UpdateTeacherSubjects(Collection<TeacherSubjectAssignmentBL> aoTeacherSubjects, ArrayList aoArrStatement)
        {
           
            IEnumerator oIEnum = aoTeacherSubjects.GetEnumerator();
            //ArrayList oArrayList = new ArrayList();
            while (oIEnum.MoveNext())
            {
                TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = (TeacherSubjectAssignmentBL)oIEnum.Current;
               
                switch (oTeacherSubjectAssignmentBL.AssignmentAction)
                {
                    case Constants.Action.Insert:
                        aoArrStatement.Add(oTeacherSubjectAssignmentBL.GetInsertStatementForTeacherSubjectAssignment());
                        break;
                }
            }

            moTeacherSubjectAssignmentCollectionDC.UpdateTeacherSubjects(aoArrStatement);
        }

        //public void UpdatePreviousTeacherSubjectAssignmentList(int aiTeacherId, ArrayList aoArrStatement)
        //{
        //    TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        //            aoArrStatement.Add(oTeacherSubjectAssignmentBL.GetInsertStatementForPreviousTeacherSubjectAssignment(aiTeacherId));   
        //    moTeacherSubjectAssignmentCollectionDC.UpdateTeacherSubjects(aoArrStatement);
        //}

        //public ArrayList DeletePreviousTeacherSubjectAssignmentList(int aiTeacherId, ArrayList aoArrDeleteStatement)
        //{
        //    TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        //    aoArrDeleteStatement.Add(oTeacherSubjectAssignmentBL.GetDeleteStatementForPreviousTeacherSubjectAssignment(aiTeacherId));
        //    return aoArrDeleteStatement;
        //}

        public ArrayList DeletePreviousSubjectList(Collection<TeacherSubjectAssignmentBL> aoTeacherSubjects, ArrayList aoArrDeleteStatement)
        {   
            IEnumerator oIEnum = aoTeacherSubjects.GetEnumerator();
            //ArrayList oArrayList = new ArrayList();
            //oArrayList.Add(aoArrDeleteStatement);
          
          
          while (oIEnum.MoveNext())
            {
                TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = (TeacherSubjectAssignmentBL)oIEnum.Current;
                aoArrDeleteStatement.Add(oTeacherSubjectAssignmentBL.GetDeleteStatementForTeacherSubjectAssignment());
            }  
            return aoArrDeleteStatement;

          //  moTeacherSubjectAssignmentCollectionDC.DeletePreviousSubjectList(aoArrDeleteStatement);
        }

        public DataTable GetAllDivisionSubjectsDetailsForTeacher(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            return moTeacherSubjectAssignmentCollectionDC.GetAllDivisionSubjectsDetailsForTeacher(aiSchoolId, aiAcademicYearId, aiTeacherId);
        }

        public DataSet RetriveTeacherClassSubjectsForTT(int aiSchoolId, int aiAcademicYearId, int aiWeekDayId)
        {
            return moTeacherSubjectAssignmentCollectionDC.RetriveTeacherClassSubjectsForTT(aiSchoolId, aiAcademicYearId, aiWeekDayId);
        }

		public DataSet RetriveSubjectsDetailsForClassTeacher(int aiSchool_Id, int aiAcademicYearId, int aiStdDivId, int aiTestId)
        {
            DataSet oDS = moTeacherSubjectAssignmentCollectionDC.RetriveSubjectsDetailsForClassTeacher(aiSchool_Id,
			aiAcademicYearId, aiStdDivId, aiTestId);
            IsPublished = moTeacherSubjectAssignmentCollectionDC.mbIsPublished;
            return oDS;
        }

        public DataTable RetriveSubjectTeachers(int aiAcademicYearId)
        {
            return moTeacherSubjectAssignmentCollectionDC.RetriveSubjectTeachers(aiAcademicYearId);
        }


        public DataTable RetriveSubjectTeacherClass(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            return moTeacherSubjectAssignmentCollectionDC.RetriveSubjectTeacherClass(aiSchoolId, aiAcademicYearId, aiTeacherId);
        }

        public DataSet RetriveClassesForTT(int aiSchoolId, int aiAcademicYearId)
        {
            return moTeacherSubjectAssignmentCollectionDC.RetriveClassesForTT(aiSchoolId,aiAcademicYearId);
        }
       
        //public DataSet RetriveStandardsForTeacher(int aiAcademicYearId)
        //{
        //    return moTeacherSubjectAssignmentCollectionDC.RetriveStandardsForTeacher(aiAcademicYearId);
        //}

        //public DataSet RetriveDivisionsForTeacher(int aiAcademicYearId, int aiStandardId)
        //{
        //    return moTeacherSubjectAssignmentCollectionDC.RetriveDivisionsForTeacher(aiAcademicYearId, aiStandardId);
        //}

        /// <summary>
        /// Returns dataset containing 5 tables.
        /// Table 1 - All associated standard-divisions for school
        /// Table 2 - All subjects associated to school
        /// Table 3 - All teachers with their subjects which they can teach
        /// Table 4 - All Subjects for Division
        /// Table 5 - All Subjects Assigned to TeachersName
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataSet GetTeacherSubjectAssociation(int aiSchoolId, int aiAcademicYearId, int aiStandardId, string asSearchText="", string asCategoryId="")
        {
            return moTeacherSubjectAssignmentCollectionDC.GetTeacherSubjectAssociation(aiSchoolId, aiAcademicYearId, aiStandardId, asSearchText, asCategoryId);
        }

        /// <summary>
        /// This method is sued to return teacher details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abShowHomeworkMtoClassTeacher"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public DataTable RetriveTeachersForHomework(int aiSchoolId, int aiAcademicYearId, bool abShowHomeworkMtoClassTeacher, int aiTeacherId)
        {
            return moTeacherSubjectAssignmentCollectionDC.RetriveTeachersForHomework(aiSchoolId, aiAcademicYearId, abShowHomeworkMtoClassTeacher, aiTeacherId);
        }

        #endregion
    }

}
