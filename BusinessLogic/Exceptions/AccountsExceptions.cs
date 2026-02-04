/* ------------------------------------------------------------------------------------
 *	Filename	: AccountsExceptions.cs
 *	Author		: Vishal B. Shah
 *	Date		: 5-Oct-2011
 *	Description	: This class contains Exceptions that are used in the Accounts module.
 * ------------------------------------------------------------------------------------
 */

using System;

namespace BusinessLogic.Exceptions
{
	/// <summary>
	/// This is a genric exception that can be used to indicate that the Name is duplicate
	/// </summary>
	public class DuplicateNameException : Exception
	{
        public string ErrorMessage { get; private set; }
        
        public DuplicateNameException(string asMessage)
        {
            ErrorMessage = asMessage;
        }
	}
	
	/// <summary>
	/// This is genric exception which can be used in edit/delete/update situation
	/// when the operation can not be carried out becuase of dependencies.
	/// </summary>
	public class DependencyException : Exception
	{
        public string ErrorMessage { get; private set; }

        public DependencyException(string asMessage)
        {
            ErrorMessage = asMessage;  
        }
	}
}
