using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using System.Data;
using PayrollEntities;


namespace BusinessLogic
{
    public class ShiftDetailsBL
    {
        #region " Constants "

        private const string S_DUPLICATE_SHIFT_NAME = "Shift Name already exists.";

        #endregion " Constants "

        #region "Constructer"
        private ShiftDetailsDC.ShiftDetailsStruct moShiftDetailsStruct;

        private ShiftDetailsDC moShiftDetailsDC;

        public ShiftDetailsBL()
        {
            moShiftDetailsDC = new ShiftDetailsDC();
        }

        public ShiftDetailsBL(int miShiftId, int miSchoolId, int miAcademicYearId)
        {
            moShiftDetailsDC = new ShiftDetailsDC(miShiftId, miSchoolId, miAcademicYearId);
            moShiftDetailsStruct = moShiftDetailsDC.ShiftDetailsStructDetails;
        }
        #endregion

        #region "properties"
        public virtual int ShiftId
        {
            get
            {
                return moShiftDetailsStruct.miShiftId;
            }
            set
            {
                moShiftDetailsStruct.miShiftId = value;
            }
        }

        public virtual string ShiftName
        {
            get
            {
                return moShiftDetailsStruct.msShiftName;
            }
            set
            {
                moShiftDetailsStruct.msShiftName = value;
            }
        }

        public virtual string ShiftStartTime
        {
            get
            {
                return moShiftDetailsStruct.msShiftStartTime;
            }
            set
            {
                moShiftDetailsStruct.msShiftStartTime = value;
            }
        }

        public virtual string ShiftEndTime
        {
            get
            {
                return moShiftDetailsStruct.msShiftEndTime;
            }
            set
            {
                moShiftDetailsStruct.msShiftEndTime = value;
            }
        }

        public virtual string HalfDayTime
        {
            get
            {
                return moShiftDetailsStruct.msHalfDayTime;
            }
            set
            {
                moShiftDetailsStruct.msHalfDayTime = value;
            }
        }

        public virtual string LateMarkTime
        {
            get
            {
                return moShiftDetailsStruct.msLateMarkTime;
            }
            set
            {
                moShiftDetailsStruct.msLateMarkTime = value;
            }
        }

        public virtual int SchoolId
        {
            get
            {
                return moShiftDetailsStruct.miSchoolId;
            }
            set
            {
                moShiftDetailsStruct.miSchoolId = value;
            }
        }

        public virtual bool IsDefault
        {
            get
            {
                return moShiftDetailsStruct.mbIsDefault;
            }
            set
            {
                moShiftDetailsStruct.mbIsDefault = value;
            }
        }

        public virtual int AcademicYearId
        {
            get
            {
                return moShiftDetailsStruct.miAcademicYearId;
            }
            set
            {
                moShiftDetailsStruct.miAcademicYearId = value;
            }
        }

        public virtual char Is_Deleted
        {
            get
            {
                return moShiftDetailsStruct.mcIs_Deleted;
            }
            set
            {
                moShiftDetailsStruct.mcIs_Deleted = value;
            }
        }

        public virtual System.DateTime InsertDate
        {
            get
            {
                return moShiftDetailsStruct.mdtInsertDate;
            }
            set
            {
                moShiftDetailsStruct.mdtInsertDate = value;
            }
        }

        public virtual int InsertedById
        {
            get
            {
                return moShiftDetailsStruct.miInsertedById;
            }
            set
            {
                moShiftDetailsStruct.miInsertedById = value;
            }
        }

        public virtual System.DateTime UpdateDate
        {
            get
            {
                return moShiftDetailsStruct.mdtUpdateDate;
            }
            set
            {
                moShiftDetailsStruct.mdtUpdateDate = value;
            }
        }

        public virtual int UpdatedById
        {
            get
            {
                return moShiftDetailsStruct.miUpdatedById;
            }
            set
            {
                moShiftDetailsStruct.miUpdatedById = value;
            }
        }
        #endregion

        private int miShiftCount;

        #region "Public Methods"
        /// <summary>
        /// This function is used to check duplicate entry of Shift Name.
        /// </summary>
        /// <returns></returns>
        public bool IsNameDuplicateShift()
        {
            moShiftDetailsDC.ShiftDetailsStructDetails = moShiftDetailsStruct;
            bool bIsDuplicate = moShiftDetailsDC.IsDuplicateShiftName();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException(S_DUPLICATE_SHIFT_NAME);
            return bIsDuplicate;
        }

        /// <summary>
        /// This Function is used to insert Shift details.
        /// </summary>
        /// <returns></returns>
        public virtual void InsertShiftDetails(string asType)
        {
            moShiftDetailsDC.ShiftDetailsStructDetails = moShiftDetailsStruct;
            moShiftDetailsDC.InsertShiftDetails(asType);
        }

        /// <summary>
        /// This function is used to get all shift details and bind to object data source. 
        /// </summary>
        public DataTable GetAll(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            DataTable oDt = ShiftDetailsDC.GetAll(aiSchoolId, aiAcademicYearId, sortExpression, iEndIndex, startRowIndex);
            if (oDt != null && oDt.Rows.Count > 0)
                miShiftCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// This function is used to get all shift details and bind to object data source. 
        /// </summary>
        public static DataTable GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            return ShiftDetailsDC.GetAll(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This function is used to get total count of Stop Names and bind to object data source. 
        /// </summary>
        public int CountTotalShiftRecords(Int32 aiSchoolId, Int32 aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex)
        {
            return miShiftCount;
        }

        /// <summary>
        /// This function is used to check duplicate entry of Shift Name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateShift()
        {
            moShiftDetailsDC.ShiftDetailsStructDetails = moShiftDetailsStruct;
            bool bIsDuplicate = moShiftDetailsDC.IsDuplicateShiftName();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException(S_DUPLICATE_SHIFT_NAME);
            return bIsDuplicate;
        }

        /// <summary>
        /// This function is used to Delete Shift Name.
        /// </summary>
        /// <returns></returns>
        public virtual void DeleteShiftDetails(int aiShiftId, int aiSchoolId, int aiAcademicYearId)
        {
            moShiftDetailsDC.ShiftDetailsStructDetails = moShiftDetailsStruct;
            moShiftDetailsDC.DeleteShiftDetails(aiShiftId, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to check weather any user is associated with any shift or not.
        /// </summary>
        /// <param name="aiShiftId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public int CheckDependencyForShift(int aiShiftId, int aiSchoolId, int aiAcademicYearId)
        {
            moShiftDetailsDC.ShiftDetailsStructDetails = moShiftDetailsStruct;
            return moShiftDetailsDC.CheckDependencyForShift(aiShiftId, aiSchoolId, aiAcademicYearId);
        }

        public List<SchoolShifts> GetAllShifts(int aiSchoolId, int aiAcademicYearId)
        {
            return moShiftDetailsDC.GetAllShifts(aiSchoolId, aiAcademicYearId);
        }

        #endregion
    }
}
