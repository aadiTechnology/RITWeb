/* Class Name - PODetailsUI
 * Created By - Vishakha
 * Created On - 13-Jun-2022
 * Description - This class is used to manage Purchase order details.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Web.Script.Serialization;
using System.Text;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Linq;
using PayrollReportingUserEntities;

public partial class PODetailsUI : SchoolBase
{
    #region Constant(s)

    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_SAVE_MSG = "External PO details saved successfully !!!";
    private const string S_UPDATE_MSG = "External PO details updated successfully !!!";
    private const string S_DELETE_MSG = "External PO details deleted successfully !!!";

    private const string S_SAVE_MSG_WO = "External WO details saved successfully !!!";
    private const string S_UPDATE_MSG_WO = "External WO details updated successfully !!!";
    private const string S_DELETE_MSG_WO = "External WO details deleted successfully !!!";
    
    private const string S_COMMAND_DELETE = "DeletePODetails";
    private const string S_COMMAND_UPDATE = "UpdatePODetails";
    private const string S_COMMAND_SENT_FOR_APPROVAL = "SendForApproval";
    private const string S_ACTION = "Action";
    

    #endregion

    #region Data Member(s)

    private PODetailsBL moPODetailsBL;
    POInstructionDetails moPOInstructionDetails;

    #endregion

    #region Event(s)
    /// <summary>
    /// THis event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "PONo";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwReceiverDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill GSt category, receiver details and PO details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPODetailsBL = new PODetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                CheckFullAccess();
                GetPrefixes();
                FillStatus();
                SetDefaultValues();                
                FillReceiverName();
                FillGSTCategory();
                FillDescriptions(0);
                FillExternalPODetails();
                FillInstructions();
                MovePageToBottom();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void GetPrefixes()
    {
        ExternalOrderPrefix oExternalOrderPrefix = moPODetailsBL.GetPrefixes();
        hidPOPrefix.Value = oExternalOrderPrefix.POPrefix;
        hidWoPrefix.Value = oExternalOrderPrefix.WOPrefix;
    }

    /// <summary>
    /// This event is used to edit, delete and sorting activity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwReceiverDetails.DataKeys[iRowId]["Id"]);

                if (e.CommandName == S_COMMAND_UPDATE)
                   SetControlsForEditMode(iId);
                else if (e.CommandName == S_COMMAND_DELETE)
                {
                   Delete(iId);
                   ResetFields();
                   ResetId();
                   FillDescriptions(0);
                   FillExternalPODetails();
                }
                else if (e.CommandName == S_COMMAND_SENT_FOR_APPROVAL)
                {
                    SendForApproval(iId);
                    FillDescriptions(0);
                    FillExternalPODetails();
                }
                else if (e.CommandName == S_ACTION)
                {
                    foreach (ListViewItem oItem in lstvwReceiverDetails.Items)
                    {
                        HtmlTableRow trCommentAll = oItem.FindControl("trComment") as HtmlTableRow;
                        if (trCommentAll != null)
                            trCommentAll.Visible = false;
                    }

                    HtmlTableRow trComment = e.Item.FindControl("trComment") as HtmlTableRow;
                    if (trComment != null)
                    {
                        trComment.Visible = true;
                        TextBox txtComment = e.Item.FindControl("txtComment") as TextBox;
                    }
                }
                else if (e.CommandName == "PAY")
                {
                    MasterPage oMaster = this.Master as MasterPage;
                    oMaster.RedirectToNextPage("ExternalPOPaymentDetailsUI.aspx?" + CommonUtility.EncryptQuerystring("POMasterId=" + iId+"&StatusId="+hidStatusId.Value+"&Filter="+hidFilter.Value));
                }
            }
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void SendForApproval(int aiId)
    {
        int iNotificationReceiverUserId  = moPODetailsBL.SendRequestForApproval(aiId);
        LblUpdateSuccess.Text = "Order successfully sent for approval.";
    }
    
    /// <summary>
    /// THis event is used to fill PO details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ExternalPODetails oExternalPODetails = e.Item.DataItem as ExternalPODetails;
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDelete") as ImageButton;
                ImageButton imgbtnEdit = e.Item.FindControl("imgbtnEdit") as ImageButton;

                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                Label lblPODate1 = e.Item.FindControl("lblPODate1") as Label;
                lblPODate1.Text = oExternalPODetails.PODate.ToString(Constants.S_DATE_FORMAT);

                ListView lstvwDescription = e.Item.FindControl("lstvwDescription") as ListView;
                if (lstvwDescription != null)
                {
                    lstvwDescription.DataSource = oExternalPODetails.Descriptions;
                    lstvwDescription.DataBind();
                }

                //It is to show inner listview.
                LinkButton LnkBtnDetails = e.Item.FindControl("LnkBtnDetails") as LinkButton;
                LnkBtnDetails.Attributes.Add("onclick", "ShowDescription(" + e.Item.DisplayIndex + "); return false;");

                int iId = Convert.ToInt32(lstvwReceiverDetails.DataKeys[e.Item.DisplayIndex]["Id"].ToString());
                string sQueryString = CommonUtility.EncryptQuerystring("POMasterId=" + iId );

                HiddenField hidData1 = e.Item.FindControl("hidData1") as HiddenField;
                hidData1.Value = sQueryString;

                LinkButton lnkExport = e.Item.FindControl("lnkExport") as LinkButton;
                lnkExport.Attributes.Add("onclick", "OpenReport(" + e.Item.DisplayIndex + "); return false;");

                ImageButton imgBtnSend = e.Item.FindControl("imgBtnSend") as ImageButton;
                
                HtmlTableCell tdAction = e.Item.FindControl("tdAction") as HtmlTableCell;
                HtmlTableCell tdEdit = e.Item.FindControl("tdEdit") as HtmlTableCell;
                HtmlTableCell tdDelete = e.Item.FindControl("tdDelete") as HtmlTableCell;
                HtmlTableCell tdSend = e.Item.FindControl("tdSend") as HtmlTableCell;

                LinkButton lnkBtnActions = e.Item.FindControl("lnkBtnActions") as LinkButton;
                if (lnkBtnActions != null)
                {
                    tdAction.Visible = true;
                    lnkBtnActions.Visible = true;
                }

                HtmlTableRow trComment = e.Item.FindControl("trComment") as HtmlTableRow;
                if (trComment != null)
                    trComment.Visible = false;

                TextBox txtComment = e.Item.FindControl("txtComment") as TextBox;
                if (txtComment != null)
                    txtComment.Text = oExternalPODetails.Comment;

                Button btnApprove = e.Item.FindControl("btnApprove") as Button;
                Button btnReject = e.Item.FindControl("btnReject") as Button;

                btnApprove.Attributes.Add("onclick", "if(!ValidateComment('" + txtComment.ClientID + "')) return false;");
                btnReject.Attributes.Add("onclick", "if(!ValidateComment('" + txtComment.ClientID + "')) return false;");

                HtmlTableCell tdPay = e.Item.FindControl("tdPay") as HtmlTableCell;

                if (ddlStatus.SelectedValue == "5")
                {
                    if (tdEdit != null)
                        tdEdit.Visible = true;

                    if (tdDelete != null)
                        tdDelete.Visible = true;

                    if (imgBtnSend != null)
                        imgBtnSend.Visible = true;

                    if (oExternalPODetails.StatusId == 1)
                    {
                        imgBtnSend.Visible = true;
                        imgbtnDelete.Visible = true;
                        imgbtnEdit.Visible = true;
                    }
                    else if (oExternalPODetails.StatusId == 2)
                    {
                        imgBtnSend.Visible = true;
                        imgbtnDelete.Visible = false;
                        imgbtnEdit.Visible = true;
                    }
                    else
                    {
                        imgbtnDelete.Visible = false;
                        imgbtnEdit.Visible = false;
                        imgBtnSend.Visible = false;
                    }

                    if (oExternalPODetails.StatusId == 2 || oExternalPODetails.StatusId == 3)
                    {
                        if (btnApprove != null)
                            btnApprove.Visible = false;

                        if (btnReject != null)
                            btnReject.Visible = false;
                    }
                    else
                    {                        
                        if (lnkBtnActions != null)
                            lnkBtnActions.Visible = false;
                    }

                    Label lblStatus = e.Item.FindControl("lblStatus") as Label;
                    if (oExternalPODetails.StatusId == 4)
                        lblStatus.Text = "Waiting for Approval";

                    if (tdPay != null)
                        tdPay.Visible = false;
                    
                }
                else if (ddlStatus.SelectedValue == "6" || ddlStatus.SelectedValue == "3")
                {
                    if (tdEdit != null)
                        tdEdit.Visible = false;

                    if (tdDelete != null)
                        tdDelete.Visible = false;

                    if (tdSend != null)
                        tdSend.Visible = false;

                    if (tdPay != null)
                        tdPay.Visible = false;
                }
                else if (ddlStatus.SelectedValue == "4")
                {
                    if (tdEdit != null)
                        tdEdit.Visible = false;

                    if (tdDelete != null)
                        tdDelete.Visible = false;

                    if (tdSend != null)
                        tdSend.Visible = false;

                    if (tdPay != null)
                        tdPay.Visible = false;
                }
                else if (ddlStatus.SelectedValue == "2")
                {
                    if (tdEdit != null)
                    {
                        tdEdit.Visible = true;
                        imgbtnEdit.Visible = false;
                    }

                    if (tdDelete != null)
                        tdDelete.Visible = false;

                    if (tdSend != null)
                    {
                        tdSend.Visible = true;
                        imgBtnSend.Visible = false;
                    }

                    if (tdPay != null)
                        tdPay.Visible = false;
                }
                else if (ddlStatus.SelectedValue == "-9999")
                {                    
                    if (tdPay != null)
                        tdPay.Visible = true;

                    if (tdEdit != null)
                        tdEdit.Visible = false;

                    if (tdDelete != null)
                        tdDelete.Visible = false;

                    if (tdSend != null)
                        tdSend.Visible = false;

                    if(tdAction != null)
                        tdAction.Visible = false;
                }
                

                if (oExternalPODetails.StatusId == 2 || oExternalPODetails.StatusId == 3)
                {
                    if (btnApprove != null)
                        btnApprove.Visible = false;

                    if (btnReject != null)
                        btnReject.Visible = false;
                }

                LinkButton lnkBtnPay = e.Item.FindControl("lnkBtnPay") as LinkButton;
                
                if (oExternalPODetails.TotalPaidAmount  == 0)
                {
                    lnkBtnPay.ForeColor = System.Drawing.Color.Red;
                    lnkBtnPay.ToolTip = "Pending : " + (oExternalPODetails.GrandTotal - oExternalPODetails.TotalPaidAmount).ToString();
                }
                else if (oExternalPODetails.GrandTotal - oExternalPODetails.TotalPaidAmount > 0)
                {
                    lnkBtnPay.ForeColor = System.Drawing.Color.Navy;
                    lnkBtnPay.ToolTip = "Pending : "+(oExternalPODetails.GrandTotal - oExternalPODetails.TotalPaidAmount).ToString();
                    lnkBtnPay.Text = "Partially Paid";
                }
                else
                    lnkBtnPay.Text = "Paid";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillExternalPODetails();
            SetFilterState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// THis event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwReceiverDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwReceiverDetails, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReceiverDetails_Sorting(object sender, ListViewSortEventArgs e)
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

    protected void lstvwInstructions_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

                FillInstructions();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwInstructions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBoxList ChkInstructionList = e.Item.FindControl("ChkInstructionList") as CheckBoxList;
                int iCategoryId = lstvwInstructions.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();

                List<Instruction> lstInstruction = moPOInstructionDetails.Instructions.Where(inst => inst.InstCategoryId == iCategoryId).ToList();
                ListSource.FillCheckBoxList(lstInstruction, ChkInstructionList, "InstructionName", "Id");

                CheckBox chkAllInstructions = e.Item.FindControl("chkAllInstructions") as CheckBox;
                chkAllInstructions.Attributes.Add("onclick", "CheckUncheckRow(this," + e.Item.DisplayIndex + ")");

                ChkInstructionList.Attributes.Add("onclick", "CheckAllDependancy(" + e.Item.DisplayIndex + "," + chkAllInstructions.ClientID + ")");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwReceiverDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

   /// <summary>
   /// This event is used to save PO details.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.IsValid)
            {
                Save();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel PO details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            ResetId();
            FillExternalPODetails();
            btnSave.Text = S_TEXT_SAVE;
            LblUpdateSuccess.Text = "";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search PO details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillExternalPODetails();
            SetFilterState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill description.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPODetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ExternalPODescription oExternalPODescription = e.Item.DataItem as ExternalPODescription;
                TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                TextBox txtQuantity = e.Item.FindControl("txtQuantity") as TextBox;
                DropDownList ddlGSTCategory = e.Item.FindControl("ddlGSTCategory") as DropDownList;
                TextBox txtRate = e.Item.FindControl("txtRate") as TextBox;
                if (oExternalPODescription.Amount != 0)
                    txtAmount.Text = oExternalPODescription.Amount.ToString();

                if (oExternalPODescription.Quantity != 0)
                    txtQuantity.Text = oExternalPODescription.Quantity.ToString();

                if (oExternalPODescription.Rate != 0)
                    txtRate.Text = oExternalPODescription.Rate.ToString();

                txtAmount.Attributes.Add("onchange", "SetTotalAmount();");
                txtQuantity.Attributes.Add("onchange", "SetTotal("+e.Item.DisplayIndex+");");
                txtRate.Attributes.Add("onchange", "SetTotal(" + e.Item.DisplayIndex + ");");
                ddlGSTCategory.Attributes.Add("onchange", "SetTotal(" + e.Item.DisplayIndex + ");");
                
                List<GSTCategory> lstCategories = new List<GSTCategory>();
                if (ViewState["GSTCategories"] != null)
                    lstCategories = ViewState["GSTCategories"] as List<GSTCategory>;
                ListSource.FillDropDownList(lstCategories, ddlGSTCategory, "Name", "Id", Constants.S_SELECT);

                ddlGSTCategory.SelectedValue = oExternalPODescription.GSTCategoryId.ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill non duplicate PO no.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    protected void Validate_PONo(object obj, ServerValidateEventArgs e)
    {
        try
        {
            bool bIsValid = moPODetailsBL.IsExternalPONoDuplicate(hidId.Value.ToInt(), txtPONo.Text.Trim(), rdCategory.SelectedValue == Constants.S_ONE);
            e.IsValid = !bIsValid;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void hidData_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            string sUserId = ddlReceiverName.SelectedValue;
            FillReceiverName();
            ddlReceiverName.SelectedValue = sUserId;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to show/hide fields as per selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            ResetId();
            HideDetails();
            FillInstructions();
            FillExternalPODetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            ApproveRequest(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            ApproveRequest(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    private void ApproveRequest(bool abIsApproved)
    {
        string sComment = string.Empty;
        int iId = 0;
        foreach (ListViewItem oItem in lstvwReceiverDetails.Items)
        {
            iId = lstvwReceiverDetails.DataKeys[oItem.DisplayIndex]["Id"].ToInt();

            TextBox txtComment = oItem.FindControl("txtComment") as TextBox;
            sComment = txtComment.Text.Trim();
            break;
        }

        moPODetailsBL.ApproveRequest(iId, sComment, abIsApproved);
        FillExternalPODetails();
    }

    /// <summary>
    /// This method is used to populate PO details.
    /// </summary>
    /// <param name="iId"></param>
    /// <returns></returns>
    private ExternalPODetails PopulatePODetails(int iId)
    {
        ExternalPODetails oExternalPODetails = new ExternalPODetails
        {
            Id = iId,
            ReceiverId = ddlReceiverName.SelectedValue.ToInt(),
            PONo = txtPONo.Text,
            PODate = txtPODate.Text.ToDateTime(),
            TotalAmount = txtTotal.Text.ToDecimal(),
            Subject = txtSubject.Text,
            AdditionalRemarks = txtAdditionalRemarks.Text,
            InstructionIds = GetSelectedInstructions(),
            IsPO = (rdCategory.SelectedValue == "1" ? true : false)
        };
        if (rdCategory.SelectedValue == "2")
        {
            oExternalPODetails.StartDate = txtStartDate.Text.ToDateTime();
            oExternalPODetails.EndDate = txtEndDate.Text.ToDateTime();
        }
        return oExternalPODetails;
    }

    /// <summary>
    /// This method is used to populate Description.
    /// </summary>
    /// <param name="aiInvoiceId"></param>
    /// <returns></returns>
    private List<ExternalPODescription> PopulateDescriptions(int aiPODId)
    {
        List<ExternalPODescription> oExternalPODescription = new List<ExternalPODescription>();
        {
            foreach (ListViewDataItem item in lstvwPODetails.Items)
            {
                TextBox oTextBoxDescription = item.FindControl("txtDescription") as TextBox;
                TextBox oTextBoxAmount = item.FindControl("txtAmount") as TextBox;
                TextBox oTextBoxQuantity = item.FindControl("txtQuantity") as TextBox;
                TextBox oTextBoxRate = item.FindControl("txtRate") as TextBox;
                DropDownList oDropdownGSTCategory = item.FindControl("ddlGSTCategory") as DropDownList;
                int iId = lstvwPODetails.DataKeys[item.DisplayIndex]["Id"].ToInt();

                if (oTextBoxAmount.Text.Trim() != string.Empty && oTextBoxAmount.Text.Trim() != Constants.S_ZERO && oTextBoxDescription.Text.Trim() != string.Empty)
                {
                    oExternalPODescription.Add(new ExternalPODescription
                    {
                        Id = iId,
                        PODId = aiPODId,
                        Description = oTextBoxDescription.Text.Trim(),
                        Amount = oTextBoxAmount.Text.ToDecimal(),
                        Quantity = oTextBoxQuantity.Text.ToInt(),
                        Rate = oTextBoxRate.Text.ToDecimal(),
                        GSTCategoryId = oDropdownGSTCategory.SelectedValue.ToInt(),
                    });
                }
            }

            return oExternalPODescription;
        }
    }

    /// <summary>
    /// This method is used to save PO details.
    /// </summary>
    private void Save()
    {
        int iId = 0;
        if (hidId.Value != string.Empty)
        {
            iId = Convert.ToInt32(hidId.Value);
        }

        ExternalPODetails oExternalPODetails = PopulatePODetails(iId);
        List<ExternalPODescription> oExternalPODetails1 = PopulateDescriptions(iId);
       
        string sXml = base.GenerateXml(oExternalPODetails1);

        moPODetailsBL.Save(sXml, oExternalPODetails);
        if (rdCategory.SelectedValue == "1")
        {
            if (btnSave.Text == S_TEXT_SAVE)
                LblUpdateSuccess.Text = S_SAVE_MSG;
            else
            {
                LblUpdateSuccess.Text = S_UPDATE_MSG;
                btnSave.Text = S_TEXT_SAVE;
            }
        }
        else
        {
            if (btnSave.Text == S_TEXT_SAVE)
                LblUpdateSuccess.Text = S_SAVE_MSG_WO;
            else
                LblUpdateSuccess.Text = S_UPDATE_MSG_WO;
        }

        ResetId();

        FillExternalPODetails();
        FillInstructions();
        ResetFields();
    }

    private void ResetId()
    {
        hidId.Value = "0";
    }

    private string GetSelectedInstructions()
    {
        StringBuilder obj = new StringBuilder();
        foreach (ListViewDataItem OItem in lstvwInstructions.Items)
        {
            CheckBoxList ChkInstructionList = OItem.FindControl("ChkInstructionList") as CheckBoxList;

            for (int iIndex = 0; iIndex < ChkInstructionList.Items.Count; iIndex++)
            {
                if (ChkInstructionList.Items[iIndex].Selected == true)
                    obj.Append("," + ChkInstructionList.Items[iIndex].Value);
            }
            string sSelectedItem = ChkInstructionList.ID;
        }
        
        if (obj.ToString().Length > 0)
            return obj.ToString().Substring(1);
        else
            return string.Empty;
    }

   /// <summary>
   /// This method is used to edit PO details.
   /// </summary>
   /// <param name="aiId"></param>
    private void SetControlsForEditMode(int aiId)
    {
        btnSave.Text = S_TEXT_UPDATE;
        hidId.Value = aiId.ToString();

        ExternalPODetails oExternalPODetails = moPODetailsBL.Get(aiId);
        ddlReceiverName.SelectedValue = oExternalPODetails.ExternalPOUserId.ToString();
        txtPONo.Text = oExternalPODetails.PONo;
        txtPODate.Text = oExternalPODetails.PODate.ToString(Constants.S_DATE_FORMAT);
        txtTotal.Text = oExternalPODetails.TotalAmount.ToString();
        //ddlGSTCategory.SelectedValue = oExternalPODetails.GSTCategoryId.ToString();
        //txtGST.Text = oExternalPODetails.GST.ToString();
        //txtGSTAmount.Text = (Math.Round((oExternalPODetails.TotalAmount * oExternalPODetails.GST) / 100,2)).ToString();
        //txtGrandTotal.Text = oExternalPODetails.GrandTotal.ToString();
        txtSubject.Text = oExternalPODetails.Subject;
        if (rdCategory.SelectedValue == "2")
        {
            txtStartDate.Text = oExternalPODetails.StartDate.ToString(Constants.S_DATE_FORMAT);
            txtEndDate.Text = oExternalPODetails.EndDate.ToString(Constants.S_DATE_FORMAT);
        }
        txtAdditionalRemarks.Text = oExternalPODetails.AdditionalRemarks;
        lblPreparedBy.Text = "Prepared By : "+oExternalPODetails.PreparedBy;

        bool bIsExtFound = false;
        foreach (ListViewDataItem OItem in lstvwInstructions.Items)
        {
            CheckBoxList ChkInstructionList = OItem.FindControl("ChkInstructionList") as CheckBoxList;

            bool bIsFound = false;
            for (int iIndex = 0; iIndex < ChkInstructionList.Items.Count; iIndex++)
            {
                if (oExternalPODetails.InstructionList.Contains(ChkInstructionList.Items[iIndex].Value.ToInt()))
                    ChkInstructionList.Items[iIndex].Selected = true;
                else
                {
                    ChkInstructionList.Items[iIndex].Selected = false;
                    bIsFound = true;
                    bIsExtFound = true;
                }
            }

            CheckBox chkAllInstructions = OItem.FindControl("chkAllInstructions") as CheckBox;
            if (!bIsFound)
                chkAllInstructions.Checked = true;
            else
                chkAllInstructions.Checked = false;

            string sSelectedItem = ChkInstructionList.ID;
        }

        if (bIsExtFound)
            chkAll.Checked = false;
        else
            chkAll.Checked = true;
        
            FillDescriptions(aiId);
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to delete perticular record of receiver details.
    /// </summary>
    /// <param name="InvoiceNo"></param>
    private void Delete(int aiId)
    {
        moPODetailsBL.Delete(aiId);
        if (rdCategory.SelectedValue == "1")
            LblUpdateSuccess.Text = S_DELETE_MSG;
        else
            LblUpdateSuccess.Text = S_DELETE_MSG_WO;
    }

   /// <summary>
   /// This method is used to reset fields.
   /// </summary>
    private void ResetFields()
    {
        ddlReceiverName.ClearSelection();
        //ddlGSTCategory.ClearSelection();
        txtTotal.Text = string.Empty;
        txtPODate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtPONo.Text = string.Empty;
        txtSearch.Text = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        //txtGST.Text = string.Empty;
        //txtGrandTotal.Text = string.Empty;
        FillDescriptions(0);
        ddlReceiverName.Focus();
        txtSubject.Text = string.Empty;
        txtStartDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        foreach (ListViewDataItem OItem in lstvwInstructions.Items)
        {
            CheckBoxList ChkInstructionList = OItem.FindControl("ChkInstructionList") as CheckBoxList;
            ChkInstructionList.ClearSelection();
        
             CheckBox chkAllInstructions = OItem.FindControl("chkAllInstructions") as CheckBox;
            chkAllInstructions.Checked = false;
        }
        chkAll.Checked = false;
        txtAdditionalRemarks.Text = string.Empty;
        //txtGSTAmount.Text = string.Empty;
        lblPreparedBy.Text = "Prepared By : -";        
    }

    /// <summary>
    /// This method is used to fill Receiver name dropdown.
    /// </summary>
    private void FillReceiverName()
    {
        List<ReceiverName> lstReceiverName = moPODetailsBL.GetReceiverName();
        ListSource.FillDropDownList(lstReceiverName, ddlReceiverName, "Name", "ReceiverId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill GSTCategoryId dropdown
    /// </summary>
    private void FillGSTCategory()
    {
        List<GSTCategory> lstGSTCategory = moPODetailsBL.GetGSTCategory();
        //ListSource.FillDropDownList(lstGSTCategory, ddlGSTCategory, "Name", "Id", Constants.S_SELECT);
        ViewState["GSTCategories"] = lstGSTCategory;

        var jsSerializer = new JavaScriptSerializer();
        hidGSTData.Value = jsSerializer.Serialize(lstGSTCategory);
    }

    /// <summary>
    /// This method is used to fill Description listview.
    /// </summary>
    /// <param name="aiId"></param>
    private void FillDescriptions(int aiId)
    {
        List<ExternalPODescription> lstPODetails = moPODetailsBL.GetPODescriptions(aiId);
        lstvwPODetails.DataSource = lstPODetails;
        lstvwPODetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill PO details listview.
    /// </summary>
    private void FillExternalPODetails()
    {
        lstvwReceiverDetails.DataSourceID = objdsReceiverDetails.ID;
        lstvwReceiverDetails.DataBind();

        HtmlTableCell thPONO = lstvwReceiverDetails.FindControl("thPONO") as HtmlTableCell;
        HtmlTableCell thPODate = lstvwReceiverDetails.FindControl("thPODate") as HtmlTableCell;
        
        if (thPONO != null)
        {
            LinkButton lnkPONO = thPONO.FindControl("lnkPONO") as LinkButton;
            LinkButton lnkPODate = thPODate.FindControl("lnkPODate") as LinkButton;
            Label lblReport = thPODate.FindControl("lblReport") as Label;

            if (rdCategory.SelectedValue == "2")
            {
                lnkPONO.Text = "WO No.";
                lnkPODate.Text = "WO Date";
                lblReport.Text = "WO";
            }
            else
            {
                lnkPONO.Text = "PO No.";
                lnkPODate.Text = "PO Date";
                lblReport.Text = "PO";
            }
        }

        HtmlTableCell thSend = lstvwReceiverDetails.FindControl("thSend") as HtmlTableCell;
        HtmlTableCell thEdit = lstvwReceiverDetails.FindControl("thEdit") as HtmlTableCell;
        HtmlTableCell thDelete = lstvwReceiverDetails.FindControl("thDelete") as HtmlTableCell;
        HtmlTableCell thPay = lstvwReceiverDetails.FindControl("thPay") as HtmlTableCell;
        HtmlTableCell thAction = lstvwReceiverDetails.FindControl("thAction") as HtmlTableCell;
        
        if (ddlStatus.SelectedValue == "5")
        {
            if (thEdit != null)
                thEdit.Visible = true;

            if (thDelete != null)
                thDelete.Visible = true;

            if (thSend != null)
                thSend.Visible = true;

            if (thPay != null)
                thPay.Visible = false;

            if (thAction != null)
                thAction.Visible = true;
        }
        else if (ddlStatus.SelectedValue == "6" || ddlStatus.SelectedValue == "3")
        {
            if (thEdit != null)
                thEdit.Visible = false;

            if (thDelete != null)
                thDelete.Visible = false;

            if (thSend != null)
                thSend.Visible = false;

            if (thPay != null)
                thPay.Visible = false;

            if (thAction != null)
                thAction.Visible = true;
        }
        else if (ddlStatus.SelectedValue == "4")
        {
            if (thEdit != null)
                thEdit.Visible = false;

            if (thDelete != null)
                thDelete.Visible = false;

            if (thSend != null)
                thSend.Visible = false;

            if (thAction != null)
                thAction.Visible = true;

            if (thPay != null)
                thPay.Visible = false;
        }
        else if (ddlStatus.SelectedValue == "2")
        {
            if (thEdit != null)
                thEdit.Visible = true;

            if (thDelete != null)
                thDelete.Visible = false;

            if (thSend != null)
                thSend.Visible = true;

            if (thPay != null)
                thPay.Visible = false;

            if (thAction != null)
                thAction.Visible = true;
        }
        else if (ddlStatus.SelectedValue == "-9999")
        {
            if (thPay != null)
                thPay.Visible = true;

            if (thEdit != null)
                thEdit.Visible = false;

            if (thDelete != null)
                thDelete.Visible = false;

            if (thSend != null)
                thSend.Visible = false;

            if (thAction != null)
                thAction.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to fill Instructions checkboxlist.
    /// </summary>
    private void FillInstructions()
    {
        moPOInstructionDetails = moPODetailsBL.GetInstructions();
        //ListSource.FillCheckBoxList(oPOInstructionDetails.Instructions, ChkInstructionList, "InstructionName", "Id");

        List<POExternalCategory> lstCategory = new List<POExternalCategory>();

        if (rdCategory.SelectedValue == Constants.S_ONE)
            lstCategory = moPOInstructionDetails.Categories.Where(nm => !nm.Category.Contains("AMC")).ToList();
        else
            lstCategory = moPOInstructionDetails.Categories;
                
        lstvwInstructions.DataSource = lstCategory;
        lstvwInstructions.DataBind();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ResetLabel()");
        txtPONoPrefix.Text = hidPOPrefix.Value;
        lnkReceiverDetails.Attributes.Add("onclick", "OpenReceiverPopup(); return false;");
        //lnkBankDetails.Attributes.Add("onclick", "OpenBankPopup(); return false;");
       
        chkAll.Attributes.Add("onclick","SelectAllInstructions(this)");

        rdCategory.SelectedValue = Constants.S_ONE;

        txtPODate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);

        if (QueryString["StatusId"] != null)
        {
            ddlStatus.SelectedValue = QueryString["StatusId"].ToString();
            txtSearch.Text = QueryString["Filter"].ToString();
            btnSearch_Click(btnSearch, null);
        }
    }

    private void HideDetails()
    {
        if (rdCategory.SelectedValue == "2")
        {
            trAMCstart.Visible = true;
            trAMCend.Visible = true;
            lblPONo.Text = "WO No :";
            lblPODate.Text = "WO Date :";
            txtPONoPrefix.Text = hidWoPrefix.Value;
            RequiredStartDate.Enabled = true;
            RequiredEndDate.Enabled = true;
            spnFilterHeader.InnerText = "Receiver Name / WO No. : ";
        }
        else
        {
            spnFilterHeader.InnerText = "Receiver Name / PO No. : ";
            trAMCstart.Visible = false;
            trAMCend.Visible = false;
            lblPONo.Text = "PO No :";
            lblPODate.Text = "PO Date :";
            txtPONoPrefix.Text = hidPOPrefix.Value;
            RequiredStartDate.Enabled = false;
            RequiredEndDate.Enabled = false;
        }
    }

    private void FillStatus()
    {
        RequisitionBL oRequisitionBL = new RequisitionBL();
        DataTable oDTStatus = oRequisitionBL.GetStatusList();

        DataRow[] dr = oDTStatus.Select("StatusName = 'Pending' OR StatusName = 'Partially Approved' OR StatusName = 'Canceled'");
        if (dr.Length > 0)
        {
            for (int k = 0; k < dr.Length; k++)
                dr[k].Delete();
        }

        DataRow[] drReq = oDTStatus.Select("StatusName = 'My Requisition'");
        if (drReq.Length > 0)
            drReq[0]["StatusName"] = "My Orders";

        oDTStatus.AcceptChanges();

        if (hidHasFullAccess.Value == Constants.S_YES)
        {
            DataRow drNew = oDTStatus.NewRow();
            drNew["StatusId"] = -9999;
            drNew["StatusName"] = "Available For Payment";
            oDTStatus.Rows.Add(drNew);
        }

        ControlUtility.FillDropDownList(oDTStatus, ref ddlStatus, "StatusId", "StatusName", Constants.S_EMPTY_STRING);
    }

    /// <summary>
    /// This method is used to set filter state.
    /// </summary>
    private void SetFilterState()
    {
        hidStatusId.Value = ddlStatus.SelectedValue;
        hidFilter.Value = txtSearch.Text.Trim();
    }

    /// <summary>
    /// This method is used to move to bottom of page.
    /// </summary>
    private void MovePageToBottom()
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ScrollToBottom", "window.scrollTo (0, document.body.scrollHeight);", true);
    }

    /// <summary>
    /// This method is used to set full access.
    /// </summary>
    private void CheckFullAccess()
    {
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        if (moUserRole == Constants.UserRoles.Admin || lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.ExternalPOStaff.ToInt() && ru.UserId == miUserId))
            hidHasFullAccess.Value = Constants.S_YES;
        else
            hidHasFullAccess.Value = Constants.S_NO;
    }

    #endregion    
}