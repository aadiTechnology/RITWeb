using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;

/// <summary>
/// This class 
/// 1. displays alreay configuered subject groups  
/// 2. Provides option to add new groups
/// 3. to edit existing group's configuration
/// </summary>

public partial class SubjectGroupsListUI : SchoolBase
{
    #region constants

    const string S_GRD_EDIT_GROUP_COMMAND = "EditGroup";
    const int I_PARENT_GROUP_NAME_COLUMN_NO = 1;
    const string S_ERROR_MSG_SELECT_SUBJECT_GROUP = "At least one subject group should be selected for deletion.";
    
    #endregion

    #region event handlers
    /// <summary>
    /// This event is used to check preconditions and fill subject group grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    {
                        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    }
                    InitializeFields();
                    FillStandardCombobox();
                }
                SetClientSideScriptAttributes();
                ReadQuerystring();
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
    /// This event is used to add new subject group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sQuerystring = "Is_Configured=" + hidIsConfigured.Value + "&ClassName=" + cmbClass.SelectedItem.Text + "&StandardDivisionId=" + cmbClass.SelectedValue;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
            string sRedirectUrl = Constants.S_PAGE_SUBJECT_GROUPS + "?" + sEncrypt;
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(sRedirectUrl);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move back on subject configuration screen. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete subject group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SubjectGroupsBL oSubjectGroupsBL = new SubjectGroupsBL();
            oSubjectGroupsBL.SchoolId = miSchoolId;
            oSubjectGroupsBL.academicyearId = miAcademicYearId;
            ArrayList oArrayList = new ArrayList();
            int iRowCount = GrdSubjectGroup.Rows.Count;
            
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {                                                                                
                CheckBox oChkDelete = (CheckBox)GrdSubjectGroup.Rows[iRowIndex].FindControl("chkDelete");
                if (oChkDelete.Checked == true)
                    oArrayList.Add(GrdSubjectGroup.DataKeys[iRowIndex]["Parent_Group_Id"]);            
            }
            string sMessage = CheckDependencies();
            if (!sMessage.IsNullOrEmpty())
                throw new BusinessLogic.Exceptions.ReferenceExceptions(sMessage);
            oSubjectGroupsBL.DeleteSubjectGroup(oArrayList);
            if (!oSubjectGroupsBL.IsSubjectGroupPresent())
                DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SubjectGroups));
            FillSubjectGroupsGrid();
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions oEx)
        {
            lblErrorMessage.Text = CommonUtility.ModifyExceptionMessage(oEx.Message, "Subject group ", Resources.LocalizedResources.SubjectGroup, "cannot be removed since it is assocated with ", Resources.LocalizedResources.valRemoveText);
            FillSubjectGroupsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill subject names to respective subject group in grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjectGroups_DataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                int iSubjectGroupId = Convert.ToInt32(GrdSubjectGroup.DataKeys[e.Row.RowIndex]["Parent_Group_Id"]);
                SubjectGroupsBL oSubjectGroupBL = SetSubjectGroupBL(iSubjectGroupId);
                string sSubjects = oSubjectGroupBL.RetriveSubjectsForGroup();
                ((Label)e.Row.FindControl("txtSubjectNames")).Text = sSubjects;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort grid columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdSubjectGroup_sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillSubjectGroupsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit subject group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Type t = e.CommandArgument.GetType();
        try
        {
            if (e.CommandName != "Sort")
            {
                Int32 iIndex = Convert.ToInt32(e.CommandArgument);
                int iParentGroupId = Convert.ToInt32(GrdSubjectGroup.DataKeys[iIndex]["Parent_Group_Id"].ToString());
                int iParentSubjectId = Convert.ToInt32(GrdSubjectGroup.DataKeys[iIndex]["parent_Subject_id"].ToString());

                switch (e.CommandName)
                {
                    case S_GRD_EDIT_GROUP_COMMAND:
                        string sURL = CreateEditURL(iParentSubjectId, iParentGroupId);
                        MasterPage oMasterPage = (MasterPage)this.Master;
                        oMasterPage.RedirectToNextPage(sURL);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort image for active sorted grid column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjectGroups_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select class and fill optional subjects.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tdSubjectGroups.Visible = btnAdd.Enabled = btnDelete.Visible = !(cmbClass.SelectedValue == Constants.S_ZERO);
            FillSubjectGroupsGrid();
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private methods

    #region configuration functions

    /// <summary>
    /// This method is used to check dependancie of standard-divisions with other school configurations. 
    /// </summary>
    private string CheckDependencies()
    {
        List<SubjectMasterBL> oSubjectGroup = new List<SubjectMasterBL>();
        for (int iRowIndex = 0; iRowIndex < GrdSubjectGroup.Rows.Count; iRowIndex++)
        {
            if (((CheckBox)GrdSubjectGroup.Rows[iRowIndex].FindControl("chkDelete")) != null && ((CheckBox)GrdSubjectGroup.Rows[iRowIndex].FindControl("chkDelete")).Checked)
                oSubjectGroup.Add(new SubjectMasterBL()
                        {
                            ParentGroupId = GrdSubjectGroup.DataKeys[iRowIndex]["Parent_Group_Id"].ToInt(),
                            SubjectName = GrdSubjectGroup.DataKeys[iRowIndex]["parent_Subject_Name"].ToString(),
                            ConfigurationAction = Constants.Action.Delete,
                        });
        }

        if (oSubjectGroup.Count > 0)
        {
            GenericReferenceList<SubjectMasterBL> objStdDivsRefereces = new GenericReferenceList<SubjectMasterBL>(oSubjectGroup, miAcademicYearId);
            return objStdDivsRefereces.CheckDependenciesForList("ParentGroupId", "SubjectName", "ConfigurationAction", Constants.ReferenceId.ClassWiseOptionalSubject, false);
        }
        return string.Empty;
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.SubjectGroups);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    #endregion

    /// <summary>
    /// This method is used to set client side java scripts to controls.
    /// </summary>
    private void SetClientSideScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnAdd, btnBack, btnDelete });
    }

    /// <summary>
    /// This method is used to visible or hide controls on page load as per configuration is 
    /// done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        btnAdd.Visible = false;
        btnDelete.Visible = false;
        divGridView.Visible = false;
        tblClassCombo.Visible = false;
    }

    /// <summary>
    /// This method initialises the fields and data members
    /// </summary>
    private void InitializeFields()
    {
        btnDelete.Attributes.Add("OnClick", "if(!(ConfirmDelete(" + GrdSubjectGroup.PageCount
                                                               + ",'" + Resources.LocalizedResources.AtLeastOneSubjectGroupShouldBeSelectedForDeletion
                                                               + "',this))){return false;}");
        ValidationSummary1.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidSortExpression.Value = GrdSubjectGroup.Columns[I_PARENT_GROUP_NAME_COLUMN_NO].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
        hidAreYouSureUWantTODeleteGroups.Value = Resources.LocalizedResources.AreYouSureUWantToDeleteGroups;
    }
    
    /// <summary>
    /// This function gets dataset for subject group 
    /// and binds it to the grid 
    /// </summary>
    private void FillSubjectGroupsGrid()
    {
        DataTable  oDT = SubjectGroupsCollectionBL.GetAllSubjectGroups(miSchoolId, miAcademicYearId, cmbClass.SelectedValue.ToInt());
        
        DataView oDataView = oDT.DefaultView;
        oDataView.Sort = hidSortExpression.Value +" " + hidSortDirection.Value;
        GrdSubjectGroup.DataSource = oDataView;

        GrdSubjectGroup.DataBind();
        if (GrdSubjectGroup.Rows.Count == 0)
        {
            divGridView.Attributes.Add("class", " ");
            divGridView.Style.Add(HtmlTextWriterStyle.Height, "35pt");
        }
        else
        {
            divGridView.Attributes.Add("class", "GridBorder ClsGridBG");
            divGridView.Style.Add(HtmlTextWriterStyle.Height, "205pt");
        }

        btnDelete.Visible = GrdSubjectGroup.Rows.Count > 0;
    }

    /// <summary>
    /// This function is used to set sort variables
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    
    /// <summary>
    /// This function sets properties of subject group object
    /// </summary>
    /// <param name="aiParentGroupId"></param>
    /// <returns> object of SubjectGroupsBL </returns>
    public SubjectGroupsBL SetSubjectGroupBL(int aiParentGroupId)
    {
        SubjectGroupsBL oSubjectGroupsBL = new SubjectGroupsBL();
        oSubjectGroupsBL.academicyearId = miAcademicYearId;
        oSubjectGroupsBL.SchoolId = miSchoolId;
        oSubjectGroupsBL.ParentGroupId = aiParentGroupId;
        return oSubjectGroupsBL;
    }

    #endregion

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string ReadQuerystring()
    {
        try
        {
            hidIsConfigured.Value = QueryString["Is_Configured"];
            if (QueryString["StandardDivisionId"] != null)
            {
                cmbClass.SelectedValue = QueryString["StandardDivisionId"];
                FillSubjectGroupsGrid();
                btnAdd.Enabled = tdSubjectGroups.Visible = (GrdSubjectGroup.Rows.Count > 0 && !tdSubjectGroups.Visible) || cmbClass.SelectedValue != Constants.S_ZERO;
            }
        }
        catch
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }

        return QueryString["Is_Configured"];
    }

    /// <summary>
    /// This function creates URL to edit subject group
    /// the URL should contain required details of group
    /// </summary>
    /// <param name="iParentGroupId"></param>
    /// <param name="sParentGroupName"></param>
    /// <returns></returns>

    private string CreateEditURL(int aiParentSubjectId, int aiParentGroupId)
    {
        string sIsConfig = ReadQuerystring();
        string sQueryString = "ParentSubjectId= " + aiParentSubjectId.ToString() + "&Is_Configured=" + sIsConfig;
        sQueryString = sQueryString + "&ParentGroupId= " + aiParentGroupId.ToString() + "&ClassName=" + cmbClass.SelectedItem.Text + "&StandardDivisionId=" + cmbClass.SelectedValue;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        string sURL = "~/Admin/SubjectGroupUI.aspx" + "?" + sEncrypt;
        return sURL;
    }

    /// <summary>
    /// This method is used to fill standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        ClasswiseOptionalSubjectBL oClasswiseOptionalSubjectBL = new ClasswiseOptionalSubjectBL(miSchoolId, miAcademicYearId);
        List<StandardMaster> lstStandard = StandardCollectionBL.GetAll(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstStandard, cmbClass, "StandardName", "StandardId", Constants.S_SELECT);        
    }

    
      
     /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        btnDelete.Attributes.Add("OnClick", "if(!(ConfirmDelete(" + GrdSubjectGroup.PageCount
                                                             + ",'" + Resources.LocalizedResources.AtLeastOneSubjectGroupShouldBeSelectedForDeletion
                                                             + "',this))){return false;}");
        hidAreYouSureUWantTODeleteGroups.Value = Resources.LocalizedResources.AreYouSureUWantToDeleteGroups;
        FillSubjectGroupsGrid();
       
    }
}
