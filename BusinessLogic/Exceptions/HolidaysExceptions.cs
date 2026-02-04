using System;

namespace BusinessLogic.Exceptions
{
    class HolidaysExceptions
    {
    }

    public class DuplicateHolidayName : Exception
    {
        private string msMessage = " ";
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }
        public DuplicateHolidayName(string asMessage)
        {
            msMessage = asMessage;
        }
    }

    public class RecordAlreadyExists : Exception
    {
        private string msMessage = " ";
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }
        public RecordAlreadyExists(string asMessage)
        {
            msMessage = asMessage;
        }
    }

    public class PerdefinedStartAndEndDate : Exception
    {
        private string msMessage = " ";
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }
        public PerdefinedStartAndEndDate(string asMessage)
        {
            msMessage = asMessage;
        }
    }


    public class NonWorkingDay : Exception
    {
        private string msMessage = "";
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }
        public NonWorkingDay(string asMessage)
        {
            msMessage = asMessage;
        }
    }
}
