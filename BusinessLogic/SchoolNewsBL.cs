// File Name : SchoolNewsBL.cs
// Creator : Sunny
// Created Date : 21-feb-2014
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used to communicate with database to insert,update and select NEWS.
    /// </summary>
    public class SchoolNewsBL
    {
        #region Data Member(s)

        private SchoolNewsDC moSchoolNewsDC;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Initializes a new instance of the <see cref="RetirementNoticeBL" /> class. 
		/// </summary>
		public SchoolNewsBL()
		{
            this.moSchoolNewsDC = new SchoolNewsDC();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RetirementNoticeBL" /> class. 
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinYearId"></param>
		/// <param name="aiUpdatedById"></param>
		/// <param name="aiAcademicYearId"></param>
        public SchoolNewsBL(int aiSchoolId, int aiUpdatedById)
		{
            this.moSchoolNewsDC = new SchoolNewsDC(aiSchoolId, aiUpdatedById);
		}

		#endregion

		#region Method(s)

		/// <summary>
		/// This method is used to return all news details.
		/// </summary>
		/// <returns></returns>
        public List<NewsDetails> GetAll(int aiIsText)
        {
            return this.moSchoolNewsDC.GetAll(aiIsText);
        }

        ///// <summary>
        ///// This method is used to retrive news details for particular ID.
        ///// </summary>
        ///// <param name="aiIncomeTaxRangeId"></param>
        ///// <returns></returns>
        public NewsDetails Get(int aiNewsId)
        {
            return this.moSchoolNewsDC.Get(aiNewsId);
        }

		/// <summary>
		/// This method is used to insert/update news details. 
		/// </summary>		
		public void Save(NewsDetails aoRetirementNotice)
		{
            this.moSchoolNewsDC.Save(aoRetirementNotice);
		}

        /// <summary>
        /// This method returns Notice ID for supplied news Name
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asNoticeName"></param> 
        public static int GetIDByName(int aiSchoolId, string asNewsName)
        {
            return SchoolNewsDC.GetIDByName(aiSchoolId, asNewsName);
        }

        /// <summary>
        /// This method is used to delete news details.
        /// </summary>
        /// <param name="aiIncomeTaxRangeId"></param>
        public void Delete(int aiNewsId)
        {
            this.moSchoolNewsDC.Delete(aiNewsId);
        }

        /// <summary>
        /// This method is used to save selected news to be displayed on home page.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SaveSelectedNews(string asXML)
        {
            this.moSchoolNewsDC.SaveSelectedNews(asXML);
        }

        /// <summary>
        /// This method is used to get selected news to be displayed on home page.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public List<NewsDetails> GetSelectedNews(int aiSchoolId)
        {
            return this.moSchoolNewsDC.GetSelectedNews(aiSchoolId);
        }
        
		#endregion

    }
}
