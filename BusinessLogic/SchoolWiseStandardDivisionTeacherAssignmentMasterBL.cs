using System;
using System.Data;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class SchoolWiseStandardDivisionTeacherAssignmentMasterBL
    {

        #region DataMembers and properties

        #region Data members

        private SchoolWiseStandardDivisionTeacherAssignmentMasterDC.SchoolWiseStandardDivisionTeacherAssignmentMasterStruct moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;
        private SchoolWiseStandardDivisionTeacherAssignmentMasterDC moSchoolWiseStandardDivisionTeacherAssignmentMasterDC = new SchoolWiseStandardDivisionTeacherAssignmentMasterDC();

        #endregion

        #region Properties

        public int SchoolWiseStandardDivisionSubjectTeacherId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolWiseStandardDivisionSubjectTeacherId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolWiseStandardDivisionSubjectTeacherId = value; }
        }

        public int SchoolId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolId = value; }
        }

        public int StandardId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miStandardId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miStandardId = value; }
        }

        public int DivisionId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miDivisionId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miDivisionId = value; }
        }

        public int TeacherId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miTeacherId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miTeacherId = value; }
        }

        public int OrgTeacherId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miOrgTeacherId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miOrgTeacherId = value; }
        }

        public int AcademicYearId
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId = value; }
        }


        public char IsClassTeacher
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.msIsClassTeacher; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.msIsClassTeacher = value; }
        }

        public string IsDeleted
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.msIsDeleted; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.msIsDeleted = value; }
        }

        public DateTime InsertDate
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.mdtInsertDate; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.mdtInsertDate = value; }
        }

        public int InsertedByid
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miInsertedByid; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miInsertedByid = value; }
        }

        public DateTime UpdateDate
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.mdtUpdateDate; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.mdtUpdateDate = value; }
        }

        public int UpdatedById
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miUpdatedById; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miUpdatedById = value; }
        }

        #endregion

        #endregion

        #region Constructors

        public SchoolWiseStandardDivisionTeacherAssignmentMasterBL()
        {
        }
       
        #endregion

        #region Public Methods

        public Int32 InsertSchoolWiseStandardDivisionSubjectTeacherAssignmentMaster()
        {

            moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.SchoolWiseStandardDivisionTeacherAssignmentMasterStructDetails = moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;
            return moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.InsertSchoolWiseStandardDivisionSubjectTeacherAssignmentMaster();
        }
       
        public void UpdateTeacherDetailsForStandardDivision(string asTeacherName)
        {
            string sErrMsg = CheckDependenciesForClassTeacher(asTeacherName,Constants.I_ZERO);
            if (String.IsNullOrEmpty(sErrMsg))
            {
                moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.SchoolWiseStandardDivisionTeacherAssignmentMasterStructDetails = moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;
                moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.UpdateTeacherDetailsForStandardDivision();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sErrMsg);
            }
        }

        /// <summary>
        /// This method returns the message informing about the dependencies.
        /// </summary>
        /// <returns></returns>
        public string CheckDependenciesForClassTeacher(string asTeacherName,int aiTeacherId)
        {
            string sReturn = "";
            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.ClassTeacherAssignment), aiTeacherId, asTeacherName, moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId);
            return sReturn;
        }
        public bool DeleteAssignStandardDivisionForTeacher(ArrayList aoArrayList)
        {
           
            //moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.SchoolWiseStandardDivisionTeacherAssignmentMasterStructDetails = moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;
            moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.DeleteAssignStandardDivisionForTeacher(aoArrayList);
            return true;
        }


        public bool DeleteAssignTeacherForStandardDivision(string asTeacherName)
        {
            string sErrMsg = CheckDependenciesForClassTeacher(asTeacherName,  moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miTeacherId);
            if (String.IsNullOrEmpty(sErrMsg))
            {
                moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.SchoolWiseStandardDivisionTeacherAssignmentMasterStructDetails = moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;
                moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.DeleteAssignTeacherForStandardDivision();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sErrMsg);
            }
            return true;
        }

        public bool IsStandardDivisionAssignToTeacher(int aiStandardId, int aiDivisionId)
        {
            // This method calls the DC method to check if the current Buyer login is duplicate or not.
            moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.SchoolWiseStandardDivisionTeacherAssignmentMasterStructDetails = moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;
            return moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.IsStandardDivisionAssignToTeacher(aiStandardId, aiDivisionId);
        }

        public bool DeleteAssignStandardDivisionForTeacher(int aiTeacherId)
        {
            moSchoolWiseStandardDivisionTeacherAssignmentMasterDC.DeleteAssignStandardDivisionForTeacher(aiTeacherId);
            return true;
        }

        public static DataTable GetAllClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolWiseStandardDivisionTeacherAssignmentMasterDC.GetAllClassTeachers(aiSchoolId, aiAcademicYearId);
        }

        public static DataTable GetAllClassTeachers1(int aiSchoolId, int aiAcademicYearId,int miuserid)
        {
            return SchoolWiseStandardDivisionTeacherAssignmentMasterDC.GetAllClassTeachers1(aiSchoolId, aiAcademicYearId,miuserid);
        }
        public static DataTable GetStandardDivisionOfTeacher(int aiTeacherId, int aiAcademicYearId)
        {
            return SchoolWiseStandardDivisionTeacherAssignmentMasterDC.GetStandardDivisionOfTeacher(aiTeacherId, aiAcademicYearId);

        }
        public static DataSet GetStdDivTeacherAssociation(int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolWiseStandardDivisionTeacherAssignmentMasterDC.GetStdDivTeacherAssociation(aiSchoolId, aiAcademicYearId);
        }

        #endregion

    }

}
