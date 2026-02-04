/*File Name - TransportChargesBL.cs
 * Created By - Pravin Shinde
 * Created Date - 26 Dec 2013
 * Description - This class is used to search/pay/refund transport charges of user.
 */
namespace BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataCommunicator;
    using SchoolEntities;
    using SchoolEntities.Transport;
using System.Data;

    /// <summary>
    /// This class is used to search/pay/refund transport charges of user.
    /// </summary>
    public class TransportChargesBL
    {
        #region Data Member(s)

        private TransportChargesDC moTransportChargesDC;

        #endregion

        #region Constructor(s)

        public TransportChargesBL()
        {
            this.moTransportChargesDC = new TransportChargesDC();
        }

        public TransportChargesBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moTransportChargesDC = new TransportChargesDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get the transport user details for selected role & criteria.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asName"></param>
        /// <param name="asRole"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<TransportFeeDetails> GetUserDetails(int aiSchoolId, int aiAcademicYearId, string asName, string asRole, String sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moTransportChargesDC.GetUserDetails(aiSchoolId, aiAcademicYearId, asName, asRole, sortExpression, iEndIndex, startRowIndex);
        }

        /// <summary>
        /// This method is used to get count of the transport user details for selected role & criteria.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asName"></param>
        /// <param name="asRole"></param>
        /// <returns></returns>
        public int CountUsers(int aiSchoolId, int aiAcademicYearId, string asName,string asRole)
        {
            return moTransportChargesDC.CountUsers(aiSchoolId, aiAcademicYearId, asName,asRole);
        }

        /// <summary>
        /// This function is used to get the transport charges details of selected role and mode.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aodtCurrentDate"></param>
        /// <param name="abIsForRefund"></param>
        /// <returns></returns>
        public List<PayTransportCharges> GetAll(int aiUserId, DateTime aodtCurrentDate, bool abIsForRefund)
        {
            return moTransportChargesDC.GetAll(aiUserId, aodtCurrentDate, abIsForRefund);
        }

        /// <summary>
        /// This method is used to pay transport charges.
        /// </summary>
        /// <param name="asTransportDetailsXML"></param>
        public void Insert(string asTransportDetailsXML)
        {
            moTransportChargesDC.Insert(asTransportDetailsXML);
        }

        /// <summary>
        ///  This method is used to refund transport charges for selected role.
        /// </summary>
        /// <param name="asTransportFeeId"></param>
        /// <param name="aodtRefundDate"></param>
        public void RefundCharges(string asTransportFeeId, DateTime aodtRefundDate)
        {
            moTransportChargesDC.RefundCharges(asTransportFeeId,aodtRefundDate);
        }

        /// <summary>
        ///  This function is used to delete paid transport charges.
        /// </summary>
        /// <param name="asReceiptNumber"></param>
        /// <param name="abIsRefund"></param>
        public void Delete(string asReceiptNumber,bool abIsRefund)
        {
            moTransportChargesDC.Delete(asReceiptNumber, abIsRefund);
        }

        #endregion
    }
}
