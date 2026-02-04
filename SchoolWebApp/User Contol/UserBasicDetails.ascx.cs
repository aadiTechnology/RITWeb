using System;
using System.Reflection;
using PayrollEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// UserBasicDetailsUC is Used to Save and Get Basic details of User.
/// </summary>
namespace SchoolWebApp
{
    public partial class UserBasicDetailsUC : System.Web.UI.UserControl
    {
        private int iStaffUserId;
        private UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL;

        #region Properties
        //This property is used to set width of the user control according to page size
        public string Width
        {
            set { tdUC.Width = value; }
        }

        //This property is used to set UserId for saving the details.
        public int StaffUserId
        {
            get { return iStaffUserId; }
            set { iStaffUserId = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to get user basic details.
        /// </summary>
        public void InitializeFields()
        {

            int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
            moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
            UserBasicDetails oUsersBasicDetails = moUsersStaffGroupsAssociationBL.GetUserBasicDetails(iStaffUserId, iSchoolId);
            txtPanNo.Text = oUsersBasicDetails.PanNo;
            txtJoiningDate.Text = oUsersBasicDetails.JoiningDate;
            txtPermanentDate.Text = oUsersBasicDetails.PermanentDate;
            txtResignationDate.Text = oUsersBasicDetails.ResignationDate;
        }

        /// <summary>
        /// This method is used to set user basic details.
        /// </summary>
        public void PopulateUserBasicDetails()
        {
            UserBasicDetails olstUserBasicDetails = PopulateDetailsList();
            int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
            int iLeaveSeperaterDay = Convert.ToInt32(Resources.SchoolSettings.LeaveSeperaterDay);
            moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
            moUsersStaffGroupsAssociationBL.SaveBasicDetails(olstUserBasicDetails, iAcademicYearId, iLeaveSeperaterDay);
        }

        /// <summary>
        /// This method is used to clear the fields.
        /// </summary>
        public void ClearFields()
        {
            txtJoiningDate.Text = string.Empty;
            txtPanNo.Text = string.Empty;
            txtPermanentDate.Text = string.Empty;
            txtResignationDate.Text = string.Empty;
        }

        /// <summary>
        /// This function is used to validate the profile details before saving it.
        /// </summary>
        public void ValidateProfile()
        {
            UserBasicDetails olstUserBasicDetails = PopulateDetailsList();
            int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
            moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
            moUsersStaffGroupsAssociationBL.ValidateProfileDetails(olstUserBasicDetails, iAcademicYearId);
        }

        /// <summary>
        ///This is a common function for populating user basic details list. 
        /// </summary>
        public UserBasicDetails PopulateDetailsList()
        {
            UserBasicDetails olstUserBasicDetails = new UserBasicDetails
                {
                    UserId = iStaffUserId,
                    PanNo = txtPanNo.Text.Trim(),
                    JoiningDate = txtJoiningDate.Text,
                    PermanentDate = txtPermanentDate.Text,
                    ResignationDate = txtResignationDate.Text,
                    SchoolId =
                        Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]),
                    InsertedById =
                        Convert.ToInt32(Session[Constants.S_SESSION_USER_ID])
                };
            return olstUserBasicDetails;
        }

        #endregion
    }
}