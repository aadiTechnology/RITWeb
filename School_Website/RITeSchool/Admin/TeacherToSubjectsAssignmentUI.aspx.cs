/* File Name :- TeacherToSubjectsAssignmentUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 24-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to assign teacher to subject of selected class.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class TeacherToSubjectsAssignmentUI : SchoolBase
{
    #region Constants

    const string S_QUERYSTRING_TEACHER_ID = "TeacherId";    
    const string S_STANDARD_DIVISION_ID = "Standard_Division_Id";
    const string S_STANDARD_DIVISION_NAME = "StandardDivision";
    const string S_SUBJECT_ID = "Subject_Id";
    const string S_SUBJECT_NMAE = "Subject_Name";
    const string S_TEACHER_SUBJECT_ID = "Teacher_Subject_Id";
    const string S_TEACHER_ID = "Teacher_Id";
    const string S_GRIDVIEW_DATASOURCE = "grdDivisionWiseSubjects_DataSource";

    //row commands 
    const string S_ALREADY_ASSOCIATE_SUBJECT = "Selected Class and Subject association already assigned to :";

    const int I_DATA_KEY_SUB_ID = 1;
    const int I_DELETE_COLUMN_INDEX = 2;

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            ReadQuerystring();
            DisplayTeacherDetails();            
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValues();
                cmbSubjects.Items.Add(new ListItem(Constants.S_SELECT, "0"));
                FillDivisionWiseSubjectGrid();
                FillStandardDivisionCombobox();
                SetJavascriptAttributes();
                SetDefaultProperties();
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add new assignment into gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddDetails_Click(object sender, EventArgs e)
    {
        try
        {
            AddAssignmentToGrid();            
            lblDuplicateDetails.Visible = false;
            cmbSubjects.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save teacher and class-subject assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            InsertTeacherSubjectAssignment();
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AssignedTeacherToSub));
            FillStandardDivisionCombobox();
            FillDivisionWiseSubjectGrid();
            if(String.IsNullOrEmpty(lblErrorMsg.Text))
				ResetControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill fill subject combobox on change of class.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandardDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {       
            FillSubjectCombobox();
            btnAddDetails.Enabled = true;
            lblDuplicateDetails.Visible = false; 
            cmbSubjects.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check whether selected subject is already associated with another teacher or assigned to current teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            const int I_FIRST_ROW = 0;
            if (!CheckIsDuplicateInGrid())
            {   
                btnAddDetails.Enabled = true;             
                int iMaxTeacher = Settings.MaxTeacherForSubject;
                int iTeacherId = Convert.ToInt32(hidTeacherId.Value);
                int iStandardDivisionId = Convert.ToInt32(cmbStandardDivision.SelectedValue);
                int iSubjectId = Convert.ToInt32(cmbSubjects.SelectedValue);
                TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
                DataTable oDTTeacherDetails = oTeacherSubjectAssignmentBL.GetSubjectAssignedTeacherName(iStandardDivisionId, iSubjectId, iTeacherId);
                if (oDTTeacherDetails.Rows.Count > 0)
                {
                    if (oDTTeacherDetails.Rows.Count == 1)
                        hidTeacherName.Value = Convert.ToString(oDTTeacherDetails.Rows[I_FIRST_ROW]["TeacherName"]);
                    else
                        if (oDTTeacherDetails.Rows.Count < iMaxTeacher)
                        {
                            hidTeacherName.Value = string.Empty;
                            for (int iRowCount = 0; iRowCount < oDTTeacherDetails.Rows.Count; iRowCount++)
                                hidTeacherName.Value += oDTTeacherDetails.Rows[iRowCount]["TeacherName"].ToString() + " & ";
                            hidTeacherName.Value = hidTeacherName.Value.Substring(0, hidTeacherName.Value.LastIndexOf(" & "));
                        }
                        else
                        {
                            btnAddDetails.Enabled = false;
                            hidTeacherName.Value = string.Empty;
                            for (int iRowCount = 0; iRowCount < oDTTeacherDetails.Rows.Count; iRowCount++)
                                hidTeacherName.Value += oDTTeacherDetails.Rows[iRowCount]["TeacherName"].ToString() + " & ";
                            hidTeacherName.Value = hidTeacherName.Value.Substring(0, hidTeacherName.Value.LastIndexOf(" & "));
                        }
                }
                else
                    hidTeacherName.Value = string.Empty;
                if (hidTeacherName.Value != string.Empty)
                {
                    lblDuplicateDetails.Visible = true;
                    lblDuplicateDetails.Text = Resources.LocalizedResources.AlreadyAssociatedWith+ "  " + hidTeacherName.Value;
                }
                else
                {                    
                    lblDuplicateDetails.Visible = false;
                    btnAddDetails.Enabled = true;
                }
            }
            else
            {
                lblDuplicateDetails.Visible = true;
                btnAddDetails.Enabled = false;
            }
            btnAddDetails.Focus();
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add attribute on delete button set cssClass to perticular cell.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDivisionWiseSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
                if (grdDivisionWiseSubjects.DataKeys[e.Row.RowIndex][3].ToString() != hidTeacherId.Value.ToString())
                    e.Row.CssClass = "ClsHilightBGB";

                int iTeacherId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[e.Row.RowIndex][3].ToString());
                if (iTeacherId != Convert.ToInt32(hidTeacherId.Value))
                    hidFlag.Value = "true";
                divNote.Visible = true;
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region  Methods

    /// <summary>
    /// This method is used to fill subject combobox.
    /// </summary>
    private void FillSubjectCombobox()
    {
        int iTeacherId = Convert.ToInt32(hidTeacherId.Value);
        int iStandardDivisionId = Convert.ToInt32(cmbStandardDivision.SelectedValue);        
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSubjectsComboBox(miSchoolId, iTeacherId, iStandardDivisionId, miAcademicYearId, ref cmbSubjects);
    }

    /// <summary>
    /// This method is used to display teacher's details.
    /// </summary>
    private void DisplayTeacherDetails()
    {
        int iTeacherId = Convert.ToInt32(hidTeacherId.Value);
        SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL(iTeacherId);
        DisplayTeacherNameAndDesignation(oSchoolWiseTeacherMasterBL);
        DisplaySubjectDetails();
        DisplayStandardDetails();
        if (grdDivisionWiseSubjects.Rows.Count == 0)
        {
            btnSave.Visible = false;
            tdSave.Visible = true;
        }
        else
            btnSave.Visible = true;
    }

    /// <summary>
    /// This method is used to display teacher name and designation.
    /// </summary>
    /// <param name="aoSchoolWiseTeacherMasterBL"></param>
    private void DisplayTeacherNameAndDesignation(SchoolWiseTeacherMasterBL aoSchoolWiseTeacherMasterBL)
    {
        if (aoSchoolWiseTeacherMasterBL.TeacherMiddleName != Constants.S_EMPTY_STRING)
            lblTeacherName.Text = aoSchoolWiseTeacherMasterBL.Salutataion + " "
                                + aoSchoolWiseTeacherMasterBL.TeacherFirstName + " "
                                + aoSchoolWiseTeacherMasterBL.TeacherMiddleName + (aoSchoolWiseTeacherMasterBL.TeacherMiddleName.Length > 1 ? " " : ". ")
                                + aoSchoolWiseTeacherMasterBL.TeacherLastName;
        else
            lblTeacherName.Text = aoSchoolWiseTeacherMasterBL.Salutataion + " "
                                + aoSchoolWiseTeacherMasterBL.TeacherFirstName + " "
                                + aoSchoolWiseTeacherMasterBL.TeacherLastName;
        lblDesignation.Text = aoSchoolWiseTeacherMasterBL.Designation;
    }

    /// <summary>
    /// This method is used to display subject details.
    /// </summary>
    private void DisplaySubjectDetails()
    {
        int iTeacherId = QueryString[S_QUERYSTRING_TEACHER_ID].ToInt();
        TeacherSubjectDetailsBL oTeacherSubjectDetailsBL = new TeacherSubjectDetailsBL();        
        DataTable oDTSubjectDetails = oTeacherSubjectDetailsBL.FetchSubjectDetailsForTeacherId(iTeacherId);
        for (int iRowCount = 0; iRowCount < oDTSubjectDetails.Rows.Count; iRowCount++)
            lblSubjects.Text += Convert.ToString(oDTSubjectDetails.Rows[iRowCount][1]) + ", ";
       if(lblSubjects.Text != string.Empty)
            lblSubjects.Text =lblSubjects.Text.Remove(lblSubjects.Text.Length - 2);
    }

    /// <summary>
    /// This method is used to display standard details.
    /// </summary>
    private void DisplayStandardDetails()
    {
        int iTeacherId = QueryString[S_QUERYSTRING_TEACHER_ID].ToInt();
        TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();     
        DataTable oDTStandardDetails = oTeacherStandardDetailsBL.FetchStandardDetailsForTeacherId(iTeacherId);
        for (int iRowCount = 0; iRowCount < oDTStandardDetails.Rows.Count; iRowCount++)
            lblStandards.Text += Convert.ToString(oDTStandardDetails.Rows[iRowCount][1]) + ", ";
        lblStandards.Text = lblStandards.Text.Remove(lblStandards.Text.Length - 2);
    }

    /// <summary>
    /// This method is used to set default properties.
    /// </summary>
    private void SetDefaultProperties()
    {
        valAddEduDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        cmbStandardDivision.Focus();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnSave.Attributes.Add("onclick", "if(!saveChk()){return false;}");
        btnBack.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnAddDetails, btnBack });
        cmbSubjects.Attributes.Add("OnChange", "ClearValSummary()");
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        hidTeacherId.Value = QueryString[S_QUERYSTRING_TEACHER_ID];            
    }

    /// <summary>
    /// This method is used to fill class combobox.
    /// </summary>
    private void FillStandardDivisionCombobox()
    {
        int iTeacherId = Convert.ToInt32(hidTeacherId.Value);
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillStandardDivisionComboBox(miSchoolId, iTeacherId, miAcademicYearId, ref cmbStandardDivision);
    }

    /// <summary>
    /// This method is used to fill gridview.
    /// </summary>
    private void FillDivisionWiseSubjectGrid()
    {
        int iTeacherId = Convert.ToInt32(hidTeacherId.Value);
        TeacherSubjectAssignmentCollectionBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentCollectionBL(iTeacherId);
        DataTable oDSSubjects = oTeacherSubjectAssignmentBL.GetAllDivisionSubjectsDetailsForTeacher(miSchoolId, miAcademicYearId, iTeacherId);

        oDSSubjects.DefaultView.Sort = S_STANDARD_DIVISION_ID + " " + Constants.S_ASCENDING;
        grdDivisionWiseSubjects.DataSource = oDSSubjects.DefaultView;
        grdDivisionWiseSubjects.DataBind();
        ViewState[S_GRIDVIEW_DATASOURCE] = oDSSubjects;
        if (grdDivisionWiseSubjects.Rows.Count == 0)
        {
            btnSave.Visible = false;
            tdSave.Visible = true;
            lblSaveSucess.Visible = false;
            divNote.Visible = false;
        }
        else
            btnSave.Visible = true;
    }

    /// <summary>
    /// This method is used to check whether selected assignment is already present in grid or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIsDuplicateInGrid()
    {
        lblDuplicateDetails.Text = Resources.LocalizedResources.DuplicateSubjectDetailsMsg;
        const int I_DATA_KEY_STD_DIV = 0;
        string sSelectedStdDiv = cmbStandardDivision.SelectedItem.Value;
        string sSubject = cmbSubjects.SelectedItem.Value;
        for (int iRowIndex = Constants.I_ZERO; iRowIndex < grdDivisionWiseSubjects.Rows.Count; iRowIndex++)
        {
            string sGridstdDiv = Convert.ToString(grdDivisionWiseSubjects.DataKeys[iRowIndex][I_DATA_KEY_STD_DIV]);
            string sGridSubId = Convert.ToString(grdDivisionWiseSubjects.DataKeys[iRowIndex][I_DATA_KEY_SUB_ID]);
            if (sGridstdDiv.Equals(sSelectedStdDiv) && sGridSubId.Equals(sSubject))
                return true;
        }
        return false;
    }

    /// <summary>
    /// This method is used to insert teacher subject assignment.
    /// </summary>
    private void InsertTeacherSubjectAssignment()
    {
        CheckBox chkIsSelected = new CheckBox();

        DataTable oDTEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
        Collection<TeacherSubjectAssignmentBL> oTeacherSubjects = new Collection<TeacherSubjectAssignmentBL>();
        oTeacherSubjects.Clear();
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = null;
        string sErrorMessage = CheckDependencyToDelete();
        if (string.IsNullOrEmpty(sErrorMessage))
        {
            for (int i = 0; i < grdDivisionWiseSubjects.Rows.Count; i++)
            {
                chkIsSelected = (CheckBox)grdDivisionWiseSubjects.Rows[i].FindControl("chkIsSelected");

                oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
                oTeacherSubjectAssignmentBL.TeacherSubjectId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[i][S_TEACHER_SUBJECT_ID]);
                oTeacherSubjectAssignmentBL.TeacherId = Convert.ToInt32(hidTeacherId.Value);
                oTeacherSubjectAssignmentBL.SchoolId = miSchoolId;
                oTeacherSubjectAssignmentBL.StandardDivisionId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[i][S_STANDARD_DIVISION_ID]);
                oTeacherSubjectAssignmentBL.SubjectId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[i][S_SUBJECT_ID]);
                oTeacherSubjectAssignmentBL.InsertedById = miUserId;
                oTeacherSubjectAssignmentBL.UpdatedById = miUserId;

                if (chkIsSelected.Checked == true)
                {
                    if (oTeacherSubjectAssignmentBL.TeacherSubjectId > 0)
                        oTeacherSubjectAssignmentBL.AssignmentAction = Constants.Action.Update;
                    else
                    {
                        oTeacherSubjectAssignmentBL.AssignmentAction = Constants.Action.Insert;
                    }
                    lblSaveSucess.Visible = true;
                    lblSaveSucess.Text = Resources.LocalizedResources.CheckedSubjectSavedSuccessfullyMsg;
                    divNote.Visible = true;

                }
                else
                {
                    if (oTeacherSubjectAssignmentBL.TeacherSubjectId >= 0)
                        oTeacherSubjectAssignmentBL.AssignmentAction = Constants.Action.Delete;
                }

                oTeacherSubjects.Add(oTeacherSubjectAssignmentBL);
            }
            TeacherSubjectAssignmentCollectionBL oTeacherSubjectAssignmentCollectionBL = new TeacherSubjectAssignmentCollectionBL(Convert.ToInt32(hidTeacherId.Value));
            oTeacherSubjectAssignmentCollectionBL.UpdatePreviousTeacherSubjects(oTeacherSubjects);
        }
        else
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = sErrorMessage;
            lblSaveSucess.Visible = false;
        }
    }
       
    /// <summary>
    /// This function is used to reset controls on the Page to their default values.
    /// </summary>
    private void ResetControls()
    {
		// Reset Class & Subject DropDown Lists
		cmbStandardDivision.SelectedIndex = 0;
		cmbSubjects.Items.Clear();
		cmbSubjects.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
		
		// Reset Error Message
		lblDuplicateDetails.Text = String.Empty;
		lblErrorMsg.Text = String.Empty;
    }

    #endregion

    # region Create Datatable and Datarow

    /// <summary>
    /// This method is used to create the datarow and bind that row to the gridview
    /// </summary>

    private void AddAssignmentToGrid()
    {
        DataTable oDTEducationDetails;
        if (ViewState[S_GRIDVIEW_DATASOURCE] == null)
            oDTEducationDetails = CreateDivisionwiseSubjectDetailsTable();
        else
            oDTEducationDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];

        // Once a table has been created,create DataRow.    
        oDTEducationDetails.Rows.Add(AddDivisionwiseSubjectDetailsToDataRow(oDTEducationDetails.NewRow()));
        oDTEducationDetails.DefaultView.Sort = S_STANDARD_DIVISION_ID + " " + Constants.S_ASCENDING;
        DataView oDTItemView = oDTEducationDetails.DefaultView;
        grdDivisionWiseSubjects.DataSource = oDTItemView;
        oDTEducationDetails = oDTItemView.ToTable();
        ViewState[S_GRIDVIEW_DATASOURCE] = oDTEducationDetails;
        grdDivisionWiseSubjects.DataBind();

        if (grdDivisionWiseSubjects.Rows.Count == 0)
        {
            btnSave.Visible = false;
            tdSave.Visible = true;
        }
        else
            btnSave.Visible = true;
    }
     
    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow  AddDivisionwiseSubjectDetailsToDataRow(DataRow aoDataRow)
    {   
        // Then add the new row to the collection.
        aoDataRow[S_STANDARD_DIVISION_ID] = Convert.ToString(cmbStandardDivision.SelectedValue);
        aoDataRow[S_STANDARD_DIVISION_NAME] = Convert.ToString(cmbStandardDivision.SelectedItem);
        aoDataRow[S_SUBJECT_ID] = Convert.ToInt32(cmbSubjects.SelectedValue);
        aoDataRow[S_SUBJECT_NMAE] = Convert.ToString(cmbSubjects.SelectedItem);
        aoDataRow[S_TEACHER_SUBJECT_ID] = 0;
        aoDataRow[S_TEACHER_ID] = Convert.ToInt32(hidTeacherId.Value);
        return aoDataRow;
    }

    /// <summary>
    /// This method is used to create new datatable
    /// </summary>
    /// <returns></returns>

    private DataTable CreateDivisionwiseSubjectDetailsTable()
    {
        const string S_INT_DATA_TYPE = "System.Int32";
        const string S_STRING_DATA_TYPE = "System.String";
 
        // Create a new DataTable for educationa details. 
        DataTable oDTEducationDetails = new DataTable();

        // Add columns to the Item table.
       
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_STANDARD_DIVISION_ID, ref oDTEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_STANDARD_DIVISION_NAME, ref oDTEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_SUBJECT_ID, ref oDTEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_SUBJECT_NMAE, ref oDTEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_TEACHER_SUBJECT_ID, ref oDTEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_TEACHER_ID, ref oDTEducationDetails, false);
        return oDTEducationDetails;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>

    private void AddDataColumnToItemTable(string asDataType, string asColumnName,ref DataTable aoDataTable,bool abIsPrimaryKey)
    {
        DataColumn oDataColumn = new DataColumn();
        oDataColumn.DataType = System.Type.GetType(asDataType);
        oDataColumn.ColumnName = asColumnName;
        aoDataTable.Columns.Add(oDataColumn);

        if (abIsPrimaryKey)
        {
            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = oDataColumn;
            aoDataTable.PrimaryKey = keys;
        }
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidAreYouSureDeleteDetails.Value = Resources.LocalizedResources.AreYouSureDeleteDetails;
        hidSomeClassSubjectsAlreadyAssignedMsg.Value = Resources.LocalizedResources.SomeClassSubjectsAlreadyAssignedMsg;
        hidAreYouSureYouWantToContinue.Value = Resources.LocalizedResources.AreYouSureYouWantToContinue;
        hidAtLeastOneClassSubjectSelected.Value = Resources.LocalizedResources.AtLeastOneClassSubjectSelected;
    }

    /// <summary>
    /// This method is used to check Dependency to delete
    /// </summary>
    /// <returns></returns>
    public string CheckDependencyToDelete()
    {
        CheckBox chkIsSelected = new CheckBox();
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = null;
        string sErrorMessage = string.Empty;

        for (int i = 0; i < grdDivisionWiseSubjects.Rows.Count; i++)
        {
            string sMessage = string.Empty;
            oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
            chkIsSelected = (CheckBox)grdDivisionWiseSubjects.Rows[i].FindControl("chkIsSelected");
            if (chkIsSelected.Checked == false)
            {
                int iTeacherSubjectId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[i][S_TEACHER_SUBJECT_ID]);
                oTeacherSubjectAssignmentBL.TeacherSubjectId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[i][S_TEACHER_SUBJECT_ID]);
                sMessage = oTeacherSubjectAssignmentBL.CheckDependencies();
                if (sMessage.Equals(string.Empty))
                {
                    int iTeacher_Id = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[i][3].ToString());
                    if (iTeacher_Id != Convert.ToInt32(hidTeacherId.Value))
                        hidFlag.Value = "true";
                    else
                        hidFlag.Value = "false";
                }
                else
                {
                    sErrorMessage = sErrorMessage + "<BR />" + lblTeacherName.Text + " " + Resources.LocalizedResources.IsAlreadyAssociatedWith + " " + grdDivisionWiseSubjects.Rows[i].Cells[1].Text + " : " + grdDivisionWiseSubjects.Rows[i].Cells[2].Text + " " + Resources.LocalizedResources.InTimetable;
                }
            }
        }
        return sErrorMessage;
    }

    #endregion    
}
