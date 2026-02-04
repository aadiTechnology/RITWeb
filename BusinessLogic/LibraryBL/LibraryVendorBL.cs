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
using BookEntities;

namespace BusinessLogic 
{
    public class LibraryVendorBL
    {
        #region "Data Members"
        LibraryVendorDC moLibraryVendorDC;
        #endregion "Data Members"

        #region "Property"
        public LibraryVendors LibraryVendor
        {
            get { return moLibraryVendorDC.moLibraryVendor; }
            set { moLibraryVendorDC.moLibraryVendor = value; }
        }
        #endregion "Property"

        #region "Constructors"
        public LibraryVendorBL()
        {
            moLibraryVendorDC = new LibraryVendorDC();
        }

        public LibraryVendorBL(int aiSchoolId,int aiVendorId)
        {
            moLibraryVendorDC = new LibraryVendorDC(aiSchoolId,aiVendorId);
        }
        #endregion "Constructors"

        #region "Public Methods"
        /// <summary>
        /// This method is used to insert library vendor details.
        /// </summary>
        public void InsertLibraryVendorBL()
        {
            moLibraryVendorDC.InsertLibraryVendorDC();
        }

        /// <summary>
        /// This method is used to update library vendor details.
        /// </summary>
        /// <param name="aiVendorId"></param>
        public void UpdateLibraryVendorBL(int aiVendorId)
        {
            moLibraryVendorDC.UpdateLibraryVendorDC(aiVendorId);
        }

        /// <summary>
        /// This method is used to delete library vendor details.
        /// </summary>
        /// <param name="iVendorId"></param>
        public void DeleteLibraryVendorBL(int iVendorId)
        {
            moLibraryVendorDC.DeleteLibraryVendorDC(iVendorId);
        }

        /// <summary>
        /// This method is used to get paged library vendor details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<LibraryVendors> GetLibraryVendorDetailsBL(int aiSchoolId, String sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moLibraryVendorDC.GetLibraryVendorDetailsDC(aiSchoolId, sortExpression, iEndIndex, iStartIndex);
        }

        /// <summary>
        /// This method is used to get count of total library vendor records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int CountTotalLibraryVendorBL(int aiSchoolId, String sortExpression, int maximumRows, int startRowIndex)
        {
            return LibraryVendorDC.CountTotalLibraryVendorDC(aiSchoolId, sortExpression, maximumRows, startRowIndex);
        }

        /// <summary>
        /// This method is used to check whether the vendor is associated with any book or not.
        /// </summary>
        /// <param name="aiVendorId"></param>
        /// <returns></returns>
        public int CountAssociatedLibraryVendorBL(int aiVendorId)
        {
            return moLibraryVendorDC.GetCountAssociatedLibraryVendorDC(aiVendorId);
        }
        #endregion "Public Methods"

        public int IsVendorDuplicate()
        {
            return moLibraryVendorDC.IsVendorDuplicateDC();
        }
    }
}