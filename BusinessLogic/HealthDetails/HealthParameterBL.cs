// File Name : HealthParameterBL.cs
// Creator :  Sachin Wagh
// Created Date : 10-12-2018
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace BusinessLogic
{
    public class HealthParameterBL  
    {
        #region Constant(s)

        #endregion

        #region Data Member(s)

        private HealthParameterDC moHealthParameterDC;   
 
        #endregion

         #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthParameterBL" /> class. 
        /// </summary>
        public HealthParameterBL()
        {
			this.moHealthParameterDC = new HealthParameterDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeTaxSlabsBL" /> class. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public HealthParameterBL(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moHealthParameterDC = new HealthParameterDC(aiSchoolId, aiFinYearId, aiAcademicYearId, aiUpdatedById);
          
        }

        #endregion    
   
        #region Method(s)
         
        /// <summary>
        /// This method is used to return all Health Parameter details.
        /// </summary>
        /// <returns></returns>
        public List<HealthParameter> GetAll(int aiHealthParameterId)
        {
            return this.moHealthParameterDC.GetAll(aiHealthParameterId);
        }              

        /// <summary>
        /// This method is used to retrive Health Parameter details for particular ID.
        /// </summary>
        /// <param name="aiIncomeTaxRangeId"></param>
        /// <returns></returns>
        public HealthParameter Get(int aiHealthParameterId)
        {
            List<HealthParameter> lstHealthParameter = GetAll(aiHealthParameterId);
            HealthParameter oHealthParameter = lstHealthParameter.Where(its => its.Id == aiHealthParameterId).FirstOrDefault();
            return oHealthParameter;
        }

        /// <summary>
        /// This method is used to insert/update Health Parameter details. 
        /// </summary>		
        public void Save(HealthParameter aoHealthParameter)
        {
            this.moHealthParameterDC.Save(aoHealthParameter);          
        }

        /// <summary>
        /// This method is used to delete Health Parameter details.
        /// </summary>		
        public void Delete(int aiHealthParameterId)
        {
            this.moHealthParameterDC.Delete(aiHealthParameterId);
         
        }  

        #endregion

    }
}
