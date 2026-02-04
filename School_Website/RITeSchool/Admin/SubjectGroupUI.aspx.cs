using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

/// <summary>
/// This class is used to add and edit subject group.
/// </summary>
public partial class SubjectGroupUI : SchoolBase
{

    #region Constants

    // Constants to index grid columns
    const Int32 I_DELETE_COLUMN_INDEX = 1;

    //constants for datatypes
    const string S_INT_DATA_TYPE = "System.Int32";
    const string S_STRING_DATA_TYPE = "System.String";

    // Constants for databse column names
    private const string S_FIELD_SUBJECT_ID = "Subject_Id";
    private const string S_FIELD_SUBJECT_NAME = "Subject_Name";
    private const string S_GRIDVIEW_DATASOURCE = "grdSubjects_DataSource";
    const string S_ERROR_DUPLICATE_SUBJECT = "The child subject is already added in group.";
    const string S_ERROR_PARENT_SUBJECT = "The child subject is same as parent group subject.";
    const string S_ERR_MSG_SELECT_TWO_SUBJECT = "Subject group should contain at least two subjects.";
    const string S_ERROR_MSG_GROUP_AVAILABLE = "These subject's group is already exists.";
    const string S_CLASS_ERROR = "LblErrorMsg";
    const string S_DELETE_ROW = "DeleteRow";

    //constants for dataset
    const int I_CHILD_SUBJECTS = 1;
    const int I_PARENT_SUBJECTS = 0;

    #endregion " Constants "

    #region event handlers

    /// <summary>
    /// This event is used to fill all page controls and to set client side script properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            ClearErrorLabel();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                InitializeFields();
                GetQueryString();
                FillSubjectsCombos();
                FillSubjectGridView();                
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add/edit subject group.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iRowCount = grdSubjects.Rows.Count;

            // check if some one changes parent subject while saving and is already added as child subject,then throw exception
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                if (cmbParentSubjects.SelectedValue.Equals(grdSubjects.DataKeys[iRowIndex].Value.ToString()))
                    throw new DuplicateSubject(Resources.LocalizedResources.TheChildSubjectIsSameAsParentGroupSubject);
            }
            // If only one subject added in group. 
            if (iRowCount < 2)
                throw new GroupCannotBeFormed(Resources.LocalizedResources.SubjectGroupShouldContainAtLeastTwoSubjects);

            //Checks that subject group allready present or not.
            if (IsGroupAvailable())
                throw new GroupAvailable(Resources.LocalizedResources.TheseSubjectsGroupIsAlreadyExists);

            ClearErrorLabel();
            Collection<SubjectMasterBL> oSubjectsCollection = new Collection<SubjectMasterBL>();
            Collection<SubjectGroupsBL> oSubjectGroupsCollection = new Collection<SubjectGroupsBL>();
            SubjectGroupsBL oSubjectGroupBL = new SubjectGroupsBL();

            int iParentGroupId;

            //add new group
            if (hidGroupId.Value.Equals("0"))
                oSubjectsCollection = CreateCollectionObjectForNewGroup();
            //edit group
            else
            {
                iParentGroupId = Convert.ToInt32(hidGroupId.Value);
                oSubjectsCollection = CreateCollectionObjectToEditGroup(iParentGroupId);
            }
            if (oSubjectsCollection.Count > 0)
            {
                oSubjectGroupBL = SetSubjectGroupBL(oSubjectsCollection);
                oSubjectGroupBL.UpdateSubjectGroups();

                if (hidIsConfig.Value != "Y")
                    //InsertSubjectsGroupConfigDetails();
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SubjectGroups));
            }
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Admin/SubjectGroupsListUI.aspx?" + CommonUtility.EncryptQuerystring("Is_Configured=" + hidIsConfig.Value + "&StandardDivisionId=" + hidStandardDivisionId.Value));
        }
        catch (DuplicateName exDuplicate)
        {
            lblError.Text = exDuplicate.Message;
            lblError.CssClass = S_CLASS_ERROR;
        }
        catch (GroupCannotBeFormed exGroupCannotBeFormed)
        {
            lblError.Text = exGroupCannotBeFormed.Message;
            lblError.CssClass = S_CLASS_ERROR;
        }
        catch (DuplicateSubject exDuplicate)
        {
            lblError.Text = exDuplicate.Message;
            lblError.CssClass = S_CLASS_ERROR;
        }
        catch (GroupAvailable exAvailable)
        {
            lblError.Text = exAvailable.Message;
            lblError.CssClass = S_CLASS_ERROR;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add selected child subject in grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        int iRowCount = grdSubjects.Rows.Count;
        try
        {
            ValidateSubjectGroup();
            AddSubjectsToGrid();
            cmbSubjects.SelectedIndex = 0;
            ClearErrorLabel();
            if (grdSubjects.Rows.Count > 0)
                cmbParentSubjects.Enabled = false;
        }
        catch (DuplicateName exDuplicate)
        {
            lblError.CssClass = S_CLASS_ERROR;
            lblError.Text = exDuplicate.Message;
        }
        catch (DuplicateSubject exDuplicate)
        {
            lblError.CssClass = S_CLASS_ERROR;
            lblError.Text = exDuplicate.Message;
        }
        catch (GroupCannotBeFormed exGroupCannotBeFormed)
        {
            lblError.Text = exGroupCannotBeFormed.Message;
            lblError.CssClass = S_CLASS_ERROR;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete child subject from subject group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_rowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case S_DELETE_ROW:
                    DeletedSubjectDetails(iRowIndex);
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set delete conformation message to each delete button of child subject grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                ImageButton oImgDelete = (ImageButton)e.Row.Cells[I_DELETE_COLUMN_INDEX].Controls[Constants.I_ZERO];
                oImgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Event Handlers "

    #region private methods

    /// <summary>
    /// This method is used to ValidateSubjectGroup selected child subject with subjects in child subject grid.
    /// </summary>
    private void ValidateSubjectGroup()
    {
        int iRowCount = grdSubjects.Rows.Count;

        if (cmbParentSubjects.SelectedValue.Equals(cmbSubjects.SelectedValue))
        {
            throw new DuplicateSubject(Resources.LocalizedResources.TheChildSubjectIsSameAsParentGroupSubject);
        }
        //check if is subject already there in grid
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            if (grdSubjects.DataKeys[iRowIndex].Value.ToString().Equals(cmbSubjects.SelectedValue))
            {
                throw new DuplicateSubject(Resources.LocalizedResources.TheChildSubjectIsAlreadyAddedInGroup);
            }
            if (cmbParentSubjects.SelectedValue.Equals(grdSubjects.DataKeys[iRowIndex].Value.ToString()))
            {
                throw new DuplicateSubject(Resources.LocalizedResources.TheChildSubjectIsSameAsParentGroupSubject);
            }
        }
    }

    /// <summary>
    /// This function  creates the subject collection to be saved in existing group
    /// in edit mode.
    /// </summary>
    /// <returns></returns>
    private Collection<SubjectMasterBL> CreateCollectionObjectToEditGroup(int aiParentGroupId)
    {
        Collection<SubjectMasterBL> oSubjectsCollection = new Collection<SubjectMasterBL>();
        int iRowCount = grdSubjects.Rows.Count;
        SubjectGroupsBL oSubjectGroupsBL = new SubjectGroupsBL();
        oSubjectGroupsBL.ParentSubjectId = Convert.ToInt32(hidSubjectId.Value);
        oSubjectGroupsBL.ParentGroupId = Convert.ToInt32(hidGroupId.Value);
        oSubjectGroupsBL.SchoolId = miSchoolId;
        oSubjectGroupsBL.academicyearId = miAcademicYearId;
        DataTable oDTSubjectIds = oSubjectGroupsBL.RetriveSubjectIdsForGroup();

        //check if subject needs to be added in the group
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            int iSubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex].Value);
            //insert subject in the group if it is not already included 
            if ((!CommonUtility.CheckIfValueExistsInDataTable(oDTSubjectIds, S_FIELD_SUBJECT_ID, iSubjectId.ToString())))
            {
                SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(iSubjectId);
                oSubjectMasterBL.ConfigurationAction = Constants.Action.Insert;
                oSubjectsCollection.Add(oSubjectMasterBL);
            }
        }

        //check for needs to be deleted from the group
        //loop through subject id dataset
        for (int iCount = 0; iCount < oDTSubjectIds.Rows.Count; iCount++)
        {
            bool bIfDelete = true;
            string sSubjectId = oDTSubjectIds.Rows[iCount][S_FIELD_SUBJECT_ID].ToString();
            //loop through grid rows
            //if subject is there in database
            //and not in grid it is marked for deletion
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                if (sSubjectId.Equals(grdSubjects.DataKeys[iRowIndex]["Subject_Id"].ToString()))
                {
                    //not to delete
                    bIfDelete = false;
                    break;
                }
            }
            if (bIfDelete)//if subject needs to be deleted from database
            {
                int iSubjectId = Convert.ToInt32(sSubjectId);
                SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(iSubjectId);
                oSubjectMasterBL.ConfigurationAction = Constants.Action.Delete;
                oSubjectsCollection.Add(oSubjectMasterBL);
            }
        }
        //Update parent name
        if (!cmbParentSubjects.SelectedValue.Equals(hidGroupId.Value.Trim()))
        {
            SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(Convert.ToInt32(hidGroupId.Value));
            oSubjectMasterBL.ConfigurationAction = Constants.Action.Update;
            oSubjectsCollection.Add(oSubjectMasterBL);
        }
        return oSubjectsCollection;
    }

    /// <summary>
    /// This function creates the subject collection,
    /// used to save in new subject group in "new" mode.
    /// </summary>
    private Collection<SubjectMasterBL> CreateCollectionObjectForNewGroup()
    {
        int iParentGroupId = SubjectGroupsBL.GetNextParentGroupId();
        Collection<SubjectMasterBL> oSubjectsCollection = new Collection<SubjectMasterBL>();
        hidGroupId.Value = iParentGroupId.ToString();
        int iRowCount = grdSubjects.Rows.Count;

        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(Convert.ToInt32(grdSubjects.DataKeys[iRowIndex]["Subject_Id"]));
            oSubjectMasterBL.ConfigurationAction = Constants.Action.Insert;
            oSubjectsCollection.Add(oSubjectMasterBL);
        }
        return oSubjectsCollection;
    }

    /// <summary>
    /// This function creates and returns subject group object.
    /// </summary>
    /// <param name="aoSubjectsCollection"></param>
    /// <param name="aiParentGroupId"></param>
    /// <returns></returns>
    private SubjectGroupsBL SetSubjectGroupBL(Collection<SubjectMasterBL> aoSubjectsCollection)
    {
        SubjectGroupsBL oSubjectGroupsBL = new SubjectGroupsBL();
        oSubjectGroupsBL.SubjectCollection = aoSubjectsCollection;
        oSubjectGroupsBL.SchoolId = miSchoolId;
        oSubjectGroupsBL.ParentSubjectId = Convert.ToInt32(cmbParentSubjects.SelectedValue);
        oSubjectGroupsBL.academicyearId = miAcademicYearId;
        oSubjectGroupsBL.ChangedParentSubjectId = Convert.ToInt32(hidGroupId.Value);
        return oSubjectGroupsBL;
    }

    /// <summary>
    /// This function extracts values from querystring 
    /// and stores them in appropriate variables.
    /// </summary>
    private void GetQueryString()
    {
        ReadQuerystring();
        MasterPage oMasterPage = (MasterPage)this.Master;
        if (QueryString["ParentSubjectId"] != null)
        {
            hidSubjectId.Value = QueryString["ParentSubjectId"];
            oMasterPage.NodeTitle = "Edit Subject Group";
        }
        else
        {
            hidSubjectId.Value = Constants.S_ZERO;
            oMasterPage.NodeTitle = "Add Subject Group";
        }
        
		hidGroupId.Value = QueryString["ParentGroupId"] ?? Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to initialise page controls.
    /// </summary>
    private void InitializeFields()
    {
        grdSubjects.PageSize = 30;
        BtnAdd.Attributes.Add("onclick", "ResetLabel()");
        validSummaryAdd.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAreYouSureYouWantToDeleteThisSubject.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisSubject;
        btnSave.Attributes["onclick"] = "javascript:DisableButtons()";       
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave, BtnAdd });
    }

    /// <summary>
    /// This function creates and returns the subject master object
    /// </summary>
    /// <param name="aiStandardId"></param>
    /// <returns></returns>
    private SubjectMasterBL SetSubjectMasterBL(int aiSubjectId)
    {
        // This method creates the default object for the configuration and returns the same.
        SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();
        oSubjectMasterBL.ParentSubjectId = Convert.ToInt32(cmbParentSubjects.SelectedValue);
        oSubjectMasterBL.ParentGroupId = Convert.ToInt32(hidGroupId.Value);
        oSubjectMasterBL.ParentSubjectId = Convert.ToInt32(cmbParentSubjects.SelectedValue);
        oSubjectMasterBL.SubjectId = aiSubjectId;
        oSubjectMasterBL.SchoolId = miSchoolId;
        oSubjectMasterBL.UpdatedById = miUserId;
        oSubjectMasterBL.InsertedByid = miUserId;
        oSubjectMasterBL.AcademicYearId = miAcademicYearId;
        oSubjectMasterBL.StandardDivisionID = hidStandardDivisionId.Value.ToInt();
        return oSubjectMasterBL;
    }

    /// <summary>
    /// This function fills the grid with subjects in the specified group in edit mode.
    /// </summary>
    private void FillSubjectGridView()
    {
        if (!hidGroupId.Value.Equals("0"))// in edit mode
        {
            SubjectGroupsBL oSubjectGroupsBL = new SubjectGroupsBL();
            oSubjectGroupsBL.ParentSubjectId = Convert.ToInt32(hidSubjectId.Value);
            oSubjectGroupsBL.ParentGroupId = Convert.ToInt32(hidGroupId.Value);
            oSubjectGroupsBL.SchoolId = miSchoolId;
            oSubjectGroupsBL.academicyearId = miAcademicYearId;
            DataTable oDsSubjectIds = oSubjectGroupsBL.RetriveSubjectIdsForGroup();
            DataView oDTItemView = oDsSubjectIds.DefaultView;

            grdSubjects.DataSource = oDTItemView;
            ViewState[S_GRIDVIEW_DATASOURCE] = oDsSubjectIds;
            grdSubjects.DataBind();
            hidSubjectIDs.Value = GetSubjectIDs();
        }
    }

    /// <summary>
    /// This function is used to decrypt the query string.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            hidIsConfig.Value = QueryString["Is_Configured"];

            hidStandardDivisionId.Value = QueryString["StandardDivisionId"];
            hidClassName.Value = lblClassName.Text = QueryString["ClassName"];
            btnBack.PostBackUrl = "~/RITeSchool/Admin/SubjectGroupsListUI.aspx?" + CommonUtility.EncryptQuerystring("Is_Configured=" + hidIsConfig.Value + "&StandardDivisionId=" + hidStandardDivisionId.Value);
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    
    /// <summary>
    /// This function is used to fill combos of both parent and child subject groups.
    /// </summary>
    private void FillSubjectsCombos()
    {
        SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDTSubjectCollection = oSubjectCollectionBL.GetChildParentSubjects();

        ControlUtility.FillDropDownList(oDTSubjectCollection.Tables[I_CHILD_SUBJECTS], ref cmbSubjects,
                                       Constants.S_SUBJECT_ID_FIELD,
                                       Constants.S_SUBJECT_NAME_FIELD,
                                       Constants.S_SELECT);


        ControlUtility.FillDropDownList(oDTSubjectCollection.Tables[I_PARENT_SUBJECTS], ref cmbParentSubjects,
                                       Constants.S_SUBJECT_ID_FIELD,
                                       Constants.S_SUBJECT_NAME_FIELD,
                                       Constants.S_SELECT);
        if (hidSubjectId.Value != "0")
        {
            cmbParentSubjects.SelectedValue = hidSubjectId.Value.Trim();
            cmbParentSubjects.Enabled = false;
        }
    }

    /// <summary>
    /// This function adds row for subject in the grid.
    /// </summary>
    private void AddSubjectsToGrid()
    {
        DataTable oDTSubjects;
        if (ViewState[S_GRIDVIEW_DATASOURCE] == null)
            oDTSubjects = CreateSubjectsTable();
        else
            oDTSubjects = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];

        // Once a table has been created,create DataRow.    
        oDTSubjects.Rows.Add(AddSubjectsDetailsToDataRow(oDTSubjects.NewRow()));

        DataView oDTItemView = oDTSubjects.DefaultView;
        grdSubjects.DataSource = oDTItemView;
        ViewState[S_GRIDVIEW_DATASOURCE] = oDTSubjects;
        grdSubjects.DataBind();
    }

    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow AddSubjectsDetailsToDataRow(DataRow oDR)
    {
        DataRow oDRItem;
        int iSubjectId = Convert.ToInt32(cmbSubjects.SelectedValue);
        oDRItem = oDR;
        // Then add the new row to the collection.
        oDRItem[S_FIELD_SUBJECT_ID] = iSubjectId;
        oDRItem[S_FIELD_SUBJECT_NAME] = Convert.ToString(cmbSubjects.SelectedItem);
        return oDRItem;
    }

    /// <summary>
    /// This method is used to check whether selected subjects group is already exists or not.
    /// </summary>
    /// <returns></returns>
    private bool IsGroupAvailable()
    {
        string sSubjectIds = GetSubjectIDs();
        if (hidSubjectIDs.Value != sSubjectIds)
            return SubjectGroupsBL.IsSubjectGroupAvailable(miSchoolId, miAcademicYearId, sSubjectIds, hidStandardDivisionId.Value.ToInt());
        else
            return false;
    }

    /// <summary>
    /// This method is used to get list of child subject IDs.
    /// </summary>
    /// <returns></returns>
    private string GetSubjectIDs()
    {
        string sSubjectIds = string.Empty;
        int iCount = grdSubjects.Rows.Count;
        for (int iRowIndex = 0; iRowIndex < iCount; iRowIndex++)
            sSubjectIds = sSubjectIds + grdSubjects.DataKeys[iRowIndex].Value + ",";
        sSubjectIds = sSubjectIds.Substring(0, sSubjectIds.Length - 1);
        return sSubjectIds;
    }

    /// <summary>
    /// This method is used to create new datatable.
    /// </summary>
    /// <returns></returns>
    private DataTable CreateSubjectsTable()
    {
        // Create a new DataTable for educationa details. 
        DataTable oDTSubjects = new DataTable();
        // Add columns to the Item table.
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_FIELD_SUBJECT_ID, ref oDTSubjects, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_FIELD_SUBJECT_NAME, ref oDTSubjects, false);

        return oDTSubjects;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemTable(string asDataType, string asColumnName,
                                                        ref DataTable aoDataTable,
                                                              bool abIsPrimaryKey)
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
    /// This function deletes the subject row from the grid.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    private void DeletedSubjectDetails(int aiRowIndex)
    {
        string sSubjectId = grdSubjects.DataKeys[aiRowIndex].Value.ToString();
        DataTable oDTSubjects = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
        DataColumn[] arrDatacolumn = new DataColumn[1];
        DataRow oDTRow = oDTSubjects.NewRow();
        arrDatacolumn[0] = (DataColumn)oDTSubjects.Columns[S_FIELD_SUBJECT_ID];
        oDTRow = oDTSubjects.Rows[aiRowIndex];
        oDTRow.Delete();
        oDTSubjects.AcceptChanges();

        grdSubjects.DataSource = oDTSubjects;
        grdSubjects.DataBind();
        ViewState[S_GRIDVIEW_DATASOURCE] = oDTSubjects;
    }

    /// <summary>
    /// This method clears error label. 
    /// </summary>
    private void ClearErrorLabel()
    {
        lblClassName.Text = hidClassName.Value;
        lblError.Text = string.Empty;
        lblError.CssClass = string.Empty;
    }

    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    public void RefreshValue()
    {
        validSummaryAdd.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAreYouSureYouWantToDeleteThisSubject.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisSubject;
    }

    #endregion
}

#region Exceptions

/// <summary>
/// This is an exception class
/// Represents the error when user tries to save subject group with less than 2 subjects in it
/// </summary>
public class GroupCannotBeFormed : Exception
{
    private string msMessage = string.Empty;

    public override string Message
    {
        get
        {
            return msMessage;
        }
    }

    public GroupCannotBeFormed(string asMessage)
        : base(asMessage)
    {
        msMessage = asMessage;
    }

}
/// <summary>
/// This is an exception class
/// Represents the error when user tries to save subject group with duplicate entry for one subject
/// </summary>
public class DuplicateSubject : Exception
{
    private string msMessage = string.Empty;

    public override string Message
    {
        get
        {
            return msMessage;
        }
    }

    public DuplicateSubject(string asMessage)
        : base(asMessage)
    {
        msMessage = asMessage;
    }

}

/// <summary>
/// This is an exception class
/// Represents the error when user tries to save subject group with duplicate entry for one subject
/// </summary>
public class GroupAvailable : Exception
{
    private string msMessage = string.Empty;

    public override string Message
    {
        get
        {
            return msMessage;
        }
    }

    public GroupAvailable(string asMessage)
        : base(asMessage)
    {
        msMessage = asMessage;
    }

    
}

#endregion