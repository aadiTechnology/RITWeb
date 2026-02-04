using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using LessonPlanEntities;
using Utility;
using System.Data.SqlClient;

public partial class LessonPlanStdSubjectUI : SchoolBase
{
    #region Constant(s)

    private string S_SAVE_MESSAGE = "Standardwise subjects are saved successfully !!!";

    #endregion

    #region Data Member(s)

    private LessonPlanStdSubjectBL moLessonPlanStdSubjectBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill up standard combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moLessonPlanStdSubjectBL = new LessonPlanStdSubjectBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStandardCombo();
                SetJavaScriptAttrbutes();
                FillSubjects();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill subject list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubjects();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set field value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSubjects_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LessonPlanStdSubject oLessonPlanStdSubject = e.Item.DataItem as LessonPlanStdSubject;
                CheckBox chkSelect = e.Item.FindControl("ChkSelect") as CheckBox;

                chkSelect.Checked = false;
                if (oLessonPlanStdSubject.Id != 0)
                    chkSelect.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save subject and standard details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            FillSubjects();

            if (QueryString["Is_Configured"].ToString() != Constants.S_YES)
                base.SaveConfigDetails(Constants.SchoolConfigurations.LessonPlanStdSubjects.ToInt());
        }
        catch(SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to save subject and standard details.
    /// </summary>
    private void Save()
    {
        StringBuilder oStringBuilder = new StringBuilder();
        foreach (ListViewDataItem oItem in lstvwSubjects.Items)
        {
            int iSubjectId = lstvwSubjects.DataKeys[oItem.DisplayIndex]["SubjectId"].ToInt();
            CheckBox chkSelect = oItem.FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked)
                oStringBuilder.Append("," + iSubjectId);
        }

        string sSubjectIds = string.Empty;
        if (oStringBuilder.Length > 0)
            sSubjectIds = oStringBuilder.ToString().Substring(1);

        if (sSubjectIds != string.Empty)
            moLessonPlanStdSubjectBL.Save(cmbStandard.SelectedValue.ToInt(), sSubjectIds);
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttrbutes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.LessonPlanRelated));
        cmbStandard.Focus();
    }

    /// <summary>
    /// This method is used to fill up subject list view.
    /// </summary>
    private void FillSubjects()
    {
        List<LessonPlanStdSubject> lstSubjects = moLessonPlanStdSubjectBL.GetAllSubjects(cmbStandard.SelectedValue.ToInt());
        lstvwSubjects.DataSource = lstSubjects;
        lstvwSubjects.DataBind();

        btnSave.Enabled = lstSubjects.Count > 0;
    }

    /// <summary>
    /// This method is used to fill up standard combobox.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandards = oStandardCollectionBL.GetAllStandards();
        DataTable oDt = oDtStandards.Select("School_Id=" + miSchoolId).CopyToDataTable();
        ListSource.FillDropDownList(oDt, cmbStandard, "Standard_Name", "Standard_Id", Constants.S_SELECT);
    } 

    #endregion
}