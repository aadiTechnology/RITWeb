// File Name  : PurchaseOrderDetailsUI.aspx.cs
// Created By : Milind
// Date       : 14/7/2009
// Description : This class is used to create and modify the purchase order.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;

public partial class PurchaseOrderDetailsUI : SchoolBase
{

    #region Constants

    const string S_LISTVIEW_DATASOURCE = "RequisitionItemQtyDataSource";

    const string S_DB_COLUMN_ITEM_ID = "ItemID";
    const string S_DB_COLUMN_REQUISITION_ID = "RequisitionID";
    const string S_DB_COLUMN_ITEM_PO_QUANTITY = "ItemPOQty";
    const string S_DB_COLUMN_ITEM_PO_PRICE = "ItemPrice";
    const string S_DB_COLUMN_ITEM_QUANTITY = "OriginalQtyUnit";
    const string S_DB_COLUMN_ITEM_DIFF = "ItemQtyDiff";
    const string S_DB_COLUMN_ITEM_NAME = "ItemName";
    const string S_DB_COLUMN_ITEM_CODE = "ItemCode";
    const string S_DB_COLUMN_REQUISITION_CODE = "RequisitionCode";
    const string S_ITEM_UNIT = "Unit";
    const string S_UOM_UNITS = "UOMUnits";
    const string S_UOM_UNIT_COUNT = "UOMUnitCount";
    const string S_ITEM_ADDED = "Added in PO";
    const string S_COMMAND_REMOVE = "Remove";
    const string S_COMMAND_ADD = "Add";
    const string S_COMMAND_DETAILS = "Details";
    const string S_COMMAND_MODIFY = "Modify";
    const string S_DEFAULT_SORT_EXP_ITEM = "ItemCode";
    const string S_DEFAULT_SORT_EXP_REQUISITION = "RequisitionCode";
    const string S_TEXT_SEARCH = "Search";
    const string S_TEXT_CHANGE_INPUT = "Change Input";
    const string S_SAVE = "Save";
    const string S_DELETE = "Delete";
    const string S_SAVE_MESSAGE = "PO Details saved successfully.!!!";
    const int I_PO_NAME_LENGTH = 40;


    #endregion

    #region Structure

    private struct POItemsDetailsStruct
    {
        public int miItemID;
        public int miRequisitionID;
        public double mdPOQty;
        public double mdOriginalQty;
        public string msUOM;
        public string msItemCode;
        public string msItemName;
        public string msRequisitionCode;
        public string msItemUnit;
        public int miPieceCount;
        public double mdItemPrice;
    }

    #endregion

    #region Data Members

    //int miItemID = 0;
    //int miRequisitionID = 0;
    //double mdPOQty = 0;
    double mdOriginalQty = 0;
    string msItemCode = string.Empty;
    string msItemName = string.Empty;
    string msRequisitionCode = string.Empty;
    string msItemUnit = string.Empty;

    #endregion

    #region Events

    #region Page Events

    /// <summary>
    /// This event is used to set page according to purchase order Id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ReadQueryString();
            if (!IsPostBack)
            {
                SetValSummaryHeaderAndAttributes();
                InitializeForm();
            }

            btnSave.Attributes.Add("onclick", "if(!AllConfirmDelete(" + lstVwPurchaseOrder.Items.Count + ")){return false;}");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view lstvwItemsOfRequisitions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optItemWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //If the radio button item wise is checked that time
            //All other controls related to other two radio buttons get visible false.
            //Also hide the Add All button also.

            if (optItemWise.Checked)
            {
                lstvwItemsOfRequisitions.DataSourceID = lstDSobj.ID;

                tblItems.Visible = true;
                tblReqItems.Visible = false;
                tblSearch.Visible = false;
                trLstReqItems.Visible = false;
                tblAddAll.Visible = false;
            }
            else
                tblItems.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view LstVwRquisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optReqWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //If the radio button requisition wise is checked that time
            //All other controls related to other two radio buttons get visible false.
            //Also hide the Add All button also.
            if (optReqWise.Checked)
            {
                LstVwRquisition.DataSourceID = objlstVwReq.ID;

                tblItems.Visible = false;
                tblReqItems.Visible = true;
                tblSearch.Visible = false;
                txtItemCode.Text = Constants.S_EMPTY_STRING;
                txtItemCode.Enabled = true;
                trLstItems.Visible = false;
                trLstReqItems.Visible = false;
                tblAddAll.Visible = false;
            }
            else
                tblReqItems.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add individual items in PO And provide the facility of serach items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optIndividual_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //If the radio button individual wise is checked that time
            //All other controls related to other two radio buttons get visible false.
            //Also hide the Add All button also.
            if (optIndividual.Checked)
            {
                tblItems.Visible = false;
                tblReqItems.Visible = false;
                tblSearch.Visible = true;
                trLstItems.Visible = false;
                txtItemCode.Text = string.Empty;
                btnSearch.Text = S_TEXT_SEARCH;
                btnSearch.Enabled = true;
                txtItemCode.Enabled = true;
                tblAddAll.Visible = false;
                trLstReqItems.Visible = false;
            }
            else
            {
                btnSearch.Text = S_TEXT_SEARCH;
                tblSearch.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search the item according to value entered in the Textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //When button text is Search that means there is new text for searching by item code/name
            //So search is accoding to that and change the text of button to Change Input.
            //and disabled the related controls.  
            if (btnSearch.Text == S_TEXT_SEARCH)
            {
                btnSearch.Text = S_TEXT_CHANGE_INPUT;
                txtItemCode.Enabled = false;
                trLstItems.Visible = true;                

                LstVwIndividualItem.DataSourceID = objlstIndividual.ID;
                LstVwIndividualItem.DataBind();
            }
            //When button text is Change Input that means User wants to search by another text.
            //So hide previously searched result enabled all controls.
            //Change the text of button to Search.  
            else
            {
                txtItemCode.Text = Constants.S_EMPTY_STRING;
                txtItemCode.Enabled = true;
                LstVwIndividualItem.DataSourceID = null;
                trLstItems.Visible = false;
                btnSearch.Text = S_TEXT_SEARCH;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add all the items along with quantity from the listview(LstVwAppReqItems)to the viewstate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        try
        {            
            foreach (ListViewDataItem oListViewDataItem in LstVwAppReqItems.Items)
            {
                int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
                TextBox txtQuantity = oListViewDataItem.FindControl("txtQty") as TextBox;
                DropDownList cmbUnits = oListViewDataItem.FindControl("cmbUnits") as DropDownList;
                Label lblItemName = oListViewDataItem.FindControl("lblItemName") as Label;
                Label lblItemCode = oListViewDataItem.FindControl("lblItemCode") as Label;
                Label lblRequisitionCode = oListViewDataItem.FindControl("lblRequisitionCode") as Label;
                Label lblUnit = oListViewDataItem.FindControl("lblOrgQtyUnit") as Label;
                HiddenField ohidActualQty = oListViewDataItem.FindControl("hidActualQty") as HiddenField;
                LinkButton lnkbtnRemove = oListViewDataItem.FindControl("lnkbtnRemove") as LinkButton;
                TextBox txtItemPrice = oListViewDataItem.FindControl("txtItemPrice") as TextBox;

                DropDownList cmbUnt = oListViewDataItem.FindControl("cmbUnt") as DropDownList;

                if (txtQuantity.Text != Constants.S_EMPTY_STRING && txtQuantity.Text != Constants.S_ZERO && txtItemPrice.Text != Constants.S_EMPTY_STRING && txtItemPrice.Text != Constants.S_ZERO)
                {
                    POItemsDetailsStruct oPOItemsDetailsStruct = new POItemsDetailsStruct();

                    oPOItemsDetailsStruct.miItemID = Convert.ToInt32(LstVwAppReqItems.DataKeys[iRowId]["ItemID"]);
                    oPOItemsDetailsStruct.miRequisitionID = Convert.ToInt32(LstVwAppReqItems.DataKeys[iRowId]["RequisitionID"]);
                    oPOItemsDetailsStruct.msItemCode = lblItemCode.Text.Trim();
                    oPOItemsDetailsStruct.msItemName = lblItemName.Text.Trim();
                    oPOItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(ohidActualQty.Value);
                    
                    oPOItemsDetailsStruct.msRequisitionCode = lblRequisitionCode.Text.Trim();
                    //oPOItemsDetailsStruct.msItemUnit = cmbUnits.SelectedItem.Text;
                    oPOItemsDetailsStruct.msItemUnit = lblUnit.Text;
                    oPOItemsDetailsStruct.msUOM = cmbUnits.SelectedValue;
                    oPOItemsDetailsStruct.miPieceCount = Convert.ToInt32(LstVwAppReqItems.DataKeys[iRowId]["PieceCount"]);
                    
                    double dbQuantity = Convert.ToDouble(txtQuantity.Text.Trim());
                    if (oPOItemsDetailsStruct.msUOM == Constants.S_ONE)
                        oPOItemsDetailsStruct.mdPOQty = dbQuantity;
                    else
                        oPOItemsDetailsStruct.mdPOQty = dbQuantity * oPOItemsDetailsStruct.miPieceCount;

                    oPOItemsDetailsStruct.mdItemPrice = txtItemPrice.Text.ToDouble();
                    AddItemsQtyToDataTable(oPOItemsDetailsStruct);

                    lnkbtnRemove.Visible = true;
                }
            }
            FillPOItemListView();
            if (lstvwItemsOfRequisitions.Visible == true)
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to modify the existing PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {
            SetFormInNewMode();
            hidReadOnly.Value = Constants.S_NO;
            btnCancel.Visible = true;
            btnSave.Visible = true;
            if (hidIsFromApproverSCreen.Value == Constants.S_NO)
            {
                btnSubmit.Visible = true;
                btnSubmit.Enabled = false;

                if (hidPOId.Value != Constants.S_ZERO)
                    btnSubmit.Enabled = true;
            }
            else
                btnSubmit.Visible = false;
            tblModify.Visible = false;
            trPOTypes.Visible = false;            
            FillPOItemListView();
            lstvwItemsOfRequisitions.Visible = false;
            txtDescription.Enabled = true;
            txtDescription.ReadOnly = false;
            lblStar.Visible = true;
            tblSearch.Visible = true;
            trSearch.Visible = false;
            trPODetails.Visible = true;
            rdoPurchase.Enabled = true;
            rdoWork.Enabled = true;
            cmbVendors.Enabled = true;
            cmbHeader.Enabled = true;
            cal_PODeliveryDate.Enabled = true;
            txtPONote.Enabled = true;
            txtAmountDiscount.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancelling the modification is done on the existing PO
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            InitializeForm();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save PO details in the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iPOId = Convert.ToInt32(hidPOId.Value);

            if (lstVwPurchaseOrder.Items.Count > Constants.I_ZERO)
            {
                FillPOItemListView();
                string sXmlPOItems = GeneratePORequisitionItemXML();
                string sXmlPOReqItems = GeneratePOItemXML();
                string sPODesc = txtDescription.Text.Trim();

                bool abOrderType = false;

                if (rdoPurchase.Checked)
                    abOrderType = true;                

                PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
                int aiPOIdForSubmit = Constants.I_ZERO;
                int aiPODiscount = Constants.I_ZERO;
                if (txtAmountDiscount.Text != string.Empty)
                    aiPODiscount = txtAmountDiscount.Text.ToInt();

                oPurchaseOrderBL.InsertPurchaseOrderDetails(miSchoolId, miUserId, hidPOName.Value, sPODesc, sXmlPOItems, sXmlPOReqItems, iPOId, abOrderType, cmbVendors.SelectedValue.ToInt(), cmbHeader.SelectedValue.ToInt(), txtPODeliveryDate.Text.ToDateTime(), txtPONote.Text.Trim(), aiPODiscount, out aiPOIdForSubmit);

                if (hidIsFromApproverSCreen.Value == Constants.S_NO)
                    hidPOId.Value = aiPOIdForSubmit.ToString();

                if (hidIsFromApproverSCreen.Value == Constants.S_YES)
                {
                    string sQueryString = "&POId=" + hidPOId.Value + "&StatusId=" + hidPOStatusId.Value + "&IsFromApproverScreen=" + hidIsFromApproverSCreen.Value;
                    string sEncryptedQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                    MasterPage oMasterPage = this.Master as MasterPage;
                    oMasterPage.RedirectToNextPage("~/RITeSchool/Inventory/PurchaseOrderListUI.aspx?" + sEncryptedQueryString);
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = S_SAVE_MESSAGE;
                    btnSubmit.Enabled = true;
                }
            }
            else
            {
                PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
                oPurchaseOrderBL.DeletePurchaseOrderDetails(iPOId, miSchoolId, miUserId);

                string sQueryString = "&POId=" + hidPOId.Value + "&StatusId=" + hidPOStatusId.Value + "&IsFromApproverScreen=" + hidIsFromApproverSCreen.Value;
                string sEncryptedQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                MasterPage oMasterPage = this.Master as MasterPage;
                oMasterPage.RedirectToNextPage("~/RITeSchool/Inventory/PurchaseOrderListUI.aspx?" + sEncryptedQueryString);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit the PO details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
            oPurchaseOrderBL.ApprovePurchaseOrder(miSchoolId, hidPOId.Value.ToInt(), miUserId);

            string sQueryString = "POId=" + hidPOId.Value + "&StatusId=" + hidPOStatusId.Value + "&IsFromApproverScreen=" + hidIsFromApproverSCreen.Value;
            string sEncryptedQueryString = CommonUtility.EncryptQuerystring(sQueryString);
            MasterPage oMasterPage = this.Master as MasterPage;
            oMasterPage.RedirectToNextPage("~/RITeSchool/Inventory/PurchaseOrderListUI.aspx?" + sEncryptedQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region ListView Events

    #region ItemWise

    /// <summary>
    /// This event is used to fill the drop down list in the listview datapager according to pagesize.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemsOfRequisitions_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItemsOfRequisitions.Items.Count > Constants.I_ZERO)
            {
                ControlUtility.FillListViewPagerFooter(lstvwItemsOfRequisitions, DtPgCount);
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
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
    /// This event is used to fill the list view according to the selected pageindex in the combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItemsOfRequisitions);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort list view items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemsOfRequisitions_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to add or remove the items details from PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemsOfRequisitions_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_ADD)
            {
                LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

                int iItemID = Convert.ToInt32(lstvwItemsOfRequisitions.DataKeys[iRowId]["ItemID"]);

                RequisitionBL oRequisitionBL = new RequisitionBL();
                int iPOId = Convert.ToInt32(hidPOId.Value);
                DataTable oDTReqsItem = oRequisitionBL.GetRequisitionsOfItem(iItemID, miSchoolId, iPOId);

                trLstReqItems.Visible = true;
                LstVwAppReqItems.DataSource = oDTReqsItem;
                LstVwAppReqItems.DataBind();
                tblAddAll.Visible = true;
                btnAddAll.Attributes.Add("onclick", "if(!AddAllReqItems(" + LstVwAppReqItems.Items.Count + ")){return false;}");
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
            }
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iItemID = Convert.ToInt32(lstvwItemsOfRequisitions.DataKeys[iRowId]["ItemID"]);
                int iReqCnt = 0;
                DataTable oDTItemsDetails;
                oDTItemsDetails = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];
                Label lblQty = e.Item.FindControl("lblQty") as Label;
                Label lblReqCnt = e.Item.FindControl("lblReqCnt") as Label;
                string sUnit = Convert.ToString(lstvwItemsOfRequisitions.DataKeys[iRowId]["ItemUnit"]);
                double dReqCount = Convert.ToDouble(lblReqCnt.Text);
                double dQty = Convert.ToDouble(lblQty.Text.Replace(sUnit, Constants.S_EMPTY_STRING).Trim());
                int iRowCount = oDTItemsDetails.Rows.Count;

                for (int iCount = iRowCount - 1; iCount >= 0; iCount--)
                {
                    //If item is from requisitions then delete all the exisiting entry of the items
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == iItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) != 0)
                    {
                        dQty += Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_PO_QUANTITY]);
                        iReqCnt++;
                        DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                        oDTRow.Delete();
                        oDTItemsDetails.AcceptChanges();
                        ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
                    }
                }
                lblQty.Text = dQty + "  " + sUnit;
                lblReqCnt.Text = Convert.ToString(dReqCount + iReqCnt);
                lnkbtnRemove.Visible = false;
                FillPOItemListView();
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view lstvwItemsOfRequisitions according to items present in the PO. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemsOfRequisitions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                string sUnit = Convert.ToString(oDataRowView["ItemUnit"]);
                Label lblQty = e.Item.FindControl("lblQty") as Label;

                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    int iItemID = Convert.ToInt32(oDataRowView["ItemID"]);

                    DataTable oDTItemsDetails;
                    oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
                    double dPOItemQty = 0;
                    int iRequisitionCount = 0;
                    double dItemQty = Convert.ToDouble(oDataRowView["ItemQty"]);
                    int iOriginalRequisitonCount = Convert.ToInt32(oDataRowView["ReqCnt"]);

                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == iItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) != 0)
                        {
                            dPOItemQty += Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_PO_QUANTITY]);
                            if (Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_DIFF]) == 0)
                                iRequisitionCount++;
                        }
                    }

                    lblQty.Text = (dItemQty - dPOItemQty).ToString();
                    Label lblReqCnt = e.Item.FindControl("lblReqCnt") as Label;
                    lblReqCnt.Text = (iOriginalRequisitonCount - iRequisitionCount).ToString();
                    //If dPOItemQty != 0 that means item is added in the PO
                    //So for removing that item from PO add one Linkbutton in the list view.
                    if (dPOItemQty != Constants.I_ZERO)
                    {
                        LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                        lnkbtnRemove.Visible = true;
                    }
                }
                lblQty.Text = lblQty.Text + " " + sUnit;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    # region RequisitionWise

    /// <summary>
    /// This event is used to fill the drop down list in the listview datapager according to pagesize.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwRquisition_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (LstVwRquisition.Items.Count > Constants.I_ZERO)
            {
                ControlUtility.FillListViewPagerFooter(LstVwRquisition, DtPgReqCnt);
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
            }
            else
                DtPgReqCnt.Visible = false;
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
    protected void ddlReqCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(LstVwRquisition);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the list view items and add sorting image according to that.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwRquisition_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to add or remove the requisition details from the PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwRquisition_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            //When command is Add that time show the requisition items list with details.
            if (e.CommandName == S_COMMAND_ADD)
            {
                tblAddAll.Visible = true;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iRequisitionID = Convert.ToInt32(LstVwRquisition.DataKeys[iRowId]["RequisitionID"]);

                RequisitionBL oRequisitionBL = new RequisitionBL();
                DataTable oDTReqsItem;
                int iPOId = Convert.ToInt32(hidPOId.Value);
                oDTReqsItem = oRequisitionBL.GetRequisitionItems(iRequisitionID, miSchoolId, iPOId);

                trLstReqItems.Visible = true;
                LstVwAppReqItems.DataSource = oDTReqsItem;
                LstVwAppReqItems.DataBind();
                btnAddAll.Attributes.Add("onclick", "if(!AddAllReqItems(" + LstVwAppReqItems.Items.Count + ")){return false;}");
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
            }
            //When command is Remove that time remove the details of that requisition from PO.
            //And hide the link button (Remove from PO).
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iRequisitionID = Convert.ToInt32(LstVwRquisition.DataKeys[iRowId]["RequisitionID"]);
                LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;

                DataTable oDTItemsDetails;
                oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
                int iRowCount = oDTItemsDetails.Rows.Count;
                for (int iCount = iRowCount - 1; iCount >= 0; iCount--)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) == iRequisitionID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) != 0)
                    {
                        DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                        oDTRow.Delete();
                        oDTItemsDetails.AcceptChanges();
                        ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
                    }
                }
                lnkbtnRemove.Visible = false;
                FillPOItemListView();
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill list view LstVwRquisition according to the requisition details present in the PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwRquisition_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = (DataRowView)oCurrentItem.DataItem;

                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    int iRequisitionID = Convert.ToInt32(oDataRowView["RequisitionID"]);

                    DataTable oDTItemsDetails;
                    oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) == iRequisitionID)
                        {
                            LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                            lnkbtnRemove.Visible = true;
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region IndividualItems

    /// <summary>
    /// This event is used to fill the drop down list in the listview datapager according to pagesize.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwIndividualItem_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (LstVwIndividualItem.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(LstVwIndividualItem, dtpgIndividual);
            else
                dtpgIndividual.Visible = false;
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
    protected void ddlIndividualCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(LstVwIndividualItem);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according LstVwIndividualItem items and requisition details present in the PO. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwIndividualItem_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                ImageButton imgbtnAdd = e.Item.FindControl("imgbtnAdd") as ImageButton;
                Label lblName = e.Item.FindControl("lblName") as Label;
                TextBox txtQuantity = e.Item.FindControl("txtQty") as TextBox;
                int iItemID = Convert.ToInt32(oDataRowView["ItemID"]);
                TextBox txtItemPrice = e.Item.FindControl("txtPrice") as TextBox;

                DropDownList cmbUnit = e.Item.FindControl("cmbUnit") as DropDownList;
                cmbUnit.Items.Add(new ListItem { Text = oDataRowView["UOMUnit"].ToString(), Value = Constants.S_ZERO });
                cmbUnit.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });

                if (oDataRowView["PieceCount"].ToInt() == Constants.I_ONE)
                {
                    cmbUnit.SelectedValue = Constants.S_ZERO;
                    cmbUnit.Enabled = false;
                }

                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    DataTable oDTItemsDetails;
                    oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        int iPOItemID = Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]);
                        int iPORequisitionID = Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]);
                        if (iPOItemID == iItemID && iPORequisitionID == Constants.I_ZERO)
                            txtQuantity.Text = Convert.ToString(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_PO_QUANTITY]);
                    }
                }
                imgbtnAdd.Attributes.Add("Onclick", "ShowHideValidation(" + txtQuantity.ClientID + ", " + lblName.ClientID + ", " + txtItemPrice.ClientID + ")");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Add items details from PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwIndividualItem_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_ADD)
            {
                trPODetails.Visible = true;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                Label lblName = e.Item.FindControl("lblName") as Label;
                Label lblCode = e.Item.FindControl("lblCode") as Label;
                Label lblUnit = e.Item.FindControl("lblUnit") as Label;
                TextBox txtQuantity = e.Item.FindControl("txtQty") as TextBox;
                DropDownList cmbUnit = e.Item.FindControl("cmbUnit") as DropDownList;
                TextBox txtItemPrice = e.Item.FindControl("txtPrice") as TextBox;


                POItemsDetailsStruct oPOItemsDetailsStruct = new POItemsDetailsStruct();
                oPOItemsDetailsStruct.miItemID = Convert.ToInt32(LstVwIndividualItem.DataKeys[iRowId]["ItemID"]);
                oPOItemsDetailsStruct.miRequisitionID = 0;
                oPOItemsDetailsStruct.mdOriginalQty = 0;
                
                oPOItemsDetailsStruct.msItemCode = lblCode.Text.Trim();
                oPOItemsDetailsStruct.msItemName = lblName.Text.Trim();
                oPOItemsDetailsStruct.msRequisitionCode = "Individual";
                oPOItemsDetailsStruct.msItemUnit = lblUnit.Text.Trim();
                //oPOItemsDetailsStruct.msItemUnit = cmbUnits.SelectedItem.Text;
                oPOItemsDetailsStruct.msUOM = cmbUnit.SelectedValue;
                oPOItemsDetailsStruct.miPieceCount = Convert.ToInt32(LstVwIndividualItem.DataKeys[iRowId]["PieceCount"]);

                double dbQuantity = Convert.ToDouble(txtQuantity.Text.Trim());
                if (oPOItemsDetailsStruct.msUOM == Constants.S_ONE)
                    oPOItemsDetailsStruct.mdPOQty = dbQuantity;
                else
                    oPOItemsDetailsStruct.mdPOQty = dbQuantity * oPOItemsDetailsStruct.miPieceCount;

                oPOItemsDetailsStruct.mdItemPrice = txtItemPrice.Text.ToDouble();

                AddItemsQtyToDataTable(oPOItemsDetailsStruct);
                FillPOItemListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region AppRequisitionItem

    /// <summary>
    /// This event is used to Add or Remove the items as well as requisition details from PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwAppReqItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            int iRequisitionID = Convert.ToInt32(LstVwAppReqItems.DataKeys[iRowId]["RequisitionID"]);
            int iItemID = Convert.ToInt32(LstVwAppReqItems.DataKeys[iRowId]["ItemID"]);
            TextBox txtQuantity = LstVwAppReqItems.Items[iRowId].FindControl("txtQty") as TextBox;
            HiddenField ohidActualQty = LstVwAppReqItems.Items[iRowId].FindControl("hidActualQty") as HiddenField;
            LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
            Label lblItemCode = e.Item.FindControl("lblItemCode") as Label;
            Label lblItemName = e.Item.FindControl("lblItemName") as Label;
            Label lblRequisitionCode = e.Item.FindControl("lblRequisitionCode") as Label;
            Label lblOrgQtyUnit = e.Item.FindControl("lblOrgQtyUnit") as Label;
            TextBox txtItemPrice = LstVwAppReqItems.Items[iRowId].FindControl("txtItemPrice") as TextBox;
            
            //Label lblUnit = e.Item.FindControl("lblUnit") as Label;
            DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;

            POItemsDetailsStruct oPOItemsDetailsStruct = new POItemsDetailsStruct();
            oPOItemsDetailsStruct.miItemID = iItemID;
            oPOItemsDetailsStruct.miRequisitionID = iRequisitionID;
            oPOItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(ohidActualQty.Value);

            oPOItemsDetailsStruct.msItemName = lblItemName.Text.Trim();
            oPOItemsDetailsStruct.msItemCode = lblItemCode.Text.Trim();
            oPOItemsDetailsStruct.msRequisitionCode = lblRequisitionCode.Text.Trim();
            oPOItemsDetailsStruct.msItemUnit = lblOrgQtyUnit.Text.Trim();
            //oPOItemsDetailsStruct.msItemUnit = cmbUnits.SelectedItem.Text;
            oPOItemsDetailsStruct.msUOM = cmbUnits.SelectedValue;
            oPOItemsDetailsStruct.miPieceCount = Convert.ToInt32(LstVwAppReqItems.DataKeys[iRowId]["PieceCount"]);

            double dbQuantity = Convert.ToDouble(txtQuantity.Text);
            if (cmbUnits.SelectedValue == Constants.S_ONE)
                oPOItemsDetailsStruct.mdPOQty = dbQuantity;
            else
                oPOItemsDetailsStruct.mdPOQty = dbQuantity * oPOItemsDetailsStruct.miPieceCount;

            oPOItemsDetailsStruct.mdItemPrice = txtItemPrice.Text.ToDouble();

            //When command is Add that time add the items details to PO and View state table.
            //And show link button (Remove From PO).
            if (e.CommandName == S_COMMAND_ADD)
            {               
                trPODetails.Visible = true;

                AddItemsQtyToDataTable(oPOItemsDetailsStruct);
                lnkbtnRemove.Visible = true;
            }
            //When command is Remove that time remove the items details to PO and View state table.
            //And hide link button (Remove From PO).
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iCount;
                DataTable oDTItemsDetails;
                oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

                for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == oPOItemsDetailsStruct.miItemID
                        && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) == oPOItemsDetailsStruct.miRequisitionID)
                        break;
                }

                DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                oDTRow.Delete();
                oDTItemsDetails.AcceptChanges();
                ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
                txtQuantity.Text = Constants.S_ZERO;
                txtItemPrice.Text = string.Empty;
                lnkbtnRemove.Visible = false;
                trPODetails.Visible = false;
            }
            FillPOItemListView();
            if (lstvwItemsOfRequisitions.Visible == true)
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according LstVwIndividualItem items and requisition details present in the PO. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwAppReqItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;

                DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;
                string sUOMName = LstVwAppReqItems.DataKeys[e.Item.DisplayIndex]["Unit"].ToString();
                cmbUnits.Items.Clear();
                cmbUnits.Items.Add(new ListItem { Text = sUOMName, Value = "0" });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = "1" });

                double dItemQty = 0;
                TextBox txtQuantity = e.Item.FindControl("txtQty") as TextBox;
                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    int iRequisitionID = Convert.ToInt32(oDataRowView["RequisitionID"]);
                    int iItemID = Convert.ToInt32(oDataRowView["ItemID"]);

                    DataTable oDTItemsDetails;
                    oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;


                    //To fill the item quantity of the item in the particular requisition from the Viewstate.
                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        int iPOItemID = Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]);
                        int iPORequisitionID = Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]);

                        if (iPOItemID == iItemID && iPORequisitionID == iRequisitionID)
                            dItemQty = Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_PO_QUANTITY]);

                        txtQuantity.Text = GetItemQuantity(dItemQty.ToDouble(), oDataRowView["PieceCount"].ToInt()).ToString();
                    }
                }
                //else
                //{
                //    txtQuantity.Text = GetItemQuantity(oDataRowView["OriginalQty"].ToDouble(), oDataRowView["PieceCount"].ToInt()).ToString();

                //    if (oDataRowView["PieceCount"].ToInt() != 1 && oDataRowView["OriginalQty"].ToInt() % oDataRowView["PieceCount"].ToInt() != 0)
                //    {
                //        cmbUnits.SelectedValue = Constants.S_ONE;
                //    }
                //    else
                //    {
                //        ListItem oListItem = cmbUnits.Items.FindByText(sUOMName);
                //        if (oListItem != null)
                //            oListItem.Selected = true;
                //    }
                //}

                if (oDataRowView["PieceCount"].ToInt() == Constants.I_ONE)
                {
                    cmbUnits.SelectedValue = Constants.S_ZERO;
                    cmbUnits.Enabled = false;
                }

                Label lblOriginalQuantity = e.Item.FindControl("lblOriginalQuantity") as Label;
                lblOriginalQuantity.Text = GetItemQuantityWithUOM(oDataRowView["OriginalQtyUnit"].ToInt(), oDataRowView["PieceCount"].ToInt(), oDataRowView["Unit"].ToString());
                //lblOriginalQuantity.Text = (lblOriginalQuantity.Text + oDataRowView["Unit"].ToString());
                //This needs for javascripts validations on item quantity.
                ImageButton imgbtnAdd = e.Item.FindControl("imgbtnAdd") as ImageButton;
                Label lblName = e.Item.FindControl("lblItemName") as Label;
                Label lblReqCode = e.Item.FindControl("lblRequisitionCode") as Label;
                HiddenField ohidActualQty = e.Item.FindControl("hidActualQty") as HiddenField;

                imgbtnAdd.Attributes.Add("Onclick", "SetValueToHiddenField('" + txtQuantity.ClientID
                                        + "', '" + ohidActualQty.ClientID + "', '" + lblName.ClientID + "' ,'" + lblReqCode.ClientID + "','" + cmbUnits.ClientID + "'," + oDataRowView["PieceCount"].ToInt() + ")");

                //Item is present in PO i.e. if item quantity is greater than zero that time only
                //show the  link button (Remove From PO).
                if (txtQuantity.Text != Constants.S_EMPTY_STRING && txtQuantity.Text != Constants.S_ZERO)
                {
                    LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                    lnkbtnRemove.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Purchase Order

    /// <summary>
    /// This event is used to display and delete details of the item of PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwPurchaseOrder_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            int iItemID = Convert.ToInt32(lstVwPurchaseOrder.DataKeys[iRowId]["ItemID"]);

            DataTable oDTItemsForPO;
            oDTItemsForPO = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

            if (e.CommandName == S_COMMAND_DETAILS)
            {
                if (oDTItemsForPO.Rows.Count > Constants.I_ZERO)
                {
                    var query = from order in oDTItemsForPO.AsEnumerable()
                                where order.Field<Int32>(S_DB_COLUMN_ITEM_ID) == iItemID
                                select order;

                    DataView view = query.AsDataView();
                    HtmlTableRow oHtmlTableRow = e.Item.FindControl("trtxtQty") as HtmlTableRow;
                    HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdtxtQty") as HtmlTableCell;
                    ListView olstVwItemDetails = oHtmlTableCell.FindControl("lstVwItemDetails") as ListView;
                    olstVwItemDetails.DataSource = view;
                    olstVwItemDetails.DataBind();
                    if (hidReadOnly.Value != Constants.S_NO && hidPOId.Value != Constants.S_ZERO)
                        oHtmlTableCell.ColSpan = Constants.I_THREE;
                    else
                        oHtmlTableCell.ColSpan = Constants.I_FOUR;

                    oHtmlTableRow.Visible = true;
                    if (txtDescription.Enabled)
                        lblStar.Visible = true;
                    else
                        lblStar.Visible = false;
                }

            }
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iRowCount = oDTItemsForPO.Rows.Count;
                for (int iCount = iRowCount - 1; iCount >= 0; iCount--)
                {
                    if (Convert.ToInt32(oDTItemsForPO.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == iItemID)
                    {
                        DataRow oDTRow = oDTItemsForPO.Rows[iCount];
                        oDTRow.Delete();
                        oDTItemsForPO.AcceptChanges();
                    }
                }
                ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsForPO;
                FillPOItemListView();
                LstVwAppReqItems.DataSource = null;
                LstVwAppReqItems.DataBind();
                trLstReqItems.Visible = false;
                tblAddAll.Visible = false;
            }
            if (lstvwItemsOfRequisitions.Visible == true)
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide details of the item of PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnHideDetails_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            oButton.Parent.Parent.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to hide/display delete column header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwPurchaseOrder_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgbtnDeleteItem = e.Item.FindControl("imgbtnDeleteItem") as ImageButton;
                Button btnHideDetails = e.Item.FindControl("btnHideDetails") as Button;
                ApplyMouseHoverEffect(new List<Button> { btnHideDetails });
                HtmlTableRow oHtmlTableHeaderRow = lstVwPurchaseOrder.FindControl("trHeader") as HtmlTableRow;
                HtmlTableCell oHtmlTableCell = oHtmlTableHeaderRow.FindControl("thDelete") as HtmlTableCell;
                HtmlTableRow oHtmlRowItem = e.Item.FindControl("trItem") as HtmlTableRow;
                HtmlTableCell oHtmlCellDeleteItem = oHtmlRowItem.FindControl("tdDeleteItem") as HtmlTableCell;
                if (hidReadOnly.Value == Constants.S_YES)
                {
                    oHtmlTableCell.Visible = false;
                    imgbtnDeleteItem.Visible = false;
                    if (oHtmlCellDeleteItem != null)
                        oHtmlCellDeleteItem.Visible = false;
                }
                else
                {
                    imgbtnDeleteItem.Visible = true;
                    oHtmlTableCell.Visible = true;
                    if (oHtmlCellDeleteItem != null)
                        oHtmlCellDeleteItem.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region lstVwItemDetails

    /// <summary>
    /// This event is used to update and delete details of the item of PO.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwItemDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            ListView olstVwItemDetails = sender as ListView;

            int iRequisitionID = Convert.ToInt32(olstVwItemDetails.DataKeys[iRowId]["RequisitionID"]);
            int iItemID = Convert.ToInt32(olstVwItemDetails.DataKeys[iRowId]["ItemID"]);
            TextBox txtQuantity = e.Item.FindControl("txtQty") as TextBox;
            HiddenField ohidActualQty = olstVwItemDetails.Items[iRowId].FindControl("hidActualQty") as HiddenField;
            Label lblItemCode = e.Item.FindControl("lblItemCode") as Label;
            Label lblItemName = e.Item.FindControl("lblItemName") as Label;
            Label lblRequisitionCode = e.Item.FindControl("lblRequisitionCode") as Label;
            Label lblUnit = e.Item.FindControl("lblUnit") as Label;
            DropDownList cmbUnits = e.Item.FindControl("cmbUnit") as DropDownList;
            TextBox txtItemPrice = e.Item.FindControl("txtItemPrice") as TextBox;

            POItemsDetailsStruct oPOItemsDetailsStruct = new POItemsDetailsStruct();
            oPOItemsDetailsStruct.miItemID = iItemID;
            oPOItemsDetailsStruct.miRequisitionID = iRequisitionID;
            oPOItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(ohidActualQty.Value);
            
            oPOItemsDetailsStruct.msItemName = lblItemName.Text.Trim();
            oPOItemsDetailsStruct.msItemCode = lblItemCode.Text.Trim();
            oPOItemsDetailsStruct.msRequisitionCode = lblRequisitionCode.Text.Trim();
            oPOItemsDetailsStruct.msItemUnit = lblUnit.Text.Trim();
            //oPOItemsDetailsStruct.msItemUnit = cmbUnits.SelectedItem.Text;
            oPOItemsDetailsStruct.msUOM = cmbUnits.SelectedValue;
            oPOItemsDetailsStruct.miPieceCount = Convert.ToInt32(olstVwItemDetails.DataKeys[iRowId]["UOMUnitCount"]);
            oPOItemsDetailsStruct.mdItemPrice = txtItemPrice.Text.ToDouble();

            if (txtQuantity.Text.Trim() != ".")
            {
                double dbQuantity = Convert.ToDouble(txtQuantity.Text);;
                if (oPOItemsDetailsStruct.msUOM == Constants.S_ONE)
                    oPOItemsDetailsStruct.mdPOQty = dbQuantity;
                else
                    oPOItemsDetailsStruct.mdPOQty = dbQuantity * oPOItemsDetailsStruct.miPieceCount;
            }

            if (e.CommandName == "ModifyItem")
                AddItemsQtyToDataTable(oPOItemsDetailsStruct);
            else if (e.CommandName == "RemoveItem")
            {
                int iCount;
                DataTable oDTItemsDetails;
                oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

                for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == oPOItemsDetailsStruct.miItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) == oPOItemsDetailsStruct.miRequisitionID)
                        break;
                }

                DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                oDTRow.Delete();
                oDTItemsDetails.AcceptChanges();
                ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
            }

            DataTable oDTItemsForPO = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];
            if (oDTItemsForPO.Rows.Count > Constants.I_ZERO)
            {
                var query = from order in oDTItemsForPO.AsEnumerable()
                            where order.Field<Int32>(S_DB_COLUMN_ITEM_ID) == iItemID
                            select order;

                DataView view = query.AsDataView();
                olstVwItemDetails.DataSource = view;
                olstVwItemDetails.DataBind();

                if (view.Count > Constants.I_ZERO)
                    olstVwItemDetails.Parent.Parent.Visible = true;
                else
                {
                    olstVwItemDetails.Parent.Parent.Parent.Visible = false;
                }

            }
            else
            {
                olstVwItemDetails.Parent.Parent.Visible = false;
                FillPOItemListView();
            }
            // This part used to remove requisition items details list view. 
            LstVwAppReqItems.DataSource = null;
            LstVwAppReqItems.DataBind();
            trLstReqItems.Visible = false;
            tblAddAll.Visible = false;
            SetQuantityForItem(iItemID);
            if (lstvwItemsOfRequisitions.Visible == true)
                AddSortImage(lstvwItemsOfRequisitions, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(LstVwRquisition, S_DEFAULT_SORT_EXP_REQUISITION);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide/display delete linkbutton.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwItemDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;

                TextBox txtQuantity = e.Item.FindControl("txtQty") as TextBox;
                LinkButton lnkbtnUpdate = e.Item.FindControl("lnkbtnUpdate") as LinkButton;
                LinkButton lnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                Label lblName = e.Item.FindControl("lblItemName") as Label;
                Label lblReqCode = e.Item.FindControl("lblRequisitionCode") as Label;
                HiddenField ohidActualQty = e.Item.FindControl("hidActualQty") as HiddenField;
                TextBox txtItemPrice = e.Item.FindControl("txtItemPrice") as TextBox;
                
                DropDownList cmbUnits = e.Item.FindControl("cmbUnit") as DropDownList;
                cmbUnits.Items.Add(new ListItem { Text = oDataRowView["Unit"].ToString(), Value = Constants.S_ZERO });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });

                cmbUnits.SelectedValue = oDataRowView["UOMUnits"].ToString();

                lnkbtnUpdate.Attributes.Add("Onclick", "SetValueToHiddenField('" + txtQuantity.ClientID + "', '" + ohidActualQty.ClientID + "', '" + lblName.ClientID + "' ,'" + lblReqCode.ClientID + "','" + cmbUnits.ClientID + "'," + oDataRowView["UOMUnitCount"].ToInt() + ")");

                decimal iQuantity = txtQuantity.Text.ToDecimal();
                int iUOMUnitCount = oDataRowView["UOMUnitCount"].ToInt();

                if(oDataRowView["UOMUnits"].ToString() == Constants.S_ZERO)
                {
                    if (iQuantity % iUOMUnitCount == 0)
                        txtQuantity.Text = (iQuantity / iUOMUnitCount).ToString();
                    else
                    {
                        txtQuantity.Text = iQuantity.ToString();
                        cmbUnits.SelectedValue = Constants.S_ONE;
                    }
                }

                if (hidReadOnly.Value == Constants.S_YES)
                {
                    lnkbtnUpdate.Enabled = false;
                    lnkbtnRemove.Enabled = false;
                    txtQuantity.Enabled = false;                    
                    cmbUnits.Enabled = false;
                    txtItemPrice.Enabled = false;
                }
                else
                {
                    lnkbtnUpdate.Enabled = true;
                    lnkbtnRemove.Enabled = true;
                    txtQuantity.Enabled = true;
                    cmbUnits.Enabled = true;
                    txtItemPrice.Enabled = true;
                }

                if (iUOMUnitCount == 1)
                {
                    txtQuantity.Text = iQuantity.ToString();
                    cmbUnits.SelectedValue = Constants.S_ZERO;
                    cmbUnits.Enabled = false;
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

    #endregion

    #endregion

    #region Private Methods

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

    ///// <summary>
    ///// This method is used to create the datarow and bind that row to the gridview
    ///// </summary>
    //private void AddItemsQtyToDataTable()
    //{
    //    bool bIsNew = true;
    //    int iCount;
    //    DataTable oDTItemsDetails;
    //    if (ViewState[S_LISTVIEW_DATASOURCE] == null)
    //        oDTItemsDetails = CreateItemsQtyTable();
    //    else
    //        oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

    //    for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
    //    {
    //        if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == miItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) == miRequisitionID)
    //        {
    //            bIsNew = false;
    //            break;
    //        }
    //    }

    //    if (bIsNew)
    //        oDTItemsDetails.Rows.Add(AddRequisitionItemsQtyToDataRow(oDTItemsDetails.NewRow()));
    //    else
    //    {
    //        DataRow oDTRow = oDTItemsDetails.Rows[iCount];
    //        oDTRow.BeginEdit();
    //        oDTRow[S_DB_COLUMN_ITEM_PO_QUANTITY] = mdPOQty;
    //        if (mdOriginalQty != 0)
    //            oDTRow[S_DB_COLUMN_ITEM_DIFF] = (mdOriginalQty - mdPOQty);
    //        else
    //            oDTRow[S_DB_COLUMN_ITEM_DIFF] = 0;
    //        oDTItemsDetails.AcceptChanges();
    //        oDTItemsDetails.Rows[iCount].EndEdit();
    //    }
    //    ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
    //}

    /// <summary>
    /// This method is used to create the datatable and bind that datatable to view state.
    /// </summary>
    private void AddItemsQtyToDataTable(POItemsDetailsStruct oPOItemsDetailsStruct)
    {
        bool bIsNew = true;
        int iCount;
        DataTable oDTItemsDetails;
        if (ViewState[S_LISTVIEW_DATASOURCE] == null)
            oDTItemsDetails = CreateItemsQtyTable();
        else
            oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

        //For loop for checking either item added in the PO is New or existing
        for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
        {
            //If item is not new that time set flag bIsNew to the false.
            if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == oPOItemsDetailsStruct.miItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) == oPOItemsDetailsStruct.miRequisitionID)
            {
                bIsNew = false;
                break;
            }
        }

        //If item is new that time add new row for that item.
        //Else Modify the existing item quantity.
        if (bIsNew)
            oDTItemsDetails.Rows.Add(AddRequisitionItemsQtyToDataRow(oDTItemsDetails.NewRow(), oPOItemsDetailsStruct));
        else
        {
            DataRow oDTRow = oDTItemsDetails.Rows[iCount];
            oDTRow.BeginEdit();
            oDTRow[S_DB_COLUMN_ITEM_PO_QUANTITY] = oPOItemsDetailsStruct.mdPOQty;
            oDTRow[S_DB_COLUMN_ITEM_PO_PRICE] = oPOItemsDetailsStruct.mdItemPrice;
            if (mdOriginalQty != Constants.I_ZERO)
                oDTRow[S_DB_COLUMN_ITEM_DIFF] = (oPOItemsDetailsStruct.mdOriginalQty - oPOItemsDetailsStruct.mdPOQty);
            else
                oDTRow[S_DB_COLUMN_ITEM_DIFF] = 0;
            oDTItemsDetails.AcceptChanges();
            oDTItemsDetails.Rows[iCount].EndEdit();
        }
        ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
    }

    /// <summary>
    /// This method is used to create new datatable
    /// </summary>
    /// <returns></returns>
    private DataTable CreateItemsQtyTable()
    {
        // Create a new DataTable for requisition items details. 
        DataTable oDTItemsDetails = new DataTable();

        // Add columns to the Item table.        
        AddDataColumnToItemQtyTable("System.Int32", S_DB_COLUMN_ITEM_ID, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Int32", S_DB_COLUMN_REQUISITION_ID, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_PO_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_DIFF, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_ITEM_NAME, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_ITEM_CODE, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_REQUISITION_CODE, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_ITEM_UNIT, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_UOM_UNITS, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_UOM_UNIT_COUNT, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_ITEM_PO_PRICE, ref oDTItemsDetails, false);

        return oDTItemsDetails;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemQtyTable(string asDataType, string asColumnName, ref DataTable aoDataTable, bool abIsPrimaryKey)
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
    //private DataRow AddRequisitionItemsQtyToDataRow(DataRow oDR)
    //{
    //    DataRow oDRItem;

    //    oDRItem = oDR;
    //    // Then add the new row to the collection.
    //    oDRItem[S_DB_COLUMN_ITEM_ID] = miItemID;
    //    oDRItem[S_DB_COLUMN_REQUISITION_ID] = miRequisitionID;
    //    oDRItem[S_DB_COLUMN_ITEM_PO_QUANTITY] = mdPOQty;
    //    oDRItem[S_DB_COLUMN_ITEM_QUANTITY] = mdOriginalQty;
    //    if (mdOriginalQty != Constants.I_ZERO)
    //        oDRItem[S_DB_COLUMN_ITEM_DIFF] = mdOriginalQty - mdPOQty;
    //    else
    //        oDRItem[S_DB_COLUMN_ITEM_DIFF] = 0;
    //    oDRItem[S_DB_COLUMN_ITEM_NAME] = msItemName;
    //    oDRItem[S_DB_COLUMN_ITEM_CODE] = msItemCode;
    //    oDRItem[S_DB_COLUMN_REQUISITION_CODE] = msRequisitionCode;
    //    oDRItem[S_ITEM_UNIT] = msItemUnit;
    //    return oDRItem;
    //}

    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow AddRequisitionItemsQtyToDataRow(DataRow oDR, POItemsDetailsStruct oPOItemsDetailsStruct)
    {
        DataRow oDRItem;

        oDRItem = oDR;
        // Then add the new row to the collection.
        oDRItem[S_DB_COLUMN_ITEM_ID] = oPOItemsDetailsStruct.miItemID;
        oDRItem[S_DB_COLUMN_REQUISITION_ID] = oPOItemsDetailsStruct.miRequisitionID;
        oDRItem[S_DB_COLUMN_ITEM_PO_QUANTITY] = oPOItemsDetailsStruct.mdPOQty;
        oDRItem[S_DB_COLUMN_ITEM_QUANTITY] = oPOItemsDetailsStruct.mdOriginalQty;
        if (mdOriginalQty != Constants.I_ZERO)
            oDRItem[S_DB_COLUMN_ITEM_DIFF] = oPOItemsDetailsStruct.mdOriginalQty - oPOItemsDetailsStruct.mdPOQty;
        else
            oDRItem[S_DB_COLUMN_ITEM_DIFF] = 0;
        oDRItem[S_DB_COLUMN_ITEM_NAME] = oPOItemsDetailsStruct.msItemName;
        oDRItem[S_DB_COLUMN_ITEM_CODE] = oPOItemsDetailsStruct.msItemCode;
        oDRItem[S_DB_COLUMN_REQUISITION_CODE] = oPOItemsDetailsStruct.msRequisitionCode;
        oDRItem[S_ITEM_UNIT] = oPOItemsDetailsStruct.msItemUnit;
        oDRItem[S_UOM_UNITS] = oPOItemsDetailsStruct.msUOM;
        oDRItem[S_UOM_UNIT_COUNT] = oPOItemsDetailsStruct.miPieceCount;
        oDRItem[S_DB_COLUMN_ITEM_PO_PRICE] = oPOItemsDetailsStruct.mdItemPrice;

        return oDRItem;
    }

    /// <summary>
    /// This method is used to fill the list view lstVwPurchaseOrder.
    /// </summary>
    private void FillPOItemListView()
    {
        DataTable oDTItemsForPO;
        oDTItemsForPO = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];

        //In this LINQ we take sum of quantity of same item from different requisition as well as Individual.
        //Add bind this LINQ to the List view (lstVwPurchaseOrder).
        if (oDTItemsForPO != null)
        {
            trPODetails.Visible = true;
            var query = from POItem in oDTItemsForPO.AsEnumerable()
                        group POItem by new { Id = POItem.Field<Int32>("ItemID"), Code = POItem.Field<string>(S_DB_COLUMN_ITEM_CODE), Name = POItem.Field<string>(S_DB_COLUMN_ITEM_NAME), Unit = POItem.Field<string>(S_ITEM_UNIT), UOMUnitCount = POItem.Field<string>("UOMUnitCount"), ItemPrice = POItem.Field<string>("ItemPrice") }
                            into POItemGroup
                            select new
                            {
                                ItemId = POItemGroup.Key.Id,
                                ItemCode = POItemGroup.Key.Code,
                                ItemName = POItemGroup.Key.Name,
                                Unit = POItemGroup.Key.Unit,
                                Qty =POItemGroup.Sum(POItem => POItem.Field<double>(S_DB_COLUMN_ITEM_PO_QUANTITY)),
                                ItemPrice = POItemGroup.Key.ItemPrice,
                                QuantityWithUnit = GetItemQuantityWithUOM(POItemGroup.Sum(POItem => POItem.Field<double>(S_DB_COLUMN_ITEM_PO_QUANTITY)), POItemGroup.Key.UOMUnitCount.ToInt(), POItemGroup.Key.Unit)
                            };

            lstVwPurchaseOrder.DataSource = query;
            lstVwPurchaseOrder.DataBind();
            hidPOItemCount.Value = lstVwPurchaseOrder.Items.Count.ToString();

            SetSaveButton(oDTItemsForPO);
        }
    }

    private double GetItemQuantity(double adcQuantity, int aiUOMUnitCount)
    {
        double dcQuantity = adcQuantity;
        if (aiUOMUnitCount > 1)
        {
            if (dcQuantity % aiUOMUnitCount == 0)
                dcQuantity = Math.Round(dcQuantity / aiUOMUnitCount, 2);
        }

        return dcQuantity;
    }

    private string GetItemQuantityWithUOM(double dcQuantity, int aiUOMUnitCount, string asUnitName)
    {
        string sQuantity = string.Empty;
        if (aiUOMUnitCount > 1)
        {
            if (dcQuantity % aiUOMUnitCount == 0)
                sQuantity = Math.Round(dcQuantity / aiUOMUnitCount, 2) + " " + asUnitName + " / " + dcQuantity + " " + Constants.S_UNITS;
            else
                sQuantity = dcQuantity + " " + Constants.S_UNITS;
        }
        else
            sQuantity = (dcQuantity)+ " " + asUnitName;


        return sQuantity;
    }
    
    /// <summary>
    /// This method is used to show the save button according to list view lstVwPurchaseOrder
    /// and view state datatable
    /// </summary>
    private void SetSaveButton(DataTable oDTItemsForPO)
    {
        if (oDTItemsForPO.Rows.Count > Constants.I_ZERO)
        {
            lstVwPurchaseOrder.Visible = true;
            trDesc.Visible = true;
            btnSave.Visible = true;
            btnSave.Text = S_SAVE;
            if (hidIsFromApproverSCreen.Value == Constants.S_NO)
            {
                btnSubmit.Visible = true;
                btnSubmit.Enabled = false;
                if (hidPOId.Value != Constants.S_ZERO)
                    btnSubmit.Enabled = true;
            }
            else
                btnSubmit.Visible = false;
        }
        else
        {
            lstVwPurchaseOrder.Visible = false;
            if (hidPOId.Value != Constants.S_ZERO)
                btnSave.Text = S_DELETE;
            else
            {
                trDesc.Visible = false;
                btnSave.Visible = false;
                btnSubmit.Visible = false;
            }
        }
    }

    /// <summary>
    /// Generate XML for the Items.
    /// </summary>
    /// <returns></returns>
    private string GeneratePORequisitionItemXML()
    {
        DataTable oDTPOReqItems = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        DataRow[] oDataRow = oDTPOReqItems.Select(S_DB_COLUMN_REQUISITION_ID + "<> 0");
        // Create a root level element.
        // Only if there is atleast one item from requisition in the PO.
        if (oDataRow.Length > Constants.I_ZERO)
        {
            XmlElement root = oDoc.CreateElement("POReqItems");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "POReqItems", "");
            for (int iCount = 0; iCount < oDTPOReqItems.Rows.Count; iCount++)
            {
                if (Convert.ToInt32(oDTPOReqItems.Rows[iCount][S_DB_COLUMN_REQUISITION_ID]) != Constants.I_ZERO)
                {
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "POReqItems", "");

                    string sAtrrName = "RequisitionId";
                    XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_REQUISITION_ID].ToString();

                    oXmlNode.Attributes.Append(attr);


                    sAtrrName = "ItemID";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_ITEM_ID].ToString();

                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ItemQty";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_ITEM_PO_QUANTITY].ToString();                   

                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ItemUnit";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_ITEM_UNIT].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "UOMUnitCount";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_UOM_UNIT_COUNT].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "UOMUnit";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_UOM_UNITS].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ItemPrice";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_ITEM_PO_PRICE].ToString();
                    oXmlNode.Attributes.Append(attr);

                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }
        else
            return null;
    }

    /// <summary>
    /// Generate XML for the Items.
    /// </summary>
    /// <returns></returns>
    private string GeneratePOItemXML()
    {
        string sPOName = Constants.S_EMPTY_STRING;
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("POItemsDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "POItemsDetails", "");

        DataTable oDTPOReqItems = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

        // Loop through all the grid rows.
        foreach (ListViewDataItem oListViewDataItem in lstVwPurchaseOrder.Items)
        {
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "POItemsDetails", "");

            int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
            
            //Label oItemQty = oListViewDataItem.FindControl("lblQty") as Label;
            HiddenField hidUnitQty = oListViewDataItem.FindControl("hidUnitQty") as HiddenField;
            HiddenField hidItemPrice = oListViewDataItem.FindControl("hidItemPrice") as HiddenField;

            Label lblItemName = oListViewDataItem.FindControl("lblName") as Label;

            //DropDownList cmbUnit = lstVwPurchaseOrder.FindControl("cmbUnit") as DropDownList;

            string sAtrrName = "ItemID";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = (lstVwPurchaseOrder.DataKeys[iRowId]["ItemID"]).ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "UOM";
            attr = oDoc.CreateAttribute(sAtrrName);
            DataRow[] dr = oDTPOReqItems.Select("ItemId=" + lstVwPurchaseOrder.DataKeys[iRowId]["ItemID"].ToString());
            if (dr.Length > 0)
                attr.Value = dr[0]["UOMUnits"].ToString();
            else
                attr.Value = Constants.S_ZERO;

            oXmlNode.Attributes.Append(attr);
          
            sAtrrName = "ItemQty";
            attr = oDoc.CreateAttribute(sAtrrName);
            //attr.Value = oItemQty.Text.Trim();
            attr.Value = hidUnitQty.Value;
            oXmlNode.Attributes.Append(attr);
           
            sAtrrName = "ItemPrice";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = hidItemPrice.Value;
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

            sPOName += lblItemName.Text + ",";
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        //Create Purchase order name.
        sPOName = sPOName.Remove(sPOName.LastIndexOf(","));
        if (sPOName.Length > I_PO_NAME_LENGTH)
            sPOName = sPOName.Substring(Constants.I_ZERO, I_PO_NAME_LENGTH);
        hidPOName.Value = sPOName;

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["POID"] != null)
            hidPOId.Value = QueryString["POID"];

        if (QueryString["CanModify"] != null)
            hidCanModify.Value = QueryString["CanModify"];

        if (QueryString["StatusId"] != null)
            hidPOStatusId.Value = QueryString["StatusId"];

        if (QueryString["IsFromApproverScreen"] != null)
            hidIsFromApproverSCreen.Value = QueryString["IsFromApproverScreen"];
    }

    /// <summary>
    /// This method is used set the form in the view mode. 
    /// </summary>
    private void SetFormInViewMode()
    {
        hidReadOnly.Value = Constants.S_YES;
        int iPOId = Convert.ToInt32(hidPOId.Value);
        PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
        //Dataset contains all the information of PO 
        DataSet oDSPOItemsDetails = oPurchaseOrderBL.GetPOItemsDetails(iPOId, miSchoolId, miUserId);

        //Datatable[0] contains all the items details of PO
        DataTable oDTPOItems = oDSPOItemsDetails.Tables[0];
        //Datatable[1] contains Description of PO
        txtDescription.Text = oDSPOItemsDetails.Tables[1].Rows[0]["PurchaseOrderDesc"].ToString();

        tblSearch.Visible = true;
        trSearch.Visible = false;
        trPODetails.Visible = true;        
        
        int iOrderType = oDSPOItemsDetails.Tables[1].Rows[0]["OrderType"].ToInt();

        if(iOrderType == Constants.I_ONE)
            rdoPurchase.Checked = true;
        else
            rdoWork.Checked = true;

        rdoPurchase.Enabled = false;
        rdoWork.Enabled = false;

        cmbVendors.SelectedValue = oDSPOItemsDetails.Tables[1].Rows[0]["VendorId"].ToString();
        cmbVendors.Enabled = false;

        cmbHeader.SelectedValue = oDSPOItemsDetails.Tables[1].Rows[0]["HeaderId"].ToString();
        cmbHeader.Enabled = false;

        int iIsPoSubmitted = oDSPOItemsDetails.Tables[1].Rows[0]["StatusId"].ToInt();
        if (iIsPoSubmitted == Constants.I_TWO)
            btnModify.Visible = false;

        txtPODeliveryDate.Text = oDSPOItemsDetails.Tables[1].Rows[0]["ExpectedDeliveryDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
        txtPONote.Text = oDSPOItemsDetails.Tables[1].Rows[0]["Note"].ToString();
        txtAmountDiscount.Text = oDSPOItemsDetails.Tables[1].Rows[0]["Discount"].ToString();

        cal_PODeliveryDate.Enabled = false;
        txtPONote.Enabled = false;
        txtAmountDiscount.Enabled = false;

        for (int iCount = 0; iCount < oDTPOItems.Rows.Count; iCount++)
        {
            POItemsDetailsStruct oPOItemsDetailsStruct = new POItemsDetailsStruct();
            oPOItemsDetailsStruct.miItemID = Convert.ToInt32(oDTPOItems.Rows[iCount]["ItemID"]);
            oPOItemsDetailsStruct.miRequisitionID = Convert.ToInt32(oDTPOItems.Rows[iCount]["RequisitionID"]);
            oPOItemsDetailsStruct.msItemCode = Convert.ToString(oDTPOItems.Rows[iCount]["ItemCode"]);
            oPOItemsDetailsStruct.msItemName = Convert.ToString(oDTPOItems.Rows[iCount]["ItemName"]);
            oPOItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(oDTPOItems.Rows[iCount]["ItemQty"]);
            oPOItemsDetailsStruct.mdItemPrice = Convert.ToDouble(oDTPOItems.Rows[iCount]["ItemPrice"]);
            
            if (Convert.ToString(oDTPOItems.Rows[iCount]["RequisitionCode"]) != "0")
                oPOItemsDetailsStruct.msRequisitionCode = Convert.ToString(oDTPOItems.Rows[iCount]["RequisitionCode"]);
            else
                oPOItemsDetailsStruct.msRequisitionCode = "Individual";

            oPOItemsDetailsStruct.msItemUnit = Convert.ToString(oDTPOItems.Rows[iCount]["UOMUnit"]);
            oPOItemsDetailsStruct.msUOM = Convert.ToString(oDTPOItems.Rows[iCount]["ConsiderUnitQuantity"].ToInt());
            oPOItemsDetailsStruct.miPieceCount = Convert.ToInt32(oDTPOItems.Rows[iCount]["PieceCount"]);

            double dbQuantity = Convert.ToDouble(oDTPOItems.Rows[iCount]["POQty"]); ;
            //if (oPOItemsDetailsStruct.msUOM == Constants.S_ONE)
                oPOItemsDetailsStruct.mdPOQty = dbQuantity;
            //else
            //    oPOItemsDetailsStruct.mdPOQty = dbQuantity * oPOItemsDetailsStruct.miPieceCount;

            AddItemsQtyToDataTable(oPOItemsDetailsStruct);
        }

        FillPOItemListView();
        tblAddAll.Visible = false;
        btnCancel.Visible = false;
        tblBasic.Visible = false;
        if (Convert.ToBoolean(hidCanModify.Value))
            tblModify.Visible = true;
        else
            tblModify.Visible = false;
        btnSave.Visible = false;
        btnSubmit.Visible = false;
        trLstReqItems.Visible = false;
        tblItems.Visible = false;
        txtDescription.Enabled = false;
        lblStar.Visible = false;

    }

    /// <summary>
    /// This method is used set the form in New mode.
    /// </summary>
    private void SetFormInNewMode()
    {
        tblBasic.Visible = true;
        optItemWise.Checked = true;
        lstvwItemsOfRequisitions.DataSourceID = lstDSobj.ID;

        tblItems.Visible = true;
        tblReqItems.Visible = false;
        tblSearch.Visible = false;
        trLstReqItems.Visible = false;
        tblAddAll.Visible = false;
        btnCancel.Visible = false;
        btnSave.Visible = false;
        btnSubmit.Visible = false;
        tblModify.Visible = false;
    }

    /// <summary>
    /// This method is used initialize the form.
    /// </summary>
    private void InitializeForm()
    {
        //If Form in the new mode that time only value of hidPOId = 0.
        //Otherwise the value of the hidPOId is the existing PO.
        FillComboboxes();
        if (hidPOId.Value != Constants.S_ZERO)
            SetFormInViewMode();
        else
            SetFormInNewMode();
    }

    /// <summary>
    /// This method is used set the validation summary header.
    /// </summary>
    private void SetValSummaryHeaderAndAttributes()
    {
        valSearch.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valReqQty.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ValPOSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valAddAll.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnAddAll, btnBack, btnModify, btnSearch, btnSubmit });
        base.SetDefaultButton(btnSearch);

        string sQueryString = "PoId=" + hidPOId.Value + "&StatusId=" + hidPOStatusId.Value + "&IsFromApproverScreen=" + hidIsFromApproverSCreen.Value;
        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
        btnBack.Attributes.Add("onclick", "if(!RedirectToPage()) return false;");

        if (hidIsFromApproverSCreen.Value == Constants.S_YES)
        {
            trPOTypes.Visible = false;
            btnSubmit.Visible = false;
            lstvwItemsOfRequisitions.Visible = false;
            if (hidPOStatusId.Value == Constants.S_ONE)
                btnModify.Visible = false;                
        }

        txtPODeliveryDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used set quantity of the item in the list view lstVwPurchaseOrder.
    /// </summary>
    private void SetQuantityForItem(int aiItemID)
    {
        DataTable oDTItemsForPO = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

        if (oDTItemsForPO.Rows.Count > Constants.I_ZERO)
        {
            foreach (ListViewDataItem oListViewDataItem in lstVwPurchaseOrder.Items)
            {
                int iRowIndex = Convert.ToInt32(oListViewDataItem.DataItemIndex);
                Label oItemQty = oListViewDataItem.FindControl("lblQty") as Label;
                HiddenField hidItemQty = oListViewDataItem.FindControl("hidItemQty") as HiddenField;

                if (Convert.ToDouble(lstVwPurchaseOrder.DataKeys[iRowIndex]["ItemID"]) == aiItemID)
                {
                    var query = from POItem in oDTItemsForPO.AsEnumerable()
                                where POItem.Field<Int32>("ItemID") == aiItemID
                                group POItem by new { ItemId = POItem.Field<Int32>("ItemID"), UOMUnitCount = POItem.Field<string>("UOMUnitCount"), Unit = POItem.Field<string>("Unit") }
                                    into POItemGroup
                                    select new
                                    {
                                        ItemId = POItemGroup.Key,
                                        Qty = POItemGroup.Sum(POItem => POItem.Field<double>(S_DB_COLUMN_ITEM_PO_QUANTITY)),
                                        UOMUnitCount = POItemGroup.Key.UOMUnitCount,
                                        Unit = POItemGroup.Key.Unit
                                    };

                    foreach (var order in query)
                        oItemQty.Text = GetItemQuantityWithUOM((order.Qty),order.UOMUnitCount.ToInt(), order.Unit).ToString();
                    break;
                }
            }
        }
    }

    /// <summary>
    /// This method is used to add sort image in list view column header.
    /// </summary>
    /// <param name="aoListView"></param>
    /// <param name="asSortExpression"></param>
    private void AddSortImage(ListView aoListView, string asSortExpression)
    {
        if (aoListView.SortDirection.ToString() == "Ascending" || aoListView.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (aoListView.SortExpression != string.Empty)
            hidSortExpression.Value = aoListView.SortExpression.ToString();
        else
            hidSortExpression.Value = asSortExpression;
        HtmlTableRow oHtmlTableHeaderRow = aoListView.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to fill the all combo boxes.
    /// </summary>
    private void FillComboboxes()
    {
        VendorDetailsBL oVendorDetailsBL = new VendorDetailsBL(miSchoolId, miUserId);
        DataSet dtsMasterDetails = oVendorDetailsBL.GetAllVendorsForCombo();
        ControlUtility.FillDropDownList(dtsMasterDetails.Tables[0], ref cmbVendors, "Id","CompanyName", Constants.S_SELECT);
        ControlUtility.FillDropDownList(dtsMasterDetails.Tables[1], ref cmbHeader, "AccountHeaderId", "AccountHeaderName", Constants.S_SELECT);

        rdoPurchase.Checked = true;
    }   

    #endregion    
}
