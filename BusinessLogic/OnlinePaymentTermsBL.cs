// -----------------------------------------------------------------------
// class  : OnlinePaymentTermsBL.cs
// Author : Yogesh
// Date   : 7 Aug 2015
// Description  : This class is used to write business logic about online payment term.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class OnlinePaymentTermsBL
    {
        #region Data Member(s)

        OnlinePaymentTermsDC moOnlinePaymentTermsDC;

        #endregion

        #region Constructor(s)

        public OnlinePaymentTermsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moOnlinePaymentTermsDC = new OnlinePaymentTermsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return data table to fill term category combo box. 
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineTermsCatagory()
        {
            return moOnlinePaymentTermsDC.GetOnlineTermsCatagory();
        }

        /// <summary>
        /// This method is used to get online payment details.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public List<OnlinePaymentTermsDetails> Get(int aiCategoryId)
        {
            return moOnlinePaymentTermsDC.Get(aiCategoryId);
        }

        /// <summary>
        /// This method is used to save Description.
        /// </summary>
        public void Save(int aiId, string asDiscription, int aiTermsCatagoryId)
        {
            moOnlinePaymentTermsDC.Save(aiId, asDiscription, aiTermsCatagoryId);
        }

        /// <summary>
        /// This method is used to delete the on line payment term.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            moOnlinePaymentTermsDC.Delete(aiId);
        }

        #endregion
    }
}
