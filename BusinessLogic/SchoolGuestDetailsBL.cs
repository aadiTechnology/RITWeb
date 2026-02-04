using System;
using DataCommunicator;
using SchoolEntities;
using System.Collections.Generic;
using SchoolEntities.Admin;


namespace BusinessLogic
{
    public class SchoolGuestDetailsBL
    {
        #region Data members

        public GuestDetailsDC moGuestDetailsDC;

        #endregion

        #region Constructor

        public SchoolGuestDetailsBL()
        {
            this.moGuestDetailsDC = new GuestDetailsDC();
        }

        public SchoolGuestDetailsBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moGuestDetailsDC = new GuestDetailsDC(aiSchoolId, aiUpdatedById);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used for Save the Guest Details in database.
        /// </summary>
        public void Save(SchoolGuestDetails moSchoolGuestDetails, out int aiGuestId)
        {
            this.moGuestDetailsDC.Save(moSchoolGuestDetails, out aiGuestId);
        }

        /// <summary>
        /// This method is used for get all the details of Guest for filling the list view..
        /// </summary>
        public List<SchoolGuestDetails> GetAll(int aiSchoolId, string asFilter, string asGuestType,int  aiCategoryType, int maximumRows, int startRowIndex)
        {
            if (asFilter == null)
                asFilter = string.Empty;
            int iEndIndex = startRowIndex + maximumRows;
            return moGuestDetailsDC.GetAll(aiSchoolId, asFilter, asGuestType, startRowIndex, iEndIndex, aiCategoryType);
        }

        /// <summary>
        /// This method is used for Getting the count of all Guests.
        /// </summary>
        public int GetCount(int aiSchoolId, string asFilter, string asGuestType, int aiCategoryType)
        {
            if (asFilter == null)
                asFilter = string.Empty;
            return moGuestDetailsDC.GetCount(aiSchoolId, asFilter, asGuestType, aiCategoryType);
        }

        /// <summary>
        /// This method is used for getting the designation of staff for filling the designation text box on screen.
        /// </summary>
        public void GetDesignationForGuestStaff(int aiSchoolId, string asStaffName, out string asDesignation)
        {
            moGuestDetailsDC.GetDesignationForGuestStaff(asStaffName, aiSchoolId, out asDesignation);
        }

        /// <summary>
        /// This method is used geting the guest details for edit mode.
        /// </summary>
        public SchoolGuestDetails Get(int aiGuestId)
        {
            return moGuestDetailsDC.Get(aiGuestId);
        }

        /// <summary>
        /// This method is used deleting the data for perticular guest.
        /// </summary>
        public void Delete(int aiId)
        {
            moGuestDetailsDC.Delete(aiId);
        }
        /// <summary>
        /// This method is used geting the Get Category Type details.
        /// </summary>
        /// <returns></returns>
        public List<SchoolGuestDetails> GetCategoryType()
        {
           return  moGuestDetailsDC.GetCategoryType();
         }
        #endregion
    }
}
