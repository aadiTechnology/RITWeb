// File Name    : AdditionalClassesInTimetablePopUp.aspx.cs
// Created By   : Pallavi
// Date         : 22/6/2009
// Description  : This class is used to assign additional classes to teacher in timetable for already assigned weekday/lecture number.

// Modified By :Rohini
// Date : 2 Aug 2012
// Description : Added functionality to add optional subject lectures for class.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic.Exceptions;
using BusinessLogic;
using SchoolEntities;
using Utility;

public partial class AdditionalClassesInTimetablePopUp : SchoolBase
{
    #region Constants

    private const string S_VIEWSTATE_TT_DATA = "TimeTableData";
    private const string S_WEEKDAY_ID = "Weekday_Id";
    private const string S_WEEKDAY_NAME = "WeekDay_Name";
    private const string S_LECT_NUMBER = "Lecture_Number";
    private const string S_CLASSSUBJECTNAME = "classSubjectName";
    private const int I_TABLE_INDEX_WEEKDAYS = 0;
    private const int I_TABLE_INDEX_LECTURES = 1;
    private const int I_TABLE_INDEX_CLASS_SUBJECTS = 2;
    private const int I_TABLE_INDEX_TT_DATA = 3;
    private const int I_TABLE_INDEX_ADITIONAL_LECT = 4;
    private const int I_TABLE_INDEX_EXCLUDE_LECT = 5;
    private const string S_LECTURE_ALREADY_EXIST = "Lecture already exists.";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill all control related to subject,standard,division and teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			   
            cmbWeekday.Focus();
            valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillAllComboboxes();
            }

            SetVisibilityOfControls();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save teacher subject assignment and classwise optional subject lectures.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState[S_VIEWSTATE_TT_DATA] != null)
            {
                lblError.Visible = false;
                DataSet oDsTimetable = (DataSet)ViewState[S_VIEWSTATE_TT_DATA];
                DataTable oDtTeacherSubjects = oDsTimetable.Tables[I_TABLE_INDEX_CLASS_SUBJECTS];
                string sSubTeacherName = hidSubjectTeacherName.Value.IsNullOrEmpty() ? cmbSubjectTeacher.SelectedItem.Text : hidSubjectTeacherName.Value;
                DataRow[] oArrRows = hidTeacherId.Value != Constants.S_ZERO ? oDtTeacherSubjects.Select("classSubjectName ='" + cmbClassSubject.SelectedValue.ToString() + "'") : oDtTeacherSubjects.Select("SubjectTeacher='" + sSubTeacherName + "'");


                // If teacherId is zero it means additional lectures for class and vice versa.
                if (oArrRows.Length > Constants.I_ZERO)
                {
                    int iTeacherId = Convert.ToInt32(hidTeacherId.Value) != 0 ? Convert.ToInt32(hidTeacherId.Value): Convert.ToInt32(oArrRows[0]["Teacher_Id"]);
                    if (!CheckDuplicateLecture(oArrRows, iTeacherId))
                    {
                        SchoolTimeTableMasterBL oTimeTable = GetSchoolTimeTableMasterBLObject();
                        string[] oArrXml = GetXMLForTeacherTimeTable();
                        DataSet oDS = null;
                        if (hidTeacherId.Value != Constants.S_ZERO)
                            oDS = oTimeTable.ManageTeacherTimeTable(
                                iTeacherId, oArrXml[0], oArrXml[1], true, hidWantToInrsCnt.Value.ToInt());
                        else
                            //oDS = oTimeTable.ManageTeacherTimeTable(iTeacherId, oArrXml[0], oArrXml[1], true);
                            oDS = oTimeTable.ManageClassTimeTable(oArrXml[0], oArrXml[1], true, hidWantToInrsCnt.Value.ToInt());
                        hidIsDataValid.Value = ValidateData(oDS).ToString();
                        string sQueryString = string.Empty;
                        if (hidTeacherId.Value != Constants.S_ZERO)
                             sQueryString = "TeacherId=" + hidTeacherId.Value;
                        else 
                             sQueryString = "StandardId=" + hidStandardId.Value + "&DivisionId=" + hidDivisionId.Value;
                        hidEncrypt.Value = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        finally
        {
            hidWantToInrsCnt.Value =Constants.S_ZERO;
        }
    }


    protected void btnIncreaseCnt_Click(object sender, EventArgs e)
    {
        try
        {
            hidWantToInrsCnt.Value =Constants.S_ONE;
            this.btnSubmit_Click(null, null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        
    }

    protected void cmbWeekday_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            DataTable oDTLectures = ((DataSet)ViewState[S_VIEWSTATE_TT_DATA]).Tables[I_TABLE_INDEX_LECTURES];
            DataRow[] oArrRows = oDTLectures.Select("Weekday_Id=" + Convert.ToInt32(cmbWeekday.SelectedValue));
            ControlUtility.FillDropDownList(oArrRows, ref cmbLectNumber, S_LECT_NUMBER, S_LECT_NUMBER, Constants.S_SELECT);
            cmbClassSubject.Items.Clear();
            cmbClassSubject.Items.Add(Constants.S_SELECT);
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbLectNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            DataTable oDTAditionalLectures = ((DataSet)ViewState[S_VIEWSTATE_TT_DATA]).Tables[I_TABLE_INDEX_ADITIONAL_LECT];
            DataRow[] oArrRows = oDTAditionalLectures.Select("Weekday_Id=" + Convert.ToInt32(cmbWeekday.SelectedValue) + " AND Lecture_Number = " + Convert.ToInt32(cmbLectNumber.SelectedValue));
            DataTable oDTTimeTable = ((DataSet)ViewState[S_VIEWSTATE_TT_DATA]).Tables[I_TABLE_INDEX_TT_DATA];
            oArrRows = oDTTimeTable.Select("Weekday_Id=" + Convert.ToInt32(cmbWeekday.SelectedValue) + " AND Lecture_Number = " + Convert.ToInt32(cmbLectNumber.SelectedValue));

            // If hidTeacherId is zero it means additional lecture for class other wise for teacher.
			
            string sClassSubject = hidTeacherId.Value == Constants.S_ZERO ? oArrRows[0][3].ToString() : string.Empty;
            int iSubjectId = Convert.ToInt32(oArrRows[0][2]);
            int iStdDivId = Convert.ToInt32(oArrRows[0][0].ToString());
            int iGroupId = hidTeacherId.Value == Constants.S_ZERO ? Convert.ToInt32(oArrRows[0][5].ToString()) : Constants.I_ZERO;
            int iSubjectGroupId = hidTeacherId.Value == Constants.S_ZERO ? Convert.ToInt32(oArrRows[0][6]) : Constants.I_ZERO;
            oDTTimeTable = ((DataSet)ViewState[S_VIEWSTATE_TT_DATA]).Tables[I_TABLE_INDEX_CLASS_SUBJECTS];

            DataTable oDTLecturesToExclude = ((DataSet)ViewState[S_VIEWSTATE_TT_DATA]).Tables[I_TABLE_INDEX_EXCLUDE_LECT];
            DataRow[] oArrDrNALectureNos = oDTLecturesToExclude.Select("Lecture_Number = " + cmbLectNumber.SelectedValue);

            string sStdDivToExcludeForLectNo = string.Empty;

            foreach (DataRow oRow in oArrDrNALectureNos)
                sStdDivToExcludeForLectNo += oRow["StandardDivision_Id"].ToString() + ",";
            string sFilterToExcludeStdDiv = string.Empty;

            if (!string.IsNullOrEmpty(sStdDivToExcludeForLectNo))
                sFilterToExcludeStdDiv = " AND Standard_Division_Id NOT IN (" + sStdDivToExcludeForLectNo.Substring(0, sStdDivToExcludeForLectNo.LastIndexOf(",")) + ")";

            if (!string.IsNullOrEmpty(hidTeacherId.Value) && hidTeacherId.Value != "0")
            {
                // Fill class subject combo box for adding additional lectures for teacher
                oArrRows = oDTTimeTable.Select("classSubjectName <> '" + sClassSubject + "' AND Standard_Division_Id <> " + iStdDivId + sFilterToExcludeStdDiv + " AND Subject_Id = " + iSubjectId, " Original_Standard_Id ASC, Original_Division_Id ASC ");
                ControlUtility.FillDropDownList(oArrRows, ref cmbClassSubject, S_CLASSSUBJECTNAME, S_CLASSSUBJECTNAME, Constants.S_SELECT);
            }
            else
            {
                // Fill subject teacher combobox for adding optional subject lectures for class.
                List<TimeTableDetails> lstOptionalSubject = SchoolTimeTableMasterBL.GetGroupwiseOptionalSubjectForTimeTable(miSchoolId, miAcademicYearId, iStdDivId, iGroupId, iSubjectGroupId);
                lstOptionalSubject = lstOptionalSubject.Where(obj => obj.SubjectID != iSubjectId).ToList();
                ListSource.FillDropDownList(lstOptionalSubject, cmbSubjectTeacher, "Teacher", "SubjectID", Constants.S_SELECT);
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    private bool ValidateData(DataSet oDs)
    {
        bool bReturn = true;
        string sMsg = string.Empty;
        string[] sArrMsgs = new string[5];
        if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
        {
            const string S_TEACHERWEEKLY_LECTURES_ERR_FIELD = "ErrMsgForWeeklyTeacherLectures";
            const string S_TEACHERWEEKDAY_LECTURES_ERR_FIELD = "ErrMsgForWeekDayTeacherLectures";
            const string S_SUBJECTWEEKDAY_LECTURES_ERR_FIELD = "ErrMsgForSubjectLectures";
			const string S_SUBJECTWEEKDAY_ASSLECTURES_ERR_FIELD = "ErrMsgForAssociateSubjectLectures";
			const string S_OVERLAP_ERR_FIELD = "OverlapErrorMessage";
            sArrMsgs[0] = oDs.Tables[0].Rows[0][S_TEACHERWEEKLY_LECTURES_ERR_FIELD].ToString();
            sArrMsgs[1] = oDs.Tables[0].Rows[0][S_TEACHERWEEKDAY_LECTURES_ERR_FIELD].ToString();
            sArrMsgs[2] = oDs.Tables[0].Rows[0][S_SUBJECTWEEKDAY_LECTURES_ERR_FIELD].ToString();
			sArrMsgs[3] = oDs.Tables[0].Rows[0][S_SUBJECTWEEKDAY_ASSLECTURES_ERR_FIELD].ToString();
			sArrMsgs[4] = oDs.Tables[0].Rows[0][S_OVERLAP_ERR_FIELD].ToString();
            sMsg = FormatErrorMessage(sArrMsgs);
        }

        if (!sMsg.Equals(string.Empty))
        {
            if (sArrMsgs[2]!= String.Empty && sMsg != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopup", "ShowPopup(this,'" + sArrMsgs[2].ToString() + "','" + sMsg.ToString() + "')", true);
                bReturn = false;
            }
            else if (sArrMsgs[2].ToString() == "" && sMsg != null)
            {
                lblError.Visible = true;
                lblError.Text = sMsg;
                bReturn = false;
            }	
            
            
        }

        return bReturn;
    }

    private string FormatErrorMessage(string[] sArrMsgs)
    {
        string sReturnMsg = string.Empty;
        for (int iIndex = 0; iIndex < sArrMsgs.Length; iIndex++)
            if (sArrMsgs[iIndex] != null && !sArrMsgs[iIndex].Equals(string.Empty))
            {
                if (!sReturnMsg.Equals(string.Empty))
                    sReturnMsg = sReturnMsg + "<BR>" + sArrMsgs[iIndex];
                else
                    sReturnMsg = sArrMsgs[iIndex];
            }

        return sReturnMsg;
    }

    private SchoolTimeTableMasterBL GetSchoolTimeTableMasterBLObject()
    {
        SchoolTimeTableMasterBL oTimeTable = new SchoolTimeTableMasterBL();
        DataSet oDSTimeTable = (DataSet)ViewState[S_VIEWSTATE_TT_DATA];
        DataTable oDTTeacherSubjects = oDSTimeTable.Tables[I_TABLE_INDEX_CLASS_SUBJECTS];

        DataRow[] oArrRows = hidTeacherId.Value != Constants.S_ZERO ? oDTTeacherSubjects.Select("classSubjectName ='" + cmbClassSubject.SelectedValue.ToString() + "'") : oDTTeacherSubjects.Select("SubjectTeacher ='" + hidSubjectTeacherName.Value + "'");
        int iStdDivId = Convert.ToInt32(oArrRows[0]["Standard_Division_Id"]);
		oTimeTable.AcademicYearId = miAcademicYearId;
		oTimeTable.SchoolId = miSchoolId;
		oTimeTable.InsertedById = miUserId;
        oTimeTable.StandardDivisionId = iStdDivId;
        return oTimeTable;
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
		if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
		{
			const string S_QUERYSTRING_TEACHER_ID = "TeacherId";
			const string S_QUERYSTRING_TEACHER_NAME = "TeacherName";

			if (QueryString[S_QUERYSTRING_TEACHER_ID] != null)
			{
				hidTeacherId.Value = QueryString[S_QUERYSTRING_TEACHER_ID];
				lblTeacherName.Text = QueryString[S_QUERYSTRING_TEACHER_NAME];
				hidStandardId.Value = Constants.S_ZERO;
				hidDivisionId.Value = Constants.S_ZERO;
                lblIndentDetails.Text = Resources.LocalizedResources.AssignAdditionalLecturesToTeacher;
			}
			else if (QueryString["StandardId"] != null)
			{
				hidTeacherId.Value = Constants.S_ZERO;
				hidStandardId.Value = QueryString["StandardId"];
				hidDivisionId.Value = QueryString["DivisionId"];
				lblClassName.Text = QueryString["Class"];
                lblIndentDetails.Text = Resources.LocalizedResources.AssignOptionalSubjectLecturesToClass;
			}
		}
    }

    /// <summary>
    /// This method is used to fill Class-Subject, weekday and lecture number comboboxes.
    /// </summary>
    private void FillAllComboboxes()
    {
        DataSet oDSTeacherTimeTable = SchoolTimeTableMasterBL.GetTeacherTimeTableForAdditionalClasses(miSchoolId, miAcademicYearId, Convert.ToInt32(hidTeacherId.Value), Convert.ToInt32(hidDivisionId.Value));
        ViewState[S_VIEWSTATE_TT_DATA] = oDSTeacherTimeTable;
        ControlUtility.FillDropDownList(oDSTeacherTimeTable.Tables[I_TABLE_INDEX_WEEKDAYS], ref cmbWeekday, S_WEEKDAY_ID, S_WEEKDAY_NAME, Constants.S_SELECT);

        cmbLectNumber.Items.Add(Constants.S_SELECT);
        cmbClassSubject.Items.Add(Constants.S_SELECT);
        cmbSubjectTeacher.Items.Add(Constants.S_SELECT);
    }

    /// <summary>
    /// This method creates an XML for Time table
    /// </summary>
    /// <returns>
    /// Array of xml strings.
    // 1. <DaywiseTimeTableMaster><DaywiseTimeTable Weekday_Id ="810"/></DaywiseTimeTableMaster>
    // 2. <DaywiseTimeTableDetails><DaywiseTimeTableDetail WeekDay_Id ="810" Lecture_Number ="1" Teacher_ID ="7" Subject_Id ="1303" />
    // <DaywiseTimeTableDetail WeekDay_Id ="810" Lecture_Number ="2" Teacher_ID ="7" Subject_Id ="1303" />
    // </DaywiseTimeTableDetails>
    /// </returns>
    private string[] GetXMLForTeacherTimeTable()
    {
        string[] sArrStrXml = new string[2];
        const string S_ELEMENT = "element";

        // This variable is set to
        // 1. true if the week day entry shud be made in master
        // 2. false otherwise
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("DaywiseTimeTableMaster");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTableMaster", string.Empty);

        XmlDocument oDocDetail = new XmlDocument();
        XmlElement rootDetail = oDocDetail.CreateElement("DaywiseTimeTableDetails");
        XmlElement DetailRootNode = oDocDetail.CreateElement("DaywiseTimeTableDetails");

        // create root level node.
        DataSet oDs = (DataSet)ViewState[S_VIEWSTATE_TT_DATA];
        DataTable oDtTeacherSubjects = oDs.Tables[I_TABLE_INDEX_CLASS_SUBJECTS];

        string sAtrrName;
        XmlAttribute attr;
        XmlNode oXmlNode;
        XmlNode oXmlDetailNode;

        int iWeekDayId = Convert.ToInt32(cmbWeekday.SelectedValue);
        string[] sSubject = cmbSubjectTeacher.SelectedItem.Text.Split('-');
        DataRow[] oArrRows = hidTeacherId.Value != Constants.S_ZERO ? oDtTeacherSubjects.Select("classSubjectName ='" + cmbClassSubject.SelectedValue.ToString() + "'") : oDtTeacherSubjects.Select("SubjectTeacher='" + hidSubjectTeacherName.Value + "'");

        int iStdDivId = Convert.ToInt32(oArrRows[0]["Standard_Division_Id"]);
        int iSubjectId = Convert.ToInt32(oArrRows[0]["Subject_Id"]);
        string sTeacherId = hidTeacherId.Value != Constants.S_ZERO ? hidTeacherId.Value : oArrRows[0]["Teacher_Id"].ToString();
        oXmlNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTable", string.Empty);

        sAtrrName = "Standard_Division_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iStdDivId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Weekday_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iWeekDayId.ToString();
        oXmlNode.Attributes.Append(attr);

        oXmlRootNode.AppendChild(oXmlNode);
        root.AppendChild(oXmlRootNode);

        oXmlDetailNode = oDocDetail.CreateNode(S_ELEMENT, "DaywiseTimeTableDetail", string.Empty);

        sAtrrName = "WeekDay_Id";
        attr = oDocDetail.CreateAttribute(sAtrrName);
        attr.Value = iWeekDayId.ToString();
        oXmlDetailNode.Attributes.Append(attr);

        sAtrrName = "Teacher_ID";
        attr = oDocDetail.CreateAttribute(sAtrrName);
        attr.Value = sTeacherId;
        oXmlDetailNode.Attributes.Append(attr);

        sAtrrName = "Standard_Division_Id";
        attr = oDocDetail.CreateAttribute(sAtrrName);
        attr.Value = iStdDivId.ToString();
        oXmlDetailNode.Attributes.Append(attr);

        sAtrrName = "Lecture_Number";
        attr = oDocDetail.CreateAttribute(sAtrrName);
        attr.Value = Convert.ToInt32(cmbLectNumber.SelectedValue).ToString();
        oXmlDetailNode.Attributes.Append(attr);

        sAtrrName = "Subject_Id";
        attr = oDocDetail.CreateAttribute(sAtrrName);
        attr.Value = iSubjectId.ToString();
        oXmlDetailNode.Attributes.Append(attr);

        DetailRootNode.AppendChild(oXmlDetailNode);
        rootDetail.AppendChild(DetailRootNode);

        sArrStrXml[0] = root.InnerXml;
        sArrStrXml[1] = rootDetail.InnerXml;
        return sArrStrXml;
    }

    /// <summary>
    /// This method is used to check weather lecture is already exist for same day and time.
    /// </summary>
    /// <param name="oArrRows"></param>
    /// <param name="iTeacherId"></param>
    /// <returns></returns>
    private bool CheckDuplicateLecture(DataRow[] aoArrRows, int aiTeacherId)
    {
        int iStdDivId = Convert.ToInt32(aoArrRows[0]["Standard_Division_Id"]);
        int iSubjectId = Convert.ToInt32(aoArrRows[0]["Subject_Id"]);
        int iLectureNo = cmbLectNumber.SelectedValue.ToInt();
		int iWeekDayId = cmbWeekday.SelectedValue.ToInt();
        SchoolTimeTableMasterBL oSchoolTimeTableMasterBL = new SchoolTimeTableMasterBL();
        string sErrorMsg= oSchoolTimeTableMasterBL.CheckDuplicateLecture(iSubjectId, aiTeacherId, miSchoolId, miAcademicYearId, iStdDivId, iLectureNo,iWeekDayId);
		if (!string.IsNullOrEmpty(sErrorMsg))
		{
			lblError.Visible = true;
			lblError.Text = sErrorMsg;
			return true;
		}
		else
			return false;
    }

    /// <summary>
    /// This method is used to show or hide subject teacher and class subject combo box.
    /// </summary>
    private void SetVisibilityOfControls()
    {
        trTeacherName.Visible = trCmbClassSubject.Visible = hidTeacherId.Value != Constants.S_ZERO;
        trClassName.Visible = trCmbSubTeacher.Visible = hidTeacherId.Value == Constants.S_ZERO;
        btnClose.Attributes.Add("onclick", "window.close();");
		ApplyMouseHoverEffect(new List<Button> { btnClose, btnSubmit,btnCancel,btnIncreaseCnt });
    }
    #endregion
}