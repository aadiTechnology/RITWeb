using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;
using XseedReportEntities;

namespace BusinessLogic
{ 
    public class TeacherStandardDetailsBL
    {

        #region DataMembers and properties

        #region Data members

        private TeacherStandardDetailsDC.TeacherStandardDetailsStruct moTeacherStandardDetailsStruct;
        private TeacherStandardDetailsDC moTeacherStandardDetailsDC = new TeacherStandardDetailsDC();
        private Constants.Action eAction;

        #endregion

        #region Properties

        public int TeacherStandardId
        {

        get { return moTeacherStandardDetailsStruct.miTeacherStandardId; }
        set { moTeacherStandardDetailsStruct.miTeacherStandardId = value; }
        }

        public int TeacherId
        {

        get { return moTeacherStandardDetailsStruct.miTeacherId; }
        set { moTeacherStandardDetailsStruct.miTeacherId = value; }
        }

        public int StandardId
        {

        get { return moTeacherStandardDetailsStruct.miStandardId; }
        set { moTeacherStandardDetailsStruct.miStandardId = value; }
        }

        public string StandardName
        {

            get { return moTeacherStandardDetailsStruct.msStandardName; }
            set { moTeacherStandardDetailsStruct.msStandardName = value; }
        }

        public int InsertedById
        {

            get { return moTeacherStandardDetailsStruct.miInsertedById; }
            set { moTeacherStandardDetailsStruct.miInsertedById = value; }
        }
   
        public int UpdatedById
        {

        get { return moTeacherStandardDetailsStruct.miUpdatedById; }
        set { moTeacherStandardDetailsStruct.miUpdatedById = value; }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }
      
        #endregion

        #endregion

        #region Constructors 

        public TeacherStandardDetailsBL()
        {
            moTeacherStandardDetailsDC = new TeacherStandardDetailsDC();
        }
        //public TeacherStandardDetailsBL(int aiId){

        //TeacherStandardDetailsDC moTeacherStandardDetailsDC = new TeacherStandardDetailsDC(aiId);

        //}
        #endregion

        #region Public Methods 

          public string InsertTeacherStandardDetails()
        {
            // This Function is used to insert the record in to database. 
            moTeacherStandardDetailsDC.TeacherStandardInfoStructure = moTeacherStandardDetailsStruct;
            return moTeacherStandardDetailsDC.GetStandardDetailsInsertStatement();
        }

       public DataTable FetchStandardDetailsForTeacherId(int aiTeacherId)
        {
             moTeacherStandardDetailsDC.TeacherStandardInfoStructure = moTeacherStandardDetailsStruct;
            return moTeacherStandardDetailsDC.FetchStandardDetailsForTeacherId(aiTeacherId);
        }

        public DataTable FetchStandardDetailsForEditDetails(int aiTeacherId, int aiSchoolId, int aiAcademicYearId)
        {
            moTeacherStandardDetailsDC.TeacherStandardInfoStructure = moTeacherStandardDetailsStruct;
            return moTeacherStandardDetailsDC.FetchStandardDetailsForEditDetails(aiTeacherId, aiSchoolId, aiAcademicYearId);
        }  
        
        //public ArrayList GetAllStandardsForTeacher(int aiTeacherId)
        //{
        //    return moTeacherStandardDetailsDC...GetAllSubjectsForTeacher(aiTeacherId);
        //}

        public bool IsTeacherPrePrimary(int iSchoolId,int iAcademicYearId,int iTeacherId)
        {
            bool bIsPrePrimary = false;
            DataTable oDataTable = moTeacherStandardDetailsDC.IsTeacherPrePrimary(iSchoolId,iAcademicYearId,iTeacherId);
            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != null)
                {
                    if(Convert.ToChar(oDataTable.Rows[0][0]).Equals(Constants.C_YES))
                        bIsPrePrimary = true;
                }
            }
            return bIsPrePrimary;
        }

        public List<ClassTeacherDetails> GetClassTeachersForOptionalSubjectClasses(int aiAcademicYearId, int aiSchoolId, int aiTeacherId)
        {
            return moTeacherStandardDetailsDC.GetClassTeachersForOptionalSubjectClasses(aiAcademicYearId, aiSchoolId, aiTeacherId);
        }
		/// <summary>
		/// This method is used to check that class of class teacher has normal configuration or preprimary exam configuartio.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiTeacherId"></param>
		/// <returns></returns>
		public bool IsPreprimaryExamConfiguration(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, string asUserRole)
        {
			return moTeacherStandardDetailsDC.IsPreprimaryExamConfiguration(aiSchoolId, aiAcademicYearId, aiStdDivId, asUserRole);
        }

	    #endregion


       
    }  
    public class TeacherStandardDetailsCollectionBL
    {
        private TeacherStandardDetailsCollectionDC moTeacherStandardDetailsCollectionDC = null;

        public TeacherStandardDetailsCollectionBL()
        {
            moTeacherStandardDetailsCollectionDC = new TeacherStandardDetailsCollectionDC();
        }

         public TeacherStandardDetailsCollectionBL(int aiSchool_Id, int aiAcademic_Year_Id)
        {
            moTeacherStandardDetailsCollectionDC = new TeacherStandardDetailsCollectionDC(aiSchool_Id,aiAcademic_Year_Id);
        }
        public bool DeleteTeacherStandardDetails(ArrayList aoArrDeleteTeacherIds)
        {
            moTeacherStandardDetailsCollectionDC.DeleteTeacherStandardDetails(aoArrDeleteTeacherIds);
            return true;
        }

        public static string RemoveAllStandardForTeacherId(int aiTeacherId)
        {
            return TeacherStandardDetailsCollectionDC.RemoveAllStandardsForTeacherId(aiTeacherId);
        }

        public bool DeleteTeacherStandardDetails(int aiTecherId)
        {
            moTeacherStandardDetailsCollectionDC.DeleteTeacherStandardDetails(aiTecherId);
            return true;
        }

        public int GetStdDivIdOfClassTeacher(int aiTeacherId)
        {
            return moTeacherStandardDetailsCollectionDC.GetStdDivIdOfClassTeacher(aiTeacherId);
        }
		
		/// <summary>
		/// This method is used to get class teacher of provided class
		/// </summary>
		/// <param name="aiStdDivId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public int GetClassTeacher(int aiStdDivId, int aiSchoolId, int aiAcademicYearId)
		{
			return moTeacherStandardDetailsCollectionDC.GetClassTeacher(aiStdDivId, aiSchoolId, aiAcademicYearId);
		}

        public char CheckIfStandardHasOnlyGradeSystem(int aiStdDivId, int aiStandardId)
        {
			return moTeacherStandardDetailsCollectionDC.CheckIfStandardHasOnlyGradeSystem(aiStdDivId, aiStandardId);
        }

        public bool IsMonthConfiguration(int aiStandardDivisionId)
        {
            return moTeacherStandardDetailsCollectionDC.IsMonthConfiguration(aiStandardDivisionId);
        }
		public static DataTable GetTeachersForPrePrimaryProgressReport(int aiStdDivId, int aiSchoolId, int aiAcademicYearId)
        {
			return TeacherStandardDetailsCollectionDC.GetTeachersForPrePrimaryProgressReport(aiStdDivId, aiSchoolId, aiAcademicYearId);
        }

    }

}
