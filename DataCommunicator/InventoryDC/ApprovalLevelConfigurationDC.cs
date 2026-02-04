// Class Name       :- ApprovalLevelConfigurationDC
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
using Utility;

namespace DataCommunicator
{
    public class ApprovalLevelConfigurationDC
    {

        private ApprovalLevelConfigurationStruct moApprovalLevelConfigurationStruct;

        public ApprovalLevelConfigurationDC()
        {
        }

        public ApprovalLevelConfigurationDC(int miApprovalLevelConfigurationID)
        {
            LoadApprovalLevelConfigurationDetails(miApprovalLevelConfigurationID);
        }

        public virtual ApprovalLevelConfigurationStruct ApprovalLevelConfigurationStructDetails
        {
            get
            {
                return moApprovalLevelConfigurationStruct;
            }
            set
            {
                moApprovalLevelConfigurationStruct = value;
            }
        }

        // This function is used to insert the ApprovalLevelConfiguration Details
        public virtual int InsertApprovalLevelConfiguration()
        {
            string sInsertStatement = "INSERT INTO ApprovalLevelConfiguration(" +
            "RequisitionByDesignationID" +
            ",FirstDesignationID" +
            ",SecondDesignationID" +
            ",ThirdDesignationID" +
            ",FourthDesignationID" +
            ",fifthDesignationID" +
            ",School_Id" +
            ",Insert_Date" +
            ",Inserted_By_Id" +
            ",Is_Deleted" +
            ")VALUES(" +
            " " + moApprovalLevelConfigurationStruct.miRequisitionByDesignationID +
             " , " + moApprovalLevelConfigurationStruct.miFirstDesignationID +
             " , " + moApprovalLevelConfigurationStruct.miSecondDesignationID +
             " , " + moApprovalLevelConfigurationStruct.miThirdDesignationID +
             " , " + moApprovalLevelConfigurationStruct.miFourthDesignationID +
             " , " + moApprovalLevelConfigurationStruct.mififthDesignationID +
             " , " + moApprovalLevelConfigurationStruct.miSchoolId +
             " , N'" + moApprovalLevelConfigurationStruct.mdtInsertDate + "' " +
             " , " + moApprovalLevelConfigurationStruct.miInsertedById +
             " , N'" + moApprovalLevelConfigurationStruct.mblnIsDeleted + "'" +
            ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        // This function is used to update the ApprovalLevelConfiguration Details
        public virtual void UpdateApprovalLevelConfiguration()
        {
            string sUpdateStatement = "UPDATE ApprovalLevelConfiguration SET " +
            "RequisitionByDesignationID= " + moApprovalLevelConfigurationStruct.miRequisitionByDesignationID +
            ",FirstDesignationID= " + moApprovalLevelConfigurationStruct.miFirstDesignationID +
            ",SecondDesignationID= " + moApprovalLevelConfigurationStruct.miSecondDesignationID +
            ",ThirdDesignationID= " + moApprovalLevelConfigurationStruct.miThirdDesignationID +
            ",FourthDesignationID= " + moApprovalLevelConfigurationStruct.miFourthDesignationID +
            ",fifthDesignationID= " + moApprovalLevelConfigurationStruct.mififthDesignationID +
            ",School_Id= " + moApprovalLevelConfigurationStruct.miSchoolId +
            ",Insert_Date= '" + moApprovalLevelConfigurationStruct.mdtInsertDate + "' " +
            ",Inserted_By_Id= " + moApprovalLevelConfigurationStruct.miInsertedById +
            ",Update_Date= N'" + moApprovalLevelConfigurationStruct.mdtUpdateDate + "' " +
            ",Updated_By_Id= " + moApprovalLevelConfigurationStruct.miUpdatedById +
            ",Is_Deleted= N'" + moApprovalLevelConfigurationStruct.mblnIsDeleted + "'" +
            "" +
            " WHERE ApprovalLevelConfigurationID=" + moApprovalLevelConfigurationStruct.miApprovalLevelConfigurationID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to delete the ApprovalLevelConfiguration Details
        public virtual void DeleteApprovalLevelConfiguration(int aiApprovalLevelId, int aiUpdatedById)
        {
            string sUpdateStatement = "UPDATE ApprovalLevelConfiguration SET " +
             "  Update_Date= dbo.GetLocalDate(DEFAULT) " +
             " , Updated_By_Id= " + aiUpdatedById +
             " , Is_Deleted= 1" +
             " WHERE ApprovalLevelConfigurationID=" + aiApprovalLevelId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to load the ApprovalLevelConfiguration Details
        private void LoadApprovalLevelConfigurationDetails(int miApprovalLevelConfigurationID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchApprovalLevelConfigurationDetailsFromDatabase(miApprovalLevelConfigurationID);
               using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
               {
                   if (oDR != null)
                   {
                       while (oDR.Read())
                       {
                           if (oDR["ApprovalLevelConfigurationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miApprovalLevelConfigurationID = Convert.ToInt32(oDR["ApprovalLevelConfigurationID"]);
                           if (oDR["RequisitionByDesignationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miRequisitionByDesignationID = Convert.ToInt32(oDR["RequisitionByDesignationID"]);
                           if (oDR["FirstDesignationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miFirstDesignationID = Convert.ToInt32(oDR["FirstDesignationID"]);
                           if (oDR["SecondDesignationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miSecondDesignationID = Convert.ToInt32(oDR["SecondDesignationID"]);
                           if (oDR["ThirdDesignationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miThirdDesignationID = Convert.ToInt32(oDR["ThirdDesignationID"]);
                           if (oDR["FourthDesignationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miFourthDesignationID = Convert.ToInt32(oDR["FourthDesignationID"]);
                           if (oDR["fifthDesignationID"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.mififthDesignationID = Convert.ToInt32(oDR["fifthDesignationID"]);
                           if (oDR["School_Id"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                           if (oDR["Insert_Date"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                           if (oDR["Inserted_By_Id"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"]);
                           if (oDR["Update_Date"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                           if (oDR["Updated_By_Id"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                           if (oDR["Is_Deleted"] != DBNull.Value)
                               moApprovalLevelConfigurationStruct.mblnIsDeleted = Convert.ToBoolean(oDR["Is_Deleted"]);
                       }
                   }
                }
            }
        }

        // This function is used to fetch the ApprovalLevelConfiguration Details
        private string  FetchApprovalLevelConfigurationDetailsFromDatabase(int miApprovalLevelConfigurationID)
        {
            string sSelectStatement = " SELECT  " +
            "ApprovalLevelConfigurationID" +
            ",RequisitionByDesignationID" +
            ",FirstDesignationID" +
            ",SecondDesignationID" +
            ",ThirdDesignationID" +
            ",FourthDesignationID" +
            ",fifthDesignationID" +
            ",School_Id" +
            ",Insert_Date" +
            ",Inserted_By_Id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            ",Is_Deleted" +
            " FROM ApprovalLevelConfiguration" +
            " WHERE ApprovalLevelConfigurationID=" + miApprovalLevelConfigurationID;
            return sSelectStatement;
        }

        public struct ApprovalLevelConfigurationStruct
        {

            public int miApprovalLevelConfigurationID;

            public int miRequisitionByDesignationID;

            public int miFirstDesignationID;

            public int miSecondDesignationID;

            public int miThirdDesignationID;

            public int miFourthDesignationID;

            public int mififthDesignationID;

            public int miSchoolId;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public bool mblnIsDeleted;
        }

        public  void IsPendingApproval(int aiRequisitionByDesignationID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moApprovalLevelConfigurationStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RequisitionByDesignationID", aiRequisitionByDesignationID, SqlDbType.Int);
                //SqlParameter oSSqlParameter= oSQLServerDbUtility.AddParameter("PendingRequisition", 0, SqlDbType.Bit,ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsPendingRequision");
              //  return Convert.ToBoolean(oSSqlParameter.Value);
            }
        }
    }

    public class ApprovalLevelConfigurationCollectionDC
    {

        /// <summary>
        ///  This function is used to Fetch all ApprovalLevelConfiguration Details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable FetchApprovalLevelConfigurationDetails(int aiSchoolId)
        {   
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchool_Id", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetApprovalLevelConfiguration");
            }
        }

        public static void UpdateFinalApproverDesignation(string asFinalApproversXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("sFinalApproversXML", asFinalApproversXML, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateFinalApproverDesignation");
            }   
        }
    }
}
