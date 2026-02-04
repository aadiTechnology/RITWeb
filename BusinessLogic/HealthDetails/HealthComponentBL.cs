// File Name : HealthComponentBL.cs
// Creator : Sachin Wagh
// Created Date : 22-Nov-2018
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace BusinessLogic
{
    public class HealthComponentBL
    { 
        #region Data Member(s)
        private HealthComponentDC moHealthComponentDC;
        #endregion
        
        #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthComponentBL" /> class. 
        /// </summary>
        public HealthComponentBL()
        {
			this.moHealthComponentDC = new HealthComponentDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthComponentBL" /> class. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public HealthComponentBL(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moHealthComponentDC = new HealthComponentDC(aiSchoolId, aiFinYearId, aiAcademicYearId, aiUpdatedById);          
        }

        #endregion    
   
        #region Method(s)   
        /// <summary>
        /// This method is used to return all health component details.
        /// </summary>
        /// <returns></returns>
        public List<HealthComponent> GetAll(int aiHealthComponentId)
        {
            return this.moHealthComponentDC.GetAll(aiHealthComponentId);
        }          
        
        /// <summary>
        /// This method is used to retrive health component details for particular ID.
        /// </summary>
        /// <param name="aiHealthComponentId"></param>
        /// <returns></returns>
        public HealthComponent Get(int aiHealthComponentId)
        {
            List<HealthComponent> lstHealthComponent = GetAll(aiHealthComponentId);
            HealthComponent oHealthComponent = lstHealthComponent.Where(its => its.Id == aiHealthComponentId).FirstOrDefault();
            return oHealthComponent;
        }
        
        /// <summary>
        /// This method is used to insert/update health component details. 
        /// </summary>		
        public void Save(HealthComponent aoHealthComponent)
        {
            this.moHealthComponentDC.Save(aoHealthComponent);          
        }

        /// <summary>
        /// This method is used to delete health component details.
        /// </summary>		
        public void Delete(int aiHealthComponentId)
        {
            this.moHealthComponentDC.Delete(aiHealthComponentId);         
        }
        #endregion 
    }
}
