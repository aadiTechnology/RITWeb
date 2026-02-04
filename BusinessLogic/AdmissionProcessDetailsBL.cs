// Class Name       :- AdmissionProcessDetailsBL
// Purpose          :- This class is used to manage Admission Process details.
// Date Of creation :- 10/10/2015
// Author Name      :- 

    using System;
    using DataCommunicator;
    using SchoolEntities;
    using System.Collections.Generic;


namespace BusinessLogic
{
    public class AdmissionProcessDetailsBL
    {
        #region Data members

        private AdmissionProcessDetailsDC moAdmissionProcessDetailsDC;

        #endregion

        #region Constructors

        public AdmissionProcessDetailsBL()
        {
            this.moAdmissionProcessDetailsDC = new AdmissionProcessDetailsDC();
        }

        public AdmissionProcessDetailsBL(int aiSchoolId, int aiAcademicYearId)
        {
            this.moAdmissionProcessDetailsDC = new AdmissionProcessDetailsDC(aiSchoolId, aiAcademicYearId);
        }

        public AdmissionProcessDetailsBL(int aiSchoolId,int aiUpdatedById, int aiAcademicYearId)
        {
            this.moAdmissionProcessDetailsDC = new AdmissionProcessDetailsDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get All Admission Process Details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>

        public List<AdmissionProcessDetails> GetAll()
        {
            return this.moAdmissionProcessDetailsDC.GetAll();
        }

        /// <summary>
        /// This method is used to get Admission Process details
        /// </summary>
        /// <param name="aiAdmissionProcessId"></param>

        public AdmissionProcessDetails Get(int aiAdmissionProcessId)
        {
            return this.moAdmissionProcessDetailsDC.Get(aiAdmissionProcessId);
        }

        /// <summary>
        /// This method is used to Save Admission Process details
        /// </summary>
        /// <param name="oAdmissionProcessDetails"></param>

        public void Save(AdmissionProcessDetails aoAdmissionProcessDetails)
        {
            this.moAdmissionProcessDetailsDC.Save(aoAdmissionProcessDetails);
        }

        /// <summary>
        /// This method is used to Save And Update Student Location details
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiLocationName"></param>
        /// <param name="aiUserId"></param>
        ///  <param name="aiSchoolId"></param>
        public void SaveStudentLocation(int aiId, string asLocationName)
        {
            moAdmissionProcessDetailsDC.SaveStudentLocation(aiId, asLocationName);
        }

        /// <summary>
        /// This method is used to Get All Student Location details
        /// </summary>
        ///  <param name="miSchoolId"></param>
        public List<StudentLivingLocation> GetAllLivingLocation(int aiSchoolId)
        {
            List<StudentLivingLocation> lstLivingLocattion = moAdmissionProcessDetailsDC.GetAllLivingLocation();
            return lstLivingLocattion;
        }

        /// <summary>
        /// This method is used to delete Location.
        /// </summary>
        /// <param name="aiLocationId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiSchoolId"></param>
        public void DeleteLocation(int aiLocationId)
        {
            moAdmissionProcessDetailsDC.DeleteLocation(aiLocationId);                    
        }

        /// <summary>
        /// This method is used to Delete Admission Process details
        /// </summary>
        /// <param name="aiId"></param>

        public void Delete(int aiId)
        {
           this. moAdmissionProcessDetailsDC.Delete(aiId);
        }

        /// <summary>
        /// This method is used to check Selected Standerd is Already Exist or Not
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="StanderdId"></param>

        public bool IsConfigurationAlreadyExist(int aiId, int aiStanderdId)
        {
            return this.moAdmissionProcessDetailsDC.IsConfigurationAlreadyExist(aiId, aiStanderdId);
        }

        /// <summary>
        /// This method is used to get Internal Link Standards details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<InternalLinkStandardDetails> GetInternalLinkStandards(int aiSchoolId, int aiAcademicYearId)
        {
            return this.moAdmissionProcessDetailsDC.GetInternalLinkStandards(aiSchoolId, aiAcademicYearId);
        }

        #endregion
    }
}
