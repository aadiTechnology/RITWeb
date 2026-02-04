// Class Name       :- ApprovalLevelConfigurationBL
// Purpose          :- This class is used to manage ApprovalLevelConfiguration details.
// Date Of creation :- 6/20/2009
// Author Name      :- Shankar


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;




namespace BusinessLogic
{


    public class ApprovalLevelConfigurationBL
    {

        private ApprovalLevelConfigurationDC.ApprovalLevelConfigurationStruct moApprovalLevelConfigurationStruct;

        private ApprovalLevelConfigurationDC moApprovalLevelConfigurationDC;

        public ApprovalLevelConfigurationBL()
        {
            moApprovalLevelConfigurationDC = new ApprovalLevelConfigurationDC();
        }

        public ApprovalLevelConfigurationBL(int miApprovalLevelConfigurationID)
        {
            moApprovalLevelConfigurationDC = new ApprovalLevelConfigurationDC(miApprovalLevelConfigurationID);
            moApprovalLevelConfigurationStruct = moApprovalLevelConfigurationDC.ApprovalLevelConfigurationStructDetails;
        }

        public virtual int ApprovalLevelConfigurationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miApprovalLevelConfigurationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miApprovalLevelConfigurationID = value;
            }
        }

        public virtual int RequisitionByDesignationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miRequisitionByDesignationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miRequisitionByDesignationID = value;
            }
        }

        public virtual int FirstDesignationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miFirstDesignationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miFirstDesignationID = value;
            }
        }

        public virtual int SecondDesignationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miSecondDesignationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miSecondDesignationID = value;
            }
        }

        public virtual int ThirdDesignationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miThirdDesignationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miThirdDesignationID = value;
            }
        }

        public virtual int FourthDesignationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miFourthDesignationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miFourthDesignationID = value;
            }
        }

        public virtual int fifthDesignationID
        {
            get
            {
                return moApprovalLevelConfigurationStruct.mififthDesignationID;
            }
            set
            {
                moApprovalLevelConfigurationStruct.mififthDesignationID = value;
            }
        }

        public virtual int School_Id
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miSchoolId;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miSchoolId = value;
            }
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moApprovalLevelConfigurationStruct.mdtInsertDate;
            }
            set
            {
                moApprovalLevelConfigurationStruct.mdtInsertDate = value;
            }
        }

        public virtual int Inserted_By_Id
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miInsertedById;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miInsertedById = value;
            }
        }

        public virtual System.DateTime Update_Date
        {
            get
            {
                return moApprovalLevelConfigurationStruct.mdtUpdateDate;
            }
            set
            {
                moApprovalLevelConfigurationStruct.mdtUpdateDate = value;
            }
        }

        public virtual int Updated_By_Id
        {
            get
            {
                return moApprovalLevelConfigurationStruct.miUpdatedById;
            }
            set
            {
                moApprovalLevelConfigurationStruct.miUpdatedById = value;
            }
        }

        public virtual bool Is_Deleted
        {
            get
            {
                return moApprovalLevelConfigurationStruct.mblnIsDeleted;
            }
            set
            {
                moApprovalLevelConfigurationStruct.mblnIsDeleted = value;
            }
        }

        public virtual int InsertApprovalLevelConfiguration()
        {
            moApprovalLevelConfigurationDC.ApprovalLevelConfigurationStructDetails = moApprovalLevelConfigurationStruct;
            return moApprovalLevelConfigurationDC.InsertApprovalLevelConfiguration();
        }

        public virtual void UpdateApprovalLevelConfiguration()
        {
            moApprovalLevelConfigurationDC.ApprovalLevelConfigurationStructDetails = moApprovalLevelConfigurationStruct;
            moApprovalLevelConfigurationDC.UpdateApprovalLevelConfiguration();
        }

        public virtual void DeleteApprovalLevelConfiguration(int aiApprovalLevelId)
        {
            int iUserId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Utility.Constants.S_SESSION_USER_ID]);
            moApprovalLevelConfigurationDC.DeleteApprovalLevelConfiguration(aiApprovalLevelId, iUserId);
        }

        public  void IsPendingApproval(int aiRequisitionByDesignationID)
        {
            moApprovalLevelConfigurationDC.ApprovalLevelConfigurationStructDetails = moApprovalLevelConfigurationStruct;
             moApprovalLevelConfigurationDC.IsPendingApproval(aiRequisitionByDesignationID);
        }
    }

    public class ApprovalLevelConfigurationCollectionBL
    {

        // This function is used to Fetch the ApprovalLevelConfiguration Details
        public static DataTable FetchApprovalLevelConfigurationDetails(int iSchoolId)
        {
            return ApprovalLevelConfigurationCollectionDC.FetchApprovalLevelConfigurationDetails(iSchoolId);
        }

        public static void UpdateFinalApproverDesignation(string sFinalApproversXML)
        {
            ApprovalLevelConfigurationCollectionDC.UpdateFinalApproverDesignation(sFinalApproversXML);
        }
    }
}
