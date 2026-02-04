/* File Name - TransferStudentAcrossBranchUI.aspx.cs
 * Description - Ths class is used to transfer student across branch.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
public partial class TransferStudentAcrossBranchUI : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to fill standard and branch drpodownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillStandards();
                FillSchoolBranchDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to transfer student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            string Ids = GetSelectedIds();
            oStudentBL.TransferStudents(Ids, miSchoolId, miAcademicYearId, ddlBranch.SelectedValue.ToInt(), miUserId);
            lblMessage.Text = "Transfer process is initiated successfully!!!";

            ddlBranch.ClearSelection();
            FillStudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions(ddldivision, Convert.ToInt32(ddlStandard.SelectedValue));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumTarget.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnTransfer.Attributes.Add("onclick", "ResetMessage();");
        btnSearch.Attributes.Add("onclick", "ResetMessage();");
    }

    /// <summary>
    /// This method is used to get ids of selected records.
    /// </summary>
    /// <returns></returns>
    private string GetSelectedIds()
    {
        StringBuilder oStudentIds = new StringBuilder();
        foreach (ListViewDataItem Item in lstvwStudentTransfer.Items)
        {
            CheckBox chkstudent = Item.FindControl("chkSelect") as CheckBox;
            int iStudentId = lstvwStudentTransfer.DataKeys[Item.DisplayIndex]["Student_Id"].ToInt();
            if (chkstudent.Checked)
                oStudentIds.Append("," + iStudentId);

        }
        string sIds = string.Empty;
        if (oStudentIds.ToString().Length > 0)
            sIds = oStudentIds.ToString().Substring(1);
        return sIds;
    }

    /// <summary>
    /// This method is used to fill branch dropdownlist.
    /// </summary>
    private void FillSchoolBranchDetails()
    {
        StudentBL oStudentBL = new StudentBL();
        List<SchoolBranchDetails> lstSchoolBranchDetails = oStudentBL.GetSchoolBranchDetails(miSchoolId);
        ListSource.FillDropDownList(lstSchoolBranchDetails, ddlBranch, "SchoolName", "SchoolId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill student list.
    /// </summary>
    private void FillStudentList()
    {
        int iCurrentStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        int iCurrentDivId = Convert.ToInt32(ddldivision.SelectedValue);
        StudentBL oStudentBL = new StudentBL();
        DataTable oDTCurrentStudents = oStudentBL.GetStudentDetails(miSchoolId, miAcademicYearId, iCurrentStandardId, iCurrentDivId, txtSearch.Text.Trim(), false);
        lstvwStudentTransfer.DataSource = oDTCurrentStudents;
        lstvwStudentTransfer.DataBind();
    }

    /// <summary>
    /// method is used to fill standard dropdownlist.
    /// </summary>
    private void FillStandards()
    {
        YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);

        FillDefaultDivisionValue();
    }

    /// <summary>
    /// This method is used to sety default value to division.
    /// </summary>
    private void FillDefaultDivisionValue()
    {
        ListItem olstDivision = new ListItem();
        olstDivision.Text = "-- Select --";
        olstDivision.Value = "0";
        ddldivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill divisions.
    /// </summary>
    /// <param name="ddlList"></param>
    /// <param name="aiStandardId"></param>
    private void FillDivisions(DropDownList ddlList, int aiStandardId)
    {
        DivisionCollectionBL oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtClass = oDiv.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDtClass, ref ddlList,
                                       "division_Id",
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT);
    } 

    #endregion
}