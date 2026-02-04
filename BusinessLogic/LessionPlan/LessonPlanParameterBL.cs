using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LessonPlanEntities;
using DataCommunicator.LessonPlan;
using System.Data;

namespace BusinessLogic.LessionPlan
{
   public class LessonPlanParameterBL
   {
       #region DataMenber(s)

       private LessonPlanParameterDC moLessonPlanParameterDC;

       #endregion

       #region Constructor (s)

       public LessonPlanParameterBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
       {
           this.moLessonPlanParameterDC = new LessonPlanParameterDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
       }

       #endregion

       #region Method(s)

       /// <summary>
       /// This Method is used to get Lesson Plan Type Details
       /// </summary>
       /// <returns></returns>
       public List<LessonPlanCategory> GetCategories()
       {
           return this.moLessonPlanParameterDC.GetCategories();
       }
       /// <summary>
       /// This Method is used to get Lesson Plan Applied Subjects Details
       /// </summary>
       /// <returns></returns>
       public List<LessonSubjectCategories> GetSubjectCategories()
       {
           return this.moLessonPlanParameterDC.GetSubjectCategories();
       }

       public DataTable GetParentLessonPlan(int aiCategoryId)
       {
           return moLessonPlanParameterDC.GetParentLessonPlan(aiCategoryId);
       }

       /// <summary>
       /// This method is used to delete Lesson Plan Parameter
       /// </summary>
       /// <param name="aiLessonPlanParameterId"></param>
       /// <param name="aiConfigId"></param>
       public void Delete(int aiLessonPlanParameterId, int aiConfigId)
       {
           this.moLessonPlanParameterDC.Delete(aiLessonPlanParameterId, aiConfigId);
       }

       /// <summary>
       /// This method is used to return all available parameters.
       /// </summary>
       /// <param name="aiYear"></param>
       /// <param name="aiSkillId"></param>
       /// <param name="aiPerformanceParameterId"></param>
       /// <returns></returns>
       public List<LessonPlanParameters> GetAll(int aiCategoryId, int aiSectionId)
       {
           return this.moLessonPlanParameterDC.GetAll(aiCategoryId, aiSectionId);
       }
        /// <summary>
        /// This method is used to save parametere details.
        /// </summary>
        /// <param name="aoPerformanceParameter"></param>
       public void Save(LessonPlanParameters aoLessonPlanParameters)
        {
            this.moLessonPlanParameterDC.Save(aoLessonPlanParameters);
        }

       /// <summary>
       /// This method is used to submit / un submit parameters of selected Lesson Plan category.
       /// </summary>
       /// <param name="aiYear"></param>
       /// <param name="aiSkillId"></param>
       /// <param name="abIsSubmit"></param>
       public void Submit(int aiLessonPlanCategoryId,bool abIsSubmit)
       {
           this.moLessonPlanParameterDC.Submit(aiLessonPlanCategoryId, abIsSubmit);
       }

       #endregion
   }
}
