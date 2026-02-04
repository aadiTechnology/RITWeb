// File Name       : SchoolwiseStandardFeeConfigurationMasterDC
// Purpose         : This class is used to manage SchoolwiseStandardFeeConfigurationMaster details.
// Date Of creation: 07/02/2008
// Author Name     : Anugandha 

using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;

using DataCommunicator;

namespace BusinessLogic
{

    public class SchoolwiseStandardFeeConfigurationMasterBL : BusinessLogicBaseBL
    {

        #region Data Members

        private SchoolwiseStandardFeeConfigurationMasterDC.SchoolwiseStandardFeeConfigurationMasterStruct moSchoolwiseStandardFeeConfigurationMasterStruct;

        private SchoolwiseStandardFeeConfigurationMasterDC moSchoolwiseStandardFeeConfigurationMasterDC = null;
        private Constants.Action eAction;

        private Collection<SchoolwiseStandardFeeConfigurationDetailsBL> moSchoolwiseStandardFeeConfigurationDetailsBL;
        #endregion

        #region Constructors

        public SchoolwiseStandardFeeConfigurationMasterBL()
        {
            moSchoolwiseStandardFeeConfigurationMasterDC = new SchoolwiseStandardFeeConfigurationMasterDC();
        }

        public SchoolwiseStandardFeeConfigurationMasterBL(int miSchoolwiseStandardFeeConfigurationId)
        {
            moSchoolwiseStandardFeeConfigurationMasterDC = new SchoolwiseStandardFeeConfigurationMasterDC(miSchoolwiseStandardFeeConfigurationId);
            moSchoolwiseStandardFeeConfigurationMasterStruct = moSchoolwiseStandardFeeConfigurationMasterDC.SchoolwiseStandardFeeConfigurationMasterStructDetails;
        }

        public SchoolwiseStandardFeeConfigurationMasterBL(int aiStandardId, int aiFeeypeId)
        {
            moSchoolwiseStandardFeeConfigurationMasterDC = new SchoolwiseStandardFeeConfigurationMasterDC(aiStandardId, aiFeeypeId);
            moSchoolwiseStandardFeeConfigurationMasterStruct = moSchoolwiseStandardFeeConfigurationMasterDC.SchoolwiseStandardFeeConfigurationMasterStructDetails;
        }
        #endregion

        #region Properties

        public int Schoolwise_Standard_Fee_Configuration_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId = value;
            }
        }

        public int Fee_Type_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId = value;
            }
        }

        public double Total_FeesForOld
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld = value;
            }
        }

        public double Total_FeesForNew
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForNew;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForNew = value;
            }
        }

        public int Standard_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId = value;
            }
        }

        public int School_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId = value;
            }
        }

        public int academic_Year_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId = value;
            }
        }

        public string Is_Deleted
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.msIsDeleted;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime Insert_Date
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.mdtInsertDate;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.mdtInsertDate = value;
            }
        }

        public string Inserted_By_id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.msInsertedByid;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.msInsertedByid = value;
            }
        }

        public DateTime Update_Date
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.mdtUpdateDate = value;
            }
        }

        public string Updated_By_Id
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.msUpdatedById;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.msUpdatedById = value;
            }
        }

        public int AmountForNewStudent
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miAmountForNewStudent;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miAmountForNewStudent = value;
            }
        }

        public int AmountForOldStudent
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.miAmountForOldStudent;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.miAmountForOldStudent = value;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.mdDueDate;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.mdDueDate = value;
            }

        }

        public bool IsStudentPayFee
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct.mbIsStudentPayFee;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct.mbIsStudentPayFee = value;
            }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        public Collection<SchoolwiseStandardFeeConfigurationDetailsBL> SchoolWiseFeeSubTypeCollection
        {
            get { return moSchoolwiseStandardFeeConfigurationDetailsBL; }
            set { moSchoolwiseStandardFeeConfigurationDetailsBL = value; }
        }
        #endregion

        #region Public Methods
        public string CheckDependenciesForFees(string asFeeType)
        {
            string sReturn = "";

            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.StandardFeeSubtypes), moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId, asFeeType, moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId);
            return sReturn;
        }
        public string InsertSchoolwiseStandardFeeConfigurationMaster()
        {
            moSchoolwiseStandardFeeConfigurationMasterDC.SchoolwiseStandardFeeConfigurationMasterStructDetails = moSchoolwiseStandardFeeConfigurationMasterStruct;
            return moSchoolwiseStandardFeeConfigurationMasterDC.InsertSchoolwiseStandardFeeConfigurationMaster();
        }

        public string UpdateSchoolwiseStandardFeeConfigurationMaster()
        {
            moSchoolwiseStandardFeeConfigurationMasterDC.SchoolwiseStandardFeeConfigurationMasterStructDetails = moSchoolwiseStandardFeeConfigurationMasterStruct;
            return moSchoolwiseStandardFeeConfigurationMasterDC.UpdateSchoolwiseStandardFeeConfigurationMaster();
        }

        //public void DeleteSchoolwiseStandardFeeConfigurationMaster()
        //{
        //    moSchoolwiseStandardFeeConfigurationMasterDC.SchoolwiseStandardFeeConfigurationMasterStructDetails = moSchoolwiseStandardFeeConfigurationMasterStruct;
        //    moSchoolwiseStandardFeeConfigurationMasterDC.DeleteSchoolwiseStandardFeeConfigurationMaster();
        //}

        public void UpdateStandardFeeTypes(Collection<SchoolwiseStandardFeeConfigurationDetailsBL> aoFeeSubTypes, string asStdFeeType)
        {
            

            string sReferenceMessage = "";
            //if (moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId != 0)
            //{
            //     sReferenceMessage = CheckDependenciesForFees(asStdFeeType);
 
            //}

            if (sReferenceMessage.Equals(""))
            {
                SchoolwiseStandardFeeConfigurationDetailsBL moSchoolwiseStandardFeeConfigurationDetailsBL = new SchoolwiseStandardFeeConfigurationDetailsBL();
                SchoolwiseStandardFeeConfigurationMasterBL oSchoolwiseStandardFeeConfigurationMasterBL = new SchoolwiseStandardFeeConfigurationMasterBL();
                IEnumerator oIEnum = aoFeeSubTypes.GetEnumerator();
                ArrayList oArrayList = new ArrayList();

                if (ConfigurationAction.Equals(Constants.Action.Insert))
                {
                    oArrayList.Add(InsertSchoolwiseStandardFeeConfigurationMaster());
                    oArrayList.Add(GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY));
                }
                else if (ConfigurationAction.Equals(Constants.Action.Update))
                {
                    //int iStandardFeeConfigId = Convert.ToInt32(oSchoolwiseStandardFeeConfigurationMasterBL.Schoolwise_Standard_Fee_Configuration_Id.ToString());
                    oArrayList.Add(UpdateSchoolwiseStandardFeeConfigurationMaster());
                    oArrayList.Add(SchoolwiseStandardFeeConfigurationDetailsCollectionBL.GetPhysicalDeleteStatement(moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId));



                    #region " To be Deleted "
                    //while (oIEnum.MoveNext())
                    //{
                    //    SchoolwiseStandardFeeConfigurationDetailsBL oSchoolwiseStandardFeeConfigurationDetailsBL = (SchoolwiseStandardFeeConfigurationDetailsBL)oIEnum.Current;
                    //    int iFeeSubTypeId = Convert.ToInt32(oSchoolwiseStandardFeeConfigurationDetailsBL.Fee_SubType_Id.ToString());

                    //    switch (oSchoolwiseStandardFeeConfigurationDetailsBL.ConfigurationAction)
                    //    {
                    //        case Constants.Action.Insert:
                    //            oArrayList.Add(oSchoolwiseStandardFeeConfigurationDetailsBL.InsertWhileInEdit());
                    //            break;
                    //        case Constants.Action.Delete:
                    //            oArrayList.Add(oSchoolwiseStandardFeeConfigurationDetailsBL.DeleteSchoolwiseStandardFeeConfigurationDetails(iFeeSubTypeId));
                    //            break;
                    //        case Constants.Action.Update:
                    //            oArrayList.Add(oSchoolwiseStandardFeeConfigurationDetailsBL.UpdateSchoolwiseStandardFeeConfigurationDetails());
                    //            break;
                    //    }

                    //}
                    #endregion " To be Deleted "
                }

                while (oIEnum.MoveNext())
                {
                    SchoolwiseStandardFeeConfigurationDetailsBL oSchoolwiseStandardFeeConfigurationDetailsBL = (SchoolwiseStandardFeeConfigurationDetailsBL)oIEnum.Current;
                    int iFeeSubTypeId = Convert.ToInt32(oSchoolwiseStandardFeeConfigurationDetailsBL.Fee_SubType_Id.ToString());
                    //int iStandardFeeConfigId = Convert.ToString(oSchoolwiseStandardFeeConfigurationDetailsBL.Schoolwise_Standard_Fee_Configuration_Id.ToString());
                    switch (oSchoolwiseStandardFeeConfigurationDetailsBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayList.Add(oSchoolwiseStandardFeeConfigurationDetailsBL.InsertSchoolwiseStandardFeeConfigurationDetails());
                            break;

                    }
                }
                moSchoolwiseStandardFeeConfigurationMasterDC.UpdateFeeSubTypeRecords(oArrayList);
                if (ConfigurationAction.Equals(Constants.Action.Update))
                {
                    if (!moSchoolwiseStandardFeeConfigurationMasterStruct.mbIsStudentPayFee)
                        StudentFeeDetailsCollectionBL.UpdateDebitEntries(moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId, moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId, moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId, moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId, Convert.ToInt32(moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld), false);
                    else
                        StudentFeeDetailsCollectionBL.UpdateDebitEntries
                            (moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId, moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId, moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId, moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId, Convert.ToInt32(moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld), true,
                            moSchoolwiseStandardFeeConfigurationMasterStruct.mdDueDate, moSchoolwiseStandardFeeConfigurationMasterStruct.miAmountForNewStudent,
                            moSchoolwiseStandardFeeConfigurationMasterStruct.miAmountForOldStudent);
                }
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sReferenceMessage);
            }
        }

        /// <summary>
        /// This method is used to get total fee amount.
        /// </summary>
        /// <param name="aiFee_Type_Id"></param>
        /// <param name="aiSchoolwiseFeeTypeConfigurationId"></param>
        //public DataSet GetTotalFee(int aiFeeTypeId, int aiFeeTypeConfigurationId)
        //{
        //    return moSchoolwiseStandardFeeConfigurationMasterDC.GetTotalFee(aiFeeTypeId, aiFeeTypeConfigurationId);
        //}

        #endregion
    }

    public class SchoolwiseStandardFeeConfigurationMasterCollectionBL
    {
        #region Data Members

        SchoolwiseStandardFeeConfigurationMasterCollectionDC moSchoolwiseStandardFeeConfigurationMasterCollectionDC;

        #endregion

        #region Constructors

        public SchoolwiseStandardFeeConfigurationMasterCollectionBL()
        {
            moSchoolwiseStandardFeeConfigurationMasterCollectionDC = new SchoolwiseStandardFeeConfigurationMasterCollectionDC();
        }

        public SchoolwiseStandardFeeConfigurationMasterCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moSchoolwiseStandardFeeConfigurationMasterCollectionDC = new SchoolwiseStandardFeeConfigurationMasterCollectionDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get all configured fee types.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns>DataSet</returns>
        public Int32 GetConfiguredFeeTypes(int aiStandardId)
        {
            return moSchoolwiseStandardFeeConfigurationMasterCollectionDC.GetConfiguredFeeTypes(aiStandardId);
        }

        /// <summary>
        /// This method is used to get records from Schoolwise_Standard_Fee_Configuration_Master.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetConfiguredStandardFee(int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolwiseStandardFeeConfigurationMasterCollectionDC.GetConfiguredStandardFee(aiSchoolId, aiAcademicYearId);
        }

        public DataSet GetStdFeeConfigurationDetails()
        {
            return moSchoolwiseStandardFeeConfigurationMasterCollectionDC.GetStdFeeConfigurationDetails();
        }
        #endregion
    }
}
