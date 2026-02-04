// -----------------------------------------------------------------------
// <copyright file="FranchiseRequestDetailsDC.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using ProductDemoEntities;

namespace DataCommunicator
{

    public class FranchiseRequestDetailsDC
    {

        #region Data Members

        private FranchiseRequestDetails moFranchiseRequestDetailsInfo;

        #endregion

        #region Constructor

        public FranchiseRequestDetailsDC()
        {
            moFranchiseRequestDetailsInfo = new FranchiseRequestDetails();
        }

        #endregion

        #region Properties

        // <Summary>
        //The developer has to use this property to get/set the data members of entity object from UI layer
        //</Summary>
        //<returns></returns>
        public FranchiseRequestDetails FranchiseRequestDetails
        {
            get
            {
                return moFranchiseRequestDetailsInfo;
            }
            set
            {
                moFranchiseRequestDetailsInfo = value;
            }
        }
        
        #endregion

        #region Public Methods

        // <Summary>
        //This function is used to insert the franchise request Details into database.
        //</Summary>
        //<returns></returns>
        public void Add()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Name", moFranchiseRequestDetailsInfo.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Designation", moFranchiseRequestDetailsInfo.Designation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("NameOfFirm", moFranchiseRequestDetailsInfo.NameOfTheFirm, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Email", moFranchiseRequestDetailsInfo.Email, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Address", moFranchiseRequestDetailsInfo.Address, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Message", moFranchiseRequestDetailsInfo.Message, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNo", moFranchiseRequestDetailsInfo.MobileNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("WebSite", moFranchiseRequestDetailsInfo.WebSite, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[dbo].[usp_InsertFranchiseRequestDetails]");
            }
        }

        #endregion
    }
}
