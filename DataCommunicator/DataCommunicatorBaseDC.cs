/*
 * File Name         :- DataCommunicatorBaseDC.cs
 * Purpose           :- This Class is treated as the base class for the other DC classes.
 * Date of creation  :- 28 Jan 2007.  
*/



namespace DataCommunicator
{
    public class DataCommunicatorBaseDC
    {
        #region Public Static Methods

        public static string GetSelectStatementForLastInsertedPKey(string sPKeyConstant)
        {
            //This function returns the select statement to get the last inserted primay key.
            string sSqlStatement = " SELECT " +
                    " SCOPE_IDENTITY() as " + sPKeyConstant;

            return sSqlStatement;
        }   

        #endregion
    }
}
