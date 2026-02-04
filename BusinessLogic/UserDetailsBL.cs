/*
 * Created By:- Rohini
 * Date:- 28 Jun 2013
 * Description: This class is used to get and save user's education details and experience details.
 */
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
	/// <summary>
	/// This class is used to save, update and delete user's experience and education details.
	/// </summary>
	public class UserDetailsBL
	{
		#region "Data Member"
		
		private UserDetailsDC moUserDetailsDC;

		#endregion

		#region "Constructor"

		public UserDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
		{
			moUserDetailsDC = new UserDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
		}

		#endregion

		#region "Public Methods"
		/// <summary>
		/// This method is used to get list of education details of user.
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public List<UserEducationDetails> GetEducationDetailsList(int aiUserId)
		{
			return UserDetailsDC.GetEducationDetailsList(aiUserId);
		}

		/// <summary>
		/// This method is used to get list of experience details.
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public List<UserExperienceDetails> GetExperienceDetailsList(int aiUserId)
		{
			return moUserDetailsDC.GetExperienceDetailsList(aiUserId);
		}

		/// <summary>
		/// This methhod is uesd to get education details for update.
		/// </summary>
		/// <param name="aiEducationDetailsId"></param>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public UserEducationDetails GetEducationDetails(int aiEducationDetailsId, int aiUserId)
		{
			return UserDetailsDC.GetEducationDetails(aiEducationDetailsId, aiUserId);
		}

		/// <summary>
		/// This method is used to get experience details for update.
		/// </summary>
		/// <param name="aiExperienceDetailsId"></param>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public UserExperienceDetails GetExperienceDetails(int aiExperienceDetailsId, int aiUserId)
		{
			return moUserDetailsDC.GetExperienceDetails(aiExperienceDetailsId, aiUserId);
		}

		/// <summary>
		/// This method is used to get all applicable documents.
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public List<UserDocument> GetApplicableDocumentList(int aiUserId)
		{
			return moUserDetailsDC.GetApplicableDocumentList(aiUserId);
		}

		/// <summary>
		/// This method is used to insert/update experience details.
		/// </summary>
		/// <param name="aoUserExperienceDetails"></param>
		public void SaveExperienceDetails(UserExperienceDetails aoUserExperienceDetails)
		{
			moUserDetailsDC.SaveExperienceDetails(aoUserExperienceDetails);
		}

		/// <summary>
		/// This method is used to insert or update education details.
		/// </summary>
		/// <param name="aoUserEducationDetails"></param>
		public void SaveEducationDetails(UserEducationDetails aoUserEducationDetails)
		{
			moUserDetailsDC.SaveEducationDetails(aoUserEducationDetails);
		}

		/// <summary>
		/// This method is used delete education details
		/// </summary>
		/// <param name="aiEducationDetailsId"></param>
		/// <param name="aiUserId"></param>
		public void DeleteEducationDetails(int aiEducationDetailsId, int aiUserId)
		{
			moUserDetailsDC.DeleteEducationDetails(aiEducationDetailsId, aiUserId);
		}

		/// <summary>
		/// This method is delete experience details.
		/// </summary>
		/// <param name="aiExperienceDetailsId"></param>
		/// <param name="aiUserId"></param>
		public void DeleteExperienceDetails(int aiExperienceDetailsId, int aiUserId)
		{
			moUserDetailsDC.DeleteExperienceDetails(aiExperienceDetailsId, aiUserId);
		}
		
		#endregion
	}
}
