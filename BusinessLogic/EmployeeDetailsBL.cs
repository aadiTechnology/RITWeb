using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;
using System.Data;
using SchoolEntities.Admin;

namespace BusinessLogic
{
    public class EmployeeDetailsBL
    {
        #region Data members

        private EmployeeDetailsDC moEmployeeDetailsDC;

        #endregion
        #region Constructors

        public EmployeeDetailsBL()
        {
            this.moEmployeeDetailsDC = new EmployeeDetailsDC();
        }


        public EmployeeDetailsBL(int aiSchoolId, int aiAcademicYearId)
        {
            moEmployeeDetailsDC = new EmployeeDetailsDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion


        /// <summary>
        /// This method is used to save data into database.
        /// </summary>
        /// <param name="oTeacherAdditionalDetails"></param>
        public void save(string asEmployeeOtherDetailsXML,string asEmployeeFamilyDetailsXML,  string asEmployeeStatutoryDetailsXML,string asemail,int aischoolid,int aiacademicyearid,int aiuserid)
        {
            moEmployeeDetailsDC.save(asEmployeeOtherDetailsXML, asEmployeeFamilyDetailsXML, asEmployeeStatutoryDetailsXML, asemail, aischoolid, aiacademicyearid, aiuserid);
        }

        /// <summary>
        /// This function is used to Get the User Basic details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public EmployeeDetails GetEmployeeBasicDetails(int aiUserId, int aiSchoolId)
        {
         return  moEmployeeDetailsDC.GetEmployeeBasicDetails(aiUserId, aiSchoolId);
        }
        public DataTable getAllBank(int aiSchoolId)
        {
            return moEmployeeDetailsDC.GetAllBank(aiSchoolId);
        
        }
    }
}
