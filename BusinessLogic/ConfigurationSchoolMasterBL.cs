using System;
using DataCommunicator;

namespace BusinessLogic
{

    public class ConfigurationSchoolMasterBL
    {

        #region DataMembers and properties

        #region Data members

        private ConfigurationSchoolMasterDC.ConfigurationSchoolMasterStruct moConfigurationSchoolMasterStruct;
        private ConfigurationSchoolMasterDC moConfigurationSchoolMasterDC = new ConfigurationSchoolMasterDC();

        #endregion
        #region Properties

        public int ConfigId
        {

            get { return moConfigurationSchoolMasterStruct.miConfigId; }
            set { moConfigurationSchoolMasterStruct.miConfigId = value; }
        }

        public int OriginalConfigId
        {

            get { return moConfigurationSchoolMasterStruct.miOriginalConfigId; }
            set { moConfigurationSchoolMasterStruct.miOriginalConfigId = value; }
        }

        public int SchoolId
        {

            get { return moConfigurationSchoolMasterStruct.miSchoolId; }
            set { moConfigurationSchoolMasterStruct.miSchoolId = value; }
        }
        public int AcademicYearId
        {

            get { return moConfigurationSchoolMasterStruct.miAcademicYearId; }
            set { moConfigurationSchoolMasterStruct.miAcademicYearId  = value; }
        }
        
        public int FinancialYearId
        {

            get { return moConfigurationSchoolMasterStruct.miFinancialYearId; }
            set { moConfigurationSchoolMasterStruct.miFinancialYearId  = value; }
        }

        public char IsConfigure
        {

            get { return moConfigurationSchoolMasterStruct.msIsConfigure; }
            set { moConfigurationSchoolMasterStruct.msIsConfigure = value; }
        }

        public char IsDeleted
        {

            get { return moConfigurationSchoolMasterStruct.msIsDeleted; }
            set { moConfigurationSchoolMasterStruct.msIsDeleted = value; }
        }

        public int InsertedById
        {

            get { return moConfigurationSchoolMasterStruct.miInsertedById; }
            set { moConfigurationSchoolMasterStruct.miInsertedById = value; }
        }

        public DateTime InsertDate
        {

            get { return moConfigurationSchoolMasterStruct.mdtInsertDate; }
            set { moConfigurationSchoolMasterStruct.mdtInsertDate = value; }
        }

        public int UpdateById
        {

            get { return moConfigurationSchoolMasterStruct.miUpdateById; }
            set { moConfigurationSchoolMasterStruct.miUpdateById = value; }
        }

        public DateTime UpdatedDate
        {

            get { return moConfigurationSchoolMasterStruct.mdtUpdatedDate; }
            set { moConfigurationSchoolMasterStruct.mdtUpdatedDate = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public ConfigurationSchoolMasterBL()
        {
        }
        #endregion

        #region Public Methods

        public Int32 InsertConfigurationSchoolMaster()
        {

            moConfigurationSchoolMasterDC.ConfigurationSchoolMasterStructDetails = moConfigurationSchoolMasterStruct;
            if (!IsSchoolConfigured())
            {
                return moConfigurationSchoolMasterDC.InsertConfigurationSchoolMaster();
            }
            return 0;
        }

        public void UpdateConfigurationSchoolMaster()
        {

            moConfigurationSchoolMasterDC.ConfigurationSchoolMasterStructDetails = moConfigurationSchoolMasterStruct;
            moConfigurationSchoolMasterDC.UpdateConfigurationSchoolMaster();
        }

        public void DeleteConfigurationSchoolMaster()
        {

            moConfigurationSchoolMasterDC.ConfigurationSchoolMasterStructDetails = moConfigurationSchoolMasterStruct;
            moConfigurationSchoolMasterDC.DeleteConfigurationSchoolMaster();
        }

        public bool IsSchoolConfigured()
        {
            moConfigurationSchoolMasterDC.ConfigurationSchoolMasterStructDetails = moConfigurationSchoolMasterStruct;
            return moConfigurationSchoolMasterDC.IsSchoolConfigured();
        }

        #endregion

    }

}
