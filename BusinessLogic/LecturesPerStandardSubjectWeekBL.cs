// File Name        : LecturesPerStandardSubjectWeekDC
// Purpose          : This class is used to manage LecturesPerStandardSubjectWeek details.
// Date Of creation : 2/29/2008
// Author Name      : Anugandha

using System.Data;
using System.Collections;
using Utility;
using DataCommunicator;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class LecturesPerStandardSubjectWeekBL
    {

        #region Data Members

        private LecturesPerStandardSubjectWeekDC.LecturesPerStandardSubjectWeekStruct moLecturesPerStandardSubjectWeekStruct;
        private LecturesPerStandardSubjectWeekDC moLecturesPerStandardSubjectWeekDC;
        private Constants.Action eAction;

        #endregion

        #region Constructor

        public LecturesPerStandardSubjectWeekBL()
        {
            moLecturesPerStandardSubjectWeekDC = new LecturesPerStandardSubjectWeekDC();
        }

        public LecturesPerStandardSubjectWeekBL(int miLecturesPerStandardSubjectWeekId)
        {
            moLecturesPerStandardSubjectWeekDC = new LecturesPerStandardSubjectWeekDC(miLecturesPerStandardSubjectWeekId);
            moLecturesPerStandardSubjectWeekStruct = moLecturesPerStandardSubjectWeekDC.LecturesPerStandardSubjectWeekStructDetails;
        }

        #endregion

        #region Properties

        public virtual int Lectures_Per_Standard_Subject_Week_Id
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miLecturesPerStandardSubjectWeekId;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miLecturesPerStandardSubjectWeekId = value;
            }
        }

		[XmlIgnore]
        public virtual int School_Id
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miSchoolId;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miSchoolId = value;
            }
        }
		[XmlIgnore]
        public virtual int Academic_Year_Id
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miAcademicYearId;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miAcademicYearId = value;
            }
        }

        public virtual int Standard_Subject_Id
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miStandardSubjectId;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miStandardSubjectId = value;
            }
        }

        public virtual int Max_Lectures_Per_Standard_Subject
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miMaxLecturesPerStandardSubject;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miMaxLecturesPerStandardSubject = value;
            }
        }
		[XmlIgnore]
        public virtual string Is_Deleted
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.msIsDeleted;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.msIsDeleted = value;
            }
        }
		[XmlIgnore]
        public virtual int Inserted_By_Id
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miInsertedById;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miInsertedById = value;
            }
        }
		[XmlIgnore]
        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.mdtInsertDate;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.mdtInsertDate = value;
            }
        }
		[XmlIgnore]
        public virtual int Updated_By_Id
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.miUpdatedById;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.miUpdatedById = value;
            }
        }
		[XmlIgnore]
        public virtual System.DateTime Update_Date
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct.mdtUpdateDate;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct.mdtUpdateDate = value;
            }
        }
		[XmlIgnore]
        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        #endregion

        #region Public Methods

        public string InsertLecturesPerStandardSubjectWeek()
        {
            moLecturesPerStandardSubjectWeekDC.LecturesPerStandardSubjectWeekStructDetails = moLecturesPerStandardSubjectWeekStruct;
            return moLecturesPerStandardSubjectWeekDC.InsertLecturesPerStandardSubjectWeek();
        }

        public string UpdateLecturesPerStandardSubjectWeek()
        {
            moLecturesPerStandardSubjectWeekDC.LecturesPerStandardSubjectWeekStructDetails = moLecturesPerStandardSubjectWeekStruct;
            return moLecturesPerStandardSubjectWeekDC.UpdateLecturesPerStandardSubjectWeek();
        }
        public static void CheckDependencies(Hashtable aoHash, int aiAcadYrId)
        {
            ReferenceBL obl = new ReferenceBL();
            string sMessage = obl.CheckDependencies(Constants.ReferenceId.WeeklyStdSubjectLectures, aoHash, aiAcadYrId);
            if (!string.IsNullOrEmpty(sMessage))
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
        }

        public DataTable GetStandardSubjectId(int aiStandardId, int aiSubjectId, int aiSchoolId)
        {
            return moLecturesPerStandardSubjectWeekDC.GetStandardSubjectId(aiStandardId, aiSubjectId, aiSchoolId);
        }

        public DataTable GetLectureCount(int aiStandardSubjectId, int aiSchoolId)
        {
            return moLecturesPerStandardSubjectWeekDC.GetLectureCount(aiStandardSubjectId, aiSchoolId);
        }

		/// <summary>
		/// This method is used to check if lecture count is changed to lower.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicyearId"></param>
		/// <param name="asXml"></param>
		/// <returns></returns>
		public static string CheckValidUpdatedCount(int aiSchoolId, int aiAcademicyearId, string asXml)
		{
			DataTable oDataTable = LecturesPerStandardSubjectWeekDC.CheckValidUpdatedCount(aiSchoolId, aiAcademicyearId, asXml);

			if (oDataTable.IsNull() || oDataTable.Rows.Count <= 0)
				return String.Empty;
			else
			{
				List<string> lstClassSubjects = new List<string>();
				foreach (DataRow row in oDataTable.Rows)
					lstClassSubjects.Add(row["ClassName"] + "-" + row["SubjectName"]);
				
				return String.Format("Maximum no. of lectures for following Class-Subject {0} cannot be reduced as Timetable already contains greater number of lectures.",
									  String.Join(", ", lstClassSubjects.ToArray()));
			}
		}
        #endregion
    }

    public class LecturesPerStandardSubjectWeekCollectionBL
    {
        #region Data Members

        LecturesPerStandardSubjectWeekCollectionDC olectureCollection = new LecturesPerStandardSubjectWeekCollectionDC();

        #endregion

        #region Public Methods

        public DataTable GetAllSubjectsforStandard(int aiSchoolId, int aiStandardId)
        {
            return olectureCollection.GetAllSubjectsforStandard(aiSchoolId, aiStandardId);
        }
        public DataSet GetStdSubjectLectures(int aiSchoolId, int aiAcadId)
        {
            return olectureCollection.GetStdSubjectLectures(aiSchoolId, aiAcadId);
        }

        #endregion

    }
}
