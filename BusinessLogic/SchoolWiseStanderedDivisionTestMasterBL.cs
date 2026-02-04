
using System;
using System.Data;
using System.Text;
using DataCommunicator;
using ProgressReportEntities;
using System.Collections.Generic;



namespace BusinessLogic
{


    public class SchoolWiseStanderedDivisionTestMasterBL
    {

        private SchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStruct moSchoolWiseStanderedDivisionTestMasterStruct;

        private SchoolWiseStanderedDivisionTestMasterDC moSchoolWiseStanderedDivisionTestMasterDC;

        public SchoolWiseStanderedDivisionTestMasterBL()
        {
            moSchoolWiseStanderedDivisionTestMasterDC = new SchoolWiseStanderedDivisionTestMasterDC();
        }

        public SchoolWiseStanderedDivisionTestMasterBL(int miStanderedDivisionTestId)
        {
            moSchoolWiseStanderedDivisionTestMasterDC = new SchoolWiseStanderedDivisionTestMasterDC(miStanderedDivisionTestId);
            moSchoolWiseStanderedDivisionTestMasterStruct = moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails;
        }

        public SchoolWiseStanderedDivisionTestMasterBL(int miSchoolID, int miAcademicYrID, int miStandardDivisionId)
        {
            moSchoolWiseStanderedDivisionTestMasterDC = new SchoolWiseStanderedDivisionTestMasterDC(miSchoolID, miAcademicYrID, miStandardDivisionId);
            moSchoolWiseStanderedDivisionTestMasterStruct = moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails;
        }

        public SchoolWiseStanderedDivisionTestMasterBL(int miSchoolID, int miAcademicYrID, int miStandardDivisionId, int miTestID)
        {
            moSchoolWiseStanderedDivisionTestMasterDC = new SchoolWiseStanderedDivisionTestMasterDC(miSchoolID, miAcademicYrID, miStandardDivisionId, miTestID);
            moSchoolWiseStanderedDivisionTestMasterStruct = moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails;
        }

        public List<PublishExamDependencyMaster> lstPublishExamDependencyMaster
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterDC.lstPublishExamDependencyMaster;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterDC.lstPublishExamDependencyMaster = value;
            }
        }

        public virtual int StanderedDivisionTest_Id
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.miStanderedDivisionTestId;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miStanderedDivisionTestId = value;
            }
        }

        public virtual int School_id
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.miSchoolid;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miSchoolid = value;
            }
        }

        public virtual int Standerd_division_Id
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.miStanderddivisionId;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miStanderddivisionId = value;
            }
        }

        public virtual int Acadmic_year_id
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.miAcadmicyearid;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miAcadmicyearid = value;
            }
        }

        public virtual int SchoolWise_Test_Id
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.miSchoolWiseTestId;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miSchoolWiseTestId = value;
            }
        }

        public virtual Char Is_Published
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.msIsPublished;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.msIsPublished = value;
            }
        }

        public virtual int Inserted_By_id 
        {            
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miInsertedByid = value;
            }
        }

        public virtual int StanderdId
        {
            get
            {
                return moSchoolWiseStanderedDivisionTestMasterStruct.miStanderdId;
            }
            set
            {
                moSchoolWiseStanderedDivisionTestMasterStruct.miStanderdId = value;
            }
        }

        public virtual int InsertSchoolWiseStanderedDivisionTestMaster()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            return moSchoolWiseStanderedDivisionTestMasterDC.InsertSchoolWiseStanderedDivisionTestMaster();
        }
        public void InsertSchoolWiseStandaredDivisionTestMaster()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.InsertSchoolWiseStandaredDivisionTestMaster();
        }

        /// <summary>
        /// This function is used to generate progress report for class.
        /// </summary>
        public virtual void PublishTestMarks()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.PublishTestMarks();
        }

        /// <summary>
        /// This function is used to insert student total test marks into SchoolWise_Student_Test_total_Marks
        /// </summary>
        public virtual void GenerateTestTotalMarks()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.GenerateTestTotalMarks();
        }

        public virtual DataSet GetTestAndSubjectToppers(int aiRankCount)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            return moSchoolWiseStanderedDivisionTestMasterDC.GetTestAndSubjectToppers(aiRankCount);
        }

        public virtual DataSet GetTestAndSubjectStdToppers(int aiStandard, int aiRankCount)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            return moSchoolWiseStanderedDivisionTestMasterDC.GetTestAndSubjectStdToppers(aiStandard,aiRankCount);
        }

        /// <summary>
        /// This method is used to get standard division id according to academic year id.
        /// </summary>
        /// <param name="miSchoolid"></param>
        /// <param name="miAcadmicyearid"></param>
        /// <param name="miStanderddivisionId"></param>
        /// <returns></returns>
        public static int GetStandardDivisionIdOfYear(int miSchoolid, int miAcadmicyearid, int miStudentId)
        {
            return SchoolWiseStanderedDivisionTestMasterDC.GetStandardDivisionIdOfYear(miSchoolid, miAcadmicyearid, miStudentId);
        }

        /// <summary>
        /// This method is used to get standard division id.
        /// </summary>
        /// <param name="miSchoolid"></param>
        /// <param name="miAcadmicyearid"></param>
        /// <param name="miStanderddivisionId"></param>
        /// <returns></returns>
        public static int GetStandardDivisionId(int miSchoolid, int miAcadmicyearid, int miStandardId)
        {
            return SchoolWiseStanderedDivisionTestMasterDC.GetStandardDivisionId(miSchoolid, miAcadmicyearid, miStandardId);
        }

        public static int GetStandardId(int miSchoolid, int miAcadmicyearid, int miStandardDivisionId)
        {
            return SchoolWiseStanderedDivisionTestMasterDC.GetStandardId(miSchoolid, miAcadmicyearid, miStandardDivisionId);
        }
       
        public virtual Boolean isAnyTestPublished(int miSchoolid, int miAcadmicyearid, int miStanderddivisionId)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            return moSchoolWiseStanderedDivisionTestMasterDC.isAnyTestPublished(miSchoolid, miAcadmicyearid, miStanderddivisionId);
        }

        public bool IsAnyTestPublishedForStudent(int aiSchoolID, int aiAcademicYrID, int aiStudentId, int aiStanderddivisionId)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            System.Data.DataSet oDataSet = moSchoolWiseStanderedDivisionTestMasterDC.IsAnyTestPublishedForStudent(aiSchoolID, aiAcademicYrID, aiStudentId, aiStanderddivisionId);
            if (oDataSet != null && oDataSet.Tables[0].Rows.Count > 0 && oDataSet.Tables[0].Rows[0][0] != DBNull.Value)
                return true;
            else
                return false;
        }
        
        public virtual Boolean isAllSchoolResultsPublished(int iSchoolID, int iAcademicYrID)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            System.Data.DataSet oDataSet = moSchoolWiseStanderedDivisionTestMasterDC.isAllSchoolResultsPublished(iSchoolID, iAcademicYrID);
            if (oDataSet != null && oDataSet.Tables[0].Rows.Count > 0 && oDataSet.Tables[0].Rows[0][0] != DBNull.Value)
            {
                if (Convert.ToInt32(oDataSet.Tables[0].Rows[0][0]) == 1)
                    return true;
            }
            return false;
        }

        public virtual Boolean IsAllResultsGeneratedForStdDiv()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            System.Data.DataSet oDataSet = moSchoolWiseStanderedDivisionTestMasterDC.IsAllResultsGeneratedForStdDiv();
            if (oDataSet != null && oDataSet.Tables[0].Rows.Count > 0 && oDataSet.Tables[0].Rows[0][0] != DBNull.Value)
            {
                if (Convert.ToInt32(oDataSet.Tables[0].Rows[0][0]) == 1)
                    return true;
            }
                return false;
        }

        public DataTable IsAtleastOneResultGeneratedForStdDiv()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            return moSchoolWiseStanderedDivisionTestMasterDC.IsAtleastOneResultGeneratedForStdDiv();
        }

        public virtual Boolean isAllResultsGenerated(int iSchoolID, int iAcademicYrID)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            DataSet oDataSet = moSchoolWiseStanderedDivisionTestMasterDC.isAllResultsGenerated(iSchoolID, iAcademicYrID);
            StringBuilder sErrMsg = new StringBuilder();
            sErrMsg.Append("Results are not genearated for classes : ");

            if ((oDataSet != null) && (oDataSet.Tables[0].Rows.Count > 0))
            {
                foreach (DataRow oDataRow in oDataSet.Tables[0].Rows)
                {
                    sErrMsg.Append(Convert.ToString(oDataRow["Standard_Name"]) + ", ");
                }

                string sError = string.Empty;
 
                if (sErrMsg.ToString().LastIndexOf(",") != -1)
                    sError = sErrMsg.ToString().Substring(0, sErrMsg.ToString().LastIndexOf(","));

                throw new Exceptions.ResultNotAvailableForOtherDiv(sError);
            }
            else
                return true;
        }

        /// <summary>
        /// This method is used to get all non configured standered division of a given standered
        /// </summary>
        /// <param name="aiStandard_Id"></param>
        /// <returns></returns>
        public virtual Boolean isAllResultsGeneratedForDivs(int aiStandard_Id)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            DataSet oDataSet = moSchoolWiseStanderedDivisionTestMasterDC.isAllResultsGeneratedForDivs(aiStandard_Id);
            StringBuilder sErrMsg = new StringBuilder();
            sErrMsg.Append("Results are not genearated for classes : ");

            if ((oDataSet != null) && (oDataSet.Tables[0].Rows.Count>0))
            {
                foreach (DataRow oDataRow in oDataSet.Tables[0].Rows)
                {
                    sErrMsg.Append(Convert.ToString(oDataRow[0]) + ", ");
                }

                string sError = string.Empty;
                if (sErrMsg.ToString().LastIndexOf(",") != -1)
                    sError = sErrMsg.ToString().Substring(0, sErrMsg.ToString().LastIndexOf(","));

                throw new Exceptions.ResultNotAvailableForOtherDiv(sError);
            }
            else
                return true;
        }
        
        /// <summary>
        /// This method is used to get all non configured standered division of a given standered and test
        /// </summary>
        /// <param name="aiStandard_Id"></param>
        /// <returns></returns>
        public virtual Boolean isTestPublishedForAllDivs(int aiStandard_Id)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            DataTable oDataTable = moSchoolWiseStanderedDivisionTestMasterDC.isTestPublishedForAllDivs(aiStandard_Id);
            StringBuilder sErrMsg = new StringBuilder();
            sErrMsg.Append("Test is not published for classes : ");

            if ((oDataTable != null) && (oDataTable.Rows.Count > 0))
            {
                foreach (DataRow oDataRow in oDataTable.Rows)
                {
                    sErrMsg.Append(Convert.ToString(oDataRow[0]) + ", ");
                }

                string sError = string.Empty; 

                if (sErrMsg.ToString().LastIndexOf(",") != -1)
                    sError = sErrMsg.ToString().Substring(0, sErrMsg.ToString().LastIndexOf(","));

                throw new Exceptions.ResultNotAvailableForOtherDiv(sError);
            }
            else
                return true;
        }

        /// <summary>
        /// This method is used to get all non configured standered division of a given standered and test
        /// </summary>
        /// <param name="aiStandard_Id"></param>
        /// <returns></returns>
        public virtual Boolean isTestPublishedForDivs(int aiStandard_Id, int aiStdDiv_Id)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            DataTable oDataTable = moSchoolWiseStanderedDivisionTestMasterDC.isTestPublishedForAllDivs(aiStandard_Id);
            StringBuilder sErrMsg = new StringBuilder();
            sErrMsg.Append("Test is not published for classes : ");
            if ((oDataTable != null) && (oDataTable.Rows.Count > 0))
            {
                DataRow[] oDataRow = oDataTable.Select("SchoolWise_Standard_Division_Id=" + aiStdDiv_Id.ToString());
                if (oDataRow.Length > 0)
                {

                    sErrMsg.Append(Convert.ToString(oDataRow[0][0]));                    
                    throw new Exceptions.ResultNotAvailableForOtherDiv(sErrMsg.ToString());
                }
                else
                    return true;
            }
            else
                return true;
        }

        public virtual Boolean isAllTestPublished()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            System.Data.DataSet oDataSet = moSchoolWiseStanderedDivisionTestMasterDC.isAllTestPublished();
            if (Convert.ToInt32(oDataSet.Tables[0].Rows[0][0])== 1)
                return true;
            else
                return false;
        }

        public virtual string AllUnpublishedTestForStdDivId()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            DataTable oDataTable = moSchoolWiseStanderedDivisionTestMasterDC.AllTestUnPublished();
            StringBuilder sUnpublishedTest = new StringBuilder();;

            for (int i = 0; i < oDataTable.Rows.Count; i++)
                sUnpublishedTest.Append(oDataTable.Rows[i][0].ToString() + ", ");

            string sReturn = string.Empty;
            if (sUnpublishedTest.Length > 0)
                sReturn = sUnpublishedTest.ToString().Substring(0, sUnpublishedTest.ToString().Length - 2);

            return sReturn;
        }

        public virtual void UpdateSchoolWiseStanderedDivisionTestMaster()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.UpdateSchoolWiseStanderedDivisionTestMaster();
        }

        public virtual void DeleteSchoolWiseStanderedDivisionTestMaster()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.DeleteSchoolWiseStanderedDivisionTestMaster();
        }

        public void UnPublishTest(string sUnPublishReason)
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.UnPublishTest(sUnPublishReason);
        }

        public void CheckGradeConfigurations()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            Boolean bIsConfig = moSchoolWiseStanderedDivisionTestMasterDC.CheckGradeConfigurations();
            if(!bIsConfig)
                throw new Exceptions.ResultNotAvailableForOtherDiv("Percentage Grades is not configured for this standard.");

            bIsConfig = moSchoolWiseStanderedDivisionTestMasterDC.CheckPassFailConfigurations();
            if (!bIsConfig)
                throw new Exceptions.ResultNotAvailableForOtherDiv("Fail Criteria is not configured for this standard.");
        }

        public bool IsPrePrimaryTeacher(int aiUserId, int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolWiseStanderedDivisionTestMasterDC.IsDependent(aiUserId, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to check the any exam is dependent on publish exam.
        /// </summary>
        public void CheckPublishExamDependency()
        {
            moSchoolWiseStanderedDivisionTestMasterDC.SchoolWiseStanderedDivisionTestMasterStructDetails = moSchoolWiseStanderedDivisionTestMasterStruct;
            moSchoolWiseStanderedDivisionTestMasterDC.CheckPublishExamDependency();
        }

        /// <summary>
        /// This method is used to check whether term exam is published.
        /// </summary>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public bool IsTermExamPublished(int miSchoolId, int miAcademicYearId, int aiStandardDivisionId, out string asStandardName)
        {
            return moSchoolWiseStanderedDivisionTestMasterDC.IsTermExamPublished(miSchoolId, miAcademicYearId, aiStandardDivisionId, out asStandardName);
        }

        /// <summary>
        /// This method is used to check whether Final exam is published.
        /// </summary>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public bool IsFinalResultPublished(int miSchoolId, int miAcademicYearId, int aiStandardDivisionId)
        {
            return moSchoolWiseStanderedDivisionTestMasterDC.IsFinalResultPublished(miSchoolId, miAcademicYearId, aiStandardDivisionId);
        }

        public DataTable GetStudentsLastAYDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            return moSchoolWiseStanderedDivisionTestMasterDC.GetStudentsLastAYDetails(aiSchoolId, aiAcademicYearId, aiStudentId);
        }
        
        public bool IsPrelimExamPublished(int aiStdDivId, int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolWiseStanderedDivisionTestMasterDC.IsPrelimExamPublished(aiStdDivId, aiSchoolId, aiAcademicYearId);
        }
    }
}
