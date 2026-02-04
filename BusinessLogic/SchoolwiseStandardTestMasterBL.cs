// File Name       : SchoolwiseStandardTestMasterBL.cs
// Purpose         :This class is used to manage SchoolwiseStandardTestMaster details.
// Date Of creation:1/31/2008
// Author Name     :Anugandha

using System.Data;
using System.Collections.ObjectModel;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{
    public class SchoolwiseStandardTestMasterBL
    {
        #region Data Members

        private SchoolwiseStandardTestMasterDC.SchoolwiseStandardTestMasterStruct moSchoolwiseStandardTestMasterStruct;

        private SchoolwiseStandardTestMasterDC moSchoolwiseStandardTestMasterDC;

        private Constants.Action eAction;

        private Collection<SchoolwiseStandardTestMasterBL> moSchoolwiseStandardTestMasterBL;

        #endregion

        #region Constructors

        public SchoolwiseStandardTestMasterBL()
        {
            moSchoolwiseStandardTestMasterDC = new SchoolwiseStandardTestMasterDC();
        }

        public SchoolwiseStandardTestMasterBL(int miSchoolwiseStandardTestId)
        {
            moSchoolwiseStandardTestMasterDC = new SchoolwiseStandardTestMasterDC(miSchoolwiseStandardTestId);
            moSchoolwiseStandardTestMasterStruct = moSchoolwiseStandardTestMasterDC.SchoolwiseStandardTestMasterStructDetails;
        }
        public SchoolwiseStandardTestMasterBL(int aiStandardId, int aiExamId)
        {
            moSchoolwiseStandardTestMasterDC = new SchoolwiseStandardTestMasterDC(aiStandardId, aiExamId);
            moSchoolwiseStandardTestMasterStruct = moSchoolwiseStandardTestMasterDC.SchoolwiseStandardTestMasterStructDetails;
        }

        #endregion

        #region Properties

        public virtual int Schoolwise_Standard_Test_Id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.miSchoolwiseStandardTestId;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.miSchoolwiseStandardTestId = value;
            }
        }
        public  string Standard_Test_Name
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.msStandardTestName;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.msStandardTestName = value;
            }
        }

        public virtual int Standard_Id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.miStandardId;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.miStandardId = value;
            }
        }

        public virtual int SchoolWise_Test_Id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.miSchoolWiseTestId;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.miSchoolWiseTestId = value;
            }
        }

        public virtual int School_Id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.miSchoolId;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.miSchoolId = value;
            }
        }

        public virtual int academic_Year_Id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.miacademicYearId;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.miacademicYearId = value;
            }
        }

        public virtual string Is_Deleted
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.msIsDeleted;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.msIsDeleted = value;
            }
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.mdtInsertDate;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.mdtInsertDate = value;
            }
        }

        public virtual string Inserted_By_id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.msInsertedByid;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.msInsertedByid = value;
            }
        }

        public virtual System.DateTime Update_Date
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.mdtUpdateDate = value;
            }
        }

        public virtual string Updated_By_Id
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct.msUpdatedById;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct.msUpdatedById = value;
            }
        }

        public Collection<SchoolwiseStandardTestMasterBL> SchoolWiseTestCollection
        {
            get { return moSchoolwiseStandardTestMasterBL; }
            set { moSchoolwiseStandardTestMasterBL = value; }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

     #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to For Inserting selected tests to 
        /// particular standard into SchoolwiseStandardTestMaster table
        /// </summary>
        public string InsertSchoolwiseStandardTestMaster()
        {
            moSchoolwiseStandardTestMasterDC.SchoolwiseStandardTestMasterStructDetails = moSchoolwiseStandardTestMasterStruct;
            return moSchoolwiseStandardTestMasterDC.InsertSchoolwiseStandardTestMaster();
        }

        /// <summary>
        /// This method is used for Updating records of a particular standard 
        /// from SchoolwiseStandardTestMaster table.
        /// </summary>        
        public void UpdateSchoolwiseStandardTestMaster()
        {
            moSchoolwiseStandardTestMasterDC.SchoolwiseStandardTestMasterStructDetails = moSchoolwiseStandardTestMasterStruct;
            moSchoolwiseStandardTestMasterDC.UpdateSchoolwiseStandardTestMaster();
        }

        /// <summary>
        /// This method is used for Deleting deselected tests of a particular standard 
        /// from SchoolwiseStandardTestMaster table.
        /// </summary>        
        public string DeleteSchoolwiseStandardTestMaster()
        {

            moSchoolwiseStandardTestMasterDC.SchoolwiseStandardTestMasterStructDetails = moSchoolwiseStandardTestMasterStruct;
            return moSchoolwiseStandardTestMasterDC.DeleteSchoolwiseStandardTestMaster();
        }
       

        #endregion

        public static int GetTestCount(int aiSchoolId,int aiAcademicYearId)
        {
            return SchoolwiseStandardTestMasterDC.GetTestCount(aiSchoolId, aiAcademicYearId);
        }

        public static int GetGeneratedTestCount(int aiSchoolId, int aiAcademicYearId, int aiStandardDivId)
        {
            return SchoolwiseStandardTestMasterDC.GetGeneratedTestCount(aiSchoolId, aiAcademicYearId, aiStandardDivId);
        }

        public static int IsStandardWithGrade(int aiStandardDivId, int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolwiseStandardTestMasterDC.IsStandardWithGrade( aiStandardDivId, aiSchoolId, aiAcademicYearId);
        }
    }

    public class SchoolwiseStandardTestMasterCollectionBL
    {
        
        #region DataMembers
        SchoolwiseStandardTestMasterCollectionDC moSchoolwiseStandardTestMasterCollectionDC = null;
        #endregion
        
        #region Constructor

        public SchoolwiseStandardTestMasterCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moSchoolwiseStandardTestMasterCollectionDC = new SchoolwiseStandardTestMasterCollectionDC(aiSchoolId, aiAcademicYearId);
        }

        public SchoolwiseStandardTestMasterCollectionBL(int aiSchoolId)
        {
            moSchoolwiseStandardTestMasterCollectionDC = new SchoolwiseStandardTestMasterCollectionDC(aiSchoolId);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get all records from Schoolwise_Standard_Test_Master.
        /// </summary>
        /// <returns>dataset</returns>
        public DataTable GetAllTestsForStandard(int aiStandardId)
        {
            return moSchoolwiseStandardTestMasterCollectionDC.GetAllTestsForStandard(aiStandardId);
        }

        /// <summary>
        /// This method is used to get all standard names
        ///  from Schoolwise_Standard_Test_Master.
        /// </summary>
        /// <returns>dataset</returns>
        public DataTable GetConfiguredStandardName()
        {
            return moSchoolwiseStandardTestMasterCollectionDC.GetConfiguredStandardName();
        }

        public DataSet GetStdExamAssociation()
        {
            return moSchoolwiseStandardTestMasterCollectionDC.GetStdExamAssociation();
        }

        #endregion


        public void UpdateExamSortOrder(int ischoolId, int iAcademicYearId, int iStandardId, string sXmlExamOrder)
        {
            moSchoolwiseStandardTestMasterCollectionDC.UpdateExamSortOrder(ischoolId, iAcademicYearId, iStandardId, sXmlExamOrder);
        }
    }
}
