using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using TransportEntities;
using Utility;
using DataCommunicator.TransportDC;
using SchoolEntities.Transport;

namespace BusinessLogic.TransportBL
{
    public class BulkDocumentDetailsBL
    {
        #region Data Member(s)

        private BulkDocumentDetailsDC moBulkDocumentDetailsDC = null; 

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDocumentBL" /> class.
        /// </summary>
         public BulkDocumentDetailsBL()
        {
            this.moBulkDocumentDetailsDC = new BulkDocumentDetailsDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDocumentBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public BulkDocumentDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moBulkDocumentDetailsDC = new BulkDocumentDetailsDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
        } 

        #endregion

        #region Method(s)
        
        public List<GetBulkDocumentDetails> GetDocumentsDetails(int aiId, string asFilter, bool abShowAll)
        {
            return moBulkDocumentDetailsDC.GetDocumentsDetails(aiId, asFilter, abShowAll);
        }

        public void Save(int aiDocumentId, string sXML)
        {
            moBulkDocumentDetailsDC.Save(aiDocumentId, sXML);
        }

        public void DeleteBulkDocument(int aiId)
        {
            moBulkDocumentDetailsDC.DeleteBulkDocument(aiId);
        }
        
        public string Validate(int aiDocumentId, string asDatesXml)
        {
            return moBulkDocumentDetailsDC.Validate(aiDocumentId, asDatesXml);
        }

        #endregion
    }
}
