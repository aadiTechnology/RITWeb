using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
   public class HealthDetailsBL 
   {
       #region Data Member(s)

       private HealthDetailsDC moHealthDetailsDC;
       private int miTotalCount = Constants.I_ZERO;

       #endregion

       #region Constructor(s)

       public HealthDetailsBL()
       {
           moHealthDetailsDC = new HealthDetailsDC();
       }

       public HealthDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
       {
           moHealthDetailsDC = new HealthDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
       }

       #endregion

       #region Method(s)

       /// <summary>
       /// This Method is used to get Student details for Health details.
       /// </summary>
       /// <param name="aiStandardId"></param>
       /// <param name="aiDivisionId"></param>
       /// <returns></returns>
       public List<HealthDetails> GetAllStudentDetails(int aiStandardId, int aiDivisionId)
       {
           return moHealthDetailsDC.GetAllStudentDetails(aiStandardId, aiDivisionId);
       }

       /// <summary>
       /// This method is used to get Student health details.
       /// </summary>
       /// <param name="aiStudentId"></param>
       /// <returns></returns>
       public List<StudentHealthDetails> GetStudentHealthDetails(int aiStudentId)
       {
           return moHealthDetailsDC.GetStudentHealthDetails(aiStudentId);
       }

       /// <summary>
       /// This method is used for Save student health details.
       /// </summary>
       /// <param name="aiStudentId"></param>
       /// <param name="asHealthDetails"></param>
       /// <param name="aiUpdatedById"></param>
       public void SaveStudentHealthDetails(int aiStudentId, string asHealthDetails)
       {
           moHealthDetailsDC.SaveStudentHealthDetails(aiStudentId, asHealthDetails);
       }

       /// <summary>
       /// This method is used for Submit students health details.
       /// </summary>
       /// <param name="aiStudentId"></param>
       /// <param name="aiIsPublish"></param>
       public void SubmitStudentHealthDetails(int aiStudentId, int aiIsPublish)
       {
           moHealthDetailsDC.SubmitStudentHealthDetails(aiStudentId, aiIsPublish);
       }

       /// <summary>
       /// This method is used to get student details for Import health details.
       /// </summary>
       /// <param name="aiStandardId"></param>
       /// <param name="aiDivisionId"></param>
       /// <returns></returns>
       public List<ImportHealthDetails> GetStudentDetailsForImport(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asDivisionId, string asFilter, int maximumRows, int startRowIndex)
       {
           List<ImportHealthDetails> lstImportHealthDetails = new List<ImportHealthDetails>();

           string asSortExpression = "vwSD.Original_Standard_Id, vwSD.Original_Division_Id, YWSD.Roll_No";

           int iEndIndex = startRowIndex + maximumRows;
           lstImportHealthDetails = moHealthDetailsDC.GetStudentDetailsForImport(aiSchoolId, aiAcademicYearId, asStandardId.ToInt(), asDivisionId.ToInt(), asFilter, asSortExpression, startRowIndex, iEndIndex);

           if (lstImportHealthDetails.Count > Constants.I_ZERO)
               miTotalCount = lstImportHealthDetails[0].TotalRows;

           return lstImportHealthDetails;
       }

       /// <summary>
       /// This method is used to return total students count.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiStandardId"></param>
       /// <param name="aiDivisionId"></param>
       /// <param name="asSortExpression"></param>
       /// <param name="asSortDirection"></param>
       /// <returns></returns>
       public int Count(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asDivisionId, string asFilter)
       {
           return miTotalCount;
       }

       /// <summary>
       /// This method is used to Insert health details for multipal students(Import).
       /// </summary>
       /// <param name="aiUpdatedById"></param>
       /// <param name="asStudentHealthDetails"></param>
       public void InsertMultipalStudentHealthDetails(int aiUpdatedById, string asStudentHealthDetails)
       {
           moHealthDetailsDC.InsertMultipalStudentHealthDetails(aiUpdatedById, asStudentHealthDetails);
       }

       public List<SiblingStudentDetails> GetSiblingStudentDetails()
       {
           return moHealthDetailsDC.GetSiblingStudentDetails();
       }

       #endregion
   }
}
