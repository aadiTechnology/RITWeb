using System;
using System.Data;
using DataCommunicator;

namespace BusinessLogic
{
    public class SchoolwiseDivisionSubjectMasterBL
    {


        #region DataMembers and properties

        #region Data members

        private SchoolwiseDivisionSubjectMasterDC.SchoolwiseDivisionSubjectMasterStruct moSchoolwiseDivisionSubjectMasterStruct;
        private SchoolwiseDivisionSubjectMasterDC moSchoolwiseDivisionSubjectMasterDC = new SchoolwiseDivisionSubjectMasterDC();

        #endregion
        #region Properties

        public int SchoolwiseDivisionSubjectId
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId = value; }
        }
        public string DivisionSubjectName
        {

            get { return moSchoolwiseDivisionSubjectMasterStruct.msDivisionSubjectName; }
            set { moSchoolwiseDivisionSubjectMasterStruct.msDivisionSubjectName = value; }
        }

        public int SchoolId
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miSchoolId; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miSchoolId = value; }
        }

        public int StandardId
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miStandardId; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miStandardId = value; }
        }

        public int DivisionId
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miDivisionId; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miDivisionId = value; }
        }

        public int SubjectId
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miSubjectId; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miSubjectId = value; }
        }

        public string IsDeleted
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.msIsDeleted; }
        set { moSchoolwiseDivisionSubjectMasterStruct.msIsDeleted = value; }
        }

        public int InsertedById
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miInsertedById; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miInsertedById = value; }
        }

        public DateTime InsertDate
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.mdtInsertDate; }
        set { moSchoolwiseDivisionSubjectMasterStruct.mdtInsertDate = value; }
        }

        public int UpdatedById
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.miUpdatedById; }
        set { moSchoolwiseDivisionSubjectMasterStruct.miUpdatedById = value; }
        }

        public DateTime UpdateDate
        {

        get { return moSchoolwiseDivisionSubjectMasterStruct.mdtUpdateDate; }
        set { moSchoolwiseDivisionSubjectMasterStruct.mdtUpdateDate = value; }
        }

        #endregion
        #endregion

        #region Constructors 

        public SchoolwiseDivisionSubjectMasterBL(){
        }
        public SchoolwiseDivisionSubjectMasterBL(int aiStandardDivisionId, int aiSubjectId){

            SchoolwiseDivisionSubjectMasterDC moSchoolwiseDivisionSubjectMasterDC = new SchoolwiseDivisionSubjectMasterDC(aiStandardDivisionId, aiSubjectId);
            moSchoolwiseDivisionSubjectMasterStruct =  moSchoolwiseDivisionSubjectMasterDC.SchoolwiseDivisionSubjectMasterStructDetails;

        }
        #endregion

        #region Public Methods 

        public Int32 InsertSchoolwiseDivisionSubjectMaster()
        {

        moSchoolwiseDivisionSubjectMasterDC.SchoolwiseDivisionSubjectMasterStructDetails =  moSchoolwiseDivisionSubjectMasterStruct;
        return moSchoolwiseDivisionSubjectMasterDC.InsertSchoolwiseDivisionSubjectMaster();
        }

        public void UpdateSchoolwiseDivisionSubjectMaster()
        {

        moSchoolwiseDivisionSubjectMasterDC.SchoolwiseDivisionSubjectMasterStructDetails =  moSchoolwiseDivisionSubjectMasterStruct;
        moSchoolwiseDivisionSubjectMasterDC.UpdateSchoolwiseDivisionSubjectMaster();
        }

        public void DeleteSchoolwiseDivisionSubjectMaster()
        {

        moSchoolwiseDivisionSubjectMasterDC.SchoolwiseDivisionSubjectMasterStructDetails =  moSchoolwiseDivisionSubjectMasterStruct;
        moSchoolwiseDivisionSubjectMasterDC.DeleteSchoolwiseDivisionSubjectMaster();
        }

        #endregion

        #region Static Methods 

        /// <summary>
        /// Returns dataset containing 3 tables.
        /// Table 1 - All Standard-divisions associated for the school.
        /// Table 2 - All Subjects defined for the school.
        /// Table 3 - All Standard-divisions-subjects(if any) associated for the school.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicId"></param>
        /// <returns></returns>

        public static DataSet GetStandardDivisionSubjectsAssociation(int aiSchoolId, int aiAcademicId)
        {
            return SchoolwiseDivisionSubjectMasterDC.GetStandardDivisionSubjectsAssociation(aiSchoolId, aiAcademicId);
        }

        #endregion Static Methods

    }   

}
