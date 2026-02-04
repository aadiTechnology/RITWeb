using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using Utility;
using ProgressReportEntities;


namespace BusinessLogic 
{
    public class PrePrimaryProgressReportMonthsBL
    {

        #region DataMembers & Properties

        #region DataMembers

        private PrePrimaryProgressReportMonthsDC moPrePrimaryProgressReportMonthsDC;

        public PrePrimaryProgressReportMonthsBL()
        {
            moPrePrimaryProgressReportMonthsDC=new PrePrimaryProgressReportMonthsDC();           
        }

        #endregion

        #region Properties

        public PrePrimaryProgressReportMonth PrePrimaryProgressReportMonthsEntity
        {
            get
            {
                return moPrePrimaryProgressReportMonthsDC.PrePrimaryProgressReportMonthsEntity;
            }
            set
            {
                moPrePrimaryProgressReportMonthsDC.PrePrimaryProgressReportMonthsEntity = value;
            }
        }

        public PrePrimaryConfiguredMonthDetails PrePrimaryConfiguredMonthDetailsEntity
        {
            set { moPrePrimaryProgressReportMonthsDC.PrePrimaryConfiguredMonthDetailsEntity = value; }
        }

        public List<PrePrimaryConfiguredMonthDetails> PrePrimaryConfiguredMonthList
        {
            get{ return moPrePrimaryProgressReportMonthsDC.olstclsPrePrimaryProgressReportMonths; }
        }

        #endregion

        #endregion

        #region Public Method

        
        /// <summary>
        /// This function is used to get students list according to the applied filter.
        /// </summary>
        /// <returns></returns>
        public static List<PrePrimaryProgressReportMonth> GetMonthsList(int aiSchoolId, int aiAcademicYearId, int iStandaradId)
        {

            return PrePrimaryProgressReportMonthsDC.GetMonthsList(aiSchoolId, aiAcademicYearId, iStandaradId);
        }
       
        //public string CheckDependencies(List<PrePrimaryProgressReportMonth> aoPrePrimaryProgressReportMonths, Constants.ReferenceId oReferenceId, int aiAcademicYearId)
        //{
        //    //GenericReferenceList<PrePrimaryProgressReportMonth> objStdRefereces = new GenericReferenceList<PrePrimaryProgressReportMonth>(aoPrePrimaryProgressReportMonths, aiAcademicYearId);
        //    //return objStdRefereces.CheckDependenciesForList("PrePrimaryProgressReportMonthId", "MonthAbbreviation", "ConfigurationAction", oReferenceId, false);
        //}      

        #endregion

        public static void Save(string asMonthXML)
        {
            PrePrimaryProgressReportMonthsDC.Save(asMonthXML);
        }

        public static List<PrePrimaryProgressReportMonth> GetSavedMonthsList(int aiSchoolId, int aiAcademicYearId, int iStandaradId)
        {
            return PrePrimaryProgressReportMonthsDC.GetSavedMonthsList(aiSchoolId, aiAcademicYearId, iStandaradId);   
        }

        public static void UpdateSortOrder(string sXmlSortOrder)
        {
            PrePrimaryProgressReportMonthsDC.UpdateSortOrder(sXmlSortOrder); 
        }

        public static string CheckDependencies(string sSavedMonthsXML)
        {
            return PrePrimaryProgressReportMonthsDC.CheckDependencies(sSavedMonthsXML);
        }

        public void GetClasswiseMonthsList(int aiSchoolId, int aiAcademicYearId, int iStandaradId)
        {
            moPrePrimaryProgressReportMonthsDC.GetClasswiseMonthsList(aiSchoolId, aiAcademicYearId, iStandaradId);   
        }

        public static void UpdateStatusClass(int aischoolid,int aiacademicid, string sStatusDetails, bool abIsSubmit)
        {
            PrePrimaryProgressReportMonthsDC.UpdateStatusClass(aischoolid, aiacademicid, sStatusDetails, abIsSubmit);
        }

        public void UpdateStatusClass()
        {
            moPrePrimaryProgressReportMonthsDC.UpdateStatusClass();
        }

        public void UnpublishExam()
        {
            moPrePrimaryProgressReportMonthsDC.UnpublishExam();
        }

        public static List<PrePrimaryConfiguredMonthDetails> GetStudentWiseMonthsList(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStudentId)
        {
            return PrePrimaryProgressReportMonthsDC.GetStudentWiseMonthsList(aiSchoolId, aiAcademicYearId, aiStandardId, aiStudentId);
        }

    }
}
