using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using System.Collections;
using Utility;

namespace BusinessLogic
{
    public class POUserDetailsBL
    {
        POUserDetailsDC moPOUserDetailsDC;
        int miTotalRows;

        public POUserDetailsBL()
        {
            moPOUserDetailsDC = new POUserDetailsDC();
        }

        public POUserDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moPOUserDetailsDC = new POUserDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }


        public void Save(POUserDetails aoPOUserDetails)
        {
            moPOUserDetailsDC.Save(aoPOUserDetails);
        }


        public POUserDetails Get(int Id)
        {
            return moPOUserDetailsDC.Get(Id);
        }


        public void Delete(int aiId)
        {
            bool bIsDependent = moPOUserDetailsDC.CheckDependencies(aiId);

            if (bIsDependent == false)
                moPOUserDetailsDC.Delete(aiId);
            else
                throw new Exceptions.ReferenceExceptions("You cannot delete this receiver. It is associated with External PO Details.");
        }


        public List<POUserDetails> GetAll(int aiSchoolId, string asFilter, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            int iEndIndex = startRowIndex + maximumRows;

            if (asFilter == null)
                asFilter = string.Empty;

            if (sortExpression == null || sortExpression == "")
                sortExpression = "Name ASC";

            List<POUserDetails> lstPOUserDetails = moPOUserDetailsDC.GetAll(aiSchoolId, asFilter, sortExpression, startRowIndex, iEndIndex);

            if (lstPOUserDetails.Count > 0)
                miTotalRows = lstPOUserDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstPOUserDetails;
        }

        public int GetCount(int aiSchoolId, string asFilter, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            return miTotalRows;
        }


        public bool Validate(int aiTypeId, string asValue, int aiId)
        {
            return moPOUserDetailsDC.Validate(aiTypeId, asValue, aiId);
        }
    }
}
