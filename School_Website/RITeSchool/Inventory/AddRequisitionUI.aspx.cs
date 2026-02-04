// File Name  : AddRequisitionUI.aspx.cs
// Created By : Milind
// Date       : 1/7/2009
//Description : This class is used to add/edit requisition as well as approve/denied requisition.


using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using CategoryEntities;
using SchoolEntities.Inventory;
using System.Linq;
using System.Web.UI;
using System.Xml.Serialization;
using System.IO;

public partial class AddRequisitionUI : SchoolBase
{

    #region Constants

    const string S_ITEM_DETAILS_DATA = "LstReqItems_DataSource";

    const string S_ITEM_ID = "ItemID";
    const string S_ITEMCODE = "ItemCode";
    const string S_ITEMNAME = "ItemName";
    const string S_CURRENTSTOCK = "CurrentStock";
    const string S_REQUISITION_ITEM_ID = "RequisitionDetailsID";
    const string S_ITEM_STATUS = "ItemStatus";
    const string S_ITEM_QUANTITY = "ItemQty";
    const string S_ISSUE_QUANTITY = "IssueQty";
    const string S_RETURN_QUANTITY = "ReturnQty";
    const string S_ITEM_ORG_QUANTITY = "ItemOrgQty";
    const string S_ITEM_UNIT = "UOMUnit";
    const string S_CAN_EDIT = "CanEdit";
    const string S_STATUS_DENIED = "Denied";
    const string S_CANCEL_QUANTITY = "CancelQty";

    const string S_STATUS_DENIED_ID = "2";
    const string S_STATUS_APPROVED_ID = "3";
    const string S_STATUS_PARTIALLYAPPROVED_ID = "8";

    const string S_CONSIDER_UNIT_QUANTITY = "ConsiderUnitQuantity";
    const string S_UOM_PIECE_COUNT = "UOMPieceCount";

    const string S_REQUISITION_SEND = "Send";
    const string S_CAN_CREATE_GENERAL_REQUISITION = "CanCreateGeneralRequisition";
    const string S_TEXT_SEARCH = "Search";
    const string S_TEXT_CHANGE_INPUT = "Change Input";
    const string S_TEXT_APPROVE = "Approve";
    const string S_TEXT_DELETE = "Delete";
    const string S_SAVE = "Save";
    const string S_SAVE_MESSAGE = "Requisition is saved(draft) successfully!!!";
    const string S_ERROR_MESSAGE = "You can not send requisition since approval level is not configured or user is not available in approval designation.";
    const string S_NEW_REQ_SUB = "New requisition for approval";
    const string S_URL = "~/RITeSchool/Inventory/RequisitionListUI.aspx";
    const string S_DENIED_MESSAGE_SUBJECT = "Requisition denied";
    const string S_APPROVE_MESSAGE_SUBJECT = "Requisition approved";
    const int I_MAX_LISTVIEW_ROWS = 5;

    const string S_NEW_REQUISITION_MESSAGE = "New requisition (%Code%) created by %Creater% is waiting for your approval.";
    const string S_REQUISITION_DENIED = "Requisition (%Code%) is denied by %DeniedlName%.";
    const string S_REQUISITION_DENIED_MESSAGE = "Requisition Item (%Code%) is denied successfully!!!";
    const string S_REQUISITION_APPROVE = "Your requisition (%Code%)  is approved by %ApprovalName%.";
    const string S_REQUISITION_FOR_APPROVAL = "New requisition (%Code%) created by %Creater% and approved by %ApprovalName% is waiting for your approval.";
    const string S_REQUISITION_MODIFYAPPROVE = "Requisition (%Code%) is modified and then approved by %ApprovalName%.";
    const string S_REQUISITION_PARTIALLYAPPROVE = "Requisition (%Code%) is Partially Approved by %ApprovalName%.";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Inventory Items/";
    private int I_DENY_COUNT = 0;

    #endregion

    
    #region Member Varialbes

    bool mbIsFinalApproval;
    string msRequisitionName;

    #endregion

    #region Events

    #region Page Events

    /// <summary>
    /// This event is used to set the page according to the status of the requisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQueryString(sender, e);
                SetGenralRequisition();
                InitailizeForm();
                RequisitionExpiry();
                FillItemCategoryCombo();
                CheckRoleAndAssignDisplayView();
                OpenIssueHistoryPopUp();
                SetDefaultButton(btnSearch);
                SetJavaScriptAttribute();
                HideFields();
            }
            lblErrorMsg.Text = Constants.S_EMPTY_STRING;
            lblMessage.Text = Constants.S_EMPTY_STRING;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill category combo box.
    /// </summary>
    private void FillItemCategoryCombo()
    {
        List<ItemCategory> lstCategory = ItemCategoryMasterBL.GetAllCategory(miSchoolId);
        ListSource.FillDropDownList(lstCategory, cmbCategory, "Name", "Id", Constants.S_ALL);
    }

    /// <summary>
    /// This event is used to search the items according to the search criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (QueryString.Count > Constants.I_ZERO)
                QueryString.Remove("ItemCode");

            if (btnSearch.Text == S_TEXT_SEARCH)
            {
                btnSearch.Text = S_TEXT_CHANGE_INPUT;
                txtItemCode.Enabled = false;
                cmbCategory.Enabled = false;
                trLstItems.Visible = true;
                DtPgCount.SetPageProperties(0, I_MAX_LISTVIEW_ROWS, false);
                lstvwItems.DataSourceID = lstDSobj.ID;
            }
            else
            {
                txtItemCode.Enabled = true;
                cmbCategory.Enabled = true;
                lstvwItems.DataSourceID = null;
                trLstItems.Visible = false;
                btnSearch.Text = S_TEXT_SEARCH;
                btnAddItem.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            string sFromPage = String.Empty;
            if (Request.QueryString.ToString() != String.Empty)
            {
                if (!QueryString["FromPage"].IsNullOrEmpty())
                    sFromPage = QueryString["FromPage"];
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the requisition in the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.IsValid)
            {
                const string S_REQUISITION_SAVE = S_SAVE;
                ManageRequisitionDetails(S_REQUISITION_SAVE);
                lblMessage.Visible = true;
                lblMessage.Text = S_SAVE_MESSAGE;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to send requisition for approval.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            if(Page.IsValid)
                SendRequisition();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to send requisition for approval.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
                SendRequisition();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the requisition in the database.
    /// as well as it send the requisition to the authority for approval.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSendReq_Click(object sender, EventArgs e)
    {
        try
        {
            if(this.IsValid)
                SendRequisition();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to approve the requisition.
    /// as well as it send the message about requisition approval to the creator of the requisition
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApproval_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable oDTUserId = ApproveRequisition(false);
            SendMailForApproval(oDTUserId);

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(S_URL + "?" + PrepareQueryString());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnFinalApproval_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable oDTUserId = ApproveRequisition(true);
            SendMailForApproval(oDTUserId);

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(S_URL + "?" + PrepareQueryString());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to modify the requisition which is coming for approval.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {
            SetEditModeToApprover();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel the modification in the requisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CancelModificationOfApprover();
            btnApproval.Text = S_TEXT_APPROVE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    #endregion

    #region ListView Events

    /// <summary>
    /// This event is used to fill the drop down list in the listview datapager according to pagesize.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItems.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwItems, DtPgCount);
            else
            {
                DtPgCount.Visible = false;
                //btnAddItem.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to capture image status according to Id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {   
                ImageButton imgItem = (ImageButton)(e.Item.FindControl("imgBtnItemImage"));
                int iImageCount = Convert.ToInt32(lstvwItems.DataKeys[e.Item.DisplayIndex]["ImageCount"]);
                imgItem.Visible = iImageCount > 0;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according to the selected pageindex in the combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            DataPager oDtPgDropDown = lstvwItems.FindControl("DtPgDropDown") as DataPager;
            DropDownList oddlCnt = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
            int iRowIndex = (Convert.ToInt32(oddlCnt.SelectedValue) - 1) * oDtPgDropDown.PageSize;
            oDtPgDropDown.SetPageProperties(iRowIndex, oDtPgDropDown.PageSize, true);
            int iCurrentPage = (oDtPgDropDown.StartRowIndex / oDtPgDropDown.PageSize) + 1;
            int iTotalPages = oDtPgDropDown.TotalRowCount / oDtPgDropDown.PageSize;
            Label oLabel = (oDtPgDropDown.Controls[0].FindControl("CurrentPageLabel")) as Label;
            oLabel.Text = "Page " + iCurrentPage + " of " + iTotalPages;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add listview(lstvwItems) items in the listview(LstVwReqItems).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            if (e.CommandName == "Add")
            {   
                lblErrorMsg.Text = Constants.S_EMPTY_STRING;
                if (hidCanCreateGeneralRequisition.Value == Constants.S_YES)
                    trIsGeneral.Visible = true;
                trLstReqItems.Visible = true;
                if (CheckIsDuplicateItemForRequisition(lstvwItems.DataKeys[iRowId]["ItemID"].ToString()))
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Item already exists.";
                }
                else
                {
                    UpdateItemQty();
                    ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
                    oItemsMasterBL.ItemCode = lstvwItems.DataKeys[iRowId]["ItemCode"].ToString();
                    oItemsMasterBL.ItemName = lstvwItems.DataKeys[iRowId]["ItemName"].ToString();
                    oItemsMasterBL.CurrentStock = lstvwItems.DataKeys[iRowId]["CurrentStock"].ToString();
                    oItemsMasterBL.Unit = lstvwItems.DataKeys[iRowId]["UOMUnit"].ToString();
                    oItemsMasterBL.ItemID = Convert.ToInt32(lstvwItems.DataKeys[iRowId]["ItemID"]);
                    oItemsMasterBL.UOMPieceCount = Convert.ToInt32(lstvwItems.DataKeys[iRowId]["PieceCount"]);
                    oItemsMasterBL.IssueQty = Convert.ToDecimal(lstvwItems.DataKeys[iRowId]["IssueQty"]);
                    oItemsMasterBL.ReturnQty = Convert.ToDecimal(lstvwItems.DataKeys[iRowId]["ReturnQty"]);
                    oItemsMasterBL.CancelQty = Convert.ToDecimal(lstvwItems.DataKeys[iRowId]["CancelQty"]);
                    //oItemsMasterBL.CurrentStock = oItemsMasterBL.CurrentStock * oItemsMasterBL.UOMPieceCount;

                    AddItemsToListview(oItemsMasterBL);
                }
                btnApproval.Text = S_TEXT_APPROVE;
                btnFinalApproval.Visible = true;
                hidRequisitionItemCount.Value = LstVwReqItems.Items.Count.ToString();
            }
            else if (e.CommandName == "ItemImage")
            {  
                int iItemId = Convert.ToInt32(lstvwItems.DataKeys[iRowId]["ItemID"]);
                ImageButton imgItem = (ImageButton)(oCurrentItem.FindControl("imgBtnItemImage"));
              
                ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
                List<ItemImageDetails> lstItemImage = oItemsMasterBL.GetImagesUrl(iItemId);
                if (lstItemImage != null && lstItemImage.Count > 0)
                {
                    DisplayItemImage(lstItemImage, 1, imgItem1);
                    DisplayItemImage(lstItemImage, 2, imgItem2);
                    DisplayItemImage(lstItemImage, 3, imgItem3);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "OpenPopup", "OpenPopup()", true);
                }
               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete the listview(LstVwReqItems)items
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwReqItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Remove")
            {
                int iItemID = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);

                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DataItemIndex);
                int iRequisitionDetailsId =
                    Convert.ToInt32(LstVwReqItems.DataKeys[iRowId][S_REQUISITION_ITEM_ID]);

                DataTable oDTItemsDetails;
                oDTItemsDetails = (DataTable)ViewState[S_ITEM_DETAILS_DATA];

                DataRow oDTRow = oDTItemsDetails.Rows[iRowId];
                oDTRow.Delete();
                oDTItemsDetails.AcceptChanges();
                LstVwReqItems.DataSource = oDTItemsDetails;
                ViewState[S_REQUISITION_ITEM_ID] = oDTItemsDetails;
                LstVwReqItems.DataBind();

                hidRequisitionItemCount.Value = LstVwReqItems.Items.Count.ToString();
                if (LstVwReqItems.Items.Count <= Constants.I_ZERO)
                {
                    trLstReqItems.Visible = false;
                    btnApproval.Text = S_TEXT_DELETE;
                    btnFinalApproval.Visible = false;
                    hidRequisitionId.Value = string.Empty;
                }
                else
                {
                    trLstReqItems.Visible = true;
                    btnApproval.Text = S_TEXT_APPROVE;
                    btnFinalApproval.Visible = true;
                }
            }
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void LstVwReqItems_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = LstVwReqItems.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void LstVwReqItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string sUOMName = LstVwReqItems.DataKeys[e.Item.DisplayIndex]["UOMUnit"].ToString();
                string sItemStatus = LstVwReqItems.DataKeys[e.Item.DisplayIndex]["ItemStatus"].ToString();
                DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;
                cmbUnits.Items.Clear();
                cmbUnits.Items.Add(new ListItem { Text = sUOMName, Value = "0" });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = "1" });

                DataRowView oDataItem = e.Item.DataItem as DataRowView;
                CheckBox chkIsSelected = (CheckBox)(e.Item.FindControl("ChkIsRequisitionToDeny"));
                if (sItemStatus == S_STATUS_DENIED)
                {
                    chkIsSelected.Checked = true;
                    chkIsSelected.Enabled = false;
                    HtmlTableRow cell = (HtmlTableRow)e.Item.FindControl("lstDataRow");
                    cell.Style.Add("color", "#ed4b40");
                }
                else
                {
                    chkIsSelected.Checked = false;
                    chkIsSelected.Enabled = true;
                }

                string sQuantity = string.Empty;
                bool bConsiderUnitQuantity = LstVwReqItems.DataKeys[e.Item.DisplayIndex]["ConsiderUnitQuantity"].ToBool();
                
                if (bConsiderUnitQuantity)
                {
                    sQuantity = oDataItem["ItemQty"].ToString();
                    cmbUnits.SelectedValue = Constants.S_ONE;
                }
                else
                {
                    if (oDataItem["UOMPieceCount"].ToInt() > 1)
                    {
                        if (oDataItem["ItemQty"].ToInt() % oDataItem["UOMPieceCount"].ToInt() == 0)
                        {
                            sQuantity = Math.Round(oDataItem["ItemQty"].ToDecimal() / oDataItem["UOMPieceCount"].ToInt(), 2).ToString();
                            cmbUnits.SelectedValue = Constants.S_ZERO;
                        }
                        else
                        {
                            sQuantity = oDataItem["ItemQty"].ToString();
                            cmbUnits.SelectedValue = Constants.S_ONE;
                        }
                    }
                    else
                    {
                        sQuantity = oDataItem["ItemQty"].ToString();
                        cmbUnits.SelectedValue = Constants.S_ZERO;
                        cmbUnits.Enabled = false;
                    }
                }

                TextBox txtQty = e.Item.FindControl("txtQty") as TextBox;
                Label lblQty = e.Item.FindControl("lblQty") as Label;
                Label lblOriginalQuantity = e.Item.FindControl("lblOriginalQuantity") as Label;
                HtmlTableCell oHtmlTableCellIssueQty
                        = (HtmlTableCell)e.Item.FindControl("tdIssueQty");
                HtmlTableCell oHtmlTableCellReturnQty
                        = (HtmlTableCell)e.Item.FindControl("tdReturnQty");
                HtmlTableCell oHtmlTableCellCancelQty
                        = (HtmlTableCell)e.Item.FindControl("tdCancelQty");
                
                if (oDataItem["CanEdit"].ToBool())
                    txtQty.Text = sQuantity;
                else
                {
                    if (bConsiderUnitQuantity)
                    {
                        lblQty.Text = sQuantity + " " + Constants.S_UNITS;
                        lblOriginalQuantity.Text = sQuantity + " " + Constants.S_UNITS;
                    }
                    else
                    {
                        lblQty.Text = sQuantity + " " + sUOMName;
                        lblOriginalQuantity.Text = sQuantity + " " + sUOMName;
                    }
                }

                if (hidRequisitionMode.Value.ToString() == "Edit")
                {
                    oHtmlTableCellIssueQty.Visible = false;
                    oHtmlTableCellReturnQty.Visible = false;
                    oHtmlTableCellCancelQty.Visible = false;
                    LstVwReqItems.FindControl("thIssueQty").Visible = false;
                    LstVwReqItems.FindControl("thReturnQty").Visible = false;
                    LstVwReqItems.FindControl("thCancelQty").Visible = false;
                }
                else
                {
                    oHtmlTableCellIssueQty.Visible = true;
                    oHtmlTableCellReturnQty.Visible = true;
                    oHtmlTableCellCancelQty.Visible = true;
                    
                    LstVwReqItems.FindControl("thIssueQty").Visible = true;
                    LstVwReqItems.FindControl("thReturnQty").Visible = true;
                    LstVwReqItems.FindControl("thCancelQty").Visible = true;
                }

                if ((hidStatusId.Value == ((int)Constants.RequisitionStatus.Waiting_For_My_Approval).ToString()
                             || hidStatusId.Value == ((int)Constants.RequisitionStatus.Pending).ToString()))
                {
                    LstVwReqItems.FindControl("thDeny").Visible = true;

                    HtmlTableCell oHtmlTableCell
                        = (HtmlTableCell)e.Item.FindControl("tdDeny");
                    oHtmlTableCell.Visible = true;
                }
                else
                {
                    LstVwReqItems.FindControl("thDeny").Visible = false;

                    HtmlTableCell oHtmlTableCell
                        = (HtmlTableCell)e.Item.FindControl("tdDeny");
                    oHtmlTableCell.Visible = false;
                }
               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Private Methods

    public string GenerateXML(object alstGenerateXML)
    {
        var oStrwrtr = new StringWriter();
        new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
        string sXml = oStrwrtr.ToString();
        return sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
    }

   /// <summary>
    /// This method is used to send requisition.
    /// </summary>
    private void SendRequisition()
    {
        if (RequisitionDetailsBL.CanSendRequisition(miSchoolId, miAcademicYearId, miUserId))
        {
            ManageRequisitionDetails(S_REQUISITION_SEND);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(S_URL);
        }
        else
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = S_ERROR_MESSAGE;
        }
    }

    /// <summary>
    /// This method is used to create the datarow and bind that row to the gridview
    /// </summary>
    private void AddItemsToListview(ItemsMasterBL oItemsMasterBL)
    {
        DataTable oDTItemsDetails;
        if (ViewState[S_ITEM_DETAILS_DATA] == null)
            oDTItemsDetails = CreateItemsTable();
        else
            oDTItemsDetails = (DataTable)ViewState[S_ITEM_DETAILS_DATA];

        // Once a table has been created,create DataRow.
        DataRow oDataRow = AddRequisitionItemsToDataRow(oDTItemsDetails.NewRow(), oItemsMasterBL);
        oDTItemsDetails.Rows.Add(oDataRow);
        DataView oDTItemView = oDTItemsDetails.DefaultView;
        LstVwReqItems.DataSource = oDTItemView;
        oDTItemsDetails = (oDTItemView).ToTable();
        ViewState[S_ITEM_DETAILS_DATA] = oDTItemsDetails;
        LstVwReqItems.DataBind();
        if (hidRequisitionItemCount.Value == Constants.S_ZERO || hidRequisitionId.Value == Constants.S_EMPTY_STRING || hidRequisitionId.Value == null || (hidIsRequisitionModified.Value != string.Empty && hidIsRequisitionModified.Value == Constants.S_YES))
        {
            HtmlTableRow oHtmlTableHeaderRow
           = (HtmlTableRow)LstVwReqItems.FindControl("trHeader");
            HtmlTableCell oHtmlOrgQty
                = (HtmlTableCell)oHtmlTableHeaderRow.FindControl("thorgQty");
            if (oHtmlOrgQty != null)
                oHtmlOrgQty.Visible = false;
        }

    }
    
    /// <summary>
    /// This method is used to create new datatable
    /// </summary>
    /// <returns></returns>
    private DataTable CreateItemsTable()
    {
        // Create a new DataTable for requisition items details. 
        DataTable oDTItemsDetails = new DataTable();

        // Add columns to the Item table.        
        AddDataColumnToItemTable("System.Int32", S_ITEM_ID, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.String", S_ITEMCODE, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.String", S_ITEMNAME, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.String", S_CURRENTSTOCK, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.String", S_ITEM_STATUS, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Int32", S_REQUISITION_ITEM_ID, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Double", S_ITEM_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Double", S_ITEM_ORG_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.String", S_ITEM_UNIT, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.String", S_CAN_EDIT, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Boolean", S_CONSIDER_UNIT_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Int32", S_UOM_PIECE_COUNT, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Double", S_ISSUE_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Double", S_RETURN_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemTable("System.Double", S_CANCEL_QUANTITY, ref oDTItemsDetails, false);

        return oDTItemsDetails;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemTable(string asDataType, string asColumnName, ref DataTable aoDataTable, bool abIsPrimaryKey)
    {
        DataColumn oDataColumn = new DataColumn();
        oDataColumn.DataType = Type.GetType(asDataType);
        oDataColumn.ColumnName = asColumnName;
        aoDataTable.Columns.Add(oDataColumn);

        if (abIsPrimaryKey)
        {
            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = oDataColumn;
            aoDataTable.PrimaryKey = keys;
        }
    }

    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow AddRequisitionItemsToDataRow(DataRow oDR, ItemsMasterBL oItemsMasterBL)
    {
        DataRow oDRItem;

        oDRItem = oDR;
        // Then add the new row to the collection.
        oDRItem[S_ITEM_ID] = oItemsMasterBL.ItemID;
        oDRItem[S_ITEMCODE] = oItemsMasterBL.ItemCode;
        oDRItem[S_ITEMNAME] = oItemsMasterBL.ItemName;
        oDRItem[S_CURRENTSTOCK] = oItemsMasterBL.CurrentStock;
        oDRItem[S_REQUISITION_ITEM_ID] = 0;
        oDRItem[S_ITEM_QUANTITY] = 0;
        oDRItem[S_ITEM_ORG_QUANTITY] = 0;
        oDRItem[S_ITEM_UNIT] = oItemsMasterBL.Unit;
        oDRItem[S_CAN_EDIT] = "True";
        oDRItem[S_CONSIDER_UNIT_QUANTITY] = "False"; 
        oDRItem[S_UOM_PIECE_COUNT] = oItemsMasterBL.UOMPieceCount;
        oDRItem[S_ISSUE_QUANTITY] = oItemsMasterBL.IssueQty;
        oDRItem[S_RETURN_QUANTITY] = oItemsMasterBL.ReturnQty;
        oDRItem[S_CANCEL_QUANTITY] = oItemsMasterBL.CancelQty;
        return oDRItem;
    }

    /// <summary>
    /// Generate XML for the Items.
    /// </summary>
    /// <returns></returns>
    private string GenerateRequisitionItemXML(bool IsIncludeStatus)
    {
        StringBuilder oRequisitionName = new StringBuilder(Constants.S_EMPTY_STRING);
        const int I_REQUISITION_NAME_LENGTH = 40;
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("RequisitionItems");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "RequisitionItems", "");

        // Loop through all the list view items.
        foreach (ListViewDataItem oListViewDataItem in LstVwReqItems.Items)
        {
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "RequisitionItems", "");

            int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
          
            TextBox otxtQty = (TextBox)oListViewDataItem.FindControl("txtQty");
            Label olblQty = (Label)oListViewDataItem.FindControl("lblQty");
            DropDownList cmbUOM = (DropDownList)oListViewDataItem.FindControl("cmbUnits");
            Label olblItemName = (Label)oListViewDataItem.FindControl("lblItemName");
            string[] qtyArray = olblQty.Text.Split(' ');

            string sAtrrName = "ItemID";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = (LstVwReqItems.DataKeys[iRowId]["ItemID"]).ToString();

            oXmlNode.Attributes.Append(attr);

            sAtrrName = "UOM";
            attr = oDoc.CreateAttribute(sAtrrName);
            if (cmbUOM.SelectedIndex == Constants.I_ZERO)
                attr.Value = Constants.S_ZERO;
            else
                attr.Value = Constants.S_ONE;

            oXmlNode.Attributes.Append(attr);

            sAtrrName = "ItemQty";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = !string.IsNullOrEmpty(otxtQty.Text.Trim()) ? otxtQty.Text.Trim() : qtyArray[0].Trim();

            oXmlNode.Attributes.Append(attr);

            sAtrrName = "ItemOrgQty";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = !string.IsNullOrEmpty(otxtQty.Text.Trim()) ? otxtQty.Text.Trim() : "0";

            oXmlNode.Attributes.Append(attr);

            if (IsIncludeStatus)
            {
                CheckBox chkIsSelected = (CheckBox)(oListViewDataItem.FindControl("ChkIsRequisitionToDeny"));
                sAtrrName = "StatusID";
                attr = oDoc.CreateAttribute(sAtrrName);
                if (chkIsSelected.Checked == true)
                    attr.Value = S_STATUS_DENIED_ID;
                else
                    attr.Value = S_STATUS_APPROVED_ID;
            }
            
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

            oRequisitionName.Append(olblItemName.Text + ",");
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        string sRequisitionName = oRequisitionName.ToString();
        sRequisitionName = sRequisitionName.Remove(sRequisitionName.LastIndexOf(","));
        if (sRequisitionName.Length > I_REQUISITION_NAME_LENGTH)
            sRequisitionName = sRequisitionName.Substring(Constants.I_ZERO, I_REQUISITION_NAME_LENGTH);
        msRequisitionName = txtReqName.Text.Trim();

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString(object Obj, EventArgs e)
    {
        if (QueryString.Count <= 0)
            return;

        if (QueryString["RequisitionID"] != null)
            hidRequisitionId.Value = QueryString["RequisitionID"];
        if (QueryString["StatusID"] != null)
            hidStatusId.Value = QueryString["StatusID"];
        if (QueryString["NextDesignationId"] != null)
            hidNextDesignationId.Value = QueryString["NextDesignationId"];
        if (QueryString["Mode"] != null)
            hidRequisitionMode.Value = QueryString["Mode"];
        if (QueryString["CreatorName"] != null)
            hidCreatorName.Value = QueryString["CreatorName"];
        if (QueryString["RequisitionCode"] != null)
            hidReqCode.Value = QueryString["RequisitionCode"];
        if (QueryString["CreatorID"] != null)
            hidCreatorID.Value = QueryString["CreatorID"];
        if (QueryString["IsFinalApproval"] != null)
            mbIsFinalApproval = QueryString["IsFinalApproval"].ToBool();

        if (QueryString["ItemCode"] != null)
        {
            txtItemCode.Text = QueryString["ItemCode"];
            btnSearch_Click(Obj, e);
        }
    }

    /// <summary>
    /// This method is used to check that item is already exist in the listview(LstVwReqItems)
    /// </summary>
    private bool CheckIsDuplicateItemForRequisition(string asItemId)
    {
        for (int iRowIndex = Constants.I_ZERO; iRowIndex < LstVwReqItems.Items.Count; iRowIndex++)
        {
            string sItemId = Convert.ToString(LstVwReqItems.DataKeys[iRowIndex]["ItemID"]);
            if (asItemId.Equals(sItemId))
                return true;
        }
        return false;
    }

    /// <summary>
    /// This method is used to display screen according to access,status and mode(edit or delete)
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {

        if (hidRequisitionId.Value != Constants.S_EMPTY_STRING && hidRequisitionId.Value != null)
        {
            int iRequisitionID = Convert.ToInt32(hidRequisitionId.Value);
            string sMode = hidRequisitionMode.Value;
            RequisitionDetailsBL oRequisitionDetailsBL = new RequisitionDetailsBL();
            DataSet oDSRequisitionItem
                = oRequisitionDetailsBL.GetDetailsOfRequisitionItem(iRequisitionID, sMode);
            DataTable oDTRequisitionItem = oDSRequisitionItem.Tables[0];
            DataTable oDTFlowDetails = oDSRequisitionItem.Tables[1];
            DataTable oDTIsGeneral = oDSRequisitionItem.Tables[2];

            if (oDSRequisitionItem != null && oDTIsGeneral.Rows.Count > 0)
            {
                if (Convert.ToInt32(oDTIsGeneral.Rows[0]["Is_General"]) == 1)
                    chkIsGeneral.Checked = true;
                else
                    chkIsGeneral.Checked = false;

                hidPrincipalUserId.Value = Convert.ToString(oDTIsGeneral.Rows[0]["PrincipalUserId"]);
            }
            FillRequisitionFlowDetails(oDTFlowDetails);
            SetFormAccordingToMode(oDTRequisitionItem);
        }
    }
    private void SetGenralRequisition()
    {
        string sCanCreateGeneralReqiusition = RequisitionDetailsBL.CanCreateGenralRequisition(miSchoolId, miUserId);
        if (sCanCreateGeneralReqiusition == Constants.S_YES)
            hidCanCreateGeneralRequisition.Value = Constants.S_YES;
        else
            hidCanCreateGeneralRequisition.Value = Constants.S_NO;
        SetIsGenralCheckbox();

    }

    /// <summary>
    /// This method is used to save the requisition details in the database.
    /// </summary>
    private void ManageRequisitionDetails(string asAction)
    {

        RequisitionDetailsBL oRequisitionDetailsBL = new RequisitionDetailsBL();

        if (LstVwReqItems.Items.Count <= Constants.I_ZERO)
        {
            oRequisitionDetailsBL.RequisitionID = Convert.ToInt32(hidRequisitionId.Value);
            oRequisitionDetailsBL.DeleteRequisitionDetails();
        }
        else
        {
            string sRequisitionItemXML = GenerateRequisitionItemXML(false);
            int iRequisitionId = 0;
            string sRequisitionDesc = Constants.S_EMPTY_STRING;

            if (hidRequisitionId.Value != null && hidRequisitionId.Value != Constants.S_EMPTY_STRING)
                iRequisitionId = Convert.ToInt32(hidRequisitionId.Value);
            sRequisitionDesc = txtDescription.Text.Trim();


            DataSet oDTRequisitionDetails
                = oRequisitionDetailsBL.InsertRequisitionDetails(miSchoolId, iRequisitionId, miUserId,
                                         msRequisitionName, sRequisitionDesc, sRequisitionItemXML, asAction, Convert.ToInt32(chkIsGeneral.Checked));

            ViewState[S_ITEM_DETAILS_DATA] = oDTRequisitionDetails.Tables[0];
            LstVwReqItems.DataSource = oDTRequisitionDetails.Tables[0];
            LstVwReqItems.DataBind();
            txtDescription.Text = oDTRequisitionDetails.Tables[0].Rows[0]["RequisitionDescription"].ToString();
            hidRequisitionId.Value = oDTRequisitionDetails.Tables[1].Rows[0]["RequisitionID"].ToString();
            hidReqCode.Value = oDTRequisitionDetails.Tables[1].Rows[0]["RequisitionCode"].ToString();
            txtReqName.Text = oDTRequisitionDetails.Tables[1].Rows[0]["RequisitionName"].ToString();

            //If requisition is send for approval that time only send message.
            if (asAction == S_REQUISITION_SEND && hidSendNotification.Value == Constants.S_YES)
            {
                hidUserID.Value = Constants.S_EMPTY_STRING;
                if (oDTRequisitionDetails.Tables[2] != null && oDTRequisitionDetails.Tables[2].Rows.Count > 0)
                {
                    for (int iCount = 0; iCount < oDTRequisitionDetails.Tables[2].Rows.Count; iCount++)
                        hidUserID.Value += oDTRequisitionDetails.Tables[2].Rows[iCount]["User_Id"].ToString() + ";";
                    hidUserID.Value = hidUserID.Value.Substring(0, hidUserID.Value.LastIndexOf(";"));

                    string sMessageBody = GetMessageBodyForNewRequisition();
                    SendMessageAboutAction(hidUserID.Value, S_NEW_REQ_SUB, sMessageBody);
                }
            }
        }
    }

    /// <summary>
    /// This method is used to display the requisition in view mode
    /// </summary>
    private void SetViewModeForRequisition()
    {
        if (chkIsGeneral.Checked)
        {
            trIsGeneral.Visible = true;
            chkIsGeneral.Enabled = false;
            chkIsGeneral.Visible = true;
        }
        else
        {
            trIsGeneral.Visible = false;
            chkIsGeneral.Visible = false;
        }

        ShowHideControls(true);

        //If hidStatusId.Value is 4(Waiting For My Approval) and login user having the rights of Final Approval.
        //Then he/she can approve or final approve any requisition because user is in the flow of requisition.
        //So both buttons FinalApproval as well as Approval are visibly true.
        if ((hidStatusId.Value == ((int)Constants.RequisitionStatus.Waiting_For_My_Approval).ToString()
            || hidStatusId.Value == ((int)Constants.RequisitionStatus.Pending).ToString())
            && mbIsFinalApproval == true)
        {
            trModify.Visible = true;
            tdModify.Visible = true;
            //This Condition Check is Login user is Principle or not if Ligin User Is Principal Then Approve button is hide
            if(hidPrincipalUserId.Value.ToInt() == miUserId)
                btnApproval.Visible = false;
            else
                btnApproval.Visible = true;
            txtComment.Enabled = true;
            trFinalApprove.Visible = true;
            btnFinalApproval.Visible = true;
            trHistory.Visible = true;
            spanComment.Visible = true;
            txtExpiryDate.Enabled = true;
            calExpiryDate.Enabled = true;
        }
       
        //If hidStatusId.Value is 4(Waiting For My Approval) and login user doesn't have the rights of Final Approval.
        //Then he/she can approve requisition in the flow of requisition.
        //So button Approval is visibly true.
        else if (hidStatusId.Value == ((int)(Constants.RequisitionStatus.Waiting_For_My_Approval)).ToString())
        {
            trModify.Visible = true;
            tdModify.Visible = true;
            btnApproval.Visible = true;
            txtComment.Enabled = true;
            spanComment.Visible = true;
            trHistory.Visible = true;
            txtExpiryDate.Enabled = true;
            calExpiryDate.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to hide and show the controls.
    /// </summary>
    private void ShowHideControls(bool abFlag)
    {
        trAction.Visible = abFlag;
        trLstItems.Visible = abFlag;
        trLstReqItems.Visible = abFlag;
        trLstItems.Visible = !abFlag;
        trSearch.Visible = !abFlag;
        trCategory.Visible = !abFlag;
        btnSave.Visible = !abFlag;
        btnSendReq.Visible = !abFlag;
        btnApproval.Visible = !abFlag;

        //For hide and show colummn header of delete button.
        HtmlTableRow oHtmlTableHeaderRow
            = (HtmlTableRow)LstVwReqItems.FindControl("trHeader");
        HtmlTableCell oHtmlTableCell
            = (HtmlTableCell)LstVwReqItems.FindControl("thDelete");

        if (oHtmlTableCell != null)
            oHtmlTableCell.Visible = !abFlag;

     
        txtComment.Enabled = !abFlag;
        txtDescription.Enabled = !abFlag;
        txtReqName.Enabled = !abFlag;
        spanComment.Visible = !abFlag;
        spanReqDescription.Visible = !abFlag;
        spanReqName.Visible = !abFlag;

        txtExpiryDate.Enabled = !abFlag;
        calExpiryDate.Enabled = !abFlag;
    }

    /// <summary>
    /// This method is used to display the requisition in edit mode
    /// </summary>
    private void SetEditModeForRequisition()
    {
        SetIsGenralCheckbox();
        ShowHideControls(false);
        trLstReqItems.Visible = true;
        trLstItems.Visible = true;
        btnSave.Enabled = false;
        btnSendReq.Enabled = false;

        //Check requisition is not pending and Not waiting for logging user approval
        //So hide the approve,denied and final approve buttons
        //and disabled save button according to status of requisition.
        if (hidStatusId.Value != ((int)Constants.RequisitionStatus.Pending).ToString()
            && hidStatusId.Value != ((int)Constants.RequisitionStatus.Waiting_For_My_Approval).ToString())
        {
            if (hidStatusId.Value == ((int)Constants.RequisitionStatus.My_Requisition).ToString()
                && hidNextDesignationId.Value != Constants.S_EMPTY_STRING
                && hidNextDesignationId.Value != null)
                btnSendReq.Enabled = true;
            else if (hidNextDesignationId.Value != Constants.S_EMPTY_STRING
                && hidNextDesignationId.Value != null)
            {
                btnSave.Enabled = false;
                btnSendReq.Enabled = false;
            }
            else if ((hidStatusId.Value == ((int)Constants.RequisitionStatus.Denied).ToString())
                && (hidNextDesignationId.Value == Constants.S_EMPTY_STRING
                || hidNextDesignationId.Value == null))
            {
                trAction.Visible = true;
                txtComment.ReadOnly = true;
                btnApproval.Visible = false;
                btnSave.Enabled = false;
                btnSendReq.Enabled = true;
            }
            else
            {
                btnSave.Enabled = true;
                btnSendReq.Enabled = true;
            }
        }
        else if ((hidStatusId.Value == ((int)Constants.RequisitionStatus.Waiting_For_My_Approval).ToString()
            || hidStatusId.Value == ((int)Constants.RequisitionStatus.Pending).ToString())
            && mbIsFinalApproval == true)
        {
            trAction.Visible = true;
            btnFinalApproval.Visible = true;
            trFinalApprove.Visible = true;
        }
        else if (hidStatusId.Value == ((int)Constants.RequisitionStatus.Waiting_For_My_Approval).ToString())
            trAction.Visible = true;
        else
        {
            btnSendReq.Enabled = true;
            trAction.Visible = false;
        }

        HtmlTableRow oHtmlTableHeaderRow
              = (HtmlTableRow)LstVwReqItems.FindControl("trHeader");
        HtmlTableCell oHtmlOrgQty
            = (HtmlTableCell)oHtmlTableHeaderRow.FindControl("thorgQty");
        if (oHtmlOrgQty != null)
            oHtmlOrgQty.Visible = false;
    }

    /// <summary>
    /// This method is used to preparing a querystring.
    /// </summary>
    private string PrepareQueryString()
    {
        string sQueryString = Constants.S_EMPTY_STRING;
        sQueryString = "StatusID=" + hidStatusId.Value;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());

        return sEncrypt;
    }

    /// <summary>
    /// This method is used to fill requisition flow details.
    /// </summary>
    private void FillRequisitionFlowDetails(DataTable oDTFlowDetails)
    {
        lstvwRequisitionWorkFlow.DataSource = oDTFlowDetails;
        lstvwRequisitionWorkFlow.DataBind();
        for (int iCount = 0; iCount < oDTFlowDetails.Rows.Count; iCount++)
        {
            if (Convert.ToInt32(oDTFlowDetails.Rows[iCount]["User_Id"]) != Constants.I_ZERO)
                hidUserID.Value += oDTFlowDetails.Rows[iCount]["User_Id"].ToString() + ";";
        }
        hidUserID.Value = hidUserID.Value.Substring(0, hidUserID.Value.LastIndexOf(";"));
    }

    /// <summary>
    /// This method is used to set the userid for message 
    /// </summary>
    private string GetUserIdForMessage(bool abSendAll)
    {
        string sUserId = hidUserID.Value;
        if (!abSendAll && sUserId.Contains(";"))
            sUserId = sUserId.Substring(0, sUserId.IndexOf(";"));
        return sUserId;
    }

    /// <summary>
    /// This method is used to send the message about the action of the requisition.
    /// </summary>
    private void SendMessageAboutAction(string asUserId, string sMsgSubject, string sMsgBody)
    {
        Message oMessage = new Message();
        oMessage.sMessageBody = sMsgBody;
        oMessage.sMessageSubject = sMsgSubject;
        oMessage.SetMessageReceivers(asUserId, miUserId);
        oMessage.InsertMessageDetails(miUserId, moUserRole.ToInt(), miAcademicYearId);
    }

    /// <summary>
    /// This method is used set  
    /// </summary>
    private void InitailizeForm()
    {
        valRegNumber.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lblErrorMsg.Visible = false;
        lblMessage.Visible = false;
        string sQueryString = PrepareQueryString();
        btnBack.Attributes.Add("onclick", "window.open('../Inventory/RequisitionListUI.aspx?" + sQueryString
                                                     + " ' , '_self');return false;");

        btnApproval.Attributes.Add("onclick", "if(!ConfirmApproved()) {return false;}");
        btnFinalApproval.Attributes.Add("onclick", "if(!ConfirmFinalApproved()) {return false;}");
    }

    /// <summary>
    /// This method is used assinging the value to the proprties of the class RequisitionDetailsBL  
    /// </summary>
    private RequisitionDetailsBL SetRequisitionDetailsBL()
    {
        RequisitionDetailsBL oRequisitionDetailsBL = new RequisitionDetailsBL();
        oRequisitionDetailsBL.SchoolId = miSchoolId;
        oRequisitionDetailsBL.User_Id = miUserId;
        oRequisitionDetailsBL.RequisitionID = Convert.ToInt32(hidRequisitionId.Value);
        oRequisitionDetailsBL.Comment = txtComment.Text.Trim();

        if (txtExpiryDate.Text != string.Empty)
            oRequisitionDetailsBL.ExpiryDate = txtExpiryDate.Text.ToDateTime();
        else
            oRequisitionDetailsBL.ExpiryDate = DateTime.MinValue;

        return oRequisitionDetailsBL;
    }

    /// <summary>
    /// This method is used to approve the requisition.
    /// </summary>
    private DataTable ApproveRequisition(bool abIsFinalApproval)
    {
        bool bModified;
        int iRequisitionStatusID;
        DataTable oDTUserId = null;
        RequisitionDetailsBL oRequisitionDetailsBL = SetRequisitionDetailsBL();

        foreach (ListViewDataItem oListViewDataItem in LstVwReqItems.Items)
        {
            CheckBox chkIsSelected = (CheckBox)(oListViewDataItem.FindControl("ChkIsRequisitionToDeny"));
            if (chkIsSelected.Checked == true)
                I_DENY_COUNT += 1;
        }

        if (I_DENY_COUNT != LstVwReqItems.Items.Count)
        {
            //If all items deleted from requisition then requisition should deleted.
            if (LstVwReqItems.Items.Count <= 0)
                oDTUserId = oRequisitionDetailsBL.DeleteRequisitionDetails();
            else
            {
                 string sRequisitionDesc = txtDescription.Text.Trim();
                 string sRequisitionName = txtReqName.Text.Trim();

                if (I_DENY_COUNT == 0 && hidIsRequisitionModified.Value != Constants.C_YES.ToString())
                {
                    bModified = false;
                    iRequisitionStatusID  = Convert.ToInt32(S_STATUS_APPROVED_ID);
                    oDTUserId = oRequisitionDetailsBL.ApprovedRequisition(bModified, abIsFinalApproval, iRequisitionStatusID);

                }
                else if (I_DENY_COUNT == 0 && hidIsRequisitionModified.Value == Constants.C_YES.ToString())
                {
                    bModified = true;
                    string sRequisitionItemXML = GenerateRequisitionItemXML(true);
                    iRequisitionStatusID = Convert.ToInt32(S_STATUS_APPROVED_ID);
                    oDTUserId = oRequisitionDetailsBL.ApprovedRequisition(bModified, abIsFinalApproval, iRequisitionStatusID, sRequisitionName, sRequisitionDesc, sRequisitionItemXML);
                }
                else
                {
                    bModified = true;
                    string sRequisitionItemXML = GenerateRequisitionItemXML(true);
                    iRequisitionStatusID = Convert.ToInt32(S_STATUS_PARTIALLYAPPROVED_ID);
                    oDTUserId = oRequisitionDetailsBL.ApprovedRequisition(bModified, abIsFinalApproval, iRequisitionStatusID, sRequisitionName, sRequisitionDesc, sRequisitionItemXML);
                }
            }
        }
        else if (I_DENY_COUNT == LstVwReqItems.Items.Count)
            oDTUserId = oRequisitionDetailsBL.DeniedRequisition();
        
        return oDTUserId;
    }

    /// <summary>
    /// This method is used sending the mail about the approval.
    /// </summary>
    private void SendMailForApproval(DataTable oDTUserId)
    {
        string sMessageBody;
        string sUserID;

        if (I_DENY_COUNT != LstVwReqItems.Items.Count)
        {
            if (I_DENY_COUNT == 0 && hidIsRequisitionModified.Value != Constants.C_YES.ToString())
            {
                sMessageBody = S_REQUISITION_APPROVE;
                sUserID = GetUserIdForMessage(false);
            }
            else if (I_DENY_COUNT == 0 && hidIsRequisitionModified.Value == Constants.C_YES.ToString())
            {
                sMessageBody = S_REQUISITION_MODIFYAPPROVE;
                sUserID = GetUserIdForMessage(false);
            }
            else
            {
                sMessageBody = S_REQUISITION_PARTIALLYAPPROVE;
                sUserID = GetUserIdForMessage(true);
            }

            sMessageBody = sMessageBody.Replace("%Code%", hidReqCode.Value);
            sMessageBody = sMessageBody.Replace("%ApprovalName%", Convert.ToString(Session[Constants.S_SESSION_USER_FULLNAME]));
            SendMessageAboutAction(sUserID, S_APPROVE_MESSAGE_SUBJECT, sMessageBody);

            hidUserID.Value = Constants.S_EMPTY_STRING;
            if (oDTUserId.Rows.Count > 0)
            {
                for (int iCount = 0; iCount < oDTUserId.Rows.Count; iCount++)
                    hidUserID.Value += oDTUserId.Rows[iCount]["User_Id"].ToString() + ";";
                hidUserID.Value = hidUserID.Value.Substring(0, hidUserID.Value.LastIndexOf(";"));
                sMessageBody = S_REQUISITION_FOR_APPROVAL;

                sMessageBody = sMessageBody.Replace("%Code%", hidReqCode.Value);
                sMessageBody = sMessageBody.Replace("%ApprovalName%", Convert.ToString(Session[Constants.S_SESSION_USER_FULLNAME]));
                sMessageBody = sMessageBody.Replace("%Creater%", hidCreatorName.Value);
                SendMessageAboutAction(hidUserID.Value, S_NEW_REQ_SUB, sMessageBody);
            }
        }
        else if (I_DENY_COUNT == LstVwReqItems.Items.Count)
        {
            sMessageBody = S_REQUISITION_DENIED;
            sMessageBody = sMessageBody.Replace("%Code%", hidReqCode.Value);
            sMessageBody = sMessageBody.Replace("%DeniedlName%", Convert.ToString(Session[Constants.S_SESSION_USER_FULLNAME]));
            sUserID = GetUserIdForMessage(true);
            SendMessageAboutAction(sUserID, S_DENIED_MESSAGE_SUBJECT, sMessageBody);
        }
    }

    private void SetEditModeToApprover()
    {
        hidIsRequisitionModified.Value = Constants.S_YES;
        DataTable oDTItemsDetails;
        oDTItemsDetails = (DataTable)ViewState[S_ITEM_DETAILS_DATA];
        for (int iRowIndex = Constants.I_ZERO; iRowIndex < oDTItemsDetails.Rows.Count; iRowIndex++)
            oDTItemsDetails.Rows[iRowIndex][S_CAN_EDIT] = true;

        DataView oDTItemView = oDTItemsDetails.DefaultView;
        LstVwReqItems.DataSource = oDTItemView;
        ViewState[S_ITEM_DETAILS_DATA] = oDTItemsDetails;
        LstVwReqItems.DataBind();

        SetEditModeForRequisition();

        chkIsGeneral.Enabled = false;
        tdModify.Visible = false;
        btnCancel.Visible = true;
        txtItemCode.Text = Constants.S_EMPTY_STRING;
        txtItemCode.Enabled = true;
        cmbCategory.Enabled = true;
        trLstItems.Visible = false;
        btnSearch.Text = "Search";

        HtmlTableRow oHtmlTableHeaderRow
          = (HtmlTableRow)LstVwReqItems.FindControl("trHeader");
        HtmlTableCell oHtmlOrgQty
            = (HtmlTableCell)oHtmlTableHeaderRow.FindControl("thorgQty");
        if (oHtmlOrgQty != null)
            oHtmlOrgQty.Visible = false;

        HtmlInputCheckBox chkIsSelected = (HtmlInputCheckBox)(LstVwReqItems.FindControl("ChkDenySelectAll"));
        if (chkIsSelected != null)
            chkIsSelected.Checked = false;
    }


    private void CancelModificationOfApprover()
    {
        hidIsRequisitionModified.Value = Constants.S_NO;
        int iRequisitionID = Convert.ToInt32(hidRequisitionId.Value);
        string sMode = hidRequisitionMode.Value;
        RequisitionDetailsBL oRequisitionDetailsBL = new RequisitionDetailsBL();
        DataSet oDSRequisitionItem = oRequisitionDetailsBL.GetDetailsOfRequisitionItem(iRequisitionID, sMode);

        LstVwReqItems.DataSource = oDSRequisitionItem.Tables[0];
        ViewState[S_ITEM_DETAILS_DATA] = oDSRequisitionItem.Tables[0];
        LstVwReqItems.DataBind();
        SetViewModeForRequisition();
        btnCancel.Visible = false;
        txtComment.Text = hidComment.Value;
        hidRequisitionItemCount.Value = LstVwReqItems.Items.Count.ToString();

        HtmlTableRow oHtmlTableHeaderRow
          = (HtmlTableRow)LstVwReqItems.FindControl("trHeader");
        HtmlTableCell oHtmlOrgQty
            = (HtmlTableCell)oHtmlTableHeaderRow.FindControl("thorgQty");
        if (oHtmlOrgQty != null)
            oHtmlOrgQty.Visible = true;
    }

    private void SetFormAccordingToMode(DataTable oDTRequisitionItem)
    {
        if (oDTRequisitionItem.Rows.Count > Constants.I_ZERO)
        {
            const string S_MODE_EDIT = "Edit";
            ViewState[S_ITEM_DETAILS_DATA] = oDTRequisitionItem;
            LstVwReqItems.DataSource = oDTRequisitionItem;
            LstVwReqItems.DataBind();
            if (oDTRequisitionItem.Rows[0]["ActionComment"] != DBNull.Value)
            {
                txtComment.Text = oDTRequisitionItem.Rows[0]["ActionComment"].ToString();
                hidComment.Value = oDTRequisitionItem.Rows[0]["ActionComment"].ToString();

                if (oDTRequisitionItem.Rows[0]["ExpiryDate"] != DBNull.Value)
                    txtExpiryDate.Text = oDTRequisitionItem.Rows[0]["ExpiryDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    txtExpiryDate.Text = string.Empty;
            }
            txtDescription.Text = oDTRequisitionItem.Rows[0]["RequisitionDescription"].ToString();
            txtReqName.Text = oDTRequisitionItem.Rows[0]["RequisitionName"].ToString();
            
            if (hidRequisitionMode.Value == S_MODE_EDIT)

                SetEditModeForRequisition();
            else
                SetViewModeForRequisition();
        }
    }

    private string GetMessageBodyForNewRequisition()
    {
        string sMessageBody = S_NEW_REQUISITION_MESSAGE;
        sMessageBody = sMessageBody.Replace("%Code%", hidReqCode.Value);
        sMessageBody = sMessageBody.Replace("%Creater%", Session[Constants.S_SESSION_USER_FULLNAME].ToString());
        return sMessageBody;
    }

    private void UpdateItemQty()
    {
        if (ViewState[S_ITEM_DETAILS_DATA] != null)
        {
            DataTable oDTItemsDetails;
            oDTItemsDetails = (DataTable)ViewState[S_ITEM_DETAILS_DATA];
            bool bFlag;
            int iCount;

            foreach (ListViewDataItem oListViewDataItem in LstVwReqItems.Items)
            {
                int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
                TextBox otxtQty = (TextBox)oListViewDataItem.FindControl("txtQty");

                DropDownList cmbUnit = oListViewDataItem.FindControl("cmbUnits") as DropDownList;

                int iUOMPieceCount = LstVwReqItems.DataKeys[oListViewDataItem.DisplayIndex]["UOMPieceCount"].ToInt();

                Label olblItemName = (Label)oListViewDataItem.FindControl("lblItemName");
                int iItemId = Convert.ToInt32(LstVwReqItems.DataKeys[iRowId]["ItemID"]);
                bFlag = false;

                for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_ITEM_ID]) == iItemId)
                    {
                        bFlag = true;
                        break;
                    }
                }
                if (bFlag)
                {
                    DataRow oDTRow = oDTItemsDetails.NewRow();
                    oDTRow = oDTItemsDetails.Rows[iCount];
                    oDTRow.BeginEdit();

                    if (cmbUnit.SelectedValue == Constants.S_ONE)
                        oDTRow[S_ITEM_QUANTITY] = Convert.ToDouble(otxtQty.Text);
                    else
                        oDTRow[S_ITEM_QUANTITY] = Convert.ToDouble(otxtQty.Text) * iUOMPieceCount;

                    oDTRow[S_ITEM_ORG_QUANTITY] = 0;
                    oDTRow[S_CONSIDER_UNIT_QUANTITY] = (cmbUnit.SelectedValue == Constants.S_ONE ? "True" : "False");
                    oDTItemsDetails.AcceptChanges();
                    oDTItemsDetails.Rows[iCount].EndEdit();
                    ViewState[S_ITEM_DETAILS_DATA] = oDTItemsDetails;
                }
            }
        }
    }

    /// <summary>
    /// This method is used to add attribute to link to open refund fee pop up.
    /// </summary>
    private void OpenIssueHistoryPopUp()
    {
        string sQueryString = "RequisitionID=" + hidRequisitionId.Value +
                              "&CreaterId=" + hidCreatorID.Value;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        lnkHistory.NavigateUrl = "ItemIssueHistory.aspx?";
        lnkHistory.NavigateUrl = lnkHistory.NavigateUrl + sEncrypt;
        lnkHistory.Attributes.Add("onclick", "window.open('" + lnkHistory.NavigateUrl
                                + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500'); return false;");
    }

    private void SetIsGenralCheckbox()
    {
        if (hidCanCreateGeneralRequisition.Value == Constants.S_YES)
        {
            trIsGeneral.Visible = true;
            chkIsGeneral.Visible = true;
            chkIsGeneral.Enabled = true;
        }
        else
        {
            trIsGeneral.Visible = false;
            chkIsGeneral.Visible = false;
            chkIsGeneral.Enabled = false;
        }
    }

    private void HidControl()
    {
        if (hidRequisitionMode.Value == "View")
        {
            txtComment.Enabled = false;
            txtDescription.Enabled = false;
            txtReqName.Enabled = false;
            spanComment.Visible = false;
            spanReqDescription.Visible = false;
            spanReqName.Visible = false;
            txtExpiryDate.Enabled = false;
            calExpiryDate.Enabled = false;            
        }
        else
        {
            txtComment.Enabled = true;
            txtDescription.Enabled = true;
            txtReqName.Enabled = true;
            spanComment.Visible = true;
            spanReqDescription.Visible = true;
            spanReqName.Visible = true;
            txtExpiryDate.Enabled = true;
            calExpiryDate.Enabled = true;
        }
    }

    private void SetJavaScriptAttribute()
    {
        ApplyMouseHoverEffect(new List<Button> { btnApproval, btnBack, btnCancel, btnFinalApproval, btnModify, btnSave, btnSearch, btnSendReq, btnYes, btnNo, btnCancelOp });

        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            btnSendReq.Attributes.Add("onclick", "OpenConfirmationPopup(); return false;");
        else
            hidSendNotification.Value = Constants.S_YES;

        btnCancelOp.Attributes.Add("onclick", "HideConfirmationPopup(); return false;");
        btnAddItem.Attributes.Add("onclick", string.Format("window.open('../Inventory/ItemDetailsUI.aspx?' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1320,height=650').focus(); return false;"));
    }

    /// <summary>
    /// This method is used to display image.
    /// </summary>
    /// <param name="aiidResult"></param>
    /// <param name="aibImgItem"></param>
    private void DisplayItemImage(List<ItemImageDetails> lstItemImageDetails, int aiControlId, Image aibImgItem)
    {
        ItemImageDetails result = (from Item in lstItemImageDetails where Item.ControlId == aiControlId select Item).FirstOrDefault();
        if (result != null)
        {
            aibImgItem.Visible = true;
            string sNewFileName = S_FOLDER_PATH + result.ImageUrl;
            aibImgItem.ImageUrl = sNewFileName;
            aibImgItem.Attributes.Add("onclick", "window.open('" + sNewFileName + "', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1000,height=700'); return false;");
        }
        else
        {
            aibImgItem.Visible = false;
        }
    }

   /// <summary>
   /// This method is used to hide fields.
   /// </summary>
    private void HideFields()
    {
        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
        {
            if (moUserRole == Constants.UserRoles.Teacher)
                trIsGeneral.Visible = false;

            trReqName.Visible = false;
        }
        else
        {    
            trIsGeneral.Visible = true;
            trReqName.Visible = true;
        }

    }

    private void RequisitionExpiry()
    {
        if (Settings.ShowRequisitionExpiryDate)
        {
            txtExpiryDate.Text = DateTime.Now.AddDays(Settings.RequisitionExpiryDaysCount).ToString(Constants.S_DATE_FORMAT);
        }
    }

    #endregion

    protected void ItemQuantity_Validate(object sender, ServerValidateEventArgs e)
    {
        List<RequisitionData> lstRequisitionData = new List<RequisitionData>();
        foreach (ListViewItem item in LstVwReqItems.Items)
        {
            if (item.ItemType == ListViewItemType.DataItem)
            {
                int iItemId = LstVwReqItems.DataKeys[item.DisplayIndex]["ItemID"].ToInt();
                TextBox txtQty = item.FindControl("txtQty") as TextBox;

                if (txtQty.Visible)
                {
                    RequisitionData oRequisitionData = new RequisitionData();
                    oRequisitionData.ItemId = iItemId;
                    oRequisitionData.Quantity = (txtQty.Text == string.Empty?0 : txtQty.Text.ToDecimal());

                    lstRequisitionData.Add(oRequisitionData);
                }
            }
        }

        if (lstRequisitionData.Count > 0)
        {
            string sData = this.GenerateXML(lstRequisitionData);

            RequisitionDetailsBL oRequisitionDetailsBL = new RequisitionDetailsBL();
            string sCodes = oRequisitionDetailsBL.ValidateItemQuantity(miSchoolId, sData);

            if (sCodes != string.Empty)
            {
                CustomValidator obj = sender as CustomValidator;
                obj.ErrorMessage = "Item quantity should not be greater than current stock for item with code : " + sCodes;

                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }
        else
            e.IsValid = true;
    }

    public class RequisitionData
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
