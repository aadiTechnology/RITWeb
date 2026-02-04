using System;
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
    public class FeedbackDetailsBL
    {
        #region Data members
        
        private FeedbackDetailsDC.FeedbackStructDetails moFeedbackStructDetails;
        private FeedbackDetailsDC moFeedbackDetailsDC = new FeedbackDetailsDC();

        #endregion

        #region Property

        public List<FeedbackType> FeedbackTypes
        {
            get { return moFeedbackDetailsDC.FeedbackType; }
            set { moFeedbackDetailsDC.FeedbackType = value; }
        }

        public int Feedback_Id
        {
            get { return moFeedbackStructDetails.Feedback_Id; }
            set { moFeedbackStructDetails.Feedback_Id = value; }
        }

        public Int32 User_Id
        {
            get
            {
                return moFeedbackStructDetails.miUser_Id;
            }
            set
            {
                moFeedbackStructDetails.miUser_Id = value;
            }
        }
        public Int32 School_Id
        {
            get
            {
                return moFeedbackStructDetails.miSchool_Id;
            }
            set
            {
                moFeedbackStructDetails.miSchool_Id = value;
            }
        }
        public Int32 Feedback_Type_Id
        {
            get
            {
                return moFeedbackStructDetails.miFeedback_Type_Id;
            }
            set
            {
                moFeedbackStructDetails.miFeedback_Type_Id = value;
            }
        }
        public string FeedbackDescription
        {
            get
            {
                return moFeedbackStructDetails.msFeedbackDescription;
            }
            set
            {
                moFeedbackStructDetails.msFeedbackDescription = value;
            }
        }

        public Int32 InsertedById
        {
            get 
            { 
                return moFeedbackStructDetails.miInsertedById; 
            }
            set
            { 
                moFeedbackStructDetails.miInsertedById = value; 
            }
        }

        public string FeedbackFor
        {
            get
            {
                return moFeedbackStructDetails.msFeedbackFor;
            }
            set
            {
                moFeedbackStructDetails.msFeedbackFor = value;
            }
        }

        public System.DateTime InsertedDate
        {
            get 
            {
                return moFeedbackStructDetails.mdtInsertDate; 
            }
            set 
            { 
                moFeedbackStructDetails.mdtInsertDate = value; 
            }
        }
        public Int32 UpdatedById
        {
            get 
            { 
                return moFeedbackStructDetails.miUpdatedById; 
            }
            set 
            { 
                moFeedbackStructDetails.miUpdatedById = value; 
            }
        }
        public System.DateTime UpdatedDate
        {
            get 
            { 
                return moFeedbackStructDetails.mdtUpdateDate; 
            }
            set 
            { 
                moFeedbackStructDetails.mdtUpdateDate = value; 
            }
        }
        public string Email
        {
            get
            {
                return moFeedbackStructDetails.msEmail;
            }
            set
            {
                moFeedbackStructDetails.msEmail = value;
            }
        }

        public string UserName
        {
            get
            {
                return moFeedbackStructDetails.msUserName;
            }
            set
            {
                moFeedbackStructDetails.msUserName = value;
            }
        }

        public int IsSelected
        {
            get { return moFeedbackStructDetails.IsSelected; }
            set { moFeedbackStructDetails.IsSelected = value; }
        }

        #endregion

        #region Helping Method

        /// <summary>
        /// This method is used to fill all roles into checkboxList from table UserRoleMaster.
        /// </summary>
        /// <returns></returns>
        public List<FeedbackTemplate> RetriveFeedbackTypeFromFeedbackTypeMaster()
        {
            moFeedbackDetailsDC.FeedbackInfo = moFeedbackStructDetails;
            return moFeedbackDetailsDC.RetriveFeedbackTypeFromFeedbackTypeMaster();
        }       

        /// <summary>
        /// This method is used to add Feedback in database.
        /// </summary>
        public void InsertFeedbackDetails()
        {
            moFeedbackDetailsDC.FeedbackInfo = moFeedbackStructDetails;
            moFeedbackDetailsDC.InsertFeedbackDetails();
        }

        /// <summary>
        /// This method is used to get mail addresses for school.
        /// </summary>
        /// <returns></returns>
        public string GetMailAddressForSchool(int aiSchoolId)
        {
            return moFeedbackDetailsDC.GetMailAddressForSchool(aiSchoolId);
        }

        /// <summary>
        /// This method is used to delete(update Is_Deleted= 'Y') the feedback from database.
        /// </summary>
        public void DeleteFeedbackDetails(int iFeedbackID, int iUserId)
        {
            moFeedbackDetailsDC.FeedbackInfo = moFeedbackStructDetails;
            moFeedbackDetailsDC.DeleteFeedbackDetails(iFeedbackID, iUserId);
        }
        
        /// <summary>
        /// This method is used to get user feedback details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiFeedback_Type_Id"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <returns>DataSet</returns>
        public DataTable GetUserFeedbackDetails(int aiSchoolId, string sortDirection, String sortExpression,string asUserName ,int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moFeedbackDetailsDC.GetUserFeedbackDetails(aiSchoolId, sortDirection, sortExpression.Replace("DESC", "").Replace("ASC", ""),asUserName, maximumRows, startRowIndex, iEndIndex);
        }

        public DataTable GetUserFeedbackDetails(int aiUserRoleId, int aiFeedbackTypeId, string asFeedBackFor, int aiSchoolId, string sortDirection, string asStartDate, string asEndDate, String sortExpression, int startRowIndex,int maximumRows)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moFeedbackDetailsDC.GetUserFeedbackDetails(aiUserRoleId, aiFeedbackTypeId, asFeedBackFor, aiSchoolId, sortDirection,asStartDate,asEndDate, sortExpression, startRowIndex, iEndIndex);
        }

        /// <summary>
        /// This method is used to get user feedback details count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiFeedback_Type_Id"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <returns>DataSet</returns>
        public int GettFeedbackCount(int aiUserRoleId, int aiFeedbackTypeId, string asFeedBackFor, int aiSchoolId, string asStartDate, string asEndDate, string sortDirection)
        {
            int iReturnValue = 0;

            DataTable oDt = moFeedbackDetailsDC.GetUserFeedbackDetails(aiUserRoleId,aiFeedbackTypeId,asFeedBackFor,aiSchoolId,string.Empty,asStartDate,asEndDate,string.Empty,0,20);
            if (oDt.Rows.Count > 0)
            {
                iReturnValue = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            }
            return iReturnValue;
        }

        /// <summary>
        /// This method is uesd to get count.(feedback screen on super admin)
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortDirection"></param>
        /// <param name="asUserName"></param>
        /// <returns></returns>
        public int GettUsersFeedbackCount(int aiSchoolId, string sortDirection, string asUserName)
        {
            int iReturnValue = 0;

            DataTable oDt = moFeedbackDetailsDC.GetUserFeedbackDetails(aiSchoolId, sortDirection, "", asUserName, 3, 0, 20);
            if (oDt.Rows.Count > 0)
            {
                iReturnValue = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            }
            return iReturnValue;
        }

        /// <summary>
        /// This method is used to save selected feedback.
        /// </summary>
        /// <param name="asXML"></param>
        /// <param name="aiFlag"></param>
        public void SaveSelectedFeedback(string asXML, int aiFlag)
        {
            moFeedbackDetailsDC.SaveSelectedFeedback(asXML, aiFlag);
        }

        /// <summary>
        /// This method is used to get feedback for edit.
        /// </summary>
        /// <param name="aiFeeedbackId"></param>
        /// <param name="aiSchoolId"></param>
        public void GetFeedbackToEdit(int aiFeeedbackId, int aiSchoolId)
        {
            moFeedbackStructDetails = moFeedbackDetailsDC.GetFeedbackToEdit(aiFeeedbackId, aiSchoolId);
            Feedback_Id=moFeedbackStructDetails.Feedback_Id;
            Email=moFeedbackStructDetails.msEmail;
            FeedbackDescription=moFeedbackStructDetails.msFeedbackDescription;
            FeedbackFor=moFeedbackStructDetails.msFeedbackFor;
            Feedback_Type_Id=moFeedbackStructDetails.miFeedback_Type_Id;
            User_Id=moFeedbackStructDetails.miUser_Id;
            UserName = moFeedbackStructDetails.msUserName;
        }

        public static List<FeedbackDetailsBL> GetSelectedFeedback(int aiSchoolId)
        {
             List<FeedbackDetailsDC.FeedbackStructDetails> lstDetails= FeedbackDetailsDC.GetSelectedFeedback(aiSchoolId);
             List<FeedbackDetailsBL> lstFeedbackList = new List<FeedbackDetailsBL>();
             FeedbackDetailsBL oFeedbackDetailsBL;
            foreach ( var item in lstDetails)
            {
                oFeedbackDetailsBL=new FeedbackDetailsBL();
                oFeedbackDetailsBL.FeedbackDescription = item.msFeedbackDescription.Replace("\n", "<BR>"); 
                oFeedbackDetailsBL.Email = item.msEmail;
                oFeedbackDetailsBL.InsertedDate = item.mdtInsertDate;
                oFeedbackDetailsBL.UserName=item.msUserName;
                lstFeedbackList.Add(oFeedbackDetailsBL);
            }
            return lstFeedbackList;
        }
        public void InsertOtherFeedbackDetails(FeedbackDetails oFeedbackDetails)
        {
            moFeedbackDetailsDC.InsertOtherFeedbackDetails(oFeedbackDetails);
        }
        public static List< FeedbackDetails> GetOtherFeedback(int aiSchoolId,string asFilter)
        {
            return FeedbackDetailsDC.GetOtherFeedback(aiSchoolId ,asFilter);
        }
        public void UpdateOtherFeedback(FeedbackDetails oFeedbackDetails)
        {
            moFeedbackDetailsDC.UpdateOtherFeedback(oFeedbackDetails);
        }

        public void SaveOtherFeedback(string sXML)
        {
            moFeedbackDetailsDC.SaveOtherFeedback(sXML);
        }
        /// <summary>
        /// This method use to get feedback details of users
        /// </summary>
        /// <returns></returns>
        public static List<UsersFeedbackDetails> GetFeedbackDetails()
        {
            return FeedbackDetailsDC.GetFeedbackDetails();
        }
        #endregion
    }
}
