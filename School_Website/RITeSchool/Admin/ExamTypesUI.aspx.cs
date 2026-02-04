//// File Name  : ExamTypesUI.aspx.cs
//// Created By : Yogesh
//// Date       : 21/05/2015
//// Description :This class is used to attach PAN copy. 
////   

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;

public partial class ExamTypesUI : SchoolBase
{
    #region Data Member(s)
    ExamTypesConfigurationBL oExamTypesConfigurationBL;
    #endregion

    #region Event(s)
    /// This event is used to load page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                oExamTypesConfigurationBL = new ExamTypesConfigurationBL(miSchoolId, miAcademicYearId);
                btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
                List<YearWiseSubjectsDetails> lstSubjects = oExamTypesConfigurationBL.GetAllYearwiseSubjects(0);
                ListSource.FillDropDownList(lstSubjects, ddlSubjects, "SubjectName", "SubjectId", Constants.S_SELECT);
                if (ddlSubjects.SelectedIndex.ToInt() != 0)
                    btnSave.Visible = true;
                else
                    btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to capture selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSubjects.SelectedIndex.ToInt() != 0)
            {
                lstvwExamTypes.Visible = true;
                btnSave.Visible = true;
                FillExamTypesListview();
            }
            else
            {
                lstvwExamTypes.Visible = false;
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to capture check box status from database record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExamTypes_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;

                int iFlag = lstvwExamTypes.DataKeys[oCurrentItem.DisplayIndex]["Flag"].ToInt();
                var chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                if (iFlag == 1)
                {

                    chkSelect.Checked = true;
                }
                else
                    chkSelect.Checked = false;
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Exam Type configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            GetCommaSaperatedIds();
            lblUpateMessage.Visible = true;
            lblUpateMessage.Text = "Exam Types configuration saved successfully!!!";
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ExamTypes));
            FillExamTypesListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Method(s)
    /// <summary>
    /// This method is used fill listview.
    /// </summary>
    private void FillExamTypesListview()
    {
        oExamTypesConfigurationBL = new ExamTypesConfigurationBL(miSchoolId, miAcademicYearId);
        List<SubjectwiseExamTypeDetails> lstSubjecwiseExamTypeDetails = oExamTypesConfigurationBL.GetAll(ddlSubjects.SelectedValue.ToInt());
        lstvwExamTypes.DataSource = lstSubjecwiseExamTypeDetails;
        lstvwExamTypes.DataBind();

        if (lstSubjecwiseExamTypeDetails.Count == 0)
            btnSave.Enabled = false;
    }

    /// <summary>
    /// This method is used get comma separated ids for insert.
    /// </summary>
    private void GetCommaSaperatedIds()
    {
        oExamTypesConfigurationBL = new ExamTypesConfigurationBL(miSchoolId, miAcademicYearId);
        string sCommaSeperatedIdsInsert = "";
        string sCommaSeperatedIdsDelete = "";
        foreach (ListViewDataItem oCurrentItem in lstvwExamTypes.Items)
        {
            int iRowId = oCurrentItem.DisplayIndex.ToInt();
            CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked == true && lstvwExamTypes.DataKeys[oCurrentItem.DataItemIndex]["Flag"].ToInt() == 0)
            {
                sCommaSeperatedIdsInsert = sCommaSeperatedIdsInsert + lstvwExamTypes.DataKeys[iRowId]["TestTypeId"].ToString() + ",";

            }
            if (chkSelect.Checked == false && lstvwExamTypes.DataKeys[oCurrentItem.DataItemIndex]["Flag"].ToInt() == 1)
            {
                sCommaSeperatedIdsDelete = sCommaSeperatedIdsDelete + lstvwExamTypes.DataKeys[iRowId]["TestTypeId"].ToString() + ",";

            }

        }

        oExamTypesConfigurationBL.Save(ddlSubjects.SelectedValue.ToInt(), sCommaSeperatedIdsInsert, sCommaSeperatedIdsDelete);
    }
#endregion

}