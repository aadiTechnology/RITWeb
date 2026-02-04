using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class IncomeDeclarationBL
    {
        #region Data Member(s)

        private IncomeDeclarationDC moIncomeDeclarationDC;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        public IncomeDeclarationBL()
        {
            this.moIncomeDeclarationDC = new IncomeDeclarationDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public IncomeDeclarationBL(int aiSchoolId, int aiFinYearId, int aiUpdatedById)
        {
            this.moIncomeDeclarationDC = new IncomeDeclarationDC(aiSchoolId, aiFinYearId, aiUpdatedById);
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
        public List<IncomeDeclaration> GetAll(int aiUserId, int aiSectionId, string asSortExpression, string asSortDirection)
        {
            return this.moIncomeDeclarationDC.GetAll(aiUserId, aiSectionId, asSortExpression, asSortDirection);
        }

        /// <summary>
        /// This method is used to save investment declarations of respective user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, string asXml, int aiRegimId)
        {
            this.moIncomeDeclarationDC.Save(aiUserId, asXml, aiRegimId);
        }

        #endregion
    }
}
