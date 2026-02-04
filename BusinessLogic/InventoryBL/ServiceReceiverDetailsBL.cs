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
    public class ServiceReceiverDetailsBL
    {
        ServiceReceiverDetailsDC moServiceReceiverDetailsDC;
        int miTotalRows;

        public ServiceReceiverDetailsBL()
        {
            moServiceReceiverDetailsDC = new ServiceReceiverDetailsDC();
        }

        public ServiceReceiverDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moServiceReceiverDetailsDC = new ServiceReceiverDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }


        public void Save(ServiceReceiverDetails aoServiceReceiverDetails)
        {
            moServiceReceiverDetailsDC.Save(aoServiceReceiverDetails);
        }


        public ServiceReceiverDetails Get(int Id)
        {
            return moServiceReceiverDetailsDC.Get(Id);
        }


        public void Delete(int aiId)
        {
            bool bIsDependent = moServiceReceiverDetailsDC.CheckDependencies(aiId);

            if (bIsDependent == false)
                moServiceReceiverDetailsDC.Delete(aiId);
            else
                throw new Exceptions.ReferenceExceptions("You cannot delete this receiver. It is associated with GST Invoice Details.");
        }
       

        public List<ServiceReceiverDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asFilter, string sortExpression, string sortDirection,int startRowIndex, int maximumRows)
        {
            int iEndIndex = startRowIndex + maximumRows;

            if (asFilter == null)
                asFilter = string.Empty;

            if (sortExpression == null || sortExpression == "")
                sortExpression = "Name ASC";

            List<ServiceReceiverDetails> lstServiceReceiverDetails = moServiceReceiverDetailsDC.GetAll(aiSchoolId, aiAcademicYearId, asFilter, sortExpression, startRowIndex, iEndIndex);

            if (lstServiceReceiverDetails.Count > 0)
                miTotalRows = lstServiceReceiverDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstServiceReceiverDetails;
        }

        public int GetCount(int aiSchoolId, int aiAcademicYearId, string asFilter, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            return miTotalRows;
        }


        public bool Validate(int aiTypeId, string asValue, int aiId)
        {
            return moServiceReceiverDetailsDC.Validate(aiTypeId, asValue, aiId);
        }
    }
}
