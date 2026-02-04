using DataCommunicator;
using System.Data;

namespace BusinessLogic
{
   public class ParentHealthDetailsBL
   {
       #region Data Member(s)

       private ParentHealthDetailsDC moParentHealthDetailsDC = null;

       #endregion

       #region Constructor(s)

       public ParentHealthDetailsBL()
       {
           moParentHealthDetailsDC = new ParentHealthDetailsDC();
       }

       public ParentHealthDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
       {
           moParentHealthDetailsDC = new ParentHealthDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
       }

       #endregion

       #region Public Method(s)

       /// <summary>
       /// This method is used to save parent health details.
       /// </summary>
       /// <param name="aiYearwiseStudentId"></param>
       /// <param name="asParentHealthDetailsXML"></param>
       public void Save(int aiYearwiseStudentId, string asParentHealthDetailsXML)
       {
           moParentHealthDetailsDC.Save(aiYearwiseStudentId, asParentHealthDetailsXML);
       }

       /// <summary>
       /// This method is used to get parent health details.
       /// </summary>
       /// <param name="aiYearwiseStudentId"></param>
       /// <returns></returns>
       public DataTable GetParentHealthDetails(int aiYearwiseStudentId)
       {
           return this.moParentHealthDetailsDC.GetParentHealthDetails(aiYearwiseStudentId);
       }

       /// <summary>
       /// This method is used to submit parent health details.
       /// </summary>
       /// <param name="aiYearwiseStudentDetails"></param>
       public void Submit(int aiYearwiseStudentDetails)
       {
           this.moParentHealthDetailsDC.Submit(aiYearwiseStudentDetails);
       }

       #endregion
   }
}
