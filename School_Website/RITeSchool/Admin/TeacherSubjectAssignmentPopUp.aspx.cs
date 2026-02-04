/* File Name  : TeacherSubjectAssignmentPopUp.aspx.cs
 * Created By : Milind
 * Date       : 27/4/2009
 * Description :This class is used to assign/delete teacher to standard-division for subject.
*/
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Resources;

public partial class TeacherSubjectAssignmentPopUp : SchoolBase
{
    #region Constants

    private const string S_STANDARD_DIVISION_ID = "SchoolWise_Standard_Division_Id";
	private const string S_SUBJECT_ID = "Subject_Id";
	private const string S_TEACHER_NAME = "TeacherName";
	private const string S_TEACHER_SUBJECT_ID = "Teacher_Subject_Id";
	private const string S_IS_EXCLUSIVE = "IsExclusive";
	private const string S_TEACHER_ID = "Teacher_Id";
	private const int I_DELETE_INDEX = 3;
	private const int I_EDIT_INDEX = 2;
	private const string S_GRIDVIEW_DATASOURCE = "grdDivisionWiseSubjects_DataSource";
	private const string S_UPDATE_TEACHER = "Update Teacher";
	private const string S_ADD_TEACHER = "Add Teacher";

    #endregion

	#region Data Members

	private int miStandardDivisionId;
	private int miSubjectId;
	private string msTeacherSubjectId;
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
	#endregion
	
    #region Events

	/// <summary>
	/// This event is used to set session variable values.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e) 
	{
		try
		{
			base.OnInit(e);
            InitializeMemberVariables();            
			ReadQuerystring();
		}
		catch (Exception ex) 
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to fill teacher combobox and fill subject teachers gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                SetDefaultValues();
                RefreshValues();
                FillTeacherNameCombobox();
                FillSubjectTeachersGrid();
                SetJavascriptAttributes();
				DisplayStandardAndDivisionName();
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
    /// This event is used fill the grid(temporary) with new assigned teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddDetails_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblDuplicateDetails.Text == string.Empty)
            {
                AddNewTeacherAssignment();
                hidSelectedIndex.Value = null;
                cmbTeacherName.Enabled = true;
                hidbtnAddDetails.Value = S_ADD_TEACHER;
                btnAddDetails.Text = oResourceManager.GetString(hidbtnAddDetails.Value.Replace(" ", string.Empty));
                chkIsExclusive.Enabled = true;
                chkIsExclusive.Checked = false;
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check whether selected teacher is exist in the grid or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeacherName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!CheckTeacherIsDuplicate())
            {
                lblDuplicateDetails.Text = string.Empty;
                btnAddDetails.Focus();
            }
            else
                lblDuplicateDetails.Text = Resources.LocalizedResources.SelectedTeacherAlreadyExistsList;// S_DUPLICATION_ERROR_MSG;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());	
	    }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
	   try
        {            
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AssignedTeacherToSub));
            ClosePopup();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save teacher - subject assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            InsertTeacherSubjectDetails();
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AssignedTeacherToSub));
            ClosePopup();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add attribute on delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDivisionWiseSubjects_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                ImageButton oImgDelete = (ImageButton)e.Row.Cells[I_DELETE_INDEX].Controls[Constants.I_ZERO];
                oImgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                bool bIsExclusive = Convert.ToBoolean(grdDivisionWiseSubjects.DataKeys[e.Row.RowIndex][S_IS_EXCLUSIVE].ToString());
                HiddenField hidIsExclusive = (HiddenField)e.Row.Cells[0].FindControl("hidIsExclusive");
                hidIsExclusive.Value = bIsExclusive ? "Y" : "N";
                Image imgIsExclusive = (Image)e.Row.Cells[0].FindControl("imgIsExclusive");
                imgIsExclusive.Visible = bIsExclusive;
                int iTeacherId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[e.Row.RowIndex][2].ToString());                
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete teacher subject assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDivisionWiseSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(e.CommandArgument);
            int iTeacherId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[iRowIndex][S_TEACHER_ID].ToString());
            if (e.CommandName.ToUpper().Equals("DELETE_ROW"))
            {
                switch (e.CommandName.ToUpper())
                {
                    case "DELETE_ROW":
                        hidSelectedIndex.Value = null;
                        lblDuplicateDetails.Text = string.Empty;
                        if (DeleteTeacherAssignment(iRowIndex))
                            lblDuplicateDetails.Text = string.Empty;
                        cmbTeacherName.Enabled = true;
                        hidbtnAddDetails.Value = S_ADD_TEACHER;
                        btnAddDetails.Text = oResourceManager.GetString(hidbtnAddDetails.Value.Replace(" ", string.Empty));
                        if (grdDivisionWiseSubjects.Rows.Count == Constants.I_ZERO)
                        {
                            chkIsExclusive.Enabled = false;
                            chkIsExclusive.Checked = true;
                        }
                        else
                            chkIsExclusive.Enabled = true;
                        break;
                }
            }
            else
                LoadTeacherDetails(iTeacherId, iRowIndex);            
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to load teacher details.
    /// </summary>
    /// <param name="aiTeacherId"></param>
    /// <param name="aiRowIndex"></param>
    private void LoadTeacherDetails(int aiTeacherId, int aiRowIndex)
    {
        cmbTeacherName.SelectedValue = aiTeacherId.ToString();
        cmbTeacherName.Enabled = false;
        chkIsExclusive.Checked = Convert.ToBoolean(grdDivisionWiseSubjects.DataKeys[aiRowIndex][S_IS_EXCLUSIVE].ToString());
        btnAddDetails.Enabled = true;
        hidbtnAddDetails.Value = S_UPDATE_TEACHER;
        btnAddDetails.Text = oResourceManager.GetString(hidbtnAddDetails.Value.Replace(" ", string.Empty));
        lblDuplicateDetails.Text = string.Empty;
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to show standard,division and subject name.
    /// </summary>
    private void DisplayStandardAndDivisionName()
    {
        const int I_TEACHER_NAME_INDEX = Constants.I_ZERO;
        const int I_DIVISION_NAME_INDEX = 1;        

        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL();
		DataTable oDSStandardDivisionName = oStandardDivisionMasterBL.GetStandardAndDivisionName(miSchoolId, miStandardDivisionId);

        lblStandardName.Text = oDSStandardDivisionName.Rows[0][I_TEACHER_NAME_INDEX].ToString();
        lblDivisionName.Text = oDSStandardDivisionName.Rows[0][I_DIVISION_NAME_INDEX].ToString();

        SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();
		lblSubjectName.Text = oSubjectMasterBL.GetSubjectName(miSchoolId, miSubjectId);
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        miStandardDivisionId = QueryString["StandardDivisionId"].ToInt();
        miSubjectId = QueryString["SubjectId"].ToInt();
        msTeacherSubjectId = QueryString["TeacherSubjectId"];
        hidStandard.Value = QueryString["StandardId"].ToString();
        hidCategoryId.Value = QueryString["CategoryId"].ToString();
        hidName.Value = QueryString["Name"].ToString();
    }

    /// <summary>
    /// This method is used to fill teacher combobox.
    /// </summary>
    private void FillTeacherNameCombobox()
    {        
        string sDisplayMember = hidDisplayMember.Value;
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillTeacherNameComboBoxForAssignSubject(miSchoolId, miSubjectId, miStandardDivisionId, ref cmbTeacherName, sDisplayMember);
    }

    /// <summary>
    /// This method is used to fill grid with teachers.
    /// </summary>
    private void FillSubjectTeachersGrid()
    {
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
		DataTable oDtTeachers = oTeacherSubjectAssignmentBL.GetSubjectAssignedTeacherDetails(miStandardDivisionId, miSubjectId, miSchoolId, miAcademicYearId, msTeacherSubjectId);

        oDtTeachers.DefaultView.Sort = S_TEACHER_NAME + " " + Constants.S_ASCENDING;
        ViewState[S_GRIDVIEW_DATASOURCE] = oDtTeachers;
       
        grdDivisionWiseSubjects.DataSource = oDtTeachers;
        grdDivisionWiseSubjects.DataBind();
        if (grdDivisionWiseSubjects.Rows.Count == Constants.I_ZERO)
        {
            chkIsExclusive.Enabled = false;
            chkIsExclusive.Checked = true;
        }

        hidRowCnt.Value = oDtTeachers.Rows.Count.ToString();
    }

    /// <summary>
    /// This method is used to add new row into grid.
    /// </summary>
    private void AddNewTeacherAssignment()
    {
        if (!CheckMaximumTeacherAssociation())
        {
            DataTable oDtEducationDetails;
            
                if (ViewState[S_GRIDVIEW_DATASOURCE] == null)
                    oDtEducationDetails = CreateDivisionwiseSubjectDetailsTable();
				else if (btnAddDetails.Text != oResourceManager.GetString(S_UPDATE_TEACHER.Replace(" ",string.Empty)))
                {
                    oDtEducationDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
                    // Once a table has been created,create DataRow.    
                    oDtEducationDetails.Rows.Add(UpdateDataRow(oDtEducationDetails.NewRow()));
                }
                else
                {
                    oDtEducationDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
                    int iTeacherId = Convert.ToInt32(cmbTeacherName.SelectedValue);
                    DataRow[] oDtRow = oDtEducationDetails.Select(S_TEACHER_ID + "=" + iTeacherId);
                    oDtRow[0].BeginEdit();
                    oDtRow[0][S_STANDARD_DIVISION_ID] = miStandardDivisionId;
                    oDtRow[0][S_TEACHER_NAME] = Convert.ToString(cmbTeacherName.SelectedItem);
                    oDtRow[0][S_IS_EXCLUSIVE] = chkIsExclusive.Checked;
                    oDtRow[0][S_SUBJECT_ID] = miSubjectId;
                    oDtRow[0][S_TEACHER_SUBJECT_ID] = Constants.I_ZERO;
                    oDtRow[0][S_TEACHER_ID] = Convert.ToInt32(cmbTeacherName.SelectedValue);
                    oDtEducationDetails.AcceptChanges();
                    oDtEducationDetails.Rows[0].EndEdit();                   
                }

            oDtEducationDetails.DefaultView.Sort = S_TEACHER_NAME + " " + Constants.S_ASCENDING;
            DataView oDtItemView = oDtEducationDetails.DefaultView;
            oDtItemView.ToTable().DefaultView.Sort = S_TEACHER_NAME + " " + Constants.S_ASCENDING;
            grdDivisionWiseSubjects.DataSource = oDtItemView;
            oDtEducationDetails = oDtItemView.ToTable();
            ViewState[S_GRIDVIEW_DATASOURCE] = oDtEducationDetails;
            grdDivisionWiseSubjects.DataBind();
            cmbTeacherName.SelectedValue = Constants.S_ZERO;
            if (grdDivisionWiseSubjects.Rows.Count == Constants.I_ZERO)
            {
                chkIsExclusive.Enabled = false;
                chkIsExclusive.Checked = true;
            }
            else
            {
                chkIsExclusive.Enabled = true;
                chkIsExclusive.Checked = false;
            }

            hidRowCnt.Value = oDtEducationDetails.Rows.Count.ToString();
        }
        else
        {
            cmbTeacherName.SelectedValue = Constants.S_ZERO;
            lblDuplicateDetails.Text = " " + Resources.LocalizedResources.YouCanAssociateOnly + " " + Settings.MaxTeacherForSubject.ToString() + " " + Resources.LocalizedResources.teachersToSubject;
            btnAddDetails.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidDisplayMember.Value = Constants.S_SELECT;
	    hidbtnAddDetails.Value=S_ADD_TEACHER;
        cmbTeacherName.Focus();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSubmit, btnClose, btnAddDetails });
        btnSubmit.Attributes.Add("onclick", "if(!CheckRecordCount()) return false;");
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
        DataTable oDtEducationDetails = new DataTable();

        // Add columns to the Item table.        
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_STANDARD_DIVISION_ID, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_SUBJECT_ID, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_TEACHER_NAME, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_TEACHER_SUBJECT_ID, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_IS_EXCLUSIVE, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_TEACHER_ID, ref oDtEducationDetails, false);

        return oDtEducationDetails;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemTable(string asDataType, string asColumnName, ref DataTable aoDataTable, bool abIsPrimaryKey)
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
    /// This method is used to set values of control to the newly added datarows.
    /// </summary>
    private DataRow UpdateDataRow(DataRow aoDataRow)
    {       
        DataRow oDrItem = aoDataRow;
        // Then add the new row to the collection.
        oDrItem[S_STANDARD_DIVISION_ID] = miStandardDivisionId;
        oDrItem[S_TEACHER_NAME] = Convert.ToString(cmbTeacherName.SelectedItem);
        oDrItem[S_IS_EXCLUSIVE] = chkIsExclusive.Checked;
        oDrItem[S_SUBJECT_ID] = miSubjectId;
        oDrItem[S_TEACHER_SUBJECT_ID] = Constants.I_ZERO;
        oDrItem[S_TEACHER_ID] = Convert.ToInt32(cmbTeacherName.SelectedValue);
        return oDrItem;
    }

    /// <summary>
    /// This method is used to check whether teacher is duplicate in list or not.
    /// </summary>
    private bool CheckTeacherIsDuplicate()
    {
        if (hidSelectedIndex.Value == Constants.S_EMPTY_STRING)
        {  
            string sSelectedTeacher = cmbTeacherName.SelectedItem.Value;
            string sTeacherId;
            for (int iRowIndex = Constants.I_ZERO; iRowIndex < grdDivisionWiseSubjects.Rows.Count; iRowIndex++)
            {
                sTeacherId = Convert.ToString(grdDivisionWiseSubjects.DataKeys[iRowIndex][S_TEACHER_ID]);
                if (sTeacherId.Equals(sSelectedTeacher))
                    return true;
            }           
        }

        return false;
    }

    /// <summary>
    /// This method is used to check number of teachers associated with subject.
    /// </summary>
    private bool CheckMaximumTeacherAssociation()
    {
        string sMaxTeacher = Settings.MaxTeacherForSubject.ToString();
        return grdDivisionWiseSubjects.Rows.Count == Convert.ToInt32(sMaxTeacher) && btnAddDetails.Text == S_ADD_TEACHER;
    }

    /// <summary>
    /// This method is used to delete teacher subject assignment information.
    /// </summary>
    private bool DeleteTeacherAssignment(int aiRowIndex)
    {
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        oTeacherSubjectAssignmentBL.TeacherSubjectId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[aiRowIndex][S_TEACHER_SUBJECT_ID].ToString());
        string sMessage = oTeacherSubjectAssignmentBL.CheckDependencies();
        bool bReturn = false;
        if (sMessage.Equals(string.Empty))
        {
            int iCount;
            string sStandardDivisionId = grdDivisionWiseSubjects.DataKeys[aiRowIndex][S_STANDARD_DIVISION_ID].ToString();
            int iTeacherId = Convert.ToInt32(grdDivisionWiseSubjects.DataKeys[aiRowIndex][S_TEACHER_ID].ToString());

			// if teacher subject id is newly added.
            if (oTeacherSubjectAssignmentBL.TeacherSubjectId != Constants.I_ZERO)   
            {
                oTeacherSubjectAssignmentBL.TeacherId = iTeacherId;               
                oTeacherSubjectAssignmentBL.DeleteTeacherSubjectAssignmentForTeacher(iTeacherId, miSubjectId, Convert.ToInt32(sStandardDivisionId));
            }

            DataTable oDtEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
            for (iCount = Constants.I_ZERO; iCount < oDtEducationGridDetails.Rows.Count; iCount++)
               {
                   if (iTeacherId == Convert.ToInt32(oDtEducationGridDetails.Rows[iCount][S_TEACHER_ID]))
                   {
                        DataRow oDtRow = oDtEducationGridDetails.Rows[iCount];
                        oDtRow.Delete();
                        oDtEducationGridDetails.AcceptChanges();

                        grdDivisionWiseSubjects.DataSource = oDtEducationGridDetails;
                        grdDivisionWiseSubjects.DataBind();
                        ViewState[S_GRIDVIEW_DATASOURCE] = oDtEducationGridDetails;
                        hidRowCnt.Value = oDtEducationGridDetails.Rows.Count.ToString();
                   }
                }

			if (grdDivisionWiseSubjects.Rows.Count == Constants.I_ZERO)
            {
                lblDuplicateDetails.Text = string.Empty;
                tdSubmit.Visible = true;
                btnSubmit.Enabled = false;
            }
            else
                btnSubmit.Visible = true;
            bReturn = true;
            btnAddDetails.Enabled = true;
        }
        else
        {
            sMessage = grdDivisionWiseSubjects.Rows[aiRowIndex].Cells[1].Text + " " + Resources.LocalizedResources.IsAlreadyAssociatedWith + " " + lblStandardName.Text + " - " + lblDivisionName.Text + " " + lblSubjectName.Text + " " + Resources.LocalizedResources.InTimetable;
            lblDuplicateDetails.Visible = true;
            lblDuplicateDetails.Text = sMessage;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to insert or update teacher subject assignment.
    /// </summary>
    private void InsertTeacherSubjectDetails()
    {
        DataTable oDtEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
        Collection<TeacherSubjectAssignmentBL> oTeacherSubjects = new Collection<TeacherSubjectAssignmentBL>();

        foreach (DataRow oEducationDataRow in oDtEducationGridDetails.Rows)
        {
            TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
            oTeacherSubjectAssignmentBL.TeacherSubjectId = Convert.ToInt32(oEducationDataRow[S_TEACHER_SUBJECT_ID]);
            oTeacherSubjectAssignmentBL.TeacherId = Convert.ToInt32(oEducationDataRow[S_TEACHER_ID]);
            oTeacherSubjectAssignmentBL.SchoolId = miSchoolId;
            oTeacherSubjectAssignmentBL.StandardDivisionId = Convert.ToInt32(oEducationDataRow[S_STANDARD_DIVISION_ID]);
            oTeacherSubjectAssignmentBL.SubjectId = Convert.ToInt32(oEducationDataRow[S_SUBJECT_ID]);
            oTeacherSubjectAssignmentBL.IsExclusive = Convert.ToBoolean(oEducationDataRow[S_IS_EXCLUSIVE]);
            oTeacherSubjectAssignmentBL.InsertedById = miUserId;
            oTeacherSubjectAssignmentBL.UpdatedById = miUserId;
            oTeacherSubjectAssignmentBL.AssignmentAction = Constants.Action.Insert;
            oTeacherSubjects.Add(oTeacherSubjectAssignmentBL);
        }

        ArrayList oArrDeleteStatement = new ArrayList();
        foreach (DataRow oEducationDataRow in oDtEducationGridDetails.Rows)
        {
            int iTeacherId = Convert.ToInt32(oEducationDataRow[S_TEACHER_ID]);
            if (iTeacherId != Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]))
            {
                int iStandardDivisionId = Convert.ToInt32(oEducationDataRow[S_STANDARD_DIVISION_ID]);
                int iSubjectId = Convert.ToInt32(oEducationDataRow[S_SUBJECT_ID]);
                TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
                string sDeleteStatement = oTeacherSubjectAssignmentBL.GetDeleteStatementForTeacherId(iTeacherId, iSubjectId, iStandardDivisionId);
                oArrDeleteStatement.Add(sDeleteStatement);
            }
        }

        if (oTeacherSubjects.Count > 0)
        {
            TeacherSubjectAssignmentCollectionBL oTeacherSubjectAssignmentCollectionBL = new TeacherSubjectAssignmentCollectionBL(Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]));
            oTeacherSubjectAssignmentCollectionBL.UpdateTeacherSubjects(oTeacherSubjects, oArrDeleteStatement);
        }
    }
    
    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        btnAddDetails.Text = oResourceManager.GetString(hidbtnAddDetails.Value.Replace(" ", string.Empty));
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAtleastTeacherAssignedToSubject.Value = Resources.LocalizedResources.AtleastTeacherAssignedToSubject;
        hidAreYouSureDeleteDetails.Value = Resources.LocalizedResources.AreYouSureDeleteDetails;
        btnAddDetails.Text = oResourceManager.GetString(hidbtnAddDetails.Value.Replace(" ", string.Empty));
    }

    /// <summary>
    /// This method is used to close popup.
    /// </summary>
    private void ClosePopup()
    {
        string sQuerystring = string.Format("StandardId={0}&CategoryId={1}&Name={2}", hidStandard.Value,hidCategoryId.Value,hidName.Value);
        sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = string.Format("'?{0}'", sQuerystring);
        Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", sQuerystring));
    }

    #endregion
}