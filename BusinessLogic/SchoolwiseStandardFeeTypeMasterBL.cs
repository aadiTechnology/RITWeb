// File Name       : SchoolwiseStandardFeeTypeMasterDC
// Purpose         : This class is used to manage SchoolwiseStandardFeeTypeMaster details.
// Date Of creation: 06/02/2008
// Author Name     : Anugandha 

using System;
using System.Data;
using System.Collections.ObjectModel;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class SchoolwiseStandardFeeTypeMasterBL
    {

        #region Data Members

        private SchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStruct moSchoolwiseStandardFeeTypeMasterStruct;

        private SchoolwiseStandardFeeTypeMasterDC moSchoolwiseStandardFeeTypeMasterDC;

        private Constants.Action eAction;

        private Collection<SchoolwiseStandardFeeTypeMasterBL> moSchoolwiseStandardFeeTypeMasterBL;

        #endregion

        #region Constructors

        public SchoolwiseStandardFeeTypeMasterBL()
        {
            moSchoolwiseStandardFeeTypeMasterDC = new SchoolwiseStandardFeeTypeMasterDC();
        }

        public SchoolwiseStandardFeeTypeMasterBL(int miSchoolWiseStandardFeeTypeId)
        {
            moSchoolwiseStandardFeeTypeMasterDC = new SchoolwiseStandardFeeTypeMasterDC(miSchoolWiseStandardFeeTypeId);
            moSchoolwiseStandardFeeTypeMasterStruct = moSchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStructDetails;
        }
        public SchoolwiseStandardFeeTypeMasterBL(int aiStandardId, int aiFeeTypeId)
        {
            moSchoolwiseStandardFeeTypeMasterDC = new SchoolwiseStandardFeeTypeMasterDC(aiStandardId, aiFeeTypeId);
            moSchoolwiseStandardFeeTypeMasterStruct = moSchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStructDetails;
        }

        #endregion

        #region Properties

        public int SchoolWise_Standard_FeeType_Id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId = value;
            }
        }
        public string StandardFeeTypeName
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.msStandardFeeTypeName;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.msStandardFeeTypeName = value;
            }
        }

        public int Standard_Id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.miStandardId;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.miStandardId = value;
            }
        }

        public int Fee_Type_Id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId = value;
            }
        }

        public int Interval
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.iInterval;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.iInterval = value;
            }
        }

        public int School_Id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.miSchoolId;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.miSchoolId = value;
            }
        }

        public int academic_Year_Id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.miacademicYearId;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.miacademicYearId = value;
            }
        }

        public string Is_Deleted
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.msIsDeleted;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime Insert_Date
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.mdtInsertDate;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.mdtInsertDate = value;
            }
        }

        public string Inserted_By_id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.msInsertedByid;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.msInsertedByid = value;
            }
        }

        public DateTime Update_Date
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.mdtUpdateDate = value;
            }
        }

        public string Updated_By_Id
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct.msUpdatedById;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct.msUpdatedById = value;
            }
        }

        public Collection<SchoolwiseStandardFeeTypeMasterBL> SchoolWiseFeeTypeCollection
        {
            get { return moSchoolwiseStandardFeeTypeMasterBL; }
            set { moSchoolwiseStandardFeeTypeMasterBL = value; }
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
        /// particular standard into SchoolwiseStandardFeeTypeMaster table
        /// </summary>
        public string InsertSchoolwiseStandardFeeTypeMaster()
        {
            moSchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStructDetails = moSchoolwiseStandardFeeTypeMasterStruct;
            return moSchoolwiseStandardFeeTypeMasterDC.InsertSchoolwiseStandardFeeTypeMaster();
        }

        /// <summary>
        /// This method is used for Updating records of a particular standard 
        /// from SchoolwiseStandardFeeTypeMaster table.
        /// </summary>        
        public void UpdateSchoolwiseStandardFeeTypeMaster()
        {
            moSchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStructDetails = moSchoolwiseStandardFeeTypeMasterStruct;
            moSchoolwiseStandardFeeTypeMasterDC.UpdateSchoolwiseStandardFeeTypeMaster();
        }

        /// <summary>
        /// This method is used to update Standard fee type master.
        /// </summary>
        /// <returns></returns>
        public string UpdateStandardFeeTypeMaster()
        {
            moSchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStructDetails = moSchoolwiseStandardFeeTypeMasterStruct;
            return moSchoolwiseStandardFeeTypeMasterDC.UpdateStandardFeeTypeMaster();
        }
        /// <summary>
        /// This method is used for Deleting deselected tests of a particular standard 
        /// from SchoolwiseStandardFeeTypeMaster table.
        /// </summary>        
        public string DeleteSchoolwiseStandardFeeTypeMaster()
        {
            moSchoolwiseStandardFeeTypeMasterDC.SchoolwiseStandardFeeTypeMasterStructDetails = moSchoolwiseStandardFeeTypeMasterStruct;
            return moSchoolwiseStandardFeeTypeMasterDC.DeleteSchoolwiseStandardFeeTypeMaster();
        }
        #endregion
    }

    public class SchoolwiseStandardFeeTypeMasterCollectionBL
    {

        #region DataMembers

        SchoolwiseStandardFeeTypeMasterCollectionDC moSchoolwiseStandardFeeTypeMasterCollectionDC = null;

        #endregion

        #region Constructor

        public SchoolwiseStandardFeeTypeMasterCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moSchoolwiseStandardFeeTypeMasterCollectionDC = new SchoolwiseStandardFeeTypeMasterCollectionDC(aiSchoolId, aiAcademicYearId);
        }

        public SchoolwiseStandardFeeTypeMasterCollectionBL(int aiSchoolId)
        {
            moSchoolwiseStandardFeeTypeMasterCollectionDC = new SchoolwiseStandardFeeTypeMasterCollectionDC(aiSchoolId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get all fee types for particular standard
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetAllFeeTypesForStandard(int aiStandardId)
        {
            return moSchoolwiseStandardFeeTypeMasterCollectionDC.GetAllFeeTypesForStandard(aiStandardId);
        }

        public DataSet GetStdExamAssociation()
        {
            return moSchoolwiseStandardFeeTypeMasterCollectionDC.GetStdExamAssociation();
        }
        #endregion
    }
}
