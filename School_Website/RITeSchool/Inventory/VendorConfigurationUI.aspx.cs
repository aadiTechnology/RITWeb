/* File Name :- VendorConfigurationUI.aspx.cs
 * Created Date :- 12-Jan-2018
 * Class Description :- This class is used to manage vendor configuration Details. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic;
using Utility;
using PayrollReportingUserEntities;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolEntities;
using System.Data;

public partial class VendorConfigurationUI : SchoolBase
{
    #region Constant(s)

    private const string S_DELETE_MESSAGE = "Vendor details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "Vendor details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "Vendor details saved successfully !!!";    
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";
    private const string S_SORT_ROW = "SortRow";

    #endregion

    #region DataMember

    private VendorDetailsBL moVendorDetailsBL;

    #endregion

    #region Event's

    /// <summary>
    /// Thoes event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty || hidSortDirection.Value == string.Empty)
            {
                hidSortExpression.Value = "VendorNo";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            base.AddSortImage(lstvwVendorDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Thoes event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVendorDetailsBL = new VendorDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillSalutationComboBox();
                FillBankDetails();
                FillVendorDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Thoes event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            VendorDetails oVendorDetails = Populate();
            moVendorDetailsBL.Save(oVendorDetails);

            if (hidVendorId.Value == Constants.S_ZERO)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
            ClearFields();
            btnSave.Text = S_SAVE_TEXT;
            FillVendorDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Bound Data in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVendorDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to perform the command operation in the  listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVendorDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iVendorId = Convert.ToInt32(lstvwVendorDetails.DataKeys[e.Item.DisplayIndex]["VendorId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    VendorDetails oVendorDetails = moVendorDetailsBL.Get(iVendorId);
                    hidVendorId.Value = oVendorDetails.VendorId.ToString();
                    trVendorNo.Visible = true;
                    txtVendorNo.Text = oVendorDetails.VendorNo.ToString();
                    cmbSalutation.SelectedValue = oVendorDetails.SalutationId.ToString();
                    txtFirstName.Text = oVendorDetails.FirstName;
                    txtMiddleName.Text = oVendorDetails.MiddleName;
                    txtLastName.Text = oVendorDetails.LastName;
                    txtCompanyName.Text = oVendorDetails.CompanyName;
                    txtAddress.Text = oVendorDetails.VendorAddress;
                    txtPinCode.Text = oVendorDetails.Pincode;
                    txtPhoneNo.Text = oVendorDetails.PhNumber;
                    txtMobileNo.Text = oVendorDetails.MobileNo;
                    txtFax.Text = oVendorDetails.FaxNo;
                    txtEmail.Text = oVendorDetails.EmailId;
                    txtGSTNo.Text = oVendorDetails.GSTNo;
                    txtPanNo.Text = oVendorDetails.PANNo;
                    txtAccountHolder.Text = oVendorDetails.AccountHolderName;
                    txtAccountNumber.Text = oVendorDetails.AccountNumber;
                    txtBranchName.Text = oVendorDetails.BranchName;
                    txtIFSCCode.Text = oVendorDetails.IFSCCode;
                    cmbBank.SelectedValue = oVendorDetails.BankId.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "ShowPopup();", true);                    
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moVendorDetailsBL.Delete(iVendorId);
                    FillVendorDetails();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    if (Convert.ToInt32(hidVendorId.Value) == iVendorId)
                        ClearFields();
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {                
                base.RevertSortOrder(hidSortDirection);                
                hidSortExpression.Value = "VendorNo";
                FillVendorDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound data for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVendorDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVendorDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwVendorDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sorting data in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVendorDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillVendorDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used to Deleting Item From the listview.
    // </summary>
    protected void lstvwVendorDetails_ItemEditing(object sender, ListViewEditEventArgs e) {    }

    // <summary>
    // This event is used to Editing Item From the listview.
    // </summary>
    protected void lstvwVendorDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e) {    }   

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVendorDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to fill salutation combo box.
    /// </summary>
    private void FillSalutationComboBox()
    {
        var oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    /// <summary>
    /// This method is used to Populate Vendir Details.
    /// </summary>
    public VendorDetails Populate()
    {
        VendorDetails oVendorDetails = new VendorDetails();
        oVendorDetails.VendorId = hidVendorId.Value.ToInt();
        oVendorDetails.SalutationId = cmbSalutation.SelectedValue.ToInt();
        oVendorDetails.FirstName = txtFirstName.Text.Trim();
        oVendorDetails.MiddleName = txtMiddleName.Text.Trim();
        oVendorDetails.LastName = txtLastName.Text.Trim();
        oVendorDetails.CompanyName = txtCompanyName.Text.Trim();
        oVendorDetails.VendorAddress = txtAddress.Text.Trim();
        oVendorDetails.Pincode = txtPinCode.Text.Trim();
        oVendorDetails.PhNumber = txtPhoneNo.Text.Trim();
        oVendorDetails.MobileNo = txtMobileNo.Text.Trim();
        oVendorDetails.FaxNo = txtFax.Text.Trim();
        oVendorDetails.EmailId = txtEmail.Text.Trim();
        oVendorDetails.GSTNo = txtGSTNo.Text.Trim();
        oVendorDetails.PANNo = txtPanNo.Text.Trim();
        oVendorDetails.AccountHolderName = txtAccountHolder.Text.Trim();
        oVendorDetails.AccountNumber = txtAccountNumber.Text;
        oVendorDetails.IFSCCode = txtIFSCCode.Text.Trim();
        oVendorDetails.BranchName = txtBranchName.Text.Trim();
        oVendorDetails.BankId = cmbBank.SelectedValue.ToInt();
        return oVendorDetails;
    }

    /// <summary>
    /// This method is used to Clear all fields.
    /// </summary>
    private void ClearFields()
    {
        trVendorNo.Visible = false;
        hidVendorId.Value = Constants.S_ZERO;
        btnSave.Text = S_SAVE_TEXT;
        cmbSalutation.SelectedValue = Constants.S_ONE;
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtCompanyName.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtPinCode.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtFax.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtGSTNo.Text = string.Empty;
        txtPanNo.Text = string.Empty;
        txtAccountHolder.Text = string.Empty;
        txtAccountNumber.Text = string.Empty;
        txtBranchName.Text = string.Empty;
        txtIFSCCode.Text = string.Empty;
        cmbBank.ClearSelection();
    }

    /// <summary>
    /// This method is used to fill the vendor details list view.
    /// </summary>
    private void FillVendorDetails()
    {
        lstvwVendorDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This method is used to set default java script attributes to control.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack, btnAdd});
        valSumError.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidEmailValidation.Value = Resources.LocalizedResources.EmailValidation;
        btnSave.Attributes.Add("onclick", "ShowValidationPopup()");
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Inventory));
    }

    /// <summary>
    /// This method is used to fill bank dropdownlist.
    /// </summary>
    private void FillBankDetails()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dt = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        ListSource.FillDropDownList(dt, cmbBank, "Bank_Name", "Schoolwise_Bank_Id", Constants.S_SELECT);
    }

    #endregion
}