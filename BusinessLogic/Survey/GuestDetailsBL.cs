using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator.Survey;
using SchoolEntities.Survey;
using Utility;
namespace BusinessLogic.Survey
{
    public class GuestDetailsBL
    {

        #region DataMember(S)

        public GuestDetailsDC moGuestDetailsDC;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// This is default constructor.
        /// </summary>
        public GuestDetailsBL()
        {
        }

        /// <summary>
        /// This is parameterized constructor.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        public GuestDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moGuestDetailsDC = new GuestDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to get reference guest name.
        /// </summary>
        /// <returns></returns>
        public List<GuestReferenceDetails> GetReferenceGuestName()
        {
            return moGuestDetailsDC.GetReferenceGuestName();
        }

        /// <summary>
        /// This method is used to save guest details.
        /// </summary>
        /// <param name="moGuestDetails"></param>
        public void Save(GuestDetails moGuestDetails)
        {
            moGuestDetailsDC.Save(moGuestDetails);
        }

        /// <summary>
        /// This method is used to get all guest details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<GuestDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "FirstName";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }
            asSortExpression = asSortExpression + " " + asSortDirection;
            int iEndIndex = startRowIndex + maximumRows;
            GuestDetailsDC oGuestDetailsDC = new GuestDetailsDC();
            return oGuestDetailsDC.GetAll(aiSchoolId, aiAcademicYearId, asSortExpression, startRowIndex, iEndIndex);
        }

        /// <summary>
        /// This method is used to count number of items record.
        /// </summary>
        /// <param name="aiItemId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection)
        {
            GuestDetailsDC oGuestDetailsDC = new GuestDetailsDC();
            return oGuestDetailsDC.Count(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get guest details.
        /// </summary>
        /// <param name="aiGuestId"></param>
        /// <returns></returns>
        public GuestDetails Get(int aiGuestId)
        {
            return moGuestDetailsDC.Get(aiGuestId);
        }

        /// <summary>
        /// This method is used to update guest details.
        /// </summary>
        /// <param name="aiGuestId"></param>
        /// <param name="moGuestDetails"></param>
        public void Update(int aiGuestId, GuestDetails moGuestDetails)
        {
            moGuestDetailsDC.Update(aiGuestId, moGuestDetails);
        }

        /// <summary>
        /// This method is used to delete guest details.
        /// </summary>
        /// <param name="aiGuestId"></param>
        public void Delete(int aiGuestId)
        {
            moGuestDetailsDC.Delete(aiGuestId);
        }

        #endregion

    }
}
