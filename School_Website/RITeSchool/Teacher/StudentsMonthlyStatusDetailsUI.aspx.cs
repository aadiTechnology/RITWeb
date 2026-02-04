using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolEntities.Teacher;
using Utility;

public partial class StudentsMonthlyStatusDetailsUI : SchoolBase
{
    #region Constants
    
    private const string S_SAVE_MESSAGE = "Remark saved successfully !!!"; 

    #endregion

    #region Data Member(s)
    
    private StudentsMonthlyStatusDetailsBL moStudentsMonthlyStatusDetailsBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill up Standard,division,Month,Category combo boxes and Fill Students ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentsMonthlyStatusDetailsBL = new StudentsMonthlyStatusDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillStandards();
                FillDivisions();
                FillMonthCombo();
                FillNoteCategories();
                FillStudentsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill up Standard combo box.
    /// </summary>    
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Remark details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                List<StudentsMonthlyStatusDetails> oStudentsMonthlyStatusDetails = Populate();
                string sXml = base.GenerateXml(oStudentsMonthlyStatusDetails);
                moStudentsMonthlyStatusDetailsBL.Save(sXml, cmbCategory.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt());
                FillStudentsListView();
                lblmessage.Text = S_SAVE_MESSAGE;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evemt is used to handle remark length.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentMonthlyStatus_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            StudentsMonthlyStatusDetails oStudentsMonthlyStatusDetails = e.Item.DataItem as StudentsMonthlyStatusDetails;
            Label lblLength = e.Item.FindControl("lblLength") as Label;
            lblLength.Text = "(" + (500 - oStudentsMonthlyStatusDetails.Remark.Trim().Length).ToString() + ")";

            TextBox txtRemark = e.Item.FindControl("txtRemark") as TextBox;
            txtRemark.Attributes.Add("onkeyup", "SetRemarkLength(this,'" + lblLength.ClientID + "');");
            txtRemark.Attributes.Add("onpaste", "SetRemarkLength(this,'" + lblLength.ClientID + "');");
        }
    }

    /// <summary>
    /// This event is used to show student list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text.ToUpper() == "SHOW")
            {
                FillStudentsListView();
                DisableFields(true);
                btnShow.Text = "Change Input";
            }
            else
            {
                DisableFields(false);
                lstvwStudentMonthlyStatus.DataSource = null;
                lstvwStudentMonthlyStatus.DataBind();
                btnShow.Text = "Show";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisions()
    {
        try
        {
            int aiStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
            DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
            ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                           Constants.S_DIVISION_ID_FIELD,
                                           Constants.S_DIVISION_NAME_FIELD,
                                           Constants.S_SELECT);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This method is used to fill month combobox.
    /// </summary>
    private void FillMonthCombo()
    {
        List<MonthMaster> oLstMonths = SchoolWiseAcademicYearMasterBL.GetAllMonth();
        ListSource.FillDropDownList(oLstMonths, cmbMonth, "Month", "MonthID", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill note categories.
    /// </summary>
    private void FillNoteCategories()
    {
        StudentAchievementBL moStudentAchievementBL = new StudentAchievementBL();
        DataTable dtNoteCategory = moStudentAchievementBL.GetNoteCategories();
        cmbCategory.Bind(dtNoteCategory, "Id", "NoteCategory", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill Students list view.
    /// </summary>
    private void FillStudentsListView()
    {
        int iStandardId = cmbStandard.SelectedValue.ToInt();
        int iDivisionId = cmbDivision.SelectedValue.ToInt();

        List<StudentsMonthlyStatusDetails> lstStudentDetails = moStudentsMonthlyStatusDetailsBL.GetAllStudentsListforMonthlyStatus(iStandardId, iDivisionId, cmbCategory.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt());
        lstvwStudentMonthlyStatus.DataSource = lstStudentDetails;
        lstvwStudentMonthlyStatus.DataBind();
    }

    /// <summary>
    /// This method is used to disabled fields.
    /// </summary>
    /// <param name="abDisable"></param>
    private void DisableFields(bool abDisable)
    {
        cmbCategory.Enabled = !abDisable;
        cmbDivision.Enabled = !abDisable;
        cmbStandard.Enabled = !abDisable;
        cmbMonth.Enabled = !abDisable;
        btnSave.Visible = abDisable;
    }

    /// <summary>
    /// This method is used to populate details.
    /// </summary>
    /// <returns></returns>
    private List<StudentsMonthlyStatusDetails> Populate()
    {
        List<StudentsMonthlyStatusDetails> lstStudentsMonthlyStatusDetails = new List<StudentsMonthlyStatusDetails>();
        {
            foreach (ListViewDataItem item in lstvwStudentMonthlyStatus.Items)
            {
                TextBox txtRemark = item.FindControl("txtRemark") as TextBox;

                int iId = lstvwStudentMonthlyStatus.DataKeys[item.DisplayIndex]["YearWise_Student_Id"].ToInt();
                
                StudentsMonthlyStatusDetails oStudentsMonthlyStatusDetails = new StudentsMonthlyStatusDetails
                {
                    Remark = txtRemark.Text.Trim(),
                    YearWise_Student_Id = iId
                };

                lstStudentsMonthlyStatusDetails.Add(oStudentsMonthlyStatusDetails);
            }
            return lstStudentsMonthlyStatusDetails;
        }
    }

    /// <summary>
    /// This method is sued to fill standards.
    /// </summary>
    private void FillStandards()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT);
    } 

    #endregion
}