using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used to communicate with database to insert,update and select Job.
    /// </summary>
    public class JobDetailsBL
    {
        #region Data Member(s)

        private JobDetailsDC moJobDetailsDC;

		#endregion

		#region Constructor(s)

		/// <summary>
        /// Initializes a new instance of the <see cref="JobDetailsBL" /> class. 
		/// </summary>
		public JobDetailsBL()
		{
            moJobDetailsDC = new JobDetailsDC();
		}

		/// <summary>
        /// Initializes a new instance of the <see cref="JobDetailsBL" /> class. 
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiUpdatedById"></param>
        public JobDetailsBL(int aiSchoolId, int aiUpdatedById)
		{
           moJobDetailsDC = new JobDetailsDC(aiSchoolId, aiUpdatedById);
		}

		#endregion

		#region Method(s)

		/// <summary>
		/// This method is used to return all job details.
		/// </summary>
		/// <returns></returns>
        public List<JobDetails> GetAll()
        {
            return moJobDetailsDC.GetAll();
        }

        /// <summary>
        /// This method is used to retrive job details for particular ID.
        /// </summary>
        public JobDetails Get(int aiJobId)
        {
            return moJobDetailsDC.Get(aiJobId);
        }

		/// <summary>
		/// This method is used to insert/update job details. 
		/// </summary>		
		public void Save(JobDetails aoJobDetails)
		{
           moJobDetailsDC.Save(aoJobDetails);
		}

        /// <summary>
        /// This method is used to delete job details.
        /// </summary>
        /// <param name="aiIncomeTaxRangeId"></param>
        public void Delete(int aiJobId)
        {
            moJobDetailsDC.Delete(aiJobId);
        }

        /// <summary>
        /// This method is used to save selected jobs to be displayed on Career page.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SaveSelectedJob(string asXML)
        {
            moJobDetailsDC.SaveSelectedJob(asXML);
        }

        /// <summary>
        /// This method is used to get selected jobs to be displayed on Career page.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public List<JobDetails> GetSelectedJobDetails(int aiSchoolId)
        {
            return moJobDetailsDC.GetSelectedJobDetails(aiSchoolId);
        }
        
		#endregion
    }
}
