using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Net;
using System.IO;
using Utility;
using BusinessLogic;

/// <summary>
/// Summary description for Message
/// </summary>
public class Message
{

    string msMessageBody;
    string msMessageSubject;
    private List<MessageReceiverDetailsBL> MessageReceiverDetailsBLList = new List<MessageReceiverDetailsBL>();

    public Message()
    {
    }

    public string sMessageBody
    {
        get
        {
            return msMessageBody;
        }
        set
        {
            msMessageBody = value;
        }
    }

    public string sMessageSubject
    {
        get
        {
            return msMessageSubject;
        }
        set
        {
            msMessageSubject = value;
        }
    }

    public void SetMessageReceivers(string asUserId,int aiUserId)
    {   
        Hashtable moHTUsersMobileNo = new Hashtable();

        //Split comma separated users ids
        string[] sArrUserIds = asUserId.Split(';');
        if (asUserId.Trim() != string.Empty)
        {
            for (int iCount = 0; iCount < sArrUserIds.Length; iCount++)
                moHTUsersMobileNo[Convert.ToInt32(sArrUserIds[iCount])] = Convert.ToInt32(sArrUserIds[iCount]);

            foreach (DictionaryEntry oDE in moHTUsersMobileNo)
            {
                MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
                oMessageReceiverDetailsBL.New_Message_Flag = "Y";
                oMessageReceiverDetailsBL.Read_Message_Flag = "N";
                oMessageReceiverDetailsBL.Receiver_User_Id = Convert.ToInt32(oDE.Key);
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(Convert.ToInt32(oDE.Key));
                oMessageReceiverDetailsBL.Receiver_User_Role_Id = oSchoolUserBL.UserRoleId;
                oMessageReceiverDetailsBL.Updated_By_Id = aiUserId;
                oMessageReceiverDetailsBL.Inserted_By_Id = aiUserId;
                oMessageReceiverDetailsBL.Is_Archive = "N";
                oMessageReceiverDetailsBL.Is_Deleted = "N";
                oMessageReceiverDetailsBL.Is_DeletedFromReceiver = "N";
                MessageReceiverDetailsBLList.Add(oMessageReceiverDetailsBL);
            }
        }
    }

    /// <summary>
    /// This method creates an object for message details.
    /// 
    /// </summary>
    public void InsertMessageDetails(int aiUserId,int aiUserRoleId,int aiAcademicYrId)
    {
        //This function is used to insert the Message details.
        MessageDetailsBL moMessageDetailsBL = new MessageDetailsBL();
        moMessageDetailsBL.Subject = sMessageSubject;
        moMessageDetailsBL.Message_Body = sMessageBody;
        moMessageDetailsBL.Display_Text = "";
        moMessageDetailsBL.Inserted_By_Id = aiUserId;
        moMessageDetailsBL.Is_Deleted = "N";
        moMessageDetailsBL.Is_DeletedFromUser = "N";
        moMessageDetailsBL.Sender_User_Id = aiUserId;
        moMessageDetailsBL.Sender_User_Role_Id = Convert.ToInt32((Constants.UserRoles)aiUserRoleId);
        moMessageDetailsBL.Updated_By_Id = aiUserId;
        moMessageDetailsBL.AcademicYrId = aiAcademicYrId;
        moMessageDetailsBL.Cc_Display_Text = "";
        moMessageDetailsBL.InsertMessageDetails(MessageReceiverDetailsBLList, new List<string>());
    }
}
