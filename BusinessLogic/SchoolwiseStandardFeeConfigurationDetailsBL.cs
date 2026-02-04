// Class Name       :- SchoolwiseStandardFeeConfigurationDetailsDC
// Purpose          :- This class is used to manage SchoolwiseStandardFeeConfigurationDetails details.
// Date Of creation :- 2/7/2008
// Author Name      :- 


using System;
using System.Collections.ObjectModel;
using Utility;
using DataCommunicator;




namespace BusinessLogic
{


    public class SchoolwiseStandardFeeConfigurationDetailsBL
    {

        private SchoolwiseStandardFeeConfigurationDetailsDC.SchoolwiseStandardFeeConfigurationDetailsStruct moSchoolwiseStandardFeeConfigurationDetailsStruct;

        private SchoolwiseStandardFeeConfigurationDetailsDC moSchoolwiseStandardFeeConfigurationDetailsDC;

        private Constants.Action eAction;

        private Collection<SchoolwiseStandardFeeConfigurationDetailsBL> moSchoolwiseStandardFeeConfigurationDetailsBL;

        public SchoolwiseStandardFeeConfigurationDetailsBL()
        {
            moSchoolwiseStandardFeeConfigurationDetailsDC = new SchoolwiseStandardFeeConfigurationDetailsDC();
        }

        public SchoolwiseStandardFeeConfigurationDetailsBL(int miSchoolwiseStandardFeeConfigurationDetailId)
        {
            moSchoolwiseStandardFeeConfigurationDetailsDC = new SchoolwiseStandardFeeConfigurationDetailsDC(miSchoolwiseStandardFeeConfigurationDetailId);
            moSchoolwiseStandardFeeConfigurationDetailsStruct = moSchoolwiseStandardFeeConfigurationDetailsDC.SchoolwiseStandardFeeConfigurationDetailsStructDetails;
        }

        public int Schoolwise_Standard_Fee_Configuration_Detail_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationDetailId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationDetailId = value;
            }
        }

        public int Schoolwise_Standard_Fee_Configuration_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationId = value;
            }
        }

        public int Fee_SubType_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeSubTypeId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeSubTypeId = value;
            }
        }

        public double Fee_AmountOld
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountOld;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountOld = value;
            }
        }

        public double Fee_AmountNew
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountNew;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountNew = value;
            }
        }

        public double TotalFee_AmountNew
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miTotalFeeAmountNew;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miTotalFeeAmountNew = value;
            }
        }

        public double TotalFee_AmountOld
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miTotalFeeAmountOld;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miTotalFeeAmountOld = value;
            }
        }
        public int Standard_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miStandardId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miStandardId = value;
            }
        }

        public int School_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolId = value;
            }
        }

        public int academic_Year_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.miacademicYearId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.miacademicYearId = value;
            }
        }

        public string Is_Deleted
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.msIsDeleted;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.msIsDeleted = value;
            }
        }

        public DateTime Insert_Date
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.mdtInsertDate;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.mdtInsertDate = value;
            }
        }

        public string Inserted_By_id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.msInsertedByid;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.msInsertedByid = value;
            }
        }

        public DateTime Update_Date
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.mdtUpdateDate = value;
            }
        }

        public string Updated_By_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct.msUpdatedById;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct.msUpdatedById = value;
            }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        public Collection<SchoolwiseStandardFeeConfigurationDetailsBL> SchoolWiseFeeTypeCollection
        {
            get { return moSchoolwiseStandardFeeConfigurationDetailsBL; }
            set { moSchoolwiseStandardFeeConfigurationDetailsBL = value; }
        }

        public string InsertSchoolwiseStandardFeeConfigurationDetails()
        {
            moSchoolwiseStandardFeeConfigurationDetailsDC.SchoolwiseStandardFeeConfigurationDetailsStructDetails = moSchoolwiseStandardFeeConfigurationDetailsStruct;
            return moSchoolwiseStandardFeeConfigurationDetailsDC.InsertSchoolwiseStandardFeeConfigurationDetails();
        }

        //public string InsertWhileInEdit()
        //{
        //    moSchoolwiseStandardFeeConfigurationDetailsDC.SchoolwiseStandardFeeConfigurationDetailsStructDetails = moSchoolwiseStandardFeeConfigurationDetailsStruct;
        //    return moSchoolwiseStandardFeeConfigurationDetailsDC.InsertWhileInEdit();
        //}

        //public  string UpdateSchoolwiseStandardFeeConfigurationDetails()
        //{
        //    moSchoolwiseStandardFeeConfigurationDetailsDC.SchoolwiseStandardFeeConfigurationDetailsStructDetails = moSchoolwiseStandardFeeConfigurationDetailsStruct;
        //    return moSchoolwiseStandardFeeConfigurationDetailsDC.UpdateSchoolwiseStandardFeeConfigurationDetails();
        //}

        //public string DeleteSchoolwiseStandardFeeConfigurationDetails(int aiFeeSubTypeId)//, int aiStandardFeeConfigId)
        //{
        //    moSchoolwiseStandardFeeConfigurationDetailsDC.SchoolwiseStandardFeeConfigurationDetailsStructDetails = moSchoolwiseStandardFeeConfigurationDetailsStruct;
        //    return moSchoolwiseStandardFeeConfigurationDetailsDC.DeleteSchoolwiseStandardFeeConfigurationDetails(aiFeeSubTypeId);//, aiStandardFeeConfigId);
        //}


    }

    public class SchoolwiseStandardFeeConfigurationDetailsCollectionBL
    {
        #region Data Members
        SchoolwiseStandardFeeConfigurationDetailsCollectionDC oSchoolwiseStandardFeeConfigurationDetailsCollectionDC;
        #endregion

        #region Constructors

        public SchoolwiseStandardFeeConfigurationDetailsCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            oSchoolwiseStandardFeeConfigurationDetailsCollectionDC = new SchoolwiseStandardFeeConfigurationDetailsCollectionDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion

        public static string GetPhysicalDeleteStatement(int aiConfigurationId)
        {
            return SchoolwiseStandardFeeConfigurationDetailsCollectionDC.GetPhysicalDeleteStatement(aiConfigurationId);
        }

    }
}
