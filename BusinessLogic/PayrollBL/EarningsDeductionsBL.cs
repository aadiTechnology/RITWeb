// Class Name       :- EarningsDeductionsBL
// Purpose          :- This class is used to manage EarningsDeductions details.
// Date Of creation :- 11/2/2009
// Author Name      :- Sachin

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using DataCommunicator;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    public class EarningsDeductionsBL
    {
        #region Constants

        private const int I_ORIGINAL_PROFESSIONAL_TAX_ID = 16;
        private const string S_LEAVE_DEDUCTED = "Leave Deducted ";

        #endregion

        #region Data Member(s)

        private EarningsDeductions moEarningsDeductions;        
        private EarningsDeductionsDC moEarningsDeductionsDC;
        private SalaryDifferenceBL moSalaryDifferenceBL;        
        private List<StaffGroupsEarningDeductionAssociation> mlstStaffGroupsEarningDeductionAssociations;
        private DataTable moDTSalaryDetails;

        #endregion

        #region Constructor

        public EarningsDeductionsBL()
        {
            moEarningsDeductionsDC = new EarningsDeductionsDC();
        }

        public EarningsDeductionsBL(DataTable aoDTSalaryDetails)
        {
            moDTSalaryDetails = aoDTSalaryDetails;
            moEarningsDeductionsDC = new EarningsDeductionsDC();
        } 

        #endregion

        #region Property(s)

        public EarningsDeductions EarningsDeduction
        {
            get { return moEarningsDeductionsDC.EarningsDeduction; }
            set 
            { 
                moEarningsDeductionsDC.EarningsDeduction = value;
                moEarningsDeductions = value;
            }
        }

        public SalaryDifferenceBL SalaryDifferenceBL
        {
            set { moSalaryDifferenceBL = value; }
        }

        public List<EarningsDeductions> EarningsDeductions
        {
            get { return moEarningsDeductionsDC.EarningsDeductions; }
            set { moEarningsDeductionsDC.EarningsDeductions = value; }
        }

        public List<StaffGroupsEarningDeductionAssociation> StaffGroupsEarningDeductionAssociations
        {
            set { mlstStaffGroupsEarningDeductionAssociations = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to return a datatable of subcategories.
        /// </summary>
        public static List<EarningsDeductions> GetAll(int aiSchoolId)
        {
            return EarningsDeductionsDC.GetAll(aiSchoolId);
        }

        /// <summary>
        /// This method is sed to validate short name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <param name="asShortNameList"></param>
        /// <param name="abIsEarningDeduction"></param>
        /// <returns></returns>
        public static string ValidateShortName(int aiSchoolId, int aiAcademicYearID, string asShortNameList, bool abIsEarningDeduction)
        {
            return EarningsDeductionsDC.ValidateShortName(aiSchoolId, aiAcademicYearID, asShortNameList, abIsEarningDeduction);
        }        
        
        /// <summary>
        /// This method is used to add / update / delet earning deduction configuration.
        /// </summary>
        /// <param name="aoEarnings"></param>
        /// <param name="aoDeductions"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        public void Update(List<EarningsDeductions> aoEarnings, List<EarningsDeductions> aoDeductions, int aiAcademicYearId, int aiSchoolId)
        {
            string sEarningMessage = CheckDependencies(aoEarnings, Constants.ReferenceId.Earnings, aiAcademicYearId);
            string sDeductionMessage = CheckDependencies(aoDeductions, Constants.ReferenceId.Deductions, aiAcademicYearId);

            const string TRUE = "1";
            const string FALSE = "0";

            if (string.IsNullOrEmpty(sEarningMessage) && string.IsNullOrEmpty(sDeductionMessage))
            {
                ArrayList oArrayList = new ArrayList();
                List<EarningsDeductions> oEarningsDeductions;

                List<int> lstEarningDeductions = new List<int>();

                const string S_ELEMENT = "element";
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("EarnDeduction");
                XmlNode xmlRootNode = doc.CreateNode(S_ELEMENT, "EarnDeduction", "");

                for (int iCollectionCount = 0; iCollectionCount < 2; iCollectionCount++)
                {
                    if (iCollectionCount == 0)
                        oEarningsDeductions = aoEarnings;
                    else
                        oEarningsDeductions = aoDeductions;

                    foreach (var ED in oEarningsDeductions)
                    {
                        XmlNode oXmlNode = doc.CreateNode(S_ELEMENT, "EarnDeduction", "");

                        XmlAttribute attr = doc.CreateAttribute("EarningsDeductionsId");
                        attr.Value = ED.EarningsDeductionsId.ToString();
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("EarningsDeductionsName");
                        attr.Value = ED.EarningsDeductionsName;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("ShortName");
                        attr.Value = StringUtility.ReplaceSingleQuoteInString(ED.ShortName, false);
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("OriginaEarningsDeductionsId");
                        attr.Value = ED.OriginalEarningsDeductionsId.ToString();
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("IsEarning");
                        attr.Value = ED.IsEarning ? TRUE : FALSE;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("HasFormula");
                        attr.Value = ED.HasFormula ? TRUE : FALSE;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("IsAttendanceDependent");
                        attr.Value = ED.IsAttendanceDependent ? TRUE : FALSE;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("IsBasicEarningDeduction");
                        attr.Value = ED.IsBasic ? TRUE : FALSE;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("MonthId");
                        attr.Value = DateTime.Now.Month.ToString();
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("Year");
                        attr.Value = DateTime.Now.Year.ToString();
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("IsModified");
                        attr.Value = ED.IsModified ? TRUE : FALSE;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("Is_Deleted");
                        attr.Value = ED.Action == Constants.Action.Delete ? Constants.S_YES : Constants.S_NO;
                        oXmlNode.Attributes.Append(attr);
                        if (ED.Action == Constants.Action.Delete)
                            lstEarningDeductions.Add(ED.EarningsDeductionsId);

                        attr = doc.CreateAttribute("IsNew");
                        attr.Value = ED.Action == Constants.Action.Insert ? Constants.S_YES : Constants.S_NO;
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("InsertedById");
                        attr.Value = ED.InsertedById.ToString();
                        oXmlNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("IncludeInSalaryDifference");
                        attr.Value = ED.IncludeInSalaryDifference ? TRUE : FALSE;
                        oXmlNode.Attributes.Append(attr);

                        xmlRootNode.AppendChild(oXmlNode);
                    }
                }
                root.AppendChild(xmlRootNode);
                if (lstEarningDeductions.Count > 0)
                    EarningsDeductionsDC.ValidateEarningsDeductions(lstEarningDeductions, aiSchoolId);
                moEarningsDeductionsDC.Update(root.InnerXml, aiSchoolId);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sEarningMessage + sDeductionMessage);
            }
        }

        /// <summary>
        /// This method is used to check dependancies of subcategories.
        /// </summary>
        /// <param name="aoEarningsDeduction"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        private string CheckDependencies(List<EarningsDeductions> aoEarningsDeduction, Constants.ReferenceId aoReferenceId, int aiAcademicYearId)
        {
            GenericReferenceList<EarningsDeductions> objStdRefereces = new GenericReferenceList<EarningsDeductions>(aoEarningsDeduction, aiAcademicYearId);
            return objStdRefereces.CheckDependenciesForList("EarningsDeductionsId", "EarningsDeductionsName", "Action", aoReferenceId, false);
        }

        /// <summary>
        /// This method is used to return earnings and deductions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet GetFormulaDetails(int aiSchoolId, int aiAcadsemicYearId, int aiEarningDeductionId)
        {
            return moEarningsDeductionsDC.GetFormulaDetails(aiSchoolId, aiAcadsemicYearId, aiEarningDeductionId);
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to add earning deduction columns.
        /// </summary>
        /// <param name="aoEarningsDeductions"></param>
        public List<string> AddEarningDeductionColumns(bool abIsEarning, DataTable aoDTTempSalaryDetails)
        {
            List<string> lstTotalEarningsDeductions = new List<string>();
            List<EarningsDeductions> olstEarningsDeductions;
            if(abIsEarning)
                olstEarningsDeductions  = EarningsDeductions.Where(earningsDeduction => earningsDeduction.IsEarning).ToList();
            else
                olstEarningsDeductions = EarningsDeductions.Where(earningsDeduction => !earningsDeduction.IsEarning).ToList();

            // Add earning deduction columns into list and table.
            olstEarningsDeductions.ForEach
                (
                    earningDeduction =>
                    {
                        moDTSalaryDetails.Columns.Add(earningDeduction.ShortName);
                        lstTotalEarningsDeductions.Add(earningDeduction.ShortName);
                        if (aoDTTempSalaryDetails != null)
                            aoDTTempSalaryDetails.Columns.Add(earningDeduction.ShortName);
                        if (Convert.ToBoolean(earningDeduction.IsAttendanceDependent) == true)
                        {
                            moDTSalaryDetails.Columns.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                            lstTotalEarningsDeductions.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                            if (aoDTTempSalaryDetails != null)
                                aoDTTempSalaryDetails.Columns.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                        }
                    }
                );
            return lstTotalEarningsDeductions;
        }

        /// <summary>
        /// This method is used to set attendance dependent columns.
        /// </summary>
        public List<String> GetAttendanceDependentColumns()
        {
            List<String> lstAttendanceDependentColumns = new List<string>();            
            EarningsDeductions.ForEach(earningDeduction =>
            {
                // Add attendance dependent columns into list.
                if (earningDeduction.OriginalEarningsDeductionsId != I_ORIGINAL_PROFESSIONAL_TAX_ID && (earningDeduction.IsAttendanceDependent || earningDeduction.HasFormula))
                {
                    if (earningDeduction.IsAttendanceDependent)
                        lstAttendanceDependentColumns.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                    else
                        lstAttendanceDependentColumns.Add(earningDeduction.ShortName);
                }
            });

            lstAttendanceDependentColumns.Add(PayrollConstants.S_GROSS_SALARY);
            lstAttendanceDependentColumns.Add(PayrollConstants.S_TOTAL_DEDUCTION);
            lstAttendanceDependentColumns.Add(PayrollConstants.S_NET_SALARY);

            return lstAttendanceDependentColumns;
        }

        /// <summary>
        /// This method is used to set earning deduction IDs.
        /// </summary>
        public List<int> GetEarningDeductionIDs()
        {
            return EarningsDeductions.Select(earningsDeduction => earningsDeduction.EarningsDeductionsId).ToList();
        }

        /// <summary>
        /// This method is used to return earning deduction.
        /// </summary>
        /// <param name="aolstEarningsDeductions"></param>
        /// <param name="aiValueOfED"></param>
        /// <param name="aiEDId"></param>
        /// <returns></returns>
        public List<UsersEarnDeductDetails> GetEarningDeductions(int aiValueOfED, int aiEDId)
        {
            List<UsersEarnDeductDetails> olstEDAppend = (from AppendED in EarningsDeductions
                                                         where AppendED.EarningsDeductionsId == aiEDId
                                                         select new UsersEarnDeductDetails
                                                         {
                                                             EarningsDeductionsId = AppendED.EarningsDeductionsId,
                                                             ShortName = AppendED.ShortName,
                                                             EarningsDeductionsValue = aiValueOfED,
                                                             IsAttendanceDependent = AppendED.IsAttendanceDependent,
                                                             IsEarning = AppendED.IsEarning,
                                                             HasFormula = AppendED.HasFormula
                                                         }).ToList();
            return olstEDAppend;
        }

        /// <summary>
        /// This method is used to set efault values to earning deduction.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="olstRemainingEarnDeduct1"></param>
        public void SetEarnDeductDefaultValues(int aiRowIndex, List<int> aolstRemainingEarnDeducts)
        {
            SetEarnDeductDefaultValues(aiRowIndex, aolstRemainingEarnDeducts, false);
            SetEarnDeductDefaultValues(aiRowIndex, aolstRemainingEarnDeducts, true);
        }

        /// <summary>
        /// This method is used to set efault values to earning deduction.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="olstRemainingEarnDeduct1"></param>
        public void SetEarnDeductDefaultValues(int aiRowIndex, List<int> aolstRemainingEarnDeduct, bool abIsAttendanceDependent)
        {
            var oEDToRem = from EarnDeduction in EarningsDeductions
                           join EarningsDeductionsId in aolstRemainingEarnDeduct
                           on EarnDeduction.EarningsDeductionsId equals EarningsDeductionsId
                           select new
                           {
                               ShortName = EarnDeduction.ShortName,
                               Value = -1,
                               EarningsDeductionsId = EarnDeduction.EarningsDeductionsId,
                               HasFormula = EarnDeduction.HasFormula,
                               IsAttendanceDependent = EarnDeduction.IsAttendanceDependent
                           };

            string sAttendanceDependent = string.Empty;
            if (abIsAttendanceDependent)
            {
                sAttendanceDependent = S_LEAVE_DEDUCTED;
                oEDToRem = oEDToRem.Where(ed => ed.IsAttendanceDependent);
            }

            // Update table with value -1 if rescpective earning deduction is not associated with group.
            foreach (var earnDeduction in oEDToRem)
            {
                moDTSalaryDetails.Rows[aiRowIndex][sAttendanceDependent + earnDeduction.ShortName] = earnDeduction.Value;
                moSalaryDifferenceBL.PopulateSalaryDifferenceClass(earnDeduction.ShortName, PayrollConstants.ED, earnDeduction.EarningsDeductionsId);
            }
        }
        
        /// <summary>
        /// This method is used to set default values to earning- deduction if these are not available.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiUserId"></param>
        public void SetDefaultEDValuesIfNotAvail(int aiRowIndex, int aiUserId, List<UsersSGAssociation> alstUsersSGAssociations)
        {
            var oAssociatedED = from StaffGroupEarnDeductAsso in mlstStaffGroupsEarningDeductionAssociations
                                join EarnDeduct in EarningsDeductions
                                on StaffGroupEarnDeductAsso.EarningsDeductionsId equals EarnDeduct.EarningsDeductionsId
                                join UsersSG in alstUsersSGAssociations
                                on StaffGroupEarnDeductAsso.StaffGroupsId equals UsersSG.StaffGroupsId
                                where UsersSG.UserId == aiUserId
                                select new
                                {
                                    ShortName = EarnDeduct.ShortName,
                                    EarningsDeductionsId = StaffGroupEarnDeductAsso.EarningsDeductionsId,
                                    Value = 0,
                                    HasFormula = EarnDeduct.HasFormula,
                                    IsAttendanceDependent = EarnDeduct.IsAttendanceDependent
                                };

            // update tbale with zero values if user doesn't have value for it.
            foreach (var staffGroupEarnDeductAsso in oAssociatedED)
            {
                moDTSalaryDetails.Rows[aiRowIndex][staffGroupEarnDeductAsso.ShortName.ToString()] = staffGroupEarnDeductAsso.Value;
                if (Convert.ToBoolean(staffGroupEarnDeductAsso.IsAttendanceDependent) == true)
                {
                    moDTSalaryDetails.Rows[aiRowIndex][staffGroupEarnDeductAsso.ShortName] = staffGroupEarnDeductAsso.Value;
                    moSalaryDifferenceBL.PopulateSalaryDifferenceClass(staffGroupEarnDeductAsso.ShortName, PayrollConstants.ED, staffGroupEarnDeductAsso.EarningsDeductionsId);

                    moDTSalaryDetails.Rows[aiRowIndex][String.Format("Leave Deducted {0}", staffGroupEarnDeductAsso.ShortName)] = staffGroupEarnDeductAsso.Value;
                    moSalaryDifferenceBL.PopulateSalaryDifferenceClass(String.Format("Leave Deducted {0}", staffGroupEarnDeductAsso.ShortName), PayrollConstants.LD, staffGroupEarnDeductAsso.EarningsDeductionsId);
                }
            }
        }

        #endregion
    }
}
