using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class UserAppointmentDetailsBL
    {
        #region Dat Member(s)
        
        UserAppointmentDetailsDC moUserAppointmentDetailsDC; 

        #endregion

        #region Constructor(s)
        
        public UserAppointmentDetailsBL()
        {
            this.moUserAppointmentDetailsDC = new UserAppointmentDetailsDC();
        }

        public UserAppointmentDetailsBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moUserAppointmentDetailsDC = new UserAppointmentDetailsDC(aiSchoolId, aiUpdatedById);
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all available appointmnet details according to given page index.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<UserAppointmentDetails> GetAll(int aiSchoolId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            int iEndIndex = startRowIndex + maximumRows;

            if (sortExpression == "")
                sortExpression = "Name";

            return this.moUserAppointmentDetailsDC.GetAll(aiSchoolId, sortExpression,  startRowIndex, iEndIndex);
        }

        /// <summary>
        /// This method is used to return appointmnet details count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            return this.moUserAppointmentDetailsDC.Count(aiSchoolId);
        }

        /// <summary>
        /// his method is used to return appointment details.
        /// </summary>
        /// <param name="aiAppointmentId"></param>
        /// <returns></returns>
        public UserAppointmentDetails Get(int aiAppointmentId)
        {
            return this.moUserAppointmentDetailsDC.Get(aiAppointmentId);
        }

        /// <summary>
        /// This method is used to save appointment details.
        /// </summary>
        /// <param name="aoUserAppointmentDetails"></param>
        public void Save(UserAppointmentDetails aoUserAppointmentDetails)
        {
            this.moUserAppointmentDetailsDC.Save(aoUserAppointmentDetails);
        }

        /// <summary>
        /// This method is used to delete appointment details.
        /// </summary>
        /// <param name="aiAppointmentId"></param>
        public void Delete(int aiAppointmentId)
        {
            this.moUserAppointmentDetailsDC.Delete(aiAppointmentId);
        } 

        #endregion
    }
}
