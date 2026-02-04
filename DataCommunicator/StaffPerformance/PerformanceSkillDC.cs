// Class Name       :- PerformanceGradeDC
// Purpose          :- This class is used to manage Performance Grade details.
// Date Of creation :- 15-Sept-2013
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StaffPerformanceEntity;


namespace DataCommunicator 
{    
    public class PerformanceSkillDC
    {
        #region "Data Members"

        private int miSchoolId;
        private int miUpdatedById;

        #endregion "Data Members"

        #region "Constructors"

        public PerformanceSkillDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;            
        }

        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all grade details.
        /// </summary>
        /// <returns></returns>
        public List<PerformanceSkill> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPerformanceSkill"))
                    return this.FillSkillDetails(oSqlDataReader);
            }                         
        }

        /// <summary>
        /// This method is used to get Input Types.
        /// </summary>
        /// <returns></returns>
        public  List<InputType> GetInputTypes()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetInputTypeDetails"))
                    return this.FillInputTypeDetails(oSqlDataReader);
            }
        }

        public List<InputType> FillInputTypeDetails(SqlDataReader aoSqlDataReader)
        {
            List<InputType> lstInputTypeDetails = new List<InputType>();
            InputType oInputType = null;
            while (aoSqlDataReader.Read())
            {
                oInputType = new InputType
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"])
                };
                lstInputTypeDetails.Add(oInputType);

            }
            return lstInputTypeDetails;
        }

        public List<FormType> GetFormTypeDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFormTypeDetails"))
                    return this.FillFormTypeDetails(oSqlDataReader);
            }
        }

        public List<FormType> FillFormTypeDetails(SqlDataReader aoSqlDataReader)
        {
            List<FormType> lstFormTypeDetails = new List<FormType>();
            FormType oFormType = null;
            while (aoSqlDataReader.Read())
            {
                oFormType = new FormType
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    name = Convert.ToString(aoSqlDataReader["Name"])
                };
                lstFormTypeDetails.Add(oFormType);
            
            }
            return lstFormTypeDetails;
        }


        /// <summary>
        /// This method is used to set values to property.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public List<PerformanceSkill> FillSkillDetails(SqlDataReader aoSqlDataReader)
        {
            List<PerformanceSkill> lstGradeDetails = new List<PerformanceSkill>();
            PerformanceSkill oPerformanceGrade = null;
            while (aoSqlDataReader.Read())
            {
                oPerformanceGrade = new PerformanceSkill
                {
                    SkillId = Convert.ToInt32(aoSqlDataReader["Id"]),
                    SkillName = Convert.ToString(aoSqlDataReader["Name"]),
                    OriginalSkillId = Convert.ToInt32(aoSqlDataReader["OriginalSkillId"]),
                    SchoolId = Convert.ToInt32(aoSqlDataReader["SchoolId"]),
                    IsDeleted = Convert.ToBoolean(aoSqlDataReader["IsDeleted"]),                    
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    InputTypeId = Convert.ToInt32(aoSqlDataReader["InputTypeId"]),
                    IsEditableToAll = Convert.ToBoolean(aoSqlDataReader["IsEditableToAll"])
                };
                                
                lstGradeDetails.Add(oPerformanceGrade);
            }

            return lstGradeDetails;
        }

        /// <summary>
        /// This method is used to Insert and Update grade details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
        public void Insert(string asXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillXML", asXml, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePerformanceSkill");
            };
        }
        
        #endregion
    }
}
