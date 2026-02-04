
// Class Name       :- SchoolwiseTermConfigurationMasterBL
// Purpose          :- This class is used to manage SchoolwiseTermConfigurationMaster details.
// Date Of creation :- 2/15/2011
// Author Name      :- Vinod

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using SchoolEntities;
using TermEntities;


namespace BusinessLogic
{
    public class SchoolwiseTermConfigurationMasterBL
    {
        #region "Data Members"

        public SchoolwiseTermConfigurationDetails moSchoolwiseTermConfigurationDetails = null;
        public SchoolwiseTermConfigurationMasterDC moSchoolwiseTermConfigurationMasterDC = null;
        public StandardwiseAcademicYearDates oStandardwiseAcademicYearDates = new StandardwiseAcademicYearDates();
       
 
        #endregion "Data Members"

        #region "Constructors"

        public SchoolwiseTermConfigurationMasterBL()
        {
            moSchoolwiseTermConfigurationMasterDC = new SchoolwiseTermConfigurationMasterDC();
            moSchoolwiseTermConfigurationDetails = new SchoolwiseTermConfigurationDetails();
        }

        public SchoolwiseTermConfigurationMasterBL(int aiSchoolId, int aiAcademicYearId)
        {
            moSchoolwiseTermConfigurationMasterDC = new SchoolwiseTermConfigurationMasterDC(aiSchoolId, aiAcademicYearId);           
        }

        #endregion "Constructors"

        #region "Properties"

        public SchoolwiseTermConfigurationDetails SchoolwiseTermConfigurationDetails
        {
            set { moSchoolwiseTermConfigurationMasterDC.SchoolwiseTermConfigurationDetails = value; }
            get { return moSchoolwiseTermConfigurationMasterDC.SchoolwiseTermConfigurationDetails; }
        }
        public List<SchoolwiseTermConfigurationDetails> lstSchoolwiseTermConfigurationDetails
        {
            set { moSchoolwiseTermConfigurationMasterDC.olstSchoolwiseTermConfigurationDetails = value; }
            get { return moSchoolwiseTermConfigurationMasterDC.olstSchoolwiseTermConfigurationDetails; }
        }
        public List<StandardwiseAcademicYearDates> lstStandardwiseAcademicYearDates
        {
            set { moSchoolwiseTermConfigurationMasterDC.olstStandardwiseAcademicYearDates = value; }
            get { return moSchoolwiseTermConfigurationMasterDC.olstStandardwiseAcademicYearDates; }
        }

       

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all term details.
        /// </summary>
        public void GetAllTermDetails()
        {
            moSchoolwiseTermConfigurationMasterDC.GetAllTermDetails();
        }
     
        /// <summary>
        /// This method is used to  All Evaluation Periods
        /// </summary>
        public List<EvaluationPeriodDetails> GetAllEvaluationPeriods(int aiTestId)
        {
            return this.moSchoolwiseTermConfigurationMasterDC.GetAllEvaluationPeriods(aiTestId);
        }


        /// <summary>
        /// This method is used to Save, Update Term Configuraion details.
        /// </summary>
        /// <param name="asTermXML"></param>
        public int SaveSchoolwiseTermDetails(string asTermXML, int aiInsertedById, int aiOrigConfigId)
        {
            return moSchoolwiseTermConfigurationMasterDC.SaveSchoolwiseTermDetails(asTermXML, aiInsertedById, aiOrigConfigId);
        }
        /// <summary>
        /// This method is used to Save, Update Evatualtion Period Details.
        /// </summary>
        /// <param name="asXML"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public void InsertEvatualtionPeriodDetails(string asXML, int aiInsertedById, int aiTestId)
        {
            moSchoolwiseTermConfigurationMasterDC.InsertEvatualtionPeriodDetails(asXML, aiInsertedById, aiTestId);
        }
        /// <summary>
        ///This method is used to Copy Evaluation Period Details . 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="asTargetTestIds"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="?"></param>
        public void CopyEvaluationPeriods(int aiSchoolId, int aiAcademicYearId, int aiTestId, string asTargetTestIds, int aiInsertedById)
        {
            moSchoolwiseTermConfigurationMasterDC.CopyEvaluationPeriods(aiSchoolId, aiAcademicYearId, aiTestId, asTargetTestIds, aiInsertedById);
        }
      
     #endregion "Public Methods"

    }
}
