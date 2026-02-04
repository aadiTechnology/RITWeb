using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Xml;
using DataCommunicator;
using MasterEntities;
using Utility;
using SchoolEntities.Admin;

namespace BusinessLogic
{
	public class StandardMasterBL
	{
		#region DataMembers and properties

		#region Data members

		private StandardMasterDC.StandardMasterStruct moStandardMasterStruct;
		private StandardMasterDC moStandardMasterDC = new StandardMasterDC();
		private Collection<DivisionMasterBL> moDivisionCollectionBL;
		private Collection<SubjectMasterBL> moSubjectCollectionBL;
		private Collection<SchoolwiseStandardTestMasterBL> moSchoolwiseStandardTestMasterBL;
		private Collection<SchoolwiseStandardFeeTypeMasterBL> moSchoolwiseStandardFeeTypeMasterBL;
		private Collection<LecturesPerStandardSubjectWeekBL> moLecturesPerStandardSubjectWeekBL;
		private Constants.Action eAction;

		#endregion

		#region Properties

		public int StandardId
		{
			get { return moStandardMasterStruct.miStandardId; }
			set { moStandardMasterStruct.miStandardId = value; }
		}

		public int AcademicYearId
		{
			get { return moStandardMasterStruct.miAcademicYearId; }
			set { moStandardMasterStruct.miAcademicYearId = value; }
		}

		public string StandardName
		{
			get { return moStandardMasterStruct.msStandardName; }
			set { moStandardMasterStruct.msStandardName = value; }
		}

		public int OriginalStandardId
		{
			get { return moStandardMasterStruct.miOriginalStandardId; }
			set { moStandardMasterStruct.miOriginalStandardId = value; }
		}

        public int NextOriginalStandardId
        {
            get { return moStandardMasterStruct.miNextOriginalStandardId; }
            set { moStandardMasterStruct.miNextOriginalStandardId = value; }
        }

		public int SchoolId
		{
			get { return moStandardMasterStruct.miSchoolId; }
			set { moStandardMasterStruct.miSchoolId = value; }
		}

		public string IsPrePrimary
		{
			get { return moStandardMasterStruct.msIsPrePrimary; }
			set { moStandardMasterStruct.msIsPrePrimary = value; }
		}

		public int SectionId
		{
			get { return moStandardMasterStruct.miSectionId; }
			set { moStandardMasterStruct.miSectionId = value; }
		}

        public int StudentStrength
        {
            get { return moStandardMasterStruct.miStudentStrength; }
            set { moStandardMasterStruct.miStudentStrength = value; }
        }

        public int Threshold
        {
            get { return moStandardMasterStruct.miThreshold; }
            set { moStandardMasterStruct.miThreshold = value; }
        }

		public string InsertedByid
		{
			get { return moStandardMasterStruct.msInsertedByid; }
			set { moStandardMasterStruct.msInsertedByid = value; }
		}

		public string UpdatedById
		{
			get { return moStandardMasterStruct.msUpdatedById; }
			set { moStandardMasterStruct.msUpdatedById = value; }
		}

		public System.DateTime InsertDate
		{
			get { return moStandardMasterStruct.mdtInsertedDate; }
			set { moStandardMasterStruct.mdtInsertedDate = value; }
		}

		public System.DateTime UpdateDate
		{
			get { return moStandardMasterStruct.mdtUpdatedDate; }
			set { moStandardMasterStruct.mdtUpdatedDate = value; }
		}

		public Collection<DivisionMasterBL> DivisionCollection
		{
			get { return moDivisionCollectionBL; }
			set { moDivisionCollectionBL = value; }
		}

		public Collection<SubjectMasterBL> SubjectCollection
		{
			get { return moSubjectCollectionBL; }
			set { moSubjectCollectionBL = value; }
		}

		public Collection<SchoolwiseStandardTestMasterBL> TestCollection
		{
			get { return moSchoolwiseStandardTestMasterBL; }
			set { moSchoolwiseStandardTestMasterBL = value; }
		}

		public Collection<SchoolwiseStandardFeeTypeMasterBL> FeeTypeCollection
		{
			get { return moSchoolwiseStandardFeeTypeMasterBL; }
			set { moSchoolwiseStandardFeeTypeMasterBL = value; }
		}

		public Collection<LecturesPerStandardSubjectWeekBL> LectureCountCollection
		{
			get { return moLecturesPerStandardSubjectWeekBL; }
			set { moLecturesPerStandardSubjectWeekBL = value; }
		}

		public Constants.Action ConfigurationAction
		{
			get { return eAction; }
			set { eAction = value; }
		}

		#endregion

		#endregion

		#region Constructors

		public StandardMasterBL()
		{
		}

		/*public StandardMasterBL(int aiId)
        {

            StandardMasterDC moStandardMasterDC = new StandardMasterDC(aiId);

        }*/

		#endregion

		#region Public Methods

		public string GetInsertStatementforStandard()
		{
			moStandardMasterDC.StandardMasterStructDetails = moStandardMasterStruct;
			return moStandardMasterDC.GetInsertStatementforStandard();
		}

		public string GetInsertStmtforStdCautionMoney(int iDefaultCautionMoney)
		{
			moStandardMasterDC.StandardMasterStructDetails = moStandardMasterStruct;
			return moStandardMasterDC.GetInsertStmtforStdCautionMoney(iDefaultCautionMoney);
		}

		public string GetUpdateStatementforStandard()
		{
			moStandardMasterDC.StandardMasterStructDetails = moStandardMasterStruct;
			return moStandardMasterDC.GetUpdateStatementforStandard();
		}

		public string GetDeleteStatementforStandard()
		{
			moStandardMasterDC.StandardMasterStructDetails = moStandardMasterStruct;
			return moStandardMasterDC.GetDeleteStatementforStandard();
		}

		/// <summary>
		/// 	This method is used to get sandards which are associated to assessments.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYeaId"> </param>
		/// <param name="aiTeacherId"> </param>
		/// <returns> </returns>
		public static List<StandardMaster> GetStandardsAssociatedToAssessments(int aiSchoolId, int aiAcademicYeaId, int aiTeacherId)
		{
			return StandardMasterDC.GetStandardsAssociatedToAssessments(aiSchoolId, aiAcademicYeaId, aiTeacherId);
		}

		public static int GetProgressReportForStandard(int aiSchoolId, int aiAcademicYeaId, int aiTeacherId)
		{
			return StandardMasterDC.GetProgressReportForStandard(aiSchoolId, aiAcademicYeaId, aiTeacherId);
		}

		/// <summary>
		///		Gets the standards for which marks are given, but only grades are displayed on the report.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public static List<StandardMaster> GetStandardsWithOnlyGradeSetting(int aiSchoolId, int aiAcademicYearId)
		{
			return StandardMasterDC.GetStandardsWithOnlyGradeSetting(aiSchoolId, aiAcademicYearId);
		}

        /// <summary>
        /// This is used to set the standard name on page load
        /// </summary>
        /// <param name="sStdID"></param>
        /// <param name="sSchoolID"></param>
        /// <param name="sAcadID"></param>
        /// <returns></returns>
        public static DataTable GetStandardDetails(string sStdID, string sSchoolID, string asAcademicYearId)
        {
            return StandardMasterDC.GetStandardDetails(sStdID, sSchoolID, asAcademicYearId);
        }

		#endregion

        public static bool IsGradingStandard(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
          return  StandardMasterDC.IsGradingStandard(aiSchoolId, aiAcademicYearId, aiStandardId);
        }
    }

	public class StandardCollectionBL : BusinessLogicBaseBL
	{
		#region DataMembers

		private StandardCollectionDC moStandardCollectionDC = null;
		private int miSchoolId;

		#endregion

		#region Constructor

		public StandardCollectionBL(int aiSchoolId)
		{
			moStandardCollectionDC = new StandardCollectionDC(aiSchoolId);
			miSchoolId = aiSchoolId;
		}

		public StandardCollectionBL(int aiSchoolId, int aiAcademicYearId)
		{
			moStandardCollectionDC = new StandardCollectionDC(aiSchoolId, aiAcademicYearId);
			miSchoolId = aiSchoolId;
		}

		#endregion

		#region Public Methods

		public DataTable GetAllStandards()
		{
			return moStandardCollectionDC.GetAllStandards();
		}

        /// <summary>
        /// This method is used to get all Fee Types.
        /// </summary>
        public DataTable GetAllFeeTypes()
        {
            return moStandardCollectionDC.GetAllFeeTypes();
        }

        /// <summary>
        /// This method is used to get all Fee Types for Challan Import.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStandardDivisionId"></param>
        public DataTable GetAllFeeTypesForChallanImport(int aiAcademicYearId, int aiStandardId, int aiStandardDivisionId)
        {
            return moStandardCollectionDC.GetAllFeeTypesForChallanImport(aiAcademicYearId, aiStandardId, aiStandardDivisionId);
        }

        /// <summary>
        /// This method is used to get all payable for of respective fee type.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiOriginalFeeTypeId"></param>
        public DataTable GetAllPayableforChallan(int aiAcademicYearId, int aiStandardId, int aiOriginalFeeTypeId)
        {
            return moStandardCollectionDC.GetAllPayableforChallan(aiAcademicYearId, aiStandardId, aiOriginalFeeTypeId);
        }


		public DataTable GetAllStandardsForFee(int aiStandardID)
		{
			return moStandardCollectionDC.GetAllStandardsForFee(aiStandardID);
		}

		public DataTable GetAssociatedStandards()
		{
			return moStandardCollectionDC.GetAssociatedStandards();
		}

        public DataTable GetAssociatedStandardsForEnquiry(int aiAcmissionForId)
        {
            return moStandardCollectionDC.GetAssociatedStandardsForEnquiry(aiAcmissionForId);
        }

        public DataTable GetAdmissionForCategories()
        {
            return moStandardCollectionDC.GetAdmissionForCategories();
        }

        public DataTable GetAssociatedStandardsForSiblingDetails()
        {
            return moStandardCollectionDC.GetAssociatedStandardsForSiblingDetails();
        }

        public DataTable GetAssociatedStandardsForHealth()
        {
            return moStandardCollectionDC.GetAssociatedStandardsForHealth();
        }

        /// <summary>
        /// This method is used to get all standard division details.
        /// </summary>
        public DataSet GetAllStandardDivisionDetails()
        { 
            return moStandardCollectionDC.GetAllStandardDivisionDetails();
        }

        public List<StandardDivisionMaster> GetAllClasses()
        {
            return moStandardCollectionDC.GetAllClasses();
        }

        public DataTable GetAssociatedStandardsForHouse()
        {
            return moStandardCollectionDC.GetAssociatedStandardsForHouse();
        }

		public DataTable GetPrePrimaryStandards()
		{
			return moStandardCollectionDC.GetPrePrimaryStandards();
		}

		public static List<StandardMaster> GetAll(int aiSchoolId, int aiAcademicYearId)
		{
			return StandardCollectionDC.GetAll(aiSchoolId, aiAcademicYearId);
		}

		public DataTable GetConfiguredPrePrimaryStandards()
		{
			return moStandardCollectionDC.GetConfiguredPrePrimaryStandards();
		}

		public List<StandardMaster> GetStandardsForExamConfiguration(bool abIsXseed)
		{
			return moStandardCollectionDC.GetStandardsForExamConfiguration(abIsXseed);
		}

        /// <summary>
        /// This method is used to standard details for which exam configuration is not done.
        /// </summary>
        /// <param name="abIsXseed"></param>
        /// <returns></returns>
        public List<StandardMaster> GetStandardsForStandardwiseAssessment()
        {
            return moStandardCollectionDC.GetStandardsForStandardwiseAssessment();
        }

        /// <summary>
        /// This method is used to save standards for grading system.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SaveStandardsForGradingSystem(string asStandardIds)
        {
            this.moStandardCollectionDC.SaveStandardsForGradingSystem(asStandardIds);
        }

        /// <summary>
        /// This method is used to return all standards which are available or not available for grading system.
        /// </summary>
        /// <param name="abIsXseed"></param>
        /// <returns></returns>
        public List<StandardMaster> GetStandardsForGradingSystem()
        {
            return moStandardCollectionDC.GetStandardsForGradingSystem();
        }

		public DataTable GetAnualToppersStandards()
		{
			return moStandardCollectionDC.GetAnualToppersStandards();
		}

        public List<StandardMaster> GetExamConfiguredStandards()
        {
            return moStandardCollectionDC.GetStandardsForExamConfiguration();
        }

		/// <summary>
		/// 	This method calls a function to check the RI dependencies for standards that are to be deleted
		/// </summary>
		/// <param name="aoStandards"> </param>
		/// <param name="aiAcadId"> </param>
		/// <returns> </returns>
		private string CheckDependenciesForStds(Collection<StandardMasterBL> aoStandards, int aiAcadId)
		{
			//get the id and name of the standards to be deleted into hashtable.
			var objStdRefereces = new GenericReferenceList<StandardMasterBL>(aoStandards, aiAcadId);
			return objStdRefereces.CheckDependencies("StandardId", "StandardName", "ConfigurationAction", Constants.ReferenceId.Standard, false);
		}

		/// <summary>
		/// </summary>
		/// <param name="aoStandards"> </param>
        public void UpdateStandards(Collection<StandardMasterBL> aoStandards, int aiAcadId, int iDefaultCautionMoney, string adtStartDate, string adtEndDate)
		{
			string sMessage = CheckDependenciesForStds(aoStandards, aiAcadId);
			if (string.IsNullOrEmpty(sMessage))
			{
				IEnumerator oIEnum = aoStandards.GetEnumerator();
				var oArrayList = new ArrayList();
				while (oIEnum.MoveNext())
				{
					var oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
					switch (oStandardMasterBL.ConfigurationAction)
					{
						case Constants.Action.Insert:
							oArrayList.Add(oStandardMasterBL.GetInsertStatementforStandard());
							oArrayList.Add(GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY));
							oArrayList.Add(oStandardMasterBL.GetInsertStmtforStdCautionMoney(iDefaultCautionMoney));
							oArrayList.Add("EXEC usp_InsertStandardwiseAcademicYear @StandardwiseAcademicYearXML=N'" + GetStandardWiseAcademicYearXML(aiAcadId, adtStartDate, adtEndDate, oStandardMasterBL.InsertedByid.ToInt()) + "'");
							break;
						case Constants.Action.Update:
							oArrayList.Add(oStandardMasterBL.GetUpdateStatementforStandard());
							break;
						case Constants.Action.Delete:
							oArrayList.Add(oStandardMasterBL.GetDeleteStatementforStandard());
							break;
					}
				}
				moStandardCollectionDC.UpdateStandards(oArrayList);
			}
			else
				throw new Exceptions.ReferenceExceptions(sMessage);
		}

		/// <summary>
		/// 	This method is used to get xml for standard wise academic year.
		/// </summary>
		/// <param name="aiAcademicYearId"> </param>
		/// <param name="adtStartDate"> </param>
		/// <param name="adtEndDate"> </param>
		/// <param name="aiInsertedById"> </param>
		/// <returns> </returns>
        private string GetStandardWiseAcademicYearXML(int aiAcademicYearId, string adtStartDate, string adtEndDate, int aiInsertedById)
		{
			const string S_ELEMENT = "element";

			string sAttribute;
			var oDoc = new XmlDocument();
			XmlElement oElement = oDoc.CreateElement("StandardwiseAcademicYear");
			XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StandardwiseAcademicYear", "");
			XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "StandardwiseAcademicYear", "");

			sAttribute = "StandardwiseAcademicYearId";
			XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = Constants.S_ZERO;
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "StandardId";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = Constants.S_LAST_INSERTED_P_KEY;
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "StartDate";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = adtStartDate.ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "EndDate";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = adtEndDate.ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "ReopningDate";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = adtStartDate.ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "SchoolId";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = miSchoolId.ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "AcademicYearId";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = aiAcademicYearId.ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "InsertedById";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = aiInsertedById.ToString();
			oXMLNode.Attributes.Append(oAttr);

			oXmlRootNode.AppendChild(oXMLNode);

			oElement.AppendChild(oXmlRootNode);
			return oElement.InnerXml;
		}

		public void UpdateStandardDivisions(Collection<StandardMasterBL> aoStandards)
		{
			IEnumerator oIEnum = aoStandards.GetEnumerator();
			var oArrayList = new ArrayList();
			while (oIEnum.MoveNext())
			{
				var oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
				Collection<DivisionMasterBL> oDivisions = oStandardMasterBL.DivisionCollection;
				IEnumerator oIEnumDivisions = oDivisions.GetEnumerator();
				while (oIEnumDivisions.MoveNext())
				{
					var oDivisionMasterBL = (DivisionMasterBL)oIEnumDivisions.Current;
					switch (oDivisionMasterBL.ConfigurationAction)
					{
						case Constants.Action.Insert:
							oArrayList.Add(oDivisionMasterBL.GetInsertStatementForStandardDivision());
							break;

						case Constants.Action.Delete:
							oArrayList.Add(oDivisionMasterBL.GetDeleteStatementForStandardDivision());
							break;

						case Constants.Action.Update:
							oArrayList.Add(oDivisionMasterBL.GetUpdateStatementForStandardDivision());
							break;
					}
				}
			}
			moStandardCollectionDC.UpdateStandardDivisions(oArrayList);
		}

		public void UpdateStandardSubjects(Collection<StandardMasterBL> aoStandards)
		{
			IEnumerator oIEnum = aoStandards.GetEnumerator();
			var oArrayList = new ArrayList();
			while (oIEnum.MoveNext())
			{
				var oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
				Collection<SubjectMasterBL> oSubjects = oStandardMasterBL.SubjectCollection;
				IEnumerator oIEnumSubjects = oSubjects.GetEnumerator();
				while (oIEnumSubjects.MoveNext())
				{
					var oSubjectMasterBL = (SubjectMasterBL)oIEnumSubjects.Current;
					switch (oSubjectMasterBL.ConfigurationAction)
					{
						case Constants.Action.Insert:
							oArrayList.Add(oSubjectMasterBL.GetInsertStatementForStandardSubjects());
							break;

						case Constants.Action.Delete:
							oArrayList.Add(oSubjectMasterBL.GetDeleteStatementForStandardSubjects());
							break;
					}
				}
			}

			moStandardCollectionDC.UpdateStandardSubjects(oArrayList);
		}

		/// <summary>
		/// 	This method is used to update SchoolwiseStandardTestMaster table (i.e.For Inserting selected tests to particular standard and For Deleting unselected tests of a particular standard.)
		/// </summary>
		public void UpdateStandardTests(Collection<StandardMasterBL> aoStandards)
		{
			IEnumerator oIEnum = aoStandards.GetEnumerator();
			var oArrayList = new ArrayList();
			while (oIEnum.MoveNext())
			{
				var oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
				Collection<SchoolwiseStandardTestMasterBL> oTests = oStandardMasterBL.TestCollection;
				IEnumerator oIEnumTests = oTests.GetEnumerator();
				while (oIEnumTests.MoveNext())
				{
					var oSchoolwiseStandardTestMasterBL = (SchoolwiseStandardTestMasterBL)oIEnumTests.Current;
					switch (oSchoolwiseStandardTestMasterBL.ConfigurationAction)
					{
						case Constants.Action.Insert:
							oArrayList.Add(oSchoolwiseStandardTestMasterBL.InsertSchoolwiseStandardTestMaster());
							break;

						case Constants.Action.Delete:
							oArrayList.Add(oSchoolwiseStandardTestMasterBL.DeleteSchoolwiseStandardTestMaster());
							break;
					}
				}
			}

			moStandardCollectionDC.UpdateStandardTests(oArrayList);
		}

		/// <summary>
		/// 	This method is used to update SchoolwiseStandardFeeTypeMaster table (i.e.For Inserting selected fee types to particular standard and For Deleting unselected fee types of a particular standard.)
		/// </summary>
		public void UpdateStandardFeeTypes(Collection<StandardMasterBL> aoStandards)
		{
			IEnumerator oIEnum = aoStandards.GetEnumerator();
			var oArrayList = new ArrayList();
			while (oIEnum.MoveNext())
			{
				var oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
				Collection<SchoolwiseStandardFeeTypeMasterBL> oFeeTypes = oStandardMasterBL.FeeTypeCollection;
				IEnumerator oIEnumFeeTypes = oFeeTypes.GetEnumerator();
				while (oIEnumFeeTypes.MoveNext())
				{
					var oSchoolwiseStandardFeeTypeMasterBL = (SchoolwiseStandardFeeTypeMasterBL)oIEnumFeeTypes.Current;
					switch (oSchoolwiseStandardFeeTypeMasterBL.ConfigurationAction)
					{
                        case Constants.Action.Insert:
                                oArrayList.Add(oSchoolwiseStandardFeeTypeMasterBL.InsertSchoolwiseStandardFeeTypeMaster());
                            break;

                        case Constants.Action.Update:
                            oArrayList.Add(oSchoolwiseStandardFeeTypeMasterBL.UpdateStandardFeeTypeMaster());
                            break;

						case Constants.Action.Delete:
							oArrayList.Add(oSchoolwiseStandardFeeTypeMasterBL.DeleteSchoolwiseStandardFeeTypeMaster());
							break;
					}
				}
			}
			moStandardCollectionDC.UpdateStandardFeeTypes(oArrayList);
		}

		public void UpdateLectureCount(Collection<StandardMasterBL> aoStandards, Hashtable oHash, int aiAcadYrId)
		{
			IEnumerator oIEnum = aoStandards.GetEnumerator();
			var oArrayList = new ArrayList();
			LecturesPerStandardSubjectWeekBL.CheckDependencies(oHash, aiAcadYrId);
			while (oIEnum.MoveNext())
			{
				var oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
				Collection<LecturesPerStandardSubjectWeekBL> oLecCount = oStandardMasterBL.LectureCountCollection;
				IEnumerator oIEnumFeeTypes = oLecCount.GetEnumerator();
				while (oIEnumFeeTypes.MoveNext())
				{
					var oLecturesPerStandardSubjectWeekBL = (LecturesPerStandardSubjectWeekBL)oIEnumFeeTypes.Current;
					switch (oLecturesPerStandardSubjectWeekBL.ConfigurationAction)
					{
						case Constants.Action.Insert:
							oArrayList.Add(oLecturesPerStandardSubjectWeekBL.InsertLecturesPerStandardSubjectWeek());
							break;

						case Constants.Action.Update:
							oArrayList.Add(oLecturesPerStandardSubjectWeekBL.UpdateLecturesPerStandardSubjectWeek());
							break;
					}
				}
			}
			moStandardCollectionDC.UpdateLectureCount(oArrayList);
		}

        /// <summary>
        /// This method is used to return classes wise student count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<StudentStrengthDetails> GetClasseswiseStudentCountDetails(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            return moStandardCollectionDC.GetClasseswiseStudentCountDetails(aiSchoolId, aiAcademicYearId, aiUserId);
        }

		#endregion
	}
}