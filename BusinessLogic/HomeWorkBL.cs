using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using System;

namespace BusinessLogic
{
	/// <summary>
	/// This class is used to get and upadate homework details.
	/// </summary>
	public class HomeWorkBL
	{
		#region "Data Members"

		HomeworkDC moHomeworkDC;

		#endregion

		#region "Constructor"

		public HomeWorkBL(int aiSchoolId, int aiAcdemicYearId, int aiUserId)
		{
			moHomeworkDC = new HomeworkDC(aiSchoolId, aiAcdemicYearId, aiUserId);
		}

		#endregion 

		#region "Public Methods"

		/// <summary>
		/// This method is used to get homework details according to standard division.
		/// </summary>
		/// <param name="aiStdDivId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiTeacherId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiSubjectId"></param>
		/// <param name="abFlag"></param>
		/// <returns></returns>
		public List<Homework> GetListForTeacher(int aiStdDivId, string asDate ,string asHomeWorkStatus, string asTitle)
		{
            return moHomeworkDC.GetListForTeacher(aiStdDivId, asDate, asHomeWorkStatus,asTitle);
		}

		/// <summary>
		/// This method is used to get list of homewoek for students.
		/// </summary>
		/// <param name="aiStdDivId"></param>
		/// <param name="asDate"></param>
		/// <returns></returns>
		public List<Homework> GetListForStudent(int aiStdDivId, string asDate,string asHomeworkStatus)
		{
            return moHomeworkDC.GetListForStudent(aiStdDivId, asDate, asHomeworkStatus);
		}

		/// <summary>
		/// This method is used to get homework details for provided id.
		/// </summary>
		/// <param name="aiHomeWorkId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public Homework Get(int aiHomeWorkId)
		{
			return moHomeworkDC.Get(aiHomeWorkId);
		}

		/// <summary>
		/// This method is used to insert or update homework details.
		/// </summary>
		/// <param name="asXml"></param>
		/// <param name="abUpdateFlag"></param>
		/// <returns></returns>
        public void Save(Homework aoHomework,string asFileName)
      // public void Save(Homework aoHomework,string asFileName)
		{
			moHomeworkDC.Save(aoHomework,asFileName);
		}

		/// <summary>
		/// This method is used to publish the homework.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="asReason"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
        public void Publish(string asHomeworkIds, bool abIsSMSSent)
		{
            moHomeworkDC.Publish(asHomeworkIds, abIsSMSSent);
		}
        
		/// <summary>
		/// This method is used to unpublish the homework.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="asReason"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
        public void UnPublish(string asHomeworkIds, string asReason)
		{
            moHomeworkDC.UnPublish(asHomeworkIds, asReason);
		}

        /// <summary>
        /// This method is used to return homework sms status.
        /// </summary>
        /// <param name="aiStdDivId"></param>
        /// <param name="adtAssignedDate"></param>
        /// <returns></returns>
        public bool IsHomeworkSMSSent(int aiStdDivId, DateTime adtAssignedDate)
        {
            return moHomeworkDC.IsHomeworkSMSSent(aiStdDivId, adtAssignedDate);
        }



        /// <summary>
        /// This method is used to return investment documents.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<Homework> GetDocuments(int aiHomeworkId, int aiAcademicYearId)
        {
            return moHomeworkDC.GetDocuments(aiHomeworkId,aiAcademicYearId);
        }
		/// <summary>
		/// This method is used to delete homework.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
        public void Delete(int aiHomeworkId, string asDeleteFromAllClasses)
		{
            moHomeworkDC.Delete(aiHomeworkId, asDeleteFromAllClasses);
		}
        /// <summary>
        /// This method is used to delete document.
        /// </summary>
        /// <param name="iId"></param>
        public string DeleteDocument(int aiDocumentId, string asDeleteFromAllClasses)
        {
           return moHomeworkDC.DeleteDocument(aiDocumentId, asDeleteFromAllClasses);
        }

		#endregion
	}
}
