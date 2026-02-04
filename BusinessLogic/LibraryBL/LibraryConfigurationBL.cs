using System;
using System.Data;
using System.Collections;
using DataCommunicator;

namespace BusinessLogic
{
    public class LibraryConfigurationBL
    {
        #region Property & Data Member

        #region Data members

        private LibraryConfigurationDC.LibraryConfigurationStructDetails moLibraryConfigurationStructDetails;
        private LibraryConfigurationDC moLibraryConfigurationDC = new LibraryConfigurationDC();

        #endregion

        public Int32 SchoolId
        {
            get { return moLibraryConfigurationStructDetails.miSchoolId; }
            set { moLibraryConfigurationStructDetails.miSchoolId = value; }
        }

        public Int32 AcademicYearId
        {
            get { return moLibraryConfigurationStructDetails.miAcademicYearId; }
            set { moLibraryConfigurationStructDetails.miAcademicYearId = value; }
        }

        public Int32 UserRoleID
        {
            get { return moLibraryConfigurationStructDetails.miUserRoleId; }
            set { moLibraryConfigurationStructDetails.miUserRoleId = value; }
        }

        public Int32 ReturnDays
        {
            get { return moLibraryConfigurationStructDetails.miReturnDays; }
            set { moLibraryConfigurationStructDetails.miReturnDays = value; }
        }

        public Int32 RenewAttempt
        {
            get { return moLibraryConfigurationStructDetails.miRenewAttempt; }
            set { moLibraryConfigurationStructDetails.miRenewAttempt = value; }
        }

        public Int32 BookPerPerson
        {
            get { return moLibraryConfigurationStructDetails.miBookPerPerson; }
            set { moLibraryConfigurationStructDetails.miBookPerPerson = value; }
        }
        public Int32 LateFeePerDay
        {
            get { return moLibraryConfigurationStructDetails.miLateFeePerDay; }
            set { moLibraryConfigurationStructDetails.miLateFeePerDay = value; }
        }
        public Int32 LateFeeEffectiveDays
        {
            get { return moLibraryConfigurationStructDetails.miLateFeeEffectiveDays; }
            set { moLibraryConfigurationStructDetails.miLateFeeEffectiveDays = value; }
        }

        public Int32 UserId
        {
            get { return moLibraryConfigurationStructDetails.miUser_Id; }
            set { moLibraryConfigurationStructDetails.miUser_Id = value; }
        }
        public Int32 InsertedById
        {
            get { return moLibraryConfigurationStructDetails.miInsertedById; }
            set { moLibraryConfigurationStructDetails.miInsertedById = value; }
        }
        public System.DateTime InsertedDate
        {
            get { return moLibraryConfigurationStructDetails.mdtInsertedDate; }
            set { moLibraryConfigurationStructDetails.mdtInsertedDate = value; }
        }
        public Int32 UpdatedById
        {
            get { return moLibraryConfigurationStructDetails.miUpdatedById; }
            set { moLibraryConfigurationStructDetails.miUpdatedById = value; }
        }
        public System.DateTime UpdatedDate
        {
            get { return moLibraryConfigurationStructDetails.mdtUpdatedDate; }
            set { moLibraryConfigurationStructDetails.mdtUpdatedDate = value; }
        }

        public Int32 LibConfigId
        {
            get { return moLibraryConfigurationStructDetails.miLibConfigId; }
            set { moLibraryConfigurationStructDetails.miLibConfigId = value; }
        }

        public Int32 ReserveBooks
        {
            get { return moLibraryConfigurationStructDetails.miReserveBooks; }
            set { moLibraryConfigurationStructDetails.miReserveBooks = value; }
        }
        #endregion

        public LibraryConfigurationBL()
        {
        }

        public LibraryConfigurationBL(int aiUserRoleID, int aiSchoolId, int aiAcademicYearId)
        {
            moLibraryConfigurationDC = new LibraryConfigurationDC(aiUserRoleID, aiSchoolId, aiAcademicYearId);
            moLibraryConfigurationStructDetails = moLibraryConfigurationDC.LibraryConfigurationInfo;
        }

        public DataTable RetriveLibraryConfigurarion()
        {
            moLibraryConfigurationDC.LibraryConfigurationInfo = moLibraryConfigurationStructDetails;
            return moLibraryConfigurationDC.RetriveLibraryConfigurarion();
        }

        public void AddLibraryConfigurarion()
        {
            moLibraryConfigurationDC.LibraryConfigurationInfo = moLibraryConfigurationStructDetails;
            moLibraryConfigurationDC.AddLibraryConfigurarion();
        }

        public void UpdateLibraryConfigurarion()
        {
            moLibraryConfigurationDC.LibraryConfigurationInfo = moLibraryConfigurationStructDetails;
            moLibraryConfigurationDC.UpdateLibraryConfigurarion();
        }

        public DataTable GetUserRoles()
        {
            return moLibraryConfigurationDC.GetUserRoles();
        }
    }
}
