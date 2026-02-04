/*
 * File Name - UserPaymentCategoryAssoBL.cs
 * Created Date - 4 April 2014
 * Created By - Sachin
 * Description - This class is used to manage association of user and payment category.
 */
using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class UserPaymentCategoryAssoBL
    {
        #region Data Member(S)
        
        private UserPaymentCategoryAssoDC moUserPaymentCategoryAssoDC; 

        #endregion

        #region Constructor(s)
        
        public UserPaymentCategoryAssoBL()
        {
            this.moUserPaymentCategoryAssoDC = new UserPaymentCategoryAssoDC();
        }

        public UserPaymentCategoryAssoBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moUserPaymentCategoryAssoDC = new UserPaymentCategoryAssoDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return all users for association.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<UserPaymentCategoryAssociation> GetAll(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int maximumRows, int startRowIndex)
        {   
            int iEndIndex = startRowIndex + maximumRows;
            return this.moUserPaymentCategoryAssoDC.GetAll(aiSchoolId, aiAcademicYearId, aiStaffGroupId, startRowIndex, iEndIndex);
        }

        /// <summary>
        /// This method is sued to return count of records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int maximumRows, int startRowIndex)
        {
            return this.moUserPaymentCategoryAssoDC.Count(aiSchoolId, aiAcademicYearId, aiStaffGroupId);
        }

        /// <summary>
        /// This method is used to save association.
        /// </summary>
        /// <param name="asXml"></param>
        public void Save(string asXml)
        {
            this.moUserPaymentCategoryAssoDC.Save(asXml);
        } 

        #endregion   
    }
}
