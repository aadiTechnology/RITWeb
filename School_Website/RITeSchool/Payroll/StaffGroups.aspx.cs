/* File Name :- StaffGroups.aspx.cs
 * Created By :- Sachin
 * Created Date :- 23-Oct-2009
 * Class Description :- This class is used to configure staff groups. 
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class StaffGroups : SchoolBase
{
    #region Constants

    private const string S_DATAKEY_STAFF_GROUPS_ID = "StaffGroupsId";
    private const string S_DATAKEY_ORIGINAL_STAFF_GROUPS_ID = "OriginalStaffGroupsId";
    private const string S_DATAKEY_SCHOOL_ID = "SchoolId";
    private const string S_SELECT_CHECKBOX = "ChkSelect";
    private const string S_STAFF_GROUP_TEXTBOX = "txtStaffGroup";

    #endregion

    #region Member(s)

    public int miSaveCount = 0; 

    #endregion

    #region Events

    /// <summary>
    /// This event is used to display available staff groups and set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                FillStaffGroupsGrid();
                hidSaveCount.Value = Convert.ToString(miSaveCount);
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to select configured staff groups.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffGroups_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                
                // If the school id is not the default id i.e. -9999 that means the staff group is already assigned
                // to the school. Thus check the checkbox.
                CheckBox chkSelect = ((CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX));
                if (lstvwStaffGroups.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    chkSelect.Checked = true;
                    miSaveCount = miSaveCount + 1;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save selected staff groups as well as add entry into configuration table if it is not configured.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            SetErrorLabel(string.Empty);

            if (QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StaffGroups));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            //Display RI check message and reset changes.            
            SetErrorLabel(ex.Message);
            FillStaffGroupsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method

    /// <summary>
    /// This method is used to set error label state.
    /// </summary>
    /// <param name="asMessage"></param>
    private void SetErrorLabel(string asMessage)
    {
        trErrorMessage.Visible = asMessage != string.Empty; ;
        lblErrorMessage.Text = asMessage;
    }

    /// <summary>
    /// This method is used to fill staff groups into gridview.
    /// </summary>
    private void FillStaffGroupsGrid()
    {   
        DataTable oDTStaffGroups = StaffGroupsBL.GetAll(miSchoolId);
        lstvwStaffGroups.DataSource = oDTStaffGroups;
        lstvwStaffGroups.DataBind();

        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStaffGroups.FindControl("trHeader");
        CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkSelectAll");
        oCheckBox.Focus();
    }

    protected void Save()
    {
        bool bIsActionNeeded;
        CheckBox chkSelect;
        List<StaffGroupsEntity> lstStaffGroups = new List<StaffGroupsEntity>();
        
        for (int iItemIndex = 0; iItemIndex < lstvwStaffGroups.Items.Count; iItemIndex++)
        {
            bIsActionNeeded = true;
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStaffGroups.Items[iItemIndex];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            StaffGroupsEntity oStaffGroupsEntity = PopulateBL();

            TextBox txtStaffGroup = (TextBox)oCurrentItem.FindControl(S_STAFF_GROUP_TEXTBOX);            
            oStaffGroupsEntity.StaffGroupsName = txtStaffGroup.Text.Trim();

            // Check if new group is being inserted.
            // I.e. If the checkbox is checked and the school id is -9999 then it is the new group being
            // introduced.
            chkSelect = (CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX);
            if (chkSelect.Checked == true && lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
            {   
                oStaffGroupsEntity.OriginalStaffGroupsId = Convert.ToInt32(lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_STAFF_GROUPS_ID]);
                oStaffGroupsEntity.Action = Constants.Action.Insert;             
            }

            // Check if existing group is being updated.
            // I.e. If the checkbox is checked and the school is not -9999 then update the existing group name.
            else if (chkSelect.Checked == true &&
                    lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {   
                oStaffGroupsEntity.OriginalStaffGroupsId = Convert.ToInt32(lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_ORIGINAL_STAFF_GROUPS_ID]);
                oStaffGroupsEntity.Action = Constants.Action.Update;
                oStaffGroupsEntity.StaffGroupsId = Convert.ToInt32(lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_STAFF_GROUPS_ID].ToString());                
            }

            // Check if existing group is being removed.
            // I.e. If the checkbox is NOT checked and the school id is not -9999. 
            // In such case need to check if any of the related data is entered for the unchecked group then
            // should be given to user.
            else if (chkSelect.Checked == false && lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {   
                oStaffGroupsEntity.Action = Constants.Action.Delete;                
                oStaffGroupsEntity.StaffGroupsId = Convert.ToInt32(lstvwStaffGroups.DataKeys[iItemIndex][S_DATAKEY_STAFF_GROUPS_ID]);                
            }
            else
                bIsActionNeeded = false;

            if (bIsActionNeeded)
                lstStaffGroups.Add(oStaffGroupsEntity);
        }

        // Update database with the configured staff group.
        if (lstStaffGroups.Count > 0)
        {
            StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
            oStaffGroupsBL.Update(lstStaffGroups, miAcademicYearId);
        }
    }

    /// <summary>
    /// This method is used to set javascript attributes and postback url to cancel button.
    /// </summary>
    public void SetJavascriptAttributes()
    {
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        valSummStaffGroups.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        BtnSave.Attributes.Add("onclick", "if(!CheckSelectedGroups(this)) return false;");
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnCancel });
    }

    /// <summary>
    /// This method is used to reurn populated object.
    /// </summary>
    /// <returns></returns>
    public StaffGroupsEntity PopulateBL()
    {
        return new StaffGroupsEntity
        {    
            SchoolId = miSchoolId,
            InsertedById = miUserId,
            UpdatedById = miUserId
        };        
    }

    #endregion
}
