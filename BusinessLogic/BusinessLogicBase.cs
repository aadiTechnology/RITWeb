/*
 * File Name         :- BusinessLogicBase.cs
 * Purpose           :- This Class is trerated as the base class for the businesslogic classes. 
 *                      It contains the generic methods which are called from the derived BL classes.
 * Date of creation  :- 28 Jan 2007.  
*/

using DataCommunicator;

namespace BusinessLogic
{
    public class BusinessLogicBaseBL
    {
       
        #region Overloaded Constructor 

            public BusinessLogicBaseBL()
            { 
            }

        #endregion

        #region Protected Methods 

            protected string GetSelectStatementForLastInsertedPKey(string sPKeyConstant)
            {
                // This method deletes the agent.
                return DataCommunicatorBaseDC.GetSelectStatementForLastInsertedPKey(sPKeyConstant);
            }

        protected static string RemoveCommaAtTheEndIfPresent(string asId)
        {
            string sId = asId;
            if (asId.EndsWith(","))
                sId = asId.Substring(0, asId.LastIndexOf(","));

            return sId;
        }

        #endregion

    }
}
