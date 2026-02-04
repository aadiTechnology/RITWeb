// Class Name       :- StandardwiseDocumentMasterBL
// Purpose          :- This class is used to manage StandardwiseDocumentMaster details.
// Date Of creation :- 3/15/2011
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
using DocumentEntity;

namespace BusinessLogic
{
    public class StandardwiseDocumentMasterBL
    {

        #region "Data Members"

        public StandardwiseDocumentMasterDC moStandardwiseDocumentMasterDC = null;

        #endregion "Data Members"

        #region "Constructors"

        public StandardwiseDocumentMasterBL()
        {
            moStandardwiseDocumentMasterDC = new StandardwiseDocumentMasterDC();
        }
        public StandardwiseDocumentMasterBL(int aiSchoolId)
        {
            moStandardwiseDocumentMasterDC = new StandardwiseDocumentMasterDC(aiSchoolId);
        }

        public StandardwiseDocumentMasterBL(int aiSchoolId, int aiAcademicYearId)
        {
            moStandardwiseDocumentMasterDC = new StandardwiseDocumentMasterDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion "Constructors"

        #region "Properties"

        public StandardwiseDocument StandardwiseDocument
        {
            set { moStandardwiseDocumentMasterDC.StandardwiseDocumentDetails = value; }
            get { return moStandardwiseDocumentMasterDC.StandardwiseDocumentDetails; }
        }
        public StudentDocument StudentDocument
        {
            set { moStandardwiseDocumentMasterDC.StudentDocument = value; }
            get { return moStandardwiseDocumentMasterDC.StudentDocument; }
        }

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all the details of documents as per the standard and Standard details.
        /// </summary>
        public DataTable GetAllStandardDetails()
        {
            return moStandardwiseDocumentMasterDC.GetAllStandardDetails();
        }

        /// <summary>
        /// This method is used to get all the details of documents as per the standard and Standard details.
        /// </summary>
        public List<StandardwiseDocument> GetAllDocumentsByStandard(int aiOriginalStandardId)
        {
            return moStandardwiseDocumentMasterDC.GetAllDocumentsByStandard(aiOriginalStandardId);
        }

        /// <summary>
        /// This method is used to save document details.
        /// </summary>
        /// <param name="asXml"></param>
        public void SaveStandardwiseDocumentDetails(string asXml, int aiInsertedById)
        {
            moStandardwiseDocumentMasterDC.SaveStandardwiseDocumentDetails(asXml, aiInsertedById);
        }

        /************Collapsible Panel**********************/

        /// <summary>
        /// This method is used to get document details which are require while taking admission.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<StudentDocument> GetAdmissionDocumentDetails(int aiStandardId, int aiStudentId)
        {
            return moStandardwiseDocumentMasterDC.GetAdmissionDocumentDetails(aiStandardId, aiStudentId);
        }

        /// <summary>
        /// This method is used to save student submitted document details.
        /// </summary>
        /// <param name="asDocXML"></param>
        public void SaveSubmittedDocuments(string asDocXML, int aiStudentId, int aiInsertedById)
        {
            moStandardwiseDocumentMasterDC.SaveSubmittedDocuments(asDocXML, aiStudentId, aiInsertedById);
        }

        #endregion "Public Methods"
    }
}
