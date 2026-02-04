// -----------------------------------------------------------------------
/* File Name = QualificationBL
 * Created Date - 12 March 2015
 * Created by - Yogesh
 * Class Description - This class used to manage business logic about qualification details*/
// -----------------------------------------------------------------------

namespace BusinessLogic
{
    using System.Collections.Generic;
    using DataCommunicator;
    using SchoolEntities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualificationBL
    {

        #region MEMBER(S)

        private QualificationDC moQualificationDC;

        #endregion

        #region CONSTRUCTOR(S)

        public QualificationBL()
        {
            moQualificationDC = new QualificationDC();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get all qualification details.
        /// </summary>
        /// <returns></returns>
        public static List<QualificatoinDetails> GetAll()
        {
            return QualificationDC.GetAll();
        }

        /// <summary>
        /// This method is used to save qualification details.
        /// </summary>
        /// <param name="aoQualificatoinDetails"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static string Save(QualificatoinDetails aoQualificatoinDetails, int aiInsertedById, int aiAcademicYearId, int aiSchoolId)
        {
            return QualificationDC.Save(aoQualificatoinDetails, aiInsertedById, aiAcademicYearId, aiSchoolId);
        }

        /// <summary>
        /// This method is used to delete qualification details.
        /// </summary>
        /// <param name="aiQualificatoinId"></param>
        public static void Delete(int aiQualificationId, int aiAcademicYearId, int aiUserId)
        {
            QualificationDC.Delete(aiQualificationId, aiAcademicYearId, aiUserId);
        }

        #endregion

    }
}
