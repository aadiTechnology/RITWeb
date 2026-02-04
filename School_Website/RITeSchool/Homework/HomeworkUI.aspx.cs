using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Linq;

/// <summary>
/// This class is used to show class and subjects.
/// </summary>
public partial class HomeworkUI : SchoolBase
{
    #region
    private enum LSTVIEW_TYPES
    {
    MySubjects = 0,
    MyClass = 1
    }
    #endregion
    #region "Events"
    /// <summary>
	/// This event is used to initialize controls, read query string and apply hover effect.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
    {
		try
		{
			if (CheckPreCondition())
			{
				InitializeMemberVariables();
				if (!IsPostBack)
				{
					FillTeachersComboBox();
                    FillTeachersClassComboBox();
					GetQueryString();
                    FillClassSubjectListview();
                    SetOwnClassVisibility();
                    SetHomelogOption();
                    //HideDailyLogBtn();
                 }
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }


	/// <summary>
	/// This event is used to fill subject class listview, which shows class and subjects.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
            FillTeachersClassComboBox();
            ResetListviewData(lstViewSubjectTeacher);
            SetOwnClassVisibility();
            SetHomelogOption();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set postback url to add image button which redirects to 'Assign Homework' screen.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstViewSubjectTeacher_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				ImageButton imgBtnAdd = oCurrentItem.FindControl("imgBtnAdd") as ImageButton;
				int iSubjectId = ((System.Data.DataRowView)oCurrentItem.DataItem).Row.ItemArray[Constants.I_EIGHT].ToInt();
				int iTeacherId = cmbTeachers.SelectedValue.ToInt();
				int iStdDivId = ((System.Data.DataRowView)oCurrentItem.DataItem).Row.ItemArray[Constants.I_SIX].ToInt();
				string sClass = ((System.Data.DataRowView)oCurrentItem.DataItem).Row.ItemArray[10].ToString();
                
                string sQueryString = "SubjectId=" + iSubjectId + "&TeacherId=" + iTeacherId + "&Class=" + sClass + "&StdDivId=" + iStdDivId + "&Teacher=" + cmbTeachers.SelectedItem.Text + "&ListViewType=" + LSTVIEW_TYPES.MySubjects.ToInt();
				imgBtnAdd.PostBackUrl = "~/RITeSchool/Homework/AssignHomeworkUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is fired when user changed class combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillClassSubjectListview();
            SetOwnClassVisibility();
            SetHomelogOption();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   /// <summary>
   /// This event is fired when my subject list view is bind with records.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void lstViewClassSubject_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton imgBtnAdd = oCurrentItem.FindControl("imgBtnAdd") as ImageButton;
                int iSubjectId = ((System.Data.DataRowView)oCurrentItem.DataItem).Row.ItemArray[Constants.I_EIGHT].ToInt();
                int iTeacherId = cmbTeachers.SelectedValue.ToInt();
                int iStdDivId = ((System.Data.DataRowView)oCurrentItem.DataItem).Row.ItemArray[Constants.I_SIX].ToInt();
                string sClass = ((System.Data.DataRowView)oCurrentItem.DataItem).Row.ItemArray[10].ToString();
                string sQueryString = "SubjectId=" + iSubjectId + "&TeacherId=" + iTeacherId + "&Class=" + sClass + "&StdDivId=" + iStdDivId + "&Teacher=" + cmbTeachers.SelectedItem.Text + "&ListViewType=" + LSTVIEW_TYPES.MyClass.ToInt();
                imgBtnAdd.PostBackUrl = "~/RITeSchool/Homework/AssignHomeworkUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to add homework log.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString1 = "ClassName=" + cmbClass.SelectedItem.Text + "&StdDivId=" + cmbClass.SelectedValue;
            MasterPage oMaster = this.Master as MasterPage;
            oMaster.RedirectToNextPage("~/RITeSchool/Homework/HomeworkDailyLogsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString1));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
	#endregion

	#region

    /// <summary>
    /// This method is used to fill  class comobox.
    /// </summary>
    private void FillTeachersClassComboBox()
    {
        // get all class teachers
        TeacherSubjectAssignmentCollectionBL oSubjectTeacherBL = new TeacherSubjectAssignmentCollectionBL();
        DataTable oDtSubjectTeachersClass = oSubjectTeacherBL.RetriveSubjectTeacherClass(miSchoolId,miAcademicYearId,cmbTeachers.SelectedValue.ToInt());
        if (!CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.HomeworkAssignment) && moUserRole == Constants.UserRoles.Teacher)
        {
            if (oDtSubjectTeachersClass.Rows.Count > 0)
                ControlUtility.FillDropDownList(oDtSubjectTeachersClass, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, string.Empty);
        }
        else
            ControlUtility.FillDropDownList(oDtSubjectTeachersClass, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_SELECT);
    }

    /// <summary>
    /// this method is decide to visibility of my subject listview
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetOwnClassVisibility()
    {
        if (Settings.EnableHomeworkMySubjectListView == true)
        {
            tblMyClass.Visible = true;
            FillMyClassSubjectListview();
        }
        else
            tblMyClass.Visible = false;

    }
	/// <summary>
	/// This method is used to fill  subject-teacher comobox.
	/// </summary>
	private void FillTeachersComboBox()
	{
		// get all class teachers
		TeacherSubjectAssignmentCollectionBL oSubjectTeacherBL = new TeacherSubjectAssignmentCollectionBL();
		
        int iTeacherId = 0;
        if (Session[Constants.S_SESSION_TEACHER_ID] != null)
            iTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();

        char IsFullAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.HomeworkAssignment);

        if (IsFullAccess.ToString() == "Y")
            iTeacherId = 0;

        hidUserHasFullAccess.Value = IsFullAccess.ToString();

        DataTable oDtSubjectTeachers = oSubjectTeacherBL.RetriveTeachersForHomework(miSchoolId, miAcademicYearId, Settings.EnableHomeworkMySubjectListView, iTeacherId);

		if (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.HomeworkAssignment) != 'Y' && moUserRole == Constants.UserRoles.Teacher)
		{
            DataRow[] oDrTeacherSubject = oDtSubjectTeachers.Select("Teacher_Id=" + iTeacherId);
			if (oDrTeacherSubject.Length > 0)
			{
				ControlUtility.FillDropDownList(oDrTeacherSubject, ref cmbTeachers, Constants.S_TEACHER_ID_FIELD, Constants.S_TEACHER_NAME_FIELD, string.Empty);
                FillTeachersClassComboBox();
			}
			else
				ControlUtility.FillDropDownList(oDrTeacherSubject, ref cmbTeachers, Constants.S_TEACHER_ID_FIELD, Constants.S_TEACHER_NAME_FIELD, Constants.S_SELECT);
		}
		else
			ControlUtility.FillDropDownList(oDtSubjectTeachers, ref cmbTeachers, Constants.S_TEACHER_ID_FIELD, Constants.S_TEACHER_NAME_FIELD, Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to read query string.
	/// </summary>
	private void GetQueryString()
	{
		if (QueryString.Count <= 0)
			return;

        if (QueryString["TeacherId"] != null)
        {
            cmbTeachers.SelectedValue = QueryString["TeacherId"];
            cmbTeachers_SelectedIndexChanged(cmbTeachers, null);
        }

        if (QueryString["StdDivId"] != null)
            cmbClass.SelectedValue = QueryString["StdDivId"];
	}

    
    /// <summary>
    /// This method is used to fill listview which shows classes and subjects.
    /// </summary>
    private void FillClassSubjectListview()
    {
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        DataTable oDtTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectDetails(miSchoolId, miAcademicYearId);
            var query = from Record in oDtTeacherSubjects.AsEnumerable() where Record.Field<int>("Teacher_Id") == cmbTeachers.SelectedValue.ToInt() && Record.Field<int>("Standard_Division_Id") == cmbClass.SelectedValue.ToInt() select Record;
            if (query.Any())
            {
                DataTable oDtFilterData = cmbTeachers.SelectedValue != Constants.S_ZERO ? query.CopyToDataTable() : null;
                lstViewSubjectTeacher.DataSource = oDtFilterData;
                lstViewSubjectTeacher.DataBind();
            }
            else
                ResetListviewData(lstViewSubjectTeacher);
    }


    /// <summary>
    /// This method is used to fill listview which shows classes and subjects.
    /// </summary>
    private void FillMyClassSubjectListview()
    {   
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        DataTable oDtTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectDetails(miSchoolId, miAcademicYearId);

        int iStdDivId = 0;
        DataRow[] drArray = oDtTeacherSubjects.Select("Is_ClassTeacher = 'Y' AND Teacher_Id=" + cmbTeachers.SelectedValue);
        if(drArray.Length > 0)
            iStdDivId = drArray[0]["Standard_Division_Id"].ToInt();

        if (iStdDivId == 0)
            iStdDivId = oTeacherSubjectAssignmentBL.GetStdDivId(miSchoolId, miAcademicYearId, cmbTeachers.SelectedValue.ToInt());

        if (iStdDivId == cmbClass.SelectedValue.ToInt())
        {
            tblMyClass.Visible = true;
            var query = from Record in oDtTeacherSubjects.AsEnumerable() where Record.Field<int>("Teacher_Id") != cmbTeachers.SelectedValue.ToInt() && Record.Field<int>("Standard_Division_Id") == iStdDivId select Record;
            DataTable oDtFilterData;

            if (query.Count() > 0)
            {
                oDtFilterData = cmbTeachers.SelectedValue != Constants.S_ZERO ? query.CopyToDataTable() : null;
                lstViewClassSubject.DataSource = oDtFilterData;
                lstViewClassSubject.DataBind();
            }
            else
                ResetListviewData(lstViewClassSubject);
        }
        else
            ResetListviewData(lstViewClassSubject);
    }

    /// <summary>
    /// This method is used t reset class subjects.
    /// </summary>
    private void ResetListviewData(ListView aoListView)
    {
        aoListView.DataSource = null;
        aoListView.DataBind();
    }

	/// <summary>
	/// This function checks the preconditons of subject-teacher assignment, checks whether subject-teacher assignment is done or not.
	/// </summary>
	/// <returns></returns>
	private bool CheckPreCondition()
	{
		bool bReturn = false;
		string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.HomeworkAssignment);
		if (sLinks.Equals(string.Empty))
		{
			trPrecondition.Visible = false;
			bReturn = true;
		}
		else
		{
			trPrecondition.Visible = true;
			divErr.InnerHtml = sLinks;
			trControls.Visible = false;
		}

		return bReturn;
	}

    /// <summary>
    /// This method is sued to set homework log option.
    /// </summary>
    private void SetHomelogOption()
    {
        if (SchoolBase.Settings.AllowHomewirkDailyLog == true)
        {
            if (moUserRole == Constants.UserRoles.Teacher)
            {
                int iStdDivId = 0;
                if (Session[Constants.S_SESSION_TEACHER_STDDIV_ID] != null && Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToString() != string.Empty)
                    iStdDivId = Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToInt();
                if ((Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.S_YES && iStdDivId != 0 && iStdDivId == cmbClass.SelectedValue.ToInt()) || (hidUserHasFullAccess.Value == Constants.S_YES && cmbClass.SelectedValue != Constants.S_ZERO))
                    btnAdd.Visible = true;
                else
                    btnAdd.Visible = false;
            }
            else
                btnAdd.Visible = false;
        }
    }

    private void HideDailyLogBtn()
    {
        if (Settings.AllowHomewirkDailyLog == false)
            btnAdd.Visible = false;
        else
            btnAdd.Visible = true;
    }
	
	#endregion
}