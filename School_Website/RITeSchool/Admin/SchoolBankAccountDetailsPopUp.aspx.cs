using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using PayrollEntities;
using Utility;

public partial class SchoolBankAccountDetailsPopUp : SchoolBase
{
    #region "Data Members"
    const string S_DEFAULT_SORT_EXP = "Bank_Name";
    const string S_COMMAND_REMOVE = "RemoveAccount";
    const string S_COMMAND_UPDATE = "UpdateAccount";
    #endregion "Data Members"

    #region "Events"

    /// <summary>
    /// This event is used to fill bank combobox and to set javascript attributes for buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetDefaultControls();
                FillBankCombo();
                valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
                SetJavaScriptAttributres();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save bank account details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolwiseBankAccountDetailsBL oSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL();
            oSchoolwiseBankAccountDetailsBL.SchoolWiseBankAccountDetails = PopulateSchoolWiseBankAccountDetails();
            if (oSchoolwiseBankAccountDetailsBL.IsBankAccountDuplicateBL() == 0)
            {
                if (hidMode.Value != "Update")
                {
                    oSchoolwiseBankAccountDetailsBL.InsertSchoolwiseBankAccountDetailsBL();
                    lblUpdateSucess.Text = Resources.LocalizedResources.MsgBankRecordSavedSuccessfully;
                }
                else
                {
                    oSchoolwiseBankAccountDetailsBL.UpdateSchoolwiseBankAccountDetailsBL();
                    lblUpdateSucess.Text = Resources.LocalizedResources.MsgBankRecordUpdatedSuccessfully;
                }
            }
            else
                lblErrorMsg.Text = Resources.LocalizedResources.AccountNumber + txtAccountNo.Text + " " + Resources.LocalizedResources.AlredyExistForBank +
                                        ddlBankName.SelectedItem + Resources.LocalizedResources.ForBank + ".";
            FillSchoolBankAccountDetails();
            lblUpdateSucess.Visible = true;
            SetDefaultControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise bank account list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwBankAccount);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancle saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetDefaultControls();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Listview Events"
    /// <summary>
    /// This event is used to fill footer property and add sort image for existing bank account details listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBankAccount_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwBankAccount.Items.Count > 0)
            {
                lstvwBankAccount.Items.Clear();
                ControlUtility.FillListViewPagerFooter(lstvwBankAccount, DtPgCount);
                AddSortImage();
            }
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
    /// This event is used to add confirmation message while deleting existing bank account details record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBankAccount_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                int iBankAssociationCount = Convert.ToInt32(lstvwBankAccount.DataKeys[oCurrentItem.DataItemIndex]["BankAssociationCount"]);
                if (iBankAssociationCount > 0)
                {
                    ((ImageButton)e.Item.FindControl("imgBtnEdit")).Visible = false;
                    ((ImageButton)e.Item.FindControl("imgBtnDelete")).Visible = false;
                }
                else
                {
                    ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                    oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the listview of bank account details by Bank Name and Account No.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBankAccount_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit and delete the existing bank account details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBankAccount_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iSchoolWiseBankAccountDetailsId = Convert.ToInt32(lstvwBankAccount.DataKeys[iListIndex]["SchoolWiseBankAccountDetailsId"]);
                hidBankName.Value = lstvwBankAccount.DataKeys[iListIndex]["BankName"].ToString();
                hidAccountNo.Value = Convert.ToString(lstvwBankAccount.DataKeys[iListIndex]["AccountNo"]);
                hidSchoolWiseBankAccountDetailsId.Value = iSchoolWiseBankAccountDetailsId.ToString();
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    SetDefaultControls();
                    DeleteBankAccountDetails(iSchoolWiseBankAccountDetailsId);
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                    LoadBankAccountDetails(iSchoolWiseBankAccountDetailsId);
                FillSchoolBankAccountDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Listview Events"

    #region "private Methods"

    /// <summary>
    /// This method is used to fill bank combobox.
    /// </summary>
    private void FillBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        ControlUtility.FillDropDownList(dtBankList, ref ddlBankName, "Schoolwise_Bank_Id",
                                                                    "Bank_Name", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnSave.Attributes["onclick"] = "javascript:btnsaveonclick('" + btnSave.ClientID + "',this);";
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
    }

    /// <summary>
    /// This method is used to set default control values.
    /// </summary>
    private void SetDefaultControls()
    {
        ddlBankName.Focus();
        txtAccountNo.Text = string.Empty;
        ddlBankName.SelectedIndex = 0;
        hidMode.Value = "Save";
        btnSave.Text = Resources .LocalizedResources.Save;
    }

    /// <summary>
    /// This method is used to set datasource to existing bank account listView.
    /// </summary>
    private void FillSchoolBankAccountDetails()
    {
        lstvwBankAccount.DataSourceID = ObjDSSchoolBankAccountDetails.ID;
        lstvwBankAccount.DataBind();
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
    /// This method is used to populate object of SchoolWiseBankAccountDetails class.
    /// </summary>
    /// <returns></returns>
    private SchoolWiseBankAccountDetails PopulateSchoolWiseBankAccountDetails()
    {
        SchoolWiseBankAccountDetails oSchoolWiseBankAccountDetails = new SchoolWiseBankAccountDetails();
        if (hidMode.Value != "Update")
            oSchoolWiseBankAccountDetails.SchoolWiseBankAccountDetailsId = 0;
        else
            oSchoolWiseBankAccountDetails.SchoolWiseBankAccountDetailsId = Convert.ToInt32(hidSchoolWiseBankAccountDetailsId.Value);
        oSchoolWiseBankAccountDetails.BankName = Convert.ToString(ddlBankName.SelectedItem);
        oSchoolWiseBankAccountDetails.BankId = Convert.ToInt32(ddlBankName.SelectedValue);
        oSchoolWiseBankAccountDetails.AccountNo = txtAccountNo.Text;
        oSchoolWiseBankAccountDetails.SchoolId = miSchoolId;
        oSchoolWiseBankAccountDetails.InsertedById = miUserId;
        oSchoolWiseBankAccountDetails.InsertDate = System.DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
        oSchoolWiseBankAccountDetails.UpdateDate = System.DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
        oSchoolWiseBankAccountDetails.UpdatedById = miUserId;
        return oSchoolWiseBankAccountDetails;
    }

    /// <summary>
    /// This method is used to set controls to update bank account details.
    /// </summary>
    /// <param name="iSchoolWiseBankAccountDetailsId"></param>
    private void LoadBankAccountDetails(int iSchoolWiseBankAccountDetailsId)
    {
        SchoolwiseBankAccountDetailsBL oSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL(iSchoolWiseBankAccountDetailsId);
        txtAccountNo.Text = oSchoolwiseBankAccountDetailsBL.SchoolWiseBankAccountDetails.AccountNo;
        ddlBankName.SelectedValue = Convert.ToString(oSchoolwiseBankAccountDetailsBL.SchoolWiseBankAccountDetails.BankId);
        btnSave.Text =  Resources.LocalizedResources.Update;
        hidMode.Value = "Update";
    }

    /// <summary>
    /// This method is used to delete exisiting bank account details.
    /// </summary>
    /// <param name="iSchoolWiseBankAccountDetailsId"></param>
    private void DeleteBankAccountDetails(int iSchoolWiseBankAccountDetailsId)
    {
        SchoolwiseBankAccountDetailsBL oSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL();
        oSchoolwiseBankAccountDetailsBL.SchoolWiseBankAccountDetails.SchoolWiseBankAccountDetailsId = iSchoolWiseBankAccountDetailsId;
            oSchoolwiseBankAccountDetailsBL.DeleteSchoolwiseBankAccountDetailsBL();
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwBankAccount.SortDirection.ToString() == "Ascending" || lstvwBankAccount.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwBankAccount.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwBankAccount.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwBankAccount.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    private void RefreshValue()
    {
        hidAlertDeleterecord.Value = Resources.LocalizedResources.AlertDeleterecord;
    }

    #endregion "Private Method"
}