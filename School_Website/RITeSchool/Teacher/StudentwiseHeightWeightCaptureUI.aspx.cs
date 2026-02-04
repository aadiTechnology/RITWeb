// File Name  : StudentwiseHeightWeightCaptureUI.aspx.cs
// Created By : Yogesh
// Date       : 10 Oct 14
// Description: This class is used to capture students termwise height weight details.
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StudentEntities;
using Utility;
using System.Drawing;

public partial class StudentwiseHeightWeightCaptureUI : ExportDataTable
{

    #region DATA MEMBER(S)
    TermwiseStudentHeightWeightMasterBL moTermwiseStudentHeightWeightMasterBL = null;
    #endregion

    #region EVENT(S)

    /// <summary>
    /// This event is used for load to teachers combo box and Term combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                base.ApplyMouseHoverEffect(new List<Button> { btnBack,btnSave });
                this.FillTeachersComboBox();
                this.FillTermComboBox();
                if (cmbTeachers.Enabled == false && cmbTermName.SelectedValue == Constants.S_ONE)
                {
                    this.FillStudentDetailsListview();
                    CheckPublishedStatus();
                }
                cmbTeachers.Focus();
                SetPostbackUrl();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to capture selected index change of teachers combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FillStudentDetailsListview();
            CheckPublishedStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to capture selected index change of Term combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTermName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            this.FillStudentDetailsListview();
            CheckPublishedStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            Label oLblRollNo = e.Item.FindControl("lblRollNo") as Label;
            Label oLblName = e.Item.FindControl("lblName") as Label;

            ListViewDataItem lstDataItem = e.Item as ListViewDataItem;

            if (oLblRollNo != null)
                oLblRollNo.Text = ((StudentInfoForHeightWeight)lstDataItem.DataItem).RollNo.ToString();
            if (oLblName != null)
                oLblName.Text = ((StudentInfoForHeightWeight)lstDataItem.DataItem).StudentName;

            if (((StudentInfoForHeightWeight)lstDataItem.DataItem).IsLeftStudent == Constants.I_ONE)
                oLblRollNo.ForeColor = oLblName.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update the current height weight status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            this.UpdateHeightWeight();
            lblMessage.Text = Resources.LocalizedResources.HeightWeightUpdateMsg;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region PRIVATE METHODS(S)

    /// <summary>
    /// This method is used to check published status of exam.
    /// </summary>
    private void CheckPublishedStatus()
    {
        bool bIsPublishedStatus;

        GetFinalExamPublishedStatus(out bIsPublishedStatus);
        if (bIsPublishedStatus == true)
        {
            lstvwStudentDetails.Enabled = false;
            btnSave.Enabled = false;
        }
        else
        {
            lstvwStudentDetails.Enabled = true;
            btnSave.Enabled = true;
        }

    }

    /// <summary>
    /// This method is used to Get Final Exam published status.
    /// </summary>
    /// <returns></returns>
    private bool GetFinalExamPublishedStatus(out bool abIsPublishedStatus)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        return oStudentwiseRemarkMasterBL.GetFinalPublishedExamStatus(Convert.ToInt32(cmbTeachers.SelectedValue), miSchoolId, Convert.ToInt32(cmbTermName.SelectedValue), miAcademicYearId, out abIsPublishedStatus);
    }

    /// <summary>
    /// This method is used to fill term combo box.
    /// </summary>
    private void FillTermComboBox()
    {
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbTermName, "Value_Member", "Display_Member", string.Empty);
        
    }

    /// <summary>
    /// This method is used to fill teacher combo box.
    /// </summary>
    private void FillTeachersComboBox()
    {
        // get all class teachers
        DataTable oDtTeachers = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);
        ControlUtility.FillDropDownList(oDtTeachers, ref cmbTeachers,
                                            Constants.S_STANDARD_DIVISION_ID_FIELD,
                                             Constants.S_TEACHER_NAME_FIELD,
                                             Constants.S_SELECT);
        
        if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.ExamResults).ToString().ToLower() == "false" &&
            CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.TermwiseHeightWeight).ToString().ToLower() == "false")
        {
            string sMaxReamrkLength = string.Empty;
            int iStdDivId = 0;
            int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            DataRow[] dr = oDtTeachers.Select("Teacher_Id=" + iTeacherId);
            if (dr.Length > 0)
                iStdDivId = Convert.ToInt32(dr[0].ItemArray[8]);
            cmbTeachers.SelectedValue = iStdDivId.ToString();
            cmbTeachers.Enabled = false;
        }   
    }
    
    /// <summary>
    /// This method is used to set values to an object.  
    /// </summary>
    /// <returns></returns>
    private List<StudentInfoForHeightWeight> Populate()
    {
        List<StudentInfoForHeightWeight> lstStudentHeightWeightDetails = new List<StudentInfoForHeightWeight>();
        StudentInfoForHeightWeight oStudentInfoForHeightWeight = null;
        foreach (ListViewDataItem oCurrentItem in lstvwStudentDetails.Items)
        {
            int iRowId = oCurrentItem.DisplayIndex;
          
            TextBox txtHeight = oCurrentItem.FindControl("txtHeight") as TextBox;
            TextBox txtWeight = oCurrentItem.FindControl("txtWeight") as TextBox;

            if (string.IsNullOrEmpty(txtHeight.Text))
               txtHeight.Text = Constants.S_ZERO;
            
            if (string.IsNullOrEmpty(txtWeight.Text))
               txtWeight.Text = Constants.S_ZERO;
            
           
            oStudentInfoForHeightWeight = new StudentInfoForHeightWeight()
            {
                YearWiseStudentId = Convert.ToInt32(lstvwStudentDetails.DataKeys[iRowId]["YearWiseStudentId"]),
                Height = Convert.ToDecimal(txtHeight.Text),
                Weight = Convert.ToDecimal(txtWeight.Text)

            };
            lstStudentHeightWeightDetails.Add(oStudentInfoForHeightWeight);
        }
        return lstStudentHeightWeightDetails;
    }
    
    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillStudentDetailsListview()
    {
        if (cmbTeachers.SelectedValue.ToString() != Constants.S_ZERO)
        {
            lstvwStudentDetails.Visible = true;
            btnSave.Visible = true;
            TermwiseStudentHeightWeightMasterBL oTermwiseStudentHeightWeightMasterBL = new TermwiseStudentHeightWeightMasterBL();
            List<StudentInfoForHeightWeight> lstStudentDetails = oTermwiseStudentHeightWeightMasterBL.GetStudentDetailsForHeightWeight(miSchoolId, miAcademicYearId, cmbTeachers.SelectedValue.ToInt(), cmbTermName.SelectedValue.ToInt());
            lstvwStudentDetails.DataSource = lstStudentDetails;
            lstvwStudentDetails.DataBind();
        }
        else
        {
            lstvwStudentDetails.Visible = false;
            btnSave.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to generate xml.0
    /// </summary>
    /// <param name="lstStudentDetails"></param>
    /// <returns></returns>
    private string GenrateHeightWeightXml(List<StudentInfoForHeightWeight> lstStudentHeightWeightDetails)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstStudentHeightWeightDetails.GetType()).Serialize(sw, lstStudentHeightWeightDetails);
        string sXml = sw.ToString();
        return sXml;
    }

    /// <summary>
    /// This method is used to update student details with current height weight.
    /// </summary>
    private void UpdateHeightWeight()
    {
        int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        string sXml = this.GenrateHeightWeightXml(this.Populate());
        moTermwiseStudentHeightWeightMasterBL = new TermwiseStudentHeightWeightMasterBL(miSchoolId, miAcademicYearId);
        moTermwiseStudentHeightWeightMasterBL.UpdateStudentDetailsForHeightWeight(sXml, miSchoolId, miAcademicYearId, iUserId, cmbTeachers.SelectedValue.ToInt(), cmbTermName.SelectedValue.ToInt());
    }

    /// <summary>
    /// this method is used to set postback url.
    /// </summary>
    private void SetPostbackUrl()
    {
        if (QueryString["IsPrimary"] != null && QueryString["IsPrimary"].ToString() != string.Empty)
            btnBack.PostBackUrl = "~/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx";
        else
            btnBack.PostBackUrl = "~/RITeSchool/Common/ControlPanel.aspx";
    }

    #endregion
}