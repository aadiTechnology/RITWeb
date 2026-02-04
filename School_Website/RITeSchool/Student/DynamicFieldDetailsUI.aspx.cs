/* Class - DynamicFieldDetailsUI.aspx.cs
 * Author - Yogesh Karne
 * Date - 10 June 2016.
 * Description - This class is used to export student details of selected columns for specific user login.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class DynamicFieldDetailsUI : ExportDataTable
{

    #region Member(s)

    private DynamicReportBL moDynamicReportBL;
    
    #endregion

    #region Event(s)

    /// <summary>
    /// This event will fired while page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moDynamicReportBL = new DynamicReportBL(miSchoolId,miAcademicYearId,miUserId);
            if (!IsPostBack)
            {
                FillStandardComboBox();
                SetDefaultValueForDivision();
                FillStudentDetailsListView();
                if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                {
                    tdAddtionalDetails.Visible = true;
                    tdAddtionalDetails.Width = "50%";
                    tdStudentDetails.Width = "50%";
                    FillAdditionalDetailsListView();
                }
                else
                {
                    tdAddtionalDetails.Width = "1%";
                    tdStudentDetails.Width = "170%";
                }
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    

    /// <summary>
    /// This event will fired while user changes selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
                FillDivisionCombobox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fired when user clicks on Save button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moDynamicReportBL = new DynamicReportBL(miSchoolId,miAcademicYearId,miUserId);
            moDynamicReportBL.Save(GenerateXml(Populate()), cmbStandards.SelectedValue.ToInt());
            lblUpdateSucess.Text = Constants.S_DATA_SAVED_SUCCESSFULLY;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fired when user clicks on export button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet oDs = moDynamicReportBL.GetStudentDataForExport(cmbStandards.SelectedValue.ToInt(), cmbDivisions.SelectedValue.ToInt(),chkIncludeWithLeft.Checked);
            DataTable tblTempDataTable = new DataTable(); ;

            //Adding header columns.
            for (int iIndex = 0; iIndex < oDs.Tables[1].Rows.Count; iIndex++)
                tblTempDataTable.Columns.Add("<b>" + oDs.Tables[1].Rows[iIndex][0].ToString() + "</b>");

            //Insert records in data tables of dataset.
            foreach (DataRow dr in oDs.Tables[0].Rows)
                tblTempDataTable.Rows.Add(dr.ItemArray);

            string sJoiningDateOldColumn = "<b>Joining Date</b>";
            string sAdmissionDateOldColumn = "<b>Admission Date</b>";
            string sDateOfBirthOldColumn = "<b>Date Of Birth</b>";

            //Method will convert all date category columns into our standard formats of date.
            if (tblTempDataTable.Columns.Contains(sJoiningDateOldColumn) || tblTempDataTable.Columns.Contains(sAdmissionDateOldColumn) || tblTempDataTable.Columns.Contains(sDateOfBirthOldColumn) || chkIncludeWithLeft.Checked || chkIncludeWithLeft.Checked == false )
                ConvertDateFormats(tblTempDataTable);

            ConvertOtherColumns(tblTempDataTable);

            ExportToExcel("StudentDetails.xls", tblTempDataTable);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set default value for Division Combo box.
    /// </summary>
    private void SetDefaultValueForDivision()
    {
        cmbDivisions.Items.Clear();
        cmbDivisions.Items.Add(new ListItem(Constants.S_SELECT_ALL,Constants.S_ZERO));
    }

    /// <summary>
    /// This method is used to fill student additional details listview.
    /// </summary>
    private void FillAdditionalDetailsListView()
    {
        List<DynamicFieldDetails> lstDynamicReportFieldMasterDetails = moDynamicReportBL.GetDynamicReportFieldMasterDetails(miUserId, true);
        lstViewAdditionalFields.DataSource = lstDynamicReportFieldMasterDetails;
        lstViewAdditionalFields.DataBind();
    }

    /// <summary>
    /// This method is used to Fill Student Details.
    /// </summary>
    private void FillStudentDetailsListView()
    {
        List<DynamicFieldDetails> lstDynamicReportFieldMasterDetails = moDynamicReportBL.GetDynamicReportFieldMasterDetails(miUserId, false);
        lstvwStudentDetails.DataSource = lstDynamicReportFieldMasterDetails;
        lstvwStudentDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill standard combo box.
    /// </summary>
    private void FillStandardComboBox()
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        cmbStandards.Bind(oDtStandardCollection, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT_ALL);
    }


    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    private void FillDivisionCombobox()
    {
        var oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dtDivision = new DataTable();
        if (cmbStandards.SelectedIndex == 0)
        {
            SetDefaultValueForDivision();
        }
        else
        {
            dtDivision = oDiv.GetAllDivisionsForStandard(cmbStandards.SelectedValue.ToInt());

            if(miSchoolId == Constants.SchoolId.PPS.ToInt() && cmbStandards.SelectedItem.Text == "10")
            {
                DataRow[] dr = dtDivision.Select("Division_Name='G'");

                if (dr.Length > 0)
                {
                    dr[0].Delete();
                    dtDivision.AcceptChanges();
                }
            }

            ListSource.FillDropDownList(dtDivision, cmbDivisions, "division_name", "division_id", Constants.S_SELECT_ALL);
           
        }
    }

    /// <summary>
    /// This method is used to populate student details into an object.
    /// </summary>
    /// <returns></returns>
    private List<int> Populate()
    {
        List<int> lstDynamicReportFieldDetails = new List<int>();

        //Populate Student Details object.
        for (int iRowCount = 0; iRowCount < lstvwStudentDetails.Items.Count; iRowCount++)
        {
            CheckBox oChkSelect = lstvwStudentDetails.Items[iRowCount].FindControl("chkSelect") as CheckBox;
            if (oChkSelect.Checked == true)
            {
                int iReportFieldMasterId;
                HiddenField oHidDynamicReportFieldMasterId = lstvwStudentDetails.Items[iRowCount].FindControl("hidDnyamicReportFieldMasterIdForStudentInfo") as HiddenField;
                iReportFieldMasterId = oHidDynamicReportFieldMasterId.Value.ToInt();
                lstDynamicReportFieldDetails.Add(iReportFieldMasterId);
            }
        }

        //Populate Additional Details into an object.
        for (int iRowCount = 0; iRowCount < lstViewAdditionalFields.Items.Count; iRowCount++)
        {
            CheckBox oChkSelectForAdditionalDeatils = lstViewAdditionalFields.Items[iRowCount].FindControl("chkSelect") as CheckBox;
            if (oChkSelectForAdditionalDeatils.Checked == true)
            {
                int iReportFieldMasterId;
                HiddenField oHidDynamicReportFieldMasterIdAdditional = lstViewAdditionalFields.Items[iRowCount].FindControl("hidDnyamicReportFieldMasterIdForStudentAddiInfo") as HiddenField;
                iReportFieldMasterId = oHidDynamicReportFieldMasterIdAdditional.Value.ToInt();
                lstDynamicReportFieldDetails.Add(iReportFieldMasterId);
            }
        }
        return lstDynamicReportFieldDetails;
    }

    /// <summary>
    /// This method is used to convert dates into our standard formats.
    /// </summary>
    /// <param name="newTable"></param>
    private void ConvertDateFormats(DataTable oDTStudentDetails)
    {
        string sJoiningDateOldColumn = "<b>Joining Date</b>";
        string sAdmissionDateOldColumn = "<b>Admission Date</b>";
        string sDateOfBirthOldColumn = "<b>Date Of Birth</b>";
        string sLeftDate = "<b>Left Date</b>";
        const string S_STUDENT_UDISE_NUMBER = "<b>Student UDISE NUmber</b>";
        const string S_SCHOOL_UDISE_NUMBER = "<b>School UDISE NUmber</b>";

        string sAdmissionDateNewColumn = "Admission Date";
        string sJoiningDateNewColumn = "Joining Date";
        string sDateOfBirthNewColumn = "Date Of Birth";

        if (oDTStudentDetails.Columns.Contains(sJoiningDateOldColumn))
            oDTStudentDetails.Columns.Add(sJoiningDateNewColumn, typeof(string));
        if (oDTStudentDetails.Columns.Contains(sAdmissionDateOldColumn))
            oDTStudentDetails.Columns.Add(sAdmissionDateNewColumn, typeof(string));
        if (oDTStudentDetails.Columns.Contains(sDateOfBirthOldColumn))
            oDTStudentDetails.Columns.Add(sDateOfBirthNewColumn, typeof(string));

            int currentRowIndex = 0;
            foreach (DataRow row in oDTStudentDetails.Rows)
            {
                if (oDTStudentDetails.Columns.Contains(sAdmissionDateOldColumn))
                {
                    if (oDTStudentDetails.Columns.Contains(sAdmissionDateNewColumn))
                        row[sAdmissionDateNewColumn] = oDTStudentDetails.Rows[currentRowIndex][sAdmissionDateOldColumn].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                }
                if (oDTStudentDetails.Columns.Contains(sJoiningDateOldColumn))
                {
                    if (oDTStudentDetails.Columns.Contains(sJoiningDateNewColumn))
                        row[sJoiningDateNewColumn] = oDTStudentDetails.Rows[currentRowIndex][sJoiningDateOldColumn].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                }

                if (oDTStudentDetails.Columns.Contains(sDateOfBirthOldColumn))
                {
                    if (oDTStudentDetails.Columns.Contains(sDateOfBirthNewColumn))
                        row[sDateOfBirthNewColumn] = oDTStudentDetails.Rows[currentRowIndex][sDateOfBirthOldColumn].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                }

                if (oDTStudentDetails.Columns.Contains(S_STUDENT_UDISE_NUMBER))
                    row[S_STUDENT_UDISE_NUMBER] = oDTStudentDetails.Rows[currentRowIndex][S_STUDENT_UDISE_NUMBER].ToString() + "&nbsp;";

                if (oDTStudentDetails.Columns.Contains(S_SCHOOL_UDISE_NUMBER))
                    row[S_SCHOOL_UDISE_NUMBER] = oDTStudentDetails.Rows[currentRowIndex][S_SCHOOL_UDISE_NUMBER].ToString() + "&nbsp;";

                currentRowIndex++;
            }
            if (oDTStudentDetails.Columns.Contains(sAdmissionDateOldColumn))
                oDTStudentDetails.Columns.Remove(sAdmissionDateOldColumn);
            if (oDTStudentDetails.Columns.Contains(sJoiningDateOldColumn))
                oDTStudentDetails.Columns.Remove(sJoiningDateOldColumn);
            if (oDTStudentDetails.Columns.Contains(sDateOfBirthOldColumn))
                oDTStudentDetails.Columns.Remove(sDateOfBirthOldColumn);
            if (oDTStudentDetails.Columns.Contains(sAdmissionDateNewColumn))
            {
                DataColumn colAdmissionDate = oDTStudentDetails.Columns.Add(sAdmissionDateOldColumn, typeof(string));
                colAdmissionDate.SetOrdinal(oDTStudentDetails.Columns.Count > 3 ? 3 : oDTStudentDetails.Columns.Count - 1);
            }
            if (oDTStudentDetails.Columns.Contains(sJoiningDateNewColumn))
            {
                DataColumn colJoiningDate = oDTStudentDetails.Columns.Add(sJoiningDateOldColumn, typeof(string));
                colJoiningDate.SetOrdinal(oDTStudentDetails.Columns.Count > 4 ? 4 : oDTStudentDetails.Columns.Count - 1);
            }
            if (oDTStudentDetails.Columns.Contains(sDateOfBirthNewColumn))
            {
                DataColumn colDateOfBirth = oDTStudentDetails.Columns.Add(sDateOfBirthOldColumn, typeof(string));
                colDateOfBirth.SetOrdinal(oDTStudentDetails.Columns.Count > 16 ? 16 : oDTStudentDetails.Columns.Count - 1);
            }
            currentRowIndex = 0;
            foreach (DataRow row in oDTStudentDetails.Rows)
            {
                if (oDTStudentDetails.Columns.Contains(sAdmissionDateOldColumn))
                    row[sAdmissionDateOldColumn] =
                        oDTStudentDetails.Rows[currentRowIndex][sAdmissionDateNewColumn].ToDateTime().ToString(
                            Constants.S_DATE_FORMAT);
                if (oDTStudentDetails.Columns.Contains(sJoiningDateOldColumn))
                    row[sJoiningDateOldColumn] =
                        oDTStudentDetails.Rows[currentRowIndex][sJoiningDateNewColumn].ToDateTime().ToString(
                            Constants.S_DATE_FORMAT);
                if (oDTStudentDetails.Columns.Contains(sDateOfBirthOldColumn))
                    row[sDateOfBirthOldColumn] =
                        oDTStudentDetails.Rows[currentRowIndex][sDateOfBirthNewColumn].ToDateTime().ToString(
                            Constants.S_DATE_FORMAT);
                if ((!oDTStudentDetails.Rows[currentRowIndex][sLeftDate].ToString().IsNullOrEmpty()))
                {
                    if (oDTStudentDetails.Columns.Contains(sLeftDate) && chkIncludeWithLeft.Checked == true)
                        row[sLeftDate] = oDTStudentDetails.Rows[currentRowIndex][sLeftDate].ToDateTime().ToString(
                            Constants.S_DATE_FORMAT);
                }
        currentRowIndex++;

                
            }

            if (oDTStudentDetails.Columns.Contains(sAdmissionDateNewColumn))
                oDTStudentDetails.Columns.Remove(sAdmissionDateNewColumn);
            if (oDTStudentDetails.Columns.Contains(sJoiningDateNewColumn))
                oDTStudentDetails.Columns.Remove(sJoiningDateNewColumn);
            if (oDTStudentDetails.Columns.Contains(sDateOfBirthNewColumn))
                oDTStudentDetails.Columns.Remove(sDateOfBirthNewColumn);

            if (oDTStudentDetails.Columns.Contains(sLeftDate) && chkIncludeWithLeft.Checked == false)
                oDTStudentDetails.Columns.Remove(sLeftDate);
    }

    /// <summary>
    /// This method is used to convert other columns.
    /// </summary>
    /// <param name="newTable"></param>
    private static void ConvertOtherColumns(DataTable newTable)
    {
        int currentRowIndex = 0;
        foreach (DataRow row in newTable.Rows)
        {
            if (newTable.Columns.Contains("<b>Gender</b>"))
                row["<b>Gender</b>"] = newTable.Rows[currentRowIndex]["<b>Gender</b>"].ToString() == "M" ? "Male" : "Female";
            if (newTable.Columns.Contains("<b>Is RTE Applicable</b>"))
                row["<b>Is RTE Applicable</b>"] = newTable.Rows[currentRowIndex]["<b>Is RTE Applicable</b>"].ToBool() == true ? "Yes" : "No";
            if (newTable.Columns.Contains("<b>New Admission</b>"))
                row["<b>New Admission</b>"] = newTable.Rows[currentRowIndex]["<b>New Admission</b>"].ToBool() == true ? "Yes" : "No";
            
            if (newTable.Columns.Contains("<b>Saral Number</b>"))
            {
                string original = newTable.Rows[currentRowIndex]["<b>Saral Number</b>"].ToString();
                if (!string.IsNullOrWhiteSpace(original) && !original.StartsWith("'"))
                {
                    row["<b>Saral Number</b>"] = "'" + original;
                }
            }

            if (newTable.Columns.Contains("<b>Apaar Id</b>"))
            {
                string original = newTable.Rows[currentRowIndex]["<b>Apaar Id</b>"].ToString();
                if (!string.IsNullOrWhiteSpace(original) && !original.StartsWith("'"))
                {
                    row["<b>Apaar Id</b>"] = "'" + original;
                }
            }
            if (newTable.Columns.Contains("<b>Adhar Card Number</b>"))
            {
                string original = newTable.Rows[currentRowIndex]["<b>Adhar Card Number</b>"].ToString();
                if (!string.IsNullOrWhiteSpace(original) && !original.StartsWith("'"))
                {
                    row["<b>Adhar Card Number</b>"] = "'" + original;
                }
            }
            if (newTable.Columns.Contains("<b>PEN Number</b>"))
            {
                string original = newTable.Rows[currentRowIndex]["<b>PEN Number</b>"].ToString();
                if (!string.IsNullOrWhiteSpace(original) && !original.StartsWith("'"))
                {
                    row["<b>PEN Number</b>"] = "'" + original;
                }
            }

            currentRowIndex++;
        }
    }
    #endregion
    
}