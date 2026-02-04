/* -------------------------------------------------------------------------------
 *	DEVELOPMENT LOG
 * -------------------------------------------------------------------------------
 *	Author	: Yogesh Karne
 *	Date	: 1-Jan-2016
 *	Purpose	: Used to mark specific item as damaged.
 * -------------------------------------------------------------------------------
 */
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.Inventory;
using Utility;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used to get and upadate homework details.
    /// </summary>
    public class ItemSpecificationBL
    {
        #region "Data Members"
        
        private ItemSpecificationDC moItemSpecificationDC;
     
        #endregion

        #region "Constructor"

        public ItemSpecificationBL()
        {
            this.moItemSpecificationDC = new ItemSpecificationDC();
        }

        public ItemSpecificationBL(int aiSchoolId, int aiUserId)
        {
            this.moItemSpecificationDC = new ItemSpecificationDC(aiSchoolId, aiUserId);
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// Method is used to Get All details acording to filters.
        /// </summary>
        /// <param name="aiItemId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public List<ItemSpecificationDetails> GetAll(int aiItemId, int aiSchoolId, string asSortExpression, string asSortDirection, int MaximumRows, int StartRowIndex)
        {
            int iStartRowIndex = StartRowIndex;
            if (StartRowIndex != 0)
                iStartRowIndex = StartRowIndex + 1;
            int iEndRowIndex = iStartRowIndex + MaximumRows;
            if (asSortExpression == "" || asSortExpression == null)
            {
                asSortExpression = "SpecificationCode";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }
            asSortExpression = asSortExpression + " " + asSortDirection;
          return  this.moItemSpecificationDC.GetAll(aiItemId, aiSchoolId, asSortExpression, iStartRowIndex, iEndRowIndex);
        }

        /// <summary>
        /// This method is used to count item details.
        /// </summary>
        /// <param name="aiItemID"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <returns></returns>
        public int GetCount(int aiItemId, int aiSchoolId, string asSortExpression, string asSortDirection, int MaximumRows, int StartRowIndex)
        {
            return this.moItemSpecificationDC.GetCount(aiItemId, aiSchoolId);
        }

        /// <summary>
        /// This method is used to get specific item details according to filter.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public ItemSpecificationDetails Get(int aiId)
        {
            return this.moItemSpecificationDC.Get(aiId);
        }

        /// <summary>
        /// This method is used to save or update Item specification details. 
        /// </summary>
        /// <param name="aoItemSpecificationDetails"></param>
        public void Save(ItemSpecificationDetails aoItemSpecificationDetails)
        {
             this.moItemSpecificationDC.Save(aoItemSpecificationDetails);
        }

        /// <summary>
        /// This event is used to delete item specification detials.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        public void Delete(int aiId)
        {
            this.moItemSpecificationDC.Delete(aiId);
        }

        #endregion
    }
}
