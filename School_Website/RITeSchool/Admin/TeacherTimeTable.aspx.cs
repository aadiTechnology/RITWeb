// File Name     : TeacherTimeTable.aspx.cs
// Modified By   : Amit
// Modified Date : 26 Sept 2009
// Description   : This class is used to set time table.
// Modified By : Rohini
// Removed additional lecture filter from filling combobox in grid.
// Date : 2 Aug 2012

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ExternalLectures;
using System.Resources;
using Utility;

/// <summary>
/// This class displayes the class (std-div) OR teacher time table.
/// </summary>
public partial class TeacherTimeTable : SchoolBase
{
	#region  " Constants "

	private const string S_CSSCLASS_COMBO = "TTCombo";
	private const string S_CSSCLASS_COMBO_SELECTED = "TTComboSelect";
	private const string S_CSS_NA = "TTNotAssignDark";
	private const string S_CSS_NOTCLASS_TEACHER = "TTNotClassTchr";

	private const string S_TXT_LECTURE_NA = "Lecture Not Applicable";
	private const string S_TXT_TEACHER_CLASS = "Teacher/Class Name";
	private const string S_TXT_SHOW = "Show";
	private const string S_TXT_CHANGE = "Change Input";

	private const string S_TEACHER_ID_FIELD = "Teacher_Id";
	private const string S_CLASS_SUBJECT_NAME_FIELD = "classSubjectName";
	private const string S_STDDIV_ID_FIELD = "Standard_Division_Id";
	private const string S_TEACHER_SUBJECT_ID_FIELD = "Teacher_Subject_Id";
	private const string S_TEACHER_SUBJECT_NAME_FIELD = "Teacher_Subject";
    private const string S_STAYBACK_LECTURE = "Stayback";

	private const string S_OVERLAP_ERR_FIELD = "OverlapErrorMessage";
	private const string S_TEACHERWEEKLY_LECTURES_ERR_FIELD = "ErrMsgForWeeklyTeacherLectures";
	private const string S_TEACHERWEEKDAY_LECTURES_ERR_FIELD = "ErrMsgForWeekDayTeacherLectures";
	private const string S_SUBJECTWEEKDAY_LECTURES_ERR_FIELD = "ErrMsgForSubjectLectures";
	private const string S_SUBJECTWEEKDAY_ASSLECTURES_ERR_FIELD = "ErrMsgForAssociateSubjectLectures";
	private const string S_EXTERNAL_LECTURE_ERR_FIELD = "ErrMsgForExternalLectures";
	// Table Indices 
	private const int I_CLASS_TABLE_INDEX = 0;
	private const int I_STANDARD_TABLE_INDEX = 2;
	private const int I_TEACHER_TABLE_INDEX = 1;
	private const int I_WEEKDAY_TABLE_INDEX = 0;
	private const int I_ALL_LECT_NUM_TABLE_INDEX = 1;
	private const int I_ADDITIONAL_LECT_TABLE_INDEX = 9;
	private const int I_STATICBOUND_COLS_COUNT = 1;
	private const int I_ADDITIONAL_CLASS_LECT_TABLE_INDEX = 3;
	private const int I_LECT_NUM_TABLE_INDEX = 4;
	private const int I_YEAR_TABLE_INDEX = 2;
   
	#endregion " Constants "

	#region " Data Members "

    private static bool mbIsMidYear;
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

	#endregion " Data Members "

	#region " Events "

	/// <summary>
	/// This event is used to fill all default page controls. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
			if (bIsUseSubmitBehavior == true)
			{
				cmbTeachers.Focus();
                InitializeMemberVariables();
				if (!IsPostBack)
				{
                    if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    {
                        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    }
                    hidBtnShow.Value = S_TXT_SHOW;
                    RefreshValues();
					mbIsMidYear = SchoolTimeTableMasterBL.IsMid(miSchoolId, miAcademicYearId);
					btnAdditionalLectures.Visible = !mbIsMidYear;

					if (CheckPreCondition())
					{
						FillAllComboxes();
						VisibleHideCheckboxes(false);
						SetDefaultProperties();
						ReadQuerystring();
						if (!string.IsNullOrEmpty(hidTeacherId.Value))
						{
							cmbTeachers.SelectedValue = hidTeacherId.Value;
							hidEncrypt.Value = CommonUtility.EncryptQuerystring("TeacherId=" + cmbTeachers.SelectedValue.ToString() + "&TeacherName=" + cmbTeachers.SelectedItem.Text);
							btnShow_Click(sender, e);
						}
						else if (!string.IsNullOrEmpty(hidStandardId.Value))
						{
							cmbStandard.SelectedValue = hidStandardId.Value;
							FillDivisionCombo();
							cmbDivision.SelectedValue = hidDivisionId.Value;
							hidEncrypt.Value = CommonUtility.EncryptQuerystring("StandardId=" + cmbStandard.SelectedValue.ToString() + "&DivisionId=" + cmbDivision.SelectedValue + "&Class=" + cmbStandard.SelectedItem.Text + " - " + cmbDivision.SelectedItem.Text);
							btnShow_Click(sender, e);
						}
					}

					SetClientSideScriptAttributes();
				}
				else
				{
					if (Page.Request.Params.Get("__EVENTTARGET") != null)
                        if ((btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET"))) || (btnIncreaseCnt.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET"))) ||
							Page.Request.Params.Get("__EVENTTARGET").Contains("grdTeacherTT")) 
							FillGrid();
				}
				SetButtonText();
                if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    if (hidBtnShow.Value == S_TXT_CHANGE)
                    {
                        btnSave.Visible = true;
                        btnReset.Visible = true;
                        
                        btnAdditionalLectures.Visible = true;
                        //FillGrid();
                        if (cmbTeachers.SelectedIndex != Constants.I_ZERO)
                            btnAdditionalLectures.Visible = !mbIsMidYear;
                    }
                    RefreshValues();
                }
			    
            }
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is triggered when the user clicks on  show button.
	/// It checks if user has clicked the button to show the grid or to change the inputs.
	/// And displayes accordingly time table.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			// user has clicked the button to show the grid   
			if (ToggleStatus())
			{
				btnSave.Visible = true;
				btnReset.Visible = true;
				btnAdditionalLectures.Visible = true;
				FillGrid();
				if (cmbTeachers.SelectedIndex != Constants.I_ZERO)
					btnAdditionalLectures.Visible = !mbIsMidYear;
			}
			else
			{            // user has clicked the button to change the inputs.
				VisibleHideCheckboxes(false);
				btnReset.Visible = false;
				btnSave.Visible = false;
				btnAdditionalLectures.Visible = false;
			}
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to reset timetable.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			if (cmbTeachers.SelectedIndex != Constants.I_ZERO)
			{
				int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
				SchoolTimeTableMasterBL.ResetTimetable(miSchoolId, miAcademicYearId, iTeacherId, 0);
			}
			else
			{
				int iStandardDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
				SchoolTimeTableMasterBL.ResetTimetable(miSchoolId, miAcademicYearId, 0, iStandardDivisionId);
			}

			FillGrid();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    
	/// <summary>
	/// This event is used to save time-table.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			lblUpdateSucess.Visible = false;
			SchoolTimeTableMasterBL oTimeTable = GetObject();
			TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
			string[] strXml = new string[2];
			DataSet oDs = null;
			if (cmbTeachers.SelectedIndex == Constants.I_ZERO)
			{
				if (IsBaseLectureRemovedForStdDiv())
				{					
					DataTable oDtTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectDetails(miSchoolId, miAcademicYearId);
					strXml = GetXMLForTimeTable(oDtTeacherSubjects);
					string sAdditionalLect = GetXMLForAdditionalLecture();

					if (sAdditionalLect == null)
                        oDs = oTimeTable.ManageClassTimeTable(strXml[0], strXml[1], false, hidWantToInrsCnt.Value.ToInt());
					else
                        oDs = oTimeTable.ManageClassTimeTable(strXml[0], strXml[1], sAdditionalLect, hidWantToInrsCnt.Value.ToInt());
					if (ValidateData(oDs))
					{
						FillGrid();
						lblUpdateSucess.Visible = true;
                        lblUpdateSucess.Text = Resources.LocalizedResources.MsgWeeklyTimetableSaved;
					}
				}
			}
			else
			{
				DeleteAdditionalLecture();
				if (IsBaseLectureRemovedForTeacher())
				{
					DataTable oDtTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectDetails(miSchoolId, miAcademicYearId);
					int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
					strXml = GetXMLForTeacherTimeTable(oDtTeacherSubjects);
					string sTeacherXML = GetXMLForTeacherDetails();
                    oDs = oTimeTable.ManageTeacherTimeTable(iTeacherId, strXml[0], strXml[1], sTeacherXML, hidWantToInrsCnt.Value.ToInt());
					if (ValidateData(oDs))
					{
						FillGrid();
						lblUpdateSucess.Visible = true;
                        lblUpdateSucess.Text = Resources.LocalizedResources.MsgWeeklyTimetableSaved;
					}
				}
			}
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			btnSave.Enabled = true;
            hidWantToInrsCnt.Value =Constants.S_ZERO;
		}
	}
    /// <summary>
    /// This button use to increament count
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIncreaseCnt_Click(object sender, EventArgs e)
    {
        try
        {
            hidWantToInrsCnt.Value =Constants.S_ONE;
            this.btnSave_Click(null, null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }



	private bool IsBaseLectureRemovedForTeacher()
	{
		DataSet oDs = (DataSet)grdTeacherTT.DataSource;

		DataTable odtBaseLect = oDs.Tables[0];
		DataTable odtAdiitionalLect = oDs.Tables[9];

		for (int iCount = 0; iCount < odtAdiitionalLect.Rows.Count; iCount++)
		{
			string sWeekDayName;
			int iLectureNumber;
			sWeekDayName = (odtAdiitionalLect.Rows[iCount]["WeekDayName"]).ToString();
			iLectureNumber = Convert.ToInt32(odtAdiitionalLect.Rows[iCount]["LectureNumber"]);

			int iRowCount = grdTeacherTT.Rows.Count;
			int iColCount = grdTeacherTT.Rows[0].Cells.Count;

			for (int j = I_STATICBOUND_COLS_COUNT; j < iColCount; j++)
			{
				for (int i = 0; i < iRowCount; i++)
				{
					if (grdTeacherTT.Rows[i].Cells[j].Controls.Count > 0)
					{
						DropDownList oCmb = (DropDownList)grdTeacherTT.Rows[i].Cells[j].Controls[0];

						if (oCmb.SelectedValue.Equals("0"))
						{
							if (grdTeacherTT.HeaderRow.Cells[j].Text == sWeekDayName && iLectureNumber == i + 1)
							{
								lblError.Visible = true;
								lblUpdateSucess.Visible = false;
                                lblError.Text = Resources.LocalizedResources.LectureNumber + " " + iLectureNumber + " " + Resources.LocalizedResources.On + " " + sWeekDayName + " " + Resources.LocalizedResources.MsgTeacherTimetable;
								return false;
							}
						}
					}
				}
			}
		}

		return true;
	}

	private bool IsBaseLectureRemovedForStdDiv()
	{
		DataSet oDs = (DataSet)grdStdTimeTable.DataSource;

		DataTable odtBaseLect = oDs.Tables[0];
		DataTable odtAdiitionalLect = oDs.Tables[3];

		for (int iCount = 0; iCount < odtAdiitionalLect.Rows.Count; iCount++)
		{
			string sWeekDayName;
			int iLectureNumber;
			sWeekDayName = odtAdiitionalLect.Rows[iCount]["WeekDayName"].ToString();
			iLectureNumber = Convert.ToInt32(odtAdiitionalLect.Rows[iCount]["LectureNumber"]);

			int iRowCount = grdStdTimeTable.Rows.Count;
			int iColCount = grdStdTimeTable.Rows[0].Cells.Count;

			for (int j = I_STATICBOUND_COLS_COUNT; j < iColCount; j++)
			{
				for (int i = 0; i < iRowCount; i++)
				{
					if (grdStdTimeTable.Rows[i].Cells[j].Controls.Count > 0)
					{
						DropDownList oCmb = (DropDownList)grdStdTimeTable.Rows[i].Cells[j].Controls[0];

						if (oCmb.SelectedValue.Equals("0"))
						{
							if (grdStdTimeTable.HeaderRow.Cells[j].Text == sWeekDayName && iLectureNumber == i + 1)
							{
								lblError.Visible = true;
                                lblError.Text = Resources.LocalizedResources.LectureNumber + " " + iLectureNumber + " " + Resources.LocalizedResources.On + " " + sWeekDayName + " " + Resources.LocalizedResources.MsgTeacherTimetable;
								return false;
							}
						}
					}
				}
			}
		}

		return true;
	}

	/// <summary>
	/// This event is used to fill division combobox.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (cmbStandard.SelectedIndex != Constants.I_ZERO)
			{
				FillDivisionCombo();
				cmbTeachers.SelectedIndex = Constants.I_ZERO;
				hidEncrypt.Value = string.Empty;
                btnAdditionalLectures.Text = Resources.LocalizedResources.OptionalSubjectLectures;
                lblLegend.Text = Resources.LocalizedResources.MsgAssociatedOptionalLecture;
			}
			else
				ClearDivisionCombo();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used reset standard and division combo.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			cmbStandard.SelectedIndex = Constants.I_ZERO;
			ClearDivisionCombo();
			hidEncrypt.Value = CommonUtility.EncryptQuerystring("TeacherId=" + cmbTeachers.SelectedValue.ToString() + "&TeacherName=" + cmbTeachers.SelectedItem.Text);
            btnAdditionalLectures.Text = Resources.LocalizedResources.AdditionalLectures;
            lblLegend.Text = Resources.LocalizedResources.MsgAssociatedAdditionalLecture;
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add/remove assembly in timetable.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkAssembly_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateMPTStayBackInTimeTable();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add/remove MPT in timetable.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkMPT_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateMPTStayBackInTimeTable();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add/remove StayBack in timetable.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkStayback_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
           UpdateMPTStayBackInTimeTable();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    protected void chkWeeklyTest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            UpdateMPTStayBackInTimeTable();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to set style for first cell at footer which displays Total Lecture lable.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdTeacherTT_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex >= Constants.I_ZERO)
			{
				if (e.Row.Cells[0].Text == "99")
				{
                    e.Row.Cells[0].Text = Resources.LocalizedResources.TotalLectures;
					e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(165, 195, 215);
					e.Row.Cells[0].Font.Bold = true;
				}
			}
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to set style to class subject lecture count grid footer.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdSubjectLect_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex == grdSubjectLect.Rows.Count - 2)
			{
				e.Row.BackColor = System.Drawing.Color.FromArgb(165, 195, 215);
				e.Row.Font.Bold = true;
			}
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to delete additional lecture.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdAdditionalClasses_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName != Constants.S_COMMAND_SORT)
			{
				int iRowIndex = Convert.ToInt32(e.CommandArgument);
				switch (e.CommandName)
				{
					case "DELETE_LECT":
						int iDetailID = Convert.ToInt32(grdAdditionalClasses.DataKeys[iRowIndex][0].ToString());
						SchoolTimeTableMasterBL.DeleteAdditionalLecture(iDetailID);

						lblUpdateSucess.Visible = true;
                        if (cmbTeachers.SelectedIndex == 0)
                            lblUpdateSucess.Text = Resources.LocalizedResources.MsgOptionalLectureDeleted;
                        else
                            lblUpdateSucess.Text = Resources.LocalizedResources.MsgAdditionalLectureDeleted;
						FillGrid();
						break;
				}
			}
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add conformation message to each additional lecture grid delete button.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdAdditionalClasses_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex >= Constants.I_ZERO)
			{
				ImageButton imgDelete = (ImageButton)e.Row.Cells[5].Controls[Constants.I_ZERO];
				if (cmbTeachers.SelectedIndex == 0)
					imgDelete.Attributes.Add("OnClick", "if(!ConfirmDelete('student')){return false;}");
				else
                    imgDelete.Attributes.Add("OnClick", "if(!ConfirmDelete('teacher')){return false;}");

			}
			SetVisibilityOfColumn();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This evant is used to set query string.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			hidEncrypt.Value = CommonUtility.EncryptQuerystring("StandardId=" + cmbStandard.SelectedValue.ToString() + "&DivisionId=" + cmbDivision.SelectedValue + "&Class=" + cmbStandard.SelectedItem.Text + " - " + cmbDivision.SelectedItem.Text);
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	#endregion " Events "

	#region " Private Methods "

	/// <summary>
	/// This method is used to set java script properties to page controls.
	/// </summary>
	private void SetClientSideScriptAttributes()
	{
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
		btnSave.Attributes["onclick"] = "javascript:DisableButtons(this)";
		btnReset.Attributes["onclick"] = "if(!ConfirmReset()){return false;}";
	    btnIncreaseCnt.Attributes["onclick"] = "javascript:HidePopUp(this)";
		btnAdditionalLectures.Attributes["onclick"] = "if(!OpenAdditionalClassesPopup()){return false;}";
        ApplyMouseHoverEffect(new List<Button> { btnAdditionalLectures, btnSave, btnReset, btnShow, btnIncreaseCnt, btnCancel });
        SetDefaultButton(btnShow);
	}

	/// <summary>
	/// This method is used to set text of button
	/// </summary>
	private void SetButtonText()
	{
		if (cmbTeachers.SelectedIndex == Constants.I_ZERO)
		{
            btnAdditionalLectures.Text = Resources.LocalizedResources.OptionalSubjectLectures;
			lblAdditionalLecture.Text = Resources.LocalizedResources.MsgAdditionalOptionalLectures;
		}
		else if (cmbStandard.SelectedIndex == Constants.I_ZERO)
		{
            btnAdditionalLectures.Text = Resources.LocalizedResources.AdditionalLectures;
            lblAdditionalLecture.Text = Resources.LocalizedResources.AdditionalLectures;
		}
	}

	/// <summary>
	/// This method is used to set default properties of page controls.
	/// </summary>
	private void SetDefaultProperties()
	{
		btnSave.Visible = false;
		btnReset.Visible = false;
		btnAdditionalLectures.Visible = false;

        if (Settings.IsAssemblyApplicable)
            chkAssembly.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.AssemblyName.Replace(" ",string.Empty)) + " " + Resources.LocalizedResources.Applicable;

        if (Settings.IsMPTApplicable)        
            chkMPT.Text = Resources.LocalizedResources.Is + " " + Settings.MPTName + " " + Resources.LocalizedResources.Applicable;            

		if (Settings.IsStaybackApplicable)
            chkStayback.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.StaybackName.Replace(" ", string.Empty)) + " " + Resources.LocalizedResources.Applicable;

        if(Settings.IsWeeklyTestApplicable)
            chkWeeklyTest.Text = Resources.LocalizedResources.Is + " " + Settings.WeeklyTestName + " " + Resources.LocalizedResources.Applicable;
	}	

	#region " Fill ComboBoxes "

	/// <summary>
	/// This method is used to fill all combo boxes.
	/// </summary>
	private void FillAllComboxes()
	{
		hidHasFullAccess.Value = Convert.ToString(CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.WeeklyTimetable));
		TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
		DataSet oDsTeacherStandard = oTeacherSubjectAssignmentBL.GetTeacherAndStandardForTT(miSchoolId, miAcademicYearId);
		
		if (moUserRole== Constants.UserRoles.Teacher && hidHasFullAccess.Value == Constants.C_NO.ToString())
		{
			DataRow[] odtRows = oDsTeacherStandard.Tables[I_TEACHER_TABLE_INDEX].Select("Teacher_Id = " + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
			foreach (DataRow dr in odtRows)
			{
				cmbTeachers.Items.Add(new ListItem { Value = dr["Teacher_Id"].ToString(), Text = dr["TeacherName"].ToString() });
			}

			cmbTeachers.Items.Insert(0, new ListItem(Constants.S_SELECT, "0"));
			cmbStandard.Items.Add(new ListItem(Constants.S_SELECT, "0"));
			cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, "0"));
			if (Convert.ToString(Session[Constants.S_SESSION_IS_CLASS_TEACHER]) == Constants.C_YES.ToString())
			{
				cmbStandard.Enabled = true;
				cmbDivision.Enabled = true;
				odtRows = oDsTeacherStandard.Tables[I_CLASS_TABLE_INDEX].Select("Teacher_Id = " + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]) + "AND Standard_Division_Id = " + Convert.ToString(Session[Constants.S_SESSION_TEACHER_STDDIV_ID]));
				foreach (DataRow dr in odtRows)
				{
					cmbStandard.Items.Add(new ListItem { Value = dr["Standard_Id"].ToString(), Text = dr["Standard_Name"].ToString() });
				}
			}
			else
			{
				trStandardFilter.Disabled = true;
				cmbStandard.Enabled = false;
				cmbDivision.Enabled = false;
			}
		}
		else
		{
			FillTeacherCombo(oDsTeacherStandard.Tables[I_TEACHER_TABLE_INDEX]);
			FillStandardCombo(oDsTeacherStandard.Tables[I_STANDARD_TABLE_INDEX]);
		}

		ClearDivisionCombo();
	}

	/// <summary>
	/// This method is used to fill combo with all teachers in school.
	/// </summary>
	/// <param name="oDTTable"></param>
	private void FillTeacherCombo(DataTable oDTTeachers)
	{
		if (oDTTeachers != null)
			ControlUtility.FillDropDownList(oDTTeachers, ref cmbTeachers, "Teacher_Id", "TeacherName", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to fill combo with standards associated with school. 
	/// </summary>
	/// <param name="oDTStandard"></param>
	private void FillStandardCombo(DataTable oDTStandard)
	{
		ControlUtility.FillDropDownList(oDTStandard, ref cmbStandard, "Standard_Id", "Standard_Name", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to clear division combobox.
	/// </summary>
	private void ClearDivisionCombo()
	{
		cmbDivision.Items.Clear();
		cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, "0"));
		cmbDivision.SelectedIndex = 0;
	}

	/// <summary>
	/// This method is used to fill division combobox.
	/// </summary>
	private void FillDivisionCombo()
	{
		DivisionCollectionBL oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
		DataTable oDTDivision = oDiv.GetAllDivisionsForStandard(Convert.ToInt32(cmbStandard.SelectedValue));
		if (moUserRole == Constants.UserRoles.Teacher && hidHasFullAccess.Value == Constants.C_NO.ToString())
		{
			DataRow[] odtRows = oDTDivision.Select("SchoolWise_Standard_Division_Id = " + Convert.ToString(Session[Constants.S_SESSION_TEACHER_STDDIV_ID]));
			foreach (DataRow dr in odtRows)
			{
				cmbDivision.Items.Add(new ListItem { Value = dr["SchoolWise_Standard_Division_Id"].ToString(), Text = dr["division_name"].ToString() });
			}
		}
		else
		{
			ControlUtility.FillDropDownList(oDTDivision, ref cmbDivision, "SchoolWise_Standard_Division_Id", "division_name", Constants.S_SELECT);
		}
	}

	#endregion " Fill ComboBoxes "

	#region " Preconditions "

	/// <summary>
	/// This method checks the preconditons of TeacherTimetable.
	/// </summary>
	/// <returns></returns>
	private bool CheckPreCondition()
	{
		bool bReturn = false;
		string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.TeacherTimeTable);

		if (string.IsNullOrEmpty(sLinks))
		{
			divErr.Visible = false;
			bReturn = true;
		}
		else
		{
			divErr.InnerHtml = sLinks;
			HideAllFields();
		}

		return bReturn;
	}

	/// <summary>
	/// This method hides all the fields when required preconditions are not satisfied.
	/// </summary>
	private void HideAllFields()
	{
		btnAdditionalLectures.Visible = false;
		tblTimeTable.Visible = false;
		tblFilters.Visible = false;
		LegendTable.Visible = false;
		btnShow.Visible = false;
	}

	#endregion " Preconditions "

	#region " Helping Methods "

	/// <summary>
	/// This method is called from click event handler of search button.
	/// it checks if user has clicked the button to show the grid or to change the inputs.
	/// It changes the caption of the button.
	/// And enables the inputs.
	/// </summary>
	/// <returns>
	/// True: 
	/// False:
	/// </returns>
	private bool ToggleStatus()
	{
		bool bReturn = true;
		if (btnShow.Text.Equals(Resources.LocalizedResources.Show) && (cmbStandard.SelectedIndex != 0 || cmbTeachers.SelectedIndex != 0))
		{
            btnShow.Text = Resources.LocalizedResources.ChangeInput;
            hidBtnShow.Value=S_TXT_CHANGE;
			EnableDisableCombos(false);
			if (cmbTeachers.SelectedIndex != Constants.I_ZERO)
			{
				lblName.Text = cmbTeachers.SelectedItem.Text;
				divStdTimeTable.Visible = false;
				divTeacherTT.Visible = true;
				grdSubjectLect.Visible = true;
				grdAdditionalClasses.Visible = true;
				divSubjectLect.Visible = true;
				divAdditionalLect.Visible = true;
			}
			else
			{
                lblName.Text = Resources.LocalizedResources.Class + " " + cmbStandard.SelectedItem.Text + " - " + cmbDivision.SelectedItem.Text;
				divStdTimeTable.Visible = true;
				divTeacherTT.Visible = false;
				grdAdditionalClasses.Visible = true;
				divAdditionalLect.Visible = true;
			}

			bReturn = true;
		}
		else
		{
            btnShow.Text = Resources.LocalizedResources.Show;
            hidBtnShow.Value=S_TXT_SHOW;
			EnableDisableCombos(true);
			EmptyGrid();
			divTeacherTT.Visible = false;
			divStdTimeTable.Visible = false;
			lblName.Text = S_TXT_TEACHER_CLASS;
			grdSubjectLect.Visible = false;
			divSubjectLect.Visible = false;
			grdAdditionalClasses.Visible = false;
			divAdditionalLect.Visible = false;
			bReturn = false;
			
			if (moUserRole== Constants.UserRoles.Teacher && Convert.ToString(Session[Constants.S_SESSION_IS_CLASS_TEACHER]) != Constants.C_YES.ToString() && hidHasFullAccess.Value == Constants.C_NO.ToString())
			{
				cmbStandard.Enabled = false;
				cmbDivision.Enabled = false;
			}
		}

		return bReturn;
	}

	/// <summary>
	/// This method enables or disables the combo-boxes according to the parameter specified.
	/// </summary>
	/// <param name="abAction"></param>
	private void EnableDisableCombos(bool abAction)
	{
		cmbStandard.Enabled = abAction;
		cmbDivision.Enabled = abAction;
		cmbTeachers.Enabled = abAction;
	}

	#region " Fill Grid "

	/// <summary>
	/// This is a wrapper method to fill the grid according to selected options.
	/// </summary>
	private void FillGrid()
	{
		// if teacher is selected
		if (cmbTeachers.SelectedIndex != Constants.I_ZERO)
			FillTeacherTTGrid();
		else  // if class (standard-division) selected.
			FillStdGrid();
	}

	/// <summary>
	///  This method is to fill timetable grid for class(standard-division).
	/// </summary>
	private void FillStdGrid()
	{
		int iStdDivId = Convert.ToInt32(cmbDivision.SelectedValue);
		DataSet oDs = SchoolTimeTableMasterBL.GetTimeTableForClass(miSchoolId, miAcademicYearId, iStdDivId);
		grdStdTimeTable.DataSource = oDs;
		grdStdTimeTable.DataBind();
		GenerateColumns(oDs);

		grdAdditionalClasses.DataSource = oDs.Tables[I_ADDITIONAL_CLASS_LECT_TABLE_INDEX];
		grdAdditionalClasses.DataBind();

		tdSubjectLect.Visible = false;
		tdHeadSubjectLect.Visible = false;

		grdAdditionalClasses.Width = Unit.Percentage(50);
		divAdditionalLect.Style["width"] = "44%";
	}

	/// <summary>
	/// This method is used to hide or show class and teacher name columns.
	/// </summary>
	private void SetVisibilityOfColumn()
	{
		this.grdAdditionalClasses.Columns[4].Visible = cmbTeachers.SelectedIndex == 0;
		this.grdAdditionalClasses.Columns[2].Visible = !(cmbTeachers.SelectedIndex == 0);
	}

	/// <summary>
	/// This method is used to create columns of grid.
	/// </summary>
	/// <param name="aoDs"></param>
	private void GenerateColumns(DataSet aoDs)
	{
		GenerateHeaderRowCols(aoDs, ref grdStdTimeTable);
		GenerateOtherColumns(aoDs);
	}

	/// <summary>
	/// This method is used to fills the timetable grid for selected teacher.
	/// </summary>
	private void FillTeacherTTGrid()
	{
		int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
		DataSet oDs = SchoolTimeTableMasterBL.GetTimeTableForTeacher(miSchoolId, miAcademicYearId, iTeacherId);
		grdTeacherTT.DataSource = oDs;
		grdTeacherTT.DataBind();
		if (Page.Request.Params.Get("__EVENTTARGET") != null)
		{
			if ((!btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET"))) &&
				!Page.Request.Params.Get("__EVENTTARGET").Contains("grdTeacherTT"))
				SetCheckBox(oDs);
		}
		else if (Page.Request.Params.Get("__EVENTTARGET") == null && Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
			SetCheckBox(oDs);

		GenerateHeaderRowCols(oDs, ref grdTeacherTT);
		GenerateOtherColumnsForTeacherTT(oDs);
		grdAdditionalClasses.DataSource = oDs.Tables[I_ADDITIONAL_LECT_TABLE_INDEX];
		grdAdditionalClasses.DataBind();

		tdSubjectLect.Visible = true;
		tdHeadSubjectLect.Visible = true;

		grdAdditionalClasses.Width = Unit.Percentage(90);
		divAdditionalLect.Style["width"] = "90%";
	}

	/// <summary>
	/// This method is used to fill teacher time table.
	/// </summary>
	private void UpdateMPTStayBackInTimeTable()
	{
		int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
		DataSet oDs = SchoolTimeTableMasterBL.GetTimeTableForTeacher(miSchoolId, miAcademicYearId, iTeacherId);
		grdTeacherTT.DataSource = oDs;
		grdTeacherTT.DataBind();
		grdAdditionalClasses.DataSource = oDs.Tables[I_ADDITIONAL_LECT_TABLE_INDEX];
		grdAdditionalClasses.DataBind();
		GenerateHeaderRowCols(oDs, ref grdTeacherTT);
		GenerateOtherColumnsForTeacherTT(oDs);
		DisplayLectureCount();
		UpdateSubjectLectureCount();
	}

	/// <summary>
	/// This method is used to show "Total Lectures" count,
	/// in all day column footer of "Teacher Timetable" grid.
	/// </summary>
	/// <param name="aoDs"></param>
	/// <param name="aiLastRowIndex"></param>
	private void DisplayLectureCount(DataSet aoDs, int aiLastRowIndex)
	{
		int iColCount = aoDs.Tables[0].Columns.Count - I_STATICBOUND_COLS_COUNT;
		DataTable oDtTeacherLectures = aoDs.Tables[0];
		string sAssemblyWeekday = Settings.AssemblyWeekday;
		string sMPTWeekday =Settings.MPTWeekday;
        string sWeeklyTestWeekDay = Settings.WeeklyTestWeekDay;
		int iTotLectCount = 0;
		TableCell oTableCell;

		for (int j = I_STATICBOUND_COLS_COUNT; j <= iColCount; j++)
		{
			oTableCell = new TableCell();
			oTableCell.HorizontalAlign = HorizontalAlign.Center;
			oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
			oTableCell.Wrap = false;
			oTableCell.Style.Add(HtmlTextWriterStyle.PaddingTop, "2px");
			oTableCell.Style.Add(HtmlTextWriterStyle.PaddingBottom, "2px");
			oTableCell.Font.Bold = true;
			oTableCell.BackColor = System.Drawing.Color.FromArgb(165, 195, 215);
			oTableCell.Style.Add(HtmlTextWriterStyle.Color, "Black");
			oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "9pt");
			int iCellIndex = grdTeacherTT.Rows[aiLastRowIndex].Cells.Add(oTableCell);

			string sWeekDayName = grdTeacherTT.HeaderRow.Cells[j].Text;

			oTableCell.Attributes.Add("title", Resources.LocalizedResources.TotalLectures+" " + (aiLastRowIndex + 1).ToString() + " [" + sWeekDayName + "]");
			string sFilterWeekDay = " Lecture_no='99'";
			DataRow[] oDrLectures = oDtTeacherLectures.Select(sFilterWeekDay);

			int iLectCount = Convert.ToInt32(oDrLectures[0][sWeekDayName].ToString());

			if (sAssemblyWeekday == sWeekDayName && chkAssembly.Checked)
				iLectCount += 1;

			if (sMPTWeekday == sWeekDayName && chkMPT.Checked)
				iLectCount += 1;

            if (sWeeklyTestWeekDay == sWeekDayName && chkWeeklyTest.Checked)
                iLectCount += 1;

			if (chkStayback.Checked)
			{
				DataTable oDtStayback = aoDs.Tables[3];
				DataRow[] oArrRows = oDtStayback.Select("WeekDay_Name='" + sWeekDayName + "'");
				iLectCount += oArrRows.Length;
			}

			oTableCell.Text = iLectCount.ToString();
			iTotLectCount += iLectCount;
		}
	}

	/// <summary>
	/// This method is used to show  "Total Lectures" count in "Teacher Timtable" grid.
	/// </summary>
	private void DisplayLectureCount()
	{
		int iRowCount = grdTeacherTT.Rows.Count;
		int iColCount = grdTeacherTT.HeaderRow.Cells.Count - 1;
		int iTotLectCount = 0;
		string sAssembly = Settings.AssemblyName;
		string sMPT = Settings.MPTName;
		string sStayback = Settings.StaybackName;
        string sWeeklyTest = Settings.WeeklyTestName;

		for (int j = I_STATICBOUND_COLS_COUNT; j <= iColCount; j++)
		{
			int iColLectCount = 0;
			for (int i = 0; i < iRowCount - 1; i++)
			{
				DropDownList oDDL = (DropDownList)grdTeacherTT.Rows[i].Cells[j].FindControl("dr" + i.ToString() + "_" + j.ToString());
				if (oDDL != null)
				{
					if (oDDL.SelectedIndex != Constants.I_ZERO)
						iColLectCount += 1;
				}
				else
				{
					if (grdTeacherTT.Rows[i].Cells[j].Text == sAssembly && chkAssembly.Checked)
						iColLectCount += 1;

					if (grdTeacherTT.Rows[i].Cells[j].Text == sMPT && chkMPT.Checked)
						iColLectCount += 1;

					if (grdTeacherTT.Rows[i].Cells[j].Text == sStayback && chkStayback.Checked)
						iColLectCount += 1;

                    if (grdTeacherTT.Rows[i].Cells[j].Text == sWeeklyTest && chkWeeklyTest.Checked)
                        iColLectCount += 1;
				}
			}

			iTotLectCount += iColLectCount;
			grdTeacherTT.Rows[grdTeacherTT.Rows.Count - 1].Cells[j].Text = iColLectCount.ToString();
		}
	}

	/// <summary>
	/// This method is used to get "Total Weekly Lectures" count in "Class Subject Lecture Count" grid.
	/// </summary>
	private void GetLectureCountsForTeachers()
	{
		int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
		string sConsiderAssembly = Settings.IsAssemblyApplicable ? Constants.S_YES : Constants.S_NO;
		string sConsiderMPT = Settings.IsMPTApplicable ? Constants.S_YES : Constants.S_NO;
		string sConsiderStayback = Settings.IsStaybackApplicable ? Constants.S_YES : Constants.S_NO;
        string sConsiderWeeklyTest = Settings.IsWeeklyTestApplicable ? Constants.S_YES : Constants.S_NO;
		SchoolTimeTableMasterBL oSchoolTimeTableMasterBL = new SchoolTimeTableMasterBL();
        DataTable oDt = oSchoolTimeTableMasterBL.GetLectureCountsForTeachers(iTeacherId, sConsiderAssembly, sConsiderMPT, sConsiderStayback, sConsiderWeeklyTest);

        grdSubjectLect.DataSource = oDt;
        grdSubjectLect.DataBind();

		grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 1].BackColor = System.Drawing.Color.FromArgb(165, 195, 215);
		grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 1].Font.Bold = true;
	}

	/// <summary>
	/// This method is used to update lecture count in "Class-Subject Lecture Count" grid,
	/// when MPT, assembly, Stay Back checkbox is checked/unchecked.
	/// </summary>
	private void UpdateSubjectLectureCount()
	{
		int iRowCount = grdTeacherTT.Rows.Count;
		int iColCount = grdTeacherTT.HeaderRow.Cells.Count - 1;
		int iCount = 0;
		// Create hashtable whose key is Teacher_Subject_Id and value is the count of lectures in the TT.
		Hashtable oHT = new Hashtable();
		for (int i = 0; i < iRowCount - 1; i++)
		{
			for (int j = I_STATICBOUND_COLS_COUNT; j <= iColCount; j++)
			{
				DropDownList oDDL = (DropDownList)grdTeacherTT.Rows[i].Cells[j].FindControl("dr" + i.ToString() + "_" + j.ToString());
				if (oDDL != null)
				{
					int iTeacherSubjectId = Convert.ToInt32(oDDL.SelectedValue);
					if (!oHT.Contains(iTeacherSubjectId))
					{ oHT[iTeacherSubjectId] = 1; }
					else
					{
						iCount = Convert.ToInt32(oHT[iTeacherSubjectId].ToString());
						iCount++;
						oHT[iTeacherSubjectId] = iCount;
					}
				}
			}
		}
		// Once the hastable is ready, set the values in the lecture count grid.
		iRowCount = grdSubjectLect.Rows.Count;
		int iTotLectCount = 0;

		for (int i = 0; i <= iRowCount - 1; i++)
		{
			int iTeacherSubjectId = Convert.ToInt32(grdSubjectLect.DataKeys[i][0].ToString());
			iCount = 0;
			// Hashtable may not be filled for the subject which is not yet associated in timetable.
			if (oHT[iTeacherSubjectId] != null)
				iCount = Convert.ToInt32(oHT[iTeacherSubjectId].ToString());

			grdSubjectLect.Rows[i].Cells[1].Text = iCount.ToString();
			iTotLectCount += iCount;
		}

		if (Settings.IsAssemblyApplicable)
		{
			// Check if assembly is applicable. if yes then increase the count by 1.
			if (chkAssembly.Checked)
                iCount = Convert.ToInt32(ViewState["TotalAssembly"].ToString()); 
			else
				iCount = 0;

			grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 5].Cells[1].Text = iCount.ToString();
			iTotLectCount += iCount;
		}

		if (Settings.IsMPTApplicable)
		{
			// Check if MPT is applicable. if yes then increase the count by 1.
			if (chkMPT.Checked)
                iCount = Convert.ToInt32(ViewState["TotalMPT"].ToString()); 
			else
				iCount = 0;

			grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 4].Cells[1].Text = iCount.ToString();
			iTotLectCount += iCount;
		}

		if (Settings.IsStaybackApplicable)
		{
			// Check if stayback is applicable. if yes then increase the count by total number of staybacks for all stds.
			if (chkStayback.Checked)
				iCount = Convert.ToInt32(ViewState["TotalStaybacks"].ToString());
			else
				iCount = 0;

			grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 3].Cells[1].Text = iCount.ToString();
			iTotLectCount += iCount;
		}

        if (Settings.IsWeeklyTestApplicable)
        {
            if (chkWeeklyTest.Checked)
                iCount = Convert.ToInt32(ViewState["TotalWeeklyTest"].ToString());
            else
                iCount = Constants.I_ZERO;

            grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 2].Cells[1].Text = iCount.ToString();
            iTotLectCount += iCount;
        }
		// @TotalStaybacks
		grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 1].Cells[1].Text = iTotLectCount.ToString();
	}

	/// <summary>
	/// This method is used to sets checkboxes as per configured timetable of teacher. 
	/// </summary>
	/// <param name="aoDS"></param>
	private void SetCheckBox(DataSet aoDS)
	{
		if (Settings.IsMPTApplicable ||
			Settings.IsAssemblyApplicable ||
			Settings.IsStaybackApplicable)
		{
			DataTable oDT = aoDS.Tables[2];
			if (oDT.Rows.Count > Constants.I_ZERO)
			{
				VisibleHideCheckboxes(true);
				if (Settings.IsMPTApplicable)
				{
					if (oDT.Rows[0]["MPT_Applicable"].ToString() == Constants.C_YES.ToString())
						chkMPT.Checked = true;
					else
						chkMPT.Checked = false;
				}
                if (Settings.IsAssemblyApplicable)
				{
					if (oDT.Rows[0]["Assembly_Applicable"].ToString() == Constants.C_YES.ToString())
						chkAssembly.Checked = true;
					else
						chkAssembly.Checked = false;
				}
                if (Settings.IsStaybackApplicable)
				{
                    if (oDT.Rows[0]["Stayback_Applicable"].ToString() ==true.ToString())
						chkStayback.Checked = true;
                    else
						chkStayback.Checked = false;
				}               
			}
		}
	}

	/// <summary>
	/// This method is used to show/hide checkboxes, 
	/// as per subjects MPT, Assembly, Stay Back associated to class or teacher.
	/// </summary>
	/// <param name="abIsVisible"></param>
	private void VisibleHideCheckboxes(bool abIsVisible)
	{
        if (Settings.IsMPTApplicable)        
            chkMPT.Visible = abIsVisible;         
        else        
            chkMPT.Visible = false;           

		if (Settings.IsAssemblyApplicable)
			chkAssembly.Visible = abIsVisible;
		else
			chkAssembly.Visible = false;

		if (Settings.IsStaybackApplicable)
			chkStayback.Visible = abIsVisible;
		else
			chkStayback.Visible = false;

        if (Settings.IsWeeklyTestApplicable)
            chkWeeklyTest.Visible = abIsVisible;
        else
            chkWeeklyTest.Visible = false;
	}

	/// <summary>
	/// This method used to add "Week days" column names to the header row of the timetable grid.
	/// </summary>
	/// <param name="aoDs"></param>
	/// <param name="aoGrdView"></param>
	private void GenerateHeaderRowCols(DataSet aoDs, ref GridView aoGrdView)
	{
		// already bound columns = 1
		int iCount = aoDs.Tables[0].Columns.Count - I_STATICBOUND_COLS_COUNT;

		if (aoGrdView.Rows.Count > Constants.I_ZERO)
		{
			// Loop to add Divisions in Header ROw 
			for (int iColIndex = I_STATICBOUND_COLS_COUNT; iColIndex <= iCount; iColIndex++)
			{
				TableCell oTableCell1 = new TableCell();
				oTableCell1.HorizontalAlign = HorizontalAlign.Center;

				oTableCell1.Width = System.Web.UI.WebControls.Unit.Point(900);
				oTableCell1.Wrap = false;
				oTableCell1.Style.Add(HtmlTextWriterStyle.Padding, "2");
				oTableCell1.Text = aoDs.Tables[0].Columns[iColIndex].ColumnName;
				aoGrdView.HeaderRow.Cells.Add(oTableCell1);
			}
		}
	}

	/// <summary>
	/// This method adds 
	///    1.  Weekdays columns.
	///    2.  Dropdownlist(containing class-Subjects) to every newly generated cell.
	/// </summary>
	/// <param name="aoDs"></param>
	private void GenerateOtherColumnsForTeacherTT(DataSet aoDs)
	{
		string sFilter = string.Empty;
		string sColName = string.Empty;
		string sWeekDayName = string.Empty;
		string sFilterWeekDay = string.Empty;
		string sFilterMaxLetures = string.Empty;

		int iCellIndex;
		int iLectureNo;
		int iStdDivId = Convert.ToInt32(cmbDivision.SelectedValue);
		int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
		int iRowCount = aoDs.Tables[0].Rows.Count;
		int iColCount = aoDs.Tables[0].Columns.Count - I_STATICBOUND_COLS_COUNT;

		TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
		DataSet oDsTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectMaxLecDetails(miSchoolId, miAcademicYearId);
		DataTable oDtTeacherSubjects = oDsTeacherSubjects.Tables[I_WEEKDAY_TABLE_INDEX];
		DataTable oDtLectureNumbers = oDsTeacherSubjects.Tables[I_ALL_LECT_NUM_TABLE_INDEX];

		if (iStdDivId != Constants.I_ZERO)
			sFilter = " AND " + S_STDDIV_ID_FIELD + "=" + iStdDivId.ToString();

		ViewState["TotalStaybacks"] = aoDs.Tables[6].Rows[0][0].ToString();
        ViewState["TotalAssembly"] = aoDs.Tables[7].Rows[0][0].ToString();
        ViewState["TotalMPT"] = aoDs.Tables[8].Rows[0][0].ToString();
        ViewState["TotalWeeklyTest"] = aoDs.Tables[11].Rows[0][0].ToString();

		TableCell oTableCell;

		DataRow[] oDrLectures;
		DropDownList ocmbTeacherSubj;

		string sAssembly = Settings.AssemblyName;
		string sMPT = Settings.MPTName;
		string sStayback = Settings.StaybackName;
        string sWeeklyTest = Settings.WeeklyTestName;
		string sAssemblyWeekday = Settings.AssemblyWeekday;
		string sMPTWeekday = Settings.MPTWeekday;

		int iAssemblyLectNo = Settings.AssemblyLectNo;
		int iMPTLectNo = Settings.MPTLectNo;
        int iWeeklyTestLectNo = Settings.WeeklyTestLectNo;

        bool bIsShortName=SchoolBase.Settings.DisplayShortNameOnTimeTableScreen;
		// The loop is executed except for last row. As the last row is added for total lectures.
		int i = 0;
		for (i = 0; i < iRowCount - 1; i++)
		{
			for (int j = I_STATICBOUND_COLS_COUNT; j <= iColCount; j++)
			{
				oTableCell = new TableCell();
				oTableCell.HorizontalAlign = HorizontalAlign.Center;
				oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
				oTableCell.Wrap = false;
				oTableCell.Style.Add(HtmlTextWriterStyle.PaddingTop, "2px");
				oTableCell.Style.Add(HtmlTextWriterStyle.PaddingBottom, "2px");
				sColName = grdTeacherTT.HeaderRow.Cells[j].Text;
				oTableCell.Text = aoDs.Tables[0].Rows[i][sColName].ToString();
				iCellIndex = grdTeacherTT.Rows[i].Cells.Add(oTableCell);
				bool bIsAdditionalLect = false;
				string sAdditionalLecFilter = string.Empty;
				sWeekDayName = grdTeacherTT.HeaderRow.Cells[j].Text;
                oTableCell.Attributes.Add("title", Resources.LocalizedResources.Lect+ " " + (i + 1).ToString() + " [" + sWeekDayName + "]");

                if(bIsShortName == true)
				    sFilterWeekDay = " AND WeekDay_Short_Name= '" + sWeekDayName + "' ";
                else
                    sFilterWeekDay = " AND WeekDay_Name= '" + sWeekDayName + "' ";
				iLectureNo = i + 1;
				sFilterMaxLetures = " AND maxDaylectures >= " + iLectureNo;

				DataRow[] oArrDrNALectureNos = oDtLectureNumbers.Select("Lecture_Number = " + iLectureNo);

				string sStdDivToExcludeForLectNo = string.Empty;
				string sFilterToExcludeStdDiv = string.Empty;

				foreach (DataRow oRow in oArrDrNALectureNos)
					sStdDivToExcludeForLectNo += oRow["StandardDivision_Id"].ToString() + ",";
				if (!string.IsNullOrEmpty(sStdDivToExcludeForLectNo))
					sFilterToExcludeStdDiv = " AND Standard_Division_Id NOT IN (" + sStdDivToExcludeForLectNo.Substring(0, sStdDivToExcludeForLectNo.LastIndexOf(",")) + ")";

				ocmbTeacherSubj = new DropDownList();
				ocmbTeacherSubj.CssClass = S_CSSCLASS_COMBO;
				ocmbTeacherSubj.Width = 145;
				ocmbTeacherSubj.ID = "dr" + i.ToString() + "_" + j.ToString();
				grdTeacherTT.Rows[i].Cells[iCellIndex].Controls.Add(ocmbTeacherSubj);

				DataTable oDtAdditionalClasses = aoDs.Tables[I_ADDITIONAL_LECT_TABLE_INDEX];
				DataRow[] oArrAdditionalRows = oDtAdditionalClasses.Select("LectureNumber=" + iLectureNo + " AND WeekDayName='" + sWeekDayName + "'");
				if (oArrAdditionalRows.Length > Constants.I_ZERO)
				{
					ocmbTeacherSubj.Enabled = true;
					sAdditionalLecFilter = " AND Subject_Id = " + oArrAdditionalRows[0]["SubjectId"].ToString() +
										   " AND " + S_TEACHER_SUBJECT_ID_FIELD + "<>" + oArrAdditionalRows[0]["TeacherSubjectId"].ToString();
					bIsAdditionalLect = true;
				}

				oDrLectures = oDtTeacherSubjects.Select(S_TEACHER_ID_FIELD + " = " + iTeacherId.ToString() + sFilterWeekDay + sFilterMaxLetures + sFilter + sFilterToExcludeStdDiv, "Original_Standard_Id ASC, Original_Division_Id ASC, Original_Subject_Id ASC");
				ocmbTeacherSubj.Attributes.Add("title", Resources.LocalizedResources.Lectures +" " + (i + 1).ToString() + " [" + sWeekDayName + "]");
				ocmbTeacherSubj.ToolTip =Resources.LocalizedResources.For+ " - " + sWeekDayName + " : " + (i+1).ToString();
				ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                List<StayBackLectureDetails> lstStayBackLectureDetails = oExternalLecturesBL.GetStayBackLecturesForStandardsAssociatedToTeachers(iTeacherId, sWeekDayName, miSchoolId, miAcademicYearId, S_STAYBACK_LECTURE);

                List<DataRow> lstLectures = new List<DataRow>();
				ocmbTeacherSubj.Items.Clear();

				foreach (DataRow oDr in oDrLectures)
				{
					bool bFlag = lstStayBackLectureDetails.Where(lst => Convert.ToInt32(oDr.ItemArray[6]) == lst.StandardwiseDivisionId && iLectureNo == lst.LectureNo).Count() == 0;
					if (bFlag)
						ocmbTeacherSubj.Items.Add(new ListItem(oDr.ItemArray[0].ToString(), oDr.ItemArray[7].ToString()));
				}

				ocmbTeacherSubj.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
				ocmbTeacherSubj.CausesValidation = false;
				ocmbTeacherSubj.EnableViewState = true;

				if (ocmbTeacherSubj.Items.Count < Constants.I_TWO)
				{
					oTableCell.CssClass = S_CSS_NA;
					ocmbTeacherSubj.Visible = false;
					oTableCell.Text = S_TXT_LECTURE_NA;
					oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
				}

				bool bLockCellForMPTAssembly = false;
				if (Settings.IsStaybackApplicable)
				{
					if (chkStayback.Checked)
					{
						DataTable oDtStayback = aoDs.Tables[3];
						DataRow[] oArrRows = oDtStayback.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + iLectureNo);
						if (oArrRows.Length > Constants.I_ZERO)
						{
							SetVisibilityOfRow(sWeekDayName, iLectureNo, false);
							oTableCell.Text = sStayback;
							bLockCellForMPTAssembly = true;
						}
					}
					else
						SetVisibilityOfRow(sWeekDayName, iLectureNo, true);
				}
				if (Settings.IsAssemblyApplicable)
				{
					if (chkAssembly.Checked)
					{
                        DataTable oDtAssembly = aoDs.Tables[4];
                        DataRow[] oArrRows = oDtAssembly.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + iLectureNo);
					    if (oArrRows.Length > Constants.I_ZERO)
					    {
                            SetVisibilityOfRow(sAssemblyWeekday, iAssemblyLectNo, false);
                            oTableCell.Text = sAssembly;
                            bLockCellForMPTAssembly = true;
                        }
					    
					}
					else
						SetVisibilityOfRow(sAssemblyWeekday, iAssemblyLectNo, true);
				}

				if (Settings.IsMPTApplicable)
				{
					if (chkMPT.Checked)
					{
                        DataTable oDtMPT = aoDs.Tables[5];
                        DataRow[] oArrRows = oDtMPT.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + iLectureNo);
					    if (oArrRows.Length > Constants.I_ZERO)
					    {
                            SetVisibilityOfRow(sMPTWeekday, iMPTLectNo, false);
                            oTableCell.Text = sMPT;
                            bLockCellForMPTAssembly = true;
					    }
		            }
					else
						SetVisibilityOfRow(sMPTWeekday, iMPTLectNo, true);
				}

                if (Settings.IsWeeklyTestApplicable)
                {
                    if (chkWeeklyTest.Checked)
                    { 
                        DataTable oDTWeelyTest = aoDs.Tables[10];
                        DataRow[] oArrRows = oDTWeelyTest.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + iLectureNo);
                        if (oArrRows.Length > Constants.I_ZERO)
                        {
                            SetVisibilityOfRow(sWeekDayName, iWeeklyTestLectNo, false);
                            oTableCell.Text = sWeeklyTest;
                            bLockCellForMPTAssembly = true;
                        }
                    }
                    else
                        SetVisibilityOfRow(sWeekDayName, iWeeklyTestLectNo, true);
                }

				if (bLockCellForMPTAssembly)
				{
					ocmbTeacherSubj.Visible = false;
					oTableCell.Font.Bold = true;
					oTableCell.BackColor = System.Drawing.Color.FromArgb(208, 226, 238);
					oTableCell.Style.Add(HtmlTextWriterStyle.Color, "Black");
					oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "9pt");
				}

                ocmbTeacherSubj.ToolTip = Resources.LocalizedResources.For + " - " + sWeekDayName + " : " + (i + 1).ToString();
				ocmbTeacherSubj.SelectedValue = oTableCell.Text;
				if (ocmbTeacherSubj.Items.Count < Constants.I_TWO)
					ocmbTeacherSubj.Visible = false;

				ocmbTeacherSubj.SelectedValue = oTableCell.Text;
				if (ocmbTeacherSubj.SelectedIndex != Constants.I_ZERO)
				{
					oTableCell.CssClass = S_CSSCLASS_COMBO_SELECTED;
					ocmbTeacherSubj.CssClass = S_CSSCLASS_COMBO_SELECTED;
				}
				else
					ocmbTeacherSubj.CssClass = S_CSSCLASS_COMBO;

				if (bIsAdditionalLect)
				{
					oTableCell.CssClass = S_CSSCLASS_COMBO_SELECTED;
					ocmbTeacherSubj.Enabled = false;
					ocmbTeacherSubj.ForeColor = System.Drawing.Color.Black;
					ocmbTeacherSubj.BackColor = System.Drawing.Color.LightGray;
					ocmbTeacherSubj.Font.Bold = true;
				}
			}
		}
		DisplayLectureCount(aoDs, i);
		GetLectureCountsForTeachers();
	}

	protected void ocmbTeacherSubj_SelectedIndexChanged(object sender, EventArgs e)
	{
		DisplayLectureCount();
		UpdateSubjectLectureCount();
	}

	/// <summary>
	///  This method adds Week days columns to other rows of the grid.
	///   1.  Weekdays columns.
	///   2.  Dropdownlist(containing teacher-Subjects) to every newly generated cell.
	/// </summary>
	/// <param name="aoDs"></param>
	private void GenerateOtherColumns(DataSet aoDs)
	{
		int iStdDivId = Convert.ToInt32(cmbDivision.SelectedValue);
		int iStandardId = Convert.ToInt32(cmbStandard.SelectedItem.Value);
		int iStdDivisionId = Convert.ToInt32(cmbDivision.SelectedItem.Value);

		// dataset for comboes
		TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
		DataSet oDsTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectMaxLecDetails(miSchoolId, miAcademicYearId);
		DataTable oDtTeacherSubjects = oDsTeacherSubjects.Tables[I_WEEKDAY_TABLE_INDEX];
		DataTable oDtLectureNumbers = aoDs.Tables[I_LECT_NUM_TABLE_INDEX];
		int iRowCount = aoDs.Tables[0].Rows.Count;
		int iColCount = aoDs.Tables[0].Columns.Count - I_STATICBOUND_COLS_COUNT;
		DropDownList oDr;
		string sColName;
		int iCellIndex;
        bool bIsShortName = SchoolBase.Settings.DisplayShortNameOnTimeTableScreen;
		SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
		DataTable oDtClassTeacher = oSchoolWiseTeacherMasterBL.GetAssignedClassTeacher(miSchoolId, iStdDivisionId);
		DataTable oDtStayback = aoDs.Tables[2];
	    DataTable oDtAssembly = aoDs.Tables[5];
        DataTable oDtMPT = aoDs.Tables[6];
        DataTable oDTWeeklyTest = aoDs.Tables[7];
        for (int i = 0; i < iRowCount; i++)
		{
			for (int j = I_STATICBOUND_COLS_COUNT; j <= iColCount; j++)
			{
				TableCell oTableCell = new TableCell();
				oTableCell.HorizontalAlign = HorizontalAlign.Center;
				oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
				oTableCell.Wrap = false;
				oTableCell.Style.Add(HtmlTextWriterStyle.PaddingTop, "2px");
				oTableCell.Style.Add(HtmlTextWriterStyle.PaddingBottom, "2px");
				sColName = grdStdTimeTable.HeaderRow.Cells[j].Text;
				oTableCell.Text = aoDs.Tables[0].Rows[i][sColName].ToString();
				iCellIndex = grdStdTimeTable.Rows[i].Cells.Add(oTableCell);
				bool bIsAdditionalLect = false;

				string sWeekDayName = grdStdTimeTable.HeaderRow.Cells[j].Text;
				string sFilterWeekDay;
                if(bIsShortName==true)
                    sFilterWeekDay = " AND WeekDay_Short_Name= '" + sWeekDayName + "' ";
                else
                    sFilterWeekDay = " AND WeekDay_Name= '" + sWeekDayName + "' ";

				int iLectureNo = i + 1;
				string sFilterMaxLetures = " AND maxDaylectures >= " + iLectureNo;

				oDr = new DropDownList();
				oDr.Width = 175;
				oDr.CssClass = S_CSSCLASS_COMBO;
				oDr.ID = "dr" + i.ToString() + "_" + j.ToString();
                oDr.ToolTip = Resources.LocalizedResources.For + " - " + sWeekDayName + " : " + (i + 1).ToString();
				DataTable oDtAdditionalClasses = aoDs.Tables[I_ADDITIONAL_CLASS_LECT_TABLE_INDEX];
				DataRow[] oArrAdditionalRows = oDtAdditionalClasses.Select("LectureNumber=" + iLectureNo + " AND WeekDayName='" + sWeekDayName + "'");

				if (oArrAdditionalRows.Length > Constants.I_ZERO)
				{
					oDr.Enabled = true;
					bIsAdditionalLect = true;
				}
				else
				{
					// check if this lecture number is applicable for this std-division                    
					DataRow[] oLectRows = oDtLectureNumbers.Select("Lecture_Number = " + iLectureNo);
					oDr.Enabled = oLectRows.Length == 0 ? true : false;
				}

				DataRow[] oDrLectures = oDtTeacherSubjects.Select(S_STDDIV_ID_FIELD + " = " + iStdDivId.ToString() + sFilterWeekDay + sFilterMaxLetures);

				grdStdTimeTable.Rows[i].Cells[iCellIndex].Controls.Add(oDr);

				ControlUtility.FillDropDownList(oDrLectures, ref oDr, S_TEACHER_SUBJECT_ID_FIELD, S_TEACHER_SUBJECT_NAME_FIELD, Constants.S_SELECT);

                string sAssemblyMPTStaybackText = GetAssemblyMPTStayBackText(iLectureNo, sWeekDayName, oDtClassTeacher, oDtStayback, oDtAssembly, oDtMPT, oDTWeeklyTest);
				if (!string.IsNullOrEmpty(sAssemblyMPTStaybackText))
				{
					oTableCell.Text = sAssemblyMPTStaybackText;
					oDr.Visible = false;
					oTableCell.Font.Bold = true;
					oTableCell.BackColor = System.Drawing.Color.FromArgb(208, 226, 238);
					oTableCell.Style.Add(HtmlTextWriterStyle.Color, "Black");
					oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "9pt");
				}

				if (oDr.Items.Count < Constants.I_TWO)
				{
					oTableCell.CssClass = S_CSS_NA;
					oDr.Visible = false;
					oTableCell.Text = S_TXT_LECTURE_NA;
					oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
				}

				oDr.SelectedValue = oTableCell.Text;
				if (oDr.SelectedIndex != Constants.I_ZERO)
				{
					oTableCell.CssClass = S_CSS_NOTCLASS_TEACHER;
					oDr.CssClass = S_CSSCLASS_COMBO_SELECTED;
				}
				else
					oDr.CssClass = S_CSSCLASS_COMBO;

				if (bIsAdditionalLect)
				{
					oTableCell.CssClass = S_CSSCLASS_COMBO_SELECTED;
					oDr.Enabled = false;
					oDr.ForeColor = System.Drawing.Color.Black;
					oDr.BackColor = System.Drawing.Color.LightGray;
					oDr.Font.Bold = true;
				}
			}
		}
	}

	/// <summary>
	/// This method is used to get names of Assembly, MPT, StayBack.
	/// </summary>
	/// <param name="aiLectNo"></param>
	/// <param name="asWeekdayName"></param>
	/// <param name="aoDtClassTeacher"></param>
	/// <param name="aoDtStayback"></param>
	/// <returns></returns>
    private string GetAssemblyMPTStayBackText(int aiLectNo, string asWeekdayName, DataTable aoDtClassTeacher, DataTable aoDtStayback, DataTable oDtAssembly, DataTable aoDtMPT, DataTable oDTWeeklyTest)
	{
		string sDesc = string.Empty;
		if (Settings.IsAssemblyApplicable ||
			Settings.IsMPTApplicable ||
			Settings.IsStaybackApplicable)
		{
			string sAssembly = Settings.AssemblyName;
			string sMPT = Settings.MPTName;
			int iAssemblyLectNo = Settings.AssemblyLectNo;
			string sAssemblyWeekday = Settings.AssemblyWeekday;
			int iMPTLectNo = Settings.MPTLectNo;
			string sMPTWeekday = Settings.MPTWeekday;

			string sStayback = Settings.StaybackName;

			string sAssemblyApplicable = "N";
			string sMPTApplicable = "N";

			if (aoDtClassTeacher.Rows.Count > Constants.I_ZERO)
			{
				for (int iCount = 0; iCount < aoDtClassTeacher.Rows.Count; iCount++)
				{
					if (aoDtClassTeacher.Rows[iCount]["Assembly_Applicable"].ToString() == "Y")
						sAssemblyApplicable = aoDtClassTeacher.Rows[iCount]["Assembly_Applicable"].ToString();
					if (aoDtClassTeacher.Rows[iCount]["MPT_Applicable"].ToString() == "Y")
						sMPTApplicable = aoDtClassTeacher.Rows[iCount]["MPT_Applicable"].ToString();
				}
			}

		    if (Settings.IsAssemblyApplicable)
		    {
                //if (sAssemblyApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                // if (aiLectNo == iAssemblyLectNo && asWeekdayName.ToLower() == sAssemblyWeekday.ToLower())
                DataRow[] oArrRowsAssembly = oDtAssembly.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                 if (oArrRowsAssembly.Length > Constants.I_ZERO)
                    sDesc = sAssembly;
            }



		    if (Settings.IsMPTApplicable)
		    {
                //if (sMPTApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                //    if (aiLectNo == iMPTLectNo && asWeekdayName.ToLower() == sMPTWeekday.ToLower())
                DataRow[] oArrRowsMPT = aoDtMPT.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                if (oArrRowsMPT.Length > Constants.I_ZERO)
                    sDesc = sMPT;
            }
		  

			if (Settings.IsStaybackApplicable)
			{
				DataRow[] oArrRows = aoDtStayback.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
				if (oArrRows.Length > Constants.I_ZERO)
					sDesc = sStayback;
			}

            if (oDTWeeklyTest.Rows.Count > Constants.I_ZERO)
            {
                DataRow[] oArrRows = oDTWeeklyTest.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                if (oArrRows.Length > Constants.I_ZERO)
                    sDesc = "Weekly Test";
            }
		}
		return sDesc;
	}

	#endregion " Fill Grid "

	#region " Save "

	/// <summary>
	/// This method populate SchoolTimeTableMasterBL object and returns same.
	/// </summary>
	/// <returns></returns>
	private SchoolTimeTableMasterBL GetObject()
	{
		SchoolTimeTableMasterBL oSchoolTimeTableMasterBL = new SchoolTimeTableMasterBL();
		int iStdDivId = Convert.ToInt32(cmbDivision.SelectedValue);
		oSchoolTimeTableMasterBL.AcademicYearId = miAcademicYearId;
		oSchoolTimeTableMasterBL.SchoolId = miSchoolId;
		oSchoolTimeTableMasterBL.InsertedById = miUserId;
		oSchoolTimeTableMasterBL.StandardDivisionId = iStdDivId;
		return oSchoolTimeTableMasterBL;
	}

	/// <summary>
	/// This method creates an XML for Time table
	/// </summary>
	/// <returns>
	/// Array of xml strings.
	//1. <DaywiseTimeTableMaster><DaywiseTimeTable Weekday_Id ="810"/></DaywiseTimeTableMaster>
	//2. <DaywiseTimeTableDetails><DaywiseTimeTableDetail WeekDay_Id ="810" Lecture_Number ="1" Teacher_ID ="7" Subject_Id ="1303" />
	//<DaywiseTimeTableDetail WeekDay_Id ="810" Lecture_Number ="2" Teacher_ID ="7" Subject_Id ="1303" />
	//</DaywiseTimeTableDetails>
	/// </returns>
	private string[] GetXMLForTeacherTimeTable(DataTable oDtTeacherSubjects)
	{
		string[] sArrStrXml = new string[2];
		int iRowCount = grdTeacherTT.Rows.Count;
		int iColumnCount = grdTeacherTT.Rows[0].Cells.Count;
		const string S_ELEMENT = "element";

		// This variable is set to
		// 1. true if the week day entry shud be made in master
		// 2. false otherwise
		ArrayList arrIncludedStdDivs = new ArrayList();
		XmlDocument oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement("DaywiseTimeTableMaster");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTableMaster", string.Empty);

		XmlDocument oDocDetail = new XmlDocument();
		XmlElement rootDetail = oDocDetail.CreateElement("DaywiseTimeTableDetails");
		XmlElement DetailRootNode = oDocDetail.CreateElement("DaywiseTimeTableDetails");
		// create root level node.
		DataSet oDs = (DataSet)grdTeacherTT.DataSource;
		int iTechaerId = Convert.ToInt32(cmbTeachers.SelectedValue);

		// as weekdays are diplayed as columns 
		for (int j = I_STATICBOUND_COLS_COUNT; j < iColumnCount; j++)
		{
			string sAtrrName;
			XmlAttribute attr;
			XmlNode oXmlNode;
			XmlNode oXmlDetailNode;
			arrIncludedStdDivs.Clear();
			int iWeekDayId = Convert.ToInt32(oDs.Tables[1].Rows[j - I_STATICBOUND_COLS_COUNT]["Weekday_id"]);
			for (int i = 0; i < iRowCount; i++)
			{
				if (grdTeacherTT.Rows[i].Cells[j].Controls.Count > Constants.I_ZERO)
				{
					DropDownList oCmb = (DropDownList)grdTeacherTT.Rows[i].Cells[j].Controls[0];
					if (!oCmb.SelectedValue.Equals("0"))
					{
						DataRow[] oDr = oDtTeacherSubjects.Select(S_TEACHER_SUBJECT_ID_FIELD + "=" + oCmb.SelectedValue);
						int iStdDivId = Convert.ToInt32(oDr[0][S_STDDIV_ID_FIELD]);

						if (!arrIncludedStdDivs.Contains(iStdDivId))
						{
							arrIncludedStdDivs.Add(iStdDivId);
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
						}

						oXmlDetailNode = oDocDetail.CreateNode(S_ELEMENT, "DaywiseTimeTableDetail", string.Empty);

						sAtrrName = "WeekDay_Id";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = iWeekDayId.ToString();
						oXmlDetailNode.Attributes.Append(attr);

						sAtrrName = "Teacher_ID";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = iTechaerId.ToString();
						oXmlDetailNode.Attributes.Append(attr);

						sAtrrName = "Standard_Division_Id";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = iStdDivId.ToString();
						oXmlDetailNode.Attributes.Append(attr);

						int iLectureNo = i + 1;
						sAtrrName = "Lecture_Number";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = iLectureNo.ToString();
						oXmlDetailNode.Attributes.Append(attr);

						sAtrrName = "Subject_Id";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = oDr[0]["Subject_Id"].ToString();
						oXmlDetailNode.Attributes.Append(attr);

						DetailRootNode.AppendChild(oXmlDetailNode);
						rootDetail.AppendChild(DetailRootNode);
					}
				}
			}
		}
		if (string.IsNullOrEmpty(root.InnerXml))
			root.AppendChild(oXmlRootNode);
		if (string.IsNullOrEmpty(rootDetail.InnerXml))
			rootDetail.AppendChild(DetailRootNode);

		sArrStrXml[0] = root.InnerXml;
		sArrStrXml[1] = rootDetail.InnerXml;
		return sArrStrXml;
	}

	/// <summary>
	/// This method creates an XML for Time table.
	/// </summary>
	/// <returns>
	// 1. <DaywiseTimeTableMaster><DaywiseTimeTable Weekday_Id ="810"/></DaywiseTimeTableMaster>
	// 2. <DaywiseTimeTableDetails><DaywiseTimeTableDetail WeekDay_Id ="810" Lecture_Number ="1" Teacher_ID ="7" Subject_Id ="1303" />
	//    <DaywiseTimeTableDetail WeekDay_Id ="810" Lecture_Number ="2" Teacher_ID ="7" Subject_Id ="1303" />
	//    </DaywiseTimeTableDetails> 
	/// </returns>
	private string[] GetXMLForTimeTable(DataTable oDtTeacherSubjects)
	{
		string[] sArrStrXml = new string[2];
		int iRowCount = grdStdTimeTable.Rows.Count;
		int iColumnCount = grdStdTimeTable.Rows[0].Cells.Count;
		const string S_ELEMENT = "element";
		// This variable is set to
		// 1. true if the week day entry shud be made in master
		// 2. false otherwise
		bool bMaster = false;

		XmlDocument oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement("DaywiseTimeTableMaster");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTableMaster", string.Empty);

		XmlDocument oDocDetail = new XmlDocument();
		XmlElement rootDetail = oDocDetail.CreateElement("DaywiseTimeTableDetails");
		XmlElement DetailRootNode = oDocDetail.CreateElement("DaywiseTimeTableDetails");
		// create root level node.
		DataSet oDs = (DataSet)grdStdTimeTable.DataSource;

		// as weekdays are diplayed as columns 
		for (int j = I_STATICBOUND_COLS_COUNT; j < iColumnCount; j++)
		{
			string sAtrrName;
			XmlAttribute attr;
			XmlNode oXmlNode;
			XmlNode oXmlDetailNode;
			bMaster = false;
			int iWeekDayId = Convert.ToInt32(oDs.Tables[1].Rows[j - I_STATICBOUND_COLS_COUNT]["Weekday_id"]);
			for (int i = 0; i < iRowCount; i++)
			{
				if (grdStdTimeTable.Rows[i].Cells[j].Controls.Count > 0)
				{
					DropDownList oCmb = (DropDownList)grdStdTimeTable.Rows[i].Cells[j].Controls[0];
                    if (!oCmb.SelectedValue.Equals("0"))
					{
						bMaster = true;
						DataRow[] oDr = oDtTeacherSubjects.Select(S_TEACHER_SUBJECT_ID_FIELD + "=" + oCmb.SelectedValue);

						oXmlDetailNode = oDocDetail.CreateNode(S_ELEMENT, "DaywiseTimeTableDetail", string.Empty);

						sAtrrName = "WeekDay_Id";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = iWeekDayId.ToString();
						oXmlDetailNode.Attributes.Append(attr);

						int iLectureNo = i + 1;
						sAtrrName = "Lecture_Number";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = iLectureNo.ToString();
						oXmlDetailNode.Attributes.Append(attr);

						string sTeacherId = oDr[0]["Teacher_Id"].ToString();
						sAtrrName = "Teacher_ID";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = sTeacherId;
						oXmlDetailNode.Attributes.Append(attr);

						sAtrrName = "Subject_Id";
						attr = oDocDetail.CreateAttribute(sAtrrName);
						attr.Value = oDr[0]["Subject_Id"].ToString();
						oXmlDetailNode.Attributes.Append(attr);

						DetailRootNode.AppendChild(oXmlDetailNode);
						rootDetail.AppendChild(DetailRootNode);
					}
				}
			}
			if (bMaster)
			{
				oXmlNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTable", string.Empty);

				sAtrrName = "Weekday_Id";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = iWeekDayId.ToString();
				oXmlNode.Attributes.Append(attr);

				oXmlRootNode.AppendChild(oXmlNode);
				root.AppendChild(oXmlRootNode);
			}
		}
		sArrStrXml[0] = root.InnerXml;
		sArrStrXml[1] = rootDetail.InnerXml;
		return sArrStrXml;
	}

	/// <summary>
	/// This method is used to get XML of teacher details of Assembly, MPT, StayBack. 
	/// </summary>
	/// <returns></returns>
	private string GetXMLForTeacherDetails()
	{
		const string S_ELEMENT = "element";
		XmlDocument oDoc = new XmlDocument();

		// Create a root level element.
		XmlElement root = oDoc.CreateElement("TeacherMaster");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "TeacherMaster", string.Empty);
		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "Teacher", string.Empty);

		XmlAttribute attr;

		string sAtrrName = "Assembly_Applicable";
		attr = oDoc.CreateAttribute(sAtrrName);
		if (chkAssembly.Checked)
			attr.Value = Constants.C_YES.ToString();
		else
			attr.Value = Constants.C_NO.ToString();

		oXmlNode.Attributes.Append(attr);
		oXmlRootNode.AppendChild(oXmlNode);

		sAtrrName = "MPT_Applicable";
		attr = oDoc.CreateAttribute(sAtrrName);
		if (chkMPT.Checked)
			attr.Value = Constants.C_YES.ToString();
		else
			attr.Value = Constants.C_NO.ToString();
		oXmlNode.Attributes.Append(attr);
		oXmlRootNode.AppendChild(oXmlNode);

		sAtrrName = "Stayback_Applicable";
		attr = oDoc.CreateAttribute(sAtrrName);
		if (chkStayback.Checked)
			attr.Value = Constants.C_YES.ToString();
		else
			attr.Value = Constants.C_NO.ToString();
		oXmlNode.Attributes.Append(attr);
		oXmlRootNode.AppendChild(oXmlNode);

		root.AppendChild(oXmlRootNode);
		return root.InnerXml;
	}

	/// <summary>
	/// This method is used to get XML of additional lectures.
	/// </summary>
	/// <returns></returns>
	private string GetXMLForAdditionalLecture()
	{
		const int LECTURE_NUMBER_CELL = Constants.I_ONE;
		if (grdAdditionalClasses.Rows.Count > Constants.I_ZERO)
		{
			const string S_ELEMENT = "element";
			XmlDocument oDoc = new XmlDocument();

			// Create a root level element.
			XmlElement root = oDoc.CreateElement("AdditionalLect");
			XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "AdditionalLect", string.Empty);

			for (int iCount = 0; iCount < grdAdditionalClasses.Rows.Count; iCount++)
			{
				// Create root xml element.
				XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "AdditionalLect", string.Empty);

				string sAtrrName = "WeekDay_Id";
				XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = grdAdditionalClasses.DataKeys[iCount]["WeekdayId"].ToString();
				oXmlNode.Attributes.Append(attr);

				sAtrrName = "Lecture_Number";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = grdAdditionalClasses.Rows[iCount].Cells[LECTURE_NUMBER_CELL].Text.ToString();
				oXmlNode.Attributes.Append(attr);

				sAtrrName = "Teacher_ID";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = grdAdditionalClasses.DataKeys[iCount]["TeacherId"].ToString();
				oXmlNode.Attributes.Append(attr);

				sAtrrName = "Subject_Id";
				attr = oDoc.CreateAttribute(sAtrrName);
				attr.Value = grdAdditionalClasses.DataKeys[iCount]["SubjectId"].ToString();
				oXmlNode.Attributes.Append(attr);

				// Add the node to root node.
				oXmlRootNode.AppendChild(oXmlNode);
			}
			// Add the root node to document element. 
			root.AppendChild(oXmlRootNode);

			// return the string generated.
			return root.InnerXml;
		}
		else
			return null;
	}

	#region  " Validate "
	/// <summary>
	/// This method checks if the dataset contains a row with error messages.
	/// And prints the messages on the label.
	/// </summary>
	/// <param name="oDs"></param>
	/// <returns>
	/// true: if no error messages.
	/// false otherwise
	/// </returns>
	private bool ValidateData(DataSet oDs)
	{
		bool bReturn = true;
		string sMsg = string.Empty;
		string[] sArrMsgs = new string[6];
		if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
		{
			sArrMsgs[0] = oDs.Tables[0].Rows[0][S_OVERLAP_ERR_FIELD].ToString();
			sArrMsgs[1] = oDs.Tables[0].Rows[0][S_TEACHERWEEKLY_LECTURES_ERR_FIELD].ToString();
			sArrMsgs[2] = oDs.Tables[0].Rows[0][S_TEACHERWEEKDAY_LECTURES_ERR_FIELD].ToString();
			sArrMsgs[3] = oDs.Tables[0].Rows[0][S_SUBJECTWEEKDAY_LECTURES_ERR_FIELD].ToString();
			sArrMsgs[4] = oDs.Tables[0].Rows[0][S_SUBJECTWEEKDAY_ASSLECTURES_ERR_FIELD].ToString();
            sArrMsgs[5] = (oDs.Tables[0].Rows[0][S_EXTERNAL_LECTURE_ERR_FIELD].ToString()).Replace("Teacher", Resources.LocalizedResources.Teacher).Replace("is already assigned for", Resources.LocalizedResources.IsAlreadyAssignedFor);
			sMsg = FormatErrorMessage(sArrMsgs);
		}
		if (!string.IsNullOrEmpty(sMsg))
		{
            if (sMsg.Remove(0, 5) == sArrMsgs[3] || sMsg==sArrMsgs[3])
            {
                ScriptManager.RegisterStartupScript(btnSave, this.GetType(), "ShowPopup", "ShowPopup(this,'" + sArrMsgs[3].ToString() + "','" + sMsg.ToString() + "')", true);
                bReturn = false;
            }
            else 
            {
                lblError.Visible = true;
                lblError.Text = sMsg;
                bReturn = false;
            }		
		}
		return bReturn;
	}
	public void EmptyGrid()
	{
		grdStdTimeTable.DataSource = null;
		grdStdTimeTable.DataBind();
		grdTeacherTT.DataSource = null;
		grdTeacherTT.DataBind();
		grdSubjectLect.DataSource = null;
		grdSubjectLect.DataBind();
		grdAdditionalClasses.DataSource = null;
		grdAdditionalClasses.DataBind();
	}

	/// <summary>
	/// This method formats the message in such a format that can be displayed. 
	/// </summary>
	/// <param name="sArrMsgs"></param>
	/// <returns>
	/// Well formatted message.
	/// </returns>
	private string FormatErrorMessage(string[] sArrMsgs)
	{
		string sReturnMsg = string.Empty;
		for (int i = 0; i < sArrMsgs.Length; i++)
		{
			if (!string.IsNullOrEmpty(sArrMsgs[i]))
			{
				if (!string.IsNullOrEmpty(sReturnMsg))
					sReturnMsg = sReturnMsg + "<BR>" + sArrMsgs[i];
				else
					sReturnMsg = sArrMsgs[i];
			}
		}
		return sReturnMsg;
	}

	/// <summary>
	/// This method is used to decrypt the encrypted querystring.
	/// </summary>
	private void ReadQuerystring()
	{
		try
		{
            if (QueryString["TeacherId"] != null)
                hidTeacherId.Value = QueryString["TeacherId"];
            else if (QueryString["StandardId"] != null)
            {
                hidStandardId.Value = QueryString["StandardId"];
                hidDivisionId.Value = QueryString["DivisionId"];
            }
		}
		catch (Exception)
		{
			MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
		}
	}

	/// <summary>
	/// This method is used to hide additional lectures if extra lecture is added.
	/// </summary>
	/// <param name="sWeekDayName"></param>
	/// <param name="iLectureNo"></param>
	/// <param name="abFlag"></param>
	private void SetVisibilityOfRow(string sWeekDayName, int aiLectureNo, bool abFlag)
	{
		foreach (GridViewRow row in grdAdditionalClasses.Rows)
		{
			if (row.Cells[1].Text == aiLectureNo.ToString() && row.Cells[0].Text == sWeekDayName)
			{
				row.Visible = abFlag;
				if(!abFlag)
				hidHidenLectures.Value = (hidHidenLectures.Value.ToInt() + 1).ToString();								
			}

			if ((grdAdditionalClasses.Rows.Count == hidHidenLectures.Value.ToInt() ))
				grdAdditionalClasses.Visible = false;
			else
				grdAdditionalClasses.Visible = true;
		}
	}

	/// <summary>
	/// This procedure is used to delete lecture which are configured in time table before configuring external lectures.
	/// </summary>
	private void DeleteAdditionalLecture()
	{
		string sAssemblyWeekday = string.Empty;
		string sIsMPTAppicable = Settings.IsMPTApplicable ? Constants.S_YES : Constants.S_NO;
		int iAssemblyLectNo = 0;

		string sMPTWeekday = string.Empty;
		string sIsAppicable= Settings.IsAssemblyApplicable ? Constants.S_YES : Constants.S_NO ;
		int iMPTLectNo = 0;

        string sWeeklyTest = string.Empty;
        string sIsWeeklyTestApplicable = Settings.IsWeeklyTestApplicable ? Constants.S_YES:Constants.S_NO;
        int iWeeklyLectNo = 0;

		bool sStayBack = false;

		int iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
		if (chkStayback.Checked)
			sStayBack = true;


		if (chkMPT.Checked)
		{
			sMPTWeekday = Settings.MPTWeekday;
			iMPTLectNo = Settings.MPTLectNo;			
		}

		if (chkAssembly.Checked)
		{
			sAssemblyWeekday = Settings.AssemblyWeekday;
			iAssemblyLectNo = Settings.AssemblyLectNo;
		}

        if (chkWeeklyTest.Checked)
        {
            sWeeklyTest = Settings.WeeklyTestName;
            iWeeklyLectNo = Settings.WeeklyTestLectNo;
        }
			SchoolTimeTableMasterBL.DeleteAdditionalLecture(iTeacherId,iMPTLectNo,iAssemblyLectNo, sAssemblyWeekday,sMPTWeekday, sStayBack, miSchoolId, miAcademicYearId);	
	}

	#endregion " Validate "

	#endregion " Save "   

	#endregion " Helping Methods "

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidValSelectTeacher.Value = Resources.LocalizedResources.ValSelectTeacher;
        hidAreYouSureResetTimetable.Value = Resources.LocalizedResources.AreYouSureResetTimetable;
        hidValDeleteAdditionallectures.Value = Resources.LocalizedResources.ValDeleteAdditionallecture;
        hidValDeleteOptionallecture.Value = Resources.LocalizedResources.ValDeleteOptionallecture;
        hidValDivisionSelected.Value = Resources.LocalizedResources.ValDivisionSelected;
        btnShow.Text = oResourceManager.GetString(hidBtnShow.Value.Replace(" ", string.Empty));
        if (Settings.IsAssemblyApplicable)
            chkAssembly.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.AssemblyName.Replace(" ", string.Empty)) + " " + Resources.LocalizedResources.Applicable;
        if (Settings.IsMPTApplicable)        
            chkMPT.Text = Resources.LocalizedResources.Is + " " + Settings.MPTName + " " + Resources.LocalizedResources.Applicable;                    
        if (Settings.IsStaybackApplicable)
            chkStayback.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.StaybackName.Replace(" ", string.Empty)) + " " + Resources.LocalizedResources.Applicable;
        if(Settings.IsWeeklyTestApplicable)
            chkWeeklyTest.Text = Resources.LocalizedResources.Is + " "+ Settings.WeeklyTestName + " " + Resources.LocalizedResources.Applicable;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

	#endregion " Private Methods "
}
