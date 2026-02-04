// File Name  : ItemIssueUI.aspx.cs
// Created By : Amit
// Date       : 07/07/2009
// Description: This class is used to issue items for approved requisition. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities.Inventory;

public partial class ItemIssueUI : SchoolBase
{
    #region " Constants "

    private const string S_DEFAULT_SORT_EXPRESSION = "ApprovedDate";
    private const string S_COMMAND_VIEW = "View";
    private const string S_COMMAND_ISSUE_ITEM = "IssueItem";
    private const string S_LVIEW_STOCK_QTY_LBL = "lblStockQuantity";
    private const string S_LVIEW_REQ_QTY_LBL = "lblReqQuantity";
    private const string S_LVIEW_ISSUE_QTY_TXT = "txtIssueQuantity";
    private const string S_LVIEW_ISSUE_BUTTON = "btnIssue";
    private const string S_LVIEW_ISSUE_DROPDOWN = "cmbUnits";
    private const string S_LVIEW_ISSUE_ITEMBUTTON = "btnItemIssue";
    private const string S_LVIEW_ISSUE_TEXTBOX = "txtIssueQuantity";
    private const string S_COMMAND_ISSUE = "Issue";
    private const string S_ISSUE_ITEM_MESSAGE_SUBJECT = "Item(s) issued for your requisition";
    private const string S_ISSUE_ITEM_MESSAGE = "%IssuedQty% %ItemUnit% of %ItemName% %Verb% issued to you.";
    private const string S_LVIEW_ISSUE_CANCEL = "btnItemCancel";
    private const string S_COMMAND_CANCEL = "ItemCancel";

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to set default properties for page controls.
    /// And to show all final approved requisitions.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillSenderDesgCombo();
                SetDefaultProperties();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill combo with senders with respect to selected designation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iUserRole = Convert.ToInt32(ddlDesignation.SelectedValue);
            if (iUserRole != 0 && iUserRole != Convert.ToInt32(Constants.UserRoles.Admin))
            {
                ShowHideSender(true);
                FillSenderNameCombo();
                AddSortImage();
            }
            else
                SetDefaultProperties();
            tdItemsIssue.Visible = false;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set footer for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwRequisition);
            AddSortImage();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " ListView Events "

    #region " lstvwRequisition "

    /// <summary>
    /// This event is used set footer to the list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwRequisition.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwRequisition, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view items in approved requisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_VIEW)
            {
                ListViewDataItem oLstVwReq = (ListViewDataItem)e.Item;
                int iRowIndex = Convert.ToInt32(oLstVwReq.DisplayIndex);
                int iRequisitionID = Convert.ToInt32(lstvwRequisition.DataKeys[iRowIndex]["RequisitionID"].ToString());
                hidRequisitionID.Value = iRequisitionID.ToString();
                hidUserID.Value = lstvwRequisition.DataKeys[iRowIndex]["User_Id"].ToString();
                tdItemsIssue.Visible = true;
                lstvwReqItems.Visible = true;
                FillIssueItemListView(iRequisitionID);

                foreach (var Items in lstvwRequisition.Items)
                {
                    Label lblclrCode = Items.FindControl("lblCode") as Label;
                    Label lblclrRequisition = Items.FindControl("lblRequisition") as Label;
                    Label lblclrRequisitionDate = Items.FindControl("lblRequisitionDate") as Label;
                    Label lblclrApprovedDate = Items.FindControl("lblApprovedDate") as Label;
                    Label lblclrSenderName = Items.FindControl("lblSenderName") as Label;

                    lblclrCode.Font.Bold = false;
                    lblclrCode.ForeColor = System.Drawing.Color.Black;

                    lblclrRequisition.Font.Bold = false;
                    lblclrRequisition.ForeColor = System.Drawing.Color.Black;

                    lblclrRequisitionDate.Font.Bold = false;
                    lblclrRequisitionDate.ForeColor = System.Drawing.Color.Black;

                    lblclrApprovedDate.Font.Bold = false;
                    lblclrApprovedDate.ForeColor = System.Drawing.Color.Black;

                    lblclrSenderName.Font.Bold = false;
                    lblclrSenderName.ForeColor = System.Drawing.Color.Black;
                }

                Label lblCode = e.Item.FindControl("lblCode") as Label;
                Label lblRequisition = e.Item.FindControl("lblRequisition") as Label;
                Label lblRequisitionDate = e.Item.FindControl("lblRequisitionDate") as Label;
                Label lblApprovedDate = e.Item.FindControl("lblApprovedDate") as Label;
                Label lblSenderName = e.Item.FindControl("lblSenderName") as Label;

                lblCode.Font.Bold = true;
                lblCode.ForeColor = System.Drawing.Color.Maroon;

                lblRequisition.Font.Bold = true;
                lblRequisition.ForeColor = System.Drawing.Color.Maroon;

                lblRequisitionDate.Font.Bold = true;
                lblRequisitionDate.ForeColor = System.Drawing.Color.Maroon;

                lblApprovedDate.Font.Bold = true;
                lblApprovedDate.ForeColor = System.Drawing.Color.Maroon;

                lblSenderName.Font.Bold = true;
                lblSenderName.ForeColor = System.Drawing.Color.Maroon;                

                AddSortImage();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort items in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            HtmlTableRow oHtmlTableHeaderRow = lstvwRequisition.FindControl("trHeader") as HtmlTableRow;
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide item issue list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSenderName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tdItemsIssue.Visible = false;
            lstvwRequisition.DataBind();
            AddSortImage();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void chkIsGeneral_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            tdItemsIssue.Visible = false;
            if (chkIsGeneral.Checked)
            {
                ddlDesignation.SelectedValue = "0";
                ddlDesignation.Enabled = false;
                ddlSenderName.SelectedValue = "0";
            }
            else
            {
                ddlDesignation.SelectedValue = "0";
                ddlDesignation.Enabled = true;
                ddlSenderName.SelectedValue = "0";
            }

            AddSortImage();
            ShowHideSender(false);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void optAddstock_CheckedChanged(object sender, EventArgs e)
    {
        Response.Redirect("AddItemIntoStock.aspx");
    }

    protected void optIssueItem_CheckedChanged(object sender, EventArgs e)
    {
        Response.Redirect("ItemIssueUI.aspx");
    }

    #endregion " lstvwRequisition "

    #region " lstvwReqItems "

    /// <summary>
    /// This event is used to isssue item in requisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReqItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iItemId = e.CommandArgument.ToInt();
            hidItemId.Value = iItemId.ToString();   

            if (e.CommandName == S_COMMAND_ISSUE_ITEM)
            {
                int iIssueQuantity = DisableFields(e.Item, true);
                int iDisplayIndex = e.Item.DisplayIndex;

                HtmlTableRow tr = e.Item.FindControl("trItemDetails") as HtmlTableRow;
                HtmlTableCell td = tr.FindControl("tdItemDetails") as HtmlTableCell;
                ListView oListView = td.FindControl("lstItemDetails") as ListView;
                tr.Visible = true;

                int iRecordCount = FillItemDetails(iItemId, oListView);

                HiddenField hidPieceCount = e.Item.FindControl("hidPieceCount") as HiddenField;

                if (hidUOM.Value == Constants.S_ZERO)
                    iIssueQuantity = iIssueQuantity * hidPieceCount.Value.ToInt();

                if (iRecordCount > 0)
                    SetDefaultFields(iIssueQuantity, oListView);

                SetButtonState(iDisplayIndex, iRecordCount, td);
            }
            else if (e.CommandName == S_COMMAND_ISSUE)
            {
                DisableFields(e.Item, false);
                //AllMethods();
            }
            else if (e.CommandName == S_COMMAND_CANCEL)
            {
                ListViewDataItem oLstVwReq = (ListViewDataItem)e.Item;
                int iRowIndex = Convert.ToInt32(oLstVwReq.DisplayIndex);
                int iRequisitionID = Convert.ToInt32(lstvwReqItems.DataKeys[iRowIndex]["RequisitionID"].ToString());

                Label lblReqQuantity = e.Item.FindControl("lblReqQuantity") as Label;

              int aiCancelQty = Convert.ToInt32(lblReqQuantity.Text.ToDecimal());////////////////////
              //  int aiCancelQty = Convert.ToInt32(lblReqQuantity.Text.ToDecimal());////////////////////

                StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
                oStockIssueDetailsBL.CancelItemFromRequisition(iRequisitionID, iItemId, aiCancelQty, miUserId);

                FillIssueItemListView(iRequisitionID);
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to issue item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIssueItem_Click(object sender, EventArgs e)
    {
        foreach (ListViewDataItem oItem in lstvwReqItems.Items)
        {
            TextBox txtIssueQuantity = oItem.FindControl("txtIssueQuantity") as TextBox;
            hidSelectedItemQuantity.Value = Convert.ToString(txtIssueQuantity.Text);
            if(!string.IsNullOrEmpty(hidSelectedItemQuantity.Value))
                AllMethods();
        }
    }

    /// <summary>
    /// This event is used to cancel issue operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelIssue_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    /// <summary>
    /// This event is used to bind item data to list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReqItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oLstVwItemIssue = e.Item as ListViewDataItem;
                LinkButton oButton = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_BUTTON) as LinkButton;
                DropDownList cmbUnits = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_DROPDOWN) as DropDownList;
                Button btnItemIssue = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_ITEMBUTTON) as Button;
                TextBox txtIssuQuantity = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_TEXTBOX) as TextBox;
                string sUOM = lstvwReqItems.DataKeys[e.Item.DisplayIndex]["UOMUnit"].ToString();
                int iPiaceCount = lstvwReqItems.DataKeys[e.Item.DisplayIndex]["PieceCount"].ToInt();
                int iDetailLevel = lstvwReqItems.DataKeys[e.Item.DisplayIndex]["IsConsiderForDetailLevel"].ToInt();

                Button btnItemCancel = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_CANCEL) as Button;

                cmbUnits.Items.Clear();
                cmbUnits.Items.Add(new ListItem { Text = sUOM, Value = Constants.S_ZERO });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });

                if (iPiaceCount != Constants.I_ONE)
                {
                    int iTextValue = 0;
                    iTextValue = txtIssuQuantity.Text.ToDecimal().ToInt();
                    if (iTextValue % iPiaceCount == 0)
                    {
                        txtIssuQuantity.Text = Convert.ToString(txtIssuQuantity.Text.ToDecimal() / iPiaceCount);
                    }
                    else
                    {
                        cmbUnits.SelectedIndex = Constants.I_ONE;
                        if (Convert.ToInt32(iTextValue / iPiaceCount) >= 1)
                            cmbUnits.Enabled = true;
                        else
                            cmbUnits.Enabled = false;
                    }
                }
                else
                {
                    cmbUnits.SelectedIndex = Constants.I_ONE;
                    cmbUnits.Enabled = false;
                }

                if (iDetailLevel == Constants.I_ZERO)
                {
                    btnItemIssue.Visible = true;
                    oButton.Visible = false;
                    btnItemIssue.Attributes.Add("onclick", "if(ValidateIssueItem(this," + e.Item.DisplayIndex + ")) {return false;}");

                    btnItemCancel.Visible = true;                    
                }
                else
                {
                    oButton.Visible = true;
                    btnItemIssue.Visible = false;
                    oButton.Attributes.Add("onclick", "if(ValidateIssueItem(this," + e.Item.DisplayIndex + ")) {return false;}");

                    btnItemCancel.Visible = false;                    
                }

                Label lblMendetorySymbol = (Label)oLstVwItemIssue.FindControl("lblMendetory");
                lblMendetorySymbol.Visible = chkIsGeneral.Checked;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for display the description upto 100 character.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstItemDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oLstVwItemIssue = e.Item as ListViewDataItem;
                Label lblDescription = oLstVwItemIssue.FindControl("lblDescription") as Label;
                lblDescription.ToolTip = lblDescription.Text;
                string Description = lblDescription.Text;
                if(Description.Length >= 100)
                Description = Description.Substring(0, 100) + "...";
                lblDescription.Text = Description;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Yes button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            hidCancelRemainig.Value = Constants.S_YES;
            DisableIssueFields();
            AllMethods();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to No button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            hidCancelRemainig.Value = Constants.S_NO;
            DisableIssueFields();
            AllMethods();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to issue items if Required and issue item count is same.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnItemIssue_Click(object sender, EventArgs e)
    {
        try
        {
            hidCancelRemainig.Value = Constants.S_NO;
            DisableIssueFields();
            AllMethods();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    #endregion " lstvwReqItems "

    #endregion " ListView Events "

    #region " Private Methods "


    /// <summary>
    /// This method is used to disable fields.
    /// </summary>
    /// <param name="aoItem"></param>
    /// <returns></returns>
    private int DisableIssueFields()
    {
        hidSelectedItemQuantity.Value = hidIssueQuantity.Value.ToDecimal().ToString();
        hidUOM.Value= hidIssueUnits.Value.ToDecimal().ToString();
        hidCurrentStock.Value = hidStockBalance.Value.ToDecimal().ToString();
        hidSelectedItemComment.Value = hidComment.Value;

        return hidIssueQuantity.Value.ToDecimal().ToInt();
    }

    /// <summary>
    /// This method is used to call Items Issue methods.
    /// </summary>
    private void AllMethods()
    {
        IssueItem();

        ClearItemDetails();
        lstvwRequisition.DataSource = null;
        lstvwRequisition.DataBind();

        int iRequisitionID = Convert.ToInt32(hidRequisitionID.Value);
        FillIssueItemListView(iRequisitionID);
        AddSortImage();
    }
    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        foreach (ListViewDataItem oItem in lstvwReqItems.Items)
        {
            HtmlTableRow tr = oItem.FindControl("trItemDetails") as HtmlTableRow;
            if (tr != null)
            {
                tr.Visible = false;
                TextBox txtIssueQuantity = oItem.FindControl("txtIssueQuantity") as TextBox;
                txtIssueQuantity.Enabled = true;

                TextBox txtComment = oItem.FindControl("txtComment") as TextBox;
                txtComment.Enabled = true;
            }
        }

        ClearItemDetails();
    }

    /// <summary>
    /// This method is used to clear item details.
    /// </summary>
    private void ClearItemDetails()
    {
        hidSelectedItemComment.Value = string.Empty;
        hidSelectedItemQuantity.Value = string.Empty;
        hidItemId.Value = Constants.S_ZERO;
        hidCurrentStock.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is sued to issue item.
    /// </summary>
    private void IssueItem()
    {
        int iItemID = Convert.ToInt32(hidItemId.Value);
        int iRequisitionID = Convert.ToInt32(hidRequisitionID.Value);
        int iUOM = Convert.ToInt32(hidUOM.Value);
        double dIssueItem = Convert.ToDouble(hidSelectedItemQuantity.Value);

        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        oStockIssueDetailsBL.ItemID = iItemID;
        oStockIssueDetailsBL.ItemQty = dIssueItem;
        oStockIssueDetailsBL.RequisitionID = iRequisitionID;
        oStockIssueDetailsBL.UOMUnits = iUOM;
        oStockIssueDetailsBL.Insert_Date = DateTime.Today;
        oStockIssueDetailsBL.Inserted_By_Id = miUserId;
        oStockIssueDetailsBL.Updated_By_Id = miUserId;
        oStockIssueDetailsBL.Update_Date = DateTime.Today;
        oStockIssueDetailsBL.Is_Deleted = false;

        if (txtExpectedReturnDate.Text != string.Empty)
            oStockIssueDetailsBL.ExpectedReturnDate = Convert.ToDateTime(txtExpectedReturnDate.Text);  //Expected return date
        else
            oStockIssueDetailsBL.ExpectedReturnDate = Constants.S_DEFAULT_DATE_2.ToDateTime();

        if (hidSelectedItemComment.Value != Constants.S_EMPTY_STRING)
            oStockIssueDetailsBL.Comment = hidSelectedItemComment.Value.Trim();
        else
            oStockIssueDetailsBL.Comment = Constants.S_EMPTY_STRING;

        oStockIssueDetailsBL.IssuedItemIds = GetSelectedItems(iItemID);

        oStockIssueDetailsBL.InsertStockIssueDetails(miSchoolId, hidCancelRemainig.Value);
        SendItemIssueMailToReqCreator();
        lblSuccess.Text = "Item issued successfully!!!";
        lblSuccess.Visible = true;
    }

    /// <summary>
    /// This method is used to return selected item IDs.
    /// </summary>
    /// <param name="aiItemID"></param>
    /// <returns></returns>
    private string GetSelectedItems(int aiItemID)
    {
        StringBuilder obj = new StringBuilder();
        foreach (ListViewDataItem oItem in lstvwReqItems.Items)
        {
            int iItemId = lstvwReqItems.DataKeys[oItem.DisplayIndex]["ItemID"].ToInt();
            if (iItemId == aiItemID)
            {
                ListView oListView = oItem.FindControl("lstItemDetails") as ListView;

                foreach (ListViewDataItem oListItem in oListView.Items)
                {
                    CheckBox chkItemSelect = oListItem.FindControl("chkItemSelect") as CheckBox;
                    if (chkItemSelect.Checked)
                    {
                        int iItemDetailsId = oListView.DataKeys[oListItem.DisplayIndex]["Id"].ToInt();
                        obj.Append("," + iItemDetailsId);
                    }
                }

                break;
            }
        }

        if (obj.ToString().StartsWith(","))
            return obj.ToString().Substring(1);
        else
            return obj.ToString();
    }

    /// <summary>
    /// This method is used to fill sender name combobox.
    /// </summary>
    private void FillSenderNameCombo()
    {
        int iUserRoleID = Convert.ToInt32(ddlDesignation.SelectedValue);
        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        DataTable oDTReqSenderName = oStockIssueDetailsBL.GetAllUsersList(miSchoolId, iUserRoleID, miAcademicYearId);
        ControlUtility.FillDropDownList(oDTReqSenderName, ref ddlSenderName, "USER_ID", "NAME", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to show/hide sender name combobox.
    /// </summary>
    /// <param name="bFlag"></param>
    private void ShowHideSender(bool bFlag)
    {
        tdLblSenderName.Visible = bFlag;
        tdDDLSenderName.Visible = bFlag;
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
    /// This method is used to fill sender designation combobox.
    /// </summary>
    private void FillSenderDesgCombo()
    {
        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        DataTable oDTReqSenderDesg = oStockIssueDetailsBL.GetAllUserRolesForItemIssue();
        ControlUtility.FillDropDownList(oDTReqSenderDesg, ref ddlDesignation, "User_Role_Id", "User_Role_Name", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to set default properties to page control.
    /// </summary>
    private void SetDefaultProperties()
    {
        ddlDesignation.Focus();
        ddlSenderName.SelectedValue = "0";
        ShowHideSender(false);
        lstvwRequisition.DataBind();
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXPRESSION;
        HtmlTableRow oHtmlTableHeaderRow = lstvwRequisition.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        ApplyMouseHoverEffect(new List<Button> { btnBack });
    }

    /// <summary>
    /// This method is used to add sorting image in the list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwRequisition.SortDirection.ToString() == "Ascending")
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;

        if (lstvwRequisition.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwRequisition.SortExpression;
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXPRESSION;
        HtmlTableRow oHtmlTableHeaderRow = lstvwRequisition.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to fill list view that contains items in aproved requisition.
    /// </summary>
    /// <param name="aiRequisitionID"></param>
    private void FillIssueItemListView(int aiRequisitionID)
    {
        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        DataTable oDtItems = oStockIssueDetailsBL.GetItemsForRequisition(aiRequisitionID);
        if (oDtItems.IsNonEmpty())
        {
            lstvwReqItems.DataSource = oDtItems;
            lstvwReqItems.DataBind();
        }
        else
        {
            lstvwReqItems.Visible = false;
            lstvwRequisition.DataBind();
        }
    }

    /// <summary>
    /// This method is used to send message after item issue.
    /// </summary>
    private void SendItemIssueMailToReqCreator()
    {
        StringBuilder sMessageBody = new StringBuilder(string.Empty);
        string sUserID = hidUserID.Value;
        string sVerb = "are";
        double iItemCount = Convert.ToDouble(hidIssueQty.Value);
        if (iItemCount <= 1)
            sVerb = "is";

        sMessageBody.Append(S_ISSUE_ITEM_MESSAGE);
        sMessageBody = sMessageBody.Replace("%ItemName%", hidItemName.Value);
        sMessageBody = sMessageBody.Replace("%IssuedQty%", hidIssueQty.Value);
        sMessageBody = sMessageBody.Replace("%ItemUnit%", hidItemUnit.Value);
        sMessageBody = sMessageBody.Replace("%Verb%", sVerb);

        SendMessageAboutAction(sUserID, S_ISSUE_ITEM_MESSAGE_SUBJECT, sMessageBody.ToString());
    }

    /// <summary>
    /// This method is used to send the message about the action item issue.
    /// </summary>
    private void SendMessageAboutAction(string asReceiverUserIds, string asMsgSubject, string asMsgBody)
    {
        Message oMessage = new Message();
        oMessage.sMessageBody = asMsgBody;
        oMessage.sMessageSubject = asMsgSubject;
        oMessage.SetMessageReceivers(asReceiverUserIds, miUserId);
        oMessage.InsertMessageDetails(miUserId, moUserRole.ToInt(), miAcademicYearId);
    }

    /// <summary>
    /// This method is used to disable fields.
    /// </summary>
    /// <param name="aoItem"></param>
    /// <returns></returns>
    private int DisableFields(ListViewItem aoItem, bool abDisableFields)
    {
        TextBox txtIssueQuantity = aoItem.FindControl("txtIssueQuantity") as TextBox;
        txtIssueQuantity.Enabled = abDisableFields;
        hidSelectedItemQuantity.Value = txtIssueQuantity.Text;

        DropDownList cmbUOMUnits = aoItem.FindControl("cmbUnits") as DropDownList;
        hidUOM.Value = cmbUOMUnits.SelectedIndex.ToString();
        cmbUOMUnits.Enabled = abDisableFields;

        Label lblStockQuantity = aoItem.FindControl("lblStockQuantity") as Label;
        hidCurrentStock.Value = lblStockQuantity.Text;

        TextBox txtComment = aoItem.FindControl("txtComment") as TextBox;
        txtComment.Enabled = abDisableFields;
        hidSelectedItemComment.Value = txtComment.Text.Trim();

        return txtIssueQuantity.Text.ToDecimal().ToInt();
    }

    /// <summary>
    /// THis method is used to fill up item details.
    /// </summary>
    /// <param name="aiItemId"></param>
    /// <param name="alstvwItemDetails"></param>
    /// <returns></returns>
    private int FillItemDetails(int aiItemId, ListView alstvwItemDetails)
    {
        int iRecordCount = 0;
        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        List<ItemDetails> lstItemDetails = oStockIssueDetailsBL.GetItemDetails(miSchoolId, aiItemId);
        iRecordCount = lstItemDetails.Count;

        alstvwItemDetails.DataSource = lstItemDetails;
        alstvwItemDetails.DataBind();

        hidIssueItemCount.Value = Convert.ToString(lstItemDetails.Count);

        return iRecordCount;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    /// <param name="aiIssueQuantity"></param>
    /// <param name="alstvwItemDetails"></param>
    private static void SetDefaultFields(int aiIssueQuantity, ListView alstvwItemDetails)
    {
        int iStartIndex = 0;

        foreach (ListViewDataItem oItem in alstvwItemDetails.Items)
        {
            if (iStartIndex < aiIssueQuantity)
            {
                CheckBox chkItemSelect = oItem.FindControl("chkItemSelect") as CheckBox;
                chkItemSelect.Checked = true;
            }
            else
                break;

            iStartIndex++;
        }
    }

    /// <summary>
    /// THis method is used to set default state.
    /// </summary>
    /// <param name="aiDisplayIndex"></param>
    /// <param name="aiRecordCount"></param>
    /// <param name="aoCell"></param>
    private void SetButtonState(int aiDisplayIndex, int aiRecordCount, HtmlTableCell aoCell)
    {
        Button btnIssueItem = aoCell.FindControl("btnIssueItem") as Button;
        Button btnCancelIssue = aoCell.FindControl("btnCancelIssue") as Button;
        base.ApplyMouseHoverEffect(new List<Button> { btnIssueItem, btnCancelIssue });
        if (aiRecordCount > 0)
        {
            btnIssueItem.Enabled = true;
            btnCancelIssue.Enabled = true;

            btnIssueItem.Attributes.Add("onclick", "if(!ValidateItem(" + aiDisplayIndex + ")) return false;");
        }
        else
        {
            btnIssueItem.Enabled = false;
            btnCancelIssue.Enabled = false;
        }
    }

    #endregion " Private Methods "
}
