/* -------------------------------------------------------------------------------------------------------
 *	Filename	: ApprovalConfigUI.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 7-Oct-2011
 *	Description	: This is the code behind class for Approval Configuration screen in the Accounts module.
 *				  It is used to manage approval configuration used in the Accounts module (Vouchers).
 * ------------------
 *  MODIFICATION LOG
 * ------------------
 *  Author		: Vishal B. Shah
 *  Date		: 6-Jan-2012
 *  Purpose		: Added another grid which displays existing configurations, with edit & delete options.
 * -------------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolBusinessService;
using Utility;

public partial class ApprovalConfigUI : SchoolBase
{

    #region -- CONSTANT(s) --

    private const string S_EDIT_ROW = "EDIT_ROW";
    private const string S_DELETE_ROW = "DELETE_ROW";

    private const string S_UPDATE_MESSAGE = "Approval configuration saved successfully!!!";
    private const string S_UPDATE_ERROR_MESSAGE = "Failed to update approval configuration.";
    private const string S_DELETE_MESSAGE = "Approval configuration deleted successfully!!!";

    #endregion -- CONSTANT(s) --

    #region -- MEMBER(s) --

    private AccountVoucherClient moAccountVoucherClient;
    private AccountApprovalConfigClient moAccountApprovalConfigClient;

    #endregion -- MEMBER(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    ///		This event is handled to set member varialbes from session	 and bind controls on first page request.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                InitVoucherServiceObj();
                InitApprovalConfigServiceObj();
                FillVoucherTypesList();
                FillCreatorDesinationList();
                FillApprovalConfig();
                ShowHideControls();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            CloseVoucherServiceObj();
            CloseApprovalConfigServiceObj();
        }
    }

    /// <summary>
    ///		This event is used to save the chosen details approval config details to the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            InitVoucherServiceObj();
            InitApprovalConfigServiceObj();

            var oApprovalConfig = new ApprovalConfig
                                    {
                                        CreatorDesignation = new DesignationMaster { DesignationId = ddlCreatorDesignationList.SelectedValue.ToInt() },
                                        VoucherType = new VoucherType { Id = ddlVoucherTypes.SelectedValue.ToInt() },
                                        ApprovalConfigDetails = PopulateApprovalConfigDetails(),
                                        SchoolId = miSchoolId,
                                        AcademicYearId = miAcademicYearId,
                                        FinancialYearId = miFinancialYearId,
                                        InsertedById = miUserId
                                    };

            string sMessage = moAccountApprovalConfigClient.SaveApprovalConfiguration(oApprovalConfig);
            if (String.IsNullOrEmpty(sMessage))
            {
                ShowUpdateMessage(S_UPDATE_MESSAGE);

                if (!IsConfigured())
                    SetConfiguration(true);

                FillApprovalConfig();
                FillApproverDesignationList();

                ddlVoucherTypes.SelectedIndex = Constants.VoucherType.Payment.ToInt();
                ddlCreatorDesignationList.SelectedIndex = 0;
            }
            else
                ShowErrorMessage(sMessage);
            ShowHideControls();
        }
        catch (Exception ex)
        {
            ShowErrorMessage(S_UPDATE_ERROR_MESSAGE);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            CloseVoucherServiceObj();
            CloseApprovalConfigServiceObj();
        }
    }

    /// <summary>
    ///		This event is used to handle if we change selected index
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        ddlCreatorDesignationList_SelectedIndexChanged(null, null);

    }

    /// <summary>
    ///		This event is handled to rebind the ListView based on dropdown list selections.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCreatorDesignationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            InitApprovalConfigServiceObj();
            FillApproverDesignationList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            CloseApprovalConfigServiceObj();
        }
    }

    /// <summary>
    ///		This event is handled to set alternating row color & set properties of controls in ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwApprovalConfig_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;

                // Set a diff class for alternate rows
                if (oCurrentItem.DisplayIndex % 2 == 1)
                {
                    var oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
                    if (oHTMLCurrentRow != null)
                        oHTMLCurrentRow.Attributes.Add("class", "ClsGridAltRow");
                }

                var omdtStar = oCurrentItem.FindControl("mdtStar") as HtmlControl;

                int iApprovalOrder = lstvwApprovalConfig.DataKeys[oCurrentItem.DisplayIndex]["ApprovalOrder"].ToInt();
                if (iApprovalOrder > 0)
                {
                    var chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                    if (chkSelect != null)
                        chkSelect.Checked = true;

                    omdtStar.Style["visibility"] = "visible";
                }
                else
                {
                    var chkFinalApprover = oCurrentItem.FindControl("chkFinalApprover") as CheckBox;
                    if (chkFinalApprover != null)
                        chkFinalApprover.InputAttributes.Add("disabled", "disabled");

                    omdtStar.Style["visibility"] = "hidden";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		This event is handled to populate the Approval Order dropdown list in the ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwApprovalConfig_DataBound(object sender, EventArgs e)
    {
        try
        {         

            if (lstvwApprovalConfig.Items.Count > 0)
            {
                var lstItemArr = new ListItem[lstvwApprovalConfig.Items.Count];
                for (int i = 0; i < lstvwApprovalConfig.Items.Count; i++)
                    lstItemArr[i] = new ListItem((i + 1).ToString());
                
                foreach (ListViewDataItem item in lstvwApprovalConfig.Items)
                {
                    var chkSelect = item.FindControl("chkSelect") as CheckBox;
                    var ddlApprovalOrder = item.FindControl("ddlApprovalOrder") as DropDownList;

                    if (ddlApprovalOrder == null)
                        continue;
                    
                    ddlApprovalOrder.Enabled = chkSelect.Checked;
                    ddlApprovalOrder.DataSource = lstItemArr;
                    ddlApprovalOrder.DataBind();
                    ddlApprovalOrder.SelectedValue = lstvwApprovalConfig.DataKeys[item.DisplayIndex]["ApprovalOrder"].ToString();
                }
            }
            HtmlTableRow oHtmlTableHeaderRow = lstvwApprovalConfig.FindControl("trHeader") as HtmlTableRow;
            CheckBox chkAll = (CheckBox)oHtmlTableHeaderRow.FindControl("chkSelectAll");
            chkAll.Checked = false;                
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		This event is used to modify the visibility of the Main Approval config input list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguredApprovalChain_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwConfiguredApprovalChain.Items.Count == 0)
            {
                tdlstvwConfiguredApprovalChain.Visible = false;
                tdlstvwApprovalConfig.ColSpan = 2;
                tdlstvwApprovalConfig.Width = "100%";
            }
            else
            {
                tdlstvwConfiguredApprovalChain.Visible = true;
                tdlstvwApprovalConfig.ColSpan = 1;
                tdlstvwApprovalConfig.Width = "50%";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		This event is handled to set a different class for alternating rows.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguredApprovalChain_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;

                // Set a diff class for alternate rows
                if (oCurrentItem.DisplayIndex % 2 == 1)
                {
                    var oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
                    if (oHTMLCurrentRow != null)
                        oHTMLCurrentRow.Attributes.Add("class", "ClsGridAltRow");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    ///		This event is used to handle update & delete commands raised from the Existing configurations grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguredApprovalChain_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            var oCurrentItem = e.Item as ListViewDataItem;
            var oVoucherType = lstvwConfiguredApprovalChain.DataKeys[oCurrentItem.DisplayIndex]["VoucherType"] as VoucherType;
            var oCreatorDesignation = lstvwConfiguredApprovalChain.DataKeys[oCurrentItem.DisplayIndex]["CreatorDesignation"] as DesignationMaster;

            switch (e.CommandName)
            {
                case S_EDIT_ROW:
                    ddlVoucherTypes.SelectedValue = oVoucherType.Id.ToString();
                    ddlCreatorDesignationList.SelectedValue = oCreatorDesignation.DesignationId.ToString();
                    ddlCreatorDesignationList_SelectedIndexChanged(null, null);
                    break;
                case S_DELETE_ROW:
                    var oApprovalConfig = new ApprovalConfig
                                            {
                                                Id = lstvwConfiguredApprovalChain.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToInt(),
                                                VoucherType = oVoucherType,
                                                CreatorDesignation = oCreatorDesignation,
                                                SchoolId = miSchoolId,
                                                AcademicYearId = miAcademicYearId,
                                                FinancialYearId = miFinancialYearId,
                                                UpdatedById = miUserId
                                            };

                    InitApprovalConfigServiceObj();
                    string sMessage = moAccountApprovalConfigClient.DeleteApprovalConfiguration(oApprovalConfig);
                    if (sMessage.IsNullOrEmpty())
                    {
                        ShowUpdateMessage(S_DELETE_MESSAGE);

                        if (lstvwConfiguredApprovalChain.Items.Count == 1 && IsConfigured())
                            SetConfiguration(false);

                        FillApprovalConfig();

                        // We only refresh the main input list, if the deleted config is the same as the one displayed on it.
                        if (ddlVoucherTypes.SelectedValue == oVoucherType.Id.ToString() && ddlCreatorDesignationList.SelectedValue == oCreatorDesignation.DesignationId.ToString())
                        {
                            FillApproverDesignationList();
                        }
                        ddlVoucherTypes.SelectedIndex = Constants.VoucherType.Payment.ToInt();
                        ddlCreatorDesignationList.SelectedIndex = 0;
                        ddlCreatorDesignationList_SelectedIndexChanged(null,null);
                    }
                    else
                        ShowErrorMessage(sMessage);
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            CloseApprovalConfigServiceObj();
        }
    }

    /// <summary>
    ///		This event is used to reset controls on the Page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            InitApprovalConfigServiceObj();

            FillApprovalConfig();
            FillApproverDesignationList();

            ddlVoucherTypes.SelectedIndex = Constants.VoucherType.Payment.ToInt();
            ddlCreatorDesignationList.SelectedIndex = 0;
            ShowHideControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            CloseApprovalConfigServiceObj();
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    ///		This function is used to popoulate the Voucher Types dropdown list.
    /// </summary>
    private void FillVoucherTypesList()
    {
        // Set default selected index to Payments
        ddlVoucherTypes.SelectedIndex = Constants.VoucherType.Payment.ToInt();

        List<VoucherType> lstVoucherTypes = moAccountVoucherClient.GetAllVoucherTypes(miSchoolId, miFinancialYearId, true);
        ListSource.FillDropDownList(lstVoucherTypes, ddlVoucherTypes, "Name", "Id", String.Empty);
    }

    /// <summary>
    ///		This function is used to populate the Creator Designation dropdown list.
    /// </summary>
    private void FillCreatorDesinationList()
    {   
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillDesignationCombobox(ref ddlCreatorDesignationList, Constants.UserRoles.None);
        trDivErr.Visible = ddlCreatorDesignationList.Items.Count == 0;
    }

    /// <summary>
    ///		Populates the grid which displays already configured approval chains.
    /// </summary>
    private void FillApprovalConfig()
    {
        lstvwConfiguredApprovalChain.DataSource = moAccountApprovalConfigClient.GetAllApprovalConfigurations(miSchoolId, miFinancialYearId);
        lstvwConfiguredApprovalChain.DataBind();
    }

    /// <summary>
    ///		Populates the grid with approval config chain details regarding the selected voucher type and creator.
    /// </summary>
    private void FillApproverDesignationList()
    {
        int iVoucherTypeId = ddlVoucherTypes.SelectedValue.ToInt();
        int iDesignationId = ddlCreatorDesignationList.SelectedValue.ToInt();

        ApprovalConfig oApprovalConfig = moAccountApprovalConfigClient.GetApprovalConfiguration(miSchoolId, miFinancialYearId, iDesignationId, iVoucherTypeId);

        lstvwApprovalConfig.DataSource = oApprovalConfig.ApprovalConfigDetails;
        lstvwApprovalConfig.DataBind();

    }

    /// <summary>
    ///		Hides controls on the page if No users are present in the system & informs the user about the same.
    /// </summary>
    private void ShowHideControls()
    {
        if (ddlCreatorDesignationList.Items.Count > 0)
        {
            FillApproverDesignationList();
            ddlCreatorDesignationList.Focus();
        }
        else
        {
            tblMain.Visible = false;
            trDivErr.Visible = true;
            btnSave.Visible = false;
        }
    }

    /// <summary>
    ///		This function is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        valsumErrorMessages.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Accounts_Related));
    }

    /// <summary>
    ///		This function is used to populate a List of ApprovalConfigDetails from the ListView on the page.
    /// </summary>
    /// <returns></returns>
    private List<ApprovalConfigDetail> PopulateApprovalConfigDetails()
    {
        return (from item in lstvwApprovalConfig.Items
                where item.ItemType == ListViewItemType.DataItem
                let chkSelect = item.FindControl("chkSelect") as CheckBox
                where chkSelect.Checked
                let iApproverDesignationId = (lstvwApprovalConfig.DataKeys[item.DisplayIndex]["ApproverDesignation"] as DesignationMaster).DesignationId
                let chkFinalApprover = item.FindControl("chkFinalApprover") as CheckBox
                let ddlApprovalOrder = item.FindControl("ddlApprovalOrder") as DropDownList
                select new ApprovalConfigDetail
                           {
                               ApproverDesignation = new DesignationMaster { DesignationId = iApproverDesignationId },
                               IsFinalApprover = chkFinalApprover.Checked,
                               ApprovalOrder = ddlApprovalOrder.SelectedValue.ToInt()
                           }).ToList();
    }

    /// <summary>
    ///		This function is used to show an update message on the Page.
    /// </summary>
    /// <param name="asMessage"></param>
    private void ShowUpdateMessage(string asMessage)
    {
        lblUpateMessage.Text = asMessage;
        lblUpateMessage.Visible = true;
        lblErrorMessage.Visible = false;
    }

    /// <summary>
    ///		This function is used to show an error message on the Page.
    /// </summary>
    /// <param name="asMessage"></param>
    private void ShowErrorMessage(string asMessage)
    {
        lblErrorMessage.Text = asMessage;
        lblErrorMessage.Visible = true;
        lblUpateMessage.Visible = false;
    }

    /// <summary>
    ///		This function is used to Decrypt the QueryString.
    /// </summary>
    /// <returns></returns>
    private bool IsConfigured()
    {
        return !QueryString[Constants.S_IS_CONFIGURED].IsNull() && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
    }

    /// <summary>
    ///		This function is used to Save/Delete the Configuration settings.
    /// </summary>
    /// <param name="abSave"></param>
    private void SetConfiguration(bool abSave)
    {
        if (abSave)
            SaveConfigDetails(Constants.SchoolConfigurations.ApprovalConfig.ToInt());
        else
            DeleteConfigDetails(Constants.SchoolConfigurations.ApprovalConfig.ToInt());
    }

    /// <summary>
    ///		Initializes the Voucher service object.
    /// </summary>
    private void InitVoucherServiceObj()
    {
        moAccountVoucherClient = new AccountVoucherClient();
        moAccountVoucherClient.Open();
    }

    /// <summary>
    ///		Disposes off the Voucher service object.
    /// </summary>
    private void CloseVoucherServiceObj()
    {
        if (moAccountVoucherClient != null)
            moAccountVoucherClient.Close();
    }

    /// <summary>
    ///		Initializes the ApprovalConfig service object.
    /// </summary>
    private void InitApprovalConfigServiceObj()
    {
        moAccountApprovalConfigClient = new AccountApprovalConfigClient();
        moAccountApprovalConfigClient.Open();
    }

    /// <summary>
    ///		Disposes off the ApprovalConfig service object.
    /// </summary>
    private void CloseApprovalConfigServiceObj()
    {
        if (moAccountApprovalConfigClient != null && moAccountApprovalConfigClient.State != CommunicationState.Faulted)
            moAccountApprovalConfigClient.Close();
    }

    #endregion -- PRIVATE METHOD(s) --

}