using System;
using System.Data;
using DataCommunicator;
using Utility;
using System.IO;

namespace BusinessLogic
{
    public class TeacherTransferBL
    {
        #region constants
        //table indices

        const int I_TBL_CLASSTEACHER = 0;
        const int I_TBL_SRCSUBJECTTEACHER = 1;
        const int I_TBL_TARGETSUBJECTTEACHER = 2;

        const int I_TBL_TTWEEKDAY = 4;
        const int I_TBL_TEACHERSTT = 3;
        const int I_TBL_MAINTT = 5;
        const int I_TBL_LECTURES = 6;
        const int I_TBL_MAXLECTURES = 7;
        const int I_TBL_ASSEMBLY = 8;
        //field names
        const string S_FLD_CANTRANSFER = "CanTransfer";
        const string S_DB_COL_ROWSTATE = "RowState";
        const string S_FLD_SUBJECTTEACHERID = "Teacher_Subject_Id";
        #endregion

        #region Properties
        private DataSet moDSTeacherTransfer;

        private string msSrcClassTeacherTransferMsg;
        private string msTargetClassTeacherTransferMsg;

        private string msSrcSubjectTransferMsg;
        private string msTargetSubjectTransferMsg;

        private string msTransferTTMsg;

        public int miSrcTeacher;
        public int miTargetTeacher;

        public DataSet TeacherTransferDS
        {
            get { return moDSTeacherTransfer; }
        }

        public string SrcClassTeacherTransferMsg
        {
            get { return msSrcClassTeacherTransferMsg; }
        }

        public string TargetClassTeacherTransferMsg
        {
            get { return msTargetClassTeacherTransferMsg; }
        }

        public string SrcSubjectTransferMsg
        {
            get { return msSrcSubjectTransferMsg; }
        }
        public string TargetSubjectTransferMsg
        {
            get { return msTargetSubjectTransferMsg; }
        }

        public string TransferTTMsg
        {
            get { return msTransferTTMsg; }
        }

        #endregion
        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcadId"></param>
        /// <param name="aiSrcTeacher"></param>
        /// <param name="aiTargetTeacher"></param>
        /// <param name="abIsSrc"></param>
        /// <param name="aoDs"></param>
        public TeacherTransferBL(int aiSrcTeacher, int aiTargetTeacher, DataSet aoDs)
        {

            miSrcTeacher = aiSrcTeacher;
            miTargetTeacher = aiTargetTeacher;
            if (aoDs != null)
            {
                moDSTeacherTransfer = aoDs;
                SetPrimaryKey();
            }
        }
        /// <summary>
        /// This method sets the primary key of the subject teacher datatables.
        /// </summary>
        private void SetPrimaryKey()
        {
            DataColumn[] oDtCols = new DataColumn[1];
            oDtCols[0] = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].Columns[S_FLD_SUBJECTTEACHERID];
            moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].PrimaryKey = oDtCols;
            DataColumn[] oDtTargetCols = new DataColumn[1];
            oDtTargetCols[0] = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER].Columns[S_FLD_SUBJECTTEACHERID];
            moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER].PrimaryKey = oDtTargetCols;
        }
        /// <summary>
        /// This method 
        ///     1. Creates xmls for the reuired datatables.
        ///     2. Calls a method to save the data in datatable.
        /// 
        /// </summary>
        public void SaveTransfer(int aiSchoolId, int aiAcadId)
        {

            StringWriter oClassStr = new StringWriter();
            moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].TableName = "ClassTeacher";
            moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].TableName = "SubjectTeacher";
            moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER].TableName = "TargetSubjectTeacher";
            moDSTeacherTransfer.Tables[I_TBL_MAINTT].TableName = "TT";
            PrepareTransferTTForSave();
            moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].WriteXml(oClassStr);
            StringWriter oSrcSubjectStr = new StringWriter();
            moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].WriteXml(oSrcSubjectStr);
            StringWriter oTargetSubjectStr = new StringWriter();
            moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER].WriteXml(oTargetSubjectStr);

            StringWriter oTTStr = new StringWriter();
            moDSTeacherTransfer.Tables[I_TBL_MAINTT].WriteXml(oTTStr);

            StringWriter oAssemblyStr = new StringWriter();
            moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].WriteXml(oAssemblyStr);

            TeacherTransferDC obj = new TeacherTransferDC(aiSchoolId, aiAcadId, miSrcTeacher, miTargetTeacher);
            obj.SaveTeacherTransfer(oClassStr.ToString(), oSrcSubjectStr.ToString(), oTargetSubjectStr.ToString(), oTTStr.ToString(), oAssemblyStr.ToString());
        }
        /// <summary>
        /// This method modifies timetable data for saving.
        ///     1. Target teacher lectures with rowstate other than original will be deleted. So mark the row state as "Updated".
        ///      2. No operation would be performed on the rows with rowstate "Original" Hence remove them from Datatable.
        /// </summary>
        private void PrepareTransferTTForSave()
        {
            DataRow[] oDrTargetTeacher = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select("Teacher_Id=" + miTargetTeacher.ToString() + " AND " + S_DB_COL_ROWSTATE + "<>'" + Constants.S_ORIGINAL + "' AND " + S_DB_COL_ROWSTATE + "<> '" + Constants.S_DELETED + "'");
            int i;
            if (oDrTargetTeacher.Length > 0)
            {
                for (i = 0; i < oDrTargetTeacher.Length; i++)
                {
                    oDrTargetTeacher[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                }
            }
            DataRow[] oDrOrig = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(S_DB_COL_ROWSTATE + "='" + Constants.S_ORIGINAL + "'");
            if (oDrOrig.Length > 0)
            {
                for (i = 0; i < oDrOrig.Length; i++)
                {
                    moDSTeacherTransfer.Tables[I_TBL_MAINTT].Rows.Remove(oDrOrig[i]);
                }
            }
            moDSTeacherTransfer.Tables[I_TBL_MAINTT].AcceptChanges();
        }

        /// <summary>
        /// This method retrives the data(with transfer status)for selected teachers. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcadId"></param>
        /// <param name="asConfig"></param>
        /// <returns></returns>
        public DataSet GetTeacherTransfer(int aiSchoolId, int aiAcadId, string asConfig)
        {
            TeacherTransferDC obj = new TeacherTransferDC(aiSchoolId, aiAcadId, miSrcTeacher, miTargetTeacher);
            return obj.GetTeacherTransfer(aiSchoolId, aiAcadId, asConfig);
        }

        /// <summary>
        /// This method is called when the class teacher assignments change.
        /// It makes appropriate changes to the timetable.
        /// </summary>
        /// <param name="asTeacherId"> teacher Id of the class teacher whose assignment is to be modified</param>
        /// <param name="bIsSrc">  The parameter decides the row index of the row to be chosen.
        /// true: If it is source teacher
        /// false: if target teacher
        /// </param>
        /// <returns></returns>
        public int PreapareTransferTTForClassTeacher(string asTeacherId, bool abIsSrc)
        {
            if (abIsSrc)
            {
                return PreapareTransferTTForSrcClassTeacher();
            }
            else
            {
                return PreapareTransferTTForTargetClassTeacher();
            }
        }
        /// <summary>
        /// This method modifies class teacher assignment according to changes in class teacher assignment.
        /// Change in class teacher assignment affects First lecture in time table.
        /// If the assignment is transferred =>
        ///     1. If the class subject(of 1st lecure) is also transferred the lecture is transfferred.
        ///     2. If the class subject(of 1st lecure) is not transferred the lecture is not transfferred.
        /// If the assignment is removed =>
        ///     1. The first lecture is also removed.
        /// </summary>
        /// <returns></returns>
        public int PreapareTransferTTForSrcClassTeacher()
        {
            DataTable oDtMainTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT];
            const string S_FLD_TEACHERID = "Teacher_Id";
            const string S_FLD_LECTURENO = "Lecture_Number";

            DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
            DataRow oDrClassteacher;
            string sRowState;
            int i;

            string sFilter;
            sFilter = S_FLD_TEACHERID + "=" + miSrcTeacher.ToString()
                       + " AND " + S_FLD_LECTURENO + "=1";
            DataTable oDtSubjectTeacher;

            oDrClassteacher = oDtClassTeacher.Rows[0];
            oDtSubjectTeacher = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];
            DataTable oDtTargetSubjectTeacher = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];
            sRowState = oDrClassteacher[S_DB_COL_ROWSTATE].ToString();
            //get the rows to be updated
            DataRow[] oDrTT = oDtMainTT.Select(sFilter);
            DataRow[] oDrTargetTT;
            DataRow oDrTargetSubject;
            string sCurrentFilter;
            if (sRowState.Equals(Constants.S_UPDATED))//transferred
            {
                //loop through the lectures
                for (i = 0; i < oDrTT.Length; i++)
                {
                    string sSubjectTeacherId = oDrTT[i][S_FLD_SUBJECTTEACHERID].ToString();
                    DataRow oDrSubject = oDtSubjectTeacher.Rows.Find(sSubjectTeacherId);
                    string sSubjectRowState = oDrSubject[S_DB_COL_ROWSTATE].ToString();
                    if (sSubjectRowState.Equals(Constants.S_UPDATED))//if subject assignment is transferred
                    {
                        //transfer the lecture
                        oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;

                        sCurrentFilter = "Teacher_Id = " + miTargetTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                        //get the corresponding lecture of target teacher
                        oDrTargetTT = oDtMainTT.Select(sCurrentFilter);
                        if (oDrTargetTT.Length > 0)
                        {
                            oDrTargetTT[0][S_DB_COL_ROWSTATE] = Constants.S_UPDATEDEL;
                        }
                    }
                    else //subject is not transferred , remove the lecture
                    {
                        oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                    }
                }
                UpdateTargetTTIfRemoveClass();
            }
            else //original
            {
                for (i = 0; i < oDrTT.Length; i++)
                {
                    string sSubjectTeacherId = oDrTT[i][S_FLD_SUBJECTTEACHERID].ToString();
                    DataRow oDrSubject = oDtSubjectTeacher.Rows.Find(sSubjectTeacherId);
                    string sSubjectRowState = oDrSubject[S_DB_COL_ROWSTATE].ToString();
                    if (sSubjectRowState.Equals(Constants.S_ORIGINAL))//if subject assignment is not changed
                    {
                        oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                    }
                    else
                    {
                        oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                    }
                    sCurrentFilter = "Teacher_Id = " + miTargetTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                    //get corrsponding target lecture.
                    oDrTargetTT = oDtMainTT.Select(sCurrentFilter);
                    if (oDrTargetTT.Length > 0)
                    {
                        oDrTargetSubject = oDtTargetSubjectTeacher.Rows.Find(oDrTargetTT[0][S_FLD_SUBJECTTEACHERID].ToString());
                        sSubjectRowState = oDrTargetSubject[S_DB_COL_ROWSTATE].ToString();

                        if (!sSubjectRowState.Equals(Constants.S_DELETED))
                        {
                            oDrTargetTT[0][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                        }
                        else
                        {
                            oDrTargetTT[0][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                        }
                    }
                }
                UpdateTargetTTIflClassNotRemoved();
            }
            oDtMainTT.AcceptChanges();
            return oDrTT.Length;
        }
        /// <summary>
        /// This method changes Timetable when class teacher is transferred.
        /// </summary>
        private int UpdateTargetTTIfRemoveClass()
        {
            int i;
            string sFilter;
            DataRow[] oDrTT;
            DataTable oDtMainTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT];
            const string S_FLD_LECTURENO = "Lecture_Number";
            const string S_FLD_TEACHERID = "Teacher_Id";
            sFilter = S_FLD_TEACHERID + "=" + miTargetTeacher.ToString()
                 + " AND " + S_FLD_LECTURENO + "=1";
            oDrTT = oDtMainTT.Select(sFilter);
            for (i = 0; i < oDrTT.Length; i++)
            {
                if (!oDrTT[i][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_UPDATEDEL))
                {
                    oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                }
            }
            return oDrTT.Length;
        }
        /// <summary>
        /// This method changes Timetable when target class teacher is not removed.
        /// </summary>
        private int UpdateTargetTTIflClassNotRemoved()
        {
            const string S_FLD_LECTURENO = "Lecture_Number";
            const string S_FLD_TEACHERID = "Teacher_Id";
            string sFilter = S_FLD_TEACHERID + "=" + miTargetTeacher.ToString()
                     + " AND " + S_FLD_LECTURENO + "=1";
            DataRow[] oDrTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sFilter);
            for (int i = 0; i < oDrTT.Length; i++)
            {
                DataRow oDrSubjectTeacher = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER].Rows.Find(oDrTT[i][S_FLD_SUBJECTTEACHERID]);
                if (!oDrSubjectTeacher[S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_DELETED))
                {
                    oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                }
                else
                {
                    oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                }
            }
            return oDrTT.Length;
        }
        /// <summary>
        /// This method modifies timetable according to changes in class teacher assignments. 
        /// </summary>
        /// <returns></returns>
        public int PreapareTransferTTForTargetClassTeacher()
        {
            DataRow oDrClassteacher;
            string sRowState;
            int iReturn = 0;
            oDrClassteacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].Rows[1];
            sRowState = oDrClassteacher[S_DB_COL_ROWSTATE].ToString();

            if (sRowState.Equals(Constants.S_DELETED))
            {
                iReturn = UpdateTargetTTIfRemoveClass();
            }
            else //original
            {
                iReturn = UpdateTargetTTIflClassNotRemoved();
            }
            return iReturn;
        }

        /// <summary>
        /// This method is called when the subject teacher assignments change.
        ///  It makes appropriate changes to the timetable.
        /// </summary>
        /// <param name="asSubjectTeacherId">
        /// primary Id of the subject teacher table.
        /// </param>
        /// <param name="bIsSrc"> The parameter decides the table index of the datatble to be chosen.
        /// true: If it is source teacher
        /// false: if target teacher
        /// </param>
        public void PreapareTransferTTForSrcSubjects(string asSubjectTeacherId)
        {
            //DataTable oDtMainTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT];

            string sFilterClassSubject = S_FLD_SUBJECTTEACHERID + "=" + asSubjectTeacherId;
            object[] objSubject = new object[1];
            objSubject[0] = asSubjectTeacherId;
            // get the  row corresponding to updated assignment
            DataRow oDrSubjectTeacher = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].Rows.Find(objSubject);
            if (oDrSubjectTeacher != null)
            {
                string sRowState = oDrSubjectTeacher[S_DB_COL_ROWSTATE].ToString();
                if (sRowState.Equals(Constants.S_UPDATED))//transferred
                {
                    TransferTTIfSubjectTransfer(asSubjectTeacherId);
                }
                else //original
                {
                    TransferTTIfCancelTransfer(asSubjectTeacherId);
                }
                moDSTeacherTransfer.Tables[I_TBL_MAINTT].AcceptChanges();
            }
        }
        /// <summary>
        /// This method changes timetable if the subject is transferred from src to target.
        /// </summary>
        /// <param name="asSubjectTeacherId"></param>
        private void TransferTTIfSubjectTransfer(string asSubjectTeacherId)
        {
            string sFilter = S_FLD_SUBJECTTEACHERID + "=" + asSubjectTeacherId;

            //get all the related timetable entries.
            DataRow[] oDrTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sFilter);
            DataRow[] oDrTargetTT;
            DataRow[] oDrSrcTT;
            for (int i = 0; i < oDrTT.Length; i++)
            {
                oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                string sCurrentFilter = "Teacher_Id = " + miTargetTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                //get corrsponding target lecture.
                oDrTargetTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sCurrentFilter);

                sFilter = "Teacher_Id = " + miSrcTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                oDrSrcTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sFilter);

                if (oDrSrcTT.Length > 0)
                {
                    for (int iCnt = 0; iCnt < oDrSrcTT.Length; iCnt++)
                        oDrSrcTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                }

                if (oDrTargetTT.Length > 0)
                {
                    for (int iCnt = 0; iCnt < oDrTargetTT.Length; iCnt++)
                    {
                        //deleted
                        if (oDrTargetTT[iCnt][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_DELETED))
                        {
                            oDrTargetTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_UPDATEDEL;
                        }
                        else if (!oDrTargetTT[iCnt][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_DELETED))
                        {
                            //overwritten
                            oDrTargetTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  This method changes timetable if the subject is not transferred from src to target.
        /// </summary>
        /// <param name="asSubjectTeacherId"></param>
        private void TransferTTIfCancelTransfer(string asSubjectTeacherId)
        {
            string sFilter = S_FLD_SUBJECTTEACHERID + "=" + asSubjectTeacherId;
            //get all the related timetable entries.
            DataRow[] oDrTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sFilter);
            DataRow[] oDrTargetTT;
            DataRow[] oDrSrcTT;
            //if target
            //updatedel => delete
            //else => original
            for (int i = 0; i < oDrTT.Length; i++)
            {
                //set to original
                oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                string sCurrentFilter = "Teacher_Id = " + miTargetTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                oDrTargetTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sCurrentFilter);
                sFilter = "Teacher_Id = " + miSrcTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                oDrSrcTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sFilter);

                if (oDrSrcTT.Length > 0)
                {
                    for (int iCnt = 0; iCnt < oDrSrcTT.Length; iCnt++)
                        oDrSrcTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                }

                if (oDrTargetTT.Length > 0)
                {
                    for (int iCnt = 0; iCnt < oDrTargetTT.Length; iCnt++)
                    {
                        if (oDrTargetTT[iCnt][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_UPDATEDEL))
                        {
                            oDrTargetTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                        }
                        else if (oDrTargetTT[iCnt][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_DELETED))
                        {
                            oDrTargetTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                        }
                        else
                        {
                            oDrTargetTT[iCnt][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// This method is called when user changes class subject assignment of src teacher.
        /// It updates the rowstate of 1st lecture from timetable according to class teacher assignment and class subject assignment changes.
        /// if subject is not transferred : 
        ///     1. If class is  transferred : delete 1st lecture of src teacher (rowstate = Deleted)
        ///     2. If class is  not transferred : keep the 1st lecture (rowstate = Original)
        /// 
        /// </summary>
        /// <param name="asSubjectTeacherId"></param>
        private void PrepareSrcFirstLectureTransfer(string asSubjectTeacherId)
        {

            string sFilter = S_FLD_SUBJECTTEACHERID + "=" + asSubjectTeacherId + " AND Lecture_Number=1";
            //get the row
            DataRow oDrSubject = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].Rows.Find(asSubjectTeacherId);
            string sRowState = oDrSubject[S_DB_COL_ROWSTATE].ToString();
            //get tt rows for 1st lecture
            DataRow[] oDrTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sFilter);
            string sSubjectRowState = oDrSubject[S_DB_COL_ROWSTATE].ToString();
            string sSrcClassRowState = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].Rows[0][S_DB_COL_ROWSTATE].ToString();
            string sTargetClassRowState = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].Rows[1][S_DB_COL_ROWSTATE].ToString();
            string sCurrentFilter;
            DataRow[] oDrTargetTT;
            if (sRowState.Equals(Constants.S_ORIGINAL))//subject not transferred
            {
                //loop through all the src teacher lectures
                for (int i = 0; i < oDrTT.Length; i++)
                {
                    //if class is transferred
                    if (sSrcClassRowState.Equals(Constants.S_UPDATED))
                    {
                        oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                    }
                    else
                    {
                        oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                    }
                    //get respective target teacher lecture
                    sCurrentFilter = "Teacher_Id = " + miTargetTeacher.ToString() + " AND Lecture_Number=" + oDrTT[i]["Lecture_Number"].ToString() + " AND Weekday_Name='" + oDrTT[i]["Weekday_Name"].ToString() + "'";
                    oDrTargetTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select(sCurrentFilter);
                    if (oDrTargetTT.Length > 0)
                    {
                        if (oDrTargetTT[0][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_UPDATEDEL))
                        {
                            oDrTargetTT[0][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                        }
                        else
                        {
                            if (sTargetClassRowState.Equals(Constants.S_ORIGINAL))
                            {
                                oDrTargetTT[0][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                            }
                            else
                            {
                                oDrTargetTT[0][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                            }
                        }
                    }
                }
            }
            else //subject  transferred
            {

                PreapareTransferTTForSrcClassTeacher();
            }
        }
        /// <summary>
        /// This is a wrapper method to call the methods to modify timetable .
        /// </summary>
        /// <param name="asSubjectTeacherId"></param>
        public void PreapareTransferTTForSubjects(string asSubjectTeacherId, bool abIsSrc)
        {
            //if source teacher
            if (abIsSrc)
            {
                PreapareTransferTTForSrcSubjects(asSubjectTeacherId);
                //PrepareSrcFirstLectureTransfer(asSubjectTeacherId);
            }
            else
            {
                PreapareTransferTTForTargetSubjects(asSubjectTeacherId);
                //PreapareTransferTTForTargetClassTeacher();
            }
        }
        /// <summary>
        /// This method modifies timetable according to changes in subject assignment of source teacher.
        /// </summary>
        /// <param name="asSubjectTeacherId">Teacher subject assignment id</param>
        private void PreapareTransferTTForTargetSubjects(string asSubjectTeacherId)
        {
            DataTable oDtMainTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT];

            DataTable oDtSubjectTeacher;
            oDtSubjectTeacher = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];
            string sFilterClassSubject = S_FLD_SUBJECTTEACHERID + "=" + asSubjectTeacherId;
            DataRow[] oDrSubjectTeacher = oDtSubjectTeacher.Select(sFilterClassSubject);
            string sFilter = sFilterClassSubject;
            DataRow[] oDrTT;//= oDtMainTT.Select(sFilter);
            if (oDrSubjectTeacher.Length > 0)
            {
                string sRowState = oDrSubjectTeacher[0][S_DB_COL_ROWSTATE].ToString();
                oDrTT = oDtMainTT.Select(sFilter);
                if (sRowState.Equals(Constants.S_DELETED))//subject removed from target
                {
                    for (int i = 0; i < oDrTT.Length; i++)
                    {
                        //if updated => updatedel
                        if (oDrTT[i][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_UPDATED))
                            oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_UPDATEDEL;
                        else
                            oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                    }
                }
                else //original
                {
                    oDrTT = oDtMainTT.Select(sFilter);
                    for (int i = 0; i < oDrTT.Length; i++)
                    {
                        if (oDrTT[i][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_UPDATEDEL))
                            oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                        else
                            oDrTT[i][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                    }
                }
            }

            oDtMainTT.AcceptChanges();
        }
        /// <summary>
        /// This method change the data according to the single change in class subject assignment made by the user.
        /// 1. If the assignment is transfered from source to target - (bsrc true and abIsChecked is true)
        ///      The rowstate is set to "Updated".
        /// 2. If the assignment transfere is cancelled - (bsrc true and abIsChecked is false)
        ///     The rowstate is set to "Original".
        /// 3. If the assignment is removed from source teacher - (bsrc false and abIsChecked is true)
        ///     The rowstate is set to "Deleted".
        /// 4. If the assignment removal from source teacher is cancelled - (bsrc false and abIsChecked is true)
        ///     The rowstate is set to "Original".
        /// </summary>
        /// <param name="asTeacherSubectId"></param>
        /// <param name="bIsChecked"></param>
        /// <param name="bIsSrc"></param>
        public void PreapareClassSubjectTransfer(string asTeacherSubectId, bool abIsChecked, bool abIsSrc)
        {
            DataTable oDtClassSubject;
            DataRow[] oDr;
            if (abIsSrc)
            {
                oDtClassSubject = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];

                oDr = oDtClassSubject.Select(S_FLD_SUBJECTTEACHERID + "=" + asTeacherSubectId);

                if (oDr.Length > 0)
                {
                    if (abIsChecked)   //transfer
                    {

                        oDr[0][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                        oDtClassSubject.AcceptChanges();
                    }
                    else //original(assignment not changed hence rowstate is set to original)
                    {
                        oDr[0][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                        oDtClassSubject.AcceptChanges();
                    }
                }
            }
            else //target
            {
                oDtClassSubject = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];

                oDr = oDtClassSubject.Select(S_FLD_SUBJECTTEACHERID + "=" + asTeacherSubectId);
                if (oDr.Length > 0)
                {
                    if (abIsChecked)   //remove assignment
                    {
                        oDr[0][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                        oDtClassSubject.AcceptChanges();
                    }
                    else // original(assignment not changed hence rowstate is set to original)
                    {
                        oDr[0][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                        oDtClassSubject.AcceptChanges();
                    }
                }
            }
        }
        /// <summary>
        /// This method will manipulate class teacher data according to changes in class teacher assignments(checkboxes). 
        /// The Rowstate field of the corresponding datatable is modified.
        /// if the class teacher assignment is transferred -
        ///     a. Source  teacher rowstate is set to "Updated"
        ///     b. Target  teacher rowstate is set to "Deleted" as the target teachers existing assignment gets removed.
        /// If the class teacher assignment is removed -
        ///     a. Source  teacher rowstate is set to "Original", (as nochange )
        ///     b. Target  teacher rowstate is set to "Deleted"
        /// </summary>
        /// <param name="abIsTransfer">True if the src class teacher assignment is to be transfrred to target teacher, false otherwise</param>
        /// <param name="abIsRemove">True if the target class teacher assignment is to be removed to target teacher, false otherwise</param>
        public void PreapareClassTracherTransfer(bool abIsTransfer, bool abIsRemove)
        {
            DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
            //check source
            if (abIsTransfer) //transfered
            {
                oDtClassTeacher.Rows[0][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
            }
            else
            {
                oDtClassTeacher.Rows[0][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
            }
            //check target
            if (abIsRemove) //removed
            {
                //oDtClassTeacher.Rows[1][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
				oDtClassTeacher.Rows[oDtClassTeacher.Rows.Count - 1][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
            }
            else
            {
                oDtClassTeacher.Rows[1][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
            }
        }
        /// <summary>
        /// This method changes the datatable for timetable according to the transfer action.
        /// If lecture is to be transferred from src to target, 
        /// the rowstate of src is marked "Updated" And rowstate of corresponding target lecture is marke "Deleted".
        /// If lecture is to be removed it is marked "Deleted".
        /// </summary>
        /// <param name="asWeekDayName"> weekday name of the lecture whose status is being changed.</param>
        /// <param name="asLectureNo">lecture number</param>
        /// <param name="bTransfer">True: if lecture is being transferred, false if the lecture should be removed.</param>
        public void PreapareTransferTTForLecture(string asWeekDayName, string asLectureNo, bool bTransfer)
        {
            const string S_FLD_WEEKDAYID = "WeekDays_Id";
            const string S_FLD_WEEKDAYNAME = "Weekday_Name";
            const string S_FLD_LECTURENO = "Lecture_Number";

            DataTable oDtTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT];
            DataTable oDtTargetSubjects = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];
            DataTable oDtWeekDay = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY];
            DataRow[] oDrWeekDay = oDtWeekDay.Select(S_FLD_WEEKDAYNAME + "='" + asWeekDayName + "'");
            string sWeekDayId = oDrWeekDay[0][S_FLD_WEEKDAYID].ToString();
            //get the lecture
            DataRow[] oDrLectures = oDtTT.Select(S_FLD_WEEKDAYNAME + "='" + asWeekDayName + "' AND " +
                                                 S_FLD_LECTURENO + "=" + asLectureNo + " AND Teacher_Id =" + miSrcTeacher.ToString());
            DataRow[] oDrTargetLectures = oDtTT.Select(S_FLD_WEEKDAYNAME + "='" + asWeekDayName + "' AND " +
                                                           S_FLD_LECTURENO + "=" + asLectureNo + " AND Teacher_Id =" + miTargetTeacher.ToString());
            // if not to transfer
            //mark the row state as "Deleted"
            if (!bTransfer)
            {
                for (int i = 0; i < oDrLectures.Length; i++)
                {
                    oDrLectures[i][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                    oDtTT.AcceptChanges();
                }
                //teacher subject id for the target lecture
                if (oDrTargetLectures.Length > 0)
                {
                    for (int iCount = 0; iCount < oDrTargetLectures.Length; iCount++)
                    {
                        if (oDrTargetLectures[iCount][S_DB_COL_ROWSTATE].Equals(Constants.S_UPDATEDEL))
                            oDrTargetLectures[iCount][S_DB_COL_ROWSTATE] = Constants.S_DELETED;
                        else
                            oDrTargetLectures[iCount][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                        oDtTT.AcceptChanges();
                    }
                }
            }
            // if To be transfered
            //mark the row state of src "Updated"
            else
            {
                for (int i = 0; i < oDrLectures.Length; i++)
                {
                    oDrLectures[i][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                    oDtTT.AcceptChanges();
                }
                //get corresponding row for target teacher
                //if exists mark it "Deleted"   
                if (oDrTargetLectures.Length > 0)
                {
                    for (int iCount = 0; iCount < oDrTargetLectures.Length; iCount++)
                    {
                        if (oDrTargetLectures[iCount][S_DB_COL_ROWSTATE].Equals(Constants.S_DELETED))
                            oDrTargetLectures[iCount][S_DB_COL_ROWSTATE] = Constants.S_UPDATEDEL;
                        else
                            oDrTargetLectures[iCount][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
                        oDtTT.AcceptChanges();
                    }
                }
            }
        }
        #region Preview Data manipulation
        /// <summary>
        /// This is a wrapper method to create a dataset for preview display.
        /// </summary>
        public void PreparePreview()
        {
            PreparePrevwClassTeacherTransfer();
            PreparePrevwSubjectTransfer();
            PreparePrevwTTTransfer();
            moDSTeacherTransfer.AcceptChanges();
        }
        /// <summary>
        /// This method modifies the timetable data for preview display.
        ///     1. Description field for all the deleted lectures of src teacher is set to Off.
        ///     2. All the deleted lectures of target teacher are removed from datatable.
        ///     3. If lecure is transferred from src to target -
        ///             a. the description field is set to Off.
        ///             b. A row for the target teacher is added.
        /// </summary>
        private void PreparePrevwTTTransfer()
        {
            //datatable of timetable
            DataTable oDtTT = moDSTeacherTransfer.Tables[I_TBL_MAINTT];
            int i;
            const string S_OFF = "Off";
            //get the deleted rows of src teacher
            DataRow[] oDrDeleted = oDtTT.Select(S_DB_COL_ROWSTATE + " = '" + Constants.S_DELETED + "' AND Teacher_Id= " + miSrcTeacher.ToString());
            //remove the rows
            for (i = 0; i < oDrDeleted.Length; i++)
            {
                //set the description field to off.
                oDrDeleted[i]["description"] = S_OFF;
            }

            //get the deleted rows of target teacher
            oDrDeleted = oDtTT.Select(S_DB_COL_ROWSTATE + " = '" + Constants.S_DELETED + "' AND Teacher_Id= " + miTargetTeacher.ToString());
            for (i = 0; i < oDrDeleted.Length; i++)
            {
                //set the description field to off.
                oDrDeleted[i]["description"] = S_OFF;
            }
            //get the deleted rows of target teacher
            oDrDeleted = oDtTT.Select(S_DB_COL_ROWSTATE + " = '" + Constants.S_UPDATEDEL + "' AND Teacher_Id= " + miTargetTeacher.ToString());
            for (i = 0; i < oDrDeleted.Length; i++)
            {
                //remove the rows.
                oDtTT.Rows.Remove(oDrDeleted[i]);
            }
            oDrDeleted = oDtTT.Select(S_DB_COL_ROWSTATE + " = '" + Constants.S_UPDATED + "' AND Teacher_Id= " + miTargetTeacher.ToString());
            for (i = 0; i < oDrDeleted.Length; i++)
            {
                //remove the rows.
                oDtTT.Rows.Remove(oDrDeleted[i]);
            }

            //get transferred lectures
            DataRow[] oDrTransferred = oDtTT.Select(S_DB_COL_ROWSTATE + " = '" + Constants.S_UPDATED + "' AND Teacher_Id= " + miSrcTeacher.ToString());
            for (i = 0; i < oDrTransferred.Length; i++)
            {
                // add corresponding lecture entry for target teacher
                DataRow oDr = oDtTT.NewRow();
                oDr.ItemArray = (object[])oDrTransferred[i].ItemArray.Clone();
                oDr[S_DB_COL_ROWSTATE] = Constants.S_ADDED;
                oDr["Teacher_Id"] = miTargetTeacher;
                oDtTT.Rows.Add(oDr);
                //make the existing lecture description to off
                oDrTransferred[i]["description"] = S_OFF;
            }
            oDtTT.AcceptChanges();
        }
        /// <summary>
        /// This method manipulates the class teacher assignment table for preview.
        /// if assignment is transferred : 
        /// If only removed : make row 2 class N/A.
        /// </summary>
        private void PreparePrevwClassTeacherTransfer()
        {
            DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
            const string S_FLD_CLASSNMAE = "StdDiv";
            const string S_NA = "N/A";
            //src row
            DataRow oDrSrc = oDtClassTeacher.Rows[0];
            DataRow oDrTarget = oDtClassTeacher.Rows[1];
            string sRowState = oDrSrc[S_DB_COL_ROWSTATE].ToString();
            //transfered
            if (sRowState.Equals(Constants.S_UPDATED))
            {
                //transfer the class
                oDrTarget[S_FLD_CLASSNMAE] = oDrSrc[S_FLD_CLASSNMAE];
                //cancel src assignment
                oDrSrc[S_FLD_CLASSNMAE] = S_NA;
            }
            else // not tranferred
            {
                sRowState = oDrTarget[S_DB_COL_ROWSTATE].ToString();
                //if src assignment removed
                if (sRowState.Equals(Constants.S_DELETED))
                {
                    oDrSrc[S_FLD_CLASSNMAE] = S_NA;
                }
            }
        }
        /// <summary>
        /// This method modifies subject data for preview display.
        ///     1. Gets the message to inform user of the changes.
        ///     2. If the subject is transferred from src to target teacher,
        ///         A datarow corrsponding to subject assignment for target teacher is added.
        /// </summary>
        private void PreparePrevwSubjectTransfer()
        {
            int i;
            DataTable oDtSrc = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];
            DataTable oDttarget = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];
            // get the transfered src subjects
            DataRow[] oDrTransfered = oDtSrc.Select(S_DB_COL_ROWSTATE + "='" + Constants.S_UPDATED + "'");
            for (i = 0; i < oDrTransfered.Length; i++)
            {
                //add into target 
                // Check if the same std-div subject is already assigned to target teacher then
                // do not add the row in target teacher.
                DataRow[] oTargetRows = oDttarget.Select("classSubjectName = '" + oDrTransfered[i]["classSubjectName"] + "'");
                if (oTargetRows.Length == 0)
                {
                    DataRow oDr = oDttarget.Rows.Add(oDrTransfered[i].ItemArray);
                    oDr[S_DB_COL_ROWSTATE] = Constants.S_ADDED;
                }
                else if (oTargetRows[0][S_DB_COL_ROWSTATE].ToString() == Constants.S_DELETED)
                {
                    DataRow oDr = oDttarget.Rows.Add(oDrTransfered[i].ItemArray);
                    oDr[S_DB_COL_ROWSTATE] = Constants.S_ADDED;
                }

                //remove from src
                // oDtSrc.Rows.Remove(oDrTransfered[i]);
                oDrTransfered[i][S_DB_COL_ROWSTATE] = Constants.S_UPDATED;
            }
            // get the deleted rows from target
            DataRow[] oDrDeleted = oDttarget.Select(S_DB_COL_ROWSTATE + "='" + Constants.S_DELETED + "'");
            oDtSrc.AcceptChanges();
            oDttarget.AcceptChanges();
        }
        #endregion
        #region Messages
        /// <summary>
        /// This is a wrapper method to call other methods to format the messages for preview.
        /// The method called from teacher transfer Preview class.
        /// </summary>
        public void PrepareMsgsForPreview()
        {
            PrepareClassTeacherMsg();
            PrepareSubjectMsg();
            ValidateTT();
        }
        /// <summary>
        /// This is a wrapper method to call other methods to format the messages. The method called from teacher transfer class.
        /// </summary>
        public void PrepareMsgsForTransfer()
        {
            PrepareClassTeacherMsg();
            ValidateTT();
        }
        /// <summary>
        /// This method prepares messages to intimate the changes in class teacher assignment.
        /// </summary>
        private void PrepareClassTeacherMsg()
        {
            const string S_NA = "N/A";
            const string S_FLD_CLASSNMAE = "StdDiv";
            //            oMsg.sClassTeacherTransferMsg = new string[2];
            DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
            string sSrcRowState = oDtClassTeacher.Rows[0][S_DB_COL_ROWSTATE].ToString();
			string sTargetRowState = oDtClassTeacher.Rows[oDtClassTeacher.Rows.Count-1][S_DB_COL_ROWSTATE].ToString();
            string sSrcTeacherName = moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT].Rows[0]["TeacherName"].ToString();
            string sTargetTeacherName = moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT].Rows[1]["TeacherName"].ToString();
            string sSrcClassName = oDtClassTeacher.Rows[0][S_FLD_CLASSNMAE].ToString();
			string sTargetClassName = oDtClassTeacher.Rows[oDtClassTeacher.Rows.Count-1][S_FLD_CLASSNMAE].ToString();

			string sSrcClasses=string.Empty;
			string sTargetClasses = string.Empty;
			if (oDtClassTeacher.Rows.Count > 2)
			{
				for (int iClassTeacherCount = 0; iClassTeacherCount < oDtClassTeacher.Rows.Count; iClassTeacherCount++)
				{
					if (oDtClassTeacher.Rows[iClassTeacherCount]["RowState"].ToString() == Constants.S_UPDATED)
						sSrcClasses += oDtClassTeacher.Rows[iClassTeacherCount][S_FLD_CLASSNMAE].ToString() + ", ";
					if (oDtClassTeacher.Rows[iClassTeacherCount]["RowState"].ToString() == Constants.S_DELETED)
						sTargetClasses += oDtClassTeacher.Rows[iClassTeacherCount][S_FLD_CLASSNMAE].ToString() + ", ";
				}

				sSrcClasses = sSrcClasses.TrimEnd(' ').TrimEnd(',');
				sTargetClasses = sTargetClasses.TrimEnd(' ').TrimEnd(',');
			}
			else
			{
				sSrcClasses = oDtClassTeacher.Rows[0][S_FLD_CLASSNMAE].ToString();
				sTargetClasses = oDtClassTeacher.Rows[1][S_FLD_CLASSNMAE].ToString();
			}
            //source teacher
            if (sSrcClassName.Equals(S_NA))
            {
                //oMsg.sClassTeacherTransferMsg[0] = sSrcTeacherName + " is not a class teacher.";
                msSrcClassTeacherTransferMsg = sSrcTeacherName + " " + CommonUtility.GetResourceValue("MsgTeacherTransfer");
            }
            else
            {
                if (sSrcRowState.Equals(Constants.S_UPDATED))//transferred
                {
                    msSrcClassTeacherTransferMsg = CommonUtility.GetResourceValue("MsgTheClass") + "<span class=\"ClsHilightBG ClsPaddingGen\"> " + sSrcClasses + "</span> " + CommonUtility.GetResourceValue("MsgWillTransferredTo") + " "+ sTargetTeacherName + " "+CommonUtility.GetResourceValue("MsgTeacherTransferBL") + ".";
                }
                else
                {
                    if (oDtClassTeacher.Rows[0][S_FLD_CANTRANSFER].ToString().Equals("N"))
                    {
                        msSrcClassTeacherTransferMsg = CommonUtility.GetResourceValue("MsgClassNotTransferred") + " " + sTargetTeacherName + ".";
                    }
                    else
                    {
                        msSrcClassTeacherTransferMsg = CommonUtility.GetResourceValue("MsgClassTeacherAssignment") + " " + sSrcTeacherName + " " + CommonUtility.GetResourceValue("MsgWillNotChanged");
                    }
                }
            }
            //target teacher
            if (sTargetClassName.Equals(S_NA))
            {
                msTargetClassTeacherTransferMsg = sTargetTeacherName + " " + CommonUtility.GetResourceValue("MsgTeacherTransfer");
            }
            else
            {
                if (sTargetRowState.Equals(Constants.S_DELETED))
                {
                    //oMsg.sClassTeacherTransferMsg[1] = "The previous assignment of "+ sTargetTeacherName + " is removed.";
                    msTargetClassTeacherTransferMsg = CommonUtility.GetResourceValue("MsgTheClass") + " <span class=\"ClsHilightBG ClsPaddingGen\"> " + sTargetClasses + "</span> " + CommonUtility.GetResourceValue("MsgClassRemoved") + " " + sTargetTeacherName + " " + CommonUtility.GetResourceValue("MsgTeacherTransferBL1") + ".";
                }
                else
                {
                    msTargetClassTeacherTransferMsg = CommonUtility.GetResourceValue("MsgClassTeacherAssignment") + " "+ sTargetTeacherName + " "+ CommonUtility.GetResourceValue("MsgWillNotChanged");
                }
            }
        }
        /// <summary>
        /// This method formats the message for changes made by the user.
        /// </summary>
        private void PrepareSubjectMsg()
        {
            const string S_FLD_SUBJECTNAME = "classSubjectName";
            string sSrcTeacherName = moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT].Rows[0]["TeacherName"].ToString();
            string sTargetTeacherName = moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT].Rows[1]["TeacherName"].ToString();

            //     oMsg.sSubjectTransferMsg = new string[3];
            DataTable oDtSrc = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];
            DataTable oDttarget = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];
            // get the transfered src subjects
            DataRow[] oDrTransfered = oDtSrc.Select(S_DB_COL_ROWSTATE + "='" + Constants.S_UPDATED + "'");
            int i;
            string sSubjectList = "";
            for (i = 0; i < oDrTransfered.Length; i++)
            {
                sSubjectList = sSubjectList + oDrTransfered[i][S_FLD_SUBJECTNAME].ToString() + ", ";
                msSrcSubjectTransferMsg = sSubjectList + " " + CommonUtility.GetResourceValue("MsgWillTransferredTo") + " "  + sTargetTeacherName + ".";
            }
            if (!String.IsNullOrEmpty(sSubjectList))
            {
                sSubjectList = sSubjectList.Substring(0, sSubjectList.LastIndexOf(','));
            }
            else
            {
                sSubjectList = CommonUtility.GetResourceValue("NoneOfTheSubjects");
                msSrcSubjectTransferMsg = sSubjectList + " " + CommonUtility.GetResourceValue("MsgWillTransferredTo") + " " + CommonUtility.GetResourceValue("MsgNot") + sTargetTeacherName + ".";
            }

            msSrcSubjectTransferMsg = sSubjectList + " " + CommonUtility.GetResourceValue("MsgWillTransferredTo") + " "+CommonUtility.GetResourceValue("MsgNot") + sTargetTeacherName + ".";
            // get the deleted rows from target
            DataRow[] oDrDeleted = oDttarget.Select(S_DB_COL_ROWSTATE + "='" + Constants.S_DELETED + "'");
            sSubjectList = "";
            for (i = 0; i < oDrDeleted.Length; i++)
            {
                sSubjectList = sSubjectList + oDrDeleted[i][S_FLD_SUBJECTNAME] + ", ";
            }
            if (!String.IsNullOrEmpty(sSubjectList))
            {
                sSubjectList = sSubjectList.Substring(0, sSubjectList.LastIndexOf(','));
                msTargetSubjectTransferMsg = sSubjectList + " " + CommonUtility.GetResourceValue("MsgWillRemovedTo") + " " + sTargetTeacherName + ".";
            }
            else
            {
                sSubjectList = CommonUtility.GetResourceValue("NoneOfTheSubjects");
                msTargetSubjectTransferMsg = CommonUtility.GetResourceValue("NoneOfTheSubjectsWillBeRemovedFrom") +" " + sTargetTeacherName + ".";
            }
            
        }
        /// <summary>
        /// This method validates timetable for max lectures per week for target teacher.
        /// As the source teacher lectures can only be lessened the validation for source teacher is not required.
        /// A table containing max lecture limit for target teacher is stored in the dataset.
        /// And the no. of lectures that would be assigned to target teacher if the changes take effect is calculated.
        /// If assigned lectures exceed the max lecture limit. an  error message is returned.
        /// </summary>
        /// <returns></returns>
        public void ValidateTT()
        {
            string sErr = string.Empty;
            //get max lectures
            int iMaxLectures = Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_MAXLECTURES].Rows[0]["MaxLectures"]);
            int iAssignedLecures = 0;
            string sTargetTeacherName = moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT].Rows[1]["TeacherName"].ToString();
            //get assigned lectures. calculated as : 
            //no of lectures transfered from src + the target teacher lectures the are not removed or overwritten
            iAssignedLecures = Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_MAINTT].Compute("COUNT(" + S_DB_COL_ROWSTATE + " )", "Teacher_Id= " + miSrcTeacher.ToString() + " AND " + S_DB_COL_ROWSTATE + " = '" + Constants.S_UPDATED + "' "));
            iAssignedLecures = iAssignedLecures + Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_MAINTT].Compute("COUNT(" + S_DB_COL_ROWSTATE + " )", "Teacher_Id= " + miTargetTeacher.ToString() + " AND " + S_DB_COL_ROWSTATE + " = '" + Constants.S_ORIGINAL + "' "));
            if (iAssignedLecures > iMaxLectures)
            {
                int iDiff = iAssignedLecures - iMaxLectures;
                //sErr = CommonUtility.GetResourceValue("MsgMaxLecturesPerWeekLimitOf") + " " + iMaxLectures.ToString() + " " + CommonUtility.GetResourceValue("MsgExceededFor") + " " + sTargetTeacherName + " " + CommonUtility.GetResourceValue("") + " " + iDiff + " " + CommonUtility.GetResourceValue("Msglectures");
                sErr = CommonUtility.GetResourceValue("MsgMaxLecturesPerWeekLimitOf") + " " + iMaxLectures.ToString() + " " + CommonUtility.GetResourceValue("MsgExceededFor") + " " + sTargetTeacherName + " " + iDiff + " " + CommonUtility.GetResourceValue("Msglectures");
            }
            msTransferTTMsg = sErr;
        }
        #endregion
    }
}
