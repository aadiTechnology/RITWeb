/* -------------------------------------------------------------------------------------
 *	Filename	: VoucherBL.cs
 *	Author		: Vishal B. Shah
 *	Date		: 8-Oct-2011
 *	Description	: This is the Business Logic Layer for Vouchers in the Accounts module.
 * -------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using Utility;

namespace BusinessLogic
{
	public class VoucherBL
	{	
        public void NotifyUsers(int aiSenderUserId, int aiAcademicYearId, string asSubject, string asMessage, List<int> UserIds)
        {
            List<MessageReceiverDetailsBL> lstMessgeReceivers = BuildRecepientList(aiSenderUserId, UserIds);

            SchoolUserBL oSchoolUser = new SchoolUserBL(aiSenderUserId);
            MessageDetailsBL moMessageDetailsBL = BuildMessageDetails(aiSenderUserId, oSchoolUser.UserRoleId, aiAcademicYearId, asSubject, asMessage);
            moMessageDetailsBL.InsertMessageDetails(lstMessgeReceivers);
        }

        public void NotifyUsers(int aiSenderUserId, int aiSenderUserRoleId, int aiAcademicYearId, string asSubject, string asMessage, List<int> UserIds)
        {
            List<MessageReceiverDetailsBL> lstMessgeReceivers = BuildRecepientList(aiSenderUserId, UserIds);

            MessageDetailsBL moMessageDetailsBL = BuildMessageDetails(aiSenderUserId, aiSenderUserRoleId, aiAcademicYearId, asSubject, asMessage);
            moMessageDetailsBL.InsertMessageDetails(lstMessgeReceivers);
        }
		
		private List<MessageReceiverDetailsBL> BuildRecepientList(int aiUserId, List<int> lstUserIds)
		{
			List<MessageReceiverDetailsBL> lstMessgeReceivers = new List<MessageReceiverDetailsBL>();
			foreach(int iUserId in lstUserIds)
			{
				MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
				oMessageReceiverDetailsBL.New_Message_Flag = Constants.C_YES.ToString();
				oMessageReceiverDetailsBL.Read_Message_Flag = Constants.C_NO.ToString();
				oMessageReceiverDetailsBL.Receiver_User_Id = iUserId;
				SchoolUserBL oSchoolUser = new SchoolUserBL(iUserId);
				oMessageReceiverDetailsBL.Receiver_User_Role_Id = oSchoolUser.UserRoleId;
				oMessageReceiverDetailsBL.Updated_By_Id = aiUserId;
				oMessageReceiverDetailsBL.Inserted_By_Id = aiUserId;
				oMessageReceiverDetailsBL.Is_Archive = Constants.C_NO.ToString();
				oMessageReceiverDetailsBL.Is_Deleted = Constants.C_NO.ToString();
				oMessageReceiverDetailsBL.Is_DeletedFromReceiver = Constants.C_NO.ToString();
				lstMessgeReceivers.Add(oMessageReceiverDetailsBL);
			}
			return lstMessgeReceivers;
		}
		
		private MessageDetailsBL BuildMessageDetails(int aiSenderUserId, int aiSenderUserRoleId, int aiAcademicYearId, string asSubject, string asMessage)
		{
			MessageDetailsBL moMessageDetailsBL = new MessageDetailsBL();
			moMessageDetailsBL.Subject = asSubject;
			moMessageDetailsBL.Message_Body = asMessage;
			moMessageDetailsBL.Display_Text = String.Empty;
			moMessageDetailsBL.Inserted_By_Id = aiSenderUserId;
			moMessageDetailsBL.Is_Deleted = Constants.C_NO.ToString();
			moMessageDetailsBL.Is_DeletedFromUser = Constants.C_NO.ToString();
			moMessageDetailsBL.Sender_User_Id = aiSenderUserId;
			moMessageDetailsBL.Sender_User_Role_Id = Convert.ToInt32((Constants.UserRoles)aiSenderUserRoleId);
			moMessageDetailsBL.Updated_By_Id = aiSenderUserId;
			moMessageDetailsBL.AcademicYrId = aiAcademicYearId;
			
			return moMessageDetailsBL;
		}		
	}
}
