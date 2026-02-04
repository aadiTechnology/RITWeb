// Class Name       :- StaffHolidayAndLeavesConfigurationBL
// Purpose          :- This class is used to configuration Staff holiday for salary deduction details.
// Date Of creation :- 12/09/2010
// Author Name      :- Shobha Patil

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class StaffHolidaysSalaryDeductionBL
    {
        #region Data Members

        StaffHolidaysSalaryDeductionDC moStaffHolidaysSalaryDeductionDC;

        #endregion

        #region Constructor(s)

        public StaffHolidaysSalaryDeductionBL()
        {
            moStaffHolidaysSalaryDeductionDC = new StaffHolidaysSalaryDeductionDC();
        }

        public StaffHolidaysSalaryDeductionBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moStaffHolidaysSalaryDeductionDC = new StaffHolidaysSalaryDeductionDC(aiSchoolId, aiAcademicYearId, aiUserId);
        } 

        #endregion

        #region Property(s)

        public StaffHolidaysSalaryDeduction StaffHolidaysSalaryDeductionConfig
        {
            get
            {
                return moStaffHolidaysSalaryDeductionDC.StaffHolidaysSalaryDeductionConfig;
            }
            set
            {
                moStaffHolidaysSalaryDeductionDC.StaffHolidaysSalaryDeductionConfig = value;
            }
        }

        public List<StaffHolidaysSalaryDeduction> StaffHolidaysSalaryDeductions
        {
            get { return moStaffHolidaysSalaryDeductionDC.StaffHolidaysSalaryDeductions; }
            set { moStaffHolidaysSalaryDeductionDC.StaffHolidaysSalaryDeductions = value; }
        }

        public List<DatewiseStaffLeave> DatewiseStaffLeaves
        {
            get { return moStaffHolidaysSalaryDeductionDC.DatewiseStaffLeaves; }
            set { moStaffHolidaysSalaryDeductionDC.DatewiseStaffLeaves = value; }
        }

        public List<ConfiguredLeaves> ConfiguredLeaves
        {
            get { return moStaffHolidaysSalaryDeductionDC.ConfiguredLeaves; }
            set { moStaffHolidaysSalaryDeductionDC.ConfiguredLeaves = value; }
        }

        public List<StaffBaseDetails> StaffBaseDetails
        {
            get { return moStaffHolidaysSalaryDeductionDC.StaffBaseDetails; }
            set { moStaffHolidaysSalaryDeductionDC.StaffBaseDetails = value; }
        }

        public List<StaffHolidayLeavesConfigTypes> StaffHolidayLeavesConfigTypes
        {
            get { return moStaffHolidaysSalaryDeductionDC.StaffHolidayLeavesConfigTypes; }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to get the staff holiday configuration details.
        /// </summary>
        public void GetAll()
        {
            moStaffHolidaysSalaryDeductionDC.GetAll();
        }

        /// <summary>
        /// This method is used to save the staff holiday configuration details.
        /// </summary>
        public void Save(string asHolidayConfigXML)
        {
            moStaffHolidaysSalaryDeductionDC.Save(asHolidayConfigXML);
        }

        /// <summary>
        /// This method is used to save the weekend configuration details.
        /// </summary>
        public void SaveWeekendConfiguration(StaffHolidaysSalaryDeduction aoStaffHolidaysSalaryDeduction)
        {
            moStaffHolidaysSalaryDeductionDC.SaveWeekendConfiguration(aoStaffHolidaysSalaryDeduction);
        }

        #endregion
    }
}
