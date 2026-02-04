using System.Data;

namespace DataCommunicator
{
   public class ParentHealthDetailsDC
   {
       #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

       #endregion

       #region Constructor(s)

        public ParentHealthDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public ParentHealthDetailsDC()
        {
        }

       #endregion

        #region Public Method(s)

       /// <summary>
       /// This method is used to save parent health details.
       /// </summary>
       /// <param name="aiYearwiseStudentId"></param>
       /// <param name="asParentHealthDetailsXML"></param>
 
       public void Save(int aiYearwiseStudentId, string asParentHealthDetailsXML)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParentHealthDetailsXML", asParentHealthDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveParentHealthDetails");
            }
        }

       /// <summary>
       /// This method is used to get parent health details.
       /// </summary>
       /// <param name="aiYearwiseStudentId"></param>
       /// <returns></returns>
 
       public DataTable GetParentHealthDetails(int aiYearwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetParentHealthDetails");
            }
        }

       /// <summary>
       /// This method is used to submit parent details.
       /// </summary>
       /// <param name="aiYearwiseStudentDetails"></param>

       public void Submit(int aiYearwiseStudentDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentDetails, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitParentHealthDetails");
            }
        }

        #endregion
   }
}
