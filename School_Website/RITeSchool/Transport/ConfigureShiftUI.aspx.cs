using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Web;

public partial class ConfigureShiftUI : SchoolBase
{
    #region "CONSTANTS"
    const string S_COMMAND_REMOVE = "REMOVESHIFT";
    const string S_COMMAND_UPDATE = "UPDATESHIFT";
    const string S_DEFAULT_SORT_EXP = "Name";
    const string S_EDIT_MODE = "EDIT";
    const string S_MODE_NEW = "NEW";
    #endregion

    bool ShowJourney
    {
        get 
        {
            return moSchool == Constants.SchoolId.SNS;    
        }
    }


    #region "Events"
    /// <summary>
    /// This event is used to fill existing shift Names listView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                SetFields();
            }
            lblErrorMsg.Visible = false;
            btnSave.Text = "Add";
            lblCheckDependency.Visible = false;
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
    /// <summary>
    /// This event is used to view page wise Stop Name list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void ddlPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwConfigureShift);
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill footer property of existing Stop name listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureShift_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwConfigureShift.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwConfigureShift, DtPgCount);
            if (IsPostBack)
                AddSortImage();
            else
            {
                DtPgCount.Visible = false;
              
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is used to add attirbutes to existing ShiftName ListViews Item Control. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to Edit or Delete Shift Names.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureShift_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iTransportShiftId = Convert.ToInt32(lstvwConfigureShift.DataKeys[iListIndex]["TransportShiftId"]);
                string sTransportShiftName = lstvwConfigureShift.DataKeys[iListIndex]["TransportShiftName"].ToString();
                hidTransportShiftId.Value = iTransportShiftId.ToString();
                hidTransportShiftName.Value = sTransportShiftName;
                if (e.CommandName == S_COMMAND_REMOVE)
                    DeleteShiftMasterDetails(iTransportShiftId);
                else if (e.CommandName == S_COMMAND_UPDATE)
                    FillControlForShiftMasterUpdate(iTransportShiftId);
                lblErrorMsg.Visible = false;
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
     /// <summary>
    /// This method is used to Add Stop Names
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveShiftMasterDetails();
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
    /// <summary>
    /// This Event is used to cancel the saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    #region "Methods"
    /// <summary>
    /// This method is used to set JavaScript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnBack});
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted));
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
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }
    /// <summary>
    /// This method is used set datasource  to ListView
    /// </summary>
    /// 
    private void FillExistingShiftListview()
    {

        lstvwConfigureShift.DataSourceID = ObjDSConfigureShift.ID;
        lstvwConfigureShift.DataBind();
    }
    /// <summary>
    /// This method is used to read the Values for ShiftMasterBL properties.
    /// </summary>
    private ShiftMasterBL CreateShiftMasterObject()
    {
        ShiftMasterBL oShiftMasterBL = new ShiftMasterBL();
        oShiftMasterBL.TransportShiftId = 0;
        oShiftMasterBL.TransportShiftName = txtShiftName.Text.Trim();
        oShiftMasterBL.SchoolId = miSchoolId;

        oShiftMasterBL.Academic_Year_Id = miAcademicYearId;
        oShiftMasterBL.InsertDate = DateTime.Now;
        oShiftMasterBL.InsertedById = miUserId;
        oShiftMasterBL.UpdateDate = DateTime.Now;
        oShiftMasterBL.UpdatedById = miUserId;
        oShiftMasterBL.JourneyTypeId = cmbJourneyType.SelectedValue.ToInt();

        if (hidMode.Value == S_EDIT_MODE)
            oShiftMasterBL.TransportShiftId = Convert.ToInt32(hidTransportShiftId.Value);
        return oShiftMasterBL;
    }
    /// <summary>
    /// This method is used to save ShiftName.
    /// </summary>
   private void SaveShiftMasterDetails()
    {
        ShiftMasterBL oShiftMasterBL = CreateShiftMasterObject();
        if (oShiftMasterBL.IsNameDuplicateShift())
        {
            if (hidMode.Value != S_EDIT_MODE)
            {
                oShiftMasterBL.InsertShiftMaster();
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = ShowJourney? "Journey details saved successfully!!!" :  "Shift Name saved successfully!!!";
            }
            else
            {
                oShiftMasterBL.UpdateShiftMaster();
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = ShowJourney ? "Journey details updated successfully!!!" : "Shift Name updated successfully!!!";
            }
        }
       ClearFields();
   }
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
    /// This method is used to delete ShiftMaster Details.
    /// </summary>
    /// <param name="iTransportShiftId"></param>
    /// <param name="iSchoolId"></param>
    private  void DeleteShiftMasterDetails(int iTransportShiftId )
    {
        ShiftMasterBL oShiftMasterBL = new ShiftMasterBL();
        int iCheckDependency = CheckDependencyForShiftName();
        if (iCheckDependency == 0)
        {
            oShiftMasterBL.DeleteShiftMaster(iTransportShiftId, miSchoolId, miAcademicYearId);
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = ShowJourney ? "Journey details deleted successfully!!!" : "Shift Name deleted successfully!!!";
        }
        else
        {
            lblCheckDependency.Visible = true;

            if(ShowJourney)
                lblCheckDependency.Text = "Journey Name " + hidTransportShiftName.Value + " can not be deleted since associated with Route-Shift-Timing Details. ";
            else
                lblCheckDependency.Text = "Shift Name " + hidTransportShiftName.Value + " can not be deleted since associated with Route-Shift-Timing Details. ";
        }
        DataTable oDT = ShiftMasterBL.GetAll(miSchoolId, miAcademicYearId);
        if (oDT.Rows.Count == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ShiftConfiguration));
        FillExistingShiftListview();
        ClearFields();
    }
    /// <summary>
    /// This method is used to fill the controls to set it in Edit mode.
    /// </summary>
    /// <param name="iTransportShiftId"></param>
    /// <param name="iSchoolId"></param>
    private void  FillControlForShiftMasterUpdate(int iTransportShiftId)
    {
        AddSortImage();
        ShiftMasterBL oShiftMasterBL=new ShiftMasterBL(iTransportShiftId, miSchoolId,miAcademicYearId);
        txtShiftName.Text = oShiftMasterBL.TransportShiftName;
        cmbJourneyType.SelectedValue = oShiftMasterBL.JourneyTypeId.ToString();
        hidMode.Value = S_EDIT_MODE;
        btnSave.Text = "Update";
    }
    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private  void ClearFields()
    {
        txtShiftName.Text = string.Empty;
        cmbJourneyType.ClearSelection();
        txtShiftName.Focus();
        hidMode.Value = S_MODE_NEW;      
    }
    
    private int CheckDependencyForShiftName()
    {
        ShiftMasterBL oShiftMasterBL = new ShiftMasterBL();
        int iTransportShiftId = Convert.ToInt32(hidTransportShiftId.Value);
        return oShiftMasterBL.CheckDependencyForShiftName(iTransportShiftId, miSchoolId, miAcademicYearId);
    }

    private void SetFields()
    {
        if (ShowJourney)
        {
            spnShiftHeader.InnerText = "Journey Name : ";
            reqShiftName.ErrorMessage = "Journey Name should not be blank.";
            reqValJourneyType.Enabled = true;
            trJourneyType.Visible = true;
        }
    }

    #endregion
}
