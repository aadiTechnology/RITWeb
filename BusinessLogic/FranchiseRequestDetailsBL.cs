// -----------------------------------------------------------------------
// <copyright file="FranchiseRequestDetailsBL.cs" company="">
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
using DataCommunicator;
using ProductDemoEntities;
namespace BusinessLogic
{
    public class FranchiseRequestDetailsBL
    {

        #region Data Members
        private FranchiseRequestDetailsDC moFranchiseRequestDetailsDC;
        #endregion

        #region Constructors

        public FranchiseRequestDetailsBL()
        {
            moFranchiseRequestDetailsDC = new FranchiseRequestDetailsDC();
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
                return moFranchiseRequestDetailsDC.FranchiseRequestDetails;
            }
            set
            {
                moFranchiseRequestDetailsDC.FranchiseRequestDetails = value;
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

            moFranchiseRequestDetailsDC.Add();
        }

        #endregion

    }
}
