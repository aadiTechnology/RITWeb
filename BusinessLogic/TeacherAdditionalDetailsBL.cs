// Class Name       :- TeacherAdditionalDetailsDC.cs
// Purpose          :- This class is used to Manage Teacher Additional Detials.
// Date Of creation :- 23/08/2018
// Author Name      :- Sonali Jatahr

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using ControlEntities;
using Utility;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SchoolEntities;
using System.Data;


namespace BusinessLogic
{
    public class TeacherAdditionalDetailsBL
    {       
        #region Data Member(s)

        private TeacherAdditionalDetailsDC moTeacherAdditionalDetailsDC;

        #endregion

        #region Constructor(s)

        public TeacherAdditionalDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
         {
             this.moTeacherAdditionalDetailsDC = new TeacherAdditionalDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
         }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to save data into database.
        /// </summary>
        /// <param name="oTeacherAdditionalDetails"></param>
        public void save(int aiTeacherId, string asTeacheAdditionalDetailsXML)
        {
            this.moTeacherAdditionalDetailsDC.save(aiTeacherId, asTeacheAdditionalDetailsXML);
        }

        /// <summary>
        /// This method is used to get the data to display on screen.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public List<TeacherAdditionalDetails> Get(int aiTeacherId)
        { 
            return this.moTeacherAdditionalDetailsDC.Get(aiTeacherId);
        }

        /// <summary>
        /// This Method is used to get all master data for Filling combobox.
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMasterDetailsForUDISEForm()
        {
            return this.moTeacherAdditionalDetailsDC.GetAllMasterDetailsForUDISEForm();
        }

        #endregion
    }
}
