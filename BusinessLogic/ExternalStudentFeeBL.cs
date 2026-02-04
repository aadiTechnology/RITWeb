// File Name : ExternalStudentFeeBL.cs
// Creator :  Sachin Wagh
// Created Date : 03-14-2018
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using Utility;

namespace BusinessLogic
{
    public class ExternalStudentFeeBL
    {
       #region Data Member(s)

       private ExternalStudentFeeDC moExternalStudentFeeDC;
       private int I_TOTAL_ROW_COUNT = Constants.I_ZERO;
        
       #endregion

       #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthParameterBL" /> class. 
        /// </summary>
        public ExternalStudentFeeBL()
        {
			this.moExternalStudentFeeDC = new ExternalStudentFeeDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeTaxSlabsBL" /> class. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public ExternalStudentFeeBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
           this.moExternalStudentFeeDC = new ExternalStudentFeeDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
          
        }

        #endregion    
   
       #region Method(s)
         
        /// <summary>
        /// This method is used to return all external Students details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="asFilter"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<ExternalStudentFee> GetAll(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection, string asFilter, int maximumRows, int startRowIndex)
        {            
            List<ExternalStudentFee> lstExternalStudentFee = new List<ExternalStudentFee>();
            if (string.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "PaymentDate";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }

            asSortExpression = asSortExpression + " " + asSortDirection;

            if (asFilter == null)
                asFilter = string.Empty;

            int iEndIndex = startRowIndex + maximumRows;
            lstExternalStudentFee = this.moExternalStudentFeeDC.GetAll(aiSchoolId, aiAcademicYearId, asSortExpression, startRowIndex, iEndIndex, asFilter);
            
            if(lstExternalStudentFee.Count > Constants.I_ZERO)
                I_TOTAL_ROW_COUNT = lstExternalStudentFee[Constants.I_ZERO].TotalRowCount.ToInt();
            else
                I_TOTAL_ROW_COUNT = Constants.I_ZERO;

            return lstExternalStudentFee;
        }

        /// <summary>
        /// This Method is used to return total row counts.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection, string asFilter)
        {
            return I_TOTAL_ROW_COUNT;
        }

        /// <summary>
        /// This method is used to retrive Health Parameter details for particular ID.
        /// </summary>
        /// <param name="aiIncomeTaxRangeId"></param>
        /// <returns></returns>
        public ExternalStudentFee Get(int aiExternalStudentId)
        {
            return this.moExternalStudentFeeDC.Get(aiExternalStudentId);
        }

        /// <summary>
        /// This method is used to insert/update Health Parameter details. 
        /// </summary>		
        public void Save(ExternalStudentFee aoExternalStudentFee)
        {
            this.moExternalStudentFeeDC.Save(aoExternalStudentFee);
        }

        /// <summary>
        /// This method is used to delete Health Parameter details.
        /// </summary>		
        public void Delete(int aiExternalStudentId)
        {
            this.moExternalStudentFeeDC.Delete(aiExternalStudentId);
         
        }

        /// <summary>
        /// This method is used to fill external students fee details combobox.
        /// </summary>
        /// <returns></returns>
        public DataTable GetExternalFeeTypesForCombo()
        {
            return this.moExternalStudentFeeDC.GetExternalFeeTypesForCombo();
        }

        /// <summary>
        /// This method is used to get external student details for display receipts.
        /// </summary>
        /// <param name="aiExternalStudentFeeId"></param>
        /// <param name="aiReceiptNo"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAccountHeaderId"></param>
        /// <returns></returns>
        public DataTable GetRecieptDetails(int aiExternalStudentFeeId, int aiReceiptNo, int aiAcademicYearId, int aiAccountHeaderId)
        {
            return this.moExternalStudentFeeDC.GetRecieptDetails(aiExternalStudentFeeId, aiReceiptNo, aiAcademicYearId, aiAccountHeaderId);
        }

        #endregion
    }
}
