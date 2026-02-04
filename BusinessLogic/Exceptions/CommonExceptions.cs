/* ----------------------------------------------------------------------------
 *	Filename	: CommonExceptions.cs
 *	Author		: Vishal B. Shah
 *	Date		: 25-Nov-2011
 *	Description	: All common exceptions that can be thrown, are included here.
 * ----------------------------------------------------------------------------
 */

using System;

namespace BusinessLogic.Exceptions
{
	public class InvalidSqlDateTimeException : Exception
    {
        private string msMessage = String.Empty;
        
        public override string Message
        {
            get { return msMessage; }
        }
        
        public InvalidSqlDateTimeException(string asMessage)
        {
            msMessage = asMessage;
        }
    }
}
