using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class RITeSchool_Payroll_Shifts : SchoolBase
{

    #region "CONSTANTS"
    const string S_COMMAND_REMOVE = "REMOVESHIFT";
    const string S_COMMAND_UPDATE = "UPDATESHIFT";
    const string S_DEFAULT_SORT_EXP = "Name";
    const string S_EDIT_MODE = "EDIT";
    const string S_MODE_NEW = "NEW";
    private bool IsSortImage = false;

    #endregion

    #region "Events"
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillExistingShiftListview();
                SetJavascriptAttributes();
                lblErrorMsg.Visible = false;
            }
            lblErrorMsg.Visible = false;
            btnSave.Text = "Add";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the ListView of ShiftName by Name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureShift_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfigureShift_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                bool bIsDefault = Convert.ToBoolean(lstvwConfigureShift.DataKeys[oCurrentItem.DisplayIndex]["IsDefault"]);
                if (bIsDefault)
                    oimgbtnDelete.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfigureShift_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iShiftId = Convert.ToInt32(lstvwConfigureShift.DataKeys[iListIndex]["ShiftId"]);
                string sShiftName = lstvwConfigureShift.DataKeys[iListIndex]["ShiftName"].ToString();              

                hidShiftId.Value = iShiftId.ToString();
                hidShiftName.Value = sShiftName;
                if (e.CommandName == S_COMMAND_REMOVE)
                     DeleteShiftDetails(iShiftId);
                if (e.CommandName == S_COMMAND_UPDATE)
                    FillControlForShiftDetailsUpdate(iShiftId);
                lblErrorMsg.Visible = false;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfigureShift_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwConfigureShift.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwConfigureShift, DtPgCount);
            if (IsPostBack)
            {
                if (!IsSortImage)
                    AddSortImage();
            }
            if (!IsPostBack)
            {
                DtPgCount.Visible = false;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveSchoolShiftDetails();
            if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ShiftConfiguration));
            FillExistingShiftListview();
        }
        catch (DuplicateEntityException Ex)
        {
            lblErrorMsg.Visible = true;
            AddSortImage();
            lblErrorMsg.Text = Ex.ErrorMessage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            txtShiftName.Focus();
            lblErrorMsg.Visible = false;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"
    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        AddSortImage();
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = SortDirection.Ascending.ToString();
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwConfigureShift.SortDirection.ToString() == "Ascending" || lstvwConfigureShift.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwConfigureShift.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwConfigureShift.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        HtmlTableRow oHtmlTableHeaderRow = lstvwConfigureShift.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
        {
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
            IsSortImage = true;
        }
    }

    /// <summary>
    /// This method is used to set JavaScript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack });
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
    }

    /// <summary>
    /// This method is used to save ShiftName.
    /// </summary>
    private void SaveSchoolShiftDetails()
    {
        ShiftDetailsBL oShiftDetailsBL = CreateShiftMasterObject();
        if (oShiftDetailsBL.IsDuplicateShift())
        {
            if (hidMode.Value != S_EDIT_MODE)
            {
                oShiftDetailsBL.InsertShiftDetails("Insert");
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = "Shift details are saved successfully!!!";
            }
            else
            {
                oShiftDetailsBL.InsertShiftDetails("Update");
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = "Shift details are updated successfully!!!";
            }
        }
        ClearFields();
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtShiftName.Text = string.Empty;
        txtShiftName.Focus();
        txtShiftStartTime.Text = string.Empty;
        txtShiftEndTime.Text = string.Empty;
        txtHalfDayTime.Text = string.Empty;
        txtLateMarkTime.Text = string.Empty;
        chkIsDefault.Checked = false;
        hidMode.Value = S_MODE_NEW;
    }

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to read the Values for ShiftMasterBL properties.
    /// </summary>
    private ShiftDetailsBL CreateShiftMasterObject()
    {
        ShiftDetailsBL oShiftDetailsBL = new ShiftDetailsBL();
        oShiftDetailsBL.ShiftId = 0;
        oShiftDetailsBL.ShiftName = txtShiftName.Text.Trim();
        oShiftDetailsBL.SchoolId = miSchoolId;
        oShiftDetailsBL.ShiftStartTime = txtShiftStartTime.Text;
        oShiftDetailsBL.ShiftEndTime = txtShiftEndTime.Text;
        oShiftDetailsBL.HalfDayTime = txtHalfDayTime.Text;
        oShiftDetailsBL.LateMarkTime = txtLateMarkTime.Text;
        oShiftDetailsBL.AcademicYearId = miAcademicYearId;
        oShiftDetailsBL.InsertedById = miUserId;
        oShiftDetailsBL.IsDefault = chkIsDefault.Checked;

        if (hidMode.Value == S_EDIT_MODE)
            oShiftDetailsBL.ShiftId = Convert.ToInt32(hidShiftId.Value);
        return oShiftDetailsBL;
    }

    /// <summary>
    /// This method is used set datasource  to ListView
    /// </summary>
    private void FillExistingShiftListview()
    {
        lstvwConfigureShift.DataSourceID = ObjDSConfigureShift.ID;
        lstvwConfigureShift.DataBind();
    }

    /// <summary>
    /// This method is used to fill the controls to set it in Edit mode.
    /// </summary>
    /// <param name="iShiftId"></param>
    /// <param name="iSchoolId"></param>
    private void FillControlForShiftDetailsUpdate(int iShiftId)
    {
        ShiftDetailsBL oShiftDetailsBL = new ShiftDetailsBL(iShiftId, miSchoolId, miAcademicYearId);
        txtShiftName.Text = oShiftDetailsBL.ShiftName;
        txtShiftStartTime.Text = oShiftDetailsBL.ShiftStartTime;
        txtShiftEndTime.Text = oShiftDetailsBL.ShiftEndTime;
        txtHalfDayTime.Text = oShiftDetailsBL.HalfDayTime;
        txtLateMarkTime.Text = oShiftDetailsBL.LateMarkTime;
        chkIsDefault.Checked = oShiftDetailsBL.IsDefault;
        hidMode.Value = S_EDIT_MODE;
        btnSave.Text = "Update";
        if (chkIsDefault.Checked)
                chkIsDefault.InputAttributes["disabled"] = "disabled";
        else
            chkIsDefault.InputAttributes.Remove("disabled");

        //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:DisableCheckBox(); ", true);
    }

    /// <summary>
    /// This method is used to delete Shift Details.
    /// </summary>
    /// <param name="iShiftId"></param>
    /// <param name="iSchoolId"></param>
    private void DeleteShiftDetails(int iShiftId)
    {
        ShiftDetailsBL oShiftDetailsBL = new ShiftDetailsBL();
        int iCheckDependency = CheckDependencyForShift();
        oShiftDetailsBL.DeleteShiftDetails(iShiftId, miSchoolId, miAcademicYearId);
        DataTable oDT = ShiftDetailsBL.GetAll(miSchoolId, miAcademicYearId);
        if (oDT.Rows.Count == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ShiftConfiguration));
        FillExistingShiftListview();
        ClearFields();
    }

    /// <summary>
    /// This method is used to check is any user is associated with this shift.
    /// </summary>
    /// <returns></returns>
    private int CheckDependencyForShift()
    {
        ShiftDetailsBL oShiftDetailsBL = new ShiftDetailsBL();
        int iShiftId = Convert.ToInt32(hidShiftId.Value);
        return oShiftDetailsBL.CheckDependencyForShift(iShiftId, miSchoolId, miAcademicYearId);
    }
    #endregion
}