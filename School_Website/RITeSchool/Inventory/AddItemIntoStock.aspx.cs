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
using System.Drawing;

public partial class AddItemIntoStock : SchoolBase
{
    #region " Constants "

    private const string S_DEFAULT_SORT_EXPRESSION = "IssuedDate";
    private const string S_COMMAND_VIEW = "View";
    private const string S_COMMAND_RETURN_ITEM = "ReturnItem";
    private const string S_LVIEW_STOCK_QTY_LBL = "lblStockQuantity";
    private const string S_LVIEW_REQ_QTY_LBL = "lblReqQuantity";
    private const string S_LVIEW_ISSUE_QTY_TXT = "txtIssueQuantity";
    private const string S_LVIEW_RETURN_BUTTON = "btnReturn";
    private const string S_LVIEW_ISSUE_DROPDOWN = "cmbUnits";
    private const string S_LVIEW_ISSUE_ITEMBUTTON = "btnItemReturn";
    private const string S_LVIEW_RETURN_TEXTBOX = "txtReturnQuantity";
    private const string S_COMMAND_RETURN = "Return";
    private const string S_RETURN_ITEM_MESSAGE_SUBJECT = "Item(s) return into stock form your requisition";
    private const string S_RETURN_ITEM_MESSAGE = "%IssuedQty% %ItemUnit% of %ItemName% %Verb% issued to you.";

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

    protected void txtExpectedReturnDate_TextChanged(object sender, EventArgs e)  //
    {
         try
         {
             if (txtExpectedReturnDate.Text != string.Empty)
                 hidExpecyedReturnDate.Value = txtExpectedReturnDate.Text;
             else
                 hidExpecyedReturnDate.Value = Constants.S_DEFAULT_DATE_2;

             lstvwIssuedRequisition.DataBind();
             AddSortImage();  //
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
                //txtExpectedReturnDate.Text = DateTime.Now.ToDateTime().ToString(Constants.S_DATE_FORMAT);
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwIssuedRequisition);
            AddSortImage();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " ListView Events "

    #region " lstvwIssuedRequisition "

    /// <summary>
    /// This event is used set footer to the list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwIssuedRequisition_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwIssuedRequisition.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwIssuedRequisition, DtPgCount);
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
    protected void lstvwIssuedRequisition_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_VIEW)
            {
                ListViewDataItem oLstVwReq = (ListViewDataItem)e.Item;
                int iRowIndex = Convert.ToInt32(oLstVwReq.DisplayIndex);
                int iRequisitionID = Convert.ToInt32(lstvwIssuedRequisition.DataKeys[iRowIndex]["RequisitionID"].ToString());
                hidRequisitionID.Value = iRequisitionID.ToString();
                hidUserID.Value = lstvwIssuedRequisition.DataKeys[iRowIndex]["User_Id"].ToString();
                tdItemsIssue.Visible = true;
                lstvwIssuedReqItems.Visible = true;
                FillAddItemListView(iRequisitionID);
                
                foreach (var Items in lstvwIssuedRequisition.Items)
                    SetItemStatus(Items,false,Color.Black);

                SetItemStatus(e.Item, true, Color.Maroon);

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
    protected void lstvwIssuedRequisition_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            HtmlTableRow oHtmlTableHeaderRow = lstvwIssuedRequisition.FindControl("trHeader") as HtmlTableRow;
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
            //txtExpectedReturnDate.Text = DateTime.Now.ToDateTime().ToString(Constants.S_DATE_FORMAT);
            if (txtExpectedReturnDate.Text != string.Empty)
                hidExpecyedReturnDate.Value = txtExpectedReturnDate.Text;
            else
                hidExpecyedReturnDate.Value = Constants.S_DEFAULT_DATE_2;

            tdItemsIssue.Visible = false;            
            lstvwIssuedRequisition.DataBind();
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

    #endregion " lstvwIssuedRequisition "

    #region " lstvwIssuedReqItems "

    /// <summary>
    /// This event is used to isssue item in requisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwIssuedReqItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iItemId = e.CommandArgument.ToInt();
            hidItemId.Value = iItemId.ToString();

            if (e.CommandName == S_COMMAND_RETURN_ITEM)
            {
                int iReturnQuantity = DisableFields(e.Item, true);
                int iDisplayIndex = e.Item.DisplayIndex;

                HtmlTableRow tr = e.Item.FindControl("trItemDetails") as HtmlTableRow;
                HtmlTableCell td = tr.FindControl("tdItemDetails") as HtmlTableCell;
                ListView oListView = td.FindControl("lstItemDetails") as ListView;
                tr.Visible = true;

                int iRecordCount = FillItemDetails(iItemId, oListView);

                HiddenField hidPieceCount = e.Item.FindControl("hidPieceCount") as HiddenField;

                if (hidUOM.Value == Constants.S_ZERO)
                    iReturnQuantity = iReturnQuantity * hidPieceCount.Value.ToInt();

                if (iRecordCount > 0)
                    SetDefaultFields(iReturnQuantity, oListView);

                SetButtonState(iDisplayIndex, iRecordCount, td);
            }
            else if (e.CommandName == S_COMMAND_RETURN)
            {
                DisableFields(e.Item, false);
                AllMethods();
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
    protected void btnReturnItem_Click(object sender, EventArgs e)
    {
        foreach (ListViewDataItem oItem in lstvwIssuedReqItems.Items)
        {
            TextBox txtReturnQuantity = oItem.FindControl("txtReturnQuantity") as TextBox;
            hidSelectedItemQuantity.Value = Convert.ToString(txtReturnQuantity.Text);
            if (!string.IsNullOrEmpty(hidSelectedItemQuantity.Value))
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
    protected void lstvwIssuedReqItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oLstVwItemIssue = e.Item as ListViewDataItem;
                LinkButton oButton = oLstVwItemIssue.FindControl(S_LVIEW_RETURN_BUTTON) as LinkButton;
                DropDownList cmbUnits = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_DROPDOWN) as DropDownList;
                Button btnItemReturn = oLstVwItemIssue.FindControl(S_LVIEW_ISSUE_ITEMBUTTON) as Button;
                TextBox txtReturnQuantity = oLstVwItemIssue.FindControl(S_LVIEW_RETURN_TEXTBOX) as TextBox;
                string sUOM = lstvwIssuedReqItems.DataKeys[e.Item.DisplayIndex]["UOMUnit"].ToString();
                int iPiaceCount = lstvwIssuedReqItems.DataKeys[e.Item.DisplayIndex]["PieceCount"].ToInt();
                int iDetailLevel = lstvwIssuedReqItems.DataKeys[e.Item.DisplayIndex]["IsConsiderForDetailLevel"].ToInt();

                cmbUnits.Items.Clear();
                cmbUnits.Items.Add(new ListItem { Text = sUOM, Value = Constants.S_ZERO });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });

                if (iPiaceCount != Constants.I_ONE)
                {
                    int iTextValue = 0;
                    iTextValue = txtReturnQuantity.Text.ToDecimal().ToInt();
                    if (iTextValue % iPiaceCount == 0)
                    {
                        txtReturnQuantity.Text = Convert.ToString(txtReturnQuantity.Text.ToDecimal() / iPiaceCount);
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
                    btnItemReturn.Visible = true;
                    oButton.Visible = false;
                    btnItemReturn.Attributes.Add("onclick", "if(ValidateReturnItem(this," + e.Item.DisplayIndex + ")) {return false;}");
                }
                else
                {
                    oButton.Visible = true;
                    btnItemReturn.Visible = false;
                    oButton.Attributes.Add("onclick", "if(ValidateReturnItem(this," + e.Item.DisplayIndex + ")) {return false;}");
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


    #endregion " lstvwIssuedReqItems "

    #endregion " ListView Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to call Items Issue methods.
    /// </summary>
    private void AllMethods()
    {
        ReturnItem();

        ClearItemDetails();
        lstvwIssuedRequisition.DataSource = null;

        if (txtExpectedReturnDate.Text != string.Empty)
            hidExpecyedReturnDate.Value = txtExpectedReturnDate.Text;
        else
            hidExpecyedReturnDate.Value = Constants.S_DEFAULT_DATE_2;

        lstvwIssuedRequisition.DataBind();

        int iRequisitionID = Convert.ToInt32(hidRequisitionID.Value);
        FillAddItemListView(iRequisitionID);
        AddSortImage();
    }
    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        foreach (ListViewDataItem oItem in lstvwIssuedReqItems.Items)
        {
            HtmlTableRow tr = oItem.FindControl("trItemDetails") as HtmlTableRow;
            if (tr != null)
            {
                tr.Visible = false;
                TextBox txtReturnQuantity = oItem.FindControl("txtReturnQuantity") as TextBox;
                txtReturnQuantity.Enabled = true;

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
    private void ReturnItem()
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
        if (hidSelectedItemComment.Value != Constants.S_EMPTY_STRING)
            oStockIssueDetailsBL.Comment = hidSelectedItemComment.Value.Trim();
        else
            oStockIssueDetailsBL.Comment = Constants.S_EMPTY_STRING;

        oStockIssueDetailsBL.IssuedItemIds = GetSelectedItems(iItemID);

        oStockIssueDetailsBL.InsertStockReturnDetails(miSchoolId);

        SendItemIssueMailToReqCreator();
        lblSuccess.Text = "Item added into the Stock successfully!!!";
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
        foreach (ListViewDataItem oItem in lstvwIssuedReqItems.Items)
        {
            int iItemId = lstvwIssuedReqItems.DataKeys[oItem.DisplayIndex]["ItemID"].ToInt();
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
        //txtExpectedReturnDate.Text = DateTime.Now.ToDateTime().ToString(Constants.S_DATE_FORMAT);
        if (txtExpectedReturnDate.Text != string.Empty)
            hidExpecyedReturnDate.Value = txtExpectedReturnDate.Text;
        else
            hidExpecyedReturnDate.Value = Constants.S_DEFAULT_DATE_2;
        
        lstvwIssuedRequisition.DataBind();
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXPRESSION;
        HtmlTableRow oHtmlTableHeaderRow = lstvwIssuedRequisition.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        ApplyMouseHoverEffect(new List<Button> { btnBack });
    }

    /// <summary>
    /// This method is used to add sorting image in the list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwIssuedRequisition.SortDirection.ToString() == "Ascending")
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;

        if (lstvwIssuedRequisition.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwIssuedRequisition.SortExpression;
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXPRESSION;
        HtmlTableRow oHtmlTableHeaderRow = lstvwIssuedRequisition.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to fill list view that contains items which are Issued from aproved requisition.
    /// </summary>
    /// <param name="aiRequisitionID"></param>
    private void FillAddItemListView(int aiRequisitionID)
    {
        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        DataTable oDtItems = oStockIssueDetailsBL.GetIssuedItemsOfRequisition(aiRequisitionID);
        if (oDtItems.IsNonEmpty())
        {
            lstvwIssuedReqItems.DataSource = oDtItems;
            lstvwIssuedReqItems.DataBind();
        }
        else
        {
            lstvwIssuedReqItems.Visible = false;
            if (txtExpectedReturnDate.Text != string.Empty)
                hidExpecyedReturnDate.Value = txtExpectedReturnDate.Text;
            else
                hidExpecyedReturnDate.Value = Constants.S_DEFAULT_DATE_2;

            lstvwIssuedRequisition.DataBind();
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
        double iItemCount = Convert.ToDouble(hidReturnQty.Value);
        if (iItemCount <= 1)
            sVerb = "is";

        sMessageBody.Append(S_RETURN_ITEM_MESSAGE);
        sMessageBody = sMessageBody.Replace("%ItemName%", hidItemName.Value);
        sMessageBody = sMessageBody.Replace("%IssuedQty%", hidReturnQty.Value);
        sMessageBody = sMessageBody.Replace("%ItemUnit%", hidItemUnit.Value);
        sMessageBody = sMessageBody.Replace("%Verb%", sVerb);

        SendMessageAboutAction(sUserID, S_RETURN_ITEM_MESSAGE_SUBJECT, sMessageBody.ToString());
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
        TextBox txtReturnQuantity = aoItem.FindControl("txtReturnQuantity") as TextBox;
        txtReturnQuantity.Enabled = abDisableFields;
        hidSelectedItemQuantity.Value = txtReturnQuantity.Text;

        DropDownList cmbUOMUnits = aoItem.FindControl("cmbUnits") as DropDownList;
        hidUOM.Value = cmbUOMUnits.SelectedIndex.ToString();
        cmbUOMUnits.Enabled = abDisableFields;

        Label lblIssuedQuantity = aoItem.FindControl("lblIssuedQuantity") as Label;
        hidCurrentStock.Value = lblIssuedQuantity.Text;

        TextBox txtComment = aoItem.FindControl("txtComment") as TextBox;
        txtComment.Enabled = abDisableFields;
        hidSelectedItemComment.Value = txtComment.Text.Trim();

        return txtReturnQuantity.Text.ToDecimal().ToInt();
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
        int iRequisitionId = Convert.ToInt32(hidRequisitionID.Value.ToString());
        StockIssueDetailsBL oStockIssueDetailsBL = new StockIssueDetailsBL();
        List<ItemDetails> lstItemDetails = oStockIssueDetailsBL.GetIssuedItemDetails(miSchoolId, aiItemId, iRequisitionId);

        iRecordCount = lstItemDetails.Count;

        alstvwItemDetails.DataSource = lstItemDetails;
        alstvwItemDetails.DataBind();

        hidReturnItemCount.Value = Convert.ToString(lstItemDetails.Count);

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
        Button btnReturnItem = aoCell.FindControl("btnReturnItem") as Button;
        Button btnCancelIssue = aoCell.FindControl("btnCancelIssue") as Button;
        base.ApplyMouseHoverEffect(new List<Button> { btnReturnItem, btnCancelIssue });
        if (aiRecordCount > 0)
        {
            btnReturnItem.Enabled = true;
            btnCancelIssue.Enabled = true;

            btnReturnItem.Attributes.Add("onclick", "if(!ValidateItem(" + aiDisplayIndex + ")) return false;");
        }
        else
        {
            btnReturnItem.Enabled = false;
            btnCancelIssue.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to set item status.
    /// </summary>
    /// <param name="aoItem"></param>
    /// <param name="abShowBold"></param>
    /// <param name="aoColor"></param>
    private void SetItemStatus(ListViewItem aoItem, bool abShowBold, Color aoColor)
    {
        Label lblclrCode = aoItem.FindControl("lblCode") as Label;
        Label lblclrRequisition = aoItem.FindControl("lblRequisition") as Label;
        Label lblclrApprovedDate = aoItem.FindControl("lblApprovedDate") as Label;
        Label lblclrIssuedDate = aoItem.FindControl("lblIssuedDate") as Label;
        Label lblclrSenderName = aoItem.FindControl("lblSenderName") as Label;

        lblclrCode.Font.Bold = abShowBold;
        lblclrCode.ForeColor = aoColor;

        lblclrRequisition.Font.Bold = abShowBold;
        lblclrRequisition.ForeColor = aoColor;

        lblclrApprovedDate.Font.Bold = abShowBold;
        lblclrApprovedDate.ForeColor = aoColor;

        lblclrIssuedDate.Font.Bold = abShowBold;
        lblclrIssuedDate.ForeColor = aoColor;

        lblclrSenderName.Font.Bold = abShowBold;
        lblclrSenderName.ForeColor = aoColor;
    }

    #endregion " Private Methods "
}
