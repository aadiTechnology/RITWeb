using System;
using System.Collections;
using System.Data;
using DataCommunicator;

namespace BusinessLogic
{

    public class SchoolWiseStandardSubjectMasterBL
    {
        #region DataMembers and properties

        #region Data members

        private SchoolWiseStandardSubjectMasterDC.SchoolWiseStandardSubjectMasterStruct moSchoolWiseStandardSubjectMasterStruct;
        private SchoolWiseStandardSubjectMasterDC moSchoolWiseStandardSubjectMasterDC = new SchoolWiseStandardSubjectMasterDC();
        private bool mbIsValid;
        #endregion
        #region Properties

        public int SchoolWiseStandardSubjectId
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId; }
        set { moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId = value; }
        }

        public int StandardId
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.miStandardId; }
        set { moSchoolWiseStandardSubjectMasterStruct.miStandardId = value; }
        }

        public string SubjectId
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.msSubjectId; }
        set { moSchoolWiseStandardSubjectMasterStruct.msSubjectId = value; }
        }
        public string StdSubjectName
        {

            get { return moSchoolWiseStandardSubjectMasterStruct.msSubjectId; }
            set { moSchoolWiseStandardSubjectMasterStruct.msSubjectId = value; }
        }

        public string SchoolId
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.msSchoolId; }
        set { moSchoolWiseStandardSubjectMasterStruct.msSchoolId = value; }
        }

        public string IsDeleted
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.msIsDeleted; }
        set { moSchoolWiseStandardSubjectMasterStruct.msIsDeleted = value; }
        }

        public DateTime InsertDate
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.mdtInsertDate; }
        set { moSchoolWiseStandardSubjectMasterStruct.mdtInsertDate = value; }
        }

        public int InsertedByid
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.miInsertedByid; }
        set { moSchoolWiseStandardSubjectMasterStruct.miInsertedByid = value; }
        }

        public DateTime UpdateDate
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.mdtUpdateDate; }
        set { moSchoolWiseStandardSubjectMasterStruct.mdtUpdateDate = value; }
        }

        public int UpdatedById
        {

        get { return moSchoolWiseStandardSubjectMasterStruct.miUpdatedById; }
        set { moSchoolWiseStandardSubjectMasterStruct.miUpdatedById = value; }
        }

        public bool IsValid
        {
            get
            {
                return mbIsValid;
            }
            set
            {
                mbIsValid = value;
            }
        }

        #endregion
        #endregion

        #region Constructors 

        public SchoolWiseStandardSubjectMasterBL(){
        }
       
        public SchoolWiseStandardSubjectMasterBL(int aiStandardId, int aiSubjectId)
        {


            moSchoolWiseStandardSubjectMasterDC = new SchoolWiseStandardSubjectMasterDC(aiStandardId,aiSubjectId);
            moSchoolWiseStandardSubjectMasterStruct = moSchoolWiseStandardSubjectMasterDC.SchoolWiseStandardSubjectMasterStructDetails;

        }
        #endregion

        #region Public Methods 

        public Int32 InsertSchoolWiseStandardSubjectMaster()
        {

        moSchoolWiseStandardSubjectMasterDC.SchoolWiseStandardSubjectMasterStructDetails =  moSchoolWiseStandardSubjectMasterStruct;
        return moSchoolWiseStandardSubjectMasterDC.InsertSchoolWiseStandardSubjectMaster();
        }
        public void UpdateSchoolWiseStandardSubjectMaster()
        {

        moSchoolWiseStandardSubjectMasterDC.SchoolWiseStandardSubjectMasterStructDetails =  moSchoolWiseStandardSubjectMasterStruct;
        moSchoolWiseStandardSubjectMasterDC.UpdateSchoolWiseStandardSubjectMaster();
        }
        public void DeleteSchoolWiseStandardSubjectMaster()
        {

        moSchoolWiseStandardSubjectMasterDC.SchoolWiseStandardSubjectMasterStructDetails =  moSchoolWiseStandardSubjectMasterStruct;
        moSchoolWiseStandardSubjectMasterDC.DeleteSchoolWiseStandardSubjectMaster();
        }

        public ArrayList GetAllSubjectsforStandard(int aiSchoolId, int aiStandardId)
        {
            return moSchoolWiseStandardSubjectMasterDC.GetAllSubjectsforStandard(aiSchoolId, aiStandardId);
        }

        /// <summary>
        /// This method is used to get standard id of year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static int GetStandardOfYear(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            return SchoolWiseStandardSubjectMasterDC.GetStandardOfYear(aiSchoolId,aiAcademicYrId,aiStudentId);
        }

        public DataSet GetStdSubjectAssociation(int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolWiseStandardSubjectMasterDC.GetStdSubjectAssociation(aiSchoolId, aiAcademicYearId);
        }

      

        #endregion

    }

}
