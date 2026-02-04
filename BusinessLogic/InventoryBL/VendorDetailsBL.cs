// Class Name       :- VendorDetailsDC
// Purpose          :- This class is used to Add vendors configurations.
// Date Of creation :- 12/01/2018
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;
using System.Data;

namespace BusinessLogic
{
    public class VendorDetailsBL
    {
        #region Data members

        private VendorDetailsDC moVendorDetailsDC;
        int miCount = Constants.I_ZERO;

        #endregion

        #region Constructors

        public VendorDetailsBL()
        {
            this.moVendorDetailsDC = new VendorDetailsDC();
        }        

        public VendorDetailsBL(int aiSchoolId,int aiUpdatedById)
        {
            this.moVendorDetailsDC = new VendorDetailsDC(aiSchoolId, aiUpdatedById);
        }

        #endregion

        #region Public Methods

        public void Save(VendorDetails oVendorDetails)
        {
            this.moVendorDetailsDC.Save(oVendorDetails);
        }

        public List<VendorDetails> GetAll(int aiSchoolId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            List<VendorDetails> lstVendorDetails = new List<VendorDetails>();
            if (asSortDirection == "" || asSortDirection == null)
                asSortDirection = Constants.S_DESCENDING;

            asSortExpression = asSortExpression + " " + asSortDirection;

            int iEndIndex = startRowIndex + maximumRows;
            lstVendorDetails = this.moVendorDetailsDC.GetAll(aiSchoolId, asSortExpression, startRowIndex, iEndIndex);

            if (lstVendorDetails.Count > Constants.I_ZERO)
                miCount = lstVendorDetails[Constants.I_ZERO].TotalRows.ToInt();

            return lstVendorDetails;
        }

        public DataSet GetAllVendorsForCombo()
        {           
            return moVendorDetailsDC.GetAllVendorsForCombo();
        }

        /// <summary>
        /// This method is used to get count of OD Details
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        public int Count(int aiSchoolId, string asSortExpression, string asSortDirection)
        {
            return miCount;
        }

        public VendorDetails Get(int aiVendorId)
        {
            return this.moVendorDetailsDC.Get(aiVendorId);
        }

        public void Delete(int aiVendorId)
        {
            this.moVendorDetailsDC.Delete(aiVendorId);
        }
        #endregion
    }
}
