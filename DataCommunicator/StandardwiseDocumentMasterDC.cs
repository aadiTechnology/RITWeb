// Class Name       :- StandardwiseDocumentMasterDC
// Purpose          :- This class is used to manage StandardwiseDocumentMaster details.
// Date Of creation :- 3/15/2011
// Author Name      :- Vinod


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using SchoolEntities;
using DocumentEntity;

namespace DataCommunicator
{
    public class StandardwiseDocumentMasterDC
    {
        #region "Data Members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private StandardwiseDocument moStandardwiseDocument = null;
        private StudentDocument moStudentDocument = null;
        public List<StandardwiseDocument> lstStandardwiseDocument = new List<StandardwiseDocument>();
        public List<StudentDocument> lstStudentDocument = new List<StudentDocument>();

        #endregion "Data Members"

        #region "Constructors"

        public StandardwiseDocumentMasterDC()
        {
            moStandardwiseDocument = new StandardwiseDocument();
        }
        public StandardwiseDocumentMasterDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
            moStudentDocument = new StudentDocument();
        }
        public StandardwiseDocumentMasterDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            moStandardwiseDocument = new StandardwiseDocument();
        }

        #endregion "Constructors"

        #region "Properties"

        public StandardwiseDocument StandardwiseDocumentDetails
        {
            get { return moStandardwiseDocument; }
            set { moStandardwiseDocument = value; }
        }
        public StudentDocument StudentDocument
        {
            get { return moStudentDocument; }
            set { moStudentDocument = value; }
        }

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all the details of documents as per the standard.
        /// </summary>
        public DataTable GetAllStandardDetails()
        {
            StandardCollectionDC moStandardCollectionDC = new StandardCollectionDC(miSchoolId, miAcademicYearId);
            return moStandardCollectionDC.GetAssociatedStandards();
        }

        /// <summary>
        /// This method is used to get all the details of documents as per the standard.
        /// </summary>
        public List<StandardwiseDocument> GetAllDocumentsByStandard(int aiOriginalStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OriginalStandardId", aiOriginalStandardId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardwiseDocumentDetails"))
                return SetStandardDocumentDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to save standardwise document details.
        /// </summary>
        /// <param name="asXml"></param>
        public void SaveStandardwiseDocumentDetails(string asXml, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentXML", asXml, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStandardwiseDocumentDetails");
            };
        }

        /// <summary>
        /// This method is used to set values to property.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public List<StandardwiseDocument> SetStandardDocumentDetails(SqlDataReader aoSqlDataReader)
        {
            StandardwiseDocument oStandardwiseDocument = null;
            while (aoSqlDataReader.Read())
            {
                oStandardwiseDocument = new StandardwiseDocument
                {
                    StandardwiseDocumentId = Convert.ToInt32(aoSqlDataReader["StandardwiseDocumentId"]),
                    DocumentName = Convert.ToString(aoSqlDataReader["DocumentName"]),
                    OriginalStandardId = Convert.ToInt32(aoSqlDataReader["Original_Standard_Id"]),
                    OriginalDocumentId = Convert.ToInt32(aoSqlDataReader["Original_Document_Id"]),
                    IsContinue = Convert.ToBoolean(aoSqlDataReader["IsContinue"]),
                    SchoolId = Convert.ToInt32(aoSqlDataReader["SchoolId"]),
                    Is_Deleted = Convert.ToInt32(aoSqlDataReader["Is_Deleted"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                };
                lstStandardwiseDocument.Add(oStandardwiseDocument);
            }
            return lstStandardwiseDocument;
        }

        /****************** StudentUI.aspx: Student need to submit documents.****************************/

        /// <summary>
        /// This method is sed to get documentdetails which are required for admission.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentDocument> GetAdmissionDocumentDetails(int aiStandardId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardwiseStudentDocumentDetails"))
                return GetDocumentDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to set dcoument details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<StudentDocument> GetDocumentDetails(SqlDataReader aoSqlDataReader)
        {
            StudentDocument oStudentDocument = null;
            while (aoSqlDataReader.Read())
            {
                oStudentDocument = new StudentDocument
                {
                    StandardwiseDocumentId = Convert.ToInt32(aoSqlDataReader["StandardwiseDocumentId"]),
                    StudentDocumentId = Convert.ToInt32(aoSqlDataReader["StudentDocumentId"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                    IsApplicable = Convert.ToBoolean(aoSqlDataReader["IsApplicable"]),
                    DocumentName = Convert.ToString(aoSqlDataReader["DocumentName"]),
					DocumentCount = Convert.ToInt32(aoSqlDataReader["DocumentCount"]),
                    IsSubmissionMandatory = Convert.ToBoolean(aoSqlDataReader["IsSubmissionMandatory"]),
                };
                lstStudentDocument.Add(oStudentDocument);
            }
            return lstStudentDocument;
        }

        /// <summary>
        /// This method is used to save student submitted document details.
        /// </summary>
        /// <param name="asDocXML"></param>
        public void SaveSubmittedDocuments(string asDocXML, int aiStudentId, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SubmittedDocumentXML", asDocXML, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStudentSubmittedDocumentDetails");
            };
        }


        #endregion "Public Methods"
    }


}
