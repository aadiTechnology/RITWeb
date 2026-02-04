using System;

namespace BusinessLogic.Exceptions
{
    /// <summary>
    /// This class is used for exception handling.
    /// </summary>
    class StandardwiseTestSchedule
    {
    }

    #region Exception Class

    /// <summary>
    /// This class is used for exception handling for predefined Exam Dates.
    /// </summary>
    public class PreDefinedStartAndEndDate: Exception
    {

        private string msMessage = " ";
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }


        public PreDefinedStartAndEndDate(string asMessage)
        {
            //This method is used to assign message for duplicate name.
            msMessage = asMessage;
        }
    }

    #endregion

}
