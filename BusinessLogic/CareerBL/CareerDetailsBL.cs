// Class Name       :- CareerDetailsBL
// Purpose          :- This class is used to manage Careers details.
// Date Of creation :- 1 December 2012
// Author Name      :- 


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using CareerEntities;
namespace BusinessLogic
{
    public class CareerDetailsBL
    {

        #region Data Members

        private CareerDetailsDC moCareerDetailsDC;
        
        #endregion

        #region Constructors

        public CareerDetailsBL()
        {
            moCareerDetailsDC = new CareerDetailsDC();
        }

        public CareerDetailsBL(int aiCareerDetailsID)
        {
            moCareerDetailsDC = new CareerDetailsDC(aiCareerDetailsID);
        }

        #endregion

        #region Properties
       
        /// <Summary>
        ///The developer has to use this property to get/set the data members of entity object from UI layer
        ///</Summary>
        ///<returns></returns>
        public  CareerDetailsInfo CareerDetails
        {
            get
            {
                return moCareerDetailsDC.CareerDetails;
            }
            set
            {
                moCareerDetailsDC.CareerDetails = value;
            }
        }

        #endregion

        #region Public Methods

        /// <Summary>
        ///This function is used to insert the Career Details
        ///</Summary>
        ///<returns></returns>
        public void Save()
        {
            moCareerDetailsDC.Save();
        }

        /// <Summary>
        ///This Methos is used to get the All the Employee Details from the CareerDetails table
        ///</Summary>
        ///<returns></returns>
        public static List<CareerDetailsInfo> GetAll()
        {
            return CareerDetailsDC.GetAll();
        }

        /// <Summary>
        ///This Methos is used to get the All the Employee Details from the CareerDetails table depending upon the search criteria
        ///</Summary>
        ///<returns></returns>
        public static List<CareerDetailsInfo> GetEmployeeCareerDetails(string asName, string asExperience, string asPost)
        {
            return CareerDetailsDC.GetEmployeeCareerDetails(asName, asExperience, asPost);
        }

        #endregion
    }
}
