using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using House;

namespace BusinessLogic
{
    public class HouseCofigurationBL
    {
        #region " Constants "

        #endregion " Constants "

        #region " Data Members "

        private HouseCofigurationDC moHouseCofigurationDC;

        #endregion " Data Members "

        #region " Constructors "
        
        public HouseCofigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moHouseCofigurationDC = new HouseCofigurationDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }
        public HouseCofigurationBL()
        {
            moHouseCofigurationDC = new HouseCofigurationDC();
        }
        #endregion " Constructors "

       

        #region " Public Methods "

        /// <summary>
        /// This method is used to get all House Details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<HouseConfiguration> GetAll(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex)
        {
            maximumRows = maximumRows + startRowIndex;
            return this.moHouseCofigurationDC.GetAll(sortExpression, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get count of Houses.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex)
        {
            return this.moHouseCofigurationDC.Count(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to Insert House Information.
        /// </summary>
        /// <param name="aoHouseConfiguration"></param>
        public void Insert(HouseConfiguration aoHouseConfiguration)
        {
            this.moHouseCofigurationDC.Insert(aoHouseConfiguration);
        }

        /// <summary>
        /// This method is used to get Single House Related Information.
        /// </summary>
        /// <param name="iHouseId"></param>
        /// <returns></returns>
        public HouseConfiguration Get(int iHouseId)
        {
          return  this.moHouseCofigurationDC.Get(iHouseId);
        }

        /// <summary>
        /// This method is used to Update House Cofiguration Details.
        /// </summary>
        /// <param name="aoHouseConfiguration"></param>
        public void Update(HouseConfiguration aoHouseConfiguration)
        {
            this.moHouseCofigurationDC.Update(aoHouseConfiguration);
        }

        /// <summary>
        /// This method is used to Delete House Details.
        /// </summary>
        /// <param name="aiHouseId"></param>
        /// <param name="amiSchoolId"></param>
        /// <param name="amiAcademicYearId"></param>
        /// <returns></returns>
        public int Delete(int aiHouseId)
        {
            return this.moHouseCofigurationDC.Delete(aiHouseId);
        }

        /// <summary>
        /// This method is used to update student House details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiUpadatedBy"></param>
        public void UpdateStudentHouseInformation(string asXml)
        {
            moHouseCofigurationDC.UpdateStudentHouseInformation(asXml);
        }

        #endregion









    }
}
