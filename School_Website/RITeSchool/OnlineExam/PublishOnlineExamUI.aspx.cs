using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Linq;

public partial class PublishOnlineExamUI : SchoolBase
{
    #region Data Member(s)

    private PublishOnlineExamBL moPublishOnlineExamBL; 

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPublishOnlineExamBL = new PublishOnlineExamBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillClass();
                FillTestCombobox();
                FillListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {   
            FillListview();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }

    }
    protected void cmbExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillListview();
    }
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        moPublishOnlineExamBL.Publish(cmbClass.SelectedValue.ToInt(), cmbExam.SelectedValue.ToInt(), true);
        lblMessage.Text = "Exam is published successfully!!!";
        FillListview();
    }
    protected void btnUnPublish_Click(object sender, EventArgs e)
    {
        moPublishOnlineExamBL.Publish(cmbClass.SelectedValue.ToInt(), cmbExam.SelectedValue.ToInt(), false);
        lblMessage.Text = "Exam is unpublished successfully!!!";
        FillListview();
    }

    protected void lstvwStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oLstVwItem = (ListViewDataItem)e.Item;                
                int iRowId = Convert.ToInt32(oLstVwItem.DisplayIndex);    
            
                ImageButton oLinkButton = e.Item.FindControl("lnkDetails") as ImageButton;
                OnlineExamStatus oOnlineExamStatus = e.Item.DataItem as OnlineExamStatus;

                int iAnswerTypeId = oOnlineExamStatus.AnswerTypeId;
                string sIsPublished = "N";
                if (oOnlineExamStatus.IsPublished)
                    sIsPublished = "Y";

                string sQueryString = "ExamId=" + cmbExam.SelectedValue + "&StdDivId=" + cmbClass.SelectedValue + "&SubjectId=" + oOnlineExamStatus.SubjectId + "&IsPublished=" + sIsPublished;
                if (iAnswerTypeId != 3)
                    oLinkButton.Attributes.Add("onclick", "window.open('OnlineExamResultUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "','_self'); return false;");
                else
                    oLinkButton.Attributes.Add("onclick", "window.open('OnlineExamStudentListUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "','_self'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
    
    #region Method(s)

    private void FillClass()
    {
        DataTable oDt = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);

        if (moUserRole == Constants.UserRoles.Teacher && hidUserHasFullAccess.Value != Constants.S_YES)
        {
            DataRow[] oDataRow = oDt.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID]);
            ControlUtility.FillDropDownList(
                       oDataRow,
                       ref cmbClass,
                       Constants.S_STANDARD_DIVISION_ID_FIELD,
                       Constants.S_TEACHER_NAME_FIELD,
                       Constants.S_SELECT);
            if (oDataRow.Length == 1)
            {
                cmbClass.SelectedIndex = 1;
                cmbClass.Enabled = false;
            }
        }
        else
        {
            ControlUtility.FillDropDownList(
                           oDt,
                           ref cmbClass,
                           Constants.S_STANDARD_DIVISION_ID_FIELD,
                           Constants.S_TEACHER_NAME_FIELD,
                           Constants.S_SELECT);
        }

        if (!string.IsNullOrEmpty(QueryString["StdDivId"]))
        {
            cmbClass.SelectedValue = QueryString["StdDivId"].ToString();
            cmbClass_SelectedIndexChanged(cmbClass, null);
        }
    }

    private void FillListview()
    {
        List<OnlineExamStatus> lstStudentInfo = moPublishOnlineExamBL.GetExamResult(cmbClass.SelectedValue.ToInt(), cmbExam.SelectedValue.ToInt());
        lstvwStudent.DataSource = lstStudentInfo;
        lstvwStudent.DataBind();

        if (lstStudentInfo.Count > 0)
        {
            btnPublish.Visible = true;
            btnUnPublish.Visible = true;

            //if (moPublishOnlineExamBL.IsPublished)
            //    btnPublish.Text = "UnPublish";

            btnUnPublish.Enabled = lstStudentInfo.Any(si => si.IsPublished);

            if (moPublishOnlineExamBL.AllowPublish && lstStudentInfo.Any(si => !si.IsPublished))
                btnPublish.Enabled = true;
            else
                btnPublish.Enabled = false;
        }
        else
        {
            btnPublish.Visible = false;
            btnUnPublish.Visible = false;
        }
    }

    private void SetDefaultValues()
    {
        hidUserHasFullAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.OnlineExamResult).ToString();
    }

    /// <summary>
    /// This method  is used to fill test combobox.
    /// </summary>
    private void FillTestCombobox()
    {
        OnlineExamConfigurationBL oOnlineExamConfigurationBL = new OnlineExamConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        using (DataTable oDsAllTests = oOnlineExamConfigurationBL.GetAllTestsForClass())
        {
            ControlUtility.FillDropDownList(
                oDsAllTests,
                ref cmbExam,
                "Id",
                "Name",
                Constants.S_SELECT);  //
        }

        if (!string.IsNullOrEmpty(QueryString["ExamId"]))
        {
            cmbExam.SelectedValue = QueryString["ExamId"].ToString();
            cmbExam_SelectedIndexChanged(cmbExam, null);
        }
    }

    #endregion   
}