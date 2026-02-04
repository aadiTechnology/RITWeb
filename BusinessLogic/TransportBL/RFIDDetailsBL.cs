using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities.Transport;
using Utility;
using DataCommunicator.TransportDC;

namespace BusinessLogic.TransportBL
{
    public class RFIDDetailsBL
    {
        #region Data Member(s)

        private RFIDDetailsDC moRFIDDetailsDC = null;
        private int miTotalRows;

        #endregion

        #region Constructor(s)

        public RFIDDetailsBL()
        {
            moRFIDDetailsDC = new RFIDDetailsDC();
        }

        public RFIDDetailsBL(int aiSchoolId, int aiUserId)
        {
            moRFIDDetailsDC = new RFIDDetailsDC(aiSchoolId, aiUserId);
        }

        #endregion
        #region Methods

        /// <summary>
        /// This method is used to save new RFID.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiRFID"></param>
        public void Save(int aiStudentId, string asRFID)
        {
            moRFIDDetailsDC.Save(aiStudentId, asRFID);   
        }

        /// <summary>
        /// This method is used to return all searched student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public List<RFIDDetails> GetAllStudents(int aiSchoolId, int aiAcademicYearId, string asFilter, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            int iEndIndex = StartRowIndex + MaximumRows;

            if (asFilter == null)
                asFilter = string.Empty;

            if (SortExpression == null || SortExpression == string.Empty)
                SortExpression = "Roll_No desc";

            List<RFIDDetails> lstSearchedStudent = moRFIDDetailsDC.GetAllStudents(aiSchoolId, aiAcademicYearId, asFilter, SortExpression, StartRowIndex, iEndIndex);

            if (lstSearchedStudent.Count > 0)
                miTotalRows = lstSearchedStudent[0].TotalRows;
            else
                miTotalRows = 0;

            return lstSearchedStudent;
        }

        /// <summary>
        ///  This method is used to return all searched student count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public int GetCountStudent(int aiSchoolId, int aiAcademicYearId, string asFilter, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {           
            return miTotalRows;
        }

        /// <summary>
        /// This method is sued to check RFID duplication.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="asRFID"></param>
        /// <returns></returns>
        public string ValidateRFID(int aiSchoolwiseStudentId, string asRFID)
        {
            return moRFIDDetailsDC.ValidateRFID(aiSchoolwiseStudentId, asRFID);
        }

        #endregion        
    }
}
