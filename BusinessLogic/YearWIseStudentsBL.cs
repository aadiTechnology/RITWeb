using System;
using DataCommunicator;

namespace BusinessLogic
{
    public class YearWIseStudentsBL : BusinessLogicBaseBL
    {
        #region DataMembers & Properties

        #region DataMembers
        YearWiseStudentsDC moYearWiseStudentDC;
        YearWiseStudentsDC.YrWiseStudentInfo moYrWiseStudentInfo;

        #endregion
        #region Properties

        #region Year wise Student details
        public Int32 SchoolIdYear
        {
            get
            {
                return moYrWiseStudentInfo.iSchoolId;
            }
            set
            {
                moYrWiseStudentInfo.iSchoolId = value;
            }
        }
        /// <summary>
        /// Primary id of yrWise table
        /// </summary>
        public Int32 YrWiseStudentId
        {
            get
            {
                return moYrWiseStudentInfo.iYearWIseStudentId;
            }
            set
            {
                moYrWiseStudentInfo.iYearWIseStudentId = value;
            }
        }
        /// <summary>
        /// main Student id
        /// </summary>
        public Int32 SchoolWiseStudentId
        {
            get
            {
                return moYrWiseStudentInfo.iStudentId;
            }
            set
            {
                moYrWiseStudentInfo.iStudentId = value;
            }
        }

        public Int32 YearId
        {
            get
            {
                return moYrWiseStudentInfo.iSchoolWiseAcademicYearId;
            }
            set
            {
                moYrWiseStudentInfo.iSchoolWiseAcademicYearId = value;
            }
        }
        public Int32 StandardId
        {
            get
            {
                return moYrWiseStudentInfo.iStandardId;
            }
            set
            {
                moYrWiseStudentInfo.iStandardId = value;
            }
        }
        public Int32 DivisionId
        {
            get
            {
                return moYrWiseStudentInfo.iDivisionId;
            }
            set
            {
                moYrWiseStudentInfo.iDivisionId = value;
            }
        }
        public string RollNo
        {
            get
            {
                return moYrWiseStudentInfo.sRollNo;
            }
            set
            {
                moYrWiseStudentInfo.sRollNo = value;
            }
        }
        public Int32 SchoolWiseAcademicYearId
        {
            get
            {
                return moYrWiseStudentInfo.iSchoolWiseAcademicYearId;
            }
            set
            {
                moYrWiseStudentInfo.iSchoolWiseAcademicYearId = value;
            }
        }
        public Double FeesTobePaid
        {
            get
            {
                return moYrWiseStudentInfo.fFeesTobePaid;
            }
            set
            {
                moYrWiseStudentInfo.fFeesTobePaid = value;
            }
        }
        public char IsFeeApplicable
        {
            get
            {
                return moYrWiseStudentInfo.cIsFeeApplicable;
            }
            set
            {
                moYrWiseStudentInfo.cIsFeeApplicable = value;
            }
        }
        public Int32 YearWiseInsertedById
        {
            get
            {
                return moYrWiseStudentInfo.iInsertedById;
            }
            set
            {
                moYrWiseStudentInfo.iInsertedById = value;
            }
        }
        public Int32 YearWiseUpdateddById
        {
            get
            {
                return moYrWiseStudentInfo.iUpdatedById;
            }
            set
            {
                moYrWiseStudentInfo.iUpdatedById = value;
            }
        }


        #endregion
        #endregion

        #endregion

        #region constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public YearWIseStudentsBL()
        {
            moYearWiseStudentDC = new YearWiseStudentsDC();
        }

        public YearWIseStudentsBL(Int32 aiYearWiseStudentId)
        {
            moYearWiseStudentDC = new YearWiseStudentsDC(aiYearWiseStudentId);
            this.moYrWiseStudentInfo = moYearWiseStudentDC.YearWiseStudentInfo;
        }

        #endregion

        #region Public methods

        public Int32 InsertYrWiseStudent()
        {
            moYearWiseStudentDC.YearWiseStudentInfo = moYrWiseStudentInfo;
            return moYearWiseStudentDC.InsertYrWiseStudentInformation();
        }
        public Int32 UpdateYrWiseStudent()
        {
            moYearWiseStudentDC.YearWiseStudentInfo = moYrWiseStudentInfo;
            return moYearWiseStudentDC.UpdateYrWiseStudentInformation();
        }

        #endregion
    }
}
