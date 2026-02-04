using System;
using System.Xml;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SchoolEntities;
using System.Linq;
using System.Drawing;
using System.Threading;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;


public partial class TimeTableGenerationUI : SchoolBase
{
    #region Data Members

    DataTable oDTWeekDays, oDTStandardDivision, oDTTeachers, oDTSubjects;

    #endregion

    #region Event Handelers

    /// <summary>
    /// Page Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                btnDownload.Attributes.Add("onclick", "HideLabel()");
                valSumTimetable.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                ApplyMouseHoverEffect(new List<Button> { btnDownload, btnUpload });
                CheckPreCondition();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method checks the preconditons to generate Time Table.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AutoTimeTable);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible/hide page controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        trTimeTable.Visible = false;
    }
    /// <summary>
    /// Click event of Download Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            DownloadXML();
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Click event of Upload Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = Color.Red;
            if (flUploadXML.HasFile)
            {

                string fileExtension = Path.GetExtension(flUploadXML.PostedFile.FileName);
                if (fileExtension.ToLower() != ".xml")
                {
                    lblMessage.Text = "Invalid File. Please upload file with extension .xml";
                    return;
                }

                try
                {
                    List<TimeTableDetails> lsTimeTable = GenerateTimeTableList(miSchoolId, miAcademicYearId);
                    List<TimeTableDetails> lstMaster = new List<TimeTableDetails>();

                    //For filtering Master table (School_TimeTable_Master) records
                    foreach (TimeTableDetails time in lsTimeTable)
                    {
                        if (lstMaster.Where(ls => ls.Class == time.Class && ls.Day == time.Day).Count() <= 0)
                        {
                            lstMaster.Add(time);
                        }
                    }
                    //Looping through each Master table record to insert into DB
                    foreach (TimeTableDetails timeMaster in lstMaster)
                    {
                        IEnumerable<TimeTableDetails> lsFilter = lsTimeTable.Where(ls => ls.Class == timeMaster.Class && ls.Day == timeMaster.Day);
                        if (lsFilter.Count() > 0)
                        {
                            MemoryStream strm = new MemoryStream();
                            XmlWriterSettings settings = new XmlWriterSettings();
                            settings.Encoding = System.Text.Encoding.Unicode;
                            XmlWriter xwriter = XmlWriter.Create(strm, settings);
                            xwriter.WriteStartElement("Lectures");

                            //Generating XML for Details table (School_TimeTable_Details) records for inserting in single go.
                            foreach (TimeTableDetails flTime in lsFilter)
                            {
                                xwriter.WriteStartElement("Lecture");
                                xwriter.WriteElementString("Lecture_Number", flTime.Period.ToString());
                                xwriter.WriteElementString("Teacher_Id", flTime.TeacherID.ToString());
                                xwriter.WriteElementString("Subject_Id", flTime.SubjectID.ToString());
                                xwriter.WriteElementString("Is_Additional_Class", flTime.IsAdditional.ToString());
                                xwriter.WriteEndElement();
                            }
                            xwriter.WriteEndElement();
                            xwriter.Flush();
                            strm.Position = 0;
                            StreamReader streamReader = new StreamReader(strm);
                            string xmlDetails = streamReader.ReadToEnd();
                            xwriter.Close();
                            strm.Dispose();
                            streamReader.Dispose();
                            SchoolTimeTableMasterBL.GenerateSchoolTimeTable(miSchoolId, miAcademicYearId, timeMaster.StandardDivisionID, timeMaster.WeekDayID,miUserId, xmlDetails);
                        }
                    }
                    lblMessage.Text = "Time table uploaded successfully.";
                    lblMessage.ForeColor = Color.Blue;

                }
                catch (Exception ex)
                {
                    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
                    lblMessage.Text = "Failed to upload Time Table.";
                }
            }
            else
            {
                lblMessage.Text = "File should be selected to upload.";
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Downloads the Tearchers,Subjects,Standard-Divisions details as a XML file for generating Auto timetable
    /// </summary>
    private void DownloadXML()
    {
        try
        {
            DataSet oDS = SchoolBL.GetTimeTableDetails(miSchoolId,miAcademicYearId);
            const string S_ELEMENT = "element";
            string sAttribute;
            XmlDocument oDoc = new XmlDocument();
            XmlElement oRoot = oDoc.CreateElement("timetable");

            sAttribute = "importtype";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "database";
            oRoot.Attributes.Append(oAttr);

            sAttribute = "options";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "idprefix:MyApp";
            oRoot.Attributes.Append(oAttr);

            //For no of periods/lectures
            if (oDS.Tables[6].Rows.Count > 0)
            {
                XmlNode oXmlPeriodRootNode = oDoc.CreateNode(S_ELEMENT, "periods", "");
                sAttribute = "options";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "";
                oXmlPeriodRootNode.Attributes.Append(oAttr);

                sAttribute = "columns";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "period";
                oXmlPeriodRootNode.Attributes.Append(oAttr);
                int maxPeriods = Convert.ToInt32(oDS.Tables[6].Rows[0]["MaxPeriods"].ToString());
                for (int i = 1; i <= maxPeriods; i++)
                {
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "period", "");
                    sAttribute = "period";
                    oAttr = oDoc.CreateAttribute(sAttribute);
                    oAttr.Value = i.ToString();
                    oXmlNode.Attributes.Append(oAttr);
                    oXmlPeriodRootNode.AppendChild(oXmlNode);
                }

                oRoot.AppendChild(oXmlPeriodRootNode);

            }
            XmlNode oXmlTecherRootNode = oDoc.CreateNode(S_ELEMENT, "teachers", "");

            sAttribute = "options";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "";
            oXmlTecherRootNode.Attributes.Append(oAttr);

            sAttribute = "columns";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "id,name,short";
            oXmlTecherRootNode.Attributes.Append(oAttr);

            for (int iRowCount = 0; iRowCount <= oDS.Tables[0].Rows.Count - 1; iRowCount++)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "teacher", "");
                sAttribute = "id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[0].Rows[iRowCount]["Teacher_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "name";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[0].Rows[iRowCount]["TeacherName"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "short";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (oDS.Tables[0].Rows[iRowCount]["Last_Name"].ToString());
                oXmlNode.Attributes.Append(oAttr);

                oXmlTecherRootNode.AppendChild(oXmlNode);
            }

            oRoot.AppendChild(oXmlTecherRootNode);

            XmlNode oXmlClassRootNode = oDoc.CreateNode(S_ELEMENT, "classes", "");

            sAttribute = "options";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "";
            oXmlClassRootNode.Attributes.Append(oAttr);

            sAttribute = "columns";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "id,name,teacherid";
            oXmlClassRootNode.Attributes.Append(oAttr);

            for (int iRowCount = 0; iRowCount <= oDS.Tables[1].Rows.Count - 1; iRowCount++)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "class", "");
                sAttribute = "id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[1].Rows[iRowCount]["SchoolWise_Standard_Division_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "name";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[1].Rows[iRowCount]["ClassName"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "short";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (oDS.Tables[1].Rows[iRowCount]["ClassName"].ToString());
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "teacherid";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (oDS.Tables[1].Rows[iRowCount]["Teacher_Id"].ToString());
                oXmlNode.Attributes.Append(oAttr);

                oXmlClassRootNode.AppendChild(oXmlNode);

            }
            oRoot.AppendChild(oXmlClassRootNode);

            XmlNode oXmlSubjectsRootNode = oDoc.CreateNode(S_ELEMENT, "subjects", "");

            sAttribute = "options";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "";
            oXmlSubjectsRootNode.Attributes.Append(oAttr);

            sAttribute = "columns";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "id,name,short";
            oXmlSubjectsRootNode.Attributes.Append(oAttr);

            for (int iRowCount = 0; iRowCount <= oDS.Tables[2].Rows.Count - 1; iRowCount++)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "subject", "");
                sAttribute = "id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[2].Rows[iRowCount]["Subject_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "name";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[2].Rows[iRowCount]["Subject_Name"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "short";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (oDS.Tables[2].Rows[iRowCount]["Subject_Name"].ToString());
                oXmlNode.Attributes.Append(oAttr);

                oXmlSubjectsRootNode.AppendChild(oXmlNode);
            }

            if (Settings.IsMPTApplicable)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "subject", "");
                sAttribute = "id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "9999";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "name";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "MPT";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "short";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "MPT";
                oXmlNode.Attributes.Append(oAttr);

                oXmlSubjectsRootNode.AppendChild(oXmlNode);
            }

            if (Settings.IsAssemblyApplicable)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "subject", "");
                sAttribute = "id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "9998";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "name";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "Assembly";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "short";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "Assembly";
                oXmlNode.Attributes.Append(oAttr);

                oXmlSubjectsRootNode.AppendChild(oXmlNode);
            }

            oRoot.AppendChild(oXmlSubjectsRootNode);

            XmlNode oXmlLessonsRootNode = oDoc.CreateNode(S_ELEMENT, "lessons", "");

            sAttribute = "options";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "canadd";
            oXmlLessonsRootNode.Attributes.Append(oAttr);

            sAttribute = "columns";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = "id,subjectid,classids,groupids,studentids,teacherids,classroomids,periodspercard,periodsperweek,weeks";
            oXmlLessonsRootNode.Attributes.Append(oAttr);

            for (int iRowCount = 0; iRowCount <= oDS.Tables[3].Rows.Count - 1; iRowCount++)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "lesson", "");
                sAttribute = "id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[3].Rows[iRowCount]["lessonid"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "classids";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oDS.Tables[3].Rows[iRowCount]["Standard_Division_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "subjectid";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (oDS.Tables[3].Rows[iRowCount]["Subject_Id"].ToString());
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "teacherids";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (oDS.Tables[3].Rows[iRowCount]["Teacher_Id"].ToString());
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "periodspercard";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "1";
                oXmlNode.Attributes.Append(oAttr);

                string periodsPerWeek = "1";

                DataRow[] drDivisionSubject = oDS.Tables[4].Select("Standard_Division_Id=" + oDS.Tables[3].Rows[iRowCount]["Standard_Division_Id"].ToString() + " AND Subject_Id=" + oDS.Tables[3].Rows[iRowCount]["Subject_Id"].ToString());

                if (drDivisionSubject.Length > 0)
                {
                    DataRow[] drStdDivSubjectLectures = oDS.Tables[5].Select("Division_Subject_Id=" + drDivisionSubject[0]["SchoolWise_Division_Subject_Id"].ToString());
                    if (drStdDivSubjectLectures.Length > 0)
                    {
                        periodsPerWeek = drStdDivSubjectLectures[0]["Max_Lectures_Per_Standard_Subject"].ToString();
                    }
                }

                //Exclusive Teacher (two or more teacher for same subject)
                if (oDS.Tables[3].Rows[iRowCount]["IsExclusive"].ToString() == "True")
                {
                    //Check if we have more than one teacher for same subject
                    DataRow[] drExclusiveTeacher = oDS.Tables[3].Select("Standard_Division_Id=" + oDS.Tables[3].Rows[iRowCount]["Standard_Division_Id"].ToString() + " AND Subject_Id=" + oDS.Tables[3].Rows[iRowCount]["Subject_Id"].ToString() + " AND Teacher_Id <> " + oDS.Tables[3].Rows[iRowCount]["Teacher_Id"].ToString());
                    if (drExclusiveTeacher.Length > 0 && drExclusiveTeacher[0]["IsExclusive"].ToString() == "True")
                    {
                        XmlNodeList xExclusiveNode = oXmlLessonsRootNode.SelectNodes("//lesson[@classids='" + drExclusiveTeacher[0]["Standard_Division_Id"] + "'" + " and @subjectid='" + drExclusiveTeacher[0]["Subject_Id"] + "']");
                        if (xExclusiveNode != null && xExclusiveNode.Count > 0)
                        {
                            int assignedPeriod = 0;
                            //xExclusiveNode.Attributes["periodsperweek"].Value;
                            if (xExclusiveNode.Count == drExclusiveTeacher.Length)
                            {
                                foreach (XmlNode xNode in xExclusiveNode)
                                {
                                    assignedPeriod += Convert.ToInt32(xNode.Attributes["periodsperweek"].Value);
                                }
                                periodsPerWeek = (Convert.ToInt32(periodsPerWeek) - assignedPeriod).ToString();
                            }
                            else
                            {
                                periodsPerWeek = (Convert.ToInt32(periodsPerWeek) / (drExclusiveTeacher.Length + 1)).ToString();
                            }

                        }
                        else
                        {
                            periodsPerWeek = (Convert.ToInt32(periodsPerWeek) / (drExclusiveTeacher.Length + 1)).ToString();
                        }

                    }
                }
                else
                {
                    periodsPerWeek = "0";
                }
                sAttribute = "periodsperweek";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = periodsPerWeek;
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "groupids";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "weeks";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "1";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "classroomids";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "studentids";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "";
                oXmlNode.Attributes.Append(oAttr);

                oXmlLessonsRootNode.AppendChild(oXmlNode);
            }

            oRoot.AppendChild(oXmlLessonsRootNode);

            oDoc.AppendChild(oRoot);
            string xml = oDoc.InnerXml;
            string sFileName = "TimeTable_" + (DateTime.Now.ToShortDateString().Replace("/", "-")) + ".xml";
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "text/xml";
            Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}", sFileName));
            Response.Write(xml);
            Response.End();
        }
        catch (ThreadAbortException)
        { 
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Generate the collection of TimeTableDetails class from uploaded XML file
    /// </summary>
    /// <param name="miSchoolId">School ID</param>
    /// <param name="miAcademicYearId">Academic Year ID</param>
    /// <returns></returns>
    private List<TimeTableDetails> GenerateTimeTableList(int miSchoolId, int miAcademicYearId)
    {
        StreamReader reader = new StreamReader(flUploadXML.PostedFile.InputStream);
        XElement xRoot = XDocument.Parse(reader.ReadToEnd()).Root;
        reader.Close();
        reader.Dispose();

        XElement xeCards = xRoot.Element("cards");
        XElement xeLessons = xRoot.Element("lessons");

        PopulateDataMembers(miSchoolId, miAcademicYearId);

        List<TimeTableDetails> lsTimeTable = new List<TimeTableDetails>();

        foreach (XElement xChildCard in xeCards.Elements())
        {
            string lessonID = xChildCard.Attribute("lessonid").Value;
            XElement xLessonChild = xeLessons.XPathSelectElement(String.Format("//lesson[@id='{0}']", lessonID));
            string[] classIds = xLessonChild.Attribute("classids").Value.Split(',');
            if (classIds.Length > 1) //Additional Class
            {
                bool isAdditionalClassAssigned = false;

                for (int i = 0; i <= classIds.Length - 1; i++)
                {
                    TimeTableDetails objTimeTable = GetTimeTableObject(xRoot, xChildCard, xLessonChild, classIds[i].ToString());

                    //For Additional Class
                    if (i == 0)
                    {
                        //Check if we have any period on the particular day & period on specific std.div
                        TimeTableDetails objTimeTableCheck = lsTimeTable.Where(t => t.WeekDayID == objTimeTable.WeekDayID && t.Period == objTimeTable.Period && t.Class == objTimeTable.Class).FirstOrDefault();
                        if (objTimeTableCheck == null) //No record found so make it as a base class
                        {
                            objTimeTable.IsAdditional = 0;
                        }
                        else
                        {
                            if (objTimeTableCheck.IsAdditional == 0) //Record found. check if we allready have base class
                            {
                                objTimeTable.IsAdditional = 1;
                                isAdditionalClassAssigned = true;
                            }
                            else
                            {
                                objTimeTable.IsAdditional = 0;
                            }
                        }
                    }
                    else
                    {
                        if (isAdditionalClassAssigned)
                        {
                            objTimeTable.IsAdditional = 0;
                        }
                        else
                        {
                            objTimeTable.IsAdditional = 1;
                            isAdditionalClassAssigned = true;
                        }
                    }
                    lsTimeTable.Add(objTimeTable);
                }
            }
            else
            {
                TimeTableDetails objTimeTable = GetTimeTableObject(xRoot, xChildCard, xLessonChild, classIds[0].ToString());
                lsTimeTable.Add(objTimeTable);
            }
        }
        lsTimeTable.Sort();
        return lsTimeTable;
    }

    /// <summary>
    /// Populates object of TimeTableDetails class from uploaded XML file
    /// </summary>
    /// <param name="xRoot">Root Element of the uploaded XML File</param>
    /// <param name="xChildCard">Card XElement</param>
    /// <param name="xLessonChild">Lession XElement</param>
    /// <param name="classID">Class ID</param>
    /// <param name="dtWeekDays">Week Days datatable</param>
    /// <param name="dtStdDiv">Standard Division Datatable</param>
    /// <param name="dtTeachers">Teachers Datatable</param>
    /// <param name="dtSubjects">Subjects Datatable</param>
    /// <returns></returns>
    private TimeTableDetails GetTimeTableObject(XElement xRoot, XElement xChildCard, XElement xLessonChild, string classID)
    {
        XElement xeTeachers = xRoot.Element("teachers");
        XElement xeDays = xRoot.Element("days");
        XElement xeClasses = xRoot.Element("classes");
        XElement xeSubjects = xRoot.Element("subjects");
        XElement xeLessons = xRoot.Element("lessons");

        TimeTableDetails objTimeTable = new TimeTableDetails();
        objTimeTable.Period = Convert.ToInt32(xChildCard.Attribute("period").Value);
        objTimeTable.Day = xeDays.XPathSelectElement(String.Format("//day[@day='{0}']", xChildCard.Attribute("day").Value)).Attribute("name").Value;
        objTimeTable.Subject = xeSubjects.XPathSelectElement(String.Format("//subject[@id='{0}']", xLessonChild.Attribute("subjectid").Value)).Attribute("name").Value;
        objTimeTable.Teacher = xeTeachers.XPathSelectElement(String.Format("//teacher[@id='{0}']", xLessonChild.Attribute("teacherids").Value)).Attribute("name").Value;
        objTimeTable.Class = xeClasses.XPathSelectElement(String.Format("//class[@id='{0}']", classID)).Attribute("name").Value;
        objTimeTable.IsAdditional = 0;

        DataRow[] drWeek = oDTWeekDays.Select("WeekDay_name='" + objTimeTable.Day + "'");
        if (drWeek.Length > 0)
        {
            objTimeTable.WeekDayID = Convert.ToInt32(drWeek[0]["WeekDays_id"].ToString());
        }

        DataRow[] drStdDiv = oDTStandardDivision.Select("StandardDivision='" + objTimeTable.Class + "'");
        if (drStdDiv.Length > 0)
        {
            objTimeTable.StandardDivisionID = Convert.ToInt32(drStdDiv[0]["SchoolWise_Standard_Division_id"].ToString());
        }
        if (objTimeTable.Teacher.IndexOf("'") > 0)
        {
            objTimeTable.Teacher = objTimeTable.Teacher.Replace("'", "''");
        }
        DataRow[] drTeacher = oDTTeachers.Select("TeacherName='" + objTimeTable.Teacher + "'");
        if (drTeacher.Length > 0)
        {
            objTimeTable.TeacherID = Convert.ToInt32(drTeacher[0]["Teacher_Id"].ToString());
        }
        DataRow[] drSubject = oDTSubjects.Select("Subject_Name='" + objTimeTable.Subject + "'");
        if (drTeacher.Length > 0 && drSubject.Length > 0)
        {
            objTimeTable.SubjectID = Convert.ToInt32(drSubject[0]["Subject_Id"].ToString());
        }
        return objTimeTable;
    }

    /// <summary>
    /// Populates the WeekDays,Subjects,StandardDivision,Teachers Datatable
    /// </summary>
    /// <param name="miSchoolId"></param>
    /// <param name="miAcademicYearId"></param>
    private void PopulateDataMembers(int miSchoolId, int miAcademicYearId)
    {
        WeekDaysMasterBL objWeekDay = new WeekDaysMasterBL();
        oDTWeekDays = objWeekDay.GetConfiguredWeekDays(miSchoolId, miAcademicYearId);
        StandardDivisionCollectionBL objStdDiv = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        oDTStandardDivision = objStdDiv.GetAssociatedStandardsDivisions();
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        DataSet oDsTeacherStandard = oTeacherSubjectAssignmentBL.GetTeacherAndStandardForTT(miSchoolId, miAcademicYearId);
        oDTTeachers = oDsTeacherStandard.Tables[1];
        SubjectCollectionBL objSubject = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
        oDTSubjects = objSubject.GetAllSubject();
    }
    #endregion
}
