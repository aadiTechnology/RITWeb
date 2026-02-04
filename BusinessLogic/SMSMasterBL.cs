//  -----------------------------------------------------------------------------
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
//  -----------------------------------------------------------------------------


// Class Name       :- SMSMasterDC
// Purpose          :- This class is used to manage SMSMaster details.
// Date Of creation :- 3/21/2008
// Author Name      :- 


using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Utility;
using DataCommunicator;
using BusinessLogic.Exceptions;

namespace BusinessLogic
{    
    public class SMSMasterBL
    {
        private SMSMasterDC.SMSMasterStruct moSMSMasterStruct;

        private SMSMasterDC moSMSMasterDC;

        const string S_MESSAGE = "Enter 10 digit mobile numbers seperated by comma.";

        public SMSMasterBL()
        {
            moSMSMasterDC = new SMSMasterDC();
        }

        public SMSMasterBL(int miSMSDetailsId)
        {
            moSMSMasterDC = new SMSMasterDC(miSMSDetailsId);
            moSMSMasterStruct = moSMSMasterDC.SMSMasterStructDetails;
        }

        public virtual int SMS_Details_Id
        {
            get
            {
                return moSMSMasterStruct.miSMSDetailsId;
            }
            set
            {
                moSMSMasterStruct.miSMSDetailsId = value;
            }
        }

        public virtual int SMS_Type_Id
        {
            get
            {
                return moSMSMasterStruct.miSMSTypeId;
            }
            set
            {
                moSMSMasterStruct.miSMSTypeId = value;
            }
        }

        public virtual string SMSDetailsXML
        {
            get
            {
                return moSMSMasterStruct.msSMSDetailsXML;
            }
            set
            {
                moSMSMasterStruct.msSMSDetailsXML = value;
            }
        }

        public virtual string Sender_Name
        {
            get
            {
                return moSMSMasterStruct.msSenderName;
            }
            set
            {
                moSMSMasterStruct.msSenderName = value;
            }
        }

        public virtual string SMS_Text
        {
            get
            {
                return moSMSMasterStruct.msSMSText;
            }
            set
            {
                moSMSMasterStruct.msSMSText = value;
            }
        }

        public virtual Int32 SMS_Count
        {
            get
            {
                return moSMSMasterStruct.miSMSCount;
            }
            set
            {
                moSMSMasterStruct.miSMSCount = value;
            }
        }


        public virtual string Display_Text
        {
            get
            {
                return moSMSMasterStruct.msDisplayText;
            }
            set
            {
                moSMSMasterStruct.msDisplayText = value;
            }
        }

        public virtual string Is_DeletedFromUser
        {
            get
            {
                return moSMSMasterStruct.msIsDeletedFromUser;
            }
            set
            {
                moSMSMasterStruct.msIsDeletedFromUser = value;
            }
        }

        public virtual string SMSShootId
        {
            get
            {
                return moSMSMasterStruct.msSMSShootId;
            }
            set
            {
                moSMSMasterStruct.msSMSShootId = value;
            }
        }

        public virtual int Sender_User_Role_Id
        {
            get
            {
                return moSMSMasterStruct.miSenderUserRoleId;
            }
            set
            {
                moSMSMasterStruct.miSenderUserRoleId = value;
            }
        }

        public virtual int Sender_User_Id
        {
            get
            {
                return moSMSMasterStruct.miSenderUserId;
            }
            set
            {
                moSMSMasterStruct.miSenderUserId = value;
            }
        }

        public virtual int SchoolId
        {
            get
            {
                return moSMSMasterStruct.miSchoolId;
            }
            set
            {
                moSMSMasterStruct.miSchoolId = value;
            }
        }

        public virtual int AcademicYearId
        {
            get
            {
                return moSMSMasterStruct.miAcademicYearId;
            }
            set
            {
                moSMSMasterStruct.miAcademicYearId = value;
            }
        }

        public virtual string Is_Deleted
        {
            get
            {
                return moSMSMasterStruct.msIsDeleted;
            }
            set
            {
                moSMSMasterStruct.msIsDeleted = value;
            }
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moSMSMasterStruct.mdtInsertDate;
            }
            set
            {
                moSMSMasterStruct.mdtInsertDate = value;
            }
        }

        public virtual int Inserted_By_Id
        {
            get
            {
                return moSMSMasterStruct.miInsertedById;
            }
            set
            {
                moSMSMasterStruct.miInsertedById = value;
            }
        }

        public virtual System.DateTime Updated_Date
        {
            get
            {
                return moSMSMasterStruct.mdtUpdatedDate;
            }
            set
            {
                moSMSMasterStruct.mdtUpdatedDate = value;
            }
        }

        public virtual int Updated_By_Id
        {
            get
            {
                return moSMSMasterStruct.miUpdatedById;
            }
            set
            {
                moSMSMasterStruct.miUpdatedById = value;
            }
        }

        public virtual bool IsScheduled
        {
            get { return moSMSMasterStruct.mbIsScheduled; }
            set { moSMSMasterStruct.mbIsScheduled = value; }
        }

        public virtual DateTime ScheduledDate
        {
            get { return moSMSMasterStruct.ScheduleDateTime; }
            set { moSMSMasterStruct.ScheduleDateTime = value; }
        }

        public virtual int InsertSMSMaster()
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;
            return moSMSMasterDC.InsertSMSMaster();
        }

        /// <summary>
        /// This procedure is used to delete scheduled SMS.
        /// </summary>
        /// <param name="aiSMSId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void DeleteScheduledSMS(int aiSMSId, int aiSchoolId, int aiAcademicYearId)
        {
            moSMSMasterDC.DeleteScheduledSMS(aiSMSId, aiSchoolId, aiAcademicYearId);
        }

        public virtual void InsertSMSMaster(List<SMSReceiverDetailsBL> aoSMSReceiverDetailsBLList)
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;

            ArrayList oArrayListInsertStatements = new ArrayList();
            oArrayListInsertStatements.Add(moSMSMasterDC.GetInsertSMSMaster());
            oArrayListInsertStatements.Add("SELECT SCOPE_IDENTITY() as " + Constants.S_LAST_INSERTED_P_KEY);

            foreach (SMSReceiverDetailsBL oSMSReceiverDetailsBL  in aoSMSReceiverDetailsBLList)
                oArrayListInsertStatements.Add(oSMSReceiverDetailsBL.GetInsertSMSReceiverDetails());

            moSMSMasterDC.InsertSMSMaster(oArrayListInsertStatements);
        }

        public virtual void UpdateSMSMaster()
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;
            moSMSMasterDC.UpdateSMSMaster();
        }

        public virtual void DeleteSMSMaster()
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;
            moSMSMasterDC.DeleteSMSMaster();
        }

        /// <summary>
        /// This method is usedto delete SMS from sent items list.
        /// </summary>
        /// <param name="aoArrMessageDetailsId"></param>
        public void DeleteSentItems(Hashtable aoHtMessageDetailsId)
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;
            moSMSMasterDC.DeleteSentItems(aoHtMessageDetailsId);
        }

        public void DeleteScheduledSMS(string asSMSIds, int aiSchoolId, int aiAcademicYearId)
        {
            moSMSMasterDC.DeleteScheduledSMS(asSMSIds, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is usedto delete SMS from sent items list.
        /// </summary>
        /// <param name="aoArrMessageDetailsId"></param>
        public void DeleteSMSFromInbox(Hashtable aoHtMessageDetailsId)
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;
            moSMSMasterDC.DeleteSMSFromInbox(aoHtMessageDetailsId);
        }

        /// <summary>
        /// This method is used to get the receiver name list.
        /// </summary>
        /// <returns></returns>
        public DataTable GetListOfReceiverName()
        {
            return moSMSMasterDC.GetListOfReceiverName();
        }

        /// <summary>
        /// This method is used to get the Count for sms
        /// </summary>
        /// <returns></returns>
        public DataTable GetCountOfSentSMS(int iSchoolID, int iAcademicYearID)
        {
            return moSMSMasterDC.GetCountOfSentSMS(iSchoolID, iAcademicYearID);
        }

        /// <summary>
        /// This method is used to get the name of SMS Provider ForWebsite 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public string GetSMSProviderForWebsite(int aiSchoolId, int aiAcademicYearId)
        {
            return moSMSMasterDC.GetSMSProviderForWebsite(aiSchoolId, aiAcademicYearId);
        } 
        /// <summary>
        /// Validates mobile nos
        /// </summary>   
        public void ValidateMobileNos(string sMobileNos)
        {
            bool bIsValid = true;
            const string NumberPattern = @"^\d{10}";
            var arrMobileNos = sMobileNos.Split(',');
            for (int iMobNocnt = 0; iMobNocnt < arrMobileNos.Length; iMobNocnt++)
            {
                sMobileNos = arrMobileNos[iMobNocnt].Replace(" ", "").Trim();
                if (sMobileNos != string.Empty)
                    bIsValid = System.Text.RegularExpressions.Regex.IsMatch(sMobileNos, NumberPattern,
        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            if (!bIsValid)
            {
                throw new ValidMobileNumberExceptions(S_MESSAGE);
            }
        }


        public void SaveSMSDetails(string asSMSDetailsXml)
        {
            moSMSMasterDC.SMSMasterStructDetails = moSMSMasterStruct;
            moSMSMasterDC.SaveSMSDetails(asSMSDetailsXml);
        }
    }

  

    public class SMSMasterCollectionBL
    {
        /// <summary>
        /// This method is used to get the Sent Items list for logged in user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public static DataTable GetSentItemsForUser(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent, String sortExpression, int maximumRows, int startRowIndex, int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.GetSentItemsForUser(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName, asContent, sortExpression, maximumRows, startRowIndex, aiShowAllSMS);

        }
        public static int CountSentSMS(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent,int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.CountSentSMS(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName, asContent, aiShowAllSMS);

        }

        /// <summary>
        /// This method is used to export the details of Sent Items list for logged in user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public static DataTable GetSentItemsForUserForExport(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent, String sortExpression, int maximumRows, int startRowIndex, int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.GetSentItemsForUserForExport(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName, asContent, sortExpression, maximumRows, startRowIndex, aiShowAllSMS);

        }

        /// <summary>
        /// This method is used to get the received SMS Items list for logged in user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public static DataTable GetReceivedSMSItemsForUser(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent, String sortExpression, int maximumRows, int startRowIndex, int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.GetReceivedSMSItemsForUser(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName, asContent, sortExpression, maximumRows, startRowIndex, aiShowAllSMS);
        }
        public static int CountSMS(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent, int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.GetReceivedSMSCountUser(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName, asContent,  string.Empty, 10000, 0);         
        }

        /// <summary>
        /// This method is used to get the received SMS Items list for logged in user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public static DataTable GetScheduledSMS(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent, String sortExpression, int maximumRows, int startRowIndex, int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.GetScheduledSMS(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName, sortExpression, maximumRows, startRowIndex);
        }

        /// <summary>
        /// This method is used to return the count of scheduled SMS.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asName"></param>
        /// <param name="asContent"></param>
        /// <returns></returns>
        public static int CountScheduledSMS(int aiSchoolId, int aiUserId, int aiUserRoleId, int aiAcademicYearId, String asName, String asContent, int aiShowAllSMS)
        {
            return SMSMasterCollectionDC.CountScheduledSMS(aiSchoolId, aiUserId, aiUserRoleId, aiAcademicYearId, asName);
        }

        /// <summary>
        /// This method is used to get existing group information.
        /// </summary>
        /// <param name="aiSmsId"></param>
        /// <returns></returns>
        public DataTable GetExistingGroup(int aiSmsId,int aiSchoolId,int aiAcademicYearId)
        {
            return SMSMasterCollectionDC.GetExistingGroup(aiSmsId,aiSchoolId,aiAcademicYearId);
        }
        
        /// <summary>
        /// This method is used to check is there principal present in existing group or not.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable IsPrincipal(int aiUserId, int aiAcademicYearId)
        {
            return SMSMasterCollectionDC.IsPrincipal(aiUserId, aiAcademicYearId);
        }
    }

}
