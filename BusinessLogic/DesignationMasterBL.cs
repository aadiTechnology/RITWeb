using System;
using System.Collections.Generic;
using DataCommunicator;
using MasterEntities;

namespace BusinessLogic
{   
    public class DesignationMasterBL
    {
        #region " Constants "

      
        #endregion " Constants "

        #region " Data Members "

        private DesignationMasterDC moDesignationMasterDC;

        #endregion " Data Members "

        #region " Constructors "

        public DesignationMasterBL()
        {
            moDesignationMasterDC = new DesignationMasterDC();
        }

        public DesignationMasterBL(int miTeacherDesignationId)
        {
            moDesignationMasterDC = new DesignationMasterDC(miTeacherDesignationId);
        }       

        public DesignationMasterBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moDesignationMasterDC = new DesignationMasterDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion " Constructors "

        #region Property

        public DesignationMaster DesignationMaster
        {
            get { return moDesignationMasterDC.moDesignationMaster; }
            set { moDesignationMasterDC.moDesignationMaster = value; }
        }

        #endregion

        #region " PUBLIC METHODS "

        /// <summary>
        /// This function is used to get single  Designation record  details . 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>

        public DesignationMaster Get(int aiDesignationId, bool abIsPTADesignation)
        {
            return this.moDesignationMasterDC.Get(aiDesignationId, abIsPTADesignation);
        }

        /// <summary>
        /// This function is used to get all Designation Name details and bind to object data source. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="abIsPTADesignation"></param>
        /// <returns></returns>

        public List<DesignationMaster> GetAll(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, bool abIsPTADesignation)
        {
            int aiStartIndex = startRowIndex;
            int aiEndIndex = aiStartIndex + maximumRows;
            return moDesignationMasterDC.GetAll(sortExpression, aiEndIndex, startRowIndex, abIsPTADesignation);
        }       

        public List<DesignationMaster> GetAll()
        {
            return moDesignationMasterDC.GetAll();
        }

        /// <summary>
        /// This method is used to get total count of designation details records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>

        public int Count(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, bool abIsPTADesignation)
        {
            return moDesignationMasterDC.Count(aiSchoolId, aiAcademicYearId, abIsPTADesignation);
        }

        /// <summary>
        /// To Insert Designation Information.
        /// </summary>
        /// <param name="oDesignationMaster"></param>

        public void Insert(DesignationMaster aoDesignationMaster, bool abIsPTADesignation)
        {
            this.moDesignationMasterDC.Insert(aoDesignationMaster, abIsPTADesignation);
        }

        /// <summary>
        /// To Update Designation Information.
        /// </summary>
        /// <param name="oDesignationMaster"></param>

        public void Update(DesignationMaster aoDesignationMaster, bool abIsPTADesignation)
        {
            this.moDesignationMasterDC.Update(aoDesignationMaster, abIsPTADesignation);
        }

        /// <summary>
        /// To Method is used to delete Designation.
        /// </summary>
        /// <param name="aiDesignationID"></param>
        /// <returns></returns>  

        public int Delete(int aiDesignationID, int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abIsPTADesignation)
        {
            return moDesignationMasterDC.Delete(aiDesignationID, aiSchoolId, aiAcademicYearId, aiUserId, abIsPTADesignation);
        }

        /// <summary>
        /// TO get Account Designations
        /// </summary>
        /// <returns></returns>
        
        public List<int> GetAccountDesignations()
        {
            return moDesignationMasterDC.GetAccountDesignations();
        }

        #endregion
    }

}


