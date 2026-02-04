using System.Collections;
using DataCommunicator;

namespace BusinessLogic
{
    public class TeacherEducationDetailsBL
    {
        #region " Data Members & Properties"

        #region " Data Members "

        // Object of the TeacherEducationDetailsDC Class. 
        //Using this object call the methods of the TeacherEducationDetailsDC Class.

        TeacherEducationDetailsDC moTeacherEducationDetailsDC;
       
        TeacherEducationDetailsDC.TeacherEduInfoStruct moTeacherEduInfoStruct;
        #endregion

        #region " Properties "

        public int TeacherEducationId
        {
            get
            {
                return moTeacherEduInfoStruct.miTeacherEducationId;
            }
            set
            {
                moTeacherEduInfoStruct.miTeacherEducationId = value;
            }
        }

        public int QualificationId
        {
            get
            {
                return moTeacherEduInfoStruct.miQualificationId;
            }
            set
            {
                moTeacherEduInfoStruct.miQualificationId = value;
            }
        }

        public string Specialization
        {
            get {

                return moTeacherEduInfoStruct.miSpecialization;
            }
            set {
                moTeacherEduInfoStruct.miSpecialization = value;
            }
        
        }
        public int TeacherId
        {
            get
            {
                return moTeacherEduInfoStruct.miTeacherId;
            }
            set
            {
                moTeacherEduInfoStruct.miTeacherId = value;
            }
        }
     
        public int YearOfPassingId
        {
            get
            {
                return moTeacherEduInfoStruct.miYearOfPassingId;
            }
            set
            {
                moTeacherEduInfoStruct.miYearOfPassingId = value;
            }
        }

        public string PassingUniversity
        {

            get
            {
                return moTeacherEduInfoStruct.msPassingUniversity;
            }
            set
            {
                moTeacherEduInfoStruct.msPassingUniversity = value;
            }
        }

        public int ClassId
        {
            get
            {
                return moTeacherEduInfoStruct.miClassId;
            }
            set
            {
                moTeacherEduInfoStruct.miClassId = value;
            }
        }
          
        public int InsertedById
        {
            get
            {
                return moTeacherEduInfoStruct.miInsertedById;
            }
            set
            {
                moTeacherEduInfoStruct.miInsertedById = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moTeacherEduInfoStruct.miUpdatedById;
            }
            set
            {
                moTeacherEduInfoStruct.miUpdatedById = value;
            }
        }

        #endregion

        #endregion

        #region " OverLoaded Constructors "

        public TeacherEducationDetailsBL()
        {
            //Default constructor
            moTeacherEducationDetailsDC = new TeacherEducationDetailsDC();
        }

       #endregion

        #region " Public Methods "

        /// <summary>
        /// This method is used to get all details from UI to insert in database.
        /// </summary>
        /// <returns></returns>
 
        public string InsertTeacherEducationDetails()
        {           
            // This Function is used to insert the record in to database. 
            moTeacherEducationDetailsDC.TeacherEduInfoStructure = moTeacherEduInfoStruct;
            return moTeacherEducationDetailsDC.GetEducationDetailsInsertStatement();
            //return false;
        }

        #endregion
    }

    public class TeacherEducationDetailsCollectionBL
    {
        private TeacherEducationDetailsCollectionDC moTeacherEducationDetailsCollectionDC = null;

        public TeacherEducationDetailsCollectionBL()
        {
            moTeacherEducationDetailsCollectionDC = new TeacherEducationDetailsCollectionDC();
        }

        public bool DeleteTeacherEducationDetails(ArrayList aoArrDeleteTeacherIds)
        {
            moTeacherEducationDetailsCollectionDC.DeleteTeacherEducationDetails(aoArrDeleteTeacherIds);
            return true;
        }
        public static string RemoveEducationalDetailsForTeacherId(int aiTeacherId)
        {
            return TeacherEducationDetailsCollectionDC.RemoveEducationalDetailsForTeacherId(aiTeacherId);
        }
      
        public bool DeleteTeacherEducationDetails(int aiTeacherId)
        {
            moTeacherEducationDetailsCollectionDC.DeleteTeacherEducationDetails(aiTeacherId);
            return true;
        }
    }
}
