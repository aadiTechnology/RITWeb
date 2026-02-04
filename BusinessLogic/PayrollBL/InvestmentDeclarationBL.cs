// File Name - InvestmentDeclarationBL.cs
// Creator - Sachin
// Created Date -

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class InvestmentDeclarationBL
    {
        #region Data Member(s)

        private InvestmentDeclarationDC moInvestmentDeclarationDC;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        public InvestmentDeclarationBL()
        {
            this.moInvestmentDeclarationDC = new InvestmentDeclarationDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public InvestmentDeclarationBL(int aiSchoolId, int aiFinYearId, int aiUpdatedById)
        {
            this.moInvestmentDeclarationDC = new InvestmentDeclarationDC(aiSchoolId, aiFinYearId, aiUpdatedById);
        }

        #endregion

        #region Property(s)

        public UserDetails UserDetails
        {
            get { return moInvestmentDeclarationDC.UserDetails; }
        }

        public List<SectionDetails> SectionDetails
        {
            get { return moInvestmentDeclarationDC.SectionDetails; }
        }

        public List<InvestmentDeclaration> InvestmentDeclarations
        {
            get { return moInvestmentDeclarationDC.InvestmentDeclarations; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all the investment declarations of respective user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSectionId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <returns>Entity list of Investment Declarations</returns>
        public List<InvestmentDeclaration> GetAll(int aiUserId, int aiSectionId, string asSortExpression, string asSortDirection)
        {
            return this.moInvestmentDeclarationDC.GetAll(aiUserId, aiSectionId, asSortExpression, asSortDirection);
        }

        /// <summary>
        /// This method is used to save investment declarations of respective user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, string asXml, int aiRegimId)
        {
            this.moInvestmentDeclarationDC.Save(aiUserId, asXml, aiRegimId);
        }

        /// <summary>
        /// This method is used to return investment documents.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<InvestmentDocument> GetDocuments(int aiDocuentId, int aiUserId, int aiDocumnetTypeId,int aiAcademicYearId,int aiReportingUserId)
        {
            return moInvestmentDeclarationDC.GetDocuments(aiDocuentId, aiUserId, aiDocumnetTypeId, aiAcademicYearId,aiReportingUserId);
        }

        /// <summary>
        /// This method is used to save document.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="asFileName"></param>
        /// <param name="aiUserId"></param>
        public void SaveDocument(int aiDocumentId, string asFileName, int aiUserId, int aiDocumnetTypeId,int aiAcademicYearId,int aiReportingUserId)
        {
            moInvestmentDeclarationDC.SaveDocument(aiDocumentId, asFileName, aiUserId,aiDocumnetTypeId,aiAcademicYearId,aiReportingUserId);
        }

        /// <summary>
        /// This method is used to delete document.
        /// </summary>
        /// <param name="iId"></param>
        public void DeleteDocument(int aiDocumentId, int aiDocumnetTypeId)
        {
            moInvestmentDeclarationDC.DeleteDocument(aiDocumentId, aiDocumnetTypeId);
        }

        /// <summary>
        /// This method is used to return user and investment method details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="asInvestmentMethod"></param>
        /// <returns></returns>
        public string GetUserInvestmentMethodDetails(int aiUserId, int aiDocumentId, out string asDocumentName, int aiDocumentTypeId)
        {
            return moInvestmentDeclarationDC.GetUserInvestmentMethodDetails(aiUserId, aiDocumentId, out asDocumentName, aiDocumentTypeId);
        }

        /// <summary>
        /// This method is used to return investment details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<InvestmentMethod> GetInvestmentDetails(int aiUserId)
        {
            return moInvestmentDeclarationDC.GetInvestmentDetails(aiUserId);
        }

        /// <summary>
        /// This method is used to save investment details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asDeclarations"></param>
        public void SaveInvestmentDeclaration(int aiUserId, string asDeclarations, int aiRegimeId)
        {
            moInvestmentDeclarationDC.SaveInvestmentDeclaration(aiUserId, asDeclarations, aiRegimeId);
        }

        /// <summary>
        /// This method is used to submit investment details.
        /// </summary>
        /// <param name="aiUserId"></param>
        public void SubmitInvestmentDetails(int aiUserId)
        {
            moInvestmentDeclarationDC.SubmitInvestmentDetails(aiUserId);
        }

        public List<UserDetails> GetRegimeDetails()
        {
            return moInvestmentDeclarationDC.GetRegimeDetails();
        }

        #endregion
    }
}
