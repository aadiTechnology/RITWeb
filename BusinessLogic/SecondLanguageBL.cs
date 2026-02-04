using System.Collections.Generic;
using DataCommunicator;
using MasterEntities;

namespace BusinessLogic
{
	public class SecondLanguageBL
	{
		#region "Data member"

		private SecondLanguageDC moSecondLanguageDC;

		#endregion

		#region "Constructor"

		public SecondLanguageBL(int aiSchoolId, int aiAcademicYearId)
		{
			moSecondLanguageDC = new SecondLanguageDC(aiSchoolId, aiAcademicYearId);
		}

		#endregion

		#region "Public Methods"

		public List<SubjectMaster> GetAll(int aiStandardDivisionId)
		{
			return moSecondLanguageDC.GetAll(aiStandardDivisionId);
		}
		
		/// <summary>
		/// 	This method is used to get second languages.
		/// </summary>
		/// <param name="aiStandardId"></param>
		/// <param name="aiDivisionId"></param>
		/// <returns> </returns>
		public List<SubjectMaster> GetAll(int aiStandardId, int aiDivisionId)
		{
			return moSecondLanguageDC.GetAll(aiStandardId, aiDivisionId);
		}

		/// <summary>
		/// 	This method is used to update student details.
		/// </summary>
		/// <param name="asXml"></param>
		/// <param name="aiUpadatedBy"></param>
		public void Update(string asXml, int aiUpadatedBy, int aiStandardId, int aiDivisionId)
		{
            moSecondLanguageDC.Update(asXml, aiUpadatedBy, aiStandardId, aiDivisionId);
		}

         /// <summary>
        /// This method is used to check whether any exam is published.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public bool IsAnyExamPublished(int aiStandardId, int aiDivisionId)
        {
            return moSecondLanguageDC.IsAnyExamPublished(aiStandardId, aiDivisionId);
        }

		#endregion
	}
}